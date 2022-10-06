using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DigitalDropTicketDraftNewsfeedModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public int SupplierCompanyId { get; set; }
        public int DriverId { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int InvoiceHeaderId { get; set; }
    }
}
