using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class InventoryDataViewModel
    {
        public int CountryId { get; set; } = (int)Country.All;

        public List<int> SelectedStateIds { get; set; } = new List<int>();

        public List<string> SelectedRegionIds { get; set; } = new List<string>();

        public List<int> SelectedCustomerIds { get; set; } = new List<int>();

        public List<int> SelectedProductIds { get; set; } = new List<int>();

        public bool IsCarrierMnagedLocations { get; set; } = false;

        public List<CustomerJobsModel> CustomerJobs { get; set; }

        public int CompanyId { get; set; }
    }

    public class InventoryDataResponseModel : StatusViewModel
    {
        public InventoryDataModel InventoryData { get; set; } = new InventoryDataModel();
    }

    public class InventoryDataModel
    {
        public string TotalInventory { get; set; }
        public string TotalUllage { get; set; }
        public int ExistingDeliverySchedule { get; set; }
        public string AvgWeekAgoSale { get; set; }
        public string PrevDaySale { get; set; }
        public int DeliveryRequests { get; set; }
        public int OverfillTanks { get; set; }
        public int RunOutTanks { get; set; }
    }
}
