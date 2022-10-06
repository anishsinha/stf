using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryGridViewModel : StatusViewModel
    {
        public DeliveryGridViewModel()
        {
            InstanceInitialize();
        }

        public DeliveryGridViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Utilities.Status status = Utilities.Status.Failed)
        {
            
        }

        public int InvoiceId { get; set; }

        public int OrderId { get; set; }

        public string Invoice { get; set; }

        public string Po { get; set; }

        public string DropDateTime { get; set; }

        public decimal AmountDropped { get; set; }

        public decimal Overage { get; set; }

        public string OverageAmount { get; set; }

        public string Quantity { get; set; }

        public string DropType { get; set; }

        public string DriverName { get; set; }

        public string Location { get; set; }
    }
}
