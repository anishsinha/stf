namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WebNotification
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public int Id { get; set; }

        public int NotificationTypeId { get; set; }

        public int EntityId { get; set; }

        public int CreatedBy { get; set; }

        public int CreatedFor { get; set; }

        public int CreatedForCompanyId { get; set; }

        public int CreatedForCompanyTypeId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public bool IsNotificationRead { get; set; }

        public string JsonMessage { get; set; }

        [ForeignKey("NotificationTypeId")]
        public virtual MstNotificationType NotificationType { get; set; }
    }
}
