using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Forcasting;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobStepsViewModel : BaseCultureViewModel
    {
        public JobStepsViewModel()
        {
            InstanceInitialize();
        }

        public JobStepsViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            IsJobCreationFromFuelRequest = false;
            IsJobCreationFromQuoteRequest = false;
            Job = new JobViewModel(status);
            ContactPersons = new List<JobContactViewModel>();
            Subcontractors = new List<SubcontractorViewModel>();
            JobBudget = new JobBudgetViewModel(status);
        }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public int CompanyTypeId { get; set; }

        public bool IsJobCreationFromFuelRequest { get; set; }
        public bool IsJobCreationFromAPI { get; set; }
        public int SupplierCompanyId { get; set; }
        public bool IsJobCreationFromQuoteRequest { get; set; }

        public JobViewModel Job { get; set; }

        public List<JobContactViewModel> ContactPersons { get; set; }

        public List<SubcontractorViewModel> Subcontractors { get; set; }

        public JobBudgetViewModel JobBudget { get; set; }

        public bool IsAuditApplicable { get; set; }

        public string RegionId { get; set; }
        public string RouteId { get; set; }
   
    }
}
