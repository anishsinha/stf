namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Acknowledgement
    {
        public int Id { get; set; }

        [Required]
        public int EntityId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int UserCompanyId { get; set; }

        public bool IsSent { get; set; }

        [StringLength(512)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("UserCompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
