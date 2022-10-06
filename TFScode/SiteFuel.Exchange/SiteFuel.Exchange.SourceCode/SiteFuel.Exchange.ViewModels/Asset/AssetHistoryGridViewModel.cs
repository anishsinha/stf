using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetHistoryGridViewModel : StatusViewModel
    {
        public AssetHistoryGridViewModel()
        {
        }

        public AssetHistoryGridViewModel(Status status)
            : base(status)
        {
            AssetDropHistory = new List<AssetDropHistoryViewModel>();
        }

        public int Id { get; set; }

        public int JobId { get; set; }

        public string JobName { get; set; }

        public string AssignedDate { get; set; }

        public string RemovedDate { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int TotalDrops { get; set; }

        public string TotalFuel { get; set; }

        public string TotalCost { get; set; }

        public List<AssetDropHistoryViewModel> AssetDropHistory { get; set; }
    }
}
