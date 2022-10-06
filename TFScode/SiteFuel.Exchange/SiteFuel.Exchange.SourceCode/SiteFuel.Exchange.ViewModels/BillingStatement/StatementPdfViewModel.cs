using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class StatementPdfViewModel:StatusViewModel
    {
        public StatementPdfViewModel()
        {

        }
        public StatementPdfViewModel(Status status)
            : base(status)
        {
            CustomerAddress = new AddressViewModel();
            ContactPerson = new ContactPersonViewModel();
            StatementSummary = new List<StatementInvoiceDetails>();
            Invoices = new List<StatementInvoiceViewModel>();
        }
        public int Id { get; set; }
        public string StatementId { get; set; }
        public string StatementDate { get; set; }
        public string BillingPeriod { get; set; }
        public string StatementReceipt { get; set; }
        public string DueDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCompany { get; set; }
        public decimal StatementValue { get; set; }
        public string SupplierCompany { get; set; }
        public string PhoneNumber { get; set; }
        public string StatementNumber { get; set; }
        public string Culture { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }
        public AddressViewModel SupplierAddress { get; set; }
        public AddressViewModel CustomerAddress { get; set; }
        public ContactPersonViewModel ContactPerson { get; set; }
        public List<StatementInvoiceDetails> StatementSummary { get; set; }
        public List<StatementInvoiceViewModel> Invoices { get; set; }
        public ImageViewModel Image { get; set; }
        public List<InvoicePaymentViewModel> InvoicePaymentSummary { get; set; }
    }

    public class StatementInvoiceViewModel
    {
        public StatementInvoiceViewModel()
        {
            Assets = new List<List<StatementAssetDropViewModel>>();
            FuelRequestFees = new List<StatementFeesViewModel>();
            DiscountLineItems = new List<StatementDiscountLineItemViewModel>();
            TaxDetails = new List<StatementTaxDetailsViewModel>();
            SpecialInstructions = new List<StatementSpecialInstructionViewModel>();
        }
        public string InvoiceNumber { get; set; }
        public string DisplayJobID { get; set; }
        public string WBSNumber { get; set; }
        public bool IsHidePricingEnabled { get; set; }
        public int WaitingForAction { get; set; }
        public bool IsApprovalWorkflowEnabledForJob { get; set; }
        public bool IsFTL { get; set; }
        public Currency Currency { get; set; }
        public int InvoiceTypeId { get; set; }
        public int NoFuelNeededAssetCount { get; set; }
        public int AssetNotAvailableCount { get; set; }
        public string DropDate { get; set; }
        public decimal BasicAmount { get; set; }
        public decimal TotalFees { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal? TotalAllowance { get; set; }
        public string TerminalName { get; set; }
        public string PoNumber { get; set; }
        public bool IsBuyAndSellOrder { get; set; }
        public string PricePerGallonDisplay { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal PricePerGallon { get; set; }
        public decimal OverWaterAssetQuantity { get; set; }
        public string OverWaterHours { get; set; }
        public decimal OverWaterFee { get; set; }
        public decimal WetHoseFee { get; set; }
        public string WetHoseHours { get; set; }
        public decimal WetHoseAssetQuantity { get; set; }
        public int FreightFeeSubTypeId { get; set; }
        public int Version { get; set; }
        public decimal FreightFee { get; set; }
        public decimal FreightFeeAmount { get; set; }
        public string QbInvoiceNumber { get; set; }
        public bool IsVariousFobOrigin { get; set; }
        public decimal SurchargePercentage { get; set; }
        public FuelSurchagePricingType SurchargePricingType { get; set; }
        public AddressViewModel ShippingLocation { get; set; }
        public AddressViewModel PickUpLocation { get; set; }
        public StatementJobViewModel Job { get; set; }
        public StatementBuyAndSellPricingDetailViewModel BuySellDetails { get; set; }
        public StatementFuelDetailsViewModel FuelDetails { get; set; }
        public StatementFtlDetailsViewModel FtlDetails { get; set; }
        public List<StatementSpecialInstructionViewModel> SpecialInstructions { get; set; }
        public List<StatementTaxDetailsViewModel> TaxDetails { get; set; }
        public List<List<StatementAssetDropViewModel>> Assets { get; set; }
        public List<StatementFeesViewModel> FuelRequestFees { get; set; }
        public List<StatementDiscountLineItemViewModel> DiscountLineItems { get; set; }
        public List<InvoicePaymentViewModel> InvoicePayments { get; set; }
        public bool ShouldHidePricing()
        {
            var response = IsHidePricingEnabled || WaitingForAction != (int)WaitingAction.Nothing;
            var isCustomerApproval = IsApprovalWorkflowEnabledForJob && (InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp);
            response = isCustomerApproval || response;
            return response;
        }
    }

    public class StatementFtlDetailsViewModel
    {
        public string BOL { get; set; }
        public string LiftTicketNumber { get; set; }
        public decimal? GrossGallons { get; set; }
        public decimal? NetGallons { get; set; }
        public decimal? LiftQuantity { get; set; }
    }

    public class StatementFeesViewModel
    {
        public int FeeSubTypeId { get; set; }
        public string FeeTypeId { get; set; }
        public bool IncludeInPPG { get; set; }
        public decimal TotalFee { get; set; }
        public string FeeSubTypeName { get; set; }
        public string FeeTypeName { get; set; }
        public string OtherFeeDescription { get; set; }
        public decimal? Fee { get; set; }
        public string TotalHours { get; set; }
        public decimal? MinimumGallons { get; set; }
        public decimal? FeeSubQuantity { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }

        public string GetQuntityInTime()
        {
            if (FeeSubQuantity.HasValue)
            {
                var totalSeconds = Convert.ToDouble(FeeSubQuantity ?? 0);
                var hours = (int)(totalSeconds / 3600);
                var mins = (totalSeconds - (hours * 3600)) / 60;
                return hours != 0 ? string.Format(Resource.constTimeFormat, hours, string.Format(Resource.constFormatDecimal2, mins))
                                : string.Format(Resource.constMinuteFormat, string.Format(Resource.constFormatDecimal2, mins));
            }
            return string.Empty;
        }
    }

    public class StatementDiscountLineItemViewModel
    {
        public string FeeTypeName { get; set; }
        public int FeeSubTypeId { get; set; }
        public decimal Amount { get; set; }
        public string FeeSubTypeName { get; set; }
        public decimal TotalFee { get; set; }
    }

    public class StatementAssetDropViewModel
    {
        public string AssetName { get; set; }
        public decimal DropGallons { get; set; }
        public DateTimeOffset DropDate{ get; set; }
        public bool IsNewAsset { get; set; }
        public string SubcontractorName { get; set; }
        public string EndTime { get; set; }
    }

    public class StatementAssetViewModel
    {
        public string VehicleId { get; set; }
        public string AssetName { get; set; }
        public decimal DropGallons { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public bool IsNewAsset { get; set; }
        public string SubcontractorName { get; set; }
        public int AssetDropId { get; set; }
        public int DropStatus { get; set; }
    }

    public class StatementJobViewModel
    {
        public string JobName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
    }

    public class StatementBuyAndSellPricingDetailViewModel
    {
        public string SellPriceDetail { get; set; }
        public string BuyPriceDetail { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal SupplierMarkUp { get; set; }
        public decimal BrokerMarkUp { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsBuyPriceInvoice { get; set; }
    }

    public class StatementFuelDetailsViewModel
    {
        public string FuelType { get; set; }
        public string FuelDescription { get; set; }
        public int PricingTypeId { get; set; }
        public int FuelDisplayGroupId { get; set; }
        public UoM UoM { get; set; }
    }

    public class StatementSpecialInstructionViewModel
    {
        public bool IsInstructionFollowed { get; set; }
        public string Instruction { get; set; }
    }

    public class StatementTaxDetailsViewModel
    {
        public bool IsModified { get; set; }
        public string RateDescription { get; set; }
        public decimal TradingTaxAmount { get; set; }
    }
}
