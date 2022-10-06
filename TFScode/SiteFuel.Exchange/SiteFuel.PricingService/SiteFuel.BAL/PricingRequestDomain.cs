using SiteFuel.DAL;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using SiteFuel.BAL.Mappers;
using System.Threading.Tasks;
using SiteFuel.DataAccess.Entities;
using System.Text;
using System.Linq;

namespace SiteFuel.BAL
{
    public class PricingRequestDomain : IPricingRequestDomain
    {
        IPricingRequestRepository _pricingReqRepository;
        public PricingRequestDomain(IPricingRequestRepository pricingReqRepository)
        {
            _pricingReqRepository = pricingReqRepository;
        }

        public async Task<CustomResponseModel> SaveRequestDetails(PricingRequestViewModel pricingrequestviewmodel)
        {

            var entity = pricingrequestviewmodel.ToEntity();

            if (pricingrequestviewmodel.PricingTypeId == (int)PricingType.Tier)
            {
                entity.PricingDetails = pricingrequestviewmodel.TierPricing.ToTierPriceDtlEntity();
                if (pricingrequestviewmodel.TierPricing.IsResetCumulation && pricingrequestviewmodel.TierPricing.TierPricingType == TierPricingType.VolumeBased)
                    entity.CumulationDetails = pricingrequestviewmodel.TierPricing.ResetCumulationSetting.ToCumulationDtlEntity();
            }

            else
                entity.PricingDetails = pricingrequestviewmodel.ToPriceDtlEntity();

            var response = await _pricingReqRepository.SaveRequestDetails(entity);

            if (pricingrequestviewmodel.PricingTypeId == (int)PricingType.Tier)
            {
                string custString1 = "";
                string custString2 = "";

                if (entity.TierTypeId == (int)TierPricingType.VolumeBased)
                    // custString1 += Enum.GetName(typeof(TierPricingType), (int)TierPricingType.VolumeBased);
                    custString1 += TierPricingType.VolumeBased.GetDisplayName();
                else if (entity.TierTypeId == (int)TierPricingType.DeliveryQuantityBased)
                {
                    // custString1 += Enum.GetName(typeof(TierPricingType), (int)TierPricingType.DeliveryQuantityBased);
                    custString1 += TierPricingType.DeliveryQuantityBased.GetDisplayName();
                }
                foreach (var item in entity.PricingDetails)
                {

                    await GetDisplayPrice(item, response);
                    //custString1 += " , "+ response.CustomString1;
                    custString2 += "" + response.CustomString2;
                }
                response.CustomString1 = custString1 + " Tier";
                response.CustomString2 = custString2;
            }
            else

                await GetDisplayPrice(entity.PricingDetails.Where(t => t.IsActive).FirstOrDefault(), response);
            return response;
        }

        public async Task<CustomResponseModel> UpdateRequestDetails(PricingRequestViewModel pricingrequestviewmodel)
        {
            var entity = await _pricingReqRepository.GetPricingRequestDetailByIdAsync(pricingrequestviewmodel.Id);
            entity = pricingrequestviewmodel.ToEntity(entity);

            //if (pricingrequestviewmodel.PricingTypeId == (int)PricingType.Tier)
            //{
            //    entity.PricingDetails = pricingrequestviewmodel.TierPricing.ToTierPriceDtlEntity();
            //    if (pricingrequestviewmodel.TierPricing.IsResetCumulation && pricingrequestviewmodel.TierPricing.TierPricingType == TierPricingType.VolumeBased)
            //        entity.CumulationDetails = pricingrequestviewmodel.TierPricing.ResetCumulationSetting.ToCumulationDtlEntity();
            //}

            //else

            if (pricingrequestviewmodel.PricingTypeId != (int)PricingType.Tier)
            {
                entity.PricingDetails = pricingrequestviewmodel.ToPriceDtlEntity();
            }

            var response = await _pricingReqRepository.UpdateRequestDetails(entity);

            //if (pricingrequestviewmodel.PricingTypeId == (int)PricingType.Tier)
            //{
            //    string custString1 = "";
            //    string custString2 = "";

            //    if (entity.TierTypeId == (int)TierPricingType.VolumeBased)
            //        // custString1 += Enum.GetName(typeof(TierPricingType), (int)TierPricingType.VolumeBased);
            //        custString1 += TierPricingType.VolumeBased.GetDisplayName();
            //    else if (entity.TierTypeId == (int)TierPricingType.DeliveryQuantityBased)
            //    {
            //        // custString1 += Enum.GetName(typeof(TierPricingType), (int)TierPricingType.DeliveryQuantityBased);
            //        custString1 += TierPricingType.DeliveryQuantityBased.GetDisplayName();
            //    }
            //    foreach (var item in entity.PricingDetails)
            //    {

            //        await GetDisplayPrice(item, response);
            //        //custString1 += " , "+ response.CustomString1;
            //        custString2 += "" + response.CustomString2;
            //    }
            //    response.CustomString1 = custString1 + " Tier";
            //    response.CustomString2 = custString2;
            //}
            //else

            await GetDisplayPrice(entity.PricingDetails.Where(t => t.IsActive).FirstOrDefault(), response);
            return response;
        }

