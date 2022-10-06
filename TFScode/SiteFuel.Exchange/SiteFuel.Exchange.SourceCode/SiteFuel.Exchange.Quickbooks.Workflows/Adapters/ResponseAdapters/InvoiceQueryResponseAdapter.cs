using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Interfaces;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.ResponseAdapters
{
    public class InvoiceQueryResponseAdapter : BaseResponseAdapter<InvoiceQueryRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.InvoiceQuery;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var invoiceQueryRs = DeserializeQbXml<InvoiceQueryRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (invoiceQueryRs != null && invoiceQueryRs.InvoiceReturn != null)
                {
                    var invoiceReturn = invoiceQueryRs.InvoiceReturn;
                    template.Templates.Add(TemplateParameter.InvoiceTxnID, invoiceReturn.TxnID);
                    if (invoiceReturn.LinkedTxn != null)
                    {
                        var linkedTxn = invoiceReturn.LinkedTxn.FirstOrDefault(t => t.TxnType == QbTxnType.SalesOrder.ToString());
                        if (linkedTxn != null)
                        {
                            template.Templates.Add(TemplateParameter.SalesOrderTxnID, linkedTxn.TxnID);
                        }
                    }
                    if (invoiceReturn.InvoiceLineReturn != null && invoiceReturn.InvoiceLineReturn.Length > 0)
                    {
                        for (int index = 0; index < invoiceReturn.InvoiceLineReturn.Length; index++)
                        {
                            var txnLineId = invoiceReturn.InvoiceLineReturn[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.InvoiceTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.OriginalInvoiceQbNumber, invoiceReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.InvoiceEditSequence, invoiceReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (invoiceQueryRs != null)
                {
                    template.StatusCode = invoiceQueryRs.StatusCode;
                }
            }

            return template;
        }
    }
}
