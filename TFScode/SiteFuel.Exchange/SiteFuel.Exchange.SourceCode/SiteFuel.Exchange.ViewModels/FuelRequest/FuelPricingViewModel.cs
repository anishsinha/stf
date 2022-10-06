using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelPricingViewModel : StatusViewModel
    {
        public FuelPricingViewModel()
        {
            InstanceInitialize();
        }

        public FuelPricingViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            PricingTypeId = (int)PricingType.PricePerGallon;
            ExchangeRate = 1;
            IsTierPricingRequired = false;
            DifferentFuelPrices = new List<DifferentFuelPriceViewModel>();
            FuelPricingDetails = new FuelRequestPricingDetailsViewModel();
            TierPricing = new TierPricingViewModel();
        }

        [Display(Name = nameof(Resource.lblPrice), ResourceType = typeof(Resource))]
        public int PricingTypeId { get; set; }

        //[RequiredIf("PricingTypeId", (int)PricingType.RackAverage, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public Nullable<int> RackAvgTypeId { get; set; }

        //[Range(typeof(Decimal), "0", ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        //[RequiredIf("PricingTypeId", (int)PricingType.PricePerGallon, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal PricePerGallon { get; set; } = 0;

        //[Range(typeof(Decimal), "0", ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        //[RequiredIf("PricingTypeId", (int)PricingType.RackAverage, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal RackPrice { get; set; } = 0;

        [Display(Name = nameof(Resource.lblIncludeTaxes), ResourceType = typeof(Resource))]
        public bool IncludeTaxes { get; set; }

        //[RequiredIf("PricingTypeId", (int)PricingType.Suppliercost, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public Nullable<int> SupplierCostMarkupTypeId { get; set; }

        [Display(Name = nameof(Resource.lblFuelCost), ResourceType = typeof(Resource))]
        //[Range(typeof(Decimal), "0", ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        //[RequiredIf("PricingTypeId", (int)PricingType.Suppliercost, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal SupplierCostMarkupValue { get; set; } = 0;

        public decimal? SupplierCost { get; set; }

        public int? SupplierCostTypeId { get; set; }

        public Nullable<int> MarkertBasedPricingTypeId { get; set; }

        [Display(Name = nameof(Resource.lblCityGroupTerminal), ResourceType = typeof(Resource))]
        public int? CityGroupTerminalId { get; set; }

        public string CityGroupTerminalName { get; set; }

        public int CityGroupTerminalStateId { get; set; }

        public string TerminalName { get; set; }

        public Nullable<int> TerminalId { get; set; }

        [Display(Name = nameof(Resource.lblBrokerMarkup), ResourceType = typeof(Resource))]
        public decimal BrokerMarkUp { get; set; }

        [Display(Name = nameof(Resource.lblSupplierMarkup), ResourceType = typeof(Resource))]
        public decimal SupplierMarkUp { get; set; }

        public Currency Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public bool IsTierPricingRequired { get; set; }

        public List<DifferentFuelPriceViewModel> DifferentFuelPrices { get; set; }

        public string FormattedPricing { get; set; }

        public FuelRequestPricingDetailsViewModel FuelPricingDetails { get; set; }

        public string ParameterJSON { get; set; }//source region,terminal,bulkplant
        public int? FuelTypeId { get; set; }
        public TierPricingViewModel TierPricing { get; set; }
        public string SetPricePerGallon()
        {
            string price = string.Empty;

            decimal pricePerGallon = PricePerGallon;

            int pricingTypeId = PricingTypeId;
            int rackAvgTypeId = RackAvgTypeId.HasValue ? RackAvgTypeId.Value : 0;
            if (pricingTypeId == (int)PricingType.Tier)
            {
                price = Resource.lblTier;
            }
            else if (pricingTypeId == (int)PricingType.PricePerGallon)
            {
                price = Resource.constSymbolCurrency + pricePerGallon.GetPreciseValue(4);
            }
            else if (pricingTypeId == (int)PricingType.Suppliercost)
            {
                pricePerGallon = SupplierCostMarkupValue;
                rackAvgTypeId = SupplierCostMarkupTypeId.Value;
                switch (rackAvgTypeId)
                {
                    case (int)RackPricingType.PlusPercent:
                        price = $"{Resource.lblFuelCostPlus} {pricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                        break;
                    case (int)RackPricingType.MinusPercent:
                        price = $"{Resource.lblFuelCostMinus} {pricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                        break;
                    case (int)RackPricingType.PlusDollar:
                        price = $"{Resource.lblFuelCostPlus} {Resource.constSymbolCurrency}{pricePerGallon.GetPreciseValue(4)}";
                        break;
                    case (int)RackPricingType.MinusDollar:
                        price = $"{Resource.lblFuelCostMinus} {Resource.constSymbolCurrency}{pricePerGallon.GetPreciseValue(4)}";
                        break;
                }
            }
            else
            {
                var rackPricingType = MarkertBasedPricingTypeId ?? pricingTypeId;
                var rackText = rackPricingType == (int)PricingType.RackHigh ? Resource.lblRackHigh : rackPricingType == (int)PricingType.RackLow ? Resource.lblRackLow : Resource.lblRackAverage;
                pricePerGallon = RackPrice;
                switch (rackAvgTypeId)
                {
                    case (int)RackPricingType.PlusPercent:
                        price = $"{rackText} + {pricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                        break;
                    case (int)RackPricingType.MinusPercent:
                        price = $"{rackText} - {pricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                        break;
                    case (int)RackPricingType.PlusDollar:
                        price = $"{rackText} + {Resource.constSymbolCurrency}{pricePerGallon.GetPreciseValue(4)}";
                        break;
                    case (int)RackPricingType.MinusDollar:
                        price = $"{rackText} - {Resource.constSymbolCurrency}{pricePerGallon.GetPreciseValue(4)}";
                        break;
                }
            }

            FormattedPricing = price;
            return FormattedPricing;
        }

    }
}
