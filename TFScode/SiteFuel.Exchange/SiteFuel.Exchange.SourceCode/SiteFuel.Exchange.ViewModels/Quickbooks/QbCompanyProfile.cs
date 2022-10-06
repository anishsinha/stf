using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Quickbooks
{
	public class QbCompanyProfile
	{
		public int CompanyId { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string LoginToken { get; set; }
		public DateTimeOffset LastAccessedOn { get; set; }
		public DateTimeOffset ProfileUpdatedOn { get; set; }
		public DateTimeOffset ProfileCreatedOn { get; set; }
		public int CreatedBy { get; set; }
		public string QwcXml { get; set; }
		public string QbVersion { get; set; }
		public string ExpenseAccountName { get; set; }
		public string IncomeAccountName { get; set; }
        public string DiscountAccountName { get; set; }
        public string ClassRef { get; set; }
		public string ItemPrefix { get; set; }
		public bool IsActive { get; set; }
		public DateTimeOffset SyncStartDate { get; set; }

		public List<PaymentTerms> PaymentTerms { get; set; } = new List<Quickbooks.PaymentTerms>();
	}
}
