using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class SalesDataResponseModel : StatusViewModel
    {
        public List<SalesDataModel> SalesData { get; set; } = new List<SalesDataModel>();
    }

    public class SalesDataModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string SiteId { get; set; }
        public string Location { get; set; }
        public string LocationName { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string TankName { get; set; }
        public string AvgSale { get; set; }
        public string PrevSale { get; set; }
        public string WeekAgoSale { get; set; }
        public string Inventory { get; set; }
        public string Ullage { get; set; }
        public string LastDeliveryDate { get; set; }
        public string LastDeliveredQuantity { get; set; }
        public string DaysRemaining { get; set; }
        public string Status { get; set; }
        public DeliveryReqPriority Priority { get; set; }
        public int TfxJobId { get; set; }
        public int ProductTypeId { get; set; }
        public decimal? MinFillQuantity { get; set; }
        public decimal? MaxFillQuantity { get; set; }
        public string RegionId { get; set; }
        public string LastReadingTime { get; set; }
        public double TankInventoryDiffinHrs { get; set; }
        public List<string> Assets { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
        public string InventoryDataCaptureTypeName { get; set; }
        public decimal? TankCapacity { get; set; }
        public int LocationManagedType { get; set; }
        public decimal WaterLevel { get; set; }
        public int UOM { get; set; }
    }

    public class SalesGraphRespDataModel : StatusViewModel
    {
        public List<SalesGraphDataModel> Sales { get; set; } = new List<SalesGraphDataModel>();
    }
    public class SalesGraphDataModel
    {
        public int JobId { get; set; }
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string TankName { get; set; }
        public decimal Sale { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
