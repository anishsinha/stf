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
    public class InvoiceModifyRequestAdapter : BaseRequestAdapter<QbInvoiceViewModel>
    {
        private readonly string itemFormat = $"{{{{:{TemplateParameter.InvoiceTxnLineID}:}}}}";

        public override AdapterResponse Convert(QbInvoiceViewModel inputViewModel)
        {
            int itemIndex = 0;
            var invoiceMod = new InvoiceMod
            {
                CustomerRef = new BasicInfo { FullName = inputViewModel.CustomerName },
                TxnDate = inputViewModel.TxnDate.ToDateString(),
                BillAddress = inputViewModel.BillAddress.ToAddress(),
                ShipAddress = inputViewModel.ShipAddress.ToAddress(),
                PONumber = inputViewModel.PONumber.CropToLastChars(25),
                TermsRef = string.IsNullOrWhiteSpace(inputViewModel.PaymentTermName) ? null :
                            new BasicInfo { FullName = inputViewModel.PaymentTermName },
                DueDate = inputViewModel.DueDate.ToDateString(),
                ShipDate = inputViewModel.ShipDate.ToDateString(),
                Memo = inputViewModel.Memo,
                InvoiceLineMod = inputViewModel.Items.Union(inputViewModel.DiscountItems)
                .Select(t => new InvoiceLineMod
                {
                    TxnLineID = t.IsNewlyAdded ? "-1" : string.Format(itemFormat, itemIndex++),
                    ItemRef = new BasicInfo { FullName = t.Name },
                    Desc = t.Desc,
                    Quantity = t.Quantity > 0 ? t.Quantity.ToString() : null,
                    Rate = t.Rate
                }).ToArray()
            };

            var invoiceModRq = new InvoiceModRq
            {
                InvoiceMod = invoiceMod
            };

            return new AdapterResponse
            {
                QbXmlObject = invoiceModRq,
                QbXmlType = QbXmlType.InvoiceMod,
                Status = QbXmlStatus.NotReadyToQueue,
                EntityId = inputViewModel.InvoiceNumberId,
                EntityType = QbEntityType.Invoice.ToString()
            };
        }
    }
}
