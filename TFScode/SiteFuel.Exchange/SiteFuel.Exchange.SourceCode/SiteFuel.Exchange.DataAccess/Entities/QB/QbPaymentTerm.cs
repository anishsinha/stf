using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class QbPaymentTerm
    {
        public int Id { get; set; }

        public int QbProfileId { get; set; }

        [StringLength(128)]
        public string TermName { get; set; }

        public int TermDays { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("QbProfileId")]
        public virtual QbCompanyProfile QbCompanyProfile { get; set; }
    }
}
