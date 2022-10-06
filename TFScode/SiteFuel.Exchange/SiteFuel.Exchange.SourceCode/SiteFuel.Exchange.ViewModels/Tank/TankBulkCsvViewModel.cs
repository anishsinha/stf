using FileHelpers;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class TankBulkCsvViewModel
    {
        [FieldQuoted] [FieldOptional]
        public string SiteName { get; set; }

        [FieldQuoted] [FieldOptional]
        public string TankName { get; set; }

        [FieldQuoted] [FieldOptional]
        public string ProductType { get; set; }
        [FieldQuoted]
        [FieldOptional]
        public string FuelType { get; set; }
        [FieldQuoted] [FieldOptional]
        public string FuelCapacity { get; set; }

        [FieldQuoted] [FieldOptional]
        public string StorageTypeID { get; set; }

        [FieldQuoted] [FieldOptional]
        public string StorageID { get; set; }

        [FieldQuoted] [FieldOptional]
        public string TankType { get; set; }

        [FieldQuoted] [FieldOptional][FieldCaption("Inventory Capture Method")]
        public string DipTestMethod { get; set; }

        [FieldQuoted] [FieldOptional]
        public string TankNumber { get; set; }

        [FieldQuoted] [FieldOptional]
        public string TankMake { get; set; }

        [FieldQuoted] [FieldOptional]
        public string TankModel { get; set; }

        [FieldQuoted] [FieldOptional]
        public string FillType { get; set; }

        [FieldQuoted] [FieldOptional]
        public string MaxFill { get; set; }

        [FieldQuoted] [FieldOptional]
        public string MinFill { get; set; }

        [FieldQuoted] [FieldOptional]
        public string RunOutLevel { get; set; }

        [FieldQuoted] [FieldOptional]
        public string ReOrderLevel { get; set; }

        [FieldQuoted] [FieldOptional]
        public string PhysicalPumpStop { get; set; }

        [FieldQuoted] [FieldOptional]
        public string Manufacture { get; set; }

        [FieldQuoted] [FieldOptional]
        public string Manifolded { get; set; }

        [FieldQuoted] [FieldOptional]
        public string Construction { get; set; }

        [FieldQuoted] [FieldOptional]
        public string NotificationUponUsageSwing { get; set; }

        [FieldQuoted] [FieldOptional]
        public string NotificationUponUsageSwingValue { get; set; }

        [FieldQuoted] [FieldOptional]
        public string NotificationUponInventorySwing { get; set; }

        [FieldQuoted] [FieldOptional]
        public string NotificationUponInventorySwingValue { get; set; }

        [FieldQuoted] [FieldOptional]
        public string TankAcceptsDeliveryDays { get; set; }

        [FieldQuoted] [FieldOptional]
        public string PedigreeAssetDBID { get; set; }
        [FieldQuoted][FieldOptional]
        public string SkyBitzRTUID { get; set; }
        [FieldQuoted] [FieldOptional]
        public string ExternalTankID { get; set; }
        [FieldQuoted] [FieldOptional]
        public string WaterLevelThreshold { get; set; }
        [FieldQuoted] [FieldOptional]
        public string IPAddress { get; set; }
        [FieldQuoted] [FieldOptional]
        public string Port { get; set; }
    }
}
