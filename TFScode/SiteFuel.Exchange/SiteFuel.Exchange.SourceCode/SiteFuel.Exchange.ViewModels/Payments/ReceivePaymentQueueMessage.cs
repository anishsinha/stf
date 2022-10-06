using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Payments
{
	public class ReceivePaymentQueueMessage
	{
		public long QbRequestId { get; set; }

		public int WorkflowId { get; set; }

        public int CompanyId { get; set; }
    }
}
