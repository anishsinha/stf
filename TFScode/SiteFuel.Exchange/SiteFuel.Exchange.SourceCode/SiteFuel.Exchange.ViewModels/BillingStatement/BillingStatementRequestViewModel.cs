using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class BillingStatementRequestViewModel
    {
        public int? BillingScheduleId { get; set; }
        public DateTimeOffset StmtStartDate { get; set; }
        public DateTimeOffset StmtEndDate { get; set; }
        public DateTimeOffset StmtDueDate { get; set; }
        public int CreatedBy { get; set; }
        public int CreatedCompany { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public decimal TotalQuantityDropped { get; set; }
        public decimal TotalStmtValue { get; set; }
        public Currency Currency { get; set; }
        public UoM Uom { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsPaid { get; set; }
        public int VersionNumber { get; set; }
        public string StmtChainId { get; set; }
        public List<int> InvoiceIds { get; set; }
        public int? ParentId { get; set; }
        public bool IsGenerated { get; set; }
        public int PaymentTermId { get; set; }
        public int PaymentNetDays { get; set; }
        public string TimeZoneName { get; set; }
        public int FrequencyType { get; set; }
    }
}
