using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class GroupModel
    {
        public int GroupId { get; set; }
        public int GroupStatus { get; set; }
        public List<ScheduleDetailModel> Schedules { get; set; } = new List<ScheduleDetailModel>();
        public int DriverId { get; set; }
        public int? PreviousDriverId { get; set; }
        public bool IsTodaySchedule { get; set; }
    }

    public class ScheduleDetailModel : ScheduleModel
    {
        public int ScheduleStatus { get; set; }
    }

    public class ScheduleNotificationModel : ScheduleModel
    {
        public int GroupId { get; set; }
        public int ScheduleStatus { get; set; }
        public int? DriverId { get; set; }
        public int? PreviousDriverId { get; set; }
        public DateTimeOffset ScheduleDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string TimeZone { get; set; }
    }
}
