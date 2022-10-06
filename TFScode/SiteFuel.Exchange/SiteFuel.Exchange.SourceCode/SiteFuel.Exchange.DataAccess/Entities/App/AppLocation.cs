namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AppLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppLocation()
        {
            
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(256)]
        public string FCMAppId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int AppTypeId { get; set; }
		
        public System.DateTimeOffset UpdatedDate { get; set; }

        public bool IsUserLogout { get; set; }

        public int? OrderId { get; set; }

        public virtual MstAppType MstAppType { get; set; }

        public virtual User User { get; set; }
		
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public int? DeliveryScheduleId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public int? StatusId { get; set; }

        [StringLength(256)]
        public string ExternalRefID { get; set; }

        [ForeignKey("StatusId")]
        public virtual MstEnrouteDeliveryStatus MstEnrouteDeliveryStatus { get; set; }
    }
}
