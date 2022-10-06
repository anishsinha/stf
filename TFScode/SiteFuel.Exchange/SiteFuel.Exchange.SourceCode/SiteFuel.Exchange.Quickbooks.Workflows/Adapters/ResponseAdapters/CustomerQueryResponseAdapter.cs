using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.Workflows.Interfaces;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.SharedEnums;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.ResponseAdapters
{
	public class CustomerQueryResponseAdapter : IResponseAdapter
	{
		public QbXmlType Type { get; set; } = QbXmlType.CustomerQuery;

		public TemplateResponse ResolveResponse(string xml)
		{
			var template = new TemplateResponse();
			template.Templates.Add(TemplateParameter.CustomerId, "67673");
			return template;
		}
	}
}
