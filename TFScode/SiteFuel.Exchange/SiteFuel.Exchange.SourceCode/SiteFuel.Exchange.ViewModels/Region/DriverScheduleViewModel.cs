using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class DriverScheduleMappingViewModel
    {
        public string Id { get; set; }
        public int DriverId { get; set; }
        public string ShiftId { get; set; }
        public string DriverName { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public int CreatedBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; } = DateTimeOffset.Now;
        public int UpdatedBy { get; set; }       
        public bool IsUnplanedSchedule { get; set; }
        public List<DriverScheduleViewModel> ScheduleList { get; set; }
        public string SelectedDate { get; set; }
        public int CompanyId { get; set; }
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
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ShiftViewModel ShiftDetail { get; set; }
        public List<string> RepeatDayStringList { get; set; }
        public int? RepeatEveryDay { get; set; }
        public int TypeId { get; set; }

    }

}
