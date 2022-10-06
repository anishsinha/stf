namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelGroupFuelType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FuelGroupFuelType()
        {
        }

        [Key]
        public int Id { get; set; }

        public int FuelGroupId { get; set; }

        public int TfxProductId { get; set; }

        [ForeignKey("TfxProductId")]
        public virtual MstTfxProduct MstTfxProduct { get; set; }

        [ForeignKey("FuelGroupId")]
        public virtual FuelGroup FuelGroup { get; set; }
    }
}
