using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Core.Resolver
{
    public static class TemplateParameter
    {
        public const string CustomerId = "CustomerId";
        public const string CustomerEditSequence = "CustomerEditSequence";

        public const string InvoiceEditSequence = "InvoiceEditSequence";
        public const string InvoiceRefNumber = "InvoiceRefNumber";
        public const string InvoiceTxnID = "InvoiceTxnID";
        public const string InvoiceTxnLineID = "InvoiceTxnLineID{0}";
        public const string InvoiceLinkToTxnID = "InvoiceLinkToTxnID";
        public const string InvoiceNumber = "InvoiceNumber";
        public const string OriginalInvoiceQbNumber = "OriginalInvoiceQbNumber";

        public const string ItemId = "ItemId";
        public const string ItemEditSequence = "ItemEditSequence";

        public const string PurchaseOrderTxnID = "PurchaseOrderTxnID";
        public const string PurchaseOrderEditSequence = "PurchaseOrderEditSequence";
        public const string PurchaseOrderRefNumber = "PurchaseOrderRefNumber";
        public const string PurchaseOrderTxnLineID = "PurchaseOrderTxnLineID{0}";

        public const string SalesOrderEditSequence = "SalesOrderEditSequence";
        public const string SalesOrderRefNumber = "SalesOrderRefNumber";
        public const string SalesOrderTxnID = "SalesOrderTxnID";
        public const string SalesOrderTxnLineID = "SalesOrderTxnLineID{0}";

        public const string VendorId = "VendorId";
        public const string VendorEditSequence = "VendorEditSequence";

        public const string BillTxnID = "BillTxnID";
        public const string BillLinkToTxnID = "BillLinkToTxnID";
        public const string BillEditSequence = "BillEditSequence";
        public const string BillRefNumber = "BillRefNumber";
        public const string BillTxnLineID = "BillTxnLineID{0}";
        public const string Memo = "Memo";
        public const string OriginalBillQbNumber = "OriginalBillQbNumber";

        public const string CreditMemoTxnID = "CreditMemoTxnID";
        public const string CreditMemoEditSequence = "CreditMemoEditSequence";
        public const string CreditMemoRefNumber = "CreditMemoRefNumber";
        public const string CreditMemoTxnLineID = "CreditMemoTxnLineID{0}";

        public const string VendorCreditTxnID = "VendorCreditTxnID";
        public const string VendorCreditEditSequence = "VendorCreditEditSequence";
        public const string VendorCreditRefNumber = "VendorCreditRefNumber";
        public const string VendorCreditTxnLineID = "VendorCreditTxnLineID{0}";

        public const string ReceivePaymentTxnID = "ReceivePaymentTxnID{0}";
        public const string ReceivePaymentRefNumber = "ReceivePaymentRefNumber{0}";
        public const string ReceivePaymentEditSequence = "ReceivePaymentEditSequence{0}";
        public const string ReceivePaymentDate = "ReceivePaymentDate{0}";
        public const string ReceivePaymentTotalAmount = "ReceivePaymentTotalAmount{0}";
        public const string ReceivePaymentPaymentMethod = "ReceivePaymentPaymentMethod{0}";
        public const string ReceivePaymentQbInvoiceId = "ReceivePaymentQbInvoiceId{0}";
        public const string ReceivePaymentBalanceRemaining = "ReceivePaymentBalanceRemaining{0}";
        public const string ReceivePaymentAmountPaid = "ReceivePaymentAmountPaid{0}";
        public const string ReceivePaymentLinkedTransID = "ReceivePaymentLinkedTransID{0}";
        public const string ReceivePaymentLinkedTransRefNumber = "ReceivePaymentLinkedTransRefNumber{0}";
        public const string ReceivePaymentLinkedTransAmount = "ReceivePaymentLinkedTransAmount{0}";
        public const string ReceivePaymentTotalTransCount = "ReceivePaymentTotalTransCount";
        public const string ReceivePaymentTotalAppliedTransCount = "ReceivePaymentTotalAppliedTransCount{0}";
    }
}
