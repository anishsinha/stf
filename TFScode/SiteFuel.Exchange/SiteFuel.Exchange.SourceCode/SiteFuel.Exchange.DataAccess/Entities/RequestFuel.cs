namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RequestFuel")]
    public partial class RequestFuel
    {
        public int Id { get; set; }

        public int RequestPriceId { get; set; }

        [Required]
        [StringLength(256)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(16)]
        public string PhoneNumber { get; set; }

        [StringLength(256)]
        public string CompanyName { get; set; }

        public DateTimeOffset RequestDateTime { get; set; }

        public bool IsEmailSentToSales { get; set; }

        public Nullable<System.DateTimeOffset> EmailSentDateTime { get; set; }

        public bool IsCustomerContacted { get; set; }

        public Nullable<System.DateTimeOffset> CustomerContactedDateTime { get; set; }

        public bool IsBusinessDone { get; set; }

        public virtual RequestPrice RequestPrice { get; set; }
    }
}
