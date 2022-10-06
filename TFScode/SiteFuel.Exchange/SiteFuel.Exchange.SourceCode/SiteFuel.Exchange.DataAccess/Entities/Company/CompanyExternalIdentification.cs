using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class CompanyExternalIdentification
    {
        public int Id { get; set; }

        public int IdentifyingCompanyId { get; set; }

        public int IdentifiedCompanyId { get; set; }

        [StringLength(256)]
        public string ExternalId { get; set; }

        public int AddedBy { get; set; }

        public DateTimeOffset? AddedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("IdentifiedCompanyId")]
        public virtual Company IdentifiedCompany { get; set; }
    }
}
