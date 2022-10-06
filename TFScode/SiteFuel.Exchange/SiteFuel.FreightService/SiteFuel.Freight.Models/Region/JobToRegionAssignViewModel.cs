using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class JobToRegionAssignViewModel
    {
        public string RegionId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int UpdatedBy { get; set; }
        public int CompanyId { get; set; }
        public string RouteId { get; set; }
    }
    
}
