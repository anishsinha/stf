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
    public class SalesOrderModifyResponseAdapter : BaseResponseAdapter<SalesOrderModRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.SalesOrderMod;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var salesOrderModRs = DeserializeQbXml<SalesOrderModRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (salesOrderModRs != null && salesOrderModRs.SalesOrderReturn != null)
                {
                    template.Templates.Add(TemplateParameter.SalesOrderTxnID, salesOrderModRs.SalesOrderReturn.TxnID);
                    template.Templates.Add(TemplateParameter.InvoiceLinkToTxnID, salesOrderModRs.SalesOrderReturn.TxnID);
                    if (salesOrderModRs.SalesOrderReturn.SalesOrderLineRet != null && salesOrderModRs.SalesOrderReturn.SalesOrderLineRet.Length > 0)
                    {
                        for (int index = 0; index < salesOrderModRs.SalesOrderReturn.SalesOrderLineRet.Length; index++)
                        {
                            var txnLineId = salesOrderModRs.SalesOrderReturn.SalesOrderLineRet[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.SalesOrderTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.InvoiceRefNumber, salesOrderModRs.SalesOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.PurchaseOrderRefNumber, salesOrderModRs.SalesOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.SalesOrderRefNumber, salesOrderModRs.SalesOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.SalesOrderEditSequence, salesOrderModRs.SalesOrderReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (salesOrderModRs != null)
                {
                    template.StatusCode = salesOrderModRs.StatusCode;
                }
            }

            return template;
        }
    }
}
