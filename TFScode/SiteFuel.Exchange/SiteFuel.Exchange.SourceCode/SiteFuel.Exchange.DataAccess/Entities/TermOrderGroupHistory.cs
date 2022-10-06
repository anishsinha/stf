namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public partial class TermOrderGroupHistory
    {
        public int Id { get; set; }
        public int OrderGroupId { get; set; }
        public int OrderId { get; set; }
        public bool IsActive { get; set; }
        public decimal MinVolume { get; set; }
        public decimal MaxVolume { get; set; }
        public decimal TotalDropped { get; set; }
        public bool IsFulfilled { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        [ForeignKey("OrderGroupId")]
        public virtual OrderGroup OrderGroup { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
