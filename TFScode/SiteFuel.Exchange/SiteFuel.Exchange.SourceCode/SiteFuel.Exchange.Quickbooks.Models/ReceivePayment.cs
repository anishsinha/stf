using System;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class ReceivePaymentQueryRq : QuickbooksXml
    {
        public ReceivePaymentQueryRq()
        {
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }
        
        public ModifiedDateRangeFilter ModifiedDateRangeFilter { get; set; }

        public bool IncludeLineItems { get; set; } = true;
    }

    [Serializable]
    public class ReceivePaymentQueryRs : QuickbooksXml
    {
        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("ReceivePaymentRet")]
        public ReceivePaymentRet[] ReceivePaymentRet { get; set; }
    }

    [Serializable]
    public class CustomerRef
    {
        [XmlElement("ListID")]
        public string ListID { get; set; }
        [XmlElement("FullName")]
        public string FullName { get; set; }
    }

    [Serializable]
    public class ARAccountRef
    {
        [XmlElement("ListID")]
        public string ListID { get; set; }
        [XmlElement("FullName")]
        public string FullName { get; set; }
    }

    [Serializable]
    public class PaymentMethodRef
    {
        [XmlElement("ListID")]
        public string ListID { get; set; }
        [XmlElement("FullName")]
        public string FullName { get; set; }
    }

    [Serializable]
    public class DepositToAccountRef
    {
        [XmlElement("ListID")]
        public string ListID { get; set; }
        [XmlElement("FullName")]
        public string FullName { get; set; }
    }

    [Serializable]
    public class AppliedToTxnRet
    {
        [XmlElement("TxnID")]
        public string TxnID { get; set; }
        [XmlElement("TxnType")]
        public string TxnType { get; set; }
        [XmlElement("TxnDate")]
        public string TxnDate { get; set; }
        [XmlElement("RefNumber")]
        public string RefNumber { get; set; }
        [XmlElement("BalanceRemaining")]
        public decimal BalanceRemaining { get; set; }
        [XmlElement("Amount")]
        public decimal Amount { get; set; }
        [XmlElement("LinkedTxn")]
        public LinkedTxn[] LinkedTxn { get; set; }
    }

    [Serializable]
    public class ReceivePaymentRet
    {
        [XmlElement("TxnID")]
        public string TxnID { get; set; }
        [XmlElement("TimeCreated")]
        public string TimeCreated { get; set; }
        [XmlElement("TimeModified")]
        public string TimeModified { get; set; }
        [XmlElement("EditSequence")]
        public string EditSequence { get; set; }
        [XmlElement("TxnNumber")]
        public string TxnNumber { get; set; }
        [XmlElement("TxnDate")]
        public string TxnDate { get; set; }
        [XmlElement("RefNumber")]
        public string RefNumber { get; set; }
        [XmlElement("TotalAmount")]
        public decimal TotalAmount { get; set; }
        [XmlElement("PaymentMethodRef")]
        public PaymentMethodRef PaymentMethodRef { get; set; }
        [XmlElement("AppliedToTxnRet")]
        public AppliedToTxnRet[] AppliedToTxnRet { get; set; }
    }
}
