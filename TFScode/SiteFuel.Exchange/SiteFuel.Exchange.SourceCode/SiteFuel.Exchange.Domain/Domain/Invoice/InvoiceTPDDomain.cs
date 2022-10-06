using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Domain.Services;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.WebNotification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteFuel.Exchange.Domain
{
    public class InvoiceTPDDomain : InvoiceBaseDomain
    {
        public InvoiceTPDDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }
        public InvoiceTPDDomain(BaseDomain domain) : base(domain)
        {
        }

        OrderEnforcement _firstOrderEnforcement = OrderEnforcement.EnforceOrderLevelValues;



        bool canCreateConsolidateInvoice = true;
        int supplierCompanyIdForCarrier = 0;
        bool IsValidAddress = true;

        #region API Methods
        public async Task<ApiResponseViewModel> ValidateAndProcessApi(TPDInvoiceViewModel apiRequestModel, string token)
        {
            ApiResponseViewModel response = new ApiResponseViewModel(Status.Failed);
            try
            {
                if (apiRequestModel != null)
                {
                    //get userdetails from token
                    var authDomain = new AuthenticationDomain(this);
                    var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
                    if (apiUserContext != null)
                    {
                        supplierCompanyIdForCarrier = SetCarrierCompanyParameters(response, apiRequestModel.CustomerID, apiUserContext);

                        ValidateHeader(response, apiRequestModel);

                        if (!response.Messages.Any())
                        {
                            ProcessAPI(response, apiUserContext, apiRequestModel);
                        }
                        else if (supplierCompanyIdForCarrier == 0 && !string.IsNullOrWhiteSpace(apiRequestModel.ExternalRefID))
                        {
                            List<ApiCodeMessages> exceptionMessages = new List<ApiCodeMessages>();
                            var apiResponse = await CreateUnassignedExceptionDdt(response, apiUserContext, apiRequestModel, exceptionMessages, false);
                            response.Messages.AddRange(exceptionMessages);
                        }
                    }
                    else
                        response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ01, Message = Resource.errMsgInvalidToken });
                }
                else
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = Resource.errMsgParmeterNotMatch });
                if (response.Status == Status.Failed && response.Messages.Any())
                {
                    await SaveCarrierDeliveryFailure(apiRequestModel, response, supplierCompanyIdForCarrier);
                }
            }
            catch (Exception ex)
            {
                
                    LogManager.Logger.WriteException("InvoiceTPDDomain", "ValidateAndProcessApi", ex.Message, ex);
            }

            return response;
        }

        private async Task SaveCarrierDeliveryFailure(TPDInvoiceViewModel apiRequestModel, ApiResponseViewModel response, int supplierCompanyIdForCarrier)
        {
            try
            {


                var Ordermessages = response.Messages.Where(x => x.OrderId != null && x.OrderId > 0).ToList();
                if (Ordermessages.Any())
                {
                    var OrderIds = Ordermessages.Where(x => x.OrderId != null && x.OrderId > 0).GroupBy(x => x.OrderId).Select(x => x.Key.Value).ToList();
                    var orderInfo = await Context.DataContext.Orders.Where(x => OrderIds.Contains(x.Id) && x.IsEndSupplier).Select(x => new
                    {
                        OrderId = x.Id,
                        BuyerCompanyId = x.BuyerCompanyId,
                        SupplierCompanyId = x.AcceptedCompanyId,
                        x.FuelRequest
                    }).ToListAsync();
                    foreach (var item in orderInfo)
                    {
                        string requestJson = string.Empty;
                        var failureMessage = response.Messages.Where(x => x.OrderId.Value == item.OrderId).ToList();
                        var finalMessages = new List<ApiCodeMessages>();
                        var failureMessageGroupBy = failureMessage.GroupBy(x => new { x.OrderId, x.Code }).Select(x => x.Key).ToList();
                        if (failureMessageGroupBy.Any())
                        {
                            foreach (var apicodeitem in failureMessageGroupBy)
                            {
                                var failureMessageInfo = failureMessage.Where(x => x.OrderId == apicodeitem.OrderId && x.Code == apicodeitem.Code).ToList();
                                foreach (var failureMessageData in failureMessageInfo)
                                {
                                    if (failureMessage != null)
                                    {
                                        var finalmessageInfo = new ApiCodeMessages();
                                        finalmessageInfo.Code = apicodeitem.Code;
                                        finalmessageInfo.OrderId = apicodeitem.OrderId;
                                        finalmessageInfo.Message = failureMessageData.Message;
                                        finalMessages.Add(finalmessageInfo);


                                    }
                                }
                            }
                            var apiRequestOrderModel = apiRequestModel;
                            if (item.FuelRequest != null && item.FuelRequest.Job != null)
                            {
                                if (apiRequestOrderModel.DropDetails.Any())
                                {
                                    apiRequestOrderModel.DropDetails.ForEach(x =>
                                    {
                                        x.Product = !string.IsNullOrEmpty(item.FuelRequest.MstProduct.DisplayName) ? item.FuelRequest.MstProduct.DisplayName : item.FuelRequest.MstProduct.Name;
                                    });
                                }
                                apiRequestOrderModel.DropAddress1 = item.FuelRequest.Job.Name;
                                apiRequestOrderModel.DropAddress2 = item.FuelRequest.Job.Address;
                                apiRequestOrderModel.DropCity = item.FuelRequest.Job.City;
                                apiRequestOrderModel.DropStateCode = item.FuelRequest.Job.MstState.Code;
                                apiRequestOrderModel.DropZip = item.FuelRequest.Job.ZipCode;
                                requestJson = JsonConvert.SerializeObject(apiRequestOrderModel);
                            }
                            CarrierXDeliveryFailure carrierXDeliveryFailure = new CarrierXDeliveryFailure();
                            carrierXDeliveryFailure.RequestJson = requestJson;
                            carrierXDeliveryFailure.ResponseJson = JsonConvert.SerializeObject(response);
                            carrierXDeliveryFailure.RequestType = (int)CarrierXDeliveryFailureRequestType.TPDAPI;
                            carrierXDeliveryFailure.FailureReason = JsonConvert.SerializeObject(finalMessages);
                            carrierXDeliveryFailure.BuyerCompanyId = item.BuyerCompanyId;
                            carrierXDeliveryFailure.SupplierCompanyId = item.SupplierCompanyId;
                            carrierXDeliveryFailure.IsEndSupplier = 1;
                            if (response.Messages.Any())
                            {
                                if (!string.IsNullOrEmpty(response.Messages.FirstOrDefault().EntityId))
                                {
                                    carrierXDeliveryFailure.EntityId = Convert.ToInt32(response.Messages.FirstOrDefault().EntityId);
                                }
                            }
                            carrierXDeliveryFailure.CreatedDate = DateTime.Now;
                            Context.DataContext.CarrierXDeliveryFailures.Add(carrierXDeliveryFailure);
                            await Context.CommitAsync();
                        }
                    }
                }
                var withoutOrdermessages = response.Messages.Where(x => x.OrderId == 0).ToList();
                if (withoutOrdermessages.Any() && supplierCompanyIdForCarrier > 0)
                {
                    int carrierCompanyId = 0;
                    if (apiRequestModel.DropDetails != null && apiRequestModel.DropDetails.Any() && apiRequestModel.DropDetails.SelectMany(x => x.LiftDetails).Any())
                    {
                        var carrierCompany = apiRequestModel.DropDetails.SelectMany(x => x.LiftDetails).Select(x1 => x1.LiftCarrier).Distinct().FirstOrDefault();
                        var carrierCompanyInfo = await Context.DataContext.Companies.Where(x => x.Name.ToLower() == carrierCompany.Trim().ToLower() && x.IsActive).FirstOrDefaultAsync();
                        if (carrierCompanyInfo != null)
                        {
                            carrierCompanyId = carrierCompanyInfo.Id;
                        }
                    }
                    if (carrierCompanyId > 0)
                    {
                        CarrierXDeliveryFailure carrierXDeliveryFailure = new CarrierXDeliveryFailure();
                        carrierXDeliveryFailure.RequestJson = JsonConvert.SerializeObject(apiRequestModel);
                        carrierXDeliveryFailure.ResponseJson = JsonConvert.SerializeObject(response);
                        carrierXDeliveryFailure.RequestType = (int)CarrierXDeliveryFailureRequestType.TPDAPI;
                        carrierXDeliveryFailure.FailureReason = JsonConvert.SerializeObject(withoutOrdermessages);
                        carrierXDeliveryFailure.BuyerCompanyId = supplierCompanyIdForCarrier;
                        carrierXDeliveryFailure.SupplierCompanyId = carrierCompanyId;
                        carrierXDeliveryFailure.IsEndSupplier = 1;
                        if (response.Messages.Any())
                        {
                            if (!string.IsNullOrEmpty(response.Messages.FirstOrDefault().EntityId))
                            {
                                carrierXDeliveryFailure.EntityId = Convert.ToInt32(response.Messages.FirstOrDefault().EntityId);
                            }
                        }
                        carrierXDeliveryFailure.CreatedDate = DateTime.Now;
                        Context.DataContext.CarrierXDeliveryFailures.Add(carrierXDeliveryFailure);
                        await Context.CommitAsync();
                    }
                    else
                    {
                        var carrierCustomerMappingsInfo = await Context.DataContext.CarrierCustomerMappings
                                       .Where(t => t.CarrierAssignedCustomerId.ToLower().Equals(apiRequestModel.CustomerID.ToLower()) && t.IsActive).FirstOrDefaultAsync();
                        if (carrierCustomerMappingsInfo != null)
                        {
                            CarrierXDeliveryFailure carrierXDeliveryFailure = new CarrierXDeliveryFailure();
                            carrierXDeliveryFailure.RequestJson = JsonConvert.SerializeObject(apiRequestModel);
                            carrierXDeliveryFailure.ResponseJson = JsonConvert.SerializeObject(response);
                            carrierXDeliveryFailure.RequestType = (int)CarrierXDeliveryFailureRequestType.TPDAPI;
                            carrierXDeliveryFailure.FailureReason = JsonConvert.SerializeObject(withoutOrdermessages);
                            carrierXDeliveryFailure.BuyerCompanyId = carrierCustomerMappingsInfo.CarrierCustomerId;
                            carrierXDeliveryFailure.SupplierCompanyId = carrierCustomerMappingsInfo.CarrierCompanyId;
                            carrierXDeliveryFailure.IsEndSupplier = 1;
                            if (response.Messages.Any())
                            {
                                if (!string.IsNullOrEmpty(response.Messages.FirstOrDefault().EntityId))
                                {
                                    carrierXDeliveryFailure.EntityId = Convert.ToInt32(response.Messages.FirstOrDefault().EntityId);
                                }
                            }
                            carrierXDeliveryFailure.CreatedDate = DateTime.Now;
                            Context.DataContext.CarrierXDeliveryFailures.Add(carrierXDeliveryFailure);
                            await Context.CommitAsync();
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(apiRequestModel.CustomerID))
                {

                    var carrierCustomerMappingsInfo = await Context.DataContext.CarrierCustomerMappings
                                     .Where(t => t.CarrierAssignedCustomerId.ToLower().Equals(apiRequestModel.CustomerID.ToLower()) && t.IsActive).FirstOrDefaultAsync();
                    if (carrierCustomerMappingsInfo != null)
                    {
                        CarrierXDeliveryFailure carrierXDeliveryFailure = new CarrierXDeliveryFailure();
                        carrierXDeliveryFailure.RequestJson = JsonConvert.SerializeObject(apiRequestModel);
                        carrierXDeliveryFailure.ResponseJson = JsonConvert.SerializeObject(response);
                        carrierXDeliveryFailure.RequestType = (int)CarrierXDeliveryFailureRequestType.TPDAPI;
                        carrierXDeliveryFailure.FailureReason = JsonConvert.SerializeObject(withoutOrdermessages);
                        carrierXDeliveryFailure.BuyerCompanyId = carrierCustomerMappingsInfo.CarrierCustomerId;
                        carrierXDeliveryFailure.SupplierCompanyId = carrierCustomerMappingsInfo.CarrierCompanyId;
                        carrierXDeliveryFailure.IsEndSupplier = 1;
                        if (response.Messages.Any())
                        {
                            if (!string.IsNullOrEmpty(response.Messages.FirstOrDefault().EntityId))
                            {
                                carrierXDeliveryFailure.EntityId = Convert.ToInt32(response.Messages.FirstOrDefault().EntityId);
                            }
                        }
                        carrierXDeliveryFailure.CreatedDate = DateTime.Now;
                        Context.DataContext.CarrierXDeliveryFailures.Add(carrierXDeliveryFailure);
                        await Context.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("InvoiceTPDDomain", "SaveCarrierDeliveryFailure", ex.Message, ex);
            }
        }


        public int SetCarrierCompanyParameters(ApiResponseViewModel apiResonse, string apiRequestModelCustomerId, UserContext apiUserContext)
        {
            int result = 0;
            if (!string.IsNullOrWhiteSpace(apiRequestModelCustomerId))
            {
                try
                {
                    result = Context.DataContext.CarrierCustomerMappings
                                        .Where(t => t.CarrierAssignedCustomerId.ToLower().Equals(apiRequestModelCustomerId.ToLower()) && t.IsActive
                                                && t.CarrierCompanyId == apiUserContext.CompanyId)//carriercompanyId is createdby companyid
                                        .Select(t => t.CarrierCustomerId).SingleOrDefault();

                    if (result == 0)
                    {
                        result = Context.DataContext.Companies
                                        .Where(t => t.Name.ToLower().Equals(apiRequestModelCustomerId.ToLower()))
                                        .Select(t => t.Id).SingleOrDefault();

                        if (result == 0)
                        {
                            apiResonse.Messages.Add(new ApiCodeMessages()
                            { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "Customer ID") });
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceTPDDomain", "ValidateCarrierCompanyParameters", ex.Message, ex);

                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgMultipleCompaniesFound, apiRequestModelCustomerId) });
                }
            }
            return result;
        }

        private void ProcessAPI(ApiResponseViewModel response, UserContext apiUserContext, TPDInvoiceViewModel apiRequestModel)
        {
            try
            {
                if (apiUserContext != null && apiRequestModel != null && apiRequestModel.DropDetails.Any())
                {
                    var invoiceCreate = new InvoiceCreateDomain(this);
                    var domain = new ConsolidatedInvoiceDomain(invoiceCreate);

                    InvoiceViewModelNew invoiceViewModel = null;
                    int dropCount = 1;
                    int orderId = 0;
                    List<ApiCodeMessages> exceptionDdtMessages = new List<ApiCodeMessages>();
                    var allStates = Context.DataContext.MstStates.Select(t => t.Code.ToLower()).ToList();
                    if (allStates.Any())
                    {
                        // check unassigned ddt already exists for ExternalRefId
                        var apiInvoiceAlreadyExistsForRefId = Task.Run(() => CheckForDuplicateApiRequest(response, apiUserContext, apiRequestModel, exceptionDdtMessages)).Result;
                        if ((apiInvoiceAlreadyExistsForRefId && !string.IsNullOrWhiteSpace(apiRequestModel.ExternalRefID)))
                        {
                            var apiResponse = Task.Run(() => CreateUnassignedExceptionDdt(response, apiUserContext, apiRequestModel, exceptionDdtMessages, true)).Result;
                            return;
                        }

                        foreach (var dropInfo in apiRequestModel.DropDetails)
                        {
                            var mstProducts = new List<int>();
                            int? trackableScheduleFromCarrierOrderId = null;
                            int? orderIdFromCarrierOrderId = null;

                            mstProducts = ValidateProduct(response, apiUserContext, dropInfo, mstProducts, ref trackableScheduleFromCarrierOrderId, ref orderIdFromCarrierOrderId);

                            if (!response.Messages.Any())
                            {
                                ValidateLocationIdAndPoNumber(dropInfo, response, apiRequestModel);
                            }

                            var isDryRun = CheckIfDryRunInvoice(response, dropInfo, apiRequestModel);
                            if (isDryRun)
                            {
                                ValidateDryRunDetails(response, apiRequestModel);
                            }

                            if (!response.Messages.Any())
                            {
                                if (dropCount == 1)
                                {
                                    orderId = GetOrderIdForInvoiceApi(apiUserContext, response, dropInfo, apiRequestModel.LocationId, mstProducts
                                                        , trackableScheduleFromCarrierOrderId, orderIdFromCarrierOrderId, exceptionDdtMessages);

                                    if (orderId > 0 && !response.Messages.Any())
                                    {
                                        if (isDryRun && !response.Messages.Any())
                                        {
                                            //CREATE DRY RUN INVOICE
                                            ProcessApiForDryRun(response, apiUserContext, apiRequestModel, orderId, dropInfo);
                                            break;
                                        }

                                        if (!response.Messages.Any())
                                        {
                                            invoiceViewModel = Task.Run(() => invoiceCreate.GetPoDetailsToCreateInvoice(apiUserContext, orderId, trackableScheduleFromCarrierOrderId)).Result;
                                            if (invoiceViewModel != null)
                                            {
                                                SetHeaderParameters(invoiceViewModel, apiRequestModel, apiUserContext);

                                                if (invoiceViewModel != null)
                                                {
                                                    AddDriver(apiRequestModel, invoiceViewModel, apiUserContext);

                                                    var invDropModel = invoiceViewModel.Drops.FirstOrDefault(t => t.OrderId == orderId);
                                                    var invFeesModel = invoiceViewModel.Fees;
                                                    if (invDropModel != null)
                                                    {
                                                        ValidateDropDateParameter(invDropModel, dropInfo, response);
                                                        invDropModel.Assets.Clear(); // Asset list is now in get call, which is calling fee calc even if its not asset drop
                                                        ValidateFTLAndIsVariousParameters(allStates, dropInfo, response, apiRequestModel, invDropModel, invoiceViewModel.IsVariousOrigin);

                                                        if ((dropInfo.BolDetails != null && dropInfo.BolDetails.Any()) || (dropInfo.Tanks != null && dropInfo.Tanks.Any()) || (dropInfo.LiftDetails != null && dropInfo.LiftDetails.Any()))
                                                            ValidateBOLLiftAndTankDetails(allStates, response, apiRequestModel, dropInfo, apiUserContext, invoiceViewModel.IsBadgeMandatory);

                                                        if (!response.Messages.Any())
                                                            SetDropDetailsFromAPI(apiUserContext, apiRequestModel, invoiceViewModel, dropInfo, invDropModel, invFeesModel, response, false);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (invoiceViewModel != null && !response.Messages.Any() && dropCount > 1)
                                {
                                    orderId = GetOrderIdForInvoiceApi(apiUserContext, response, dropInfo, apiRequestModel.LocationId, mstProducts, trackableScheduleFromCarrierOrderId, orderIdFromCarrierOrderId);
                                    if (orderId > 0 && !response.Messages.Any())
                                    {
                                        var invDropModel = Task.Run(() => invoiceCreate.GetInvoiceDropModel(orderId)).Result;
                                        var invFeesModel = Task.Run(() => invoiceCreate.GetInvoiceDropFeesAsync(orderId)).Result;

                                        CheckForConsolidatedInvoice(invoiceViewModel, invDropModel, response);

                                        if (invoiceViewModel != null)
                                        {
                                            if (!response.Messages.Any())
                                            {
                                                if (invDropModel != null)
                                                {
                                                    ValidateDropDateParameter(invDropModel, dropInfo, response);
                                                    invDropModel.Assets.Clear(); // Asset list is now in get call, which is calling fee calc even if its not asset drop
                                                    ValidateFTLAndIsVariousParameters(allStates, dropInfo, response, apiRequestModel, invDropModel, invoiceViewModel.IsVariousOrigin);

                                                    if ((dropInfo.BolDetails != null && dropInfo.BolDetails.Any()) || (dropInfo.Tanks != null && dropInfo.Tanks.Any()) || (dropInfo.LiftDetails != null && dropInfo.LiftDetails.Any()))
                                                        ValidateBOLLiftAndTankDetails(allStates, response, apiRequestModel, dropInfo, apiUserContext, invoiceViewModel.IsBadgeMandatory);

                                                    if (!response.Messages.Any())
                                                        SetDropDetailsFromAPI(apiUserContext, apiRequestModel, invoiceViewModel, dropInfo, invDropModel, invFeesModel, response, true);

                                                    //check cancreateconsolidated inv check and create another invoiceview model
                                                }
                                            }
                                        }
                                    }
                                }

                                dropCount++;
                            }
                            if (response.Status == Status.Failed)
                            {
                                if (response.Messages.Any())
                                {
                                    response.Messages.ForEach(x => x.OrderId = dropInfo.OrderId);
                                }
                            }
                        }

                        if (invoiceViewModel != null && !response.Messages.Any() && !exceptionDdtMessages.Any())
                        {
                            try
                            {
                                //call invoice creation 
                                invoiceViewModel.CreationMethod = CreationMethod.APIUpload;
                                var createResponse = Task.Run(() => domain.CreateManualInvoice(apiUserContext, invoiceViewModel)).Result;
                                if (createResponse.StatusCode == Status.Success)
                                {
                                    response.Status = Status.Success;
                                    var codeMessages = createResponse.ResponseData;
                                    var jsonData = JsonConvert.SerializeObject(codeMessages);
                                    response.Messages.Add(JsonConvert.DeserializeObject<ApiCodeMessages>(jsonData));
                                }
                                else
                                {
                                    response.Status = Status.Failed;
                                    response.Messages.Add(new ApiCodeMessages()
                                    {
                                        Code = Constants.ApiCodeRS01,
                                        Message = createResponse.StatusMessage
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                LogManager.Logger.WriteException("InvoiceTPDDomain", "ProcessAPI", ex.Message, ex);
                                if (!response.Messages.Any())
                                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeTF01, Message = Resource.errMsgProcessRequestFailed });
                            }
                        }
                        else if (exceptionDdtMessages.Any() && !string.IsNullOrWhiteSpace(apiRequestModel.ExternalRefID))
                        {
                            // Create unassigned DDT with waiting for exception
                            var apiResponse = Task.Run(() => CreateUnassignedExceptionDdt(response, apiUserContext, apiRequestModel, exceptionDdtMessages, false)).Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceTPDDomain", "ProcessAPI", ex.Message, ex);
                if (!IsValidAddress)
                {
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = Resource.errMsgInvalidAddress });
                }
                else if (!response.Messages.Any())
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeTF01, Message = Resource.errMsgProcessRequestFailed });
            }
        }

        private void ValidateDropDateParameter(InvoiceDropViewModel invDropVM, TPDDropDetails dropInfo, ApiResponseViewModel apiResonse)
        {
            var minDropDate = invDropVM.MinDropDate;
            DateTime.TryParse(dropInfo.DropArrivalDate, out DateTime dropArrivalDate);
            if (dropArrivalDate.Date < minDropDate.Date)
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.ErrMsgDropDateCanNotBeLessThanOrderStartdate) });
            }
        }

        private List<int> ValidateProduct(ApiResponseViewModel response, UserContext apiUserContext, TPDDropDetails dropInfo
                                    , List<int> mstProducts, ref int? trackableScheduleId, ref int? orderIdFromCarrierOrderId)
        {
            // check if product found by product name
            if (!string.IsNullOrWhiteSpace(dropInfo.Product))
            {
                mstProducts = Task.Run(() => GetProductListByName(dropInfo.Product, apiUserContext)).Result;
                if (!mstProducts.Any())
                {
                    var errorMsg = new ApiCodeMessages() { Code = Constants.ApiCodeRQ06, Message = string.Format(Resource.errMsgApiCodeRQ06ProductNotFound, dropInfo.Product) };
                    response.Messages.Add(errorMsg);
                }
                else
                {
                    dropInfo.FuelTypeId = mstProducts.FirstOrDefault();
                }
            }

            if (!string.IsNullOrWhiteSpace(dropInfo.CarrierOrderID))
            {
                var deliverySch = Context.DataContext.DeliveryScheduleXTrackableSchedules
                                    .Where(t => t.Order.AcceptedCompanyId == apiUserContext.CompanyId && t.CarrierOrderId != null && t.CarrierOrderId != ""
                                                && t.OrderId > 0 && t.IsActive && t.CarrierOrderId.ToLower() == dropInfo.CarrierOrderID.ToLower())
                                    .Where(Extensions.IsOpenMissedTrackableSchedule())
                                        .Select(t => new { t.OrderId, t.Order.FuelRequest.FuelTypeId, t.Id, t.CarrierOrderId })
                                        .ToList();

                if (deliverySch.Count > 1 || !deliverySch.Any())
                {
                    response.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, $"{nameof(dropInfo.CarrierOrderID)} - {dropInfo.CarrierOrderID}") });
                }
                else
                {
                    var ord = deliverySch.FirstOrDefault();
                    dropInfo.FuelTypeId = ord.FuelTypeId;
                    trackableScheduleId = ord.Id; //ID is from above query which is TrackableScheduleId for invoice
                    orderIdFromCarrierOrderId = ord.OrderId;
                }
            }

            return mstProducts;
        }

        private async Task<StatusViewModel> CreateUnassignedExceptionDdt(ApiResponseViewModel response, UserContext apiUserContext, TPDInvoiceViewModel apiRequestModel, List<ApiCodeMessages> exceptionDdtMessages, bool isExistingDdt)
        {
            var apiResponse = new StatusViewModel();
            // check exception is enabled at supplier company 
            if (isExistingDdt)
            {
                apiResponse = await CreateConsolidatedUnassignedDdt(response, apiUserContext, apiRequestModel, exceptionDdtMessages);
            }
            else
            {
                var isExceptionEnabled = await CheckExceptionEnabledByType(apiUserContext.CompanyId, (int)ExceptionType.InvoiceApiException);
                if (isExceptionEnabled)
                {
                    apiResponse = await CreateConsolidatedUnassignedDdt(response, apiUserContext, apiRequestModel, exceptionDdtMessages);
                }
            }

            return apiResponse;
        }

        private async Task<StatusViewModel> CreateConsolidatedUnassignedDdt(ApiResponseViewModel response, UserContext apiUserContext, TPDInvoiceViewModel apiRequestModel, List<ApiCodeMessages> exceptionDdtMessages)
        {
            InvoiceViewModelNew invoiceViewModel = new InvoiceViewModelNew();
            var domain = new ConsolidatedInvoiceDomain();
            var result = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    List<InvoiceModel> invoices = new List<InvoiceModel>();

                    SetExceptionDropModelFromApiRequest(response, apiUserContext, apiRequestModel, invoiceViewModel);

                    if (invoiceViewModel.Drops.All(t => t.FuelTypeId > 0))
                    {
                        // generate invoice number
                        var invoiceNumber = await GenerateInvoiceNumber_New();

                        // format invoice number for ddt/invoice
                        invoiceViewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                        var waitingFor = WaitingAction.ExceptionApproval;
                        invoiceNumber.Number = invoiceNumber.Number.FormattedInvoiceNumber(invoiceViewModel.InvoiceTypeId, waitingFor);

                        foreach (var dropInfo in invoiceViewModel.Drops)
                        {
                            var invoice = await domain.GetUnassignedDdtInvoiceModel(apiUserContext, dropInfo, invoiceViewModel, apiRequestModel, invoiceNumber);
                            invoices.Add(invoice);
                        }

                        var invoiceHeader = GenerateInvoiceHeader(invoices);
                        foreach (var invoiceModel in invoices)
                        {
                            var invoice = invoiceModel.ToEntity();
                            //var assetDrops = invoiceModel.AssetDrops.Select(t => t.ToEntity()).ToList();
                            //invoice.AssetDrops = assetDrops;
                            invoiceHeader.Invoices.Add(invoice);
                            Context.Commit();
                            await SetInvoiceBolDetails(invoiceHeader, invoiceModel, invoice);
                            await SaveBulkPlantLocations(invoiceModel.BolDetails);

                            ApiCodeMessages codeMessage = new ApiCodeMessages();
                            codeMessage.EntityId = invoiceModel.DisplayInvoiceNumber;
                            if (exceptionDdtMessages.Any(t => t.Code != null && (t.Code == Constants.ApiCodeEV01 || t.Code == Constants.ApiCodeEV02)))
                            {
                                codeMessage.Code = exceptionDdtMessages.First().Code;
                                codeMessage.Message = string.Format(Resource.successMsgEV01EV02UnassignedDdtUpdatedSuccessfully, invoiceModel.DisplayInvoiceNumber, apiRequestModel.ExternalRefID);
                            }
                            else
                            {
                                codeMessage.Code = Constants.ApiCodeEV03;
                                codeMessage.Message = string.Format(Resource.successMsgEV03UnassignedDdtCreated, invoiceModel.DisplayInvoiceNumber);
                            }

                            result.ResponseData = codeMessage;
                            result.StatusCode = Status.Success;
                            result.StatusMessage = codeMessage.Message;
                            response.Messages = new List<ApiCodeMessages>();
                            response.Messages.Add(codeMessage);
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    ApiCodeMessages codeMessage = new ApiCodeMessages();
                    codeMessage.Code = Constants.ApiCodeTF04;
                    codeMessage.Message = Resource.errMsgProcessRequestFailedUnassignedDdt;
                    result.ResponseData = codeMessage;

                    LogManager.Logger.WriteException("InvoiceTPDDomain", "CreateConsolidatedUnassignedDdt", ex.Message, ex);
                    if (!response.Messages.Any())
                        response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeTF04, Message = Resource.errMsgProcessRequestFailedUnassignedDdt });
                }
            }

            return result;
        }

        public async Task<StatusViewModel> ApproveUnassignedExceptionDdt(int orderId, int exceptionDdtId, TPDInvoiceViewModel apiRequestModel)
        {
            var result = new StatusViewModel();
            var invoiceViewModel = new InvoiceViewModelNew();
            var invoiceCreate = new InvoiceCreateDomain(this);
            var consolidatedInvoiceDomain = new ConsolidatedInvoiceDomain(invoiceCreate);
            var authDomain = new AuthenticationDomain(invoiceCreate);
            var response = new ApiResponseViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.Where(t => t.Id == orderId && t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open))
                                                                .Select(t => new { t.Id, t.PoNumber, t.FuelRequest.FuelTypeId, t.FuelRequest.Job.TimeZoneName })
                                                                .SingleOrDefaultAsync();
                    var exceptionDdt = await Context.DataContext.Invoices.Where(t => t.Id == exceptionDdtId && t.InvoiceXAdditionalDetail.ExternalRefID == apiRequestModel.ExternalRefID)
                                                                         .FirstOrDefaultAsync();

                    if (order != null && exceptionDdt != null)
                    {
                        var userContext = await authDomain.GetUserContextAsync(exceptionDdt.CreatedBy);
                        var allStates = await Context.DataContext.MstStates.Select(t => t.Code.ToLower()).ToListAsync();
                        var dropInfo = apiRequestModel.DropDetails.Where(t => t.PONumber == exceptionDdt.PoNumber).FirstOrDefault();

                        // check api request product/fueltype and order fueltype match
                        var mstProducts = new List<int>();
                        mstProducts = await GetProductListByName(dropInfo.Product, userContext);
                        var isValidOrder = await ValidateApiRequestAndOrderForFueltype(orderId, dropInfo.Product, mstProducts, result);

                        if (isValidOrder)
                        {
                            // check dry run scenario
                            var isDryRun = CheckIfDryRunInvoice(response, dropInfo, apiRequestModel);
                            if (isDryRun && apiRequestModel.DropDetails.Count == 1)
                            {
                                //CREATE DRY RUN INVOICE
                                ProcessApiForDryRun(response, userContext, apiRequestModel, orderId, dropInfo);

                                // mark existing exception ddt as inactive
                                exceptionDdt.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;
                                Context.DataContext.Entry(exceptionDdt).State = EntityState.Modified;
                                await Context.CommitAsync();

                                transaction.Commit();

                                result.StatusCode = Status.Success;
                                result.StatusMessage = Resource.successMessageDryRunInvoiceCreated;
                            }
                            else
                            {
                                InvoiceDropViewModel invDropModel = null;
                                List<FeesViewModel> invFeesModel = null;
                                var firstDrop = apiRequestModel.DropDetails.FirstOrDefault();
                                if (firstDrop != null)
                                {
                                    invoiceViewModel = await invoiceCreate.GetPoDetailsToCreateInvoice(userContext, orderId);
                                    if (invoiceViewModel != null)
                                    {
                                        // set drop details
                                        invDropModel = invoiceViewModel?.Drops?.FirstOrDefault(t => t.OrderId == orderId);

                                        // set fees details
                                        var addDrop = firstDrop?.PONumber == exceptionDdt.PoNumber;
                                        if (addDrop)
                                        {
                                            invFeesModel = invoiceViewModel?.Fees;
                                        }
                                        else
                                        {
                                            invFeesModel = Task.Run(() => invoiceCreate.GetInvoiceDropFeesAsync(orderId)).Result;
                                        }

                                        //drop address
                                        SetHeaderParameters(invoiceViewModel, apiRequestModel, userContext);

                                        // set driver details
                                        AddDriver(apiRequestModel, invoiceViewModel, userContext);

                                        if (invDropModel != null)
                                        {
                                            if (apiRequestModel.DropDetails.Count > 1)
                                                await CheckConsolidatedInvoiceForExceptionDdt(invoiceViewModel, invDropModel, response);

                                            if (!response.Messages.Any())
                                            {
                                                invDropModel.Assets.Clear();
                                                ValidateDropAddressAndIsVariousParameters(allStates, dropInfo, response, apiRequestModel, invDropModel, invoiceViewModel.IsVariousOrigin);
                                            }

                                            if (!response.Messages.Any() && (dropInfo.Tanks != null && dropInfo.Tanks.Any()))
                                                ValidateTankDetails(response, dropInfo, apiRequestModel);
                                        }

                                        if (!response.Messages.Any())
                                        {
                                            var invoiceModels = new List<InvoiceModel>();
                                            var waitingFor = WaitingAction.Nothing;

                                            // set api drop details
                                            SetDropDetailsFromAPI(userContext, apiRequestModel, invoiceViewModel, dropInfo, invDropModel, invFeesModel, response, addDrop);

                                            // validate bol and lift details
                                            if (invDropModel != null && invDropModel.IsBolDetailsRequired)//invDropModel.IsFTL || 
                                            {
                                                if ((dropInfo.BolDetails != null && dropInfo.BolDetails.Any()) || (dropInfo.LiftDetails != null && dropInfo.LiftDetails.Any()))
                                                {
                                                    var bolLiftValidation = new ApiResponseViewModel();
                                                    ValidateBOLDetails(bolLiftValidation, dropInfo, userContext, invoiceViewModel.IsBadgeMandatory);
                                                    ValidateLiftDetails(bolLiftValidation, dropInfo, allStates, invoiceViewModel.IsBadgeMandatory, userContext);
                                                    if (bolLiftValidation.Messages.Any())
                                                    {
                                                        waitingFor = WaitingAction.BolDetails;
                                                    }
                                                }
                                                else
                                                {
                                                    waitingFor = WaitingAction.BolDetails;
                                                }
                                            }

                                            var isLastDrop = false;
                                            var totalDrops = apiRequestModel.DropDetails.Count;
                                            InvoiceHeaderDetail invoiceHeader = new InvoiceHeaderDetail();
                                            InvoiceNumber invoiceNumber = new InvoiceNumber();
                                            var approvedInvoices = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.ExternalRefID == invoiceViewModel.ExternalRefID &&
                                                                                t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive &&
                                                                                t.OrderId != null &&
                                                                                t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)InvoiceStatus.Unassigned)
                                                                  .OrderByDescending(t => t.Id)
                                                                  .Select(t => new { InvoiceId = t.Id, t.InvoiceHeader })
                                                                  .ToListAsync();
                                            if (approvedInvoices.Count == (totalDrops - 1))
                                            {
                                                isLastDrop = true;
                                            }

                                            if (!approvedInvoices.Any())
                                            {
                                                invoiceNumber = await GenerateInvoiceNumber_New();
                                            }
                                            else
                                            {
                                                invoiceNumber = approvedInvoices.FirstOrDefault().InvoiceHeader.InvoiceNumber;
                                            }

                                            // set model values
                                            invoiceViewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                                            invoiceViewModel.CreationMethod = CreationMethod.APIUpload;

                                            List<DropAdditionalDetailsModel> otherDetails = new List<DropAdditionalDetailsModel>();
                                            var priceRequestModels = new List<FuelPriceRequestModel>();
                                            DropAdditionalDetailsModel deliveryDetails = consolidatedInvoiceDomain.GetDropAdditionalDetails(orderId);
                                            otherDetails.Add(deliveryDetails);
                                            invDropModel.ParentFuelRequestId = deliveryDetails.ParentFuelRequestId;

                                            ConsolidatedInvoiceDomain.SetOrderTerminalAsPickupLocation(invoiceViewModel, invDropModel, deliveryDetails);
                                            var invoiceModel = await consolidatedInvoiceDomain.GetInvoiceModel(userContext, invoiceViewModel, invoiceModels, invoiceNumber, priceRequestModels, invDropModel, deliveryDetails);

                                            // check waiting bol
                                            if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                                            {
                                                invoiceModel.WaitingFor = waitingFor;
                                            }

                                            // set pricing
                                            await consolidatedInvoiceDomain.GetPriceDetails(priceRequestModels, invoiceModels);

                                            // set invoice fees
                                            consolidatedInvoiceDomain.SetInvoiceFees(invoiceViewModel, invoiceModels);
                                            consolidatedInvoiceDomain.SetCalculatedFees(invoiceViewModel, invoiceModels, true);

                                            // set statuses
                                            consolidatedInvoiceDomain.GetInvoiceStatus(invoiceModels);

                                            // generate new header for last drop
                                            if (!approvedInvoices.Any())
                                            {
                                                invoiceHeader = GenerateInvoiceHeader(invoiceModels);
                                                await Context.CommitAsync();
                                            }
                                            else
                                                invoiceHeader = approvedInvoices.FirstOrDefault().InvoiceHeader;

                                            foreach (var invoice in invoiceModels)
                                            {
                                                invoice.ParentId = exceptionDdt.Id;
                                                invoice.InvoiceHeaderId = invoiceHeader.Id;

                                                SetInvoiceNumber(invoiceNumber.Number, invoiceModel);
                                                var entity = invoiceModel.ToEntity();
                                                var assetDrops = invoice.AssetDrops.Select(t => t.ToEntity()).ToList();
                                                entity.AssetDrops = assetDrops;

                                                if (isLastDrop)
                                                    entity.InvoiceVersionStatusId = (int)InvoiceVersionStatus.Active;
                                                else
                                                    entity.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;

                                                invoiceHeader.Invoices.Add(entity);
                                                await Context.CommitAsync();

                                                await SetInvoiceBolDetails(invoiceHeader, invoiceModel, entity);
                                                await SaveBulkPlantLocations(invoiceModel.BolDetails);

                                                invoiceModel.Id = invoice.Id;
                                                invoiceModel.DisplayInvoiceNumber = invoice.DisplayInvoiceNumber;
                                                invoiceModel.InvoiceHeaderId = invoiceHeader.Id;

                                                // mark existing exception ddt as inactive
                                                //exceptionDdt.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId = (int)InvoiceStatus.ExceptionApproved;
                                                exceptionDdt.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;
                                                Context.DataContext.Entry(exceptionDdt).State = EntityState.Modified;

                                                await Context.CommitAsync();

                                                result.StatusCode = Status.Success;
                                                result.StatusMessage = Resource.errMessageApproveUnassignedDDTSuccess;
                                            }

                                            // update approved invoices for new invoice header and invoice number
                                            if (isLastDrop)
                                            {
                                                foreach (var ddt in approvedInvoices)
                                                {
                                                    var approveDdt = await Context.DataContext.Invoices.Where(t => t.Id == ddt.InvoiceId && t.InvoiceXAdditionalDetail.ExternalRefID == apiRequestModel.ExternalRefID)
                                                                                                       .FirstOrDefaultAsync();
                                                    approveDdt.InvoiceVersionStatusId = (int)InvoiceVersionStatus.Active;

                                                    //approveDdt.InvoiceHeaderId = invoiceHeader.Id;
                                                    //approveDdt.InvoiceHeader.InvoiceNumberId = invoiceNumber.Id;
                                                    //approveDdt.DisplayInvoiceNumber = invoiceModel.DisplayInvoiceNumber;
                                                    //approveDdt.TransactionId = invoiceModel.TransactionId;

                                                    //var bolDetails = await Context.DataContext.InvoiceXBolDetails.Where(t => t.InvoiceId == approveDdt.Id && t.InvoiceHeaderId == exceptionDdt.InvoiceHeaderId).ToListAsync();
                                                    //if (bolDetails != null && bolDetails.Any())
                                                    //{
                                                    //    List<InvoiceXBolDetail> bolList = new List<InvoiceXBolDetail>();
                                                    //    foreach (var bolDetail in bolDetails)
                                                    //    {
                                                    //        InvoiceXBolDetail invoiceXBol = new InvoiceXBolDetail() { InvoiceHeaderId = invoiceHeader.Id, BolDetailId = bolDetail.BolDetailId };
                                                    //        bolList.Add(invoiceXBol);
                                                    //    }
                                                    //    Context.DataContext.InvoiceXBolDetails.RemoveRange(bolDetails);
                                                    //    await Context.CommitAsync();

                                                    //    foreach (var invoiceXBol in bolList)
                                                    //    {
                                                    //        approveDdt.InvoiceXBolDetails.Add(invoiceXBol);
                                                    //    }
                                                    //    await Context.CommitAsync();
                                                    //}

                                                    Context.DataContext.Entry(approveDdt).State = EntityState.Modified;
                                                    await Context.CommitAsync();
                                                }
                                            }

                                            transaction.Commit();
                                        }
                                        else
                                        {
                                            result.StatusCode = Status.Failed;
                                            result.StatusMessage = response.Messages.FirstOrDefault()?.Message;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            result.StatusCode = Status.Failed;
                            result.StatusMessage = string.Format(Resource.errMsgProductForOrderAndApiDoesNotMatch, dropInfo.Product, order.PoNumber);
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceTPDDomain", "ApproveUnassignedExceptionDdt", ex.Message, ex);
                }
            }
            return result;
        }

        private async Task<bool> ValidateApiRequestAndOrderForFueltype(int orderId, string productName, List<int> mstProducts, StatusViewModel response)
        {
            var order = await Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new { t.FuelRequest.FuelTypeId, t.PoNumber }).FirstOrDefaultAsync();
            if (mstProducts.Any(id => id == order.FuelTypeId))
            {
                response.StatusCode = Status.Success;
                return true;
            }
            else
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.errMsgProductForOrderAndApiDoesNotMatch, productName, order.PoNumber);
                return false;
            }
        }

        private async Task CheckConsolidatedInvoiceForExceptionDdt(InvoiceViewModelNew invoiceViewModel, InvoiceDropViewModel invDropModel, ApiResponseViewModel apiResonse)
        {
            var invoices = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.ExternalRefID == invoiceViewModel.ExternalRefID &&
                                                                                t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active &&
                                                                                t.OrderId != null &&
                                                                                t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)InvoiceStatus.Unassigned)
                                                                  .OrderByDescending(t => t.Id)
                                                                  .Select(t => new { t.PoNumber, OrderId = t.Order.Id, t.Order.FuelRequest.FuelTypeId, t.Order.FuelRequest.JobId })
                                                                  .ToListAsync();
            if (invoices != null && invoices.Any())
            {
                if (invoices.Any(t => t.PoNumber == invDropModel.PoNumber))
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgUniqueParameterIsRequired, "PO Number") });
                }

                if (invoices.Any(t => t.FuelTypeId == invDropModel.FuelTypeId))
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgDupFuelTypeForConsolidatedInv, invDropModel.PoNumber) });
                }

                var jobId = Context.DataContext.Orders.Where(t => t.Id == invDropModel.OrderId).Select(t => t.FuelRequest.JobId).SingleOrDefault();
                if (invoices.Any(t => t.JobId != jobId))
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgAllPOShouldBeFromSameLocation, invDropModel.PoNumber) });
                }
            }
        }

        private void SetExceptionDropModelFromApiRequest(ApiResponseViewModel response, UserContext apiUserContext, TPDInvoiceViewModel apiRequestModel, InvoiceViewModelNew invoiceViewModel)
        {
            // set driver details
            if (!string.IsNullOrWhiteSpace(apiRequestModel.DriverFirstName) && !string.IsNullOrWhiteSpace(apiRequestModel.DriverLastName))
            {
                apiRequestModel.DriverId =
                    Task.Run(() => GetDriverIdForAPI(apiRequestModel.DriverFirstName, apiRequestModel.DriverLastName, apiRequestModel.DropDetails.First().PONumber, apiUserContext)).Result.Item1;
            }

            // set customer details
            if (invoiceViewModel.Customer == null)
                invoiceViewModel.Customer = new CustomerViewModel();
            invoiceViewModel.Customer.CompanyId = supplierCompanyIdForCarrier;
            invoiceViewModel.Customer.CompanyName = apiRequestModel.CustomerID;

            // set drop details
            foreach (var dropInfo in apiRequestModel.DropDetails)
            {
                var invDropViewModel = new InvoiceDropViewModel();

                // check for fueltype
                if (dropInfo.FuelTypeId == null || dropInfo.FuelTypeId <= 0)
                {
                    var mstProducts = new List<int>();
                    int? trackableScheduleFromCarrierOrderId = null;
                    int? orderIdFromCarrierOrderId = null;

                    mstProducts = ValidateProduct(response, apiUserContext, dropInfo, mstProducts, ref trackableScheduleFromCarrierOrderId, ref orderIdFromCarrierOrderId);
                }

                if (dropInfo.FuelTypeId != null && dropInfo.FuelTypeId > 0)
                {
                    // set drops
                    SetUnassignedDdtDropModelFromAPI(apiUserContext, invDropViewModel, dropInfo, apiRequestModel, invoiceViewModel);
                }
                else
                {
                    return;
                }
            }
        }

        private async Task<bool> CheckForDuplicateApiRequest(ApiResponseViewModel response, UserContext apiUserContext, TPDInvoiceViewModel apiRequestModel, List<ApiCodeMessages> exceptionDdtMessages)
        {
            var result = false;
            if (!string.IsNullOrWhiteSpace(apiRequestModel.ExternalRefID))
            {
                // check if request is made for same ExternalRefID, if already exists then update existing ddt with new details.
                var existingInvoices = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.ExternalRefID != null && t.InvoiceXAdditionalDetail.ExternalRefID.ToLower() == apiRequestModel.ExternalRefID.ToLower() &&
                                                                                     t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.Unassigned &&
                                                                                     t.WaitingFor == (int)WaitingAction.ExceptionApproval)
                                                                         .ToListAsync();

                if (existingInvoices != null && existingInvoices.Any())
                {
                    var errorMsg = new ApiCodeMessages() { Code = Constants.ApiCodeEV02, Message = string.Format(Resource.errMsgEV02UnassignedExceptionDdtAlreadyExists, apiRequestModel.ExternalRefID) };
                    exceptionDdtMessages.Add(errorMsg);
                    result = true;

                    var exceptionDomain = new ExceptionDomain(this);
                    // mark existing ddt to InActive
                    foreach (var invoice in existingInvoices)
                    {
                        invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;
                        Context.DataContext.Entry(invoice).State = EntityState.Modified;
                        await Context.CommitAsync();

                        var exceptionId = invoice.InvoiceExceptions.Where(t => t.InvoiceId == invoice.Id).Select(t => t.GeneratedExceptionId).OrderByDescending(id => id).FirstOrDefault();
                        var exceptionResponse = await exceptionDomain.ApproveException(new List<int> { exceptionId }, ExceptionResolution.DiscardDropTicket, (int)ExceptionStatus.Deleted);
                    }
                }
            }

            return result;
        }

        public async Task<bool> CheckExceptionEnabledByType(int companyId, int exceptionTypeId)
        {
            var response = false;
            try
            {
                var exceptionDomain = new ExceptionDomain();
                response = await exceptionDomain.IsExceptionEnabledByType(companyId, exceptionTypeId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceTPDDomain", "CheckExceptionEnabledByType", ex.Message, ex);
            }

            return response;
        }

        private void AddQueueEventForBrokerImageUpdate(UserContext userContext, TPDImagesToUpdate imagesToUpdate, ApiResponseViewModel apiResonse, int ddtId)
        {
            try
            {
                var jsonViewModel = new BrokerImageUpdateProcessorViewModel();
                jsonViewModel.ImagesToUpdate.AdditionalImage = imagesToUpdate.AdditionalImage;
                jsonViewModel.ImagesToUpdate.BolImage = imagesToUpdate.BolImage;
                jsonViewModel.ImagesToUpdate.DropImage = imagesToUpdate.DropImage;
                jsonViewModel.ImagesToUpdate.SignatureImage = imagesToUpdate.SignatureImage;
                jsonViewModel.ImagesToUpdate.BrokerChainId = imagesToUpdate.BrokerChainId;
                jsonViewModel.ImagesToUpdate.CanConverToInvoice = imagesToUpdate.CanConverToInvoice;
                jsonViewModel.UpdateFromDDTId = ddtId;
                jsonViewModel.ImagesToUpdate.ExternalRefID = imagesToUpdate.ExternalRefID;

                string json = JsonConvert.SerializeObject(jsonViewModel);

                var queueRequest = new QueueMessageViewModel()
                {
                    CreatedBy = userContext.Id,
                    QueueProcessType = QueueProcessType.BrokerInvoiceImageUpload,
                    JsonMessage = json
                };

                var queueDomain = new QueueMessageDomain();
                var queueId = queueDomain.EnqeueMessage(queueRequest);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceTPDDomain", "AddQueueEventForBrokerImageUpdate", ex.Message, ex);
                apiResonse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.errMsgProcessRequestFailed });
            }
        }

        private void ValidateDryRunDetails(ApiResponseViewModel apiResonse, TPDInvoiceViewModel apiRequestModel)
        {
            if (apiRequestModel.DropDryRunCount.GetValue<int>() <= 0 || apiRequestModel.DropDryRunFees.GetValue<decimal>() <= 0)
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.ErrMsgValueShouldBeGreaterThanZero) });
            }
        }

        public async Task<ApiResponseViewModel> ValidateAndProcessImages(TPDImageFileUploadViewModel viewModel, string token)
        {
            var response = new ApiResponseViewModel();
            try
            {
                if (viewModel != null)
                {
                    //get userdetails from token
                    var authDomain = new AuthenticationDomain(this);
                    var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
                    if (apiUserContext != null)
                    {
                        if (!string.IsNullOrWhiteSpace(viewModel.EntityId))
                        {
                            response = ProcessImageForEntityId(viewModel, response, apiUserContext);
                        }
                        else if (!string.IsNullOrWhiteSpace(viewModel.CustomerId) && !string.IsNullOrWhiteSpace(viewModel.CarrierOrderID))
                        {
                            response = ProcessImageForCustomerIdAndCarrierOrderId(viewModel, response, apiUserContext);
                        }
                        else
                        {
                            response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.errMsgProvideValidEntityIdOrCusterOrdId });
                        }
                    }
                    else
                        response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ01, Message = Resource.errMsgInvalidToken });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceTPDDomain", "ValidateAndProcessImages", ex.Message, ex);
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.errMsgMultipleDropTicketsFound });
            }
            return response;
        }

        private ApiResponseViewModel ProcessImageForCustomerIdAndCarrierOrderId(TPDImageFileUploadViewModel viewModel, ApiResponseViewModel response, UserContext apiUserContext)
        {
            supplierCompanyIdForCarrier = SetCarrierCompanyParameters(response, viewModel.CustomerId, apiUserContext);

            if (supplierCompanyIdForCarrier > 0 && !response.Messages.Any())
            {
                //var ddtToUpdate = GetDdtToUpdate(viewModel, apiUserContext, response);
                var ddtToUpdate = Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.CarrierOrderId.ToLower().Equals(viewModel.CarrierOrderID.ToLower())
                        && t.Order.BuyerCompanyId == supplierCompanyIdForCarrier
                        && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                        && (t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.APIUpload || t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.BulkUploaded)
                        && t.Order.AcceptedCompanyId == apiUserContext.CompanyId)
                        .Select(t => new
                        {
                            t.Id,
                            t.DisplayInvoiceNumber,
                            t.WaitingFor,
                            t.SupplierPreferredInvoiceTypeId,
                            t.InvoiceHeaderId,
                            t.OrderId,
                            t.IsDropImageReq,
                            t.Image,
                            t.IsBolImageReq,
                            t.InvoiceXBolDetails,
                            t.InvoiceXAdditionalDetail,
                            t.IsSignatureReq,
                            t.Signaure,
                            t.BrokeredChainId
                        })
                        .SingleOrDefault();

                if (ddtToUpdate != null && !response.Messages.Any())
                {
                    if (ddtToUpdate.WaitingFor == (int)WaitingAction.Images)
                    {
                        var isBolImgProvided = false;
                        var isDroppImgProvided = false;
                        var issiggImgProvided = false;
                        var canConvertToInvoice = true;

                        TPDImagesToUpdate imagesToUpdate = new TPDImagesToUpdate();
                        imagesToUpdate.BrokerChainId = ddtToUpdate.BrokeredChainId;
                        imagesToUpdate.ExternalRefID = viewModel.ExternalRefID;

                        if (ddtToUpdate.Image == null)
                        {
                            imagesToUpdate.DropImage = Task.Run(() => GetImageEntityModel(viewModel.DropFile, apiUserContext)).Result;
                            if (!string.IsNullOrEmpty(imagesToUpdate.DropImage?.FilePath))
                                isDroppImgProvided = true;
                        }

                        var ftlDetail = ddtToUpdate.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail).FirstOrDefault();
                        if (ftlDetail != null && ftlDetail.Image == null)
                        {
                            imagesToUpdate.BolImage = Task.Run(() => GetImageEntityModel(viewModel.BolFile, apiUserContext)).Result;
                            if (!string.IsNullOrEmpty(imagesToUpdate.BolImage?.FilePath))
                                isBolImgProvided = true;
                        }

                        if (ddtToUpdate.InvoiceXAdditionalDetail.AdditionalImage == null && viewModel.AdditionalFile != null)
                        {
                            imagesToUpdate.AdditionalImage = Task.Run(() => GetImageEntityModel(viewModel.AdditionalFile, apiUserContext)).Result;
                        }

                        if (ddtToUpdate.Signaure == null || ddtToUpdate.Signaure.Image == null)
                        {
                            imagesToUpdate.SignatureImage = Task.Run(() => GetImageEntityModel(viewModel.SignatureFile, apiUserContext)).Result;
                            if (!string.IsNullOrEmpty(imagesToUpdate.SignatureImage?.FilePath))
                                issiggImgProvided = true;
                        }

                        if (ddtToUpdate.IsBolImageReq && !isBolImgProvided)
                            canConvertToInvoice = false;

                        if (ddtToUpdate.IsSignatureReq && !issiggImgProvided)
                            canConvertToInvoice = false;

                        if (ddtToUpdate.IsDropImageReq && !isDroppImgProvided)
                            canConvertToInvoice = false;

                        if (imagesToUpdate.DropImage != null || imagesToUpdate.BolImage != null || imagesToUpdate.SignatureImage != null)
                        {
                            imagesToUpdate.CanConverToInvoice = canConvertToInvoice;
                            response = Task.Run(() => UpdateImagesToDDT(apiUserContext, response, ddtToUpdate.Id, ddtToUpdate.SupplierPreferredInvoiceTypeId ?? 2,
                                                                    ddtToUpdate.OrderId, ddtToUpdate.InvoiceXAdditionalDetail.SplitLoadChainId, imagesToUpdate)).Result;
                        }
                        else
                            response.Messages.Add(new ApiCodeMessages()
                            { Code = Constants.ApiCodeRQ01, Message = string.Format(Resource.errMsgDropTicketNotAllowedToEdit, ddtToUpdate.DisplayInvoiceNumber) });
                    }
                    else
                        response.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ01, Message = string.Format(Resource.errMsgDropTicketNotAllowedToEdit, ddtToUpdate.DisplayInvoiceNumber) });
                }
                else
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.errMsgMultipleDropTicketsFound });
            }

            return response;
        }

        private ApiResponseViewModel ProcessImageForEntityId(TPDImageFileUploadViewModel viewModel, ApiResponseViewModel response, UserContext apiUserContext)
        {
            //var ddtToUpdate = GetDdtToUpdate(viewModel, apiUserContext, response);
            var ddtToUpdate = Context.DataContext.Invoices.Where(t => t.DisplayInvoiceNumber.ToLower().Equals(viewModel.EntityId.ToLower())
                    && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                    && (t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.APIUpload || t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.BulkUploaded)
                    && t.Order.AcceptedCompanyId == apiUserContext.CompanyId)
                    .Select(t => new
                    {
                        t.Id,
                        t.DisplayInvoiceNumber,
                        t.WaitingFor,
                        t.SupplierPreferredInvoiceTypeId,
                        t.InvoiceHeaderId,
                        t.OrderId,
                        t.IsDropImageReq,
                        t.Image,
                        t.IsBolImageReq,
                        t.InvoiceXBolDetails,
                        t.InvoiceXAdditionalDetail,
                        t.IsSignatureReq,
                        t.Signaure,
                        t.BrokeredChainId
                    })
                    .SingleOrDefault();

            if (ddtToUpdate != null && !response.Messages.Any())
            {
                if (ddtToUpdate.WaitingFor == (int)WaitingAction.Images)
                {
                    var isBolImgProvided = false;
                    var isDroppImgProvided = false;
                    var issiggImgProvided = false;
                    var canConvertToInvoice = true;

                    TPDImagesToUpdate imagesToUpdate = new TPDImagesToUpdate();
                    imagesToUpdate.BrokerChainId = ddtToUpdate.BrokeredChainId;
                    imagesToUpdate.ExternalRefID = viewModel.ExternalRefID;

                    if (ddtToUpdate.Image == null)
                    {
                        imagesToUpdate.DropImage = Task.Run(() => GetImageEntityModel(viewModel.DropFile, apiUserContext)).Result;
                        if (!string.IsNullOrEmpty(imagesToUpdate.DropImage?.FilePath))
                            isDroppImgProvided = true;
                    }

                    var ftlDetail = ddtToUpdate.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail).FirstOrDefault();
                    if (ftlDetail != null && ftlDetail.Image == null)
                    {
                        imagesToUpdate.BolImage = Task.Run(() => GetImageEntityModel(viewModel.BolFile, apiUserContext)).Result;
                        if (!string.IsNullOrEmpty(imagesToUpdate.BolImage?.FilePath))
                            isBolImgProvided = true;
                    }

                    if (ddtToUpdate.InvoiceXAdditionalDetail.AdditionalImage == null && viewModel.AdditionalFile != null)
                    {
                        imagesToUpdate.AdditionalImage = Task.Run(() => GetImageEntityModel(viewModel.AdditionalFile, apiUserContext)).Result;
                    }

                    if (ddtToUpdate.Signaure == null || ddtToUpdate.Signaure.Image == null)
                    {
                        imagesToUpdate.SignatureImage = Task.Run(() => GetImageEntityModel(viewModel.SignatureFile, apiUserContext)).Result;
                        if (!string.IsNullOrEmpty(imagesToUpdate.SignatureImage?.FilePath))
                            issiggImgProvided = true;
                    }

                    if (ddtToUpdate.IsBolImageReq && !isBolImgProvided)
                        canConvertToInvoice = false;

                    if (ddtToUpdate.IsSignatureReq && !issiggImgProvided)
                        canConvertToInvoice = false;

                    if (ddtToUpdate.IsDropImageReq && !isDroppImgProvided)
                        canConvertToInvoice = false;

                    var isDropImgPresent = false;
                    var isBolImagePresent = false;
                    var isSignaturePresent = false;

                    if (ddtToUpdate.IsDropImageReq && (isDroppImgProvided || ddtToUpdate.Image != null))
                    {
                        isDropImgPresent = true;
                    }
                    if (ddtToUpdate.IsBolImageReq && ((ftlDetail != null && ftlDetail.Image != null) || isBolImgProvided))
                    {
                        isBolImagePresent = true;
                    }
                    if (ddtToUpdate.IsSignatureReq && (ddtToUpdate.Signaure != null || issiggImgProvided))
                    {
                        isSignaturePresent = true;
                    }
                    if (isDropImgPresent && isBolImagePresent && isSignaturePresent)
                    {
                        canConvertToInvoice = true;
                    }

                    if (imagesToUpdate.DropImage != null || imagesToUpdate.BolImage != null || imagesToUpdate.SignatureImage != null)
                    {
                        imagesToUpdate.CanConverToInvoice = canConvertToInvoice;
                        response = Task.Run(() => UpdateImagesToDDT(apiUserContext, response, ddtToUpdate.Id, ddtToUpdate.SupplierPreferredInvoiceTypeId ?? 2,
                                                                ddtToUpdate.OrderId, ddtToUpdate.InvoiceXAdditionalDetail.SplitLoadChainId, imagesToUpdate)).Result;
                    }
                    else
                        response.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ01, Message = string.Format(Resource.errMsgDropTicketNotAllowedToEdit, ddtToUpdate.DisplayInvoiceNumber) });
                }
                else
                    response.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ01, Message = string.Format(Resource.errMsgDropTicketNotAllowedToEdit, ddtToUpdate.DisplayInvoiceNumber) });
            }
            else
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.errMsgMultipleDropTicketsFound });
            return response;
        }

        private async Task<StatusViewModel> UpdateImageToDdtAndConvertToInvoice(UserContext apiUserContext, TPDImagesToUpdate imagesToUpdate, int ddtId, string splitLoadChainId, ApiResponseViewModel response)
        {
            if (ddtId > 0)
            {
                var invoiceDomain = new InvoiceDomain(this);
                var invoiceViewModel = await invoiceDomain.GetOriginalInvoiceDetails(ddtId);
                try
                {
                    if (imagesToUpdate.DropImage != null)
                    {
                        invoiceViewModel.InvoiceImage = imagesToUpdate.DropImage;
                    }

                    foreach (var bol in invoiceViewModel.BolDetails)
                    {
                        if (imagesToUpdate.BolImage != null)
                        {
                            bol.Images = imagesToUpdate.BolImage;
                        }

                    }
                    if (imagesToUpdate.SignatureImage != null)
                    {
                        invoiceViewModel.SignatureImage = imagesToUpdate.SignatureImage;
                    }
                    invoiceViewModel.AdditionalImage = imagesToUpdate.AdditionalImage;
                    invoiceViewModel.InspectionRequestVoucherImage = imagesToUpdate.InspectionVoucherImage;
                    invoiceViewModel.ExternalRefID = imagesToUpdate.ExternalRefID;

                    if (imagesToUpdate.CanConverToInvoice && string.IsNullOrWhiteSpace(splitLoadChainId))
                        CreateInvoiceFromDdtWaitingForImagesNew(ddtId, apiUserContext, response, invoiceViewModel, imagesToUpdate);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceTPDDomain", "UpdateImageToDdtAndConvertToInvoice", ex.Message, ex);
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.errMsgProcessRequestFailed });
                }
            }
            return null;
        }

        public void CreateInvoiceFromDdtWaitingForImagesNew(int ddtId, UserContext apiUserContext, ApiResponseViewModel apiResonse, InvoiceViewModelNew invoiceViewModel,
                                                         TPDImagesToUpdate imagesToUpdate)
        {
            var consolidatedDdtToInvoiceDomain = new ConsolidatedDdtToInvoiceDomain(this);
            if (ddtId > 0)
            {
                try
                {
                    var editStatus = consolidatedDdtToInvoiceDomain.ConvertDdtToInvoiceWithBolManually(apiUserContext, ddtId, invoiceViewModel).Result;
                    if (editStatus.StatusCode == Status.Success)
                    {
                        AddQueueEventForBrokerImageUpdate(apiUserContext, imagesToUpdate, apiResonse, ddtId);
                        apiResonse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS02, EntityId = editStatus.EntityNumber, Message = Resource.successUpdatedDDTWithImages });
                    }
                    else
                        apiResonse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, EntityId = editStatus.EntityNumber, Message = Resource.errMsgFailedToUpdateImage });
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceTPDDomain", "CreateInvoiceFromDdtWaitingForImagesNew", "TPD image processing failed", ex);
                    apiResonse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.errMsgProcessRequestFailed });
                }
            }
        }

        private async Task<ApiResponseViewModel> UpdateImagesToDDT(UserContext apiUserContext, ApiResponseViewModel apiResponse, int ddtId,
            int supPreferedInvType, int? ddtOrderId, string splitLoadChainId, TPDImagesToUpdate imagesToUpdate, bool isFromQueService = false)
        {
            try
            {
                if (IsDigitalDropTicket(supPreferedInvType) || !imagesToUpdate.CanConverToInvoice)
                {
                    //create new version with image
                    var invoiceEditDomain = new InvoiceEditDomain(this);

                    var editStatus = await invoiceEditDomain.DDTEditForImagesAsync(apiUserContext, ddtId, ddtOrderId, imagesToUpdate, imagesToUpdate.CanConverToInvoice ? WaitingAction.Nothing : WaitingAction.Images);
                    if (editStatus.StatusCode == Status.Success)
                    {
                        if (!string.IsNullOrWhiteSpace(imagesToUpdate.BrokerChainId))
                        {
                            //add image paths to broker invoices
                            if (!isFromQueService)
                                AddQueueEventForBrokerImageUpdate(apiUserContext, imagesToUpdate, apiResponse, ddtId);
                        }
                        apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS02, EntityId = editStatus.EntityNumber, Message = Resource.successUpdatedDDTWithImages });
                    }
                    else
                        apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, EntityId = editStatus.EntityNumber, Message = Resource.errMsgFailedToUpdateImage });
                }
                else
                {
                    await UpdateImageToDdtAndConvertToInvoice(apiUserContext, imagesToUpdate, ddtId, splitLoadChainId, apiResponse);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceTPDDomain", "UpdateImagesToDDT", ex.Message, ex);
                apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.errMsgProcessRequestFailed });
            }
            return apiResponse;
        }

        internal void ProcessBrokerImagesUpload(BrokerImageUpdateProcessorViewModel imageRequestViewModel, List<string> errorInfo)
        {
            StringBuilder processMessage = new StringBuilder();

            try
            {
                if (imageRequestViewModel != null && imageRequestViewModel.ImagesToUpdate != null)
                {
                    if (!string.IsNullOrWhiteSpace(imageRequestViewModel.ImagesToUpdate.BrokerChainId))
                    {
                        //Get brokerchain id ddt's wating for images
                        AuthenticationDomain authDomain = new AuthenticationDomain(this);
                        //get all invoices from unique headerid of broker chainid
                        var invoiceHeaders = Context.DataContext.Invoices.Where(t => t.BrokeredChainId == imageRequestViewModel.ImagesToUpdate.BrokerChainId
                                                && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                && t.Id != imageRequestViewModel.UpdateFromDDTId
                                                && (t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.APIUpload || t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.BulkUploaded))
                                                .SelectMany(t => t.InvoiceHeader.Invoices)
                                                .Select(t => new
                                                {
                                                    t.Id,
                                                    t.DisplayInvoiceNumber,
                                                    t.WaitingFor,
                                                    t.SupplierPreferredInvoiceTypeId,
                                                    t.InvoiceHeaderId,
                                                    t.OrderId,
                                                    t.IsDropImageReq,
                                                    t.Image,
                                                    t.IsBolImageReq,
                                                    t.InvoiceXBolDetails,
                                                    t.InvoiceXAdditionalDetail,
                                                    t.IsSignatureReq,
                                                    t.Signaure,
                                                    t.BrokeredChainId,
                                                    t.CreatedBy
                                                }).ToList();

                        foreach (var ddtToUpdate in invoiceHeaders)
                        {
                            if (ddtToUpdate.WaitingFor == (int)WaitingAction.Images)
                            {
                                var apiUserContext = Task.Run(() => authDomain.GetUserContextAsync(ddtToUpdate.CreatedBy)).Result;
                                var response = new ApiResponseViewModel();
                                response = Task.Run(() => UpdateImagesToDDT(apiUserContext, response, ddtToUpdate.Id, ddtToUpdate.SupplierPreferredInvoiceTypeId ?? 2,
                                                                            ddtToUpdate.OrderId, ddtToUpdate.InvoiceXAdditionalDetail.SplitLoadChainId, imageRequestViewModel.ImagesToUpdate, true)).Result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!(ex is QueueMessageFatalException))
                    LogManager.Logger.WriteException("InvoiceBulkUploadDomain", "ProcessBulkUploadJsonMessage", ex.Message, ex);
                if (processMessage.Length == 0)
                {
                    processMessage.Append(Constants.ErrorWhileProcessingBulkOrder);
                    errorInfo.Add(processMessage.ToString());
                }
                throw new QueueMessageFatalException(errorInfo[0], errorInfo);
            }
        }

        private static async Task<ImageViewModel> GetImageEntityModel(System.Web.HttpPostedFile imgFile, UserContext apiUserContext)
        {
            ImageViewModel imgViewModel = null;
            if (imgFile != null)
            {
                var bulkDomain = ContextFactory.Current.GetDomain<InvoiceBulkUploadDomain>();
                var fileName = bulkDomain.RemoveSpecialCharacters(imgFile.FileName);

                var result = await AzureStorageService.UploadImageToBlob(apiUserContext, imgFile.InputStream, fileName, BlobContainerType.InvoicePdfFiles);
                if (result.StatusCode == Status.Success)
                {
                    imgViewModel = new ImageViewModel();
                    imgViewModel.FilePath = result.StatusMessage;
                    imgViewModel.IsPdf = true;
                    return imgViewModel;
                }
            }
            return imgViewModel;
        }

        private Invoice GetDdtToUpdate(TPDImageFileUploadViewModel viewModel, UserContext apiUserContext, ApiResponseViewModel response)
        {
            try
            {
                var ddtWaitingForImg = Context.DataContext.Invoices.Where(t => t.DisplayInvoiceNumber.ToLower().Equals(viewModel.EntityId.ToLower())
                                    && t.IsActive
                                    && (t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.APIUpload || t.InvoiceXAdditionalDetail.CreationMethod == CreationMethod.BulkUploaded)
                                    && t.Order.AcceptedCompanyId == apiUserContext.CompanyId).SingleOrDefault();

                return ddtWaitingForImg;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceTPDDomain", "GetDdtToUpdate", ex.Message, ex);
                response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeTF01, Message = Resource.errMsgProcessRequestFailed });
            }
            return null;
        }

        private void CheckForConsolidatedInvoice(InvoiceViewModelNew invoiceViewModel, InvoiceDropViewModel invDropModel, ApiResponseViewModel apiResonse)
        {
            if (invoiceViewModel.Drops.Any(t => t.PoNumber == invDropModel.PoNumber))
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgUniqueParameterIsRequired, "PO Number") });
            }

            if (invoiceViewModel.Drops.Any(t => t.FuelTypeId == invDropModel.FuelTypeId))
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgDupFuelTypeForConsolidatedInv, invDropModel.PoNumber) });
            }

            var jobId = Context.DataContext.Orders.Where(t => t.Id == invDropModel.OrderId).Select(t => t.FuelRequest.JobId).SingleOrDefault();
            if (invoiceViewModel.Customer.Location.JobId != jobId)
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgAllPOShouldBeFromSameLocation, invDropModel.PoNumber) });
            }
        }

        private void AddDriver(TPDInvoiceViewModel apiRequestModel, InvoiceViewModelNew invoiceViewModel, UserContext apiUserContext)
        {
            if (!string.IsNullOrWhiteSpace(apiRequestModel.DriverFirstName) && !string.IsNullOrWhiteSpace(apiRequestModel.DriverLastName))
            {
                invoiceViewModel.Driver = new DropdownDisplayItem();
                invoiceViewModel.Driver.Id =
                    Task.Run(() => GetDriverIdForAPI(apiRequestModel.DriverFirstName, apiRequestModel.DriverLastName, invoiceViewModel.Drops.First().PoNumber, apiUserContext)).Result.Item1;
            }
        }

        public async Task<Tuple<int, string>> GetDriverIdForAPI(string firstName, string lastName, string poNumber, UserContext context,
                                                                string emailId = null, string phoneNumber = null)
        {
            var existingUser = Context.DataContext.Users.Where(t => t.CompanyId == context.CompanyId
                                        && t.FirstName.ToLower().Equals(firstName.ToLower().Trim())
                                        && t.LastName.ToLower().Equals(lastName.ToLower().Trim()))
                                        .Select(t => new { t.Id, t.FirstName, t.LastName })
                                        .FirstOrDefault();

            if (existingUser == null)
            {
                var roleId = new List<int> { (int)UserRoles.Driver };
                var drivers = new List<AdditionalUserViewModel>();
                var driver = new AdditionalUserViewModel()
                {
                    CompanyId = context.CompanyId,
                    FirstName = firstName.Trim(),
                    LastName = lastName.Trim(),
                    Email = string.IsNullOrWhiteSpace(emailId) ? $"{firstName.ToLower()}+{lastName.ToLower()}+{poNumber.ToLower()}@mailinator.com" : emailId,
                    RoleIds = roleId,
                    DisplayMode = PageDisplayMode.Create,
                    UpdatedDate = DateTimeOffset.Now,
                    IsInvitationSent = false
                };

                if (!string.IsNullOrWhiteSpace(phoneNumber))
                    driver.PhoneNumber = phoneNumber;

                drivers.Add(driver);
                var newdrivers = new AdditionalUsersViewModel() { UserId = context.Id, AdditionalUsers = drivers };
                var newUser = await new SettingsDomain().AddCompanyUser(newdrivers);
                var newDriver = newdrivers.AdditionalUsers.First();
                if (newDriver != null)
                    return Tuple.Create(newDriver.Id, $"{firstName.ToLower()} {lastName.ToLower()}");

            }
            if (existingUser != null)
            {
                return Tuple.Create(existingUser.Id, $"{existingUser.FirstName.ToLower()} {existingUser.LastName.ToLower()}");
            }
            return Tuple.Create(int.MinValue, $"{string.Empty} {string.Empty}");
        }

        private void ValidateBOLLiftAndTankDetails(List<string> allStates, ApiResponseViewModel apiResonse, TPDInvoiceViewModel apiRequestModel, TPDDropDetails dropInfo, UserContext apiUserContext, bool IsBadgeMandatory)
        {
            ValidateBOLDetails(apiResonse, dropInfo, apiUserContext, IsBadgeMandatory);

            ValidateLiftDetails(apiResonse, dropInfo, allStates, IsBadgeMandatory, apiUserContext);

            ValidateTankDetails(apiResonse, dropInfo, apiRequestModel);

            ValidateQuantities(apiResonse, dropInfo, apiRequestModel);
        }

        private void ValidateBOLDetails(ApiResponseViewModel apiResonse, TPDDropDetails dropInfo, UserContext apiUserContext, bool IsBadgeMandatory)
        {
            if (dropInfo.BolDetails != null && dropInfo.BolDetails.Any())
            {
                //validate boldetails
                foreach (var bol in dropInfo.BolDetails)
                {
                    if (string.IsNullOrWhiteSpace(bol.BolNumber))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(bol.BolNumber)) });

                    if (bol.BolNet <= 0)
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(bol.BolNet)) });

                    if (bol.BolGross <= 0)
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(bol.BolGross)) });

                    if (!string.IsNullOrWhiteSpace(bol.TerminalControl))
                    {
                        var terminal = GetTerminalIdFromControlNumber(bol.TerminalControl, apiUserContext);
                        if (terminal == 0)
                        {
                            apiResonse.Messages.Add(new ApiCodeMessages()
                            { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(bol.TerminalControl)) });
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(bol.BolNumber) && string.IsNullOrWhiteSpace(dropInfo.LoadingBadge) && IsBadgeMandatory)
                    {
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(dropInfo.LoadingBadge)) });
                    }
                }

                if (dropInfo.BolDetails.Where(t => t.BolNumber != null).Count()
                    != dropInfo.BolDetails.Where(t => t.BolNumber != null).Select(t => t.BolNumber.Trim()).Distinct().Count())
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgUniqueParameterIsRequired, "BolNumber") });
                }
            }
        }

        private void ValidateLiftDetails(ApiResponseViewModel apiResonse, TPDDropDetails dropInfo, List<string> allStates, bool IsBadgeMandatory, UserContext userContext)
        {
            if (dropInfo.LiftDetails != null && dropInfo.LiftDetails.Any())
            {
                if (dropInfo.LiftDetails.Where(t => t.LiftTicketNumber != null).Count()
                    != dropInfo.LiftDetails.Where(t => t.LiftTicketNumber != null).Select(t => t.LiftTicketNumber.Trim()).Distinct().Count())
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgUniqueParameterIsRequired, "LiftTicketNumber") });
                }

                //validate LiftDetails
                foreach (var lift in dropInfo.LiftDetails)
                {
                    if (string.IsNullOrWhiteSpace(lift.LiftTicketNumber))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.LiftTicketNumber)) });

                    if (!string.IsNullOrWhiteSpace(lift.LiftTicketCreationTime))
                        ValidateTimeParameter(nameof(lift.LiftTicketCreationTime), lift.LiftTicketCreationTime, apiResonse);

                    if (lift.LiftNet <= 0 && lift.LiftGross <= 0)
                    {
                        if (lift.LiftQuantity <= 0)
                        {
                            apiResonse.Messages.Add(new ApiCodeMessages()
                            { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errTPDApiLiftQuantityRequired, nameof(lift.LiftQuantity)) });
                        }
                    }
                    else if (lift.LiftNet <= 0)
                    {
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.LiftNet)) });
                    }
                    else if (lift.LiftGross <= 0)
                    {
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.LiftGross)) });
                    }

                    if (string.IsNullOrWhiteSpace(lift.LiftDate))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.LiftDate)) });
                    else
                    {
                        DateTime.TryParse(dropInfo.DropArrivalDate, out DateTime dropDate);
                        ValidateDate(nameof(lift.LiftDate), lift.LiftDate, apiResonse, dropDate);
                    }

                    bool isExistingBulkPlant = false;

                    if (string.IsNullOrWhiteSpace(lift.BulkPlantName))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.BulkPlantName)) });
                    else
                    {
                        //get existing bulk plant details
                        var bulkPlantdetails = ContextFactory.Current.GetDomain<DispatchDomain>().GetBulkPlantDetailsByName(lift.BulkPlantName, userContext.CompanyId);
                        //FALL BACK FOR MAPPING
                        if (bulkPlantdetails != null && bulkPlantdetails.SiteId == 0)
                        {
                            var bulkplantMapping = Context.DataContext.TerminalCompanyAliases.Where(t => t.IsActive
                                                        && t.CreatedByCompanyId == userContext.CompanyId
                                                    && t.AssignedTerminalId.ToLower() == lift.BulkPlantName.ToLower() && t.BulkPlantId != null)
                                        .Select(t => new
                                        {
                                            t.BulkPlantLocation.City,
                                            t.BulkPlantLocation.CountyName,
                                            t.BulkPlantLocation.Latitude,
                                            t.BulkPlantLocation.Longitude,
                                            t.BulkPlantLocation.StateCode,
                                            t.BulkPlantLocation.Address,
                                            t.BulkPlantLocation.ZipCode,
                                            t.BulkPlantLocation.CountryCode,
                                            t.BulkPlantLocation.Name
                                        })
                                        .FirstOrDefault();
                            if (bulkplantMapping != null)
                            {
                                isExistingBulkPlant = true;
                                //SET LIFT DETAILS FROM EXISTING RECROD
                                lift.LiftAddressCity = bulkplantMapping.City;
                                lift.LiftAddressCounty = bulkplantMapping.CountyName;
                                lift.LiftAddressLat = bulkplantMapping.Latitude.ToString();
                                lift.LiftAddressLong = bulkplantMapping.Longitude.ToString();
                                lift.LiftAddressState = bulkplantMapping.StateCode;
                                lift.LiftAddressStreet1 = bulkplantMapping.Address;
                                lift.LiftAddressZip = bulkplantMapping.ZipCode;
                                lift.BulkPlantName = bulkplantMapping.Name;
                            }
                        }

                        if (bulkPlantdetails != null && bulkPlantdetails.SiteId > 0 && !string.IsNullOrWhiteSpace(bulkPlantdetails.ZipCode))
                        {
                            isExistingBulkPlant = true;
                            //SET LIFT DETAILS FROM EXISTING RECROD
                            lift.LiftAddressCity = bulkPlantdetails.City;
                            lift.LiftAddressCounty = bulkPlantdetails.CountyName;
                            lift.LiftAddressLat = bulkPlantdetails.Latitude.ToString();
                            lift.LiftAddressLong = bulkPlantdetails.Longitude.ToString();
                            lift.LiftAddressState = bulkPlantdetails.State.Code;
                            lift.LiftAddressStreet1 = bulkPlantdetails.Address;
                            lift.LiftAddressZip = bulkPlantdetails.ZipCode;
                        }
                    }

                    if (!isExistingBulkPlant)
                    {

                        if (string.IsNullOrWhiteSpace(lift.LiftAddressLat) && string.IsNullOrWhiteSpace(lift.LiftAddressLong))
                        {
                            if (string.IsNullOrWhiteSpace(lift.LiftAddressStreet1))
                                apiResonse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.LiftAddressStreet1)) });

                            if (string.IsNullOrWhiteSpace(lift.LiftAddressCity))
                                apiResonse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.LiftAddressCity)) });

                            //if (IsvalidateCounty)
                            //{
                            //    if (string.IsNullOrWhiteSpace(lift.LiftAddressCounty))
                            //    apiResonse.Messages.Add(new ApiCodeMessages()
                            //        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.LiftAddressCounty)) });
                            //}

                            if (string.IsNullOrWhiteSpace(lift.LiftAddressZip))
                                apiResonse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.LiftAddressZip)) });

                            if (string.IsNullOrWhiteSpace(lift.LiftAddressState))
                                apiResonse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.LiftAddressState)) });
                            else
                            {
                                if (!allStates.Any(t => t == lift.LiftAddressState.ToLower()))
                                    apiResonse.Messages.Add(new ApiCodeMessages()
                                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterFormatIsInvalid, nameof(lift.LiftAddressState)) });
                            }

                        }
                        else
                        {
                            var isLatParse = Double.TryParse(lift.LiftAddressLat, out double lat);
                            var isLongParse = Double.TryParse(lift.LiftAddressLong, out double lang);
                            if (string.IsNullOrWhiteSpace(lift.LiftAddressLat) || lat == 0)
                                apiResonse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.LiftAddressLat)) });

                            if (string.IsNullOrWhiteSpace(lift.LiftAddressLong) || lang == 0)
                                apiResonse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(lift.LiftAddressLong)) });
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(lift.LiftTicketNumber) && string.IsNullOrWhiteSpace(dropInfo.LoadingBadge) && IsBadgeMandatory)
                    {
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(dropInfo.LoadingBadge)) });
                    }
                }
            }
        }

        private bool IsValidateCountyName(TPDLifts liftTicket)
        {
            //In api, for LiftTicket both zip and Lat-long are required hence the AND Condition
            if (!string.IsNullOrWhiteSpace(liftTicket.LiftAddressZip) && !string.IsNullOrWhiteSpace(liftTicket.LiftAddressLat) && !string.IsNullOrWhiteSpace(liftTicket.LiftAddressLong))
            {
                var geoAddress = GoogleApiDomain.GetGeocode(liftTicket.LiftAddressZip);
                if (geoAddress != null && !string.IsNullOrWhiteSpace(geoAddress.CountryCode))
                {
                    if ((geoAddress.CountryCode.ToLower() == "us") || geoAddress.CountryCode.ToLower() == "usa")
                    {
                        return true;
                    }
                    if ((geoAddress.CountryCode.ToLower() == "can") || (geoAddress.CountryCode.ToLower() == "ca"))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void ValidateTankDetails(ApiResponseViewModel apiResonse, TPDDropDetails dropInfo, TPDInvoiceViewModel tpdInvVM)
        {
            if (dropInfo.Tanks != null && dropInfo.Tanks.Any())
            {
                if (dropInfo.Tanks.Where(t => t.TankId != null).Count()
                    != dropInfo.Tanks.Where(t => t.TankId != null).Select(t => t.TankId.Trim()).Distinct().Count())
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgUniqueParameterIsRequired, "TankId") });
                }

                foreach (var tank in dropInfo.Tanks)
                {
                    if (string.IsNullOrWhiteSpace(tank.TankId))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "TankId") });

                    if (tank.DropQuantity <= 0)
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, $"DropQuantity for Tank {tank.TankId}") });

                    if (string.IsNullOrWhiteSpace(tank.EndTime))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, $"EndTime for Tank {tank.TankId}") });
                    else
                        ValidateTimeParameter("Tank EndTime", tank.EndTime, apiResonse);

                    if (string.IsNullOrWhiteSpace(tank.StartTime))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, $"StartTime for Tank {tank.TankId}") });
                    else
                        ValidateTimeParameter("Tank StartTime", tank.StartTime, apiResonse);

                    if ((tank.PreDip > 0 && tank.PostDip <= 0) || (tank.PostDip > 0 && tank.PreDip <= 0))
                    {
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, $"Pre-Post Dip Data") });
                    }
                }
            }
            else
            {
                if (dropInfo.TotalDropQuantity <= 0 && (string.IsNullOrWhiteSpace(tpdInvVM.DropDryRunCount) &&
                    string.IsNullOrWhiteSpace(tpdInvVM.DropDryRunFees)))
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "TotalDropQuantity") });
            }
        }


        private void ProcessApiForDryRun(ApiResponseViewModel response, UserContext apiUserContext, TPDInvoiceViewModel apiRequestModel, int orderId, TPDDropDetails dropInfo)
        {
            var invoiceDomain = new InvoiceDomain(this);
            var dryRunInvoiceViewModel = Task.Run(() => invoiceDomain.GetDryRunInvoiceAsync(orderId, apiUserContext.Id)).Result;
            SetDryRunViewModel(dryRunInvoiceViewModel, dropInfo, apiRequestModel, apiUserContext);

            var createResponse = Task.Run(() => invoiceDomain.CreateDryRunInvoiceAsync(dryRunInvoiceViewModel)).Result;
            if (createResponse.StatusCode == Status.Success)
            {
                response.Status = Status.Success;
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRS04,
                    Message = $"{createResponse.StatusMessage} - {createResponse.EntityNumber}"
                });
            }
            else
            {
                response.Status = Status.Failed;
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRS01,
                    Message = createResponse.StatusMessage
                });
            }
        }

        private void SetDryRunViewModel(DryRunInvoiceViewModel dryRunInvoiceViewModel, TPDDropDetails dropInfo, TPDInvoiceViewModel apiRequestModel, UserContext apiUserContext)
        {
            dryRunInvoiceViewModel.DryRunDate = dropInfo.DropArrivalDate;
            dryRunInvoiceViewModel.DeliveryTime = dropInfo.DropArrivalTime;
            dryRunInvoiceViewModel.UserId = apiUserContext.Id;
            dryRunInvoiceViewModel.SupplierInvoiceNumber = apiRequestModel.SupplierInvoiceNumber;
            dryRunInvoiceViewModel.CreationMethod = CreationMethod.APIUpload;
            //total dry run invoice amount =  total count * fees
            if (dryRunInvoiceViewModel.OrderEnforcement != OrderEnforcement.EnforceOrderLevelValues)
                dryRunInvoiceViewModel.DryRunFee = apiRequestModel.DropDryRunCount.GetValue<int>() * apiRequestModel.DropDryRunFees.GetValue<decimal>();
            else
                dryRunInvoiceViewModel.DryRunFee = apiRequestModel.DropDryRunCount.GetValue<int>() * dryRunInvoiceViewModel.DryRunFee;
        }

        private bool CheckIfDryRunInvoice(ApiResponseViewModel response, TPDDropDetails dropInfo, TPDInvoiceViewModel apiRequestModel)
        {
            if ((dropInfo.TotalDropQuantity > 0 || (dropInfo.Tanks != null && dropInfo.Tanks.Any(t => t.DropQuantity > 0)))
                && (!string.IsNullOrWhiteSpace(apiRequestModel.DropDryRunCount) || !string.IsNullOrWhiteSpace(apiRequestModel.DropDryRunFees)))
            {
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ05,
                    Message = string.Format(string.Format(Resource.errTPDFileDryRunFeeWithDropInfo, 1))
                });
                return true;
            }
            else if (!(dropInfo.TotalDropQuantity > 0 || (dropInfo.Tanks != null && dropInfo.Tanks.Any(t => t.DropQuantity > 0)))
                && (!string.IsNullOrWhiteSpace(apiRequestModel.DropDryRunCount) || !string.IsNullOrWhiteSpace(apiRequestModel.DropDryRunFees)))
            {
                return true;
            }

            return false;
        }

        private void ValidateLocationIdAndPoNumber(TPDDropDetails drop, ApiResponseViewModel apiResonse, TPDInvoiceViewModel apiRequestModel)
        {
            if (string.IsNullOrWhiteSpace(drop.PONumber) && string.IsNullOrWhiteSpace(drop.Product)
                    && string.IsNullOrWhiteSpace(apiRequestModel.LocationId) && string.IsNullOrWhiteSpace(drop.CarrierOrderID))
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(drop.PONumber)) });
            }

            if (string.IsNullOrWhiteSpace(drop.PONumber) && string.IsNullOrWhiteSpace(drop.CarrierOrderID) &&
                (string.IsNullOrWhiteSpace(drop.Product) || string.IsNullOrWhiteSpace(apiRequestModel.LocationId)))
            {
                if (string.IsNullOrWhiteSpace(drop.CarrierOrderID))
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "PO Number Or Carrier Order ID") });
                }
                else
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "PO Number Or LocationId - Product") });
                }
            }
        }

        private void SetDropDetailsFromAPI(UserContext apiUserContext, TPDInvoiceViewModel apiRequestModel, InvoiceViewModelNew invoiceViewModel,
                                            TPDDropDetails dropInfo, InvoiceDropViewModel invDropModel, List<FeesViewModel> invFeesModel, ApiResponseViewModel apiResponse, bool addDrop)
        {
            SetDropModelFromAPI(apiUserContext, invDropModel, dropInfo, apiRequestModel, invoiceViewModel, invFeesModel, addDrop, apiResponse);
            //set asset drop info            
            if (dropInfo.Tanks != null && dropInfo.Tanks.Any())
            {
                foreach (var tank in dropInfo.Tanks)
                {
                    var assetdrop = new AssetDropViewModel();
                    assetdrop.Id = 0;
                    assetdrop.AssetName = tank.TankId;
                    assetdrop.VehicleId = tank.TankId;
                    assetdrop.DropGallons = tank.DropQuantity;
                    assetdrop.StartTime = tank.StartTime;
                    assetdrop.EndTime = tank.EndTime;
                    assetdrop.PreDip = tank.PreDip;
                    assetdrop.PostDip = tank.PostDip;
                    assetdrop.Gravity = dropInfo.ApiGravity;
                    assetdrop.Id = GetAssetId(tank.TankId, invoiceViewModel.Customer.CompanyId);
                    assetdrop.AssetType = GetAssetType(assetdrop.Id);
                    invDropModel.Assets.Add(assetdrop);
                    invDropModel.IsAssetTracked = true;
                    if (assetdrop.AssetType == (int)AssetType.Tank)
                    {
                        ValidateTankCompatability(apiResponse, invDropModel, assetdrop.Id, assetdrop.AssetName);
                    }
                }
                invDropModel.ActualDropQuantity = invDropModel.Assets.Where(t => t.DropGallons.HasValue).Sum(t => t.DropGallons.Value);

            }
            else
            {
                if (dropInfo.OrderId != null && dropInfo.OrderId > 0)
                {
                    var order = Context.DataContext.Orders
                                 .Where(t => t.Id == dropInfo.OrderId && t.IsActive && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                 .Select(t => new
                                 {
                                     //  job = t.FuelRequest.Job,
                                     IsAssetTrackingEnabled = t.FuelRequest.Job.JobBudget.IsAssetTracked,
                                     //  fuelTypeId = t.FuelRequest.FuelTypeId,
                                     IsRetailJob = t.FuelRequest.Job.IsRetailJob,
                                     //   ProductTypeId = t.FuelRequest.MstProduct.MstProductType.Id,
                                     jobAssets = t.FuelRequest.Job.JobXAssets.Where(t1 => t1.RemovedBy == null && t1.RemovedDate == null).ToList()
                                 }).FirstOrDefault();

                    if (order != null && order.jobAssets != null && order.jobAssets.Any() && order.IsAssetTrackingEnabled)
                    {
                        var assetsOnLocation = order.jobAssets;
                        var productTypeId = invDropModel.TypeOfFuel;
                        List<int> assetIds = new List<int>();
                        assetsOnLocation.ForEach(t => assetIds.Add(t.AssetId));
                        if (order.IsRetailJob) // check for tanks 
                        {
                            var assets = Context.DataContext.Assets.Where(t => assetIds.Contains(t.Id) && t.FuelType == productTypeId && t.Type == (int)AssetType.Tank).ToList();
                            if (assets.Any() && assets.Count == 1)
                            {
                                var asset = assets.FirstOrDefault();
                                var assetdrop = new AssetDropViewModel();
                                assetdrop.Id = asset.Id;
                                assetdrop.AssetName = asset.Name;
                                assetdrop.VehicleId = asset.AssetAdditionalDetail.VehicleId;
                                assetdrop.DropGallons = dropInfo.TotalDropQuantity;
                                assetdrop.StartTime = dropInfo.DropArrivalTime;
                                assetdrop.EndTime = dropInfo.DropCompleteTime;
                                assetdrop.Gravity = dropInfo.ApiGravity;
                                invDropModel.Assets.Add(assetdrop);
                                invDropModel.IsAssetTracked = true;
                            }
                        }
                        else
                        {
                            var assets = Context.DataContext.Assets.Where(t => assetIds.Contains(t.Id) && t.FuelType == productTypeId && t.Type == (int)AssetType.Asset).ToList();
                            if (assets.Any() && assets.Count == 1)
                            {
                                var asset = assets.FirstOrDefault();
                                var assetdrop = new AssetDropViewModel();
                                assetdrop.Id = asset.Id;
                                assetdrop.AssetName = asset.Name;
                                assetdrop.VehicleId = asset.AssetAdditionalDetail.VehicleId;
                                assetdrop.DropGallons = dropInfo.TotalDropQuantity;
                                assetdrop.StartTime = dropInfo.DropArrivalTime;
                                assetdrop.EndTime = dropInfo.DropCompleteTime;
                                assetdrop.Gravity = dropInfo.ApiGravity;
                                invDropModel.Assets.Add(assetdrop);
                                invDropModel.IsAssetTracked = true;
                            }
                        }
                    }
                }
            }

            if (addDrop)
            {
                invoiceViewModel.Drops.Add(invDropModel);
            }
        }

        private void ValidateTankCompatability(ApiResponseViewModel apiResponse, InvoiceDropViewModel Drops, int assetId, string tankName)
        {
            var TankProductType = Context.DataContext.Assets
                            .Where(t => t.Id == assetId)
                            .Select(t => t.FuelType)
                            .FirstOrDefault();
            if (Drops.TypeOfFuel != TankProductType)
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgTankIsNotCompatible, tankName) });
            }
        }

        private int GetAssetType(int assetId)
        {
            var typeOfTank = Context.DataContext.Assets
                            .Where(t => t.Id == assetId)
                            .Select(t => t.Type)
                            .FirstOrDefault();
            return typeOfTank;
        }

        private void SetTankDetailsFromAPI(InvoiceViewModelNew invoiceViewModel, TPDDropDetails dropInfo, InvoiceDropViewModel invDropModel)
        {
            //set asset drop info
            if (dropInfo.Tanks != null && dropInfo.Tanks.Any())
            {
                foreach (var tank in dropInfo.Tanks)
                {
                    var assetdrop = new AssetDropViewModel();
                    assetdrop.Id = 0;
                    assetdrop.AssetName = tank.TankId;
                    assetdrop.VehicleId = tank.TankId;
                    assetdrop.DropGallons = tank.DropQuantity;
                    assetdrop.StartTime = tank.StartTime;
                    assetdrop.EndTime = tank.EndTime;
                    assetdrop.PreDip = tank.PreDip;
                    assetdrop.PostDip = tank.PostDip;
                    assetdrop.Id = GetAssetId(tank.TankId, invoiceViewModel.Customer.CompanyId);
                    invDropModel.Assets.Add(assetdrop);
                    invDropModel.IsAssetTracked = true;
                }
                invDropModel.ActualDropQuantity = invDropModel.Assets.Where(t => t.DropGallons.HasValue).Sum(t => t.DropGallons.Value);
                invoiceViewModel.Drops.Add(invDropModel);
            }
        }

        private int GetAssetId(string tankId, int buyerCompanyId)
        {
            var asset = Context.DataContext.Assets
                            .Where(t => (t.AssetAdditionalDetail.VehicleId.ToLower().Equals(tankId.ToLower())
                                    || t.Name.ToLower().Equals(tankId.ToLower()))
                                    && t.CompanyId == buyerCompanyId && t.IsActive)
                            .Select(t => t.Id)
                            .FirstOrDefault();
            return asset;
        }

        private void SetHeaderParameters(InvoiceViewModelNew invoiceViewModel, TPDInvoiceViewModel apiRequestModel, UserContext apiUserContext)
        {
            //drop address
            if (!string.IsNullOrWhiteSpace(apiRequestModel.DropZip) ||
                    (invoiceViewModel.IsVariousOrigin && !string.IsNullOrWhiteSpace(apiRequestModel.DropAddress1)))
            {
                invoiceViewModel.FuelDropLocation = new DropAddressViewModel();
                invoiceViewModel.FuelDropLocation.Address = $"{apiRequestModel.DropAddress1} {apiRequestModel.DropAddress2}";
                invoiceViewModel.FuelDropLocation.City = apiRequestModel.DropCity;
                invoiceViewModel.FuelDropLocation.State.Code = apiRequestModel.DropStateCode;

                var allstates = Context.DataContext.MstStates.Where(t => t.IsActive).Select(t => new { t.Id, t.Code, CountryCode = t.MstCountry.Code, t.CountryId, t.MstCountry.Name }).ToList();
                var stateDetails = allstates.FirstOrDefault(t => t.Code.ToLower().Trim().Equals(apiRequestModel.DropStateCode.ToLower()));
                if (stateDetails != null)
                {
                    invoiceViewModel.FuelDropLocation.State.Id = stateDetails.Id;
                    invoiceViewModel.FuelDropLocation.Country.Id = stateDetails.CountryId;
                    invoiceViewModel.FuelDropLocation.Country.Code = stateDetails.CountryCode;
                }
                invoiceViewModel.FuelDropLocation.ZipCode = apiRequestModel.DropZip;
            }

            invoiceViewModel.InvoiceNotes = apiRequestModel.Notes;
            invoiceViewModel.CreationMethod = CreationMethod.APIUpload;
            invoiceViewModel.IsRebillInvoice = false;
            invoiceViewModel.SupplierInvoiceNumber = apiRequestModel.SupplierInvoiceNumber;
            invoiceViewModel.ExternalRefID = apiRequestModel.ExternalRefID;
            invoiceViewModel.IsSiteOutOfFuel = !string.IsNullOrWhiteSpace(apiRequestModel.SiteOutOfFuel) ? true : false;
            invoiceViewModel.OutOfFuelProduct = apiRequestModel.OutOfFuelProduct;
        }

        private void SetDropModelFromAPI(UserContext apiUserContext, InvoiceDropViewModel invDropModel, TPDDropDetails dropInfo,
                            TPDInvoiceViewModel apiRequestModel, InvoiceViewModelNew invoiceViewModel, List<FeesViewModel> invFeesModel, bool addDrop, ApiResponseViewModel apiResponse)
        {
            DateTime.TryParse(dropInfo.DropArrivalDate, out DateTime newDate);
            invDropModel.DropDate = newDate; //set drop end date here from API
            if (!string.IsNullOrWhiteSpace(dropInfo.DropCompleteDate) && DateTime.TryParse(dropInfo.DropCompleteDate, out DateTime newEndDate))
            {
                invDropModel.DropEndDate = newEndDate;
            }
            invDropModel.StartTime = dropInfo.DropArrivalTime;
            invDropModel.EndTime = dropInfo.DropCompleteTime;
            invDropModel.ActualDropQuantity = dropInfo.TotalDropQuantity;
            invDropModel.DropTicketNumber = dropInfo.DropTicketNumber;
            invDropModel.CarrierOrderId = string.IsNullOrWhiteSpace(dropInfo.CarrierOrderID) ? apiRequestModel.CarrierOrderID : dropInfo.CarrierOrderID;
            invDropModel.CarrierOrder = dropInfo.CarrierOrder;
            invDropModel.Gravity = dropInfo.ApiGravity;

            if (!string.IsNullOrWhiteSpace(dropInfo.OrderDate))
            {
                DateTime ordDate;
                DateTime.TryParse(dropInfo.OrderDate, out ordDate);
                invDropModel.OrderDate = ordDate;
            }

            invDropModel.OrderQuantity = dropInfo.OrderQuantity;
            invDropModel.Tractor = dropInfo.Tractor;
            invDropModel.Truck = dropInfo.Truck;
            invDropModel.LoadingBadge = dropInfo.LoadingBadge;

            if (!string.IsNullOrWhiteSpace(dropInfo.FuelCost))
                invDropModel.SupplierFuelCost = dropInfo.FuelCost.GetValue<decimal>();

            if (invDropModel.OtherTaxDetails != null && invDropModel.OtherTaxDetails.Any())
                invoiceViewModel.OtherProductTaxes.AddRange(invDropModel.OtherTaxDetails);

            var order = Context.DataContext.Orders.
                                Where(t => t.IsActive && t.Id == invDropModel.OrderId).
                                Select(t => new
                                {
                                    IsFTL = t.IsFTL,
                                    Id = t.Id,
                                    Currency = t.FuelRequest.Currency,
                                    UoM = t.FuelRequest.UoM,
                                    PoNumber = t.PoNumber,
                                    ProductName = t.FuelRequest.MstProduct.Name,
                                    ProductId = t.FuelRequest.FuelTypeId,
                                    TypeOfFuel = t.FuelRequest.MstProduct.ProductTypeId,
                                    t.FuelRequest.FuelRequestFees,
                                    t.OrderAdditionalDetail,
                                    t.AcceptedCompanyId,
                                    t.BuyerCompanyId,
                                    t.TerminalId,
                                    TerminalName = t.MstExternalTerminal.Name,
                                    t.FuelRequest.FuelRequestDetail.OrderEnforcementId,
                                    t.OrderAdditionalDetail.FreightPricingMethod,
                                    BulkPlantId = t.FuelDispatchLocations.Where(t2 => t2.IsActive && t2.LocationType == (int)LocationType.PickUp && t2.TrackableScheduleId != null).Select(t3 => t3.BulkPlantId).FirstOrDefault()
                                }).FirstOrDefault();

            if (!addDrop)
                _firstOrderEnforcement = order.OrderEnforcementId;

            if (addDrop)
            {
                if (_firstOrderEnforcement != order.OrderEnforcementId)
                {
                    canCreateConsolidateInvoice = false;
                }
            }

            //Freight Fee
            SetFuelSurchargeFeeNew(order.PoNumber, invoiceViewModel, order, invDropModel, dropInfo, apiUserContext.CompanyId, apiResponse);

            //Fees
            if (order.OrderEnforcementId != OrderEnforcement.EnforceOrderLevelValues)
            {
                if (order.OrderEnforcementId == OrderEnforcement.ManageException)
                {
                    //manage exception
                }
                else if (order.OrderEnforcementId == OrderEnforcement.NoEnforcement)
                {
                    //no enforcement is only for API
                }

                if (!addDrop)
                {
                    invoiceViewModel.Fees.RemoveAll(t => t.FeeTypeId != ((int)FeeType.ProcessingFee).ToString() && t.FeeTypeId != ((int)FeeType.SurchargeFreightFee).ToString() && t.FeeTypeId != ((int)FeeType.FreightCost).ToString());
                    SetFeesFromCsvFileNew(apiRequestModel, invoiceViewModel, order.IsFTL, order.Currency, order.UoM);
                }
            }
            else
            {
                if (addDrop)
                {
                    //invoiceViewModel.Fees.AddRange(invFeesModel);
                    var dedupFees = ConsolidatedInvoiceDomain.DeDuplicateFees(invFeesModel, invoiceViewModel.Fees);
                    invoiceViewModel.Fees = dedupFees;
                }
            }
            decimal totalDropQuantity = dropInfo.TotalDropQuantity;

            //Bol details
            if (dropInfo.BolDetails != null)
            {
                var bolDetails = new List<InvoiceBolViewModel>();

                foreach (var bolFromAPI in dropInfo.BolDetails)
                {
                    if (!string.IsNullOrWhiteSpace(bolFromAPI.BolNumber))
                    {
                        List<BolProductViewModel> lstBolProducts = new List<BolProductViewModel>();

                        var bolProduct = new BolProductViewModel();
                        var bolDetail = new InvoiceBolViewModel();
                        var liftTicketDetail = new InvoiceLiftTicketViewModel();

                        bolDetail.BolNumber = bolFromAPI.BolNumber;
                        bolDetail.BadgeNumber = dropInfo.LoadingBadge;
                        bolDetail.LiftStartTime = Convert.ToString(GetTimeSpan(bolFromAPI.LiftStartTime));
                        bolDetail.LiftEndTime = Convert.ToString(GetTimeSpan(bolFromAPI.LiftEndTime));
                        bolProduct.NetQuantity = bolFromAPI.BolNet;
                        bolProduct.GrossQuantity = bolFromAPI.BolGross;
                        bolProduct.ProductId = order.ProductId;
                        bolProduct.ProductName = order.ProductName;
                        invoiceViewModel.Carrier = string.IsNullOrWhiteSpace(bolFromAPI.BolCarrier) ? apiUserContext.CompanyName : bolFromAPI.BolCarrier;

                        liftTicketDetail.BolCreationTime = string.IsNullOrWhiteSpace(bolFromAPI.BolCreationTime)
                                            ? GetTimeSpan(DateTimeOffset.Now.GetTimeInHhMmFormat()) : GetTimeSpan(bolFromAPI.BolCreationTime);

                        bolDetail.LiftDate = invDropModel.DropDate;
                        //setting up delivered quantity
                        totalDropQuantity = SetBolDeliveredQuantity(bolFromAPI.BolDelivered, bolProduct, totalDropQuantity);

                        // set terminal details
                        if (!string.IsNullOrWhiteSpace(bolFromAPI.TerminalControl))
                        {
                            var terminalDetails = Context.DataContext.MstExternalTerminals.
                                                    Where(t => t.IsActive && t.ControlNumber != null && t.ControlNumber != string.Empty
                                                    && (t.ControlNumber.ToLower() == bolFromAPI.TerminalControl.ToLower() || t.Name.ToLower() == bolFromAPI.TerminalControl.ToLower()))
                                                    .Select(t => new { TerminalId = t.Id, TerminalName = t.Name })
                                                    .FirstOrDefault();

                            if (terminalDetails == null)
                            {
                                terminalDetails = Context.DataContext.TerminalCompanyAliases.Where(t => t.CreatedByCompanyId == apiUserContext.CompanyId && t.TerminalId != null
                                                    && t.IsActive && t.TerminalSupplierId == null && t.AssignedTerminalId.ToLower().Equals(bolFromAPI.TerminalControl.ToLower()))
                                                    .Select(t => new { TerminalId = t.TerminalId.Value, TerminalName = t.MstExternalTerminal.Name })
                                                                        .FirstOrDefault();
                            }

                            if (terminalDetails != null)
                            {
                                bolProduct.TerminalId = terminalDetails.TerminalId;
                                bolProduct.TerminalName = terminalDetails.TerminalName;
                            }
                            else
                            {
                                bolProduct.TerminalId = order.TerminalId;
                                bolProduct.TerminalName = order.TerminalName;
                            }
                        }
                        else
                        {
                            bolProduct.TerminalId = order.TerminalId;
                            bolProduct.TerminalName = order.TerminalName;
                        }

                        lstBolProducts.Add(bolProduct);
                        bolDetail.Products = lstBolProducts;
                        bolDetails.Add(bolDetail);
                    }
                }

                invoiceViewModel.BolDetails.AddRange(bolDetails);
            }

            if (dropInfo.LiftDetails != null)
            {
                var liftTicketDetails = new List<InvoiceLiftTicketViewModel>();

                foreach (var liftDetails in dropInfo.LiftDetails)
                {
                    var liftTicketProduct = new LiftProductViewModel();
                    var liftTicketDetail = new InvoiceLiftTicketViewModel();

                    //Lift Details
                    if (!string.IsNullOrWhiteSpace(liftDetails.LiftTicketNumber))
                    {
                        List<LiftProductViewModel> lstLiftTikcetProducts = new List<LiftProductViewModel>();
                        liftTicketDetail.LiftTicketNumber = liftDetails.LiftTicketNumber;
                        liftTicketDetail.BadgeNumber = dropInfo.LoadingBadge;
                        liftTicketDetail.BolCreationTime = string.IsNullOrWhiteSpace(liftDetails.LiftTicketCreationTime) ? liftTicketDetail.BolCreationTime : GetTimeSpan(liftDetails.LiftTicketCreationTime);
                        liftTicketProduct.LiftQuantity = liftDetails.LiftQuantity;
                        liftTicketProduct.NetQuantity = liftDetails.LiftNet;
                        liftTicketProduct.GrossQuantity = liftDetails.LiftGross;

                        liftTicketDetail.LiftArrivalTime = GetTimeSpan(liftDetails.LiftArrivalTime);
                        liftTicketDetail.LiftStartTime = liftDetails.LiftStartTime;
                        liftTicketDetail.LiftEndTime = liftDetails.LiftEndTime;
                        liftTicketProduct.ProductId = order.ProductId;
                        liftTicketProduct.ProductName = order.ProductName;

                        invoiceViewModel.Carrier = string.IsNullOrWhiteSpace(liftDetails.LiftCarrier) ? apiUserContext.CompanyName : liftDetails.LiftCarrier;
                        if (totalDropQuantity > 0 && liftTicketProduct.NetQuantity > 0)
                        {
                            totalDropQuantity = SetLiftDeliveredQuantity(liftDetails.LiftDelivered, liftTicketProduct, totalDropQuantity);
                        }
                        if (!string.IsNullOrWhiteSpace(liftDetails.LiftDate))
                        {
                            DateTime liftDate;
                            DateTime.TryParse(liftDetails.LiftDate, out liftDate);
                            liftTicketDetail.LiftDate = liftDate;
                        }

                        //lift address
                        if (!string.IsNullOrWhiteSpace(liftDetails.LiftAddressZip))
                        {
                            var point = GoogleApiDomain.GetGeocode($"{liftDetails.LiftAddressZip}");
                            if (point == null)
                            {
                                //SOMETIMES FROM ZIP NOT GETTING GEOCODE
                                point = GoogleApiDomain.GetGeocode($"{liftDetails.LiftAddressStreet1} {liftDetails.LiftAddressStreet2} {liftDetails.LiftAddressCity} {liftDetails.LiftAddressState} {liftDetails.LiftAddressCounty} {liftDetails.LiftAddressZip}");
                            }

                            var isLatParse = Double.TryParse(liftDetails.LiftAddressLat, out double lat);
                            var isLongParse = Double.TryParse(liftDetails.LiftAddressLong, out double lang);

                            if (point != null)
                            {
                                var allstates = Context.DataContext.MstStates.Where(t => t.IsActive).Select(t => new { t.Id, t.Code, CountryCode = t.MstCountry.Code, t.CountryId, t.MstCountry.Name }).ToList();
                                liftTicketProduct.Address = new DropAddressViewModel();
                                liftTicketProduct.BulkPlantName = liftDetails.BulkPlantName;
                                liftTicketProduct.Address.Address = $"{liftDetails.LiftAddressStreet1} {liftDetails.LiftAddressStreet2}";
                                liftTicketProduct.Address.City = point.City;
                                liftTicketProduct.Address.CountyName = point.CountyName;
                                liftTicketProduct.Address.State.Code = point.StateCode;
                                liftTicketProduct.Address.ZipCode = point.ZipCode;
                                liftTicketProduct.Address.Latitude = (Convert.ToDecimal(lat) == 0) ? Convert.ToDecimal(point.Latitude) : Convert.ToDecimal(lat);
                                liftTicketProduct.Address.Longitude = (Convert.ToDecimal(lang) == 0) ? Convert.ToDecimal(point.Longitude) : Convert.ToDecimal(lang);
                                var stateDetails = allstates.FirstOrDefault(t => t.Code.ToLower().Trim().Equals(point.StateCode.ToLower()));
                                if (stateDetails != null)
                                {
                                    liftTicketProduct.Address.State.Id = stateDetails.Id;
                                    liftTicketProduct.Address.Country.Id = stateDetails.CountryId;
                                    liftTicketProduct.Address.Country.Code = stateDetails.CountryCode;
                                    liftTicketProduct.Address.Country.Name = stateDetails.Name;
                                }
                                else
                                    break;
                            }
                            else if (isLatParse && isLongParse)
                            {
                                var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(liftDetails.LiftAddressLat), Convert.ToDouble(liftDetails.LiftAddressLong));
                                if (geoAddress == null || geoAddress.ZipCode == null || geoAddress.StateCode == null)
                                {
                                    IsValidAddress = false;
                                    throw new Exception();
                                }
                                liftTicketProduct.Address = new DropAddressViewModel();
                                liftTicketProduct.Address.ZipCode = geoAddress.ZipCode.ToString();
                                liftTicketProduct.Address.Latitude = Convert.ToDecimal(lat);
                                liftTicketProduct.Address.Longitude = Convert.ToDecimal(lang);
                                liftTicketProduct.BulkPlantName = liftDetails.BulkPlantName;
                                liftTicketProduct.Address.Address = geoAddress.FormattedAddress.ToString();
                                liftTicketProduct.Address.City = geoAddress.City;
                                liftTicketProduct.Address.CountyName = geoAddress.CountyName;
                                liftTicketProduct.Address.State.Code = geoAddress.StateCode;
                                var stateDetails = Context.DataContext.MstStates.FirstOrDefault(t => t.Code.ToLower().Trim().Equals(geoAddress.StateCode.ToLower()));
                                if (stateDetails != null)
                                {
                                    liftTicketProduct.Address.State.Id = stateDetails.Id;
                                    liftTicketProduct.Address.Country.Id = stateDetails.CountryId;
                                    liftTicketProduct.Address.Country.Code = geoAddress.CountryCode;
                                    liftTicketProduct.Address.Country.Name = stateDetails.Name;
                                }
                                else
                                    break;


                            }
                            else
                            {
                                IsValidAddress = false;
                                throw new Exception();

                            }

                            liftTicketProduct.Address.IsAddressAvailable = true;
                        }
                        else
                        {
                            //Only Lat and Long provided by user in lift details object
                            var isLatParse = Double.TryParse(liftDetails.LiftAddressLat, out double lat);
                            var isLongParse = Double.TryParse(liftDetails.LiftAddressLong, out double lang);
                            var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(liftDetails.LiftAddressLat), Convert.ToDouble(liftDetails.LiftAddressLong));
                            if (geoAddress == null || geoAddress.ZipCode == null || geoAddress.StateCode == null)
                            {
                                IsValidAddress = false;
                                throw new Exception();
                            }
                            liftTicketProduct.Address = new DropAddressViewModel();

                            liftTicketProduct.Address.ZipCode = geoAddress.ZipCode.ToString();
                            liftTicketProduct.Address.Latitude = Convert.ToDecimal(lat);
                            liftTicketProduct.Address.Longitude = Convert.ToDecimal(lang);
                            liftTicketProduct.BulkPlantName = liftDetails.BulkPlantName;
                            liftTicketProduct.Address.Address = geoAddress.FormattedAddress.ToString();
                            liftTicketProduct.Address.City = geoAddress.City;
                            liftTicketProduct.Address.CountyName = geoAddress.CountyName;
                            liftTicketProduct.Address.State.Code = geoAddress.StateCode;
                            var stateDetails = Context.DataContext.MstStates.FirstOrDefault(t => t.Code.ToLower().Trim().Equals(geoAddress.StateCode.ToLower()));
                            if (stateDetails != null)
                            {
                                liftTicketProduct.Address.State.Id = stateDetails.Id;
                                liftTicketProduct.Address.Country.Id = stateDetails.CountryId;
                                liftTicketProduct.Address.Country.Code = geoAddress.CountryCode;
                                liftTicketProduct.Address.Country.Name = stateDetails.Name;
                            }
                            else
                                break;
                            liftTicketProduct.Address.IsAddressAvailable = true;
                        }

                        lstLiftTikcetProducts.Add(liftTicketProduct);
                        liftTicketDetail.Products = lstLiftTikcetProducts;
                        liftTicketDetails.Add(liftTicketDetail);
                    }
                }
                invoiceViewModel.TicketDetails.AddRange(liftTicketDetails);
            }
        }
        private static decimal SetBolDeliveredQuantity(decimal deliveredQuantity, BolProductViewModel bolProduct, decimal totalDropQuantity)
        {
            if (bolProduct.NetQuantity > 0)
            {
                decimal deliveredQty = deliveredQuantity;
                if (deliveredQty <= 0)
                {
                    decimal pickupQty = bolProduct.NetQuantity > bolProduct.GrossQuantity ? bolProduct.NetQuantity.Value : bolProduct.GrossQuantity ?? 0;
                    deliveredQty = totalDropQuantity <= pickupQty ? totalDropQuantity : pickupQty;
                }
                totalDropQuantity = totalDropQuantity - deliveredQty;
                bolProduct.DeliveredQuantity = deliveredQty;
            }
            return totalDropQuantity;
        }

        private static decimal SetLiftDeliveredQuantity(decimal deliveredQuantity, LiftProductViewModel bolProduct, decimal totalDropQuantity)
        {
            if (bolProduct.NetQuantity > 0)
            {
                decimal deliveredQty = deliveredQuantity;
                if (deliveredQty <= 0)
                {
                    decimal pickupQty = bolProduct.NetQuantity > bolProduct.GrossQuantity ? bolProduct.NetQuantity.Value : bolProduct.GrossQuantity ?? 0;
                    deliveredQty = totalDropQuantity <= pickupQty ? totalDropQuantity : pickupQty;
                }
                totalDropQuantity = totalDropQuantity - deliveredQty;
                bolProduct.DeliveredQuantity = deliveredQty;
            }

            return totalDropQuantity;
        }

        private void SetUnassignedDdtDropModelFromAPI(UserContext apiUserContext, InvoiceDropViewModel invDropModel, TPDDropDetails dropInfo,
                                                      TPDInvoiceViewModel apiRequestModel, InvoiceViewModelNew invoiceViewModel)
        {
            DateTime.TryParse(dropInfo.DropArrivalDate, out DateTime newDate);
            invDropModel.DropDate = newDate; //SET DROPEND DATE HERE 
            if (!string.IsNullOrWhiteSpace(dropInfo.DropCompleteDate) && DateTime.TryParse(dropInfo.DropCompleteDate, out DateTime newEndDate))
            {
                invDropModel.DropEndDate = newEndDate;
            }
            invDropModel.StartTime = dropInfo.DropArrivalTime;
            invDropModel.EndTime = dropInfo.DropCompleteTime;
            invDropModel.ActualDropQuantity = dropInfo.TotalDropQuantity;
            invDropModel.DropTicketNumber = dropInfo.DropTicketNumber;
            invDropModel.CarrierOrderId = string.IsNullOrWhiteSpace(dropInfo.CarrierOrderID) ? apiRequestModel.CarrierOrderID : dropInfo.CarrierOrderID;
            invDropModel.CarrierOrder = dropInfo.CarrierOrder;
            invDropModel.PoNumber = dropInfo.PONumber;
            invDropModel.FuelTypeName = dropInfo.Product;
            invDropModel.FuelTypeId = dropInfo.FuelTypeId ?? 0;

            if (!string.IsNullOrWhiteSpace(dropInfo.OrderDate))
            {
                DateTime ordDate;
                DateTime.TryParse(dropInfo.OrderDate, out ordDate);
                invDropModel.OrderDate = ordDate;
            }

            invDropModel.OrderQuantity = dropInfo.OrderQuantity;
            invDropModel.Tractor = dropInfo.Tractor;
            invDropModel.Truck = dropInfo.Truck;
            invDropModel.LoadingBadge = dropInfo.LoadingBadge;

            if (!string.IsNullOrWhiteSpace(dropInfo.FuelCost))
                invDropModel.SupplierFuelCost = dropInfo.FuelCost.GetValue<decimal>();

            // add drop model
            invoiceViewModel.Drops.Add(invDropModel);

            //Bol details
            SetBolDetailsFromApi(apiUserContext, invDropModel, dropInfo, invoiceViewModel, invDropModel.FuelTypeId, dropInfo.Product, null, null);

            // set lift details
            SetLiftDetailsFromApi(dropInfo, invoiceViewModel, invDropModel.FuelTypeId, dropInfo.Product);
        }

        private void SetLiftDetailsFromApi(TPDDropDetails dropInfo, InvoiceViewModelNew invoiceViewModel, int? productId, string productName)
        {
            if (dropInfo.LiftDetails != null)
            {
                var liftTicketDetails = new List<InvoiceLiftTicketViewModel>();

                foreach (var liftDetails in dropInfo.LiftDetails)
                {
                    var liftTicketProduct = new LiftProductViewModel();
                    var liftTicketDetail = new InvoiceLiftTicketViewModel();

                    //Lift Details
                    if (!string.IsNullOrWhiteSpace(liftDetails.LiftTicketNumber))
                    {
                        List<LiftProductViewModel> lstLiftTikcetProducts = new List<LiftProductViewModel>();
                        liftTicketDetail.LiftTicketNumber = liftDetails.LiftTicketNumber;
                        liftTicketDetail.BadgeNumber = dropInfo.LoadingBadge;
                        liftTicketDetail.BolCreationTime = string.IsNullOrWhiteSpace(liftDetails.LiftTicketCreationTime) ? liftTicketDetail.BolCreationTime : GetTimeSpan(liftDetails.LiftTicketCreationTime);
                        liftTicketProduct.LiftQuantity = liftDetails.LiftQuantity;
                        liftTicketProduct.NetQuantity = liftDetails.LiftNet;
                        liftTicketProduct.GrossQuantity = liftDetails.LiftGross;
                        liftTicketProduct.DeliveredQuantity = liftDetails.LiftDelivered;
                        liftTicketDetail.LiftArrivalTime = GetTimeSpan(liftDetails.LiftArrivalTime);
                        liftTicketProduct.LiftStartTime = GetTimeSpan(liftDetails.LiftStartTime);
                        liftTicketProduct.LiftEndTime = GetTimeSpan(liftDetails.LiftEndTime);
                        liftTicketProduct.ProductId = productId ?? 0;
                        liftTicketProduct.ProductName = productName;

                        if (!string.IsNullOrWhiteSpace(liftDetails.LiftDate))
                        {
                            DateTime liftDate;
                            DateTime.TryParse(liftDetails.LiftDate, out liftDate);
                            liftTicketDetail.LiftDate = liftDate;
                        }

                        //lift address
                        if (!string.IsNullOrWhiteSpace(liftDetails.LiftAddressZip))
                        {
                            var allstates = Context.DataContext.MstStates.Where(t => t.IsActive).Select(t => new { t.Id, t.Code, CountryCode = t.MstCountry.Code, t.CountryId, t.MstCountry.Name }).ToList();
                            liftTicketProduct.Address = new DropAddressViewModel();
                            liftTicketProduct.BulkPlantName = liftDetails.BulkPlantName;
                            liftTicketProduct.Address.Address = $"{liftDetails.LiftAddressStreet1} {liftDetails.LiftAddressStreet2}";
                            liftTicketProduct.Address.City = liftDetails.LiftAddressCity;
                            liftTicketProduct.Address.CountyName = liftDetails.LiftAddressCounty;
                            liftTicketProduct.Address.State.Code = liftDetails.LiftAddressState;
                            var stateDetails = allstates.FirstOrDefault(t => t.Code.ToLower().Trim().Equals(liftDetails.LiftAddressState.ToLower()));
                            if (stateDetails != null)
                            {
                                liftTicketProduct.Address.State.Id = stateDetails.Id;
                                liftTicketProduct.Address.Country.Id = stateDetails.CountryId;
                                liftTicketProduct.Address.Country.Code = stateDetails.CountryCode;
                                liftTicketProduct.Address.Country.Name = stateDetails.Name;
                            }
                            else
                                break;

                            liftTicketProduct.Address.ZipCode = liftDetails.LiftAddressZip;
                            liftTicketProduct.Address.Latitude = liftDetails.LiftAddressLat.GetValue<decimal>();
                            liftTicketProduct.Address.Longitude = liftDetails.LiftAddressLong.GetValue<decimal>();
                            liftTicketProduct.Address.IsAddressAvailable = true;
                        }

                        lstLiftTikcetProducts.Add(liftTicketProduct);
                        liftTicketDetail.Products = lstLiftTikcetProducts;
                        liftTicketDetails.Add(liftTicketDetail);
                    }
                }
                invoiceViewModel.TicketDetails.AddRange(liftTicketDetails);
            }
        }

        private void SetBolDetailsFromApi(UserContext apiUserContext, InvoiceDropViewModel invDropModel, TPDDropDetails dropInfo, InvoiceViewModelNew invoiceViewModel,
                                          int? productId, string productName, int? terminalId, string terminalName)
        {
            if (dropInfo.BolDetails != null)
            {
                var bolDetails = new List<InvoiceBolViewModel>();

                foreach (var bolFromAPI in dropInfo.BolDetails)
                {
                    if (!string.IsNullOrWhiteSpace(bolFromAPI.BolNumber))
                    {
                        List<BolProductViewModel> lstBolProducts = new List<BolProductViewModel>();

                        var bolProduct = new BolProductViewModel();
                        var bolDetail = new InvoiceBolViewModel();

                        bolDetail.BolNumber = bolFromAPI.BolNumber;
                        bolDetail.BadgeNumber = dropInfo.LoadingBadge;
                        bolDetail.LiftStartTime = bolFromAPI.LiftStartTime;
                        bolDetail.LiftEndTime = bolFromAPI.LiftEndTime;
                        bolProduct.NetQuantity = bolFromAPI.BolNet;
                        bolProduct.GrossQuantity = bolFromAPI.BolGross;
                        bolProduct.DeliveredQuantity = bolFromAPI.BolDelivered;
                        bolProduct.ProductId = productId ?? 0;
                        bolProduct.ProductName = productName;
                        bolDetail.LiftDate = invDropModel.DropDate;

                        invoiceViewModel.Carrier = string.IsNullOrWhiteSpace(bolFromAPI.BolCarrier) ? apiUserContext.CompanyName : bolFromAPI.BolCarrier;

                        // set terminal details
                        if (!string.IsNullOrWhiteSpace(bolFromAPI.TerminalControl))
                        {
                            var terminalDetails = Context.DataContext.MstExternalTerminals.
                                                    Where(t => t.IsActive && t.ControlNumber != null && t.ControlNumber != string.Empty
                                                    && (t.ControlNumber.ToLower() == bolFromAPI.TerminalControl.ToLower() || t.Name.ToLower() == bolFromAPI.TerminalControl.ToLower()))
                                                    .Select(t => new { TerminalId = t.Id, TerminalName = t.Name })
                                                    .FirstOrDefault();

                            if (terminalDetails == null)
                            {
                                terminalDetails = Context.DataContext.TerminalCompanyAliases.Where(t => t.CreatedByCompanyId == apiUserContext.CompanyId && t.TerminalId != null
                                                    && t.IsActive && t.TerminalSupplierId == null && t.AssignedTerminalId.ToLower().Equals(bolFromAPI.TerminalControl.ToLower()))
                                                    .Select(t => new { TerminalId = t.TerminalId.Value, TerminalName = t.MstExternalTerminal.Name })
                                                                        .FirstOrDefault();
                            }

                            if (terminalDetails != null)
                            {
                                bolProduct.TerminalId = terminalDetails.TerminalId;
                                bolProduct.TerminalName = terminalDetails.TerminalName;
                            }
                            else
                            {
                                bolProduct.TerminalId = terminalId;
                                bolProduct.TerminalName = terminalName;
                            }
                        }
                        else
                        {
                            bolProduct.TerminalId = terminalId;
                            bolProduct.TerminalName = terminalName;
                        }

                        lstBolProducts.Add(bolProduct);
                        bolDetail.Products = lstBolProducts;
                        bolDetails.Add(bolDetail);
                    }
                }

                invoiceViewModel.BolDetails.AddRange(bolDetails);
            }
        }

        private void SetFeesFromCsvFileNew(TPDInvoiceViewModel apiRequestModel, InvoiceViewModelNew manualInvoiceViewModel, bool isFtl, Currency currency, UoM uoM)
        {
            //DemurrageOther fee
            var feeFromApi = apiRequestModel.DropDemurrageFees.GetValue<decimal>();
            var demurrageTime = apiRequestModel.DropDemurrageTime.GetValue<decimal>();
            if (feeFromApi > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.DemurrageOther).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromApi;
                    feeObjFromOrder.FeeSubQuantity = demurrageTime; //to save value in Hr
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.DemurrageOther, feeFromApi, manualInvoiceViewModel,
                            isFtl, currency, uoM, demurrageTime * 60);
                }
            }

            //WetHoseFee fee
            feeFromApi = apiRequestModel.DropWethoseFees.GetValue<decimal>();
            if (feeFromApi > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.WetHoseFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromApi;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.WetHoseFee, feeFromApi, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //FreightFee fee
            feeFromApi = apiRequestModel.DropFreightFees.GetValue<decimal>();
            if (feeFromApi > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.FreightFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromApi;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.FreightFee, feeFromApi, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //LoadFee fee
            feeFromApi = apiRequestModel.DropLoadFees.GetValue<decimal>();
            if (feeFromApi > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.LoadFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromApi;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.LoadFee, feeFromApi, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //Envrironmental fee
            feeFromApi = apiRequestModel.DropEnvironmentalFees.GetValue<decimal>();
            if (feeFromApi > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.EnvironmentalFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromApi;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.EnvironmentalFee, feeFromApi, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //ServiceFee fee
            feeFromApi = apiRequestModel.DropServiceFees.GetValue<decimal>();
            if (feeFromApi > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.ServiceFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromApi;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.ServiceFee, feeFromApi, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //OverWaterFee fee
            feeFromApi = apiRequestModel.DropOverWaterFees.GetValue<decimal>();
            if (feeFromApi > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.OverWaterFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromApi;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.OverWaterFee, feeFromApi, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //SurchargeFee fee
            feeFromApi = apiRequestModel.DropSurchargeFees.GetValue<decimal>();
            if (feeFromApi > 0)
            {
                var feeObjFromOrder = manualInvoiceViewModel.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.SurchargeFee).ToString()));
                if (feeObjFromOrder != null)
                {
                    feeObjFromOrder.FeeSubTypeId = (int)FeeSubType.FlatFee;
                    feeObjFromOrder.Fee = feeFromApi;
                }
                else
                {
                    AddFuelReqFeeViewModelNew((int)FeeType.SurchargeFee, feeFromApi, manualInvoiceViewModel, isFtl, currency, uoM);
                }
            }

            //OtherFee fee
            feeFromApi = apiRequestModel.DropOtherFees.GetValue<decimal>();
            if (feeFromApi > 0)
            {
                AddFuelReqFeeViewModelNew((int)FeeType.OtherFee, feeFromApi, manualInvoiceViewModel, isFtl, currency, uoM);
            }
        }

        private void AddFuelReqFeeViewModelNew(int feeType, decimal totalFee, InvoiceViewModelNew manualInvoiceViewModel,
                bool isFTL, Currency currency, UoM uoM, decimal? feeSubQuantity = null)
        {
            int feeSubType = (int)FeeSubType.FlatFee;

            var feeViewModel = new FeesViewModel()
            {
                FeeTypeId = feeType.ToString(),
                FeeSubTypeId = feeSubType,
                Fee = totalFee,
                FeeSubQuantity = feeSubQuantity,
                TruckLoadType = isFTL ? (int)TruckLoadTypes.FullTruckLoad : (int)TruckLoadTypes.LessTruckLoad,
                CommonFee = true,
                Currency = currency,
                UoM = uoM,
            };

            if (feeType == (int)FeeType.OtherFee)
            {
                feeViewModel.CommonFee = false;
                feeViewModel.OtherFeeDescription = "bulk other fee";
            }

            manualInvoiceViewModel.Fees.Add(feeViewModel);
        }

        private void SetFuelSurchargeFeeNew(string poNumber, InvoiceViewModelNew invoiceViewModel, dynamic order, InvoiceDropViewModel drop, TPDDropDetails tpdDrop, int CompanyId, ApiResponseViewModel apiResponse)
        {
            InvoiceCreateDomain invoiceCreateDomain = new InvoiceCreateDomain(this);
            var isDropDetails = invoiceViewModel.Drops.FirstOrDefault(t => t.PoNumber == poNumber.Trim());
            if (isDropDetails == null)
            {
                drop.FuelSurchargeFreightFee = invoiceCreateDomain.GetFuelSurchargeDetails(order.FuelRequestFees, order.OrderAdditionalDetail,
                                                            order.TypeOfFuel, order.AcceptedCompanyId, order.BuyerCompanyId);
            }
            var fscObj = drop.FuelSurchargeFreightFee;
            if (fscObj != null)
            {
                if (order.FreightPricingMethod == FreightPricingMethod.Manual)
                {
                    if (fscObj.IsFeeByDistance && fscObj.DeliveryFeeByQuantity.Any())
                    {
                        var exactFee = fscObj.DeliveryFeeByQuantity.FirstOrDefault();
                        if (exactFee != null)
                            fscObj.TotalFuelSurchargeFee = GetFuelSurchageFrieghtFee(fscObj.SurchargePercentage, exactFee.Fee, drop.ActualDropQuantity);
                    }
                    else
                    {
                        fscObj.TotalFuelSurchargeFee = GetFuelSurchageFrieghtFee(fscObj.SurchargePercentage, fscObj.SurchargeFreightCost, drop.ActualDropQuantity);
                    }
                }
                else if (order.FreightPricingMethod == FreightPricingMethod.Auto)
                {
                    bool applyFreightRate = false;
                    bool applyFuelSurcharge = false;
                    FreightRateInputViewModel frinput = new FreightRateInputViewModel();
                    frinput.OrderId = order.Id;
                    frinput.SupplierId = CompanyId;
                    frinput.TerminalId = order.TerminalId;
                    frinput.BulkPlantId = order.BulkPlantId;

                    if (!string.IsNullOrEmpty(tpdDrop.FreightRateRuleType))
                    {
                        FreightRateRuleType frRuleType = (FreightRateRuleType)Enum.Parse(typeof(FreightRateRuleType), tpdDrop.FreightRateRuleType, true);
                        fscObj.FreightRateRuleType = frRuleType;
                        frinput.FreightRateRuleType = (int)frRuleType;
                        applyFreightRate = true;
                    }
                    if (!string.IsNullOrEmpty(tpdDrop.FreightRateTableType))
                    {
                        TableTypes frTableType = (TableTypes)Enum.Parse(typeof(TableTypes), tpdDrop.FreightRateTableType, true);
                        fscObj.FreightRateTableType = frTableType;
                        frinput.TableType = (int)frTableType;
                        applyFreightRate = true;
                    }


                    if (!string.IsNullOrEmpty(tpdDrop.FreightRateTableName))
                    {
                        fscObj.FreightRateRuleId = GetFreightRateTablesForInvoice(frinput, tpdDrop.FreightRateTableName);
                        if (fscObj.FreightRateRuleId <= 0)
                        {
                            apiResponse.Messages.Add(new ApiCodeMessages()
                            { Code = Constants.ApiCodeFR06, Message = "Order does not associated with given freight Rate Table Name." });
                        }
                        else
                        {
                            if (tpdDrop.FreightCost > 0)
                            {
                                fscObj.SurchargeFreightCost = tpdDrop.FreightCost;
                            }
                            else
                            {
                                FreightCostInputViewModel fcInput = new FreightCostInputViewModel();
                                fcInput.FreightRateRuleId = fscObj.FreightRateRuleId.Value;
                                fcInput.OrderId = frinput.OrderId.Value;
                                fcInput.TerminalId = frinput.TerminalId;
                                fcInput.BulkPlantId = frinput.BulkPlantId;
                                fcInput.SupplierId = frinput.SupplierId;
                                fcInput.Distance = tpdDrop.Distance;
                                fscObj.SurchargeFreightCost = GetFreightCostForInvoice(fcInput);
                            }

                            applyFreightRate = true;
                        }
                    }

                    fscObj.AutoFreightDistance = tpdDrop.Distance;
                    

                    if (!string.IsNullOrEmpty(tpdDrop.FuelSurchargeTableType))
                    {
                        TableTypes fsTableType = (TableTypes)Enum.Parse(typeof(TableTypes), tpdDrop.FuelSurchargeTableType, true);
                        fscObj.FuelSurchargeTableType = fsTableType;
                        frinput.TableType = (int)fsTableType;
                        applyFuelSurcharge = true;
                    }

                    if (!string.IsNullOrEmpty(tpdDrop.FuelSurchargeTableName))
                    {
                        fscObj.FuelSurchargeTableId = GetFuelSurchargeTablesForInvoice(frinput, tpdDrop.FuelSurchargeTableName);
                        if (fscObj.FuelSurchargeTableId <= 0)
                        {
                            apiResponse.Messages.Add(new ApiCodeMessages()
                            { Code = Constants.ApiCodeFR07, Message = "Order does not associated with given Fuel Surcharge Table Name." });
                        }
                        else
                        {
                            applyFuelSurcharge = true;
                        }
                    }

                    fscObj.IsFreightCostApplicable = applyFreightRate;
                    fscObj.IsSurchargeApplicable = applyFuelSurcharge;

                    SetAccessorialFeesTable(invoiceViewModel, tpdDrop, frinput, apiResponse);
                }
            }
        }

        private void SetAccessorialFeesTable(InvoiceViewModelNew invoiceViewModel, TPDDropDetails tpdDrop, FreightRateInputViewModel frinput, ApiResponseViewModel apiResponse)
        {
            if (!string.IsNullOrEmpty(tpdDrop.AccessorialFeesTableType) && !string.IsNullOrEmpty(tpdDrop.AccessorialFeesTableName))
            {
                if (invoiceViewModel.AccessorialFeeDetails == null) invoiceViewModel.AccessorialFeeDetails = new List<AccessorialFeeTableDetailViewModel>();
                AccessorialFeeTableDetailViewModel item = new AccessorialFeeTableDetailViewModel();
                TableTypes accTableType = (TableTypes)Enum.Parse(typeof(TableTypes), tpdDrop.AccessorialFeesTableType, true);
                item.AccessorialFeeTableType = accTableType;
                frinput.TableType = (int)accTableType;
                item.AccessorialFeeId = GetAccessorialFeeIdForInvoice(frinput, tpdDrop.AccessorialFeesTableName);
                if (item.AccessorialFeeId <= 0)
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeFR08, Message = "Order does not associated with given Accessorial Fees Table Name." });
                }
                invoiceViewModel.AccessorialFeeDetails.Add(item);
            }
        }



        public decimal GetFreightCostForInvoice(FreightCostInputViewModel fcostInput)
        {
            decimal response;
            response = Task.Run(() => ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFreightCostForInvoice(fcostInput)).Result;           
            return response;
        }
        public int GetAccessorialFeeIdForInvoice(FreightRateInputViewModel filter, string accessorialFeesTableName)
        {
            List<DropdownDisplayItem> response;
            int AccessorialFeeId = -1;
            response = Task.Run(() => ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAccessorialFeeTablesForInvoice(filter)).Result;
            if (response != null)
            {
                var AccessorialFee = response.FirstOrDefault(t => t.Name.Trim().ToUpper() == accessorialFeesTableName.Trim().ToUpper());
                if (AccessorialFee != null)
                {
                    AccessorialFeeId = AccessorialFee.Id;
                }
            }
            return AccessorialFeeId;
        }
        public int GetFuelSurchargeTablesForInvoice(FreightRateInputViewModel filter, string fuelSurchargeTableName)
        {
            List<DropdownDisplayItem> response;
            int fuelSurchargeTableId = -1;
            response = Task.Run(() => ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFuelSurchargeTablesForInvoice(filter)).Result;
            if (response != null)
            {
                var fuelSurchargeTable = response.FirstOrDefault(t => t.Name.Trim().ToUpper() == fuelSurchargeTableName.Trim().ToUpper());
                if (fuelSurchargeTable != null)
                {
                    fuelSurchargeTableId = fuelSurchargeTable.Id;
                }
            }
            return fuelSurchargeTableId;
        }

        public int GetFreightRateTablesForInvoice(FreightRateInputViewModel filter, string freightRateTableName)
        {
            List<DropdownDisplayItem> response;
            int freightRateTableId = -1;
            response = Task.Run(() => ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFreightRateTablesForInvoice(filter)).Result;            
            if (response != null)
            {
                var freightRateTable = response.FirstOrDefault(t => t.Name.Trim().ToUpper() == freightRateTableName.Trim().ToUpper());
                if (freightRateTable != null)
                {
                    freightRateTableId = freightRateTable.Id;
                }
            }
            return freightRateTableId;
        }

        public int GetOrderId(UserContext apiUserContext, ApiResponseViewModel response, string poNumber, string locationId, string productId)
        {
            int resultOrderId = 0;

            try
            {
                //int fueltype = GetFuelTypeId(dropInfo, apiUserContext); //need to wait for Sharque's changes
                if (!string.IsNullOrWhiteSpace(poNumber))
                {
                    var orderPo = Context.DataContext.Orders.Where(t => t.PoNumber.ToLower() == poNumber.ToLower()
                                                            && t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open)
                                                            && t.AcceptedCompanyId == apiUserContext.CompanyId
                                                            && (t.BuyerCompanyId == supplierCompanyIdForCarrier || supplierCompanyIdForCarrier == 0))
                                                            .OrderByDescending(t => t.AcceptedDate).SingleOrDefault();
                    if (orderPo != null)
                    {
                        resultOrderId = orderPo.Id;
                    }
                }

                if (resultOrderId == 0 && !string.IsNullOrWhiteSpace(locationId) && !string.IsNullOrWhiteSpace(productId))
                {
                    if (supplierCompanyIdForCarrier == 0)
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "Customer ID") });
                    }
                    string ponumber = poNumber;
                    resultOrderId = GetOrderIdFromLocAndProduct(apiUserContext, productId, locationId, supplierCompanyIdForCarrier, ref ponumber);
                }

                if (resultOrderId == 0)
                {
                    if (!string.IsNullOrWhiteSpace(poNumber))
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRS01,
                            Message = string.Format(Resource.errMsgEnableToFindPO, poNumber)
                        });
                    }
                    else if (!string.IsNullOrWhiteSpace(locationId) && !string.IsNullOrWhiteSpace(productId))
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRS01,
                            Message = string.Format(Resource.errMsgEnableToFindPOFrom, locationId, productId)
                        });
                    }
                    else
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRS01,
                            Message = Resource.errMsgEnableToFindExactPO
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceTPDDomain", "GetOrderId", ex.Message, ex);
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRS01,
                    Message = Resource.errMsgMultipleOrdersFound
                });
            }
            return resultOrderId;
        }

        public int GetOrderIdForInvoiceApi(UserContext apiUserContext, ApiResponseViewModel response, TPDDropDetails dropInfo
                                , string locationId, List<int> mstProducts, int? trackableScheduleId, int? orderIdFromCarrierOrderId, List<ApiCodeMessages> exceptionDdtMessages = null)
        {
            int resultOrderId = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(dropInfo.PONumber))
                {
                    var orderPo = Context.DataContext.Orders.Where(t => t.PoNumber.ToLower() == dropInfo.PONumber.ToLower()
                                                            && t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open)
                                                            && t.AcceptedCompanyId == apiUserContext.CompanyId
                                                            && (t.BuyerCompanyId == supplierCompanyIdForCarrier || supplierCompanyIdForCarrier == 0))
                                                    .OrderByDescending(t => t.AcceptedDate)
                                                    .Select(t => new { t.Id })
                                                    .SingleOrDefault();
                    if (orderPo != null)
                    {
                        resultOrderId = orderPo.Id;
                        if (dropInfo != null)
                        {
                            dropInfo.OrderId = resultOrderId;
                        }
                    }
                }

                if (resultOrderId == 0 && !string.IsNullOrWhiteSpace(locationId) && !string.IsNullOrWhiteSpace(dropInfo.Product))
                {
                    if (supplierCompanyIdForCarrier == 0)
                    {
                        var errorMsg = new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "Customer ID") };
                        response.Messages.Add(errorMsg);
                    }

                    string ponumber = dropInfo.PONumber;
                    resultOrderId = GetOrderIdForApiLocAndProduct(apiUserContext, mstProducts, locationId, supplierCompanyIdForCarrier, ref ponumber);
                    if (dropInfo != null)
                    {
                        dropInfo.OrderId = resultOrderId;
                    }
                }

                if (resultOrderId == 0 && !string.IsNullOrWhiteSpace(dropInfo.CarrierOrderID))
                {
                    if (orderIdFromCarrierOrderId.HasValue && orderIdFromCarrierOrderId.Value > 0)
                    {
                        resultOrderId = orderIdFromCarrierOrderId.Value;
                        if (dropInfo != null)
                        {
                            dropInfo.OrderId = resultOrderId;
                        }
                    }
                }

                if (resultOrderId == 0)
                {
                    if (!string.IsNullOrWhiteSpace(dropInfo.PONumber))
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRS01,
                            Message = string.Format(Resource.errMsgEnableToFindPO, dropInfo.PONumber)
                        });
                    }
                    else if (!string.IsNullOrWhiteSpace(locationId) && !string.IsNullOrWhiteSpace(dropInfo.Product))
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRS01,
                            Message = string.Format(Resource.errMsgEnableToFindPOFrom, locationId, dropInfo.Product)
                        });
                    }
                    else if (!string.IsNullOrWhiteSpace(dropInfo.CarrierOrderID))
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRS01,
                            Message = string.Format(Resource.errMsgApiCodeRQ06CarrierOrderIdNotFound, dropInfo.CarrierOrderID)
                        });
                    }
                    else
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRS01,
                            Message = Resource.errMsgEnableToFindExactPO
                        });
                    }

                    // generate exception ddt if order not found but product/fueltype found for api request. 
                    // If product/fueltype not found then do not raise exception 
                    if (exceptionDdtMessages != null && mstProducts.Count > 0)
                    {
                        exceptionDdtMessages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeEV03,
                            Message = Resource.errMsgEnableToFindExactPO
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceTPDDomain", "GetOrderIdForInvoiceApi", ex.Message, ex);
                var errorMsg = new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.errMsgMultipleOrdersFound };
                response.Messages.Add(errorMsg);
                exceptionDdtMessages.Add(errorMsg);
            }
            return resultOrderId;
        }

        private void ValidateHeader(ApiResponseViewModel apiResonse, TPDInvoiceViewModel apiRequestModel)
        {
            //WILL BE REQUIRED SO COMMENTED
            //if (string.IsNullOrWhiteSpace(apiRequestModel.Carrier))

            ValidateDropDriver(apiResonse, apiRequestModel);

            if (apiRequestModel.DropDetails == null || !apiRequestModel.DropDetails.Any())
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = string.Format(Resource.errMsgParameterIsRequired, "Drop Details")
                });
            }
            else
                ValidateDropDetails(apiResonse, apiRequestModel);

            if (!string.IsNullOrWhiteSpace(apiRequestModel.SupplierInvoiceNumber))
            {
                if (apiRequestModel.SupplierInvoiceNumber.Length > 19)
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = Resource.errMsgSupInvTooLong });
                }
            }
            if (apiResonse.Messages.Any())
            {
                if (apiRequestModel.DropDetails.Any())
                {
                    var OrderId = apiRequestModel.DropDetails.FirstOrDefault().OrderId;
                    if (OrderId != null)
                    {
                        apiResonse.Messages.ForEach(x =>
                        {
                            x.OrderId = OrderId;
                        });
                    }
                }
            }
            //ValidateDropAddressParameters(apiResonse, apiRequestModel);


        }

        private void ValidateFreightRate(ApiResponseViewModel apiResonse, TPDDropDetails dropDetail)
        {
            if (!string.IsNullOrEmpty(dropDetail.FreightRateRuleType)
                && (dropDetail.FreightRateRuleType.Trim().ToUpper() == FreightRateRuleType.Range.GetDisplayName().Trim().ToUpper()
                || dropDetail.FreightRateRuleType.Trim().ToUpper() == FreightRateRuleType.Route.GetDisplayName().Trim().ToUpper()))
            {
                if (string.IsNullOrEmpty(dropDetail.FreightRateTableType))
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeFR01, Message = "FreightTableType is required if FreightType is Range or Route." });
                }
            }


            if (!string.IsNullOrEmpty(dropDetail.FreightRateRuleType)
                && dropDetail.FreightRateRuleType.Trim().ToUpper() == FreightRateRuleType.Range.GetDisplayName().Trim().ToUpper())
            {
                if (dropDetail.Distance == 0)
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeFR02, Message = "Distance is required if FreightType is Range" }); 
                }
            }
            if (!string.IsNullOrEmpty(dropDetail.FreightRateRuleType) && !Enum.IsDefined(typeof(FreightRateRuleType), dropDetail.FreightRateRuleType))
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeFR03, Message = "Invalid FreightRateRuleType." });

            }

            if (!string.IsNullOrEmpty(dropDetail.FreightRateTableType) && !Enum.IsDefined(typeof(TableTypes), dropDetail.FreightRateTableType))
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeFR03, Message = "Invalid FreightRateTableType." });
            }

            if (!string.IsNullOrEmpty(dropDetail.FuelSurchargeTableType) && !Enum.IsDefined(typeof(TableTypes), dropDetail.FuelSurchargeTableType))
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeFR04, Message = "Invalid FuelSurchargeTableType." });
            }
        }

        private void ValidateDropDetails(ApiResponseViewModel apiResonse, TPDInvoiceViewModel apiRequestModel)
        {
            foreach (var drop in apiRequestModel.DropDetails)
            {
                if (string.IsNullOrWhiteSpace(drop.PONumber))
                {
                    if (string.IsNullOrWhiteSpace(drop.Product) || string.IsNullOrWhiteSpace(apiRequestModel.CustomerID)
                        || string.IsNullOrWhiteSpace(apiRequestModel.LocationId))
                    {
                        if (string.IsNullOrWhiteSpace(drop.CarrierOrderID))
                        {
                            apiResonse.Messages.Add(new ApiCodeMessages()
                            {
                                Code = Constants.ApiCodeRQ02,
                                Message = string.Format(Resource.errMsgParameterIsRequired, "PO Number OR CustomerID - LocationId - Product OR CarrierOrder ID for each DropDetails")
                            });
                        }
                    }
                }

                ValidateDropDateTime(apiResonse, drop);
                ValidateTankDetails(apiResonse, drop, apiRequestModel);
                ValidateFreightRate(apiResonse, drop);
                if (apiResonse.Status == Status.Failed)
                {
                    if (apiResonse.Messages.Any())
                    {
                        apiResonse.Messages.ForEach(x => x.OrderId = drop.OrderId);
                    }
                }
            }
        }

        private static void ValidateDropDriver(ApiResponseViewModel apiResponse, TPDInvoiceViewModel apiRequestModel)
        {
            if (string.IsNullOrWhiteSpace(apiRequestModel.DriverFirstName) && !string.IsNullOrWhiteSpace(apiRequestModel.DriverLastName))
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DriverFirstName)) });
            }

            if (!string.IsNullOrWhiteSpace(apiRequestModel.DriverFirstName) && string.IsNullOrWhiteSpace(apiRequestModel.DriverLastName))
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DriverLastName)) });
            }
        }

        private void ValidateDropDateTime(ApiResponseViewModel apiResonse, TPDDropDetails dropDetail)
        {
            if (string.IsNullOrWhiteSpace(dropDetail.DropArrivalDate))
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(dropDetail.DropArrivalDate)) });
            }
            else
            {
                ValidateDate(nameof(dropDetail.DropArrivalDate), dropDetail.DropArrivalDate, apiResonse);
            }

            if (!string.IsNullOrWhiteSpace(dropDetail.DropCompleteDate))
            {
                ValidateDate(nameof(dropDetail.DropCompleteDate), dropDetail.DropCompleteDate, apiResonse);
                if (DateTime.TryParse(dropDetail.DropArrivalDate, out DateTime startdateTime) && DateTime.TryParse(dropDetail.DropCompleteDate, out DateTime enddateTime))
                {
                    if (startdateTime > enddateTime)
                    {
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ05, Message = string.Format(Resource.errMsgParameterGreaterThanDropDate, nameof(dropDetail.DropArrivalDate)) });
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(dropDetail.DropArrivalTime))
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(dropDetail.DropArrivalTime)) });
            }
            else
                ValidateTimeParameter(nameof(dropDetail.DropArrivalTime), dropDetail.DropArrivalTime, apiResonse);

            if (string.IsNullOrWhiteSpace(dropDetail.DropCompleteTime))
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(dropDetail.DropCompleteTime)) });
            }

            else
            {
                ValidateTimeParameter(nameof(dropDetail.DropCompleteTime), dropDetail.DropCompleteTime, apiResonse);
                //ValidateStartAndEndTimeParameter(dropDetail.DropArrivalTime, dropDetail.DropCompleteTime, "Drop Complete Time", apiResonse);                
            }
            if (!string.IsNullOrWhiteSpace(dropDetail.DropArrivalTime) && !string.IsNullOrWhiteSpace(dropDetail.DropCompleteTime))
            {
                ValidateAssetStartAndEndTime(apiResonse, dropDetail);
            }
        }

        private void ValidateStartAndEndTimeParameter(string startTime, string endTime, string startTimeParamName, string endTimeParameterName, ApiResponseViewModel apiResponse)
        {
            var starttimespan = GetTimeSpan(startTime);
            var endtimespan = GetTimeSpan(endTime);

            if (starttimespan.HasValue && endtimespan.HasValue)
            {
                if (starttimespan.Value.Hours > endtimespan.Value.Hours)
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.valMessageLessThan, startTimeParamName, endTimeParameterName) });
                }
            }
        }

        private TimeSpan? GetTimeSpan(string csvTime)
        {
            if (!string.IsNullOrWhiteSpace(csvTime))
            {
                var isPmTime = csvTime.ToLower().Contains("pm");
                var time = csvTime.ToLower().Replace("am", string.Empty).Replace("pm", string.Empty);
                TimeSpan.TryParse(time, out TimeSpan result);
                if (isPmTime && result.Hours != 12)
                    result = result.Add(new TimeSpan(12, 0, 0));
                if (!isPmTime && result.Hours == 12)
                    result = result.Subtract(new TimeSpan(12, 0, 0));

                return result;
            }
            return null;
        }
        private void ValidateTimeParameter(string parameterName, string value, ApiResponseViewModel apiResonse)
        {
            var time = value.ToLower().Replace("am", string.Empty).Replace("pm", string.Empty);
            bool isSucess = TimeSpan.TryParse(time, out TimeSpan result);
            if (!isSucess)
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ03, Message = string.Format(Resource.errMsgParameterFormatIsInvalid, parameterName) });
            }
        }

        private void ValidateDate(string propertyName, string dateToValidate, ApiResponseViewModel apiResonse, DateTime? orderDropDate = null)
        {
            if (!DateTime.TryParse(dateToValidate, out DateTime dateTime))
            {
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ03, Message = string.Format(Resource.errMsgParameterFormatIsInvalid, propertyName) });
            }

            if (dateTime.Date > DateTime.Now.Date)
                apiResonse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ05, Message = string.Format(Resource.errMsgParameterGreaterThanCurrentDate, propertyName) });

            if (orderDropDate != null)
            {
                if (dateTime > orderDropDate.Value)
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ05, Message = string.Format(Resource.errMsgParameterGreaterThanDropDate, propertyName) });
            }

        }

        private void ValidateFTLAndIsVariousParameters(List<string> allStates, TPDDropDetails dropInfo, ApiResponseViewModel apiResonse, TPDInvoiceViewModel apiRequestModel, InvoiceDropViewModel invDropModel, bool isVarious)
        {
            if (invDropModel.IsBolDetailsRequired)//invDropModel.IsFTL || 
            {
                if (!string.IsNullOrWhiteSpace(apiRequestModel.DropZip))
                {
                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropAddress1) && string.IsNullOrWhiteSpace(apiRequestModel.DropAddress2))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DropAddress1)) });

                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropCity))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DropCity)) });

                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropStateCode))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DropStateCode)) });
                    else
                    {
                        if (!allStates.Any(t => t == apiRequestModel.DropStateCode.ToLower()))
                            apiResonse.Messages.Add(new ApiCodeMessages()
                            { Code = Constants.ApiCodeRQ03, Message = string.Format(Resource.errMsgParameterFormatIsInvalid, nameof(apiRequestModel.DropStateCode)) });
                    }
                }

                if (!string.IsNullOrWhiteSpace(apiRequestModel.DropLatitude))
                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropLongitude))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DropLongitude)) });

                if (!string.IsNullOrWhiteSpace(apiRequestModel.DropLongitude))
                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropLatitude))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DropLatitude)) });

                if (isVarious)
                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropZip) && string.IsNullOrWhiteSpace(apiRequestModel.DropLatitude) && string.IsNullOrWhiteSpace(apiRequestModel.DropLongitude))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "Drop Address") });

                if ((dropInfo.BolDetails == null && dropInfo.LiftDetails == null) || (!dropInfo.BolDetails.Any() && !dropInfo.LiftDetails.Any()))
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "BOL/Lift Details for Order") });
            }

            if (!string.IsNullOrWhiteSpace(dropInfo.OrderDate))
            {
                ValidateDate(nameof(dropInfo.OrderDate), dropInfo.OrderDate, apiResonse);
            }

            // validate MFN parameters when UOM is MetricTons or Barrel
            if (invDropModel.IsMarineLocation && invDropModel.UoM == UoM.MetricTons)
            {
                if (dropInfo.ApiGravity == null || dropInfo.ApiGravity <= 0)
                {
                    apiResonse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "API Gravity") });
                }
                else
                {
                    var modelForConversion = new MFNConversionRequestViewModel() { DroppedGallons = dropInfo.TotalDropQuantity, ConversionFactor = dropInfo.ApiGravity ?? 0, JobCountryId = invDropModel.JobCountryId, UoM = invDropModel.UoM };
                    var invoiceDomain = new InvoiceDomain(this);
                    var conversionResponse = Task.Run(() => invoiceDomain.ValidateGravityAndConvertForMFN(modelForConversion)).Result;
                    if (!conversionResponse.IsValidGravity)
                    {
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "API Gravity") });
                    }
                }
            }
        }

        private void ValidateDropAddressAndIsVariousParameters(List<string> allStates, TPDDropDetails dropInfo, ApiResponseViewModel apiResonse, TPDInvoiceViewModel apiRequestModel, InvoiceDropViewModel invDropModel, bool isVarious)
        {
            if (invDropModel.IsBolDetailsRequired)//invDropModel.IsFTL || 
            {
                if (!string.IsNullOrWhiteSpace(apiRequestModel.DropZip))
                {
                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropAddress1) && string.IsNullOrWhiteSpace(apiRequestModel.DropAddress2))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DropAddress1)) });

                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropCity))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DropCity)) });

                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropStateCode))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DropStateCode)) });
                    else
                    {
                        if (!allStates.Any(t => t == apiRequestModel.DropStateCode.ToLower()))
                            apiResonse.Messages.Add(new ApiCodeMessages()
                            { Code = Constants.ApiCodeRQ03, Message = string.Format(Resource.errMsgParameterFormatIsInvalid, nameof(apiRequestModel.DropStateCode)) });
                    }
                }

                if (!string.IsNullOrWhiteSpace(apiRequestModel.DropLatitude))
                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropLongitude))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DropLongitude)) });

                if (!string.IsNullOrWhiteSpace(apiRequestModel.DropLongitude))
                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropLatitude))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DropLatitude)) });

                if (isVarious)
                    if (string.IsNullOrWhiteSpace(apiRequestModel.DropZip) && string.IsNullOrWhiteSpace(apiRequestModel.DropLatitude) && string.IsNullOrWhiteSpace(apiRequestModel.DropLongitude))
                        apiResonse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "Drop Address") });
            }

            if (!string.IsNullOrWhiteSpace(dropInfo.OrderDate))
            {
                ValidateDate(nameof(dropInfo.OrderDate), dropInfo.OrderDate, apiResonse);
            }
        }

        private void ValidateAssetStartAndEndTime(ApiResponseViewModel apiResponse, TPDDropDetails dropDetails)
        {
            if (!string.IsNullOrWhiteSpace(dropDetails.DropArrivalTime) && !string.IsNullOrWhiteSpace(dropDetails.DropCompleteTime))
            {
                var DropArrivalTimespan = GetTimeSpan(dropDetails.DropArrivalTime);
                var DropCompleteTimespan = GetTimeSpan(dropDetails.DropCompleteTime);

                if (dropDetails.Tanks != null && dropDetails.Tanks.Any())
                {
                    foreach (var tank in dropDetails.Tanks)
                    {
                        if (!string.IsNullOrWhiteSpace(tank.StartTime) && !string.IsNullOrWhiteSpace(tank.EndTime))
                        {
                            var assetStarttimespan = GetTimeSpan(tank.StartTime);
                            var assetEndtimespan = GetTimeSpan(tank.EndTime);

                            //if (assetStarttimespan.Value.Hours > assetEndtimespan.Value.Hours)
                            //{
                            //    apiResponse.Messages.Add(new ApiCodeMessages()
                            //    { Code = Constants.ApiCodeRQ01, Message = Resource.errMessageStartimeLessthanEndTime });
                            //}
                            if (assetStarttimespan.Value.Hours == assetEndtimespan.Value.Hours)
                            {
                                if (assetStarttimespan.Value.Minutes > assetEndtimespan.Value.Minutes)
                                {
                                    apiResponse.Messages.Add(new ApiCodeMessages()
                                    { Code = Constants.ApiCodeRQ01, Message = Resource.errMessageStartimeLessthanEndTime });
                                }
                            }

                            if (assetStarttimespan.Value.Hours < DropArrivalTimespan.Value.Hours)
                            {
                                apiResponse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ01, Message = Resource.errMessageStartTimeGreaterThanDropStartTime });
                            }
                            if (assetStarttimespan.Value.Hours == DropArrivalTimespan.Value.Hours)
                            {
                                if (assetStarttimespan.Value.Minutes < DropArrivalTimespan.Value.Minutes)
                                {
                                    apiResponse.Messages.Add(new ApiCodeMessages()
                                    { Code = Constants.ApiCodeRQ01, Message = Resource.errMessageStartTimeGreaterThanDropStartTime });
                                }
                            }

                            if (assetEndtimespan.Value.Hours > DropCompleteTimespan.Value.Hours)
                            {
                                apiResponse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ01, Message = Resource.errMessageAssetTimeLessThanDropCompleteTime });
                            }
                            if (assetEndtimespan.Value.Hours == DropCompleteTimespan.Value.Hours)
                            {
                                if (assetEndtimespan.Value.Minutes > DropCompleteTimespan.Value.Minutes)
                                {
                                    apiResponse.Messages.Add(new ApiCodeMessages()
                                    { Code = Constants.ApiCodeRQ01, Message = Resource.errMessageAssetTimeLessThanDropCompleteTime });
                                }
                            }
                            //else if (assetStarttimespan.Value.Hours < DropArrivalTimespan.Value.Hours)
                            //{
                            //    apiResponse.Messages.Add(new ApiCodeMessages()
                            //    { Code = Constants.ApiCodeRQ01, Message = " Start time Should be More than Drop Arrival Time" });
                            //}
                            //else if (assetEndtimespan.Value.Hours > DropCompleteTimespan.Value.Hours)
                            //{
                            //    apiResponse.Messages.Add(new ApiCodeMessages()
                            //    { Code = Constants.ApiCodeRQ01, Message = "End time Should be Less than Drop Complete Time" });
                            //}

                        }
                    }
                }
            }
        }


        #endregion

        #region API-csv style

        #endregion


        #region TPD API
        public async Task<ApiResponseViewModel> ValidateAndCreateSchedule(TPDCreateScheduleViewModel apiRequestModel, string token)
        {
            ApiResponseViewModel apiResponse = new ApiResponseViewModel();
            try
            {
                if (apiRequestModel != null)
                {
                    //get userdetails from token
                    var authDomain = new AuthenticationDomain(this);
                    var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
                    if (apiUserContext != null)
                    {
                        ValidateRequireParameters(apiRequestModel, apiResponse);
                        if (!apiResponse.Messages.Any())
                        {
                            await ProcessScheduleCreateApiRequest(apiRequestModel, apiResponse, apiUserContext);
                        }
                    }
                    else
                        apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ01, Message = Resource.errMsgInvalidToken });
                }
                else
                    apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = Resource.errMsgParmeterNotMatch });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceTPDDomain", "ValidateAndCreateSchedule", ex.Message, ex);
                apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeTF01, Message = Resource.errMsgProcessRequestFailed });
            }

            if (!apiResponse.Messages.Any())
                apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeTF01, Message = Resource.errMsgProcessRequestFailed });

            return apiResponse;
        }

        private async Task<int> ProcessScheduleCreateApiRequest(TPDCreateScheduleViewModel apiRequestModel, ApiResponseViewModel apiResponse, UserContext apiUserContext)
        {
            //get driverid
            //Get OrderId from parameters
            //insert into queue service
            //var driverId = GetDriverIdForAPI(apiRequestModel.DriverFirstName, apiRequestModel.DriverLastName, )

            if (!apiResponse.Messages.Any())
            {
                //var orderId = GetOrderId(apiUserContext, apiResonse, schedule.PoNumber, schedule.LocationID, schedule.ProductID);
                ProcessDSBCreation dSBCreation = await SetProcessDSBCreationViewModel(apiRequestModel, apiResponse, apiUserContext);
                if (!apiResponse.Messages.Any())
                {
                    await new CreateScheduleApiDomain(this).CreateSchedule(dSBCreation, apiResponse);
                }
            }
            //apiResonse.Messages.Add(new ApiCodeMessages()
            //{ Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.DriverEmail)) });
            return 1;
        }

        private async Task<ProcessDSBCreation> SetProcessDSBCreationViewModel(TPDCreateScheduleViewModel apiRequestModel, ApiResponseViewModel apiResponse, UserContext apiUserContext)
        {
            var resultViewModel = new ProcessDSBCreation();

            resultViewModel.CarrierCompanyId = apiUserContext.CompanyId;
            resultViewModel.CarrierCompanyName = apiUserContext.CompanyName;

            var driver = Context.DataContext.Users.Where(t => t.CompanyId == apiUserContext.CompanyId
                                        && t.Email.ToLower().Equals(apiRequestModel.DriverEmail.ToLower().Trim()))
                                        .Select(t => new { t.Id, t.FirstName, t.LastName })
                                        .FirstOrDefault();

            if (driver != null)
                resultViewModel.Drivers = new DropdownDisplayExtendedItem() { Id = driver.Id, Name = $"{driver.FirstName} {driver.LastName}" };
            else
            {
                if (!string.IsNullOrWhiteSpace(apiRequestModel.DriverFirstName) && !string.IsNullOrWhiteSpace(apiRequestModel.DriverLastName) && apiRequestModel.DriverContactNumber > 0)
                {
                    var newDriver = await GetDriverIdForAPI(apiRequestModel.DriverFirstName, apiRequestModel.DriverLastName, apiUserContext.CompanyName, apiUserContext, apiRequestModel.DriverEmail, apiRequestModel.DriverContactNumber.ToString());
                    if (newDriver != null)
                        resultViewModel.Drivers = new DropdownDisplayExtendedItem() { Id = newDriver.Item1, Name = newDriver.Item2 };
                    else
                        apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "Driver Email") });
                }
                else
                    apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "Driver Email") });
            }

            resultViewModel.Date = apiRequestModel.ScheduleDate;
            resultViewModel.StartTime = apiRequestModel.ScheduleStartTime;
            resultViewModel.EndTime = apiRequestModel.ScheduleEndTime;
            resultViewModel.UserId = apiUserContext.Id;
            resultViewModel.ExternalRefID = apiRequestModel.ExternalRefID;

            //validate carrier order id
            var carrierOrderIdList = Context.DataContext.DeliveryScheduleXTrackableSchedules
                                    .Where(t => t.Order.AcceptedCompanyId == apiUserContext.CompanyId && t.CarrierOrderId != null && t.OrderId > 0 && t.IsActive)
                                    .Where(Extensions.IsOpenMissedTrackableSchedule())
                                .Select(t => new { t.OrderId, t.CarrierOrderId, t.Date }).ToList();

            foreach (var schedule in apiRequestModel.ScheduleDetails)
            {
                supplierCompanyIdForCarrier = SetCarrierCompanyParameters(apiResponse, schedule.CustomerID, apiUserContext);
                if (!apiResponse.Messages.Any())
                {
                    var orderId = GetOrderId(apiUserContext, apiResponse, schedule.PoNumber, schedule.LocationID, schedule.ProductID);
                    if (orderId > 0)
                    {
                        var ordDetails = Context.DataContext.Orders.Where(t => t.Id == orderId)
                                        .Select(t => new
                                        {
                                            t.Id,
                                            t.PoNumber,
                                            SupplierCompanyId = t.BuyerCompanyId,
                                            SupplierCompanyName = t.BuyerCompany.Name,
                                            t.FuelRequest.JobId,
                                            JobName = t.FuelRequest.Job.Name,
                                            JobCompanyId = t.FuelRequest.Job.CompanyId,
                                            t.FuelRequest.FuelTypeId,
                                            t.FuelRequest.Job.DisplayJobID,
                                            JobAddress = t.FuelRequest.Job.Address + " " + t.FuelRequest.Job.City,
                                            JobCity = t.FuelRequest.Job.City,
                                            t.FuelRequest.Job.UoM,
                                            t.FuelRequest.FuelRequestTypeId,
                                            t.AcceptedCompanyId,
                                            AcceptedCompanyName = t.Company.Name,
                                            ProductTypeName = t.FuelRequest.MstProduct.MstProductType.Name,
                                            t.FuelRequest.MstProduct.ProductTypeId,
                                            t.FuelRequest.Currency,
                                            t.FuelRequest.Job.TimeZoneName
                                        }).SingleOrDefault();

                        var scheduleDetail = new ScheduleDetails()
                        {
                            BadgeNumber = schedule.BadgeNumber,
                            DispatcherNote = schedule.DispatcherNote,
                            FuelTypeId = ordDetails.FuelTypeId,
                            JobId = ordDetails.JobId,
                            JobName = ordDetails.JobName,
                            JobAddress = ordDetails.JobAddress,
                            JobCity = ordDetails.JobCity,
                            UoM = ordDetails.UoM,
                            OrderId = ordDetails.Id,
                            PoNumber = ordDetails.PoNumber,
                            RequiredQuantity = schedule.ScheduleQuantity,
                            SiteId = ordDetails.DisplayJobID,
                            StorageId = schedule.StorageID,
                            TankId = schedule.TankID,
                            Currency = ordDetails.Currency,
                            TimeZoneName = ordDetails.TimeZoneName,
                            ProductTypeId = ordDetails.ProductTypeId,
                            ProductTypeName = ordDetails.ProductTypeName,
                            CustomerCompanyName = ordDetails.SupplierCompanyName,
                            CarrierOrderID = schedule.CarrierOrderID
                        };


                        if (!string.IsNullOrWhiteSpace(schedule.TerminalControlNumber))
                        {
                            scheduleDetail.PickupLocationType = PickupLocationType.Terminal;
                            scheduleDetail.TCNFromAPI = schedule.TerminalControlNumber;
                        }
                        else if (!string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.SiteName))
                        {
                            scheduleDetail.PickupLocationType = PickupLocationType.BulkPlant;

                            var existingbulkPlant = Context.DataContext.BulkPlantLocations.Where(t => t.IsActive && t.CompanyId == apiUserContext.CompanyId
                                    && t.Name.ToLower().Equals(schedule.BulkPlantDetails.SiteName))
                                    .Select(t => new DropAddressViewModel
                                    {
                                        SiteId = t.Id,
                                        SiteName = t.Name,
                                        Address = t.Address,
                                        City = t.City,
                                        CountyName = t.CountyName,
                                        ZipCode = t.ZipCode,
                                        State = new StateViewModel() { Id = t.StateId, Code = t.StateCode },
                                        Country = new CountryViewModel() { Id = t.CountryId, Code = t.CountryCode },
                                        IsAddressAvailable = true,
                                        Latitude = t.Latitude,
                                        Longitude = t.Longitude
                                    }).FirstOrDefault();

                            if (existingbulkPlant != null)
                            {
                                scheduleDetail.BulkPlant = existingbulkPlant;
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.Address) && !string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.City)
                                    && !string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.State) && !string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.Country)
                                    && !string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.CountyName) && !string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.ZipCode))
                                {
                                    var country = Context.DataContext.MstCountries.Where(t => t.IsActive &&
                                            (t.Name.ToLower().Equals(schedule.BulkPlantDetails.Country.ToLower()) || t.Code.ToLower().Equals(schedule.BulkPlantDetails.Country.ToLower())))
                                            .Select(t => new CountryViewModel() { Id = t.Id, Name = t.Name, Code = t.Code }).FirstOrDefault();
                                    if (country != null)
                                    {
                                        var state = Context.DataContext.MstStates.Where(t => t.IsActive &&
                                                     (t.Name.ToLower().Equals(schedule.BulkPlantDetails.State.ToLower()) || t.Code.ToLower().Equals(schedule.BulkPlantDetails.State.ToLower())))
                                                     .Select(t => new StateViewModel() { Id = t.Id, Code = t.Code, Name = t.Name }).FirstOrDefault();
                                        if (state != null)
                                        {
                                            //ADD new bulk plant
                                            scheduleDetail.BulkPlant = new DropAddressViewModel()
                                            {
                                                Address = schedule.BulkPlantDetails.Address,
                                                City = schedule.BulkPlantDetails.City,
                                                Country = country,
                                                CountyName = schedule.BulkPlantDetails.CountyName,
                                                IsAddressAvailable = true,
                                                Latitude = schedule.BulkPlantDetails.Latitude,
                                                Longitude = schedule.BulkPlantDetails.Longitude,
                                                SiteName = string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.SiteName) ? schedule.BulkPlantDetails.Address : schedule.BulkPlantDetails.SiteName,
                                                State = state,
                                                ZipCode = schedule.BulkPlantDetails.ZipCode
                                            };
                                        }
                                        else
                                        {
                                            apiResponse.Messages.Add(new ApiCodeMessages()
                                            { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(schedule.BulkPlantDetails.State)) });
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        apiResponse.Messages.Add(new ApiCodeMessages()
                                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(schedule.BulkPlantDetails.Country)) });
                                        break;
                                    }
                                }
                                else
                                {
                                    if (schedule.BulkPlantDetails.Latitude != 0 && schedule.BulkPlantDetails.Longitude != 0)
                                    {
                                        //get address from lat long
                                        var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(schedule.BulkPlantDetails.Latitude), Convert.ToDouble(schedule.BulkPlantDetails.Longitude));
                                        if (geoAddress != null)
                                        {
                                            scheduleDetail.BulkPlant = new DropAddressViewModel()
                                            {
                                                Address = geoAddress.Address,
                                                City = geoAddress.City,
                                                Country = new CountryViewModel() { Id = 0, Code = geoAddress.CountryCode },
                                                CountyName = geoAddress.CountyName,
                                                IsAddressAvailable = true,
                                                Latitude = schedule.BulkPlantDetails.Latitude,
                                                Longitude = schedule.BulkPlantDetails.Longitude,
                                                SiteName = schedule.BulkPlantDetails.SiteName,
                                                State = new StateViewModel() { Id = 0, Code = geoAddress.StateCode },
                                                ZipCode = geoAddress.ZipCode
                                            };

                                            var country = Context.DataContext.MstCountries.Where(t => t.IsActive &&
                                                            (t.Name.ToLower().Contains(geoAddress.CountryName.ToLower()) || t.Code.ToLower().Equals(geoAddress.CountryCode.ToLower())))
                                            .Select(t => new CountryViewModel() { Id = t.Id, Name = t.Name, Code = t.Code }).FirstOrDefault();
                                            if (country != null)
                                            {
                                                var state = Context.DataContext.MstStates.Where(t => t.IsActive &&
                                                             (t.Name.ToLower().Equals(geoAddress.StateName.ToLower()) || t.Code.ToLower().Equals(geoAddress.StateCode.ToLower())))
                                                             .Select(t => new StateViewModel() { Id = t.Id, Code = t.Code, Name = t.Name }).FirstOrDefault();
                                                if (state != null)
                                                {
                                                    scheduleDetail.BulkPlant.State.Id = state.Id;
                                                    scheduleDetail.BulkPlant.Country.Id = country.Id;
                                                }
                                                else
                                                {
                                                    apiResponse.Messages.Add(new ApiCodeMessages()
                                                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "Latitude - Longitude") });
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                apiResponse.Messages.Add(new ApiCodeMessages()
                                                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "Latitude - Longitude") });
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            apiResponse.Messages.Add(new ApiCodeMessages()
                                            { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "Latitude - Longitude") });
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "TerminalControlNumber or BulkPlant Details") });
                                        break;
                                    }
                                }
                            }
                        }

                        if (ordDetails.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
                        {
                            scheduleDetail.SupplierCompanyId = ordDetails.SupplierCompanyId;
                            scheduleDetail.SupplierCompanyName = ordDetails.SupplierCompanyName;
                        }
                        else
                        {
                            scheduleDetail.SupplierCompanyId = ordDetails.AcceptedCompanyId;
                            scheduleDetail.SupplierCompanyName = ordDetails.AcceptedCompanyName;
                        }

                        //validate job time zone
                        DateTime.TryParse(apiRequestModel.ScheduleDate, out DateTime scheduleDateTime);
                        var jobTimeZoneDate = DateTimeOffset.Now.ToTargetDateTimeOffset(ordDetails.TimeZoneName);
                        if (scheduleDateTime < jobTimeZoneDate.Date)
                        {
                            apiResponse.Messages.Add(new ApiCodeMessages()
                            { Code = Constants.ApiCodeRQ02, Message = Resource.errMessageScheduleDateCannotBeLessThanTodaysDate });
                        }

                        if (carrierOrderIdList.Any())
                        {
                            //t.OrderId != orderId && t.Date.Date == scheduleDateTime &&
                            if (carrierOrderIdList.Any(t => t.CarrierOrderId == schedule.CarrierOrderID))
                            {
                                apiResponse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgUniqueCarrierOrderIdRequired, schedule.CarrierOrderID) });
                                break;
                            }
                        }

                        resultViewModel.ScheduleDetails.Add(scheduleDetail);
                    }
                    else
                        break;
                }
            }

            if (resultViewModel.ScheduleDetails.Any())
            {
                //validate PO and carrierOrderId
                if (resultViewModel.ScheduleDetails.Select(t => t.PoNumber).Distinct().Count() != resultViewModel.ScheduleDetails.Select(t => t.CarrierOrderID).Distinct().Count())
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgUniqueParameterIsRequired, "CarrierOrderID") });
                }

                var tcnFromAPI = apiRequestModel.ScheduleDetails.Where(t => t.TerminalControlNumber != null && t.TerminalControlNumber.Trim() != "").Select(t => new TPDTerminalDetails() { APITcn = t.TerminalControlNumber.ToLower() }).Distinct().ToList();
                var tcnlist = tcnFromAPI.Select(t => t.APITcn.ToLower()).ToList();

                var terminalDetailsFromAlias = Context.DataContext.TerminalCompanyAliases.Where(t => t.CreatedByCompanyId == apiUserContext.CompanyId && t.TerminalId != null
                                                        && t.IsActive && t.TerminalSupplierId == null && tcnlist.Contains(t.AssignedTerminalId.ToLower()))
                                                        .Select(t => new
                                                        {
                                                            t.AssignedTerminalId,
                                                            t.TerminalId,
                                                            TerminalName = t.MstExternalTerminal.Name,
                                                            t.MstExternalTerminal.Address,
                                                            t.MstExternalTerminal.City,
                                                            State = new StateViewModel() { Id = t.MstExternalTerminal.StateId, Code = t.MstExternalTerminal.StateCode },
                                                            t.MstExternalTerminal.ZipCode,
                                                            Country = new CountryViewModel() { Code = t.MstExternalTerminal.CountryCode },
                                                            t.MstExternalTerminal.CountyName,
                                                            t.MstExternalTerminal.Latitude,
                                                            t.MstExternalTerminal.Longitude
                                                        }).ToList();
                if (terminalDetailsFromAlias.Any())
                {
                    foreach (var tcn in tcnFromAPI)
                    {
                        var termAlias = terminalDetailsFromAlias.Where(t => t.AssignedTerminalId.ToLower() == tcn.APITcn.ToLower()).FirstOrDefault();
                        if (termAlias != null)
                        {
                            tcn.Terminal = new DropAddressViewModel()
                            {
                                SiteId = termAlias.TerminalId.Value,
                                SiteName = termAlias.TerminalName,
                                Address = termAlias.Address,
                                City = termAlias.City,
                                State = termAlias.State,
                                ZipCode = termAlias.ZipCode,
                                Country = termAlias.Country,
                                CountyName = termAlias.CountyName,
                                Latitude = termAlias.Latitude,
                                Longitude = termAlias.Longitude
                            };
                        }
                    }
                }

                tcnlist = tcnFromAPI.Where(t => t.Terminal.SiteId == 0).Select(t => t.APITcn.ToLower()).ToList();

                var terminalDetailsFromTerminalTable = Context.DataContext.MstExternalTerminals.
                                                            Where(t => t.IsActive && t.ControlNumber != null && t.ControlNumber != string.Empty
                                                                    && (tcnlist.Contains(t.ControlNumber.ToLower()) || tcnlist.Contains(t.Name.ToLower())))
                                                            .Select(t => new
                                                            {
                                                                TerminalId = t.Id,
                                                                TerminalName = t.Name,
                                                                t.Address,
                                                                t.City,
                                                                State = new StateViewModel() { Id = t.StateId, Code = t.StateCode },
                                                                t.ZipCode,
                                                                Country = new CountryViewModel() { Code = t.CountryCode },
                                                                t.CountyName,
                                                                t.Latitude,
                                                                t.Longitude,
                                                                t.ControlNumber
                                                            })
                                                            .ToList();
                if (terminalDetailsFromTerminalTable.Any())
                {
                    foreach (var item in tcnFromAPI)
                    {
                        var termAlias = terminalDetailsFromTerminalTable.SingleOrDefault(t => (t.ControlNumber.ToLower() == item.APITcn.ToLower()));
                        if (termAlias != null)
                        {
                            item.Terminal = new DropAddressViewModel()
                            {
                                SiteId = termAlias.TerminalId,
                                SiteName = termAlias.TerminalName,
                                Address = termAlias.Address,
                                City = termAlias.City,
                                State = termAlias.State,
                                ZipCode = termAlias.ZipCode,
                                Country = termAlias.Country,
                                CountyName = termAlias.CountyName,
                                Latitude = termAlias.Latitude,
                                Longitude = termAlias.Longitude
                            };
                        }
                    }
                }

                if (tcnFromAPI.Any(t => t.Terminal.SiteId == 0))
                {
                    foreach (var item in tcnFromAPI.Where(t => t.Terminal.SiteId == 0))
                    {
                        apiResponse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, $"TerminalControlNumber {item.APITcn}") });
                    }
                }
                else
                {
                    foreach (var item in resultViewModel.ScheduleDetails.Where(t => t.TCNFromAPI != null))
                    {
                        var terminal = tcnFromAPI.Where(t => t.APITcn.ToLower() == item.TCNFromAPI.ToLower()).FirstOrDefault();
                        if (terminal != null)
                        {
                            item.Terminal = new DropdownDisplayItem() { Id = terminal.Terminal.SiteId, Name = terminal.Terminal.SiteName };
                            resultViewModel.Terminals.Add(terminal.Terminal);
                        }
                    }
                }
            }

            return resultViewModel;
        }

        private void ValidateRequireParameters(TPDCreateScheduleViewModel apiRequestModel, ApiResponseViewModel apiResponse)
        {
            if (string.IsNullOrWhiteSpace(apiRequestModel.DriverEmail))
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = string.Format(Resource.errMsgParameterIsRequired, "Driver Email")
                });
            }

            if (string.IsNullOrWhiteSpace(apiRequestModel.ScheduleDate))
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.ScheduleDate)) });
            }
            else
            { //ValidateDate(nameof(schedule.ScheduleDate), schedule.ScheduleDate, apiResonse);
                if (!DateTime.TryParse(apiRequestModel.ScheduleDate, out DateTime dateTime))
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ03, Message = string.Format(Resource.errMsgParameterFormatIsInvalid, nameof(apiRequestModel.ScheduleDate)) });
                }


            }

            if (string.IsNullOrWhiteSpace(apiRequestModel.ScheduleStartTime))
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.ScheduleStartTime)) });
            }
            else
                ValidateTimeParameter(nameof(apiRequestModel.ScheduleStartTime), apiRequestModel.ScheduleStartTime, apiResponse);

            if (string.IsNullOrWhiteSpace(apiRequestModel.ScheduleEndTime))
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(apiRequestModel.ScheduleEndTime)) });
            }
            else
                ValidateTimeParameter(nameof(apiRequestModel.ScheduleEndTime), apiRequestModel.ScheduleEndTime, apiResponse);

            if (!string.IsNullOrWhiteSpace(apiRequestModel.ScheduleStartTime) && !string.IsNullOrWhiteSpace(apiRequestModel.ScheduleEndTime))
                ValidateStartAndEndTimeParameter(apiRequestModel.ScheduleStartTime, apiRequestModel.ScheduleEndTime, nameof(apiRequestModel.ScheduleStartTime), nameof(apiRequestModel.ScheduleEndTime), apiResponse);

            if (apiRequestModel.ScheduleDetails == null || !apiRequestModel.ScheduleDetails.Any())
            {
                apiResponse.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = string.Format(Resource.errMsgParameterIsRequired, "Schedule Details")
                });
            }
            else
            {
                if (apiRequestModel.ScheduleDetails.Count > 15)
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = Resource.errMsgExceedsMaxAllowedCount });
                }

                foreach (var schedule in apiRequestModel.ScheduleDetails)
                {
                    if (string.IsNullOrWhiteSpace(schedule.PoNumber))
                    {
                        if (string.IsNullOrWhiteSpace(schedule.LocationID) && string.IsNullOrWhiteSpace(schedule.ProductID) && string.IsNullOrWhiteSpace(schedule.CustomerID))
                        {
                            apiResponse.Messages.Add(new ApiCodeMessages()
                            { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "PO Number Or CustomerID - LocationId - Product") });
                        }
                    }

                    if (schedule.ScheduleQuantity <= 0)
                    {
                        apiResponse.Messages.Add(new ApiCodeMessages()
                        { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(schedule.ScheduleQuantity)) });
                    }

                    if (string.IsNullOrWhiteSpace(schedule.TerminalControlNumber))
                    {
                        if (schedule.BulkPlantDetails != null)
                        {
                            if (!string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.SiteName))
                            {
                                //verify while processing                                
                            }
                            else
                            {
                                apiResponse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "TerminalControlNumber or BulkPlant Details") });

                                //if (string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.Address) || string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.City)
                                //    || string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.State) || string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.Country)
                                //    || string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.CountyName) || string.IsNullOrWhiteSpace(schedule.BulkPlantDetails.ZipCode)
                                //    )
                                //{
                                //    apiResonse.Messages.Add(new ApiCodeMessages()
                                //    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "TerminalControlNumber or BulkPlant Details") });
                                //}
                            }
                        }
                        else
                        {
                            apiResponse.Messages.Add(new ApiCodeMessages()
                            { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, nameof(schedule.TerminalControlNumber)) });
                        }
                    }
                }
                var carrirerOrderIds = apiRequestModel.ScheduleDetails.Where(t => !string.IsNullOrWhiteSpace(t.CarrierOrderID)).Select(t => t.CarrierOrderID).Count();
                if (carrirerOrderIds != apiRequestModel.ScheduleDetails.Count)
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgParameterIsRequired, "CarrierOrderID") });
                }
                //check duplicate carrierorderid in request
                var IsDuplicateCarrierOrderId = apiRequestModel.ScheduleDetails.GroupBy(x => x.CarrierOrderID).Any(g => g.Count() > 1);
                if (IsDuplicateCarrierOrderId)
                {
                    apiResponse.Messages.Add(new ApiCodeMessages()
                    { Code = Constants.ApiCodeRQ02, Message = string.Format(Resource.errMsgUniqueParameterIsRequired, "CarrierOrderID") });
                }
            }

        }
        #endregion

        public async Task<ApiResponseViewModel> UpdateScheduleStatus(TPDScheduleStatusViewModel apiRequestModel, string token)
        {
            ApiResponseViewModel response = new ApiResponseViewModel(Status.Failed);
            try
            {
                if (apiRequestModel != null)
                {
                    //get userdetails from token
                    var authDomain = new AuthenticationDomain(this);
                    var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
                    if (apiUserContext != null)
                    {
                        ValidateScheduleUpdateRequest(apiRequestModel, response, apiUserContext);
                        if (!response.Messages.Any())
                        {
                            var distpatchDomain = new DispatchDomain(this);
                            response = await distpatchDomain.UpdateDeliveryStatusFromAPI(apiUserContext, apiRequestModel);
                        }
                    }
                    else
                        response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ01, Message = Resource.errMsgInvalidToken });
                }
                else
                    response.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRQ04, Message = Resource.errMsgParmeterNotMatch });
            }
            catch (Exception ex)
            {
                string carrierOrderId = apiRequestModel != null ? apiRequestModel.CarrierOrderID : string.Empty;
                int tfxScheduleID = apiRequestModel != null ? apiRequestModel.TFXScheduleID : int.MinValue;
                LogManager.Logger.WriteException("DispatchDomain", "UpdateScheduleStatus", $"{ex.Message} carrierOrderId={carrierOrderId} deliveryScheduleId={tfxScheduleID}", ex);
            }

            return response;
        }
        private void ValidateScheduleUpdateRequest(TPDScheduleStatusViewModel apiRequestModel, ApiResponseViewModel response, UserContext apiUserContext)
        {
            if (string.IsNullOrWhiteSpace(apiRequestModel.CarrierOrderID) && apiRequestModel.TFXScheduleID <= 0)
            {
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = string.Format(Resource.errMsgParameterIsRequired, "CarrierOrderID or TFXScheduleID")
                });
            }

            if (!string.IsNullOrWhiteSpace(apiRequestModel.CarrierOrderID) && apiRequestModel.TFXScheduleID > 0)
            {
                bool isValidCarrierOrderIdAndScheduleId = Context.DataContext.DeliveryScheduleXTrackableSchedules.
                                                           Any(t => t.Order.AcceptedCompanyId == apiUserContext.CompanyId && t.CarrierOrderId == apiRequestModel.CarrierOrderID && t.Id == apiRequestModel.TFXScheduleID && t.IsActive
                                                           && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled
                                                           && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled);
                if (!isValidCarrierOrderIdAndScheduleId)
                    response.Messages.Add(new ApiCodeMessages()
                    {
                        Code = Constants.ApiCodeRQ02,
                        Message = string.Format(Resource.errMsgInvalidCarrierOrderIdOrScheduleId, apiRequestModel.CarrierOrderID, apiRequestModel.TFXScheduleID)
                    });
            }

            if (apiRequestModel.DriversLatestLat.GetValue<decimal>() == 0 || apiRequestModel.DriversLatestLong.GetValue<decimal>() == 0)
            {
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = string.Format(Resource.errMsgParameterIsRequired, "DriversLatestLat and DriversLatestLong")
                });
            }
            else
            {
                var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(apiRequestModel.DriversLatestLat), Convert.ToDouble(apiRequestModel.DriversLatestLong));
                if (geoAddress != null)
                {
                    if (!string.IsNullOrWhiteSpace(geoAddress.CountryCode)
                        && !(geoAddress.CountryCode.ToLower().Equals("us") || geoAddress.CountryCode.ToLower().Equals("ca")))
                    {
                        response.Messages.Add(new ApiCodeMessages()
                        {
                            Code = Constants.ApiCodeRQ02,
                            Message = string.Format(Resource.errMsgParameterIsRequired, "DriversLatestLat and DriversLatestLong")
                        });
                    }
                }
            }

            if (apiRequestModel.DeliveryScheduleStatus != 1 && apiRequestModel.DeliveryScheduleStatus != 2 && apiRequestModel.DeliveryScheduleStatus != 3
                && apiRequestModel.DeliveryScheduleStatus != 4 && apiRequestModel.DeliveryScheduleStatus != 5)
            {
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = string.Format(Resource.errMsgParameterIsRequired, "DeliveryScheduleStatus")
                });
            }

            if (!string.IsNullOrWhiteSpace(apiRequestModel.ExternalRefID) && apiRequestModel.ExternalRefID.Length > 256)
            {
                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ02,
                    Message = string.Format(Resource.errMsgParameterIsRequired, "ExternalRefID, Must be less than 256 characters")
                });
            }
        }
        private void ValidateQuantities(ApiResponseViewModel apiResonse, TPDDropDetails dropInfo, TPDInvoiceViewModel tpdInvVM)
        {
            try
            {
                if (tpdInvVM.DropDetails != null && tpdInvVM.DropDetails.Any())
                {
                    foreach (var dropListItem in tpdInvVM.DropDetails)
                    {
                        if (dropListItem.BolDetails.AnyAndNotNull() || dropListItem.LiftDetails.AnyAndNotNull())
                        {
                            decimal bolNetQuantity = dropListItem.BolDetails.Sum(t => t.BolNet);
                            decimal liftNetQuantity = dropListItem.LiftDetails.Sum(t => t.LiftNet);
                            decimal bolGrossQuantity = dropListItem.BolDetails.Sum(t => t.BolGross);
                            decimal liftGrossQuantity = dropListItem.LiftDetails.Sum(t => t.LiftGross);
                            decimal bolDelivered = dropListItem.BolDetails.Sum(t => t.BolDelivered);
                            decimal liftDelivered = dropListItem.LiftDetails.Sum(t => t.LiftDelivered);
                            decimal totalNetQuantity = bolNetQuantity + liftNetQuantity;
                            decimal totalGrossQuantity = bolGrossQuantity + liftGrossQuantity;
                            decimal totalDeliveredQuantity = bolDelivered + liftDelivered;

                            var isBolDelivered = dropListItem.BolDetails.Where(t => t.BolNet > 0).All(t => t.BolDelivered > 0);
                            var isLiftDelivered = dropListItem.LiftDetails.Where(t => t.LiftNet > 0).All(t => t.LiftDelivered > 0);

                            if (isBolDelivered && isLiftDelivered && (dropListItem.TotalDropQuantity != totalDeliveredQuantity))
                            {
                                apiResonse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ02, Message = Resource.errMsgTotalDroppedQuantity });
                            }
                            if (totalNetQuantity > 0 && totalGrossQuantity > 0 && (dropListItem.TotalDropQuantity > totalGrossQuantity) && (dropListItem.TotalDropQuantity > totalNetQuantity))
                            {
                                apiResonse.Messages.Add(new ApiCodeMessages()
                                { Code = Constants.ApiCodeRQ02, Message = Resource.errMsgDroppedQuantity });
                            }
                            if (dropListItem.BolDetails.AnyAndNotNull())
                            {
                                var isBolDeliveredDetail = dropListItem.BolDetails.Where(t => t.BolNet > 0 && t.BolGross > 0).Any(t => t.BolDelivered > t.BolNet && t.BolDelivered > t.BolGross);
                                if (isBolDeliveredDetail)
                                {
                                    apiResonse.Messages.Add(new ApiCodeMessages()
                                    { Code = Constants.ApiCodeRQ02, Message = Resource.errorMsgBolDelivered });
                                }
                            }
                            if (dropListItem.LiftDetails.AnyAndNotNull())
                            {
                                var isLiftDeliveredDetail = dropListItem.LiftDetails.Where(t => t.LiftNet > 0 && t.LiftGross > 0).Any(t => t.LiftDelivered > t.LiftNet && t.LiftDelivered > t.LiftGross);
                                if (isLiftDeliveredDetail)
                                {
                                    apiResonse.Messages.Add(new ApiCodeMessages()
                                    { Code = Constants.ApiCodeRQ02, Message = Resource.errorMsgLiftDelivered });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ValidateQuantities", "InvoiceTPDDomain", ex.Message, ex);
            }
        }
    }
}
