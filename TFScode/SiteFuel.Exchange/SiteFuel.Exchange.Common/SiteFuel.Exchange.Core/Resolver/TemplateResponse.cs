using SiteFuel.Exchange.Quickbooks.SharedEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Core.Resolver
{
	public class TemplateResponse
	{
		public string ChainedAction { get; set; }
		public Dictionary<string, string> Templates { get; set; } = new Dictionary<string, string>();
		public QbXmlStatus Status { get; set; } = QbXmlStatus.Failed;
        public int StatusCode { get; set; }

		public TemplateResponse ShallowClone()
		{
			return new TemplateResponse()
			{
				Templates = new Dictionary<string, string>(Templates)
			};
		}

        public void AddTemplate(string key , string val)
        {
            Templates.Add(key, val);
        }
	}
}
