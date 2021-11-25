using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaTracker.Shared.Models
{
    public class City
    {
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; } = "city";

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("latitude")]
		public double Latitude { get; set; }

		[JsonProperty("longitude")]
		public double Longitude { get; set; }

		[JsonProperty("region")]
		public string Region { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

	}
}
