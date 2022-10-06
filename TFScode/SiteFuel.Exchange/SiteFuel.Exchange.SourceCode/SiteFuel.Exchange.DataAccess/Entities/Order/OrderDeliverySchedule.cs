namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderVersionXDeliverySchedule
    {
        public int Id { get; set; }

        public int? DeliveryRequestId { get; set; }

        public int OrderId { get; set; }

        public int Version { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string AdditionalNotes { get; set; }

        public bool IsActive { get; set; }

        public virtual Order Order { get; set; }

        public virtual User User { get; set; }

        public virtual DeliverySchedule DeliverySchedule { get; set; }
    }
}
