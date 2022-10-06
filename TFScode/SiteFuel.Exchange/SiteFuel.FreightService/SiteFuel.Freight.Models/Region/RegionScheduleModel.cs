using System;
using System.Collections.Generic;

namespace SiteFuel.FreightModels
{
    public class RegionScheduleModel: CommonFieldsModel
    {
        public string Id { get; set; }
        public string RegionId { get; set; }
        public string RouteId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> RepeatDayList { get; set; } = new List<string>();
        public string Description { get; set; }
        public List<RegionShiftDetailViewModel> RegionShiftDetail { get; set; } = new List<RegionShiftDetailViewModel>();
        public ShiftViewModel ShiftDetail { get; set; }
    }

    public class RegionShiftDetailViewModel
    {
        public string ShiftId { get; set; }
        public int ColumnIndex { get; set; }
    }

}
