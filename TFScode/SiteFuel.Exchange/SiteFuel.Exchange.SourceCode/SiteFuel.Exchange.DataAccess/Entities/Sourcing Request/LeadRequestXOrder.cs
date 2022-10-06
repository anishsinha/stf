using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class LeadRequestXOrder
    {
        [Key]
        public int Id { get; set; }
        public int LeadRequestId { get; set; }
        public int OrderId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey("LeadRequestId")]
        public virtual LeadRequest LeadRequest { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order{ get; set; }
    }
}
