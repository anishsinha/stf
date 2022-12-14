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
    public class CreditMemoAddResponseAdapter : BaseResponseAdapter<CreditMemoAddRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.CreditMemoAdd;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var creditMemoAddRs = DeserializeQbXml<CreditMemoAddRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (creditMemoAddRs != null && creditMemoAddRs.CreditMemoReturn != null)
                {
                    template.Templates.Add(TemplateParameter.CreditMemoTxnID, creditMemoAddRs.CreditMemoReturn.TxnID);
                    if (creditMemoAddRs.CreditMemoReturn.CreditMemoLineReturn != null && creditMemoAddRs.CreditMemoReturn.CreditMemoLineReturn.Length > 0)
                    {
                        for (int index = 0; index < creditMemoAddRs.CreditMemoReturn.CreditMemoLineReturn.Length; index++)
                        {
                            var txnLineId = creditMemoAddRs.CreditMemoReturn.CreditMemoLineReturn[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.CreditMemoTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.CreditMemoRefNumber, creditMemoAddRs.CreditMemoReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.CreditMemoEditSequence, creditMemoAddRs.CreditMemoReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                    template.StatusCode = creditMemoAddRs.StatusCode;
                }
                if (creditMemoAddRs != null)
                {
                    template.StatusCode = creditMemoAddRs.StatusCode;
                }
            }

            return template;
        }
    }
}
