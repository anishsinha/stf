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
    public class SalesOrderModifyRequestAdapter : BaseRequestAdapter<SalesOrderViewModel>
    {
        private readonly string itemFormat = $"{{{{:{TemplateParameter.SalesOrderTxnLineID}:}}}}";

        public override AdapterResponse Convert(SalesOrderViewModel inputViewModel)
        {
            int itemIndex = 0;
            var salesOrder = new SalesOrderMod
            {
                CustomerRef = new BasicInfo { FullName = inputViewModel.CustomerName },
                TxnDate = inputViewModel.TxnDate.ToDateString(),
                BillAddress = inputViewModel.BillAddress.ToAddress(),
                ShipAddress = inputViewModel.ShipAddress.ToAddress(),
                PONumber = inputViewModel.PONumber.CropToLastChars(25),
                DueDate = inputViewModel.DueDate.ToDateString(),
                ShipDate = inputViewModel.ShipDate.ToDateString(),
                Memo = inputViewModel.Memo,
                SalesOrderLineMod = inputViewModel.Items.Union(inputViewModel.DiscountItems)
                .Select((t, i) => new OrderItem
                {
                    TxnLineID = t.IsNewlyAdded ? "-1" : string.Format(itemFormat, itemIndex++),
                    ItemRef = new BasicInfo { FullName = t.Name },
                    Desc = t.Desc,
                    Quantity = t.Quantity > 0 ? t.Quantity.ToString() : null,
                    Rate = t.Rate
                }).ToArray()
            };

            SalesOrderModRq salesOrderModRq = new SalesOrderModRq
            {
                SalesOrderMod = salesOrder
            };
            return new AdapterResponse
            {
                QbXmlObject = salesOrderModRq,
                QbXmlType = QbXmlType.SalesOrderMod,
                Status = QbXmlStatus.NotReadyToQueue,
                EntityId = inputViewModel.OrderId,
                EntityType = QbEntityType.SalesOrder.ToString()
            };
        }
    }
}
