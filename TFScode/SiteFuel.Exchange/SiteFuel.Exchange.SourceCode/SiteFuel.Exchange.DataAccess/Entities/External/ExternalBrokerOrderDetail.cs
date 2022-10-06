namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExternalBrokerOrderDetail
    {
        [Key]
        public int OrderId { get; set; }

        public int InvoicePreferenceId { get; set; }

        [MaxLength(100)]
        public string VendorId { get; set; }

        [MaxLength(100)]
        public string CustomerNumber { get; set; }

        [MaxLength(100)]
        public string ShipTo { get; set; }

        [MaxLength(100)]
        public string Source { get; set; }

        [MaxLength(100)]
        public string ProductCode { get; set; }

        public int? ThirdPartyNozzleId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
