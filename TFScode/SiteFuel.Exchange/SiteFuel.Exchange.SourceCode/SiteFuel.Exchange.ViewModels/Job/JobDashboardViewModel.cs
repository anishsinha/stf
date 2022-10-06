using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobDashboardViewModel : StatusViewModel
    {
        public JobDashboardViewModel()
        {
            InstanceInitialize();
        }

        public JobDashboardViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            JobList = new List<JobGridViewModel>();
        }

        public int UserId { get; set; }

        public List<JobGridViewModel> JobList { get; set; }
    }
}
