using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Domain.Mappers.TankRental;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Invoice.Pdf;
using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using SiteFuel.Exchange.ViewModels.NewsfeedRequest;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using SiteFuel.Exchange.Domain.Domain.ThirdParty;
using System.IO;
using System.Net.Mail;
using SiteFuel.Exchange.EmailManager;

namespace SiteFuel.Exchange.Domain
{
    public class InvoiceDomain : InvoiceCommonDomain
    {
        public InvoiceDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public InvoiceDomain(BaseDomain domain) : base(domain)
        {
        }

        public AssignToOrderViewModel GetOrderforInvoice(int invoiceId)
        {
            var response = new AssignToOrderViewModel();
            var invoice = Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => new { t.OrderId, t.UoM }).SingleOrDefault();
            if (invoice != null)
            {
                response.InvoiceUoM = invoice.UoM;
                if (invoice.OrderId != null)
                {
                    response.OrderId = invoice.OrderId.Value;
                    response.StatusCode = Status.Warning;
                    response.StatusMessage = Resource.warningInvoiceAlreadyAssigned;
                }
            }
            return response;
        }

        //public bool IsBuySellOrder(int orderId)
        //{
        //    return Context.DataContext.Orders.Any(t => t.Id == orderId && t.ExternalBrokerBuySellDetail != null);
        //}

        public async Task<StatusViewModel> CreateInvoiceForAmpAsync(AmpJobViewModel ampJobViewModel, Order order, Job job, string csvPath, List<string> errorInfo)
        {
            var response = new StatusViewModel();
            StringBuilder processMessage = new StringBuilder();

            var assetDomain = new AssetDomain(this);
            using (var tracer = new Tracer("InvoiceDomain", "CreateInvoiceForAmpAsync"))
            {
                try
                {
                    if (order != null && job != null)
                    {
                        var externaldrop = order.ExternalDropDetails.FirstOrDefault(t => t.OrderId == order.Id && !t.DeliveryScheduleXTrackableSchedule.IsDropped &&
                        t.DropDate.Year == job.StartDate.Year && t.DropDate.Month == job.StartDate.Month && t.DropDate.Day == job.StartDate.Day);
                        var createdBy = externaldrop == null ? order.AcceptedBy : externaldrop.UserId;
                        var trackableScheduleId = externaldrop == null ? null : externaldrop.TrackableScheduleId;

                        await assetDomain.AddAmpDropStreamAssets(ampJobViewModel, job, createdBy);
                        var assetDrops = GetAmpAssetDrops(ampJobViewModel, order, job);
                        var createdDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                        var invoiceViewModel = new InvoiceViewModel
                        {
                            OrderId = order.Id,
                            PoNumber = order.PoNumber,
                            DroppedGallons = ampJobViewModel.Drops.Sum(t => t.DropQuantity),
                            DropStartDate = ampJobViewModel.StartDate,
                            DropEndDate = ampJobViewModel.EndDate,
                            CreatedBy = createdBy,
                            CreatedDate = createdDate,
                            UpdatedBy = createdBy,
                            UpdatedDate = createdDate,
                            UserId = createdBy,
                            DriverId = createdBy,
                            TrackableScheduleId = trackableScheduleId,
                            CsvFilePath = csvPath,
                            TimeZoneName = order.FuelRequest.Job.TimeZoneName
                        };

                        if (order.ExternalBrokerOrderDetail.InvoicePreferenceId == 3)
                        {
                            invoiceViewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                        }
                        else
                        {
                            invoiceViewModel.InvoiceTypeId = (int)InvoiceType.Manual;
                        }

                        var manualInvoiceModel = new ManualInvoiceViewModel
                        {
                            ExternalBrokerId = order.ExternalBrokerId ?? 0
                        };
                        manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = GetFuelRequestFee(order.FuelRequest);

                        manualInvoiceModel.ExternalBrokeredOrder.BrokeredOrderFee = new TPOBrokeredOrderFeeViewModel();
                        manualInvoiceModel.ExternalBrokeredOrder.BrokeredOrderFee.Currency = order.FuelRequest.Currency;
                        manualInvoiceModel.ExternalBrokeredOrder.BrokeredOrderFee.UoM = order.FuelRequest.UoM;
                        var freightFee = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == ((int)FeeType.FreightFee).ToString() && t.DiscountLineItemId == null);
                        if (freightFee != null)
                        {
                            manualInvoiceModel.ExternalBrokeredOrder.BrokeredOrderFee.FreightFeeTypeId = Convert.ToInt32(freightFee.FeeTypeId);
                            manualInvoiceModel.ExternalBrokeredOrder.BrokeredOrderFee.FreightFeeSubTypeId = freightFee.FeeSubTypeId;
                            manualInvoiceModel.ExternalBrokeredOrder.BrokeredOrderFee.FreightFee = freightFee.Fee ?? 0;
                        }
                        var additionalFees = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Where(t =>
                            t.FeeTypeId == ((int)FeeType.OtherFee).ToString() || t.FeeTypeId.Contains(Constants.OtherCommonFeeCode) && t.DiscountLineItemId == null);
                        if (additionalFees.Any())
                        {
                            manualInvoiceModel.ExternalBrokeredOrder.BrokeredOrderFee.AdditionalFees =
                                additionalFees.Select(t => new BrokeredOrderFeeViewModel
                                {
                                    FeeTypeId = (int)FeeType.OtherFee,
                                    FeeSubTypeId = t.FeeSubTypeId,
                                    Fee = t.Fee ?? 0,
                                    FeeDetails = t.OtherFeeDescription
                                }).ToList();
                        }
                        manualInvoiceModel.PaymentTermId = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentTermId;
                        manualInvoiceModel.NetDays = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).NetDays;
                        manualInvoiceModel.BolDetails.TypeofFuel = order.FuelRequest.MstProduct.ProductDisplayGroupId;
                        manualInvoiceModel.Assets = assetDrops;
                        manualInvoiceModel.CsvFilePath = csvPath;
                        manualInvoiceModel.BolDetails.TerminalId = order.TerminalId ?? 0;
                        manualInvoiceModel.BolDetails.CityGroupTerminalId = order.CityGroupTerminalId ?? 0;

                        if (order.ExternalBrokerBuySellDetail != null)
                        {
                            response = await GenerateInvoiceForSellPrice(invoiceViewModel, manualInvoiceModel, true);
                        }
                        else
                        {
                            response = await GenerateInvoiceForAmpAsync(invoiceViewModel, manualInvoiceModel);
                        }
                        ampJobViewModel.InvoiceViewModel = invoiceViewModel;
                        if (response.StatusCode == Status.Success)
                        {
                            var authenticationDomain = new AuthenticationDomain(this);
                            var userContext = await authenticationDomain.GetUserContextAsync(createdBy, CompanyType.Supplier);
                            var newsfeed = new NewsfeedDomain(this);
                            await newsfeed.SetInvoiceCreatedNewsfeed(userContext, manualInvoiceModel, false);
                            SetAmpInvoiceSuccessMessage(processMessage, ampJobViewModel, invoiceViewModel.DisplayInvoiceNumber);
                        }
                        else
                        {
                            SetAmpInvoiceFailedMessage(processMessage, ampJobViewModel);
                        }
                        if (processMessage.Length > 0)
                        {
                            errorInfo.Add(processMessage.ToString());
                        }
                    }
                    else
                    {
                        errorInfo.Add(SetAmpOrderFailedProcessMessage(ampJobViewModel));
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceDomain", "CreateInvoiceForAmpAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private static string SetAmpOrderFailedProcessMessage(AmpJobViewModel item)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-maroon'>").Append("<b>Job Info: </b>")
                            .Append($"Job: {item.JobName}, FuelType: {item.AmpProductType} <br>Failed to create invoice. Reason:</b>Order with {item.AmpProductType} not found.</p><br>");
            return processMessage.ToString();
        }

        public async Task<StatusViewModel> EditInvoicePoNumberAsync(UserContext userContext, int invoiceId, string poNumber)
        {
            var response = new StatusViewModel(Status.Success);
            var newsfeedDomain = new NewsfeedDomain(this);
            try
            {
                var invoice = await Context.DataContext.Invoices.SingleOrDefaultAsync(t => t.Id == invoiceId);
                if (invoice != null)
                {
                    var previousPoNumber = invoice.PoNumber;
                    if (!string.IsNullOrWhiteSpace(poNumber) && previousPoNumber != poNumber)
                    {
                        var invoiceEditDomain = new InvoiceEditDomain(this);
                        response = await invoiceEditDomain.InvoiceEditForInvoicePoNumberAsync(userContext, invoiceId, poNumber, invoice.OrderId);

                        if (response.StatusCode == (int)Status.Success)
                        {
                            await newsfeedDomain.SetPONumberRenamedNewsfeed(userContext, invoice.Order, poNumber, previousPoNumber);

                            var message = new OrderMessageViewModel { PreviousPoNumber = previousPoNumber };
                            var jsonMessage = new JavaScriptSerializer().Serialize(message);
                            await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.PoNumberChangedForMultipleDeliveryOrder, response.EntityHeaderId, userContext.Id, null, jsonMessage);
                        }
                    }
                }

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessagePoUpdatedSuccessfully;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "EditInvoicePoNumberAsync", ex.Message, ex);
                response.StatusMessage = Resource.errMessageUpdateFailed;
            }

            return response;
        }

        private void SetAmpInvoiceFailedMessage(StringBuilder processMessage, AmpJobViewModel viewModel)
        {
            processMessage.Append("<p class='color-maroon'>").Append("<b>Failed to create invoice.")
                .Append($"Job Name: {viewModel.JobName}, Fuel Type: {viewModel.AmpProductType}</b></p>");
        }

        private void SetAmpInvoiceSuccessMessage(StringBuilder processMessage, AmpJobViewModel viewModel, string inviceNumber)
        {
            if (inviceNumber.Contains(ApplicationConstants.SFDD))
            {
                processMessage.Append("<p class='color-green'><b>").Append("Digital Drop Ticket created successfully for ")
                    .Append($"Job Name: {viewModel.JobName}, Fuel Type: {viewModel.AmpProductType} Digital Drop Ticket Number: {inviceNumber}</b></p>");
            }
            else
            {
                processMessage.Append("<p class='color-green'><b>").Append("Invoice created successfully for ")
                    .Append($"Job Name: {viewModel.JobName}, Fuel Type: {viewModel.AmpProductType} Invoice Number: {inviceNumber}</b></p>");
            }
        }

        public async Task<StatusViewModel> CreateInvoiceFromDropTicketWithBol(ManualInvoiceViewModel manualInvoiceviewModel, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var ddtToConvertId = manualInvoiceviewModel.InvoiceId;
                manualInvoiceviewModel.InvoiceNumber.Id = 0;
                manualInvoiceviewModel.InvoiceId = 0;
                manualInvoiceviewModel.IsInvoiceFromDropTicket = true;
                if (manualInvoiceviewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual)
                {
                    manualInvoiceviewModel.InvoiceTypeId = (int)InvoiceType.Manual;
                }
                else if (manualInvoiceviewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    manualInvoiceviewModel.InvoiceTypeId = (int)InvoiceType.MobileApp;
                }
                var preferInvoiceTypeId = manualInvoiceviewModel.InvoiceTypeId;
                var ddtNumber = manualInvoiceviewModel.InvoiceNumber.Number;

                response = await GenerateInvoiceFromDDT(manualInvoiceviewModel, ddtToConvertId);
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                if (response.StatusCode == Status.Success)
                {
                    await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed(userContext, manualInvoiceviewModel, ddtNumber);
                    response.EntityNumber = response.StatusMessage + " - " + manualInvoiceviewModel.DisplayInvoiceNumber;
                }
                else if (response.StatusCode == Status.Failed && manualInvoiceviewModel.IsTaxServiceFailure && manualInvoiceviewModel.WaitingForAction != (int)WaitingAction.AvalaraTax)
                {
                    await newsfeedDomain.SetDdtToInvoiceWaitingForTaxesNewsfeed(userContext, manualInvoiceviewModel.OrderId, manualInvoiceviewModel.InvoiceId, ddtNumber, manualInvoiceviewModel.InvoiceHeaderId);
                    await SetAvalaraFailureFlag(userContext.Id, manualInvoiceviewModel, preferInvoiceTypeId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "CreateInvoiceFromDropTicketWithBol", ex.Message, ex);
            }
            return response;
        }

        public bool HasRequiredInvoiceImages(ManualInvoiceViewModel viewModel)
        {
            bool hasImages = false;
            try
            {
                var invoice = Context.DataContext.Invoices.Where(t => t.Id == viewModel.InvoiceId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                          .Select(t => new { t.IsBolImageReq, t.IsDropImageReq, t.IsSignatureReq, IsApprovalWorkflowEnabled = t.Order.FuelRequest.Job.IsApprovalWorkflowEnabled }).FirstOrDefault();
                hasImages = SetInvoiceRequiredImages(invoice, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "HasRequiredInvoiceImages", ex.Message, ex);
            }

            return hasImages;
        }

        public async Task<StatusViewModel> CreateInvoicesFromSplitLoadDropTicketsWFB(UserContext userContext, ManualInvoiceViewModel viewModel)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var splitLoadDdts = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == viewModel.SplitLoadChainId
                                                                                    && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                                    && t.IsActive).Select(t => new { t.InvoiceTypeId, t.WaitingFor, t.Id, InvoiceFtlDetail = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail).FirstOrDefault() }).ToListAsync();

                var ddtsToConvert = splitLoadDdts.Where(t => (t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual
                                                                                    || t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) && t.WaitingFor == (int)WaitingAction.BolDetails
                                                                                   ).ToList();

                foreach (var splitDdt in ddtsToConvert)
                {
                    var manualInvoiceViewModel = await GetManualSplitInvoiceForEditAsync(splitDdt.Id);
                    manualInvoiceViewModel.userId = userContext.Id;
                    // update common details
                    manualInvoiceViewModel.TrackableScheduleId = viewModel.TrackableScheduleId;
                    manualInvoiceViewModel.DriverId = viewModel.DriverId;
                    var existingBol = splitLoadDdts.FirstOrDefault(t => t.WaitingFor != (int)WaitingAction.BolDetails && t.InvoiceFtlDetail != null);
                    if (existingBol != null)
                    {
                        manualInvoiceViewModel.BolDetails = existingBol.InvoiceFtlDetail.ToViewModel();
                    }

                    response = await CreateInvoiceFromDropTicketWithBol(manualInvoiceViewModel, userContext);
                }

                var splitInvoices = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == viewModel.SplitLoadChainId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && (t.InvoiceTypeId == (int)InvoiceType.Manual || t.InvoiceTypeId == (int)InvoiceType.MobileApp)).ToListAsync();
                if (splitInvoices.Count == splitLoadDdts.Count)
                {
                    BillingStatementDomain statementDomain = new BillingStatementDomain(this);
                    await statementDomain.GeneateBillingStatementForSplitLoadInvoice(splitInvoices, viewModel.TimeZoneName, userContext.CompanyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "CreateInvoiceFromDropTicket", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateInvoiceFromSplitLoadDropTicket(UserContext userContext, InvoiceDetailViewModel viewModel)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var splitLoadDdts = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == viewModel.SplitLoadChainId
                                                                                    && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                                    && t.IsActive).Select(t => new { t.InvoiceTypeId, t.WaitingFor, t.Id, t.InvoiceHeaderId }).ToListAsync();
                var ddtsToConvert = splitLoadDdts.Where(t => (t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual
                                                                                    || t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                                                                                    && t.WaitingFor == (int)WaitingAction.Nothing).ToList();
                foreach (var splitDdt in ddtsToConvert)
                {
                    response = await CreateInvoiceFromDropTicket(userContext, splitDdt.Id, splitDdt.InvoiceHeaderId, userContext.Id);
                }
                var splitInvoices = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == viewModel.SplitLoadChainId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && t.InvoiceTypeId == (int)InvoiceType.Manual).ToListAsync();
                if (splitInvoices.Count == splitLoadDdts.Count)
                {
                    BillingStatementDomain statementDomain = new BillingStatementDomain(this);
                    await statementDomain.GeneateBillingStatementForSplitLoadInvoice(splitInvoices, viewModel.Invoice.TimeZoneName, userContext.CompanyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "CreateInvoiceFromDropTicket", ex.Message, ex);
            }
            return response;
        }



        public async Task<StatusViewModel> CreateInvoiceFromDropTicket(UserContext userContext, int dropTicketId, int dropTicketHeaderId, int userId, int? buyerId = null, bool isWaitingForApproval = false, bool isDdtToInvoiceApproved = false)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var dropTicketDetails = await GetManualInvoiceForEditAsync(dropTicketId);
                int waitingForAction = dropTicketDetails.WaitingForAction;
                dropTicketDetails.IsTaxServiceFailure = false;
                dropTicketDetails.userId = userId;
                dropTicketDetails.UpdatedBy = userId;
                dropTicketDetails.InvoiceNumber.Id = 0;
                dropTicketDetails.InvoiceId = 0;
                dropTicketDetails.IsInvoiceFromDropTicket = true;
                if (dropTicketDetails.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual)
                {
                    dropTicketDetails.InvoiceTypeId = (int)InvoiceType.Manual;
                }
                else if (dropTicketDetails.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    dropTicketDetails.InvoiceTypeId = (int)InvoiceType.MobileApp;
                }
                var preferInvoiceTypeId = dropTicketDetails.InvoiceTypeId;
                var orderId = dropTicketDetails.OrderId;
                var ddtNumber = dropTicketDetails.InvoiceNumber.Number;
                response = await GenerateInvoiceFromDDT(dropTicketDetails, dropTicketId);
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                if (response.StatusCode == Status.Success)
                {
                    if (isWaitingForApproval)
                    {
                        await newsfeedDomain.SetApprovedDDTNewsfeed(userContext.CompanyId, (int)buyerId, dropTicketDetails, ddtNumber, dropTicketId, dropTicketHeaderId);
                    }
                    else if (waitingForAction == (int)WaitingAction.UpdatedPrice)
                    {
                        await newsfeedDomain.SetSystemDdtToInvoiceCreatedForUpdatedPriceNewsfeed(userContext.CompanyId, dropTicketDetails, ddtNumber, dropTicketId, dropTicketHeaderId);
                    }
                    else if (waitingForAction == (int)WaitingAction.AvalaraTax)
                    {
                        await newsfeedDomain.SetInvoiceGeneratedEstablishConnectionWithAvalaraNewsfeed(userContext.CompanyId, dropTicketDetails, ddtNumber, dropTicketId, dropTicketHeaderId);
                    }
                    else if (!isDdtToInvoiceApproved)
                    {
                        await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed(userContext, dropTicketDetails, ddtNumber);
                    }
                    response.EntityNumber = response.StatusMessage + " - " + dropTicketDetails.DisplayInvoiceNumber;
                }
                else if (response.StatusCode == Status.Failed && dropTicketDetails.IsTaxServiceFailure && waitingForAction != (int)WaitingAction.AvalaraTax)
                {
                    await newsfeedDomain.SetDdtToInvoiceWaitingForTaxesNewsfeed(userContext, orderId, dropTicketId, ddtNumber, dropTicketHeaderId);
                    await SetAvalaraFailureFlag(userContext.Id, dropTicketDetails, preferInvoiceTypeId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "CreateInvoiceFromDropTicket", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ApproveEddtAndCreateInvoice(UserContext userContext, int exceptionId, ExceptionResolution resolutionType, decimal quantity, int statusId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var eDdt = await Context.DataContext.InvoiceExceptions.Where(t => t.GeneratedExceptionId == exceptionId && t.IsActive)
                                       .Select(t => new
                                       {
                                           BrokeredChainId = t.Invoice.BrokeredChainId,
                                           InvoiceId = t.Invoice.Id,
                                           InvoiceHeaderId = t.Invoice.InvoiceHeaderId,
                                           SplitLoadChainId = t.Invoice.InvoiceXAdditionalDetail.SplitLoadChainId,
                                           ExceptionTypeId = t.ExceptionTypeId,
                                           DefaultInvoiceType = t.Invoice.SupplierPreferredInvoiceTypeId,
                                           UserId = t.Invoice.UpdatedBy
                                       }).FirstOrDefaultAsync();

                if (eDdt == null)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessageNotFound;
                    return response;
                }

                var exceptionIds = new List<int> { exceptionId };
                var resolvedExceptions = new List<InvoiceExceptionViewModel>();
                var invoiceId = eDdt.InvoiceId;
                var exceptionDetails = new InvoiceExceptionViewModel()
                {
                    InvoiceId = eDdt.InvoiceId,
                    ExceptionTypeId = eDdt.ExceptionTypeId,
                    StatusId = statusId,
                    GeneratedExceptionId = exceptionId
                };
                resolvedExceptions.Add(exceptionDetails);
                if (!string.IsNullOrEmpty(eDdt.BrokeredChainId))
                {
                    var brokerInvoices = Context.DataContext.Invoices.Where(t => t.BrokeredChainId == eDdt.BrokeredChainId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t => new { t.Id, t.OrderId }).OrderBy(t => t.Id).ToList();
                    var brokerInvoiceIds = brokerInvoices.Select(t => t.Id).ToList();
                    var brokerInvoiceExceptions = await Context.DataContext.InvoiceExceptions.Where(t => brokerInvoiceIds.Contains(t.InvoiceId) && t.IsActive && t.ExceptionTypeId == eDdt.ExceptionTypeId)
                                        .Select(t => new { t.InvoiceId, t.GeneratedExceptionId, t.ExceptionTypeId }).ToListAsync();
                    exceptionIds.AddRange(brokerInvoiceExceptions.Select(t => t.GeneratedExceptionId).ToList());
                    foreach (var brokerException in brokerInvoiceExceptions)
                    {
                        var brokerExceptionModel = new InvoiceExceptionViewModel()
                        {
                            InvoiceId = brokerException.InvoiceId,
                            ExceptionTypeId = brokerException.ExceptionTypeId,
                            StatusId = statusId,
                            GeneratedExceptionId = brokerException.GeneratedExceptionId
                        };
                        resolvedExceptions.Add(brokerExceptionModel);
                    }
                    if (brokerInvoices.Any())
                    {
                        invoiceId = brokerInvoices[0].Id;
                    }
                }
                var exceptionDomain = new ExceptionDomain();
                var apiApprovalResult = await exceptionDomain.ApproveException(exceptionIds, resolutionType, statusId);
                var isPendingException = false;
                if (apiApprovalResult)
                {
                    await UpdateExceptionStatus(resolvedExceptions);
                    if (string.IsNullOrWhiteSpace(eDdt.SplitLoadChainId))
                    {
                        if (eDdt.ExceptionTypeId == (int)ExceptionType.DeliveredQuantityVariance)
                        {
                            isPendingException = await CheckPendingExceptions(exceptionDetails, eDdt.InvoiceHeaderId);
                        }
                        if (userContext.Id == (int)SystemUser.System)
                        {
                            userContext.Id = eDdt.UserId;
                        }

                        response = await UpdateDDTDropQuantity(invoiceId, quantity, userContext.Id);
                        if (isPendingException)
                        {
                            if (response.StatusCode == Status.Success)
                            {
                                response.StatusMessage = Resource.successExceptionApproved;
                            }
                        }
                        else if (eDdt.DefaultInvoiceType == (int)InvoiceType.Manual || eDdt.DefaultInvoiceType == (int)InvoiceType.MobileApp)
                        {
                            if (response.StatusCode == 0)
                            {
                                response.StatusMessage = Resource.successMessageInvoiceCreatedFromEddt;
                            }
                        }
                        else
                        {
                            if (response.StatusCode == 0)
                            {
                                response.StatusMessage = Resource.successMessageDdtCreatedFromEddt;
                            }
                        }
                    }
                    else
                    {
                        var splitLoadInvoiceDomain = new SplitLoadInvoiceDomain(this);
                        response = await splitLoadInvoiceDomain.CreateInvoicesFromSplitLoadEddts(userContext, eDdt.SplitLoadChainId, quantity);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "ApproveEddtAndCreateInvoice", ex.Message, ex);
            }
            return response;
        }
        private async Task<bool> CheckPendingExceptions(InvoiceExceptionViewModel model, int invoiceHeaderId)
        {
            var pendingExceptionIds = await Context.DataContext.InvoiceExceptions.Where(t => t.Invoice.InvoiceHeaderId == invoiceHeaderId
                                                                                && t.InvoiceId != model.InvoiceId && t.ExceptionTypeId == model.ExceptionTypeId
                                                                                && t.StatusId == (int)ExceptionStatus.Raised
                                                                                && t.IsActive).Select(t => t.GeneratedExceptionId).ToListAsync();
            var apiPendingException = false;
            if (pendingExceptionIds != null && pendingExceptionIds.Any())
            {
                apiPendingException = true;
            }
            return apiPendingException;
        }

        public async Task<StatusViewModel> UpdateDDTDropQuantity(int invoiceId, decimal quantity, int userId)
        {
            var response = new StatusViewModel();
            try
            {
                var invoiceModel = await GetOriginalInvoiceDetails(invoiceId);
                var approvedInvoice = invoiceModel.Drops.FirstOrDefault(t => t.InvoiceId == invoiceId);
                if (approvedInvoice != null)
                {
                    approvedInvoice.ActualDropQuantity = quantity;
                }

                UserContext user = new UserContext() { Id = userId };

                response = await new ConsolidatedInvoiceDomain(this).ApproveExceptionDropTicket(user, invoiceModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "UpdateDDTDropQuantity", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> DiscardEddt(UserContext userContext, int exceptionId, ExceptionResolution resolutionType, ExceptionStatus exceptionStatus)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var eDdt = await Context.DataContext.InvoiceExceptions.Where(t => t.GeneratedExceptionId == exceptionId && t.IsActive)
                                       .Select(t => new
                                       {
                                           BrokeredChainId = t.Invoice.BrokeredChainId,
                                           InvoiceId = t.Invoice.Id,
                                           InvoiceHeaderId = t.Invoice.InvoiceHeaderId,
                                           SplitLoadChainId = t.Invoice.InvoiceXAdditionalDetail.SplitLoadChainId,
                                           ExceptionTypeId = t.ExceptionTypeId,
                                           DefaultInvoiceType = t.Invoice.SupplierPreferredInvoiceTypeId,
                                           UserId = t.Invoice.UpdatedBy
                                       }).FirstOrDefaultAsync();

                if (eDdt == null)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessageNotFound;
                    return response;
                }

                var exceptionIds = new List<int> { exceptionId };
                var resolvedExceptions = new List<InvoiceExceptionViewModel>();
                var invoiceId = eDdt.InvoiceId;
                var exceptionDetails = new InvoiceExceptionViewModel()
                {
                    InvoiceId = eDdt.InvoiceId,
                    ExceptionTypeId = eDdt.ExceptionTypeId,
                    StatusId = (int)exceptionStatus,
                    GeneratedExceptionId = exceptionId
                };
                resolvedExceptions.Add(exceptionDetails);
                if (!string.IsNullOrEmpty(eDdt.BrokeredChainId))
                {
                    var brokerInvoices = Context.DataContext.Invoices.Where(t => t.BrokeredChainId == eDdt.BrokeredChainId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t => new { t.Id, t.OrderId }).OrderBy(t => t.Id).ToList();
                    var brokerInvoiceIds = brokerInvoices.Select(t => t.Id).ToList();
                    var brokerInvoiceExceptions = await Context.DataContext.InvoiceExceptions.Where(t => brokerInvoiceIds.Contains(t.InvoiceId) && t.IsActive && t.ExceptionTypeId == eDdt.ExceptionTypeId)
                                        .Select(t => new { t.InvoiceId, t.GeneratedExceptionId, t.ExceptionTypeId }).ToListAsync();
                    exceptionIds.AddRange(brokerInvoiceExceptions.Select(t => t.GeneratedExceptionId).ToList());
                    foreach (var brokerException in brokerInvoiceExceptions)
                    {
                        var brokerExceptionModel = new InvoiceExceptionViewModel()
                        {
                            InvoiceId = brokerException.InvoiceId,
                            ExceptionTypeId = brokerException.ExceptionTypeId,
                            StatusId = (int)exceptionStatus,
                            GeneratedExceptionId = brokerException.GeneratedExceptionId
                        };
                        resolvedExceptions.Add(brokerExceptionModel);
                    }
                    if (brokerInvoices.Any())
                    {
                        invoiceId = brokerInvoices[0].Id;
                    }
                }
                var exceptionDomain = new ExceptionDomain();
                var apiApprovalResult = await exceptionDomain.ApproveException(exceptionIds, resolutionType, (int)exceptionStatus);
                if (apiApprovalResult)
                {
                    // update invoice exceptions status
                    response = await UpdateInvoiceStatusToDiscard(resolvedExceptions);
                    if (response.StatusCode == Status.Success)
                        response.StatusMessage = Resource.messageDiscardExceptionInvoice;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "DiscardEddt", ex.Message, ex);
            }
            return response;
        }

        public void UpdateDropQuantityByPrePostDip(InvoiceViewModelNew model)
        {
            try
            {
                var assets = model.Drops.SelectMany(t => t.Assets).Where(t1 => t1.IsOfflineMode && t1.PreDip > 0 && t1.PostDip > 0 && t1.TankScaleMeasurement > (int)TankScaleMeasurement.None);
                List<int> jobXassets = assets.Select(t => t.JobXAssetId).ToList();
                var jobXassetIds = Context.DataContext.JobXAssets.Where(t => jobXassets.Contains(t.Id)).Select(t => new { t.JobId, t.Id, t.AssetId }).ToList();
                var apiInput = new List<DropQuantityByPrePostDipRequestModel>();
                foreach (var asset in assets)
                {
                    var assetInfo = jobXassetIds.FirstOrDefault(t => t.Id == asset.JobXAssetId);
                    if (assetInfo != null)
                    {
                        var input = new DropQuantityByPrePostDipRequestModel()
                        {
                            JobxAssetId = assetInfo.Id,
                            JobId = assetInfo.JobId,
                            TankId = assetInfo.AssetId,
                            PreDipValue = asset.PreDip.Value,
                            PostDipValue = asset.PostDip.Value,
                            ScaleMeasurement = (int)asset.TankScaleMeasurement
                        };
                        apiInput.Add(input);
                    }
                }
                if (apiInput.Any())
                {
                    var apiResponse = Task.Run(() => new FreightServiceDomain(this).GetDropQuantityByPrePostDip(apiInput)).Result;
                    if (apiResponse != null && apiResponse.Any(t => t.StatusCode != Status.Failed))
                    {
                        foreach (var drop in model.Drops)
                        {
                            foreach (var assetDrop in drop.Assets)
                            {
                                var tankResponse = apiResponse.FirstOrDefault(t => t.JobxAssetId == assetDrop.JobXAssetId && t.StatusCode != Status.Failed);
                                if (tankResponse != null)
                                {
                                    drop.ActualDropQuantity -= assetDrop.DropGallons.Value;
                                    assetDrop.DropGallons = tankResponse.DropQuantity;
                                    drop.ActualDropQuantity += assetDrop.DropGallons.Value;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "UpdateDropQuantityByPrePostDip", ex.Message, ex);
            }
        }

        public async Task<StatusViewModel> ConvertDdtToInvoiceMissingDelivery(int invoiceId, int userId, List<AssignOrderMissingDeliveryModel> assignOrderMissingDelivery = null)
        {
            var response = new StatusViewModel();
            try
            {
                var invoiceModel = await GetOriginalInvoiceDetails(invoiceId);
                invoiceModel.IsMissingDeliveryDDTConversion = true;

                if (assignOrderMissingDelivery != null)
                {
                    foreach (var drop in invoiceModel.Drops)
                    {
                        var assignOrder = assignOrderMissingDelivery.FirstOrDefault(t => t.NewOrderId == drop.OrderId);
                        if (assignOrder != null)
                        {
                            if (assignOrder.IsReassignDifferentJob)
                            {
                                drop.Assets = new List<AssetDropViewModel>();
                                drop.IsReassignDifferentJob = true;
                            }
                            drop.OldOrderId = assignOrder.OldOrderId;
                        }
                    }
                }

                UserContext user = new UserContext() { Id = userId };
                response = await new ConsolidatedInvoiceDomain(this).ApproveExceptionDropTicket(user, invoiceModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "UpdateDDTOrder", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateDDTStatus(int invoiceHeaderId, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            try
            {
                var invoices = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId).ToListAsync();
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var invoice in invoices)
                        {
                            invoice.WaitingFor = (int)WaitingAction.Nothing;
                            invoice.UpdatedBy = userContext.Id;
                            invoice.UpdatedDate = DateTimeOffset.Now;
                            await Context.CommitAsync();
                            transaction.Commit();
                        }
                        response.StatusCode = Status.Success;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "UpdateDDTStatus", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageFailed;

            }
            return response;
        }

        public async Task<StatusViewModel> CreateInvoicesFromEddt(UserContext userContext, EddtApprovalParameterModel model)
        {
            var response = new StatusViewModel(Status.Failed);
            var dropTicketDetails = await GetManualInvoiceForEditAsync(model.InvoiceId);
            
            dropTicketDetails.IsTaxServiceFailure = false;
            dropTicketDetails.userId = model.CreatedBy;
            dropTicketDetails.UpdatedBy = model.UpdatedBy;
            dropTicketDetails.InvoiceNumber.Id = 0;
            dropTicketDetails.InvoiceId = 0;
            dropTicketDetails.IsInvoiceFromDropTicket = true;
            dropTicketDetails.WaitingForAction = (int)WaitingAction.Nothing;
            dropTicketDetails.FuelDropped = model.ApprovedQuantity;
            if (dropTicketDetails.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual)
            {
                dropTicketDetails.InvoiceTypeId = (int)InvoiceType.Manual;
            }
            else if (dropTicketDetails.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                dropTicketDetails.InvoiceTypeId = (int)InvoiceType.MobileApp;
            }            
            
            var ddtNumber = dropTicketDetails.DisplayInvoiceNumber;
            response = await GenerateInvoiceFromDDT(dropTicketDetails, model.InvoiceId);
            if (response.StatusCode == Status.Success)
            {
                response.StatusMessage = GetEddtStatusMessage(dropTicketDetails.InvoiceTypeId, dropTicketDetails.WaitingForAction, dropTicketDetails.DDTConversionReason);
                var newsfeedModel = new EddtToInvoiceCreatedNewsfeedModel()
                {
                    BuyerCompanyId = dropTicketDetails.BuyerCompanyId,
                    SupplierCompanyId = dropTicketDetails.SupplierCompanyId,
                    OrderId = dropTicketDetails.OrderId,
                    InvoiceId = response.EntityId,
                    DisplayInvoiceNumber = dropTicketDetails.DisplayInvoiceNumber,
                    TimeZoneName = dropTicketDetails.TimeZoneName,
                    IsDigitalDropTicket = dropTicketDetails.IsDigitalDropTicket(),
                    JobId = dropTicketDetails.JobId,
                    InvoiceHeaderId = dropTicketDetails.InvoiceHeaderId,
                    WaitingFor = dropTicketDetails.WaitingForAction
                };
                await SetEddtNewsfeeds(userContext, newsfeedModel, ddtNumber);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateInvoiceFromApprovedDropTicket(UserContext userContext, int dropTicketId, int dropTicketHeaderId, int userId, int? buyerId = null, bool isWaitingForApproval = false, bool isDdtToInvoiceApproved = false)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var dropTicketDetails = await GetManualInvoiceForEditAsync(dropTicketId);
                int waitingForAction = dropTicketDetails.WaitingForAction;
                dropTicketDetails.WaitingForAction = (int)WaitingAction.Nothing;
                dropTicketDetails.IsTaxServiceFailure = false;
                dropTicketDetails.userId = userId;
                dropTicketDetails.UpdatedBy = userId;
                dropTicketDetails.InvoiceNumber.Id = 0;
                dropTicketDetails.InvoiceId = 0;
                dropTicketDetails.IsInvoiceFromDropTicket = true;
                if (dropTicketDetails.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual)
                {
                    dropTicketDetails.InvoiceTypeId = (int)InvoiceType.Manual;
                }
                else if (dropTicketDetails.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    dropTicketDetails.InvoiceTypeId = (int)InvoiceType.MobileApp;
                }
                var preferInvoiceTypeId = dropTicketDetails.InvoiceTypeId;
                var orderId = dropTicketDetails.OrderId;
                var ddtNumber = dropTicketDetails.InvoiceNumber.Number;
                response = await GenerateInvoiceFromDDT(dropTicketDetails, dropTicketId);
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                if (response.StatusCode == Status.Success)
                {
                    if (isWaitingForApproval)
                    {
                        await newsfeedDomain.SetApprovedDDTNewsfeed(userContext.CompanyId, (int)buyerId, dropTicketDetails, ddtNumber, dropTicketId, dropTicketHeaderId);
                    }
                    else if (waitingForAction == (int)WaitingAction.UpdatedPrice)
                    {
                        await newsfeedDomain.SetSystemDdtToInvoiceCreatedForUpdatedPriceNewsfeed(userContext.CompanyId, dropTicketDetails, ddtNumber, dropTicketId, dropTicketHeaderId);
                    }
                    else if (waitingForAction == (int)WaitingAction.AvalaraTax)
                    {
                        await newsfeedDomain.SetInvoiceGeneratedEstablishConnectionWithAvalaraNewsfeed(userContext.CompanyId, dropTicketDetails, ddtNumber, dropTicketId, dropTicketHeaderId);
                    }
                    else if (!isDdtToInvoiceApproved)
                    {
                        await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed(userContext, dropTicketDetails, ddtNumber);
                    }
                }
                else if (response.StatusCode == Status.Failed && dropTicketDetails.IsTaxServiceFailure && waitingForAction != (int)WaitingAction.AvalaraTax)
                {
                    await newsfeedDomain.SetDdtToInvoiceWaitingForTaxesNewsfeed(userContext, orderId, dropTicketId, ddtNumber, dropTicketHeaderId);
                    await SetAvalaraFailureFlag(userContext.Id, dropTicketDetails, preferInvoiceTypeId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "CreateInvoiceFromDropTicket", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateInvoiceFromDropTicketWaitingForTax(UserContext userContext, int dropTicketId, int dropTicketHeaderId, int userId, int? buyerId = null, bool isWaitingForApproval = false, bool isDdtToInvoiceApproved = false)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var dropTicketDetails = await GetManualInvoiceForEditAsync(dropTicketId);
                int waitingForAction = dropTicketDetails.WaitingForAction;
                dropTicketDetails.WaitingForAction = (int)WaitingAction.Nothing;
                dropTicketDetails.IsTaxServiceFailure = false;
                dropTicketDetails.userId = userId;
                dropTicketDetails.UpdatedBy = userId;
                dropTicketDetails.InvoiceNumber.Id = 0;
                dropTicketDetails.InvoiceId = 0;
                dropTicketDetails.IsInvoiceFromDropTicket = true;
                if (dropTicketDetails.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual)
                {
                    dropTicketDetails.InvoiceTypeId = (int)InvoiceType.Manual;
                }
                else if (dropTicketDetails.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    dropTicketDetails.InvoiceTypeId = (int)InvoiceType.MobileApp;
                }
                var preferInvoiceTypeId = dropTicketDetails.InvoiceTypeId;
                var orderId = dropTicketDetails.OrderId;
                var ddtNumber = dropTicketDetails.InvoiceNumber.Number;
                response = await GenerateInvoiceFromDDT(dropTicketDetails, dropTicketId);
                if (!string.IsNullOrEmpty(dropTicketDetails.SplitLoadChainId))
                {
                    await CheckForSplitLoadInvoiceAndGenerateStatement(dropTicketDetails.SplitLoadChainId, userContext.CompanyId, dropTicketDetails.TimeZoneName);
                }
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                if (response.StatusCode == Status.Success)
                {
                    if (isWaitingForApproval)
                    {
                        await newsfeedDomain.SetApprovedDDTNewsfeed(userContext.CompanyId, (int)buyerId, dropTicketDetails, ddtNumber, dropTicketId, dropTicketHeaderId);
                    }
                    else if (waitingForAction == (int)WaitingAction.UpdatedPrice)
                    {
                        await newsfeedDomain.SetSystemDdtToInvoiceCreatedForUpdatedPriceNewsfeed(userContext.CompanyId, dropTicketDetails, ddtNumber, dropTicketId, dropTicketHeaderId);
                    }
                    else if (waitingForAction == (int)WaitingAction.AvalaraTax)
                    {
                        await newsfeedDomain.SetInvoiceGeneratedEstablishConnectionWithAvalaraNewsfeed(userContext.CompanyId, dropTicketDetails, ddtNumber, dropTicketId, dropTicketHeaderId);
                    }
                    else if (!isDdtToInvoiceApproved)
                    {
                        await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed(userContext, dropTicketDetails, ddtNumber);
                    }
                }
                else if (response.StatusCode == Status.Failed && dropTicketDetails.IsTaxServiceFailure && waitingForAction != (int)WaitingAction.AvalaraTax)
                {
                    await newsfeedDomain.SetDdtToInvoiceWaitingForTaxesNewsfeed(userContext, orderId, dropTicketId, ddtNumber, dropTicketHeaderId);
                    await SetAvalaraFailureFlag(userContext.Id, dropTicketDetails, preferInvoiceTypeId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "CreateInvoiceFromDropTicket", ex.Message, ex);
            }
            return response;
        }

        public async Task SetAvalaraFailureFlag(int userId, ManualInvoiceViewModel viewModel, int? originalInvnvoiceTypeId)
        {
            var isEditInvoice = viewModel.IsTaxServiceFailure ? false : viewModel.InvoiceNumber.Id > 0;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var dropTicket = Context.DataContext.Invoices.FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == viewModel.InvoiceNumberId && t.IsActive && t.InvoiceVersionStatusId == 1);
                    if (dropTicket != null)
                    {
                        if (dropTicket.WaitingFor != (int)WaitingAction.AvalaraTax)
                        {
                            dropTicket.WaitingFor = (int)WaitingAction.AvalaraTax;
                            if (!isEditInvoice && originalInvnvoiceTypeId.HasValue)
                            {
                                dropTicket.SupplierPreferredInvoiceTypeId = originalInvnvoiceTypeId;
                            }
                            dropTicket.UpdatedBy = userId;
                            dropTicket.UpdatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName);
                            Context.DataContext.Entry(dropTicket).State = EntityState.Modified;
                            await Context.CommitAsync();
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "SetAvalaraFailureFlag", ex.Message, ex);
                }
            }
        }

        public async Task<StatusViewModel> CreateInvoiceFromDropTicketForNonStandardFuel(UserContext userContext, int userId, ManualInvoiceViewModel viewModel)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                viewModel.userId = userId;
                viewModel.UpdatedBy = userId;
                viewModel.InvoiceNumber.Id = 0;
                viewModel.InvoiceId = 0;
                viewModel.IsInvoiceFromDropTicket = true;
                if (viewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual)
                {
                    viewModel.InvoiceTypeId = (int)InvoiceType.Manual;
                }
                else if (viewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    viewModel.InvoiceTypeId = (int)InvoiceType.MobileApp;
                }
                response = await GenerateInvoiceFromDDT(viewModel, viewModel.ConversionDDTId);
                var ddtNumber = string.Empty;
                var invoice = await Context.DataContext.Invoices.Where(t => t.Id == viewModel.ConversionDDTId)
                                    .Select(t => new { t.InvoiceHeader.InvoiceNumber.Number }).SingleOrDefaultAsync();
                if (invoice != null)
                {
                    ddtNumber = invoice.Number;
                }

                if (response.StatusCode == Status.Success)
                {
                    NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                    await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed(userContext, viewModel, ddtNumber);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "CreateInvoiceFromDropTicketForNonStandardFuel", ex.Message, ex);
            }
            return response;
        }

        public async Task<ManualInvoiceViewModel> GetManualInvoiceAsync(int orderId)
        {
            var response = new ManualInvoiceViewModel();
            try
            {
                string SupplierEmail = string.Empty;
                string SupplierPhone = string.Empty;
                string SupplierName = string.Empty;
                string BuyerCompanyName = string.Empty;
                decimal avgGallonsPercentagePerDelivery = 0;
                HelperDomain helperDomain = new HelperDomain(this);
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open);
                //// check if order is closed order and gallon delivery is 0 percent drop/0 gallons drop
                if (order == null)
                {
                    order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId &&
                                                                                       t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed);
                    avgGallonsPercentagePerDelivery = helperDomain.GetAverageFuelDropPercentagePerOrder(order);
                }

                if (order != null || avgGallonsPercentagePerDelivery <= 0)
                {
                    var user = order.FuelRequest.User;
                    if (order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest)
                    {
                        var counterOffer = order.FuelRequest.CounterOffers.FirstOrDefault(t => t.BuyerStatus == (int)CounterOfferStatus.Accepted);
                        if (counterOffer != null)
                        {
                            user = Context.DataContext.Users.SingleOrDefault(t => t.Id == counterOffer.BuyerId);
                        }
                    }
                    SupplierEmail = user.Email;
                    SupplierPhone = user.PhoneNumber;
                    SupplierName = $"{user.FirstName} {user.LastName}";
                    BuyerCompanyName = user.Company.Name;

                    response = new ManualInvoiceViewModel(Status.Success)
                    {
                        JobId = order.FuelRequest.JobId,
                        OrderId = orderId,
                        FuelRemaining = ((order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity) - order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons)),
                        AssetTracked = order.FuelRequest.Job.JobBudget.IsAssetTracked,
                        Assets = order
                                    .FuelRequest
                                    .Job
                                    .JobXAssets.Where(t => t.Asset.Type == (int)AssetType.Asset).OrderByDescending(t => t.Id).GroupBy(t => new { t.Asset.Id })
                                    .Select(g => g.First())
                                    .Select(t => new AssetDropViewModel(Status.Success)
                                    {
                                        AssetName = t.Asset.Name,
                                        JobXAssetId = t.Id,
                                        OrderId = orderId
                                    }).ToList(),
                        FuelRequestDeliveryStartDate = order.FuelRequest.FuelRequestDetail.StartDate.Date,
                        OrderAcceptDate = order.AcceptedDate.Date,
                        PoNumber = order.PoNumber,
                        OrderTypeId = order.FuelRequest.OrderTypeId,
                        SupplierEmail = SupplierEmail,
                        SupplierPhone = SupplierPhone,
                        SupplierName = SupplierName,
                        IsPublicRequest = order.FuelRequest.IsPublicRequest,
                        SupplierQualifications = order.FuelRequest.MstSupplierQualifications.Select(t => t.Id).ToList(),
                        FuelId = order.FuelRequest.FuelTypeId,
                        FuelType = helperDomain.GetProductName(order.FuelRequest.MstProduct),
                        OrderTotal = order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity,
                        PaymentTermId = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentTermId,
                        NetDays = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).NetDays,
                        IsMulitpleDelivery = order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries,
                        IsBackDatedJob = order.FuelRequest.Job.IsBackdatedJob,
                        BuyerCompanyName = BuyerCompanyName,
                        BuyerCompanyId = order.BuyerCompanyId,
                        ProductDescription = order.FuelRequest.FuelDescription,
                        FuelRequestId = order.FuelRequest.Id,
                        AvgGallonsPercentagePerDelivery = avgGallonsPercentagePerDelivery,
                        Currency = order.FuelRequest.Currency,
                        UoM = order.FuelRequest.UoM,
                        PricingType = (PricingType)order.FuelRequest.PricingTypeId,
                        QuantityTypeId = order.FuelRequest.QuantityTypeId,
                        IsFTL = order.IsFTL,
                        PricingCodeId = order.FuelRequest.FuelRequestPricingDetail.PricingCodeId,
                        IsDriverSignatureEnabled = order.FuelRequest.Job.SignatureEnabled == false ? order.SignatureEnabled : order.FuelRequest.Job.SignatureEnabled,
                        PaymentMethod = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentMethod,
                        BolDetails = new BolDetailViewModel() { TypeofFuel = order.FuelRequest.MstProduct.ProductDisplayGroupId, CityGroupTerminalId = order.CityGroupTerminalId ?? 0, TerminalId = order.TerminalId ?? order.FuelRequest.TerminalId ?? 0, TerminalName = order.TerminalId.HasValue ? order.MstExternalTerminal.Name : (order.FuelRequest.TerminalId.HasValue ? order.FuelRequest.MstExternalTerminal.Name : string.Empty) }
                    };

                    if (order.FuelRequest.FuelRequestFees != null && order.FuelRequest.FuelRequestFees.Any())
                    {
                        response.FuelDeliveryDetails.FuelFees.FuelRequestFees = order.FuelRequest.FuelRequestFees.ToFeesViewModel();
                        response.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
                    }

                    if (order.CityGroupTerminalId.HasValue && order.CityGroupTerminalId.Value > 0)
                    {
                        response.BolDetails.CityGroupTerminalName = Context.DataContext.MstExternalTerminals.SingleOrDefault(t => t.Id == order.CityGroupTerminalId).Name;

                        var terminal = ContextFactory.Current.GetDomain<HelperDomain>().GetExternalTerminal(order.CityGroupTerminalId.Value);
                        if (terminal != null)
                        {
                            response.BolDetails.CityGroupTerminalName = $"{terminal.Name}, {terminal.StateCode}";
                        }
                    }

                    if (order.ExternalBrokerOrderDetail != null)
                    {
                        response.ExternalBrokerId = order.ExternalBrokerId ?? 0;
                        response.ExternalBrokeredOrder.CustomerId = order.ExternalBrokerId ?? 0;
                        response.ExternalBrokeredOrder = order.ExternalBrokerOrderDetail.ToViewModel();
                        response.ExternalBrokeredOrder.BrokeredOrderFee = order.FuelRequest.FuelRequestFees.ToExternalBrokerViewModel();
                        response.IsThirdPartyHardwareUsed = true;
                    }

                    if (order.ExternalBrokerBuySellDetail != null)
                    {
                        response.ExternalBrokerId = order.ExternalBrokerId ?? 0;
                        response.ExternalBrokeredOrder.CustomerId = order.ExternalBrokerId ?? 0;
                        response.IsBuySellOrder = true;
                    }

                    if (response.AssetTracked && response.Assets.Count == 0)
                    {
                        response.AssetTracked = false;
                    }

                    if (order.FuelRequest.FuelRequestDetail.EndDate.HasValue)
                    {
                        response.MaxDropDate = order.FuelRequest.FuelRequestDetail.EndDate.Value.Date;
                    }
                    else if (order.FuelRequest.Job.EndDate.HasValue)
                    {
                        response.MaxDropDate = order.FuelRequest.Job.EndDate.Value.Date;
                    }

                    if (order.OrderTaxDetails != null && order.OrderTaxDetails.Count > 0)
                    {
                        response.Taxes = order.OrderTaxDetails.Where(t => t.IsActive).ToTaxViewModel();
                        response.IsOtherFuelTypeTaxesGiven = response.Taxes.Count > 0;
                    }

                    response.FuelDeliveryDetails.FuelFees.TruckLoadType = TruckLoadTypes.LessTruckLoad;

                    if (order.IsFTL)
                    {
                        response.Assets.Clear();
                        response.AssetTracked = false;
                        response.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach((t) => t.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad);
                        response.FuelDeliveryDetails.FuelFees.TruckLoadType = TruckLoadTypes.FullTruckLoad;

                        if (order.FuelRequest.FuelRequestPricingDetail != null && order.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId.HasValue)
                            response.QuantityIndicatorTypeId = order.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId ?? (int)QuantityIndicatorTypes.Net;
                    }

                    if (order.OrderAdditionalDetail != null)
                    {
                        response.SupplierAllowance = order.OrderAdditionalDetail.Allowance.HasValue && order.OrderAdditionalDetail.Allowance.Value > 0
                                                                ? order.OrderAdditionalDetail.Allowance : null;
                        response.IsBolRequired = order.OrderAdditionalDetail.IsDriverToUpdateBOL;
                        if (order.OrderAdditionalDetail.Carrier != null)
                            response.BolDetails.Carrier = order.OrderAdditionalDetail.Carrier.Name;
                    }
                    SetFuelSurchargeDetails(response, order);
                    response.DropAddress = new DropAddressViewModel();
                    response.DropAddress.State.Id = order.FuelRequest.Job.StateId;
                    response.DropAddress.State.Code = order.FuelRequest.Job.MstState.Code;
                    if (order.FuelRequest.FreightOnBoardTypeId.HasValue)
                        response.IsVariousFobOrigin = order.FuelRequest.FreightOnBoardTypeId == (int)FreightOnBoardTypes.Terminal && order.FuelRequest.Job.LocationType == JobLocationTypes.Various;

                    if (order.OrderAdditionalDetail != null)
                    {
                        response.Notes = order.OrderAdditionalDetail.Notes;
                    }

                    response.FuelDeliveryDetails.FuelFees.Currency = order.FuelRequest.Currency;
                    response.FuelDeliveryDetails.FuelFees.UoM = order.FuelRequest.UoM;
                    response.ExternalBrokeredOrder.BrokeredOrderFee.Currency = order.FuelRequest.Currency;
                    response.ExternalBrokeredOrder.BrokeredOrderFee.UoM = order.FuelRequest.UoM;
                    response.PickUpAddress = new DropAddressViewModel() { State = new StateViewModel() { Id = 1, Code = Resource.lblDummy } };
                    var pickupAddress = order.FuelDispatchLocations.FirstOrDefault(t => t.DeliveryScheduleId == null && t.TrackableScheduleId == null && t.TerminalId == null && t.LocationType == (int)LocationType.PickUp && t.IsActive);
                    if (pickupAddress != null)
                    {
                        response.PickUpAddress = pickupAddress.ToPickUpLocation();
                    }
                    response.MaxDropDate = (response.MaxDropDate == null || response.MaxDropDate.Value > DateTimeOffset.Now.Date) ? DateTimeOffset.Now.Date : response.MaxDropDate;
                    response.InvoiceTypeId = order.DefaultInvoiceType;
                    response = response.CorrectValues();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetManualInvoiceAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<AssetDropViewModel>> GetAssignedAssetsAsync(int orderId)
        {
            var response = new List<AssetDropViewModel>();
            try
            {
                var orderDetails = await Context.DataContext.Orders.Where(t => t.Id == orderId && t.IsActive)
                                            .Select(t => new
                                            {
                                                FuelType = t.FuelRequest.MstProduct.ProductTypeId,
                                                JobXAssets = t.FuelRequest.Job.JobXAssets,
                                                IsMarine = t.FuelRequest.Job.IsMarine,
                                                BuyerCompanyId = t.BuyerCompanyId,
                                                AssignedJobBuyerCompanyId = t.FuelRequest.Job.CompanyId
                                            })
                                            .FirstOrDefaultAsync();

                var mappedToProductTypeIds = new List<int>();
                mappedToProductTypeIds.Add(orderDetails.FuelType);
                var productTypeMappings = Context.DataContext.ProductTypeCompatibilityMappings.Where(t => t.ProductTypeId == orderDetails.FuelType)
                                                                .Select(t => t.MappedToProductTypeId).ToList();
                if (productTypeMappings != null)
                {
                    mappedToProductTypeIds.AddRange(productTypeMappings);
                }

                if (orderDetails.IsMarine)
                {
                    response = Context.DataContext.JobXAssets.Where(t => t.Asset.IsMarine && t.Asset.IsActive && t.Asset.Type == (int)AssetType.Vessle
                                        && t.OrderId == orderId && t.RemovedBy == null)
                               .OrderByDescending(t => t.Id).GroupBy(t => new { t.Id }).Select(g => g.OrderByDescending(t => t.Id).FirstOrDefault())
                               .Select(t => new AssetDropViewModel()
                               {
                                   Id = t.Id,
                                   AssetName = t.Asset.Name,
                                   JobXAssetId = t.Id,// set asset Id in case of marine asset
                                   OrderId = orderId,
                                   AssetType = t.Asset.Type,
                                   TankScaleMeasurement = TankScaleMeasurement.None
                               }).ToList();
                }
                else
                {
                    response = orderDetails.JobXAssets.Where(t => t.Asset.IsActive
                                            && (t.Asset.Type == (int)AssetType.Asset || (t.Asset.FuelType.HasValue && mappedToProductTypeIds.Contains(t.Asset.FuelType.Value))))
                               .OrderByDescending(t => t.Id).GroupBy(t => new { t.Asset.Id }).Select(g => g.OrderByDescending(t => t.Id).FirstOrDefault())
                               .Select(t => new AssetDropViewModel()
                               {
                                   Id = t.Asset.Id,
                                   AssetName = t.Asset.Name,
                                   JobXAssetId = t.Id,
                                   OrderId = orderId,
                                   AssetType = t.Asset.Type
                                   //TankScaleMeasurement = t.AssetDrops.
                                   //                        Where(t1 => t1.OrderId == orderId && t1.JobXAssetId == t.Id).
                                   //                        Select(t1 => t1.TankScaleMeasurement).FirstOrDefault()
                               }).ToList();
                    var jobXAssetId = response.Select(t => t.JobXAssetId).ToList();
                    var assetAndScaleMeasurement = Context.DataContext.AssetDrops.Where(t => t.OrderId == orderId && t.IsActive &&
                                                           jobXAssetId.Contains(t.JobXAssetId)).
                                                           Select(t1 => new DropdownDisplayExtendedId()
                                                           {
                                                               Id = t1.JobXAssetId,
                                                               CodeId = (int)t1.TankScaleMeasurement
                                                           }).ToList();
                    foreach (var item in response)
                    {
                        var tankScaleMeasurement = assetAndScaleMeasurement.Where(t => t.Id == item.JobXAssetId).Select(t => t.CodeId).FirstOrDefault();
                        item.TankScaleMeasurement = tankScaleMeasurement != 0 ? (TankScaleMeasurement)tankScaleMeasurement : TankScaleMeasurement.None;
                    }
                }
                if (response != null && response.Any(t => t.AssetType == (int)AssetType.Tank))
                {
                    var assetIds = response.Where(t => t.AssetType == (int)AssetType.Tank)
                                   .Select(t => t.Id).Distinct().ToList();
                    if (assetIds != null && assetIds.Any())
                    {
                        List<TankDetailViewModel> tankListInfo = await new FreightServiceDomain(this).GetTankList(assetIds);
                        if (tankListInfo != null && tankListInfo.Any())
                        {
                            foreach (var item in tankListInfo)
                            {
                                var tank = response.Find(t => t.Id == item.AssetId);
                                if (tank != null)
                                {
                                    tank.TankMakeModel = item.TankMakeModel;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetAssignedAssetsAsync", ex.Message, ex);
            }
            return response;
        }

        private static void SetFuelSurchargeDetails(ManualInvoiceViewModel response, Order order)
        {
            var freightPricingMethod = order.OrderAdditionalDetail == null ? FreightPricingMethod.Manual : order.OrderAdditionalDetail.FreightPricingMethod;
            if (freightPricingMethod == FreightPricingMethod.Manual)
            {
                response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee = order.FuelRequest.FuelRequestFees.ToSurchargeFreightFeesViewModel();
                if (order.OrderAdditionalDetail != null)
                {
                    response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsSurchargeApplicable = order.OrderAdditionalDetail.IsFuelSurcharge;
                }
                if (response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsSurchargeApplicable)
                {
                    response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsFeeByDistance = response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.DeliveryFeeByQuantity.Any();
                    response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargePricingType = order.OrderAdditionalDetail.FuelSurchagePricingType.HasValue ? (FuelSurchagePricingType)order.OrderAdditionalDetail.FuelSurchagePricingType.Value : FuelSurchagePricingType.Unknown;
                    response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeProductType = order.FuelRequest.MstProduct.ProductTypeId.GetFuelSurchargeProductType();
                    if (response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeProductType != SurchargeProductTypes.Unknown)
                    {
                        response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeFreightCost = response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.Fee;

                        if (response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsFeeByDistance)
                            response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeFreightCost = response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.DeliveryFeeByQuantity
                                                                                                                 .Where(t => t.MinQuantity >= 0).Select(t => t.Fee).FirstOrDefault();

                        response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeEiaPrice = new EIAPriceUpdateDomain()
                                                                            .GetEIAPrice(response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargePricingType,
                                                                            response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeProductType,
                                                                            response.DeliveryDate.Date);
                        var fsc = new FuelSurchargeDomain().GetFuelSurchargeRecordForEaiPrice(response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeEiaPrice, order.AcceptedCompanyId, order.BuyerCompanyId, response.DeliveryDate, response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeProductType);
                        if (fsc != null)
                        {
                            response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargePercentage = fsc.FuelSurchargeStartPercentage;
                            response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeTableRangeStart = fsc.PriceRangeStartValue;
                            response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeTableRangeEnd = fsc.PriceRangeEndValue;
                        }
                    }
                }
            }
            else if (freightPricingMethod == FreightPricingMethod.Auto)
            {
                SetAutoFuelSurchargeDetails(response, order);
            }
        }

        public async Task<ManualInvoiceViewModel> GetManualSplitInvoiceAsync(int orderId, string splitLoadChainId)
        {
            var response = new ManualInvoiceViewModel();
            try
            {
               
                var orderDetails = await Context.DataContext.Orders.Where(t => t.Id == orderId && t.OrderXStatuses.Any(t1 => t1.IsActive && (t1.StatusId == (int)OrderStatus.Open || t1.StatusId == (int)OrderStatus.Closed)))
                                .Select(t => new
                                {
                                    t.OrderXStatuses,
                                    t.FuelRequestId,
                                    FuelRemaining = ((t.BrokeredMaxQuantity ?? t.FuelRequest.MaxQuantity) - t.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t1.IsActive && !t1.IsBuyPriceInvoice).Sum(t1 => t1.DroppedGallons)),
                                    IsFTL = t.IsFTL,
                                    TypeofFuel = t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                    t.OrderTaxDetails,
                                    t.FuelRequest.FuelRequestFees,
                                    t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                    FuelRequestDeliveryStartDate = t.FuelRequest.FuelRequestDetail.StartDate,
                                    t.AcceptedDate,
                                    FuelRequestEndDate = t.FuelRequest.FuelRequestDetail.EndDate,
                                    JobEndDate = t.FuelRequest.Job.EndDate,
                                    JobStateId = t.FuelRequest.Job.StateId,
                                    JobStateCode = t.FuelRequest.Job.MstState.Code,
                                    t.FuelRequest.Currency,
                                    t.FuelRequest.UoM,
                                    t.DefaultInvoiceType,
                                    IsDriverSignatureEnabled = t.FuelRequest.Job.SignatureEnabled == false ? t.SignatureEnabled : t.FuelRequest.Job.SignatureEnabled,
                                    SplitDroppedGallons = t.Invoices.Where(t1 => t1.IsActive && t1.InvoiceXAdditionalDetail.SplitLoadChainId == splitLoadChainId && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                        .Sum(i => i.DroppedGallons),
                                    PrevSplitChain = t.Invoices.Where(t1 => t1.InvoiceXAdditionalDetail.SplitLoadChainId == splitLoadChainId && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t1 =>
                                              new
                                              {
                                                  t1.InvoiceXAdditionalDetail.SplitLoadChainId,
                                                  t1.InvoiceXAdditionalDetail.SplitLoadSequence,
                                                  t1.TrackableScheduleId,
                                                  t1.DriverId,
                                                  DeliveryDate = t1.DropStartDate,
                                                  BolDetailsData = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault(),
                                                  BolImage = new ImageViewModel { Id = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault() == null || !t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault().ImageId.HasValue ? 0 : t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault().ImageId.Value },
                                              }).OrderByDescending(t1 => t1.SplitLoadSequence).FirstOrDefault()
                                })
                                .FirstOrDefaultAsync();

                if (orderDetails != null)
                {
                    response = new ManualInvoiceViewModel(Status.Success)
                    {
                        OrderId = orderId,
                        FuelRequestId = orderDetails.FuelRequestId,
                        FuelRemaining = orderDetails.FuelRemaining,
                        IsMulitpleDelivery = orderDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries,
                        IsFTL = orderDetails.IsFTL,
                        InvoiceTypeId = orderDetails.DefaultInvoiceType,
                        SplitLoadChainId = orderDetails.PrevSplitChain?.SplitLoadChainId,
                        SplitLoadSequence = orderDetails.PrevSplitChain?.SplitLoadSequence,
                        TrackableScheduleId = orderDetails.PrevSplitChain?.TrackableScheduleId,
                        DriverId = orderDetails.PrevSplitChain?.DriverId,
                        DeliveryDate = orderDetails.PrevSplitChain != null ? orderDetails.PrevSplitChain.DeliveryDate.DateTime : DateTimeOffset.Now.Date,
                        Currency = orderDetails.Currency,
                        UoM = orderDetails.UoM,
                        BolImage = orderDetails.PrevSplitChain?.BolImage,
                        TotalSplitDroppedGallons = orderDetails.SplitDroppedGallons,
                        IsDriverSignatureEnabled = orderDetails.IsDriverSignatureEnabled,
                        FuelRequestDeliveryStartDate = orderDetails.FuelRequestDeliveryStartDate.Date,
                        OrderAcceptDate = orderDetails.AcceptedDate.Date,
                    };
                    if (orderDetails.PrevSplitChain != null)
                    {
                        response.BolDetails = orderDetails.PrevSplitChain.BolDetailsData.ToViewModel();
                    }
                    response.BolDetails.TypeofFuel = orderDetails.TypeofFuel;
                    if (orderDetails.FuelRequestFees != null && orderDetails.FuelRequestFees.Any())
                    {
                        response.FuelDeliveryDetails.FuelFees.FuelRequestFees = orderDetails.FuelRequestFees.ToFeesViewModel();
                        response.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
                    }

                    if (orderDetails.OrderTaxDetails != null && orderDetails.OrderTaxDetails.Count > 0)
                    {
                        response.Taxes = orderDetails.OrderTaxDetails.Where(t => t.IsActive).ToTaxViewModel();
                        response.IsOtherFuelTypeTaxesGiven = response.Taxes.Count > 0;
                    }
                    if (orderDetails.FuelRequestEndDate.HasValue)
                    {
                        response.MaxDropDate = orderDetails.FuelRequestEndDate.Value.Date;
                    }
                    else if (orderDetails.JobEndDate.HasValue)
                    {
                        response.MaxDropDate = orderDetails.JobEndDate.Value.Date;
                    }

                    //for FTL
                    response.FuelDeliveryDetails.FuelFees.TruckLoadType = TruckLoadTypes.FullTruckLoad;
                    response.Assets.Clear();
                    response.AssetTracked = false;
                    response.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach((t) => t.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad);
                    response.DropAddress = new DropAddressViewModel();
                    response.DropAddress.State.Id = orderDetails.JobStateId;
                    response.DropAddress.State.Code = orderDetails.JobStateCode;

                    response.FuelDeliveryDetails.FuelFees.Currency = orderDetails.Currency;
                    response.FuelDeliveryDetails.FuelFees.UoM = orderDetails.UoM;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetManualSplitInvoiceAsync", ex.Message, ex);
            }
            return response;
        }

        private static void SetAutoFuelSurchargeDetails(ManualInvoiceViewModel response, Order order)
        {
            response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee = order.FuelRequest.FuelRequestFees.ToSurchargeFreightFeesViewModel();
            if (order.OrderAdditionalDetail != null)
            {
                response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsSurchargeApplicable = order.OrderAdditionalDetail.IsFuelSurcharge;
                response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsFreightCostApplicable = order.OrderAdditionalDetail.IsFreightCost;
            }
            if (response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsSurchargeApplicable)
            {
                response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsFeeByDistance = response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.DeliveryFeeByQuantity.Any();
                if (order.OrderAdditionalDetail.FuelSurchargeTableId.HasValue && order.OrderAdditionalDetail.FuelSurchargeTableId.Value > 0)
                {
                    var fuelSurchargeIndexId = order.OrderAdditionalDetail.FuelSurchargeTableId.Value;
                    var autoResponse = Task.Run(() => ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetFuelSurchargeTableForAutoFreightMethod(fuelSurchargeIndexId)).Result;

                    response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeEiaPrice = new EIAPriceUpdateDomain().GetEIAPrice(autoResponse.SurchargePricingType, autoResponse.SurchargeProductType, response.DeliveryDate.Date, autoResponse.FuelSurchageArea);
                    var fsc = new FuelSurchargeDomain().GetFuelSurchargeRecordForEaiPriceForAutoFreightMethod(response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeEiaPrice, fuelSurchargeIndexId);
                    if (fsc != null)
                    {
                        response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargePricingType = autoResponse.SurchargePricingType;
                        response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeProductType = autoResponse.SurchargeProductType;

                        if (response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeProductType != SurchargeProductTypes.Unknown)
                        {
                            response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargePercentage = fsc.FuelSurchargeStartPercentage;
                            response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeTableRangeStart = fsc.PriceRangeStartValue;
                            response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeTableRangeEnd = fsc.PriceRangeEndValue;
                        }
                    }
                }

                if (response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsFreightCostApplicable)
                {
                    var freightCostInput = new FreightCostInputViewModel();
                    freightCostInput.FreightRateRuleId = order.OrderAdditionalDetail.FreightRateRuleId.Value;
                    freightCostInput.OrderId = order.OrderAdditionalDetail.OrderId;
                    freightCostInput.SupplierId = order.AcceptedCompanyId;
                    response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeFreightCost = Task.Run(() => ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFreightCostForInvoice(freightCostInput)).Result;
                }
            }
        }

        public async Task<ManualInvoiceViewModel> GetManualSplitInvoiceForEditAsync(int id)
        {
            var response = new ManualInvoiceViewModel();
            try
            {
                var invoiceDetails = await Context.DataContext.Invoices.Where(t => t.Id == id)
                                    .Select(t => new
                                    {
                                        OrderId = t.OrderId,
                                        FuelRemaining = ((t.Order.BrokeredMaxQuantity ?? t.Order.FuelRequest.MaxQuantity) - t.Order.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Sum(t1 => t1.DroppedGallons)),
                                        t.DroppedGallons,
                                        DeliveryDate = t.DropEndDate,
                                        StartTime = t.DropStartDate,
                                        EndTime = t.DropEndDate,
                                        FuelId = t.Order.FuelRequest.FuelTypeId,
                                        ProductTypeId = t.Order.FuelRequest.MstProduct.ProductTypeId,
                                        FuelRequestDeliveryStartDate = t.Order.FuelRequest.FuelRequestDetail.StartDate,
                                        OrderAcceptDate = t.Order.AcceptedDate,
                                        PoNumber = t.PoNumber,
                                        OrderTypeId = t.Order.FuelRequest.OrderTypeId,
                                        DispatchLocation = t.InvoiceDispatchLocation.OrderByDescending(t1 => t1.Id).FirstOrDefault(t1 => t1.LocationType == (int)LocationType.Drop),
                                        t.InvoiceXAdditionalDetail,
                                        JobStateId = t.Order.FuelRequest.Job.StateId,
                                        FuelRequestEndDate = t.Order.FuelRequest.FuelRequestDetail.EndDate,
                                        JobEndDate = t.Order.FuelRequest.Job.EndDate,
                                        t.Currency,
                                        t.UoM,
                                        t.FuelRequestFees,
                                        InvoiceImage = new ImageViewModel { Id = t.ImageId ?? 0 },
                                        BolImage = new ImageViewModel { Id = t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault() == null || !t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault().ImageId.HasValue ? 0 : t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault().ImageId.Value },
                                        TaxAffidavitImage = new ImageViewModel { Id = t.InvoiceXAdditionalDetail.TaxAffidavitImageId ?? 0 },
                                        BDNImage = new ImageViewModel { Id = t.InvoiceXAdditionalDetail.BDNImageId ?? 0 },
                                        CoastGuardInspectionImage = new ImageViewModel { Id = t.InvoiceXAdditionalDetail.CoastGuardInspectionImageId ?? 0 },
                                        InspectionRequestVoucherImage = new ImageViewModel { Id = t.InvoiceXAdditionalDetail.InspectionRequestVoucherImageId ?? 0 },
                                        t.Order.IsFTL,
                                        t.Order.FuelRequest.PricingTypeId,
                                        t.Order.FuelRequestId,
                                        t.PaymentDueDate,
                                        t.WaitingFor,
                                        DriverId = t.DriverId,
                                        IsMulitpleDelivery = t.Order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries,
                                        Signature = t.Signaure != null ? new ImageViewModel { Id = t.Signaure.ImageId ?? 0 } : null,
                                        t.TrackableScheduleId,
                                        OrderTotal = t.Order.BrokeredMaxQuantity ?? t.Order.FuelRequest.MaxQuantity,
                                        PaymentTermId = t.PaymentTermId,
                                        NetDays = t.NetDays,
                                        InvoiceNumberId = t.InvoiceHeader.InvoiceNumberId,
                                        InvoiceNumber = t.InvoiceHeader.InvoiceNumber,
                                        StatusId = t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId,
                                        InvoiceTypeId = t.InvoiceTypeId,
                                        t.CreatedBy,
                                        t.Order.FuelRequest.Job.TimeZoneName,
                                        InvoiceFtlDetail = t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault(),
                                        TerminalId = t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).Select(t1 => t1.TerminalId).First() ?? t.Order.TerminalId ?? t.Order.FuelRequest.TerminalId ?? 0,
                                        CityGroupTerminalId = t.Order.CityGroupTerminalId ?? 0,
                                        TerminalName = t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).Select(t1 => t1.TerminalId).First().HasValue ? t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).Select(t1 => t1.SiteName).First() : (t.Order.TerminalId.HasValue ? t.Order.MstExternalTerminal.Name : (t.Order.FuelRequest.TerminalId.HasValue ? t.Order.FuelRequest.MstExternalTerminal.Name : string.Empty)),
                                        SplitDroppedGallons = t.Order.Invoices.Where(t1 => t1.IsActive && t1.InvoiceXAdditionalDetail.SplitLoadChainId == t.InvoiceXAdditionalDetail.SplitLoadChainId && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                        .Sum(i => i.DroppedGallons)
                                    })
                                    .FirstOrDefaultAsync();
                if (invoiceDetails != null)
                {

                    response = new ManualInvoiceViewModel(Status.Success)
                    {
                        OrderId = invoiceDetails.OrderId ?? 0,
                        FuelRemaining = invoiceDetails.FuelRemaining,
                        FuelDropped = invoiceDetails.DroppedGallons,
                        DeliveryDate = invoiceDetails.DeliveryDate.Date,
                        StartTime = invoiceDetails.StartTime.DateTime.ToString(Resource.constFormat12HourTime2),
                        EndTime = invoiceDetails.EndTime.DateTime.ToString(Resource.constFormat12HourTime2),
                        InvoiceId = id,
                        FuelId = invoiceDetails.FuelId,
                        FuelRequestDeliveryStartDate = invoiceDetails.FuelRequestDeliveryStartDate.Date,
                        OrderAcceptDate = invoiceDetails.OrderAcceptDate.Date,
                        PoNumber = invoiceDetails.PoNumber,
                        OrderTypeId = invoiceDetails.OrderTypeId,
                        OrderTotal = invoiceDetails.OrderTotal,
                        PaymentTermId = invoiceDetails.PaymentTermId,
                        NetDays = invoiceDetails.NetDays,
                        InvoiceNumberId = invoiceDetails.InvoiceNumberId,
                        InvoiceNumber = invoiceDetails.InvoiceNumber.ToViewModel(null),
                        StatusId = invoiceDetails.StatusId,
                        InvoiceTypeId = invoiceDetails.InvoiceTypeId,
                        InvoiceImage = invoiceDetails.InvoiceImage,
                        BolImage = invoiceDetails.BolImage,
                        SignatureImage = invoiceDetails.Signature,
                        TaxAffidavitImage = invoiceDetails.TaxAffidavitImage,
                        BDNImage = invoiceDetails.BDNImage,
                        CoastGuardInspectionImage = invoiceDetails.CoastGuardInspectionImage,
                        InspectionRequestVoucherImage = invoiceDetails.InspectionRequestVoucherImage,
                        DriverId = invoiceDetails.DriverId,
                        IsMulitpleDelivery = invoiceDetails.IsMulitpleDelivery,
                        ConversionDDTId = id,
                        PaymentDueDate = invoiceDetails.PaymentDueDate.ToString(Resource.constFormatDate),
                        FuelRequestId = invoiceDetails.FuelRequestId,
                        UoM = invoiceDetails.UoM,
                        Currency = invoiceDetails.Currency,
                        PricingType = (PricingType)invoiceDetails.PricingTypeId,
                        IsFTL = invoiceDetails.IsFTL,
                        SplitLoadChainId = invoiceDetails.InvoiceXAdditionalDetail.SplitLoadChainId,
                        SplitLoadSequence = invoiceDetails.InvoiceXAdditionalDetail.SplitLoadSequence,
                        TotalSplitDroppedGallons = invoiceDetails.SplitDroppedGallons,
                        BolDetails = invoiceDetails.InvoiceFtlDetail.ToViewModel(),
                        TimeZoneName = invoiceDetails.TimeZoneName,
                        userId = invoiceDetails.CreatedBy,
                        WaitingForAction = invoiceDetails.WaitingFor,
                        CreationMethod = invoiceDetails.InvoiceXAdditionalDetail.CreationMethod
                    };

                    response.FuelDeliveryDetails.FuelFees.TruckLoadType = TruckLoadTypes.LessTruckLoad;
                    response.FuelDeliveryDetails.FuelFees = GetInvoiceFuelFees(invoiceDetails.FuelRequestFees, invoiceDetails.DroppedGallons);
                    response.FuelDeliveryDetails.FuelFees.DiscountLineItems.ForEach(t => t.InvoiceId = id);
                    SetSurchargeDetails(invoiceDetails.InvoiceXAdditionalDetail, response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee, invoiceDetails.ProductTypeId, response.FuelDropped ?? 0);
                    response.BolDetails.TerminalName = invoiceDetails.TerminalName;
                    if (response.BolDetails.CityGroupTerminalId > 0)
                    {
                        response.BolDetails.CityGroupTerminalName = Context.DataContext.MstExternalTerminals.SingleOrDefault(t => t.Id == response.BolDetails.CityGroupTerminalId).Name;
                    }

                    response.TrackableScheduleId = invoiceDetails.TrackableScheduleId;

                    if (invoiceDetails.FuelRequestEndDate.HasValue)
                    {
                        response.MaxDropDate = invoiceDetails.FuelRequestEndDate.Value.Date;
                    }
                    else if (invoiceDetails.JobEndDate.HasValue)
                    {
                        response.MaxDropDate = invoiceDetails.JobEndDate.Value.Date;
                    }

                    //Ftl
                    response.FuelDeliveryDetails.FuelFees.TruckLoadType = TruckLoadTypes.FullTruckLoad;
                    if (invoiceDetails.DispatchLocation != null)
                    {
                        response.DropAddress = invoiceDetails.DispatchLocation.ToDispatchAddress(response.DropAddress);
                    }
                    else
                    {
                        response.DropAddress = invoiceDetails.InvoiceXAdditionalDetail.ToDispatchAddress();
                        response.DropAddress.State.Id = invoiceDetails.JobStateId;
                    }
                    response.AssetTracked = false;
                    response.Assets.Clear();
                    response.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach((t) => t.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad);

                    response.FuelDeliveryDetails.FuelFees.Currency = invoiceDetails.Currency;
                    response.FuelDeliveryDetails.FuelFees.UoM = invoiceDetails.UoM;
                    response.MaxDropDate = (response.MaxDropDate == null || response.MaxDropDate.Value > DateTimeOffset.Now.Date) ? DateTimeOffset.Now.Date : response.MaxDropDate;
                    response = response.CorrectValues();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetManualSplitInvoiceForEditAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task SetInvoiceAdditionDetails(InvoiceViewModel invoiceViewModel, int previousInvoiceId, int orderId, bool isSellInvoice = false)
        {
            StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
            var additionalDetail = await storedProcedureDomain.GetInvoiceAdditionalDetailAsync(orderId, previousInvoiceId, isSellInvoice);
            if (additionalDetail != null)
            {
                invoiceViewModel.AdditionalDetail = additionalDetail.Clone(invoiceViewModel.AdditionalDetail);
            }
            if (invoiceViewModel.TaxDetails != null && invoiceViewModel.TaxDetails.IsTrueFillTax)
            {
                invoiceViewModel.AdditionalDetail.CustomAttributeViewModel.IsTrueFillTax = invoiceViewModel.TaxDetails.IsTrueFillTax;
                invoiceViewModel.AdditionalDetail.CustomAttribute = invoiceViewModel.AdditionalDetail.CustomAttributeViewModel.ToString();
            }
        }

        public async Task<InvoiceViewModelNew> GetOriginalInvoiceDetails(int invoiceId, int companyId = 0)
        {
            InvoiceViewModelNew invoiceModel = new InvoiceViewModelNew();
            try
            {
                var invoices = await GetInvoiceDetails(invoiceId);
                invoiceModel.OriginalInvoiceHeaderId = invoiceId;
                var invoice = invoices.FirstOrDefault();

                SetInvoiceHeaderDetails(invoice, invoiceModel);
                SetBolDetails(invoiceModel, invoices);
                SetCustomerDetails(invoice, invoiceModel);
                SetDropDetails(invoiceModel, invoices);
                invoiceModel.IsBadgeMandatory = IsBadgeNumberMandatory(invoice.OrderId, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetOriginalInvoiceDetails", ex.Message, ex);
            }
            return invoiceModel;
        }

        public async Task<ManualInvoiceViewModel> GetManualInvoiceForEditAsync(int id)
        {
            var response = new ManualInvoiceViewModel();
            try
            {
                HelperDomain helperDomain = new HelperDomain(this);
                string SupplierEmail = string.Empty;
                string SupplierPhone = string.Empty;
                string SupplierName = string.Empty;
                string BuyerCompanyName = string.Empty;

                var invoice = await Context.DataContext.Invoices.SingleOrDefaultAsync(t => t.Id == id);
                if (invoice != null)
                {
                    if (invoice.TaxDetails != null)
                        invoice.TaxDetails = invoice.TaxDetails.Where(w => w.TaxExemptionInd != Resource.TaxExemptionEnable).ToList();
                    if (invoice.Order != null)
                    {
                        var user = invoice.Order.FuelRequest.User;
                        if (invoice.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest)
                        {
                            var counterOffer = invoice.Order.FuelRequest.CounterOffers.FirstOrDefault(t => t.BuyerStatus == (int)CounterOfferStatus.Accepted);
                            if (counterOffer != null)
                            {
                                user = Context.DataContext.Users.SingleOrDefault(t => t.Id == counterOffer.BuyerId);
                            }
                        }
                        SupplierEmail = user.Email;
                        SupplierPhone = user.PhoneNumber;
                        SupplierName = $"{user.FirstName} {user.LastName}";
                        BuyerCompanyName = user.Company.Name;
                    }
                    var bolDetails = invoice.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault();
                    response = new ManualInvoiceViewModel(Status.Success)
                    {
                        OrderId = invoice.Order.Id,
                        InvoiceHeaderId = invoice.InvoiceHeaderId,
                        JobId = invoice.Order.FuelRequest.JobId,
                        FuelRemaining = ((invoice.Order.BrokeredMaxQuantity ?? invoice.Order.FuelRequest.MaxQuantity) - invoice.Order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Sum(t => t.DroppedGallons)),
                        FuelDropped = invoice.DroppedGallons,
                        OriginalDroppedGallons = invoice.DroppedGallons, // used in cumulation qty update
                        DeliveryDate = invoice.DropStartDate.Date,
                        DropEndDate = invoice.DropEndDate.Date,
                        IsOverWaterDelivery = invoice.IsOverWaterDelivery,
                        IsWethosingDelivery = invoice.IsWetHosingDelivery,
                        StartTime = invoice.DropStartDate.DateTime.ToString(Resource.constFormat12HourTime2),
                        EndTime = invoice.DropEndDate.DateTime.ToString(Resource.constFormat12HourTime2),
                        InvoiceId = invoice.Id,
                        FuelId = invoice.Order.FuelRequest.FuelTypeId,
                        AssetTracked = invoice.Order.FuelRequest.Job.JobBudget.IsAssetTracked,
                        Assets = GetAssetDropDetails(invoice),
                        FuelRequestDeliveryStartDate = invoice.Order.FuelRequest.FuelRequestDetail.StartDate.Date,
                        OrderAcceptDate = invoice.Order.AcceptedDate.Date,
                        PoNumber = invoice.PoNumber,
                        OrderTypeId = invoice.Order.FuelRequest.OrderTypeId,
                        SupplierEmail = SupplierEmail,
                        SupplierPhone = SupplierPhone,
                        SupplierName = SupplierName,
                        TerminalId = bolDetails != null && bolDetails.TerminalId.HasValue ? bolDetails.TerminalId.Value : (invoice.Order.TerminalId ?? invoice.Order.FuelRequest.TerminalId ?? 0),
                        CityGroupTerminalId = invoice.Order.CityGroupTerminalId ?? 0,
                        TerminalName = GetTerminalName(invoice),
                        IsPublicRequest = invoice.Order.FuelRequest.IsPublicRequest,
                        SupplierQualifications = invoice.Order.FuelRequest.MstSupplierQualifications.Select(t => t.Id).ToList(),
                        FuelType = helperDomain.GetProductName(invoice.Order.FuelRequest.MstProduct),
                        OrderTotal = invoice.Order.BrokeredMaxQuantity ?? invoice.Order.FuelRequest.MaxQuantity,
                        PaymentTermId = invoice.PaymentTermId,
                        NetDays = invoice.NetDays,
                        InvoiceNumberId = invoice.InvoiceHeader.InvoiceNumberId,
                        InvoiceNumber = invoice.InvoiceHeader.InvoiceNumber.ToViewModel(),
                        StatusId = invoice.InvoiceXInvoiceStatusDetails.SingleOrDefault(t => t.IsActive).StatusId,
                        InvoiceTypeId = invoice.InvoiceTypeId,
                        TotalInvoiceAmount = helperDomain.GetInvoiceAmount(invoice),
                        CreatedDate = invoice.CreatedDate,
                        InvoiceImage = GetImageViewModel(invoice),
                        BolImage = GetBolImageViewModel(invoice),
                        AdditionalImage = GetAdditionalImageViewModel(invoice),
                        TaxAffidavitImage = GetTaxAffidavitImageViewModel(invoice),
                        BDNImage = GetBDNImageViewModel(invoice),
                        CoastGuardInspectionImage = GetCoastGuardInspectionImageViewModel(invoice),
                        InspectionRequestVoucherImage = GetInspectionRequestVoucherImageViewModel(invoice),
                        SignatureImage = GetSignatureImage(invoice.Signaure),
                        AssetDropImages = GetAssetDropImages(invoice),
                        StateTax = invoice.StateTax,
                        FederalTax = invoice.FedTax,
                        SalesTax = invoice.SalesTax,
                        DriverId = invoice.DriverId,
                        IsBolRequired = invoice.Order.OrderAdditionalDetail != null && invoice.Order.OrderAdditionalDetail.IsDriverToUpdateBOL,
                        IsMulitpleDelivery = invoice.Order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries,
                        IsBackDatedJob = invoice.Order.FuelRequest.Job.IsBackdatedJob,
                        TypeofFuel = invoice.Order.FuelRequest.MstProduct.ProductDisplayGroupId,
                        BuyerCompanyName = BuyerCompanyName,
                        ConversionDDTId = id,
                        PaymentDueDate = invoice.PaymentDueDate.ToString(Resource.constFormatDate),
                        ProductDescription = invoice.Order.FuelRequest.FuelDescription,
                        FuelRequestId = invoice.Order.FuelRequest.Id,
                        CsvFilePath = invoice.FilePath,
                        WaitingForAction = invoice.WaitingFor,
                        IsBuyPriceInvoice = invoice.IsBuyPriceInvoice,
                        UoM = invoice.UoM,
                        Currency = invoice.Currency,
                        PricingType = (PricingType)invoice.Order.FuelRequest.PricingTypeId,
                        SignatureId = invoice.SignatureId,
                        DisplayInvoiceNumber = invoice.DisplayInvoiceNumber,
                        QuantityTypeId = invoice.Order.FuelRequest.QuantityTypeId,
                        TimeZoneName = invoice.Order.FuelRequest.Job.TimeZoneName,
                        BrokeredChainId = invoice.BrokeredChainId,
                        QbInvoiceNumber = invoice.QbInvoiceNumber,
                        PricingCodeId = invoice.Order.FuelRequest.FuelRequestPricingDetail.PricingCodeId,
                        IsFTL = invoice.Order != null ? invoice.Order.IsFTL : false,
                        SplitLoadChainId = invoice.InvoiceXAdditionalDetail.SplitLoadChainId,
                        SplitLoadSequence = invoice.InvoiceXAdditionalDetail.SplitLoadSequence,
                        BolDetails = invoice.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault().ToViewModel(),
                        TotalSplitDroppedGallons = invoice.Order.Invoices.Where(t1 => t1.IsActive && t1.InvoiceXAdditionalDetail.SplitLoadChainId == invoice.InvoiceXAdditionalDetail.SplitLoadChainId && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                                                        .Sum(i => i.DroppedGallons),
                        PaymentMethod = invoice.InvoiceXAdditionalDetail.PaymentMethod,
                        DropTicketNumber = invoice.InvoiceXAdditionalDetail.DropTicketNumber,
                        TruckNumber = invoice.InvoiceXAdditionalDetail.TruckNumber,
                        CreationMethod = invoice.InvoiceXAdditionalDetail.CreationMethod,
                        OriginalInvoiceId = invoice.InvoiceXAdditionalDetail.OriginalInvoiceId,
                        InvoiceCreationPricePerGallon = bolDetails != null ? bolDetails.PricePerGallon : 0,
                        IsDriverSignatureEnabled = invoice.IsSignatureReq,
                        DDTConversionReason = invoice.DDTConversionReason,
                        SupplierAllowance = invoice.InvoiceXAdditionalDetail.SupplierAllowance,
                        userId = invoice.CreatedBy,
                        IsPrePostDipRequired = invoice.Order.FuelRequest.FuelRequestDetail.IsPrePostDipRequired,
                        IsMarineLocation = invoice.Order.FuelRequest.Job.IsMarine,
                        Gravity = invoice.Gravity,
                        ConvertedQuantity = invoice.ConvertedQuantity,
                        JobCountryId = invoice.Order.FuelRequest.Job.CountryId,// used for UI ,
                        ConvertionFactor = invoice.ConversionFactor,
                        BDRDetail = invoice.BDRDetail != null ? invoice.BDRDetail.ToViewModel() : new BDRDetailsModel(),
                        IsSuppressPricingOrder = invoice.Order.OrderAdditionalDetail != null ? invoice.Order.OrderAdditionalDetail.IsSupressPricingEnabled : false,
                        DeliveryLevelPO = invoice.TrackableSchedule != null ? invoice.TrackableSchedule.DeliveryLevelPO : string.Empty
                    };
                    var lstBolDetails = invoice.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).ToList();
                    foreach (var item in lstBolDetails)
                    {
                        response.BolDetailsNew.Add(item.ToViewModel());
                    }
                    response.FuelDeliveryDetails.FuelFees.TruckLoadType = TruckLoadTypes.LessTruckLoad;
                    response.FuelDeliveryDetails.FuelFees = GetInvoiceFuelFees(invoice.FuelRequestFees,invoice.DroppedGallons);
                    response.FuelDeliveryDetails.FuelFees.DiscountLineItems.ForEach(t => t.InvoiceId = id);
                    SetSurchargeDetails(invoice.InvoiceXAdditionalDetail, response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee, invoice.Order.FuelRequest.MstProduct.ProductTypeId, invoice.DroppedGallons);
                    var lstAccessorialFees = invoice.InvoiceXAccessorialFees.ToList();
                    foreach (var item in lstAccessorialFees)
                    {
                        response.AccessorialFeeDetails.Add(item.ToViewModel());
                    }
                    response.SpecialInstructions = invoice.InvoiceXSpecialInstructions.Select(t => t.ToViewModel()).ToList();

                    if (response.CityGroupTerminalId > 0)
                    {
                        response.CityGroupTerminalName = Context.DataContext.MstExternalTerminals.SingleOrDefault(t => t.Id == response.CityGroupTerminalId).Name;
                    }

                    if (invoice.Order.ExternalBrokerBuySellDetail != null)
                    {
                        response.ExternalBrokerId = invoice.Order.ExternalBrokerId ?? 0;
                        response.ExternalBrokeredOrder.CustomerId = invoice.Order.ExternalBrokerId ?? 0;
                        response.IsBuySellOrder = true;
                    }

                    if (invoice.Order.ExternalBrokerOrderDetail != null)
                    {
                        response.ExternalBrokerId = invoice.Order.ExternalBrokerId ?? 0;
                        response.ExternalBrokeredOrder.CustomerId = invoice.Order.ExternalBrokerId ?? 0;
                        response.ExternalBrokeredOrder = invoice.Order.ExternalBrokerOrderDetail.ToViewModel();
                        var invoiceFees = invoice.FuelRequestFees.ToExternalBrokerViewModel();
                        if (invoiceFees == null)
                        {
                            response.ExternalBrokeredOrder.BrokeredOrderFee = invoice.Order.FuelRequest.FuelRequestFees.ToExternalBrokerViewModel();
                        }
                        else
                        {
                            response.ExternalBrokeredOrder.BrokeredOrderFee = invoiceFees;
                        }
                        response.IsThirdPartyHardwareUsed = true;
                    }

                    var isOtherFueltypeInFr = Context.DataContext.MstProducts.Any(t => t.Id == invoice.Order.FuelRequest.FuelTypeId && t.ProductTypeId == 10);

                    response.SelectedAssets = response.Assets.Where(t => t.IsDropMade).Select(t => t.JobXAssetId).ToList();
                    response.TaxDetails = invoice.TaxDetails.ToList().ToViewModel();
                    response.Taxes = invoice.TaxDetails.ToTaxViewModel(isOtherFueltypeInFr);
                    //set tax service failure flag
                    response.IsTaxServiceFailure = response.TaxDetails.AvaTaxDetails.Count == 0;
                    response.TrackableScheduleId = invoice.TrackableScheduleId;

                    if (isOtherFueltypeInFr)
                    {
                        response.IsOtherFuelTypeTaxesGiven = true;
                    }
                    response.AssetTracked = response.Assets.Count > 0;

                    if (invoice.Order.FuelRequest.FuelRequestDetail.EndDate.HasValue)
                    {
                        response.MaxDropDate = invoice.Order.FuelRequest.FuelRequestDetail.EndDate.Value.Date;
                    }
                    else if (invoice.Order.FuelRequest.Job.EndDate.HasValue)
                    {
                        response.MaxDropDate = invoice.Order.FuelRequest.Job.EndDate.Value.Date;
                    }
                    if (invoice.InvoiceXAdditionalDetail != null)
                    {
                        if (!string.IsNullOrWhiteSpace(invoice.InvoiceXAdditionalDetail.Notes))
                        {
                            response.Notes = invoice.InvoiceXAdditionalDetail.Notes;
                        }
                        else
                        {
                            response.Notes = invoice.Order != null &&
                                             invoice.Order.OrderAdditionalDetail != null && !string.IsNullOrWhiteSpace(invoice.Order.OrderAdditionalDetail.Notes) ? invoice.Order.OrderAdditionalDetail.Notes : "";
                        }
                    }

                    if (invoice.InvoiceXAdditionalDetail.SplitLoadChainId != null)
                    {
                        response.SplitLoadInvoices = Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == invoice.InvoiceXAdditionalDetail.SplitLoadChainId
                                                                                             && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                                             && t.IsActive)
                                                                                 .OrderBy(t => t.InvoiceXAdditionalDetail.SplitLoadSequence)
                                                                                 .Select(t => new SplitLoadInvoiceDetailViewModel()
                                                                                 {
                                                                                     Id = t.Id,
                                                                                     Number = t.DisplayInvoiceNumber,
                                                                                     ChainId = t.InvoiceXAdditionalDetail.SplitLoadChainId,
                                                                                     Sequence = t.InvoiceXAdditionalDetail.SplitLoadSequence,
                                                                                     InvoiceTypeId = t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp ? (int)InvoiceType.DigitalDropTicketManual : (int)InvoiceType.Manual
                                                                                 }).ToList();
                        response.IsBolEditAllowed = response.SplitLoadInvoices.Select(t => t.InvoiceTypeId).Distinct().Count() == 1;
                    }
                    if (response.IsFTL)
                    {
                        response.QuantityIndicatorTypeId = invoice.Order.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId ?? 0;
                        // response.AssetTracked = false;
                        //  response.Assets.Clear();
                        response.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach((t) => t.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad);
                        response.FuelDeliveryDetails.FuelFees.TruckLoadType = TruckLoadTypes.FullTruckLoad;
                    }
                    if (invoice.Order.FuelRequest.FreightOnBoardTypeId.HasValue)
                        response.IsVariousFobOrigin = invoice.Order.FuelRequest.FreightOnBoardTypeId == (int)FreightOnBoardTypes.Terminal && invoice.Order.FuelRequest.Job.LocationType == JobLocationTypes.Various;

                    if (response.IsVariousFobOrigin || !string.IsNullOrEmpty(response.SplitLoadChainId))
                    {
                        var dropLocation = invoice.InvoiceDispatchLocation.OrderByDescending(t => t.Id).FirstOrDefault(t => t.LocationType == (int)LocationType.Drop);
                        if (dropLocation != null)
                            response.DropAddress = dropLocation.ToDispatchAddress(response.DropAddress);
                        else
                        {
                            response.DropAddress = invoice.InvoiceXAdditionalDetail.ToDispatchAddress();
                            response.DropAddress.State.Id = invoice.Order.FuelRequest.Job.StateId;
                        }
                    }
                    response.PickUpAddress = new DropAddressViewModel() { State = new StateViewModel() { Id = 1, Code = Resource.lblDummy } };
                    //var pickupAddress = invoice.InvoiceDispatchLocation.FirstOrDefault(t => t.LocationType == (int)LocationType.PickUp && t.PickupLocation == PickupLocationType.BulkPlant);
                    //if (pickupAddress != null && pickupAddress.Address != null)
                    //{
                    //    response.PickUpAddress = pickupAddress.ToPickUpLocation();
                    //}
                    if (response.BolDetailsNew != null && response.BolDetailsNew.Any(t => t.IsLiftInfoAvailable()))
                        response.PickUpAddress = response.BolDetailsNew.FirstOrDefault().ToPickUpLocation(response.PickUpAddress);

                    response.FuelDeliveryDetails.FuelFees.Currency = invoice.Currency;
                    response.FuelDeliveryDetails.FuelFees.UoM = invoice.UoM;
                    response.ExternalBrokeredOrder.BrokeredOrderFee.Currency = invoice.Currency;
                    response.ExternalBrokeredOrder.BrokeredOrderFee.UoM = invoice.UoM;
                    response.MaxDropDate = (response.MaxDropDate == null || response.MaxDropDate.Value > DateTimeOffset.Now.Date) ? DateTimeOffset.Now.Date : response.MaxDropDate;
                    if (invoice.Order.FuelRequest.TankRentals.FirstOrDefault() != null)
                    {
                        response.TankFrequency = invoice.Order.FuelRequest.TankRentals.FirstOrDefault().ToViewModel();
                        response.TankFrequency.Tanks.ForEach(t => { t.UoM = invoice.Order.FuelRequest.UoM; t.Currency = invoice.Order.FuelRequest.Currency; t.IsToBeIncludedInInvoice = true; });
                        response.TankRentalFrequencyTypes = new List<DropdownDisplayItem>();
                        response.SelectedFrequency = invoice.InvoiceXAdditionalDetail.TankRentalFrequency != null ? invoice.InvoiceXAdditionalDetail.TankRentalFrequency.FrequencyTypeId : 0;
                        response.TankRentalFrequencyTypes = invoice.Order.FuelRequest.TankRentals
                        .Where(t => t.ActivationStatusId == (int)ActivationStatus.Active).Select(t => new DropdownDisplayItem()
                        {
                            Id = t.FrequencyTypeId,
                            Name = ((FrequencyTypes)t.FrequencyTypeId).GetDisplayName()
                        }).GroupBy(t => t.Id).Select(g => g.First()).ToList();
                    }

                    /// on display we show only converted values
                    if (response.Assets != null && response.Assets.Any())
                    {
                        foreach (var asset in response.Assets)
                        {
                            if (response.UoM == UoM.MetricTons)
                            {
                                var gravity = response.CreationMethod != CreationMethod.Mobile ? response.Gravity : asset.Gravity;
                                if (gravity.HasValue && gravity.Value > 0)
                                {
                                    var convertedQty = GetOriginalDroppedGallonsForMFN(asset.DropGallons.Value, gravity.Value, (int)response.UoM);
                                    asset.DropGallons = convertedQty;
                                }

                            }
                            else if (response.UoM == UoM.Barrels)
                            {
                                var convertedQty = GetOriginalDroppedGallonsForMFN(asset.DropGallons.Value, ApplicationConstants.GallonsToBarrelConversion, (int)response.UoM);
                                asset.DropGallons = convertedQty;
                            }
                            asset.JobCountryId = response.JobCountryId;
                        }
                    }
                    if (invoice.TrackableSchedule != null)
                    {
                        response.DeliveryLevelPO = invoice.TrackableSchedule.DeliveryLevelPO;
                        response.DeliveryLevelTrackableScheduleId = invoice.TrackableScheduleId == null ? 0 : invoice.TrackableScheduleId.GetValueOrDefault();
                    }
                    response = response.CorrectValues();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetManualInvoiceForEditAsync", ex.Message, ex);
            }
            return response;
        }

        public void SetSurchargeDetails(InvoiceXAdditionalDetail invoiceXAdditionalDetail, FuelSurchargeFreightFeeViewModel fuelSurchargeFreightFee, int productType, decimal droppedGallons)
        {
            if (fuelSurchargeFreightFee != null)
            {
                fuelSurchargeFreightFee.SurchargeEiaPrice = invoiceXAdditionalDetail.SurchargeEIAPrice ?? 0;
                fuelSurchargeFreightFee.SurchargePercentage = invoiceXAdditionalDetail.SurchargePercentage ?? 0;
                fuelSurchargeFreightFee.SurchargePricingType = invoiceXAdditionalDetail.SurchargePricingType;
                fuelSurchargeFreightFee.SurchargeProductType = productType.GetFuelSurchargeProductType();
                fuelSurchargeFreightFee.SurchargeTableRangeEnd = invoiceXAdditionalDetail.SurchargeTableRangeEnd ?? 0;
                fuelSurchargeFreightFee.SurchargeTableRangeStart = invoiceXAdditionalDetail.SurchargeTableRangeStart ?? 0;
                fuelSurchargeFreightFee.IsSurchargeApplicable = invoiceXAdditionalDetail.IsSurchargeApplicable;
                fuelSurchargeFreightFee.GallonsDelivered = droppedGallons;
                fuelSurchargeFreightFee.FreightPricingMethod = invoiceXAdditionalDetail.FreightPricingMethod;
                if (invoiceXAdditionalDetail.FreightPricingMethod == FreightPricingMethod.Auto)
                {
                    SetSurchargeDetailsForAuto(invoiceXAdditionalDetail, fuelSurchargeFreightFee);
                }
            }
        }

        private static void SetSurchargeDetailsForAuto(InvoiceXAdditionalDetail invoiceXAdditionalDetail, FuelSurchargeFreightFeeViewModel fuelSurchargeFreightFee)
        {
            fuelSurchargeFreightFee.IsFreightCostApplicable = invoiceXAdditionalDetail.IsFreightCostApplicable;
            fuelSurchargeFreightFee.FuelSurchargeTableId = invoiceXAdditionalDetail.FuelSurchargeTableId;
            fuelSurchargeFreightFee.FuelSurchargeTableType = invoiceXAdditionalDetail.FuelSurchargeTableType;
            fuelSurchargeFreightFee.FreightRateRuleId = invoiceXAdditionalDetail.FreightRateRuleId;
            fuelSurchargeFreightFee.FreightRateRuleType = invoiceXAdditionalDetail.FreightRateRuleType;
            fuelSurchargeFreightFee.FreightRateTableType = invoiceXAdditionalDetail.FreightRateTableType;
            fuelSurchargeFreightFee.AutoFreightDistance = invoiceXAdditionalDetail.Distance.HasValue ? invoiceXAdditionalDetail.Distance.Value : 0;
        }

        private static List<ImageViewModel> GetAssetDropImages(Invoice invoice)
        {
            return invoice.AssetDrops.Where(t => t.ImageId.HasValue).Select(t => new ImageViewModel { Id = t.ImageId.Value }).ToList();
        }

        private static ImageViewModel GetImageViewModel(Invoice invoice)
        {
            return invoice.ImageId == null ? new ImageViewModel() : new ImageViewModel { Id = invoice.ImageId.Value, FilePath = invoice.Image.FilePath };
        }

        private static ImageViewModel GetImageViewModel(int? imageId, string imagePath)
        {
            return imageId == null ? new ImageViewModel() : new ImageViewModel { Id = imageId.Value, FilePath = imagePath };
        }

        private static ImageViewModel GetBolImageViewModel(int? imageId, string imagePath)
        {
            return string.IsNullOrEmpty(imagePath) ? new ImageViewModel() : new ImageViewModel { Id = imageId.HasValue ? imageId.Value : 0, FilePath = imagePath };
        }

        private static ImageViewModel GetBolImageViewModel(Invoice invoice)
        {
            return invoice.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault() == null || invoice.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault().ImageId == null ? new ImageViewModel() : new ImageViewModel { Id = invoice.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault().ImageId.Value, FilePath = invoice.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault().Image.FilePath };
        }

        private static ImageViewModel GetAdditionalImageViewModel(Invoice invoice)
        {
            return invoice.InvoiceXAdditionalDetail == null || invoice.InvoiceXAdditionalDetail.AdditionalImageId == null ? new ImageViewModel() : new ImageViewModel { Id = invoice.InvoiceXAdditionalDetail.AdditionalImageId.Value, FilePath = invoice.InvoiceXAdditionalDetail.AdditionalImage.FilePath };
        }
        private static ImageViewModel GetTaxAffidavitImageViewModel(Invoice invoice)
        {
            return invoice.InvoiceXAdditionalDetail == null || invoice.InvoiceXAdditionalDetail.TaxAffidavitImageId == null ? new ImageViewModel() : new ImageViewModel { Id = invoice.InvoiceXAdditionalDetail.TaxAffidavitImageId.Value, FilePath = invoice.InvoiceXAdditionalDetail.TaxAffidavitImage.FilePath };
        }
        private static ImageViewModel GetBDNImageViewModel(Invoice invoice)
        {
            return invoice.InvoiceXAdditionalDetail == null || invoice.InvoiceXAdditionalDetail.BDNImageId == null ? new ImageViewModel() : new ImageViewModel { Id = invoice.InvoiceXAdditionalDetail.BDNImageId.Value, FilePath = invoice.InvoiceXAdditionalDetail.BDNImage.FilePath };
        }
        private static ImageViewModel GetCoastGuardInspectionImageViewModel(Invoice invoice)
        {
            return invoice.InvoiceXAdditionalDetail == null || invoice.InvoiceXAdditionalDetail.CoastGuardInspectionImageId == null ? new ImageViewModel() : new ImageViewModel { Id = invoice.InvoiceXAdditionalDetail.CoastGuardInspectionImageId.Value, FilePath = invoice.InvoiceXAdditionalDetail.CoastGuardInspectionImage.FilePath };
        }
        private static ImageViewModel GetInspectionRequestVoucherImageViewModel(Invoice invoice)
        {
            return invoice.InvoiceXAdditionalDetail == null || invoice.InvoiceXAdditionalDetail.InspectionRequestVoucherImageId == null ? new ImageViewModel() : new ImageViewModel { Id = invoice.InvoiceXAdditionalDetail.InspectionRequestVoucherImageId.Value, FilePath = invoice.InvoiceXAdditionalDetail.InspectionRequestVoucherImage.FilePath };
        }
        private static ImageViewModel GetSignatureImage(Signature signature)
        {
            return signature?.ImageId == null ? new ImageViewModel() : new ImageViewModel { Id = signature.ImageId.Value, FilePath = signature.Image.FilePath };
        }

        private string GetTerminalName(Invoice invoice)
        {
            var bolDetails = invoice.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault();
            if (bolDetails != null && bolDetails.TerminalId.HasValue)
            {
                return bolDetails.MstExternalTerminal.Name;
            }
            else if (invoice.Order.TerminalId.HasValue)
            {
                return invoice.Order.MstExternalTerminal.Name;
            }
            else if (invoice.Order.FuelRequest.TerminalId.HasValue)
            {
                return invoice.Order.FuelRequest.MstExternalTerminal.Name;
            }

            return string.Empty;
        }

        public async Task<List<InvoiceGridViewModel>> GetBuyerInvoiceGridByOrderAsync(int userId, int orderId, int allowedInvoiceType)
        {
            List<InvoiceGridViewModel> response = new List<InvoiceGridViewModel>();

            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId && t.IsActive);
                if (user != null && user.Company != null)
                {
                    HelperDomain helperDomain = new HelperDomain(this);
                    List<Invoice> invoices;

                    if (allowedInvoiceType == (int)InvoiceType.DigitalDropTicketManual)
                    {
                        invoices = Context.DataContext.Invoices.Where(t => t.OrderId == orderId &&
                                                                      (t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual ||
                                                                      t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) &&
                                                                      t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t.IsBuyPriceInvoice).ToList();
                    }
                    else
                    {
                        // if not DDT, then load all other invoices types
                        invoices = Context.DataContext.Invoices.Where(t => t.OrderId == orderId &&
                                                                      t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual &&
                                                                      t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp &&
                                                                      t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t.IsBuyPriceInvoice).ToList();
                    }
                    response = invoices.Select(t => new InvoiceGridViewModel(Status.Success)
                    {
                        Id = t.Id.ToString(),
                        InvoiceNumber = t.DisplayInvoiceNumber,
                        InvoiceNumberId = t.InvoiceHeader.InvoiceNumberId,
                        PoNumber = t.PoNumber,
                        BolNumber = t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault() == null ? Resource.lblHyphen : t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault().BolNumber,
                        Supplier = t.Order.Company.Name,
                        Email = t.Order.User.Email,
                        PhoneNumber = t.Order.User.PhoneNumber,
                        OrderType = t.Order.FuelRequest.MstOrderType.Name,
                        AssetCount = t.AssetDrops.Where(t2 => t2.DropStatus == (int)DropStatus.None || t2.DropStatus == (int)DropStatus.UnplannedTankDrop).Select(t1 => t1.JobXAsset.AssetId).Distinct().Count(),
                        //OrderTotal = helperDomain.GetOrderAmount(t.Order.FuelRequest, t.Order.BrokeredMaxQuantity),
                        Quantity = t.DroppedGallons,
                        PricePerGallon = Convert.ToString(t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).Select(t1 => t1.PricePerGallon).FirstOrDefault()),
                        FuelDeliveredPercentage = helperDomain.GetFuelDeliveredPercentagePerInvoice(t),
                        InvoiceAmount = t.OrderId == null ? 0 : helperDomain.GetInvoiceAmount(t).GetPreciseValue(6),
                        DropDate = t.DropEndDate.ToString(Resource.constFormatDate),
                        Status = t.InvoiceXInvoiceStatusDetails.SingleOrDefault(t1 => t1.IsActive).MstInvoiceStatus.Name,
                        WaitingFor = t.WaitingFor

                    }).OrderByDescending(t => t.Id).ToList();

                    response = response.CorrectValues();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetBuyerInvoiceGridByOrderAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceGridViewModel>> GetBuyerInvoiceGridAsync(int userId, int companyId, bool isBuyerAdmin, InvoiceDataTableViewModel invoiceFilter = null, int allowedInvoiceType = 0, int BrandedCompanyId = -1, string InvoiceTypeIdFilter = "")
        {
            List<InvoiceGridViewModel> response = new List<InvoiceGridViewModel>();

            try
            {
                if (invoiceFilter == null)
                {
                    invoiceFilter = new InvoiceDataTableViewModel();
                }
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetBuyerInvoicesAsync(companyId, userId, isBuyerAdmin, allowedInvoiceType, invoiceFilter, BrandedCompanyId, 30, InvoiceTypeIdFilter);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetBuyerInvoiceGridAsync", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<InvoiceGridBuyerDashboardModel>> GetInvoiceGridForBuyerDashboardAsync(InvoiceGridBuyerDashboardInputModel requestModel)
        {
            List<InvoiceGridBuyerDashboardModel> response = new List<InvoiceGridBuyerDashboardModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetInvoiceGridForBuyerDashboardAsync(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceGridForBuyerDashboardAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<InvoiceGridViewModel>> GetSupplierInvoiceGridAsync(int companyId, InvoiceDataTableViewModel invoiceFilter = null, int allowedInvoiceType = (int)InvoiceType.Manual, ViewInvoices view = ViewInvoices.All)
        {
            List<InvoiceGridViewModel> response = new List<InvoiceGridViewModel>();
            try
            {
                if (invoiceFilter == null)
                {
                    invoiceFilter = new InvoiceDataTableViewModel();
                }
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetSupplierInvoicesAsync(companyId, allowedInvoiceType, (int)view, invoiceFilter);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetSupplierInvoicesAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceBolGridViewModel>> GetSupplierBolInvoicesAsync(int companyId, InvoiceDataTableViewModel invoiceFilter = null, ViewInvoices view = ViewInvoices.All)
        {
            List<InvoiceBolGridViewModel> response = new List<InvoiceBolGridViewModel>();
            try
            {
                if (invoiceFilter == null)
                {
                    invoiceFilter = new InvoiceDataTableViewModel();
                }
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetSupplierBolInvoicesAsync(companyId, invoiceFilter, view);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetSupplierBolInvoicesAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceBolGridViewModel>> GetBuyerBolInvoicesAsync(int userId, int companyId, InvoiceDataTableViewModel invoiceFilter = null, ViewInvoices view = ViewInvoices.All, int BrandedCompanyId = 0)
        {
            List<InvoiceBolGridViewModel> response = new List<InvoiceBolGridViewModel>();
            try
            {
                if (invoiceFilter == null)
                {
                    invoiceFilter = new InvoiceDataTableViewModel();
                }
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetBuyerBolInvoicesAsync(userId, companyId, invoiceFilter, view, BrandedCompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetBuyerBolInvoicesAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceHistoryGridViewModel>> GetInvoiceHistoryGridAsync(int id, int userId = 0, bool isSuperAdmin = false)
        {
            List<InvoiceHistoryGridViewModel> response = new List<InvoiceHistoryGridViewModel>();
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId && t.IsActive);
                if ((user != null && user.Company != null) || isSuperAdmin)
                {
                    HelperDomain helperDomain = new HelperDomain(this);
                    var invoice = Context.DataContext.Invoices.FirstOrDefault(t => t.Id == id);

                    var allInvoices = Context.DataContext.Invoices.Where(t => ((t.Id == invoice.ParentId || t.InvoiceHeader.InvoiceNumber.Id == invoice.InvoiceHeader.InvoiceNumber.Id) && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive)).OrderByDescending(t => t.Id);
                    var allInvoiceHistory = allInvoices.Select(t => new
                    {
                        t.Id,
                        t.InvoiceHeaderId,
                        t.DisplayInvoiceNumber,
                        t.OrderId,
                        t.DropEndDate,
                        t.DropStartDate,
                        InvoiceStatus = t.InvoiceXInvoiceStatusDetails.Where(t1 => t1.IsActive).Select(t1 => new { t1.StatusId, t1.MstInvoiceStatus.Name }).FirstOrDefault(),
                        t.UpdatedDate,
                        t.Version,
                        t.DroppedGallons,
                        PricePerGallon = t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).Select(t1 => t1.PricePerGallon).FirstOrDefault(),
                        PricingType = t.OrderId == null ? null : new { t.Order.FuelRequest.PricingTypeId, t.Order.FuelRequest.MstPricingType.Name },
                        User = Context.DataContext.Users.Where(x => x.Id == t.UpdatedBy).Select(s => new { s.FirstName, s.LastName }).FirstOrDefault(),
                        t.UoM,
                        t.InvoiceTypeId,
                        t.BasicAmount,
                        t.TotalDiscountAmount,
                        t.TotalTaxAmount,
                        t.TotalFeeAmount
                    });

                    foreach (var item in allInvoiceHistory)
                    {
                        var invoiceHistory = new InvoiceHistoryGridViewModel();

                        invoiceHistory.Id = item.Id;
                        invoiceHistory.InvoiceHeaderId = item.InvoiceHeaderId;
                        invoiceHistory.InvoiceNumber = item.DisplayInvoiceNumber;
                        invoiceHistory.InvoiceAmount = item.OrderId == null ? 0 : helperDomain.GetInvoiceAmount(item.InvoiceTypeId, item.BasicAmount, item.TotalTaxAmount, item.TotalDiscountAmount, item.TotalFeeAmount);
                        invoiceHistory.DropDate = item.DropEndDate.ToString(Resource.constFormatDate);
                        invoiceHistory.DropTime = $"{item.DropStartDate.DateTime.ToShortTimeString()}{Resource.lblSingleHyphen}{item.DropEndDate.DateTime.ToShortTimeString()}";
                        if (item.InvoiceStatus.StatusId == (int)InvoiceStatus.Draft || item.InvoiceStatus.StatusId == (int)InvoiceStatus.Canceled)
                        {
                            invoiceHistory.InvoiceDate = Resource.lblHyphen;
                        }
                        else
                        {
                            invoiceHistory.InvoiceDate = item.UpdatedDate.ToString(Resource.constFormatDate);
                        }

                        invoiceHistory.Version = Convert.ToString("V" + item.Version);
                        invoiceHistory.Quantity = $"{item.DroppedGallons.GetCommaSeperatedValue()} {item.UoM}";
                        if (item.OrderId != null)
                        {
                            invoiceHistory.PricePerGallon = item.PricingType.PricingTypeId == (int)PricingType.Tier ?
                                                item.PricingType.Name :
                                                item.PricePerGallon.ToString(ApplicationConstants.DecimalFormat2);
                        }
                        else
                        {
                            invoiceHistory.PricePerGallon = Resource.lblHyphen;
                        }
                        invoiceHistory.ModifiedDate = item.UpdatedDate.ToString(Resource.constFormatDate);
                        invoiceHistory.ModifiedBy = $"{item.User.FirstName} {item.User.LastName}";
                        invoiceHistory.Status = item.InvoiceStatus.Name;
                        response.Add(invoiceHistory);
                    }
                    response = response.CorrectValues();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceHistoryGridAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceApprovalHistoryGridViewModel>> GetInvoiceApprovalHistoryGridAsync(int id = 0)
        {
            List<InvoiceApprovalHistoryGridViewModel> response = new List<InvoiceApprovalHistoryGridViewModel>();

            try
            {

                var invoice = await Context.DataContext.Invoices.Include(t => t.InvoiceXInvoiceStatusDetails).SingleOrDefaultAsync(t => t.Id == id);
                if (invoice != null)
                {
                    List<int> statusIds = new List<int>() { (int)InvoiceStatus.Received, (int)InvoiceStatus.Rejected, (int)InvoiceStatus.WaitingForApproval };
                    var invoiceStatusDetails = invoice.InvoiceXInvoiceStatusDetails.Where(t => statusIds.Contains(t.StatusId));
                    var recievedStatus = invoice.InvoiceXInvoiceStatusDetails.FirstOrDefault(t => t.StatusId == (int)InvoiceStatus.Received);
                    if (recievedStatus != null)
                    {
                        invoiceStatusDetails = invoiceStatusDetails.Where(t => t.Id <= recievedStatus.Id).OrderByDescending(t => t.Id);
                    }
                    var assignedUser = invoice.Order.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t => t.IsActive);
                    foreach (var invoicestatus in invoiceStatusDetails)
                    {
                        response.Add(new InvoiceApprovalHistoryGridViewModel(Status.Success)
                        {
                            Id = invoice.Id,
                            InvoiceNumber = invoice.DisplayInvoiceNumber,
                            ApprovedDate = (invoicestatus.StatusId == (int)InvoiceStatus.Received || invoicestatus.StatusId == (int)InvoiceStatus.Rejected) ?
                                                invoicestatus.UpdatedDate.ToString(Resource.constFormatDate) : Resource.lblHyphen,
                            AssignedTo = assignedUser != null ? $"{assignedUser.User.FirstName} {assignedUser.User.LastName}" : Resource.lblHyphen,
                            CreatedDate = invoice.CreatedDate.ToString(Resource.constFormatDate),
                            CreatedTime = invoice.CreatedDate.DateTime.ToShortTimeString(),
                            Status = invoicestatus.StatusId == (int)InvoiceStatus.Received ? Resource.lblApproved : invoicestatus.MstInvoiceStatus.Name,
                            ApprovedBy = (invoicestatus.StatusId == (int)InvoiceStatus.Received || invoicestatus.StatusId == (int)InvoiceStatus.Rejected) ?
                                                $"{invoicestatus.User.FirstName} {invoicestatus.User.LastName}" : Resource.lblHyphen
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceApprovalHistoryGridAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceApprovalHistoryGridViewModel>> GetBuyerWaitingForApprovalListAsync(int userId, InvoiceFilterViewModel filter = null)
        {
            List<InvoiceApprovalHistoryGridViewModel> response = new List<InvoiceApprovalHistoryGridViewModel>();
            try
            {
                var helperDomain = new HelperDomain(this);
                var jobIds = await helperDomain.GetJobIdsAsync(userId, filter.GroupIds);
                var groupIdslist = helperDomain.GetGroupList(filter.GroupIds);

                if (jobIds != null)
                {
                    var user = Context.DataContext.Users.Include(t => t.MstRoles).SingleOrDefault(t => t.Id == userId);
                    var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active &&
                                    !t.IsBuyPriceInvoice && t.Order != null && t.Order.FuelRequest.Job != null &&
                                    ((groupIdslist.Count == 0 && t.Order.BuyerCompanyId == user.CompanyId) || (groupIdslist.Count > 0 && t.Order.BuyerCompany.SubCompanies.Any(t1 => t1.SubCompanyId == t.Order.BuyerCompanyId && groupIdslist.Contains(t1.CompanyGroupId)))) &&
                                    t.Order.FuelRequest.Job.IsActive && (filter.JobId == 0 || t.Order.FuelRequest.Job.Id == filter.JobId) &&
                                    jobIds.Contains(t.Order.FuelRequest.Job.Id) && t.Order.FuelRequest.Job.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open &&
                                    (t.InvoiceXInvoiceStatusDetails.OrderByDescending(t1 => t1.Id).FirstOrDefault().StatusId == (int)InvoiceStatus.WaitingForApproval ||
                                    (t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.Rejected) && !t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.Received))));

                    if (filter.AllowedInvoiceType == (int)InvoiceType.DigitalDropTicketManual)
                    {
                        invoices = invoices.Where(t => t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual ||
                                                 t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp);
                    }
                    else if (filter.AllowedInvoiceType != (int)InvoiceType.All)
                    {
                        invoices = invoices.Where(t => t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual &&
                                                 t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp);
                    }
                    invoices = ApplyInvoiceFilter(filter, invoices);

                    foreach (var invoice in invoices)
                    {
                        var invoiceCreatedDate = invoice.CreatedDate.ToTargetDateTimeOffset(invoice.Order.FuelRequest.Job.TimeZoneName);
                        var assignedUser = invoice.Order.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t => t.IsActive);
                        response.Add(new InvoiceApprovalHistoryGridViewModel(Status.Success)
                        {
                            Id = invoice.Id,
                            InvoiceNumber = invoice.DisplayInvoiceNumber,
                            AssignedTo = assignedUser != null ? $"{assignedUser.User.FirstName} {assignedUser.User.LastName}" : Resource.lblHyphen,
                            CreatedDate = invoiceCreatedDate.ToString(Resource.constFormatDate),
                            CreatedTime = invoiceCreatedDate.DateTime.ToShortTimeString(),
                            IsApprovalUser = (assignedUser != null && userId == assignedUser.UserId) || user.MstRoles.Any(t => t.Id == (int)UserRoles.Admin),
                            JobName = invoice.Order.FuelRequest.Job.Name,
                            PoNumber = invoice.PoNumber,
                            Quantity = invoice.DroppedGallons,
                            Status = invoice.InvoiceXInvoiceStatusDetails.FirstOrDefault(t => t.IsActive).StatusId == (int)InvoiceStatus.Received ? Resource.lblApproved : invoice.InvoiceXInvoiceStatusDetails.FirstOrDefault(t => t.IsActive).MstInvoiceStatus.Name,
                            StatusId = invoice.InvoiceXInvoiceStatusDetails.FirstOrDefault(t => t.IsActive).StatusId,
                            UnitOfMeasurement = (int)invoice.UoM,
                            Currency = (int)invoice.Currency
                        });
                    }
                    response = response.OrderByDescending(t => t.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetBuyerWaitingForApprovalListAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DdtApprovalListViewModel>> GetDdtApprovalListAsync(UserContext userContext, int userId, int brandedSuppCompId = 0)
        {
            List<DdtApprovalListViewModel> response = new List<DdtApprovalListViewModel>();
            try
            {
                var spResponse = new StoredProcedureDomain(this).GetDdtApprovalListAsync(userContext.CompanyId, userId, true, brandedSuppCompId).Result;
                if (spResponse != null && spResponse.Any())
                {
                    foreach (var invoice in spResponse)
                    {
                        var ddtApprovalItem = new DdtApprovalListViewModel();
                        ddtApprovalItem.Id = invoice.Id;
                        ddtApprovalItem.InvoiceNumber = invoice.InvoiceNumber;
                        ddtApprovalItem.AssignedTo = invoice.AssignedTo;
                        ddtApprovalItem.IsApprovalUser = invoice.IsApprovalUser;
                        ddtApprovalItem.JobName = invoice.JobName;
                        ddtApprovalItem.PoNumber = invoice.PoNumber;
                        ddtApprovalItem.Quantity = invoice.DroppedGallons;

                        ddtApprovalItem.Status = invoice.Status;
                        ddtApprovalItem.StatusId = invoice.StatusId;

                        ddtApprovalItem.DeliveryDate = invoice.DropDate;
                        ddtApprovalItem.DeliveryTime = invoice.DeliveryTime;

                        if (ddtApprovalItem.StatusId == (int)InvoiceStatus.WaitingForApproval)
                        {
                            ddtApprovalItem.CreatedDate = invoice.InvoiceUpdatedDate;
                            ddtApprovalItem.CreatedTime = invoice.InvoiceUpdateTime;
                        }
                        else if (ddtApprovalItem.StatusId == (int)InvoiceStatus.Received)
                        {
                            ddtApprovalItem.ApprovedDate = invoice.InvoiceUpdatedDate;
                            ddtApprovalItem.ApprovedTime = invoice.InvoiceUpdateTime;
                        }
                        else if (ddtApprovalItem.StatusId == (int)InvoiceStatus.Rejected)
                        {
                            ddtApprovalItem.RejectedDate = invoice.InvoiceUpdatedDate;
                            ddtApprovalItem.RejectedTime = invoice.InvoiceUpdateTime;
                        }
                        ddtApprovalItem.UnitOfMeasurement = invoice.UnitOfMeasurement;
                        ddtApprovalItem.Currency = invoice.Currency;
                        ddtApprovalItem.FuelType = invoice.FuelType;
                        ddtApprovalItem.AssetCount = invoice.AssetCount;
                        ddtApprovalItem.DropDate = invoice.DropDate;
                        ddtApprovalItem.DroppedGallons = invoice.DroppedGallons;
                        ddtApprovalItem.HeaderId = invoice.InvoiceHeaderId;
                        ddtApprovalItem.DropTime = invoice.DeliveryTime;
                        ddtApprovalItem.InvoiceAmount = invoice.InvoiceAmount;
                        response.Add(ddtApprovalItem);
                    }
                }
                response = response.OrderByDescending(t => t.HeaderId).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetDdtApprovalListAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<InvoiceDetailViewModel> GetBuyerInvoiceStatusAsync(int invoiceId)
        {
            var response = new InvoiceDetailViewModel();
            try
            {
                var invoice = await Context.DataContext.Invoices.Select(t => new
                {
                    t.Id,
                    t.InvoiceHeader.InvoiceNumberId,
                    InvoiceHeaderId = t.InvoiceHeader.Id,
                    t.ImageId,
                    t.InvoiceTypeId,
                    t.InvoiceVersionStatusId,
                    t.PaymentDate,
                    StatementId = t.BillingStatementXInvoices.Where(t1 => t1.IsActive && t1.BillingStatement.IsGenerated).Select(t1 => t1.StatementId).FirstOrDefault(),
                    AssetDrops = t.AssetDrops.Select(t1 => new { t1.ImageId }),
                    InvoiceXInvoiceStatusDetails = t.InvoiceXInvoiceStatusDetails.Select(t1 => new { t1.Id, t1.StatusId, t1.IsActive }),
                    SignatureImageId = t.Signaure == null ? (int?)null : t.Signaure.ImageId,
                    Currency = t.Currency,
                    DisplayInvoiceNumber = t.DisplayInvoiceNumber,
                    t.ReferenceId,
                    Order = new
                    {
                        t.Order.Id,
                        t.Order.AcceptedCompanyId,
                        t.Order.FuelRequest.Currency,
                        t.Order.FuelRequest.UoM,
                        t.Order.FuelRequest.FuelRequestTypeId,
                        t.PoNumber,
                        JobXApprovalUsers = t.Order.FuelRequest.Job.JobXApprovalUsers.Select(t1 => new { t1.UserId, t1.IsActive }),
                        JobCountryId = t.Order.FuelRequest.Job.CountryId
                    },
                    t.Order.FuelRequest.Job.IsMarine,
                    IsExceptionDdt = t.InvoiceExceptions.Any()
                }).FirstOrDefaultAsync(t => t.Id == invoiceId);

                if (invoice != null)
                {
                    //to redirect to Invoice Summary page when Invoice has been deleted
                    if (invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive && invoice.InvoiceXInvoiceStatusDetails.Last().StatusId == (int)InvoiceStatus.Deleted)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageInvoiceDeleted;
                        return response;
                    }

                    response.SignatureImage = invoice.SignatureImageId == null ? new ImageViewModel() : new ImageViewModel { Id = invoice.SignatureImageId.Value };
                    response.InvoiceImage = invoice.ImageId == null ? new ImageViewModel() : new ImageViewModel { Id = invoice.ImageId.Value };
                    response.AssetDropImages = invoice.AssetDrops.Where(t => t.ImageId.HasValue).Select(t => new ImageViewModel { Id = t.ImageId.Value }).ToList();
                    response.Invoice.Id = invoice.Id;
                    response.Invoice.InvoiceHeaderId = invoice.InvoiceHeaderId;
                    response.OrderId = invoice.Order.Id;
                    response.Invoice.StatementId = invoice.StatementId;
                    response.Invoice.StatusId = invoice.InvoiceXInvoiceStatusDetails.First(t => t.IsActive).StatusId;
                    response.Invoice.InvoiceTypeId = invoice.InvoiceTypeId;
                    response.Invoice.Currency = invoice.Currency;
                    response.FuelRequest.FuelDetails.FuelPricing.Currency = invoice.Order.Currency;
                    response.FuelRequest.FuelDetails.FuelQuantity.UoM = invoice.Order.UoM;
                    response.Invoice.PaymentDate = invoice.PaymentDate;
                    response.Invoice.InvoiceNumber.Id = invoice.InvoiceNumberId;
                    response.Invoice.DisplayInvoiceNumber = invoice.DisplayInvoiceNumber;
                    response.Invoice.ReferenceId = invoice.ReferenceId;
                    response.PoNumber = invoice.Order.PoNumber;
                    response.IsApprovalWorkflowEnabled = invoice.InvoiceXInvoiceStatusDetails.Any(t => t.StatusId == (int)InvoiceStatus.WaitingForApproval);
                    response.ApprovalUserId = invoice.Order == null || invoice.Order.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest || !invoice.Order.JobXApprovalUsers.Any(t => t.IsActive) ?
                    (int?)null : invoice.Order.JobXApprovalUsers.First(t => t.IsActive).UserId;
                    if (response.Invoice.StatusId == (int)InvoiceStatus.Rejected)
                    {
                        var invoiceStatus = invoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault(t => t.StatusId != (int)InvoiceStatus.Rejected);
                        if (invoiceStatus != null && invoiceStatus.StatusId == (int)InvoiceStatus.WaitingForApproval)
                        {
                            response.IsRejectedAndWaitingApproval = true;
                        }
                    }
                    response.SupplierCompanyId = invoice.Order.AcceptedCompanyId;
                    response.IsExceptionDdt = invoice.IsExceptionDdt;
                    response.IsMarineLocation = invoice.IsMarine;
                    response.JobCountryId = invoice.Order.JobCountryId;
                    await SetInvoiceReceivePaymentStatus(response);

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetBuyerInvoiceStatusAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<InvoiceDetailViewModel> GetBuyerInvoiceDetailAsync(int invoiceId, UserContext userContext)
        {
            CheckEntityAccess(userContext, invoiceId, EntityType.Invoice);

            var response = new InvoiceDetailViewModel();
            var storedProcedureDomain = new StoredProcedureDomain(this);
            try
            {

                HelperDomain helperDomain = new HelperDomain(this);
                var invoice = await Context.DataContext.Invoices.Include(t => t.AssetDrops)
                    .Include(t => t.Order).Include(t => t.Order.User).Include(t => t.Order.FuelRequest)
                    .Include(t => t.Order.FuelRequest.Job).Include(t => t.FuelRequestFees)
                    .FirstOrDefaultAsync(t => t.Id == invoiceId);
                if (invoice != null)
                {
                    //to redirect to Invoice Summary page when Invoice has been deleted
                    if (invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive && invoice.InvoiceXInvoiceStatusDetails.Last().StatusId == (int)InvoiceStatus.Deleted)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageInvoiceDeleted;
                        return response;
                    }
                    var bolDetails = invoice.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault();
                    response = new InvoiceDetailViewModel
                    {
                        PoNumber = invoice.Order == null ? Resource.lblHyphen : invoice.PoNumber,
                        OrderId = invoice.Order == null ? 0 : invoice.Order.Id,
                        InvoiceImage = GetImageViewModel(invoice),
                        BolImage = GetBolImageViewModel(invoice),
                        AdditionalImage = GetAdditionalImageViewModel(invoice),
                        IsFTL = invoice.Order != null ? invoice.Order.IsFTL : false,
                        AssetDropImages = GetAssetDropImages(invoice),
                        SignatureImage = GetSignatureImage(invoice.Signaure),
                        SupplierEmail = invoice.Order == null ? string.Empty : invoice.Order.User.Email,
                        SupplierPhone = invoice.Order == null ? string.Empty : invoice.Order.User.PhoneNumber,
                        SupplierCompanyName = invoice.Order == null ? string.Empty : invoice.Order.Company.Name,
                        PercentFuelDelivered = helperDomain.GetFuelDeliveredPercentagePerInvoice(invoice),
                        SupplierName = invoice.Order == null ? string.Empty : $"{invoice.Order.User.FirstName} {invoice.Order.User.LastName}",
                        Invoice = invoice.ToViewModel(),
                        AssetCount = invoice.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).GroupBy(t => t.JobXAssetId).Count(),
                        FuelRequest = invoice.Order == null ? new FuelRequestViewModel(Status.Success) : invoice.Order.FuelRequest.ToViewModel(),
                        TerminalName = bolDetails == null || bolDetails?.TerminalId == null ? Resource.lblHyphen : bolDetails.SiteName,
                        CityGroupTerminalId = bolDetails?.CityGroupTerminalId,
                        PaymentTermId = invoice.PaymentTermId,
                        NetDays = invoice.NetDays,
                        StateTax = invoice.StateTax,
                        FederalTax = invoice.FedTax,
                        SalesTax = invoice.SalesTax,
                        Distance = bolDetails == null || bolDetails?.TerminalId == null ? 0 : helperDomain.CalculateDistance(invoice.Order.FuelRequest.Job.Latitude, invoice.Order.FuelRequest.Job.Longitude, bolDetails.Latitude, bolDetails.Longitude),
                        ApprovalUserId = invoice.Order == null || invoice.Order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest || !invoice.Order.FuelRequest.Job.JobXApprovalUsers.Any(t => t.IsActive) ?
                        (int?)null : invoice.Order.FuelRequest.Job.JobXApprovalUsers.First(t => t.IsActive).UserId,
                        IsApprovalWorkflowEnabled = invoice.InvoiceXInvoiceStatusDetails.Any(t => t.StatusId == (int)InvoiceStatus.WaitingForApproval),
                        IsHidePricingEnabled = helperDomain.IsHidePricingEnabled(invoice, CompanyType.Buyer),
                        CustomerSignature = invoice.Signaure != null ? invoice.Signaure.ToViewModel() : null,
                        SplitLoadChainId = invoice.InvoiceXAdditionalDetail.SplitLoadChainId,
                        Notes = invoice.InvoiceXAdditionalDetail.Notes,
                        InvoiceNumberId = invoice.InvoiceHeader.InvoiceNumberId,
                        OriginalInvoiceId = invoice.InvoiceXAdditionalDetail.OriginalInvoiceId,
                        OriginalInvoiceNumber = invoice.InvoiceXAdditionalDetail.OriginalInvoice != null ? invoice.InvoiceXAdditionalDetail.OriginalInvoice.DisplayInvoiceNumber : string.Empty,
                        IsMarineLocation = invoice.Order != null ? invoice.Order.FuelRequest.Job.IsMarine : false,
                        DeliveryLevelPO = GetDeliveryScheduleDeliveryLevelPO(invoice.InvoiceHeaderId)
                    };

                    response.Invoice.IsPendingInvoiceAdjustment = Context.DataContext.Discounts.Any(t => t.InvoiceId == invoice.Id
                                                                    && t.DealStatus == (int)DealStatus.Pending
                                                                    && t.CreatedCompanyId != userContext.CompanyId);

                    var creditInvoice = helperDomain.GetCreditInvoice(invoice.InvoiceXAdditionalDetail.OriginalInvoiceId);
                    if (creditInvoice != null)
                    {
                        response.CreditInvoiceId = creditInvoice.Item1;
                        response.CreditInvoiceDisplayNumber = creditInvoice.Item2;
                    }

                    if (response.CustomerSignature != null)
                    {
                        response.CustomerSignature.IsJobSignatureEnabled = invoice.Order != null ? invoice.Order.SignatureEnabled : false;
                    }
                    if (response.OriginalInvoiceId != null)
                    {
                        response.OriginalInvoiceNumber = invoice.InvoiceXAdditionalDetail.OriginalInvoice.DisplayInvoiceNumber;
                    }

                    response.Invoice.BolLiftDetails = await storedProcedureDomain.GetBolDetailsAsync(invoice.Id);
                    response.Invoice.TotalFees = helperDomain.GetInvoiceTotalFees(invoice);
                    response.Invoice.IsHidePricingEnabled = helperDomain.IsHidePricingEnabled(invoice, CompanyType.Buyer);
                    response.Invoice.IsApprovalWorkflowEnabledForJob = invoice.Order.FuelRequest.Job.IsApprovalWorkflowEnabled;
                    response.Invoice.IsSupplierPreferenceDDT = invoice.SupplierPreferredInvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.SupplierPreferredInvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp;

                    SetSurchargeDetails(invoice.InvoiceXAdditionalDetail, response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee, 0, response.Invoice.DroppedGallons); //productytpe is not required to set

                    response.Invoice.TimeZoneName = invoice.Order == null ? string.Empty : invoice.Order.FuelRequest.Job.TimeZoneName;
                    response.Invoice.InvoiceLineItemDetails = await storedProcedureDomain.GetInvoiceLineItemDetailsAsync(invoice.InvoiceHeaderId);

                    //var taxInvoiceDetails = response.Invoice.InvoiceLineItemDetails.FirstOrDefault(t => t.TaxInvoiceId > 0);
                    //if (taxInvoiceDetails != null)
                    //{
                    //    var taxinvoice = Context.DataContext.Invoices.FirstOrDefault(t => t.Id == taxInvoiceDetails.TaxInvoiceId);
                    //    if (taxinvoice.TaxDetails != null && taxinvoice.TaxDetails.Count > 0)
                    //    {
                    //        response.Invoice.TaxDetails = taxinvoice.TaxDetails.Where(t => t.TaxExemptionInd != ApplicationConstants.AvaTaxExemptedInd).ToList().ToViewModel();
                    //    }
                    //}

                    var taxDetails = await storedProcedureDomain.GetConsolidatedInvoicePdfTaxesAsync(invoice.InvoiceHeaderId);
                    if (taxDetails.Any())
                    {
                        response.Invoice.TaxDetails.AvaTaxDetails = new List<TaxDetailsViewModel>();
                        var distinctTaxes = taxDetails.GroupBy(x => new { x.TaxPricingTypeId, x.RateDescription }).Select(group => group.ToList()).ToList();

                        foreach (var tax in distinctTaxes)
                        {
                            TaxDetailsViewModel taxDetailsViewModel = new TaxDetailsViewModel();
                            taxDetailsViewModel.TaxPricingTypeId = tax.Select(y => y.TaxPricingTypeId).FirstOrDefault();
                            taxDetailsViewModel.TradingTaxAmount = tax.Select(y => y.TradingTaxAmount).Sum();
                            taxDetailsViewModel.RateDescription = tax.Select(y => y.RateDescription).FirstOrDefault();
                            taxDetailsViewModel.IsModified = tax.Select(y => y.IsModified).FirstOrDefault();
                            response.Invoice.TaxDetails.AvaTaxDetails.Add(taxDetailsViewModel);
                        }
                    }

                    var fuelfees = await storedProcedureDomain.GetInvoiceDetailsFuelFeesAsync(invoice.InvoiceHeaderId);
                    response.FuelDeliveryDetails.FuelFees.FuelRequestFees = fuelfees.ToFeesViewModel();
                    response.FuelDeliveryDetails.FuelFees.DiscountLineItems = fuelfees.ToDiscountFeesViewModel();

                    //var feesInvoiceDetails = response.Invoice.InvoiceLineItemDetails.FirstOrDefault(t => t.FeesInvoiceId > 0);
                    //if (feesInvoiceDetails != null)
                    //{
                    //    var fessinvoice = Context.DataContext.Invoices.FirstOrDefault(t => t.Id == feesInvoiceDetails.FeesInvoiceId);
                    //    response.FuelDeliveryDetails.FuelFees = GetInvoiceFuelFees(fessinvoice.FuelRequestFees);
                    //}

                    if (response.Invoice.StatusId == (int)InvoiceStatus.Rejected)
                    {
                        var invoiceStatus = invoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault(t => t.StatusId != (int)InvoiceStatus.Rejected);
                        if (invoiceStatus != null && invoiceStatus.StatusId == (int)InvoiceStatus.WaitingForApproval)
                        {
                            response.IsRejectedAndWaitingApproval = true;
                        }
                    }
                    if (response.ApprovalUserId.HasValue)
                    {
                        var user = invoice.Order.FuelRequest.Job.JobXApprovalUsers.First(t => t.IsActive).User;
                        response.ApprovalUserName = $"{user.FirstName} {user.LastName}";
                    }
                    if (invoice.InvoiceXInvoiceStatusDetails.SingleOrDefault(t => t.IsActive).StatusId == (int)InvoiceStatus.Unassigned)
                    {
                        response.FuelRequest.Job.IsTaxExempted = true;
                    }
                    if (!string.IsNullOrEmpty(response.SplitLoadChainId))
                    {
                        response.SplitLoadInvoices = Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == invoice.InvoiceXAdditionalDetail.SplitLoadChainId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive).OrderBy(t => t.InvoiceXAdditionalDetail.SplitLoadSequence).Select(t => new InvoiceNumberViewModel() { Id = t.Id, Number = t.DisplayInvoiceNumber }).ToList();
                    }
                    if (invoice.ParentId > 0)
                    {
                        var parentInvoice = Context.DataContext.Invoices.SingleOrDefault(t => t.Id == invoice.ParentId);
                        if (parentInvoice != null)
                        {
                            response.LinkedInvoiceId = parentInvoice.Id;
                            response.LinkedInvoiceType = parentInvoice.InvoiceTypeId;
                            response.LinkedInvoiceNumber = parentInvoice.DisplayInvoiceNumber;
                            response.StatusId = parentInvoice.InvoiceXInvoiceStatusDetails.FirstOrDefault().StatusId;
                        }
                    }
                    else
                    {
                        var childInvoice = Context.DataContext.Invoices.SingleOrDefault(t => t.ParentId == invoice.Id);
                        if (childInvoice != null)
                        {
                            response.LinkedInvoiceId = childInvoice.Id;
                            response.LinkedInvoiceType = childInvoice.InvoiceTypeId;
                            response.LinkedInvoiceNumber = childInvoice.DisplayInvoiceNumber;
                            response.StatusId = childInvoice.InvoiceXInvoiceStatusDetails.FirstOrDefault().StatusId;
                        }
                    }
                    SetInvoiceAmountAndFees(response, invoice);
                    await SetInvoiceReceivePaymentStatus(response);

                    response = response.CorrectValues();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetBuyerInvoiceDetailAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<InvoiceDetailViewModel> GetInvoiceDetailSummary(int invoiceId)
        {
            var response = new InvoiceDetailViewModel();
            if (invoiceId > 0)
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => new { UserId = t.Order.AcceptedBy, CompanyId = t.Order.AcceptedCompanyId }).FirstOrDefaultAsync();
                if (invoice != null)
                {
                    var authDomain = new AuthenticationDomain();
                    var userContext = await authDomain.GetUserContextAsync(invoice.UserId);
                    response = await GetSupplierInvoiceDetailAsync(invoiceId, invoice.CompanyId, userContext);
                }
            }
            else
            {
                return null;
            }

            return response;
        }

        public async Task<InvoiceDetailViewModel> GetSupplierInvoiceDetailAsync(int invoiceId, int companyId, UserContext userContext = null)
        {
            CheckEntityAccess(userContext, invoiceId, EntityType.Invoice);
            var response = new InvoiceDetailViewModel();
            try 
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var invoice = await storedProcedureDomain.GetSupplierInvoiceDetails(companyId, invoiceId);
                if (invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive && invoice.StatusId == (int)InvoiceStatus.Deleted)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageInvoiceDeleted;
                    return response;
                }
                else if (invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive && invoice.StatusId == (int)InvoiceStatus.Draft)
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageDraftConvertedtoDDT;
                    return response;
                }

                HelperDomain helperDomain = new HelperDomain(this);
                var actingCompany = invoice.OrderId != null && invoice.AcceptedCompanyId.Value == companyId ? (int)CompanyType.Supplier : (int)CompanyType.Buyer;
                response = new InvoiceDetailViewModel
                {
                    PoNumber = invoice.OrderId == null ? Resource.lblHyphen : invoice.PoNumber,
                    OrderId = invoice.OrderId ?? 0,
                    InvoiceImage = GetImageViewModel(invoice.ImageId, invoice.ImagePath),
                    BolImage = GetBolImageViewModel(invoice.BolImageId, invoice.BolFilePath),
                    SignatureImage = GetImageViewModel(invoice.SignImageId, null),
                    CreditCheckApprovalImage = GetBolImageViewModel(null, invoice.CreditCheckApprovalFilePath),
                    AdditionalImage = GetImageViewModel(invoice.AdditionalImageId, invoice.AdditionalImgFilePath),
                    PercentFuelDelivered = GetFuelDeliveredPercentagePerInvoice(invoice),
                    Invoice = invoice.ToViewModel(),
                    FuelRequest = invoice.OrderId == null ? new FuelRequestViewModel(Status.Success) : invoice.ToFuelRequestViewModel(),
                    ActingCompanyType = invoice.OrderId == null ? (int)CompanyType.Supplier : actingCompany,
                    TerminalName = invoice.TerminalName,
                    CityGroupTerminalId = invoice.CityGroupTerminalId,
                    PaymentTermId = invoice.PaymentTermId == 0 ? (int)PaymentTerms.DueOnReceipt : invoice.PaymentTermId,
                    NetDays = invoice.NetDays,
                    StateTax = invoice.StateTax,
                    FederalTax = invoice.FedTax,
                    IsFTL = invoice.IsFtl,
                    SalesTax = invoice.SalesTax,
                    Distance = invoice.TerminalId == null || invoice.OrderId == null ? 0 : helperDomain.CalculateDistance(invoice.JobLatitude.Value, invoice.JobLongitude.Value, invoice.TerminalLatitude.Value, invoice.TerminalLongitude.Value),
                    ApprovalUserId = invoice.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest ? (int?)null : invoice.ApprovalUserId,
                    IsHidePricingEnabled = IsHidePricingEnabled(invoice, CompanyType.Supplier),
                    DriverName = invoice.DriverName,
                    TrackableSchedule = invoice.OrderId != null && invoice.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries ? GetDeliverySchedule(invoice) : "",
                    ParentId = invoice.ParentId,
                    CustomerSignature = invoice.SignatureId != null ? invoice.ToSignatureViewModel() : null,
                    SplitLoadChainId = invoice.SplitLoadChainId,
                    PaymentMethod = invoice.PaymentMethod,
                    SupplierEmail = invoice.OrderId == null ? "" : invoice.CustomerEmail,
                    SupplierName = invoice.OrderId == null ? "" : invoice.CustomerName,
                    BuyerCompanyName = invoice.OrderId == null ? invoice.DriverComment ?? "" : invoice.BuyerCompanyName,
                    SupplierPhone = invoice.OrderId == null ? "" : invoice.CustomerPhoneNumber,
                    AssetCount = invoice.AssetFilled > 0 ? invoice.AssetFilled : invoice.AssetCount,
                    ExternalBrokerId = invoice.ExternalBrokerId,
                    AmountPaid = invoice.AmountPaid,
                    BalanceRemaining = invoice.BalanceRemaining,
                    PaymentStatus = invoice.PaymentStatus,
                    OriginalInvoiceId = invoice.OriginalInvoiceId,
                    OriginalInvoiceNumber = invoice.OriginalInvoiceNumber,
                    CreditInvoiceId = invoice.CreditInvoiceId,
                    CreditInvoiceDisplayNumber = invoice.CreditInvoiceDisplayNumber,
                    IsSingleBolInvoice = invoice.IsSingleBolInvoice,
                    IsLiftFileValidated = invoice.IsLiftFileValidated,
                    IsMarineLocation = invoice.IsMarineLocation,
                    JobCountryId = invoice.JobCountryId,
                    DeliveryLevelPO = invoice.DeliveryLevelPO,
                    BDRNumber = invoice.BDRNumber
                };
                response.Invoice.IsHidePricingEnabled = response.IsHidePricingEnabled;
                var assetDropImages = await storedProcedureDomain.GetAssetDropImagesAsync(invoice.Id);
                assetDropImages.ForEach(t => response.AssetDropImages.Add(new ImageViewModel() { Id = t.ImageId, FilePath = t.FilePath }));
                response.Invoice.BolLiftDetails = await storedProcedureDomain.GetBolDetailsAsync(invoice.Id);
                response.DisplayPricePerGallon = invoice.DisplayPrice; //GetInvoiceDisplayPrice(invoice, helperDomain);
                response.Invoice.InvoiceLineItemDetails = await storedProcedureDomain.GetInvoiceLineItemDetailsAsync(invoice.InvoiceHeaderId);

                //var taxInvoiceDetails = response.Invoice.InvoiceLineItemDetails.FirstOrDefault(t => t.TaxInvoiceId > 0);
                //if (taxInvoiceDetails != null)
                //{
                //    var taxinvoice = Context.DataContext.Invoices.FirstOrDefault(t => t.Id == taxInvoiceDetails.TaxInvoiceId);
                //    if (taxinvoice.TaxDetails != null && taxinvoice.TaxDetails.Count > 0)
                //    {
                //        response.Invoice.TaxDetails = taxinvoice.TaxDetails.Where(t => t.TaxExemptionInd != ApplicationConstants.AvaTaxExemptedInd).ToList().ToViewModel();
                //    }
                //}

                var notesList = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoice.InvoiceHeaderId).Select(s => new { Notes = s.TrackableSchedule.Notes, DispatcherNotes = s.TrackableSchedule.DisPatcherNote }).ToList(); 
                response.Invoice.DispatcherNotes = notesList.Where(t => !string.IsNullOrWhiteSpace(t.Notes)).Select(t => t.Notes).ToList();
                response.Invoice.CommonNote = notesList.Where(t => !string.IsNullOrWhiteSpace(t.DispatcherNotes)).Select(t => t.DispatcherNotes).Distinct().ToList();
                
                var fuelfees = await storedProcedureDomain.GetInvoiceDetailsFuelFeesAsync(invoice.InvoiceHeaderId);
                response.FuelDeliveryDetails.FuelFees.FuelRequestFees = fuelfees.ToFeesViewModel();
                response.FuelDeliveryDetails.FuelFees.DiscountLineItems = fuelfees.ToDiscountFeesViewModel();
                if (response.ExternalBrokerId > 0)
                {
                    response.BrokeredOrder.BrokeredOrderFee = fuelfees.ToExternalBrokerViewModel();
                }

                if (invoice.IsBuyAndSellOrder)
                {
                    response.IsBuyAndSellOrder = true;
                    PricingServiceDomain pricingServiceDomain = new PricingServiceDomain(this);
                    var inputModel = new PricingDetailRequestViewModel { Id = invoice.RequestPriceDetailId };
                    var requestPriceDetails = await pricingServiceDomain.GetPricingRequestDetailByIdAsync(inputModel);
                    invoice.RequestPricePerGallon = requestPriceDetails.PricingRequestDetail.PricePerGallon;
                    response.BuyAndSellPricingDetail = GetBuyAndSellPricingDetails(invoice);
                    if (!string.IsNullOrWhiteSpace(response.BuyAndSellPricingDetail.ExternalBrokerName))
                    {
                        response.BuyerCompanyName = invoice.ExternalBrokerName;
                    }
                }
                var taxDetails = await storedProcedureDomain.GetConsolidatedInvoicePdfTaxesAsync(invoice.InvoiceHeaderId);
                if (taxDetails.Any())
                {
                    response.Invoice.TaxDetails.AvaTaxDetails = taxDetails.Select(t => new TaxDetailsViewModel()
                    {
                        RateDescription = t.RateDescription,
                        TradingTaxAmount = t.TradingTaxAmount,
                        TaxPricingTypeId = t.TaxPricingTypeId,
                        IsModified = t.IsModified
                    }).ToList();
                }
                if (!string.IsNullOrWhiteSpace(invoice.SplitLoadChainId))
                {
                    response.SplitLoadInvoices = await storedProcedureDomain.GetSplitLoadInvoicesAsync(invoice.SplitLoadChainId);
                }
                if (invoice.OrderId != null)
                {
                    var supplierQualifications = await storedProcedureDomain.GetSupplierQualifications(invoice.FuelRequestId.Value);
                    if (supplierQualifications.Any())
                    {
                        response.FuelRequest.FuelOfferDetails.SupplierQualifications = supplierQualifications;
                    }
                }
                if (invoice.AcceptedCompanyId == companyId && invoice.StatementId > 0)
                {
                    response.Invoice.StatementNumber = invoice.StatementNumber;
                    response.Invoice.StatementId = invoice.StatementId;
                }
                if (invoice.StatusId == (int)InvoiceStatus.Unassigned)
                {
                    response.FuelRequest.Job.IsTaxExempted = true;
                }
                if (invoice.OrderId != null && !IsDigitalDropTicket(response.Invoice.InvoiceTypeId) && invoice.SendDtnFile)
                {
                    response.BuyerCompanyId = invoice.BuyerCompanyId.Value;
                    GetFtlSupplierDtnDetails(response, companyId);
                }
                if (invoice.LinkedInvoiceId.HasValue)
                {
                    response.LinkedInvoiceId = invoice.LinkedInvoiceId.Value;
                    response.LinkedInvoiceType = invoice.LinkedInvoiceType.Value;
                    response.LinkedInvoiceNumber = invoice.LinkedInvoiceNumber;
                }
                SetInvoiceAmountAndFees(response, invoice);

                response.PreferencesSetting = new OnboardingPreferenceViewModel()
                {
                    Id = invoice.PreferencesSettingId ?? 0,
                    IsSupressOrderPricing = invoice.IsSupressOrderPricing
                };

                response.Culture = new HelperDomain(this).SetEntityThreadCulture(invoice.Currency);
                response = response.CorrectValues();

                ApplicationDomain appDomain = new ApplicationDomain(this);
                response.ShowEditInvoiceMenu = appDomain.GetKeySettingValue(ApplicationConstants.KeyAppSettingShowEditInvoiceMenu, true) && !invoice.IsLiftFileValidated;
                response.ShowCreditRebillMenu = appDomain.GetKeySettingValue(ApplicationConstants.KeyAppSettingShowCreditRebillMenu, true) && !invoice.IsLiftFileValidated;

                SetCanEditFlag(invoice, response);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetSupplierInvoiceDetailAsync", ex.Message, ex);
            }
            return response;
        }

        private void SetCanEditFlag(UspGetSupplierInvoiceDetails invoice, InvoiceDetailViewModel response)
        {
            if (response.Invoice != null)
            {
                response.CanEdit = IsDigitalDropTicket(invoice.InvoiceTypeId);
            }
        }

        public BuyAndSellPricingDetailViewModel GetBuyAndSellPricingDetails(Invoice invoice)
        {
            BuyAndSellPricingDetailViewModel response = new BuyAndSellPricingDetailViewModel();
            try
            {
                decimal pricePerGallon = invoice.Order.FuelRequest.CreationTimeRackPPG;
                decimal rackPrice = invoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail.RackPrice).FirstOrDefault();
                decimal brokerMarkUp = invoice.Order.ExternalBrokerBuySellDetail.BrokerMarkUp;
                decimal supplierMarkUp = invoice.Order.ExternalBrokerBuySellDetail.SupplierMarkUp;

                response.BasePrice = pricePerGallon + rackPrice;
                response.BrokerMarkUp = brokerMarkUp;
                response.SupplierMarkUp = supplierMarkUp;
                response.BuyPrice = response.BasePrice + brokerMarkUp;
                response.SellPrice = response.BuyPrice + supplierMarkUp;
                response.BuyPriceDetail = $"{Resource.lblBasePrice} + {Resource.constSymbolCurrency}{brokerMarkUp.ToString(ApplicationConstants.DecimalFormat2)}";
                response.SellPriceDetail = $"{Resource.lblBuyPrice} + {Resource.constSymbolCurrency}{supplierMarkUp.ToString(ApplicationConstants.DecimalFormat2)}";
                response.IsBuyPriceInvoice = invoice.IsBuyPriceInvoice;
                response.Currency = invoice.Order.ExternalBrokerBuySellDetail.Currency;
                if (!response.IsBuyPriceInvoice)
                {
                    var brokerId = invoice.Order.ExternalBrokerBuySellDetail.ExternalBrokerId;
                    response.ExternalBrokerName = GetExternalBrokereName(brokerId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetBuyAndSellPricingDetails", ex.Message, ex);
            }

            return response;
        }

        public BuyAndSellPricingDetailViewModel GetBuyAndSellPricingDetails(UspGetSupplierInvoiceDetails invoice)
        {
            BuyAndSellPricingDetailViewModel response = new BuyAndSellPricingDetailViewModel();
            response.IsBuyPriceInvoice = invoice.IsBuyPriceInvoice;
            decimal brokerMarkUp = invoice.BrokerMarkUp.Value;
            decimal supplierMarkUp = invoice.SupplierMarkUp.Value;

            response.BasePrice = invoice.RequestPricePerGallon.Value + invoice.RackPrice.Value;
            response.BrokerMarkUp = brokerMarkUp;
            response.SupplierMarkUp = supplierMarkUp;
            response.BuyPrice = response.BasePrice + brokerMarkUp;
            response.SellPrice = response.BuyPrice + supplierMarkUp;
            response.BuyPriceDetail = $"{Resource.lblBasePrice} + {Resource.constSymbolCurrency}{brokerMarkUp.ToString(ApplicationConstants.DecimalFormat2)}";
            response.SellPriceDetail = $"{Resource.lblBuyPrice} + {Resource.constSymbolCurrency}{supplierMarkUp.ToString(ApplicationConstants.DecimalFormat2)}";
            if (!response.IsBuyPriceInvoice)
            {
                response.ExternalBrokerName = invoice.ExternalBrokerName;
            }


            return response;
        }

        private string GetExternalBrokereName(int brokerId)
        {
            var response = string.Empty;
            try
            {
                response = Context.DataContext.ExternalBrokers.Where(t => t.Id == brokerId).Select(t => t.CompanyName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetExternalBrokereName", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ApproveInvoiceAsync(UserContext userContext, int invoiceId, bool isBrokeredDropTicket = false)
        {
            StatusViewModel response = new StatusViewModel();
            var isWaitingForApproval = false;
            int buyerId = userContext.Id;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var invoice = await Context.DataContext.Invoices.SingleOrDefaultAsync(t => t.Id == invoiceId);
                    if (invoice != null)
                    {
                        var invoiceCurrentStatus = invoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault(t => t.StatusId != (int)InvoiceStatus.Rejected);
                        if (invoiceCurrentStatus != null && invoiceCurrentStatus.StatusId == (int)InvoiceStatus.WaitingForApproval)
                        {
                            isWaitingForApproval = true;
                        }
                        if (invoiceCurrentStatus == null)
                        {
                            isWaitingForApproval = GetInvoicePreviousStatus(invoice);
                        }
                        invoice.InvoiceXInvoiceStatusDetails.ToList().ForEach(t => t.IsActive = false);

                        InvoiceXInvoiceStatusDetail invoiceStatus = new InvoiceXInvoiceStatusDetail();
                        invoiceStatus.StatusId = isWaitingForApproval || isBrokeredDropTicket ? (int)InvoiceStatus.Received : (int)InvoiceStatus.Approved;
                        invoiceStatus.IsActive = true;
                        invoiceStatus.UpdatedBy = userContext.Id;
                        invoiceStatus.UpdatedDate = DateTimeOffset.Now;
                        invoice.InvoiceXInvoiceStatusDetails.Add(invoiceStatus);

                        Context.DataContext.Entry(invoice).State = EntityState.Modified;

                        await Context.CommitAsync();
                        transaction.Commit();

                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                        NotificationDomain notificationDomain = new NotificationDomain(this);

                        if (isWaitingForApproval)
                        {
                            // approval workflow enabled
                            await notificationDomain.AddNotificationEventAsync(EventType.InvoiceApprovedApprovalWorkflow, invoice.InvoiceHeaderId, userContext.Id);

                            if (invoice.Order != null && (invoice.Order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketMobileApp || invoice.Order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketManual))
                            {
                                //Buyer DDT Approved
                                await newsfeedDomain.SetInvoiceApprovedNewsfeed(userContext, invoice);
                            }
                            else
                            {
                                // Buyer Invoice Accepted
                                await newsfeedDomain.SetInvoiceAcceptedNewsfeed(userContext, invoice);
                            }
                        }
                        else
                        {
                            await notificationDomain.AddNotificationEventAsync(EventType.InvoiceApproved, invoice.InvoiceHeaderId, userContext.Id);

                            // Buyer Invoice Approved
                            if (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                            {
                                await newsfeedDomain.SetInvoiceApprovedNewsfeed(userContext, invoice);
                            }
                            else
                            {
                                await newsfeedDomain.SetInvoiceAcceptedNewsfeed(userContext, invoice);
                            }
                        }

                        response.StatusCode = Status.Success;
                        response.StatusMessage = invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp ? Resource.errMessageApproveInvoiceSuccess : Resource.errMessageApproveDigitalDropTicketSuccess;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "ApproveInvoiceAsync", ex.Message, ex);
                }
            }

            var updatedInvoice = await Context.DataContext.Invoices.Include(t => t.Order).Include(t => t.Order.FuelRequest)
                .Include(t => t.InvoiceXInvoiceStatusDetails).SingleOrDefaultAsync(t => t.Id == invoiceId);

            if (response.StatusCode == Status.Success && updatedInvoice.WaitingFor == (int)WaitingAction.CustomerApproval)
            {
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                var pricingDomain = new PricingServiceDomain(this);

                var FrPricingDetail = updatedInvoice.Order.FuelRequest.FuelRequestPricingDetail;

                int requestPriceDetailId = FrPricingDetail.RequestPriceDetailId;
                var lastUpdateDate = await pricingDomain.GetLastUpdatedPricingDate(requestPriceDetailId);
                if (lastUpdateDate != null)
                {
                    if ((updatedInvoice.Order.FuelRequest.PricingTypeId == (int)PricingType.RackAverage
                        || updatedInvoice.Order.FuelRequest.PricingTypeId == (int)PricingType.RackHigh
                        || updatedInvoice.Order.FuelRequest.PricingTypeId == (int)PricingType.RackLow)
                        && lastUpdateDate.Date < updatedInvoice.DropEndDate.Date)
                    {
                        updatedInvoice.WaitingFor = (int)WaitingAction.UpdatedPrice;
                        Context.DataContext.Entry(updatedInvoice).State = EntityState.Modified;
                        await Context.CommitAsync();
                        await newsfeedDomain.SetBuyerApprovedDDTWaitingForPriceNewsfeed(userContext, updatedInvoice);
                        response.StatusMessage = Resource.successMessageDDTApprovedInvoiceWaitingForPrice;
                    }
                    else if (updatedInvoice.Order.DefaultInvoiceType == (int)InvoiceType.Manual && ((updatedInvoice.SupplierPreferredInvoiceTypeId == null && updatedInvoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                            ||
                            (updatedInvoice.SupplierPreferredInvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && updatedInvoice.SupplierPreferredInvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp))
                            ||
                            (updatedInvoice.Order.DefaultInvoiceType == (int)InvoiceType.Manual && updatedInvoice.SupplierPreferredInvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
                    {
                        userContext = new UserContext()
                        {
                            CompanyId = updatedInvoice.User.CompanyId ?? updatedInvoice.Order.AcceptedCompanyId,
                            CompanyName = updatedInvoice.User.Company.Name,
                            Email = updatedInvoice.User.Email,
                            Id = updatedInvoice.User.Id,
                            Name = $"{updatedInvoice.User.FirstName} {updatedInvoice.User.LastName}"
                        };
                        response = await CreateInvoiceFromApprovedDropTicket(userContext, invoiceId, updatedInvoice.InvoiceHeaderId, updatedInvoice.CreatedBy, buyerId, isWaitingForApproval, true);

                        if (response.StatusCode == Status.Success)
                        {
                            response.StatusMessage = Resource.successMessageDDTApprovedAndInvoiceCreated;
                        }
                    }
                    else
                    {
                        updatedInvoice.WaitingFor = (int)WaitingAction.Nothing;
                        Context.DataContext.Entry(updatedInvoice).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }
                    // If Broker Selected Invoice to be created after approval.
                    await ApproveBrokeredInvoice(userContext, updatedInvoice);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> DeclineInvoiceAsync(UserContext userContext, DeclineInvoiceViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var invoice = await Context.DataContext.Invoices.SingleOrDefaultAsync(t => t.Id == viewModel.DeclineReason.Id);
                    if (invoice != null)
                    {
                        invoice.InvoiceXInvoiceStatusDetails.ToList().ForEach(t => t.IsActive = false);

                        InvoiceXDeclineReason invoiceDeclineReason = new InvoiceXDeclineReason()
                        {
                            AdditionalNotes = viewModel.DeclineReason.AdditionalNotes,
                            InvoiceId = viewModel.DeclineReason.Id,
                            DeclineReasonId = viewModel.DeclineReason.ReasonId
                        };
                        invoice.InvoiceXDeclineReasons.Add(invoiceDeclineReason);

                        InvoiceXInvoiceStatusDetail invoiceStatus = new InvoiceXInvoiceStatusDetail();
                        invoiceStatus.StatusId = (int)InvoiceStatus.Rejected;
                        invoiceStatus.IsActive = true;
                        invoiceStatus.UpdatedBy = userContext.Id;
                        invoiceStatus.UpdatedDate = DateTimeOffset.Now;
                        invoice.InvoiceXInvoiceStatusDetails.Add(invoiceStatus);

                        Context.DataContext.Entry(invoice).State = EntityState.Modified;
                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;

                        var declineReason = string.Empty;
                        if (viewModel.DeclineReason.ReasonId == (int)InvoiceDeclinedReason.Other) // if other reason
                        {
                            declineReason = viewModel.DeclineReason.AdditionalNotes;
                        }
                        else
                        {
                            declineReason = Context.DataContext.MstInvoiceDeclineReasons.First(t => t.Id == viewModel.DeclineReason.ReasonId).Name;
                        }
                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                        await newsfeedDomain.SetInvoiceRejectedNewsfeed(userContext, invoice, declineReason);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "DeclineInvoiceAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<DeclineInvoiceViewModel> GetDeclineInvoiceDetailAsync(UserContext userContext, int invoiceId, int statusId)
        {
            var response = new DeclineInvoiceViewModel()
            {
                UserId = userContext.Id,
            };
            response.DeclineReason.Id = invoiceId;
            response.DeclineReason.InvoiceStatusId = statusId;
            try
            {
                var invoiceUoM = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => t.UoM).FirstOrDefaultAsync();
                if (invoiceUoM != UoM.None)
                {
                    response.UoM = invoiceUoM;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetDeclineInvoiceDetailAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> PaidInvoiceAsync(InvoiceDetailViewModel viewModel, int userId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var invoice = await Context.DataContext.Invoices.SingleOrDefaultAsync(t => t.Id == viewModel.Invoice.Id);
                    if (invoice != null)
                    {
                        invoice.InvoiceXInvoiceStatusDetails.ToList().ForEach(t => t.IsActive = false);
                        invoice.PaymentDate = viewModel.Invoice.PaymentDate;

                        InvoiceXInvoiceStatusDetail invoiceStatus = new InvoiceXInvoiceStatusDetail();
                        invoiceStatus.StatusId = (int)InvoiceStatus.Unconfirmed;
                        invoiceStatus.IsActive = true;
                        invoiceStatus.UpdatedBy = userId;
                        invoiceStatus.UpdatedDate = DateTimeOffset.Now;
                        invoice.InvoiceXInvoiceStatusDetails.Add(invoiceStatus);

                        Context.DataContext.Entry(invoice).State = EntityState.Modified;
                        await Context.CommitAsync();

                        await ContextFactory.Current.GetDomain<NotificationDomain>()
                                             .AddNotificationEventAsync(
                                                 EventType.InvoicePaid,
                                                 invoice.InvoiceHeaderId,
                                                 invoice.CreatedBy);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageInvoicePaidSuccess;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "PaidInvoiceAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> PayConfirmInvoiceAsync(InvoiceDetailViewModel viewModel, int userId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var invoice = await Context.DataContext.Invoices.SingleOrDefaultAsync(t => t.Id == viewModel.Invoice.Id);
                    if (invoice != null)
                    {
                        invoice.InvoiceXInvoiceStatusDetails.ToList().ForEach(t => t.IsActive = false);
                        invoice.PaymentDate = viewModel.Invoice.PaymentDate;

                        InvoiceXInvoiceStatusDetail invoiceStatus = new InvoiceXInvoiceStatusDetail();
                        invoiceStatus.StatusId = (int)InvoiceStatus.Confirmed;
                        invoiceStatus.IsActive = true;
                        invoiceStatus.UpdatedBy = userId;
                        invoiceStatus.UpdatedDate = DateTimeOffset.Now;
                        invoice.InvoiceXInvoiceStatusDetails.Add(invoiceStatus);

                        Context.DataContext.Entry(invoice).State = EntityState.Modified;
                        await Context.CommitAsync();

                        await ContextFactory.Current.GetDomain<NotificationDomain>()
                                             .AddNotificationEventAsync(
                                                 EventType.InvoicePayConfirmed,
                                                 invoice.InvoiceHeaderId,
                                                 invoice.CreatedBy);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessagePayInvoiceConfirmedSuccess;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "PayConfirmInvoiceAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> CancelDraftAsync(int invoiceId, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            DeliveryReqStatusUpdateModel deliveryReqStatus = null;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var data = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => new
                    {
                        Invoice = t,
                        Order = t.Order,
                        OrderPreviousStatus = t.Order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive),
                        OrderDroppedGallons = t.Order.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice).Sum(t1 => t1.DroppedGallons),
                        FuelRequest = t.Order.FuelRequest,
                        DeliveryTypeId = t.Order.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                        InvoicePreviousStatus = t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive),
                        TrackableSchedule = t.TrackableSchedule,
                        TimeZoneName = t.Order.FuelRequest.Job.TimeZoneName,
                        Job = t.Order.FuelRequest.Job
                    }).FirstOrDefaultAsync();
                    if (data.Invoice != null)
                    {
                        data.InvoicePreviousStatus.IsActive = false;

                        InvoiceXInvoiceStatusDetail invoiceStatus = new InvoiceXInvoiceStatusDetail();
                        invoiceStatus.StatusId = (int)InvoiceStatus.Canceled;
                        invoiceStatus.IsActive = true;
                        invoiceStatus.InvoiceId = invoiceId;
                        invoiceStatus.UpdatedBy = userContext.Id;
                        invoiceStatus.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.InvoiceXInvoiceStatusDetails.Add(invoiceStatus);

                        data.Invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;

                        if (data.Order != null)
                        {
                            UpdateHedgeAndSpotData(data.Invoice, data.Order.BuyerCompanyId, data.FuelRequest, data.Job);
                            AutoOpenOrder(data.Order, data.FuelRequest, data.DeliveryTypeId);

                            if (data.TrackableSchedule != null)
                            {
                                data.Invoice.TrackableScheduleId = null;
                                data.TrackableSchedule.DeliveryScheduleStatusId = GetDeliveryScheduleStatus(data.Invoice, data.TrackableSchedule, true);
                                if (!string.IsNullOrWhiteSpace(data.TrackableSchedule.FrDeliveryRequestId))
                                {
                                    deliveryReqStatus = new DeliveryReqStatusUpdateModel() { DeliveryRequestId = data.TrackableSchedule.FrDeliveryRequestId, ScheduleStatusId = data.TrackableSchedule.DeliveryScheduleStatusId, UserId = userContext.Id };
                                }
                            }
                        }

                        Context.DataContext.Entry(data.Invoice).State = EntityState.Modified;
                        await Context.CommitAsync();

                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetDigitalDropTicketCanceledNewsfeed(userContext, data.Invoice, data.TimeZoneName);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageCancelDraftSuccess;
                    }

                    transaction.Commit();
                    if (deliveryReqStatus != null)
                    {
                        new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "CancelDraftAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<InvoicePdfViewModel> GetInvoicePdfNewAsync(int invoiceId, CompanyType companyType)
        {
            InvoicePdfViewModel response = new InvoicePdfViewModel();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var pdfDetail = await spDomain.GetInvoicePdfDetailsAsync(invoiceId);
                if (pdfDetail != null)
                {
                    var helperDomain = new HelperDomain(this);
                    response = pdfDetail.ToInvoicePdfViewModel(response);
                    response.Invoice = pdfDetail.ToInvoiceViewModel();

                    response.Invoice.PricePerGallonDisplay = pdfDetail.FuelRequestPPG;
                    response.Invoice.IsHidePricingEnabled = companyType == CompanyType.Buyer ? pdfDetail.IsHidePricingEnabledForBuyer : pdfDetail.IsHidePricingEnabledForSupplier;

                    var fuelfees = await spDomain.GetInvoicePdfFuelFeesAsync(invoiceId);
                    response.FuelFees.FuelRequestFees = fuelfees.ToFeesViewModel();
                    if (response.FuelFees.FuelRequestFees != null && response.FuelFees.FuelRequestFees.Any())
                    {
                        foreach (var fee in response.FuelFees.FuelRequestFees)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && (fee.FeeSubQuantity != null && fee.FeeSubQuantity.Value != 0))
                            {
                                fee.TotalHours = GetHosingTimeInHours(fee.FeeSubQuantity.Value.ToString());
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.ByAssetCount && (fee.FeeSubQuantity != null && fee.FeeSubQuantity.Value != 0))
                            {
                                fee.TotalAssetQty = Convert.ToInt64(fee.FeeSubQuantity.Value);
                            }
                        }
                    }
                    if (pdfDetail.IsSurchargeApplicable)
                    {
                        response.FuelFees.FuelSurchargeFreightFee.IsSurchargeApplicable = pdfDetail.IsSurchargeApplicable;
                        response.FuelFees.FuelSurchargeFreightFee.SurchargeEiaPrice = pdfDetail.SurchargeEIAPrice ?? 0;
                        response.FuelFees.FuelSurchargeFreightFee.SurchargePercentage = pdfDetail.SurchargePercentage ?? 0;
                        response.FuelFees.FuelSurchargeFreightFee.SurchargePricingType = (FuelSurchagePricingType)pdfDetail.SurchargePricingType;
                    }

                    response.FuelFees.DiscountLineItems = fuelfees.ToDiscountFeesViewModel();
                    if (pdfDetail.IsThirdPartyHardwareUsed)
                    {
                        response.BrokeredOrder.BrokeredOrderFee = fuelfees.ToExternalBrokerViewModel();
                        //SetFreightFeeAmount(response.BrokeredOrder.BrokeredOrderFee, invoice);
                    }

                    if (pdfDetail.IsAssetDropAvailable)
                    {
                        var assetDrops = await spDomain.GetInvoicePdfAssetDropsAsync(invoiceId);
                        response.Assets = assetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Select(t => t.ToViewModel()).GroupBy(t => t.AssetName).Select(t => t.ToList()).ToList();
                        response.AssetNotAvailableCount = assetDrops.Count(t => t.DropStatus == (int)DropStatus.AssetNotAvailable);
                        response.NoFuelNeededAssetCount = assetDrops.Count(t => t.DropStatus == (int)DropStatus.NoFuelNeeded);
                        if (companyType != CompanyType.Buyer)
                        {
                            response.Assets.ForEach(t => t.ForEach(t1 => t1.SubcontractorName = string.Empty));
                        }
                    }

                    var taxDetails = await spDomain.GetInvoicePdfTaxDetailsAsync(invoiceId);
                    response.Invoice.TaxDetails.AvaTaxDetails = taxDetails.Select(t => new TaxDetailsViewModel()
                    {
                        Id = t.Id,
                        RateDescription = t.RateDescription,
                        TradingTaxAmount = t.TradingTaxAmount,
                        IsModified = t.IsModified
                    }).ToList();

                    if (pdfDetail.IsInstructionAvailable)
                    {
                        var specialInstructions = await spDomain.GetInvoicePdfSpecialInstructionsAsync(invoiceId);
                        response.SpecialInstructions = specialInstructions.Select(t => new InvoiceXSpecialInstructionViewModel()
                        {
                            Instruction = t.Instruction,
                            IsInstructionFollowed = t.IsInstructionFollowed
                        }).ToList();
                    }

                    if ((pdfDetail.StatusId == InvoiceStatus.PartiallyPaid || pdfDetail.StatusId == InvoiceStatus.Paid) && !string.IsNullOrWhiteSpace(pdfDetail.QbInvoiceNumber))
                    {
                        response.InvoicePayments = await GetInvoicePayments(pdfDetail.InvoiceNumberId, pdfDetail.QbInvoiceNumber);
                    }

                    // set culture
                    response.Culture = helperDomain.SetEntityThreadCulture(response.Invoice.Currency);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoicePdfNewAsync", ex.Message, ex);
            }

            return response;
        }
        public async Task<ConsolidatedInvoicePdfViewModel> GetConsolidatedBillDetailFromInvoiceIdAsync(int invoiceId)
        {
            var invoice = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => new { t.InvoiceHeaderId, t.BrokeredChainId, t.Order.AcceptedCompanyId, t.Order }).FirstOrDefaultAsync();
            var brokeredOrderDetails = await Context.DataContext.Invoices.Where(t => invoice.BrokeredChainId != null && t.BrokeredChainId == invoice.BrokeredChainId
                                                && t.Order.AcceptedCompanyId != invoice.AcceptedCompanyId)
                                        .Select(t => new BillMemoModel() { DisplayInvoiceNumber = t.DisplayInvoiceNumber, BuyerCompanyName = t.Order.BuyerCompany.Name, Order = t.Order, Invoiceid = t.Id }).OrderByDescending(t => t.Invoiceid).ToListAsync();
            var response = await GetConsolidatedInvoicePdfAsync(invoice.InvoiceHeaderId, CompanyType.Supplier);
            if (brokeredOrderDetails.Any())
            {
                SetMemoFields(response, invoice.Order, brokeredOrderDetails);
            }

            return response;
        }

        private void SetMemoFields(ConsolidatedInvoicePdfViewModel response, Order order, List<BillMemoModel> brokeredOrderDetails)
        {
            var parentOrder = GetParentOrder(order);
            if (parentOrder != null && parentOrder.Id != order.Id)
            {
                if (brokeredOrderDetails.Any(t => t.Order.Id == parentOrder.Id))
                {
                    var brokeredOrder = brokeredOrderDetails.FirstOrDefault(t => t.Order.Id == parentOrder.Id);
                    response.InvoicePdfHeaderDetail.BrokerCompany = brokeredOrder.BuyerCompanyName;
                    response.InvoicePdfHeaderDetail.BrokerInvoiceNumber = brokeredOrder.DisplayInvoiceNumber;
                }
                else
                {
                    SetMemoFields(response, parentOrder, brokeredOrderDetails);
                }
            }
        }

        public static Order GetParentOrder(Order order)
        {
            if (order.FuelRequest.FuelRequest1 != null && order.FuelRequest.FuelRequest1.GetParentFuelRequest() != null)
            {
                return order.FuelRequest.FuelRequest1.GetParentFuelRequest().Orders.LastOrDefault();
            }
            return order;
        }

        public async Task<ConsolidatedInvoicePdfViewModel> GetConsolidatedInvoicePdfFromInvoiceIdAsync(int invoiceId, CompanyType companyType)
        {
            var headerId = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => t.InvoiceHeaderId).FirstOrDefaultAsync();
            return await GetConsolidatedInvoicePdfAsync(headerId, companyType);
        }

        public async Task<ConsolidatedInvoicePdfViewModel> GetConsolidatedInvoicePdfAsync(int invoiceHeaderId, CompanyType companyType, UserContext userContext = null)
        {
            if (invoiceHeaderId == 0)
                return null;
            ConsolidatedInvoicePdfViewModel response = new ConsolidatedInvoicePdfViewModel();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var pdfDetail = await spDomain.GetConsolidatedInvoicePdfAsync(invoiceHeaderId);
                if (pdfDetail != null)
                {
                    var helperDomain = new HelperDomain(this);


                    // set invoice pdf header summary
                    response.InvoicePdfHeaderDetail = pdfDetail.InvoiceHeaderDetail.ToViewModel();
                    response.InvoicePdfHeaderDetail.InvoiceHeaderId = invoiceHeaderId;
                    // set invoice fuel and drop details
                    response.Invoices = pdfDetail.InvoiceDropDetails.Select(t => t.ToInvoiceViewModel(companyType)).ToList();

                    // set lift details
                    if (pdfDetail.LiftDetails.Any())
                    {
                        response.LiftDetails = pdfDetail.LiftDetails.Select(t => t.ToLiftTicketViewModel()).ToList();
                        FilterDuplicateLiftDetails(response);
                    }

                    // set bol details
                    if (pdfDetail.BolDetails.Any())
                    {
                        response.BolDetails = pdfDetail.BolDetails.Select(t => t.ToBolViewModel()).ToList();
                        FilterDuplicateBOLDetails(response);
                    }

                    // set pickup locations
                    if (pdfDetail.PickupLocations.Any())
                        response.PickupLocations = pdfDetail.PickupLocations.Select(t => t.ToPickUpLocation()).ToList();

                    // set invoice fuel fees 
                    if (pdfDetail.FuelFeeDetails.Any())
                    {
                        response.FuelFees.FuelRequestFees = pdfDetail.FuelFeeDetails.ToFeesViewModel();

                        // update fuel fees
                        //if true, round up fee and total fee amount to 6 decimals
                        var isMarineLocation = response.Invoices.FirstOrDefault().IsMarineLocation;
                        UpdateFuelFees(response.FuelFees, isMarineLocation, response.InvoicePdfHeaderDetail.SupplierLocation.CountryId);

                        // set discounted line items
                        response.FuelFees.DiscountLineItems = pdfDetail.FuelFeeDetails.ToDiscountFeesViewModel();
                    }

                    // set asset details
                    if (pdfDetail.AssetDetails.Any())
                    {
                        response.Assets = pdfDetail.AssetDetails.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Select(t => t.ToViewModel()).GroupBy(t => new { t.AssetName, t.OrderId, t.InvoiceId }).Select(t => t.ToList()).ToList();
                        response.AssetNotAvailableCount = pdfDetail.AssetDetails.Count(t => t.DropStatus == (int)DropStatus.AssetNotAvailable);
                        response.NoFuelNeededAssetCount = pdfDetail.AssetDetails.Count(t => t.DropStatus == (int)DropStatus.NoFuelNeeded);
                        if (companyType != CompanyType.Buyer)
                        {
                            response.Assets.ForEach(t => t.ForEach(t1 => t1.SubcontractorName = string.Empty));
                        }
                    }

                    // set tax details
                    if (pdfDetail.TaxDetails.Any())
                    {

                        response.TaxDetail = new InvoiceTaxDetailsViewModel();
                        var distinctTaxes = pdfDetail.TaxDetails.GroupBy(x => new { x.TaxPricingTypeId, x.RateDescription }).Select(group => group.ToList()).ToList();
                        foreach (var tax in distinctTaxes)
                        {
                            TaxDetailsViewModel taxDetailsViewModel = new TaxDetailsViewModel();
                            taxDetailsViewModel.TaxPricingTypeId = tax.Select(y => y.TaxPricingTypeId).FirstOrDefault();
                            taxDetailsViewModel.TradingTaxAmount = tax.Select(y => y.TradingTaxAmount).Sum();
                            taxDetailsViewModel.RateDescription = tax.Select(y => y.RateDescription).FirstOrDefault();
                            taxDetailsViewModel.IsModified = tax.Select(y => y.IsModified).FirstOrDefault();
                            response.TaxDetail.AvaTaxDetails.Add(taxDetailsViewModel);
                        }
                        response.TaxDetail.TotalTaxAmount = response.Invoices.Sum(t => t.TotalTaxAmount); //pdfDetail.TaxDetails.Sum(t => t.TradingTaxAmount);
                    }

                    // set special instructions
                    if (pdfDetail.SpecialInstructions.Any())
                    {
                        response.SpecialInstructions = pdfDetail.SpecialInstructions.Select(t => new InvoiceXSpecialInstructionViewModel()
                        {
                            Instruction = t.Instruction,
                            IsInstructionFollowed = t.IsInstructionFollowed
                        }).ToList();
                    }
                    response.PoNumbers = response.Invoices.Select(t => t.PoNumber).Distinct().ToList();
                    response.InvoicePdfHeaderDetail.EditedInvoiceNote = await SetEditedInvoiceNote(invoiceHeaderId, companyType, userContext);

                    response.DropTicketNumbers = response.Invoices.Where(t => !string.IsNullOrEmpty(t.AdditionalDetail.DropTicketNumber)
                                                    && t.AdditionalDetail.DropTicketNumber != "--").Select(t => t.AdditionalDetail.DropTicketNumber).Distinct().ToList();
                    // set culture
                    response.Culture = helperDomain.SetEntityThreadCulture(response.InvoicePdfHeaderDetail.Currency);

                    if (!string.IsNullOrEmpty(response.InvoicePdfHeaderDetail.DeliveryRequestId))
                    {
                        await SetRouteInformation(response);
                    }

                    // get invoice footer details
                    if (!string.IsNullOrEmpty(response.InvoicePdfHeaderDetail.InvoiceFooterJson))
                    {
                        var pdfFooterDetailList = JsonConvert.DeserializeObject<InvoicePdfFooterViewModel>(response.InvoicePdfHeaderDetail.InvoiceFooterJson);
                        if (pdfFooterDetailList != null)
                        {
                            var pdfFooterDetail = pdfFooterDetailList.InvoicePdfFooterList.FirstOrDefault(t => t.CompanyId == response.InvoicePdfHeaderDetail.SupplierCompanyId);
                            if (pdfFooterDetail != null)
                            {
                                response.InvoiceFooter = new PdfFooterModel()
                                {
                                    CompanyId = pdfFooterDetail.CompanyId,
                                    Description = pdfFooterDetail.Description,
                                    BankingInstructions = pdfFooterDetail.BankingInstructions,
                                    AdditionalDetails = pdfFooterDetail.AdditionalDetails,
                                    CompanyName = pdfFooterDetail.CompanyName
                                };

                                var appDomain = new ApplicationDomain();
                                var siteFuelExchangeUrl = appDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingSiteFuelExchangeUrl);
                                response.InvoiceFooter.QRCodePath = siteFuelExchangeUrl + "/Content/images/QRCodeTropic.png";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetConsolidatedInvoicePdfAsync", ex.Message, ex);
            }

            return response;

        }

        //public async Task<BDRPdfViewModel> GetProformaBDNPdfAsync(int orderId, CompanyType companyType, UserContext userContext = null)
        //{
        //    if (orderId == 0)
        //        return null;
        //    BDRPdfViewModel response = new BDRPdfViewModel();
        //    try
        //    {
        //        var spDomain = new StoredProcedureDomain(this);
        //        var pdfDetail = await spDomain.GetProformaBDNPdfAsync(orderId);
        //        if (pdfDetail != null)
        //        {
        //            var helperDomain = new HelperDomain(this);

        //            // set invoice pdf header summary
        //            response.InvoicePdfHeaderDetail = pdfDetail.InvoiceHeaderDetail.ToViewModel();
        //            response.InvoicePdfHeaderDetail.InvoiceHeaderId = orderId;
        //            // set invoice fuel and drop details
        //            response.Invoices = pdfDetail.InvoiceDropDetails.Select(t => t.ToInvoiceViewModel(companyType)).ToList();

        //            // set pickup locations
        //            if (pdfDetail.PickupLocations.Any())
        //                response.PickupLocations = pdfDetail.PickupLocations.Select(t => t.ToPickUpLocation()).ToList();

        //            response.PoNumbers = response.Invoices.Select(t => t.PoNumber).Distinct().ToList();
        //            response.DropTicketNumbers = response.Invoices.Where(t => !string.IsNullOrEmpty(t.AdditionalDetail.DropTicketNumber)
        //                                            && t.AdditionalDetail.DropTicketNumber != "--").Select(t => t.AdditionalDetail.DropTicketNumber).Distinct().ToList();

        //            if (pdfDetail.BDRDetailsModel != null && pdfDetail.BDRDetailsModel.Any())
        //            {
        //                BdnConsolidationCalculation(pdfDetail, response);
        //            }
        //            // set culture
        //            response.Culture = helperDomain.SetEntityThreadCulture(response.InvoicePdfHeaderDetail.Currency);

        //            response.BDNImages = await GetMarineBDNImages(orderId);

        //            // get invoice footer details
        //            if (!string.IsNullOrEmpty(response.InvoicePdfHeaderDetail.InvoiceFooterJson))
        //            {
        //                var pdfFooterDetailList = JsonConvert.DeserializeObject<InvoicePdfFooterViewModel>(response.InvoicePdfHeaderDetail.InvoiceFooterJson);
        //                if (pdfFooterDetailList != null)
        //                {
        //                    var pdfFooterDetail = pdfFooterDetailList.InvoicePdfFooterList.FirstOrDefault(t => t.CompanyId == response.InvoicePdfHeaderDetail.SupplierCompanyId);
        //                    if (pdfFooterDetail != null)
        //                    {
        //                        response.InvoiceFooter = new PdfFooterModel()
        //                        {
        //                            CompanyId = pdfFooterDetail.CompanyId,
        //                            Description = pdfFooterDetail.Description,
        //                            BankingInstructions = pdfFooterDetail.BankingInstructions,
        //                            AdditionalDetails = pdfFooterDetail.AdditionalDetails,
        //                            CompanyName = pdfFooterDetail.CompanyName
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Logger.WriteException("InvoiceDomain", "GetProformaBDNPdfAsync", ex.Message, ex);
        //    }

        //    return response;
        //}

        public async Task<BDRPdfViewModel> GetBDRPdfAsync(int invoiceHeaderId, CompanyType companyType, UserContext userContext = null)
        {
            if (invoiceHeaderId == 0)
                return null;
            BDRPdfViewModel response = new BDRPdfViewModel();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var pdfDetail = await spDomain.GetBDRPdfAsync(invoiceHeaderId);
                if (pdfDetail != null)
                {
                    var helperDomain = new HelperDomain(this);

                    // set invoice pdf header summary
                    response.InvoicePdfHeaderDetail = pdfDetail.InvoiceHeaderDetail.ToViewModel();
                    response.InvoicePdfHeaderDetail.InvoiceHeaderId = invoiceHeaderId;
                    // set invoice fuel and drop details
                    response.Invoices = pdfDetail.InvoiceDropDetails.Select(t => t.ToInvoiceViewModel(companyType)).ToList();

                    // set pickup locations
                    if (pdfDetail.PickupLocations.Any())
                        response.PickupLocations = pdfDetail.PickupLocations.Select(t => t.ToPickUpLocation()).ToList();

                    response.PoNumbers = response.Invoices.Select(t => t.PoNumber).Distinct().ToList();
                    response.DropTicketNumbers = response.Invoices.Where(t => !string.IsNullOrEmpty(t.AdditionalDetail.DropTicketNumber)
                                                    && t.AdditionalDetail.DropTicketNumber != "--").Select(t => t.AdditionalDetail.DropTicketNumber).Distinct().ToList();

                    if (pdfDetail.BDRDetailsModel != null && pdfDetail.BDRDetailsModel.Any())
                    {
                        BdnConsolidationCalculation(pdfDetail, response);
                    }
                    // set culture
                    response.Culture = helperDomain.SetEntityThreadCulture(response.InvoicePdfHeaderDetail.Currency);

                    response.BDNImages = await GetMarineBDNImages(invoiceHeaderId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetBDRPdfAsync", ex.Message, ex);
            }

            return response;
        }

        private void BdnConsolidationCalculation(UspBDRPdfDetail pdfDetail, BDRPdfViewModel response)
        {
            if (pdfDetail != null && response != null && response.BDRDetailsModel != null)
            {
                response.BDRDetailsModel = pdfDetail.BDRDetailsModel.FirstOrDefault();
                response.BDRDetailsModel.MarpolSampleNumbers = String.Join(",", pdfDetail.BDRDetailsModel.Select(s => s.MarpolSampleNumbers).Distinct().ToList());
                response.BDRDetailsModel.MVMarpolSampleNumbers = String.Join(",", pdfDetail.BDRDetailsModel.Select(s => s.MVMarpolSampleNumbers).Distinct().ToList());
                decimal consolidatedTemp = 0, consolidatedSulfurContent = 0, consolidatedViscocity = 0, consolidatedFlashPoint = 0, consolidatedDensityInVacuum = 0;
                decimal denominatorTotalDropQty = 0;

                foreach (var bdn in pdfDetail.BDRDetailsModel)
                {
                    decimal.TryParse(bdn.ObservedTemperature, out decimal t1);
                    consolidatedTemp += pdfDetail.InvoiceDropDetails.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons) * t1;

                    decimal.TryParse(bdn.SulphurContent, out decimal s1);
                    consolidatedSulfurContent += pdfDetail.InvoiceDropDetails.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons) * s1;

                    decimal.TryParse(bdn.Viscosity, out decimal v1);
                    consolidatedViscocity += pdfDetail.InvoiceDropDetails.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons) * v1;

                    decimal.TryParse(bdn.FlashPoint, out decimal f1);
                    consolidatedFlashPoint += pdfDetail.InvoiceDropDetails.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons) * f1;

                    decimal.TryParse(bdn.DensityInVaccum, out decimal d1);
                    consolidatedDensityInVacuum += pdfDetail.InvoiceDropDetails.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons) * d1;

                    denominatorTotalDropQty += pdfDetail.InvoiceDropDetails.Where(t => t.Id == bdn.InvoiceId).Sum(t => t.DroppedGallons);


                }

                if (denominatorTotalDropQty > 0)
                {
                    if (pdfDetail.BDRDetailsModel.Any(t => t.ObservedTemperature == null || t.ObservedTemperature == ""))
                        response.BDRDetailsModel.ObservedTemperature = Resource.messageNA;
                    else
                        response.BDRDetailsModel.ObservedTemperature = (consolidatedTemp / denominatorTotalDropQty).GetCommaSeperatedValue();

                    if (pdfDetail.BDRDetailsModel.Any(t => t.SulphurContent == null || t.SulphurContent == ""))
                        response.BDRDetailsModel.SulphurContent = Resource.messageNA;
                    else
                        response.BDRDetailsModel.SulphurContent = (consolidatedSulfurContent / denominatorTotalDropQty).ToString($"#,##.###");

                    if (pdfDetail.BDRDetailsModel.Any(t => t.Viscosity == null || t.Viscosity == ""))
                        response.BDRDetailsModel.Viscosity = Resource.messageNA;
                    else
                        response.BDRDetailsModel.Viscosity = (consolidatedViscocity / denominatorTotalDropQty).GetCommaSeperatedValue();

                    if (pdfDetail.BDRDetailsModel.Any(t => t.FlashPoint == null || t.FlashPoint == ""))
                        response.BDRDetailsModel.FlashPoint = Resource.messageNA;
                    else
                        response.BDRDetailsModel.FlashPoint = (consolidatedFlashPoint / denominatorTotalDropQty).GetCommaSeperatedValue();

                    if (pdfDetail.BDRDetailsModel.Any(t => t.DensityInVaccum == null || t.DensityInVaccum == ""))
                        response.BDRDetailsModel.DensityInVaccum = Resource.messageNA;
                    else
                        response.BDRDetailsModel.DensityInVaccum = (consolidatedDensityInVacuum / denominatorTotalDropQty).GetCommaSeperatedValue();

                    // calculate consolidated API Gravity
                    var consolidatedApiGravity = pdfDetail.InvoiceDropDetails.Sum(t => ((t.Gravity ?? 0) * t.DroppedGallons));
                    response.CalculatedAPIGravity = (consolidatedApiGravity / denominatorTotalDropQty).GetCommaSeperatedValue();
                }
            }
        }

        private void FilterDuplicateBOLDetails(ConsolidatedInvoicePdfViewModel response)
        {
            if (response != null && response.BolDetails != null && response.BolDetails.Any())
            {
                var gruoupByBol = response.BolDetails.GroupBy(t => new { t.BolNumber, t.TerminalName, t.FuelTypeId }).ToList();
                response.BolDetails.Clear();
                foreach (var item in gruoupByBol)
                {
                    response.BolDetails.Add(item.FirstOrDefault());
                }
            }
        }

        private void FilterDuplicateLiftDetails(ConsolidatedInvoicePdfViewModel response)
        {
            if (response != null && response.LiftDetails != null && response.LiftDetails.Any())
            {
                var gruoupByLift = response.LiftDetails.GroupBy(t => new { t.LiftTicketNumber, t.TerminalName, t.FuelTypeId }).ToList();
                response.LiftDetails.Clear();
                foreach (var item in gruoupByLift)
                {
                    response.LiftDetails.Add(item.FirstOrDefault());
                }
            }
        }

        private static async Task SetRouteInformation(ConsolidatedInvoicePdfViewModel response)
        {
            List<string> delReqId = response.InvoicePdfHeaderDetail.DeliveryRequestId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (delReqId.Count > 0)
            {
                var sbDomain = ContextFactory.Current.GetDomain<ScheduleBuilderDomain>();
                var invoiceRouteInfo = await sbDomain.GetInvoiceRouteInfo(delReqId);
                if (invoiceRouteInfo.Count > 0)
                {
                    string routeName = string.Empty;
                    foreach (var item in invoiceRouteInfo)
                    {
                        if (string.IsNullOrEmpty(routeName))
                            routeName = item.Name;
                        else
                            routeName += "," + item.Name;
                    }
                    response.InvoicePdfHeaderDetail.RouteName = routeName;
                }
            }
        }

        private async Task<string> SetEditedInvoiceNote(int invoiceHeaderId, CompanyType companyType, UserContext userContext)
        {
            var editInvoiceText = "";
            try
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.Version > 1)
                                                                .Select(t => new { InvoiceId = t.Id, t.InvoiceTypeId, t.ParentId, InvoiceNumberId = t.InvoiceHeader.InvoiceNumber.Id, t.Version, t.Order.Company.CompanyTypeId })
                                                                .FirstOrDefaultAsync();
                if (invoice != null)
                {
                    var previousInvoice = await Context.DataContext.Invoices.Where(t => ((t.Id == invoice.ParentId || t.InvoiceHeader.InvoiceNumber.Id == invoice.InvoiceNumberId) && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive && t.Version == (invoice.Version - 1)))
                                                                            .OrderByDescending(t => t.Id)
                                                                            .Select(t => new { InvoiceId = t.Id, DisplayInvoiceNumber = t.DisplayInvoiceNumber, t.InvoiceTypeId })
                                                                            .FirstOrDefaultAsync();

                    string url = "";
                    if (previousInvoice != null)
                    {
                        string area = CompanyType.Supplier.ToString();
                        var baseUrl = new ApplicationDomain().GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingSiteFuelExchangeUrl);
                        if (HttpContext.Current != null)
                            area = Convert.ToString(HttpContext.Current?.Request?.RequestContext?.RouteData?.DataTokens["area"]);
                        var ddtOrInvoiceText = (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) ? Resource.lblDDT : Resource.lblInvoice;
                        if (userContext != null && userContext.ActingCompanyType.ToLower().Equals("supplier"))
                        {
                            url = $"{baseUrl}{((area == null || area == "") ? CompanyType.Supplier.ToString() : area)}/Invoice/Details/{invoice.InvoiceId}?isShowHistory=true";
                            editInvoiceText = string.Format(Resource.messageEditedInvoiceText, ddtOrInvoiceText, url);
                        }
                        else
                        {
                            url = "<a href=javascript:void(0) onclick=showHistoryPanelsection()>click here</a>";
                            editInvoiceText = string.Format(Resource.messageEditedInvoiceTextNonSupplier, ddtOrInvoiceText, url);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "SetEditedInvoiceNote", ex.Message + " InvoiceHeaderId => " + invoiceHeaderId, ex);
            }
            return editInvoiceText;
        }

        public async Task<int> GetInvoiceHeaderIdByIdAsync(int invoiceId)
        {
            var invoiceHeaderId = 0;
            try
            {
                invoiceHeaderId = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => t.InvoiceHeaderId).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceHeaderIdByIdAsync", ex.Message, ex);
                invoiceHeaderId = 0;
            }

            return invoiceHeaderId;
        }

        public void UpdateFuelFees(FuelFeesViewModel fuelFeesModel, bool isMarineLocation = false,int companyCountryId = 1)
        {
            try
            {
                var fees = fuelFeesModel.FuelRequestFees.Where(t => t.FeeSubTypeId != (int)FeeSubType.NoFee).ToList();
                if (!fees.Any())
                    return;

                foreach (var fee in fees)
                {
                    fee.DisplayTotalFee = (fee.TotalFee).GetInvoiceAmountValue(2, @Resource.constSymbolCurrency);
                    if (fee.FeeTypeId == ((int)FeeType.FreightFee).ToString())
                    {
                        if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon && !fee.IncludeInPPG)
                        {
                            var feeSubQuantity = fee.FeeSubQuantity == null ? 0 : fee.FeeSubQuantity.Value;
                            fee.DisplayFeeType = Resource.lblFreight;
                            fee.DisplayFeeName = fee.FeeSubTypeName;
                            fee.DisplayDroppedGallons = feeSubQuantity.GetPreciseValue(2).GetCommaSeperatedValue();
                            fee.DisplayFee = Resource.constSymbolCurrency + ((fee.Fee ?? 0).ToString(ApplicationConstants.DecimalFormat4));
                        }
                        else if ((fee.FeeSubTypeId == (int)FeeSubType.ByAssetCount || fee.FeeSubTypeId == (int)FeeSubType.PerRoute || fee.FeeSubTypeId == (int)FeeSubType.FlatFee) && !fee.IncludeInPPG)
                        {
                            fee.DisplayFeeType = Resource.lblFreight;
                            fee.DisplayFeeName = fee.FeeSubTypeName;
                            fee.DisplayDroppedGallons = fee.FeeSubQuantity == 0 || fee.FeeSubQuantity == null ? "1" : fee.FeeSubQuantity.GetPreciseValue(0).ToString();
                            fee.DisplayFee = Resource.constSymbolCurrency + (fee.Fee ?? 0).ToString(ApplicationConstants.DecimalFormat4);
                        }
                    }
                    else if (!fee.FeeTypeId.Contains(Constants.OtherCommonFeeCode))
                    {
                        var feeTypeId = Convert.ToInt32(fee.FeeTypeId);
                        if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && feeTypeId == (int)FeeType.DeliveryFee)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                fee.DisplayFeeType = Resource.lblDeliveryFeeSummary;
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = fee.InvoiceTypeId == (int)InvoiceType.CreditInvoice || fee.InvoiceTypeId == (int)InvoiceType.PartialCredit ? "-1" : "1";
                                fee.DisplayFee = Resource.constSymbolCurrency + Math.Abs(fee.TotalFee).ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon)
                            {
                                fee.DisplayFeeType = Resource.lblDeliveryFeeSummary;
                                fee.DisplayFeeName = fee.FeeSubTypeName;
                                var feeSubQuantity = fee.FeeSubQuantity == null ? 0 : fee.FeeSubQuantity.Value;
                                fee.DisplayDroppedGallons = feeSubQuantity.GetPreciseValue(2).GetCommaSeperatedValue();
                                //fee.DisplayDroppedGallons = fee.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                fee.DisplayFeeType = Resource.lblDeliveryFeeSummary;
                                fee.DisplayFeeName = Resource.lblHourlyRate;
                                fee.DisplayDroppedGallons = fee.TotalHours;
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else
                            {
                                fee.DisplayFeeType = Resource.lblDeliveryFeeSummary;
                                fee.DisplayFeeName = Resource.lblByQuantity;
                                fee.DisplayDroppedGallons = "";
                                fee.DisplayFee = "";
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee &&
                                (feeTypeId == (int)FeeType.WetHoseFee || feeTypeId == (int)FeeType.OverWaterFee) &&
                                (fee.Fee != null && fee.Fee.Value != 0))
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
                            {
                                fee.DisplayFeeType = feeTypeId == (int)FeeType.WetHoseFee ? Resource.lblWetHoseFee : Resource.lblOverWaterFee;
                                fee.DisplayFeeName = Resource.lblPoFeeByAssetCount;
                                fee.DisplayDroppedGallons = (fee.FeeSubQuantity ?? 0).GetPreciseValue(0).ToString() + " " + Resource.lblAssetsCount;
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                fee.DisplayFeeType = feeTypeId == (int)FeeType.WetHoseFee ? Resource.lblWetHoseFee : Resource.lblOverWaterFee;
                                fee.DisplayFeeName = Resource.lblHourlyRate;
                                fee.DisplayDroppedGallons = fee.GetQuntityInTime();
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                fee.DisplayFeeType = feeTypeId == (int)FeeType.WetHoseFee ? Resource.lblWetHoseFee : Resource.lblOverWaterFee;
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = fee.FeeSubQuantity == 0 || fee.FeeSubQuantity == null ? "1" : fee.FeeSubQuantity.GetPreciseValue(0).ToString();
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && feeTypeId == (int)FeeType.UnderGallonFee && fee.MinimumGallons.Value > fee.DroppedGallons)
                        {
                            fee.DisplayFeeType = Resource.lblMinimumGallonFee;
                            fee.DisplayFeeName = Resource.lblFlatFee;
                            fee.DisplayDroppedGallons = fee.InvoiceTypeId == (int)InvoiceType.CreditInvoice || fee.InvoiceTypeId == (int)InvoiceType.PartialCredit ? "-1" : "1";
                            fee.DisplayFee = Resource.constSymbolCurrency + Math.Abs(fee.TotalFee).ToString(ApplicationConstants.DecimalFormat4);
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && feeTypeId == (int)FeeType.EnvironmentalFee && fee.TotalFee != 0)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                fee.DisplayFeeType = Resource.lblEnvironmentalFee;
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = fee.InvoiceTypeId == (int)InvoiceType.CreditInvoice || fee.InvoiceTypeId == (int)InvoiceType.PartialCredit ? "-1" : "1";
                                fee.DisplayFee = Resource.constSymbolCurrency + Math.Abs(fee.TotalFee).ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon)
                            {
                                var feeSubQuantity = fee.FeeSubQuantity == null ? 0 : fee.FeeSubQuantity.Value;
                                fee.DisplayDroppedGallons = feeSubQuantity.GetPreciseValue(2).GetCommaSeperatedValue();

                                fee.DisplayFeeType = Resource.lblEnvironmentalFee;
                                fee.DisplayFeeName = fee.FeeSubTypeName;
                                //fee.DisplayDroppedGallons = fee.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && (feeTypeId == (int)FeeType.LoadFee || feeTypeId == (int)FeeType.AdditiveFee) && fee.TotalFee != 0)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                fee.DisplayFeeType = ((FeeType)feeTypeId).GetDisplayName();
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = fee.InvoiceTypeId == (int)InvoiceType.CreditInvoice || fee.InvoiceTypeId == (int)InvoiceType.PartialCredit ? "-1" : "1";
                                fee.DisplayFee = Resource.constSymbolCurrency + Math.Abs(fee.TotalFee).ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon)
                            {
                                var feeSubQuantity = fee.FeeSubQuantity == null ? 0 : fee.FeeSubQuantity.Value;
                                fee.DisplayDroppedGallons = feeSubQuantity.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayFeeType = ((FeeType)feeTypeId).GetDisplayName();
                                //fee.DisplayFeeName = Resource.lblPerGallon;
                                fee.DisplayFeeName = fee.FeeSubTypeName;
                                //fee.DisplayDroppedGallons = fee.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && feeTypeId == (int)FeeType.ServiceFee && fee.TotalFee != 0)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                fee.DisplayFeeType = Resource.lblServiceFee;
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = fee.InvoiceTypeId == (int)InvoiceType.CreditInvoice || fee.InvoiceTypeId == (int)InvoiceType.PartialCredit ? "-1" : "1";
                                fee.DisplayFee = Resource.constSymbolCurrency + Math.Abs(fee.TotalFee).ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon)
                            {
                                var feeSubQuantity = fee.FeeSubQuantity == null ? 0 : fee.FeeSubQuantity.Value;
                                fee.DisplayFeeType = Resource.lblServiceFee;
                                //fee.DisplayFeeName = Resource.lblPerGallon;
                                fee.DisplayFeeName = fee.FeeSubTypeName;
                                // fee.DisplayDroppedGallons = fee.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayDroppedGallons = feeSubQuantity.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                fee.DisplayFeeType = Resource.lblServiceFee;
                                fee.DisplayFeeName = Resource.lblHourlyRate;
                                fee.DisplayDroppedGallons = fee.TotalHours;
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && feeTypeId == (int)FeeType.SurchargeFee && fee.TotalFee != 0)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                fee.DisplayFeeType = Resource.lblSurcharge;
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = fee.InvoiceTypeId == (int)InvoiceType.CreditInvoice || fee.InvoiceTypeId == (int)InvoiceType.PartialCredit ? "-1" : "1";
                                fee.DisplayFee = Resource.constSymbolCurrency + Math.Abs(fee.TotalFee).ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon)
                            {
                                var feeSubQuantity = fee.FeeSubQuantity == null ? 0 : fee.FeeSubQuantity.Value;
                                fee.DisplayFeeType = Resource.lblSurcharge;
                                //fee.DisplayFeeName = Resource.lblPerGallon;
                                fee.DisplayFeeName = fee.FeeSubTypeName;
                                fee.DisplayDroppedGallons = feeSubQuantity.GetPreciseValue(2).GetCommaSeperatedValue();
                                // fee.DisplayDroppedGallons = fee.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                fee.DisplayFeeType = Resource.lblSurcharge;
                                fee.DisplayFeeName = Resource.lblHourlyRate;
                                fee.DisplayDroppedGallons = fee.TotalHours;
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee &&
                                (feeTypeId == (int)FeeType.StopOffFee || feeTypeId == (int)FeeType.SplitTank || feeTypeId == (int)FeeType.PumpCharge || feeTypeId == (int)FeeType.Retain)
                                && fee.TotalFee != 0)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                fee.DisplayFeeType = fee.FeeTypeName;
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = fee.InvoiceTypeId == (int)InvoiceType.CreditInvoice || fee.InvoiceTypeId == (int)InvoiceType.PartialCredit ? "-1" : "1";
                                fee.DisplayFee = Resource.constSymbolCurrency + Math.Abs(fee.TotalFee).ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon)
                            {
                                var feeSubQuantity = fee.FeeSubQuantity == null ? 0 : fee.FeeSubQuantity.Value;
                                fee.DisplayFeeType = fee.FeeTypeName;
                                // fee.DisplayFeeName = Resource.lblPerGallon;
                                fee.DisplayFeeName = fee.FeeSubTypeName;
                                fee.DisplayDroppedGallons = feeSubQuantity.GetPreciseValue(2).GetCommaSeperatedValue();
                                //  fee.DisplayDroppedGallons = fee.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                fee.DisplayFeeType = fee.FeeTypeName;
                                fee.DisplayFeeName = Resource.lblHourlyRate;
                                fee.DisplayDroppedGallons = fee.TotalHours;
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && feeTypeId == (int)FeeType.DemurrageFeeDestination && fee.TotalFee != 0)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                fee.DisplayFeeType = Resource.lblDemurrageFeeDestination;
                                fee.DisplayFeeName = Resource.lblHourlyRate;
                                fee.DisplayDroppedGallons = fee.TotalHours;
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else
                            {
                                fee.DisplayFeeType = Resource.lblDemurrageFeeDestination;
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = "1";
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && feeTypeId == (int)FeeType.DemurrageFeeTerminal && fee.TotalFee != 0)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                fee.DisplayFeeType = Resource.lblDemurrageFeeTerminal;
                                fee.DisplayFeeName = Resource.lblHourlyRate;
                                fee.DisplayDroppedGallons = fee.TotalHours;
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                                fee.DisplayTotalFee = (fee.TotalFee).GetInvoiceAmountValue(2, Resource.constSymbolCurrency);
                            }
                            else
                            {
                                fee.DisplayFeeType = Resource.lblDemurrageFeeTerminal;
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = "1";
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && feeTypeId == (int)FeeType.DemurrageOther && fee.TotalFee != 0)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                fee.DisplayFeeType = Resource.lblDemurrageOther;
                                fee.DisplayFeeName = Resource.lblHourlyRate;
                                fee.DisplayDroppedGallons = fee.TotalHours;
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else
                            {
                                fee.DisplayFeeType = Resource.lblDemurrageOther;
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = string.Format(Resource.constMinuteFormat, fee.TimeInMinutes == 0 || fee.TimeInMinutes == null ? "1" : fee.TimeInMinutes.Value.ToString());
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && feeTypeId == (int)FeeType.SurchargeFreightFee && fee.TotalFee != 0)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.ByDistance || fee.FeeSubTypeId == (int)FeeSubType.ByQuantity || fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                var surchargePerc = fee.SurchargePercentage ?? 0;
                                fee.DisplayFeeType = Resource.lblFuelSurcharge + " " + ((FuelSurchagePricingType)fee.SurchargePricingType).GetDisplayName() + " price from EIA";
                                fee.DisplayFeeName = "FSC Rate " + surchargePerc.ToString(ApplicationConstants.DecimalFormat2) + Resource.constSymbolPercent + ", Base Freight " + Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                                fee.DisplayDroppedGallons = fee.DroppedGallons.GetCommaSeperatedValue();
                                fee.DisplayFee = Resource.constSymbolCurrency + (fee.TotalFee / fee.DroppedGallons).ToString(ApplicationConstants.DecimalFormat4);
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && feeTypeId == (int)FeeType.FreightCost && fee.TotalFee != 0)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                fee.DisplayFeeType = Resource.lblFreight + " Cost";
                                if (fee.FreightRateRuleType.HasValue && fee.FreightRateRuleType.Value == (int)FreightRateRuleType.Range)
                                {
                                    var uoD = (companyCountryId == (int)Country.CAN ? Resource.lblKiloMeters : Resource.lblMiles.ToLower());
                                    var distance = " Distance: " + (fee.Distance.HasValue ? Convert.ToString(fee.Distance.Value.ToString(ApplicationConstants.DecimalFormat2)) + " " + uoD : Resource.lblSingleHyphen);
                                    fee.DisplayFeeName = "Freight Type : " + (fee.FreightRateRuleType.HasValue ? ((FreightRateRuleType)fee.FreightRateRuleType.Value).GetDisplayName() + distance : Resource.lblHyphen);
                                }
                                else
                                {
                                    fee.DisplayFeeName = "Freight Type : " + (fee.FreightRateRuleType.HasValue ? ((FreightRateRuleType)fee.FreightRateRuleType.Value).GetDisplayName() : Resource.lblHyphen);
                                }
                                fee.DisplayDroppedGallons = fee.DroppedGallons.GetCommaSeperatedValue();
                                fee.DisplayFee = Resource.constSymbolCurrency + (fee.TotalFee / fee.DroppedGallons).ToString(ApplicationConstants.DecimalFormat4);
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee && feeTypeId == (int)FeeType.OtherFee && fee.TotalFee != 0)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : Resource.lblOtherFee + " - " + fee.OtherFeeDescription;
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = fee.InvoiceTypeId == (int)InvoiceType.CreditInvoice || fee.InvoiceTypeId == (int)InvoiceType.PartialCredit ? "-1" : "1";
                                fee.DisplayFee = Resource.constSymbolCurrency + Math.Abs(fee.TotalFee).ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon)
                            {
                                var feeSubQuantity = fee.FeeSubQuantity == null ? 0 : fee.FeeSubQuantity.Value;
                                fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : Resource.lblOtherFee + " - " + fee.OtherFeeDescription;
                                fee.DisplayFeeName = fee.FeeSubTypeName;
                                //fee.DisplayDroppedGallons = fee.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayDroppedGallons = feeSubQuantity.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : Resource.lblOtherFee + " - " + fee.OtherFeeDescription;
                                fee.DisplayFeeName = Resource.lblHourlyRate;
                                fee.DisplayDroppedGallons = fee.TotalHours;
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                            {
                                fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : fee.OtherFeeDescription;
                                fee.DisplayFeeName = Resource.lblByQuantity;
                                fee.DisplayDroppedGallons = "";
                                fee.DisplayFee = "";
                            }
                        }
                        else if (!fee.IncludeInPPG && fee.FeeTypeId != ((int)FeeType.ProcessingFee).ToString() && fee.FeeSubTypeId != (int)FeeSubType.NoFee)
                        {
                            if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                            {
                                fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : fee.OtherFeeDescription;
                                fee.DisplayFeeName = Resource.lblFlatFee;
                                fee.DisplayDroppedGallons = fee.InvoiceTypeId == (int)InvoiceType.CreditInvoice || fee.InvoiceTypeId == (int)InvoiceType.PartialCredit ? "-1" : "1";
                                fee.DisplayFee = Resource.constSymbolCurrency + Math.Abs(fee.TotalFee).ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon)
                            {
                                var feeSubQuantity = fee.FeeSubQuantity == null ? 0 : fee.FeeSubQuantity.Value;
                                fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : fee.OtherFeeDescription;
                                fee.DisplayFeeName = Resource.lblPerGallon;
                                //fee.DisplayDroppedGallons = fee.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayDroppedGallons = feeSubQuantity.GetPreciseValue(2).GetCommaSeperatedValue();
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                            {
                                fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : fee.OtherFeeDescription;
                                fee.DisplayFeeName = Resource.lblHourlyRate;
                                fee.DisplayDroppedGallons = fee.TotalHours;
                                fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                            }
                            else if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                            {
                                fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : fee.OtherFeeDescription;
                                fee.DisplayFeeName = Resource.lblByQuantity;
                                fee.DisplayDroppedGallons = "";
                                fee.DisplayFee = "";
                            }
                        }
                    }
                    else if (!fee.IncludeInPPG && fee.FeeSubTypeId != (int)FeeSubType.NoFee)
                    {
                        if (fee.FeeSubTypeId == (int)FeeSubType.FlatFee)
                        {
                            fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : Resource.lblOtherFee + " - " + fee.OtherFeeDescription;
                            fee.DisplayFeeName = Resource.lblFlatFee;
                            fee.DisplayDroppedGallons = fee.InvoiceTypeId == (int)InvoiceType.CreditInvoice || fee.InvoiceTypeId == (int)InvoiceType.PartialCredit ? "-1" : "1";
                            fee.DisplayFee = Resource.constSymbolCurrency + Math.Abs(fee.TotalFee).ToString(ApplicationConstants.DecimalFormat4);
                        }
                        else if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon)
                        {
                            var feeSubQuantity = fee.FeeSubQuantity == null ? 0 : fee.FeeSubQuantity.Value;
                            fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : Resource.lblOtherFee + " - " + fee.OtherFeeDescription;
                            fee.DisplayFeeName = Resource.lblPerGallon;
                            //fee.DisplayDroppedGallons = fee.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                            fee.DisplayDroppedGallons = feeSubQuantity.GetPreciseValue(2).GetCommaSeperatedValue();
                            fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                        }
                        else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                        {
                            fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : Resource.lblOtherFee + " - " + fee.OtherFeeDescription;
                            fee.DisplayFeeName = Resource.lblHourlyRate;
                            fee.DisplayDroppedGallons = fee.TotalHours;
                            fee.DisplayFee = Resource.constSymbolCurrency + fee.Fee.Value.ToString(ApplicationConstants.DecimalFormat4);
                        }
                        else if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                        {
                            fee.DisplayFeeType = string.IsNullOrWhiteSpace(fee.OtherFeeDescription) ? Resource.lblOtherFee : fee.OtherFeeDescription;
                            fee.DisplayFeeName = Resource.lblByQuantity;
                            fee.DisplayDroppedGallons = "";
                            fee.DisplayFee = "";
                        }
                    }

                    // update total hours & total asset qty
                    if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && (fee.FeeSubQuantity != null && fee.FeeSubQuantity.Value != 0))
                    {
                        fee.TotalHours = GetHosingTimeInHours(fee.FeeSubQuantity.Value.ToString());
                        fee.DisplayDroppedGallons = fee.TotalHours;
                    }
                    else if (fee.FeeSubTypeId == (int)FeeSubType.ByAssetCount && (fee.FeeSubQuantity != null && fee.FeeSubQuantity.Value != 0))
                    {
                        fee.TotalAssetQty = Convert.ToInt64(fee.FeeSubQuantity.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "UpdateFuelFees", ex.Message, ex);
            }
        }

        public async Task<InvoicePdfViewModel> GetBillDetailsAsync(int invoiceId)
        {
            InvoicePdfViewModel response = new InvoicePdfViewModel();
            var helperDomain = new HelperDomain(this);
            try
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => new
                {
                    InvoiceId = t.Id,
                    InvoiceNumberId = t.InvoiceHeader.InvoiceNumber.Id,
                    OrderId = t.OrderId.Value,
                    CustomerId = t.Order.AcceptedCompanyId,
                    SupplierName = t.CreatedBy,
                    SupplierCompanyName = t.Order.Company.Name,
                    TxnDate = t.DropEndDate,
                    DueDate = t.PaymentDueDate,
                    SupplierAddress = t.Order.Company.CompanyAddresses.Where(t1 => t1.IsDefault && t1.IsActive).Select(t2 => new { t2.Address, t2.City, t2.MstState.Code, t2.ZipCode, Country = t2.MstCountry.Code }).FirstOrDefault(),
                    PoNumber = t.Order.PoNumber,
                    InvoiceNumber = t.DisplayInvoiceNumber,
                    NetDays = t.Order.FuelRequest.NetDays,
                    PaymentDiscount = t.Order.FuelRequest.PaymentDiscounts.FirstOrDefault(),
                    JobName = t.InvoiceXAdditionalDetail.JobName,
                    t.Order.FuelRequest.PaymentTermId,
                    t.InvoiceTypeId,
                    t.FuelRequestFees,
                    t.BasicAmount,
                    FuelType = t.Order.FuelRequest.MstProduct.TfxProductId.HasValue ? t.Order.FuelRequest.MstProduct.MstTFXProduct.Name : t.Order.FuelRequest.MstProduct.Name,
                    t.DroppedGallons,
                    PricePerGallon = t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).Select(t1 => t1.PricePerGallon).First(),
                    t.Order.FuelRequest.FuelTypeId,
                    t.CreatedDate,
                    t.TotalTaxAmount,
                    t.Order.ExternalBrokerId,
                    t.Order.ExternalBrokerOrderDetail,
                    t.QbInvoiceNumber,
                    t.BrokeredChainId,
                    SupplierCompanyId = t.Order.AcceptedCompanyId,
                    t.InvoiceXAdditionalDetail,
                    OriginalInvoiceNumber = t.InvoiceXAdditionalDetail.OriginalInvoice != null ? t.InvoiceXAdditionalDetail.OriginalInvoice.DisplayInvoiceNumber : null,
                    OriginalInvoiceNumberId = t.InvoiceXAdditionalDetail.OriginalInvoice != null ? t.InvoiceXAdditionalDetail.OriginalInvoice.InvoiceHeader.InvoiceNumberId : (int?)null
                }).FirstOrDefaultAsync();

                if (invoice != null)
                {
                    var previousInvoice = Context.DataContext.Invoices.Where(t => t.BrokeredChainId == invoice.BrokeredChainId && t.Id > invoice.InvoiceId && t.Order.AcceptedCompanyId != invoice.SupplierCompanyId).Select(t => new { t.DisplayInvoiceNumber, t.Order.BuyerCompany.Name }).FirstOrDefault();
                    response = new InvoicePdfViewModel
                    {
                        Invoice = new InvoiceViewModel()
                        {
                            Id = invoice.InvoiceId,
                            OrderId = invoice.OrderId,
                            InvoiceNumber = new InvoiceNumberViewModel() { Id = invoice.InvoiceNumberId, Number = invoice.InvoiceNumber },
                            InvoiceTypeId = invoice.InvoiceTypeId,
                            DroppedGallons = invoice.DroppedGallons,
                            PricePerGallon = invoice.PricePerGallon,
                            DropEndDate = invoice.TxnDate,
                            PaymentDueDate = invoice.DueDate,
                            BasicAmount = invoice.BasicAmount,
                            CreatedDate = invoice.CreatedDate,
                            TotalTaxAmount = invoice.TotalTaxAmount,
                            QbInvoiceNumber = invoice.QbInvoiceNumber
                        },
                        OriginalInvoiceNumber = invoice.OriginalInvoiceNumber,
                        OriginalInvoiceNumberId = invoice.OriginalInvoiceNumberId,
                        IsRebillInvoice = invoice.InvoiceXAdditionalDetail.OriginalInvoiceId != null && invoice.InvoiceTypeId != (int)InvoiceType.CreditInvoice && invoice.InvoiceTypeId != (int)InvoiceType.PartialCredit,
                        PoNumber = invoice.PoNumber,
                        SupplierCompanyName = invoice.SupplierCompanyName,
                        SupplierLocation = { Address = invoice.SupplierAddress.Address, City = invoice.SupplierAddress.City,
                                                StateCode = invoice.SupplierAddress.Code, CountryCode = invoice.SupplierAddress.Country, ZipCode = invoice.SupplierAddress.ZipCode },
                        FuelRequest = new FuelRequestViewModel()
                        {
                            FuelDetails = new FuelDetailsViewModel() { FuelType = invoice.FuelType, FuelTypeId = invoice.FuelTypeId }
                        },
                        PaymentTermId = invoice.PaymentTermId,
                        NetDays = invoice.NetDays,
                    };
                    response.FuelRequest.Job.Name = invoice.JobName;
                    response.CustomerId = invoice.CustomerId.ToString();
                    // get fuel fee details
                    response.FuelFees = GetInvoiceFuelFees(invoice.FuelRequestFees, invoice.DroppedGallons);
                    //SetSurchargeDetails(invoice.InvoiceXAdditionalDetail, response.FuelFees.FuelSurchargeFreightFee);

                    if (previousInvoice != null)
                    {
                        response.CustomerInvoiceNumber = previousInvoice.DisplayInvoiceNumber;
                        response.CustomerCompany = previousInvoice.Name;
                    }
                    response.Invoice.PricePerGallonDisplay = helperDomain.GetInvoicePrice(invoice.InvoiceId);
                    if (invoice.ExternalBrokerId.HasValue && invoice.ExternalBrokerId.Value > 0
                        && invoice.ExternalBrokerOrderDetail != null && invoice.ExternalBrokerOrderDetail.InvoicePreferenceId == (int)InvoicePreference.SendInvoiceOnBehalfOfBroker)
                    {
                        var brokerDetails = Context.DataContext.ExternalBrokers.Where(t => t.IsActive && t.Id == invoice.ExternalBrokerId).FirstOrDefault();
                        var brokerName = brokerDetails.CompanyName;
                        var state = Context.DataContext.MstStates.SingleOrDefault(t => t.Id == brokerDetails.StateId);
                        if (!string.IsNullOrWhiteSpace(brokerName))
                        {
                            response.SupplierCompanyName = brokerDetails.CompanyName;
                            response.SupplierLocation.Address = brokerDetails.Address;
                            response.SupplierLocation.City = brokerDetails.City;
                            response.SupplierLocation.StateCode = state.Code;
                            response.SupplierLocation.ZipCode = brokerDetails.ZipCode;
                            response.PhoneNumber = brokerDetails.PhoneNumber;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetBillDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<ApiInvoiceDetailViewModel> GetBuyerInvoiceDetail(int invoiceId)
        {
            ApiInvoiceDetailViewModel response = new ApiInvoiceDetailViewModel();
            HelperDomain helperDomain = new HelperDomain(this);

            try
            {
                var invoice = await Context.DataContext.Invoices.Include(t => t.Order).Include(t => t.Order.FuelRequest)
                    .Include(t => t.Order.FuelRequest.Job).SingleOrDefaultAsync(t => t.Id == invoiceId);
                if (invoice != null)
                {
                    response = new ApiInvoiceDetailViewModel
                    {
                        Invoice = invoice.ToApiInvoiceViewModel(),
                        PoNumber = invoice.Order == null ? Resource.lblHyphen : invoice.PoNumber,
                        SupplierCompanyName = invoice.Order.Company.Name,
                        JobName = invoice.Order.FuelRequest.Job.Name,
                        Assets = invoice.AssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).OrderBy(t => t.JobXAsset.Asset.Id).Select(t => new AssetDropViewModel(Status.Success)
                        {
                            AssetName = t.JobXAsset.Asset.Name,
                            JobXAssetId = t.JobXAsset.Id,
                            DropDate = t.DropStartDate,
                            EndTime = t.DropEndDate.DateTime.ToString(Resource.constFormat12HourTime2),
                            DropGallons = t.DroppedGallons.GetPreciseValue(6),
                            PricePerGallon = t.Invoice.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).Select(t1 => t1.PricePerGallon).First(),
                            SubcontractorName = t.SubcontractorName ?? string.Empty,
                        }).GroupBy(t => t.AssetName).Select(t => t.ToList()).ToList(),
                        FuelRequestFee = GetInvoiceFee(invoice),
                    };

                    response.Invoice.TotalFees = helperDomain.GetInvoiceTotalFees(invoice);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetBuyerInvoiceDetail", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoicePaymentViewModel>> GetInvoicePayments(int invoiceNumberId, string qbInvoiceNumber)
        {
            List<InvoicePaymentViewModel> invoicePayments = new List<InvoicePaymentViewModel>();
            try
            {
                invoicePayments = await (from invoice in Context.DataContext.Invoices
                                         join payment in Context.DataContext.InvoicePayments on invoice.InvoiceHeader.InvoiceNumberId equals payment.InvoiceNumberId
                                         where payment.IsActive && payment.InvoiceNumberId == invoiceNumberId && payment.QbInvoiceNumber == qbInvoiceNumber
                                         select new InvoicePaymentViewModel()
                                         {
                                             InvoiceNumberId = payment.InvoiceNumberId,
                                             QbInvoiceNumber = payment.QbInvoiceNumber,
                                             PaymentDate = payment.PaymentDate,
                                             PaymentMethod = payment.PaymentMethod,
                                             AmountPaid = payment.AmountPaid,
                                             BalanceRemaining = payment.BalanceRemaining,
                                             TransRefNumber = payment.TransRefNumber,
                                             DisplayInvoiceNumber = invoice.DisplayInvoiceNumber
                                         })
                                          .OrderBy(t => t.DisplayInvoiceNumber)
                                          .ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoicePayments", ex.Message, ex);
            }

            return invoicePayments;
        }

        //private void SetFreightFeeAmount(TPOBrokeredOrderFeeViewModel BrokeredOrderFee, Invoice invoice)
        //{
        //    if (BrokeredOrderFee.FreightFeeSubTypeId == (int)FeeSubType.ByAssetCount)
        //    {
        //        BrokeredOrderFee.FreightFeeAmount = (BrokeredOrderFee.FreightFee * invoice.AssetDrops.Count(t => t.DropStatus == (int)DropStatus.None));
        //    }
        //    else if (BrokeredOrderFee.FreightFeeSubTypeId == (int)FeeSubType.PerGallon)
        //    {
        //        BrokeredOrderFee.FreightFeeAmount = (BrokeredOrderFee.FreightFee * invoice.DroppedGallons);
        //    }
        //    else
        //    {
        //        BrokeredOrderFee.FreightFeeAmount = BrokeredOrderFee.FreightFee;
        //    }
        //}

        public async Task<List<MapViewModel>> GetMapDataAsync(int userId, InvoiceFilterViewModel invoiceFilter)
        {
            var response = new List<MapViewModel>();
            var helperDomain = new HelperDomain(this);
            try
            {
                var groupIdslist = helperDomain.GetGroupList(invoiceFilter.GroupIds);

                var allJobs = Context.DataContext.Jobs.Where(t => ((groupIdslist.Count == 0 && t.Company.Users.Any(x => x.Id == userId)) ||
                                                                  (t.Company.SubCompanies.Any(t1 => t1.SubCompanyId == t.CompanyId && groupIdslist.Contains(t1.CompanyGroupId)))) &&
                                                                   t.IsActive && t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open &&
                                                                   t.CountryId == invoiceFilter.CountryId);

                if (invoiceFilter != null && invoiceFilter.JobId > 0)
                {
                    allJobs = allJobs.Where(t => t.Id == invoiceFilter.JobId);
                }

                response = await allJobs.Select(entity => new MapViewModel
                {
                    JobId = entity.Id,
                    Name = entity.Name,
                    Address = entity.Address,
                    City = entity.City,
                    State = entity.MstState.Code,
                    Country = entity.MstCountry.Code,
                    ZipCode = entity.ZipCode,
                    Latitude = entity.Latitude,
                    Longitude = entity.Longitude,
                    ContactPersons = entity.Users1.Select(t => new ContactPersonViewModel()
                    {
                        Id = t.Id,
                        Name = t.FirstName + " " + t.LastName,
                        Email = t.Email,
                        PhoneNumber = t.PhoneNumber
                    }).ToList()
                }).ToListAsync();
            }

            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetMap", ex.Message + " :userId: " + userId, ex);
            }
            return response;
        }

        public async Task<DryRunInvoiceViewModel> GetDryRunInvoiceAsync(int orderId, int userId)
        {
            var response = new DryRunInvoiceViewModel();
            HelperDomain helperDomain = new HelperDomain(this);
            var order = await Context.DataContext.Orders.Where(t => t.Id == orderId && t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open))
                        .Select(t => new
                        {
                            TotalDroppedQuantity = (decimal?)t.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice).Sum(t1 => t1.DroppedGallons),
                            t.FuelRequest.MaxQuantity,
                            t.AcceptedDate,
                            FrStartDate = t.FuelRequest.FuelRequestDetail.StartDate,
                            FrEndDate = t.FuelRequest.FuelRequestDetail.EndDate,
                            JobEndDate = t.FuelRequest.Job.EndDate,
                            t.FuelRequest.Currency,
                            t.FuelRequest.UoM,
                            t.FuelRequest.FuelRequestFees,
                            t.PoNumber,
                            t.FuelRequest.QuantityTypeId,
                            t.FuelRequest.FuelRequestDetail.OrderEnforcementId,
                        }).FirstOrDefaultAsync();

            if (order != null)
            {
                response = new DryRunInvoiceViewModel(Status.Success)
                {
                    OrderId = orderId,
                    PoNumber = order.PoNumber,
                    FuelRemaining = order.MaxQuantity - (order.TotalDroppedQuantity ?? 0),
                    UserId = userId,
                    MinDropDate = order.AcceptedDate < order.FrStartDate ? order.AcceptedDate : order.FrStartDate,
                    DryRunFee = helperDomain.GetDryRunFee(order.FuelRequestFees, DateTimeOffset.Now),
                    Currency = order.Currency,
                    UoM = order.UoM,
                    QuantityTypeId = order.QuantityTypeId,
                    OrderEnforcement = order.OrderEnforcementId
                };

                if (order.FrEndDate.HasValue)
                {
                    response.MaxDropDate = order.FrEndDate.Value.Date;
                }
                else if (order.JobEndDate.HasValue)
                {
                    response.MaxDropDate = order.JobEndDate.Value.Date;
                }
                else
                {
                    response.MaxDropDate = null;
                }
                response.MaxDropDate = (response.MaxDropDate == null || response.MaxDropDate.Value > DateTimeOffset.Now.Date) ? DateTimeOffset.Now.Date : response.MaxDropDate;
                response = response.CorrectValues();
            }
            return response;
        }

        public async Task<DryRunInvoiceViewModel> GetDryRunInvoiceForEditAsync(int invoiceId, int userId)
        {
            var response = new DryRunInvoiceViewModel();

            var invoice = await Context.DataContext.Invoices
                            .Include(t => t.Order)
                            .Include(t => t.Order.FuelRequest.FuelRequestDetail)
                            .FirstOrDefaultAsync(t => t.Id == invoiceId);
            if (invoice != null)
            {
                response = new DryRunInvoiceViewModel(Status.Success)
                {
                    InvoiceId = invoiceId,
                    OrderId = invoice.OrderId ?? 0,
                    PoNumber = invoice.PoNumber,
                    FuelRemaining = invoice.OrderId == null ? 0 :
                                    (
                                        (invoice.Order.BrokeredMaxQuantity ?? invoice.Order.FuelRequest.MaxQuantity) - invoice.Order.Invoices
                                        .Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                        .Sum(t => t.DroppedGallons)
                                    ),
                    UserId = userId,
                    DryRunDate = invoice.DropEndDate.ToString(Resource.constFormatDate),
                    DeliveryTime = invoice.DropEndDate.DateTime.ToShortTimeString(),
                    InvoiceNumberId = invoice.InvoiceHeader.InvoiceNumberId,
                    QuantityTypeId = invoice.Order.FuelRequest.QuantityTypeId
                };

                if (invoice.Order.FuelRequest.Job.IsBackdatedJob)
                {
                    response.MinDropDate = invoice.Order.AcceptedDate < invoice.Order.FuelRequest.FuelRequestDetail.StartDate ? invoice.Order.AcceptedDate : invoice.Order.FuelRequest.FuelRequestDetail.StartDate;
                }
                else
                {
                    response.MinDropDate = invoice.Order.AcceptedDate > invoice.Order.FuelRequest.FuelRequestDetail.StartDate ? invoice.Order.AcceptedDate : invoice.Order.FuelRequest.FuelRequestDetail.StartDate;
                }

                if (invoice.Order.FuelRequest.FuelRequestDetail.EndDate.HasValue)
                {
                    response.MaxDropDate = invoice.Order.FuelRequest.FuelRequestDetail.EndDate.Value.Date;
                }
                else if (invoice.Order.FuelRequest.Job.EndDate.HasValue)
                {
                    response.MaxDropDate = invoice.Order.FuelRequest.Job.EndDate.Value.Date;
                }
                else
                {
                    response.MaxDropDate = null;
                }

                response.MaxDropDate = (response.MaxDropDate == null || response.MaxDropDate.Value > DateTimeOffset.Now.Date) ? DateTimeOffset.Now.Date : response.MaxDropDate;
                response = response.CorrectValues();
            }
            return response;
        }

        public async Task<StatusViewModel> CreateDryRunInvoiceAsync(DryRunInvoiceViewModel viewModel)
        {
            var invoiceModel = new InvoiceViewModel();
            var response = new StatusViewModel();
            SetDryRunInputs(invoiceModel, viewModel);
            var isDuplicate = IsDuplicateInvoiceNumber(viewModel.SupplierInvoiceNumber);
            if (!isDuplicate)
                response = await GenerateInvoiceForDryRun(invoiceModel);
            else
                response.StatusMessage = ResourceMessages.GetMessage(Resource.valMessageAlreadyExist, new object[] { Resource.lblInvoiceNumber });
            return response;
        }

        public async Task<Tuple<string, string>> GetImage(int imageId)
        {
            try
            {
                var image = await Context.DataContext.Images.SingleOrDefaultAsync(t => t.Id == imageId);
                if (image != null)
                {
                    return Tuple.Create(image.Id.ToString(), image.FilePath);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", $"GetInvoiceImagesAsync Params: {imageId}", ex.Message, ex);
            }
            return null;
        }

        public async Task<List<ImageViewModel>> GetInvoiceImagesAsync(int id, int imageId = 0)
        {
            var response = new List<ImageViewModel>();
            try
            {
                var invoice = await Context.DataContext.Invoices.Include(t => t.Image).Include(t => t.AssetDrops).SingleOrDefaultAsync(t => t.Id == id);
                if (invoice != null)
                {
                    if (invoice.Image != null && (imageId == 0 || imageId == invoice.Image.Id))
                    {
                        var image = invoice.Image.ToViewModel();
                        image.Name = invoice.DisplayInvoiceNumber;
                        response.Add(image);
                    }
                    var assetDropImages = imageId > 0 ? invoice.AssetDrops.Where(t => t.Image != null
                                                                && t.Image.Id == imageId) : invoice.AssetDrops.Where(t => t.Image != null);
                    foreach (var assetdrop in assetDropImages)
                    {
                        var assetDropImage = assetdrop.Image.ToViewModel();
                        assetDropImage.Name = "asset-drop-" + assetdrop.JobXAsset.Asset.Name;
                        response.Add(assetDropImage);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceImagesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ProcessInvoiceApprovalReminders()
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t.IsBuyPriceInvoice
                    && t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.WaitingForApproval
                    && t.Order.FuelRequest.Job.JobXApprovalUsers.Any(t1 => t1.IsActive));
                    if (invoices != null)
                    {
                        foreach (var invoice in invoices)
                        {
                            if ((DateTime.Today - invoice.CreatedDate).TotalDays > 0)
                            {
                                var invoicePendingUser = invoice.Order.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t => t.IsActive);
                                if (invoicePendingUser != null && invoicePendingUser.UserId > 0)
                                {
                                    await ContextFactory.Current.GetDomain<NotificationDomain>()
                                          .AddNotificationEventAsync(EventType.InvoiceApprovalReminder, invoice.InvoiceHeaderId, invoicePendingUser.UserId);

                                    if (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                                    {
                                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetSystemInvoiceWaitingForApprovalNewsfeed(invoice, invoicePendingUser.User);
                                    }
                                }
                            }
                        }
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageInvoiceApprovalRemindersSuccess;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "ProcessInvoiceApprovalReminders", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<decimal> GetFuelPriceAsync(Order order, InvoiceViewModel viewModel)
        {
            var response = 0.0M;
            if (order != null)
            {
                var fuelRequest = order.FuelRequest;
                var productDisplayGroupId = fuelRequest.MstProduct.ProductDisplayGroupId;
                var fuelPricingRequestVm = GetFuelPricingRequestViewModel(viewModel, fuelRequest);

                var commonDomain = new InvoiceCommonDomain(this);
                var pricingRespose = await commonDomain.GetFuelPriceByPricingTypeAsync(fuelPricingRequestVm, productDisplayGroupId);
                if (pricingRespose != null)
                {
                    response = pricingRespose.PricePerGallon;
                    viewModel.RackPrice = pricingRespose.TerminalPrice;
                    viewModel.TerminalPricingDate = pricingRespose.PricingDate;
                    viewModel.WaitingForAction = (int)pricingRespose.WaitingFor;
                }
            }
            return response;
        }

        private FuelPricingRequestViewModel GetFuelPricingRequestViewModel(InvoiceViewModel viewModel, FuelRequest fuelRequest)
        {
            var fuelPricingModel = new FuelPricingRequestViewModel
            {
                FuelTypeId = fuelRequest.FuelTypeId,
                TerminalId = viewModel.TerminalId,
                CityGroupTerminalId = viewModel.CityGroupTerminalId,
                PricingTypeId = fuelRequest.PricingTypeId,
                DropEndDate = viewModel.DropEndDate,
                Currency = viewModel.Currency,
                WaitingFor = (WaitingAction)viewModel.WaitingForAction,
                DroppedQuantity = viewModel.DroppedGallons
            };
            SetTierDetailsToPricingModel(fuelPricingModel, viewModel);
            fuelPricingModel.FuelRequestPricingDetails = fuelRequest.FuelRequestPricingDetail.ToViewModel();
            return fuelPricingModel;
        }

        private void SetTierDetailsToPricingModel(FuelPricingRequestViewModel fuelPricingModel, InvoiceViewModel invoiceModel)
        {
            if (invoiceModel.BolDetails != null)
            {
                var bol = invoiceModel.BolDetails;
                if (bol != null && bol.TierPricingForBol.Any())
                {
                    fuelPricingModel.DroppedQuantity = invoiceModel.DroppedGallons;
                    fuelPricingModel.TierMaxQuantity = bol.TierPricingForBol.FirstOrDefault().TierMaxQuantity;
                    fuelPricingModel.TierMinQuantity = bol.TierPricingForBol.FirstOrDefault().TierMinQuantity;
                }
            }
        }

        public async Task<decimal> GetFuelPriceForBuySellOrderAsync(Order order, InvoiceViewModel viewModel, bool IsBuyPrice)
        {
            var response = await GetFuelPriceAsync(order, viewModel);
            if (order != null)
            {
                var buySellDetails = order.ExternalBrokerBuySellDetail;
                if (buySellDetails != null && buySellDetails.OrderId == order.Id)
                {
                    if (IsBuyPrice)
                    {
                        //buy price for supplier
                        response = response + buySellDetails.BrokerMarkUp;
                    }
                    else
                    {
                        //sell price for supplier
                        response = response + buySellDetails.BrokerMarkUp + buySellDetails.SupplierMarkUp;
                    }
                }
            }

            return response;
        }

        public async Task<decimal> GetDryRunFee(int orderId, DateTimeOffset datetime)
        {
            var response = 0.0M;
            try
            {
                var fuelReqestFees = await Context.DataContext.Orders.Where(t => t.Id == orderId)
                                    .SelectMany(t => t.FuelRequest.FuelRequestFees
                                    .Where(t1 => t1.FeeTypeId == (int)FeeType.DryRunFee && t1.FeeSubTypeId == (int)FeeSubType.FlatFee)).ToListAsync();
                HelperDomain helperDomain = new HelperDomain(this);
                response = helperDomain.GetDryRunFee(fuelReqestFees, datetime);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetDryRunFee", ex.Message, ex);
            }
            return response.GetPreciseValue();
        }

        private async Task<decimal> GetTierPricingAmountAsync(Order order, InvoiceViewModel viewModel)
        {
            var response = 0.0M;
            if (order != null)
            {
                var fuelRequest = order.FuelRequest;
                var productId = fuelRequest.MstProduct.Id;
                var terminalId = viewModel.TerminalId ?? order.TerminalId ?? fuelRequest.TerminalId ?? 0;
                var cityGroupTerminalId = viewModel.CityGroupTerminalId ?? order.CityGroupTerminalId ?? fuelRequest.CityGroupTerminalId ?? 0;
                var tierPricing = fuelRequest.DifferentFuelPrices.Where(t => t.MinQuantity <= viewModel.DroppedGallons).OrderBy(t => t.MinQuantity);
                foreach (var item in tierPricing)
                {
                    decimal pricePerGallon = 0;

                    var externalPricingDomain = new ExternalPricingDomain(this);
                    FuelPricingRequestViewModel fuelPricingRequestViewModel = new FuelPricingRequestViewModel()
                    {
                        TerminalId = terminalId,
                        FuelTypeId = productId,
                        PricePerGallon = viewModel.PricePerGallon,
                        DropEndDate = viewModel.DropEndDate,
                        CityGroupTerminalId = cityGroupTerminalId
                    };
                    fuelPricingRequestViewModel.FuelRequestPricingDetails.RequestPriceDetailId = fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId;
                    var externalPricing = await externalPricingDomain.GetFuelPriceAsync(fuelPricingRequestViewModel);
                    if (externalPricing != null)
                    {
                        pricePerGallon = externalPricing.PricePerGallon;
                    }
                    var maxQuantity = item.MaxQuantity ?? viewModel.DroppedGallons;
                    maxQuantity = maxQuantity < viewModel.DroppedGallons ? maxQuantity : viewModel.DroppedGallons;
                    var amount = (maxQuantity - item.MinQuantity + 1) * pricePerGallon;
                    response = response + amount;
                }
            }

            return response;
        }

        private async Task SetInvoiceAmounts(Order order, InvoiceViewModel viewModel)
        {
            if (order.FuelRequest.PricingTypeId == (int)PricingType.Tier)
            {
                viewModel.BasicAmount = await GetTierPricingAmountAsync(order, viewModel);
            }
            else
            {
                viewModel.PricePerGallon = await GetFuelPriceAsync(order, viewModel);
                viewModel.BasicAmount = Math.Round(viewModel.DroppedGallons * viewModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                if (viewModel.AdditionalDetail != null)
                {
                    var allowance = Math.Round(viewModel.AdditionalDetail.SupplierAllowance ?? 0, ApplicationConstants.InvoiceSuppplierAllowanceUnitPriceDecimalDisplay);
                    viewModel.AdditionalDetail.TotalAllowance = Math.Round(viewModel.DroppedGallons * allowance, ApplicationConstants.InvoiceSuppplierAllowanceTotalDecimalDisplay);
                }
                viewModel.BasicAmount = viewModel.BasicAmount - (viewModel.AdditionalDetail?.TotalAllowance ?? 0);
            }
            viewModel.TerminalId = (viewModel.TerminalId.HasValue && viewModel.TerminalId > 0) ? viewModel.TerminalId : (order.TerminalId ?? order.FuelRequest.TerminalId);
            viewModel.CityGroupTerminalId = order.CityGroupTerminalId;
        }

        private async Task SetSplitInvoiceAmounts(Order order, InvoiceViewModel viewModel)
        {
            if (order.FuelRequest.PricingTypeId == (int)PricingType.Tier)
            {
                viewModel.BasicAmount = await GetTierPricingAmountAsync(order, viewModel);
            }
            else
            {
                viewModel.PricePerGallon = await GetFuelPriceAsync(order, viewModel);
                viewModel.BasicAmount = Math.Round(viewModel.DroppedGallons * viewModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                if (viewModel.IsFTL)
                {
                    var allowance = Math.Round(viewModel.AdditionalDetail.SupplierAllowance ?? 0, ApplicationConstants.InvoiceSuppplierAllowanceUnitPriceDecimalDisplay);
                    viewModel.AdditionalDetail.TotalAllowance = Math.Round(viewModel.DroppedGallons * allowance, ApplicationConstants.InvoiceSuppplierAllowanceTotalDecimalDisplay);
                    viewModel.BasicAmount = Math.Round(viewModel.DroppedGallons * viewModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay) - viewModel.AdditionalDetail.TotalAllowance ?? 0;
                }
            }
            viewModel.TerminalId = viewModel.TerminalId ?? order.TerminalId ?? order.FuelRequest.TerminalId;
            viewModel.CityGroupTerminalId = order.CityGroupTerminalId;
        }

        private async Task SetInvoiceAmountsForBuySellOrder(Order order, InvoiceViewModel viewModel, bool IsBuyPrice)
        {
            viewModel.PricePerGallon = await GetFuelPriceForBuySellOrderAsync(order, viewModel, IsBuyPrice);
            viewModel.BasicAmount = viewModel.DroppedGallons * viewModel.PricePerGallon;
            viewModel.TerminalId = viewModel.TerminalId ?? order.TerminalId ?? order.FuelRequest.TerminalId;
            viewModel.CityGroupTerminalId = order.CityGroupTerminalId;
        }

        private InvoiceTaxDetailsViewModel GetInvoiceTaxes(Order order, InvoiceViewModel viewModel)
        {
            var job = order.FuelRequest.Job;
            var avaTaxInputViewModel = viewModel.ToAvaTaxViewModel(order, job);
            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
            avaTaxInputViewModel.CurrencyRates = currencyRateDomain.GetCurrencyRatesForAvalara(Currency.USD, job.MstCountry.Currency, viewModel.DropEndDate, avaTaxInputViewModel.Currency);
            avaTaxInputViewModel.Exclusions = GetTaxEclusionIfExist(viewModel.UserId);

            InvoiceTaxDetailsViewModel response;
            using (var tracer = new Tracer("InvoiceDomain", "GetInvoiceTaxes"))
            {
                avaTaxInputViewModel.SupplierAllowance = viewModel.AdditionalDetail?.SupplierAllowance ?? 0;
                avaTaxInputViewModel.IsDirectTaxCompany = Context.DataContext.DirectTaxes.Any(t => t.CompanyId == order.BuyerCompanyId && t.StateId == job.StateId && t.IsActive);
                if (viewModel.IsFTL)
                {
                    SetFTLParametsToAvaTaxViewModel(avaTaxInputViewModel, order, job, viewModel);
                    var avaDomain = AvalaraDomain.InvokeProcessTransactions_5_27_0_For_FTL(avaTaxInputViewModel);
                    var currencyRate = avaTaxInputViewModel.CurrencyRates.FirstOrDefault(t => t.ToCurrency == job.Currency.ToString());
                    var exchangeRate = currencyRate == null ? 1 : currencyRate.ExchangeRate;
                    response = avaDomain.ToResponseViewModel(job.JobBudget.IsTaxExempted, exchangeRate);
                }
                else
                {
                    var avaDomain = AvalaraDomain.InvokeProcessTransactions_5_27_0(avaTaxInputViewModel);
                    var currencyRate = avaTaxInputViewModel.CurrencyRates.FirstOrDefault(t => t.ToCurrency == job.Currency.ToString());
                    var exchangeRate = currencyRate == null ? 1 : currencyRate.ExchangeRate;
                    response = avaDomain.ToResponseViewModel(job.JobBudget.IsTaxExempted, exchangeRate);
                }
            }
            return response;
        }

        private void SetFTLParametsToAvaTaxViewModel(AvalaraTaxInputViewModel avaTaxInputViewModel, Order order, Job job, InvoiceViewModel viewModel)
        {
            avaTaxInputViewModel.IsFobOrigin = job.LocationType == JobLocationTypes.Various;

            if (avaTaxInputViewModel.IsFobOrigin)
            {
                avaTaxInputViewModel.DestinationAddress = null;
                avaTaxInputViewModel.DestinationCity = null;
                avaTaxInputViewModel.DestinationCounty = null;
                string statecode;
                avaTaxInputViewModel.DestinationPostalCode = SetFirstZipCodeOfState(viewModel.DropAddress.State.Id, viewModel.DropAddress.State.Code, out statecode);
                avaTaxInputViewModel.DestinationJurisdiction = statecode;

            }
        }

        private void SetInvoiceDueDate(ManualInvoiceViewModel invoiceViewModel, InvoiceViewModel viewModel)
        {
            var paymentDueDate = DateTimeOffset.Now.ToTargetDateTimeOffset(invoiceViewModel.TimeZoneName);
            var paymentTermId = invoiceViewModel.PaymentTermId;
            if (paymentTermId == (int)PaymentTerms.NetDays)
            {
                paymentDueDate = paymentDueDate.Date.AddDays(invoiceViewModel.NetDays);
            }

            viewModel.PaymentDueDate = paymentDueDate;
        }

        private void SetManualInputs(InvoiceViewModel viewModel, ManualInvoiceViewModel manualInvoiceModel)
        {
            viewModel.DropStartDate = new DateTimeOffset(manualInvoiceModel.DeliveryDate.Add(Convert.ToDateTime(manualInvoiceModel.StartTime).TimeOfDay));
            var dropEndDate = new DateTimeOffset(manualInvoiceModel.DeliveryDate.Add(Convert.ToDateTime(manualInvoiceModel.EndTime).TimeOfDay));
            if (manualInvoiceModel.DropEndDate != null)
                dropEndDate = new DateTimeOffset(manualInvoiceModel.DropEndDate.Value.Add(Convert.ToDateTime(manualInvoiceModel.EndTime).TimeOfDay));
            viewModel.DropEndDate = dropEndDate;

            viewModel.DroppedGallons = manualInvoiceModel.FuelDropped ?? 0.0M;
            viewModel.Id = manualInvoiceModel.InvoiceId;

            viewModel.IsWetHosingDelivery = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.WetHoseFee).ToString() && t.DiscountLineItemId == null);
            viewModel.IsOverWaterDelivery = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.OverWaterFee).ToString() && t.DiscountLineItemId == null);

            viewModel.UserId = manualInvoiceModel.userId;
            viewModel.OrderId = manualInvoiceModel.OrderId;
            viewModel.PoNumber = manualInvoiceModel.PoNumber;
            viewModel.BrokeredChainId = manualInvoiceModel.BrokeredChainId;
            viewModel.InvoiceTypeId = manualInvoiceModel.InvoiceTypeId;
            viewModel.Image = manualInvoiceModel.InvoiceImage;
            if (manualInvoiceModel.SignatureImage != null && (manualInvoiceModel.SignatureImage.Id > 0))
                viewModel.Signature = manualInvoiceModel.SignatureImage?.ToCustomerSignature();
            viewModel.BolImage = manualInvoiceModel.BolImage;
            viewModel.AdditionalImage = manualInvoiceModel.AdditionalImage;
            viewModel.TaxAffidavitImage = manualInvoiceModel.TaxAffidavitImage;
            viewModel.BDNImage = manualInvoiceModel.BDNImage;
            viewModel.CoastGuardInspectionImage = manualInvoiceModel.CoastGuardInspectionImage;
            viewModel.InspectionRequestVoucherImage = manualInvoiceModel.InspectionRequestVoucherImage;
            viewModel.DriverId = manualInvoiceModel.DriverId;
            viewModel.TrackableScheduleId = manualInvoiceModel.TrackableScheduleId;
            if (manualInvoiceModel.StatusId != 0)
            {
                viewModel.StatusId = manualInvoiceModel.StatusId;
            }

            viewModel.CsvFilePath = manualInvoiceModel.CsvFilePath;
            viewModel.WaitingForAction = manualInvoiceModel.WaitingForAction;
            viewModel.IsBuyPriceInvoice = manualInvoiceModel.IsBuyPriceInvoice;
            viewModel.UoM = manualInvoiceModel.UoM;
            viewModel.Currency = manualInvoiceModel.Currency;
            viewModel.SpecialInstructions = manualInvoiceModel.SpecialInstructions;
            viewModel.BolDetails = manualInvoiceModel.BolDetails;
            viewModel.UpdatedBy = manualInvoiceModel.UpdatedBy;
            viewModel.IsVariousFobOrigin = manualInvoiceModel.IsVariousFobOrigin;
            viewModel.DropAddress = manualInvoiceModel.DropAddress;
            viewModel.IsFTL = manualInvoiceModel.IsFTL; //need when waiting for updatedprice, approval workflow
            viewModel.FuelSurchargeFreightFee = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee;
            viewModel.FuelSurchargeFreightFee.GallonsDelivered = viewModel.DroppedGallons;
            viewModel.CreationMethod = manualInvoiceModel.CreationMethod;
            viewModel.IsInvoiceImagesAvailable = manualInvoiceModel.IsInvoiceImagesAvailable;
            if (manualInvoiceModel.PickUpAddress != null && manualInvoiceModel.PickUpAddress.IsAddressAvailable)
            {
                viewModel.FuelPickLocation = manualInvoiceModel.ToPickUpLocation();
                viewModel.FuelPickLocation.PickupLocationType = PickupLocationType.BulkPlant;
            }
            viewModel.IsTerminalPickup = manualInvoiceModel.PickUpAddress == null || !manualInvoiceModel.PickUpAddress.IsAddressAvailable || string.IsNullOrEmpty(manualInvoiceModel.PickUpAddress.Address);
        }

        private void SetDryRunInputs(InvoiceViewModel viewModel, DryRunInvoiceViewModel dryRunInvoiceModel)
        {
            var selectedDate = dryRunInvoiceModel.DryRunDate + " " + dryRunInvoiceModel.DeliveryTime;
            viewModel.DropEndDate = DateTimeOffset.Parse(selectedDate);
            viewModel.DropStartDate = viewModel.DropEndDate;
            viewModel.Id = dryRunInvoiceModel.InvoiceId;
            viewModel.UserId = dryRunInvoiceModel.UserId;
            viewModel.OrderId = dryRunInvoiceModel.OrderId;
            viewModel.PoNumber = dryRunInvoiceModel.PoNumber;
            viewModel.InvoiceTypeId = (int)InvoiceType.DryRun;
            viewModel.InvoiceNumber.Id = dryRunInvoiceModel.InvoiceNumberId;
            viewModel.BasicAmount = dryRunInvoiceModel.DryRunFee;
            viewModel.SupplierInvoiceNumber = dryRunInvoiceModel.SupplierInvoiceNumber;
            viewModel.CreationMethod = dryRunInvoiceModel.CreationMethod;
            viewModel.TrackableScheduleId = dryRunInvoiceModel.TrackableScheduleId;
            viewModel.DivertedOrderIds = dryRunInvoiceModel.DivertedOrderIds;
            if (viewModel.CreationMethod == CreationMethod.Mobile)
                viewModel.DriverId = dryRunInvoiceModel.DriverId;
        }

        private void SetAssetDropsToInvoice(Invoice invoice, List<AssetDropViewModel> jobAssets)
        {
            var jobxAssetIds = jobAssets.Select(t => t.JobXAssetId).ToList();
            Context.DataContext.AssetDrops.RemoveRange(invoice.AssetDrops);
            var subcontractors = (from jAsset in Context.DataContext.JobXAssets
                                  join a in Context.DataContext.Assets on jAsset.AssetId equals a.Id
                                  join aSub in Context.DataContext.AssetSubcontractors on a.Id equals aSub.AssetId into subContr
                                  from subContractor in subContr.DefaultIfEmpty()
                                  join s in Context.DataContext.Subcontractors on subContractor.SubcontractorId equals s.Id into contr
                                  from contractor in contr.DefaultIfEmpty()
                                  join ac in Context.DataContext.AssetContractNumbers on a.Id equals ac.AssetId into contractNum
                                  from contractNumber in contractNum.DefaultIfEmpty()
                                  where jobxAssetIds.Contains(jAsset.Id)
                                  select new
                                  {
                                      Id = jAsset.Id,
                                      ContractNum = contractNumber != null ? contractNumber.ContractNumber : "",
                                      Timezone = jAsset.Job.TimeZoneName,
                                      AssignedDate = subContractor != null ? subContractor.AssignedDate : (DateTimeOffset?)null,
                                      RemovedDate = subContractor != null ? subContractor.RemovedDate : null,
                                      SubName = contractor != null ? contractor.Name : "",
                                      SubId = contractor != null ? contractor.Id : 0,
                                      AddedDate = contractNumber != null ? contractNumber.AddedDate : (DateTimeOffset?)null,
                                      DeletedDate = contractNumber != null ? contractNumber.RemovedDate : null,
                                  }).ToList();

            foreach (var asset in jobAssets)
            {
                if (asset.DropGallons > 0 || (asset.DropGallons == 0 &&
                    (asset.DropStatusId == (int)DropStatus.NoFuelNeeded || asset.DropStatusId == (int)DropStatus.AssetNotAvailable)))
                {
                    asset.DropDate = invoice.DropEndDate;
                    asset.DroppedBy = invoice.CreatedBy;
                    asset.UpdatedBy = invoice.UpdatedBy;
                    asset.UpdatedDate = DateTimeOffset.Now;
                    DateTimeOffset endDate = asset.DropDate.Date.Add(Convert.ToDateTime(asset.EndTime).TimeOfDay);
                    var subcontractor = subcontractors.FirstOrDefault(t => t.Id == asset.JobXAssetId && (t.AssignedDate == null || t.AssignedDate.Value.ToTargetDateTimeOffset(t.Timezone).DateTime <= endDate.DateTime)
                                                                && (t.RemovedDate == null || t.RemovedDate.Value.ToTargetDateTimeOffset(t.Timezone).DateTime >= endDate.DateTime));
                    if (subcontractor != null)
                    {
                        asset.SubcontractorName = subcontractor.SubName;
                        asset.SubcontractorId = subcontractor.SubId;
                    }
                    var contractNumber = subcontractors.FirstOrDefault(t => t.Id == asset.JobXAssetId && (t.AddedDate == null || t.AddedDate.Value.ToTargetDateTimeOffset(t.Timezone).DateTime <= endDate.DateTime)
                                                                && (t.DeletedDate == null || t.DeletedDate.Value.ToTargetDateTimeOffset(t.Timezone).DateTime >= endDate.DateTime));
                    if (contractNumber != null)
                    {
                        asset.ContractNumber = contractNumber.ContractNum;
                    }
                    invoice.AssetDrops.Add(asset.ToEntity());
                }
            }
        }


        private List<DeliveryReqStatusUpdateModel> UpdateInvoiceTrackableScheduleId(InvoiceViewModel viewModel, Invoice invoice, int oldTrackableScheduleId, Order order)
        {
            List<DeliveryReqStatusUpdateModel> delReqStatuses = new List<DeliveryReqStatusUpdateModel>();
            if (viewModel.TrackableScheduleId != null)
            {
                var deliveryScheduleXTrackableSchedule = order.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.Id == viewModel.TrackableScheduleId);
                if (invoice.TrackableScheduleId != viewModel.TrackableScheduleId)
                {
                    deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId = GetDeliveryScheduleStatus(invoice, deliveryScheduleXTrackableSchedule);
                    invoice.TrackableScheduleId = viewModel.TrackableScheduleId;
                    if (!string.IsNullOrWhiteSpace(deliveryScheduleXTrackableSchedule.FrDeliveryRequestId))
                    {
                        delReqStatuses.Add(new DeliveryReqStatusUpdateModel()
                        {
                            DeliveryRequestId = deliveryScheduleXTrackableSchedule.FrDeliveryRequestId,
                            ScheduleStatusId = deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId,
                            UserId = invoice.UpdatedBy
                        });
                    }
                }

                if (oldTrackableScheduleId != 0 && oldTrackableScheduleId != viewModel.TrackableScheduleId)
                {
                    var oldDeliveryScheduleXTrackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.Id == oldTrackableScheduleId).First();
                    oldDeliveryScheduleXTrackableSchedule.Invoices.Where(t => t.IsActiveInvoice).ToList().ForEach(t => t.TrackableScheduleId = null);
                    oldDeliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId = GetDeliveryScheduleStatus(invoice, oldDeliveryScheduleXTrackableSchedule, true);
                    Context.DataContext.Entry(oldDeliveryScheduleXTrackableSchedule).State = EntityState.Modified;
                    if (!string.IsNullOrWhiteSpace(oldDeliveryScheduleXTrackableSchedule.FrDeliveryRequestId))
                    {
                        delReqStatuses.Add(new DeliveryReqStatusUpdateModel()
                        {
                            DeliveryRequestId = oldDeliveryScheduleXTrackableSchedule.FrDeliveryRequestId,
                            ScheduleStatusId = oldDeliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId,
                            UserId = invoice.UpdatedBy
                        });
                    }
                }
                Context.DataContext.Entry(deliveryScheduleXTrackableSchedule).State = EntityState.Modified;
            }
            else if (oldTrackableScheduleId != 0)
            {
                var oldDeliveryScheduleXTrackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.Id == oldTrackableScheduleId).First();
                oldDeliveryScheduleXTrackableSchedule.Invoices.Where(t => t.IsActiveInvoice).ToList().ForEach(t => t.TrackableScheduleId = null);
                oldDeliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId = GetDeliveryScheduleStatus(invoice, oldDeliveryScheduleXTrackableSchedule, true);
                Context.DataContext.Entry(oldDeliveryScheduleXTrackableSchedule).State = EntityState.Modified;
                if (!string.IsNullOrWhiteSpace(oldDeliveryScheduleXTrackableSchedule.FrDeliveryRequestId))
                {
                    delReqStatuses.Add(new DeliveryReqStatusUpdateModel()
                    {
                        DeliveryRequestId = oldDeliveryScheduleXTrackableSchedule.FrDeliveryRequestId,
                        ScheduleStatusId = oldDeliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId,
                        UserId = invoice.UpdatedBy
                    });
                }
            }
            return delReqStatuses;
        }

        private void UpdateInvoiceNumber(Invoice invoice, InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel)
        {
            if (invoiceViewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice || invoiceViewModel.StatusId == (int)InvoiceStatus.WaitingForApproval || IsDigitalDropTicket(invoiceViewModel.InvoiceTypeId))
            {
                invoiceViewModel.DisplayInvoiceNumber = invoiceViewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
                manualInvoiceModel.DisplayInvoiceNumber = invoiceViewModel.DisplayInvoiceNumber;
                manualInvoiceModel.InvoiceNumber.Number = invoiceViewModel.DisplayInvoiceNumber;
                invoiceViewModel.InvoiceNumber.Number = invoiceViewModel.DisplayInvoiceNumber;

                invoice.DisplayInvoiceNumber = invoiceViewModel.DisplayInvoiceNumber;
            }
        }

        private async Task<StatusViewModel> GenerateInvoiceFromDDT(ManualInvoiceViewModel manualInvoiceModel, int parentId)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("InvoiceDomain", "GenerateInvoiceFromDDT"))
            {
                var viewModel = new InvoiceViewModel();
                SetManualInputs(viewModel, manualInvoiceModel);
                var invoice = new Invoice();
                int originalInvoiceTypeId = viewModel.InvoiceTypeId;
                var originalWaitingFor = viewModel.WaitingForAction;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                        var originalinvoice = new Invoice();

                        int oldTrackableScheduleId = 0;
                        decimal totalDelivered = 0;

                        List<AssetDropViewModel> assetDrops = null;
                        if (manualInvoiceModel.Assets != null)
                        {
                            assetDrops = manualInvoiceModel.Assets;
                        }

                        var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.OrderId);
                        if (order != null)
                        {
                            var timeZoneName = order.FuelRequest.Job.TimeZoneName;
                            manualInvoiceModel.TimeZoneName = timeZoneName;
                            viewModel.TimeZoneName = timeZoneName;
                            SetInvoiceDueDate(manualInvoiceModel, viewModel);

                            var notificationEvent = EventType.InvoiceTaxValuesChanged;
                            var invoiceNumber = Context.DataContext.InvoiceNumbers.FirstOrDefault(t => t.Id == manualInvoiceModel.InvoiceNumber.Id);
                            if (invoiceNumber == null)
                            {
                                notificationEvent = order.FuelRequest.Job != null && order.FuelRequest.Job.IsApprovalWorkflowEnabled ? EventType.InvoiceCreatedApprovalWorkflow : EventType.CreateInvoiceFromDDT;
                                invoiceNumber = manualInvoiceModel.InvoiceNumber.ToEntity();
                                Context.DataContext.InvoiceNumbers.Add(invoiceNumber);
                                await Context.CommitAsync();

                                viewModel.InvoiceNumber.Id = invoiceNumber.Id;
                                viewModel.DisplayInvoiceNumber = invoiceNumber.Number;
                                viewModel.InvoiceNumber.Number = invoiceNumber.Number;
                                manualInvoiceModel.DisplayInvoiceNumber = invoiceNumber.Number;
                            }
                            else
                            {
                                originalinvoice = invoiceNumber.InvoiceHeaderDetails.SelectMany(t => t.Invoices).FirstOrDefault(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active);
                                oldTrackableScheduleId = originalinvoice.TrackableScheduleId != null ? originalinvoice.TrackableScheduleId.Value : 0;
                                viewModel.BrokeredChainId = originalinvoice.BrokeredChainId;
                                manualInvoiceModel.IsInvoiceEdit = true;
                                viewModel.ExchangeRate = originalinvoice.ExchangeRate;
                                viewModel.DisplayInvoiceNumber = originalinvoice.DisplayInvoiceNumber;
                                manualInvoiceModel.DisplayInvoiceNumber = originalinvoice.DisplayInvoiceNumber;
                                SetImageFlags(manualInvoiceModel, viewModel, originalinvoice);
                            }

                            viewModel.TerminalId = manualInvoiceModel.TerminalId;
                            viewModel.CityGroupTerminalId = manualInvoiceModel.CityGroupTerminalId;
                            viewModel.TerminalPricingDate = viewModel.DropEndDate;
                            manualInvoiceModel.TerminalPricingDate = viewModel.DropEndDate;

                            if (manualInvoiceModel.IsInvoiceEdit && parentId == 0)
                            {
                                var invoices = invoiceNumber.InvoiceHeaderDetails.SelectMany(t => t.Invoices).FirstOrDefault(t => t.ParentId != null);
                                parentId = invoices != null ? Convert.ToInt32(invoices.ParentId) : 0;
                            }
                            CheckWorkflowAndSetInvoiceCreationType(order, viewModel, manualInvoiceModel, parentId);

                            if (!viewModel.IsApprovalWorkflowEnabledForJob)
                            {
                                if (order.ExternalBrokerBuySellDetail == null)
                                {
                                    if (!string.IsNullOrEmpty(manualInvoiceModel.SplitLoadChainId))
                                    {
                                        await SetSplitInvoiceAmounts(order, viewModel);
                                    }
                                    else
                                    {
                                        await SetInvoiceAmounts(order, viewModel);
                                    }
                                }
                                else
                                {
                                    await SetInvoiceAmountsForBuySellOrder(order, viewModel, viewModel.IsBuyPriceInvoice);
                                }
                                if (viewModel.WaitingForAction != (int)WaitingAction.Nothing)
                                {
                                    SetInvoiceCreationTypeToDdt(viewModel, manualInvoiceModel);
                                }
                            }

                            if (viewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice && originalWaitingFor == (int)WaitingAction.UpdatedPrice)
                            {
                                viewModel.PricePerGallon = 0;
                                viewModel.BasicAmount = 0;
                                response.StatusMessage = Resource.errMessagePricingNotAvailable;
                                transaction.Rollback();
                                return response;
                            }

                            if (manualInvoiceModel.TypeofFuel == (int)ProductDisplayGroups.OtherFuelType)
                            {
                                SetTaxAmountForEditNonStandardProduct(viewModel, manualInvoiceModel, order);
                            }
                            else if (viewModel.WaitingForAction == (int)WaitingAction.Nothing && !viewModel.IsApprovalWorkflowEnabledForJob && viewModel.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                            {
                                SetTaxAmountForStandardProduct(viewModel, manualInvoiceModel, order, false, originalInvoiceTypeId);
                            }

                            if (viewModel.IsTaxServiceFailure && parentId > 0)
                            {
                                response.StatusMessage = Resource.errMessageTaxCalculationFailed;
                                transaction.Rollback();

                                //Incase of Mobile Drop and if Avalara Product Not Mapped set DDTConversion = 1
                                if (manualInvoiceModel.DDTConversionReason == (int)DDTConversionReason.Nothing &&
                                        viewModel.DDTConversionReason == (int)DDTConversionReason.AvalaraProductNotMapped)
                                {
                                    await UpdateDDTConversionReason(manualInvoiceModel.ConversionDDTId, viewModel.DDTConversionReason);
                                }

                                return response;
                            }

                            if (string.IsNullOrEmpty(viewModel.TransactionId) || viewModel.TransactionId == "0")
                            {
                                viewModel.TransactionId = invoiceNumber.Number;
                            }
                            var createdDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);

                            var isConnectedWithBuyer = false;
                            if (order.FuelRequest.Job.IsApprovalWorkflowEnabled && order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                            {
                                if (order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.FuelRequest1 != null)
                                {
                                    var brokerOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.OrderByDescending(t => t.Id).FirstOrDefault();
                                    var parentOrder = GetConnectingBuyerOrder(brokerOrder);
                                    if (parentOrder == null)
                                    {
                                        isConnectedWithBuyer = true;
                                    }
                                }
                            }

                            Order brokeredOrder = null;
                            if (!manualInvoiceModel.IsInvoiceEdit && !manualInvoiceModel.IsInvoiceFromDropTicket && order.FuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                            {
                                var childRequest = order.FuelRequest.GetParentFuelRequest().FuelRequest1;
                                brokeredOrder = childRequest.Orders.LastOrDefault();
                                SetBrokeredChainId(viewModel);
                            }

                            var previousStatus = order.Invoices.FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id && t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.WaitingForApproval));
                            if (viewModel.StatusId == (int)InvoiceStatus.Received && ((order.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.GetParentFuelRequest().FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.Job.IsApprovalWorkflowEnabled) || (isConnectedWithBuyer)))
                            {
                                if (parentId > 0)
                                {
                                    viewModel.StatusId = Context.DataContext.Invoices.First(t => t.Id == parentId).InvoiceXInvoiceStatusDetails.First(t => t.IsActive).StatusId;
                                }
                                else if (previousStatus == null)
                                {
                                    viewModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                                }
                                else
                                {
                                    var lastInvoice = order.Invoices.OrderByDescending(t => t.Id).FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id);
                                    viewModel.StatusId = lastInvoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault().StatusId;
                                    if (lastInvoice != null && lastInvoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault().StatusId == (int)InvoiceStatus.Rejected)
                                    {
                                        if (!lastInvoice.InvoiceXInvoiceStatusDetails.Any(t => t.StatusId == (int)InvoiceStatus.Received))
                                        {
                                            viewModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                                        }
                                        else
                                        {
                                            viewModel.StatusId = (int)InvoiceStatus.Received;
                                        }
                                    }
                                }

                                var parentFr = order.FuelRequest.GetParentFuelRequest();
                                if (!manualInvoiceModel.IsInvoiceFromDropTicket && parentFr.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && parentFr.FuelRequest1 != null)
                                {
                                    if (brokeredOrder != null && order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery &&
                                        (
                                            brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyClosed
                                            || brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed
                                        ))
                                    {
                                        viewModel.StatusId = (int)InvoiceStatus.Received;
                                    }
                                }
                            }

                            await SetInvoiceAdditionDetails(viewModel, manualInvoiceModel.ConversionDDTId, 0);
                            invoice = viewModel.ToEntity();
                            invoice.SignatureId = manualInvoiceModel.SignatureId;
                            if (!manualInvoiceModel.IsInvoiceEdit)
                            {
                                invoice.SupplierPreferredInvoiceTypeId = originalInvoiceTypeId;
                            }
                            if (viewModel.Image != null && viewModel.Image.IsRemoved)
                            {
                                var image = await Context.DataContext.Images.FirstOrDefaultAsync(t => t.Id == viewModel.Image.Id);
                                if (image != null)
                                {
                                    invoice.Image = null;
                                    Context.DataContext.Images.Remove(image);
                                }
                            }
                            if (parentId > 0)
                            {
                                // DDT to invoice create
                                invoice.ParentId = parentId;
                                var parentInvoice = Context.DataContext.Invoices.FirstOrDefault(t => t.Id == parentId);
                                if (parentInvoice != null)
                                {
                                    SetImageFlags(manualInvoiceModel, viewModel, parentInvoice, invoice);

                                    parentInvoice.IsActive = false;
                                    parentInvoice.WaitingFor = (int)WaitingAction.Nothing;
                                    parentInvoice.DDTConversionReason = (int)DDTConversionReason.Nothing;
                                    Context.DataContext.Entry(parentInvoice).State = EntityState.Modified;
                                    await Context.CommitAsync();

                                    if (parentInvoice.InvoiceDispatchLocation.Any(t => t.LocationType == (int)LocationType.PickUp) && !invoice.InvoiceDispatchLocation.Any(t => t.LocationType == (int)LocationType.PickUp))
                                    {
                                        var picupLocation = GetDispatchLocation(parentInvoice.InvoiceDispatchLocation, viewModel, LocationType.PickUp);
                                        invoice.InvoiceDispatchLocation.Add(picupLocation.ToEntity());
                                    }
                                    if (!invoice.InvoiceDispatchLocation.Any(t => t.LocationType == (int)LocationType.Drop) && parentInvoice.InvoiceDispatchLocation.Any(t => t.LocationType == (int)LocationType.Drop))
                                    {
                                        var dropLocation = GetDispatchLocation(parentInvoice.InvoiceDispatchLocation, viewModel, LocationType.Drop);
                                        invoice.InvoiceDispatchLocation.Add(dropLocation.ToEntity());
                                    }
                                }
                            }

                            //FuelRequestFee Entity
                            if (manualInvoiceModel.ExternalBrokerId <= 0)
                            {
                                invoice.FuelRequestFees = manualInvoiceModel.FuelDeliveryDetails.FuelFees.ToInvoiceFeesEntity(invoice.DropStartDate.Date);
                            }
                            else
                            {
                                invoice.FuelRequestFees = manualInvoiceModel.ExternalBrokeredOrder.BrokeredOrderFee.ToEntity();
                            }

                            //Tax details Entity
                            invoice.TaxDetails = viewModel.TaxDetails.ToEntity();
                            invoice.InvoiceXAccessorialFees = manualInvoiceModel.AccessorialFeeDetails.Select(t => t.ToEntity()).ToList();

                            if (manualInvoiceModel.IsInvoiceEdit)
                            {
                                if (manualInvoiceModel.TypeofFuel != (int)ProductDisplayGroups.OtherFuelType)
                                {
                                    TaxUpdationForStandardFuelType(originalinvoice, invoice);
                                }
                            }

                            invoice.InvoiceXAdditionalDetail.PaymentMethod = manualInvoiceModel.PaymentMethod;
                            invoice.PaymentTermId = manualInvoiceModel.PaymentTermId;
                            if (manualInvoiceModel.PaymentTermId == (int)PaymentTerms.NetDays)
                            {
                                invoice.NetDays = manualInvoiceModel.NetDays;
                            }

                            invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.Active;
                            invoice.Version = Context.DataContext.Invoices.Count(t => t.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id) + 1;
                            invoice.CreatedBy = viewModel.UserId;
                            invoice.UpdatedBy = viewModel.UserId;
                            invoice.CreatedDate = createdDate;
                            invoice.Currency = order.FuelRequest.Currency;
                            invoice.UoM = order.FuelRequest.UoM;

                            // set no-data exception status
                            if (invoice.WaitingFor == (int)WaitingAction.Images)
                                invoice.InvoiceXAdditionalDetail.NoDataExceptionApprovalId = (int)NoDataExceptionApproval.UploadImages;

                            var invoiceHeader = GenerateInvoiceHeader(new List<InvoiceViewModel>() { viewModel });
                            invoiceHeader.Invoices.Add(invoice);
                            await Context.CommitAsync();

                            SetInvoiceBolDetails(invoiceHeader, viewModel, invoice);

                            var job = order.FuelRequest.Job;
                            if (job.JobBudget.IsAssetTracked && assetDrops != null && assetDrops.Count > 0)
                            {
                                InvoiceCommonDomain invoiceCommonDomain = new InvoiceCommonDomain(this);
                                assetDrops = invoiceCommonDomain.SetJobAssetId(assetDrops, viewModel.UserId, job);
                                SetAssetDropsToInvoice(invoice, assetDrops);
                            }
                            //need to get fees after adding asset drops
                            var totalFeeAmount = new HelperDomain(this).SetCalculatedInvoiceFeesTotal(invoice);
                            invoice.TotalFeeAmount = invoiceHeader.TotalFeeAmount = totalFeeAmount;

                            var invoiceProcessingFee = invoice.FuelRequestFees.Where(t => t.FeeTypeId == (int)FeeType.ProcessingFee && t.FeeSubTypeId == (int)FeeSubType.Percent).FirstOrDefault();
                            if (invoiceProcessingFee != null)
                            {
                                var totalAmount = (invoice.BasicAmount + (invoice.TotalFeeAmount ?? 0) + invoice.TotalTaxAmount - invoice.TotalDiscountAmount);
                                invoiceProcessingFee.TotalFee = totalAmount * invoiceProcessingFee.Fee / 100;
                                invoice.TotalFeeAmount = invoiceProcessingFee.TotalFee + (invoice.TotalFeeAmount ?? 0);
                            }

                            if (invoice.InvoiceTypeId != (int)InvoiceType.DryRun && parentId == 0)
                            {
                                AutoCloseOrder(order, out totalDelivered, viewModel.TrackableScheduleId);
                            }

                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            var delReqStatuses = UpdateInvoiceTrackableScheduleId(viewModel, invoice, oldTrackableScheduleId, order);

                            await Context.CommitAsync();
                            transaction.Commit();

                            CreateQbAccountingWorkflowForInvoice(manualInvoiceModel.IsInvoiceEdit, invoice, order, brokeredOrder?.Id);
                            CreateQbAccountingWorkflowForBill(manualInvoiceModel.IsInvoiceEdit, invoice, order, brokeredOrder?.Id);
                            bool isDtnUploaded = CreateDtnFileGenerationWorkflow(invoice, order);
                            if (delReqStatuses != null && delReqStatuses.Any())
                            {
                                new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatuses);
                            }
                            var notificationDomain = new NotificationDomain(this);
                            var userId = viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp ? order.AcceptedBy : viewModel.UserId;
                            if (notificationEvent != EventType.InvoiceTaxValuesChanged)//adding this as logic is not implemented for this event
                            {
                                if (invoice.WaitingFor == (int)WaitingAction.UpdatedPrice)
                                {
                                    notificationEvent = EventType.DdtCreatedAsInvoiceIsWaitingForUpdatedPrice;
                                }
                                else if (invoice.WaitingFor == (int)WaitingAction.AvalaraTax)
                                {
                                    notificationEvent = EventType.DDTCreateAsInvoiceWaitingForTaxes;
                                }
                                else if (invoice.WaitingFor == (int)WaitingAction.Nothing && originalWaitingFor == (int)WaitingAction.AvalaraTax)
                                {
                                    notificationEvent = EventType.InvoiceGeneratedEstablishConnectionWithAvalara;
                                }
                                await notificationDomain.AddNotificationEventAsync(notificationEvent, invoice.InvoiceHeaderId, userId);
                            }

                            // add taxes from ddt into hedge dropped amount or spot droppedd amount
                            AddHedgeSpotAmountsOfInvoiceCreatedFromDDT(order, invoice, viewModel.IsRecursiveCallForBrokerOrders);
                            await Context.CommitAsync();

                            response.EntityId = invoice.Id;
                            response.StatusCode = Status.Success;
                            if (viewModel.IsTaxServiceFailure && originalInvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual)
                            {
                                response.StatusMessage = Resource.successMessageDDTCreatedDueToAvaTaxFailure;
                            }
                            else
                            {
                                if (IsDigitalDropTicket(invoice.InvoiceTypeId) && viewModel.IsApprovalWorkflowEnabledForJob)
                                {
                                    response.StatusMessage = Resource.SuccessMessgeDDTCreatedAsApprovalWorkflowIsEnabled;
                                }
                                else if (IsDigitalDropTicket(invoice.InvoiceTypeId) && viewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice)
                                {
                                    response.StatusMessage = Resource.errMessageDDTCreatedWaitingForUpdatedPrice;
                                }
                                else
                                {
                                    response.StatusMessage = IsDigitalDropTicket(invoice.InvoiceTypeId) ? Resource.errMessageDropTicketCreateSuccess : Resource.errMessageInvoiceCreateSuccess;
                                }
                            }
                            //ProcessInvoicesForWebNotifications(invoice, order, null);
                            UpdateInvoiceActionResponseStatus(isDtnUploaded, response);
                        }

                        viewModel = invoice.ToViewModel(viewModel);
                        manualInvoiceModel.InvoiceId = invoice.Id;
                        manualInvoiceModel.InvoiceHeaderId = invoice.InvoiceHeaderId;
                        manualInvoiceModel.BuyerCompanyId = invoice.Order.FuelRequest.Job.CompanyId;
                        manualInvoiceModel.InvoiceNumber.Number = invoice.DisplayInvoiceNumber;
                        manualInvoiceModel.JobId = invoice.Order.FuelRequest.JobId;
                        if (order != null)
                        {
                            manualInvoiceModel.SupplierCompanyId = order.AcceptedCompanyId;
                        }
                        manualInvoiceModel.InvoiceTypeId = viewModel.InvoiceTypeId;
                        manualInvoiceModel.OrderId = viewModel.OrderId ?? 0;
                        manualInvoiceModel.WaitingForAction = viewModel.WaitingForAction;
                        manualInvoiceModel.DisplayInvoiceNumber = invoice.DisplayInvoiceNumber;

                        if (!manualInvoiceModel.IsInvoiceEdit && viewModel.StatusId != (int)InvoiceStatus.Draft)
                        {
                            await SetApprovalWorkflowEnabledNewsFeeds(invoice, newsfeedDomain, order);
                        }
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("InvoiceDomain", "GenerateInvoiceFromDDT", ex.Message + " Drop Ticket Id : " + parentId, ex);
                    }
                }
            }
            return response;
        }

        private void SetImageFlags(ManualInvoiceViewModel manualInvoiceModel, InvoiceViewModel viewModel, Invoice originalinvoice, Invoice newInvoice = null)
        {
            viewModel.IsBOLImageReq = originalinvoice.IsBolImageReq;
            viewModel.IsDropImageReq = originalinvoice.IsDropImageReq;
            viewModel.IsSignatureReq = originalinvoice.IsSignatureReq;
            if (originalinvoice.WaitingFor == (int)WaitingAction.Images)
            {
                CheckRequiredImagesAndSetWaitingForImageAction(manualInvoiceModel.IsFTL, originalinvoice, manualInvoiceModel, viewModel);
                if (newInvoice != null)
                {
                    newInvoice.WaitingFor = viewModel.WaitingForAction;
                    newInvoice.InvoiceTypeId = viewModel.InvoiceTypeId;
                    newInvoice.IsBolImageReq = originalinvoice.IsBolImageReq;
                    newInvoice.IsDropImageReq = originalinvoice.IsDropImageReq;
                    newInvoice.IsSignatureReq = originalinvoice.IsSignatureReq;
                }
            }
        }

        private void CheckRequiredImagesAndSetWaitingForImageAction(bool isFTL, Invoice invoice, ManualInvoiceViewModel manualInvoiceModel, InvoiceViewModel viewModel)
        {
            var isBolImgProvided = false;
            var isDropImgProvided = false;
            var issiggImgProvided = false;

            if (invoice.IsSignatureReq || invoice.IsBolImageReq || invoice.IsDropImageReq)
            {
                if (isFTL)
                {
                    if (invoice.IsSignatureReq)
                    {
                        if ((invoice.Signaure != null && invoice.Signaure.Image != null && !string.IsNullOrWhiteSpace(invoice.Signaure.Image?.FilePath)) || invoice.Signaure.Id > 0)
                            issiggImgProvided = true;
                    }
                    else
                        issiggImgProvided = true;

                    if (invoice.IsBolImageReq)
                    {
                        if ((viewModel.BolImage != null && !string.IsNullOrWhiteSpace(viewModel.BolImage?.FilePath)) || viewModel.BolImage.Id > 0)
                            isBolImgProvided = true;
                    }
                    else
                        isBolImgProvided = true;

                    if (invoice.IsDropImageReq)
                    {
                        if ((viewModel.Image != null && !string.IsNullOrWhiteSpace(viewModel.Image?.FilePath)) || viewModel.Image.Id > 0)
                            isDropImgProvided = true;
                    }
                    isDropImgProvided = true;

                    var canSetToNothing = (isBolImgProvided && issiggImgProvided && isDropImgProvided);

                    if (canSetToNothing)
                        SetWaitingActionToNothing(manualInvoiceModel, viewModel);
                    else
                        SetWaitingActionToImages(viewModel);
                }
                else
                {
                    if (invoice.IsSignatureReq)
                    {
                        if ((invoice.Signaure != null && invoice.Signaure.Image != null && !string.IsNullOrWhiteSpace(invoice.Signaure.Image?.FilePath)) || invoice.Signaure.Id > 0)
                            issiggImgProvided = true;
                    }
                    else
                        issiggImgProvided = true;

                    if (invoice.IsDropImageReq)
                    {
                        if ((viewModel.Image != null && !string.IsNullOrWhiteSpace(viewModel.Image?.FilePath)) || viewModel.Image.Id > 0)
                            isDropImgProvided = true;
                    }
                    else
                        isDropImgProvided = true;

                    var canSetToNothing = (issiggImgProvided && isDropImgProvided);

                    if (canSetToNothing)
                        SetWaitingActionToNothing(manualInvoiceModel, viewModel);
                    else
                        SetWaitingActionToImages(viewModel);
                }
            }
        }

        private bool SetInvoiceRequiredImages(dynamic invoice, ManualInvoiceViewModel manualInvoiceModel)
        {
            var isBolImgProvided = false;
            var isDropImgProvided = false;
            var issiggImgProvided = false;
            bool hasRequiredInvoiceImages = false;

            if (invoice.IsSignatureReq || invoice.IsBolImageReq || invoice.IsDropImageReq)
            {
                if (invoice.IsSignatureReq)
                {
                    if ((manualInvoiceModel.SignatureImage != null && !string.IsNullOrWhiteSpace(manualInvoiceModel.SignatureImage?.FilePath)) || manualInvoiceModel.SignatureImage?.Id > 0)
                        issiggImgProvided = true;
                }
                else
                {
                    issiggImgProvided = true;
                }

                if (invoice.IsBolImageReq)
                {
                    if ((manualInvoiceModel.BolImage != null && !string.IsNullOrWhiteSpace(manualInvoiceModel.BolImage?.FilePath)) || manualInvoiceModel.BolImage?.Id > 0)
                        isBolImgProvided = true;
                }
                else
                {
                    isBolImgProvided = true;
                }

                if (invoice.IsDropImageReq)
                {
                    if ((manualInvoiceModel.InvoiceImage != null && !string.IsNullOrWhiteSpace(manualInvoiceModel.InvoiceImage?.FilePath)) || manualInvoiceModel.InvoiceImage?.Id > 0)
                        isDropImgProvided = true;
                }
                else
                {
                    isDropImgProvided = true;
                }

                hasRequiredInvoiceImages = (isBolImgProvided && issiggImgProvided && isDropImgProvided);

                if (hasRequiredInvoiceImages)
                {
                    manualInvoiceModel.WaitingForAction = (int)WaitingAction.Nothing;
                    manualInvoiceModel.InvoiceTypeId = (int)InvoiceType.Manual;
                    manualInvoiceModel.IsInvoiceImagesAvailable = true;
                    manualInvoiceModel.IsApprovalWorkflowEnabledForJob = invoice.IsApprovalWorkflowEnabled;
                }
                else
                {
                    manualInvoiceModel.WaitingForAction = (int)WaitingAction.Images;
                    manualInvoiceModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(manualInvoiceModel.InvoiceTypeId);
                }
            }

            return hasRequiredInvoiceImages;
        }

        private static void SetWaitingActionToImages(InvoiceViewModel viewModel)
        {
            viewModel.WaitingForAction = (int)WaitingAction.Images;
            viewModel.InvoiceTypeId = GetInvoiceCreationTypeToDdt(viewModel.InvoiceTypeId);
        }

        private static void SetWaitingActionToNothing(ManualInvoiceViewModel manualInvoiceModel, InvoiceViewModel viewModel)
        {
            if (viewModel.WaitingForAction == (int)WaitingAction.Images)
            {
                viewModel.WaitingForAction = (int)WaitingAction.Nothing;
                manualInvoiceModel.WaitingForAction = (int)WaitingAction.Nothing;
                viewModel.InvoiceTypeId = (int)InvoiceType.Manual;
            }
        }

        private DispatchLocationViewModel GetDispatchLocation(ICollection<InvoiceDispatchLocation> invoiceDispatchLocation, InvoiceViewModel viewModel, LocationType locationType)
        {
            var invoiceLocation = invoiceDispatchLocation.Where(t => t.LocationType == (int)locationType).FirstOrDefault();
            if (invoiceLocation != null)
            {
                var pickUplocation = new DispatchLocationViewModel();
                pickUplocation.Address = invoiceLocation.Address;
                pickUplocation.City = invoiceLocation.City;
                pickUplocation.CountryCode = invoiceLocation.CountryCode;
                pickUplocation.CountyName = invoiceLocation.CountyName;
                pickUplocation.CreatedBy = viewModel.UserId;
                pickUplocation.CreatedDate = viewModel.CreatedDate;
                pickUplocation.Currency = viewModel.Currency;
                pickUplocation.IsValidAddress = true;
                pickUplocation.IsVariousFobOriginType = viewModel.IsVariousFobOrigin;
                pickUplocation.Latitude = invoiceLocation.Latitude;
                pickUplocation.LocationType = invoiceLocation.LocationType;
                pickUplocation.Longitude = invoiceLocation.Longitude;
                pickUplocation.OrderId = invoiceLocation.OrderId;
                pickUplocation.StateCode = invoiceLocation.StateCode;
                pickUplocation.StateId = invoiceLocation.StateId ?? 0;
                pickUplocation.ZipCode = invoiceLocation.ZipCode;

                return pickUplocation;
            }
            return null;
        }

        private async Task SetNewsfeedForManualCreateInvoiceAsync(UserContext userContext, InvoiceViewModel viewModel, ManualInvoiceViewModel manualInvoiceModel, bool isInvoiceEdit)
        {
            var newsfeed = new NewsfeedDomain(this);
            if (viewModel.StatusId == (int)InvoiceStatus.Draft && isInvoiceEdit)
            {
                await newsfeed.SetDraftDDTModifiedNewsfeed(userContext, manualInvoiceModel, isInvoiceEdit);
            }
            else if (viewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice && !isInvoiceEdit)
            {
                await newsfeed.SetSupplierCreatedDDTWaitingForUpdatedPriceNewsfeed(userContext, manualInvoiceModel);
            }
            else if (viewModel.WaitingForAction == (int)WaitingAction.AvalaraTax && !isInvoiceEdit)
            {
                await newsfeed.SetSupplierCreatedDDTWaitingForTaxesNewsfeed(userContext, manualInvoiceModel);
            }
            else if (isInvoiceEdit || viewModel.WaitingForAction != (int)WaitingAction.CustomerApproval)
            {
                await newsfeed.SetInvoiceCreatedNewsfeed(userContext, manualInvoiceModel, isInvoiceEdit);
            }
        }

        private async Task SetNewsfeedForManualCreateInvoiceMobileAsync(Order order, InvoiceViewModel invoiceViewModel)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            var appMessageDomain = new AppMessageDomain(this);
            if (invoiceViewModel.StatusId == (int)InvoiceStatus.Draft)
            {
                await newsfeedDomain.SetSystemDigitalDropTicketDraftCreatedNewsfeed(order, invoiceViewModel);
                await appMessageDomain.SendDraftDDTMessage(order, invoiceViewModel);
            }
            else if (invoiceViewModel.WaitingForAction == (int)WaitingAction.CustomerApproval)
            {
                await newsfeedDomain.SetDriverDroppedInvoiceCreatedWaitingForNewsfeed(order, invoiceViewModel, NewsfeedEvent.DriverDropsWaitingForApproval);
            }
            else if (invoiceViewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice)
            {
                await newsfeedDomain.SetDriverDroppedInvoiceCreatedWaitingForNewsfeed(order, invoiceViewModel, NewsfeedEvent.DriverDropsWaitingForUpdatedPrice);
            }
            else
            {
                await newsfeedDomain.SetSystemInvoiceCreatedNewsfeed(order, invoiceViewModel);
            }
        }

        private void CheckWorkflowAndSetInvoiceCreationType(Order order, InvoiceViewModel viewModel, ManualInvoiceViewModel manualInvoiceModel, int parentId)
        {
            if (parentId <= 0 || viewModel.PreviousStatusId == (int)InvoiceStatus.Unassigned || manualInvoiceModel.WaitingForAction == (int)WaitingAction.BolDetails
                || manualInvoiceModel.WaitingForAction == (int)WaitingAction.Images || manualInvoiceModel.IsInvoiceImagesAvailable)
            {
                if (manualInvoiceModel.WaitingForAction == (int)WaitingAction.BolDetails)
                {
                    manualInvoiceModel.WaitingForAction = (int)WaitingAction.Nothing;
                    viewModel.WaitingForAction = (int)WaitingAction.Nothing;
                }

                var job = order.FuelRequest.Job;
                if (job != null && job.IsApprovalWorkflowEnabled)
                {
                    viewModel.IsApprovalWorkflowEnabledForJob = job.IsApprovalWorkflowEnabled;
                    viewModel.WaitingForAction = (int)WaitingAction.CustomerApproval;
                    viewModel.StatusId = (int)InvoiceStatus.WaitingForApproval;

                    manualInvoiceModel.IsApprovalWorkflowEnabledForJob = job.IsApprovalWorkflowEnabled;
                    manualInvoiceModel.WaitingForAction = (int)WaitingAction.CustomerApproval;
                    manualInvoiceModel.StatusId = (int)InvoiceStatus.WaitingForApproval;

                    SetInvoiceCreationTypeToDdt(viewModel, manualInvoiceModel);
                    viewModel.TerminalId = viewModel.TerminalId ?? order.TerminalId ?? order.FuelRequest.TerminalId;
                    viewModel.CityGroupTerminalId = order.CityGroupTerminalId;
                }
            }
        }

        private void TaxUpdationForStandardFuelType(Invoice originalinvoice, Invoice invoice)
        {
            if (!Enumerable.SequenceEqual(originalinvoice.TaxDetails.Select(t => t.TradingTaxAmount.GetPreciseValue(2)), invoice.TaxDetails.Select(t => t.TradingTaxAmount.GetPreciseValue(2))))
            {
                for (int i = 0; i < invoice.TaxDetails.Count; i++)
                {
                    if (originalinvoice.TaxDetails.Count == 0)
                    {
                        invoice.TaxDetails.ToList()[i].IsModified = true;
                    }
                    else
                    {
                        if (invoice.TaxDetails.ToList()[i].TradingTaxAmount.GetPreciseValue(2) != originalinvoice.TaxDetails.ToList()[i].TradingTaxAmount.GetPreciseValue(2))
                        {
                            invoice.TaxDetails.ToList()[i].IsModified = true;
                        }
                    }
                }
            }
        }

        private int GetDeliveryScheduleStatus(Invoice invoice, DeliveryScheduleXTrackableSchedule deliveryScheduleXTrackableSchedule, bool isEditing = false)
        {
            var invoiceStatusId = invoice.InvoiceXInvoiceStatusDetails.FirstOrDefault(t => t.IsActive).StatusId;
            var deliveryStatusId = GetDeliveryScheduleStatus(deliveryScheduleXTrackableSchedule, invoice.Order.FuelRequest.Job.TimeZoneName, invoiceStatusId, invoice.DropEndDate, isEditing);
            return deliveryStatusId;
        }

        private List<FeesViewModel> GetFuelRequestFee(FuelRequest fuelRequest)
        {
            List<FeesViewModel> response = new List<FeesViewModel>();
            try
            {
                response.AddRange(fuelRequest.FuelRequestFees.ToFeesViewModel());
                //var deliveryFeeByQuantity = fuelRequest.FeeByQuantities.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();
                //foreach (var item in response)
                //{
                //    var feeByQuantity = deliveryFeeByQuantity.Where(t => t.FeeTypeId.ToString() == item.FeeTypeId && t.FeeSubTypeId == item.FeeSubTypeId);
                //    item.DeliveryFeeByQuantity.AddRange(feeByQuantity);
                //}
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetFuelRequestFee", ex.Message, ex);
            }
            return response;
        }

        //TODO: Verify this method reference
        private FuelRequestFeeViewModel GetInvoiceFee(Invoice invoice)
        {
            FuelRequestFeeViewModel response = new FuelRequestFeeViewModel();
            try
            {
                response = invoice.FuelRequestFees.ToViewModel();
                //response.DeliveryFeeByQuantity = invoice.FeeByQuantities.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();
                invoice.FuelRequestFees.ToList().ForEach(t => { if (t.FeeSubTypeId == (int)FeeSubType.ByQuantity) { response.DeliveryFeeByQuantity.AddRange(t.FeeByQuantities.OrderBy(t1 => t1.MinQuantity).Select(t2 => t2.ToViewModel())); } });
                response.AdditionalFee = invoice.FuelRequestFees.ToAdditionalFeeViewModel().ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceFee", ex.Message, ex);
            }
            return response;
        }

        //private FuelFeesViewModel GetInvoiceDetailsFuelFees(ICollection<FuelFee> fuelFees)
        //{
        //    FuelFeesViewModel response = new FuelFeesViewModel();
        //    try
        //    {
        //        response.FuelRequestFees = fuelFees.Where(t => t.FeeSubTypeId != (int)FeeSubType.NoFee
        //                                    && t.TotalFee.HasValue && t.TotalFee.Value > 0
        //                                    && t.FeeTypeId != (int)FeeType.DryRunFee).OrderBy(t => t.FeeTypeId).ToList().ToFeesViewModel();
        //        if (response.FuelRequestFees != null && response.FuelRequestFees.Any())
        //        {
        //            // calculate fees
        //            foreach (var fee in response.FuelRequestFees)
        //            {
        //                if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && (fee.FeeSubQuantity != null && fee.FeeSubQuantity.Value > 0))
        //                {
        //                    fee.TotalHours = GetHosingTimeInHours(fee.FeeSubQuantity.Value.ToString());
        //                }
        //                else if (fee.FeeSubTypeId == (int)FeeSubType.ByAssetCount && (fee.FeeSubQuantity != null && fee.FeeSubQuantity.Value > 0))
        //                {
        //                    fee.TotalAssetQty = Convert.ToInt64(fee.FeeSubQuantity.Value);
        //                }
        //            }
        //        }

        //        response.DiscountLineItems = fuelFees.Where(t => t.DiscountLineItemId != null).OrderBy(t => t.FeeTypeId).ToList().ToDiscountFeesViewModel();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceDetailsFuelFees", ex.Message, ex);
        //    }
        //    return response;
        //}



        public List<AssetDropViewModel> GetAssetDropDetails(Invoice invoice)
        {
            List<AssetDropViewModel> response = new List<AssetDropViewModel>();

            try
            {

                var mappedToProductTypeIds = new List<int>();
                mappedToProductTypeIds.Add(invoice.Order.FuelRequest.MstProduct.ProductTypeId);
                var productTypeMappings = Context.DataContext.ProductTypeCompatibilityMappings.Where(t => t.ProductTypeId == invoice.Order.FuelRequest.MstProduct.ProductTypeId)
                                                                .Select(t => t.MappedToProductTypeId).ToList();
                if (productTypeMappings != null)
                {
                    mappedToProductTypeIds.AddRange(productTypeMappings);
                }

                List<JobXAsset> jobXAssets;
                if (invoice.Order.FuelRequest.Job.IsMarine)
                {
                    jobXAssets = invoice.Order.FuelRequest.Job.JobXAssets
                                            .Where(t => t.Asset.IsMarine && t.Asset.Type == (int)AssetType.Vessle && t.RemovedBy == null)
                                            .OrderByDescending(t => t.Id).GroupBy(t => t.Asset.Id).Select(grp => grp.OrderByDescending(t => t.Id).First()).ToList();
                }
                else
                {
                    jobXAssets = invoice.Order.FuelRequest.Job.JobXAssets
                                            .Where(t => t.Asset.Type == (int)AssetType.Asset || (t.Asset.FuelType.HasValue && mappedToProductTypeIds.Contains(t.Asset.FuelType.Value)))
                                            .OrderByDescending(t => t.Id).GroupBy(t => t.Asset.Id).Select(grp => grp.OrderByDescending(t => t.Id).First()).ToList();
                }
                foreach (var jobXAsset in jobXAssets)
                {
                    var assetDrop = GetAssetDropViewModel(invoice, jobXAsset);
                    var invoiceAssetDrops = Context.DataContext.AssetDrops.Where(t => t.JobXAssetId == jobXAsset.Id && t.InvoiceId == invoice.Id).ToList();
                    if (invoiceAssetDrops.Count == 0)
                    {
                        response.Add(assetDrop);
                    }
                    else
                    {
                        foreach (var item in invoiceAssetDrops)
                        {
                            assetDrop = GetAssetDropViewModel(invoice, jobXAsset);
                            assetDrop.IsDropMade = true;
                            assetDrop.Id = item.Id;
                            assetDrop.StartTime = item.DropStartDate.DateTime.ToString(Resource.constFormat12HourTime2);
                            assetDrop.EndTime = item.DropEndDate.DateTime.ToString(Resource.constFormat12HourTime2);
                            assetDrop.DropGallons = item.DroppedGallons;
                            assetDrop.Image = item.ImageId.HasValue ? new ImageViewModel() { Id = item.ImageId.Value } : new ImageViewModel();
                            assetDrop.MeterStartReading = item.MeterStartReading;
                            assetDrop.MeterEndReading = item.MeterEndReading;
                            assetDrop.DropStatusId = item.DropStatus;
                            assetDrop.DropStatusName = GetDropStatusName(item.DropStatus);
                            assetDrop.IsNewAsset = item.IsNewAsset;
                            assetDrop.AssetType = jobXAsset.Asset.Type;
                            if (item.TankScaleMeasurement == TankScaleMeasurement.Inches || item.TankScaleMeasurement == TankScaleMeasurement.Cm || item.TankScaleMeasurement == TankScaleMeasurement.None || item.TankScaleMeasurement == TankScaleMeasurement.Gallons || item.TankScaleMeasurement == TankScaleMeasurement.Litres)
                            {
                                assetDrop.PreDip = item.PreDip.HasValue ? item.PreDip.Value.GetPreciseValue(4) : item.PreDip;
                                assetDrop.PostDip = item.PostDip.HasValue ? item.PostDip.Value.GetPreciseValue(4) : item.PostDip;
                                assetDrop.TankScaleMeasurement = item.TankScaleMeasurement;
                            }
                            assetDrop.Gravity = item.Gravity;
                            response.Add(assetDrop);
                        }
                    }
                }
                response = response.OrderBy(t => t.Id).ToList();
                if (response != null && response.Any(t => t.AssetType == (int)AssetType.Tank))
                {
                    var jobXassetIds = response.Where(t => t.AssetType == (int)AssetType.Tank)
                                   .Select(t => t.JobXAssetId).Distinct().ToList();
                    var assetIdXJobXassetId = Context.DataContext.JobXAssets
                                            .Where(t => jobXassetIds.Contains(t.Id))
                                            .Select(t => new { t.AssetId, t.Id }).ToList();
                    if (assetIdXJobXassetId.Select(t => t.AssetId).ToList() != null && assetIdXJobXassetId.Select(t => t.AssetId).ToList().Any())
                    {
                        var assetIds = assetIdXJobXassetId.Select(t => t.AssetId).ToList();
                        List<TankDetailViewModel> tankListInfo = Task.Run(() => new FreightServiceDomain(this).GetTankList(assetIds)).Result;
                        if (tankListInfo != null && tankListInfo.Any())
                        {
                            foreach (var item in tankListInfo)
                            {
                                var jobXassetId = assetIdXJobXassetId.Where(t => t.AssetId == item.AssetId).Select(t => t.Id).FirstOrDefault();
                                var tank = response.Find(t => t.JobXAssetId == jobXassetId);
                                if (tank != null)
                                {
                                    tank.TankMakeModel = item.TankMakeModel;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetAssetDropDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        private string GetDropStatusName(int dropStatusId)
        {
            string dropStatusName = string.Empty;
            if (dropStatusId == (int)DropStatus.NoFuelNeeded)
            {
                dropStatusName = Constants.NoFuelNeeded;
            }
            else if (dropStatusId == (int)DropStatus.AssetNotAvailable)
            {
                dropStatusName = Constants.AssetNotAvailable;
            }
            return dropStatusName;
        }

        private AssetDropViewModel GetAssetDropViewModel(Invoice invoice, JobXAsset jobXAsset)
        {
            var assetDrop = new AssetDropViewModel
            {
                AssetName = jobXAsset.Asset.Name,
                JobXAssetId = jobXAsset.Id,
                OrderId = invoice.Order.Id
            };
            return assetDrop;
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
        //---------------------------------Mobile API------------------------------------------

        public async Task<StatusViewModel> CreateUnassignedInvoiceAsync(DriverDropOrderViewModel viewModel)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (!string.IsNullOrEmpty(viewModel.TraceId) && Context.DataContext.Invoices.Any(t => t.TraceId == viewModel.TraceId))
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = string.Format(Resource.valMessageAlreadyExist, Resource.lblTraceId);
                        return response;
                    }


                    if (viewModel.Image != null)
                        await viewModel.Image.UploadImageToAzureBlobService(ApplicationConstants.DropImageFileNamePrefix, BlobContainerType.InvoicePdfFiles);

                    if (viewModel.CustomerSignatureViewModel != null && viewModel.CustomerSignatureViewModel.Image != null)
                        await viewModel.CustomerSignatureViewModel.Image.UploadImageToAzureBlobService(ApplicationConstants.CustomerSignatureFileNamePrefix, BlobContainerType.InvoicePdfFiles);

                    var invoiceViewModel = viewModel.ToInvoiceViewModel();
                    invoiceViewModel.Signature = viewModel.CustomerSignatureViewModel;
                    invoiceViewModel.AdditionalDetail = viewModel.ToInvoiceXAdditionalDetailsViewModel();
                    invoiceViewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                    invoiceViewModel.StatusId = (int)InvoiceStatus.Unassigned;
                    invoiceViewModel.OrderId = null;
                    invoiceViewModel.UoM = (UoM)viewModel.UnitOfMeasurement;

                    if (viewModel.UnitOfMeasurement == (int)UoM.Gallons)
                    {
                        invoiceViewModel.Currency = Currency.USD;
                    }
                    else
                    {
                        invoiceViewModel.Currency = Currency.CAD;
                    }

                    var invoiceNumber = invoiceViewModel.InvoiceNumber.ToEntity();
                    Context.DataContext.InvoiceNumbers.Add(invoiceNumber);
                    await Context.CommitAsync();

                    // format invoice number for ddt/invoice
                    invoiceNumber.Number = invoiceNumber.Number.FormattedInvoiceNumber(invoiceViewModel.InvoiceTypeId);
                    invoiceViewModel.InvoiceNumber = invoiceNumber.ToViewModel();
                    var invoiceHeader = invoiceViewModel.ToInvoiceHeaderEntity();
                    var invoice = invoiceViewModel.ToEntity();
                    //invoice.InvoiceHeader.InvoiceNumberId = invoiceNumber.Id;
                    invoice.TransactionId = invoiceNumber.Number;
                    invoice.PaymentTermId = (int)PaymentTerms.DueOnReceipt;
                    invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.Active;
                    invoice.Version = 1;
                    Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                    invoiceHeader.Invoices.Add(invoice);

                    await Context.CommitAsync();
                    transaction.Commit();

                    invoiceViewModel.DisplayInvoiceNumber = invoiceNumber.Number;
                    invoiceViewModel.Id = invoice.Id;
                    invoiceViewModel.InvoiceHeaderId = invoice.InvoiceHeaderId;
                    NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                    await newsfeedDomain.SetUnassignedDigitalDropTicketCreatedNewsfeed(invoiceViewModel);
                    AppMessageDomain appMessageDomain = new AppMessageDomain(this);
                    await appMessageDomain.SendUnassignedDigitalDropTicketMessage(invoiceViewModel);
                    NotificationDomain notificationDomain = new NotificationDomain(this);
                    await notificationDomain.AddNotificationEventAsync(EventType.UnassignedDdtCreate, invoiceViewModel.InvoiceHeaderId, invoiceViewModel.DriverId.Value);

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "CreateUnassignedInvoiceAsync", ex.Message, ex);
                }
            }
            return response;
        }


        //public async Task<StatusViewModel> CreateMobileInvoiceAsync(DriverDropOrderViewModel viewModel)
        //{
        //    var response = new StatusViewModel();
        //    using (var tracer = new Tracer("InvoiceDomain", "CreateMobileInvoiceAsync"))
        //    {
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(viewModel.TraceId) && Context.DataContext.Invoices.Any(t => t.TraceId == viewModel.TraceId))
        //            {
        //                response.StatusCode = Status.Success; //Do not set as Failed
        //                response.StatusMessage = string.Format(Resource.valMessageAlreadyExist, Resource.lblTraceId);
        //                return response;
        //            }

        //            var firstAssetDrop = Context.DataContext.AssetDrops.Where(t => t.OrderId == viewModel.OrderId &&
        //                            t.DroppedBy == viewModel.Driver.UserId && t.InvoiceId == null).OrderBy(t => t.DropStartDate).FirstOrDefault();
        //            if (firstAssetDrop != null && viewModel.DropStartDate > firstAssetDrop.DropStartDate)
        //            {
        //                LogManager.Logger.WriteException("InvoiceDomain", "CreateMobileInvoice", $"Invoice Start:{viewModel.DropStartDate}  First Asset Drop Start:{firstAssetDrop.DropStartDate}", new InvalidOperationException("Invalid invoice drop start date"));
        //                viewModel.DropStartDate = firstAssetDrop.DropStartDate.AddSeconds(-30);
        //            }

        //            var invoiceViewModel = viewModel.ToInvoiceViewModel();
        //            invoiceViewModel.CustomerSignatureViewModel = viewModel.CustomerSignatureViewModel;
        //            invoiceViewModel.AdditionalDetail = viewModel.ToInvoiceXAdditionalDetailsViewModel();
        //            invoiceViewModel.SpecialInstructions = await GetSpecialInstructions(viewModel);
        //            invoiceViewModel.InvoiceTypeId = (int)InvoiceType.MobileApp;
        //            invoiceViewModel.TrackableScheduleId = viewModel.TrackableScheduleId;

        //            ManualInvoiceViewModel manualInvoiceModel = new ManualInvoiceViewModel();
        //            var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.OrderId);
        //            if (order != null)
        //            {
        //                var timeZoneName = order.FuelRequest.Job.TimeZoneName;
        //                invoiceViewModel.DropStartDate = viewModel.DropStartDate.ToTargetDateTimeOffset(timeZoneName);
        //                invoiceViewModel.DropEndDate = viewModel.DropEndDate.ToTargetDateTimeOffset(timeZoneName);
        //                invoiceViewModel.UoM = order.FuelRequest.UoM;
        //                invoiceViewModel.Currency = order.FuelRequest.Currency;
        //                invoiceViewModel.PoNumber = order.PoNumber;

        //                if (order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketManual)
        //                {
        //                    // as this is mobile invoice and ddt , set it as ddtmobile so in future also we can retrieve mobile invoices
        //                    invoiceViewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
        //                }
        //                manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = GetFuelRequestFee(order.FuelRequest);

        //                invoiceViewModel.IsWetHosingDelivery = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.WetHoseFee).ToString() && t.DiscountLineItemId == null);
        //                invoiceViewModel.IsOverWaterDelivery = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.OverWaterFee).ToString() && t.DiscountLineItemId == null);

        //                manualInvoiceModel.PaymentTermId = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentTermId;
        //                manualInvoiceModel.NetDays = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).NetDays;
        //                manualInvoiceModel.TypeofFuel = order.FuelRequest.MstProduct.ProductDisplayGroupId;
        //                manualInvoiceModel.TimeZoneName = timeZoneName;
        //                manualInvoiceModel.TerminalId = order.TerminalId ?? 0;
        //                manualInvoiceModel.CityGroupTerminalId = order.CityGroupTerminalId ?? 0;
        //            }

        //            //If InvoiceStatus is Draft creat only DDT
        //            if (viewModel.InvoiceStatusId == (int)InvoiceStatus.Draft)
        //            {
        //                invoiceViewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
        //            }
        //            if (order.ExternalBrokerBuySellDetail != null)
        //            {
        //                response = await GenerateInvoiceForSellPrice(invoiceViewModel, manualInvoiceModel, true);
        //                await SetNewsfeedForManualCreateInvoiceMobileAsync(order, invoiceViewModel);
        //            }
        //            else
        //            {
        //                response = await GenerateInvoiceForMobileAsync(invoiceViewModel, manualInvoiceModel, InvoiceCreationFrom.Mobile);
        //            }

        //            if (response.StatusCode == Status.Success)
        //            {
        //                viewModel.InvoiceId = invoiceViewModel.Id;
        //                invoiceViewModel.UserId = viewModel.Driver.UserId;
        //                response = await UpdateMobileFuelSpills(viewModel.OrderId, invoiceViewModel.Id);
        //                await DriverCompletedOrder(viewModel.Driver.UserId, viewModel.Driver.FCMAppId, viewModel.OrderId, viewModel.TrackableScheduleId);

        //                var newsfeedDomain = new NewsfeedDomain(this);
        //                var appMessageDomain = new AppMessageDomain(this);
        //                if (viewModel.InvoiceStatusId == (int)InvoiceStatus.Draft)
        //                {
        //                    await newsfeedDomain.SetSystemDigitalDropTicketDraftCreatedNewsfeed(order, invoiceViewModel);
        //                    await appMessageDomain.SendDraftDDTMessage(order, invoiceViewModel);
        //                }
        //                else if (invoiceViewModel.WaitingForAction == (int)WaitingAction.CustomerApproval)
        //                {
        //                    await newsfeedDomain.SetDriverDroppedInvoiceCreatedWaitingForNewsfeed(order, invoiceViewModel, NewsfeedEvent.DriverDropsWaitingForApproval);
        //                }
        //                else if (invoiceViewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice)
        //                {
        //                    await newsfeedDomain.SetDriverDroppedInvoiceCreatedWaitingForNewsfeed(order, invoiceViewModel, NewsfeedEvent.DriverDropsWaitingForUpdatedPrice);
        //                }
        //                else if (invoiceViewModel.WaitingForAction == (int)WaitingAction.AvalaraTax)
        //                {
        //                    await newsfeedDomain.SetDriverDroppedInvoiceCreatedWaitingForNewsfeed(order, invoiceViewModel, NewsfeedEvent.DriverDropsWaitingForTaxes);
        //                }
        //                else
        //                {
        //                    await newsfeedDomain.SetSystemInvoiceCreatedNewsfeed(order, invoiceViewModel);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogManager.Logger.WriteException("InvoiceDomain", "CreateMobileInvoice", ex.Message, ex);
        //        }
        //    }
        //    return response;
        //}

        //public async Task<StatusViewModel> DriverCompletedOrder(int driverId, string fcmId, int orderId, int? trackableScheduleId)
        //{
        //    var response = new StatusViewModel();
        //    using (var transaction = Context.DataContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var appLocation = Context.DataContext.AppLocations.FirstOrDefault(t => t.UserId == driverId && t.FCMAppId == fcmId);
        //            if (appLocation != null)
        //            {
        //                appLocation.OrderId = null;
        //                appLocation.DeliveryScheduleId = null;
        //                appLocation.TrackableScheduleId = null;
        //                appLocation.StatusId = (int)EnrouteDeliveryStatus.CompletedDrop;

        //                Context.DataContext.Entry(appLocation).State = EntityState.Modified;
        //                await Context.CommitAsync();
        //            }

        //            var enrouteViewModel = new EnrouteDeliveryViewModel();
        //            enrouteViewModel.UserId = driverId;
        //            enrouteViewModel.OrderId = orderId;

        //            if (trackableScheduleId.HasValue && trackableScheduleId.Value > 0)
        //            {
        //                var trackableschedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.FirstOrDefault(t => t.Id == trackableScheduleId.Value);
        //                if (trackableschedule != null)
        //                    enrouteViewModel.DeliveryScheduleId = trackableschedule.DeliveryScheduleId;
        //                enrouteViewModel.TrackableScheduleId = trackableScheduleId.Value;
        //            }

        //            enrouteViewModel.StatusId = (int)EnrouteDeliveryStatus.CompletedDrop;

        //            var enrouteDeliveryHistory = enrouteViewModel.ToEntity();
        //            Context.DataContext.EnrouteDeliveryHistories.Add(enrouteDeliveryHistory);

        //            await Context.CommitAsync();
        //            transaction.Commit();

        //            response.StatusCode = Status.Success;
        //            response.StatusMessage = Resource.errMessageSuccess;
        //        }
        //        catch (Exception ex)
        //        {
        //            LogManager.Logger.WriteException("InvoiceDomain", "DriverCompletedOrder", ex.Message, ex);
        //        }
        //    }

        //    return response;
        //}

        public async Task<AssignToOrderViewModel> AssignInvoiceToOrderAsync(int orderId, int invoiceId, int userId)
        {
            var response = new AssignToOrderViewModel();
            try
            {
                var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == orderId);
                if (order != null)
                {
                    var invoice = Context.DataContext.Invoices.SingleOrDefault(t => t.Id == invoiceId);
                    if (invoice != null)
                    {
                        //createmanual invoice
                        var invoiceModel = invoice.ToViewModel();
                        if (order.FuelRequest.UoM != invoice.UoM)
                        {
                            invoiceModel.DroppedGallons = Quantity.Convert(invoice.UoM, order.FuelRequest.UoM, invoice.DroppedGallons);
                            invoiceModel.UoM = order.FuelRequest.UoM;
                            invoiceModel.Currency = order.FuelRequest.Currency;
                        }
                        invoiceModel.OrderId = orderId;
                        invoiceModel.PoNumber = order.PoNumber;
                        invoiceModel.UpdatedBy = userId;
                        invoiceModel.PreviousStatusId = invoiceModel.StatusId;
                        invoiceModel.StatusId = (int)InvoiceStatus.Received;
                        invoiceModel.DriverId = invoice.DriverId;
                        invoiceModel.AdditionalDetail = null;
                        if (order.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.Job.IsApprovalWorkflowEnabled)
                        {
                            invoiceModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                        }
                        if (!order.FuelRequest.Job.IsApprovalWorkflowEnabled && order.DefaultInvoiceType == (int)InvoiceType.Manual)
                        {
                            invoiceModel.InvoiceTypeId = (int)InvoiceType.MobileApp;
                        }
                        invoiceModel.UserId = userId;

                        ManualInvoiceViewModel manualInvoiceModel = new ManualInvoiceViewModel();
                        manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = GetFuelRequestFee(order.FuelRequest);
                        manualInvoiceModel.InvoiceId = invoiceId;
                        manualInvoiceModel.SignatureId = invoice.SignatureId;

                        invoiceModel.BolDetails = SetInvoiceFtlDetails(order, userId);

                        StatusViewModel status = await GenerateInvoiceAnotherCustomerWorkflow(invoiceModel, manualInvoiceModel, true, invoiceId);

                        if (status.StatusCode == Status.Failed)
                        {
                            invoice.OrderId = null;
                            await Context.CommitAsync();
                        }
                        else
                        {
                            invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;
                            await Context.CommitAsync();
                            response.InvoiceId = invoiceModel.Id;

                            if (order.DefaultInvoiceType == (int)InvoiceType.Manual && !order.FuelRequest.Job.IsApprovalWorkflowEnabled)
                            {
                                invoiceModel.InvoiceNumber.Number = invoiceModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFDD, ApplicationConstants.SFIN);
                            }

                            var newsfeedDomain = new NewsfeedDomain(this);
                            if (!order.FuelRequest.Job.JobBudget.IsAssetTracked)
                            {
                                await UpdateAssetLevelTrackingInJob(order.FuelRequest.Job.Id);
                                await newsfeedDomain.SetAssetLevelTrackingEnableFromAssignDDTToOrderNewsfeed(order, invoiceModel, userId);
                            }
                            if (order.FuelRequest.Job.IsApprovalWorkflowEnabled)
                            {
                                await newsfeedDomain.SetAssignDDTToOrderWhenApprovalWFEnabledNewsfeed(order, invoiceModel, userId);
                            }
                            else if (order.DefaultInvoiceType == (int)InvoiceType.Manual && invoiceModel.WaitingForAction == (int)WaitingAction.UpdatedPrice)
                            {
                                await newsfeedDomain.SetAssignDDTToOrderWaitingForPriceSettingInvoiceNewsfeed(order, invoiceModel, userId);
                            }
                            else if (order.DefaultInvoiceType == (int)InvoiceType.Manual && invoiceModel.WaitingForAction == (int)WaitingAction.AvalaraTax)
                            {
                                await newsfeedDomain.SetAnotherCustomerWaitingForTaxesNewsfeed(order, invoiceModel, userId);
                            }
                            else
                            {
                                await newsfeedDomain.SetAssignDigitalDropTicketToOrderNewsfeed(order, invoiceModel, userId);
                            }

                            var currentUser = Context.DataContext.Users.First(t => t.Id == userId);
                            var driver = Context.DataContext.Users.First(t => t.Id == invoiceModel.DriverId);
                            var notificationDomain = new NotificationDomain(this);
                            var appMessageDomain = new AppMessageDomain(this);
                            if (invoiceModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                            {
                                if (order.DefaultInvoiceType != (int)InvoiceType.Manual && invoiceModel.WaitingForAction != (int)WaitingAction.UpdatedPrice && invoiceModel.WaitingForAction != (int)WaitingAction.AvalaraTax && !order.FuelRequest.Job.IsApprovalWorkflowEnabled)
                                {
                                    //// #6328 - buyer set unassigned DDT to order linked newsfeed
                                    await newsfeedDomain.SetUnassignedDDTToOrderLinkNewsfeed(order, driver, invoiceModel, userId);

                                    //// #6328 - supplier newsfeed after linking on DDT
                                    await newsfeedDomain.SetUnassignedDDTToOrderLinkSupplierNewsfeed(order, driver, invoiceModel, currentUser);
                                }

                                if (invoiceModel.IsTaxServiceFailure)
                                {
                                    await notificationDomain.AddNotificationEventAsync(EventType.DDTCreateAsInvoiceWaitingForTaxes, invoiceModel.InvoiceHeaderId, userId);
                                }
                                else
                                {
                                    //// #6328 -buyer and supplier email 
                                    await notificationDomain.AddNotificationEventAsync(EventType.LinkUnassignedDdtToOrder, invoiceModel.InvoiceHeaderId, userId);
                                }

                                //// #6328 -buyer message
                                await appMessageDomain.SendUnassignedDdtToOrderLinkedMessage(invoiceModel, order, CompanyFilterType.Buyer, userId);

                                //// #6328 -supplier message
                                await appMessageDomain.SendUnassignedDdtToOrderLinkedMessage(invoiceModel, order, CompanyFilterType.Supplier, userId);
                            }
                            else if (invoiceModel.InvoiceTypeId == (int)InvoiceType.MobileApp)
                            {
                                //// #6328 - buyer set unassigned DDT to order invoice generate newsfeed
                                await newsfeedDomain.SetUnassignedDDTToOrderInvoiceGenerateBuyerNewsfeed(order, driver, invoiceModel, userId);

                                //// #6328 - supplier set unassigned DDT to order invoice generate newsfeed
                                await newsfeedDomain.SetInvoiceGenerateOnLinkDDTToOrderSupplierNewsfeed(order, invoiceModel, currentUser);

                                //// #6328 - buyer message
                                await appMessageDomain.SendUnassignedDdtToOrderLinkedInvoiceGenerateMessage(invoiceModel, order, CompanyFilterType.Buyer, userId);

                                //// #6328 - supplier message
                                await appMessageDomain.SendUnassignedDdtToOrderLinkedInvoiceGenerateMessage(invoiceModel, order, CompanyFilterType.Supplier, userId);

                                //// #6328 -buyer and supplier email 
                                await notificationDomain.AddNotificationEventAsync(EventType.InvoiceCreatedViaMobileDrop, invoiceModel.InvoiceHeaderId, userId);
                            }
                        }

                        response.StatusCode = status.StatusCode;
                        response.StatusMessage = status.StatusMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "AssignInvoiceToOrderAsync", ex.Message, ex);
            }
            return response;
        }

        private BolDetailViewModel SetInvoiceFtlDetails(Order order, int userId)
        {
            var bolDetail = new BolDetailViewModel()
            {
                Carrier = order.OrderAdditionalDetail?.Carrier?.Name,
                CreatedBy = userId,
                CreatedDate = DateTimeOffset.Now
                //LiftDate = DateTimeOffset.Now
            };

            //if (viewModel.IsBulkPlant)
            //{
            //    bolDetail.LiftTicketNumber = viewModel.BolNumber;
            //    bolDetail.LiftQuantity = bolDetails.NetQuantity;
            //}
            //else
            //{
            //    bolDetail.BolNumber = viewModel.BolNumber;
            //    bolDetail.NetQuantity = bolDetails.NetQuantity;
            //    bolDetail.GrossQuantity = bolDetails.GrossQuantity;
            //}
            var pickUpAddress = order.FuelDispatchLocations.FirstOrDefault(t => !t.TrackableScheduleId.HasValue && t.IsActive);
            bolDetail.IsDeleted = false;
            bolDetail.IsActive = true;
            bolDetail.FuelTypeId = order.FuelRequest.FuelTypeId;
            bolDetail.CityGroupTerminalId = order.CityGroupTerminalId;
            bolDetail.TerminalId = order.TerminalId;
            bolDetail.TerminalName = order.MstExternalTerminal?.Name;
            if (pickUpAddress != null)
            {
                bolDetail.Address = pickUpAddress.Address;
                bolDetail.City = pickUpAddress.City;
                bolDetail.CountryCode = pickUpAddress.CountryCode;
                bolDetail.CountyName = pickUpAddress.CountyName;
                bolDetail.Latitude = pickUpAddress.Latitude;
                bolDetail.Longitude = pickUpAddress.Longitude;
                bolDetail.SiteName = pickUpAddress.SiteName;
                bolDetail.StateCode = pickUpAddress.StateCode;
                bolDetail.StateId = pickUpAddress.StateId ?? 0;
                bolDetail.ZipCode = pickUpAddress.ZipCode;
            }
            else if (order.MstExternalTerminal != null)
            {
                var terminal = order.MstExternalTerminal;
                bolDetail.Address = terminal.Address;
                bolDetail.City = terminal.City;
                bolDetail.CountryCode = terminal.CountryCode;
                bolDetail.CountyName = terminal.CountyName;
                bolDetail.Latitude = terminal.Latitude;
                bolDetail.Longitude = terminal.Longitude;
                bolDetail.StateCode = terminal.StateCode;
                bolDetail.StateId = terminal.StateId;
                bolDetail.ZipCode = terminal.ZipCode;
            }
            return bolDetail;
        }

        public async Task<StatusViewModel> UpdateDDTConversionReason(int invoiceId, int reasonId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var invoice = await Context.DataContext.Invoices.SingleOrDefaultAsync(t => t.Id == invoiceId);
                    if (invoice != null)
                    {
                        invoice.DDTConversionReason = reasonId;

                        Context.DataContext.Entry(invoice).State = EntityState.Modified;
                        await Context.CommitAsync();

                        response.StatusCode = Status.Success;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "UpdateDDTConversionReason", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateAssetLevelTrackingInJob(int jobId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var job = await Context.DataContext.Jobs.SingleOrDefaultAsync(t => t.Id == jobId);
                    if (job != null)
                    {
                        job.JobBudget.IsAssetTracked = true;

                        Context.DataContext.Entry(job).State = EntityState.Modified;
                        await Context.CommitAsync();

                        response.StatusCode = Status.Success;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "UpdateAssetLevelTrackingInJob", ex.Message, ex);
                }
            }

            return response;
        }

        public InvoiceFilterViewModel GetInvoiceFilter(int jobId, InvoiceFilterType filter, string groupIds = "")
        {
            var response = new InvoiceFilterViewModel();
            try
            {
                response.JobId = jobId;
                response.GroupIds = groupIds;
                if (filter > 0)
                {
                    response.Filter = filter;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetFuelRequestFilterAsync", ex.Message, ex);
            }
            return response;
        }

        public InvoiceFilterViewModel GetDigitalDropTicketFilter(int jobId, InvoiceFilterType filter, string groupIds = "")
        {
            var response = new InvoiceFilterViewModel();
            try
            {
                response.JobId = jobId;
                response.AllowedInvoiceType = (int)InvoiceType.DigitalDropTicketManual;
                response.GroupIds = groupIds;
                if (filter > 0)
                {
                    response.Filter = filter;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetDigitalDropTicketFilter", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteInvoice(int invoiceId, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var data = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => new
                    {
                        Invoice = t,
                        InvoicePreviousStatus = t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive),
                        Order = t.Order,
                        FuelRequest = t.Order.FuelRequest,
                        Job = t.Order.FuelRequest.Job,
                        InvoiceTypeId = t.InvoiceTypeId
                    }).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        data.InvoicePreviousStatus.IsActive = false;

                        InvoiceXInvoiceStatusDetail invoiceStatus = new InvoiceXInvoiceStatusDetail();
                        invoiceStatus.StatusId = (int)InvoiceStatus.Deleted;
                        invoiceStatus.IsActive = false;
                        invoiceStatus.InvoiceId = data.Invoice.Id;
                        invoiceStatus.UpdatedBy = userContext.Id;
                        invoiceStatus.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.InvoiceXInvoiceStatusDetails.Add(invoiceStatus);

                        data.Invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;
                        data.Invoice.IsActive = false;

                        if (data.Order != null)
                        {
                            UpdateHedgeAndSpotData(data.Invoice, data.Order.BuyerCompanyId, data.FuelRequest, data.Job);
                        }

                        if (data.Invoice.ParentId != null)
                        {
                            data.Invoice.Invoice1.IsActive = true;
                        }

                        Context.DataContext.Entry(data.Invoice).State = EntityState.Modified;

                        var newsfeeds = Context.DataContext.Newsfeeds.Where(t => t.TargetEntityId == invoiceId);
                        newsfeeds.ToList().ForEach(t => t.IsActive = false);
                        await new QbDomain(this).MarkQbRequestsToDeleted(data.Invoice.InvoiceHeader.InvoiceNumberId);

                        await Context.CommitAsync();
                        transaction.Commit();
                        var cumulationUpdateList = await CreateListOfCumulationEntitiesToUpdateForDeleteInv(data.Invoice);
                        if (cumulationUpdateList != null && cumulationUpdateList.Any())
                        {
                            await new ConsolidatedInvoiceDomain(this).UpdateCumulationQuantitiesPostInvoiceCreate(cumulationUpdateList);
                        }
                        response.StatusCode = Status.Success;
                        if (data.Invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || data.Invoice.InvoiceTypeId == (int)(int)InvoiceType.DigitalDropTicketMobileApp)
                            response.StatusMessage = string.Format(Resource.errMessageDeletedSuccess, Resource.lblDDT);
                        else
                            response.StatusMessage = string.Format(Resource.errMessageDeletedSuccess, Resource.lblInvoice);
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageFailed;
                    LogManager.Logger.WriteException("InvoiceDomain", "DeleteInvoice", ex.Message, ex);
                }
            }

            return response;
        }

        private IQueryable<Invoice> ApplyInvoiceFilter(InvoiceFilterViewModel invoiceFilter, IQueryable<Invoice> allInvoices)
        {
            if (invoiceFilter != null)
            {
                if (invoiceFilter.JobId > 0)
                {
                    allInvoices = allInvoices.Where(t => t.Order.FuelRequest.Job.Id == invoiceFilter.JobId);
                }
                else
                {
                    DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                    DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);

                    if (!string.IsNullOrEmpty(invoiceFilter.StartDate))
                    {
                        startDate = Convert.ToDateTime(invoiceFilter.StartDate).Date;
                    }
                    if (!string.IsNullOrEmpty(invoiceFilter.EndDate))
                    {
                        endDate = Convert.ToDateTime(invoiceFilter.EndDate).Date.AddDays(1);
                    }
                    allInvoices = allInvoices.Where(t => t.CreatedDate >= startDate && t.CreatedDate < endDate);
                }

                if (invoiceFilter.Filter == InvoiceFilterType.Approved)
                {
                    allInvoices = allInvoices.Where(t => t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.Approved);
                }
                else if (invoiceFilter.Filter == InvoiceFilterType.Rejected)
                {
                    allInvoices = allInvoices.Where(t => t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.Rejected && t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.Received));
                }
                else if (invoiceFilter.Filter == InvoiceFilterType.Received)
                {
                    allInvoices = allInvoices.Where(t => t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.Received);
                }
                else if (invoiceFilter.Filter == InvoiceFilterType.Confirmed)
                {
                    allInvoices = allInvoices.Where(t => t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.Confirmed);
                }
                else if (invoiceFilter.Filter == InvoiceFilterType.Paid)
                {
                    allInvoices = allInvoices.Where(t => t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.Paid);
                }
                else if (invoiceFilter.Filter == InvoiceFilterType.Unassigned)
                {
                    allInvoices = allInvoices.Where(t => t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.Unassigned);
                }
                else if (invoiceFilter.Filter == InvoiceFilterType.Unconfirmed)
                {
                    allInvoices = allInvoices.Where(t => t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.Unconfirmed);
                }
                else if (invoiceFilter.Filter == InvoiceFilterType.InvoiceWaitingForApprovalRejected || invoiceFilter.Filter == InvoiceFilterType.DropTicketWaitingForApprovalRejected)
                {
                    allInvoices = allInvoices.Where(t => t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.Rejected) && !t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.Received));
                }
                else if (invoiceFilter.Filter == InvoiceFilterType.InvoiceWaitingApproval || invoiceFilter.Filter == InvoiceFilterType.DropTicketWaitingApproval)
                {
                    allInvoices = allInvoices.Where(t => t.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)InvoiceStatus.WaitingForApproval);
                }
                else if (invoiceFilter.Filter == InvoiceFilterType.InvoicesFromDropTicket)
                {
                    allInvoices = allInvoices.Where(t => t.ParentId != null);
                }
            }

            return allInvoices;
        }

        //private async Task<StatusViewModel> UpdateMobileFuelSpills(int orderId, int invoiceId)
        //{
        //    var response = new StatusViewModel();
        //    var spills = Context.DataContext.Spills.Where(t => t.OrderId == orderId && t.InvoiceId == null);
        //    using (var transaction = Context.DataContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            foreach (var spill in spills)
        //            {
        //                spill.InvoiceId = invoiceId;
        //                Context.DataContext.Entry(spill).State = EntityState.Modified;
        //            }
        //            await Context.CommitAsync();
        //            transaction.Commit();

        //            response.StatusCode = Status.Success;
        //            response.StatusMessage = Resource.errMessageSuccess;
        //        }
        //        catch (Exception ex)
        //        {
        //            LogManager.Logger.WriteException("InvoiceDomain", "UpdateMobileFuelSpills", ex.Message, ex);
        //        }
        //    }

        //    return response;
        //}

        //private async Task<List<InvoiceXSpecialInstructionViewModel>> GetSpecialInstructions(DriverDropOrderViewModel viewModel)
        //{
        //    var specialInstructions = new List<InvoiceXSpecialInstructionViewModel>();
        //    if (viewModel.SpecialInstructions != null && viewModel.SpecialInstructions.Count > 0)
        //    {
        //        var fuelRequest = await Context.DataContext.FuelRequests.SingleOrDefaultAsync(t => t.Id == viewModel.FuelId);
        //        if (fuelRequest.SpecialInstructions.Count > 0 && fuelRequest.SpecialInstructions.Count == viewModel.SpecialInstructions.Count)
        //        {
        //            foreach (var item in viewModel.SpecialInstructions)
        //            {
        //                var FrInstruction = fuelRequest.SpecialInstructions.FirstOrDefault(t => t.Instruction == item.Key);
        //                if (FrInstruction != null)
        //                {
        //                    var instruction = new InvoiceXSpecialInstructionViewModel
        //                    {
        //                        SpecialInstructionId = FrInstruction.Id,
        //                        IsInstructionFollowed = item.Value
        //                    };
        //                    specialInstructions.Add(instruction);
        //                }
        //            }
        //        }
        //    }

        //    return specialInstructions;
        //}

        private InvoiceTaxDetailsViewModel GetTaxDetailsForInternalTaxValues(ManualInvoiceViewModel viewModel, int tranId)
        {
            var taxDetailsViewModel = new InvoiceTaxDetailsViewModel()
            {
                TranId = tranId,
                ReturnCode = tranId
            };

            taxDetailsViewModel.AvaTaxDetails = new List<TaxDetailsViewModel>();

            foreach (var item in viewModel.TaxDetails.AvaTaxDetails)
            {
                //tax details from edit.
                taxDetailsViewModel.AvaTaxDetails.Add(new TaxDetailsViewModel()
                {
                    CalculationTypeInd = ApplicationConstants.CalculationType,
                    Currency = item.Currency,
                    ProductCategory = 1,
                    RateDescription = item.RateDescription,
                    RateSubtype = item.RateSubtype,
                    RateType = ApplicationConstants.ExternalTaxRateTypeTAX,
                    SalesTaxBaseAmount = item.SalesTaxBaseAmount,
                    TaxAmount = item.TaxAmount,
                    TaxExemptionInd = ApplicationConstants.TaxExemptionInd,
                    TaxRate = 0,
                    TaxType = item.TaxType,
                    TaxingLevel = item.TaxingLevel,
                    UnitOfMeasure = ApplicationConstants.UnitOfMeasure,
                    TradingTaxAmount = item.TradingTaxAmount,
                    TradingCurrency = item.TradingCurrency,
                    ExchangeRate = item.ExchangeRate,
                    IsModified = item.IsModified
                });

                taxDetailsViewModel.TotalTaxAmount += item.TradingTaxAmount;
            }

            return taxDetailsViewModel;
        }

        private InvoiceTaxDetailsViewModel GetTaxDetailsFromManualTaxInputs(ManualInvoiceViewModel viewModel, Order order, int tranId, decimal basicAmount, decimal gallons)
        {
            List<TaxViewModel> applicableTaxes = order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketManual &&
                                                 viewModel.TypeofFuel == (int)ProductDisplayGroups.OtherFuelType
                                                    ? order.OrderTaxDetails.Where(x => x.IsActive).ToTaxViewModel() : viewModel.Taxes;

            var invoiceCommonDomain = new InvoiceCommonDomain(this);
            var taxDetailsViewModel = invoiceCommonDomain.GetTaxDetailsFromInputs(applicableTaxes, order.FuelRequest.Currency, tranId, basicAmount, gallons);

            return taxDetailsViewModel;
        }

        public async Task<List<DropdownDisplayItem>> GetInvoiceNumbersAsync(int companyId, int allowedInvoiceType = (int)InvoiceType.Manual)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (allowedInvoiceType == (int)InvoiceType.DigitalDropTicketManual)
                {
                    var invoices = await Context.DataContext.Invoices.Where(t => t.OrderId != null &&
                                                                    (t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual ||
                                                                    t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) &&
                                                                    t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t.IsBuyPriceInvoice &&
                                                                    (t.Order.AcceptedCompanyId == companyId || t.Order.BuyerCompanyId == companyId)
                                                                   ).ToListAsync();
                    invoices.ForEach(t => response.Add(new DropdownDisplayItem
                    {
                        Id = t.Id,
                        Name = t.DisplayInvoiceNumber
                    }));
                }
                else
                {
                    // if not DDT, then load all other invoices types
                    var invoices = await Context.DataContext.Invoices.Where(t => t.IsActive && t.OrderId != null &&
                                                                     t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual &&
                                                                     t.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp &&
                                                                     t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t.IsBuyPriceInvoice &&
                                                                     (t.Order.AcceptedCompanyId == companyId || t.Order.BuyerCompanyId == companyId)
                                                                    ).ToListAsync();

                    invoices.ForEach(t => response.Add(new DropdownDisplayItem
                    {
                        Id = t.Id,
                        Name = t.DisplayInvoiceNumber
                    }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceNumbersAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<int> GetDriverByDeliveryScheduleAsync(int trackableScheduleId)
        {
            int result = 0;
            try
            {
                result = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.Id == trackableScheduleId)
                                                                                   .Select(t => t.DeliverySchedule.DeliveryScheduleXDrivers.Where(t1 => t1.IsActive)
                                                                                                                                            .Select(t1 => t1.Id).FirstOrDefault())
                                                                                   .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HelperDomain", "GetDriverByDeliveryScheduleAsync", ex.Message, ex);
            }
            return result;
        }

        public async Task<StatusViewModel> SaveExternalDrop(ExternalDropDetailViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            DeliveryReqStatusUpdateModel drStatus = null;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var externalDrop = new ExternalDropDetail();
                    externalDrop = viewModel.ToExternalDropDetailEntity(externalDrop);
                    Context.DataContext.ExternalDropDetails.Add(externalDrop);

                    var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == viewModel.OrderId);
                    if (order != null)
                    {
                        if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
                        {
                            order.OrderXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                            OrderXStatus orderStatus = new OrderXStatus();
                            orderStatus.StatusId = (int)OrderStatus.Closed;
                            orderStatus.IsActive = true;
                            orderStatus.UpdatedBy = viewModel.UserId;
                            orderStatus.UpdatedDate = DateTimeOffset.Now;
                            order.OrderXStatuses.Add(orderStatus);
                            order.DeliveryScheduleXTrackableSchedules.Where(t => t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled &&
                                                                       t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted &&
                                                                       t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted &&
                                                                       t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.MissedAndCanceled &&
                                                                       t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed &&
                                                                       t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate &&
                                                                       t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted &&
                                                                       t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate).ToList().ForEach(t => t.DeliveryScheduleStatusId = (int)DeliveryScheduleStatus.Canceled);

                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }
                        else
                        {
                            if (viewModel.TrackableScheduleId != null)
                            {
                                var schedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.FirstOrDefault(t => t.IsActive && t.Id == viewModel.TrackableScheduleId);
                                if (schedule != null)
                                {
                                    schedule.DeliveryScheduleStatusId = GetDeliveryScheduleStatus(DateTimeOffset.FromUnixTimeMilliseconds(viewModel.DropEndDate), schedule);
                                    if (!string.IsNullOrWhiteSpace(schedule.FrDeliveryRequestId))
                                    {
                                        drStatus = new DeliveryReqStatusUpdateModel() { DeliveryRequestId = schedule.FrDeliveryRequestId, ScheduleStatusId = schedule.DeliveryScheduleStatusId, UserId = viewModel.UserId };
                                    }
                                }
                            }
                        }
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                    if (drStatus != null)
                    {
                        new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { drStatus });
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "SaveExternalDrop", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<ManualInvoiceViewModel> GetAssetsForInvoice(ManualInvoiceViewModel viewModel)
        {
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.OrderId && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open);
                if (order != null)
                {
                    viewModel.Assets.ForEach(t => t.IsDropMade = true);
                    var existingAssets = viewModel.Assets.Select(t => t.JobXAssetId);
                    order.FuelRequest.Job
                                   .JobXAssets.Where(t => !existingAssets.Contains(t.Id)).OrderByDescending(t => t.Id).GroupBy(t => new { t.Asset.Id })
                                   .Select(g => g.First())
                                   .ToList().ForEach(t => viewModel.Assets.Add(new AssetDropViewModel(Status.Success)
                                   {
                                       AssetName = t.Asset.Name,
                                       JobXAssetId = t.Id,
                                       OrderId = viewModel.OrderId
                                   }));
                    viewModel.Assets = viewModel.Assets.OrderByDescending(t => t.JobXAssetId).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetAssetsForInvoice", ex.Message, ex);
            }
            return viewModel;
        }

        private int GetDeliveryScheduleStatus(DateTimeOffset DropEndDate, DeliveryScheduleXTrackableSchedule deliveryScheduleXTrackableSchedule)
        {
            int deliveryStatusId = (int)TrackableDeliveryScheduleStatus.Accepted;
            var statusId = deliveryScheduleXTrackableSchedule.DeliverySchedule.StatusId;

            if (DropEndDate.Date <= deliveryScheduleXTrackableSchedule.Date.Date)
            {
                if (DropEndDate.Hour <= deliveryScheduleXTrackableSchedule.EndTime.Hours)
                {
                    return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledCompleted : (int)TrackableDeliveryScheduleStatus.Completed;
                }
                else
                {
                    return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledLate : (int)TrackableDeliveryScheduleStatus.CompletedLate;
                }
            }
            else if (DropEndDate.Date > deliveryScheduleXTrackableSchedule.Date.Date)
            {
                return statusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledLate : (int)TrackableDeliveryScheduleStatus.CompletedLate;
            }

            return deliveryStatusId;
        }

        public async Task<InvoiceGridViewModel> GetDeleteRequestsInvoiceGridAsync(string invoiceNumber)
        {
            InvoiceGridViewModel response = new InvoiceGridViewModel();

            try
            {
                var invoice = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetDeleteRequestsInvoicesAsync(invoiceNumber);
                if (invoice != null)
                {
                    var fuelType = invoice.InvoiceTypeId == (int)InvoiceType.DryRun ? Resource.lblDryRunFee : invoice.FuelType;
                    response = new InvoiceGridViewModel(Status.Success)
                    {
                        Id = invoice.Id.ToString(),
                        OrderId = invoice.OrderId.ToString(),
                        Supplier = invoice.OrderId == 0 ? Resource.lblHyphen : invoice.Supplier,
                        FuelType = invoice.OrderId == 0 ? Resource.lblHyphen : fuelType,
                        InvoiceNumber = invoice.InvoiceNumber,
                        PoNumber = invoice.OrderId == 0 ? Resource.lblHyphen : invoice.PoNumber,
                        InvoiceAmount = invoice.OrderId == 0 ? 0 : invoice.InvoiceAmount.GetPreciseValue(6),
                        DropDate = invoice.DropDate.ToString(Resource.constFormatDate),
                        DropTime = $"{invoice.DropStartDate.DateTime.ToShortTimeString()}{Resource.lblSingleHyphen}{invoice.DropDate.DateTime.ToShortTimeString()}",
                        InvoiceDate = invoice.InvoiceDate.ToString(Resource.constFormatDate),
                        PaymentDueDate = invoice.PaymentDueDate.ToString(Resource.constFormatDate),
                        Status = invoice.Status,
                        InvoiceNumberId = invoice.InvoiceNumberId,
                        TerminalName = invoice.TerminalName,
                        DriverName = string.IsNullOrWhiteSpace(invoice.DriverFName) ? Resource.lblHyphen : $"{invoice.DriverFName} {invoice.DriverLName.First()}"
                    };

                    response = response.CorrectValues();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetSupplierInvoicesAsync", ex.Message, ex);
            }

            return response;
        }

        private void SetTaxAmountForStandardProduct(InvoiceViewModel viewModel, ManualInvoiceViewModel manualInvoiceModel, Order order, bool isDryRunOrAssignToOrder, int originalInvoiceTypeId)
        {
            if (!manualInvoiceModel.IsInvoiceEdit || isDryRunOrAssignToOrder || manualInvoiceModel.TaxType == TaxType.Standard)
            {
                if (!viewModel.IsRecursiveCallForBrokerOrders)
                {
                    try
                    {
                        var avaTaxes = GetInvoiceTaxes(order, viewModel);
                        viewModel.TotalTaxAmount = avaTaxes.TotalTaxAmount;
                        viewModel.TransactionId = avaTaxes.TranId.ToString();
                        viewModel.TaxDetails = avaTaxes;
                        if (avaTaxes.FailedStatusCode == (int)DDTConversionReason.AvalaraProductNotMapped)
                        {
                            viewModel.IsTaxServiceFailure = true;
                            manualInvoiceModel.IsTaxServiceFailure = true;
                            viewModel.DDTConversionReason = avaTaxes.FailedStatusCode;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (manualInvoiceModel.TaxType == TaxType.Standard)
                        {
                            viewModel.IsTaxServiceFailure = true;
                            manualInvoiceModel.IsTaxServiceFailure = true;
                        }
                        else
                        {
                            SetTaxServiceFailureFlag(viewModel, manualInvoiceModel, originalInvoiceTypeId);
                        }
                        LogManager.Logger.WriteException("InvoiceDomain", "SetTaxAmountForStandardProduct -InvoiceNumber" + viewModel.InvoiceNumber.Number, ex.Message, ex);
                    }
                }
                else if (viewModel.IsTaxServiceFailure && manualInvoiceModel.TaxType != TaxType.Standard)
                {
                    SetTaxServiceFailureFlag(viewModel, manualInvoiceModel, originalInvoiceTypeId);
                }
            }
            else
            {
                if (manualInvoiceModel.TaxDetails != null)
                {
                    var taxFromManualEntry = GetTaxDetailsForInternalTaxValues(manualInvoiceModel, viewModel.Id);
                    viewModel.TotalTaxAmount = taxFromManualEntry.TotalTaxAmount;
                    viewModel.TransactionId = taxFromManualEntry.TranId.ToString();
                    viewModel.TaxDetails = taxFromManualEntry;
                }
                else if (manualInvoiceModel.Taxes != null)
                {
                    var taxFromManualEntry = GetTaxDetailsFromManualTaxInputs(manualInvoiceModel, order, viewModel.Id, viewModel.BasicAmount, viewModel.DroppedGallons);
                    viewModel.TotalTaxAmount = taxFromManualEntry.TotalTaxAmount;
                    viewModel.TransactionId = taxFromManualEntry.TranId.ToString();
                    viewModel.TaxDetails = taxFromManualEntry;
                }
            }
        }

        private static void SetTaxServiceFailureFlag(InvoiceViewModel viewModel, ManualInvoiceViewModel manualInvoiceModel, int originalInvoiceTypeId)
        {
            viewModel.IsTaxServiceFailure = true;
            manualInvoiceModel.IsTaxServiceFailure = true;
            SetInvoiceCreationTypeToDdt(viewModel, manualInvoiceModel);

            if (originalInvoiceTypeId == (int)InvoiceType.Manual || originalInvoiceTypeId == (int)InvoiceType.MobileApp)
            {
                viewModel.WaitingForAction = (int)WaitingAction.AvalaraTax;
                manualInvoiceModel.WaitingForAction = (int)WaitingAction.AvalaraTax;
            }
        }

        private static void SetInvoiceCreationTypeToDdt(InvoiceViewModel viewModel, ManualInvoiceViewModel manualInvoiceModel)
        {
            if (viewModel.InvoiceTypeId == (int)InvoiceType.Manual)
            {
                viewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                manualInvoiceModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
            }
            else if (viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp)
            {
                viewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                manualInvoiceModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
            }
        }

        private void SetTaxAmountForNonStandardProduct(InvoiceViewModel viewModel, ManualInvoiceViewModel manualInvoiceModel, Order order)
        {
            var taxFromManualEntry = GetTaxDetailsFromManualTaxInputs(manualInvoiceModel, order, viewModel.Id, viewModel.BasicAmount, viewModel.DroppedGallons);
            viewModel.TotalTaxAmount = taxFromManualEntry.TotalTaxAmount;
            viewModel.TransactionId = taxFromManualEntry.TranId.ToString();
            viewModel.TaxDetails = taxFromManualEntry;
        }

        private void SetTaxAmountForEditNonStandardProduct(InvoiceViewModel viewModel, ManualInvoiceViewModel manualInvoiceModel, Order order)
        {
            var invoiceCommonDomain = new InvoiceCommonDomain(this);
            var taxFromManualEntry = invoiceCommonDomain.GetTaxDetailsFromInputs(manualInvoiceModel.Taxes, order.FuelRequest.Currency, viewModel.Id, viewModel.BasicAmount, viewModel.DroppedGallons);
            viewModel.TotalTaxAmount = taxFromManualEntry.TotalTaxAmount;
            viewModel.TransactionId = taxFromManualEntry.TranId.ToString();
            viewModel.TaxDetails = taxFromManualEntry;
            if (viewModel.BolDetails != null)
            {
                viewModel.BolDetails.FuelTypeId = order.FuelRequest.FuelTypeId;
                viewModel.BolDetails.PricePerGallon = manualInvoiceModel?.InvoiceCreationPricePerGallon ?? 0.00M;
            }
        }

        public bool GetInvoicePreviousStatus(Invoice invoice)
        {
            bool isWaitingForApproval = false;
            var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceHeader.InvoiceNumberId == invoice.InvoiceHeader.InvoiceNumberId);
            var invoiceStatuses = invoices.SelectMany(t => t.InvoiceXInvoiceStatusDetails.Select(t1 => t1.StatusId));
            if (!invoiceStatuses.Any(t => t == (int)InvoiceStatus.Received))
            {
                isWaitingForApproval = true;
            }
            return isWaitingForApproval;
        }

        private void SetInvoiceAmountAndFees(InvoiceDetailViewModel response, Invoice invoice)
        {
            if (invoice.WaitingFor == (int)WaitingAction.Nothing && invoice.Order != null)
            {
                var invoiceHeader = Context.DataContext.InvoiceHeaderDetails.FirstOrDefault(t => t.Id == invoice.InvoiceHeaderId);
                response.TotalInvoiceAmount = invoiceHeader.TotalBasicAmount + invoiceHeader.TotalFeeAmount - invoiceHeader.TotalDiscountAmount;
                //response.TotalInvoiceAmount = response.Invoice.BasicAmount + response.Invoice.TotalFees - response.Invoice.TotalDiscountAmount;
                if (invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    //response.TotalInvoiceAmount += invoice.TotalTaxAmount;
                    response.TotalInvoiceAmount += invoiceHeader.TotalTaxAmount;
                }
            }
        }

        private async Task SetInvoiceReceivePaymentStatus(InvoiceDetailViewModel response)
        {
            try
            {
                var invoicePaidAmounts = await Context.DataContext.InvoicePayments.Where(t => t.InvoiceNumberId == response.Invoice.InvoiceNumber.Id && t.IsActive)
                    .Select(t1 => new
                    {
                        t1.AmountPaid,
                        t1.BalanceRemaining
                    }).ToListAsync();

                if (invoicePaidAmounts != null && invoicePaidAmounts.Count > 0)
                {
                    response.AmountPaid = Math.Round(invoicePaidAmounts.Sum(t => t.AmountPaid), 2);
                    response.BalanceRemaining = Math.Round(invoicePaidAmounts.Min(t => t.BalanceRemaining));
                    if (response.BalanceRemaining == 0)
                        response.PaymentStatus = PaymentStatus.Paid;
                    else if (response.BalanceRemaining > 0)
                        response.PaymentStatus = PaymentStatus.PartiallyPaid;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "SetInvoiceReceivePaymentStatus", ex.Message, ex);
            }
        }

        private void SetInvoiceAmountAndFees(InvoiceDetailViewModel response, UspGetSupplierInvoiceDetails invoice)
        {
            if (invoice.WaitingFor == (int)WaitingAction.Nothing && invoice.OrderId != null)
            {
                //response.TotalInvoiceAmount = response.Invoice.BasicAmount + response.Invoice.TotalFees - response.Invoice.TotalDiscountAmount;
                response.TotalInvoiceAmount = invoice.TotalInvoiceAmount;
                //if (invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                //{
                //   response.TotalInvoiceAmount += invoice.TotalTaxAmount;
                //}
            }
        }

        private List<AssetDropViewModel> GetAmpAssetDrops(AmpJobViewModel viewModel, Order order, Job job)
        {
            var jobDrops = new List<AssetDropViewModel>();
            foreach (var drop in viewModel.Drops)
            {
                var jobAsset = job.JobXAssets.FirstOrDefault(t => t.RemovedBy == null && t.Asset.Name.Equals(drop.AssetName, StringComparison.OrdinalIgnoreCase));
                if (jobAsset != null)
                {
                    jobDrops.Add(new AssetDropViewModel
                    {
                        AssetName = jobAsset.Asset.Name,
                        OrderId = order.Id,
                        JobXAssetId = jobAsset.Id,
                        DropGallons = drop.DropQuantity,
                        StartTime = drop.StartTime.ToString(),
                        EndTime = drop.StartTime.ToString()
                    });
                }
            }
            jobDrops = jobDrops.Where(t => t.JobXAssetId > 0).ToList();
            return jobDrops;
        }

        private void GetFtlSupplierDtnDetails(InvoiceDetailViewModel response, int companyId)
        {
            string ftlSuppliers = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingFTLSupplierDtnDetails).Select(t => t.Value).First();
            DtnSupplierDetails ftlSupplierDetails = JsonConvert.DeserializeObject<DtnSupplierDetails>(ftlSuppliers);
            DtnSuppliers ftlSupplier = ftlSupplierDetails.DtnSuppliers.FirstOrDefault(t => t.CompanyId == companyId);
            string buyerSiteNumber = ftlSupplierDetails.SiteNumbers.Where(t => t.BuyerCompanyId == response.BuyerCompanyId && t.SupplierCompanyId == companyId).Select(t => t.BuyerSiteNumber).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(buyerSiteNumber))
            {
                buyerSiteNumber = ftlSupplierDetails.SiteNumbers.Where(t => t.BuyerCompanyId == response.BuyerCompanyId && t.SupplierCompanyId == null).Select(t => t.BuyerSiteNumber).FirstOrDefault();
            }
            if (!string.IsNullOrEmpty(buyerSiteNumber) && ftlSupplier != null)
            {
                response.RefId = ftlSupplier.RefId;
                response.Password = ftlSupplier.Password;
                response.SiteNumber = buyerSiteNumber;
            }
        }

        private async Task<StatusViewModel> GenerateInvoiceAnotherCustomerWorkflow(InvoiceViewModel viewModel, ManualInvoiceViewModel manualInvoiceModel = null, bool isDryRunOrAssignToOrder = false, int parentId = 0, InvoiceCreationFrom invoiceCreationFrom = InvoiceCreationFrom.Default)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("InvoiceDomain", "GenerateInvoiceAnotherCustomerWorkflow"))
            {
                var invoice = new Invoice();
                bool isTransactionCommited = false;
                int originalInvoiceTypeId = viewModel.InvoiceTypeId;
                int originalWaitingForAction = viewModel.WaitingForAction;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                        var originalinvoice = new Invoice();

                        bool autocloseOrder = false;
                        string transactionId = string.Empty;
                        int oldTrackableScheduleId = 0;
                        decimal totalDelivered = 0;
                        int previousInvoiceStatusId = 0;

                        List<AssetDropViewModel> assetDrops = null;
                        if (manualInvoiceModel.Assets != null)
                        {
                            assetDrops = manualInvoiceModel.Assets;
                        }

                        var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.OrderId);
                        if (order != null)
                        {
                            manualInvoiceModel.TimeZoneName = order.FuelRequest.Job.TimeZoneName;
                            viewModel.TimeZoneName = order.FuelRequest.Job.TimeZoneName;
                            SetPaymentTermsToViewmodel(order, manualInvoiceModel);
                            SetInvoiceDueDate(manualInvoiceModel, viewModel);

                            var currentOrderPricingTypeId = order.FuelRequest.PricingTypeId;
                            var notificationEvent = EventType.InvoiceCreatedViaMobileDrop;
                            var invoiceNumber = Context.DataContext.InvoiceNumbers.FirstOrDefault(t => t.Id == manualInvoiceModel.InvoiceNumber.Id);
                            if (invoiceNumber == null)
                            {
                                notificationEvent = order.FuelRequest.Job != null && order.FuelRequest.Job.IsApprovalWorkflowEnabled ? EventType.InvoiceCreatedApprovalWorkflow : EventType.InvoiceCreatedViaMobileDrop;
                                invoiceNumber = manualInvoiceModel.InvoiceNumber.ToEntity();
                                Context.DataContext.InvoiceNumbers.Add(invoiceNumber);
                                await Context.CommitAsync();
                                viewModel.DisplayInvoiceNumber = invoiceNumber.Number;
                                manualInvoiceModel.DisplayInvoiceNumber = invoiceNumber.Number;
                            }
                            else
                            {
                                originalinvoice = invoiceNumber.InvoiceHeaderDetails.SelectMany(t => t.Invoices).FirstOrDefault(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active);
                                transactionId = originalinvoice.TransactionId;
                                oldTrackableScheduleId = originalinvoice.TrackableScheduleId != null ? originalinvoice.TrackableScheduleId.Value : 0;
                                manualInvoiceModel.IsInvoiceEdit = true;
                                viewModel.ExchangeRate = originalinvoice.ExchangeRate;
                                viewModel.DisplayInvoiceNumber = originalinvoice.DisplayInvoiceNumber;
                                manualInvoiceModel.DisplayInvoiceNumber = originalinvoice.DisplayInvoiceNumber;
                            }

                            viewModel.InvoiceNumber = invoiceNumber.ToViewModel();
                            viewModel.TerminalId = manualInvoiceModel.TerminalId;
                            viewModel.CityGroupTerminalId = manualInvoiceModel.CityGroupTerminalId;
                            viewModel.TerminalPricingDate = viewModel.DropEndDate;
                            manualInvoiceModel.TerminalPricingDate = viewModel.DropEndDate;
                            CheckWorkflowAndSetInvoiceCreationType(order, viewModel, manualInvoiceModel, parentId);
                            if (!viewModel.IsApprovalWorkflowEnabledForJob)
                            {
                                if (order.ExternalBrokerBuySellDetail == null)
                                {
                                    await SetInvoiceAmounts(order, viewModel);
                                }
                                else
                                {
                                    await SetInvoiceAmountsForBuySellOrder(order, viewModel, viewModel.IsBuyPriceInvoice);
                                }
                                if (viewModel.WaitingForAction != (int)WaitingAction.Nothing)
                                {
                                    SetInvoiceCreationTypeToDdt(viewModel, manualInvoiceModel);
                                }
                            }

                            if (viewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice)
                            {
                                viewModel.PricePerGallon = 0;
                                viewModel.BasicAmount = 0;
                            }

                            if (order.FuelRequest.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                && (order.DefaultInvoiceType == (int)InvoiceType.Manual || order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketManual) && !order.FuelRequest.Job.IsApprovalWorkflowEnabled)
                            {
                                manualInvoiceModel.TypeofFuel = (int)ProductDisplayGroups.OtherFuelType;
                                manualInvoiceModel.GetOrderTaxesForNonStandardFuel(order);
                                SetTaxAmountForNonStandardProduct(viewModel, manualInvoiceModel, order);
                            }
                            else if (viewModel.WaitingForAction == (int)WaitingAction.Nothing && !viewModel.IsApprovalWorkflowEnabledForJob && viewModel.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                            {
                                SetTaxAmountForStandardProduct(viewModel, manualInvoiceModel, order, isDryRunOrAssignToOrder, originalInvoiceTypeId);
                            }

                            if (string.IsNullOrEmpty(viewModel.TransactionId) || viewModel.TransactionId == "0")
                            {
                                viewModel.TransactionId = invoiceNumber.Number;
                            }
                            if (!isDryRunOrAssignToOrder)
                            {
                                var previousInvoice = order.Invoices.FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id
                                                                                && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active);
                                if (previousInvoice != null)
                                {
                                    previousInvoiceStatusId = previousInvoice.InvoiceXInvoiceStatusDetails.FirstOrDefault(t1 => t1.IsActive).StatusId;
                                    previousInvoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;
                                    Context.DataContext.Entry(previousInvoice).State = EntityState.Modified;
                                    await Context.CommitAsync();
                                }
                            }

                            var isConnectedWithBuyer = false;
                            if (order.FuelRequest.Job.IsApprovalWorkflowEnabled && order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                            {
                                if (order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.FuelRequest1 != null)
                                {
                                    var brokerOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.OrderByDescending(t => t.Id).FirstOrDefault();
                                    var parentOrder = GetConnectingBuyerOrder(brokerOrder);
                                    if (parentOrder == null)
                                    {
                                        isConnectedWithBuyer = true;
                                    }
                                }
                            }
                            Order brokeredOrder = null;
                            var previousStatus = order.Invoices.FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id && t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.WaitingForApproval));
                            if (viewModel.StatusId == (int)InvoiceStatus.Received && ((order.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.GetParentFuelRequest().FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.Job.IsApprovalWorkflowEnabled) || (isConnectedWithBuyer)))
                            {
                                if (!manualInvoiceModel.IsInvoiceFromDropTicket && order.FuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                                {
                                    var childRequest = order.FuelRequest.GetParentFuelRequest().FuelRequest1;
                                    brokeredOrder = childRequest.Orders.LastOrDefault();

                                    if (brokeredOrder != null && order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery &&
                                        (
                                            brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyClosed
                                            || brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed
                                        ))
                                    {
                                        viewModel.StatusId = (int)InvoiceStatus.Received;
                                    }
                                }
                                else if (previousStatus == null)
                                {
                                    viewModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                                }
                                else
                                {
                                    var lastInvoice = order.Invoices.OrderByDescending(t => t.Id).FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id);
                                    viewModel.StatusId = lastInvoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault().StatusId;
                                    if (lastInvoice != null && lastInvoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault().StatusId == (int)InvoiceStatus.Rejected)
                                    {
                                        if (!lastInvoice.InvoiceXInvoiceStatusDetails.Any(t => t.StatusId == (int)InvoiceStatus.Received))
                                        {
                                            viewModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                                        }
                                        else
                                        {
                                            viewModel.StatusId = (int)InvoiceStatus.Received;
                                        }
                                    }
                                }
                            }

                            await SetInvoiceAdditionDetails(viewModel, originalinvoice.Id, order.Id);
                            invoice = viewModel.ToEntity();
                            invoice.SignatureId = manualInvoiceModel.SignatureId;
                            invoice.SupplierPreferredInvoiceTypeId = originalInvoiceTypeId;

                            // update invoice number
                            UpdateInvoiceNumber(invoice, viewModel, manualInvoiceModel);

                            if (viewModel.Image != null && viewModel.Image.IsRemoved)
                            {
                                var image = await Context.DataContext.Images.FirstOrDefaultAsync(t => t.Id == viewModel.Image.Id);
                                if (image != null)
                                {
                                    invoice.Image = null;
                                    Context.DataContext.Images.Remove(image);
                                }
                            }
                            if (parentId > 0)
                            {
                                // DDT to invoice create
                                invoice.ParentId = parentId;
                                var parentInvoice = Context.DataContext.Invoices.FirstOrDefault(t => t.Id == parentId);
                                if (parentInvoice != null)
                                {
                                    parentInvoice.IsActive = false;
                                    parentInvoice.WaitingFor = invoice.WaitingFor;
                                    Context.DataContext.Entry(parentInvoice).State = EntityState.Modified;
                                    await Context.CommitAsync();
                                }
                            }

                            if (viewModel.AdditionalDetail != null)
                            {
                                invoice.InvoiceXAdditionalDetail = viewModel.AdditionalDetail.ToEntity();
                            }

                            if (viewModel.SpecialInstructions != null && viewModel.SpecialInstructions.Count > 0)
                            {
                                invoice.InvoiceXSpecialInstructions = viewModel.SpecialInstructions.Select(t => t.ToEntity()).ToList();
                            }
                            // remove dry run fees
                            RemoveDryRunFees(manualInvoiceModel);
                            //FuelRequestFee Entity
                            if (manualInvoiceModel.ExternalBrokerId <= 0)
                            {
                                invoice.FuelRequestFees = manualInvoiceModel.FuelDeliveryDetails.FuelFees.ToInvoiceFeesEntity(viewModel.DropStartDate.Date);
                            }
                            else
                            {
                                invoice.FuelRequestFees = manualInvoiceModel.ExternalBrokeredOrder.BrokeredOrderFee.ToEntity();
                            }

                            //Tax details Entity
                            invoice.TaxDetails = viewModel.TaxDetails.ToEntity();

                            if (manualInvoiceModel.IsInvoiceEdit && !isDryRunOrAssignToOrder)
                            {
                                if (manualInvoiceModel.TypeofFuel != (int)ProductDisplayGroups.OtherFuelType)
                                {
                                    TaxUpdationForStandardFuelType(originalinvoice, invoice);
                                }
                            }

                            invoice.PaymentTermId = manualInvoiceModel.PaymentTermId;
                            if (manualInvoiceModel.PaymentTermId == (int)PaymentTerms.NetDays)
                            {
                                invoice.NetDays = manualInvoiceModel.NetDays;
                            }

                            invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.Active;
                            invoice.Version = 1;
                            invoice.CreatedBy = viewModel.UserId;
                            invoice.UpdatedBy = viewModel.UserId;
                            invoice.CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(manualInvoiceModel.TimeZoneName);
                            invoice.UpdatedDate = invoice.CreatedDate;
                            //order.Invoices.Add(invoice);

                            var invoiceHeader = viewModel.ToInvoiceHeaderEntity();
                            Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                            await Context.CommitAsync();

                            invoiceHeader.Invoices.Add(invoice);
                            //order.Invoices.Add(invoice);
                            await Context.CommitAsync();
                            await SetMobileInvoiceBolDetails(invoiceHeader.Id, viewModel, invoice.Id);
                            await SaveBulkPlantLocation(viewModel.BolDetails);

                            var job = order.FuelRequest.Job;
                            if (job.JobBudget.IsAssetTracked && assetDrops != null && assetDrops.Count > 0)
                            {
                                InvoiceCommonDomain invoiceCommonDomain = new InvoiceCommonDomain(this);
                                assetDrops = invoiceCommonDomain.SetJobAssetId(assetDrops, viewModel.UserId, job);
                                SetAssetDropsToInvoice(invoice, assetDrops);
                            }

                            //need to get fees after adding asset drops
                            invoice.TotalFeeAmount = new HelperDomain(this).SetCalculatedInvoiceFeesTotal(invoice);

                            //// close order
                            autocloseOrder = AutoCloseOrder(order, out totalDelivered, viewModel.TrackableScheduleId);
                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            var delReqStatuses = UpdateInvoiceTrackableScheduleId(viewModel, invoice, oldTrackableScheduleId, order);

                            await Context.CommitAsync();
                            transaction.Commit();
                            isTransactionCommited = true;
                            if (delReqStatuses != null && delReqStatuses.Any())
                            {
                                new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatuses);
                            }
                            NotificationDomain notificationDomain = new NotificationDomain(this);
                            var userId = viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp ? order.AcceptedBy : viewModel.UserId;
                            if (invoice.WaitingFor == (int)WaitingAction.UpdatedPrice)
                            {
                                notificationEvent = EventType.DdtCreatedAsInvoiceIsWaitingForUpdatedPrice;
                            }
                            await notificationDomain.AddNotificationEventAsync(notificationEvent, invoice.InvoiceHeaderId, userId);


                            SetHedgeSpotAmounts(order, invoice, manualInvoiceModel.IsInvoiceEdit, viewModel.IsRecursiveCallForBrokerOrders);

                            await Context.CommitAsync();
                            var deliveryTypeId = order.FuelRequest.FuelRequestDetail.DeliveryTypeId;
                            if (order.FuelRequest.MaxQuantity > 0 && autocloseOrder && deliveryTypeId != (int)DeliveryType.OneTimeDelivery)
                            {
                                await newsfeedDomain.SetSystemOrderAutoClosedNewsfeed(order, totalDelivered);
                            }
                            //FuelRequest.FuelRequest1 gives child FR details
                            if (!manualInvoiceModel.IsInvoiceFromDropTicket && order.FuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                            {
                                var childRequest = order.FuelRequest.GetParentFuelRequest().FuelRequest1;
                                brokeredOrder = childRequest.Orders.LastOrDefault();
                                var isSingleDeliveryClosedOrder = false;

                                SetBrokeredChainId(viewModel);
                                invoice.BrokeredChainId = viewModel.BrokeredChainId;

                                if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery &&
                                    (
                                        brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyClosed
                                        || brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed
                                    ))
                                {
                                    isSingleDeliveryClosedOrder = true;
                                }
                                if (!isSingleDeliveryClosedOrder)
                                {
                                    brokeredOrder = brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open ? brokeredOrder : GetConnectingBuyerOrder(brokeredOrder);
                                }

                                if (brokeredOrder != null &&
                                    (
                                        brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                        || isSingleDeliveryClosedOrder
                                    ))
                                {
                                    //assing new/child FR's order id and userid to viewmodel and create invoice for this
                                    viewModel.OrderId = brokeredOrder.Id;
                                    viewModel.PoNumber = brokeredOrder.PoNumber;
                                    viewModel.UserId = brokeredOrder.AcceptedBy;
                                    viewModel.UpdatedBy = brokeredOrder.AcceptedBy;
                                    viewModel.CreatedBy = brokeredOrder.AcceptedBy;
                                    viewModel.IsRecursiveCallForBrokerOrders = true;
                                    if (currentOrderPricingTypeId != brokeredOrder.FuelRequest.PricingTypeId)
                                    {
                                        if (brokeredOrder.FuelRequest.PricingTypeId == (int)PricingType.PricePerGallon
                                            || brokeredOrder.FuelRequest.PricingTypeId == (int)PricingType.Suppliercost)
                                        {
                                            viewModel.IsRecursiveCallForBrokerOrders = false;
                                        }
                                    }
                                    if (viewModel.TrackableScheduleId != null)//updating deliveryschedule id for broker order
                                    {
                                        var trackableSchedule = order.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.Id == viewModel.TrackableScheduleId);
                                        viewModel.TrackableScheduleId = trackableSchedule.DeliverySchedule.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.OrderId == brokeredOrder.Id && t.Date == trackableSchedule.Date && t.IsActive).Id;
                                    }
                                    var brokeredInvoice = Context.DataContext.Invoices.Where(t => t.TransactionId == transactionId && t.OrderId == viewModel.OrderId).FirstOrDefault();
                                    if (manualInvoiceModel.IsInvoiceEdit)
                                    {
                                        if (brokeredInvoice != null)
                                        {
                                            manualInvoiceModel.InvoiceNumber.Id = brokeredInvoice.InvoiceHeader.InvoiceNumberId;
                                        }
                                    }
                                    if ((viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp || viewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) && (assetDrops == null || !assetDrops.Any()))
                                    {
                                        manualInvoiceModel.Assets = GetAssetDropDetails(invoice);
                                    }
                                    if (manualInvoiceModel.Assets != null)
                                    {
                                        // Update Order-Id of assets in broker case
                                        manualInvoiceModel.Assets.ForEach(t => t.OrderId = brokeredOrder.Id);
                                    }
                                    var fuleRequestOfBrokeredOrder = brokeredOrder.FuelRequest;
                                    manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = GetFuelRequestFee(fuleRequestOfBrokeredOrder);
                                    manualInvoiceModel.PaymentTermId = brokeredOrder.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentTermId;
                                    viewModel.InvoiceTypeId = originalInvoiceTypeId;
                                    viewModel.WaitingForAction = originalWaitingForAction;
                                    var brokeredCaseResponse = await GenerateInvoiceAnotherCustomerWorkflow(viewModel, manualInvoiceModel, true);
                                    if (invoiceCreationFrom != InvoiceCreationFrom.Default && brokeredCaseResponse.StatusCode == Status.Success)
                                    {
                                        var authenticationDomain = new AuthenticationDomain(this);
                                        var userContext = await authenticationDomain.GetUserContextAsync(viewModel.CreatedBy, CompanyType.Supplier);
                                        if (parentId <= 0 && invoiceCreationFrom == InvoiceCreationFrom.ManualSFX)
                                        {
                                            await SetNewsfeedForManualCreateInvoiceAsync(userContext, viewModel, manualInvoiceModel, manualInvoiceModel.IsInvoiceEdit);
                                        }
                                        else if (invoiceCreationFrom == InvoiceCreationFrom.Mobile)
                                        {
                                            await SetNewsfeedForManualCreateInvoiceMobileAsync(order, viewModel);
                                        }
                                    }
                                }
                            }

                            response.StatusCode = Status.Success;
                            if (viewModel.IsTaxServiceFailure && originalInvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual)
                            {
                                response.StatusMessage = Resource.successMessageDDTCreatedDueToAvaTaxFailure;
                            }
                            else
                            {
                                if (viewModel.IsApprovalWorkflowEnabledForJob && (originalInvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual || originalInvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp))
                                {
                                    response.StatusMessage = Resource.SuccessMessgeDDTCreatedAsApprovalWorkflowIsEnabled;
                                }
                                else if (viewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice)
                                {
                                    response.StatusMessage = Resource.errMessageDDTCreatedWaitingForUpdatedPrice;
                                }
                                else
                                {
                                    response.StatusMessage = IsDigitalDropTicket(invoice.InvoiceTypeId)
                                                            ? Resource.errMessageDropTicketCreateSuccess
                                                            : Resource.errMessageInvoiceCreateSuccess;
                                }
                            }
                            CreateQbAccountingWorkflowForInvoice(manualInvoiceModel.IsInvoiceEdit, invoice, order, brokeredOrder?.Id);
                            CreateQbAccountingWorkflowForBill(manualInvoiceModel.IsInvoiceEdit, invoice, order, brokeredOrder?.Id);
                            CreatePDIAPIWorkflow(invoice, order);
                            SAPEnterpriseDomain sapDomain = new SAPEnterpriseDomain(this);
                            sapDomain.CreateSAPWorkflow(invoice);
                            bool isDtnUploaded = CreateDtnFileGenerationWorkflow(invoice, order);
                            UpdateInvoiceActionResponseStatus(isDtnUploaded, response);
                        }

                        viewModel = invoice.ToViewModel(viewModel);
                        manualInvoiceModel.InvoiceId = invoice.Id;
                        manualInvoiceModel.InvoiceHeaderId = invoice.InvoiceHeaderId;
                        manualInvoiceModel.BuyerCompanyId = invoice.Order.FuelRequest.Job.CompanyId;
                        manualInvoiceModel.InvoiceNumber.Number = invoice.DisplayInvoiceNumber;
                        manualInvoiceModel.JobId = invoice.Order.FuelRequest.JobId;
                        manualInvoiceModel.InvoiceTypeId = viewModel.InvoiceTypeId;
                        manualInvoiceModel.OrderId = viewModel.OrderId ?? 0;

                        if (!manualInvoiceModel.IsInvoiceEdit && viewModel.StatusId != (int)InvoiceStatus.Draft)
                        {
                            await SetApprovalWorkflowEnabledNewsFeeds(invoice, newsfeedDomain, order);
                        }
                        else
                        {
                            if (previousInvoiceStatusId == (int)InvoiceStatus.Draft && viewModel.StatusId != (int)InvoiceStatus.Draft)
                            {
                                await newsfeedDomain.SetDigitalDropTicketDraftConvertedNewsfeed(order, viewModel);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                        if (!isTransactionCommited)
                        {
                            transaction.Rollback();
                        }
                        LogManager.Logger.WriteException("InvoiceDomain", "GenerateInvoiceAnotherCustomerWorkflow", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        public async Task<List<InvoiceHistoryGridViewModel>> GetInvoiceHistoryGridBuyerAsync(int userId, int id = 0)
        {
            List<InvoiceHistoryGridViewModel> response = new List<InvoiceHistoryGridViewModel>();

            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId && t.IsActive);

                if (user != null && user.Company != null)
                {
                    HelperDomain helperDomain = new HelperDomain(this);

                    var allInvoices = Context.DataContext.Invoices.Where(t => (t.InvoiceHeader.InvoiceNumberId == id && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive) &&
                                                                               t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.IsActive && (t1.StatusId != (int)InvoiceStatus.Unassigned && t1.StatusId != (int)InvoiceStatus.Draft)))
                                                                  .OrderByDescending(t => t.Id).ToList();
                    foreach (var item in allInvoices)
                    {
                        var invoiceHistory = new InvoiceHistoryGridViewModel();

                        invoiceHistory.Id = item.Id;
                        invoiceHistory.InvoiceNumber = item.DisplayInvoiceNumber;
                        invoiceHistory.InvoiceHeaderId = item.InvoiceHeaderId;
                        invoiceHistory.InvoiceAmount = item.OrderId == null ? 0 : helperDomain.GetInvoiceAmount(item);
                        invoiceHistory.DropDate = item.DropEndDate.ToString(Resource.constFormatDate);
                        invoiceHistory.DropTime = $"{item.DropStartDate.DateTime.ToShortTimeString()}{Resource.lblSingleHyphen}{item.DropEndDate.DateTime.ToShortTimeString()}";
                        var invoiceStatus = item.InvoiceXInvoiceStatusDetails.Single(t => t.IsActive);
                        if (invoiceStatus.StatusId == (int)InvoiceStatus.Draft || invoiceStatus.StatusId == (int)InvoiceStatus.Canceled)
                        {
                            invoiceHistory.InvoiceDate = Resource.lblHyphen;
                        }
                        else
                        {
                            invoiceHistory.InvoiceDate = item.UpdatedDate.ToString(Resource.constFormatDate);
                        }

                        invoiceHistory.Version = Convert.ToString("V" + item.Version);
                        invoiceHistory.Quantity = item.DroppedGallons.GetCommaSeperatedValue();
                        if (item.Order != null)
                        {
                            invoiceHistory.PricePerGallon = item.Order.FuelRequest.PricingTypeId == (int)PricingType.Tier ?
                                                item.Order.FuelRequest.MstPricingType.Name :
                                                item.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).Select(t => t.PricePerGallon).First().ToString(ApplicationConstants.DecimalFormat2);
                        }
                        else
                        {
                            invoiceHistory.PricePerGallon = Resource.lblHyphen;
                        }
                        invoiceHistory.ModifiedDate = item.UpdatedDate.ToString(Resource.constFormatDate);
                        var updatedByUser = Context.DataContext.Users.Where(x => x.Id == item.UpdatedBy).Select(s => new { s.FirstName, s.LastName }).FirstOrDefault();
                        invoiceHistory.ModifiedBy = $"{updatedByUser.FirstName} {updatedByUser.LastName}";
                        invoiceHistory.Status = invoiceStatus.MstInvoiceStatus.Name;
                        response.Add(invoiceHistory);
                    }
                    response = response.CorrectValues();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceHistoryGridBuyerAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> SaveDiscountAsync(DiscountViewModel viewModel, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            NotificationDomain notificationDomain = new NotificationDomain();
            try
            {
                var currentInvoice = Context.DataContext.Invoices.Where(t => t.Id == viewModel.InvoiceId)
                                        .Select(t => new { t.Id, t.InvoiceHeader.InvoiceNumberId, t.InvoiceVersionStatusId }).FirstOrDefault();
                if (currentInvoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive)
                {
                    viewModel.InvoiceId = Context.DataContext.Invoices.Where(t => t.InvoiceHeader.InvoiceNumberId == currentInvoice.InvoiceNumberId
                                        && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t => t.Id).First();
                }
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var discount = viewModel.ToEntity();
                        Context.DataContext.Discounts.Add(discount);
                        await Context.CommitAsync();
                        transaction.Commit();

                        var newsfeedDomain = new NewsfeedDomain(this);
                        await newsfeedDomain.SetDealCreatedNewsfeed(userContext, discount.Id, viewModel.InvoiceId);

                        await notificationDomain.AddNotificationEventAsync(EventType.DealCreatedForInvoice, discount.Id, userContext.Id);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageDealCreatedSucess;
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageCreateRequestFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("InvoiceDomain", "SaveDiscountAsync", ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "SaveDiscountAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> DealAgreeAsync(UserContext userContext, int discountId, int invoiceId, int invoiceHeaderId)
        {
            var response = new StatusViewModel();
            try
            {
                var invoice = await Context.DataContext.Invoices
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.InvoiceHeader.InvoiceNumberId,
                                            x.CreatedBy,
                                            x.InvoiceVersionStatusId,
                                            supplierCompanyId = x.Order.AcceptedCompanyId
                                        }).FirstOrDefaultAsync(t => t.Id == invoiceId);

                if (invoice != null && invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive)
                {
                    response.EntityId = await GetLatestInvoiceId(invoice.InvoiceNumberId);
                    response.StatusMessage = Resource.errMessageOldInvoiceVersion;
                    return response;
                }

                if (invoice != null)
                {
                    List<ManualInvoiceViewModel> manualInvoiceModels = new List<ManualInvoiceViewModel>();
                    var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId).Select(t => t.Id).ToList();
                    foreach (int manulInvoice in invoices)
                    {
                        var invoiceDomain = new InvoiceDomain(this);
                        var manualInvoiceViewModel = await invoiceDomain.GetManualInvoiceForEditAsync(manulInvoice);
                        manualInvoiceViewModel.userId = invoice.CreatedBy;
                        manualInvoiceViewModel.UpdatedBy = userContext.Id;
                        if (manualInvoiceViewModel.InvoiceId == invoiceId)
                        {
                            manualInvoiceViewModel.DiscountId = discountId;
                            manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.DiscountLineItems = GetDiscountLineItems(discountId, manualInvoiceViewModel.FuelDeliveryDetails.FuelFees.DiscountLineItems);
                        }
                        manualInvoiceModels.Add(manualInvoiceViewModel);
                    }

                    var invoiceEditDomain = new InvoiceEditDomain(this);
                    var dealResponse = await invoiceEditDomain.ConsolidatedInvoiceDealAsync(userContext, manualInvoiceModels);

                    response.EntityId = dealResponse.InvoiceId;

                    var newsfeedDomain = new NewsfeedDomain(this);
                    await newsfeedDomain.SetDealAcceptedNewsfeed(userContext, invoice.supplierCompanyId, dealResponse, discountId);

                    var dealName = Context.DataContext.Discounts.Select(x => new { x.Id, x.DealName }).FirstOrDefault(t => t.Id == discountId)?.DealName;
                    if (!string.IsNullOrEmpty(dealName))
                    {
                        var newDiscountId = Context.DataContext.Discounts.Select(x => new { x.Id, x.DealName, x.InvoiceId }).FirstOrDefault(t => t.DealName == dealName && t.InvoiceId == dealResponse.InvoiceId);
                        if (newDiscountId != null)
                        {
                            var notificationDomain = new NotificationDomain(this);
                            await notificationDomain.AddNotificationEventAsync(EventType.DealAcceptedForInvoice, newDiscountId.Id, userContext.Id);
                        }
                    }
                }

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageDealAgreedSuccessfully;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "DealAgreeAsync", ex.Message, ex);
                response.StatusMessage = Resource.errMessageUpdateFailed;
            }

            return response;
        }

        public async Task<StatusViewModel> DealNotAgreeAsync(UserContext userContext, int discountId, int invoiceId)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var discount = await Context.DataContext.Discounts.SingleOrDefaultAsync(t => t.Id == discountId);
                if (discount != null)
                {
                    discount.DealStatus = (int)DealStatus.Declined;
                    discount.StatusChangedBy = userContext.Id;
                    discount.StatusChangedCompanyId = userContext.CompanyId;
                    discount.StatusChangedDate = DateTimeOffset.Now;

                    Context.DataContext.Entry(discount).State = EntityState.Modified;
                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageDealNotAgreed;

                    var newsfeedDomain = new NewsfeedDomain(this);
                    await newsfeedDomain.SetDealDeclinedNewsfeed(userContext, discountId, invoiceId);

                    var notificationDomain = new NotificationDomain(this);
                    await notificationDomain.AddNotificationEventAsync(EventType.DealDeclinedForInvoice, discountId, userContext.Id);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "DealNotAgreeAsync", ex.Message, ex);
            }

            return response;
        }

        public List<DiscountLineItemViewModel> GetDiscountLineItems(int discountId, List<DiscountLineItemViewModel> discountLineItem)
        {
            if (discountLineItem == null)
            {
                discountLineItem = new List<DiscountLineItemViewModel>();
            }

            try
            {
                var discountLineItems = Context.DataContext.DiscountLineItems.Where(t => t.DiscountId == discountId);
                foreach (var item in discountLineItems)
                {
                    if (item.FeeTypeId != (int)FeeType.OtherFee)
                    {
                        discountLineItem.Add(new DiscountLineItemViewModel { FeeTypeId = item.FeeTypeId, FeeSubTypeId = item.FeeSubTypeId, Amount = item.Amount, Id = item.Id });
                    }
                    else
                    {
                        discountLineItem.Add(new DiscountLineItemViewModel { FeeTypeId = item.FeeTypeId, FeeSubTypeId = item.FeeSubTypeId, Amount = item.Amount, Id = item.Id, FeeDetails = item.FeeDetails });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetDiscountLineItems", ex.Message, ex);
            }
            return discountLineItem;
        }

        public Tuple<List<FeesViewModel>, List<DiscountLineItemViewModel>> GetPreviousInvoiceFees(int invoiceId, int invoiceNumberId)
        {
            var fees = new List<FeesViewModel>();
            var discounts = new List<DiscountLineItemViewModel>();
            try
            {
                var invoiceFees = Context.DataContext.Invoices.Where(t => t.InvoiceHeader.InvoiceNumberId == invoiceNumberId
                        && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive && t.Id < invoiceId)
                        .OrderByDescending(t => t.Id).Select(t => t.FuelRequestFees).FirstOrDefault();
                if (invoiceFees != null && invoiceFees.Any())
                {
                    fees = invoiceFees.Where(t => t.DiscountLineItemId == null && t.TotalFee != null && t.TotalFee != 0).OrderBy(t => t.FeeTypeId).ToList().ToFeesViewModel();
                    discounts = invoiceFees.Where(t => t.DiscountLineItemId != null && t.TotalFee != null && t.TotalFee != 0).OrderBy(t => t.Id).ToList().ToDiscountFeesViewModel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceFees", ex.Message, ex);
            }
            return new Tuple<List<FeesViewModel>, List<DiscountLineItemViewModel>>(fees, discounts);
        }

        public async Task<List<DiscountSummaryViewModel>> GetDiscountSummary(int invoiceId)
        {
            using (var tracer = new Tracer("InvoiceDomain", "GetDiscountSummary"))
            {
                var response = new List<DiscountSummaryViewModel>();
                try
                {
                    var invoice = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => new { t.Currency, t.InvoiceHeaderId }).SingleOrDefaultAsync();
                    if (invoice != null)
                    {
                        var sqlQuery = Context.DataContext.Discounts.Where(t => t.InvoiceId == invoiceId).OrderByDescending(t => t.CreatedDate);
                        foreach (var item in sqlQuery)
                        {
                            var discountLineItems = new List<DiscountLineItemViewModel>();
                            var discount = new DiscountSummaryViewModel();
                            discount.DiscountId = item.Id;
                            discount.DealName = item.DealName;
                            discount.Notes = item.Notes;
                            discount.CreatedBy = $"{item.User.FirstName} {item.User.LastName}";
                            discount.CreatedCompanyId = item.CreatedCompanyId;
                            discount.InvoiceHeaderId = invoice.InvoiceHeaderId;
                            discount.DealStatusId = item.DealStatus;

                            if (item.DealStatus == (int)DealStatus.Pending)
                                discount.DealStatus = Resource.lblPending;
                            else if (item.DealStatus == (int)DealStatus.Accepted)
                                discount.DealStatus = Resource.lblApproved;
                            else if (item.DealStatus == (int)DealStatus.Declined)
                                discount.DealStatus = Resource.lblDeclined;

                            foreach (var discountItem in item.DiscountLineItems)
                            {
                                DiscountLineItemViewModel model = new DiscountLineItemViewModel();
                                if (discountItem.FeeTypeId != (int)FeeType.OtherFee)
                                    model.FeeTypeName = invoice.Currency == Currency.CAD ? discountItem.MstFeeType.Name.Replace(Constants.Gallon, Constants.Litre) : discountItem.MstFeeType.Name;
                                else
                                    model.FeeTypeName = discountItem.FeeDetails;
                                model.FeeSubTypeName = invoice.Currency == Currency.CAD ? discountItem.MstFeeSubType.Name.Replace(Constants.Gallon, Constants.Litre) : discountItem.MstFeeSubType.Name;
                                model.FeeSubTypeName = model.FeeSubTypeName.Replace(Resource.lblFlatFee, Resource.lblFlat);
                                model.Amount = discountItem.Amount.GetPreciseValue(6);
                                discountLineItems.Add(model);
                            }

                            discount.DiscountLineItem = discountLineItems;
                            response.Add(discount);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceDomain", "GetDiscountSummary", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<List<InvoiceGridViewModel>> GetInvoiceGridAsync(InvoiceDataTableViewModel invoiceFilter = null)
        {
            List<InvoiceGridViewModel> response = new List<InvoiceGridViewModel>();
            try
            {
                if (invoiceFilter == null)
                {
                    invoiceFilter = new InvoiceDataTableViewModel();
                }

                response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetInvoiceGridAsync(invoiceFilter);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceGridAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<InvoiceDetailViewModel> GetInvoiceDetailAsync(int invoiceId)
        {
            var response = new InvoiceDetailViewModel();
            try
            {
                HelperDomain helperDomain = new HelperDomain(this);
                var invoice = await Context.DataContext.Invoices.Include(t => t.Order).Include(t => t.Order.FuelRequest)
                    .Include(t => t.Order.FuelRequest.User).Include(t => t.Order.ExternalBrokerOrderDetail).Include(t => t.AssetDrops)
                    .FirstOrDefaultAsync(t => t.Id == invoiceId);
                if (invoice != null)
                {
                    //to redirect to Invoice Summary page when Invoice has been deleted
                    if (invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive && invoice.InvoiceXInvoiceStatusDetails.Last().StatusId == (int)InvoiceStatus.Deleted)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageInvoiceDeleted;
                        return response;
                    }
                    else if (invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive && invoice.InvoiceXInvoiceStatusDetails.Single(t => t.IsActive).StatusId == (int)InvoiceStatus.Draft)
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageDraftConvertedtoDDT;
                        return response;
                    }

                    response = new InvoiceDetailViewModel
                    {
                        PoNumber = invoice.Order == null ? Resource.lblHyphen : invoice.PoNumber,
                        OrderId = invoice.Order == null ? 0 : invoice.Order.Id,
                        Invoice = invoice.ToViewModel(),
                        PercentFuelDelivered = helperDomain.GetFuelDeliveredPercentagePerInvoice(invoice),
                        FuelRequest = invoice.Order == null ? new FuelRequestViewModel(Status.Success) : invoice.Order.FuelRequest.ToViewModel(),
                    };

                    // get fuel fee details
                    response.FuelDeliveryDetails.FuelFees = GetInvoiceFuelFees(invoice.FuelRequestFees,invoice.DroppedGallons);
                    SetSurchargeDetails(invoice.InvoiceXAdditionalDetail, response.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee, 0, response.Invoice.DroppedGallons); //producttype not req.

                    if (invoice.Order != null && invoice.Order.ExternalBrokerBuySellDetail != null)
                    {
                        response.IsBuyAndSellOrder = true;
                        response.BuyAndSellPricingDetail = GetBuyAndSellPricingDetails(invoice);
                        if (!string.IsNullOrWhiteSpace(response.BuyAndSellPricingDetail.ExternalBrokerName))
                        {
                            response.BuyerCompanyName = response.BuyAndSellPricingDetail.ExternalBrokerName;
                        }
                    }

                    response.Invoice.TotalFees = helperDomain.GetInvoiceTotalFees(invoice);
                    SetInvoiceAmountAndFees(response, invoice);

                    // set culture
                    response.Culture = helperDomain.SetEntityThreadCulture(invoice.Currency);

                    response = response.CorrectValues();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetInvoiceDetailAsync", ex.Message, ex);
            }
            return response;
        }

        #region Methods-calling-from-webJob
        public async Task<int> ProcessInvoicesWaitingForAxxisUpdatedPrice()
        {
            int generatedInvoices = 0;
            var ddtList = Context.DataContext.Invoices.Where(t => t.WaitingFor == (int)WaitingAction.UpdatedPrice
                                                                    && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                    && t.IsActive)
                                                                    .Select(t => new
                                                                    {
                                                                        t.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                                                                        t.SupplierPreferredInvoiceTypeId,
                                                                        t.Id,
                                                                        t.CreatedBy,
                                                                        t.Order.AcceptedCompanyId,
                                                                        t.InvoiceHeaderId,
                                                                        CompanyName = t.User.Company.Name,
                                                                        SplitLoadChainId = t.InvoiceXAdditionalDetail.SplitLoadChainId,
                                                                        TimeZoneName = t.Order.FuelRequest.Job.TimeZoneName
                                                                    })
                                                                    .ToList();
            var axxisRequestPriceDetailIds = await new PricingServiceDomain(this).GetPriceDetailIdsBySource(ddtList.Select(t => t.RequestPriceDetailId).ToList(), true);
            ddtList = ddtList.Where(t => axxisRequestPriceDetailIds.Contains(t.RequestPriceDetailId)).ToList();

            foreach (var item in ddtList)
            {
                try
                {
                    if (item.SupplierPreferredInvoiceTypeId.HasValue && IsDigitalDropTicket(item.SupplierPreferredInvoiceTypeId.Value))
                    {
                        //only update ppg and waiting for status
                        var result = await UpdateDDTAfterUpdatedPrice(item.Id);
                        if (result.StatusCode == Status.Success)
                        {
                            generatedInvoices++;
                        }

                        LogManager.Logger.WriteDebug("DDTUpdatedWithPrice", "ProcessInvoicesWaitingForAxxisUpdatedPrice", $"DDT Id:{item.Id}");
                        LogManager.Logger.WriteException("DDTUpdatedWithPrice", "ProcessInvoicesWaitingForAxxisUpdatedPrice", $"DDT Id:{item.Id}", new Exception());
                    }
                    else
                    {
                        //generated invoice from ddt
                        var status = await CreateInvoiceFromDropTicket(new UserContext() { Id = item.CreatedBy, CompanyId = item.AcceptedCompanyId, CompanyName = item.CompanyName }, item.Id, item.InvoiceHeaderId, item.CreatedBy);
                        {
                            if (status.StatusCode == Status.Success)
                            {
                                generatedInvoices++;
                                if (!string.IsNullOrEmpty(item.SplitLoadChainId))
                                {
                                    await CheckForSplitLoadInvoiceAndGenerateStatement(item.SplitLoadChainId, item.AcceptedCompanyId, item.TimeZoneName);
                                }
                            }

                            LogManager.Logger.WriteDebug("DDTtoInvoiceCreation", "ProcessInvoicesWaitingForAxxisUpdatedPrice", $"DDT Id:{item.Id} to Invoice conversion - {status.StatusMessage}");
                            LogManager.Logger.WriteException("DDTtoInvoiceCreation", "ProcessInvoicesWaitingForAxxisUpdatedPrice", $"DDT Id:{item.Id} to Invoice conversion - {status.StatusMessage}", new Exception());
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceDomain", "ProcessInvoicesWaitingForAxxisUpdatedPrice", ex.Message, ex);
                }
            }
            return generatedInvoices;
        }

        public async Task<int> ProcessInvoicesWaitingForOpisUpdatedPrice()
        {
            int generatedInvoices = 0;
            var ddtList = Context.DataContext.Invoices.Where(t => t.WaitingFor == (int)WaitingAction.UpdatedPrice
                                                                    && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                    && t.IsActive
                                                                    && t.Order.FuelRequest.FuelRequestPricingDetail != null)
                                                                    .Select(t => new
                                                                    {
                                                                        t.SupplierPreferredInvoiceTypeId,
                                                                        t.Id,
                                                                        t.CreatedBy,
                                                                        t.Order.AcceptedCompanyId,
                                                                        t.InvoiceHeaderId,
                                                                        CompanyName = t.User.Company.Name,
                                                                        RequestPriceDetailId = t.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                                                                        SplitLoadChainId = t.InvoiceXAdditionalDetail.SplitLoadChainId,
                                                                        TimeZoneName = t.Order.FuelRequest.Job.TimeZoneName
                                                                    })
                                                                    .ToList();
            var requestPriceDetailIds = await new PricingServiceDomain(this).GetPriceDetailIdsBySource(ddtList.Select(t => t.RequestPriceDetailId).ToList());
            ddtList = ddtList.Where(t => requestPriceDetailIds.Contains(t.RequestPriceDetailId)).ToList();

            foreach (var item in ddtList)
            {
                try
                {
                    if (item.SupplierPreferredInvoiceTypeId.HasValue && IsDigitalDropTicket(item.SupplierPreferredInvoiceTypeId.Value))
                    {
                        //only update ppg and waiting for status
                        var result = await UpdateDDTAfterUpdatedPrice(item.Id);
                        if (result.StatusCode == Status.Success)
                        {
                            generatedInvoices++;
                        }

                        LogManager.Logger.WriteDebug("DDTUpdatedWithPrice", "ProcessInvoicesWaitingForOpisUpdatedPrice", $"DDT Id:{item.Id}");
                        LogManager.Logger.WriteException("DDTUpdatedWithPrice", "ProcessInvoicesWaitingForOpisUpdatedPrice", $"DDT Id:{item.Id}", new Exception());
                    }
                    else
                    {
                        //generated invoice from ddt
                        var status = await CreateInvoiceFromDropTicket(new UserContext() { Id = item.CreatedBy, CompanyId = item.AcceptedCompanyId, CompanyName = item.CompanyName }, item.Id, item.InvoiceHeaderId, item.CreatedBy);
                        {
                            if (status.StatusCode == Status.Success)
                            {
                                generatedInvoices++;
                                if (!string.IsNullOrEmpty(item.SplitLoadChainId))
                                {
                                    await CheckForSplitLoadInvoiceAndGenerateStatement(item.SplitLoadChainId, item.AcceptedCompanyId, item.TimeZoneName);
                                }
                            }

                            LogManager.Logger.WriteDebug("DDTtoInvoiceCreation", "ProcessInvoicesWaitingForOpisUpdatedPrice", $"DDT Id:{item.Id} to Invoice conversion - {status.StatusMessage}");
                            LogManager.Logger.WriteException("DDTtoInvoiceCreation", "ProcessInvoicesWaitingForOpisUpdatedPrice", $"DDT Id:{item.Id} to Invoice conversion - {status.StatusMessage}", new Exception());
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceDomain", "ProcessInvoicesWaitingForOpisUpdatedPrice", ex.Message, ex);
                }
            }
            return generatedInvoices;
        }

        private async Task<StatusViewModel> UpdateDDTAfterUpdatedPrice(int ddtId)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("InvoiceDomain", "UpdateDDTAfterUpdatedPrice"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var ddtDetails = Context.DataContext.Invoices.SingleOrDefault(t => t.Id == ddtId);
                        if (ddtDetails != null && ddtDetails.WaitingFor == (int)WaitingAction.UpdatedPrice)
                        {
                            var viewModel = new InvoiceViewModel()
                            {
                                DropEndDate = ddtDetails.DropEndDate,
                                TerminalId = ddtDetails.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).Select(t => t.TerminalId).First(),
                                CityGroupTerminalId = ddtDetails.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).Select(t => t.CityGroupTerminalId).FirstOrDefault(),
                                Currency = ddtDetails.Currency,
                                WaitingForAction = ddtDetails.WaitingFor,
                                IsFTL = ddtDetails.Order.IsFTL
                            };
                            var pricePerGallon = await GetFuelPriceAsync(ddtDetails.Order, viewModel);
                            ddtDetails.BasicAmount = pricePerGallon * ddtDetails.DroppedGallons;
                            ddtDetails.WaitingFor = (int)WaitingAction.Nothing;
                            var bolInfo = ddtDetails.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault();
                            bolInfo.PricePerGallon = pricePerGallon;
                            Context.DataContext.Entry(ddtDetails).State = EntityState.Modified;
                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.msgSuccess;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageFaildToUpdateDDT;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("InvoiceDomain", "UpdateDDTAfterUpdatedPrice", ex.Message + " DDT Id: " + ddtId, ex);
                    }
                }
            }
            return response;
        }

        public async Task<int> ProcessInvoicesWaitingForTaxes()
        {
            int generatedInvoices = 0;

            var btaxServiceEnabled = GetTaxServiceEnabledFlag();
            if (!btaxServiceEnabled)
            {
                return generatedInvoices;
            }

            var ddtList = Context.DataContext.Invoices
                        .Where(t => t.WaitingFor == (int)WaitingAction.AvalaraTax
                                    && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                    && t.IsActive)
                        .Select(t => new
                        {
                            t.CreatedBy,
                            t.Order.AcceptedCompanyId,
                            t.User.Company.Name,
                            t.Id,
                            t.InvoiceHeaderId,
                            TerminalId = t.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).Select(t1 => t1.TerminalId).First(),
                            t.PoNumber,
                            t.DisplayInvoiceNumber
                        })
                        .ToList();

            foreach (var item in ddtList)
            {
                try
                {
                    if (item.TerminalId != null && item.TerminalId.Value > 0)
                    {
                        //generated invoice from ddt
                        var status = await CreateInvoiceFromDropTicketWaitingForTax(new UserContext() { Id = item.CreatedBy, CompanyId = item.AcceptedCompanyId, CompanyName = item.Name }, item.Id, item.InvoiceHeaderId, item.CreatedBy);
                        {
                            if (status.StatusCode == Status.Success)
                            {
                                generatedInvoices++;
                            }

                            LogManager.Logger.WriteDebug("DDTtoInvoiceCreation", "ProcessInvoicesWaitingForTaxes", $"DDT Id:{item.Id} to Invoice conversion - {status.StatusMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceDomain", "ProcessInvoicesWaitingForTaxes", ex.Message, ex);
                }
            }

            var invoicesWithoutTerminals = ddtList.Where(t => t.TerminalId == null || t.TerminalId <= 0).ToList();
            if (invoicesWithoutTerminals != null && invoicesWithoutTerminals.Any())
            {
                StringBuilder msg = new StringBuilder("Terminal not assigned to DDT/PO : ");
                foreach (var item in invoicesWithoutTerminals)
                {
                    msg.Append($" Id: {item.Id} & PoNumber: {item.PoNumber}, ");
                }

                LogManager.Logger.WriteException("InvoiceDomain", "ProcessInvoicesWaitingForTaxes", msg.ToString(), new Exception());
            }

            return generatedInvoices;
        }

        public async Task<StatusViewModel> EditInvoiceNumberAsync(UserContext userContext, int invoiceId, string displayInvoiceNumber)
        {
            var response = new StatusViewModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(displayInvoiceNumber))
                {
                    var isExist = await Context.DataContext.Invoices.AnyAsync(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.DisplayInvoiceNumber.Trim().ToLower() == displayInvoiceNumber.Trim().ToLower());
                    if (!isExist)
                    {
                        var invoice = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t => new
                        {
                            t.Id,
                            t.InvoiceHeader.InvoiceNumberId,
                            t.CreatedBy,
                            t.OrderId,
                            t.Order.OrderAdditionalDetail,
                            t.InvoiceVersionStatusId,
                            t.InvoiceHeaderId,
                            t.Order,
                            t.DisplayInvoiceNumber,
                            t.UpdatedBy
                        }).SingleOrDefaultAsync();

                        if (invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive)
                        {
                            response.EntityId = await GetLatestInvoiceId(invoice.InvoiceNumberId);
                            response.StatusMessage = Resource.errMessageOldInvoiceVersion;
                            return response;
                        }

                        var invoiceEditDomain = new InvoiceEditDomain(this);
                        var editStatus = await invoiceEditDomain.InvoiceEditForInvoiceNumberAsync(userContext, invoiceId, displayInvoiceNumber, invoice.OrderId);
                        if (editStatus.StatusCode == Status.Success && invoice.OrderAdditionalDetail != null && invoice.OrderAdditionalDetail.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails)
                        {
                            Invoice pdiInvoice = new Invoice { InvoiceHeaderId = invoice.InvoiceHeaderId, DisplayInvoiceNumber = invoice.DisplayInvoiceNumber, UpdatedBy = invoice.UpdatedBy };
                            Order pdiOrder = invoice.Order;
                            if (invoice != null && invoice.Order != null)
                            {
                                CreatePDIAPIWorkflow(pdiInvoice, pdiOrder);
                            }
                        }
                        if (editStatus.StatusCode == Status.Success)
                        {
                            Invoice sapInvoice = new Invoice { InvoiceHeaderId = invoice.InvoiceHeaderId, DisplayInvoiceNumber = invoice.DisplayInvoiceNumber, UpdatedBy = invoice.UpdatedBy };
                            
                            SAPEnterpriseDomain sapDomain = new SAPEnterpriseDomain(this);

                            if (invoice != null && invoice.Order != null)
                            {
                                sapDomain.CreateSAPWorkflow(sapInvoice);
                            }
                        }
                        response.EntityId = editStatus.EntityId;
                        response.StatusCode = editStatus.StatusCode;
                        response.StatusMessage = editStatus.StatusMessage;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = ResourceMessages.GetMessage(Resource.valMessageAlreadyExist, new object[] { Resource.lblInvoiceNumber });
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = ResourceMessages.GetMessage(Resource.valMessageInvalid, new object[] { Resource.lblInvoiceNumber });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "EditInvoiceNumberAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveInvoicePdfEmailNotificationDetails(EmailDocumentViewModel model, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (model.SelectedInvoices.Count == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageSelectInvoiceToSendEmail;
                        return response;
                    }

                    if (string.IsNullOrWhiteSpace(model.ToEmailAddress))
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageEmailToRequired;
                        return response;
                    }
                    else
                    {
                        model.ToEmailAddress = model.ToEmailAddress.Replace("[\"", "").Replace("\",\"", ";").Replace("\"]", "").Replace("[]", "");
                        if (string.IsNullOrWhiteSpace(model.ToEmailAddress))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errorMessageInvalidEmailAddress;
                            return response;
                        }
                    }

                    model.InvoiceIds = string.Join(",", model.SelectedInvoices);
                    var jsonMessage = JsonConvert.SerializeObject(model);
                    await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.PdfEmailAttachment, model.OrderId, userContext.Id, null, jsonMessage, 1, true);

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessagePdfEmailRequest;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessgeFailedToSubmitEmailRequest;

                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "SaveInvoicePdfEmailNotificationDetails", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> SaveEmailDocNotificationDetails(EmailDocumentViewModel model, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(model.ToEmailAddress))
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageEmailToRequired;
                        return response;
                    }
                    else
                    {
                        model.ToEmailAddress = model.ToEmailAddress.Replace("[\"", "").Replace("\",\"", ";").Replace("\"]", "").Replace("[]", "");
                        if (string.IsNullOrWhiteSpace(model.ToEmailAddress))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errorMessageInvalidEmailAddress;
                            return response;
                        }
                    }

                    if (model.InvoiceId <= 0 && model.DocumentName == DocumentName.Invoice)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessgeFailedToSubmitEmailRequest;
                        return response;
                    }

                    if (model.OrderId <= 0 && model.DocumentName == DocumentName.PO)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessgeFailedToSubmitEmailRequest;
                        return response;
                    }

                    if (model.InvoiceHeaderId <= 0 && model.DocumentName == DocumentName.BDR)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessgeFailedToSubmitEmailRequest;
                        return response;
                    }

                    if (model.InvoiceId <= 0 && model.DocumentName == DocumentName.InvoiceSummary)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessgeFailedToSubmitEmailRequest;
                        return response;
                    }
                    if (model.InvoiceHeaderId <= 0 && model.DocumentName == DocumentName.MarineTaxAffidavit)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessgeFailedToSubmitEmailRequest;
                        return response;
                    }
                    if (model.InvoiceHeaderId <= 0 && model.DocumentName == DocumentName.CGInspection)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessgeFailedToSubmitEmailRequest;
                        return response;
                    }
                    if (model.InvoiceHeaderId <= 0 && model.DocumentName == DocumentName.InspRequestVoucher)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessgeFailedToSubmitEmailRequest;
                        return response;
                    }

                    var jsonMessage = JsonConvert.SerializeObject(model);
                    if (model.InvoiceId > 0 && model.DocumentName == DocumentName.Invoice)
                        await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.PdfEmailAttachment, model.InvoiceId, userContext.Id, null, jsonMessage, 1, true);
                    else if (model.OrderId > 0 && model.DocumentName == DocumentName.PO)
                        await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.PdfEmailAttachment, model.OrderId, userContext.Id, null, jsonMessage);
                    else if (model.InvoiceHeaderId > 0 && model.DocumentName == DocumentName.BDR)
                        await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.PdfEmailAttachment, model.InvoiceHeaderId, userContext.Id, null, jsonMessage);
                    else if (model.InvoiceId > 0 && model.DocumentName == DocumentName.InvoiceSummary)
                        await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.PdfEmailAttachment, model.InvoiceId, userContext.Id, null, jsonMessage);
                    else if (model.InvoiceHeaderId > 0 && model.DocumentName == DocumentName.MarineTaxAffidavit)
                        await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.PdfEmailAttachment, model.InvoiceHeaderId, userContext.Id, null, jsonMessage);
                    else if (model.InvoiceHeaderId > 0 && model.DocumentName == DocumentName.CGInspection)
                        await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.PdfEmailAttachment, model.InvoiceHeaderId, userContext.Id, null, jsonMessage);
                    else if (model.InvoiceHeaderId > 0 && model.DocumentName == DocumentName.InspRequestVoucher)
                        await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.PdfEmailAttachment, model.InvoiceHeaderId, userContext.Id, null, jsonMessage);

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessagePdfEmailRequest;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessgeFailedToSubmitEmailRequest;

                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceDomain", "SaveEmailDocNotificationDetails", ex.Message, ex);
                }
            }

            return response;
        }
        #endregion

        #region Specific Invoice Generation
        private async Task<StatusViewModel> GenerateInvoiceForDryRun(InvoiceViewModel viewModel)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("InvoiceDomain", "GenerateInvoiceForDryRun"))
            {
                int originalInvoiceTypeId = viewModel.InvoiceTypeId;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var order = await Context.DataContext.Orders.Include(t => t.FuelRequest).SingleOrDefaultAsync(t => t.Id == viewModel.OrderId);
                        if (order != null)
                        {
                            Invoice invoice = await GenerateDryRunInvoiceEndSupplier(viewModel, order, originalInvoiceTypeId);

                            NotificationDomain notificationDomain = new NotificationDomain(this);
                            StoredProcedureDomain spDomain = new StoredProcedureDomain(this);

                            List<Order> ParentOrders = new List<Order>();
                            var parentFuelRequest = await spDomain.GetParentFuelRequest(order.FuelRequestId);
                            if (parentFuelRequest != null && (parentFuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest || parentFuelRequest.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest))
                            {
                                var helperDomain = new HelperDomain(this);
                                SetBrokeredChainId(viewModel);
                                invoice.BrokeredChainId = viewModel.BrokeredChainId;

                                var parentOrderIds = await spDomain.GetBrokerChainOrderListTillOriginalOrder(order.Id, viewModel.DivertedOrderIds, viewModel.DropEndDate);
                                ParentOrders = await Context.DataContext.Orders.Where(t => parentOrderIds.Contains(t.Id)).Select(t => t).ToListAsync();

                                Order[] ParentOrdersCopy = new Order[ParentOrders.Count];
                                ParentOrders.CopyTo(ParentOrdersCopy);
                                var originalParentOrders = ParentOrdersCopy.ToList();

                                foreach (var brokeredOrder in originalParentOrders)
                                {
                                    var basicAmount = helperDomain.GetDryRunFee(brokeredOrder.FuelRequest.FuelRequestFees, viewModel.DropStartDate);
                                    if (basicAmount > 0)
                                    {
                                        var brokeredInvoiceViewModel = new InvoiceViewModel
                                        {
                                            DropEndDate = viewModel.DropEndDate,
                                            DropStartDate = viewModel.DropEndDate,
                                            Id = viewModel.Id,
                                            UserId = brokeredOrder.AcceptedBy,
                                            OrderId = brokeredOrder.Id,
                                            PoNumber = brokeredOrder.PoNumber,
                                            InvoiceTypeId = (int)InvoiceType.DryRun,
                                            BrokeredChainId = viewModel.BrokeredChainId,
                                            BasicAmount = basicAmount,
                                            Currency = viewModel.Currency,
                                            UoM = viewModel.UoM
                                        };

                                        if (viewModel.CreationMethod == CreationMethod.APIUpload || viewModel.CreationMethod == CreationMethod.BulkUploaded)
                                        {
                                            if (brokeredOrder.FuelRequest.FuelRequestDetail.OrderEnforcementId != OrderEnforcement.EnforceOrderLevelValues)
                                            {
                                                brokeredInvoiceViewModel.BasicAmount = invoice.BasicAmount;
                                            }
                                        }
                                        var brokeredInvoice = await GenerateDryRunInvoiceForBrokeredSuppliers(brokeredInvoiceViewModel, brokeredOrder, originalInvoiceTypeId);
                                        await notificationDomain.AddNotificationEventAsync(EventType.DryRunInvoiceCreated, brokeredInvoice.InvoiceHeaderId, brokeredInvoiceViewModel.UserId);
                                    }
                                    else
                                    {
                                        ParentOrders.Remove(brokeredOrder);
                                    }
                                }
                            }
                            await Context.CommitAsync();
                            transaction.Commit();

                            await notificationDomain.AddNotificationEventAsync(EventType.DryRunInvoiceCreated, invoice.InvoiceHeaderId, viewModel.UserId);
                            ProcessOrderListForQbInvoiceAccountingWorkflow(invoice, order, ParentOrders);
                            //ProcessInvoicesForWebNotifications(invoice, order, ParentOrders);

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageDryRunInvoiceCreated;
                            response.EntityNumber = invoice.DisplayInvoiceNumber;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("InvoiceDomain", "GenerateInvoiceForDryRun", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        private async Task<Invoice> GenerateDryRunInvoiceForBrokeredSuppliers(InvoiceViewModel brokeredInvoiceViewModel, Order brokeredOrder, int originalInvoiceTypeId)
        {
            var brokeredInvoiceNumber = brokeredInvoiceViewModel.InvoiceNumber.ToEntity();
            Context.DataContext.InvoiceNumbers.Add(brokeredInvoiceNumber);
            await Context.CommitAsync();
            brokeredInvoiceViewModel.DisplayInvoiceNumber = brokeredInvoiceNumber.Number;
            brokeredInvoiceViewModel.InvoiceNumber = brokeredInvoiceNumber.ToViewModel();
            SetDryRunInvoiceStatus(brokeredInvoiceViewModel, brokeredOrder);
            await SetInvoiceAdditionDetails(brokeredInvoiceViewModel, 0, brokeredOrder.Id);

            var brokeredInvoice = brokeredInvoiceViewModel.ToEntity();
            SetDryRunInvoiceAmount(brokeredInvoiceViewModel, brokeredOrder, brokeredInvoice);
            SetDryRunInvoiceEntityDetails(brokeredInvoiceViewModel, brokeredInvoice, originalInvoiceTypeId, brokeredOrder, brokeredInvoiceNumber);

            //add dry run fee to FR/Job spent
            SetHedgeSpotAmounts(brokeredOrder, brokeredInvoice, false, false);

            var brokerInvoiceHeader = brokeredInvoiceViewModel.ToInvoiceHeaderEntity();
            Context.DataContext.InvoiceHeaderDetails.Add(brokerInvoiceHeader);
            brokerInvoiceHeader.Invoices.Add(brokeredInvoice);

            await Context.CommitAsync();
            return brokeredInvoice;
        }

        private async Task<Invoice> GenerateDryRunInvoiceEndSupplier(InvoiceViewModel viewModel, Order order, int originalInvoiceTypeId)
        {
            viewModel.UoM = order.FuelRequest.UoM;
            viewModel.Currency = order.FuelRequest.Currency;
            var invoiceNumber = viewModel.InvoiceNumber.ToEntity();
            Context.DataContext.InvoiceNumbers.Add(invoiceNumber);
            await Context.CommitAsync();
            if (string.IsNullOrWhiteSpace(viewModel.SupplierInvoiceNumber))
            {
                viewModel.DisplayInvoiceNumber = invoiceNumber.Number;
            }
            else
            {
                viewModel.DisplayInvoiceNumber = viewModel.SupplierInvoiceNumber;
                viewModel.ReferenceId = invoiceNumber.Number;
            }
            SetDryRunInvoiceStatus(viewModel, order);
            viewModel.InvoiceNumber.Id = invoiceNumber.Id;
            viewModel.CreatedBy = viewModel.UserId;
            viewModel.CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
            viewModel.UpdatedDate = viewModel.CreatedDate;

            await SetInvoiceAdditionDetails(viewModel, 0, order.Id);
            viewModel.AdditionalDetail.CreationMethod = viewModel.CreationMethod;

            var invoice = viewModel.ToEntity();
            SetDryRunInvoiceAmount(viewModel, order, invoice);
            SetDryRunInvoiceEntityDetails(viewModel, invoice, originalInvoiceTypeId, order, invoiceNumber);

            //add dry run fee to FR/Job spent
            SetHedgeSpotAmounts(order, invoice, false, false);

            var invoiceHeader = viewModel.ToInvoiceHeaderEntity();
            invoiceHeader.Invoices.Add(invoice);
            Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);

            await Context.CommitAsync();
            Context.DataContext.Entry(order).State = EntityState.Modified;
            return invoice;
        }

        private void SetDryRunInvoiceAmount(InvoiceViewModel viewModel, Order order, Invoice invoice)
        {
            if (!(viewModel.CreationMethod == CreationMethod.APIUpload || viewModel.CreationMethod == CreationMethod.BulkUploaded))
            {
                if (order.FuelRequest.FuelRequestFees.Any(t => t.FeeTypeId == (int)FeeType.DryRunFee))
                {
                    invoice.BasicAmount = new HelperDomain(this).GetDryRunFee(order.FuelRequest.FuelRequestFees, viewModel.DropStartDate);
                }
                if (invoice.BasicAmount == 0)
                {
                    invoice.BasicAmount = viewModel.BasicAmount;
                }
            }
        }

        private void SetDryRunInvoiceEntityDetails(InvoiceViewModel viewModel, Invoice invoice, int originalInvoiceTypeId, Order order, InvoiceNumber invoiceNumber)
        {
            invoice.TransactionId = invoiceNumber.Number;
            invoice.SupplierPreferredInvoiceTypeId = originalInvoiceTypeId;
            invoice.PaymentTermId = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentTermId;
            invoice.NetDays = invoice.PaymentTermId == (int)PaymentTerms.NetDays ? order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).NetDays : 0;
            var paymentDueDatetype = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == order.AcceptedCompanyId && t.IsActive)
                                    .Select(t => t.PaymentDueDateType).FirstOrDefault();
            invoice.PaymentDueDate = GetPaymentDueDate(invoice.PaymentTermId, invoice.NetDays, order.FuelRequest.Job.TimeZoneName, invoice.DropEndDate, paymentDueDatetype);

            //invoice.InvoiceHeader.InvoiceNumberId = invoiceNumber.Id;
            invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.Active;
            invoice.Version = Context.DataContext.Invoices.Count(t => t.InvoiceHeader.InvoiceNumberId == viewModel.InvoiceNumber.Id) + 1;
            invoice.CreatedBy = viewModel.UserId;
            invoice.UpdatedBy = viewModel.UserId;
            invoice.CreatedDate = DateTimeOffset.Now;
        }

        private void SetDryRunInvoiceStatus(InvoiceViewModel viewModel, Order order)
        {
            if (order.FuelRequest.Job.IsApprovalWorkflowEnabled)
            {
                viewModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
            }
            var isConnectedWithBuyer = false;

            if (order.FuelRequest.Job.IsApprovalWorkflowEnabled && order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
            {
                if (order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.FuelRequest1 != null)
                {
                    var brokerOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.OrderByDescending(t => t.Id).FirstOrDefault();
                    var parentOrder = GetConnectingBuyerOrder(brokerOrder);
                    if (parentOrder == null)
                    {
                        isConnectedWithBuyer = true;
                    }
                }
            }

            var previousStatus = order.Invoices.FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == viewModel.InvoiceNumber.Id && t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.WaitingForApproval));
            if (viewModel.StatusId == (int)InvoiceStatus.Received && ((order.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.Job.IsApprovalWorkflowEnabled) || (isConnectedWithBuyer)))
            {
                if (previousStatus == null)
                {
                    viewModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                }
                else
                {
                    var lastInvoice = order.Invoices.OrderByDescending(t => t.Id).FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == viewModel.InvoiceNumber.Id);
                    viewModel.StatusId = lastInvoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault().StatusId;
                    if (lastInvoice != null && lastInvoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault().StatusId == (int)InvoiceStatus.Rejected)
                    {
                        if (!lastInvoice.InvoiceXInvoiceStatusDetails.Any(t => t.StatusId == (int)InvoiceStatus.Received))
                        {
                            viewModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                        }
                        else
                        {
                            viewModel.StatusId = (int)InvoiceStatus.Received;
                        }
                    }
                }
            }
        }

        private List<Order> GetBrokerChainOrderListTillOriginalOrder(Order entity, List<Order> list)
        {
            if ((entity.FuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest || entity.FuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
                && entity.FuelRequest.FuelRequest1 != null && entity.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
            {
                bool isSingleDeliveryClosedOrder = false;
                if (entity.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery &&
                                    (
                                        entity.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyClosed
                                        || entity.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed
                                    ))
                {
                    isSingleDeliveryClosedOrder = true;
                }
                var childRequest = entity.FuelRequest.GetParentFuelRequest().FuelRequest1;
                var brokeredOrder = childRequest.Orders.LastOrDefault();
                brokeredOrder = (brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open || isSingleDeliveryClosedOrder) ? brokeredOrder : GetConnectingBuyerOrder(brokeredOrder);
                if (brokeredOrder != null && (brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open || isSingleDeliveryClosedOrder))
                {
                    list.Add(brokeredOrder);
                    return GetBrokerChainOrderListTillOriginalOrder(brokeredOrder, list);
                }
                else
                {
                    return list;
                }
            }
            else
            {
                return list;
            }
        }

        private async Task<StatusViewModel> GenerateInvoiceForAmpAsync(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("InvoiceDomain", "GenerateInvoiceForAmpAsync"))
            {
                var invoice = new Invoice();
                int originalInvoiceTypeId = invoiceViewModel.InvoiceTypeId;
                int originalWaitingForAction = invoiceViewModel.WaitingForAction;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);

                        bool autocloseOrder = false;
                        int oldTrackableScheduleId = 0;
                        decimal totalDelivered = 0;

                        List<AssetDropViewModel> assetDrops = null;
                        if (manualInvoiceModel.Assets != null)
                        {
                            assetDrops = manualInvoiceModel.Assets;
                        }

                        var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == invoiceViewModel.OrderId);
                        if (order != null)
                        {
                            manualInvoiceModel.TimeZoneName = order.FuelRequest.Job.TimeZoneName;
                            invoiceViewModel.TimeZoneName = order.FuelRequest.Job.TimeZoneName;
                            SetInvoiceDueDate(manualInvoiceModel, invoiceViewModel);

                            var currentOrderPricingTypeId = order.FuelRequest.PricingTypeId;
                            var notificationEvent = EventType.InvoiceCreated;
                            var invoiceNumber = await GetInvoiceNumberViewModel(invoiceViewModel, manualInvoiceModel);

                            invoiceViewModel.TerminalId = manualInvoiceModel.TerminalId;
                            invoiceViewModel.CityGroupTerminalId = manualInvoiceModel.CityGroupTerminalId;

                            await SetPPGAndBasicAmountForInvoice(invoiceViewModel, manualInvoiceModel, order);
                            SetTaxAmount(invoiceViewModel, manualInvoiceModel, order, originalInvoiceTypeId);

                            SetStatusForInvoice(invoiceViewModel, manualInvoiceModel, order);
                            await SetInvoiceAdditionDetails(invoiceViewModel, 0, order.Id);
                            InvoiceHeaderDetail invoiceHeader = GenerateInvoiceHeader(new List<InvoiceViewModel>() { invoiceViewModel });
                            invoice = invoiceViewModel.ToEntity();
                            SetInvoiceBolDetails(invoiceHeader, invoiceViewModel, invoice);
                            await SetInvoiceDetailsForInvoice(invoiceViewModel, manualInvoiceModel, invoice, originalInvoiceTypeId, invoiceNumber, order);

                            // update invoice number
                            UpdateInvoiceNumber(invoice, invoiceViewModel, manualInvoiceModel);

                            order.Invoices.Add(invoice);

                            assetDrops = SetAssetDropsForInvoice(invoiceViewModel, invoice, assetDrops, order);
                            autocloseOrder = AutoCloseOrder(order, out totalDelivered, invoiceViewModel.TrackableScheduleId);
                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            var delReqStatuses = UpdateInvoiceTrackableScheduleId(invoiceViewModel, invoice, oldTrackableScheduleId, order);
                            await AddNotificationEventForInvoice(invoiceViewModel, invoice, order, notificationEvent);
                            await SetAssetDropsForInvoice(invoiceViewModel, invoice);

                            // to ensure that this is not conversion from ddt to invoice
                            SetHedgeSpotAmounts(order, invoice, false, invoiceViewModel.IsRecursiveCallForBrokerOrders);

                            await AddOrderAutoCloseNewsFeedForInvoice(newsfeedDomain, autocloseOrder, totalDelivered, order);

                            var parentFuelRequest = order.FuelRequest.GetParentFuelRequest();
                            List<Order> ParentOrders = new List<Order>();
                            List<DeliveryReqStatusUpdateModel> brokerDrStatuses = null;
                            //FuelRequest.FuelRequest1 gives child FR details
                            if (parentFuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.FuelRequest1 != null && parentFuelRequest.FuelRequest1 != null)
                            {
                                SetBrokeredChainId(invoiceViewModel);
                                invoice.BrokeredChainId = invoiceViewModel.BrokeredChainId;

                                // End supplier trackable schedule Id to associate trackable schedule to broker chain orders
                                int endSupplierDeliveryScheduleId = 0;
                                DateTimeOffset endSupplierDeliveryScheduleDate = DateTimeOffset.Now;
                                var endSupplierDeliveryScheduleXTrackableSchedule = order.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.Id == invoiceViewModel.TrackableScheduleId);
                                if (endSupplierDeliveryScheduleXTrackableSchedule != null)
                                {
                                    endSupplierDeliveryScheduleId = endSupplierDeliveryScheduleXTrackableSchedule.DeliveryScheduleId;
                                    endSupplierDeliveryScheduleDate = endSupplierDeliveryScheduleXTrackableSchedule.Date;
                                }

                                GetBrokerChainOrderListTillOriginalOrder(order, ParentOrders);
                                foreach (var item in ParentOrders)
                                {
                                    var brokeredOriginalInvoiceTypeId = item.DefaultInvoiceType;
                                    var brokeredManaulInvoiceViewModel = manualInvoiceModel;
                                    var brokeredInvoiceViewModel = invoiceViewModel;
                                    SetPaymentTermsToViewmodel(item, brokeredManaulInvoiceViewModel);

                                    brokeredInvoiceViewModel.OrderId = item.Id;
                                    brokeredInvoiceViewModel.PoNumber = item.PoNumber;
                                    brokeredInvoiceViewModel.UserId = item.AcceptedBy;
                                    brokeredInvoiceViewModel.UpdatedBy = item.AcceptedBy;
                                    brokeredInvoiceViewModel.CreatedBy = item.AcceptedBy;
                                    brokeredInvoiceViewModel.IsRecursiveCallForBrokerOrders = true;

                                    if (currentOrderPricingTypeId != item.FuelRequest.PricingTypeId)
                                    {
                                        if (item.FuelRequest.PricingTypeId == (int)PricingType.PricePerGallon || item.FuelRequest.PricingTypeId == (int)PricingType.Suppliercost)
                                        {
                                            brokeredInvoiceViewModel.IsRecursiveCallForBrokerOrders = false;
                                        }
                                    }

                                    if (brokeredInvoiceViewModel.TrackableScheduleId != null)//updating deliveryschedule id for broker order
                                    {
                                        var trackableSchedule = item.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.DeliveryScheduleId == endSupplierDeliveryScheduleId && t.Date == endSupplierDeliveryScheduleDate && t.IsActive);
                                        brokeredInvoiceViewModel.TrackableScheduleId = trackableSchedule?.Id;
                                    }

                                    if ((brokeredInvoiceViewModel.InvoiceTypeId == (int)InvoiceType.MobileApp || brokeredInvoiceViewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) && (assetDrops == null || !assetDrops.Any()))
                                    {
                                        manualInvoiceModel.Assets = GetAssetDropDetails(invoice);
                                    }
                                    if (brokeredManaulInvoiceViewModel.Assets != null)
                                    {
                                        // Update Order-Id of assets in broker case
                                        brokeredManaulInvoiceViewModel.Assets.ForEach(t => t.OrderId = item.Id);
                                    }

                                    var fuleRequestOfBrokeredOrder = item.FuelRequest;
                                    brokeredManaulInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = GetFuelRequestFee(fuleRequestOfBrokeredOrder);
                                    brokeredManaulInvoiceViewModel.PaymentTermId = item.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentTermId;
                                    brokeredInvoiceViewModel.InvoiceTypeId = brokeredOriginalInvoiceTypeId;
                                    brokeredInvoiceViewModel.WaitingForAction = originalWaitingForAction;
                                    //generate invoice start
                                    oldTrackableScheduleId = 0;

                                    assetDrops = null;
                                    if (manualInvoiceModel.Assets != null)
                                    {
                                        assetDrops = manualInvoiceModel.Assets;
                                    }

                                    SetInvoiceDueDate(brokeredManaulInvoiceViewModel, brokeredInvoiceViewModel);

                                    currentOrderPricingTypeId = item.FuelRequest.PricingTypeId;
                                    var brokeredInvoiceNumber = await GetInvoiceNumberViewModel(invoiceViewModel, manualInvoiceModel);
                                    await SetPPGAndBasicAmountForInvoice(brokeredInvoiceViewModel, brokeredManaulInvoiceViewModel, item);
                                    SetTaxAmount(brokeredInvoiceViewModel, brokeredManaulInvoiceViewModel, order, brokeredOriginalInvoiceTypeId);

                                    SetStatusForInvoice(brokeredInvoiceViewModel, brokeredManaulInvoiceViewModel, item);
                                    await SetInvoiceAdditionDetails(brokeredInvoiceViewModel, 0, item.Id);
                                    InvoiceHeaderDetail brokerInvoiceHeader = GenerateInvoiceHeader(new List<InvoiceViewModel>() { brokeredInvoiceViewModel });
                                    var brokeredInvoice = brokeredInvoiceViewModel.ToEntity();
                                    SetInvoiceBolDetails(brokerInvoiceHeader, invoiceViewModel, brokeredInvoice);
                                    brokeredManaulInvoiceViewModel.SetFuelFeesForBrokerOrderToManualInvoiceViewModel(item);
                                    await SetInvoiceDetailsForInvoice(brokeredInvoiceViewModel, brokeredManaulInvoiceViewModel, brokeredInvoice, brokeredOriginalInvoiceTypeId, brokeredInvoiceNumber, item);

                                    // update invoice number
                                    UpdateInvoiceNumber(brokeredInvoice, brokeredInvoiceViewModel, brokeredManaulInvoiceViewModel);

                                    item.Invoices.Add(brokeredInvoice);
                                    await Context.CommitAsync();

                                    assetDrops = SetAssetDropsForInvoice(brokeredInvoiceViewModel, brokeredInvoice, assetDrops, item);
                                    autocloseOrder = AutoCloseOrder(item, out totalDelivered, brokeredInvoiceViewModel.TrackableScheduleId);
                                    Context.DataContext.Entry(item).State = EntityState.Modified;
                                    brokerDrStatuses = UpdateInvoiceTrackableScheduleId(brokeredInvoiceViewModel, brokeredInvoice, oldTrackableScheduleId, item);

                                    await AddNotificationEventForInvoice(brokeredInvoiceViewModel, brokeredInvoice, item, notificationEvent);
                                    await SetAssetDropsForInvoice(brokeredInvoiceViewModel, brokeredInvoice);

                                    // to ensure that this is not conversion from ddt to invoice
                                    SetHedgeSpotAmounts(item, brokeredInvoice, false, brokeredInvoiceViewModel.IsRecursiveCallForBrokerOrders);

                                    await AddOrderAutoCloseNewsFeedForInvoice(newsfeedDomain, autocloseOrder, totalDelivered, item);
                                }
                            }

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            if (invoiceViewModel.IsTaxServiceFailure && originalInvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual)
                            {
                                response.StatusMessage = Resource.successMessageDDTCreatedDueToAvaTaxFailure;
                            }
                            else
                            {
                                var isDigitalDropTicket = IsDigitalDropTicket(invoice.InvoiceTypeId);
                                if (isDigitalDropTicket && invoiceViewModel.IsApprovalWorkflowEnabledForJob)
                                {
                                    response.StatusMessage = Resource.SuccessMessgeDDTCreatedAsApprovalWorkflowIsEnabled;
                                }
                                else if (isDigitalDropTicket && invoiceViewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice)
                                {
                                    response.StatusMessage = Resource.errMessageDDTCreatedWaitingForUpdatedPrice;
                                }
                                else
                                {
                                    response.StatusMessage = isDigitalDropTicket ? Resource.errMessageDropTicketCreateSuccess : Resource.errMessageInvoiceCreateSuccess;
                                }
                            }

                            ProcessOrderListForQbInvoiceAccountingWorkflow(invoice, order, ParentOrders);
                            if (delReqStatuses != null && delReqStatuses.Any())
                            {
                                if (brokerDrStatuses != null && brokerDrStatuses.Any())
                                {
                                    delReqStatuses.AddRange(brokerDrStatuses);
                                }
                                new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatuses);
                            }
                            //ProcessInvoicesForWebNotifications(invoice, order, ParentOrders);
                        }

                        invoiceViewModel = invoice.ToViewModel(invoiceViewModel);
                        manualInvoiceModel.InvoiceId = invoice.Id;
                        manualInvoiceModel.BuyerCompanyId = invoice.Order.FuelRequest.Job.CompanyId;
                        manualInvoiceModel.InvoiceNumber.Number = invoice.DisplayInvoiceNumber;
                        manualInvoiceModel.JobId = invoice.Order.FuelRequest.JobId;
                        manualInvoiceModel.InvoiceTypeId = invoiceViewModel.InvoiceTypeId;
                        manualInvoiceModel.OrderId = invoiceViewModel.OrderId ?? 0;

                        if (invoiceViewModel.StatusId != (int)InvoiceStatus.Draft)
                        {
                            await SetApprovalWorkflowEnabledNewsFeeds(invoice, newsfeedDomain, order);
                        }
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("InvoiceDomain", "GenerateInvoiceForAmpAsync", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        //private void ProcessInvoicesForWebNotifications(Invoice invoice, Order order, List<Order> ParentOrders)
        //{
        //    if (!IsDigitalDropTicket(invoice.InvoiceTypeId))
        //    {
        //        var queueDomain = new QueueMessageDomain();
        //        var queueRequest = GetEnqueueMessageRequestViewModel(invoice, order);
        //        var queueId = queueDomain.EnqeueMessage(queueRequest);
        //        if (ParentOrders != null)
        //        {
        //            var parentOrder = ParentOrders.FirstOrDefault();
        //            for (int index = 0; index < ParentOrders.Count; index++)
        //            {
        //                var currentOrder = ParentOrders[index];
        //                var currentInvoice = currentOrder.Invoices.Last();
        //                parentOrder = index == (ParentOrders.Count - 1) ? null : ParentOrders[index + 1];
        //                queueRequest = GetEnqueueMessageRequestViewModel(currentInvoice, currentOrder);
        //                queueId = queueDomain.EnqeueMessage(queueRequest);
        //            }
        //        }
        //    }
        //}

        //private QueueMessageViewModel GetEnqueueMessageRequestViewModel(Invoice invoiceObj, Order order)
        //{
        //    var jsonViewModel = new NotificationInvoiceQueMsg();
        //    jsonViewModel.CreatedByCompanyId = order.AcceptedCompanyId;
        //    jsonViewModel.InvoiceId = invoiceObj.Id;
        //    jsonViewModel.InvoiceNumber = invoiceObj.DisplayInvoiceNumber;
        //    jsonViewModel.OrderNumber = order.PoNumber;
        //    jsonViewModel.CreatedByCompanyName = order.Company.Name;
        //    //we are not using this name anywhere..so for now assigning from order as Invoice.user is NULL in dry run and ddt to invoice conversion
        //    jsonViewModel.CreatedByUserName = order.User.FirstName;

        //    string json = JsonConvert.SerializeObject(jsonViewModel);

        //    return new QueueMessageViewModel()
        //    {
        //        CreatedBy = invoiceObj.CreatedBy,
        //        QueueProcessType = QueueProcessType.InvoiceCreated,
        //        JsonMessage = json
        //    };
        //}

        private void ProcessOrderListForQbInvoiceAccountingWorkflow(Invoice invoice, Order order, List<Order> ParentOrders)
        {
            var parentOrder = ParentOrders.FirstOrDefault();
            CreateQbAccountingWorkflowForInvoice(false, invoice, order, parentOrder?.Id);
            CreateQbAccountingWorkflowForBill(false, invoice, order, parentOrder?.Id);
            CreateDtnFileGenerationWorkflow(invoice, order);
            for (int index = 0; index < ParentOrders.Count; index++)
            {
                var currentOrder = ParentOrders[index];
                var currentInvoice = currentOrder.Invoices.Last();
                parentOrder = (index == (ParentOrders.Count - 1) ? null : ParentOrders[index + 1]);
                CreateQbAccountingWorkflowForInvoice(false, currentInvoice, currentOrder, parentOrder?.Id);
                CreateQbAccountingWorkflowForBill(false, currentInvoice, currentOrder, parentOrder?.Id);
                CreateDtnFileGenerationWorkflow(currentInvoice, currentOrder);
            }
        }

        //private async Task<StatusViewModel> GenerateInvoiceForMobileAsync(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel, InvoiceCreationFrom invoiceCreationFrom)
        //{
        //    var response = new StatusViewModel();
        //    using (var tracer = new Tracer("InvoiceDomain", "GenerateInvoiceForMobileAsync"))
        //    {
        //        var invoice = new Invoice();
        //        int originalInvoiceTypeId = invoiceViewModel.InvoiceTypeId;
        //        int originalWaitingForAction = invoiceViewModel.WaitingForAction;
        //        using (var transaction = Context.DataContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
        //                bool autocloseOrder = false;
        //                int oldTrackableScheduleId = 0;
        //                decimal totalDelivered = 0;

        //                List<AssetDropViewModel> assetDrops = null;
        //                if (manualInvoiceModel.Assets != null)
        //                {
        //                    assetDrops = manualInvoiceModel.Assets;
        //                }

        //                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == invoiceViewModel.OrderId);
        //                if (order != null)
        //                {
        //                    var timeZoneName = order.FuelRequest.Job.TimeZoneName;
        //                    manualInvoiceModel.TimeZoneName = timeZoneName;
        //                    invoiceViewModel.TimeZoneName = timeZoneName;
        //                    SetInvoiceDueDate(manualInvoiceModel, invoiceViewModel);

        //                    var currentOrderPricingTypeId = order.FuelRequest.PricingTypeId;
        //                    var notificationEvent = order.FuelRequest.Job != null && order.FuelRequest.Job.IsApprovalWorkflowEnabled ? EventType.InvoiceCreatedApprovalWorkflow : EventType.InvoiceCreatedViaMobileDrop;
        //                    invoiceViewModel.TerminalId = manualInvoiceModel.TerminalId;
        //                    invoiceViewModel.CityGroupTerminalId = manualInvoiceModel.CityGroupTerminalId;

        //                    var invoiceNumber = await GetInvoiceNumberViewModel(invoiceViewModel, manualInvoiceModel);
        //                    await SetPPGAndBasicAmountForInvoice(invoiceViewModel, manualInvoiceModel, order);

        //                    manualInvoiceModel.GetOrderTaxesForNonStandardFuel(order);
        //                    SetTaxAmount(invoiceViewModel, manualInvoiceModel, order, originalInvoiceTypeId);

        //                    SetStatusForInvoice(invoiceViewModel, manualInvoiceModel, order);
        //                    await SetInvoiceAdditionDetails(invoiceViewModel, 0, order.Id);
        //                    invoice = invoiceViewModel.ToEntity();
        //                    RemoveDryRunFees(manualInvoiceModel);
        //                    await SetInvoiceDetailsForInvoice(invoiceViewModel, manualInvoiceModel, invoice, originalInvoiceTypeId, invoiceNumber, order);

        //                    // update invoice number
        //                    UpdateInvoiceNumber(invoice, invoiceViewModel, manualInvoiceModel);

        //                    order.Invoices.Add(invoice);
        //                    await Context.CommitAsync();

        //                    await SetAssetDropsForInvoice(invoiceViewModel, invoice);
        //                    assetDrops = SetAssetDropsForInvoice(invoiceViewModel, invoice, assetDrops, order);
        //                    autocloseOrder = AutoCloseOrder(order, out totalDelivered);
        //                    Context.DataContext.Entry(order).State = EntityState.Modified;
        //                    UpdateInvoiceTrackableScheduleId(invoiceViewModel, invoice, oldTrackableScheduleId, order);

        //                    await AddNotificationEventForInvoice(invoiceViewModel, invoice, order, notificationEvent);

        //                    // to ensure that this is not conversion from ddt to invoice
        //                    SetHedgeSpotAmounts(order, invoice, false, invoiceViewModel.IsRecursiveCallForBrokerOrders);
        //                    await Context.CommitAsync();

        //                    await AddOrderAutoCloseNewsFeedForInvoice(newsfeedDomain, autocloseOrder, totalDelivered, order);

        //                    var parentFuelRequest = order.FuelRequest.GetParentFuelRequest();
        //                    List<Order> ParentOrders = new List<Order>();

        //                    //FuelRequest.FuelRequest1 gives child FR details
        //                    if (parentFuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest
        //                                    && order.FuelRequest.FuelRequest1 != null && parentFuelRequest.FuelRequest1 != null)
        //                    {
        //                        SetBrokeredChainId(invoiceViewModel);
        //                        invoice.BrokeredChainId = invoiceViewModel.BrokeredChainId;
        //                        SetEndSupplierDeliveryScheduleIdForBrokerChain(invoiceViewModel, order, out int endSupplierDeliveryScheduleId, out DateTimeOffset endSupplierDeliveryScheduleDate);

        //                        //no need to consider close or open order, as from mobile we can drop only for open orders.
        //                        GetBrokerChainOrderListTillOriginalOrder(order, ParentOrders);

        //                        foreach (var item in ParentOrders)
        //                        {
        //                            var brokeredOriginalInvoiceTypeId = item.DefaultInvoiceType == (int)InvoiceType.Manual ? (int)InvoiceType.MobileApp : (int)InvoiceType.DigitalDropTicketMobileApp;
        //                            var brokeredManaulInvoiceViewModel = manualInvoiceModel;
        //                            var brokeredInvoiceViewModel = invoiceViewModel;
        //                            SetBrokeredInvoiceViewModel(currentOrderPricingTypeId, endSupplierDeliveryScheduleId, endSupplierDeliveryScheduleDate, item, brokeredInvoiceViewModel);
        //                            SetPaymentTermsToViewmodel(item, brokeredManaulInvoiceViewModel);

        //                            if ((brokeredInvoiceViewModel.InvoiceTypeId == (int)InvoiceType.MobileApp || brokeredInvoiceViewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp) && (assetDrops == null || !assetDrops.Any()))
        //                            {
        //                                manualInvoiceModel.Assets = GetAssetDropDetails(invoice);
        //                            }
        //                            if (brokeredManaulInvoiceViewModel.Assets != null)
        //                            {
        //                                // Update Order-Id of assets in broker case
        //                                brokeredManaulInvoiceViewModel.Assets.ForEach(t => t.OrderId = item.Id);
        //                            }

        //                            SetBrokeredManualInvoiceViewModel(brokeredOriginalInvoiceTypeId, originalWaitingForAction, item, brokeredManaulInvoiceViewModel, brokeredInvoiceViewModel);
        //                            //generate invoice start
        //                            oldTrackableScheduleId = 0;

        //                            assetDrops = null;
        //                            if (manualInvoiceModel.Assets != null)
        //                            {
        //                                assetDrops = manualInvoiceModel.Assets;
        //                            }

        //                            SetInvoiceDueDate(brokeredManaulInvoiceViewModel, brokeredInvoiceViewModel);

        //                            currentOrderPricingTypeId = item.FuelRequest.PricingTypeId;
        //                            notificationEvent = item.FuelRequest.Job != null && item.FuelRequest.Job.IsApprovalWorkflowEnabled ? EventType.InvoiceCreatedApprovalWorkflow : EventType.InvoiceCreatedViaMobileDrop;

        //                            var brokeredInvoiceNumber = await GetInvoiceNumberViewModel(invoiceViewModel, manualInvoiceModel);
        //                            await SetPPGAndBasicAmountForInvoice(brokeredInvoiceViewModel, brokeredManaulInvoiceViewModel, item);

        //                            brokeredManaulInvoiceViewModel.GetOrderTaxesForNonStandardFuel(item);
        //                            SetTaxAmount(brokeredInvoiceViewModel, brokeredManaulInvoiceViewModel, item, brokeredOriginalInvoiceTypeId);

        //                            SetStatusForInvoice(brokeredInvoiceViewModel, brokeredManaulInvoiceViewModel, item);
        //                            await SetInvoiceAdditionDetails(brokeredInvoiceViewModel, 0, item.Id);
        //                            var brokeredInvoice = brokeredInvoiceViewModel.ToEntity();
        //                            brokeredManaulInvoiceViewModel.SetFuelFeesForBrokerOrderToManualInvoiceViewModel(item);
        //                            await SetInvoiceDetailsForInvoice(brokeredInvoiceViewModel, brokeredManaulInvoiceViewModel, brokeredInvoice, brokeredOriginalInvoiceTypeId, brokeredInvoiceNumber, item);

        //                            // update brokered invoice number
        //                            UpdateInvoiceNumber(brokeredInvoice, brokeredInvoiceViewModel, brokeredManaulInvoiceViewModel);

        //                            item.Invoices.Add(brokeredInvoice);
        //                            await Context.CommitAsync();

        //                            assetDrops = SetAssetDropsForInvoice(brokeredInvoiceViewModel, brokeredInvoice, assetDrops, item);
        //                            autocloseOrder = AutoCloseOrder(item, out totalDelivered);
        //                            Context.DataContext.Entry(item).State = EntityState.Modified;
        //                            UpdateInvoiceTrackableScheduleId(brokeredInvoiceViewModel, brokeredInvoice, oldTrackableScheduleId, item);

        //                            await AddNotificationEventForInvoice(brokeredInvoiceViewModel, brokeredInvoice, item, notificationEvent);
        //                            await SetAssetDropsForInvoice(brokeredInvoiceViewModel, brokeredInvoice);

        //                            // to ensure that this is not conversion from ddt to invoice
        //                            SetHedgeSpotAmounts(item, brokeredInvoice, false, brokeredInvoiceViewModel.IsRecursiveCallForBrokerOrders);

        //                            await AddOrderAutoCloseNewsFeedForInvoice(newsfeedDomain, autocloseOrder, totalDelivered, item);
        //                        }
        //                    }

        //                    await Context.CommitAsync();
        //                    transaction.Commit();

        //                    response.StatusCode = Status.Success;
        //                    if (invoiceViewModel.IsTaxServiceFailure && originalInvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual)
        //                    {
        //                        response.StatusMessage = Resource.successMessageDDTCreatedDueToAvaTaxFailure;
        //                    }
        //                    else
        //                    {
        //                        if (invoiceViewModel.IsApprovalWorkflowEnabledForJob && (originalInvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual || originalInvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp))
        //                        {
        //                            response.StatusMessage = Resource.SuccessMessgeDDTCreatedAsApprovalWorkflowIsEnabled;
        //                        }
        //                        else if (invoiceViewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice)
        //                        {
        //                            response.StatusMessage = Resource.errMessageDDTCreatedWaitingForUpdatedPrice;
        //                        }
        //                        else
        //                        {
        //                            var isDigitalDropTicket = IsDigitalDropTicket(invoice.InvoiceTypeId);
        //                            response.StatusMessage = isDigitalDropTicket ? Resource.errMessageDropTicketCreateSuccess : Resource.errMessageInvoiceCreateSuccess;
        //                        }
        //                    }

        //                    ProcessOrderListForQbInvoiceAccountingWorkflow(invoice, order, ParentOrders);
        //                    ProcessInvoicesForWebNotifications(invoice, order, ParentOrders);
        //                }

        //                invoiceViewModel = SetViewModelBackFromNewInvoice(invoiceViewModel, manualInvoiceModel, invoice);

        //                await SetApprovalWorkflowEnabledNewsFeeds(invoice, newsfeedDomain, order);
        //            }
        //            catch (Exception ex)
        //            {
        //                response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
        //                transaction.Rollback();
        //                LogManager.Logger.WriteException("InvoiceDomain", "GenerateInvoiceForMobileAsync", ex.Message, ex);
        //            }
        //        }
        //    }
        //    return response;
        //}

        //private static InvoiceViewModel SetViewModelBackFromNewInvoice(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel, Invoice invoice)
        //{
        //    invoiceViewModel = invoice.ToViewModel(invoiceViewModel);
        //    manualInvoiceModel.InvoiceId = invoice.Id;
        //    manualInvoiceModel.BuyerCompanyId = invoice.Order.FuelRequest.Job.CompanyId;
        //    manualInvoiceModel.InvoiceNumber.Number = invoice.DisplayInvoiceNumber;
        //    manualInvoiceModel.JobId = invoice.Order.FuelRequest.JobId;
        //    manualInvoiceModel.InvoiceTypeId = invoiceViewModel.InvoiceTypeId;
        //    manualInvoiceModel.OrderId = invoiceViewModel.OrderId ?? 0;
        //    return invoiceViewModel;
        //}

        //private void SetBrokeredManualInvoiceViewModel(int originalInvoiceTypeId, int originalWaitingForAction, Order item, ManualInvoiceViewModel brokeredManaulInvoiceViewModel, InvoiceViewModel brokeredInvoiceViewModel)
        //{
        //    var fuleRequestOfBrokeredOrder = item.FuelRequest;
        //    brokeredManaulInvoiceViewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = GetFuelRequestFee(fuleRequestOfBrokeredOrder);
        //    brokeredManaulInvoiceViewModel.PaymentTermId = item.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentTermId;
        //    brokeredInvoiceViewModel.InvoiceTypeId = originalInvoiceTypeId;
        //    brokeredInvoiceViewModel.WaitingForAction = originalWaitingForAction;
        //}

        //private static void SetBrokeredInvoiceViewModel(int currentOrderPricingTypeId, int endSupplierDeliveryScheduleId, DateTimeOffset endSupplierDeliveryScheduleDate, Order item, InvoiceViewModel brokeredInvoiceViewModel)
        //{
        //    brokeredInvoiceViewModel.OrderId = item.Id;
        //    brokeredInvoiceViewModel.PoNumber = item.PoNumber;
        //    brokeredInvoiceViewModel.UserId = item.AcceptedBy;
        //    brokeredInvoiceViewModel.UpdatedBy = item.AcceptedBy;
        //    brokeredInvoiceViewModel.CreatedBy = item.AcceptedBy;
        //    brokeredInvoiceViewModel.IsRecursiveCallForBrokerOrders = true;

        //    if (currentOrderPricingTypeId != item.FuelRequest.PricingTypeId)
        //    {
        //        if (item.FuelRequest.PricingTypeId == (int)PricingType.PricePerGallon || item.FuelRequest.PricingTypeId == (int)PricingType.Suppliercost)
        //        {
        //            brokeredInvoiceViewModel.IsRecursiveCallForBrokerOrders = false;
        //        }
        //    }

        //    if (brokeredInvoiceViewModel.TrackableScheduleId != null)//updating deliveryschedule id for broker order
        //    {
        //        var trackableSchedule = item.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.DeliveryScheduleId == endSupplierDeliveryScheduleId && t.Date == endSupplierDeliveryScheduleDate && t.IsActive);
        //        brokeredInvoiceViewModel.TrackableScheduleId = trackableSchedule?.Id;
        //    }
        //}

        //private static void SetEndSupplierDeliveryScheduleIdForBrokerChain(InvoiceViewModel invoiceViewModel, Order order, out int endSupplierDeliveryScheduleId, out DateTimeOffset endSupplierDeliveryScheduleDate)
        //{
        //    // End supplier trackable schedule Id to associate trackable schedule to broker chain orders
        //    endSupplierDeliveryScheduleId = 0;
        //    endSupplierDeliveryScheduleDate = DateTimeOffset.Now;
        //    var endSupplierDeliveryScheduleXTrackableSchedule = order.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.Id == invoiceViewModel.TrackableScheduleId);
        //    if (endSupplierDeliveryScheduleXTrackableSchedule != null)
        //    {
        //        endSupplierDeliveryScheduleId = endSupplierDeliveryScheduleXTrackableSchedule.DeliveryScheduleId;
        //        endSupplierDeliveryScheduleDate = endSupplierDeliveryScheduleXTrackableSchedule.Date;
        //    }
        //}

        private async Task SetApprovalWorkflowEnabledNewsFeeds(Invoice invoice, NewsfeedDomain newsfeedDomain, Order order)
        {
            if (invoice.Order.FuelRequest.Job.IsApprovalWorkflowEnabled)
            {
                var invoicePendingUser = invoice.Order.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t => t.IsActive);
                if (invoicePendingUser != null && invoicePendingUser.UserId > 0 && invoicePendingUser.User.OnboardedTypeId != (int)OnboardedType.ThirdPartyOrderOnboarded
                    && (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
                {
                    var childRequest = order.FuelRequest.GetParentFuelRequest().FuelRequest1;
                    var brokeredOrder = childRequest != null ? childRequest.Orders.LastOrDefault() : null;
                    if (brokeredOrder == null)
                    {
                        if (invoice.SupplierPreferredInvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.SupplierPreferredInvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                        {
                            await newsfeedDomain.SetSupplierCreatedDDTWaitingForApprovalNewsfeed(invoice, invoicePendingUser.User, NewsfeedEvent.SupplierCreatedDDTWaitingForApprovalSettingDDT);
                        }
                        else
                        {
                            await newsfeedDomain.SetSupplierCreatedDDTWaitingForApprovalNewsfeed(invoice, invoicePendingUser.User, NewsfeedEvent.SupplierCreatedDDTWaitingForApprovalSettingInvoice);
                        }
                    }
                    else
                    {
                        await newsfeedDomain.SetSupplierInvoiceDDTCreatedNewsfeed(invoice, invoicePendingUser.User);
                    }
                }
            }
        }

        private static async Task AddOrderAutoCloseNewsFeedForInvoice(NewsfeedDomain newsfeedDomain, bool autocloseOrder, decimal totalDelivered, Order order)
        {
            var deliveryTypeId = order.FuelRequest.FuelRequestDetail.DeliveryTypeId;
            if (order.FuelRequest.MaxQuantity > 0 && autocloseOrder && deliveryTypeId != (int)DeliveryType.OneTimeDelivery)
            {
                await newsfeedDomain.SetSystemOrderAutoClosedNewsfeed(order, totalDelivered);
            }
        }

        private async Task SetAssetDropsForInvoice(InvoiceViewModel invoiceViewModel, Invoice invoice)
        {
            if ((invoiceViewModel.InvoiceTypeId == (int)InvoiceType.MobileApp
                                            || invoiceViewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                                            && invoiceViewModel.OrderId != null)
            {
                var assetDrops = Context.DataContext.AssetDrops.Where(t => t.OrderId == invoiceViewModel.OrderId.Value && t.InvoiceId == null);
                if (assetDrops.Count() > 0)
                {
                    foreach (var assetDrop in assetDrops)
                    {
                        assetDrop.InvoiceId = invoice.Id;
                        Context.DataContext.Entry(assetDrop).State = EntityState.Modified;
                    }
                    await Context.CommitAsync();
                }
            }
        }

        private async Task<EventType> AddNotificationEventForInvoice(InvoiceViewModel invoiceViewModel, Invoice invoice, Order order, EventType notificationEvent)
        {
            if (notificationEvent != EventType.InvoiceTaxValuesChanged)//adding this as logic is not implemented for this event
            {
                var userId = invoiceViewModel.InvoiceTypeId == (int)InvoiceType.MobileApp ? order.AcceptedBy : invoiceViewModel.UserId;
                if (invoice.WaitingFor == (int)WaitingAction.UpdatedPrice)
                {
                    notificationEvent = EventType.DdtCreatedAsInvoiceIsWaitingForUpdatedPrice;
                }
                else if (invoice.WaitingFor == (int)WaitingAction.AvalaraTax)
                {
                    notificationEvent = EventType.DDTCreateAsInvoiceWaitingForTaxes;
                }
                await ContextFactory.Current.GetDomain<NotificationDomain>()
                      .AddNotificationEventAsync(notificationEvent, invoice.InvoiceHeaderId, userId);
            }
            return notificationEvent;
        }

        private List<AssetDropViewModel> SetAssetDropsForInvoice(InvoiceViewModel invoiceViewModel, Invoice invoice, List<AssetDropViewModel> assetDrops, Order order)
        {
            var job = order.FuelRequest.Job;
            if (job.JobBudget.IsAssetTracked && assetDrops != null && assetDrops.Count > 0)
            {
                InvoiceCommonDomain invoiceCommonDomain = new InvoiceCommonDomain(this);
                assetDrops = invoiceCommonDomain.SetJobAssetId(assetDrops, invoiceViewModel.UserId, job);
                SetAssetDropsToInvoice(invoice, assetDrops);
            }

            //need to get fees after adding asset drops
            invoice.TotalFeeAmount = new HelperDomain(this).SetCalculatedInvoiceFeesTotal(invoice);

            return assetDrops;
        }

        private async Task SetInvoiceDetailsForInvoice(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel, Invoice invoice, int originalInvoiceTypeId, InvoiceNumber invoiceNumber, Order order)
        {
            invoice.SupplierPreferredInvoiceTypeId = originalInvoiceTypeId;
            if (invoiceViewModel.Image != null && invoiceViewModel.Image.IsRemoved)
            {
                var image = await Context.DataContext.Images.FirstOrDefaultAsync(t => t.Id == invoiceViewModel.Image.Id);
                if (image != null)
                {
                    invoice.Image = null;
                    Context.DataContext.Images.Remove(image);
                }
            }

            if (invoiceViewModel.AdditionalDetail != null)
            {
                invoice.InvoiceXAdditionalDetail = invoiceViewModel.AdditionalDetail.ToEntity();
            }

            if (invoiceViewModel.SpecialInstructions != null && invoiceViewModel.SpecialInstructions.Count > 0)
            {
                invoice.InvoiceXSpecialInstructions = invoiceViewModel.SpecialInstructions.Select(t => t.ToEntity()).ToList();
            }

            if (!manualInvoiceModel.IsInvoiceEdit)
            {
                CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
                invoiceViewModel.ExchangeRate = currencyRateDomain.GetCurrencyRate(order.FuelRequest.Currency, Currency.USD, DateTimeOffset.Now);
                invoice.ExchangeRate = invoiceViewModel.ExchangeRate;
            }
            SetInvoiceCurrencyAndUoM(manualInvoiceModel, invoice, order);
            SetInvoiceBaseAmounts(invoiceViewModel, invoice);

            //FuelRequestFee Entity
            if (manualInvoiceModel.ExternalBrokerId <= 0 && !manualInvoiceModel.IsThirdPartyHardwareUsed)
            {
                var authenticationDomain = new AuthenticationDomain(this);
                var currentUser = await authenticationDomain.GetUserContextAsync(invoiceViewModel.UserId, CompanyType.Supplier);
                FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
                await fuelFeesDomain.SaveFuelFees(invoiceViewModel, manualInvoiceModel, invoice, currentUser);
            }
            else
            {
                invoice.FuelRequestFees = manualInvoiceModel.ExternalBrokeredOrder.BrokeredOrderFee.ToEntity();
            }

            //Tax details Entity
            invoice.TaxDetails = invoiceViewModel.TaxDetails.ToEntity();

            if (manualInvoiceModel.PaymentTermId == (int)PaymentTerms.NetDays)
            {
                invoice.NetDays = manualInvoiceModel.NetDays;
            }
            invoice.PaymentTermId = manualInvoiceModel.PaymentTermId;
            invoice.InvoiceHeader.InvoiceNumberId = invoiceNumber.Id;
            invoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.Active;
            invoice.Version = Context.DataContext.Invoices.Count(t => t.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id) + 1;
            invoice.CreatedBy = invoiceViewModel.UserId;
            invoice.CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(manualInvoiceModel.TimeZoneName);
            invoice.UpdatedDate = invoice.CreatedDate;
        }

        private static void SetInvoiceCurrencyAndUoM(ManualInvoiceViewModel manualInvoiceModel, Invoice invoice, Order order)
        {
            invoice.UoM = order.FuelRequest.UoM;
            invoice.Currency = order.FuelRequest.Currency;
            foreach (var fee in manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees)
            {
                fee.UoM = order.FuelRequest.UoM;
                fee.Currency = order.FuelRequest.Currency;
            }
        }

        private static void SetInvoiceBaseAmounts(InvoiceViewModel viewModel, Invoice invoice)
        {
            invoice.BaseDroppedQuntity = VolumeConverter.GetBaseQuantity(invoice.UoM, invoice.DroppedGallons);
            invoice.BasePrice = MoneyConverter.GetBaseAmount(invoice.Currency, viewModel.PricePerGallon, viewModel.ExchangeRate);
            invoice.BaseStateTax = MoneyConverter.GetBaseAmount(invoice.Currency, invoice.StateTax, viewModel.ExchangeRate);
            invoice.BaseFedTax = MoneyConverter.GetBaseAmount(invoice.Currency, invoice.FedTax, viewModel.ExchangeRate);
            invoice.BaseSalesTax = MoneyConverter.GetBaseAmount(invoice.Currency, invoice.SalesTax, viewModel.ExchangeRate);
            invoice.BaseBasicAmount = MoneyConverter.GetBaseAmount(invoice.Currency, invoice.BasicAmount, viewModel.ExchangeRate);
            invoice.BaseTotalTaxAmount = MoneyConverter.GetBaseAmount(invoice.Currency, invoice.TotalTaxAmount, viewModel.ExchangeRate);
            invoice.BaseRackPrice = MoneyConverter.GetBaseAmount(invoice.Currency, invoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail.RackPrice).FirstOrDefault(), viewModel.ExchangeRate);
            if (invoice.TotalFeeAmount.HasValue)
            {
                invoice.BaseTotalFeeAmount = MoneyConverter.GetBaseAmount(invoice.Currency, invoice.TotalFeeAmount.Value, viewModel.ExchangeRate);
            }
        }

        private void SetStatusForInvoice(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel, Order order)
        {
            var isConnectedWithBuyer = false;
            if (order.FuelRequest.Job.IsApprovalWorkflowEnabled && order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
            {
                if (order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.FuelRequest1 != null)
                {
                    var brokerOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.OrderByDescending(t => t.Id).FirstOrDefault();
                    var parentOrder = GetConnectingBuyerOrder(brokerOrder);
                    if (parentOrder == null)
                    {
                        isConnectedWithBuyer = true;
                    }
                }
            }
            var previousStatus = order.Invoices.FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id && t.InvoiceXInvoiceStatusDetails.Any(t1 => t1.StatusId == (int)InvoiceStatus.WaitingForApproval));
            if (invoiceViewModel.StatusId == (int)InvoiceStatus.Received && ((order.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.Job.IsApprovalWorkflowEnabled) || (isConnectedWithBuyer)))
            {
                if (previousStatus == null)
                {
                    invoiceViewModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                }
                else
                {
                    var lastInvoice = order.Invoices.OrderByDescending(t => t.Id).FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == manualInvoiceModel.InvoiceNumber.Id);
                    invoiceViewModel.StatusId = lastInvoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault().StatusId;
                    if (lastInvoice != null && lastInvoice.InvoiceXInvoiceStatusDetails.OrderByDescending(t => t.Id).FirstOrDefault().StatusId == (int)InvoiceStatus.Rejected)
                    {
                        if (!lastInvoice.InvoiceXInvoiceStatusDetails.Any(t => t.StatusId == (int)InvoiceStatus.Received))
                        {
                            invoiceViewModel.StatusId = (int)InvoiceStatus.WaitingForApproval;
                        }
                        else
                        {
                            invoiceViewModel.StatusId = (int)InvoiceStatus.Received;
                        }
                    }
                }

                if (!manualInvoiceModel.IsInvoiceFromDropTicket && order.FuelRequest.GetParentFuelRequest().FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                {
                    var childRequest = order.FuelRequest.GetParentFuelRequest().FuelRequest1;
                    var brokeredOrder = childRequest.Orders.LastOrDefault();

                    if (brokeredOrder != null && order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery &&
                        (
                            brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyClosed
                            || brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed
                        ))
                    {
                        invoiceViewModel.StatusId = (int)InvoiceStatus.Received;
                    }
                }
            }
        }

        private async Task SetPPGAndBasicAmountForInvoice(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel, Order order)
        {
            invoiceViewModel.TerminalPricingDate = invoiceViewModel.DropEndDate;
            manualInvoiceModel.TerminalPricingDate = invoiceViewModel.DropEndDate;

            CheckWorkflowAndSetInvoiceCreationType(order, invoiceViewModel, manualInvoiceModel, 0);
            if (!invoiceViewModel.IsApprovalWorkflowEnabledForJob)
            {
                if (order.ExternalBrokerBuySellDetail == null)
                {
                    await SetInvoiceAmounts(order, invoiceViewModel);
                }
                else
                {
                    await SetInvoiceAmountsForBuySellOrder(order, invoiceViewModel, invoiceViewModel.IsBuyPriceInvoice);
                }
                if (invoiceViewModel.WaitingForAction != (int)WaitingAction.Nothing)
                {
                    SetInvoiceCreationTypeToDdt(invoiceViewModel, manualInvoiceModel);
                }
            }
        }

        private async Task SetPPGAndBasicAmountForBuyPriceInvoice(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel, Order order)
        {
            invoiceViewModel.TerminalPricingDate = invoiceViewModel.DropEndDate;
            manualInvoiceModel.TerminalPricingDate = invoiceViewModel.DropEndDate;

            CheckWorkflowAndSetInvoiceCreationType(order, invoiceViewModel, manualInvoiceModel, 0);
            if (!invoiceViewModel.IsApprovalWorkflowEnabledForJob)
            {
                await SetInvoiceAmountsForBuySellOrder(order, invoiceViewModel, true);
                if (invoiceViewModel.WaitingForAction != (int)WaitingAction.Nothing)
                {
                    SetInvoiceCreationTypeToDdt(invoiceViewModel, manualInvoiceModel);
                }
            }
        }

        private async Task SetPPGAndBasicAmountForSellPriceInvoice(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel, Order order)
        {
            invoiceViewModel.TerminalPricingDate = invoiceViewModel.DropEndDate;
            manualInvoiceModel.TerminalPricingDate = invoiceViewModel.DropEndDate;

            CheckWorkflowAndSetInvoiceCreationType(order, invoiceViewModel, manualInvoiceModel, 0);
            if (!invoiceViewModel.IsApprovalWorkflowEnabledForJob)
            {
                await SetInvoiceAmountsForBuySellOrder(order, invoiceViewModel, false);
                if (invoiceViewModel.WaitingForAction != (int)WaitingAction.Nothing)
                {
                    SetInvoiceCreationTypeToDdt(invoiceViewModel, manualInvoiceModel);
                }
            }
        }

        private void SetTaxAmount(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel, Order order, int originalInvoiceTypeId)
        {
            if (invoiceViewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice)
            {
                invoiceViewModel.PricePerGallon = 0;
                invoiceViewModel.BasicAmount = 0;
            }

            if (manualInvoiceModel.TypeofFuel == (int)ProductDisplayGroups.OtherFuelType)
            {
                SetTaxAmountForNonStandardProduct(invoiceViewModel, manualInvoiceModel, order);
            }
            else if (invoiceViewModel.WaitingForAction == (int)WaitingAction.Nothing && !invoiceViewModel.IsApprovalWorkflowEnabledForJob && !IsDigitalDropTicket(invoiceViewModel.InvoiceTypeId))
            {
                SetTaxAmountForStandardProduct(invoiceViewModel, manualInvoiceModel, order, false, originalInvoiceTypeId);
            }
        }

        private async Task<InvoiceNumber> GetInvoiceNumberViewModel(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel)
        {
            var invoiceNumber = manualInvoiceModel.InvoiceNumber.ToEntity();
            Context.DataContext.InvoiceNumbers.Add(invoiceNumber);
            await Context.CommitAsync();

            invoiceViewModel.TransactionId = invoiceNumber.Number;
            invoiceViewModel.InvoiceNumber = invoiceNumber.ToViewModel();
            invoiceViewModel.DisplayInvoiceNumber = invoiceNumber.Number;
            manualInvoiceModel.InvoiceNumber = invoiceViewModel.InvoiceNumber;
            manualInvoiceModel.DisplayInvoiceNumber = invoiceNumber.Number;
            return invoiceNumber;
        }

        private async Task<StatusViewModel> GenerateInvoiceForSellPrice(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel, bool IsBuyPriceInvoiceRequired)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("InvoiceDomain", "GenerateInvoiceForBuySellOrder"))
            {
                var sellPriceInvoice = new Invoice();
                int originalInvoiceTypeId = invoiceViewModel.InvoiceTypeId;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                        bool isInvoiceEdit = false, autocloseOrder = false;
                        int oldTrackableScheduleId = 0, previousInvoiceStatusId = 0;
                        decimal totalDelivered = 0;

                        List<AssetDropViewModel> assetDrops = null;
                        List<DeliveryReqStatusUpdateModel> drStatuses = null;
                        if (manualInvoiceModel.Assets != null)
                        {
                            assetDrops = manualInvoiceModel.Assets;
                        }

                        var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == invoiceViewModel.OrderId);
                        if (order != null)
                        {
                            manualInvoiceModel.TimeZoneName = order.FuelRequest.Job.TimeZoneName;
                            invoiceViewModel.TimeZoneName = order.FuelRequest.Job.TimeZoneName;
                            SetInvoiceDueDate(manualInvoiceModel, invoiceViewModel);

                            var notificationEvent = EventType.InvoiceCreated;
                            var invoiceNumber = await GetInvoiceNumberViewModel(invoiceViewModel, manualInvoiceModel);

                            invoiceViewModel.TerminalId = manualInvoiceModel.TerminalId;
                            invoiceViewModel.CityGroupTerminalId = manualInvoiceModel.CityGroupTerminalId;
                            await SetPPGAndBasicAmountForSellPriceInvoice(invoiceViewModel, manualInvoiceModel, order);
                            SetTaxAmount(invoiceViewModel, manualInvoiceModel, order, originalInvoiceTypeId);

                            SetStatusForInvoice(invoiceViewModel, manualInvoiceModel, order);
                            await SetInvoiceAdditionDetails(invoiceViewModel, 0, order.Id, true);
                            InvoiceHeaderDetail invoiceHeader = GenerateInvoiceHeader(new List<InvoiceViewModel>() { invoiceViewModel });
                            sellPriceInvoice = invoiceViewModel.ToEntity();
                            SetInvoiceBolDetails(invoiceHeader, invoiceViewModel, sellPriceInvoice);
                            await SetInvoiceDetailsForInvoice(invoiceViewModel, manualInvoiceModel, sellPriceInvoice, originalInvoiceTypeId, invoiceNumber, order);

                            // update invoice number
                            UpdateInvoiceNumber(sellPriceInvoice, invoiceViewModel, manualInvoiceModel);

                            order.Invoices.Add(sellPriceInvoice);
                            await Context.CommitAsync();

                            SetAssetDropsForInvoice(invoiceViewModel, sellPriceInvoice, assetDrops, order);
                            autocloseOrder = AutoCloseOrder(order, out totalDelivered, invoiceViewModel.TrackableScheduleId);
                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            var drStatus = UpdateInvoiceTrackableScheduleId(invoiceViewModel, sellPriceInvoice, oldTrackableScheduleId, order);
                            await AddNotificationEventForInvoice(invoiceViewModel, sellPriceInvoice, order, notificationEvent);
                            await SetAssetDropsForInvoice(invoiceViewModel, sellPriceInvoice);

                            // to ensure that this is not conversion from ddt to invoice
                            SetHedgeSpotAmounts(order, sellPriceInvoice, false, invoiceViewModel.IsRecursiveCallForBrokerOrders);

                            await AddOrderAutoCloseNewsFeedForInvoice(newsfeedDomain, autocloseOrder, totalDelivered, order);
                            await Context.CommitAsync();

                            if (IsBuyPriceInvoiceRequired)
                            {
                                drStatuses = await GenerateInvoiceForBuyPrice(invoiceViewModel, manualInvoiceModel, order);
                            }

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            if (invoiceViewModel.IsTaxServiceFailure && originalInvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual)
                            {
                                response.StatusMessage = Resource.successMessageDDTCreatedDueToAvaTaxFailure;
                            }
                            else
                            {
                                var isDigitalDropTicket = IsDigitalDropTicket(sellPriceInvoice.InvoiceTypeId);
                                if (isDigitalDropTicket && invoiceViewModel.IsApprovalWorkflowEnabledForJob)
                                {
                                    response.StatusMessage = Resource.SuccessMessgeDDTCreatedAsApprovalWorkflowIsEnabled;
                                }
                                else if (isDigitalDropTicket && invoiceViewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice)
                                {
                                    response.StatusMessage = Resource.errMessageDDTCreatedWaitingForUpdatedPrice;
                                }
                                else
                                {
                                    response.StatusMessage = isDigitalDropTicket ? Resource.errMessageDropTicketCreateSuccess : Resource.errMessageInvoiceCreateSuccess;
                                }
                            }
                            SAPEnterpriseDomain sapDomain = new SAPEnterpriseDomain(this);
                            sapDomain.CreateSAPWorkflow(sellPriceInvoice);
                            CreatePDIAPIWorkflow(sellPriceInvoice, order);
                            CreateQbAccountingWorkflowForInvoice(false, sellPriceInvoice, order, null);
                            CreateQbAccountingWorkflowForBill(false, sellPriceInvoice, order, null);
                            bool isDtnUploaded = CreateDtnFileGenerationWorkflow(sellPriceInvoice, order);
                            if (drStatus != null && drStatus.Any() || (drStatuses != null && drStatuses.Any()))
                            {
                                drStatus.AddRange(drStatuses);
                                new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(drStatus);
                            }
                            UpdateInvoiceActionResponseStatus(isDtnUploaded, response);
                        }

                        invoiceViewModel = sellPriceInvoice.ToViewModel(invoiceViewModel);
                        manualInvoiceModel.InvoiceId = sellPriceInvoice.Id;
                        manualInvoiceModel.BuyerCompanyId = sellPriceInvoice.Order.FuelRequest.Job.CompanyId;
                        manualInvoiceModel.InvoiceNumber.Number = sellPriceInvoice.DisplayInvoiceNumber;
                        manualInvoiceModel.JobId = sellPriceInvoice.Order.FuelRequest.JobId;
                        manualInvoiceModel.InvoiceTypeId = invoiceViewModel.InvoiceTypeId;
                        manualInvoiceModel.OrderId = invoiceViewModel.OrderId ?? 0;

                        if (!isInvoiceEdit && invoiceViewModel.StatusId != (int)InvoiceStatus.Draft)
                        {
                            await SetApprovalWorkflowEnabledNewsFeeds(sellPriceInvoice, newsfeedDomain, order);
                        }
                        else
                        {
                            if (previousInvoiceStatusId == (int)InvoiceStatus.Draft && invoiceViewModel.StatusId != (int)InvoiceStatus.Draft)
                            {
                                await newsfeedDomain.SetDigitalDropTicketDraftConvertedNewsfeed(order, invoiceViewModel);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("InvoiceDomain", "GenerateInvoiceForBuySellOrder", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        private async Task<List<DeliveryReqStatusUpdateModel>> GenerateInvoiceForBuyPrice(InvoiceViewModel invoiceViewModel, ManualInvoiceViewModel manualInvoiceModel, Order order)
        {
            List<DeliveryReqStatusUpdateModel> response = new List<DeliveryReqStatusUpdateModel>();
            using (var tracer = new Tracer("InvoiceDomain", "GenerateInvoiceForBuyPrice"))
            {
                int originalInvoiceTypeId = invoiceViewModel.InvoiceTypeId;
                int oldTrackableScheduleId = 0;

                List<AssetDropViewModel> assetDrops = null;
                if (manualInvoiceModel.Assets != null)
                {
                    assetDrops = manualInvoiceModel.Assets;
                }

                if (order != null)
                {
                    SetInvoiceDueDate(manualInvoiceModel, invoiceViewModel);
                    manualInvoiceModel.InvoiceNumber.Id = 0;
                    manualInvoiceModel.InvoiceNumber.Number = string.Empty;
                    invoiceViewModel.Id = 0;

                    var buyPriceInvoiceNumber = await GetInvoiceNumberViewModel(invoiceViewModel, manualInvoiceModel);

                    invoiceViewModel.TerminalId = manualInvoiceModel.TerminalId;
                    invoiceViewModel.CityGroupTerminalId = manualInvoiceModel.CityGroupTerminalId;
                    await SetPPGAndBasicAmountForBuyPriceInvoice(invoiceViewModel, manualInvoiceModel, order);
                    await SetInvoiceAdditionDetails(invoiceViewModel, 0, order.Id);
                    InvoiceHeaderDetail invoiceHeader = GenerateInvoiceHeader(new List<InvoiceViewModel>() { invoiceViewModel });
                    var buyPriceInvoice = invoiceViewModel.ToEntity();
                    SetInvoiceBolDetails(invoiceHeader, invoiceViewModel, buyPriceInvoice);
                    buyPriceInvoice.IsBuyPriceInvoice = true;
                    await SetInvoiceDetailsForInvoice(invoiceViewModel, manualInvoiceModel, buyPriceInvoice, originalInvoiceTypeId, buyPriceInvoiceNumber, order);

                    // update invoice number
                    UpdateInvoiceNumber(buyPriceInvoice, invoiceViewModel, manualInvoiceModel);

                    order.Invoices.Add(buyPriceInvoice);
                    SetAssetDropsForInvoice(invoiceViewModel, buyPriceInvoice, assetDrops, order);
                    Context.DataContext.Entry(order).State = EntityState.Modified;
                    response = UpdateInvoiceTrackableScheduleId(invoiceViewModel, buyPriceInvoice, oldTrackableScheduleId, order);
                    await SetAssetDropsForInvoice(invoiceViewModel, buyPriceInvoice);
                    await Context.CommitAsync();
                }
            }
            return response;
        }

        #endregion

        #region Private Methods

        private static void SetBrokeredChainId(InvoiceViewModel viewModel)
        {
            if (viewModel.BrokeredChainId == null)
            {
                viewModel.BrokeredChainId = DateTimeOffset.Now.ToString("yyyyMMddHHmmssFFFFFF") + viewModel.CreatedBy;
            }
        }

        private static void SetPaymentTermsToViewmodel(Order order, ManualInvoiceViewModel manaulInvoiceViewModel)
        {
            if (order != null)
            {
                manaulInvoiceViewModel.NetDays = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).NetDays;
                manaulInvoiceViewModel.PaymentTermId = order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PaymentTermId;
            }
        }
        #endregion

        private async Task ApproveBrokeredInvoice(UserContext userContext, Invoice updatedInvoice)
        {
            try
            {
                if ((updatedInvoice.TransactionId != "0" && !string.IsNullOrWhiteSpace(updatedInvoice.TransactionId)) ||
                               !string.IsNullOrWhiteSpace(updatedInvoice.BrokeredChainId))
                {
                    var brokeredDropTicket = await Context.DataContext.Invoices.Where(t => (t.TransactionId.Equals(updatedInvoice.TransactionId) || (t.BrokeredChainId != null && t.BrokeredChainId.Equals(updatedInvoice.BrokeredChainId))) && t.Id < updatedInvoice.Id && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).OrderByDescending(t => t.Id).FirstOrDefaultAsync();
                    if (brokeredDropTicket != null && (brokeredDropTicket.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || brokeredDropTicket.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
                    {
                        var user = brokeredDropTicket.Order.FuelRequest.User;
                        userContext = new UserContext()
                        {
                            CompanyId = user.CompanyId ?? brokeredDropTicket.Order.BuyerCompanyId,
                            CompanyName = user.Company.Name,
                            Email = user.Email,
                            Id = user.Id,
                            Name = $"{user.FirstName} {user.LastName}"
                        };

                        await ApproveInvoiceAsync(userContext, brokeredDropTicket.Id, true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "ApproveBrokeredInvoice", ex.Message, ex);
            }
        }

        private void RemoveDryRunFees(ManualInvoiceViewModel manualInvoiceModel)
        {
            if (manualInvoiceModel.FuelDeliveryDetails != null && manualInvoiceModel.FuelDeliveryDetails.FuelFees != null)
            {
                manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => !string.IsNullOrWhiteSpace(t.FeeTypeId) && t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
            }
        }



        private string GetFuelDeliveredPercentagePerInvoice(UspGetSupplierInvoiceDetails invoice)
        {
            string response = Resource.lblHyphen;
            if (invoice.OrderId != null && invoice.QuantityTypeId != (int)QuantityType.NotSpecified)
            {
                decimal deliveredPercentage = 0;
                deliveredPercentage = (invoice.DroppedGallons / (invoice.MaxQuantity == 0 ? 1 : invoice.BrokeredMaxQuantity ?? invoice.MaxQuantity.Value)) * 100;
                response = deliveredPercentage.GetPreciseValue(2) + Resource.constSymbolPercent;
            }
            return response;
        }

        private bool IsHidePricingEnabled(UspGetSupplierInvoiceDetails invoice, CompanyType companyType)
        {
            var response = false;
            if (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                if (companyType == CompanyType.Supplier)
                {
                    response = invoice.IsHidePricingEnabledForSupplier ?? false;
                }
                else
                {
                    response = invoice.IsHidePricingEnabledForBuyer ?? false;
                }
            }
            return response;
        }

        private string GetDeliverySchedule(UspGetSupplierInvoiceDetails invoice)
        {
            string deliverySchedule = string.Empty;
            string deliveryLevelPO = string.Empty;
            if (invoice.TrackableScheduleId != null)
            {
                var deliveryScheduleInfo = Context.DataContext.Invoices.Where(x => x.InvoiceHeaderId == invoice.InvoiceHeaderId && x.TrackableSchedule != null).Select(x => new { x.TrackableSchedule.Date, x.TrackableSchedule.StartTime, x.TrackableSchedule.EndTime, x.TrackableSchedule.Quantity, x.TrackableSchedule.UoM, x.TrackableSchedule.DeliveryLevelPO }).ToList();
                if (deliveryScheduleInfo.Count() > 1)
                {
                    foreach (var item in deliveryScheduleInfo)
                    {
                        deliverySchedule = deliverySchedule + $"{item.Date.Date.ToShortDateString()}   { Convert.ToDateTime(item.StartTime.ToString()).ToShortTimeString()} - {Convert.ToDateTime(item.EndTime.ToString()).ToShortTimeString()}   { item.Quantity.GetPreciseValue(2)} {item.UoM}" + "</br>";
                        deliveryLevelPO = deliveryLevelPO + item.DeliveryLevelPO + "</br>";
                    }
                    invoice.DeliveryLevelPO = deliveryLevelPO;
                }
                else
                {
                    deliverySchedule = $"{invoice.ScheduleDate.Value.Date.ToShortDateString()}   { Convert.ToDateTime(invoice.StartTime.ToString()).ToShortTimeString()} - {Convert.ToDateTime(invoice.EndTime.ToString()).ToShortTimeString()}   { invoice.Quantity.Value.GetPreciseValue(2)} {invoice.UoM} ";
                }
            }
            return deliverySchedule;
        }

        private async Task<int> GetLatestInvoiceId(int invoiceNumberId)
        {
            int invoiceId = await Context.DataContext.Invoices.Where(t => t.InvoiceHeader.InvoiceNumberId == invoiceNumberId
                            && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                            .Select(t => t.Id).OrderBy(t => t).FirstOrDefaultAsync();
            return invoiceId;
        }


        private async Task<List<GetInvoiceDetailsModel>> GetInvoiceDetails(int invoiceId)
        {
            return await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).SelectMany(t => t.InvoiceHeader.Invoices).Select(t => new
                GetInvoiceDetailsModel()
            {
                Invoice = t,
                PreferredInvoiceTypeId = t.Order.DefaultInvoiceType,
                InvoiceXAdditionalDetail = t.InvoiceXAdditionalDetail,
                PaymentTermId = t.PaymentTermId,
                NetDays = t.NetDays,
                BuyerCompanyId = t.Order.BuyerCompanyId,
                CustomerCompanyName = t.Order.BuyerCompany.Name,
                IsAssetTracked = t.Order.FuelRequest.Job.JobBudget.IsAssetTracked,
                IsDipDataRequired = t.Order.FuelRequest.FuelRequestDetail.IsPrePostDipRequired,
                Job = new JobLocationViewModel()
                {
                    JobId = t.Order.FuelRequest.JobId,
                    SiteName = t.Order.FuelRequest.Job.Name,
                    Address = t.Order.FuelRequest.Job.Address,
                    City = t.Order.FuelRequest.Job.City,
                    StateCode = t.Order.FuelRequest.Job.MstState.Code,
                    ZipCode = t.Order.FuelRequest.Job.ZipCode,
                    CountryId = t.Order.FuelRequest.Job.CountryId,
                    IsMarineLocation = t.Order.FuelRequest.Job.IsMarine
                },
                BolDetails = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail).ToList(),
                OrderId = t.OrderId ?? 0,
                OrderDetailVersion = t.Order.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive),
                FuelTypeId = t.Order.FuelRequest.FuelTypeId,
                FuelTypeName = t.Order.FuelRequest.MstProduct.DisplayName ?? t.Order.FuelRequest.MstProduct.Name,
                DroppedGallons = t.DroppedGallons,
                DropStartDate = t.DropStartDate,
                DropEndDate = t.DropEndDate,
                FuelRequest = t.Order.FuelRequest,
                TrackableScheduleId = t.TrackableScheduleId,
                Driver = t.DriverId == null ? null : new UserViewModel()
                {
                    Id = t.Driver.Id,
                    FirstName = t.Driver.FirstName,
                    LastName = t.Driver.LastName,
                    Email = "",
                    PhoneNumber = "",
                    CompanyName = ""
                },
                IsFtl = t.Order.IsFTL,
                InvoiceTypeId = t.InvoiceTypeId,
                IsDriverToUpdateBOL = (t.Order.OrderAdditionalDetail != null && t.Order.OrderAdditionalDetail.IsDriverToUpdateBOL),
                DropAddress = t.InvoiceDispatchLocation.FirstOrDefault(),
                BrokeredChainId = t.BrokeredChainId,
                TerminalId = t.Order.TerminalId ?? t.Order.FuelRequest.TerminalId,
                TerminalName = t.Order.MstExternalTerminal.Name ?? t.Order.FuelRequest.MstExternalTerminal.Name,
                TypeofFuel = t.Order.FuelRequest.MstProduct.ProductTypeId,
                ProductTypeId = t.Order.FuelRequest.MstProduct.ProductTypeId,
                FuelRequestFees = t.FuelRequestFees.ToList(),
                Customer = new UserViewModel()
                {
                    Id = t.Order.FuelRequest.User.Id,
                    FirstName = t.Order.FuelRequest.User.FirstName,
                    LastName = t.Order.FuelRequest.User.LastName,
                    Email = t.Order.FuelRequest.User.Email,
                    PhoneNumber = t.Order.FuelRequest.User.PhoneNumber,
                    CompanyName = t.Order.FuelRequest.User.Company.Name
                },
                InvoiceImage = t.Image,
                AdditionalImage = t.InvoiceXAdditionalDetail.AdditionalImage,
                Signature = t.Signaure,
                TaxAffidavitImage = t.InvoiceXAdditionalDetail.TaxAffidavitImage,
                BDNImage = t.InvoiceXAdditionalDetail.BDNImage,
                CoastGuardInspectionImage = t.InvoiceXAdditionalDetail.CoastGuardInspectionImage,
                InspectionVoucherImage = t.InvoiceXAdditionalDetail.InspectionRequestVoucherImage,
                TaxDetails = t.TaxDetails.ToList(),
                BDRDetail = t.BDRDetail,
                IsVariousOrigin = t.Order.FuelRequest.FreightOnBoardTypeId == (int)FreightOnBoardTypes.Terminal && t.Order.FuelRequest.Job.LocationType == JobLocationTypes.Various
            }).ToListAsync();
        }

        private void SetDropDetails(InvoiceViewModelNew invoiceModel, List<GetInvoiceDetailsModel> invoices)
        {
            var drops = invoices.GroupBy(t => t.FuelTypeId).ToList();
            foreach (var drop in drops)
            {
                var fuel = drop.FirstOrDefault();
                InvoiceDropViewModel dropViewModel = new InvoiceDropViewModel()
                {
                    InvoiceId = fuel.Invoice.Id,
                    IsBolDetailsRequired = fuel.IsDriverToUpdateBOL,
                    IsDipDataRequired = fuel.IsDipDataRequired,
                    OrderId = fuel.OrderId,
                    PoNumber = fuel.OrderDetailVersion.PoNumber,
                    TypeOfFuel = fuel.TypeofFuel,
                    FuelTypeId = fuel.FuelTypeId,
                    FuelTypeName = fuel.FuelTypeName,
                    ActualDropQuantity = drop.Sum(t => t.DroppedGallons),
                    DropDate = fuel.DropEndDate.Date,
                    DisplayDropDate = fuel.DropEndDate.ToString(Resource.constFormatDate),
                    StartTime = fuel.DropStartDate.DateTime.ToString(Resource.constFormat12HourTime2),
                    EndTime = fuel.DropEndDate.DateTime.ToString(Resource.constFormat12HourTime2),
                    TrackableScheduleId = fuel.TrackableScheduleId,
                    TerminalId = fuel.TerminalId,
                    TerminalName = fuel.TerminalName,
                    IsBOLImageRequired = fuel.Invoice.IsBolImageReq,
                    IsSignatureRequired = fuel.Invoice.IsSignatureReq,
                    IsDropImageRequired = fuel.Invoice.IsDropImageReq,
                    IsFTL = fuel.IsFtl,
                    Allowance = fuel.InvoiceXAdditionalDetail.SupplierAllowance,
                    BrokerChainId = fuel.BrokeredChainId,
                    IsAssetTracked = fuel.IsAssetTracked,
                    UoM = fuel.Invoice.UoM,
                    Currency = fuel.Invoice.Currency,
                    QuantityIndicatorTypeId = fuel.Invoice.QuantityIndicatorTypeId ?? (int)QuantityIndicatorTypes.Net,
                    Gravity = fuel.Invoice.Gravity,
                    ConvertedQuantity = fuel.Invoice.ConvertedQuantity.HasValue && fuel.Invoice.ConvertedQuantity.Value > 0 ? Math.Round(fuel.Invoice.ConvertedQuantity.Value, 2) : fuel.Invoice.ConvertedQuantity,
                    JobCountryId = fuel.FuelRequest.Job.CountryId,
                    ConversionFactor = fuel.Invoice.ConversionFactor,
                    DeliveryLevelPO = fuel.Invoice.TrackableSchedule != null ? fuel.Invoice.TrackableSchedule.DeliveryLevelPO : string.Empty,
                };
                if (fuel.TypeofFuel == (int)ProductTypes.NonStandardFuel)
                {
                    dropViewModel.OtherTaxDetails.AddRange(drop.SelectMany(t => t.TaxDetails).ToList().ToTaxViewModel(true));
                    dropViewModel.OtherTaxDetails.ForEach(t => t.OrderId = fuel.OrderId);
                }
                SetAssetDetails(drop, dropViewModel);
                if ((dropViewModel.UoM == UoM.MetricTons || dropViewModel.UoM == UoM.Barrels) && dropViewModel.Assets != null && dropViewModel.Assets.Any())
                {
                    foreach (var asset in dropViewModel.Assets)
                    {
                        decimal conversionFator = 0;
                        if (dropViewModel.UoM == UoM.MetricTons && asset.Gravity.HasValue && asset.Gravity.Value > 0)
                        {
                            conversionFator = asset.Gravity.Value;
                        }
                        else if (dropViewModel.UoM == UoM.Barrels)
                        {
                            conversionFator = ApplicationConstants.GallonsToBarrelConversion;
                        }
                        if (conversionFator > 0)
                        {
                            if (asset.DropGallons == null)
                            {
                                asset.DropGallons = 0;
                            }
                            else
                            {
                                var qtyInGallons = GetOriginalDroppedGallonsForMFN(asset.DropGallons.Value, conversionFator, (int)dropViewModel.UoM);
                                asset.DropGallons = qtyInGallons;
                            }
                        }
                    }
                }
                var fees = GetInvoiceFuelFees(drop.SelectMany(t => t.FuelRequestFees).ToList(), dropViewModel.ActualDropQuantity);
                invoiceModel.Fees.AddRange(fees.FuelRequestFees);
                SetSurchargeDetails(fuel.InvoiceXAdditionalDetail, fees.FuelSurchargeFreightFee, fuel.ProductTypeId, dropViewModel.ActualDropQuantity);
                dropViewModel.FuelSurchargeFreightFee = fees.FuelSurchargeFreightFee;
                dropViewModel.FuelSurchargeFreightFee.GallonsDelivered = fuel.DroppedGallons;
                if (fuel.BDRDetail != null && IsAnyBdrPropertyAdded(fuel.BDRDetail))
                {
                    dropViewModel.BdrDetails = fuel.BDRDetail.ToViewModel();
                    dropViewModel.IsBdrDetailsAdded = true;
                }
                invoiceModel.Drops.Add(dropViewModel);
            }
        }

        private static void SetBolDetails(InvoiceViewModelNew invoiceModel, List<GetInvoiceDetailsModel> invoices)
        {
            var bolDetails = invoices.SelectMany(t => t.BolDetails).Where(t => t.PickupLocation == PickupLocationType.Terminal || t.PickupLocation == PickupLocationType.None)
                                .GroupBy(t => t.BolNumber).ToList();
            if (bolDetails.Any()
                && (bolDetails.Count > 1
                    || bolDetails.FirstOrDefault().Any(t => (t.BolNumber != null && t.BolNumber != "")
                    || t.ImageId == null
                    || t.InvoiceXBolDetails.Any(t1 => t1.Invoice.Order.TerminalId != t.TerminalId))))
            {
                foreach (var bol in bolDetails)
                {
                    var bolDetail = bol.Select(t => t).ToList();
                    invoiceModel.BolDetails.Add(bolDetail.ToBolViewModel());
                }
            }
            var liftDetails = invoices.SelectMany(t => t.BolDetails).Where(t => t.PickupLocation == PickupLocationType.BulkPlant).GroupBy(t => t.LiftTicketNumber).ToList();
            if (liftDetails != null && liftDetails.Any())
            {
                foreach (var liftTicket in liftDetails)
                {
                    var liftDetail = liftTicket.Select(t => t).ToList();
                    invoiceModel.TicketDetails.Add(liftDetail.ToLiftViewModel());
                }
            }

        }

        private void SetAssetDetails(IGrouping<int, GetInvoiceDetailsModel> drop, InvoiceDropViewModel dropViewModel)
        {
            foreach (var inv in drop)
            {
                var assets = GetAssetDropDetails(inv.Invoice);
                assets.Where(t => t.DropGallons != null && t.DropGallons != 0).ToList().ForEach(t => t.InvoiceId = inv.Invoice.Id);
                dropViewModel.Assets.AddRange(assets);
            }
        }

        private void SetInvoiceHeaderDetails(GetInvoiceDetailsModel invoice, InvoiceViewModelNew invoiceModel)
        {
            invoiceModel.OriginalInvoiceHeaderId = invoice.Invoice.InvoiceHeaderId;
            invoiceModel.IsVariousOrigin = invoice.IsVariousOrigin;
            invoiceModel.OriginalInvoiceNumberId = invoice.Invoice.InvoiceHeader.InvoiceNumberId;
            invoiceModel.FuelDropLocation = invoice.DropAddress.ToPickUpLocation();
            invoiceModel.PaymentTerm = new PaymentTermViewModel() { TermId = (PaymentTerms)invoice.PaymentTermId, NetDays = invoice.NetDays };
            invoiceModel.Customer = new CustomerViewModel()
            {
                CompanyId = invoice.BuyerCompanyId,
                CompanyName = invoice.CustomerCompanyName,
                DropLocation = invoice.DropAddress.ToPickUpLocation(),
                Location = new JobLocationViewModel() { JobId = invoice.Job.JobId, SiteName = invoice.Job.SiteName, Address = invoice.Job.Address, City = invoice.Job.City, StateCode = invoice.Job.StateCode, ZipCode = invoice.Job.ZipCode, IsMarineLocation = invoice.Job.IsMarineLocation }
            };
            invoiceModel.AdditionalImage = invoice.AdditionalImage?.ToViewModel();
            invoiceModel.InspectionRequestVoucherImage = invoice.InspectionVoucherImage?.ToViewModel();

            if (invoice.Driver != null)
            {
                invoiceModel.Driver = new DropdownDisplayItem() { Id = invoice.Driver.Id, Name = $"{invoice.Driver.FirstName} {invoice.Driver.LastName}" };
            }
            invoiceModel.InvoiceTypeId = invoice.InvoiceTypeId;
            invoiceModel.InvoiceNotes = invoice.InvoiceXAdditionalDetail.Notes;
            invoiceModel.Carrier = invoice.BolDetails.Select(t => t.Carrier).FirstOrDefault();
            invoiceModel.IsAssetTracked = invoice.IsAssetTracked;
            invoiceModel.InvoiceImage = invoice.InvoiceImage?.ToViewModel();
            invoiceModel.TaxAffidavitImage = invoice.TaxAffidavitImage?.ToViewModel();
            invoiceModel.BDNImage = invoice.BDNImage?.ToViewModel();
            invoiceModel.CoastGuardInspectionImage = invoice.CoastGuardInspectionImage?.ToViewModel();
            invoiceModel.BrokerChainId = invoice.BrokeredChainId;
            invoiceModel.SignatureImage = invoice.Signature?.ToViewModel().Image;
            invoiceModel.InvoiceTypeId = invoice.Invoice.SupplierPreferredInvoiceTypeId ?? invoice.PreferredInvoiceTypeId;
            SetBlobContainerType(invoiceModel.InvoiceImage);
            SetBlobContainerType(invoiceModel.SignatureImage);
            SetBlobContainerType(invoiceModel.AdditionalImage);
            SetBlobContainerType(invoiceModel.InspectionRequestVoucherImage);
            SetBlobContainerType(invoiceModel.CoastGuardInspectionImage);
            SetBlobContainerType(invoiceModel.TaxAffidavitImage);
            SetBlobContainerType(invoiceModel.BDNImage);

            if (!string.IsNullOrWhiteSpace(invoice.Invoice.ReferenceId))
                invoiceModel.SupplierInvoiceNumber = invoice.Invoice.DisplayInvoiceNumber;

            if (invoice.Invoice.WaitingFor == (int)WaitingAction.Nothing)
                invoiceModel.IsRebillInvoice = true;
        }


        private void SetBlobContainerType(ImageViewModel imageViewModel)
        {
            if (imageViewModel != null)
                imageViewModel.BlobContainerType = BlobContainerType.InvoicePdfFiles;
        }

        private void SetCustomerDetails(GetInvoiceDetailsModel invoice, InvoiceViewModelNew invoiceModel)
        {
            var user = invoice.Customer;
            if (invoice.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest)
            {
                var counterOffer = invoice.FuelRequest.CounterOffers.FirstOrDefault(t => t.BuyerStatus == (int)CounterOfferStatus.Accepted);
                if (counterOffer != null)
                {
                    user = Context.DataContext.Users.Where(t => t.Id == counterOffer.BuyerId).Select(t => new UserViewModel()
                    {
                        Email = t.Email,
                        PhoneNumber = t.PhoneNumber,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        CompanyName = t.Company.Name
                    }).SingleOrDefault();
                }
            }
            invoiceModel.Customer.ContactEmail = user.Email;
            invoiceModel.Customer.ContactPhone = user.PhoneNumber;
            invoiceModel.Customer.ContactName = $"{user.FirstName} {user.LastName}";
            invoiceModel.Customer.Location = invoice.Job;
        }
        public StatusViewModel UpdateInvoicePONumber(int invoiceHeaderId, int telaFuelServiceOrderId)
        {
            var response = new StatusViewModel();
            try
            {

                Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId).ToList().ForEach(t =>
                {
                    LogManager.Logger.WriteDebug("InvoiceDomain", "UpdateInvoicePONumber", string.Format("PO Number {0} is updated with {1} for invoice number {2}", t.PoNumber, Convert.ToString(telaFuelServiceOrderId), t.Id));
                    t.PoNumber = Convert.ToString(telaFuelServiceOrderId);
                });
                Context.Commit();
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("InvoiceDomain", "UpdateInvoicePONumber", ex.Message, ex);
            }
            return response;
        }
        public async Task UpdateDiversionDropDelieveryRequests(InvoiceViewModelNew model)
        {
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var trackableScheduleIds = model.DivertedDrops.Select(x => x.TrackableScheduleId).ToList();
                    if (trackableScheduleIds.Any())
                    {
                        var trackableSchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => trackableScheduleIds.Contains(t.Id)).ToList();
                        foreach (var trackableSchedule in trackableSchedules)
                        {
                            trackableSchedule.DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.Diverted;
                            Context.DataContext.Entry(trackableSchedule).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }

                        transaction.Commit();

                        var delieveryRequests = trackableSchedules.Select(t => t.FrDeliveryRequestId).ToList();
                        var drIds = delieveryRequests.Where(dr => !string.IsNullOrEmpty(dr)).ToList();
                        if (drIds.Any())
                        {
                            var delReqStatusUpdate = new List<DeliveryReqStatusUpdateModel>();
                            var driverId = model.Driver.Id;

                            foreach (var drId in drIds)
                            {
                                delReqStatusUpdate.Add(new DeliveryReqStatusUpdateModel() { DeliveryRequestId = drId, ScheduleStatusId = (int)TrackableDeliveryScheduleStatus.Diverted, UserId = driverId });
                            }
                            if (delReqStatusUpdate.Any())
                            {
                                new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatusUpdate);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceDomain", "UpdateDiversionDropDelieveryRequests", ex.Message, ex);
                    transaction.Rollback();
                }
            }
        }

        private async Task<List<CumulationQuantityUpdateViewModel>> CreateListOfCumulationEntitiesToUpdateForDeleteInv(Invoice invoiceModel)
        {
            var responseList = new List<CumulationQuantityUpdateViewModel>();
            try
            {
                if (invoiceModel != null && invoiceModel.Order != null)
                {
                    if (invoiceModel.InvoiceTypeId != ((int)InvoiceType.DigitalDropTicketManual)
                        && invoiceModel.InvoiceTypeId != ((int)InvoiceType.DigitalDropTicketMobileApp))
                    {
                        var updatedQty = new CumulationQuantityUpdateViewModel();
                        updatedQty.DroppedGallons = invoiceModel.DroppedGallons * -1;
                        updatedQty.RequestPriceDetailsId = invoiceModel.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId;
                        responseList.Add(updatedQty);
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "CreateListOfCumulationEntitiesToUpdateForDeleteInv", ex.Message, ex);
            }
            return responseList;
        }

        public async Task<List<DropQuantityByPrePostDipResponseModel>> CalculateDropQuantitiesFromPrePostForCreateInvoice(List<AssetDropViewModel> assetInfoList)
        {
            var response = new List<DropQuantityByPrePostDipResponseModel>();
            try
            {
                if (assetInfoList != null && assetInfoList.Any())
                {
                    var IncompleteInfoAssets = assetInfoList.Where(t => t.PreDip == null || t.PostDip == null || t.TankScaleMeasurement == TankScaleMeasurement.None).ToList();
                    if (IncompleteInfoAssets != null && IncompleteInfoAssets.Any())
                    {
                        foreach (var inCompleteAsset in IncompleteInfoAssets)
                        {
                            var result = new DropQuantityByPrePostDipResponseModel();
                            result.StatusCode = Status.Warning;
                            result.JobxAssetId = inCompleteAsset.JobXAssetId;
                            result.StatusMessage = "Dip Values /UoM is required for asset/Tank ";
                            response.Add(result);
                        }
                        return response;
                    }
                    else
                    {
                        var assets = assetInfoList.Where(t1 => t1.PreDip > 0 && t1.PostDip > 0 && t1.TankScaleMeasurement > (int)TankScaleMeasurement.None);
                        List<int> jobXassets = assets.Select(t => t.JobXAssetId).ToList();
                        var jobXassetIds = Context.DataContext.JobXAssets.Where(t => jobXassets.Contains(t.Id)).Select(t => new { t.JobId, t.Id, t.AssetId, t.Asset.Type }).ToList();
                        var apiInput = new List<DropQuantityByPrePostDipRequestModel>();
                        foreach (var asset in assets)
                        {
                            var assetInfo = jobXassetIds.FirstOrDefault(t => t.Id == asset.JobXAssetId);
                            if (assetInfo.Type == (int)AssetType.Tank)
                            {
                                if (assetInfo != null)
                                {
                                    var input = new DropQuantityByPrePostDipRequestModel()
                                    {
                                        JobxAssetId = assetInfo.Id,
                                        JobId = assetInfo.JobId,
                                        TankId = assetInfo.AssetId,
                                        PreDipValue = asset.PreDip.Value,
                                        PostDipValue = asset.PostDip.Value,
                                        ScaleMeasurement = (int)asset.TankScaleMeasurement
                                    };
                                    apiInput.Add(input);
                                }
                            }
                            else if (assetInfo.Type == (int)AssetType.Asset)
                            {
                                var result = new DropQuantityByPrePostDipResponseModel();
                                result.JobxAssetId = assetInfo.Id;
                                result.DropQuantity = asset.PostDip.Value - asset.PreDip.Value;
                                result.StatusCode = (int)Status.Success;
                                response.Add(result);
                            }

                        }
                        if (apiInput.Any())
                        {
                            var apiResponse = Task.Run(() => new FreightServiceDomain(this).GetDropQuantityByPrePostDip(apiInput)).Result;
                            if (apiResponse != null && apiResponse.Any())
                            {
                                response.AddRange(apiResponse);
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "CalculateDropQuantitiesFromPrePostForCreateInvoice", ex.Message, ex);
            }
            return response;
        }

        public async Task<MFNConversionResponseViewModel> ValidateGravityAndConvertForMFN(MFNConversionRequestViewModel conversionRequest)
        {
            var response = new MFNConversionResponseViewModel();
            try
            {
                if (conversionRequest.UoM == UoM.MetricTons)
                {
                    decimal gallonsPerMetricTon;
                    if (conversionRequest.UserProvidedConversionFactor.HasValue && conversionRequest.UserProvidedConversionFactor.Value > 0)
                        gallonsPerMetricTon = conversionRequest.UserProvidedConversionFactor.Value;
                    else
                        gallonsPerMetricTon = await Context.DataContext.MstGravityConversions
                                                                       .Where(t => t.Gravity == conversionRequest.ConversionFactor)
                                                                       .Select(t => t.GallonsPerMetricTon)
                                                                       .FirstOrDefaultAsync();
                    if (gallonsPerMetricTon > 0)
                    {
                        decimal droppedMetricTons = 0;
                        if (conversionRequest.JobCountryId == (int)Country.CAN)
                        {
                            var qtyInGallons = conversionRequest.DroppedGallons * Convert.ToDecimal(ApplicationConstants.LitreToGallonsConversion); // convert to gallons
                            droppedMetricTons = qtyInGallons / gallonsPerMetricTon;
                        }
                        else if (conversionRequest.JobCountryId == (int)Country.USA || conversionRequest.JobCountryId == (int)Country.CAR)
                        {
                            droppedMetricTons = conversionRequest.DroppedGallons / gallonsPerMetricTon;
                        }
                        response.IsValidGravity = true;
                        response.ConvertedQty = Math.Round(droppedMetricTons, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                        response.UoM = conversionRequest.UoM;
                    }
                    else
                    {
                        response.IsValidGravity = false;
                        response.UoM = conversionRequest.UoM;
                        response.StatusMessage = Resource.errorMessageInvalidApiGravity;
                    }
                }
                else if (conversionRequest.UoM == UoM.Barrels)
                {
                    decimal droppedBarrels = 0;
                    if (conversionRequest.JobCountryId == (int)Country.CAN)
                    {
                        var qtyInGallons = conversionRequest.DroppedGallons * Convert.ToDecimal(ApplicationConstants.LitreToGallonsConversion); // convert to gallons
                        droppedBarrels = qtyInGallons / ApplicationConstants.GallonsToBarrelConversion;
                    }
                    else if (conversionRequest.JobCountryId == (int)Country.USA || conversionRequest.JobCountryId == (int)Country.CAR)
                    {
                        droppedBarrels = conversionRequest.DroppedGallons / ApplicationConstants.GallonsToBarrelConversion;
                    }
                    response.IsValidGravity = true;
                    response.ConvertedQty = droppedBarrels;
                    response.UoM = conversionRequest.UoM;
                }
                else
                {
                    response.StatusMessage = Resource.errorMessageApiGravityNotApplicable;
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errorMessageUnableToProcessRequest;
                LogManager.Logger.WriteException("InvoiceDomain", "ValidateGravityAndConvertForMFN", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ValidateGravityByInvoiceId(int invoiceId, decimal gravity)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                if (invoiceId <= 0)
                {
                    response.StatusMessage = Resource.errRecordNotFound;
                }

                var validateGravityModel = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId)
                                                                      .Select(t => new MFNConversionRequestViewModel
                                                                      {
                                                                          ConversionFactor = gravity,
                                                                          DroppedGallons = t.DroppedGallons,
                                                                          JobCountryId = t.Order.FuelRequest.Job.CountryId,
                                                                          UoM = t.Order.FuelRequest.UoM,
                                                                      }).FirstOrDefaultAsync();

                var result = await ValidateGravityAndConvertForMFN(validateGravityModel);
                response.ResponseData = result;
                response.StatusCode = Status.Success;
                response.StatusMessage = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "ValidateGravityByInvoiceId", ex.Message, ex);
            }

            return response;
        }

        public async Task<MFNConversionResponseViewModel> IsValidApiGravity(int invoiceId, decimal gravity)
        {
            MFNConversionResponseViewModel response = null;
            try
            {
                if (invoiceId <= 0)
                {
                    return response;
                }

                var validateGravityModel = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId)
                                                                      .Select(t => new MFNConversionRequestViewModel
                                                                      {
                                                                          ConversionFactor = gravity,
                                                                          DroppedGallons = t.DroppedGallons,
                                                                          JobCountryId = t.Order.FuelRequest.Job.CountryId,
                                                                          UoM = t.Order.FuelRequest.UoM,
                                                                      }).FirstOrDefaultAsync();

                response = await ValidateGravityAndConvertForMFN(validateGravityModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "IsValidApiGravity", ex.Message, ex);
            }

            return response;
        }

        public decimal GetOriginalDroppedGallonsForMFN(decimal convertedDroppedGallons, decimal conversionFactor, int uom)
        {
            decimal response = 0;
            try
            {
                if (uom == (int)UoM.MetricTons)
                {
                    if (convertedDroppedGallons > 0 && conversionFactor > 0)
                    {
                        var gallonsPerMetricTon = Context.DataContext.MstGravityConversions.Where(t => t.Gravity == Math.Round(conversionFactor, 1)).Select(t => t.GallonsPerMetricTon).FirstOrDefault();
                        if (gallonsPerMetricTon > 0)
                        {
                            response = convertedDroppedGallons * gallonsPerMetricTon;
                        }
                    }
                }
                else if (uom == (int)UoM.Barrels)
                {
                    response = convertedDroppedGallons * conversionFactor;

                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "ValidateGravityAndConvertForMFN", ex.Message, ex);
            }
            return Math.Round(response, 4);
        }

        public async Task<MFNConversionResponseViewModel> ValidateAndConvertPricingForMarketBasedMFN(MFNConversionRequestViewModel conversionRequest)
        {
            var response = new MFNConversionResponseViewModel();
            try
            {
                if (conversionRequest.UoM == UoM.MetricTons)
                {
                    if (conversionRequest.UserProvidedConversionFactor.HasValue && conversionRequest.UserProvidedConversionFactor.Value > 0)
                    {
                        response.IsValidGravity = true;
                        response.ConvertedQty = conversionRequest.DroppedGallons;
                        response.UoM = conversionRequest.UoM;
                    }
                    else
                    {
                        var gallonsPerMetricTon = await Context.DataContext.MstGravityConversions
                                                                    .Where(t => t.Gravity == conversionRequest.ConversionFactor)
                                                                    .Select(t => t.GallonsPerMetricTon)
                                                                    .FirstOrDefaultAsync();
                        if (gallonsPerMetricTon > 0)
                        {
                            decimal droppedMetricTons = 0;
                            if (conversionRequest.JobCountryId == (int)Country.CAN)
                            {
                                var qtyInGallons = conversionRequest.DroppedGallons * Convert.ToDecimal(ApplicationConstants.LitreToGallonsConversion); // convert to gallons
                                droppedMetricTons = qtyInGallons * gallonsPerMetricTon;
                            }
                            else if (conversionRequest.JobCountryId == (int)Country.USA)
                            {
                                droppedMetricTons = conversionRequest.DroppedGallons * gallonsPerMetricTon;
                            }
                            response.IsValidGravity = true;
                            response.ConvertedQty = Math.Round(droppedMetricTons, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                            response.UoM = conversionRequest.UoM;
                        }
                        else
                        {
                            response.IsValidGravity = false;
                            response.UoM = conversionRequest.UoM;
                        }
                    }
                }
                else if (conversionRequest.UoM == UoM.Barrels)
                {
                    decimal droppedBarrels = 0;
                    if (conversionRequest.JobCountryId == (int)Country.CAN)
                    {
                        var qtyInGallons = conversionRequest.DroppedGallons * Convert.ToDecimal(ApplicationConstants.LitreToGallonsConversion); // convert to gallons
                        droppedBarrels = qtyInGallons * ApplicationConstants.GallonsToBarrelConversion;
                    }
                    else if (conversionRequest.JobCountryId == (int)Country.USA)
                    {
                        droppedBarrels = conversionRequest.DroppedGallons * ApplicationConstants.GallonsToBarrelConversion;
                    }
                    response.IsValidGravity = true;
                    response.ConvertedQty = droppedBarrels;
                    response.UoM = conversionRequest.UoM;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "ValidateAndConvertPricingForMarketBasedMFN", ex.Message, ex);
            }
            return response;
        }

        public async Task<MarineTaxAffidavitPdfViewModel> GetMarineTaxAffidavitInfo(int invoiceHeaderId, UserContext userContext = null)
        {
            var response = new MarineTaxAffidavitPdfViewModel();
            try
            {
                response.InvoiceHeaderId = invoiceHeaderId;
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetDetailsForMarineTaxAffidavitPdf(invoiceHeaderId, userContext);
                response.TaxAffidavitImages = await GetMarineTaxAffidavitImages(invoiceHeaderId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetMarineTaxAffidavitInfo", ex.Message, ex);
            }
            return response;
        }

        public async Task<MarineBDNPdfViewModel> GetMarineBDNInfo(int invoiceHeaderId, UserContext userContext = null)
        {
            var response = new MarineBDNPdfViewModel();
            try
            {
                response.InvoiceHeaderId = invoiceHeaderId;
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetDetailsForMarineBDNPdf(invoiceHeaderId, userContext);
                response.BDNImages = await GetMarineBDNImages(invoiceHeaderId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetMarineBDNInfo", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ImageViewModel>> GetMarineTaxAffidavitImages(int invoiceHeaderId, int imageId = 0)
        {
            var response = new List<ImageViewModel>();
            try
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToListAsync();
                if (invoice != null)
                {
                    foreach (var item in invoice)
                    {
                        var taxAffidateImageId = (item.InvoiceXAdditionalDetail != null && item.InvoiceXAdditionalDetail.TaxAffidavitImageId != null &&
                                           item.InvoiceXAdditionalDetail.TaxAffidavitImageId.Value > 0) ? item.InvoiceXAdditionalDetail.TaxAffidavitImageId.Value : 0;
                        var taxAffidateImage = await Context.DataContext.Images.FirstOrDefaultAsync(t => t.Id == taxAffidateImageId);
                        if (taxAffidateImage != null)
                        {
                            var taxAffidateImageModel = taxAffidateImage.ToViewModel();
                            var taxAffidateImageModels = taxAffidateImageModel.ToBreakFilePathToMany();
                            foreach (var img in taxAffidateImageModels)
                            {
                                img.Name = "Tax Affidavit-" + item.DisplayInvoiceNumber;
                                response.Add(img);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetMarineTaxAffidavitImages", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ImageViewModel>> GetMarineBDNImages(int invoiceHeaderId, int imageId = 0)
        {
            var response = new List<ImageViewModel>();
            try
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToListAsync();
                if (invoice != null)
                {
                    foreach (var item in invoice)
                    {
                        var BDNImageId = (item.InvoiceXAdditionalDetail != null && item.InvoiceXAdditionalDetail.BDNImageId != null &&
                                           item.InvoiceXAdditionalDetail.BDNImageId.Value > 0) ? item.InvoiceXAdditionalDetail.BDNImageId.Value : 0;
                        var BDNImage = await Context.DataContext.Images.FirstOrDefaultAsync(t => t.Id == BDNImageId);
                        if (BDNImage != null)
                        {
                            var BDNImageModel = BDNImage.ToViewModel();
                            var BDNImageModels = BDNImageModel.ToBreakFilePathToMany();
                            foreach (var img in BDNImageModels)
                            {
                                img.Name = BlobContainerType.BDN.GetDisplayName().ToLower() + item.DisplayInvoiceNumber;
                                response.Add(img);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetMarineBDNImages", ex.Message, ex);
            }
            return response;
        }

        public async Task<MarineCGInspectionDocumentPdfViewModel> GetMarineCGInspectionDocumentInfo(int invoiceHeaderId, UserContext userContext = null)
        {
            var response = new MarineCGInspectionDocumentPdfViewModel();
            try
            {
                response.InvoiceHeaderId = invoiceHeaderId;
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetDetailsForMarineCGInspectionDocument(invoiceHeaderId, userContext);
                if (response != null)
                {
                    response.CGInspectionImages = await GetMarineTaxCGInspectionImages(invoiceHeaderId);
                    var invoices = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToListAsync();
                    if (invoices != null && invoices.Any())
                    {
                        foreach (var invoice in invoices)
                        {
                            var assetDrop = await spDomain.GetAssetDropsForInvoice(invoice.Id);
                            if (assetDrop != null && assetDrop.Any())
                            {
                                response.AssetDrops.AddRange(assetDrop);
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetMarineCGInspectionDocumentInfo", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ImageViewModel>> GetMarineTaxCGInspectionImages(int invoiceHeaderId, int imageId = 0)
        {
            var response = new List<ImageViewModel>();
            try
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToListAsync();
                if (invoice != null)
                {
                    foreach (var item in invoice)
                    {
                        var coastGuardInspectionImageId = (item.InvoiceXAdditionalDetail != null && item.InvoiceXAdditionalDetail.CoastGuardInspectionImageId != null &&
                                           item.InvoiceXAdditionalDetail.CoastGuardInspectionImageId.Value > 0) ? item.InvoiceXAdditionalDetail.CoastGuardInspectionImageId.Value : 0;
                        var coastGuardInspectionImage = await Context.DataContext.Images.FirstOrDefaultAsync(t => t.Id == coastGuardInspectionImageId);
                        if (coastGuardInspectionImage != null)
                        {
                            var coastGuardImageModel = coastGuardInspectionImage.ToViewModel();
                            var coastGuardImageModels = coastGuardImageModel.ToBreakFilePathToMany();
                            foreach (var img in coastGuardImageModels)
                            {
                                img.Name = "CGInspection -" + item.DisplayInvoiceNumber;
                                response.Add(img);
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetMarineTaxCGInspectionImages", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<MarineInspectionRequestVoucherViewModel>> GetMarineInspectionVoucherDocumentInfo(int invoiceHeaderId, UserContext userContext = null)
        {
            var response = new List<MarineInspectionRequestVoucherViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetDetailsForMarineInspRequestVoucher(invoiceHeaderId, userContext);
                if (response != null && response.Any())
                {

                    foreach (var voucher in response)
                    {
                        var driverId = (voucher.DriverId == null || voucher.DriverId == 0) ? 0 : Convert.ToInt32(voucher.DriverId);
                        voucher.InspRequestVoucherImages = await GetMarineInspRequestVoucherImages(voucher.InvoiceId, 0, driverId);
                        var invoiceAssetDrops = await spDomain.GetAssetDropsForInvoice(voucher.InvoiceId);
                        if (invoiceAssetDrops != null && invoiceAssetDrops.Any())
                        {
                            voucher.AssetDrops = invoiceAssetDrops;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetMarineInspectionVoucherDocumentInfo", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ImageViewModel>> GetMarineInspRequestVoucherImages(int invoiceId, int invoiceHeaderId = 0, int driverId = 0)
        {
            var response = new List<ImageViewModel>();
            try
            {
                List<Invoice> invoices = new List<Invoice>();
                if (invoiceHeaderId > 0)
                {
                    if (driverId > 0)
                        invoices = await Context.DataContext.Invoices.Where(t => t.DriverId == driverId && t.InvoiceHeaderId == invoiceHeaderId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToListAsync();
                    else
                        invoices = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToListAsync();
                }
                else
                {
                    invoiceHeaderId = Context.DataContext.Invoices.Where(t => t.Id == invoiceId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).FirstOrDefault().InvoiceHeaderId;
                    if (driverId > 0)
                        invoices = await Context.DataContext.Invoices.Where(t => t.DriverId == driverId && t.InvoiceHeaderId == invoiceHeaderId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToListAsync();
                    else
                        invoices = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToListAsync();
                }
                if (invoices != null)
                {
                    foreach (var item in invoices)
                    {
                        var inspVoucherImageId = (item.InvoiceXAdditionalDetail != null && item.InvoiceXAdditionalDetail.InspectionRequestVoucherImageId != null &&
                                           item.InvoiceXAdditionalDetail.InspectionRequestVoucherImageId.Value > 0) ? item.InvoiceXAdditionalDetail.InspectionRequestVoucherImageId.Value : 0;
                        var inspectionVoucherImage = await Context.DataContext.Images.FirstOrDefaultAsync(t => t.Id == inspVoucherImageId);
                        if (inspectionVoucherImage != null)
                        {
                            var inspectionVoucherImageModel = inspectionVoucherImage.ToViewModel();
                            var inspectionVoucherImageModels = inspectionVoucherImageModel.ToBreakFilePathToMany();
                            foreach (var img in inspectionVoucherImageModels)
                            {
                                img.Name = "InspRequestVoucher -" + item.DisplayInvoiceNumber;
                                response.Add(img);
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetMarineInspRequestVoucherImages", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetBOlListForInvoiceHeader(int headerId)
        {
            var response = await Context.DataContext.InvoiceXBolDetails.Where(t => t.InvoiceHeaderId == headerId && t.InvoiceFtlDetail != null && (t.InvoiceFtlDetail.LiftTicketNumber != null || t.InvoiceFtlDetail.BolNumber != null))
                                .Select(t => new DropdownDisplayItem() { Id = t.InvoiceFtlDetail.Id, Name = (t.InvoiceFtlDetail.LiftTicketNumber != null ? t.InvoiceFtlDetail.LiftTicketNumber : t.InvoiceFtlDetail.BolNumber) })
                                .ToListAsync();

            return response;
        }

        bool IsAnyBdrPropertyAdded(object obj)
        {
            var isProperyAdded = false;
            try
            {
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (prop.Name.Equals("InvoiceId") || prop.Name.Equals("BDRNumber") || prop.Name.Equals("IsActive"))
                    {
                        continue;
                    }
                    if (prop.PropertyType == typeof(string))
                    {
                        string value = (string)prop.GetValue(obj);
                        if (!string.IsNullOrEmpty(value))
                        {
                            isProperyAdded = true;
                            return isProperyAdded;
                        }
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        bool value = (bool)prop.GetValue(obj);
                        if (value)
                        {
                            isProperyAdded = true;
                            return isProperyAdded;

                        }
                    }
                }
                isProperyAdded = false;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "IsAnyBdrPropertyAdded", ex.Message, ex);

            }
            return isProperyAdded;

        }

        public async Task<MFNConversionResponseViewModel> ValidateGravityForDDTInlineEdit(List<InvoiceBolEditGrid> models)
        {
            MFNConversionResponseViewModel response = new MFNConversionResponseViewModel();
            try
            {
                if (models != null && models.Any())
                {
                    var domain = new OrderDomain();
                    foreach (var item in models)
                    {
                        if (item.ApiGravity.HasValue && item.ApiGravity.Value > 0 && item.UoM == UoM.MetricTons)
                        {
                            response.IsValidGravity = true;
                            var conversionResponse = await domain.GetGallonsPerMetricTonAsync(item.ApiGravity.Value);
                            if (conversionResponse.Result <= 0)
                            {
                                response.IsValidGravity = false;
                                response.ConvertedQty = item.ApiGravity.Value;
                                return response;
                            }
                        }
                        else if (item.UoM != UoM.MetricTons)
                        {
                            item.ApiGravity = null;
                            response.IsValidGravity = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("InvoiceDomain", "ValidateGravityForDDTsInlineEdit", ex.Message, ex);
            }
            return response;
        }


        public async Task<StatusViewModel> DeleteBolForMarineInvoice(int invoiceHeaderId, int invoiceFtlDetailsId, int invoiceId)
        {
            var response = new StatusViewModel();
            try
            {
                if (invoiceHeaderId > 0 && invoiceFtlDetailsId > 0 && invoiceId > 0)
                {

                    var spDomain = new StoredProcedureDomain(this);
                    BooleanResponseModel spResponse = await spDomain.DeleteBolForMarineInvoice(invoiceHeaderId, invoiceFtlDetailsId, invoiceId);
                    if (spResponse != null)
                    {
                        response.StatusCode = spResponse.Result ? Status.Success : Status.Failed;
                        response.StatusMessage = spResponse.Message;
                    }

                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = "Failed to get details to validate delete request";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "DeleteBolForMarineInvoice", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = "Error occurred when processing your request.";
            }
            return response;
        }

        public async Task<bool> CreateDeliveryDetailsDailyDataDumpReport()
        {
            var response = false;
            try
            {
                var csvMailingList = await Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingDailyDataDumpCSVMailingList).Select(t => t.Value).FirstOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(csvMailingList))
                {
                    var mailingList = JsonConvert.DeserializeObject<List<DailyDataDumpCSVMailingListViewModel>>(csvMailingList);
                    if (mailingList != null && mailingList.Any())
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        MemoryStream contentStream = null;
                        foreach (var item in mailingList)
                        {
                            if (!string.IsNullOrWhiteSpace(item.Emails) && item.CompanyId > 0)
                            {
                                contentStream = null;
                                List<Usp_CompanySpecificDeliveryDetails> records = new List<Usp_CompanySpecificDeliveryDetails>();
                                records = await spDomain.GetCompanySpecificDeliveryDetails(item.CompanyId);
                                if (records != null && records.Any())
                                {
                                    var csvRecords = records.Select(t => t.ToCsvViewModel());

                                    contentStream = await GetCsvAsMemoryStream(csvRecords);

                                    var subject = Resource.emailSubjectDeliveryDetailsReport;
                                    var body = string.Format(Resource.emailDeliveryDetailsBodyText, DateTimeOffset.Now.Date.AddDays(-1).ToString("MM/dd/yyyy"));
                                    var fileName = "DeliveryDetails-" + DateTimeOffset.Now.ToString("MM/dd/yyyy HH:mm tt") + ".csv";
                                    response = SendEmailCsvFile(contentStream, subject, body, fileName, item.Emails);
                                }
                                var debugMessage = string.Empty;
                                if (records == null || !records.Any())
                                {
                                    debugMessage = $"No Delivery details records found for timewindow {DateTimeOffset.Now.ToString()} for companyId {item.CompanyId.ToString()}";
                                }
                                else if (records != null && records.Any())
                                {
                                    if (response)
                                    {
                                        debugMessage = $"Delivery details records sent for timewindow {DateTimeOffset.Now.ToString()} for companyId {item.CompanyId.ToString()}";
                                    }
                                    else
                                    {
                                        debugMessage = $"Error occured when sending Delivery details records for timewindow {DateTimeOffset.Now.ToString()} for companyId {item.CompanyId.ToString()}";
                                    }

                                }
                                LogManager.Logger.WriteDebug("InvoiceDomain", "CreateDeliveryDetailsDailyDataDumpReport", debugMessage);
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "CreateDeliveryDetailsDailyDataDumpReport", ex.Message, ex);
            }
            return response;
        }

        private async Task<MemoryStream> GetCsvAsMemoryStream(IEnumerable<CompanySpecificDeliveryDetailsCsvViewModel> transactionsArray)
        {
            var memoryStream = new MemoryStream();
            var flatFileWriter = new StreamWriter(memoryStream, Encoding.ASCII);
            var fileWriterEngine = new FileHelperEngine(typeof(CompanySpecificDeliveryDetailsCsvViewModel));

            fileWriterEngine.HeaderText = Resource.DeliveryDetailsDataDumpHeaders;
            fileWriterEngine.WriteStream(flatFileWriter, transactionsArray);

            // Flush contents of fileWriterStream to underlying docStream:
            await flatFileWriter.FlushAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        protected bool SendEmailCsvFile(Stream contentStream, string subject, string body, string fileName, string mailingList)
        {
            var response = false;
            try
            {

                HelperDomain helperDomain = new HelperDomain();
                var serverUrl = helperDomain.GetServerUrl();
                Attachment file = new Attachment(contentStream, fileName, Core.Utilities.MediaType.Text);

                var attachements = new List<Attachment>() { file };

                if (!string.IsNullOrWhiteSpace(mailingList))
                {
                    var emails = mailingList.Split(';').ToList();
                    var companyLogo = helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.email_HeaderLogo);
                    var _emailTemplate = helperDomain.GetApplicationEventNotificationTemplate();
                    var emailModel = new ApplicationEventNotificationViewModel
                    {
                        To = emails,
                        Subject = subject,
                        CompanyLogo = companyLogo,
                        BodyText = body,
                        Attachments = attachements,
                        ShowFooterContent = false,
                        ShowHelpLineInfo = true
                    };
                    response = Email.GetClient().Send(_emailTemplate, emailModel);

                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "SendEmailCsvFile", ex.Message, ex);
            }
            return response;
        }
        private string GetDeliveryScheduleDeliveryLevelPO(int invoiceHeaderId)
        {
            string deliveryLevelPO = string.Empty;
            if (invoiceHeaderId > 0)
            {
                var deliveryScheduleInfo = Context.DataContext.Invoices.Where(x => x.InvoiceHeaderId == invoiceHeaderId && x.TrackableSchedule != null && !string.IsNullOrEmpty(x.TrackableSchedule.DeliveryLevelPO)).Select(x => new { x.TrackableSchedule.DeliveryLevelPO }).ToList();
                if (deliveryScheduleInfo.Any())
                {
                    foreach (var item in deliveryScheduleInfo)
                    {
                        deliveryLevelPO = deliveryLevelPO + item.DeliveryLevelPO + "</br>";
                    }
                }
            }
            return deliveryLevelPO;
        }
    }


    public class BillMemoModel
    {
        public Order Order { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public string BuyerCompanyName { get; set; }
        public int Invoiceid { get; set; }
    }

    public class GetInvoiceDetailsModel
    {
        public Invoice Invoice { get; set; }
        public int PreferredInvoiceTypeId { get; set; }
        public InvoiceXAdditionalDetail InvoiceXAdditionalDetail { get; set; }
        public int PaymentTermId { get; set; }
        public int NetDays { get; set; }
        public int BuyerCompanyId { get; set; }
        public string CustomerCompanyName { get; set; }
        public JobLocationViewModel Job { get; set; }
        public List<InvoiceFtlDetail> BolDetails { get; set; }
        public int OrderId { get; set; }
        public OrderDetailVersion OrderDetailVersion { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelTypeName { get; set; }
        public decimal DroppedGallons { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public FuelRequest FuelRequest { get; set; }
        public int? TrackableScheduleId { get; set; }
        public UserViewModel Driver { get; set; }
        public int InvoiceTypeId { get; set; }
        public InvoiceDispatchLocation DropAddress { get; set; }
        public string BrokeredChainId { get; set; }
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        public int TypeofFuel { get; set; }
        public int ProductTypeId { get; set; }
        public List<FuelFee> FuelRequestFees { get; set; }
        public UserViewModel Customer { get; set; }
        public bool IsAssetTracked { get; set; }
        public bool IsFtl { get; set; }

        public bool IsDriverToUpdateBOL { get; set; }
        public Image InvoiceImage { get; set; }
        public Image AdditionalImage { get; set; }
        public Image InspectionVoucherImage { get; set; }
        public Signature Signature { get; set; }
        public Image BolImage { get; set; }
        public bool IsVariousOrigin { get; set; }
        public List<TaxDetail> TaxDetails { get; set; }
        public bool IsDipDataRequired { get; set; }
        public Image TaxAffidavitImage { get; set; }
        public Image BDNImage { get; set; }
        public Image CoastGuardInspectionImage { get; set; }
        public BDRDetail BDRDetail { get; set; }
    }
}
