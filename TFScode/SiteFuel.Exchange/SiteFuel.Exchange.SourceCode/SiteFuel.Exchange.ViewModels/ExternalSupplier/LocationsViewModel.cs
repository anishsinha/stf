using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class LocationsViewModel : BaseViewModel
    {
        public LocationsViewModel()
        {
            IsDefault = true;
            SupplierProfile = new ServicesViewModel();
            SupplierProductTypes = new List<int>();
            Country = new CountryViewModel();
        }

        public LocationsViewModel(Status status)
            : base(status)
        {
            IsDefault = true;
            SupplierProfile = new ServicesViewModel(status);
            SupplierProductTypes = new List<int>();
            Country = new CountryViewModel(status);
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        [Remote("IsValidExternalCompanyAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        [RequiredWithCustomLabel(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public int StateId { get; set; }

        [RequiredWithCustomLabel(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public int CountryId { get; set; }

        public CountryViewModel Country { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]        
        public string ZipCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public int PhoneType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblPhoneNumber), ResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }

        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblDefault), ResourceType = typeof(Resource))]
        public bool IsDefault { get; set; }

        public bool IsOnboarding { get; set; }

        public ServicesViewModel SupplierProfile { get; set; }

        [Display(Name = nameof(Resource.lblSupplierFuelTypes), ResourceType = typeof(Resource))]
        public IList<int> SupplierProductTypes { get; set; }

        public CompanyType CompanyTypeId { get; set; }

    }
}
