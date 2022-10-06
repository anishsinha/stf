using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardAssetsViewModel : StatusViewModel
    {
        public DashboardAssetsViewModel()
        {
           
        }

        public DashboardAssetsViewModel(Status status)
            : base(status)
        {
           
        }

        public int TotalAssetCount { get; set; }
        public int AssignedAssetCount { get; set; }

        public int SelectedJobId { get; set; }
    }
}
