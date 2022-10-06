using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
    public class QbBillViewModel : WorkflowRequest
    {
        public QbBillViewModel()
        {
            VendorAddress = new AddressViewModel();
            Items = new List<OrderItemViewModel>();
        }

        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorCompanyName { get; set; }
        public AddressViewModel VendorAddress { get; set; }
        public DateTimeOffset TxnDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public string PaymentTermName { get; set; }
        public int PaymentTermDays { get; set; }
        public int PaymentTermDiscountDays { get; set; }
        public decimal PaymentTermDiscountPct { get; set; }
        public string Memo { get; set; }
        public int OrderId { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
        public List<OrderItemViewModel> DiscountItems { get; set; }
        public decimal Amount { get; set; }
        public string ReferenceNum { get; set; }
        public string PoNumber { get; set; }
        public int InvoiceNumberId { get; set; }
        public string QbPurchaseOrderTxnID { get; set; }
        public string QbBillTxnID { get; set; }
        public string InvoiceNumber { get; set; }
        public string ParentInvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string ClassName { get; set; }
        public bool IsRebillInvoice { get; set; }
        public int? OriginalInvoiceNumberId { get; set; }
        public string OriginalInvoiceNumber { get; set; }
    }
}
