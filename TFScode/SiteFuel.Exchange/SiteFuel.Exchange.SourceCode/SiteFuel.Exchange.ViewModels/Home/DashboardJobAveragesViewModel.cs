using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardJobAveragesViewModel : StatusViewModel
    {
        public DashboardJobAveragesViewModel()
        {
            InstanceInitialize();
        }

        public DashboardJobAveragesViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            StatusCode = status;
            FuelTypes = new List<DropdownDisplayItem>();
        }

        public int OrderCount { get; set; }
        public int TotalDrops { get; set; }
        public decimal AveragePpgPerDrop { get; set; }
        public decimal AverageGallonsPerDrop { get; set; }

        public int FuelTypeId { get; set; }
        public int FuelTypeName { get; set; }
        public List<DropdownDisplayItem> FuelTypes { get; set; }

        public int SelectedJobId { get; set; }

        public string GroupIds { get; set; }

        public bool IsCollapsed { get; set; }
    }
}
