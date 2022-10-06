using SiteFuel.Exchange.Core;
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

namespace SiteFuel.Exchange.Domain
{
    public class ConsolidatedDdtDomain : InvoiceBaseDomain
    {
        public ConsolidatedDdtDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ConsolidatedDdtDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task ProcessConsolidatedDdtCreation()
        {
            try
            {
                var appDomain = ContextFactory.Current.GetDomain<ApplicationDomain>();
                var ddtConversionPeriod = appDomain.GetKeySettingValue(ApplicationConstants.KeyAppSettingMFNDdtConversionPeriod, 2);
                var ddtConversionFallbackPeriod = appDomain.GetKeySettingValue(ApplicationConstants.KeyAppSettingMFNDdtConversionFallbackPeriod, 6);
                var scheduleStatusesToSkip = GetScheduleStatusToSkip();

                //int ddtConversionPeriod
                var spDomain = ContextFactory.Current.GetDomain<StoredProcedureDomain>();
                var draftDDTs = await spDomain.GetMgnDraftDDTs();
                var consolidatedDDTs = draftDDTs.GroupBy(t => new { t.GroupParentDRId }, (key, g) => new { GroupKey = key, DDTs = g.ToList() }).ToList();
                foreach (var item in consolidatedDDTs)
                {
                    if (!item.DDTs.Any())
                        continue;

                    var timeZoneName = item.DDTs.Select(t => t.TimeZoneName).First();
                    var jobDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
                    var endTime2Hours = jobDateTime.AddSeconds(-DateTimeOffset.Now.Second).AddMilliseconds(-DateTimeOffset.Now.Millisecond);
                    
                    var startTime6Hours = endTime2Hours.AddHours(-ddtConversionFallbackPeriod);

                    
                    var groupDDTs = item.DDTs.OrderBy(t => t.CreatedDate).ToList();
                    if (groupDDTs.Where(t => t.CreatedDate.HasValue).All(t => t.CreatedDate <= startTime6Hours))
                    {
                        var ddts6HrsOld = groupDDTs.Where(t => t.InvoiceId.HasValue).Where(t => t.CreatedDate <= startTime6Hours).ToList();
                        var groupDDTids = ddts6HrsOld.Select(t => t.InvoiceId.Value).ToList();
                        if (groupDDTids.Any())
                        {
                            var result = await ConvertToConsolidatedDDTs(groupDDTids);
                            LogManager.Logger.WriteException("ConsolidatedDdtDomain", "ProcessConsolidatedDdtCreation", $"Consolidated DDT has been created for {string.Join(",", groupDDTids)} draft DDTs (6 Hrs Job)", new Exception($"(6 Hrs Job)"));
                        }
                    }
                    else if (!groupDDTs.Any(t => scheduleStatusesToSkip.Contains(t.DeliveryScheduleStatusId)))
                    {
                        var groupDDTids = groupDDTs.Where(t => t.InvoiceId.HasValue).Select(t => t.InvoiceId.Value).ToList();
                        if (groupDDTids.Any())
                        {
                            var result = await ConvertToConsolidatedDDTs(groupDDTids);
                            LogManager.Logger.WriteException("ConsolidatedDdtDomain", "ProcessConsolidatedDdtCreation", $"Consolidated DDT has been created for {string.Join(",", groupDDTids)} draft DDTs (1 Hrs Job)", new Exception($"(1 Hrs Job)"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedDdtDomain", "ProcessConsolidatedDdtCreation", ex.Message, ex);
            }
        }

        public async Task<int> ConvertToConsolidatedDDTs(List<int> draftDDTs)
        {
            int generatedInvoices = 0;
            try
            {
                var draftDdtList = Context.DataContext.Invoices
                                    .Where(t1 => draftDDTs.Contains(t1.Id) && t1.WaitingFor == (int)WaitingAction.NextMarineDrop
                                            && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t1.IsActive)
                                    .Select(t2 => new DdtWatingActionDetailModel()
                                    {
                                        DdtId = t2.Id,
                                        RequestPriceDetailId = t2.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                                        ProductCode = t2.Order.FuelRequest.MstProduct.ProductCode,
                                        ProductTypeId = t2.Order.FuelRequest.MstProduct.ProductTypeId,
                                        CreatedBy = t2.CreatedBy,
                                        AcceptedCompanyId = t2.Order.AcceptedCompanyId,
                                        JobId = t2.Order.FuelRequest.JobId,
                                        BuyerCompanyName = t2.Order.FuelRequest.User.Company.Name,
                                        SupplierCompanyName = t2.User.Company.Name,
                                        BuyerCompanyId = t2.Order.BuyerCompanyId,
                                        FirstName = t2.User.FirstName,
                                        LastName = t2.User.LastName,
                                        DisplayInvoiceNumber = t2.DisplayInvoiceNumber,
                                        InvoiceHeaderId = t2.InvoiceHeaderId,
                                        TimeZoneName = t2.Order.FuelRequest.Job.TimeZoneName,
                                    }).OrderBy(t => t.InvoiceHeaderId).ToList();

                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                var ddtToInvoiceDomain = new ConsolidatedDdtToInvoiceDomain(this);

                List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
                List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>();
                if (draftDdtList.Any())
                {
                    var newsfeedParms = draftDdtList.FirstOrDefault();
                    var originalDdtNumber = newsfeedParms?.DisplayInvoiceNumber;

                    foreach (var ddtItem in draftDdtList)
                    {
                        var originalDdt = Context.DataContext.Invoices.Where(t => t.Id == ddtItem.DdtId && t.InvoiceHeaderId == ddtItem.InvoiceHeaderId).SingleOrDefault();
                        if (originalDdt != null)
                        {
                            InvoiceModel invoiceModel = ddtToInvoiceDomain.GetInvoiceModel(originalDdt, ddtItem.ProductCode, ddtItem.ProductTypeId, ddtItem.TimeZoneName);
                            priceRequestModels.AddRange(ddtToInvoiceDomain.SetPriceRequestDetails(invoiceModel.BolDetails, ddtItem.RequestPriceDetailId, invoiceModel));
                            invoiceModelList.Add(invoiceModel);
                        }
                    }

                    if (invoiceModelList.Any())
                    {
                        //convert DDTs to consoldated DDT

                        //int invoiceHeaderId = draftDdtList.FirstOrDefault().InvoiceHeaderId;
                        //var result = await ddtToInvoiceDomain.ProcessDdtList(invoiceModelList, invoiceHeaderId);
                        var result = await ProcessDdtList(invoiceModelList, priceRequestModels);
                        if (result.StatusCode == Status.Success)
                        {
                            generatedInvoices += invoiceModelList.Count;
                            if (newsfeedParms != null)
                            {
                                var invoiceModelFirst = invoiceModelList.First();
                                var usercontext = new UserContext() { Id = newsfeedParms.CreatedBy, CompanyName = newsfeedParms.SupplierCompanyName, CompanyId = newsfeedParms.AcceptedCompanyId, Name = $"{newsfeedParms.FirstName} {newsfeedParms.LastName}" };
                                ConversionNewsfeedViewModel conversionnewsfeedmodel = ddtToInvoiceDomain.GetConversionNewsfeedViewModel(invoiceModelFirst, newsfeedParms.JobId, newsfeedParms.AcceptedCompanyId, newsfeedParms.BuyerCompanyName, newsfeedParms.SupplierCompanyName, newsfeedParms.BuyerCompanyId);
                                await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed_New(usercontext, conversionnewsfeedmodel, originalDdtNumber);

                                var notificationDomain = new NotificationDomain(this);
                                await notificationDomain.AddNotificationEventAsync(EventType.CreateInvoiceFromDDT, invoiceModelFirst.InvoiceHeaderId, newsfeedParms.CreatedBy);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedDdtDomain", "ConvertToConsolidatedDDTs", ex.Message, ex);
            }
            return generatedInvoices;
        }

        private void SetWaitingForAction(List<InvoiceModel> invoices)
        {
            var invoiceCommonDomain = new InvoiceCommonDomain(this);
            foreach (var invoice in invoices)
            {
                invoice.WaitingFor = invoiceCommonDomain.IsWaitingForImage(invoice) ? WaitingAction.Images : WaitingAction.Nothing;

                if (invoice.WaitingFor != WaitingAction.Nothing || invoice.StatusId == (int)InvoiceStatus.Draft)
                {
                    invoice.InvoiceTypeId = InvoiceCommonDomain.GetInvoiceCreationTypeToDdt(invoice.InvoiceTypeId);
                }
            }
        }

        private async Task<StatusViewModel> ProcessDdtList(List<InvoiceModel> invoiceModelList, List<FuelPriceRequestModel> priceRequestModels)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            var ddtToInvoiceDomain = new ConsolidatedDdtToInvoiceDomain(this);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var ddtInvoiceList = 1;
                    bool isUpdateOnlyPriceAndWaitingAction = false;
                    var invoiceNumber = await GenerateInvoiceNumber_New();
                    

                    // check for waiting for images
                    SetWaitingForAction(invoiceModelList);

                    // get pricing
                    if (invoiceModelList.All(t => t.WaitingFor == WaitingAction.Nothing))
                    {
                        await ddtToInvoiceDomain.GetPriceDetails(priceRequestModels, invoiceModelList);
                        if (invoiceModelList.Any(t => t.WaitingFor == WaitingAction.UpdatedPrice))
                        {
                            isUpdateOnlyPriceAndWaitingAction = true;
                            var invNumberToLog = invoiceModelList.Where(t => t.WaitingFor == WaitingAction.UpdatedPrice).Select(t => t.DisplayInvoiceNumber).ToList();
                            var numbers = string.Join(",", invNumberToLog);
                            //do not convert any invoice of this header
                            LogManager.Logger.WriteDebug("ConsolidatedDdtDomain", "ConvertToConsolidatedDDTs", $"{numbers} are still waiting for price");
                        }
                    }

                    // get avalara taxes
                    foreach (var invoiceModel in invoiceModelList)
                    {
                        if (invoiceModel.WaitingFor == WaitingAction.Nothing && invoiceModel.SupplierPreferredInvoiceTypeId != null && !IsDigitalDropTicket(invoiceModel.SupplierPreferredInvoiceTypeId.Value))
                        {
                            if (ddtInvoiceList == 1)
                            {
                                if (string.IsNullOrEmpty(invoiceModel.FuelProductCode) || invoiceModel.TypeOfFuel == (int)ProductTypes.NonStandardFuel)
                                {
                                    ddtInvoiceList = ddtInvoiceList - 1;
                                }
                                else
                                {
                                    if (invoiceModelList.Any(t => string.IsNullOrWhiteSpace(t.ReferenceId))) //IF SUPPLIER INV IS PROVIDED WE ARE SAVING REFERENCEID
                                        invoiceModelList.ForEach(t => t.DisplayInvoiceNumber = invoiceNumber.Number);
                                    
                                    foreach (var item in invoiceModelList)
                                    {
                                        //Already added IncludeInppg at Fee calculation method
                                        if (item.FuelFees.Any(t => t.IncludeInPPG))
                                        {
                                            item.BasicAmount += item.FuelFees.Where(t => t.IncludeInPPG).Select(t => t.TotalFee ?? 0).Sum();
                                            item.PricePerGallon = item.BasicAmount / item.DroppedGallons;
                                            item.BolDetails.ForEach(t => t.PricePerGallon = item.PricePerGallon);
                                        }
                                    }

                                    TaxResponseViewModel taxResponse = ddtToInvoiceDomain.SetConsolidatedAvalaraTaxes(invoiceModelList);
                                    if (invoiceModelList.Any(t => t.TypeOfFuel != (int)ProductTypes.NonStandardFuel)
                                                            && invoiceModelList.Where(t => t.TypeOfFuel != (int)ProductTypes.NonStandardFuel)
                                                        .Sum(t => t.TotalTaxAmount) <= 0
                                        && taxResponse.StatusCode != Status.Success)
                                    {
                                        isUpdateOnlyPriceAndWaitingAction = true;
                                        var invoiceTypeId = InvoiceCommonDomain.GetInvoiceCreationTypeToDdt(invoiceModel.InvoiceTypeId);
                                        invoiceModelList.ForEach(t => { t.WaitingFor = WaitingAction.AvalaraTax; t.InvoiceTypeId = invoiceTypeId; t.DDTConversionReason = taxResponse.FailedStatusCode; });
                                        LogManager.Logger.WriteException("", "ProcessDdtList", "avalara failed or taxes are not available while converting ddt to invoice and displayInvoiceNumber:" + invoiceModel.DisplayInvoiceNumber, new Exception());
                                    }
                                }
                            }                            
                        }
                        ddtInvoiceList++;

                        await UpdateExistingDdtToInactive(invoiceModel);
                    }

                    if (invoiceModelList.Any(t => t.WaitingFor != WaitingAction.Nothing || (t.SupplierPreferredInvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || t.SupplierPreferredInvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)))
                    {
                        isUpdateOnlyPriceAndWaitingAction = true;
                        var invoiceTypeId = invoiceModelList.First().InvoiceTypeId;
                        invoiceTypeId = InvoiceCommonDomain.GetInvoiceCreationTypeToDdt(invoiceTypeId);
                        invoiceModelList.ForEach(t => { t.InvoiceTypeId = invoiceTypeId; });
                    }

                    var newHeader = await AddNewInvoiceHeader(invoiceModelList, invoiceNumber.Id);
                    if(isUpdateOnlyPriceAndWaitingAction)
                        await AddInvoicesForHeader(newHeader, invoiceModelList);
                    else
                        await ddtToInvoiceDomain.AddInvoicesForHeader(newHeader, invoiceModelList);

                    var existingHeaders = invoiceModelList.Select(t => t.InvoiceHeaderId).ToList();
                    await UpdateExistingInvoiceHeader(existingHeaders, newHeader);
                    response.StatusCode = Status.Success;
                    invoiceModelList.ForEach(t => t.InvoiceHeaderId = newHeader.Id);

                    await Context.CommitAsync();
                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageDdtUpdatedSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageFaildToUpdateDDT;
                    transaction.Rollback();
                    var existingHeaders = string.Join(",", invoiceModelList.Select(t => t.InvoiceHeaderId));
                    LogManager.Logger.WriteException("ConsolidatedDdtDomain", "ProcessDdtList", $"{ex.Message}, InvoiceHeader - {existingHeaders}", ex);
                }
            }
            return response;
        }

        private async Task UpdateExistingDdtToInactive(InvoiceModel invoiceModel)
        {
            var ddtDetails = Context.DataContext.Invoices.SingleOrDefault(t => t.Id == invoiceModel.Id);
            if (ddtDetails != null)
            {
                ddtDetails.IsActive = false;
                ddtDetails.WaitingFor = (int)WaitingAction.Nothing;
                ddtDetails.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;
                if (!string.IsNullOrWhiteSpace(ddtDetails.ReferenceId))
                    ddtDetails.DisplayInvoiceNumber = ddtDetails.ReferenceId;

                Context.DataContext.Entry(ddtDetails).State = EntityState.Modified;
                await Context.CommitAsync();
            }
        }

        private async Task<InvoiceHeaderDetail> AddNewInvoiceHeader(List<InvoiceModel> invoices, int invoiceNumberId)
        {
            var invoice = invoices.First();

            InvoiceHeaderDetail entity = new InvoiceHeaderDetail();
            entity.InvoiceNumberId = invoiceNumberId;
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
            await Context.CommitAsync();
            return entity;
        }

        private async Task AddInvoicesForHeader(InvoiceHeaderDetail newinvoiceHeader, List<InvoiceModel> invoiceModelList)
        {
            foreach (var invoiceModel in invoiceModelList)
            {
                //invoiceModel.WaitingFor = WaitingAction.Nothing;
                invoiceModel.InvoiceNumberId = newinvoiceHeader.InvoiceNumberId;

                if (string.IsNullOrWhiteSpace(invoiceModel.ReferenceId))
                    invoiceModel.DisplayInvoiceNumber = newinvoiceHeader.InvoiceNumber.Number;

                SetInvoiceNumber(newinvoiceHeader.InvoiceNumber.Number, invoiceModel);

                //generated invoice from ddt
                var invoice = invoiceModel.ToEntity();
                var assetDrops = invoiceModel.AssetDrops.Select(t => t.ToEntity()).ToList();
                invoice.AssetDrops = assetDrops;
                newinvoiceHeader.Invoices.Add(invoice);
                await Context.CommitAsync();

                await SetInvoiceBolDetails(newinvoiceHeader, invoiceModel, invoice);
                await SaveBulkPlantLocations(invoiceModel.BolDetails);
            }
            await Context.CommitAsync();
        }

        #region Group DR consolidation

        public async Task<bool> TriggerGroupDrConsolidationProcess()
        {
            var groupDRList = await Context.DataContext.Invoices.Where(t => t.WaitingFor == (int)WaitingAction.AllDRCompletion &&
                                                                      t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active &&
                                                                      t.GroupParentDrId != null
                                                                ).Select(t => t.GroupParentDrId).Distinct().ToListAsync();

            foreach (var item in groupDRList)
            {
                var response = ProcessConsolidationForGroupParentDr(new GroupDrInvoiceCreationViewModelForQueueService() { GroupParentDrId = item }, new List<string>());
                if (response != null && response.StatusCode == Status.Failed)
                    LogManager.Logger.WriteDebug("ConsolidatedDdtDomain", "TriggerGroupDrConsolidationProcess", "Failed to consolidate for GroupParentDrId: " + item);
            }
            return true;
        }


        public StatusViewModel ProcessConsolidationForGroupParentDr(GroupDrInvoiceCreationViewModelForQueueService groupDrInvoiceCreationView, List<string> errorInfo)
        {
            StatusViewModel statusViewModel = null;
            try
            {
                if (groupDrInvoiceCreationView.CompanyId > 0)
                {
                    if (!string.IsNullOrWhiteSpace(groupDrInvoiceCreationView.GroupParentDrId))
                    {
                        var deliveryStatussShouldNotbe = GetScheduleStatusToSkip();

                        var groupDrList = Context.DataContext.DeliveryScheduleXTrackableSchedules
                                            .Where(t => t.GroupParentDRId == groupDrInvoiceCreationView.GroupParentDrId)
                                            .Select(t => new { t.DeliveryScheduleStatusId, t.Id }).ToList();

                        if (!groupDrList.Any(t => deliveryStatussShouldNotbe.Contains(t.DeliveryScheduleStatusId)))
                        {
                            statusViewModel = new StatusViewModel();
                            var ddtToInvoiceDomain = new ConsolidatedDdtToInvoiceDomain(this);
                            statusViewModel = Task.Run(() => ddtToInvoiceDomain.ConsolidateGroupDrInvoices(groupDrInvoiceCreationView.GroupParentDrId, groupDrInvoiceCreationView.CompanyId, 0, groupDrInvoiceCreationView.IsProcessWithoutTax)).Result;

                            if (statusViewModel.StatusCode == Status.Failed)
                                errorInfo.Add(statusViewModel.StatusMessage);
                        }
                    }
                    else if (groupDrInvoiceCreationView.InvoiceHeaderId > 0)
                    {
                        statusViewModel = new StatusViewModel();
                        var ddtToInvoiceDomain = new ConsolidatedDdtToInvoiceDomain(this);
                        statusViewModel = Task.Run(() => ddtToInvoiceDomain.ConsolidateGroupDrInvoices(string.Empty, groupDrInvoiceCreationView.CompanyId, groupDrInvoiceCreationView.InvoiceHeaderId, groupDrInvoiceCreationView.IsProcessWithoutTax)).Result;

                        if (statusViewModel.StatusCode == Status.Failed)
                            errorInfo.Add(statusViewModel.StatusMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedDdtDomain", "ProcessConsolidationForGroupParentDr - GroupParentDrId: ", ex.Message, ex);
            }

            return statusViewModel;
        }
        #endregion

        private List<int> GetScheduleStatusToSkip()
        {
            return new List<int>()
            {
                (int)DeliveryScheduleStatus.Accepted,
                (int)DeliveryScheduleStatus.Acknowledged,
                (int)DeliveryScheduleStatus.Assigned,
                (int)DeliveryScheduleStatus.Missed,
                (int)DeliveryScheduleStatus.MissedAndRescheduled,
                (int)DeliveryScheduleStatus.Modified,
                (int)DeliveryScheduleStatus.New,
                (int)DeliveryScheduleStatus.Pending,
                (int)DeliveryScheduleStatus.Reassigned,
                (int)DeliveryScheduleStatus.Rescheduled,
                (int)DeliveryScheduleStatus.RescheduledMissed
            };
        }
    }
}
