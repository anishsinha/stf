namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserToken
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTimeOffset ExpiryTime { get; set; }

        public virtual User User { get; set; }
    }
}
