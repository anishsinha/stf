using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.WebNotification
{
    public class NotificationInvoiceQueMsg
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string OrderNumber { get; set; }
        public string CreatedByUserName { get; set; }
        public int CreatedByCompanyId { get; set; }
        public string CreatedByCompanyName { get; set; }
        public string CreatedForUserName { get; set; }
        public string CreatedForCompanyName { get; set; }

    }
}
