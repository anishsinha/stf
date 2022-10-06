using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderDropDetailsViewModel
    {
        public string Number { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal? PricePerGallon { get; set; }
        public string Amount { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string DriverName { get; set; }
        public int OrderId { get; set; }
        public Nullable<int> ParentId { get; set; }
    }

    public class CarrierOrderDetails
    {
        public int SupplierOrderId { get; set; }
        public int CarrierOrderId { get; set; }
        public int CarrierCompanyId { get; set; }

    }
}
