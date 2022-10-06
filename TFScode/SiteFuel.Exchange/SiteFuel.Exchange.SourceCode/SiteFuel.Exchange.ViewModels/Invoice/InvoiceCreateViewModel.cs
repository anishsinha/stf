using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using System;
using System.Collections.Generic;


namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceCreateViewModel : StatusViewModel
    {
        public InvoiceCreateViewModel()
        {
            InstanceInitialize();
        }

        public InvoiceCreateViewModel(Status status) : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            DropStartDate = DateTimeOffset.Now.Date;
            FuelRequestFee = new FuelRequestFeeViewModel();
            InvoiceImage = new ImageViewModel(status);
            Signature = new CustomerSignatureViewModel();
            BolImage = new ImageViewModel(status);
            OtherProductTaxes = new List<TaxViewModel>();
            ExternalBrokeredOrder = new TPOBrokeredOrderViewModel();
            AssetDrops = new List<AssetDropViewModel>();
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel();
        }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal? FuelDropped { get; set; }
        public bool IsWetHosingDelivery { get; set; }
        public bool IsOverWaterDelivery { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public ImageViewModel InvoiceImage { get; set; }
        public CustomerSignatureViewModel Signature { get; set; }
        public ImageViewModel BolImage { get; set; }
        public ImageViewModel AdditionalImage { get; set; } = new ImageViewModel();
        public ImageViewModel TaxAffidavitImage { get; set; } = new ImageViewModel();
        public ImageViewModel BDNImage { get; set; } = new ImageViewModel();
        public ImageViewModel CoastGuardInspectionImage { get; set; } = new ImageViewModel();
        public ImageViewModel InspectionRequestVoucherImage { get; set; } = new ImageViewModel();
        public int ApprovalUserId { get; set; }
        public int InvoiceTypeId { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public int PaymentTermId { get; set; }
        public int NetDays { get; set; }
        public string CsvFilePath { get; set; }
        public Currency Currency { get; set; }
        public Currency CountryCurrency { get; set; }
        public UoM UoM { get; set; }
        public string TimeZoneName { get; set; }
        public int? TerminalId { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public int FuelTypeId { get; set; }
        public int? MappedParentFuelTypeId { get; set; }
        public string FuelProductCode { get; set; }
        public int TypeOfFuel { get; set; }
        public bool IsAssetTracked { get; set; }
        public List<AssetDropViewModel> AssetDrops { get; set; }
        public FuelRequestFeeViewModel FuelRequestFee { get; set; }
        public int? TrackableScheduleId { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int JobStateId { get; set; }
        public bool IsDirectTaxCompany { get; set; }
        public decimal SupplierAllowance { get; set; }
        public bool IsFTL { get; set; }
        public List<TaxViewModel> OtherProductTaxes { get; set; }
        public string ProductDescription { get; set; }
        public int ExternalBrokerId { get; set; }
        public TPOBrokeredOrderViewModel ExternalBrokeredOrder { get; set; }
        public decimal BrokerMarkUp { get; set; }
        public decimal SupplierMarkUp { get; set; }
        public bool IsApprovalWorkflowEnabledForJob { get; set; }
        public bool IsThirdPartyHardwareUsed { get; set; }
        public bool IsBuySellOrder { get; set; }
        public bool IsBrokeredChainOrder { get; set; }
        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }
        public int PricingTypeId { get; set; }
        public int? RackAvgTypeId { get; set; }
        public decimal? SupplierCost { get; set; }
        public decimal? CurrentCost { get; set; }
        public int? SupplierCostTypeId { get; set; }
        public decimal PricePerGallon { get; set; }
        public bool IsSalesTaxExempted { get; set; }
        public bool IsDriverToUpdateBol { get; set; }
        public AddressViewModel JobAddess { get; set; }
        public AddressViewModel TerminalAddress { get; set; }
        public int TerminalStateId { get; set; }
        public string TraceId { get; set; }
        public InvoiceXAdditionalDetailViewModel AdditionalDetail { get; set; }
        public int InvoiceStatusId { get; set; }
        public int JobCompanyId { get; set; }
        public int DeliveryTypeId { get; set; }
        public decimal MaxQuantity { get; set; }
        public int AcceptedCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public decimal? BrokeredMaxQuantity { get; set; }
        public int OrderAcceptedBy { get; set; }
        public int UserId { get; set; }
        public string BuyerCustomId { get; set; }
        public string SellerCustomId { get; set; }
        public bool IsBuyPrice { get; set; }
        public FuelRequestPricingDetailsViewModel FuelRequestPricingDetail { get; set; } = new FuelRequestPricingDetailsViewModel();
        public bool IsVariousFobOrigin { get; set; }
        public int StateLevelPricingIndicator { get; set; }
        public int CountryId { get; set; }
        public int? SupplierPreferredInvoiceTypeId { get; set; }
        public TankRentalFrequencyViewModel TankFrequency { get; set; }
        public CreationMethod CreationMethod { get; set; } = CreationMethod.SFX;
        public BolDetailViewModel BolDetails { get; set; }
        public DispatchLocationViewModel DropLocation { get; set; }
        public DispatchLocationViewModel PickupLocation { get; set; }
        public bool IsSignatureReq { get; set; }
        public bool IsBOLImageReq { get; set; }
        public bool IsDropImageReq { get; set; }
        public bool IsTerminalPickup { get; set; }
        public int? QuantityIndicatorTypeId { get; set; }
        public decimal? ActualDropQuantity { get; set; }

        public PaymentDueDateType PaymentDueDateType { get; set; }
        public bool IsDigitalDropTicket()
        {
            return InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp;
        }
        public InvoiceCreateViewModel Clone()
        {
            var thisObject = (InvoiceCreateViewModel)this.MemberwiseClone();
            thisObject.AdditionalDetail = this.AdditionalDetail.Clone();
            return thisObject;
        }
    }
}
