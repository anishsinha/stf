using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardSupplierFuelRequestGridViewModel : StatusViewModel
    {
        public DashboardSupplierFuelRequestGridViewModel()
        {
            InstanceInitialize();
        }

        public DashboardSupplierFuelRequestGridViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            RecentFuelRequests = new List<FuelRequestGridViewModel>();
        }        

        public bool IsFRTileCollapsed { get; set; }

        public List<FuelRequestGridViewModel> RecentFuelRequests { get; set; }

    }
}
