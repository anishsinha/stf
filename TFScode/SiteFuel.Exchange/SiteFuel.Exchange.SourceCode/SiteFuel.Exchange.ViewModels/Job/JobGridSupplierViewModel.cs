using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobGridSupplierViewModel : BaseViewModel
    {
        public JobGridSupplierViewModel()
        {
        }

        public JobGridSupplierViewModel(Status status) : base(status)
        {
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int Id { get; set; }

        public string JobName { get; set; }

        public string JobID { get; set; }

        public string Address { get; set; }

        public string ContactPerson { get; set; }
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string LastUpdated { get; set; }
        public int UserOnboardedTypeId { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public bool IsRetailJob { get; set; }
        public int AssetAssigned { get; set; }
        public string RegionId { get; set; }

        public string RegionName { get; set; }
        public string CarrierId { get; set; }

        public string CarrierName { get; set; }
        public string AccountingCompanyId { get; set; }
        public bool IsBadgeMandatory { get; set; }

        public string RouteId { get; set; }

        public string RouteName { get; set; }
        public string LocationInventoryManagedByNames { get; set; }
        public bool CompanyOwnedLocation { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
    }


    public class JobSummaryModel
    {
        public int JobId { get; set; }
        public string RegionId { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public string CarrierId { get; set; } = string.Empty;
        public string CarrierName { get; set; } = string.Empty;
        public string RouteId { get; set; } = string.Empty;
        public string RouteName { get; set; } = string.Empty;
        public string DistanceCovered { get; set; } = string.Empty;
    }

    public class JobAdditionalDetailsForSummary : StatusViewModel
    {
        public List<JobSummaryModel> JobDetails { get; set; } = new List<JobSummaryModel>();
    }
}
