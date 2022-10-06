using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
	public class PurchaseOrderViewModel : WorkflowRequest
	{
        public PurchaseOrderViewModel()
        {
            VendorAddress = new AddressViewModel();
            ShipAddress = new AddressViewModel();
            Items = new List<OrderItemViewModel>();
            DiscountItems = new List<OrderItemViewModel>();
        }
        public bool IsPOAlreadyExist { get; set; }

        public string QbPurchaseOrderTxnID { get; set; }

        public string QbSalesOrderTxnID { get; set; }

        public int OrderId { get; set; }

        public int? ParentOrderId { get; set; }

        public bool IsBrokeredOrder { get; set; }

        public int VendorId { get; set; }

        public string VendorName { get; set; }

        public string VendorCompanyName { get; set; }

        public int CustomerId { get; set; }

        public string CustomerCompanyName { get; set; }

        public AddressViewModel VendorAddress { get; set; }

        public AddressViewModel ShipAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTimeOffset TxnDate { get; set; }

        public DateTimeOffset ShipDate { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public string PONumber { get; set; }

        public string Memo { get; set; }

        public List<OrderItemViewModel> Items { get; set; }

        public List<OrderItemViewModel> DiscountItems { get; set; }

        public int? InvoiceNumberId { get; set; }

        public string InvoiceNumber { get; set; }

        public int? OriginalInvoiceNumberId { get; set; }

        public string OriginalInvoiceNumber { get; set; }

        public string OriginalInvoiceQbNumber { get; set; }

        public string ClassName { get; set; }

        public string QbBillTxnID { get; set; }

        public bool IsRebillInvoice { get; set; }
    }
}
