using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class RequestFuelViewModel
    {
        public int RequestPriceId { get; set; }

        [Display(Name = nameof(Resource.lblFirstName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string FirstName { get; set; }

        [Display(Name = nameof(Resource.lblLastName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string LastName { get; set; }

        [Display(Name = nameof(Resource.lblPhoneNumber), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string PhoneNumber { get; set; }

        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Email { get; set; }

        [Display(Name = nameof(Resource.lblCompanyName), ResourceType = typeof(Resource))]
        public string CompanyName { get; set; }

        public DateTimeOffset RequestDateTime { get; set; }

        public bool IsEmailSentToSales { get; set; }

        public DateTimeOffset? EmailSentDateTime { get; set; }

        public bool IsCustomerContacted { get; set; }

        public DateTimeOffset? CustomerContactedDateTime { get; set; }

        public bool IsBusinessDone { get; set; }
    }
}
