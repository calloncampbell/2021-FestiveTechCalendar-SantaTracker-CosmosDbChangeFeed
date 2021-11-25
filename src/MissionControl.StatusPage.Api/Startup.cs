using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using MissionControl.StatusPage.Api.Abstractions.Interfaces;
using MissionControl.StatusPage.Api.Services;
using SantaTracker.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(MissionControl.StatusPage.Api.Startup))]

namespace MissionControl.StatusPage.Api
{
    public class Startup : FunctionsStartup
    {
        private static IConfigurationRoot Configuration { get; set; }
        public IConfigurationBuilder ConfigurationBuilder { get; set; }
        private static IConfigurationRefresher ConfigurationRefresher { set; get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup()
        {
            ConfigurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddUserSecrets("67331bb4-f6b7-4642-84ad-b4d1e1ca3b61")
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

            //var connectionString_PrimaryStore = Environment.GetEnvironmentVariable("AzureAppConfiguration.ConnectionString_PrimaryStore");
            //var cacheExpiryInSeconds = double.Parse(Environment.GetEnvironmentVariable("AzureAppConfiguration.CacheExpirationTimeInSeconds") ?? "300");
            //var environmentLabel = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("AzureAppConfiguration.EnvironmentLabel"))
            //    ? Environment.GetEnvironmentVariable("AzureAppConfiguration.EnvironmentLabel")
            //    : LabelFilter.Null;

            //ConfigurationBuilder
            //    .AddAzureAppConfiguration(options =>
            //    {
            //        options.Connect(connectionString_PrimaryStore)
            //               .Select($"{Constants.AzureAppConfigPrefix}:*")
            //               .Select($"{Constants.AzureAppConfigPrefix}:*", environmentLabel)
            //               .ConfigureRefresh(refreshOptions =>
            //                    refreshOptions.Register(key: $"{Constants.AzureAppConfigPrefix}:Sentinel", label: environmentLabel, refreshAll: true)
            //                              .SetCacheExpiration(TimeSpan.FromSeconds(cacheExpiryInSeconds))
            //               )
            //               .UseFeatureFlags(flagOptions =>
            //               {
            //                   flagOptions.Label = environmentLabel;
            //                   flagOptions.CacheExpirationInterval = TimeSpan.FromSeconds(cacheExpiryInSeconds);
            //               });
            //        ConfigurationRefresher = options.GetRefresher();
            //    });

            Configuration = ConfigurationBuilder.Build();

            builder.Services.AddLogging();
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton(Configuration);
            //builder.Services.AddSingleton(ConfigurationRefresher);
            builder.Services.AddSingleton<INorthPoleService, NorthPoleService>();
            builder.Services.AddSingleton<ISantaTrackerService, SantaTrackerService>();
        }
    }
}
