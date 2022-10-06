using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.SharedEnums;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Interfaces
{
	public interface IResponseAdapter
	{
        QbXmlType Type { get; set; }

		TemplateResponse ResolveResponse(string xml);
	}
}
