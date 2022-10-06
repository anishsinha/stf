using SiteFuel.DataAccess.Entities;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.DAL
{

    public class PricingRepository : IPricingRepository
    {
        private DataContext dbContext;
     
        public PricingRepository()
        {
            dbContext = new DataContext();
           
        }

        public PricingRepository(DataContext dbPricingContext)
        {
            dbContext = dbPricingContext;

        }


        #region Generic function, for multiple purpose 

        /// <summary>
        /// CustomSqlQuery : This generic method serve two purpose.
        /// 1. It will execute sql query aginst the database. old way calling mechanism
        ///         await dbContext.Database.SqlQuery<PricingResponseModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
        /// 2. It will also help to mock in Unit test so we can avoid db server trip.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual async Task<T> SqlQueryFirstOrDefaultAsync<T>(string query, SqlParameter[] param)
        {
            return await dbContext.Database.SqlQuery<T>(query, param).FirstOrDefaultAsync();
        }

        public virtual int ExecuteSqlCommand(string query)
        {
           return dbContext.Database.ExecuteSqlCommand(query);
        }
        public virtual async Task<T> SqlQueryFirstOrDefaultAsync<T>(string query)
        {
            return await dbContext.Database.SqlQuery<T>(query).FirstOrDefaultAsync();
        }
        public virtual async Task<List<T>> SqlQueryToListAsync<T>(string query, SqlParameter[] param)
        {   
            return await dbContext.Database.SqlQuery<T>(query, param).ToListAsync();
        }

        public virtual async Task<List<T>> SqlQueryToListAsync<T>(string query)
        {
            return await dbContext.Database.SqlQuery<T>(query).ToListAsync();
        }

        public virtual void SetEntityStateModified<T>(T entity) where T : class
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual DbContextTransaction BeginTransaction()
        {
            return dbContext.Database.BeginTransaction();
        }

        public virtual void Commit(DbContextTransaction dbContextTransaction)
        {
            dbContextTransaction.Commit();
        }

        #endregion


        public async Task<PricingResponseModel> GetAxxisPricingDataAsync(int terminalId, int? cityGroupTerminalId, int productId, DateTime priceDate, int pricingCodeId, int timeout = 30)
        {
            priceDate = priceDate.Date.AddDays(1).AddSeconds(-1);
            object inputmodel = null;
            if (cityGroupTerminalId.HasValue && cityGroupTerminalId > 0)
                inputmodel = new { @TerminalId = terminalId, @PricingDate = priceDate, @ProductId = productId, @PricingCodeId = pricingCodeId, @CityGroupTerminalId = cityGroupTerminalId };
            else
                inputmodel = new { @TerminalId = terminalId, @PricingDate = priceDate, @ProductId = productId, @PricingCodeId = pricingCodeId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetTerminalPriceForAxxis", inputmodel);
            dbContext.Database.CommandTimeout = timeout;
            
           // var response = await dbContext.Database.SqlQuery<PricingResponseModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
             var response = await SqlQueryFirstOrDefaultAsync<PricingResponseModel>(input.Query, input.Params.ToArray());
            return response;
        }

     

        public async Task<PricingResponseModel> GetTerminalPriceForOpisAsync(PriceRequestModel requestModel, int timeout = 30)
        {
            var inputmodel = new
            {
                PriceDate = requestModel.PriceDate,
                CityRackTerminalId = requestModel.CityGroupTerminalId ?? 0,
                ProductId = requestModel.ProductId,
                PricingCodeId = requestModel.PricingCodeId
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetOpisTerminalPrice_v1", inputmodel);

            dbContext.Database.CommandTimeout = timeout;
            var response = await SqlQueryFirstOrDefaultAsync< PricingResponseModel>(input.Query, input.Params.ToArray());
           // var response = await dbContext.Database.SqlQuery<PricingResponseModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            return response;
        }

        public async Task<PricingResponseModel> GetTerminalPriceForPlattsAsync(int? terminalId, int productId, DateTime priceDate, int pricingCodeId, int timeout = 30)
        {
            var inputmodel = new { PriceDate = priceDate, CityRackTerminalId = terminalId ?? 0, ProductId = productId, PricingCodeId = pricingCodeId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetPlattsTerminalPrice", inputmodel);
            dbContext.Database.CommandTimeout = timeout;

            var response = await SqlQueryFirstOrDefaultAsync<PricingResponseModel>(input.Query, input.Params.ToArray());
            //var response = await dbContext.Database.SqlQuery<PricingResponseModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            return response;
        }

        public async Task<List<PricingData>> GetClosestTerminalPriceAsync(SalesCalculatorRequestModel requestModel, int timeout = 30)
        {
            TimeSpan duration = new TimeSpan(0, 23, 59, 59, 999);
            requestModel.PriceDate = requestModel.PriceDate.Date.Add(duration);
            var inputmodel = new
            {
                @PricingCodeId = requestModel.PricingCodeId ?? 0,
                @PricingDate = requestModel.PriceDate,
                @ExternalProductId = requestModel.ProductId,
                @SrcLatitude = requestModel.SrcLatitude,
                @SrcLongitude = requestModel.SrcLongitude,
                @RecordCount = requestModel.RecordCount,
                @CityGroupTerminalId = requestModel.CityGroupTerminalIds.FirstOrDefault(),
                @CountryCode = requestModel.CountryCode,
                @CompanyCountryId = requestModel.CompanyCountryId
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetAxxisTerminalPricesForCalculator", inputmodel);

            dbContext.Database.CommandTimeout = timeout;
            // var response = await dbContext.Database.SqlQuery<PricingData>(input.Query, input.Params.ToArray()).ToListAsync();
            var response = await SqlQueryToListAsync<PricingData>(input.Query, input.Params.ToArray());
            return response;
        }

        public async Task<List<PricingData>> GetOpisTerminalPricesForCalculatorAsync(SalesCalculatorRequestModel requestModel, int timeout = 30)
        {
            var inputmodel = new
            {
                //SourceId = requestModel.PricingSourceId,
                PriceDate = requestModel.PriceDate,
                CityRackTerminalIds = string.Join(",", requestModel.CityGroupTerminalIds),
                ProductId = requestModel.ProductId,
                //FeedTypeId = requestModel.FeedTypeId ?? 0,
                //BrandTypeId = requestModel.BrandTypeId ?? 0,
                //PriceTypeId = requestModel.PriceTypeId ?? 0,
                PricingCodeId = requestModel.PricingCodeId ?? 0,
                requestModel.requestModel
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetOpisTerminalPricesForCalculator_v1", inputmodel);

            dbContext.Database.CommandTimeout = timeout;
            //var response = await dbContext.Database.SqlQuery<PricingData>(input.Query, input.Params.ToArray()).ToListAsync();
            var response = await SqlQueryToListAsync<PricingData>(input.Query, input.Params.ToArray());
            return response;
        }

        public async Task<List<PricingData>> GetPlattsTerminalPricesForCalculatorAsync(SalesCalculatorRequestModel requestModel, int timeout = 30)
        {
            var inputmodel = new
            {
                //SourceId = requestModel.PricingSourceId,
                PriceDate = requestModel.PriceDate,
                CityRackTerminalIds = string.Join(",", requestModel.CityGroupTerminalIds),
                ProductId = requestModel.ProductId,
                PricingCodeId = requestModel.PricingCodeId ?? 0,
                requestModel.requestModel
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetPlattsTerminalPricesForCalculator", inputmodel);

            dbContext.Database.CommandTimeout = timeout;
            //var response = await dbContext.Database.SqlQuery<PricingData>(input.Query, input.Params.ToArray()).ToListAsync();
            var response = await SqlQueryToListAsync<PricingData>(input.Query, input.Params.ToArray());
            return response;
        }

        public async Task<List<PricingData>> GetCityRackTerminalPricesForCalculator(CityRackPricesRequestModel model, int timeout = 30)
        {
            TimeSpan duration = new TimeSpan(0, 23, 59, 59, 999);
            model.PriceDate = model.PriceDate.Date.Add(duration);

            var input = SqlHelperMethods.GetStoredProcedure("usp_GetCityRackTerminalPricesForCalculator", model);

            dbContext.Database.CommandTimeout = timeout;
            //var response = await dbContext.Database.SqlQuery<PricingData>(input.Query, input.Params.ToArray()).ToListAsync();
            var response = await SqlQueryToListAsync<PricingData>(input.Query, input.Params.ToArray());

            return response;
        }

        public async Task<List<PricingData>> GetTerminalPricesForAuditAsync(TerminalPricesRequestModel model, int timeout = 30)
        {
            TimeSpan duration = new TimeSpan(0, 23, 59, 59, 999);
            model.PricingDate = model.PricingDate.Date.Add(duration);

            var inputModel = new { model.PricingDate, model.ExternalProductId, model.SrcLatitude, model.SrcLongitude, model.RecordCount };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetTerminalPricesForAudit", inputModel);

            dbContext.Database.CommandTimeout = timeout;
            //var response = await dbContext.Database.SqlQuery<PricingData>(input.Query, input.Params.ToArray()).ToListAsync();
            var response = await SqlQueryToListAsync<PricingData>(input.Query, input.Params.ToArray());

            return response;
        }

        public async Task<List<TerminalDetails>> GetClosestTerminalsAsync(TerminalRequestModel requestModel, int timeout = 30)
        {
            var inputModel = new
            {
                CountryId = requestModel.CountryId,
                FuelTypeId = requestModel.ProductId,
                Latitude = requestModel.SrcLatitude,
                Longitude = requestModel.SrcLongitude,
                PricingCode = requestModel.PricingCodeId,
                Terminal = requestModel.SearchStringTeminal.Trim(),
                CompanyCountryId = requestModel.CompanyCountryId > 0 ? requestModel.CompanyCountryId : requestModel.CountryId,
                IsSuppressPricing = requestModel.IsSuppressPricing
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetClosestTerminals", inputModel);

            dbContext.Database.CommandTimeout = timeout;

            //var response = await dbContext.Database.SqlQuery<TerminalDetails>(input.Query, input.Params.ToArray()).ToListAsync();
            var response = await SqlQueryToListAsync<TerminalDetails>(input.Query, input.Params.ToArray());

            return response;
        }


        public async Task<List<TerminalDetails>> GetClosestTerminalsByDistanceAsync(TerminalRequestViewModel requestModel, int timeout = 30)
        {
            var inputModel = new
            {
                FuelTypeId = requestModel.ProductId,
                PricingCode = requestModel.PricingCodeId,
                CountryId = requestModel.CountryId,
                JobLatitude = requestModel.SrcLatitude,
                JobLongitude = requestModel.SrcLongitude,
                TerminalIds = requestModel.TerminalIds,
                PricingTypeId = requestModel.PricingTypeId,
                CompanyCountryId = requestModel.CompanyCountryId > 0 ? requestModel.CompanyCountryId : requestModel.CountryId,
                IsSuppressPricing = requestModel.IsSuppressPricing
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetClosestTerminalsByDistance", inputModel);

            dbContext.Database.CommandTimeout = timeout;

            var response = await SqlQueryToListAsync<TerminalDetails>(input.Query, input.Params.ToArray());

            return response;
        }

        public async Task<DateTime> GetLastUpdatedPricingDate(int requestPriceDetailId)
        {
            // int pricingSourceId = await dbContext.RequestPriceDetails.Where(t => t.Id == requestPriceDetailId).Select(t => t.MstPricingCode.PricingSourceId).FirstOrDefaultAsync();
            int pricingSourceId = await dbContext.PricingDetails.Where(t => t.RequestPriceDetailId == requestPriceDetailId && t.IsActive).Select(t => t.MstPricingCode.PricingSourceId).FirstOrDefaultAsync();
            string key = pricingSourceId == (int)PricingSource.Axxis ? ApplicationConstants.PricingDataLastUpdatedDate : ApplicationConstants.PricingDataSourcesUpdatedDate;
            string value = await dbContext.MstPricingConfig.Where(t => t.Key == key).Select(t => t.Value).FirstOrDefaultAsync();
            var result = (DateTime)Convert.ChangeType(value, typeof(DateTime));
            return result.Date;
        }

        public async Task<PricingResponseModel> GetTerminalPriceAsync(int productId, int terminalId)
        {
            var response = new PricingResponseModel(Status.Failed);

            var spResponse = await dbContext.ExternalPricingAxxis.OrderByDescending(t => t.EffectiveDate).FirstOrDefaultAsync(t => t.TerminalId == terminalId && t.ProductId == productId);
            if (spResponse != null)
            {
                response = new PricingResponseModel(Status.Success) { Currency = spResponse.Currency };
            }

            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetAxxisProductDetailsAsync(ProductDetailsRequestModel requestModel, int timeout = 30)
        {
            var inputmodel = new
            {
                @Latitude = requestModel.Latitude,
                @Longitude = requestModel.Longitude,
                @Radius = requestModel.Radius,
                @CountryCode = requestModel.CountryCode,
                @CompanyCountryId = requestModel.CompanyCountryId
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetProductsInYourArea", inputmodel);

            dbContext.Database.CommandTimeout = timeout;
            //var response = await dbContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            var response = await SqlQueryToListAsync<DropdownDisplayItem>(input.Query, input.Params.ToArray());
            return response;
        }

        public async Task<PricingConfigResponse> GetPricingConfigAsync(List<string> keys)
        {
            var response = new PricingConfigResponse(Status.Failed);
            var pricingConfigs = await dbContext.MstPricingConfig.Where(t => keys.Contains(t.Key) && t.IsActive).ToListAsync();
            if (pricingConfigs != null)
            {
                response.Configs = new List<PricingConfig>();
                foreach (var item in pricingConfigs)
                {
                    var config = new PricingConfig() { Key = item.Key, Value = item.Value };
                    response.Configs.Add(config);
                }
                response.Status = Status.Success;
            }
            return response;
        }

        public async Task<bool> IsAxxisCityRackPriceAvailable(int fueltypeId, int cityGroupTerminalId, DateTime effectiveDate)
        {
            var response = await dbContext.ExternalPricingAxxis.AnyAsync(t => t.ProductId == fueltypeId && t.TerminalId == cityGroupTerminalId && t.EffectiveDate <= effectiveDate);
            return response;
        }

        public async Task<int> GetPricingSourceIdAsync(int? requestPriceId)
        {
            int pricingSourceId = (int)PricingSource.Axxis;
            if (requestPriceId != null)
            {
                // pricingSourceId = await dbContext.RequestPriceDetails.Where(t => t.Id == requestPriceId).Select(t => t.MstPricingCode.PricingSourceId).FirstOrDefaultAsync();
                pricingSourceId = await dbContext.PricingDetails.Where(t => t.RequestPriceDetailId == requestPriceId && t.IsActive).Select(t => t.MstPricingCode.PricingSourceId).FirstOrDefaultAsync();
            }
            return pricingSourceId;
        }

        public async Task<bool> IsOpisCityRackPriceAvailable(int fueltypeId, int cityGroupTerminalId)
        {
            var response = await dbContext.ExternalPricingOpis.AnyAsync(t => t.ProductId == fueltypeId && t.TerminalId == cityGroupTerminalId);
            return response;
        }

        public async Task<bool> IsPlattsCityRackPriceAvailable(int fueltypeId, int cityGroupTerminalId)
        {
            var response = await dbContext.ExternalPricingPlatts.AnyAsync(t => t.ProductId == fueltypeId && t.TerminalId == cityGroupTerminalId);
            return response;
        }

        public async Task<IntResponseModel> ExecuteStoredProcedureScalar(string spName, int timeout = 30)
        {
            IntResponseModel response = new IntResponseModel();
            try
            {
                dbContext.Database.CommandTimeout = timeout;
                //response.Result = await dbContext.Database.SqlQuery<int>($"exec {spName}").FirstOrDefaultAsync();//todo : need to check with null
                response.Result = await SqlQueryFirstOrDefaultAsync<int>($"exec {spName}");
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRepository", "ExecuteStoredProcedureScalar=> " + spName, ex.Message, ex);
            }
            return response;
        }

        public async Task<SyncPricingResponseModel> SyncExternalSourcePricing(int timeout = 30)
        {
            SyncPricingResponseModel response = new SyncPricingResponseModel();
            try
            {
                dbContext.Database.CommandTimeout = timeout;
                //response.PricingResponse = await dbContext.Database.SqlQuery<SyncPricingResponse>($"exec usp_SyncExternalSourcePricings").ToListAsync();

                response.PricingResponse = await SqlQueryToListAsync<SyncPricingResponse>($"exec usp_SyncExternalSourcePricings");
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRepository", "SyncExternalSourcePricing ", ex.Message, ex);
            }
            return response;
        }

        public async Task<PricingResponseModel> GetAxxisTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel, int timeout = 30)
        {
            var inputmodel = new
            {
                @PricingDate = DateTime.Now,
                @ProductId = Convert.ToInt32(requestModel.ProductId),
                @SrcLatitude = requestModel.Latitude,
                @SrcLongitude = requestModel.Longitude,
                @CityGroupTerminalId = 0,
                @CountryCode = "USA"
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetAxxisTerminalPriceForCurrentDate", inputmodel);
            dbContext.Database.CommandTimeout = timeout;

            // var response = await dbContext.Database.SqlQuery<PricingResponseModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            var response = await SqlQueryFirstOrDefaultAsync<PricingResponseModel>(input.Query, input.Params.ToArray());
            return response;
        }

        public async Task<PricingResponseModel> GetOpisTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel, int timeout = 30)
        {
            var inputmodel = new
            {
                @CityGroupTerminalId = requestModel.TerminalId ?? 0,
                @PricingSource = requestModel.PricingSourceId,
                @FuelTypes = requestModel.ProductId,
                @PricingCodeId = requestModel.PricingCodeId
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetOpisTerminalPriceForCurrentDate_v1", inputmodel);

            dbContext.Database.CommandTimeout = timeout;
            var response = await SqlQueryFirstOrDefaultAsync<PricingResponseModel>(input.Query, input.Params.ToArray());
            return response;
        }

        public async Task<PricingResponseModel> GetPlattsTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel, int timeout = 30)
        {
            var inputmodel = new { @CityGroupTerminalId = requestModel.TerminalId, @PricingSource = requestModel.PricingSourceId, @FuelTypes = requestModel.ProductId, @PricingCodeId = requestModel.PricingCodeId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetPlattsTerminalPriceForCurrentDate", inputmodel);
            dbContext.Database.CommandTimeout = timeout;
            var response = await SqlQueryFirstOrDefaultAsync<PricingResponseModel>(input.Query, input.Params.ToArray());
            return response;
        }

        public async Task<IntResponseModel> AddNewProduct(ProductRequestModel product)
        {
            IntResponseModel response = new IntResponseModel();
                MstProduct newProduct = new MstProduct
                {
                    Name = product.Name,
                    PricingSourceId = product.PricingSourceId,
                    ProductTypeId = product.ProductTypeId,
                    ProductDisplayGroupId = product.ProductDisplayGroupId,
                    IsActive = true,
                    UpdatedBy = product.UpdatedBy,
                    UpdatedDate = product.UpdatedDate,
                    CompanyId = product.CompanyId,
                    TfxProductId= product.TfxProductId,
                    DisplayName= product.DisplayName

                };
                dbContext.MstProducts.Add(newProduct);
                await dbContext.SaveChangesAsync();

                response.Result = newProduct.Id;
                response.Status = Status.Success;
         
            return response;
        }

        public async Task<IntResponseModel> UpdateProductDetails(ProductRequestModel product)
        {
            IntResponseModel response = new IntResponseModel();
            if (product.Id > 0)
            {
                var existingProduct = dbContext.MstProducts.SingleOrDefault(t => t.Id == product.Id);
                if(existingProduct!=null)
                {
                    existingProduct.Name = product.Name;
                    if (product.ProductTypeId == (int)ProductTypes.Additives)
                        existingProduct.DisplayName = product.Name;
                    if (product.IsDeleted)
                        existingProduct.IsActive = false;

                    SetEntityStateModified<MstProduct>(existingProduct);
                    await dbContext.SaveChangesAsync();

                    response.Result = existingProduct.Id;
                    response.Status = Status.Success;
                }
            }
            return response;
        }

        public async Task<IntResponseModel> SaveTerminalDetails(PickupLocationDetailViewModel terminal)
        {
            IntResponseModel response = new IntResponseModel();
            if (terminal != null)
            {
                if (terminal.Id > 0)
                {
                    var existingTerminal = dbContext.MstExternalTerminals.SingleOrDefault(t => t.Id == terminal.Id);
                    if (existingTerminal != null)
                    {
                        existingTerminal = ToTerminalEntity(terminal, existingTerminal);
                        SetEntityStateModified<MstExternalTerminal>(existingTerminal);
                        await dbContext.SaveChangesAsync();

                        response.Result = existingTerminal.Id;
                        response.Status = Status.Success;
                    }
                }
                else
                {
                    var newTerminal = ToTerminalEntity(terminal);
                    dbContext.MstExternalTerminals.Add(newTerminal);
                    await dbContext.SaveChangesAsync();

                    response.Result = newTerminal.Id;
                    response.Status = Status.Success;
                }
            }
            return response;
        }

        private MstExternalTerminal ToTerminalEntity(PickupLocationDetailViewModel viewModel, MstExternalTerminal entity = null)
        {
            if (entity == null)
                entity = new MstExternalTerminal();

            entity.Id = viewModel.Id;
            entity.Name = viewModel.Name;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.Address = viewModel.Address;
            entity.City = viewModel.City;
            entity.StateCode = viewModel.StateCode;
            entity.StateId = viewModel.StateId;
            entity.CountryCode = viewModel.CountryCode;
            entity.CountyName = viewModel.County;
            entity.ZipCode = viewModel.ZipCode;
            entity.Abbreviation = string.IsNullOrWhiteSpace(viewModel.Abbreviation) ? Resource.lblSingleHyphen : viewModel.Abbreviation;
            entity.Code = string.IsNullOrWhiteSpace(viewModel.Abbreviation) ? Resource.lblSingleHyphen : viewModel.Abbreviation;
            entity.ControlNumber = string.IsNullOrWhiteSpace(viewModel.ControlNumber) ? Resource.lblSingleHyphen : viewModel.ControlNumber; 
            entity.Currency = viewModel.CountryCode.ToLower().Equals("usa") || viewModel.CountryCode.ToLower().Equals("us") ? (int)Currency.USD : (int)Currency.CAD;
            entity.PricingSourceId = (int)PricingSource.Axxis;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = DateTimeOffset.Now;
            entity.TerminalOwner = viewModel.TerminalOwner;
            entity.IsActive = true;

            return entity;
        }

        public async Task<IntResponseModel> AddNewTfxProduct(ProductRequestModel product)
        {
            IntResponseModel response = new IntResponseModel();
            MstTfxProduct newProduct = new MstTfxProduct
            {
                Name = product.Name,
                ProductTypeId = product.ProductTypeId,
                ProductCode = product.ProductCode,
                ProductDisplayGroupId = product.ProductDisplayGroupId,
                IsActive = true,
                UpdatedBy = product.UpdatedBy,
                UpdatedDate = product.UpdatedDate
            };
            dbContext.MstTfxProducts.Add(newProduct);
            await dbContext.SaveChangesAsync();

            if (product.AxxisProductId > 0)
                await AddProductMapping(newProduct.Id, product.Name, product.AxxisProductId.Value);

            if (product.OpisProductId > 0)
                await AddProductMapping(newProduct.Id, product.Name, product.OpisProductId.Value);

            if (product.PlattsProductId > 0)
                await AddProductMapping(newProduct.Id, product.Name, product.PlattsProductId.Value);

            response.Result = newProduct.Id;
            response.Status = Status.Success;

            return response;
        }

        public async Task<IntResponseModel> UpdateTfxProduct(ProductRequestModel product)
        {
            IntResponseModel response = new IntResponseModel();
            var tfxProduct = await dbContext.MstTfxProducts.FirstOrDefaultAsync(t => t.IsActive && t.Id == product.Id);
            if (product.Id > 0 && tfxProduct != null)
            {
                tfxProduct.Name = product.Name;
                tfxProduct.ProductCode = product.ProductCode;
                tfxProduct.ProductDisplayGroupId = product.ProductDisplayGroupId;
                tfxProduct.ProductTypeId = product.ProductTypeId;
                tfxProduct.IsActive = true;
                tfxProduct.UpdatedBy = product.UpdatedBy;
                tfxProduct.UpdatedDate = product.UpdatedDate;
                SetEntityStateModified<MstTfxProduct>(tfxProduct);
               // dbContext.Entry(tfxProduct).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();

                if (product.AxxisProductId > 0)
                    await UpdateProductMapping(tfxProduct, product.Name, product.AxxisProductId.Value, (int)PricingSource.Axxis);
                else
                    await RemoveProductMapping(tfxProduct, (int)PricingSource.Axxis);

                if (product.OpisProductId > 0)
                    await UpdateProductMapping(tfxProduct, product.Name, product.OpisProductId.Value, (int)PricingSource.OPIS);
                else
                    await RemoveProductMapping(tfxProduct, (int)PricingSource.OPIS);

                if (product.PlattsProductId > 0)
                    await UpdateProductMapping(tfxProduct, product.Name, product.PlattsProductId.Value, (int)PricingSource.PLATTS);
                else
                    await RemoveProductMapping(tfxProduct, (int)PricingSource.PLATTS);

                response.Result = tfxProduct.Id;
                response.Status = Status.Success;
            }

            return response;
        }

        private async Task AddProductMapping(int tfxProductId, string displayName, int productId)
        {
            var product = await dbContext.MstProducts.FirstOrDefaultAsync(t => t.IsActive && t.Id == productId);
            if (product != null)
            {
                product.TfxProductId = tfxProductId;
                product.DisplayName = displayName;
                SetEntityStateModified<MstProduct>(product);
               // dbContext.Entry(product).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task UpdateProductMapping(MstTfxProduct tfxProduct, string displayName, int newProductId, int pricingSourceId)
        {
            await RemoveProductMapping(tfxProduct, pricingSourceId);
            var product = await dbContext.MstProducts.FirstOrDefaultAsync(t => t.IsActive && t.Id == newProductId);
            if (product != null)
            {
                product.TfxProductId = tfxProduct.Id;
                product.DisplayName = displayName;
                product.ProductDisplayGroupId = tfxProduct.ProductDisplayGroupId;
                product.ProductTypeId = tfxProduct.ProductTypeId;
                product.ProductCode = tfxProduct.ProductCode;
                SetEntityStateModified<MstProduct>(product);
                //dbContext.Entry(product).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task RemoveProductMapping(MstTfxProduct tfxProduct, int pricingSourceId)
        {
            var product = tfxProduct.MstProducts.FirstOrDefault(t => t.PricingSourceId == pricingSourceId);
            if (product != null)
            {
                product.TfxProductId = null;
                product.DisplayName = null;
                //dbContext.Entry(product).State = EntityState.Modified;
                SetEntityStateModified<MstProduct>(product);
                await dbContext.SaveChangesAsync();
            }
        }


        public async Task<List<RequestPriceDetailModel>> GetRequestPriceDetailsAsync(List<int> requestPriceDetailIds)
        {
            //return await dbContext.RequestPriceDetails.Where(t => requestPriceDetailIds.Contains(t.Id)).Select(t => new RequestPriceDetailModel()
            //{
            //    PricePerGallon = t.PricePerGallon,
            //    SupplierCost = t.SupplierCost,
            //    SupplierCostTypeId = t.SupplierCostTypeId,
            //    RackTypeId = t.MstPricingCode.RackTypeId,
            //    RackAvgTypeId = t.RackAvgTypeId,
            //    FeedTypeId = t.MstPricingCode.FeedTypeId,
            //    PricingTypeId = t.MstPricingCode.PricingTypeId,
            //    PricingSourceId = t.MstPricingCode.PricingSourceId,
            //    RequestPriceDetailId = t.Id,
            //    Currency = t.Currency
            //}).ToListAsync();
            return await dbContext.PricingDetails.Where(t => requestPriceDetailIds.Contains(t.RequestPriceDetailId) && t.IsActive).Select(t => new RequestPriceDetailModel()
            {
                PricePerGallon = t.PricePerGallon,
                SupplierCost = t.SupplierCost,
                SupplierCostTypeId = t.SupplierCostTypeId,
                RackTypeId = t.MstPricingCode.RackTypeId,
                RackAvgTypeId = t.RackAvgTypeId,
                FeedTypeId = t.MstPricingCode.FeedTypeId,
                PricingTypeId = t.RequestPriceDetails.PricingTypeId ?? t.MstPricingCode.PricingTypeId,
                PricingSourceId = t.MstPricingCode.PricingSourceId,
                RequestPriceDetailId = t.RequestPriceDetailId,
                Currency = t.RequestPriceDetails.Currency
            }).ToListAsync();
        }

        public async Task<int> GetSourceFromPriceCodeAsync(int pricingCodeId)
        {
            return await dbContext.MstPricingCodes.Where(t => t.Id == pricingCodeId).Select(t => t.PricingSourceId).FirstOrDefaultAsync();
        }

        public async Task<int> GetPriceCodeId(int requestPriceId)
        {
            //return await dbContext.RequestPriceDetails.Where(t => t.Id == requestPriceId).Select(t => t.PricingCodeId).FirstOrDefaultAsync();
            return await dbContext.PricingDetails.Where(t => t.RequestPriceDetailId == requestPriceId && t.IsActive).Select(t => t.PricingCodeId).FirstOrDefaultAsync();
        }

        public async Task<List<TierPricingRequestModel>> GetTierPricingReqModel(int requestPriceId, decimal maxQuantity)
        {
            var response = new List<TierPricingRequestModel>();

                response = await (from pricingDetail in dbContext.PricingDetails
                                      join codeDetail in dbContext.MstPricingCodes on pricingDetail.PricingCodeId equals codeDetail.Id
                                      where pricingDetail.RequestPriceDetailId == requestPriceId &&
                                                  (pricingDetail.MinQuantity < maxQuantity || maxQuantity == 0)
                                                  && pricingDetail.IsActive && codeDetail.IsActive
                                      select new TierPricingRequestModel()
                                      {
                                          TierTypeId = pricingDetail.RequestPriceDetails.TierTypeId,
                                          MaxQuantity = pricingDetail.MaxQuantity,
                                          MinQuantity = pricingDetail.MinQuantity,
                                          PricingCodeId = codeDetail.Id,
                                          PricingTypeId = codeDetail.PricingTypeId,
                                          CityGroupTerminalId = pricingDetail.CityRackTerminalId,
                                          ProductId = pricingDetail.FuelTypeId,
                                          TerminalId = pricingDetail.TerminalId,
                                          PricePerGallon = pricingDetail.PricePerGallon,
                                          SupplierCost = pricingDetail.SupplierCost,
                                          SupplierCostTypeId = pricingDetail.SupplierCostTypeId,
                                          RackTypeId = codeDetail.RackTypeId,
                                          RackAvgTypeId = pricingDetail.RackAvgTypeId,
                                          FeedTypeId = codeDetail.FeedTypeId,
                                          PricingSourceId = codeDetail.PricingSourceId,
                                          Currency = pricingDetail.RequestPriceDetails.Currency
                                      }
                           ).ToListAsync();
            
            var cumulatedQty = dbContext.CumulationDetails.Where(t => t.RequestPriceDetailId == requestPriceId && t.IsActive)
                                .Select(t => t.CumulatedQuantity).FirstOrDefault();
            response.ForEach(t => t.CumulatedQuantity = cumulatedQty);
            return response;
        }

        public async Task<PricingConfigResponseModel> GetPricingConfigDetailsAsync(int id = 0)
        {
            var response = new PricingConfigResponseModel();
            try
            {
                if (id == 0)
                {
                    response.ConfigList = await dbContext.MstPricingConfig.Where(t => t.IsActive).Select(t => new PricingConfigModel()
                    {
                        Id = t.Id,
                        Key = t.Key,
                        Value = t.Value,
                        Description = t.Description,
                        IsActive = t.IsActive,
                        UpdatedBy = t.UpdatedBy.ToString(),
                        UpdatedDate = t.UpdatedDate.ToString()
                    }).ToListAsync();
                }
                else
                {
                    response.Config = await dbContext.MstPricingConfig.Where(t => t.IsActive && t.Id == id).Select(t => new PricingConfigModel()
                    {
                        Id = t.Id,
                        Key = t.Key,
                        Value = t.Value,
                        Description = t.Description,
                        IsActive = t.IsActive,
                        UpdatedBy = t.UpdatedBy.ToString(),
                        UpdatedDate = t.UpdatedDate.ToString()
                    }).FirstOrDefaultAsync();
                }

                response.Status = Status.Success;
                response.Message = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                response.Status = Status.Failed;
                response.Message = Status.Failed.ToString();
                LogManager.Logger.WriteException("PricingRepository", "GetPricingConfigDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<PricingConfigResponseModel> EditPricingConfigAsync(PricingConfigModel model)
        {
            var response = new PricingConfigResponseModel();
            try
            {
                var config = await dbContext.MstPricingConfig.FirstOrDefaultAsync(t => t.IsActive && t.Id == model.Id);
                if (config != null)
                {
                    config.Value = model.Value;
                    config.UpdatedBy = Convert.ToInt32(model.UpdatedBy);
                    config.UpdatedDate = DateTimeOffset.Now;

                    // dbContext.Entry(config).State = EntityState.Modified;
                    SetEntityStateModified<MstPricingConfig>(config);
                    await dbContext.SaveChangesAsync();

                    response.Config = new PricingConfigModel()
                    {
                        Id = config.Id,
                        Key = config.Key,
                        Value = config.Value,
                        Description = config.Description,
                        IsActive = config.IsActive,
                        UpdatedBy = config.UpdatedBy.ToString(),
                        UpdatedDate = config.UpdatedDate.ToString()
                    };

                    response.Status = Status.Success;
                    response.Message = Status.Success.ToString();
                }
            }
            catch (Exception ex)
            {
                response.Status = Status.Failed;
                response.Message = Status.Failed.ToString();

                LogManager.Logger.WriteException("PricingRepository", "EditPricingConfigAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TerminalDetails>> GetClosestTerminalsForFueltypesAsync(TerminalForFueltypesRequestModel requestModel, int timeout = 30)
        {
            var inputModel = new
            {
                CountryId = requestModel.CountryId,
                FuelTypeId = requestModel.ProductId,
                Latitude = requestModel.SrcLatitude,
                Longitude = requestModel.SrcLongitude,
                PricingCode = requestModel.PricingCodeId,
                Terminal = requestModel.SearchStringTeminal.Trim(),
                CompanyCountryId = requestModel.CompanyCountryId,
                IsSuppressPricing = requestModel.IsSuppressPricing
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetClosestTerminalsForMultipleProducts", inputModel);

            dbContext.Database.CommandTimeout = timeout;
            var response = await SqlQueryToListAsync<TerminalDetails>(input.Query, input.Params.ToArray());

            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetAllTerminalsAsync()
        {
            var response = await dbContext.MstExternalTerminals
                            .Where(t => t.ControlNumber != "-" && t.IsActive)
                            .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name })
                            .OrderBy(t => t.Name).ToListAsync();
            return response;
        }

        /// <summary>
        /// Synchronize the external actual opis pricing to the database.
        /// </summary>
        /// <param name="timeout">Time out.</param>
        /// <returns><see cref="SyncPricingResponseModel"/>.</returns>
        public async Task<SyncPricingResponseModel> SyncExternalActualOPISPricing(int timeout = 30)
        {
            var response = new SyncPricingResponseModel();
            try
            {
                dbContext.Database.CommandTimeout = timeout;
                response.PricingResponse = await SqlQueryToListAsync<SyncPricingResponse>($"exec usp_SyncExternalActualOPISPricings");
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Status = Status.Failed;
                LogManager.Logger.WriteException("PricingRepository", "SyncExternalActualOPISPricing ", ex.Message, ex);
            }
            return response;
        }

        public async Task<BooleanResponseModel> AssignNewTerminalForTierPricedOrder(int? terminalId, int requestPricingDetailsId)
        {
            //Context.DataContext.Database.BeginTransaction()
            BooleanResponseModel response = new BooleanResponseModel(Status.Failed);
            
            //using (var transaction = dbContext.Database.BeginTransaction())
            using (var transaction = BeginTransaction())
            {
                try
                {
                    if (requestPricingDetailsId > 0)
                    {
                        var tiers = await dbContext.PricingDetails.Where(t => t.RequestPriceDetailId == requestPricingDetailsId && t.IsActive).ToListAsync();

                        if (tiers != null && tiers.Any())
                        {
                            foreach (var tier in tiers)
                            {
                                tier.TerminalId = terminalId;
                                // dbContext.Entry(tier).State = EntityState.Modified;
                                SetEntityStateModified<PricingDetail>(tier);
                            }
                            // Updating a List is not supported in EF6
                            //dbContext.Entry(tiers).State = EntityState.Modified;

                            await dbContext.SaveChangesAsync();
                            Commit(transaction);
                            response.Message = string.Format(Resource.errMessageTerminalAssignmentSuccess, string.Empty);
                            response.Status = Status.Success;
                            response.Result = true;

                        }
                    }
                }
                catch (Exception ex)
                {
                    response.Status = Status.Failed;
                    response.Message = Resource.errMessageFailedToAssignTerminal;
                    response.Result = false;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("PricingRepository", "AssignNewTerminalForTierPricedOrder", ex.Message, ex);

                }
            }

            return response;
        }

        public async Task<bool> ResetCumulation()
        {
            var isRecordProcessed = false;

            try
            {
                var requestpricingDetailEntities = await dbContext.RequestPriceDetails
                                                                   .Where(t => t.CumulationTypeId != null
                                                                   && t.CumulationDetails.Any(t1 => t1.IsActive))
                                                                   .ToListAsync();
                var currentDate = DateTimeOffset.Now;
                List<int> entityIdsToDeactivate = new List<int>();
                //  Dictionary<int, DateTimeOffset> reqPriceDetailsEntitiesToUpdate = new Dictionary<int, DateTimeOffset>();
                List<CumulationDetail> newlyAddedEntities = new List<CumulationDetail>();

                if (requestpricingDetailEntities != null)
                {
                    foreach (var item in requestpricingDetailEntities)
                    {
                        if (item.CumulationDetails != null && item.CumulationDetails.Any())
                        {
                            CumulationDetail cumulationEntity = item.CumulationDetails.Where(t => t.RequestPriceDetailId == item.Id && t.IsActive).FirstOrDefault();

                            if (cumulationEntity != null)
                            {
                                var cumulationResetDay = item.CumulationResetDay != null ? item.CumulationResetDay : 0;

                                if (item.CumulationTypeId == (int)CumulationType.Weekly)
                                {

                                    if (currentDate.Date >= cumulationEntity.EndDate.Date)
                                    {
                                        var newCumulationResetDate = GetNewResetDateForWeeklyReset(cumulationEntity.EndDate, cumulationResetDay);

                                        entityIdsToDeactivate.Add(cumulationEntity.Id);


                                        CumulationDetail entity = new CumulationDetail();
                                        entity.StartDate = currentDate;
                                        entity.EndDate = newCumulationResetDate;
                                        entity.RequestPriceDetailId = item.Id;
                                        entity.CumulatedQuantity = 0;
                                        entity.IsActive = true;

                                        newlyAddedEntities.Add(entity);
                                        //    reqPriceDetailsEntitiesToUpdate.Add(item.Id, newCumulationResetDate);

                                        isRecordProcessed = true;
                                    }

                                }
                                else if (item.CumulationTypeId == (int)CumulationType.BiWeekly)
                                {
                                    if (currentDate.Date >= cumulationEntity.EndDate.Date)
                                    {
                                        var newCumulationResetDate = GetNewResetDateForBiWeeklyReset(cumulationEntity.EndDate, cumulationResetDay);

                                        entityIdsToDeactivate.Add(cumulationEntity.Id);


                                        CumulationDetail entity = new CumulationDetail();
                                        entity.StartDate = currentDate;
                                        entity.EndDate = newCumulationResetDate;
                                        entity.RequestPriceDetailId = item.Id;
                                        entity.CumulatedQuantity = 0;
                                        entity.IsActive = true;

                                        newlyAddedEntities.Add(entity);
                                        //     reqPriceDetailsEntitiesToUpdate.Add(item.Id, newCumulationResetDate);

                                        isRecordProcessed = true;
                                    }
                                }
                                else if (item.CumulationTypeId == (int)CumulationType.Monthly)
                                {
                                    var newCumulationResetDate = GetNewResetDateForMonthlyReset(cumulationEntity.EndDate, cumulationResetDay);

                                    entityIdsToDeactivate.Add(cumulationEntity.Id);

                                    CumulationDetail entity = new CumulationDetail();

                                    entity.StartDate = currentDate;//currentdate will be start date of new record
                                    entity.EndDate = newCumulationResetDate;
                                    entity.RequestPriceDetailId = item.Id;
                                    entity.CumulatedQuantity = 0;
                                    entity.IsActive = true;

                                    newlyAddedEntities.Add(entity);
                                    //        reqPriceDetailsEntitiesToUpdate.Add(item.Id, newCumulationResetDate);

                                    isRecordProcessed = true;

                                }
                                else if (item.CumulationTypeId == (int)CumulationType.SpecificDates)
                                {

                                    if (currentDate.Date == cumulationEntity.EndDate.Date)
                                    {
                                        var existingStartDate = cumulationEntity.StartDate;

                                        entityIdsToDeactivate.Add(cumulationEntity.Id);


                                        CumulationDetail entity = new CumulationDetail();
                                        entity.StartDate = existingStartDate;
                                        entity.EndDate = currentDate;
                                        entity.RequestPriceDetailId = item.Id;
                                        entity.CumulatedQuantity = 0;
                                        entity.IsActive = true;

                                        newlyAddedEntities.Add(entity);
                                        //          reqPriceDetailsEntitiesToUpdate.Add(item.Id, currentDate);

                                        isRecordProcessed = true;
                                    }
                                }
                            }
                        }

                    }
                    if (entityIdsToDeactivate.Any() && newlyAddedEntities.Any())
                    {
                        var isSuccess = await UpdateCumulationRelatedEntitiesAfterReset(entityIdsToDeactivate, newlyAddedEntities);
                    }

                }
            }
            catch (Exception ex)
            {
                isRecordProcessed = false;
                LogManager.Logger.WriteException("PricingRepository", "ResetCumulation", ex.Message, ex);
            }
            return isRecordProcessed;
        }

        private DateTimeOffset GetNewResetDateForWeeklyReset(DateTimeOffset endDate, int? cumulationResetDay)
        {
            var newCumulationResetDate = DateTimeOffset.Now;
            try
            {
                if (cumulationResetDay.HasValue && cumulationResetDay > 0)
                {
                    var dayOfweek = (int)endDate.DayOfWeek;
                    var daysToGoForNextWeek = 7 - dayOfweek;
                    var actualDaysToAdd = daysToGoForNextWeek == 0 ? 7 : daysToGoForNextWeek;
                    var sundayDate = endDate.AddDays(actualDaysToAdd);// assuming week starts from sunday
                    //S  var actualCumulationResetday = cumulationResetDay - 1;
                    // var newCumulationResetDate = sundayDate.AddDays(cumulationResetDay.Value);
                    newCumulationResetDate = dayOfweek == 0 ? sundayDate : sundayDate.AddDays(cumulationResetDay.Value);

                }
                else //no reset day given add 7 days in enddate
                {
                    newCumulationResetDate = endDate.AddDays(7);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRepository", "GetNewResetDateForWeeklyReset", ex.Message, ex);
                throw;
            }
            return newCumulationResetDate;
        }

        private DateTimeOffset GetNewResetDateForBiWeeklyReset(DateTimeOffset endDate, int? cumulationResetDay)
        {
            var newCumulationResetDate = DateTimeOffset.Now;
            try
            {
                if (cumulationResetDay.HasValue && cumulationResetDay > 0)
                {
                    var dayOfweek = (int)endDate.DayOfWeek;
                    var daysToGoForNextWeek = 7 - dayOfweek;
                    var actualDaysToAdd = daysToGoForNextWeek == 0 ? 7 : daysToGoForNextWeek;
                    var firstsundayDate = endDate.AddDays(actualDaysToAdd);// assuming week starts from sunday
                    var secondSundayDate = firstsundayDate.AddDays(7);
                    //S  var actualCumulationResetday = cumulationResetDay - 1;
                    // var newCumulationResetDate = sundayDate.AddDays(cumulationResetDay.Value);
                    newCumulationResetDate = dayOfweek == 0 ? secondSundayDate : secondSundayDate.AddDays(cumulationResetDay.Value);

                }
                else //no reset day given add 14 days in enddate
                {
                    newCumulationResetDate = endDate.AddDays(14);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRepository", "GetNewResetDateForBiWeeklyReset", ex.Message, ex);
                throw;
            }
            return newCumulationResetDate;
        }

        private DateTimeOffset GetNewResetDateForMonthlyReset(DateTimeOffset endDate, int? cumulationResetDay)
        {
            var newCumulationResetDate = DateTimeOffset.Now;
            try
            {

                newCumulationResetDate = endDate.AddMonths(1);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRepository", "GetNewResetDateForMonthlyReset", ex.Message, ex);
                throw;
            }
            return newCumulationResetDate;
        }

        private async Task<bool> UpdateCumulationRelatedEntitiesAfterReset(List<int> entityIdsToDeactivate, List<CumulationDetail> newlyAddedEntities)
        {
            var isSuccess = false;
            using (var transaction = BeginTransaction())
            {
                try
                {
                    if (entityIdsToDeactivate != null && entityIdsToDeactivate.Any())
                    {
                        var cumulationEntities = await dbContext.CumulationDetails.Where(t => entityIdsToDeactivate.Contains(t.Id)).ToListAsync();
                        if (cumulationEntities != null && cumulationEntities.Any())
                        {
                            var query = $"UPDATE CumulationDetails SET IsActive = {0} WHERE Id IN ({string.Join<int>(",", entityIdsToDeactivate)})";

                            ExecuteSqlCommand(query);
                            //dbContext.Database.ExecuteSqlCommand(query);
                            dbContext.SaveChanges();
                        }
                    }

                    if (newlyAddedEntities != null && newlyAddedEntities.Any())
                    {
                        dbContext.CumulationDetails.AddRange(newlyAddedEntities);
                    }

                    await dbContext.SaveChangesAsync();
                    Commit(transaction); 
                    isSuccess = true;


                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("PricingRepository", "UpdateCumulationRelatedEntitiesAfterReset", ex.Message, ex);
                }
            }
            return isSuccess;
        }

        public async Task<bool> UpdateCumulationQtyPostInvoiceCreate(List<CumulationQuantityUpdateViewModel> cumulationQtyList)
        {
            var response = false;

            using (var transaction = BeginTransaction())
            {
                try
                {

                    if (cumulationQtyList != null && cumulationQtyList.Any())
                    {
                        List<int> RequestPricingDetailIds = new List<int>();
                        cumulationQtyList.ForEach(t => RequestPricingDetailIds.Add(t.RequestPriceDetailsId));

                        if (RequestPricingDetailIds != null && RequestPricingDetailIds.Any())
                        {
                            var cumulationEntities = await dbContext.CumulationDetails.Where(t => RequestPricingDetailIds.Contains(t.RequestPriceDetailId) && t.IsActive).ToListAsync();

                            if (cumulationEntities != null && cumulationEntities.Any())
                            {
                                foreach (var item in cumulationEntities)
                                {
                                    var updatedDroppedGallons = cumulationQtyList.Where(t => t.RequestPriceDetailsId == item.RequestPriceDetailId).Select(t => t.DroppedGallons).FirstOrDefault();
                                    item.CumulatedQuantity = item.CumulatedQuantity + updatedDroppedGallons;
                                    //dbContext.Entry(item).State = EntityState.Modified;
                                    SetEntityStateModified<CumulationDetail>(item);

                                }
                                await dbContext.SaveChangesAsync();
                                response = true;
                                
                                Commit(transaction);

                            }

                        }

                    }

                }
                catch (Exception ex)
                {
                    response = false;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("PricingRepository", "UpdateCumulationQtyPostInvoiceCreate", ex.Message, ex);
                }
            }

            return response;
        }

        //fromQty is cumlatedqty only.
        public async Task<List<TierPricingRequestModel>> GetPricingForVolBasedTierWithCumulationReset(int requestPriceId,decimal fromQuantity, decimal toQuantity)
        {
            var response = new List<TierPricingRequestModel>();
            var actualDroppedGallons = toQuantity - fromQuantity;
            var lastTierqty = await (from pricingDetail in dbContext.PricingDetails
                                     join codeDetail in dbContext.MstPricingCodes on pricingDetail.PricingCodeId equals codeDetail.Id
                                     where pricingDetail.RequestPriceDetailId == requestPriceId &&
                                                 pricingDetail.MaxQuantity == 0
                                                 && pricingDetail.IsActive && codeDetail.IsActive
                                     //select *from PricingDetails where RequestPriceDetailId = 16111 and MaxQuantity > 950 AND MinQuantity<1350
                                     select new TierPricingRequestModel()
                                     {
                                         TierTypeId = pricingDetail.RequestPriceDetails.TierTypeId,
                                         MaxQuantity = pricingDetail.MaxQuantity,
                                         MinQuantity = pricingDetail.MinQuantity,
                                         PricingCodeId = codeDetail.Id,
                                         PricingTypeId = codeDetail.PricingTypeId,
                                         CityGroupTerminalId = pricingDetail.CityRackTerminalId,
                                         ProductId = pricingDetail.FuelTypeId,
                                         TerminalId = pricingDetail.TerminalId,
                                         PricePerGallon = pricingDetail.PricePerGallon,
                                         SupplierCost = pricingDetail.SupplierCost,
                                         SupplierCostTypeId = pricingDetail.SupplierCostTypeId,
                                         RackTypeId = codeDetail.RackTypeId,
                                         RackAvgTypeId = pricingDetail.RackAvgTypeId,
                                         FeedTypeId = codeDetail.FeedTypeId,
                                         PricingSourceId = codeDetail.PricingSourceId,
                                         Currency = pricingDetail.RequestPriceDetails.Currency
                                     }
                       ).FirstOrDefaultAsync();
            if (lastTierqty!=null && fromQuantity > lastTierqty.MinQuantity) // for last tier
            {
                response.Add(lastTierqty);
            }
            else
            {
                response = await (from pricingDetail in dbContext.PricingDetails
                                  join codeDetail in dbContext.MstPricingCodes on pricingDetail.PricingCodeId equals codeDetail.Id
                                  where pricingDetail.RequestPriceDetailId == requestPriceId &&
                                              ((pricingDetail.MaxQuantity > fromQuantity && pricingDetail.MinQuantity < toQuantity) || pricingDetail.MaxQuantity == 0)
                                              && pricingDetail.IsActive && codeDetail.IsActive
                                 // select *from PricingDetails where RequestPriceDetailId = 16112 and((MaxQuantity > 1900 AND MinQuantity < 3900) OR MaxQuantity = 0)
                                  //select *from PricingDetails where RequestPriceDetailId = 16111 and MaxQuantity > 950 AND MinQuantity<1350
                                  select new TierPricingRequestModel()
                                  {
                                      TierTypeId = pricingDetail.RequestPriceDetails.TierTypeId,
                                      MaxQuantity = pricingDetail.MaxQuantity,
                                      MinQuantity = pricingDetail.MinQuantity,
                                      PricingCodeId = codeDetail.Id,
                                      PricingTypeId = codeDetail.PricingTypeId,
                                      CityGroupTerminalId = pricingDetail.CityRackTerminalId,
                                      ProductId = pricingDetail.FuelTypeId,
                                      TerminalId = pricingDetail.TerminalId,
                                      PricePerGallon = pricingDetail.PricePerGallon,
                                      SupplierCost = pricingDetail.SupplierCost,
                                      SupplierCostTypeId = pricingDetail.SupplierCostTypeId,
                                      RackTypeId = codeDetail.RackTypeId,
                                      RackAvgTypeId = pricingDetail.RackAvgTypeId,
                                      FeedTypeId = codeDetail.FeedTypeId,
                                      PricingSourceId = codeDetail.PricingSourceId,
                                      Currency = pricingDetail.RequestPriceDetails.Currency
                                  }
                       ).ToListAsync();
            }
            if (lastTierqty != null && toQuantity <= lastTierqty.MinQuantity)
            {
                response.RemoveAll(t => t.MaxQuantity == 0);
            }
             
            var cumulatedQty = dbContext.CumulationDetails.Where(t => t.RequestPriceDetailId == requestPriceId && t.IsActive)
                                .Select(t => t.CumulatedQuantity).FirstOrDefault();
            response.ForEach(t => t.CumulatedQuantity = cumulatedQty);
            return response;
        }

        public async Task<List<DropdownDisplayExtendedId>> GetSourceRegionForCustomers(List<DropdownDisplayExtendedId> requestPriceDetailIds)
        {
            List<DropdownDisplayExtendedId> lstSourceRegion = null;
            try
            {
                if (requestPriceDetailIds != null && requestPriceDetailIds.Count > 0)
                {
                    lstSourceRegion = new List<DropdownDisplayExtendedId>();
                    foreach (var item in requestPriceDetailIds)
                    {
                        var response = await dbContext.PricingDetails.Where(t =>t.RequestPriceDetailId==item.Id && t.IsActive)
                       .Select(t => t.ParameterJson).ToListAsync();
                        if (response != null && response.Count > 0)
                        {
                            
                            foreach (var json in response)
                            {
                                lstSourceRegion.Add(new DropdownDisplayExtendedId
                                {
                                    CodeId = item.CodeId, //buyer company id.
                                    Name = json
                                });
                            }
                        }
                    }
                    return lstSourceRegion;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRepository", "GetSourceRegionForCustomers", ex.Message, ex);
            }
            return lstSourceRegion;

        }
    }
}
