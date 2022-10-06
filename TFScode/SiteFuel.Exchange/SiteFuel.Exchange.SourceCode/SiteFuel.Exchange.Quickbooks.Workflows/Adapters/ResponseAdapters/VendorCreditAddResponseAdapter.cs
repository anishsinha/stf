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
    public class VendorCreditAddResponseAdapter : BaseResponseAdapter<VendorCreditAddRs>, IResponseAdapter
    {
        public QbXmlType Type { get; set; } = QbXmlType.VendorCreditAdd;

        public TemplateResponse ResolveResponse(string xml)
        {
            var template = new TemplateResponse();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                var vendorCreditAddRs = DeserializeQbXml<VendorCreditAddRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                if (vendorCreditAddRs != null && vendorCreditAddRs.VendorCreditReturn != null)
                {
                    template.Templates.Add(TemplateParameter.VendorCreditTxnID, vendorCreditAddRs.VendorCreditReturn.TxnID);
                    if (vendorCreditAddRs.VendorCreditReturn.ItemLineReturn != null && vendorCreditAddRs.VendorCreditReturn.ItemLineReturn.Length > 0)
                    {
                        for (int index = 0; index < vendorCreditAddRs.VendorCreditReturn.ItemLineReturn.Length; index++)
                        {
                            var txnLineId = vendorCreditAddRs.VendorCreditReturn.ItemLineReturn[index].TxnLineID;
                            template.Templates.Add(string.Format(TemplateParameter.VendorCreditTxnLineID, index), txnLineId);
                        }
                    }
                    template.Templates.Add(TemplateParameter.VendorCreditRefNumber, vendorCreditAddRs.VendorCreditReturn.RefNumber);
                    template.Templates.Add(TemplateParameter.VendorCreditEditSequence, vendorCreditAddRs.VendorCreditReturn.EditSequence);
                    template.Status = QbXmlStatus.Completed;
                    template.StatusCode = vendorCreditAddRs.StatusCode;
                }
                if (vendorCreditAddRs != null)
                {
                    template.StatusCode = vendorCreditAddRs.StatusCode;
                }
            }

            return template;
        }
    }
}
