using System;
using System.Collections.Generic;

namespace SiteFuel.FreightModels
{
   public class DriverScheduleMappingViewModel : CommonFieldsModel
    {
        public string Id { get; set; }
        public int DriverId { get; set; }
        public string ShiftId { get; set; }
        public string DriverName { get; set; }
        public bool IsUnplanedSchedule { get; set; }       
        public List<DriverScheduleViewModel> ScheduleList { get; set; }
        public string SelectedDate { get; set; }
        public bool IsDsbDriverSchedule { get; set; } = false;

    }

    public class DriverScheduleViewModel
    {        
        public string Id { get; set; }
        public string ShiftId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StrStartDate { get; set; }
        public string StrEndDate { get; set; }
        public List<DateTime> RepeatDayList { get; set; }
        public List<string> RepeatDayStringList { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public ShiftViewModel ShiftDetail { get; set; }
        public int? RepeatEveryDay { get; set; }
        public int TypeId { get; set; }
    }

    public class DeleteDriverSchedules
    {
        public DriverScheduleMappingViewModel driverScheduleMappingViewModel { get; set; }
        public int deleteDriverSchedule { get; set; } 
    }
}
