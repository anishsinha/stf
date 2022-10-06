namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class MstOfferType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstOfferType()
        {
            OfferPricings = new HashSet<OfferPricing>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferPricing> OfferPricings { get; set; }
    }
}
