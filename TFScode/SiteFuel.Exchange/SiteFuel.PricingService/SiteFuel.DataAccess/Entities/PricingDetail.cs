using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.DataAccess.Entities
{
    public class PricingDetail
    {
        public int Id { get; set; }

        public int RequestPriceDetailId { get; set; }

        public int PricingCodeId { get; set; }

        public Nullable<int> RackAvgTypeId { get; set; }

        public decimal PricePerGallon { get; set; }

        public decimal? SupplierCost { get; set; }

        public int? SupplierCostTypeId { get; set; }

        public Nullable<int> MarginTypeId { get; set; }

        public decimal Margin { get; set; }

        public decimal BasePrice { get; set; }

        public decimal? BaseSupplierCost { get; set; }

        public decimal? MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public int? CityRackTerminalId { get; set; }
        public int? TerminalId { get; set; }
        public bool IsActive { get; set; }
        public int? FuelTypeId { get; set; }
        public string ParameterJson { get; set; }

        [ForeignKey("PricingCodeId")]
        public virtual MstPricingCode MstPricingCode { get; set; }

        [ForeignKey("RequestPriceDetailId")]
        public virtual RequestPriceDetail RequestPriceDetails { get; set; }
    }
}
