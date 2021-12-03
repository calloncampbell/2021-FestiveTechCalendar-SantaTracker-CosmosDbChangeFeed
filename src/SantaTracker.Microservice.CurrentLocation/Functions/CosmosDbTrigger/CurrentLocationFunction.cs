using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using SantaTracker.Shared.Constants;
using Microsoft.Azure.Cosmos;
using SantaTracker.Shared.Models;

namespace SantaTracker.Microservice.CurrentLocation.Functions
{
    public class CurrentLocationFunction
    {
        private CosmosClient _cosmosClient;
        private readonly Dictionary<string, DateTime> _timestamps = new Dictionary<string, DateTime>();
        private readonly object _threadLock = new object();

        public CurrentLocationFunction(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }
            
        [FunctionName(nameof(CurrentLocationFunction))]
        public async Task CurrentLocationAsync([CosmosDBTrigger(
                databaseName: Constants.CosmosDb.DatabaseName,
                collectionName: Constants.CosmosDb.LocationContainerName,
                ConnectionStringSetting = Constants.CosmosDb.Connection,
                LeaseCollectionName = Constants.CosmosDb.LeaseContainerName,                
                LeaseCollectionPrefix = "CurrentLocation-",
                CreateLeaseCollectionIfNotExists = true)]
            IReadOnlyList<Document> input,
            ILogger log)
        {
            var container = _cosmosClient.GetContainer(Constants.CosmosDb.DatabaseName, Constants.CosmosDb.CurrentLocationContainerName);

            foreach (var document in input)
            {
                try
                {
                    await UpdateCurrentLocationAsync(container, document, log);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message);
                }
            }
        }

        private async Task UpdateCurrentLocationAsync(Container container, Document document, ILogger log)
        {
            var locationEvent = JsonConvert.DeserializeObject<LocationEvent>(document.ToString());

            if (ShouldSkip(locationEvent))
            {
                return;
            }

            // Swap the GUID in the document's ID with the flight number to enable point reads
            locationEvent.Id = locationEvent.FlightNumber;

            // Upsert to the currentLocation container
            var result = await container.UpsertItemAsync(locationEvent, new Microsoft.Azure.Cosmos.PartitionKey("location"));

            log.LogWarning($"Upserted location event to materialized view for flight segment {locationEvent.RouteNumber} ({result.RequestCharge} RUs)");
        }

        private bool ShouldSkip(LocationEvent locationEvent)
        {
            // Make sure not to miss the last location event
            if (locationEvent.IsComplete)   
            {
                return false;
            }

            // Throttle continuous processing by delaying between updates of the same Flight Number to the CurrentLocation container
            lock (_threadLock)
            {
                if (_timestamps.ContainsKey(locationEvent.RouteNumber))
                {
                    if (DateTime.Now.Subtract(_timestamps[locationEvent.RouteNumber]).TotalSeconds < 5)
                    {
                        return true;
                    }
                    else
                    {
                        _timestamps[locationEvent.RouteNumber] = DateTime.Now;
                    }
                }
                else
                {
                    _timestamps.Add(locationEvent.RouteNumber, DateTime.Now);
                }
            }
            return false;
        }
    }
}
