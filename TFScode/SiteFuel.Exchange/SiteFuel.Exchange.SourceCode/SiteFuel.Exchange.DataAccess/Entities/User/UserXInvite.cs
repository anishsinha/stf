namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserXInvite
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        public int InvitedBy { get; set; }

        public bool IsInvitationSent { get; set; }

        public bool IsOnboarded { get; set; }

        public string Message { get; set; }

        public Nullable<DateTimeOffset> CreatedDate { get; set; }

        public Nullable<DateTimeOffset> UpdatedDate { get; set; }

        public Nullable<int> InvitedToUserId { get; set; }

        public virtual User User { get; set; }
    }
}
