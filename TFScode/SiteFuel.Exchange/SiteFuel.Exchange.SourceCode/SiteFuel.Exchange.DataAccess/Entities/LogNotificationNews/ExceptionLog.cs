namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExceptionLog")]
    public partial class ExceptionLog
    {
        public int ID { get; set; }

        [StringLength(200)]
        public string MachineName { get; set; }

        [Required]
        [StringLength(200)]
        public string SiteName { get; set; }

        public DateTime LogDateTime { get; set; }

        [Required]
        [StringLength(5)]
        public string Level { get; set; }

        [StringLength(200)]
        public string UserName { get; set; }

        [Required]
        public string Message { get; set; }

        [StringLength(300)]
        public string Logger { get; set; }

        public string Properties { get; set; }

        [StringLength(200)]
        public string ServerName { get; set; }

        [StringLength(100)]
        public string Port { get; set; }

        [StringLength(2000)]
        public string Url { get; set; }

        public bool? Https { get; set; }

        [StringLength(100)]
        public string ServerAddress { get; set; }

        [StringLength(100)]
        public string RemoteAddress { get; set; }

        [StringLength(300)]
        public string Callsite { get; set; }

        public string Exception { get; set; }
    }
}
