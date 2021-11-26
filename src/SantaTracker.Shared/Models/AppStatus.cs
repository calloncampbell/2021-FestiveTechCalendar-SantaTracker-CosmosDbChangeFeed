using SantaTracker.Shared.Enums;
using System;
using System.Linq;

namespace SantaTracker.Shared.Models
{
    public class AppStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppStatusType Status { get; set; }
        public string StatusName { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
