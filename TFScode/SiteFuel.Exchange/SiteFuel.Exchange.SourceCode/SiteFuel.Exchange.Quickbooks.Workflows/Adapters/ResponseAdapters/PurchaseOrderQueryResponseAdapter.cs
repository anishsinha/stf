using SiteFuel.Exchange.Quickbooks.Models;
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
    public class PurchaseOrderQueryResponseAdapter : BaseResponseAdapter<PurchaseOrderQueryRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.PurchaseOrderQuery;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var purchaseOrderQueryRs = DeserializeQbXml<PurchaseOrderQueryRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (purchaseOrderQueryRs != null && purchaseOrderQueryRs.PurchaseOrderReturn != null)
                {
                    var purchaseOrderReturn = purchaseOrderQueryRs.PurchaseOrderReturn;
                    template.Templates.Add(TemplateParameter.PurchaseOrderTxnID, purchaseOrderReturn.TxnID);
                    template.Templates.Add(TemplateParameter.BillLinkToTxnID, purchaseOrderReturn.TxnID);
                    if (purchaseOrderReturn.PurchaseOrderLineRet != null && purchaseOrderReturn.PurchaseOrderLineRet.Length > 0)
                    {
                        for (int index = 0; index < purchaseOrderReturn.PurchaseOrderLineRet.Length; index++)
                        {
                            var txnLineId = purchaseOrderReturn.PurchaseOrderLineRet[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.PurchaseOrderTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.PurchaseOrderRefNumber, purchaseOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.PurchaseOrderEditSequence, purchaseOrderReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (purchaseOrderQueryRs != null)
                {
                    template.StatusCode = purchaseOrderQueryRs.StatusCode;
                }
            }

            return template;
        }
    }
}
