using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Dispatcher
{
    public class FreightDeliveryRequestDetail
    {
        public string Id { get; set; }
        public int LdPri { get; set; } // Load priority
        public string RgId { get; set; } // RegionId
        public string RgName { get; set; } // Region name
        public List<DropdownDisplayExtendedItem> States { get; set; } // States
    }
}
