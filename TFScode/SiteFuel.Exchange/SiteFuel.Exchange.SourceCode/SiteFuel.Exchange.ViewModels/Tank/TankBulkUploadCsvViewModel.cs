using CsvHelper.Configuration.Attributes;

namespace SiteFuel.Exchange.ViewModels
{
    public class TankBulkUploadCsvViewModel
    {
        [Name("Customer*")]
        public string Customer { get; set; }
        
        [Name("SiteName*")]
        public string SiteName { get; set; }

        [Name("TankName*")]
        public string TankName { get; set; }

        [Name("ProductType*")]
        public string ProductType { get; set; }

        public string FuelType { get; set; }

        [Name("FuelCapacity*")]
        public string FuelCapacity { get; set; }

        [Name("StorageTypeID")]
        public string StorageTypeID { get; set; }

        [Name("StorageID")]
        public string StorageID { get; set; }

        public string TankType { get; set; }

        [Name("Inventory Capture Method")]
        public string DipTestMethod { get; set; }

        public string TankNumber { get; set; }

        public string TankMake { get; set; }

        public string TankModel { get; set; }

        [Name("FillType*")]
        public string FillType { get; set; }

        [Name("MaxFill*")]
        public string MaxFill { get; set; }

        [Name("MinFill(MustGoLevel)*")]
        public string MinFill { get; set; }

        [Name("ShouldGoLevel*")]
        public string RunOutLevel { get; set; }

        [Name("Re-Order(CouldGo)level*")]
        public string ReOrderLevel { get; set; }

        public string PhysicalPumpStop { get; set; }

        public string Manufacture { get; set; }

        public string Manifolded { get; set; }

        public string Construction { get; set; }

        public string NotificationUponUsageSwing { get; set; }

        public string NotificationUponUsageSwingValue { get; set; }

        public string NotificationUponInventorySwing { get; set; }

        public string NotificationUponInventorySwingValue { get; set; }

        public string TankAcceptsDeliveryDays { get; set; }

        public string PedigreeAssetDBID { get; set; }
        public string SkyBitzRTUID { get; set; }
        public string ExternalTankID { get; set; }
        public string WaterLevelThreshold { get; set; }
        public string IPAddress { get; set; }
        public string Port { get; set; }
    }
}
