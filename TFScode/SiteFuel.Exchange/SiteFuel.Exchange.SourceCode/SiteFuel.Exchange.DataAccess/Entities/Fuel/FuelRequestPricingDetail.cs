namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelRequestPricingDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FuelRequestId { get; set; }      

        public int RequestPriceDetailId { get; set; }

        public int PricingCodeId { get; set; }

        public string PricingCode { get; set; }

        public string DisplayPriceCode { get; set; }

        public string DisplayPrice { get; set; }

        //[ForeignKey("FuelRequestId")]
        public virtual FuelRequest FuelRequest { get; set; }
    }
}