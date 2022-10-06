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
    public class BillAddResponseAdapter : BaseResponseAdapter<BillAddRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.BillAdd;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var billAddRs = DeserializeQbXml<BillAddRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (billAddRs != null && billAddRs.BillReturn != null)
                {
                    template.Templates.Add(TemplateParameter.BillTxnID, billAddRs.BillReturn.TxnID);
                    if (billAddRs.BillReturn.ItemLineReturn != null && billAddRs.BillReturn.ItemLineReturn.Length > 0)
                    {
                        for (int index = 0; index < billAddRs.BillReturn.ItemLineReturn.Length; index++)
                        {
                            var txnId = billAddRs.BillReturn.ItemLineReturn[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.BillTxnLineID, index), txnId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.BillRefNumber, billAddRs.BillReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.BillEditSequence, billAddRs.BillReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (billAddRs != null)
                {
                    template.StatusCode = billAddRs.StatusCode;
                }
            }

            return template;
        }
    }
}
