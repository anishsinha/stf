using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.Workflows.Adapters.ResponseAdapters;
using SiteFuel.Exchange.Quickbooks.Workflows.Interfaces;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.SharedEnums;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.ResponseAdapters
{
    public class SalesOrderAddResponseAdapter : BaseResponseAdapter<SalesOrderAddRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.SalesOrderAdd;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var salesOrderAddRs = DeserializeQbXml<SalesOrderAddRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (salesOrderAddRs != null && salesOrderAddRs.SalesOrderReturn != null)
                {
                    template.Templates.Add(TemplateParameter.SalesOrderTxnID, salesOrderAddRs.SalesOrderReturn.TxnID);
                    template.Templates.Add(TemplateParameter.InvoiceLinkToTxnID, salesOrderAddRs.SalesOrderReturn.TxnID);
                    if (salesOrderAddRs.SalesOrderReturn.SalesOrderLineRet != null && salesOrderAddRs.SalesOrderReturn.SalesOrderLineRet.Length > 0)
                    {
                        for (int index = 0; index < salesOrderAddRs.SalesOrderReturn.SalesOrderLineRet.Length; index++)
                        {
                            var txnLineId = salesOrderAddRs.SalesOrderReturn.SalesOrderLineRet[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.SalesOrderTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.InvoiceRefNumber, salesOrderAddRs.SalesOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.PurchaseOrderRefNumber, salesOrderAddRs.SalesOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.SalesOrderRefNumber, salesOrderAddRs.SalesOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.SalesOrderEditSequence, salesOrderAddRs.SalesOrderReturn.EditSequence);
					template.Status = QbXmlStatus.Completed;
                }
                if (salesOrderAddRs != null)
                {
                    template.StatusCode = salesOrderAddRs.StatusCode;
                }
            }

            return template;
        }
    }
}
