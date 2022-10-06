namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BulkPlantLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BulkPlantLocation()
        {

        }

        public int Id { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

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

        public int StateId { get; set; }
        public int CountryId { get; set; }

        [Required]
        [StringLength(32)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(32)]
        public string CountryCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        [Required]
        [StringLength(64)]
        public string CountyName { get; set; }

        public int CreatedBy { get; set; }
        
        public int CompanyId { get; set; }
        
        public DateTimeOffset CreatedDate { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }
        
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        
        [ForeignKey("CountryId")]
        public virtual MstCountry MstCountry { get; set; }
    }
}
