using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class AdditionalUserViewModel : BaseViewModel
    {
        public AdditionalUserViewModel()
        {
            InstanceInitialize();
        }

        public AdditionalUserViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
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

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        public string Email { get; set; }

        public int InvitedById { get; set; }

        public string InvitedBy { get; set; }

        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.gridColumnRoles), ResourceType = typeof(Resource))]
        public IList<int> RoleIds { get; set; }

        public List<int> EventTypeId { get; set; }

        public bool IsInvitationSent { get; set; }

        public string RoleNames { get; set; }

        public List<string> RoleNameList { get; set; }

        public string Message { get; set; }

        public bool IsOnboarded { get; set; }

        public string Name { get; set; }

        public string CompanyName { get; set; }

        public bool IsInvitedUser { get; set; }

        public int CompanyTypeId { get; set; }

        public string PhoneNumber { get; set; }

        public int DriverUserId { get; set; }

        [Display(Name = nameof(Resource.lblIsApiAccessAllowed), ResourceType = typeof(Resource))]
        public bool IsApiAccessAllowed { get; set; }

        public DriverInformationViewModel DriverInfo { get; set; } = new DriverInformationViewModel();
        public int DriverId { get; set; }
        public bool IsScheduleExists { get; set; } = false;
        public List<string> ScheduleBuilderIds = new List<string>();
        public string ScheduleBuilderIdInfo { get { return string.Join(",", ScheduleBuilderIds); } }
        public string DT_RowId { get; set; }
        public string Title { get; set; }
    }
}
