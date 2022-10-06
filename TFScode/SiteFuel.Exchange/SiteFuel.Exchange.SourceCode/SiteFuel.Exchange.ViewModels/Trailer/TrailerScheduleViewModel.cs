using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class TrailerScheduleViewModel
    {
        public string Id { get; set; }
        public string RegionId { get; set; }
        public string TrailerId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public List<DateTimeOffset> RepeatDayList { get; set; }
        public bool IsActive { get; set; }
        public List<TrailerShiftDetailViewModel> TrailerShiftDetail { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public int CreatedBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; } = DateTimeOffset.Now;
        public int UpdatedBy { get; set; }
    }

    public class TrailerShiftDetailViewModel
    {
        public string ShiftId { get; set; }
        public string ColumnId { get; set; }
    }
}
