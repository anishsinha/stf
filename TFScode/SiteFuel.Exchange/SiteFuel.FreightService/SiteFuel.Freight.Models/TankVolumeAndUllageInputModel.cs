using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class TankVolumeAndUllageInputModel
    {
        public string TankMakeModelId { get; set; }

        public int ScaleMeasurement { get; set; }

        public decimal DipValue { get; set; }

        public decimal FuelCapacity { get; set; }

        public decimal MaxFill { get; set; }

        public decimal MaxFillPercent { get; set; }
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

    public class DropQuantityByPrePostDipResponseModel : StatusModel
    {
        public decimal DropQuantity { get; set; }
        public int JobxAssetId { get; set; }
    }
}
