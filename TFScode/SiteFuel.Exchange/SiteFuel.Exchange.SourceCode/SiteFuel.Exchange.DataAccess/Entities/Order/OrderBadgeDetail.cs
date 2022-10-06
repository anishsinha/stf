namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OrderBadgeDetail
    {
        [Key]
        public int Id { get; set; }
        
        public int OrderId { get; set; }

        public bool IsCommonBadge { get; set; }

        public PickupLocationType PickupLocationType { get; set; }

        [StringLength(100)]
        public string BadgeNo1 { get; set; }

        [StringLength(100)]
        public string BadgeNo2 { get; set; }

        [StringLength(100)]
        public string BadgeNo3 { get; set; }

        public int? TerminalId { get; set; }

        public int? BulkPlantId { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("TerminalId")]
        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        [ForeignKey("BulkPlantId")]
        public virtual BulkPlantLocation BulkPlantLocation { get; set; }
    }
}
