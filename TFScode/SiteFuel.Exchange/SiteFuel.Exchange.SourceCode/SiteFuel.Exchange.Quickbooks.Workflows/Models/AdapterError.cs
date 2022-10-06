using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
	public class AdapterError
	{
		public AdapterType ErroneousAdapter { get; set; }
		public Exception ExceptionDetails { get; set; }
		public int AdapterSequenceNumber { get; set; }
		public string ErrorMessage { get; set; }
	}
}
