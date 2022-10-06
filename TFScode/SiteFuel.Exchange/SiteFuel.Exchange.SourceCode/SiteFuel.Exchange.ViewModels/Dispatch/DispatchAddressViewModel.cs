using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class DispatchAddressViewModel : StatusViewModel
    {
       // [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        //[Remote("IsValidAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Name,Country.Code,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        // [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

      //  [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public StateViewModel State { get; set; } = new StateViewModel();

      //  [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public CountryViewModel Country { get; set; } = new CountryViewModel();

        [Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public DropdownDisplayExtendedItem CountryGroup { get; set; } = new DropdownDisplayExtendedItem();

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
      //  [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ZipCode { get; set; }

        public bool IsGeocodeUsed { get; set; }

        public bool IsProFormaPoEnabled { get; set; }

        public bool SignatureEnabled { get; set; }

        [Display(Name = nameof(Resource.lblCountyName), ResourceType = typeof(Resource))]
       // [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string CountyName { get; set; }

        [Display(Name = nameof(Resource.lblLatitude), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d)|(\d+(\.\d{1,8}))|(-\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
       // [RequiredIfTrue("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
       // [GreaterThanZeroIf("IsGeocodeUsed", true, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public decimal Latitude { get; set; }

        [Display(Name = nameof(Resource.lblLongitude), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d)|(\d+(\.\d{1,8}))|(-\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
      //  [RequiredIfTrue("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
       // [GreaterThanZeroIf("IsGeocodeUsed", true, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public decimal Longitude { get; set; }

        [Display(Name = nameof(Resource.lblTimeZoneName), ResourceType = typeof(Resource))]
        public string TimeZoneName { get; set; }

        [Display(Name = nameof(Resource.lblBulkPlant), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string SiteName { get; set; }

        public int SiteId { get; set; }

        public bool IsMissingAddress()
        {
            return string.IsNullOrWhiteSpace(ZipCode) || string.IsNullOrWhiteSpace(CountyName) || string.IsNullOrWhiteSpace(Address)
                || string.IsNullOrWhiteSpace(City) || Latitude ==0 || Longitude == 0;
        }

    }

    public class DropAddressViewModel
    {
        [RequiredIfTrue("IsAddressAvailable", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        //[Remote("IsValidAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Name,Country.Code,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        [RequiredIfTrue("IsAddressAvailable", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        [RequiredIfTrue("IsAddressAvailable", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public StateViewModel State { get; set; } = new StateViewModel();

        [RequiredIfTrue("IsAddressAvailable", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public CountryViewModel Country { get; set; } = new CountryViewModel();

        public NullableDropdownDisplayExtendedItem CountryGroup { get; set; } = new NullableDropdownDisplayExtendedItem();



        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        [RequiredIfTrue("IsAddressAvailable", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ZipCode { get; set; }

        public bool IsGeocodeUsed { get; set; }

        public bool IsProFormaPoEnabled { get; set; }

        public bool SignatureEnabled { get; set; }

        public string CountyName { get; set; }

        [Display(Name = nameof(Resource.lblLatitude), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d)|(\d+(\.\d{1,8}))|(-\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIfTrue("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [GreaterThanZeroIf("IsGeocodeUsed", true, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public decimal Latitude { get; set; }

        [Display(Name = nameof(Resource.lblLongitude), ResourceType = typeof(Resource))]
        [RegularExpression(@"^((\d)|(\d+(\.\d{1,8}))|(-\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIfTrue("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [GreaterThanZeroIf("IsGeocodeUsed", true, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public decimal Longitude { get; set; }

        [Display(Name = nameof(Resource.lblTimeZoneName), ResourceType = typeof(Resource))]
        public string TimeZoneName { get; set; }

        [Display(Name = nameof(Resource.lblWeHaveDropAddress), ResourceType = typeof(Resource))]
        public bool IsAddressAvailable { get; set; }

        [Display(Name = nameof(Resource.lblBulkPlant), ResourceType = typeof(Resource))]
        [RequiredIfTrue("IsAddressAvailable", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string SiteName { get; set; }
        public int SiteId { get; set; }
        public int? BulkPlantId { get; set; }
        public override bool Equals(object obj)
        {
            var other = obj as DropAddressViewModel;

            if (other == null)
                return false;

            if ((Latitude == other.Latitude && Longitude == other.Longitude) || (Address == other.Address && City == other.City && ZipCode == other.ZipCode))
                return true;
            else
                return false;
        }

        public bool IsMissingAddress()
        {
            return string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(ZipCode) || string.IsNullOrWhiteSpace(CountyName)
                || Latitude == 0 || Longitude == 0;
        }
    }
}
