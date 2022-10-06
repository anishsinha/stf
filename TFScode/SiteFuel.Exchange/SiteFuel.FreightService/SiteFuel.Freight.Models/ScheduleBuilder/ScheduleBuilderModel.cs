using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.FreightModels.ScheduleBuilder
{

    public class ScheduleBuilderDetailViewModel : StatusModel
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
        public int Status { get; set; }
        public int UserId { get; set; }
        public string DeletedDriverScheduleMappingId { get; set; }
        public bool isCreateSchedule { get; set; }
        public int WindowMode { get; set; }
        public string DeletedGroupId { get; set; }
        public string DeletedTripId { get; set; }
        public bool IsPreloadSchedule { get; set; } = false;
        public bool IsDriverScheduleReset { get; set; } = false;

    }

    public class ScheduleBuilderViewModel : ScheduleBuilderDetailViewModel
    {
        public List<TripViewModel> Trips { get; set; } = new List<TripViewModel>();
        public List<TrailerModel> Trailers { get; set; } = new List<TrailerModel>();
        public List<ShiftModel> Shifts { get; set; } = new List<ShiftModel>();
        public List<DSBLoadQueueModel> DsbLoadQueueModel { get; set; } = new List<DSBLoadQueueModel>();
    }

    public class CancelScheduleModel
    {
        public List<int> TrackableScheduleIds { get; set; }
        public List<string> DeliveryRequestIds { get; set; }
        public int DriverId { get; set; }
        public List<string> GroupedParentDrIds { get; set; }
        public bool IsCancelAll { get; set; }
    }

    public class DSBSaveModel : ScheduleBuilderDetailViewModel
    {
        public List<ShiftModel> Shifts { get; set; } = new List<ShiftModel>();
        public List<TripViewModel> Trips { get; set; } = new List<TripViewModel>();
        public List<ScheduleApiResponse> ApiResponseModel { get; set; } = new List<ScheduleApiResponse>();
    }

    public class LockDrModel
    {
        public List<string> DrIds { get; set; }
        public DropdownDisplayItem User { get; set; }
    }

    public class DriversViewModel
    {
        public int companyId { get; set; }
        public List<string> trailerTypeId { get; set; }
        public string regionId { get; set; }
        public string selectedDate { get; set; }
        public string shiftId { get; set; }
        public bool IsDsbDriverSchedule { get; set; } = false;
        public string otherRegion { get; set; }
    }

    public class DriverAdditionalDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFilldCompatible { get; set; }
        public string Shifts { get; set; }
    }

    public class TripViewModel
    {
        public string TripId { get; set; }
        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; } = new List<DeliveryRequestViewModel>();
        public List<DriverAdditionalDetailsViewModel> Drivers { get; set; } = new List<DriverAdditionalDetailsViewModel>();
        public List<DriverAdditionalDetailsViewModel> DriverDetails { get; set; } = new List<DriverAdditionalDetailsViewModel>();
        public List<TrailerModel> Trailers { get; set; } = new List<TrailerModel>();
        public string ShiftId { get; set; }
        public int? DriverRowIndex { get; set; }
        public int? DriverColIndex { get; set; }
        public int? TrailerRowIndex { get; set; }
        public int? TrailerColIndex { get; set; }
        public int? ShiftIndex { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public bool IsShiftCollapsed { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string LoadCode { get; set; }
        public string RouteInfo { get; set; }
        public string SupplierSource { get; set; }
        public string Carrier { get; set; }
        public int SlotPeriod { get; set; }
        public long TimeStamp { get; set; }
        public bool IsDsbLoadQueueBackgroundProcess { get; set; } = false;
        
        public TripStatus TripStatus { get; set; }
        public TripStatus TripPrevStatus { get; set; }
        public DeliveryGroupStatus DeliveryGroupPrevStatus { get; set; }
        public DeliveryGroupStatus DeliveryGroupStatus { get; set; }
        public bool IsCommonPickup { get; set; }
        public DropdownDisplayItem Terminal { get; set; } = new DropdownDisplayItem();
        public BulkPlantAddressModel BulkPlant { get; set; } = new BulkPlantAddressModel();
        public int GroupId { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public string DriverScheduleMappingId { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedByName { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public bool IsTrailerExists { get; set; }
        public bool IsDriverScheduleExists { get; set; } = true;
        public bool IsDriverColPriority { get; set; } = false;

        public bool IsIncludeAllRegionDriver { get; set; } = false;
        public bool IsDispatcherDragDropSequence { get; set; } = false;
        public bool IsDispatcherDragDropSequenceModified { get; set; } = false;
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
        [BsonIgnore]
        public List<TrailerFuelRetainViewModel> RetainFuel { get; set; }
    }

    public class ShiftModel
    {
        public string Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int SlotPeriod { get; set; }
        public int OrderNo { get; set; }
        public long Ticks { get; set; }
    }
    public class ShiftOrderModel
    {
        public string Id { get; set; }
        public long Ticks { get; set; }
        public int OrderNo { get; set; }
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

    public class PreLoadDrViewModel
    {
        public int SbView { get; set; } = 1;
        public string RegionId { get; set; }
        public string ShiftEndDate { get; set; }
        public string ShiftId { get; set; }
        public int ShiftIndex { get; set; }
        public int ScheduleIndex { get; set; }
        public int TripIndex { get; set; }
        public List<TrailerModel> PreloadTrailers { get; set; }
        public List<string> PreloadDrs { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
    }

    public class PreLoadDrResponseViewModel: StatusModel
    {
        public List<PreLoadDrModel> PreloadDrs { get; set; } = new List<PreLoadDrModel>();
    }

    public class PreLoadDrModel
    {
        public string Id { get; set; }
        public string PreLoadedForId { get; set; }
    }

    public class UnassignDriverViewModel:StatusModel
    {
        public string sbId { get; set; }
        public int DriverRowIdx { get; set; }
        public List<TripViewModel> Trips { get; set; } = new List<TripViewModel>();
        public List<DropdownDisplayExtendedItem> Drivers { get; set; } = new List<DropdownDisplayExtendedItem>();
        public List<TrailerModel> Trailers { get; set; }
        public int updatedBy { get; set; }
        public long TimeStamp { get; set; }
        public string updatedByName { get; set; }
        public int shiftIdx { get; set; }
    }
}
