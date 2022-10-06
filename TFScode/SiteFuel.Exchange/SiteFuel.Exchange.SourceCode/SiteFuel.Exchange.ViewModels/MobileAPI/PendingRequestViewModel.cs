using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class PendingRequestViewModel 
    {
        public PendingRequestViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
        }

        public int FuelRequestId { get; set; }

        public string FuelType { get; set; }

        public decimal Quantity { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public Nullable<DateTimeOffset> EndDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int QuantityTypeId { get; set; }
    }
}
