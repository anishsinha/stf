using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class TermsQueryRq : QuickbooksXml
    {
        public TermsQueryRq()
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
    public class DateDrivenTermsRet
    {
        public string ListID { get; set; }

        public string TimeCreated { get; set; }

        public string TimeModified { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int DayOfMonthDue { get; set; }

        public int DueNextMonthDays { get; set; }

        public int DiscountDayOfMonth { get; set; }

        public decimal DiscountPct { get; set; }
    }

    [Serializable]
    public class TermsQueryRs : QuickbooksXml
    {

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlElement("StandardTermsRet")]
        public StandardTermsRet[] StandardTermsRet { get; set; }

        [XmlElement("DateDrivenTermsRet")]
        public DateDrivenTermsRet[] DateDrivenTermsRet { get; set; }
    }
}
