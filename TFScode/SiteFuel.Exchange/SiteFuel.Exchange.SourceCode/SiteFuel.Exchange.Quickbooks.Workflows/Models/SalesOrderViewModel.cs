using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
    public class SalesOrderViewModel : WorkflowRequest
    {
        public SalesOrderViewModel()
        {
            BillAddress = new AddressViewModel();
            ShipAddress = new AddressViewModel();
            Items = new List<OrderItemViewModel>();
            DiscountItems = new List<OrderItemViewModel>();
        }
        public string QbSalesOrderTxnID { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerCompanyName { get; set; }

        public AddressViewModel BillAddress { get; set; }

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
    }
}
