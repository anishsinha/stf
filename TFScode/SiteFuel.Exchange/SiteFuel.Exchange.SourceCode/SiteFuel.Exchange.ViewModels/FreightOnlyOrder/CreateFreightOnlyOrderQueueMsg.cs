using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.FreightOnlyOrder
{
    public class CreateFreightOnlyOrderQueueMsg
    {
        public int OrderId { get; set; }
        public string PONumber { get; set; }
        public int FuelRequestId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int SupplierUserId { get; set; }
        public int CarrierCompanyId { get; set; }
    }
}
