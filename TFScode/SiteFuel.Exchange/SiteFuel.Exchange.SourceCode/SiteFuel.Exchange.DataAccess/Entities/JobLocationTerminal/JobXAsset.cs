namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class JobXAsset
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobXAsset()
        {
            AssetDrops = new HashSet<AssetDrop>();
        }

        public int Id { get; set; }

        public int JobId { get; set; }

        public int AssetId { get; set; }

        public int AssignedBy { get; set; }

        public DateTimeOffset AssignedDate { get; set; }

        public Nullable<int> RemovedBy { get; set; }

        public Nullable<System.DateTimeOffset> RemovedDate { get; set; }

        public int? OrderId { get; set; }

        public int? FuelRequestId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetDrop> AssetDrops { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual Job Job { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("FuelRequestId")]
        public virtual FuelRequest FuelRequest { get; set; }
    }
}
