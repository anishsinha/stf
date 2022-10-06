namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RequestPrice")]
    public partial class RequestPrice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequestPrice()
        {
            RequestFuels = new HashSet<RequestFuel>();
            Currency = Currency.USD;
            UoM = UoM.Gallons;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string ZipCode { get; set; }

        public decimal Quantity { get; set; }

        public int ProductId { get; set; }

        public DateTimeOffset RequestDateTime { get; set; }

        public int TerminalId { get; set; }

        public decimal PricePerGallon { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public DateTimeOffset PricingDate { get; set; }

        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        public virtual MstProduct MstProduct { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestFuel> RequestFuels { get; set; }
    }
}
