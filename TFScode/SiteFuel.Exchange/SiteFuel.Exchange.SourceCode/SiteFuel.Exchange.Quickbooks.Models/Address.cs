using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable()]
    public class Address
    {
        public string Addr1 { get; set; }

        public string Addr2 { get; set; }

        public string Addr3 { get; set; }

        public string Addr4 { get; set; }

        public string Addr5 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }
    }
}
