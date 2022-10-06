using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class GetOfferViewModel
    {
        public GetOfferViewModel()
        {
        }
        public int BuyerCompanyId { get; set; }

        public int FuelTypeId { get; set; }

        public int Quantity { get; set; }

        public int? JobId { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public int? StateId { get; set; }

        public TruckLoadTypes TruckLoadType { get; set; }

        public int PricingTypeId { get; set; }

        public PricingSource PricingSource { get; set; }

        public PricingSourceFeedTypes FeedType { get; set; }

        public QuantityIndicatorTypes QuantityIndicator { get; set; }
     
        public FuelClassTypes FuelClass { get; set; }
      
        public int? CityGroupTerminalId { get; set; }
    }
}
