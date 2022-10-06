using System;
using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class CurrentCostRequestModel
    {
        public decimal Cost { get; set; }
        public int SupplierCostType { get; set; }
        public List<int> RequestPriceDetailIds { get; set; }
    }

    public class CurrentCostResponseModel : BaseResponseModel
    {
        public List<CostResponseModel> Cost { get; set; } = new List<CostResponseModel>();
    }

    public class CostResponseModel
    {
        public int PriceDetailId { get; set; }
        public decimal previousCost { get; set; }
        public int previousCostType { get; set; }
    }
}
