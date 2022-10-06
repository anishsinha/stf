using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ScheduleBuilderDetailViewModel : StatusViewModel
    {
        public string Id { get; set; }
        public int CompanyId { get; set; }
        public string RegionId { get; set; }
        public int ObjectFilter { get; set; } = 1;
        public int RegionFilter { get; set; } = 1;
        public int DateFilter { get; set; }
        public int DSBFilter { get; set; } = 1;
        public int IsNoDriverShiftFound { get; set; } = 0;
        public string Date { get; set; }
        public long TimeStamp { get; set; }
        public DSBMethod Status { get; set; }
        public int UserId { get; set; }
        public string DeletedTripId { get; set; }
        public int? DeletedGroupId { get; set; }
        public bool IsLoadReset { get; set; }
        public int WindowMode { get; set; } = 1;
        public int ToggleRequestMode { get; set; } = 1;
        public string DeletedDriverScheduleMappingId { get; set; }

        public bool isCreateSchedule { get; set; }

        public bool IsPreloadSchedule { get; set; } = false;
        public bool IsDriverScheduleReset { get; set; } = false;
        public bool IsDsbDriverSchedule { get; set; }
    }

    public class ScheduleBuilderViewModel : ScheduleBuilderDetailViewModel
    {
        public List<TripViewModel> Trips { get; set; } = new List<TripViewModel>();
        public List<ShiftModel> Shifts { get; set; } = new List<ShiftModel>();
        public List<TrailerModel> Trailers { get; set; } = new List<TrailerModel>();
        public List<DSBLoadQueueModel> DsbLoadQueueModel { get; set; } = new List<DSBLoadQueueModel>();

    }

    public class SbDriverViewModel : ScheduleBuilderDetailViewModel
    {
        public List<ScheduleShiftViewModel> Shifts { get; set; } = new List<ScheduleShiftViewModel>();
    }
    public class CalendarScheduleViewModel
    {
        public string Date { get; set; }
        public string RegionId { get; set; }
        public string ShiftId { get; set; }
        public int DriverRowIndex { get; set; }
        public int DriverColIndex { get; set; }
        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; }

    }
    public class ScheduleShiftViewModel
    {
        public string Id { get; set; }
        public List<DriverModel> Schedules { get; set; } = new List<DriverModel>();
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsCollapsed { get; set; }
        public int SlotPeriod { get; set; }
        public int OrderNo { get; set; }

    }

    public class DriverModel
    {
        public bool AllowDriverChange { get; set; } = true;
        public List<TripViewModel> Trips { get; set; } = new List<TripViewModel>();
        public List<DriverAdditionalDetailsViewModel> Drivers { get; set; } = new List<DriverAdditionalDetailsViewModel>();
        public List<TrailerModel> Trailers { get; set; } = new List<TrailerModel>();
        public bool IsLoadQueueCollapsed { get; set; } = false;
        public string LoadQueueId { get; set; } = string.Empty;
        public bool IsLoadQueueColumnBlocked { get; set; } = false;
        public bool IsIncludeAllRegionDriver { get; set; } = false;


        public int LoadQueueColumnStatus { get; set; } = -1;
        public bool IsDriverScheduleExists { get; set; } = true;
        public int DriverRowIndex { get; set; }
    }

    public class DSBSaveModel : ScheduleBuilderDetailViewModel
    {
        public List<ShiftModel> Shifts { get; set; } = new List<ShiftModel>();
        public List<TripViewModel> Trips { get; set; } = new List<TripViewModel>();
        public List<string> PreloadedDRs { get; set; } = new List<string>();
        public List<string> PostloadedDRs { get; set; } = new List<string>();
        public List<ScheduleApiResponse> ApiResponseModel { get; set; } = new List<ScheduleApiResponse>();
    }

    public class ScheduleApiResponse
    {
        public string RequestCarrierOrderId { get; set; }
        public string ResponseCarrierOrderId { get; set; }
        public int TrackableScheduleId { get; set; }
    }

    public class DRDragDropModel : ScheduleBuilderDetailViewModel
    {
        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; }
        public TripViewModel SourceTrip { get; set; }
        public TripViewModel DestinationTrip { get; set; }
    }

    public class SbTrailerViewModel : ScheduleBuilderDetailViewModel
    {
        public List<TrailerViewModel> Trailers { get; set; } = new List<TrailerViewModel>();
        public List<ShiftModel> Shifts { get; set; } = new List<ShiftModel>();
    }

    public class TrailerViewModel : TrailerModel
    {
        public List<TrailerShiftModel> Shifts { get; set; } = new List<TrailerShiftModel>();
    }

    public class TrailerModel
    {
        public string Id { get; set; }
        public string TrailerId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Compartments { get; set; }
        public string TrailerType { get; set; }
        public bool IsFilldCompatible { get; set; }
        public decimal FuelCapacity { get; set; }
        public decimal OptimizedCapacity { get; set; }
        public List<TrailerFuelRetainViewModel> RetainFuel { get; set; }
    }

    public class TrailerShiftModel
    {
        public string ShiftId { get; set; }
        public string StartTime { get; set; }
        public int SlotPeriod { get; set; }
        public string EndTime { get; set; }
        public List<TripViewModel> Trips { get; set; } = new List<TripViewModel>();
    }

    public class TripViewModel
    {
        public string TripId { get; set; }
        public int GroupId { get; set; }
        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; } = new List<DeliveryRequestViewModel>();
        public List<DriverAdditionalDetailsViewModel> Drivers { get; set; } = new List<DriverAdditionalDetailsViewModel>();
        public List<TrailerModel> Trailers { get; set; } = new List<TrailerModel>();
        public string ShiftId { get; set; }
        public int? DriverRowIndex { get; set; }
        public int? DriverColIndex { get; set; }
        public int? TrailerRowIndex { get; set; }
        public int? TrailerColIndex { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public bool IsShiftCollapsed { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string LoadCode { get; set; }
        public string RouteInfo { get; set; }
        public int? ShiftIndex { get; set; }
        public string SupplierSource { get; set; }
        public string Carrier { get; set; }
        public TripStatus TripStatus { get; set; }
        public TripStatus TripPrevStatus { get; set; }
        public DeliveryGroupStatus DeliveryGroupStatus { get; set; }
        public DeliveryGroupStatus DeliveryGroupPrevStatus { get; set; }
        public bool IsCommonPickup { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public DropdownDisplayItem Terminal { get; set; }
        public DropAddressViewModel BulkPlant { get; set; }
        public bool IsEditable { get; set; }
        public string DriverScheduleMappingId { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public bool IsCommonBadge { get; set; }
        public long TimeStamp { get; set; }
        public bool IsDsbLoadQueueBackgroundProcess { get; set; } = false;

        public int SlotPeriod { get; set; }
        public string UpdatedByName { get; set; }
        public bool IsTrailerExists { get; set; }
        public bool IsDriverScheduleExists { get; set; }
        public bool IsIncludeAllRegionDriver { get; set; } // need property at schedule level.
        public bool IsDispatcherDragDropSequence { get; set; } = false;
        public bool IsDispatcherDragDropSequenceModified { get; set; } = false;
    }
    public class TrailersDeliveryRequestViewModel
    {
        public List<TrailerModel> trailers { get; set; }
        public List<DeliveryRequestViewModel> deliveryRequests { get; set; }
        public int ShiftIndex { get; set; }
        public int ScheduleIndex { get; set; }
    }

    public class TrailerJobNonCompatibleDrs
    {
        public int DrCount { get; set; }
        public int ShiftIndex { get; set; }
        public int ScheduleIndex { get; set; }
    }

    public class UnassignDriverViewModel : StatusViewModel
    {
        public string sbId { get; set; }
        public int DriverRowIdx { get; set; }
        public List<TripViewModel> Trips { get; set; }
        public List<DropdownDisplayExtended> Drivers { get; set; }
        public List<TrailerModel> Trailers { get; set; }
        public int Updatedby { get; set; }
        public long TimeStamp { get; set; }
        public string updatedByName { get; set; }
        public int shiftIdx { get; set; }
    }
    public class CancelDeliverySchedule
    {
        public string ScheduleBuilderId { get; set; }
        public int DriverId { get; set; }
        public int ShiftIndex { get; set; }
        public string ShiftId { get; set; }
        public int DriverRowIndex { get; set; }
        public int DriverColIndex { get; set; }
        public string DeliveryReqId { get; set; }
        public int TrackableScheduleId { get; set; }
        public int UserId { get; set; }
        public int ScheduleStatus { get; set; }
        public int TrackScheduleStatus { get; set; }
        public string TrackScheduleStatusName { get; set; }
        public int StatusClassId { get; set; }
    }
    public class CancelDSDeliveryScheduleInfo
    {
        public int TfxCompanyId { get; set; }
        public string RegionId { get; set; }
        public List<CancelDSDeliverySchedule> CancelDSDeliverySchedules { get; set; } = new List<CancelDSDeliverySchedule>();
    }
    public class CancelDSDeliverySchedule
    {
        public string DeliveryReqId { get; set; }
        public bool IsSubDR { get; set; } = false;
        public bool IsPreLoadDR { get; set; } = false;
    }

    public class SubDRStatus
    {
        public string GroupParentDRId { get; set; }
        public int DeliveryScheduleStatusId { get; set; }
    }
    public class CancelDSDeliveryScheduleViewModel
    {
        public bool IsChecked { get; set; } = false;
        public string ScheduleBuilderDate { get; set; }
        public string ScheduleBuilderId { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string Quantity { get; set; }
        public string FuelType { get; set; }
        public string CurrentState { get; set; }
        public string ShiftId { get; set; }
        public int ShiftIndex { get; set; }
        public int DriverRowIndex { get; set; }
        public int DriverColIndex { get; set; }
        public string DeliveryReqId { get; set; }
        public int TrackableScheduleId { get; set; }

    }

    public class ShiftDataModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<IndexModel> Indexes { get; set; }
    }


    public class IndexModel
    {
        public int LoadIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string LoadTime { get; set; }
        public string Driver { get; set; }
    }
}