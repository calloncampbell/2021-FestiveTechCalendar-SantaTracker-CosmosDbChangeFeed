using MissionControl.StatusPage.Api.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionControl.StatusPage.Api.Abstractions.Models
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
