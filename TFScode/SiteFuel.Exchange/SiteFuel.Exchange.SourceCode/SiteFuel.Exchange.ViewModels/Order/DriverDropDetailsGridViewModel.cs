using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverDropDetailsGridViewModel
    {
        public DriverDropDetailsGridViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
           
        }

        public int DeliveryScheduleId { get; set; }

        public int TrackableScheduleId { get; set; }

        public int OrderId { get; set; }

        public string DriverName { get; set; }

        public int DriverId { get; set; }

        public string PhoneNumber { get; set; }

        public string PONumber { get; set; }

        public string ScheduleDate { get; set; }

        public string ScheduleTime { get; set; }

        public TimeSpan StartTime { get; set; }

        public string FuelType { get; set; }

        public string Quantity { get; set; }

        public string Customer { get; set; }

        public string Location { get; set; }

        public string Status { get; set; }

        public decimal JobLatitude { get; set; }

        public decimal JobLongitude { get; set; }

        public string DeliverySchedule { get; set; }

        public bool IsFtlOrder { get; set; }

        public int EnrouteStatus { get; set; }

        public int CountryId { get; set; }

        public string CountryCode { get; set; }

        public Currency Currency { get; set; }
        public CarrierViewModel Carrier { get; set; }
    }
}
