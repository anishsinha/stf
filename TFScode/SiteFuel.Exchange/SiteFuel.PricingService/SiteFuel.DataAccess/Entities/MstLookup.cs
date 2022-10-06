using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.DataAccess.Entities
{
    public partial class MstLookup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstLookup()
        {
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Key { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstLookupType> MstLookupTypes { get; set; }
    }
}
