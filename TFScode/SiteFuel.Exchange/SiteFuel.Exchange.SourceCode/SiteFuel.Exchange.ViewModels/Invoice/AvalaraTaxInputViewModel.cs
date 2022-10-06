using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AvalaraTaxInputViewModel : BaseViewModel
    {
        public AvalaraTaxInputViewModel()
        {
            InstanceInitialize();
        }

        public AvalaraTaxInputViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            CurrencyRates = new List<CurrencyRateViewModel>();
        }

        public string CompanyName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string TransactionType { get; set; }
        public string TransportationModeCode { get; set; } //"j"
        public string TitleTransferCode { get; set; }
        public string BuyerCompanyName { get; set; }
        public string SupplierCompanyName { get; set; }
        public int JobId { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public string ProductCode { get; set; }
        public decimal UnitPrice { get; set; } //PPG
        public decimal NetUnitsDropped { get; set; }//dropped quantity
        public decimal GrossUnitsDropped { get; set; }
        public decimal BilledUnitsDropped { get; set; }
        public string OriginCountryCode { get; set; }
        public string OriginJurisdiction { get; set; }
        public string OriginCounty { get; set; }
        public string OriginCity { get; set; }
        public string OriginPostalCode { get; set; }
        public string OriginType { get; set; }
        public string OriginOutCityLimitInd { get; set; }
        public string OriginSpecialJurisdictionInd { get; set; }
        public string DestinationCountryCode { get; set; }
        public string DestinationJurisdiction { get; set; }
        public string DestinationCounty { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationPostalCode { get; set; }
        public string DestinationOutCityLimitInd { get; set; }
        public string DestinationSpecialJurisdictionInd { get; set; }
        public string SaleSpecialJurisdictionInd { get; set; }
        public string BuyerCustomId { get; set; }
        public string SellerCustomId { get; set; }
        public Currency Currency { get; set; }
        public UoM UoM { get; set; }
        public List<CurrencyRateViewModel> CurrencyRates { get; set; }
        public decimal SupplierAllowance { get; set; }
        public bool IsDirectTaxCompany { get; set; }
        public bool IsFobOrigin { get; set; }
        public TaxExclusionType Exclusions { get; set; }
        public string DestinationAddress { get; set; }
        public string OriginAddress { get; set; }
        public List<AvalaraInputTransactionsViewModel> FeeTransactionLines { get; set; } = new List<AvalaraInputTransactionsViewModel>();

    }

    public class InvoiceTaxDetailsViewModel : BaseViewModel
    {
        public InvoiceTaxDetailsViewModel()
        {
            InstanceInitialize();
        }

        public InvoiceTaxDetailsViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            AvaTaxDetails = new List<TaxDetailsViewModel>();
        }

        public int InvoiceId { get; set; }
        public int TranId { get; set; }
        public int ReturnCode { get; set; }
        public decimal TotalTaxAmount { get; set; } //"j"
        public bool IsTrueFillTax { get; set; }
        public List<TaxDetailsViewModel> AvaTaxDetails { get; set; }
        public InvoiceTaxDetailsViewModel Clone()
        {
            var thisObject = (InvoiceTaxDetailsViewModel)this.MemberwiseClone();
            if (this.AvaTaxDetails != null)
                thisObject.AvaTaxDetails = AvaTaxDetails.Select(t => t.Clone()).ToList();
            return thisObject;
        }
    }

    public class TaxDetailsViewModel : BaseViewModel
    {
        public TaxDetailsViewModel()
        {
        }

        public TaxDetailsViewModel(Status status) : base(status)
        {
        }
        public int Id { get; set; }
        public decimal ProductCategory { get; set; }
        public string TaxingLevel { get; set; }
        public string TaxType { get; set; }
        public string RateType { get; set; }
        public string RateSubtype { get; set; }
        public string CalculationTypeInd { get; set; }
        public decimal TaxRate { get; set; } //percentage
        public decimal TaxAmount { get; set; }//tax with this type
        public decimal SalesTaxBaseAmount { get; set; }
        public string TaxExemptionInd { get; set; }
        public string RateDescription { get; set; }
        public string Currency { get; set; }
        public string UnitOfMeasure { get; set; }
        public string LicenseNumber { get; set; }
        public bool IsModified { get; set; }
        public int? TaxPricingTypeId { get; set; }
        public decimal TradingTaxAmount { get; set; }
        public string TradingCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public string RelatedLineItem { get; set; }
        public TaxDetailsViewModel Clone()
        {
            return (TaxDetailsViewModel)this.MemberwiseClone();
        }
    }

    public class AvalaraTaxMultipleInputViewModel : BaseViewModel
    {
        public AvalaraTaxMultipleInputViewModel()
        {
            InstanceInitialize();
        }

        public AvalaraTaxMultipleInputViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            CurrencyRates = new List<CurrencyRateViewModel>();
        }

        public DateTime EffectiveDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CompanyName { get; set; }
        public string BuyerCustomId { get; set; }
        public string SellerCustomId { get; set; }
        public Currency Currency { get; set; }
        public bool IsDirectTaxCompany { get; set; }
        public List<CurrencyRateViewModel> CurrencyRates { get; set; }
        public bool IsFobOrigin { get; set; }
        public TaxExclusionType Exclusions { get; set; }
        public int JobId { get; set; }
        public List<AvalaraInputTransactionsViewModel> InputTransactionLines { get; set; } = new List<AvalaraInputTransactionsViewModel>();
    }

    public class AvalaraInputTransactionsViewModel
    {

        public decimal SupplierAllowance { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        //public string BuyerCompanyName { get; set; }
        //public string SupplierCompanyName { get; set; }
        public string ProductCode { get; set; }
        public decimal UnitPrice { get; set; } //PPG
        public decimal NetUnitsDropped { get; set; }//dropped quantity
        public decimal GrossUnitsDropped { get; set; }
        public decimal BilledUnitsDropped { get; set; }
        public UoM UoM { get; set; }
        public string OriginCountryCode { get; set; }
        public string OriginJurisdiction { get; set; }
        public string OriginCounty { get; set; }
        public string OriginAddress { get; set; }
        public string OriginCity { get; set; }
        public string OriginPostalCode { get; set; }
        public string OriginType { get; set; }
        public string OriginOutCityLimitInd { get; set; }
        public string OriginSpecialJurisdictionInd { get; set; }
        public string DestinationCountryCode { get; set; }
        public string DestinationJurisdiction { get; set; }
        public string DestinationCounty { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationPostalCode { get; set; }
        public string DestinationOutCityLimitInd { get; set; }
        public string DestinationSpecialJurisdictionInd { get; set; }
        public string DestinationAddress { get; set; }
        public string SaleSpecialJurisdictionInd { get; set; }
        public string BillOfLaddingNumber { get; set; }
        public DateTime BillOfLaddingDate { get; set; }
    }
}
