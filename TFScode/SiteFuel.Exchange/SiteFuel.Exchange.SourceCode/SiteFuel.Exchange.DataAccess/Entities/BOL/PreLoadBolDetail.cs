namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class PreLoadBolDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PreLoadBolDetail()
        {
            
        }
        public int Id { get; set; }

        public decimal? GrossQuantity { get; set; }

        public decimal? NetQuantity { get; set; }

        [StringLength(256)]
        public string BolNumber { get; set; }

        [StringLength(256)]
        public string Carrier { get; set; }

        public int? ImageId { get; set; }

        public DateTimeOffset PickupDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }

        public DateTimeOffset? LiftDate { get; set; }

        public TimeSpan? BolCreationTime { get; set; }

        [StringLength(256)]
        public string BadgeNumber { get; set; }

        [StringLength(64)]
        public string LiftTicketNumber { get; set; }
        public TimeSpan? LiftArrivalTime { get; set; }
        public TimeSpan? LiftStartTime { get; set; }
        public TimeSpan? LiftEndTime { get; set; }
        public decimal? LiftQuantity { get; set; }

        [StringLength(256)]
        public string TraceId { get; set; }

        [StringLength(256)]
        public string Address { get; set; }

        [StringLength(128)]
        public string City { get; set; }

        [StringLength(32)]
        public string StateCode { get; set; }

        public int? StateId { get; set; }

        [StringLength(32)]
        public string ZipCode { get; set; }

        [StringLength(32)]
        public string CountryCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        [StringLength(64)]
        public string CountyName { get; set; }

        [StringLength(256)]
        public string SiteName { get; set; }

        public PickupLocationType PickupLocation { get; set; }
        public int FuelTypeId { get; set; }
        public int? TerminalId { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public decimal PricePerGallon { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public decimal RackPrice { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int? TrackableScheduleId { get; set; }
        [StringLength(256)]
        public string TerminalName { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }

        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        [ForeignKey("FuelTypeId")]
        public virtual MstProduct MstProduct { get; set; }

        [ForeignKey("TrackableScheduleId")]
        public virtual DeliveryScheduleXTrackableSchedule TrackableSchedule { get; set; }

        [ForeignKey("DeliveryScheduleId")]
        public virtual DeliverySchedule DeliverySchedule { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User UpdatedByUser { get; set; }

        public bool IsPickupBOLRetain { get; set; } = false;
        public string TrailerRetainInfo { get; set; } = string.Empty;
    }
}
