using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class InvoiceLineAdd
    {
        public BasicInfo ItemRef { get; set; }

        public string Desc { get; set; }

        public decimal Quantity { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal Rate { get; set; }
    }

    [Serializable]
    public class InvoiceAdd
    {
        public InvoiceAdd()
        {
            RefNumber = "{:InvoiceRefNumber:}";
            LinkToTxnID = "{:InvoiceLinkToTxnID:}";
            BillAddress = new Address();
            ShipAddress = new Address();
        }

        public BasicInfo CustomerRef { get; set; }

        public string TxnDate { get; set; }

        public string RefNumber { get; set; }

        public Address BillAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string PONumber { get; set; }

        public BasicInfo TermsRef { get; set; }

        public string DueDate { get; set; }

        public string ShipDate { get; set; }

        public string Memo { get; set; }

        public string LinkToTxnID { get; set; }

        [XmlElement("InvoiceLineAdd")]
        public InvoiceLineAdd[] InvoiceLineAdd { get; set; }
    }

    [Serializable]
    public class InvoiceAddRq : QuickbooksXml
    {
        public InvoiceAddRq()
        {
            InvoiceAdd = new InvoiceAdd();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public InvoiceAdd InvoiceAdd { get; set; }
    }

    [Serializable]
    public class InvoiceLineReturn
    {
        public string TxnLineID { get; set; }

        public BasicInfo ItemRef { get; set; }

        public string Desc { get; set; }

        public decimal Quantity { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }
    }

    [Serializable]
    public class InvoiceReturn
    {
        public InvoiceReturn()
        {
            BillAddress = new Address();
            ShipAddress = new Address();
        }

        public string TxnID { get; set; }

        public string TimeCreated { get; set; }

        public string TimeModified { get; set; }

        public string EditSequence { get; set; }

        public int TxnNumber { get; set; }

        public BasicInfo CustomerRef { get; set; }

        public string TxnDate { get; set; }

        public string RefNumber { get; set; }

        public Address BillAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string PONumber { get; set; }

        public string DueDate { get; set; }

        public string ShipDate { get; set; }

        public decimal Subtotal { get; set; }

        public decimal SalesTaxTotal { get; set; }

        public decimal AppliedAmount { get; set; }

        public string Memo { get; set; }

        [XmlElement("LinkedTxn")]
        public LinkedTxn[] LinkedTxn { get; set; }

        [XmlElement("InvoiceLineRet")]
        public InvoiceLineReturn[] InvoiceLineReturn { get; set; }
    }

    public class LinkedTxn
    {
        public string TxnID { get; set; }

        public string TxnType { get; set; }

        public string RefNumber { get; set; }

        public decimal Amount { get; set; }
    }

    [Serializable]
    public class InvoiceAddRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("InvoiceRet")]
        public InvoiceReturn InvoiceReturn { get; set; }
    }

    [Serializable]
    public class InvoiceLineMod
    {
        public string TxnLineID { get; set; }

        public BasicInfo ItemRef { get; set; }

        public string Desc { get; set; }

        public string Quantity { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal Rate { get; set; }
    }

    [Serializable]
    public class InvoiceMod : QuickbooksXml
    {
        public InvoiceMod()
        {
            TxnID = "{:InvoiceTxnID:}";
            EditSequence = "{:InvoiceEditSequence:}";
            BillAddress = new Address();
            ShipAddress = new Address();
        }

        public string TxnID { get; set; }

        public string EditSequence { get; set; }

        public BasicInfo CustomerRef { get; set; }

        public BasicInfo ClassRef { get; set; }

        public string TxnDate { get; set; }

        public Address BillAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string PONumber { get; set; }

        public BasicInfo TermsRef { get; set; }

        public string DueDate { get; set; }

        public string ShipDate { get; set; }

        public string Memo { get; set; }

        [XmlElement("InvoiceLineMod")]
        public InvoiceLineMod[] InvoiceLineMod { get; set; }
    }

    [Serializable]
    public class InvoiceModRq : QuickbooksXml
    {
        public InvoiceModRq()
        {
            InvoiceMod = new InvoiceMod();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public InvoiceMod InvoiceMod { get; set; }
    }

    [Serializable]
    public class InvoiceModRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("InvoiceRet")]
        public InvoiceReturn InvoiceReturn { get; set; }
    }

    [Serializable]
    public class InvoiceQueryRq : QuickbooksXml
    {
        public InvoiceQueryRq()
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

        public ModifiedDateRangeFilter ModifiedDateRangeFilter { get; set; }

        public CommonFilter EntityFilter { get; set; }

        public CommonFilter AccountFilter { get; set; }

        public bool IncludeLineItems { get; set; }

        public bool IncludeLinkedTxns { get; set; }
    }

    [Serializable]
    public class InvoiceQueryRs : QuickbooksXml
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

        [XmlElement("InvoiceRet")]
        public InvoiceReturn InvoiceReturn { get; set; }
    }
}
