namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Newsfeed
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int EntityId { get; set; }

        public int EntityTypeId { get; set; }

        public int TargetEntityId { get; set; }

        public int RecipientCompanyId { get; set; }

        [Required]
        [StringLength(512)]
        public string FeedMessage { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public bool IsRead { get; set; }

        public bool IsActive { get; set; }
    }
}
