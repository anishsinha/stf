using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class USP_TimeCardActionSummaryForAllDrivers
    {
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string ClockIn { get; set; }
        public string ClockOut { get; set; }
        public string FuelDropTime { get; set; }
        public string TransitTime { get; set; }
        public string BreakTime { get; set; }
        public string TotalTime { get; set; }
        public string ActionDate { get; set; }
        public string TotalShiftTime { get; set; }
    }
}
