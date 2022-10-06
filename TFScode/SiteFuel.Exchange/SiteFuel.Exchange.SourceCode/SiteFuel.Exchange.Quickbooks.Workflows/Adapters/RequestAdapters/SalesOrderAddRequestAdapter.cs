using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters;
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
    public class SalesOrderAddRequestAdapter : BaseRequestAdapter<SalesOrderViewModel>
    {
        public override AdapterResponse Convert(SalesOrderViewModel inputViewModel)
        {
            var salesOrder = new SalesOrderAdd
            {
                CustomerRef = new BasicInfo { FullName = inputViewModel.CustomerName },
                TxnDate = inputViewModel.TxnDate.ToDateString(),
                BillAddress = inputViewModel.BillAddress.ToAddress(),
                ShipAddress = inputViewModel.ShipAddress.ToAddress(),
                PONumber = inputViewModel.PONumber.CropToLastChars(25),
                DueDate = inputViewModel.DueDate.ToDateString(),
                ShipDate = inputViewModel.ShipDate.ToDateString(),
                Memo = inputViewModel.Memo,
                SalesOrderLineAdd = inputViewModel.Items.Union(inputViewModel.DiscountItems)
                .Select(t => new OrderItem
                {
                    ItemRef = new BasicInfo { FullName = t.Name },
                    Desc = t.Desc,
                    Quantity = t.Quantity > 0 ? t.Quantity.ToString() : null,
                    Rate = t.Rate
                }).ToArray()
            };

            SalesOrderAddRq salesOrderAddRq = new SalesOrderAddRq
            {
                SalesOrderAdd = salesOrder
            };
            return new AdapterResponse
            {
                QbXmlObject = salesOrderAddRq,
                QbXmlType = QbXmlType.SalesOrderAdd,
                Status = QbXmlStatus.ReadyToQueue,
                EntityId = inputViewModel.OrderId,
                EntityType = QbEntityType.SalesOrder.ToString()
            };
        }
    }
}
