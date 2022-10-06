namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelDispatchLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FuelDispatchLocation()
        {

        }

        public int Id { get; set; }

        public int LocationType { get; set; }

        public int? DeliveryGroupId { get; set; }

        public int? OrderId { get; set; }

        public int? DeliveryScheduleId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public int? TerminalId { get; set; }

        public int? BulkPlantId { get; set; }

        public bool IsFuturePickUp { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("TerminalId")]
        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        [ForeignKey("BulkPlantId")]
        public virtual BulkPlantLocation BulkPlantLocation { get; set; }

        [Required]
        public string Address { get; set; }

        [StringLength(512)]
        public string AddressLine2 { get; set; }

        [StringLength(512)]
        public string AddressLine3 { get; set; }

        [Required]
        [StringLength(128)]
        public string City { get; set; }

        [Required]
        [StringLength(32)]
        public string StateCode { get; set; }

        public int? StateId { get; set; }

        [Required]
        [StringLength(32)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(32)]
        public string CountryCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
            
        public DateTimeOffset CreatedDate { get; set; }

        [Required]
        [StringLength(64)]
        public string CountyName { get; set; }
        
        [StringLength(256)]
        public string TimeZoneName { get; set; }

        public DropAddressStatus DropStatus { get; set; }

        public bool IsJobLocation { get; set; }

        public Currency Currency { get; set; }

        [StringLength(256)]
        public string SiteName { get; set; }

        public int? ParentId { get; set; }

        public bool IsSkipped { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }

        [ForeignKey("DeliveryScheduleId")]
        public virtual DeliverySchedule DeliverySchedule { get; set; }

        [ForeignKey("TrackableScheduleId")]
        public virtual DeliveryScheduleXTrackableSchedule TrackableSchedule { get; set; }

        [ForeignKey("ParentId")] 
        public virtual FuelDispatchLocation FuelDispatchLocation1 { get; set; }

        [ForeignKey("DeliveryGroupId")]
        public virtual DeliveryGroup DeliveryGroup { get; set; }
    }
}
