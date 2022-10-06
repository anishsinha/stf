using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class StandardTermsAdd
    {
        public StandardTermsAdd()
        {
            IsActive = true;
        }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int StdDueDays { get; set; }

        public int StdDiscountDays { get; set; }

        public decimal DiscountPct { get; set; }
    }

    [Serializable]
    public class StandardTermsAddRq :QuickbooksXml
    {
        public StandardTermsAddRq()
        {
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public StandardTermsAdd StandardTermsAdd { get; set; }
    }

    [Serializable]
    public class StandardTermsRet
    {
        public string ListID { get; set; }

        public string TimeCreated { get; set; }

        public string TimeModified { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int StdDueDays { get; set; }

        public int StdDiscountDays { get; set; }

        public decimal DiscountPct { get; set; }
    }

    [Serializable]
    public class StandardTermsAddRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        public StandardTermsRet StandardTermsRet { get; set; }
    }

    [Serializable]
    public class StandardTermsQueryRq
    {
        public StandardTermsQueryRq()
        {
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public string ListID { get; set; }

        public string FullName { get; set; }

        /// <summary>
        /// ActiveStatus may have one of the following values: ActiveOnly [DEFAULT], InactiveOnly, All
        /// </summary>
        public string ActiveStatus { get; set; }

        public string FromModifiedDate { get; set; }

        public string ToModifiedDate { get; set; }

        public NameFilter NameFilter { get; set; }
    }

    [Serializable]
    public class StandardTermsQueryRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("StandardTermsRet")]
        public StandardTermsRet[] StandardTermsRet { get; set; }
    }
}
