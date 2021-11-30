using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SantaTracker.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(SantaTracker.Microservice.DataArchival.Startup))]

namespace SantaTracker.Microservice.DataArchival
{
    public class Startup : FunctionsStartup
    {
        private static IConfigurationRoot Configuration { get; set; }
        public IConfigurationBuilder ConfigurationBuilder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup()
        {
            ConfigurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddUserSecrets("e863a0c5-9246-4654-8bfb-1d46bd2561ed")
                .AddEnvironmentVariables();

            Configuration = ConfigurationBuilder.Build();
        }

        /// <summary>
        /// Configures the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var regionName = Environment.GetEnvironmentVariable("REGION_NAME");
            if (string.IsNullOrWhiteSpace(regionName))
            {
                regionName = Regions.CanadaCentral;
            }

            // Register the CosmosClient as a Singleton
            builder.Services.AddSingleton((s) =>
            {
                CosmosClientBuilder configurationBuilder = new CosmosClientBuilder(Configuration[Constants.CosmosDb.Connection])
                    .WithApplicationRegion(regionName);

                return configurationBuilder.Build();
            });

            // Register the BlobServiceClient as a Singleton
            builder.Services.AddAzureClients(builder =>
            {
                var storageConnectionString = Configuration[Constants.StorageAccount.ConnectionString];
                builder.AddBlobServiceClient(storageConnectionString);
            });

            Configuration = ConfigurationBuilder.Build();

            builder.Services.AddLogging();
            builder.Services.AddSingleton(Configuration);            
        }
    }
}
