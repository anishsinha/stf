using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardCompanyUsersCountViewModel
    { 
        public int TotalCompanyUsersCount { get; set; }

        public int TotalActiveCompanyUsersCount { get; set; }

        public int TotalInactiveCompanyUsersCount { get; set; }
    }
}
