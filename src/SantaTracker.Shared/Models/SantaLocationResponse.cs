using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SantaTracker.Shared.Models;

namespace SantaTracker.Shared.Models
{
    public class SantaLocationResponse
    {        
        public LocationEvent? CurrentLocation { get; set; }
        public double Cost { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
