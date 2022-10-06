using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class QuickEntryDRModels
    {
        public string RegionId { get; set; }
        public int CompanyId { get; set; }
        public List<DropdownDisplayItem> Jobs { get; set; }

    }
}
