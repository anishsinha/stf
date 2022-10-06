using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels.Offer
{
    public class OfferOrderViewModel : StatusViewModel
    {
        public OfferOrderViewModel()
        {
            FuelDetails = new FuelDetailsViewModel()
            {
                FuelDisplayGroupId = (int)ProductDisplayGroups.CommonFuelType,
                IsOverageAllowed = false,
                IsDifferentFuelPriceEntered = false
            };
            AddressDetails = new OfferAddressViewModel()
            {
                Country = new CountryViewModel()
                {
                    Id = (int)Country.USA,
                    Currency = Currency.USD,
                    UoM = UoM.Gallons
                }
            };
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel();
            FuelOfferDetails = new FuelOfferDetailsViewModel();
        }

        public int OfferPricingId { get; set; }
        public OfferAddressViewModel AddressDetails { get; set; }
        public FuelDetailsViewModel FuelDetails { get; set; }
        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }
        public FuelOfferDetailsViewModel FuelOfferDetails { get; set; }
        public List<DropdownDisplayItem> FuelTypes { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class OfferAddressViewModel
    {
        public OfferAddressViewModel()
        {
            State = new StateViewModel();
            Country = new CountryViewModel();
            IsNewJob = true;
        }

        [RequiredIfTrue("IsNewJob", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblJobName), ResourceType = typeof(Resource))]
        public string JobName { get; set; }

        [Display(Name = nameof(Resource.lblKiewitJobID), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 0)]
        public string DisplayJobID { get; set; }

        [RequiredIfFalse("IsNewJob", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblJobName), ResourceType = typeof(Resource))]
        public int? JobId { get; set; }

        public bool IsNewJob { get; set; }

        [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        [Remote("IsValidTPOJobAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

        [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public StateViewModel State { get; set; }

        [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public CountryViewModel Country { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
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

        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblOnsiteContactEmail), ResourceType = typeof(Resource))]
        public string OnsiteContactEmail { get; set; }

        [Display(Name = nameof(Resource.lblOnsiteContactPhone), ResourceType = typeof(Resource))]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string OnsiteContactPhone { get; set; }

        [Display(Name = nameof(Resource.lblOnsiteContactName), ResourceType = typeof(Resource))]
        public string OnsiteContactName { get; set; }

        public string OnsiteFirstName { get; set; }
        public string OnsiteLastName { get; set; }
        public int OnsiteContactUserId { get; set; }

        [Display(Name = nameof(Resource.lblTimeZoneName), ResourceType = typeof(Resource))]
        public string TimeZoneName { get; set; }

    }
}
