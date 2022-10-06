namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CreditAppDetail
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int From { get; set; }

        [Required]
        public string EmailSubject { get; set; }

        [Required]
        public string EmailContent { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public virtual Company Company { get; set; }

        public virtual User User { get; set; }
    }
}
