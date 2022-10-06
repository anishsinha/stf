using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.FreightModels;
using System;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class DSBColumnOptionalPickupInfo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId RegionId { get; set; } = ObjectId.Empty;
        public int CompanyId { get; set; }
        public ObjectId ScheduleBuilderId { get; set; } = ObjectId.Empty;
        public ObjectId ShiftId { get; set; } = ObjectId.Empty;
        public int ShiftIndex { get; set; } = 0;
        public int DriverColIndex { get; set; } = 0;
        public int TfxFuelTypeId { get; set; } = 0;
        public string TfxFuelTypeName { get; set; }
        public DSBPickupLocationInfo DSBPickupLocationInfo { get; set; } = new DSBPickupLocationInfo();
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
    public class DSBPickupLocationInfo
    {
        public int PickupLocationType { get; set; }
        public DropdownDisplayItem TfxTerminal { get; set; } = new DropdownDisplayItem();
        public BulkPlantAddressModel TfxBulkPlant { get; set; } = new BulkPlantAddressModel();
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
    }
}
