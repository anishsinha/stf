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
    public class InvoiceAddResponseAdapter : BaseResponseAdapter<InvoiceAddRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.InvoiceAdd;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var invoiceAddRs = DeserializeQbXml<InvoiceAddRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (invoiceAddRs != null && invoiceAddRs.InvoiceReturn != null)
                {
                    template.Templates.Add(TemplateParameter.InvoiceTxnID, invoiceAddRs.InvoiceReturn.TxnID);
                    if (invoiceAddRs.InvoiceReturn.InvoiceLineReturn != null && invoiceAddRs.InvoiceReturn.InvoiceLineReturn.Length > 0)
                    {
                        for (int index = 0; index < invoiceAddRs.InvoiceReturn.InvoiceLineReturn.Length; index++)
                        {
                            var txnLineId = invoiceAddRs.InvoiceReturn.InvoiceLineReturn[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.InvoiceTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.InvoiceNumber, invoiceAddRs.InvoiceReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.InvoiceEditSequence, invoiceAddRs.InvoiceReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                    template.StatusCode = invoiceAddRs.StatusCode;
                }
                if (invoiceAddRs != null)
                {
                    template.StatusCode = invoiceAddRs.StatusCode;
                }
            }

            return template;
        }
    }
}
