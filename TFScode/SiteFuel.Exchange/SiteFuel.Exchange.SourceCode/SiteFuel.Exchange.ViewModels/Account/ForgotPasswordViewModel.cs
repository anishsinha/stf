using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public ForgotPasswordViewModel()
        {
            InstanceInitialize();
        }

        public ForgotPasswordViewModel(AuthStatus status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(AuthStatus status = AuthStatus.Failed)
        {
            if (status == AuthStatus.Success)
            {
                StatusCode = AuthStatus.Success;
                StatusMessage = Resource.errMessageSuccess;
            }
            else
            {
                StatusCode = AuthStatus.Failed;
                StatusMessage = Resource.errMessageFailed;
            }
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        public string Email { get; set; }

        public AuthStatus StatusCode { get; set; }

        public string StatusMessage { get; set; }
        public string SupplierURL { get; set; }
        public string SupplierLogoPath { get; set; }
        public string BackgroundImagePath { get; set; }
        public string FaviconImagePath { get; set; }
        public string ButtonColor { get; set; }
    }
}
