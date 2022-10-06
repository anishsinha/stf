namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Carrier
    {
        public int Id { get; set; }

        [StringLength(256)]
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
    }
}
