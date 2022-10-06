namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderXDriver
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int DriverId { get; set; }

        public int AssignedBy { get; set; }

        public DateTimeOffset AssignedDate { get; set; }

        public Nullable<int> RemovedBy { get; set; }

        public Nullable<DateTimeOffset> RemovedDate { get; set; }

        public bool IsActive { get; set; }

        public virtual Order Order { get; set; }

        public virtual User User { get; set; }
    }
}
