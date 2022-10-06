namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderXStatuses")]
    public partial class OrderXStatus
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int StatusId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public virtual MstOrderStatus MstOrderStatus { get; set; }

        public virtual Order Order { get; set; }

        public virtual User User { get; set; }
    }
}
