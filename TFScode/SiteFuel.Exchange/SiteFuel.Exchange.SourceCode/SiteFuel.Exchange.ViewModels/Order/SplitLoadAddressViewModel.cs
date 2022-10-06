using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class SplitLoadAddressViewModel
    {
        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblSiteName), ResourceType = typeof(Resource))]
        public string SiteName { get; set; }

        [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        //[Remote("IsValidAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Name,Country.Code,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

        [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public int StateId { get; set; }

        public string StateCode { get; set; }

        public int CountryId { get; set; } 

        public string CountryCode { get; set; } 

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        [RequiredIfFalse("IsGeocodeUsed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ZipCode { get; set; }

        public bool IsGeocodeUsed { get; set; }

       
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

        [Display(Name = nameof(Resource.lblCountyName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string CountyName { get; set; }

        public int LocationTypeId { get; set; } = (int)LocationType.Drop;

        public Currency Currency { get; set; }

        public string TimeZoneName { get; set; }

        public string CollectionHtmlPrefix { get; set; }
    }  
}
