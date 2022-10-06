using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Interfaces;
using System.Xml;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.ResponseAdapters
{
    public class ReceivePaymentQueryResponseAdapter : BaseResponseAdapter<ReceivePaymentQueryRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.ReceivePaymentQuery;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var receivePaymentQueryRs = DeserializeQbXml<ReceivePaymentQueryRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (receivePaymentQueryRs != null)
                {
                    template.Status = QbXmlStatus.Completed;
                    template.StatusCode = receivePaymentQueryRs.StatusCode;
                }
                if (receivePaymentQueryRs != null)
                {
                    template.StatusCode = receivePaymentQueryRs.StatusCode;
                }
            }

            return template;
        }
    }
}
