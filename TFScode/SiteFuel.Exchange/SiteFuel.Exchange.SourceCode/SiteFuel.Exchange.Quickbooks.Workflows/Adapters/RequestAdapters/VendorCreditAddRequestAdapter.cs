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
    public class VendorCreditAddRequestAdapter : BaseRequestAdapter<PurchaseOrderViewModel>
    {
        public override AdapterResponse Convert(PurchaseOrderViewModel inputViewModel)
        {
            var itemlines = inputViewModel.Items
                .Select(t => new ItemLineAdd
                {
                    ItemRef = new BasicInfo { FullName = string.IsNullOrWhiteSpace(t.Prefix) ? t.Name : $"{t.Prefix} {t.Name}" },
                    Desc = t.Desc,
                    Quantity = t.Quantity.ToString(),
                    Cost = t.Rate
                }).ToArray();
            var discountLineItems = inputViewModel.DiscountItems
                .Select(t => new ItemLineAdd
                 {
                     ItemRef = new BasicInfo { FullName = string.IsNullOrWhiteSpace(t.Prefix) ? t.Name : $"{t.Prefix} {t.Name}" },
                     Desc = t.Desc,
                     Quantity = t.Quantity > 0 ? t.Quantity.ToString() : null,
                    Cost = t.Rate
                 }).ToArray();

            var vendorCreditAdd = new VendorCreditAdd
            {
                VendorRef = new BasicInfo { FullName = inputViewModel.VendorName },
                TxnDate = inputViewModel.TxnDate.ToDateString(),
                Memo = inputViewModel.Memo?.CropToLastChars(50),
                RefNumber = inputViewModel.InvoiceNumber.Replace(ApplicationConstants.SFCI, "CM").CropToLastChars(20),
                ItemLineAdd = itemlines.Union(discountLineItems).ToArray()
            };
            
            var VendorCreditAddRq = new VendorCreditAddRq
            {
                VendorCreditAdd = vendorCreditAdd
            };

            return new AdapterResponse
            {
                QbXmlObject = VendorCreditAddRq,
                QbXmlType = QbXmlType.VendorCreditAdd,
                Status = QbXmlStatus.NotReadyToQueue,
                EntityId = inputViewModel.InvoiceNumberId.Value,
                EntityType = QbEntityType.VendorCredit.ToString()
            };
        }
    }
}
