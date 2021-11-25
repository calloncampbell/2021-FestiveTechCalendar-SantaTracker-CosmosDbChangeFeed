using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MissionControl.StatusPage.Api.Abstractions.Models;
using System.Collections.Generic;
using MissionControl.StatusPage.Api.Abstractions.Enums;

namespace MissionControl.StatusPage.Api.Functions.HttpTrigger
{
    public static class StatusFunction
    {
        [FunctionName(nameof(StatusFunction))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var systemStatus = new List<AppStatus>();

            var status0 = new AppStatus()
            {
                Id = 1000,
                Name = "Santa Tracker",
                Status = AppStatusType.Online,
                StatusName = AppStatusType.Online.ToString(),
                UpdatedAt = DateTimeOffset.UtcNow
            };
            systemStatus.Add(status0);

            var status1 = new AppStatus()
            {
                Id = 1001,
                Name = "Sled Telemetry",
                Status = AppStatusType.Online,
                StatusName = AppStatusType.Online.ToString(),
                UpdatedAt = DateTimeOffset.UtcNow
            };
            systemStatus.Add(status1);

            var status2 = new AppStatus()
            {
                Id = 1002,
                Name = "Espresso Machine",
                Status = AppStatusType.Online,
                StatusName = AppStatusType.Online.ToString(),
                UpdatedAt = DateTimeOffset.UtcNow
            };
            systemStatus.Add(status2);

            var status3 = new AppStatus()
            {
                Id = 1003,
                Name = "Wrapping",
                Status = AppStatusType.Online,
                StatusName = AppStatusType.Online.ToString(),
                UpdatedAt = DateTimeOffset.UtcNow
            };
            systemStatus.Add(status3);

            var status4 = new AppStatus()
            {
                Id = 1004,
                Name = "Letters",
                Status = AppStatusType.Offline,
                StatusName = AppStatusType.Offline.ToString(),
                UpdatedAt = DateTimeOffset.UtcNow
            };
            systemStatus.Add(status4);

            var status5 = new AppStatus()
            {
                Id = 1005,
                Name = "Workshop",
                Status = AppStatusType.Offline,
                StatusName = AppStatusType.Offline.ToString(),
                UpdatedAt = DateTimeOffset.UtcNow
            };
            systemStatus.Add(status5);

            var status6 = new AppStatus()
            {
                Id = 1006,
                Name = "Stables",
                Status = AppStatusType.Offline,
                StatusName = AppStatusType.Offline.ToString(),
                UpdatedAt = DateTimeOffset.UtcNow
            };
            systemStatus.Add(status6);

            return new OkObjectResult(systemStatus);
        }
    }
}
