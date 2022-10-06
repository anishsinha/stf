using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProgressReportNewAccountsViewModel
    {
        public string CompanyName { get; set; }

        public string AdminUser { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int CompanyTypeId { get; set; }

        public int AccountTypeId { get; set; }

        public string AccountType { get; set; }
    }
}
