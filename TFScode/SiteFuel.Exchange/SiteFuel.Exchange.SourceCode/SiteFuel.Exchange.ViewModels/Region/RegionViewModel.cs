using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class RegionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SlotPeriod { get; set; }
        public int CompanyId { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public int CreatedBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; } = DateTimeOffset.Now;
        public int UpdatedBy { get; set; }
        public List<DropdownDisplayExtendedItem> Jobs { get; set; }
        public List<DropdownDisplayExtendedItem> Drivers { get; set; }
        public List<DropdownDisplayExtendedItem> Dispatchers { get; set; }
        public List<DropdownDisplayExtendedItem> Trailers { get; set; }
        public List<DropdownDisplayExtendedItem> States { get; set; }

        public List<TfxCarrierDropdownDisplayItem> Carriers { get; set; }
        public List<ShiftViewModel> Shifts { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDsbDriverSchedule { get; set; } = false;
        public List<int> ProductTypeIds { get; set; }
        public List<DropdownDisplayExtendedItem> FuelTypeIds { get; set; }
        public int? FavProductTypeId { get; set; }
    }

    public class TfxCarrierDropdownDisplayItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int SequenceNo { get; set; } = 0;
        public string RegionId { get; set; } = string.Empty;
    }

    public class RegionModel
    {
        public int CountryId { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int DefaultSlotPeriod { get; set; }
        public List<RegionViewModel> Regions { get; set; }
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
        public List<TfxCarrierRegionDetailsModel> Regions { get; set; }
    }
    public class RegionDriverRemoveModel
    {
        public int DriverId { get; set; }
        public int UserId { get; set; }
        public bool IsScheduleExists { get; set; } = false;
        public List<string> ScheduleBuilderIds { get; set; } = new List<string>();
    }
    public class InvitedDriverResponseModel
    {
        public List<string> ScheduleBuilderIds { get; set; } = new List<string>();
        public int DriverId { get; set; }
        public int UserId { get; set; }
    }
}
