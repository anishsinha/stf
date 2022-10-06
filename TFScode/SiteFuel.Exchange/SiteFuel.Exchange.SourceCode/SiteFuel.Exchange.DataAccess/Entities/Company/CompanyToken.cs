namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CompanyToken
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } 
    }
}
