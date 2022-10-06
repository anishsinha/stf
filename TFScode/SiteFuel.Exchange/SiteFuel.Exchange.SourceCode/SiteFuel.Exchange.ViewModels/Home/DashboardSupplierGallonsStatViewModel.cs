using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardSupplierGallonsStatViewModel : StatusViewModel
    {
        public DashboardSupplierGallonsStatViewModel()
        {
            InstanceInitialize();
        }

        public DashboardSupplierGallonsStatViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            FuelTypes = new List<DropdownDisplayItem>();
        }

        public decimal TotalRequestedGallons { get; set; }
        public decimal MissedGallons { get; set; }
        public decimal ExpiredGallons { get; set; }
        public decimal DeliveredGallons { get; set; }
        public decimal AcceptedGallons { get; set; }
        public int FuelTypeId { get; set; }
        public int FuelTypeName { get; set; }
        public List<DropdownDisplayItem> FuelTypes { get; set; }

        public int AcceptedFrCount { get; set; }
        public int MissedFrCount { get; set; }
        public int ExpiredFrCount { get; set; }
        public int DeclinedFrCount { get; set; }
        public int CounterOfferCount { get; set; }

        public decimal BusinessYouWon { get; set; }
        public decimal BusinessYouMissed { get; set; }
        public decimal BusinessInYourArea { get; set; }

        public int TotalFrCount { get; set; }

        public int TotalQrCount { get; set; }
        public int OpenQrCount { get; set; }
        public int MissedQrCount { get; set; }
        public int AcceptedQrCount { get; set; }
        public int DeclinedQrCount { get; set; }

        public bool IsGallonStatTileCollapsed { get; set; }

        public bool IsDropAvgTileCollapsed { get; set; }
    }
}
