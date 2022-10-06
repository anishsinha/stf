namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelSurchargeGeneratedTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FuelSurchargeGeneratedTable()
        {
            
        }
        [Key]
        public int Id { get; set; }
        public int FuelSurchargeIndexId { get; set; }
        public decimal PriceRangeStartValue { get; set; }
        public decimal PriceRangeEndValue { get; set; }
        public decimal FuelSurchargePercentage { get; set; }

        [ForeignKey("FuelSurchargeIndexId")]
        public virtual FuelSurchargeIndex FuelSurchargeIndex { get; set; }
    }
}