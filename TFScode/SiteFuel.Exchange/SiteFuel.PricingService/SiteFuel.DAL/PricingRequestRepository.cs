using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Models;
using SiteFuel.Exchange.Utilities;
using SiteFuel.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using System.Data.Entity;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace SiteFuel.DAL
{
    public class PricingRequestRepository : IPricingRequestRepository
    {
        private DataContext dbContext;
        public PricingRequestRepository()
        {
            dbContext = new DataContext();
        }

        public async Task<CustomResponseModel> SaveRequestDetails(RequestPriceDetail priceDetail)
        {
            var responseModel = new CustomResponseModel();
            try
            {
                dbContext.RequestPriceDetails.Add(priceDetail);
                var result = await dbContext.SaveChangesAsync();
                responseModel.Result = priceDetail.Id;
                responseModel.Status = Status.Success;
            }
            catch (Exception ex)
            {
                responseModel.Status = Status.Failed;
                LogManager.Logger.WriteException("PricingRequestRepository", "SaveRequestDetails", ex.Message, ex);
            }
            return responseModel;
        }

        public async Task<CustomResponseModel> UpdateRequestDetails(RequestPriceDetail priceDetail)
        {
            var responseModel = new CustomResponseModel();
            try
            {
                priceDetail.PricingDetails.Where(t => t.Id > 0 && t.IsActive).ToList().ForEach(t => t.IsActive = false);
                dbContext.Entry<RequestPriceDetail>(priceDetail).State = EntityState.Modified;
                var result = await dbContext.SaveChangesAsync();
                responseModel.Result = priceDetail.Id;
                responseModel.Status = Status.Success;
            }
            catch (Exception ex)
            {
                responseModel.Status = Status.Failed;
                LogManager.Logger.WriteException("PricingRequestRepository", "SaveRequestDetails", ex.Message, ex);
            }
            return responseModel;
        }

        public async Task<CustomResponseModel> UpdateSourceRegion(SourceRegionPricingRequestModel model)
        {
            var responseModel = new CustomResponseModel();
            try
            {
                var prcingDtl = dbContext.RequestPriceDetails.Where(w => w.Id == model.RequestPricingDetailId).SingleOrDefault().PricingDetails.Where(Pri=>Pri.IsActive==true).FirstOrDefault();
                if (prcingDtl != null)
                {
                    prcingDtl.ParameterJson = model.ParameterJSON;
                    prcingDtl.TerminalId = model.TerminalId;
                    var result = await dbContext.SaveChangesAsync();
                    responseModel.Result = prcingDtl.RequestPriceDetailId;
                    responseModel.Status = Status.Success;
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = Status.Failed;
                LogManager.Logger.WriteException("PricingRequestRepository", "UpdateSourceRegion", ex.Message, ex);
            }
            return responseModel;
        }

        public async Task<CustomResponseModel> SavePricingDetails(List<PricingDetail> priceDetails)
        {
            var responseModel = new CustomResponseModel();
            try
            {
                dbContext.PricingDetails.AddRange(priceDetails);
                var result = await dbContext.SaveChangesAsync();
                responseModel.Result = priceDetails.FirstOrDefault().RequestPriceDetailId;
                responseModel.Status = Status.Success;
            }
            catch (Exception ex)
            {
                responseModel.Status = Status.Failed;
                LogManager.Logger.WriteException("PricingRequestRepository", "SavePricingDetails", ex.Message, ex);
            }
            return responseModel;
        }
        public async Task<PricingCodesResponseModel> GetPricingCodesAsync(PricingCodesRequestModel requestModel, int timeout = 30)
        {
            var result = new PricingCodesResponseModel();
            try
            {
                var inputmodel = new
                {
                    PricingSourceId = requestModel.PricingSourceId ?? 0,
                    PricingTypeId = requestModel.PricingTypeId ?? 0,
                    FeedTypeId = requestModel.FeedTypeId ?? 0,
                    QuantityIndicatorId = requestModel.QuantityIndicatorId ?? 0,
                    FuelClassTypeId = requestModel.FuelClassTypeId ?? 0,
                    RackTypeId = requestModel.RackTypeId ?? 0,
                    WeekendPricingTypeId = requestModel.WeekendPricingTypeId ?? 0,
                    Search = requestModel.Search ?? "",
                    TfxProductId = requestModel.TFxProductId ?? 0
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetPricingCodes", inputmodel);

                dbContext.Database.CommandTimeout = timeout;
                result.PricingCodes = await dbContext.Database.SqlQuery<PricingCodesModel>(input.Query, input.Params.ToArray()).ToListAsync();

                result.Status = Status.Success;
                result.Message = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                result.Status = Status.Failed;
                result.Message = Status.Failed.ToString();
                LogManager.Logger.WriteException("PricingRequestRepository", "GetPricingCodesAsync", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<int>> GetRequestPriceDetailIdsByPricingSourceAsync(RequestPriceBySourceInputViewModel inputModel)
        {
            var response = new List<int>();

            //if (inputModel.IsAxxis)
            //    response = await dbContext.RequestPriceDetails.Where(t => inputModel.RequestPriceDetailIds.Contains(t.Id) && t.MstPricingCode.PricingSourceId == (int)PricingSource.Axxis).Select(t => t.Id).ToListAsync();
            //else
            //    response = await dbContext.RequestPriceDetails.Where(t => inputModel.RequestPriceDetailIds.Contains(t.Id) && t.MstPricingCode.PricingSourceId != (int)PricingSource.Axxis).Select(t => t.Id).ToListAsync();

            if (inputModel.IsAxxis)
                response = await dbContext.PricingDetails.Where(t => inputModel.RequestPriceDetailIds.Contains(t.RequestPriceDetailId) && t.MstPricingCode.PricingSourceId == (int)PricingSource.Axxis && t.IsActive).Select(t => t.RequestPriceDetailId).ToListAsync();
            else
                response = await dbContext.PricingDetails.Where(t => inputModel.RequestPriceDetailIds.Contains(t.RequestPriceDetailId) && t.MstPricingCode.PricingSourceId != (int)PricingSource.Axxis && t.IsActive).Select(t => t.RequestPriceDetailId).ToListAsync();
            return response;
        }

        public async Task<MstPricingCode> GetCodeDetails(int codeId)
        {
            return await dbContext.MstPricingCodes.Where(t => t.Id == codeId).SingleOrDefaultAsync();
        }

        private int? GetPricingTypeByRequestPriceId(int requestPriceId)
        {
            if (requestPriceId <= 0)
                return null;
            return  dbContext.RequestPriceDetails.Where(t => t.Id == requestPriceId).FirstOrDefault().PricingTypeId;
        }

        public async Task<RequestPriceDetail> GetPricingRequestDetailByIdAsync(int id)
        {
            return await dbContext.RequestPriceDetails.FirstOrDefaultAsync(testc => testc.Id == id);
        }

        public async Task<PricingRequestDetailResponseModel> GetPricingRequestDetailByIdAsync(PricingRequestViewModel requestModel, int timeout = 30)
        {
            var result = new PricingRequestDetailResponseModel();
            try
            {
                int? priceTypeId = GetPricingTypeByRequestPriceId(requestModel.Id);
                var inputmodel = new
                {
                    Id = requestModel.Id,
                    AcceptedCompanyId = requestModel.AcceptedCompanyId ?? 0,
                    FuelTypeId = requestModel.FuelTypeId ?? 0,
                    StateId = requestModel.StateId ?? 0,
                    Currency = requestModel.Currency == 0 ? 1 : requestModel.Currency,
                    PricingCodeId = requestModel.PricingCodeId,
                    PricingSourceId = requestModel.PricingSourceId ?? 0,
                    PricingTypeId = requestModel .PricingTypeId,
                    RackTypeId = requestModel.RackTypeId ?? 0,
                    FeedTypeId = requestModel.FeedTypeId ?? 0,
                    QuantityIndicatorId = requestModel.QuantityIndicatorId ?? 0,
                    FuelClassTypeId = requestModel.FuelClassTypeId ?? 0,
                    WeekendPricingTypeId = requestModel.WeekendPricingTypeId ?? 0
                };
                string procedureName = priceTypeId == (int)PricingType.Tier ? "usp_GetTierPricingRequestDetailById" : "usp_GetPricingRequestDetailById";
                var input = SqlHelperMethods.GetStoredProcedure(procedureName, inputmodel);

                dbContext.Database.CommandTimeout = timeout;
                if (priceTypeId == (int)PricingType.Tier)
                {
                    List<TierPricing> TierPricings = await dbContext.Database.SqlQuery<TierPricing>(input.Query, input.Params.ToArray()).ToListAsync();
                    if(TierPricings !=null && TierPricings.Any())
                    {
                        result.PricingRequestDetail = new PricingRequestDetailModel();
                        result.PricingRequestDetail.TierPricings = TierPricings;
                    }
                }
                else
                    result.PricingRequestDetail = await dbContext.Database.SqlQuery<PricingRequestDetailModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
                   result.Status = Status.Success;
                    result.Message = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                result.Status = Status.Failed;
                result.Message = Status.Failed.ToString();
                LogManager.Logger.WriteException("PricingRequestRepository", "GetPricingRequestDetailByIdAsync", ex.Message, ex);
            }
            return result;
        }

        public PricingDetailResponseModelForExchangeAPI GetPricingDetailsByIdList(List<int> requestPriceDetailIds, int timeout = 30)
        {
            var result = new PricingDetailResponseModelForExchangeAPI();
            try
            {
                if(requestPriceDetailIds == null || !requestPriceDetailIds.Any())
                {
                    result.Status = Status.Failed;
                    result.Message = Status.Failed.ToString();
                    return result;
                }

                string procedureName = "usp_GetPricingDetailsForAPI";
                var response = new List<PricingDetailModel>();
                response = ExecuteSPByIdList(response, procedureName, requestPriceDetailIds);

                result.PricingDetails = response;
                result.Status = Status.Success;
                result.Message = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                result.Status = Status.Failed;
                result.Message = Status.Failed.ToString();
                LogManager.Logger.WriteException("PricingRequestRepository", "GetPricingDetailsByIdList", ex.Message, ex);
            }
            return result;
        }

        public List<T> ExecuteSPByIdList<T>(List<T> response, string procedureName, List<int> ids)
        {
            if (response == null)
                response = new List<T>();

            DataTable objDt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["PricingDatabaseConnection"] != null ? ConfigurationManager.ConnectionStrings["PricingDatabaseConnection"].ConnectionString : string.Empty;
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(procedureName))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        DataTable table = new DataTable();
                        table.Columns.Add($"IdSearchTypes", typeof(int));
                        foreach (var item in ids)
                        {
                            table.Rows.Add(item);
                        }
                        cmd.Parameters.AddWithValue($"IdSearchTypes", table);

                        SqlDataAdapter objDa = new SqlDataAdapter();
                        objDa.SelectCommand = cmd;
                        DataSet objDs = new DataSet();
                        objDa.Fill(objDs);
                        if (objDs.Tables.Count > 0)
                        {
                            objDt = objDs.Tables[0];
                        }
                        con.Close();
                    }
                }
                response = ConvertDataTableToList<T>(objDt);
            }

            return response;
        }

        private static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        //pro.SetValue(obj, dr[column.ColumnName], null);
                        pro.SetValue(obj, dr.IsNull(column.ColumnName) ? null : dr[column.ColumnName], null);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        public async Task<IntResponseModel> GetFilterPriceDetailsByPricingType(FilterPricingRequestViewModel requestModel)
        {
            IntResponseModel response = new IntResponseModel();
            try
            {
                //response.ListResult = await dbContext.RequestPriceDetails
                //                            .Where(t => t.SupplierCostTypeId == requestModel.PricingType && requestModel.PriceDetailIds.Contains(t.Id))
                //                            .Select(t => t.Id)
                //                            .ToListAsync();

                response.ListResult = await dbContext.PricingDetails
                                            .Where(t => t.SupplierCostTypeId == requestModel.PricingType && t.IsActive && requestModel.PriceDetailIds.Contains(t.RequestPriceDetailId))
                                            .Select(t => t.RequestPriceDetailId)
                                            .ToListAsync();
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingRequestRepository", "GetFilterPriceDetailsByPricingType", ex.Message, ex);
            }
            return response;
        }
    }
}
