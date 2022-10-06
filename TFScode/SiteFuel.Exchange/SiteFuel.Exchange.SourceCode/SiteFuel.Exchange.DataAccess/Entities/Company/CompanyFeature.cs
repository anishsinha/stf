namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CompanyFeature
    {
        public CompanyFeature()
        {
            UpdatedDate = DateTimeOffset.Now;
            CreatedDate = DateTimeOffset.Now;
        }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int FeatureId { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public virtual Company Company { get; set; }
        [ForeignKey("FeatureId")]
        public virtual MstFeature MstFeature { get; set; }
    }
}
