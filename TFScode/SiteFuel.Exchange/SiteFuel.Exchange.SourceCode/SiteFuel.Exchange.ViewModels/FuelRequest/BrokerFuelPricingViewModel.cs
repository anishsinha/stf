using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;

namespace SiteFuel.Exchange.ViewModels
{
    public class BrokerFuelPricingViewModel : StatusViewModel
    {
        public BrokerFuelPricingViewModel()
        {
            InstanceInitialize();
        }

        public BrokerFuelPricingViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }
        private void InstanceInitialize()
        {
            PricingTypeId = (int)PricingType.RackAverage;
            ExchangeRate = 1;
            FuelPricingDetails = new FuelRequestPricingDetailsViewModel();
            TierPricing = new TierPricingViewModel();
        }
        public BrokerMarginViewModel BrokerMargin { get; set; }

        [Display(Name = nameof(Resource.lblPrice), ResourceType = typeof(Resource))]
        public int PricingTypeId { get; set; }

        [RequiredIf("PricingTypeId", (int)PricingType.RackAverage, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public Nullable<int> RackAvgTypeId { get; set; }

        [RequiredIf("PricingTypeId", (int)PricingType.PricePerGallon, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal PricePerGallon { get; set; }

        [RequiredIf("PricingTypeId", (int)PricingType.RackAverage, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal RackPrice { get; set; }

        public decimal EstimatedPPG { get; set; }

        [Display(Name = nameof(Resource.lblIncludeTaxes), ResourceType = typeof(Resource))]
        public bool IncludeTaxes { get; set; }

        [RequiredIf("PricingTypeId", (int)PricingType.Suppliercost, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public Nullable<int> SupplierCostMarkupTypeId { get; set; }

        [Display(Name = nameof(Resource.lblFuelCost), ResourceType = typeof(Resource))]
        [Range(typeof(Decimal), "0", ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIf("PricingTypeId", (int)PricingType.Suppliercost, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal SupplierCostMarkupValue { get; set; }

        public decimal? SupplierCost { get; set; }

        public Nullable<int> MarkertBasedPricingTypeId { get; set; }

        [Display(Name = nameof(Resource.lblCityGroupTerminal), ResourceType = typeof(Resource))]
        public bool IsCityGroupTerminal { get; set; }

        [Display(Name = nameof(Resource.lblCityGroupTerminal), ResourceType = typeof(Resource))]
        public int? CityGroupTerminalId { get; set; }

        public string CityGroupTerminalName { get; set; }

        public int CityGroupTerminalStateId { get; set; }

        public Currency Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public FuelRequestPricingDetailsViewModel FuelPricingDetails { get; set; }

        public bool IsTierPricing { get; set; }
        public TierPricingViewModel TierPricing { get; set; }
    }
}
