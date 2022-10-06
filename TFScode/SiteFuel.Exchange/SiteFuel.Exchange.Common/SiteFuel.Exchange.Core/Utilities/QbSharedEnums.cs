using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.SharedEnums
{
    public enum QbAccountType
    {
        AccountsPayable,
        AccountsReceivable,
        Bank,
        CostOfGoodsSold,
        CreditCard,
        Equity,
        Expense,
        FixedAsset,
        Income,
        LongTermLiability,
        NonPosting,
        OtherAsset,
        OtherCurrentAsset,
        OtherCurrentLiability,
        OtherExpense,
        OtherIncome
    }

    public enum ActiveStatus
    {
        ActiveOnly,
        InactiveOnly,
        All
    }

    public enum MatchCriterion
    {
        StartsWith,
        Contains,
        EndsWith
    }

    public enum QbXmlType
    {
        Unknown,
        CustomerQuery,
        CustomerAdd,
        CustomerMod,
        ItemQuery,
        ItemAdd,
        ItemMod,
        SalesOrderQuery,
        SalesOrderAdd,
        SalesOrderMod,
        InvoiceQuery,
        InvoiceAdd,
        InvoiceMod,
        VendorQuery,
        VendorAdd,
        VendorMod,
        PurchaseOrderQuery,
        PurchaseOrderAdd,
        PurchaseOrderMod,
        TermsQuery,
        StdTermsAdd,
        StdTermsMod,
        StdTermsQuery,
        DiscountAdd,
        DiscountMod,
        BillAdd,
        BillMod,
        BillQuery,
        ReceivePaymentQuery,
        CreditMemoAdd,
        VendorCreditAdd
    }

    public enum QbEntityType
    {
        Customer,
        SalesOrder,
        Invoice,
        Vendor,
        PurchaseOrder,
        PaymentTerms,
        Bill,
        ReceivePayment,
        CreditMemo,
        VendorCredit
    }

    public enum QbTxnType
    {
        ARRefundCreditCard,
        Bill,
        BillPaymentCheck,
        BillPaymentCreditCard,
        BuildAssembly,
        Charge,
        Check,
        CreditCardCharge,
        CreditCardCredit,
        CreditMemo,
        Deposit,
        Estimate,
        InventoryAdjustment,
        Invoice,
        ItemReceipt,
        JournalEntry,
        LiabilityAdjustment,
        Paycheck,
        PayrollLiabilityCheck,
        PurchaseOrder,
        ReceivePayment,
        SalesOrder,
        SalesReceipt,
        SalesTaxPaymentCheck,
        Transfer,
        VendorCredit,
        YTDAdjustment
    }

    public enum QbRequestStatus
    {
        Unknown = 0,
        NotReadToQueue,
        ReadyToQueue,
        Queued,
        Completed,
        Failed,
        MapperDependent,
        SyncSkip,
        OnFailureDuplicateAttempt,
        OnFailureRequeued,
        Retried,
        FailedAndStoppedForRetry,
        SFXInvoiceDeleted
    }

    public enum QbXmlStatus
    {
        Unknown = 0,
        NotReadyToQueue,
        ReadyToQueue,
        Queued,
        Completed,
        Failed,
        MapperDependent,
        SyncSkip,
        OnFailureDuplicateAttempt,
        OnFailureRequeued,
        Retried
    }

    public enum QResponseStatusCode
    {
        InternalError = 1000,
        AlreadyExist = 3100,
        // The internals could not be locked. They are in use by another user.
        InternalsCouldNotBeLocked = 3170,
        // The transaction could not be locked. It is in use by another user.
        TheTransactionCouldNotBeLocked = 3175,
        // There was an error when saving a Customers list. QuickBooks error message: This list has been modified by another user.
        ListHasBeenModifiedByOtherUser = 3180
    }
}
