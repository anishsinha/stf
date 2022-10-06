using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class RegionViewModel : CommonFieldsModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SlotPeriod { get; set; }
        public int CompanyId { get; set; }
        public List<DropdownDisplayItem> Jobs { get; set; }
        public List<DropdownDisplayItem> Drivers { get; set; }
        public List<DropdownDisplayItem> States { get; set; }
        public List<DropdownDisplayItem> Dispatchers { get; set; }
        public List<DropdownDisplayItem> Trailers { get; set; }
        public List<TfxCarrierDropdownDisplayViewModelItem> Carriers { get; set; } = new List<TfxCarrierDropdownDisplayViewModelItem>();
        public List<ShiftViewModel> Shifts { get; set; }
        public bool IsDsbDriverSchedule { get; set; } = false;
        public List<int> ProductTypeIds { get; set; }
        public List<DropdownDisplayItem> FuelTypeIds { get; set; }
        public RegionFavProductType? FavProductTypeId { get; set; }
    }

    public class JobModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string RegionId { get; set; }
    }
    public class RegionJobsModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DropdownDisplayItem> Jobs { get; set; }
    }
    public class CarrierRegionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SequenceNumber { get; set; }
        public string RegionId { get; set; }
        public List<TfxCarrierRegionDetailsModel> Regions { get; set; }
    }
    public class RegionDriverRemoveModel
    {
        public int DriverId { get; set; }
        public int UserId { get; set; }
        public bool IsScheduleExists { get; set; } = false;
        public List<string> ScheduleBuilderIds { get; set; } = new List<string>();
    }
    public class RegionDriverRemoveViewModelModel
    {
        public string Id { get; set; }
        public List<DropdownDisplayItem> Drivers { get; set; } = new List<DropdownDisplayItem>();
    }

    public class RegionFavProductModel
    {
        public List<int> TfxProductTypeIds { get; set; } = new List<int>();
        public List<DropdownDisplayItem> TfxFuelTypeIds { get; set; } = new List<DropdownDisplayItem>();
        public RegionFavProductType TfxFavProductTypeId { get; set; }
    }
}
