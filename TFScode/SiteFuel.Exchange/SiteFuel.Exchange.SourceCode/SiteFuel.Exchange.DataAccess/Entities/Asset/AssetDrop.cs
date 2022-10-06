namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AssetDrop
    {
        public int Id { get; set; }

        public int JobXAssetId { get; set; }

        public int OrderId { get; set; }

        public Nullable<int> InvoiceId { get; set; }

        public decimal MeterStartReading { get; set; }

        public decimal MeterEndReading { get; set; }

        public decimal DroppedGallons { get; set; }

        public DateTimeOffset DropStartDate { get; set; }

        public DateTimeOffset DropEndDate { get; set; }

        public int DroppedBy { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int? ImageId { get; set; }

        public string SubcontractorName { get; set; }

        public int? SubcontractorId { get; set; }

        public string ContractNumber { get; set; }

        public int DropStatus { get; set; }

        public bool IsNewAsset { get; set; }
        public decimal? PreDip { get; set; }
        public decimal? PostDip { get; set; }
        public decimal? Gravity { get; set; }
        public decimal? ConvertedQuantity { get; set; }
        public virtual Invoice Invoice { get; set; }

        public virtual JobXAsset JobXAsset { get; set; }

        public virtual Order Order { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }
        public TankScaleMeasurement TankScaleMeasurement { get; set; }
    }
}
