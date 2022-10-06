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
    public class BillModifyResponseAdapter : BaseResponseAdapter<BillModRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.BillMod;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var billModRs = DeserializeQbXml<BillModRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (billModRs != null && billModRs.BillReturn != null)
                {
                    template.Templates.Add(TemplateParameter.BillTxnID, billModRs.BillReturn.TxnID);
                    if (billModRs.BillReturn.ItemLineReturn != null && billModRs.BillReturn.ItemLineReturn.Length > 0)
                    {
                        for (int index = 0; index < billModRs.BillReturn.ItemLineReturn.Length; index++)
                        {
                            var txnId = billModRs.BillReturn.ItemLineReturn[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.BillTxnLineID, index), txnId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.BillRefNumber, billModRs.BillReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.BillEditSequence, billModRs.BillReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                    template.StatusCode = billModRs.StatusCode;
                }
                if (billModRs != null)
                {
                    template.StatusCode = billModRs.StatusCode;
                }
            }

            return template;
        }
    }
}
