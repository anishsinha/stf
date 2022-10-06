using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyAddressViewModel : BaseViewModel
    {
        public CompanyAddressViewModel()
        {
            Initialize();
            State = new StateViewModel();
            Country = new CountryViewModel();
            SupplierProfile = new SupplierProfileViewModel();
        }

        public CompanyAddressViewModel(Status status)
            : base(status)
        {
            Initialize();
            State = new StateViewModel(status);
            Country = new CountryViewModel(status);
            SupplierProfile = new SupplierProfileViewModel(status);
        }

        private void Initialize()
        {
            IsDefault = true;
            SupplierWorkingHours = new List<SupplierWorkingHoursViewModel>();
            SupplierProductTypes = new List<int>();
            BillingAddress = new BillingAddressViewModel();
            Phone = new PhoneViewModel();
        }

        public int Id { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddressLine1), ResourceType = typeof(Resource))]
        //[Remote("IsValidCompanyAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress2), ResourceType = typeof(Resource))]
        public string Address2{ get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress3), ResourceType = typeof(Resource))]
        public string Address3 { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public StateViewModel State { get; set; }
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public CountryViewModel Country { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public PhoneViewModel Phone { get; set; }

        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblDefault), ResourceType = typeof(Resource))]
        public bool IsDefault { get; set; }

        public bool IsOnboarding { get; set; }

        public SupplierProfileViewModel SupplierProfile { get; set; }

        public IList<SupplierWorkingHoursViewModel> SupplierWorkingHours { get; set; }

        [Display(Name = nameof(Resource.lblSupplierProductTypes), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public IList<int> SupplierProductTypes { get; set; }

        public CompanyType CompanyTypeId { get; set; }

        public BillingAddressViewModel BillingAddress { get; set; }

        [Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public int? CountryGroupId { get; set; }
        public List<CompanyServiceAreaModel> ServiceOffering { get; set; } = new List<CompanyServiceAreaModel>();

        public bool IsMissingAddress()
        {
            return string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(ZipCode);
        }
    }
}
