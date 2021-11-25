using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SantaTracker.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(SantaTracker.Microservice.CurrentLocation.Startup))]

namespace SantaTracker.Microservice.CurrentLocation
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
                .AddUserSecrets("b36ab30f-6c40-411e-8ba6-b23a9ad54b25")
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

            Configuration = ConfigurationBuilder.Build();

            builder.Services.AddLogging();
            builder.Services.AddSingleton(Configuration);            
        }
    }
}
