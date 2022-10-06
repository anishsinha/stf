using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class LeadAdditionalDetail
    {
        [Key]
        public int Id { get; set; }
        public int LeadRequestId { get; set; }
        public bool IsAssetTracked { get; set; }
        public bool IsAssetDropStatusEnabled { get; set; }
        [ForeignKey("LeadRequestId")]
        public virtual LeadRequest LeadRequest { get; set; }
    }
}
