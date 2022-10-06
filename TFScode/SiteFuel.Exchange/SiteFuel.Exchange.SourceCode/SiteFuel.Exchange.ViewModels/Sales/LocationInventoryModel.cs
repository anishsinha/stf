using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class LocationInventoryModel
    {
        public string SiteId { get; set; }
        public string Location { get; set; }
        public string TankName { get; set; }
        public string AvgSale { get; set; }
        public string Inventory { get; set; }
        public string DaysRemaining { get; set; }
        public DeliveryReqPriority Priority { get; set; }      
    }
}
