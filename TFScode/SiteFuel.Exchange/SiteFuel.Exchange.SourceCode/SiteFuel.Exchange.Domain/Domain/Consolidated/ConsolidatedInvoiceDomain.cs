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
using SiteFuel.Exchange.ViewModels.FilldService;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
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
    public class ConsolidatedInvoiceDomain : InvoiceCommonDomain
    {
        public ConsolidatedInvoiceDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ConsolidatedInvoiceDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public InvoiceViewModelNew GetInvoiceViewModelFromBlob(InvoiceBulkUploadProcessorReqViewModel viewModel)
        {
            InvoiceViewModelNew invoiceViewModel = null;
            if (!string.IsNullOrWhiteSpace(viewModel.FileUploadPath))
            {
                var azureBlob = new AzureBlobStorage();
                var fileStream = azureBlob.DownloadBlob(viewModel.FileUploadPath, BlobContainerType.CreateInvoice.ToString().ToLower());
                if (fileStream != null)
                {
                    string createInvoiceJson = new StreamReader(fileStream).ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(createInvoiceJson))
                    {
                        invoiceViewModel = JsonConvert.DeserializeObject<InvoiceViewModelNew>(createInvoiceJson);
                    }
                }
            }
            return invoiceViewModel;
        }

        public async Task<List<string>> CreateManualInvoiceFromQueueService(InvoiceBulkUploadProcessorReqViewModel viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("ConsolidatedInvoiceDomain", "CreateManualInvoiceFromQueueService"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    if (!string.IsNullOrWhiteSpace(viewModel.FileUploadPath))
                    {
                        var createInvoiceViewModel = GetInvoiceViewModelFromBlob(viewModel);
                        createInvoiceViewModel.SupplierCompanyId = viewModel.SupplierCompanyId;
                        UserContext userContext = new UserContext() { Id = viewModel.SupplierId, CompanyId = viewModel.SupplierCompanyId };
                        StatusViewModel response = null;
                        if (createInvoiceViewModel.IsExceptionApprove)
                        {
                            response = await ApproveExceptionDropTicket(userContext, createInvoiceViewModel);
                        }
                        else if (createInvoiceViewModel.IsRebillInvoice)
                        {
                            response = await CreateRebillInvoice(userContext, createInvoiceViewModel);
                        }
                        else
                        {
                            var invoiceNumber = await GenerateInvoiceNumber_New(createInvoiceViewModel.ExistingHeaderId);
                            var consolidatedModel = await GetConsolidatedInvoiceModels(userContext, createInvoiceViewModel, invoiceNumber);
                            response = await ProcessManualInvoiceCreation(userContext, createInvoiceViewModel, consolidatedModel, invoiceNumber);
                        }
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
                        LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "CreateManualInvoiceFromQueueService", ex.Message, ex);
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
        /// <summary>
        /// To Bipass Queue service developed as suggest Rajiv on 03/03/2020
        /// </summary>
        /// <param name="createInvoiceViewModel">Invoice details with BOL details</param>
        /// <returns></returns>
        public async Task<StatusViewModel> CreateManualInvoice(UserContext userContext, InvoiceViewModelNew createInvoiceViewModel)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                //Check for if BOL having any null GrossQuantity and NetQuantity
                if (createInvoiceViewModel.BolDetails.Count > 0)
                {
                    RemoveEmptyBOLInManualInvoice(createInvoiceViewModel);
                }
                //Check for if Lift having any null GrossQuantity and NetQuantity
                if (createInvoiceViewModel.TicketDetails.Count > 0)
                {
                    RemoveEmptyLiftInManualInvoice(createInvoiceViewModel);
                }

                var isduplicateInvoiceNumber = IsDuplicateInvoiceNumber(createInvoiceViewModel.SupplierInvoiceNumber, createInvoiceViewModel.OriginalInvoiceHeaderId ?? 0);
                if (!isduplicateInvoiceNumber)
                {
                    if (createInvoiceViewModel.IsRebillInvoice)
                    {
                        response = await CreateRebillInvoice(userContext, createInvoiceViewModel);
                    }
                    else
                    {
                        var invoiceNumber = await GenerateInvoiceNumber_New(createInvoiceViewModel.ExistingHeaderId);
                        var consolidatedModel = await GetConsolidatedInvoiceModels(userContext, createInvoiceViewModel, invoiceNumber);
                        response = await ProcessManualInvoiceCreation(userContext, createInvoiceViewModel, consolidatedModel, invoiceNumber);
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = ResourceMessages.GetMessage(Resource.valMessageAlreadyExist, new object[] { Resource.lblInvoiceNumber });
                }
            }
            catch (Exception ex)
            {
                if (!(ex is QueueMessageFatalException))
                {
                    LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "CreateManualInvoice", ex.Message, ex);
                }
            }
            return response;
        }

        private bool ValidateBDRDetails(InvoiceViewModelNew createInvoiceViewModel, UserContext userContext)
        {
            if (createInvoiceViewModel.Drops.Any(t => t.IsMarineLocation))
            {
                if (createInvoiceViewModel.Drops.Any(t => t.IsMarineLocation && string.IsNullOrWhiteSpace(t.BdrDetails.CloseMeterReading)))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Used to remove empty BOL while creating LTL Invoice.
        /// </summary>
        /// <param name="createInvoiceViewModel">Object contain list of invoice ,drop and BOl list.</param>
        public void RemoveEmptyBOLInManualInvoice(InvoiceViewModelNew createInvoiceViewModel)
        {
            try
            {
                //Check for if BOL having any null GrossQuantity and NetQuantity
                foreach (var DropType in createInvoiceViewModel.Drops)
                {
                    createInvoiceViewModel.BolDetails.ForEach(item => item.Products.RemoveAll(k => k.GrossQuantity == null && k.NetQuantity == null && k.DeliveredQuantity == null));
                }
            }

            catch (Exception ex)
            {
                if (!(ex is QueueMessageFatalException))
                {
                    LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "RemoveEmptyBOLInManualInvoice", ex.Message, ex);
                }
            }
        }
        public void RemoveEmptyLiftInManualInvoice(InvoiceViewModelNew createInvoiceViewModel)
        {

            try
            {
                //Check for if Lift having any null GrossQuantity and NetQuantity
                foreach (var DropType in createInvoiceViewModel.Drops)
                {
                    createInvoiceViewModel.TicketDetails.ForEach(item => item.Products.RemoveAll(k => k.GrossQuantity == null && k.NetQuantity == null && k.DeliveredQuantity == null));
                }
            }

            catch (Exception ex)
            {
                if (!(ex is QueueMessageFatalException))
                {
                    LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "RemoveEmptyLiftInManualInvoice", ex.Message, ex);
                }
            }
        }
        public async Task<StatusViewModel> ApproveExceptionDropTicket(UserContext userContext, InvoiceViewModelNew createInvoiceViewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("ConsolidatedInvoiceDomain", "ApproveExceptionDropTicket"))
            {
                try
                {
                    //Check for if BOL having any null GrossQuantity and NetQuantity
                    if (createInvoiceViewModel.BolDetails.Count > 0)
                    {
                        RemoveEmptyBOLInManualInvoice(createInvoiceViewModel);
                    }
                    if (createInvoiceViewModel.TicketDetails.Count > 0)
                    {
                        RemoveEmptyLiftInManualInvoice(createInvoiceViewModel);
                    }
                    var invoiceIds = createInvoiceViewModel.Drops.Select(t => t.InvoiceId).ToList();
                    if (invoiceIds.Any())
                    {
                        var invoiceAssetDrops = Context.DataContext.AssetDrops.Where(t => invoiceIds.Contains(t.InvoiceId ?? 0)).ToList();
                        foreach (var invoice in createInvoiceViewModel.Drops)
                        {
                            invoice.Assets.Clear();
                            decimal invoiceDroppedGallons = invoice.ActualDropQuantity;
                            decimal remainingAssetQuantity = invoiceDroppedGallons;
                            var assetDrops = invoiceAssetDrops.Where(t => t.InvoiceId == invoice.InvoiceId).ToList();
                            for (int i = 0; i < assetDrops.Count; i++)
                            {
                                if (remainingAssetQuantity > 0)
                                {
                                    var assetDropModel = assetDrops[i].ToDropViewModel();
                                    if (assetDropModel.DropGallons > remainingAssetQuantity)
                                    {
                                        assetDropModel.DropGallons = remainingAssetQuantity;
                                    }
                                    else if (i == assetDrops.Count - 1 && assetDropModel.DropGallons < remainingAssetQuantity)
                                    {
                                        assetDropModel.DropGallons = remainingAssetQuantity;
                                    }
                                    remainingAssetQuantity -= assetDropModel.DropGallons ?? 0;
                                    invoice.Assets.Add(assetDropModel);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    var invoiceNumber = await Context.DataContext.InvoiceNumbers.FirstOrDefaultAsync(t => t.Id == createInvoiceViewModel.OriginalInvoiceNumberId);
                    var consolidatedModel = await GetConsolidatedInvoiceModels(userContext, createInvoiceViewModel, invoiceNumber);
                    response = await ApproveExceptionDropTicket(createInvoiceViewModel, consolidatedModel, invoiceNumber);
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                    {
                        LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "ApproveExceptionDropTicket", ex.Message, ex);
                    }
                }
                return response;
            }
        }

        public async Task<List<string>> CreateMobileInvoiceFromQueueService(InvoiceBulkUploadProcessorReqViewModel viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("ConsolidatedInvoiceDomain", "CreateMobileInvoiceFromQueueService"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    if (!string.IsNullOrWhiteSpace(viewModel.FileUploadPath))
                    {
                        var createInvoiceViewModel = GetInvoiceViewModelFromBlob(viewModel);
                        if (IsDuplicateMobileDrop(createInvoiceViewModel.TraceId, viewModel.SupplierCompanyId) && IsDuplicateMobileDropForSchedule(createInvoiceViewModel.Drops, viewModel.SupplierCompanyId))
                        {
                            UserContext userContext = new UserContext() { Id = viewModel.SupplierId, CompanyId = viewModel.SupplierCompanyId };
                            var invoiceNumber = await GenerateInvoiceNumber_New();
                            var consolidatedModel = await GetConsolidatedInvoiceModels(userContext, createInvoiceViewModel, invoiceNumber);
                            var response = new StatusViewModel();
                            if (createInvoiceViewModel.DiversionType == DiversionType.Full)
                                response = await ProcessMobileInvoiceForDryRun(userContext, createInvoiceViewModel, invoiceNumber);
                            else
                                response = await ProcessMobileInvoiceCreation(userContext, createInvoiceViewModel, consolidatedModel, invoiceNumber);
                            if (response.StatusCode == Status.Failed)
                            {
                                errorInfo.Add(response.StatusMessage);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "CreateMobileInvoiceFromQueueService", ex.Message, ex);
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

        private bool IsDuplicateMobileDrop(string traceId, int supplierCompanyId)
        {
            bool response = true;
            if (!string.IsNullOrEmpty(traceId) && Context.DataContext.Invoices.Any(t => t.TraceId == traceId && t.Order.AcceptedCompanyId == supplierCompanyId))
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "IsDuplicateMobileDrop",
                    string.Format(Resource.valMessageAlreadyExist, Resource.lblTraceId) + " " + traceId, null);
                response = false;
            }
            return response;
        }

        private bool IsDuplicateMobileDropForSchedule(List<InvoiceDropViewModel> invoiceDrops, int supplierCompanyId)
        {
            bool response = true;

            var scheduleIdList = invoiceDrops.Where(t => t.TrackableScheduleId.HasValue && t.TrackableScheduleId.Value > 0).Select(t => t.TrackableScheduleId.Value).ToList();
            if (scheduleIdList.Any())
            {
                if (Context.DataContext.Invoices.Any(t => scheduleIdList.Any(t1 => t1 == t.TrackableScheduleId) && t.Order.AcceptedCompanyId == supplierCompanyId))
                {
                    LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "IsDuplicateMobileDropForSchedule",
                        string.Format(Resource.valMessageAlreadyExist, Resource.gridColumnDeliverySchedule) + " - " + string.Join(", ", scheduleIdList), null);
                    response = false;
                }
            }
            return response;
        }

        private async Task<StatusViewModel> ProcessManualInvoiceCreation(UserContext userContext, InvoiceViewModelNew createInvoiceViewModel, ConsolidatedInvoiceViewModels consolidatedModel, InvoiceNumber invoiceNumber)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (createInvoiceViewModel.Drops.All(t => t.BrokerChainId == null || t.BrokerChainId == ""))
                    {
                        await AddBrokerInvoiceCreationToQueueService(createInvoiceViewModel, consolidatedModel.OtherDetails, consolidatedModel.Invoices);
                    }
                    var delReqStatuses = await CreateConsolidatedManualInvoice(consolidatedModel.Invoices, createInvoiceViewModel, invoiceNumber, consolidatedModel.OtherDetails);
                    transaction.Commit();

                    response = await SetManualInvoiceCreatedPostEvents(userContext, consolidatedModel.Invoices, consolidatedModel.OtherDetails, createInvoiceViewModel);
                    var cumulationUpdateList = await CreateListOfCumulationEntitiesToUpdateForCreateInv(consolidatedModel.Invoices, consolidatedModel.OtherDetails);
                    if (cumulationUpdateList != null && cumulationUpdateList.Any())
                    {
                        await UpdateCumulationQuantitiesPostInvoiceCreate(cumulationUpdateList);
                    }
                    await new OrderDomain(this).UpdateBrokeredFreightOrderQuantity(createInvoiceViewModel.Drops.Select(dr =>
                        new OrderDropDetailsViewModel
                        {
                            OrderId = dr.OrderId,
                            Quantity = dr.ActualDropQuantity,
                        }).ToList());
                    response.StatusCode = Status.Success;
                    if (delReqStatuses != null && delReqStatuses.Any())
                    {
                        new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatuses);
                    }
                    var groupDrId = consolidatedModel.Invoices.Where(t => !string.IsNullOrWhiteSpace(t.GroupParentDrId)).Select(t => t.GroupParentDrId).FirstOrDefault();

                    if (!string.IsNullOrEmpty(groupDrId))
                        AddQueueMessageForDrCompletion(userContext, groupDrId);

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "ProcessManualInvoiceCreation", ex.Message, ex);
                    if (transaction != null && transaction.UnderlyingTransaction.Connection != null)
                    {
                        transaction.Rollback();
                    }
                    throw ex;
                }
            }
            return response;
        }

        public async Task<StatusViewModel> ProcessMobileInvoiceForDryRun(UserContext userContext, InvoiceViewModelNew createInvoiceViewModel, InvoiceNumber invoiceNumber)
        {
            var response = new StatusViewModel(Status.Success);
            var invoiceDomain = new InvoiceDomain(this);
            var dryRunFee = GetMaxDryRunFeeAsync(createInvoiceViewModel, userContext.Id, out int orderId);
            var dropDetails = createInvoiceViewModel.Drops.FirstOrDefault(t => t.OrderId == orderId);
            if (dropDetails != null && dryRunFee > 0)
            {
                var dryRunInvoiceViewModel = await invoiceDomain.GetDryRunInvoiceAsync(dropDetails.OrderId, userContext.Id);
                SetDryRunViewModel(dryRunInvoiceViewModel, dropDetails, userContext, invoiceNumber, createInvoiceViewModel, dryRunFee);
                dryRunInvoiceViewModel.DivertedOrderIds = createInvoiceViewModel.Drops.Where(t => t.OrderId != orderId).Select(t => t.OrderId).ToList();
                response = await invoiceDomain.CreateDryRunInvoiceAsync(dryRunInvoiceViewModel);
            }
            return response;
        }

        private void SetDryRunViewModel(DryRunInvoiceViewModel dryRunInvoiceViewModel, InvoiceDropViewModel invoiceDropViewModel, UserContext userContext, InvoiceNumber invoiceNumber, InvoiceViewModelNew createInvoiceViewModel, decimal dryRunFee)
        {
            dryRunInvoiceViewModel.DryRunDate = invoiceDropViewModel.DropDate.ToString(Resource.constFormatDate);
            dryRunInvoiceViewModel.DeliveryTime = invoiceDropViewModel.EndTime;
            dryRunInvoiceViewModel.UserId = userContext.Id;
            dryRunInvoiceViewModel.SupplierInvoiceNumber = invoiceNumber.Number;
            dryRunInvoiceViewModel.CreationMethod = CreationMethod.Mobile;
            dryRunInvoiceViewModel.TrackableScheduleId = invoiceDropViewModel.TrackableScheduleId;
            dryRunInvoiceViewModel.DryRunFee = dryRunFee;
            if (createInvoiceViewModel.Driver != null)
                dryRunInvoiceViewModel.DriverId = createInvoiceViewModel.Driver.Id;
        }

        private decimal GetMaxDryRunFeeAsync(InvoiceViewModelNew createInvoiceViewModel, int userId, out int orderId)
        {
            var response = 0.0M;
            orderId = 0;
            HelperDomain helperDomain = new HelperDomain(this);
            foreach (var drop in createInvoiceViewModel.Drops)
            {
                var order = Context.DataContext.Orders.Where(t => t.Id == drop.OrderId && t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open))
                            .Select(t => new
                            {
                                t.FuelRequest.FuelRequestFees,
                            }).FirstOrDefault();

                if (order != null)
                {
                    var dryRunFee = helperDomain.GetDryRunFee(order.FuelRequestFees, drop.DropDate);
                    if (dryRunFee > response)
                    {
                        response = dryRunFee;
                        orderId = drop.OrderId;
                    }
                }
            }
            return response;
        }

        private async Task<StatusViewModel> ProcessMobileInvoiceCreation(UserContext userContext, InvoiceViewModelNew createInvoiceViewModel, ConsolidatedInvoiceViewModels consolidatedModel, InvoiceNumber invoiceNumber)
        {
            StatusViewModel response = new StatusViewModel();
            ScheduleBuilderDomain scheduleBuilderDomain = new ScheduleBuilderDomain(this);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (createInvoiceViewModel.Drops.All(t => t.BrokerChainId == null || t.BrokerChainId == ""))
                    {
                        await AddBrokerInvoiceCreationToQueueService(createInvoiceViewModel, consolidatedModel.OtherDetails, consolidatedModel.Invoices);
                    }
                    var delReqStatuses = await CreateConsolidatedMobileInvoice(consolidatedModel.Invoices, createInvoiceViewModel, invoiceNumber, consolidatedModel.OtherDetails);
                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    
                    if (delReqStatuses != null && delReqStatuses.Any())
                    {
                        scheduleBuilderDomain.UpdateDeliveryRequestStatus(delReqStatuses);
                    }
                    if (createInvoiceViewModel.DeliveryRequestCompartments != null && createInvoiceViewModel.DeliveryRequestCompartments.Any())
                    {
                        await scheduleBuilderDomain.UpdateDeliveryRequestCompartmentInfo(createInvoiceViewModel.DeliveryRequestCompartments);
                    }
                    await ScheduleOptionalPickupFlow(createInvoiceViewModel, scheduleBuilderDomain);
                    //UPDATE BROKERED FREIGHT ORDER QUANTITY
                    await new OrderDomain(this).UpdateBrokeredFreightOrderQuantity(createInvoiceViewModel.Drops.Select(dr =>
                        new OrderDropDetailsViewModel
                        {
                            OrderId = dr.OrderId,
                            Quantity = dr.ActualDropQuantity,
                        }).ToList());

                    var groupDrId = consolidatedModel.Invoices.Where(t => !string.IsNullOrWhiteSpace(t.GroupParentDrId)).Select(t => t.GroupParentDrId).FirstOrDefault();
                    if (!string.IsNullOrEmpty(groupDrId))
                        AddQueueMessageForDrCompletion(userContext, groupDrId);
                    await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToApprover_DropCompleted(createInvoiceViewModel.Drops.First().OrderId);
                    await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.DeliveryCompleted, response.EntityId, createInvoiceViewModel.Driver.Id);
                    var messageDomain = new AppMessageDomain(this);
                    foreach (var invoiceModel in consolidatedModel.Invoices)
                    {
                        if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                        {
                            var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == invoiceModel.OrderId);
                            await messageDomain.SendInvoiceDDTMessage(order.AcceptedBy, order.Company.Name, invoiceModel);
                        }
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return response;
        }



        public void AddQueueMessageForDrCompletion(UserContext userContext, string GroupParentDrId)
        {
            try
            {
                if (!string.IsNullOrEmpty(GroupParentDrId))
                {
                    var queserviceVm = new GroupDrInvoiceCreationViewModelForQueueService()
                    {
                        UserId = userContext.Id,
                        CompanyId = userContext.CompanyId,
                        GroupParentDrId = GroupParentDrId
                    };
                    string json = JsonConvert.SerializeObject(queserviceVm);

                    var queueRequest = new QueueMessageViewModel()
                    {
                        CreatedBy = userContext.Id,
                        QueueProcessType = QueueProcessType.ConsolidationForDrCompletion,
                        JsonMessage = json
                    };

                    var queueId = new QueueMessageDomain().EnqeueMessage(queueRequest);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "AddQueueMessageForDrCompletion - GroupParentDrId : " + GroupParentDrId, ex.Message, ex);
            }
        }

        public async Task<StatusViewModel> ConfirmInvoiceDetails(UserContext userContext, int invoiceid, int orderid, int invoiceHeaderId)
        {
            StatusViewModel response = new StatusViewModel();
            //call BDN confirmation method
            response = await new ScheduleBuilderDomain(this).CancelDriverScheduleAfterBDNConfirmation(userContext, orderid, invoiceid, invoiceHeaderId);
            if (response.StatusCode == Status.Success)
            {
                AddQueueMessageForInvoiceConsolidationCompletion(userContext.Id, userContext.CompanyId, invoiceHeaderId);
                var brokeredOrderChainDetails = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId).Select(t => t.BrokeredChainId).ToList();
                if (brokeredOrderChainDetails.Any())
                {
                    var brInvDetails = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId != invoiceHeaderId && brokeredOrderChainDetails.Contains(t.BrokeredChainId) && t.IsActive
                                        && t.WaitingFor == (int)WaitingAction.InvoiceConfirmation)
                                        .Select(t => new { t.InvoiceHeaderId, t.CreatedBy, t.Order.AcceptedCompanyId }).FirstOrDefault();
                    if (brInvDetails != null)
                    {
                        AddQueueMessageForInvoiceConsolidationCompletion(brInvDetails.CreatedBy, brInvDetails.AcceptedCompanyId, brInvDetails.InvoiceHeaderId);
                    }
                }
                response.StatusMessage = Resource.sucessMsgInvoiceConfirmation;
                response.StatusCode = Status.Success;
            }
            return response;
        }

        public StatusViewModel ConvertToInvoiceWithoutTax(UserContext userContext, int invoiceid, int orderid, int invoiceHeaderId)
        {
            StatusViewModel response = new StatusViewModel();
            //call BDN confirmation method
            AddQueueMessageForInvoiceConsolidationCompletion(userContext.Id, userContext.CompanyId, invoiceHeaderId, true);
            var brokeredOrderChainDetails = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId).Select(t => t.BrokeredChainId).ToList();
            if (brokeredOrderChainDetails.Any())
            {
                var brInvDetails = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId != invoiceHeaderId && brokeredOrderChainDetails.Contains(t.BrokeredChainId) && t.IsActive
                                    && (t.WaitingFor == (int)WaitingAction.AvalaraTax || t.WaitingFor == (int)WaitingAction.PDITaxes))
                                    .Select(t => new { t.InvoiceHeaderId, t.CreatedBy, t.Order.AcceptedCompanyId }).FirstOrDefault();
                if (brInvDetails != null)
                {
                    AddQueueMessageForInvoiceConsolidationCompletion(brInvDetails.CreatedBy, brInvDetails.AcceptedCompanyId, brInvDetails.InvoiceHeaderId, true);
                }
            }
            response.StatusMessage = Resource.sucessMsgConvertDDTWithoutTax;
            response.StatusCode = Status.Success;
            return response;
        }

        public void AddQueueMessageForInvoiceConsolidationCompletion(int userId, int companyId, int invoiceHeaderId, bool isWithoutTax = false)
        {
            try
            {
                if (invoiceHeaderId > 0)
                {
                    var queserviceVm = new GroupDrInvoiceCreationViewModelForQueueService()
                    {
                        UserId = userId,
                        CompanyId = companyId,
                        InvoiceHeaderId = invoiceHeaderId,
                        IsProcessWithoutTax = isWithoutTax
                    };
                    string json = JsonConvert.SerializeObject(queserviceVm);

                    var queueRequest = new QueueMessageViewModel()
                    {
                        CreatedBy = userId,
                        QueueProcessType = QueueProcessType.ConsolidationForDrCompletion,
                        JsonMessage = json
                    };

                    var queueId = new QueueMessageDomain().EnqeueMessage(queueRequest);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "AddQueueMessageForDrCompletion - InvoiceHeaderId : " + invoiceHeaderId, ex.Message, ex);
            }
        }
        private async Task<StatusViewModel> ApproveExceptionDropTicket(InvoiceViewModelNew createInvoiceViewModel, ConsolidatedInvoiceViewModels consolidatedModel, InvoiceNumber invoiceNumber)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);

            try
            {
                if (!createInvoiceViewModel.IsBrokerInvoice)
                {
                    await AddBrokerInvoiceCreationToQueueService(createInvoiceViewModel, consolidatedModel.OtherDetails, consolidatedModel.Invoices, true);
                }
                var delReqStatuses = await UpdateConsolidatedDropTicket(consolidatedModel.Invoices, createInvoiceViewModel, invoiceNumber, consolidatedModel.OtherDetails, response);
                if (delReqStatuses != null && delReqStatuses.Any())
                {
                    new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatuses);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "ApproveExceptionDropTicket", ex.Message, ex);

                throw ex;
            }
            return response;
        }

        public async Task<StatusViewModel> SaveFuelRetainDetails(FuelRetainViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                foreach (var fuelRetain in viewModel.TrailerFuelRetain)
                {
                    DeliveryScheduleXTrackableSchedule trackableSchedule = null;
                    if (fuelRetain.TrackableScheduleId.HasValue && fuelRetain.TrackableScheduleId.Value > 0)
                    {
                        trackableSchedule = await Context.DataContext.DeliveryScheduleXTrackableSchedules.FirstOrDefaultAsync(t => t.Id == fuelRetain.TrackableScheduleId.Value);
                        if (trackableSchedule != null && !string.IsNullOrWhiteSpace(trackableSchedule.FrDeliveryRequestId))
                        {
                            fuelRetain.OrderId = trackableSchedule.OrderId;
                            fuelRetain.DeliveryRequestId = trackableSchedule.FrDeliveryRequestId;
                            var productBOLs = viewModel.BolDetails.Where(t => t.Products.Any(t2 => t2.TrackableScheduleId == trackableSchedule.Id)).ToList();
                            for (int i = 0; i < productBOLs.Count; i++)
                            {
                                var bol = productBOLs[i];
                                bol.SupplierCompanyId = !trackableSchedule.OrderId.HasValue ? trackableSchedule.User.CompanyId.Value : trackableSchedule.Order.AcceptedCompanyId;
                                bol.Carrier = trackableSchedule.Carrier?.Name;
                                for (int j = 0; j < bol.Products.Count; j++)
                                {
                                    var product = bol.Products[j];
                                    product.OrderId = trackableSchedule.OrderId;
                                    product.DeliveryScheduleId = trackableSchedule.DeliveryScheduleId;
                                }
                            }
                        }
                    }
                    fuelRetain.CreatedDate = DateTimeOffset.Now;
                }

                if (viewModel.BolDetails.Any())
                {
                    var firebaseDomain = new FirebaseDomain(this);
                    await firebaseDomain.SavePreLoadBolDetails(viewModel.BolDetails);
                }
                var scheduleBuilderDomain = new ScheduleBuilderDomain(this);
                response = await scheduleBuilderDomain.SaveTrailerFuelRetain(viewModel.TrailerFuelRetain);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "SaveFuelRetainDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SavePreLoadBolDetails(UserContext userContext, List<PreLoadBolViewModel> viewModels)
        {
            StatusViewModel response = new StatusViewModel();
            var deliveryRequestCompartments = new List<DeliveryRequestCompartmentInfoViewModel>();
            if (IsDuplicatePreLoadBol(viewModels.First().TraceId))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var viewModel in viewModels)
                        {

                            var terminalIds = viewModel.Products.Select(t => t.TerminalId).ToList();
                            var orderIds = viewModel.Products.Select(t => t.OrderId).ToList();
                            var terminals = Context.DataContext.MstExternalTerminals.Where(t => terminalIds.Contains(t.Id)).ToList();

                            var orderDetails = await Context.DataContext.Orders.Where(t => orderIds.Contains(t.Id))
                                                  .Select(t => new
                                                  {
                                                      OrderId = t.Id,
                                                      TimeZoneName = t.FuelRequest.Job.TimeZoneName,
                                                      CityGroupTerminalId = t.CityGroupTerminalId
                                                  }).ToListAsync();

                            foreach (var product in viewModel.Products)
                            {
                                var order = orderDetails.FirstOrDefault(t => t.OrderId == product.OrderId);
                                var terminal = terminals.FirstOrDefault(t => t.Id == product.TerminalId);

                                if (order != null || !product.OrderId.HasValue)
                                {
                                    var entity = new PreLoadBolDetail();
                                    entity.Id = viewModel.Id;
                                    entity.GrossQuantity = product.GrossQuantity;
                                    entity.NetQuantity = product.NetQuantity;
                                    entity.TraceId = viewModel.TraceId;
                                    entity.BolNumber = viewModel.BolNumber;
                                    entity.BadgeNumber = viewModel.BadgeNumber;
                                    entity.Carrier = viewModel.Carrier;
                                    entity.CreatedBy = viewModel.Driver.Id;
                                    entity.CreatedDate = order != null ? DateTimeOffset.Now.ToTargetDateTimeOffset(order.TimeZoneName) : DateTimeOffset.Now;
                                    entity.LiftDate = viewModel.LiftDate;
                                    entity.PickupDate = order != null ? viewModel.PickupDate.ToTargetDateTimeOffset(order.TimeZoneName) : DateTimeOffset.Now;
                                    entity.LiftTicketNumber = viewModel.LiftTicketNumber;
                                    // entity.LiftStartTime = product.LiftStartTime;
                                    // entity.LiftEndTime = product.LiftEndTime;
                                    entity.LiftStartTime = string.IsNullOrEmpty(viewModel.LiftStartTime) ? (TimeSpan?)null : Convert.ToDateTime(viewModel.LiftStartTime).TimeOfDay;
                                    entity.LiftEndTime = string.IsNullOrEmpty(viewModel.LiftEndTime) ? (TimeSpan?)null : Convert.ToDateTime(viewModel.LiftEndTime).TimeOfDay;
                                    entity.LiftQuantity = product.LiftQuantity;
                                    entity.PickupLocation = viewModel.IsBulkPlant ? PickupLocationType.BulkPlant : PickupLocationType.Terminal;
                                    entity.TerminalId = product.TerminalId.HasValue && product.TerminalId > 0 ? product.TerminalId : null;
                                    entity.CityGroupTerminalId = order != null && order.CityGroupTerminalId > 0 ? order.CityGroupTerminalId : null;
                                    entity.FuelTypeId = product.FuelTypeId;
                                    entity.IsActive = true;
                                    entity.IsDeleted = false;
                                    entity.RackPrice = 0;

                                    if (!viewModel.IsBulkPlant)
                                    {
                                        entity.PickupLocation = PickupLocationType.Terminal;
                                        if (terminal != null)
                                        {
                                            entity.TerminalId = product.TerminalId;
                                            entity.TerminalName = terminal.Name;
                                            entity.Address = terminal.Address;
                                            entity.City = terminal.City;
                                            entity.StateCode = terminal.StateCode;
                                            entity.StateId = terminal.StateId;
                                            entity.ZipCode = terminal.ZipCode;
                                            entity.CountryCode = terminal.CountryCode;
                                            entity.Latitude = terminal.Latitude;
                                            entity.Longitude = terminal.Longitude;
                                            entity.CountyName = terminal.CountyName;
                                            entity.SiteName = terminal.Name;
                                        }
                                    }
                                    else
                                    {
                                        entity.PickupLocation = PickupLocationType.BulkPlant;
                                        product.Address = GetLocationUsingGoecode(product.Address);
                                        entity.SiteName = product.BulkPlantName;

                                        entity.Address = product.Address.Address;
                                        entity.City = product.Address.City;
                                        if (product.Address.State != null)
                                        {
                                            entity.StateCode = product.Address.State.Code;
                                            entity.StateId = product.Address.State.Id;
                                        }

                                        entity.ZipCode = product.Address.ZipCode;
                                        entity.CountryCode = product.Address.Country.Code;
                                        entity.Latitude = product.Address.Latitude;
                                        entity.Longitude = product.Address.Longitude;
                                        entity.CountyName = product.Address.CountyName;

                                        if (entity.StateId.HasValue)
                                        {
                                            var countryId = Context.DataContext.MstStates.Where(t => t.Id == entity.StateId).Select(t => t.CountryId).FirstOrDefault();
                                            var bulkPlantDetail = entity.ToBulkPlantLocationEntity(countryId, viewModel.SupplierCompanyId);
                                            var dispatchDomain = new DispatchDomain(this);
                                            await dispatchDomain.SaveBulkPlantIfNotExists(bulkPlantDetail);
                                        }
                                    }

                                    entity.TrackableScheduleId = product.TrackableScheduleId;
                                    entity.DeliveryScheduleId = product.DeliveryScheduleId;

                                    if (viewModel.Images != null && (!string.IsNullOrWhiteSpace(viewModel.Images.FilePath)))
                                    {
                                        Image image = new Image() { FilePath = viewModel.Images.FilePath, Data = viewModel.Images.Data };
                                        entity.Image = image;
                                    }
                                    else if (viewModel.Images != null && viewModel.Images.Id > 0)
                                    {
                                        entity.ImageId = viewModel.Images.Id;
                                    }

                                    if (product.TrackableScheduleId.HasValue)
                                    {
                                        var preLoadCompartmentInfo = GetPreLoadCompartmentInfoDetails(viewModel);
                                        var trackableSchedule = await Context.DataContext.DeliveryScheduleXTrackableSchedules.SingleOrDefaultAsync(t => t.Id == product.TrackableScheduleId.Value);
                                        if (trackableSchedule != null)
                                        {
                                            if (viewModel.IsPreLoadBolCompleted)
                                            {
                                                trackableSchedule.DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.PreLoadBolCompleted;
                                                trackableSchedule.DeliveryScheduleType = (int)TrackableScheduleType.PickupOnly;
                                            }
                                            if (preLoadCompartmentInfo != null && preLoadCompartmentInfo.Count > 0)
                                            {
                                                trackableSchedule.CompartmentInfo = JsonConvert.SerializeObject(preLoadCompartmentInfo);
                                                deliveryRequestCompartments.Add(new DeliveryRequestCompartmentInfoViewModel
                                                {
                                                    DeliveryRequestId = trackableSchedule.FrDeliveryRequestId,
                                                    Compartments = preLoadCompartmentInfo
                                                });
                                            }

                                            Context.DataContext.Entry(trackableSchedule).State = EntityState.Modified;
                                            await Context.CommitAsync();
                                        }
                                    }
                                    Context.DataContext.PreLoadBolDetails.Add(entity);
                                }
                            }
                        }
                        await Context.CommitAsync();
                        transaction.Commit();

                        if (deliveryRequestCompartments != null && deliveryRequestCompartments.Any())
                        {
                            ScheduleBuilderDomain scheduleBuilderDomain = new ScheduleBuilderDomain(this);
                            await scheduleBuilderDomain.UpdateDeliveryRequestCompartmentInfo(deliveryRequestCompartments);
                        }

                        response.StatusCode = Status.Success;
                    }
                    catch (Exception ex)
                    {
                        LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "SavePreLoadBolDetails", ex.Message, ex);
                        transaction.Rollback();
                    }
                }
            }
            else
            {
                response.StatusCode = Status.Success;
            }
            return response;
        }
        public async Task<StatusViewModel> SavePickupBolRetainDetails(UserContext userContext, List<PreLoadBolViewModel> viewModels)
        {
            StatusViewModel response = new StatusViewModel();
            if (IsDuplicatePreLoadBol(viewModels.First().TraceId))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var viewModel in viewModels)
                        {
                            var terminalIds = viewModel.Products.Select(t => t.TerminalId).ToList();
                            var orderIds = viewModel.Products.Select(t => t.OrderId).ToList();
                            var terminals = Context.DataContext.MstExternalTerminals.Where(t => terminalIds.Contains(t.Id)).ToList();

                            var orderDetails = await Context.DataContext.Orders.Where(t => orderIds.Contains(t.Id))
                                                  .Select(t => new
                                                  {
                                                      OrderId = t.Id,
                                                      TimeZoneName = t.FuelRequest.Job.TimeZoneName,
                                                      CityGroupTerminalId = t.CityGroupTerminalId
                                                  }).ToListAsync();

                            foreach (var product in viewModel.Products)
                            {
                                var order = orderDetails.FirstOrDefault(t => t.OrderId == product.OrderId);
                                var terminal = terminals.FirstOrDefault(t => t.Id == product.TerminalId);

                                if (order != null || !product.OrderId.HasValue)
                                {
                                    var entity = new PreLoadBolDetail();
                                    entity.Id = viewModel.Id;
                                    entity.GrossQuantity = product.GrossQuantity;
                                    entity.NetQuantity = product.NetQuantity;
                                    entity.TraceId = viewModel.TraceId;
                                    entity.BolNumber = viewModel.BolNumber;
                                    entity.BadgeNumber = viewModel.BadgeNumber;
                                    entity.Carrier = viewModel.Carrier;
                                    entity.CreatedBy = viewModel.Driver.Id;
                                    entity.CreatedDate = order != null ? DateTimeOffset.Now.ToTargetDateTimeOffset(order.TimeZoneName) : DateTimeOffset.Now;
                                    entity.LiftDate = viewModel.LiftDate;
                                    entity.PickupDate = order != null ? viewModel.PickupDate.ToTargetDateTimeOffset(order.TimeZoneName) : DateTimeOffset.Now;
                                    entity.LiftTicketNumber = viewModel.LiftTicketNumber;
                                    entity.LiftStartTime = product.LiftStartTime;
                                    entity.LiftEndTime = product.LiftEndTime;
                                    entity.LiftQuantity = product.LiftQuantity;
                                    entity.PickupLocation = viewModel.IsBulkPlant ? PickupLocationType.BulkPlant : PickupLocationType.Terminal;
                                    entity.TerminalId = product.TerminalId.HasValue && product.TerminalId > 0 ? product.TerminalId : null;
                                    entity.CityGroupTerminalId = order != null && order.CityGroupTerminalId > 0 ? order.CityGroupTerminalId : null;
                                    entity.FuelTypeId = product.FuelTypeId;
                                    entity.IsActive = true;
                                    entity.IsDeleted = false;
                                    entity.RackPrice = 0;

                                    if (!viewModel.IsBulkPlant)
                                    {
                                        entity.PickupLocation = PickupLocationType.Terminal;
                                        if (terminal != null)
                                        {
                                            entity.TerminalId = product.TerminalId;
                                            entity.TerminalName = terminal.Name;
                                            entity.Address = terminal.Address;
                                            entity.City = terminal.City;
                                            entity.StateCode = terminal.StateCode;
                                            entity.StateId = terminal.StateId;
                                            entity.ZipCode = terminal.ZipCode;
                                            entity.CountryCode = terminal.CountryCode;
                                            entity.Latitude = terminal.Latitude;
                                            entity.Longitude = terminal.Longitude;
                                            entity.CountyName = terminal.CountyName;
                                            entity.SiteName = terminal.Name;
                                        }
                                    }
                                    else
                                    {
                                        entity.PickupLocation = PickupLocationType.BulkPlant;
                                        product.Address = GetLocationUsingGoecode(product.Address);
                                        entity.SiteName = product.BulkPlantName;

                                        entity.Address = product.Address.Address;
                                        entity.City = product.Address.City;
                                        if (product.Address.State != null)
                                        {
                                            entity.StateCode = product.Address.State.Code;
                                            entity.StateId = product.Address.State.Id;
                                        }

                                        entity.ZipCode = product.Address.ZipCode;
                                        entity.CountryCode = product.Address.Country.Code;
                                        entity.Latitude = product.Address.Latitude;
                                        entity.Longitude = product.Address.Longitude;
                                        entity.CountyName = product.Address.CountyName;

                                        if (entity.StateId.HasValue)
                                        {
                                            var countryId = Context.DataContext.MstStates.Where(t => t.Id == entity.StateId).Select(t => t.CountryId).FirstOrDefault();
                                            var bulkPlantDetail = entity.ToBulkPlantLocationEntity(countryId, viewModel.SupplierCompanyId);
                                            var dispatchDomain = new DispatchDomain(this);
                                            await dispatchDomain.SaveBulkPlantIfNotExists(bulkPlantDetail);
                                        }
                                    }

                                    entity.TrackableScheduleId = product.TrackableScheduleId;
                                    entity.DeliveryScheduleId = product.DeliveryScheduleId;

                                    if (viewModel.Images != null && (!string.IsNullOrWhiteSpace(viewModel.Images.FilePath)))
                                    {
                                        Image image = new Image() { FilePath = viewModel.Images.FilePath, Data = viewModel.Images.Data };
                                        entity.Image = image;
                                    }
                                    else if (viewModel.Images != null && viewModel.Images.Id > 0)
                                    {
                                        entity.ImageId = viewModel.Images.Id;
                                    }

                                    if (product.TrackableScheduleId.HasValue)
                                    {
                                        entity.IsPickupBOLRetain = viewModel.IsPickupBOLRetain;
                                        var preLoadCompartmentInfo = GetPickupBOLCompartmentInfoDetails(product);
                                        List<TrailerFuelRetainViewModel> trailerFuelRetainViews = new List<TrailerFuelRetainViewModel>();
                                        preLoadCompartmentInfo.ForEach(x =>
                                        {
                                            TrailerFuelRetainViewModel fuelRetainViewModel = new TrailerFuelRetainViewModel();
                                            fuelRetainViewModel.OrderId = order.OrderId;
                                            fuelRetainViewModel.TrailerId = x.TrailerId;
                                            fuelRetainViewModel.CompartmentId = x.CompartmentId;
                                            fuelRetainViewModel.Quantity = x.Quantity;
                                            fuelRetainViewModel.UOM = x.UOM;
                                            fuelRetainViewModel.ProductType = product.FuelType;
                                            fuelRetainViewModel.FuelType = product.FuelType;
                                            fuelRetainViewModel.ProductId = product.ProductId;
                                            fuelRetainViewModel.TrackableScheduleId = product.TrackableScheduleId;
                                            fuelRetainViewModel.TfxDriverId = viewModel.Driver.Id;
                                            trailerFuelRetainViews.Add(fuelRetainViewModel);
                                        });
                                        entity.TrailerRetainInfo = JsonConvert.SerializeObject(trailerFuelRetainViews);
                                    }
                                    Context.DataContext.PreLoadBolDetails.Add(entity);
                                }
                            }
                        }
                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                    }
                    catch (Exception ex)
                    {
                        LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "SavePickupBolRetainDetails", ex.Message, ex);
                        transaction.Rollback();
                    }
                }
            }
            else
            {
                response.StatusCode = Status.Success;
            }
            return response;
        }

        private bool IsDuplicatePreLoadBol(string traceId)
        {
            bool response = true;
            if (!string.IsNullOrEmpty(traceId) && Context.DataContext.PreLoadBolDetails.Any(t => t.TraceId == traceId))
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "IsDuplicatePreLoadBol",
                    string.Format(Resource.valMessageAlreadyExist, Resource.lblTraceId) + " " + traceId, null);
                response = false;
            }
            return response;
        }

        public List<CompartmentsInfoViewModel> GetPreLoadCompartmentInfoDetails(PreLoadBolViewModel viewModel)
        {
            var allcompartmentInfo = new List<CompartmentsInfoViewModel>();
            try
            {
                foreach (var products in viewModel.Products)
                {
                    foreach (var item in products.CompartmentInfo)
                    {
                        var compartmentInfo = new CompartmentsInfoViewModel();
                        compartmentInfo.CompartmentId = item.CompartmentId;
                        compartmentInfo.TrailerId = item.TrailerId;
                        compartmentInfo.Quantity = item.Quantity;
                        allcompartmentInfo.Add(compartmentInfo);
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "GetPreLoadCompartmentInfoDetails", ex.Message, ex);
            }
            return allcompartmentInfo;
        }
        public List<CompartmentsInfoViewModel> GetPickupBOLCompartmentInfoDetails(PreLoadProductViewModel viewModel)
        {
            var allcompartmentInfo = new List<CompartmentsInfoViewModel>();
            try
            {
                foreach (var item in viewModel.CompartmentInfo)
                {
                    var compartmentInfo = new CompartmentsInfoViewModel();
                    compartmentInfo.CompartmentId = item.CompartmentId;
                    compartmentInfo.TrailerId = item.TrailerId;
                    compartmentInfo.Quantity = item.Quantity;
                    compartmentInfo.UOM = item.UOM;
                    allcompartmentInfo.Add(compartmentInfo);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "GetPickupBOLCompartmentInfoDetails", ex.Message, ex);
            }
            return allcompartmentInfo;
        }
        public async Task<InvoiceViewModelNew> SetMobileDropInformation(InvoiceViewModelNew model, int invoiceStatusId)
        {
            var invoiceCreateDomain = new InvoiceCreateDomain(this);
            var orderIds = model.Drops.Select(t => t.OrderId).Distinct().ToList();
            var additionalDropDetails = await invoiceCreateDomain.GetMobileDropAditionalDetails(orderIds, model);

            // 1. Load mobile assets while creating invoice
            var assetDropsOrderIds = model.Drops.Where(t => !t.IsFilldInvoke).Select(t => t.OrderId).Distinct().ToList();
            var assetDrops = await GetIncompleteMobileDropsAsync(assetDropsOrderIds, model.Driver.Id);

            foreach (var item in model.Drops)
            {
                var assetDrop = assetDrops.Where(x => x.OrderId == item.OrderId).ToList();
                item.Assets.AddRange(assetDrop);
            }

            var firstDropDetail = additionalDropDetails.First();
            model.PaymentTerm = firstDropDetail.PaymentTerm;
            model.InvoiceNotes = firstDropDetail.InvoiceNotes;
            model.SupplierCompanyId = firstDropDetail.SupplierCompanyId;
            
            if (invoiceStatusId == (int)InvoiceStatus.Draft)
                model.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
            else
                model.InvoiceTypeId = firstDropDetail.InvoiceTypeId;

            foreach (var drop in model.Drops)
            {
                var additionalDetail = additionalDropDetails.FirstOrDefault(t => t.OrderId == drop.OrderId);
                drop.TypeOfFuel = additionalDetail.ProductTypeId;
                drop.PoNumber = additionalDetail.PoNumber;
                drop.TimeZoneName = additionalDetail.TimeZoneName;
                drop.JobId = additionalDetail.JobId;
                drop.IsMarineLocation = additionalDetail.IsMarineLocation;
                drop.FreightPricingMethod = additionalDetail.FreightPricingMethod;
                drop.DropDate = drop.DropDate.ToTargetDateTimeOffset(additionalDetail.TimeZoneName);
                if (drop.Assets.Any())
                {
                    foreach (var asset in drop.Assets)
                    {
                        asset.DropDate = asset.DropDate.ToTargetDateTimeOffset(additionalDetail.TimeZoneName);
                    }
                }
                drop.Allowance = additionalDetail.Allowance;
                drop.FuelSurchargeFreightFee = additionalDetail.FuelSurcharge;
            }
            var jobTimeZone = additionalDropDetails.First()?.TimeZoneName;
            if (!string.IsNullOrEmpty(jobTimeZone))
            {
                model.BolDetails.Where(t => t.LiftDate.HasValue)
                    .ToList().ForEach(t => { t.LiftDate = t.LiftDate.Value.ToTargetDateTimeOffset(jobTimeZone); });

                model.TicketDetails.Where(t => t.LiftDate.HasValue)
                .ToList().ForEach(t => { t.LiftDate = t.LiftDate.Value.ToTargetDateTimeOffset(jobTimeZone); });
            }

            var specialInstructions = additionalDropDetails.SelectMany(t => t.SpecialInstructions);
            foreach (var item in model.SpecialInstructions)
            {
                var instruction = specialInstructions.FirstOrDefault(t => t.Instruction == item.Instruction);
                if (instruction != null)
                    item.SpecialInstructionId = instruction.Id;
            }

            // 2. Load order fees and de-duplicate fees
            var fuelFees = additionalDropDetails.SelectMany(t => t.Fees).ToList();
            ProcessMobileDropFees(model, fuelFees);

            // 3. Load other product taxes
            model.OtherProductTaxes = additionalDropDetails.SelectMany(t => t.OtherTaxDetails).ToList();
            SetMobileDropPickupAndDropLocations(model, firstDropDetail.BulkPlantAddress);

            return model;
        }

        private void SetMobileDropPickupAndDropLocations(InvoiceViewModelNew model, DropAddressViewModel bulkPlantAddress)
        {
            // Set pickup location from bol/lift details
            if (model.BolDetails.Any() || model.TicketDetails.Any())
            {
                // Fetch bulk plant address from google api
                foreach (var ticket in model.TicketDetails)
                {
                    foreach (var product in ticket.Products.Where(t => t.NetQuantity > 0 && t.GrossQuantity > 0 && t.DeliveredQuantity > 0))
                    {
                        if (bulkPlantAddress != null && product.BulkPlantId == bulkPlantAddress.SiteId)
                        {
                            product.BulkPlantId = bulkPlantAddress.SiteId;
                            product.BulkPlantName = bulkPlantAddress.SiteName;

                            product.Address.SiteId = bulkPlantAddress.SiteId;
                            product.Address.SiteName = bulkPlantAddress.SiteName;
                        }
                        product.Address = GetLocationUsingGoecode(product.Address);
                    }
                }
            }
            else
            {
                // Set pickup location if bol/lift details not available
                if (bulkPlantAddress != null && model.FuelPickupLocation != null &&
                    model.FuelPickupLocation.SiteId == bulkPlantAddress.SiteId)
                {
                    model.FuelPickupLocation.SiteId = bulkPlantAddress.SiteId;
                    model.FuelPickupLocation.SiteName = bulkPlantAddress.SiteName;
                }
                if (model.FuelPickupLocation != null)
                {
                    model.FuelPickupLocation = GetLocationUsingGoecode(model.FuelPickupLocation);
                }
            }
            //Set drop location from google api
            model.FuelDropLocation = GetLocationUsingGoecode(model.FuelDropLocation);
        }

        private DropAddressViewModel GetLocationUsingGoecode(DropAddressViewModel address)
        {
            var point = GoogleApiDomain.GetAddress(Convert.ToDouble(address.Latitude), Convert.ToDouble(address.Longitude));
            if (point != null)
            {
                address.Address = point.Address;
                address.City = point.City;
                address.ZipCode = point.ZipCode;
                address.CountyName = point.CountyName;

                var state = Context.DataContext.MstStates.FirstOrDefault(t => t.Code.ToLower() == point.StateCode.ToLower());
                if (state != null)
                {
                    address.State = new StateViewModel() { Id = state == null ? 0 : state.Id, Code = state.Code };
                }

                var country = Context.DataContext.MstCountries.Single(t => t.Name.ToLower().Contains(point.CountryName.ToLower()));
                if (country != null)
                {
                    address.Country = new CountryViewModel() { Id = country == null ? 0 : country.Id, Code = country.Code };
                }
                address.IsAddressAvailable = !string.IsNullOrWhiteSpace(point.Address);
            }
            return address;
        }

        private static void ProcessMobileDropFees(InvoiceViewModelNew model, List<FeesViewModel> fuelFees)
        {
            // Set fee subtypes to demurrage fees
            var demurrageFeeTypes = new List<string>()
                {
                    ((int)FeeType.DemurrageFeeTerminal).ToString(),
                    ((int)FeeType.DemurrageFeeDestination).ToString(),
                    ((int)FeeType.DemurrageOther).ToString()
                };
            foreach (var item in model.Fees)
            {

                var demurageFee = fuelFees.FirstOrDefault(t => t.FeeTypeId == item.FeeTypeId);
                if (demurageFee != null && demurrageFeeTypes.Contains(item.FeeTypeId))
                {
                    item.FeeSubTypeId = demurageFee.FeeSubTypeId;
                    item.Fee = demurageFee.Fee;

                }
            }
            fuelFees.RemoveAll(top => demurrageFeeTypes.Contains(top.FeeTypeId));
            var deDuplicatedFees = new List<FeesViewModel>();
            foreach (var drop in model.Drops)
            {
                var fees = fuelFees.Where(t => t.OrderId == drop.OrderId).ToList();
                deDuplicatedFees = DeDuplicateFees(fees, deDuplicatedFees);
            }
            model.Fees.AddRange(deDuplicatedFees);
        }

        public async Task<StatusViewModel> CreateRebillInvoice(UserContext userContext, InvoiceViewModelNew createInvoiceViewModel)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (Context.DataContext.InvoiceHeaderDetails.Any(t => t.Id == createInvoiceViewModel.OriginalInvoiceHeaderId && t.Invoices.Any(t1 => t1.InvoiceXInvoiceStatusDetails.Any(t2 => t2.StatusId == (int)InvoiceStatus.CreditedAndRebilled))))
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageAlreadyCreditedRebilled;
                        return response;
                    }
                    List<InvoiceModel> invoices = new List<InvoiceModel>();
                    var invoiceNumber = await GenerateInvoiceNumber_New();
                    List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>();
                    List<DropAdditionalDetailsModel> otherDetails = new List<DropAdditionalDetailsModel>();
                    foreach (var drop in createInvoiceViewModel.Drops)
                    {
                        DropAdditionalDetailsModel deliveryDetails = GetDropAdditionalDetails(drop.OrderId);
                        otherDetails.Add(deliveryDetails);
                        SetOrderTerminalAsPickupLocation(createInvoiceViewModel, drop, deliveryDetails);
                        drop.ParentFuelRequestId = deliveryDetails.ParentFuelRequestId;

                        InvoiceModel invoice = await GetInvoiceModel(userContext, createInvoiceViewModel, invoices, invoiceNumber, priceRequestModels, drop, deliveryDetails);
                        if (IsPickupFromMultipleTerminals(invoices.Last(), deliveryDetails.PricingTypeId))
                        {
                            GetInvoicesForSameProduct(invoice, invoices);
                        }
                    }
                    if (invoices.Any())
                    {
                        await GetPriceDetails(priceRequestModels, invoices);
                        SetInvoiceFees(createInvoiceViewModel, invoices);
                        SetCalculatedFees(createInvoiceViewModel, invoices, true);
                        GetInvoiceStatus(invoices);
                        GetTaxes(createInvoiceViewModel, invoices);

                        var delReqStatuses = await CreateRebillInvoice(invoices, createInvoiceViewModel, invoiceNumber, otherDetails);
                        var crRblInvoice = Context.DataContext.InvoiceHeaderDetails.Where(t => t.Id == createInvoiceViewModel.OriginalInvoiceHeaderId).SelectMany(t => t.Invoices).FirstOrDefault();
                        StatusViewModel creditInvoiceResponse = AddCreditInvoiceToQueueServiceAsync(crRblInvoice.Id, invoices.Where(t => t.TrackableScheduleId != null).Select(t => t.TrackableScheduleId.Value).ToList(), userContext.Id);
                        transaction.Commit();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = "Credit & Rebill invoice request submitted successfully";
                        await SetRebillInvoiceCreatedPostEvents(userContext, invoices, otherDetails);
                        if (delReqStatuses != null && delReqStatuses.Any())
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatuses);
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "CreateRebillInvoice", ex.Message, ex);
                }
            }
            return response;
        }

        private async Task RaiseDuplicateInvoiceException(List<InvoiceModel> invoices, List<DropAdditionalDetailsModel> otherDetails, InvoiceViewModelNew createInvoiceViewModel)
        {
            if (createInvoiceViewModel.OriginalInvoiceNumberId == 0)
            {
                if (invoices.All(t => t.WaitingFor == WaitingAction.Nothing) || invoices.Any(t => t.WaitingFor == WaitingAction.UpdatedPrice))
                {
                    foreach (var invoice in invoices)
                    {
                        var additionDetails = otherDetails.FirstOrDefault(t => t.OrderId == invoice.OrderId);
                        await CheckAndSetDuplicateInvoiceException(additionDetails, invoice);
                    }
                }
            }
            else
            {
                var originalInvoiceDetails = Context.DataContext.Invoices.Where(T => T.InvoiceHeader.InvoiceNumberId == createInvoiceViewModel.OriginalInvoiceNumberId &&
                 T.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && T.IsActive)
                    .Select(t => new { t.Id, InvoiceExceptions = t.InvoiceExceptions.Where(t1 => t1.IsActive).ToList(), t.OrderId, t.DisplayInvoiceNumber }).ToList();
                if (originalInvoiceDetails.SelectMany(t => t.InvoiceExceptions).Any(t => t.ExceptionTypeId != (int)ExceptionType.MissingDeliveries))
                {
                    foreach (var invoiceModel in invoices)
                    {
                        var originalInvoice = originalInvoiceDetails.Where(t => t.OrderId == invoiceModel.OrderId).ToList();
                        invoiceModel.InvoiceExceptions = originalInvoice.SelectMany(t => t.InvoiceExceptions).Select(t => t.ToViewModel()).ToList();
                        invoiceModel.WaitingFor = WaitingAction.ExceptionApproval;
                        invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                        invoiceModel.DisplayInvoiceNumber = originalInvoice.Select(t => t.DisplayInvoiceNumber).First();
                    }
                }
            }
        }

        private async Task CheckAndSetInventoryVerificationForMissingDelivery(List<InvoiceModel> invoices, int companyId, int invoiceTypeId)
        {
            InvoiceTPDDomain invoiceTpd = new InvoiceTPDDomain();
            List<int> jobXAssetIds = new List<int>();
            foreach (var item in invoices)
                jobXAssetIds.AddRange(item.AssetDrops.Select(t => t.JobXAssetId).ToList());

            if (jobXAssetIds.Count > 0)
            {
                var assetType = Context.DataContext.JobXAssets.Where(t => jobXAssetIds.Contains(t.Id)).Select(t => t.Asset.Type).Distinct().ToList();
                if (invoices.All(t => t.WaitingFor == WaitingAction.Nothing) && assetType.Any(t1 => t1 == (int)AssetType.Tank))
                {
                    var isExceptionEnabled = await invoiceTpd.CheckExceptionEnabledByType(companyId, (int)ExceptionType.MissingDeliveries);
                    if (isExceptionEnabled)
                    {
                        if (invoiceTypeId == (int)InvoiceType.MobileApp || invoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                            invoices.ForEach(t => { t.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp; t.WaitingFor = WaitingAction.InventoryVerification; });
                        else
                            invoices.ForEach(t => { t.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual; t.WaitingFor = WaitingAction.InventoryVerification; });
                    }
                }
            }
        }

        public async Task<ConsolidatedInvoiceViewModels> GetConsolidatedInvoiceModels(UserContext userContext, InvoiceViewModelNew createInvoiceViewModel, InvoiceNumber invoiceNumber)
        {
            ConsolidatedInvoiceViewModels response = new ConsolidatedInvoiceViewModels();
            try
            {
                List<InvoiceModel> invoices = new List<InvoiceModel>();
                List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>();
                List<DropAdditionalDetailsModel> otherDetails = new List<DropAdditionalDetailsModel>();
                foreach (var drop in createInvoiceViewModel.Drops)
                {
                    DropAdditionalDetailsModel deliveryDetails = GetDropAdditionalDetails(drop.OrderId, drop.TrackableScheduleId);
                    // From mobile we are getting lat/long, so set that address instead of job address
                    if (createInvoiceViewModel.FuelDropLocation != null &&
                        (createInvoiceViewModel.InvoiceTypeId == (int)InvoiceType.MobileApp
                        || createInvoiceViewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
                    {
                        deliveryDetails.JobAddress = createInvoiceViewModel.FuelDropLocation;
                    }
                    otherDetails.Add(deliveryDetails);
                    SetOrderTerminalAsPickupLocation(createInvoiceViewModel, drop, deliveryDetails);
                    drop.ParentFuelRequestId = deliveryDetails.ParentFuelRequestId;

                    InvoiceModel invoice = await GetInvoiceModel(userContext, createInvoiceViewModel, invoices, invoiceNumber, priceRequestModels, drop, deliveryDetails);
                    if (deliveryDetails.IsMarineJob)
                    {
                        if (drop.TrackableScheduleId.HasValue && !string.IsNullOrWhiteSpace(deliveryDetails.GroupParentDrId))
                        {
                            SetWaitingForAllDrCompletion(userContext, createInvoiceViewModel, deliveryDetails.GroupParentDrId ?? createInvoiceViewModel.GroupParentDrId, invoice);
                        }
                        else if (!string.IsNullOrWhiteSpace(createInvoiceViewModel.GroupParentDrId)) // broker case handled here
                        {
                            SetWaitingForAllDrCompletion(userContext, createInvoiceViewModel, createInvoiceViewModel.GroupParentDrId, invoice);
                        }
                        else
                        {
                            if (createInvoiceViewModel.ExistingHeaderId > 0 && string.IsNullOrWhiteSpace(deliveryDetails.GroupParentDrId))
                            {
                                var existingGroupParentDr = Context.DataContext.Invoices
                                                .Where(t => t.InvoiceHeaderId == createInvoiceViewModel.ExistingHeaderId && t.GroupParentDrId != null)
                                                .Select(t => t.GroupParentDrId).FirstOrDefault();
                                if (existingGroupParentDr != null)
                                {
                                    SetWaitingForAllDrCompletion(userContext, createInvoiceViewModel, existingGroupParentDr, invoice);
                                }
                            }
                        }

                        if (invoice.WaitingFor != WaitingAction.AllDRCompletion)
                        {
                            if (deliveryDetails.IsBdnConfirmationRequired || invoices.Any(t => t.WaitingFor == WaitingAction.BDNConfirmation))
                                invoice.WaitingFor = WaitingAction.BDNConfirmation;
                            else if (deliveryDetails.IsInvoiceConfirmationRequired || invoices.Any(t => t.WaitingFor == WaitingAction.InvoiceConfirmation))
                                invoice.WaitingFor = WaitingAction.InvoiceConfirmation;

                            if (invoice.WaitingFor == WaitingAction.BDNConfirmation || invoice.WaitingFor == WaitingAction.InvoiceConfirmation)
                            {
                                if (createInvoiceViewModel.InvoiceTypeId == (int)InvoiceType.MobileApp)
                                    invoice.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                                else
                                    invoice.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                            }
                        }
                        if (invoice.WaitingFor == WaitingAction.BDNConfirmation || invoice.WaitingFor == WaitingAction.InvoiceConfirmation)
                        {
                            if (createInvoiceViewModel.InvoiceTypeId == (int)InvoiceType.MobileApp)
                                invoice.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                            else
                                invoice.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                        }
                    }
                    else if (drop.TrackableScheduleId.HasValue && !string.IsNullOrWhiteSpace(deliveryDetails.GroupParentDrId)
                                || (createInvoiceViewModel.WaitingForAction == WaitingAction.AllDRCompletion
                                    && !string.IsNullOrWhiteSpace(createInvoiceViewModel.GroupParentDrId)))
                    {
                        invoice.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                        invoice.WaitingFor = WaitingAction.AllDRCompletion;
                        invoice.GroupParentDrId = deliveryDetails.GroupParentDrId;
                        createInvoiceViewModel.GroupParentDrId = deliveryDetails.GroupParentDrId;
                        invoice.SupplierCompanyId = userContext.CompanyId;
                    }
                    else if (!string.IsNullOrWhiteSpace(createInvoiceViewModel.GroupParentDrId)) // broker case handled here
                    {
                        SetWaitingForAllDrCompletion(userContext, createInvoiceViewModel, createInvoiceViewModel.GroupParentDrId, invoice);
                    }

                    if (IsPickupFromMultipleTerminals(invoices.Last(), deliveryDetails.PricingTypeId))
                    {
                        GetInvoicesForSameProduct(invoice, invoices);
                    }
                }
                if (invoices.Any())
                {
                    // check and raise exceptions
                    await GetExceptions(createInvoiceViewModel, invoices, otherDetails, userContext);

                    if (invoices.Any(t => t.IsIncludePricingInExternalObj))
                        await GetPriceDetails(priceRequestModels, invoices, true);
                    else
                    {
                        if (invoices.All(t => t.WaitingFor == WaitingAction.Nothing))
                            await GetPriceDetails(priceRequestModels, invoices);
                    }

                    // check for duplicate invoice exception. Duplication invoice exception needs pricing info, so pricing check should be before exception check 
                    await RaiseDuplicateInvoiceException(invoices, otherDetails, createInvoiceViewModel);

                    if (createInvoiceViewModel.Drops.Any(t => t.BrokerChainId != null && t.BrokerChainId != ""))
                    {
                        var newFeesAddedByUser = createInvoiceViewModel.Fees.Where(t => t.OrderId == 0).ToList();
                        var deDuplicatedFees = new List<FeesViewModel>();

                        foreach (var drop in createInvoiceViewModel.Drops)
                        {

                            var fuelFees = createInvoiceViewModel.Fees.Where(t => t.OrderId == drop.OrderId).ToList();
                            deDuplicatedFees = DeDuplicateFees(fuelFees, deDuplicatedFees);
                        }

                        deDuplicatedFees.AddRange(newFeesAddedByUser);
                        createInvoiceViewModel.Fees = deDuplicatedFees;
                    }
                    SetInvoiceFees(createInvoiceViewModel, invoices);
                    SetCalculatedFees(createInvoiceViewModel, invoices, true);
                    GetInvoiceStatus(invoices);
                    GetTaxes(createInvoiceViewModel, invoices);

                    response.Invoices = invoices;
                    response.OtherDetails = otherDetails;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "CreateConsolidatedInvoice", ex.Message, ex);
            }
            return response;
        }

        private static void SetWaitingForAllDrCompletion(UserContext userContext, InvoiceViewModelNew createInvoiceViewModel, string groupParentDrId, InvoiceModel invoice)
        {
            invoice.WaitingFor = WaitingAction.AllDRCompletion;
            invoice.SupplierCompanyId = userContext.CompanyId;
            invoice.GroupParentDrId = groupParentDrId;
            createInvoiceViewModel.GroupParentDrId = groupParentDrId;
            if (createInvoiceViewModel.InvoiceTypeId == (int)InvoiceType.MobileApp)
                invoice.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
            else
                invoice.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
        }

        private async Task GetExceptions(InvoiceViewModelNew createInvoiceViewModel, List<InvoiceModel> invoices, List<DropAdditionalDetailsModel> otherDetails, UserContext userContext)
        {
            if (createInvoiceViewModel.OriginalInvoiceNumberId == 0)
            {
                var dqvExceptions = CheckAndSetInvoiceExceptions(otherDetails, invoices);
                if (dqvExceptions.Any())
                {
                    dqvExceptions[0].BrokeredOrders = new List<BrokeredOrdersModel>();
                    if (createInvoiceViewModel.Drops.All(t => t.BrokerChainId == null || t.BrokerChainId == ""))
                    {
                        foreach (var drop in createInvoiceViewModel.Drops)
                        {
                            createInvoiceViewModel.BrokeredOrders.Add(new BrokeredOrdersModel() { BuyerCompanyId = otherDetails[0].BuyerCompanyId, SupplierCompanyId = otherDetails[0].SupplierCompanyId, SequenceFromEndSupplier = 1, OrderId = drop.OrderId });
                            GetBrokerOrderListTillOriginalOrder(drop.OrderId, createInvoiceViewModel.BrokeredOrders);
                        }
                    }
                    if (createInvoiceViewModel.BrokeredOrders.Count > createInvoiceViewModel.Drops.Count)
                        dqvExceptions[0].BrokeredOrders.AddRange(createInvoiceViewModel.BrokeredOrders);
                    else
                        dqvExceptions[0].BrokeredOrders.Clear();
                }
                await CheckAndSetInventoryVerificationForMissingDelivery(invoices, userContext.CompanyId, createInvoiceViewModel.InvoiceTypeId);
                if (dqvExceptions.Any())
                {
                    var exceptionDomain = new ExceptionDomain(this);
                    var exceptionResult = await exceptionDomain.CheckExceptions(dqvExceptions);
                    if (exceptionResult != null && exceptionResult.Exceptions != null && exceptionResult.IsExceptionsRaised && exceptionResult.Exceptions.Any())
                    {
                        foreach (var invoiceModel in invoices)
                        {
                            var exceptions = exceptionResult.Exceptions.Where(t => t.OrderId == invoiceModel.OrderId).ToList();
                            invoiceModel.InvoiceExceptions.AddRange(exceptions.Select(t => t.ToInvoiceExceptionModel()));
                            invoiceModel.WaitingFor = WaitingAction.ExceptionApproval;
                            invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                            invoiceModel.DisplayInvoiceNumber = invoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(invoiceModel.InvoiceTypeId, invoiceModel.WaitingFor);
                        }
                    }
                }
            }
            else
            {
                var originalInvoiceDetails = Context.DataContext.Invoices.Where(T => T.InvoiceHeader.InvoiceNumberId == createInvoiceViewModel.OriginalInvoiceNumberId &&
                 T.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && T.IsActive)
                    .Select(t => new { t.Id, InvoiceExceptions = t.InvoiceExceptions.Where(t1 => t1.IsActive).ToList(), t.OrderId, t.DisplayInvoiceNumber }).ToList();
                if (originalInvoiceDetails.SelectMany(t => t.InvoiceExceptions).Any(t => t.ExceptionTypeId != (int)ExceptionType.MissingDeliveries))
                {
                    foreach (var invoiceModel in invoices)
                    {
                        var originalInvoice = originalInvoiceDetails.Where(t => t.OrderId == invoiceModel.OrderId).ToList();
                        invoiceModel.InvoiceExceptions = originalInvoice.SelectMany(t => t.InvoiceExceptions).Select(t => t.ToViewModel()).ToList();
                        invoiceModel.WaitingFor = WaitingAction.ExceptionApproval;
                        invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                        invoiceModel.DisplayInvoiceNumber = originalInvoice.Select(t => t.DisplayInvoiceNumber).First();
                    }
                }
            }
        }

        private async Task<StatusViewModel> SetManualInvoiceCreatedPostEvents(UserContext userContext, List<InvoiceModel> invoiceModels, List<DropAdditionalDetailsModel> otherDetails, InvoiceViewModelNew createInvoiceModel)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            var newsfeedDomain = new NewsfeedDomain(this);
            var messageDomain = new AppMessageDomain(this);
            var invoices = invoiceModels.GroupBy(t => t.OrderId).ToList();
            var user = Context.DataContext.Users.Where(t => t.Id == userContext.Id).Select(t => new { t.Id, t.CompanyId, t.FirstName, t.LastName, CompanyName = t.Company.Name }).FirstOrDefault();
            userContext = new UserContext() { Id = user.Id, CompanyId = user.CompanyId ?? 0, Name = $"{user.FirstName} {user.LastName}", CompanyName = user.CompanyName };
            InvoiceModel invoiceModel = new InvoiceModel();
            foreach (var invoice in invoices)
            {
                invoiceModel = invoice.FirstOrDefault();

                var additionalDetail = otherDetails.FirstOrDefault(t => t.OrderId == invoiceModel.OrderId);
                var creationMethod = createInvoiceModel.CreationMethod;
                if ((creationMethod == CreationMethod.BulkUploaded || creationMethod == CreationMethod.APIUpload)
                    && additionalDetail.PricingTypeId == (int)PricingType.Suppliercost && invoiceModel.CurrentCost.HasValue
                    && (invoiceModel.CurrentCost != invoiceModel.SupplierCost || invoiceModel.FuelCostTypeId != (int)SupplierCostTypes.SupplierCost))
                {
                    UpdateCurrentCostViewModel updateCurrentCost = new UpdateCurrentCostViewModel()
                    {
                        CountryId = additionalDetail.CountryId,
                        FuelRequestId = additionalDetail.FuelRequestId,
                        CurrencyType = additionalDetail.Currency,
                        FuelCost = invoiceModel.CurrentCost.Value,
                        FuelTypeId = additionalDetail.FuelTypeId,
                        IsGlobalCost = false,
                        OrderId = invoiceModel.OrderId.Value,
                        OriginalFuelCost = invoiceModel.SupplierCost.Value,
                        PriceRequestDetailId = additionalDetail.RequestPriceDetailId,
                        SupplierFuelCostTypeId = invoiceModel.FuelCostTypeId.Value
                    };
                    updateCurrentCost.TfxFuelTypeId = Context.DataContext.MstProducts.Where(t => t.Id == updateCurrentCost.FuelTypeId).Select(t => t.TfxProductId).FirstOrDefault() ?? 0;
                    await new CurrentCostDomain(this).UpdateSupplierCostForOrder(userContext, updateCurrentCost);
                }

                switch (invoiceModel.WaitingFor)
                {
                    case WaitingAction.UpdatedPrice:
                    case WaitingAction.AvalaraTax:
                        var newsfeedViewModel = GetDigitalDropTicketApprovalNewsfeedModel(invoiceModel, additionalDetail);
                        NewsfeedEvent newsfeedEvent = invoiceModel.WaitingFor == WaitingAction.UpdatedPrice ? NewsfeedEvent.SupplierCreatedDDTWaitingForUpdatedPrice : NewsfeedEvent.DDTCreatedWaitingForTaxes;
                        await newsfeedDomain.SetDDTWaitingForNewsfeed(userContext, newsfeedViewModel, newsfeedEvent);
                        break;

                    case WaitingAction.CustomerApproval:
                        if (additionalDetail.IsApprovalWorkflowEnabled && additionalDetail.ApprovalUserId > 0 &&
                            additionalDetail.ApprovalUserOnboardedType != (int)OnboardedType.ThirdPartyOrderOnboarded &&
                            invoiceModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual)
                        {
                            var approvalViewModel = GetApprovalWorkflowNewsfeedModel(userContext, invoiceModel, additionalDetail);
                            await newsfeedDomain.SetApprovalWorkflowEnabledNewsFeeds(approvalViewModel);
                        }
                        break;

                    case WaitingAction.PrePostDipData:
                        // set newsfeed
                        var newsfeedViewModelForPrePostDipData = GetDigitalDropTicketApprovalNewsfeedModel(invoiceModel, additionalDetail);
                        NewsfeedEvent newsfeedEventPrePostDipData = NewsfeedEvent.SupplierCreatedDDTWaitingForPrePostDipData;
                        await newsfeedDomain.SetDDTWaitingForPrePostDipDataNewsfeed(userContext, newsfeedViewModelForPrePostDipData, newsfeedEventPrePostDipData);
                        break;

                    default:
                        var newsfeedRequestModel = GetManualInvoiceCreatedNewsfeedModel(invoiceModel, additionalDetail);
                        await newsfeedDomain.SetManualInvoiceCreatedNewsfeed(userContext, newsfeedRequestModel);
                        break;
                }
                if (additionalDetail.DeliveryTypeId != (int)DeliveryType.OneTimeDelivery && additionalDetail.IsOrderAutoClosed)
                {
                    var newsfeedModel = GetSystemOrderAutoClosedNewsfeedViewModel(invoiceModel, additionalDetail);
                    await newsfeedDomain.SetSystemOrderAutoClosedNewsfeed(newsfeedModel);
                }

                if (creationMethod == CreationMethod.APIUpload)
                {
                    SetAPIUploadResponse(response, invoiceModel);
                }

                SetStatusMessage(invoiceModel.InvoiceTypeId, invoiceModel.WaitingFor, response, true, invoiceModel.DDTConversionReason);
                if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                {
                    await messageDomain.SendInvoiceDDTMessage(userContext.Id, userContext.CompanyName, invoiceModel);
                }
            }

            bool isDQVExceptionRaised = false;
            if (invoiceModels.Any(t => t.InvoiceExceptions.Any(t1 => t1.ExceptionTypeId == (int)ExceptionType.DeliveredQuantityVariance)))
            {
                NotificationDomain notificationDomain = new NotificationDomain(this);
                await notificationDomain.AddNotificationEventAsync(EventType.DeliveryQuantityVarianceExceptionRaised, invoiceModel.InvoiceHeaderId, invoiceModel.CreatedBy);
                isDQVExceptionRaised = true;
            }
            await AddNotificationEventForManualInvoice(invoiceModel.WaitingFor, invoiceModel.InvoiceHeaderId, invoiceModel.CreatedBy, isDQVExceptionRaised);
            if (createInvoiceModel.Drops != null && createInvoiceModel.Drops.Any(t => t.ExceptionId != null && t.ExceptionId > 0))
            {
                var exceptionDomain = new ExceptionDomain(this);
                var exceptionId = createInvoiceModel.Drops.Where(t => t.ExceptionId != null && t.ExceptionId > 0).Select(t1 => t1.ExceptionId).FirstOrDefault();
                await exceptionDomain.ApproveException(new List<int> { exceptionId.Value }, ExceptionResolution.CreateManualInvoice, (int)ExceptionStatus.Resolved);
            }

            return response;
        }

        private void SetAPIUploadResponse(StatusViewModel response, InvoiceModel invoiceModel)
        {
            ApiCodeMessages codeMessage = new ApiCodeMessages();

            if (IsDigitalDropTicket(invoiceModel.InvoiceTypeId))
            {
                switch (invoiceModel.WaitingFor)
                {
                    case WaitingAction.Nothing:
                        codeMessage.Code = Constants.ApiCodeRS04;
                        codeMessage.Message = Resource.successMsgRS04DddtCreated;
                        break;

                    case WaitingAction.UpdatedPrice:
                        codeMessage.Code = Constants.ApiCodeRS02;
                        codeMessage.Message = string.Format(Resource.successMsgRS02IDdtCreated, Resource.RS02UpdatedPrice);
                        break;

                    case WaitingAction.AvalaraTax:
                        codeMessage.Code = Constants.ApiCodeRS02;
                        codeMessage.Message = string.Format(Resource.successMsgRS02IDdtCreated, Resource.RS02TaxService);
                        break;

                    case WaitingAction.PrePostDipData:
                        codeMessage.Code = Constants.ApiCodeRS02;
                        codeMessage.Message = string.Format(Resource.successMsgRS02IDdtCreated, Resource.RS02PrePostDipData);
                        break;

                    case WaitingAction.CustomerApproval:
                    case WaitingAction.BolDetails:
                    case WaitingAction.Images:
                    case WaitingAction.ExceptionApproval:
                        codeMessage.Code = Constants.ApiCodeRS02;
                        codeMessage.Message = string.Format(Resource.successMsgRS02IDdtCreated, Resource.RS02RequiredInfo);
                        break;

                    default:
                        codeMessage.Code = Constants.ApiCodeRS04;
                        codeMessage.Message = Resource.successMsgRS04DddtCreated;
                        break;
                }
            }
            else
            {
                codeMessage.Code = Constants.ApiCodeRS03;
                codeMessage.Message = Resource.successMsgRS03InvoiceCreated;
            }

            codeMessage.EntityId = invoiceModel.DisplayInvoiceNumber;
            response.ResponseData = codeMessage;
        }

        private async Task SetRebillInvoiceCreatedPostEvents(UserContext userContext, List<InvoiceModel> invoiceModels, List<DropAdditionalDetailsModel> otherDetails)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            var invoices = invoiceModels.GroupBy(t => t.OrderId).ToList();
            var user = Context.DataContext.Users.Where(t => t.Id == userContext.Id).Select(t => new { t.Id, t.CompanyId, t.FirstName, t.LastName, CompanyName = t.Company.Name }).FirstOrDefault();
            userContext = new UserContext() { Id = user.Id, CompanyId = user.CompanyId ?? 0, Name = $"{user.FirstName} {user.LastName}", CompanyName = user.CompanyName };
            foreach (var invoice in invoices)
            {
                var invoiceModel = invoice.FirstOrDefault();
                var additionalDetail = otherDetails.FirstOrDefault(t => t.OrderId == invoiceModel.OrderId);
                var newsfeedRequestModel = GetManualInvoiceCreatedNewsfeedModel(invoiceModel, additionalDetail);
                await newsfeedDomain.SetRebillInvoiceCreatedNewsfeed(userContext, newsfeedRequestModel);
                if (additionalDetail.DeliveryTypeId != (int)DeliveryType.OneTimeDelivery && additionalDetail.IsOrderAutoClosed)
                {
                    var newsfeedModel = GetSystemOrderAutoClosedNewsfeedViewModel(invoiceModel, additionalDetail);
                    await newsfeedDomain.SetSystemOrderAutoClosedNewsfeed(newsfeedModel);
                }
            }

            await new NotificationDomain(this).AddNotificationEventAsync(EventType.RebilledInvoiceCreated, invoiceModels.Select(t => t.InvoiceHeaderId).FirstOrDefault(), userContext.Id);
        }

        private static SystemOrderAutoClosedNewsfeedViewModel GetSystemOrderAutoClosedNewsfeedViewModel(InvoiceModel invoice, DropAdditionalDetailsModel otherDetails)
        {
            return new SystemOrderAutoClosedNewsfeedViewModel
            {
                JobId = otherDetails.JobId,
                OrderId = otherDetails.OrderId,
                PoNumber = invoice.PoNumber,
                BuyerCompanyId = otherDetails.BuyerCompanyId,
                SupplierCompanyId = otherDetails.SupplierCompanyId,
                TimeZoneName = otherDetails.TimeZoneName,
                TotalDelivered = otherDetails.OrderTotalDelivered,
                JobCompanyId = otherDetails.JobCompanyId,
                UoM = otherDetails.UoM
            };
        }

        private async Task AddNotificationEventForManualInvoice(WaitingAction waitingFor, int invoiceId, int createdBy, bool isDQVExceptionRaised)
        {
            var notificationEvent = EventType.InvoiceCreated;
            if (waitingFor == WaitingAction.UpdatedPrice)
            {
                notificationEvent = EventType.DdtCreatedAsInvoiceIsWaitingForUpdatedPrice;
            }
            else if (waitingFor == WaitingAction.AvalaraTax)
            {
                notificationEvent = EventType.DDTCreateAsInvoiceWaitingForTaxes;
            }
            else if (waitingFor == WaitingAction.CustomerApproval)
            {
                notificationEvent = EventType.InvoiceCreatedApprovalWorkflow;
            }
            else if (waitingFor == WaitingAction.ExceptionApproval && !isDQVExceptionRaised)
            {
                notificationEvent = EventType.DdtCreatedAsInvoiceIsWaitingForExceptionApproval;
            }
            else if (waitingFor == WaitingAction.PrePostDipData)
            {
                //NEED TO WORK
                notificationEvent = EventType.DdtCreatedAsInvoiceIsWaitingForPrePostDipData;
            }

            NotificationDomain notificationDomain = new NotificationDomain(this);
            await notificationDomain.AddNotificationEventAsync(notificationEvent, invoiceId, createdBy);
        }

        private static DigitalDropTicketNewsfeedModel GetDigitalDropTicketApprovalNewsfeedModel(InvoiceModel invoiceModel, DropAdditionalDetailsModel otherDetails)
        {
            return new DigitalDropTicketNewsfeedModel
            {
                InvoiceId = invoiceModel.Id,
                InvoiceNumber = invoiceModel.DisplayInvoiceNumber,
                OrderId = otherDetails.OrderId,
                PoNumber = invoiceModel.PoNumber,
                BuyerCompanyId = otherDetails.BuyerCompanyId,
                SupplierCompanyId = otherDetails.SupplierCompanyId,
                CreatedDate = invoiceModel.CreatedDate,
                UoM = invoiceModel.UoM
            };
        }

        protected static DigitalDropTicketApprovalNewsfeedModel GetApprovalWorkflowNewsfeedModel(UserContext user, InvoiceModel invoice, DropAdditionalDetailsModel otherDetails)
        {
            return new DigitalDropTicketApprovalNewsfeedModel
            {
                InvoiceId = invoice.Id,
                InvoiceNumber = invoice.DisplayInvoiceNumber,
                OrderId = otherDetails.OrderId,
                PoNumber = invoice.PoNumber,
                SupplierCompanyId = otherDetails.SupplierCompanyId,
                TimeZoneName = otherDetails.TimeZoneName,
                IsBrokeredOrder = otherDetails.SupplierCompanyId == otherDetails.JobCompanyId,
                SupplierPreferredInvoiceTypeId = invoice.SupplierPreferredInvoiceTypeId.Value,
                CreatedBy = user.Id,
                ApprovalUserCompanyId = otherDetails.JobCompanyId,
                ApprovalUserCompany = otherDetails.JobCompanyName,
                JobId = otherDetails.JobId,
                UserName = user.Name,
                SupplierCompanyName = user.CompanyName,
                ApprovalUserName = otherDetails.ApprovalUserName,
                InvoiceHeaderId = invoice.InvoiceHeaderId
            };
        }

        protected static ManualInvoiceCreatedNewsfeedModel GetManualInvoiceCreatedNewsfeedModel(InvoiceModel invoice, DropAdditionalDetailsModel otherDetails)
        {
            return new ManualInvoiceCreatedNewsfeedModel
            {
                InvoiceId = invoice.Id,
                InvoiceNumber = invoice.DisplayInvoiceNumber,
                OrderId = otherDetails.OrderId,
                PoNumber = invoice.PoNumber,
                BuyerCompanyId = otherDetails.BuyerCompanyId,
                SupplierCompanyId = otherDetails.SupplierCompanyId,
                JobId = otherDetails.JobId,
                InvoiceTypeId = invoice.InvoiceTypeId,
                TimeZoneName = otherDetails.TimeZoneName,
                DeliveryTypeId = otherDetails.DeliveryTypeId,
                OrderCloseDate = invoice.DropEndDate,
                DropPercentage = otherDetails.DropPercentPerDelivery,
                WaitingFor = invoice.WaitingFor,
                InvoiceHeaderId = invoice.InvoiceHeaderId
            };
        }

        private void GetTaxes(InvoiceViewModelNew createInvoiceViewModel, List<InvoiceModel> invoices)
        {
            //remove otherfueltype invoices
            var otherFuelTypeInvoices = invoices.Where(t => t.TypeOfFuel == (int)ProductTypes.NonStandardFuel || t.TypeOfFuel == (int)ProductTypes.Additives).ToList();
            invoices.RemoveAll(t => t.TypeOfFuel == (int)ProductTypes.NonStandardFuel || t.TypeOfFuel == (int)ProductTypes.Additives);
            //get taxes for other fuel types
            foreach (var otherFTInvoice in otherFuelTypeInvoices)
            {
                var otherTaxes = createInvoiceViewModel.OtherProductTaxes.Where(t => t.OrderId == otherFTInvoice.OrderId || t.OrderId == null || t.OrderId == 0).ToList();
                if (otherTaxes.Any())
                {
                    otherFTInvoice.TaxDetails = GetTaxDetailsFromInputs(otherTaxes, otherFTInvoice.Currency, otherFTInvoice.Id, otherFTInvoice.BasicAmount, otherFTInvoice.DroppedGallons);
                    otherFTInvoice.TotalTaxAmount = otherFTInvoice.TaxDetails.TotalTaxAmount;
                }
            }
            //again add otherfueltype invoices to the listif (otherFuelTypeInvoices.Any())
            invoices.AddRange(otherFuelTypeInvoices);
            if (invoices.All(t => !t.IsDigitalDropTicket() && t.WaitingFor == WaitingAction.Nothing))
            {
                if (invoices.Any(t => t.IsPdieTaxRequired))
                {
                    invoices.ForEach(t => { t.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual; t.WaitingFor = WaitingAction.PDITaxes; });
                }
                else
                {
                    TaxResponseViewModel taxResponse = SetConsolidatedAvalaraTaxes(invoices);
                    if (invoices.Any(t => t.TypeOfFuel != (int)ProductTypes.NonStandardFuel && t.TypeOfFuel != (int)ProductTypes.Additives)
                        && invoices.Where(t => (t.TypeOfFuel != (int)ProductTypes.NonStandardFuel &&
                                               t.TypeOfFuel != (int)ProductTypes.Additives)).Sum(t => t.TotalTaxAmount) <= 0
                        && taxResponse.StatusCode != Status.Success)
                    {
                        invoices.ForEach(t => { t.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual; t.WaitingFor = WaitingAction.AvalaraTax; t.DDTConversionReason = taxResponse.FailedStatusCode; });
                    }
                }
            }
        }

        public async Task<InvoiceModel> GetInvoiceModel(UserContext userContext, InvoiceViewModelNew createInvoiceViewModel,
                    List<InvoiceModel> invoices, InvoiceNumber invoiceNumber, List<FuelPriceRequestModel> priceRequestModels,
                    InvoiceDropViewModel drop, DropAdditionalDetailsModel deliveryDetails)
        {
            DateTimeOffset currentDate;
            InvoiceModel invoice = new InvoiceModel();
            SetDecimalPlacesForNetGrossDropAndFees(createInvoiceViewModel, deliveryDetails.UoM);
            SetInvoiceEntityDetails(createInvoiceViewModel, drop, deliveryDetails, invoice);
            invoice.InvoiceNumberId = invoiceNumber.Id;
            SetSupplierInvoiceNumberAsDisplayInvoiceNumber(createInvoiceViewModel, invoiceNumber, invoice);
            SetBDRDetails(drop, invoiceNumber, invoice, deliveryDetails);
            invoice.CreatedBy = invoice.UpdatedBy = userContext.Id;
            invoice.SupplierCompanyId = userContext.CompanyId;
            currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(deliveryDetails.TimeZoneName);
            invoice.CreatedDate = invoice.UpdatedDate = currentDate;
            invoice.CurrentCost = drop.SupplierFuelCost;
            invoice.IsActive = true;
            await SetInvoiceAdditionalDetails(createInvoiceViewModel, drop, deliveryDetails.PaymentMethod, invoice);
            SetBolDetails(createInvoiceViewModel, drop, deliveryDetails.CityGroupTerminalId, invoice);
            SetLiftInformation(createInvoiceViewModel, drop, deliveryDetails, invoice, userContext);
            invoice.BolDetails.ForEach(t => { t.CreatedBy = userContext.Id; t.CreatedDate = currentDate; });
            SetDropLocationDetails(createInvoiceViewModel, drop.OrderId, deliveryDetails, invoice);
            SetWaitingForAction(invoice, deliveryDetails.IsApprovalWorkFlowEnabled, createInvoiceViewModel.WaitingForAction);
            SetMobileDropFreightCostForAutoFeightMethod(drop, invoice);
            invoice.TaxQuantityIndicatorTypeId = deliveryDetails.TaxQuantityIndicatorTypeId;
            if (!createInvoiceViewModel.IsRebillInvoice && (createInvoiceViewModel.InvoiceTypeId == (int)InvoiceType.MobileApp ||
                createInvoiceViewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
            {
                invoice.AssetDrops = drop.Assets.Where(t => t.Id > 0).Select(t => new AssetDropModel
                {
                    Id = t.Id,
                    OrderId = t.OrderId,
                    DropGallons = t.DropGallons,
                    DropStartDate = t.DropDate.Add(DateTime.Parse(t.StartTime).TimeOfDay),
                    //DropEndDate = t.DropDate.Add(DateTime.Parse(t.EndTime).TimeOfDay),
                    DropEndDate = t.DropEndDate != null ? t.DropEndDate.Value.Add(DateTime.Parse(t.EndTime).TimeOfDay) : t.DropDate.Add(DateTime.Parse(t.EndTime).TimeOfDay),
                    PreDip = t.PreDip,
                    PostDip = t.PostDip,
                    TankScaleMeasurement = t.TankScaleMeasurement,
                    Gravity = t.Gravity
                }).ToList();
                invoice.AssetDrops.AddRange(drop.Assets.Where(t => t.Id == 0).Select(t => t.ToAssetDropModel(currentDate.Offset)).ToList());
            }
            else
            {
                //set jobXAssetId = 0 for marine assets.Code added for bug fix 20468
                if (deliveryDetails.IsMarineJob && drop.Assets != null && drop.Assets.Any())
                {
                    drop.Assets.ForEach(t => t.JobXAssetId = 0);
                }
                drop.Assets.ForEach(t => t.DropDate = drop.DropDate);
                invoice.AssetDrops = SetAssetDropsToInvoice(deliveryDetails.JobId, deliveryDetails.JobCompanyId, userContext.Id, createInvoiceViewModel.Driver?.Id, invoice.DropEndDate, drop.Assets, deliveryDetails.IsMarineJob);
                invoice.AssetDrops.ForEach(t => t.OrderId = invoice.OrderId.Value);
            }

            //check assetdrops n prepost data
            if ((createInvoiceViewModel.CreationMethod == CreationMethod.BulkUploaded || createInvoiceViewModel.CreationMethod == CreationMethod.APIUpload || createInvoiceViewModel.CreationMethod == CreationMethod.Mobile)
                && invoice.AssetDrops.Any() && invoice.IsPrePostDipDataRequired)
            {
                if (invoice.AssetDrops.Any(t => t.PreDip == null || t.PostDip == null || (t.PreDip.HasValue && t.PreDip.Value <= 0) || (t.PostDip.HasValue && t.PostDip.Value <= 0)))
                {
                    invoice.WaitingFor = WaitingAction.PrePostDipData;
                    invoice.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoice.InvoiceTypeId);

                    //in case of consolidated invoice - it should create consolidated DDT
                    if (invoices.Any())
                        invoices.ForEach((t) => { t.WaitingFor = WaitingAction.PrePostDipData; t.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoice.InvoiceTypeId); });
                }
            }
            if (deliveryDetails.IsMarineJob)
            {
                SetConvertedQuantitiesAndGravityForMFN(invoice, deliveryDetails.CountryId);
            }

            SetDecimalPlacesForNetGrossDrop(invoice);

            priceRequestModels.AddRange(SetPriceRequestDetails(invoice.BolDetails, deliveryDetails.RequestPriceDetailId, invoice));

            // set no-data exception status
            SetNoDataExceptionStatus(invoice);
            invoices.Add(invoice);
            return invoice;
        }

        private void SetBDRDetails(InvoiceDropViewModel dropViewModel, InvoiceNumber invoiceNumber, InvoiceModel invoice, DropAdditionalDetailsModel deliveryDetails)
        {
            if (deliveryDetails.IsMarineJob && dropViewModel.BdrDetails != null)
            {
                invoice.BDRDetails.BDRNumber = dropViewModel.OrderId.ToString(); /*ApplicationConstants.TFBD + invoiceNumber.Id*/;
                invoice.BDRDetails.CloseMeterReading = dropViewModel.BdrDetails.CloseMeterReading;
                invoice.BDRDetails.DensityInVaccum = dropViewModel.BdrDetails.DensityInVaccum;
                invoice.BDRDetails.FlashPoint = dropViewModel.BdrDetails.FlashPoint;
                invoice.BDRDetails.IsEngineerInvitedToWitnessSample = dropViewModel.BdrDetails.IsEngineerInvitedToWitnessSample;
                invoice.BDRDetails.IsNoticeToProtestIssued = dropViewModel.BdrDetails.IsNoticeToProtestIssued;
                invoice.BDRDetails.MarpolSampleNumbers = dropViewModel.BdrDetails.MarpolSampleNumbers;
                invoice.BDRDetails.MVMarpolSampleNumbers = dropViewModel.BdrDetails.MVMarpolSampleNumbers;
                invoice.BDRDetails.MeasuredVolume = dropViewModel.BdrDetails.MeasuredVolume;
                invoice.BDRDetails.ObservedTemperature = dropViewModel.BdrDetails.ObservedTemperature;
                invoice.BDRDetails.OpenMeterReading = dropViewModel.BdrDetails.OpenMeterReading;
                invoice.BDRDetails.PumpingStartTime = dropViewModel.BdrDetails.PumpingStartTime;
                invoice.BDRDetails.PumpingStopTime = dropViewModel.BdrDetails.PumpingStopTime;
                invoice.BDRDetails.StandardVolume = string.IsNullOrWhiteSpace(dropViewModel.BdrDetails.StandardVolume) ? dropViewModel.BdrDetails.MeasuredVolume : dropViewModel.BdrDetails.StandardVolume;
                invoice.BDRDetails.SulphurContent = dropViewModel.BdrDetails.SulphurContent;
                invoice.BDRDetails.Viscosity = dropViewModel.BdrDetails.Viscosity;
            }
        }

        public void SetNoDataExceptionStatus(InvoiceModel invoiceModel)
        {
            if (invoiceModel.WaitingFor == WaitingAction.PrePostDipData)
            {
                invoiceModel.AdditionalDetail.NoDataExceptionApprovalId = (int)NoDataExceptionApproval.AddPreAndPostDip;
            }
            else if (invoiceModel.WaitingFor == WaitingAction.Images)
            {
                invoiceModel.AdditionalDetail.NoDataExceptionApprovalId = (int)NoDataExceptionApproval.UploadImages;
            }
            else if (invoiceModel.WaitingFor == WaitingAction.BolDetails)
            {
                invoiceModel.AdditionalDetail.NoDataExceptionApprovalId = (int)NoDataExceptionApproval.BOLDetails;
            }
        }

        private static void SetSupplierInvoiceNumberAsDisplayInvoiceNumber(InvoiceViewModelNew createInvoiceViewModel, InvoiceNumber invoiceNumber, InvoiceModel invoice)
        {
            if (string.IsNullOrWhiteSpace(createInvoiceViewModel.SupplierInvoiceNumber))
                invoice.DisplayInvoiceNumber = invoice.TransactionId = invoiceNumber.Number;
            else
            {
                invoice.DisplayInvoiceNumber = invoice.TransactionId = createInvoiceViewModel.SupplierInvoiceNumber;
                invoice.ReferenceId = invoiceNumber.Number;
            }
        }

        public static void SetOrderTerminalAsPickupLocation(InvoiceViewModelNew createInvoiceViewModel, InvoiceDropViewModel drop, DropAdditionalDetailsModel deliveryDetails)
        {
            if (!createInvoiceViewModel.BolDetails.SelectMany(t => t.Products).Any(t => t.ProductId == drop.FuelTypeId) && !createInvoiceViewModel.TicketDetails.SelectMany(t => t.Products).Any(t => t.ProductId == drop.FuelTypeId))
            {
                BolProductViewModel bolProductViewModel = new BolProductViewModel()
                {
                    ProductName = drop.FuelTypeName,
                    ProductId = drop.FuelTypeId,
                    TerminalId = drop.TerminalId ?? deliveryDetails.TerminalId,
                    TerminalName = !string.IsNullOrWhiteSpace(drop.TerminalName) ? drop.TerminalName : deliveryDetails.TerminalName
                };
                createInvoiceViewModel.BolDetails.Add(new InvoiceBolViewModel() { Products = new List<BolProductViewModel>() { bolProductViewModel } });
            }
        }

        public void GetInvoiceStatus(List<InvoiceModel> invoices)
        {
            var invoiceStatus = GetInvoiceStatusId(invoices.First().WaitingFor, (int)InvoiceStatus.Received);
            invoices.ForEach(t => t.StatusId = invoiceStatus);
            foreach (var invoice in invoices)
            {
                if (invoice.TrackableScheduleId.HasValue)
                    invoice.TrackableScheduleStatusId = GetDeliveryScheduleStatus(invoice.TrackableScheduleId, invoice.StatusId, invoice.DropEndDate);
            }
        }

        private void SetInvoiceEntityDetails(InvoiceViewModelNew createInvoiceViewModel, InvoiceDropViewModel dropViewModel, DropAdditionalDetailsModel deliveryDetails, InvoiceModel invoice)
        {
            invoice.OrderId = dropViewModel.OrderId;
            invoice.Version = 1;
            invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.Active;
            invoice.InvoiceTypeId = GetInvoiceTypeId(deliveryDetails.FuelRequestTypeId, createInvoiceViewModel.InvoiceTypeId);
            invoice.OriginalInvoiceId = dropViewModel.InvoiceId;
            invoice.DroppedGallons = dropViewModel.ActualDropQuantity;
            invoice.DropStartDate = new DateTimeOffset(dropViewModel.DropDate.Date.Add(Convert.ToDateTime(dropViewModel.StartTime).TimeOfDay)); //CHECK ENDDATE AND SET ACCORDINLGLY
            invoice.IsPdieTaxRequired = deliveryDetails.IsPdieTaxRequired;
            invoice.IsIncludePricingInExternalObj = deliveryDetails.IsIncludePricingInExternalObj;

            if (dropViewModel.DropEndDate != null && dropViewModel.DropEndDate.HasValue)
                invoice.DropEndDate = dropViewModel.DropEndDate.Value.Add(Convert.ToDateTime(dropViewModel.EndTime).TimeOfDay);
            else
                invoice.DropEndDate = new DateTimeOffset(dropViewModel.DropDate.Date.Add(Convert.ToDateTime(dropViewModel.EndTime).TimeOfDay));

            var offset = invoice.DropEndDate.GetOffset(deliveryDetails.TimeZoneName);
            invoice.DropStartDate = invoice.DropStartDate.AttachOffset(offset);
            invoice.DropEndDate = invoice.DropEndDate.AttachOffset(offset);
            invoice.IsWetHosingDelivery = createInvoiceViewModel.Fees.Any(t => t.FeeTypeId == ((int)FeeType.WetHoseFee).ToString() && t.DiscountLineItemId == null);
            invoice.IsOverWaterDelivery = createInvoiceViewModel.Fees.Any(t => t.FeeTypeId == ((int)FeeType.OverWaterFee).ToString() && t.DiscountLineItemId == null);
            invoice.PaymentTermId = (int)createInvoiceViewModel.PaymentTerm.TermId;
            invoice.NetDays = createInvoiceViewModel.PaymentTerm.NetDays;
            invoice.PaymentDueDate = GetPaymentDueDate(invoice.PaymentTermId, invoice.NetDays, deliveryDetails.TimeZoneName, invoice.DropEndDate, deliveryDetails.PaymentDueDateType);
            invoice.DriverId = createInvoiceViewModel.Driver != null && createInvoiceViewModel.Driver.Id != 0 ? createInvoiceViewModel.Driver.Id : (int?)null;
            invoice.FuelProductCode = deliveryDetails.ProductCode;
            invoice.TypeOfFuel = deliveryDetails.ProductTypeId;
            invoice.TraceId = createInvoiceViewModel.TraceId;
            invoice.IsReassignDifferentJob = dropViewModel.IsReassignDifferentJob;
            invoice.OldOrderId = dropViewModel.OldOrderId;
            if (createInvoiceViewModel.InvoiceImage != null && !string.IsNullOrWhiteSpace(createInvoiceViewModel.InvoiceImage.FilePath))
            {
                invoice.Image = createInvoiceViewModel.InvoiceImage;
            }
            invoice.SupplierPreferredInvoiceTypeId = createInvoiceViewModel.InvoiceTypeId;
            invoice.Currency = deliveryDetails.Currency;
            invoice.ExchangeRate = deliveryDetails.ExchangeRate;
            invoice.UoM = deliveryDetails.UoM;
            if (createInvoiceViewModel.SignatureImage != null && !string.IsNullOrWhiteSpace(createInvoiceViewModel.SignatureImage.FilePath))
            {
                invoice.Signature = createInvoiceViewModel.SignatureImage.ToCustomerSignature();
            }
            invoice.PoNumber = dropViewModel.PoNumber;
            invoice.IsSignatureReq = deliveryDetails.SignatureEnabled;
            invoice.IsBOLImageReq = deliveryDetails.IsBolImageRequired;
            invoice.IsPrePostDipDataRequired = deliveryDetails.IsPrePostDipDataRequired;
            invoice.IsDropImageReq = deliveryDetails.IsDropImageRequired;
            invoice.QuantityIndicatorTypeId = deliveryDetails.QuantityIndicatorTypeId;
            invoice.IsVariousFobOrigin = createInvoiceViewModel.IsVariousOrigin;
            invoice.TrackableScheduleId = dropViewModel.TrackableScheduleId;
            //DeliveryLevelPO details
            invoice.DeliveryLevelPO = dropViewModel.DeliveryLevelPO;
            invoice.BrokeredChainId = dropViewModel.BrokerChainId;
            invoice.FuelRequestTypeId = deliveryDetails.FuelRequestTypeId;
            invoice.SupplierCompanyId = createInvoiceViewModel.SupplierCompanyId;
            invoice.Gravity = dropViewModel.Gravity;
            invoice.ConvertedQuantity = dropViewModel.ConvertedQuantity;
            invoice.ConversionFactor = dropViewModel.ConversionFactor;
            invoice.IsMarineLocation = deliveryDetails.IsMarineJob;
            invoice.PricingTypeId = deliveryDetails.PricingTypeId;
            invoice.JobCountryId = deliveryDetails.CountryId;
            invoice.UserPriceToOverride = dropViewModel.UserPriceToOverride;
        }
        private int GetInvoiceTypeId(int fuelRequestTypeId, int invoiceTypeId)
        {
            var invTypeId = invoiceTypeId;
            if (fuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
            {
                if (invoiceTypeId == (int)InvoiceType.Manual)
                    invTypeId = (int)InvoiceType.DigitalDropTicketManual;
                else if (invoiceTypeId == (int)InvoiceType.MobileApp)
                    invTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
            }
            return invTypeId;
        }

        private async Task SetInvoiceAdditionalDetails(InvoiceViewModelNew createInvoiceViewModel, InvoiceDropViewModel dropViewModel, PaymentMethods paymentMethod, InvoiceModel invoice)
        {
            await SetInvoiceAdditionDetailToInvoiceModel(invoice, 0, dropViewModel.OrderId, false);
            if (dropViewModel.Allowance.HasValue)
            {
                dropViewModel.Allowance = Math.Round(dropViewModel.Allowance.Value, ApplicationConstants.InvoiceSuppplierAllowanceUnitPriceDecimalDisplay);
            }
            var droppedQty = invoice.DroppedGallons;
            if ((invoice.PricingTypeId == (int)PricingType.PricePerGallon || invoice.PricingTypeId == (int)PricingType.Suppliercost) && invoice.IsMarineLocation &&
                    invoice.ConvertedQuantity != null && (invoice.UoM == UoM.MetricTons || invoice.UoM == UoM.Barrels))
            {
                droppedQty = Math.Round(invoice.ConvertedQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
            }
            invoice.AdditionalDetail.CreationMethod = createInvoiceViewModel.CreationMethod;
            invoice.AdditionalDetail.TruckNumber = createInvoiceViewModel.TruckNumber;
            invoice.AdditionalDetail.DropTicketNumber = dropViewModel.DropTicketNumber;
            invoice.AdditionalDetail.TotalAllowance = Math.Round(droppedQty * dropViewModel.Allowance ?? 0, ApplicationConstants.InvoiceSuppplierAllowanceTotalDecimalDisplay);
            invoice.AdditionalDetail.SupplierAllowance = dropViewModel.Allowance;
            invoice.AdditionalDetail.Notes = createInvoiceViewModel.InvoiceNotes;
            invoice.AdditionalDetail.PaymentMethod = paymentMethod;
            invoice.AdditionalDetail.OriginalInvoiceHeaderId = createInvoiceViewModel.OriginalInvoiceHeaderId;
            invoice.AdditionalDetail.CarrierOrderId = dropViewModel.CarrierOrderId;
            invoice.AdditionalDetail.IsSiteOutOfFuel = createInvoiceViewModel.IsSiteOutOfFuel;
            invoice.AdditionalDetail.OutOfFuelProduct = createInvoiceViewModel.OutOfFuelProduct;
            invoice.AdditionalDetail.CarrierOrder = dropViewModel.CarrierOrder;
            invoice.AdditionalDetail.OrderDate = dropViewModel.OrderDate;
            invoice.AdditionalDetail.OrderQuantity = dropViewModel.OrderQuantity;
            invoice.AdditionalDetail.LoadingBadge = dropViewModel.LoadingBadge;
            invoice.AdditionalDetail.Tracktor = dropViewModel.Tractor;
            invoice.AdditionalDetail.TruckNumber = string.IsNullOrWhiteSpace(dropViewModel.Truck) ? createInvoiceViewModel.TruckNumber : dropViewModel.Truck;
            invoice.AdditionalDetail.ExternalRefId = createInvoiceViewModel.ExternalRefID;
            invoice.AdditionalDetail.FreightPricingMethod = dropViewModel.FreightPricingMethod;

            if (invoice.AdditionalDetail.OriginalInvoiceHeaderId != null)
            {
                invoice.AdditionalDetail.OriginalInvoiceId = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoice.AdditionalDetail.OriginalInvoiceHeaderId).Select(t => t.Id).FirstOrDefault();
            }
            if (createInvoiceViewModel.AdditionalImage != null && !string.IsNullOrWhiteSpace(createInvoiceViewModel.AdditionalImage.FilePath))
            {
                invoice.AdditionalImage = createInvoiceViewModel.AdditionalImage;
            }
            if (createInvoiceViewModel.TaxAffidavitImage != null && !string.IsNullOrWhiteSpace(createInvoiceViewModel.TaxAffidavitImage.FilePath))
            {
                invoice.TaxAffidavitImage = createInvoiceViewModel.TaxAffidavitImage;
            }

            if (createInvoiceViewModel.BDNImage != null && !string.IsNullOrWhiteSpace(createInvoiceViewModel.BDNImage.FilePath))
            {
                invoice.BDNImage = createInvoiceViewModel.BDNImage;
            }

            if (createInvoiceViewModel.CoastGuardInspectionImage != null && !string.IsNullOrWhiteSpace(createInvoiceViewModel.CoastGuardInspectionImage.FilePath))
            {
                invoice.CoastGuardInspectionImage = createInvoiceViewModel.CoastGuardInspectionImage;
            }
            if (createInvoiceViewModel.InspectionRequestVoucherImage != null && !string.IsNullOrWhiteSpace(createInvoiceViewModel.InspectionRequestVoucherImage.FilePath))
            {
                invoice.InspectionRequestVoucherImage = createInvoiceViewModel.InspectionRequestVoucherImage;
            }
            if (dropViewModel.FuelSurchargeFreightFee != null)
            {
                invoice.SurchargeFreightFeeViewModel = dropViewModel.FuelSurchargeFreightFee;
                invoice.SurchargeFreightFeeViewModel.GallonsDelivered = dropViewModel.ActualDropQuantity;
                if (dropViewModel.FuelSurchargeFreightFee.FreightPricingMethod == FreightPricingMethod.Auto)
                {
                    SetFuelSurchargeForAuto(dropViewModel, invoice);
                }
            }
            if (dropViewModel.IsAssetDropOffline)
            {
                invoice.AdditionalDetail.AssetFilled = dropViewModel.AssetCount;
            }
        }

        private async Task SetMobileDropFreightCostForAutoFeightMethod(InvoiceDropViewModel dropViewModel, InvoiceModel invoice)
        {
            HelperDomain helperDomain = new HelperDomain(this);
            try
            {
                if (invoice.AdditionalDetail.CreationMethod == CreationMethod.Mobile && dropViewModel.FuelSurchargeFreightFee.FreightPricingMethod == FreightPricingMethod.Auto && dropViewModel.FuelSurchargeFreightFee.FreightRateRuleType == FreightRateRuleType.Range &&
                        dropViewModel.FuelSurchargeFreightFee.IsFreightCostApplicable && dropViewModel.FuelSurchargeFreightFee.FreightRateRuleId.HasValue && dropViewModel.FuelSurchargeFreightFee.FreightRateRuleId.Value > 0)
                {
                    var freightCostInput = new FreightCostInputViewModel();
                    freightCostInput.FreightRateRuleId = dropViewModel.FuelSurchargeFreightFee.FreightRateRuleId.Value;
                    freightCostInput.OrderId = invoice.OrderId.Value;
                    freightCostInput.SupplierId = invoice.SupplierCompanyId;
                    freightCostInput.DeliveredQuantity = invoice.DroppedGallons;

                    decimal pickupLatitude = 0;
                    decimal pickupLongitude = 0;
                    var dropLatitude = invoice.FuelDropLocation.Latitude;
                    var dropLongitude = invoice.FuelDropLocation.Longitude;

                    var bolDetails = invoice.BolDetails.Where(t => t.PickupLocationType == PickupLocationType.Terminal).ToList();
                    var terminal = bolDetails.FirstOrDefault();

                    var liftDetails = invoice.BolDetails.Where(t => t.PickupLocationType == PickupLocationType.BulkPlant).ToList();
                    var bulkPlant = liftDetails.FirstOrDefault();
                    if (terminal != null)
                    {
                        freightCostInput.TerminalId = terminal.TerminalId.Value;

                        pickupLatitude = terminal.TerminalId != null ? terminal.Latitude : 0;
                        pickupLongitude = terminal.TerminalId != null ? terminal.Longitude : 0;
                    }
                    else if (bulkPlant != null)
                    {
                        freightCostInput.TerminalId = bulkPlant.TerminalId.Value;

                        pickupLatitude = bulkPlant.TerminalId != null ? bulkPlant.Latitude : 0;
                        pickupLongitude = bulkPlant.TerminalId != null ? bulkPlant.Longitude : 0;
                    }

                    if (pickupLatitude != 0 && pickupLongitude != 0 && dropLatitude != 0 && dropLongitude != 0)
                    {
                        freightCostInput.Distance = Convert.ToDecimal(helperDomain.CalculateDistance(pickupLatitude, pickupLongitude, dropLatitude, dropLongitude));
                        if (freightCostInput.Distance > 0)
                        {
                            dropViewModel.FuelSurchargeFreightFee.AutoFreightDistance = freightCostInput.Distance;
                            dropViewModel.FuelSurchargeFreightFee.SurchargeFreightCost = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFreightCostForInvoice(freightCostInput);
                            dropViewModel.FuelSurchargeFreightFee.TotalFuelSurchargeFee = GetFuelSurchageFrieghtFee(dropViewModel.FuelSurchargeFreightFee.SurchargePercentage, dropViewModel.FuelSurchargeFreightFee.SurchargeFreightCost, invoice.DroppedGallons);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "SetMobileDropFreightCostForAutoFeightMethod", ex.Message, ex);
            }
        }

        private void SetFuelSurchargeForAuto(InvoiceDropViewModel dropViewModel, InvoiceModel invoice)
        {
            if (dropViewModel.FuelSurchargeFreightFee.IsSurchargeApplicable && dropViewModel.FuelSurchargeFreightFee.FuelSurchargeTableId.HasValue && dropViewModel.FuelSurchargeFreightFee.FuelSurchargeTableId.Value > 0)
            {
                var fuelSurcharge = Context.DataContext.FuelSurchargeIndexes.Where(t => t.Id == dropViewModel.FuelSurchargeFreightFee.FuelSurchargeTableId).FirstOrDefault();
                if (fuelSurcharge != null)
                    invoice.SurchargeFreightFeeViewModel.SurchargePricingType = (FuelSurchagePricingType)fuelSurcharge.FuelSurchargePeriod.Value;
            }
        }

        public DropAdditionalDetailsModel GetDropAdditionalDetails(int orderId, int? trackableScheduleId = null)
        {
            var deliveryDetails = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new DropAdditionalDetailsModel()
            {
                OrderId = t.Id,
                CityGroupTerminalId = t.CityGroupTerminalId,
                TerminalId = t.TerminalId,
                TerminalName = t.TerminalId != null ? t.MstExternalTerminal.Name : null,
                Currency = t.FuelRequest.Currency,
                ExchangeRate = t.FuelRequest.ExchangeRate,
                UoM = t.FuelRequest.UoM,
                IsBolImageRequired = t.FuelRequest.FuelRequestDetail.IsBolImageRequired,
                IsDropImageRequired = t.FuelRequest.FuelRequestDetail.IsDropImageRequired,
                IsPrePostDipDataRequired = t.FuelRequest.FuelRequestDetail.IsPrePostDipRequired,
                SignatureEnabled = t.FuelRequest.Job.SignatureEnabled ? t.FuelRequest.Job.SignatureEnabled : t.SignatureEnabled,
                TimeZoneName = t.FuelRequest.Job.TimeZoneName,
                QuantityIndicatorTypeId = t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId,
                TaxQuantityIndicatorTypeId = t.FuelRequest.Job.MstState.QuantityIndicatorTypeId,
                PaymentMethod = t.FuelRequest.FuelRequestDetail.PaymentMethod,
                IsApprovalWorkFlowEnabled = t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                RequestPriceDetailId = t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                IsFtl = t.IsFTL,
                JobId = t.FuelRequest.JobId,
                IsMarineJob = t.FuelRequest.Job.IsMarine,
                BuyerCompanyId = t.BuyerCompanyId,
                SupplierCompanyId = t.AcceptedCompanyId,
                BuyerCompanyName = t.BuyerCompany.Name,
                SupplierCompanyName = t.Company.Name,
                JobName = t.FuelRequest.Job.Name,
                MaxQuantity = t.FuelRequest.MaxQuantity,
                JobCompanyId = t.FuelRequest.Job.CompanyId,
                PricingTypeId = t.FuelRequest.PricingTypeId,
                ProductCode = t.FuelRequest.MstProduct.ProductCode,
                ProductTypeId = t.FuelRequest.MstProduct.ProductTypeId,
                IsApprovalWorkflowEnabled = t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                DeliveryTypeId = t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                CountryId = t.FuelRequest.Job.CountryId,
                FuelRequestId = t.FuelRequestId,
                ParentFuelRequestId = t.FuelRequest.ParentId,
                FuelRequestTypeId = t.FuelRequest.FuelRequestTypeId,
                FuelTypeId = t.FuelRequest.FuelTypeId,
                JobCompanyName = t.FuelRequest.Job.Company.Name,
                IsBdnConfirmationRequired = t.OrderAdditionalDetail != null && t.OrderAdditionalDetail.IsManualBDNConfirmationRequired,
                IsInvoiceConfirmationRequired = t.OrderAdditionalDetail != null && t.OrderAdditionalDetail.IsManualInvoiceConfirmationRequired,
                IsPdieTaxRequired = t.OrderAdditionalDetail != null && t.OrderAdditionalDetail.IsPDITaxRequired,
                IsIncludePricingInExternalObj = t.OrderAdditionalDetail != null && t.OrderAdditionalDetail.IsIncludePricingInExternalObj,
                JobAddress = new DropAddressViewModel() { Address = t.FuelRequest.Job.Address, AddressLine2 = t.FuelRequest.Job.AddressLine2, AddressLine3 = t.FuelRequest.Job.AddressLine3, City = t.FuelRequest.Job.City, State = new StateViewModel() { Id = t.FuelRequest.Job.StateId, Code = t.FuelRequest.Job.MstState.Code }, ZipCode = t.FuelRequest.Job.ZipCode, Country = new CountryViewModel() { Code = t.FuelRequest.Job.MstCountry.Code }, Latitude = t.FuelRequest.Job.Latitude, Longitude = t.FuelRequest.Job.Longitude, CountyName = t.FuelRequest.Job.CountyName, SiteName = t.FuelRequest.Job.Name, IsAddressAvailable = true },
                GroupParentDrId = trackableScheduleId.HasValue && trackableScheduleId.Value > 0 ? t.DeliveryScheduleXTrackableSchedules.Where(t1 => t1.Id == trackableScheduleId).Select(t1 => t1.GroupParentDRId).FirstOrDefault() : null
            }).FirstOrDefault();

            var approvalUser = Context.DataContext.JobXApprovalUsers.Where(t => t.JobId == deliveryDetails.JobId && t.IsActive).FirstOrDefault();
            if (approvalUser != null)
            {
                deliveryDetails.ApprovalUserId = approvalUser.Id;
                deliveryDetails.ApprovalUserOnboardedType = approvalUser.User.OnboardedTypeId;
                deliveryDetails.ApprovalUserName = $"{approvalUser.User.FirstName} {approvalUser.User.LastName}";
            }

            if (deliveryDetails != null)
            {
                var paymentDueDatebasis = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == deliveryDetails.SupplierCompanyId
                                        && t.IsActive).Select(t => t.PaymentDueDateType).FirstOrDefault();
                if (paymentDueDatebasis != PaymentDueDateType.None)
                    deliveryDetails.PaymentDueDateType = paymentDueDatebasis;
            }
            return deliveryDetails;
        }

        private async Task AddBrokerInvoiceCreationToQueueService(InvoiceViewModelNew createInvoiceViewModel, List<DropAdditionalDetailsModel> dropDetails, List<InvoiceModel> invoices, bool isExceptionApprove = false)
        {
            if (dropDetails.Any(t => t.JobCompanyId != t.BuyerCompanyId))
            {
                var brokeredOrderInfo = new List<BrokeredOrdersModel>();
                foreach (var drop in createInvoiceViewModel.Drops)
                {
                    GetBrokerOrderListTillOriginalOrder(drop.OrderId, brokeredOrderInfo);
                }
                if (createInvoiceViewModel.Drops.All(t => t.BrokerChainId == null || t.BrokerChainId == ""))
                {
                    if (createInvoiceViewModel.ExistingHeaderId > 0)
                    {
                        var brokerChainIdForExistingDrop = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == createInvoiceViewModel.ExistingHeaderId).Select(t => t.BrokeredChainId).FirstOrDefault();
                        createInvoiceViewModel.BrokerChainId = brokerChainIdForExistingDrop;
                    }
                    GetBrokerChainId(createInvoiceViewModel, invoices);
                }
                var brokerConsolidatedOrders = brokeredOrderInfo.GroupBy(t => t.SupplierCompanyId);
                var schedules = GetScheduleDatesForBrokerChain(createInvoiceViewModel.Drops.Where(t => t.TrackableScheduleId != null).Select(t => t.TrackableScheduleId.Value).ToList());
                Dictionary<int, int> trackableSchedules = GetTrackableSchedulesForBrokers(brokeredOrderInfo.Select(t => t.OrderId).ToList(), schedules);
                foreach (var consolidatedOrder in brokerConsolidatedOrders)
                {
                    if (consolidatedOrder.Any(t => t.OrderId != 0))
                    {
                        await GetBrokerInvoiceModels(createInvoiceViewModel, consolidatedOrder, trackableSchedules, invoices, isExceptionApprove);
                    }
                }
            }
        }

        private void GetBrokerChainId(InvoiceViewModelNew createInvoiceViewModel, List<InvoiceModel> invoices)
        {
            if (createInvoiceViewModel.ExistingHeaderId > 0 && !string.IsNullOrWhiteSpace(createInvoiceViewModel.BrokerChainId))
            {
                foreach (var invoice in invoices)
                {
                    invoice.BrokeredChainId = createInvoiceViewModel.BrokerChainId;
                }
                return;
            }

            int brokerChainDifferentiator = 0;
            string brokerChainId = GetBrokeredChainId("", invoices.Select(t => t.CreatedBy).FirstOrDefault());
            createInvoiceViewModel.BrokerChainId = brokerChainId;
            foreach (var invoice in invoices)
            {
                invoice.BrokeredChainId = brokerChainId + brokerChainDifferentiator++;
            }
        }

        private async Task GetBrokerInvoiceModels(InvoiceViewModelNew createInvoiceViewModel, IGrouping<int, BrokeredOrdersModel> consolidatedOrder, Dictionary<int, int> trackableSchedules, List<InvoiceModel> invoices, bool isExceptionApprove)
        {
            var dropAdditionalDetails = GetBrokerOrderDetails(consolidatedOrder.Select(t => t.OrderId).ToList());
            var brokerInvoiceModel = new InvoiceViewModelNew();
            brokerInvoiceModel = brokerInvoiceModel.CopyObject(createInvoiceViewModel);

            brokerInvoiceModel.SupplierInvoiceNumber = string.Empty;
            brokerInvoiceModel.IsExceptionApprove = isExceptionApprove;
            brokerInvoiceModel.IsBrokerInvoice = true;
            brokerInvoiceModel.ExistingHeaderId = 0;
            brokerInvoiceModel.PaymentTerm = dropAdditionalDetails.Select(t => t.PaymentTerm).FirstOrDefault();
            brokerInvoiceModel.InvoiceTypeId = GetInvoiceTypeId(createInvoiceViewModel.CreationMethod, dropAdditionalDetails.Select(t => t.DefaultInvoiceType).FirstOrDefault());
            if (!brokerInvoiceModel.TicketDetails.Any() && brokerInvoiceModel.BolDetails.Count == 1
                        && !brokerInvoiceModel.BolDetails.Any(t => t.BolNumber != null && t.BolNumber != ""))
            {
                brokerInvoiceModel.BolDetails.Clear();
            }
            GetBrokerInvoiceDropModels(dropAdditionalDetails, brokerInvoiceModel, trackableSchedules, invoices);

            if (isExceptionApprove)
            {
                var brokerChainIds = invoices.Select(t => t.BrokeredChainId).ToList();
                var brokerOrderIds = brokerInvoiceModel.Drops.Select(t => t.OrderId).ToList();
                var activeInvoice = Context.DataContext.Invoices.Where(t => brokerChainIds.Contains(t.BrokeredChainId) && brokerOrderIds.Contains(t.OrderId ?? 0) && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t => new { t.InvoiceHeaderId, t.InvoiceHeader.InvoiceNumberId }).FirstOrDefault();

                if (activeInvoice != null)
                {
                    brokerInvoiceModel.OriginalInvoiceHeaderId = activeInvoice.InvoiceHeaderId;
                    brokerInvoiceModel.OriginalInvoiceNumberId = activeInvoice.InvoiceNumberId;
                }
            }

            if (createInvoiceViewModel.CreationMethod == CreationMethod.APIUpload || createInvoiceViewModel.CreationMethod == CreationMethod.BulkUploaded)
            {
                if (dropAdditionalDetails.First() != null && dropAdditionalDetails.First().OrderEnforcement != OrderEnforcement.EnforceOrderLevelValues)
                {
                    brokerInvoiceModel.Fees.ForEach(t => t.OrderId = dropAdditionalDetails.First().OrderId);
                    brokerInvoiceModel.Fees.RemoveAll(t => t.FeeTypeId == ((int)FeeType.ProcessingFee).ToString() || t.FeeTypeId == ((int)FeeType.SurchargeFreightFee).ToString() || t.FeeTypeId == ((int)FeeType.FreightCost).ToString());
                    var ccAndSurchargeFee = dropAdditionalDetails.SelectMany(f => f.Fees.Where(t => t.FeeTypeId == ((int)FeeType.ProcessingFee).ToString() || t.FeeTypeId == ((int)FeeType.SurchargeFreightFee).ToString())).ToList();
                    brokerInvoiceModel.Fees.AddRange(ccAndSurchargeFee);
                }
                else
                {
                    brokerInvoiceModel.Fees = dropAdditionalDetails.SelectMany(t => t.Fees).ToList();
                }
            }
            else
            {
                brokerInvoiceModel.Fees = dropAdditionalDetails.SelectMany(t => t.Fees).ToList();
            }

            UpdateWaitingTimeInFeeDetails(brokerInvoiceModel.Fees, createInvoiceViewModel.Fees);
            UserContext user = new UserContext() { CompanyId = dropAdditionalDetails.Select(t => t.SupplierCompanyId).FirstOrDefault(), Id = dropAdditionalDetails.Select(t => t.AcceptedBy).FirstOrDefault() };
            await AddCreateInvioceToQueue(user, brokerInvoiceModel);
        }

        private static int GetInvoiceTypeId(CreationMethod creationMethod, int defaultInvoiceType)
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

        private static void GetBrokerInvoiceDropModels(List<BrokerOrderDropModel> dropAdditionalDetails, InvoiceViewModelNew brokerInvoiceModel, Dictionary<int, int> trackableSchedules, List<InvoiceModel> invoices)
        {
            brokerInvoiceModel.OtherProductTaxes.Clear();
            var dropsToRemove = new List<InvoiceDropViewModel>();
            foreach (var drop in brokerInvoiceModel.Drops)
            {
                drop.BrokerChainId = invoices.Where(t => t.OrderId == drop.OrderId).Select(t => t.BrokeredChainId).FirstOrDefault();
                var dropAdditionalDetail = dropAdditionalDetails.FirstOrDefault(t => t.FuelTypeId == drop.FuelTypeId || t.FuelRequestId == drop.ParentFuelRequestId);
                if (dropAdditionalDetail != null)
                {
                    drop.OrderId = dropAdditionalDetail.OrderId;
                    drop.PoNumber = dropAdditionalDetail.PoNumber;
                    drop.TerminalId = dropAdditionalDetail.TerminalId;
                    drop.TerminalName = dropAdditionalDetail.TerminalName;
                    drop.Allowance = dropAdditionalDetail.Allowance;
                    drop.TypeOfFuel = dropAdditionalDetail.ProductTypeId;
                    drop.FuelTypeId = dropAdditionalDetail.FuelTypeId;
                    if (drop.TypeOfFuel == (int)ProductTypes.NonStandardFuel && dropAdditionalDetail.OtherTaxes != null && dropAdditionalDetail.OtherTaxes.Any())
                    {
                        brokerInvoiceModel.OtherProductTaxes.AddRange(dropAdditionalDetail.OtherTaxes);
                    }
                    if (drop.TrackableScheduleId.HasValue && drop.TrackableScheduleId.Value > 0)
                    {
                        drop.TrackableScheduleId = trackableSchedules.Where(t => t.Key == drop.OrderId).Select(t => t.Value).FirstOrDefault();
                    }
                    GetBrokerOrderSurchargeFee(drop, dropAdditionalDetail);

                    //in case of pricing source change in Broker, FuelTypeId has to match with BOL ProductId
                    if (brokerInvoiceModel.BolDetails.Any(t => t.Products.Any(p => p.ProductName.Equals(drop.FuelTypeName))))
                    {
                        brokerInvoiceModel.BolDetails.ForEach(t => t.Products.ForEach((p) =>
                              {
                                  if (p.ProductName.Equals(drop.FuelTypeName))
                                  {
                                      p.ProductId = dropAdditionalDetail.FuelTypeId;
                                  }
                              })
                            );
                    }

                    if (drop.ConversionFactor != null && drop.ConversionFactor.HasValue && drop.ConversionFactor.Value > 0)
                    {
                        drop.ConvertedQuantity = null;
                    }
                }
                else
                {
                    dropsToRemove.Add(drop);
                }
            }
            foreach (var item in dropsToRemove)
            {
                brokerInvoiceModel.Drops.Remove(item);
            }
        }

        private static void GetBrokerOrderSurchargeFee(InvoiceDropViewModel drop, BrokerOrderDropModel dropAdditionalDetail)
        {
            if (dropAdditionalDetail.IsFuelSurcharge && dropAdditionalDetail.FuelSurchargePricingType.HasValue)
            {
                var eaiPrice = new FuelSurchargeFreightFeeViewModel();
                var surchargeFee = dropAdditionalDetail.Fees.FirstOrDefault(t => t.FeeTypeId.Equals(((int)FeeType.SurchargeFreightFee)));
                if (surchargeFee != null)
                {
                    eaiPrice.IsSurchargeApplicable = dropAdditionalDetail.IsFuelSurcharge;
                    var fscproductType = dropAdditionalDetail.ProductTypeId.GetFuelSurchargeProductType();
                    eaiPrice.SurchargeProductType = fscproductType;
                    eaiPrice.SurchargeFreightCost = surchargeFee.Fee ?? 0;
                    eaiPrice.SurchargePricingType = (FuelSurchagePricingType)dropAdditionalDetail.FuelSurchargePricingType.Value;
                    eaiPrice.SurchargeEiaPrice = new EIAPriceUpdateDomain().GetEIAPrice((FuelSurchagePricingType)dropAdditionalDetail.FuelSurchargePricingType.Value, fscproductType, drop.DropDate.Date);
                    eaiPrice.GallonsDelivered = drop.ActualDropQuantity;

                    if (surchargeFee.FeeSubTypeId == (int)FeeSubType.ByDistance)
                    {
                        var distanceFromOriginalDrop = drop.FuelSurchargeFreightFee.Distance;
                        eaiPrice.IsFeeByDistance = true;
                        eaiPrice.Distance = distanceFromOriginalDrop;

                        var byQantity = surchargeFee.DeliveryFeeByQuantity.FirstOrDefault(t => distanceFromOriginalDrop >= t.MinQuantity && distanceFromOriginalDrop <= (t.MaxQuantity ?? distanceFromOriginalDrop));
                        if (byQantity != null)
                            eaiPrice.SurchargeFreightCost = byQantity.Fee;
                    }

                    var surchargeTableRecord = new FuelSurchargeDomain().GetFuelSurchargeRecordForEaiPrice(eaiPrice.SurchargeEiaPrice, dropAdditionalDetail.SupplierCompanyId, dropAdditionalDetail.BuyerCompanyId, drop.DropDate.Date, fscproductType);
                    if (surchargeTableRecord != null)
                    {
                        eaiPrice.SurchargePercentage = surchargeTableRecord.FuelSurchargeStartPercentage;
                        eaiPrice.SurchargeTableRangeStart = surchargeTableRecord.PriceRangeStartValue;
                        eaiPrice.SurchargeTableRangeEnd = surchargeTableRecord.PriceRangeEndValue;
                        eaiPrice.TotalFuelSurchargeFee = GetFuelSurchageFrieghtFee(surchargeTableRecord.FuelSurchargeStartPercentage, eaiPrice.SurchargeFreightCost, drop.ActualDropQuantity);
                    }
                    drop.FuelSurchargeFreightFee = eaiPrice;
                }
                else
                {
                    drop.FuelSurchargeFreightFee = null;
                }
            }
            else
            {
                drop.FuelSurchargeFreightFee = null;
            }
        }

        private List<BrokerOrderDropModel> GetBrokerOrderDetails(List<int> orderIds)
        {
            List<BrokerOrderDropModel> brokerOrders = new List<BrokerOrderDropModel>();
            var orders = Context.DataContext.Orders
                        .Where(t => orderIds.Contains(t.Id))
                        .Select(t => new
                        {
                            t.AcceptedBy,
                            ActiveOrderVersion = t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive),
                            Terminal = t.MstExternalTerminal,
                            t.Id,
                            AdditionalDetail = t.OrderAdditionalDetail,
                            fees = t.FuelRequest.FuelRequestFees,
                            t.DefaultInvoiceType,
                            t.FuelRequest.FuelTypeId,
                            t.FuelRequest.MstProduct.ProductTypeId,
                            t.AcceptedCompanyId,
                            t.BuyerCompanyId,
                            t.FuelRequest.FuelRequestDetail.OrderEnforcementId,
                            t.FuelRequest.FuelRequestTypeId,
                            OrderTaxDetails = t.OrderTaxDetails.Where(t1 => t1.IsActive).ToList(),
                            t.FuelRequestId
                        }).ToList();
            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    BrokerOrderDropModel brokerOrder = new BrokerOrderDropModel();
                    brokerOrder.OrderId = order.Id;
                    brokerOrder.DefaultInvoiceType = order.DefaultInvoiceType;
                    brokerOrder.ProductTypeId = order.ProductTypeId;
                    brokerOrder.SupplierCompanyId = order.AcceptedCompanyId;
                    brokerOrder.BuyerCompanyId = order.BuyerCompanyId;
                    brokerOrder.OrderEnforcement = order.OrderEnforcementId;
                    brokerOrder.AcceptedBy = order.AcceptedBy;
                    brokerOrder.FuelTypeId = order.FuelTypeId;
                    brokerOrder.FuelRequestId = order.FuelRequestId;
                    if (order.fees.Any())
                    {
                        brokerOrder.Fees = order.fees.ToFeesViewModel();
                        brokerOrder.Fees.ForEach(t => t.OrderId = order.Id); // Reuired for de-duplication logic
                    }
                    if (order.ActiveOrderVersion != null)
                    {
                        brokerOrder.PoNumber = order.ActiveOrderVersion.PoNumber;
                        brokerOrder.PaymentTerm = new PaymentTermViewModel() { NetDays = order.ActiveOrderVersion.NetDays, TermId = (PaymentTerms)order.ActiveOrderVersion.PaymentTermId };
                    }
                    if (order.Terminal != null)
                    {
                        brokerOrder.TerminalId = order.Terminal.Id;
                        brokerOrder.TerminalName = order.Terminal.Name;
                    }
                    if (order.AdditionalDetail != null)
                    {
                        brokerOrder.Allowance = order.AdditionalDetail.Allowance;
                        brokerOrder.IsFuelSurcharge = order.AdditionalDetail.IsFuelSurcharge;
                        brokerOrder.FuelSurchargePricingType = order.AdditionalDetail.FuelSurchagePricingType;
                    }
                    if (order.OrderTaxDetails.Any())
                    {
                        brokerOrder.OtherTaxes = order.OrderTaxDetails.ToTaxViewModel();
                    }

                    brokerOrders.Add(brokerOrder);
                }
            }
            return brokerOrders;
        }

        private void SetDropLocationDetails(InvoiceViewModelNew createInvoiceViewModel, int orderId, DropAdditionalDetailsModel deliveryDetails, InvoiceModel invoice)
        {
            DispatchLocationViewModel dispatchLocation;

            if (createInvoiceViewModel.IsVariousOrigin && createInvoiceViewModel.FuelDropLocation != null)
            {
                dispatchLocation = createInvoiceViewModel.FuelDropLocation.ToDropLocation();
            }
            else
            {
                dispatchLocation = deliveryDetails.JobAddress.ToDropLocation();
            }
            dispatchLocation.OrderId = orderId;
            dispatchLocation.CreatedBy = invoice.CreatedBy;
            dispatchLocation.CreatedDate = invoice.CreatedDate;
            invoice.FuelDropLocation = dispatchLocation;
        }

        private void SetWaitingForAction(InvoiceModel invoice, bool isApprovalWorkflowEnabled, WaitingAction defaultAction)
        {
            //if (creationMethod == CreationMethod.BulkUploaded || creationMethod == CreationMethod.APIUpload)
            if (defaultAction == WaitingAction.FilldResponse)
            {
                invoice.WaitingFor = WaitingAction.FilldResponse;
                invoice.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoice.InvoiceTypeId);
            }
            else
            {
                invoice.WaitingFor = IsWaitingForImage(invoice) ? WaitingAction.Images : WaitingAction.Nothing;

                if (invoice.WaitingFor == WaitingAction.Nothing && isApprovalWorkflowEnabled && invoice.StatusId != (int)InvoiceStatus.Draft)
                {
                    invoice.WaitingFor = WaitingAction.CustomerApproval;
                    invoice.StatusId = (int)InvoiceStatus.WaitingForApproval;
                }
                if (invoice.WaitingFor != WaitingAction.Nothing || invoice.StatusId == (int)InvoiceStatus.Draft)
                {
                    invoice.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoice.InvoiceTypeId);
                }
            }
        }

        public void SetInvoiceFees(InvoiceViewModelNew consolidateViewModel, List<InvoiceModel> invoices)
        {
            consolidateViewModel.Fees.RemoveAll(t => !string.IsNullOrWhiteSpace(t.FeeTypeId) && t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
            consolidateViewModel.Fees.Where(t => t.FeeTypeId == ((int)FeeType.ProcessingFee).ToString()).ToList().ForEach(t => t.OrderId = 0);
            //if true , round of the fee value to 6 decimals 
            bool IsMarineLocation = invoices.FirstOrDefault().IsMarineLocation;
            var invoiceFees = consolidateViewModel.Fees.Where(t => t.OrderId == 0).ToList().ToInvoiceFees(invoices.OrderByDescending(t => t.DropEndDate).Select(t => t.DropEndDate).FirstOrDefault(), IsMarineLocation);
            var fuelTypes = invoices.GroupBy(t => t.OrderId).ToList();
            foreach (var fuelType in fuelTypes)
            {
                var invoice = fuelType.FirstOrDefault();
                var drop = consolidateViewModel.Drops.FirstOrDefault(t => t.OrderId == fuelType.Key);
                invoice.FuelFees = consolidateViewModel.Fees.Where(t => t.OrderId == fuelType.Key).ToList().ToInvoiceFees(invoice.DropEndDate, IsMarineLocation);
                FuelFeeViewModel surchargeFee = GetSurchargeFee(drop);
                if (surchargeFee != null)
                {
                    invoice.FuelFees.Add(surchargeFee);
                }

                var freightCost = GetFreightCost(drop);
                if (freightCost != null)
                {
                    invoice.FuelFees.Add(freightCost);
                }

                invoice.FuelFees.ForEach(t => { t.Currency = invoice.Currency; t.UoM = invoice.UoM; });
                invoice.FuelFees.SelectMany(t => t.FeeByQuantities).ToList().ForEach(t =>
                {
                    t.Currency = invoice.Currency;
                    t.UoM = invoice.UoM;
                });
            }

            //to make sure Fee details present for other fee type
            foreach (var invFee in invoiceFees)
            {
                if (invFee.FeeTypeId == (int)FeeType.OtherFee && string.IsNullOrEmpty(invFee.FeeDetails) && invFee.OtherFeeTypeId.HasValue && invFee.OtherFeeTypeId.Value > 0)
                {
                    var otherFeeName = Context.DataContext.MstOtherFeeTypes.Where(t => t.Id == invFee.OtherFeeTypeId.Value).Select(t => t.Name).FirstOrDefault();
                    if (!string.IsNullOrEmpty(otherFeeName))
                        invFee.FeeDetails = otherFeeName;
                }
            }

            invoices.FirstOrDefault().FuelFees.AddRange(invoiceFees);
            if (consolidateViewModel.AccessorialFeeDetails != null && consolidateViewModel.AccessorialFeeDetails.Any())
                invoices.FirstOrDefault().AccessorialFeeDetails.AddRange(consolidateViewModel.AccessorialFeeDetails);
            //FOR FREIGHT ONLY INVOICE - ONLY DELIVERY FEE WILL BE INCLUDED
            if (invoices.Any(t => t.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest))
            {
                invoices.FirstOrDefault().FuelFees.RemoveAll(t => t.FeeTypeId != (int)FeeType.DeliveryFee);
            }
        }

        public void SetCalculatedFees(InvoiceViewModelNew consolidatedViewModel, List<InvoiceModel> invoices, bool isFeeTypeBasedCalculationRequired)
        {
            decimal? tierPricingInvQty = null;
            if (invoices.Any(t => t.BolDetails.Any(b => b.TierPricingForBol != null && b.TierPricingForBol.Any())))
                tierPricingInvQty = invoices.Sum(t => t.DroppedGallons);
            // if true. set fee amount to 6 decimal places 
            var IsMarineLocation = invoices.FirstOrDefault().IsMarineLocation;
            foreach (var invoice in invoices)
            {
                invoice.FuelFees.ForEach(feeModel => feeModel.IsMarineLocation = IsMarineLocation);
                if (isFeeTypeBasedCalculationRequired)
                {
                    FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
                    fuelFeesDomain.CalculateAndSetTotalFeeAndQuantityToFuelFees(consolidatedViewModel, invoice, invoice.AssetDrops.Count, tierPricingInvQty);
                }
                else                // negate for credit partial invoice
                {
                    invoice.FuelFees.ForEach(t => { t.TotalFee = -1 * t.Fee; t.FeeSubTypeId = (int)FeeSubType.FlatFee; t.FeeSubQuantity = -1; });
                }

                invoice.TotalFeeAmount = invoice.FuelFees.Where(t => t.DiscountLineItemId == null && !t.IncludeInPPG).Sum(t => t.TotalFee ?? 0);
                invoice.TotalDiscountAmount = invoice.FuelFees.Where(t => t.DiscountLineItemId != null).Sum(t => t.TotalFee ?? 0);
                if (invoice.FuelFees.Any(t => t.IncludeInPPG))
                {
                    invoice.BasicAmount += invoice.FuelFees.Where(t => t.IncludeInPPG).Select(t => t.TotalFee ?? 0).Sum();
                    invoice.PricePerGallon = invoice.BasicAmount / invoice.DroppedGallons;

                    // update ppg in bol list
                    invoice.BolDetails.ForEach(t => t.PricePerGallon = invoice.PricePerGallon);
                }
            }
        }

        private static FuelFeeViewModel GetSurchargeFee(InvoiceDropViewModel invoiceDrop)
        {
            FuelFeeViewModel entity = null;

            if (invoiceDrop.FuelSurchargeFreightFee != null && invoiceDrop.FuelSurchargeFreightFee.IsSurchargeApplicable)
            {
                entity = new FuelFeeViewModel();
                entity.FeeTypeId = invoiceDrop.FuelSurchargeFreightFee.FeeTypeId;
                entity.Fee = invoiceDrop.IsMarineLocation ? Math.Round(invoiceDrop.FuelSurchargeFreightFee.SurchargeFreightCost, ApplicationConstants.MarineInvoiceFeeUnitPriceDecimalDisplay) : invoiceDrop.FuelSurchargeFreightFee.SurchargeFreightCost;
                entity.FeeSubTypeId = invoiceDrop.FuelSurchargeFreightFee.FeeSubTypeId;
                entity.Currency = invoiceDrop.FuelSurchargeFreightFee.Currency;
                var totalFuelSurchargeFee = GetFuelSurchageFrieghtFee(invoiceDrop.FuelSurchargeFreightFee.SurchargePercentage, invoiceDrop.FuelSurchargeFreightFee.SurchargeFreightCost, invoiceDrop.FuelSurchargeFreightFee.GallonsDelivered);
                entity.TotalFee = invoiceDrop.IsMarineLocation ? Math.Round(totalFuelSurchargeFee, ApplicationConstants.MarineInvoiceFeeUnitPriceDecimalDisplay) : Math.Round(totalFuelSurchargeFee, ApplicationConstants.InvoiceFuelSurchargeDecimalDisplay);
                entity.FeeSubQuantity = invoiceDrop.FuelSurchargeFreightFee.GallonsDelivered * invoiceDrop.FuelSurchargeFreightFee.SurchargePercentage;
                entity.WaiveOffTime = invoiceDrop.FuelSurchargeFreightFee.Distance;
            }
            return entity;
        }

        private static FuelFeeViewModel GetFreightCost(InvoiceDropViewModel invoiceDrop)
        {
            FuelFeeViewModel entity = null;

            if (invoiceDrop.FuelSurchargeFreightFee != null && invoiceDrop.FuelSurchargeFreightFee.IsFreightCostApplicable)
            {
                entity = new FuelFeeViewModel();
                entity.FeeTypeId = (int)FeeType.FreightCost;
                entity.Fee = invoiceDrop.IsMarineLocation ? Math.Round(invoiceDrop.FuelSurchargeFreightFee.SurchargeFreightCost, ApplicationConstants.MarineInvoiceFeeUnitPriceDecimalDisplay) : invoiceDrop.FuelSurchargeFreightFee.SurchargeFreightCost;
                entity.FeeSubTypeId = (int)FeeSubType.FlatFee;
                entity.Currency = invoiceDrop.FuelSurchargeFreightFee.Currency;
                var totalFreightCost = invoiceDrop.FuelSurchargeFreightFee.GallonsDelivered * invoiceDrop.FuelSurchargeFreightFee.SurchargeFreightCost;
                entity.TotalFee = invoiceDrop.IsMarineLocation ? Math.Round(totalFreightCost, ApplicationConstants.MarineInvoiceFeeUnitPriceDecimalDisplay) : Math.Round(totalFreightCost, ApplicationConstants.InvoiceFuelSurchargeDecimalDisplay);
                entity.FeeSubQuantity = invoiceDrop.FuelSurchargeFreightFee.GallonsDelivered * invoiceDrop.FuelSurchargeFreightFee.SurchargeFreightCost;
                //entity.WaiveOffTime = invoiceDrop.FuelSurchargeFreightFee.Distance;
            }
            return entity;
        }

        private async Task<List<AssetDropViewModel>> GetIncompleteMobileDropsAsync(List<int> orderIds, int driverId)
        {
            var drops = new List<AssetDropViewModel>();
            try
            {
                var assetDrops = await Context.DataContext.AssetDrops.Where(t => orderIds.Contains(t.OrderId)
                            && t.DroppedBy == driverId && t.InvoiceId == null)
                            .Select(t => new
                            {
                                t.Id,
                                t.OrderId,
                                t.DroppedGallons,
                                t.Gravity,
                                t.DropStartDate,
                                t.DropEndDate
                            }).ToListAsync();

                drops = assetDrops.Select(t => new AssetDropViewModel
                {
                    Id = t.Id,
                    OrderId = t.OrderId,
                    DropDate = t.DropStartDate.Date,
                    Gravity = t.Gravity,
                    StartTime = t.DropStartDate.ToString(Resource.constFormat12HourTime2),
                    EndTime = t.DropEndDate.ToString(Resource.constFormat12HourTime2)
                }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "GetIncompleteMobileDropsAsync", ex.Message, ex);
            }
            return drops;
        }

        public static List<FeesViewModel> DeDuplicateFees(List<FeesViewModel> existingFees, List<FeesViewModel> newFees)
        {
            var _combined = new List<FeesViewModel>();
            if (existingFees == null && newFees == null)
            {
                return _combined;
            }
            if (existingFees == null)
            {
                return newFees;
            }
            if (newFees == null)
            {
                return existingFees;
            }
            //1. Get higher from existing
            var f1 = GetHigherFromFirst(existingFees, newFees);
            _combined.AddRange(f1);

            //2. Get unmatched from existing
            var f2 = GetUnmatchedFromFirst(existingFees, newFees);
            _combined.AddRange(f2);

            //3. Take higher from new fees
            var f3 = GetHigherFromFirst(newFees, _combined);
            _combined.AddRange(f3);

            //4. Take unmatched from new fees
            var f4 = GetUnmatchedFromFirst(newFees, _combined);
            _combined.AddRange(f4);

            return _combined;
        }

        private static bool IsMatched(FeesViewModel x, FeesViewModel y)
        {
            return y.FeeTypeId == x.FeeTypeId
                && y.FeeSubTypeId == x.FeeSubTypeId
                && y.FeeConstraintTypeId == x.FeeConstraintTypeId
                && x.OtherFeeTypeId == y.OtherFeeTypeId;
        }

        private static List<FeesViewModel> GetHigherFromFirst(List<FeesViewModel> array1, List<FeesViewModel> array2)
        {
            var _highers = new List<FeesViewModel>();
            //1. Flat Fee: Remove if duplicate or take higher fee
            //2. Per Hour: Remove if duplicate or take higher fee
            //3. Percent: Always take higher fee
            //4. Per Asset: Don't allow duplicate, take higher fee
            //5. Per Gallon: Apply product-wise : No need to check for duplicates
            array1.ForEach(first =>
            {
                var _higher = array2.FirstOrDefault(second =>

                     IsMatched(first, second) && (
                        (IsFlatFee(first, second) && first.Fee > second.Fee) ||
                        (IsPerHourFee(first, second)/* && first.Fee > second.Fee*/) ||
                        (IsPercentFee(first, second) && first.Fee > second.Fee) ||
                        (IsPerAssetFee(first, second)/* && first.Fee > second.Fee*/) ||
                        IsPerGallonFee(first, second)));

                if (_higher != null)
                {
                    _highers.Add(first);
                }
            });
            return _highers;
        }

        private static List<FeesViewModel> GetUnmatchedFromFirst(List<FeesViewModel> array1, List<FeesViewModel> array2)
        {
            var _unmatched = new List<FeesViewModel>();
            array1.ForEach(first =>
            {
                var _unmatch = array2.FirstOrDefault(second => IsMatched(first, second));
                if (_unmatch == null)
                {
                    _unmatched.Add(first);
                }
            });
            return _unmatched;
        }

        private static bool IsFlatFee(FeesViewModel x, FeesViewModel y)
        {
            return x.FeeSubTypeId == (int)FeeSubType.FlatFee && y.FeeSubTypeId == (int)FeeSubType.FlatFee && x.OtherFeeTypeId == y.OtherFeeTypeId;
        }

        private static bool IsPerAssetFee(FeesViewModel x, FeesViewModel y)
        {
            return x.FeeSubTypeId == (int)FeeSubType.ByAssetCount && y.FeeSubTypeId == (int)FeeSubType.ByAssetCount;
        }

        private static bool IsPerHourFee(FeesViewModel x, FeesViewModel y)
        {
            return x.FeeSubTypeId == (int)FeeSubType.HourlyRate && y.FeeSubTypeId == (int)FeeSubType.HourlyRate && x.OtherFeeTypeId == y.OtherFeeTypeId;
        }

        private static bool IsPerGallonFee(FeesViewModel x, FeesViewModel y)
        {
            return x.FeeSubTypeId == (int)FeeSubType.PerGallon && y.FeeSubTypeId == (int)FeeSubType.PerGallon && x.OtherFeeTypeId == y.OtherFeeTypeId;
        }

        private static bool IsPercentFee(FeesViewModel x, FeesViewModel y)
        {
            return x.FeeSubTypeId == (int)FeeSubType.Percent && y.FeeSubTypeId == (int)FeeSubType.Percent;
        }

        public async Task<InvoiceModel> GetUnassignedDdtInvoiceModel(UserContext apiUserContext, InvoiceDropViewModel drop, InvoiceViewModelNew invoiceViewModel, TPDInvoiceViewModel apiRequestModel, InvoiceNumber invoiceNumber)
        {
            InvoiceModel invoice = new InvoiceModel();
            invoice.Version = 1;
            invoice.StatusId = (int)InvoiceStatus.Unassigned;
            invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.Active;
            invoice.CreationMethod = CreationMethod.APIUpload;
            invoice.InvoiceTypeId = invoiceViewModel.InvoiceTypeId;
            invoice.PaymentTermId = (int)PaymentTerms.DueOnReceipt;
            invoice.IsActive = true;
            invoice.InvoiceNumberId = invoiceNumber.Id;
            invoice.WaitingFor = WaitingAction.ExceptionApproval;

            invoice = drop.ToApiInvoiceDropViewModel(apiRequestModel, invoice);

            // setting driver id to user context id, as getting ddt pdf not showing. getting entity access error 
            if (invoice.DriverId == null || invoice.DriverId < 0)
                invoice.DriverId = apiUserContext.Id;

            if (string.IsNullOrWhiteSpace(apiRequestModel.SupplierInvoiceNumber))
                invoice.DisplayInvoiceNumber = invoice.TransactionId = invoiceNumber.Number;
            else
            {
                invoice.DisplayInvoiceNumber = invoice.TransactionId = apiRequestModel.SupplierInvoiceNumber;
                invoice.ReferenceId = invoiceNumber.Number;
            }

            DateTimeOffset currentDate = DateTimeOffset.Now;
            invoice.CreatedDate = invoice.UpdatedDate = currentDate;
            invoice.CreatedBy = invoice.UpdatedBy = apiUserContext.Id;

            //// set drop location for NULL, as InvoiceDispatchLocation table has OrderId NOT NULL.
            invoice.FuelDropLocation = null;

            // set bol and lift details
            SetBolDetails(invoiceViewModel, drop, null, invoice);
            invoice.BolDetails.ForEach(t => { t.CreatedBy = apiUserContext.Id; t.CreatedDate = currentDate; });

            // set lift details
            SetLiftInformation(invoiceViewModel, drop.FuelTypeId, new DropAdditionalDetailsModel(), invoice);

            // Generate invoice api exception
            await CheckAndSetInvoiceApiException(true, invoice, drop, apiRequestModel, invoiceViewModel, apiUserContext);

            return invoice;
        }

        public async Task<StatusViewModel> UpdateCumulationQuantitiesPostInvoiceCreate(List<CumulationQuantityUpdateViewModel> updateQuantitesInfo)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                response = await new PricingServiceDomain(this).UpdateCumulationQuantitiesPostInvoiceCreate(updateQuantitesInfo);
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "UpdateCumulationQuantitiesPostInvoiceCreate", ex.Message, ex);
            }
            return response;
        }


        public async Task<List<CumulationQuantityUpdateViewModel>> CreateListOfCumulationEntitiesToUpdateForCreateInv(List<InvoiceModel> invoiceModels, List<DropAdditionalDetailsModel> dropAdditionalDetails)
        {
            var responseList = new List<CumulationQuantityUpdateViewModel>();
            try
            {

                if (invoiceModels != null && invoiceModels.Any())
                {
                    var tempList = new List<CumulationQuantityUpdateViewModel>();
                    foreach (var invoice in invoiceModels)
                    {
                        if (invoice.InvoiceTypeId != ((int)InvoiceType.DigitalDropTicketManual)
                            && invoice.InvoiceTypeId != ((int)InvoiceType.DigitalDropTicketMobileApp) && invoice.BolDetails.Any(t => t.TierPricingForBol.Any()))
                        {

                            // NEED TO DO GROUP BY REQUESTPRICEDETAISLSID AND SUM DROPPED GALLONS 
                            var additionalDetail = dropAdditionalDetails.FirstOrDefault(t => t.OrderId == invoice.OrderId);

                            var item = new CumulationQuantityUpdateViewModel();
                            item.DroppedGallons = invoice.DroppedGallons;
                            item.RequestPriceDetailsId = additionalDetail.RequestPriceDetailId;
                            tempList.Add(item);

                        }

                    }
                    if (tempList != null && tempList.Any())
                    {
                        var grpResult = tempList.GroupBy(t => t.RequestPriceDetailsId).ToList();
                        foreach (var item in grpResult)
                        {
                            var requestPriceDetailsId = item.Key;
                            var TotalDroppedGallons = tempList.Where(t => t.RequestPriceDetailsId == requestPriceDetailsId).Select(t => t.DroppedGallons).Sum();
                            var cumlationQty = new CumulationQuantityUpdateViewModel();
                            cumlationQty.DroppedGallons = TotalDroppedGallons;
                            cumlationQty.RequestPriceDetailsId = requestPriceDetailsId;
                            responseList.Add(cumlationQty);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "CreateListOfCumulationEntitiesToUpdateForCreateInv", ex.Message, ex);
            }
            return responseList;
        }

        private async Task ScheduleOptionalPickupFlow(InvoiceViewModelNew createInvoiceViewModel, ScheduleBuilderDomain scheduleBuilderDomain)
        {
            var optionalPickUpDropModel = createInvoiceViewModel.Drops.Where(x => x.OptionalPickIds != null && x.OptionalPickIds.Any() && x.IsOptionalPickup).ToList();
            if (optionalPickUpDropModel.Any())
            {
                List<ScheduleOptionalPickupModel> scheduleOptionalPickupModel = new List<ScheduleOptionalPickupModel>();
                optionalPickUpDropModel.ForEach(x =>
                {
                    var scheduleOptionalPickupItem = scheduleOptionalPickupModel.Find(x1 => x1.TrackableScheduleId == x.TrackableScheduleId.GetValueOrDefault());
                    if (scheduleOptionalPickupItem == null)
                    {
                        ScheduleOptionalPickupModel scheduleOptionalPickup = new ScheduleOptionalPickupModel();
                        scheduleOptionalPickup.TrackableScheduleId = x.TrackableScheduleId.GetValueOrDefault();
                        scheduleOptionalPickup.OptionalPickupIds = x.OptionalPickIds;
                    }
                    else
                    {
                        scheduleOptionalPickupItem.OptionalPickupIds.AddRange(x.OptionalPickIds);
                    }
                });
                //update records in DeliveryXTrackable Schedule Table & Delivery Request mongo collection.
                await UpdateScheduleOptionalPickup(scheduleBuilderDomain, scheduleOptionalPickupModel);
            }
        }

        private async Task UpdateScheduleOptionalPickup(ScheduleBuilderDomain scheduleBuilderDomain, List<ScheduleOptionalPickupModel> scheduleOptionalPickupModel)
        {
            var updateDRStatus = await scheduleBuilderDomain.UpdateDeliveryRequestOptionalPickupInfo(scheduleOptionalPickupModel);
            if (updateDRStatus.StatusCode != (int)Status.Success)
            {
                LogManager.Logger.WriteError("ConsolidatedInvoiceDomain-ScheduleOptionalPickupFlow", "UpdateDRScheduleOptionalPickupInfo", "Error in update optional pickup info in Delivery Request collection.");
            }
        }

        public async Task<StatusViewModel> UpdateImagesToMarineInvoice(UpdateImagesViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);

            var invoiceToUpdate = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == viewModel.InvoiceHeaderId && t.IsActive
                                    && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                    .FirstOrDefaultAsync();

            if (invoiceToUpdate != null)
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in viewModel.ImagesModels)
                        {
                            //if drop image
                            if (item.ImageType == InvoiceImageType.Drop)
                                invoiceToUpdate.Image = item.ImageViewModel.ToEntity();

                            //if signature image
                            if (item.ImageType == InvoiceImageType.Signature)
                            {
                                var signVm = new CustomerSignatureViewModel() { Image = item.ImageViewModel, Name = item.ImageFile.FileName, SignatoryAvailable = true };
                                invoiceToUpdate.Signaure = signVm.ToEntity();
                            }

                            if (invoiceToUpdate.InvoiceXAdditionalDetail != null)
                            {
                                //if taxaffadavit
                                if (item.ImageType == InvoiceImageType.TaxAffidavit)
                                    invoiceToUpdate.InvoiceXAdditionalDetail.TaxAffidavitImage = item.ImageViewModel.ToEntity();

                                //if BDN Image
                                if (item.ImageType == InvoiceImageType.BDNImage)
                                    invoiceToUpdate.InvoiceXAdditionalDetail.BDNImage = item.ImageViewModel.ToEntity();

                                //if cg
                                if (item.ImageType == InvoiceImageType.CGInspection)
                                    invoiceToUpdate.InvoiceXAdditionalDetail.CoastGuardInspectionImage = item.ImageViewModel.ToEntity();

                                //request inspection
                                if (item.ImageType == InvoiceImageType.RequestInspectionVoucher)
                                    invoiceToUpdate.InvoiceXAdditionalDetail.InspectionRequestVoucherImage = item.ImageViewModel.ToEntity();

                                //if additional image
                                if (item.ImageType == InvoiceImageType.AdditionalImage)
                                    invoiceToUpdate.InvoiceXAdditionalDetail.AdditionalImage = item.ImageViewModel.ToEntity();
                            }

                            if (invoiceToUpdate.InvoiceXBolDetails.Any() && item.InvoiceFtlDetailsId > 0)
                            {
                                var bolToUpdateWithImage = await Context.DataContext.InvoiceFtlDetails.FirstOrDefaultAsync(t => t.Id == item.InvoiceFtlDetailsId);
                                if (bolToUpdateWithImage != null)
                                {
                                    bolToUpdateWithImage.Image = item.ImageViewModel.ToEntity();
                                }
                            }
                        }

                        Context.Commit();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageImagesUpload;
                    }
                    catch (Exception ex)
                    {
                        if (transaction != null && transaction.UnderlyingTransaction.Connection != null)
                        {
                            transaction.Rollback();
                        }
                        LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "UpdateImagesToMarineInvoice", ex.Message, ex);
                    }
                }
            }
            else
                response.StatusMessage = Resource.ErrNoActiveInvoiceFoundToUpdateImage;

            return response;
        }

        public UpdateImagesViewModel ValidateUploadImagesModel(UploadImageModel viewModel)
        {
            var response = new UpdateImagesViewModel(Status.Failed);
            var isValid = true;

            if (viewModel == null || viewModel.InvoiceHeaderId <= 0)
            {
                response.StatusMessage = Resource.errMessageInvalidInvoiceNo;
                isValid = false;
            }
            else
            {
                response.InvoiceHeaderId = viewModel.InvoiceHeaderId;
            }

            if (viewModel!=null && viewModel.AdditionalImage != null)
            {
                response.ImagesModels.Add(new UpdateImagesModel() { ImageFile = viewModel.AdditionalImage, ImageType = InvoiceImageType.AdditionalImage });
            }

            if (viewModel != null && viewModel.BolImages != null && viewModel.BolImages.Any())
            {
                foreach (var bolImage in viewModel.BolImages)
                {
                    var bolArr = bolImage.FileName.Split('|');
                    if (bolArr != null && bolArr.Length == 2)
                    {
                        var invoiceFtlDetailsId = Convert.ToInt32(bolArr[0]);
                        
                        response.ImagesModels.Add(new UpdateImagesModel() { ImageFile = bolImage, ImageType = InvoiceImageType.Bol, InvoiceFtlDetailsId = invoiceFtlDetailsId });
                    }
                    else
                    {
                        response.StatusMessage = "Invalid BOL details";
                        isValid = false;
                        break;
                    }
                }
            }

            if (viewModel != null && viewModel.CGInspectionImage != null)
            {
                response.ImagesModels.Add(new UpdateImagesModel() { ImageFile = viewModel.CGInspectionImage, ImageType = InvoiceImageType.CGInspection });
            }

            if (viewModel != null && viewModel.DropImage != null)
            {
                response.ImagesModels.Add(new UpdateImagesModel() { ImageFile = viewModel.DropImage, ImageType = InvoiceImageType.Drop });
            }

            if (viewModel != null && viewModel.InspectionVoucherImage != null)
            {
                response.ImagesModels.Add(new UpdateImagesModel() { ImageFile = viewModel.InspectionVoucherImage, ImageType = InvoiceImageType.RequestInspectionVoucher });
            }

            if (viewModel != null && viewModel.SignatureImage != null)
            {
                response.ImagesModels.Add(new UpdateImagesModel() { ImageFile = viewModel.SignatureImage, ImageType = InvoiceImageType.Signature });
            }

            if (viewModel != null && viewModel.TaxAffidavitImage != null)
            {
                response.ImagesModels.Add(new UpdateImagesModel() { ImageFile = viewModel.TaxAffidavitImage, ImageType = InvoiceImageType.TaxAffidavit });
            }

            if (viewModel != null && viewModel.BDNImage != null)
            {
                response.ImagesModels.Add(new UpdateImagesModel() { ImageFile = viewModel.BDNImage, ImageType = InvoiceImageType.BDNImage });
            }

            if (!response.ImagesModels.Any())
            {
                response.StatusMessage = Resource.btnLabelSelectAtleastOneFile;
                isValid = false;
            }

            if (isValid)
            {
                response.StatusCode = Status.Success;
                response.StatusMessage = Status.Success.ToString();
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateInvoices(List<InvoiceBolEditGrid> postedInvoiceBolEdits, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                if (postedInvoiceBolEdits != null && postedInvoiceBolEdits.Any())
                {
                    var invoiceHeaderid = postedInvoiceBolEdits.FirstOrDefault().InvoiceHeaderId;
                    var spDomain = new StoredProcedureDomain(this);
                    var originalValues = await spDomain.GetMarineInvoiceBolListAsync(userContext.CompanyId, invoiceHeaderid, 0);

                    //filter out non edited records.
                    foreach (var originalItem in originalValues)
                    {
                        var postedValue = postedInvoiceBolEdits.Where(t => t.InvoiceId == originalItem.InvoiceId && t.InvoiceFtlDetailId == originalItem.InvoiceFtlDetailId).FirstOrDefault();
                        if (postedValue != null)
                        {
                            //check if all values are same
                            if (originalItem.ApiGravity == postedValue.ApiGravity && originalItem.DroppedQty == postedValue.DroppedQty
                                    && originalItem.FlashPoint == postedValue.FlashPoint && originalItem.GrossQty == postedValue.GrossQty
                                    && originalItem.NetQty == postedValue.NetQty && originalItem.SulfurContent == postedValue.SulfurContent
                                    && originalItem.Temperature == postedValue.Temperature && originalItem.Viscosity == postedValue.Viscosity
                                    && originalItem.Density == postedValue.Density
                                    && originalItem.BolOrLiftNumber == postedValue.BolOrLiftNumber && originalItem.DeliveryLevelPO == postedValue.DeliveryLevelPO)
                            {
                                postedInvoiceBolEdits.Remove(postedValue);
                            }
                        }
                    }

                    if (postedInvoiceBolEdits.Any())
                    {
                        bool isCreateNewVersion = false;
                        //if only bdr or whole invoice to update
                        foreach (var item in postedInvoiceBolEdits)
                        {
                            var originalRecord = originalValues.Where(t => t.InvoiceId == item.InvoiceId && t.InvoiceFtlDetailId == item.InvoiceFtlDetailId).FirstOrDefault();
                            if (originalRecord != null)
                            {
                                if (item.ApiGravity != originalRecord.ApiGravity || item.NetQty != originalRecord.NetQty
                                    || item.GrossQty != originalRecord.GrossQty || item.DroppedQty != originalRecord.DroppedQty)
                                {
                                    isCreateNewVersion = true;
                                    break;
                                }
                            }
                        }

                        //update bdn of records
                        foreach (var item in postedInvoiceBolEdits)
                        {
                            var bdnToModify = Context.DataContext.BDRDetails.Where(t => t.InvoiceId == item.InvoiceId).FirstOrDefault();
                            if (bdnToModify != null)
                            {
                                bdnToModify.FlashPoint = item.FlashPoint;
                                bdnToModify.DensityInVaccum = item.Density;
                                bdnToModify.ObservedTemperature = item.Temperature;
                                bdnToModify.SulphurContent = item.SulfurContent;
                                bdnToModify.Viscosity = item.Viscosity;
                                Context.Commit();
                            }

                            var invFtl = Context.DataContext.InvoiceFtlDetails.Where(t => t.Id == item.InvoiceFtlDetailId).FirstOrDefault();
                            if (invFtl != null)
                            {
                                if (!string.IsNullOrWhiteSpace(invFtl.BolNumber))
                                    invFtl.BolNumber = item.BolOrLiftNumber;
                                else if (!string.IsNullOrWhiteSpace(invFtl.LiftTicketNumber))
                                    invFtl.LiftTicketNumber = item.BolOrLiftNumber;
                                invFtl.DeliveredQuantity = item.DroppedQty > 0 ? item.DroppedQty : invFtl.DeliveredQuantity;
                                Context.Commit();
                            }

                            var btnModifyInfo = Context.DataContext.Invoices.Where(t => t.Id == item.InvoiceId).FirstOrDefault();
                            if (btnModifyInfo != null && btnModifyInfo.TrackableSchedule != null)
                            {
                                var deliveryScheduleXTrackableSchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(x => x.Id == btnModifyInfo.TrackableScheduleId).FirstOrDefault();
                                if (deliveryScheduleXTrackableSchedules != null)
                                {
                                    deliveryScheduleXTrackableSchedules.DeliveryLevelPO = item.DeliveryLevelPO;
                                }
                                Context.Commit();
                            }
                        }
                        if (isCreateNewVersion)
                        {
                            response = await new ConsolidatedDdtToInvoiceDomain(this).UpdateConsolidatedInvoices(invoiceHeaderid, postedInvoiceBolEdits);
                            if (response.StatusCode != Status.Success)
                            {
                                return response;
                            }
                        }

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageInvoiceUpdatedSuccess;
                    }
                    else
                    {
                        response.StatusMessage = "No difference in original records.";
                        response.StatusCode = Status.Warning;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "UpdateInvoices", ex.Message, ex);
            }
            return response;
        }
    }
}
