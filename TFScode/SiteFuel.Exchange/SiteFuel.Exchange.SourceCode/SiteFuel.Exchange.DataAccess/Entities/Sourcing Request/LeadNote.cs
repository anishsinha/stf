using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class LeadNote
    {
        [Key]
        public int Id { get; set; }
        public int LeadRequestId { get; set; }
        public string GeneralNote { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        [ForeignKey("LeadRequestId")]
        public virtual LeadRequest LeadRequest { get; set; }
    }
}
