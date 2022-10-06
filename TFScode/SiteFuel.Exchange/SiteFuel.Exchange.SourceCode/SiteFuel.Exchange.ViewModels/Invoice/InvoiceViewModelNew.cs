using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceViewModelNew
    {
        public string InvoiceChainId { get; set; }
        public PaymentTermViewModel PaymentTerm { get; set; }
        public CustomerViewModel Customer { get; set; }
        public List<InvoiceBolViewModel> BolDetails { get; set; } = new List<InvoiceBolViewModel>();
        public List<InvoiceLiftTicketViewModel> TicketDetails { get; set; } = new List<InvoiceLiftTicketViewModel>();
        public List<InvoiceDropViewModel> Drops { get; set; } = new List<InvoiceDropViewModel>();
        public List<InvoiceDropViewModel> DivertedDrops { get; set; } = new List<InvoiceDropViewModel>();
        public List<DeliveryRequestCompartmentInfoViewModel> DeliveryRequestCompartments { get; set; } = new List<DeliveryRequestCompartmentInfoViewModel>();
        public string SupplierInvoiceNumber { get; set; }
        public string TraceId { get; set; }
        public DropdownDisplayItem Driver { get; set; }
        public string Carrier { get; set; }
        public bool IsAssetTracked { get; set; }
        public List<FeesViewModel> Fees { get; set; } = new List<FeesViewModel>();
        public List<AccessorialFeeTableDetailViewModel> AccessorialFeeDetails { get; set; }
        public ImageViewModel InvoiceImage { get; set; }
        public bool IsBadgeMandatory { get; set; }
        public bool IsBrokerInvoice { get; set; }
        public bool IsMissingDeliveryDDTConversion { get; set; }
        public ImageViewModel[] InvoiceImages
        {
            get
            {
                if (InvoiceImage != null && !string.IsNullOrEmpty(InvoiceImage.FilePath))
                    return InvoiceImage.BreakFilePathToMany().ToArray();
                else
                    return new List<ImageViewModel>().ToArray();
            }
        }
        public ImageViewModel SignatureImage { get; set; }
        public ImageViewModel AdditionalImage { get; set; }
        public ImageViewModel[] AdditionalImages
        {
            get
            {
                if (AdditionalImage != null && !string.IsNullOrEmpty(AdditionalImage.FilePath))
                    return AdditionalImage.BreakFilePathToMany().ToArray();
                else
                    return new List<ImageViewModel>().ToArray();
            }
        }
        public ImageViewModel TaxAffidavitImage { get; set; }
        public ImageViewModel[] TaxAffidavitImages
        {
            get
            {
                if (TaxAffidavitImage != null && !string.IsNullOrEmpty(TaxAffidavitImage.FilePath))
                    return TaxAffidavitImage.BreakFilePathToMany().ToArray();
                else
                    return new List<ImageViewModel>().ToArray();
            }
        }

        public ImageViewModel BDNImage { get; set; }
        public ImageViewModel[] BDNImages
        {
            get
            {
                if (BDNImage != null && !string.IsNullOrEmpty(BDNImage.FilePath))
                    return BDNImage.BreakFilePathToMany().ToArray();
                else
                    return new List<ImageViewModel>().ToArray();
            }
        }

        public ImageViewModel InspectionRequestVoucherImage { get; set; }
        public ImageViewModel[] InspectionRequestVoucherImages
        {
            get
            {
                if (InspectionRequestVoucherImage != null && !string.IsNullOrEmpty(InspectionRequestVoucherImage.FilePath))
                    return InspectionRequestVoucherImage.BreakFilePathToMany().ToArray();
                else
                    return new List<ImageViewModel>().ToArray();
            }
        }
        public ImageViewModel CoastGuardInspectionImage { get; set; }
        public ImageViewModel[] CoastGuardInspectionImages
        {
            get
            {
                if (CoastGuardInspectionImage != null && !string.IsNullOrEmpty(CoastGuardInspectionImage.FilePath))
                    return CoastGuardInspectionImage.BreakFilePathToMany().ToArray();
                else
                    return new List<ImageViewModel>().ToArray();
            }
        }
        public int InvoiceTypeId { get; set; }
        public DiversionType DiversionType { get; set; }
        public string InvoiceNotes { get; set; }
        public bool IsVariousOrigin { get; set; }
        public DropAddressViewModel FuelPickupLocation { get; set; }
        public DropAddressViewModel FuelDropLocation { get; set; }
        public string BrokerChainId { get; set; }
        public WaitingAction WaitingForAction { get; set; }
        public string GroupParentDrId { get; set; }
        public int? OriginalInvoiceHeaderId { get; set; }
        public bool IsRebillInvoice { get; set; }
        public bool IsExceptionApprove { get; set; }
        public List<TaxViewModel> OtherProductTaxes { get; set; } = new List<TaxViewModel>();
        public List<InvoiceXSpecialInstructionViewModel> SpecialInstructions { get; set; } = new List<InvoiceXSpecialInstructionViewModel>();
        public CreationMethod CreationMethod { get; set; }
        public int SupplierCompanyId { get; set; } // Used in mobile drop to set companyId in queuemessage

        //Used in TPD
        public bool IsSiteOutOfFuel { get; set; }
        public string OutOfFuelProduct { get; set; }
        public string TruckNumber { get; set; }
        public int DropStatus { get; set; }
        public int? PreferencesSettingId { get; set; }
        public bool IsSupressOrderPricing { get; set; } = false;
        public string ExternalRefID { get; set; }
        public int OriginalInvoiceNumberId { get; set; }
        public List<BrokeredOrdersModel> BrokeredOrders { get; set; } = new List<BrokeredOrdersModel>();
        public int ExistingHeaderId { get; set; }
        public InvoiceViewModelNew CopyObject(InvoiceViewModelNew source)
        {
            var inputString = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<InvoiceViewModelNew>(inputString);
        }
    }

    public class DeliveryRequestCompartmentInfoViewModel
    {
        public string DeliveryRequestId { get; set; }

        public List<CompartmentsInfoViewModel> Compartments = new List<CompartmentsInfoViewModel>();
    }

    public class PaymentTermViewModel
    {
        public PaymentTerms TermId { get; set; }
        public int NetDays { get; set; }
    }

    public class JobLocationViewModel
    {
        public int JobId { get; set; }
        public string SiteName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public bool IsMarineLocation { get; set; }
        public int CountryId { get; set; }
    }

    public class CustomerViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public JobLocationViewModel Location { get; set; }
        public DropAddressViewModel DropLocation { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
    }

    public class InvoiceDropViewModel
    {
        private decimal _actualDropQuantity = 0;
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public int TypeOfFuel { get; set; }
        public int FuelTypeId { get; set; }
        public int? ParentFuelRequestId { get; set; }
        public string FuelTypeName { get; set; }
        public decimal ActualDropQuantity { get { return Math.Round(_actualDropQuantity, ApplicationConstants.InvoiceDropQuantityDecimalDisplay); } set { _actualDropQuantity = value; } }
        public DateTimeOffset DropDate { get; set; }
        public DateTimeOffset? DropEndDate { get; set; }
        public string DisplayDropDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal? Allowance { get; set; }
        public DateTimeOffset MinDropDate { get; set; }
        public string DisplayMinDropDate { get; set; }
        public int? TrackableScheduleId { get; set; }
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        public bool IsAssetTracked { get; set; }
        public List<AssetDropViewModel> Assets { get; set; } = new List<AssetDropViewModel>();
        public PickupLocationType PickupLocationType { get; set; }
        public DropAddressViewModel PickUpAddress { get; set; }
        public FuelSurchargeFreightFeeViewModel FuelSurchargeFreightFee { get; set; }
        public List<TaxViewModel> OtherTaxDetails { get; set; } = new List<TaxViewModel>();
        public bool IsSignatureRequired { get; set; }
        public bool IsBOLImageRequired { get; set; }
        public bool IsDropImageRequired { get; set; }
        public string BrokerChainId { get; set; }
        public bool IsBolDetailsRequired { get; set; }
        public bool IsFTL { get; set; }
        public UoM UoM { get; set; }
        public Currency Currency { get; set; }
        public int? QuantityIndicatorTypeId { get; set; }
        //For mobile drops
        public int AssetCount { get; set; }
        public bool IsAssetDropOffline { get; set; }
        public decimal? SupplierFuelCost { get; set; }
        public bool IsDipDataRequired { get; set; } = false;
        public string DropTicketNumber { get; set; }

        public string Truck { get; set; }
        public string Tractor { get; set; }
        public string LoadingBadge { get; set; }
        public string CarrierOrderId { get; set; }
        public string CarrierOrder { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public decimal OrderQuantity { get; set; }
        public bool IsFreightOnlyOrder { get; set; }
        public int? ExceptionId { get; set; }
        public bool IsReassignDifferentJob { get; set; }
        public int OldOrderId { get; set; }
        public bool IsFilldInvoke { get; set; }
        public long FilldStopId { get; set; }

        public decimal? Gravity { get; set; }
        public decimal? ConvertedQuantity { get; set; }
        public decimal? ConversionFactor { get; set; }

        //Used in MFN 
        public int JobCountryId { get; set; }
        public bool IsMarineLocation { get; set; }
        public string TimeZoneName { get; set; }
        public int JobId { get; set; }
        public bool IsSupressOrderPricing { get; set; }
        //USE in OPTIONAL PICKUP
        public bool IsOptionalPickup { get; set; }
        public List<string> OptionalPickIds { get; set; } = new List<string>();

        public BDRDetailsModel BdrDetails { get; set; }
        public bool IsBdrDetailsAdded { get; set; }

        public string DeliveryPO { get; set; }

        public string DeliveryLevelPO { get; set; }

        public FreightPricingMethod FreightPricingMethod { get; set; } = FreightPricingMethod.Manual;

        public decimal? UserPriceToOverride { get; set; }
    }

    public class AccessorialFeeTableDetailViewModel
    {
        public TableTypes? AccessorialFeeTableType { get; set; }
        public int? AccessorialFeeId { get; set; }
    }

    public class InvoiceBolViewModel
    {
        public int Id { get; set; }
        public string BolNumber { get; set; }
        public DateTimeOffset? LiftDate { get; set; }

        public string LiftStartTime { get; set; }

        public string LiftEndTime { get; set; }
        public int? CommonTerminalId { get; set; }
        public string CommonTerminalName { get; set; }
        public string DisplayLiftDate { get { return LiftDate?.ToString(Resource.constFormatDate); } }
        public List<BolProductViewModel> Products { get; set; }
        public ImageViewModel Images { get; set; }
        public string BadgeNumber { get; set; }
        public ImageViewModel[] ImageList
        {
            get
            {
                if (Images != null && !string.IsNullOrEmpty(Images.FilePath))
                    return Images.BreakFilePathToMany().ToArray();
                else
                    return new List<ImageViewModel>().ToArray();
            }
        }

        public bool IsBolAvailable()
        {
            return !string.IsNullOrWhiteSpace(BolNumber);
        }
    }

    public class InvoiceLiftTicketViewModel
    {
        public int Id { get; set; }
        public string LiftTicketNumber { get; set; }
        public DateTimeOffset? LiftDate { get; set; }
        public string DisplayLiftDate { get { return LiftDate?.ToString(Resource.constFormatDate); } }
        public List<LiftProductViewModel> Products { get; set; }
        public TimeSpan? LiftArrivalTime { get; set; }
        public TimeSpan? BolCreationTime { get; set; }
        public ImageViewModel Images { get; set; }

        public string LiftStartTime { get; set; }

        public string LiftEndTime { get; set; }
        public string BadgeNumber { get; set; }
        public int CommonBulkPlantId { get; set; }
        public string CommonBulkPlantName { get; set; }
        public DropAddressViewModel Address { get; set; }

        public ImageViewModel[] ImageList
        {
            get
            {
                if (Images != null && !string.IsNullOrEmpty(Images.FilePath))
                    return Images.BreakFilePathToMany().ToArray();
                else
                    return new List<ImageViewModel>().ToArray();
            }
        }
        public bool IsLiftInfoAvailable()
        {
            return !string.IsNullOrWhiteSpace(LiftTicketNumber);
        }
    }

    public class BolProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? NetQuantity { get; set; }
        public decimal? DeliveredQuantity { get; set; }
        public decimal? GrossQuantity { get; set; }
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        public List<CompartmentInfoViewModel> CompartmentInfo { get; set; } = new List<CompartmentInfoViewModel>();
    }
    public class ProductQuantityViewModel
    {
        public int ProductId { get; set; }
        public double? DeliveredQuantity { get; set; }
        public QuantityIndicatorTypes BillableType { get; set; }
    }
    public class LiftProductViewModel
    {
        private decimal? _netQuantity = null;
        private decimal? _grossQuantity = null;
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? LiftQuantity { get; set; }
        public decimal? NetQuantity
        {
            get { return (_netQuantity == null || _netQuantity <= 0) && LiftQuantity > 0 ? LiftQuantity : _netQuantity; }
            set { _netQuantity = value; }
        }
        public decimal? DeliveredQuantity { get; set; }

        public decimal? GrossQuantity {
            get { return (_grossQuantity == null || _grossQuantity <= 0) && LiftQuantity > 0 ? LiftQuantity : _grossQuantity; }
            set { _grossQuantity = value; }
        }
        public int BulkPlantId { get; set; }
        public string BulkPlantName { get; set; }
        public TimeSpan? LiftStartTime { get; set; }
        public TimeSpan? LiftEndTime { get; set; }
        public DropAddressViewModel Address { get; set; }
        public List<CompartmentInfoViewModel> CompartmentInfo { get; set; } = new List<CompartmentInfoViewModel>();
    }

    public class CompartmentInfoViewModel
    {
        public string TrailerId { get; set; }
        public string CompartmentId { get; set; }
        public decimal Quantity { get; set; }
        public int? TrackableScheduleId { get; set; }
    }

    public class CustomerPoViewModel
    {
        public int OrderId { get; set; }
        public int FuelTypeId { get; set; }
        public string PoNumber { get; set; }
        public string FuelTypeName { get; set; }
        public string DisplayPoNumber { get; set; }
    }


    public class BulkPlantAddressViewModel
    {
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string CountyName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string ZipCode { get; set; }
    }

    public class MFNConversionResponseViewModel : StatusViewModel
    {
        public bool IsValidGravity { get; set; }

        public decimal ConvertedQty { get; set; }

        public UoM UoM { get; set; }

    }

    public class MFNConversionRequestViewModel
    {
        public decimal DroppedGallons { get; set; }

        public UoM UoM { get; set; }

        //Could be gravity for MT conversion or hardcoded 42 for barrel conversion
        public  decimal ConversionFactor{ get; set; }
        public decimal? UserProvidedConversionFactor { get; set; }
        public int JobCountryId { get; set; }

    }
    public class ScheduleOptionalPickupModel
    {
        public int TrackableScheduleId { get; set; }
        public List<string> OptionalPickupIds { get; set; } = new List<string>();
    }

    public class UpdateImagesViewModel : StatusViewModel
    {
        public UpdateImagesViewModel()
        {

        }

        public UpdateImagesViewModel(Status status)
        {
            this.StatusCode = status;
        }

        public int InvoiceHeaderId { get; set; }
        public bool IsMarineLocation { get; set; }
        public int? countryId { get; set; }
        public List<UpdateImagesModel> ImagesModels { get; set; } = new List<UpdateImagesModel>();
    }

    public class UploadImageModel
    {
        public int InvoiceHeaderId { get; set; }
        public int InvoiceFtlDetailsId { get; set; }
        public List<HttpPostedFileBase> BolImages { get; set; }
        public HttpPostedFileBase SignatureImage { get; set; }
        public HttpPostedFileBase DropImage { get; set; }
        public HttpPostedFileBase AdditionalImage { get; set; }
        public HttpPostedFileBase TaxAffidavitImage { get; set; }
        public HttpPostedFileBase BDNImage { get; set; }
        public HttpPostedFileBase CGInspectionImage { get; set; }
        public HttpPostedFileBase InspectionVoucherImage { get; set; }
    }

    public class UpdateImagesModel {

        public int InvoiceHeaderId { get; set; }
        public bool IsMarineLocation { get; set; }
        public InvoiceImageType ImageType { get; set; }
        public int InvoiceFtlDetailsId { get; set; }
        public System.Web.HttpPostedFileBase ImageFile { get; set; }
        
        //used only at serverside. no need to set at UI
        public ImageViewModel ImageViewModel { get; set; }
        public int? countryId { get; set; }
    }

    public class DailyDataDumpCSVMailingListViewModel
    {
        public int CompanyId { get; set; }

        public string Emails { get; set; }


    }
}

