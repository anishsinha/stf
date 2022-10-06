using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class QbEntityMapping
    {
        public long Id { get; set; }

        public int QbProfileId { get; set; }

        [StringLength(1024)]
        public string EntityType { get; set; }

        public int EntityId { get; set; }

        [StringLength(1024)]
        public string QbReferenceId { get; set; }

        [StringLength(1024)]
        public string EditSequence { get; set; }

        public int? ParentId { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

        public int? InvoiceNumberId { get; set; }

        [ForeignKey("QbProfileId")]
        public virtual QbCompanyProfile QbProfile { get; set; }

    }
}
