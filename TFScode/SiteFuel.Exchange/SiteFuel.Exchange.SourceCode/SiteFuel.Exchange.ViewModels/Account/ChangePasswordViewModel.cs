using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class ChangePasswordViewModel : StatusViewModel
    {
        public ChangePasswordViewModel()
        {
        }

        public ChangePasswordViewModel(Status status)
            : base(status)
        {
        }

        public int UserId { get; set;  }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Resource.lblOldPassword), ResourceType = typeof(Resource))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 8)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[\w~@#$%^&*+=`|{}:;!.?\()\[\]-]{8,}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valPasswordValidate))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Resource.lblNewPassword), ResourceType = typeof(Resource))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessagePasswordMismatch))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Resource.lblConfirmPassword), ResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }
}