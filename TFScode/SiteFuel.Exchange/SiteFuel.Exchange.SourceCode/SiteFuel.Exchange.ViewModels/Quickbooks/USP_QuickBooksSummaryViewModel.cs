using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Quickbooks
{
    public class USP_QuickBooksSummaryViewModel
    {
        public int WorkflowID { get; set; }
        public string WorkflowDetails { get; set; }
        public long RequestID { get; set; }
        public string WorkflowStatus { get; set; }
        public string RequestStatus { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int EntityID { get; set; }
        public string EntityType { get; set; }
        public long? ResponseID { get; set; }
        public string CompanyName { get; set; }
        public int TotalCount { get; set; }
        public DateTimeOffset ReqUpdatedOn { get; set; }
        public DateTimeOffset? ResUpdatedOn { get; set; }
        public int RetryCount { get; set; }
        public string QbXmlType { get; set; }
        public string WorkflowType { get; set; }
        public string PoNumber { get; set; }
    }
}
