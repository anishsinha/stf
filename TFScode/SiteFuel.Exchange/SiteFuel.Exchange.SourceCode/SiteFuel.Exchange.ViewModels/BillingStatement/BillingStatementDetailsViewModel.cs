using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class BillingStatementDetailsViewModel: BaseCultureViewModel
    {
        public BillingStatementDetailsViewModel()
        {

        }
        public BillingStatementDetailsViewModel(Status status)
            : base(status)
        {
            Invoices = new List<StatementInvoiceDetails>();
        }
        public int Id { get; set; }
        public string BillingScheduleId { get; set; }
        public string BillingPeriod { get; set; }
        public string StatementNumber { get; set; }
        public string StmtDueDate { get; set; }
        public string StatementDate { get; set; }
        public decimal TotalQuantityDropped { get; set; }
        public decimal TotalStmtValue { get; set; }
        public Currency Currency { get; set; }
        public UoM Uom { get; set; }
        public string Customer { get; set; }
        public decimal ExchangeRate { get; set; }
        public List<StatementInvoiceDetails> Invoices { get; set; }
    }
}
