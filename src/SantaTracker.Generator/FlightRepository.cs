using Microsoft.Azure.Cosmos;
using SantaTracker.Shared.Constants;
using SantaTracker.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaTracker.Generator
{
    public class FlightRepository
    {
		public async Task<City[]> GetCities() => await this.GetList<City>("city");

		public async Task<FlightSegment[]> GetFlightSegments() => await this.GetList<FlightSegment>("flightSegment");

		public async Task<T[]> GetList<T>(string type)
		{
			var list = new List<T>();
			var container = Cosmos.Client.GetContainer(Constants.CosmosDb.DatabaseName, Constants.CosmosDb.MetaDataContainerName);
			var sql = $"SELECT * FROM c WHERE c.type = '{type}'";
			var options = new QueryRequestOptions { PartitionKey = new PartitionKey(type) };
			var iterator = container.GetItemQueryIterator<T>(sql, requestOptions: options);

			while (iterator.HasMoreResults)
			{
				var page = await iterator.ReadNextAsync();
				list.AddRange(page);
			}

			return list.ToArray();
		}

		public async Task<(LocationEvent LocationEvent, string Sql, double Cost)> QueryLocation(string flightNumber)
		{
			var container = Cosmos.Client.GetContainer(Constants.CosmosDb.DatabaseName, Constants.CosmosDb.LocationContainerName);
			var query = $"SELECT TOP 1 * FROM c WHERE c.flightNumber = '{flightNumber}' ORDER BY c._ts DESC";
			var options = new QueryRequestOptions { MaxConcurrency = -1 };
			var iterator = container.GetItemQueryIterator<LocationEvent>(query, requestOptions: options);
			var page = await iterator.ReadNextAsync();
			var cost = page.RequestCharge;
			var location = page.FirstOrDefault();

			return (location, query, cost);
		}
	}
}
