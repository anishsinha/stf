using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
    public class SalesOrderQueryViewModel : WorkflowRequest
    {
        public int OrderId { get; set; }

        public string QbSalesOrderTxnID { get; set; }
    }
}
