namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ApiLog
    {
        public int Id { get; set; }

        [Required]
        public string Request { get; set; }
        [Required]
        public string Response { get; set; }
      
        [Required]
        [StringLength(512)]
        public string Url { get; set; }
        [StringLength(512)]
        public string ExternalRefID { get; set; }
        public string Message { get; set; }

        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        [Required]
        public int CreatedBy { get; set; }

        public int CompanyId { get; set; }

        public bool IsSuccessStatusCode { get; set; }
        public int RetryCount { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

       

       

        
    }
}
