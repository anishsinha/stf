using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class OrderItem : QuickbooksXml
    {
        public string TxnLineID { get; set; }

        public BasicInfo ItemRef { get; set; }

        public string Desc { get; set; }

        public string Quantity { get; set; }

        public decimal Rate { get; set; }
    }
}
