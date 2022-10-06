using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class SalesOrderAdd
    {
        public SalesOrderAdd()
        {
            CustomerRef = new BasicInfo();
            BillAddress = new Address();
            ShipAddress = new Address();
        }

        public BasicInfo CustomerRef { get; set; }

        public string TxnDate { get; set; }

        public string RefNumber { get; set; }

        public Address BillAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string PONumber { get; set; }

        public string DueDate { get; set; }

        public string ShipDate { get; set; }

        public string Memo { get; set; }

        [XmlElement("SalesOrderLineAdd")]
        public OrderItem[] SalesOrderLineAdd { get; set; }
    }

    [Serializable]
    public class SalesOrderMod
    {
        public SalesOrderMod()
        {
            TxnID = "{:SalesOrderTxnID:}";
            EditSequence = "{:SalesOrderEditSequence:}";
            CustomerRef = new BasicInfo();
            BillAddress = new Address();
            ShipAddress = new Address();
        }

        public string TxnID { get; set; }

        public string EditSequence { get; set; }

        public BasicInfo CustomerRef { get; set; }

        public string TxnDate { get; set; }

        public string RefNumber { get; set; }

        public Address BillAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string PONumber { get; set; }

        public string DueDate { get; set; }

        public string ShipDate { get; set; }

        public string Memo { get; set; }

        [XmlElement("SalesOrderLineMod")]
        public OrderItem[] SalesOrderLineMod { get; set; }
    }

    [Serializable]
    public class SalesOrderAddRq : QuickbooksXml
    {
        public SalesOrderAddRq()
        {
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public SalesOrderAdd SalesOrderAdd { get; set; }
    }

    [Serializable]
    public class SalesOrderModRq : QuickbooksXml
    {
        public SalesOrderModRq()
        {
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public SalesOrderMod SalesOrderMod { get; set; }
    }

    [Serializable]
    public class SalesOrderLineRet
    {
        public string TxnLineID { get; set; }

        public BasicInfo ItemRef { get; set; }

        public string Desc { get; set; }

        public decimal Quantity { get; set; }

        public decimal Rate { get; set; }

        public BasicInfo ClassRef { get; set; }

        public decimal Amount { get; set; }
    }

    [Serializable]
    public class SalesOrderReturn
    {
        public SalesOrderReturn()
        {
            BillAddress = new Address();
            ShipAddress = new Address();
        }

        public string TxnID { get; set; }

        public string TimeCreated { get; set; }

        public string TimeModified { get; set; }

        public string EditSequence { get; set; }

        public string TxnNumber { get; set; }

        public BasicInfo CustomerRef { get; set; }

        public string TxnDate { get; set; }

        public string RefNumber { get; set; }

        public Address BillAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string PONumber { get; set; }

        public string DueDate { get; set; }

        public string ShipDate { get; set; }

        public string Memo { get; set; }

        [XmlElement("SalesOrderLineRet")]
        public SalesOrderLineRet[] SalesOrderLineRet { get; set; }
    }

    [Serializable]
    public class SalesOrderAddRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("SalesOrderRet")]
        public SalesOrderReturn SalesOrderReturn { get; set; }
    }

    [Serializable]
    public class SalesOrderModRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("SalesOrderRet")]
        public SalesOrderReturn SalesOrderReturn { get; set; }
    }

    [Serializable]
    public class SalesOrderQueryRq : QuickbooksXml
    {
        public SalesOrderQueryRq()
        {
            IncludeLineItems = true;
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

        public string TxnID { get; set; }

        public string RefNumber { get; set; }

        public ModifiedDateRangeFilter ModifiedDateRangeFilter { get; set; }

        public TxnDateRangeFilter TxnDateRangeFilter { get; set; }

        public CommonFilter EntityFilter { get; set; }

        public bool IncludeLineItems { get; set; }
    }

    [Serializable]
    public class SalesOrderQueryRs : QuickbooksXml
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

        [XmlElement("SalesOrderRet")]
        public SalesOrderReturn SalesOrderReturn { get; set; }
    }
}