        public async Task<CustomResponseModel> UpdateSourceRegion(SourceRegionPricingRequestModel model)
        {
            var response = await _pricingReqRepository.UpdateSourceRegion(model);
            return response;
        }

        public async Task<List<int>> GetPriceDetailIdsBySourceAsync(RequestPriceBySourceInputViewModel inputModel)
        {
            List<int> response = new List<int>();
            try
            {
                response = await _pricingReqRepository.GetRequestPriceDetailIdsByPricingSourceAsync(inputModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRequestDomain", "GetPriceDetailIdsBySourceAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<IntResponseModel> GetFilterPriceDetailsByPricingType(FilterPricingRequestViewModel requestModel)
        {
            IntResponseModel response = new IntResponseModel();
            try
            {
                response = await _pricingReqRepository.GetFilterPriceDetailsByPricingType(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRequestDomain", "GetFilterPriceDetailsByPricingType", ex.Message, ex);
            }
            return response;
        }


        private async Task GetDisplayPrice(PricingDetail entity, CustomResponseModel response)
        {
            string price = string.Empty;
            var codeDetails = await _pricingReqRepository.GetCodeDetails(entity.PricingCodeId);
            if (codeDetails != null)
            {
                try
                {
                    //if (codeDetails.PricingTypeId == (int)PricingType.Tier)
                    //{
                    //    price = Resource.lblTier;
                    //    if (entity.TierTypeId == (int)TierPricingType.VolumeBased)
                    //    {
                    //        price += Enum.GetName(typeof(TierPricingType), (int)TierPricingType.VolumeBased);
                    //    }
                    //    else if(entity.TierTypeId == (int)TierPricingType.DeliveryQuantityBased)
                    //    {
                    //        price += Enum.GetName(typeof(TierPricingType), (int)TierPricingType.DeliveryQuantityBased);
                    //    }
                    //}else
                    if (codeDetails.PricingTypeId == (int)PricingType.PricePerGallon)
                    {
                        price = Resource.constSymbolCurrency + entity.PricePerGallon.GetPreciseValue(4);
                    }
                    else if (codeDetails.PricingTypeId == (int)PricingType.Suppliercost)
                    {
                        switch (entity.RackAvgTypeId)
                        {
                            case (int)RackPricingType.PlusPercent:
                                price = $"{Resource.lblFuelCostPlus} {entity.PricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                                break;
                            case (int)RackPricingType.MinusPercent:
                                price = $"{Resource.lblFuelCostMinus} {entity.PricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                                break;
                            case (int)RackPricingType.PlusDollar:
                                price = $"{Resource.lblFuelCostPlus} {Resource.constSymbolCurrency}{entity.PricePerGallon.GetPreciseValue(4)}";
                                break;
                            case (int)RackPricingType.MinusDollar:
                                price = $"{Resource.lblFuelCostMinus} {Resource.constSymbolCurrency}{entity.PricePerGallon.GetPreciseValue(4)}";
                                break;
                        }
                    }
                    else
                    {
                        var rackText = codeDetails.RackTypeId == (int)PricingType.RackHigh ? Resource.lblRackHigh : codeDetails.RackTypeId == (int)PricingType.RackLow ? Resource.lblRackLow : Resource.lblRackAverage;

                        switch (entity.RackAvgTypeId)
                        {
                            case (int)RackPricingType.PlusPercent:
                                price = $"{rackText} + {entity.PricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                                break;
                            case (int)RackPricingType.MinusPercent:
                                price = $"{rackText} - {entity.PricePerGallon.GetPreciseValue(4)}{Resource.constSymbolPercent}";
                                break;
                            case (int)RackPricingType.PlusDollar:
                                price = $"{rackText} + {Resource.constSymbolCurrency}{entity.PricePerGallon.GetPreciseValue(4)}";
                                break;
                            case (int)RackPricingType.MinusDollar:
                                price = $"{rackText} - {Resource.constSymbolCurrency}{entity.PricePerGallon.GetPreciseValue(4)}";
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("PricingRequestDomain", "GetDisplayPrice", ex.Message, ex);
                }
            }
            response.CustomString1 = String.IsNullOrEmpty(price) ? Resource.constSymbolCurrency + entity.PricePerGallon.GetPreciseValue(4) : price;
            response.CustomString2 = GetCodeDescription(codeDetails);
        }

        private string GetCodeDescription(MstPricingCode codeDetails)
        {
            var strBuilder = new StringBuilder();

            strBuilder.Append(((PricingSource)codeDetails.PricingSourceId).ToString());
            if (codeDetails.PricingTypeId == (int)PricingType.PricePerGallon)
                strBuilder.Append(", " + "Fixed");
            else if (codeDetails.PricingTypeId == (int)PricingType.Suppliercost)
                strBuilder.Append(", " + PricingType.Suppliercost.GetDisplayName());
            else if (codeDetails.PricingTypeId == (int)PricingType.Tier)
                strBuilder.Append(", " + PricingType.Tier.GetDisplayName());

            if (codeDetails.RackTypeId > 0)
                strBuilder.Append($", {((PricingType)codeDetails.RackTypeId).GetDisplayName()}");
            if (codeDetails.FeedTypeId > 0)
                strBuilder.Append($", {((PricingSourceFeedTypes)codeDetails.FeedTypeId).GetDisplayName()}");
            if (codeDetails.QuantityIndicatorId > 0)
                strBuilder.Append($", {((QuantityIndicatorTypes)codeDetails.QuantityIndicatorId).GetDisplayName()}");
            if (codeDetails.FuelClassTypeId > 0)
                strBuilder.Append($", {((FuelClassTypes)codeDetails.FuelClassTypeId).GetDisplayName()}");
            if (codeDetails.WeekendPricingTypeId > 0)
                strBuilder.Append($", {((WeekendDropPricingDay)codeDetails.WeekendPricingTypeId).GetDisplayName()}");

            return strBuilder.ToString();
        }

        public async Task<PricingCodesResponseModel> GetPricingCodesAsync(PricingCodesRequestModel requestModel)
        {
            PricingCodesResponseModel response = new PricingCodesResponseModel();
            try
            {
                response = await _pricingReqRepository.GetPricingCodesAsync(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRequestDomain", "GetPricingCodesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<PricingRequestDetailResponseModel> GetPricingRequestDetailByIdAsync(PricingRequestViewModel requestModel)
        {
            var response = new PricingRequestDetailResponseModel();
            try
            {
                response = await _pricingReqRepository.GetPricingRequestDetailByIdAsync(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRequestDomain", "GetPricingRequestDetailByIdAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<PricingDetailResponseModelForExchangeAPI> GetPricingDetailsByIdList(List<int> requestPriceDetailIds)
        {
            var response = new PricingDetailResponseModelForExchangeAPI();
            try
            {
                response = Task.Run(() => _pricingReqRepository.GetPricingDetailsByIdList(requestPriceDetailIds)).Result;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRequestDomain", "GetPricingDetailsByIdList", ex.Message, ex);
            }
            return response;
        }
    }
}
