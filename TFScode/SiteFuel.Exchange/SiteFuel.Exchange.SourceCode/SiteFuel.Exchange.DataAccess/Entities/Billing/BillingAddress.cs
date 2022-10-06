namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BillingAddress
    {
        public int Id { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [StringLength(512)]
        public string AddressLine2 { get; set; }

        [StringLength(512)]
        public string AddressLine3 { get; set; }

        [StringLength(128)]
        public string City { get; set; }

        [StringLength(32)]
        public string ZipCode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [StringLength(128)]
        public string CountryName { get; set; }

        [StringLength(128)]
        public string StateName { get; set; }

        [StringLength(128)]
        public string County { get; set; }

        public int PhoneTypeId { get; set; }

        [StringLength(16)]
        public string PhoneNumber { get; set; }

        public int CompanyId { get; set; }

        public bool IsDefault { get; set; } 

        public bool IsActive { get; set; }

        public int? StateId { get; set; }

        public int? CountryId { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("PhoneTypeId")] 
        public virtual MstPhoneType MstPhoneType { get; set; }
    }
}
