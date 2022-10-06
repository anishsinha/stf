using System;
using System.Xml.Serialization;
using SiteFuel.Exchange.Quickbooks.SharedEnums;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class VendorAdd
    {
        public VendorAdd()
        {
            IsActive = true;
        }

        public string Name { get; set; }  //41 Chars

        public bool IsActive { get; set; }

        public string CompanyName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Address VendorAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }

    [Serializable]
    public class VendorMod
    {
        public string ListID { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }  //41 Chars

        public bool IsActive { get; set; }

        public string CompanyName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Address VendorAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }

    [Serializable]
    public class VendorReturn
    {
        public VendorReturn()
        {
            VendorAddress = new Address();
            ShipAddress = new Address();
        }

        public string ListID { get; set; }

        public string TimeCreated { get; set; }

        public string TimeModified { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string CompanyName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Address VendorAddress { get; set; }

        public Address ShipAddress { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }

    [Serializable]
    public class VendorAddRq :QuickbooksXml
    {
        public VendorAddRq()
        {
            VendorAdd = new VendorAdd();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public VendorAdd VendorAdd { get; set; }
    }

    [Serializable]
    public class VendorAddRs : QuickbooksXml
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("VendorRet")]
        public VendorReturn VendorReturn { get; set; }
    }

    [Serializable]
    public class VendorModRq
    {
        public VendorModRq()
        {
            VendorMod = new VendorMod();
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public VendorMod VendorMod { get; set; }
    }

    [Serializable]
    public class VendorModRs
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("VendorRet")]
        public VendorReturn VendorReturn { get; set; }
    }

    [Serializable]
    public class VendorQueryRq
    {
        public VendorQueryRq()
        {
            ActiveStatus = ActiveStatus.ActiveOnly;
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public int ListID { get; set; }

        public string FullName { get; set; }

        /// <summary>
        /// ActiveStatus may have one of the following values: ActiveOnly [DEFAULT], InactiveOnly, All
        /// </summary>
        public ActiveStatus ActiveStatus { get; set; }

        public string FromModifiedDate { get; set; }

        public string ToModifiedDate { get; set; }

        /// <summary>
        /// MatchCriterion may have one of the following values: StartsWith, Contains, EndsWith
        /// </summary>
        public NameFilter NameFilter { get; set; }
    }

    [Serializable]
    public class VendorQueryRs
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

        [XmlElement("VendorRet")]
        public VendorReturn VendorReturn { get; set; }
    }
}
