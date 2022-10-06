namespace SiteFuel.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MstProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstProduct()
        {
            MstProductMappings = new HashSet<MstProductMapping>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        public int ProductDisplayGroupId { get; set; }

        public int PricingSourceId { get; set; }

        public int? MappedParentId { get; set; }

        [StringLength(32)]
        public string ProductCode { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
        
        public int? TfxProductId { get; set; }

        [StringLength(256)]
        public string DisplayName { get; set; }

        //for products added as additives at company level
        public int? CompanyId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstProductMapping> MstProductMappings { get; set; }

        public virtual MstProductType MstProductType { get; set; }
        
        [ForeignKey("TfxProductId")]
        public virtual MstTfxProduct MstTfxProduct { get; set; }

        [ForeignKey("PricingSourceId")]
        public virtual MstPricingSource MstPricingSource { get; set; }
    }
}
