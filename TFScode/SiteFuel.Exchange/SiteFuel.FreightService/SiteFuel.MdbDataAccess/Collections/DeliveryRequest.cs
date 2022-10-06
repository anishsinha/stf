using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class DeliveryRequest : CommonFields
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string TfxJobAddress { get; set; }
        public string TfxJobCity { get; set; }
        public string TfxJobName { get; set; }
        public string TfxProductType { get; set; }
        public int TfxUoM { get; set; }
        public bool IsBlendedRequest { get; set; }
        public bool IsAdditive { get; set; }
        public string BlendedGroupId { get; set; }
        public string CreditApprovalFilePath { get;set; }
        public int TfxCreatedByCompanyId { get; set; }
        public string TfxDisplayJobId { get; set; } //from freightservice db
        public string StorageTypeId { get; set; } // old TankID
        public string StorageId { get; set; }
        public int TfxProductTypeId { get; set; }
        public int? TfxFuelTypeId { get; set; }
        public string FuelType { get; set; }
        public decimal DRCreationLevel { get; set; }
        public decimal RequiredQuantity { get; set; }
        public DeliveryReqPriority Priority { get; set; } = DeliveryReqPriority.MustGo;
        public decimal? CurrentThreshold { get; set; }
        public DeliveryReqStatus Status { get; set; } = DeliveryReqStatus.Pending;
        public int TfxScheduleStatus { get; set; }
        public int TfxScheduleEnrouteStatus { get; set; }
        public int TfxAssignedToUserId { get; set; }
        public int TfxAssignedToCompanyId { get; set; }
        public string TfxAssignedToRegionId { get; set; }
        public bool IsReCreated { get; set; }
        public int? TfxSupplierCompanyId { get; set; }
        public ObjectId CreatedRegionId { get; set; }
        public string ParentId { get; set; }
        public int TfxJobId { get; set; }
        public string TfxCustomerCompany { get; set; }
        public int? TfxDeliveryGroupId { get; set; }
        public int? TfxDeliveryScheduleId { get; set; }
        public int? TfxTrackableScheduleId { get; set; }
        public int? TfxOrderId { get; set; }
        public DropdownDisplayItem TfxTerminal { get; set; } = new DropdownDisplayItem();
        public BulkPlantAddressModel TfxBulkPlant { get; set; } = new BulkPlantAddressModel();
        public AutoDrStatus AutoDRStatus { get; set; }
        public DateTimeOffset? AutoCreatedOn { get; set; }
        public DateTimeOffset? AutoUpdatedOn { get; set; }
        public string TfxScheduleStatusName { get; set; }
        public string ScheduleBuilderId { get; set; }
        public string TripId { get; set; }
        public decimal TankMaxFill { get; set; }
        public int DeliveryRequestType { get; set; }
        public ObjectId? PostLoadedFor { get; set; }
        public LoadInfo PostLoadInfo { get; set; }
        public ObjectId? PreLoadedFor { get; set; }
        public LoadInfo PreLoadInfo { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public bool IsCommonBadge { get; set; }
        public string DispactherNote { get; set; }
        public DRSource DelReqSource { get; set; }
        public string CarrierOrderId { get; set; }
        public string ExternalRefId { get; set; }
        public bool IsRecurringSchedule { get; set; } = false;
        public ObjectId RecurringScheduleId { get; set; } = new ObjectId("000000000000000000000000");
        public int ScheduleQuantityType { get; set; } = 0;

        public RouteInfo RouteInfo { get; set; } = null;
        public string BrokeredParentId { get; set; }
        public string BrokeredChildId { get; set; }
        public int CarrierStatus { get; set; }
        public bool IsDispatchRetainedByCustomer { get; set; } = false;
        public List<CompartmentsInfo> Compartments = new List<CompartmentsInfo>();
        public DeliveryRequestFor DeliveryRequestFor { get; set; } = DeliveryRequestFor.UnKnown;
        public bool IsFilldInvoke { get; set; } = false;
        public string Notes { get; set; }
        public bool IsRetainFuelLoaded { get; set; }
        public DeliveryWindowInfo DeliveryWindowInfo { get; set; }
        public ObjectId? GroupParentDRId { get; set; } = null;
        public List<ObjectId> GroupChildDRs { get; set; } = new List<ObjectId>();
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ScheduleShiftEndDateTime { get; set; }
        public TimeSpan JobTimeZoneOffset { get; set; }
        public bool IsMaxFillAllowed { get; set; }
        public decimal CurretInventory { get; set; }
        public DateTimeOffset? AssignedOn { get; set; }
        public List<string> OptionalPickupIds { get; set; } = new List<string>();
        public bool IsTBD { get; set; }
        public string TBDGroupId { get; set; }
        public string DeliveryDateStartTime { get; set; } = string.Empty;
        public string Vessel { get; set; } = string.Empty;
        public string Berth { get; set; } = string.Empty;
        public bool IsMarine { get; set; } = false;
        public DropdownDisplayItem LockedBy { get; set; }
        public decimal? QuantityInPercent { get; set; }
        public int BlendParentProductTypeId { get;set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime? SelectedDate { get; set; }
        public bool IsFutureDR { get; set; }
        public bool IsCalendarView { get; set; }
        public bool IsDispatcherDragDrop { get; set; } = false;
        public int DispatcherDragDropSequence { get; set; } = 0;
        public int? NumOfSubDrs { get; set; }
        public string DeliveryLevelPO { get; set; } = string.Empty;
        public TimeSpan? ScheduleStartTime { get; set; }
        public TimeSpan? ScheduleEndTime { get; set; }
        public decimal? IndicativePrice { get; set; }
        public string UniqueOrderNo { get; set; }
        public string Sap_OrderNo { get; set; }
    }

    public class CompartmentsInfo
    {
        public ObjectId TrailerId { get; set; }
        public string CompartmentId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class DeliveryWindowInfo
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime RetainDate { get; set; }
        public string RetainTime { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime EndDate { get; set; }
        public string EndTime { get; set; }
       
    }
}
