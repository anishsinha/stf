using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardSupplierOrdersViewModel : StatusViewModel
    {
        public DashboardSupplierOrdersViewModel()
        {
            InstanceInitialize();
        }

        public DashboardSupplierOrdersViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Last5ActiveOrders = new List<USP_GetSupplierActiveOrders>();
        }

        public int TotalOrderCount { get; set; }

        public int OpenOrderCount { get; set; }

        public int ClosedOrderCount { get; set; }

        public int TotalDrops { get; set; }

        public int FiftyPlusPercentageDeliveredOrderCount { get; set; }

        public int DeliveryRequestCount { get; set; }

        public string GroupIds { get; set; }

        public List<USP_GetSupplierActiveOrders> Last5ActiveOrders { get; set; }

        public bool IsOrderTileCollapsed { get; set; }
    }
}
