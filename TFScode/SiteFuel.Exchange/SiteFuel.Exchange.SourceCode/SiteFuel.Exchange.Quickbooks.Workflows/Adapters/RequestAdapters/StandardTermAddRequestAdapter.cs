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
    public class StandardTermAddRequestAdapter : BaseRequestAdapter<QbInvoiceViewModel>
    {
        public override AdapterResponse Convert(QbInvoiceViewModel inputViewModel)
        {
            var standardTermsAdd = new StandardTermsAdd
            {
                Name = inputViewModel.PaymentTermName,
                StdDueDays = inputViewModel.PaymentTermDays,
                StdDiscountDays = inputViewModel.PaymentTermDiscountDays,
                DiscountPct = inputViewModel.PaymentTermDiscountPct
            };
            var standardTermsAddRq = new StandardTermsAddRq
            {
                StandardTermsAdd = standardTermsAdd
            };
            return new AdapterResponse
            {
                QbXmlObject = standardTermsAddRq,
                QbXmlType = QbXmlType.StdTermsAdd,
                Status = QbXmlStatus.ReadyToQueue,
                EntityType = QbEntityType.PaymentTerms.ToString()
            };
        }
    }
}
