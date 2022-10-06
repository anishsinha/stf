namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EnrouteDeliveryHistory")]
    public partial class EnrouteDeliveryHistory
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public int? DeliveryScheduleId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public int UserId { get; set; }

        public int StatusId { get; set; }

        public DateTimeOffset EnrouteDate { get; set; }

        public virtual MstEnrouteDeliveryStatus MstEnrouteDeliveryStatus { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public virtual User User { get; set; }
    }
}
