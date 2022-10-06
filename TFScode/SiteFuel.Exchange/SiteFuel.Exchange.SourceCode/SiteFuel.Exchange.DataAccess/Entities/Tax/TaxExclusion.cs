
namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TaxExclusion
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public TaxExclusionType ExclusionType { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.Now;
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }
}