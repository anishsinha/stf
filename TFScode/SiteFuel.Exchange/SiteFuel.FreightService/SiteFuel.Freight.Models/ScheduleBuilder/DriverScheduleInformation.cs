using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class DriverScheduleInformation
    {
        public int StatusCode { get; set; } = 1;
        public string StatusMessage { get; set; }
        public List<DriverShiftInfo> DriverShiftInfo { get; set; } = new List<DriverShiftInfo>();
    }
    public class DriverShiftInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string RegionId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string RegionName { get; set; }
        public int SlotPeriod { get; set; }
        public List<DropdownDisplayItem> DriverInformation { get; set; } = new List<DropdownDisplayItem>();
    }
}
