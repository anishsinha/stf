using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class CompanyXCreators
    {
        public int Id { get; set; }

        public int CreatedByCompanyId { get; set; }

        public int CompanyId { get; set; }

       
        [StringLength(256)]
        public string ExternalRefId { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
        public bool IsActive { get; set; }

       [ForeignKey("CreatedByCompanyId")]
        public virtual Company CreatedByCompany { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company CreatedCompany { get; set; }
    }
}
