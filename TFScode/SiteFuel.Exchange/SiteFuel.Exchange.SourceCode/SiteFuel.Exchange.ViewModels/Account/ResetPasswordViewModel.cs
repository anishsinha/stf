using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class ResetPasswordViewModel : StatusViewModel
    {
        public ResetPasswordViewModel()
        {
            DisbleEmail = true;
        }

        public ResetPasswordViewModel(Status status)
            : base(status)
        {
            DisbleEmail = true;
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        public string Email { get; set; }

        public bool DisbleEmail { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 8)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[\w~@#$%^&*+=`|{}:;!.?\()\[\]-]{8,}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valPasswordValidate))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Resource.lblPassword), ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessagePasswordMismatch))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Resource.lblConfirmPassword), ResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }

        public string SupplierURL { get; set; }
        public string SupplierLogoPath { get; set; }
        public string FaviconImagePath { get; set; }
        public string BackgroundImagePath { get; set; }
        public string ButtonColor { get; set; }
    }
}
