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
    public class InvoiceAddRequestAdapter : BaseRequestAdapter<QbInvoiceViewModel>
    {
        public override AdapterResponse Convert(QbInvoiceViewModel inputViewModel)
        {
            var invoiceAdd = new InvoiceAdd
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
            };
            var InvoiceAddRq = new InvoiceAddRq
            {
                InvoiceAdd = invoiceAdd
            };

            return new AdapterResponse
            {
                QbXmlObject = InvoiceAddRq,
                QbXmlType = QbXmlType.InvoiceAdd,
                Status = QbXmlStatus.NotReadyToQueue,
                EntityId = inputViewModel.InvoiceNumberId,
                EntityType = QbEntityType.Invoice.ToString()
            };
        }
    }
}
