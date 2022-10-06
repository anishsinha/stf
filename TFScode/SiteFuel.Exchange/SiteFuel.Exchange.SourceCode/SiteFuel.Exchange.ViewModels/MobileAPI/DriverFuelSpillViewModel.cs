using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverFuelSpillViewModel : StatusViewModel
    {
        public DriverFuelSpillViewModel()
        {
            InstanceInitialize();
        }

        public DriverFuelSpillViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }
        private void InstanceInitialize()
        {
            SpillImages = new List<ImageViewModel>();
        }

        public int Id { get; set; }

        public DateTimeOffset SpillDate { get; set; }

        public int SpilledBy { get; set; }

        public string Notes { get; set; }

        public int AssetId { get; set; }

        public int OrderId { get; set; }

        public int? InvoiceId { get; set; }

        public List<ImageViewModel> SpillImages { get; set; }
    }
}
