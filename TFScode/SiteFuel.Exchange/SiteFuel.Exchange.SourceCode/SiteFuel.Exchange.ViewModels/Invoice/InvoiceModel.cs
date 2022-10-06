using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceModel
    {
        public InvoiceModel()
        {
            AssetDrops = new List<AssetDropModel>();
            Discounts = new List<DiscountViewModel>();
            FuelFees = new List<FuelFeeViewModel>();
            AccessorialFeeDetails = new List<AccessorialFeeTableDetailViewModel>();
            SpecialInstructions = new List<InvoiceXSpecialInstructionViewModel>();
            InvoiceExceptions = new List<InvoiceExceptionViewModel>();
            InvoiceVersionStatusId = (int)InvoiceVersionStatus.Active;
            Version = 1;
            IsActive = true;
        }
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int InvoiceNumberId { get; set; }
        public int OriginalInvoiceId { get; set; }
        public int Version { get; set; }
        public int InvoiceVersionStatusId { get; set; }
        public int InvoiceTypeId { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal PricePerGallon { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public string PoNumber { get; set; }
        public decimal StateTax { get; set; }
        public decimal FedTax { get; set; }
        public decimal SalesTax { get; set; }
        public decimal BasicAmount { get; set; }
        public int DDTConversionReason { get; set; }
        public bool IsWetHosingDelivery { get; set; }
        public bool IsOverWaterDelivery { get; set; }
        public bool IsReassignDifferentJob { get; set; }
        public int OldOrderId { get; set; }
        public int PaymentTermId { get; set; }
        public int NetDays { get; set; }
        public DateTimeOffset PaymentDueDate { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public int? TerminalId { get; set; }
        public int? ParentId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public int UpdatedByCompanyId { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public string TransactionId { get; set; }
        public decimal RackPrice { get; set; }
        public int? DriverId { get; set; }
        public string TraceId { get; set; }
        public int? SignatureId { get; set; }
        public string FilePath { get; set; }
        public WaitingAction WaitingFor { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public int? SupplierPreferredInvoiceTypeId { get; set; }
        public bool IsBuyPriceInvoice { get; set; }
        public decimal? TotalFeeAmount { get; set; }
        public string BrokeredChainId { get; set; }
        public decimal BaseDroppedQuntity { get; set; }
        public decimal BasePrice { get; set; }
        public decimal BaseStateTax { get; set; }
        public decimal BaseFedTax { get; set; }
        public decimal BaseSalesTax { get; set; }
        public decimal BaseBasicAmount { get; set; }
        public decimal BaseTotalTaxAmount { get; set; }
        public decimal BaseRackPrice { get; set; }
        public decimal? BaseTotalFeeAmount { get; set; }
        public Currency Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public UoM UoM { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public string ReferenceId { get; set; }
        public string QbInvoiceNumber { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public int StatusId { get; set; }
        public int? TrackableScheduleId { get; set; }
        public string GroupParentDrId { get; set; }
        public int? TrackableScheduleStatusId { get; set; }
        public InvoiceXAdditionalDetailViewModel AdditionalDetail { get; set; }
        public List<AssetDropModel> AssetDrops { get; set; }
        public List<FuelFeeViewModel> FuelFees { get; set; }
        public List<AccessorialFeeTableDetailViewModel> AccessorialFeeDetails { get; set; }
        public List<InvoiceXSpecialInstructionViewModel> SpecialInstructions { get; set; }
        public InvoiceTaxDetailsViewModel TaxDetails { get; set; }
        public List<DiscountViewModel> Discounts { get; set; }
        public ImageViewModel Image { get; set; }
        public ImageViewModel BolImage { get; set; }
        public ImageViewModel AdditionalImage { get; set; }
        public ImageViewModel BDNImage { get; set; }
        public ImageViewModel TaxAffidavitImage { get; set; }
        public ImageViewModel CoastGuardInspectionImage { get; set; }
        public ImageViewModel InspectionRequestVoucherImage { get; set; }
        public CustomerSignatureViewModel Signature { get; set; }
        public List<BolDetailViewModel> BolDetails { get; set; } = new List<BolDetailViewModel>();
        public DispatchLocationViewModel FuelPickLocation { get; set; }
        public DispatchLocationViewModel FuelDropLocation { get; set; }
        public bool IsVariousFobOrigin { get; set; }
        public TankRentalFrequencyViewModel TankFrequency { get; set; }
        public List<InvoiceExceptionViewModel> InvoiceExceptions { get; set; }
        public FuelSurchargeFreightFeeViewModel SurchargeFreightFeeViewModel { get; set; }
        public CreationMethod CreationMethod { get; set; } = CreationMethod.SFX;
        public decimal BuySellBasePPG { get; set; }
        public bool IsSignatureReq { get; set; }
        public bool IsBOLImageReq { get; set; }
        public bool IsPrePostDipDataRequired { get; set; }
        public bool IsDropImageReq { get; set; }
        public bool IsInvoiceImagesAvailable { get; set; }
        public bool IsTerminalPickup { get; set; }
        public int? QuantityIndicatorTypeId { get; set; }
        public int? TaxQuantityIndicatorTypeId { get; set; }
        public decimal? SupplierCost { get; set; }
        public decimal? CurrentCost { get; set; }
        public int? FuelCostTypeId { get; set; }
        public string FuelProductCode { get; set; }
        public int TypeOfFuel { get; set; }
        public int InvoiceHeaderId { get; set; }
        public int FuelRequestTypeId { get; set; }
        public int SupplierCompanyId { get; set; } //used only in mobile case for Group DR consolidaiton

        public decimal? Gravity { get; set; }
        public decimal? ConversionFactor { get; set; }
        public decimal? ConvertedQuantity { get; set; }
        public bool IsMarineLocation { get; set; }
        public int PricingTypeId { get; set; }
        public int JobCountryId { get; set; }
        public decimal? ConvertedPricing { get; set; }

        public BDRDetailsModel BDRDetails { get; set; } = new BDRDetailsModel();

        public string PDIInvoiceNo { get; set; }
        public bool IsPdieTaxRequired { get; set; }
        public bool IsIncludePricingInExternalObj { get; set; }
        public string DeliveryLevelPO { get; set; }
        public PaymentDueDateType PaymentDueDateType { get; set; } = PaymentDueDateType.InvoiceCreationDate; //use only in edit case

        public bool IsProcessWithoutTax { get; set; }
        public decimal? UserPriceToOverride { get; set; }
        
        public bool IsDigitalDropTicket()
        {
            return (InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp);
        }
        private string GetInvoiceNumber()
        {
            var response = ApplicationConstants.SFIN + InvoiceNumberId.ToString().PadLeft(7, '0');
            if (IsDigitalDropTicket())
            {
                response = ApplicationConstants.SFDD + InvoiceNumberId.ToString().PadLeft(7, '0');
            }
            return response;
        }
        public InvoiceModel Clone()
        {
            var thisObject = (InvoiceModel)this.MemberwiseClone();
            thisObject.AdditionalDetail = this.AdditionalDetail.Clone();
            
            if (thisObject.BDRDetails != null)
                thisObject.BDRDetails = this.BDRDetails.Clone();

            if (this.TaxDetails != null)
                thisObject.TaxDetails = this.TaxDetails.Clone();
            return thisObject;
        }

        public InvoiceModel CopyObject(InvoiceModel source)
        {
            var inputString = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<InvoiceModel>(inputString);
        }
    }

    public class BDRDetailsModel
    {
        public int InvoiceId { get; set; }
        public string BDRNumber { get; set; }

        [Display(Name = nameof(Resource.lblDeliveryPumpingStartTime), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string PumpingStartTime { get; set; }

        [Display(Name = nameof(Resource.lblDeliveryPumpingCompletionTime), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string PumpingStopTime { get; set; }

        [Display(Name = nameof(Resource.lblOpenMeterReading), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string OpenMeterReading { get; set; }

        [Display(Name = nameof(Resource.lblCloseMeterReading), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string CloseMeterReading { get; set; }

        [Display(Name = nameof(Resource.lblViscosity), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Viscosity { get; set; }

        [Display(Name = nameof(Resource.lblSulfurContent), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string SulphurContent { get; set; }

        [Display(Name = nameof(Resource.lblFlashPoint), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string FlashPoint { get; set; }

        [Display(Name = nameof(Resource.lblDensityInVacuum), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string DensityInVaccum { get; set; }

        [Display(Name = nameof(Resource.lblObservedTemperature), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ObservedTemperature { get; set; }

        [Display(Name = nameof(Resource.lblMeasuredVolume), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string MeasuredVolume { get; set; }

        [Display(Name = nameof(Resource.lblStandardVolume), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string StandardVolume { get; set; }

        [Display(Name = nameof(Resource.lblMARPOLSampleNumbers), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string MarpolSampleNumbers { get; set; }

        [Display(Name = nameof(Resource.lblMVMARPOLSampleNumbers), ResourceType = typeof(Resource))]
        public string MVMarpolSampleNumbers { get; set; }

        public bool IsEngineerInvitedToWitnessSample { get; set; }
        public bool IsNoticeToProtestIssued { get; set; }

        public BDRDetailsModel Clone()
        {
            return (BDRDetailsModel)this.MemberwiseClone();
        }
    }

    public class PDITaxDetailsViewModel
    {
        public string PDIOrderNumber { get; set; }

        public string CustomerDescription { get; set; }

        public string TaxType { get; set; }

        public decimal TaxRate { get; set; }

        public string TaxDescription { get; set; }

        public string TaxMethod { get; set; }

        public decimal TaxBasis { get; set; }

        public decimal TaxAmount { get; set; }

        public string TaxExemptionDescription { get; set; }

        public int TaxExemptionOverride { get; set; }

        public string TaxCertificateNumber { get; set; }
        public string BasedOnUnits { get; set; }

        public int? TaxPricingTypeId { get; set; }
        public string PDIInvoiceNo { get; set; }
    }
}
