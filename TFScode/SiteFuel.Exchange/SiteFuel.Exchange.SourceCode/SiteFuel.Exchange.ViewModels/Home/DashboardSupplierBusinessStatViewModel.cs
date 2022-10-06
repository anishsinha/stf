using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardSupplierBusinessStatViewModel : StatusViewModel
    {
        public DashboardSupplierBusinessStatViewModel()
        {
          
        }

        public DashboardSupplierBusinessStatViewModel(Status status)
            : base(status)
        {
          
        }

        public decimal BusinessYouWon { get; set; }
        public decimal BusinessYouMissed { get; set; }
        public decimal BusinessInYourArea { get; set; }

        public bool IsBusinessStatTileCollapsed { get; set; }
    }
}
