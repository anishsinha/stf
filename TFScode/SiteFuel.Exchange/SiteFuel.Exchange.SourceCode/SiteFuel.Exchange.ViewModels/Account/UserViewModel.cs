using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            InstanceInitialize();
        }

        public UserViewModel(AuthStatus status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(AuthStatus status = AuthStatus.Failed)
        {
            IsActive = true;
            UpdatedBy = (int)SystemUser.System;
            UpdatedDate = DateTimeOffset.Now;
            Roles = new List<RoleViewModel>();
            StatusCode = status;
            IsBuyerTPOCreated = false;

            if (status == AuthStatus.Success)
            {
                StatusMessage = Resource.errMessageSuccess;
            }
            else
            {
                StatusMessage = Resource.errMessageFailed;
            }
            ApplicationTemplateId = (int)ApplicationTemplate.TrueFill;
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMaximumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblFirstName), ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256)]
        [Display(Name = nameof(Resource.lblLastName), ResourceType = typeof(Resource))]
        public string LastName { get; set; }

        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMobileInvalidLength))]
        [Display(Name = nameof(Resource.lblMobileNumber), ResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lbl2FAuthentication), ResourceType = typeof(Resource))]
        public bool IsTwoFactorEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblAccountLockout), ResourceType = typeof(Resource))]
        public bool IsLockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEndDateUtc { get; set; }

        public string FingerPrint { get; set; }

        public bool IsOnboardingComplete { get; set; }

        public DateTimeOffset? OnboardedDate { get; set; }

        public IList<RoleViewModel> Roles { get; set; }

        public int CompanyId { get; set; }

        public int CompanyTypeId { get; set; }

        public string CompanyName { get; set; }

        public int? CompanyLogoId { get; set; }

        public string CompanyLogo { get; set; } = string.Empty;
        public bool IsBuyerTPOCreated { get; set; }

        public bool IsFirstLogin { get; set; }

        public bool IsSalesCalculatorAllowed { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public AuthStatus StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public int OnboardedTypeId { get; set; }
        public string SupplierURL { get; set; }
        public string ButtonColor { get; set; }
        public string SupplierLogoPath { get; set; }
        //public string SupplierFaviconPath { get; set; }

        [Display(Name = nameof(Resource.lblIsApiAccessAllowed), ResourceType = typeof(Resource))]
        public bool IsApiAccessAllowed { get; set; }

        public int CompanyDefaultCountry { get; set; }
        public int BrandedCompanyId { get; set; } = -1;

        public int ApplicationTemplateId { get; set; }
        public string Title { get; set; }
    }
}
