using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Dispatcher
{
    public class UspBuyerLoadsForDashboard
    {   
        public string Dispatcher { get; set; } 
        public decimal Quantity { get; set; }
        public int QuantityTypeId { get; set; }
        public UoM UoM { get; set; }
        public string PoNumber { get; set; }
        public string Date { get; set; }
        public string ProductName { get; set; }
        public string Location { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
    }
}
