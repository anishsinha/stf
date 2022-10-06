namespace SiteFuel.Exchange.ViewModels
{
    public class AssetFuelRequestInputViewModel : ResponseViewModel
    {
        public int AssetId { get; set; }

        public string AssetExternalId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public decimal QuantityRequired { get; set; }

        public int FuelRequestId { get; set; }

        public bool IsThisRequestClosed { get; set; }

        public bool IsValidAsset { get; set; }
    }
}
