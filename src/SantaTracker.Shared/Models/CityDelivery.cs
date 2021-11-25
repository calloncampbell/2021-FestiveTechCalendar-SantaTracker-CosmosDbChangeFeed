using Newtonsoft.Json;
using System;
using System.Linq;

namespace SantaTracker.Shared.Models
{
    public class CityDelivery
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = "location";

        [JsonProperty("flightNumber")]
        public string FlightNumber { get; set; }

        [JsonProperty("routeNumber")]
        public string RouteNumber { get; set; }

        [JsonProperty("departureCity")]
        public string DepartureCity { get; set; }

        [JsonProperty("arrivalCity")]
        public string ArrivalCity { get; set; }

        [JsonProperty("remainingMinutes")]
        public double RemainingMinutes { get; set; }

        [JsonProperty("remainingMiles")]
        public int RemainingMiles { get; set; }

        [JsonProperty("arrivalTime")]
        public DateTime? ArrivalTime { get; set; }

        //[JsonProperty("presentsDelivered")]
        //public int PresentsDelivered { get; set; }

        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }

    }
}
