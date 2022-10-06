using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Core.Utilities
{
    public static class XmlSerialization
    {
        public static T Deserialize<T>(string xmlDocumentStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader textReader = new StringReader(xmlDocumentStream);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
            {
                return (T)serializer.Deserialize(xmlReader);
            }
        }

        public static string Serialize<T>(T serializableObject)
        {
            string xmlString = string.Empty;
            MemoryStream memoryStream = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                // If set to true XmlWriter would close MemoryStream automatically and using would then do double dispose
                CloseOutput = false,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false,
                Indent = true
            };
            using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
            {
                xmlWriter.WriteDocType("cXML", null, "http://xml.cxml.org/schemas/cXML/1.2.007/cXML.dtd", null);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(xmlWriter, serializableObject);
            }
            xmlString = Encoding.UTF8.GetString(memoryStream.ToArray());
            return xmlString;
        }
    }
}
