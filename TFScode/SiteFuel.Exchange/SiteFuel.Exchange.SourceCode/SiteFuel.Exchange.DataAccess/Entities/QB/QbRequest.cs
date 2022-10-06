using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
	public class QbRequest
	{
        public QbRequest()
        {
            RetryCount = 0;
        }

		public long Id { get; set; }

		public int AccountingWorkflowId { get; set; }

        public int? EntityId { get; set; }

        public string QbXmlRq { get; set; }

		public int Status { get; set; }

		public int QbXmlType { get; set; }

		public DateTimeOffset ReadyForQueueOn { get; set; }

		public DateTimeOffset CreatedOn { get; set; }

		public DateTimeOffset UpdatedOn { get; set; }

        [StringLength(1024)]
        public string EntityType { get; set; }

        [ForeignKey("AccountingWorkflowId")]
		public virtual AccountingWorkflow AccountingWorkflow { get; set; }

        public int RetryCount { get; set; }

        [StringLength(256)]
        public string PoNumber { get; set; }

        public long? ParentId { get; set; }

        public int? InvoiceNumberId { get; set; }
    }
}

