using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
	public class QbResponse
    {
		public long Id { get; set; }

		public long QbRequestId { get; set; }

		public string QbXmlRs { get; set; }

		public DateTimeOffset CreatedOn { get; set; }

		public DateTimeOffset UpdatedOn { get; set; }

		[ForeignKey("QbRequestId")]
		public virtual QbRequest QbRequest { get; set; }

        public long? StatusCode { get; set; }
    }
}

