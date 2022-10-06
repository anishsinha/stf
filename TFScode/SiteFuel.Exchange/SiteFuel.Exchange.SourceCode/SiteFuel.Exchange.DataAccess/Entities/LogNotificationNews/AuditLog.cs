namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class AuditLog
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string AuditEventType { get; set; }

        [StringLength(20)]
        public string AuditEntityType { get; set; }

        public int AuditEntityId { get; set; }

        [StringLength(100)]
        public string Message { get; set; }

        [StringLength(20)]
        public string MachineName { get; set; }

        [StringLength(20)]
        public string RemoteAddress { get; set; }

        [StringLength(20)]
        public string Url { get; set; }

        [StringLength(30)]
        public string CallSite { get; set; }

        [StringLength(100)]
        public string JsonMessage { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
