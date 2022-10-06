using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    [Table("APIDebugLog")]
    public partial class APIDebugLog
    {
        public int ID { get; set; }

        [StringLength(200)]
        public string MachineName { get; set; }

        [Required]
        [StringLength(200)]
        public string SiteName { get; set; }

        public DateTime LogDateTime { get; set; }

        [StringLength(200)]
        public string UserName { get; set; }

        [StringLength(2000)]
        public string Url { get; set; }

        [Required]
        public string message { get; set; }

        [Required]
        public string RequestJson { get; set; }

        [Required]
        public string ResponseJson { get; set; }

        public double TotalMilliseconds { get; set; }
        public string device { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }
    }
}
