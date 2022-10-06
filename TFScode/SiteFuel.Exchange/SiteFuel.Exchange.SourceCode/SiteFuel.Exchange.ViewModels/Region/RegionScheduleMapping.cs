using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class RegionScheduleMapping
    {
        public string Id { get; set; }
        public string RegionId { get; set; }
        public string RouteId { get; set; }
        public string RegionName { get; set; }
        public string RouteName { get; set; }
        public bool IsUnplanedSchedule { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string Description { get; set; }
        public List<string> RepeatDayList { get; set; }
        public List<ShiftDetailViewModel> ShiftDetail { get; set; }
    }

    public class ShiftDetailViewModel
    {
        public string ShiftId { get; set; }
        public string ShiftName { get; set; }
        public int ColumnIndex { get; set; }
        public string ColumnName { get; set; }
    }
}
