using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DealResponseViewModel : StatusViewModel
    {
        public int JobId { get; set; }
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public int InvoiceTypeId { get; set; }
        public string TimeZoneName { get; set; }
        public int BuyerCompanyId { get; set; }
        public int InvoiceNumberId { get; set; }
    }
}
