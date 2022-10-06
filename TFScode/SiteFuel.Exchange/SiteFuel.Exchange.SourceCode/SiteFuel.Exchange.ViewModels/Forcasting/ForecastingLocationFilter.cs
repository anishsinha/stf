namespace SiteFuel.Exchange.ViewModels
{
    public class ForecastingLocationFilter
    {
        public string RegionId { get; set; }
        public string CustomerIds { get; set; }
        public bool IsShowCarrierManaged { get; set; }
        public string Carriers { get; set; }
        public string InventoryCaptureType { get; set; }
        public bool IsRateOfConsumption { get; set; }
    }
}
