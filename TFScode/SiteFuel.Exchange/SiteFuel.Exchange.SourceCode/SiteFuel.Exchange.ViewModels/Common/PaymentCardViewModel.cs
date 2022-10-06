using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class PaymentCardViewModel : BaseViewModel
    {
        public PaymentCardViewModel()
        {
        }

        public PaymentCardViewModel(Status status)
            : base(status)
        {
        }

        public bool BypassPaymentDetails { get; set; } = true;

        //[RequiredIf("BypassPaymentDetails", false,ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblNameOnCard), ResourceType = typeof(Resource))]
        public string NameOnCard { get; set; }

        [RequiredIf("BypassPaymentDetails", false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblNumber), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(19, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 16)]
        [RegularExpression(@"(^4\d{15,18}$)|(^3\d{14,18}$)|(^35[28-89]\d{12,15}$)|(^5\d{11,18})$|(^5[1-5]\d{14,17}$)|(^2\d{15,18}$)|(^6\d{15,18}$)", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string CardNumber { get; set; }

        //[RequiredIf("BypassPaymentDetails", false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCvv), ResourceType = typeof(Resource))]
        [StringLength(4, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 3)]
        [RegularExpression(@"^\d{3,4}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Cvv { get; set; }

        //[RequiredIf("BypassPaymentDetails", false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblExpiration), ResourceType = typeof(Resource))]
        //[RegularExpression(@"^(1[0-2]|0[1-9]|[1-9])$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public int ExpirationMonth { get; set; }

        //[RequiredIf("BypassPaymentDetails", false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblExpiration), ResourceType = typeof(Resource))]
        //[RegularExpression(@"^\d{2}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public int ExpirationYear { get; set; }

        //[RequiredIf("BypassPaymentDetails", false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblPrimaryCard), ResourceType = typeof(Resource))]
        public bool IsPrimary { get; set; }

        public bool IsPrimaryVisible { get; set; }
    }
}
