using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class DipatchersRegionDetails
    {
        public string RegionID { get; set; }
        public string RegionName { get; set; }
        public string RegionDescription { get; set; }
        public List<int> dispactherIds { get; set; }
    }
}
