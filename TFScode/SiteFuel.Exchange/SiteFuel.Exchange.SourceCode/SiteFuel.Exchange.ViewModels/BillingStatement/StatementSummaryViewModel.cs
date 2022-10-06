using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class StatementSummaryViewModel
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public string StatementDate { get; set; }
        public string BillingPeriod { get; set; }
        public string PaymentTerms { get; set; }
        public string DueDate { get; set; }
        public string CustomerCompany { get; set; }
        public decimal TotalStatementValue { get; set; }
        public string TotalQuantityDropped { get; set; }
        public string StatementNumber { get; set; }
        public string BillingStatementId { get; set; }
        public int InvoiceCount { get; set; }
        public int TotalCount { get; set; }
    }
}
