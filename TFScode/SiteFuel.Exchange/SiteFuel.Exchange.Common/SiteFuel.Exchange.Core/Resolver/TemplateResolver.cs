using System.Text;
using System.Text.RegularExpressions;

namespace SiteFuel.Exchange.Core.Resolver
{
	public static class TemplateResolver
	{
		static string format = "{{:{0}:}}";
		static Regex regex = new Regex(@"{:\w*:}");

		static public string Resolve(string incompleteXmls, TemplateResponse templateResponse)
		{
			var incompleteXmlBuilder = new StringBuilder(incompleteXmls);
            try
            {
                foreach (var item in templateResponse.Templates)
                {
                    var stringToReplace = string.Format(format, item.Key);
                    incompleteXmlBuilder = incompleteXmlBuilder.Replace(stringToReplace, item.Value);
                }
            }
            catch
            {
                // In some cases value is coming null
            }
			return incompleteXmlBuilder.ToString();
		}

		static public bool IsXmlComplete(string qbXml)
		{
			var match =  regex.Match(qbXml);
			return !match.Success;
		}
	}
}
