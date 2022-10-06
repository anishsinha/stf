using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class PDIAPIRequestViewModel
    {
        public int InvoiceHeaderId { get; set; }
        public string InvoiceNumber { get; set; }
        public int OrderId { get; set; }
    }

}
