using SiteFuel.Exchange.ViewModels.AccountingEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Quickbooks
{
    public class QbReportViewModel
    {
        public int AccountingWorkflowId { get; set; }

        public string EntityType { get; set; }

        public string CreatedDate { get; set; }

        public string WorkFlowType { get; set; }

        public string SyncDate { get; set; }

        public string PoNumber { get; set; }

        public int OrderId { get; set; }

        public string InvoiceNumber { get; set; }

        public int InvoiceId { get; set; }

        public string Status { get; set; }

        public int TotalCount { get; set; }
    }
}
