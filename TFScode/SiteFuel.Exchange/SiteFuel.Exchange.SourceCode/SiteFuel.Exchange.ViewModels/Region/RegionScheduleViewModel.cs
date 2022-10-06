using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;


namespace SiteFuel.Exchange.ViewModels
{
   public class RegionScheduleViewModel
    {
        public string Id { get; set; }
        public string RegionId { get; set; }
        public string RouteId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public List<string> RepeatDayList { get; set; }
        //public string Repeat { get; set; }        
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<RegionShiftDetailViewModel> RegionShiftDetail { get; set; }        
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public int CreatedBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; } = DateTimeOffset.Now;
        public int UpdatedBy { get; set; }
    }

    public class RegionShiftDetailViewModel
    {
        public string ShiftId { get; set; }
        public int ColumnIndex { get; set; }
    }

    public class RegionFavProductModel
    {
        public List<int> TfxProductTypeIds { get; set; } = new List<int>();
        public List<DropdownDisplayExtendedItem> TfxFuelTypeIds { get; set; } = new List<DropdownDisplayExtendedItem>();
        public RegionFavProductType TfxFavProductTypeId { get; set; }
    }
}
