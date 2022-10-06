namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public class ForecastingTankInformationModel
    {
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public float DaysLeft { get; set; }
        public decimal EstimatedCurrentInventory { get; set; }
        public TankInformationModel TankInformation { get; set; } = new TankInformationModel();
    }
    public class TankInformationModel
    {
        public string Date { get; set; }
        public int BandNumber { get; set; }
        public int SaleTankId { get; set; }
        public decimal TotalSale { get; set; }
        public decimal AverageSale { get; set; }
    }
    public class UspForecastingTankInfomation
    {
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string SiteID { get; set; }
    }
}
