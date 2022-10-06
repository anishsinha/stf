using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class DSBColumnOptionalPickupInfoModel
    {

        public string Id { get; set; }
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
    }
    public class DSBPickupLocationInfoModel
    {
        public int PickupLocationType { get; set; }
        public DropdownDisplayItem TfxTerminal { get; set; } = new DropdownDisplayItem();
        public BulkPlantAddressModel TfxBulkPlant { get; set; } = new BulkPlantAddressModel();
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
    }
    public class ScheduleOptionalPickupModel
    {
        public int TrackableScheduleId { get; set; }
        public List<string> OptionalPickupIds { get; set; } = new List<string>();

    }
}
