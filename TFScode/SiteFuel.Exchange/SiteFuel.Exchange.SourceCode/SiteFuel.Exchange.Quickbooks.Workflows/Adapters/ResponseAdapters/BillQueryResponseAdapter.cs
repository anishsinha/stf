using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Interfaces;
using System.Xml;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.ResponseAdapters
{
    public class BillQueryResponseAdapter : BaseResponseAdapter<BillQueryRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.BillQuery;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var billQueryRs = DeserializeQbXml<BillQueryRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (billQueryRs != null && billQueryRs.BillReturn != null)
                {
                    var billReturn = billQueryRs.BillReturn;
                    template.Templates.Add(TemplateParameter.BillTxnID, billReturn.TxnID);
                    template.Templates.Add(TemplateParameter.Memo, new System.Xml.Linq.XText(billReturn.Memo ?? string.Empty).ToString());

                    if (billReturn.ItemLineReturn != null && billReturn.ItemLineReturn.Length > 0)
                    {
                        for (int index = 0; index < billReturn.ItemLineReturn.Length; index++)
                        {
                            var txnLineId = billReturn.ItemLineReturn[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.BillTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.OriginalBillQbNumber, billReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.BillEditSequence, billReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (billQueryRs != null)
                {
                    template.StatusCode = billQueryRs.StatusCode;
                }
            }

            return template;
        }
    }
}
