using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class OttoScheduleInfo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string SiteId { get; set; } //from freightservice db
        public string TankId { get; set; } // old TankID
        public string StorageId { get; set; }
        public int TfxJobId { get; set; }
        public int TfxAssetId { get; set; }
        public string DeliveryReqId { get; set; }
        public string ScheduleBuilderId { get; set; }
        public OttoShiftInfo ShiftInfo { get; set; } = null;
        public OttoDeliveryRequestInfo DeliveryRequestInfo { get; set; } = null;
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; } = true;
     }
    public class OttoShiftInfo
    {
        public int CompanyId { get; set; }
        public string RegionId { get; set; }
        public string ShiftId { get; set; }
        public int ShiftIndex { get; set; }
        public int DriverRowIndex { get; set; }
        public int DriverColIndex { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class OttoDeliveryRequestInfo
    {
        public PickupLocationType PickupLocationType { get; set; }
        public FreightModels.DropdownDisplayItem TfxTerminal { get; set; }
        public BulkPlantAddressModel TfxBulkPlant { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public bool IsCommonBadge { get; set; }
        public string DispactherNote { get; set; }
        public string Notes { get; set; }
        public RouteInfo RouteInfo { get; set; } = null;
    }
}
