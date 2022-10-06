using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class OfferPricingItem
    {
        public OfferPricingItem Clone(int userId)
        {
            var pricingItem = new OfferPricingItem()
            {
                CityId = CityId,
                CustomerId = CustomerId,
                IsActive = true,
                CreatedBy = CreatedBy,
                CreatedDate = CreatedDate,
                ParentId = Id,
                StateId = StateId,
                TierId = TierId,
                ZipCode = ZipCode,
                UpdateCommandId = UpdateCommandId,
                UpdatedBy = userId,
                UpdatedDate = DateTimeOffset.Now
            };
            return pricingItem;
        }
        public OfferPricingItem CloneWithId(int userId)
        {
            var pricingItem = Clone(userId);
            pricingItem.OfferPricingId = OfferPricingId;
            return pricingItem;
        }
    }
}
