using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Domain.Domain.ThirdParty;

namespace SiteFuel.Exchange.Domain
{
    public class CreditRebillInvoiceDomain : InvoiceCommonDomain
    {
        public CreditRebillInvoiceDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public CreditRebillInvoiceDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<StatusViewModel> CreateCreditInvoiceAsync(int userId, int invoiceId, bool isRebilled = false, List<int> trackableScheduleIds = null)
        {
            var response = new StatusViewModel();
            List<DeliveryReqStatusUpdateModel> scheduleStatusModel = null;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (Context.DataContext.InvoiceXInvoiceStatusDetails.Any(t => t.InvoiceId == invoiceId && t.StatusId == (int)InvoiceStatus.Credited && t.IsActive))
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageAlreadyCredited;
                        return response;
                    }
                    bool isCreditInvoiceExists = Context.DataContext.InvoiceXAdditionalDetails.Any(t => t.OriginalInvoiceId == invoiceId && t.Invoice.InvoiceTypeId == (int)InvoiceType.CreditInvoice && t.Invoice.IsActive && t.Invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active);
                    if (isCreditInvoiceExists)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageAlreadyCreditedRebilled;
                        return response;
                    }
                    var invoices = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).SelectMany(t => t.InvoiceHeader.Invoices).Select(t => new CreditInvoiceInputViewModel()
                    {
                        Invoice = t,
                        InvoiceDispatchLocation = t.InvoiceDispatchLocation,
                        InvoiceXAdditionalDetail = t.InvoiceXAdditionalDetail,
                        AssetDrops = t.AssetDrops,
                        SpecialInstructions = t.InvoiceXSpecialInstructions,
                        TaxDetails = t.TaxDetails,
                        Fees = t.FuelRequestFees,
                        PaymentDiscounts = t.PaymentDiscounts,
                        Discounts = t.Discounts,
                        InvoiceNumber = t.InvoiceHeader.InvoiceNumber,
                        Order = t.Order,
                        InvoiceStatus = t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive),
                        TrackableSchedule = t.TrackableSchedule,
                        OrderPreviousStatus = t.Order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive),
                        OrderDroppedGallons = t.Order.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice).Sum(t1 => t1.DroppedGallons),
                        FuelRequest = t.Order.FuelRequest,
                        DeliveryTypeId = t.Order.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                        Job = t.Order.FuelRequest.Job,
                        BolDetails = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail).ToList(),
                        SplitChainId = t.InvoiceXAdditionalDetail.SplitLoadChainId,
                        SplitChainSequenceId = t.InvoiceXAdditionalDetail.SplitLoadSequence,
                        BDRDetails = t.BDRDetail,
                        IsMarineLocation = t.Order.FuelRequest.Job.IsMarine
                        
                    }).ToListAsync();
                    List<Invoice> invoiceEntities = new List<Invoice>();
                    foreach (var invoice in invoices)
                    {
                        var IsAutoRebilledInvoice = Context.DataContext.InvoiceXAdditionalDetails.Any(t => t.OriginalInvoiceId == invoiceId && t.Invoice.DDTConversionReason == (int)DDTConversionReason.AutoCreditRebill);
                        Invoice creditInvoice = invoice.ToCreditInvoice(userId);
                        creditInvoice.DDTConversionReason = IsAutoRebilledInvoice ? (int)DDTConversionReason.AutoCreditRebill : (int)DDTConversionReason.Nothing;
                        creditInvoice.InvoiceXAdditionalDetail.SplitLoadChainId = GetCreditRebillSplitLoadChainId(invoice.SplitChainId, invoice.SplitChainSequenceId);
                        invoiceEntities.Add(creditInvoice);
                    }
                    if (invoiceEntities.Any())
                    {
                        scheduleStatusModel = await SaveCreditInvoiceDetails(invoiceEntities, invoices, userId, isRebilled, trackableScheduleIds);
                    }
                    transaction.Commit();

                    response.EntityId = invoiceEntities.FirstOrDefault(t => t.InvoiceXAdditionalDetail.OriginalInvoiceId == invoiceId).Id;
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageCreditInvoiceCreationSuccess;
                    await SetCreditInvoiceCreatePostEvents(userId, invoiceEntities, invoices);
                    new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(scheduleStatusModel);
                    var cumulationList =  await CreateListOfCumulationEntitiesToUpdateForCredRebill(invoiceEntities);
                    if (cumulationList != null && cumulationList.Any())
                    {
                         await new ConsolidatedInvoiceDomain(this).UpdateCumulationQuantitiesPostInvoiceCreate(cumulationList);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (isRebilled)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                    response.StatusMessage = Resource.errMessageCreditInvoiceCreationFailed;
                    LogManager.Logger.WriteException("CreditRebillInvoiceDomain", "CreateCreditInvoiceAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<string>> CreateCreditInvoiceFromQueueService(CreditInvoiceQueueServiceInputViewModel viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("CreditRebillInvoiceDomain", "CreateCreditInvoiceFromQueueService"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    var status = await CreateCreditInvoiceAsync(viewModel.UserId, viewModel.InvoiceId, true, viewModel.TrackableScheduleIds);
                    var invoiceData = Context.DataContext.InvoiceXAdditionalDetails.Where(t => t.OriginalInvoiceId == viewModel.InvoiceId && t.Invoice.IsActive && t.Invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t => new { t.Invoice.Order.FuelRequest.Job.TimeZoneName, t.Invoice.Order.AcceptedCompanyId, t.SplitLoadChainId });
                    var IsCreditAndRebillCreated = invoiceData.Count() == 2;
                    if (IsCreditAndRebillCreated && !string.IsNullOrEmpty(invoiceData.FirstOrDefault().SplitLoadChainId))
                    {
                        await CheckForSplitLoadInvoiceAndGenerateStatement(invoiceData.FirstOrDefault().SplitLoadChainId, invoiceData.FirstOrDefault().AcceptedCompanyId, invoiceData.FirstOrDefault().TimeZoneName);
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("CreditRebillInvoiceDomain", "CreateCreditInvoiceFromQueueService", ex.Message, ex);
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

        public async Task<StatusViewModel> RebillInvoiceAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel, bool isRebillFromQueueService = false)
        {
            var response = new StatusViewModel();
            var invoiceEditDomain = new InvoiceEditDomain(this);
            try
            {
                InvoiceEditRequestViewModel invoiceEditRequest = null;
                int originalInvoiceId = manualInvoiceModel.InvoiceId;
                int? trackableScheduleId = manualInvoiceModel.TrackableScheduleId;
                if (Context.DataContext.InvoiceXInvoiceStatusDetails.Any(t => t.InvoiceId == originalInvoiceId && t.StatusId == (int)InvoiceStatus.CreditedAndRebilled))
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageAlreadyCreditedRebilled;
                    return response;
                }
                if (manualInvoiceModel.IsQuanityOrDateChanged)
                {
                    invoiceEditRequest = await invoiceEditDomain.GetInvoiceEditViewModelForNewDateAsync(userContext, manualInvoiceModel);
                    if (invoiceEditRequest == null)
                    {
                        response.StatusMessage = Resource.errMessageSelectedDateCannotBeSet;
                        return response;
                    }
                }
                else
                {
                    invoiceEditRequest = await invoiceEditDomain.GetInvoiceEditViewModelAsync(userContext, manualInvoiceModel);
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

                CheckForProcessingFeeOnTotalAmount(invoiceEditRequest.InvoiceModel);
                InvoiceEditResponseViewModel updateResponse;
                if (manualInvoiceModel.SplitLoadSequence.HasValue)
                {
                    invoiceEditRequest.InvoiceModel.AdditionalDetail.SplitLoadChainId = GetCreditRebillSplitLoadChainId(invoiceEditRequest.InvoiceModel.AdditionalDetail?.SplitLoadChainId, invoiceEditRequest.InvoiceModel.AdditionalDetail?.SplitLoadSequence);
                    invoiceEditRequest.InvoiceModel.DDTConversionReason = isRebillFromQueueService ? (int)DDTConversionReason.AutoCreditRebill : (int)DDTConversionReason.Nothing;
                }
                updateResponse = await CreateRebilledInvoiceAsync(invoiceEditRequest);
                response.StatusCode = updateResponse.StatusCode;
                if (updateResponse.StatusCode == Status.Failed)
                {
                    return response;
                }

                var notificationDomain = new NotificationDomain(this);
                await notificationDomain.AddNotificationEventAsync(EventType.RebilledInvoiceCreated, updateResponse.InvoiceHeaderId, userContext.Id);
                await SetSystemAutoClosedOrderWhileEditing(invoiceEditRequest, updateResponse);

                UpdateManualInvoiceViewModel(manualInvoiceModel, invoiceEditRequest, updateResponse);
                var newsfeedDomain = new NewsfeedDomain(this);
                var newsfeedRequestModel = GetRebillInvoiceCreatedNewsfeedModel(manualInvoiceModel, invoiceEditRequest);
                await newsfeedDomain.SetRebillInvoiceCreatedNewsfeed(userContext, newsfeedRequestModel);

                response.EntityId = updateResponse.InvoiceId;
                response.StatusMessage = Resource.errMessageRebilledInvoiceUpdatedSuccess;
                List<int> trackableScheduleIds = null;
                if (trackableScheduleId != null)
                {
                    trackableScheduleIds = new List<int>() { trackableScheduleId.Value };
                }
                StatusViewModel creditInvoiceResponse = AddCreditInvoiceToQueueServiceAsync(originalInvoiceId, trackableScheduleIds, userContext.Id);

                if (!isRebillFromQueueService && !string.IsNullOrEmpty(manualInvoiceModel.SplitLoadChainId))
                {
                    await UpdateCommonDetailsForOtherSplitInvoices(userContext, manualInvoiceModel, originalInvoiceId, updateResponse);
                }
                if (creditInvoiceResponse.StatusCode == Status.Failed)
                {
                    response.StatusMessage += creditInvoiceResponse.StatusMessage;
                }
                UpdateInvoiceActionResponseStatus(updateResponse.IsDtnUploaded, response);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CreditRebillInvoiceDomain", "RebillInvoiceAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<string>> CreateRebillInvoiceFromQueueServiceAsync(RebillInvoiceQueueServiceInputViewModel viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("CreditRebillInvoiceDomain", "CreateRebillInvoiceFromQueueServiceAsync"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    UserContext userContext = new UserContext() { Id = viewModel.UserId, CompanyId = viewModel.CompanyId, Name = viewModel.UserName, CompanyName = viewModel.CompanyName };
                    var manualInvoiceModel = await GetManualInvoiceViewModel(viewModel.InvoiceId, viewModel.RebilledInvoiceId);
                    var response = await RebillInvoiceAsync(userContext, manualInvoiceModel, true);
                    if (response.StatusCode == Status.Failed)
                    {
                        errorInfo.Add(response.StatusMessage);
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("CreditRebillInvoiceDomain", "CreateRebillInvoiceFromQueueServiceAsync", ex.Message, ex);
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

        private static ManualInvoiceCreatedNewsfeedModel GetCreditInvoiceCreatedNewsfeedModel(CreditInvoiceInputViewModel viewModel, Invoice creditInvoice)
        {
            return new ManualInvoiceCreatedNewsfeedModel
            {
                InvoiceId = creditInvoice.Id,
                InvoiceNumber = creditInvoice.DisplayInvoiceNumber,
                OrderId = viewModel.Order.Id,
                PoNumber = viewModel.Invoice.PoNumber,
                BuyerCompanyId = viewModel.Order.BuyerCompanyId,
                SupplierCompanyId = viewModel.Order.AcceptedCompanyId,
                JobId = viewModel.Invoice.Order.FuelRequest.JobId,
                InvoiceTypeId = viewModel.Invoice.InvoiceTypeId,
                TimeZoneName = viewModel.Order.FuelRequest.Job.TimeZoneName,
                InvoiceHeaderId = creditInvoice.InvoiceHeaderId,
                OriginalInvoiceNumber = viewModel.Invoice.DisplayInvoiceNumber
            };
        }

        private static ManualInvoiceCreatedNewsfeedModel GetRebillInvoiceCreatedNewsfeedModel(ManualInvoiceViewModel viewModel, InvoiceEditRequestViewModel invoiceEditviewModel)
        {
            return new ManualInvoiceCreatedNewsfeedModel
            {
                InvoiceId = viewModel.InvoiceId,
                InvoiceNumber = viewModel.DisplayInvoiceNumber,
                OrderId = viewModel.OrderId,
                PoNumber = viewModel.PoNumber,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = invoiceEditviewModel.SupplierCompanyId,
                JobId = viewModel.JobId,
                InvoiceTypeId = viewModel.InvoiceTypeId,
                TimeZoneName = viewModel.TimeZoneName,
                OriginalInvoiceNumber = invoiceEditviewModel.InvoiceModel.DisplayInvoiceNumber
            };
        }

        private async Task<List<DeliveryReqStatusUpdateModel>> SaveCreditInvoiceDetails(List<Invoice> creditInvoices, List<CreditInvoiceInputViewModel> originalInvoices, int userId, bool isRebilled, List<int> trackableScheduleIds = null)
        {
            List<DeliveryReqStatusUpdateModel> response = new List<DeliveryReqStatusUpdateModel>();
            InvoiceNumber invoiceNumber = new InvoiceNumber();
            Context.DataContext.InvoiceNumbers.Add(invoiceNumber);


            var invoiceHeader = GenerateInvoiceHeader(creditInvoices);
            invoiceNumber.InvoiceHeaderDetails.Add(invoiceHeader);
            await Context.CommitAsync();
            Order dtnOrder = null; Invoice dtnInvoice = null; bool sendDtnFile = true; bool sendDeliveryDetailsToPDI = false;
            foreach (var creditInvoice in creditInvoices)
            {
                
                var originalInvoice = originalInvoices.FirstOrDefault(t => t.Invoice.Id == creditInvoice.InvoiceXAdditionalDetail.OriginalInvoiceId);
                if (originalInvoice != null)
                {
                    foreach (var bolDetail in originalInvoice.BolDetails)
                    {
                        InvoiceXBolDetail invoiceXBol = new InvoiceXBolDetail() { InvoiceHeaderId = invoiceHeader.Id, BolDetailId = bolDetail.Id };
                        creditInvoice.InvoiceXBolDetails.Add(invoiceXBol);
                    }
                    creditInvoice.TransactionId = creditInvoice.DisplayInvoiceNumber = originalInvoice.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFCI).Replace(ApplicationConstants.SFRB, ApplicationConstants.SFCI);
                    if (!isRebilled)
                    {
                        originalInvoice.InvoiceStatus.IsActive = false;
                        originalInvoice.InvoiceStatus.UpdatedBy = userId;
                        originalInvoice.InvoiceStatus.UpdatedDate = DateTimeOffset.Now;

                        InvoiceXInvoiceStatusDetail statusDetail = new InvoiceXInvoiceStatusDetail()
                        {
                            InvoiceId = originalInvoice.Invoice.Id,
                            StatusId = (int)InvoiceStatus.Credited,
                            IsActive = true,
                            UpdatedBy = userId,
                            UpdatedDate = DateTimeOffset.Now
                        };
                        Context.DataContext.InvoiceXInvoiceStatusDetails.Add(statusDetail);
                    }
                    if (originalInvoice.Order !=null && originalInvoice.Order.FuelRequest.Job.IsMarine && creditInvoice.BDRDetail !=null)
                    {
                        //Check if existing invoices header is there, if not then take orderid from current creditinvoices list
                        var firstOrderId =  invoiceNumber.InvoiceHeaderDetails.Select(t => t.Invoices.OrderBy(x => x.OrderId).Select(x => x.OrderId).FirstOrDefault()).FirstOrDefault() ?? creditInvoices.OrderBy(t => t.OrderId).Select(x => x.OrderId).FirstOrDefault();

                        creditInvoice.BDRDetail = creditInvoice.BDRDetail.ToBDREntity(invoiceHeader.InvoiceNumber, (firstOrderId != null ? firstOrderId.ToString() : ""));
                    }
                    
                    var trackableScheduleStatus = await UpdateInvoiceDependentEntitiesPostCreate(originalInvoice, isRebilled, trackableScheduleIds);
                    if (trackableScheduleStatus != null)
                    {
                        response.Add(trackableScheduleStatus);
                    }
                    if (originalInvoice.Order.SendDtnFile)
                    {
                        dtnOrder = originalInvoice.Order;
                        dtnInvoice = creditInvoice;
                    }
                    else
                    {
                        sendDtnFile = false;
                    }

                    if (originalInvoice.Order.OrderAdditionalDetail != null && originalInvoice.Order.OrderAdditionalDetail.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails)
                    {
                        sendDeliveryDetailsToPDI = true;
                    }
                }
            }
            creditInvoices.ForEach(t => invoiceHeader.Invoices.Add(t));
            await Context.CommitAsync();

            if (sendDtnFile)
            {
                CreateDtnFileGenerationWorkflow(dtnInvoice, dtnOrder);
            }

            var firstInvoice = creditInvoices.FirstOrDefault();
            if (sendDeliveryDetailsToPDI)
            {
                CreatePDIAPIWorkflow(firstInvoice, firstInvoice.Order);
            }

            
             SAPEnterpriseDomain sapDomain = new SAPEnterpriseDomain(this);
             sapDomain.CreateSAPWorkflow(firstInvoice);
            
            var invoiceBaseDomain = new InvoiceBaseDomain(this);
            invoiceBaseDomain.CreateQbAccountingWorkflowForCreditInvoice(firstInvoice, firstInvoice.Order);
            return response;
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

        private static void UpdateManualInvoiceViewModel(ManualInvoiceViewModel manualInvoiceModel, InvoiceEditRequestViewModel invoiceEditRequest, InvoiceEditResponseViewModel updateResponse)
        {
            manualInvoiceModel.InvoiceId = updateResponse.InvoiceId;
            manualInvoiceModel.DisplayInvoiceNumber = updateResponse.InvoiceNumber;
            manualInvoiceModel.TimeZoneName = invoiceEditRequest.TimeZoneName;
            manualInvoiceModel.InvoiceNumberId = invoiceEditRequest.InvoiceModel.InvoiceNumberId;
            manualInvoiceModel.JobId = updateResponse.JobId;
        }

        private static string GetCreditRebillSplitLoadChainId(string parentSplitLoadChainId, int? parentSequence)
        {
            if (!string.IsNullOrEmpty(parentSplitLoadChainId))
            {
                var commonString = "C" + (parentSequence ?? 0) + "R";
                var splitArr = parentSplitLoadChainId.Split(new[] { commonString }, StringSplitOptions.None);
                if (splitArr.Length > 1)
                {
                    var intValue = Convert.ToInt32(splitArr[1]);
                    intValue++;
                    parentSplitLoadChainId = splitArr[0] + commonString + intValue;
                }
                else
                {
                    parentSplitLoadChainId = parentSplitLoadChainId + commonString + 1;
                }
            }
            return parentSplitLoadChainId;
        }

        private async Task<ManualInvoiceViewModel> GetManualInvoiceViewModel(int invoiceId, int rebilledInvoiceId)
        {
            var invoiceDomain = new InvoiceDomain(this);

            var manualInvoiceModel = await invoiceDomain.GetManualSplitInvoiceForEditAsync(invoiceId);
            var rebilledInvoice = Context.DataContext.Invoices.Where(t => t.Id == rebilledInvoiceId).Select(t => new { t.Id, t.PaymentDueDate, t.PaymentTermId, t.NetDays, t.TrackableScheduleId, t.DriverId, InvoiceFtlDetail = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail).FirstOrDefault() }).FirstOrDefault();
            manualInvoiceModel.PaymentTermId = rebilledInvoice.PaymentTermId;
            manualInvoiceModel.NetDays = rebilledInvoice.NetDays;
            manualInvoiceModel.TrackableScheduleId = rebilledInvoice.TrackableScheduleId;
            manualInvoiceModel.DriverId = rebilledInvoice.DriverId;
            manualInvoiceModel.TaxType = TaxType.Standard;
            manualInvoiceModel.BolDetails = rebilledInvoice.InvoiceFtlDetail.ToViewModel();

            return manualInvoiceModel;
        }

        private async Task SetCreditInvoiceCreatePostEvents(int userId, List<Invoice> invoiceEntities, List<CreditInvoiceInputViewModel> otherDetails)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            var invoices = invoiceEntities.GroupBy(t => t.OrderId).ToList();
            var user = Context.DataContext.Users.Where(t => t.Id == userId).Select(t => new { t.Id, t.CompanyId, t.FirstName, t.LastName, CompanyName = t.Company.Name }).FirstOrDefault();
            var userContext = new UserContext() { Id = user.Id, CompanyId = user.CompanyId ?? 0, Name = $"{user.FirstName} {user.LastName}", CompanyName = user.CompanyName };
            foreach (var invoice in invoices)
            {
                var invoiceModel = invoice.FirstOrDefault();
                var additionalDetail = otherDetails.FirstOrDefault(t => t.Order.Id == invoiceModel.OrderId);
                var newsfeedRequestModel = GetCreditInvoiceCreatedNewsfeedModel(additionalDetail, invoiceModel);
                await newsfeedDomain.SetCreditInvoiceCreatedNewsfeed(userContext, newsfeedRequestModel);
            }
            var notificationDomain = new NotificationDomain(this);
            await notificationDomain.AddNotificationEventAsync(EventType.CreditInvoiceCreated, invoiceEntities.Select(t => t.InvoiceHeaderId).FirstOrDefault(), userId);
        }

        private async Task<DeliveryReqStatusUpdateModel> UpdateInvoiceDependentEntitiesPostCreate(CreditInvoiceInputViewModel invoice, bool isRebilled, List<int> trackableScheduleIds = null)
        {
            DeliveryReqStatusUpdateModel drToUpdate = null;
            UpdateHedgeAndSpotData(invoice.Invoice, invoice.Order.BuyerCompanyId, invoice.FuelRequest, invoice.Job);
            if (invoice.Invoice.TrackableSchedule != null)
            {
                invoice.Invoice.TrackableScheduleId = null;
                if (!isRebilled || (trackableScheduleIds != null && !trackableScheduleIds.Any(t => t == invoice.TrackableSchedule.Id)))
                {
                    invoice.TrackableSchedule.DeliveryScheduleStatusId = GetDeliveryScheduleStatus(invoice.TrackableSchedule, invoice.Job.TimeZoneName, (int)InvoiceStatus.Received, invoice.Invoice.DropEndDate, true);
                    if (!string.IsNullOrWhiteSpace(invoice.TrackableSchedule.FrDeliveryRequestId))
                    {
                        drToUpdate = new DeliveryReqStatusUpdateModel() { DeliveryRequestId = invoice.TrackableSchedule.FrDeliveryRequestId, ScheduleStatusId = invoice.TrackableSchedule.DeliveryScheduleStatusId, UserId = invoice.InvoiceStatus.UpdatedBy };
                    }
                }
            }
            await Context.CommitAsync();
            return drToUpdate;
        }

        private async Task<List<CumulationQuantityUpdateViewModel>> CreateListOfCumulationEntitiesToUpdateForCredRebill(List<Invoice> invoiceModels)
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
                            && invoice.InvoiceTypeId != ((int)InvoiceType.DigitalDropTicketMobileApp))
                        {

                            var updatedQty = new CumulationQuantityUpdateViewModel();
                            updatedQty.DroppedGallons = Math.Abs(invoice.DroppedGallons);
                            updatedQty.RequestPriceDetailsId = invoice.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId;
                            tempList.Add(updatedQty);

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
                            cumlationQty.DroppedGallons = TotalDroppedGallons *-1; // need to substract qty in case of credit 
                            cumlationQty.RequestPriceDetailsId = requestPriceDetailsId;
                            responseList.Add(cumlationQty);
                        }
                    } 
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CreditRebillInvoiceDomain", "CreateListOfCumulationEntitiesToUpdateForCredRebill", ex.Message, ex);
            }
            return responseList;
        }
    }

    public class GetRelatedSplitInvoicesInputViewModel
    {
        public Invoice Invoice { get; set; }

        public string SplitLoadChainId { get; set; }

        public int? OriginalInvoiceId { get; set; }
    }

    public class CreditInvoiceInputViewModel
    {
        public Invoice Invoice { get; set; }
        public ICollection<InvoiceDispatchLocation> InvoiceDispatchLocation { get; set; }
        public InvoiceXAdditionalDetail InvoiceXAdditionalDetail { get; set; }
        public ICollection<AssetDrop> AssetDrops { get; set; }
        public ICollection<InvoiceXSpecialInstruction> SpecialInstructions { get; set; }
        public ICollection<TaxDetail> TaxDetails { get; set; }
        public ICollection<FuelFee> Fees { get; set; }
        public InvoiceNumber InvoiceNumber { get; set; }
        public Order Order { get; set; }
        public FuelRequest FuelRequest { get; set; }
        public Job Job { get; set; }
        public int DeliveryTypeId { get; set; }
        public InvoiceXInvoiceStatusDetail InvoiceStatus { get; set; }
        public DeliveryScheduleXTrackableSchedule TrackableSchedule { get; set; }
        public decimal OrderDroppedGallons { get; set; }
        public OrderXStatus OrderPreviousStatus { get; set; }
        public ICollection<Discount> Discounts { get; set; }
        public ICollection<PaymentDiscount> PaymentDiscounts { get; set; }
        public List<InvoiceFtlDetail> BolDetails { get; set; }
        public string SplitChainId { get; set; }
        public int? SplitChainSequenceId { get; set; }
        public BDRDetail BDRDetails { get; set; }

        public bool IsMarineLocation { get; set; }
    }
}
