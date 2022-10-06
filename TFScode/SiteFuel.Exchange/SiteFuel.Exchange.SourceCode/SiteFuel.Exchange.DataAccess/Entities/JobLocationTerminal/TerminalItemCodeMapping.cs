namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class TerminalItemCodeMapping
    {
        public int Id { get; set; }

        [Required]
        public int TerminalSupplierId { get; set; }

        [Required]
        public int ItemDescriptionId { get; set; }

        [Required]
        [StringLength(128)]
        public string ItemCode { get; set; }

        [Required]
        public DateTimeOffset EffectiveDate { get; set; }

        public DateTimeOffset? ExpiryDate { get; set; }

        public int CompanyId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int AddedBy { get; set; }

        [Required]
        public DateTimeOffset AddedDate { get; set; }

        [Required]
        public int UpdatedBy { get; set; }

        [Required]
        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("TerminalSupplierId")]
        public virtual MstTerminalSupplier MstTerminalSupplier { get; set; }

        [ForeignKey("ItemDescriptionId")]
        public virtual MstTerminalItemDescription MstTerminalItemDescription { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }
}
