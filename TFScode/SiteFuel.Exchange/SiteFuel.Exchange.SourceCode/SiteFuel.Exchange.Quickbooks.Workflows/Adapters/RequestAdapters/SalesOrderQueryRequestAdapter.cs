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
    public class SalesOrderQueryRequestAdapter : BaseRequestAdapter<SalesOrderQueryViewModel>
    {
        private readonly string itemFormat = $"{{:{TemplateParameter.SalesOrderTxnID}:}}";

        public override AdapterResponse Convert(SalesOrderQueryViewModel inputViewModel)
        {
            var salesOrderQuery = new SalesOrderQueryRq
            {
                TxnID = string.IsNullOrEmpty(inputViewModel.QbSalesOrderTxnID) ? itemFormat : inputViewModel.QbSalesOrderTxnID,
            };
            return new AdapterResponse
            {
                QbXmlObject = salesOrderQuery,
                QbXmlType = QbXmlType.SalesOrderQuery,
                Status = string.IsNullOrEmpty(inputViewModel.QbSalesOrderTxnID) ? QbXmlStatus.MapperDependent : QbXmlStatus.ReadyToQueue,
                EntityId = inputViewModel.OrderId,
                EntityType = QbEntityType.SalesOrder.ToString()
            };
        }
    }
}
