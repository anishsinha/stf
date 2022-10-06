using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Extensions;
using SiteFuel.Exchange.Quickbooks.Workflows.Mappers;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters
{
    public class BillQueryRequestAdapter : BaseRequestAdapter<QbBillViewModel>
    {
        private readonly string itemFormat = $"{{:{TemplateParameter.BillTxnID}:}}";

        public override AdapterResponse Convert(QbBillViewModel inputViewModel)
        {
            var billQueryRq = new BillQueryRq
            {
                TxnID = string.IsNullOrEmpty(inputViewModel.QbBillTxnID) ? itemFormat : inputViewModel.QbBillTxnID
            };

            return new AdapterResponse
            {
                QbXmlObject = billQueryRq,
                QbXmlType = QbXmlType.BillQuery,
                Status = string.IsNullOrEmpty(inputViewModel.QbBillTxnID) ? QbXmlStatus.MapperDependent : QbXmlStatus.ReadyToQueue,
                EntityId = inputViewModel.OrderId,
                EntityType = QbEntityType.Bill.ToString()
            };
        }
    }
}
