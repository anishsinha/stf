using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class ItemLineAdd
    {
        public BasicInfo ItemRef { get; set; }

        public string Desc { get; set; }

        public string Quantity { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal Cost { get; set; }

        public BasicInfo CustomerRef { get; set; }
    }

    [Serializable]
    public class VendorCreditAdd
    {
        public BasicInfo VendorRef { get; set; }

        public string TxnDate { get; set; }

        public string RefNumber { get; set; }

        public string Memo { get; set; }

        [XmlElement("ItemLineAdd")]
        public ItemLineAdd[] ItemLineAdd { get; set; }
    }

    [Serializable]
    public class VendorCreditAddRq : QuickbooksXml
    {
        public VendorCreditAddRq()
        {
            VendorCreditAdd = new VendorCreditAdd();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public VendorCreditAdd VendorCreditAdd { get; set; }
    }


    [Serializable]
    public class VendorCreditReturn
    {
        public string TxnID { get; set; }

        public string TimeCreated { get; set; }

        public string TimeModified { get; set; }

        public string EditSequence { get; set; }

        public int TxnNumber { get; set; }

        public BasicInfo VendorRef { get; set; }

        public string TxnDate { get; set; }

        public decimal CreditAmount { get; set; }

        public string RefNumber { get; set; }
       
        public string Memo { get; set; }

        [XmlElement("LinkedTxn")]
        public LinkedTxn[] LinkedTxn { get; set; }

        [XmlElement("ItemLineRet")]
        public ItemLine[] ItemLineReturn { get; set; }
    }


    [Serializable]
    public class VendorCreditAddRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("VendorCreditRet")]
        public VendorCreditReturn VendorCreditReturn { get; set; }
    }
}
