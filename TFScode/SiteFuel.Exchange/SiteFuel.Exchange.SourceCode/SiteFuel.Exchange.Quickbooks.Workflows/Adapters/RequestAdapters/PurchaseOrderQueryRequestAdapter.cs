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
    public class PurchaseOrderQueryRequestAdapter : BaseRequestAdapter<PurchaseOrderViewModel>
    {
        private readonly string itemFormat = $"{{:{TemplateParameter.PurchaseOrderTxnID}:}}";

        public override AdapterResponse Convert(PurchaseOrderViewModel inputViewModel)
        {
            var purchaseOrderQuery = new PurchaseOrderQueryRq
            {
                TxnID = string.IsNullOrEmpty(inputViewModel.QbPurchaseOrderTxnID) ? itemFormat : inputViewModel.QbPurchaseOrderTxnID,
            };
            return new AdapterResponse
            {
                QbXmlObject = purchaseOrderQuery,
                QbXmlType = QbXmlType.PurchaseOrderQuery,
                Status = string.IsNullOrEmpty(inputViewModel.QbPurchaseOrderTxnID) ? QbXmlStatus.MapperDependent : QbXmlStatus.ReadyToQueue,
                EntityId = inputViewModel.OrderId,
                EntityType = QbEntityType.PurchaseOrder.ToString()
            };
        }
    }
}
