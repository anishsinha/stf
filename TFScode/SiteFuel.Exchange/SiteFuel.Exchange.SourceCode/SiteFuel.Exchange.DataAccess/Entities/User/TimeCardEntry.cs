namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TimeCardEntry
    {
        public int Id { get; set; }

        public int DriverId { get; set; }

        public int ActionId { get; set; }

        public Nullable<System.DateTimeOffset> ActionStartDate { get; set; }

        public Nullable<System.DateTimeOffset> ActionEndDate { get; set; }

        public System.DateTimeOffset CreatedDate { get; set; }

        public int ActionGroup { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        [StringLength(256)]
        public string UserLocation { get; set; }

        [StringLength(256)]
        public string TimeZoneName { get; set; }

        public virtual MstTimeCardAction MstTimeCardAction { get; set; }

        public virtual User User { get; set; }
    }
}
