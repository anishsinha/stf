using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ConsolidatedInvoiceEditViewModel : StatusViewModel
    {
        public InvoiceHeaderViewModel InvoiceHeader { get; set; }
        public List<InvoiceModel> invoiceModels { get; set; } = new List<InvoiceModel>();
    }
}
