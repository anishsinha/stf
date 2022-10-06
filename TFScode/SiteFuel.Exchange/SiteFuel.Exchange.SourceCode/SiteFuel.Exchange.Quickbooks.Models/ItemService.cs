using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SiteFuel.Exchange.Quickbooks.SharedEnums;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class BasicInfo
    {
        public string ListID { get; set; }

        public string FullName { get; set; }
    }

    [Serializable]
    public class SalesOrPurchase
    {
        public string Desc { get; set; }

        public string Price { get; set; }

        public BasicInfo AccountRef { get; set; }
    }

    [Serializable]
    public class ItemServiceAdd
    {
        public string Name { get; set; }

        public BasicInfo ClassRef { get; set; }

        public BasicInfo UnitOfMeasureSetRef { get; set; }

        public SalesOrPurchase SalesOrPurchase { get; set; }
    }

    [Serializable]
    public class ItemServiceAddRq : QuickbooksXml
    {
        public ItemServiceAddRq()
        {
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public ItemServiceAdd ItemServiceAdd { get; set; }
    }

    [Serializable]
    public class ItemServiceReturn
    {
        public string ListID { get; set; }

        public string TimeCreated { get; set; }

        public string TimeModified { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public BasicInfo ClassRef { get; set; }

        public int Sublevel { get; set; }

        public BasicInfo UnitOfMeasureSetRef { get; set; }

        public bool IsTaxIncluded { get; set; }

        public SalesOrPurchase SalesOrPurchase { get; set; }
    }

    [Serializable]
    public class ItemServiceAddRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("ItemServiceRet")]
        public ItemServiceReturn ItemServiceReturn { get; set; }
    }

    [Serializable]
    public class ItemServiceMod
    {
        public string ListID { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        public BasicInfo ClassRef { get; set; }

        public BasicInfo UnitOfMeasureSetRef { get; set; }

        public SalesOrPurchase SalesOrPurchase { get; set; }
    }

    [Serializable]
    public class ItemServiceModRq
    {
        public ItemServiceModRq()
        {
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public ItemServiceMod ItemServiceMod { get; set; }
    }

    [Serializable]
    public class ItemServiceQueryRq
    {
        public ItemServiceQueryRq()
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
    public class ItemServiceQueryRs
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

        [XmlElement("ItemServiceRet")]
        public ItemServiceReturn ItemServiceRet { get; set; }
    }
}