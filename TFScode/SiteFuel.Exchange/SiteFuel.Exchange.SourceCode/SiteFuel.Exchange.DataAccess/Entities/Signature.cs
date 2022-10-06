namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Signature
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Signature()
        {
        }

        public int Id { get; set; }

        [StringLength(512)]
        public string Signatory { get; set; }

        public bool SignatoryAvailable { get; set; }

        public int? ImageId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }
    }
}
