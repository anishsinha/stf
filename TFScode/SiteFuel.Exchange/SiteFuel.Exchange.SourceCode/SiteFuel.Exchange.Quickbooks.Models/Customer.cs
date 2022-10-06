using System;
using System.Xml.Serialization;
using SiteFuel.Exchange.Quickbooks.SharedEnums;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class CustomerAdd
    {
        public CustomerAdd()
        {
            IsActive = true;
        }

        public string Name { get; set; }  //41 Chars

        public bool IsActive { get; set; }

        public string CompanyName { get; set; }

        public string Salutation { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Address BillAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string TaxRegistrationNumber { get; set; }
    }

    [Serializable]
    public class CustomerAddRq : QuickbooksXml
    {
        public CustomerAddRq()
        {
            CustomerAdd = new CustomerAdd();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public CustomerAdd CustomerAdd { get; set; }
    }

    [Serializable]
    public class CustomerReturn
    {
        public CustomerReturn()
        {
            BillAddress = new Address();
            ShipAddress = new Address();
        }

        public string ListID { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeModified { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public bool IsActive { get; set; }

        public int Sublevel { get; set; }

        public string CompanyName { get; set; }

        public string Salutation { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Address BillAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string TaxRegistrationNumber { get; set; }
    }

    [Serializable]
    public class CustomerAddRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("CustomerRet")]
        public CustomerReturn CustomerReturn { get; set; }
    }

    [Serializable]
    public class CustomerMod
    {
        public CustomerMod()
        {
            BillAddress = new Address();
            ShipAddress = new Address();
            IsActive = true;
        }

        public string ListID { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }  //41 Chars

        public bool IsActive { get; set; }

        public string CompanyName { get; set; }

        public string Salutation { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Address BillAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string TaxRegistrationNumber { get; set; }
    }

    [Serializable]
    public class CustomerModRq : QuickbooksXml
    {
        public CustomerModRq()
        {
            CustomerMod = new CustomerMod();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public CustomerMod CustomerMod { get; set; }
    }

    [Serializable]
    public class CustomerModRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("CustomerRet")]
        public CustomerReturn CustomerReturn { get; set; }
    }

    [Serializable]
    public class CustomerQueryRq : QuickbooksXml
    {
        public CustomerQueryRq()
        {
            ActiveStatus = ActiveStatus.ActiveOnly;
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public string ListID { get; set; }

        public string FullName { get; set; }

        public ActiveStatus ActiveStatus { get; set; }

        public string FromModifiedDate { get; set; }

        public string ToModifiedDate { get; set; }

        public NameFilter NameFilter { get; set; }
    }

    [Serializable]
    public class CustomerQueryRs : QuickbooksXml
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

        [XmlElement("CustomerRet")]
        public CustomerReturn CustomerReturn { get; set; }
    }
}