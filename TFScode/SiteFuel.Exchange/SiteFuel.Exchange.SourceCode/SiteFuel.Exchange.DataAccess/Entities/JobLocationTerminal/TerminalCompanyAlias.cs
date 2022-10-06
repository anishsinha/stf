using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
   public partial class TerminalCompanyAlias
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? TerminalId { get; set; }

        public string AssignedTerminalId { get; set; }

        public int CreatedByCompanyId { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public bool IsBulkPlant { get; set; }

        public int? BulkPlantId { get; set; }

        public string AssignedTerminalSupplierId { get; set; }

        public int? TerminalSupplierId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("TerminalId")]
        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        [ForeignKey("CreatedByCompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("BulkPlantId")]
        public virtual BulkPlantLocation BulkPlantLocation { get; set; }

        [ForeignKey("TerminalSupplierId")]
        public virtual MstTerminalSupplier MstTerminalSupplier { get; set; }
    }
}
