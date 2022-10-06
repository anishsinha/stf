using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class BillingAddressViewModel : BaseViewModel
    {
        public BillingAddressViewModel()
        {
            State = new StateViewModel();
            Country = new CountryViewModel();
            Phone = new PhoneViewModel();
        }

        public int Id { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        public string Name { get; set; }

      //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress1), ResourceType = typeof(Resource))]
        //[Remote("IsValidBillingAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]        
        public string Address { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress2), ResourceType = typeof(Resource))]
        //[Remote("IsValidBillingAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]        
        public string Address2 { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress3), ResourceType = typeof(Resource))]
        //[Remote("IsValidBillingAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]        
        public string Address3 { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

      //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblZipOrPostalCode), ResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string County { get; set; }

        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public StateViewModel State { get; set; }
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public CountryViewModel Country { get; set; }

        public PhoneViewModel Phone { get; set; }

        public int CompanyId { get; set; }

        [Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public int? CountryGroupId { get; set; }

        public bool IsDefault { get; set; }

        public bool IsMissingAddress()
        {
            return string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(ZipCode);
        }

    }


    public class JobSpecificBillToViewModel
    {
        public JobSpecificBillToViewModel()
        {
            State = new StateViewModel();
            Country = new CountryViewModel();
        }

        public int Id { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress1), ResourceType = typeof(Resource))]
        public string Address { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress2), ResourceType = typeof(Resource))]
        public string AddressLine2 { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress3), ResourceType = typeof(Resource))]
        public string AddressLine3 { get; set; }

        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        [Display(Name = nameof(Resource.lblZipOrPostalCode), ResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        [Display(Name = nameof(Resource.lblCountyName), ResourceType = typeof(Resource))]
        public string County { get; set; }

        public StateViewModel State { get; set; }

        public CountryViewModel Country { get; set; }

        //[Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public int? CountryGroupId { get; set; }

        public bool IsExistingBillAddress { get; set; }

        public int? BillingAddressId { get; set; }

        public int? CompanyId { get; set; }
    }


    public class JobSpecificBillingInfoViewModel
    {
        public bool IsJobSpecificBillToEnabled { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress { get; set; }
        public string BillToAddressLine2 { get; set; }
        public string BillToAddressLine3 { get; set; }
        public string BillToCity { get; set; }
        public string BillToCounty { get; set; }
        public string BillToZipCode { get; set; }
        public string BillToStateCode { get; set; } 
        public string BillToStateName { get; set; }
        public string BillToCountryCode { get; set; }
        public string BillToCountryName { get; set; }
    }
}
