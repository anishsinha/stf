using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobGridViewModel : BaseViewModel
    {
        public JobGridViewModel()
        {
        }

        public JobGridViewModel(Status status) : base(status)
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public string JobID { get; set; }

        public string Address { get; set; }

        public string ContactPerson { get; set; }

        public int AssetAssigned { get; set; }

        public decimal Budget { get; set; }

        public decimal TotalSpend { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string LastUpdated { get; set; }

        public string Status { get; set; }

        public int UserOnboardedTypeId { get; set; }

        public bool IsCompanyOnboardingComplete { get; set; }

        public string RegionId { get; set; }

        public string RegionName { get; set; }

        public bool IsRetailJob { get; set; } = false;
        public string AccountingCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public string RouteId { get; set; }

        public string RouteName { get; set; }
        public string LocationInventoryManagedByNames { get; set; }
        public bool IsMarine { get; set; }
        public string CarrierId { get; set; }
        public string CarrierName { get; set; }
        public string DistanceCovered { get; set; }
    }
}
