using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelRequestSummaryViewModel : StatusViewModel
    {
        public FuelRequestSummaryViewModel()
        {
            InstanceInitialize();
        }

        public FuelRequestSummaryViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            FuelList = new List<FuelRequestGridViewModel>();
        }

        public int UserId { get; set; }

        public List<FuelRequestGridViewModel> FuelList { get; set; }
    }
}
