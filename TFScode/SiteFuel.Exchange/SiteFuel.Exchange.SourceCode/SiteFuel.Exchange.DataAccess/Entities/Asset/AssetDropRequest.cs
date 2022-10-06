namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AssetDropRequest
    {
        public int Id { get; set; }

        public int AssetId { get; set; }

        [Required]
        [StringLength(128)]
        public string AssetExternalId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public decimal QuantityRequired { get; set; }

        public int FuelRequestId { get; set; }

        public bool IsThisRequestClosed { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual FuelRequest FuelRequest { get; set; }
    }
}
