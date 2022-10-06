using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class CreditInvoiceQueueServiceInputViewModel
    {
        public int InvoiceId { get; set; }
        public int UserId { get; set; }
        public List<int> TrackableScheduleIds { get; set; }
    }
}
