using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoicePdfViewModel : BaseCultureViewModel
    {
        public InvoicePdfViewModel()
        {
            InstanceInitialize();
        }

        public InvoicePdfViewModel(Status status) : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            SupplierLocation = new AddressViewModel(status);
            BuyerLocation = new AddressViewModel(status);
            ShippingLocation = new AddressViewModel(status);
            FuelRequest = new FuelRequestViewModel(status);
            CompanyLogo = new ImageViewModel(status);
            PoContact = new ContactPersonViewModel(status);
            WetHoseOverWaterCalculation = new WetHoseOverWaterCalculationViewModel(status);
            SpecialInstructions = new List<InvoiceXSpecialInstructionViewModel>();
            FuelRequestFee = new FuelRequestFeeViewModel();
            PaymentDiscount = new PaymentDiscountViewModel();
            BrokeredOrder = new TPOBrokeredOrderViewModel();
            Invoice = new InvoiceViewModel();
            FuelFees = new FuelFeesViewModel(status);
            Assets = new List<List<AssetDropViewModel>>();
        }
        public InvoiceViewModel Invoice { get; set; }

        public string PoNumber { get; set; }

        public string SupplierCompanyName { get; set; }

        public string BuyerCompanyName { get; set; }

        public string PhoneNumber { get; set; }

        public string CustomerId { get; set; }

        public AddressViewModel SupplierLocation { get; set; }

        public AddressViewModel BuyerLocation { get; set; }

        public AddressViewModel ShippingLocation { get; set; }
        public AddressViewModel PickUpLocation { get; set; }

        public ContactPersonViewModel PoContact { get; set; }

        public FuelRequestViewModel FuelRequest { get; set; }

        public ImageViewModel CompanyLogo { get; set; }

        public List<List<AssetDropViewModel>> Assets { get; set; }

        public int NoFuelNeededAssetCount { get; set; }

        public int AssetNotAvailableCount { get; set; }

        public List<InvoiceXSpecialInstructionViewModel> SpecialInstructions { get; set; }

        public string TerminalName { get; set; }

        public string PickupTerminal { get; set; }
        
        public WetHoseOverWaterCalculationViewModel WetHoseOverWaterCalculation { get; set; }

        public FuelRequestFeeViewModel FuelRequestFee { get; set; }

        public PaymentDiscountViewModel PaymentDiscount { get; set; }

        public int PaymentTermId { get; set; }

        public string PaymentTermName { get; set; }

        public int NetDays { get; set; }

        public int ExternalBrokerId { get; set; }

        public TPOBrokeredOrderViewModel BrokeredOrder { get; set; }

        public bool IsBuyAndSellOrder { get; set; }

        public BuyAndSellPricingDetailViewModel BuyAndSellPricingDetail { get; set; }

        public FuelFeesViewModel FuelFees { get; set; }

        public bool IsFTL { get; set; }

        public string CustomerInvoiceNumber { get; set; }

        public string CustomerCompany { get; set; }

        public string OriginalInvoiceNumber { get; set; }

        public string OriginalInvoiceQbNumber { get; set; }

        public string CreditInvoiceDisplayNumber { get; set; }

        public int? OriginalInvoiceNumberId { get; set; }

        public bool IsRebillInvoice { get; set; }

        public int? OriginalInvoiceType { get; set; }

        public List<InvoicePaymentViewModel> InvoicePayments { get; set; }
        public string BulkPlantName { get; set; }
    }
}

