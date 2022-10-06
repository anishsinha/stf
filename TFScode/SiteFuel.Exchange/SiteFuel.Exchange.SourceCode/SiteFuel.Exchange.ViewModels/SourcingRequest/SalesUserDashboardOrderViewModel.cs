using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SalesUserDashboardOrderViewModel
    {
        public int Id { get; set; }
        public string PoNumber { get; set; }
        public string JobName { get; set; }
        public string Customer { get; set; }
        public string FuelType { get; set; }
        public string Quantity { get; set; }
        public int StatusId { get; set; }

    }
}
