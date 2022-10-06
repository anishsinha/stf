using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.Dispatcher
{
    public class DispatcherDashboardRegionModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DropdownDisplayItem> States { get; set; }

        public List<DropdownDisplayItem> Dispatchers { get; set; }
    }
}
