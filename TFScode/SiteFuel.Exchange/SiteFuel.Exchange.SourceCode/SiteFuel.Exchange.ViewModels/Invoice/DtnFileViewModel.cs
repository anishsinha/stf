using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DtnFileViewModel
    {
        public int InvoiceNumberId { get; set; }
        public string SupplierCompanyName { get; set; }
        public string BuyerCompanyName { get; set; }
        public List<TaxDetailsViewModel> TaxDetails { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public string PoNumber { get; set; }
        public int NetDays { get; set; }
        public string InvoiceNumber { get; set; }
        public string JobName { get; set; }
        public string JobAddress { get; set; }
        public string JobCity { get; set; }
        public string JobStateCode { get; set; }
        public string JobZipCode { get; set; }
        public string ControlNumber { get; set; }
        public int PaymentTermId { get; set; }
        public int InvoiceTypeId { get; set; }
        public decimal BasicAmount { get; set; }
        public string FuelType { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal PricePerGallon { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal? TotalFeeAmount { get; set; }
        public decimal TotalAllowance { get; set; }
        public decimal SupplierAllowance { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal GrossQuantity { get; set; }
        public decimal NetQuantity { get; set; }
        public string BolNumber { get; set; }
        public string PaymentTerm { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public string Carrier { get; set; }
        public string TerminalName { get; set; }        
        public int Version { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public bool AllowanceAsTxdl { get; set; }
        public int FreightFeeType { get; set; }
        public bool AllFeeAsTxdl { get; set; }
        public int? PricingQuantityIndicatorTypeId { get; set; }
        public List<FeesViewModel> FuelRequestFees { get; set; }
        public string OriginalInvoiceNumber { get; set; }
        public decimal? SurchargePercentage { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public string LiftTicketNumber { get; set; }
        public bool IsFtl { get; set; }
        public int OrderId { get; set; }

        public bool IsCreditInvoice()
        {
            return (InvoiceTypeId == (int)InvoiceType.PartialCredit || InvoiceTypeId == (int)InvoiceType.CreditInvoice);
        }
    }
}
