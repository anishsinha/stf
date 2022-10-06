namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderXTogglePricingDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }

        public bool IsHidePricingEnabledForBuyer { get; set; }

        public bool IsHidePricingEnabledForSupplier { get; set; }

        public virtual Order Order { get; set; }
    }
}
