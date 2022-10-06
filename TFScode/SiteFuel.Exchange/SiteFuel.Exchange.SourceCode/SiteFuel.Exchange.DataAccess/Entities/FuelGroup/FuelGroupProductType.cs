namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelGroupProductType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FuelGroupProductType()
        {
        }

        [Key]
        public int Id { get; set; }

        public int FuelGroupId { get; set; }

        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public virtual MstProductType MstProductType { get; set; }

        [ForeignKey("FuelGroupId")]
        public virtual FuelGroup FuelGroup { get; set; }
    }
}
