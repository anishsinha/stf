using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.AccountingEvent
{
	public enum AccountingWorkflowType
	{
		Unknown = 0,
		PurchaseOrder,
		SaleOrder,
		InvoiceAdd,
        InvoicePoAdd,
        InvoiceModify,
        POModify,
        PaymentTerms,
        BillAdd,
        BillModify,
        ReceivePayment,
        CreditMemoAdd,
        VendorCreditAdd
    }

	public enum AccountingWorkflowStatus
	{
		Unknown = 0,
		Created,
		Initializing,
		Initialized,
		Started,
		Completed,
		QbRequestFailed,
        SFXInvoiceDeleted,
        Skip
	}
}
