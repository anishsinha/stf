namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("NotificationLog")]
    public partial class NotificationLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        [StringLength(500)]
        public string Body { get; set; }

        [StringLength(200)]
        public string To { get; set; }

        [StringLength(200)]
        public string CC { get; set; }

        [StringLength(200)]
        public string BCC { get; set; }

        public DateTimeOffset LogDateTime { get; set; }

        public int? NotificationId { get; set; }

        public int Status { get; set; }

        public int NotificationType { get; set; }

        [ForeignKey("NotificationId")]
        public virtual Notification Notification { get; set; }
    }
}
