namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MstOtherFeeType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstOtherFeeType()
        {
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public int CompanyId { get; set; }

        public int? UserId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }
}
