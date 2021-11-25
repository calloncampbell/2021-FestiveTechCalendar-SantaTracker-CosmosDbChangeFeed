using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaTracker.Generator
{
	public class FlightSpatial
	{
		public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
		{
			var distanceInRadians =
				Math.Sin(this.DegreesToRadians(lat1)) * Math.Sin(this.DegreesToRadians(lat2)) +
				Math.Cos(this.DegreesToRadians(lat1)) * Math.Cos(this.DegreesToRadians(lat2)) * Math.Cos(this.DegreesToRadians(lon1 - lon2));

			var distanceInDegrees = this.RadiansToDegrees(Math.Acos(distanceInRadians));
			var distanceInMiles = distanceInDegrees * 60 * 1.1515;

			return distanceInMiles;
		}

		private double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;

		private double RadiansToDegrees(double radians) => radians / Math.PI * 180.0;
	}
}
