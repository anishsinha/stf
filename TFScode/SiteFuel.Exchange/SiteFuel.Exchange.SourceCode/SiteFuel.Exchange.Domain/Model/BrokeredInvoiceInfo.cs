using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class BrokeredInvoiceInfo
    {
        public int InvoiceId { get; set; }

        public int InvoiceNumberId { get; set; }

        public int FuelRequestId { get; set; }
    }
}
