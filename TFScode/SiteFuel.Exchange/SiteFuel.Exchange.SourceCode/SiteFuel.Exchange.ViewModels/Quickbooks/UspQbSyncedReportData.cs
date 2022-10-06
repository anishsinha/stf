using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Quickbooks
{
    public class UspQbSyncedReportData
    {
        public int Status { get; set; }
        public int QbXmlType { get; set; }
        public int EntityId { get; set; }
        public string EntityType { get; set; }
        public int WorkflowStatus { get; set; }
        public int WorkflowType { get; set; }
        public string PoNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string QbInvoiceNumber { get; set; }
    }
}
