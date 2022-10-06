using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryGroupPickupViewModel
    {
        public bool IsPickupLocation { get; set; }
        [Display(Name = nameof(Resource.lblTerminal), ResourceType = typeof(Resource))]
        [RequiredIfFalse("IsPickupLocation", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
       // [RequiredIfTrue("IsPickupLocation", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string PickupAddress { get; set; }
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        //[RequiredIfTrue("IsPickupLocation", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string PickupCity { get; set; }
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        [RequiredIfTrue("IsPickupLocation", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? PickupStateId { get; set; }
        public string PickupStateCode { get; set; }
        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
       // [RequiredIfTrue("IsPickupLocation", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string PickupZipCode { get; set; }
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        [RequiredIfTrue("IsPickupLocation", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? PickupCountryId { get; set; }
        [Display(Name = nameof(Resource.lblCountyName), ResourceType = typeof(Resource))]
        [RequiredIfTrue("IsPickupLocation", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string PickupCountryCode { get; set; }
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public string PickupCountyName { get; set; }
        [Display(Name = nameof(Resource.lblGeoCodes), ResourceType = typeof(Resource))]
        public bool IsPickupGeocodeUsed { get; set; }
        [Display(Name = nameof(Resource.lblLatitude), ResourceType = typeof(Resource))]
        public decimal? PickupLatitude { get; set; }
        [Display(Name = nameof(Resource.lblLongitude), ResourceType = typeof(Resource))]
        public decimal? PickupLongitude { get; set; }
        [Display(Name = nameof(Resource.lblTimeZoneName), ResourceType = typeof(Resource))]
        public string PickupTimeZone { get; set; }

        [Display(Name = nameof(Resource.lblBulkPlant), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string SiteName { get; set; }
        public int SiteId { get; set; }

        [Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public int PickupCountryGroupId { get; set; }

        public bool IsMissingAddress()
        {
            return string.IsNullOrWhiteSpace(PickupAddress) || string.IsNullOrWhiteSpace(PickupCity) || string.IsNullOrWhiteSpace(PickupZipCode)
                || string.IsNullOrWhiteSpace(PickupCountyName);                  
        }
    }
}
