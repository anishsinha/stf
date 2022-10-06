using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class BadgePickupDetailModel
    {
        public int OrderId { get; set; }
        public int? TerminalId { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public int? BulkPlantId { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
    }
}
