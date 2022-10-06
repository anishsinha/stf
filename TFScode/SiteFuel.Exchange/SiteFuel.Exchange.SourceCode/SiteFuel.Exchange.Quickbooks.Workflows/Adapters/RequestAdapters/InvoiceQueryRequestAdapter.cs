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
    public class InvoiceQueryRequestAdapter : BaseRequestAdapter<QbInvoiceViewModel>
    {
        private readonly string itemFormat = $"{{:{TemplateParameter.InvoiceTxnID}:}}";

        public override AdapterResponse Convert(QbInvoiceViewModel inputViewModel)
        {
            var invoiceQueryRq = new InvoiceQueryRq
            {
                TxnID = string.IsNullOrEmpty(inputViewModel.QbInvoiceTxnID) ? itemFormat : inputViewModel.QbInvoiceTxnID
            };
            
            return new AdapterResponse
            {
                QbXmlObject = invoiceQueryRq,
                QbXmlType = QbXmlType.InvoiceQuery,
                Status = string.IsNullOrEmpty(inputViewModel.QbInvoiceTxnID) ? QbXmlStatus.MapperDependent : QbXmlStatus.ReadyToQueue,
                EntityId = inputViewModel.InvoiceNumberId,
                EntityType = QbEntityType.Invoice.ToString()
            };
        }
    }
}
