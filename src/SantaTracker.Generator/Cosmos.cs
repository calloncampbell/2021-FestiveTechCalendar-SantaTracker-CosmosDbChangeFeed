using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaTracker.Generator
{
    public static class Cosmos
    {
        private static string _connectionString;
        private static CosmosClient _client;

		public static void SetAuth(string connectionString)
		{
			_connectionString = connectionString;
		}

		public static CosmosClient Client
		{
			get
			{
				if (_client == null)
				{
					if (_connectionString == null)
					{
						throw new Exception("Error connection string is missing");	
					}

					_client = new CosmosClient(_connectionString);
				}
				return _client;
			}
		}
	}
}
