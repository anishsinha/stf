using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardSuperAdminCompanyCountViewModel
    { 
        public int TotalCompanyCount { get; set; }

        public int TotalBuyerCompanyCount { get; set; }

        public int TotalSupplierCompanyCount { get; set; }

        public int TotalBuyerAndSupplierCompanyCount { get; set; }

        public int TotalDriverCompanyCount { get; set; }
    }
}
