using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
        }

        public LoginViewModel(Status status)
            : base(status)
        {
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Resource.lblPassword), ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [Display(Name = nameof(Resource.lblRememberMe), ResourceType = typeof(Resource))]
        public bool RememberMe { get; set; }
        public string SupplierURL { get; set; }
        public string SupplierLogoPath { get; set; }
        public string BackgroundImagePath { get; set; }
        public string FaviconImagePath { get; set; }
        public string ButtonColor { get; set; }
    }
}
