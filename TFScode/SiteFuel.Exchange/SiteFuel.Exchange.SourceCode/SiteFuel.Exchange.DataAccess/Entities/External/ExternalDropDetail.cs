namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExternalDropDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExternalDropDetail()
        {
            
        }

        public int Id { get; set; }

        public DateTimeOffset DropStartDate { get; set; }

        public DateTimeOffset DropEndDate { get; set; }

        public int OrderId { get; set; }

        public Nullable<int> TrackableScheduleId { get; set; }

        public decimal DropStartLatitude { get; set; }

        public decimal DropStartLongitude { get; set; }

        public decimal DropEndLatitude { get; set; }

        public decimal DropEndLongitude { get; set; }

        public int UserId { get; set; }

        public DateTimeOffset DropDate { get; set; }

        public bool IsActive { get; set; }

        public virtual Order Order { get; set; }

        public virtual DeliveryScheduleXTrackableSchedule DeliveryScheduleXTrackableSchedule { get; set; }
    }
}
