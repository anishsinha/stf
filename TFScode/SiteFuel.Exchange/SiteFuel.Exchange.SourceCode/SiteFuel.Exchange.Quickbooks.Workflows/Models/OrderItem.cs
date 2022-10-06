using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
    public class OrderItemViewModel : WorkflowRequest
    {
        public string Name { get; set; }

        public string AccountName { get; set; }

        public string Desc { get; set; }

        public decimal Quantity { get; set; }

        public decimal Rate { get; set; }

		public bool IsNewlyAdded { get; set; }

        public string Prefix { get; set; }
    }
}
