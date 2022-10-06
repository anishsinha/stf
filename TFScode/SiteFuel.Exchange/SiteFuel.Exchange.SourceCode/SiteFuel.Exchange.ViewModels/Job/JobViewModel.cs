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
    public class JobViewModel : BaseViewModel
    {
        public JobViewModel()
        {
            InstanceInitialize();
        }

        public JobViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            StatusId = (int)JobStatus.Draft;
            CreatedDate = DateTimeOffset.Now;
            StartDate = DateTimeOffset.Now;
            ReopenDate = DateTimeOffset.Now;
            State = new StateViewModel(status);
            Country = new CountryViewModel(status);
            IsBackdatedJob = true;
            AssignedTo = new List<int>();
            OnsiteContacts = new List<int>();
            OnsiteContactPersons = new List<ContactPersonViewModel>();
            DeliveryDaysList = new List<DeliveryDaysViewModel>();
            JobLicenses = new List<int>();
            IsVarious = false;
            LocationType = JobLocationTypes.Location;
            LocationManagedType = LocationManagedType.NotSpecified;
            ForcastingPreference = new ForcastingPreferenceViewModel();
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Remote("IsValidJobName", "Validation", AreaReference.UseRoot, AdditionalFields = "Id", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 0)]
        [Display(Name = nameof(Resource.lblKiewitJobID), ResourceType = typeof(Resource))]
        [Remote("IsValidSiteId", "Validation", AreaReference.UseRoot, AdditionalFields = "Id", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string JobID { get; set; }

        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        //[Remote("IsValidJobAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        //[RequiredIfMultiple("IsVarious", true, false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public StateViewModel State { get; set; }

        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public CountryViewModel Country { get; set; }

        [Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public int? CountryGroupId { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ZipCode { get; set; }

        public bool IsGeocodeUsed { get; set; }

        [Display(Name = nameof(Resource.lblCountyName), ResourceType = typeof(Resource))]
        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string CountyName { get; set; }

        [Display(Name = nameof(Resource.lblLatitude), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d)|(\d+(\.\d{1,8}))|(-\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [GreaterThanZeroIf("LocationType", JobLocationTypes.GeoLocation, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public decimal Latitude { get; set; }

        [Display(Name = nameof(Resource.lblLongitude), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d)|(\d+(\.\d{1,8}))|(-\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        //[RequiredIfFalse("IsVarious", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [GreaterThanZeroIf("LocationType", JobLocationTypes.GeoLocation, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public decimal Longitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblStatus), ResourceType = typeof(Resource))]
        public int StatusId { get; set; }

        [Display(Name = nameof(Resource.lblIsBackdatedJob), ResourceType = typeof(Resource))]
        public bool IsBackdatedJob { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = nameof(Resource.lblStartDate), ResourceType = typeof(Resource))]
        [LessThan("EndDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageLessThan), PassOnNull = true)]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Remote("IsFuelRequestExistForJobStartDate", "Validation", AreaReference.UseRoot, ErrorMessageResourceType = typeof(Resource), AdditionalFields = "Id", ErrorMessageResourceName = nameof(Resource.valMessageJobStartDate))]
        public DateTimeOffset StartDate { get; set; }

        [Display(Name = nameof(Resource.lblEndDate), ResourceType = typeof(Resource))]
        [GreaterThan("StartDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        [Remote("IsFuelRequestExistForJobEndDate", "Validation", AreaReference.UseRoot, ErrorMessageResourceType = typeof(Resource), AdditionalFields = "Id", ErrorMessageResourceName = nameof(Resource.valMessageJobEndDate))]
        public Nullable<DateTimeOffset> EndDate { get; set; }

        public bool IsReopened { get; set; }

        public DateTimeOffset ReopenDate { get; set; }

        [Display(Name = nameof(Resource.lblEnableAssetTracking), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public bool IsAssetTracked { get; set; }

        [Display(Name = nameof(Resource.lblEnableStatusForAllAssets), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public bool IsAssetDropStatusEnabled { get; set; }

        public bool IsProFormaPoEnabled { get; set; }

        public bool IsRetailJob { get; set; }
        public bool IsAutoCreateDREnable { get; set; }

        public bool IsOpenFuelRequestsExist { get; set; }

        public Nullable<int> PoContactId { get; set; }

        public List<int> AssignedTo { get; set; }

        public List<int> OnsiteContacts { get; set; }

        [RequiredIfTrue("IsApprovalWorkFlowEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblApprovalUser), ResourceType = typeof(Resource))]
        public int? ApprovalUser { get; set; }

        [Display(Name = nameof(Resource.lblEnable), ResourceType = typeof(Resource))]
        public bool IsApprovalWorkFlowEnabled { get; set; }

        [Display(Name = nameof(Resource.lblEnable), ResourceType = typeof(Resource))]
        public bool SignatureEnabled { get; set; }

        [Display(Name = nameof(Resource.lblEnable), ResourceType = typeof(Resource))]
        public bool IsResaleEnabled { get; set; }

        [Display(Name = nameof(Resource.lblContractNumber), ResourceType = typeof(Resource))]
        public string ContractNumber { get; set; }

        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblCustomerEmail), ResourceType = typeof(Resource))]
        public string CustomerEmail { get; set; }

        [Display(Name = nameof(Resource.lblCustomerName), ResourceType = typeof(Resource))]
        public string CustomerName { get; set; }

        public List<ContactPersonViewModel> OnsiteContactPersons { get; set; }

        [Display(Name = nameof(Resource.lblTimeZoneName), ResourceType = typeof(Resource))]
        public string TimeZoneName { get; set; }

        public bool AllowBuyFuel { get; set; }

        public List<int> JobLicenses { get; set; }

        public bool IsWaitingForApprovalExists { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public JobLocationTypes LocationType { get; set; }
        public bool IsVarious { get; set; }

        [Display(Name = nameof(Resource.headingDeliveryDays), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public List<DeliveryDaysViewModel> DeliveryDaysList { get; set; }
        public string SiteInstructions { get; set; }
        public ImageViewModel SiteImage { get; set; } = new ImageViewModel();
        public HttpPostedFileBase[] SiteImageFiles { get; set; }
        public AdditionalSiteImage AdditionalImage { get; set; } = new AdditionalSiteImage();
        public bool IsJobSpecificBillToEnabled { get; set; }
        public JobSpecificBillToViewModel BillToInfo { get; set; } = new JobSpecificBillToViewModel();

        public int AssignedCarrierCompId { get; set; }

        [Display(Name = nameof(Resource.lblTrailerType), ResourceType = typeof(Resource))]
        public List<TrailerTypeStatus> TrailerType { get; set; }
        public string AccountingCompanyId { get; set; }

        public LocationManagedType LocationManagedType { get; set; }

        [Display(Name = nameof(Resource.lblLocationInventoryManged), ResourceType = typeof(Resource))]
        public List<LocationInventoryManagedBy> LocationInventoryManagedBy { get; set; }
        public bool IsCompanyOwned { get; set; }
        public ForcastingPreferenceViewModel ForcastingPreference { get; set; } = new ForcastingPreferenceViewModel();
        public bool IsMarine { get; set; }
        public UoM MarineUom { get; set; }
        public string DistanceCovered { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
        public string ExternalRefId { get; set; }
    }

    public class AdditionalSiteImage
    {
        public string Description { get; set; }
        public ImageViewModel SiteImage { get; set; } = new ImageViewModel();
        public HttpPostedFileBase[] SiteImageFiles { get; set; }
    }

    public class JobCoordinates
    {
        public int JobId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
