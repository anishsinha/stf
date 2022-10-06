namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class QueueMessage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QueueMessage()
        {
            QueueResults = new HashSet<QueueResult>();
        }

        public int Id { get; set; }

        public int RetryCount { get; set; }

        public int Status { get; set; }

        public int ProcessTypeId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }

        [Required]
        public string JsonMessage { get; set; }

        public virtual MstProcessType MstProcessType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QueueResult> QueueResults { get; set; }
    }
}
