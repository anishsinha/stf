using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
	public class PaymentViewModel : WorkflowRequest
	{
        public DateTimeOffset FromModifiedDate { get; set; }

        public DateTimeOffset ToModifiedDate { get; set; }
    }
}
