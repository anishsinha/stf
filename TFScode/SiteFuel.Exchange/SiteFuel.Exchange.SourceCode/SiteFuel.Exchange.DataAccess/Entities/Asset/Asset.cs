namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Asset
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Asset()
        {
            AssetDropRequests = new HashSet<AssetDropRequest>();
            JobXAssets = new HashSet<JobXAsset>();
            Spills = new HashSet<Spill>();
            AssetSubcontractors = new HashSet<AssetSubcontractor>();
            AssetContractNumbers = new HashSet<AssetContractNumber>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public Nullable<int> FuelType { get; set; }  /// <summary>
        /// product type id
        /// </summary>

        public System.DateTimeOffset CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public int Type { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int? CompanyId { get; set; }

        public int? ImageId { get; set; }

        public bool IsMarine { get; set; }

        public virtual AssetAdditionalDetail AssetAdditionalDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetDropRequest> AssetDropRequests { get; set; }

        public virtual MstProductType MstProductType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobXAsset> JobXAssets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Spill> Spills { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetSubcontractor> AssetSubcontractors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetContractNumber> AssetContractNumbers { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public Nullable<int> FuelTypeId { get; set; }

        [ForeignKey("FuelTypeId")]
        public virtual MstTfxProduct MstTfxProduct  { get; set; }

    }
}
