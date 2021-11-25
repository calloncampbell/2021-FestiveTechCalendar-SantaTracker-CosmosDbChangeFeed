using System;
using System.Linq;

namespace SantaTracker.Shared.Models
{
    public class SpeedAltitudeEvent
    {
        public int Speed { get; set; }
        public int Altitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
