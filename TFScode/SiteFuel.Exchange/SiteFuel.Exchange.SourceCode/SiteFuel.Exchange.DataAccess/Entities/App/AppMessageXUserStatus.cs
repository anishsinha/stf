namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AppMessageXUserStatuses")]
    public partial class AppMessageXUserStatus
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MessageId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public int UserTypeId { get; set; }

        public int AppMessageStatusId { get; set; }

        public bool IsMarkedAsRead { get; set; }

        public bool IsMarkedAsImportant { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public virtual AppMessage AppMessage { get; set; }

        public virtual MstAppMessageStatus MstAppMessageStatus { get; set; }

        public virtual MstAppMessageUserType MstAppMessageUserType { get; set; }

        public virtual User User { get; set; }
    }
}
