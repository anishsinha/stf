using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    [Table("EBOLProductMappings")]
    public partial class EBOLProductMappings
    {
        [Key]
        public int Id { get; set; }

        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual MstProduct MstProducts { get; set; }

        [StringLength(100)]
        public string EBOLProductCode { get; set; }

        [StringLength(500)]
        public string EBOLProductDescription { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
