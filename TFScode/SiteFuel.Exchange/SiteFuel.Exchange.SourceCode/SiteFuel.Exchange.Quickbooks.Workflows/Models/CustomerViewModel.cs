using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
    public class CustomerViewModel : WorkflowRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CompanyName { get; set; }

        public AddressViewModel BillAddress { get; set; }

        public AddressViewModel ShipAddress { get; set; }
    }
}
