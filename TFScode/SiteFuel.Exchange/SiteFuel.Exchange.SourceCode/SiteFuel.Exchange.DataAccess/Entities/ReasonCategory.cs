using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    [Table("ReasonCategories")]
    public partial class ReasonCategory
    {
        public ReasonCategory()
        {
            ReasonCodeDetails = new HashSet<ReasonCodeDetail>();
        }

        [Key]
        public int Id { get; set; }
        
        [StringLength(512)]
        public string Name { get; set; }

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

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReasonCodeDetail> ReasonCodeDetails { get; set; }
    }
}
