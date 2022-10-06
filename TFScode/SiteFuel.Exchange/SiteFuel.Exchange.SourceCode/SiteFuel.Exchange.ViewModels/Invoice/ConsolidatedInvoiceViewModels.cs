using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ConsolidatedInvoiceViewModels
    {
        public List<InvoiceModel> Invoices { get; set; }
        public List<DropAdditionalDetailsModel> OtherDetails { get; set; }
    }
}
