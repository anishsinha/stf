using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProgressReportViewModel
    {
        public ProgressReportViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            ProgressReportCount = new ProgressReportCountViewModel();
            NewBuyers = new List<ProgressReportNewAccountsViewModel>();
            NewSuppliers = new List<ProgressReportNewAccountsViewModel>();
            NewBuyerAndSuppliers = new List<ProgressReportNewAccountsViewModel>();
        }

        public ProgressReportCountViewModel ProgressReportCount { get; set; }

        public List<ProgressReportNewAccountsViewModel> NewBuyers { get; set; }

        public List<ProgressReportNewAccountsViewModel> NewSuppliers { get; set; }

        public List<ProgressReportNewAccountsViewModel> NewBuyerAndSuppliers { get; set; }

        public string Date { get; set; }

        public string DailyReportLogo { get; set; }

        public bool HideActiveAccountsAndUsers { get; set; }

        public string Culture { get; set; }
    }
}
