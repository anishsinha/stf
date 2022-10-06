using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ConvertBrokeredInvoiceForDipDataQueueMessage
    {
        public int AcceptedBy { get; set; }
        public NoDataExceptionPrePostViewModel NoDipDataExceptionModel { get; set; }
    }
}
