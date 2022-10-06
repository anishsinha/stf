using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class RegisterAdditionalUserViewModel : StatusViewModel
    {
        public RegisterAdditionalUserViewModel()
        {
            Company = new CompanyViewModel();
        }

        public RegisterAdditionalUserViewModel(Status status)
            : base(status)
        {
            Company = new CompanyViewModel(status);
        }

        public int Id { get; set; }

        public CompanyViewModel Company { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFirstName), ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblLastName), ResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        [Remote("IsAdditionalUserEmailExist", "Validation", AreaReference.UseRoot, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMobileInvalidLength))]
        [Display(Name = nameof(Resource.lblMobileNumber), ResourceType = typeof(Resource))]
        public string MobileNumber { get; set; }

        [RequiredIfFalse("IsRegisterThroughOkta", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 8)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[\w~@#$%^&*+=`|{}:;!.?\()\[\]-]{8,}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valPasswordValidate))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Resource.lblPassword), ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [RequiredIfFalse("IsRegisterThroughOkta", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessagePasswordMismatch))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Resource.lblConfirmPassword), ResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }

        public int AdditionalUserId { get; set; }

        public UserRoles UserRole { get; set; }

        public bool IsAccountSfxOwned { get; set; }
        public bool IsRegisterThroughOkta { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }

        public string SupplierLogoPath { get; set; }
        public string FaviconImagePath { get; set; }

        public string SupplierURL { get; set; }

        public string BackgroundImagePath { get; set; }

        public string ButtonColor { get; set; }
        
        public int InvitedBy { get; set; }
        public List<int> RoleIds { get; set; }


    }
}
