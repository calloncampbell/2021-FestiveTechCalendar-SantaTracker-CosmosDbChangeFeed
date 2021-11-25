using Newtonsoft.Json;
using System;
using System.Linq;

namespace SantaTracker.Shared.Models
{
    public class FlightSegment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = "flightSegment";

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

        [JsonProperty("arrivalTime")]
        public DateTime? ArrivalTime { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
