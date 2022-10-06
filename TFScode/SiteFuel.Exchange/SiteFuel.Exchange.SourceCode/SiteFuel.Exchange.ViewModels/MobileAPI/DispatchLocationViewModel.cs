using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class DispatchLocationViewModel
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public int StateId { get; set; }
        public string CountryCode { get; set; }
        public string CountyName { get; set; }
        public string ZipCode { get; set; }
        public int OrderId { get; set; }
        public int LocationType { get; set; }
        public bool IsValidAddress { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string TimeZoneName { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int?TrackableScheduleId { get; set; }
        public Currency Currency { get; set; }
        public bool IsVariousFobOriginType { get; set; }
        public string SiteName { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public string TerminalName { get; set; }
        public bool IsAddressAvailable { get; set; }
        public PickupLocationType PickupLocationType { get; set; } = PickupLocationType.Terminal;
    }
}
