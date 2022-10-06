using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class OfferPricing
    {
        public OfferPricing Clone(int userId, List<FuelFee> fees)
        {
            var newEntity = new OfferPricing();
            newEntity.FuelTypeId = FuelTypeId;
            newEntity.PricingTypeId = PricingTypeId;
            newEntity.PricePerGallon = PricePerGallon;
            newEntity.RackAvgTypeId = RackAvgTypeId;
            newEntity.SupplierCostTypeId = SupplierCostTypeId;
            newEntity.SupplierCost = SupplierCost;
            newEntity.CityGroupTerminalId = CityGroupTerminalId;
            newEntity.UoM = UoM;
            newEntity.Currency = Currency;
            newEntity.CountryId = CountryId;
            newEntity.ExchangeRate = ExchangeRate;
            newEntity.BasePrice = BasePrice;
            newEntity.CreatedBy = userId;
            newEntity.CreatedDate = CreatedDate;
            newEntity.Name = Name;
            newEntity.OfferChainId = OfferChainId; // ... ??
            newEntity.OfferTypeId = OfferTypeId;
            newEntity.SupplierCompanyId = SupplierCompanyId;
            newEntity.StatusId = StatusId;
            newEntity.ExpirationDate = ExpirationDate;
            newEntity.PricingSource = PricingSource;
            newEntity.TruckLoadType = TruckLoadType;

            if (fees.Any())
                newEntity.FuelFees = fees.Select(x => x.Clone()).ToList();
            else
                newEntity.FuelFees = FuelFees.Select(x => x.Clone()).ToList();
            SetDifferentFuelPrices(newEntity, this);

            // Clone FuelFee as well
            return newEntity;
        }


        private static void SetDifferentFuelPrices(OfferPricing newEntity, OfferPricing oldEntity)
        {
            foreach (var diffPrice in oldEntity.DifferentFuelPrices)
            {
                var tier = new DifferentFuelPrice();
                tier.Currency = diffPrice.Currency;
                tier.MaxQuantity = diffPrice.MaxQuantity;
                tier.MinQuantity = diffPrice.MinQuantity;
                tier.PricePerGallon = diffPrice.PricePerGallon;
                tier.PricingTypeId = diffPrice.PricingTypeId;
                tier.RackAvgTypeId = diffPrice.RackAvgTypeId;
                tier.ResaleId = diffPrice.ResaleId;
                tier.SupplierCost = diffPrice.SupplierCost;
                tier.SupplierCostTypeId = diffPrice.SupplierCostTypeId;
                tier.UoM = diffPrice.UoM;
                newEntity.DifferentFuelPrices.Add(tier);
            }
        }
    }
}
