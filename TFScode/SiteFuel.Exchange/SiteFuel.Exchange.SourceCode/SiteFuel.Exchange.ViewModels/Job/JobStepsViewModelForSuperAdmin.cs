using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Forcasting;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobStepsViewModelForSuperAdmin : BaseCultureViewModel
    {
        public JobStepsViewModelForSuperAdmin()
        {
            InstanceInitialize();
        }

        public JobStepsViewModelForSuperAdmin(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            Job = new JobViewModelForSuperAdmin(status);
        }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public int CompanyTypeId { get; set; }
        public int UserOnboardedTypeId { get; set; }
        public bool IsCreatedByMe { get; set; }
        public JobViewModelForSuperAdmin Job { get; set; }
        public ProductSequenceViewModel ProductSequencing { get; set; } = new ProductSequenceViewModel();
    }
}
