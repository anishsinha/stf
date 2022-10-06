namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class MstNotificationType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstNotificationType()
        {
            WebNotifications = new HashSet<WebNotification>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WebNotification> WebNotifications { get; set; }
    }
}
