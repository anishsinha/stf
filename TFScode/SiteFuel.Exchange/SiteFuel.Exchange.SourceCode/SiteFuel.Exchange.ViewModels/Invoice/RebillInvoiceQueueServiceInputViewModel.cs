using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class RebillInvoiceQueueServiceInputViewModel : CreditInvoiceQueueServiceInputViewModel
    {
        public int CompanyId { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public int RebilledInvoiceId { get; set; }
    }
}
