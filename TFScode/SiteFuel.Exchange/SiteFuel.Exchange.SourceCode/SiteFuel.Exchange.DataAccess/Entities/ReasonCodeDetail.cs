using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    [Table("ReasonCodeDetails")]
    public partial class ReasonCodeDetail
    {
        [Key]
        public int Id { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(128)]
        public string ReasonCode { get; set; }

        [Required]
        [StringLength(512)]
        public string Description { get; set; }

        public int CompanyId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTimeOffset CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ReasonCategory Category { get; set; }
    }
}
