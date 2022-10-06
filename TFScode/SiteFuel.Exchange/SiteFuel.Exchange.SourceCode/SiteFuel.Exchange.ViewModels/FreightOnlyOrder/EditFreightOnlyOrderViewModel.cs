using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class EditFreightOnlyOrderViewModel
    {
        public List<int> removedJobsIds { get; set; }
        public List<int>newJobsIds { get; set; }
        public bool IsCreateOrder { get; set; }
        public int CarrierCompanyId { get; set; }
    }
}
