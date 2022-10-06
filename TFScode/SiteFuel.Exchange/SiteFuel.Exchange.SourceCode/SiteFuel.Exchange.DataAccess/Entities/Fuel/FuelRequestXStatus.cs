namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FuelRequestXStatuses")]
    public partial class FuelRequestXStatus
    {
        public int Id { get; set; }

        public int FuelRequestId { get; set; }

        public int StatusId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public virtual FuelRequest FuelRequest { get; set; }
		
        public virtual MstFuelRequestStatus MstFuelRequestStatus { get; set; }
    }
}
