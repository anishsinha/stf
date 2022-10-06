using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardDriverDropsHistoryViewModel : StatusViewModel
    {
        public DashboardDriverDropsHistoryViewModel()
        {
            InstanceInitialize();
        }

        public DashboardDriverDropsHistoryViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            RecentDrops = new List<UspCompletedDeliveriesViewModel>();
        }

        public int TotalDrops { get; set; }
        public int TotalOnTimeDrops { get; set; }
        public int TotalLateDrops { get; set; }
        public int TotalMissedDrops { get; set; }
        public int TotalDropsWithOverage { get; set; }

        public int TotalDiscontinuedDrops { get; set; }
        public List<UspCompletedDeliveriesViewModel> RecentDrops { get; set; }

    }
}
