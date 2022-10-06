namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public partial class OrderGroupXOrder
    {
        public OrderGroupXOrder()
        {

        }

        [Key, Column(Order = 0)]
        public int OrderGroupId { get; set; }

        [Key, Column(Order = 1)]
        public int OrderId { get; set; }
        public bool IsActive { get; set; }
        public decimal BlendPercentage { get; set; }
        public decimal MinVolume { get; set; }
        public decimal MaxVolume { get; set; }

        public decimal TotalDropped { get; set; }
        public bool IsFulfilled { get; set; }

        [StringLength(275)]
        public string GroupPoNumber { get; set; }

        [ForeignKey("OrderGroupId")]
        public virtual OrderGroup OrderGroup { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

    }
}
