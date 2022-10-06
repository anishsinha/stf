using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetBuyerOrders
    {
        public int Id { get; set; }

        public int UOM { get; set; }

        public string PoNumber { get; set; }

        public string Eligibility { get; set; }

        public string Supplier { get; set; }

        public string PricePerGallon { get; set; }

        public decimal Quantity { get; set; }

        public decimal FuelDeliveredPercentage { get; set; }

        public string Status { get; set; }

        public string FuelType { get; set; }

        public string Type { get; set; }

        public int AssetsAssigned { get; set; }

        public decimal? TotalAmount { get; set; }

        public int TotalCount { get; set; }

        public string DeliveryType { get; set; }

        public string OrderType { get; set; }
        public int? OrderGroupId { get; set; }
        public string GroupPoNumber { get; set; }
        public string VesselName { get; set; }
    }

    public class UspGetBuyerActiveOrders
    {
        public int Id { get; set; }

        public string PoNumber { get; set; }

        public string Supplier { get; set; }

        public string StartDate { get; set; }

        public int InvoiceCount { get; set; }

        public decimal FuelDeliveredPercentage { get; set; }

        public int QuantityTypeId { get; set; }

        public int TotalOrders { get; set; }

        public int OpenOrders { get; set; }

        public int ClosedOrders { get; set; }

        public int CancelledOrders { get; set; }

    }
}
