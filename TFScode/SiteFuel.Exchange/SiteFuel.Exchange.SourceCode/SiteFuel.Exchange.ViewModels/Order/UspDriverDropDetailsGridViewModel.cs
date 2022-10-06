using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspDriverDropDetailsGridViewModel
    {
        public int DeliveryScheduleId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public int OrderId { get; set; }

        public string DriverName { get; set; }

        public int DriverId { get; set; }

        public string PhoneNumber { get; set; }

        public string PoNumber { get; set; }

        public JobLocationTypes LocationType { get; set; }

        public string BuyerCompanyName { get; set; }

        public DateTimeOffset ScheduleDate { get; set; }

        public decimal ScheduleQuantity { get; set; }

        public string ScheduleStartTime { get; set; }

        public string ScheduleEndTime { get; set; }

        public string FuelType { get; set; }

        public decimal JobLatitude { get; set; }

        public decimal JobLongitude { get; set; }

        public string JobAddress { get; set; }

        public string JobCity { get; set; }

        public string JobStateCode { get; set; }

        public string JobZipCode { get; set; }

        public int CountryId { get; set; }

        public string CountryCode { get; set; }

        public Currency Currency { get; set; }

        public string DeliveryScheduleStatus { get; set; }

        public int DeliveryScheduleStatusId { get; set; }

        public bool IsFTL { get; set; }

        public int? AppLocationStatusId { get; set; }

        public int? CarrierId { get; set; }

        public string CarrierName { get; set; }
    }
}
