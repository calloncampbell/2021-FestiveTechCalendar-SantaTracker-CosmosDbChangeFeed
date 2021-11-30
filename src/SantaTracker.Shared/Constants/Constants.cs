using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaTracker.Shared.Constants
{
    public class Constants
    {
        public const string AzureAppConfigPrefix = "SantaTracker";

        public class CosmosDb
        {
            public const string Connection = "CosmosDB-ConnectionStringReadWrite";
            public const string DatabaseName = "SantaTracker";

            public const string MetaDataContainerName = "MetaData";
            public const string LeaseContainerName = "Lease";
            public const string LocationContainerName = "Location";
            public const string CurrentLocationContainerName = "CurrentLocation";
            public const string DeliveryBoardContainerName = "DeliveryBoard";
            public const string StatusContainerName = "Status";
            public const string CurrentStatusContainerName = "CurrentStatus";
        }

        public class StorageAccount
        {
            public const string ConnectionString = "Storage-ConnectionString";

            public const string FlightLocationArchiveContainer = "flight-location-archive";
        }
    }
}
