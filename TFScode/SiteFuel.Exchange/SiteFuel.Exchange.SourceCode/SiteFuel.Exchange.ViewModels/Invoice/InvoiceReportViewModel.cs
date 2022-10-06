using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceReportViewModel
    {
        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; }

        public string InvoiceDate { get; set; }

        public decimal InvoiceAmount { get; set; }

        public decimal FuelAmount { get; set; }

        public decimal StateSalesTax { get; set; }

        public decimal StateTax { get; set; }

        public decimal FederalTax { get; set; }

        public decimal DeliveryAmount { get; set; }

        public string FuelType { get; set; }

        public string JobName { get; set; }
        public int TotalCount { get; set; }



    }

    public class InvoiceReconcilationViewModel
    {
        public int InvoiceId { get; set; }
        public string Customer { get; set; }
        public string DropLocation { get; set; }
        public string InvoiceNumber { get; set; }
        public string ReferenceId { get; set; }
        public string PoNumber { get; set; }
        public string DropDate { get; set; }
        public string InvoiceDate { get; set; }
        public string FuelType { get; set; }
        public decimal Quantity { get; set; }
        public string PaymentDueDate { get; set; }
        public int TotalCount { get; set; }
    }
}
