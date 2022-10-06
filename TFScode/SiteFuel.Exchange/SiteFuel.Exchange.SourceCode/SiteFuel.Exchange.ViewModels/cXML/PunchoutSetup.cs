using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.ViewModels.cXML
{
    [Serializable]
    [XmlRoot(ElementName = "Name")]
    public class Name
    {
        [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string Lang { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Contact")]
    public class Contact
    {
        [XmlElement(ElementName = "Name")]
        public Name Name { get; set; }
        [XmlElement(ElementName = "Email")]
        public string Email { get; set; }
        [XmlAttribute(AttributeName = "role")]
        public string Role { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "BrowserFormPost")]
    public class BrowserFormPost
    {
        [XmlElement(ElementName = "URL")]
        public string URL { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "PunchOutSetupRequest")]
    public class PunchOutSetupRequest
    {
        public PunchOutSetupRequest()
        {
            Extrinsic = new List<Extrinsic>();
        }
        [XmlElement(ElementName = "BuyerCookie")]
        public string BuyerCookie { get; set; }
        [XmlElement(ElementName = "Extrinsic")]
        public List<Extrinsic> Extrinsic { get; set; }
        [XmlElement(ElementName = "Contact")]
        public Contact Contact { get; set; }
        [XmlElement(ElementName = "BrowserFormPost")]
        public BrowserFormPost BrowserFormPost { get; set; }
        [XmlAttribute(AttributeName = "operation")]
        public string Operation { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Request")]
    public class Request
    {
        public Request()
        {
            PunchOutSetupRequest = new PunchOutSetupRequest();
        }
        [XmlElement(ElementName = "PunchOutSetupRequest")]
        public PunchOutSetupRequest PunchOutSetupRequest { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "cXML")]
    public class PunchoutRequest
    {
        public PunchoutRequest()
        {
            Header = new Header();
            Request = new Request();
        }
        [XmlElement(ElementName = "Header")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "Request")]
        public Request Request { get; set; }
        [XmlAttribute(AttributeName = "payloadID")]
        public string PayloadID { get; set; }
        [XmlAttribute(AttributeName = "timestamp")]
        public string Timestamp { get; set; }
        [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string Lang { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Status")]
    public class Status
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "StartPage")]
    public class StartPage
    {
        [XmlElement(ElementName = "URL")]
        public string URL { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "PunchOutSetupResponse")]
    public class PunchOutSetupResponse
    {
        public PunchOutSetupResponse()
        {
            StartPage = new StartPage();
        }
        [XmlElement(ElementName = "StartPage")]
        public StartPage StartPage { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Response")]
    public class Response
    {
        public Response()
        {
            PunchOutSetupResponse = new PunchOutSetupResponse();
        }
        [XmlElement(ElementName = "Status")]
        public Status Status { get; set; }
        [XmlElement(ElementName = "PunchOutSetupResponse")]
        public PunchOutSetupResponse PunchOutSetupResponse { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "cXML")]
    public class PunchoutResponse
    {
        public PunchoutResponse()
        {
            Response = new Response();
        }

        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }
        [XmlAttribute(AttributeName = "payloadID")]
        public string PayloadID { get; set; }
        [XmlAttribute(AttributeName = "timestamp")]
        public string Timestamp { get; set; }
        [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string Lang { get; set; }
    }
}