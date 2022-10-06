namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Spill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Spill()
        {
            Images = new HashSet<Image>();
        }

        public int Id { get; set; }

        public DateTimeOffset SpillDate { get; set; }

        public int SpilledBy { get; set; }

        [Required]
        public string Notes { get; set; }

        public int AssetId { get; set; }

        public int OrderId { get; set; }

        public Nullable<int> InvoiceId { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual Order Order { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }
    }
}
