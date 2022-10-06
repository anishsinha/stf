using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DemandInputModel
    {
        public int CompanyId { get; set; }
        public int? JobId { get; set; }
        public string RegionId { get; set; }
        public string BuyerJobs { get; set; }
        public int SourceTypeId { get; set; } = 1;
        public bool IsCreateDR { get; set; }
    }
}
