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
    public class VendorAddResponseAdapter : BaseResponseAdapter<VendorAddRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.VendorAdd;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var vendorAddRs = DeserializeQbXml<VendorAddRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
				if (vendorAddRs != null && string.Equals(vendorAddRs.StatusCode, (int)QResponseStatusCode.AlreadyExist))
				{
					template.Status = QbXmlStatus.Completed;
				}
				if (vendorAddRs != null && vendorAddRs.VendorReturn != null)
                {
                    template.Templates.Add(TemplateParameter.VendorId, vendorAddRs.VendorReturn.ListID);
                    template.Templates.Add(TemplateParameter.VendorEditSequence, vendorAddRs.VendorReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                }
                if (vendorAddRs != null)
                {
                    template.StatusCode = vendorAddRs.StatusCode;
                }
            }

            return template;
        }
    }
}
