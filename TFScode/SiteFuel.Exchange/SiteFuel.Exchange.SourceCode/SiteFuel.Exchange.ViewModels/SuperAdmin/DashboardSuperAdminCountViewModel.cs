using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardSuperAdminCountViewModel
    { 
        public int TotalSuperAdminCount { get; set; }

        public int TotalActiveSuperAdminCount { get; set; }

        public int TotalInactiveSuperAdminCount { get; set; }

        public int SelectedCompanyId { get; set; }
    }
}
