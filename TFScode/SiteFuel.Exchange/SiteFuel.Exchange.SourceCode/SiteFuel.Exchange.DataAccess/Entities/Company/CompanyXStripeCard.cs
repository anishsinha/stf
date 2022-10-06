namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CompanyXStripeCard
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(256)]
        public string StripeCardToken { get; set; }

        public bool IsPrimary { get; set; }

        public virtual Company Company { get; set; }

        public virtual User User { get; set; }
    }
}
