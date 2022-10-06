using System.Collections.Generic;

namespace SiteFuel.FreightModels
{
    public class RecurringScheduleBuilder
    {
        public string RegionId { get; set; }
        public string Date { get; set; }
        public string ScheduleBuilderId { get; set; }
        public int View { get; set; }
        public int DsbView { get; set; } = 1;
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public List<RecurringShiftDetails> ShiftInformation { get; set; }
        public string ScheduleBuilderViewId { get; set; }
        public bool IsBackgroundJobScheduleCreation { get; set; } = false;
        public bool IsDsbDriverSchedule { get; set; } = false;
    }
    public class RecurringShiftDetails
    {
        public string ShiftId { get; set; }
        public int DriverRowIndex { get; set; }
        public int ShiftIndex { get; set; }
        public int DriverColIndex { get; set; }
    }
}
