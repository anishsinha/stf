using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssociatedUsersGridViewModel : StatusViewModel
    {
        public AssociatedUsersGridViewModel()
        {
            InstanceInitialize();
        }

        public AssociatedUsersGridViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            OnboardedUsers = new List<AdditionalUserViewModel>();
            InvitedUsers = new List<AdditionalUserViewModel>();
        }

        public List<AdditionalUserViewModel> OnboardedUsers { get; set; }

        public List<AdditionalUserViewModel> InvitedUsers { get; set; }
    }
}
