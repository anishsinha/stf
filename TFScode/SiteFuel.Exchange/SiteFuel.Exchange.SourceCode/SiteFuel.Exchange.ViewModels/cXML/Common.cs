using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.ViewModels.cXML
{
    [Serializable]
    [XmlRoot(ElementName = "Credential")]
    public class Credential
    {
        [XmlElement(ElementName = "Identity")]
        public string Identity { get; set; }
        [XmlAttribute(AttributeName = "domain")]
        public string Domain { get; set; }
        [XmlElement(ElementName = "SharedSecret")]
        public string SharedSecret { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "From")]
    public class From
    {
        public From()
        {
            Credential = new Credential();
        }
        [XmlElement(ElementName = "Credential")]
        public Credential Credential { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "To")]
    public class To
    {
        public To()
        {
            Credential = new Credential();
        }
        [XmlElement(ElementName = "Credential")]
        public Credential Credential { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Sender")]
    public class Sender
    {
        public Sender()
        {
            Credential = new Credential();
        }
        [XmlElement(ElementName = "Credential")]
        public Credential Credential { get; set; }
        [XmlElement(ElementName = "UserAgent")]
        public string UserAgent { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Header")]
    public class Header
    {
        public Header()
        {
            From = new From();
            To = new To();
            Sender = new Sender();
        }
        [XmlElement(ElementName = "From")]
        public From From { get; set; }
        [XmlElement(ElementName = "To")]
        public To To { get; set; }
        [XmlElement(ElementName = "Sender")]
        public Sender Sender { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Extrinsic")]
    public class Extrinsic
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlText]
        public string Text { get; set; }
    }
}
