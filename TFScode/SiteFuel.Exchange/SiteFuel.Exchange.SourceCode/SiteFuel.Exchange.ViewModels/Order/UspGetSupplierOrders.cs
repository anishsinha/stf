using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetSupplierOrders
    {
        public int Id { get; set; }

        public int UOM { get; set; }

        public string PoNumber { get; set; }

        public string Eligibility { get; set; }

        public string Supplier { get; set; }

        public string JobName { get; set; }

        public int PricingTypeId { get; set; }

        public string PricePerGallon { get; set; }

        public decimal Quantity { get; set; }

        public string StartDate { get; set; }

        public int InvoiceCount { get; set; }

        public int DDTCount { get; set; }

        public decimal FuelDeliveredPercentage { get; set; }

        public string Status { get; set; }

        public int BrokerFuelRequestId { get; set; }

        public int ParentId { get; set; }

        public decimal? TotalAmount { get; set; }

        public int TotalCount { get; set; }

        public string CustomerPoNumber { get; set; }

        public string FuelType { get; set; }

        public int CustomerOrderId { get; set; }

        public string Location { get; set; }

        public bool IsEndSupplier { get; set; }

        public string DeliveryType { get; set; }

        public string OrderType { get; set; }

        public int? OrderGroupId { get; set; }
        public string GroupPoNumber { get; set; }
        public string OrderName { get; set; }
        public string VesselName { get; set; }
    }

    public class USP_GetSupplierActiveOrders
    {
        public int Id { get; set; }

        public string PoNumber { get; set; }

        public string Customer { get; set; }

        public string StartDate { get; set; }

        public int InvoiceCount { get; set; }

        public decimal FuelDeliveredPercentage { get; set; }

        public int TotalOrders { get; set; }

        public int OpenOrders { get; set; }

        public int ClosedOrders { get; set; }

        public int TotalDrops { get; set; }

        public int QuantityTypeId { get; set; }

        public int FiftyPlusPercentDelivered { get; set; }

        
    }
}
