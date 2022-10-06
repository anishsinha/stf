namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EIAPriceUpdate
    {
        public int Id { get; set; }

        [Required]
        [DefaultValue(1)]
        public FuelSurchageArea FuelSurchageArea { get; set; }

        [Required]
        public SurchargeProductTypes ProductType { get; set; }

        [Required]
        public FuelSurchagePricingType PricingType { get; set; }

        [StringLength(256)]
        public string SeriesName { get; set; }

        [StringLength(128)]
        public string SeriesId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTimeOffset PriceAddedDate { get; set; }
        public bool IsActive { get; set; }

    }
}
