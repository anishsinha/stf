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
    public class CreditMemoAddRequestAdapter : BaseRequestAdapter<QbInvoiceViewModel>
    {
        public override AdapterResponse Convert(QbInvoiceViewModel inputViewModel)
        {
            var itemlines = inputViewModel.Items
                .Select(t => new CreditMemoLineAdd
                {
                    ItemRef = new BasicInfo { FullName = t.Name },
                    Desc = t.Desc,
                    Quantity = t.Quantity.ToString(),
                    Rate = t.Rate
                }).ToArray();
            var discountLineItems = inputViewModel.DiscountItems
                .Select(t => new CreditMemoLineAdd
                {
                    ItemRef = new BasicInfo { FullName = t.Name },
                    Desc = t.Desc,
                    Quantity = t.Quantity > 0 ? t.Quantity.ToString() : null,
                    Rate = t.Rate
                }).ToArray();
            var creditMemoAdd = new CreditMemoAdd
            {
                CustomerRef = new BasicInfo { FullName = inputViewModel.CustomerName },
                TxnDate = inputViewModel.TxnDate.ToDateString(),
                BillAddress = inputViewModel.BillAddress.ToAddress(),
                ShipAddress = inputViewModel.ShipAddress.ToAddress(),
                PONumber = inputViewModel.PONumber.CropToLastChars(25),
                DueDate = inputViewModel.DueDate.ToDateString(),
                ShipDate = inputViewModel.ShipDate.ToDateString(),
                Memo = inputViewModel.Memo?.CropToLastChars(50),
                RefNumber = inputViewModel.InvoiceNumber.CropToLastChars(20),
                CreditMemoLineAdd = itemlines.Union(discountLineItems).ToArray()
            };
            var CreditMemoAddRq = new CreditMemoAddRq
            {
                CreditMemoAdd = creditMemoAdd
            };

            return new AdapterResponse
            {
                QbXmlObject = CreditMemoAddRq,
                QbXmlType = QbXmlType.CreditMemoAdd,
                Status = QbXmlStatus.NotReadyToQueue,
                EntityId = inputViewModel.InvoiceNumberId,
                EntityType = QbEntityType.CreditMemo.ToString()
            };
        }
    }
}
