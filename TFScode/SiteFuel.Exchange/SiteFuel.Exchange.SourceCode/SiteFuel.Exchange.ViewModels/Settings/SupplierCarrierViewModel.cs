using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SupplierCarrierViewModel
    {
        public string Id { get; set; }
        public DropdownDisplayItem Carrier { get; set; }
        public List<CarrierJobViewModel> Jobs { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class JobWithEmails
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<DropdownDisplayItem> Emails { get; set; }
    }
    public class CarrierJobViewModel
    {
        public JobWithEmails Job { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
    }

    public class CustomerJobForCarrierViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string RegionId { get; set; }      
        public JobRegionModel Job { get; set; }       
    }

    public class CustomerJobsForCarrierViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<string> RegionIds { get; set; } = new List<string>();
        public List<JobRegionModel> Jobs { get; set; }
    }

    public class JobRegionModel : DropdownDisplayExtendedItem
    {
        public string RegionId { get; set; }
        public int LocationManagedType { get; set; }
    }

    public class CarrierJobWithEmailViewModel
    {
        public int JobId { get; set; }
        public int UserId { get; set; }
        public int CarrierCompanyId { get; set; }
        public string Email { get; set; }
    }

    public class CarrierJobDetailsViewModel: StatusViewModel
    {
        public List<CarrierJobDetailsModel> Carriers { get; set; }
    }
    public class CarrierJobDetailsModel
    {
        public string CarrierCompanyName { get; set; }
        public int CarrierCompanyId { get; set; }
        public int LocationCount { get; set; }
        public string AssignedLocations { get; set; }
    }
}
