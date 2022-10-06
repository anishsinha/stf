using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.NewsfeedRequest
{
    public class EddtToInvoiceCreatedNewsfeedModel
    {
        public int JobId { get; set; }
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public string TimeZoneName { get; set; }
        public int WaitingFor { get; set; }
        public bool IsDigitalDropTicket { get; set; }
        public int InvoiceHeaderId { get; set; }
    }
}
