namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class InvoiceTierPricingDetail
    {
        public int Id { get; set; }
        public int InvoiceFtlDetailId { get; set; }
        public decimal Quantity { get; set; } // sum of all records for each invoice = invoice dropped gallons
        public decimal PricePerGallon { get; set; }
        public decimal TierMinQuantity { get; set; }
        public decimal TierMaxQuantity { get; set; }

        [ForeignKey("InvoiceFtlDetailId")]
        public virtual InvoiceFtlDetail InvoiceFtlDetails { get; set; }
    }
}
