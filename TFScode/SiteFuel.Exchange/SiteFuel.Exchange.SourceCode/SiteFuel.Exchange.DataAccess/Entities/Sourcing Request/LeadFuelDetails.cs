using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class LeadFuelDetail
    {
        public LeadFuelDetail()
        {
            LeadFuelFees = new HashSet<LeadFuelFee>();
        }
        [Key]
        public int Id { get; set; }
        public int LeadRequestId { get; set; }
        public int FuelDisplayGroupId { get; set; }
        public int FuelTypeId { get; set; }
        public int QuantityTypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal MinimumQuantity { get; set; }
        public decimal MaximumQuantity { get; set; }
        public QuantityIndicatorTypes QuantityIndicatorTypes { get; set; }
        [StringLength(256)]
        public string NonStandardFuelName { get; set; }
        [StringLength(1024)]
        public string NonStandardFuelDescription { get; set; }
        public bool IsTierPricing { get; set; }
        public TierPricingType TierPricingType { get; set; }
        public int PricingTypeId { get; set; }
        public decimal PricePerGallon { get; set; }
        [ForeignKey("LeadRequestId")]
        public virtual LeadRequest LeadRequest { get; set; }
        public virtual ICollection<LeadFuelFee> LeadFuelFees { get; set; }
    }
}
