namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QueueResult")]
    public partial class QueueResult
    {
        public int Id { get; set; }

        public int QueueMessageId { get; set; }

        public int RetryCount { get; set; }

        [Required]
        public string RetryMessage { get; set; }

        public virtual QueueMessage QueueMessage { get; set; }
    }
}
