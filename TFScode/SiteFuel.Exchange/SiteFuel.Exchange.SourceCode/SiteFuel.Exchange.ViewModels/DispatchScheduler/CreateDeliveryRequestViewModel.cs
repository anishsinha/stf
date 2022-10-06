using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class RaiseDeliveryRequestInput
    {
        public int Id { get; set; }
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
        public List<RecurringSchdule> RecurringSchdule { get; set; }
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
        public bool RequestFromBuyerWallyBoard { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public string DispatcherNote { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public DropdownDisplayItem Terminal { get; set; }
        public DropAddressViewModel Bulkplant { get; set; }
        public bool IsAcceptNightDeliveries { get; set; }
        public List<DropdownDisplayItem> TrailerTypes { get; set; }
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
        public List<BlendedRequest> BlendedRequests { get; set; }
        public bool IsCommonPickupForBlend { get; set; }
        public string BlendedGroupId { get; set; }
        public int BlendParentProductTypeId { get; set; }
        public string SelectedDate { get; set; }
        public bool IsFutureDR { get; set; }  
        public bool IsCalendarView { get; set; }
        public int NumOfSubDrs { get; set; }
        public decimal? IndicativePrice { get; set; }
        public string Sap_OrderNo { get; set; }
        public string UniqueOrderNo { get; set; }
        public string ProductShortCode { get; set; }
        public string CreditApprovalFilePath { get; set; }
    }

    public class RaiseDeliveryRequestViewModel : RaiseDeliveryRequestInput
    {
        public int CreatedBy { get; set; }
        public int CreatedByCompanyId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DeliveryReqStatus Status { get; set; } = DeliveryReqStatus.Pending;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int AssignedTo { get; set; }
        public int AssignedToCompanyId { get; set; }
        public int BrokeredOrderId { get; set; }
        public DateTimeOffset AssignedOn { get; set; }
        public DeliveryWindowInfoModel DeliveryWindowInfo { get; set; }
    }
    public class DeliveryWindowInfoModel
    {
        public DateTime RetainDate { get; set; }
        public string RetainTime { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public string EndTime { get; set; }
    }

    public class HeldDeliveryRequestsModel : StatusViewModel
    {
        public List<HeldDeliveryRequestModel> Requests { get; set; } = new List<HeldDeliveryRequestModel>();
    }

    public class HeldDeliveryRequestModel : RaiseDeliveryRequestInput
    {
        public string HeldDrId { get; set; }
        public bool IsDREdited { get; set; }
        public int AssignedToCompanyId { get; set; }
        public int CreatedBy { get; set; }
        public int CreatedByCompanyId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public HeldDrStatus Status { get; set; }
        public CompanyType CompanyType { get; set; }
        public string ValidationMessage { get; set; }
    }

    public class RaiseDeliveryRequest
    {
        public List<RaiseDeliveryRequestInput> DeliveryRequests { get; set; } = new List<RaiseDeliveryRequestInput>();
    }
    public class TankBuyerSupplierViewModel
    {
        public int JobId { get; set; }
        public int ProductTypeId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int OrderId { get; set; }
    }
   
    public class RaiseDeliveryRequestModel : StatusViewModel
    {
        public List<RaiseDeliveryRequestViewModel> DeliveryRequests { get; set; } = new List<RaiseDeliveryRequestViewModel>();
    }

    public class GroupDeliveryRequests
    {
        public int JobId { get; set; }
        public string RegionId { get; set; }
        public int Customer { get; set; }
        public List<DeliveryRequestViewModel> DeliveryReqs { get; set; }
        public List<string> ExistingDrIds { get; set; }
    }

    public class ReCreateDeliveryRequestsViewModel
    {
        public List<string> ExistingDrIds { get; set; }
        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; }
        public bool SkipMarineConversion { get; set; }
    }
    public class RecurringSchdule
    {
        public string Id { get; set; }
        public int ScheduleType { get; set; }
        public string[] WeekDayId { get; set; }
        public int MonthDayId { get; set; }
        public string Date { get; set; }
        public int ScheduleQuantityType { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string TfxCompanyName { get; set; }
        public string TfxBuyerCompanyId { get; set; }
        public string TankName { get; set; }
        public int AssetId { get; set; }
        public string RecurringBlendedGroupId { get; set; }
        public string DeliveryLevelPO { get; set; } = string.Empty;
    }
    public class UpdateRaiseDeliveryRequestInput
    {
        public string DeliveryReqId { get; set; } //from freightservice db
        public string SiteId { get; set; } //from freightservice db
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public int ScheduleQuantityType { get; set; }
        public decimal RequiredQuantity { get; set; }
        public int JobId { get; set; }
        public int FuelTypeId { get; set; }
        public string CustomerCompany { get; set; }
        public string ProductType { get; set; }
        public int? OrderId { get; set; }
        public bool isRecurringSchedule { get; set; }
        public bool IsAcceptNightDeliveries { get; set; }
        public List<DropdownDisplayItem> TrailerTypes { get; set; }
        public LoadQueueAttributesViewModel LoadQueueAttributes { get; set; } = new LoadQueueAttributesViewModel();
        public DRQueueAttributesViewModel DRQueueAttributes { get; set; } = new DRQueueAttributesViewModel();
        public string HoursToCoverDistance { get; set; }


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
    public class BrokeredDeliveryRequestInput
    {
        public string Id { get; set; }
        public int CreatedBy { get; set; }
        public int AssignedTo { get; set; }
        public int OrderId { get; set; }
        public string AssignedToRegionId { get; set; }
        public int AssignedToCompanyId { get; set; }
    }

    public class BlendedRequest
    {
        public string Id { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public string ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public decimal RequiredQuantity { get; set; }
        public decimal QuantityInPercent { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public DropdownDisplayItem Terminal { get; set; }
        public DropAddressViewModel BulkPlant { get; set; }
        public bool IsAdditive { get; set; }
        public string Berth { get; set; }
    }
}
