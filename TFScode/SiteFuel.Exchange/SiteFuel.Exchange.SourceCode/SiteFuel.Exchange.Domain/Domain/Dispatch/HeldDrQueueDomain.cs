using Easy.Common;
using Easy.Common.Interfaces;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class HeldDrQueueDomain : FreightServiceApiDomain
    {
        public HeldDrQueueDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public HeldDrQueueDomain(string connectionString) : base(connectionString)
        {
        }

        public HeldDrQueueDomain(BaseDomain domain)
            : base(domain)
        {
        }


        public async Task<HeldDeliveryRequestsModel> PushtoHeldQueue(UserContext userContext, List<RaiseDeliveryRequestInput> raiseDeliveryRequests)
        {
            var response = new HeldDeliveryRequestsModel();
            try
            {
                List<HeldDeliveryRequestModel> input = new List<HeldDeliveryRequestModel>();
                List<int> supplierCompanyIds = raiseDeliveryRequests.Where(t => t.SupplierCompanyId > 0).Select(t => t.SupplierCompanyId.Value).Distinct().ToList();
                List<int> orderIds = raiseDeliveryRequests.Where(t => t.OrderId > 0).Select(t => t.OrderId.Value).ToList();
                var companyIdentifiers = await Context.DataContext.MstCompanyIdentifiers.Where(t => supplierCompanyIds.Contains(t.SupplierCompanyId)).Select(t => new { t.SupplierCompanyId, t.Identifier }).ToListAsync();
                var orders = await Context.DataContext.Orders.Where(t => orderIds.Contains(t.Id)).Select(t => new { t.FuelRequest.Job.TimeZoneName, t.Id, t.FuelRequest.UoM, JobName = t.FuelRequest.Job.Name, DisplayName = t.FuelRequest.MstProduct.DisplayName, FuelName = t.FuelRequest.MstProduct.Name, t.FuelRequest.MstProduct.MstProductType.ProductCode }).ToListAsync();
                foreach (var dr in raiseDeliveryRequests)
                {
                    HeldDeliveryRequestModel heldDr = dr.ToHeldDrViewModel();
                    heldDr.CreatedBy = userContext.Id;
                    heldDr.SupplierCompanyId = userContext.CompanyId;
                    heldDr.CreatedByCompanyId = userContext.CompanyId;
                    heldDr.Status = HeldDrStatus.New;
                    heldDr.CreatedOn = DateTimeOffset.Now;
                    heldDr.CompanyType = userContext.CompanyTypeId;
                    heldDr.UserId = userContext.Id;
                    var order = orders.FirstOrDefault(t => t.Id == heldDr.OrderId.Value);
                    if (order != null)
                    {
                        heldDr.JobName = order.JobName;
                        heldDr.UoM = (int)order.UoM;
                        heldDr.ProductShortCode = order.ProductCode;
                        var jobOffsetInfo = new List<TimeZoneOffsetModel> { new TimeZoneOffsetModel() { Id = dr.OrderId.Value, TimeZoneName = order.TimeZoneName } };
                        GetOffsetForTimezones(jobOffsetInfo);
                        heldDr.JobTimeZoneOffset = jobOffsetInfo.Where(t => t.Id == dr.OrderId.Value).Select(t => t.Offset).FirstOrDefault();
                        heldDr.FuelType = !string.IsNullOrWhiteSpace(order.DisplayName) ? order.DisplayName : order.FuelName;
                    }
                    GetUniqueOrderNo(userContext, heldDr);
                    input.Add(heldDr);
                }
                response = await ApiPostCall<HeldDeliveryRequestsModel>(ApplicationConstants.UrlRaiseHeldDeliveryRequests, input);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldDrQueueDomain", "PushtoHeldQueue", ex.Message, ex);
            }
            return response;
        }

        public void GetUniqueOrderNo(UserContext userContext, HeldDeliveryRequestModel model)
        {
            string[] companyWordsDetails = userContext.CompanyName.Split(' ');
            var supplierName = GetCompanyWordInfo(userContext.CompanyName, companyWordsDetails);
            string[] customercompanyWordsDetails = model.CustomerCompany.Split(' ');
            var customerName = GetCompanyWordInfo(model.CustomerCompany, customercompanyWordsDetails);
            var productCode = model.ProductShortCode;
            var dateFormat = DateTime.Now.ToString("MMddyy");
            var uniqueDRID = supplierName + customerName + productCode + dateFormat + GetUniqueKey();
            model.UniqueOrderNo = uniqueDRID;
        }

        public async Task PushDRStatusToQueue(OrderStatusRequestModel model)
        {
            try
            {
                if (model.SalesOrderStatus.Length > 0)
                {
                    foreach (var drStatus in model.SalesOrderStatus)
                    {
                        var sapOrderStatus = new SAPOrderStatus() { JsonRequest = JsonConvert.SerializeObject(drStatus), IsProcessed = false };
                        Context.DataContext.SAPOrderStatus.Add(sapOrderStatus);
                    }
                    await Context.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldDrQueueDomain", "PushDRStatusToQueue", ex.Message + "Model:" + JsonConvert.SerializeObject(model), ex);
            }
        }

        public StatusViewModel CallSAPAPI(HeldDeliveryRequestsModel heldDrs, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                if (heldDrs != null && heldDrs.Requests != null)
                {
                    foreach (var dr in heldDrs.Requests)
                    {
                        string json = JsonConvert.SerializeObject(dr);

                        var queueRequest = new QueueMessageViewModel()
                        {
                            CreatedBy = userContext.Id,
                            QueueProcessType = QueueProcessType.SAPOrderCreation,
                            JsonMessage = json
                        };

                        var queueId = new QueueMessageDomain().EnqeueMessage(queueRequest);
                    }
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.SuccessDrEnqueue;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldDrQueueDomain", "CallSAPAPI", ex.Message, ex);
            }
            return response;
        }


        public async Task<StatusViewModel> EditHeldDr(HeldDeliveryRequestModel model, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                var apiResponse = await ApiPostCall<HeldDeliveryRequestModel>(ApplicationConstants.UrlEditHeldDr, model);
                if (!string.IsNullOrWhiteSpace(apiResponse.HeldDrId))
                {
                    if (apiResponse.IsDREdited)
                    {
                        HeldDeliveryRequestsModel heldDrModel = new HeldDeliveryRequestsModel();
                        heldDrModel.Requests.Add(apiResponse);
                        CallSAPAPI(heldDrModel, userContext);
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.msgDelReqEditSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldDrQueueDomain", "EditHeldDr", ex.Message + " model:" + JsonConvert.SerializeObject(model), ex);
            }
            return response;
        }


        public async Task<StatusViewModel> DeleteHeldDr(string id, UserContext user)
        {

            var response = new StatusViewModel();
            try
            {
                var url = string.Format(ApplicationConstants.UrlDeleteHeldDr, id, user.Id);
                var apiResponse = await ApiGetCall<HeldDeliveryRequestsModel>(url);
                if (apiResponse.StatusCode == Status.Success && apiResponse.Requests != null && apiResponse.Requests.Any())
                {
                    var heldDr = apiResponse.Requests[0];
                    heldDr.RequiredQuantity = 0;
                    CallSAPAPI(apiResponse, user);
                }
                response.StatusCode = apiResponse.StatusCode;
                response.StatusMessage = apiResponse.StatusMessage;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldDrQueueDomain", "DeleteHeldDr", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<HeldDeliveryRequestModel>> GetHeldDeliveryRequests(int companyId)
        {

            var response = new List<HeldDeliveryRequestModel>();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetHeldDeliveryRequests, companyId);
                response = await ApiGetCall<List<HeldDeliveryRequestModel>>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldDrQueueDomain", "GetHeldDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public async Task<HeldDeliveryRequestModel> GetHeldDeliveryRequestById(string id)
        {

            var response = new HeldDeliveryRequestModel();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetHeldDeliveryRequestById, id);
                response = await ApiGetCall<HeldDeliveryRequestModel>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldDrQueueDomain", "GetHeldDeliveryRequestById", ex.Message + " Id:" + id, ex);
            }
            return response;
        }

        public async Task<List<string>> CreateSAPOrderFromQueueService(HeldDeliveryRequestModel model)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("CreateSAPOrderFromQueueService", "HeldDrQueueDomain"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    var response = new StatusViewModel();
                    if (model != null)
                    {
                        var _keys = new List<string> { ApplicationConstants.SAPOrderCreationUrl, ApplicationConstants.SAPUserId, ApplicationConstants.SAPPassword };
                        var _sapSettings = Context.DataContext.MstAppSettings.Where(t => _keys.Contains(t.Key) && t.IsActive).Select(t => new { t.Key, t.Value }).ToList();
                        var sapUrl = _sapSettings.Where(t => t.Key == ApplicationConstants.SAPOrderCreationUrl).Select(t => t.Value).FirstOrDefault();
                        var sapUserId = _sapSettings.Where(t => t.Key == ApplicationConstants.SAPUserId).Select(t => t.Value).FirstOrDefault();
                        var sapPassword = _sapSettings.Where(t => t.Key == ApplicationConstants.SAPPassword).Select(t => t.Value).FirstOrDefault();
                        if (_sapSettings == null || string.IsNullOrWhiteSpace(sapUrl) || string.IsNullOrWhiteSpace(sapUserId) || string.IsNullOrWhiteSpace(sapPassword))
                            throw new Exception("ApiPostCall: SAP configuration is missing");

                        var authenticationString = $"{sapUserId}:{sapPassword}";
                        var token = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));

                        var status = await CallSapOrderCreationAPI(model, sapUrl, token);
                        if (status.StatusCode == Status.Failed)
                        {
                            errorInfo.Add(status.StatusMessage);
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = status.StatusMessage;
                        }
                        else
                        {
                            await UpdateHeldDrStatus(model.HeldDrId);
                        }
                    }
                    else
                    {
                        errorInfo.Add("Invalid request");
                        response.StatusCode = Status.Failed;
                    }
                }

                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                    {
                        LogManager.Logger.WriteException("CreateDRFromQueueService", "ThirdPartyOrderDomain", ex.Message, ex);
                    }

                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.RequestError);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return errorInfo;
            }
        }

        public StatusViewModel GetSapOrderCreationInput(HeldDeliveryRequestModel model, SapOrderCreationRequest salesOrderDetails)
        {
            StatusViewModel response = new StatusViewModel();
            salesOrderDetails.SalesOrderDetails.ExternalOrderNo = model.UniqueOrderNo;
            string terminalControl = string.Empty;
            if (model.Terminal != null && model.Terminal.Id > 0)
            {
                terminalControl = Context.DataContext.TerminalCompanyAliases.Where(t => t.TerminalId == model.Terminal.Id && t.CreatedByCompanyId == model.SupplierCompanyId && t.IsActive).Select(t => t.AssignedTerminalId).FirstOrDefault();
            }
            else if (model.Bulkplant != null && model.Bulkplant.SiteId > 0)
            {
                terminalControl = Context.DataContext.TerminalCompanyAliases.Where(t => t.BulkPlantId == model.Bulkplant.SiteId && t.CreatedByCompanyId == model.SupplierCompanyId && t.IsActive).Select(t => t.AssignedTerminalId).FirstOrDefault();
            }
            string customerId = Context.DataContext.CarrierCustomerMappings.Where(t => t.CarrierCompanyId == model.SupplierCompanyId && t.CarrierCustomerId == model.BuyerCompanyId && t.IsActive).Select(t => t.CarrierAssignedCustomerId).FirstOrDefault();
            string productId = string.Empty;
            int? fuelTypeId = Context.DataContext.Orders.Where(t => t.Id == model.OrderId).Select(t => t.FuelRequest.MstProduct.TfxProductId).FirstOrDefault();
            if (fuelTypeId.HasValue)
            {
                productId = Context.DataContext.SupplierMappedProductDetails.Where(t => t.FuelTypeId == fuelTypeId && t.TerminalId == null && t.CompanyId == model.SupplierCompanyId && t.IsActive).Select(t => t.MyProductId).FirstOrDefault();
            }
            List<string> missingFields = new List<string>();
            if (string.IsNullOrWhiteSpace(terminalControl))
            {
                missingFields.Add("Terminal/Bulkplant");
                response.FailedStatusCode = 1;
            }
            if (string.IsNullOrWhiteSpace(customerId))
            {
                missingFields.Add("Customer");
                response.FailedStatusCode = 1;
            }
            if (String.IsNullOrWhiteSpace(model.SiteId))
            {
                missingFields.Add("Location");
                response.FailedStatusCode = 1;
            }
            if (string.IsNullOrWhiteSpace(productId))
            {
                missingFields.Add("Product");
                response.FailedStatusCode = 1;
            }
            if (response.FailedStatusCode == 1)
            {
                response.StatusMessage = string.Format(Resource.valMessageMappingReq, string.Join(", ", missingFields));
                return response;
            }
            salesOrderDetails.SalesOrderDetails.CustomerID = customerId;
            salesOrderDetails.SalesOrderDetails.LocationID = model.SiteId;
            salesOrderDetails.SalesOrderDetails.SAP_DocNumber = model.Sap_OrderNo;
            salesOrderDetails.SalesOrderDetails.TerminalControl = terminalControl;
            if (!string.IsNullOrWhiteSpace(model.SelectedDate))
            {
                salesOrderDetails.SalesOrderDetails.LiftDate = DateTimeOffset.Parse(model.SelectedDate).ToString("yyyyMMdd");
            }
            else
            {
                salesOrderDetails.SalesOrderDetails.LiftDate = DateTimeOffset.Now.Add(model.JobTimeZoneOffset).ToString("yyyyMMdd");
            }
            salesOrderDetails.SalesOrderDetails.Products = new SapProductModel[] { new SapProductModel() {
                OrderQuantity = model.RequiredQuantity.ToString(),
                Price = model.IndicativePrice.ToString(),
                ProductID = productId
            } };
            response.StatusCode = Status.Success;
            return response;
        }

        public async Task<StatusViewModel> CallSapOrderCreationAPI(HeldDeliveryRequestModel model, string sapUrl, string token)
        {
            StatusViewModel response = new StatusViewModel();
            SapOrderCreationRequest salesOrderDetails = new SapOrderCreationRequest();
            try
            {
                response = GetSapOrderCreationInput(model, salesOrderDetails);
                if (response.StatusCode == Status.Failed)
                {
                    if (response.FailedStatusCode == 1)
                    {
                        await new FreightServiceDomain(this).UpdateHeldDrValidationStatus(model.HeldDrId, response.StatusMessage);
                    }
                    LogManager.Logger.WriteDebug("SAP", "CallSapOrderCreationAPI:Failed", "HeldDrId: " + salesOrderDetails.SalesOrderDetails.ExternalOrderNo + " response: " + JsonConvert.SerializeObject(response.StatusMessage));
                    return response;
                }
                IDictionary<string, IEnumerable<string>> defaultRequestHeaders = new Dictionary<string, IEnumerable<string>>();
                defaultRequestHeaders.Add("Authorization", new List<string>() { "Basic " + token });

                using (IRestClient client = new RestClient(defaultRequestHeaders))
                {
                    var json = JsonConvert.SerializeObject(salesOrderDetails);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    HttpResponseMessage apiResponse = await client.PostAsync(sapUrl, stringContent);
                    if (apiResponse.IsSuccessStatusCode)
                    {
                        if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK || apiResponse.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {
                            LogManager.Logger.WriteDebug("SAP", "OrderCreationSuccess", "HeldDrId: " + salesOrderDetails.SalesOrderDetails.ExternalOrderNo);
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
                            LogManager.Logger.WriteDebug("SAP", "OrderCreationFailed", "HeldDrId: " + salesOrderDetails.SalesOrderDetails.ExternalOrderNo);
                        }
                    }
                    else
                    {
                        LogManager.Logger.WriteDebug("SAP", "OrderCreationFailed", "HeldDrId: " + salesOrderDetails.SalesOrderDetails.ExternalOrderNo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteDebug("SAP", "OrderCreationFailed", "HeldDrId: " + salesOrderDetails.SalesOrderDetails.ExternalOrderNo);
            }
            return response;
        }

        public async Task UpdateHeldDrStatus(string id)
        {
            try
            {
                var url = string.Format(ApplicationConstants.UrlUpdateHeldDrStatus, id);
                await ApiGetCall<StatusViewModel>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldDrQueueDomain", "UpdateHeldDrStatus", ex.Message + "id:" + id, ex);
            }
        }

        public async Task<List<SalesOrderStatusRequestModel>> GetUnProcessedOrderStatusRequest()
        {
            var response = new List<SalesOrderStatusRequestModel>();
            try
            {
                response = await Context.DataContext.SAPOrderStatus.Where(t => !t.IsProcessed).Select(t => new SalesOrderStatusRequestModel
                {
                    Id = t.Id,
                    JsonRequest = t.JsonRequest
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldDrQueueDomain", "GetUnProcessedOrderStatusRequest: Failed", ex.Message, ex);
            }
            return response;
        }

        public async Task UpdateIsProcessedColumn(int id)
        {
            try
            {
                var request = await Context.DataContext.SAPOrderStatus.FirstOrDefaultAsync(t => t.Id == id);
                request.IsProcessed = true;
                await Context.CommitAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldDrQueueDomain", "UpdateIsProcessedColumn: Failed", ex.Message, ex);
            }
        }

        public async Task UpdateHeldDrCreditCheckStatus(SalesOrderStatusModel viewModel)
        {
            try
            {
                //get userdetails from token
                var authDomain = new AuthenticationDomain(this);
                var response = await ApiPostCall<HeldDeliveryRequestModel>(ApplicationConstants.UrlUpdateHeldDrCreditCheckStatus, viewModel);
                if (viewModel.SAP_Order_Status == "00" && response != null && !string.IsNullOrWhiteSpace(response.HeldDrId))
                {
                    var userContext = await authDomain.GetUserContextAsync(response.UserId, response.CompanyType);
                    if (viewModel.Products != null && viewModel.Products.Any())
                    {
                        int? fuelTypeId = Context.DataContext.Orders.Where(t => t.Id == response.OrderId).Select(t => t.FuelRequest.MstProduct.TfxProductId).FirstOrDefault();
                        string productId = Context.DataContext.SupplierMappedProductDetails.Where(t => t.FuelTypeId == fuelTypeId && t.TerminalId == null && t.CompanyId == response.SupplierCompanyId && t.IsActive).Select(t => t.MyProductId).FirstOrDefault();
                        var productModel = viewModel.Products.FirstOrDefault(t => t.ProductID == productId);
                        if (productModel != null && !string.IsNullOrWhiteSpace(productModel.Price))
                        {
                            decimal price = 0;
                            var isConverted = decimal.TryParse(productModel.Price, out price);
                            if (isConverted && price > 0)
                            {
                                response.IndicativePrice = price;
                            }
                        }
                    }
                    var drCreationStatus = await new FreightServiceDomain(this).RaiseDeliveryRequests(new List<RaiseDeliveryRequestInput>() { response }, userContext);
                    if (drCreationStatus.StatusCode == Status.Failed)
                    {
                        LogManager.Logger.WriteDebug("HeldDrQueueDomain", "UpdateHeldDrCreditCheckStatus: DR Creation Failed", "model:" + JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldDrQueueDomain", "UpdateHeldDrCreditCheckStatus: Failed", ex.Message + " model:" + JsonConvert.SerializeObject(viewModel), ex);
            }
        }

        public async Task<StatusViewModel> OverrideCreditCheckApproval(string heldDrId, string fileName, string filePath, int userId)
        {
            var response = new StatusViewModel();
            try
            {
                var viewModel = new OverrideCreditCheckApprovalModel() { FileName = fileName, FilePath = filePath, HeldDRId = heldDrId, UserId = userId };
                var apiResponse = await ApiPostCall<HeldDeliveryRequestModel>(ApplicationConstants.UrlOverrideCreditCheckApproval, viewModel);
                if (!string.IsNullOrWhiteSpace(apiResponse.HeldDrId))
                {
                    var authDomain = new AuthenticationDomain(this);
                    var userContext = await authDomain.GetUserContextAsync(apiResponse.UserId, apiResponse.CompanyType);
                    var drCreationStatus = await new FreightServiceDomain(this).RaiseDeliveryRequests(new List<RaiseDeliveryRequestInput>() { apiResponse }, userContext);
                    if (drCreationStatus.StatusCode == Status.Failed)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = drCreationStatus.StatusMessage;
                        LogManager.Logger.WriteDebug("HeldDrQueueDomain", "OverrideCreditCheckApproval: DR Creation Failed", "model:" + JsonConvert.SerializeObject(apiResponse));
                    }
                    else
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = drCreationStatus.StatusMessage;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("HeldDrQueueDomain", "OverrideCreditCheckApproval: Failed", ex.Message + " model:" + heldDrId, ex);
            }
            return response;
        }
    }
}
