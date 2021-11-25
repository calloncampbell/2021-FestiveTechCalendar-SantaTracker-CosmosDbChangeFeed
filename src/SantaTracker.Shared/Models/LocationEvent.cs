using Newtonsoft.Json;
using System;
using System.Linq;

namespace SantaTracker.Shared.Models
{
    public class LocationEvent
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

        [JsonProperty("durationMinutes")]
        public double DurationMinutes { get; set; }

        [JsonProperty("distanceMiles")]
        public int DistanceMiles { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("altitude")]
        public int Altitude { get; set; }

        [JsonProperty("speed")]
        public int Speed { get; set; }

        [JsonProperty("remainingMinutes")]
        public double RemainingMinutes { get; set; }

        [JsonProperty("remainingMiles")]
        public int RemainingMiles { get; set; }

        [JsonProperty("arrivalTime")]
        public DateTime? ArrivalTime { get; set; }

        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }

        [JsonProperty("ttl")]
        public int Ttl { get; set; }
    }
}
