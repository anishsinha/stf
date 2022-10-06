using Easy.Common;
using Easy.Common.Interfaces;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class ExceptionDomain : BaseDomain
    {
        private string _exceptionApiBaseUrl = string.Empty;
        public ExceptionDomain() : base(ContextFactory.Current.ConnectionString)
        {
            InstanceInitialize();
        }

        public ExceptionDomain(BaseDomain domain) : base(domain)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            var appDomain = new ApplicationDomain(this);
            _exceptionApiBaseUrl = appDomain.GetKeySettingValue("ExceptionApiBaseUrl", "");
            //_exceptionApiBaseUrl = "https://localhost:44355/api/";
        }

        public async Task<ManageExceptionModel> GetCompanyExceptions(UserContext userContext)
        {
            var response = new ManageExceptionModel();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetCompanyExceptions;
                var apiPageUrl = string.Format(apiUrl, userContext.CompanyId);
                response = await ApiGetCall<ManageExceptionModel>(apiPageUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetCompanyExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveCompanyExceptions(UserContext userContext, ManageExceptionModel model)
        {
            var response = new StatusViewModel();
            try
            {
                model.UserId = userContext.Id;
                model.OwnerCompanyId = userContext.CompanyId;
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlSaveCompanyExceptions;
                response = await ApiPostCall<StatusViewModel>(apiUrl, model);
                response.StatusMessage = response.StatusCode == Status.Success ? "Exceptions save successfully" : "Failed to save exceptions";
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetCompanyExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> IsExceptionEnabled(UserContext userContext)
        {
            var response = false;
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlIsExceptionsEnabled;
                var apiPageUrl = string.Format(apiUrl, userContext.CompanyId);
                response = await ApiGetCall<bool>(apiPageUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetCustomerExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> IsExceptionEnabledByType(int ownerCompanyId, int exceptionTypeId)
        {
            var response = false;
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlIsExceptionsEnabledByType;
                var apiPageUrl = string.Format(apiUrl, ownerCompanyId, exceptionTypeId);
                response = await ApiGetCall<bool>(apiPageUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "IsExceptionEnabledByType", ex.Message, ex);
            }
            return response;
        }

        public async Task<ManageCustomerExceptionModel> GetCustomerExceptions(UserContext userContext, int enabledForCompanyId)
        {
            var response = new ManageCustomerExceptionModel();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetCustomerExceptions;
                var apiPageUrl = string.Format(apiUrl, userContext.CompanyId, enabledForCompanyId);
                response = await ApiGetCall<ManageCustomerExceptionModel>(apiPageUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetCustomerExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<ManageCustomerExceptionModel> SaveCustomerExceptions(UserContext userContext, ManageCustomerExceptionModel model)
        {
            var response = new ManageCustomerExceptionModel();
            try
            {
                model.UserId = userContext.Id;
                model.OwnerCompanyId = userContext.CompanyId;
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlSaveCustomerExceptions;
                response = await ApiPostCall<ManageCustomerExceptionModel>(apiUrl, model);
                response.StatusMessage = response.StatusCode == Status.Success ? "Customer exceptions save successfully" : "Failed to save customer exceptions";
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "SaveCustomerExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<InvoiceExceptionResponseModel> CheckExceptions(List<InvoiceExceptionRequestModel> model)
        {
            var response = new InvoiceExceptionResponseModel();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlCheckInvoiceExceptions;
                response = await ApiPostCall<InvoiceExceptionResponseModel>(apiUrl, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "CheckExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<InvoiceExceptionResponseModel> CheckInvoiceApiExceptions(InvoiceExceptionRequestModel model)
        {
            var response = new InvoiceExceptionResponseModel();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlCheckInvoiceApiExceptions;
                response = await ApiPostCall<InvoiceExceptionResponseModel>(apiUrl, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "CheckInvoiceApiExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<CompanyApprovalExceptionModel> GetMyApprovalExceptions(UserContext userContext)
        {
            var response = new CompanyApprovalExceptionModel();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetMyApprovalExceptions;
                var apiPageUrl = string.Format(apiUrl, userContext.CompanyId);
                response = await ApiGetCall<CompanyApprovalExceptionModel>(apiPageUrl);

                if (response != null && response.GeneratedExceptions != null && response.GeneratedExceptions.Any())
                {
                    var generatedExceptionIds = new List<int>();
                    if (userContext.IsBuyer || userContext.IsBuyerAdmin)
                    {
                        generatedExceptionIds = await Context.DataContext.InvoiceExceptions.Where(t => t.Invoice.Order.BuyerCompanyId == userContext.CompanyId
                                                                                           )
                                                                                           .Select(t1 => t1.GeneratedExceptionId).Distinct().ToListAsync();
                    }
                    else
                    {
                        generatedExceptionIds = await Context.DataContext.InvoiceExceptions.Where(t => t.Invoice.Order.AcceptedCompanyId == userContext.CompanyId
                                                                                            )
                                                                                            .Select(t1 => t1.GeneratedExceptionId).Distinct().ToListAsync();
                    }
                    response.GeneratedExceptions = response.GeneratedExceptions.Where(t => generatedExceptionIds.Contains(t.Id)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetMyApprovalExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<TrailerFuelRetainException> GetRetainFuelExceptionData(UserContext userContext)
        {
            var response = new TrailerFuelRetainException();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetCompanyExceptions;
                var apiPageUrl = string.Format(apiUrl, userContext.CompanyId);
                var exceptionModel = await ApiGetCall<ManageExceptionModel>(apiPageUrl);
                var RetainException = exceptionModel.Exceptions.FirstOrDefault(ex => ex.TypeName == ApplicationConstants.KeyRetainManagementException);
                if (RetainException != null && RetainException.IsActive)
                {
                    response.companyExceptionModel = RetainException;
                    response.truckDetailViewModels = await new FreightServiceDomain(this).GetAllTruckFuelRetainDetails(userContext.CompanyId);
                }
                else
                {
                    response.companyExceptionModel = RetainException;
                    response.truckDetailViewModels = new List<TruckDetailViewModel>();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetRetainFuelExceptionData", ex.Message, ex);
            }
            return response;
        }

        public async Task<CompanyApprovalExceptionModel> GetGeneratedExceptionsForApproval(string exceptionTypes, UserContext userContext)
        {
            var response = new CompanyApprovalExceptionModel();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetExceptionsForApproval;
                var apiPageUrl = string.Format(apiUrl, userContext.CompanyId, exceptionTypes);
                response = await ApiGetCall<CompanyApprovalExceptionModel>(apiPageUrl);

                if (response != null && response.GeneratedExceptions != null && response.GeneratedExceptions.Any())
                {
                    UpdateExceptionsForExchangeData(response, exceptionTypes);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetGeneratedExceptionsForApproval", ex.Message, ex);
            }
            return response;
        }

        private void UpdateExceptionsForExchangeData(CompanyApprovalExceptionModel response, string exceptionTypes)
        {
            int exceptionTypeId = 0;
            int.TryParse(exceptionTypes, out exceptionTypeId);
            try
            {
                if (exceptionTypeId == (int)ExceptionType.DuplicateInvoice)
                {
                    foreach (var ddt in response.GeneratedExceptions)
                    {
                        if (ddt.ExceptionAdditionalDetail != null)
                        {
                            ddt.OrigionalInvoice = JsonConvert.DeserializeObject<InvoiceExceptionModel>(ddt.ExceptionAdditionalDetail);
                            if (ddt.OrigionalInvoice != null && ddt.OrigionalInvoice.InvoiceId.HasValue)
                            {
                                var originalInvoice = Context.DataContext.Invoices.FirstOrDefault(t => t.Id == ddt.OrigionalInvoice.InvoiceId.Value);
                                ddt.OrigionalInvoice.InvoiceHeaderId = originalInvoice.InvoiceHeaderId;
                            }
                        }
                        var invoiceException = Context.DataContext.InvoiceExceptions.Where(t => t.GeneratedExceptionId == ddt.Id).Select(t => new { t.InvoiceId, t.Invoice.InvoiceHeaderId }).FirstOrDefault();
                        if (invoiceException != null)
                        {
                            ddt.InvoiceId = invoiceException.InvoiceId;
                            ddt.InvoiceHeaderId = invoiceException.InvoiceHeaderId;
                        }
                    }
                }
                else if (exceptionTypeId == (int)ExceptionType.InvoiceApiException)
                {
                    foreach (var ddt in response.GeneratedExceptions)
                    {
                        var invoiceException = Context.DataContext.InvoiceExceptions.Where(t => t.GeneratedExceptionId == ddt.Id)
                                                                   .Select(t => new { t.InvoiceId, t.Invoice.InvoiceHeaderId })
                                                                   .FirstOrDefault();
                        if (invoiceException != null)
                            ddt.InvoiceId = invoiceException.InvoiceId;
                    }
                }
                else if (exceptionTypeId == (int)ExceptionType.UnknownDeliveries)
                {
                    response.DeliveryMismatchExceptions = new List<DeliveryMismatchExceptionModel>();
                    response.GeneratedExceptions.ForEach(t => response.DeliveryMismatchExceptions.Add(t.ToExceptionRequestModel()));

                    foreach (var exception in response.DeliveryMismatchExceptions)
                    {
                        if (exception.ExceptionAdditionalDetail != null)
                        {
                            var otherDetail = JsonConvert.DeserializeObject<JobTankDetailsViewModel>(exception.ExceptionAdditionalDetail);
                            exception.JobAddress = otherDetail.JobAddress;
                            exception.ProductName = otherDetail.ProductName;
                            exception.ReasonOfFailure = otherDetail.ReasonOfFailure;
                            exception.JobId = otherDetail.JobId;
                            exception.SiteId = otherDetail.SiteId;
                            exception.TankId = otherDetail.TankId;
                            exception.StorageId = otherDetail.StorageId;
                            exception.CaptureTime = otherDetail.CaptureTime;
                            exception.Ullage = otherDetail.Ullage;
                            exception.PrevUllage = otherDetail.PrevUllage;
                        }
                    }
                }
                else if (exceptionTypeId == (int)ExceptionType.MissingDeliveries)
                {
                    response.DeliveryMismatchExceptions = new List<DeliveryMismatchExceptionModel>();
                    response.GeneratedExceptions.ForEach(t => response.DeliveryMismatchExceptions.Add(t.ToExceptionRequestModel()));
                    foreach (var ddt in response.DeliveryMismatchExceptions)
                    {
                        if (ddt.ExceptionAdditionalDetail != null)
                        {
                            var otherDetail = JsonConvert.DeserializeObject<List<MissedDeliveryExceptionOtherDetails>>(ddt.ExceptionAdditionalDetail);
                            ddt.JobAddress = otherDetail.Select(t => t.JobAddress).FirstOrDefault();
                            ddt.ProductName = string.Join(",", otherDetail.Select(t => t.ProductName).ToList());
                            ddt.PoNumber = string.Join(",", otherDetail.Select(t => t.PoNumber).ToList());
                            ddt.InvoiceNumber = string.Join(",", otherDetail.Select(t => t.DisplayInvoiceNumber).Distinct());
                            var orderAndInvoiceIds = string.Empty;
                            foreach (var item in otherDetail)
                            {
                                orderAndInvoiceIds = string.IsNullOrEmpty(orderAndInvoiceIds) ?
                                    item.OrderId + "," + item.InvoiceId + "," + item.TfxProductId + "," + item.ProductName : orderAndInvoiceIds + "^" + item.OrderId + "," + item.InvoiceId + "," + item.TfxProductId + "," + item.ProductName;
                            }
                            ddt.OrderAndInvoiceIds = orderAndInvoiceIds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "UpdateExceptionsForExchangeData", ex.Message, ex);
            }
        }

        public async Task<bool> ApproveException(List<int> exceptionIds, ExceptionResolution resolutionTypeId, int statusId)
        {
            var response = false;
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlApproveException;
                var apiPageUrl = string.Format(apiUrl, exceptionIds, resolutionTypeId);
                var approvalInputs = new ExceptionApprovalRequestModel
                {
                    ExceptionIds = exceptionIds,
                    ResolutionTypeId = (int)resolutionTypeId,
                    StatusId = statusId
                };
                response = await ApiPostCall<bool>(apiPageUrl, approvalInputs);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "ApproveException", ex.Message, ex);
            }
            return response;
        }
        public async Task<bool> CheckPendingExceptions(int exceptionTypeId, List<int> pendingExceptionIds = null)
        {
            var response = false;
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlIsPendingException;
                var approvalInputs = new ExceptionApprovalRequestModel
                {
                    ExceptionTypeId = exceptionTypeId,
                    PendingExceptionIds = pendingExceptionIds
                };
                response = await ApiPostCall<bool>(apiUrl, approvalInputs);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "CheckPendingExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveredQuantityVarianceModel>> GetBuyerApprovalExceptions(string exceptionTypes, UserContext userContext)
        {
            var response = new List<DeliveredQuantityVarianceModel>();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetBuyerApprovalExceptions;
                var apiPageUrl = string.Format(apiUrl, userContext.CompanyId, exceptionTypes);
                response = await ApiGetCall<List<DeliveredQuantityVarianceModel>>(apiPageUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetBuyerApprovalExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<CompanyApprovalExceptionModel> GetSupplierApprovalExceptions(string exceptionTypes, UserContext userContext)
        {
            var response = new CompanyApprovalExceptionModel();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetSupplierApprovalExceptions;
                var apiPageUrl = string.Format(apiUrl, userContext.CompanyId, exceptionTypes);
                response = await ApiGetCall<CompanyApprovalExceptionModel>(apiPageUrl);

                if (response != null && response.GeneratedExceptions != null && response.GeneratedExceptions.Any())
                {
                    UpdateExceptionsForExchangeData(response, exceptionTypes);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetSupplierApprovalExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<RaisedExceptionModel>> GetRaisedExceptions(string exceptionTypes, int companyId, bool isBuyerCompany)
        {
            var response = new List<RaisedExceptionModel>();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetRaisedExceptions;
                var apiPageUrl = string.Format(apiUrl, exceptionTypes, companyId, isBuyerCompany);
                var spResult = await ApiGetCall<List<RaisedExceptionModel>>(apiPageUrl);
                if (spResult != null && spResult.Any())
                {
                    foreach (var exception in spResult)
                    {
                        if (!string.IsNullOrEmpty(exception.ParameterJson))
                        {
                            if (exception.ExceptionTypeId == (int)ExceptionType.MissingDeliveries)
                            {
                                var missingDetail = JsonConvert.DeserializeObject<List<MissedDeliveryExceptionOtherDetails>>(exception.ParameterJson);
                                if (missingDetail != null)
                                {
                                    foreach (var item in missingDetail)
                                    {
                                        if (item.TankDetails != null)
                                        {
                                            foreach (var tank in item.TankDetails)
                                            {
                                                var newException = new RaisedExceptionModel
                                                {
                                                    ExceptionTypeId = exception.ExceptionTypeId,
                                                    Id = exception.Id,
                                                    TankDetail = new TankExceptionDetailModel
                                                    {
                                                        SiteId = tank.SiteId,
                                                        StorageId = tank.StorageId,
                                                        TankId = tank.TankId
                                                    }
                                                };
                                                response.Add(newException);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var tankDet = JsonConvert.DeserializeObject<TankExceptionDetailModel>(exception.ParameterJson);
                                if (tankDet != null)
                                {
                                    exception.TankDetail = tankDet;
                                    response.Add(exception);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetRaisedExceptions", ex.Message, ex);
            }
            return response;
        }


        private async Task<List<AutoApprovalExceptionModel>> GetAutoApprovalExceptions()
        {
            var response = new List<AutoApprovalExceptionModel>();
            try
            {
                var appDomain = new ApplicationDomain();
                var holidayList = appDomain.GetKeySettingValue(ApplicationConstants.PublicHolidayList, string.Empty);
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetAutoApprovalExceptions;
                var apiPageUrl = string.Format(apiUrl, holidayList, true);
                response = await ApiGetCall<List<AutoApprovalExceptionModel>>(apiPageUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetSupplierApprovalExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> AutoApproveExceptions()
        {
            var response = false;
            try
            {
                string approvedExceptionIds = string.Empty;
                var userContext = new UserContext() { Id = (int)SystemUser.System, Roles = new List<int>() { (int)UserRoles.SuperAdmin } };
                var autoApprovalExceptions = await GetAutoApprovalExceptions();
                if (autoApprovalExceptions != null)
                {
                    var invoiceDomain = new InvoiceDomain(this);
                    foreach (var item in autoApprovalExceptions)
                    {
                        if (item.ExceptionTypeId == (int)ExceptionType.DuplicateInvoice)
                        {
                            var result = await invoiceDomain.ApproveEddtAndCreateInvoice(userContext, item.ExceptionId, ExceptionResolution.ApproveDropTicket, item.DeliveredQuantity, (int)ExceptionStatus.AutoApproved);
                            if (result.StatusCode == Status.Success)
                                approvedExceptionIds += item.ExceptionId + ",";
                        }
                        else if (item.ExceptionTypeId == (int)ExceptionType.MissingDeliveries)
                        {
                            var result = await AutoApproveEddtAndCreateInvoiceMissingDelivery(item.ExceptionId, ExceptionResolution.AttachOrder, (int)ExceptionStatus.AutoApproved);
                            if (result.StatusCode == Status.Success)
                                approvedExceptionIds += item.ExceptionId + ",";
                        }
                        else
                        {
                            var result = await invoiceDomain.ApproveEddtAndCreateInvoice(userContext, item.ExceptionId, ExceptionResolution.ApproveDroppedQuantity, item.DeliveredQuantity, (int)ExceptionStatus.AutoApproved);
                            if (result.StatusCode == Status.Success)
                                approvedExceptionIds += item.ExceptionId + ",";
                        }
                    }
                    if (autoApprovalExceptions.Any())
                        LogManager.Logger.WriteException("ExceptionDomain", "AutoApproveExceptions", "Exceptions Auto Approved: " + approvedExceptionIds, new Exception());
                    else
                        LogManager.Logger.WriteException("ExceptionDomain", "AutoApproveExceptions", "There are no exceptions available to Auto Approve", new Exception());
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "AutoApproveExceptions", ex.Message, ex);
            }
            return response;
        }

        private async Task<T> ApiGetCall<T>(string url)
        {
            T response = default(T);
            if (string.IsNullOrWhiteSpace(_exceptionApiBaseUrl))
                throw new Exception("ApiGetCall: Exception service configuration is missing");

            using (IRestClient client = new RestClient())
            {
                
                HttpResponseMessage apiResponse = await client.GetAsync(url);
                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseString = await apiResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<T>(responseString);
                }
            }
            return response;
        }

        private async Task<T> ApiPostCall<T>(string url, object inputObject)
        {
            T response = default(T);
            if (string.IsNullOrWhiteSpace(_exceptionApiBaseUrl))
                throw new Exception("ApiPostCall: Exception service configuration is missing");

            using (IRestClient client = new RestClient())
            {
                var json = JsonConvert.SerializeObject(inputObject);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage apiResponse = await client.PostAsync(url, stringContent);
                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseString = await apiResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<T>(responseString);
                }
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetCustomersBySupplierOrCarrier(UserContext userContext)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetCustomersBySupplierOrCarrier(userContext.CompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetCustomers", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayItem>> GetLocationByCustomerId(int CustomerId, UserContext userContext, bool isRetailJob)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetLocationByCustomerId(userContext.CompanyId, CustomerId, isRetailJob);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetCustomers", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetOrdersByCustomerAndLocationId(int CustomerId, int locationId, UserContext userContext, int tfxProductId = 0)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetOrdersByCustomerAndLocationId(userContext.CompanyId, CustomerId, locationId, tfxProductId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetCustomers", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> LinkedUnAssignDDTToOrder(int exceptionId, int customerId, int locationId, int orderId, int exceptionDdtId)
        {
            var response = new StatusViewModel();
            try
            {
                var apiRequestModel = await GetApiLogByInvoiceId(exceptionDdtId);
                //Convert unassign ddt to invoice function here
                var invoiceTPDDomain = new InvoiceTPDDomain();

                response = await invoiceTPDDomain.ApproveUnassignedExceptionDdt(orderId, exceptionDdtId, apiRequestModel);

                if (response.StatusCode == Status.Success)
                {
                    await ApproveException(new List<int> { exceptionId }, ExceptionResolution.ApproveDropTicket, (int)ExceptionStatus.Resolved);
                }
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "LinkedUnAssignDDTToOrder", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetPDIExceptions(int companyId, bool IsDdt)
        {
            List<DropdownDisplayItem> responseList = null;
            try
            {
                if (Context.DataContext.QueueMessages.Any(t => t.Status == 0
                    && t.ProcessTypeId == (int)QueueProcessType.PDIAPIDeliveryDetails))
                {
                    return responseList;
                }
                DateTimeOffset createdDate = new DateTimeOffset(2022, 2, 28, 0, 0, 0, new TimeSpan(0, 0, 0));
                if (IsDdt)
                {
                    responseList = Context.DataContext.InvoiceXAdditionalDetails.Where(t1 => t1.Invoice != null && t1.Invoice.IsActive
                                && t1.Invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                && (t1.Invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || t1.Invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                                && string.IsNullOrEmpty(t1.PDIDeliveryOrderNo)
                                && !string.IsNullOrEmpty(t1.ExceptionMessage) && t1.Invoice.Order.AcceptedCompanyId == companyId 
                                && t1.Invoice.CreatedDate>createdDate).
                    Select(m => new DropdownDisplayItem
                    {
                        Id = m.Invoice.InvoiceHeaderId
                    }).Distinct().ToList();
                }
                else
                {
                    responseList = Context.DataContext.InvoiceXAdditionalDetails.Where(t1 => t1.Invoice != null && t1.Invoice.IsActive
                                && t1.Invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                && (t1.Invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && t1.Invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                                && string.IsNullOrEmpty(t1.PDIDeliveryOrderNo)
                                && !string.IsNullOrEmpty(t1.ExceptionMessage) && t1.Invoice.Order.AcceptedCompanyId == companyId
                                && t1.Invoice.CreatedDate>createdDate).
                Select(m => new DropdownDisplayItem
                {
                    Id = m.Invoice.InvoiceHeaderId
                }).Distinct().ToList();
                }

            }
            catch (Exception ex)
            {
                responseList = null;
                LogManager.Logger.WriteException("ExceptionDomain", "GetPDIExceptions", ex.Message, ex);
            }

            return responseList;
        }

        private async Task<TPDInvoiceViewModel> GetApiLogByInvoiceId(int invoiceId)
        {
            var response = new TPDInvoiceViewModel();
            try
            {
                var externalRefid = Context.DataContext.InvoiceXAdditionalDetails.Where(w => w.InvoiceId == invoiceId).Select(s => s.ExternalRefID).FirstOrDefault();
                if (externalRefid != null)
                {
                    //string apiRequest = Context.DataContext.ApiLogs.Where(w => w.ExternalRefID == externalRefid.ToString()).OrderByDescending(o => o.CreatedDate).Select(s => s.Request).FirstOrDefault();
                    var spDomain = new StoredProcedureDomain();
                    var apiLogs = await spDomain.GetApiLogsForCompany(0, null, null, 0, externalRefid);
                    var apiRequest = apiLogs.AnyAndNotNull() ? apiLogs.OrderByDescending(t => t.CreatedDate).Select(t => t.Request).FirstOrDefault():string.Empty;
                    var json_serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var req = (IDictionary<string, object>)json_serializer.DeserializeObject(apiRequest);
                    var modelSer = req["viewModel"];
                    var ser = Newtonsoft.Json.JsonConvert.SerializeObject(modelSer);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<TPDInvoiceViewModel>(ser);
                }

                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetApiLogByGeneratedExceptionId", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> DiscardUnAssignDDT(int exceptionId, int exceptionDdtId)
        {
            var response = new StatusViewModel();
            try
            {   
                var exceptionDdt = await Context.DataContext.Invoices.Where(t => t.Id == exceptionDdtId).Select(sa => sa).FirstOrDefaultAsync();
                if (exceptionDdt != null)
                {
                    var status = exceptionDdt.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive);
                    status.StatusId = (int)InvoiceStatus.Rejected;
                    Context.DataContext.Entry(exceptionDdt).State = EntityState.Modified;
                    await Context.CommitAsync();
                    var result = await ApproveException(new List<int> { exceptionId }, ExceptionResolution.DiscardDropTicket, (int)ExceptionStatus.Resolved);
                    if (result)
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.messageDiscardExceptionInvoice;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        //response.StatusMessage = Resource.faile;
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "DiscardUnAssignDDT", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AutoDiscardUnAssignDDT()
        {
            var response = new StatusViewModel();
            try
            {
                response = await AutoDiscardApiExceptionUnassignedDDT(ExceptionType.InvoiceApiException, ExceptionStatus.Raised);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "AutoDiscardUnAssignDDT", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AutoDiscardUnknownDeliveries()
        {
            var response = new StatusViewModel();
            try
            {
                response = await AutoDiscardUnknownDeliveries(ExceptionType.UnknownDeliveries, ExceptionStatus.Raised);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "AutoDiscardUnknownDeliveries", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AutoDiscardApiExceptionUnassignedDDT(ExceptionType exceptionType, ExceptionStatus exceptionStatus)
        {
            var response = new StatusViewModel();
            try
            {
                var generatedExceptionIds = await GetExceptionIdsForAutoRejection((int)exceptionType, (int)exceptionStatus);
                if (generatedExceptionIds != null && generatedExceptionIds.Count > 0)
                {
                    var invoiceIdList = Context.DataContext.InvoiceExceptions.Where(w => generatedExceptionIds.Contains(w.GeneratedExceptionId)).Select(s => s.InvoiceId).ToList();
                    if (invoiceIdList != null && invoiceIdList.Count > 0)
                    {   
                        var exceptionDdtList = await Context.DataContext.Invoices.Where(t => invoiceIdList.Contains(t.Id)).Select(sa => sa).ToListAsync();
                        if (exceptionDdtList != null && exceptionDdtList.Count > 0)
                        {
                            foreach (var exceptionDdt in exceptionDdtList)
                            {
                                if (exceptionDdt != null)
                                {
                                    var status = exceptionDdt.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive);
                                    status.StatusId = (int)InvoiceStatus.Rejected;
                                    Context.DataContext.Entry(exceptionDdt).State = EntityState.Modified;
                                    await Context.CommitAsync();
                                }
                            }
                            var result = await UpdateExceptionIdForAutoReject(generatedExceptionIds);
                            if (result)
                            {
                                response.StatusCode = Status.Success;
                                // response.StatusMessage = Resource.messageDiscardExceptionInvoice;
                            }
                            else
                            {
                                response.StatusCode = Status.Failed;
                            }
                            return response;
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "AutoDiscardApiExceptionUnassignedDDT", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AutoDiscardUnknownDeliveries(ExceptionType exceptionType, ExceptionStatus exceptionStatus)
        {
            var response = new StatusViewModel();
            try
            {
                var generatedExceptionIds = await GetExceptionIdsForAutoRejection((int)exceptionType, (int)exceptionStatus);
                if (generatedExceptionIds != null && generatedExceptionIds.Count > 0)
                {
                    var result = await UpdateExceptionIdForAutoReject(generatedExceptionIds);
                    if (result)
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.messageDiscardExceptionInvoice;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "AutoDiscardUnknownDeliveries", ex.Message, ex);
            }
            return response;
        }

        private async Task<bool> UpdateExceptionIdForAutoReject(List<int> generatedExceptionList)
        {
            var response = false;
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlUpdateExceptionIdForAutoReject;
                response = await ApiPostCall<bool>(apiUrl, generatedExceptionList);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "UpdateExceptionIdForAutoReject", ex.Message, ex);
            }
            return response;
        }


        private async Task<List<int>> GetExceptionIdsForAutoRejection(int exceptionTypeId, int statusId)
        {
            var response = new List<int>();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetExceptionIdsforAutoRejection;
                var apiPageUrl = string.Format(apiUrl, exceptionTypeId, statusId);
                response = await ApiGetCall<List<int>>(apiPageUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetExceptionIdsForAutoRejection", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<EnabledExceptionViewModel>> GetCompaniesForEnabledException(ExceptionType exceptionType)
        {
            var response = new List<EnabledExceptionViewModel>();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetCompaniesForEnabledException;
                var apiPageUrl = string.Format(apiUrl, (int)exceptionType);
                response = await ApiGetCall<List<EnabledExceptionViewModel>>(apiPageUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetCompaniesForEnabledException", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> ProcessUnknownDeliveryExceptionManagement()
        {
            bool response = false;
            try
            {
                var jobDomain = new JobDomain(this);
                var masterDomain = new MasterDomain(this);
                var unknownDeliveryModel = new UnknownDeliveryTankRequestModel();

                // get the suppliers/carriers who has enabled the Unknown Deliveries Exceptions
                var companiesWhoEnabledException = await GetCompaniesForEnabledException(ExceptionType.UnknownDeliveries);
                if (companiesWhoEnabledException != null && companiesWhoEnabledException.Any())
                {
                    // get the buyers of each supplier
                    foreach (var enabledException in companiesWhoEnabledException)
                    {
                        var companyType = await Context.DataContext.Companies.Where(t => t.Id == enabledException.OwnerCompanyId).Select(t1 => (CompanyType)t1.CompanyTypeId).FirstOrDefaultAsync();
                        var buyerIds = await masterDomain.GetCustomersWithSupplierTolerance(enabledException.OwnerCompanyId, companyType, enabledException.Threshold);

                        // get tank details for buyer jobs                    
                        if (buyerIds != null && buyerIds.Any())
                        {
                            // get the tank details by buyers
                            unknownDeliveryModel.JobTanks = await jobDomain.GetTankDetailsByCustomer(enabledException.OwnerCompanyId, companyType, buyerIds);

                            if (unknownDeliveryModel.JobTanks != null && unknownDeliveryModel.JobTanks.Any())
                            {
                                // get the exception details to check already existing exception
                                var exceptionType = ((int)ExceptionType.UnknownDeliveries).ToString();
                                var userContext = new UserContext() { CompanyId = enabledException.OwnerCompanyId };
                                var exceptionResponse = await GetGeneratedExceptionsForApproval(exceptionType, userContext);

                                // get the distinct SiteId, TankId and StorageId
                                var uniqueTanks = unknownDeliveryModel.JobTanks.Select(t => new { t.SiteId, t.TankId, t.StorageId }).Distinct().ToList();
                                foreach (var tank in uniqueTanks)
                                {
                                    var tankVolumes = unknownDeliveryModel.JobTanks.Where(t => t.SiteId == tank.SiteId && t.TankId == tank.TankId && t.StorageId == tank.StorageId)
                                                                                    .OrderByDescending(t => t.CaptureTime).Take(2).ToList();
                                    if (tankVolumes.Count == 2)
                                    {
                                        var latestTankVolume = tankVolumes[0];
                                        var prevTankVolume = tankVolumes[1];
                                        decimal prevTankUllage = 0;
                                        decimal latestTankUllage = 0;
                                        decimal.TryParse(prevTankVolume.Ullage, out prevTankUllage);
                                        decimal.TryParse(latestTankVolume.Ullage, out latestTankUllage);
                                        var ullageDIfference = prevTankUllage - latestTankUllage;
                                        decimal tankCapacity = latestTankVolume.TankCapacity ?? 0;
                                        decimal minimumUllageDifference = 100;

                                        var isExceptionAlreadyExist = false;
                                        if (exceptionResponse != null && exceptionResponse.DeliveryMismatchExceptions != null && exceptionResponse.DeliveryMismatchExceptions.Any())
                                        {
                                            //var existingExps = exceptionResponse.DeliveryMismatchExceptions.Where(t => t.JobId == latestTankVolume.JobId && t.SiteId == tank.SiteId && t.TankId == tank.TankId && t.StorageId == tank.StorageId && Convert.ToDecimal(t.Ullage) == latestTankUllage && t.StatusId == (int)ExceptionStatus.Raised).ToList();
                                            isExceptionAlreadyExist = exceptionResponse.DeliveryMismatchExceptions.Any(t => t.JobId == latestTankVolume.JobId && t.SiteId == tank.SiteId && t.TankId == tank.TankId && t.StorageId == tank.StorageId && Convert.ToDecimal(t.Ullage) == latestTankUllage);
                                        }

                                        if (!isExceptionAlreadyExist)
                                        {
                                            if (tankCapacity > 0)
                                            {
                                                decimal minimumUllagePercOfTankCapacity = 10;
                                                var minimumUllageDiffOfTankCapacityAppSetting = await Context.DataContext.MstAppSettings.Where(m => m.Key == ApplicationConstants.KeyAppSettingDefaultTankCapacityPercentage).Select(t => t.Value).FirstOrDefaultAsync();
                                                if (minimumUllageDiffOfTankCapacityAppSetting != null)
                                                    decimal.TryParse(minimumUllageDiffOfTankCapacityAppSetting, out minimumUllagePercOfTankCapacity);
                                                minimumUllageDifference = tankCapacity * minimumUllagePercOfTankCapacity / 100;
                                            }

                                            // if latest ullage is less than previous ullage, then drop done into tank else not 
                                            if (latestTankUllage < prevTankUllage && (ullageDIfference >= minimumUllageDifference || ullageDIfference > 100))
                                            {
                                                var dateTimeToCheck = DateTimeOffset.Now.AddHours(-1);
                                                var supplierCompany = await Context.DataContext.Companies.Where(t => t.Id == enabledException.OwnerCompanyId).Select(t1 => t1.Name).FirstOrDefaultAsync();

                                                // RAISE THE EXCEPTION
                                                var tankInfoForException = new
                                                {
                                                    JobId = latestTankVolume.JobId,
                                                    JobAddress = latestTankVolume.JobAddress,
                                                    ProductName = latestTankVolume.ProductName,
                                                    SiteId = tank.SiteId,
                                                    TankId = tank.TankId,
                                                    StorageId = tank.StorageId,
                                                    CaptureTime = latestTankVolume.CaptureTime,
                                                    Ullage = latestTankUllage,
                                                    PrevUllage = prevTankUllage,
                                                    ReasonOfFailure = Resource.messageUnknownDeliveryReasonOfFailure,
                                                    SourceFileId = latestTankVolume.SourceFileId,
                                                };

                                                var exceptionModel = new InvoiceExceptionRequestModel();

                                                exceptionModel.JobName = latestTankVolume.JobName;
                                                exceptionModel.DropDate = latestTankVolume.CaptureTime;
                                                exceptionModel.SupplierCompanyId = enabledException.OwnerCompanyId;
                                                exceptionModel.SupplierCompanyName = supplierCompany;
                                                exceptionModel.BuyerCompanyId = latestTankVolume.CustomerId;
                                                exceptionModel.BuyerCompanyName = latestTankVolume.Customer;
                                                exceptionModel.ExceptionTypeId = (int)ExceptionType.UnknownDeliveries;
                                                exceptionModel.Ullage = ullageDIfference;
                                                exceptionModel.UOM = latestTankVolume.QuantityUoM.ToString();

                                                var otherDetails = tankInfoForException;
                                                exceptionModel.ParameterJson = JsonConvert.SerializeObject(otherDetails);

                                                var invoiceCreated = await Context.DataContext.Invoices.Where(t => t.CreatedDate > dateTimeToCheck &&
                                                                            t.Order.BuyerCompanyId == latestTankVolume.CustomerId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active &&
                                                                            t.Order.FuelRequest.Job.JobXAssets.Any(x1 => x1.Asset.Type == (int)AssetType.Tank && x1.Job.Id == latestTankVolume.JobId &&
                                                                                                                            x1.Job.DisplayJobID != null && x1.Job.DisplayJobID.Trim() != "" && x1.Job.DisplayJobID == tank.SiteId &&
                                                                                                                            x1.Asset.AssetAdditionalDetail.VehicleId != null && x1.Asset.AssetAdditionalDetail.VehicleId.Trim() != "" && x1.Asset.AssetAdditionalDetail.VehicleId == tank.TankId &&
                                                                                                                            x1.Asset.AssetAdditionalDetail.Vendor != null && x1.Asset.AssetAdditionalDetail.Vendor.Trim() != "" && x1.Asset.AssetAdditionalDetail.Vendor == tank.StorageId
                                                                                                                    ))
                                                                            .Select(t1 => new { InvoiceId = t1.Id, t1.PoNumber, t1.DroppedGallons, t1.DropEndDate, t1.DisplayInvoiceNumber, t1.DriverId })
                                                                            .FirstOrDefaultAsync();
                                                if (invoiceCreated == null)
                                                {
                                                    await RaiseDeliveryMismatchExceptions(exceptionModel);
                                                    response = true;
                                                }
                                                else
                                                {
                                                    decimal tolerance = 75;
                                                    var toleranceAppSetting = await Context.DataContext.MstAppSettings.Where(m => m.Key == ApplicationConstants.KeyAppSettingDefaultTolerancePercentage).Select(t => t.Value).FirstOrDefaultAsync();
                                                    if (toleranceAppSetting != null)
                                                        decimal.TryParse(toleranceAppSetting, out tolerance);
                                                    var ullageWithTolerance = (invoiceCreated.DroppedGallons * tolerance) / 100;
                                                    if (ullageDIfference < ullageWithTolerance || ullageDIfference > invoiceCreated.DroppedGallons)
                                                    {
                                                        exceptionModel.DriverId = invoiceCreated.DriverId;
                                                        exceptionModel.DroppedQuantity = invoiceCreated.DroppedGallons;
                                                        exceptionModel.InvoiceId = invoiceCreated.InvoiceId;
                                                        exceptionModel.PoNumber = invoiceCreated.PoNumber;
                                                        exceptionModel.InvoiceNumber = invoiceCreated.DisplayInvoiceNumber;
                                                        exceptionModel.DropDate = invoiceCreated.DropEndDate;

                                                        await RaiseDeliveryMismatchExceptions(exceptionModel);
                                                        response = true;
                                                    }
                                                    //LogManager.Logger.WriteInfo("ExchangeDomain", "ProcessUnknownDeliveryExceptionManagement", $"Invoice {invoiceCreated.DisplayInvoiceNumber} found for SiteId => {tank.SiteId} TankId => {tank.TankId} StorageId => {tank.StorageId}");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LogManager.Logger.WriteInfo("ExchangeDomain", "ProcessUnknownDeliveryExceptionManagement", $"Tank ullage details not found to compare for SiteId => {tank.SiteId} TankId => {tank.TankId} StorageId => {tank.StorageId}");
                                    }

                                }
                            }
                            else
                            {
                                LogManager.Logger.WriteInfo("ExchangeDomain", "ProcessUnknownDeliveryExceptionManagement", "Tank details not found for supplier id " + enabledException.OwnerCompanyId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExchangeDomain", "ProcessUnknownDeliveryExceptionManagement", "Exception Details : ", ex);
            }

            return response;
        }

        public async Task<InvoiceExceptionResponseModel> RaiseDeliveryMismatchExceptions(InvoiceExceptionRequestModel model)
        {
            var response = new InvoiceExceptionResponseModel();
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlRaiseDeliveryMismatchExceptions;
                response = await ApiPostCall<InvoiceExceptionResponseModel>(apiUrl, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "RaiseDeliveryMismatchExceptions", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> ProcessMissingDeliveryExceptionManagement()
        {
            bool response = false;
            var spDomain = new StoredProcedureDomain(this);

            try
            {
                decimal tolerance = 75;
                var toleranceAppSetting = await Context.DataContext.MstAppSettings.Where(m => m.Key == ApplicationConstants.KeyAppSettingDefaultTolerancePercentage).Select(t => t.Value).FirstOrDefaultAsync();
                if (toleranceAppSetting != null)
                    decimal.TryParse(toleranceAppSetting, out tolerance);

                //new headlist
                var waitingForInventoryVerificationDdtList =
                    Context.DataContext.Invoices.Where(t1 => t1.WaitingFor == (int)WaitingAction.InventoryVerification
                                            && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                            && t1.IsActive)
                                            .Select(t2 => new
                                            {
                                                DdtId = t2.Id,
                                                t2.Order.FuelRequest.Job.TimeZoneName,
                                                t2.InvoiceHeaderId,
                                                OrderId = t2.Order.Id,
                                                PoNumber = t2.PoNumber,
                                                DisplayInvoiceNumber = t2.DisplayInvoiceNumber,
                                                AcceptedCompanyId = t2.Order.AcceptedCompanyId,
                                                SupplierCompany = t2.Order.Company.Name,
                                                BuyerCompanyId = t2.Order.BuyerCompanyId,
                                                BuyerCompany = t2.Order.BuyerCompany.Name,
                                                JobId = t2.Order.FuelRequest.JobId,
                                                DropDate = t2.DropEndDate,
                                                JobName = t2.Order.FuelRequest.Job.Name,
                                                Address = t2.Order.FuelRequest.Job.Address,
                                                City = t2.Order.FuelRequest.Job.City,
                                                StateCode = t2.Order.FuelRequest.Job.MstState.Code,
                                                ZipCode = t2.Order.FuelRequest.Job.ZipCode,
                                                LocationType = t2.Order.FuelRequest.Job.LocationType,
                                                ProductName = t2.Order.FuelRequest.MstProduct.MstTFXProduct != null ? t2.Order.FuelRequest.MstProduct.MstTFXProduct.Name : t2.Order.FuelRequest.MstProduct.Name,
                                                CarrierName = t2.InvoiceXBolDetails.Select(t3 => t3.InvoiceFtlDetail).FirstOrDefault() != null ? t2.InvoiceXBolDetails.FirstOrDefault().InvoiceFtlDetail.Carrier : string.Empty,
                                                DriverId = t2.DriverId,
                                                DriverFirstName = t2.DriverId.HasValue ? t2.Driver.FirstName : string.Empty,
                                                DriverLastName = t2.DriverId.HasValue ? t2.Driver.LastName : string.Empty,
                                                UserId = t2.CreatedBy,
                                                CreateDate = t2.CreatedDate,
                                                TfxProductId = t2.Order.FuelRequest.MstProduct.MstTFXProduct != null ? t2.Order.FuelRequest.MstProduct.MstTFXProduct.Id : t2.Order.FuelRequest.MstProduct.Id,
                                                DroppedGallons = t2.DroppedGallons
                                            }).ToList().GroupBy(t => t.InvoiceHeaderId).OrderBy(t => t.Select(t1 => t1.InvoiceHeaderId).FirstOrDefault()).ToList();

                List<int> companyIds = new List<int>();
                foreach (var groupItem in waitingForInventoryVerificationDdtList)
                {
                    var ddtListFromHeader = groupItem.ToList();
                    foreach (var ddtItem in ddtListFromHeader)
                        companyIds.Add(ddtItem.AcceptedCompanyId);
                }

                companyIds = companyIds.Distinct().ToList();
                var companiesDelayInvoiceCreationTime = await GetDelayInvoiceCreationTimeByCompany(companyIds);

                //iterate header list and process each ddt
                foreach (var groupItem in waitingForInventoryVerificationDdtList)
                {
                    var ddtListFromHeader = groupItem.ToList();
                    var ddtlist = new List<int>();
                    var ddtDetailsForExceptionList = new List<MissedDeliveryExceptionOtherDetails>();
                    var exceptionModel = new InvoiceExceptionRequestModel();
                    var raiseException = false;
                    var addExceptionForDDT = false;
                    var isDateTimeValid = false;

                    foreach (var ddtItem in ddtListFromHeader)
                    {
                        var delayInvoiceCreationTime = companiesDelayInvoiceCreationTime.FirstOrDefault(t => t.Id == ddtItem.AcceptedCompanyId);
                        var minutesToAdd = delayInvoiceCreationTime != null ? delayInvoiceCreationTime.CodeId : 60;
                        var invoiceCreationDelayDateTime = ddtItem.CreateDate.AddMinutes(minutesToAdd);
                        var jobCurrentDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(ddtItem.TimeZoneName);

                        if (invoiceCreationDelayDateTime <= jobCurrentDateTime)
                        {
                            isDateTimeValid = true;
                            //addExceptionForDDT = false;
                            var tankDetailsForExceptionList = new List<MissedDeliveryTankDetails>();
                            ddtlist.Add(ddtItem.DdtId);

                            var tankDetails = await spDomain.GetTankDetailsByInvoice(ddtItem.DdtId);

                            if (tankDetails != null && tankDetails.Any())
                            {
                                var tankInventoryResonse = VerifyTankInventory(tankDetails, tolerance, out addExceptionForDDT);
                                tankDetailsForExceptionList.AddRange(tankInventoryResonse);

                                if (addExceptionForDDT)
                                {
                                    raiseException = true;
                                    //Start of Adding DDT and Order Details
                                    var ddtDetailsForException = new MissedDeliveryExceptionOtherDetails
                                    {
                                        OrderId = ddtItem.OrderId,
                                        PoNumber = ddtItem.PoNumber,
                                        DisplayInvoiceNumber = ddtItem.DisplayInvoiceNumber,
                                        InvoiceId = ddtItem.DdtId,
                                        TankDetails = tankDetailsForExceptionList,
                                        JobAddress = ddtItem.LocationType != JobLocationTypes.Various ? ddtItem.Address + ',' + ddtItem.City + ',' + ddtItem.StateCode + ',' + ddtItem.ZipCode : ddtItem.StateCode,
                                        ProductName = ddtItem.ProductName,
                                        TfxProductId = ddtItem.TfxProductId
                                    };
                                    ddtDetailsForExceptionList.Add(ddtDetailsForException);
                                    //End of Adding DDT and Order Details
                                }
                            }
                            else
                            {
                                LogManager.Logger.WriteInfo("ExceptionDomain", "ProcessMissingDeliveryExceptionManagement", "Tank details not found for supplier id " + ddtItem.AcceptedCompanyId);
                            }

                            exceptionModel.JobName = ddtItem.JobName;
                            exceptionModel.DropDate = ddtItem.DropDate;
                            exceptionModel.ExceptionTypeId = (int)ExceptionType.MissingDeliveries;
                            exceptionModel.CarrierName = ddtItem.CarrierName;
                            exceptionModel.SupplierCompanyId = ddtItem.AcceptedCompanyId;
                            exceptionModel.SupplierCompanyName = ddtItem.SupplierCompany;
                            exceptionModel.BuyerCompanyId = ddtItem.BuyerCompanyId;
                            exceptionModel.BuyerCompanyName = ddtItem.BuyerCompany;
                            exceptionModel.UserId = ddtItem.UserId;
                            exceptionModel.DriverId = ddtItem.DriverId;
                            if (ddtItem.DriverId.HasValue)
                                exceptionModel.DriverName = ddtItem.DriverFirstName + " " + ddtItem.DriverLastName;
                        }
                    }

                    if (isDateTimeValid)
                    {
                        if (raiseException)
                        {
                            exceptionModel.PoNumber = string.Join(",", ddtDetailsForExceptionList.Select(t => t.PoNumber).ToList());
                            exceptionModel.InvoiceNumber = string.Join(",", ddtDetailsForExceptionList.Select(t => t.DisplayInvoiceNumber).ToList());
                            exceptionModel.ParameterJson = JsonConvert.SerializeObject(ddtDetailsForExceptionList);
                            exceptionModel.IsInventoryVerified = true;

                            // RAISE THE EXCEPTION
                            var exceptionResult = await RaiseDeliveryMismatchExceptions(exceptionModel);
                            if (exceptionResult != null && exceptionResult.Exceptions != null && exceptionResult.IsExceptionsRaised && exceptionResult.Exceptions.Any())
                            {
                                await RaiseDDTForExceptionApproval(ddtlist, exceptionModel, exceptionResult);
                            }
                        }
                        else
                        {
                            await ContextFactory.Current.GetDomain<InvoiceDomain>().ConvertDdtToInvoiceMissingDelivery(ddtlist.First(), exceptionModel.UserId.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "ProcessMissingDeliveryExceptionManagement", "Exception Details : ", ex);
            }

            return response;
        }

        private List<MissedDeliveryTankDetails> VerifyTankInventory(List<JobTankDetailsViewModel> tankDetails, decimal tolerance, out bool addExceptionForDDT)
        {
            var tankDetailsForExceptionList = new List<MissedDeliveryTankDetails>();
            addExceptionForDDT = false;
            // get the distinct SiteId, TankId and StorageId
            var uniqueTanks = tankDetails.Select(t => new { t.SiteId, t.TankId, t.StorageId, t.DroppedGallons }).Distinct().ToList();
            foreach (var tank in uniqueTanks)
            {
                var tankVolumes = tankDetails.Where(t => t.SiteId == tank.SiteId && t.TankId == tank.TankId && t.StorageId == tank.StorageId)
                                                               .OrderByDescending(t => t.CaptureTime).Take(2).ToList();
                if (tankVolumes.Count == 2)
                {
                    var latestTankVolume = tankVolumes[0];
                    var prevTankVolume = tankVolumes[1];
                    decimal prevTankUllage = 0;
                    decimal latestTankUllage = 0;
                    decimal.TryParse(prevTankVolume.Ullage, out prevTankUllage);
                    decimal.TryParse(latestTankVolume.Ullage, out latestTankUllage);
                    var ullageDIfference = prevTankUllage - latestTankUllage;

                    var droppedQtyWithTolerance = (tank.DroppedGallons * tolerance) / 100;
                    if (latestTankUllage > prevTankUllage || ullageDIfference < droppedQtyWithTolerance)
                    {
                        addExceptionForDDT = true;
                        //Start of Adding Tank Details
                        var missingTankDetails = new MissedDeliveryTankDetails
                        {
                            TankName = latestTankVolume.TankName,
                            TankCapacity = latestTankVolume.TankCapacity,
                            SiteId = tank.SiteId,
                            TankId = tank.TankId,
                            StorageId = tank.StorageId,
                            CaptureTime = latestTankVolume.CaptureTime,
                            Ullage = latestTankUllage,
                            PrevUllage = prevTankUllage,
                        };
                        tankDetailsForExceptionList.Add(missingTankDetails);
                        //End of Adding Tank Details
                    }
                }
                else
                {
                    LogManager.Logger.WriteInfo("ExceptionDomain", "VerifyTankInventory", $"Tank ullage details not found to compare for SiteId => {tank.SiteId} TankId => {tank.TankId} StorageId => {tank.StorageId}");
                }
            }
            return tankDetailsForExceptionList;
        }

        private async Task<List<DropdownDisplayExtendedId>> GetDelayInvoiceCreationTimeByCompany(List<int> companyIds)
        {
            List<DropdownDisplayExtendedId> result = null;
            try
            {
                var apiUrl = _exceptionApiBaseUrl + ApplicationConstants.UrlGetDelayInvoiceCreationTimeByCompany;
                result = await ApiPostCall<List<DropdownDisplayExtendedId>>(apiUrl, companyIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetDelayInvoiceCreationTimeByCompany", ex.Message, ex);
            }
            return result;
        }

        private async Task<bool> RaiseDDTForExceptionApproval(List<int> ddtlist, InvoiceExceptionRequestModel exceptionModel, InvoiceExceptionResponseModel exceptionResult)
        {
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var id in ddtlist)
                    {
                        var invoice = await Context.DataContext.Invoices.FirstOrDefaultAsync(t => t.Id == id);
                        if (invoice != null)
                        {
                            invoice.WaitingFor = (int)WaitingAction.ExceptionApproval;
                            invoice.UpdatedDate = DateTimeOffset.Now;
                            Context.DataContext.Entry(invoice).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }
                    }

                    if (exceptionModel.ParameterJson != null)
                    {
                        var otherDetail = JsonConvert.DeserializeObject<List<MissedDeliveryExceptionOtherDetails>>(exceptionModel.ParameterJson);
                        var invoices = otherDetail.Select(t => t.InvoiceId).ToList();
                        foreach (var invoice in invoices)
                        {
                            var exception = exceptionResult.Exceptions.FirstOrDefault();
                            if (exception != null)
                            {
                                var entity = new InvoiceException();

                                entity.InvoiceId = invoice;
                                entity.ExceptionTypeId = exception.ExceptionTypeId;
                                entity.GeneratedExceptionId = exception.ExceptionId;
                                entity.RaisedOn = exception.RaisedOn;
                                entity.StatusId = exception.StatusId;
                                entity.IsActive = true;
                                entity.CreatedDate = DateTimeOffset.Now;
                                entity.UpdatedDate = DateTimeOffset.Now;

                                Context.DataContext.InvoiceExceptions.Add(entity);
                                await Context.CommitAsync();
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("ExceptionDomain", "RaiseDDTForExceptionApproval", ex.Message, ex);
                }
            }
            return true;
        }

        public async Task<StatusViewModel> ApproveEddtAndCreateInvoiceMissingDelivery(List<AssignOrderMissingDeliveryModel> assignOrderMissingDelivery, int exceptionId)
        {
            var response = new StatusViewModel();
            try
            {
                int userId = 0;
                int invoiceId = 0;
                foreach (var item in assignOrderMissingDelivery)
                {
                    var invoice = await Context.DataContext.Invoices.FirstOrDefaultAsync(t => t.Id == item.InvoiceId);
                    if (invoice != null)
                    {
                        invoice.OrderId = item.NewOrderId;
                        Context.DataContext.Entry(invoice).State = EntityState.Modified;
                        await Context.CommitAsync();

                        userId = invoice.CreatedBy;
                        invoiceId = invoice.Id;
                    }

                    List<int> orderIds = new List<int>();
                    orderIds.Add(item.OldOrderId);
                    orderIds.Add(item.NewOrderId);
                    var orderDetails = await Context.DataContext.Orders.Where(t => orderIds.Contains(t.Id)).Select(t => new { t.Id, t.FuelRequest.JobId }).ToListAsync();
                    if (orderDetails != null)
                    {
                        var oldOrderJob = orderDetails.Where(t => t.Id == item.OldOrderId).Select(t => t.JobId).FirstOrDefault();
                        var newOrderJob = orderDetails.Where(t => t.Id == item.NewOrderId).Select(t => t.JobId).FirstOrDefault();
                        if (oldOrderJob != newOrderJob)
                            item.IsReassignDifferentJob = true;
                    }
                }

                response = await ContextFactory.Current.GetDomain<InvoiceDomain>().ConvertDdtToInvoiceMissingDelivery(invoiceId, userId, assignOrderMissingDelivery);

                if (response.StatusCode == Status.Success)
                {
                    var apiApprovalResult = await ApproveException(new List<int> { exceptionId }, ExceptionResolution.AttachOrder, (int)ExceptionStatus.Resolved);
                    if (apiApprovalResult)
                    {
                        response.StatusMessage = Resource.successMessageInvoiceCreatedFromEddt;
                        await UpdateExceptionStatus(exceptionId, (int)ExceptionStatus.Resolved);
                    }
                    else
                    {
                        response.StatusMessage = Resource.FaileExceptionApproveMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "ApproveEddtAndCreateInvoiceMissingDelivery", ex.Message, ex);
            }
            return response;
        }

        protected async Task UpdateExceptionStatus(int generatedExceptionId, int statusId)
        {
            try
            {
                var exceptionDetails = await Context.DataContext.InvoiceExceptions.Where(t => t.GeneratedExceptionId == generatedExceptionId).ToListAsync();
                if (exceptionDetails != null && exceptionDetails.Any())
                {
                    foreach (var exceptionDetail in exceptionDetails)
                    {
                        exceptionDetail.StatusId = statusId;
                        exceptionDetail.IsActive = false;
                        exceptionDetail.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(exceptionDetail).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "UpdateExceptionStatus", ex.Message, ex);
            }
        }

        private async Task<StatusViewModel> AutoApproveEddtAndCreateInvoiceMissingDelivery(int exceptionId, ExceptionResolution resolutionType, int statusId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var eDdt = await Context.DataContext.InvoiceExceptions.Where(t => t.GeneratedExceptionId == exceptionId && t.IsActive)
                                       .Select(t => new
                                       {
                                           InvoiceId = t.Invoice.Id,
                                           UserId = t.Invoice.CreatedBy,
                                           CompanyId = t.Invoice.Order.AcceptedCompanyId
                                       }).FirstOrDefaultAsync();

                if (eDdt == null)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessageNotFound;
                    return response;
                }
                else
                {
                    UserContext userContext = new UserContext() { Id = eDdt.UserId, CompanyId = eDdt.CompanyId };
                    response = await ContextFactory.Current.GetDomain<InvoiceDomain>().ConvertDdtToInvoiceMissingDelivery(eDdt.InvoiceId, userContext.Id);
                    if (response.StatusCode == Status.Success)
                    {
                        var apiApprovalResult = await ApproveException(new List<int> { exceptionId }, resolutionType, statusId);
                        if (apiApprovalResult)
                        {
                            await UpdateExceptionStatus(exceptionId, statusId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "AutoApproveEddtAndCreateInvoiceMissingDelivery", ex.Message, ex);
            }
            return response;
        }
        public ManageExceptionViewModel IsLiftFileValidationEnabled(UserContext userContext)
        {
            var response = new ManageExceptionViewModel();
            try
            {
                response.IsLiftFileValidationEnabled = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == userContext.CompanyId && t.IsActive).Select(t => t.IsLiftFileValidationEnabled).FirstOrDefault();
                response.CompanyId = userContext.CompanyId;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "IsLiftFileValidationEnabled", ex.Message, ex);
            }
            return response;
        }

        //usp_GetSupplierNoDataExceptionDdts
        public async Task<List<NoDataExceptionDdtGridViewModel>> GetSupplierNoDataExceptionDdts(int companyId, NoDataExceptionDataTableViewModel requestModel)
        {
            List<NoDataExceptionDdtGridViewModel> response = new List<NoDataExceptionDdtGridViewModel>();
            try
            {
                if (requestModel == null)
                {
                    requestModel = new NoDataExceptionDataTableViewModel();
                }
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetSupplierNoDataExceptionDdts(companyId, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetSupplierNoDataExceptionDdts", ex.Message, ex);
            }
            return response;
        }

        public async Task<NoDataExceptionPrePostViewModel> GetNoDataExceptionDDtsForDipData(int InvoiceHeaderId, int CompanyId)
        {
            var response = new NoDataExceptionPrePostViewModel();
            try
            {
                if (InvoiceHeaderId > 0)
                {
                    var InvoiceTypeIds = new List<int> { (int)InvoiceType.DigitalDropTicketManual, (int)InvoiceType.DigitalDropTicketMobileApp };
                    var waitingForDipDataddts = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == InvoiceHeaderId
                                                         && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && InvoiceTypeIds.Contains(t.InvoiceTypeId)
                                                         && t.WaitingFor == (int)WaitingAction.PrePostDipData)
                                                         .Select(t => new
                                                         {
                                                             DDTId = t.Id,
                                                             InvoiceHeaderId = t.InvoiceHeaderId,
                                                             AssetDrops = t.AssetDrops,
                                                             PoNumber = t.PoNumber,
                                                             JobDetails = t.Order.FuelRequest.Job,
                                                             OrderId = t.OrderId,
                                                             DropTicketNo = t.DisplayInvoiceNumber,
                                                             UOM = t.UoM
                                                         }).
                                                         ToListAsync();

                    if (waitingForDipDataddts != null && waitingForDipDataddts.Any())
                    {
                        response.InvoiceHeaderId = InvoiceHeaderId;
                        response.companyId = CompanyId;
                        var listAssetDropDetails = new List<AssetDetailsForApprovalViewModel>();
                        List<int> assetIds = new List<int>();
                        foreach (var ddt in waitingForDipDataddts)
                        {
                            //asset details
                            if (ddt.AssetDrops != null && ddt.AssetDrops.Any() && ddt.JobDetails != null)
                            {
                                foreach (var assetdrop in ddt.AssetDrops)
                                {
                                    var assetDropdetail = new AssetDetailsForApprovalViewModel();
                                    var asset = ddt.JobDetails.JobXAssets.Where(t => t.Id == assetdrop.JobXAssetId && t.RemovedBy == null && t.RemovedDate == null).Select(t => t.Asset).FirstOrDefault();
                                    //sending Id of asset table and not of AssetDrop
                                    if (asset != null)
                                    {
                                        assetDropdetail.AssetId = asset.Id;
                                        assetDropdetail.AssetName = asset != null ? asset.Name : string.Empty;
                                        assetDropdetail.AssetType = (AssetType)asset.Type;
                                        assetDropdetail.DroppedGallons = assetdrop.DroppedGallons.GetPreciseValue(6);
                                        assetDropdetail.DropEndTime = assetdrop.DropEndDate.ToString(Resource.constFormat12HourTime2);
                                        assetDropdetail.DropStartTime = assetdrop.DropStartDate.ToString(Resource.constFormat12HourTime2);
                                        assetDropdetail.InvoiceId = ddt.DDTId;
                                        assetDropdetail.OrderId = ddt.OrderId;
                                        assetDropdetail.PreDip = assetdrop.PreDip;
                                        assetDropdetail.PostDip = assetdrop.PostDip;
                                        assetDropdetail.PONumber = ddt.PoNumber;
                                        assetDropdetail.DropTicketNumber = ddt.DropTicketNo;
                                        assetDropdetail.UoM = ddt.UOM;
                                        assetDropdetail.JobXAssetId = assetdrop.JobXAssetId;
                                        assetDropdetail.TankScaleMeasurement = assetdrop.TankScaleMeasurement;
                                        listAssetDropDetails.Add(assetDropdetail);
                                        if (asset.Type == (int)AssetType.Tank)
                                        {
                                            assetIds.Add(asset.Id);
                                        }
                                    }
                                }
                            }
                        }
                        if (assetIds != null && assetIds.Any() && listAssetDropDetails.Any())
                        {
                            List<TankDetailViewModel> tankListInfo = await new FreightServiceDomain(this).GetTankList(assetIds);
                            if (tankListInfo != null && tankListInfo.Any())
                            {
                                foreach (var item in tankListInfo)
                                {
                                    var tank = listAssetDropDetails.Find(t => t.AssetId == item.AssetId);
                                    if (tank != null)
                                    {
                                        tank.TankMakeModel = item.TankMakeModel;
                                    }
                                }
                            }
                        }
                        response.AssetDetails.AddRange(listAssetDropDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetNoDataExceptionDDtsForDipData", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateInvoiceWithPrePostData(UserContext userContext, NoDataExceptionPrePostViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                if (viewModel != null && viewModel.AssetDetails != null && viewModel.AssetDetails.Any(t => t.DroppedGallons < 0))
                {
                    response.StatusMessage = Resource.errMessageInvalidDroppedGallons;
                }
                else if (viewModel != null && viewModel.AssetDetails != null && viewModel.AssetDetails.Any())
                {
                    int ddt = viewModel.AssetDetails.FirstOrDefault().InvoiceId;
                    response = await ContextFactory.Current.GetDomain<ConsolidatedDdtToInvoiceDomain>().ConvertDdtToInvoiceManually(userContext, ddt, false, viewModel);
                    if (response.StatusCode == (int)Status.Success)
                    {
                        //Call Queue Service Creation Method For Brokered Invoices Against That Orders
                        await GetBrokeredInvoicesForQueueServiceProcessingForDipData(ddt, viewModel, userContext);
                    }
                }
                else
                {
                    response.StatusMessage = Resource.errPrePostDipDataIsMissing;
                }
            }
            catch (Exception ex)
            {
                int invHeaderId = viewModel!=null? viewModel.InvoiceHeaderId:0;
                LogManager.Logger.WriteException("ExceptionDomain", "CreateInvoiceWithPrePostData", ex.Message + $"invoiceHeaderid={invHeaderId}", ex);
            }
            return response;
        }

        public async Task<StatusViewModel> RaiseWaiverRequest(int invoiceHeaderId, WaitingAction waitingFor, NoDataExceptionApproval noDataExceptionApproval, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var invoices = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId && t.WaitingFor == (int)waitingFor && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                     .ToListAsync();
                    if (invoices != null && invoices.Any())
                    {
                        await UpdateNoDataExceptionStatus(noDataExceptionApproval, userContext.Id, invoices);

                        var brokeredChainIds = invoices.Where(t => t.BrokeredChainId != null && t.BrokeredChainId != "").Select(t => t.BrokeredChainId).ToList();
                        if (brokeredChainIds.Any())
                        {
                            var invoiceIds = invoices.Select(t => t.Id);
                            var brokeredInvoices = await Context.DataContext.Invoices.Where(t => !invoiceIds.Contains(t.Id) && brokeredChainIds.Contains(t.BrokeredChainId) && t.WaitingFor == (int)waitingFor && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                         .ToListAsync();
                            await UpdateNoDataExceptionStatus(noDataExceptionApproval, 0, brokeredInvoices);
                        }

                        transaction.Commit();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageOnSubmitWaiverRequest;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errorMessageOnSubmitWaiverRequest;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("ExceptionDomain", "RaiseWaiverRequest", ex.Message, ex);
                }
            }
            return response;
        }

        private async Task UpdateNoDataExceptionStatus(NoDataExceptionApproval noDataExceptionApproval, int updatedBy, List<Invoice> invoices)
        {
            foreach (var invoice in invoices)
            {
                if (invoice.OrderId != null && invoice.OrderId > 0)
                {
                    var invoiceAdditionalDetail = invoice.InvoiceXAdditionalDetail;
                    if (invoiceAdditionalDetail != null)
                    {
                        if (updatedBy == 0)
                        {
                            updatedBy = invoice.Order.AcceptedBy;
                        }

                        invoiceAdditionalDetail.NoDataExceptionApprovalId = (int)noDataExceptionApproval;
                        invoice.UpdatedBy = updatedBy;
                        invoice.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(invoice).State = EntityState.Modified;
                        await Context.CommitAsync();

                        updatedBy = 0;
                    }
                }
            }
        }

        public async Task<List<NoDataExceptionDdtGridViewModel>> GetBuyerNoDataExceptionDdts(int companyId, NoDataExceptionDataTableViewModel requestModel)
        {
            List<NoDataExceptionDdtGridViewModel> response = new List<NoDataExceptionDdtGridViewModel>();
            try
            {
                if (requestModel == null)
                {
                    requestModel = new NoDataExceptionDataTableViewModel();
                }
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetBuyerNoDataExceptionDdts(companyId, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetBuyerNoDataExceptionDdts", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AcceptDDTWithoutData(int ddtId, WaitingAction waitingFor, NoDataExceptionApproval action, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<ConsolidatedDdtToInvoiceDomain>().ConvertDdtToInvoiceWithoutData(userContext, ddtId, action);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "AcceptDDTWithoutData", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AcceptDdtWithNoData(int invoiceHeaderId, WaitingAction waitingFor, NoDataExceptionApproval noDataExceptionApproval, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId && t.WaitingFor == (int)waitingFor && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                 .FirstOrDefaultAsync();
                if (invoice != null)
                {
                    // for brokered scenario, find the end supplier/carrier and process invoice creation from end supplier/carrier
                    if (invoice.BrokeredChainId != null && invoice.BrokeredChainId != "")
                    {
                        var endSupplierBrokeredInvoice = await Context.DataContext.Invoices.Where(t => t.BrokeredChainId == invoice.BrokeredChainId && t.WaitingFor == (int)waitingFor && t.Order != null && t.Order.IsEndSupplier && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                                 .OrderByDescending(t => t.InvoiceHeaderId)
                                                                                 .Select(t => new { t.Id, t.InvoiceHeaderId, t.Order.AcceptedCompanyId, t.Order.AcceptedBy, t.DisplayInvoiceNumber })
                                                                                 .FirstOrDefaultAsync();
                        // process invoice creation
                        if (endSupplierBrokeredInvoice != null)
                        {
                            var authDomain = new AuthenticationDomain(this);
                            var context = await authDomain.GetUserContextAsync(endSupplierBrokeredInvoice.AcceptedBy);
                            response = await AcceptDDTWithoutData(endSupplierBrokeredInvoice.Id, waitingFor, noDataExceptionApproval, context);

                            if (response.StatusCode == Status.Success)
                            {
                                await AddBrokerInvoicesToQueueServiceForNoData(endSupplierBrokeredInvoice.Id, waitingFor, noDataExceptionApproval);
                            }
                        }
                    }
                    else
                    {
                        // process invoice creation
                        var authDomain = new AuthenticationDomain(this);
                        var context = await authDomain.GetUserContextAsync(invoice.Order.AcceptedBy);
                        response = await AcceptDDTWithoutData(invoice.Id, waitingFor, noDataExceptionApproval, context);
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errorMessageOnSubmitWaiverRequest;
                LogManager.Logger.WriteException("ExceptionDomain", "AcceptDdtWithoutDataFromBuyer", ex.Message, ex);
            }

            return response;
        }

        private async Task GetBrokeredInvoicesForQueueServiceProcessingForDipData(int invoiceId, NoDataExceptionPrePostViewModel noDataExceptionPrePostModel, UserContext userContext)
        {
            try
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId && (t.BrokeredChainId != "" && t.BrokeredChainId != null) && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                .Select(t => new { t.OrderId, t.BrokeredChainId, t.WaitingFor })
                                                                .FirstOrDefaultAsync();

                if (invoice != null && invoice.OrderId != null)
                {
                    var brokeredOrderInfo = new Dictionary<int, int>();
                    var invEditDomain = new InvoiceEditDomain(this);
                    invEditDomain.GetBrokerOrdersTillOriginalOrder(invoice.OrderId.Value, brokeredOrderInfo);
                    foreach (var brokeredOrder in brokeredOrderInfo)
                    {
                        var brokeredInvoice = await Context.DataContext.Invoices.Where(t => t.OrderId == brokeredOrder.Key && t.BrokeredChainId == invoice.BrokeredChainId &&
                                                                                            t.WaitingFor == (int)WaitingAction.PrePostDipData && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                       .Select(t => new
                                                                       {
                                                                           t.InvoiceHeaderId,
                                                                           t.InvoiceHeader.InvoiceNumberId,
                                                                           InvoiceId = t.Id,
                                                                           OrderId = t.Order.Id,
                                                                           t.DisplayInvoiceNumber,
                                                                           t.BrokeredChainId,
                                                                           PoNumber = t.Order.PoNumber,
                                                                           IsEndSupplier = t.Order.IsEndSupplier,
                                                                           WaitingFor = t.WaitingFor,
                                                                           t.IsWetHosingDelivery,
                                                                           t.IsOverWaterDelivery,
                                                                           //t.InvoiceTypeId,
                                                                           t.PaymentTermId,
                                                                           t.PaymentDueDate,
                                                                           t.InvoiceXAdditionalDetail.PaymentMethod,
                                                                           t.Order.FuelRequest.FreightOnBoardTypeId,
                                                                           t.NetDays,
                                                                           t.InvoiceXAdditionalDetail.Notes,
                                                                           t.InvoiceXAdditionalDetail.SupplierAllowance,
                                                                           t.InvoiceXBolDetails,
                                                                           t.Order.DefaultInvoiceType,
                                                                           t.Order.AcceptedCompanyId,
                                                                           t.Order.AcceptedBy,
                                                                           OrderTerminalId = t.Order.TerminalId,
                                                                           FRTerminalId = t.Order.FuelRequest.TerminalId,
                                                                           t.Order.FuelRequest.FuelRequestDetail.OrderEnforcementId,
                                                                           t.FuelRequestFees,
                                                                           t.InvoiceXAdditionalDetail.OriginalInvoiceId,
                                                                           t.InvoiceHeader.TotalBasicAmount,
                                                                           t.InvoiceHeader.TotalTaxAmount,
                                                                           t.InvoiceHeader.TotalDiscountAmount,
                                                                           t.InvoiceHeader.TotalFeeAmount,
                                                                           TerminalId = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                                           StatusId = t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t2 => t2.IsActive).StatusId
                                                                       })
                                                                    .FirstOrDefaultAsync();
                        if (brokeredInvoice != null)
                        {
                            //UserContext brokerUserContext = new UserContext() { Id = brokeredInvoice.AcceptedBy, CompanyId = brokeredInvoice.AcceptedCompanyId };

                            if (noDataExceptionPrePostModel != null &&
                                noDataExceptionPrePostModel.AssetDetails != null && noDataExceptionPrePostModel.AssetDetails.Any())
                            {
                                noDataExceptionPrePostModel.InvoiceHeaderId = brokeredInvoice.InvoiceHeaderId;
                                var assetDetails = noDataExceptionPrePostModel.AssetDetails;
                                foreach (var asset in assetDetails)
                                {
                                    asset.InvoiceId = brokeredInvoice.InvoiceId;
                                    asset.OrderId = brokeredInvoice.OrderId;
                                }
                                await AddWaitingForDipDataBrokeredInvoicesToQueueService(brokeredInvoice.AcceptedBy, noDataExceptionPrePostModel, userContext);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "GetBrokeredInvoicesForQueueServiceProcessingForDipData", ex.Message, ex);
            }
        }

        public async Task<StatusViewModel> AddWaitingForDipDataBrokeredInvoicesToQueueService(int acceptedBy, NoDataExceptionPrePostViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                QueueProcessType queueProcessType = QueueProcessType.ConvertBrokeredInvoiceForDipData;
                var queueDomain = new QueueMessageDomain();
                var queueRequest = GetQueueEventForConvertBrokeredInvoiceForDipData(acceptedBy, queueProcessType, viewModel, userContext);
                var queueId = queueDomain.EnqeueMessage(queueRequest);
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.SuccessMsgAddedInvoicesToQueueService;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.ErrorMsgAddedInvoicesToQueueService;
                LogManager.Logger.WriteException("ExceptionDomain", "AddWaitingForDipDataBrokeredInvoicesToQueueService", ex.Message, ex);
            }
            return response;
        }



        private QueueMessageViewModel GetQueueEventForConvertBrokeredInvoiceForDipData(int acceptedBy, QueueProcessType queueProcessType, NoDataExceptionPrePostViewModel viewModel, UserContext userContext)
        {
            var jsonViewModel = new WaitingForDipDataBrokeredInvoicesProcessorModel();
            jsonViewModel.AcceptedBy = acceptedBy;
            jsonViewModel.NoDipDataExceptionModel = viewModel;
            string json = JsonConvert.SerializeObject(jsonViewModel);
            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = queueProcessType,
                JsonMessage = json
            };
        }

        public StatusViewModel ProcessConvertBrokeredInvoiceForDipData(ConvertBrokeredInvoiceForDipDataQueueMessage bulkUploadMsg, List<string> errorInfo)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                if (bulkUploadMsg != null)
                {
                    var domain = ContextFactory.Current.GetDomain<ConsolidatedDdtToInvoiceDomain>();
                    var acceptedById = bulkUploadMsg.AcceptedBy;
                    var authDomain = new AuthenticationDomain(this);
                    var brokerUserContext = authDomain.GetUserContextAsync(acceptedById).Result;

                    if (bulkUploadMsg.NoDipDataExceptionModel != null && bulkUploadMsg.NoDipDataExceptionModel.AssetDetails != null
                        && bulkUploadMsg.NoDipDataExceptionModel.AssetDetails.Any())
                    {
                        int ddt = bulkUploadMsg.NoDipDataExceptionModel.AssetDetails.FirstOrDefault().InvoiceId;
                        if (ddt > 0)
                        {
                            response = domain.ConvertDdtToInvoiceManually(brokerUserContext, ddt, true, bulkUploadMsg.NoDipDataExceptionModel).Result;
                            if (response.StatusCode == Status.Success)
                            {
                                errorInfo.Add("Brokered Invoice with InvoiceId" + ddt + " Created successfully");
                            }
                            else if (response.StatusCode == Status.Failed)
                            {
                                errorInfo.Add("Brokered Invoice with InvoiceId" + ddt + " Failed To Convert");
                            }
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "ProcessConvertBrokeredInvoiceForDipData", ex.Message, ex);
            }
            return response;
        }

        private async Task AddBrokerInvoicesToQueueServiceForNoData(int invoiceId, WaitingAction waitingFor, NoDataExceptionApproval noDataExceptionApproval)
        {
            try
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId && (t.BrokeredChainId != "" && t.BrokeredChainId != null) && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                .Select(t => new { t.OrderId, t.BrokeredChainId, t.WaitingFor })
                                                                .FirstOrDefaultAsync();

                if (invoice != null && invoice.OrderId != null)
                {
                    var invoiceEditDomain = new InvoiceEditDomain(this);
                    var brokeredOrderInfo = new Dictionary<int, int>();

                    invoiceEditDomain.GetBrokerOrdersTillOriginalOrder(invoice.OrderId.Value, brokeredOrderInfo);
                    foreach (var brokeredOrder in brokeredOrderInfo)
                    {
                        var brokeredInvoice = await Context.DataContext.Invoices.Where(t => t.OrderId == brokeredOrder.Key && t.BrokeredChainId == invoice.BrokeredChainId &&
                                                                                            t.WaitingFor == (int)waitingFor && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                       .Select(t => new NoDataProcessorModel
                                                                       {
                                                                           InvoiceHeaderId = t.InvoiceHeaderId,
                                                                           DdtId = t.Id,
                                                                           OrderId = t.Order.Id,
                                                                           AcceptedBy = t.Order.AcceptedBy,
                                                                           AcceptedCompanyId = t.Order.AcceptedCompanyId,
                                                                           DisplayInvoiceNumber = t.DisplayInvoiceNumber,
                                                                           BrokeredChainId = t.BrokeredChainId,
                                                                           WaitingFor = t.WaitingFor,
                                                                           NoDataExceptionApproval = noDataExceptionApproval
                                                                       })
                                                                    .FirstOrDefaultAsync();
                        if (brokeredInvoice != null)
                        {
                            var authDomain = new AuthenticationDomain(this);
                            var brokerUserContext = await authDomain.GetUserContextAsync(brokeredInvoice.AcceptedBy);
                            var response = AddQueueEventForNoDataExceptionBrokeredInvoice(brokerUserContext, brokeredInvoice);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "AddBrokerInvoicesToQueueServiceForNoData", ex.Message, ex);
            }
        }

        private StatusViewModel AddQueueEventForNoDataExceptionBrokeredInvoice(UserContext userContext, NoDataProcessorModel brokeredInvoice)
        {
            var response = new StatusViewModel();
            try
            {
                QueueProcessType queueProcessType = QueueProcessType.CreateInvoiceForNoData;

                string json = JsonConvert.SerializeObject(brokeredInvoice);
                var queueRequest = new QueueMessageViewModel()
                {
                    CreatedBy = userContext.Id,
                    QueueProcessType = queueProcessType,
                    JsonMessage = json
                };

                var queueDomain = new QueueMessageDomain();
                var queueId = queueDomain.EnqeueMessage(queueRequest);
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.succcessMsgInvoiceDDTSubmitted;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionDomain", "AddQueueEventForNoDataExceptionBrokeredInvoice", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<string>> ProcessNoDataInvoiceCreate(NoDataProcessorModel model)
        {
            var response = new List<string>();
            try
            {
                var authDomain = new AuthenticationDomain(this);
                var userContext = await authDomain.GetUserContextAsync(model.AcceptedBy);
                var result = await ContextFactory.Current.GetDomain<ConsolidatedDdtToInvoiceDomain>().ConvertDdtToInvoiceWithoutData(userContext, model.DdtId, model.NoDataExceptionApproval);
            }
            catch (Exception ex)
            {
                response.Add(ex.Message);
                LogManager.Logger.WriteException("ExceptionDomain", "ProcessNoDataInvoiceCreate", ex.Message, ex);
            }

            return response;
        }
    }
}
