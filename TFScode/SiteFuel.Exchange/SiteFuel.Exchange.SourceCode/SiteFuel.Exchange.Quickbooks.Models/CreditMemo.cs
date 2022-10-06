using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class CreditMemoLineAdd
    {
        public BasicInfo ItemRef { get; set; }

        public string Desc { get; set; }

        public string Quantity { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal Rate { get; set; }
    }

    [Serializable]
    public class CreditMemoAdd
    {
        public CreditMemoAdd()
        {
            Other = "{:InvoiceTxnID:}";
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

        public string Other { get; set; }


        [XmlElement("CreditMemoLineAdd")]
        public CreditMemoLineAdd[] CreditMemoLineAdd { get; set; }
    }

    [Serializable]
    public class CreditMemoAddRq : QuickbooksXml
    {
        public CreditMemoAddRq()
        {
            CreditMemoAdd = new CreditMemoAdd();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public CreditMemoAdd CreditMemoAdd { get; set; }
    }

    [Serializable]
    public class CreditMemoLineReturn
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
    public class CreditMemoReturn
    {
        public CreditMemoReturn()
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

        public decimal TotalAmount { get; set; }

        public string Memo { get; set; }

        [XmlElement("LinkedTxn")]
        public LinkedTxn[] LinkedTxn { get; set; }

        [XmlElement("CreditMemoLineRet")]
        public CreditMemoLineReturn[] CreditMemoLineReturn { get; set; }
    }


    [Serializable]
    public class CreditMemoAddRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("CreditMemoRet")]
        public CreditMemoReturn CreditMemoReturn { get; set; }
    }
}
