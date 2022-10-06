using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.ResponseAdapters
{
    public class PurchaseOrderAddResponseAdapter : BaseResponseAdapter<PurchaseOrderAddRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.PurchaseOrderAdd;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var purchaseOrderAddRs = DeserializeQbXml<PurchaseOrderAddRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (purchaseOrderAddRs != null && purchaseOrderAddRs.PurchaseOrderReturn != null)
                {
                    template.Templates.Add(TemplateParameter.PurchaseOrderTxnID, purchaseOrderAddRs.PurchaseOrderReturn.TxnID);
                    if (purchaseOrderAddRs.PurchaseOrderReturn.PurchaseOrderLineRet != null && purchaseOrderAddRs.PurchaseOrderReturn.PurchaseOrderLineRet.Length > 0)
                    {
                        for (int index = 0; index < purchaseOrderAddRs.PurchaseOrderReturn.PurchaseOrderLineRet.Length; index++)
                        {
                            var txnLineId = purchaseOrderAddRs.PurchaseOrderReturn.PurchaseOrderLineRet[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.PurchaseOrderTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.PurchaseOrderRefNumber, purchaseOrderAddRs.PurchaseOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.PurchaseOrderEditSequence, purchaseOrderAddRs.PurchaseOrderReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (purchaseOrderAddRs != null)
                {
                    template.StatusCode = purchaseOrderAddRs.StatusCode;
                }
            }

            return template;
        }
    }
}
