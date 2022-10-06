namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System.ComponentModel.DataAnnotations;

    public partial class EIAAreaMapping
    {
        public int Id { get; set; }

        [Required]
        public FuelSurchageArea FuelSurchageArea { get; set; }

        [Required]
        public SurchargeProductTypes ProductType { get; set; }

        [Required]
        public FuelSurchagePricingType PricingType { get; set; }

        [StringLength(128)]
        public string SeriesId { get; set; }

        public bool IsActive { get; set; }
    }
}
