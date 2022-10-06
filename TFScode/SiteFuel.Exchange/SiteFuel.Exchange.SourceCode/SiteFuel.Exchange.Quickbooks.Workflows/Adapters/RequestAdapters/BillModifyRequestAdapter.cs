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
    public class BillModifyRequestAdapter : BaseRequestAdapter<QbBillViewModel>
    {
        private readonly string itemFormat = $"{{{{:{TemplateParameter.BillTxnLineID}:}}}}";
        public override AdapterResponse Convert(QbBillViewModel inputViewModel)
        {
            int itemIndex = 0;
            var billMod = new BillMod
            {
                VendorRef = new BasicInfo { FullName = inputViewModel.VendorName },
                VendorAddress = inputViewModel.VendorAddress.ToAddress(),
                TxnDate = inputViewModel.TxnDate.ToDateString(),
                DueDate = inputViewModel.DueDate.ToDateString(),
                //need to confirm about length of PO number from Brandon
                RefNumber = inputViewModel.ReferenceNum.CropToLastChars(20),
                TermsRef = string.IsNullOrWhiteSpace(inputViewModel.PaymentTermName) ? null :
                            new BasicInfo { FullName = inputViewModel.PaymentTermName },
                ItemLineMod = inputViewModel.Items.Union(inputViewModel.DiscountItems)
                .Select(t => new ItemLine
                {
                    TxnLineID = t.IsNewlyAdded ? "-1" : string.Format(itemFormat, itemIndex++),
                    ItemRef = new BasicInfo { FullName = string.IsNullOrWhiteSpace(t.Prefix) ? t.Name : $"{t.Prefix} {t.Name}" },
                    Desc = t.Desc,
                    Quantity = t.Quantity > 0 ? t.Quantity.ToString() : null,
                    Cost = t.Rate,
                    ClassRef = !string.IsNullOrWhiteSpace(inputViewModel.ClassName) ? new BasicInfo { FullName = inputViewModel.ClassName } : null,
                }).ToArray()
            };
            var billModRq = new BillModRq
            {
                BillMod = billMod
            };

            return new AdapterResponse
            {
                QbXmlObject = billModRq,
                QbXmlType = QbXmlType.BillMod,
                Status = QbXmlStatus.NotReadyToQueue,
                EntityId = inputViewModel.OrderId,
                EntityType = QbEntityType.Bill.ToString()
            };
        }
    }
}
