using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Adapters.ResponseAdapters;
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
    public class ItemServiceAddResponseAdapter : BaseResponseAdapter<ItemServiceAddRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.ItemAdd;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var itemServiceAddRs = DeserializeQbXml<ItemServiceAddRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (itemServiceAddRs != null && string.Equals(itemServiceAddRs.StatusCode, (int)QResponseStatusCode.AlreadyExist))
                {
                    template.Status = QbXmlStatus.Completed;
                }
                if (itemServiceAddRs != null && itemServiceAddRs.ItemServiceReturn != null)
                {
                    template.Templates.Add(TemplateParameter.ItemId, itemServiceAddRs.ItemServiceReturn.ListID);
                    template.Templates.Add(TemplateParameter.ItemEditSequence, itemServiceAddRs.ItemServiceReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (itemServiceAddRs != null)
                {
                    template.StatusCode = itemServiceAddRs.StatusCode;
                }
            }

            return template;
        }
    }
}
