namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MstTfxProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstTfxProduct()
        {
            MstProducts = new HashSet<MstProduct>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [StringLength(32)]
        public string ProductCode { get; set; }

        public int ProductDisplayGroupId { get; set; }

        public int ProductTypeId { get; set; }

        [StringLength(256)]
        public string ProductDescription { get; set; }

        [ForeignKey("ProductTypeId")]
        public virtual MstProductType MstProductType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstProduct> MstProducts { get; set; }
    }
}
