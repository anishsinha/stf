using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetDropHistoryViewModel : StatusViewModel
    {
        public AssetDropHistoryViewModel()
        {
        }

        public AssetDropHistoryViewModel(Status status)
            : base(status)
        {
        }

        public string DropDate { get; set; }

        public string DropTime { get; set; }

        public decimal DropAmount { get; set; }

        public string SubcontractorName { get; set; }

        public string TimeZoneName { get; set; }
    }
}
