using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.MobileAPI;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryScheduleForDriverRequestViewModel
    {
        public int? OrderId { get; set; }

        public int DeliveryScheduleId { get; set; }

        public int TrackableScheduleId { get; set; }

        public string PoNumber { get; set; }

        public string FrDeliveryRequestId { get; set; }

        public decimal GallonsOrdered { get; set; }

        public string CompanyName { get; set; }

        public int? JobId { get; set; }

        public string JobName { get; set; }

        public string JobAddress { get; set; }

        public string JobCity { get; set; }

        public string JobState { get; set; }

        public string JobZip { get; set; }

        public int? CountryId { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int? OrderDeliveryType { get; set; }

        public int? LocationType { get; set; }

        public int TrackableScheduleType { get; set; }

        public int? PostLoadedForId { get; set; }

        public int? UnitOfMeasurement { get; set; }

        public int? Currency { get; set; }

        public bool CustomerSignatureRequired { get; set; }

        public int QuantityTypeId { get; set; }

        public int? ContactPersonId { get; set; }

        public int? SpecialInstructionId { get; set; }

        public string Instruction { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string ContactPersonName { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public bool IsFTL { get; set; }

        public bool IsDriverToUpdateBOL { get; set; }

        public bool IsDropImageRequired { get; set; }

        public bool IsBolImageRequired { get; set; }

        public bool IsBadgeMandatory { get; set; }

        public string FuelType { get; set; }

        public int? FuelTypeId { get; set; }

        public int? ProductTypeId { get; set; }

        public int? OrderTerminalId { get; set; }

        public string OrderTerminalName { get; set; }

        public int? OrderPickUpLocationTerminalId { get; set; }

        public int? SchedulePickUpTerminalId { get; set; }

        public string OrderPickUpLocationTerminalName { get; set; }

        public string SchedulePickUpTerminalName { get; set; }

        public int? OrderPickUpLocationId { get; set; }

        public int? SchedulePickupLocationId { get; set; }

        public decimal? OrderPickUpLocationLatitude { get; set; }
        public decimal? SchedulePickUpLatitude { get; set; }
        public decimal? OrderPickUpLocationLongitude { get; set; }
        public decimal? SchedulePickUpLongitude { get; set; }
        public string SchedulePickupAddress { get; set; }
        public string OrderPickUpLocationAddress { get; set; }
        public string SchedulePickupCity { get; set; }
        public string OrderPickUpLocationCity { get; set; }
        public string SchedulePickUpStateCode { get; set; }
        public string OrderPickUpLocationStateCode { get; set; }
        public string SchedulePickUpZipCode { get; set; }
        public string OrderPickUpLocationZipCode { get; set; }
        public string SchedulePickUpCountryCode { get; set; }
        public string OrderPickUpLocationCountryCode { get; set; }
        public string OrderPickUpLocationCountyName { get; set; }
            
        public string SchedulePickUpCountyName { get; set; }

        public string SupplierSource { get; set; }

        public string SupplierContract { get; set; }

        public string LoadCode { get; set; }

        public string FileDetails { get; set; }

        public int DeliveryGroupId { get; set; }

        public string RouteNote { get; set; }

        public int? CarrierId { get; set; }

        public string CarrierName { get; set; }

        public int? PricingQuantityIndicatorTypeId { get; set; }

        public string FsTrailerDisplayId { get; set; }

        public string LoadNumber { get; set; }

        public ScheduleQuantityType ScheduleQuantityType { get; set; } = ScheduleQuantityType.Quantity;

        public DriverAcknowledgementStatus DeliveryAcknowledgementStatus { get; set; } = DriverAcknowledgementStatus.Unknown;

        public DateTimeOffset ShiftStartDate { get; set; }

        public DateTimeOffset Date { get; set; }

        public string ProductTypeName { get; set; }

        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }

        public string BadgeNo3 { get; set; }

        public string DispatcherNote { get; set; }
        public bool IsCommonBadge { get; set; }

        public string RouteAdditionalInfo { get; set; }
        public string RecurringScheduleInfo { get; set; }
        public bool IsPrePostDipRequired { get; set; }
        public int ProductSequence { get; set; }
        public string CompartmentInfo { get; set; }
        public bool IsFilldInvoke { get; set; }
        public long FilldStopId { get; set; }
        public long FilldDriverId { get; set; }
        public String Notes { get; set; }
        public bool IsMarine { get; set; }
        public string OptionalPickupInfo { get; set; }
        public bool IsOptionalPickup { get; set; }
        public string Berth { get; set; }
        public string BlendGroupId { get; set; }
        public bool IsAdditive { get; set; }
        public bool IsDispatcherDragDrop { get; set; }
        public int DispatcherDragDropSequence { get; set; }
        public string DeliveryLevelPO { get; set; }
        public string IndicativePrice { get; set; }
    }
}
