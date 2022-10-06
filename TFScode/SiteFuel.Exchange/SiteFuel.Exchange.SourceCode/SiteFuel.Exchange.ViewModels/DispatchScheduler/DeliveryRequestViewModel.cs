using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryRequestViewModel
    {
        public DeliveryRequestViewModel()
        {
            DeliveryRequestFor = DeliveryRequestFor.UnKnown;
        }
        public string Id { get; set; }
        public string JobAddress { get; set; }
        public string JobCity { get; set; }
        public string JobName { get; set; }
        public string ProductType { get; set; }
        public int UoM { get; set; }
        public DeliveryReqPriority Priority { get; set; }
        public int AssignedToCompanyId { get; set; }
        public int CreatedByCompanyId { get; set; }
        public int? SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string SiteId { get; set; }
        public decimal RequiredQuantity { get; set; }
        public int? FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public int ProductTypeId { get; set; }
        public int Status { get; set; }
        public int PreviousStatus { get; set; }
        public string CustomerCompany { get; set; }
        public int? DeliveryGroupId { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int? TrackableScheduleId { get; set; }
        public int? OrderId { get; set; }
        public int JobId { get; set; }
        public string CreatedByRegionId { get; set; }
        public string AssignedToRegionId { get; set; }
        public DropdownDisplayItem Terminal { get; set; }
        public DropAddressViewModel BulkPlant { get; set; }
        public int ScheduleStatus { get; set; }
        public int SchedulePreviousStatus { get; set; }
        public string ParentId { get; set; }
        public int NotificationDeliveryStatus { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public bool IsDeleted { get; set; }
        public int TrackScheduleStatus { get; set; }
        public int TrackScheduleEnrouteStatus { get; set; }
        public string TrackScheduleStatusName { get; set; }
        public int StatusClassId { get; set; }
        public decimal TankMaxFill { get; set; }
        public bool IsAutoCreatedDR { get; set; } = false;
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public bool IsCommonBadge { get; set; }
        public string DispactherNote { get; set; }
        public string SourceTripId { get; set; }
        public string PreLoadedFor { get; set; }
        public LoadInfo PreLoadInfo { get; set; }
        public string PostLoadedFor { get; set; }
        public LoadInfo PostLoadInfo { get; set; }
        public string CarrierOrderId { get; set; }
        public string ExternalRefId { get; set; }
        public bool isRecurringSchedule { get; set; }
        public string RecurringScheduleId { get; set; }
        public int ScheduleQuantityType { get; set; }
        public string RecurringScheduleInfo { get; set; }
        public string ScheduleQuantityTypeText { get; set; }
        public RouteInfoDetails RouteInfo { get; set; } = null;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int CarrierStatus { get; set; }
        public List<CompartmentsInfoViewModel> Compartments = new List<CompartmentsInfoViewModel>();
        public bool IsDiverted { get; set; }
        public DeliveryRequestFor DeliveryRequestFor { get; set; }
        public bool IsFilldInvoke { get; set; }
        public string Notes { get; set; }
        public bool IsRetainFuelLoaded { get; set; }
        public string DeliveryWindow { get; set; }
        public int DelReqSource { get; set; }
        public bool IsPreloadDisable { get; set; }
        public string GroupParentDRId { get; set; }
        public List<string> GroupChildDRs { get; set; } = new List<string>();
        public bool IsSpiltDRIconVisible { get; set; }
        public int ProductSequence { get; set; }
        public bool IsBrokered { get; set; }
        public List<DropdownDisplayItem> TrailerTypes { get; set; } = new List<DropdownDisplayItem>();
        public bool IsAcceptNightDeliveries { get; set; }
        public LoadQueueAttributesViewModel LoadQueueAttributes { get; set; } = new LoadQueueAttributesViewModel();
        public DRQueueAttributesViewModel DRQueueAttributes { get; set; } = new DRQueueAttributesViewModel();
        public string HoursToCoverDistance { get; set; }
        public string CustomerBrandId { get; set; }
        public DateTime? ScheduleShiftEndTime { get; set; }
        public TimeSpan JobTimeZoneOffset { get; set; }
        public bool IsMaxFillAllowed { get; set; }
        public string Ullage { get; set; }
        public string CurrentInventory { get; set; }
        public DateTimeOffset? AssignedOn { get; set; }
        public string CarrierRejected { get; set; }
        public string CurrentCarrier { get; set; }
        public string UpcomingCarrier { get; set; }
        public int? DrBrokeredCompanyId { get; set; }
        public bool IsBlendedRequest { get; set; }
        public string BlendedGroupId { get; set; }
        public bool IsTBD { get; set; }
        public string TBDGroupId { get; set; }
        public bool IsAdditive { get; set; }
        public bool IsSpiltDRAdded { get; set; } = false;
        public List<SpiltDRsViewModel> SpiltDRs { get; set; } = new List<SpiltDRsViewModel>();
        //Order Type define LTL : 1 , FTL =2 , ALL =0 
        public int OrderType { get; set; } = 0;
        public string DeliveryDateStartTime { get; set; } = string.Empty;
        public string Vessel { get; set; } = string.Empty;
        public string Berth { get; set; } = string.Empty;
        public bool IsMarine { get; set; } = false;
        public bool IsBlendedDrParent { get; set; } = false;
        public decimal? TotalBlendedQuantity { get; set; }
        public int BlendDrScheduleStatus { get; set; }
        public decimal? QuantityInPercent { get; set; }
        public string BlendedProductName { get; set; }
        public string AdditiveProductName { get; set; }
        public int BlendParentProductTypeId { get; set; }
        public string SelectedDate { get; set; }
        public bool IsFutureDR { get; set; }
        public bool IsCalendarView { get; set; }
        public bool IsDispatcherDragDrop { get; set; } = false;
        public int DispatcherDragDropSequence { get; set; } = 0;
        public string DeliveryLevelPO { get; set; } = string.Empty;
        public string ScheduleStartTime { get; set; }
        public string ScheduleEndTime { get; set; }
        public string Sap_OrderNo { get; set; }
        public string UniqueOrderNo { get; set; }
        public decimal? IndicativePrice { get; set; }
        public string BrokeredParentId { get; set; }
        public string CreditApprovalFilePath { get; set; }
    }


    public class BlendedDrModel
    {
        public bool IsAdditive { get; set; }
        public string Id { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int? TrackableScheduleId { get; set; }
        public int? OrderId { get; set; }
        public DropdownDisplayItem Terminal { get; set; }
        public DropAddressViewModel BulkPlant { get; set; }
        public int ScheduleStatus { get; set; }
        public int SchedulePreviousStatus { get; set; }
        public bool IsDeleted { get; set; }
        public int TrackScheduleStatus { get; set; }
        public int TrackScheduleEnrouteStatus { get; set; }
        public string TrackScheduleStatusName { get; set; }
        public int StatusClassId { get; set; }
    }

    public class TimeZoneOffsetModel
    {
        public int Id { get; set; }
        public string TimeZoneName { get; set; }
        public TimeSpan Offset { get; set; } = TimeSpan.Zero;
    }

    public class LoadInfo
    {
        public string ShiftId { get; set; }
        public int ScheduleIndex { get; set; }
        public int TripIndex { get; set; }
        public string DrId { get; set; }
    }

    public class DeliveryRequestsViewModel : StatusViewModel
    {
        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; } = new List<DeliveryRequestViewModel>();
    }

    public class PreLoadDrViewModel
    {
        public int SbView { get; set; } = 1;
        public int SbDsbView { get; set; } = 1;
        public string RegionId { get; set; }
        public string ShiftEndDate { get; set; }
        public string ShiftId { get; set; }
        public List<TrailerModel> PreloadTrailers { get; set; } = new List<TrailerModel>();
        public List<DropdownDisplayExtendedItem> PreloadDrivers { get; set; } = new List<DropdownDisplayExtendedItem>();
        public bool IsTrailerExists { get; set; }
        public List<DeliveryRequestViewModel> PreloadDrs { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
    }

    public class PreLoadDrResponseViewModel : StatusViewModel
    {
        public List<PreLoadDrModel> PreloadDrs { get; set; } = new List<PreLoadDrModel>();
    }

    public class PreLoadDrModel
    {
        public string Id { get; set; }
        public string PreLoadedForId { get; set; }
    }
    public class RouteInfoDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int LocationSeqNo { get; set; }
    }
    public class RecurringScheduleInfo
    {
        public string Id { get; set; }
        public string SCQtyTypeText { get; set; }
        public int SCQtyType { get; set; }
    }
    public class CompartmentsInfoViewModel
    {
        public string TrailerId { get; set; }
        public string CompartmentId { get; set; }
        public decimal Quantity { get; set; }
        public int UOM { get; set; }
        public string PumpId { get; set; }
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
        public string UniqueOrderNo { get; set; }
    }
    public class ModifiedDeliveryReqs
    {
        public int? OrderId { get; set; }
        public int? TrackableScheduleId { get; set; }
        public int DispatcherDragDropSequence { get; set; }
    }

}
