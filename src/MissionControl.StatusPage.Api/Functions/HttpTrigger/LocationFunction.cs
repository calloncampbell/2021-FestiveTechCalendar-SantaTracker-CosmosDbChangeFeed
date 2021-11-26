using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MissionControl.StatusPage.Api.Abstractions.Interfaces;
using SantaTracker.Shared.Models;

namespace MissionControl.StatusPage.Api.Functions.HttpTrigger
{
    public class LocationFunction
    {
        private readonly ISantaTrackerService _santaTrackerService;

        public LocationFunction(ISantaTrackerService santaTrackerService)
        {
            _santaTrackerService = santaTrackerService;
        }

        [FunctionName("CurrentLocationFunction")]
        public async Task<IActionResult> GetCurrentLocationAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request for CurrentLocationFunction");

            var result = await _santaTrackerService.GetSantaLocationFromMaterializedViewAsync(DateTime.Now.Year.ToString());
            var response = new SantaLocationResponse()
            {                
                CurrentLocation = result.LocationEvent,
                Cost = result.Cost,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            return new OkObjectResult(response);
        }

        [FunctionName("DeliveryBoardFunction")]
        public async Task<IActionResult> GetCitiesDeliveredAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request for DeliveryBoardFunction");

            string[] cities = { "YYT", 
                                "YQY", 
                                "YYG", 
                                "YHZ", 
                                "YFC", 
                                "YFB", 
                                "YQB", 
                                "YUL", 
                                "YOW", 
                                "YYZ", 
                                "YAM", 
                                "YQT", 
                                "YWG", 
                                "YQR", 
                                "YXE", 
                                "YZF", 
                                "YEG", 
                                "YCG", 
                                "YVR", 
                                "YYJ", 
                                "YXS", 
                                "YXY", 
                                "ANC" }; 

            var results = await _santaTrackerService.GetCityDeliveryStatusFromMaterializedViewAsync(cities);
            var response = new FlightSegmentStatusResponse()
            {
                Cost = results.Cost,
                FlightSegments = results.CitiesDelivered
            };
            return new OkObjectResult(response);
        }
    }
}
