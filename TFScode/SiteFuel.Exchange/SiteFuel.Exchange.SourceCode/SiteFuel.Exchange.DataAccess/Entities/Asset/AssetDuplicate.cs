namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AssetDuplicate
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Make { get; set; }

        [StringLength(256)]
        public string Model { get; set; }

        [StringLength(256)]
        public string Year { get; set; }

        [StringLength(256)]
        public string Color { get; set; }

        [StringLength(256)]
        public string FuelType { get; set; }

        public Nullable<decimal> FuelCapacity { get; set; }

        [StringLength(256)]
        public string VehicleId { get; set; }

        [StringLength(256)]
        public string TelematicsProvider { get; set; }

        [StringLength(256)]
        public string LicensePlateState { get; set; }

        [StringLength(256)]
        public string LicensePlate { get; set; }

        [StringLength(256)]
        public string AssetClass { get; set; }

        [StringLength(256)]
        public string Vendor { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        [StringLength(256)]
        public string Subcontractor { get; set; }

        public DateTimeOffset DateAdded { get; set; }

        public virtual Company Company { get; set; }
    }
}
