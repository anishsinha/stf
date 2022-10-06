namespace SiteFuel.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class MstState
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstState()
        {
            MstExternalTerminals = new HashSet<MstExternalTerminal>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int CountryId { get; set; }

        public int QuantityIndicatorTypeId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public virtual MstCountry MstCountry { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstExternalTerminal> MstExternalTerminals { get; set; }
    }
}
