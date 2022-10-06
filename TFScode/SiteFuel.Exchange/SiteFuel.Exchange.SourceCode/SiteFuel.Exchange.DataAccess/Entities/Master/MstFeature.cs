namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MstFeature
    {
        public MstFeature()
        {
            CreatedDate = DateTimeOffset.Now;
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int CompanyTypeId { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
