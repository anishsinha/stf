using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SiteFuel.Exchange.Quickbooks.SharedEnums;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public  class ClassAdd
    {
        public ClassAdd()
        {
            IsActive = true;
        }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }

    [Serializable]
    public class ClassReturn
    {
        public int ListID { get; set; }

        public string TimeCreated { get; set; }

        public string TimeModified { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string IsActive { get; set; }
    }

    [Serializable]
    public class ClassAddRq
    {
        public ClassAddRq()
        {
            ClassAdd = new ClassAdd();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public ClassAdd ClassAdd { get; set; }
    }

    [Serializable]
    public class ClassAddRs
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("ClassRet")]
        public ClassReturn ClassReturn { get; set; }
    }

    [Serializable]
    public class ClassMod
    {
        public ClassMod()
        {
            IsActive = true;
        }

        public string ListID { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }

    public class ClassModRq
    {
        public ClassModRq()
        {
            ClassMod = new ClassMod();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public ClassMod ClassMod { get; set; }
    }

    [Serializable]
    public class ClassModRs
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("ClassRet")]
        public ClassReturn ClassReturn { get; set; }
    }

    [Serializable]
    public class ClassQueryRq
    {
        public ClassQueryRq()
        {
            ActiveStatus = ActiveStatus.ActiveOnly;
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public string ListID { get; set; }

        public string FullName { get; set; }

        /// <summary>
        /// ActiveStatus may have one of the following values: ActiveOnly [DEFAULT], InactiveOnly, All
        /// </summary>
        public ActiveStatus ActiveStatus { get; set; }

        public string FromModifiedDate { get; set; }

        public string ToModifiedDate { get; set; }

        public NameFilter NameFilter { get; set; }
    }

    [Serializable]
    public class ClassQueryRs
    {
        public ClassQueryRs()
        {
            ClassReturn = new ClassReturn();
        }

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

        [XmlElement("ClassRet")]
        public ClassReturn ClassReturn { get; set; }
    }
}
