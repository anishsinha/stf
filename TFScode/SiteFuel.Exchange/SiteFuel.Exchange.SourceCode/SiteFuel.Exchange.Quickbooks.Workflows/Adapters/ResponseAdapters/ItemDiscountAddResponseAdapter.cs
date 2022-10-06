using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.Workflows.Adapters.ResponseAdapters;
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
    public class ItemDiscountAddResponseAdapter : BaseResponseAdapter<ItemDiscountAddRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.DiscountAdd;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var itemDiscountAddRs = DeserializeQbXml<ItemDiscountAddRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (itemDiscountAddRs != null && string.Equals(itemDiscountAddRs.StatusCode, (int)QResponseStatusCode.AlreadyExist))
                {
                    template.Status = QbXmlStatus.Completed;
                }
                if (itemDiscountAddRs != null && itemDiscountAddRs.ItemDiscountReturn != null)
                {
                    template.Templates.Add(TemplateParameter.ItemId, itemDiscountAddRs.ItemDiscountReturn.ListID);
                    template.Templates.Add(TemplateParameter.ItemEditSequence, itemDiscountAddRs.ItemDiscountReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (itemDiscountAddRs != null)
                {
                    template.StatusCode = itemDiscountAddRs.StatusCode;
                }
            }

            return template;
        }
    }
}
