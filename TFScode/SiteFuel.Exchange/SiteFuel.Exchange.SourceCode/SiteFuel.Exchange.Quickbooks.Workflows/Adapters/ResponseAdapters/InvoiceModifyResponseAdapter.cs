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
    public class InvoiceModifyResponseAdapter : BaseResponseAdapter<InvoiceModRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.InvoiceMod;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var invoiceModRs = DeserializeQbXml<InvoiceModRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (invoiceModRs != null && invoiceModRs.InvoiceReturn != null)
                {
                    template.Templates.Add(TemplateParameter.InvoiceTxnID, invoiceModRs.InvoiceReturn.TxnID);
                    if (invoiceModRs.InvoiceReturn.InvoiceLineReturn != null && invoiceModRs.InvoiceReturn.InvoiceLineReturn.Length > 0)
                    {
                        for (int index = 0; index < invoiceModRs.InvoiceReturn.InvoiceLineReturn.Length; index++)
                        {
                            var txnLineId = invoiceModRs.InvoiceReturn.InvoiceLineReturn[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.InvoiceTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.InvoiceEditSequence, invoiceModRs.InvoiceReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (invoiceModRs != null)
                {
                    template.StatusCode = invoiceModRs.StatusCode;
                }
            }

            return template;
        }
    }
}
