using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class SourcingTierPricingMapper
    {
        public static LeadRequestPriceDetails ToEntity(this SourcingPricingRequestViewModel viewModel, LeadRequestPriceDetails entity = null)
        {
            if (entity == null)
                entity = new LeadRequestPriceDetails();

            entity.Id = viewModel.Id;
            entity.PricingTypeId = viewModel.PricingTypeId;
            //entity.Currency = viewModel.Currency;
            entity.ExchangeRate = viewModel.ExchangeRate;
            entity.UoM = viewModel.UoM;
            if (viewModel.TierPricing != null)
            {
                entity.TierTypeId = (int)viewModel.TierPricing.TierPricingType;
                entity.PricingTypeId = viewModel.PricingTypeId;
                if (viewModel.TierPricing.IsResetCumulation && viewModel.TierPricing.TierPricingType == TierPricingType.VolumeBased)
                {
                    entity.CumulationTypeId = (int)viewModel.TierPricing.ResetCumulationSetting.CumulationType;
                    entity.CumulationResetDate = viewModel.TierPricing.ResetCumulationSetting.Date;
                    entity.CumulationResetDay = viewModel.TierPricing.ResetCumulationSetting.Day.HasValue ?(int)viewModel.TierPricing.ResetCumulationSetting.Day : 1;
                }

            }
            return entity;
        }
        public static List<LeadPricingDetail> ToTierPriceDtlEntity(this SourcingTierPricingViewModel viewModel, List<LeadPricingDetail> entityList = null)
        {
            if (entityList == null)
                entityList = new List<LeadPricingDetail>();
            decimal lastAboveQuantity = 0;
            decimal maxValue = 9999999999;
            if (viewModel.Pricings.Where(w => w.ToQuantity == 0).FirstOrDefault() != null)
                viewModel.Pricings.Where(w => w.ToQuantity == 0).FirstOrDefault().ToQuantity = maxValue;
            viewModel.Pricings = viewModel.Pricings.ToList().OrderBy(o => o.ToQuantity).ToList();

            foreach (var item in viewModel.Pricings)
            {
                if (item.IsAboveQuantity || item.ToQuantity == maxValue)
                {
                    item.FromQuantity = lastAboveQuantity;
                    item.ToQuantity = 0;
                }

                LeadPricingDetail entity = new LeadPricingDetail();
                if (item.PricingTypeId == (int)PricingType.Suppliercost)
                {
                    entity.SupplierCostMarkupValue = item.SupplierCostMarkupValue ?? 0;
                    entity.SupplierCostMarkupTypeId = item.SupplierCostMarkupTypeId;
                }
                entity.RequestPriceDetailId = viewModel.RequestPriceDetailId;
                entity.CodeId = item.PricingCode.Id;
                entity.Code = item.PricingCode.Code;
                entity.CodeDescription = item.PricingCode.Description;
                entity.RackAvgTypeId = item.RackAvgTypeId;
                entity.PricePerGallon = Convert.ToDecimal(item.PricePerGallon);
                entity.PricingTypeId = item.PricingTypeId;
                entity.RackPrice = item.RackPrice ?? 0;
                entity.MinQuantity = item.FromQuantity;
                entity.MaxQuantity = item.ToQuantity;
                lastAboveQuantity = item.ToQuantity;
                entity.CityGroupTerminalId = item.CityGroupTerminalId;
                entity.TerminalId = item.TerminalId;
                entity.TerminalName = item.TerminalName;
                entityList.Add(entity);

            }

            return entityList;
        }
        public static List<LeadCumulationDetail> ToCumulationDtlEntity(this CumulationSetting viewModel, List<LeadCumulationDetail> entityList = null)
        {
            if (entityList == null)
                entityList = new List<LeadCumulationDetail>();
            LeadCumulationDetail entity = new LeadCumulationDetail();
            entity.StartDate = DateTimeOffset.Now;
            entity.EndDate = viewModel.Date ?? DateTimeOffset.Now;
            entity.CumulatedQuantity = 0;
            entity.IsActive = true;
            entityList.Add(entity);
            return entityList;
        }
    }
}
