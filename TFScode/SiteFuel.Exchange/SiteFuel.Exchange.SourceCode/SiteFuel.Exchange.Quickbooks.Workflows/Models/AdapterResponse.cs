using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
	public class AdapterResponse
	{
        public AdapterResponse()
        {
            ProgressMessages = new List<string>();
        }
        public string QbXml { get; set; }
		public QuickbooksXml QbXmlObject { get; set; }
		public QbXmlType QbXmlType { get; set; }
		public QbXmlStatus Status { get; set; }
		public List<string> ProgressMessages { get; set; }
        public int EntityId { get; set; }
        public string EntityType { get; set; }
    }
}
