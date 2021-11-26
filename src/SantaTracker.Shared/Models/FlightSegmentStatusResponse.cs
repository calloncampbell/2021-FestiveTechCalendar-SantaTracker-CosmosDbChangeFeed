using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaTracker.Shared.Models
{
    public class FlightSegmentStatusResponse
    {
        public double Cost { get; set; }
        public List<CityDelivery> FlightSegments { get; set; }
    }
}
