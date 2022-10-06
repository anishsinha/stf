using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Quickbooks
{
	public class PaymentTerms
	{
		public string TermName { get; set; }

		public int TermDays { get; set; }

		public bool IsActive { get; set; }

		public DateTimeOffset CreatedDate { get; set; }

		public int Id { get; set; }
	}
}
