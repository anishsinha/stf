using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.FreightModels
{
    public class CarrierViewModel:CommonFieldsModel
    {
        public string Id { get; set; }
        public DropdownDisplayItem Carrier { get; set; }
        public List<JobViewModel> Jobs { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
    }

    public class JobViewModel
    {
        public DropdownDisplayItem Job { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
    }

    public class DipTestRequestModel
    {
        public int CompanyId { get; set; }
        public CompanyType CompanyTypeId { get; set; }
        public int JobId { get; set; }
    }
    public class CarrierJobDetailsViewModel : StatusModel
    {
        public List<CarrierJobDetailsModel> Carriers { get; set; } = new List<CarrierJobDetailsModel>();
    }
    public class CarrierJobDetailsModel
    {
        public string CarrierCompanyName { get; set; }
        public int CarrierCompanyId { get; set; }
        public int LocationCount { get; set; }
        public string AssignedLocations { get; set; }
        public List<string> Locations { get; set; }
    }
}
