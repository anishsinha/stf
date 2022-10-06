using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class TelaFuelServiceSupplierDetails
    {
        public List<TelaSuppliers> TelaSuppliers { get; set; }
    }

    public class TelaSuppliers
    {
        public int CompanyId { get; set; }
        public string CarrierLookup { get; set; }
        public string FreightLaneLookup { get; set; }
        public string SupplierLookup { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<int> BuyerCompanyIds { get; set; }
    }
}
