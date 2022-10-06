namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class InvoiceDispatchLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvoiceDispatchLocation()
        {

        }

        public int Id { get; set; }

        public int LocationType { get; set; }

        public int OrderId { get; set; }

        public int InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

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

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        [StringLength(64)]
        public string CountyName { get; set; }

        [StringLength(256)]
        public string SiteName { get; set; }

        public PickupLocationType PickupLocation { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }
    }
}
