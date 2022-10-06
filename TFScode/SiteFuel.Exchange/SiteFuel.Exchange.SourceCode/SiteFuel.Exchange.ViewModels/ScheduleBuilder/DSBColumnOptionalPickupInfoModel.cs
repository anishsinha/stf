using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class DSBColumnOptionalPickupInfoModel
    {
        public string Id { get; set; }
        public int incId { get; set; }
        public string RegionId { get; set; }
        public int CompanyId { get; set; }
        public string ScheduleBuilderId { get; set; }
        public string ShiftId { get; set; }
        public int ShiftIndex { get; set; } = 0;
        public int ShiftOrderNumber { get; set; } = 0;
        public int DriverColIndex { get; set; } = 0;
        public int TfxFuelTypeId { get; set; } = 0;
        public string TfxFuelTypeName { get; set; }
        public DSBPickupLocationInfoModel DSBPickupLocationInfo { get; set; } = new DSBPickupLocationInfoModel();
        public int isAdded { get; set; } = 0;
        public int DriverId { get; set; } = 0;
    }
    public class DSBPickupLocationInfoModel
    {
        public int PickupLocationType { get; set; }
        public DropdownDisplayItem TfxTerminal { get; set; } = new DropdownDisplayItem();
        public OptionalBulkPlantAddressModel TfxBulkPlant { get; set; } = new OptionalBulkPlantAddressModel();
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
    }
    public class OptionalBulkPlantAddressModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public DropdownDisplayExtendedItem State { get; set; } = new DropdownDisplayExtendedItem();
        public DropdownDisplayExtendedItem Country { get; set; } = new DropdownDisplayExtendedItem();
        public string ZipCode { get; set; }
        public string CountyName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string TimeZoneName { get; set; }
        public string SiteName { get; set; }
        public int Id { get; set; }
    }
}
