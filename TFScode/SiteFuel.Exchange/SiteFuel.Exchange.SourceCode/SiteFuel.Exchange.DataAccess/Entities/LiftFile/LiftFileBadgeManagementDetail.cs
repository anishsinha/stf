namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LiftFileBadgeManagementDetail
    {
        public int Id { get; set; }

        [StringLength(256)]
        public string BadgeNumber { get; set; }

        public decimal CustomerNumber { get; set; }

        public DateTimeOffset AddedDate { get; set; }

        public int AddedByCompanyId { get; set; }
        public bool IsActive { get; set; }

        [StringLength(1)]
        public string StatusCode { get; set; }
    }
}
