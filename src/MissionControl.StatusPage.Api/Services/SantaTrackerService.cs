using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using MissionControl.StatusPage.Api.Abstractions.Interfaces;
using SantaTracker.Shared.Constants;
using SantaTracker.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static SantaTracker.Shared.Constants.Constants;

namespace MissionControl.StatusPage.Api.Services
{
    public class SantaTrackerService : ISantaTrackerService
    {
        private CosmosClient _cosmosClient;
        private readonly ILogger<SantaTrackerService> _logger;

        public SantaTrackerService(
            CosmosClient cosmosClient,
            ILogger<SantaTrackerService> log)
        {
            _cosmosClient = cosmosClient;
            _logger = log;
        }

        public async Task<(LocationEvent LocationEvent, double Cost)> GetSantaLocationFromMaterializedViewAsync(string flightNumber)
        {
            try
            {
                var container = _cosmosClient.GetContainer(Constants.CosmosDb.DatabaseName, Constants.CosmosDb.CurrentLocationContainerName);
                var result = await container.ReadItemAsync<LocationEvent>(flightNumber, new PartitionKey("location"));
                var cost = result.RequestCharge;    // should never be more than 1 RU for documents under 1K
                var location = result.Resource;

                return (location, cost);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return (null, 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetSantaLocationFromMaterializedViewAsync error");
                throw;
            }
        }

        public async Task<(List<CityDelivery> CitiesDelivered, double Cost)> GetCityDeliveryStatusFromMaterializedViewAsync(string[] cities)
        {
            try
            {
                var container = _cosmosClient.GetContainer(Constants.CosmosDb.DatabaseName, Constants.CosmosDb.DeliveryBoardContainerName);
                var citiesCsv = string.Join("','", cities);
                var query = $"SELECT * FROM c WHERE c.type = 'location' AND c.arrivalCity IN('{citiesCsv}')";
                var options = new QueryRequestOptions { PartitionKey = new PartitionKey("location") };
                var iterator = container.GetItemQueryIterator<CityDelivery>(query, requestOptions: options);

                var locations = new List<CityDelivery>();
                var cost = 0d;
                while (iterator.HasMoreResults)
                {
                    var page = await iterator.ReadNextAsync();
                    cost += page.RequestCharge;
                    locations.AddRange(page);
                }

                return (locations.OrderBy(x=>x.RouteNumber).ToList(), cost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetCityDeliveryStatusFromMaterializedViewAsync error");
                throw;
            }
        }

    }
}
