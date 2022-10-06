namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FreightTableCompany
    {
        [Key]
        public int Id { get; set; }

        public int? FuelSurchargeIndexId { get; set; }

        public int? AccessorialFeeId { get; set; }

        public int? FreightRateRuleId { get; set; }

        public bool IsActive { get; set; }

        public AssignedCompanyType AssignedCompanyType { get; set; }

        public int AssignedCompanyId { get; set; }

        [ForeignKey("FuelSurchargeIndexId")]
        public virtual FuelSurchargeIndex FuelSurchargeIndex { get; set; }

        [ForeignKey("AccessorialFeeId")]
        public virtual AccessorialFee AccessorialFee { get; set; }

        [ForeignKey("FreightRateRuleId")]
        public virtual FreightRateRule FreightRateRule { get; set; }

        [ForeignKey("AssignedCompanyId")]
        public virtual Company AssignedCompany { get; set; }
    }
}
