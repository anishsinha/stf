namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CreditAppDocument
    {
        public int Id { get; set; }

        [Required]
        [StringLength(252)]
        public string FileName { get; set; }

        [Required]
        [StringLength(1024)]
        public string ModifiedFileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        public int AddedBy { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual User User { get; set; }
    }
}
