using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters
{
    public abstract class BaseRequestAdapter<inputType>
        where inputType :WorkflowRequest
    {
        public abstract AdapterResponse Convert(inputType inputViewModel);

        public XmlDocument GetBaseQBXml()
        {
            var BaseXmlDoc = SerializeQbXmlBaseObject(new QBXML());
            return BaseXmlDoc;
        }

        public static XmlDocument SerializeQbXmlBaseObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            var xmlDoc = new XmlDocument();
            using (MemoryStream textStream = new MemoryStream())
            {
                try
                {
                    var encoding = Encoding.GetEncoding(QbConstants.QbEncoding);
                    var streamWriter = new StreamWriter(textStream, encoding);
                    var xmlns = new XmlSerializerNamespaces();
                    xmlns.Add(string.Empty, string.Empty);
                    xmlSerializer.Serialize(streamWriter, toSerialize, xmlns);

                    //Add ProcessingInstruction qbxml
                    textStream.Seek(0, SeekOrigin.Begin);
                    xmlDoc.Load(textStream);

                    XmlProcessingInstruction processingInstruction = xmlDoc.CreateProcessingInstruction("qbxml", "version=\"13.0\"");
                    xmlDoc.InsertAfter(processingInstruction, xmlDoc.FirstChild);
                }
                catch
                {
                    throw;
                }
                return xmlDoc;
            }
        }

        public string SerializeQbXmlObject<T>(T toSerialize, XmlDocument BaseXmlDoc)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            using (MemoryStream textStream = new MemoryStream())
            {
                try
                {
                    var xmlns = new XmlSerializerNamespaces();
                    xmlns.Add(string.Empty, string.Empty);
                    xmlSerializer.Serialize(textStream, toSerialize, xmlns);

                    var xmlDoc = new XmlDocument();
                    textStream.Seek(0, SeekOrigin.Begin);
                    xmlDoc.Load(textStream);

                    XmlNode importedDocument = BaseXmlDoc.ImportNode(xmlDoc.DocumentElement, true);
                    BaseXmlDoc.DocumentElement.LastChild.AppendChild(importedDocument);
                }
                catch
                {
                    throw;
                }
                return BaseXmlDoc.OuterXml;
            }
        }

        public AdapterResponse GetQbXml(inputType viewModel)
        {
            var adapterResponse = Convert(viewModel);
            var baseXml = GetBaseQBXml();
			adapterResponse.QbXml = SerializeQbXmlObject(adapterResponse.QbXmlObject, baseXml);
            return adapterResponse;
        }
    }

    [GeneratedCodeAttribute("System.Xml", "1.0")]
    [Serializable]
    public class QBXML
    {
        public QBXML()
        {
            QBXMLMsgsRq = new QBXMLMsgs();
        }

        [XmlElement("QBXMLMsgsRq")]
        public QBXMLMsgs QBXMLMsgsRq { get; set; }
    }

    [Serializable]
    public class QBXMLMsgs
    {
        public QBXMLMsgs()
        {
            OnError = "stopOnError";
        }

        [XmlAttribute("onError")]
        public string OnError { get; set; }
    }
}