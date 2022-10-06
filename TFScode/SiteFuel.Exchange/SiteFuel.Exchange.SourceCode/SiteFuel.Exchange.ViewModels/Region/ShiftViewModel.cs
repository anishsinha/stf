using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ShiftViewModel : ShiftModel
    {
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string DisplayStartTime { get; set; }
        public string DisplayEndTime { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; } = true;
        public string ShiftInfo { get; set; }
        public long StartTimespan { get; set; }
    }

    public class ShiftModel
    {
        public string Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int SlotPeriod { get; set; }

        //[2 Apr 2020]
        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public int OrderNo { get; set; }
    }
    public class RegionDSBModel
    {
        public int Id { get; set; }
        public string RegionId { get; set; }
        public List<ShiftViewModel> DSBShiftInfo = new List<ShiftViewModel>();
    }
}