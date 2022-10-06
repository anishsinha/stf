using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardFuelRequestViewModel : StatusViewModel
    {
        public DashboardFuelRequestViewModel()
        {
            InstanceInitialize();
        }

        public DashboardFuelRequestViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            
        }

        public int TotalFuelRequestCount { get; set; }

        public int ThirdPartyFRCount { get; set; }

        public int OpenFuelRequestCount { get; set; }

        public int DraftFuelRequestCount { get; set; }

        public int ExpiredFuelRequestCount { get; set; }

        public int CancelledFuelRequestCount { get; set; }

        public int AcceptedFuelRequestCount { get; set; }

        public int BrokeredFuelRequestRequestCount { get; set; }

        public List<FuelRequestGridViewModel> RecentFuelRequests { get; set; }

        public int SelectedJobId { get; set; }

        public int AboutToExpireCount { get; set; }

        public bool IsDashboardSummaryRequest { get; set; }

        public string GroupIds { get; set; }

        public FuelRequestFilterType FuelRequestStatus { get; set; }

        public FuelRequestType FuelRequestType { get; set; }

        public bool IsCollapsed { get; set; }
    }
}
