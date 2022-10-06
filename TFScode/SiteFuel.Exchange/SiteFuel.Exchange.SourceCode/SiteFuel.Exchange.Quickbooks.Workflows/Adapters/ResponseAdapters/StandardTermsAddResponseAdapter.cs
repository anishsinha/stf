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
    public class StandardTermsAddResponseAdapter : BaseResponseAdapter<StandardTermsAddRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.StdTermsAdd;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var termsQueryRs = DeserializeQbXml<StandardTermsAddRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
				if (termsQueryRs != null && string.Equals(termsQueryRs.StatusCode, (int)QResponseStatusCode.AlreadyExist))
				{
					template.Status = QbXmlStatus.Completed;
				}
				if (termsQueryRs != null && termsQueryRs.StandardTermsRet != null)
                {
                    var termName = termsQueryRs.StandardTermsRet.Name;
                    var termDays = termsQueryRs.StandardTermsRet.StdDueDays;
                    template.Templates.Add($"{termName}", termDays.ToString());
                    template.Status = QbXmlStatus.Completed;
                }
                if (termsQueryRs != null)
                {
                    template.StatusCode = termsQueryRs.StatusCode;
                }
            }

            return template;
        }
    }
}
