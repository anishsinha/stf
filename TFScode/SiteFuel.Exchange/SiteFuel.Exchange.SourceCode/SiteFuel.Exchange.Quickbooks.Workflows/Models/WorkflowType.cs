using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
	public enum WorkflowType
	{
		Unknown = 0,
		PurchaseOrder,
		SaleOrder,
		InvoiceAdd,
        InvoicePoAdd,
        InvoiceModify,
        InvoicePoMod,
        PaymentTerms,
        BillAdd,
        BillModify,
        ReceivePayment,
        CreditMemoAdd,
        VendorCreditAdd
    }
}
