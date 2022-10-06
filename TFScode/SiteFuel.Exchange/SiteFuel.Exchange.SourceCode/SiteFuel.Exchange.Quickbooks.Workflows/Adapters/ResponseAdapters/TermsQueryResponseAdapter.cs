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
    public class TermsQueryResponseAdapter : BaseResponseAdapter<TermsQueryRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.TermsQuery;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
			template.ChainedAction = "SavePaymentTerms";
			if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var termsQueryRs = DeserializeQbXml<TermsQueryRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (termsQueryRs != null && termsQueryRs.StandardTermsRet != null && termsQueryRs.StandardTermsRet.Length > 0)
                {
                    for (int index = 0; index < termsQueryRs.StandardTermsRet.Length; index++)
                    {
                        var termName = termsQueryRs.StandardTermsRet[index].Name;
                        var termDays = termsQueryRs.StandardTermsRet[index].StdDueDays;
                        template.Templates.Add($"{termName}", termDays.ToString());
                    }
                    template.Status = QbXmlStatus.Completed;
                    template.StatusCode = termsQueryRs.StatusCode;
                }
                //if (termsQueryRs != null && termsQueryRs.DateDrivenTermsRet != null && termsQueryRs.DateDrivenTermsRet.Length > 0)
                //{
                //    //TODO: Code to save/update terms in database
                //}
            }

            return template;
        }
    }
}
