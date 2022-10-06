using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardJobsViewModel : StatusViewModel
    {
        public DashboardJobsViewModel()
        {
            
        }

        public DashboardJobsViewModel(Status status)
            : base(status)
        {
           
        }

        public int TotalJobsCount { get; set; }

        public int UnderBudgetJobsCount { get; set; }

        public int NoBudgetJobsCount { get; set; }

        public int OverBudgetJobsCount { get; set; }

        public decimal TotalBudget { get; set; }

        public decimal TotalHedgeDroppedAmount { get; set; }

        public decimal TotalSpotDroppedAmount { get; set; }

        public int BudgetAlertPercentage { get; set; }

        public int SelectedJobId { get; set; }

        public int AssignedAssetsCount { get; set; }

        public string GroupIds { get; set; }
    }
}
