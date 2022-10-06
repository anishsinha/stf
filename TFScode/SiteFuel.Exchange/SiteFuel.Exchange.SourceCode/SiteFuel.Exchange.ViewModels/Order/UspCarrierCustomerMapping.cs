using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspCarrierCustomerMapping
    {
        public int BuyerCompanyId { get; set; }
        public string BuyerName { get; set; }
        public int TotalOrders { get; set; }
        public int TotalDDTCount { get; set; }
        public int TotalInvoiceCount { get; set; }
        public string CarrierAssignedCustomerId { get; set; }
        public int? Id { get; set; }
    }

    public class DsbCalenderFiltersDataViewModel
    {
        public DsbCalenderFiltersDataViewModel()
        {
            CustomerList = new List<UspCarrierCustomerMapping>();
            Locations = new List<DropdownDisplayExtendedProperty>();
            Vessels = new List<DropdownDisplayExtendedProperty>();
        }
        public List<UspCarrierCustomerMapping> CustomerList { get; set; }
        public List<DropdownDisplayExtendedProperty> Locations { get; set; }
        public List<DropdownDisplayExtendedProperty> Vessels { get; set; }

    }
}
