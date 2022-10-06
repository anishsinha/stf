using SiteFuel.Exchange.Quickbooks.SharedEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Quickbooks
{
	public class QbRequestViewModel
	{
		public long Id { get; set; }
        public int? EntityId { get; set; }
        public int WorkflowId { get; set; }
		public string QbXmlRq { get; set; }
		public QbRequestStatus Status { get; set; }
		public int QbXmlType { get; set; }
        public string EntityType { get; set; }
        public int? OrderId { get; set; }
        public int? InvoiceNumberId { get; set; }
        public string PoNumber { get; set; }
    }
}
