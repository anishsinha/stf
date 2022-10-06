using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
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
    public class InvoiceBaseDomain : BaseDomain
    {
        public InvoiceBaseDomain(string connectionString) : base(connectionString)
        {
        }

        public InvoiceBaseDomain(BaseDomain domain) : base(domain)
        {
        }

        public static bool IsDigitalDropTicket(int invoiceTypeId)
        {
            return invoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp;
        }

        protected string SetFirstZipCodeOfState(int stateId, string currentState, out string stateCode)
        {
            var zipList = Context.DataContext.MstCities.Where(t => t.StateId == stateId).FirstOrDefault();
            if (zipList != null)
            {
                stateCode = zipList.MstState.Code;
                var firstZip = zipList.ZipCodes.Split(',').FirstOrDefault();
                if (firstZip != null)
                    return firstZip;
            }
            else
                stateCode = currentState;
            return string.Empty;
        }
        SAPEnterpriseDomain sapDomain = new SAPEnterpriseDomain();
        protected async Task CheckForSplitLoadInvoiceAndGenerateStatement(string splitLoadChainId, int companyId, string timeZoneName)
        {
            var splitLoadInvoices = await Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == splitLoadChainId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive).ToListAsync();
            if (!splitLoadInvoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
            {
                BillingStatementDomain statementDomain = new BillingStatementDomain(this);
                await statementDomain.GeneateBillingStatementForSplitLoadInvoice(splitLoadInvoices, timeZoneName, companyId);
            }
        }

        protected async Task<List<InvoiceCreateResponseViewModel>> CreateMobileInvoiceAsync(InvoiceCreateRequestViewModel viewModel, List<InvoiceCreateRequestViewModel> brokeredViewModels, List<AssetDrop> mobileAssetDrops)
        {
            var response = new List<InvoiceCreateResponseViewModel>();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var invoiceHeader = viewModel.InvoiceModel.ToInvoiceHeaderEntity();

            var invoice = viewModel.InvoiceModel.ToEntity();
            invoice.AssetDrops = mobileAssetDrops;

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var invoiceResponse = new InvoiceCreateResponseViewModel();
                    if (order != null)
                    {
                        Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();
                        await SetMobileInvoiceBolDetails(invoiceHeader.Id, viewModel.InvoiceModel, invoice.Id);
                        await SaveBulkPlantLocations(viewModel.InvoiceModel.BolDetails);

                        var deliveryReqStatus = await UpdateMobileInvoiceDependentEntitiesPostCreate(viewModel, order, invoice, invoiceResponse);
                        var brokersResponse = await CreateMobileInvoicesForBrokers(brokeredViewModels, invoice.ImageId, invoice.SignatureId);

                        await Context.CommitAsync();
                        transaction.Commit();

                        ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
                        invoiceResponse.StatusCode = Status.Success;

                        response.Add(invoiceResponse);
                        response.AddRange(brokersResponse);
                        if (deliveryReqStatus != null)
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateMobileInvoiceAsync", ex.Message, ex);
                }
            }

            return response;
        }

        protected async Task<List<InvoiceCreateResponseViewModel>> CreateFtlMobileInvoiceAsync(InvoiceCreateRequestViewModel viewModel, List<InvoiceCreateRequestViewModel> brokeredViewModels, List<AssetDrop> mobileAssetDrops)
        {
            var response = new List<InvoiceCreateResponseViewModel>();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var invoiceHeader = viewModel.InvoiceModel.ToInvoiceHeaderEntity();

            var invoice = viewModel.InvoiceModel.ToEntity();
            invoice.AssetDrops = mobileAssetDrops;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var invoiceResponse = new InvoiceCreateResponseViewModel();
                    DeliveryReqStatusUpdateModel deliveryReqStatus = null;
                    if (order != null)
                    {
                        Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                        await Context.CommitAsync();

                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();
                        await SetMobileInvoiceBolDetails(invoiceHeader.Id, viewModel.InvoiceModel, invoice.Id);
                        if (!viewModel.IsSplitLoad)
                        {
                            deliveryReqStatus = await UpdateMobileInvoiceDependentEntitiesPostCreate(viewModel, order, invoice, invoiceResponse);
                        }
                        await SaveBulkPlantLocations(viewModel.InvoiceModel.BolDetails);

                        var brokersResponse = await CreateMobileInvoicesForBrokers(brokeredViewModels, invoice.ImageId, invoice.SignatureId);

                        await Context.CommitAsync();
                        transaction.Commit();

                        ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
                        invoiceResponse.StatusCode = Status.Success;

                        response.Add(invoiceResponse);
                        response.AddRange(brokersResponse);
                        if (deliveryReqStatus != null)
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateFtlMobileInvoiceAsync", ex.Message, ex);
                }
            }

            return response;
        }

        protected async Task<List<InvoiceCreateResponseViewModel>> CreateManualInvoiceAsync(InvoiceCreateRequestViewModel viewModel, List<InvoiceCreateRequestViewModel> brokeredViewModels, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var response = new List<InvoiceCreateResponseViewModel>();
            DeliveryReqStatusUpdateModel deliveryReqStatus = null;
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var invoiceHeader = viewModel.InvoiceModel.ToInvoiceHeaderEntity();

            var invoice = viewModel.InvoiceModel.ToEntity();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var assetDropViewModel = SetAssetDropsToInvoice(viewModel.JobId, viewModel.JobCompanyId, viewModel.InvoiceModel.CreatedBy, viewModel.InvoiceModel.DriverId, viewModel.InvoiceModel.DropEndDate, manualInvoiceViewModel.Assets);
                    var assetDrops = assetDropViewModel.Select(t => t.ToEntity()).ToList();
                    invoice.AssetDrops = assetDrops;

                    var invoiceResponse = new InvoiceCreateResponseViewModel();
                    if (order != null)
                    {
                        Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        order.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        await SetInvoiceBolDetails(invoiceHeader, viewModel.InvoiceModel, invoice);
                        await Context.CommitAsync();
                        await SaveBulkPlantLocations(viewModel.InvoiceModel.BolDetails);

                        deliveryReqStatus = UpdateManualInvoiceDependentEntitiesPostCreate(viewModel, order, invoice, invoiceResponse);
                        var assetDropModel = assetDrops.Select(t => t.ToViewModel()).ToList();
                        var brokersResponse = await CreateManualInvoicesForBrokers(brokeredViewModels, assetDropModel, invoice.ImageId, invoice.SignatureId);

                        await Context.CommitAsync();
                        transaction.Commit();

                        ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
                        invoiceResponse.StatusCode = Status.Success;

                        response.Add(invoiceResponse);
                        response.AddRange(brokersResponse);
                        if (deliveryReqStatus != null)
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateManualInvoiceAsync", ex.Message, ex);
                }
            }

            return response;
        }

        protected async Task<List<DeliveryReqStatusUpdateModel>> CreateConsolidatedManualInvoice(List<InvoiceModel> invoices, InvoiceViewModelNew invoiceViewModel, InvoiceNumber invoiceNumber, List<DropAdditionalDetailsModel> dropAdditionalDetails)
        {
            List<DeliveryReqStatusUpdateModel> delReqStatuses = new List<DeliveryReqStatusUpdateModel>();
            try
            {
                var invoiceHeader = GenerateInvoiceHeader(invoices, invoiceViewModel.ExistingHeaderId);
                Order dtnOrder = null; Invoice dtnInvoice = null; bool sendDtnFile = true;
                Order pdiOrder = null; Invoice pdiInvoice = null; bool sendDeliveryDetailsToPDI = false;
                Invoice firstInvoice = null;
                var firstOrderId = (invoiceHeader.Invoices != null && invoiceHeader.Invoices.Count > 0) ? invoiceHeader.Invoices.OrderBy(t => t.OrderId).Select(t => t.OrderId).FirstOrDefault().ToString() : invoices.OrderBy(t => t.OrderId).Select(t => t.OrderId).FirstOrDefault().ToString();
                foreach (var invoiceModel in invoices)
                {
                    var dropAdditionalDetail = dropAdditionalDetails.FirstOrDefault(t => t.OrderId == invoiceModel.OrderId);
                    SetInvoiceNumber(invoiceNumber.Number, invoiceModel);
                    var invoice = invoiceModel.ToEntity();
                    //Set Same BDR number for all invoices in the consolidated invoice, so different BDRNumber will not go in BDRDetails Table, BDRNumber = FirstOrDefault OrderId from invoices list
                    if (invoice.BDRDetail != null)
                    {
                        invoice.BDRDetail.BDRNumber = firstOrderId;
                    }
                    var assetDrops = invoiceModel.AssetDrops.Select(t => t.ToEntity()).ToList();
                    invoice.AssetDrops = assetDrops;
                    invoiceHeader.Invoices.Add(invoice);
                    await Context.CommitAsync();
                    await SetInvoiceBolDetails(invoiceHeader, invoiceModel, invoice);
                    await SaveBulkPlantLocations(invoiceModel.BolDetails);

                    var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == invoiceModel.OrderId);
                    var delReqStatus = UpdateInvoiceDependentEntitiesPostCreate(invoiceModel, order, invoice, dropAdditionalDetail);
                    if (delReqStatus != null)
                    {
                        delReqStatuses.Add(delReqStatus);
                    }
                    await Context.CommitAsync();
                    if (order.SendDtnFile)
                    {
                        dtnOrder = order;
                        dtnInvoice = invoice;
                    }
                    else
                    {
                        sendDtnFile = false;
                    }
                    if (order.OrderAdditionalDetail != null && order.OrderAdditionalDetail.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails)
                    {
                        sendDeliveryDetailsToPDI = true;
                        pdiOrder = order;
                        pdiInvoice = invoice;
                    }
                    invoiceModel.Id = invoice.Id;
                    invoiceModel.DisplayInvoiceNumber = invoice.DisplayInvoiceNumber;
                    invoiceModel.InvoiceHeaderId = invoiceHeader.Id;
                    if (firstInvoice == null)
                    {
                        firstInvoice = invoice;
                    }
                }
                if (sendDtnFile)
                {
                    CreateDtnFileGenerationWorkflow(dtnInvoice, dtnOrder);
                }

                var isWaitingForNothing = invoices.All(t => t.WaitingFor == WaitingAction.Nothing || t.WaitingFor == WaitingAction.PDITaxes);
                if (isWaitingForNothing)
                {
                    var sapDomain = new SAPEnterpriseDomain(this);
                    sapDomain.CreateSAPWorkflow(firstInvoice);
                    if (sendDeliveryDetailsToPDI)
                    {
                        CreatePDIAPIWorkflow(pdiInvoice, pdiOrder);
                    }
                }
                if (firstInvoice != null)
                {
                    CreateQbAccountingWorkflowForInvoice(false, firstInvoice, firstInvoice.Order, null);
                    CreateQbAccountingWorkflowForBill(false, firstInvoice, firstInvoice.Order, null);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateConsolidatedManualInvoice", ex.Message, ex);
            }
            return delReqStatuses;
        }

        protected async Task<List<DeliveryReqStatusUpdateModel>> UpdateConsolidatedDropTicket(List<InvoiceModel> invoices, InvoiceViewModelNew invoiceViewModel, InvoiceNumber invoiceNumber, List<DropAdditionalDetailsModel> dropAdditionalDetails, StatusViewModel response)
        {
            List<DeliveryReqStatusUpdateModel> delReqStatuses = new List<DeliveryReqStatusUpdateModel>();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    bool isDdtToInvoiceConversion = false;
                    Order dtnOrder = null; Invoice dtnInvoice = null; bool sendDtnFile = true;
                    Order pdiOrder = null; Invoice pdiInvoice = null; bool sendDeliveryDetailsToPDI = false;

                    int invoiceVersion = InactivatePreviousInvoiceVersion(invoiceViewModel.OriginalInvoiceHeaderId.Value, invoices, out isDdtToInvoiceConversion);
                    if (invoices.Any(t => t.DisplayInvoiceNumber.Contains(ApplicationConstants.SFDD) && !t.IsDigitalDropTicket()))
                    {
                        var newInvoiceNumber = await GenerateInvoiceNumber_New();
                        invoices.ForEach(t => { t.InvoiceNumberId = newInvoiceNumber.Id; t.DisplayInvoiceNumber = newInvoiceNumber.Number; });
                    }
                    var invoiceHeader = CreateNewInvoiceHeaderVersion(invoices, invoiceVersion);
                    invoices.ForEach(t => t.Version = invoiceVersion);

                    foreach (var invoiceModel in invoices)
                    {
                        var dropAdditionalDetail = dropAdditionalDetails.FirstOrDefault(t => t.OrderId == invoiceModel.OrderId);
                        SetInvoiceNumber(invoiceNumber.Number, invoiceModel);
                        if (invoiceModel.AdditionalDetail.OriginalInvoiceHeaderId == invoiceViewModel.OriginalInvoiceHeaderId)
                        {
                            invoiceModel.AdditionalDetail.OriginalInvoiceHeaderId = null;
                            invoiceModel.AdditionalDetail.OriginalInvoiceId = null;
                        }
                        var invoice = invoiceModel.ToEntity();
                        if (isDdtToInvoiceConversion)
                            invoice.ParentId = invoiceModel.OriginalInvoiceId;
                        var assetDrops = invoiceModel.AssetDrops.Select(t => t.ToEntity()).ToList();
                        invoice.AssetDrops = assetDrops;
                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();
                        await SetInvoiceBolDetails(invoiceHeader, invoiceModel, invoice);
                        await SaveBulkPlantLocations(invoiceModel.BolDetails);

                        var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == invoiceModel.OrderId);
                        var delReqStatus = UpdateInvoiceDependentEntitiesPostCreate(invoiceModel, order, invoice, dropAdditionalDetail, invoiceViewModel.IsMissingDeliveryDDTConversion);
                        if (delReqStatus != null)
                        {
                            delReqStatuses.Add(delReqStatus);
                        }

                        await Context.CommitAsync();
                        invoiceModel.Id = invoice.Id;
                        invoiceModel.DisplayInvoiceNumber = invoice.DisplayInvoiceNumber;
                        invoiceModel.InvoiceHeaderId = invoiceHeader.Id;

                        if (order.SendDtnFile)
                        {
                            dtnOrder = order;
                            dtnInvoice = invoice;
                        }
                        else
                        {
                            sendDtnFile = false;
                        }

                        if (order.OrderAdditionalDetail != null && order.OrderAdditionalDetail.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails)
                        {
                            sendDeliveryDetailsToPDI = true;
                            pdiOrder = order;
                            pdiInvoice = invoice;
                        }

                        UpdatePreviousVersionTrackableScheduleAndOrderDetails(invoiceViewModel.OriginalInvoiceHeaderId.Value, invoiceViewModel);
                    }

                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageExceptionEDDTApproved;

                    if (sendDtnFile)
                    {
                        CreateDtnFileGenerationWorkflow(dtnInvoice, dtnOrder);
                    }

                    var isWaitingForNothing = invoices.All(t => t.WaitingFor == WaitingAction.Nothing || t.WaitingFor == WaitingAction.PDITaxes);
                    if (isWaitingForNothing)
                    {
                        sapDomain.CreateSAPWorkflow(dtnInvoice);
                        if (sendDeliveryDetailsToPDI)
                        {
                            CreatePDIAPIWorkflow(pdiInvoice, pdiOrder);
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessageFailedToApproveExceptionDDT;

                    if (transaction != null && transaction.UnderlyingTransaction.Connection != null)
                    {
                        transaction.Rollback();
                    }

                    LogManager.Logger.WriteException("InvoiceBaseDomain", "UpdateConsolidatedDropTicket", ex.Message, ex);
                }
            }
            return delReqStatuses;
        }

        protected async Task<List<DeliveryReqStatusUpdateModel>> CreateConsolidatedMobileInvoice(List<InvoiceModel> invoices, InvoiceViewModelNew invoiceViewModel, InvoiceNumber invoiceNumber, List<DropAdditionalDetailsModel> dropAdditionalDetails)
        {
            List<DeliveryReqStatusUpdateModel> delReqStatuses = new List<DeliveryReqStatusUpdateModel>();
            try
            {
                var invoiceHeader = GenerateInvoiceHeader(invoices);
                Order dtnOrder = null; Invoice dtnInvoice = null; bool sendDtnFile = true;
                Order pdiOrder = null; Invoice pdiInvoice = null; bool sendDeliveryDetailsToPDI = false;
                Invoice firstInvoice = null;
                int? firstOrderId = invoices.OrderBy(t => t.OrderId).Select(t => t.OrderId).FirstOrDefault();
                foreach (var invoiceModel in invoices)
                {
                    var dropAdditionalDetail = dropAdditionalDetails.FirstOrDefault(t => t.OrderId == invoiceModel.OrderId);
                    SetInvoiceNumber(invoiceNumber.Number, invoiceModel);
                    var invoice = invoiceModel.ToEntity();
                    if(invoice.BDRDetail != null)
                    {
                        invoice.BDRDetail.BDRNumber = firstOrderId.HasValue ? firstOrderId.ToString() : string.Empty;
                    }
                    if (invoiceModel.AssetDrops.Any())
                    {
                        // Tank drops will come directly in invoiceModel
                        var invoiceAssetDrops = new List<AssetDrop>();
                        var tankDrops = invoiceModel.AssetDrops.Where(t => t.Id == 0).Select(t => t.ToEntity()).ToList();
                        invoiceAssetDrops.AddRange(tankDrops); // Tank Drops

                        // Mobile Asset drops happen first and then invoice drop happens, So load asset drop make before drop completed.
                        var assetDropIds = invoiceModel.AssetDrops.Where(t => t.Id > 0).Select(t => t.Id).ToList();
                        if (assetDropIds.Any())
                        {
                            var assetDrops = await GetMobileAssetDropsAsync(assetDropIds);
                            //convert asset dropped qtys to Metric tons 
                            if ((invoice.UoM == UoM.MetricTons) || invoice.UoM == UoM.Barrels)
                            {
                                var invoiceDomain = new InvoiceDomain(this);
                                foreach (var asset in assetDrops)
                                {
                                    if (invoice.UoM == UoM.MetricTons)
                                    {
                                        if (asset.Gravity.HasValue && asset.Gravity.Value > 0)
                                        {
                                            var conversionRequest = new MFNConversionRequestViewModel() { DroppedGallons = asset.DroppedGallons, ConversionFactor = asset.Gravity.Value, JobCountryId = dropAdditionalDetail.CountryId, UoM = invoice.UoM };
                                            var response = Task.Run(() => invoiceDomain.ValidateGravityAndConvertForMFN(conversionRequest)).Result;
                                            asset.DroppedGallons = response.ConvertedQty;

                                        }
                                    }
                                    else if (invoice.UoM == UoM.Barrels)
                                    {
                                        var conversionRequest = new MFNConversionRequestViewModel() { DroppedGallons = asset.DroppedGallons, JobCountryId = dropAdditionalDetail.CountryId, UoM = invoice.UoM };
                                        var response = Task.Run(() => invoiceDomain.ValidateGravityAndConvertForMFN(conversionRequest)).Result;
                                        asset.DroppedGallons = response.ConvertedQty;
                                    }
                                }
                            }
                            invoiceAssetDrops.AddRange(assetDrops); // Asset drops
                            invoice.AssetDrops = invoiceAssetDrops;
                        }
                    }

                    if (invoiceViewModel.DropStatus == (int)MobileDropStatus.UnplannedDropCompleted)
                    {
                        var trackableScheduleId = await SaveDeliveryScheduleAsync(invoiceModel);
                        if (trackableScheduleId > 0)
                        {
                            invoiceModel.TrackableScheduleId = trackableScheduleId;
                        }
                    }
                    invoiceHeader.Invoices.Add(invoice);
                    await Context.CommitAsync();
                    await SetInvoiceBolDetails(invoiceHeader, invoiceModel, invoice);
                    await SaveBulkPlantLocations(invoiceModel.BolDetails);

                    var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == invoiceModel.OrderId);
                    var delReqStatus = UpdateInvoiceDependentEntitiesPostCreate(invoiceModel, order, invoice, dropAdditionalDetail);
                    if (delReqStatus != null)
                    {
                        delReqStatuses.Add(delReqStatus);
                    }
                    await Context.CommitAsync();
                    if (order.SendDtnFile)
                    {
                        dtnOrder = order;
                        dtnInvoice = invoice;
                    }
                    else
                    {
                        sendDtnFile = false;
                    }
                    if (order.OrderAdditionalDetail != null && order.OrderAdditionalDetail.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails)
                    {
                        sendDeliveryDetailsToPDI = true;
                        pdiOrder = order;
                        pdiInvoice = invoice;
                    }
                    invoiceModel.Id = invoice.Id;
                    invoiceModel.DisplayInvoiceNumber = invoice.DisplayInvoiceNumber;
                    invoiceModel.InvoiceHeaderId = invoiceHeader.Id;
                    if (firstInvoice == null)
                    {
                        firstInvoice = invoice;
                    }
                }
                if (sendDtnFile)
                {
                    CreateDtnFileGenerationWorkflow(dtnInvoice, dtnOrder);
                }

                var isWaitingForNothing = invoices.All(t => t.WaitingFor == WaitingAction.Nothing || t.WaitingFor == WaitingAction.PDITaxes);
                if (isWaitingForNothing)
                {
                    sapDomain.CreateSAPWorkflow(firstInvoice);
                    if (sendDeliveryDetailsToPDI)
                    {
                        CreatePDIAPIWorkflow(pdiInvoice, pdiOrder);
                    }
                }
                if (firstInvoice != null)
                {
                    CreateQbAccountingWorkflowForInvoice(false, firstInvoice, firstInvoice.Order, null);
                    CreateQbAccountingWorkflowForBill(false, firstInvoice, firstInvoice.Order, null);
                }
                await SaveCompartmentInfoDetails(invoiceViewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateConsolidatedMobileInvoice", ex.Message, ex);
                throw ex;
            }
            return delReqStatuses;
        }

        public async Task SaveCompartmentInfoDetails(InvoiceViewModelNew createInvoiceViewModel)
        {
            try
            {
                var allcompartmentInfo = new List<CompartmentInfoViewModel>();
                var deliveryRequestCompartments = new List<DeliveryRequestCompartmentInfoViewModel>();

                foreach (var bols in createInvoiceViewModel.BolDetails)
                {
                    foreach (var products in bols.Products)
                    {
                        foreach (var item in products.CompartmentInfo)
                        {
                            var compartmentInfo = new CompartmentInfoViewModel();
                            compartmentInfo.CompartmentId = item.CompartmentId;
                            compartmentInfo.TrailerId = item.TrailerId;
                            compartmentInfo.Quantity = item.Quantity;
                            compartmentInfo.TrackableScheduleId = item.TrackableScheduleId;

                            allcompartmentInfo.Add(compartmentInfo);
                        }
                    }
                }

                foreach (var bols in createInvoiceViewModel.TicketDetails)
                {
                    foreach (var products in bols.Products)
                    {
                        foreach (var item in products.CompartmentInfo)
                        {
                            var compartmentInfo = new CompartmentInfoViewModel();
                            compartmentInfo.CompartmentId = item.CompartmentId;
                            compartmentInfo.TrailerId = item.TrailerId;
                            compartmentInfo.Quantity = item.Quantity;
                            compartmentInfo.TrackableScheduleId = item.TrackableScheduleId;

                            allcompartmentInfo.Add(compartmentInfo);
                        }
                    }
                }


                var trackableScheduleIds = allcompartmentInfo.Select(t => t.TrackableScheduleId).Distinct().ToList();
                foreach (var trackableScheduleId in trackableScheduleIds)
                {
                    var compartmentsInfo = new List<CompartmentsInfoViewModel>();
                    var specificScheduleCompartments = allcompartmentInfo.Where(t => t.TrackableScheduleId == trackableScheduleId).ToList();

                    var trackableSchedule = await Context.DataContext.DeliveryScheduleXTrackableSchedules.FirstOrDefaultAsync(t => t.Id == trackableScheduleId);
                    if (trackableSchedule != null && specificScheduleCompartments.Count > 0 && string.IsNullOrEmpty(trackableSchedule.CompartmentInfo))
                    {
                        foreach (var item in specificScheduleCompartments)
                        {
                            compartmentsInfo.Add(new CompartmentsInfoViewModel
                            {
                                CompartmentId = item.CompartmentId,
                                TrailerId = item.TrailerId,
                                Quantity = item.Quantity
                            });
                        }

                        trackableSchedule.CompartmentInfo = JsonConvert.SerializeObject(compartmentsInfo);
                        await Context.CommitAsync();

                        deliveryRequestCompartments.Add(new DeliveryRequestCompartmentInfoViewModel
                        {
                            DeliveryRequestId = trackableSchedule.FrDeliveryRequestId,
                            Compartments = compartmentsInfo
                        });

                        createInvoiceViewModel.DeliveryRequestCompartments = deliveryRequestCompartments;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "SaveCompartmentInfoDetails", ex.Message, ex);
            }
        }

        public async Task<int> SaveDeliveryScheduleAsync(InvoiceModel drop)
        {
            var response = 0;
            HelperDomain helperDomain = new HelperDomain(this);
            OrderDomain orderDomain = new OrderDomain(this);

            try
            {
                if (drop.OrderId.HasValue)
                {
                    var order = await Context.DataContext.Orders.Include(t => t.OrderDeliverySchedules).Include(t => t.FuelRequest).Include(t => t.FuelRequest.Job).SingleOrDefaultAsync(t => t.Id == drop.OrderId.Value);
                    int latestGroupNumber = 0;
                    var deliverySchedules = Context.DataContext.DeliverySchedules;
                    if (deliverySchedules.Any())
                    {
                        latestGroupNumber = deliverySchedules.Max(t => t.GroupId);
                    }

                    var deliverySchedule = new DeliverySchedule();
                    deliverySchedule.Date = drop.DropStartDate.Date;
                    deliverySchedule.StartTime = drop.DropStartDate.TimeOfDay;
                    deliverySchedule.EndTime = drop.DropEndDate.TimeOfDay;
                    deliverySchedule.Quantity = drop.DroppedGallons;
                    deliverySchedule.Type = (int)DeliveryScheduleType.SpecificDates;
                    deliverySchedule.GroupId = ++latestGroupNumber;
                    deliverySchedule.WeekDayId = helperDomain.GetWeekDayId(drop.DropStartDate);
                    deliverySchedule.CreatedBy = drop.CreatedBy;
                    deliverySchedule.StatusId = (int)DeliveryScheduleStatus.New;
                    Context.DataContext.DeliverySchedules.Add(deliverySchedule);
                    await Context.CommitAsync();

                    var latestSchedules = orderDomain.GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                    latestSchedules.ForEach(t => t.IsActive = false);

                    if (latestSchedules.Any())
                    {
                        foreach (var item in latestSchedules)
                        {
                            var schedule = orderDomain.GetOrderDeliverySchedule(drop.OrderId.Value, item.Version, drop.CreatedBy, item.DeliveryRequestId);
                            order.OrderDeliverySchedules.Add(schedule);
                        }
                    }
                    else
                    {
                        var schedule = orderDomain.GetOrderDeliverySchedule(drop.OrderId.Value, 0, drop.CreatedBy, null);
                        order.OrderDeliverySchedules.Add(schedule);
                    }

                    var trackableSchedule = new DeliveryScheduleXTrackableSchedule();
                    trackableSchedule.UoM = order.FuelRequest.Job.UoM;
                    trackableSchedule.Date = deliverySchedule.Date.Date;
                    trackableSchedule.ShiftStartDate = deliverySchedule.Date.Date;
                    trackableSchedule.StartTime = deliverySchedule.StartTime;
                    trackableSchedule.EndTime = deliverySchedule.EndTime;
                    trackableSchedule.Quantity = deliverySchedule.Quantity;
                    trackableSchedule.QuantityTypeId = order.FuelRequest.QuantityTypeId;
                    trackableSchedule.IsActive = true;
                    trackableSchedule.DeliveryScheduleId = deliverySchedule.Id;
                    trackableSchedule.DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.UnplannedDropCompleted;
                    trackableSchedule.DriverId = drop.DriverId;
                    trackableSchedule.DeliveryLevelPO = drop.DeliveryLevelPO;
                    order.DeliveryScheduleXTrackableSchedules.Add(trackableSchedule);

                    await Context.CommitAsync();
                    drop.TrackableScheduleStatusId = trackableSchedule.DeliveryScheduleStatusId;
                    response = trackableSchedule.Id;
                }
            }
            catch (Exception ex)
            {
                response = 0;
                LogManager.Logger.WriteException("ConsolidatedInvoiceDomain", "SaveDeliveryScheduleAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<AssetDrop>> GetMobileAssetDropsAsync(List<int> assetDropIds)
        {
            var assetDrops = new List<AssetDrop>();
            try
            {
                assetDrops = await Context.DataContext.AssetDrops.Where(t => assetDropIds.Contains(t.Id) && t.InvoiceId == null).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateManualInvoiceAsync", ex.Message, ex);
            }
            return assetDrops;
        }

        protected async Task<List<DeliveryReqStatusUpdateModel>> CreateRebillInvoice(List<InvoiceModel> invoices, InvoiceViewModelNew invoiceViewModel, InvoiceNumber invoiceNumber, List<DropAdditionalDetailsModel> otherDetails)
        {
            List<DeliveryReqStatusUpdateModel> delReqStatuses = new List<DeliveryReqStatusUpdateModel>();
            var invoiceHeader = GenerateInvoiceHeader(invoices);
            Order dtnOrder = null; Invoice dtnInvoice = null; bool sendDtnFile = true;
            Order pdiOrder = null; Invoice pdiInvoice = null; bool sendDeliveryDetailsToPDI = false;
            Invoice firstInvoice = null;
            foreach (var invoiceModel in invoices)
            {
                SetInvoiceNumber(invoiceNumber.Number, invoiceModel);
                var invoice = invoiceModel.ToEntity();
                var assetDrops = invoiceModel.AssetDrops.Select(t => t.ToEntity()).ToList();
                invoice.AssetDrops = assetDrops;
                invoiceHeader.Invoices.Add(invoice);
                await Context.CommitAsync();

                if (string.IsNullOrWhiteSpace(invoice.ReferenceId))
                {
                    invoice.TransactionId = invoice.DisplayInvoiceNumber = invoiceModel.DisplayInvoiceNumber = invoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFRB);
                }
                else
                {
                    invoice.ReferenceId = invoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFRB);
                }

                UpdatePreviousInvoiceStatusAsCreditedRebilled(invoiceViewModel.OriginalInvoiceHeaderId.Value, (int)InvoiceStatus.CreditedAndRebilled);
                await SetInvoiceBolDetails(invoiceHeader, invoiceModel, invoice);
                await SaveBulkPlantLocations(invoiceModel.BolDetails);

                var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == invoiceModel.OrderId);
                if (order.SendDtnFile)
                {
                    dtnOrder = order;
                    dtnInvoice = invoice;
                }
                else
                {
                    sendDtnFile = false;
                }
                if (order.OrderAdditionalDetail != null && order.OrderAdditionalDetail.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails)
                {
                    sendDeliveryDetailsToPDI = true;
                    pdiOrder = order;
                    pdiInvoice = invoice;
                }
                var dropAdditionalDetail = otherDetails.FirstOrDefault(t => t.OrderId == invoiceModel.OrderId);
                var delReqStatus = UpdateInvoiceDependentEntitiesPostCreate(invoiceModel, order, invoice, dropAdditionalDetail);
                if (delReqStatus != null)
                {
                    delReqStatuses.Add(delReqStatus);
                }
                await Context.CommitAsync();
                invoiceModel.Id = invoice.Id;
                invoiceModel.InvoiceHeaderId = invoiceHeader.Id;
                if (firstInvoice == null)
                {
                    firstInvoice = invoice;
                }
            }
            var cumulationQtyList = CreateListOfCumulationQuantityUpdateForRebillInv(invoices, otherDetails);
            if (cumulationQtyList != null && cumulationQtyList.Any())
            {
                await new ConsolidatedInvoiceDomain(this).UpdateCumulationQuantitiesPostInvoiceCreate(cumulationQtyList);

            }
            if (sendDtnFile)
            {
                CreateDtnFileGenerationWorkflow(dtnInvoice, dtnOrder);
            }
            if (sendDeliveryDetailsToPDI)
            {
                CreatePDIAPIWorkflow(pdiInvoice, pdiOrder);
            }

            sapDomain.CreateSAPWorkflow(firstInvoice);

            if (firstInvoice != null)
            {
                CreateQbAccountingWorkflowForInvoice(false, firstInvoice, firstInvoice.Order, null);
                CreateQbAccountingWorkflowForBill(false, firstInvoice, firstInvoice.Order, null);
            }
            return delReqStatuses;
        }

        protected async Task SetInvoiceBolDetails(InvoiceHeaderDetail invoiceHeader, InvoiceModel invoiceModel, Invoice invoice)
        {
            await ValidateEBOLDetails(invoiceModel);
            foreach (var bolDetail in invoiceModel.BolDetails)
            {
                var ftlDetail = bolDetail.ToEntity();
                if (bolDetail.Image != null && (!string.IsNullOrWhiteSpace(bolDetail.Image.FilePath)))
                {
                    Image image = new Image() { FilePath = bolDetail.Image.FilePath, Data = bolDetail.Image.Data };
                    ftlDetail.Image = image;
                }
                else if (bolDetail.Image != null && bolDetail.Image.Id > 0)
                {
                    ftlDetail.ImageId = bolDetail.Image.Id;
                }
                var bolEntity = Context.DataContext.InvoiceFtlDetails.Add(ftlDetail);
                InvoiceXBolDetail invoiceXBol = new InvoiceXBolDetail() { InvoiceHeaderId = invoiceHeader.Id, InvoiceId = invoice.Id };
                bolEntity.InvoiceXBolDetails.Add(invoiceXBol);
                await Context.CommitAsync();
            }
        }

        protected async Task SetInvoiceBolDetails(List<BolDetailViewModel> bolDetails, int invoiceHeaderId, int invoiceId)
        {
            foreach (var bolDetail in bolDetails)
            {
                var ftlDetail = bolDetail.ToEntity();
                if (bolDetail.Image != null && (!string.IsNullOrWhiteSpace(bolDetail.Image.FilePath)))
                {
                    Image image = new Image() { FilePath = bolDetail.Image.FilePath, Data = bolDetail.Image.Data };
                    ftlDetail.Image = image;
                }
                else if (bolDetail.Image != null && bolDetail.Image.Id > 0)
                {
                    ftlDetail.ImageId = bolDetail.Image.Id;
                }
                var bolEntity = Context.DataContext.InvoiceFtlDetails.Add(ftlDetail);
                InvoiceXBolDetail invoiceXBol = new InvoiceXBolDetail() { InvoiceHeaderId = invoiceHeaderId, InvoiceId = invoiceId };
                bolEntity.InvoiceXBolDetails.Add(invoiceXBol);
                await Context.CommitAsync();
            }
        }

        protected async Task SaveBulkPlantLocations(List<BolDetailViewModel> ftlDetails)
        {
            foreach (var ftlDetail in ftlDetails)
            {
                await SaveBulkPlantLocation(ftlDetail);
            }
        }

        protected async Task SaveBulkPlantLocation(BolDetailViewModel ftlDetail)
        {
            if (ftlDetail != null && ftlDetail.StateId > 0 && ftlDetail.PickupLocationType == PickupLocationType.BulkPlant)
            {
                var companyId = Context.DataContext.Users.Where(t => t.Id == ftlDetail.CreatedBy && t.CompanyId.HasValue).Select(t => t.CompanyId.Value).FirstOrDefault();
                var countryId = Context.DataContext.MstStates.Where(t => t.Id == ftlDetail.StateId).Select(t => t.CountryId).FirstOrDefault();
                //sending county as NULL in case of lift from canada location in TPD API
                //Code added to avoid DB exception for countyName column in BulkPlantLocationsTable
                if (string.IsNullOrWhiteSpace(ftlDetail.CountyName))
                {
                    ftlDetail.CountyName = ftlDetail.StateCode;
                }
                var bulkPlantDetail = ftlDetail.ToBulkPlantLocationEntity(countryId, companyId);
                var dispatchDomain = new DispatchDomain(this);
                await dispatchDomain.SaveBulkPlantIfNotExists(bulkPlantDetail);
            }
        }

        protected void SetInvoiceBolDetails(InvoiceHeaderDetail invoiceHeader, InvoiceViewModel invoiceModel, Invoice invoice)
        {
            if (invoiceModel.BolDetails != null)
            {
                var ftlDetail = invoiceModel.BolDetails.ToEntity();
                if (invoiceModel.BolDetails.Image != null && (!string.IsNullOrWhiteSpace(invoiceModel.BolDetails.Image.FilePath)))
                {
                    Image image = new Image() { FilePath = invoiceModel.BolDetails.Image.FilePath, Data = invoiceModel.BolDetails.Image.Data };
                    ftlDetail.Image = image;
                }
                var bolEntity = Context.DataContext.InvoiceFtlDetails.Add(ftlDetail);
                InvoiceXBolDetail invoiceXBol = new InvoiceXBolDetail() { InvoiceHeaderId = invoiceHeader.Id, BolDetailId = bolEntity.Id };
                invoice.InvoiceXBolDetails.Add(invoiceXBol);
            }
        }

        protected async Task SetInvoiceBolDetailsForEdit(InvoiceHeaderDetail invoiceHeader, InvoiceModel invoiceModel, Invoice invoice)
        {
            foreach (var bolDetail in invoiceModel.BolDetails)
            {
                var ftlDetail = Context.DataContext.InvoiceFtlDetails.FirstOrDefault(t => t.Id == bolDetail.Id);
                if (ftlDetail != null)
                {
                    if (invoiceModel.BolImage != null && ftlDetail.ImageId == null)
                    {
                        Image image = new Image() { FilePath = invoiceModel.BolImage.FilePath, Data = invoiceModel.BolImage.Data };
                        ftlDetail.Image = image;
                    }

                    InvoiceXBolDetail invoiceXBol = new InvoiceXBolDetail() { InvoiceHeaderId = invoiceHeader.Id, InvoiceId = invoice.Id };
                    ftlDetail.InvoiceXBolDetails.Add(invoiceXBol);
                    await Context.CommitAsync();
                }
            }
        }

        protected async Task SetMobileInvoiceBolDetails(int invoiceHeaderId, InvoiceModel invoiceModel, int invoiceId)
        {
            foreach (var bolDetail in invoiceModel.BolDetails)
            {
                if (bolDetail != null)
                {
                    InvoiceXBolDetail invoiceXBol = new InvoiceXBolDetail() { InvoiceHeaderId = invoiceHeaderId, InvoiceId = invoiceId };
                    InvoiceFtlDetail bolEntity;
                    if (bolDetail.Id > 0)
                    {
                        bolEntity = Context.DataContext.InvoiceFtlDetails.FirstOrDefault(t => t.Id == bolDetail.Id);
                        bolEntity.PricePerGallon = invoiceModel.PricePerGallon;
                        bolEntity.RackPrice = invoiceModel.RackPrice;
                    }
                    else
                    {
                        bolDetail.PricePerGallon = invoiceModel.PricePerGallon;
                        bolDetail.RackPrice = invoiceModel.RackPrice;
                        var ftlDetail = bolDetail.ToEntity();
                        if (bolDetail.Image != null && (!string.IsNullOrWhiteSpace(bolDetail.Image.FilePath)))
                        {
                            Image image = new Image() { FilePath = bolDetail.Image.FilePath, Data = bolDetail.Image.Data };
                            ftlDetail.Image = image;
                        }
                        bolEntity = Context.DataContext.InvoiceFtlDetails.Add(ftlDetail);
                    }
                    bolEntity.InvoiceXBolDetails.Add(invoiceXBol);
                    await Context.CommitAsync();
                }
            }
        }

        protected async Task SetMobileInvoiceBolDetails(int invoiceHeaderId, InvoiceViewModel invoiceModel, int invoiceId)
        {
            var bolDetail = invoiceModel.BolDetails;
            InvoiceXBolDetail invoiceXBol = new InvoiceXBolDetail() { InvoiceHeaderId = invoiceHeaderId, InvoiceId = invoiceId };
            InvoiceFtlDetail bolEntity;
            if (bolDetail.Id > 0)
            {
                bolEntity = Context.DataContext.InvoiceFtlDetails.FirstOrDefault(t => t.Id == bolDetail.Id);
                bolEntity.PricePerGallon = invoiceModel.PricePerGallon;
                bolEntity.RackPrice = invoiceModel.RackPrice;
            }
            else
            {
                bolDetail.PricePerGallon = invoiceModel.PricePerGallon;
                bolDetail.RackPrice = invoiceModel.RackPrice;
                var ftlDetail = bolDetail.ToEntity();
                if (bolDetail.Image != null && (!string.IsNullOrWhiteSpace(bolDetail.Image.FilePath)))
                {
                    Image image = new Image() { FilePath = bolDetail.Image.FilePath, Data = bolDetail.Image.Data };
                    ftlDetail.Image = image;
                }
                bolEntity = Context.DataContext.InvoiceFtlDetails.Add(ftlDetail);
            }
            bolEntity.InvoiceXBolDetails.Add(invoiceXBol);
            await Context.CommitAsync();
        }

        private void SetZeroGallonInvoiceBolDetails(InvoiceHeaderDetail invoiceHeader, InvoiceModel invoiceModel, Invoice invoice, int fuelTypeId)
        {
            InvoiceFtlDetail bolDetail = new InvoiceFtlDetail() { Latitude = 0, Longitude = 0, PickupLocation = PickupLocationType.Terminal, CityGroupTerminalId = invoiceModel.CityGroupTerminalId, TerminalId = invoiceModel.TerminalId, PricePerGallon = 0, RackPrice = 0, IsActive = true, IsDeleted = false, CreatedBy = invoiceModel.CreatedBy, CreatedDate = invoiceModel.CreatedDate, FuelTypeId = fuelTypeId };
            var bolEntity = Context.DataContext.InvoiceFtlDetails.Add(bolDetail);
            InvoiceXBolDetail invoiceXBol = new InvoiceXBolDetail() { InvoiceHeaderId = invoiceHeader.Id, BolDetailId = bolEntity.Id };
            invoice.InvoiceXBolDetails.Add(invoiceXBol);
        }

        private void SetCreditInvoiceBolDetails(InvoiceHeaderDetail invoiceHeader, InvoiceModel viewModel, Invoice invoice)
        {
            if (viewModel.BolDetails != null && viewModel.BolDetails.Any(t => t.Id > 0))
            {
                foreach (var bol in viewModel.BolDetails)
                {
                    InvoiceXBolDetail invoiceXBol = new InvoiceXBolDetail() { InvoiceHeaderId = invoiceHeader.Id, BolDetailId = bol.Id };
                    invoice.InvoiceXBolDetails.Add(invoiceXBol);
                }
            }
        }

        protected async Task<List<InvoiceCreateResponseViewModel>> CreatePartialCreditInvoiceAsync(InvoiceCreateRequestViewModel viewModel, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var response = new List<InvoiceCreateResponseViewModel>();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    InvoiceNumber invoiceNumber = new InvoiceNumber();
                    Context.DataContext.InvoiceNumbers.Add(invoiceNumber);
                    Context.Commit();

                    var invoice = viewModel.InvoiceModel.ToPartialCreditEntity(currentDate);
                    viewModel.InvoiceModel.InvoiceNumberId = invoiceNumber.Id;
                    var invoiceHeader = viewModel.InvoiceModel.ToPartialCreditInvoiceHeaderEntity();
                    Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                    var assetDropViewModel = SetAssetDropsToInvoice(viewModel.JobId, viewModel.JobCompanyId, viewModel.InvoiceModel.CreatedBy, viewModel.InvoiceModel.DriverId, viewModel.InvoiceModel.DropEndDate, manualInvoiceViewModel.Assets);
                    var assetDrops = assetDropViewModel.Select(t => t.ToEntity()).ToList();
                    assetDrops.ForEach(t => t.DroppedGallons *= -1);
                    invoice.AssetDrops = assetDrops;
                    var invoiceResponse = new InvoiceCreateResponseViewModel();
                    if (order != null)
                    {
                        invoice.DisplayInvoiceNumber = invoice.TransactionId = invoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFCI).Replace(ApplicationConstants.SFRB, ApplicationConstants.SFCI);
                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();
                        SetCreditInvoiceBolDetails(invoiceHeader, viewModel.InvoiceModel, invoice);
                        await Context.CommitAsync();
                        UpdateManualInvoiceDependentEntitiesPostPartialCreditCreate(order, invoice, invoiceResponse);
                        await Context.CommitAsync();
                        transaction.Commit();

                        ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
                        invoiceResponse.StatusCode = Status.Success;

                        var cumulationList = CreateListOfCumulationQuantityUpdateForPartialCreditInv(invoice.DroppedGallons, invoice.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId);
                        if (cumulationList != null && cumulationList.Any())
                        {
                            await new ConsolidatedInvoiceDomain(this).UpdateCumulationQuantitiesPostInvoiceCreate(cumulationList);
                        }
                        response.Add(invoiceResponse);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateManualInvoiceAsync", ex.Message, ex);
                }
            }

            return response;
        }

        protected async Task<List<InvoiceCreateResponseViewModel>> CreateManualSplitDraftDdtAsync(InvoiceCreateRequestViewModel viewModel, List<InvoiceCreateRequestViewModel> brokeredViewModels, ManualInvoiceViewModel manualInvoiceViewModel)

        {
            var response = new List<InvoiceCreateResponseViewModel>();
            DeliveryReqStatusUpdateModel deliveryReqStatus = null;
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var invoiceHeader = viewModel.InvoiceModel.ToInvoiceHeaderEntity();

            var invoice = viewModel.InvoiceModel.ToEntity();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var assetDropViewModel = SetAssetDropsToInvoice(viewModel.JobId, viewModel.JobCompanyId, viewModel.InvoiceModel.CreatedBy, viewModel.InvoiceModel.DriverId, viewModel.InvoiceModel.DropEndDate, manualInvoiceViewModel.Assets);
                    var assetDrops = assetDropViewModel.Select(t => t.ToEntity()).ToList();
                    invoice.AssetDrops = assetDrops;

                    var invoiceResponse = new InvoiceCreateResponseViewModel();
                    if (order != null)
                    {
                        Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        order.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        await SetInvoiceBolDetails(invoiceHeader, viewModel.InvoiceModel, invoice);
                        await Context.CommitAsync();
                        await SaveBulkPlantLocations(viewModel.InvoiceModel.BolDetails);
                        UpdateBolDetails(viewModel.InvoiceModel, invoice);
                        deliveryReqStatus = UpdateManualInvoiceDependentEntitiesPostCreate(viewModel, order, invoice, invoiceResponse);

                        var assetDropModel = assetDrops.Select(t => t.ToViewModel()).ToList();
                        var brokersResponse = await CreateManualInvoicesForBrokers(brokeredViewModels, assetDropModel, invoice.ImageId, invoice.SignatureId);

                        await Context.CommitAsync();
                        transaction.Commit();

                        ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
                        invoiceResponse.StatusCode = Status.Success;

                        response.Add(invoiceResponse);
                        response.AddRange(brokersResponse);
                        if (deliveryReqStatus != null)
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateManualSplitDraftDdtAsync", ex.Message, ex);
                }
            }

            return response;
        }

        protected async Task<List<InvoiceCreateResponseViewModel>> CreateDraftDdtInvoiceAsync(InvoiceCreateRequestViewModel viewModel, List<InvoiceCreateRequestViewModel> brokeredViewModels, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var response = new List<InvoiceCreateResponseViewModel>();
            DeliveryReqStatusUpdateModel deliveryReqStatus = null;
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);

            var invoiceHeader = viewModel.InvoiceModel.ToInvoiceHeaderEntity();
            var invoice = viewModel.InvoiceModel.ToEntity();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var assetDropViewModel = SetAssetDropsToInvoice(viewModel.JobId, viewModel.JobCompanyId, viewModel.InvoiceModel.CreatedBy, viewModel.InvoiceModel.DriverId, viewModel.InvoiceModel.DropEndDate, manualInvoiceViewModel.Assets);
                    var assetDrops = assetDropViewModel.Select(t => t.ToEntity()).ToList();
                    invoice.AssetDrops = assetDrops;

                    var invoiceResponse = new InvoiceCreateResponseViewModel();
                    if (order != null)
                    {
                        Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        await SetInvoiceBolDetails(invoiceHeader, viewModel.InvoiceModel, invoice);
                        UpdatePreviousInvoiceVersionStatus(manualInvoiceViewModel.InvoiceId, (int)InvoiceVersionStatus.InActive);
                        await Context.CommitAsync();
                        await SaveBulkPlantLocations(viewModel.InvoiceModel.BolDetails);

                        deliveryReqStatus = UpdateManualInvoiceDependentEntitiesPostCreate(viewModel, order, invoice, invoiceResponse);

                        var assetDropModel = assetDrops.Select(t => t.ToViewModel()).ToList();
                        var brokersResponse = await CreateManualInvoicesForBrokers(brokeredViewModels, assetDropModel, invoice.ImageId, invoice.SignatureId);

                        await Context.CommitAsync();
                        transaction.Commit();

                        ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
                        invoiceResponse.StatusCode = Status.Success;

                        response.Add(invoiceResponse);
                        response.AddRange(brokersResponse);
                        if (deliveryReqStatus != null)
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateDraftDdtInvoiceAsync", ex.Message, ex);
                }
            }

            return response;
        }

        protected async Task<List<InvoiceCreateResponseViewModel>> CreateBuySellInvoiceForMobileAsync(InvoiceCreateRequestViewModel sellInvoiceViewModel, InvoiceCreateRequestViewModel buyInvoiceViewModel, List<AssetDrop> mobileAssetDrops)
        {
            var response = new List<InvoiceCreateResponseViewModel>();
            DeliveryReqStatusUpdateModel deliveryReqStatus = null;
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == sellInvoiceViewModel.OrderId);

            var invoice = sellInvoiceViewModel.InvoiceModel.ToEntity();
            invoice.AssetDrops = mobileAssetDrops;

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var invoiceResponse = new InvoiceCreateResponseViewModel();
                    if (order != null)
                    {
                        order.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        deliveryReqStatus = await UpdateMobileInvoiceDependentEntitiesPostCreate(sellInvoiceViewModel, order, invoice, invoiceResponse);

                        var delReqStatus = await CreateBuyInvoice(buyInvoiceViewModel, order, invoice.ImageId, invoice.SignatureId, invoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail).FirstOrDefault());
                        await Context.CommitAsync();
                        transaction.Commit();

                        ConstructInvoiceCreateResponseViewModel(sellInvoiceViewModel, invoiceResponse, order, invoice);
                        invoiceResponse.StatusCode = Status.Success;

                        response.Add(invoiceResponse);
                        if (deliveryReqStatus != null)
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                        }
                        if (delReqStatus != null)
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { delReqStatus });
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateBuySellInvoiceForMobileAsync", ex.Message, ex);
                }
            }

            return response;
        }

        protected async Task<List<InvoiceCreateResponseViewModel>> CreateBuySellInvoiceAsync(InvoiceCreateRequestViewModel viewModel, InvoiceCreateRequestViewModel buyInvoiceViewModel, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var response = new List<InvoiceCreateResponseViewModel>();
            DeliveryReqStatusUpdateModel deliveryReqStatus = null;
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);

            var invoice = viewModel.InvoiceModel.ToEntity();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var assetDropViewModel = SetAssetDropsToInvoice(viewModel.JobId, viewModel.JobCompanyId, viewModel.InvoiceModel.CreatedBy, viewModel.InvoiceModel.DriverId, viewModel.InvoiceModel.DropEndDate, manualInvoiceViewModel.Assets);
                    var assetDrops = assetDropViewModel.Select(t => t.ToEntity()).ToList();
                    invoice.AssetDrops = assetDrops;
                    buyInvoiceViewModel.InvoiceModel.AssetDrops = assetDrops.Select(t => t.ToViewModel()).ToList();

                    var invoiceResponse = new InvoiceCreateResponseViewModel();
                    if (order != null)
                    {
                        order.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        deliveryReqStatus = UpdateManualInvoiceDependentEntitiesPostCreate(viewModel, order, invoice, invoiceResponse);
                        var delReqStatus = await CreateBuyInvoice(buyInvoiceViewModel, order, invoice.ImageId, invoice.SignatureId, invoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail).FirstOrDefault());

                        await Context.CommitAsync();
                        transaction.Commit();

                        ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
                        invoiceResponse.StatusCode = Status.Success;

                        response.Add(invoiceResponse);
                        if (deliveryReqStatus != null)
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                        }
                        if (delReqStatus != null)
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { delReqStatus });
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateBuySellInvoiceAsync", ex.Message, ex);
                }
            }

            return response;
        }

        private async Task<DeliveryReqStatusUpdateModel> CreateBuyInvoice(InvoiceCreateRequestViewModel viewModel, Order order, int? imageId, int? signatureId, InvoiceFtlDetail invoiceFtlDetail)
        {
            DeliveryReqStatusUpdateModel delReqStatus = null;
            try
            {
                // Prevent duplicate image save in database
                viewModel.InvoiceModel.Image.Id = imageId ?? 0;
                if (invoiceFtlDetail != null && invoiceFtlDetail.ImageId.HasValue)
                {
                    viewModel.InvoiceModel.BolImage = new ImageViewModel();
                    viewModel.InvoiceModel.BolImage.Id = invoiceFtlDetail.ImageId.Value;
                }
                viewModel.InvoiceModel.SignatureId = signatureId;

                var invoice = viewModel.InvoiceModel.ToEntity();
                order.Invoices.Add(invoice);
                await Context.CommitAsync();

                //post invcoice creation updating dependent entities
                delReqStatus = UpdateTrackableScheduleStatus(viewModel.CurrentTrackableScheduleId, viewModel.CurrentTrackableScheduleStatusId, invoice);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateBuyerInvoice", ex.Message, ex);
            }
            return delReqStatus;
        }

        private async Task<List<InvoiceCreateResponseViewModel>> CreateMobileInvoicesForBrokers(List<InvoiceCreateRequestViewModel> brokeredViewModels, int? imageId, int? signatureId)
        {
            var response = new List<InvoiceCreateResponseViewModel>();
            DeliveryReqStatusUpdateModel deliveryReqStatus = null;
            try
            {
                foreach (var viewModel in brokeredViewModels)
                {
                    // Prevent duplicate image save in database
                    if (imageId > 0)
                    {
                        if (viewModel.InvoiceModel.Image == null)
                        {
                            viewModel.InvoiceModel.Image = new ImageViewModel();
                        }
                        viewModel.InvoiceModel.Image.Id = imageId ?? 0;
                    }
                    viewModel.InvoiceModel.SignatureId = signatureId;

                    var invoiceResponse = new InvoiceCreateResponseViewModel();
                    var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);

                    var invoiceHeader = viewModel.InvoiceModel.ToInvoiceHeaderEntity();
                    var invoice = viewModel.InvoiceModel.ToEntity();
                    if (order != null)
                    {
                        Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();
                        await SetMobileInvoiceBolDetails(invoiceHeader.Id, viewModel.InvoiceModel, invoice.Id);
                        await SaveBulkPlantLocations(viewModel.InvoiceModel.BolDetails);

                        deliveryReqStatus = await UpdateMobileInvoiceDependentEntitiesPostCreate(viewModel, order, invoice, invoiceResponse);
                        ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);

                        invoiceResponse.StatusCode = Status.Success;
                        response.Add(invoiceResponse);
                        if (deliveryReqStatus != null)
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateMobileInvoicesForBrokers", ex.Message, ex);
            }
            return response;
        }

        protected async Task<List<InvoiceCreateResponseViewModel>> CreateManualInvoicesForBrokers(List<InvoiceCreateRequestViewModel> brokeredViewModels, List<AssetDropModel> assetDrops, int? imageId, int? signImageId, int? bolImgId = null, int? additionalImgId = null, int? taxAffidavitImgId = null, int? coastGuardInspectionImgId = null, int? inspectionRequestVoucherImgId = null)
        {
            var response = new List<InvoiceCreateResponseViewModel>();
            DeliveryReqStatusUpdateModel deliveryReqStatus = null;
            try
            {
                foreach (var viewModel in brokeredViewModels)
                {
                    // Prevent duplicate image save in database
                    viewModel.InvoiceModel.Image.Id = imageId ?? 0;
                    viewModel.InvoiceModel.SignatureId = signImageId;
                    viewModel.InvoiceModel.BolImage.Id = bolImgId ?? 0;
                    viewModel.InvoiceModel.AdditionalImage.Id = additionalImgId ?? 0;
                    viewModel.InvoiceModel.TaxAffidavitImage.Id = taxAffidavitImgId ?? 0;
                    viewModel.InvoiceModel.CoastGuardInspectionImage.Id = coastGuardInspectionImgId ?? 0;
                    viewModel.InvoiceModel.InspectionRequestVoucherImage.Id = inspectionRequestVoucherImgId ?? 0;
                    assetDrops.ForEach(t => { t.OrderId = viewModel.OrderId; });
                    viewModel.InvoiceModel.AssetDrops = assetDrops;

                    var invoiceResponse = new InvoiceCreateResponseViewModel();
                    var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
                    var invoice = viewModel.InvoiceModel.ToEntity();

                    if (order != null)
                    {
                        var invoiceHeader = viewModel.InvoiceModel.ToInvoiceHeaderEntity();
                        Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        order.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        await SetInvoiceBolDetails(invoiceHeader, viewModel.InvoiceModel, invoice);
                        await Context.CommitAsync();
                        await SaveBulkPlantLocations(viewModel.InvoiceModel.BolDetails);

                        deliveryReqStatus = UpdateManualInvoiceDependentEntitiesPostCreate(viewModel, order, invoice, invoiceResponse);
                        ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);

                        invoiceResponse.StatusCode = Status.Success;
                        response.Add(invoiceResponse);
                        if (deliveryReqStatus != null)
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateManualInvoicesForBrokers", ex.Message, ex);
            }
            return response;
        }

        protected async Task<InvoiceEditResponseViewModel> CreateRebilledInvoiceAsync(InvoiceEditRequestViewModel viewModel, bool isFromQueueService = false)
        {
            var response = new InvoiceEditResponseViewModel();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            viewModel.InvoiceModel.CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName);
            viewModel.InvoiceModel.Version = 1;
            var invoice = viewModel.InvoiceModel.ToEntity();

            // update rebill specific values
            invoice.InvoiceXAdditionalDetail.OriginalInvoiceId = viewModel.InvoiceId;
            InvoiceNumber invoiceNumber = new InvoiceNumber();
            Context.DataContext.InvoiceNumbers.Add(invoiceNumber);
            Context.Commit();

            invoice.InvoiceHeader.InvoiceNumber = invoiceNumber;
            invoice.TransactionId = invoice.DisplayInvoiceNumber = invoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFRB);

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (order != null)
                    {
                        order.Invoices.Add(invoice);
                        var headerId = Context.DataContext.Invoices.Where(t => t.Id == viewModel.InvoiceId).Select(t => t.InvoiceHeaderId).FirstOrDefault();
                        UpdatePreviousInvoiceStatusAsCreditedRebilled(headerId, (int)InvoiceStatus.CreditedAndRebilled);
                        await Context.CommitAsync();
                        if (!isFromQueueService)
                        {
                            UpdateBolDetails(viewModel.InvoiceModel, invoice);
                        }
                        UpdateInvoiceDependentEntitiesPostRebillInvoiceCreation(viewModel, response, order, invoice);
                        var isCreditAndRebilledGenerated = order.Invoices.Count(t => t.InvoiceXAdditionalDetail.OriginalInvoiceId == invoice.ParentId) == 2;
                        if (!string.IsNullOrEmpty(invoice.InvoiceXAdditionalDetail.SplitLoadChainId) && isCreditAndRebilledGenerated)
                        {
                            await CheckForSplitLoadInvoiceAndGenerateStatement(invoice.InvoiceXAdditionalDetail.SplitLoadChainId, viewModel.SupplierCompanyId, viewModel.TimeZoneName);
                        }
                        await Context.CommitAsync();
                        transaction.Commit();

                        ConstructInvoiceEditResponseViewModel(viewModel, response, order, invoice);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "UpdateInvoiceAsync", ex.Message, ex);
                }
            }
            return response;
        }

        protected async Task<InvoiceEditResponseViewModel> UpdateInvoiceForOtherDetailsAsync(InvoiceEditRequestViewModel viewModel)
        {
            var response = new InvoiceEditResponseViewModel();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var invoiceHeader = viewModel.InvoiceModel.ToInvoiceHeaderEntity();
            var invoice = viewModel.InvoiceModel.ToEntity();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (order != null)
                    {
                        Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        await SetInvoiceBolDetailsForEdit(invoiceHeader, viewModel.InvoiceModel, invoice);
                        await Context.CommitAsync();
                        await SaveBulkPlantLocations(viewModel.InvoiceModel.BolDetails);

                        UpdatePreviousInvoiceVersionStatus(viewModel.InvoiceId, (int)InvoiceVersionStatus.InActive);
                        await Context.CommitAsync();

                        UpdateInvoiceDealStatus(invoice.Id, viewModel.DiscountId, viewModel.InvoiceModel.UpdatedBy, viewModel.InvoiceModel.UpdatedByCompanyId);
                        var delReqStatuses = UpdateInvoiceDependentEntitiesPostUpdate(viewModel, response, order, invoice);

                        await Context.CommitAsync();
                        transaction.Commit();
                        if (delReqStatuses != null && delReqStatuses.Any())
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatuses);
                        }
                        ConstructInvoiceEditResponseViewModel(viewModel, response, order, invoice);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "UpdateInvoiceForOtherDetailsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        protected async Task<StatusViewModel> UpdateInvoiceForEditNumberAsync(ConsolidatedInvoiceEditViewModel invoiceEditViewModel, int? orderId)
        {
            var response = new StatusViewModel();
            var invoices = invoiceEditViewModel.invoiceModels;
            var existingInvoiceHeader = await Context.DataContext.InvoiceHeaderDetails.Where(t => t.Id == invoiceEditViewModel.InvoiceHeader.Id).FirstOrDefaultAsync();
            var newInvoiceHeader = invoiceEditViewModel.InvoiceHeader.ToEntity();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    UpdateExistingInvoiceHeader(existingInvoiceHeader, newInvoiceHeader);
                    newInvoiceHeader.Version += 1;

                    Context.DataContext.InvoiceHeaderDetails.Add(newInvoiceHeader);
                    await Context.CommitAsync();
                    Order dtnOrder = null; Invoice dtnInvoice = null; bool sendDtnFile = true;
                    Invoice firstInvoice = null;
                    foreach (var invoiceModel in invoices)
                    {
                        var invoice = invoiceModel.ToEntity();
                        var assetDrops = invoiceModel.AssetDrops.Select(t => t.ToEntity()).ToList();
                        invoice.AssetDrops = assetDrops;
                        newInvoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();
                        await SetInvoiceBolDetailsForEdit(newInvoiceHeader, invoiceModel, invoice);

                        UpdatePreviousInvoiceVersionStatus(invoiceModel.Id, (int)InvoiceVersionStatus.InActive);
                        await Context.CommitAsync();
                        invoiceModel.Id = invoice.Id;
                        var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == invoice.OrderId);
                        if (order.SendDtnFile)
                        {
                            dtnOrder = order;
                            dtnInvoice = invoice;
                        }
                        else
                        {
                            sendDtnFile = false;
                        }
                        if (firstInvoice == null)
                        {
                            firstInvoice = invoice;
                        }
                    }

                    if (firstInvoice != null)
                    {
                        CreateQbAccountingWorkflowForInvoice(true, firstInvoice, firstInvoice.Order, null);
                        CreateQbAccountingWorkflowForBill(true, firstInvoice, firstInvoice.Order, null);
                    }
                    if (sendDtnFile)
                    {
                        CreateDtnFileGenerationWorkflow(dtnInvoice, dtnOrder);
                    }
                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.EntityId = invoices.Where(t => t.OrderId == orderId).Select(t => t.Id).FirstOrDefault();
                    response.EntityNumber = invoices.Where(t => t.OrderId == orderId).Select(t => t.DisplayInvoiceNumber).FirstOrDefault();
                    response.EntityHeaderId = newInvoiceHeader.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "UpdateInvoiceForEditNumberAsync", ex.Message, ex);
                }
                return response;
            }

        }

        protected void UpdateExistingInvoiceHeader(InvoiceHeaderDetail existingHeaderDetails, InvoiceHeaderDetail newInvoiceHeader)
        {
            existingHeaderDetails.IsActive = false;
            existingHeaderDetails.UpdatedBy = newInvoiceHeader.UpdatedBy;
            existingHeaderDetails.UpdatedDate = newInvoiceHeader.UpdatedDate;
            // Inactive assest drop of previous version invoice
            InActivePreviousInvoiceVersionAssestDropByHeaderId(existingHeaderDetails.Id);
            Context.DataContext.Entry(existingHeaderDetails).State = EntityState.Modified;
        }

        public async Task UpdateExistingInvoiceHeader(List<int> headerIds, InvoiceHeaderDetail newInvoiceHeader)
        {
            foreach (var headerId in headerIds)
            {
                var existingHeaderDetails = await Context.DataContext.InvoiceHeaderDetails.FirstOrDefaultAsync(t => t.Id == headerId);
                if (existingHeaderDetails != null)
                {
                    existingHeaderDetails.IsActive = false;
                    existingHeaderDetails.UpdatedBy = newInvoiceHeader.UpdatedBy;
                    existingHeaderDetails.UpdatedDate = newInvoiceHeader.UpdatedDate;
                    // Inactive assest drop of previous version invoice
                    InActivePreviousInvoiceVersionAssestDropByHeaderId(existingHeaderDetails.Id);
                    Context.DataContext.Entry(existingHeaderDetails).State = EntityState.Modified;
                }
            }
        }

        protected async Task<List<InvoiceEditResponseViewModel>> UpdateConsolidatedInvoiceDealAsync(List<InvoiceEditRequestViewModel> consolidatedViewModel)
        {
            var response = new List<InvoiceEditResponseViewModel>();
            List<DeliveryReqStatusUpdateModel> delReqStatuses = new List<DeliveryReqStatusUpdateModel>();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var invoiceNumberId = consolidatedViewModel.First().InvoiceModel.InvoiceNumberId;
                    var oldInvoiceHeader = Context.DataContext.InvoiceHeaderDetails.FirstOrDefault(t => t.InvoiceNumberId == invoiceNumberId && t.IsActive);
                    if (oldInvoiceHeader != null)
                    {
                        oldInvoiceHeader.IsActive = false;
                        // Inactive assest drop of previous version invoice
                        InActivePreviousInvoiceVersionAssestDropByHeaderId(oldInvoiceHeader.Id);
                        consolidatedViewModel.ForEach(t => t.InvoiceHeaderVersion = oldInvoiceHeader.Version);
                        await Context.CommitAsync();
                    }

                    var invoiceHeader = consolidatedViewModel.ToConsolidatedInvoiceHeaderEntity();
                    Context.DataContext.InvoiceHeaderDetails.Add(invoiceHeader);
                    Order dtnOrder = null; Invoice dtnInvoice = null; bool sendDtnFile = true;
                    Invoice firstInvoice = null;
                    foreach (var viewModel in consolidatedViewModel)
                    {
                        var invoiceEditResponse = new InvoiceEditResponseViewModel();

                        var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
                        var invoice = viewModel.InvoiceModel.ToEntity();

                        if (order != null)
                        {
                            invoiceHeader.Invoices.Add(invoice);
                            await Context.CommitAsync();

                            await SetInvoiceBolDetailsForEdit(invoiceHeader, viewModel.InvoiceModel, invoice);
                            await Context.CommitAsync();

                            UpdatePreviousInvoiceVersionStatus(viewModel.InvoiceId, (int)InvoiceVersionStatus.InActive);
                            await Context.CommitAsync();

                            UpdateInvoiceDealStatus(invoice.Id, viewModel.DiscountId, viewModel.InvoiceModel.UpdatedBy, viewModel.InvoiceModel.UpdatedByCompanyId);
                            delReqStatuses = UpdateConsolidatedInvoiceDependentEntitiesPostUpdate(viewModel, invoiceEditResponse, order, invoice);

                            await Context.CommitAsync();
                            if (order.SendDtnFile)
                            {
                                dtnOrder = order;
                                dtnInvoice = invoice;
                            }
                            else
                            {
                                sendDtnFile = false;
                            }
                            if (firstInvoice == null)
                            {
                                firstInvoice = invoice;
                            }
                            ConstructInvoiceEditResponseViewModel(viewModel, invoiceEditResponse, order, invoice);

                            response.Add(invoiceEditResponse);
                        }
                    }
                    if (firstInvoice != null)
                    {
                        CreateQbAccountingWorkflowForInvoice(true, firstInvoice, firstInvoice.Order, null);
                        CreateQbAccountingWorkflowForBill(true, firstInvoice, firstInvoice.Order, null);
                    }
                    if (sendDtnFile)
                    {
                        CreateDtnFileGenerationWorkflow(dtnInvoice, dtnOrder);
                    }
                    transaction.Commit();
                    if (delReqStatuses != null && delReqStatuses.Any())
                    {
                        new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatuses);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "UpdateConsolidatedInvoiceForOtherDetailsAsync", ex.Message, ex);
                }
            }

            return response;
        }

        protected async Task<InvoiceEditResponseViewModel> UpdateInvoiceAsync(InvoiceEditRequestViewModel viewModel)
        {
            var response = new InvoiceEditResponseViewModel();
            var existingDetails = Context.DataContext.Invoices.Where(t => t.Id == viewModel.InvoiceId).Select(t => new { t.InvoiceHeader, t.Order }).FirstOrDefault();
            var order = existingDetails?.Order;
            var newHeaderEntity = viewModel.InvoiceModel.ToInvoiceHeaderEntity();
            newHeaderEntity.UpdatedBy = viewModel.InvoiceModel.UpdatedBy;
            newHeaderEntity.UpdatedDate = viewModel.InvoiceModel.UpdatedDate;

            newHeaderEntity.Version = existingDetails != null ? existingDetails.InvoiceHeader.Version + 1 : 1;
            if(existingDetails!=null) existingDetails.InvoiceHeader.IsActive = false;
            var invoice = viewModel.InvoiceModel.ToEntity();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (order != null)
                    {
                        Context.DataContext.InvoiceHeaderDetails.Add(newHeaderEntity);
                        newHeaderEntity.Invoices.Add(invoice);
                        await Context.CommitAsync();
                        await SetInvoiceBolDetails(newHeaderEntity, viewModel.InvoiceModel, invoice);
                        // Inactive assest drop of previous version invoice
                        if (existingDetails != null && existingDetails.InvoiceHeader != null)
                            InActivePreviousInvoiceVersionAssestDropByHeaderId(existingDetails.InvoiceHeader.Id);
                        UpdatePreviousInvoiceVersionStatus(viewModel.InvoiceId, (int)InvoiceVersionStatus.InActive);
                        await Context.CommitAsync();
                        await SaveBulkPlantLocations(viewModel.InvoiceModel.BolDetails);

                        UpdateInvoiceDealStatus(invoice.Id, viewModel.DiscountId, viewModel.InvoiceModel.UpdatedBy, viewModel.InvoiceModel.UpdatedByCompanyId);
                        var delReqStatuses = UpdateInvoiceDependentEntitiesPostUpdate(viewModel, response, order, invoice);

                        await Context.CommitAsync();
                        transaction.Commit();
                        if (delReqStatuses != null && delReqStatuses.Any())
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatuses);
                        }

                        var cumulationQtyList = CreateListOfCumulationQuantityUpdateForInvEdit(viewModel.OriginalDroppedGallons, order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId, invoice.DroppedGallons);

                        if (cumulationQtyList != null && cumulationQtyList.Any())
                        {
                            await new ConsolidatedInvoiceDomain(this).UpdateCumulationQuantitiesPostInvoiceCreate(cumulationQtyList);
                        }
                        ConstructInvoiceEditResponseViewModel(viewModel, response, order, invoice);

                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "UpdateInvoiceAsync", ex.Message, ex);
                }
            }
            return response;
        }

        protected async Task<InvoiceEditResponseViewModel> UpdateSplitLoadInvoiceAsync(InvoiceEditRequestViewModel viewModel)
        {
            var response = new InvoiceEditResponseViewModel();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var invoice = viewModel.InvoiceModel.ToEntity();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (order != null)
                    {
                        order.Invoices.Add(invoice);
                        UpdatePreviousInvoiceVersionStatus(viewModel.InvoiceId, (int)InvoiceVersionStatus.InActive);
                        await Context.CommitAsync();
                        UpdateBolDetails(viewModel.InvoiceModel, invoice);
                        var delReqStatuses = UpdateInvoiceDependentEntitiesPostUpdate(viewModel, response, order, invoice);

                        await Context.CommitAsync();
                        transaction.Commit();
                        if (delReqStatuses != null && delReqStatuses.Any())
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatuses);
                        }
                        ConstructInvoiceEditResponseViewModel(viewModel, response, order, invoice);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "UpdateInvoiceAsync", ex.Message, ex);
                }
            }
            return response;
        }

        protected async Task<InvoiceEditResponseViewModel> UpdateSplitLoadDraftDdtAsync(InvoiceEditRequestViewModel viewModel, List<Invoice> draftDdts)
        {
            var response = new InvoiceEditResponseViewModel();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var invoice = viewModel.InvoiceModel.ToEntity();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (order != null)
                    {
                        order.Invoices.Add(invoice);
                        UpdatePreviousInvoiceVersionStatus(viewModel.InvoiceId, (int)InvoiceVersionStatus.InActive);
                        await Context.CommitAsync();
                        UpdateBolDetails(viewModel.InvoiceModel, invoice);
                        var delReqStatuses = UpdateInvoiceDependentEntitiesPostUpdate(viewModel, response, order, invoice);
                        if (draftDdts != null)
                        {
                            UpdateHeaderDetailsForDdtsInChain(draftDdts, viewModel);
                        }
                        await Context.CommitAsync();
                        transaction.Commit();
                        if (delReqStatuses != null && delReqStatuses.Any())
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatuses);
                        }
                        ConstructInvoiceEditResponseViewModel(viewModel, response, order, invoice);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "UpdateSplitLoadDraftDdtAsync", ex.Message, ex);
                }
            }
            return response;
        }

        protected void UpdateBolDetails(InvoiceModel viewModel, Invoice invoice)
        {
            if (viewModel.BolDetails.Any(t => t.Id > 0))
            {
                var bolInfo = viewModel.BolDetails.FirstOrDefault(t => t.Id > 0);
                var bolDetail = Context.DataContext.InvoiceFtlDetails.FirstOrDefault(t => t.Id == bolInfo.Id);
                if (bolDetail != null)
                {
                    var boldetailEntity = viewModel.ToBolEntity(bolDetail);
                    if (boldetailEntity != null && boldetailEntity.Id == 0)
                    {
                        invoice.InvoiceXBolDetails.Clear();
                        Context.DataContext.InvoiceFtlDetails.Add(boldetailEntity);
                        InvoiceXBolDetail invoiceXBolDetail = new InvoiceXBolDetail() { InvoiceHeaderId = invoice.InvoiceHeaderId, InvoiceId = invoice.Id };
                        boldetailEntity.InvoiceXBolDetails.Add(invoiceXBolDetail);
                        var splitInvoices = Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == viewModel.AdditionalDetail.SplitLoadChainId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToList();
                        foreach (var splitinvoice in splitInvoices)
                        {
                            splitinvoice.InvoiceXBolDetails.Clear();
                            InvoiceXBolDetail invoiceBolDetail = new InvoiceXBolDetail() { InvoiceHeaderId = splitinvoice.InvoiceHeaderId, InvoiceId = splitinvoice.Id };
                            boldetailEntity.InvoiceXBolDetails.Add(invoiceBolDetail);
                        }

                        Context.Commit();
                    }
                }
            }
        }

        protected void UpdateBolDetails(InvoiceViewModel viewModel, Order order, Invoice invoice)
        {
            if (viewModel.BolDetails.Id > 0)
            {
                var bolDetail = Context.DataContext.InvoiceFtlDetails.FirstOrDefault(t => t.Id == viewModel.BolDetails.Id);
                if (bolDetail != null)
                {
                    var boldetailEntity = viewModel.ToBolEntity(bolDetail);
                    if (boldetailEntity != null && boldetailEntity.Id == 0)
                    {
                        invoice.InvoiceXBolDetails.Clear();
                        Context.DataContext.InvoiceFtlDetails.Add(boldetailEntity);
                        InvoiceXBolDetail invoiceXBolDetail = new InvoiceXBolDetail() { InvoiceHeaderId = invoice.InvoiceHeaderId, InvoiceId = invoice.Id };
                        boldetailEntity.InvoiceXBolDetails.Add(invoiceXBolDetail);
                        var splitInvoices = Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == viewModel.AdditionalDetail.SplitLoadChainId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToList();
                        foreach (var splitinvoice in splitInvoices)
                        {
                            splitinvoice.InvoiceXBolDetails.Clear();
                            InvoiceXBolDetail invoiceBolDetail = new InvoiceXBolDetail() { InvoiceHeaderId = splitinvoice.InvoiceHeaderId, InvoiceId = splitinvoice.Id };
                            boldetailEntity.InvoiceXBolDetails.Add(invoiceBolDetail);
                        }

                        UpdateApprovalWorkflowForSplitInvoices(viewModel, order, invoice);

                        Context.Commit();
                    }
                }
            }
        }

        public void UpdateApprovalWorkflowForSplitInvoices(InvoiceViewModel viewModel, Order order, Invoice invoice)
        {
            var splitInvoices = Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == viewModel.AdditionalDetail.SplitLoadChainId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToList();
            foreach (var splitinvoice in splitInvoices)
            {
                bool isInvoiceImgExists = !splitinvoice.IsDropImageReq || splitinvoice.ImageId != null;
                bool isBolImgExists = !splitinvoice.IsBolImageReq || (invoice.InvoiceXBolDetails.Any(t => t.InvoiceFtlDetail != null && t.InvoiceFtlDetail.ImageId != null) || !string.IsNullOrWhiteSpace(viewModel.BolImage?.FilePath));
                bool isSignExists = !splitinvoice.IsSignatureReq || (splitinvoice.Signaure != null && splitinvoice.Signaure.ImageId != null);
                if (isBolImgExists && isInvoiceImgExists && isSignExists)
                {
                    if (order.FuelRequest.Job.IsApprovalWorkflowEnabled)
                        splitinvoice.WaitingFor = (int)WaitingAction.CustomerApproval;
                    else
                        splitinvoice.WaitingFor = (int)WaitingAction.Nothing;
                }
                else
                {
                    splitinvoice.WaitingFor = (int)WaitingAction.Images;
                }

                InvoiceXInvoiceStatusDetail invoiceStatus = splitinvoice.InvoiceXInvoiceStatusDetails.FirstOrDefault(t => t.IsActive);
                int statusId = (splitinvoice.WaitingFor == (int)WaitingAction.CustomerApproval) ? (int)InvoiceStatus.WaitingForApproval : (splitinvoice.WaitingFor == (int)WaitingAction.Images ? (int)InvoiceStatus.Received : invoiceStatus.StatusId);

                if (statusId != invoiceStatus.StatusId)
                {
                    invoiceStatus.IsActive = false;
                    InvoiceXInvoiceStatusDetail statusDetail = new InvoiceXInvoiceStatusDetail()
                    {
                        InvoiceId = splitinvoice.Id,
                        StatusId = statusId,
                        IsActive = true,
                        UpdatedBy = viewModel.UpdatedBy,
                        UpdatedDate = DateTimeOffset.Now
                    };
                    Context.DataContext.InvoiceXInvoiceStatusDetails.Add(statusDetail);
                }
            }
        }

        public void UpdateApprovalWorkflowForSplitInvoices(InvoiceModel viewModel, Order order, Invoice invoice)
        {
            var splitInvoices = Context.DataContext.Invoices.Where(t => t.InvoiceXAdditionalDetail.SplitLoadChainId == viewModel.AdditionalDetail.SplitLoadChainId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).ToList();
            foreach (var splitinvoice in splitInvoices)
            {
                bool isInvoiceImgExists = !splitinvoice.IsDropImageReq || splitinvoice.ImageId != null;
                bool isBolImgExists = !splitinvoice.IsBolImageReq || (invoice.InvoiceXBolDetails.Any(t => t.InvoiceFtlDetail != null && t.InvoiceFtlDetail.ImageId != null) || !string.IsNullOrWhiteSpace(viewModel.BolImage?.FilePath));
                bool isSignExists = !splitinvoice.IsSignatureReq || (splitinvoice.Signaure != null && splitinvoice.Signaure.ImageId != null);
                if (isBolImgExists && isInvoiceImgExists && isSignExists)
                {
                    if (order.FuelRequest.Job.IsApprovalWorkflowEnabled)
                        splitinvoice.WaitingFor = (int)WaitingAction.CustomerApproval;
                    else
                        splitinvoice.WaitingFor = (int)WaitingAction.Nothing;
                }
                else
                {
                    splitinvoice.WaitingFor = (int)WaitingAction.Images;
                }

                InvoiceXInvoiceStatusDetail invoiceStatus = splitinvoice.InvoiceXInvoiceStatusDetails.FirstOrDefault(t => t.IsActive);
                int statusId = (splitinvoice.WaitingFor == (int)WaitingAction.CustomerApproval) ? (int)InvoiceStatus.WaitingForApproval : (splitinvoice.WaitingFor == (int)WaitingAction.Images ? (int)InvoiceStatus.Received : invoiceStatus.StatusId);

                if (statusId != invoiceStatus.StatusId)
                {
                    invoiceStatus.IsActive = false;
                    InvoiceXInvoiceStatusDetail statusDetail = new InvoiceXInvoiceStatusDetail()
                    {
                        InvoiceId = splitinvoice.Id,
                        StatusId = statusId,
                        IsActive = true,
                        UpdatedBy = viewModel.UpdatedBy,
                        UpdatedDate = DateTimeOffset.Now
                    };
                    Context.DataContext.InvoiceXInvoiceStatusDetails.Add(statusDetail);
                }
            }
        }

        protected bool AutoCloseOrderPostSave(Order order, int? currentTrackableScheduleId, out decimal totalDelivered)
        {
            var response = false; totalDelivered = 0;
            var orderCurrentStatus = order.OrderXStatuses.FirstOrDefault(t => t.IsActive);
            if (orderCurrentStatus != null && orderCurrentStatus.StatusId == (int)OrderStatus.Open && order.FuelRequest.MaxQuantity > 0)
            {
                if (order.FuelRequest.UoM == UoM.MetricTons || order.FuelRequest.UoM == UoM.Barrels)
                {
                    totalDelivered = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.ConvertedQuantity ?? 0);
                }
                else
                {
                    totalDelivered = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons);
                }
                response = PerformOrderAutoClose(order, totalDelivered, currentTrackableScheduleId);
            }
            return response;
        }

        protected bool AutoCloseOrder(Order order, out decimal totalDelivered, int? currentTrackableScheduleId = null)
        {
            var response = false;
            if (order.FuelRequest.UoM == UoM.MetricTons || order.FuelRequest.UoM == UoM.Barrels)
            {
                totalDelivered = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.ConvertedQuantity ?? 0);
            }
            else
            {
                totalDelivered = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons);
            }
            response = PerformOrderAutoClose(order, totalDelivered, currentTrackableScheduleId);
            return response;
        }

        protected void SetHedgeSpotAmounts(Order order, Invoice invoice, bool isInvoiceEdit, bool isRecursiveCallFromBrokered)
        {
            if (IsDigitalDropTicket(invoice.InvoiceTypeId))
            {
                SetHedgeSpotAmountsForDDT(order, invoice, isInvoiceEdit);
            }
            else
            {
                SetHedgeSpotAmountsForInvoice(order, invoice, isInvoiceEdit);
            }
        }

        protected void CreateQbAccountingWorkflowForInvoice(bool isInvoiceEdit, Invoice invoice, Order order, int? brokeredOrderId)
        {
            var shouldCreateInvoiceWorkflow = IsInvoiceValidForInvoiceWorkflow(invoice);
            if (shouldCreateInvoiceWorkflow)
            {
                var qbWorkflowDomain = new QbWorkflowDomain(this);
                var userContext = new UserContext() { CompanyId = order.AcceptedCompanyId };
                if (isInvoiceEdit)
                {
                    qbWorkflowDomain.CreateInvoiceWorkflow(userContext, order, invoice, AccountingWorkflowType.InvoiceModify);
                    qbWorkflowDomain.CreateInvoicePoWorkflow(userContext, order, invoice, brokeredOrderId, AccountingWorkflowType.POModify);
                }
                else
                {
                    qbWorkflowDomain.CreateInvoiceWorkflow(userContext, order, invoice, AccountingWorkflowType.InvoiceAdd);
                    qbWorkflowDomain.CreateInvoicePoWorkflow(userContext, order, invoice, brokeredOrderId, AccountingWorkflowType.InvoicePoAdd);
                }
            }
        }

        protected void CreateQbAccountingWorkflowForBill(bool isInvoiceEdit, Invoice invoice, Order order, int? brokeredOrderId)
        {
            var shouldCreateBillWorkflow = IsInvoiceValidForInvoiceWorkflow(invoice);
            if (shouldCreateBillWorkflow)
            {
                var qbWorkflowDomain = new QbWorkflowDomain(this);
                var userContext = new UserContext() { CompanyId = order.AcceptedCompanyId };
                if (isInvoiceEdit)
                {
                    qbWorkflowDomain.CreateBillWorkflow(userContext, order, invoice, brokeredOrderId, AccountingWorkflowType.BillModify);
                }
                else
                {
                    qbWorkflowDomain.CreateBillWorkflow(userContext, order, invoice, brokeredOrderId, AccountingWorkflowType.BillAdd);
                }
            }
        }

        public void CreateQbAccountingWorkflowForCreditInvoice(Invoice invoice, Order order)
        {
            var shouldCreateBillWorkflow = IsInvoiceValidForInvoiceWorkflow(invoice);
            if (shouldCreateBillWorkflow)
            {
                var qbWorkflowDomain = new QbWorkflowDomain(this);
                var userContext = new UserContext() { CompanyId = order.AcceptedCompanyId };
                qbWorkflowDomain.CreateCreditMemoWorkflow(userContext, order, invoice, AccountingWorkflowType.CreditMemoAdd);
                userContext = new UserContext() { CompanyId = order.BuyerCompanyId };
                qbWorkflowDomain.CreateVendorCreditWorkflow(userContext, order, invoice, AccountingWorkflowType.VendorCreditAdd);
            }
        }

        protected DateTimeOffset GetPaymentDueDate(int paymentTermId, int netDays, string timeZoneName, DateTimeOffset dropEndDate, PaymentDueDateType paymentDueDateType, DateTimeOffset? dateTime = null)
        {
            var paymentDueDate = dateTime ?? DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
            if (paymentTermId == (int)PaymentTerms.NetDays)
            {
                if (paymentDueDateType == PaymentDueDateType.InvoiceCreationDate)
                    paymentDueDate = paymentDueDate.Date.AddDays(netDays);
                else if (paymentDueDateType == PaymentDueDateType.DeliveryDate)
                    paymentDueDate = dropEndDate.Date.AddDays(netDays);
            }
            else if (paymentTermId == (int)PaymentTerms.Net30)
            {
                if (paymentDueDateType == PaymentDueDateType.InvoiceCreationDate)
                    paymentDueDate = paymentDueDate.Date.AddDays(30);
                else if (paymentDueDateType == PaymentDueDateType.DeliveryDate)
                    paymentDueDate = dropEndDate.Date.AddDays(30);
            }
            return paymentDueDate;
        }

        protected async Task<InvoiceNumberViewModel> GenerateInvoiceNumber()
        {
            var invoiceNumberViewModel = new InvoiceNumberViewModel();
            var invoiceNumber = invoiceNumberViewModel.ToEntity();
            Context.DataContext.InvoiceNumbers.Add(invoiceNumber);
            await Context.CommitAsync();
            invoiceNumberViewModel = invoiceNumber.ToViewModel();
            return invoiceNumberViewModel;
        }

        protected async Task<InvoiceNumber> GenerateInvoiceNumber_New(int existingHeaderId = 0)
        {
            if (existingHeaderId > 0)
            {
                var invoicenumber = await Context.DataContext.InvoiceHeaderDetails.Where(t => t.Id == existingHeaderId)
                                            .Select(t => t.InvoiceNumber).FirstOrDefaultAsync();
                if (invoicenumber != null)
                    return invoicenumber;
            }

            var invoiceNumberViewModel = new InvoiceNumberViewModel();
            var invoiceNumber = invoiceNumberViewModel.ToEntity();
            Context.DataContext.InvoiceNumbers.Add(invoiceNumber);
            await Context.CommitAsync();

            return invoiceNumber;
        }

        protected List<AssetDropModel> SetAssetDropsToInvoice(int jobId, int jobCompanyId, int userId, int? driverId, DateTimeOffset endDate, List<AssetDropViewModel> assets, bool isMarineJob = false)
        {
            List<AssetDropModel> assetDrops = new List<AssetDropModel>();
            if (assets.Any())
            {
                var assetViewModel = SetJobAssetId(assets, userId, jobId, jobCompanyId, endDate, isMarineJob);
                var assetDropsWithAdditionDetail = GetAssetDropsWithAdditionalInformation(assetViewModel, driverId ?? userId, userId, endDate, isMarineJob);
                assetDrops = assetDropsWithAdditionDetail.Select(t => t.ToAssetDropModel(endDate.Offset)).ToList();
            }
            return assetDrops;
        }

        private List<AssetDropViewModel> SetJobAssetId(List<AssetDropViewModel> assetDrops, int userId, int jobId, int jobCompanyId, DateTimeOffset endDate, bool isMarineJob)
        {
            var unSavedAssets = assetDrops.Where(t => t.JobXAssetId == 0).ToList();
            var assetNames = unSavedAssets.Select(x => x.AssetName).ToArray();
            int assetId = 0;
            Asset asset = null;
            var assetMappings = Context.DataContext.Assets.Where(x => (x.CompanyId == jobCompanyId && assetNames.Contains(x.Name)) || (x.CompanyId == ApplicationConstants.SuperAdminCompanyId && assetNames.Contains(x.Name) && x.Type == (int)AssetType.Vessle && x.IsActive)).
                    Select(x => new
                    {
                        x.Id,
                        x.Name,
                        JobXAsset = x.JobXAssets.Where(t => t.JobId == jobId).OrderByDescending(t => t.Id).FirstOrDefault(),
                        AssignedToAnotherJob = x.JobXAssets.FirstOrDefault(t => t.JobId != jobId && t.RemovedBy == null && t.RemovedDate == null)
                    }
                    ).ToList();

            foreach (var newAsset in unSavedAssets)
            {
                var dropStartDate = endDate.Date.Add(Convert.ToDateTime(newAsset.StartTime).TimeOfDay);
                var dropEndDate = endDate.Date.Add(Convert.ToDateTime(newAsset.EndTime).TimeOfDay);
                var assignToJob = false;
                var assetMapping = assetMappings.FirstOrDefault(x => x.Name.Equals(newAsset.AssetName, StringComparison.InvariantCultureIgnoreCase));
                if (assetMapping != null && assetMapping.Id != 0) //existing asset
                {
                    assetId = assetMapping.Id;
                    var jobXasset = assetMapping.JobXAsset;
                    if (jobXasset != null) //assigned/removed to current job atlease one time
                    {
                        newAsset.JobXAssetId = jobXasset.Id;
                    }
                    else
                    {
                        var assignedToAnotherJob = assetMapping.AssignedToAnotherJob;
                        if (assignedToAnotherJob != null) // assigned to another job and never assigned to current job
                        {
                            if (assignedToAnotherJob.AssignedDate >= dropEndDate)
                            {
                                JobXAsset jobXnAsset = new JobXAsset() { JobId = jobId, AssetId = assetMapping.Id, AssignedBy = userId, AssignedDate = dropStartDate, RemovedBy = userId, RemovedDate = dropEndDate };
                                Context.DataContext.JobXAssets.Add(jobXnAsset);
                                Context.Commit();
                                newAsset.JobXAssetId = jobXnAsset.Id;
                            }
                            else
                            {
                                assignedToAnotherJob.RemovedBy = userId;
                                assignedToAnotherJob.RemovedDate = DateTimeOffset.Now;
                                assignToJob = true;
                            }
                        }
                        else // not assigned to any job
                        {
                            assignToJob = true;
                        }
                    }
                }
                else //new asset
                {
                    asset = new Asset()
                    {
                        Name = newAsset.AssetName,
                        UpdatedBy = userId,
                        CompanyId = jobCompanyId,
                        IsActive = true,
                        CreatedDate = dropStartDate,
                        UpdatedDate = dropStartDate,
                        Type = (int)AssetType.Asset,
                        IsMarine = isMarineJob,
                        AssetAdditionalDetail = new AssetAdditionalDetail() { IsActive = true, UpdatedBy = userId, UpdatedDate = dropStartDate }
                    };
                    if (isMarineJob)
                    {
                        asset.FuelType = (int)ProductTypes.Marine;
                    }
                    Context.DataContext.Assets.Add(asset);
                    assignToJob = true;
                }
                if (assignToJob)
                {
                    JobXAsset jobXnAsset;
                    if (asset != null)
                    {
                        jobXnAsset = new JobXAsset() { JobId = jobId, AssignedBy = userId, AssignedDate = dropStartDate };
                        asset.JobXAssets.Add(jobXnAsset);
                    }
                    else
                    {
                        jobXnAsset = new JobXAsset() { AssetId = assetId, JobId = jobId, AssignedBy = userId, AssignedDate = dropStartDate };
                        Context.DataContext.JobXAssets.Add(jobXnAsset);
                    }

                    //set jobbudget asset tracking true
                    var job = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.Id, JobBudgetId = t.JobBudget.Id, t.JobBudget.IsAssetTracked }).SingleOrDefault();
                    if (job != null && !job.IsAssetTracked)
                    {
                        Context.DataContext.Database
                                   .ExecuteSqlCommand("UPDATE JobBudgets SET IsAssetTracked=1 WHERE Id={0}", job.JobBudgetId);
                        Context.Commit();
                    }


                    Context.Commit();
                    newAsset.JobXAssetId = jobXnAsset.Id;
                }
            }
            return assetDrops;
        }

        protected List<AssetDropViewModel> GetAssetDropsWithAdditionalInformation(List<AssetDropViewModel> jobAssets, int droppedByUserId, int updatedByUserId, DateTimeOffset dropEndDate, bool isMarineJob = false)
        {
            var response = new List<AssetDropViewModel>();
            var jobxAssetIds = jobAssets.Select(t => t.JobXAssetId).ToList();
            var subcontractors = GetSubcontractors(jobxAssetIds);

            foreach (var asset in jobAssets)
            {
                if (asset.DropGallons > 0 || (asset.DropGallons == 0 &&
                    (asset.DropStatusId == (int)DropStatus.NoFuelNeeded || asset.DropStatusId == (int)DropStatus.AssetNotAvailable)))
                {
                    asset.DropDate = asset.DropDate;
                    asset.DropEndDate = dropEndDate;
                    asset.DroppedBy = droppedByUserId;
                    asset.UpdatedBy = updatedByUserId;
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

                    response.Add(asset);
                }
            }
            return response;
        }

        private List<SubcontractorListViewModel> GetSubcontractors(List<int> jobxAssetIds)
        {
            var subcontractors = (from jAsset in Context.DataContext.JobXAssets
                                  join a in Context.DataContext.Assets on jAsset.AssetId equals a.Id
                                  join aSub in Context.DataContext.AssetSubcontractors on a.Id equals aSub.AssetId into subContr
                                  from subContractor in subContr.DefaultIfEmpty()
                                  join s in Context.DataContext.Subcontractors on subContractor.SubcontractorId equals s.Id into contr
                                  from contractor in contr.DefaultIfEmpty()
                                  join ac in Context.DataContext.AssetContractNumbers on a.Id equals ac.AssetId into contractNum
                                  from contractNumber in contractNum.DefaultIfEmpty()
                                  where jobxAssetIds.Contains(jAsset.Id)
                                  select new SubcontractorListViewModel
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
            return subcontractors;
        }

        private async Task<DeliveryReqStatusUpdateModel> UpdateMobileInvoiceDependentEntitiesPostCreate(InvoiceCreateRequestViewModel viewModel, Order order, Invoice invoice, InvoiceCreateResponseViewModel response)
        {
            var driverId = viewModel.InvoiceModel.DriverId ?? 0;
            await UpdateMobileFuelSpills(viewModel.OrderId, invoice.Id, driverId);
            await DriverCompletedOrder(driverId, viewModel.FCMAppId, viewModel.OrderId, viewModel.CurrentTrackableScheduleId);
            var delReqStatusUpdateModel = UpdateInvoiceDependentEntitiesPostCreate(viewModel, order, invoice, response);
            return delReqStatusUpdateModel;
        }

        private DeliveryReqStatusUpdateModel UpdateInvoiceDependentEntitiesPostCreate(InvoiceCreateRequestViewModel viewModel, Order order, Invoice invoice, InvoiceCreateResponseViewModel response)
        {
            var delReqStatusUpdateModel = UpdateTrackableScheduleStatus(viewModel.CurrentTrackableScheduleId, viewModel.CurrentTrackableScheduleStatusId, invoice);
            SetHedgeSpotAmounts(order, invoice, false, false);

            response.IsOrderAutoClosed = AutoCloseOrderPostSave(order, viewModel.CurrentTrackableScheduleId, out decimal orderTotalDelivered);
            response.OrderTotalDelivered = orderTotalDelivered;
            sapDomain.CreateSAPWorkflow(invoice);
            CreatePDIAPIWorkflow(invoice, order);
            CreateQbAccountingWorkflowForInvoice(false, invoice, order, null);
            CreateQbAccountingWorkflowForBill(false, invoice, order, null);
            response.IsDtnUploaded = CreateDtnFileGenerationWorkflow(invoice, order);
            return delReqStatusUpdateModel;
        }

        private DeliveryReqStatusUpdateModel UpdateManualInvoiceDependentEntitiesPostCreate(InvoiceCreateRequestViewModel viewModel, Order order, Invoice invoice, InvoiceCreateResponseViewModel response)
        {
            var delReqStatus = UpdateTrackableScheduleStatus(viewModel.CurrentTrackableScheduleId, viewModel.CurrentTrackableScheduleStatusId, invoice);
            SetHedgeSpotAmounts(order, invoice, false, false);

            response.IsOrderAutoClosed = AutoCloseOrderPostSave(order, viewModel.CurrentTrackableScheduleId, out decimal orderTotalDelivered);
            response.OrderTotalDelivered = orderTotalDelivered;
            if (order.FuelRequest.MaxQuantity > 0)
            {
                decimal maxQuantity = order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity;
                response.DropPercentPerDelivery = orderTotalDelivered / maxQuantity * 100;
            }
            sapDomain.CreateSAPWorkflow(invoice);
            CreatePDIAPIWorkflow(invoice, order);
            CreateQbAccountingWorkflowForInvoice(false, invoice, order, null);
            CreateQbAccountingWorkflowForBill(false, invoice, order, null);
            response.IsDtnUploaded = CreateDtnFileGenerationWorkflow(invoice, order);
            return delReqStatus;
        }

        private DeliveryReqStatusUpdateModel UpdateInvoiceDependentEntitiesPostCreate(InvoiceModel viewModel, Order order, Invoice invoice, DropAdditionalDetailsModel response, bool isMissingDeliveryDDTConversion = false)
        {
            DeliveryReqStatusUpdateModel delReqStatus;
            if (isMissingDeliveryDDTConversion && viewModel.IsReassignDifferentJob)
                delReqStatus = UpdateTrackableScheduleStatusForMissingDelivery(viewModel.TrackableScheduleId, invoice, viewModel.DeliveryLevelPO);
            else
                delReqStatus = UpdateTrackableScheduleStatus(viewModel.TrackableScheduleId, viewModel.TrackableScheduleStatusId ?? 0, invoice, viewModel.DeliveryLevelPO);

            SetHedgeSpotAmounts(order, invoice, false, false);

            response.IsOrderAutoClosed = AutoCloseOrderPostSave(order, viewModel.TrackableScheduleId, out decimal orderTotalDelivered);
            response.OrderTotalDelivered += orderTotalDelivered;
            if (order.FuelRequest.MaxQuantity > 0)
            {
                decimal maxQuantity = order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity;
                response.DropPercentPerDelivery += orderTotalDelivered / maxQuantity * 100;
            }
            return delReqStatus;
        }

        private void UpdateInvoiceDependentEntitiesPostRebillInvoiceCreation(InvoiceEditRequestViewModel viewModel, InvoiceEditResponseViewModel response, Order order, Invoice invoice)
        {
            UpdateTrackableScheduleStatus(viewModel.PreviousTrackableScheduleId, viewModel.PreviousTrackableScheduleStatusId, null);
            UpdateTrackableScheduleStatus(viewModel.CurrentTrackableScheduleId, viewModel.CurrentTrackableScheduleStatusId, invoice);
            SetHedgeSpotAmounts(order, invoice, true, false);
            if (viewModel.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
            {
                response.IsOrderAutoClosed = AutoCloseOrderPostSave(order, viewModel.CurrentTrackableScheduleId, out decimal orderTotalDelivered);
                response.OrderTotalDelivered = orderTotalDelivered;
            }
            sapDomain.CreateSAPWorkflow(invoice);
            CreatePDIAPIWorkflow(invoice, order);
            CreateQbAccountingWorkflowForInvoice(false, invoice, order, null);
            CreateQbAccountingWorkflowForBill(false, invoice, order, null);
            response.IsDtnUploaded = CreateDtnFileGenerationWorkflow(invoice, order);
        }

        private void UpdateManualInvoiceDependentEntitiesPostPartialCreditCreate(Order order, Invoice invoice, InvoiceCreateResponseViewModel response)
        {
            AutoOpenOrder(order, order.FuelRequest, order.FuelRequest.FuelRequestDetail.DeliveryTypeId);
            SetHedgeSpotAmounts(order, invoice, false, false);
            CreateQbAccountingWorkflowForCreditInvoice(invoice, order);
            response.IsDtnUploaded = CreateDtnFileGenerationWorkflow(invoice, order);
        }

        private List<DeliveryReqStatusUpdateModel> UpdateInvoiceDependentEntitiesPostUpdate(InvoiceEditRequestViewModel viewModel, InvoiceEditResponseViewModel response, Order order, Invoice invoice)
        {
            List<DeliveryReqStatusUpdateModel> delReqStatuses = new List<DeliveryReqStatusUpdateModel>();
            var delReqStatus = UpdateTrackableScheduleStatus(viewModel.PreviousTrackableScheduleId, viewModel.PreviousTrackableScheduleStatusId, null);
            if (delReqStatus != null)
            {
                delReqStatuses.Add(delReqStatus);
            }
            var deliveryReqStatus = UpdateTrackableScheduleStatus(viewModel.CurrentTrackableScheduleId, viewModel.CurrentTrackableScheduleStatusId, invoice, viewModel.DeliveryLevelPO);
            if (deliveryReqStatus != null)
            {
                delReqStatuses.Add(deliveryReqStatus);
            }
            SetHedgeSpotAmounts(order, invoice, true, false);
            if (viewModel.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
            {
                response.IsOrderAutoClosed = AutoCloseOrderPostSave(order, viewModel.CurrentTrackableScheduleId, out decimal orderTotalDelivered);
                response.OrderTotalDelivered = orderTotalDelivered;
            }
            sapDomain.CreateSAPWorkflow(invoice);
            CreatePDIAPIWorkflow(invoice, order);
            CreateQbAccountingWorkflowForInvoice(true, invoice, order, null);
            CreateQbAccountingWorkflowForBill(true, invoice, order, null);
            response.IsDtnUploaded = CreateDtnFileGenerationWorkflow(invoice, order);
            return delReqStatuses;
        }

        private List<DeliveryReqStatusUpdateModel> UpdateConsolidatedInvoiceDependentEntitiesPostUpdate(InvoiceEditRequestViewModel viewModel, InvoiceEditResponseViewModel response, Order order, Invoice invoice)
        {
            List<DeliveryReqStatusUpdateModel> delReqStatuses = new List<DeliveryReqStatusUpdateModel>();
            var delReqStatus = UpdateTrackableScheduleStatus(viewModel.PreviousTrackableScheduleId, viewModel.PreviousTrackableScheduleStatusId, null);
            if (delReqStatus != null)
            {
                delReqStatuses.Add(delReqStatus);
            }
            var deliveryReqStatus = UpdateTrackableScheduleStatus(viewModel.CurrentTrackableScheduleId, viewModel.CurrentTrackableScheduleStatusId, invoice);
            if (deliveryReqStatus != null)
            {
                delReqStatuses.Add(deliveryReqStatus);
            }
            SetHedgeSpotAmounts(order, invoice, true, false);
            if (viewModel.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
            {
                response.IsOrderAutoClosed = AutoCloseOrderPostSave(order, viewModel.CurrentTrackableScheduleId, out decimal orderTotalDelivered);
                response.OrderTotalDelivered = orderTotalDelivered;
            }
            return delReqStatuses;
        }

        private void SetHedgeSpotAmountsForDDT(Order order, Invoice invoice, bool isInvoiceEdit)
        {
            // Set hedge and spot quantity and amount in FR and Job table
            decimal previousDroppedGallons = 0, previousBaseDroppedQuantity = 0;
            if (isInvoiceEdit)
            {
                var previousInvoice = order.Invoices.Where(t => t.InvoiceHeader.InvoiceNumberId == invoice.InvoiceHeader.InvoiceNumberId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive).OrderByDescending(t => t.Id).FirstOrDefault();
                if (previousInvoice != null)
                {
                    previousDroppedGallons = previousInvoice.DroppedGallons;
                    previousBaseDroppedQuantity = previousInvoice.BaseDroppedQuntity;
                }
            }
            SetHedgeSpotQuantity(order, invoice, previousDroppedGallons, previousBaseDroppedQuantity);
        }

        private void SetHedgeSpotAmountsForInvoice(Order order, Invoice invoice, bool isInvoiceEdit)
        {
            // Set hedge and spot quantity and amount in FR and Job table
            decimal previousDroppedGallons = 0, previousDroppedAmount = 0;
            decimal previousBaseDroppedQuantity = 0, previousBaseDroppedAmount = 0;
            if (isInvoiceEdit)
            {
                var previousInvoice = order.Invoices.Where(t => t.InvoiceHeader.InvoiceNumberId == invoice.InvoiceHeader.InvoiceNumberId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.InActive).OrderByDescending(t => t.Id).FirstOrDefault();
                if (previousInvoice != null)
                {
                    previousDroppedGallons = previousInvoice.DroppedGallons;
                    previousBaseDroppedQuantity = previousInvoice.BaseDroppedQuntity;
                    var fees = previousInvoice.TotalFeeAmount ?? 0;
                    if (!IsDigitalDropTicket(previousInvoice.InvoiceTypeId))
                    {
                        previousDroppedAmount = previousInvoice.BasicAmount + fees + previousInvoice.TotalTaxAmount - previousInvoice.TotalDiscountAmount;
                        previousBaseDroppedAmount = previousInvoice.BaseBasicAmount + previousInvoice.BaseTotalTaxAmount
                            + MoneyConverter.GetBaseAmount(invoice.Currency, fees, invoice.ExchangeRate)
                            - MoneyConverter.GetBaseAmount(invoice.Currency, previousInvoice.TotalDiscountAmount, invoice.ExchangeRate);
                    }
                }
            }

            decimal amountWithTaxes = 0;
            if (invoice.InvoiceTypeId == (int)InvoiceType.DryRun)
            {
                amountWithTaxes = invoice.BasicAmount;
            }
            else
            {
                var fees = invoice.TotalFeeAmount ?? 0;
                amountWithTaxes = invoice.BasicAmount + fees + invoice.TotalTaxAmount - invoice.TotalDiscountAmount;
            }

            bool isUpdateTotalBudget = true;
            if (isInvoiceEdit && invoice.InvoiceTypeId == (int)InvoiceType.DryRun)
            {
                isUpdateTotalBudget = false;
            }

            if (isUpdateTotalBudget)
            {
                SetHedgeSpotQuantity(order, invoice, previousDroppedGallons, previousBaseDroppedQuantity);
                SetHedgeSpotAmount(order, invoice, previousDroppedAmount, previousBaseDroppedAmount, amountWithTaxes);
            }
        }

        private static void SetHedgeSpotQuantity(Order order, Invoice invoice, decimal previousDroppedGallons, decimal previousBaseDroppedQuantity)
        {
            var quantityDifference = invoice.DroppedGallons - previousDroppedGallons;
            var invoiceBaseQuantity = VolumeConverter.GetBaseQuantity(invoice.UoM, invoice.DroppedGallons);
            var baseQuantityDifference = (invoiceBaseQuantity - previousBaseDroppedQuantity);
            var orderTypeId = order.FuelRequest.OrderTypeId;
            if (orderTypeId == (int)OrderType.Hedge)
            {
                order.FuelRequest.HedgeDroppedGallons += quantityDifference;
                order.FuelRequest.BaseHedgeDroppedQuantity += baseQuantityDifference;
                if (order.FuelRequest.Job != null && order.FuelRequest.Job.CompanyId == order.BuyerCompanyId)
                {
                    order.FuelRequest.Job.HedgeDroppedGallons += quantityDifference;
                    order.FuelRequest.Job.BaseHedgeDroppedQuantity += baseQuantityDifference;
                }
            }
            else
            {
                order.FuelRequest.SpotDroppedGallons += quantityDifference;
                order.FuelRequest.BaseSpotDroppedQuantity += baseQuantityDifference;
                if (order.FuelRequest.Job != null && order.FuelRequest.Job.CompanyId == order.BuyerCompanyId)
                {
                    order.FuelRequest.Job.SpotDroppedGallons += quantityDifference;
                    order.FuelRequest.Job.BaseSpotDroppedQuantity += baseQuantityDifference;
                }
            }
        }

        public static decimal GetFuelSurchageFrieghtFee(decimal fuelSurchargeStartPercentage, decimal surchargeFreightCost, decimal? fuelDropped)
        {
            decimal surchargeRate = (fuelSurchargeStartPercentage / 100) * surchargeFreightCost;
            return Math.Round(surchargeRate.GetPreciseValue(4) * fuelDropped ?? 0, ApplicationConstants.InvoiceFuelSurchargeDecimalDisplay);
        }

        private static void SetHedgeSpotAmount(Order order, Invoice invoice, decimal previousDroppedAmount, decimal previousBaseDroppedAmount, decimal amountWithTaxes)
        {
            var amountDifference = (amountWithTaxes - previousDroppedAmount);
            var invoiceBaseAmount = MoneyConverter.GetBaseAmount(invoice.Currency, amountWithTaxes, invoice.ExchangeRate);
            var baseAmountDifference = (invoiceBaseAmount - previousBaseDroppedAmount);
            var orderTypeId = order.FuelRequest.OrderTypeId;
            if (orderTypeId == (int)OrderType.Hedge)
            {
                order.FuelRequest.HedgeDroppedAmount += amountDifference;
                order.FuelRequest.BaseHedgeDroppedAmount += baseAmountDifference;
                if (order.FuelRequest.Job != null && order.FuelRequest.Job.CompanyId == order.BuyerCompanyId)
                {
                    order.FuelRequest.Job.HedgeDroppedAmount += amountDifference;
                    order.FuelRequest.Job.BaseHedgeDroppedAmount += baseAmountDifference;
                }
            }
            else
            {
                order.FuelRequest.SpotDroppedAmount += amountDifference;
                order.FuelRequest.BaseSpotDroppedAmount += baseAmountDifference;
                if (order.FuelRequest.Job != null && order.FuelRequest.Job.CompanyId == order.BuyerCompanyId)
                {
                    order.FuelRequest.Job.SpotDroppedAmount += amountDifference;
                    order.FuelRequest.Job.BaseSpotDroppedAmount += baseAmountDifference;
                }
            }
        }

        private static void ConstructInvoiceEditResponseViewModel(InvoiceEditRequestViewModel viewModel, InvoiceEditResponseViewModel response, Order order, Invoice invoice)
        {
            response.JobId = viewModel.JobId;
            response.InvoiceId = invoice.Id;
            response.InvoiceHeaderId = invoice.InvoiceHeaderId;
            response.OrderId = order.Id;
            response.InvoiceNumber = invoice.DisplayInvoiceNumber;
            response.InvoiceTypeId = viewModel.InvoiceModel.InvoiceTypeId;
            response.BuyerCompanyId = order.BuyerCompanyId;
            response.SupplierCompanyId = order.AcceptedCompanyId;
            response.DropStartDate = viewModel.InvoiceModel.DropStartDate;
            response.DropEndDate = viewModel.InvoiceModel.DropEndDate;
            response.JobCompanyId = viewModel.JobCompanyId;
            response.SplitLoadChainId = invoice.InvoiceXAdditionalDetail?.SplitLoadChainId;
            response.SplitLoadSequence = invoice.InvoiceXAdditionalDetail?.SplitLoadSequence;
            response.DealCreatedInvoiceId = viewModel.DiscountId > 0 ? invoice.Id : 0;
            response.TimeZoneName = viewModel.TimeZoneName;
            response.InvoiceNumberId = viewModel.InvoiceModel.InvoiceNumberId;
            response.StatusCode = Status.Success;
            response.StatusMessage = Status.Success.ToString();
        }

        private static void ConstructInvoiceCreateResponseViewModel(InvoiceCreateRequestViewModel viewModel, InvoiceCreateResponseViewModel response, Order order, Invoice invoice)
        {
            response.JobId = viewModel.JobId;
            response.InvoiceId = invoice.Id;
            response.InvoiceNumber = invoice.DisplayInvoiceNumber;
            response.InvoiceTypeId = viewModel.InvoiceModel.InvoiceTypeId;
            response.SupplierPrefferedInvoiceTypeId = invoice.SupplierPreferredInvoiceTypeId;
            response.BuyerCompanyId = order.BuyerCompanyId;
            response.SupplierCompanyId = order.AcceptedCompanyId;
            response.DropStartDate = viewModel.InvoiceModel.DropStartDate;
            response.DropEndDate = viewModel.InvoiceModel.DropEndDate;
            response.DriverId = viewModel.InvoiceModel.DriverId;
            response.UserId = viewModel.InvoiceModel.CreatedBy;
            response.OrderId = viewModel.InvoiceModel.OrderId ?? 0;
            response.PoNumber = viewModel.InvoiceModel.PoNumber;
            response.WaitingFor = viewModel.InvoiceModel.WaitingFor;
            response.OrderAcceptedBy = viewModel.OrderAcceptedBy;
            response.InvoiceHeaderId = invoice.InvoiceHeaderId;
            response.SplitLoadChainId = invoice.InvoiceXAdditionalDetail.SplitLoadChainId;
            response.SplitLoadSequence = invoice.InvoiceXAdditionalDetail.SplitLoadSequence;
            response.BolDetailId = invoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail).FirstOrDefault()?.Id;
            response.InvoiceHeaderId = invoice.InvoiceHeaderId;
            if (invoice.InvoiceXAdditionalDetail.OriginalInvoiceId != null)
            {
                response.OriginalInvoiceNumber = invoice.InvoiceXAdditionalDetail.OriginalInvoice.DisplayInvoiceNumber;
            }
            response.StatusCode = Status.Success;
        }

        private void UpdatePreviousInvoiceVersionStatus(int invoiceId, int versionStatusId)
        {
            var previousInvoice = Context.DataContext.Invoices.FirstOrDefault(t => t.Id == invoiceId);
            if (previousInvoice != null)
            {
                previousInvoice.InvoiceVersionStatusId = versionStatusId;
            }
        }

        private void UpdatePreviousInvoiceStatusAsCreditedRebilled(int invoiceHeaderId, int statusId)
        {
            var previousInvoiceStatus = Context.DataContext.InvoiceHeaderDetails.Where(t => t.Id == invoiceHeaderId).SelectMany(t => t.Invoices.SelectMany(t1 => t1.InvoiceXInvoiceStatusDetails.Where(t2 => t2.IsActive))).ToList();
            foreach (var status in previousInvoiceStatus)
            {
                status.StatusId = statusId;
            }
        }

        private void UpdateInvoiceDealStatus(int newInvoiceId, int discountId, int updateByUserId, int updateByCompanyId)
        {
            if (discountId > 0)
            {
                var discount = Context.DataContext.Discounts.SingleOrDefault(t => t.Id == discountId);
                if (discount != null)
                {
                    var newDiscount = Context.DataContext.Discounts.FirstOrDefault(t => t.DealName == discount.DealName && t.InvoiceId == newInvoiceId);
                    if (newDiscount != null)
                    {
                        newDiscount.DealStatus = (int)DealStatus.Accepted;
                        newDiscount.StatusChangedBy = updateByUserId;
                        newDiscount.StatusChangedCompanyId = updateByCompanyId;
                        newDiscount.StatusChangedDate = DateTimeOffset.Now;

                        Context.DataContext.Entry(newDiscount).State = EntityState.Modified;
                    }
                }
            }
        }

        protected DeliveryReqStatusUpdateModel UpdateTrackableScheduleStatus(int? TrackableScheduleId, int statusId, Invoice invoice, string DeliveryLevelPO = "")
        {
            DeliveryReqStatusUpdateModel deliveryReqStatus = null;
            DeliveryScheduleXTrackableSchedule trackableSchedule = null;
            if (TrackableScheduleId.HasValue && TrackableScheduleId.Value > 0)
            {
                trackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.FirstOrDefault(t => t.Id == TrackableScheduleId.Value);
            }
            if (trackableSchedule != null)
            {
                if (invoice != null)
                {
                    invoice.TrackableScheduleId = TrackableScheduleId;
                }
                else
                {
                    trackableSchedule.Invoices.Where(t => t.IsActiveInvoice && t.InvoiceHeaderId != invoice.InvoiceHeaderId).ToList().ForEach(t => t.TrackableScheduleId = null);
                }
                trackableSchedule.DeliveryScheduleStatusId = statusId;
                trackableSchedule.DeliveryLevelPO = DeliveryLevelPO == string.Empty ? string.Empty : DeliveryLevelPO;
                Context.DataContext.Entry(trackableSchedule).State = EntityState.Modified;
                Context.Commit();

                if (!string.IsNullOrWhiteSpace(trackableSchedule.FrDeliveryRequestId))
                {
                    deliveryReqStatus = new DeliveryReqStatusUpdateModel() { DeliveryRequestId = trackableSchedule.FrDeliveryRequestId, ScheduleStatusId = statusId, UserId = invoice.UpdatedBy };
                }
            }
            return deliveryReqStatus;
        }

        protected DeliveryReqStatusUpdateModel UpdateTrackableScheduleStatusForMissingDelivery(int? TrackableScheduleId, Invoice invoice, string DeliveryLevelPO = "")
        {
            DeliveryReqStatusUpdateModel deliveryReqStatus = null;
            DeliveryScheduleXTrackableSchedule trackableSchedule = null;
            if (TrackableScheduleId.HasValue && TrackableScheduleId.Value > 0)
            {
                trackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.FirstOrDefault(t => t.Id == TrackableScheduleId.Value);
                if (trackableSchedule != null)
                {
                    invoice.TrackableScheduleId = null;
                    trackableSchedule.DeliveryScheduleStatusId = (int)DeliveryScheduleStatus.Accepted;
                    trackableSchedule.DeliveryLevelPO = DeliveryLevelPO == string.Empty ? string.Empty : DeliveryLevelPO;
                    if (!string.IsNullOrWhiteSpace(trackableSchedule.FrDeliveryRequestId))
                    {
                        deliveryReqStatus = new DeliveryReqStatusUpdateModel() { DeliveryRequestId = trackableSchedule.FrDeliveryRequestId, ScheduleStatusId = trackableSchedule.DeliveryScheduleStatusId, UserId = invoice.UpdatedBy };
                    }
                }
            }
            return deliveryReqStatus;
        }

        private static bool IsInvoiceValidForInvoiceWorkflow(Invoice invoice)
        {
            return invoice.InvoiceXInvoiceStatusDetails.Any(t => t.IsActive && (t.StatusId == (int)InvoiceStatus.Received || t.StatusId == (int)InvoiceStatus.Rejected))
                    && !IsDigitalDropTicket(invoice.InvoiceTypeId);
        }

        private bool PerformOrderAutoClose(Order order, decimal totalDelivered, int? currentTrackableScheduleId = null)
        {
            bool response = false;
            // Close order if single delivery or quantity completed
            var maxQuantity = order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity;
            var percentDelivered = totalDelivered / maxQuantity * 100;
            var percentThreshold = order.FuelRequest.OrderClosingThreshold ?? 100;
            var deliveryTypeId = order.FuelRequest.FuelRequestDetail.DeliveryTypeId;
            if (deliveryTypeId == (int)DeliveryType.OneTimeDelivery || (percentDelivered >= percentThreshold && order.FuelRequest.QuantityTypeId != (int)QuantityType.NotSpecified))
            {
                bool hasSplitDrs = false;
                if (currentTrackableScheduleId != null && currentTrackableScheduleId.HasValue)
                {
                    string groupParentDRId = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.Id == currentTrackableScheduleId)
                                                                                             .Select(t => t.GroupParentDRId)
                                                                                             .FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(groupParentDRId))
                    {
                        hasSplitDrs = Context.DataContext.DeliveryScheduleXTrackableSchedules.Any(t => t.IsActive && t.GroupParentDRId == groupParentDRId &&
                                                                                        (t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                                                          && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                                                          && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                                          && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                                          && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                                                          && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted));
                    }
                }
                if (!hasSplitDrs && (!order.FuelRequest.Job.IsMarine || (order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive).All(t => t.WaitingFor == (int)WaitingAction.Nothing || t.WaitingFor == (int)WaitingAction.PDITaxes))))
                {
                    order.OrderXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                    OrderXStatus orderStatus = new OrderXStatus
                    {
                        StatusId = (int)OrderStatus.Closed,
                        IsActive = true,
                        UpdatedBy = (int)SystemUser.System,
                        UpdatedDate = DateTimeOffset.Now
                    };
                    order.OrderXStatuses.Add(orderStatus);
                    var openschedules = order.DeliveryScheduleXTrackableSchedules.Where(t => t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled &&
                                                                            t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted &&
                                                                            t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted &&
                                                                            t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.MissedAndCanceled &&
                                                                            t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed &&
                                                                            t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate &&
                                                                            t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted &&
                                                                            t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate).ToList();
                    openschedules.ForEach(t => t.DeliveryScheduleStatusId = (int)DeliveryScheduleStatus.Canceled);
                    response = true;

                    //Delete DS and DR on order close.
                    UserContext userContext = new UserContext();
                    userContext.UserName = SystemUser.System.ToString();
                    var scheduleBuilderDomain = new ScheduleBuilderDomain();
                    var drScheduleResonse = Task.Run(() => scheduleBuilderDomain.DeleteDeliveryRequestOnOrderClose(new List<int> { order.Id }, userContext)).Result;
                }



                var tankRentalDomain = new TankRentalInvoiceDomain();
                var isTankRental = Task.Run(() => tankRentalDomain.AddTankRentalInvoiceCreateMessage((int)SystemUser.System, order.Id)).Result;
            }
            return response;
        }

        private async Task DriverCompletedOrder(int driverId, string fcmId, int orderId, int? trackableScheduleId)
        {
            try
            {
                var appLocation = Context.DataContext.AppLocations.FirstOrDefault(t => t.UserId == driverId && t.FCMAppId == fcmId);
                if (appLocation != null)
                {
                    appLocation.OrderId = null;
                    appLocation.DeliveryScheduleId = null;
                    appLocation.TrackableScheduleId = null;
                    appLocation.StatusId = (int)EnrouteDeliveryStatus.CompletedDrop;

                    Context.DataContext.Entry(appLocation).State = EntityState.Modified;
                    await Context.CommitAsync();
                }

                var enrouteViewModel = new EnrouteDeliveryViewModel
                {
                    UserId = driverId,
                    OrderId = orderId,
                    TrackableScheduleId = trackableScheduleId,
                    StatusId = (int)EnrouteDeliveryStatus.CompletedDrop
                };

                if (trackableScheduleId.HasValue && trackableScheduleId.Value > 0)
                {
                    var trackableschedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.FirstOrDefault(t => t.Id == trackableScheduleId.Value);
                    if (trackableschedule != null)
                        enrouteViewModel.DeliveryScheduleId = trackableschedule.DeliveryScheduleId;
                }

                var enrouteDeliveryHistory = enrouteViewModel.ToEntity();
                Context.DataContext.EnrouteDeliveryHistories.Add(enrouteDeliveryHistory);
                await Context.CommitAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "DriverCompletedOrder", ex.Message, ex);
            }
        }

        protected void SetInvoiceNumber(string invoiceNumber, InvoiceModel invoiceModel)
        {
            if (invoiceModel.IsDigitalDropTicket())
            {
                if (!string.IsNullOrWhiteSpace(invoiceModel.ReferenceId))
                {
                    invoiceModel.ReferenceId = invoiceModel.ReferenceId.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
                }
                else
                {
                    invoiceModel.DisplayInvoiceNumber = invoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
                }
            }
        }

        private async Task UpdateMobileFuelSpills(int orderId, int invoiceId, int driverId)
        {
            var spills = Context.DataContext.Spills.Where(t => t.OrderId == orderId && t.InvoiceId == null && t.SpilledBy == driverId);
            try
            {
                foreach (var spill in spills)
                {
                    spill.InvoiceId = invoiceId;
                    Context.DataContext.Entry(spill).State = EntityState.Modified;
                }
                await Context.CommitAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "UpdateMobileFuelSpills", ex.Message, ex);
            }
        }

        protected dynamic GetFuelDispatchLocation(int orderId, int? trackableSchduelId, int locationType)
        {
            var dispatchLocation = Context.DataContext.FuelDispatchLocations.Where(t => t.IsActive && t.LocationType == (int)LocationType.PickUp
                                                                    && t.OrderId == orderId)
                                                                    .Select(t => new
                                                                    {
                                                                        t.TerminalId,
                                                                        t.TrackableScheduleId,
                                                                        t.DeliveryScheduleId,
                                                                        t.Latitude,
                                                                        t.Longitude,
                                                                        t.StateId,
                                                                        t.TimeZoneName,
                                                                        Address = new AddressViewModel
                                                                        {
                                                                            Address = t.Address,
                                                                            City = t.City,
                                                                            StateCode = t.StateCode,
                                                                            CountryCode = t.CountryCode,
                                                                            ZipCode = t.ZipCode,
                                                                            CountyName = t.CountyName
                                                                        }
                                                                    }).ToList();
            if (dispatchLocation.Any())
            {
                if (trackableSchduelId.HasValue)
                {
                    var locationForScheduleId = dispatchLocation.FirstOrDefault(t => t.TrackableScheduleId == trackableSchduelId);
                    if (locationForScheduleId != null)
                    {
                        return locationForScheduleId;
                    }
                }
                else
                {
                    var locationForOrder = dispatchLocation.FirstOrDefault(t => !t.TrackableScheduleId.HasValue);
                    if (locationForOrder != null)
                    {
                        return locationForOrder;
                    }
                }
            }

            return null;
        }

        protected static void SetFTLPricingToInvoiceModel(InvoiceModel invoiceModel, bool isAssetAvailable = false)
        {
            if (!isAssetAvailable)
            {
                var allowance = Math.Round(invoiceModel.AdditionalDetail.SupplierAllowance ?? 0, ApplicationConstants.InvoiceSuppplierAllowanceUnitPriceDecimalDisplay);
                if (invoiceModel.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net && invoiceModel.BolDetails.Select(t => t.NetQuantity).First().HasValue)
                {
                    invoiceModel.AdditionalDetail.TotalAllowance = Math.Round(invoiceModel.BolDetails.Select(t => t.NetQuantity).First().Value * allowance, ApplicationConstants.InvoiceSuppplierAllowanceTotalDecimalDisplay);
                    invoiceModel.BasicAmount = Math.Round(invoiceModel.BolDetails.Select(t => t.NetQuantity).First().Value * invoiceModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay) - invoiceModel.AdditionalDetail.TotalAllowance ?? 0;
                }
                else if (invoiceModel.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Gross && invoiceModel.BolDetails.Select(t => t.GrossQuantity).First().HasValue)
                {
                    invoiceModel.AdditionalDetail.TotalAllowance = Math.Round(invoiceModel.BolDetails.Select(t => t.GrossQuantity).First().Value * allowance, ApplicationConstants.InvoiceSuppplierAllowanceTotalDecimalDisplay);
                    invoiceModel.BasicAmount = Math.Round(invoiceModel.BolDetails.Select(t => t.GrossQuantity).First().Value * invoiceModel.PricePerGallon, ApplicationConstants.InvoiceBasicAmountDecimalDisplay) - invoiceModel.AdditionalDetail.TotalAllowance ?? 0;
                }
            }
        }

        protected void UpdateInvoiceActionResponseStatus(bool isDtnUploaded, StatusViewModel response)
        {
            if (!isDtnUploaded)
            {
                response.StatusMessage = response.StatusMessage + " " + Resource.warningForDtnCannotbeUploaded;
                response.StatusCode = Status.Warning;
            }
        }

        protected bool CreateDtnFileGenerationWorkflow(Invoice invoice, Order order)
        {
            CreateTelaFuelServiceWorkflow(invoice, order, invoice.UpdatedBy);
            bool isDtnUploadQueued = true;
            if (invoice != null && order != null && !IsDigitalDropTicket(invoice.InvoiceTypeId) && order.SendDtnFile)
            {
                string ftlSupplierDetails = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingFTLSupplierDtnDetails).Select(t => t.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(ftlSupplierDetails))
                {
                    DtnSupplierDetails ftlSuppliers = JsonConvert.DeserializeObject<DtnSupplierDetails>(ftlSupplierDetails);
                    DtnSuppliers ftlSupplier = ftlSuppliers.DtnSuppliers.FirstOrDefault(t => t.CompanyId == order.AcceptedCompanyId);
                    if (ftlSupplier != null)
                    {
                        string buyerSiteNumber = ftlSuppliers.SiteNumbers.Where(t => t.BuyerCompanyId == order.BuyerCompanyId && t.SupplierCompanyId == order.AcceptedCompanyId).Select(t => t.BuyerSiteNumber).FirstOrDefault();
                        if (string.IsNullOrWhiteSpace(buyerSiteNumber))
                        {
                            buyerSiteNumber = ftlSuppliers.SiteNumbers.Where(t => t.BuyerCompanyId == order.BuyerCompanyId && t.SupplierCompanyId == null).Select(t => t.BuyerSiteNumber).FirstOrDefault();
                        }
                        if (!string.IsNullOrEmpty(buyerSiteNumber) && IsTelaFuelServiceConfigured(ftlSupplier.CompanyId, order.BuyerCompanyId, invoice))
                        {
                            isDtnUploadQueued = true;
                        }
                        else if (!string.IsNullOrEmpty(buyerSiteNumber))
                        {
                            var jsonViewModel = new DtnFileProcessingRequestViewModel();
                            jsonViewModel.InvoiceId = invoice.InvoiceHeaderId;
                            jsonViewModel.InvoiceNumber = invoice.DisplayInvoiceNumber;
                            jsonViewModel.RefId = ftlSupplier.RefId;
                            jsonViewModel.Password = ftlSupplier.Password;
                            jsonViewModel.SiteNumber = buyerSiteNumber;
                            jsonViewModel.FtpUrl = ftlSuppliers.FtpUrl;
                            jsonViewModel.FtpUserName = ftlSupplier.FtpUserName;
                            jsonViewModel.FtpPassword = ftlSupplier.FtpPassword;
                            jsonViewModel.PathToUpload = ftlSupplier.FtpPathToUpload;
                            jsonViewModel.ReceiversEmail = ftlSuppliers.NotifiedUsers;
                            AddQueueEventForDtnFileGeneration(jsonViewModel, invoice.UpdatedBy);
                        }
                        else
                        {
                            isDtnUploadQueued = false;
                        }
                    }
                }
            }
            return isDtnUploadQueued;
        }
        public bool CreateDtnFileGenerationWorkflowFromQueueService(Invoice invoice, Order order)
        {
            bool isDtnUploadQueued = true;
            if (invoice != null && order != null && !IsDigitalDropTicket(invoice.InvoiceTypeId) && order.SendDtnFile)
            {
                string ftlSupplierDetails = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingFTLSupplierDtnDetails).Select(t => t.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(ftlSupplierDetails))
                {
                    DtnSupplierDetails ftlSuppliers = JsonConvert.DeserializeObject<DtnSupplierDetails>(ftlSupplierDetails);
                    DtnSuppliers ftlSupplier = ftlSuppliers.DtnSuppliers.FirstOrDefault(t => t.CompanyId == order.AcceptedCompanyId);
                    if (ftlSupplier != null)
                    {
                        string buyerSiteNumber = ftlSuppliers.SiteNumbers.Where(t => t.BuyerCompanyId == order.BuyerCompanyId && t.SupplierCompanyId == order.AcceptedCompanyId).Select(t => t.BuyerSiteNumber).FirstOrDefault();
                        if (string.IsNullOrWhiteSpace(buyerSiteNumber))
                        {
                            buyerSiteNumber = ftlSuppliers.SiteNumbers.Where(t => t.BuyerCompanyId == order.BuyerCompanyId && t.SupplierCompanyId == null).Select(t => t.BuyerSiteNumber).FirstOrDefault();
                        }
                        if (!string.IsNullOrEmpty(buyerSiteNumber))
                        {
                            var jsonViewModel = new DtnFileProcessingRequestViewModel();
                            jsonViewModel.InvoiceId = invoice.InvoiceHeaderId;
                            jsonViewModel.InvoiceNumber = invoice.DisplayInvoiceNumber;
                            jsonViewModel.RefId = ftlSupplier.RefId;
                            jsonViewModel.Password = ftlSupplier.Password;
                            jsonViewModel.SiteNumber = buyerSiteNumber;
                            jsonViewModel.FtpUrl = ftlSuppliers.FtpUrl;
                            jsonViewModel.FtpUserName = ftlSupplier.FtpUserName;
                            jsonViewModel.FtpPassword = ftlSupplier.FtpPassword;
                            jsonViewModel.PathToUpload = ftlSupplier.FtpPathToUpload;
                            jsonViewModel.ReceiversEmail = ftlSuppliers.NotifiedUsers;
                            AddQueueEventForDtnFileGeneration(jsonViewModel, invoice.UpdatedBy);
                        }
                    }
                }
            }
            return isDtnUploadQueued;
        }

        private bool IsTelaFuelServiceConfigured(int supplierCompanyId, int buyerCompanyId, Invoice invoice)
        {
            try
            {
                string telaFuelServiceSupplierDetails = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingTelaOrderAddApiSettings).Select(t => t.Value).FirstOrDefault();
                TelaFuelServiceSupplierDetails telaSupplierDetails = new TelaFuelServiceSupplierDetails();
                if (!string.IsNullOrEmpty(telaFuelServiceSupplierDetails))
                {
                    telaSupplierDetails = JsonConvert.DeserializeObject<TelaFuelServiceSupplierDetails>(telaFuelServiceSupplierDetails);
                }
                return telaSupplierDetails.TelaSuppliers != null
                                && telaSupplierDetails.TelaSuppliers.Any(t => t.CompanyId == supplierCompanyId && t.BuyerCompanyIds.Contains(buyerCompanyId))
                                && (invoice.InvoiceXAdditionalDetail.OriginalInvoiceHeaderId == 0 || invoice.InvoiceXAdditionalDetail.OriginalInvoiceHeaderId == null);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "IsTelaFuelServiceConfigured", ex.Message, ex);
                return false;
            }
        }
        public bool CreateTelaFuelServiceWorkflow(Invoice invoice, Order order, int userId = 0)
        {
            bool isRequestProcessed = true;
            try
            {
                if (invoice != null && order != null && !IsDigitalDropTicket(invoice.InvoiceTypeId) && order.SendDtnFile && (invoice.InvoiceXAdditionalDetail.OriginalInvoiceHeaderId == 0 || invoice.InvoiceXAdditionalDetail.OriginalInvoiceHeaderId == null))
                {
                    string telaFuelServiceSupplierDetails = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingTelaOrderAddApiSettings).Select(t => t.Value).FirstOrDefault();
                    if (!string.IsNullOrEmpty(telaFuelServiceSupplierDetails))
                    {
                        TelaFuelServiceSupplierDetails telaSuppliers = JsonConvert.DeserializeObject<TelaFuelServiceSupplierDetails>(telaFuelServiceSupplierDetails);
                        TelaSuppliers telaSupplier = telaSuppliers.TelaSuppliers.FirstOrDefault(t => t.CompanyId == order.AcceptedCompanyId);
                        if (telaSupplier != null)
                        {
                            if (telaSupplier.BuyerCompanyIds.Any(t => t == order.BuyerCompanyId))
                            {
                                var jsonViewModel = new TelaFuelServiceRequestViewModel();
                                jsonViewModel.InvoiceId = invoice.InvoiceHeaderId;
                                jsonViewModel.InvoiceNumber = invoice.DisplayInvoiceNumber;
                                jsonViewModel.CarrierLookup = telaSupplier.CarrierLookup;
                                jsonViewModel.FreightLaneLookup = telaSupplier.FreightLaneLookup;
                                jsonViewModel.SupplierLookup = telaSupplier.SupplierLookup;
                                jsonViewModel.UserName = telaSupplier.UserName;
                                jsonViewModel.Password = telaSupplier.Password;
                                if (order.FuelRequest.Job.LocationInventoryManagedBy != null)
                                {
                                    jsonViewModel.LocationInventoryManagedBy = (LocationInventoryManagedBy)order.FuelRequest.Job.LocationInventoryManagedBy;
                                }
                                AddQueueEventTelaFuelService(jsonViewModel, userId);
                            }
                            else
                            {
                                isRequestProcessed = false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                isRequestProcessed = false;
                LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateTelaFuelServiceWorkflow", ex.Message, ex);
            }
            return isRequestProcessed;
        }

        protected InvoiceHeaderDetail GenerateInvoiceHeader(List<InvoiceModel> invoices, int existingHeaderId = 0)
        {
            var groupDrIdInvModel = invoices.Where(t => !string.IsNullOrWhiteSpace(t.GroupParentDrId)
                                                    && t.WaitingFor == WaitingAction.AllDRCompletion).FirstOrDefault();

            if (groupDrIdInvModel != null && groupDrIdInvModel.SupplierCompanyId > 0)
            {
                var existingHeaderEntity = Context.DataContext.InvoiceHeaderDetails
                                .Where(t => t.Invoices.Any(i => i.GroupParentDrId.ToString() == groupDrIdInvModel.GroupParentDrId
                                            && i.IsActive && i.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                            && i.Order != null && i.Order.AcceptedCompanyId == groupDrIdInvModel.SupplierCompanyId))
                                .FirstOrDefault();
                if (existingHeaderEntity != null)
                {   
                    existingHeaderEntity.TotalDroppedGallons += invoices.Sum(t => t.DroppedGallons);
                    existingHeaderEntity.TotalBasicAmount += invoices.Sum(t => t.BasicAmount);
                    existingHeaderEntity.TotalFeeAmount += invoices.Sum(t => t.TotalFeeAmount ?? 0);
                    existingHeaderEntity.TotalTaxAmount += invoices.Sum(t => t.TotalTaxAmount);
                    existingHeaderEntity.TotalDiscountAmount += invoices.Sum(t => t.TotalDiscountAmount);

                    return existingHeaderEntity;
                }
                else
                    return GetNewHeaderDetailEntity(invoices);
            }
            else if (existingHeaderId > 0)
            {
                var existingHeaderEntity = Context.DataContext.InvoiceHeaderDetails.Where(t => t.Id == existingHeaderId).FirstOrDefault();
                if (existingHeaderEntity != null)
                {   
                    existingHeaderEntity.TotalDroppedGallons += invoices.Sum(t => t.DroppedGallons);
                    existingHeaderEntity.TotalBasicAmount += invoices.Sum(t => t.BasicAmount);
                    existingHeaderEntity.TotalFeeAmount += invoices.Sum(t => t.TotalFeeAmount ?? 0);
                    existingHeaderEntity.TotalTaxAmount += invoices.Sum(t => t.TotalTaxAmount);
                    existingHeaderEntity.TotalDiscountAmount += invoices.Sum(t => t.TotalDiscountAmount);
                    return existingHeaderEntity;
                }
                else
                    return GetNewHeaderDetailEntity(invoices);
            }
            else
            {
                return GetNewHeaderDetailEntity(invoices);
            }
        }

        private InvoiceHeaderDetail GetNewHeaderDetailEntity(List<InvoiceModel> invoices)
        {
            InvoiceHeaderDetail entity = new InvoiceHeaderDetail();
            var invoice = invoices.First();
            entity.InvoiceNumberId = invoice.InvoiceNumberId;
            entity.TotalDroppedGallons = invoices.Sum(t => t.DroppedGallons);
            entity.TotalBasicAmount = invoices.Sum(t => t.BasicAmount);
            entity.TotalFeeAmount = invoices.Sum(t => t.TotalFeeAmount ?? 0);
            entity.TotalTaxAmount = invoices.Sum(t => t.TotalTaxAmount);
            entity.TotalDiscountAmount = invoices.Sum(t => t.TotalDiscountAmount);
            entity.Version = 1;
            entity.CreatedBy = entity.UpdatedBy = invoice.CreatedBy;
            entity.CreatedDate = entity.UpdatedDate = invoice.CreatedDate;
            entity.IsActive = true;
            Context.DataContext.InvoiceHeaderDetails.Add(entity);
            return entity;
        }

        protected InvoiceHeaderDetail CreateNewInvoiceHeaderVersion(List<InvoiceModel> invoices, int invoiceVersion)
        {
            InvoiceHeaderDetail entity = new InvoiceHeaderDetail();
            var invoice = invoices.First();
            entity.InvoiceNumberId = invoice.InvoiceNumberId;
            entity.TotalDroppedGallons = invoices.Sum(t => t.DroppedGallons);
            entity.TotalBasicAmount = invoices.Sum(t => t.BasicAmount);
            entity.TotalFeeAmount = invoices.Sum(t => t.TotalFeeAmount ?? 0);
            entity.TotalTaxAmount = invoices.Sum(t => t.TotalTaxAmount);
            entity.TotalDiscountAmount = invoices.Sum(t => t.TotalDiscountAmount);
            entity.Version = invoiceVersion + 1;
            entity.CreatedBy = entity.UpdatedBy = invoice.CreatedBy;
            entity.CreatedDate = entity.UpdatedDate = invoice.CreatedDate;
            entity.IsActive = true;
            Context.DataContext.InvoiceHeaderDetails.Add(entity);
            return entity;
        }

        protected InvoiceHeaderDetail GenerateInvoiceHeader(List<Invoice> invoices)
        {
            InvoiceHeaderDetail entity = new InvoiceHeaderDetail();
            var invoice = invoices.First();
            entity.TotalDroppedGallons = invoices.Sum(t => t.DroppedGallons);
            entity.TotalBasicAmount = invoices.Sum(t => t.BasicAmount);
            entity.TotalFeeAmount = invoices.Sum(t => t.TotalFeeAmount ?? 0);
            entity.TotalTaxAmount = invoices.Sum(t => t.TotalTaxAmount);
            entity.TotalDiscountAmount = invoices.Sum(t => t.TotalDiscountAmount);
            entity.Version = 1;
            entity.CreatedBy = entity.UpdatedBy = invoice.CreatedBy;
            entity.CreatedDate = entity.UpdatedDate = invoice.CreatedDate;
            entity.IsActive = true;
            Context.DataContext.InvoiceHeaderDetails.Add(entity);
            return entity;
        }

        protected InvoiceHeaderDetail GenerateInvoiceHeader(List<InvoiceViewModel> invoices)
        {
            InvoiceHeaderDetail entity = new InvoiceHeaderDetail();
            var invoice = invoices.First();
            entity.InvoiceNumberId = invoice.InvoiceNumber.Id;
            entity.TotalDroppedGallons = invoices.Sum(t => t.DroppedGallons);
            entity.TotalBasicAmount = invoices.Sum(t => t.BasicAmount);
            entity.TotalFeeAmount = invoices.Sum(t => t.TotalFees);
            entity.TotalTaxAmount = invoices.Sum(t => t.TotalTaxAmount);
            entity.TotalDiscountAmount = invoices.Sum(t => t.TotalDiscountAmount);
            entity.Version = invoice.Version;
            entity.CreatedBy = entity.UpdatedBy = invoice.CreatedBy;
            entity.CreatedDate = entity.UpdatedDate = invoice.CreatedDate;
            entity.IsActive = true;
            Context.DataContext.InvoiceHeaderDetails.Add(entity);
            return entity;
        }

        private void AddQueueEventForDtnFileGeneration(DtnFileProcessingRequestViewModel viewModel, int userId)
        {
            QueueMessageDomain queueMessageDomain = new QueueMessageDomain();
            string json = JsonConvert.SerializeObject(viewModel);
            var queueRequest = new QueueMessageViewModel()
            {
                CreatedBy = userId,
                QueueProcessType = QueueProcessType.DtnFileGeneration,
                JsonMessage = json
            };
            queueMessageDomain.EnqeueMessage(queueRequest);
        }

        private void AddQueueEventTelaFuelService(TelaFuelServiceRequestViewModel viewModel, int userId)
        {
            QueueMessageDomain queueMessageDomain = new QueueMessageDomain();
            string json = JsonConvert.SerializeObject(viewModel);
            var queueRequest = new QueueMessageViewModel()
            {
                CreatedBy = userId,
                QueueProcessType = QueueProcessType.TelaFuelServiceOrderAdd,
                JsonMessage = json
            };
            queueMessageDomain.EnqeueMessage(queueRequest);
        }

        private void UpdateHeaderDetailsForDdtsInChain(List<Invoice> draftDdts, InvoiceEditRequestViewModel viewModel)
        {
            foreach (var ddt in draftDdts)
            {
                ddt.PaymentTermId = viewModel.InvoiceModel.PaymentTermId;
                ddt.NetDays = viewModel.InvoiceModel.NetDays;
                ddt.PaymentDueDate = viewModel.InvoiceModel.PaymentDueDate;
                ddt.TrackableScheduleId = viewModel.CurrentTrackableScheduleId;
                ddt.DriverId = viewModel.InvoiceModel.DriverId;
            }
        }

        protected async Task<InvoiceCreateResponseViewModel> CreateBalanceInvoiceAsync(InvoiceCreateRequestViewModel viewModel, InvoiceCreateRequestViewModel brokeredViewModels, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var invoiceResponse = new InvoiceCreateResponseViewModel();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var invoice = viewModel.InvoiceModel.ToBalanceInvoiceEntity();
            var invoiceHeader = GenerateInvoiceHeader(new List<Invoice>() { invoice });
            invoiceHeader.InvoiceNumberId = viewModel.InvoiceModel.InvoiceNumberId;
            await Context.CommitAsync();
            SetZeroGallonInvoiceBolDetails(invoiceHeader, viewModel.InvoiceModel, invoice, manualInvoiceViewModel.FuelId);
            if (order != null)
            {
                invoiceHeader.Invoices.Add(invoice);
                await Context.CommitAsync();

                UpdateZeroGallonsInvoiceDependentEntitiesPostCreate(order, invoice, invoiceResponse);

                await Context.CommitAsync();
                ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
            }

            return invoiceResponse;
        }

        protected async Task<InvoiceCreateResponseViewModel> CreateTankRentalInvoiceAsync(InvoiceCreateRequestViewModel viewModel, InvoiceCreateRequestViewModel brokeredViewModels, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var invoiceResponse = new InvoiceCreateResponseViewModel();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var invoice = viewModel.InvoiceModel.ToTankRentalInvoiceEntity();
            var invoiceHeader = GenerateInvoiceHeader(new List<Invoice>() { invoice });
            invoiceHeader.InvoiceNumberId = viewModel.InvoiceModel.InvoiceNumberId;
            await Context.CommitAsync();
            SetZeroGallonInvoiceBolDetails(invoiceHeader, viewModel.InvoiceModel, invoice, manualInvoiceViewModel.FuelId);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (order != null)
                    {
                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        UpdateZeroGallonsInvoiceDependentEntitiesPostCreate(order, invoice, invoiceResponse);

                        await Context.CommitAsync();
                        transaction.Commit();
                        invoiceResponse.StatusCode = Status.Success;
                        ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
                    }
                }
                catch (Exception ex)
                {
                    invoiceResponse.StatusCode = Status.Failed;
                    invoiceResponse.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    transaction.Rollback();
                    int ordreId = order != null ? order.Id : 0;
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateTankRentalInvoiceAsync", ex.Message + "OrderId -" + ordreId + " TankScheduleId -" + invoice.InvoiceXAdditionalDetail.TankFrequencyId, ex);
                }
            }
            return invoiceResponse;
        }

        protected async Task<InvoiceCreateResponseViewModel> CreateAutoTankRentalInvoiceAsync(InvoiceCreateRequestViewModel viewModel, InvoiceCreateRequestViewModel brokeredViewModels, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var invoiceResponse = new InvoiceCreateResponseViewModel();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var invoice = viewModel.InvoiceModel.ToAutoTankRentalInvoiceEntity();
            var invoiceHeader = GenerateInvoiceHeader(new List<Invoice>() { invoice });
            invoiceHeader.InvoiceNumberId = viewModel.InvoiceModel.InvoiceNumberId;
            await Context.CommitAsync();
            SetZeroGallonInvoiceBolDetails(invoiceHeader, viewModel.InvoiceModel, invoice, manualInvoiceViewModel.FuelId);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (order != null)
                    {
                        invoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        UpdateZeroGallonsInvoiceDependentEntitiesPostCreate(order, invoice, invoiceResponse);

                        await Context.CommitAsync();
                        transaction.Commit();

                        ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
                        invoiceResponse.StatusCode = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    invoiceResponse.StatusCode = Status.Failed;
                    invoiceResponse.StatusMessage = Resource.errMessageInvoiceCreateFailed;
                    transaction.Rollback();
                    int ordreId = order != null ? order.Id : 0;
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateAutoTankRentalInvoiceAsync", ex.Message + "OrderId -" + ordreId + " TankScheduleId -" + invoice.InvoiceXAdditionalDetail.TankFrequencyId, ex);
                }
            }
            return invoiceResponse;
        }

        protected async Task<InvoiceCreateResponseViewModel> TankRentalRebillInvoice(InvoiceCreateRequestViewModel viewModel, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var invoiceResponse = new InvoiceCreateResponseViewModel();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);
            var invoice = viewModel.InvoiceModel.ToTankRentalInvoiceEntity();
            var invoiceHeader = GenerateInvoiceHeader(new List<Invoice>() { invoice });
            invoiceHeader.InvoiceNumberId = viewModel.InvoiceModel.InvoiceNumberId;
            await Context.CommitAsync();
            SetZeroGallonInvoiceBolDetails(invoiceHeader, viewModel.InvoiceModel, invoice, manualInvoiceViewModel.FuelId);
            invoice.InvoiceXAdditionalDetail.OriginalInvoiceId = manualInvoiceViewModel.InvoiceId;
            if (invoice.InvoiceXAdditionalDetail.OriginalInvoiceId != null)
            {
                invoice.InvoiceXAdditionalDetail.OriginalInvoiceHeaderId = Context.DataContext.Invoices.Where(t => t.Id == manualInvoiceViewModel.InvoiceId).Select(t => t.InvoiceHeaderId).FirstOrDefault();
            }
            invoice.DisplayInvoiceNumber = invoice.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFRB);

            if (order != null)
            {
                invoiceHeader.Invoices.Add(invoice);
                await Context.CommitAsync();
                var headerId = Context.DataContext.Invoices.Where(t => t.Id == manualInvoiceViewModel.InvoiceId).Select(t => t.InvoiceHeaderId).FirstOrDefault();
                UpdatePreviousInvoiceStatusAsCreditedRebilled(headerId, (int)InvoiceStatus.CreditedAndRebilled);
                UpdateZeroGallonsInvoiceDependentEntitiesPostRebill(order, invoice, invoiceResponse);

                await Context.CommitAsync();

                ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
            }
            return invoiceResponse;
        }

        protected async Task<InvoiceCreateResponseViewModel> BalanceRebillInvoice(InvoiceCreateRequestViewModel viewModel, ManualInvoiceViewModel manualInvoiceViewModel)
        {
            var invoiceResponse = new InvoiceCreateResponseViewModel();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == viewModel.OrderId);

            var invoice = viewModel.InvoiceModel.ToBalanceInvoiceEntity();
            var invoiceHeader = GenerateInvoiceHeader(new List<Invoice>() { invoice });
            invoiceHeader.InvoiceNumberId = viewModel.InvoiceModel.InvoiceNumberId;
            await Context.CommitAsync();
            SetZeroGallonInvoiceBolDetails(invoiceHeader, viewModel.InvoiceModel, invoice, manualInvoiceViewModel.FuelId);
            invoice.InvoiceXAdditionalDetail.OriginalInvoiceId = manualInvoiceViewModel.InvoiceId;
            if (invoice.InvoiceXAdditionalDetail.OriginalInvoiceId != null)
            {
                invoice.InvoiceXAdditionalDetail.OriginalInvoiceHeaderId = Context.DataContext.Invoices.Where(t => t.Id == manualInvoiceViewModel.InvoiceId).Select(t => t.InvoiceHeaderId).FirstOrDefault();
            }
            invoice.DisplayInvoiceNumber = invoice.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFRB);

            if (order != null)
            {
                invoiceHeader.Invoices.Add(invoice);
                await Context.CommitAsync();
                var headerId = Context.DataContext.Invoices.Where(t => t.Id == manualInvoiceViewModel.InvoiceId).Select(t => t.InvoiceHeaderId).FirstOrDefault();
                UpdatePreviousInvoiceStatusAsCreditedRebilled(headerId, (int)InvoiceStatus.CreditedAndRebilled);
                UpdateZeroGallonsInvoiceDependentEntitiesPostRebill(order, invoice, invoiceResponse);

                await Context.CommitAsync();

                ConstructInvoiceCreateResponseViewModel(viewModel, invoiceResponse, order, invoice);
            }
            return invoiceResponse;
        }

        private void UpdateZeroGallonsInvoiceDependentEntitiesPostCreate(Order order, Invoice invoice, InvoiceCreateResponseViewModel response)
        {
            SetHedgeSpotAmounts(order, invoice, false, false);
            CreateQbAccountingWorkflowForInvoice(false, invoice, order, null);
            CreateQbAccountingWorkflowForBill(false, invoice, order, null);
            response.IsDtnUploaded = true;
        }

        private void UpdateZeroGallonsInvoiceDependentEntitiesPostRebill(Order order, Invoice invoice, InvoiceCreateResponseViewModel response)
        {
            SetHedgeSpotAmounts(order, invoice, true, false);
            CreateQbAccountingWorkflowForInvoice(false, invoice, order, null);
            CreateQbAccountingWorkflowForBill(false, invoice, order, null);
            response.IsDtnUploaded = true;
        }

        protected void AutoOpenOrder(Order order, FuelRequest fuelRequest, int deliveryTypeId)
        {
            OrderXStatus orderPreviousStatus = order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive);
            decimal orderDroppedGallons = order.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice).Sum(t1 => t1.DroppedGallons);
            if (orderPreviousStatus.StatusId == (int)OrderStatus.Closed && fuelRequest.MaxQuantity > 0)
            {
                // 12.Open order if draft ddt canceled
                var percentDelivered = orderDroppedGallons / (order.BrokeredMaxQuantity ?? fuelRequest.MaxQuantity) * 100;
                var percentThreshold = order.FuelRequest.OrderClosingThreshold ?? 100;
                if (deliveryTypeId == (int)DeliveryType.OneTimeDelivery || percentDelivered < percentThreshold)
                {
                    orderPreviousStatus.IsActive = false;
                    OrderXStatus orderStatus = new OrderXStatus();
                    orderStatus.StatusId = (int)OrderStatus.Open;
                    orderStatus.IsActive = true;
                    orderStatus.OrderId = order.Id;
                    orderStatus.UpdatedBy = (int)SystemUser.System;
                    orderStatus.UpdatedDate = DateTimeOffset.Now;
                    Context.DataContext.OrderXStatuses.Add(orderStatus);
                }
            }
        }

        public bool IsDuplicateInvoiceNumber(string supplierInvoiceNumber, int invHeaderId = 0)
        {
            bool isExist = false;
            if (!string.IsNullOrWhiteSpace(supplierInvoiceNumber))
            {
                isExist = Context.DataContext.Invoices
                            .Any(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                        && t.DisplayInvoiceNumber.Trim().ToLower() == supplierInvoiceNumber.Trim().ToLower()
                                        && (invHeaderId == 0 || t.InvoiceHeaderId != invHeaderId));
            }
            return isExist;
        }

        public int? GetDriverId(string firstName, string lastName, string poNumber, UserContext context)
        {
            var existingUser = Context.DataContext.Users.Where(t => t.CompanyId == context.CompanyId
                                        && t.FirstName.ToLower().Equals(firstName.ToLower().Trim())
                                        && t.LastName.ToLower().Equals(lastName.ToLower().Trim()))
                                        .Select(t => t.Id)
                                        .FirstOrDefault();

            if (existingUser == 0)
            {
                var roleId = new List<int> { (int)UserRoles.Driver };
                var drivers = new List<AdditionalUserViewModel>();
                var driver = new AdditionalUserViewModel()
                {
                    CompanyId = context.CompanyId,
                    FirstName = firstName.Trim(),
                    LastName = lastName.Trim(),
                    Email = $"{firstName.ToLower()}+{lastName.ToLower()}+{poNumber.ToLower()}@mailinator.com",
                    RoleIds = roleId,
                    DisplayMode = PageDisplayMode.Create,
                    UpdatedDate = DateTimeOffset.Now,
                    IsInvitationSent = false
                };
                drivers.Add(driver);
                var newdrivers = new AdditionalUsersViewModel() { UserId = context.Id, AdditionalUsers = drivers };
                var newUser = new SettingsDomain().AddCompanyUser(newdrivers).Result;
                var newDriver = newdrivers.AdditionalUsers.First();
                if (newDriver != null)
                    existingUser = newDriver.Id;
            }
            return existingUser;
        }

        public int GetOrderIdFromLocAndProduct(UserContext apiUserContext, string productId, string locationId, int supplierCompanyIdForCarrier, ref string poNumber)
        {
            var tfxFueltypeId = Context.DataContext.SupplierMappedProductDetails
                                                .Where(t => t.IsActive && t.CompanyId == apiUserContext.CompanyId && t.MyProductId.ToLower().Equals(productId.ToLower()))
                                                .OrderByDescending(t => t.Id)
                                                .Select(t => t.FuelTypeId)
                                                .FirstOrDefault();

            if (tfxFueltypeId > 0)
            {
                var mstProducts = Context.DataContext.MstProducts.Where(t => t.TfxProductId == tfxFueltypeId
                                    && (t.ProductCode != null || t.ProductTypeId == (int)ProductDisplayGroups.OtherFuelType) && t.IsActive).Select(t => t.Id).ToList();

                var order = Context.DataContext.Orders.Where(t => mstProducts.Any(p => p == t.FuelRequest.FuelTypeId)
                            && t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open)
                            && (t.FuelRequest.Job.DisplayJobID.ToLower().Equals(locationId.ToLower()) || t.FuelRequest.Job.Name.ToLower().Equals(locationId.ToLower()))
                            && t.AcceptedCompanyId == apiUserContext.CompanyId
                            && (t.BuyerCompanyId == supplierCompanyIdForCarrier || supplierCompanyIdForCarrier == 0))
                        .OrderByDescending(t => t.AcceptedDate)
                        .Select(t => new { t.Id, t.PoNumber })
                        .SingleOrDefault();
                if (order != null)
                {
                    poNumber = order.PoNumber;
                    return order.Id;
                }
            }
            else
            {
                var mstProducts = Context.DataContext.MstProducts.Where(t =>
                                    ((t.DisplayName.ToLower().Equals(productId.ToLower()) && t.ProductCode != null && t.ProductTypeId != (int)ProductTypes.NonStandardFuel)
                                    || (t.Name.ToLower().Equals(productId.ToLower()) && t.ProductTypeId == (int)ProductTypes.NonStandardFuel))
                                    && t.IsActive).Select(t => t.Id).ToList();

                var order = Context.DataContext.Orders.Where(t => mstProducts.Any(p => p == t.FuelRequest.FuelTypeId)
                            && t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open)
                            && (t.FuelRequest.Job.DisplayJobID.ToLower().Equals(locationId.ToLower()) || t.FuelRequest.Job.Name.ToLower().Equals(locationId.ToLower()))
                            && t.AcceptedCompanyId == apiUserContext.CompanyId
                            && (t.BuyerCompanyId == supplierCompanyIdForCarrier || supplierCompanyIdForCarrier == 0))
                        .OrderByDescending(t => t.AcceptedDate)
                        .Select(t => new { t.Id, t.PoNumber })
                        .SingleOrDefault();
                if (order != null)
                {
                    poNumber = order.PoNumber;
                    return order.Id;
                }
            }

            return 0;
        }

        public int GetOrderIdForApiLocAndProduct(UserContext apiUserContext, List<int> mstProducts, string locationId, int supplierCompanyIdForCarrier, ref string poNumber)
        {
            if (mstProducts != null && mstProducts.Any())
            {
                var order = Context.DataContext.Orders.Where(t => mstProducts.Any(p => p == t.FuelRequest.FuelTypeId)
                            && t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open)
                            && (t.FuelRequest.Job.DisplayJobID.ToLower().Equals(locationId.ToLower()) || t.FuelRequest.Job.Name.ToLower().Equals(locationId.ToLower()))
                            && t.AcceptedCompanyId == apiUserContext.CompanyId
                            && (t.BuyerCompanyId == supplierCompanyIdForCarrier || supplierCompanyIdForCarrier == 0))
                        .OrderByDescending(t => t.AcceptedDate)
                        .Select(t => new { t.Id, t.PoNumber })
                        .SingleOrDefault();
                if (order != null)
                {
                    poNumber = order.PoNumber;
                    return order.Id;
                }
            }

            return 0;
        }

        public async Task<List<int>> GetProductListByName(string productId, UserContext apiUserContext)
        {
            var mstProducts = new List<int>();
            var tfxFueltypeId = await Context.DataContext.SupplierMappedProductDetails
                                                .Where(t => t.IsActive && t.CompanyId == apiUserContext.CompanyId && t.MyProductId.ToLower().Equals(productId.ToLower()))
                                                .OrderByDescending(t => t.Id)
                                                .Select(t => t.FuelTypeId)
                                                .FirstOrDefaultAsync();
            if (tfxFueltypeId > 0)
            {
                mstProducts = await Context.DataContext.MstProducts.Where(t => t.TfxProductId == tfxFueltypeId
                                    && (t.ProductCode != null || t.ProductTypeId == (int)ProductTypes.NonStandardFuel) && t.IsActive).Select(t => t.Id).ToListAsync();

            }
            else
            {
                mstProducts = await Context.DataContext.MstProducts.Where(t =>
                                    ((t.DisplayName.ToLower().Equals(productId.ToLower()) && t.ProductCode != null && t.ProductTypeId != (int)ProductTypes.NonStandardFuel)
                                    || (t.Name.ToLower().Equals(productId.ToLower()) && t.ProductTypeId == (int)ProductTypes.NonStandardFuel))
                                    && t.IsActive).Select(t => t.Id).ToListAsync();
            }

            return mstProducts;
        }

        protected async Task<StatusViewModel> UpdateDDTForImagesAsync(ConsolidatedInvoiceEditViewModel invoiceEditViewModel, int? orderId)
        {
            var response = new StatusViewModel();
            var invoices = invoiceEditViewModel.invoiceModels;
            var existingInvoiceHeader = await Context.DataContext.InvoiceHeaderDetails.Where(t => t.Id == invoiceEditViewModel.InvoiceHeader.Id).FirstOrDefaultAsync();
            var newInvoiceHeader = invoiceEditViewModel.InvoiceHeader.ToEntity();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    UpdateExistingInvoiceHeader(existingInvoiceHeader, newInvoiceHeader);
                    newInvoiceHeader.Version += 1;
                    Context.DataContext.InvoiceHeaderDetails.Add(newInvoiceHeader);
                    await Context.CommitAsync();

                    foreach (var invoiceModel in invoices)
                    {
                        var invoice = invoiceModel.ToEntity();
                        var assetDrops = invoiceModel.AssetDrops.Select(t => t.ToEntity()).ToList();
                        invoice.AssetDrops = assetDrops;
                        newInvoiceHeader.Invoices.Add(invoice);
                        await Context.CommitAsync();

                        await SetInvoiceBolDetailsForEdit(newInvoiceHeader, invoiceModel, invoice);

                        UpdatePreviousInvoiceVersionStatus(invoiceModel.Id, (int)InvoiceVersionStatus.InActive);
                        await Context.CommitAsync();
                        invoiceModel.Id = invoice.Id;
                    }

                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.EntityId = invoices.Where(t => t.OrderId == orderId).Select(t => t.Id).FirstOrDefault();
                    response.EntityNumber = invoices.Where(t => t.OrderId == orderId).Select(t => t.DisplayInvoiceNumber).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("InvoiceBaseDomain", "UpdateInvoiceForEditNumberAsync", ex.Message, ex);
                }
                return response;
            }

        }

        protected int GetTerminalIdFromControlNumber(string terminalNumber, UserContext userContext)
        {
            int terminal = 0;
            try
            {
                terminal = Context.DataContext.MstExternalTerminals.Where(t => t.ControlNumber != null
                && (t.ControlNumber.ToLower().Equals(terminalNumber.ToLower()) || t.Name.ToLower().Equals(terminalNumber.ToLower())))
                                            .Select(t => t.Id).SingleOrDefault();
                if (terminal == 0)
                {
                    terminal = Context.DataContext.TerminalCompanyAliases.Where(t => t.CreatedByCompanyId == userContext.CompanyId && t.TerminalId != null
                    && t.IsActive && t.TerminalSupplierId == null && t.AssignedTerminalId.ToLower().Equals(terminalNumber.ToLower())).Select(t => t.Id).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                //no need to raise exception
            }
            return terminal;
        }

        public async Task<PDIFuelOrder> GetFuelOrderAddInput(int invoiceHeaderId)
        {
            PDIFuelOrder response = null;

            var deliveryDetails = await new StoredProcedureDomain(this).GetDeliveryDetailsAsync(invoiceHeaderId);
            if (deliveryDetails != null && deliveryDetails.Any())
            {
                response = new PDIFuelOrder();
                var headerDetails = deliveryDetails[0];
                response.DestinationType = headerDetails.DestinationType ? (int)PDIDestinationType.CompanyOwnedLocation : (int)PDIDestinationType.CustomerLocation;
                if (response.DestinationType == (int)PDIDestinationType.CustomerLocation)
                {
                    response.CustomerID = headerDetails.CustomerId;
                    response.CustomerLocationID = headerDetails.JobName;
                }
                else if (response.DestinationType == (int)PDIDestinationType.CompanyOwnedLocation)
                {
                    response.SiteID = headerDetails.SiteId;
                }

                if (!string.IsNullOrWhiteSpace(headerDetails.PoNumber))
                    response.PurchaseOrderNo = headerDetails.PoNumber.CropToFirstChars(19);

                if (headerDetails.IsPDITaxRequired && headerDetails.IsMarineLocation
                    && (headerDetails.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual
                                                        || headerDetails.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
                {
                    //prefix M: to get Taxes from PDI
                    response.AlternateReferenceNo = "M:" + headerDetails.DisplayInvoiceNumber;
                    if (response.AlternateReferenceNo.Length > 20)
                    {
                        //PDI has max length limit on AlternateReferenceNo
                        response.AlternateReferenceNo = response.AlternateReferenceNo.CropToFirstChars(19);
                    }
                }
                else
                    response.AlternateReferenceNo = headerDetails.DisplayInvoiceNumber;

                //added as per new requirement
                if (!string.IsNullOrWhiteSpace(headerDetails.TargetDriverValue))
                    response.DriverID = headerDetails.TargetDriverValue.CropToFirstChars(9);

                if (string.IsNullOrWhiteSpace(response.DriverID))
                {
                    if (!string.IsNullOrEmpty(headerDetails.DriverName))
                        response.DriverName = headerDetails.DriverName.CropToFirstChars(39);
                }

                response.DeliveryDateTime = deliveryDetails.Max(t => t.DropEndDate.DateTime);
                response.BusinessDate = deliveryDetails.Max(t => t.BusinessDate.DateTime.Date).ToString("yyyy-MM-dd");
                response.CarrierID = !string.IsNullOrEmpty(headerDetails.Carrier) ? headerDetails.Carrier : headerDetails.VendorId;
                if (deliveryDetails.Any(t => t.LiftDate.HasValue))
                {
                    response.LiftDateTime = deliveryDetails.Where(t => t.LiftDate != null).Max(t => t.LiftDate.Value.DateTime);
                }
                else
                {
                    response.LiftDateTime = deliveryDetails.Where(t => t.DropEndDate != null).Max(t => t.DropEndDate.DateTime);
                }
                var dropDetails = deliveryDetails.GroupBy(t => t.InvoiceId);
                int i = 1;
                foreach (var drops in dropDetails)
                {
                    var header = drops.FirstOrDefault();
                    if (header != null)
                    {
                        foreach (var pickup in drops)
                        {
                            FuelDetail fuelDetail = new FuelDetail();
                            fuelDetail.OrderLineItemNo = i;
                            fuelDetail.PurchasedProductID = fuelDetail.OrderedProductID = header.FuelType;

                            if (pickup.IsMarineLocation)
                            {
                                fuelDetail.OrderedQuantity = pickup.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Gross ? pickup.GrossQuantity ?? pickup.DroppedGallons : pickup.NetQuantity ?? pickup.DroppedGallons;
                                fuelDetail.DeliveredGrossQuantity = pickup.GrossQuantity ?? pickup.DroppedGallons;
                                fuelDetail.DeliveredNetQuantity = pickup.NetQuantity ?? pickup.DroppedGallons;
                            }
                            else
                            {
                                fuelDetail.OrderedQuantity = pickup.DroppedGallons;
                                fuelDetail.DeliveredGrossQuantity = pickup.DroppedGallons;
                                fuelDetail.DeliveredNetQuantity = pickup.DroppedGallons;
                            }

                            LoadDetail loadDetail = new LoadDetail();
                            if (pickup.LiftDate.HasValue)
                            {
                                // lift date should not be greater than drop date
                                if (pickup.LiftDate.Value > pickup.DropEndDate)
                                    loadDetail.LiftDateTime = pickup.DropEndDate.DateTime;
                                else
                                    loadDetail.LiftDateTime = pickup.LiftDate.Value.DateTime;
                            }
                            else
                            {
                                loadDetail.LiftDateTime = pickup.DropEndDate.DateTime;
                            }
                            loadDetail.LoadProductID = pickup.FuelType;
                            loadDetail.LiftNetQuantity = pickup.NetQuantity ?? pickup.DroppedGallons;
                            loadDetail.LiftGrossQuantity = pickup.GrossQuantity ?? pickup.DroppedGallons;
                            loadDetail.OriginType = pickup.PickupLocation == (int)PickupLocationType.BulkPlant ? 1 : 0;
                            if (loadDetail.OriginType == (int)PDIOriginType.Terminal)
                            {
                                loadDetail.OriginVendorID = header.VendorId;
                                loadDetail.OriginTerminalID = pickup.SiteName;
                            }
                            else if (loadDetail.OriginType == (int)PDIOriginType.BulkPlant)
                            {
                                loadDetail.OriginSiteID = pickup.SiteName;
                            }

                            if (!string.IsNullOrWhiteSpace(header.StaticOriginVendorId) && string.IsNullOrWhiteSpace(loadDetail.OriginVendorID))
                                loadDetail.OriginVendorID = header.StaticOriginVendorId;

                            loadDetail.BOLNo = pickup.BolNumber ?? header.DisplayInvoiceNumber;
                            loadDetail.LoadQuantity = pickup.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Gross ? pickup.GrossQuantity ?? pickup.DroppedGallons : pickup.NetQuantity ?? pickup.DroppedGallons;


                            ///// get pricing details to send to PDI for Marine
                            if (pickup.IsIncludePricing)
                            {
                                if ((pickup.UoM == (int)UoM.Barrels || pickup.UoM == (int)UoM.MetricTons) &&
                                                    pickup.ConvertedPricing != null && pickup.ConvertedPricing.HasValue)
                                {
                                    //// Decimal(15,7) format already done in SP, so no need to format here
                                    fuelDetail.UnitPrice = pickup.ConvertedPricing ?? 0;
                                }
                                else //if (pickup.RequestPriceDetailId > 0 && (pickup.UoM == (int)UoM.Gallons || pickup.UoM == (int)UoM.Litres))
                                {
                                    fuelDetail.UnitPrice = pickup.PricePerGallon;
                                }
                            }

                            // for Rebill invoice
                            if (!string.IsNullOrWhiteSpace(headerDetails.OriginalInvoiceNumber) && headerDetails.InvoiceTypeId != (int)InvoiceType.CreditInvoice)
                            {
                                loadDetail.BOLNo = loadDetail.BOLNo + Constants.PDIDefaultKeyForRebillInvoice;
                            }
                            else if (pickup.InvoiceTypeId == (int)InvoiceType.CreditInvoice)
                            {
                                loadDetail.BOLNo = loadDetail.BOLNo + Constants.PDIDefaultKeyForCreditInvoice;
                                loadDetail.LoadQuantity = -1 * loadDetail.LoadQuantity;
                                loadDetail.LiftNetQuantity = -1 * loadDetail.LiftNetQuantity;
                                loadDetail.LiftGrossQuantity = -1 * loadDetail.LiftGrossQuantity;

                                fuelDetail.OrderedQuantity = -1 * fuelDetail.OrderedQuantity;
                                fuelDetail.DeliveredGrossQuantity = -1 * fuelDetail.DeliveredGrossQuantity;
                                fuelDetail.DeliveredNetQuantity = -1 * fuelDetail.DeliveredNetQuantity;
                            }

                            fuelDetail.LoadDetail = loadDetail;

                            response.FuelDetails.Add(fuelDetail);

                            i++;
                        }
                    }
                }
            }

            return response;
        }


        private void InActivePreviousInvoiceVersionAssestDropByHeaderId(int invoiceHeaderId)
        {
            var previousDt = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId).Select(t => new { t.AssetDrops }).ToList();
            if (previousDt != null)
            {
                foreach (var asstDrp in previousDt)
                {
                    if (asstDrp.AssetDrops != null && asstDrp.AssetDrops.Count > 0)
                    {
                        asstDrp.AssetDrops.ToList().ForEach(asst => asst.IsActive = false);
                    }
                }
            }
        }
        private int InactivatePreviousInvoiceVersion(int originalInvoiceHeaderId, List<InvoiceModel> invoices, out bool isDdtToInvoiceConversion)
        {
            int invoiceVersion = 0;
            isDdtToInvoiceConversion = false;
            var invoiceHeader = Context.DataContext.InvoiceHeaderDetails.FirstOrDefault(t => t.Id == originalInvoiceHeaderId);
            if (invoiceHeader != null)
            {
                invoiceVersion = invoiceHeader.Version;
                invoiceHeader.IsActive = false;
                // Inactive assest drop of previous version invoice
                InActivePreviousInvoiceVersionAssestDropByHeaderId(invoiceHeader.Id);
                if (invoiceHeader.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || t.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                        && invoices.Any(t => !t.IsDigitalDropTicket()))
                {
                    invoiceHeader.Invoices.ToList().ForEach(t => t.WaitingFor = (int)WaitingAction.Nothing);
                    isDdtToInvoiceConversion = true;
                }
                else
                {
                    invoiceHeader.Invoices.ToList().ForEach(t => t.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive);
                }

                invoiceHeader.Invoices.SelectMany(t => t.InvoiceExceptions).ToList().ForEach(t => t.IsActive = false);
            }

            Context.Commit();
            return invoiceVersion;
        }

        private void UpdatePreviousVersionTrackableScheduleAndOrderDetails(int originalInvoiceHeaderId, InvoiceViewModelNew invoiceViewModel)
        {
            var invoiceHeader = Context.DataContext.InvoiceHeaderDetails.FirstOrDefault(t => t.Id == originalInvoiceHeaderId);
            if (invoiceHeader != null && invoiceViewModel.IsMissingDeliveryDDTConversion)
            {
                foreach (var invoice in invoiceHeader.Invoices)
                {
                    var drop = invoiceViewModel.Drops.FirstOrDefault(t => t.InvoiceId == invoice.Id);
                    if (drop != null)
                    {
                        if (drop.IsReassignDifferentJob)
                            invoice.TrackableScheduleId = null;
                        if (drop.OldOrderId != 0)
                            invoice.OrderId = drop.OldOrderId;
                    }
                }
            }

            Context.Commit();
        }

        public bool CreatePDIAPIWorkflow(Invoice invoice, Order order)
        {
            bool isRequestProcessed = true;
            string IsPDIAPIEnabled = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingPDIEnterpriseApiIsEnabled).Select(t => t.Value).FirstOrDefault();
            try
            {
                if (IsPDIAPIEnabled == "1" && invoice != null)
                {
                    var orderAdditionalDetails = Context.DataContext.Invoices.Where(t => t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.InvoiceHeaderId == invoice.InvoiceHeaderId && (t.WaitingFor == (int)WaitingAction.Nothing || t.WaitingFor == (int)WaitingAction.PDITaxes))
                                                                            .Select(t => new { OrderId = t.OrderId, BOLInvoicePreferenceId = t.Order != null && t.Order.OrderAdditionalDetail != null ? t.Order.OrderAdditionalDetail.BOLInvoicePreferenceId : null })
                                                                            .Distinct().ToList();

                    if (orderAdditionalDetails != null && orderAdditionalDetails.All(t => t.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails))
                    {
                        var jsonViewModel = new PDIAPIRequestViewModel();
                        jsonViewModel.InvoiceHeaderId = invoice.InvoiceHeaderId;
                        jsonViewModel.InvoiceNumber = invoice.DisplayInvoiceNumber;
                        //jsonViewModel.OrderId = order.Id;
                        AddQueueEventPDIAPI(jsonViewModel, invoice.UpdatedBy);
                    }
                }
            }
            catch (Exception ex)
            {
                isRequestProcessed = false;
                LogManager.Logger.WriteException("InvoiceBaseDomain", "CreatePDIAPIWorkflow", ex.Message, ex);
            }
            return isRequestProcessed;
        }

        public void AddQueueEventPDIAPI(PDIAPIRequestViewModel viewModel, int userId)
        {
            QueueMessageDomain queueMessageDomain = new QueueMessageDomain();
            string json = JsonConvert.SerializeObject(viewModel);
            var queueRequest = new QueueMessageViewModel()
            {
                CreatedBy = userId,
                QueueProcessType = QueueProcessType.PDIAPIDeliveryDetails,
                JsonMessage = json
            };
            queueMessageDomain.EnqeueMessage(queueRequest);
        }

        private List<CumulationQuantityUpdateViewModel> CreateListOfCumulationQuantityUpdateForInvEdit(decimal originalDroppedGallons, int requestPriceDetailsId, decimal fuelDroppedAfterInvEdit)
        {
            var responseList = new List<CumulationQuantityUpdateViewModel>();
            try
            {

                var droppedGallonsDelta = fuelDroppedAfterInvEdit - originalDroppedGallons;
                var item = new CumulationQuantityUpdateViewModel();
                item.DroppedGallons = droppedGallonsDelta;
                item.RequestPriceDetailsId = requestPriceDetailsId;

                responseList.Add(item);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateListOfCumulationQuantityUpdateForInvEdit", ex.Message, ex);
            }
            return responseList;
        }

        private List<CumulationQuantityUpdateViewModel> CreateListOfCumulationQuantityUpdateForPartialCreditInv(decimal DroppedGallons, int requestPriceDetailsId)
        {
            var responseList = new List<CumulationQuantityUpdateViewModel>();
            try
            {
                var item = new CumulationQuantityUpdateViewModel();
                item.DroppedGallons = DroppedGallons;
                item.RequestPriceDetailsId = requestPriceDetailsId;

                responseList.Add(item);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateListOfCumulationQuantityUpdateForPartialCreditInv", ex.Message, ex);
            }
            return responseList;
        }

        private List<CumulationQuantityUpdateViewModel> CreateListOfCumulationQuantityUpdateForRebillInv(List<InvoiceModel> invoiceModels, List<DropAdditionalDetailsModel> additionalDetails)
        {
            // add dropped gallons in case of rebill invoice and substract total dropped gallons in case of credit invoice
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
                            var dropAdditionalDetail = additionalDetails.FirstOrDefault(t => t.OrderId == invoice.OrderId);
                            var item = new CumulationQuantityUpdateViewModel();
                            item.DroppedGallons = invoice.DroppedGallons;
                            item.RequestPriceDetailsId = dropAdditionalDetail.RequestPriceDetailId;
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
                LogManager.Logger.WriteException("InvoiceBaseDomain", "CreateListOfCumulationQuantityUpdateForRebillInv", ex.Message, ex);
            }
            return responseList;
        }

        public DateTimeOffset GetPaymentDueDateForMarineInvoice(int paymentTermId, DateTimeOffset paymentDueDate, DateTimeOffset lastInvDropEndDate, DateTimeOffset firstInvCreateDate, int companyId)
        {
            if (paymentDueDate != null && lastInvDropEndDate != null && firstInvCreateDate != null)
            {
                var paymentDueDateType = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == companyId && t.IsActive).Select(t => t.PaymentDueDateType).FirstOrDefault();
                if (paymentTermId == (int)PaymentTerms.Net30)
                {
                    if (paymentDueDateType == PaymentDueDateType.InvoiceCreationDate)
                        paymentDueDate = firstInvCreateDate.Date.AddDays(30);
                    else if (paymentDueDateType == PaymentDueDateType.DeliveryDate)
                        paymentDueDate = lastInvDropEndDate.Date.AddDays(30);
                }
            }

            return paymentDueDate;
        }

        public void SetDecimalPlacesForNetGrossDrop(InvoiceModel invoice)
        {
            if (invoice != null)
            {
                invoice.DroppedGallons = Math.Round(invoice.DroppedGallons, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);

                invoice.BolDetails.ForEach((t) =>
                {
                    if (t.NetQuantity != null)
                        t.NetQuantity = Math.Round(t.NetQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                    if (t.GrossQuantity != null)
                        t.GrossQuantity = Math.Round(t.GrossQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                });

                if (invoice.ConvertedPricing != null)
                    invoice.ConvertedPricing = Math.Round(invoice.ConvertedPricing.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                if (invoice.ConvertedQuantity != null)
                    invoice.ConvertedQuantity = Math.Round(invoice.ConvertedQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);

                if (invoice.UoM == UoM.MetricTons)
                {
                    invoice.BolDetails.ForEach((t) =>
                    {
                        t.PricePerGallon = Math.Round(t.PricePerGallon, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                    });
                }
                else
                {
                    invoice.BolDetails.ForEach((t) =>
                    {
                        t.PricePerGallon = Math.Round(t.PricePerGallon, ApplicationConstants.InvoicePricePerGallonDecimalDisplay);
                    });
                }
            }
        }

        public void SetDecimalPlacesForNetGrossDropAndFees(InvoiceViewModelNew createInvoiceViewModel, UoM invUom)
        {
            if (createInvoiceViewModel != null)
            {
                createInvoiceViewModel.BolDetails.ForEach((b) =>
                {
                    b.Products.ForEach((t) =>
                    {
                        if (t.NetQuantity != null)
                            t.NetQuantity = Math.Round(t.NetQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                        if (t.GrossQuantity != null)
                            t.GrossQuantity = Math.Round(t.GrossQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                        if (t.DeliveredQuantity != null)
                            t.DeliveredQuantity = Math.Round(t.DeliveredQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                    });
                });

                createInvoiceViewModel.TicketDetails.ForEach((b) =>
                {
                    b.Products.ForEach((t) =>
                    {
                        if (t.NetQuantity != null)
                            t.NetQuantity = Math.Round(t.NetQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                        if (t.GrossQuantity != null)
                            t.GrossQuantity = Math.Round(t.GrossQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                        if (t.DeliveredQuantity != null)
                            t.DeliveredQuantity = Math.Round(t.DeliveredQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                    });
                });

                createInvoiceViewModel.Fees.ForEach((t) =>
                {
                    if (t.Fee != null)
                        t.Fee = Math.Round(t.Fee.Value, ApplicationConstants.InvoiceFeeUnitPriceDecimalDisplay);

                    t.TotalFee = Math.Round(t.TotalFee, ApplicationConstants.InvoiceFeeUnitPriceDecimalDisplay);
                });

                foreach (var invoice in createInvoiceViewModel.Drops)
                {
                    invoice.ActualDropQuantity = Math.Round(invoice.ActualDropQuantity, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);

                    if (invoice.ConvertedQuantity != null)
                        invoice.ConvertedQuantity = Math.Round(invoice.ConvertedQuantity.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);

                    invoice.Assets.ForEach((t) =>
                    {
                        if (t.DropGallons != null)
                            t.DropGallons = Math.Round(t.DropGallons.Value, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);

                        if (invUom == UoM.MetricTons)
                        {
                            t.PricePerGallon = Math.Round(t.PricePerGallon, ApplicationConstants.MarineInvoicePricePerUnitDecimalDisplay);
                        }
                        else
                        {
                            t.PricePerGallon = Math.Round(t.PricePerGallon, ApplicationConstants.InvoicePricePerGallonDecimalDisplay);
                        }
                    });
                }
            }
        }
        private async Task ValidateEBOLDetails(InvoiceModel invoiceModel)
        {
            List<EBolViewModel> ebolDetails = new List<EBolViewModel>();
            List<EBolAPIRequestDetails> eBolAPIRequestDetails = new List<EBolAPIRequestDetails>();
            var ebOLEnabled = Context.DataContext.OnboardingPreferences.Any(x => x.CompanyId == invoiceModel.SupplierCompanyId && x.IsActive == true && x.IsEbolWorkflowEnabled == true);
            if (ebOLEnabled)
            {
                foreach (var item in invoiceModel.BolDetails)
                {
                    if (item.TerminalId != null)
                    {
                        eBolAPIRequestDetails.Add(new EBolAPIRequestDetails { BOLNumber = item.BolNumber, TerminalId = item.TerminalId.Value, FuelTypeId = item.FuelTypeId, InvoiceDate = item.LiftDate != null ? item.LiftDate.GetValueOrDefault().Date : invoiceModel.DropEndDate.Date });
                    }
                }
                if (eBolAPIRequestDetails.Any())
                {
                    ebolDetails = await GetEBolDetails(eBolAPIRequestDetails);
                }
                if (ebolDetails.Any())
                {
                    foreach (var invoiceBOLDetails in invoiceModel.BolDetails)
                    {
                        var eBOLRecordStatus = ebolDetails.FirstOrDefault(x => x.BOLNumber == invoiceBOLDetails.BolNumber && x.TerminalId == invoiceBOLDetails.TerminalId);
                        if (eBOLRecordStatus != null)
                        {
                            if (invoiceBOLDetails.GrossQuantity != eBOLRecordStatus.GrossGallons || invoiceBOLDetails.NetQuantity != eBOLRecordStatus.NetGallons)
                            {
                                invoiceBOLDetails.EbolMatchStatus = EbolMatchStatus.Override;
                                invoiceBOLDetails.GrossQuantity = eBOLRecordStatus.GrossGallons;
                                invoiceBOLDetails.NetQuantity = eBOLRecordStatus.NetGallons;
                            }
                            else
                            {
                                invoiceBOLDetails.EbolMatchStatus = EbolMatchStatus.Match;
                                invoiceBOLDetails.GrossQuantity = eBOLRecordStatus.GrossGallons;
                                invoiceBOLDetails.NetQuantity = eBOLRecordStatus.NetGallons;
                            }
                        }
                        else
                        {
                            invoiceBOLDetails.EbolMatchStatus = EbolMatchStatus.NoMatch;
                        }
                    }
                }
                else
                {
                    invoiceModel.BolDetails.ForEach(x => x.EbolMatchStatus = EbolMatchStatus.NoMatch);
                }
            }
        }
        public async Task<List<EBolViewModel>> GetEBolDetails(List<EBolAPIRequestDetails> eBOLRequest)
        {
            var response = new List<EBolViewModel>();
            try
            {
                if (eBOLRequest != null && eBOLRequest.Any())
                {
                    foreach (var item in eBOLRequest)
                    {
                        var controlNumber = Context.DataContext.MstExternalTerminals.Where(t => t.Id == item.TerminalId && t.IsActive).Select(t => t.ControlNumber).FirstOrDefault();
                        if (!string.IsNullOrEmpty(controlNumber))
                        {
                            string eBOLProductCode = string.Empty;
                            string eBOLProductDescription = string.Empty;

                            var eBOLProductMapping = await Context.DataContext.EBOLProductMappings.FirstOrDefaultAsync(x => x.ProductId == item.FuelTypeId && x.IsActive && !x.IsDeleted);
                            if (eBOLProductMapping != null)
                            {
                                eBOLProductCode = eBOLProductMapping.EBOLProductCode;
                                eBOLProductDescription = eBOLProductMapping.EBOLProductDescription;
                            }
                            //first match with BOL Number , Terminal Number , Load Date , ProductId
                            var eBolMatchDetailsDetails = await Context.DataContext.EBolDetails.Where(t => t.BOLNumber == item.BOLNumber && t.TerminalName == controlNumber && t.ProductCode == eBOLProductCode && t.ProductDescription == eBOLProductDescription ).OrderByDescending(x => x.Id).ToListAsync();
                            var eBolMatchDetails = eBolMatchDetailsDetails.Where(x => x.StartLoadDate != null && x.StartLoadDate.Value.Date == item.InvoiceDate).OrderByDescending(x => x.Id).FirstOrDefault();
                            //second match with BOL Number , Terminal Number , ProductId
                            if (eBolMatchDetails == null)
                            {
                                eBolMatchDetails = eBolMatchDetailsDetails.OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            //thrid match with BOL Number , Terminal Number 
                            if (eBolMatchDetails == null)
                            {
                                eBolMatchDetails = await Context.DataContext.EBolDetails.Where(t => t.BOLNumber == item.BOLNumber && t.TerminalName == controlNumber).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                            }
                            if (eBolMatchDetails != null)
                            {
                                //eBOL Details
                                EBOLMappingDetails(response, item, eBolMatchDetails);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "GetEBolDetails", ex.Message, ex);
            }
            return response;
        }

        private static void EBOLMappingDetails(List<EBolViewModel> response, EBolAPIRequestDetails item, EBolDetails ebolFirstMatchDetails)
        {
            var eBolViewModel = new EBolViewModel();
            eBolViewModel.TerminalName = ebolFirstMatchDetails.TerminalName;
            eBolViewModel.BOLNumber = ebolFirstMatchDetails.BOLNumber;
            eBolViewModel.GrossGallons = ebolFirstMatchDetails.GrossGallons;
            eBolViewModel.NetGallons = ebolFirstMatchDetails.NetGallons;
            eBolViewModel.TerminalId = item.TerminalId;
            response.Add(eBolViewModel);
        }
    }
}
