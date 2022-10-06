using System;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class LoadOptimizationUser
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string DistributedUsers { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
