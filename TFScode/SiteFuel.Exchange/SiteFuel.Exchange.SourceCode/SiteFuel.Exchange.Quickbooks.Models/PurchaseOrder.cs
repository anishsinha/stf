using System;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class PurchaseOrderAdd
    {
        public PurchaseOrderAdd()
        {
            RefNumber = "{:PurchaseOrderRefNumber:}";
            VendorAddress = new Address();
            ShipAddress = new Address();
        }

        public BasicInfo VendorRef { get; set; }

        public BasicInfo ClassRef { get; set; }

        public BasicInfo ShipToEntityRef { get; set; }

        public string TxnDate { get; set; }

        public string RefNumber { get; set; }

        public Address VendorAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string DueDate { get; set; }

        public string Memo { get; set; }

        [XmlElement("PurchaseOrderLineAdd")]
        public OrderItem[] PurchaseOrderLineAdd { get; set; }
    }

    [Serializable]
    public class PurchaseOrderMod
    {
        public PurchaseOrderMod()
        {
            TxnID = "{:PurchaseOrderTxnID:}";
            EditSequence = "{:PurchaseOrderEditSequence:}";
            VendorAddress = new Address();
            ShipAddress = new Address();
        }

        public string TxnID { get; set; }

        public string EditSequence { get; set; }

        public BasicInfo VendorRef { get; set; }

        public BasicInfo ClassRef { get; set; }

        public BasicInfo ShipToEntityRef { get; set; }

        public string TxnDate { get; set; }

        public string RefNumber { get; set; }

        public Address VendorAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string DueDate { get; set; }

        public string Memo { get; set; }

        [XmlElement("PurchaseOrderLineMod")]
        public OrderItem[] PurchaseOrderLineMod { get; set; }
    }

    [Serializable]
    public class PurchaseOrderAddRq : QuickbooksXml
    {
        public PurchaseOrderAddRq()
        {
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public PurchaseOrderAdd PurchaseOrderAdd { get; set; }
    }

    [Serializable]
    public class PurchaseOrderReturn
    {
        public PurchaseOrderReturn()
        {
            VendorAddress = new Address();
            ShipAddress = new Address();
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        public string TxnID { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeModified { get; set; }

        public string EditSequence { get; set; }

        public int TxnNumber { get; set; }

        public BasicInfo VendorRef { get; set; }

        public BasicInfo ClassRef { get; set; }

        public BasicInfo ShipToEntityRef { get; set; }

        public string TxnDate { get; set; }

        public string RefNumber { get; set; }

        public Address VendorAddress { get; set; }

        public Address ShipAddress { get; set; }

        public DateTime DueDate { get; set; }

        [XmlElement("PurchaseOrderLineRet")]
        public OrderItem[] PurchaseOrderLineRet { get; set; }
    }

    [Serializable]
    public class PurchaseOrderAddRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("PurchaseOrderRet")]
        public PurchaseOrderReturn PurchaseOrderReturn { get; set; }
    }

    [Serializable]
    public class PurchaseOrderModRq : QuickbooksXml
    {
        public PurchaseOrderModRq()
        {
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public PurchaseOrderMod PurchaseOrderMod { get; set; }
    }

    [Serializable]
    public class PurchaseOrderModRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("PurchaseOrderRet")]
        public PurchaseOrderReturn PurchaseOrderReturn { get; set; }
    }

    [Serializable]
    public class PurchaseOrderQueryRq : QuickbooksXml
    {
        public PurchaseOrderQueryRq()
        {
            RequestId = "{:RequestId:}";
            IncludeLineItems = true;
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("metaData")]
        public string MetaData { get; set; }

        [XmlAttribute("iterator")]
        public string Iterator { get; set; }

        [XmlAttribute("iteratorID")]
        public string IteratorID { get; set; }

        public string TxnID { get; set; }

        public ModifiedDateRangeFilter ModifiedDateRangeFilter { get; set; }

        public TxnDateRangeFilter TxnDateRangeFilter { get; set; }

        public CommonFilter EntityFilter { get; set; }

        public CommonFilter AccountFilter { get; set; }

        public bool IncludeLineItems { get; set; }
    }

    [Serializable]
    public class PurchaseOrderQueryRs :QuickbooksXml
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

        [XmlElement("PurchaseOrderRet")]
        public PurchaseOrderReturn PurchaseOrderReturn { get; set; }
    }
}
