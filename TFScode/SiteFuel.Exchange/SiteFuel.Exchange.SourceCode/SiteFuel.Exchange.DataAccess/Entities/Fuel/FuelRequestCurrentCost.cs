namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelRequestCurrentCost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FuelRequestCurrentCost()
        {
        }

        [Key]
        public int Id { get; set; }

        public int FuelRequestId { get; set; }
        public int CurrentCostId { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        [ForeignKey("FuelRequestId")]
        public virtual FuelRequest FuelRequest { get; set; }

        [ForeignKey("CurrentCostId")]
        public virtual CurrentCost CurrentCost { get; set; }

    }
}
