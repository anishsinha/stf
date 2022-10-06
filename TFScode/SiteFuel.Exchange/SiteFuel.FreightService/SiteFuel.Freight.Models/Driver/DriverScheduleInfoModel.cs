using System;

namespace SiteFuel.FreightModels
{
    public class DriverScheduleInfoModel
    {
        public int DriverId { get; set; }
        public string ShiftId { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
