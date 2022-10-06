using SiteFuel.Exchange.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Model
{
    public class OfferQuickUpdateTempItem
    {
        public OfferPricing OfferPricing { get; set; }
        public OfferPricingItem OfferPricingItem { get; set; }
        public List<FuelFee> FuelFees { get; set; }
        public int? CustomerTierId { get; set; }
    }
}
