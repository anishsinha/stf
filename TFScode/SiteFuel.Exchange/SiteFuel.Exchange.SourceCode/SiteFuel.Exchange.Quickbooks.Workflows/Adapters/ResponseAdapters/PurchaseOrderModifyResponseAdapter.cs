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
    public class PurchaseOrderModifyResponseAdapter : BaseResponseAdapter<PurchaseOrderModRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.PurchaseOrderMod;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var purchaseOrderModRs = DeserializeQbXml<PurchaseOrderModRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (purchaseOrderModRs != null && purchaseOrderModRs.PurchaseOrderReturn != null)
                {
                    template.Templates.Add(TemplateParameter.PurchaseOrderTxnID, purchaseOrderModRs.PurchaseOrderReturn.TxnID);
                    var purchaseOrderLineRet = purchaseOrderModRs.PurchaseOrderReturn.PurchaseOrderLineRet;
                    if (purchaseOrderLineRet != null && purchaseOrderLineRet.Length > 0)
                    {
                        for (int index = 0; index < purchaseOrderLineRet.Length; index++)
                        {
                            var txnLineId = purchaseOrderLineRet[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.PurchaseOrderTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.PurchaseOrderRefNumber, purchaseOrderModRs.PurchaseOrderReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.PurchaseOrderEditSequence, purchaseOrderModRs.PurchaseOrderReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (purchaseOrderModRs != null)
                {
                    template.StatusCode = purchaseOrderModRs.StatusCode;
                }
            }

            return template;
        }
    }
}
