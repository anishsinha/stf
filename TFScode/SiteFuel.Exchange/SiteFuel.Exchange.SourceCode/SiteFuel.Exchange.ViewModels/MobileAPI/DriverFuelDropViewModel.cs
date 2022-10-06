using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverFuelDropViewModel : BaseViewModel
    {
        public DriverFuelDropViewModel()
        {
          
        }

        public int AssetDropId { get; set; }

        public int OrderId { get; set; }

        public int InvoiceId { get; set; }

        public RunningMeterMode RunningMeterMode { get; set; }

        public decimal PrimaryDrop { get; set; }

        public decimal PrimaryMeterStartReading { get; set; }

        public decimal PrimaryMeterEndReading { get; set; }

        public int PrimaryDropId { get; set; }

        public decimal SecondaryDrop { get; set; }

        public decimal SecondaryMeterStartReading { get; set; }

        public decimal SecondaryMeterEndReading { get; set; }

        public int SecondaryDropId { get; set; }

        public bool IsNoFuelNeeded { get; set; }

        public bool IsSpillOccurred { get; set; }

        public int SpillId { get; set; }
        public decimal? Gravity { get; set; }

        public int JobXAssetId { get; set; }

        public int DroppedBy { get; set; }

        public int DropStatus { get; set; }

        public string FuelType { get; set; }

        public string ProductType { get; set; }

        public bool IsNewAsset { get; set; }

        public decimal DroppedGallons { get; set; }

        public DateTimeOffset DropDate { get; set; }

        public ImageViewModel Image { get; set; }

        public List<AssetDropResponseViewModel> AssetDropDetail { get; set; }
    }
}
