using SiteFuel.DAL;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class PricingDomain : IPricingDomain
    {
        readonly IPricingRepository _pricingRepository;

        public PricingDomain(IPricingRepository PricingRepository)
        {
            _pricingRepository = PricingRepository;
        }

        public async Task<PricingResponseModel> GetTerminalPriceAsync(PriceRequestModel requestModel, int? pricingSourceId = null)
        {
            PricingResponseModel response = null;
            if (pricingSourceId == null)
            {
                pricingSourceId = await _pricingRepository.GetSourceFromPriceCodeAsync(requestModel.PricingCodeId);
            }
            switch (pricingSourceId)
            {
                case (int)PricingSource.Axxis:
                    response = await GetTerminalPriceFromAxxisAsync(requestModel);
                    break;
                case (int)PricingSource.OPIS:
                    response = await GetTerminalPriceFromOpisAsync(requestModel);
                    break;
                case (int)PricingSource.PLATTS:
                    response = await GetTerminalPriceFromPlattsAsync(requestModel);
                    break;
                default:
                    response = new PricingResponseModel(Status.Failed) { Message = Resource.valMessagePricingSourceIdNotSupported };
                    break;
            }
            return response;
        }

        public async Task<PricingResponseModel> GetTerminalPriceAsync(int productId, int terminalId)
        {
            PricingResponseModel response = null;
            try
            {
                response = await _pricingRepository.GetTerminalPriceAsync(productId, terminalId);
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetTerminalPriceAsync", ex.Message + " TerminalId:" + terminalId + " ProductId:" + productId, ex);
            }
            return response;
        }

        public async Task<List<FuelPricingResponseModel>> GetFuelPriceAsync(List<FuelPriceRequestModel> requestModels)
        {
            List<FuelPricingResponseModel> response = new List<FuelPricingResponseModel>();
            try
            {
                var requestPriceDetailsIds = requestModels.Select(t => t.RequestPriceDetailId).ToList();
                var requestPriceDetails = await _pricingRepository.GetRequestPriceDetailsAsync(requestPriceDetailsIds);
                foreach (var requestModel in requestModels)
                {
                    var requestPriceDetail = requestPriceDetails?.FirstOrDefault(t => t.RequestPriceDetailId == requestModel.RequestPriceDetailId);
                    if (requestPriceDetail != null)
                    {
                        var priceResponse = await GetFuelPriceAsync(requestModel, requestPriceDetail);
                        response.AddRange(priceResponse);
                    }
                    else
                    {
                        LogManager.Logger.WriteException("PricingDomain", "GetFuelPriceAsync", $"RequestPriceDetailId => {requestModel.RequestPriceDetailId} not found.", new Exception());
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "GetFuelPriceAsync", ex.Message, ex);
            }

            return response;
        }

        private async Task<List<FuelPricingResponseModel>> GetFuelPriceAsync(FuelPriceRequestModel requestModel, RequestPriceDetailModel requestPriceDetails)
        {
            List<FuelPricingResponseModel> responseList = new List<FuelPricingResponseModel>();

            FuelPricingResponseModel response = new FuelPricingResponseModel(Status.Failed);
            try
            {
                response.PricingTypeId = requestPriceDetails.PricingTypeId;
                switch (requestPriceDetails.PricingTypeId)
                {
                    //assuming Marine orders are only in Fixed pricing and we allow edit/override pricing for marine only.
                    case (int)PricingType.PricePerGallon:
                        response.PricePerGallon = requestModel.PricePerGallonToOverride.HasValue && requestModel.PricePerGallonToOverride.Value > 0 ? 
                                                    requestModel.PricePerGallonToOverride.Value : 
                                                    requestPriceDetails.PricePerGallon;
                        response.WaitingFor = requestModel.WaitingFor;
                        break;
                    case (int)PricingType.Suppliercost:
                        response.FuelCost = requestPriceDetails.SupplierCost;
                        response.FuelCostTypeId = requestPriceDetails.SupplierCostTypeId;
                        var supplierCost = requestModel.SupplierCost.HasValue && requestModel.SupplierCost > 0 ? requestModel.SupplierCost.Value : requestPriceDetails.SupplierCost ?? 0;
                        response.PricePerGallon = GetCalculatedPricePerGallon(supplierCost, requestPriceDetails.PricePerGallon, requestPriceDetails.RackAvgTypeId ?? 0);
                        response.WaitingFor = requestModel.WaitingFor;
                        break;
                    case (int)PricingType.Tier:
                        await GetFuelPriceForTier(requestModel, requestPriceDetails, responseList);
                        break;
                    default:
                        await GetFuelPriceForRackType(requestModel, requestPriceDetails, response);
                        break;
                }

                if (requestPriceDetails.PricingTypeId != (int)PricingType.Tier)
                {
                    response.Status = Status.Success;
                    response.Guid = requestModel.Guid;
                    responseList.Add(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetFuelPriceAsync", ex.Message + " requestPriceDetailId: " + requestPriceDetails.RequestPriceDetailId + " PricingTypeId: " + requestPriceDetails.PricingTypeId + " TerminalId:" + requestModel.TerminalId + " CityGroupTerminalId:" + requestModel.CityGroupTerminalId + " ProductId:" + requestModel.ProductId + " PricingDate:" + requestModel.PriceDate, ex);
            }
            return responseList;
        }

        public async Task<SalesCalculatorResponseModel> GetClosestTerminalPriceAsync(SalesCalculatorRequestModel requestModel)
        {
            SalesCalculatorResponseModel response = new SalesCalculatorResponseModel(Status.Failed);
            try
            {
                if (ValidateAxxisInputDataForCalculator(requestModel, response))
                {
                    response.TerminalPrices = await _pricingRepository.GetClosestTerminalPriceAsync(requestModel);
                    response.Status = Status.Success;
                }
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetClosestTerminalPriceAsync", ex.Message + " TerminalId:" + requestModel.CityGroupTerminalIds.FirstOrDefault() + " ProductId:" + requestModel.ProductId + " PricingDate:" + requestModel.PriceDate, ex);
            }
            return response;
        }

        public async Task<SalesCalculatorResponseModel> GetOpisTerminalPricesForCalculatorAsync(SalesCalculatorRequestModel requestModel)
        {
            var response = new SalesCalculatorResponseModel(Status.Failed);
            try
            {
                response.TerminalPrices = await _pricingRepository.GetOpisTerminalPricesForCalculatorAsync(requestModel);
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetOpisTerminalPricesForCalculatorAsync", ex.Message + " TerminalId:" + requestModel.CityGroupTerminalIds.FirstOrDefault() + " ProductId:" + requestModel.ProductId + " PricingDate:" + requestModel.PriceDate, ex);
            }
            return response;
        }

        public async Task<SalesCalculatorResponseModel> GetPlattsTerminalPricesForCalculatorAsync(SalesCalculatorRequestModel requestModel)
        {
            var response = new SalesCalculatorResponseModel(Status.Failed);
            try
            {
                response.TerminalPrices = await _pricingRepository.GetPlattsTerminalPricesForCalculatorAsync(requestModel);
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetPlattsTerminalPricesForCalculatorAsync", ex.Message + " TerminalId:" + requestModel.CityGroupTerminalIds.FirstOrDefault() + " ProductId:" + requestModel.ProductId + " PricingDate:" + requestModel.PriceDate, ex);
            }
            return response;
        }

        public async Task<SalesCalculatorResponseModel> GetCityRackTerminalPricesForCalculator(CityRackPricesRequestModel requestModel)
        {
            var response = new SalesCalculatorResponseModel(Status.Failed);
            try
            {
                response.TerminalPrices = await _pricingRepository.GetCityRackTerminalPricesForCalculator(requestModel);
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetCityRackTerminalPricesForCalculator", ex.Message + " TerminalIds:" + requestModel.StateOrTerminalIds.FirstOrDefault() + " ProductId:" + requestModel.ExternalProductId + " PricingDate:" + requestModel.PriceDate, ex);
            }
            return response;
        }

        public async Task<SalesCalculatorResponseModel> GetTerminalPricesForAuditAsync(TerminalPricesRequestModel requestModel)
        {
            var response = new SalesCalculatorResponseModel(Status.Failed);
            try
            {
                response.TerminalPrices = await _pricingRepository.GetTerminalPricesForAuditAsync(requestModel);
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetTerminalPricesForAuditAsync", ex.Message + " ProductId:" + requestModel.ExternalProductId + " PricingDate:" + requestModel.PricingDate, ex);
            }
            return response;
        }

        public async Task<TerminalResponseModel> GetClosestTerminalsAsync(TerminalRequestModel requestModel)
        {
            var response = new TerminalResponseModel(Status.Failed);
            try
            {
                response.Terminals = await _pricingRepository.GetClosestTerminalsAsync(requestModel);
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetClosestTerminalsAsync", ex.Message + " ProductId:" + requestModel.ProductId + " SerachTerminal:" + requestModel.SearchStringTeminal, ex);
            }
            return response;
        }

        public async Task<DateTime> GetLastUpdatedPricingDate(int requestPriceDetailId)
        {
            DateTime response = await _pricingRepository.GetLastUpdatedPricingDate(requestPriceDetailId);
            return response;
        }

        public async Task<PricingConfigResponse> GetPricingConfigAsync(List<string> keys)
        {
            var response = new PricingConfigResponse(Status.Failed);
            try
            {
                response = await _pricingRepository.GetPricingConfigAsync(keys);
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetPricingConfigAsync", ex.Message, ex);
            }
            return response;
        }


        public async Task<IntResponseModel> SyncAxxisPricingData()
        {
            IntResponseModel response = new IntResponseModel();
            try
            {
                response = await _pricingRepository.ExecuteStoredProcedureScalar("usp_SyncExternalPricingData", 60);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "SyncAxxisPricingDataAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<PricingResponseModel> GetAxxisTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel)
        {
            var response = new PricingResponseModel(Status.Failed);
            try
            {
                var spResponse = await _pricingRepository.GetAxxisTerminalPriceForCurrentDateAsync(requestModel);
                if (spResponse != null)
                {
                    response = spResponse;
                }
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetAxxisTerminalPriceForCurrentDateAsync", ex.Message + " TerminalId:" + requestModel.TerminalId.Value + " ProductId:" + requestModel.ProductId, ex);
            }
            return response;
        }

        public async Task<PricingResponseModel> GetOpisTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel)
        {
            var response = new PricingResponseModel(Status.Failed);
            try
            {
                var spResponse = await _pricingRepository.GetOpisTerminalPriceForCurrentDateAsync(requestModel);
                if (spResponse != null)
                {
                    return spResponse;
                }
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetOpisTerminalPriceForCurrentDateAsync", ex.Message + " TerminalId:" + requestModel.TerminalId + " ProductId:" + requestModel.ProductId, ex);
            }
            return response;
        }

        public async Task<PricingResponseModel> GetPlattsTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel)
        {
            var response = new PricingResponseModel(Status.Failed);
            try
            {
                var spResponse = await _pricingRepository.GetPlattsTerminalPriceForCurrentDateAsync(requestModel);
                if (spResponse != null)
                {
                    return spResponse;
                }
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetPlattsTerminalPriceForCurrentDateAsync", ex.Message + " TerminalId:" + requestModel.TerminalId + " ProductId:" + requestModel.ProductId, ex);
            }
            return response;
        }

        public async Task<SyncPricingResponseModel> SyncOpisPlattsPricingData()
        {
            SyncPricingResponseModel response = new SyncPricingResponseModel();
            try
            {
                response = await _pricingRepository.SyncExternalSourcePricing(600);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "SyncOpisPlattsPricingDataAsync", ex.Message, ex);
            }
            return response;
        }


        public async Task<BooleanResponseModel> IsAxxisCityRackPriceAvailable(int productId, int cityGroupTerminalId, DateTime effectiveDate)
        {
            BooleanResponseModel response = new BooleanResponseModel(Status.Failed);
            try
            {
                response.Result = await _pricingRepository.IsAxxisCityRackPriceAvailable(productId, cityGroupTerminalId, effectiveDate);
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "IsAxxisCityRackPriceAvaiable", ex.Message, ex);
            }
            return response;
        }

        public async Task<BooleanResponseModel> IsOpisCityRackPriceAvailable(int productId, int cityGroupTerminalId)
        {
            BooleanResponseModel response = new BooleanResponseModel(Status.Failed);
            try
            {
                response.Result = await _pricingRepository.IsOpisCityRackPriceAvailable(productId, cityGroupTerminalId);
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "IsOpisCityRackPriceAvailable", ex.Message, ex);
            }
            return response;
        }

        public async Task<BooleanResponseModel> IsPlattsCityRackPriceAvailable(int productId, int cityGroupTerminalId)
        {
            BooleanResponseModel response = new BooleanResponseModel(Status.Failed);
            try
            {
                response.Result = await _pricingRepository.IsPlattsCityRackPriceAvailable(productId, cityGroupTerminalId);
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "IsPlattsCityRackPriceAvailable", ex.Message, ex);
            }
            return response;
        }

        public async Task<ProductDetailsResponseModel> GetAxxisProductDetailsAsync(ProductDetailsRequestModel requestModel)
        {
            ProductDetailsResponseModel response = new ProductDetailsResponseModel(Status.Failed);
            try
            {
                response.ProductDetails = await _pricingRepository.GetAxxisProductDetailsAsync(requestModel);
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetAxxisProductDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<TerminalResponseModel> GetClosestTerminalsByDistanceAsync(TerminalRequestViewModel requestModel)
        {
            var response = new TerminalResponseModel(Status.Failed);
            try
            {
                response.Terminals = await _pricingRepository.GetClosestTerminalsByDistanceAsync(requestModel);
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetClosestTerminalsByDistanceAsync", ex.Message + " ProductId:" + requestModel.ProductId, ex);
            }
            return response;
        }
        private async Task<PricingResponseModel> GetTerminalPriceFromAxxisAsync(PriceRequestModel requestModel)
        {
            var response = new PricingResponseModel(Status.Failed);
            try
            {
                if (ValidateAxxisInputData(requestModel, response))
                {
                    var spResponse = await _pricingRepository.GetAxxisPricingDataAsync(requestModel.TerminalId.Value, requestModel.CityGroupTerminalId, requestModel.ProductId, requestModel.PriceDate, requestModel.PricingCodeId);
                    if (spResponse != null)
                    {
                        response = spResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetAxxisPricingDataAsync", ex.Message + " TerminalId:" + requestModel.TerminalId.Value + " ProductId:" + requestModel.ProductId + " PricingDate:" + requestModel.PriceDate, ex);
            }
            return response;
        }

        private async Task<PricingResponseModel> GetTerminalPriceFromOpisAsync(PriceRequestModel requestModel)
        {
            var response = new PricingResponseModel(Status.Failed);
            try
            {
                var spResponse = await _pricingRepository.GetTerminalPriceForOpisAsync(requestModel);
                if (spResponse != null)
                {
                    return spResponse;
                }
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetTerminalPriceFromOpisAsync", ex.Message + " TerminalId:" + requestModel.TerminalId + " ProductId:" + requestModel.ProductId + " PricingDate:" + requestModel.PriceDate, ex);
            }
            return response;
        }

        private async Task<PricingResponseModel> GetTerminalPriceFromPlattsAsync(PriceRequestModel requestModel)
        {
            var response = new PricingResponseModel(Status.Failed);
            try
            {
                var spResponse = await _pricingRepository.GetTerminalPriceForPlattsAsync(requestModel.CityGroupTerminalId, requestModel.ProductId, requestModel.PriceDate, requestModel.PricingCodeId);
                if (spResponse != null)
                {
                    return spResponse;
                }
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetTerminalPriceFromPlattsAsync", ex.Message + " TerminalId:" + requestModel.TerminalId + " ProductId:" + requestModel.ProductId + " PricingDate:" + requestModel.PriceDate, ex);
            }
            return response;
        }

        private bool ValidateAxxisInputData(PriceRequestModel requestModel, PricingResponseModel responseModel)
        {
            if (!requestModel.TerminalId.HasValue)
            {
                responseModel.Message = Resource.valMsgTerminalIdRequired;
                return false;
            }
            return true;
        }

        private bool ValidateAxxisInputDataForCalculator(SalesCalculatorRequestModel requestModel, SalesCalculatorResponseModel responseModel)
        {
            List<string> errors = new List<string>();
            if (!requestModel.ProductId.HasValue)
            {
                errors.Add(Resource.valMsgProductIdRequired);
            }
            if (!requestModel.SrcLatitude.HasValue)
            {
                errors.Add(Resource.valMsgSrcLatitudeRequired);
            }
            if (!requestModel.SrcLongitude.HasValue)
            {
                errors.Add(Resource.valMsgSrcLongitudeRequired);
            }
            if (requestModel.SrcLatitude == 0 && requestModel.SrcLongitude == 0)
            {
                errors.Add(Resource.valMsgInvalidLatLong);
            }
            if (!requestModel.RecordCount.HasValue)
            {
                errors.Add(Resource.valMsgRecordCountRequired);
            }
            if (String.IsNullOrEmpty(requestModel.CountryCode))
            {
                errors.Add(Resource.valMsgCountryCodeRequired);
            }
            if (errors.Any())
            {
                responseModel.Message = string.Join(";", errors);
                return false;
            }
            return true;
        }

        public async Task<IntResponseModel> AddNewProduct(ProductRequestModel product)
        {
            IntResponseModel response = new IntResponseModel();
            try
            {
                if (product.Id > 0)
                    response = await _pricingRepository.UpdateProductDetails(product);
                else
                    response = await _pricingRepository.AddNewProduct(product);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "AddNewProduct", ex.Message, ex);
            }
            return response;
        }

        public async Task<IntResponseModel> SaveTerminalDetails(PickupLocationDetailViewModel terminal)
        {
            IntResponseModel response = new IntResponseModel();
            try
            {
                    response = await _pricingRepository.SaveTerminalDetails(terminal);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "SaveTerminalDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<IntResponseModel> AddNewTfxProduct(ProductRequestModel product)
        {
            IntResponseModel response = new IntResponseModel();
            try
            {
                response = await _pricingRepository.AddNewTfxProduct(product);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "AddNewTfxProduct", ex.Message, ex);
            }
            return response;
        }

        public async Task<IntResponseModel> UpdateTfxProduct(ProductRequestModel product)
        {
            IntResponseModel response = new IntResponseModel();
            try
            {
                response = await _pricingRepository.UpdateTfxProduct(product);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "UpdateTfxProduct", ex.Message, ex);
            }
            return response;
        }

        private decimal GetCalculatedPricePerGallon(decimal rackPrice, decimal variablePrice, int rackAvgTypeId)
        {
            var response = rackPrice;

            if (rackAvgTypeId == (int)RackPricingType.PlusDollar)
            {
                response = rackPrice + variablePrice;
            }
            else if (rackAvgTypeId == (int)RackPricingType.MinusDollar)
            {
                response = rackPrice - variablePrice;
            }
            else if (rackAvgTypeId == (int)RackPricingType.PlusPercent)
            {
                response = rackPrice + (rackPrice / 100 * variablePrice);
            }
            else if (rackAvgTypeId == (int)RackPricingType.MinusPercent)
            {
                response = rackPrice - (rackPrice / 100 * variablePrice);
            }
            return response;
        }

        private async Task GetFuelPriceForTier(FuelPriceRequestModel requestModel, RequestPriceDetailModel requestPriceDetails, List<FuelPricingResponseModel> responseList)
        {
            var getTiersForQuantity = (requestModel.TierMaxQuantity == 0 && requestModel.TierMinQuantity == 0) ? requestModel.DroppedQuantity : 0;

            var tierPricingDetails = await _pricingRepository.GetTierPricingReqModel(requestModel.RequestPriceDetailId, getTiersForQuantity);
            if (tierPricingDetails.FirstOrDefault().TierTypeId == (int)TierPricingType.DeliveryQuantityBased)
            {
                if (requestModel.DroppedQuantity > 0)
                    tierPricingDetails = tierPricingDetails.Where(t => t.MinQuantity < requestModel.DroppedQuantity
                                                                    && (requestModel.DroppedQuantity <= t.MaxQuantity || t.MaxQuantity == 0)).ToList();
            }
            
            //USED FOR DDT TO INVOICE CONVERSION
            if (!(requestModel.TierMaxQuantity == 0 && requestModel.TierMinQuantity == 0))
            {
                if (requestModel.TierMaxQuantity != 0)
                    tierPricingDetails = tierPricingDetails.Where(t => t.MinQuantity <= requestModel.TierMinQuantity
                                                                        && (requestModel.TierMaxQuantity <= t.MaxQuantity || t.MaxQuantity == 0)).ToList();
                else
                    tierPricingDetails = tierPricingDetails.Where(t => t.MaxQuantity == 0).ToList();
            }
            else if (tierPricingDetails.FirstOrDefault().TierTypeId == (int)TierPricingType.VolumeBased)
            {
                if (tierPricingDetails.Any(t => t.CumulatedQuantity > 0))
                {
                    var cumulatedQty = tierPricingDetails.FirstOrDefault().CumulatedQuantity;//fromQty
                    var totalDroppedGallons = cumulatedQty + requestModel.DroppedQuantity;//toQty
                     tierPricingDetails = await _pricingRepository.GetPricingForVolBasedTierWithCumulationReset(requestModel.RequestPriceDetailId, cumulatedQty, totalDroppedGallons); 
                    //tierPricingDetails = await _pricingRepository.GetTierPricingReqModel(requestModel.RequestPriceDetailId, cumulatedQty);
                    //if (tierPricingDetails.Count >1)
                    //{
                    //    tierPricingDetails.RemoveAll(t => t.MaxQuantity <= cumulatedQty && t.MaxQuantity > 0);                        
                    //}
                    tierPricingDetails.OrderBy(t => t.MinQuantity).FirstOrDefault().MinQuantity = cumulatedQty;
                }
            }

            foreach (var tierData in tierPricingDetails)
            {
                FuelPricingResponseModel response = new FuelPricingResponseModel(Status.Failed);
                try
                {
                    response.PricingTypeId = tierData.PricingTypeId;
                    switch (tierData.PricingTypeId)
                    {
                        case (int)PricingType.PricePerGallon:
                            response.PricePerGallon = tierData.PricePerGallon;
                            response.WaitingFor = requestModel.WaitingFor;
                            break;
                        case (int)PricingType.Suppliercost:
                            response.FuelCost = tierData.SupplierCost;
                            response.FuelCostTypeId = tierData.SupplierCostTypeId;
                            var supplierCost = requestModel.SupplierCost.HasValue && requestModel.SupplierCost > 0 ? requestModel.SupplierCost.Value : tierData.SupplierCost ?? 0;
                            response.PricePerGallon = GetCalculatedPricePerGallon(supplierCost, tierData.PricePerGallon, tierData.RackAvgTypeId ?? 0);
                            response.WaitingFor = requestModel.WaitingFor;
                            break;
                        default:
                            PriceRequestModel priceRequest = GetPriceRequestModel(requestModel, tierData.PricingCodeId);

                            if ((tierData.TerminalId.HasValue || tierData.CityGroupTerminalId.HasValue))
                            {
                                priceRequest.TerminalId = tierData.TerminalId;
                                priceRequest.CityGroupTerminalId = tierData.CityGroupTerminalId;
                                priceRequest.ProductId = tierData.ProductId ?? requestModel.ProductId;
                            }
                            requestPriceDetails.PricingSourceId = tierData.PricingSourceId;
                            requestPriceDetails.Currency = tierData.Currency;
                            requestPriceDetails.PricePerGallon = tierData.PricePerGallon;
                            requestPriceDetails.RackAvgTypeId = tierData.RackAvgTypeId;

                            await GetPriceForRack(priceRequest, requestModel, requestPriceDetails, response);
                            break;
                    }

                    response.Guid = requestModel.Guid;
                    response.Status = Status.Success;
                    response.TierPricingDetails.MaxQuantity = tierData.MaxQuantity;
                    response.TierPricingDetails.MinQuantity = tierData.MinQuantity;

                    if (requestModel.CanForceTerminalForTierPricing)
                    {
                        response.TierPricingDetails.CityGroupTerminalId = tierData.CityGroupTerminalId;
                        response.TierPricingDetails.TerminalId = tierData.TerminalId;
                    }

                    responseList.Add(response);
                }
                catch (Exception ex)
                {
                    response.Message = "error occurred";
                    LogManager.Logger.WriteException("PricingDomain", "GetFuelPriceForTier", ex.Message
                                    + " requestPriceDetailId: " + requestPriceDetails.RequestPriceDetailId + " PricingTypeId: "
                                    + requestPriceDetails.PricingTypeId + " TerminalId:" + requestModel.TerminalId + " CityGroupTerminalId:"
                                    + requestModel.CityGroupTerminalId + " ProductId:" + requestModel.ProductId + " PricingDate:" + requestModel.PriceDate, ex);

                }
            }
        }

        private async Task GetFuelPriceForRackType(FuelPriceRequestModel requestModel, RequestPriceDetailModel requestPriceDetail, FuelPricingResponseModel response)
        {
            var pricingCode = await _pricingRepository.GetPriceCodeId(requestModel.RequestPriceDetailId);
            PriceRequestModel priceRequest = GetPriceRequestModel(requestModel, pricingCode);
            await GetPriceForRack(priceRequest, requestModel, requestPriceDetail, response);
        }

        private static PriceRequestModel GetPriceRequestModel(FuelPriceRequestModel requestModel, int pricingCode)
        {
            return new PriceRequestModel()
            {
                TerminalId = requestModel.TerminalId,
                CityGroupTerminalId = requestModel.CityGroupTerminalId,
                ProductId = requestModel.ProductId,
                PriceDate = requestModel.PriceDate,
                PricingCodeId = pricingCode
            };
        }

        private async Task GetPriceForRack(PriceRequestModel priceRequest, FuelPriceRequestModel requestModel, RequestPriceDetailModel requestPriceDetail, FuelPricingResponseModel response)
        {
            PricingResponseModel terminalPrice = await GetTerminalPriceAsync(priceRequest, requestPriceDetail.PricingSourceId);
            if (terminalPrice.Price > 0)
            {
                response.TerminalPrice = terminalPrice.Price;
                CurrencyRateDomain _currencyRateDomain = new CurrencyRateDomain();
                response.TerminalPrice = _currencyRateDomain.Convert(terminalPrice.Currency, requestPriceDetail.Currency, terminalPrice.Price, requestModel.PriceDate);
            }
            response.PricePerGallon = GetCalculatedPricePerGallon(response.TerminalPrice, requestPriceDetail.PricePerGallon, requestPriceDetail.RackAvgTypeId ?? 0);
            response.PricingDate = terminalPrice.EffectiveDate;
            response.PriceLastUpdatedDate = terminalPrice.PriceLastUpdatedDate;
            await GetWaitingForAction(requestModel, response, requestPriceDetail.PricingSourceId);
        }

        private async Task GetWaitingForAction(FuelPriceRequestModel requestModel, FuelPricingResponseModel response, int pricingSourceId)
        {
            if (response.TerminalPrice == 0)
                response.WaitingFor = WaitingAction.UpdatedPrice;

            if (pricingSourceId == (int)PricingSource.Axxis)
            {
                if (response.PricingDate.Date < requestModel.PriceDate.Date || response.TerminalPrice == 0)
                {
                    var keys = new List<string>();
                    keys.Add(ApplicationConstants.PublicHolidayList);
                    var appKeyValue = await _pricingRepository.GetPricingConfigAsync(keys);
                    var holidayList = appKeyValue.Configs.FirstOrDefault()?.Value;
                    var holidayResponse = GetDatelistFromString(holidayList);

                    if (!CheckPriceDateAndDropDate(holidayResponse, requestModel.PriceDate, response.PricingDate)) //cosidered Axxis price will not update on Sat or Sun
                        response.WaitingFor = WaitingAction.UpdatedPrice;
                }
            }
        }

        private bool CheckPriceDateAndDropDate(List<DateTime> holidayList, DateTime dropDate, DateTimeOffset pricingDate)
        {
            var daysToCheck = (dropDate - pricingDate.Date).TotalDays; //get total day's diff

            for (int i = 1; i <= daysToCheck; i++)
            {
                var startDate = pricingDate.Date.AddDays(i); //add days from last effective price date
                if (!holidayList.Contains(startDate))
                {
                    if (!new DateTimeOffset(startDate).IsWeekEnd())
                        return false;
                }
            }

            return true;
        }

        public async Task<PricingConfigResponseModel> GetPricingConfigDetailsAsync(int id = 0)
        {
            var response = await _pricingRepository.GetPricingConfigDetailsAsync(id);
            return response;
        }

        public async Task<PricingConfigResponseModel> EditPricingConfigAsync(PricingConfigModel model)
        {
            var response = await _pricingRepository.EditPricingConfigAsync(model);
            return response;
        }

        public async Task<TerminalResponseModel> GetClosestTerminalsForFueltypesAsync(TerminalForFueltypesRequestModel requestModel)
        {
            var response = new TerminalResponseModel(Status.Failed);
            try
            {
                response.Terminals = await _pricingRepository.GetClosestTerminalsForFueltypesAsync(requestModel);
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Message = "error occurred";
                LogManager.Logger.WriteException("PricingDomain", "GetClosestTerminalsForFueltypesAsync", ex.Message + " ProductId:" + requestModel.ProductId + " SerachTerminal:" + requestModel.SearchStringTeminal, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetAllTerminalsAsync()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = await _pricingRepository.GetAllTerminalsAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "GetAllTerminalsAsync", ex.Message, ex);
            }
            return response;
        }

        private List<DateTime> GetDatelistFromString(string publicHolidayList)
        {
            var holidayDates = new List<DateTime>();
            if (publicHolidayList != null)
            {
                var holidays = publicHolidayList.TrimEnd(';').Split(';').ToList();
                holidayDates = holidays.Select(date => DateTime.Parse(date)).ToList();
            }

            return holidayDates;
        }


        public async Task<SyncPricingResponseModel> SyncActualOpisPricingData()
        {
            var response = new SyncPricingResponseModel();
            try
            {
                response = await _pricingRepository.SyncExternalActualOPISPricing(600);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "SyncActualOpisPricingData", ex.Message, ex);
            }
            return response;
        }
        public async Task<IntResponseModel> SyncDyedProductPricingFromClearProducts()
        {
            IntResponseModel response = new IntResponseModel();
            try
            {
                response = await _pricingRepository.ExecuteStoredProcedureScalar("usp_SyncDyedProductPricingFromClearProducts", 60);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "SyncDyedProductPricingFromClearProducts", ex.Message, ex);
            }
            return response;
        }

        public async Task<BooleanResponseModel> AssignNewTerminalForTierPricedOrder(int? terminalId, int requestPricingDetailsId)
        {
            BooleanResponseModel response = new BooleanResponseModel(Status.Failed);
            try
            {
                response = await _pricingRepository.AssignNewTerminalForTierPricedOrder(terminalId, requestPricingDetailsId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "AssignNewTerminalForTierPricedOrder", ex.Message, ex);
            }
            return response;


        }

        public async Task<bool> ResetCumulation()
        {
            bool response = false;
            try
            {
                response= await _pricingRepository.ResetCumulation();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "ResetCumulation", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> UpdateCumulationQtyPostInvoiceCreate(List<CumulationQuantityUpdateViewModel> cumulationQtyList)
        {
            bool response = false;
            try
            {
                response = await _pricingRepository.UpdateCumulationQtyPostInvoiceCreate(cumulationQtyList);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingDomain", "UpdateCumulationQtyPostInvoiceCreate", ex.Message, ex);
            }
            return response;

        }


        public async Task<List<DropdownDisplayExtendedId>> GetSourceRegionForCustomers(List<DropdownDisplayExtendedId> requestPriceDetailIds)
        {
            return await _pricingRepository.GetSourceRegionForCustomers(requestPriceDetailIds);

        }
    }
}
