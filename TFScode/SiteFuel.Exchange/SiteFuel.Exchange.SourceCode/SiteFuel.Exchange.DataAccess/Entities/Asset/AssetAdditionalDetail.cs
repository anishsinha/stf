namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AssetAdditionalDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AssetId { get; set; }

        [StringLength(256)]
        public string Make { get; set; }

        [StringLength(256)]
        public string Model { get; set; }

        [StringLength(256)]
        public string Year { get; set; }

        [StringLength(256)]
        public string Color { get; set; }

        [StringLength(256)]
        public string TelematicsProvider { get; set; }

        public Nullable<int> LicensePlateStateId { get; set; }

        [StringLength(256)]
        public string LicensePlate { get; set; }

        [StringLength(256)]
        public string AssetClass { get; set; }

        [StringLength(256)]
        public string Vendor { get; set; }

        [StringLength(256)]
        public string VehicleId { get; set; }

        public string Description { get; set; }

        public Nullable<decimal> FuelCapacity { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public int? TankType { get; set; }

        public decimal? ThresholdDeliveryRequest { get; set; }

        public decimal? MinFill { get; set; }

        public decimal? MaxFill { get; set; }

        public int? DipTestMethod { get; set; }

        [StringLength(500)]
        public string PedigreeAssetDBId { get; set; }

        public int? FillType { get; set; }

        [StringLength(500)]
        public string SkyBitzRTUID { get; set; }

        
        [StringLength(500)]
        public string ExternalTankId { get; set; } //Indicates tank id added for Third party datasource e.g. insight 360.

        [StringLength(500)]
        public string VeederRootIPAddress { get; set; }

        [StringLength(500)]
        public string Port { get; set; } //used in VeederRoot Tank Inventory datacapture Method

        public decimal? WaterLevel { get; set; } = 0;

        [StringLength(256)]
        public string IMONumber { get; set; }

        [StringLength(256)]
        public string Flag { get; set; }

        public System.DateTimeOffset UpdatedDate { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual MstState MstState { get; set; }

        
    }
}
