using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetSoucingRequest
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; }
        public string JobName { get; set; }
        public string FuelType { get; set; }
        public decimal Quantity { get; set; }
        public DateTimeOffset? DeliveryDate { get; set; }
        public string DeliveryType { get; set; }
        public int UOM { get; set; }
        public bool ViewedModified { get; set; }
        public int Status { get; set; }
        public int ModifiedBy { get; set; }
        public decimal? PricePerGallon { get; set; } = 0;
        public int PricingTypeId { get; set; }
        public Nullable<int> RackAvgTypeId { get; set; }
        public decimal? SupplierCostMarkupValue { get; set; } = 0;
        public Nullable<int> SupplierCostMarkupTypeId { get; set; }
        public decimal? RackPrice { get; set; } = 0;
    }

}
