using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.ViewModels.cXML
{
    [XmlRoot(ElementName = "Money")]
    public class Money
    {
        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Total")]
    public class Total
    {
        public Total()
        {
            Money = new Money();
        }
        [XmlElement(ElementName = "Money")]
        public Money Money { get; set; }
    }

    [XmlRoot(ElementName = "PunchOutOrderMessageHeader")]
    public class PunchOutOrderMessageHeader
    {
        public PunchOutOrderMessageHeader()
        {
            Total = new Total();
        }
        [XmlElement(ElementName = "Total")]
        public Total Total { get; set; }
        [XmlAttribute(AttributeName = "operationAllowed")]
        public string OperationAllowed { get; set; }
    }

    [XmlRoot(ElementName = "ItemID")]
    public class ItemID
    {
        [XmlElement(ElementName = "SupplierPartID")]
        public string SupplierPartID { get; set; }
        [XmlElement(ElementName = "SupplierPartAuxiliaryID")]
        public string SupplierPartAuxiliaryID { get; set; }
    }

    [XmlRoot(ElementName = "UnitPrice")]
    public class UnitPrice
    {
        public UnitPrice()
        {
            Money = new Money();
        }
        [XmlElement(ElementName = "Money")]
        public Money Money { get; set; }
    }

    [XmlRoot(ElementName = "Description")]
    public class Description
    {
        public Description()
        {
            Lang = "en";
        }
        [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string Lang { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Classification")]
    public class Classification
    {
        [XmlAttribute(AttributeName = "domain")]
        public string Domain { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ItemDetail")]
    public class ItemDetail
    {
        public ItemDetail()
        {
            UnitPrice = new UnitPrice();
            Description = new Description();
            Classification = new Classification();
            Extrinsic = new List<Extrinsic>();
        }
        [XmlElement(ElementName = "UnitPrice")]
        public UnitPrice UnitPrice { get; set; }
        [XmlElement(ElementName = "Description")]
        public Description Description { get; set; }
        [XmlElement(ElementName = "UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }
        [XmlElement(ElementName = "Classification")]
        public Classification Classification { get; set; }
        [XmlElement(ElementName = "ManufacturerPartID")]
        public string ManufacturerPartID { get; set; }
        [XmlElement(ElementName = "ManufacturerName")]
        public string ManufacturerName { get; set; }
        [XmlElement(ElementName = "Extrinsic")]
        public List<Extrinsic> Extrinsic { get; set; }
    }

    [XmlRoot(ElementName = "ItemIn")]
    public class ItemIn
    {
        public ItemIn()
        {
            ItemID = new ItemID();
            ItemDetail = new ItemDetail();
        }
        [XmlElement(ElementName = "ItemID")]
        public ItemID ItemID { get; set; }
        [XmlElement(ElementName = "ItemDetail")]
        public ItemDetail ItemDetail { get; set; }
        [XmlAttribute(AttributeName = "quantity")]
        public string Quantity { get; set; }
    }

    [XmlRoot(ElementName = "PunchOutOrderMessage")]
    public class PunchOutOrderMessage
    {
        public PunchOutOrderMessage()
        {
            PunchOutOrderMessageHeader = new PunchOutOrderMessageHeader();
            ItemIn = new List<ItemIn>();
        }
        [XmlElement(ElementName = "BuyerCookie")]
        public string BuyerCookie { get; set; }
        [XmlElement(ElementName = "PunchOutOrderMessageHeader")]
        public PunchOutOrderMessageHeader PunchOutOrderMessageHeader { get; set; }
        [XmlElement(ElementName = "ItemIn")]
        public List<ItemIn> ItemIn { get; set; }
    }

    [XmlRoot(ElementName = "Message")]
    public class Message
    {
        public Message()
        {
            PunchOutOrderMessage = new PunchOutOrderMessage();
        }
        [XmlElement(ElementName = "PunchOutOrderMessage")]
        public PunchOutOrderMessage PunchOutOrderMessage { get; set; }
    }

    [XmlRoot(ElementName = "cXML")]
    public class OrderMessage
    {
        public OrderMessage()
        {
            Header = new Header();
            Message = new Message();
        }
        [XmlElement(ElementName = "Header")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "Message")]
        public Message Message { get; set; }
        [XmlAttribute(AttributeName = "timestamp")]
        public string Timestamp { get; set; }
        [XmlAttribute(AttributeName = "payloadID")]
        public string PayloadID { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
    }
}
