namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExternalBroker
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string CompanyName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [StringLength(512)]
        public string AddressLine2 { get; set; }

        [StringLength(512)]
        public string AddressLine3 { get; set; }

        [Required]
        [StringLength(32)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(128)]
        public string City { get; set; }

        [Required]
        public int? StateId { get; set; }

        [Required]
        public int? CountryId { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public int? SupplierCompanyId { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }
}
