namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class InvoiceFtlDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvoiceFtlDetail()
        {
            InvoiceXBolDetails = new HashSet<InvoiceXBolDetail>();
            InvoiceTierPricingDetails = new HashSet<InvoiceTierPricingDetail>();
        }
        public int Id { get; set; }

        public decimal? GrossQuantity { get; set; }

        public decimal? NetQuantity { get; set; }

        [StringLength(256)]
        public string BolNumber { get; set; }

        [StringLength(256)]
        public string Carrier { get; set; }

        public int? ImageId { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }

        public DateTimeOffset? LiftDate { get; set; }

        public TimeSpan? BolCreationTime { get; set; }

        [StringLength(64)]
        public string LiftTicketNumber { get; set; }
        public TimeSpan? LiftArrivalTime { get; set; }
        public TimeSpan? LiftStartTime { get; set; }
        public TimeSpan? LiftEndTime { get; set; }
        public decimal? LiftQuantity { get; set; }

        [StringLength(512)]
        public string Address { get; set; }

        [StringLength(512)]
        public string AddressLine2 { get; set; }

        [StringLength(512)]
        public string AddressLine3 { get; set; }

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
        [StringLength(256)]
        public string TerminalName { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }

        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        [ForeignKey("FuelTypeId")]
        public virtual MstProduct MstProduct { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceXBolDetail> InvoiceXBolDetails { get; set; }

        [StringLength(256)]
        public string BadgeNumber { get; set; }

        public bool IsLiftFileValidated { get; set; }

        public int? changedFuelTypeId { get; set; }
        
        public bool IsBOLEditedForLfv { get; set; }

        public int? ReasonCodeId { get; set; }

        public string Notes { get; set; }

        public string RecordHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceTierPricingDetail> InvoiceTierPricingDetails { get; set; }

        public EbolMatchStatus EbolMatchStatus { get; set; } = EbolMatchStatus.NoMatch;

        public decimal? DeliveredQuantity { get; set; }
    }
}
