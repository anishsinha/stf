using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Offer;
using System;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class JobBudgetMapper
    {
        public static JobBudgetViewModel ToViewModel(this JobBudget entity, JobBudgetViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new JobBudgetViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.BudgetCalculationTypeId = entity.BudgetCalculationTypeId;
            viewModel.BudgetTypeId = entity.BudgetTypeId;
            viewModel.Budget = entity.Budget.GetPreciseValue(6);
            viewModel.Gallons = entity.Gallons.GetPreciseValue(6);
            viewModel.PricePerGallon = entity.PricePerGallon.GetPreciseValue(6);
            viewModel.IsBudgetTracked = entity.IsBudgetTracked;
            viewModel.HedgeAmount = entity.HedgeAmount.GetPreciseValue(6);
            viewModel.SpotAmount = entity.SpotAmount.GetPreciseValue(6);
            viewModel.IsExceededBudget = entity.IsExceededBudget;
            viewModel.IsTaxExempted = entity.IsTaxExempted;
            viewModel.IsDropPictureRequired = entity.IsDropPictureRequired;
            viewModel.IsAssetTracked = entity.IsAssetTracked;
            viewModel.IsAssetDropStatusEnabled = entity.IsAssetDropStatusEnabled;
            viewModel.IsHedgeAmountTracked = entity.IsHedgeAmountTracked;

            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;

            return viewModel;
        }

        public static JobBudget ToEntity(this JobBudgetViewModel viewModel, JobBudget entity = null)
        {
            if (entity == null)
                entity = new JobBudget();

            if (viewModel.Id > 0)
                entity.Id = viewModel.Id;

            entity.BudgetTypeId = viewModel.BudgetTypeId;
            entity.BudgetCalculationTypeId = viewModel.BudgetCalculationTypeId;
            entity.IsTankAvailable =false; 
            entity.UoM = viewModel.UoM;
            entity.Currency = viewModel.Currency;
            entity.ExchangeRate = viewModel.ExchangeRate;

            if (entity.BudgetCalculationTypeId == (int)BudgetCalculationType.Budget)
            {
                entity.Budget = viewModel.Budget;
                entity.Gallons = 0;
                entity.PricePerGallon = 0;
            }
            else if (entity.BudgetCalculationTypeId == (int)BudgetCalculationType.Fuel)
            {
                entity.Budget = 0;
                entity.Gallons = viewModel.Gallons;
                entity.PricePerGallon = viewModel.PricePerGallon;
                entity.BasePrice = MoneyConverter.GetBaseAmount(viewModel.Currency, viewModel.PricePerGallon, viewModel.ExchangeRate);
            }
            else
            {
                entity.Budget = 0;
                entity.Gallons = 0;
                entity.PricePerGallon = 0;
            }

            entity.IsBudgetTracked = viewModel.IsBudgetTracked;
            entity.IsHedgeAmountTracked = viewModel.IsHedgeAmountTracked;
            if (entity.IsBudgetTracked)
            {
                entity.HedgeAmount = entity.IsHedgeAmountTracked ? viewModel.HedgeAmount : 0;
                entity.SpotAmount = !entity.IsHedgeAmountTracked ? viewModel.SpotAmount : 0;
                entity.BaseHedgeAmount = entity.IsHedgeAmountTracked ? VolumeConverter.GetBaseQuantity(viewModel.UoM, viewModel.HedgeAmount) : 0;
                entity.BaseSpotAmount = !entity.IsHedgeAmountTracked ? VolumeConverter.GetBaseQuantity(viewModel.UoM, viewModel.SpotAmount) : 0;
            }
            else
            {
                entity.HedgeAmount = 0;
                entity.SpotAmount = 0;
            }

            entity.IsExceededBudget = viewModel.IsExceededBudget;
            entity.IsTaxExempted = viewModel.IsTaxExempted;
            entity.IsDropPictureRequired = viewModel.IsDropPictureRequired;
            entity.IsAssetTracked = viewModel.IsAssetTracked;
            if (viewModel.IsAssetTracked)
            {
                entity.IsAssetDropStatusEnabled = viewModel.IsAssetDropStatusEnabled;
            }
            else
            {
                entity.IsAssetDropStatusEnabled = false;
            }
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;

            return entity;
        }

        public static JobBudget ToBudgetEntityFromTPO(this ThirdPartyOrderViewModel viewModel, JobBudget entity = null)
        {
            if (entity == null)
                entity = new JobBudget();

            entity.BudgetTypeId = 1; //Job level budget
            entity.BudgetCalculationTypeId = (int)BudgetCalculationType.NoBudget;
            entity.IsTankAvailable = false;
            entity.Budget = 0;
            entity.Gallons = 0;
            entity.PricePerGallon = 0;

            entity.IsBudgetTracked = false;
            entity.IsHedgeAmountTracked = false;

            entity.HedgeAmount = 0;
            entity.SpotAmount = 0;

            entity.IsExceededBudget = true;
            entity.IsTaxExempted = viewModel.IsTaxExempted;
            entity.IsDropPictureRequired = false;
            entity.IsAssetTracked = viewModel.IsAssetTracked;
            entity.IsAssetDropStatusEnabled = viewModel.IsAssetDropStatusEnabled;
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;

            entity.UoM = viewModel.AddressDetails.Country.UoM;
            entity.Currency = viewModel.AddressDetails.Country.Currency;

            return entity;
        }

        public static JobBudget ToBudgetEntityFromOffer(this OfferOrderViewModel viewModel, JobBudget entity = null)
        {
            if (entity == null)
                entity = new JobBudget();

            entity.BudgetTypeId = 1; //Job level budget
            entity.BudgetCalculationTypeId = (int)BudgetCalculationType.NoBudget;
            entity.IsTankAvailable = false;
            entity.Budget = 0;
            entity.Gallons = 0;
            entity.PricePerGallon = 0;

            entity.IsBudgetTracked = false;
            entity.IsHedgeAmountTracked = false;

            entity.HedgeAmount = 0;
            entity.SpotAmount = 0;

            entity.IsExceededBudget = true;
            entity.IsTaxExempted = false;
            entity.IsDropPictureRequired = false;
            entity.IsAssetTracked = true;
            entity.IsAssetDropStatusEnabled = true;
            entity.IsActive = true;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = DateTimeOffset.Now;

            entity.UoM = viewModel.AddressDetails.Country.UoM;
            entity.Currency = viewModel.AddressDetails.Country.Currency;

            return entity;
        }
    }
}
