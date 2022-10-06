namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class JobCarrierDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int JobId { get; set; }
        public int CarrierCompanyId { get; set; }
        public int CreatedByCompanyId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
        [ForeignKey("CarrierCompanyId")]
        public virtual Company Company { get; set; }
        [ForeignKey("CreatedByCompanyId")]
        public virtual Company CreatedByCompany { get; set; }
    }
}
