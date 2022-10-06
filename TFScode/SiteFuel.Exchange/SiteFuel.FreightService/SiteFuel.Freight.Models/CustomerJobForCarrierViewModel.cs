using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class CustomerJobForCarrierViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string RegionId { get; set; }
        public DropdownDisplayExtendedItem Job { get; set; }
    }
}
