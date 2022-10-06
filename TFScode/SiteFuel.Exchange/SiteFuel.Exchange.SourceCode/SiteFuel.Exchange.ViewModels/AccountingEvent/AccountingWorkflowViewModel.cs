using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.AccountingEvent
{
	public class AccountingWorkflowViewModel
	{
		public int Id { get; set; }
		public AccountingWorkflowType Type { get; set; }
		public AccountingWorkflowStatus Status { get; set; }
		public string ParameterJson { get; set; }
		public int QbCompanyProfileId { get; set; }
		public DateTimeOffset CreatedOn { get; set; }
		public DateTimeOffset UpdatedOn { get; set; }
		public QbCompanyProfile QbCompanyProfile { get; set; }
		public List<QbRequestViewModel> QbRequests { get; set; }
		public string SoftwareVersion { get; set; }
	}
}
