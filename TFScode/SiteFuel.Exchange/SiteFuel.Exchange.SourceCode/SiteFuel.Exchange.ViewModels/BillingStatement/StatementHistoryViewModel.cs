using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class StatementHistoryViewModel
    {
        public int Id { get; set; }
        public string StatementNumber { get; set; }
        public string BillingScheduleId { get; set; }
        public string BillingPeriod { get; set; }
        public string StmtDueDate { get; set; }
        public decimal TotalQuantityDropped { get; set; }
        public decimal TotalStmtValue { get; set; }
        public string Customer { get; set; }
        public int InvoiceCount { get; set; }
        public string Version { get; set; }
    }
}
