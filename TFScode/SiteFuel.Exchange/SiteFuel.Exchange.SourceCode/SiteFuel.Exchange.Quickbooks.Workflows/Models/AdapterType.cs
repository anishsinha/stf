using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
	public enum AdapterType
	{
		Unknown = 0,
		BuyerToCustomer,
		SupplierToVendor,
		OrderToPurchaseOrder,
		OrderToSalesOrder,
        InvoiceDdtToInvoice
	}
}
