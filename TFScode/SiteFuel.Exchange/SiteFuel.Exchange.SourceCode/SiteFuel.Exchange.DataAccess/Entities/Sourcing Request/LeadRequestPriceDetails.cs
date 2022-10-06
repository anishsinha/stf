using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class LeadRequestPriceDetails
    {
        public LeadRequestPriceDetails()
        {
            LeadPricingDetails = new HashSet<LeadPricingDetail>();
            LeadCumulationDetails = new HashSet<LeadCumulationDetail>();
        }
        [Key]
        public int Id { get; set; }

        public int Currency { get; set; }
        public int LeadRequestId { get; set; }
        public bool IsTierPricingRequired { get; set; }
        public bool IsSuppressPricing { get; set; }
        public decimal ExchangeRate { get; set; }

        public int UoM { get; set; }

        public int? TierTypeId { get; set; }

        public int? PricingTypeId { get; set; }

        public int? CumulationTypeId { get; set; }
        public int? CumulationResetDay { get; set; }
        public DateTimeOffset? CumulationResetDate { get; set; }
        [ForeignKey("LeadRequestId")]
        public virtual LeadRequest LeadRequest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeadPricingDetail> LeadPricingDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeadCumulationDetail> LeadCumulationDetails { get; set; }
    }
}
