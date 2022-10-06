using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceBolGridViewModel : StatusViewModel
    {
        public int Id { get; set; }

        public string BolNumber { get; set; }

        public string InvoiceNumber { get; set; }

        public string Customer { get; set; }

        public string LiftDate { get; set; }

        public string DropEndDate { get; set; }

        public string PickupLocation { get; set; }

        public string ShipTo { get; set; }

        public string DropLocation { get; set; }

        public decimal? GrossQuantity { get; set; }

        public decimal? NetQuantity { get; set; }

        public decimal ActualDropQuantity { get; set; }

        public string InvoiceCreateMethod { get; set; }

        public string DropTicketNumber { get; set; }

        public string Status { get; set; }

        public int TotalCount { get; set; }
        public string BadgeNumber { get; set; }
        public string TerminalName { get; set; }
        public string PrePostValues { get; set; }
    }
}
