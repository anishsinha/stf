
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetGridViewModel : BaseViewModel
    {
        public AssetGridViewModel()
        {
            InstanceInitialize();
        }

        public AssetGridViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            Subcontractors = new List<DropdownDisplayItem>();
        }

        public int JobXAssetsId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Class { get; set; }

        public string CurrentJobName { get; set; }

        public int? CurrentJobId { get; set; }

        public string LastJobName { get; set; }

        public int? LastJobId { get; set; }

        public string DateAdded { get; set; }

        public string Vendor { get; set; }

        public int? ImageId { get; set; }

        public string AzureBlobImageURL { get; set; }

        public List<DropdownDisplayItem> Subcontractors { get; set; }

        public int SubcontractorId { get; set; }

        public int NumberOfAssignedJobs { get; set; }

        public string VehicleId { get; set; }

        public string FuelCapacity { get; set; }

        public string FuelType { get; set; } // product Type

        public string TankType { get; set; }

        public string Threshold { get; set; }

        public string DipTestMethod { get; set; }

        public string TankChart { get; set; }
        public List<int> TanksConnected { get; set; }
        public string TanksConnectedNames { get; set; }
        public Nullable<int> TankSequence { get; set; }
        public List<int> SupplierCompanyIds { get; set; } = new List<int>();
        public string TFXFuelType { get; set; }
        public bool IsStopATGPolling { get; set; }
    }
}
