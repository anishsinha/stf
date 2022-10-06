using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
    public class QbInvoiceViewModel : SalesOrderViewModel
    {
        public string QbInvoiceTxnID { get; set; }
        public int InvoiceId { get; set; }
        public int InvoiceHeaderId { get; set; }
        public int InvoiceNumberId { get; set; }
        public string InvoiceNumber { get; set; }
        public bool IsPrimaryOrder { get; set; }
		public string PaymentTermName { get; set; }
        public int PaymentTermDays { get; set; }
        public int PaymentTermDiscountDays { get; set; }
        public decimal PaymentTermDiscountPct { get; set; }
        public int? OriginalInvoiceNumberId { get; set; }
        public bool IsRebillInvoice { get; set; }
    }
}
