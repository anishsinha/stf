using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelDispatchLocationViewModel
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int? TrackableScheduleId { get; set; }
        public LocationType LocationType { get; set; }
        public int? TerminalId { get; set; }
        public bool IsFuturePickUp { get; set; }
        public PickUpAddressViewModel PickupLocation { get; set; }
        public Currency Currency { get; set; }
        public string TimeZoneName { get; set; }
        public DropAddressStatus DropStatus { get; set; }
        public bool IsJobLocation { get; set; }
        public string SiteName { get; set; }
        public int? ParentId { get; set; }
        public bool IsSkipped { get; set; }
        public int? DeliveryGroupId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
    }
}
