using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using SiteFuel.Exchange.ViewModels.Forcasting;
using SiteFuel.Exchange.ViewModels.Job;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobViewModelForSuperAdmin : BaseViewModel
    {
        public JobViewModelForSuperAdmin()
        {
            InstanceInitialize();
        }

        public JobViewModelForSuperAdmin(Status status)
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
            CountryGroup = new DropdownDisplayExtendedItem();
            IsBackdatedJob = true;
            LocationType = JobLocationTypes.Location;
            DeliveryDaysList = new List<DeliveryDaysViewModel>();
            ImageDetails = new TPOSiteImageViewModel();
            ForcastingPreference = new ForcastingPreferenceViewModel();
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Remote("IsValidJobName", "Validation", AreaReference.UseRoot, AdditionalFields = "Id", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 0)]
        [Display(Name=nameof(Resource.lblKiewitJobID), ResourceType = typeof(Resource))]
        public string JobID { get; set; }

        //[RequiredIf("LocationType", JobLocationTypes.Location, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        //[Remote("IsValidJobAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

       // [RequiredIf("LocationType", JobLocationTypes.Location, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        [RequiredIf("LocationType", JobLocationTypes.Location, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public StateViewModel State { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public CountryViewModel Country { get; set; }

        [Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public DropdownDisplayExtendedItem CountryGroup { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
       // [RequiredIf("LocationType", JobLocationTypes.Location, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ZipCode { get; set; }

        public bool IsGeocodeUsed { get; set; }

        [Display(Name = nameof(Resource.lblCountyName), ResourceType = typeof(Resource))]
       // [RequiredIf("LocationType", JobLocationTypes.Location, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string CountyName { get; set; }

        [Display(Name = nameof(Resource.lblLatitude), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d)|(\d+(\.\d{1,8}))|(-\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIf("LocationType", JobLocationTypes.GeoLocation, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [GreaterThanZeroIf("LocationType", JobLocationTypes.GeoLocation, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public decimal Latitude { get; set; }

        [Display(Name = nameof(Resource.lblLongitude), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d)|(\d+(\.\d{1,8}))|(-\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIf("LocationType", JobLocationTypes.GeoLocation, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
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

        public bool IsTaxExempted { get; set; }

        [Display(Name = nameof(Resource.lblEnableStatusForAllAssets), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public bool IsAssetDropStatusEnabled { get; set; }

        public bool IsProFormaPoEnabled { get; set; }

        public bool IsOpenFuelRequestsExist { get; set; }

        [Display(Name = nameof(Resource.lblEnable), ResourceType = typeof(Resource))]
        public bool SignatureEnabled { get; set; }

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

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public JobLocationTypes LocationType { get; set; }

        [Display(Name = nameof(Resource.AutoDeliveryRequest), ResourceType = typeof(Resource))]
        public bool IsAutoCreateDREnable { get; set; }

        [Display(Name = nameof(Resource.headingDeliveryDays), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public List<DeliveryDaysViewModel> DeliveryDaysList { get; set; }
        public string SiteInstructions { get; set; }

        public bool IsRetailJob { get; set; }

        [Display(Name = nameof(Resource.lblDispatchRegion),ResourceType =typeof(Resource))]
        public string RegionId { get; set; }

        public int SupplierCompanyId { get; set; }

        public List<TrailerTypeStatus> TrailerType { get; set; }

        public TPOSiteImageViewModel ImageDetails { get; set; }
        public string AccountingCompanyId { get; set; }

        [Display(Name = nameof(Resource.lblLocationInventoryManged), ResourceType = typeof(Resource))]
        public List<LocationInventoryManagedBy> LocationInventoryManagedBy { get; set; }
        public ForcastingPreferenceViewModel ForcastingPreference { get; set; }
        
        [Display(Name = nameof(Resource.lblDistanceCovered), ResourceType = typeof(Resource))]
        public string DistanceCovered { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }

        public List<OnsiteJobUserViewModel> OnSiteContactUserInfo { get; set; }

        public bool IsMissingAddress()
        {
            return string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(ZipCode)
                || string.IsNullOrWhiteSpace(CountyName);
        }

    }
}
