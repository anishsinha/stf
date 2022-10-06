using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobContactViewModel : BaseViewModel
    {
        public JobContactViewModel()
        {
            InstanceInitialize();
        }

        public JobContactViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            RoleIds = new List<int>();
            DisplayMode = PageDisplayMode.Create;
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFirstName), ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblLastName), ResourceType = typeof(Resource))]
        public string LastName { get; set; }

        public int CompanyId { get; set; }

        public int CompanyTypeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        public string Email { get; set; }

        public int InvitedById { get; set; }

        public string InvitedBy { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.gridColumnRoles), ResourceType = typeof(Resource))]
        public IList<int> RoleIds { get; set; }

        public bool IsInvitationSent { get; set; }

        public string RoleNames { get; set; }
    }
}
