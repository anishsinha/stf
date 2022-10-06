using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetGroupOrders
    {
        public int OrderGroupId { get; set; }

        public int OrderId { get; set; }

        public string PoNumber { get; set; }

        public string SupplierCompany { get; set; }

        public string BuyerCompany { get; set; }

        public string PricePerGallon { get; set; }

        public decimal Quantity { get; set; }

        public decimal FuelDeliveredPercentage { get; set; }

        public string Status { get; set; }

        public string FuelType { get; set; }

        public int TotalCount { get; set; }

        public string DeliveryType { get; set; }

        public string OrderType { get; set; }

        public string CustomerPoNumber { get; set; }

        public OrderGroupType GroupType { get; set; }

        public ProductCategory ProductType { get; set; }

        public string JobName { get; set; }

        public string JobAddress { get; set; }

        public OrderGroupFrequency RenewalFrequency { get; set; }

        public decimal BlendPercentage { get; set; }

        public decimal MinVolume { get; set; }

        public decimal MaxVolume { get; set; }

        public string TfxPoNumber { get; set; }

        public UoM UoM { get; set; }

        public QuantityType QuantityType { get; set; }

        public int CreatedBy { get; set; }
    }
}
