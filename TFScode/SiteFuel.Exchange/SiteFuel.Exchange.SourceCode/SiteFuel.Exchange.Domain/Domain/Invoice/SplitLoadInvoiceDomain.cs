using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using SiteFuel.Exchange.ViewModels.NewsfeedRequest;
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
    public class SplitLoadInvoiceDomain : InvoiceCommonDomain
    {
        public SplitLoadInvoiceDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public SplitLoadInvoiceDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<StatusViewModel> CreateInvoicesFromSplitLoadDraftDdts(UserContext userContext, string splitLoadChainId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                List<SplitLoadDraftDdtDetails> draftDdtList = await GetDraftDdtDetails(splitLoadChainId);
                var firstDraftDdt = draftDdtList.First();
                var exceptionModel = firstDraftDdt.ToExceptionRequestModel();
                exceptionModel.DroppedQuantity = draftDdtList.Sum(t => t.DraftDdt.DroppedGallons);
                var invoiceCreateDomain = new InvoiceCreateDomain(this);
                var exceptions = await invoiceCreateDomain.CheckAndGetInvoiceExceptions(exceptionModel);

                bool exceptionRaised = false;
                List<InvoiceModel> invoiceModels = new List<InvoiceModel>();
                foreach (var item in draftDdtList)
                {
                    var invoiceModel = GetDataToConvertDraftDdtToInvoice(item);
                    invoiceModel.InvoiceExceptions = exceptions;
                    if (invoiceModel.InvoiceExceptions.Any())
                    {
                        invoiceModel.WaitingFor = WaitingAction.ExceptionApproval;
                        invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                        invoiceModel.DisplayInvoiceNumber = invoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(invoiceModel.InvoiceTypeId, invoiceModel.WaitingFor);
                        exceptionRaised = true;
                    }
                    else
                    {
                        await SetPricingToDraftDdtModel(item, invoiceModel);
                        UpdateTaxDetails(item, invoiceModel);
                        UpdateCustomAttributeForTax(invoiceModel);
                    }
                    invoiceModels.Add(invoiceModel);
                }
                var updateStatus = await ConvertSplitLoadDraftDdtToInvoice(draftDdtList, invoiceModels, userContext.Id, userContext.CompanyId);
                if (updateStatus.StatusCode == Status.Failed)
                {
                    response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    return response;
                }
                response.StatusCode = Status.Success;
                response.EntityId = draftDdtList.First().DraftDdt.Id;
                foreach (var updatedDdt in draftDdtList)
                {
                    await SetSplitLoadInvoiceCreatedPostEvents(updatedDdt, userContext);
                    SetStatusMessage(updatedDdt.DraftDdt.InvoiceTypeId, (WaitingAction)updatedDdt.DraftDdt.WaitingFor, response, true);
                    SetStatusCustomMessage(updatedDdt.DraftDdt.DisplayInvoiceNumber, response);
                }

                if (!exceptionRaised && firstDraftDdt.Order.BuyerCompanyId != firstDraftDdt.Job.CompanyId)
                {
                    AddBrokerSplitInvoiceToQueueServiceAsync(splitLoadChainId, userContext.Id);
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                LogManager.Logger.WriteException("SplitLoadInvoiceDomain", "CreateInvoicesFromSplitLoadDraftDdts", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateInvoicesFromSplitLoadEddts(UserContext userContext, string SplitLoadChainId, decimal ApprovedQuantity)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                List<SplitLoadDraftDdtDetails> draftDdtList = await GetDraftDdtDetails(SplitLoadChainId);
                var firstDraftDdt = draftDdtList.First();

                // Add difference to the first DDT and create invoices for split load
                var totalDroppedQuantity = draftDdtList.Sum(t => t.DraftDdt.DroppedGallons);
                var difference = ApprovedQuantity - totalDroppedQuantity;
                firstDraftDdt.DraftDdt.DroppedGallons += difference;

                List<InvoiceModel> invoiceModels = new List<InvoiceModel>();
                foreach (var item in draftDdtList)
                {
                    var invoiceModel = GetDataToConvertDraftDdtToInvoice(item);
                    if (invoiceModel.InvoiceExceptions != null)
                    {
                        invoiceModel.InvoiceExceptions.Clear();
                    }
                    await SetPricingToDraftDdtModel(item, invoiceModel);
                    UpdateTaxDetails(item, invoiceModel);
                    UpdateCustomAttributeForTax(invoiceModel);
                    invoiceModels.Add(invoiceModel);
                }
                var updateStatus = await ConvertSplitLoadDraftDdtToInvoice(draftDdtList, invoiceModels, userContext.Id, userContext.CompanyId);
                if (updateStatus.StatusCode == Status.Failed)
                {
                    response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    return response;
                }
                response.StatusCode = Status.Success;
                response.EntityId = firstDraftDdt.DraftDdt.Id;
                foreach (var updatedDdt in draftDdtList)
                {
                    var ddtNumber = invoiceModels.Where(t => t.InvoiceNumberId == updatedDdt.DraftDdt.InvoiceHeader.InvoiceNumberId)
                                    .Select(t => t.DisplayInvoiceNumber).FirstOrDefault();
                    var newsfeedModel = new EddtToInvoiceCreatedNewsfeedModel()
                    {
                        BuyerCompanyId = updatedDdt.DraftDdt.Order.BuyerCompanyId,
                        SupplierCompanyId = updatedDdt.DraftDdt.Order.AcceptedCompanyId,
                        OrderId = updatedDdt.DraftDdt.Order.Id,
                        InvoiceId = updatedDdt.DraftDdt.Id,
                        InvoiceHeaderId = updatedDdt.DraftDdt.InvoiceHeaderId,
                        DisplayInvoiceNumber = updatedDdt.DraftDdt.DisplayInvoiceNumber,
                        TimeZoneName = updatedDdt.DraftDdt.Order.FuelRequest.Job.TimeZoneName,
                        IsDigitalDropTicket = IsDigitalDropTicket(updatedDdt.DraftDdt.InvoiceTypeId),
                        JobId = updatedDdt.DraftDdt.Order.FuelRequest.Job.Id,
                        WaitingFor = updatedDdt.DraftDdt.WaitingFor
                    };
                    await SetEddtNewsfeeds(userContext, newsfeedModel, ddtNumber ?? string.Empty);
                    response.StatusMessage = GetEddtStatusMessage(updatedDdt.DraftDdt.InvoiceTypeId, updatedDdt.DraftDdt.WaitingFor, updatedDdt.DraftDdt.DDTConversionReason);
                    SetStatusCustomMessage(updatedDdt.DraftDdt.DisplayInvoiceNumber, response);
                }

                if (firstDraftDdt.Order.BuyerCompanyId != firstDraftDdt.Job.CompanyId)
                {
                    AddBrokerSplitInvoiceToQueueServiceAsync(SplitLoadChainId, userContext.Id);
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                LogManager.Logger.WriteException("SplitLoadInvoiceDomain", "CreateInvoicesFromSplitLoadDraftDdts", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateInvoicesFromMobileSplitLoadDraftDdts(UserContext userContext, string splitLoadChainId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                List<SplitLoadDraftDdtDetails> draftDdtList = await GetDraftDdtDetails(splitLoadChainId);
                var firstDraftDdt = draftDdtList.First();
                var exceptionModel = firstDraftDdt.ToExceptionRequestModel();
                exceptionModel.DroppedQuantity = draftDdtList.Sum(t => t.DraftDdt.DroppedGallons);
                var invoiceCreateDomain = new InvoiceCreateDomain(this);
                var exceptions = await invoiceCreateDomain.CheckAndGetInvoiceExceptions(exceptionModel);
                List<InvoiceModel> invoiceModels = new List<InvoiceModel>();
                foreach (var item in draftDdtList)
                {
                    var invoiceModel = GetDataToConvertMobileDraftDdtToInvoice(item);
                    invoiceModel.InvoiceExceptions = exceptions;
                    if (invoiceModel.InvoiceExceptions.Any())
                    {
                        invoiceModel.WaitingFor = WaitingAction.ExceptionApproval;
                        invoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                        invoiceModel.DisplayInvoiceNumber = invoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(invoiceModel.InvoiceTypeId, invoiceModel.WaitingFor);
                    }
                    else
                    {
                        await SetPricingToDraftDdtModel(item, invoiceModel);
                        UpdateTaxDetails(item, invoiceModel);
                        UpdateCustomAttributeForTax(invoiceModel);
                    }
                    invoiceModels.Add(invoiceModel);
                }

                var updateStatus = await ConvertSplitLoadDraftDdtToInvoice(draftDdtList, invoiceModels, userContext.Id, userContext.CompanyId);
                if (updateStatus.StatusCode == Status.Failed)
                {
                    response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    return response;
                }
                response.StatusCode = Status.Success;
                response.EntityId = firstDraftDdt.DraftDdt.Id;
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                LogManager.Logger.WriteException("SplitLoadInvoiceDomain", "CreateInvoicesFromSplitLoadDraftDdts", ex.Message, ex);
            }
            return response;
        }

        private void UpdateCustomAttributeForTax(InvoiceModel invoiceModel)
        {
            if (invoiceModel.TaxDetails != null && invoiceModel.TaxDetails.IsTrueFillTax)
            {
                invoiceModel.AdditionalDetail.CustomAttributeViewModel.IsTrueFillTax = invoiceModel.TaxDetails.IsTrueFillTax;
                invoiceModel.AdditionalDetail.CustomAttribute = invoiceModel.AdditionalDetail.CustomAttributeViewModel.ToString();
            }
        }

        public async Task<StatusViewModel> CancelAllSplitLoadDraftDdtsAsync(string splitLoadChainId, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            var invoices = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == splitLoadChainId)
                                .Select(t => new
                                {
                                    Invoice = t,
                                    InvoicePreviousStatus = t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive),
                                }).ToListAsync();

            if (!invoices.Any())
            {
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageCancelDraftSuccess;
                return response;
            }

            var invoiceId = invoices.Select(t => t.Invoice.Id).First();
            var commonData = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId)
                                .Select(t => new
                                {
                                    TrackableSchedule = t.TrackableSchedule,
                                    Order = t.Order,
                                    OrderPreviousStatus = t.Order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive),
                                    OrderDroppedGallons = t.Order.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice).Sum(t1 => t1.DroppedGallons),
                                    FuelRequest = t.Order.FuelRequest,
                                    DeliveryTypeId = t.Order.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                    Job = t.Order.FuelRequest.Job
                                }).FirstOrDefaultAsync();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in invoices)
                    {
                        item.InvoicePreviousStatus.IsActive = false;

                        InvoiceXInvoiceStatusDetail invoiceStatus = new InvoiceXInvoiceStatusDetail();
                        invoiceStatus.StatusId = (int)InvoiceStatus.Canceled;
                        invoiceStatus.IsActive = true;
                        invoiceStatus.UpdatedBy = userContext.Id;
                        invoiceStatus.InvoiceId = item.Invoice.Id;
                        invoiceStatus.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.InvoiceXInvoiceStatusDetails.Add(invoiceStatus);

                        item.Invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;

                        if (commonData.Order != null)
                        {
                            UpdateHedgeAndSpotData(item.Invoice, commonData.Order.BuyerCompanyId, commonData.FuelRequest, commonData.Job);
                            AutoOpenOrder(commonData.Order, commonData.FuelRequest, commonData.DeliveryTypeId);

                            if (commonData.TrackableSchedule != null)
                            {
                                item.Invoice.TrackableScheduleId = null;
                                commonData.TrackableSchedule.DeliveryScheduleStatusId = GetDeliveryScheduleStatus(commonData.TrackableSchedule, commonData.Job.TimeZoneName, invoiceStatus.StatusId, item.Invoice.DropEndDate, true);
                            }
                        }
                    }
                    await Context.CommitAsync();
                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageCancelDraftSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusMessage = Resource.errMessageCanceSplitLoadDropFailed;
                    LogManager.Logger.WriteException("SplitLoadInvoiceDomain", "CancelAllSplitLoadDraftDdtsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> CancelSplitLoadDraftDdtAsync(int invoiceId, string splitLoadChainId, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            var invoices = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == splitLoadChainId)
                                .Select(t => new
                                {
                                    Invoice = t,
                                    InvoicePreviousStatus = t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive),
                                    InvoiceAdditionalDetail = t.InvoiceXAdditionalDetail
                                }).ToListAsync();

            var cancelledDraftDdt = invoices.FirstOrDefault(t => t.Invoice.Id == invoiceId);
            var commonData = await Context.DataContext.Invoices.Where(t => t.Id == cancelledDraftDdt.Invoice.Id)
                    .Select(t => new
                    {
                        TrackableSchedule = t.TrackableSchedule,
                        Order = t.Order,
                        OrderPreviousStatus = t.Order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive),
                        OrderDroppedGallons = t.Order.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice).Sum(t1 => t1.DroppedGallons),
                        FuelRequest = t.Order.FuelRequest,
                        DeliveryTypeId = t.Order.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                        Job = t.Order.FuelRequest.Job
                    }).FirstOrDefaultAsync();
            var ddtsToUpdateSequence = invoices.Where(t => t.InvoiceAdditionalDetail.SplitLoadSequence > cancelledDraftDdt.InvoiceAdditionalDetail.SplitLoadSequence);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (cancelledDraftDdt != null)
                    {
                        cancelledDraftDdt.Invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;
                        cancelledDraftDdt.InvoicePreviousStatus.IsActive = false;
                        InvoiceXInvoiceStatusDetail currentStatus = new InvoiceXInvoiceStatusDetail()
                        {
                            StatusId = (int)InvoiceStatus.Canceled,
                            IsActive = true,
                            InvoiceId = cancelledDraftDdt.Invoice.Id,
                            UpdatedBy = userContext.Id,
                            UpdatedDate = DateTimeOffset.Now
                        };
                        Context.DataContext.InvoiceXInvoiceStatusDetails.Add(currentStatus);
                        if (cancelledDraftDdt.Invoice.Order != null)
                        {
                            UpdateHedgeAndSpotData(cancelledDraftDdt.Invoice, commonData.Order.BuyerCompanyId, commonData.FuelRequest, commonData.Job);
                            AutoOpenOrder(commonData.Order, commonData.FuelRequest, commonData.DeliveryTypeId);

                            if (cancelledDraftDdt.Invoice.TrackableSchedule != null)
                            {
                                cancelledDraftDdt.Invoice.TrackableScheduleId = null;
                                commonData.TrackableSchedule.DeliveryScheduleStatusId = GetDeliveryScheduleStatus(commonData.TrackableSchedule, commonData.Job.TimeZoneName, currentStatus.StatusId, cancelledDraftDdt.Invoice.DropEndDate, true);
                            }
                        }
                    }
                    foreach (var item in ddtsToUpdateSequence)
                    {
                        item.InvoiceAdditionalDetail.SplitLoadSequence -= 1;
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    response.EntityId = invoices.Where(t => t.InvoiceAdditionalDetail.SplitLoadSequence == cancelledDraftDdt.InvoiceAdditionalDetail.SplitLoadSequence && t.Invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t => t.Invoice.Id).FirstOrDefault();
                    if (response.EntityId == 0)
                    {
                        response.EntityId = invoices.Where(t => t.InvoiceAdditionalDetail.SplitLoadSequence == (cancelledDraftDdt.InvoiceAdditionalDetail.SplitLoadSequence - 1) && t.Invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t => t.Invoice.Id).FirstOrDefault();
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageCancelDraftSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusMessage = Resource.errMessageCanceSplitLoadDropFailed;
                    LogManager.Logger.WriteException("SplitLoadInvoiceDomain", "CancelSplitLoadDraftDdtAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<InvoiceEditResponseViewModel> EditSplitDraftDDTAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new InvoiceEditResponseViewModel();
            try
            {
                var inoiceEditDomain = new InvoiceEditDomain(this);
                var invoiceEditRequest = await inoiceEditDomain.GetDraftDDTViewModelAsync(userContext, manualInvoiceModel);
                SetDropLocation(manualInvoiceModel, invoiceEditRequest.InvoiceModel);
                if (manualInvoiceModel.TaxType == TaxType.Standard && !invoiceEditRequest.IsTaxServiceSucceeded)
                {
                    response.StatusMessage = Resource.errMessageUpdateTaxFailed;
                    return response;
                }
                List<Invoice> draftDdtToUpdateHeaderDetails = null;
                if (manualInvoiceModel.SplitLoadSequence == 1)
                {
                    draftDdtToUpdateHeaderDetails = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == manualInvoiceModel.SplitLoadChainId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToListAsync();
                }
                response = await UpdateSplitLoadDraftDdtAsync(invoiceEditRequest, draftDdtToUpdateHeaderDetails);
                response.DisplayMode = PageDisplayMode.Edit;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SplitLoadInvoiceDomain", "EditSplitDraftDDTAsync", ex.Message, ex);
            }
            return response;
        }

        private StatusViewModel AddBrokerSplitInvoiceToQueueServiceAsync(string splitLoadChainId, int userId)
        {
            var response = new StatusViewModel();
            try
            {
                CreateBrokerSplitInvoiceQueueViewModel viewModel = new CreateBrokerSplitInvoiceQueueViewModel() { SplitLoadChainId = splitLoadChainId };

                string json = JsonConvert.SerializeObject(viewModel);

                var queueRequest = new QueueMessageViewModel()
                {
                    CreatedBy = userId,
                    QueueProcessType = QueueProcessType.BrokerSplitInvoiceCreation,
                    JsonMessage = json
                };
                var queueDomain = new QueueMessageDomain();
                var queueId = queueDomain.EnqeueMessage(queueRequest);
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SplitLoadInvoiceDomain", "AddBrokerSplitInvoiceToQueueServiceAsync", ex.Message + " splitLoadChainId:" + splitLoadChainId, ex);
            }

            return response;

        }

        private async Task<StatusViewModel> ConvertSplitLoadDraftDdtToInvoice(List<SplitLoadDraftDdtDetails> invoices, List<InvoiceModel> dataToUpdate, int userId, int companyId)
        {
            var response = new StatusViewModel();
            DeliveryReqStatusUpdateModel deliveryReqStatus = null;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var itemToUpdate in invoices)
                    {
                        var inputToConvert = dataToUpdate.First(t => t.Id == itemToUpdate.DraftDdt.Id);

                        deliveryReqStatus = UpdateDdtDetails(itemToUpdate, inputToConvert, userId);
                        UpdateHedgeAndSpotData(itemToUpdate.DraftDdt, itemToUpdate.Order.BuyerCompanyId, itemToUpdate.FuelRequest, itemToUpdate.Job);
                        UpdateProcessingFeeForTotalAmount(itemToUpdate.DraftDdt);

                        Context.DataContext.Entry(itemToUpdate.DraftDdt).State = EntityState.Modified;
                        await Context.CommitAsync();
                        SAPEnterpriseDomain sapDomain = new SAPEnterpriseDomain(this);
                        sapDomain.CreateSAPWorkflow(itemToUpdate.DraftDdt);
                        CreatePDIAPIWorkflow(itemToUpdate.DraftDdt, itemToUpdate.Order);
                        CreateQbAccountingWorkflowForInvoice(false, itemToUpdate.DraftDdt, itemToUpdate.Order, null);
                        CreateQbAccountingWorkflowForBill(false, itemToUpdate.DraftDdt, itemToUpdate.Order, null);
                    }
                    string timeZoneName = invoices.Select(t => t.InvoiceCreateViewModel.TimeZoneName).First();
                    var splitLoadInvoices = invoices.Select(t => t.DraftDdt).ToList();
                    await CreateBillingStatement(splitLoadInvoices, timeZoneName, companyId);

                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    if (deliveryReqStatus != null)
                    {
                        new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SplitLoadInvoiceDomain", "ConvertSplitLoadDraftDdtToInvoice", ex.Message, ex);
                }
            }

            return response;
        }

        private static void UpdateProcessingFeeForTotalAmount(Invoice invoice)
        {
            var processingFee = invoice.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.ProcessingFee && t.FeeSubTypeId == (int)FeeSubType.Percent);
            if (processingFee != null)
            {
                var totalAmount = (invoice.BasicAmount + (invoice.TotalFeeAmount ?? 0) + invoice.TotalTaxAmount - invoice.TotalDiscountAmount);
                processingFee.TotalFee = totalAmount * processingFee.Fee / 100;
                invoice.TotalFeeAmount += processingFee.TotalFee;
            }
        }

        private DeliveryReqStatusUpdateModel UpdateDdtDetails(SplitLoadDraftDdtDetails draftDdtsToUpdate, InvoiceModel invoiceModel, int userId)
        {
            DeliveryReqStatusUpdateModel response = null;
            var invoice = draftDdtsToUpdate.DraftDdt;
            var invoiceNumber = draftDdtsToUpdate.InvoiceNumber;
            var invoiceStatus = draftDdtsToUpdate.Status;
            var ftlDetail = invoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail).FirstOrDefault();
            ftlDetail.PricePerGallon = invoiceModel.PricePerGallon;
            ftlDetail.RackPrice = invoiceModel.RackPrice;
            invoice.BasicAmount = invoiceModel.BasicAmount;
            invoice.TotalTaxAmount = invoiceModel.TotalTaxAmount;
            invoice.WaitingFor = (int)invoiceModel.WaitingFor;
            invoice.InvoiceTypeId = invoiceModel.InvoiceTypeId;
            if (invoiceModel.TaxDetails != null && invoiceModel.TaxDetails.AvaTaxDetails.Any())
            {
                invoice.TaxDetails = invoiceModel.TaxDetails.ToEntity();
                invoice.TransactionId = invoiceModel.TransactionId;
            }
            invoice.BaseDroppedQuntity = invoiceModel.BaseDroppedQuntity;
            invoice.BasePrice = invoiceModel.BasePrice;
            invoice.BaseBasicAmount = invoiceModel.BaseBasicAmount;
            invoice.BaseTotalTaxAmount = invoiceModel.BaseTotalTaxAmount;
            invoice.BaseRackPrice = invoiceModel.BaseRackPrice;
            invoice.BaseTotalFeeAmount = invoiceModel.BaseTotalFeeAmount;
            invoice.UpdatedBy = userId;
            invoice.UpdatedDate = DateTimeOffset.Now;

            string invNumber = invoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(invoiceModel.InvoiceTypeId, invoiceModel.WaitingFor);
            invoice.DisplayInvoiceNumber = invNumber;
            invoiceNumber.Number = invNumber;

            invoiceStatus.IsActive = false;
            InvoiceXInvoiceStatusDetail invoiceStatusDetail = new InvoiceXInvoiceStatusDetail()
            {
                StatusId = invoiceModel.StatusId,
                IsActive = true,
                UpdatedBy = userId,
                UpdatedDate = DateTimeOffset.Now
            };
            invoice.InvoiceXInvoiceStatusDetails.Add(invoiceStatusDetail);
            if (invoice.InvoiceExceptions != null && invoice.InvoiceExceptions.Any())
            {
                Context.DataContext.InvoiceExceptions.RemoveRange(invoice.InvoiceExceptions);
            }
            invoice.InvoiceExceptions = invoiceModel.InvoiceExceptions.Select(t => t.ToEntity()).ToList();
            if (draftDdtsToUpdate.TrackableSchedule != null)
            {
                int scheduleStatus = GetDeliveryScheduleStatus(draftDdtsToUpdate.TrackableSchedule, draftDdtsToUpdate.InvoiceCreateViewModel.TimeZoneName, invoiceModel.StatusId, invoice.DropEndDate, false);
                response = UpdateTrackableScheduleStatus(draftDdtsToUpdate.TrackableSchedule?.Id, scheduleStatus, invoice);
            }
            if (!string.IsNullOrEmpty(draftDdtsToUpdate.DraftDdt.InvoiceXAdditionalDetail?.CustomAttribute))
                invoice.InvoiceXAdditionalDetail.CustomAttribute = draftDdtsToUpdate.DraftDdt.InvoiceXAdditionalDetail.CustomAttribute;
            return response;
        }

        private async Task CreateBillingStatement(List<Invoice> splitLoadInvoices, string timeZoneName, int supplierCompanyId)
        {
            try
            {
                var invoices = splitLoadInvoices.Where(t => t.InvoiceTypeId == (int)InvoiceType.Manual || t.InvoiceTypeId == (int)InvoiceType.MobileApp).ToList();
                if (invoices.Any() && invoices.Count == splitLoadInvoices.Count)
                {
                    BillingStatementDomain statementDomain = new BillingStatementDomain(this);
                    await statementDomain.GeneateBillingStatementForSplitLoadInvoice(invoices, timeZoneName, supplierCompanyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SplitLoadInvoiceDomain", "CreateBillingStatement", ex.Message, ex);
            }
        }

        private async Task<List<SplitLoadDraftDdtDetails>> GetDraftDdtDetails(string splitLoadChainId)
        {
            var draftDdtList = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == splitLoadChainId
                                                                       && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                     .Select(t => new SplitLoadDraftDdtDetails
                                                                     {
                                                                         DraftDdt = t,
                                                                         InvoiceNumber = t.InvoiceHeader.InvoiceNumber,
                                                                         DisplayInvoiceNumber = t.DisplayInvoiceNumber,
                                                                         BuyerCompanyName = t.Order.BuyerCompany.Name,
                                                                         SupplierCompanyName = t.Order.Company.Name,
                                                                         Order = t.Order,
                                                                         FuelRequest = t.Order.FuelRequest,
                                                                         Job = t.Order.FuelRequest.Job,
                                                                         InvoiceCreateViewModel = new InvoiceCreateViewModel
                                                                         {
                                                                             FuelTypeId = t.Order.FuelRequest.FuelTypeId,
                                                                             CityGroupTerminalId = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.CityGroupTerminalId).FirstOrDefault(),
                                                                             PricingTypeId = t.Order.FuelRequest.PricingTypeId,
                                                                             DropEndDate = t.DropEndDate,
                                                                             Currency = t.Currency,
                                                                             PricePerGallon = t.Order.FuelRequest.CreationTimeRackPPG,
                                                                             IsApprovalWorkflowEnabledForJob = t.Order.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                                                             IsBOLImageReq = t.Order.FuelRequest.FuelRequestDetail.IsBolImageRequired,
                                                                             IsDropImageReq = t.Order.FuelRequest.FuelRequestDetail.IsDropImageRequired,
                                                                             IsSignatureReq = t.Order.FuelRequest.Job.SignatureEnabled,
                                                                             TypeOfFuel = t.Order.FuelRequest.MstProduct.ProductDisplayGroupId,
                                                                             IsFTL = t.Order.IsFTL,
                                                                             BuyerCompanyName = t.Order.BuyerCompany.Name,
                                                                             SupplierCompanyName = t.Order.Company.Name,
                                                                             JobName = t.Order.FuelRequest.Job.Name,
                                                                             AcceptedCompanyId = t.Order.AcceptedCompanyId,
                                                                             InvoiceTypeId = t.SupplierPreferredInvoiceTypeId.Value,
                                                                             MappedParentFuelTypeId = t.Order.FuelRequest.MstProduct.MappedParentId,
                                                                             FuelProductCode = t.Order.FuelRequest.MstProduct.ProductCode,
                                                                             UoM = t.UoM,
                                                                             CountryCurrency = t.Order.FuelRequest.Job.MstCountry.Currency,
                                                                             IsSalesTaxExempted = t.Order.FuelRequest.Job.JobBudget.IsTaxExempted,
                                                                             JobAddess = new AddressViewModel
                                                                             {
                                                                                 Address = t.Order.FuelRequest.Job.Address,
                                                                                 City = t.Order.FuelRequest.Job.City,
                                                                                 StateCode = t.Order.FuelRequest.Job.MstState.Code,
                                                                                 CountryCode = t.Order.FuelRequest.Job.MstCountry.Code,
                                                                                 ZipCode = t.Order.FuelRequest.Job.ZipCode,
                                                                                 CountyName = t.Order.FuelRequest.Job.CountyName
                                                                             },
                                                                             JobCompanyId = t.Order.FuelRequest.Job.CompanyId,
                                                                             JobId = t.Order.FuelRequest.Job.Id,
                                                                             TimeZoneName = t.Order.FuelRequest.Job.TimeZoneName,
                                                                             ApprovalUserId = t.Order.FuelRequest.Job.IsApprovalWorkflowEnabled ? t.Order.FuelRequest.Job.JobXApprovalUsers.Where(t1 => t1.IsActive).Select(t1 => t1.User.Id).FirstOrDefault() : 0,
                                                                             TerminalAddress = t.Order.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                                                                                    || t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.TerminalId).FirstOrDefault() == null ? null : new AddressViewModel
                                                                                                    {
                                                                                                        Address = t.Order.MstExternalTerminal.Address,
                                                                                                        City = t.Order.MstExternalTerminal.City,
                                                                                                        StateCode = t.Order.MstExternalTerminal.StateCode,
                                                                                                        CountryCode = t.Order.MstExternalTerminal.CountryCode,
                                                                                                        ZipCode = t.Order.MstExternalTerminal.ZipCode,
                                                                                                        CountyName = t.Order.MstExternalTerminal.CountyName
                                                                                                    },
                                                                             FuelDropped = t.DroppedGallons,
                                                                             DropStartDate = t.DropStartDate,
                                                                             OtherProductTaxes = t.Order.OrderTaxDetails
                                                                                                        .Where(t1 => t1.IsActive).Select(t1 => new TaxViewModel
                                                                                                        {
                                                                                                            TaxAmount = t1.TaxRate,
                                                                                                            TaxPricingTypeId = t1.TaxPricingTypeId,
                                                                                                            TaxDescription = t1.TaxDescription
                                                                                                        }).ToList(),
                                                                             BuyerCompanyId = t.Order.BuyerCompanyId,
                                                                             JobStateId = t.Order.FuelRequest.Job.StateId,
                                                                             QuantityIndicatorTypeId = t.QuantityIndicatorTypeId,
                                                                             SupplierAllowance = t.InvoiceXAdditionalDetail.SupplierAllowance ?? 0,
                                                                             ActualDropQuantity = t.DroppedGallons,
                                                                             IsVariousFobOrigin = t.Order.FuelRequest.Job.LocationType == JobLocationTypes.Various,
                                                                             UserId = t.CreatedBy
                                                                         },
                                                                         TrackableSchedule = t.TrackableSchedule,
                                                                         BolDetails = t.InvoiceXBolDetails.Any(t1 => t1.InvoiceFtlDetail != null) ? new BolDetailViewModel()
                                                                         {
                                                                             NetQuantity = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.NetQuantity).FirstOrDefault(),
                                                                             GrossQuantity = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.GrossQuantity).FirstOrDefault(),
                                                                         } : null,
                                                                         FuelRequestPricingDetail = t.Order.FuelRequest.FuelRequestPricingDetail != null ? new FuelRequestPricingDetailsViewModel()
                                                                         {
                                                                             RequestPriceDetailId = t.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                                                                             FuelRequestId = t.Order.FuelRequest.FuelRequestPricingDetail.FuelRequestId,
                                                                             PricingQuantityIndicatorTypeId = t.Order.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId,
                                                                             StateDefaultQuantityIndicatorId = t.Order.FuelRequest.FuelRequestPricingDetail.FuelRequest.Job.MstState.QuantityIndicatorTypeId,
                                                                             PricingCode = new PricingCodeDetailViewModel { Code = t.Order.FuelRequest.FuelRequestPricingDetail.PricingCode },
                                                                             TruckLoadTypeId = t.Order.FuelRequest.FuelRequestDetail.TruckLoadTypeId,
                                                                             TruckLoadTypes = (TruckLoadTypes)t.Order.FuelRequest.FuelRequestDetail.TruckLoadTypeId,
                                                                             FreightOnBoardTypes = t.Order.FuelRequest.FuelRequestPricingDetail.FuelRequest.FreightOnBoardTypeId != null ? (FreightOnBoardTypes)t.Order.FuelRequest.FuelRequestPricingDetail.FuelRequest.FreightOnBoardTypeId : 0
                                                                         } : null,
                                                                         BuyerTaxExemptLicence = t.Order.FuelRequest.Job.Company.TaxExemptLicenses.Where(t1 => t1.IsActive).Select(t1 => t1.EntityCustomId).FirstOrDefault(),
                                                                         SupplierTaxExemptLicence = t.Order.Company.TaxExemptLicenses.Where(t1 => t1.IsActive).Select(t1 => t1.EntityCustomId).FirstOrDefault(),
                                                                         IsEndSupplier = t.Order.IsEndSupplier,
                                                                         CountryId = t.Order.FuelRequest.Job.CountryId,
                                                                         InvoiceDispatchLocation = t.InvoiceDispatchLocation.Select(t1 => new DispatchLocationViewModel() { ZipCode = t1.ZipCode, LocationType = t1.LocationType, StateId = t1.StateId.Value }).ToList(),
                                                                         OrderMaxQuantity = t.Order.BrokeredMaxQuantity ?? t.Order.FuelRequest.MaxQuantity,
                                                                         DeliveryTypeId = t.Order.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                                                         JobCompanyName = t.Order.FuelRequest.Job.Company.Name,
                                                                         ApprovalUserName = t.Order.FuelRequest.Job.IsApprovalWorkflowEnabled ? t.Order.FuelRequest.Job.JobXApprovalUsers.Where(t1 => t1.IsActive).Select(t1 => t1.User.FirstName + " " + t1.User.LastName).FirstOrDefault() : (string)null,
                                                                         ApprovalUserOnboardedType = t.Order.FuelRequest.Job.IsApprovalWorkflowEnabled ? t.Order.FuelRequest.Job.JobXApprovalUsers.Where(t1 => t1.IsActive).Select(t1 => t1.User.OnboardedTypeId).FirstOrDefault() : (int?)null,
                                                                         OrderTotalDelivered = t.Order.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t1.IsActive && !t1.IsBuyPriceInvoice).Sum(t1 => t1.DroppedGallons),
                                                                         Status = t.InvoiceXInvoiceStatusDetails.OrderByDescending(t1 => t1.Id).FirstOrDefault(),
                                                                         InvoicePreferenceTypeId = t.SupplierPreferredInvoiceTypeId,
                                                                         WaitingForAction = (WaitingAction)t.WaitingFor
                                                                     })
                                                                    .ToListAsync();
            return draftDdtList;
        }

        private InvoiceModel GetDataToConvertDraftDdtToInvoice(SplitLoadDraftDdtDetails splitLoadDdt)
        {
            InvoiceModel modelToUpdateDdt = new InvoiceModel()
            {
                Id = splitLoadDdt.DraftDdt.Id,
                CreatedDate = splitLoadDdt.DraftDdt.CreatedDate,
                DisplayInvoiceNumber = splitLoadDdt.InvoiceNumber.Number,
                DroppedGallons = splitLoadDdt.DraftDdt.DroppedGallons,
                ExchangeRate = splitLoadDdt.DraftDdt.ExchangeRate,
                TotalFeeAmount = splitLoadDdt.DraftDdt.TotalFeeAmount
            };

            if (splitLoadDdt.WaitingForAction == WaitingAction.Images)
            {
                modelToUpdateDdt.WaitingFor = WaitingAction.Images;
                modelToUpdateDdt.InvoiceTypeId = GetInvoiceCreationTypeToDdt(splitLoadDdt.DraftDdt.InvoiceTypeId);
                modelToUpdateDdt.StatusId = (int)InvoiceStatus.Received;
            }
            else
            {
                var isApprovalWorkFlowEnabled = splitLoadDdt.InvoiceCreateViewModel.IsApprovalWorkflowEnabledForJob;
                modelToUpdateDdt.InvoiceTypeId = isApprovalWorkFlowEnabled ? GetInvoiceCreationTypeToDdt(splitLoadDdt.DraftDdt.InvoiceTypeId) : splitLoadDdt.DraftDdt.SupplierPreferredInvoiceTypeId.Value;
                modelToUpdateDdt.WaitingFor = isApprovalWorkFlowEnabled ? WaitingAction.CustomerApproval : WaitingAction.Nothing;
                modelToUpdateDdt.StatusId = isApprovalWorkFlowEnabled ? (int)InvoiceStatus.WaitingForApproval : (int)InvoiceStatus.Received;
            }
            modelToUpdateDdt.AdditionalDetail = new InvoiceXAdditionalDetailViewModel()
            {
                CustomAttribute = splitLoadDdt.DraftDdt.InvoiceXAdditionalDetail.CustomAttribute
            };
            if (!string.IsNullOrWhiteSpace(modelToUpdateDdt.AdditionalDetail.CustomAttribute))
            {
                modelToUpdateDdt.AdditionalDetail.CustomAttributeViewModel = JsonConvert.DeserializeObject<InvoiceCustomAttributeViewModel>(modelToUpdateDdt.AdditionalDetail.CustomAttribute);
            }
            return modelToUpdateDdt;
        }

        private async Task SetPricingToDraftDdtModel(SplitLoadDraftDdtDetails ddtDetails, InvoiceModel modelToUpdateDdt)
        {
            await UpdatePricingData(ddtDetails.InvoiceCreateViewModel, ddtDetails.BolDetails, ddtDetails.FuelRequestPricingDetail, modelToUpdateDdt);
            if (modelToUpdateDdt.WaitingFor != WaitingAction.Nothing)
            {
                modelToUpdateDdt.InvoiceTypeId = GetInvoiceCreationTypeToDdt(ddtDetails.DraftDdt.InvoiceTypeId);
            }
            if (modelToUpdateDdt.WaitingFor != WaitingAction.UpdatedPrice)
            {
                if (ddtDetails.DraftDdt.FuelRequestFees.Any(t => t.IncludeInPPG))
                {
                    modelToUpdateDdt.BasicAmount += ddtDetails.DraftDdt.FuelRequestFees.Where(t => t.IncludeInPPG).Select(t => t.TotalFee ?? 0).Sum();
                    modelToUpdateDdt.PricePerGallon = modelToUpdateDdt.BasicAmount / modelToUpdateDdt.DroppedGallons;
                }
                UpdateSplitLoadInvoiceBaseAmounts(ddtDetails.InvoiceCreateViewModel, modelToUpdateDdt);
            }
        }

        private InvoiceModel GetDataToConvertMobileDraftDdtToInvoice(SplitLoadDraftDdtDetails splitDdt)
        {
            InvoiceModel modelToUpdateDdt = new InvoiceModel()
            {
                Id = splitDdt.DraftDdt.Id,
                CreatedDate = splitDdt.DraftDdt.CreatedDate,
                DisplayInvoiceNumber = splitDdt.InvoiceNumber.Number,
                DroppedGallons = splitDdt.DraftDdt.DroppedGallons,
                ExchangeRate = splitDdt.DraftDdt.ExchangeRate,
                TotalFeeAmount = splitDdt.DraftDdt.TotalFeeAmount
            };

            if (splitDdt.DraftDdt.WaitingFor == (int)WaitingAction.BolDetails)
            {
                modelToUpdateDdt.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                modelToUpdateDdt.WaitingFor = WaitingAction.BolDetails;
                modelToUpdateDdt.StatusId = (int)InvoiceStatus.Received;
            }
            else if (splitDdt.InvoiceCreateViewModel.IsApprovalWorkflowEnabledForJob)
            {
                modelToUpdateDdt.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                modelToUpdateDdt.WaitingFor = WaitingAction.CustomerApproval;
                modelToUpdateDdt.StatusId = (int)InvoiceStatus.WaitingForApproval;
            }
            else
            {
                modelToUpdateDdt.InvoiceTypeId = splitDdt.DraftDdt.SupplierPreferredInvoiceTypeId.Value;
                modelToUpdateDdt.WaitingFor = splitDdt.DraftDdt.WaitingFor == (int)WaitingAction.Nothing ? WaitingAction.AvalaraTax : (WaitingAction)splitDdt.DraftDdt.WaitingFor;
                modelToUpdateDdt.StatusId = (int)InvoiceStatus.Received;
            }
            modelToUpdateDdt.AdditionalDetail = new InvoiceXAdditionalDetailViewModel()
            {
                CustomAttribute = splitDdt.DraftDdt.InvoiceXAdditionalDetail.CustomAttribute
            };
            if (!string.IsNullOrWhiteSpace(modelToUpdateDdt.AdditionalDetail.CustomAttribute))
            {
                modelToUpdateDdt.AdditionalDetail.CustomAttributeViewModel = JsonConvert.DeserializeObject<InvoiceCustomAttributeViewModel>(modelToUpdateDdt.AdditionalDetail.CustomAttribute);
            }
            return modelToUpdateDdt;
        }

        private async Task SetSplitLoadInvoiceCreatedPostEvents(SplitLoadDraftDdtDetails viewModel, UserContext userContext)
        {
            CreateSplitLoadInvoiceOutputViewModel modelForPostEvents = GetCreateInvoiceResponseViewModel(viewModel, userContext);
            var newsfeedDomain = new NewsfeedDomain(this);
            switch (modelForPostEvents.WaitingFor)
            {
                case WaitingAction.UpdatedPrice:
                case WaitingAction.AvalaraTax:
                    var newsfeedViewModel = GetDigitalDropTicketApprovalNewsfeedModel(modelForPostEvents);
                    NewsfeedEvent newsfeedEvent = modelForPostEvents.WaitingFor == WaitingAction.UpdatedPrice ? NewsfeedEvent.SupplierCreatedDDTWaitingForUpdatedPrice : NewsfeedEvent.DDTCreatedWaitingForTaxes;
                    await newsfeedDomain.SetDDTWaitingForNewsfeed(userContext, newsfeedViewModel, newsfeedEvent);
                    break;

                case WaitingAction.CustomerApproval:
                    if (modelForPostEvents.IsApprovalWorkflowEnabledForJob && modelForPostEvents.ApprovalUserId > 0 &&
                        modelForPostEvents.ApprovalUserOnboardedType != (int)OnboardedType.ThirdPartyOrderOnboarded &&
                        modelForPostEvents.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual)
                    {
                        var approvalViewModel = GetApprovalWorkflowNewsfeedModel(userContext, modelForPostEvents);
                        approvalViewModel.IsBrokeredOrder = modelForPostEvents.IsBrokeredOrder;
                        await newsfeedDomain.SetApprovalWorkflowEnabledNewsFeeds(approvalViewModel);
                    }
                    break;

                default:
                    var newsfeedRequestModel = GetManualInvoiceCreatedNewsfeedModel(modelForPostEvents);
                    await newsfeedDomain.SetManualInvoiceCreatedNewsfeed(userContext, newsfeedRequestModel);
                    break;
            }
        }

        private CreateSplitLoadInvoiceOutputViewModel GetCreateInvoiceResponseViewModel(SplitLoadDraftDdtDetails viewModel, UserContext userContext)
        {
            CreateSplitLoadInvoiceOutputViewModel modelForPostEvents = new CreateSplitLoadInvoiceOutputViewModel()
            {

                WaitingFor = (WaitingAction)viewModel.DraftDdt.WaitingFor,
                ApprovalUserId = viewModel.DraftDdt.WaitingFor == (int)WaitingAction.CustomerApproval ? viewModel.InvoiceCreateViewModel.ApprovalUserId : (int?)null,
                IsApprovalWorkflowEnabledForJob = viewModel.InvoiceCreateViewModel.IsApprovalWorkflowEnabledForJob,
                ApprovalUserOnboardedType = viewModel.DraftDdt.WaitingFor == (int)WaitingAction.CustomerApproval ? viewModel.ApprovalUserOnboardedType : (int?)null,
                InvoiceTypeId = viewModel.DraftDdt.InvoiceTypeId,
                IsBrokeredOrder = viewModel.Order.BuyerCompanyId != viewModel.InvoiceCreateViewModel.JobCompanyId,
                InvoiceId = viewModel.DraftDdt.Id,
                InvoiceNumber = viewModel.DraftDdt.DisplayInvoiceNumber,
                OrderId = viewModel.Order.Id,
                PoNumber = viewModel.Order.PoNumber,
                BuyerCompanyId = viewModel.Order.BuyerCompanyId,
                SupplierCompanyId = viewModel.Order.AcceptedCompanyId,
                CreatedDate = viewModel.DraftDdt.CreatedDate,
                DriverId = viewModel.DraftDdt.DriverId ?? 0,
                DropStartDate = viewModel.DraftDdt.DropStartDate,
                DropEndDate = viewModel.DraftDdt.DropEndDate,
                DroppedGallons = viewModel.DraftDdt.DroppedGallons,
                UoM = viewModel.DraftDdt.UoM,
                TimeZoneName = viewModel.InvoiceCreateViewModel.TimeZoneName,
                SupplierPreferredInvoiceTypeId = viewModel.DraftDdt.SupplierPreferredInvoiceTypeId,
                UserId = userContext.Id,
                JobCompanyId = viewModel.InvoiceCreateViewModel.JobCompanyId,
                JobCompanyName = viewModel.JobCompanyName,
                JobId = viewModel.InvoiceCreateViewModel.JobId,
                DeliveryTypeId = viewModel.DeliveryTypeId,
                SupplierCompanyName = userContext.CompanyName,
                ApprovalUserName = viewModel.DraftDdt.WaitingFor == (int)WaitingAction.CustomerApproval ? viewModel.ApprovalUserName : (string)null,
            };
            if (viewModel.OrderMaxQuantity > 0)
            {
                modelForPostEvents.DropPercentPerDelivery = viewModel.OrderTotalDelivered / viewModel.OrderMaxQuantity * 100;
            }
            return modelForPostEvents;
        }

        private InvoiceTaxDetailsViewModel GetTaxDetailsForSplitLoadInvoice(InvoiceCreateViewModel invoiceCreateModel, BolDetailViewModel bolData, InvoiceModel draftDdt, ICollection<DispatchLocationViewModel> dispatchLocations, int? invoicePreferenceType)
        {
            InvoiceTaxDetailsViewModel invoiceTax = new InvoiceTaxDetailsViewModel();
            if (invoiceCreateModel.TypeOfFuel != (int)ProductDisplayGroups.OtherFuelType)
            {
                DispatchLocationViewModel dropLocation = null;
                if (dispatchLocations != null)
                {
                    dropLocation = dispatchLocations.FirstOrDefault(t => t.LocationType == (int)LocationType.Drop);
                }
                var avalaraServiceModel = GetAvalaraServiceViewModelForSpitLoadInvoice(invoiceCreateModel, draftDdt, dropLocation);
                var taxResponse = GetFtlOrderTaxForStandardProduct(avalaraServiceModel, invoiceCreateModel, bolData, draftDdt.FuelFees);
                invoiceTax = taxResponse.TaxDetailsViewModel;
            }
            else
            {
                if (invoiceCreateModel.OtherProductTaxes != null && invoiceCreateModel.OtherProductTaxes.Any() && invoicePreferenceType != (int)InvoiceType.DigitalDropTicketManual)
                {
                    invoiceTax = GetTaxDetailsFromInputs(invoiceCreateModel.OtherProductTaxes, invoiceCreateModel.Currency, draftDdt.Id, draftDdt.BasicAmount, draftDdt.DroppedGallons);
                }
                invoiceTax.StatusCode = Status.Success;
            }
            return invoiceTax;
        }

        private AvalaraServiceViewModel GetAvalaraServiceViewModelForSpitLoadInvoice(InvoiceCreateViewModel invoiceCreateModel, InvoiceModel draftDdt, DispatchLocationViewModel fuelDropLocation)
        {
            var avalaraServiceModel = new AvalaraServiceViewModel
            {
                FuelTypeId = invoiceCreateModel.MappedParentFuelTypeId ?? invoiceCreateModel.FuelTypeId,
                FuelProductCode = invoiceCreateModel.FuelProductCode,
                JobUoM = invoiceCreateModel.UoM,
                JobCurrency = invoiceCreateModel.Currency,
                CountryCurrency = invoiceCreateModel.CountryCurrency,
                IsSalesTaxExempted = invoiceCreateModel.IsSalesTaxExempted,
                DestinationJobAddress = invoiceCreateModel.JobAddess,
                SourceTerminalAddress = invoiceCreateModel.TerminalAddress,
                InvoiceNumber = draftDdt.DisplayInvoiceNumber,
                DroppedGallons = invoiceCreateModel.FuelDropped ?? 0,
                PricePerGallon = draftDdt.PricePerGallon,
                DropEndDate = invoiceCreateModel.DropStartDate,
                InvoiceDate = draftDdt.CreatedDate,
                BuyerCustomId = invoiceCreateModel.BuyerCustomId,
                SellerCustomId = invoiceCreateModel.SellerCustomId,
                IsDirectTaxCompany = invoiceCreateModel.IsDirectTaxCompany,
                SupplierAllowance = invoiceCreateModel.SupplierAllowance,
                Exclusions = GetTaxEclusionIfExist(invoiceCreateModel.UserId),
                BuyerCompanyId = invoiceCreateModel.BuyerCompanyId,
                SupplierCompanyId = invoiceCreateModel.AcceptedCompanyId,
                JobId = invoiceCreateModel.JobId
            };
            if (invoiceCreateModel.IsVariousFobOrigin && fuelDropLocation != null)
            {
                string stateCode;
                avalaraServiceModel.DestinationJobAddress.ZipCode = SetFirstZipCodeOfState(fuelDropLocation.StateId, avalaraServiceModel.DestinationJobAddress.StateCode, out stateCode);
                avalaraServiceModel.DestinationJobAddress.StateCode = stateCode;
            }
            return avalaraServiceModel;
        }

        private async Task<FuelPricingResponseViewModel> GetPricingDetails(InvoiceCreateViewModel pricingRequestViewModel, WaitingAction waitingFor)
        {
            FuelPricingResponseViewModel pricingData = new FuelPricingResponseViewModel();
            if (!pricingRequestViewModel.IsApprovalWorkflowEnabledForJob)
            {
                var fuelPricingRequestViewModel = GetFuelPricingRequestViewModel(pricingRequestViewModel, waitingFor);
                pricingData = await GetFuelPriceByPricingTypeAsync(fuelPricingRequestViewModel, pricingRequestViewModel.TypeOfFuel);
            }
            return pricingData;
        }

        private void UpdatePpgAndBasicAmount(FuelPricingResponseViewModel pricingData, decimal supplierAllowance, InvoiceModel draftDdt)
        {
            if (pricingData.WaitingFor == WaitingAction.Nothing)
            {
                draftDdt.PricePerGallon = pricingData.PricePerGallon;
                draftDdt.RackPrice = pricingData.TerminalPrice;
                draftDdt.BasicAmount = Math.Round(draftDdt.DroppedGallons * draftDdt.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                if (supplierAllowance > 0)
                {
                    draftDdt.BasicAmount -= Math.Round(draftDdt.DroppedGallons * supplierAllowance, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                }
            }
        }

        private void UpdateSplitLoadInvoiceBaseAmounts(InvoiceCreateViewModel viewModel, InvoiceModel draftDdt)
        {
            draftDdt.BaseDroppedQuntity = VolumeConverter.GetBaseQuantity(viewModel.UoM, draftDdt.DroppedGallons);
            draftDdt.BasePrice = MoneyConverter.GetBaseAmount(viewModel.Currency, draftDdt.PricePerGallon, draftDdt.ExchangeRate);
            draftDdt.BaseBasicAmount = MoneyConverter.GetBaseAmount(viewModel.Currency, draftDdt.BasicAmount, draftDdt.ExchangeRate);
            draftDdt.BaseTotalTaxAmount = MoneyConverter.GetBaseAmount(viewModel.Currency, draftDdt.TotalTaxAmount, draftDdt.ExchangeRate);
            draftDdt.BaseRackPrice = MoneyConverter.GetBaseAmount(viewModel.Currency, draftDdt.RackPrice, draftDdt.ExchangeRate);
            if (draftDdt.TotalFeeAmount.HasValue)
            {
                draftDdt.BaseTotalFeeAmount = MoneyConverter.GetBaseAmount(viewModel.Currency, draftDdt.TotalFeeAmount.Value, draftDdt.ExchangeRate);
            }
        }

        private async Task UpdatePricingData(InvoiceCreateViewModel pricingRequestViewModel, BolDetailViewModel bolDetails, FuelRequestPricingDetailsViewModel fuelPricingDetails, InvoiceModel draftDdt)
        {
            pricingRequestViewModel.InvoiceTypeId = draftDdt.InvoiceTypeId;
            if (fuelPricingDetails != null)
            {
                pricingRequestViewModel.FuelRequestPricingDetail = fuelPricingDetails;
            }

            var pricingData = await GetPricingDetails(pricingRequestViewModel, draftDdt.WaitingFor);
            UpdatePpgAndBasicAmount(pricingData, pricingRequestViewModel.SupplierAllowance, draftDdt);
            if (pricingData.WaitingFor != (int)WaitingAction.Nothing)
            {
                draftDdt.WaitingFor = pricingData.WaitingFor;
            }
        }

        private void UpdateTaxDetails(SplitLoadDraftDdtDetails viewModel, InvoiceModel modelToUpdateDdt)
        {
            if (modelToUpdateDdt.WaitingFor == WaitingAction.Nothing && modelToUpdateDdt.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual)
            {
                if (viewModel.BuyerTaxExemptLicence != null)
                {
                    viewModel.InvoiceCreateViewModel.BuyerCustomId = viewModel.BuyerTaxExemptLicence;
                }
                if (viewModel.SupplierTaxExemptLicence != null && viewModel.IsEndSupplier)
                {
                    viewModel.InvoiceCreateViewModel.SellerCustomId = viewModel.SupplierTaxExemptLicence;
                }
                viewModel.InvoiceCreateViewModel.IsDirectTaxCompany = Context.DataContext.DirectTaxes.Any(t => t.CompanyId == viewModel.InvoiceCreateViewModel.BuyerCompanyId && t.StateId == viewModel.InvoiceCreateViewModel.JobStateId && t.IsActive);
                var taxDetails = GetTaxDetailsForSplitLoadInvoice(viewModel.InvoiceCreateViewModel, viewModel.BolDetails, modelToUpdateDdt, viewModel.InvoiceDispatchLocation, viewModel.InvoicePreferenceTypeId);
                if (taxDetails == null || taxDetails.StatusCode == Status.Failed)
                {
                    modelToUpdateDdt.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                    modelToUpdateDdt.WaitingFor = WaitingAction.AvalaraTax;
                }
                else
                {
                    modelToUpdateDdt.TaxDetails = taxDetails;
                    modelToUpdateDdt.TotalTaxAmount = taxDetails.TotalTaxAmount;
                    modelToUpdateDdt.TransactionId = taxDetails.TranId.ToString();
                }
            }
        }
    }

    public class SplitLoadDraftDdtDetails
    {
        public Invoice DraftDdt { get; set; }
        public InvoiceNumber InvoiceNumber { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public string BuyerCompanyName { get; set; }
        public string SupplierCompanyName { get; set; }
        public Order Order { get; set; }
        public FuelRequest FuelRequest { get; set; }
        public Job Job { get; set; }
        public InvoiceCreateViewModel InvoiceCreateViewModel { get; set; }
        public BolDetailViewModel BolDetails { get; set; }
        public DeliveryScheduleXTrackableSchedule TrackableSchedule { get; set; }
        public FuelRequestPricingDetailsViewModel FuelRequestPricingDetail { get; set; }
        public string BuyerTaxExemptLicence { get; set; }
        public string SupplierTaxExemptLicence { get; set; }
        public InvoiceXInvoiceStatusDetail Status { get; set; }
        public List<DispatchLocationViewModel> InvoiceDispatchLocation { get; set; }
        public bool IsEndSupplier { get; set; }
        public int? ApprovalUserOnboardedType { get; set; }
        public int? InvoicePreferenceTypeId { get; set; }
        public string JobCompanyName { get; set; }
        public string ApprovalUserName { get; set; }
        public int DeliveryTypeId { get; set; }
        public int CountryId { get; set; }
        public decimal OrderMaxQuantity { get; set; }
        public decimal OrderTotalDelivered { get; set; }
        public WaitingAction WaitingForAction { get; set; }
        public string CustomAttribute { get; set; }
    }
}
