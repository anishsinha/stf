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
    public class PurchaseOrderModifyRequestAdapter : BaseRequestAdapter<PurchaseOrderViewModel>
    {
        private readonly string itemFormat = $"{{{{:{TemplateParameter.PurchaseOrderTxnLineID}:}}}}";

        public override AdapterResponse Convert(PurchaseOrderViewModel inputViewModel)
        {
            int itemIndex = 0;
            var purchaseOrder = new PurchaseOrderMod
            {
                VendorRef = new BasicInfo { FullName = inputViewModel.VendorName },
                ShipToEntityRef = string.IsNullOrWhiteSpace(inputViewModel.CustomerCompanyName) ? null :
                                    new BasicInfo { FullName = inputViewModel.CustomerCompanyName },
                TxnDate = inputViewModel.TxnDate.ToDateString(),
                VendorAddress = inputViewModel.VendorAddress.ToAddress(),
                ShipAddress = inputViewModel.ShipAddress.ToAddress(),
                DueDate = inputViewModel.DueDate.ToDateString(),
                ClassRef = !string.IsNullOrWhiteSpace(inputViewModel.ClassName) ? new BasicInfo { FullName = inputViewModel.ClassName } : null,
                RefNumber = inputViewModel.PONumber.CropToLastChars(11),
                Memo = inputViewModel.Memo,
                PurchaseOrderLineMod = inputViewModel.Items.Union(inputViewModel.DiscountItems)
                .Select(t => new OrderItem
                {
                    TxnLineID = t.IsNewlyAdded ? "-1" : string.Format(itemFormat, itemIndex++),
                    ItemRef = new BasicInfo { FullName = string.IsNullOrWhiteSpace(t.Prefix) ? t.Name : $"{t.Prefix} {t.Name}" },
                    Desc = t.Desc,
                    Quantity = t.Quantity > 0 ? t.Quantity.ToString() : null,
                    Rate = t.Rate
                }).ToArray()
            };

            PurchaseOrderModRq purchaseOrderModRq = new PurchaseOrderModRq
            {
                PurchaseOrderMod = purchaseOrder
            };
            return new AdapterResponse
            {
                QbXmlObject = purchaseOrderModRq,
                QbXmlType = QbXmlType.PurchaseOrderMod,
                Status = QbXmlStatus.NotReadyToQueue,
                EntityId = inputViewModel.OrderId,
                EntityType = QbEntityType.PurchaseOrder.ToString()
            };
        }
    }
}
