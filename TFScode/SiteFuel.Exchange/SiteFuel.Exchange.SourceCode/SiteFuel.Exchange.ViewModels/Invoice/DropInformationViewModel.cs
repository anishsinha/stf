using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class DropInformationViewModel
    {
        public int InvoiceId { get; set; }

        public string PoNumber { get; set; }

        public string InvoiceNumber { get; set; }

        public string DDTNumber { get; set; }

        public string DropDate { get; set; }

        public string Quantity { get; set; }

        public string InvoiceAmount { get; set; }

        public bool AllowPoEdit { get; set; }

    }
}
