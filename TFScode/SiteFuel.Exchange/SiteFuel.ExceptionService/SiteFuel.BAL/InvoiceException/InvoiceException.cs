using SiteFuel.BAL.ExceptionEngine;
using SiteFuel.DAL.Create;
using SiteFuel.DAL.Read;
using SiteFuel.DAL.Update;
using SiteFuel.DAL.Usp;
using SiteFuel.Exchange.Logger;
using SiteFuel.Models.ApiModels;
using SiteFuel.Models.Common.Enums;
using SiteFuel.Models.CompanyException;
using SiteFuel.Models.InvoiceException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL.InvoiceException
{
    public class InvoiceException
    {
        private static IEnumerable<IExceptionEngine> exceptionEngines;
        public InvoiceException()
        {
            if (exceptionEngines == null)
            {
                exceptionEngines = (from t in (AppDomain.CurrentDomain.GetAssemblies().
                                    SingleOrDefault(assembly => assembly.GetName().Name == "SiteFuel.BAL").GetTypes())
                                    where t.GetInterfaces().Contains(typeof(IExceptionEngine))
                                    && t.GetConstructor(Type.EmptyTypes) != null
                                    select Activator.CreateInstance(t) as IExceptionEngine).ToList();
            }
        }

        public async Task<bool> ApproveException(ExceptionApprovalRequestModel model)
        {
            var response = false;
            try
            {
                var updateLayer = new ExceptionUpdateLayer();
                response = await updateLayer.ApproveException(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceException", "ApproveException", ex.Message, ex);
                
            }
            return response;
        }

        public async Task<InvoiceExceptionResponseModel> CheckExceptions(List<InvoiceExceptionRequestModel> models)
        {
            var response = new InvoiceExceptionResponseModel();
            try
            {
                var readLayer = new ExceptionReadLayer();
                var model = models.FirstOrDefault();
                var brokeredOrders = models.Where(t => t.BrokeredOrders != null).Select(t => t.BrokeredOrders).FirstOrDefault();
                var enabledExceptions = await readLayer.GetEnabledExceptions(model.SupplierCompanyId, model.BuyerCompanyId, brokeredOrders);
                var generatedExceptions = new List<GeneratedExceptionModel>();
                foreach (var exception in enabledExceptions)
                {
                    var engine = exceptionEngines.FirstOrDefault(t => t.Type == (ExceptionType)exception.ExceptionTypeId);                    
                    var exceptionModels = models.Where(t => t.ExceptionTypeId == exception.ExceptionTypeId).ToList();
                    if (exceptionModels != null && exceptionModels.Any())
                    {
                        foreach (var exceptionModel in exceptionModels)
                        {
                            var generatedException = engine.CheckException(exceptionModel, exception);
                            if (generatedException != null)
                            {
                                generatedExceptions.Add(generatedException);
                            }
                        }
                    }
                }
                if (generatedExceptions.Any())
                {
                    var createLayer = new ExceptionCreateLayer(readLayer);
                    response.Exceptions = await createLayer.SaveGeneratedExceptions(generatedExceptions);
                    response.IsExceptionsRaised = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceException", "CheckExceptions", ex.Message, ex);
            }
            return response;
        }
        public async Task<CompanyApprovalExceptionModel> GetMyApprovalExceptions(int approvalCompanyId)
        {
            var response = new CompanyApprovalExceptionModel();
            try
            {
                var spLayer = new UspLayer();
                var exceptions = await spLayer.GetDeliveredQuantityVarianceExceptions(approvalCompanyId);
                response.GeneratedExceptions = exceptions;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceException", "GetMyApprovalExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveredQuantityVarianceExceptionModel>> GetBuyerApprovalExceptions(int supplierCompanyId, string exceptionTypes)
        {
            var response = new List<DeliveredQuantityVarianceExceptionModel>();
            try
            {
                var spLayer = new UspLayer();
                response = await spLayer.GetBuyerApprovalExceptions(supplierCompanyId, exceptionTypes);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceException", "GetBuyerApprovalExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<CompanyApprovalExceptionModel> GetSupplierApprovalExceptions(int buyerCompanyId, string exceptionTypes)
        {
            var response = new CompanyApprovalExceptionModel();
            try
            {
                var spLayer = new UspLayer();
                var exceptions = await spLayer.GetSupplierApprovalExceptions(buyerCompanyId, exceptionTypes);
                response.GeneratedExceptions = exceptions;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceException", "GetSupplierApprovalExceptions", ex.Message, ex);
            }
            return response;
        }

        /// <summary>
        /// Returns auto approval exception list
        /// </summary>
        /// <param name="holidayList">Holiday dates in YYYY/MM/DD format with semicolon (;) as separator</param>
        /// <param name="isSaturdayOff">Boolean value</param>
        /// <returns></returns>
        public async Task<List<AutoApprovalExceptionModel>> GetAutoApprovalExceptions(string holidayList, bool isSaturdayOff)
        {
            var response = new List<AutoApprovalExceptionModel>();
            try
            {
                var spLayer = new UspLayer();
                response = await spLayer.GetAutoApprovalExceptions(holidayList, isSaturdayOff);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceException", "GetAutoApprovalExceptions", ex.Message, ex);
                
            }
            return response;
        }

        public async Task<CompanyApprovalExceptionModel> GetExceptionsForApproval(int approvalCompanyId, string exceptionTypes)
        {
            var response = new CompanyApprovalExceptionModel();
            try
            {
                var spLayer = new UspLayer();
                var exceptions = await spLayer.GetExceptionsForApproval(approvalCompanyId, exceptionTypes);
                response.GeneratedExceptions = exceptions;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceException", "GetExceptionsForApproval", ex.Message, ex);
            }
            return response;
        }

        public async Task<InvoiceExceptionResponseModel> RaiseException(InvoiceExceptionRequestModel model)
        {
            var response = new InvoiceExceptionResponseModel();
            try
            {
                var readLayer = new ExceptionReadLayer();
                var enabledExceptions = await readLayer.GetEnabledException(model.SupplierCompanyId, model.ExceptionTypeId);
                var generatedExceptions = new List<GeneratedExceptionModel>();
                foreach (var exception in enabledExceptions)
                {
                    var engine = exceptionEngines.FirstOrDefault(t => t.Type == (ExceptionType)exception.ExceptionTypeId);
                    var generatedException = engine.CheckException(model, exception);
                    if (generatedException != null)
                    {
                        if (exception.AutoApprovalDays.HasValue && exception.AutoApprovalDays > 0)
                            generatedException.AutoApprovedOn = DateTimeOffset.Now.AddHours(Convert.ToInt32(exception.AutoApprovalDays) * 24);
                        generatedExceptions.Add(generatedException);
                    }
                }
                if (generatedExceptions.Any())
                {
                    var createLayer = new ExceptionCreateLayer(readLayer);
                    response.Exceptions = await createLayer.SaveGeneratedExceptions(generatedExceptions);
                    response.IsExceptionsRaised = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceException", "RaiseException", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetExceptionIdsForAutoRejection(int exceptionTypeId, int statusId)
        {
            var response = new List<int>();
            try
            {
                var spLayer = new UspLayer();
                response = await spLayer.GetExceptionIdsForAutoRejection(exceptionTypeId, statusId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceException", "GetExceptionIdsForAutoRejection", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> UpdateExceptionIdForAutoReject(List<int> generatedExceptionIdList)
        {
            var response = false;
            try
            {
                var readLayer = new ExceptionReadLayer();
                var createLayer = new ExceptionCreateLayer(readLayer);
                response = await createLayer.UpdateGeneratedExceptionForAutoReject(generatedExceptionIdList);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceException", "UpdateExceptionIdForAutoReject", ex.Message, ex);
            }
            return response;
        }
    }
}
