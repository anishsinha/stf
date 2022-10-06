using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SourcingRequestGridViewModel 
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; }
        public string JobName { get; set; }
        public string FuelType { get; set; }
        public string Quantity { get; set; }
        public string Pricing { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryType { get; set; }
        public int ModifiedBy { get; set; }
        public string Status { get; set; }
        public bool ViewedModified { get; set; }
        public decimal PricePerGallon { get; set; } = 0;
        public int PricingTypeId { get; set; }
        public Nullable<int> RackAvgTypeId { get; set; }
        public decimal SupplierCostMarkupValue { get; set; } = 0;
        public Nullable<int> SupplierCostMarkupTypeId { get; set; }
        public int? MarkertBasedPricingTypeId { get; set; }
        public decimal RackPrice { get; set; } = 0;
        public string FormattedPricing { get; set; }

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
                rackAvgTypeId = SupplierCostMarkupTypeId.HasValue ? SupplierCostMarkupTypeId.Value : 0;
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
