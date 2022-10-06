using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class OfferQuickUpdatePreference
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public bool IsCustomerTier { get; set; }

        public bool IsCustomer { get; set; }

        public bool IsMarketOffer { get; set; }

        public bool IsState { get; set; }

        public bool IsCity { get; set; }

        public bool IsPricingType { get; set; }

        public bool IsFeeType { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }
}
