using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SiteFuel.Exchange.Quickbooks.SharedEnums;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class ItemDiscountAdd
    {
        public string Name { get; set; }

        public BasicInfo ClassRef { get; set; }

        public string ItemDesc { get; set; }

        public BasicInfo AccountRef { get; set; }
    }

    [Serializable]
    public class ItemDiscountAddRq : QuickbooksXml
    {
        public ItemDiscountAddRq()
        {
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public ItemDiscountAdd ItemDiscountAdd { get; set; }
    }

    [Serializable]
    public class ItemDiscountReturn
    {
        public string ListID { get; set; }

        public string TimeCreated { get; set; }

        public string TimeModified { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public BasicInfo ClassRef { get; set; }

        public int Sublevel { get; set; }

        public string ItemDesc { get; set; }

        public BasicInfo AccountRef { get; set; }
    }

    [Serializable]
    public class ItemDiscountAddRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("ItemDiscountRet")]
        public ItemDiscountReturn ItemDiscountReturn { get; set; }
    }

    [Serializable]
    public class ItemDiscountMod
    {
        public string ListID { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        public BasicInfo ClassRef { get; set; }

        public string ItemDesc { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DiscountRatePercent { get; set; }

        public BasicInfo AccountRef { get; set; }
    }

    [Serializable]
    public class ItemDiscountModRs
    {
        public ItemDiscountModRs()
        {
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public ItemDiscountMod ItemDiscountMod { get; set; }
    }

    [Serializable]
    public class ItemDiscountQueryRq
    {
        public ItemDiscountQueryRq()
        {
            ActiveStatus = ActiveStatus.ActiveOnly;
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("metaData")]
        public string MetaData { get; set; }

        [XmlAttribute("iterator")]
        public string Iterator { get; set; }

        [XmlAttribute("iteratorID")]
        public string IteratorId { get; set; }

        public string ListID { get; set; }

        public string FullName { get; set; }

        public ActiveStatus ActiveStatus { get; set; }

        public string FromModifiedDate { get; set; }

        public string ToModifiedDate { get; set; }

        public NameFilter NameFilter { get; set; }
    }

    [Serializable]
    public class ItemDiscountQueryRs
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlAttribute("retCount")]
        public int ReturnCount { get; set; }

        [XmlAttribute("iteratorRemainingCount")]
        public int IteratorRemainingCount { get; set; }

        [XmlAttribute("iteratorID")]
        public string IteratorId { get; set; }

        [XmlElement("ItemDiscountRet")]
        public ItemDiscountReturn ItemDiscountReturn { get; set; }
    }
}