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
    public class SalesOrderQueryResponseAdapter : BaseResponseAdapter<SalesOrderQueryRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.SalesOrderQuery;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var salesOrderQueryRs = DeserializeQbXml<SalesOrderQueryRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (salesOrderQueryRs != null && salesOrderQueryRs.SalesOrderReturn != null)
                {
                    var salesOrderReturn = salesOrderQueryRs.SalesOrderReturn;
                    template.Templates.Add(TemplateParameter.SalesOrderTxnID, salesOrderReturn.TxnID);
                    if (salesOrderReturn.SalesOrderLineRet != null && salesOrderReturn.SalesOrderLineRet.Length > 0)
                    {
                        for (int index = 0; index < salesOrderReturn.SalesOrderLineRet.Length; index++)
                        {
                            var txnLineId = salesOrderReturn.SalesOrderLineRet[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.SalesOrderTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.InvoiceRefNumber, salesOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.PurchaseOrderRefNumber, salesOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.SalesOrderRefNumber, salesOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.SalesOrderEditSequence, salesOrderReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (salesOrderQueryRs != null)
                {
                    template.StatusCode = salesOrderQueryRs.StatusCode;
                }
            }

            return template;
        }
    }
}
