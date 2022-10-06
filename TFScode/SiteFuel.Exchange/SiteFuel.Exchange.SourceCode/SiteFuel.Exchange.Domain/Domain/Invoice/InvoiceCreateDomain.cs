using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Domain.Services;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using SiteFuel.Exchange.ViewModels.Queue;
//using SiteFuel.Exchange.ViewModels.WebNotification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class InvoiceCreateDomain : InvoiceCommonDomain
    {
        public InvoiceCreateDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public InvoiceCreateDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<StatusViewModel> CreateMobileInvoiceAsync(DriverDropOrderViewModel DriverDropViewModel)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("InvoiceCreateDomain", "CreateMobileInvoiceAsync"))
            {
                try
                {

                    if (!string.IsNullOrEmpty(DriverDropViewModel.TraceId) && Context.DataContext.Invoices.Any(t => t.TraceId == DriverDropViewModel.TraceId))
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = string.Format(Resource.valMessageAlreadyExist, Resource.lblTraceId);
                        return response;
                    }
                    CorrectMobileDropStartDateIfNotValid(DriverDropViewModel);

                    if (DriverDropViewModel.Image != null)
                        await DriverDropViewModel.Image.UploadImageToAzureBlobService(ApplicationConstants.DropImageFileNamePrefix, BlobContainerType.InvoicePdfFiles);

                    if (DriverDropViewModel.CustomerSignatureViewModel != null && DriverDropViewModel.CustomerSignatureViewModel.Image != null)
                        await DriverDropViewModel.CustomerSignatureViewModel.Image.UploadImageToAzureBlobService(ApplicationConstants.CustomerSignatureFileNamePrefix, BlobContainerType.InvoicePdfFiles);

                    var invoiceRequestViewModel = await GetCreateMobileInvoiceRequestViewModelAsync(DriverDropViewModel);
                    var mobileAssetDrops = await GetIncompleteMobileAssetDropsAsync(invoiceRequestViewModel.IsAssetTracked, invoiceRequestViewModel.InvoiceTypeId, invoiceRequestViewModel.OrderId, invoiceRequestViewModel.DriverId);
                    int assetCount = GetMobileAssetDropCount(mobileAssetDrops);
                    //var invoiceModel = await GetMobileInvoiceViewModelAsync(mobileInvoiceCreateRequestViewModel, assetCount);

                    InvoiceModel invoiceModel = new InvoiceModel();
                    SetMobileInputsToInvoiceModel(invoiceRequestViewModel, invoiceModel);
                    await SetInvoiceNumberToInvoiceModel(invoiceModel);
                    invoiceModel = await GetInvoiceViewModel(invoiceRequestViewModel, invoiceModel, assetCount);

                    if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                    {
                        // Set invoice pricing to invoice model
                        await SetInvoicePricingToInvoiceModel(invoiceRequestViewModel, invoiceModel);
                        SetInvoiceAmountAndAllowanceToInvoiceModel(invoiceRequestViewModel, invoiceModel);
                    }

                    if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                    {
                        invoiceRequestViewModel.PricePerGallon = invoiceModel.PricePerGallon;
                        await CheckAndSetDuplicateInvoiceException(invoiceRequestViewModel, invoiceModel);
                    }

                    RemoveDryRunFeeFromManualInvoiceModel(invoiceRequestViewModel);
                    ProcessInvoiceFuelFeesAndSetCalculatedValues(invoiceRequestViewModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                    InvoiceCreateRequestViewModel invoiceCreateRequestViewModel;
                    //CREATE DDT WAITINGOR TAX
                    if (invoiceModel.InvoiceTypeId == (int)InvoiceType.MobileApp && invoiceRequestViewModel.TypeOfFuel != (int)ProductDisplayGroups.OtherFuelType)
                    {
                        invoiceCreateRequestViewModel = GetInvoiceCreateRequestViewModelWithTaxWaitingStatus(invoiceRequestViewModel, invoiceModel);
                    }
                    else
                    {
                        invoiceCreateRequestViewModel = SetTaxesToInvoiceModel(invoiceRequestViewModel, invoiceModel);
                    }

                    invoiceCreateRequestViewModel.FCMAppId = DriverDropViewModel.Driver.FCMAppId;
                    List<InvoiceCreateResponseViewModel> invoiceCreateResponses;
                    if (invoiceRequestViewModel.IsBuySellOrder)
                    {
                        invoiceCreateResponses = await GenerateBuySellInvoiceForMobile(invoiceRequestViewModel, invoiceCreateRequestViewModel, invoiceModel, mobileAssetDrops);
                    }
                    else
                    {
                        invoiceCreateResponses = await GenerateMobileInvoice(invoiceRequestViewModel, invoiceCreateRequestViewModel, invoiceModel, DriverDropViewModel, mobileAssetDrops);
                    }
                    var invoiceCreateResponse = invoiceCreateResponses.First();
                    response.StatusCode = invoiceCreateResponse.StatusCode;
                    response.EntityId = invoiceCreateResponse.InvoiceHeaderId;
                    await SetMobileInvoiceCreatedPostEvents(invoiceRequestViewModel, invoiceModel, invoiceCreateResponses);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceCreateDomain", "CreateMobileInvoiceAsync", ex.Message, ex);
                }
            }
            return response;
        }


        public async Task<StatusViewModel> CreateFtlMobileInvoiceAsync(FtlDriverDropOrderViewModel DriverDropViewModel)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("InvoiceCreateDomain", "CreateFtlMobileInvoiceAsync"))
            {
                try
                {
                    if (!string.IsNullOrEmpty(DriverDropViewModel.TraceId) && Context.DataContext.Invoices.Any(t => t.TraceId == DriverDropViewModel.TraceId))
                    {
                        response.StatusCode = Status.Success; //Do not set as Failed
                        response.StatusMessage = string.Format(Resource.valMessageAlreadyExist, Resource.lblTraceId);
                        return response;
                    }
                    CorrectMobileDropStartDateIfNotValid(DriverDropViewModel);

                    if (DriverDropViewModel.Image != null)
                        await DriverDropViewModel.Image.UploadImageToAzureBlobService(ApplicationConstants.DropImageFileNamePrefix, BlobContainerType.InvoicePdfFiles);

                    if (DriverDropViewModel.CustomerSignatureViewModel != null && DriverDropViewModel.CustomerSignatureViewModel.Image != null)
                        await DriverDropViewModel.CustomerSignatureViewModel.Image.UploadImageToAzureBlobService(ApplicationConstants.CustomerSignatureFileNamePrefix, BlobContainerType.InvoicePdfFiles);

                    var invoiceRequestViewModel = await GetCreateFtlMobileInvoiceRequestViewModelAsync(DriverDropViewModel);
                    var mobileAssetDrops = await GetIncompleteMobileAssetDropsAsync(invoiceRequestViewModel.IsAssetTracked, invoiceRequestViewModel.InvoiceTypeId, invoiceRequestViewModel.OrderId, invoiceRequestViewModel.DriverId);
                    int assetCount = GetMobileAssetDropCount(mobileAssetDrops);
                    //var invoiceModel = await GetMobileInvoiceViewModelAsync(mobileInvoiceCreateRequestViewModel, assetCount);

                    InvoiceModel invoiceModel = new InvoiceModel();
                    SetMobileInputsToInvoiceModel(invoiceRequestViewModel, invoiceModel);
                    await SetInvoiceNumberToInvoiceModel(invoiceModel);
                    invoiceModel = await GetInvoiceViewModel(invoiceRequestViewModel, invoiceModel, assetCount);

                    //if (invoiceRequestViewModel.IsFTL && !invoiceRequestViewModel.IsDriverToUpdateBol)
                    //{
                    //    invoiceModel.WaitingFor = WaitingAction.BolDetails;
                    //    invoiceModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                    //}

                    await CheckAndSetInvoiceExceptions(invoiceRequestViewModel, invoiceModel);
                    if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                    {
                        // Set invoice pricing to invoice model
                        await SetInvoicePricingToInvoiceModel(invoiceRequestViewModel, invoiceModel);
                        SetInvoiceAmountAndAllowanceToInvoiceModel(invoiceRequestViewModel, invoiceModel);
                    }

                    if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                    {
                        invoiceRequestViewModel.PricePerGallon = invoiceModel.PricePerGallon;
                        await CheckAndSetDuplicateInvoiceException(invoiceRequestViewModel, invoiceModel);
                    }

                    // Set fees with calculation to invoice model
                    RemoveDryRunFeeFromManualInvoiceModel(invoiceRequestViewModel);
                    ProcessInvoiceFuelFeesAndSetCalculatedValues(invoiceRequestViewModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                    InvoiceCreateRequestViewModel invoiceCreateRequestViewModel;
                    //CREATE DDT WAITINGOR TAX
                    if (invoiceModel.InvoiceTypeId == (int)InvoiceType.MobileApp)
                    {
                        invoiceCreateRequestViewModel = GetInvoiceCreateRequestViewModelWithTaxWaitingStatus(invoiceRequestViewModel, invoiceModel);
                    }
                    else
                    {
                        invoiceCreateRequestViewModel = SetTaxesToFtlInvoiceModel(invoiceRequestViewModel, invoiceModel);
                    }

                    invoiceCreateRequestViewModel.FCMAppId = DriverDropViewModel.Driver.FCMAppId;
                    List<InvoiceCreateResponseViewModel> invoiceCreateResponses;
                    if (invoiceRequestViewModel.IsBuySellOrder)
                    {
                        invoiceCreateResponses = await GenerateBuySellInvoiceForMobile(invoiceRequestViewModel, invoiceCreateRequestViewModel, invoiceModel, mobileAssetDrops);
                    }
                    else
                    {
                        invoiceCreateResponses = await GenerateFtlMobileInvoice(invoiceRequestViewModel, invoiceCreateRequestViewModel, invoiceModel, DriverDropViewModel, mobileAssetDrops);
                    }

                    if (invoiceCreateResponses != null)
                    {
                        var invoiceCreateResponse = invoiceCreateResponses.First();
                        response.StatusCode = invoiceCreateResponse.StatusCode;
                        response.EntityId = invoiceCreateResponse.InvoiceId;
                        await SetMobileInvoiceCreatedPostEvents(invoiceRequestViewModel, invoiceModel, invoiceCreateResponses);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("InvoiceCreateDomain", "CreateFtlMobileInvoiceAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<FtlDropResponseModel> CreateMobileSplitLoadDraftDdtAsync(FtlDriverDropOrderViewModel DriverDropViewModel)
        {
            var response = new FtlDropResponseModel();

            try
            {
                if (!string.IsNullOrEmpty(DriverDropViewModel.TraceId) && Context.DataContext.Invoices.Any(t => t.TraceId == DriverDropViewModel.TraceId))
                {
                    response.Status = (int)Status.Success; //Do not set as Failed
                    response.Message = string.Format(Resource.valMessageAlreadyExist, Resource.lblTraceId);
                    return response;
                }

                if (DriverDropViewModel.Image != null)
                    await DriverDropViewModel.Image.UploadImageToAzureBlobService(ApplicationConstants.DropImageFileNamePrefix, BlobContainerType.InvoicePdfFiles);

                if (DriverDropViewModel.CustomerSignatureViewModel != null && DriverDropViewModel.CustomerSignatureViewModel.Image != null)
                    await DriverDropViewModel.CustomerSignatureViewModel.Image.UploadImageToAzureBlobService(ApplicationConstants.CustomerSignatureFileNamePrefix, BlobContainerType.InvoicePdfFiles);

                //SET iNVOICE STATUS AS DRAFT
                DriverDropViewModel.InvoiceStatusId = (int)InvoiceStatus.Draft;
                DriverDropViewModel.SplitLoadChainId = SetSplitLoadChainId(DriverDropViewModel.SplitLoadChainId, DriverDropViewModel.Driver.UserId);

                var invoiceRequestViewModel = await GetCreateFtlMobileInvoiceRequestViewModelAsync(DriverDropViewModel);
                //var invoiceModel = await GetMobileInvoiceViewModelAsync(mobileInvoiceCreateRequestViewModel, assetCount: 0);

                InvoiceModel invoiceModel = new InvoiceModel();
                SetMobileInputsToInvoiceModel(invoiceRequestViewModel, invoiceModel);

                await SetInvoiceNumberToInvoiceModel(invoiceModel);
                invoiceModel = await GetInvoiceViewModel(invoiceRequestViewModel, invoiceModel, 0);

                // DO NOT CALL PRICING SERVICE FOR DDT
                // Set fees with calculation to invoice model
                RemoveDryRunFeeFromManualInvoiceModel(invoiceRequestViewModel);
                ProcessInvoiceFuelFeesAndSetCalculatedValues(invoiceRequestViewModel, invoiceModel, invoiceModel.DropEndDate, 0);

                if (invoiceRequestViewModel.IsFTL && !invoiceRequestViewModel.IsDriverToUpdateBol)
                {
                    invoiceModel.WaitingFor = WaitingAction.BolDetails;
                    invoiceModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                }

                InvoiceCreateRequestViewModel invoiceCreateRequestViewModel;
                //CREATE DDT WAITINGOR TAX
                if (invoiceModel.InvoiceTypeId == (int)InvoiceType.MobileApp)
                {
                    invoiceCreateRequestViewModel = GetInvoiceCreateRequestViewModelWithTaxWaitingStatus(invoiceRequestViewModel, invoiceModel);
                }
                else
                {
                    invoiceCreateRequestViewModel = SetTaxesToFtlInvoiceModel(invoiceRequestViewModel, invoiceModel);
                }

                invoiceCreateRequestViewModel.FCMAppId = DriverDropViewModel.Driver.FCMAppId;
                invoiceCreateRequestViewModel.IsSplitLoad = DriverDropViewModel.IsSplitLoad;
                List<InvoiceCreateResponseViewModel> invoiceCreateResponses;

                invoiceCreateResponses = await GenerateFtlMobileInvoice(invoiceRequestViewModel, invoiceCreateRequestViewModel, invoiceModel, DriverDropViewModel, null);

                if (invoiceCreateResponses != null)
                {
                    var invoiceCreateResponse = invoiceCreateResponses.First();
                    response.Status = (int)invoiceCreateResponse.StatusCode;
                    response.SplitLoadChainId = invoiceCreateResponse.SplitLoadChainId;
                    await SetMobileInvoiceCreatedPostEvents(invoiceRequestViewModel, invoiceModel, invoiceCreateResponses);
                }

                if (response.Status == (int)Status.Success) // convert ddt's to invoice
                {
                    //update address status to complete
                    var trackableScheduleId = DriverDropViewModel.TrackableScheduleId == 0 ? null : DriverDropViewModel.TrackableScheduleId;
                    Context.DataContext.FuelDispatchLocations.Where(t => t.OrderId == DriverDropViewModel.OrderId
                                                                        && t.TrackableScheduleId == trackableScheduleId
                                                                        && t.LocationType == (int)LocationType.Drop
                                                                        && t.DropStatus == DropAddressStatus.InProgress && t.IsActive)
                                                            .ToList().ForEach(t => t.DropStatus = DropAddressStatus.Complete);
                    await Context.CommitAsync();

                    if (!DriverDropViewModel.IsSplitLoad)
                    {
                        var authDomain = new AuthenticationDomain(this);
                        var splitLoadDomain = new SplitLoadInvoiceDomain(this);
                        var userContext = await authDomain.GetUserContextAsync(DriverDropViewModel.Driver.UserId);
                        var status = await splitLoadDomain.CreateInvoicesFromMobileSplitLoadDraftDdts(userContext, DriverDropViewModel.SplitLoadChainId);
                        response.Status = (int)status.StatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SplitLoadInvoiceDomain", "CreateMobileSplitLoadDraftDdtAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<InvoiceCreateResponseViewModel> CreateSplitLoadDraftDdtAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var invoiceCreateResponse = new InvoiceCreateResponseViewModel();
            var status = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("InvoiceCreateDomain", "CreateSplitLoadDraftDdtAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel();

                    //Set for split invoice
                    manualInvoiceModel.SplitLoadChainId = SetSplitLoadChainId(manualInvoiceModel.SplitLoadChainId, userContext.Id);
                    manualInvoiceModel.SplitLoadSequence = (manualInvoiceModel.SplitLoadSequence ?? 0) + 1;

                    var invoiceRequestModel = await GetCreateManualInvoiceRequestViewModelAsync(manualInvoiceModel);
                    SetDropLocation(manualInvoiceModel, invoiceModel);

                    SetManualInputToInvoiceModel(invoiceRequestModel, invoiceModel);
                    var assetCount = manualInvoiceModel.Assets.Select(t => t.AssetName.ToLower()).Distinct().Count();
                    CheckRequiredImagesAndSetWaitingForImageAction(invoiceModel);

                    await SetInvoiceNumberToInvoiceModel(invoiceModel);
                    invoiceModel = await GetInvoiceViewModel(invoiceRequestModel, invoiceModel, assetCount);

                    // DO NOT CALL PRICING SERVICE FOR DDT
                    // Set fees with calculation to invoice model
                    RemoveDryRunFeeFromManualInvoiceModel(invoiceRequestModel);
                    ProcessInvoiceFuelFeesAndSetCalculatedValues(invoiceRequestModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                    //------------Set Invoice fees from manual invoice view model------------------------
                    var invoiceCreateRequestViewModel = SetTaxesToFtlInvoiceModel(invoiceRequestModel, invoiceModel);
                    List<InvoiceCreateResponseViewModel> invoiceCreateResponses;
                    if (invoiceRequestModel.IsBuySellOrder)
                    {
                        invoiceCreateResponses = await GenerateBuySellInvoice(invoiceRequestModel, invoiceCreateRequestViewModel, invoiceModel, manualInvoiceModel);
                    }
                    else
                    {
                        invoiceCreateResponses = await GenerateManualSplitDraftDdt(invoiceRequestModel, invoiceCreateRequestViewModel, invoiceModel, manualInvoiceModel);
                    }

                    invoiceCreateResponse = invoiceCreateResponses.First();
                    status.StatusCode = invoiceCreateResponse.StatusCode;
                    SetStatusMessage(manualInvoiceModel.InvoiceTypeId, invoiceModel.WaitingFor, status, invoiceCreateResponse.IsDtnUploaded);
                    invoiceCreateResponse.StatusMessage = status.StatusMessage;
                }
                catch (Exception ex)
                {
                    invoiceCreateResponse.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    LogManager.Logger.WriteException("InvoiceCreateDomain", "CreateSplitLoadDraftDdtAsync", ex.Message, ex);
                }
            }
            return invoiceCreateResponse;
        }

        public async Task<StatusViewModel> CreateManualFtlInvoiceAsync(UserContext UserContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("InvoiceCreateDomain", "CreateManualFtlInvoiceAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel();
                    var invoiceRequestModel = await GetCreateManualInvoiceRequestViewModelAsync(manualInvoiceModel);

                    SetManualInputToInvoiceModel(invoiceRequestModel, invoiceModel);
                    var assetCount = manualInvoiceModel.Assets.Select(t => t.AssetName.ToLower()).Distinct().Count();
                    await SetInvoiceNumberToInvoiceModel(invoiceModel);
                    invoiceModel = await GetInvoiceViewModel(invoiceRequestModel, invoiceModel, assetCount);
                    CheckRequiredImagesAndSetWaitingForImageAction(invoiceModel);
                    CheckForWaitingForApproval(invoiceModel, invoiceRequestModel.IsApprovalWorkflowEnabledForJob, invoiceRequestModel.InvoiceTypeId, invoiceRequestModel.InvoiceStatusId);
                    await CheckAndSetInvoiceExceptions(invoiceRequestModel, invoiceModel);

                    if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                    {
                        // Set invoice pricing to invoice model
                        await SetInvoicePricingToInvoiceModel(invoiceRequestModel, invoiceModel);
                        SetInvoiceAmountAndAllowanceToInvoiceModel(invoiceRequestModel, invoiceModel);
                    }

                    if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                    {
                        invoiceRequestModel.PricePerGallon = invoiceModel.PricePerGallon;
                        await CheckAndSetDuplicateInvoiceException(invoiceRequestModel, invoiceModel);
                    }

                    // Set fees with calculation to invoice model
                    RemoveDryRunFeeFromManualInvoiceModel(invoiceRequestModel);
                    ProcessInvoiceFuelFeesAndSetCalculatedValues(invoiceRequestModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                    var invoiceCreateRequestViewModel = SetTaxesToFtlInvoiceModel(invoiceRequestModel, invoiceModel);
                    List<InvoiceCreateResponseViewModel> invoiceCreateResponses;
                    if (invoiceRequestModel.IsBuySellOrder)
                    {
                        invoiceCreateResponses = await GenerateBuySellInvoice(invoiceRequestModel, invoiceCreateRequestViewModel, invoiceModel, manualInvoiceModel);
                    }
                    else
                    {
                        invoiceCreateResponses = await GenerateManualInvoice(invoiceRequestModel, invoiceCreateRequestViewModel, invoiceModel, manualInvoiceModel);
                    }

                    var invoiceCreateResponse = invoiceCreateResponses.First();
                    response.StatusCode = invoiceCreateResponse.StatusCode;
                    await SetManualInvoiceCreatedPostEvents(UserContext, invoiceRequestModel, invoiceModel, invoiceCreateResponses);
                    SetStatusMessage(manualInvoiceModel.InvoiceTypeId, invoiceModel.WaitingFor, response, invoiceCreateResponse.IsDtnUploaded, invoiceModel.DDTConversionReason);
                    SetStatusCustomMessage(invoiceCreateResponses, response);
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    LogManager.Logger.WriteException("InvoiceCreateDomain", "CreateManualFtlInvoiceAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> CreateBrokerSplitLoadInvoicesAsync(string splitLoadChainId)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("InvoiceCreateDomain", "CreateBrokerSplitLoadInvoicesAsync"))
            {
                try
                {
                    var invoiceDomain = new InvoiceDomain(this);
                    var parentInvoices = Context.DataContext.InvoiceXAdditionalDetails.Where(t => t.SplitLoadChainId == splitLoadChainId && t.Invoice.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t => new { t.InvoiceId, t.Invoice.Order.FuelRequest.Job.TimeZoneName }).ToList();
                    var splitInvoiceBolId = new List<DropdownDisplayItem>();
                    foreach (var item in parentInvoices.Select(t => t.InvoiceId))
                    {
                        var manualInvoiceModel = await invoiceDomain.GetManualSplitInvoiceForEditAsync(item);

                        InvoiceModel invoiceModel = new InvoiceModel();
                        invoiceModel.CreatedBy = manualInvoiceModel.userId;
                        var manualInvoiceCreateRequestViewModel = GetBrokerSplitInvoiceRequestViewModelAsync(manualInvoiceModel);

                        List<InvoiceCreateResponseViewModel> invoiceCreateResponses;
                        invoiceCreateResponses = await GenerateBrokerSplitInvoice(manualInvoiceCreateRequestViewModel, invoiceModel, manualInvoiceModel, splitInvoiceBolId);

                        foreach (var splitId in invoiceCreateResponses)
                        {
                            if (!splitInvoiceBolId.Any(t => t.Name == splitId.SplitLoadChainId) && splitId.BolDetailId.HasValue)
                            {
                                splitInvoiceBolId.Add(new DropdownDisplayItem { Id = splitId.BolDetailId.Value, Name = splitId.SplitLoadChainId });
                            }
                        }

                        var invoiceCreateResponse = invoiceCreateResponses.FirstOrDefault();
                        response.StatusCode = invoiceCreateResponse.StatusCode;
                        //await SetManualInvoiceCreatedPostEvents(UserContext, manualInvoiceCreateRequestViewModel, invoiceModel, invoiceCreateResponses);
                        SetStatusMessage(manualInvoiceModel.InvoiceTypeId, invoiceModel.WaitingFor, response, invoiceCreateResponse.IsDtnUploaded);
                    }
                    foreach (var item in splitInvoiceBolId)
                    {
                        await CheckForBrokerSplitLoadInvoiceAndGenerateStatement(item.Name, parentInvoices.FirstOrDefault().TimeZoneName);
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    LogManager.Logger.WriteException("InvoiceCreateDomain", "CreateBrokerSplitLoadInvoicesAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> CreateManualInvoiceAsync(UserContext UserContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("InvoiceCreateDomain", "CreateManualInvoiceAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel();
                    var invoiceRequestModel = await GetCreateManualInvoiceRequestViewModelAsync(manualInvoiceModel);

                    SetManualInputToInvoiceModel(invoiceRequestModel, invoiceModel);
                    var assetCount = manualInvoiceModel.Assets.Select(t => t.AssetName.ToLower()).Distinct().Count();
                    await SetInvoiceNumberToInvoiceModel(invoiceModel);
                    invoiceModel = await GetInvoiceViewModel(invoiceRequestModel, invoiceModel, assetCount);
                    CheckRequiredImagesAndSetWaitingForImageAction(invoiceModel);
                    CheckForWaitingForApproval(invoiceModel, invoiceRequestModel.IsApprovalWorkflowEnabledForJob, invoiceRequestModel.InvoiceTypeId, invoiceRequestModel.InvoiceStatusId);

                    if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                    {
                        // Set invoice pricing to invoice model
                        await SetInvoicePricingToInvoiceModel(invoiceRequestModel, invoiceModel);
                        SetInvoiceAmountAndAllowanceToInvoiceModel(invoiceRequestModel, invoiceModel);
                    }

                    if (invoiceModel.WaitingFor == WaitingAction.Nothing)
                    {
                        invoiceRequestModel.PricePerGallon = invoiceModel.PricePerGallon;
                        await CheckAndSetDuplicateInvoiceException(invoiceRequestModel, invoiceModel);
                    }

                    // Set fees with calculation to invoice model
                    RemoveDryRunFeeFromManualInvoiceModel(invoiceRequestModel);
                    ProcessInvoiceFuelFeesAndSetCalculatedValues(invoiceRequestModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                    //------------Set Invoice fees from manual invoice view model------------------------
                    var invoiceCreateRequestViewModel = SetTaxesToInvoiceModel(invoiceRequestModel, invoiceModel);
                    List<InvoiceCreateResponseViewModel> invoiceCreateResponses;
                    if (invoiceRequestModel.IsBuySellOrder)
                    {
                        invoiceCreateResponses = await GenerateBuySellInvoice(invoiceRequestModel, invoiceCreateRequestViewModel, invoiceModel, manualInvoiceModel);
                    }
                    else
                    {
                        invoiceCreateResponses = await GenerateManualInvoice(invoiceRequestModel, invoiceCreateRequestViewModel, invoiceModel, manualInvoiceModel);
                    }

                    var invoiceCreateResponse = invoiceCreateResponses.First();
                    response.StatusCode = invoiceCreateResponse.StatusCode;
                    await SetManualInvoiceCreatedPostEvents(UserContext, invoiceRequestModel, invoiceModel, invoiceCreateResponses);
                    SetStatusMessage(manualInvoiceModel.InvoiceTypeId, invoiceModel.WaitingFor, response, invoiceCreateResponse.IsDtnUploaded, invoiceModel.DDTConversionReason);
                    SetStatusCustomMessage(invoiceCreateResponses, response);
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    LogManager.Logger.WriteException("InvoiceCreateDomain", "CreateManualInvoiceAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> CreatePartialCreditManualInvoiceAsync(UserContext userContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("InvoiceCreateDomain", "CreatePartialCreditManualInvoiceAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel();
                    manualInvoiceModel.InvoiceTypeId = (int)InvoiceType.PartialCredit;
                    var invoiceRequestModel = await GetCreateManualInvoiceRequestViewModelAsync(manualInvoiceModel);

                    SetManualInputToInvoiceModel(invoiceRequestModel, invoiceModel);
                    var assetCount = manualInvoiceModel.Assets.Select(t => t.AssetName.ToLower()).Distinct().Count();
                    await SetInvoiceNumberToInvoiceModel(invoiceModel);
                    invoiceModel = await GetInvoiceViewModel(invoiceRequestModel, invoiceModel, assetCount);
                    invoiceModel.InvoiceTypeId = (int)InvoiceType.PartialCredit;
                    invoiceModel.AdditionalDetail.OriginalInvoiceId = manualInvoiceModel.InvoiceId;
                    invoiceModel.AdditionalDetail.OriginalInvoiceHeaderId = manualInvoiceModel.InvoiceHeaderId;
                    invoiceModel.WaitingFor = WaitingAction.Nothing;

                    var invoice = Context.DataContext.Invoices.Where(t => t.Id == manualInvoiceModel.InvoiceId).Select(t => new { t.PoNumber }).FirstOrDefault();
                    if (invoice != null)
                    {
                        invoiceModel.PoNumber = invoice.PoNumber;
                    }

                    //CheckRequiredImagesAndSetWaitingForImageAction(manualInvoiceModel.IsFTL, invoiceModel);

                    // Set invoice pricing to invoice model
                    invoiceModel.PricePerGallon = manualInvoiceModel.InvoiceCreationPricePerGallon;
                    invoiceModel.RackPrice = manualInvoiceModel.TerminalPrice;
                    invoiceModel.BasicAmount = Math.Round(invoiceModel.DroppedGallons * invoiceModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                    invoiceModel.Id = manualInvoiceModel.InvoiceId;

                    // Set fees with calculation to invoice model
                    ProcessInvoiceFuelFeesAndSetCalculatedValues(invoiceRequestModel, invoiceModel, invoiceModel.DropEndDate, assetCount, false);

                    var invoiceCreateRequestViewModel = SetTaxesToPartialCreditInvoiceModel(invoiceRequestModel, invoiceModel, manualInvoiceModel);
                    invoiceRequestModel.IsBrokeredChainOrder = false;
                    List<InvoiceCreateResponseViewModel> invoiceCreateResponses;

                    invoiceCreateResponses = await CreatePartialCreditInvoiceAsync(invoiceCreateRequestViewModel, manualInvoiceModel);

                    var invoiceCreateResponse = invoiceCreateResponses.First();
                    response.StatusCode = invoiceCreateResponse.StatusCode;

                    var notificationDomain = new NotificationDomain(this);
                    await notificationDomain.AddNotificationEventAsync(EventType.CreditInvoiceCreated, invoiceCreateResponse.InvoiceHeaderId, userContext.Id);

                    var newsfeedDomain = new NewsfeedDomain(this);
                    var newsfeedRequestModel = GetManualInvoiceCreatedNewsfeedModel(invoiceRequestModel, invoiceCreateResponse, invoiceModel);
                    if (newsfeedRequestModel != null)
                    {
                        newsfeedRequestModel.OriginalInvoiceNumber = manualInvoiceModel.DisplayInvoiceNumber;
                    }
                    await newsfeedDomain.SetCreditInvoiceCreatedNewsfeed(userContext, newsfeedRequestModel);

                    SetStatusMessage(manualInvoiceModel.InvoiceTypeId, invoiceModel.WaitingFor, response, invoiceCreateResponse.IsDtnUploaded);
                    SetStatusCustomMessage(invoiceCreateResponses, response);
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    LogManager.Logger.WriteException("InvoiceCreateDomain", "CreatePartialCreditManualInvoiceAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<InvoiceCreateResponseViewModel> CreateSplitManualFtlInvoiceAsync(UserContext UserContext, ManualInvoiceViewModel manualInvoiceModel)
        {
            
            var invoiceCreateResponse = new InvoiceCreateResponseViewModel();
            using (var tracer = new Tracer("InvoiceCreateDomain", "CreateSplitManualFtlInvoiceAsync"))
            {
                try
                {
                    InvoiceModel invoiceModel = new InvoiceModel();
                    var invoiceRequestModel = await GetCreateManualInvoiceRequestViewModelAsync(manualInvoiceModel);

                    SetManualInputToInvoiceModel(invoiceRequestModel, invoiceModel);
                    var assetCount = manualInvoiceModel.Assets.Select(t => t.AssetName.ToLower()).Distinct().Count();
                    await SetInvoiceNumberToInvoiceModel(invoiceModel);
                    invoiceModel = await GetInvoiceViewModel(invoiceRequestModel, invoiceModel, assetCount);
                    CheckRequiredImagesAndSetWaitingForImageAction(invoiceModel);
                    CheckForWaitingForApproval(invoiceModel, invoiceRequestModel.IsApprovalWorkflowEnabledForJob, invoiceRequestModel.InvoiceTypeId, invoiceRequestModel.InvoiceStatusId);

                    // Set invoice pricing to invoice model
                    await SetInvoicePricingToInvoiceModel(invoiceRequestModel, invoiceModel);
                    SetInvoiceAmountAndAllowanceToInvoiceModel(invoiceRequestModel, invoiceModel);

                    // Set fees with calculation to invoice model
                    RemoveDryRunFeeFromManualInvoiceModel(invoiceRequestModel);
                    ProcessInvoiceFuelFeesAndSetCalculatedValues(invoiceRequestModel, invoiceModel, invoiceModel.DropEndDate, assetCount);

                    //------------Set Invoice fees from manual invoice view model------------------------
                    var invoiceCreateRequestViewModel = SetTaxesToFtlInvoiceModel(invoiceRequestModel, invoiceModel);
                    SetDropLocation(manualInvoiceModel, invoiceModel);
                    List<InvoiceCreateResponseViewModel> invoiceCreateResponses;
                    if (invoiceRequestModel.IsBuySellOrder)
                    {
                        invoiceCreateResponses = await GenerateBuySellInvoice(invoiceRequestModel, invoiceCreateRequestViewModel, invoiceModel, manualInvoiceModel);
                    }
                    else
                    {
                        invoiceCreateResponses = await GenerateManualInvoice(invoiceRequestModel, invoiceCreateRequestViewModel, invoiceModel, manualInvoiceModel);
                    }

                    invoiceCreateResponse = invoiceCreateResponses.First();
                    await SetManualInvoiceCreatedPostEvents(UserContext, invoiceRequestModel, invoiceModel, invoiceCreateResponses);
                    await EditBillingStatement(invoiceModel.AdditionalDetail.SplitLoadChainId, invoiceRequestModel.TimeZoneName, UserContext.CompanyId);
                }
                catch (Exception ex)
                {
                    invoiceCreateResponse.StatusCode = Status.Failed;
                    invoiceCreateResponse.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    LogManager.Logger.WriteException("InvoiceCreateDomain", "CreateSplitManualFtlInvoiceAsync", ex.Message, ex);
                }
            }
            return invoiceCreateResponse;
        }

        public async Task<List<string>> CreateBrokerSplitInvoiceFromQueueService(CreateBrokerSplitInvoiceQueueViewModel viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("InvoiceCreateDomain", "CreateBrokerSplitInvoiceFromQueueService"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    var status = await CreateBrokerSplitLoadInvoicesAsync(viewModel.SplitLoadChainId);
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("InvoiceCreateDomain", "CreateBrokerSplitInvoiceFromQueueService", ex.Message, ex);
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

        private async Task CheckForBrokerSplitLoadInvoiceAndGenerateStatement(string splitLoadChainId, string timeZoneName)
        {
            var splitLoadInvoices = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == splitLoadChainId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive)
                .Select(t => new { Invoice = t, t.Order.AcceptedCompanyId }).ToListAsync();
            if (!splitLoadInvoices.Any(t => t.Invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || t.Invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
            {
                BillingStatementDomain statementDomain = new BillingStatementDomain(this);
                var acceptedCompanyId = splitLoadInvoices.FirstOrDefault().AcceptedCompanyId;
                var invoices = splitLoadInvoices.Select(t => t.Invoice).ToList();
                await statementDomain.GeneateBillingStatementForSplitLoadInvoice(invoices, timeZoneName, acceptedCompanyId);
            }
        }

        public async Task<StatusViewModel> CreateInvoiceFromDraftDdtAsync(UserContext UserContext, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("InvoiceCreateDomain", "CreateInvoiceFromDraftDdtAsync"))
            {
                try
                {

                    InvoiceModel invoiceModel = new InvoiceModel();
                    var manualInvoiceCreateRequestViewModel = await GetCreateManualInvoiceFromDraftDdtViewModelAsync(manualInvoiceViewModel, invoiceModel);

                    //------------Set Invoice fees from manual invoice view model------------------------
                    var invoiceCreateRequestViewModel = SetTaxesToInvoiceModel(manualInvoiceCreateRequestViewModel, invoiceModel);

                    List<InvoiceCreateResponseViewModel> invoiceCreateResponses;
                    if (manualInvoiceCreateRequestViewModel.IsBuySellOrder)
                    {
                        invoiceCreateResponses = await GenerateBuySellInvoice(manualInvoiceCreateRequestViewModel, invoiceCreateRequestViewModel, invoiceModel, manualInvoiceViewModel);
                    }
                    else
                    {
                        invoiceCreateResponses = await GenerateDraftDdtManualInvoice(manualInvoiceCreateRequestViewModel, invoiceCreateRequestViewModel, invoiceModel, manualInvoiceViewModel);
                    }

                    var invoiceCreateResponse = invoiceCreateResponses.First();
                    manualInvoiceViewModel.InvoiceId = invoiceCreateResponse.InvoiceId;

                    response.StatusCode = invoiceCreateResponse.StatusCode;

                    await SetManualInvoiceCreatedPostEvents(UserContext, manualInvoiceCreateRequestViewModel, invoiceModel, invoiceCreateResponses);
                    SetStatusMessage(invoiceModel.InvoiceTypeId, invoiceModel.WaitingFor, response, invoiceCreateResponse.IsDtnUploaded);
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    LogManager.Logger.WriteException("InvoiceCreateDomain", "CreateInvoiceFromDraftDdtAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private async Task<List<InvoiceCreateResponseViewModel>> GenerateMobileInvoice(MobileInvoiceCreateRequestViewModel mobileInvoiceCreateRequestViewModel, InvoiceCreateRequestViewModel invoiceCreateRequestViewModel, InvoiceModel invoiceModel, DriverDropOrderViewModel DriverDropViewModel, List<AssetDrop> mobileAssetDrops)
        {
            var brokeredinvoiceCreateRequestModels = new List<InvoiceCreateRequestViewModel>();
            if (mobileInvoiceCreateRequestViewModel.IsBrokeredChainOrder && invoiceModel.StatusId != (int)InvoiceStatus.Draft)
            {
                invoiceModel.BrokeredChainId = GetBrokeredChainId(invoiceModel.BrokeredChainId, invoiceModel.CreatedBy);
                brokeredinvoiceCreateRequestModels = await GetMobileInvoiceModelForBrokers(DriverDropViewModel, mobileInvoiceCreateRequestViewModel, mobileAssetDrops, invoiceModel);
                UpdateInvoiceStatus(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels);
            }
            var invoiceCreateResponses = await CreateMobileInvoiceAsync(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels, mobileAssetDrops);
            return invoiceCreateResponses;
        }

        private async Task<List<InvoiceCreateResponseViewModel>> GenerateFtlMobileInvoice(MobileInvoiceCreateRequestViewModel mobileInvoiceCreateRequestViewModel, InvoiceCreateRequestViewModel invoiceCreateRequestViewModel, InvoiceModel invoiceModel, FtlDriverDropOrderViewModel DriverDropViewModel, List<AssetDrop> mobileAssetDrops)
        {
            var brokeredinvoiceCreateRequestModels = new List<InvoiceCreateRequestViewModel>();
            if (mobileInvoiceCreateRequestViewModel.IsBrokeredChainOrder && invoiceModel.StatusId != (int)InvoiceStatus.Draft)
            {
                invoiceModel.BrokeredChainId = GetBrokeredChainId(invoiceModel.BrokeredChainId, invoiceModel.CreatedBy);
                brokeredinvoiceCreateRequestModels = await GetFtlMobileInvoiceModelForBrokers(DriverDropViewModel, mobileInvoiceCreateRequestViewModel, mobileAssetDrops, invoiceModel);
                UpdateInvoiceStatus(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels);
            }
            var invoiceCreateResponses = await CreateFtlMobileInvoiceAsync(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels, mobileAssetDrops);
            return invoiceCreateResponses;
        }

        private async Task<List<InvoiceCreateResponseViewModel>> GenerateManualInvoice(ManualInvoiceCreateRequestViewModel manualInvoiceCreateRequestViewModel, InvoiceCreateRequestViewModel invoiceCreateRequestViewModel, InvoiceModel invoiceModel, ManualInvoiceViewModel invoiceViewModel)
        {
            var brokeredinvoiceCreateRequestModels = new List<InvoiceCreateRequestViewModel>();
            if (manualInvoiceCreateRequestViewModel.IsBrokeredChainOrder)
            {
                invoiceModel.BrokeredChainId = GetBrokeredChainId(invoiceModel.BrokeredChainId, invoiceModel.CreatedBy);
                brokeredinvoiceCreateRequestModels = await GetManualInvoiceModelForBrokers(invoiceViewModel, manualInvoiceCreateRequestViewModel, invoiceModel);
                UpdateInvoiceStatus(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels);
            }
            var invoiceCreateResponses = await CreateManualInvoiceAsync(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels, invoiceViewModel);
            return invoiceCreateResponses;
        }

        private async Task<List<InvoiceCreateResponseViewModel>> GenerateBrokerSplitInvoice(ManualInvoiceCreateRequestViewModel manualInvoiceCreateRequestViewModel, InvoiceModel invoiceModel, ManualInvoiceViewModel invoiceViewModel, List<DropdownDisplayItem> SplitInvoiceBolId)
        {
            var brokeredinvoiceCreateRequestModels = new List<InvoiceCreateRequestViewModel>();
            invoiceModel.BrokeredChainId = GetBrokeredChainId(invoiceModel.BrokeredChainId, invoiceModel.CreatedBy);
            brokeredinvoiceCreateRequestModels = await GetBrokerSplitInvoiceModel(invoiceViewModel, manualInvoiceCreateRequestViewModel, invoiceModel, SplitInvoiceBolId);

            var imageId = invoiceViewModel.InvoiceImage?.Id; //prev invoiceImagId
            var signImageId = invoiceViewModel.SignatureImage?.Id; //prev invoiceImagId
            var assetDropModel = new List<AssetDropModel>(); // empty assets for FTL
            var invoiceCreateResponses = new List<InvoiceCreateResponseViewModel>();

            //update broker chain id to parent invoice
            var parentInvoice = await Context.DataContext.Invoices.Where(t => t.Id == invoiceViewModel.InvoiceId).FirstOrDefaultAsync();
            if (parentInvoice != null)
            {
                parentInvoice.BrokeredChainId = invoiceModel.BrokeredChainId;
                await Context.CommitAsync();
            }

            //using (var transaction = Context.DataContext.Database.BeginTransaction())
            //{
            invoiceCreateResponses = await CreateManualInvoicesForBrokers(brokeredinvoiceCreateRequestModels, assetDropModel, imageId, signImageId);
            //}
            return invoiceCreateResponses;
        }

        private async Task<List<InvoiceCreateResponseViewModel>> GenerateManualSplitDraftDdt(ManualInvoiceCreateRequestViewModel manualInvoiceCreateRequestViewModel, InvoiceCreateRequestViewModel invoiceCreateRequestViewModel, InvoiceModel invoiceModel, ManualInvoiceViewModel invoiceViewModel)
        {
            var brokeredinvoiceCreateRequestModels = new List<InvoiceCreateRequestViewModel>();
            //if (manualInvoiceCreateRequestViewModel.IsBrokeredChainOrder)
            //{
            //    invoiceModel.BrokeredChainId = GetBrokeredChainId(invoiceModel.BrokeredChainId, invoiceModel.CreatedBy);
            //    brokeredinvoiceCreateRequestModels = await GetManualInvoiceModelForBrokers(invoiceViewModel, manualInvoiceCreateRequestViewModel, invoiceModel);
            //    UpdateInvoiceStatus(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels);
            //}
            var invoiceCreateResponses = await CreateManualSplitDraftDdtAsync(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels, invoiceViewModel);
            return invoiceCreateResponses;
        }

        private async Task<List<InvoiceCreateResponseViewModel>> GenerateDraftDdtManualInvoice(ManualInvoiceCreateRequestViewModel manualInvoiceCreateRequestViewModel, InvoiceCreateRequestViewModel invoiceCreateRequestViewModel, InvoiceModel invoiceModel, ManualInvoiceViewModel invoiceViewModel)
        {
            var brokeredinvoiceCreateRequestModels = new List<InvoiceCreateRequestViewModel>();
            if (manualInvoiceCreateRequestViewModel.IsBrokeredChainOrder)
            {
                invoiceModel.BrokeredChainId = GetBrokeredChainId(invoiceModel.BrokeredChainId, invoiceModel.CreatedBy);
                brokeredinvoiceCreateRequestModels = await GetDraftDdtInvoiceModelForBrokers(invoiceViewModel, manualInvoiceCreateRequestViewModel, invoiceModel);
                UpdateInvoiceStatus(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels);
            }
            var invoiceCreateResponses = await CreateDraftDdtInvoiceAsync(invoiceCreateRequestViewModel, brokeredinvoiceCreateRequestModels, invoiceViewModel);
            return invoiceCreateResponses;
        }

        private async Task<List<InvoiceCreateResponseViewModel>> GenerateBuySellInvoiceForMobile(MobileInvoiceCreateRequestViewModel mobileInvoiceCreateRequestViewModel, InvoiceCreateRequestViewModel invoiceCreateRequestViewModel, InvoiceModel sellPriceInvoiceModel, List<AssetDrop> mobileAssetDrops)
        {   
            var buyPriceInvoiceRequestModel = mobileInvoiceCreateRequestViewModel.Clone();
            buyPriceInvoiceRequestModel.IsBuyPrice = true;

            InvoiceModel buyPriceInvoiceModel = sellPriceInvoiceModel.Clone();
            buyPriceInvoiceModel.IsBuyPriceInvoice = true;
            SetInvoiceAmountAndAllowanceToInvoiceModel(buyPriceInvoiceRequestModel, buyPriceInvoiceModel);

            // Additional detail will change in case of buy sell invoice
            await SetInvoiceAdditionDetailToInvoiceModel(buyPriceInvoiceModel, 0, buyPriceInvoiceRequestModel.OrderId);
            buyPriceInvoiceModel.AssetDrops = GetMobileAssetDropsForBrokers(mobileAssetDrops, buyPriceInvoiceModel.OrderId.Value);

            var invoiceNumber = await GenerateInvoiceNumber();
            buyPriceInvoiceModel.InvoiceNumberId = invoiceNumber.Id;
            buyPriceInvoiceModel.DisplayInvoiceNumber = invoiceNumber.Number;
            if (buyPriceInvoiceModel.AdditionalDetail == null) buyPriceInvoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
            buyPriceInvoiceModel.AdditionalDetail.Notes = mobileInvoiceCreateRequestViewModel.AdditionalDetail?.Notes;

            if (buyPriceInvoiceModel.TaxDetails != null && buyPriceInvoiceModel.TaxDetails.AvaTaxDetails != null)
                buyPriceInvoiceModel.TaxDetails.AvaTaxDetails.Clear();

            var buyerInvoiceCreateRequestModel = SetTaxesToInvoiceModel(buyPriceInvoiceRequestModel, buyPriceInvoiceModel);
            var invoiceCreateResponses = await CreateBuySellInvoiceForMobileAsync(invoiceCreateRequestViewModel, buyerInvoiceCreateRequestModel, mobileAssetDrops);
            return invoiceCreateResponses;
        }

        private async Task<List<InvoiceCreateResponseViewModel>> GenerateBuySellInvoice(ManualInvoiceCreateRequestViewModel manualInvoiceCreateRequestViewModel, InvoiceCreateRequestViewModel invoiceCreateRequestViewModel, InvoiceModel sellPriceInvoiceModel, ManualInvoiceViewModel invoiceView)
        {   
            var buyPriceInvoiceRequestModel = manualInvoiceCreateRequestViewModel.Clone();
            buyPriceInvoiceRequestModel.IsBuyPrice = true;

            InvoiceModel buyPriceInvoiceModel = sellPriceInvoiceModel.Clone();
            buyPriceInvoiceModel.IsBuyPriceInvoice = true;
            SetInvoiceAmountAndAllowanceToInvoiceModel(buyPriceInvoiceRequestModel, buyPriceInvoiceModel);

            // Additional detail will change in case of buy sell invoice
            await SetInvoiceAdditionDetailToInvoiceModel(buyPriceInvoiceModel, 0, buyPriceInvoiceRequestModel.OrderId);

            var invoiceNumber = await GenerateInvoiceNumber();
            buyPriceInvoiceModel.InvoiceNumberId = invoiceNumber.Id;
            buyPriceInvoiceModel.DisplayInvoiceNumber = invoiceNumber.Number;
            if (buyPriceInvoiceModel.AdditionalDetail == null) buyPriceInvoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
            buyPriceInvoiceModel.AdditionalDetail.Notes = manualInvoiceCreateRequestViewModel.AdditionalDetail?.Notes;

            if (buyPriceInvoiceModel.TaxDetails != null && buyPriceInvoiceModel.TaxDetails.AvaTaxDetails != null)
                buyPriceInvoiceModel.TaxDetails.AvaTaxDetails.Clear();

            InvoiceCreateRequestViewModel buyerInvoiceCreateRequestModel;
            if (invoiceView.IsFTL)
            {
                buyerInvoiceCreateRequestModel = SetTaxesToFtlInvoiceModel(buyPriceInvoiceRequestModel, buyPriceInvoiceModel);
            }
            else
            {
                buyerInvoiceCreateRequestModel = SetTaxesToInvoiceModel(buyPriceInvoiceRequestModel, buyPriceInvoiceModel);
            }
            var invoiceCreateResponses = await CreateBuySellInvoiceAsync(invoiceCreateRequestViewModel, buyerInvoiceCreateRequestModel, invoiceView);
            return invoiceCreateResponses;
        }

        private void UpdateInvoiceStatus(InvoiceCreateRequestViewModel invoiceCreateRequestViewModel, List<InvoiceCreateRequestViewModel> brokeredinvoiceCreateRequestModels)
        {
            if (invoiceCreateRequestViewModel.InvoiceModel.WaitingFor == WaitingAction.CustomerApproval)
            {
                var invoices = new List<InvoiceCreateRequestViewModel>();
                brokeredinvoiceCreateRequestModels.ForEach(t => invoices.Add(t));
                invoices.Add(invoiceCreateRequestViewModel);
                foreach (var invoice in invoices)
                {
                    if (invoices.Any(t => t.SupplierCompanyId == invoice.BuyerCompanyId && t.OrderId != invoice.OrderId))
                    {
                        invoice.InvoiceModel.StatusId = (int)InvoiceStatus.Received;
                    }
                }
            }
        }

        private async Task SetMobileInvoiceCreatedPostEvents(MobileInvoiceCreateRequestViewModel mobileInvoiceCreateRequestViewModel, InvoiceModel invoiceModel, List<InvoiceCreateResponseViewModel> invoiceCreateResponses)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            var invoiceCreateResponse = invoiceCreateResponses.First();
            if (invoiceCreateResponse.StatusCode == Status.Success)
            {
                if (mobileInvoiceCreateRequestViewModel.InvoiceStatusId == (int)InvoiceStatus.Draft)
                {
                    var newsfeedModel = GetDigitalDropTicketDraftNewsfeedModel(invoiceCreateResponse, invoiceModel.CreatedDate);
                    await newsfeedDomain.SetSystemDigitalDropTicketDraftCreatedNewsfeed(newsfeedModel);
                    //await appMessageDomain.SendDraftDDTMessage(order, invoiceViewModel);
                }
                else if (invoiceModel.WaitingFor != WaitingAction.Nothing)
                {
                    var newsfeedModel = GetDigitalDropTicketApprovalNewsfeedModel(invoiceModel, invoiceCreateResponse, mobileInvoiceCreateRequestViewModel.ApprovalUserId);
                    var newsfeedType = GetWaitingForNewsfeedType(invoiceModel.WaitingFor);
                    await newsfeedDomain.SetDriverDroppedInvoiceCreatedWaitingForNewsfeed(newsfeedModel, newsfeedType);
                }
                else
                {
                    var newsfeedModel = GetSystemInvoiceCreatedNewsfeedModel(mobileInvoiceCreateRequestViewModel, invoiceModel, invoiceCreateResponse);
                    await newsfeedDomain.SetSystemInvoiceCreatedNewsfeed(newsfeedModel);
                }
            }
            foreach (var item in invoiceCreateResponses.Where(t => t.IsOrderAutoClosed))
            {
                var newsfeedModel = GetSystemOrderAutoClosedNewsfeedViewModel(mobileInvoiceCreateRequestViewModel, item);
                await newsfeedDomain.SetSystemOrderAutoClosedNewsfeed(newsfeedModel);
            }

            //var queueDomain = new QueueMessageDomain();
            foreach (var item in invoiceCreateResponses.Where(t => t.StatusCode == Status.Success))
            {
                if (item.WaitingFor != WaitingAction.AvalaraTax && item.SupplierPrefferedInvoiceTypeId.HasValue && item.SupplierPrefferedInvoiceTypeId.Value != (int)InvoiceType.MobileApp)
                {
                    await AddNotificationEventForMobileInvoice(item);
                }
                //AddWebNotificationEventForMobileInvoice(item, queueDomain);
            }
        }

        private void SetTaxExemptLicences(InvoiceCreateViewModel viewModel, dynamic order)
        {
            if (order.Job.BuyerTaxExemptLicence != null)
            {
                viewModel.BuyerCustomId = order.Job.BuyerTaxExemptLicence.EntityCustomId;
            }
            if (order.SupplierTaxExemptLicence != null && order.IsEndSupplier)
            {
                viewModel.SellerCustomId = order.SupplierTaxExemptLicence.EntityCustomId;
            }
        }

        private async Task SetManualInvoiceCreatedPostEvents(UserContext userContext, ManualInvoiceCreateRequestViewModel requestViewModel, InvoiceModel invoiceModel, List<InvoiceCreateResponseViewModel> invoiceCreateResponses)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            var invoiceCreateResponse = invoiceCreateResponses.First();
            if (invoiceCreateResponse.StatusCode == Status.Success)
            {
                if ((requestViewModel.CreationMethod == CreationMethod.BulkUploaded || requestViewModel.CreationMethod == CreationMethod.APIUpload)
                    && requestViewModel.PricingTypeId == (int)PricingType.Suppliercost 
                    && (requestViewModel.CurrentCost != requestViewModel.SupplierCost || requestViewModel.SupplierCostTypeId != (int)SupplierCostTypes.SupplierCost))
                {
                    UpdateCurrentCostViewModel updateCurrentCost = new UpdateCurrentCostViewModel() { CountryId = requestViewModel.CountryId, FuelRequestId = requestViewModel.FuelRequestPricingDetail.FuelRequestId, CurrencyType = requestViewModel.Currency, FuelCost = requestViewModel.CurrentCost.Value, FuelTypeId = requestViewModel.FuelTypeId, IsGlobalCost = false, OrderId = invoiceCreateResponse.OrderId, OriginalFuelCost = requestViewModel.SupplierCost.Value, PriceRequestDetailId = requestViewModel.FuelRequestPricingDetail.RequestPriceDetailId, SupplierFuelCostTypeId = requestViewModel.SupplierCostTypeId.Value };
                    updateCurrentCost.TfxFuelTypeId = Context.DataContext.MstProducts.Where(t => t.Id == updateCurrentCost.FuelTypeId).Select(t => t.TfxProductId).FirstOrDefault() ?? 0;
                    var costUpdateStatus = await new CurrentCostDomain(this).UpdateSupplierCostForOrder(userContext, updateCurrentCost);
                }
                switch (invoiceModel.WaitingFor)
                {
                    case WaitingAction.UpdatedPrice:
                    case WaitingAction.AvalaraTax:
                        var newsfeedViewModel = GetDigitalDropTicketApprovalNewsfeedModel(invoiceModel, invoiceCreateResponse, requestViewModel.ApprovalUserId);
                        NewsfeedEvent newsfeedEvent = invoiceModel.WaitingFor == WaitingAction.UpdatedPrice ? NewsfeedEvent.SupplierCreatedDDTWaitingForUpdatedPrice : NewsfeedEvent.DDTCreatedWaitingForTaxes;
                        await newsfeedDomain.SetDDTWaitingForNewsfeed(userContext, newsfeedViewModel, newsfeedEvent);
                        break;

                    case WaitingAction.CustomerApproval:
                        if (requestViewModel.IsApprovalWorkflowEnabledForJob && requestViewModel.ApprovalUserId > 0 &&
                            requestViewModel.ApprovalUserOnboardedType != (int)OnboardedType.ThirdPartyOrderOnboarded &&
                            invoiceModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual)
                        {
                            var approvalViewModel = GetApprovalWorkflowNewsfeedModel(userContext, requestViewModel, invoiceModel, invoiceCreateResponse);
                            approvalViewModel.IsBrokeredOrder = invoiceCreateResponses.Any(t => t.SupplierCompanyId == invoiceCreateResponse.BuyerCompanyId);
                            await newsfeedDomain.SetApprovalWorkflowEnabledNewsFeeds(approvalViewModel);
                        }
                        break;

                    default:
                        var newsfeedRequestModel = GetManualInvoiceCreatedNewsfeedModel(requestViewModel, invoiceCreateResponse, invoiceModel);
                        await newsfeedDomain.SetManualInvoiceCreatedNewsfeed(userContext, newsfeedRequestModel);
                        break;
                }
            }
            if (requestViewModel.DeliveryTypeId != (int)DeliveryType.OneTimeDelivery)
            {
                foreach (var item in invoiceCreateResponses.Where(t => t.IsOrderAutoClosed))
                {
                    var newsfeedModel = GetSystemOrderAutoClosedNewsfeedViewModel(requestViewModel, item);
                    await newsfeedDomain.SetSystemOrderAutoClosedNewsfeed(newsfeedModel);
                }
            }
            //var queueDomain = new QueueMessageDomain();
            //foreach (var item in invoiceCreateResponses.Where(t => t.StatusCode == Status.Success))
            //{
            //    await AddNotificationEventForManualInvoice(item, invoiceModel);
            //    AddWebNotificationEventForMobileInvoice(item, queueDomain);
            //}
        }

        //private void AddWebNotificationEventForMobileInvoice(InvoiceCreateResponseViewModel invoiceCreateResponse, QueueMessageDomain queueDomain)
        //{
        //    if (!IsDigitalDropTicket(invoiceCreateResponse.InvoiceTypeId))
        //    {
        //        var queueRequest = GetEnqueueMessageRequestViewModel(invoiceCreateResponse);
        //        var queueId = queueDomain.EnqeueMessage(queueRequest);
        //        //MIGHT BE USE QUEUEID FOR  reporting purpose
        //    }
        //}

        private async Task AddNotificationEventForMobileInvoice(InvoiceCreateResponseViewModel invoiceModel)
        {
            var notificationEvent = EventType.InvoiceCreatedViaMobileDrop;
            if (invoiceModel.WaitingFor == WaitingAction.UpdatedPrice)
            {
                notificationEvent = EventType.DdtCreatedAsInvoiceIsWaitingForUpdatedPrice;
            }
            else if (invoiceModel.WaitingFor == WaitingAction.AvalaraTax)
            {
                notificationEvent = EventType.DDTCreateAsInvoiceWaitingForTaxes;
            }
            else if (invoiceModel.WaitingFor == WaitingAction.CustomerApproval)
            {
                notificationEvent = EventType.InvoiceCreatedApprovalWorkflow;
            }
            NotificationDomain notificationDomain = new NotificationDomain(this);
            await notificationDomain.AddNotificationEventAsync(notificationEvent, invoiceModel.InvoiceHeaderId, invoiceModel.OrderAcceptedBy);
        }

        private async Task AddNotificationEventForManualInvoice(InvoiceCreateResponseViewModel invoiceModel, InvoiceModel invoiceViewModel)
        {
            var notificationEvent = EventType.InvoiceCreated;
            if (invoiceModel.WaitingFor == WaitingAction.UpdatedPrice)
            {
                notificationEvent = EventType.DdtCreatedAsInvoiceIsWaitingForUpdatedPrice;
            }
            else if (invoiceModel.WaitingFor == WaitingAction.AvalaraTax)
            {
                notificationEvent = EventType.DDTCreateAsInvoiceWaitingForTaxes;
            }
            else if (invoiceModel.WaitingFor == WaitingAction.CustomerApproval)
            {
                notificationEvent = EventType.InvoiceCreatedApprovalWorkflow;
            }
            NotificationDomain notificationDomain = new NotificationDomain(this);
            await notificationDomain.AddNotificationEventAsync(notificationEvent, invoiceModel.InvoiceHeaderId, invoiceViewModel.CreatedBy);
        }

        private static SystemOrderAutoClosedNewsfeedViewModel GetSystemOrderAutoClosedNewsfeedViewModel(InvoiceCreateViewModel invoiceCreateRequestViewModel, InvoiceCreateResponseViewModel item)
        {
            return new SystemOrderAutoClosedNewsfeedViewModel
            {
                JobId = item.JobId,
                OrderId = item.OrderId,
                PoNumber = item.PoNumber,
                BuyerCompanyId = item.BuyerCompanyId,
                SupplierCompanyId = item.SupplierCompanyId,
                TimeZoneName = invoiceCreateRequestViewModel.TimeZoneName,
                TotalDelivered = item.OrderTotalDelivered,
                JobCompanyId = invoiceCreateRequestViewModel.JobCompanyId,
                UoM = invoiceCreateRequestViewModel.UoM
            };
        }

        private static SystemInvoiceCreatedNewsfeedModel GetSystemInvoiceCreatedNewsfeedModel(InvoiceCreateViewModel mobileInvoiceCreateRequestViewModel, InvoiceModel invoiceModel, InvoiceCreateResponseViewModel invoiceCreateResponse)
        {
            var maxQuantity = mobileInvoiceCreateRequestViewModel.BrokeredMaxQuantity ?? mobileInvoiceCreateRequestViewModel.MaxQuantity;
            return new SystemInvoiceCreatedNewsfeedModel
            {
                JobId = mobileInvoiceCreateRequestViewModel.JobId,
                InvoiceId = invoiceCreateResponse.InvoiceId,
                InvoiceNumber = invoiceCreateResponse.InvoiceNumber,
                OrderId = invoiceCreateResponse.OrderId,
                PoNumber = invoiceCreateResponse.PoNumber,
                JobCompanyId = mobileInvoiceCreateRequestViewModel.JobCompanyId,
                BuyerCompanyId = invoiceCreateResponse.BuyerCompanyId,
                SupplierCompanyId = invoiceCreateResponse.SupplierCompanyId,
                CreatedDate = invoiceModel.CreatedDate,
                DriverId = invoiceCreateResponse.DriverId ?? 0,
                DropStartDate = invoiceCreateResponse.DropStartDate,
                DropEndDate = invoiceCreateResponse.DropEndDate,
                DroppedGallons = invoiceModel.DroppedGallons,
                OrderMaxQuantity = maxQuantity,
                ApprovalUserId = mobileInvoiceCreateRequestViewModel.ApprovalUserId,
                DeliveryTypeId = mobileInvoiceCreateRequestViewModel.DeliveryTypeId,
                UoM = invoiceModel.UoM,
                InvoiceHeaderId = invoiceCreateResponse.InvoiceHeaderId
            };
        }

        private static DigitalDropTicketNewsfeedModel GetDigitalDropTicketApprovalNewsfeedModel(InvoiceModel invoiceModel, InvoiceCreateResponseViewModel invoiceCreateResponse, int approvalUserId)
        {
            return new DigitalDropTicketNewsfeedModel
            {
                InvoiceId = invoiceCreateResponse.InvoiceId,
                InvoiceNumber = invoiceCreateResponse.InvoiceNumber,
                OrderId = invoiceCreateResponse.OrderId,
                PoNumber = invoiceCreateResponse.PoNumber,
                BuyerCompanyId = invoiceCreateResponse.BuyerCompanyId,
                SupplierCompanyId = invoiceCreateResponse.SupplierCompanyId,
                CreatedDate = invoiceModel.CreatedDate,
                DriverId = invoiceCreateResponse.DriverId ?? 0,
                DropStartDate = invoiceCreateResponse.DropStartDate,
                DropEndDate = invoiceCreateResponse.DropEndDate,
                DroppedGallons = invoiceModel.DroppedGallons,
                ApprovalUserId = approvalUserId,
                UoM = invoiceModel.UoM,
                InvoiceHeaderId = invoiceCreateResponse.InvoiceHeaderId
            };
        }

        private static DigitalDropTicketApprovalNewsfeedModel GetApprovalWorkflowNewsfeedModel(UserContext user, ManualInvoiceCreateRequestViewModel requestViewModel, InvoiceModel invoiceModel, InvoiceCreateResponseViewModel invoiceCreateResponse)
        {
            return new DigitalDropTicketApprovalNewsfeedModel
            {
                InvoiceId = invoiceCreateResponse.InvoiceId,
                InvoiceNumber = invoiceCreateResponse.InvoiceNumber,
                OrderId = invoiceCreateResponse.OrderId,
                PoNumber = invoiceCreateResponse.PoNumber,
                BuyerCompanyId = invoiceCreateResponse.BuyerCompanyId,
                SupplierCompanyId = invoiceCreateResponse.SupplierCompanyId,
                ApprovalUserId = requestViewModel.ApprovalUserId,
                TimeZoneName = requestViewModel.TimeZoneName,
                IsBrokeredOrder = requestViewModel.IsBrokeredOrder,
                SupplierPreferredInvoiceTypeId = invoiceModel.SupplierPreferredInvoiceTypeId.Value,
                CreatedBy = user.Id,
                ApprovalUserCompanyId = requestViewModel.JobCompanyId,
                ApprovalUserCompany = requestViewModel.JobCompanyName,
                JobId = requestViewModel.JobId,
                UserName = user.Name,
                SupplierCompanyName = user.CompanyName,
                ApprovalUserName = requestViewModel.ApprovalUserName,
                InvoiceTypeId = invoiceModel.InvoiceTypeId
            };
        }

        private static DigitalDropTicketDraftNewsfeedModel GetDigitalDropTicketDraftNewsfeedModel(InvoiceCreateResponseViewModel invoiceCreateResponse, DateTimeOffset createdDate)
        {
            return new DigitalDropTicketDraftNewsfeedModel
            {
                InvoiceId = invoiceCreateResponse.InvoiceId,
                InvoiceNumber = invoiceCreateResponse.InvoiceNumber,
                SupplierCompanyId = invoiceCreateResponse.SupplierCompanyId,
                DriverId = invoiceCreateResponse.DriverId ?? 0,
                DropStartDate = invoiceCreateResponse.DropStartDate,
                DropEndDate = invoiceCreateResponse.DropEndDate,
                OrderId = invoiceCreateResponse.OrderId,
                PoNumber = invoiceCreateResponse.PoNumber,
                CreatedDate = createdDate,
                InvoiceHeaderId = invoiceCreateResponse.InvoiceHeaderId
            };
        }

        private static ManualInvoiceCreatedNewsfeedModel GetManualInvoiceCreatedNewsfeedModel(ManualInvoiceCreateRequestViewModel viewModel, InvoiceCreateResponseViewModel invoiceCreateResponse, InvoiceModel invoiceModel)
        {
            return new ManualInvoiceCreatedNewsfeedModel
            {
                InvoiceId = invoiceCreateResponse.InvoiceId,
                InvoiceNumber = invoiceCreateResponse.InvoiceNumber,
                OrderId = invoiceCreateResponse.OrderId,
                PoNumber = invoiceCreateResponse.PoNumber,
                BuyerCompanyId = invoiceCreateResponse.BuyerCompanyId,
                SupplierCompanyId = invoiceCreateResponse.SupplierCompanyId,
                JobId = viewModel.JobId,
                InvoiceTypeId = invoiceCreateResponse.InvoiceTypeId,
                TimeZoneName = viewModel.TimeZoneName,
                DeliveryTypeId = viewModel.DeliveryTypeId,
                OrderCloseDate = viewModel.DeliveryStartDate.Add(viewModel.DeliveryEndTime),
                DropPercentage = invoiceCreateResponse.DropPercentPerDelivery,
                WaitingFor = invoiceModel.WaitingFor,
                InvoiceHeaderId = invoiceCreateResponse.InvoiceHeaderId
            };
        }

        private NewsfeedEvent GetWaitingForNewsfeedType(WaitingAction WaitingForAction)
        {
            var response = NewsfeedEvent.DriverDropsWaitingForApproval;
            if (WaitingForAction == WaitingAction.UpdatedPrice)
            {
                return NewsfeedEvent.DriverDropsWaitingForUpdatedPrice;
            }
            else if (WaitingForAction == WaitingAction.AvalaraTax)
            {
                return NewsfeedEvent.DriverDropsWaitingForTaxes;
            }
            return response;
        }

        private static int GetMobileAssetDropCount(List<AssetDrop> mobileAssetDrops)
        {
            return mobileAssetDrops.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).Select(t => t.JobXAssetId).Distinct().Count();
        }

        private async Task<List<InvoiceCreateRequestViewModel>> GetMobileInvoiceModelForBrokers(DriverDropOrderViewModel DriverDropViewModel, MobileInvoiceCreateRequestViewModel mobileInvoiceCreateRequestViewModel, List<AssetDrop> mobileAssetDrops, InvoiceModel invoiceModel)
        {
            var response = new List<InvoiceCreateRequestViewModel>();
            var endSupplierDeliveryScheduleDate = DateTimeOffset.Now;
            var endSupplierDeliveryScheduleId = GetEndSupplierDeliveryScheduleIdForBrokerChain(DriverDropViewModel.TrackableScheduleId, out endSupplierDeliveryScheduleDate);

            var brokeredOrderInfo = new Dictionary<int, int>();
            GetBrokerChainOrderListTillOriginalOrder(mobileInvoiceCreateRequestViewModel.OrderId, brokeredOrderInfo);
            var brokeredOrderIds = brokeredOrderInfo.Select(t => t.Key).ToList();
            Dictionary<int, int> trackableScheduleIds = GetTrackableSchedulesForBrokers(brokeredOrderIds, endSupplierDeliveryScheduleId, endSupplierDeliveryScheduleDate);
            foreach (var brokeredOrder in brokeredOrderInfo)
            {
                var BrDriverDropViewModel = DriverDropViewModel.Clone(brokeredOrder.Key);
                BrDriverDropViewModel.Driver.UserId = brokeredOrder.Value;

                var trackableId = trackableScheduleIds.Where(t => t.Key == brokeredOrder.Key).Select(t => t.Value).FirstOrDefault();
                BrDriverDropViewModel.TrackableScheduleId = trackableId;

                var BrInvoiceRequestViewModel = await GetCreateMobileInvoiceRequestViewModelAsync(BrDriverDropViewModel);
                int assetCount = GetMobileAssetDropCount(mobileAssetDrops);
                //var BrInvoiceModel = await GetMobileInvoiceViewModelAsync(BrMobileInvoiceCreateRequestViewModel, assetCount);

                InvoiceModel BrInvoiceModel = new InvoiceModel();
                SetMobileInputsToInvoiceModel(BrInvoiceRequestViewModel, BrInvoiceModel);
                await SetInvoiceNumberToInvoiceModel(BrInvoiceModel);
                BrInvoiceModel = await GetInvoiceViewModel(BrInvoiceRequestViewModel, BrInvoiceModel, assetCount);

                // Set invoice pricing to invoice model
                await SetInvoicePricingToInvoiceModel(BrInvoiceRequestViewModel, BrInvoiceModel);
                SetInvoiceAmountAndAllowanceToInvoiceModel(BrInvoiceRequestViewModel, BrInvoiceModel);

                // Set fees with calculation to invoice model
                RemoveDryRunFeeFromManualInvoiceModel(BrInvoiceRequestViewModel);
                ProcessInvoiceFuelFeesAndSetCalculatedValues(BrInvoiceRequestViewModel, BrInvoiceModel, BrInvoiceModel.DropEndDate, assetCount);

                BrInvoiceModel.AssetDrops = GetMobileAssetDropsForBrokers(mobileAssetDrops, brokeredOrder.Key);
                BrInvoiceModel.BrokeredChainId = invoiceModel.BrokeredChainId;

                if (BrInvoiceRequestViewModel.AdditionalDetail == null) BrInvoiceRequestViewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                BrInvoiceRequestViewModel.AdditionalDetail.Notes = mobileInvoiceCreateRequestViewModel.AdditionalDetail?.Notes;

                if (BrInvoiceModel.AdditionalDetail == null) BrInvoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                BrInvoiceModel.AdditionalDetail.Notes = mobileInvoiceCreateRequestViewModel.AdditionalDetail?.Notes;

                // Carry forward taxes from first invoice to set in brokered invoices. It will save avalara service call
                BrInvoiceModel.TaxDetails = invoiceModel.TaxDetails;
                var BrInvoiceCreateRequestViewModel = GetInvoiceCreateRequestViewModelWithTaxWaitingStatus(BrInvoiceRequestViewModel, BrInvoiceModel);
                if (BrInvoiceCreateRequestViewModel.StatusCode == Status.Success)
                {
                    response.Add(BrInvoiceCreateRequestViewModel);
                }
            }
            return response;
        }

        private async Task<List<InvoiceCreateRequestViewModel>> GetFtlMobileInvoiceModelForBrokers(FtlDriverDropOrderViewModel DriverDropViewModel, MobileInvoiceCreateRequestViewModel mobileInvoiceCreateRequestViewModel, List<AssetDrop> mobileAssetDrops, InvoiceModel invoiceModel)
        {
            var response = new List<InvoiceCreateRequestViewModel>();
            var endSupplierDeliveryScheduleDate = DateTimeOffset.Now;
            var endSupplierDeliveryScheduleId = GetEndSupplierDeliveryScheduleIdForBrokerChain(DriverDropViewModel.TrackableScheduleId, out endSupplierDeliveryScheduleDate);

            var brokeredOrderInfo = new Dictionary<int, int>();
            GetBrokerChainOrderListTillOriginalOrder(mobileInvoiceCreateRequestViewModel.OrderId, brokeredOrderInfo);
            var brokeredOrderIds = brokeredOrderInfo.Select(t => t.Key).ToList();
            Dictionary<int, int> trackableScheduleIds = GetTrackableSchedulesForBrokers(brokeredOrderIds, endSupplierDeliveryScheduleId, endSupplierDeliveryScheduleDate);
            foreach (var brokeredOrder in brokeredOrderInfo)
            {
                var BrDriverDropViewModel = DriverDropViewModel.Clone(brokeredOrder.Key);
                BrDriverDropViewModel.Driver.UserId = brokeredOrder.Value;

                var trackableId = trackableScheduleIds.Where(t => t.Key == brokeredOrder.Key).Select(t => t.Value).FirstOrDefault();
                BrDriverDropViewModel.TrackableScheduleId = trackableId;

                var BrInvoiceRequestViewModel = await GetCreateFtlMobileInvoiceRequestViewModelAsync(BrDriverDropViewModel);
                int assetCount = GetMobileAssetDropCount(mobileAssetDrops);
                //var BrInvoiceModel = await GetMobileInvoiceViewModelAsync(BrMobileInvoiceCreateRequestViewModel, assetCount);

                InvoiceModel BrInvoiceModel = new InvoiceModel();
                SetMobileInputsToInvoiceModel(BrInvoiceRequestViewModel, BrInvoiceModel);
                await SetInvoiceNumberToInvoiceModel(BrInvoiceModel);
                BrInvoiceModel = await GetInvoiceViewModel(BrInvoiceRequestViewModel, BrInvoiceModel, assetCount);

                await CheckAndSetInvoiceExceptions(BrInvoiceRequestViewModel, BrInvoiceModel);
                if (BrInvoiceModel.WaitingFor == WaitingAction.Nothing)
                {
                    // Set invoice pricing to invoice model
                    await SetInvoicePricingToInvoiceModel(BrInvoiceRequestViewModel, BrInvoiceModel);
                    SetInvoiceAmountAndAllowanceToInvoiceModel(BrInvoiceRequestViewModel, BrInvoiceModel);
                }

                // Set fees with calculation to invoice model
                RemoveDryRunFeeFromManualInvoiceModel(BrInvoiceRequestViewModel);
                ProcessInvoiceFuelFeesAndSetCalculatedValues(BrInvoiceRequestViewModel, BrInvoiceModel, BrInvoiceModel.DropEndDate, assetCount);

                BrInvoiceModel.AssetDrops = GetMobileAssetDropsForBrokers(mobileAssetDrops, brokeredOrder.Key);
                BrInvoiceModel.BrokeredChainId = invoiceModel.BrokeredChainId;

                if (BrInvoiceRequestViewModel.AdditionalDetail == null) BrInvoiceRequestViewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                BrInvoiceRequestViewModel.AdditionalDetail.Notes = mobileInvoiceCreateRequestViewModel.AdditionalDetail?.Notes;

                if (BrInvoiceModel.AdditionalDetail == null) BrInvoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                BrInvoiceModel.AdditionalDetail.Notes = mobileInvoiceCreateRequestViewModel.AdditionalDetail?.Notes;

                // Carry forward taxes from first invoice to set in brokered invoices. It will save avalara service call
                BrInvoiceModel.TaxDetails = invoiceModel.TaxDetails;
                if (invoiceModel.WaitingFor == WaitingAction.BolDetails)
                    BrInvoiceModel.WaitingFor = WaitingAction.BolDetails;

                var BrInvoiceCreateRequestViewModel = GetInvoiceCreateRequestViewModelWithTaxWaitingStatus(BrInvoiceRequestViewModel, BrInvoiceModel);
                //SetTaxesToFtlInvoiceModel(BrMobileInvoiceCreateRequestViewModel, BrInvoiceModel);
                if (BrInvoiceCreateRequestViewModel.StatusCode == Status.Success)
                {
                    response.Add(BrInvoiceCreateRequestViewModel);
                }
            }
            return response;
        }

        private async Task<List<InvoiceCreateRequestViewModel>> GetManualInvoiceModelForBrokers(ManualInvoiceViewModel manualInvoiceViewModel, ManualInvoiceCreateRequestViewModel manualInvoiceCreateRequestViewModel, InvoiceModel invoiceModel)
        {
            var response = new List<InvoiceCreateRequestViewModel>();
            DateTimeOffset endSupplierDeliveryScheduleDate;
            var endSupplierDeliveryScheduleId = GetEndSupplierDeliveryScheduleIdForBrokerChain(manualInvoiceViewModel.TrackableScheduleId, out endSupplierDeliveryScheduleDate);

            var brokeredOrderInfo = new Dictionary<int, int>();
            GetBrokerChainOrderListTillOriginalOrder(manualInvoiceCreateRequestViewModel.OrderId, brokeredOrderInfo);
            var brokeredOrderIds = brokeredOrderInfo.Select(t => t.Key).ToList();
            Dictionary<int, int> trackableScheduleIds = GetTrackableSchedulesForBrokers(brokeredOrderIds, endSupplierDeliveryScheduleId, endSupplierDeliveryScheduleDate);

            foreach (var brokeredOrder in brokeredOrderInfo)
            {
                var BrInvoiceViewModel = manualInvoiceViewModel.Clone(brokeredOrder.Value, brokeredOrder.Key);

                var trackableId = trackableScheduleIds.Where(t => t.Key == brokeredOrder.Key).Select(t => t.Value).FirstOrDefault();
                BrInvoiceViewModel.TrackableScheduleId = trackableId;
                InvoiceModel BrInvoiceModel = new InvoiceModel();
                var BrInvoiceRequestModel = await GetBrokerInvoiceRequestViewModelAsync(BrInvoiceViewModel);
                BrInvoiceModel.BrokeredChainId = invoiceModel.BrokeredChainId;

                SetManualInputToInvoiceModel(BrInvoiceRequestModel, BrInvoiceModel);
                var assetCount = BrInvoiceViewModel.Assets.Select(t => t.AssetName.ToLower()).Distinct().Count();
                await SetInvoiceNumberToInvoiceModel(BrInvoiceModel);
                BrInvoiceModel = await GetInvoiceViewModel(BrInvoiceRequestModel, BrInvoiceModel, assetCount);
                CheckForWaitingForApproval(BrInvoiceModel, BrInvoiceRequestModel.IsApprovalWorkflowEnabledForJob, BrInvoiceRequestModel.InvoiceTypeId, BrInvoiceRequestModel.InvoiceStatusId);
                CheckRequiredImagesAndSetWaitingForImageAction(BrInvoiceModel);

                await CheckAndSetInvoiceExceptions(BrInvoiceRequestModel, BrInvoiceModel);
                if (BrInvoiceModel.WaitingFor == WaitingAction.Nothing)
                {
                    // Set invoice pricing to invoice model
                    await SetInvoicePricingToInvoiceModel(BrInvoiceRequestModel, BrInvoiceModel);
                    SetInvoiceAmountAndAllowanceToInvoiceModel(BrInvoiceRequestModel, BrInvoiceModel);
                }

                // Set fees with calculation to invoice model
                RemoveDryRunFeeFromManualInvoiceModel(BrInvoiceRequestModel);
                ProcessInvoiceFuelFeesAndSetCalculatedValues(BrInvoiceRequestModel, BrInvoiceModel, BrInvoiceModel.DropEndDate, assetCount);

                if (BrInvoiceRequestModel.AdditionalDetail == null) BrInvoiceRequestModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                BrInvoiceRequestModel.AdditionalDetail.Notes = manualInvoiceCreateRequestViewModel.AdditionalDetail?.Notes;

                if (BrInvoiceModel.AdditionalDetail == null) BrInvoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                BrInvoiceModel.AdditionalDetail.Notes = manualInvoiceCreateRequestViewModel.AdditionalDetail?.Notes;

                // Carry forward taxes from first invoice to set in brokered invoices. It will save avalara service call
                BrInvoiceModel.TaxDetails = invoiceModel.TaxDetails;
                var BrInvoiceCreateRequestViewModel = SetTaxesToInvoiceModel(BrInvoiceRequestModel, BrInvoiceModel);
                if (BrInvoiceCreateRequestViewModel.StatusCode == Status.Success)
                {
                    response.Add(BrInvoiceCreateRequestViewModel);
                }
            }
            return response;
        }

        private async Task<List<InvoiceCreateRequestViewModel>> GetBrokerSplitInvoiceModel(ManualInvoiceViewModel manualInvoiceViewModel, ManualInvoiceCreateRequestViewModel manualInvoiceCreateRequestViewModel, InvoiceModel invoiceModel, List<DropdownDisplayItem> SplitInvoiceBolId)
        {
            var response = new List<InvoiceCreateRequestViewModel>();
            DateTimeOffset endSupplierDeliveryScheduleDate;
            var endSupplierDeliveryScheduleId = GetEndSupplierDeliveryScheduleIdForBrokerChain(manualInvoiceViewModel.TrackableScheduleId, out endSupplierDeliveryScheduleDate);

            var brokeredOrderInfo = new Dictionary<int, int>();
            GetBrokerChainOrderListTillOriginalOrder(manualInvoiceCreateRequestViewModel.OrderId, brokeredOrderInfo);
            var brokeredOrderIds = brokeredOrderInfo.Select(t => t.Key).ToList();
            Dictionary<int, int> trackableScheduleIds = GetTrackableSchedulesForBrokers(brokeredOrderIds, endSupplierDeliveryScheduleId, endSupplierDeliveryScheduleDate);
            int splitId = 0;
            foreach (var brokeredOrder in brokeredOrderInfo)
            {
                var BrInvoiceViewModel = manualInvoiceViewModel.Clone(brokeredOrder.Value, brokeredOrder.Key); // modify this

                //Add SplitLoad Chain Id for Brokers
                splitId++;
                BrInvoiceViewModel.SplitLoadChainId = GetBrokerSplitLoadChainId(BrInvoiceViewModel.SplitLoadChainId, splitId);

                var trackableId = trackableScheduleIds.Where(t => t.Key == brokeredOrder.Key).Select(t => t.Value).FirstOrDefault();
                BrInvoiceViewModel.TrackableScheduleId = trackableId;
                InvoiceModel BrInvoiceModel = new InvoiceModel();
                var BrMobileInvoiceRequestModel = await GetBrokerInvoiceRequestViewModelAsync(BrInvoiceViewModel);
                BrInvoiceModel.BrokeredChainId = invoiceModel.BrokeredChainId;

                SetManualInputToInvoiceModel(BrMobileInvoiceRequestModel, BrInvoiceModel);
                var assetCount = BrInvoiceViewModel.Assets.Select(t => t.AssetName.ToLower()).Distinct().Count();
                await SetInvoiceNumberToInvoiceModel(BrInvoiceModel);
                BrInvoiceModel = await GetInvoiceViewModel(BrMobileInvoiceRequestModel, BrInvoiceModel, assetCount);
                CheckForWaitingForApproval(BrInvoiceModel, BrMobileInvoiceRequestModel.IsApprovalWorkflowEnabledForJob, BrMobileInvoiceRequestModel.InvoiceTypeId, BrMobileInvoiceRequestModel.InvoiceStatusId);
                CheckRequiredImagesAndSetWaitingForImageAction(BrInvoiceModel);

                // Set invoice pricing to invoice model
                await SetInvoicePricingToInvoiceModel(BrMobileInvoiceRequestModel, BrInvoiceModel);
                SetInvoiceAmountAndAllowanceToInvoiceModel(BrMobileInvoiceRequestModel, BrInvoiceModel);

                // Set fees with calculation to invoice model
                RemoveDryRunFeeFromManualInvoiceModel(BrMobileInvoiceRequestModel);
                ProcessInvoiceFuelFeesAndSetCalculatedValues(BrMobileInvoiceRequestModel, BrInvoiceModel, BrInvoiceModel.DropEndDate, assetCount);

                var splitBolId = SplitInvoiceBolId.FirstOrDefault(t => t.Name == BrInvoiceViewModel.SplitLoadChainId);
                if (splitBolId != null)
                {
                    BrMobileInvoiceRequestModel.BolDetails.Id = splitBolId.Id;
                }

                if (BrMobileInvoiceRequestModel.AdditionalDetail == null) BrMobileInvoiceRequestModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                BrMobileInvoiceRequestModel.AdditionalDetail.Notes = manualInvoiceCreateRequestViewModel.AdditionalDetail?.Notes;

                if (BrInvoiceModel.AdditionalDetail == null) BrInvoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                BrInvoiceModel.AdditionalDetail.Notes = manualInvoiceCreateRequestViewModel.AdditionalDetail?.Notes;

                // Carry forward taxes from first invoice to set in brokered invoices. It will save avalara service call
                BrInvoiceModel.TaxDetails = invoiceModel.TaxDetails;
                var BrInvoiceCreateRequestViewModel = SetTaxesToInvoiceModel(BrMobileInvoiceRequestModel, BrInvoiceModel);
                if (BrInvoiceCreateRequestViewModel.StatusCode == Status.Success)
                {
                    response.Add(BrInvoiceCreateRequestViewModel);
                }
            }
            return response;
        }

        private string GetBrokerSplitLoadChainId(string splitLoadChainId, int SplitId)
        {
            var response = splitLoadChainId;
            if (!string.IsNullOrWhiteSpace(splitLoadChainId))
            {
                response = splitLoadChainId + "_" + SplitId;
            }
            return response;
        }

        private async Task<List<InvoiceCreateRequestViewModel>> GetDraftDdtInvoiceModelForBrokers(ManualInvoiceViewModel manualInvoiceViewModel, ManualInvoiceCreateRequestViewModel manualInvoiceCreateRequestViewModel, InvoiceModel invoiceModel)
        {
            var response = new List<InvoiceCreateRequestViewModel>();
            using (var tracer = new Tracer("InvoiceCreateDomain", "GetDraftDdtInvoiceModelForBrokers"))
            {
                DateTimeOffset endSupplierDeliveryScheduleDate;
                var endSupplierDeliveryScheduleId = GetEndSupplierDeliveryScheduleIdForBrokerChain(manualInvoiceViewModel.TrackableScheduleId, out endSupplierDeliveryScheduleDate);

                var brokeredOrderInfo = new Dictionary<int, int>();
                GetBrokerChainOrderListTillOriginalOrder(manualInvoiceCreateRequestViewModel.OrderId, brokeredOrderInfo);
                var brokeredOrderIds = brokeredOrderInfo.Select(t => t.Key).ToList();
                Dictionary<int, int> trackableScheduleIds = GetTrackableSchedulesForBrokers(brokeredOrderIds, endSupplierDeliveryScheduleId, endSupplierDeliveryScheduleDate);
                foreach (var brokeredOrder in brokeredOrderInfo)
                {
                    var BrInvoiceViewModel = manualInvoiceViewModel.Clone(brokeredOrder.Value, brokeredOrder.Key);

                    var trackableId = trackableScheduleIds.Where(t => t.Key == brokeredOrder.Key).Select(t => t.Value).FirstOrDefault();
                    BrInvoiceViewModel.TrackableScheduleId = trackableId;
                    InvoiceModel BrInvoiceModel = new InvoiceModel();
                    var BrMobileInvoiceRequestModel = await GetBrokerInvoiceRequestViewModelAsync(BrInvoiceViewModel);
                    BrInvoiceModel.BrokeredChainId = invoiceModel.BrokeredChainId;

                    SetManualInputToInvoiceModel(BrMobileInvoiceRequestModel, BrInvoiceModel);
                    var assetCount = BrInvoiceViewModel.Assets.Select(t => t.AssetName.ToLower()).Distinct().Count();

                    await SetInvoiceNumberToInvoiceModel(BrInvoiceModel);
                    BrInvoiceModel = await GetInvoiceViewModel(BrMobileInvoiceRequestModel, BrInvoiceModel, assetCount);
                    CheckRequiredImagesAndSetWaitingForImageAction(BrInvoiceModel);

                    // Set invoice pricing to invoice model
                    await SetInvoicePricingToInvoiceModel(BrMobileInvoiceRequestModel, BrInvoiceModel);
                    SetInvoiceAmountAndAllowanceToInvoiceModel(BrMobileInvoiceRequestModel, BrInvoiceModel);

                    // Set fees with calculation to invoice model
                    RemoveDryRunFeeFromManualInvoiceModel(BrMobileInvoiceRequestModel);
                    ProcessInvoiceFuelFeesAndSetCalculatedValues(BrMobileInvoiceRequestModel, BrInvoiceModel, BrInvoiceModel.DropEndDate, assetCount);

                    // set invoice type an display invoice no for broker
                    BrInvoiceModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                    BrInvoiceModel.DisplayInvoiceNumber = BrInvoiceModel.DisplayInvoiceNumber.FormattedInvoiceNumber(BrInvoiceModel.InvoiceTypeId);

                    // Carry forward taxes from first invoice to set in brokered invoices. It will save avalara service call
                    BrInvoiceModel.TaxDetails = invoiceModel.TaxDetails;

                    var BrInvoiceCreateRequestViewModel = SetTaxesToInvoiceModel(BrMobileInvoiceRequestModel, BrInvoiceModel);
                    if (BrInvoiceCreateRequestViewModel.StatusCode == Status.Success)
                    {
                        response.Add(BrInvoiceCreateRequestViewModel);
                    }
                }
            }
            return response;
        }

        private Dictionary<int, int> GetTrackableSchedulesForBrokers(List<int> brokeredOrderIds, int deliveryScheduleId, DateTimeOffset deliveryScheduleDate)
        {
            var trackableScheduleIds = new Dictionary<int, int>();
            try
            {
                var scheduleIds = Context.DataContext.DeliveryScheduleXTrackableSchedules
                                    .Where(t => t.OrderId != null && brokeredOrderIds.Contains(t.OrderId.Value)
                                    && t.DeliveryScheduleId == deliveryScheduleId && t.Date == deliveryScheduleDate)
                                    .Select(t => new { t.OrderId, t.Id }).Distinct().ToList();
                scheduleIds.ForEach(t => trackableScheduleIds.Add(t.OrderId.Value, t.Id));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetTrackableSchedulesForBrokers", ex.Message, ex);
            }
            return trackableScheduleIds;
        }

        private static List<AssetDropModel> GetMobileAssetDropsForBrokers(List<AssetDrop> mobileAssetDrops, int brokeredOrderId)
        {
            var assetDrops = mobileAssetDrops.Select(t => t.ToViewModel()).ToList();
            assetDrops.ForEach(t => t.OrderId = brokeredOrderId);
            return assetDrops;
        }

        private async Task<MobileInvoiceCreateRequestViewModel> GetCreateMobileInvoiceRequestViewModelAsync(DriverDropOrderViewModel DriverDropViewModel)
        {
            var viewModel = new MobileInvoiceCreateRequestViewModel();
            try
            {
                viewModel = DriverDropViewModel.ToMobileInvoiceViewModel();
                viewModel.SpecialInstructions = await GetSpecialInstructions(DriverDropViewModel);
                viewModel.InvoiceTypeId = GetMobileInvoiceType(DriverDropViewModel);
                var order = await Context.DataContext.Orders.Where(t => t.Id == DriverDropViewModel.OrderId)
                            .Select(t => new
                            {
                                t.PoNumber,
                                t.DefaultInvoiceType,
                                t.OrderTaxDetails,
                                t.BuyerCompanyId,
                                t.AcceptedCompanyId,
                                t.ExternalBrokerId,
                                t.ExternalBrokerOrderDetail,
                                t.TerminalId,
                                t.MstExternalTerminal,
                                t.CityGroupTerminalId,
                                t.BrokeredMaxQuantity,
                                t.AcceptedBy,
                                t.IsEndSupplier,
                                t.IsFTL,
                                t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive).PaymentTermId,
                                t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive).NetDays,
                                SupplierTaxExemptLicence = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                IsBuySellOrder = t.ExternalBrokerBuySellDetail != null,
                                t.ExternalBrokerBuySellDetail,
                                TerminalAddress = t.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                || t.TerminalId == null ? null : new AddressViewModel
                                {
                                    Address = t.MstExternalTerminal.Address,
                                    City = t.MstExternalTerminal.City,
                                    StateCode = t.MstExternalTerminal.StateCode,
                                    CountryCode = t.MstExternalTerminal.CountryCode,
                                    ZipCode = t.MstExternalTerminal.ZipCode,
                                    CountyName = t.MstExternalTerminal.CountyName
                                },
                                TerminalStateId = t.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                || t.TerminalId == null ? 0 : t.MstExternalTerminal.StateId,
                                Job = new
                                {
                                    t.FuelRequest.Job.TimeZoneName,
                                    JobAddrsss = new AddressViewModel
                                    {
                                        Address = t.FuelRequest.Job.Address,
                                        City = t.FuelRequest.Job.City,
                                        StateCode = t.FuelRequest.Job.MstState.Code,
                                        CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                        ZipCode = t.FuelRequest.Job.ZipCode,
                                        CountyName = t.FuelRequest.Job.CountyName
                                    },
                                    CountryCurrency = t.FuelRequest.Job.MstCountry.Currency,
                                    t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                    t.FuelRequest.Job.CompanyId,
                                    t.FuelRequest.Job.StateId,
                                    t.FuelRequest.Job.JobBudget.IsAssetTracked,
                                    t.FuelRequest.Job.JobBudget.IsTaxExempted,
                                    BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                    ApprovalUserId = t.FuelRequest.Job.IsApprovalWorkflowEnabled ? t.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t1 => t1.IsActive).UserId : 0
                                },
                                FuelRequest = new
                                {
                                    t.FuelRequest.Id,
                                    t.FuelRequest.JobId,
                                    t.FuelRequest.UoM,
                                    t.FuelRequest.Currency,
                                    t.FuelRequest.FuelTypeId,
                                    t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                    t.FuelRequest.MstProduct.ProductCode,
                                    t.FuelRequest.PricingTypeId,
                                    t.FuelRequest.FuelDescription,
                                    t.FuelRequest.FuelRequestFees,
                                    t.FuelRequest.MaxQuantity,
                                    t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                    t.FuelRequest.FreightOnBoardTypeId,
                                    t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId
                                },
                                OrderAdditionalDetails = t.OrderAdditionalDetail == null ? null : new
                                {
                                    t.OrderAdditionalDetail.Notes
                                },
                                PickupLocation = t.FuelDispatchLocations.FirstOrDefault(t1 => t1.LocationType == (int)LocationType.PickUp && t1.TrackableScheduleId == DriverDropViewModel.TrackableScheduleId && t1.IsActive),
                            }).FirstOrDefaultAsync();
                if (order != null)
                {
                    var timeZoneName = order.Job.TimeZoneName;
                    viewModel.DropStartDate = DriverDropViewModel.DropStartDate.ToTargetDateTimeOffset(timeZoneName);
                    viewModel.DropEndDate = DriverDropViewModel.DropEndDate.ToTargetDateTimeOffset(timeZoneName);
                    viewModel.UoM = order.FuelRequest.UoM;
                    viewModel.Currency = order.FuelRequest.Currency;
                    viewModel.PoNumber = order.PoNumber;
                    if (order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketManual)
                    {
                        // Set mobile DDT type for invoce if DDT is selected on order as default invoice type
                        viewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                    }
                    viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = order.FuelRequest.FuelRequestFees.ToFeesViewModel();
                    viewModel.IsWetHosingDelivery = viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.WetHoseFee).ToString() && t.DiscountLineItemId == null);
                    viewModel.IsOverWaterDelivery = viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.OverWaterFee).ToString() && t.DiscountLineItemId == null);

                    viewModel.TimeZoneName = timeZoneName;
                    viewModel.PaymentTermId = order.PaymentTermId;
                    viewModel.NetDays = order.NetDays;
                    viewModel.FuelTypeId = order.FuelRequest.FuelTypeId;
                    viewModel.TypeOfFuel = order.FuelRequest.ProductDisplayGroupId;
                    viewModel.FuelProductCode = order.FuelRequest.ProductCode;
                    if (viewModel.TypeOfFuel == (int)ProductDisplayGroups.OtherFuelType)
                    {
                        viewModel.OtherProductTaxes = order.OrderTaxDetails
                            .Where(t => t.IsActive).Select(t => new TaxViewModel
                            {
                                TaxAmount = t.TaxRate,
                                TaxPricingTypeId = t.TaxPricingTypeId,
                                TaxDescription = t.TaxDescription
                            }).ToList();
                    }

                    viewModel.IsBuySellOrder = order.IsBuySellOrder;
                    viewModel.SupplierPreferredInvoiceTypeId = order.DefaultInvoiceType;
                    if (viewModel.IsBuySellOrder)
                    {
                        viewModel.BrokerMarkUp = order.ExternalBrokerBuySellDetail.BrokerMarkUp;
                        viewModel.SupplierMarkUp = order.ExternalBrokerBuySellDetail.SupplierMarkUp;
                    }
                    viewModel.CountryCurrency = order.Job.CountryCurrency;
                    viewModel.BuyerCompanyId = order.BuyerCompanyId;
                    viewModel.JobId = order.FuelRequest.JobId;
                    viewModel.JobCompanyId = order.Job.CompanyId;
                    viewModel.JobStateId = order.Job.StateId;
                    viewModel.IsAssetTracked = order.Job.IsAssetTracked;
                    viewModel.PricingTypeId = order.FuelRequest.PricingTypeId;
                    viewModel.DeliveryTypeId = order.FuelRequest.DeliveryTypeId;
                    viewModel.MaxQuantity = order.FuelRequest.MaxQuantity;
                    viewModel.BrokeredMaxQuantity = order.BrokeredMaxQuantity;
                    viewModel.AcceptedCompanyId = order.AcceptedCompanyId;
                    viewModel.IsSalesTaxExempted = order.Job.IsTaxExempted;
                    viewModel.FuelRequestPricingDetail.RequestPriceDetailId = order.FuelRequest.RequestPriceDetailId;
                    viewModel.OrderAcceptedBy = order.AcceptedBy;
                    viewModel.ApprovalUserId = order.Job.ApprovalUserId;
                    viewModel.IsApprovalWorkflowEnabledForJob = order.Job.IsApprovalWorkflowEnabled;
                    viewModel.ProductDescription = order.FuelRequest.FuelDescription;
                    viewModel.IsSalesTaxExempted = order.Job.IsTaxExempted;
                    viewModel.ExternalBrokerId = order.ExternalBrokerId ?? 0;
                    viewModel.IsThirdPartyHardwareUsed = order.ExternalBrokerOrderDetail != null;
                    if (order.ExternalBrokerOrderDetail != null)
                    {
                        viewModel.ExternalBrokeredOrder = order.ExternalBrokerOrderDetail.ToViewModel(viewModel.ExternalBrokeredOrder);
                    }
                    SetTaxExemptLicences(viewModel, order);

                    // get pricing details from pricing service
                    PricingRequestDetailResponseViewModel pricingDetails = new PricingRequestDetailResponseViewModel();  // need to check is this needed ?
                    if (order.FuelRequest.RequestPriceDetailId > 0)
                    {
                        var orderDomain = new OrderDomain(this);
                        pricingDetails = await orderDomain.GetRequestPricingDetail(order.FuelRequest.RequestPriceDetailId, (int)viewModel.Currency, order.AcceptedCompanyId, order.FuelRequest.FuelTypeId, viewModel.JobStateId);
                        if (pricingDetails != null)
                        {
                            viewModel.PricingTypeId = pricingDetails.PricingTypeId;
                            viewModel.RackAvgTypeId = pricingDetails.RackAvgTypeId;
                            viewModel.PricePerGallon = pricingDetails.PricePerGallon;
                            viewModel.SupplierCost = pricingDetails.SupplierCost;
                        }
                    }

                    viewModel.TerminalId = order.TerminalId;
                    viewModel.CityGroupTerminalId = order.CityGroupTerminalId;
                    viewModel.IsBrokeredChainOrder = order.BuyerCompanyId != order.Job.CompanyId;
                    viewModel.JobAddess = order.Job.JobAddrsss;
                    viewModel.TerminalAddress = order.TerminalAddress;
                    viewModel.TerminalStateId = order.TerminalStateId;

                    if (order.OrderAdditionalDetails != null)
                    {
                        if (viewModel.AdditionalDetail == null)
                            viewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                        viewModel.AdditionalDetail.Notes = order.OrderAdditionalDetails.Notes;
                        viewModel.AdditionalDetail.SupplierAllowance = viewModel.SupplierAllowance;
                    }
                    viewModel.QuantityIndicatorTypeId = viewModel.FuelRequestPricingDetail.PricingQuantityIndicatorTypeId ?? 0;
                    if (viewModel.BolDetails != null)
                    {
                        viewModel.BolDetails.Id = DriverDropViewModel.BolDetails.Id;
                        if (viewModel.BolDetails.Id > 0)
                        {
                            viewModel.BolDetails = Context.DataContext.InvoiceFtlDetails.Where(t => t.Id == viewModel.BolDetails.Id).FirstOrDefault()?.ToViewModel();
                        }
                       
                        if (order.IsFTL)
                        {
                            //if FTL order then assign DroppedGallons to Net/Gross
                            viewModel.ActualDropQuantity = viewModel.FuelDropped;
                            if (DriverDropViewModel.FuelPickLocation.PickupLocationType == PickupLocationType.Terminal)
                            {
                                viewModel.FuelDropped = viewModel.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net ? viewModel.BolDetails.NetQuantity : viewModel.BolDetails.GrossQuantity;
                            }
                        }
                        viewModel.BolDetails.ImageId = DriverDropViewModel.BolDetails.ImageId;
                    }
                    else
                    {
                        viewModel.BolDetails = new BolDetailViewModel();                        
                    }

                    viewModel.BolDetails.TerminalId = order.TerminalId;
                    viewModel.BolDetails.TerminalName = order.MstExternalTerminal?.Name;
                    viewModel.BolDetails.CityGroupTerminalId = order.CityGroupTerminalId;
                    viewModel.BolDetails.IsDeleted = false;
                    viewModel.BolDetails.IsActive = true;
                    viewModel.BolDetails.FuelTypeId = order.FuelRequest.FuelTypeId;
                    if (order.PickupLocation != null)
                    {
                        viewModel.BolDetails.Address = order.PickupLocation.Address;
                        viewModel.BolDetails.City = order.PickupLocation.City;
                        viewModel.BolDetails.CountryCode = order.PickupLocation.CountryCode;
                        viewModel.BolDetails.CountyName = order.PickupLocation.CountyName;
                        viewModel.BolDetails.Latitude = order.PickupLocation.Latitude;
                        viewModel.BolDetails.Longitude = order.PickupLocation.Longitude;
                        viewModel.BolDetails.SiteName = order.PickupLocation.SiteName;
                        viewModel.BolDetails.StateCode = order.PickupLocation.StateCode;
                        viewModel.BolDetails.StateId = order.PickupLocation.StateId ?? 0;
                        viewModel.BolDetails.ZipCode = order.PickupLocation.ZipCode;
                    }
                    else if (order.PickupLocation == null && order.MstExternalTerminal != null)
                    {
                        var terminal = order.MstExternalTerminal;
                        viewModel.BolDetails.Address = terminal.Address;
                        viewModel.BolDetails.City = terminal.City;
                        viewModel.BolDetails.CountryCode = terminal.CountryCode;
                        viewModel.BolDetails.CountyName = terminal.CountyName;
                        viewModel.BolDetails.Latitude = terminal.Latitude;
                        viewModel.BolDetails.Longitude = terminal.Longitude;
                        viewModel.BolDetails.StateCode = terminal.StateCode;
                        viewModel.BolDetails.StateId = terminal.StateId;
                        viewModel.BolDetails.ZipCode = terminal.ZipCode;
                    }

                    if (DriverDropViewModel.FuelPickLocation != null)
                    {
                        viewModel.PickupLocation = GetPickupLocationDetails(DriverDropViewModel.FuelPickLocation.Latitude, DriverDropViewModel.FuelPickLocation.Longitude, DriverDropViewModel, viewModel.Currency, (int)LocationType.PickUp);
                    }
                    if (DriverDropViewModel.Driver.Latitude != 0 && DriverDropViewModel.Driver.Longitude != 0)
                    {
                        viewModel.DropLocation = GetPickupLocationDetails(DriverDropViewModel.Driver.Latitude, DriverDropViewModel.Driver.Longitude, DriverDropViewModel, viewModel.Currency, (int)LocationType.Drop);
                    }
                    viewModel.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetCreateMobileInvoiceRequestViewModelAsync", ex.Message, ex);
            }
            return viewModel;
        }

        private async Task<MobileInvoiceCreateRequestViewModel> GetCreateFtlMobileInvoiceRequestViewModelAsync(FtlDriverDropOrderViewModel DriverDropViewModel)
        {
            var viewModel = new MobileInvoiceCreateRequestViewModel();
            try
            {
                viewModel = DriverDropViewModel.ToMobileInvoiceViewModel();

                viewModel.BolDetails = DriverDropViewModel.BolDetails;
                viewModel.SpecialInstructions = await GetSpecialInstructions(DriverDropViewModel);
                viewModel.InvoiceTypeId = GetMobileInvoiceType(DriverDropViewModel);

                var order = await Context.DataContext.Orders.Where(t => t.Id == DriverDropViewModel.OrderId)
                            .Select(t => new
                            {
                                t.PoNumber,
                                t.DefaultInvoiceType,
                                t.OrderTaxDetails,
                                t.BuyerCompanyId,
                                t.AcceptedCompanyId,
                                t.ExternalBrokerId,
                                t.ExternalBrokerOrderDetail,
                                t.TerminalId,
                                t.CityGroupTerminalId,
                                t.BrokeredMaxQuantity,
                                t.AcceptedBy,
                                t.IsEndSupplier,
                                t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive).PaymentTermId,
                                t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive).NetDays,
                                SupplierTaxExemptLicence = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                IsBuySellOrder = t.ExternalBrokerBuySellDetail != null,
                                t.ExternalBrokerBuySellDetail,
                                BuyerCompanyName = t.BuyerCompany.Name,
                                SupplierCompanyName = t.Company.Name,
                                OrderAdditionalDetails = t.OrderAdditionalDetail == null ? null : new
                                {
                                    t.OrderAdditionalDetail.Allowance,
                                    t.OrderAdditionalDetail.BOLInvoicePreferenceId,
                                    t.OrderAdditionalDetail.IsDriverToUpdateBOL,
                                    t.OrderAdditionalDetail.Notes,
                                    t.OrderAdditionalDetail.IsFuelSurcharge,
                                    t.OrderAdditionalDetail.FuelSurchagePricingType
                                },
                                t.IsFTL,
                                IsDriverToUpdateBOL = t.OrderAdditionalDetail != null ? t.OrderAdditionalDetail.IsDriverToUpdateBOL : false,
                                TerminalAddress = t.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                || t.TerminalId == null ? null : new AddressViewModel
                                {
                                    Address = t.MstExternalTerminal.Address,
                                    City = t.MstExternalTerminal.City,
                                    StateCode = t.MstExternalTerminal.StateCode,
                                    CountryCode = t.MstExternalTerminal.CountryCode,
                                    ZipCode = t.MstExternalTerminal.ZipCode,
                                    CountyName = t.MstExternalTerminal.CountyName
                                },
                                Job = new
                                {
                                    t.FuelRequest.Job.Name,
                                    t.FuelRequest.Job.StateId,
                                    t.FuelRequest.Job.ZipCode,
                                    t.FuelRequest.Job.TimeZoneName,
                                    t.FuelRequest.Job.LocationType,
                                    JobAddrsss = new AddressViewModel
                                    {
                                        Address = t.FuelRequest.Job.Address,
                                        City = t.FuelRequest.Job.City,
                                        StateCode = t.FuelRequest.Job.MstState.Code,
                                        CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                        ZipCode = t.FuelRequest.Job.ZipCode,
                                        CountyName = t.FuelRequest.Job.CountyName
                                    },
                                    CountryCurrency = t.FuelRequest.Job.MstCountry.Currency,
                                    t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                    t.FuelRequest.Job.CompanyId,
                                    t.FuelRequest.Job.JobBudget.IsAssetTracked,
                                    t.FuelRequest.Job.JobBudget.IsTaxExempted,
                                    BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                    ApprovalUserId = t.FuelRequest.Job.IsApprovalWorkflowEnabled ? t.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t1 => t1.IsActive).UserId : 0
                                },
                                FuelRequest = new
                                {
                                    t.FuelRequest.Id,
                                    t.FuelRequest.JobId,
                                    t.FuelRequest.UoM,
                                    t.FuelRequest.Currency,
                                    t.FuelRequest.FuelTypeId,
                                    t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                    t.FuelRequest.MstProduct.MappedParentId,
                                    t.FuelRequest.MstProduct.ProductCode,
                                    t.FuelRequest.MstProduct.ProductTypeId,
                                    t.FuelRequest.PricingTypeId,
                                    t.FuelRequest.FuelDescription,
                                    t.FuelRequest.FuelRequestFees,
                                    t.FuelRequest.MaxQuantity,
                                    t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                    t.FuelRequest.FreightOnBoardTypeId
                                },
                                t.FuelRequest.FuelRequestPricingDetail,
                                t.FuelRequest.FuelRequestDetail,
                                prevSplitDrop = t.Invoices.Where(x => !string.IsNullOrEmpty(DriverDropViewModel.SplitLoadChainId)
                                                                            && x.InvoiceXAdditionalDetail.SplitLoadChainId == DriverDropViewModel.SplitLoadChainId
                                                                            && x.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && x.IsActive)
                                                              .Select(t1 => new { t1.InvoiceXAdditionalDetail.SplitLoadSequence, t1.TrackableScheduleId, FtlDetailId = t1.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault() })
                                                              .OrderByDescending(t1 => t1.SplitLoadSequence)
                                                              .FirstOrDefault()
                            }).FirstOrDefaultAsync();
                if (order != null)
                {
                    var timeZoneName = order.Job.TimeZoneName;
                    viewModel.DropStartDate = DriverDropViewModel.DropStartDate.ToTargetDateTimeOffset(timeZoneName);
                    viewModel.DropEndDate = DriverDropViewModel.DropEndDate.ToTargetDateTimeOffset(timeZoneName);
                    viewModel.UoM = order.FuelRequest.UoM;
                    viewModel.Currency = order.FuelRequest.Currency;
                    viewModel.PoNumber = order.PoNumber;
                    if (order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketManual)
                    {
                        // Set mobile DDT type for invoce if DDT is selected on order as default invoice type
                        viewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                    }
                    viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = order.FuelRequest.FuelRequestFees.ToFeesViewModel();
                    viewModel.IsWetHosingDelivery = viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.WetHoseFee).ToString() && t.DiscountLineItemId == null);
                    viewModel.IsOverWaterDelivery = viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.OverWaterFee).ToString() && t.DiscountLineItemId == null);

                    // Update Demurrage start time and end time coming from mobile
                    viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = DriverDropViewModel.DemurrageDetails.UpdateDemurrageFees(viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees);

                    if (DriverDropViewModel.FuelTruckRetainDetails != null)
                    {
                        // Update Fuel Truck start time and end time coming from mobile
                        viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = DriverDropViewModel.FuelTruckRetainDetails.UpdateFuelTruckRetainFees(viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees);
                    }
                    else
                    {
                        //If Fuel Truck Retain is not set from mobile then remove from fee view model
                        viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Remove(viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == ((int)FeeType.Retain).ToString()));
                    }

                    //If Split Tank is not enabled from mobile then remove from fee view model
                    if (!DriverDropViewModel.IsSplitTank)
                    {
                        viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Remove(viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == ((int)FeeType.SplitTank).ToString()));
                    }

                    viewModel.TimeZoneName = timeZoneName;
                    viewModel.PaymentTermId = order.PaymentTermId;
                    viewModel.NetDays = order.NetDays;
                    viewModel.FuelTypeId = order.FuelRequest.FuelTypeId;
                    viewModel.TypeOfFuel = order.FuelRequest.ProductDisplayGroupId;
                    viewModel.MappedParentFuelTypeId = order.FuelRequest.MappedParentId;
                    viewModel.FuelProductCode = order.FuelRequest.ProductCode;
                    if (viewModel.TypeOfFuel == (int)ProductDisplayGroups.OtherFuelType)
                    {
                        viewModel.OtherProductTaxes = order.OrderTaxDetails
                            .Where(t => t.IsActive).Select(t => new TaxViewModel
                            {
                                TaxAmount = t.TaxRate,
                                TaxPricingTypeId = t.TaxPricingTypeId,
                                TaxDescription = t.TaxDescription
                            }).ToList();
                    }

                    viewModel.IsBuySellOrder = order.IsBuySellOrder;
                    viewModel.SupplierPreferredInvoiceTypeId = order.DefaultInvoiceType == (int)InvoiceType.Manual ? (int)InvoiceType.MobileApp : (int)InvoiceType.DigitalDropTicketMobileApp;
                    if (viewModel.IsBuySellOrder)
                    {
                        viewModel.BrokerMarkUp = order.ExternalBrokerBuySellDetail.BrokerMarkUp;
                        viewModel.SupplierMarkUp = order.ExternalBrokerBuySellDetail.SupplierMarkUp;
                    }

                    viewModel.CountryCurrency = order.Job.CountryCurrency;
                    viewModel.BuyerCompanyId = order.BuyerCompanyId;
                    viewModel.BuyerCompanyName = order.BuyerCompanyName;
                    viewModel.JobId = order.FuelRequest.JobId;
                    viewModel.JobName = order.Job.Name;
                    viewModel.JobStateId = order.Job.StateId;
                    viewModel.IsVariousFobOrigin = order.FuelRequest.FreightOnBoardTypeId == (int)FreightOnBoardTypes.Terminal && order.Job.LocationType == JobLocationTypes.Various;
                    viewModel.JobCompanyId = order.Job.CompanyId;
                    viewModel.IsAssetTracked = order.Job.IsAssetTracked;
                    viewModel.DeliveryTypeId = order.FuelRequest.DeliveryTypeId;
                    viewModel.MaxQuantity = order.FuelRequest.MaxQuantity;
                    viewModel.BrokeredMaxQuantity = order.BrokeredMaxQuantity;
                    viewModel.AcceptedCompanyId = order.AcceptedCompanyId;
                    viewModel.SupplierCompanyName = order.SupplierCompanyName;
                    viewModel.PricingTypeId = order.FuelRequest.PricingTypeId;
                    viewModel.IsSalesTaxExempted = order.Job.IsTaxExempted;

                    viewModel.IsFTL = order.IsFTL;
                    viewModel.IsDriverToUpdateBol = order.IsDriverToUpdateBOL;
                    viewModel.OrderAcceptedBy = order.AcceptedBy;
                    viewModel.ApprovalUserId = order.Job.ApprovalUserId;
                    viewModel.IsApprovalWorkflowEnabledForJob = order.Job.IsApprovalWorkflowEnabled;
                    viewModel.ProductDescription = order.FuelRequest.FuelDescription;
                    viewModel.IsSalesTaxExempted = order.Job.IsTaxExempted;
                    viewModel.ExternalBrokerId = order.ExternalBrokerId ?? 0;
                    viewModel.IsThirdPartyHardwareUsed = order.ExternalBrokerOrderDetail != null;
                    if (order.ExternalBrokerOrderDetail != null)
                    {
                        viewModel.ExternalBrokeredOrder = order.ExternalBrokerOrderDetail.ToViewModel(viewModel.ExternalBrokeredOrder);
                    }
                    SetTaxExemptLicences(viewModel, order);

                    viewModel.TerminalId = order.TerminalId;
                    viewModel.CityGroupTerminalId = order.CityGroupTerminalId;
                    viewModel.IsBrokeredChainOrder = order.BuyerCompanyId != order.Job.CompanyId;
                    viewModel.JobAddess = order.Job.JobAddrsss;
                    viewModel.TerminalAddress = order.TerminalAddress;
                    viewModel.QuantityIndicatorTypeId = viewModel.FuelRequestPricingDetail.PricingQuantityIndicatorTypeId ?? 0;
                    if (viewModel.AdditionalDetail == null)
                    {
                        viewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                        viewModel.AdditionalDetail.SupplierAllowance = viewModel.SupplierAllowance;
                        viewModel.AdditionalDetail.ActualDropQuantity = viewModel.FuelDropped;
                    }
                    if (DriverDropViewModel.IsSplitLoad || !string.IsNullOrEmpty(DriverDropViewModel.SplitLoadChainId))
                    {
                        viewModel.AdditionalDetail.SplitLoadChainId = DriverDropViewModel.SplitLoadChainId;
                        viewModel.AdditionalDetail.SplitLoadSequence = ((order.prevSplitDrop?.SplitLoadSequence) ?? 0) + 1;
                        viewModel.TrackableScheduleId = (order.prevSplitDrop?.TrackableScheduleId) ?? DriverDropViewModel.TrackableScheduleId;
                    }
                    if (order.OrderAdditionalDetails != null)
                    {
                        viewModel.SupplierAllowance = order.OrderAdditionalDetails.Allowance ?? 0; //might require more values like, carrier, BOL preference etc
                        viewModel.AdditionalDetail.Notes = order.OrderAdditionalDetails.Notes;
                    }

                    if (order.FuelRequestPricingDetail != null)
                    {
                        // get pricing details from pricing service
                        PricingRequestDetailResponseViewModel pricingDetails = new PricingRequestDetailResponseViewModel();  // need to check is this needed ?
                        if (order.FuelRequestPricingDetail.RequestPriceDetailId > 0)
                        {
                            var orderDomain = new OrderDomain(this);
                            pricingDetails = await orderDomain.GetRequestPricingDetail(order.FuelRequestPricingDetail.RequestPriceDetailId, (int)viewModel.Currency, order.AcceptedCompanyId, viewModel.FuelTypeId, viewModel.JobStateId);
                            if (pricingDetails != null)
                            {
                                viewModel.PricingTypeId = pricingDetails.PricingTypeId;
                                viewModel.RackAvgTypeId = pricingDetails.RackAvgTypeId;
                                viewModel.PricePerGallon = pricingDetails.PricePerGallon;
                                viewModel.SupplierCost = pricingDetails.SupplierCost;
                            }
                        }

                        viewModel.FuelRequestPricingDetail = order.FuelRequestPricingDetail.ToViewModel(viewModel.FuelRequestPricingDetail);
                        viewModel.FuelRequestPricingDetail.PricingQuantityIndicatorTypeId = order.FuelRequestDetail.PricingQuantityIndicatorTypeId;
                        if (viewModel.BolDetails != null)
                        {
                            viewModel.BolDetails.Id = (order.prevSplitDrop?.FtlDetailId?.Id) ?? DriverDropViewModel.BolDetails.Id;
                            if (viewModel.BolDetails.Id > 0)
                            {
                                viewModel.BolDetails = Context.DataContext.InvoiceFtlDetails.Where(t => t.Id == viewModel.BolDetails.Id).FirstOrDefault()?.ToViewModel();
                            }


                            if (order.IsFTL)
                            {
                                if (!DriverDropViewModel.IsSplitLoad && string.IsNullOrEmpty(DriverDropViewModel.SplitLoadChainId))
                                {
                                    //if FTL order then assign DroppedGallons to Net/Gross
                                    if (DriverDropViewModel.FuelPickLocation.PickupLocationType == PickupLocationType.Terminal)
                                    {
                                        viewModel.FuelDropped = viewModel.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net ? viewModel.BolDetails.NetQuantity : viewModel.BolDetails.GrossQuantity;
                                    }
                                    else
                                    {
                                        viewModel.FuelDropped = viewModel.BolDetails?.LiftQuantity;
                                    }
                                }
                            }
                            viewModel.BolDetails.ImageId = DriverDropViewModel.BolDetails.ImageId;
                        }
                    }

                    if (order.OrderAdditionalDetails.IsFuelSurcharge && order.OrderAdditionalDetails.FuelSurchagePricingType.HasValue)
                    {
                        viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee = new FuelSurchargeFreightFeeViewModel();
                        var surchareFee = order.FuelRequest.FuelRequestFees.Where(t => t.FeeTypeId.Equals(((int)FeeType.SurchargeFreightFee))).FirstOrDefault();
                        if (surchareFee != null)
                        {
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsSurchargeApplicable = order.OrderAdditionalDetails.IsFuelSurcharge;
                            var fscproductType = order.FuelRequest.ProductTypeId.GetFuelSurchargeProductType();
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeProductType = fscproductType;
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeFreightCost = surchareFee.Fee;
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargePricingType = (FuelSurchagePricingType)order.OrderAdditionalDetails.FuelSurchagePricingType.Value;
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeEiaPrice = new EIAPriceUpdateDomain().GetEIAPrice((FuelSurchagePricingType)order.OrderAdditionalDetails.FuelSurchagePricingType.Value, fscproductType, viewModel.DropEndDate.Date);
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.GallonsDelivered = viewModel.FuelDropped ?? 0;
                            //get distance from mob api
                            var distance = DriverDropViewModel.FuelSurchargeDistance;
                            if (surchareFee.FeeSubTypeId == (int)FeeSubType.ByDistance)
                            {
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsFeeByDistance = true;
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.Distance = distance;

                                var byQantity = surchareFee.FeeByQuantities.FirstOrDefault(t => distance >= t.MinQuantity && distance <= (t.MaxQuantity ?? distance));
                                if (byQantity != null)
                                    viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeFreightCost = byQantity.Fee;
                            }

                            var surchargeTableRecord = new FuelSurchargeDomain().GetFuelSurchargeRecordForEaiPrice(viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeEiaPrice, order.AcceptedCompanyId, order.BuyerCompanyId, viewModel.DropEndDate, fscproductType);
                            if (surchargeTableRecord != null)
                            {
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargePercentage = surchargeTableRecord.FuelSurchargeStartPercentage;
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeTableRangeStart = surchargeTableRecord.PriceRangeStartValue;
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeTableRangeEnd = surchargeTableRecord.PriceRangeEndValue;
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.TotalFuelSurchargeFee = GetFuelSurchageFrieghtFee(surchargeTableRecord.FuelSurchargeStartPercentage, viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeFreightCost, viewModel.FuelDropped);
                                //(surchargeTableRecord.FuelSurchargeStartPercentage / 100) * viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeFreightCost * viewModel.FuelDropped ?? 0;
                            }
                        }
                    }
                    viewModel.JobStateId = order.Job.StateId;

                    viewModel.PickupLocation = GetDispatchLocationDetails(DriverDropViewModel.FuelPickLocation.Latitude, DriverDropViewModel.FuelPickLocation.Longitude, DriverDropViewModel, viewModel.Currency, (int)LocationType.PickUp);
                    if (DriverDropViewModel.FuelDropLocation.Latitude != 0 && DriverDropViewModel.FuelDropLocation.Longitude != 0)
                    {
                        viewModel.DropLocation = GetDispatchLocationDetails(DriverDropViewModel.FuelDropLocation.Latitude, DriverDropViewModel.FuelDropLocation.Longitude, DriverDropViewModel, viewModel.Currency, (int)LocationType.Drop);
                    }
                    else
                    {
                        viewModel.DropLocation = GetDispatchLocationDetails(viewModel, DriverDropViewModel, viewModel.Currency, (int)LocationType.Drop);
                    }
                    viewModel.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetCreateFtlMobileInvoiceRequestViewModelAsync", ex.Message, ex);
            }
            return viewModel;
        }

        private DispatchLocationViewModel GetDispatchLocationDetails(MobileInvoiceCreateRequestViewModel viewModel, FtlDriverDropOrderViewModel driverDropViewModel, Currency currency, int locationType)
        {
            var response = new DispatchLocationViewModel();
            try
            {
                response.LocationType = locationType;

                var variousDropDetails = Context.DataContext.MstStates.Where(t => t.Id == viewModel.JobStateId)
                            .Select(t => new { t.Id, t.Code, CounryCode = t.MstCountry.Code }).FirstOrDefault();
                if (variousDropDetails != null)
                {
                    response.StateCode = variousDropDetails.Code;
                    response.StateId = variousDropDetails.Id;
                    response.CountryCode = variousDropDetails.CounryCode;
                }
                response.IsValidAddress = true;
                response.OrderId = driverDropViewModel.OrderId;
                response.CreatedBy = driverDropViewModel.Driver.UserId;
                response.CreatedDate = DateTimeOffset.Now;
                response.DeliveryScheduleId = driverDropViewModel.DeliveryScheduleId;
                response.TrackableScheduleId = driverDropViewModel.TrackableScheduleId;
                response.Currency = currency;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetDispatchLocationDetails", $"Invalid Address: OrderId:-{driverDropViewModel.OrderId} UserId:- {driverDropViewModel.Driver.UserId}", ex);
            }
            return response;
        }

        private DispatchLocationViewModel GetDispatchLocationDetails(decimal latitude, decimal longitude, FtlDriverDropOrderViewModel DriverDropViewModel, Currency currency, int locationType)
        {
            var response = new DispatchLocationViewModel();
            try
            {
                var point = GoogleApiDomain.GetAddress(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                if (point != null)
                {
                    response.Address = point.Address;
                    response.City = point.City;
                    response.ZipCode = point.ZipCode;
                    response.CountyName = point.CountyName;
                    response.LocationType = locationType;

                    var country = Context.DataContext.MstCountries.Single(t => t.Name.ToLower().Contains(point.CountryName.ToLower()));
                    response.CountryCode = country != null ? country.Code : string.Empty;

                    response.StateCode = point.StateCode;
                    var state = Context.DataContext.MstStates.FirstOrDefault(t => t.Code.ToLower() == point.StateCode.ToLower());
                    response.StateId = state != null ? state.Id : 0;

                    response.IsValidAddress = true;
                    response.OrderId = DriverDropViewModel.OrderId;
                    response.CreatedBy = DriverDropViewModel.Driver.UserId;
                    response.CreatedDate = DateTimeOffset.Now;
                    response.DeliveryScheduleId = DriverDropViewModel.DeliveryScheduleId;
                    response.TrackableScheduleId = DriverDropViewModel.TrackableScheduleId;
                    response.Currency = currency;
                    response.PickupLocationType = DriverDropViewModel.FuelPickLocation != null ? DriverDropViewModel.FuelPickLocation.PickupLocationType : PickupLocationType.Terminal;
                }
                else
                {
                    LogManager.Logger.WriteDebug("InvoiceCreateDomain", "GetDispatchLocationDetails", $"Invalid Address: latitude:-:{latitude} longitude:-{longitude} OrderId:-{DriverDropViewModel.OrderId} UserId:- {DriverDropViewModel.Driver.UserId}");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetDispatchLocationDetails", $"Invalid Address: latitude:-:{latitude} longitude:-{longitude} OrderId:-{DriverDropViewModel.OrderId} UserId:- {DriverDropViewModel.Driver.UserId}", ex);
            }
            return response;
        }

        private DispatchLocationViewModel GetPickupLocationDetails(decimal latitude, decimal longitude, DriverDropOrderViewModel DriverDropViewModel, Currency currency, int locationType)
        {
            var response = new DispatchLocationViewModel();
            try
            {
                var point = GoogleApiDomain.GetAddress(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                if (point != null)
                {
                    response.Address = point.Address;
                    response.City = point.City;
                    response.ZipCode = point.ZipCode;
                    response.CountyName = point.CountyName;
                    response.LocationType = locationType;

                    var country = Context.DataContext.MstCountries.Single(t => t.Name.ToLower().Contains(point.CountryName.ToLower()));
                    response.CountryCode = country != null ? country.Code : string.Empty;

                    response.StateCode = point.StateCode;
                    response.StateId = Context.DataContext.MstStates.Single(t => t.Code.ToLower() == point.StateCode.ToLower()).Id;
                    response.IsValidAddress = true;
                    response.OrderId = DriverDropViewModel.OrderId;
                    response.CreatedBy = DriverDropViewModel.Driver.UserId;
                    response.CreatedDate = DateTimeOffset.Now;
                    response.DeliveryScheduleId = DriverDropViewModel.DeliveryScheduleId;
                    response.TrackableScheduleId = DriverDropViewModel.TrackableScheduleId;
                    response.Currency = currency;
                    response.PickupLocationType = DriverDropViewModel.FuelPickLocation != null ? DriverDropViewModel.FuelPickLocation.PickupLocationType : PickupLocationType.Terminal;
                }
                else
                {
                    LogManager.Logger.WriteDebug("InvoiceCreateDomain", "GetPickupLocationDetails", $"Invalid Address: latitude:-:{latitude} longitude:-{longitude} OrderId:-{DriverDropViewModel.OrderId} UserId:- {DriverDropViewModel.Driver.UserId}");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetPickupLocationDetails", $"Invalid Address: latitude:-:{latitude} longitude:-{longitude} OrderId:-{DriverDropViewModel.OrderId} UserId:- {DriverDropViewModel.Driver.UserId}", ex);
            }
            return response;
        }

        public async Task<ManualInvoiceCreateRequestViewModel> GetCreateManualInvoiceRequestViewModelAsync(ManualInvoiceViewModel manualInvoiceModel)
        {
            var viewModel = new ManualInvoiceCreateRequestViewModel();
            try
            {
                viewModel = manualInvoiceModel.ToManualInvoiceViewModel();

                var order = await Context.DataContext.Orders.Where(t => t.Id == manualInvoiceModel.OrderId)
                            .Select(t => new
                            {
                                t.PoNumber,
                                t.DefaultInvoiceType,
                                t.OrderTaxDetails,
                                t.BuyerCompanyId,
                                t.AcceptedCompanyId,
                                t.ExternalBrokerId,
                                t.ExternalBrokerOrderDetail,
                                TerminalId = t.TerminalId,
                                t.CityGroupTerminalId,
                                t.BrokeredMaxQuantity,
                                t.AcceptedBy,
                                t.SignatureEnabled,
                                SupplierTaxExemptLicence = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                t.IsEndSupplier,
                                IsBuySellOrder = t.ExternalBrokerBuySellDetail != null,
                                t.ExternalBrokerBuySellDetail,
                                BuyerCompanyName = t.BuyerCompany.Name,
                                SupplierCompanyName = t.Company.Name,
                                OrderAdditionalDetails = t.OrderAdditionalDetail == null ? null : new
                                {
                                    t.OrderAdditionalDetail.Allowance,
                                    t.OrderAdditionalDetail.BOLInvoicePreferenceId,
                                    t.OrderAdditionalDetail.IsDriverToUpdateBOL,
                                    t.OrderAdditionalDetail.IsFuelSurcharge,
                                    t.OrderAdditionalDetail.FuelSurchagePricingType
                                },
                                t.IsFTL,
                                TerminalAddress = t.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                || t.TerminalId == null ? null : new AddressViewModel
                                {
                                    Address = t.MstExternalTerminal.Address,
                                    City = t.MstExternalTerminal.City,
                                    StateCode = t.MstExternalTerminal.StateCode,
                                    CountryCode = t.MstExternalTerminal.CountryCode,
                                    ZipCode = t.MstExternalTerminal.ZipCode,
                                    CountyName = t.MstExternalTerminal.CountyName
                                },
                                TerminalStateId = t.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                || t.TerminalId == null ? 0 : t.MstExternalTerminal.StateId,
                                Job = new
                                {
                                    t.FuelRequest.Job.Name,
                                    CountryId = t.FuelRequest.Job.CountryId,
                                    JobAddress = new AddressViewModel
                                    {
                                        Address = t.FuelRequest.Job.Address,
                                        City = t.FuelRequest.Job.City,
                                        StateCode = t.FuelRequest.Job.MstState.Code,
                                        CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                        ZipCode = t.FuelRequest.Job.ZipCode,
                                        CountyName = t.FuelRequest.Job.CountyName
                                    },
                                    t.FuelRequest.Job.StateId,
                                    t.FuelRequest.Job.MstState.QuantityIndicatorTypeId,
                                    t.FuelRequest.Job.ZipCode,
                                    t.FuelRequest.Job.TimeZoneName,
                                    t.FuelRequest.Job.CompanyId,
                                    t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                    t.FuelRequest.Job.Id,
                                    t.FuelRequest.Job.LocationType,
                                    CompanyName = t.FuelRequest.Job.Company.Name,
                                    CountryCurrency = t.FuelRequest.Job.MstCountry.Currency,
                                    t.FuelRequest.Job.JobBudget.IsAssetTracked,
                                    BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                    ApprovalUser = t.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t1 => t1.IsActive),
                                    t.FuelRequest.Job.JobBudget.IsTaxExempted
                                },
                                FuelRequest = new
                                {
                                    t.FuelRequest.Id,
                                    t.FuelRequest.JobId,
                                    t.FuelRequest.UoM,
                                    t.FuelRequest.Currency,
                                    t.FuelRequest.FuelTypeId,
                                    t.FuelRequest.MstProduct.ProductTypeId,
                                    t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                    t.FuelRequest.MstProduct.MappedParentId,
                                    t.FuelRequest.MstProduct.ProductCode,
                                    t.FuelRequest.PricingTypeId,
                                    t.FuelRequest.FuelDescription,
                                    t.FuelRequest.MaxQuantity,
                                    t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                    t.FuelRequest.FuelRequestDetail.StartDate,
                                    t.FuelRequest.FuelRequestDetail.EndTime,
                                    t.FuelRequest.FuelRequestDetail.IsBolImageRequired,
                                    t.FuelRequest.FuelRequestDetail.IsDropImageRequired,
                                    t.FuelRequest.Job.SignatureEnabled,
                                    t.FuelRequest.FreightOnBoardTypeId
                                },
                                t.FuelRequest.FuelRequestPricingDetail
                            }).FirstOrDefaultAsync();
                if (order != null)
                {
                    var timeZoneName = order.Job.TimeZoneName;
                    viewModel.UoM = order.FuelRequest.UoM;
                    viewModel.Currency = order.FuelRequest.Currency;
                    viewModel.PoNumber = order.PoNumber;
                    viewModel.TimeZoneName = timeZoneName;
                    viewModel.FuelTypeId = order.FuelRequest.FuelTypeId;
                    viewModel.MappedParentFuelTypeId = order.FuelRequest.MappedParentId;
                    viewModel.FuelProductCode = order.FuelRequest.ProductCode;
                    viewModel.TypeOfFuel = order.FuelRequest.ProductDisplayGroupId;
                    if (viewModel.TypeOfFuel == (int)ProductDisplayGroups.OtherFuelType)
                    {
                        viewModel.OtherProductTaxes = order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketManual ? order.OrderTaxDetails
                            .Where(t => t.IsActive).Select(t => new TaxViewModel
                            {
                                TaxAmount = t.TaxRate,
                                TaxPricingTypeId = t.TaxPricingTypeId,
                                TaxDescription = t.TaxDescription
                            }).ToList() : manualInvoiceModel.Taxes;
                    }

                    viewModel.JobCompanyName = order.Job.CompanyName;
                    viewModel.IsBuySellOrder = order.IsBuySellOrder;
                    if (viewModel.IsBuySellOrder)
                    {
                        viewModel.BrokerMarkUp = order.ExternalBrokerBuySellDetail.BrokerMarkUp;
                        viewModel.SupplierMarkUp = order.ExternalBrokerBuySellDetail.SupplierMarkUp;
                    }
                    viewModel.CountryId = order.Job.CountryId;
                    viewModel.CountryCurrency = order.Job.CountryCurrency;
                    viewModel.BuyerCompanyId = order.BuyerCompanyId;
                    viewModel.BuyerCompanyName = order.BuyerCompanyName;
                    viewModel.JobId = order.FuelRequest.JobId;
                    viewModel.JobName = order.Job.Name;
                    viewModel.JobStateId = order.Job.StateId;
                    viewModel.IsVariousFobOrigin = order.FuelRequest.FreightOnBoardTypeId == (int)FreightOnBoardTypes.Terminal && order.Job.LocationType == JobLocationTypes.Various;
                    viewModel.JobCompanyId = order.Job.CompanyId;
                    viewModel.IsAssetTracked = order.Job.IsAssetTracked;
                    viewModel.PricingTypeId = order.FuelRequest.PricingTypeId;
                    viewModel.DeliveryTypeId = order.FuelRequest.DeliveryTypeId;
                    viewModel.MaxQuantity = order.FuelRequest.MaxQuantity;
                    viewModel.BrokeredMaxQuantity = order.BrokeredMaxQuantity;
                    viewModel.AcceptedCompanyId = order.AcceptedCompanyId;
                    viewModel.SupplierCompanyName = order.SupplierCompanyName;
                    viewModel.DeliveryStartDate = order.FuelRequest.StartDate;
                    viewModel.DeliveryEndTime = order.FuelRequest.EndTime;
                    viewModel.IsSalesTaxExempted = order.Job.IsTaxExempted;

                    viewModel.OrderAcceptedBy = order.AcceptedBy;
                    if (order.Job.ApprovalUser != null)
                    {
                        viewModel.ApprovalUserId = order.Job.ApprovalUser.Id;
                        viewModel.ApprovalUserOnboardedType = order.Job.ApprovalUser.User.OnboardedTypeId;
                        viewModel.ApprovalUserName = $"{order.Job.ApprovalUser.User.FirstName} {order.Job.ApprovalUser.User.LastName}";
                    }
                    viewModel.IsApprovalWorkflowEnabledForJob = order.Job.IsApprovalWorkflowEnabled;
                    var isSignatureEnabled = order.FuelRequest.SignatureEnabled ? order.FuelRequest.SignatureEnabled : order.SignatureEnabled;
                    SetImageFlags(viewModel, order.FuelRequest.IsBolImageRequired, order.FuelRequest.IsDropImageRequired, isSignatureEnabled);
                    viewModel.ProductDescription = order.FuelRequest.FuelDescription;
                    viewModel.ExternalBrokerId = order.ExternalBrokerId ?? 0;
                    viewModel.IsThirdPartyHardwareUsed = order.ExternalBrokerOrderDetail != null;
                    if (order.ExternalBrokerOrderDetail != null)
                    {
                        viewModel.ExternalBrokeredOrder = order.ExternalBrokerOrderDetail.ToViewModel(viewModel.ExternalBrokeredOrder);
                    }
                    SetTaxExemptLicences(viewModel, order);
                    viewModel.TerminalId = order.TerminalId;
                    viewModel.CityGroupTerminalId = order.CityGroupTerminalId;
                    viewModel.IsBrokeredChainOrder = order.BuyerCompanyId != order.Job.CompanyId;
                    viewModel.JobAddess = order.Job.JobAddress;
                    viewModel.TerminalAddress = order.TerminalAddress;
                    viewModel.TerminalStateId = order.TerminalStateId;
                    if (viewModel.AdditionalDetail != null)
                        viewModel.SupplierAllowance = viewModel.AdditionalDetail.SupplierAllowance ?? 0;

                    viewModel.IsFTL = order.IsFTL;
                    viewModel.StateLevelPricingIndicator = order.Job.QuantityIndicatorTypeId;
                    if (order.FuelRequestPricingDetail != null)
                    {
                        // get pricing details from pricing service
                        PricingRequestDetailResponseViewModel pricingDetails = new PricingRequestDetailResponseViewModel();  // need to check is this needed ?
                        if (order.FuelRequestPricingDetail.RequestPriceDetailId > 0)
                        {
                            var orderDomain = new OrderDomain(this);
                            pricingDetails = await orderDomain.GetRequestPricingDetail(order.FuelRequestPricingDetail.RequestPriceDetailId, (int)viewModel.Currency, order.AcceptedCompanyId, order.FuelRequest.FuelTypeId, viewModel.JobStateId);
                            if (pricingDetails != null)
                            {
                                viewModel.PricingTypeId = pricingDetails.PricingTypeId;
                                viewModel.RackAvgTypeId = pricingDetails.RackAvgTypeId;
                                viewModel.PricePerGallon = pricingDetails.PricePerGallon;
                                viewModel.SupplierCost = pricingDetails.SupplierCost;
                            }
                        }

                        viewModel.FuelRequestPricingDetail = order.FuelRequestPricingDetail.ToViewModel(viewModel.FuelRequestPricingDetail);
                    }

                    viewModel.SupplierPreferredInvoiceTypeId = order.DefaultInvoiceType;

                    //// set updated terminal details on creating invoice
                    if (manualInvoiceModel.BolDetails.TerminalId != 0 && manualInvoiceModel.BolDetails.TerminalId != order.TerminalId)
                    {
                        GetTerminalDetailsById(viewModel, manualInvoiceModel);
                    }

                    var terminalId = 0;
                    if (manualInvoiceModel.BolDetails.TerminalId.HasValue)
                        terminalId = manualInvoiceModel.BolDetails.TerminalId.Value;
                    else
                        terminalId = order.TerminalId.HasValue ? order.TerminalId.Value : 0;

                    var terminal = Context.DataContext.MstExternalTerminals.FirstOrDefault(t => t.Id == terminalId);

                    if (viewModel.BolDetails != null)
                    {
                        viewModel.BolDetails.TerminalId = terminalId;
                        viewModel.BolDetails.TerminalName = terminal != null ? terminal.Name : string.Empty;
                        viewModel.BolDetails.CityGroupTerminalId = order.CityGroupTerminalId;
                        viewModel.BolDetails.IsDeleted = false;
                        viewModel.BolDetails.IsActive = true;
                        viewModel.BolDetails.FuelTypeId = order.FuelRequest.FuelTypeId;
                        viewModel.BolDetails.CreatedBy = manualInvoiceModel.userId;
                        viewModel.BolDetails.CreatedDate = manualInvoiceModel.CreatedDate;
                        viewModel.BolDetails.PricePerGallon = viewModel.PricePerGallon;
                        viewModel.BolDetails.PickupLocationType = !string.IsNullOrEmpty(viewModel.BolDetails.LiftTicketNumber) ? PickupLocationType.BulkPlant : PickupLocationType.Terminal;

                        if (manualInvoiceModel.PickUpAddress != null && manualInvoiceModel.PickUpAddress.IsAddressAvailable)
                        {
                            //If Pickup location is supplied at Order
                            viewModel.BolDetails.Address = manualInvoiceModel.PickUpAddress.Address;
                            viewModel.BolDetails.AddressLine2 = manualInvoiceModel.PickUpAddress.AddressLine2;
                            viewModel.BolDetails.AddressLine3 = manualInvoiceModel.PickUpAddress.AddressLine3;
                            viewModel.BolDetails.City = manualInvoiceModel.PickUpAddress.City;
                            viewModel.BolDetails.CountryCode = manualInvoiceModel.PickUpAddress.Country.Code;
                            viewModel.BolDetails.CountyName = manualInvoiceModel.PickUpAddress.CountyName;
                            viewModel.BolDetails.Latitude = manualInvoiceModel.PickUpAddress.Latitude;
                            viewModel.BolDetails.Longitude = manualInvoiceModel.PickUpAddress.Longitude;
                            viewModel.BolDetails.SiteName = manualInvoiceModel.PickUpAddress.SiteName;
                            viewModel.BolDetails.StateCode = manualInvoiceModel.PickUpAddress.State.Code;
                            viewModel.BolDetails.StateId = manualInvoiceModel.PickUpAddress.State.Id;
                            viewModel.BolDetails.ZipCode = manualInvoiceModel.PickUpAddress.ZipCode;
                        }
                        else
                        {
                            //If Lift Details are not given in Csv then use Terminal
                            if (terminal != null)
                            {
                                viewModel.BolDetails.Address = terminal.Address;
                                viewModel.BolDetails.City = terminal.City;
                                viewModel.BolDetails.CountryCode = terminal.CountryCode;
                                viewModel.BolDetails.CountyName = terminal.CountyName;
                                viewModel.BolDetails.Latitude = terminal.Latitude;
                                viewModel.BolDetails.Longitude = terminal.Longitude;
                                viewModel.BolDetails.StateCode = terminal.StateCode;
                                viewModel.BolDetails.StateId = terminal.StateId;
                                viewModel.BolDetails.ZipCode = terminal.ZipCode;
                            }
                        }
                    }

                    viewModel.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetCreateManualInvoiceRequestViewModelAsync", ex.Message, ex);
            }
            return viewModel;
        }

        private void GetTerminalDetailsById(ManualInvoiceCreateRequestViewModel viewModel, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            viewModel.TerminalId = manualInvoiceViewModel.BolDetails.TerminalId;
            var terminalDetails = Context.DataContext.MstExternalTerminals.FirstOrDefault(t => t.Id == manualInvoiceViewModel.BolDetails.TerminalId && t.IsActive);
            if (terminalDetails != null)
            {
                viewModel.TerminalAddress = new AddressViewModel() { Address = terminalDetails.Address, City = terminalDetails.City, StateCode = terminalDetails.StateCode, CountryCode = terminalDetails.CountryCode, ZipCode = terminalDetails.ZipCode, CountyName = terminalDetails.CountyName };
                viewModel.TerminalStateId = terminalDetails.StateId;
            }
        }

        private ManualInvoiceCreateRequestViewModel GetBrokerSplitInvoiceRequestViewModelAsync(ManualInvoiceViewModel manualInvoiceModel)
        {
            var viewModel = new ManualInvoiceCreateRequestViewModel();
            try
            {
                viewModel.DropStartDate = new DateTimeOffset(manualInvoiceModel.DeliveryDate.Add(Convert.ToDateTime(manualInvoiceModel.StartTime).TimeOfDay));
                viewModel.DropEndDate = new DateTimeOffset(manualInvoiceModel.DeliveryDate.Add(Convert.ToDateTime(manualInvoiceModel.EndTime).TimeOfDay));
                viewModel.FuelDropped = manualInvoiceModel.FuelDropped ?? 0.0M;

                viewModel.InvoiceTypeId = manualInvoiceModel.InvoiceTypeId;
                viewModel.UserId = manualInvoiceModel.userId;
                viewModel.OrderId = manualInvoiceModel.OrderId;
                viewModel.InvoiceImage = manualInvoiceModel.InvoiceImage;
                if (manualInvoiceModel.SignatureImage != null && !string.IsNullOrWhiteSpace(manualInvoiceModel.SignatureImage?.FilePath))
                    viewModel.Signature = manualInvoiceModel.SignatureImage?.ToCustomerSignature();
                viewModel.BolImage = manualInvoiceModel.BolImage;
                viewModel.TaxAffidavitImage = manualInvoiceModel.TaxAffidavitImage;
                viewModel.BDNImage = manualInvoiceModel.BDNImage;
                viewModel.CoastGuardInspectionImage = manualInvoiceModel.CoastGuardInspectionImage;
                viewModel.InspectionRequestVoucherImage = manualInvoiceModel.InspectionRequestVoucherImage;
                viewModel.DriverId = manualInvoiceModel.DriverId;
                viewModel.TrackableScheduleId = manualInvoiceModel.TrackableScheduleId;
                viewModel.InvoiceStatusId = manualInvoiceModel.StatusId;
                viewModel.AssetDrops = manualInvoiceModel.Assets;
                viewModel.BolDetails = manualInvoiceModel.BolDetails;
                viewModel.DropLocation = manualInvoiceModel.ToDropLocation();
                viewModel.PickupLocation = manualInvoiceModel.ToPickUpLocation();
                viewModel.InvoiceStatusId = manualInvoiceModel.StatusId;
                viewModel.TrackableScheduleId = manualInvoiceModel.TrackableScheduleId;


                var order = Context.DataContext.Orders.Where(t => t.Id == manualInvoiceModel.OrderId)
                            .Select(t => new
                            {
                                t.DefaultInvoiceType,
                                TerminalId = t.TerminalId,
                                t.CityGroupTerminalId,
                                OrderAdditionalDetails = t.OrderAdditionalDetail == null ? null : new
                                {
                                    t.OrderAdditionalDetail.Allowance,
                                    t.OrderAdditionalDetail.BOLInvoicePreferenceId
                                },
                                t.IsFTL,
                                TerminalAddress = t.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                || t.TerminalId == null ? null : new AddressViewModel
                                {
                                    Address = t.MstExternalTerminal.Address,
                                    City = t.MstExternalTerminal.City,
                                    StateCode = t.MstExternalTerminal.StateCode,
                                    CountryCode = t.MstExternalTerminal.CountryCode,
                                    ZipCode = t.MstExternalTerminal.ZipCode,
                                    CountyName = t.MstExternalTerminal.CountyName
                                },
                                TerminalStateId = t.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                || t.TerminalId == null ? 0 : t.MstExternalTerminal.StateId,
                                t.FuelRequest.JobId,
                                t.FuelRequest.Job.TimeZoneName,
                                t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                t.FuelRequest.FuelRequestDetail.EndTime,
                                t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId
                            }).FirstOrDefault();
                if (order != null)
                {
                    viewModel.TimeZoneName = order.TimeZoneName;
                    viewModel.JobId = order.JobId;
                    viewModel.InvoiceTypeId = order.DefaultInvoiceType;
                    viewModel.TerminalId = order.TerminalId;
                    viewModel.CityGroupTerminalId = order.CityGroupTerminalId;
                    viewModel.TerminalAddress = order.TerminalAddress;
                    viewModel.TerminalStateId = order.TerminalStateId;

                    viewModel.DeliveryTypeId = order.DeliveryTypeId;
                    viewModel.DeliveryEndTime = order.EndTime;
                    viewModel.FuelRequestPricingDetail.RequestPriceDetailId = order.RequestPriceDetailId;
                }

                if (viewModel.AdditionalDetail == null)
                    viewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();

                viewModel.AdditionalDetail.SplitLoadSequence = manualInvoiceModel.SplitLoadSequence;
                viewModel.AdditionalDetail.SplitLoadChainId = manualInvoiceModel.SplitLoadChainId;
                if (!string.IsNullOrWhiteSpace(manualInvoiceModel.Notes))
                {
                    viewModel.AdditionalDetail.Notes = manualInvoiceModel.Notes;
                }
                viewModel.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetCreateManualInvoiceRequestViewModelAsync", ex.Message, ex);
            }
            return viewModel;
        }

        //private void SetSurchargePropertiesToInvoiceModel(ManualInvoiceCreateRequestViewModel viewModel, ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel)
        //{
        //    var surchareFee = viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Where(t => t.FeeTypeId.Equals(((int)FeeType.SurchargeFreightFee))).FirstOrDefault();
        //    if(surchareFee != null)
        //    {
        //        invoiceModel.SurchargeEaiPrice = new EIAPriceUpdateDomain().GetEaiPrice(invoiceModel.SurchargePricingType, invoiceModel.SurchargeProductType, viewModel.DropEndDate.Date);
        //        var surchargeTableRecord = new FuelSurchargeDomain().GetFuelSurchargeRecordForEaiPrice(invoiceModel.SurchargeEaiPrice, viewModel.AcceptedCompanyId, viewModel.BuyerCompanyId, viewModel.DropEndDate, invoiceModel.SurchargeProductType);
        //        if(surchargeTableRecord != null)
        //        {
        //            invoiceModel.SurchargePercentage = surchargeTableRecord.FuelSurchargeStartPercentage;
        //            invoiceModel.SurchargeTableRangeStart = surchargeTableRecord.PriceRangeStartValue;
        //            invoiceModel.SurchargeTableRangeEnd = surchargeTableRecord.PriceRangeEndValue;
        //        }
        //    }
        //}

        private async Task<ManualInvoiceCreateRequestViewModel> GetCreateManualInvoiceFromDraftDdtViewModelAsync(ManualInvoiceViewModel manualInvoiceModel, InvoiceModel invoiceModel)
        {
            var viewModel = new ManualInvoiceCreateRequestViewModel();
            try
            {
                viewModel = manualInvoiceModel.ToManualInvoiceViewModel();

                var order = await Context.DataContext.Orders.Where(t => t.Id == manualInvoiceModel.OrderId)
                            .Select(t => new
                            {
                                t.PoNumber,
                                t.DefaultInvoiceType,
                                t.OrderTaxDetails,
                                t.BuyerCompanyId,
                                t.AcceptedCompanyId,
                                t.ExternalBrokerId,
                                t.ExternalBrokerOrderDetail,
                                TerminalId = t.TerminalId,
                                t.CityGroupTerminalId,
                                t.BrokeredMaxQuantity,
                                t.AcceptedBy,
                                SupplierTaxExemptLicence = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                t.IsEndSupplier,
                                IsBuySellOrder = t.ExternalBrokerBuySellDetail != null,
                                t.ExternalBrokerBuySellDetail,
                                TerminalAddress = t.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                || t.TerminalId == null ? null : new AddressViewModel
                                {
                                    Address = t.MstExternalTerminal.Address,
                                    City = t.MstExternalTerminal.City,
                                    StateCode = t.MstExternalTerminal.StateCode,
                                    CountryCode = t.MstExternalTerminal.CountryCode,
                                    ZipCode = t.MstExternalTerminal.ZipCode,
                                    CountyName = t.MstExternalTerminal.CountyName
                                },
                                TerminalStateId = t.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                || t.TerminalId == null ? 0 : t.MstExternalTerminal.StateId,
                                Job = new
                                {
                                    JobAddress = new AddressViewModel
                                    {
                                        Address = t.FuelRequest.Job.Address,
                                        City = t.FuelRequest.Job.City,
                                        StateCode = t.FuelRequest.Job.MstState.Code,
                                        CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                        ZipCode = t.FuelRequest.Job.ZipCode,
                                        CountyName = t.FuelRequest.Job.CountyName
                                    },
                                    t.FuelRequest.Job.TimeZoneName,
                                    t.FuelRequest.Job.CompanyId,
                                    t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                    t.FuelRequest.Job.Id,
                                    CompanyName = t.FuelRequest.Job.Company.Name,
                                    CountryCurrency = t.FuelRequest.Job.MstCountry.Currency,
                                    t.FuelRequest.Job.JobBudget.IsAssetTracked,
                                    BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                    ApprovalUser = t.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t1 => t1.IsActive),
                                    t.FuelRequest.Job.JobBudget.IsTaxExempted
                                },
                                FuelRequest = new
                                {
                                    t.FuelRequest.Id,
                                    t.FuelRequest.JobId,
                                    t.FuelRequest.UoM,
                                    t.FuelRequest.Currency,
                                    t.FuelRequest.FuelTypeId,
                                    t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                    t.FuelRequest.MstProduct.ProductCode,
                                    t.FuelRequest.PricingTypeId,
                                    t.FuelRequest.FuelDescription,
                                    t.FuelRequest.MaxQuantity,
                                    t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                    t.FuelRequest.FuelRequestDetail.StartDate,
                                    t.FuelRequest.FuelRequestDetail.EndTime,
                                    t.FuelRequest.FuelRequestDetail.IsBolImageRequired,
                                    t.FuelRequest.FuelRequestDetail.IsDropImageRequired,
                                    t.FuelRequest.Job.SignatureEnabled,
                                    t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId
                                },
                                OriginalInvoice = new
                                {
                                    BolDetail = t.Invoices.FirstOrDefault(t1 => t1.Id == manualInvoiceModel.InvoiceId).InvoiceXBolDetails.FirstOrDefault(),
                                    SupplierPreferredInvoiceTypeId = t.Invoices.FirstOrDefault(t1 => t1.Id == manualInvoiceModel.InvoiceId).SupplierPreferredInvoiceTypeId
                                },
                                t.FuelRequest.FuelRequestPricingDetail,
                                t.IsFTL,
                                t.MstExternalTerminal,
                                PickupLocation = t.FuelDispatchLocations.FirstOrDefault(t1 => t1.LocationType == (int)LocationType.PickUp && t1.TrackableScheduleId == manualInvoiceModel.TrackableScheduleId && t1.IsActive),
                            }).FirstOrDefaultAsync();
                if (order != null)
                {
                    var timeZoneName = order.Job.TimeZoneName;
                    viewModel.UoM = order.FuelRequest.UoM;
                    viewModel.Currency = order.FuelRequest.Currency;
                    viewModel.PoNumber = order.PoNumber;
                    viewModel.TimeZoneName = timeZoneName;
                    viewModel.FuelTypeId = order.FuelRequest.FuelTypeId;
                    viewModel.TypeOfFuel = order.FuelRequest.ProductDisplayGroupId;
                    viewModel.FuelProductCode = order.FuelRequest.ProductCode;
                    if (viewModel.TypeOfFuel == (int)ProductDisplayGroups.OtherFuelType)
                    {
                        viewModel.OtherProductTaxes = order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketManual ? order.OrderTaxDetails
                            .Where(t => t.IsActive).Select(t => new TaxViewModel
                            {
                                TaxAmount = t.TaxRate,
                                TaxPricingTypeId = t.TaxPricingTypeId,
                                TaxDescription = t.TaxDescription
                            }).ToList() : manualInvoiceModel.Taxes;
                    }

                    viewModel.JobCompanyName = order.Job.CompanyName;
                    viewModel.IsBuySellOrder = order.IsBuySellOrder;
                    if (viewModel.IsBuySellOrder)
                    {
                        viewModel.BrokerMarkUp = order.ExternalBrokerBuySellDetail.BrokerMarkUp;
                        viewModel.SupplierMarkUp = order.ExternalBrokerBuySellDetail.SupplierMarkUp;
                    }
                    viewModel.CountryCurrency = order.Job.CountryCurrency;
                    viewModel.BuyerCompanyId = order.BuyerCompanyId;
                    viewModel.JobId = order.FuelRequest.JobId;
                    viewModel.JobCompanyId = order.Job.CompanyId;
                    viewModel.IsAssetTracked = order.Job.IsAssetTracked;
                    viewModel.PricingTypeId = order.FuelRequest.PricingTypeId;
                    viewModel.DeliveryTypeId = order.FuelRequest.DeliveryTypeId;
                    viewModel.MaxQuantity = order.FuelRequest.MaxQuantity;
                    viewModel.BrokeredMaxQuantity = order.BrokeredMaxQuantity;
                    viewModel.AcceptedCompanyId = order.AcceptedCompanyId;
                    viewModel.DeliveryStartDate = order.FuelRequest.StartDate;
                    viewModel.DeliveryEndTime = order.FuelRequest.EndTime;
                    viewModel.IsSalesTaxExempted = order.Job.IsTaxExempted;
                    viewModel.SupplierPreferredInvoiceTypeId = order.OriginalInvoice?.SupplierPreferredInvoiceTypeId ?? order.DefaultInvoiceType;
                    viewModel.InvoiceTypeId = order.OriginalInvoice?.SupplierPreferredInvoiceTypeId ?? manualInvoiceModel.InvoiceTypeId;
                    viewModel.OrderAcceptedBy = order.AcceptedBy;

                    if (order.FuelRequestPricingDetail != null)
                    {
                        // get pricing details from pricing service
                        PricingRequestDetailResponseViewModel pricingDetails = new PricingRequestDetailResponseViewModel();  // need to check is this needed ?
                        if (order.FuelRequestPricingDetail.RequestPriceDetailId > 0)
                        {
                            var orderDomain = new OrderDomain(this);
                            pricingDetails = await orderDomain.GetRequestPricingDetail(order.FuelRequestPricingDetail.RequestPriceDetailId, (int)viewModel.Currency, order.AcceptedCompanyId, order.FuelRequest.FuelTypeId, viewModel.JobStateId);
                            if (pricingDetails != null)
                            {
                                viewModel.PricingTypeId = pricingDetails.PricingTypeId;
                                viewModel.RackAvgTypeId = pricingDetails.RackAvgTypeId;
                                viewModel.PricePerGallon = pricingDetails.PricePerGallon;
                                viewModel.SupplierCost = pricingDetails.SupplierCost;
                            }
                        }

                        viewModel.FuelRequestPricingDetail = order.FuelRequestPricingDetail.ToViewModel(viewModel.FuelRequestPricingDetail);
                        viewModel.FuelRequestPricingDetail.PricingQuantityIndicatorTypeId = order.FuelRequest.PricingQuantityIndicatorTypeId;
                        if (viewModel.BolDetails != null && viewModel.FuelRequestPricingDetail != null)
                        {
                            viewModel.QuantityIndicatorTypeId = viewModel.FuelRequestPricingDetail.PricingQuantityIndicatorTypeId ?? 0;
                            //if FTL order then assign DroppedGallons to Net/Gross
                            if (order.IsFTL)
                            {
                                if (string.IsNullOrEmpty(viewModel.AdditionalDetail?.SplitLoadChainId))
                                {
                                    if (manualInvoiceModel.PickUpAddress != null && manualInvoiceModel.PickUpAddress.IsAddressAvailable)
                                    {
                                        viewModel.FuelDropped = viewModel.BolDetails.LiftQuantity;
                                    }
                                    else
                                    {
                                        viewModel.FuelDropped = viewModel.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net ? viewModel.BolDetails.NetQuantity : viewModel.BolDetails.GrossQuantity;
                                    }
                                }
                            }
                            else
                            {
                                viewModel.BolDetails = order.OriginalInvoice.BolDetail.InvoiceFtlDetail.ToViewModel();
                            }
                        }
                        else
                        {
                            viewModel.BolDetails = new BolDetailViewModel();
                            viewModel.BolDetails.TerminalId = order.TerminalId;
                            viewModel.BolDetails.TerminalName = order.MstExternalTerminal?.Name;
                            viewModel.BolDetails.CityGroupTerminalId = order.CityGroupTerminalId;
                            viewModel.BolDetails.IsDeleted = false;
                            viewModel.BolDetails.IsActive = true;
                            viewModel.BolDetails.FuelTypeId = order.FuelRequest.FuelTypeId;
                            if (order.PickupLocation != null)
                            {
                                viewModel.BolDetails.Address = order.PickupLocation.Address;
                                viewModel.BolDetails.City = order.PickupLocation.City;
                                viewModel.BolDetails.CountryCode = order.PickupLocation.CountryCode;
                                viewModel.BolDetails.CountyName = order.PickupLocation.CountyName;
                                viewModel.BolDetails.Latitude = order.PickupLocation.Latitude;
                                viewModel.BolDetails.Longitude = order.PickupLocation.Longitude;
                                viewModel.BolDetails.SiteName = order.PickupLocation.SiteName;
                                viewModel.BolDetails.StateCode = order.PickupLocation.StateCode;
                                viewModel.BolDetails.StateId = order.PickupLocation.StateId ?? 0;
                                viewModel.BolDetails.ZipCode = order.PickupLocation.ZipCode;
                            }
                            else if (order.PickupLocation == null && order.MstExternalTerminal != null)
                            {
                                var terminal = order.MstExternalTerminal;
                                viewModel.BolDetails.Address = terminal.Address;
                                viewModel.BolDetails.City = terminal.City;
                                viewModel.BolDetails.CountryCode = terminal.CountryCode;
                                viewModel.BolDetails.CountyName = terminal.CountyName;
                                viewModel.BolDetails.Latitude = terminal.Latitude;
                                viewModel.BolDetails.Longitude = terminal.Longitude;
                                viewModel.BolDetails.StateCode = terminal.StateCode;
                                viewModel.BolDetails.StateId = terminal.StateId;
                                viewModel.BolDetails.ZipCode = terminal.ZipCode;
                            }
                        }
                    }
                    if (order.Job.ApprovalUser != null)
                    {
                        viewModel.ApprovalUserId = order.Job.ApprovalUser.Id;
                        viewModel.ApprovalUserOnboardedType = order.Job.ApprovalUser.User.OnboardedTypeId;
                        viewModel.ApprovalUserName = $"{order.Job.ApprovalUser.User.FirstName} {order.Job.ApprovalUser.User.LastName}";
                    }
                    viewModel.IsApprovalWorkflowEnabledForJob = order.Job.IsApprovalWorkflowEnabled;
                    SetImageFlags(viewModel, order.FuelRequest.IsBolImageRequired, order.FuelRequest.IsDropImageRequired, order.FuelRequest.SignatureEnabled);
                    viewModel.ProductDescription = order.FuelRequest.FuelDescription;
                    viewModel.ExternalBrokerId = order.ExternalBrokerId ?? 0;
                    viewModel.IsThirdPartyHardwareUsed = order.ExternalBrokerOrderDetail != null;
                    if (order.ExternalBrokerOrderDetail != null)
                    {
                        viewModel.ExternalBrokeredOrder = order.ExternalBrokerOrderDetail.ToViewModel();
                    }
                    SetTaxExemptLicences(viewModel, order);
                    viewModel.TerminalId = order.TerminalId;
                    viewModel.CityGroupTerminalId = order.CityGroupTerminalId;
                    viewModel.IsBrokeredChainOrder = order.BuyerCompanyId != order.Job.CompanyId;
                    viewModel.JobAddess = order.Job.JobAddress;
                    viewModel.TerminalAddress = order.TerminalAddress;
                    viewModel.TerminalStateId = order.TerminalStateId;
                    SetManualInputToInvoiceModel(viewModel, invoiceModel);
                    var assetCount = manualInvoiceModel.Assets.Select(t => t.AssetName.ToLower()).Distinct().Count();
                    CheckRequiredImagesAndSetWaitingForImageAction(invoiceModel);

                    await GetDraftDdtInvoiceViewModel(viewModel, invoiceModel, assetCount);

                    invoiceModel.InvoiceNumberId = manualInvoiceModel.InvoiceNumber.Id;
                    invoiceModel.DisplayInvoiceNumber = manualInvoiceModel.DisplayInvoiceNumber;

                    viewModel.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetCreateManualInvoiceFromDraftDdtViewModelAsync", ex.Message, ex);
            }
            return viewModel;
        }

        private async Task<ManualInvoiceCreateRequestViewModel> GetBrokerInvoiceRequestViewModelAsync(ManualInvoiceViewModel manualInvoiceModel)
        {
            var viewModel = new ManualInvoiceCreateRequestViewModel();
            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == manualInvoiceModel.OrderId)
                            .Select(t => new
                            {
                                t.PoNumber,
                                t.DefaultInvoiceType,
                                t.OrderTaxDetails,
                                t.BuyerCompanyId,
                                t.AcceptedCompanyId,
                                t.ExternalBrokerId,
                                t.ExternalBrokerOrderDetail,
                                t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive).PaymentTermId,
                                t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive).NetDays,
                                t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive).PaymentMethod,
                                TerminalId = t.TerminalId,
                                t.CityGroupTerminalId,
                                t.BrokeredMaxQuantity,
                                t.AcceptedBy,
                                t.IsFTL,
                                SupplierTaxExemptLicence = t.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                t.IsEndSupplier,
                                SupplierAllowance = t.OrderAdditionalDetail != null && t.OrderAdditionalDetail.Allowance.HasValue ? t.OrderAdditionalDetail.Allowance.Value : 0,
                                IsBuySellOrder = t.ExternalBrokerBuySellDetail != null,
                                BuyerCompanyName = t.BuyerCompany.Name,
                                OrderAdditionalDetails = t.OrderAdditionalDetail == null ? null : new
                                {
                                    t.OrderAdditionalDetail.IsFuelSurcharge,
                                    t.OrderAdditionalDetail.FuelSurchagePricingType
                                },
                                TerminalAddress = t.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                || t.TerminalId == null ? null : new AddressViewModel
                                {
                                    Address = t.MstExternalTerminal.Address,
                                    City = t.MstExternalTerminal.City,
                                    StateCode = t.MstExternalTerminal.StateCode,
                                    CountryCode = t.MstExternalTerminal.CountryCode,
                                    ZipCode = t.MstExternalTerminal.ZipCode,
                                    CountyName = t.MstExternalTerminal.CountyName
                                },
                                TerminalStateId = t.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType
                                || t.TerminalId == null ? 0 : t.MstExternalTerminal.StateId,
                                Job = new
                                {
                                    t.FuelRequest.Job.Name,
                                    JobAddress = new AddressViewModel
                                    {
                                        Address = t.FuelRequest.Job.Address,
                                        City = t.FuelRequest.Job.City,
                                        StateCode = t.FuelRequest.Job.MstState.Code,
                                        CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                        ZipCode = t.FuelRequest.Job.ZipCode,
                                        CountyName = t.FuelRequest.Job.CountyName
                                    },
                                    t.FuelRequest.Job.TimeZoneName,
                                    t.FuelRequest.Job.IsApprovalWorkflowEnabled,
                                    t.FuelRequest.Job.Id,
                                    t.FuelRequest.Job.CompanyId,
                                    CompanyName = t.FuelRequest.Job.Company.Name,
                                    CountryCurrency = t.FuelRequest.Job.MstCountry.Currency,
                                    t.FuelRequest.Job.JobBudget.IsAssetTracked,
                                    BuyerTaxExemptLicence = t.FuelRequest.Job.Company.TaxExemptLicenses.FirstOrDefault(t1 => t1.IsActive),
                                    ApprovalUser = t.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t1 => t1.IsActive),
                                    t.FuelRequest.Job.JobBudget.IsTaxExempted
                                },
                                FuelRequest = new
                                {
                                    t.FuelRequest.Id,
                                    t.FuelRequest.JobId,
                                    t.FuelRequest.UoM,
                                    t.FuelRequest.Currency,
                                    t.FuelRequest.FuelTypeId,
                                    t.FuelRequest.MstProduct.ProductDisplayGroupId,
                                    t.FuelRequest.MstProduct.ProductCode,
                                    t.FuelRequest.MstProduct.ProductTypeId,
                                    t.FuelRequest.PricingTypeId,
                                    t.FuelRequest.FuelDescription,
                                    t.FuelRequest.MaxQuantity,
                                    t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                                    t.FuelRequest.FuelRequestDetail.StartDate,
                                    t.FuelRequest.FuelRequestDetail.EndTime,
                                    t.FuelRequest.FuelRequestDetail.IsBolImageRequired,
                                    t.FuelRequest.FuelRequestDetail.IsDropImageRequired,
                                    t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId,
                                    t.FuelRequest.Job.SignatureEnabled,
                                    t.FuelRequest.FuelRequestFees
                                },
                                t.FuelRequest.FuelRequestPricingDetail
                            }).FirstOrDefaultAsync();
                if (order != null)
                {
                    var timeZoneName = order.Job.TimeZoneName;
                    viewModel.OrderId = manualInvoiceModel.OrderId;
                    viewModel.DropStartDate = new DateTimeOffset(manualInvoiceModel.DeliveryDate.Add(Convert.ToDateTime(manualInvoiceModel.StartTime).TimeOfDay));
                    viewModel.DropEndDate = new DateTimeOffset(manualInvoiceModel.DeliveryDate.Add(Convert.ToDateTime(manualInvoiceModel.EndTime).TimeOfDay));
                    viewModel.FuelDropped = manualInvoiceModel.FuelDropped ?? 0.0M;
                    if (manualInvoiceModel.SignatureImage != null && !string.IsNullOrWhiteSpace(manualInvoiceModel.SignatureImage?.FilePath))
                        viewModel.Signature = manualInvoiceModel.SignatureImage?.ToCustomerSignature();

                    viewModel.InvoiceImage = manualInvoiceModel.InvoiceImage;
                    viewModel.BolImage = manualInvoiceModel.BolImage;
                    viewModel.AdditionalImage = manualInvoiceModel.AdditionalImage;
                    viewModel.TaxAffidavitImage = manualInvoiceModel.TaxAffidavitImage;
                    viewModel.BDNImage = manualInvoiceModel.BDNImage;
                    viewModel.CoastGuardInspectionImage = manualInvoiceModel.CoastGuardInspectionImage;
                    viewModel.InspectionRequestVoucherImage = manualInvoiceModel.InspectionRequestVoucherImage;
                    viewModel.UoM = order.FuelRequest.UoM;
                    viewModel.Currency = order.FuelRequest.Currency;
                    viewModel.PoNumber = order.PoNumber;
                    viewModel.UserId = order.AcceptedBy;
                    viewModel.TimeZoneName = timeZoneName;
                    viewModel.FuelTypeId = order.FuelRequest.FuelTypeId;
                    viewModel.TypeOfFuel = order.FuelRequest.ProductDisplayGroupId;
                    viewModel.FuelProductCode = order.FuelRequest.ProductCode;
                    if (viewModel.TypeOfFuel == (int)ProductDisplayGroups.OtherFuelType)
                    {
                        viewModel.OtherProductTaxes = order.OrderTaxDetails
                            .Where(t => t.IsActive).Select(t => new TaxViewModel
                            {
                                TaxAmount = t.TaxRate,
                                TaxPricingTypeId = t.TaxPricingTypeId,
                                TaxDescription = t.TaxDescription
                            }).ToList();
                    }

                    viewModel.JobCompanyName = order.Job.CompanyName;
                    viewModel.IsBuySellOrder = order.IsBuySellOrder;
                    viewModel.CountryCurrency = order.Job.CountryCurrency;
                    viewModel.BuyerCompanyId = order.BuyerCompanyId;
                    viewModel.BuyerCompanyName = order.BuyerCompanyName;
                    viewModel.JobId = order.FuelRequest.JobId;
                    viewModel.JobName = order.Job.Name;
                    viewModel.JobCompanyId = order.Job.CompanyId;
                    viewModel.IsAssetTracked = order.Job.IsAssetTracked;
                    viewModel.PricingTypeId = order.FuelRequest.PricingTypeId;
                    viewModel.DeliveryTypeId = order.FuelRequest.DeliveryTypeId;
                    viewModel.MaxQuantity = order.FuelRequest.MaxQuantity;
                    viewModel.BrokeredMaxQuantity = order.BrokeredMaxQuantity;
                    viewModel.AcceptedCompanyId = order.AcceptedCompanyId;
                    viewModel.DeliveryStartDate = order.FuelRequest.StartDate;
                    viewModel.DeliveryEndTime = order.FuelRequest.EndTime;
                    viewModel.NetDays = order.NetDays;
                    viewModel.PaymentTermId = order.PaymentTermId;
                    viewModel.InvoiceTypeId = order.DefaultInvoiceType;
                    viewModel.IsSalesTaxExempted = order.Job.IsTaxExempted;

                    viewModel.DropLocation = manualInvoiceModel.ToDropLocation();
                    if (manualInvoiceModel.PickUpAddress != null && manualInvoiceModel.PickUpAddress.IsAddressAvailable)
                    {
                        viewModel.PickupLocation = manualInvoiceModel.ToPickUpLocation();
                    }
                    viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = order.FuelRequest.FuelRequestFees.ToFeesViewModel();
                    UpdateWaitingTimeInFeeDetails(viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees, manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees);
                    viewModel.IsWetHosingDelivery = viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.WetHoseFee).ToString() && t.DiscountLineItemId == null);
                    viewModel.IsOverWaterDelivery = viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.OverWaterFee).ToString() && t.DiscountLineItemId == null);
                    viewModel.TrackableScheduleId = manualInvoiceModel.TrackableScheduleId;
                    viewModel.IsFTL = order.IsFTL;

                    viewModel.OrderAcceptedBy = order.AcceptedBy;
                    if (order.Job.ApprovalUser != null)
                    {
                        viewModel.ApprovalUserId = order.Job.ApprovalUser.Id;
                        viewModel.ApprovalUserOnboardedType = order.Job.ApprovalUser.User.OnboardedTypeId;
                        viewModel.ApprovalUserName = $"{order.Job.ApprovalUser.User.FirstName} {order.Job.ApprovalUser.User.LastName}";
                    }

                    if (order.OrderAdditionalDetails != null && order.OrderAdditionalDetails.IsFuelSurcharge && order.OrderAdditionalDetails.FuelSurchagePricingType.HasValue)
                    {
                        viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee = new FuelSurchargeFreightFeeViewModel();
                        var surchareFee = order.FuelRequest.FuelRequestFees.Where(t => t.FeeTypeId.Equals(((int)FeeType.SurchargeFreightFee))).FirstOrDefault();
                        if (surchareFee != null)
                        {
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsSurchargeApplicable = order.OrderAdditionalDetails.IsFuelSurcharge;
                            var fscproductType = order.FuelRequest.ProductTypeId.GetFuelSurchargeProductType();
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeProductType = fscproductType;
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeFreightCost = surchareFee.Fee;
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargePricingType = (FuelSurchagePricingType)order.OrderAdditionalDetails.FuelSurchagePricingType.Value;
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeEiaPrice = new EIAPriceUpdateDomain().GetEIAPrice((FuelSurchagePricingType)order.OrderAdditionalDetails.FuelSurchagePricingType.Value, fscproductType, viewModel.DropEndDate.Date);
                            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.GallonsDelivered = viewModel.FuelDropped ?? 0;

                            if (surchareFee.FeeSubTypeId == (int)FeeSubType.ByDistance)
                            {
                                var distanceFromOriginalDrop = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.Distance;
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsFeeByDistance = true;
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.Distance = distanceFromOriginalDrop;

                                var byQantity = surchareFee.FeeByQuantities.FirstOrDefault(t => distanceFromOriginalDrop >= t.MinQuantity && distanceFromOriginalDrop <= (t.MaxQuantity ?? distanceFromOriginalDrop));
                                if (byQantity != null)
                                    viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeFreightCost = byQantity.Fee;
                            }

                            var surchargeTableRecord = new FuelSurchargeDomain().GetFuelSurchargeRecordForEaiPrice(viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeEiaPrice, order.AcceptedCompanyId, order.BuyerCompanyId, viewModel.DropEndDate, fscproductType);
                            if (surchargeTableRecord != null)
                            {
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargePercentage = surchargeTableRecord.FuelSurchargeStartPercentage;
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeTableRangeStart = surchargeTableRecord.PriceRangeStartValue;
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeTableRangeEnd = surchargeTableRecord.PriceRangeEndValue;
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.TotalFuelSurchargeFee = GetFuelSurchageFrieghtFee(surchargeTableRecord.FuelSurchargeStartPercentage, viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeFreightCost, viewModel.FuelDropped);
                                //(surchargeTableRecord.FuelSurchargeStartPercentage / 100) * viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.SurchargeFreightCost * viewModel.FuelDropped ?? 0;
                            }
                        }
                    }

                    SetTaxExemptLicences(viewModel, order);
                    viewModel.IsApprovalWorkflowEnabledForJob = order.Job.IsApprovalWorkflowEnabled;
                    SetImageFlags(viewModel, order.FuelRequest.IsBolImageRequired, order.FuelRequest.IsDropImageRequired, order.FuelRequest.SignatureEnabled);
                    viewModel.ProductDescription = order.FuelRequest.FuelDescription;
                    viewModel.ExternalBrokerId = order.ExternalBrokerId ?? 0;
                    viewModel.IsThirdPartyHardwareUsed = order.ExternalBrokerOrderDetail != null;
                    if (order.ExternalBrokerOrderDetail != null)
                    {
                        viewModel.ExternalBrokeredOrder = order.ExternalBrokerOrderDetail.ToViewModel(viewModel.ExternalBrokeredOrder);
                    }

                    viewModel.TerminalId = order.TerminalId;
                    viewModel.CityGroupTerminalId = order.CityGroupTerminalId;
                    viewModel.IsBrokeredChainOrder = order.BuyerCompanyId != order.Job.CompanyId;
                    viewModel.JobAddess = order.Job.JobAddress;
                    viewModel.TerminalAddress = order.TerminalAddress;
                    viewModel.TerminalStateId = order.TerminalStateId;
                    viewModel.BolDetails = manualInvoiceModel.BolDetails.Clone(viewModel.UserId);
                    viewModel.BolDetails.Id = 0;
                    viewModel.SupplierPreferredInvoiceTypeId = order.DefaultInvoiceType;
                    if (viewModel.AdditionalDetail == null)
                    {
                        viewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                        viewModel.AdditionalDetail.SupplierAllowance = order.SupplierAllowance;
                        viewModel.AdditionalDetail.ActualDropQuantity = viewModel.FuelDropped;
                    }
                    viewModel.AdditionalDetail.SplitLoadChainId = manualInvoiceModel.SplitLoadChainId;
                    viewModel.AdditionalDetail.SplitLoadSequence = manualInvoiceModel.SplitLoadSequence;
                    viewModel.AdditionalDetail.Notes = manualInvoiceModel.Notes;
                    viewModel.AdditionalDetail.PaymentMethod = order.PaymentMethod;
                    viewModel.CreationMethod = manualInvoiceModel.CreationMethod;

                    if (order.FuelRequestPricingDetail != null)
                    {
                        // get pricing details from pricing service
                        PricingRequestDetailResponseViewModel pricingDetails = new PricingRequestDetailResponseViewModel();  // need to check is this needed ?
                        if (order.FuelRequestPricingDetail.RequestPriceDetailId > 0)
                        {
                            var orderDomain = new OrderDomain(this);
                            pricingDetails = await orderDomain.GetRequestPricingDetail(order.FuelRequestPricingDetail.RequestPriceDetailId, (int)viewModel.Currency, order.AcceptedCompanyId, order.FuelRequest.FuelTypeId, viewModel.JobStateId);
                            if (pricingDetails != null)
                            {
                                viewModel.PricingTypeId = pricingDetails.PricingTypeId;
                                viewModel.RackAvgTypeId = pricingDetails.RackAvgTypeId;
                                viewModel.PricePerGallon = pricingDetails.PricePerGallon;
                                viewModel.SupplierCost = pricingDetails.SupplierCost;
                            }
                        }

                        viewModel.FuelRequestPricingDetail = order.FuelRequestPricingDetail.ToViewModel(viewModel.FuelRequestPricingDetail);
                        viewModel.FuelRequestPricingDetail.PricingQuantityIndicatorTypeId = order.FuelRequest.PricingQuantityIndicatorTypeId;
                        if (viewModel.BolDetails != null && viewModel.FuelRequestPricingDetail != null)
                        {
                            viewModel.QuantityIndicatorTypeId = viewModel.FuelRequestPricingDetail.PricingQuantityIndicatorTypeId ?? 0;
                            //if FTL order then assign DroppedGallons to Net/Gross
                            if (order.IsFTL)
                            {
                                if (string.IsNullOrEmpty(viewModel.AdditionalDetail?.SplitLoadChainId))
                                {
                                    if (manualInvoiceModel.PickUpAddress != null && manualInvoiceModel.PickUpAddress.IsAddressAvailable)
                                    {
                                        viewModel.FuelDropped = viewModel.BolDetails.LiftQuantity;
                                    }
                                    else
                                    {
                                        viewModel.FuelDropped = viewModel.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net ? viewModel.BolDetails.NetQuantity : viewModel.BolDetails.GrossQuantity;
                                    }
                                }
                            }
                        }
                    }

                    //// set updated terminal details on creating invoice
                    if (manualInvoiceModel.BolDetails.TerminalId != 0 && manualInvoiceModel.BolDetails.TerminalId != order.TerminalId)
                    {
                        GetTerminalDetailsById(viewModel, manualInvoiceModel);
                    }

                    var terminalId = 0;
                    if (manualInvoiceModel.BolDetails.TerminalId.HasValue)
                        terminalId = manualInvoiceModel.BolDetails.TerminalId.Value;
                    else
                        terminalId = order.TerminalId.HasValue ? order.TerminalId.Value : 0;

                    var terminal = Context.DataContext.MstExternalTerminals.FirstOrDefault(t => t.Id == terminalId);

                    if (viewModel.BolDetails != null)
                    {
                        viewModel.BolDetails.TerminalId = terminalId;
                        viewModel.BolDetails.TerminalName = terminal != null ? terminal.Name : string.Empty;
                        viewModel.BolDetails.CityGroupTerminalId = order.CityGroupTerminalId;
                        viewModel.BolDetails.IsDeleted = false;
                        viewModel.BolDetails.IsActive = true;
                        viewModel.BolDetails.FuelTypeId = order.FuelRequest.FuelTypeId;
                        viewModel.BolDetails.CreatedBy = manualInvoiceModel.userId;
                        viewModel.BolDetails.CreatedDate = manualInvoiceModel.CreatedDate;
                        viewModel.BolDetails.PricePerGallon = viewModel.PricePerGallon;
                        viewModel.BolDetails.PickupLocationType = !string.IsNullOrEmpty(viewModel.BolDetails.LiftTicketNumber) ? PickupLocationType.BulkPlant : PickupLocationType.Terminal;

                        if (manualInvoiceModel.PickUpAddress != null && manualInvoiceModel.PickUpAddress.IsAddressAvailable)
                        {
                            //If Pickup location is supplied at Order
                            viewModel.BolDetails.Address = manualInvoiceModel.PickUpAddress.Address;
                            viewModel.BolDetails.City = manualInvoiceModel.PickUpAddress.City;
                            viewModel.BolDetails.CountryCode = manualInvoiceModel.PickUpAddress.Country.Code;
                            viewModel.BolDetails.CountyName = manualInvoiceModel.PickUpAddress.CountyName;
                            viewModel.BolDetails.Latitude = manualInvoiceModel.PickUpAddress.Latitude;
                            viewModel.BolDetails.Longitude = manualInvoiceModel.PickUpAddress.Longitude;
                            viewModel.BolDetails.SiteName = manualInvoiceModel.PickUpAddress.SiteName;
                            viewModel.BolDetails.StateCode = manualInvoiceModel.PickUpAddress.State.Code;
                            viewModel.BolDetails.StateId = manualInvoiceModel.PickUpAddress.State.Id;
                            viewModel.BolDetails.ZipCode = manualInvoiceModel.PickUpAddress.ZipCode;
                        }
                        else
                        {
                            //If Lift Details are not given in Csv then use Terminal
                            if (terminal != null)
                            {
                                viewModel.BolDetails.Address = terminal.Address;
                                viewModel.BolDetails.City = terminal.City;
                                viewModel.BolDetails.CountryCode = terminal.CountryCode;
                                viewModel.BolDetails.CountyName = terminal.CountyName;
                                viewModel.BolDetails.Latitude = terminal.Latitude;
                                viewModel.BolDetails.Longitude = terminal.Longitude;
                                viewModel.BolDetails.StateCode = terminal.StateCode;
                                viewModel.BolDetails.StateId = terminal.StateId;
                                viewModel.BolDetails.ZipCode = terminal.ZipCode;
                            }
                        }
                    }

                    viewModel.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetBrokerInvoiceRequestViewModelAsync", ex.Message, ex);
            }
            return viewModel;
        }

        private void SetImageFlags(ManualInvoiceCreateRequestViewModel viewModel, bool isBolImageRequired, bool isDropImageRequired, bool signatureEnabled)
        {
            viewModel.IsBOLImageReq = isBolImageRequired;
            viewModel.IsDropImageReq = isDropImageRequired;
            viewModel.IsSignatureReq = signatureEnabled;
        }

        private async Task<List<AssetDrop>> GetIncompleteMobileAssetDropsAsync(bool isAssetTracked, int InvoiceTypeId, int OrderId, int DriverId)
        {
            var assetDrops = new List<AssetDrop>();
            if (isAssetTracked && (InvoiceTypeId == (int)InvoiceType.MobileApp || InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
            {
                assetDrops = await Context.DataContext.AssetDrops.Where(t => t.OrderId == OrderId
                                    && t.InvoiceId == null && t.DroppedBy == DriverId).ToListAsync();
            }
            return assetDrops;
        }

        private static int GetMobileInvoiceType(DriverDropOrderViewModel DriverDropViewModel)
        {
            var invoiceType = (int)InvoiceType.MobileApp;
            if (DriverDropViewModel.InvoiceStatusId == (int)InvoiceStatus.Draft)
            {
                // Discontinue delivery from mobile driver app
                invoiceType = (int)InvoiceType.DigitalDropTicketMobileApp;
            }
            return invoiceType;
        }

        private void CorrectMobileDropStartDateIfNotValid(DriverDropOrderViewModel viewModel)
        {
            var firstAssetDrop = Context.DataContext.AssetDrops.Where(t => t.OrderId == viewModel.OrderId
                                 && t.DroppedBy == viewModel.Driver.UserId && t.InvoiceId == null)
                                  .OrderBy(t => t.DropStartDate).FirstOrDefault();
            if (firstAssetDrop != null && viewModel.DropStartDate > firstAssetDrop.DropStartDate)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "CreateMobileInvoice", $"Invoice Start:{viewModel.DropStartDate}  First Asset Drop Start:{firstAssetDrop.DropStartDate}", new InvalidOperationException("Invalid invoice drop start date"));
                viewModel.DropStartDate = firstAssetDrop.DropStartDate.AddSeconds(-30);
            }
        }

        private async Task<List<InvoiceXSpecialInstructionViewModel>> GetSpecialInstructions(DriverDropOrderViewModel viewModel)
        {
            var specialInstructions = new List<InvoiceXSpecialInstructionViewModel>();
            if (viewModel.SpecialInstructions != null && viewModel.SpecialInstructions.Any())
            {
                var FrInstructions = await Context.DataContext.FuelRequests.Where(t => t.Id == viewModel.FuelId).SelectMany(t => t.SpecialInstructions).ToListAsync();
                foreach (var item in viewModel.SpecialInstructions)
                {
                    var FrInstruction = FrInstructions.FirstOrDefault(t => t.Instruction == item.Key);
                    if (FrInstruction != null)
                    {
                        var instruction = new InvoiceXSpecialInstructionViewModel
                        {
                            SpecialInstructionId = FrInstruction.Id,
                            IsInstructionFollowed = item.Value
                        };
                        specialInstructions.Add(instruction);
                    }
                }
            }
            return specialInstructions;
        }

        private void SetMobileInputsToInvoiceModel(MobileInvoiceCreateRequestViewModel manualInvoiceModel, InvoiceModel invoiceModel)
        {
            invoiceModel.DropStartDate = manualInvoiceModel.DropStartDate;
            invoiceModel.DropEndDate = manualInvoiceModel.DropEndDate;

            invoiceModel.DroppedGallons = manualInvoiceModel.FuelDropped ?? 0.0M;
            invoiceModel.IsWetHosingDelivery = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.WetHoseFee).ToString() && t.DiscountLineItemId == null);
            invoiceModel.IsOverWaterDelivery = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.OverWaterFee).ToString() && t.DiscountLineItemId == null);

            //viewModel.UserId = manualInvoiceModel.userId;
            invoiceModel.OrderId = manualInvoiceModel.OrderId;
            invoiceModel.PoNumber = manualInvoiceModel.PoNumber;
            if (manualInvoiceModel.InvoiceImage != null && !manualInvoiceModel.InvoiceImage.IsRemoved)
            {
                invoiceModel.Image = manualInvoiceModel.InvoiceImage;
            }
            invoiceModel.DriverId = manualInvoiceModel.DriverId;
            invoiceModel.TraceId = manualInvoiceModel.TraceId;
            invoiceModel.InvoiceTypeId = manualInvoiceModel.InvoiceTypeId;
            invoiceModel.PaymentTermId = manualInvoiceModel.PaymentTermId;
            invoiceModel.NetDays = manualInvoiceModel.PaymentTermId == (int)PaymentTerms.NetDays ? manualInvoiceModel.NetDays : 0;

            invoiceModel.FilePath = manualInvoiceModel.CsvFilePath;
            invoiceModel.SpecialInstructions = manualInvoiceModel.SpecialInstructions;
            invoiceModel.Signature = manualInvoiceModel.CustomerSignature;
            invoiceModel.BolDetails.Add(manualInvoiceModel.BolDetails);
            if (manualInvoiceModel.BolImage != null && (manualInvoiceModel.BolImage.Id > 0 || !string.IsNullOrWhiteSpace(manualInvoiceModel.BolImage?.FilePath)))
            {
                invoiceModel.BolImage = manualInvoiceModel.BolImage;
            }
            if (manualInvoiceModel.AdditionalImage != null && (manualInvoiceModel.AdditionalImage.Id > 0 || !string.IsNullOrWhiteSpace(manualInvoiceModel.AdditionalImage?.FilePath)))
            {
                invoiceModel.AdditionalImage = manualInvoiceModel.AdditionalImage;
            }

            invoiceModel.IsTerminalPickup = (manualInvoiceModel.PickupLocation != null && manualInvoiceModel.PickupLocation.PickupLocationType == PickupLocationType.BulkPlant) ? false : true;
            invoiceModel.FuelPickLocation = manualInvoiceModel.PickupLocation;
            invoiceModel.FuelDropLocation = manualInvoiceModel.DropLocation;

            if (manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee != null)
            {
                invoiceModel.SurchargeFreightFeeViewModel = new FuelSurchargeFreightFeeViewModel();
                invoiceModel.SurchargeFreightFeeViewModel = manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee;
            }
        }

        private void SetManualInputToInvoiceModel(ManualInvoiceCreateRequestViewModel manualInvoiceCreateRequestModel, InvoiceModel invoiceModel)
        {
            var offset = manualInvoiceCreateRequestModel.DropEndDate.GetOffset(manualInvoiceCreateRequestModel.TimeZoneName);
            invoiceModel.DropStartDate = manualInvoiceCreateRequestModel.DropStartDate.AttachOffset(offset);
            invoiceModel.DropEndDate = manualInvoiceCreateRequestModel.DropEndDate.AttachOffset(offset);

            invoiceModel.DroppedGallons = manualInvoiceCreateRequestModel.FuelDropped ?? 0.0M;
            invoiceModel.IsWetHosingDelivery = manualInvoiceCreateRequestModel.IsWetHosingDelivery;
            invoiceModel.IsOverWaterDelivery = manualInvoiceCreateRequestModel.IsOverWaterDelivery;

            invoiceModel.OrderId = manualInvoiceCreateRequestModel.OrderId;
            invoiceModel.PoNumber = manualInvoiceCreateRequestModel.PoNumber;
            if (manualInvoiceCreateRequestModel.InvoiceImage != null && !manualInvoiceCreateRequestModel.InvoiceImage.IsRemoved)
            {
                invoiceModel.Image = manualInvoiceCreateRequestModel.InvoiceImage;
            }
            if (manualInvoiceCreateRequestModel.BolImage != null && !manualInvoiceCreateRequestModel.BolImage.IsRemoved)
            {
                invoiceModel.BolImage = manualInvoiceCreateRequestModel.BolImage;
            }
            if (manualInvoiceCreateRequestModel.Signature != null && !manualInvoiceCreateRequestModel.Signature.Image.IsRemoved)
            {
                invoiceModel.Signature = manualInvoiceCreateRequestModel.Signature;
            }
            if (manualInvoiceCreateRequestModel.AdditionalImage != null && !manualInvoiceCreateRequestModel.AdditionalImage.IsRemoved)
            {
                invoiceModel.AdditionalImage = manualInvoiceCreateRequestModel.AdditionalImage;
            }
            invoiceModel.DriverId = manualInvoiceCreateRequestModel.DriverId;
            invoiceModel.InvoiceTypeId = manualInvoiceCreateRequestModel.InvoiceTypeId;
            invoiceModel.PaymentTermId = manualInvoiceCreateRequestModel.PaymentTermId;
            invoiceModel.NetDays = manualInvoiceCreateRequestModel.NetDays;
            invoiceModel.FilePath = manualInvoiceCreateRequestModel.CsvFilePath;
            invoiceModel.BolDetails.Add(manualInvoiceCreateRequestModel.BolDetails);

            if (manualInvoiceCreateRequestModel.IsVariousFobOrigin || !string.IsNullOrEmpty(manualInvoiceCreateRequestModel.AdditionalDetail?.SplitLoadChainId))
            {
                invoiceModel.FuelDropLocation = manualInvoiceCreateRequestModel.DropLocation;
                SetFirstZipCodeOfState(manualInvoiceCreateRequestModel.DropLocation.StateId, manualInvoiceCreateRequestModel.DropLocation.StateCode, out string stateCode);
                invoiceModel.FuelDropLocation.StateCode = stateCode;
            }

            if (invoiceModel.SurchargeFreightFeeViewModel == null) invoiceModel.SurchargeFreightFeeViewModel = new FuelSurchargeFreightFeeViewModel();
            invoiceModel.SurchargeFreightFeeViewModel = manualInvoiceCreateRequestModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee;

            invoiceModel.IsBOLImageReq = manualInvoiceCreateRequestModel.IsBOLImageReq;
            invoiceModel.IsDropImageReq = manualInvoiceCreateRequestModel.IsDropImageReq;
            invoiceModel.IsSignatureReq = manualInvoiceCreateRequestModel.IsSignatureReq;
            invoiceModel.IsTerminalPickup = manualInvoiceCreateRequestModel.IsTerminalPickup;
        }

        private int GetEndSupplierDeliveryScheduleIdForBrokerChain(int? trackableScheduleId, out DateTimeOffset scheduleDate)
        {
            // End supplier trackable schedule Id to associate trackable schedule to broker chain orders
            var deliveryScheduleId = 0; scheduleDate = DateTimeOffset.Now;
            if (trackableScheduleId.HasValue)
            {
                var deliveryScheduleXTrackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.Id == trackableScheduleId);
                if (deliveryScheduleXTrackableSchedule != null)
                {
                    deliveryScheduleId = deliveryScheduleXTrackableSchedule.DeliveryScheduleId;
                    scheduleDate = deliveryScheduleXTrackableSchedule.Date;
                }
            }

            return deliveryScheduleId;
        }

        public static string SetSplitLoadChainId(string splitLoadChainId, int createdBy)
        {
            if (string.IsNullOrWhiteSpace(splitLoadChainId))
            {
                splitLoadChainId = DateTimeOffset.Now.ToString("yyyyMMddHHmmssFFFFFF") + createdBy;
            }
            return splitLoadChainId;
        }

        private async Task SetInvoiceNumberToInvoiceModel(InvoiceModel invoiceModel)
        {
            var invoiceNumber = await GenerateInvoiceNumber();
            invoiceModel.InvoiceNumberId = invoiceNumber.Id;
            invoiceModel.DisplayInvoiceNumber = invoiceNumber.Number;
        }

        #region New invoice methods for Angular

        public async Task<InvoiceViewModelNew> GetPoDetailsToCreateInvoice(UserContext userContext, int orderId, int? trackableScheduleId = null)
        {
            var response = new InvoiceViewModelNew();
            try
            {
                string customerEmail = string.Empty;
                string customerPhone = string.Empty;
                string customerName = string.Empty;
                string BuyerCompanyName = string.Empty;

                var order = await Context.DataContext.Orders
                                    .Where(t => t.Id == orderId && (t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                    || (t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery
                                        && !t.Invoices.Any() && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed)))
                                    .Select(t => new
                                    {
                                        t.Id,
                                        IsFTL = t.IsFTL,
                                        t.PoNumber,
                                        t.DefaultInvoiceType,
                                        User = new
                                        {
                                            t.FuelRequest.User.Id,
                                            t.FuelRequest.User.Email,
                                            t.FuelRequest.User.PhoneNumber,
                                            t.FuelRequest.User.FirstName,
                                            t.FuelRequest.User.LastName,
                                            CompanyName = t.FuelRequest.User.Company.Name
                                        },
                                        FuelRequest = new
                                        {
                                            t.FuelRequest.FuelTypeId,
                                            t.FuelRequest.FuelRequestTypeId,
                                            t.FuelRequest.CounterOffers,
                                            t.FuelRequest.FuelRequestFees,
                                            FuelRequestStartDate = t.FuelRequest.FuelRequestDetail.StartDate,
                                            ProductName = t.FuelRequest.MstProduct.DisplayName ?? t.FuelRequest.MstProduct.Name,
                                            TypeOfFuel = t.FuelRequest.MstProduct.ProductTypeId,
                                            t.FuelRequest.FreightOnBoardTypeId,
                                            UoM = t.FuelRequest.UoM,
                                            Currency = t.FuelRequest.Currency
                                        },
                                        OrderAdditionalDetail = t.OrderAdditionalDetail ?? null,
                                        t.BuyerCompanyId,
                                        t.AcceptedCompanyId,
                                        Job = new
                                        {
                                            t.FuelRequest.JobId,
                                            t.FuelRequest.Job.Name,
                                            t.FuelRequest.Job.Address,
                                            t.FuelRequest.Job.City,
                                            StateCode = t.FuelRequest.Job.MstState.Code,
                                            StateId = t.FuelRequest.Job.StateId,
                                            CountryId = t.FuelRequest.Job.CountryId,
                                            CountryCode = t.FuelRequest.Job.MstCountry.Code,
                                            Statecod = t.FuelRequest.Job.MstState.Code,
                                            t.FuelRequest.Job.ZipCode,
                                            t.FuelRequest.Job.DisplayJobID,
                                            t.FuelRequest.Job.JobBudget.IsAssetTracked,
                                            t.FuelRequest.Job.LocationType,
                                            t.FuelRequest.Job.IsMarine
                                        },
                                        t.TerminalId,
                                        TerminalName = t.MstExternalTerminal.Name,
                                        Pickup = t.FuelDispatchLocations.FirstOrDefault(t1 => t1.IsActive && t1.LocationType == (int)LocationType.PickUp),
                                        OrderDetailVersion = t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive),
                                        Driver = t.OrderXDrivers.FirstOrDefault(t2 => t2.IsActive),
                                        OrderTaxDetails = t.OrderTaxDetails.Where(t1 => t1.IsActive).ToList(),
                                        IsBolImageRequired = t.FuelRequest.FuelRequestDetail.IsBolImageRequired,
                                        IsDropImageRequired = t.FuelRequest.FuelRequestDetail.IsDropImageRequired,
                                        IsDipDataRequired = t.FuelRequest.FuelRequestDetail.IsPrePostDipRequired,
                                        SignatureEnabled = t.FuelRequest.Job.SignatureEnabled ? t.FuelRequest.Job.SignatureEnabled : t.SignatureEnabled,
                                        QuantityIndicatorTypeId = t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId ?? (int)QuantityIndicatorTypes.Net,
                                        TrackableSchedule = t.DeliveryScheduleXTrackableSchedules.Where(d => trackableScheduleId.HasValue && d.Id == trackableScheduleId).FirstOrDefault(),
                                        // IsSupressOrderPricing = t.OrderAdditionalDetail.OnboardingPreference !=null? t.OrderAdditionalDetail.OnboardingPreference.IsSupressOrderPricing : false
                                        IsSupressOrderPricing=t.OrderAdditionalDetail.IsSupressPricingEnabled
                                    }).SingleOrDefaultAsync();

                if (order != null)
                {
                    var user = order.User;
                    var job = order.Job;                   
                    if (order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest)
                    {
                        var counterOffer = order.FuelRequest.CounterOffers.FirstOrDefault(t => t.BuyerStatus == (int)CounterOfferStatus.Accepted);
                        if (counterOffer != null)
                        {
                            user = Context.DataContext.Users.Where(t => t.Id == counterOffer.BuyerId).Select(t => new
                            {
                                t.Id,
                                t.Email,
                                t.PhoneNumber,
                                t.FirstName,
                                t.LastName,
                                CompanyName = t.Company.Name
                            }).SingleOrDefault();
                        }
                    }
                    customerEmail = user.Email;
                    customerPhone = user.PhoneNumber;
                    customerName = $"{user.FirstName} {user.LastName}";
                    BuyerCompanyName = user.CompanyName;

                    //SET REPSONSE MODEL VALUES
                    response.IsAssetTracked = order.Job.IsAssetTracked;
                    response.InvoiceTypeId = order.DefaultInvoiceType;
                    response.IsVariousOrigin = order.FuelRequest.FreightOnBoardTypeId == (int)FreightOnBoardTypes.Terminal
                                                && order.Job.LocationType == JobLocationTypes.Various;
                    response.InvoiceNotes = order.OrderAdditionalDetail != null ? order.OrderAdditionalDetail.Notes : null;

                    // set preferences setting details
                    response.PreferencesSettingId = order.OrderAdditionalDetail != null ? order.OrderAdditionalDetail.PreferencesSettingId : null;
                    if(response.PreferencesSettingId != null && response.PreferencesSettingId > 0)
                    {
                        
                       // response.IsSupressOrderPricing = preferencesSetting != null && order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest ? preferencesSetting.IsSupressOrderPricing : false;
                        response.IsSupressOrderPricing = order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest ? order.OrderAdditionalDetail.IsSupressPricingEnabled : false;
                        if (response.IsSupressOrderPricing)
                            response.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                    }

                    // Please fill FuelDropLocation if available here
                    if (response.IsVariousOrigin)
                    {
                        response.FuelDropLocation = new DropAddressViewModel();
                        response.FuelDropLocation.State = new StateViewModel() { Id = order.Job.StateId, Code = order.Job.StateCode };
                        response.FuelDropLocation.Country = new CountryViewModel() { Id = order.Job.CountryId, Code = order.Job.CountryCode };
                    }

                    //Carriers already exists for autocomplete 
                    response.Customer = new CustomerViewModel()
                    {
                        CompanyId = order.BuyerCompanyId,
                        CompanyName = BuyerCompanyName,
                        ContactEmail = customerEmail,
                        ContactName = customerName,
                        ContactPhone = customerPhone,
                        Location = new JobLocationViewModel()
                        {
                            JobId = job.JobId,
                            Address = job.Address,
                            City = job.City,
                            SiteName = job.Name,
                            StateCode = job.Statecod,
                            ZipCode = job.ZipCode,
                            CountryId = job.CountryId,
                            IsMarineLocation = job.IsMarine
                        }
                    };
                    if (order.OrderAdditionalDetail != null && order.OrderAdditionalDetail.Carrier != null)
                        response.Carrier = order.OrderAdditionalDetail.Carrier.Name;

                    var orderTax = order.OrderTaxDetails.Any() ? order.OrderTaxDetails.ToList().ToTaxViewModel() : null;

                    var invoiceDropModel = GetInvoiceDropModel(order, order.Pickup, orderTax, order.TrackableSchedule);
                    invoiceDropModel.FuelSurchargeFreightFee = GetFuelSurchargeDetails(order.FuelRequest.FuelRequestFees, order.OrderAdditionalDetail,
                                                                order.FuelRequest.TypeOfFuel, order.AcceptedCompanyId, order.BuyerCompanyId);
                    response.Drops.Add(invoiceDropModel);

                    if (order.Driver != null)
                        response.Driver = new DropdownDisplayItem() { Id = order.Driver.User.Id, Name = $"{order.Driver.User.FirstName} {order.Driver.User.LastName}" };

                    response.Fees = order.FuelRequest.FuelRequestFees.ToFeesViewModel();
                    response.Fees.RemoveAll(t => t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
                    response.Fees.ForEach(t => t.OrderId = orderId);

                    if (order.OrderAdditionalDetail.AccessorialFeeId.HasValue && order.OrderAdditionalDetail.AccessorialFeeId.Value > 0)
                    {
                        response.AccessorialFeeDetails = new List<AccessorialFeeTableDetailViewModel>();
                        var accessorialFeeDetails = new AccessorialFeeTableDetailViewModel();
                        accessorialFeeDetails.AccessorialFeeTableType = order.OrderAdditionalDetail.AccessorialFeeTableType;
                        accessorialFeeDetails.AccessorialFeeId = order.OrderAdditionalDetail.AccessorialFeeId;
                        response.AccessorialFeeDetails.Add(accessorialFeeDetails);
                    }

                    response.PaymentTerm = new PaymentTermViewModel()
                    {
                        NetDays = order.OrderDetailVersion.NetDays,
                        TermId = (PaymentTerms)order.OrderDetailVersion.PaymentTermId
                    };
                    response.IsBadgeMandatory = IsBadgeNumberMandatory(order.Id, userContext.CompanyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetPoDetailsToCreateInvoice", ex.Message, ex);
            }
            return response;
        }

        private InvoiceDropViewModel GetInvoiceDropModel(dynamic order, FuelDispatchLocation dispatchLocation, List<TaxViewModel> orderTaxDetails, DeliveryScheduleXTrackableSchedule trackableSchedule = null)
        {
            var item = new InvoiceDropViewModel();
            item.OrderId = order.Id;
            item.PoNumber = order.PoNumber;
            item.FuelTypeName = order.FuelRequest.ProductName;
            item.FuelTypeId = order.FuelRequest.FuelTypeId;
            item.MinDropDate = order.FuelRequest.FuelRequestStartDate;
            item.DisplayMinDropDate = order.FuelRequest.FuelRequestStartDate.ToString(Resource.constFormatDate);
            item.TerminalId = order.TerminalId;
            item.TerminalName = order.TerminalName;
            item.TypeOfFuel = order.FuelRequest.TypeOfFuel;
            item.IsBOLImageRequired = order.IsBolImageRequired;
            item.IsDropImageRequired = order.IsDropImageRequired;
            item.IsSignatureRequired = order.SignatureEnabled;
            item.IsDipDataRequired = order.IsDipDataRequired;
            item.IsFreightOnlyOrder = order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest;
            item.JobCountryId = order.Job.CountryId;
            item.IsMarineLocation = order.Job.IsMarine;
            item.IsSupressOrderPricing = order.IsSupressOrderPricing;

            if (!order.IsFTL)
            {
                item.IsAssetTracked = order.Job.IsAssetTracked;
            }
            item.QuantityIndicatorTypeId = order.QuantityIndicatorTypeId;
            item.IsFTL = order.IsFTL;
            if (order.OrderAdditionalDetail != null)
            {
                item.Allowance = order.OrderAdditionalDetail.Allowance;
                item.IsBolDetailsRequired = order.OrderAdditionalDetail.IsDriverToUpdateBOL;  // bol details required flag
                item.FreightPricingMethod = order.OrderAdditionalDetail.FreightPricingMethod;
            }
            item.UoM = order.FuelRequest.UoM;
            item.Currency = order.FuelRequest.Currency;
            
            if (dispatchLocation != null)
            {
                item.PickupLocationType = dispatchLocation.TerminalId.HasValue ? PickupLocationType.Terminal : PickupLocationType.BulkPlant;
            }
            if (item.PickupLocationType == PickupLocationType.BulkPlant)
            {
                item.PickUpAddress = dispatchLocation.ToPickUpLocation();
            }
            if (orderTaxDetails != null && orderTaxDetails.Any())
            {
                item.OtherTaxDetails = orderTaxDetails;
            }
            var invoiceDomain = new InvoiceDomain(this);
            item.Assets = Task.Run(() => invoiceDomain.GetAssignedAssetsAsync(item.OrderId)).Result;

            if(trackableSchedule != null)
            {
                item.TrackableScheduleId = trackableSchedule.Id;
                item.StartTime = Convert.ToDateTime(trackableSchedule.StartTime.ToString()).ToString(Resource.constFormat12HourTime2);
                item.EndTime = Convert.ToDateTime(trackableSchedule.EndTime.ToString()).ToString(Resource.constFormat12HourTime2);
                item.DropDate = trackableSchedule.Date;
                item.DisplayDropDate = trackableSchedule.Date.ToString(Resource.constFormatDate);
                item.ActualDropQuantity = trackableSchedule.Quantity;
            }

            return item;
        }

        public async Task<InvoiceDropViewModel> GetInvoiceDropModel(int orderId)
        {
            var order = await Context.DataContext.Orders
                                        .Where(t => t.Id == orderId && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                    .Select(t => new
                                    {

                                        t.Id,
                                        t.IsFTL,
                                        t.PoNumber,
                                        FuelRequest = new
                                        {
                                            t.FuelRequest.FuelTypeId,
                                            t.FuelRequest.FuelRequestTypeId,
                                            FuelRequestStartDate = t.FuelRequest.FuelRequestDetail.StartDate,
                                            ProductName = t.FuelRequest.MstProduct.DisplayName ?? t.FuelRequest.MstProduct.Name,
                                            FuelRequestFees = t.FuelRequest.FuelRequestFees,
                                            TypeOfFuel = t.FuelRequest.MstProduct.ProductTypeId,
                                            UoM = t.FuelRequest.UoM,
                                            Currency = t.FuelRequest.Currency
                                        },
                                        Job = new
                                        {
                                            t.FuelRequest.Job.JobBudget.IsAssetTracked,
                                            t.FuelRequest.Job.CountryId,
                                            t.FuelRequest.Job.IsMarine
                                        },
                                        OrderAdditionalDetail = t.OrderAdditionalDetail ?? null,
                                        t.TerminalId,
                                        t.AcceptedCompanyId,
                                        t.BuyerCompanyId,
                                        TerminalName = t.MstExternalTerminal.Name,
                                        Pickup = t.FuelDispatchLocations.FirstOrDefault(t1 => t1.IsActive && t1.LocationType == (int)LocationType.PickUp),
                                        OrderTaxDetails = t.OrderTaxDetails.Where(t1 => t1.IsActive).ToList(),
                                        IsBolImageRequired = t.FuelRequest.FuelRequestDetail.IsBolImageRequired,
                                        IsDropImageRequired = t.FuelRequest.FuelRequestDetail.IsDropImageRequired,
                                        IsDipDataRequired = t.FuelRequest.FuelRequestDetail.IsPrePostDipRequired,
                                        SignatureEnabled = t.FuelRequest.Job.SignatureEnabled ? t.FuelRequest.Job.SignatureEnabled : t.SignatureEnabled,
                                        QuantityIndicatorTypeId = t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId,
                                        //  IsSupressOrderPricing = t.OrderAdditionalDetail.OnboardingPreference != null ? t.OrderAdditionalDetail.OnboardingPreference.IsSupressOrderPricing : false
                                        IsSupressOrderPricing = t.OrderAdditionalDetail.IsSupressPricingEnabled
                                    })
                                    .SingleOrDefaultAsync();
            if (order != null)
            {
                
                var orderTax = order.OrderTaxDetails.Any() ? order.OrderTaxDetails.ToList().ToTaxViewModel() : null;
                var invoiceDropModel = GetInvoiceDropModel(order, order.Pickup, orderTax);
                
                invoiceDropModel.FuelSurchargeFreightFee = GetFuelSurchargeDetails(order.FuelRequest.FuelRequestFees, order.OrderAdditionalDetail,
                    order.FuelRequest.TypeOfFuel, order.AcceptedCompanyId, order.BuyerCompanyId);
                    return invoiceDropModel;
            }
            return null;
        }

        public async Task<List<FeesViewModel>> GetInvoiceDropFeesAsync(int orderId)
        {
            var response = new List<FeesViewModel>();
            try
            {
                var fees = await Context.DataContext.Orders.Where(t => t.Id == orderId)
                                .SelectMany(t => t.FuelRequest.FuelRequestFees)
                                .ToListAsync();
                response = fees.ToFeesViewModel();
                response.RemoveAll(t => t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
                response.ForEach(t => t.OrderId = orderId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetInvoiceDropFeesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<CustomerPoViewModel>> GetCustomerPoList(UserContext userContext, int orderId)
        {
            var response = new List<CustomerPoViewModel>();
            try
            {
                var order = await Context.DataContext.Orders
                                        .Where(t => t.AcceptedCompanyId == userContext.CompanyId && t.Id == orderId)
                                                    .Select(t => new { t.FuelRequest.JobId, t.BuyerCompanyId, FreightPricingMethod = t.OrderAdditionalDetail != null ? t.OrderAdditionalDetail.FreightPricingMethod : FreightPricingMethod.Manual,
                                                    t.FuelRequest.Job.IsMarine,
                                                        VesselId = t.FuelRequest.Job.JobXAssets.Any() ? t.FuelRequest.Job.JobXAssets.Where(t1=>t1.OrderId == orderId).Select(t1=>t1.AssetId).FirstOrDefault() :0

                                                    }).FirstOrDefaultAsync();

                if (order != null && !order.IsMarine)
                {
                    response = await Context.DataContext.Orders
                               .Where(t => t.AcceptedCompanyId == userContext.CompanyId && t.FuelRequest.JobId == order.JobId && t.BuyerCompanyId == order.BuyerCompanyId
                                           && t.Id != orderId && (t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                           || (t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && !t.Invoices.Any() && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed)))
                               .Select(t => new CustomerPoViewModel
                               {
                                   FuelTypeId = t.FuelRequest.FuelTypeId,
                                   FuelTypeName = t.FuelRequest.MstProduct.DisplayName == null ? t.FuelRequest.MstProduct.Name : t.FuelRequest.MstProduct.DisplayName,
                                   OrderId = t.Id,
                                   PoNumber = t.PoNumber,
                                   DisplayPoNumber = t.PoNumber + " | " + (t.FuelRequest.MstProduct.DisplayName ?? t.FuelRequest.MstProduct.Name)
                               }).ToListAsync();
                }
                else if (order != null && order.IsMarine)
                {
                      response =  await Context.DataContext.JobXAssets
                                        .Where(t => t.AssetId == order.VesselId
                                        && t.JobId == order.JobId
                                        && t.Order.AcceptedCompanyId == userContext.CompanyId                                        
                                        && t.Order.BuyerCompanyId == order.BuyerCompanyId
                                        && t.Order.Id != orderId
                                        && (t.Order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                        || (t.Order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && !t.Order.Invoices.Any()
                                                   && t.Order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed))
                                        )
                                        .Select(t=> new CustomerPoViewModel 
                                        {
                                            FuelTypeId = t.Order.FuelRequest.FuelTypeId,
                                            FuelTypeName = t.Order.FuelRequest.MstProduct.DisplayName == null ? t.Order.FuelRequest.MstProduct.Name : t.Order.FuelRequest.MstProduct.DisplayName,
                                            OrderId = t.Order.Id,
                                            PoNumber = t.Order.PoNumber,
                                            DisplayPoNumber = t.Order.PoNumber + " | " + (t.Order.FuelRequest.MstProduct.DisplayName ?? t.Order.FuelRequest.MstProduct.Name)
                                        }).ToListAsync();
                   
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetCustomerPoList", ex.Message, ex);
            }
            return response;
        }

        public FuelSurchargeFreightFeeViewModel GetFuelSurchargeDetails(ICollection<FuelFee> fuelRequestFees,
            OrderAdditionalDetail orderAdditionalDetail, int productTypeId, int acceptedCompanyId, int buyerCompanyId)
        {
            var response = new FuelSurchargeFreightFeeViewModel();
            var freightPricingMethod = orderAdditionalDetail != null ? orderAdditionalDetail.FreightPricingMethod : FreightPricingMethod.Manual;
            response.FreightPricingMethod = freightPricingMethod;
            if (freightPricingMethod == FreightPricingMethod.Manual)
            {
                response = fuelRequestFees.ToSurchargeFreightFeesViewModel();
                if (orderAdditionalDetail != null && orderAdditionalDetail.IsFuelSurcharge)
                {
                    response.IsSurchargeApplicable = orderAdditionalDetail.IsFuelSurcharge;
                }
                if (response.IsSurchargeApplicable)
                {
                    response.BuyerCompanyId = buyerCompanyId;
                    response.IsFeeByDistance = response.DeliveryFeeByQuantity.Any();
                    if (orderAdditionalDetail != null)
                    {
                        response.SurchargePricingType = orderAdditionalDetail.FuelSurchagePricingType.HasValue ? (FuelSurchagePricingType)orderAdditionalDetail.FuelSurchagePricingType.Value : FuelSurchagePricingType.Unknown;
                    }
                    response.SurchargeProductType = productTypeId.GetFuelSurchargeProductType();
                    if (response.SurchargeProductType != SurchargeProductTypes.Unknown)
                    {
                        response.SurchargeFreightCost = response.Fee;
                        if (response.IsFeeByDistance)
                        {
                            response.SurchargeFreightCost = response.DeliveryFeeByQuantity.Where(t => t.MinQuantity >= 0).Select(t => t.Fee).FirstOrDefault();
                        }
                        var eiaDomain = new EIAPriceUpdateDomain(this);
                        response.SurchargeEiaPrice = eiaDomain.GetEIAPrice(response.SurchargePricingType,
                                                    response.SurchargeProductType, DateTimeOffset.Now.Date);
                        var fsc = new FuelSurchargeDomain(this).GetFuelSurchargeRecordForEaiPrice(response.SurchargeEiaPrice, acceptedCompanyId, buyerCompanyId, DateTimeOffset.Now.Date, response.SurchargeProductType);
                        if (fsc != null)
                        {
                            response.SurchargePercentage = fsc.FuelSurchargeStartPercentage;
                            response.SurchargeTableRangeStart = fsc.PriceRangeStartValue;
                            response.SurchargeTableRangeEnd = fsc.PriceRangeEndValue;
                        }
                    }
                }
            }
            else if (freightPricingMethod == FreightPricingMethod.Auto)
            {
                GetAutoFuelSurchargeDetails(orderAdditionalDetail, acceptedCompanyId, buyerCompanyId, response);
            }
            
            return response;
        }

        private void GetAutoFuelSurchargeDetails(OrderAdditionalDetail orderAdditionalDetail, int acceptedCompanyId, int buyerCompanyId, FuelSurchargeFreightFeeViewModel response)
        {
            if (orderAdditionalDetail != null && orderAdditionalDetail.FuelSurchargeTableId.HasValue && orderAdditionalDetail.FuelSurchargeTableId.Value > 0)
            {
                response.FuelSurchargeTableType = orderAdditionalDetail.FuelSurchargeTableType;
                response.FuelSurchargeTableId = orderAdditionalDetail.FuelSurchargeTableId;

                response.IsSurchargeApplicable = orderAdditionalDetail.IsFuelSurcharge;
                response.BuyerCompanyId = buyerCompanyId;

                var fuelSurchargeIndexId = orderAdditionalDetail.FuelSurchargeTableId.Value;
                var autoResponse = Task.Run(() => ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetFuelSurchargeTableForAutoFreightMethod(fuelSurchargeIndexId)).Result;

                var eiaDomain = new EIAPriceUpdateDomain(this);
                response.SurchargeEiaPrice = eiaDomain.GetEIAPrice(autoResponse.SurchargePricingType, autoResponse.SurchargeProductType, DateTimeOffset.Now.Date, autoResponse.FuelSurchageArea);
                var fsc = new FuelSurchargeDomain(this).GetFuelSurchargeRecordForEaiPriceForAutoFreightMethod(response.SurchargeEiaPrice, fuelSurchargeIndexId);
                if (fsc != null)
                {
                    response.SurchargePercentage = fsc.FuelSurchargeStartPercentage;
                    response.SurchargeTableRangeStart = fsc.PriceRangeStartValue;
                    response.SurchargeTableRangeEnd = fsc.PriceRangeEndValue;
                }
            }

            if (orderAdditionalDetail != null && orderAdditionalDetail.FreightRateRuleId.HasValue && orderAdditionalDetail.FreightRateRuleId.Value > 0)
            {
                response.IsFreightCostApplicable = orderAdditionalDetail.IsFreightCost;
                response.FreightRateRuleId = orderAdditionalDetail.FreightRateRuleId;
                response.FreightRateRuleType = orderAdditionalDetail.FreightRateRuleType;
                response.FreightRateTableType = orderAdditionalDetail.FreightRateTableType;
            }
        }

        public async Task<List<MobileDropAditionalDetailViewModel>> GetMobileDropAditionalDetails(List<int> orderIds, InvoiceViewModelNew model)
        {
            var additionalDetails = new List<MobileDropAditionalDetailViewModel>();
            var dropDetails = await Context.DataContext.Orders.Where(t => orderIds.Contains(t.Id))
                                    .Select(t => new
                                    {
                                        OrderId = t.Id,
                                        t.PoNumber,
                                        t.BuyerCompanyId,
                                        t.AcceptedCompanyId,
                                        InvoiceTypeId = t.DefaultInvoiceType,
                                        OrderAdditionalDetail = t.OrderAdditionalDetail == null ? null : new
                                        {
                                            t.OrderAdditionalDetail.Allowance,
                                            t.OrderAdditionalDetail.Notes,
                                            t.OrderAdditionalDetail.IsFuelSurcharge,
                                            t.OrderAdditionalDetail.FuelSurchagePricingType,
                                            t.OrderAdditionalDetail.IsDriverToUpdateBOL,
                                            t.OrderAdditionalDetail.FreightPricingMethod,
                                            t.OrderAdditionalDetail.FreightRateRuleId,
                                            t.OrderAdditionalDetail.FuelSurchargeTableId,
                                            t.OrderAdditionalDetail.FuelSurchargeTableType,
                                            t.OrderAdditionalDetail.FreightRateRuleType,
                                            t.OrderAdditionalDetail.FreightRateTableType,
                                            t.OrderAdditionalDetail.IsFreightCost
                                        },
                                        t.OrderTaxDetails,
                                        t.FuelRequest.FuelRequestFees,
                                        t.FuelRequest.MstProduct.ProductTypeId,
                                        t.FuelRequest.SpecialInstructions,
                                        PaymentTerm = t.OrderDetailVersions.Where(x => x.IsActive).Select(x => new { x.PaymentTermId, x.NetDays }).FirstOrDefault(),
                                        t.FuelRequest.FuelRequestDetail.IsBolImageRequired,
                                        t.FuelRequest.FuelRequestDetail.IsDropImageRequired,
                                        SignatureEnabled = t.FuelRequest.Job.SignatureEnabled ? t.FuelRequest.Job.SignatureEnabled : t.SignatureEnabled,
                                        t.FuelRequest.Job.TimeZoneName,
                                        t.FuelRequest.JobId,
                                        t.FuelRequest.Job.IsMarine,
                                        Pickup = t.FuelDispatchLocations.FirstOrDefault(t1 => t1.IsActive && t1.LocationType == (int)LocationType.PickUp)
                                        //QuantityIndicatorTypeId = t.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId
                                    }).ToListAsync();
            foreach (var item in dropDetails)
            {
                var additionalDetail = new MobileDropAditionalDetailViewModel();
                additionalDetail.OrderId = item.OrderId;
                additionalDetail.JobId = item.JobId;
                additionalDetail.PoNumber = item.PoNumber;
                additionalDetail.TimeZoneName = item.TimeZoneName;
                additionalDetail.BuyerCompanyId = item.BuyerCompanyId;
                additionalDetail.SupplierCompanyId = item.AcceptedCompanyId;
                additionalDetail.IsMarineLocation = item.IsMarine;
                additionalDetail.PaymentTerm = new PaymentTermViewModel()
                {
                    NetDays = item.PaymentTerm.NetDays,
                    TermId = (PaymentTerms)item.PaymentTerm.PaymentTermId
                };

                if (item.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual)
                    additionalDetail.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketMobileApp;
                else
                    additionalDetail.InvoiceTypeId = (int)InvoiceType.MobileApp;
                
                if (item.OrderAdditionalDetail != null)
                {
                    additionalDetail.FreightPricingMethod = item.OrderAdditionalDetail.FreightPricingMethod;
                    additionalDetail.Allowance = item.OrderAdditionalDetail.Allowance;
                    additionalDetail.InvoiceNotes = item.OrderAdditionalDetail.Notes;
                    additionalDetail.SpecialInstructions = item.SpecialInstructions.Select(t => t.ToViewModel()).ToList();
                    var orderAdditionalDetail = new OrderAdditionalDetail()
                    {
                        Notes = item.OrderAdditionalDetail.Notes,
                        IsFuelSurcharge = item.OrderAdditionalDetail.IsFuelSurcharge,
                        FuelSurchagePricingType = item.OrderAdditionalDetail.FuelSurchagePricingType,
                        IsDriverToUpdateBOL = item.OrderAdditionalDetail.IsDriverToUpdateBOL,
                        FreightPricingMethod = item.OrderAdditionalDetail.FreightPricingMethod,
                        FuelSurchargeTableId = item.OrderAdditionalDetail.FuelSurchargeTableId,
                        FuelSurchargeTableType = item.OrderAdditionalDetail.FuelSurchargeTableType,
                        FreightRateRuleId = item.OrderAdditionalDetail.FreightRateRuleId,
                        FreightRateRuleType = item.OrderAdditionalDetail.FreightRateRuleType,
                        FreightRateTableType = item.OrderAdditionalDetail.FreightRateTableType,
                        IsFreightCost = item.OrderAdditionalDetail.IsFreightCost
                    };
                    additionalDetail.FuelSurcharge = GetFuelSurchargeDetails(item.FuelRequestFees, orderAdditionalDetail, item.ProductTypeId, item.AcceptedCompanyId, item.BuyerCompanyId);
                    var drop = model.Drops.Where(t => t.OrderId == item.OrderId).FirstOrDefault();
                    if (drop != null)
                    {
                        if (orderAdditionalDetail.FreightPricingMethod == FreightPricingMethod.Manual)
                        {
                            additionalDetail.FuelSurcharge.TotalFuelSurchargeFee = GetFuelSurchageFrieghtFee(additionalDetail.FuelSurcharge.SurchargePercentage, additionalDetail.FuelSurcharge.SurchargeFreightCost, drop.ActualDropQuantity);
                        }
                        else if (orderAdditionalDetail.FreightPricingMethod == FreightPricingMethod.Auto && orderAdditionalDetail.FreightRateRuleType != FreightRateRuleType.Range)
                        {
                            additionalDetail.FuelSurcharge.SurchargeFreightCost = await GetSurchargeFreightCostForAuto(item, drop, model.FuelDropLocation);
                            additionalDetail.FuelSurcharge.TotalFuelSurchargeFee = GetFuelSurchageFrieghtFee(additionalDetail.FuelSurcharge.SurchargePercentage, additionalDetail.FuelSurcharge.SurchargeFreightCost, drop.ActualDropQuantity);
                        }
                    }
                    additionalDetail.IsBolDetailsRequired = item.OrderAdditionalDetail.IsDriverToUpdateBOL;
                }
                if(item.Pickup!=null)
                {
                    additionalDetail.PickupLocationType = item.Pickup.TerminalId.HasValue ? PickupLocationType.Terminal : PickupLocationType.BulkPlant;
                    additionalDetail.BulkPlantAddress = item.Pickup.ToPickUpLocation();
                }
                additionalDetail.IsSignatureRequired = item.SignatureEnabled;
                additionalDetail.IsBOLImageRequired = item.IsBolImageRequired;
                additionalDetail.IsDropImageRequired = item.IsDropImageRequired;
                //additionalDetail.QuantityIndicatorTypeId = item.QuantityIndicatorTypeId;

                additionalDetail.Fees = item.FuelRequestFees.ToFeesViewModel();
                additionalDetail.Fees.ForEach(t => t.OrderId = item.OrderId);
                if (item.ProductTypeId == (int)ProductTypes.NonStandardFuel)
                {
                    additionalDetail.OtherTaxDetails = item.OrderTaxDetails.ToTaxViewModel();
                }
                additionalDetails.Add(additionalDetail);
            }
            return additionalDetails;
        }

        private async Task<decimal> GetSurchargeFreightCostForAuto(dynamic order, InvoiceDropViewModel drop, DropAddressViewModel fuelDropLocation)
        {
            decimal response = 0;
            try
            {   
                var freightCostInput = new FreightCostInputViewModel();
                if (order.OrderAdditionalDetail.FreightRateRuleId != 0)
                {
                    freightCostInput.FreightRateRuleId = order.OrderAdditionalDetail.FreightRateRuleId;
                    freightCostInput.OrderId = order.OrderId;
                    freightCostInput.SupplierId = order.AcceptedCompanyId;
                    freightCostInput.DeliveredQuantity = drop.ActualDropQuantity;

                    if (drop.TerminalId.HasValue)
                    {
                        freightCostInput.TerminalId = drop.TerminalId.Value;
                    }
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFreightCostForInvoice(freightCostInput);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreateDomain", "GetSurchargeFreightCostForAuto", ex.Message, ex);
            }
            return response;
        }

        #endregion
    }
}
