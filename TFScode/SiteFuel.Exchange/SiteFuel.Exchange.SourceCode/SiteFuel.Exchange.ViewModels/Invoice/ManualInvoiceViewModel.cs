using Foolproof;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SiteFuel.Exchange.ViewModels
{
    public class ManualInvoiceViewModel : BaseViewModel
    {
        public ManualInvoiceViewModel()
        {
            InstanceInitialize();
        }

        public ManualInvoiceViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            CreatedDate = DateTimeOffset.Now;
            DeliveryDate = DateTimeOffset.Now.Date;
            InvoiceNumber = new InvoiceNumberViewModel();
            FuelRequestFee = new FuelRequestFeeViewModel();
            InvoiceImage = new ImageViewModel(status);
            BolImage = new ImageViewModel(status);
            SignatureImage = new ImageViewModel(status);
            TaxAffidavitImage = new ImageViewModel(status);
            BDNImage = new ImageViewModel(status);
            CoastGuardInspectionImage = new ImageViewModel(status);
            InspectionRequestVoucherImage = new ImageViewModel(status);
            AssetDropImages = new List<ImageViewModel>();
            SupplierQualifications = new List<int>();
            Taxes = new List<TaxViewModel>();
            PaymentDiscount = new PaymentDiscountViewModel();
            ExternalBrokeredOrder = new TPOBrokeredOrderViewModel();
            Assets = new List<AssetDropViewModel>();
            SelectedAssets = new List<int>();
            TaxDetailsViewModel = new List<OrderTaxDetailsViewModel>();
            IsOtherFuelTypeTaxesGiven = false;
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel();
            SpecialInstructions = new List<InvoiceXSpecialInstructionViewModel>();
            TaxType = TaxType.Unknown;
            IsPoNumberEdit = false;
            TaxDetails = new InvoiceTaxDetailsViewModel();
            BolDetails = new BolDetailViewModel();
            AdditionalImage = new ImageViewModel(status);
            AccessorialFeeDetails = new List<AccessorialFeeTableDetailViewModel>();
        }

        public InvoiceNumberViewModel InvoiceNumber { get; set; }

        public int userId { get; set; }

        public int OrderId { get; set; }

        public int InvoiceId { get; set; }

        public int PaymentTermId { get; set; }

        public int DDTConversionReason { get; set; }

        public PaymentMethods PaymentMethod { get; set; }

        public int FuelRequestId { get; set; }

        public int NetDays { get; set; }

        public int InvoiceNumberId { get; set; }

        public int InvoiceTypeId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public decimal TerminalPrice { get; set; }

        public int StatusId { get; set; }

        public decimal TotalInvoiceAmount { get; set; }

        public string SupplierName { get; set; }

        public string SupplierEmail { get; set; }

        public string SupplierPhone { get; set; }

        public int TerminalId { get; set; }

        public int DiscountId { get; set; }

         [RequiredIfTrue("IsTerminalNameRequired", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
       // [RequiredIfNot("TypeOfFuel", (int)ProductDisplayGroups.OtherFuelType, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string TerminalName { get; set; }

        public int CityGroupTerminalId { get; set; }

        public string CityGroupTerminalName { get; set; }

        public FuelRequestFeeViewModel FuelRequestFee { get; set; }

        public ImageViewModel InvoiceImage { get; set; }

        public ImageViewModel SignatureImage { get; set; }
        public ImageViewModel TaxAffidavitImage { get; set; }
        public ImageViewModel BDNImage { get; set; }
        public ImageViewModel CoastGuardInspectionImage { get; set; }
        public ImageViewModel InspectionRequestVoucherImage { get; set; }

        public ImageViewModel BolImage { get; set; }

        public ImageViewModel AdditionalImage { get; set; }

        public string PoNumber { get; set; }

        public bool IsPublicRequest { get; set; }

        public bool IsExistingDropLocation { get; set; }

        public List<ImageViewModel> AssetDropImages { get; set; }

        [Display(Name = nameof(Resource.lblFuelRemaining), ResourceType = typeof(Resource))]
        public decimal FuelRemaining { get; set; }

        public bool AssetTracked { get; set; }

        public List<AssetDropViewModel> Assets { get; set; }

        public List<int> SelectedAssets { get; set; }

        [Display(Name = nameof(Resource.lblSupplierNeedToQualify), ResourceType = typeof(Resource))]
        public List<int> SupplierQualifications { get; set; }

        public List<AccessorialFeeTableDetailViewModel> AccessorialFeeDetails { get; set; }
        
        public string FuelType { get; set; }

        public int OrderTypeId { get; set; }

        public int QuantityTypeId { get; set; }

        public decimal OrderTotal { get; set; }

        public decimal StateTax { get; set; }

        public decimal FederalTax { get; set; }

        public decimal SalesTax { get; set; }

        public int TypeofFuel { get; set; }

        public string DisplayInvoiceNumber { get; set; }
        public string SupplierInvoiceNumber { get; set; }
        public bool IsMarineLocation { get; set; }
        public PaymentDiscountViewModel PaymentDiscount { get; set; }

        [Display(Name = nameof(Resource.lblFuelQuantity), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d+)|(\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal? FuelDropped { get; set; }

        [Display(Name = nameof(Resource.lblDropDate), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = nameof(Resource.lblStartTime), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string StartTime { get; set; }

        [Display(Name = nameof(Resource.lblDropEndDate), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTime? DropEndDate { get; set; } = DateTime.Now;

        [Display(Name = nameof(Resource.lblEndTime), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string EndTime { get; set; }

        public bool IsWethosingDelivery { get; set; }

        public bool IsOverWaterDelivery { get; set; }

        public DateTimeOffset FuelRequestDeliveryStartDate { get; set; }

        public DateTimeOffset OrderAcceptDate { get; set; }

        public DateTimeOffset? MaxDropDate { get; set; }

        [Display(Name = nameof(Resource.lblDriver), ResourceType = typeof(Resource))]
        public int? DriverId { get; set; }

        [RequiredIfTrue("IsExistingDropLocation", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFuelDropAddress), ResourceType = typeof(Resource))]
        public int? ExistingDropLocationId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public bool IsMulitpleDelivery { get; set; }

        public bool IsBackDatedJob { get; set; }

        public InvoiceTaxDetailsViewModel TaxDetails { get; set; }

        public bool IsInvoiceFromDropTicket { get; set; }

        public string TimeZoneName { get; set; }

        public int BuyerCompanyId { get; set; }

        public int SupplierCompanyId { get; set; }

        public int JobId { get; set; }

        public int FuelId { get; set; }

        public string BuyerCompanyName { get; set; }

        public List<TaxViewModel> Taxes { get; set; }

        public int ConversionDDTId { get; set; }

        public bool IsConvertFromDDT { get; set; }

        public string PaymentDueDate { get; set; }

        public string ProductDescription { get; set; }

        public bool IsTaxServiceFailure { get; set; }

        public bool IsPoNumberEdit { get; set; }

        public int ExternalBrokerId { get; set; }

        public TPOBrokeredOrderViewModel ExternalBrokeredOrder { get; set; }

        public string CsvFilePath { get; set; }

        public int WaitingForAction { get; set; }

        public DateTimeOffset TerminalPricingDate { get; set; }

        public bool IsApprovalWorkflowEnabledForJob { get; set; }

        public decimal AvgGallonsPercentagePerDelivery { get; set; }

        public bool IsBuyPriceInvoice { get; set; }

        public bool IsThirdPartyHardwareUsed { get; set; }

        public bool IsBuySellOrder { get; set; }

        public List<OrderTaxDetailsViewModel> TaxDetailsViewModel { get; set; }

        public bool IsOtherFuelTypeTaxesGiven { get; set; }

        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }

        public bool IsInvoiceEdit { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public List<InvoiceXSpecialInstructionViewModel> SpecialInstructions { get; set; }

        public PricingType PricingType { get; set; }

        public int? SignatureId { get; set; }

        public TaxType TaxType { get; set; }

        public string BrokeredChainId { get; set; }

        public string QbInvoiceNumber { get; set; }

        public string SplitLoadChainId { get; set; }

        public int? SplitLoadSequence { get; set; }

        public bool IsFTL { get; set; }

        [MaxLength(500, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvoiceNotesGreaterThan))]
        public string Notes { get; set; }

        public BolDetailViewModel BolDetails { get; set; }
        public List<BolDetailViewModel> BolDetailsNew { get; set; } = new List<BolDetailViewModel>();
        public DropAddressViewModel DropAddress { get; set; }
        public DropAddressViewModel PickUpAddress { get; set; }
        public bool IsVariousFobOrigin { get; set; }
        public bool IsDirectTaxCompany { get; set; }
        public List<SplitLoadInvoiceDetailViewModel> SplitLoadInvoices { get; set; }

        public BDRDetailsModel BDRDetail { get; set; }
        public TankRentalFrequencyViewModel TankFrequency { get; set; }
        public List<DropdownDisplayItem> TankRentalFrequencyTypes { get; set; }
        public int SelectedFrequency { get; set; }

        public decimal TotalSplitDroppedGallons { get; set; }

        public decimal? FuelCost { get; set; }

        public bool IsQuanityOrDateChanged { get; set; }

        public string DropTicketNumber { get; set; }
        public string TruckNumber { get; set; }

        public CreationMethod CreationMethod { get; set; } = CreationMethod.SFX;

        public int? OriginalInvoiceId { get; set; }

        public decimal InvoiceCreationPricePerGallon { get; set; }

        [Display(Name = nameof(Resource.lblFuelQuantity), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d+)|(\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Range(typeof(decimal), ApplicationConstants.Zero, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal? ZeroGallonAllowedFuelDropped { get; set; }

        [Display(Name = nameof(Resource.lblTotalCreditAmount), ResourceType = typeof(Resource))]
        [LessThanOrEqualTo("TotalInvoiceAmount", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageLessThan), PassOnNull = true)]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal? TotalCreditAmount { get; set; }

        public bool IsDriverSignatureEnabled { get; set; }

        public bool IsBolEditAllowed { get; set; }

        public bool IsInvoiceImagesAvailable { get; set; }

        public bool IsBolRequired { get; set; }

        public int PricingCodeId { get; set; }

        public int? QuantityIndicatorTypeId { get; set; }

        public decimal? SupplierAllowance { get; set; }

        public decimal? ActualDropQuantity { get; set; }
        public int InvoiceHeaderId { get; set; }
        public bool IsBadgeMandatory { get; set; }
        public string InvoiceChainId { get; set; }
        public decimal OriginalDroppedGallons{ get; set; } // actual drop quantity before Invoice edit


        [Display(Name = nameof(Resource.lblAPIGravity), ResourceType = typeof(Resource))]
        public decimal? Gravity { get; set; }

        public decimal? ConvertedQuantity { get; set; }
        public decimal? ConvertionFactor { get; set; }
        public bool IsConversionFactor { get; set; }

        public int JobCountryId { get; set; }

        public bool IsSuppressPricingOrder { get; set; }
        public string DeliveryLevelPO { get; set; }
        public int DeliveryLevelTrackableScheduleId { get; set; }
        public bool IsDigitalDropTicket()
        {
            return InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp;
        }

        public ManualInvoiceViewModel Clone(int userId, int orderId = 0)
        {
            var thisObject = (ManualInvoiceViewModel)this.MemberwiseClone();
            thisObject.userId = userId;
            if (orderId > 0)
            {
                thisObject.OrderId = orderId;
            }
            return thisObject;
        }

        public bool IsPrePostDipRequired { get; set; }

        public ManualInvoiceViewModel CopyObject(ManualInvoiceViewModel source)
        {
            var inputString = JsonConvert.SerializeObject(source, new ManualInvoiceModelConverter());
            return JsonConvert.DeserializeObject<ManualInvoiceViewModel>(inputString, new ManualInvoiceModelConverter());
        }

        public bool IsTerminalRequired()
        {
            bool isTerminalNameRequired = true;
            if (TypeofFuel == (int)ProductDisplayGroups.OtherFuelType || IsSuppressPricingOrder)
            {
                isTerminalNameRequired = false;
            }
            return isTerminalNameRequired;
        }
        public bool IsTerminalNameRequired
        {
            get
            {
                return IsTerminalRequired();
            }
        }
    }

    public class ManualInvoiceModelConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var stream = (Stream)value;
            using (var sr = new BinaryReader(stream))
            {
                var buffer = sr.ReadBytes((int)stream.Length);
                writer.WriteValue(Convert.ToBase64String(buffer));
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(Stream));
        }
       
    }
}
