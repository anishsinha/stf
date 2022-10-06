namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DifferentFuelPrice
    {
        public DifferentFuelPrice()
        {
            Currency = Currency.USD;
            UoM = UoM.Gallons;
        }
        public int Id { get; set; }

        public decimal MinQuantity { get; set; }

        public Nullable<decimal> MaxQuantity { get; set; }

        public int PricingTypeId { get; set; }

        public Nullable<int> RackAvgTypeId { get; set; }

        public decimal PricePerGallon { get; set; }

        public Nullable<int> MarginTypeId { get; set; }

        public decimal Margin { get; set; }

        public Nullable<decimal> SupplierCost { get; set; }

        public Nullable<int> SupplierCostTypeId { get; set; }

        public int? FuelRequestId { get; set; }

        public int? ResaleId { get; set; }

        public int? OfferPricingId { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public virtual MstMarginType MstMarginType { get; set; }

        public virtual MstPricingType MstPricingType { get; set; }

        public virtual MstRackAvgPricingType MstRackAvgPricingType { get; set; }

        [ForeignKey("FuelRequestId")]
        public virtual FuelRequest FuelRequest { get; set; }

        [ForeignKey("ResaleId")]
        public virtual Resale Resale { get; set; }

        [ForeignKey("OfferPricingId")]
        public virtual OfferPricing OfferPricing { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ForeignKey("SupplierCostTypeId")]
        public virtual MstSupplierCostTypes MstSupplierCostTypes { get; set; }
    }
}
