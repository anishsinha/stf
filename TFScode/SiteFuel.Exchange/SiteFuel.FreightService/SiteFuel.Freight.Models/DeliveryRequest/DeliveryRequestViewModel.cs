using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels.DeliveryRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class DeliveryRequestViewModel : CommonFieldsModel
    {
        public string Id { get; set; }
        public string JobAddress { get; set; }
        public string JobName { get; set; }
        public string JobCity { get; set; }
        public string ProductType { get; set; }
        public int UoM { get; set; }
        public DeliveryReqPriority Priority { get; set; } = DeliveryReqPriority.MustGo;
        public decimal CurrentThreshold { get; set; }
        public int AssignedToCompanyId { get; set; }
        public int CreatedByCompanyId { get; set; }
        public int? SupplierCompanyId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string SiteId { get; set; }
        public int? FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public int ProductTypeId { get; set; }
        public decimal RequiredQuantity { get; set; }
        public DeliveryReqStatus Status { get; set; } = DeliveryReqStatus.Pending;
        public DeliveryReqStatus PreviousStatus { get; set; }
        public decimal CurrentQuantity { get; set; }
        public string CreditApprovalFilePath { get; set; }
        public string CreatedByRegionId { get; set; }
        public string AssignedToRegionId { get; set; }
        //public int AssignedTo { get; set; }
        public int JobId { get; set; }
        public string CustomerCompany { get; set; }
        public int? DeliveryGroupId { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int? TrackableScheduleId { get; set; }
        public int? OrderId { get; set; }
        public DropdownDisplayItem Terminal { get; set; } = new DropdownDisplayItem();
        public BulkPlantAddressModel BulkPlant { get; set; } = new BulkPlantAddressModel();
        public int ScheduleStatus { get; set; }
        public string TrackScheduleStatusName { get; set; }
        public int StatusClassId { get; set; }
        public int SchedulePreviousStatus { get; set; }
        public int TrackScheduleStatus { get; set; }
        public int TrackScheduleEnrouteStatus { get; set; }
        public int NotificationDeliveryStatus { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public decimal TankMaxFill { get; set; }
        public int WindowMode { get; set; } = 1;
        public int QueueMode { get; set; } = 1;
        public AutoDrStatus AutoDRStatus { get; set; }
        public DateTimeOffset? AutoCreatedOn { get; set; }
        public DateTimeOffset? AutoUpdatedOn { get; set; }
        public bool IsAutoCreatedDR { get; set; } = false;
        public string ParentId { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public bool IsCommonBadge { get; set; }
        public string DispactherNote { get; set; }
        public string TripId { get; set; }
        public string SourceTripId { get; set; }
        public string PreLoadedFor { get; set; }
        public LoadInfo PreLoadInfo { get; set; }
        public string PostLoadedFor { get; set; }
        public LoadInfo PostLoadInfo { get; set; }
        public DRSource DelReqSource { get; set; }
        public string CarrierOrderId { get; set; }
        public string ExternalRefId { get; set; }
        public bool isRecurringSchedule { get; set; }
        public string RecurringScheduleId { get; set; }
        public int ScheduleQuantityType { get; set; }
        public string RecurringScheduleInfo { get; set; }
        public string ScheduleQuantityTypeText { get; set; }
        public List<RecurringDRSchdule> RecurringSchdule { get; set; }
        public int BuyerCompanyId { get; set; }
        public string PoNumber { get; set; }
        public RouteInfo RouteInfo { get; set; } = null;
        public string BrokeredDrId { get; set; }
        public int? BrokeredOrderId { get; set; }
        public int CarrierStatus { get; set; }
        public bool IsDispatchRetainedByCustomer { get; set; } = false;
        public List<CompartmentsInfoViewModel> Compartments = new List<CompartmentsInfoViewModel>();
        public DeliveryRequestFor DeliveryRequestFor { get; set; }
        public bool IsFilldInvoke { get; set; }
        public string Notes { get; set; }
        public bool IsRetainFuelLoaded { get; set; }
        public string DeliveryWindow { get; set; }
        public DeliveryWindowInfoModel DeliveryWindowInfo { get; set; }
        public bool IsPreloadDisable { get; set; } = false;
        public string GroupParentDRId { get; set; }
        public List<string> GroupChildDRs { get; set; } = new List<string>();
        public bool IsSpiltDRIconVisible { get; set; } = true;
        public List<DropdownDisplayItem> TrailerTypes { get; set; } = new List<DropdownDisplayItem>();
        public bool IsAcceptNightDeliveries { get; set; }
        public string HoursToCoverDistance { get; set; }
        public DateTime? ScheduleShiftEndTime { get; set; }
        public TimeSpan JobTimeZoneOffset { get; set; }
        public bool IsMaxFillAllowed { get; set; }
        public string CurrentInventory { get; set; }
        public string CarrierRejected { get; set; }
        public string CurrentCarrier { get; set; }
        public string UpcomingCarrier { get; set; }
        public string Ullage { get; set; }
        public DateTimeOffset? AssignedOn { get; set; }
        public int? DrBrokeredCompanyId { get; set; }
        public bool IsTBD { get; set; }
        public string TBDGroupId { get; set; }
        public string DeliveryDateStartTime { get; set; } = string.Empty;
        public string Vessel { get; set; } = string.Empty;
        public string Berth { get; set; } = string.Empty;
        public bool IsMarine { get; set; } = false;
        public bool IsBlendedRequest { get; set; }
        public int BlendParentProductTypeId { get; set; }
        public bool IsAdditive { get; set; }
        public string BlendedGroupId { get; set; }
        public bool IsBlendedDrParent { get; set; } = false;
        public decimal? TotalBlendedQuantity { get; set; }
        public string BlendedProductName { get; set; }
        public decimal? QuantityInPercent { get; set; }
        public string AdditiveProductName { get; set; }
        public int BlendDrScheduleStatus { get; set; }
        public string SelectedDate { get; set; }
        public bool IsFutureDR { get; set; }
        public bool IsCalendarView { get; set; }
        public bool IsDispatcherDragDrop { get; set; } = false;
        public int DispatcherDragDropSequence { get; set; } = 0;
        public int NumOfSubDrs { get; set; }
        public string DeliveryLevelPO { get; set; } = string.Empty;
        public string ScheduleStartTime { get; set; }
        public string ScheduleEndTime { get; set; }
        public decimal? IndicativePrice { get; set; }
        public string UniqueOrderNo { get; set; }
        public string Sap_OrderNo { get; set; }
        public string BrokeredParentId { get; set; }
    }

    public class LoadInfo
    {
        public string ShiftId { get; set; }
        public int ScheduleIndex { get; set; }
        public int TripIndex { get; set; }
        public string DrId { get; set; }
    }

    public class SalesOrderStatusModel
    {
        public string SAP_Order_No { get; set; }
        public string ExternalOrderNo { get; set; }
        public string SAP_Order_Status { get; set; }
        public SapProductModel[] Products { get; set; }
    }

    public class OverrideCreditCheckApprovalModel
    {
        public int UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string HeldDRId { get; set; }
    }

    public class SapProductModel
    {
        public string ProductID { get; set; }
        public string OrderQuantity { get; set; }
        public string Price { get; set; }
    }

    public class DeliveryRequestsViewModel : StatusModel
    {
        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; } = new List<DeliveryRequestViewModel>();
    }

    public class ReCreateDeliveryRequestsViewModel
    {
        public List<string> ExistingDrIds { get; set; }
        public int UserId { get; set; }
        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; }
    }
    public class RecurringDRSchdule
    {
        public string Id { get; set; }
        public int ScheduleType { get; set; }
        public List<string> WeekDayId { get; set; }
        public int MonthDayId { get; set; }
        public string Date { get; set; }
        public decimal RequiredQuantity { get; set; }
        public int ScheduleQuantityType { get; set; }
        public int? OrderId { get; set; }
        public string PoNumber { get; set; }
        public string SiteId { get; set; }
        public int JobId { get; set; }
        public int? TfxSupplierCompanyId { get; set; }
        public string TfxCompanyName { get; set; }
        public int TfxUserId { get; set; }
        public int AssignedToCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public bool isIgnoreRecord { get; set; } = false; // any change in record then isIgnoreRecord=true. and if empty record then isIgnoreRecord=true.
        public string TankName { get; set; }
        public int AssetId { get; set; }
        public int? ProductTypeId { get; set; }
        public DeliveryRequestFor DeliveryRequestFor { get; set; }
        public bool IsBlendedRequest { get; set; } = false;
        public string BlendedGroupId { get; set; } = string.Empty;
        public string RecurringBlendedGroupId { get; set; }
        public string DeliveryLevelPO { get; set; }
    }
    public class RouteInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int LocationSeqNo { get; set; } = 0;
    }

    public class HeldDeliveryRequestsModel : StatusModel
    {
        public List<HeldDeliveryRequestModel> Requests { get; set; } = new List<HeldDeliveryRequestModel>();
    }

    public class HeldDeliveryRequestModel
    {
        public string Sap_OrderNo { get; set; }
        public string Sap_Order_Status { get; set; }
        public string HeldDrId { get; set; }
        public string UniqueOrderNo { get; set; }
        public string SiteId { get; set; } //from freightservice db
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public int ScheduleQuantityType { get; set; }
        public decimal RequiredQuantity { get; set; }
        public int JobId { get; set; }
        public decimal QuantityInPercent { get; set; }
        public bool IsAdditive { get; set; }
        public int ProductTypeId { get; set; }
        public int? FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public string JobName { get; set; }
        public string JobAddress { get; set; }
        public string JobCity { get; set; }
        public string FileName { get; set; }
        public string CreditApprovalFilePath { get; set; }
        public string CustomerCompany { get; set; }
        public string ProductType { get; set; }
        public int UoM { get; set; }
        public string CreatedByRegionId { get; set; }
        public string AssignedToRegionId { get; set; }
        public DeliveryReqPriority Priority { get; set; } = DeliveryReqPriority.MustGo;
        public decimal CurrentThreshold { get; set; }
        public decimal TankMaxFill { get; set; }
        public int? OrderId { get; set; }
        public DRSource DelReqSource { get; set; } = DRSource.Manual;
        public bool isRecurringSchedule { get; set; }
        public string PoNumber { get; set; }
        //public List<RecurringSchdule> RecurringSchdule { get; set; }
        public int BuyerCompanyId { get; set; }
        public string DispactherNote { get; set; }
        public string BrokeredDrId { get; set; }
        public bool IsDispatchRetainedByCustomer { get; set; } = false;
        public DeliveryRequestFor DeliveryRequestFor { get; set; }
        public string Notes { get; set; }
        public bool isRetainInfo { get; set; }
        public bool isTankExists { get; set; }
        public bool IsRetainButtonClick { get; set; }
        public string RetainTime { get; set; }
        public string RetainDate { get; set; }
        public string WindowStartTime { get; set; }
        public string WindowStartDate { get; set; }
        public string WindowEndTime { get; set; }
        public string WindowEndDate { get; set; }
        public int? SupplierCompanyId { get; set; }
        public int AssignedToCompanyId { get; set; }
        public bool RequestFromBuyerWallyBoard { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public string DispatcherNote { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public DropdownDisplayItem Terminal { get; set; }
        public BulkPlantAddressModel Bulkplant { get; set; }
        public bool IsAcceptNightDeliveries { get; set; }
        public string HoursToCoverDistance { get; set; }
        public TimeSpan JobTimeZoneOffset { get; set; }
        public bool IsMaxFillAllowed { get; set; }
        public bool IsReAssignToCarrier { get; set; }
        public bool IsTBD { get; set; }
        public string TBDGroupId { get; set; }
        public int UserId { get; set; }
        public string DeliveryDateStartTime { get; set; } = string.Empty;
        public string Vessel { get; set; } = string.Empty;
        public string Berth { get; set; } = string.Empty;
        public bool IsMarine { get; set; } = false;
        public string DeliveryLevelPO { get; set; } = string.Empty;
        public string ScheduleStartTime { get; set; }
        public string ScheduleEndTime { get; set; }
        //BLENDED REQUEST
        public bool IsBlendedRequest { get; set; }
        //public List<BlendedRequest> BlendedRequests { get; set; }
        public bool IsCommonPickupForBlend { get; set; }
        public string BlendedGroupId { get; set; }
        public int BlendParentProductTypeId { get; set; }
        public string SelectedDate { get; set; }
        public bool IsFutureDR { get; set; }
        public bool IsCalendarView { get; set; }
        public int NumOfSubDrs { get; set; }
        public decimal? IndicativePrice { get; set; }
        public int CreatedBy { get; set; }
        public int CreatedByCompanyId { get; set; }
        public CompanyType CompanyTypeId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public HeldDrStatus Status { get; set; }
        public bool IsDREdited { get; set; }
        public int UpdatedBy { get; set; }
        public int UpdatedByCompanyId { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }

    public class CompartmentsInfoViewModel
    {
        public string TrailerId { get; set; }
        public string CompartmentId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class BlendDrArray
    {
        public int OrderId { get; set; }
        public string ParentDrId { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string ProductType { get; set; }
        public string FuelType { get; set; }
        public int UoM { get; set; }
    }

    public class SplitDrArray
    {
        public List<BlendDrArray> BlendDrArray { get; set; }
        public string BlendGroupId { get; set; }
    }

    public class SplitDeliveryRequestModel
    {
        public string ParentDRId { get; set; }
        public List<RequiredQtyDetails> RequiredQtyDetails { get; set; }
        public int UserId { get; set; }
    }
    public class RequiredQtyDetails
    {
        public decimal RequiredQty { get; set; }
        public string DeliveryLevelPO { get; set; }
        public string UniqueOrderNo { get; set; }
    }

    public class TankDeliveryRequestMessageModel
    {
        public string EntityId { get; set; }
    }

    public class CarrierExceedsDeliveryModel
    {
        public int TfxCarrierCompanyId { get; set; }

        public int TfxJobId { get; set; }

        public int TfxProductId { get; set; }
    }

    public class DeliveryRequestsByJobModel
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public DeliveryReqPriority Priority { get; set; }
        public DeliveryRequestViewModel[] DeliveryRequests { get; set; }
    }

    public class DeliveryReqJobInfoModel
    {
        public int JobId { get; set; }
        public List<DropdownDisplayItem> TrailerTypes { get; set; } = new List<DropdownDisplayItem>();
        public bool IsAcceptNightDeliveries { get; set; }
        public string HoursToCoverDistance { get; set; }
    }
    public class BrokeredDeliveryRequestInput
    {
        public string Id { get; set; }
        public int CreatedBy { get; set; }
        public int AssignedTo { get; set; }
        public int OrderId { get; set; }
        public string AssignedToRegionId { get; set; }
        public int AssignedToCompanyId { get; set; }
    }
    public class SpiltDeliveryRequestViewModel
    {
        public string GroupParentDrId { get; set; }
        public int UserId { get; set; }
        public List<SpiltDRsViewModel> SpiltDRsViewModel { get; set; } = new List<SpiltDRsViewModel>();

    }
    public class SpiltDRsViewModel
    {
        public int ScheduleQuantityType { get; set; }
        public decimal RequiredQuantity { get; set; }
    }

    public class CalendarRequestModel
    {
        public List<string> deliveryRequestIds { get; set; }
        public bool isCalendarView { get; set; }
        public int userId { get; set; }
    }
    public class CalendarFilterModel
    {
        public List<string> Customers { get; set; } = new List<string>();
        public List<int> Locations { get; set; } = new List<int>();
        public List<string> Vessels { get; set; } = new List<string>();
        public bool LocationType { get; set; }
        public List<int> Priorities { get; set; } = new List<int>();
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
