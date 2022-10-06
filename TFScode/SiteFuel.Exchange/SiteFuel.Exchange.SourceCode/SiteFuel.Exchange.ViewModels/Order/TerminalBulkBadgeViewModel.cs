using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.IO;

namespace SiteFuel.Exchange.ViewModels
{
    public class TerminalBulkBadgeViewModel
    {
        public bool IsPickupTerminal { get; set; } = true;
        public int? TerminalId { get; set; }
        public int? BulkPlantId { get; set; }
        public string TerminalBulkPlantName { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public int OrderId { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
    }

    public class GetOrderBadgeDetailsInput
    {
        public List<int> OrderIds { get; set; }
        public int PickupLocationType { get; set; }
        public int PickupLocationId { get; set; }
    }
}
