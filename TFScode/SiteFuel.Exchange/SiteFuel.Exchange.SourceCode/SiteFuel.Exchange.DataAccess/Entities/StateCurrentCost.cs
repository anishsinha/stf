namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class StateCurrentCost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StateCurrentCost()
        {

        }

        public int Id { get; set; }
        public int? StateId { get; set; }
        public int CurrentCostId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("CurrentCostId")]
        public virtual CurrentCost CurrentCost { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }
    }
}
