using Newtonsoft.Json;
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
    public class CustomerAddResponseAdapter : BaseResponseAdapter<CustomerAddRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.CustomerAdd;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var customerAddRs = DeserializeQbXml<CustomerAddRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
				if (customerAddRs != null && string.Equals(customerAddRs.StatusCode, (int)QResponseStatusCode.AlreadyExist))
				{
					template.Status = QbXmlStatus.Completed;
				}
				if (customerAddRs != null && customerAddRs.CustomerReturn != null)
                {
                    template.Templates.Add(TemplateParameter.CustomerId, customerAddRs.CustomerReturn.ListID);
                    template.Templates.Add(TemplateParameter.CustomerEditSequence, customerAddRs.CustomerReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (customerAddRs != null)
                {
                    template.StatusCode = customerAddRs.StatusCode;
                }
            }

            return template;
        }
    }
}
