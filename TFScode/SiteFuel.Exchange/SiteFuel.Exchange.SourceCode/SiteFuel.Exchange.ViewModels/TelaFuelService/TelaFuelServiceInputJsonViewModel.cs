using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class TelaFuelServiceRequestViewModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string CarrierLookup { get; set; }
        public string FreightLaneLookup { get; set;}
        public string SupplierLookup { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public LocationInventoryManagedBy? LocationInventoryManagedBy { get; set; }
    }

}
