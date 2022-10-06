using System.IO;

namespace SiteFuel.Exchange.ViewModels
{
    public class TankVolumeAndUllageInputModel
    {
        public string TankMakeModelId { get; set; }

        public int ScaleMeasurement { get; set; }

        public decimal DipValue { get; set; }

        public decimal FuelCapacity { get; set; }

        public decimal? MaxFill { get; set; }

        public decimal? MaxFillPercent { get; set; }
        public int JobId { get; set; }
    }

    public class DropQuantityByPrePostDipRequestModel
    {
        public int TankId { get; set; }

        public int ScaleMeasurement { get; set; }

        public decimal PreDipValue { get; set; }

        public decimal PostDipValue { get; set; }
        public int JobId { get; set; }
        public int JobxAssetId { get; set; }
    }

    public class DropQuantityByPrePostDipResponseModel : StatusViewModel
    {
        public decimal DropQuantity { get; set; }
        public int JobxAssetId { get; set; }
    }

}
