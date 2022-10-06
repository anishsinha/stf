using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryDetailsViewModel
    {
        public DeliveryDetailsViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {

        }

        public DateTimeOffset ScheduleDate { get; set; }

        public string ScheduleStartTime { get; set; }

        public string ScheduleEndTime { get; set; }

        public string DriverName { get; set; }

        public string PhoneNumber { get; set; }

        public decimal DriverLatitude { get; set; }

        public decimal DriverLongitude { get; set; }

        public string JobName { get; set; }

        public string PONumber { get; set; }

        public decimal JobLatitude { get; set; }

        public decimal JobLongitude { get; set; }

        public string JobAddress { get; set; }

        public int CountryId { get; set; }

        public bool IsDriverStartedDelivery { get; set; }

        public long ScheduleDateTime { get; set; }

        public int? StatusId { get; set; }
    }
}
