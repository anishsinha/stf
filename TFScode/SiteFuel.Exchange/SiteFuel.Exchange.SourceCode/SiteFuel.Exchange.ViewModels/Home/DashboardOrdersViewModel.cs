using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardOrdersViewModel : StatusViewModel
    {
        public DashboardOrdersViewModel()
        {
            InstanceInitialize();
        }

        public DashboardOrdersViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            StatusCode = status;
            Last5ActiveOrders = new List<UspGetBuyerActiveOrders>();
        }

        public int TotalOrderCount { get; set; }
        public int OpenOrderCount { get; set; }
        public int CanceledOrderCount { get; set; }
        public int ClosedOrderCount { get; set; }
        public int SelectedJobId { get; set; }

        public string GroupIds { get; set; }

        public bool IsOrderTileCollapsed { get; set; }

        public List<UspGetBuyerActiveOrders> Last5ActiveOrders { get; set; }
    }
}
