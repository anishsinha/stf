namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DeliveryScheduleXDriver
    {
        public int Id { get; set; }

        public int DeliveryScheduleId { get; set; }

        public int DriverId { get; set; }

        public int AssignedBy { get; set; }

        public DateTimeOffset AssignedDate { get; set; }
		
        public Nullable<int> RemovedBy { get; set; }
		
        public Nullable<System.DateTimeOffset> RemovedDate { get; set; }
		
        public bool IsActive { get; set; }

        public virtual DeliverySchedule DeliverySchedule { get; set; }

        public virtual User User { get; set; }
    }
}
