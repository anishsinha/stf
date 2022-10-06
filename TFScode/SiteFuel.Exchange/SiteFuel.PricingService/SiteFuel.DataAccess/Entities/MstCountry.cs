namespace SiteFuel.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstCountry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstCountry()
        {
            MstStates = new HashSet<MstState>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(8)]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int Currency { get; set; }

        [Required]
        [StringLength(8)]
        public string CurrencySymbol { get; set; }

        public int DefaultUoM { get; set; }
        
        [StringLength(100)]
        public string UoD { get; set; }
        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstState> MstStates { get; set; }
    }
}
