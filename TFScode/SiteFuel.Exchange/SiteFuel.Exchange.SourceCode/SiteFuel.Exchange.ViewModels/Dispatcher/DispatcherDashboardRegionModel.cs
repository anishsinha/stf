using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Dispatcher
{
    public class DispatcherDashboardRegionModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DropdownDisplayExtendedItem> States { get; set; }
        public List<DropdownDisplayItem> Dispatchers { get; set; }
    }
}
