using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Dispatcher
{
    public class BuyerLoadsForDashboardViewModel
    {
        public string PoNumber { get; set; }
        public string Location { get; set; }
        public string Product { get; set; }
        public string Quantity { get; set; }
        public string Dispatcher { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public string Date { get; set; }
    }
}
