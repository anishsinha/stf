using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.FreightModels
{
    public class SalesDataRequestModel
    {
        public string RegionId { get; set; }
        public List<CustomerJobsModel> Jobs { get; set; }
        public int Priority { get; set; }
        public int SelectedTab { get; set; }
        public int CompanyId { get; set; }
        public bool isFromExchangeApiForDataExpose { get; set; }

    }
    public class CustomerJobsModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int JobId { get; set; }
        public string JobAddress { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string TimeZoneName { get; set; }
        public string LocationName { get; set; }
        public JobLocationTypes LocationTypeId { get; set; }
        public int LocationManagedType { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
    }

    public class SalesDataResponseModel : StatusModel
    {
        public List<SalesDataModel> SalesData { get; set; } = new List<SalesDataModel>();
    }

    public class SalesInventoryResponseModel : StatusModel
    {
        public List<SalesInventoryData> SalesData { get; set; } = new List<SalesInventoryData>();
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
        public decimal InventoryData { get; set; }
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
        public DateTimeOffset EndUllageDate { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
        public string InventoryDataCaptureTypeName { get; set; }
        public decimal? TankCapacity { get; set; }
        public int LocationManagedType { get; set; }
        public decimal WaterLevel { get; set; }
        public decimal PrevSaleData { get; set; }
        public decimal UllageData { get; set; }
        public decimal WeekAgoSaleData { get; set; }
        public int UOM { get; set; }
    }

    public class SalesInventoryData
    {      
        public string SiteId { get; set; }     
        public string TankId { get; set; }
        public string StorageId { get; set; }      
        public decimal InventoryData { get; set; }
        public string Ullage { get; set; }
    }

    public class SalesInventoryWaterLevel
    {
        public string SiteId { get; set; }        
        public string StorageId { get; set; }
        public string TankId { get; set; }
        public Single WaterNetLevel { get; set; }
    }

    public class SalesGraphRespDataModel : StatusModel
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

    public enum SelctedSalesTab
    {
        Priority =1,
        Tanks,
        Location
    }
    public class SalesDataDaysRemainingModel
    {
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string DaysRemaining { get; set; }
       
    }
    public class TankRounOutSaleData
    {
      
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string DaysRemaining { get; set; }
        public decimal InventoryData { get; set; }
    }
}
