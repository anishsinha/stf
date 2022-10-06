using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceExportViewModel
    {
        [Display(Name = "Invoice #")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "QB Invoice #")]
        public string QbInvoiceNumber { get; set; }

        [Display(Name = "PO #")]
        public string PoNumber { get; set; }
                
        public string Buyer { get; set; }

        [Display(Name = "Buyer Account Owner")]
        public string BuyerAccountOwner { get; set; }

        [Display(Name = "Supplier")]
        public string Supplier { get; set; }

        [Display(Name = "Supplier Account Owner")]
        public string SupplierAccountOwner { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }

        [Display(Name = "Drop Date")]
        public string DropDate { get; set; }

        [Display(Name = "Drop Time")]
        public string DropTime { get; set; }

        [Display(Name = "Gallons Delivered")]
        public string TotalDroppedGallons { get; set; }

        [Display(Name = "Rack/PPG")]
        public string RackPPG { get; set; }

        [Display(Name = "Invoice Amount")]
        public string TotalInvoiceAmount { get; set; }

        [Display(Name = "Created Date")]
        public string InvoiceDate { get; set; }

        [Display(Name = "Payment Due Date")]
        public string PaymentDueDate { get; set; }

        [Display(Name = "Payment Terms")]
        public string PaymentTerms { get; set; }

        [Display(Name = "Last Edit")]
        public string LastEditDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Brokered")]
        public string IsBrokered { get; set; }
    }
}
