using SiteFuel.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.BAL.Mappers
{
    public static class PricingRequestMapper
    {
        public static RequestPriceDetail ToEntity(this PricingRequestViewModel viewModel, RequestPriceDetail entity = null)
        {
            if (entity == null)
                entity = new RequestPriceDetail();

            entity.Id = viewModel.Id;
            entity.PricingTypeId = viewModel.PricingTypeId;
            // entity.BasePrice = viewModel.BasePrice;
            // entity.BaseSupplierCost = viewModel.BaseSupplierCost;
            entity.Currency = viewModel.Currency;
            entity.ExchangeRate = viewModel.ExchangeRate;
            // entity.Margin = viewModel.Margin;
            //entity.MarginTypeId = viewModel.MarginTypeId;
            // entity.PricePerGallon = viewModel.PricePerGallon;
            //if (viewModel.PricingCodeId == 0)  // temp code till migration runs to remove extra col from requestPricingDetails
            //{
            //    entity.PricingCodeId = 1;
            //}
            //else
            //{
            //    entity.PricingCodeId = viewModel.PricingCodeId;
            //}

            // entity.RackAvgTypeId = viewModel.RackAvgTypeId;
            // entity.SupplierCost = viewModel.SupplierCost;
            //entity.SupplierCostTypeId = viewModel.SupplierCostTypeId;
            entity.UoM = viewModel.UoM;
            if (viewModel.TierPricing != null)
            {
                entity.TierTypeId = (int)viewModel.TierPricing.TierPricingType;
                entity.PricingTypeId = viewModel.PricingTypeId;
                if (viewModel.TierPricing.IsResetCumulation && viewModel.TierPricing.TierPricingType == TierPricingType.VolumeBased)
                {
                    entity.CumulationTypeId = (int)viewModel.TierPricing.ResetCumulationSetting.CumulationType;
                    entity.CumulationResetDate = viewModel.TierPricing.ResetCumulationSetting.Date;
                    entity.CumulationResetDay = (int)viewModel.TierPricing.ResetCumulationSetting.Day;
                }

            }

            return entity;
        }

        public static PricingRequestViewModel ToViewModel(this RequestPriceDetail entity, PricingRequestViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new PricingRequestViewModel();

            viewModel.Id = entity.Id;
            //viewModel.BasePrice = entity.BasePrice;
            //viewModel.BaseSupplierCost = entity.BaseSupplierCost;
            viewModel.Currency = entity.Currency;
            viewModel.ExchangeRate = entity.ExchangeRate;
            // viewModel.Margin = entity.Margin;
            // viewModel.MarginTypeId = entity.MarginTypeId;
            // viewModel.PricePerGallon = entity.PricePerGallon;
            //viewModel.PricingCodeId = entity.PricingCodeId;
            // viewModel.RackAvgTypeId = entity.RackAvgTypeId;
            // viewModel.SupplierCost = entity.SupplierCost;
            // viewModel.SupplierCostTypeId = entity.SupplierCostTypeId;
            viewModel.UoM = entity.UoM;

            return viewModel;
        }

        public static List<PricingDetail> ToTierPriceDtlEntity(this TierPricingViewModel viewModel, List<PricingDetail> entityList = null)
        {
            if (entityList == null)
                entityList = new List<PricingDetail>();
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

                PricingDetail entity = new PricingDetail();
                if (item.PricingTypeId == (int)PricingType.Suppliercost)
                {
                    entity.SupplierCost = item.SupplierCostMarkupValue;
                    entity.SupplierCostTypeId = item.SupplierCostMarkupTypeId;
                    entity.BaseSupplierCost = item.BaseSupplierCost;
                }
                entity.RequestPriceDetailId = viewModel.RequestPriceDetailId;
                entity.PricingCodeId = item.PricingCode.Id;
                entity.RackAvgTypeId = item.RackAvgTypeId;
                entity.PricePerGallon = item.PricePerGallon ?? 0;

                entity.MarginTypeId = item.MarginTypeId;
                entity.Margin = item.Margin ?? 0;
                entity.BasePrice = item.BasePrice ?? 0;

                entity.MinQuantity = item.FromQuantity;
                entity.MaxQuantity = item.ToQuantity;
                lastAboveQuantity = item.ToQuantity;
                entity.CityRackTerminalId = item.CityGroupTerminalId;
                entity.IsActive = true;
                entity.TerminalId = item.TerminalId;
                entity.FuelTypeId = item.FuelTypeId;
                entity.ParameterJson = item.ParameterJson;
                entityList.Add(entity);

            }

            return entityList;
        }

        public static List<PricingDetail> ToPriceDtlEntity(this PricingRequestViewModel viewModel, List<PricingDetail> entityList = null)
        {
            if (entityList == null)
                entityList = new List<PricingDetail>();

            PricingDetail entity = new PricingDetail();
            entity.RequestPriceDetailId = viewModel.RequestPriceDetailId;
            entity.PricingCodeId = viewModel.PricingCodeId;
            entity.RackAvgTypeId = viewModel.RackAvgTypeId;
            entity.PricePerGallon = viewModel.PricePerGallon;
            entity.SupplierCost = viewModel.SupplierCost;
            entity.SupplierCostTypeId = viewModel.SupplierCostTypeId;
            entity.MarginTypeId = viewModel.MarginTypeId;
            entity.Margin = viewModel.Margin;
            entity.BasePrice = viewModel.BasePrice;
            entity.BaseSupplierCost = viewModel.BaseSupplierCost;
            entity.MinQuantity = 0;
            entity.MaxQuantity = 0;
            entity.TerminalId = viewModel.TerminalId;
            entity.FuelTypeId = viewModel.FuelTypeId;
            entity.CityRackTerminalId = viewModel.CityGroupTerminalId;
            entity.ParameterJson = viewModel.ParameterJson;
            entity.IsActive = true;
            entityList.Add(entity);
            return entityList;
        }

        public static List<CumulationDetail> ToCumulationDtlEntity(this CumulationSetting viewModel, List<CumulationDetail> entityList = null)
        {
            if (entityList == null)
                entityList = new List<CumulationDetail>();
            CumulationDetail entity = new CumulationDetail();
            entity.StartDate = DateTimeOffset.Now;
            entity.EndDate = viewModel.Date ?? DateTimeOffset.Now;
            entity.CumulatedQuantity = 0;
            entity.IsActive = true;
            entityList.Add(entity);
            return entityList;


        }
    }
}
