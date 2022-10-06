using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryScheduleForJobViewModel
    {
        public DeliveryScheduleForJobViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {

        }

        public int OrderId { get; set; }

        public int DeliveryRequestId { get; set; }

        public string FuelType { get; set; }

        public string PoNumber { get; set; }

        public decimal GallonsOrdered { get; set; }

        public int QuantityTypeId { get; set; }

        public DateTimeOffset ScheduleDate { get; set; }

        public string ScheduleStartTime { get; set; }

        public string ScheduleEndTime { get; set; }

        public int DriverId { get; set; }

        public string ScheduleDetails { get; set; }

        public string PhoneNumber { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public UoM UoM { get; set; }

        public int CountryId { get; set; }

        public DeliveryDetailsViewModel DeliveryDetails { get; set; } = new DeliveryDetailsViewModel();
    }
}
