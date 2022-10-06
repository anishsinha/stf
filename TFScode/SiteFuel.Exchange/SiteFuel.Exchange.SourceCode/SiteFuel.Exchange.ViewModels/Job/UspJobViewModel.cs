using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspJobViewModel : BaseViewModel
    {
        public UspJobViewModel()
        {
        }

        public UspJobViewModel(Status status) : base(status)
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayJobId { get; set; }

        public string Address { get; set; }

        public string ContactPerson { get; set; }

        public int AssetAssigned { get; set; }

        public decimal Budget { get; set; }

        public decimal TotalSpend { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public string Status { get; set; }

        public int BuyerCompanyId { get; set; }

        public string BuyerCompanyName { get; set; }

        public int OnboardedTypeId { get; set; }

        public bool IsOnboardingComplete { get; set; }

        public string CreatedBy { get; set; }

        public bool IsRetailJob { get; set; }
        public string AccountingCompanyId { get; set; }
        public bool IsBadgeMandatory { get; set; }
        public Nullable<LocationInventoryManagedBy> LocationInventoryManagedBy { get; set; }
        public bool CompanyOwnedLocation { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
    }

    public class UspJobProductModel
    {
        public int JobId { get; set; }
        public int AssetId { get; set; }
        public string JobName { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int AcceptedCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int UoM { get; set; }
        public int OrderId { get; set; }
        public string TfxPoNumber { get; set; }

    }
}
