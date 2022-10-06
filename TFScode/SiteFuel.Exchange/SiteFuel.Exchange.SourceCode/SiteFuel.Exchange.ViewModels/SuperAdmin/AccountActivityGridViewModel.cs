using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AccountActivityGridViewModel
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public string AccountPhoneNumber { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string CompanyLocation { get; set; }

        public string AccountType { get; set; }

        public string AccountActivityDateTime { get; set; }
    }
}
