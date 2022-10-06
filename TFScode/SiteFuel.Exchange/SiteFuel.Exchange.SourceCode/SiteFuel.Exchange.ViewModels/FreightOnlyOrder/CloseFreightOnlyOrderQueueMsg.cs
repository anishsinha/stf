using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class CloseFreightOnlyOrderQueueMsg
    {
        public List<int> OrderIds { get; set; }
        public int CarrierCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }        
        public int SupplierUserId { get; set; }
    }
}
