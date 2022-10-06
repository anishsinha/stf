using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class ContactPersonViewModel : StatusViewModel
    {
        public ContactPersonViewModel()
        {
        }

        public ContactPersonViewModel(Status status) : base(status)
        {
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Name { get; set; }

        [Display(Name = nameof(Resource.lblMobileNumber), ResourceType = typeof(Resource))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMobileInvalidLength))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string PhoneNumber { get; set; }

        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Email { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }
    }
}
