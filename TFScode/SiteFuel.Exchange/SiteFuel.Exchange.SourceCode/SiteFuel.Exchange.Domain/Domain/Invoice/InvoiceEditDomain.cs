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
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class InvoiceEditDomain : InvoiceCommonDomain
    {
        public InvoiceEditDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public InvoiceEditDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<StatusViewModel> InvoiceEditAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new StatusViewModel();
            try
            {
                InvoiceEditRequestViewModel invoiceEditRequest = null;

                if (manualInvoiceModel.IsQuanityOrDateChanged)
                {
                    invoiceEditRequest = await GetInvoiceEditViewModelForNewDateAsync(userContext, manualInvoiceModel);
                    if (invoiceEditRequest == null)
                    {
                        response.StatusMessage = Resource.errMessageSelectedDateCannotBeSet;
                        return response;
                    }
                }
                else
                {
                    invoiceEditRequest = await GetInvoiceEditViewModelAsync(userContext, manualInvoiceModel);
                    if (invoiceEditRequest == null)
                    {
                        response.StatusMessage = Resource.errMessageSelectedDateCannotBeSet;
                        return response;
                    }
                }

                if (manualInvoiceModel.TaxType == TaxType.Standard && !invoiceEditRequest.IsTaxServiceSucceeded)
                {
                    response.StatusMessage = Resource.errMessageUpdateTaxFailed;
                    return response;
                }

                if (!IsDigitalDropTicket(manualInvoiceModel.InvoiceTypeId))
                {
                    CheckForProcessingFeeOnTotalAmount(invoiceEditRequest.InvoiceModel);
                }
                var updateResponse = await UpdateInvoiceAsync(invoiceEditRequest);
                response.StatusCode = updateResponse.StatusCode;
                if (updateResponse.StatusCode == Status.Failed)
                {
                    return response;
                }
                else
                {
                    await AddBrokerInvoicesToQueueServiceForEdit(updateResponse.InvoiceId, manualInvoiceModel);
                }

                var notificationDomain = new NotificationDomain(this);
                await notificationDomain.AddNotificationEventAsync(EventType.InvoiceUpdated, updateResponse.InvoiceHeaderId, userContext.Id);
                await SetSystemAutoClosedOrderWhileEditing(invoiceEditRequest, updateResponse);

                // await SendInvoiceTaxValueChangedMessage(invoice)
                UpdateManualInvoiceViewModel(manualInvoiceModel, invoiceEditRequest, updateResponse);
                await SetInvoiceUpdatedNewsfeed(userContext, manualInvoiceModel, invoiceEditRequest.JobCompanyId);

                response.EntityId = updateResponse.InvoiceId;
                response.StatusMessage = IsDigitalDropTicket(manualInvoiceModel.InvoiceTypeId)
                    ? Resource.errMessageDdtUpdatedSuccess : Resource.errMessageInvoiceUpdatedSuccess;
                UpdateInvoiceActionResponseStatus(updateResponse.IsDtnUploaded, response);
                await new BillingStatementDomain(this).CreateBillingStatementForEditedInvoice(invoiceEditRequest.InvoiceModel.InvoiceNumberId, userContext.Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "InvoiceEditAsync", ex.Message, ex);
            }
            return response;
        }

        private int GetInvoiceTypeId(CreationMethod creationMethod, int defaultInvoiceType)
        {
            var invoiceTypeId = defaultInvoiceType;
            if (creationMethod == CreationMethod.Mobile)
            {
                if (defaultInvoiceType == (int)InvoiceType.Manual)
                    invoiceTypeId = (int)InvoiceType.MobileApp;
                else
                    invoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
            }
            return invoiceTypeId;
        }

        private async Task AddBrokerInvoicesToQueueServiceForEdit(int invoiceId, ManualInvoiceViewModel manualInvoiceModel)
        {
            try
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.IsActive && t.Id == invoiceId && (t.BrokeredChainId != "" && t.BrokeredChainId != null))
                                                                .Select(t => new { t.OrderId, t.BrokeredChainId, t.WaitingFor })
                                                                .FirstOrDefaultAsync();

                if (invoice != null && invoice.OrderId != null)
                {
                    var brokeredOrderInfo = new Dictionary<int, int>();

                    GetBrokerOrdersTillOriginalOrder(invoice.OrderId.Value, brokeredOrderInfo);
                    foreach (var brokeredOrder in brokeredOrderInfo)
                    {
                        var brokeredInvoice = await Context.DataContext.Invoices.Where(t => t.OrderId == brokeredOrder.Key && t.BrokeredChainId == invoice.BrokeredChainId &&
                                                                                            t.WaitingFor != (int)WaitingAction.Nothing && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
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
                                                                           t.DroppedGallons,
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
                            var bolDetails = brokeredInvoice.InvoiceXBolDetails?.Select(t2 => t2.InvoiceFtlDetail)?.FirstOrDefault();

                            var brokerManualInvoiceModel = new ManualInvoiceViewModel();
                            brokerManualInvoiceModel = brokerManualInvoiceModel.CopyObject(manualInvoiceModel);
                            brokerManualInvoiceModel.InvoiceId = brokeredInvoice.InvoiceId;
                            brokerManualInvoiceModel.OrderId = brokeredOrder.Key;
                            brokerManualInvoiceModel.BrokeredChainId = brokeredInvoice.BrokeredChainId;
                            brokerManualInvoiceModel.InvoiceNumber.Id = brokeredInvoice.InvoiceNumberId;
                            brokerManualInvoiceModel.InvoiceHeaderId = brokeredInvoice.InvoiceHeaderId;
                            brokerManualInvoiceModel.DisplayInvoiceNumber = brokeredInvoice.DisplayInvoiceNumber;
                            brokerManualInvoiceModel.PoNumber = brokeredInvoice.PoNumber;

                            var defaultInvoiceType = GetInvoiceTypeId(manualInvoiceModel.CreationMethod, brokeredInvoice.DefaultInvoiceType);
                            if ((defaultInvoiceType == (int)InvoiceType.Manual || defaultInvoiceType == (int)InvoiceType.MobileApp) &&
                               (manualInvoiceModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || manualInvoiceModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
                                brokerManualInvoiceModel.InvoiceTypeId = manualInvoiceModel.InvoiceTypeId;
                            else
                                brokerManualInvoiceModel.InvoiceTypeId = defaultInvoiceType;

                            brokerManualInvoiceModel.PaymentTermId = brokeredInvoice.PaymentTermId;
                            brokerManualInvoiceModel.NetDays = brokeredInvoice.NetDays;
                            brokerManualInvoiceModel.PaymentDueDate = brokeredInvoice.PaymentDueDate.ToString(Resource.constFormatDate);
                            brokerManualInvoiceModel.PaymentMethod = brokeredInvoice.PaymentMethod;
                            brokerManualInvoiceModel.SupplierAllowance = brokeredInvoice.SupplierAllowance;
                            brokerManualInvoiceModel.OriginalInvoiceId = brokeredInvoice.OriginalInvoiceId;
                            brokerManualInvoiceModel.TerminalId = bolDetails != null && bolDetails.TerminalId.HasValue ? bolDetails.TerminalId.Value : (brokeredInvoice.OrderTerminalId ?? brokeredInvoice.FRTerminalId ?? 0);

                            var helperDomain = new HelperDomain(this);
                            brokerManualInvoiceModel.TotalInvoiceAmount = helperDomain.GetInvoiceAmount(manualInvoiceModel.InvoiceTypeId, brokeredInvoice.TotalBasicAmount, brokeredInvoice.TotalTaxAmount, brokeredInvoice.TotalDiscountAmount, brokeredInvoice.TotalFeeAmount);

                            var brokeredOrderLevelFees = GetInvoiceFuelFees(brokeredInvoice.FuelRequestFees, brokeredInvoice.DroppedGallons);
                            if (manualInvoiceModel.CreationMethod == CreationMethod.APIUpload || manualInvoiceModel.CreationMethod == CreationMethod.BulkUploaded)
                            {
                                if (brokeredInvoice.OrderEnforcementId != OrderEnforcement.EnforceOrderLevelValues)
                                {
                                    brokerManualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => t.FeeTypeId == ((int)FeeType.ProcessingFee).ToString() || t.FeeTypeId == ((int)FeeType.SurchargeFreightFee).ToString());
                                    var ccAndSurchargeFee = brokeredOrderLevelFees.FuelRequestFees.Where(t => t.FeeTypeId == ((int)FeeType.ProcessingFee).ToString() || t.FeeTypeId == ((int)FeeType.SurchargeFreightFee).ToString()).ToList();
                                    brokerManualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.AddRange(ccAndSurchargeFee);
                                }
                                else
                                {
                                    brokerManualInvoiceModel.FuelDeliveryDetails.FuelFees = brokeredOrderLevelFees;
                                }
                            }
                            else
                            {
                                brokerManualInvoiceModel.FuelDeliveryDetails.FuelFees = brokeredOrderLevelFees;
                            }

                            UserContext brokerUserContext = new UserContext() { Id = brokeredInvoice.AcceptedBy, CompanyId = brokeredInvoice.AcceptedCompanyId };
                            var response = await AddEditedInvoiceToQueueService(brokerUserContext, brokerManualInvoiceModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "AddBrokerInvoicesToQueueServiceForEdit", ex.Message, ex);
            }
        }

        //private void UpdateInvoiceImageDetails(ManualInvoiceViewModel manualInvoiceModel, ManualInvoiceViewModel brokerManualInvoiceModel)
        //{
        //    if (manualInvoiceModel.InvoiceImage != null && !manualInvoiceModel.InvoiceImage.IsRemoved)
        //    {
        //        brokerManualInvoiceModel.InvoiceImage = manualInvoiceModel.InvoiceImage;
        //    }
        //    if (manualInvoiceModel.BolImage != null && !manualInvoiceModel.BolImage.IsRemoved)
        //    {
        //        brokerManualInvoiceModel.BolImage = manualInvoiceModel.BolImage;
        //    }
        //    if (manualInvoiceModel.SignatureImage != null && !manualInvoiceModel.SignatureImage.IsRemoved)
        //    {
        //        brokerManualInvoiceModel.SignatureImage = manualInvoiceModel.SignatureImage;
        //    }
        //    if (manualInvoiceModel.AdditionalImage != null && !manualInvoiceModel.AdditionalImage.IsRemoved)
        //    {
        //        brokerManualInvoiceModel.AdditionalImage = manualInvoiceModel.AdditionalImage;
        //    }
        //}

        public async Task<StatusViewModel> AddEditedInvoiceToQueueService(UserContext userContext, ManualInvoiceViewModel manualinvoiceViewModel)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            if (manualinvoiceViewModel != null)
            {
                try
                {
                    manualinvoiceViewModel.InvoiceChainId = Guid.NewGuid().ToString();
                    response = await SaveEditInvoiceFileToBlob(userContext, manualinvoiceViewModel);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceEditDomain", "AddEditedInvoiceToQueueService", ex.Message + "brokerChainId:" + manualinvoiceViewModel.BrokeredChainId, ex);
                }
            }
            return response;
        }

        private async Task<StatusViewModel> SaveEditInvoiceFileToBlob(UserContext userContext, ManualInvoiceViewModel manualinvoiceViewModel)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            try
            {
                string json = JsonConvert.SerializeObject(manualinvoiceViewModel, new ManualInvoiceModelConverter());
                byte[] byteArray = Encoding.ASCII.GetBytes(json);
                MemoryStream stream = new MemoryStream(byteArray);
                var azureBlob = new AzureBlobStorage();
                var fileName = await azureBlob.SaveBlobAsync(stream, $"{userContext.Id}-{manualinvoiceViewModel.InvoiceChainId}.json", BlobContainerType.EditInvoice.ToString().ToLower());

                QueueProcessType queueProcessType = QueueProcessType.EditBrokerInvoice;
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    var queueDomain = new QueueMessageDomain();
                    var queueRequest = GetQueueEventForEditBrokeredInvoice(userContext, queueProcessType, fileName);
                    var queueId = queueDomain.EnqeueMessage(queueRequest);
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.succcessMsgInvoiceDDTSubmitted;
                }
                else
                {
                    response.StatusMessage = response.StatusMessage = Resource.errMessageErrorInAzureServer;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "SaveEditInvoiceFileToBlob", ex.Message, ex);
            }

            return response;
        }

        private QueueMessageViewModel GetQueueEventForEditBrokeredInvoice(UserContext userContext, QueueProcessType queueProcessType, string blobStoragePath)
        {
            var jsonViewModel = new EditInvoiceProcessorModel();
            jsonViewModel.UserId = userContext.Id;
            jsonViewModel.CompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = queueProcessType,
                JsonMessage = json
            };
        }

        public Dictionary<int, int> GetBrokerOrdersTillOriginalOrder(int endSupplierOrderId, Dictionary<int, int> orderList)
        {
            var thisOrder = Context.DataContext.Orders.Where(t => t.Id == endSupplierOrderId)
                            .Select(t => new
                            {
                                t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId,
                                t.FuelRequest,
                                t.FuelRequest.FuelRequestDetail.DeliveryTypeId
                            }).FirstOrDefault();

            if ((thisOrder.FuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest || thisOrder.FuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
                && thisOrder.FuelRequest.FuelRequest1 != null
                && thisOrder.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
            {
                bool isSingleDeliveryClosedOrder = false;
                if (thisOrder.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery
                    && (thisOrder.StatusId == (int)OrderStatus.PartiallyClosed || thisOrder.StatusId == (int)OrderStatus.Closed))
                {
                    isSingleDeliveryClosedOrder = true;
                }
                var childRequest = thisOrder.FuelRequest.GetParentFuelRequest().FuelRequest1;
                var brokeredOrder = childRequest.Orders.LastOrDefault();
                brokeredOrder = (brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open || isSingleDeliveryClosedOrder) ? brokeredOrder : GetConnectingBuyerOrder(brokeredOrder);
                if (brokeredOrder != null && (brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open || isSingleDeliveryClosedOrder))
                {
                    orderList.Add(brokeredOrder.Id, brokeredOrder.AcceptedBy);
                    return GetBrokerChainOrderListTillOriginalOrder(brokeredOrder.Id, orderList);
                }
                else
                {
                    return orderList;
                }
            }
            else
            {
                return orderList;
            }
        }

        private Order GetConnectingBuyerOrder(Order order)
        {
            if (order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
            {
                return order;
            }
            else
            {
                var parentRequest = order.FuelRequest.FuelRequest1;
                if (parentRequest != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                {
                    var parentOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.OrderByDescending(t => t.Id).FirstOrDefault();
                    return GetConnectingBuyerOrder(parentOrder);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<string>> EditBrokeredInvoiceFromQueueService(EditInvoiceProcessorModel viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("InvoiceEditDomain", "EditBrokeredInvoiceFromQueueService"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    if (!string.IsNullOrWhiteSpace(viewModel.FileUploadPath))
                    {
                        var brokerManualInvoiceViewModel = GetBrokeredInvoiceViewModelFromBlob(viewModel);
                        UserContext userContext = new UserContext() { Id = viewModel.UserId, CompanyId = viewModel.CompanyId };
                        StatusViewModel response = null;

                        response = await BrokeredInvoiceEditAsync(userContext, brokerManualInvoiceViewModel);

                        if (response.StatusCode == Status.Failed)
                        {
                            errorInfo.Add(response.StatusMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                    {
                        LogManager.Logger.WriteException("InvoiceEditDomain", "EditBrokeredInvoiceFromQueueService", ex.Message, ex);
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

        public ManualInvoiceViewModel GetBrokeredInvoiceViewModelFromBlob(EditInvoiceProcessorModel viewModel)
        {
            ManualInvoiceViewModel invoiceViewModel = null;
            using (var tracer = new Tracer("InvoiceEditDomain", "GetBrokeredInvoiceViewModelFromBlob"))
            {   
                if (!string.IsNullOrWhiteSpace(viewModel.FileUploadPath))
                {
                    var azureBlob = new AzureBlobStorage();
                    var fileStream = azureBlob.DownloadBlob(viewModel.FileUploadPath, BlobContainerType.EditInvoice.ToString().ToLower());
                    if (fileStream != null)
                    {
                        string editInvoiceJson = new StreamReader(fileStream).ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(editInvoiceJson))
                        {
                            invoiceViewModel = JsonConvert.DeserializeObject<ManualInvoiceViewModel>(editInvoiceJson);
                        }
                    }
                }
            }
            return invoiceViewModel;
        }

        public async Task<StatusViewModel> BrokeredInvoiceEditAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new StatusViewModel();
            try
            {
                InvoiceEditRequestViewModel invoiceEditRequest = null;
                if (manualInvoiceModel.IsQuanityOrDateChanged)
                {
                    invoiceEditRequest = await GetBrokeredInvoiceEditViewModelForNewDateAsync(userContext, manualInvoiceModel);
                    if (invoiceEditRequest == null)
                    {
                        response.StatusMessage = Resource.errMessageSelectedDateCannotBeSet;
                        return response;
                    }
                }
                else
                {
                    invoiceEditRequest = await GetBrokeredInvoiceEditViewModelAsync(userContext, manualInvoiceModel);
                    if (invoiceEditRequest == null)
                    {
                        response.StatusMessage = Resource.errMessageSelectedDateCannotBeSet;
                        return response;
                    }
                }

                if (manualInvoiceModel.TaxType == TaxType.Standard && !invoiceEditRequest.IsTaxServiceSucceeded)
                {
                    response.StatusMessage = Resource.errMessageUpdateTaxFailed;
                    return response;
                }

                if (!IsDigitalDropTicket(manualInvoiceModel.InvoiceTypeId))
                {
                    CheckForProcessingFeeOnTotalAmount(invoiceEditRequest.InvoiceModel);
                }
                var updateResponse = await UpdateInvoiceAsync(invoiceEditRequest);
                response.StatusCode = updateResponse.StatusCode;
                if (updateResponse.StatusCode == Status.Failed)
                {
                    return response;
                }

                var notificationDomain = new NotificationDomain(this);
                await notificationDomain.AddNotificationEventAsync(EventType.InvoiceUpdated, updateResponse.InvoiceHeaderId, userContext.Id);
                await SetSystemAutoClosedOrderWhileEditing(invoiceEditRequest, updateResponse);

                // await SendInvoiceTaxValueChangedMessage(invoice)
                UpdateManualInvoiceViewModel(manualInvoiceModel, invoiceEditRequest, updateResponse);
                await SetInvoiceUpdatedNewsfeed(userContext, manualInvoiceModel, invoiceEditRequest.JobCompanyId);

                response.EntityId = updateResponse.InvoiceId;
                response.StatusMessage = IsDigitalDropTicket(manualInvoiceModel.InvoiceTypeId)
                    ? Resource.errMessageDdtUpdatedSuccess : Resource.errMessageInvoiceUpdatedSuccess;
                UpdateInvoiceActionResponseStatus(updateResponse.IsDtnUploaded, response);
                await new BillingStatementDomain(this).CreateBillingStatementForEditedInvoice(invoiceEditRequest.InvoiceModel.InvoiceNumberId, userContext.Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "InvoiceEditAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> InvoiceEditForOtherDetailsAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new StatusViewModel();
            try
            {
                InvoiceEditRequestViewModel invoiceEditRequest = null;

                if (manualInvoiceModel.TaxType == TaxType.Unknown)
                    manualInvoiceModel.TaxType = TaxType.Manual;

                invoiceEditRequest = await GetInvoiceEditViewModelAsync(userContext, manualInvoiceModel);
                if (invoiceEditRequest == null)
                {
                    response.StatusMessage = Resource.errMessageSelectedDateCannotBeSet;
                    return response;
                }

                if (manualInvoiceModel.TaxType == TaxType.Standard && !invoiceEditRequest.IsTaxServiceSucceeded)
                {
                    response.StatusMessage = Resource.errMessageUpdateTaxFailed;
                    return response;
                }

                CheckForProcessingFeeOnTotalAmount(invoiceEditRequest.InvoiceModel);
                var updateResponse = await UpdateInvoiceForOtherDetailsAsync(invoiceEditRequest);
                response.StatusCode = updateResponse.StatusCode;
                if (updateResponse.StatusCode == Status.Failed)
                {
                    return response;
                }

                await SetSystemAutoClosedOrderWhileEditing(invoiceEditRequest, updateResponse);
                UpdateManualInvoiceViewModel(manualInvoiceModel, invoiceEditRequest, updateResponse);

                response.EntityId = updateResponse.InvoiceId;
                response.StatusMessage = IsDigitalDropTicket(manualInvoiceModel.InvoiceTypeId)
                    ? Resource.errMessageDdtUpdatedSuccess : Resource.errMessageInvoiceUpdatedSuccess;
                UpdateInvoiceActionResponseStatus(updateResponse.IsDtnUploaded, response);
                await new BillingStatementDomain(this).CreateBillingStatementForEditedInvoice(invoiceEditRequest.InvoiceModel.InvoiceNumberId, userContext.Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "InvoiceEditUsingOtherDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<DealResponseViewModel> ConsolidatedInvoiceDealAsync(UserContext userContext, List<ManualInvoiceViewModel> manualInvoiceModels)
        {
            var response = new DealResponseViewModel();
            try
            {
                var lstInvoiceEditRequest = new List<InvoiceEditRequestViewModel>();
                foreach (var manualInvoiceModel in manualInvoiceModels)
                {
                    var invoiceEditRequest = new InvoiceEditRequestViewModel();
                    if (manualInvoiceModel.TaxType == TaxType.Unknown)
                        manualInvoiceModel.TaxType = TaxType.Manual;

                    invoiceEditRequest = await GetConsolidatedInvoiceDealViewModelAsync(userContext, manualInvoiceModel);

                    CheckForProcessingFeeOnTotalAmount(invoiceEditRequest.InvoiceModel);
                    lstInvoiceEditRequest.Add(invoiceEditRequest);
                }

                var lstUpdateResponse = await UpdateConsolidatedInvoiceDealAsync(lstInvoiceEditRequest);

                foreach (var invoiceEditRequest in lstInvoiceEditRequest)
                {
                    var updateResponseForOrder = lstUpdateResponse.FirstOrDefault(t => t.OrderId == invoiceEditRequest.OrderId);
                    await SetSystemAutoClosedOrderWhileEditing(invoiceEditRequest, updateResponseForOrder);
                }

                var updateResponse = lstUpdateResponse.FirstOrDefault(t => t.DealCreatedInvoiceId > 0);
                if (updateResponse != null)
                {
                    response = UpdateDealResponseViewModel(updateResponse);
                    response.EntityId = updateResponse.InvoiceId;
                    response.StatusMessage = IsDigitalDropTicket(updateResponse.InvoiceTypeId) ? Resource.errMessageDdtUpdatedSuccess : Resource.errMessageInvoiceUpdatedSuccess;
                    UpdateInvoiceActionResponseStatus(updateResponse.IsDtnUploaded, response);
                    await new BillingStatementDomain(this).CreateBillingStatementForEditedInvoice(updateResponse.InvoiceNumberId, userContext.Id);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "ConsolidatedInvoiceEditForOtherDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> InvoiceEditForInvoiceNumberAsync(UserContext userContext, int invoiceId, string newDisplayInvoiceNumber, int? orderId)
        {
            var response = new StatusViewModel();
            try
            {
                ConsolidatedInvoiceEditViewModel invoiceEditRequest = null;

                invoiceEditRequest = await GetInvoiceEditForEditInvoiceNumber(userContext, invoiceId, newDisplayInvoiceNumber);
                if (invoiceEditRequest?.invoiceModels != null && invoiceEditRequest.invoiceModels.Any())
                {
                    response = await UpdateInvoiceForEditNumberAsync(invoiceEditRequest, orderId);
                    if (response.StatusCode == Status.Failed)
                    {
                        return response;
                    }
                    response.StatusMessage = IsDigitalDropTicket(invoiceEditRequest.invoiceModels.FirstOrDefault().InvoiceTypeId)
                        ? Resource.errMessageDdtUpdatedSuccess : Resource.errMessageInvoiceUpdatedSuccess;
                    //UpdateInvoiceActionResponseStatus(updateResponse.IsDtnUploaded, response);

                    //await new BillingStatementDomain(this).CreateBillingStatementForEditedInvoice(invoiceEditRequest.InvoiceModel.InvoiceNumberId, userContext.Id);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "InvoiceEditForInvoiceNumberAsync", ex.Message, ex);
            }
            return response;
        }
        private async Task<ConsolidatedInvoiceEditViewModel> GetInvoiceEditForEditInvoiceNumber(UserContext userContext, int invoiceId, string newDisplayNumber, bool fromDDTUpdateImg = false)
        {
            var response = new ConsolidatedInvoiceEditViewModel();
            using (var tracer = new Tracer("InvoiceEditDomain", "GetInvoiceEditForEditInvoiceNumber"))
            {
                try
                {
                    var originalInvoices = await Context.DataContext.InvoiceHeaderDetails
                                            .Where(t => t.Invoices.Any(t1 => t1.Id == invoiceId))
                                            .Select(t => new { invoiceHeader = t, invoices = t.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active) })
                                            .FirstOrDefaultAsync();
                    if (originalInvoices?.invoices != null)
                    {
                        foreach (var originalInvoice in originalInvoices.invoices)
                        {
                            var invoice = new InvoiceModel(); ;

                            invoice = GetInvoiceModelFromOriginalInvoice(originalInvoice);

                            if (string.IsNullOrWhiteSpace(originalInvoice.ReferenceId))
                                invoice.ReferenceId = originalInvoice.InvoiceHeader.InvoiceNumber.Number;
                            else
                                invoice.ReferenceId = originalInvoice.ReferenceId;
                            //modify displaynumber and version
                            invoice.Version += 1;
                            if (!fromDDTUpdateImg)
                                invoice.DisplayInvoiceNumber = newDisplayNumber;

                            invoice.UpdatedBy = userContext.Id;
                            invoice.UpdatedByCompanyId = userContext.CompanyId;
                            response.invoiceModels.Add(invoice);
                        }
                        response.InvoiceHeader = originalInvoices.invoiceHeader.ToViewModel();
                        response.InvoiceHeader.UpdatedBy = userContext.Id;
                        response.InvoiceHeader.UpdatedDate = DateTimeOffset.Now;
                        response.StatusCode = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceEditDomain", "GetInvoiceEditRequestForEditInvoiceNumber", ex.Message, ex);
                }
            }
            return response;
        }

        //private async Task<InvoiceEditRequestViewModel> GetInvoiceEditRequestForEditInvoiceNumber(UserContext userContext, int invoiceId, string newDisplayNumber)
        //{
        //    var response = new InvoiceEditRequestViewModel();
        //    using (var tracer = new Tracer("InvoiceEditDomain", "GetInvoiceEditRequestForEditInvoiceNumber"))
        //    {
        //        try
        //        {
        //            var originalInvoice = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).SingleOrDefaultAsync();
        //            if (originalInvoice != null)
        //            {
        //                SetInvoiceModelFromOriginalInvoice(originalInvoice, response);

        //                //modify displaynumber and version
        //                response.InvoiceModel.Version = response.InvoiceModel.Version + 1;
        //                response.InvoiceModel.DisplayInvoiceNumber = newDisplayNumber;
        //                response.InvoiceModel.UpdatedBy = userContext.Id;
        //                response.InvoiceModel.UpdatedByCompanyId = userContext.CompanyId;
        //                response.IsInvoiceImagesAvailable = true;
        //                response.IsTaxServiceSucceeded = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogManager.Logger.WriteException("InvoiceEditDomain", "GetInvoiceEditRequestForEditInvoiceNumber", ex.Message, ex);
        //        }
        //    }
        //    return response;
        //}

        public async Task<StatusViewModel> InvoiceEditForInvoicePoNumberAsync(UserContext userContext, int invoiceId, string newInvoicePoNumber, int? orderId)
        {
            var response = new StatusViewModel();
            try
            {
                ConsolidatedInvoiceEditViewModel invoiceEditRequest = null;

                invoiceEditRequest = await GetInvoiceEditForInvoicePoNumber(userContext, invoiceId, newInvoicePoNumber);
                if (invoiceEditRequest?.invoiceModels != null && invoiceEditRequest.invoiceModels.Any())
                {
                    response = await UpdateInvoiceForEditNumberAsync(invoiceEditRequest, orderId);
                    if (response.StatusCode == Status.Failed)
                    {
                        return response;
                    }
                    response.StatusMessage = Resource.errMessageInvoiceUpdatedSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "InvoiceEditForInvoicePoNumberAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task<ConsolidatedInvoiceEditViewModel> GetInvoiceEditForInvoicePoNumber(UserContext userContext, int invoiceId, string newInvoicePoNumber)
        {
            var response = new ConsolidatedInvoiceEditViewModel();
            using (var tracer = new Tracer("InvoiceEditDomain", "GetInvoiceEditForInvoicePoNumber"))
            {
                try
                {
                    var originalInvoices = await Context.DataContext.InvoiceHeaderDetails
                                            .Where(t => t.Invoices.Any(t1 => t1.Id == invoiceId))
                                            .Select(t => new { invoiceHeader = t, invoices = t.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active) })
                                            .FirstOrDefaultAsync();
                    if (originalInvoices?.invoices != null)
                    {
                        foreach (var originalInvoice in originalInvoices.invoices)
                        {
                            var invoice = GetInvoiceModelFromOriginalInvoice(originalInvoice);

                            if (string.IsNullOrWhiteSpace(originalInvoice.ReferenceId))
                                invoice.ReferenceId = originalInvoice.InvoiceHeader.InvoiceNumber.Number;
                            else
                                invoice.ReferenceId = originalInvoice.ReferenceId;
                            //modify displaynumber and version
                            invoice.Version += 1;
                            invoice.PoNumber = newInvoicePoNumber;

                            invoice.UpdatedBy = userContext.Id;
                            invoice.UpdatedByCompanyId = userContext.CompanyId;
                            response.invoiceModels.Add(invoice);
                        }
                        response.InvoiceHeader = originalInvoices.invoiceHeader.ToViewModel();
                        response.InvoiceHeader.UpdatedBy = userContext.Id;
                        response.InvoiceHeader.UpdatedDate = DateTimeOffset.Now;
                        response.StatusCode = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceEditDomain", "GetInvoiceEditForInvoicePoNumber", ex.Message, ex);
                }
            }
            return response;
        }

        private static void UpdateManualInvoiceViewModel(ManualInvoiceViewModel manualInvoiceModel, InvoiceEditRequestViewModel invoiceEditRequest, InvoiceEditResponseViewModel updateResponse)
        {
            manualInvoiceModel.InvoiceId = updateResponse.InvoiceId;
            manualInvoiceModel.InvoiceHeaderId = updateResponse.InvoiceHeaderId;
            manualInvoiceModel.DisplayInvoiceNumber = updateResponse.InvoiceNumber;
            manualInvoiceModel.TimeZoneName = invoiceEditRequest.TimeZoneName;
            manualInvoiceModel.InvoiceNumberId = invoiceEditRequest.InvoiceModel.InvoiceNumberId;
            manualInvoiceModel.JobId = updateResponse.JobId;
        }

        private DealResponseViewModel UpdateDealResponseViewModel(InvoiceEditResponseViewModel updateResponse)
        {
            var response = new DealResponseViewModel();
            response.InvoiceId = updateResponse.InvoiceId;
            response.DisplayInvoiceNumber = updateResponse.InvoiceNumber;
            response.TimeZoneName = updateResponse.TimeZoneName;
            response.InvoiceNumberId = updateResponse.InvoiceNumberId;
            response.InvoiceTypeId = updateResponse.InvoiceTypeId;
            response.JobId = updateResponse.JobId;
            response.BuyerCompanyId = updateResponse.BuyerCompanyId;
            return response;
        }

        private async Task SetInvoiceUpdatedNewsfeed(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel, int jobCompanyId)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            if (manualInvoiceModel.StatusId == (int)InvoiceStatus.Draft)
            {
                await newsfeedDomain.SetDraftDDTModifiedNewsfeed(userContext, manualInvoiceModel, true);
            }
            else
            {
                await newsfeedDomain.SetInvoiceUpdatedNewsfeedToInvoice(userContext, manualInvoiceModel);
                await newsfeedDomain.SetInvoiceUpdatedNewsfeedToOrder(userContext, manualInvoiceModel);
                await newsfeedDomain.SetInvoiceUpdatedNewsfeedToJob(userContext, manualInvoiceModel, jobCompanyId);
            }
        }

        public async Task<InvoiceEditRequestViewModel> GetInvoiceEditViewModelAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new InvoiceEditRequestViewModel();
            using (var tracer = new Tracer("InvoiceEditDomain", "GetInvoiceEditViewModelAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel() { IsActive = true };
                    var order = await Context.DataContext.Orders.Where(t => t.Id == manualInvoiceModel.OrderId)
                                        .Select(t => new
                                        {
                                            t.FuelRequest.Job,
                                            t.FuelRequest.Job.JobBudget.IsTaxExempted,
                                            JobAddress = new AddressViewModel
                                            {
                                                Address = t.FuelRequest.Job.Address,
                                                City = t.FuelRequest.Job.City,
                                                StateCode = t.FuelRequest.Job.MstState.Code,
                                                CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                                ZipCode = t.FuelRequest.Job.ZipCode,
                                                CountyName = t.FuelRequest.Job.CountyName
                                            },
                                            t.FuelRequest.Job.IsMarine,
                                            t.FuelRequest.UoM,
                                            t.FuelRequest.Job.CountryId,
                                            t.FuelRequest.Currency,
                                            t.FuelRequest.FuelTypeId,
                                            FuelType = t.FuelRequest.MstProduct.DisplayName,
                                            t.FuelRequest.MstProduct.MappedParentId,
                                            t.FuelRequest.MstProduct.ProductCode,
                                            t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                            t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                            t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId,
                                            t.FuelRequest.FuelRequestDetail.TruckLoadTypeId,
                                            t.FuelRequest.MaxQuantity,
                                            t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                            t.FuelRequest.PricingTypeId,
                                            t.BuyerCompanyId,
                                            BuyerCompanyName = t.BuyerCompany.Name,
                                            SupplierCompanyName = t.Company.Name,
                                            t.AcceptedCompanyId,
                                            t.PoNumber,
                                            t.IsFTL,
                                            t.FuelRequest.FuelRequestPricingDetail,
                                            t.FuelRequest.Job.MstState.QuantityIndicatorTypeId,
                                            SupplierTaxExemptLicence = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            t.IsEndSupplier,
                                            BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            JobCompanyId = t.FuelRequest.Job.CompanyId,
                                            // IsSuppressOrderPricing = (t.OrderAdditionalDetail != null && t.OrderAdditionalDetail.OnboardingPreference != null) ? t.OrderAdditionalDetail.OnboardingPreference.IsSupressOrderPricing : false,
                                            IsSuppressOrderPricing = t.OrderAdditionalDetail.IsSupressPricingEnabled,
                                            OriginalInvoice = t.Invoices.Where(t1 => t1.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id
                                                                && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                .Select(t1 => new
                                                                {
                                                                    t1.Id,
                                                                    t1.CreatedBy,
                                                                    t1.CreatedDate,
                                                                    t1.UpdatedDate,
                                                                    t1.InvoiceTypeId,
                                                                    t1.SupplierPreferredInvoiceTypeId,
                                                                    t1.BrokeredChainId,
                                                                    t1.DisplayInvoiceNumber,
                                                                    t1.ReferenceId,
                                                                    t1.ExchangeRate,
                                                                    t1.InvoiceXAdditionalDetail.SupplierAllowance,
                                                                    t1.ParentId,
                                                                    PricePerGallon = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.PricePerGallon).FirstOrDefault(),
                                                                    TerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                                    CityGroupTerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.CityGroupTerminalId).FirstOrDefault(),
                                                                    RackPrice = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.RackPrice).FirstOrDefault(),
                                                                    BolWithTier = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.InvoiceTierPricingDetails.FirstOrDefault()).FirstOrDefault(),
                                                                    t1.WaitingFor,
                                                                    t1.Version,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadChainId,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadSequence,
                                                                    TrackableSchedule = t1.TrackableSchedule,
                                                                    TerminalAddress = new AddressViewModel
                                                                    {
                                                                        Address = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.Address).FirstOrDefault(),
                                                                        City = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.City).FirstOrDefault(),
                                                                        StateCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.StateCode).FirstOrDefault(),
                                                                        CountryCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountryCode).FirstOrDefault(),
                                                                        ZipCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.ZipCode).FirstOrDefault(),
                                                                        CountyName = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountyName).FirstOrDefault()
                                                                    },
                                                                    t1.Discounts,
                                                                    t1.TaxDetails,
                                                                    t1.QbInvoiceNumber,
                                                                    PickupLocation = t1.InvoiceDispatchLocation.FirstOrDefault(t2 => t2.LocationType == (int)LocationType.PickUp),
                                                                    t1.DDTConversionReason,
                                                                    t1.IsBolImageReq,
                                                                    t1.IsSignatureReq,
                                                                    t1.IsDropImageReq,
                                                                    t1.BDRDetail
                                                                }).FirstOrDefault()
                                        }).FirstOrDefaultAsync();
                    if (order != null && order.OriginalInvoice != null)
                    {
                        SetImageFlagsToInvoiceModel(order.OriginalInvoice.IsBolImageReq, order.OriginalInvoice.IsDropImageReq, order.OriginalInvoice.IsSignatureReq, invoiceModel, manualInvoiceModel);
                        //-------------------- From SetManualInputs -----------------------------------------
                        var timeZoneName = order.Job.TimeZoneName;
                        var invoiceCommonDomain = new InvoiceCommonDomain(this);
                        invoiceCommonDomain.SetManualInputsToEditInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName);
                        SetDefaultPayamentDueDateBasis(invoiceModel, order.AcceptedCompanyId);

                        //-------------------- From Invoice Edit --------------------------------------------
                        SetInvoiceInformationToInvoiceModel(order, manualInvoiceModel, invoiceModel);

                        var job = order.Job;
                        var droppedByUserId = manualInvoiceModel.DriverId ?? userContext.Id;
                        invoiceModel.UpdatedBy = userContext.Id;
                        invoiceModel.UpdatedByCompanyId = userContext.CompanyId;
                        invoiceModel.SupplierCompanyId = userContext.CompanyId;
                        invoiceCommonDomain.SetAssetDropsToInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName, job, droppedByUserId);
                        if (order.Job.IsMarine)
                        {
                            SetConvertedDropQtyAndGravityForMFN(order, invoiceModel);
                        }

                        // -------------------------------calculate pricing for terminal change-----------------------------
                        if (!order.IsSuppressOrderPricing && ((manualInvoiceModel.TerminalId != 0 && order.OriginalInvoice.TerminalId != null
                            && manualInvoiceModel.TerminalId != order.OriginalInvoice.TerminalId
                            && order.OriginalInvoice.InvoiceTypeId != (int)InvoiceType.Balance
                            && order.OriginalInvoice.InvoiceTypeId != (int)InvoiceType.DryRun
                            && order.OriginalInvoice.InvoiceTypeId != (int)InvoiceType.TankRental
                            )
                            || (IsPricingRequired(order.OriginalInvoice.WaitingFor, order.OriginalInvoice.InvoiceTypeId, manualInvoiceModel))))
                        {
                            var isAssetAvailable = manualInvoiceModel.Assets != null && manualInvoiceModel.Assets.Any() ? true : false;
                            await invoiceCommonDomain.SetEditInvoicePricingToInvoiceModel(order, invoiceModel, isAssetAvailable);
                            if (invoiceModel.WaitingFor == WaitingAction.UpdatedPrice || invoiceModel.PricePerGallon <= 0)
                            {
                                if (order.OriginalInvoice.WaitingFor == (int)WaitingAction.Nothing)
                                    return null;
                            }
                        }

                        //------------Set Invoice fees from manual invoice view model------------------------
                        var assetCount = invoiceModel.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Select(t => t.AssetName).Distinct().Count();
                        ProcessInvoiceFuelFeesAndSetCalculatedValues(manualInvoiceModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                        //Set Bol details after pricing
                        SetBolDetailsForEdit(invoiceModel, order.FuelType, order.FuelTypeId);
                        if (order.Job.IsMarine)
                        {
                            invoiceModel.PricePerGallon = invoiceModel.ConvertedPricing ?? 0;
                        }

                        //------------Set Invoice taxes for the product base on product type-----------------
                        var isDirectTaxCompany = Context.DataContext.DirectTaxes.Any(t => t.CompanyId == order.BuyerCompanyId && t.StateId == job.StateId && t.IsActive);
                        manualInvoiceModel.IsDirectTaxCompany = isDirectTaxCompany;
                        var serviceViewModel = new AvalaraServiceViewModel()
                        {
                            FuelTypeId = order.MappedParentId ?? order.FuelTypeId,
                            FuelProductCode = order.ProductCode,
                            JobUoM = order.UoM,
                            JobCurrency = order.Currency,
                            IsSalesTaxExempted = order.IsTaxExempted,
                            CountryCurrency = order.Job.MstCountry.Currency,
                            DestinationJobAddress = order.JobAddress,
                            SourceTerminalAddress = invoiceModel.FuelPickLocation != null && !invoiceModel.IsTerminalPickup ? invoiceModel.FuelPickLocation.ToAddressViewModel() : order.OriginalInvoice.TerminalAddress,
                            InvoiceNumber = invoiceModel.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFRB),
                            DroppedGallons = invoiceModel.DroppedGallons,
                            PricePerGallon = invoiceModel.PricePerGallon,
                            DropEndDate = invoiceModel.DropEndDate,
                            InvoiceDate = invoiceModel.UpdatedDate,
                            IsDirectTaxCompany = isDirectTaxCompany,
                            Exclusions = GetTaxEclusionIfExist(invoiceModel.CreatedBy), // if we need to allow update NORA tax on edit
                            BuyerCompanyId = order.BuyerCompanyId,
                            SupplierCompanyId = order.AcceptedCompanyId,
                            JobId = order.Job.Id
                        };
                        if (order.BuyerTaxExemptLicence != null)
                        {
                            serviceViewModel.BuyerCustomId = order.BuyerTaxExemptLicence.EntityCustomId;
                        }
                        if (order.SupplierTaxExemptLicence != null && order.IsEndSupplier)
                        {
                            serviceViewModel.SellerCustomId = order.SupplierTaxExemptLicence.EntityCustomId;
                        }
                        var previousTaxDetails = order.OriginalInvoice.TaxDetails.ToList().ToViewModel();
                        var taxResponse = SetTaxesToEditInvoiceModel(manualInvoiceModel, invoiceModel, serviceViewModel, previousTaxDetails);
                        if (manualInvoiceModel.TaxType == TaxType.Manual && taxResponse.StatusCode == Status.Success && manualInvoiceModel.TypeofFuel == (int)ProductDisplayGroups.OtherFuelType)
                        {
                            SetTaxModifiedFlag(previousTaxDetails, invoiceModel.TaxDetails);
                        }

                        //----------Set Invoice additional details and deals from previous invoice to current invoice----------
                        await invoiceCommonDomain.SetInvoiceAdditionDetailToInvoiceModel(invoiceModel, order.OriginalInvoice.Id, manualInvoiceModel.OrderId);
                        invoiceModel.Discounts = order.OriginalInvoice.Discounts.Select(t => t.ToViewModel()).ToList();
                        invoiceCommonDomain.SetInvoiceBaseAmounts(invoiceModel, invoiceModel.ExchangeRate);

                        //--------- Construct response view model-------------------------------------------------------------
                        response = GetInvoiceEditRequestViewModel(order, manualInvoiceModel, invoiceModel);
                        response.IsTaxServiceSucceeded = manualInvoiceModel.TaxType == TaxType.Standard && taxResponse.StatusCode == Status.Success;
                        response.StatusCode = Status.Success;

                        var trackableSchedule = order.OriginalInvoice.TrackableSchedule;
                        SetTrackableScheduleInfoInResponseViewModel(manualInvoiceModel, response, invoiceModel, timeZoneName, trackableSchedule);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceEditDomain", "GetInvoiceEditViewModelAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private void SetDefaultPayamentDueDateBasis(InvoiceModel invoiceModel, int acceptedCompanyId)
        {
            invoiceModel.PaymentDueDateType = Context.DataContext.OnboardingPreferences.Where(t => t.IsActive && t.CompanyId == acceptedCompanyId)
                .Select(t => t.PaymentDueDateType).FirstOrDefault();
        }

        public async Task<InvoiceEditRequestViewModel> GetBrokeredInvoiceEditViewModelAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new InvoiceEditRequestViewModel();
            using (var tracer = new Tracer("InvoiceEditDomain", "GetBrokeredInvoiceEditViewModelAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel() { IsActive = true };
                    var order = await Context.DataContext.Orders.Where(t => t.Id == manualInvoiceModel.OrderId)
                                        .Select(t => new
                                        {
                                            t.FuelRequest.Job,
                                            t.FuelRequest.Job.JobBudget.IsTaxExempted,
                                            JobAddress = new AddressViewModel
                                            {
                                                Address = t.FuelRequest.Job.Address,
                                                City = t.FuelRequest.Job.City,
                                                StateCode = t.FuelRequest.Job.MstState.Code,
                                                CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                                ZipCode = t.FuelRequest.Job.ZipCode,
                                                CountyName = t.FuelRequest.Job.CountyName
                                            },
                                            t.FuelRequest.UoM,
                                            t.FuelRequest.Currency,
                                            t.FuelRequest.FuelTypeId,
                                            t.FuelRequest.Job.CountryId,
                                            FuelType = t.FuelRequest.MstProduct.DisplayName,
                                            t.FuelRequest.MstProduct.MappedParentId,
                                            t.FuelRequest.MstProduct.ProductCode,
                                            t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                            t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                            t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId,
                                            t.FuelRequest.FuelRequestDetail.TruckLoadTypeId,
                                            t.FuelRequest.MaxQuantity,
                                            t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                            t.FuelRequest.PricingTypeId,
                                            t.BuyerCompanyId,
                                            BuyerCompanyName = t.BuyerCompany.Name,
                                            SupplierCompanyName = t.Company.Name,
                                            t.AcceptedCompanyId,
                                            t.PoNumber,
                                            t.IsFTL,
                                            t.FuelRequest.FuelRequestPricingDetail,
                                            t.FuelRequest.Job.MstState.QuantityIndicatorTypeId,
                                            SupplierTaxExemptLicence = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            t.IsEndSupplier,
                                            BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            JobCompanyId = t.FuelRequest.Job.CompanyId,
                                            //   IsSuppressOrderPricing = (t.OrderAdditionalDetail != null && t.OrderAdditionalDetail.OnboardingPreference != null) ? t.OrderAdditionalDetail.OnboardingPreference.IsSupressOrderPricing : false,
                                            IsSuppressOrderPricing = t.OrderAdditionalDetail.IsSupressPricingEnabled,
                                            OriginalInvoice = t.Invoices.Where(t1 => t1.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id
                                                                && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                .Select(t1 => new
                                                                {
                                                                    t1.Id,
                                                                    t1.CreatedBy,
                                                                    t1.CreatedDate,
                                                                    t1.UpdatedDate,
                                                                    t1.InvoiceTypeId,
                                                                    t1.SupplierPreferredInvoiceTypeId,
                                                                    t1.BrokeredChainId,
                                                                    t1.DisplayInvoiceNumber,
                                                                    t1.ReferenceId,
                                                                    t1.ExchangeRate,
                                                                    t1.InvoiceXAdditionalDetail.SupplierAllowance,
                                                                    t1.ParentId,
                                                                    PricePerGallon = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.PricePerGallon).FirstOrDefault(),
                                                                    TerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                                    CityGroupTerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.CityGroupTerminalId).FirstOrDefault(),
                                                                    RackPrice = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.RackPrice).FirstOrDefault(),
                                                                    BolWithTier = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.InvoiceTierPricingDetails.FirstOrDefault()).FirstOrDefault(),
                                                                    t1.WaitingFor,
                                                                    t1.Version,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadChainId,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadSequence,
                                                                    TrackableSchedule = t1.TrackableSchedule,
                                                                    TerminalAddress = new AddressViewModel
                                                                    {
                                                                        Address = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.Address).FirstOrDefault(),
                                                                        City = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.City).FirstOrDefault(),
                                                                        StateCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.StateCode).FirstOrDefault(),
                                                                        CountryCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountryCode).FirstOrDefault(),
                                                                        ZipCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.ZipCode).FirstOrDefault(),
                                                                        CountyName = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountyName).FirstOrDefault()
                                                                    },
                                                                    t1.Discounts,
                                                                    t1.TaxDetails,
                                                                    t1.QbInvoiceNumber,
                                                                    PickupLocation = t1.InvoiceDispatchLocation.FirstOrDefault(t2 => t2.LocationType == (int)LocationType.PickUp),
                                                                    t1.DDTConversionReason,
                                                                    t1.IsBolImageReq,
                                                                    t1.IsSignatureReq,
                                                                    t1.IsDropImageReq
                                                                }).FirstOrDefault()
                                        }).FirstOrDefaultAsync();
                    if (order != null && order.OriginalInvoice != null)
                    {
                        SetImageFlagsToInvoiceModel(order.OriginalInvoice.IsBolImageReq, order.OriginalInvoice.IsDropImageReq, order.OriginalInvoice.IsSignatureReq, invoiceModel, manualInvoiceModel);
                        //-------------------- From SetManualInputs -----------------------------------------
                        var timeZoneName = order.Job.TimeZoneName;
                        var invoiceCommonDomain = new InvoiceCommonDomain(this);
                        invoiceCommonDomain.SetManualInputsToEditInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName);
                        SetDefaultPayamentDueDateBasis(invoiceModel, order.AcceptedCompanyId);

                        //-------------------- From Invoice Edit --------------------------------------------
                        SetInvoiceInformationToInvoiceModelForBrokerEdit(order, manualInvoiceModel, invoiceModel);

                        var job = order.Job;
                        var droppedByUserId = manualInvoiceModel.DriverId ?? userContext.Id;
                        invoiceModel.UpdatedBy = userContext.Id;
                        invoiceModel.UpdatedByCompanyId = userContext.CompanyId;
                        invoiceCommonDomain.SetAssetDropsToInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName, job, droppedByUserId);
                        if (order.Job.IsMarine)
                        {
                            SetConvertedDropQtyAndGravityForMFN(order, invoiceModel);
                        }

                        // -------------------------------calculate pricing for terminal change-----------------------------
                        if (!order.IsSuppressOrderPricing && ((manualInvoiceModel.TerminalId != 0 && order.OriginalInvoice.TerminalId != null
                            && manualInvoiceModel.TerminalId != order.OriginalInvoice.TerminalId
                            && order.OriginalInvoice.InvoiceTypeId != (int)InvoiceType.Balance
                            && order.OriginalInvoice.InvoiceTypeId != (int)InvoiceType.DryRun
                            && order.OriginalInvoice.InvoiceTypeId != (int)InvoiceType.TankRental)
                            || (IsPricingRequired(order.OriginalInvoice.WaitingFor, order.OriginalInvoice.InvoiceTypeId, manualInvoiceModel))))
                        {
                            await invoiceCommonDomain.SetEditInvoicePricingToInvoiceModel(order, invoiceModel);
                            if (invoiceModel.WaitingFor == WaitingAction.UpdatedPrice || invoiceModel.PricePerGallon <= 0)
                            {
                                if (order.OriginalInvoice.WaitingFor == (int)WaitingAction.Nothing)
                                    return null;
                            }
                        }
                        else if (!order.IsSuppressOrderPricing)
                        {
                            await invoiceCommonDomain.SetPricingToInvoiceModelForEditBrokeredOrder(order, invoiceModel);
                        }

                        //------------Set Invoice fees from manual invoice view model------------------------
                        var assetCount = invoiceModel.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Select(t => t.AssetName).Distinct().Count();
                        ProcessInvoiceFuelFeesAndSetCalculatedValues(manualInvoiceModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                        //Set Bol details after pricing
                        SetBolDetailsForEdit(invoiceModel, order.FuelType, order.FuelTypeId);
                        if (order.Job.IsMarine)
                        {
                            invoiceModel.PricePerGallon = invoiceModel.ConvertedPricing ?? 0;
                        }

                        //------------Set Invoice taxes for the product base on product type-----------------
                        var isDirectTaxCompany = Context.DataContext.DirectTaxes.Any(t => t.CompanyId == order.BuyerCompanyId && t.StateId == job.StateId && t.IsActive);
                        manualInvoiceModel.IsDirectTaxCompany = isDirectTaxCompany;
                        var serviceViewModel = new AvalaraServiceViewModel()
                        {
                            FuelTypeId = order.MappedParentId ?? order.FuelTypeId,
                            FuelProductCode = order.ProductCode,
                            JobUoM = order.UoM,
                            JobCurrency = order.Currency,
                            IsSalesTaxExempted = order.IsTaxExempted,
                            CountryCurrency = order.Job.MstCountry.Currency,
                            DestinationJobAddress = order.JobAddress,
                            SourceTerminalAddress = invoiceModel.FuelPickLocation != null && !invoiceModel.IsTerminalPickup ? invoiceModel.FuelPickLocation.ToAddressViewModel() : order.OriginalInvoice.TerminalAddress,
                            InvoiceNumber = invoiceModel.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFRB),
                            DroppedGallons = invoiceModel.DroppedGallons,
                            PricePerGallon = invoiceModel.PricePerGallon,
                            DropEndDate = invoiceModel.DropEndDate,
                            InvoiceDate = invoiceModel.UpdatedDate,
                            IsDirectTaxCompany = isDirectTaxCompany,
                            Exclusions = GetTaxEclusionIfExist(invoiceModel.CreatedBy), // if we need to allow update NORA tax on edit
                            BuyerCompanyId = order.BuyerCompanyId,
                            SupplierCompanyId = order.AcceptedCompanyId,
                            JobId = order.Job.Id
                        };
                        if (order.BuyerTaxExemptLicence != null)
                        {
                            serviceViewModel.BuyerCustomId = order.BuyerTaxExemptLicence.EntityCustomId;
                        }
                        if (order.SupplierTaxExemptLicence != null && order.IsEndSupplier)
                        {
                            serviceViewModel.SellerCustomId = order.SupplierTaxExemptLicence.EntityCustomId;
                        }
                        var previousTaxDetails = order.OriginalInvoice.TaxDetails.ToList().ToViewModel();
                        var taxResponse = SetTaxesToEditInvoiceModel(manualInvoiceModel, invoiceModel, serviceViewModel, previousTaxDetails);
                        if (manualInvoiceModel.TaxType == TaxType.Manual && taxResponse.StatusCode == Status.Success && manualInvoiceModel.TypeofFuel == (int)ProductDisplayGroups.OtherFuelType)
                        {
                            SetTaxModifiedFlag(previousTaxDetails, invoiceModel.TaxDetails);
                        }

                        //----------Set Invoice additional details and deals from previous invoice to current invoice----------
                        await invoiceCommonDomain.SetInvoiceAdditionDetailToInvoiceModel(invoiceModel, order.OriginalInvoice.Id, manualInvoiceModel.OrderId);
                        invoiceModel.Discounts = order.OriginalInvoice.Discounts.Select(t => t.ToViewModel()).ToList();
                        invoiceCommonDomain.SetInvoiceBaseAmounts(invoiceModel, invoiceModel.ExchangeRate);

                        //--------- Construct response view model-------------------------------------------------------------
                        response = GetInvoiceEditRequestViewModel(order, manualInvoiceModel, invoiceModel);
                        response.IsTaxServiceSucceeded = manualInvoiceModel.TaxType == TaxType.Standard && taxResponse.StatusCode == Status.Success;
                        response.StatusCode = Status.Success;

                        var trackableSchedule = order.OriginalInvoice.TrackableSchedule;
                        SetTrackableScheduleInfoInResponseViewModel(manualInvoiceModel, response, invoiceModel, timeZoneName, trackableSchedule);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceEditDomain", "GetBrokeredInvoiceEditViewModelAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private bool IsPricingRequired(int waitingFor, decimal originalDroppedGallons, ManualInvoiceViewModel manualInvoiceModel)
        {
            if (waitingFor == (int)WaitingAction.PrePostDipData && manualInvoiceModel.WaitingForAction == (int)WaitingAction.Nothing)
                return true;

            if (manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.IncludeInPPG))
                return true;

            if (originalDroppedGallons != manualInvoiceModel.FuelDropped)
                return true;
            return false;
        }

        protected void SetBolDetailsForEdit(InvoiceModel viewModel, string fuelType, int fuelTypeId)
        {
            foreach (var item in viewModel.BolDetails)
            {
                item.RackPrice = viewModel.RackPrice;
                item.PricePerGallon = viewModel.PricePerGallon;
                item.CreatedBy = viewModel.UpdatedBy;
                item.CreatedDate = viewModel.UpdatedDate;
                item.FuelType = fuelType;
                item.FuelTypeId = fuelTypeId;
                item.TypeofFuel = viewModel.TypeOfFuel;
                item.Image = viewModel.BolImage;
                item.IsActive = true;
                item.IsDeleted = false;
                item.CityGroupTerminalId = viewModel.CityGroupTerminalId;
                var terminal = Context.DataContext.MstExternalTerminals.Where(t => t.Id == viewModel.TerminalId).FirstOrDefault();
                if (!string.IsNullOrEmpty(item.LiftTicketNumber))
                {
                    item.PickupLocationType = PickupLocationType.BulkPlant;
                    var pickUpLocation = viewModel.FuelPickLocation;
                    if (pickUpLocation != null)
                    {
                        item.TerminalId = terminal?.Id;
                        item.TerminalName = terminal?.Name;
                        item.Address = pickUpLocation.Address;
                        item.City = pickUpLocation.City;
                        item.StateCode = pickUpLocation.StateCode;
                        item.StateId = pickUpLocation.StateId;
                        item.ZipCode = pickUpLocation.ZipCode;
                        item.CountryCode = pickUpLocation.CountryCode;
                        item.Latitude = pickUpLocation.Latitude;
                        item.Longitude = pickUpLocation.Longitude;
                        item.CountyName = pickUpLocation.CountyName;
                        item.SiteName = pickUpLocation.SiteName;
                    }
                }
                else
                {
                    item.PickupLocationType = PickupLocationType.Terminal;
                    if (terminal != null)
                    {
                        item.TerminalId = terminal.Id;
                        item.TerminalName = terminal.Name;
                        item.Address = terminal.Address;
                        item.City = terminal.City;
                        item.StateCode = terminal.StateCode;
                        item.StateId = terminal.StateId;
                        item.ZipCode = terminal.ZipCode;
                        item.CountryCode = terminal.CountryCode;
                        item.Latitude = terminal.Latitude;
                        item.Longitude = terminal.Longitude;
                        item.CountyName = terminal.CountyName;
                        item.SiteName = terminal.Name;
                    }
                }
            }
        }


        private static BolDetailViewModel GetLiftInformation(string carrier, int productId, DropAdditionalDetailsModel deliveryDetails, InvoiceLiftTicketViewModel bol, LiftProductViewModel product)
        {
            return new BolDetailViewModel()
            {
                LiftQuantity = product.LiftQuantity,
                LiftTicketNumber = bol.LiftTicketNumber,
                Carrier = carrier,
                LiftDate = bol.LiftDate,
                LiftStartTime = Convert.ToDateTime(bol.LiftStartTime).TimeOfDay,
                LiftEndTime = Convert.ToDateTime(bol.LiftEndTime).TimeOfDay,
                LiftArrivalTime = bol.LiftArrivalTime,
                BolCreationTime = bol.BolCreationTime,
                TerminalId = deliveryDetails.TerminalId,
                TerminalName = deliveryDetails.TerminalName,
                PickupLocationType = PickupLocationType.BulkPlant,
                CityGroupTerminalId = deliveryDetails.CityGroupTerminalId,
                FuelTypeId = productId,
                Address = product.Address.Address,
                City = product.Address.City,
                StateCode = product.Address.State.Code,
                StateId = product.Address.State.Id,
                ZipCode = product.Address.ZipCode,
                CountryCode = product.Address.Country.Code,
                Latitude = product.Address.Latitude,
                Longitude = product.Address.Longitude,
                CountyName = product.Address.CountyName,
                SiteName = product.Address.SiteName,
                Image = bol.Images != null && !string.IsNullOrWhiteSpace(bol.Images.FilePath) ? bol.Images : null,
                IsActive = true,
                IsDeleted = false,
                RackPrice = 0
            };
        }

        public async Task<InvoiceEditRequestViewModel> GetConsolidatedInvoiceDealViewModelAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new InvoiceEditRequestViewModel();
            using (var tracer = new Tracer("InvoiceEditDomain", "GetInvoiceEditViewModelAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel() { IsActive = true };
                    var order = await Context.DataContext.Orders.Where(t => t.Id == manualInvoiceModel.OrderId)
                                        .Select(t => new
                                        {
                                            t.FuelRequest.Job,
                                            t.FuelRequest.Job.JobBudget.IsTaxExempted,
                                            JobAddress = new AddressViewModel
                                            {
                                                Address = t.FuelRequest.Job.Address,
                                                City = t.FuelRequest.Job.City,
                                                StateCode = t.FuelRequest.Job.MstState.Code,
                                                CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                                ZipCode = t.FuelRequest.Job.ZipCode,
                                                CountyName = t.FuelRequest.Job.CountyName
                                            },
                                            FuelType = t.FuelRequest.MstProduct.DisplayName,
                                            t.FuelRequest.UoM,
                                            t.FuelRequest.Currency,
                                            t.FuelRequest.FuelTypeId,
                                            t.FuelRequest.Job.CountryId,
                                            t.FuelRequest.MstProduct.MappedParentId,
                                            t.FuelRequest.MstProduct.ProductCode,
                                            t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                            t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                            t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId,
                                            t.FuelRequest.FuelRequestDetail.TruckLoadTypeId,
                                            t.FuelRequest.MaxQuantity,
                                            t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                            t.FuelRequest.PricingTypeId,
                                            t.FuelRequest.Job.IsMarine,
                                            t.BuyerCompanyId,
                                            BuyerCompanyName = t.BuyerCompany.Name,
                                            SupplierCompanyName = t.Company.Name,
                                            t.AcceptedCompanyId,
                                            t.PoNumber,
                                            t.IsFTL,
                                            t.FuelRequest.FuelRequestPricingDetail,
                                            t.FuelRequest.Job.MstState.QuantityIndicatorTypeId,
                                            SupplierTaxExemptLicence = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            t.IsEndSupplier,
                                            BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            JobCompanyId = t.FuelRequest.Job.CompanyId,
                                            //  IsSuppressOrderPricing = (t.OrderAdditionalDetail != null && t.OrderAdditionalDetail.OnboardingPreference != null) ? t.OrderAdditionalDetail.OnboardingPreference.IsSupressOrderPricing : false,
                                            IsSuppressOrderPricing = t.OrderAdditionalDetail.IsSupressPricingEnabled,
                                            OriginalInvoice = t.Invoices.Where(t1 => t1.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id
                                                                && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                .Select(t1 => new
                                                                {
                                                                    t1.Id,
                                                                    t1.CreatedBy,
                                                                    t1.CreatedDate,
                                                                    t1.UpdatedDate,
                                                                    t1.InvoiceTypeId,
                                                                    t1.SupplierPreferredInvoiceTypeId,
                                                                    t1.BrokeredChainId,
                                                                    t1.DisplayInvoiceNumber,
                                                                    t1.ReferenceId,
                                                                    t1.ExchangeRate,
                                                                    t1.ParentId,
                                                                    PricePerGallon = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.PricePerGallon).FirstOrDefault(),
                                                                    TerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                                    CityGroupTerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.CityGroupTerminalId).FirstOrDefault(),
                                                                    RackPrice = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.RackPrice).FirstOrDefault(),
                                                                    BolWithTier = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.InvoiceTierPricingDetails.FirstOrDefault()).FirstOrDefault(),
                                                                    t1.WaitingFor,
                                                                    t1.Version,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadChainId,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadSequence,
                                                                    TrackableSchedule = t1.TrackableSchedule,
                                                                    TerminalAddress = new AddressViewModel
                                                                    {
                                                                        Address = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.Address).FirstOrDefault(),
                                                                        City = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.City).FirstOrDefault(),
                                                                        StateCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.StateCode).FirstOrDefault(),
                                                                        CountryCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountryCode).FirstOrDefault(),
                                                                        ZipCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.ZipCode).FirstOrDefault(),
                                                                        CountyName = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountyName).FirstOrDefault()
                                                                    },
                                                                    t1.Discounts,
                                                                    t1.InvoiceXAdditionalDetail.SupplierAllowance,
                                                                    t1.TaxDetails,
                                                                    t1.QbInvoiceNumber,
                                                                    PickupLocation = t1.InvoiceDispatchLocation.FirstOrDefault(t2 => t2.LocationType == (int)LocationType.PickUp),
                                                                    t1.DDTConversionReason,
                                                                    t1.IsBolImageReq,
                                                                    t1.IsSignatureReq,
                                                                    t1.IsDropImageReq,
                                                                    t1.BDRDetail
                                                                }).FirstOrDefault()
                                        }).FirstOrDefaultAsync();
                    if (order != null && order.OriginalInvoice != null)
                    {
                        SetImageFlagsToInvoiceModel(order.OriginalInvoice.IsBolImageReq, order.OriginalInvoice.IsDropImageReq, order.OriginalInvoice.IsSignatureReq, invoiceModel, manualInvoiceModel);
                        //-------------------- From SetManualInputs -----------------------------------------
                        var timeZoneName = order.Job.TimeZoneName;
                        var invoiceCommonDomain = new InvoiceCommonDomain(this);
                        invoiceCommonDomain.SetManualInputsToConsolidatedEditInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName);
                        SetDefaultPayamentDueDateBasis(invoiceModel, order.AcceptedCompanyId);

                        //-------------------- From Invoice Edit --------------------------------------------
                        SetInvoiceInformationToInvoiceModel(order, manualInvoiceModel, invoiceModel);

                        var job = order.Job;
                        var droppedByUserId = manualInvoiceModel.DriverId ?? userContext.Id;
                        invoiceModel.UpdatedBy = userContext.Id;
                        invoiceModel.UpdatedByCompanyId = userContext.CompanyId;
                        invoiceCommonDomain.SetAssetDropsToInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName, job, droppedByUserId);
                        if (order.Job.IsMarine)
                        {
                            SetConvertedDropQtyAndGravityForMFN(order, invoiceModel);
                        }

                        // -------------------------------calculate pricing for terminal change-----------------------------
                        if (!order.IsSuppressOrderPricing && (manualInvoiceModel.TerminalId != 0 && order.OriginalInvoice.TerminalId != null
                            && manualInvoiceModel.TerminalId != order.OriginalInvoice.TerminalId
                            && order.OriginalInvoice.InvoiceTypeId != (int)InvoiceType.Balance
                            && order.OriginalInvoice.InvoiceTypeId != (int)InvoiceType.DryRun
                            && order.OriginalInvoice.InvoiceTypeId != (int)InvoiceType.TankRental))
                        {
                            await invoiceCommonDomain.SetEditInvoicePricingToInvoiceModel(order, invoiceModel);
                            if (invoiceModel.WaitingFor == WaitingAction.UpdatedPrice || invoiceModel.PricePerGallon <= 0)
                            {
                                return null;
                            }
                        }

                        if (order.Job.IsMarine)
                        {
                            invoiceModel.PricePerGallon = invoiceModel.ConvertedPricing ?? 0;
                        }

                        //------------Set Invoice fees from manual invoice view model------------------------
                        var assetCount = invoiceModel.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Select(t => t.AssetName).Distinct().Count();
                        ProcessInvoiceFuelFeesAndSetCalculatedValues(manualInvoiceModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                        //------------Set Invoice taxes for the product base on product type-----------------
                        var isDirectTaxCompany = Context.DataContext.DirectTaxes.Any(t => t.CompanyId == order.BuyerCompanyId && t.StateId == job.StateId && t.IsActive);
                        manualInvoiceModel.IsDirectTaxCompany = isDirectTaxCompany;
                        var serviceViewModel = new AvalaraServiceViewModel()
                        {
                            FuelTypeId = order.MappedParentId ?? order.FuelTypeId,
                            FuelProductCode = order.ProductCode,
                            JobUoM = order.UoM,
                            JobCurrency = order.Currency,
                            IsSalesTaxExempted = order.IsTaxExempted,
                            CountryCurrency = order.Job.MstCountry.Currency,
                            DestinationJobAddress = order.JobAddress,
                            SourceTerminalAddress = invoiceModel.FuelPickLocation != null && !invoiceModel.IsTerminalPickup ? invoiceModel.FuelPickLocation.ToAddressViewModel() : order.OriginalInvoice.TerminalAddress,
                            InvoiceNumber = invoiceModel.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFRB),
                            DroppedGallons = invoiceModel.DroppedGallons,
                            PricePerGallon = invoiceModel.PricePerGallon,
                            DropEndDate = invoiceModel.DropEndDate,
                            InvoiceDate = invoiceModel.UpdatedDate,
                            IsDirectTaxCompany = isDirectTaxCompany,
                            Exclusions = GetTaxEclusionIfExist(invoiceModel.CreatedBy), // if we need to allow update NORA tax on edit
                            BuyerCompanyId = order.BuyerCompanyId,
                            SupplierCompanyId = order.AcceptedCompanyId,
                            JobId = order.Job.Id
                        };
                        if (order.BuyerTaxExemptLicence != null)
                        {
                            serviceViewModel.BuyerCustomId = order.BuyerTaxExemptLicence.EntityCustomId;
                        }
                        if (order.SupplierTaxExemptLicence != null && order.IsEndSupplier)
                        {
                            serviceViewModel.SellerCustomId = order.SupplierTaxExemptLicence.EntityCustomId;
                        }
                        var previousTaxDetails = order.OriginalInvoice.TaxDetails.ToList().ToViewModel();
                        var taxResponse = SetTaxesToEditInvoiceModel(manualInvoiceModel, invoiceModel, serviceViewModel, previousTaxDetails);
                        if (manualInvoiceModel.TaxType == TaxType.Manual && taxResponse.StatusCode == Status.Success && manualInvoiceModel.TypeofFuel == (int)ProductDisplayGroups.OtherFuelType)
                        {
                            SetTaxModifiedFlag(previousTaxDetails, invoiceModel.TaxDetails);
                        }

                        //----------Set Invoice additional details and deals from previous invoice to current invoice----------
                        await invoiceCommonDomain.SetInvoiceAdditionDetailToInvoiceModel(invoiceModel, order.OriginalInvoice.Id, manualInvoiceModel.OrderId);
                        invoiceModel.Discounts = order.OriginalInvoice.Discounts.Select(t => t.ToViewModel()).ToList();
                        invoiceCommonDomain.SetInvoiceBaseAmounts(invoiceModel, invoiceModel.ExchangeRate);

                        //--------- Construct response view model-------------------------------------------------------------
                        response = GetInvoiceEditRequestViewModel(order, manualInvoiceModel, invoiceModel);
                        response.IsTaxServiceSucceeded = manualInvoiceModel.TaxType == TaxType.Standard && taxResponse.StatusCode == Status.Success;
                        response.StatusCode = Status.Success;

                        var trackableSchedule = order.OriginalInvoice.TrackableSchedule;
                        SetTrackableScheduleInfoInResponseViewModel(manualInvoiceModel, response, invoiceModel, timeZoneName, trackableSchedule);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceEditDomain", "GetInvoiceEditViewModelAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private void SetImageFlagsToInvoiceModel(bool isBolImageReq, bool isDropImageReq, bool IsSignatureReq, InvoiceModel invoiceModel, ManualInvoiceViewModel manualInvoiceModel)
        {
            invoiceModel.IsBOLImageReq = isBolImageReq;
            invoiceModel.IsDropImageReq = isDropImageReq;
            invoiceModel.IsSignatureReq = IsSignatureReq;
        }

        public async Task<InvoiceEditRequestViewModel> GetInvoiceEditViewModelForNewDateAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new InvoiceEditRequestViewModel();
            using (var tracer = new Tracer("InvoiceEditDomain", "GetInvoiceEditViewModelForNewDateAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel() { IsActive = true };
                    var order = await Context.DataContext.Orders.Where(t => t.Id == manualInvoiceModel.OrderId)
                                        .Select(t => new
                                        {
                                            t.FuelRequest.Job,
                                            t.FuelRequest.Job.JobBudget.IsTaxExempted,
                                            JobAddress = new AddressViewModel
                                            {
                                                Address = t.FuelRequest.Job.Address,
                                                City = t.FuelRequest.Job.City,
                                                StateCode = t.FuelRequest.Job.MstState.Code,
                                                CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                                ZipCode = t.FuelRequest.Job.ZipCode,
                                                CountyName = t.FuelRequest.Job.CountyName
                                            },
                                            t.FuelRequest.UoM,
                                            t.FuelRequest.Currency,
                                            t.FuelRequest.Job.CountryId,
                                            t.FuelRequest.FuelTypeId,
                                            FuelType = t.FuelRequest.MstProduct.DisplayName,
                                            t.FuelRequest.MstProduct.MappedParentId,
                                            t.FuelRequest.MstProduct.ProductCode,
                                            t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                            t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                            t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId,
                                            t.FuelRequest.FuelRequestDetail.TruckLoadTypeId,
                                            t.FuelRequest.MaxQuantity,
                                            t.BuyerCompanyId,
                                            BuyerCompanyName = t.BuyerCompany.Name,
                                            SupplierCompanyName = t.Company.Name,
                                            t.AcceptedCompanyId,
                                            t.PoNumber,
                                            t.IsFTL,
                                            t.FuelRequest.PricingTypeId,
                                            t.FuelRequest.Job.IsMarine,
                                            t.FuelRequest.FuelRequestPricingDetail,
                                            t.FuelRequest.Job.MstState.QuantityIndicatorTypeId,
                                            t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                            SupplierTaxExemptLicence = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            t.IsEndSupplier,
                                            BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            JobCompanyId = t.FuelRequest.Job.CompanyId,
                                            // IsSuppressOrderPricing = (t.OrderAdditionalDetail != null && t.OrderAdditionalDetail.OnboardingPreference != null) ? t.OrderAdditionalDetail.OnboardingPreference.IsSupressOrderPricing : false,
                                            IsSuppressOrderPricing = t.OrderAdditionalDetail.IsSupressPricingEnabled,
                                            OriginalInvoice = t.Invoices.Where(t1 => t1.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id
                                                                && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                .Select(t1 => new
                                                                {
                                                                    t1.Id,
                                                                    t1.CreatedBy,
                                                                    t1.CreatedDate,
                                                                    t1.UpdatedDate,
                                                                    t1.InvoiceTypeId,
                                                                    t1.SupplierPreferredInvoiceTypeId,
                                                                    t1.BrokeredChainId,
                                                                    t1.DisplayInvoiceNumber,
                                                                    t1.ReferenceId,
                                                                    t1.ExchangeRate,
                                                                    t1.InvoiceXAdditionalDetail.SupplierAllowance,
                                                                    t1.ParentId,
                                                                    PricePerGallon = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.PricePerGallon).FirstOrDefault(),
                                                                    TerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                                    CityGroupTerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.CityGroupTerminalId).FirstOrDefault(),
                                                                    RackPrice = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.RackPrice).FirstOrDefault(),
                                                                    BolWithTier = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.InvoiceTierPricingDetails.FirstOrDefault()).FirstOrDefault(),
                                                                    t1.WaitingFor,
                                                                    t1.Version,
                                                                    t1.InvoiceVersionStatusId,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadChainId,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadSequence,
                                                                    TrackableSchedule = t1.TrackableSchedule,
                                                                    TerminalAddress = new AddressViewModel
                                                                    {
                                                                        Address = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.Address).FirstOrDefault(),
                                                                        City = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.City).FirstOrDefault(),
                                                                        StateCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.StateCode).FirstOrDefault(),
                                                                        CountryCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountryCode).FirstOrDefault(),
                                                                        ZipCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.ZipCode).FirstOrDefault(),
                                                                        CountyName = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountyName).FirstOrDefault()
                                                                    },
                                                                    t1.Discounts,
                                                                    t1.TaxDetails,
                                                                    t1.QbInvoiceNumber,
                                                                    PickupLocation = t1.InvoiceDispatchLocation.FirstOrDefault(t2 => t2.LocationType == (int)LocationType.PickUp),
                                                                    t1.DDTConversionReason,
                                                                    t1.IsBolImageReq,
                                                                    t1.IsSignatureReq,
                                                                    t1.IsDropImageReq,
                                                                    t1.BDRDetail
                                                                }).FirstOrDefault()
                                        }).FirstOrDefaultAsync();
                    if (order != null && order.OriginalInvoice != null)
                    {
                        SetImageFlagsToInvoiceModel(order.OriginalInvoice.IsBolImageReq, order.OriginalInvoice.IsDropImageReq, order.OriginalInvoice.IsSignatureReq, invoiceModel, manualInvoiceModel);
                        //-------------------- From SetManualInputs -----------------------------------------
                        var timeZoneName = order.Job.TimeZoneName;
                        var invoiceCommonDomain = new InvoiceCommonDomain(this);
                        invoiceCommonDomain.SetManualInputsToEditInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName);
                        SetDefaultPayamentDueDateBasis(invoiceModel, order.AcceptedCompanyId);

                        //-------------------- From Invoice Edit --------------------------------------------
                        SetInvoiceInformationToInvoiceModel(order, manualInvoiceModel, invoiceModel);

                        invoiceModel.UpdatedBy = userContext.Id;
                        invoiceModel.SupplierCompanyId = userContext.CompanyId;
                        var job = order.Job;
                        var droppedByUserId = manualInvoiceModel.DriverId ?? userContext.Id;
                        invoiceCommonDomain.SetAssetDropsToInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName, job, droppedByUserId);
                        if (order.Job.IsMarine)
                        {
                            SetConvertedDropQtyAndGravityForMFN(order, invoiceModel);
                        }

                        //recalculate PPG
                        if (!order.IsSuppressOrderPricing)
                            await invoiceCommonDomain.SetEditInvoicePricingToInvoiceModel(order, invoiceModel);

                        //Set Bol details after pricing
                        SetBolDetailsForEdit(invoiceModel, order.FuelType, order.FuelTypeId);

                        if (invoiceModel.WaitingFor == WaitingAction.UpdatedPrice || invoiceModel.PricePerGallon <= 0)
                        {
                            return null;
                        }

                        if (order.Job.IsMarine)
                        {
                            invoiceModel.PricePerGallon = invoiceModel.ConvertedPricing ?? 0;
                        }

                        //------------Set Invoice fees from manual invoice view model------------------------
                        var assetCount = invoiceModel.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Select(t => t.AssetName).Distinct().Count();
                        ProcessInvoiceFuelFeesAndSetCalculatedValues(manualInvoiceModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                        //------------Set Invoice taxes for the product base on product type-----------------
                        var isDirectTaxCompany = Context.DataContext.DirectTaxes.Any(t => t.CompanyId == order.BuyerCompanyId && t.StateId == job.StateId && t.IsActive);
                        manualInvoiceModel.IsDirectTaxCompany = isDirectTaxCompany;
                        var serviceViewModel = new AvalaraServiceViewModel()
                        {
                            FuelTypeId = order.MappedParentId ?? order.FuelTypeId,
                            FuelProductCode = order.ProductCode,
                            JobUoM = order.UoM,
                            JobCurrency = order.Currency,
                            IsSalesTaxExempted = order.IsTaxExempted,
                            CountryCurrency = order.Job.MstCountry.Currency,
                            DestinationJobAddress = order.JobAddress,
                            SourceTerminalAddress = invoiceModel.FuelPickLocation != null ? invoiceModel.FuelPickLocation.ToAddressViewModel() : order.OriginalInvoice.TerminalAddress,
                            InvoiceNumber = invoiceModel.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFRB),
                            DroppedGallons = invoiceModel.DroppedGallons,
                            PricePerGallon = invoiceModel.PricePerGallon,
                            DropEndDate = invoiceModel.DropEndDate,
                            InvoiceDate = invoiceModel.UpdatedDate,
                            IsDirectTaxCompany = isDirectTaxCompany,
                            BuyerCompanyId = order.BuyerCompanyId,
                            SupplierCompanyId = order.AcceptedCompanyId,
                            JobId = order.Job.Id
                        };
                        if (order.BuyerTaxExemptLicence != null)
                        {
                            serviceViewModel.BuyerCustomId = order.BuyerTaxExemptLicence.EntityCustomId;
                        }
                        if (order.SupplierTaxExemptLicence != null && order.IsEndSupplier)
                        {
                            serviceViewModel.SellerCustomId = order.SupplierTaxExemptLicence.EntityCustomId;
                        }
                        var previousTaxDetails = order.OriginalInvoice.TaxDetails.ToList().ToViewModel();
                        var taxResponse = SetTaxesToEditInvoiceModel(manualInvoiceModel, invoiceModel, serviceViewModel, previousTaxDetails);
                        if (manualInvoiceModel.TaxType == TaxType.Manual && taxResponse.StatusCode == Status.Success && manualInvoiceModel.TypeofFuel == (int)ProductDisplayGroups.OtherFuelType)
                        {
                            SetTaxModifiedFlag(previousTaxDetails, invoiceModel.TaxDetails);
                        }

                        //----------Set Invoice additional details and deals from previous invoice to current invoice----------
                        await invoiceCommonDomain.SetInvoiceAdditionDetailToInvoiceModel(invoiceModel, order.OriginalInvoice.Id, manualInvoiceModel.OrderId);
                        invoiceModel.Discounts = order.OriginalInvoice.Discounts.Select(t => t.ToViewModel()).ToList();
                        invoiceCommonDomain.SetInvoiceBaseAmounts(invoiceModel, invoiceModel.ExchangeRate);

                        //--------- Construct response view model-------------------------------------------------------------
                        response = GetInvoiceEditRequestViewModel(order, manualInvoiceModel, invoiceModel);
                        response.IsTaxServiceSucceeded = manualInvoiceModel.TaxType == TaxType.Standard && taxResponse.StatusCode == Status.Success;
                        response.StatusCode = Status.Success;

                        var trackableSchedule = order.OriginalInvoice.TrackableSchedule;
                        SetTrackableScheduleInfoInResponseViewModel(manualInvoiceModel, response, invoiceModel, timeZoneName, trackableSchedule);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceEditDomain", "GetInvoiceEditViewModelForNewDateAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<InvoiceEditRequestViewModel> GetBrokeredInvoiceEditViewModelForNewDateAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new InvoiceEditRequestViewModel();
            using (var tracer = new Tracer("InvoiceEditDomain", "GetBrokeredInvoiceEditViewModelForNewDateAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel() { IsActive = true };
                    var order = await Context.DataContext.Orders.Where(t => t.Id == manualInvoiceModel.OrderId)
                                        .Select(t => new
                                        {
                                            t.FuelRequest.Job,
                                            t.FuelRequest.Job.JobBudget.IsTaxExempted,
                                            JobAddress = new AddressViewModel
                                            {
                                                Address = t.FuelRequest.Job.Address,
                                                City = t.FuelRequest.Job.City,
                                                StateCode = t.FuelRequest.Job.MstState.Code,
                                                CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                                ZipCode = t.FuelRequest.Job.ZipCode,
                                                CountyName = t.FuelRequest.Job.CountyName
                                            },
                                            t.FuelRequest.UoM,
                                            t.FuelRequest.Currency,
                                            t.FuelRequest.FuelTypeId,
                                            t.FuelRequest.Job.CountryId,
                                            FuelType = t.FuelRequest.MstProduct.DisplayName,
                                            t.FuelRequest.MstProduct.MappedParentId,
                                            t.FuelRequest.MstProduct.ProductCode,
                                            t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                            t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                            t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId,
                                            t.FuelRequest.FuelRequestDetail.TruckLoadTypeId,
                                            t.FuelRequest.MaxQuantity,
                                            t.BuyerCompanyId,
                                            BuyerCompanyName = t.BuyerCompany.Name,
                                            SupplierCompanyName = t.Company.Name,
                                            t.AcceptedCompanyId,
                                            t.PoNumber,
                                            t.IsFTL,
                                            t.FuelRequest.PricingTypeId,
                                            t.FuelRequest.FuelRequestPricingDetail,
                                            t.FuelRequest.Job.MstState.QuantityIndicatorTypeId,
                                            t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                            SupplierTaxExemptLicence = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            t.IsEndSupplier,
                                            BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            JobCompanyId = t.FuelRequest.Job.CompanyId,
                                            // IsSuppressOrderPricing = (t.OrderAdditionalDetail != null && t.OrderAdditionalDetail.OnboardingPreference != null) ? t.OrderAdditionalDetail.OnboardingPreference.IsSupressOrderPricing : false,
                                            IsSuppressOrderPricing = t.OrderAdditionalDetail.IsSupressPricingEnabled,
                                            OriginalInvoice = t.Invoices.Where(t1 => t1.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id
                                                                && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                .Select(t1 => new
                                                                {
                                                                    t1.Id,
                                                                    t1.CreatedBy,
                                                                    t1.CreatedDate,
                                                                    t1.UpdatedDate,
                                                                    t1.InvoiceTypeId,
                                                                    t1.SupplierPreferredInvoiceTypeId,
                                                                    t1.BrokeredChainId,
                                                                    t1.DisplayInvoiceNumber,
                                                                    t1.ReferenceId,
                                                                    t1.ExchangeRate,
                                                                    t1.InvoiceXAdditionalDetail.SupplierAllowance,
                                                                    t1.ParentId,
                                                                    PricePerGallon = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.PricePerGallon).FirstOrDefault(),
                                                                    TerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                                    CityGroupTerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.CityGroupTerminalId).FirstOrDefault(),
                                                                    RackPrice = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.RackPrice).FirstOrDefault(),
                                                                    BolWithTier = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.InvoiceTierPricingDetails.FirstOrDefault()).FirstOrDefault(),
                                                                    t1.WaitingFor,
                                                                    t1.Version,
                                                                    t1.InvoiceVersionStatusId,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadChainId,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadSequence,
                                                                    TrackableSchedule = t1.TrackableSchedule,
                                                                    TerminalAddress = new AddressViewModel
                                                                    {
                                                                        Address = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.Address).FirstOrDefault(),
                                                                        City = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.City).FirstOrDefault(),
                                                                        StateCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.StateCode).FirstOrDefault(),
                                                                        CountryCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountryCode).FirstOrDefault(),
                                                                        ZipCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.ZipCode).FirstOrDefault(),
                                                                        CountyName = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountyName).FirstOrDefault()
                                                                    },
                                                                    t1.Discounts,
                                                                    t1.TaxDetails,
                                                                    t1.QbInvoiceNumber,
                                                                    PickupLocation = t1.InvoiceDispatchLocation.FirstOrDefault(t2 => t2.LocationType == (int)LocationType.PickUp),
                                                                    t1.DDTConversionReason,
                                                                    t1.IsBolImageReq,
                                                                    t1.IsSignatureReq,
                                                                    t1.IsDropImageReq
                                                                }).FirstOrDefault()
                                        }).FirstOrDefaultAsync();
                    if (order != null && order.OriginalInvoice != null)
                    {
                        SetImageFlagsToInvoiceModel(order.OriginalInvoice.IsBolImageReq, order.OriginalInvoice.IsDropImageReq, order.OriginalInvoice.IsSignatureReq, invoiceModel, manualInvoiceModel);
                        //-------------------- From SetManualInputs -----------------------------------------
                        var timeZoneName = order.Job.TimeZoneName;
                        var invoiceCommonDomain = new InvoiceCommonDomain(this);
                        invoiceCommonDomain.SetManualInputsToEditInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName);
                        SetDefaultPayamentDueDateBasis(invoiceModel, order.AcceptedCompanyId);

                        //-------------------- From Invoice Edit --------------------------------------------
                        SetInvoiceInformationToInvoiceModelForBrokerEdit(order, manualInvoiceModel, invoiceModel);

                        invoiceModel.UpdatedBy = userContext.Id;
                        var job = order.Job;
                        var droppedByUserId = manualInvoiceModel.DriverId ?? userContext.Id;
                        invoiceCommonDomain.SetAssetDropsToInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName, job, droppedByUserId);
                        if (order.Job.IsMarine)
                        {
                            SetConvertedDropQtyAndGravityForMFN(order, invoiceModel);
                        }

                        //recalculate PPG
                        if (!order.IsSuppressOrderPricing)
                        {
                            if (manualInvoiceModel.WaitingForAction != (int)WaitingAction.UpdatedPrice)
                            {
                                await invoiceCommonDomain.SetEditInvoicePricingToInvoiceModel(order, invoiceModel);
                            }
                            else
                            {
                                await invoiceCommonDomain.SetPricingToInvoiceModelForEditBrokeredOrder(order, invoiceModel);
                            }
                        }

                        //Set Bol details after pricing
                        SetBolDetailsForEdit(invoiceModel, order.FuelType, order.FuelTypeId);

                        if (invoiceModel.WaitingFor == WaitingAction.UpdatedPrice || invoiceModel.PricePerGallon <= 0)
                        {
                            LogManager.Logger.WriteInfo("InvoiceEditDomain", "GetBrokeredInvoiceEditViewModelForNewDateAsync", "Pricing not available for the selected date. Please set a new date for the invoice ID " + invoiceModel.Id);
                            return null;
                        }

                        if (order.Job.IsMarine)
                        {
                            invoiceModel.PricePerGallon = invoiceModel.ConvertedPricing ?? 0;
                        }

                        //------------Set Invoice fees from manual invoice view model------------------------
                        var assetCount = invoiceModel.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Select(t => t.AssetName).Distinct().Count();
                        ProcessInvoiceFuelFeesAndSetCalculatedValues(manualInvoiceModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                        //------------Set Invoice taxes for the product base on product type-----------------
                        var isDirectTaxCompany = Context.DataContext.DirectTaxes.Any(t => t.CompanyId == order.BuyerCompanyId && t.StateId == job.StateId && t.IsActive);
                        manualInvoiceModel.IsDirectTaxCompany = isDirectTaxCompany;
                        var serviceViewModel = new AvalaraServiceViewModel()
                        {
                            FuelTypeId = order.MappedParentId ?? order.FuelTypeId,
                            FuelProductCode = order.ProductCode,
                            JobUoM = order.UoM,
                            JobCurrency = order.Currency,
                            IsSalesTaxExempted = order.IsTaxExempted,
                            CountryCurrency = order.Job.MstCountry.Currency,
                            DestinationJobAddress = order.JobAddress,
                            SourceTerminalAddress = invoiceModel.FuelPickLocation != null ? invoiceModel.FuelPickLocation.ToAddressViewModel() : order.OriginalInvoice.TerminalAddress,
                            InvoiceNumber = invoiceModel.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFRB),
                            DroppedGallons = invoiceModel.DroppedGallons,
                            PricePerGallon = invoiceModel.PricePerGallon,
                            DropEndDate = invoiceModel.DropEndDate,
                            InvoiceDate = invoiceModel.UpdatedDate,
                            IsDirectTaxCompany = isDirectTaxCompany,
                            BuyerCompanyId = order.BuyerCompanyId,
                            SupplierCompanyId = order.AcceptedCompanyId,
                            JobId = order.Job.Id
                        };
                        if (order.BuyerTaxExemptLicence != null)
                        {
                            serviceViewModel.BuyerCustomId = order.BuyerTaxExemptLicence.EntityCustomId;
                        }
                        if (order.SupplierTaxExemptLicence != null && order.IsEndSupplier)
                        {
                            serviceViewModel.SellerCustomId = order.SupplierTaxExemptLicence.EntityCustomId;
                        }
                        var previousTaxDetails = order.OriginalInvoice.TaxDetails.ToList().ToViewModel();
                        var taxResponse = SetTaxesToEditInvoiceModel(manualInvoiceModel, invoiceModel, serviceViewModel, previousTaxDetails);
                        if (manualInvoiceModel.TaxType == TaxType.Manual && taxResponse.StatusCode == Status.Success && manualInvoiceModel.TypeofFuel == (int)ProductDisplayGroups.OtherFuelType)
                        {
                            SetTaxModifiedFlag(previousTaxDetails, invoiceModel.TaxDetails);
                        }

                        //----------Set Invoice additional details and deals from previous invoice to current invoice----------
                        await invoiceCommonDomain.SetInvoiceAdditionDetailToInvoiceModel(invoiceModel, order.OriginalInvoice.Id, manualInvoiceModel.OrderId);
                        invoiceModel.Discounts = order.OriginalInvoice.Discounts.Select(t => t.ToViewModel()).ToList();
                        invoiceCommonDomain.SetInvoiceBaseAmounts(invoiceModel, invoiceModel.ExchangeRate);

                        //--------- Construct response view model-------------------------------------------------------------
                        response = GetInvoiceEditRequestViewModel(order, manualInvoiceModel, invoiceModel);
                        response.IsTaxServiceSucceeded = manualInvoiceModel.TaxType == TaxType.Standard && taxResponse.StatusCode == Status.Success;
                        response.StatusCode = Status.Success;

                        var trackableSchedule = order.OriginalInvoice.TrackableSchedule;
                        SetTrackableScheduleInfoInResponseViewModel(manualInvoiceModel, response, invoiceModel, timeZoneName, trackableSchedule);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceEditDomain", "GetBrokeredInvoiceEditViewModelForNewDateAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private void GetTerminalDetailsById(InvoiceModel invoiceModel, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var terminalDetails = Context.DataContext.MstExternalTerminals.FirstOrDefault(t => t.Id == manualInvoiceViewModel.TerminalId && t.IsActive);
            if (terminalDetails != null)
            {
                if (invoiceModel.FuelPickLocation == null)
                    invoiceModel.FuelPickLocation = new DispatchLocationViewModel();

                invoiceModel.FuelPickLocation.Address = terminalDetails.Address;
                invoiceModel.FuelPickLocation.City = terminalDetails.City;
                invoiceModel.FuelPickLocation.StateCode = terminalDetails.StateCode;
                invoiceModel.FuelPickLocation.CountryCode = terminalDetails.CountryCode;
                invoiceModel.FuelPickLocation.ZipCode = terminalDetails.ZipCode;
                invoiceModel.FuelPickLocation.CountyName = terminalDetails.CountyName;
                invoiceModel.FuelPickLocation.StateId = terminalDetails.StateId;
                invoiceModel.FuelPickLocation.PickupLocationType = PickupLocationType.Terminal;
                invoiceModel.FuelPickLocation.CreatedBy = invoiceModel.CreatedBy;
                invoiceModel.FuelPickLocation.CreatedDate = invoiceModel.CreatedDate;
                invoiceModel.FuelPickLocation.Currency = invoiceModel.Currency;
                invoiceModel.FuelPickLocation.IsValidAddress = true;
                invoiceModel.FuelPickLocation.IsVariousFobOriginType = invoiceModel.IsVariousFobOrigin;
                invoiceModel.FuelPickLocation.OrderId = manualInvoiceViewModel.OrderId;
                invoiceModel.FuelPickLocation.TrackableScheduleId = manualInvoiceViewModel.TrackableScheduleId;
            }
        }

        public async Task<InvoiceEditResponseViewModel> EditSplitInvoiceAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new StatusViewModel();
            var updateResponse = new InvoiceEditResponseViewModel();
            try
            {
                InvoiceEditRequestViewModel invoiceEditRequest = null;
                if (manualInvoiceModel.IsQuanityOrDateChanged)
                {
                    invoiceEditRequest = await GetInvoiceEditViewModelForNewDateAsync(userContext, manualInvoiceModel);
                    if (invoiceEditRequest == null)
                    {
                        updateResponse.StatusMessage = Resource.errMessageSelectedDateCannotBeSet;
                        return updateResponse;
                    }
                }
                else
                {
                    invoiceEditRequest = await GetInvoiceEditViewModelAsync(userContext, manualInvoiceModel);
                    if (invoiceEditRequest == null)
                    {
                        updateResponse.StatusMessage = Resource.errMessageSelectedDateCannotBeSet;
                        return updateResponse;
                    }
                }
                int originalInvoiceId = manualInvoiceModel.InvoiceId;
                SetDropLocation(manualInvoiceModel, invoiceEditRequest.InvoiceModel);
                if (manualInvoiceModel.TaxType == TaxType.Standard && !invoiceEditRequest.IsTaxServiceSucceeded)
                {
                    updateResponse.StatusMessage = Resource.errMessageUpdateTaxFailed;
                    return updateResponse;
                }

                CheckForProcessingFeeOnTotalAmount(invoiceEditRequest.InvoiceModel);
                updateResponse = await UpdateSplitLoadInvoiceAsync(invoiceEditRequest);

                await new InvoiceCommonDomain(this).EditBillingStatement(manualInvoiceModel.SplitLoadChainId, invoiceEditRequest.TimeZoneName, invoiceEditRequest.SupplierCompanyId);
                response.StatusCode = updateResponse.StatusCode;
                if (updateResponse.StatusCode == Status.Failed)
                {
                    return updateResponse;
                }
                var notificationDomain = new NotificationDomain(this);
                await notificationDomain.AddNotificationEventAsync(EventType.InvoiceUpdated, updateResponse.InvoiceHeaderId, userContext.Id);
                await SetSystemAutoClosedOrderWhileEditing(invoiceEditRequest, updateResponse);

                // await SendInvoiceTaxValueChangedMessage(invoice)
                UpdateManualInvoiceViewModel(manualInvoiceModel, invoiceEditRequest, updateResponse);
                await SetInvoiceUpdatedNewsfeed(userContext, manualInvoiceModel, invoiceEditRequest.JobCompanyId);
                if (manualInvoiceModel.SplitLoadSequence == 1)
                {
                    await UpdateCommonDetailsForOtherSplitInvoices(userContext, manualInvoiceModel, originalInvoiceId, updateResponse);
                }
                response.StatusMessage = IsDigitalDropTicket(manualInvoiceModel.InvoiceTypeId)
                    ? Resource.errMessageDdtUpdatedSuccess : Resource.errMessageInvoiceUpdatedSuccess;
                UpdateInvoiceActionResponseStatus(updateResponse.IsDtnUploaded, response);
                updateResponse.StatusCode = response.StatusCode;
                updateResponse.StatusMessage = response.StatusMessage;
                await new BillingStatementDomain(this).CreateBillingStatementForEditedInvoice(invoiceEditRequest.InvoiceModel.InvoiceNumberId, userContext.Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SplitLoadInvoiceDomain", "EditSplitInvoiceAsync", ex.Message, ex);
            }
            return updateResponse;
        }

        private async Task SetSystemAutoClosedOrderWhileEditing(InvoiceEditRequestViewModel invoiceEditRequest, InvoiceEditResponseViewModel updateResponse)
        {
            if (updateResponse.IsOrderAutoClosed)
            {
                var newsfeedModel = new SystemOrderAutoClosedNewsfeedViewModel
                {
                    OrderId = invoiceEditRequest.OrderId,
                    PoNumber = invoiceEditRequest.PoNumber,
                    BuyerCompanyId = invoiceEditRequest.BuyerCompanyId,
                    SupplierCompanyId = invoiceEditRequest.SupplierCompanyId,
                    TimeZoneName = invoiceEditRequest.TimeZoneName,
                    TotalDelivered = updateResponse.OrderTotalDelivered,
                    JobCompanyId = invoiceEditRequest.JobCompanyId,
                    JobId = invoiceEditRequest.JobId,
                    UoM = invoiceEditRequest.InvoiceModel.UoM
                };
                var newsfeedDomain = new NewsfeedDomain(this);
                await newsfeedDomain.SetSystemOrderAutoClosedNewsfeed(newsfeedModel);
            }
        }

        private InvoiceEditRequestViewModel GetInvoiceEditRequestViewModel(dynamic order, ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel)
        {
            var response = new InvoiceEditRequestViewModel();
            response.OriginalDroppedGallons = manualInvoiceModel.OriginalDroppedGallons;
            response.JobId = order.Job.Id;
            response.JobCompanyId = order.JobCompanyId;
            response.OrderId = manualInvoiceModel.OrderId;
            response.PoNumber = order.PoNumber;
            response.InvoiceId = manualInvoiceModel.InvoiceId;
            response.DeliveryTypeId = order.DeliveryTypeId;
            response.OrderMaxQuantity = order.MaxQuantity;
            response.TimeZoneName = order.Job.TimeZoneName;
            response.BuyerCompanyId = order.BuyerCompanyId;
            response.SupplierCompanyId = order.AcceptedCompanyId;
            response.InvoiceModel = invoiceModel;
            response.DiscountId = manualInvoiceModel.DiscountId;
            response.IsInvoiceImagesAvailable = invoiceModel.IsInvoiceImagesAvailable;
            if (invoiceModel.TaxDetails != null && invoiceModel.TaxDetails.AvaTaxDetails.Any())
            {
                response.IsTaxManuallyModified = invoiceModel.TaxDetails.AvaTaxDetails.Any(t => t.IsModified);
            }
            return response;
        }

        private void SetInvoiceInformationToInvoiceModel(dynamic order, ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel)
        {
            string timeZoneName = order.Job.TimeZoneName;
            invoiceModel.UoM = order.UoM;
            invoiceModel.Currency = order.Currency;
            invoiceModel.Version = order.OriginalInvoice.Version + 1;
            invoiceModel.SupplierPreferredInvoiceTypeId = order.OriginalInvoice.SupplierPreferredInvoiceTypeId;
            invoiceModel.BrokeredChainId = order.OriginalInvoice.BrokeredChainId;
            invoiceModel.ExchangeRate = order.OriginalInvoice.ExchangeRate;

            invoiceModel.DisplayInvoiceNumber = manualInvoiceModel.DisplayInvoiceNumber ?? order.OriginalInvoice.DisplayInvoiceNumber;
            invoiceModel.ReferenceId = order.OriginalInvoice.ReferenceId;
            invoiceModel.QbInvoiceNumber = order.OriginalInvoice.QbInvoiceNumber;
            if (manualInvoiceModel.IsPoNumberEdit)
            {
                invoiceModel.PaymentDueDate = GetPaymentDueDate(manualInvoiceModel.PaymentTermId, manualInvoiceModel.NetDays, timeZoneName, invoiceModel.DropEndDate, invoiceModel.PaymentDueDateType, order.OriginalInvoice.UpdatedDate);
                invoiceModel.UpdatedDate = order.OriginalInvoice.UpdatedDate;

            }
            else
            {
                invoiceModel.PaymentDueDate = GetPaymentDueDate(manualInvoiceModel.PaymentTermId, manualInvoiceModel.NetDays, timeZoneName, invoiceModel.DropEndDate, invoiceModel.PaymentDueDateType);
                invoiceModel.UpdatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
            }

            invoiceModel.CreatedBy = order.OriginalInvoice.CreatedBy;
            invoiceModel.CreatedDate = order.OriginalInvoice.CreatedDate;

            invoiceModel.ParentId = order.OriginalInvoice.ParentId;
            invoiceModel.PricePerGallon = order.OriginalInvoice.PricePerGallon;
            invoiceModel.RackPrice = order.OriginalInvoice.RackPrice;
            invoiceModel.TerminalId = manualInvoiceModel.TerminalId == 0 ? order.OriginalInvoice.TerminalId : manualInvoiceModel.TerminalId;
            invoiceModel.CityGroupTerminalId = order.OriginalInvoice.CityGroupTerminalId;
            invoiceModel.WaitingFor = (WaitingAction)order.OriginalInvoice.WaitingFor;
            invoiceModel.DDTConversionReason = order.OriginalInvoice.DDTConversionReason;
            invoiceModel.IsMarineLocation = order.IsMarine;
            invoiceModel.PricingTypeId = order.PricingTypeId;

            var droppedQty = invoiceModel.DroppedGallons;
            if ((invoiceModel.PricingTypeId == (int)PricingType.PricePerGallon || invoiceModel.PricingTypeId == (int)PricingType.Suppliercost) && invoiceModel.IsMarineLocation &&
                    invoiceModel.ConvertedQuantity != null && (invoiceModel.UoM == UoM.MetricTons || invoiceModel.UoM == UoM.Barrels))
            {
                droppedQty = invoiceModel.ConvertedQuantity.Value;
            }
            if (invoiceModel.InvoiceTypeId != (int)InvoiceType.DryRun)
                invoiceModel.BasicAmount = Math.Round(droppedQty * invoiceModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
            else
                invoiceModel.BasicAmount = Math.Round(manualInvoiceModel.TotalInvoiceAmount, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);

            if (invoiceModel.AdditionalDetail == null)
                invoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
            if (!string.IsNullOrWhiteSpace(manualInvoiceModel.Notes))
            {
                invoiceModel.AdditionalDetail.Notes = manualInvoiceModel.Notes;
            }
            invoiceModel.AdditionalDetail.SupplierAllowance = (manualInvoiceModel.SupplierAllowance.HasValue && manualInvoiceModel.SupplierAllowance.Value > 0) ? manualInvoiceModel.SupplierAllowance : order.OriginalInvoice.SupplierAllowance;
            invoiceModel.AdditionalDetail.PaymentMethod = manualInvoiceModel.PaymentMethod;
            invoiceModel.AdditionalDetail.OriginalInvoiceId = manualInvoiceModel.OriginalInvoiceId;
            var waitingForOriginalStatus = (WaitingAction)order.OriginalInvoice.WaitingFor;

            var domain = new ConsolidatedInvoiceDomain();
            domain.SetNoDataExceptionStatus(invoiceModel);

            if (invoiceModel.WaitingFor == WaitingAction.PrePostDipData)
            {
                bool IsDipDataUnavailable = manualInvoiceModel.Assets.Any(t => t.PreDip == null);
                if (!IsDipDataUnavailable)
                {
                    invoiceModel.WaitingFor = WaitingAction.Nothing;
                    manualInvoiceModel.WaitingForAction = (int)WaitingAction.Nothing;
                }
            }

            if (waitingForOriginalStatus != WaitingAction.Nothing || manualInvoiceModel.WaitingForAction == (int)WaitingAction.Images)
            {
                CheckRequiredImagesAndSetWaitingForImageAction(invoiceModel);
                if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                    manualInvoiceModel.WaitingForAction = (int)WaitingAction.Nothing;

                //above method is common for create and edit, so need to write below line
                invoiceModel.InvoiceTypeId = manualInvoiceModel.InvoiceTypeId;
            }

            if (order.IsApprovalWorkflowEnabled && invoiceModel.WaitingFor == WaitingAction.Nothing && invoiceModel.IsDigitalDropTicket())
            {
                invoiceModel.WaitingFor = WaitingAction.CustomerApproval;
                manualInvoiceModel.WaitingForAction = (int)WaitingAction.CustomerApproval;
                invoiceModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                manualInvoiceModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                invoiceModel.InvoiceTypeId = manualInvoiceModel.InvoiceTypeId;
                invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
            }

            if (invoiceModel.BolDetails != null && invoiceModel.BolDetails.Any(t => t.BolNumber != null && t.BolNumber != "") && invoiceModel.WaitingFor == WaitingAction.BolDetails)
            {
                if (manualInvoiceModel.IsConvertFromDDT)
                {
                    invoiceModel.WaitingFor = WaitingAction.Nothing;
                    manualInvoiceModel.TaxType = TaxType.Standard;
                }
                else
                {
                    if (order.IsApprovalWorkflowEnabled)
                    {
                        invoiceModel.WaitingFor = WaitingAction.CustomerApproval;
                        manualInvoiceModel.WaitingForAction = (int)WaitingAction.CustomerApproval;
                        invoiceModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                        manualInvoiceModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                    }
                    else
                    {
                        invoiceModel.WaitingFor = WaitingAction.Nothing;
                        manualInvoiceModel.WaitingForAction = (int)WaitingAction.Nothing;
                    }
                }
            }

            // calculate total allowance and basic amount
            //SetTotalAllowanceToInvoiceModel(invoiceModel);

            if (manualInvoiceModel.IsFTL)
            {
                invoiceModel.AdditionalDetail.SplitLoadSequence = order.OriginalInvoice.SplitLoadSequence;
                invoiceModel.AdditionalDetail.SplitLoadChainId = order.OriginalInvoice.SplitLoadChainId;

                if (order.PricingQuantityIndicatorTypeId > 0)
                    invoiceModel.QuantityIndicatorTypeId = order.PricingQuantityIndicatorTypeId;
            }

            if (manualInvoiceModel.PickUpAddress != null && manualInvoiceModel.PickUpAddress.IsAddressAvailable)
            {
                SetBulkPlantAddress(manualInvoiceModel, invoiceModel);
            }
            //// set edited/updated terminal details
            else if (order.OriginalInvoice.PickupLocation != null)
            {
                if (manualInvoiceModel.TerminalId != 0 && manualInvoiceModel.TerminalId != order.OriginalInvoice.TerminalId)
                {
                    invoiceModel.TerminalId = manualInvoiceModel.TerminalId;
                    if (manualInvoiceModel.PickUpAddress == null || !manualInvoiceModel.PickUpAddress.IsAddressAvailable)
                    {
                        invoiceModel.IsTerminalPickup = true;
                        GetTerminalDetailsById(invoiceModel, manualInvoiceModel);
                    }
                }
                else
                {
                    invoiceModel.IsTerminalPickup = true;
                    GetTerminalDetailsById(invoiceModel, manualInvoiceModel);
                }
            }

            if (order.OriginalInvoice.BDRDetail != null)
            {
                // invoiceModel.BDRDetails = ((BDRDetail)order.OriginalInvoice.BDRDetail).ToViewModel();
                invoiceModel.BDRDetails = manualInvoiceModel.BDRDetail;
            }
        }

        private void SetInvoiceInformationToInvoiceModelForBrokerEdit(dynamic order, ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel)
        {
            string timeZoneName = order.Job.TimeZoneName;
            invoiceModel.UoM = order.UoM;
            invoiceModel.Currency = order.Currency;
            invoiceModel.Version = order.OriginalInvoice.Version + 1;
            invoiceModel.SupplierPreferredInvoiceTypeId = order.OriginalInvoice.SupplierPreferredInvoiceTypeId;
            invoiceModel.BrokeredChainId = order.OriginalInvoice.BrokeredChainId;
            invoiceModel.ExchangeRate = order.OriginalInvoice.ExchangeRate;

            invoiceModel.DisplayInvoiceNumber = manualInvoiceModel.DisplayInvoiceNumber ?? order.OriginalInvoice.DisplayInvoiceNumber;
            invoiceModel.ReferenceId = order.OriginalInvoice.ReferenceId;
            invoiceModel.QbInvoiceNumber = order.OriginalInvoice.QbInvoiceNumber;
            if (manualInvoiceModel.IsPoNumberEdit)
            {
                invoiceModel.PaymentDueDate = GetPaymentDueDate(manualInvoiceModel.PaymentTermId, manualInvoiceModel.NetDays, timeZoneName, invoiceModel.DropEndDate, invoiceModel.PaymentDueDateType, order.OriginalInvoice.UpdatedDate);
                invoiceModel.UpdatedDate = order.OriginalInvoice.UpdatedDate;

            }
            else
            {
                invoiceModel.PaymentDueDate = GetPaymentDueDate(manualInvoiceModel.PaymentTermId, manualInvoiceModel.NetDays, timeZoneName, invoiceModel.DropEndDate, invoiceModel.PaymentDueDateType);
                invoiceModel.UpdatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
            }

            var waitingForOriginalInvoice = (WaitingAction)order.OriginalInvoice.WaitingFor;
            
            invoiceModel.CreatedBy = order.OriginalInvoice.CreatedBy;
            invoiceModel.CreatedDate = order.OriginalInvoice.CreatedDate;
            invoiceModel.ParentId = order.OriginalInvoice.ParentId;
            invoiceModel.PricePerGallon = order.OriginalInvoice.PricePerGallon;
            invoiceModel.RackPrice = order.OriginalInvoice.RackPrice;
            invoiceModel.TerminalId = manualInvoiceModel.TerminalId == 0 ? order.OriginalInvoice.TerminalId : manualInvoiceModel.TerminalId;
            invoiceModel.CityGroupTerminalId = order.OriginalInvoice.CityGroupTerminalId;
            invoiceModel.DDTConversionReason = order.OriginalInvoice.DDTConversionReason;

            invoiceModel.InvoiceTypeId = manualInvoiceModel.InvoiceTypeId;
            invoiceModel.WaitingFor = (WaitingAction)manualInvoiceModel.WaitingForAction;

            if (invoiceModel.InvoiceTypeId != (int)InvoiceType.DryRun)
                invoiceModel.BasicAmount = Math.Round(invoiceModel.DroppedGallons * invoiceModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
            else
                invoiceModel.BasicAmount = Math.Round(manualInvoiceModel.TotalInvoiceAmount, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);

            if (invoiceModel.AdditionalDetail == null)
                invoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
            if (!string.IsNullOrWhiteSpace(manualInvoiceModel.Notes))
            {
                invoiceModel.AdditionalDetail.Notes = manualInvoiceModel.Notes;
            }
            invoiceModel.AdditionalDetail.SupplierAllowance = (manualInvoiceModel.SupplierAllowance.HasValue && manualInvoiceModel.SupplierAllowance.Value > 0) ? manualInvoiceModel.SupplierAllowance : order.OriginalInvoice.SupplierAllowance;
            invoiceModel.AdditionalDetail.PaymentMethod = manualInvoiceModel.PaymentMethod;
            invoiceModel.AdditionalDetail.OriginalInvoiceId = manualInvoiceModel.OriginalInvoiceId;

            // check waiting for pre-post
            if (manualInvoiceModel.WaitingForAction == (int)WaitingAction.Nothing && waitingForOriginalInvoice == WaitingAction.PrePostDipData)
            {
                bool IsDipDataUnavailable = manualInvoiceModel.Assets.Any(t => t.PreDip == null);
                if (!IsDipDataUnavailable)
                {
                    invoiceModel.WaitingFor = WaitingAction.Nothing;
                }
            }

            // check ddt to waiting for images
            if (manualInvoiceModel.WaitingForAction == (int)WaitingAction.Nothing)
            {
                CheckRequiredImagesAndSetWaitingForImageAction(invoiceModel);
                invoiceModel.InvoiceTypeId = manualInvoiceModel.InvoiceTypeId;
            }

            // set ddt to waiting for approval if end supplier ddt or ddt between buyer and middle supplier is in customer approval
            if ((manualInvoiceModel.WaitingForAction == (int)WaitingAction.CustomerApproval) ||
               (manualInvoiceModel.WaitingForAction == (int)WaitingAction.Nothing && order.IsApprovalWorkflowEnabled && invoiceModel.IsDigitalDropTicket()))
            {
                invoiceModel.WaitingFor = WaitingAction.CustomerApproval;
                manualInvoiceModel.WaitingForAction = (int)WaitingAction.CustomerApproval;
                invoiceModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                manualInvoiceModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(manualInvoiceModel.InvoiceTypeId);
            }

            // check for BOL
            if (manualInvoiceModel.WaitingForAction == (int)WaitingAction.BolDetails)
            {
                invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(manualInvoiceModel.InvoiceTypeId);
            }
            else if (invoiceModel.BolDetails != null && invoiceModel.BolDetails.Any(t => t.BolNumber != null && t.BolNumber != "") && manualInvoiceModel.WaitingForAction == (int)WaitingAction.BolDetails)
            {
                if (manualInvoiceModel.IsConvertFromDDT)
                {
                    invoiceModel.WaitingFor = WaitingAction.Nothing;
                    manualInvoiceModel.TaxType = TaxType.Standard;
                }
                else
                {
                    if (order.IsApprovalWorkflowEnabled)
                    {
                        invoiceModel.WaitingFor = WaitingAction.CustomerApproval;
                        manualInvoiceModel.WaitingForAction = (int)WaitingAction.CustomerApproval;
                        invoiceModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                        manualInvoiceModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                    }
                    else
                    {
                        invoiceModel.WaitingFor = WaitingAction.Nothing;
                        manualInvoiceModel.WaitingForAction = (int)WaitingAction.Nothing;
                    }
                }
            }

            // calculate total allowance and basic amount
            SetTotalAllowanceToInvoiceModel(invoiceModel);

            if (manualInvoiceModel.IsFTL)
            {
                invoiceModel.AdditionalDetail.SplitLoadSequence = order.OriginalInvoice.SplitLoadSequence;
                invoiceModel.AdditionalDetail.SplitLoadChainId = order.OriginalInvoice.SplitLoadChainId;

                if (order.PricingQuantityIndicatorTypeId > 0)
                    invoiceModel.QuantityIndicatorTypeId = order.PricingQuantityIndicatorTypeId;
            }

            if (manualInvoiceModel.PickUpAddress != null && manualInvoiceModel.PickUpAddress.IsAddressAvailable)
            {
                SetBulkPlantAddress(manualInvoiceModel, invoiceModel);
            }
            //// set edited/updated terminal details
            else if (order.OriginalInvoice.PickupLocation != null)
            {
                if (manualInvoiceModel.TerminalId != 0 && manualInvoiceModel.TerminalId != order.OriginalInvoice.TerminalId)
                {
                    invoiceModel.TerminalId = manualInvoiceModel.TerminalId;
                    if (manualInvoiceModel.PickUpAddress == null || !manualInvoiceModel.PickUpAddress.IsAddressAvailable)
                    {
                        invoiceModel.IsTerminalPickup = true;
                        GetTerminalDetailsById(invoiceModel, manualInvoiceModel);
                    }
                }
                else
                {
                    invoiceModel.IsTerminalPickup = true;
                    GetTerminalDetailsById(invoiceModel, manualInvoiceModel);
                }
            }
        }

        private void SetTrackableScheduleInfoInResponseViewModel(ManualInvoiceViewModel manualInvoiceModel, InvoiceEditRequestViewModel response, InvoiceModel invoiceModel, string timeZoneName, DeliveryScheduleXTrackableSchedule trackableSchedule)
        {
            response.CurrentTrackableScheduleId = manualInvoiceModel.TrackableScheduleId;
            response.CurrentTrackableScheduleStatusId = (int)DeliveryScheduleStatus.Completed;
            if (trackableSchedule != null)
            {
                response.DeliveryLevelPO = manualInvoiceModel.DeliveryLevelPO;
            }
            if (manualInvoiceModel.TrackableScheduleId != trackableSchedule?.Id)
            {
                response.PreviousTrackableScheduleId = trackableSchedule?.Id;
            }
            if(response.CurrentTrackableScheduleId.HasValue)
            {
                invoiceModel.TrackableScheduleId = response.CurrentTrackableScheduleId.Value;
            }
            if (response.PreviousTrackableScheduleId.HasValue)
            {
                response.PreviousTrackableScheduleStatusId = GetDeliveryScheduleStatus(trackableSchedule, timeZoneName, invoiceModel.StatusId, invoiceModel.DropEndDate, true);
            }
        }

        private StatusViewModel SetTaxesToEditInvoiceModel(ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel, AvalaraServiceViewModel serviceViewModel, InvoiceTaxDetailsViewModel previousTaxDetails)
        {
            var response = new StatusViewModel();
            if (manualInvoiceModel.TypeofFuel == (int)ProductDisplayGroups.OtherFuelType)
            {
                var invoiceCommonDomain = new InvoiceCommonDomain(this);
                invoiceModel.TaxDetails = invoiceCommonDomain.GetTaxDetailsFromInputs(manualInvoiceModel.Taxes, invoiceModel.Currency, invoiceModel.Id, invoiceModel.BasicAmount, invoiceModel.DroppedGallons);
                response.StatusCode = Status.Success;
            }
            else if (invoiceModel.WaitingFor == (int)WaitingAction.Nothing && !invoiceModel.IsDigitalDropTicket())
            {
                if (serviceViewModel != null && serviceViewModel.DestinationJobAddress != null
                            && serviceViewModel.DestinationJobAddress.CountryCode.IsValidCountryForTax()
                                    && serviceViewModel.SourceTerminalAddress != null && serviceViewModel.SourceTerminalAddress.CountryCode.IsValidCountryForTax())
                {
                    var taxResponse = GetTaxesBasedOnTaxCalculationType(manualInvoiceModel, serviceViewModel, previousTaxDetails, invoiceModel.FuelFees);
                    invoiceModel.TaxDetails = taxResponse.TaxDetailsViewModel;
                    response.StatusCode = taxResponse.StatusCode;
                }
                else
                    response.StatusCode = Status.Success;
            }
            if (invoiceModel.TaxDetails != null)
            {
                invoiceModel.TotalTaxAmount = invoiceModel.TaxDetails.TotalTaxAmount;
                invoiceModel.TransactionId = invoiceModel.TaxDetails.TranId.ToString();
            }
            if (string.IsNullOrWhiteSpace(invoiceModel.TransactionId) || invoiceModel.TransactionId == "0")
            {
                invoiceModel.TransactionId = invoiceModel.DisplayInvoiceNumber;
            }
            return response;
        }

        public TaxResponseViewModel GetTaxesBasedOnTaxCalculationType(ManualInvoiceViewModel manualInvoiceModel, AvalaraServiceViewModel serviceViewModel, InvoiceTaxDetailsViewModel previousTaxDetails, List<FuelFeeViewModel> fuelFees)
        {
            TaxResponseViewModel taxResponse = new TaxResponseViewModel();
            if (manualInvoiceModel.TaxType == TaxType.Standard)
            {
                var invoiceCommonDomain = new InvoiceCommonDomain(this);
                if (manualInvoiceModel.IsFTL)
                {
                    taxResponse = invoiceCommonDomain.GetFtlTaxForStandardProduct(serviceViewModel, manualInvoiceModel, fuelFees);
                }
                else
                {
                    InvoiceCreateViewModel invoiceCreateViewModel = new InvoiceCreateViewModel();
                    invoiceCreateViewModel.SupplierAllowance = manualInvoiceModel.SupplierAllowance ?? 0;
                    invoiceCreateViewModel.IsVariousFobOrigin = manualInvoiceModel.IsVariousFobOrigin;
                    if (manualInvoiceModel.PickUpAddress == null || !manualInvoiceModel.PickUpAddress.IsAddressAvailable)
                    {
                        invoiceCreateViewModel.IsTerminalPickup = true;
                    }
                    BolDetailViewModel bolDetailViewModel = new BolDetailViewModel() { NetQuantity = manualInvoiceModel.BolDetails?.NetQuantity, GrossQuantity = manualInvoiceModel.BolDetails?.GrossQuantity };
                    taxResponse = invoiceCommonDomain.GetTaxForStandardProduct(serviceViewModel, invoiceCreateViewModel, bolDetailViewModel, fuelFees);
                }
            }
            else if (manualInvoiceModel.TaxType == TaxType.Manual)
            {
                taxResponse.TaxDetailsViewModel = GetTaxDetailsForInternalTaxValues(manualInvoiceModel, manualInvoiceModel.InvoiceId, previousTaxDetails);
                taxResponse.StatusCode = Status.Success;
            }
            return taxResponse;
        }

        private void ProcessInvoiceFuelFeesAndSetCalculatedValues(ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel, DateTimeOffset dropEndDate, int assetCount)
        {
            if (manualInvoiceModel.ExternalBrokerId > 0 && manualInvoiceModel.IsThirdPartyHardwareUsed)
            {
                invoiceModel.FuelFees = manualInvoiceModel.ExternalBrokeredOrder.BrokeredOrderFee.ToInvoiceModelFuelFees();
            }
            else
            {
                invoiceModel.FuelFees = manualInvoiceModel.FuelDeliveryDetails.FuelFees.ToInvoiceModelFuelFees(dropEndDate);
            }
            invoiceModel.FuelFees.ForEach(t => { t.Currency = invoiceModel.Currency; t.UoM = invoiceModel.UoM; });
            invoiceModel.FuelFees.SelectMany(t => t.FeeByQuantities).ToList().ForEach(t =>
            {
                t.Currency = invoiceModel.Currency;
                t.UoM = invoiceModel.UoM;
            });
            FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
            fuelFeesDomain.CalculateAndSetTotalFeeAndQuantityToFuelFees(invoiceModel, assetCount);

            invoiceModel.TotalFeeAmount = invoiceModel.FuelFees.Where(t => t.DiscountLineItemId == null && !t.IncludeInPPG).Sum(t => t.TotalFee ?? 0);
            invoiceModel.TotalDiscountAmount = invoiceModel.FuelFees.Where(t => t.DiscountLineItemId != null).Sum(t => t.TotalFee ?? 0);

            //WAITING FOR CONFIRMATION FROM SONALI
            if (invoiceModel.FuelFees.Any(t => t.IncludeInPPG))
            {
                invoiceModel.BasicAmount += invoiceModel.FuelFees.Where(t => t.IncludeInPPG).Select(t => t.TotalFee ?? 0).Sum();
                invoiceModel.PricePerGallon = invoiceModel.BasicAmount / invoiceModel.DroppedGallons;

                // update ppg in bol list
                invoiceModel.BolDetails.ForEach(t => t.PricePerGallon = invoiceModel.PricePerGallon);
            }
        }

        private void SetTaxModifiedFlag(InvoiceTaxDetailsViewModel previousTaxDetails, InvoiceTaxDetailsViewModel currentTaxDetails)
        {
            if (!Enumerable.SequenceEqual(previousTaxDetails.AvaTaxDetails.Select(t => t.TradingTaxAmount.GetPreciseValue(2)), currentTaxDetails.AvaTaxDetails.Select(t => t.TradingTaxAmount.GetPreciseValue(2))))
            {
                for (int i = 0; i < currentTaxDetails.AvaTaxDetails.Count; i++)
                {
                    if (previousTaxDetails.AvaTaxDetails.Count == 0)
                    {
                        currentTaxDetails.AvaTaxDetails[i].IsModified = true;
                    }
                    else
                    {
                        if (currentTaxDetails.AvaTaxDetails[i].TradingTaxAmount.GetPreciseValue(2) != previousTaxDetails.AvaTaxDetails[i].TradingTaxAmount.GetPreciseValue(2))
                        {
                            currentTaxDetails.AvaTaxDetails[i].IsModified = true;
                        }
                    }
                }
            }
        }

        public async Task<StatusViewModel> EditDraftDDTAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new StatusViewModel();
            try
            {
                var invoiceEditRequest = await GetDraftDDTViewModelAsync(userContext, manualInvoiceModel);
                if (manualInvoiceModel.TaxType == TaxType.Standard && !invoiceEditRequest.IsTaxServiceSucceeded)
                {
                    response.StatusMessage = Resource.errMessageUpdateTaxFailed;
                    return response;
                }

                var updateResponse = await UpdateInvoiceAsync(invoiceEditRequest);
                response.StatusCode = updateResponse.StatusCode;
                if (updateResponse.StatusCode == Status.Failed)
                {
                    return response;
                }
                //else if(updateResponse.StatusCode == Status.Success)
                //{
                //    await AddBrokerInvoicesToQueueServiceForEdit(updateResponse.InvoiceId, manualInvoiceModel);
                //}

                var notificationDomain = new NotificationDomain(this);
                await notificationDomain.AddNotificationEventAsync(EventType.InvoiceUpdated, updateResponse.InvoiceHeaderId, userContext.Id);
                await SetSystemAutoClosedOrderWhileEditing(invoiceEditRequest, updateResponse);

                UpdateManualInvoiceViewModel(manualInvoiceModel, invoiceEditRequest, updateResponse);
                await SetInvoiceUpdatedNewsfeed(userContext, manualInvoiceModel, invoiceEditRequest.JobCompanyId);

                response.EntityId = updateResponse.InvoiceId;
                response.StatusMessage = IsDigitalDropTicket(manualInvoiceModel.InvoiceTypeId)
                    ? Resource.errMessageDdtUpdatedSuccess : Resource.errMessageInvoiceUpdatedSuccess;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "EditDraftDDTAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<InvoiceEditRequestViewModel> GetDraftDDTViewModelAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new InvoiceEditRequestViewModel();
            using (var tracer = new Tracer("InvoiceEditDomain", "GetDraftDDTViewModelAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel() { IsActive = true };
                    var order = await Context.DataContext.Orders.Where(t => t.Id == manualInvoiceModel.OrderId)
                                        .Select(t => new
                                        {
                                            t.FuelRequest.Job,
                                            t.FuelRequest.Job.JobBudget.IsTaxExempted,
                                            JobAddress = new AddressViewModel
                                            {
                                                Address = t.FuelRequest.Job.Address,
                                                City = t.FuelRequest.Job.City,
                                                StateCode = t.FuelRequest.Job.MstState.Code,
                                                CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                                ZipCode = t.FuelRequest.Job.ZipCode,
                                                CountyName = t.FuelRequest.Job.CountyName
                                            },
                                            t.FuelRequest.UoM,
                                            t.FuelRequest.Currency,
                                            t.FuelRequest.FuelTypeId,
                                            t.FuelRequest.PricingTypeId,
                                            t.FuelRequest.Job.IsMarine,
                                            FuelType = t.FuelRequest.MstProduct.DisplayName,
                                            t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                            t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId,
                                            t.FuelRequest.MaxQuantity,
                                            t.FuelRequest.FuelRequestPricingDetail,
                                            t.BuyerCompanyId,
                                            t.AcceptedCompanyId,
                                            t.PoNumber,
                                            t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                            SupplierTaxExemptLicence = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            t.IsEndSupplier,
                                            t.FuelRequest.CreationTimeRackPPG,
                                            BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                            JobCompanyId = t.FuelRequest.Job.CompanyId,
                                            OriginalInvoice = t.Invoices.Where(t1 => t1.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id
                                                                && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                .Select(t1 => new
                                                                {
                                                                    t1.Id,
                                                                    t1.CreatedBy,
                                                                    t1.CreatedDate,
                                                                    t1.UpdatedDate,
                                                                    t1.InvoiceTypeId,
                                                                    t1.SupplierPreferredInvoiceTypeId,
                                                                    t1.BrokeredChainId,
                                                                    t1.DisplayInvoiceNumber,
                                                                    t1.ReferenceId,
                                                                    t1.ExchangeRate,
                                                                    t1.ParentId,
                                                                    PricePerGallon = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.PricePerGallon).FirstOrDefault(),
                                                                    TerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                                    CityGroupTerminalId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.CityGroupTerminalId).FirstOrDefault(),
                                                                    RackPrice = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.RackPrice).FirstOrDefault(),
                                                                    BolWithTier = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.InvoiceTierPricingDetails.FirstOrDefault()).FirstOrDefault(),
                                                                    t1.WaitingFor,
                                                                    t1.Version,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadChainId,
                                                                    t1.InvoiceXAdditionalDetail.SplitLoadSequence,
                                                                    t1.InvoiceXAdditionalDetail.SupplierAllowance,
                                                                    t1.InvoiceXInvoiceStatusDetails.FirstOrDefault(t2 => t2.IsActive).StatusId,
                                                                    TrackableSchedule = t1.TrackableSchedule,
                                                                    TerminalAddress = new AddressViewModel
                                                                    {
                                                                        Address = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.Address).FirstOrDefault(),
                                                                        City = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.City).FirstOrDefault(),
                                                                        StateCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.StateCode).FirstOrDefault(),
                                                                        CountryCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountryCode).FirstOrDefault(),
                                                                        ZipCode = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.ZipCode).FirstOrDefault(),
                                                                        CountyName = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail.MstExternalTerminal.CountyName).FirstOrDefault()
                                                                    },
                                                                    t1.Discounts,
                                                                    t1.TaxDetails,
                                                                    t1.QbInvoiceNumber,
                                                                    PickupLocation = t1.InvoiceDispatchLocation.FirstOrDefault(t2 => t2.LocationType == (int)LocationType.PickUp),
                                                                    t1.DDTConversionReason,
                                                                    t1.IsBolImageReq,
                                                                    t1.IsSignatureReq,
                                                                    t1.IsDropImageReq,
                                                                    t1.BDRDetail
                                                                }).FirstOrDefault()
                                        }).FirstOrDefaultAsync();
                    if (order != null && order.OriginalInvoice != null)
                    {
                        SetImageFlagsToInvoiceModel(order.OriginalInvoice.IsBolImageReq, order.OriginalInvoice.IsDropImageReq, order.OriginalInvoice.IsSignatureReq, invoiceModel, manualInvoiceModel);
                        //-------------------- From SetManualInputs -----------------------------------------
                        var timeZoneName = order.Job.TimeZoneName;
                        var invoiceCommonDomain = new InvoiceCommonDomain(this);
                        invoiceCommonDomain.SetManualInputsToEditInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName);
                        SetDefaultPayamentDueDateBasis(invoiceModel, order.AcceptedCompanyId);

                        //-------------------- From Invoice Edit --------------------------------------------
                        SetInvoiceInformationToInvoiceModel(order, manualInvoiceModel, invoiceModel);
                        invoiceModel.UpdatedBy = userContext.Id;

                        //Set Bol details after pricing
                        SetBolDetailsForEdit(invoiceModel, order.FuelType, order.FuelTypeId);

                        var job = order.Job;
                        var droppedByUserId = manualInvoiceModel.DriverId ?? userContext.Id;
                        invoiceCommonDomain.SetAssetDropsToInvoiceModel(manualInvoiceModel, invoiceModel, timeZoneName, job, droppedByUserId);

                        //------------Set Invoice fees from manual invoice view model------------------------
                        var assetCount = invoiceModel.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Select(t => t.AssetName).Distinct().Count();
                        ProcessInvoiceFuelFeesAndSetCalculatedValues(manualInvoiceModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                        //-------------------------------- set PPG from order for draft ddt -----------------------------------------------------------
                        if (order.OriginalInvoice.StatusId == (int)InvoiceStatus.Draft)
                        {
                            invoiceModel.PricePerGallon = order.CreationTimeRackPPG;
                        }
                        if (string.IsNullOrWhiteSpace(invoiceModel.TransactionId) || invoiceModel.TransactionId == "0")
                        {
                            invoiceModel.TransactionId = invoiceModel.DisplayInvoiceNumber;
                        }

                        //----------Set Invoice additional details and deals from previous invoice to current invoice----------
                        await invoiceCommonDomain.SetInvoiceAdditionDetailToInvoiceModel(invoiceModel, order.OriginalInvoice.Id, manualInvoiceModel.OrderId);
                        invoiceModel.AdditionalDetail.SplitLoadChainId = manualInvoiceModel.SplitLoadChainId;
                        invoiceModel.AdditionalDetail.SplitLoadSequence = manualInvoiceModel.SplitLoadSequence;
                        invoiceModel.Discounts = order.OriginalInvoice.Discounts.Select(t => t.ToViewModel()).ToList();
                        invoiceCommonDomain.SetInvoiceBaseAmounts(invoiceModel, invoiceModel.ExchangeRate);

                        //--------- Construct response view model-------------------------------------------------------------
                        response = GetInvoiceEditRequestViewModel(order, manualInvoiceModel, invoiceModel);
                        response.StatusCode = Status.Success;

                        var trackableSchedule = order.OriginalInvoice.TrackableSchedule;
                        SetTrackableScheduleInfoInResponseViewModel(manualInvoiceModel, response, invoiceModel, timeZoneName, trackableSchedule);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceEditDomain", "GetDraftDDTViewModelAsync", ex.Message, ex);
                }
            }
            return response;
        }


        private InvoiceTaxDetailsViewModel GetTaxDetailsForInternalTaxValues(ManualInvoiceViewModel viewModel, int tranId, InvoiceTaxDetailsViewModel prevTaxDetails)
        {
            var taxDetailsViewModel = new InvoiceTaxDetailsViewModel()
            {
                TranId = tranId,
                ReturnCode = tranId
            };

            taxDetailsViewModel.AvaTaxDetails = new List<TaxDetailsViewModel>();

            foreach (var item in viewModel.TaxDetails.AvaTaxDetails)
            {
                var previousTax = prevTaxDetails.AvaTaxDetails.FirstOrDefault(t => t.Id == item.Id);
                if (previousTax != null)
                {
                    //tax details from edit.
                    taxDetailsViewModel.AvaTaxDetails.Add(new TaxDetailsViewModel()
                    {
                        CalculationTypeInd = previousTax.CalculationTypeInd,
                        Currency = previousTax.Currency,
                        ProductCategory = previousTax.ProductCategory,
                        RateDescription = previousTax.RateDescription,
                        RelatedLineItem = previousTax.RelatedLineItem,
                        RateSubtype = previousTax.RateSubtype,
                        RateType = previousTax.RateType,
                        SalesTaxBaseAmount = previousTax.SalesTaxBaseAmount,
                        TaxAmount = previousTax.TaxAmount,
                        TaxExemptionInd = previousTax.TaxExemptionInd,
                        TaxRate = previousTax.TradingTaxAmount != item.TradingTaxAmount ? GetUpdatedTaxRate(previousTax.TaxRate, previousTax.TradingTaxAmount, item.TradingTaxAmount) : previousTax.TaxRate,
                        TaxType = previousTax.TaxType,
                        TaxingLevel = previousTax.TaxingLevel,
                        UnitOfMeasure = previousTax.UnitOfMeasure,
                        TradingTaxAmount = item.TradingTaxAmount,
                        TradingCurrency = previousTax.TradingCurrency,
                        ExchangeRate = previousTax.ExchangeRate,
                        IsModified = previousTax.TradingTaxAmount != item.TradingTaxAmount && viewModel.InvoiceTypeId != (int)InvoiceType.PartialCredit
                    });

                    if (previousTax.TaxExemptionInd != "Y")
                    {
                        taxDetailsViewModel.TotalTaxAmount += item.TradingTaxAmount;
                    }
                }
            }

            return taxDetailsViewModel;
        }

        private decimal GetUpdatedTaxRate(decimal prevTaxRate, decimal prevTaxAmount, decimal currentTaxAmount)
        {
            if (prevTaxAmount != 0)
            {
                return (prevTaxRate * currentTaxAmount) / prevTaxAmount;
            }
            else
            {
                return currentTaxAmount;
            }
        }

        private int GetDeliveryScheduleStatus(DeliveryScheduleXTrackableSchedule deliveryScheduleXTrackableSchedule, string timeZoneName, int invoiceStatusId, DateTimeOffset dropEndDate, bool isEditing = false)
        {
            int deliveryStatusId = (int)TrackableDeliveryScheduleStatus.Accepted;
            var statusId = deliveryScheduleXTrackableSchedule.DeliverySchedule.StatusId;

            if (isEditing)
            {
                var currentDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
                if (currentDateTime.Date > deliveryScheduleXTrackableSchedule.Date.Date)
                {
                    return (int)TrackableDeliveryScheduleStatus.Missed;
                }
                else if (currentDateTime.Date == deliveryScheduleXTrackableSchedule.Date.Date)
                {
                    if (currentDateTime.Hour > deliveryScheduleXTrackableSchedule.EndTime.Hours)
                    {
                        return (int)TrackableDeliveryScheduleStatus.Missed;
                    }
                    else
                    {
                        return (int)TrackableDeliveryScheduleStatus.Accepted;
                    }
                }
            }
            else
            {
                if (invoiceStatusId == (int)InvoiceStatus.Draft)
                {
                    return (int)TrackableDeliveryScheduleStatus.Discontinued;
                }
                else
                {
                    if (dropEndDate.Date <= deliveryScheduleXTrackableSchedule.Date.Date)
                    {
                        if (dropEndDate.Hour <= deliveryScheduleXTrackableSchedule.EndTime.Hours)
                        {
                            return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledCompleted : (int)TrackableDeliveryScheduleStatus.Completed;
                        }
                        else
                        {
                            return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledLate : (int)TrackableDeliveryScheduleStatus.CompletedLate;
                        }
                    }
                    else if (dropEndDate.Date > deliveryScheduleXTrackableSchedule.Date.Date)
                    {
                        return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledLate : (int)TrackableDeliveryScheduleStatus.CompletedLate;
                    }
                }
            }
            return deliveryStatusId;
        }

        private static void SetBulkPlantAddress(ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel)
        {
            invoiceModel.FuelPickLocation = new DispatchLocationViewModel()
            {
                Address = manualInvoiceModel.PickUpAddress.Address,
                City = manualInvoiceModel.PickUpAddress.City,
                StateCode = manualInvoiceModel.PickUpAddress.State.Code,
                StateId = manualInvoiceModel.PickUpAddress.State.Id,
                ZipCode = manualInvoiceModel.PickUpAddress.ZipCode,
                CountryCode = manualInvoiceModel.PickUpAddress.Country.Code,
                Latitude = manualInvoiceModel.PickUpAddress.Latitude,
                Longitude = manualInvoiceModel.PickUpAddress.Longitude,
                CountyName = manualInvoiceModel.PickUpAddress.CountyName,
                LocationType = (int)LocationType.PickUp,
                CreatedBy = invoiceModel.CreatedBy,
                CreatedDate = invoiceModel.CreatedDate,
                PickupLocationType = PickupLocationType.BulkPlant,
                Currency = invoiceModel.Currency,
                IsValidAddress = true,
                IsVariousFobOriginType = invoiceModel.IsVariousFobOrigin,
                OrderId = manualInvoiceModel.OrderId,
                TrackableScheduleId = manualInvoiceModel.TrackableScheduleId,
                SiteName = manualInvoiceModel.PickUpAddress.SiteName

            };
        }

        public async Task<StatusViewModel> DDTEditForImagesAsync(UserContext userContext, int invoiceId, int? orderId, ViewModels.MobileAPI.TPD.TPDImagesToUpdate imagesToUpdate, WaitingAction updatedWaitingAction = WaitingAction.Images)
        {
            var response = new StatusViewModel();
            try
            {
                ConsolidatedInvoiceEditViewModel invoiceEditRequest = null;

                invoiceEditRequest = await GetInvoiceEditForEditInvoiceNumber(userContext, invoiceId, string.Empty, true);
                if (invoiceEditRequest?.invoiceModels != null && invoiceEditRequest.invoiceModels.Any())
                {
                    foreach (var ddt in invoiceEditRequest.invoiceModels)
                    {
                        //ddt.Image = imagesToUpdate.DropImage;
                        if (imagesToUpdate.DropImage != null)
                        {
                            ddt.Image = imagesToUpdate.DropImage;
                        }

                        if (imagesToUpdate.BolImage != null && !string.IsNullOrWhiteSpace(imagesToUpdate.BolImage.FilePath))
                        {
                            ddt.BolDetails.ForEach(t => t.Image = imagesToUpdate.BolImage);
                            ddt.BolImage = imagesToUpdate.BolImage;
                        }
                        ddt.Signature = imagesToUpdate.SignatureImage?.ToCustomerSignature();
                        if (imagesToUpdate.AdditionalImage != null)
                        {
                            ddt.AdditionalImage = imagesToUpdate.AdditionalImage;
                        }

                        ddt.WaitingFor = updatedWaitingAction;
                        if (ddt.AdditionalDetail != null)
                            ddt.AdditionalDetail.ExternalRefId = imagesToUpdate.ExternalRefID;
                    }

                    response = await UpdateDDTForImagesAsync(invoiceEditRequest, orderId);
                    if (response.StatusCode == Status.Failed)
                    {
                        return response;
                    }
                    response.StatusMessage = IsDigitalDropTicket(invoiceEditRequest.invoiceModels.FirstOrDefault().InvoiceTypeId)
                        ? Resource.errMessageDdtUpdatedSuccess : Resource.errMessageInvoiceUpdatedSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceEditDomain", "DDTEditForImagesAsync", ex.Message, ex);
            }
            return response;
        }

        private void SetConvertedDropQtyAndGravityForMFN(dynamic order, InvoiceModel invoice)
        {
            var modelForConversion = new MFNConversionRequestViewModel() { DroppedGallons = invoice.DroppedGallons, JobCountryId = order.CountryId, UoM = invoice.UoM };
            if (invoice.UoM == UoM.MetricTons || invoice.UoM == UoM.Barrels)
            {
                if (invoice.UoM == UoM.MetricTons)
                {

                    if (invoice.ConversionFactor.HasValue && invoice.ConversionFactor.Value > 0)
                    {
                        if (invoice.ConvertedQuantity == null || invoice.ConvertedQuantity == 0) //for broker case, it will not calculate again
                        {
                            SetQuantityFromConversionFactorUserValue(invoice);
                        }
                    }
                    else
                    {
                        var gravity = invoice.Gravity.HasValue && invoice.Gravity > 0 ? invoice.Gravity.Value : 0;

                        modelForConversion.ConversionFactor = gravity;
                        if (modelForConversion.ConversionFactor > 0)
                        {
                            var invoiceDomain = new InvoiceDomain(this);
                            var conversionResponse = Task.Run(() => invoiceDomain.ValidateGravityAndConvertForMFN(modelForConversion)).Result;
                            invoice.ConvertedQuantity = conversionResponse.ConvertedQty;

                            if (invoice.AssetDrops != null && invoice.AssetDrops.Any())
                            {
                                invoice.AssetDrops.ForEach(asset => asset.Gravity = invoice.Gravity);
                                foreach (var asset in invoice.AssetDrops)
                                {
                                    if (asset.Gravity.HasValue && asset.Gravity.Value > 0)
                                    {
                                        var conversionRequest = new MFNConversionRequestViewModel() { DroppedGallons = asset.DropGallons.Value, ConversionFactor = asset.Gravity.Value, JobCountryId = order.CountryId, UoM = invoice.UoM };
                                        var response = Task.Run(() => new InvoiceDomain(this).ValidateGravityAndConvertForMFN(conversionRequest)).Result;
                                        asset.DropGallons = response.ConvertedQty;

                                    }
                                }
                            }
                        }
                        else if (invoice.UoM == UoM.Barrels)
                        {
                            var invoiceDomain = new InvoiceDomain(this);
                            var conversionResponse = Task.Run(() => invoiceDomain.ValidateGravityAndConvertForMFN(modelForConversion)).Result;
                            invoice.ConvertedQuantity = conversionResponse.ConvertedQty;

                            if (invoice.AssetDrops != null && invoice.AssetDrops.Any())
                            {
                                foreach (var asset in invoice.AssetDrops)
                                {
                                    var conversionRequest = new MFNConversionRequestViewModel() { DroppedGallons = asset.DropGallons.Value, JobCountryId = order.CountryId, UoM = invoice.UoM };
                                    var response = Task.Run(() => new InvoiceDomain(this).ValidateGravityAndConvertForMFN(conversionRequest)).Result;
                                    asset.DropGallons = response.ConvertedQty;
                                }
                            }

                        }
                    }
                }
            }
        }
    }
}
