using System;
using System.Collections.Generic;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.DataAccess.Entities;
using System.Linq;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Core.StringResources;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.Domain
{
    public class ExceptionLogDomain : BaseDomain
    {
        public ExceptionLogDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ExceptionLogDomain(SiteFuelUow SiteFuelDbContext)
          : base(SiteFuelDbContext)
        {
        }

        public ExceptionLogDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public  List<Usp_ExceptionLogsViewModel> GetParticularExceptionsAsync(ExceptionDataTableModel exceptionModel, DataTableSearchModel searchModel)
        {
            List<Usp_ExceptionLogsViewModel> response = new List<Usp_ExceptionLogsViewModel>();
            try
            { 
            var spDomain = new StoredProcedureLogDomain();
            response =  spDomain.GetExceptionLogs(exceptionModel.FromDateTime, exceptionModel.ToDateTime, searchModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionLogDomain", "GetParicularExceptionsAsync", ex.Message, ex);
            }
            return response;
        }

        public String GetException(int id)
        {
            var exception = "";
            try
            {
            var spDomain = new StoredProcedureLogDomain();
            exception = spDomain.GetParticularException(id);
            }
            catch (Exception ex)
            {
            LogManager.Logger.WriteException("ExceptionLogDomain", "GetException", ex.Message, ex);
            }
            return exception;
        }

        public void AddAuditLogs(List<AuditLog> auditlogs)
        {
            try
            {
                Context.DataContext.AuditLogs.AddRange(auditlogs);
                Context.Commit();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionLogDomain", "AddAuditLog", ex.Message, ex);
            }
        }

        public void AddApiLogs(ApiLog apiLog)
        {
            try
            {
                //var companyId = Context.DataContext.Users.Where(t => t.Id == apiLog.CreatedBy).Select(s => s.CompanyId).FirstOrDefault();
                //if (companyId != null)
                //    apiLog.CompanyId = companyId.Value;
                 //Context.DataContext.ApiLogs.Add(apiLog);
                // Context.DataContext.SaveChanges();
                var storedProcedureDomain = new StoredProcedureDomain(this);
                storedProcedureDomain.AddApiLogs(apiLog);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionLogDomain", "AddApiLogs", ex.Message, ex);
            }
        }

        public async Task<ApiDetailLogViewModel> GetApiLogs(ApiLogViewModel model)
        {
            var response = new ApiDetailLogViewModel();
            var logList = new List<ApiLogViewModel>();
            try
            {
                if ( model.FromDate != DateTimeOffset.MinValue && model.ToDate != DateTimeOffset.MinValue)
                {
                    model.ToDate = model.ToDate.AddDays(1);
                    //logList = Context.DataContext.ApiLogs.Where(w => w.CompanyId == model.CompanyId && w.CreatedDate >= model.FromDate && w.CreatedDate <= model.ToDate && w.Message!= Resource.PostLiftValidateStatusCalled).Select(s=>new ApiLogViewModel { 
                    //Id=s.Id,
                    //CompanyId=s.CompanyId,
                    //CreatedBy=s.CreatedBy,
                    //CreatedDate=s.CreatedDate,
                    //Message=s.Message,
                    //ExternalRefID=s.ExternalRefID,
                    //Url=s.Url
                    //}).OrderByDescending(o=>o.Id).ToList();
                    var storedProcedureDomain = new StoredProcedureDomain();
                    logList = await storedProcedureDomain.GetApiLogsForCompany(model.CompanyId, model.FromDate, model.ToDate);
                }
                if (!string.IsNullOrWhiteSpace(model.Url))
                    logList = logList.Where(w => w.Url==(model.Url)).ToList();
                if (logList.AnyAndNotNull())
                {
                    response.TotalCall = logList.Count;
                    response.SuccessCall = logList.Count(w => Convert.ToInt32(w.Message) == (int)Utilities.APIResultType.Success);
                    response.FailedCall = logList.Count(w => Convert.ToInt32(w.Message) == (int)Utilities.APIResultType.Exception);
                }
                if (Convert.ToInt32(model.Message) != (int)Utilities.APIResultType.Total)
                    logList = logList.Where(w => w.Message==model.Message).ToList();
               
                response.ApiLogList = logList;
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionLogDomain", "GetApiLogs", ex.Message, ex);
            }
            return response;
        }

        public async Task<ApiLogViewModel> GetApiLogRequestResponse(ApiLogViewModel model,UserContext userContext)
        {
            var response = new ApiLogViewModel();
            try
            {
                var spDomain = new StoredProcedureDomain();
                var apiLog = await spDomain.GetApiLogsForCompany(userContext.CompanyId, null, null, model.Id);


                if (apiLog.AnyAndNotNull())
                {
                    if (model.ReqResType == (int)Utilities.ReqResType.Request)
                    {
                        response.Request = apiLog.FirstOrDefault().Request;
                    }
                    else if (model.ReqResType == (int)Utilities.ReqResType.Response)
                    {
                        response.Response = apiLog.FirstOrDefault().Response;
                    }
                }
                //if (model.ReqResType == (int)Utilities.ReqResType.Request)
                //{
                //    response = Context.DataContext.ApiLogs.Where(w => w.Id == model.Id).Select(s => new ApiLogViewModel
                //                {
                //                    Request = s.Request,
                //                }).FirstOrDefault();
                //}                   
                //else if (model.ReqResType == (int)Utilities.ReqResType.Response) 
                //{
                //    response = Context.DataContext.ApiLogs.Where(w => w.Id == model.Id).Select(s => new ApiLogViewModel
                //                {
                //                    Response = s.Response,
                //                }).FirstOrDefault();

                //}                    
                //return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionLogDomain", "GetApiLogRequestResponse", ex.Message, ex);
            }
            return response;
        }
    }
}
