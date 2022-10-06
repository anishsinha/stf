using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using SiteFuel.Exchange.ViewModels.Forcasting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ThirdPartyOrderViewModel : BaseViewModel
    {
        public ThirdPartyOrderViewModel()
        {
            InstanceInitialize();
        }

        public ThirdPartyOrderViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            CustomerDetails = new TPOCustomerViewModel();
            AddressDetails = new TPOAddressViewModel();
            BillingAddress = new TPOBillingAddressViewModel();
            FuelDetails = new FuelDetailsViewModel();
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel();
            PricingDetails = new FuelPricingViewModel();
            Assets = new List<AssetViewModel>();
            IsInvitationEnabled = false;
            ExternalBrokeredOrder = new TPOBrokeredOrderViewModel();
            TaxDetailsViewModel = new List<OrderTaxDetailsViewModel>();
            FuelOfferDetails = new FuelOfferDetailsViewModel();
            DefaultInvoiceType = (int)InvoiceType.DigitalDropTicketManual;
            IsSupressOrderPricing = false;
            IsDriverProdutDisplayEnable = false;
            ImageDetails = new TPOSiteImageViewModel();
            ForcastingPreference = new ForcastingPreferenceViewModel();
            OrderBadgeDetails = new OrderBadgeViewModel();
        }

        public TPOCustomerViewModel CustomerDetails { get; set; }

        public TPOAddressViewModel AddressDetails { get; set; }

        public TPOBillingAddressViewModel BillingAddress { get; set; }

        public FuelDetailsViewModel FuelDetails { get; set; }

        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }

        public OrderAdditionalDetailsViewModel OrderAdditionalDetailsViewModel { get; set; } = new OrderAdditionalDetailsViewModel();

        public FuelPricingViewModel PricingDetails { get; set; }

        [Display(Name = nameof(Resource.lblPoNumber), ResourceType = typeof(Resource))]
        [Remote("IsTPOValidPONumber", "Validation", AreaReference.UseRoot, AdditionalFields = "Id,CustomerDetails.CompanyName", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessagePoAlreadyExist))]
        public string PONumber { get; set; }

        public int CurrentCompanyId { get; set; }

        [Display(Name = nameof(Resource.lblAddAssets), ResourceType = typeof(Resource))]
        public bool IsAssetTracked { get; set; }

        public bool IsAssetDropStatusEnabled { get; set; }

        public List<AssetViewModel> Assets { get; set; }

        public int FuelRequestId { get; set; }

        public int OrderId { get; set; }

        public string ExternalOrderNumber { get; set; }

        public bool IsNotifyDeliveries { get; set; }

        public bool IsNotifySchedules { get; set; }

        public bool IsInvitationEnabled { get; set; }

        public int MaxAllowedFileSize { get; set; }

        public bool IsBrokeredOrder { get; set; }

        public bool IsSendFileToBroker { get; set; }

        public bool IsBuyAndSellOrder { get; set; }

        [Display(Name = nameof(Resource.lblCityGroupTerminal), ResourceType = typeof(Resource))]
        public int? CityGroupTerminalId { get; set; }

        public TPOBrokeredOrderViewModel ExternalBrokeredOrder { get; set; }

        [Display(Name = nameof(Resource.lblDriver), ResourceType = typeof(Resource))]
        public int? DriverId { get; set; }

        public bool IsNewDriver { get; set; }

        [RequiredIfTrue("IsNewDriver", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFirstName), ResourceType = typeof(Resource))]
        public string DriverFirstName { get; set; }

        [RequiredIfTrue("IsNewDriver", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblLastName), ResourceType = typeof(Resource))]
        public string DriverLastName { get; set; }

        [RequiredIfTrue("IsNewDriver", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Remote("IsNewDriverUserEmailExist", "Validation", AreaReference.UseRoot, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string DriverEmail { get; set; }

        public bool IsThirdPartyHardwareUsed { get; set; }

        public bool IsOtherFuelTypeTaxesGiven { get; set; }

        public List<OrderTaxDetailsViewModel> TaxDetailsViewModel { get; set; }

        [Display(Name = nameof(Resource.lblTaxExempted), ResourceType = typeof(Resource))]
        public bool IsTaxExempted { get; set; }

        public FuelOfferDetailsViewModel FuelOfferDetails { get; set; }

        public int DefaultInvoiceType { get; set; }

        public bool IsFTLEnabled { get; set; }

        public BolDetailViewModel BolDetails { get; set; } = new BolDetailViewModel();

        public CarrierViewModel Carrier { get; set; } = new CarrierViewModel();

        public JobSpecificBillToViewModel BillToInfo { get; set; } = new JobSpecificBillToViewModel();

        public OnboardingPreferenceViewModel PreferencesSetting { get; set; }

        public bool IsOnboardingPreferenceExists { get; set; } = false;

        public bool IsSupressOrderPricing { get; set; } = false;

        [Display(Name = nameof(Resource.lblDispatchRegion), ResourceType = typeof(Resource))]
        public string RegionId { get; set; }

        [Display(Name = nameof(Resource.lblNumOfSubDrs), ResourceType = typeof(Resource))]
        //[RegularExpression(@"^\d{2}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public int? NumOfSubDrs { get; set; }

        [Display(Name = nameof(Resource.lblCarrierForLocation), ResourceType = typeof(Resource))]
        public int? AssignedCarrierCompId { get; set; }
        public List<int> CarrierUserEmails { get; set; } = new List<int>();
        public bool IsDriverProdutDisplayEnable { get; set; }
        public List<TrailerTypeStatus> TrailerType { get; set; }
        public bool IsCloneOrder { get; set; }

        [Display(Name = nameof(Resource.lblOrderName), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        public string OrderName { get; set; }

        public TPOSiteImageViewModel ImageDetails { get; set; }
        [Display(Name = nameof(Resource.lblAccountingCompanyId), ResourceType = typeof(Resource))]
        public string AccountingCompanyId { get; set; }
        public bool IsBadgeMandatory { get; set; }
        [Display(Name = nameof(Resource.lblAssignRoute), ResourceType = typeof(Resource))]
        public string RouteId { get; set; }

        [Display(Name = nameof(Resource.lblLocationInventoryManged), ResourceType = typeof(Resource))]
        public List<LocationInventoryManagedBy> LocationInventoryManagedBy { get; set; }

        //public bool IsTierPricing { get; set; } = false;

        public TierPricingViewModel TierPricing { get; set; } = new TierPricingViewModel();
        public ForcastingPreferenceViewModel ForcastingPreference { get; set; } = new ForcastingPreferenceViewModel();
        public ForcastingServiceSettingViewModel ForcastingServiceSetting { get; set; } = new ForcastingServiceSettingViewModel();
        public OrderBadgeViewModel OrderBadgeDetails { get; set; }
        public SourceRegionTpoViewModel SourceRegion { get; set; } = new SourceRegionTpoViewModel();
        public int? LeadRequestId { get; set; }

        public bool IsPortUsedByAnotherCompany { get; set; } = false;
    }

    public class TPOBillingAddressViewModel : StatusViewModel
    {
        public bool IsBillingAddressRequired { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        //[RequiredIfTrue("IsBillingAddressRequired", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        //[Remote("IsValidBillingAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]        
        public string Address { get; set; }


        //[RequiredIfTrue("IsBillingAddressRequired", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress2), ResourceType = typeof(Resource))]
        //[Remote("IsValidBillingAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]        
        public string AddressLine2 { get; set; }

        //[RequiredIfTrue("IsBillingAddressRequired", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress3), ResourceType = typeof(Resource))]
        //[Remote("IsValidBillingAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]        
        public string AddressLine3 { get; set; }

        //[StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        //  [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength))]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        //[RequiredIfTrue("IsBillingAddressRequired", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblZipOrPostalCode), ResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        //[RequiredIfTrue("IsBillingAddressRequired", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? State { get; set; }

        [Display(Name = nameof(Resource.lblStateProvince), ResourceType = typeof(Resource))]
        public string StateName { get; set; }

        //[RequiredIfTrue("IsBillingAddressRequired", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public int? Country { get; set; }

        [Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public int? CountryGroupId { get; set; }

        [Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public string CountryName { get; set; }

        [Display(Name = nameof(Resource.lblCountyName), ResourceType = typeof(Resource))]
        public string County { get; set; }
    }

    public class TPOCustomerViewModel : StatusViewModel
    {
        public TPOCustomerViewModel()
        {
            IsNewCompany = true;
        }

        [RequiredIfEmpty("Name", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblContactPerson), ResourceType = typeof(Resource))]
        public int? UserId { get; set; }

        [RequiredIfFalse("IsNewCompany", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCompanyName), ResourceType = typeof(Resource))]
        public int? CompanyId { get; set; }
        public bool IsNewCompany { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        //[RequiredIfTrue("IsNewCompany", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMaximumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCompanyName), ResourceType = typeof(Resource))]
        //[Remote("IsOnboardedCompany", "Validation", AreaReference.UseRoot, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.errMessageTPOCannotBeCreatedForOnboardedCompany))]
        //[Remote("IsTPOCompanyExist", "Validation", AreaReference.UseRoot, AdditionalFields = "IsNewCompany,CompanyId", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist), HttpMethod = "POST")]
        public string CompanyName { get; set; }

        [RequiredIf("UserId", null, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMaximumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblContactPerson), ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMobileInvalidLength))]
        [Display(Name = nameof(Resource.lblMobileNumber), ResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }

        public bool IsInvitationEnabled { get; set; }

        public bool IsNotifyDeliveries { get; set; }

        public bool IsNotifySchedules { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }
        public List<ContactPersonViewModel> ContactPersons { get; set; } = new List<ContactPersonViewModel>();
    }

    public class TPOAddressViewModel
    {
        public TPOAddressViewModel()
        {
            State = new StateViewModel();
            Country = new CountryViewModel();
            IsNewJob = true;
            JobLocationType = JobLocationTypes.Location;
            IsVarious = false;
            LocationManagedType = LocationManagedType.NotSpecified;
        }

        [RequiredIfTrue("IsNewJob", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblJobName), ResourceType = typeof(Resource))]
        public string JobName { get; set; }

        [Display(Name = nameof(Resource.lblKiewitJobID), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 0)]
        public string DisplayJobID { get; set; }

        [Display(Name = nameof(Resource.lblWBSNumber), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 0)]
        public string WBSNumber { get; set; }

        [RequiredIfFalse("IsNewJob", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblJobName), ResourceType = typeof(Resource))]
        public int? JobId { get; set; }

        public bool IsNewJob { get; set; }

        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        //[Remote("IsValidTPOJobAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress2), ResourceType = typeof(Resource))]
        //[Remote("IsValidTPOJobAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string AddressLine2 { get; set; }

        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress3), ResourceType = typeof(Resource))]
        //[Remote("IsValidTPOJobAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string AddressLine3 { get; set; }

        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        //[RequiredIfMultiple("IsVarious", true, false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public StateViewModel State { get; set; }


        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public CountryViewModel Country { get; set; }

        
        [Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public int? CountryGroupId { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ZipCode { get; set; }

        public JobLocationTypes JobLocationType { get; set; }
        public bool IsVarious { get; set; }

        public bool IsProFormaPoEnabled { get; set; }

        public bool IsRetailJob { get; set; }
        public bool IsAutomateDeliveryRequest { get; set; } //for bulk upload

        public bool SignatureEnabled { get; set; }

        [Display(Name = nameof(Resource.lblCountyName), ResourceType = typeof(Resource))]
        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string CountyName { get; set; }

        [Display(Name = nameof(Resource.lblLatitude), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d)|(\d+(\.\d{1,8}))|(-\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        //[GreaterThanZeroIf("JobLocationType", JobLocationTypes.GeoLocation, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public decimal Latitude { get; set; }

        public bool IsGeocodeUsed { get; set; }

        [Display(Name = nameof(Resource.lblLongitude), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d)|(\d+(\.\d{1,8}))|(-\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        // [GreaterThanZeroIf("JobLocationType", JobLocationTypes.GeoLocation, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public decimal Longitude { get; set; }

        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        public string OnsiteContactEmail { get; set; }

        [Display(Name = nameof(Resource.lblPhoneNumber), ResourceType = typeof(Resource))]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMobileInvalidLength))]
        public string OnsiteContactPhone { get; set; }

        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        public string OnsiteContactName { get; set; }

        public string OnsiteFirstName { get; set; }
        public string OnsiteLastName { get; set; }
        public int? OnsiteContactUserId { get; set; }
        public bool IsNewContactPerson { get; set; }
        [Display(Name = nameof(Resource.lblTimeZoneName), ResourceType = typeof(Resource))]
        public string TimeZoneName { get; set; }

        [Display(Name = nameof(Resource.lblLocationInventoryManagement), ResourceType = typeof(Resource))]
        public LocationManagedType LocationManagedType { get; set; }

        [Display(Name = nameof(Resource.lblCompanyOwnedLocation), ResourceType = typeof(Resource))]
        public bool IsCompanyOwned { get; set; }

        public bool IsOnsiteContactAvailable()
        {
            var result = !string.IsNullOrWhiteSpace(OnsiteContactEmail) &&
                        !string.IsNullOrWhiteSpace(OnsiteContactName) &&
                        !string.IsNullOrWhiteSpace(OnsiteContactPhone);
            return result;
        }
        public UoM MarineUoM { get; set; }
        public bool IsMarineLocation { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }

        [Display(Name = nameof(Resource.lblVessle), ResourceType = typeof(Resource))]
        public int? VessleId { get; set; }

        [Display(Name = nameof(Resource.lblIMONumber), ResourceType = typeof(Resource))]
        public string IMONumber { get; set; }

        [Display(Name = nameof(Resource.lblFlag), ResourceType = typeof(Resource))]
        public string Flag { get; set; }

        //Needed for on the fly location creation viewmodel 
        public int CountryId { get; set; }
        public int StateId { get; set; }
        [Display(Name = nameof(Resource.lblDistanceCovered), ResourceType = typeof(Resource))]
        public string DistanceCovered { get; set; }
    }

    public class TPOFuelDeliveryViewModel
    {
        public TPOFuelDeliveryViewModel()
        {
            DeliveryTypeId = (int)DeliveryType.OneTimeDelivery;
            StartDate = DateTimeOffset.Now;
            StartTime = Convert.ToDateTime("08:00").ToShortTimeString();
            EndTime = Convert.ToDateTime("17:00").ToShortTimeString();
            DeliverySchedules = new List<DeliveryScheduleViewModel>();
        }

        [Display(Name = nameof(Resource.lblDeliveryType), ResourceType = typeof(Resource))]
        public int DeliveryTypeId { get; set; }

        public string DeliveryTypeName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblStartDate), ResourceType = typeof(Resource))]
        public DateTimeOffset StartDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblDeliveryWindow), ResourceType = typeof(Resource))]
        public string StartTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string EndTime { get; set; }

        public List<DeliveryScheduleViewModel> DeliverySchedules { get; set; }
    }

    public class TPOBrokeredOrderViewModel : BaseViewModel
    {
        public TPOBrokeredOrderViewModel()
        {
            BrokeredOrderFee = new TPOBrokeredOrderFeeViewModel();
            TPOBrokeredCustomerDetails = new TPOBrokeredCustomerViewModel();
        }
        public string VendorId { get; set; }

        public string CustomerNumber { get; set; }

        public string ShipTo { get; set; }

        public string Source { get; set; }

        public string ProductCode { get; set; }

        [Display(Name = nameof(Resource.lblCompanyName), ResourceType = typeof(Resource))]
        public int? CustomerId { get; set; }

        public int InvoicePreferenceId { get; set; }

        public decimal BrokerMarkUp { get; set; }

        public decimal SupplierMarkUp { get; set; }

        public TPOBrokeredOrderFeeViewModel BrokeredOrderFee { get; set; }

        public TPOBrokeredCustomerViewModel TPOBrokeredCustomerDetails { get; set; }

        public int OrderId { get; set; }

        public int? ThirdPartyNozzleId { get; set; }

        public Currency Currency { get; set; }
    }

    public class TPOBrokeredOrderFeeViewModel
    {
        public TPOBrokeredOrderFeeViewModel()
        {
            FreightFeeTypeId = (int)FeeType.FreightFee;
            FreightFeeSubTypeId = (int)FeeSubType.NoFee;
            AdditionalFees = new List<BrokeredOrderFeeViewModel>();
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFreight), ResourceType = typeof(Resource))]
        public int FreightFeeSubTypeId { get; set; }

        public string FreightFeeType { get; set; }

        public string FreightFeeSubType { get; set; }

        [RequiredIf("FreightFeeSubTypeId", (int)FeeSubType.FlatFee, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFlatFee), ResourceType = typeof(Resource))]
        public decimal FreightFee { get; set; }

        public decimal FreightFeeAmount { get; set; }

        public int FreightFeeTypeId { get; set; }

        public List<BrokeredOrderFeeViewModel> AdditionalFees { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }
    }

    public class TPOBrokeredCustomerViewModel
    {
        public int Id { get; set; }

        public bool IsNewCompany { get; set; }

        [RequiredIfTrue("IsNewCompany", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMaximumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCompanyName), ResourceType = typeof(Resource))]
        public string CustomerCompanyName { get; set; }

        [RequiredIfTrue("IsNewCompany", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        public string CustomerEmail { get; set; }

        [RequiredIfTrue("IsNewCompany", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        //[Remote("IsValidCompanyAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

        [RequiredIfTrue("IsNewCompany", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        [RequiredIfTrue("IsNewCompany", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblPhoneNumber), ResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }

        [RequiredIfTrue("IsNewCompany", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public int? StateId { get; set; }

        [RequiredIfTrue("IsNewCompany", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public int? CountryId { get; set; }

        [RequiredIfTrue("IsNewCompany", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        public string ZipCode { get; set; }
    }

    public class TPOSiteImageViewModel
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public ImageViewModel SiteImage { get; set; } = new ImageViewModel();
        public HttpPostedFileBase[] SiteImageFiles { get; set; }
        public AdditionalSiteImage AdditionalImage { get; set; } = new AdditionalSiteImage();
    }

    public class OrderBadgeViewModel
    {
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public List<TerminalBulkBadgeViewModel> TerminalBulkBadge { get; set; }
    }
}
