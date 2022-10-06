using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeclineReasonViewModel : StatusViewModel
    {
        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblAdditionalNotes), ResourceType = typeof(Resource))]
        [RequiredIf("ReasonId", (int)InvoiceDeclinedReason.Other, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string AdditionalNotes { get; set; }

        [Display(Name = nameof(Resource.lblReason), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int ReasonId { get; set; }

        public int InvoiceStatusId { get; set; }
    }
}
