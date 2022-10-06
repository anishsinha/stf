using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardDriverOrdersViewModel : StatusViewModel
    {
        public DashboardDriverOrdersViewModel()
        {
           
        }

        public DashboardDriverOrdersViewModel(Status status)
            : base(status)
        {
           
        }
        public int TotalOrderCount { get; set; }

        public int AssignedOrderCount { get; set; }

        public int ClosedOrderCount { get; set; }

        public int TodaysScheduledOrderCount { get; set; }
    }
}
