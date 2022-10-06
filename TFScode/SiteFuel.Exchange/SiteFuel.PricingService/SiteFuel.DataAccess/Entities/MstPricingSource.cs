namespace SiteFuel.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MstPricingSource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstPricingSource()
        {
        }

        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }
}