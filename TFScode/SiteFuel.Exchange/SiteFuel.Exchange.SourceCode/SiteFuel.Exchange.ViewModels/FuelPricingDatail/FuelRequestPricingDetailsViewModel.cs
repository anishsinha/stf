using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels.FuelPricingDatail
{
    public class FuelRequestPricingDetailsViewModel
    {
        public int FuelRequestId { get; set; }

        [Display(Name = nameof(Resource.lblFeed), ResourceType = typeof(Resource))]
        public int? FeedTypeId { get; set; }

        [Display(Name = nameof(Resource.lblFuelClass), ResourceType = typeof(Resource))]
        public int? FuelClassTypeId { get; set; }

        [Display(Name = nameof(Resource.lblNetGross), ResourceType = typeof(Resource))]
        public int? PricingQuantityIndicatorTypeId { get; set; }

        [Display(Name = nameof(Resource.lblIndices), ResourceType = typeof(Resource))]
        public int PricingSourceId { get; set; } = (int)PricingSource.Axxis;

        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public int? TruckLoadTypeId { get; set; }

        [Display(Name = nameof(Resource.lblNetGross), ResourceType = typeof(Resource))]
        public int? PricingSourceQuantityIndicatorTypeId { get; set; }

        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public TruckLoadTypes TruckLoadTypes { get; set; }

        [Display(Name = nameof(Resource.lblFreightOnBoard), ResourceType = typeof(Resource))]
        public FreightOnBoardTypes FreightOnBoardTypes { get; set; } = FreightOnBoardTypes.Terminal;

        [Display(Name = nameof(Resource.lblNetGross), ResourceType = typeof(Resource))]
        public QuantityIndicatorTypes PricingSourceQuantityIndicatorTypes { get; set; }

        [Display(Name = nameof(Resource.lblFuelClass), ResourceType = typeof(Resource))]
        public FuelClassTypes FuelClassTypes { get; set; }

        public int StateDefaultQuantityIndicatorId { get; set; } // required to get state's default quantity indicator

        public WeekendDropPricingDay WeekendDropPricingDay { get; set; } = WeekendDropPricingDay.Saturday;

        public int RequestPriceDetailId { get; set; }

        public PricingCodeDetailViewModel PricingCode { get; set; } = new PricingCodeDetailViewModel();

        public FuelRequestPricingDetailsViewModel Clone()
        {
            var thisObject = (FuelRequestPricingDetailsViewModel)this.MemberwiseClone();
            return thisObject;
        }
    }
}
