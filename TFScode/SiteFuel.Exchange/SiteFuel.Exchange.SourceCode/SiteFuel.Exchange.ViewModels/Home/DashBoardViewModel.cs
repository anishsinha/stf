using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardViewModel : StatusViewModel
    {
        public DashboardViewModel()
        {
            InstanceInitialize();
        }

        public DashboardViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            RecentFuelRequests = new List<FuelRequestGridViewModel>();
            JobLocations = new List<MapViewModel>();
            Customers = new List<DropdownDisplayItem>();
            CustomerOrders = new List<int>();
            NextSchedulesOfJob = new List<DeliveryScheduleForJobViewModel>();
            Drivers = new List<DropdownDisplayItem>();
            Country = new CountryViewModel();
            CompanyGroup = new CompanySubGroupViewModel();
            TileSetting = new UserPageSettingViewModel();
        }

        public bool IsTaxExemptDisplayed { get; set; }

        public int OpenFuelRequestCount { get; set; }

        public int DraftFuelRequestCount { get; set; }

        public int ExpiredFuelRequestCount { get; set; }

        public int CancelledFuelRequestCount { get; set; }

        public int AcceptedFuelRequestCount { get; set; }

        public int OpenOrderCount { get; set; }

        public int CanceledOrderCount { get; set; }

        public int ClosedOrderCount { get; set; }

        public int ReceivedInvoiceCount { get; set; }

        public int NotApprovedInvoiceCount { get; set; }

        public int UnconfirmedInvoiceCount { get; set; }

        public int UnderBudgetJobsCount { get; set; }

        public int NoBudgetJobsCount { get; set; }

        public int OverBudgetJobsCount { get; set; }

        public decimal TotalBudget { get; set; }

        public decimal TotalHedgeDroppedAmount { get; set; }

        public decimal TotalSpotDroppedAmount { get; set; }

        public int BudgetAlertPercentage { get; set; }

        public int TotalAssetCount { get; set; }

        public int TotalJobsCount { get; set; }

        public int AssignedAssetCount { get; set; }

        public int SelectedJobId { get; set; }

        public int TotalFuelRequestCount { get; set; }

        public int TotalOrderCount { get; set; }

        public int TotalInvoiceCount { get; set; }

        public int TotalGallons { get; set; }

        public int TotalDelivered { get; set; }

        public decimal DueAmount { get; set; }

        public int CustomerCompanyId { get; set; }

        public int SelectedDriver { get; set; }

        public IList<FuelRequestGridViewModel> RecentFuelRequests { get; set; }

        public IList<MapViewModel> JobLocations { get; set; }

        public List<DropdownDisplayItem> Customers { get; set; }

        public List<int> CustomerOrders { get; set; }

        public string SelectedJobNextDeliverySchedule { get; set; }

        public string SelectedJobPoNumber { get; set; }

        public bool IsCompanyGroupAvailable { get; set; }

        public bool IsCalendarTileClosed { get; set; }

        public bool IsCalendarTileCollapsed { get; set; }

        public bool IsDispatchTileClosed { get; set; }

        public bool IsDispatchTileCollapsed { get; set; }

        public bool IsFRTileClosed { get; set; }

        public bool IsFRTileCollapsed { get; set; }

        public bool IsFRQuoteTileClosed { get; set; }

        public bool IsFRQuoteTileCollapsed { get; set; }

        public bool IsOrderTileClosed { get; set; }

        public bool IsOrderTileCollapsed { get; set; }

        public bool IsInvoiceTileClosed { get; set; }

        public bool IsInvoiceTileCollapsed { get; set; }

        public bool IsDDTTileClosed { get; set; }

        public bool IsDDTTileCollapsed { get; set; }

        public bool IsGFCTileClosed { get; set; }

        public bool IsGFCTileCollapsed { get; set; }

        public bool IsGallonStatTileClosed { get; set; }

        public bool IsGallonStatTileCollapsed { get; set; }

        public bool IsYourBusinessTileClosed { get; set; }

        public bool IsYourBusinessTileCollapsed { get; set; }

        public bool IsDropAvgTileClosed { get; set; }

        public bool IsDropAvgTileCollapsed { get; set; }

        public bool IsDeliveriesTileClosed { get; set; }

        public bool IsDeliveriesTileCollapsed { get; set; }

        public bool IsJobAvgTileClosed { get; set; }

        public bool IsJobAvgTileCollapsed { get; set; }

        public bool IsDeliveryRequestTileClosed { get; set; }

        public bool IsDeliveryRequestTileCollapsed { get; set; }

        public int SelectedJobOrderId { get; set; }

        public List<DeliveryScheduleForJobViewModel> NextSchedulesOfJob { get; set; }

        public List<DropdownDisplayItem> Drivers { get; set; }

        public CountryViewModel Country { get; set; }

        public CompanySubGroupViewModel CompanyGroup { get; set; }

        public UserPageSettingViewModel TileSetting { get; set; }

        public InventoryDataViewModel InventoryFilter { get; set; } = new InventoryDataViewModel();
    }    
}
