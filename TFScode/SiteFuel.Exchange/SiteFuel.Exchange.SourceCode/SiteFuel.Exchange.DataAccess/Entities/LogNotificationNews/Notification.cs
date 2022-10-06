namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Notification
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Notification()
        {
            Companies = new HashSet<Company>();
        }

        public int Id { get; set; }

        public int EventTypeId { get; set; }

        public int EntityId { get; set; }

        public int TriggeredBy { get; set; }

        public bool IsEmailNotificationSent { get; set; }

        public bool IsSmsNotificationSent { get; set; }

        public bool IsInAppNotificationSent { get; set; }

        public Nullable<DateTimeOffset> CreatedDate { get; set; }

        public string JsonMessage { get; set; }

        public int Status { get; set; }

        public int NotificationType { get; set; }

        public int ApplicationTemplateId { get; set; }

        public bool IsManualTrigger { get; set; }

        public virtual MstEventType MstEventType { get; set; }

        [ForeignKey("ApplicationTemplateId")]
        public virtual MstApplicationTemplate MstApplicationTemplate { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Companies { get; set; }
    }
}
