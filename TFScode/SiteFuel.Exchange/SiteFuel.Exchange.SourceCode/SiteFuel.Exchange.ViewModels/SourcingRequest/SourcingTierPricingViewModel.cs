using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SourcingTierPricingViewModel
    {
        public TierPricingType TierPricingType { get; set; } = TierPricingType.VolumeBased;

        public bool IsResetCumulation { get; set; } = false;

        public PricingViewModel AboveQuantityPricing { get; set; } = new PricingViewModel();

        public List<PricingViewModel> Pricings { get; set; } = new List<PricingViewModel>();

        public CumulationSetting ResetCumulationSetting { get; set; } = new CumulationSetting();
        public int RequestPriceDetailId { get; set; }
    }
}
