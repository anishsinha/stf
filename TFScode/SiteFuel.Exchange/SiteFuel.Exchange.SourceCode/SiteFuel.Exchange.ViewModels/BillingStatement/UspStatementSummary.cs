using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspStatementSummary
    {
        public int Id { get; set; }
        public DateTimeOffset StatementDate { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public string CustomerCompany { get; set; }
        public decimal TotalStatementValue { get; set; }
        public decimal TotalQuantityDropped { get; set; }
        public string StatementNumber { get; set; }
        public string BillingStatementId { get; set; }
        public int InvoiceCount { get; set; }
        public int ScheduleId { get; set; }
        public int TotalCount { get; set; }
    }
}
