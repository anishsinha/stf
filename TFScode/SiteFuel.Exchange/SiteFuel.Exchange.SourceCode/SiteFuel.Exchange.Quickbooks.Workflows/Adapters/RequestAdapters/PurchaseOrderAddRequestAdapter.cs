using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Extensions;
using SiteFuel.Exchange.Quickbooks.Workflows.Mappers;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters
{
    public class PurchaseOrderAddRequestAdapter : BaseRequestAdapter<PurchaseOrderViewModel>
    {
        public override AdapterResponse Convert(PurchaseOrderViewModel inputViewModel)
        {
            var purchaseOrder = new PurchaseOrderAdd
            {
                VendorRef = new BasicInfo { FullName = inputViewModel.VendorName },
                ShipToEntityRef = string.IsNullOrWhiteSpace(inputViewModel.CustomerCompanyName) ? null :
                                    new BasicInfo { FullName = inputViewModel.CustomerCompanyName },
                TxnDate = inputViewModel.TxnDate.ToDateString(),
                VendorAddress = inputViewModel.VendorAddress.ToAddress(),
                ShipAddress = inputViewModel.ShipAddress.ToAddress(),
                DueDate = inputViewModel.DueDate.ToDateString(),
                ClassRef = !string.IsNullOrWhiteSpace(inputViewModel.ClassName) ? new BasicInfo { FullName = inputViewModel.ClassName } : null,
                //need to confirm about length of PO number from Brandon
                RefNumber = inputViewModel.PONumber.CropToLastChars(11),
                Memo = inputViewModel.Memo,
                PurchaseOrderLineAdd = inputViewModel.Items.Union(inputViewModel.DiscountItems)
                .Select(t => new OrderItem
                {
                    ItemRef = new BasicInfo { FullName = string.IsNullOrWhiteSpace(t.Prefix) ? t.Name : $"{t.Prefix} {t.Name}" },
                    Desc = t.Desc,
                    Quantity = t.Quantity > 0 ? t.Quantity.ToString() : null,
                    Rate = t.Rate
                }).ToArray()
            };

            PurchaseOrderAddRq purchaseOrderAddRq = new PurchaseOrderAddRq
            {
                PurchaseOrderAdd = purchaseOrder
            };
            return new AdapterResponse
            {
                QbXmlObject = purchaseOrderAddRq,
                QbXmlType = QbXmlType.PurchaseOrderAdd,
                Status = QbXmlStatus.ReadyToQueue,
                EntityId = inputViewModel.OrderId,
                EntityType = QbEntityType.PurchaseOrder.ToString()
            };
        }
    }
}
