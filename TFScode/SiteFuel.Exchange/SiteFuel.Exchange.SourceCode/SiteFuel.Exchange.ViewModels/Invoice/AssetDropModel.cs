using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetDropModel : BaseViewModel
    {
        public AssetDropModel()
        {
            IsActive = true;
        }
        public string AssetName { get; set; }
        public int Id { get; set; }
        public int JobXAssetId { get; set; }
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public decimal MeterStartReading { get; set; }
        public decimal MeterEndReading { get; set; }
        public decimal? DropGallons { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public int DroppedBy { get; set; }
        public int? ImageId { get; set; }
        public string SubcontractorName { get; set; }
        public int? SubcontractorId { get; set; }
        public string ContractNumber { get; set; }
        public int DropStatus { get; set; }
        public bool IsNewAsset { get; set; }
        public ImageViewModel Image { get; set; }

        public decimal? PreDip { get; set; }
        public decimal? PostDip { get; set; }
        public TankScaleMeasurement TankScaleMeasurement { get; set; }

        public decimal? Gravity { get; set; }

    }
}
