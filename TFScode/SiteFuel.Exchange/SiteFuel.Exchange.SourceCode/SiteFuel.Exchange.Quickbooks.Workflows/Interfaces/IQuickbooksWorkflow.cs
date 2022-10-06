using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Interfaces
{
	public interface IQuickbooksWorkflow<in T>
		where T : WorkflowRequest
	{
		WorkflowType Type { get; }
		List<string> SupportedVersions { get; set; }
		WorkflowResult ExecuteWorkflow(T viewModel);
	}
}
