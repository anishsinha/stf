using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class PhoneViewModel : StatusViewModel
    {
        public PhoneViewModel()
        {
        }

        public PhoneViewModel(Status status)
            : base(status)
        {
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public int PhoneType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessagePhoneInvalidLength))]
        [Display(Name = nameof(Resource.lblPhoneNumber), ResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }
    }
}
