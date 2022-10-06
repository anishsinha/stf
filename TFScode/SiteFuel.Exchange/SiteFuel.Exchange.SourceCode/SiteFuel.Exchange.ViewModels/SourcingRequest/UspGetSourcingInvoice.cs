using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetSourcingInvoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string SourcingRequest { get; set; }
        public string PoNumber { get; set; }
        public DateTimeOffset DropDate { get; set; }
        public string Status { get; set; }
    }
}
