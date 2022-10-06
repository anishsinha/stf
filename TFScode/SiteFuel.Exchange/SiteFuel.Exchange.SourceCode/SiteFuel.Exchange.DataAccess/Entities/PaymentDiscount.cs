namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PaymentDiscount
    {
        public int Id { get; set; }

        public int DiscountPercentage { get; set; }

        public int? FuelRequestId { get; set; }

        public int? InvoiceId { get; set; }

        public int WithInDays { get; set; }

        [ForeignKey("FuelRequestId")]
        public virtual FuelRequest FuelRequest { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }
    }
}
