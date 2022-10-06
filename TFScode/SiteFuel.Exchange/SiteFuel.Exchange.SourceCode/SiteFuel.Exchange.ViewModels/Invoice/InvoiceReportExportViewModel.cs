using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceReportExportViewModel
    {
        [Display(Name = "Invoice Total Amount")]
        public string InvoiceAmount { get; set; }

        [Display(Name = "Fuel Amount")]
        public string FuelAmount { get; set; }

        [Display(Name = "State Sales Tax")]
        public string StateSalesTax { get; set; }

        [Display(Name = "State Tax")]
        public string StateTax { get; set; }

        [Display(Name = "Federal Tax")]
        public string FederalTax { get; set; }

        [Display(Name = "Delivery Amount")]
        public string DeliveryAmount { get; set; }

        [Display(Name = "Invoice #")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Invoice Date")]
        public string InvoiceDate { get; set; }

        [Display(Name = "Location Name")]
        public string JobName { get; set; }

        [Display(Name = "Product Name")]
        public string Description { get; set; }
    }
}
