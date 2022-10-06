using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryScheduleForDriverViewModel
    {
        public DeliveryScheduleForDriverViewModel()
        {
        }

        public int? OrderId { get; set; }

        public int DeliveryScheduleId { get; set; }

        public int TrackableScheduleId { get; set; }
        public bool IsTBD { get; set; }

        public int AssetId { get; set; }

        public string PoNumber { get; set; }

        public decimal GallonsOrdered { get; set; }

        public string CompanyName { get; set; }

        public int? JobId { get; set; }

        public string JobName { get; set; }

        public string FuelType { get; set; }

        public int FuelTypeId { get; set; }

        public int ProductTypeId { get; set; }

        public int TankProductTypeId { get; set; }

        public string JobAddress { get; set; }

        public string JobCity { get; set; }

        public string JobState { get; set; }

        public string JobZip { get; set; }

        public int? LocationType { get; set; }

        public int TrackableScheduleType { get; set; }

        public int? PostLoadedForId { get; set; }

        public string ScheduleDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public long UtcStartTime { get; set; }

        public long UtcEndTime { get; set; }

        public int EnrouteDeliveryStatus { get; set; }

        public DriverDeliveryDetailsViewModel DriverDeliveryDetails { get; set; }

        public List<ApiAddressViewModel> DropLocations { get; set; } = new List<ApiAddressViewModel>();

        public int? OrderDeliveryType { get; set; }

        public int UnitOfMeasurement { get; set; }

        public int OrderUoM { get; set; }

        public int Currency { get; set; }

        public bool CustomerSignatureRequired { get; set; }

        public int QuantityTypeId { get; set; }

        public bool IsFTL { get; set; }

        public bool IsMarine { get; set; }

        public string Berth { get; set; }

        public string Vessle { get; set; }
        
        public bool IsDriverToUpdateBOL { get; set; }

        public bool IsDropImageRequired { get; set; }

        public bool IsBolImageRequired { get; set; }

        public bool IsBadgeMandatory { get; set; }

        public int CompletedType { get; set; }

        public FuelPickUpLocationViewModel FuelPickUpLocation { get; set; }

        public string SupplierSource { get; set; }

        public string SupplierContract { get; set; }

        public string LoadCode { get; set; }

        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public string DispatcherNote { get; set; }
        public bool IsCommonBadge { get; set; }
        public RouteInfoDetails RouteAdditionalInfo { get; set; }
        public RecurringScheduleInfo RecurringScheduleInfo { get; set; }
        public int LocationSeqNo { get; set; }
        public string Carrier { get; set; }

        public int? ScheduleQuantityTypeId { get; set; }

        public string ScheduleQuantityTypeName { get; set; }

        public int DriverAcknowledgementStatus { get; set; } = (int)Utilities.DriverAcknowledgementStatus.Unknown;

        public int NetGrossTypeId { get; set; }

        public OrderDetails OrderDetails { get; set; }
        public long ShiftStartDate { get; set; }

        public string ProductTypeName { get; set; }

        public List<ApiPreLoadBolViewModel> PreLoadBols { get; set; }
        public bool IsPrePostDipEnabled { get; set; }
        public int ProductSequence { get; set; }
        public List<TrailerWithCompartments> TrailerWithCompartments { get; set; }
        public string CompartmentInfo { get; set; }
        public bool IsFilldInvoke { get; set; }
        public long FilldStopId { get; set; }
        public long FilldDriverId { get; set; }
        public string Notes { get; set; }
        public bool IsEbolWorkflowEnabled { get; set; }
        public bool IsOptionalPickup { get; set; } = false;
        public string DeliveryRequestId { get; set; }
        public string GroupedParentDrId { get; set; }
        public bool IsOnGoingScheduleExists { get; set; }
        public List<OptionalPickupInfo> OptionalPickupInfo { get; set; } = new List<OptionalPickupInfo>();
        public string BlendGroupId { get; set; }
        public bool IsAdditive { get; set; }
        public bool IsOnlyAdditiveGroup { get; set; }
        public bool IsDispatcherDragDrop { get; set; }
        public int DispatcherDragDropSequence { get; set; }
        public string DeliveryLevelPO { get; set; }
    }
    public class TrailerWithCompartments
    {
        public string TrailerId { get; set; }
        public string TrailerName { get; set; }
        public List<Compartment> Compartments { get; set; }
    }

    public class DeliveryScheduleGroup
    {
        public DeliveryScheduleGroup()
        {
            DeliverySchedules = new List<DeliveryScheduleForDriverViewModel>();
        }
        public int GroupId { get; set; }
        public string RouteNote { get; set; }
        public string LoadCode { get; set; }
        public string Carrier { get; set; }
        public string FsTrailerDisplayId { get; set; }
        public string LoadNumber { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }

        public string BadgeNo3 { get; set; }

        public bool IsCommonBadge { get; set; }
        public List<DeliveryScheduleForDriverViewModel> DeliverySchedules { get; set; }
        public List<ApiTankDetailViewModel> Tanks { get; set; }
        public List<ApiJobDetailViewModel> Jobs { get; set; }
        public long ShiftStartDate { get; set; }
        public List<TrailerWithCompartments> TrailerWithCompartments { get; set; }
        public bool IsOptionalPickup { get; set; } = false;
        public List<OptionalPickupInfo> OptionalPickupInfo { get; set; } = new List<OptionalPickupInfo>();
        public OptionalPickupAPIInfo OptionalPickupAPIInfo { get; set; }
    }

    public class ScheduleInputDetails
    {
        public int JobId { get; set; }
        public int OrderId { get; set; }
        public int TrackableScheduleId { get; set; }
        public int DeliveryScheduleId { get; set; }
    }

    public class ScheduleOutputDetails
    {
        public List<TankDetailViewModel> TankDetailList { get; set; }
        public List<ScheduleTankViewModel> ScheduleTank { get; set; }
        public List<JobAdditionalDetailsViewModel> JobDetails { get; set; }
    }

    public class ScheduleTankViewModel
    {
        public int AssetId { get; set; }
        public int TrackableScheduleId { get; set; }
        public int DeliveryScheduleId { get; set; }
        public int ProductTypeId { get; set; }
    }
    public class OptionalPickupInfo
    {
        public int FuelTypeId { get; set; }
        public string FuelTypeName { get; set; }
        public List<OptionalPickupTerminalInfo> OptionalPickupTerminalInfo { get; set; } = new List<OptionalPickupTerminalInfo>();
    }
    public class OptionalPickupTerminalInfo
    {
        public string Id { get; set; }
        public int TerminalId { get; set; }
        public bool IsTerminal { get; set; }
        public string TerminalName { get; set; }
        public decimal PickUpLocationLatitude { get; set; }
        public decimal PickUpLocationLongitude { get; set; }

        public string PickUpLocationCountyName { get; set; }

        public string PickUpLocationCountryCode { get; set; }
        public string PickUpLocationZipCode { get; set; }
        public string PickUpLocationStateCode { get; set; }
        public string PickUpLocationCity { get; set; }
        public string PickUpLocationAddress { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }

    }
    public class OptionalPickupScheduleAdditionalInfo
    {
        public string ScheduleBuilderId { get; set; } = string.Empty;
        public string ShiftId { get; set; } = string.Empty;
        public int ShiftIndex { get; set; } = 0;
        public int DriverColIndex { get; set; } = 0;
    }
    public class OptionalPickupAPIInfo
    {
        public string ScheduleBuilderId { get; set; }
        public string ShiftId { get; set; }
        public int DriverColIndex { get; set; }
    }
}
