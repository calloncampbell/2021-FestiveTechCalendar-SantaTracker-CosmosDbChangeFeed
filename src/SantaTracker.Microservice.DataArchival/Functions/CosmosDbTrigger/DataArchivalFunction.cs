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
using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using System.IO;
using Azure.Storage.Blobs.Models;

namespace SantaTracker.Microservice.DataArchival.Functions.CosmosDbTrigger
{
    public class DataArchivalFunction
    { 
        private CosmosClient _cosmosClient;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly Dictionary<string, DateTime> _timestamps = new Dictionary<string, DateTime>();
        private readonly object _threadLock = new object();

        public DataArchivalFunction(
            CosmosClient cosmosClient,
            IAzureClientFactory<BlobServiceClient> blobClientFactory)
        {
            _cosmosClient = cosmosClient;

            var blobServiceClient = blobClientFactory.CreateClient("Default");
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(Constants.StorageAccount.FlightLocationArchiveContainer);
            _blobContainerClient.CreateIfNotExistsAsync();
        }

        [FunctionName(nameof(DataArchivalFunction))]
        public async Task DeliveryBoardAsync([CosmosDBTrigger(
                    databaseName: Constants.CosmosDb.DatabaseName,
                    collectionName: Constants.CosmosDb.LocationContainerName,
                    ConnectionStringSetting = Constants.CosmosDb.Connection,
                    LeaseCollectionName = Constants.CosmosDb.LeaseContainerName,
                    LeaseCollectionPrefix = "DataArchival-",
                    CreateLeaseCollectionIfNotExists = true)]
                IReadOnlyList<Document> input,
            ILogger log)
        {
            foreach (var document in input)
            {
                try
                {
                    await ArchiveFlightLocationAsync(document, log);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message);
                }
            }
        }

        private async Task ArchiveFlightLocationAsync(Document document, ILogger log)
        {
            var locationEvent = JsonConvert.DeserializeObject<LocationEvent>(document.ToString());

            if (ShouldSkip(locationEvent))
            {
                return;
            }
            
            var blobName = $"{DateTime.UtcNow:yyyyMMdd-HHmmss}-{locationEvent.FlightNumber}-{locationEvent.RouteNumber}-{locationEvent.Id}.json";
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            var bytes = Encoding.UTF8.GetBytes(document.ToString());
            await blobClient.UploadAsync(
                new MemoryStream(bytes),
                new BlobHttpHeaders()
                {
                    ContentType = "application/json"
                });

            log.LogWarning($"Archived '{blobName}' to blob storage");
        }

        private bool ShouldSkip(LocationEvent locationEvent)
        {
            // Make sure not to miss the last archive for completed flights
            if (locationEvent.IsComplete)
            {
                return false;
            }

            // Throttle continuous processing by delaying between archival of the same flight to blob storage
            lock (_threadLock)
            {
                if (_timestamps.ContainsKey(locationEvent.FlightNumber))
                {
                    if (DateTime.Now.Subtract(_timestamps[locationEvent.FlightNumber]).TotalSeconds < 15)
                    {
                        return true;
                    }
                    _timestamps[locationEvent.FlightNumber] = DateTime.Now;
                }
                else
                {
                    _timestamps.Add(locationEvent.FlightNumber, DateTime.Now);
                }
            }

            return false;
        }
    }
}
