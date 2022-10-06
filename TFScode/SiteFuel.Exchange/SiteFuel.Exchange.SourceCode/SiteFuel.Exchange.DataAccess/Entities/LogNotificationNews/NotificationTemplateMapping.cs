namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NotificationTemplateMapping
    {
        public int Id { get; set; }

        public int NotificationTemplateId { get; set; }

        public EventSubType EventSubTypeId { get; set; }

        public int ApplicationTemplateId { get; set; }

        [ForeignKey("NotificationTemplateId")]
        public virtual MstNotificationTemplate MstNotificationTemplate { get; set; }

        [ForeignKey("ApplicationTemplateId")]
        public virtual MstApplicationTemplate MstApplicationTemplate { get; set; }
    }
}
