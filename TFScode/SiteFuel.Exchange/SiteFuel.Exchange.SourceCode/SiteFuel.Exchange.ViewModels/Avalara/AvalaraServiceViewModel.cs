using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AvalaraServiceViewModel
    {
        public int FuelTypeId { get; set; }
        public string FuelProductCode { get; set; }
        public UoM JobUoM { get; set; }
        public Currency JobCurrency { get; set; }
        public bool IsSalesTaxExempted { get; set; }
        public Currency CountryCurrency { get; set; }
        public AddressViewModel DestinationJobAddress { get; set; }
        public AddressViewModel SourceTerminalAddress { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal PricePerGallon { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public string BuyerCustomId { get; set; }
        public string SellerCustomId { get; set; }
        public bool IsDirectTaxCompany { get; set; }
        public decimal SupplierAllowance { get; set; }
        public TaxExclusionType Exclusions { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int JobId { get; set; }
    }
}
