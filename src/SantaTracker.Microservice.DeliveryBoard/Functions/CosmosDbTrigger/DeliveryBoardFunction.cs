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

namespace SantaTracker.Microservice.DeliveryBoard.Functions.CosmosDbTrigger
{
    public class DeliveryBoardFunction
    { 
        private CosmosClient _cosmosClient;
        private readonly Dictionary<string, DateTime> _timestamps = new Dictionary<string, DateTime>();
        private readonly object _threadLock = new object();

        public DeliveryBoardFunction(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        [FunctionName(nameof(DeliveryBoardFunction))]
        public async Task DeliveryBoardAsync([CosmosDBTrigger(
                    databaseName: Constants.CosmosDb.DatabaseName,
                    collectionName: Constants.CosmosDb.LocationContainerName,
                    ConnectionStringSetting = Constants.CosmosDb.Connection,
                    LeaseCollectionName = Constants.CosmosDb.LeaseContainerName,
                    LeaseCollectionPrefix = "DeliveryBoard-",
                    CreateLeaseCollectionIfNotExists = true)]
                IReadOnlyList<Document> input,
            ILogger log)
        {
            var container = _cosmosClient.GetContainer(Constants.CosmosDb.DatabaseName, Constants.CosmosDb.DeliveryBoardContainerName);

            foreach (var document in input)
            {
                try
                {
                    await UpdateDeliveryBoardAsync(container, document, log);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message);
                }
            }
        }

        private async Task UpdateDeliveryBoardAsync(Container container, Document document, ILogger log)
        {
            var delivery = JsonConvert.DeserializeObject<CityDelivery>(document.ToString());

            if (ShouldSkip(delivery))
            {
                return;
            }

            // Swap the GUID in the document's ID with the arrival city code to enable point reads
            delivery.Id = delivery.ArrivalCity;

            // Upsert to the currentLocation container
            var result = await container.UpsertItemAsync(delivery, new Microsoft.Azure.Cosmos.PartitionKey("location"));

            log.LogWarning($"Upserted location event to materialized view for flight segment {delivery.ArrivalCity} ({result.RequestCharge} RUs)");
        }

        private bool ShouldSkip(CityDelivery delivery)
        {
            if (delivery.IsComplete)   // Make sure not to miss the last location event
            {
                return false;
            }
            // Throttle continuous processing by delaying between updates of the same city to the DeliveryBoard container
            lock (_threadLock)
            {
                if (_timestamps.ContainsKey(delivery.RouteNumber))
                {
                    if (DateTime.Now.Subtract(_timestamps[delivery.RouteNumber]).TotalSeconds < 5)
                    {
                        return true;
                    }
                    else
                    {
                        _timestamps[delivery.RouteNumber] = DateTime.Now;
                    }
                }
                else
                {
                    _timestamps.Add(delivery.RouteNumber, DateTime.Now);
                }
            }
            return false;
        }
    }
}
