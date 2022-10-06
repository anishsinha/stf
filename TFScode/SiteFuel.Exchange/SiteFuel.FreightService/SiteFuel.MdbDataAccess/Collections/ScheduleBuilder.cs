using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.ScheduleBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class ScheduleBuilder : CommonFields
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int TfxCompanyId { get; set; }
        public string RegionId { get; set; }
        public int ObjectFilter { get; set; }
        public int RegionFilter { get; set; }
        public int DSBFilter { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime DateFilter { get; set; }
        public List<TripModel> Trips { get; set; } = new List<TripModel>();
        public List<ShiftModel> Shifts { get; set; } = new List<ShiftModel>();
        public List<TrailerModel> Trailers { get; set; } = new List<TrailerModel>();
        public long TimeStamp { get; set; }
        public int Status { get; set; }
        
    }

    public class TripModel
    {
        [BsonId]
        public ObjectId TripId { get; set; }
        public int GroupId { get; set; }
        public List<ObjectId> DeliveryRequests { get; set; } = new List<ObjectId>();
        public List<DropdownDisplayExtendedItem> TfxDrivers { get; set; } = new List<DropdownDisplayExtendedItem>();
        public List<TrailerModel> Trailers { get; set; } = new List<TrailerModel>();
        public string ShiftId { get; set; }
        public int? ShiftIndex { get; set; }
        public int? DriverRowIndex { get; set; }
        public int? DriverColIndex { get; set; }
        public int? TrailerRowIndex { get; set; }
        public int? TrailerColIndex { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public bool IsShiftCollapsed { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string LoadCode { get; set; }
        public string RouteInfo { get; set; }
        public long TimeStamp { get; set; }
        public bool IsDsbLoadQueueBackgroundProcess { get; set; } = false;
        public DropdownDisplayItem SupplierSource { get; set; }
        public DropdownDisplayItem Carrier { get; set; }
        public TripStatus TripStatus { get; set; }
        public DeliveryGroupStatus DeliveryGroupStatus { get; set; }
        public bool IsCommonPickup { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public DropdownDisplayItem TfxTerminal { get; set; } = new DropdownDisplayItem();
        public BulkPlantAddressModel TfxBulkPlant { get; set; } = new BulkPlantAddressModel();
        public string DriverScheduleMappingId { get; set; }
        public int SlotPeriod { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedByName { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public bool IsIncludeAllRegionDriver { get; set; }
        public bool IsDispatcherDragDropSequence { get; set; } = false;
    }

    public class ShiftTimings
    {
        public string ShiftId { get; set; }
        public DateTimeOffset ShiftStartTime { get; set; }
        public DateTimeOffset ShiftEndTime { get; set; }
    }
}
