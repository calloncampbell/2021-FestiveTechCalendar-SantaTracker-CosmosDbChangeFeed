using SantaTracker.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionControl.StatusPage.Api.Abstractions.Interfaces
{
    public interface ISantaTrackerService
    {
        Task<(LocationEvent LocationEvent, double Cost)> GetSantaLocationFromMaterializedViewAsync(string flightNumber);
        Task<(List<CityDelivery> CitiesDelivered, double Cost)> GetCityDeliveryStatusFromMaterializedViewAsync(string[] cities);
    }
}
