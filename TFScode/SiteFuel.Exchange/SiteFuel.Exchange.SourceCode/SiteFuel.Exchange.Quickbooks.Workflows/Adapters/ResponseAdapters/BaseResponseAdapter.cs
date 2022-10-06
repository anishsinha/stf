using SiteFuel.Exchange.Quickbooks.Models;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.ResponseAdapters
{
	public class BaseResponseAdapter<returnType> where returnType : QuickbooksXml
	{
		public T DeserializeQbXml<T>(string toDeserialize)
		{
			T response;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(returnType));
			using (MemoryStream textStream = new MemoryStream())
			{

				var streamWriter = new StreamWriter(textStream, Encoding.UTF8);
				streamWriter.Write(toDeserialize);
				streamWriter.Flush();
				textStream.Seek(0, SeekOrigin.Begin);

				var xmlns = new XmlSerializerNamespaces();
				xmlns.Add(string.Empty, string.Empty);
				var target = xmlSerializer.Deserialize(textStream);
				response = (T)target;
			}
			return response;
		}

	}
}
