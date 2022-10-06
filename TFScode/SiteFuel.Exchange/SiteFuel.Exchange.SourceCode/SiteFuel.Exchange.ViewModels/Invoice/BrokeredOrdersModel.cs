using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class BrokeredOrdersModel
    {
        public int BuyerCompanyId { get; set; }

        public int SupplierCompanyId { get; set; }

        public int SequenceFromEndSupplier { get; set; }

        public int OrderId { get; set; }
    }
}
