namespace SiteFuel.Exchange.ViewModels.Forcasting
{
    public class ForecastingInventoryViewModel
    {
        public string TankName { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string SiteId { get; set; }
        public float FuelCapacity { get; set; }
        public float InventoryLevel { get; set; }
        public string InventoryLevelQty { get; set; }
        public string Ullage { get; set; }
        public string PrevInventoryReading { get; set; }
        public float SafetyStock { get; set; }
        public string SafetyStockQty { get; set; }
        public float RunOutLevel { get; set; }
        public string RunOutLevelQty { get; set; }
        public float PhysicalPumpStop { get; set; }
        public string PhysicalPumpStopQty { get; set; }
    }
}
