using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class TankInventoryResponseModel : ApiStatusViewModel
    {
    }

    public class TankInventoryDataCaptureResponseModel
    {
        public string TankName { get; set; }
        [JsonProperty(PropertyName = "Location")]
        public string SiteId { get; set; }
        [JsonProperty(PropertyName = "TankID")]
        public string TankId { get; set; }
        [JsonIgnore]
        public string StorageId { get; set; }
        public string Customer { get; set; }
        [JsonIgnore]
        public string ProductType { get; set; }
        [JsonProperty(PropertyName = "VolumeUoM")]
        public string UoM { get; set; }
        public string TankCapacity { get; set; }
        [JsonProperty(PropertyName = "TankMaxFill")]
        public string MaxFill { get; set; }
        [JsonProperty(PropertyName = "TankMinFill")]
        public string MinFill { get; set; }
        [JsonIgnore]
        public string StartUllage { get; set; }
        public string Ullage { get; set; }
        [JsonProperty(PropertyName = "PreviousDaySale")]
        public string PrevSale { get; set; }
        public string Inventory { get; set; }
        [JsonProperty(PropertyName = "Trailing7daysAvg")]
        public string AvgSale { get; set; }
        [JsonProperty(PropertyName = "WeekAgoSale")]
        public string WeekAgoSale { get; set; }
        public string DaysRemaining { get; set; }
        public string LastDeliveredQuantity { get; set; }
        [JsonProperty(PropertyName = "LastInventoryReadingDateAndTime")]
        public string LastReadingTime { get; set; }
        [JsonProperty(PropertyName = "LastDeliveredOn")]
        public string LastDeliveryDate { get; set; }
        [JsonIgnore]
        public int JobId { get; set; }
        [JsonIgnore]
        public int ProductTypeId { get; set; }
        public string WaterLevel { get; set; }

        
    }

    public class UnAthorizedInventoryData : ApiStatusViewModel
    {
        public List<TankInventoryDataCaptureResponseModel> Data = new List<TankInventoryDataCaptureResponseModel>();
        public List<DropdownDisplayItem> SupplierList { get; set; } = new List<DropdownDisplayItem>();
        public int SelectedSupplierId { get; set; }
        public string CompanyToken { get; set; }
    }

    public class CarrierInfoForUnAthorizedInventoryDataViewModel
    {
        public int CarrierCompanyId { get; set; }
        public string CarrierAdminEmail { get; set; }
        public int SupplierCompanyId { get; set; }
    }
}
