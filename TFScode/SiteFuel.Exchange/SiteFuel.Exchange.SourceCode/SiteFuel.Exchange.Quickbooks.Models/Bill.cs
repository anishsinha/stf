using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class ItemLine
    {
        public string TxnLineID { get; set; }

        public BasicInfo ItemRef { get; set; }

        public string Desc { get; set; }

        public string Quantity { get; set; }

        public decimal Cost { get; set; }

        public BasicInfo ClassRef { get; set; }
    }


    [Serializable]
    public class BillAdd
    {
        public BillAdd()
        {
            RefNumber = "{:BillRefNumber:}";
            LinkToTxnID = "{:BillLinkToTxnID:}";
            VendorAddress = new Address();
        }

        public BasicInfo VendorRef { get; set; }

        public Address VendorAddress { get; set; }

        public string TxnDate { get; set; }

        public string DueDate { get; set; }

        public string RefNumber { get; set; }

        public BasicInfo TermsRef { get; set; }

        public string Memo { get; set; }

        public string LinkToTxnID { get; set; }

        [XmlElement("ItemLineAdd")]
        public ItemLine[] ItemLineAdd { get; set; }
    }

    [Serializable]
    public class BillAddRq : QuickbooksXml
    {
        public BillAddRq()
        {
            BillAdd = new BillAdd();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public BillAdd BillAdd { get; set; }
    }

    [Serializable]
    public class BillAddRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("BillRet")]
        public BillReturn BillReturn { get; set; }
    }

    [Serializable]
    public class BillReturn
    {
        public BillReturn()
        {
            VendorAddress = new Address();
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

        public Address VendorAddress { get; set; }

        public string TxnDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal AmountDue { get; set; }

        public string RefNumber { get; set; }

        public string Memo { get; set; }

        [XmlElement("ItemLineRet")]
        public ItemLine[] ItemLineReturn { get; set; }
    }

    [Serializable]
    public class BillMod : QuickbooksXml
    {
        public BillMod()
        {
            TxnID = "{:BillTxnID:}";
            EditSequence = "{:BillEditSequence:}";
            VendorAddress = new Address();
        }

        public string TxnID { get; set; }

        public string EditSequence { get; set; }

        public BasicInfo VendorRef { get; set; }

        public Address VendorAddress { get; set; }

        public string TxnDate { get; set; }

        public string DueDate { get; set; }

        public string RefNumber { get; set; }

        public BasicInfo TermsRef { get; set; }

         public string Memo { get; set; } = "{:Memo:}";

        [XmlElement("ItemLineMod")]
        public ItemLine[] ItemLineMod { get; set; }
    }

    [Serializable]
    public class BillModRq : QuickbooksXml
    {
        public BillModRq()
        {
            BillMod = new BillMod();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public BillMod BillMod { get; set; }
    }

    [Serializable]
    public class BillModRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("BillRet")]
        public BillReturn BillReturn { get; set; }
    }

    [Serializable]
    public class BillQueryRq : QuickbooksXml
    {
        public BillQueryRq()
        {
            RequestId = "{:RequestId:}";
            IncludeLineItems = true;
            IncludeLinkedTxns = true;
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

        public CommonFilter EntityFilter { get; set; }

        public CommonFilter AccountFilter { get; set; }

        public bool IncludeLineItems { get; set; }

        public bool IncludeLinkedTxns { get; set; }
    }

    [Serializable]
    public class BillQueryRs : QuickbooksXml
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

        [XmlElement("BillRet")]
        public BillReturn BillReturn { get; set; }
    }
}
