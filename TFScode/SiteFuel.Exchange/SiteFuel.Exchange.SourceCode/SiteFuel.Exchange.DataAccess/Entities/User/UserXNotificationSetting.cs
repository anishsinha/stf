namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserXNotificationSetting
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EventTypeId { get; set; }

        public bool IsEmail { get; set; }

        public bool IsSMS { get; set; }

        public bool IsInApp { get; set; }

        public virtual MstEventType MstEventType { get; set; }

        public virtual User User { get; set; }
    }
}
