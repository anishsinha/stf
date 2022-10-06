using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters
{
    public class TermsQueryRequestAdapter : BaseRequestAdapter<TermsQueryViewModel>
    {
        public override AdapterResponse Convert(TermsQueryViewModel inputViewModel)
        {
            var termsQueryRq = new TermsQueryRq
            {
                ActiveStatus = "ActiveOnly"
            };
            return new AdapterResponse
            {
                QbXmlObject = termsQueryRq,
                QbXmlType = QbXmlType.TermsQuery,
                Status = QbXmlStatus.ReadyToQueue,
                EntityId = inputViewModel.CompanyId,
                EntityType = QbEntityType.PaymentTerms.ToString()
            };
        }
    }
}
