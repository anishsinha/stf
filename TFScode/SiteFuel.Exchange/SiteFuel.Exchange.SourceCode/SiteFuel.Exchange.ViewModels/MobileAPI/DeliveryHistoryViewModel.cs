using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryHistoryViewModel
    {
        public DeliveryHistoryViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {

        }

        public DateTimeOffset Date { get; set; }

        public string Time { get; set; }

        public decimal Quantity { get; set; }

        public int  InvoiceId { get; set; }

        public string InvoiceDisplayName { get; set; }
    }
}
