using ExcelDataReader;
using Newtonsoft.Json;
//using Renci.SshNet;
//using Renci.SshNet.Sftp;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Domain.Domain.ThirdParty;

namespace SiteFuel.Exchange.Domain
{

    public class ConsolidatedDdtToInvoiceDomain : InvoiceCommonDomain
    {
        public ConsolidatedDdtToInvoiceDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ConsolidatedDdtToInvoiceDomain(BaseDomain domain)
            : base(domain)
        {
        }

        #region WaitingFor UpdatedPrice DDT

        /// <summary>
        /// Common method for AXXIS, OPIS AND PLATS
        /// </summary>
        /// <returns></returns>
        public async Task<int> ProcessInvoicesWaitingForUpdatedPrice()
        {
            int generatedInvoices = 0;
            try
            {
                var waitingForPriceDdtList = Context.DataContext.Invoices.
                                                        Where(t1 => t1.WaitingFor == (int)WaitingAction.UpdatedPrice
                                                        && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                        && (t1.SupplierPreferredInvoiceTypeId.HasValue
                                                                && (t1.SupplierPreferredInvoiceTypeId.Value != (int)InvoiceType.DigitalDropTicketManual
                                                                    || t1.SupplierPreferredInvoiceTypeId.Value != (int)InvoiceType.DigitalDropTicketMobileApp))
                                                        && t1.IsActive)
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
                                                        }).ToList().GroupBy(t => t.InvoiceHeaderId).OrderBy(t => t.Select(t1 => t1.InvoiceHeaderId).FirstOrDefault()).ToList();

                //iterate header list and process each ddt
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);

                foreach (var invoiceHeader in waitingForPriceDdtList)
                {
                    List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
                    List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>();

                    var ddtList = invoiceHeader.ToList();
                    if (ddtList.Any())
                    {
                        var newsfeedParms = ddtList.FirstOrDefault();
                        var originalDdtNumber = newsfeedParms?.DisplayInvoiceNumber;

                        foreach (var ddtItem in ddtList)
                        {
                            var originalDdt = Context.DataContext.Invoices.Where(t => t.Id == ddtItem.DdtId && t.InvoiceHeaderId == invoiceHeader.Key).SingleOrDefault();
                            if (originalDdt != null)
                            {
                                InvoiceModel invoiceModel = GetInvoiceModel(originalDdt, ddtItem.ProductCode, ddtItem.ProductTypeId, ddtItem.TimeZoneName);
                                priceRequestModels.AddRange(SetPriceRequestDetails(invoiceModel.BolDetails, ddtItem.RequestPriceDetailId, invoiceModel));
                                invoiceModelList.Add(invoiceModel);
                            }
                        }

                        if (invoiceModelList.Any())
                        {
                            await GetPriceDetails(priceRequestModels, invoiceModelList);
                            if (invoiceModelList.Any(t => t.WaitingFor == WaitingAction.UpdatedPrice))
                            {
                                var invNumberToLog = invoiceModelList.Where(t => t.WaitingFor == WaitingAction.UpdatedPrice).Select(t => t.DisplayInvoiceNumber).ToList();
                                var numbers = string.Join(",", invNumberToLog);
                                //do not convert any invoice of this header
                                LogManager.Logger.WriteDebug("ConsolidatedDdtToInvoiceDomain", "ProcessInvoicesWaitingForUpdatedPrice", $"{numbers} are still waiting for price");
                                continue;
                            }
                            else
                            {
                                //convert ddt to invoice
                                var result = await ProcessDdtList(invoiceModelList, invoiceHeader.Key);
                                if (result.StatusCode == Status.Success)
                                {
                                    generatedInvoices += invoiceModelList.Count;
                                    if (newsfeedParms != null)
                                    {
                                        var invoiceModelFirst = invoiceModelList.First();
                                        var usercontext = new UserContext() { Id = newsfeedParms.CreatedBy, CompanyName = newsfeedParms.SupplierCompanyName, CompanyId = newsfeedParms.AcceptedCompanyId, Name = $"{newsfeedParms.FirstName} {newsfeedParms.LastName}" };
                                        ConversionNewsfeedViewModel conversionnewsfeedmodel = GetConversionNewsfeedViewModel(invoiceModelFirst, newsfeedParms.JobId, newsfeedParms.AcceptedCompanyId, newsfeedParms.BuyerCompanyName, newsfeedParms.SupplierCompanyName, newsfeedParms.BuyerCompanyId);
                                        await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed_New(usercontext, conversionnewsfeedmodel, originalDdtNumber);

                                        var notificationDomain = new NotificationDomain(this);
                                        await notificationDomain.AddNotificationEventAsync(EventType.CreateInvoiceFromDDT, invoiceModelFirst.InvoiceHeaderId, newsfeedParms.CreatedBy);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "ProcessInvoicesWaitingForUpdatedPrice", ex.Message, ex);
            }

            return generatedInvoices;
        }

        public ConversionNewsfeedViewModel GetConversionNewsfeedViewModel(InvoiceModel invoiceModelFirst, int jobId, int acceptedCompanyId, string buyerCompanyName, string supplierCompanyName, int buyerCompId)
        {
            ConversionNewsfeedViewModel conversionNewsfeed = invoiceModelFirst.ToNewsfeedViewModel();
            conversionNewsfeed.JobId = jobId;
            conversionNewsfeed.SupplierCompanyId = acceptedCompanyId;
            conversionNewsfeed.BuyerCompanyName = buyerCompanyName;
            conversionNewsfeed.BuyerCompanyId = buyerCompId;
            conversionNewsfeed.SupplierCompanyName = supplierCompanyName;
            return conversionNewsfeed;
        }

        public InvoiceModel GetInvoiceModel(Invoice originalDdt, string productCode, int productTypeId, string TimeZoneName)
        {
            InvoiceModel invoiceModel = GetInvoiceModelFromOriginalInvoice(originalDdt);
            invoiceModel.Version = invoiceModel.Version + 1;
            invoiceModel.ParentId = originalDdt.Id;
            invoiceModel.FuelProductCode = productCode;
            invoiceModel.TypeOfFuel = productTypeId;
            invoiceModel.UpdatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(TimeZoneName);
            return invoiceModel;
        }

        public async Task<StatusViewModel> ProcessDdtList(List<InvoiceModel> invoiceModelList, int invoiceHeaderId, bool isWaitingForTax = false, bool isFromGridEdit = false)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var existingHeader = Context.DataContext.InvoiceHeaderDetails.SingleOrDefault(t => t.Id == invoiceHeaderId);
                    if (existingHeader != null)
                    {

                        var invoiceNumber = await GenerateInvoiceNumber_New();
                        int ddtInvoiceList = 1;
                        bool isUpdateOnlyPriceAndWaitingAction = false;
                        string previousDisplayInvoiceNumber = invoiceModelList.First().DisplayInvoiceNumber;

                        response.EntityHeaderId = invoiceHeaderId;
                        foreach (var invoiceModel in invoiceModelList)
                        {
                            if (invoiceModel.SupplierPreferredInvoiceTypeId.HasValue && IsDigitalDropTicket(invoiceModel.SupplierPreferredInvoiceTypeId.Value))
                            {
                                //only update ppg and waiting for status
                                await UpdatePpgAndWaitingForExistingDdt(invoiceModel, WaitingAction.Nothing);
                                isUpdateOnlyPriceAndWaitingAction = true;
                                await SendInvoiceToPDI(invoiceModelList.First().Id);
                            }
                            else
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

                                        if (!isWaitingForTax)
                                        {
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
                                        }

                                        if (!isFromGridEdit)
                                        {
                                            if (invoiceModelList.Any(t => t.IsPdieTaxRequired))
                                            {
                                                invoiceModelList.ForEach(t => { t.WaitingFor = WaitingAction.PDITaxes; });
                                                isUpdateOnlyPriceAndWaitingAction = await RemoveNewInvoiceNumberFromModelList(invoiceModelList, invoiceHeaderId, invoiceNumber, previousDisplayInvoiceNumber, invoiceModel);
                                            }
                                            else
                                            {
                                                TaxResponseViewModel taxResponse = SetConsolidatedAvalaraTaxes(invoiceModelList);
                                                if (invoiceModelList.Any(t => t.TypeOfFuel != (int)ProductTypes.NonStandardFuel)
                                                                        && invoiceModelList.Where(t => t.TypeOfFuel != (int)ProductTypes.NonStandardFuel)
                                                                    .Sum(t => t.TotalTaxAmount) <= 0
                                                    && taxResponse.StatusCode != Status.Success)
                                                {
                                                    invoiceModelList.ForEach(t => { t.WaitingFor = WaitingAction.AvalaraTax; t.DDTConversionReason = taxResponse.FailedStatusCode; });
                                                    isUpdateOnlyPriceAndWaitingAction = await RemoveNewInvoiceNumberFromModelList(invoiceModelList, invoiceHeaderId, invoiceNumber, previousDisplayInvoiceNumber, invoiceModel);
                                                }
                                            }
                                        }
                                        else
                                            isUpdateOnlyPriceAndWaitingAction = await RemoveNewInvoiceNumberFromModelList(invoiceModelList, invoiceHeaderId, invoiceNumber, previousDisplayInvoiceNumber, invoiceModel);

                                    }
                                }

                                if (isUpdateOnlyPriceAndWaitingAction)
                                {
                                    await UpdatePpgAndWaitingForExistingDdt(invoiceModel, invoiceModelList.FirstOrDefault().WaitingFor);
                                }
                                else
                                {
                                    await UpdateExistingDdtToInactive(invoiceModel);
                                }
                            }
                            ddtInvoiceList++;
                        }

                        if (!isUpdateOnlyPriceAndWaitingAction)
                        {
                            var newHeader = await AddNewInvoiceHeader(invoiceModelList, existingHeader, invoiceNumber.Id);
                            await AddInvoicesForHeader(newHeader, invoiceModelList);
                            UpdateExistingInvoiceHeader(existingHeader, newHeader);
                            response.StatusCode = Status.Success;
                            invoiceModelList.ForEach(t => t.InvoiceHeaderId = newHeader.Id);
                            response.EntityHeaderId = newHeader.Id;
                        }
                        else
                        {
                            if (isFromGridEdit)
                            {
                                existingHeader.TotalDroppedGallons = invoiceModelList.Sum(t => t.DroppedGallons);
                                existingHeader.TotalBasicAmount = invoiceModelList.Sum(t => t.BasicAmount);
                                existingHeader.TotalFeeAmount = invoiceModelList.Sum(t => t.TotalFeeAmount ?? 0);
                                existingHeader.TotalTaxAmount = invoiceModelList.Sum(t => t.TotalTaxAmount);
                                existingHeader.TotalDiscountAmount = invoiceModelList.Sum(t => t.TotalDiscountAmount);
                                Context.DataContext.Entry(existingHeader).State = EntityState.Modified;
                            }
                        }

                        if (invoiceModelList.All(t => t.WaitingFor == WaitingAction.Nothing || t.WaitingFor == WaitingAction.PDITaxes))
                        {
                            await SendInvoiceToPDI(invoiceModelList.First().Id);
                        }


                        if (invoiceModelList.Count > 1)
                        {
                            var invoiceHeaderid = isUpdateOnlyPriceAndWaitingAction ? invoiceHeaderId : invoiceModelList.FirstOrDefault().InvoiceHeaderId;
                            await UpdateFlatFeesForMarineConsolidatedInvoices(invoiceHeaderid);
                        }

                        if (invoiceModelList.Any() && !isFromGridEdit)
                        {
                            int? orderId = invoiceModelList.Select(t => t.OrderId).FirstOrDefault();
                            if (orderId > 0)
                            {
                                var order = await Context.DataContext.Orders.FirstOrDefaultAsync(t => t.Id == orderId);
                                if (order.FuelRequest.Job.IsMarine && order.OrderXStatuses.Any(t => t.StatusId == (int)OrderStatus.Open && t.IsActive))
                                {
                                    AutoCloseOrderPostSave(order, null, out decimal orderTotalDelivered);
                                }
                            }
                        }
                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageDdtUpdatedSuccess;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageFaildToUpdateDDT;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "ProcessDDTList_New", $"{ex.Message}, InvoiceHeader - {invoiceHeaderId}", ex);
                }
            }
            return response;
        }

        private async Task<bool> RemoveNewInvoiceNumberFromModelList(List<InvoiceModel> invoiceModelList, int invoiceHeaderId, InvoiceNumber invoiceNumber, string previousDisplayInvoiceNumber, InvoiceModel invoiceModel)
        {
            bool isUpdateOnlyPriceAndWaitingAction = true;
            invoiceModelList.ForEach(t => t.DisplayInvoiceNumber = previousDisplayInvoiceNumber);
            Context.DataContext.InvoiceNumbers.Remove(invoiceNumber);
            await Context.CommitAsync();
            LogManager.Logger.WriteException("", "ProcessDdtList", "avalara failed or taxes are not available while converting ddt to invoice and invoiceheader is:" + invoiceHeaderId + " displayNumber:" + invoiceModel.DisplayInvoiceNumber, new Exception());
            return isUpdateOnlyPriceAndWaitingAction;
        }

        protected async Task UpdateFlatFeesForMarineConsolidatedInvoices(int invoiceHeaderId)
        {
            try
            {
                var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceHeaderId).ToList();
                if (invoices.Any())
                {
                    var flatFuelFees = new List<FuelFee>();
                    //remove flat fees from all invoices and sbstract total fee amount from each invoice
                    foreach (var invoice in invoices)
                    {
                        var flatfeesForInvoice = invoice.FuelRequestFees.Where(t => t.FeeSubTypeId == (int)FeeSubType.FlatFee && t.FeeTypeId != (int)FeeType.OtherFee).ToList();

                        if (flatfeesForInvoice != null && flatfeesForInvoice.Any())
                        {
                            flatFuelFees.AddRange(flatfeesForInvoice);

                            foreach (var item in flatfeesForInvoice)
                            {
                                Context.DataContext.FuelRequestFees.Remove(item);
                                Context.DataContext.Entry(item).State = EntityState.Modified;
                            }

                            var removedFeeAmount = flatfeesForInvoice.Sum(t => t.Fee);
                            invoice.TotalFeeAmount = invoice.TotalFeeAmount - removedFeeAmount;
                            // need to add logic for includeinppg fees

                            //if (flatfeesForInvoice.Any(t => t.IncludeInPPG))
                            //{
                            //    invoice.BasicAmount -= flatfeesForInvoice.Where(t => t.IncludeInPPG).Select(t => t.TotalFee ?? 0).Sum();
                            //    //invoice.PricePerGallon = invoice.BasicAmount / invoice.DroppedGallons;

                            //    //// update ppg in bol list
                            //    //invoice.BolDetails.ForEach(t => t.PricePerGallon = invoice.PricePerGallon);
                            //}

                            var flatOtherfeesForInvoice = invoice.FuelRequestFees.Where(t => t.FeeSubTypeId == (int)FeeSubType.FlatFee && t.FeeTypeId == (int)FeeType.OtherFee).ToList();

                            if (flatOtherfeesForInvoice != null && flatOtherfeesForInvoice.Any())
                            {
                                flatFuelFees.AddRange(flatOtherfeesForInvoice);

                                foreach (var otherfee in flatOtherfeesForInvoice)
                                {
                                    Context.DataContext.FuelRequestFees.Remove(otherfee);
                                    Context.DataContext.Entry(otherfee).State = EntityState.Modified;
                                }

                                var removedOtherFeeAmount = flatOtherfeesForInvoice.Sum(t => t.Fee);
                                invoice.TotalFeeAmount = invoice.TotalFeeAmount - removedOtherFeeAmount;
                            }

                            Context.DataContext.Entry(invoice).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }
                    }
                    //group all flat fees by FeeType and Take highest fee from each group
                    if (flatFuelFees.Any())
                    {
                        var groupedAllFees = flatFuelFees.GroupBy(t => new { t.FeeTypeId, t.FeeDetails }).ToList();
                        var finalFlatFees = new List<FuelFee>();
                        foreach (var groupItem in groupedAllFees)
                        {
                            var groupedFees = groupItem.ToList();
                            var fees = groupedFees.OrderByDescending(t => t.Fee).ToList();
                            var highestFee = fees.FirstOrDefault();
                            if (highestFee != null)
                            {
                                FuelFee entity = new FuelFee();
                                entity.FeeTypeId = highestFee.FeeTypeId;
                                entity.FeeSubTypeId = highestFee.FeeSubTypeId;
                                entity.MinimumGallons = highestFee.MinimumGallons;
                                entity.Fee = highestFee.Fee;
                                entity.FeeDetails = highestFee.FeeDetails;
                                entity.MarginTypeId = highestFee.MarginTypeId;
                                entity.Margin = highestFee.Margin;
                                entity.IncludeInPPG = highestFee.IncludeInPPG;
                                entity.FeeSubQuantity = highestFee.FeeSubQuantity;
                                entity.TotalFee = highestFee.TotalFee != null ? Math.Round(highestFee.TotalFee.Value, 2) : highestFee.TotalFee;
                                entity.OtherFeeTypeId = highestFee.OtherFeeTypeId;
                                entity.FeeConstraintTypeId = highestFee.FeeConstraintTypeId;
                                entity.SpecialDate = highestFee.SpecialDate;
                                entity.Currency = highestFee.Currency;
                                entity.UoM = highestFee.UoM;
                                entity.OfferPricingId = highestFee.OfferPricingId;
                                entity.DiscountLineItemId = highestFee.DiscountLineItemId;
                                entity.StartTime = highestFee.StartTime;
                                entity.EndTime = highestFee.EndTime;
                                entity.WaiveOffTime = highestFee.WaiveOffTime;

                                finalFlatFees.Add(entity);
                            }
                        }

                        // add all flat fees to last invoice
                        if (finalFlatFees != null && finalFlatFees.Any())
                        {
                            var invoice = invoices.FirstOrDefault();
                            if (invoice != null)
                            {
                                foreach (var fee in finalFlatFees)
                                {
                                    invoice.FuelRequestFees.Add(fee);
                                }
                                invoice.TotalFeeAmount = invoice.TotalFeeAmount + finalFlatFees.Sum(t => t.Fee);
                                Context.DataContext.Entry(invoice).State = EntityState.Modified;
                                await Context.CommitAsync();
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "UpdateFlatFeesForMarineConsolidatedInvoices", ex.Message, ex);
                throw; // transaction rolled back in calling function
            }

        }

        public async Task AddInvoicesForHeader(InvoiceHeaderDetail newinvoiceHeader, List<InvoiceModel> invoiceModelList, bool isSentToPDI = true)
        {
            Order dtnOrder = null; Invoice dtnInvoice = null; bool sendDtnFile = true;
            Invoice firstInvoice = null;
            foreach (var invoiceModel in invoiceModelList)
            {
                invoiceModel.WaitingFor = WaitingAction.Nothing;

                if (invoiceModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual)
                {
                    invoiceModel.InvoiceTypeId = (int)InvoiceType.Manual;
                }
                else if (invoiceModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    invoiceModel.InvoiceTypeId = (int)InvoiceType.MobileApp;
                }

                invoiceModel.InvoiceNumberId = newinvoiceHeader.InvoiceNumberId;

                if (string.IsNullOrWhiteSpace(invoiceModel.ReferenceId))
                    invoiceModel.DisplayInvoiceNumber = newinvoiceHeader.InvoiceNumber.Number;

                SetInvoiceNumber(newinvoiceHeader.InvoiceNumber.Number, invoiceModel);

                //generated invoice from ddt
                var invoice = invoiceModel.ToEntity();
                //var assetDrops = invoiceModel.AssetDrops.Select(t => t.ToEntity()).ToList();
                //invoice.AssetDrops = assetDrops;
                newinvoiceHeader.Invoices.Add(invoice);
                await Context.CommitAsync();

                var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == invoice.OrderId);
                if (order != null)
                {
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

                    if (order.BuyerCompanyId == order.FuelRequest.Job.CompanyId)
                    {
                        AddHedgeSpotAmountsOfInvoiceCreatedFromDDT(order, invoice, false);
                    }
                }
                await SetInvoiceBolDetails(newinvoiceHeader, invoiceModel, invoice);
                await SaveBulkPlantLocations(invoiceModel.BolDetails);
                LogManager.Logger.WriteDebug("DDTtoInvoiceCreation", "ProcessDDTList_New", $"DDT Id:{invoiceModel.Id} to Invoice conversion - {invoice.Id}");
            }

            if (sendDtnFile)
            {
                CreateDtnFileGenerationWorkflow(dtnInvoice, dtnOrder);
            }
            SAPEnterpriseDomain sapDomain = new SAPEnterpriseDomain(this);
            var isWaitingForNothing = invoiceModelList.All(t => t.WaitingFor == WaitingAction.Nothing);
            if (isWaitingForNothing)
            {
                sapDomain.CreateSAPWorkflow(firstInvoice);
                if (isSentToPDI)
                {
                    if (firstInvoice != null)
                    {
                        CreatePDIAPIWorkflow(firstInvoice, firstInvoice.Order);
                    }
                }
            }

            if (firstInvoice != null)
            {
                CreateQbAccountingWorkflowForInvoice(false, firstInvoice, firstInvoice.Order, null);
                CreateQbAccountingWorkflowForBill(false, firstInvoice, firstInvoice.Order, null);
            }

            // add taxes from ddt into hedge dropped amount or spot droppedd amount            
            await Context.CommitAsync();
        }

        private async Task UpdatePpgAndWaitingForExistingDdt(InvoiceModel invoiceModel, WaitingAction waitingAction)
        {
            var ddtDetails = Context.DataContext.Invoices.SingleOrDefault(t => t.Id == invoiceModel.Id);
            if (ddtDetails != null)
            {
                SetDecimalPlacesForNetGrossDrop(invoiceModel);

                ddtDetails.UoM = invoiceModel.UoM;
                ddtDetails.DroppedGallons = invoiceModel.DroppedGallons;
                var ppg = invoiceModel.PricePerGallon;

                if (invoiceModel.BolDetails != null && invoiceModel.BolDetails.Any() && invoiceModel.BolDetails.Select(t => t.PricePerGallon).FirstOrDefault() != 0)
                    ppg = invoiceModel.BolDetails.Select(t => t.PricePerGallon).FirstOrDefault();

                if (invoiceModel.IsMarineLocation)//added this code to update values from ddt/bol grid.
                {
                    ddtDetails.ConvertedPricing = invoiceModel.ConvertedPricing;
                    ddtDetails.ConvertedQuantity = invoiceModel.ConvertedQuantity;
                    ddtDetails.Gravity = invoiceModel.Gravity;
                    ddtDetails.ConversionFactor = invoiceModel.ConversionFactor;

                    if (ddtDetails.UoM == UoM.MetricTons || ddtDetails.UoM == UoM.Barrels)
                    {
                        if (invoiceModel.ConvertedQuantity.HasValue && invoiceModel.ConvertedQuantity.Value > 0)
                        {
                            ppg = Math.Round(ppg, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                            invoiceModel.ConvertedQuantity = Math.Round(invoiceModel.ConvertedQuantity.Value, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                            invoiceModel.AdditionalDetail.TotalAllowance = Math.Round(invoiceModel.AdditionalDetail.TotalAllowance ?? 0, ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                            ddtDetails.BasicAmount = (ppg * invoiceModel.ConvertedQuantity.Value) - (invoiceModel.AdditionalDetail.TotalAllowance ?? 0);
                        }
                    }
                    else
                    {
                        ddtDetails.BasicAmount = Math.Round((ppg * invoiceModel.DroppedGallons)
                            - (invoiceModel.AdditionalDetail.TotalAllowance ?? 0), ApplicationConstants.InvoiceBasicAmountDecimalDisplay);
                    }
                }
                else
                    ddtDetails.BasicAmount = Math.Round((ppg * invoiceModel.DroppedGallons) - (invoiceModel.AdditionalDetail.TotalAllowance ?? 0), ApplicationConstants.InvoiceBasicAmountDecimalDisplay);

                ddtDetails.WaitingFor = (int)waitingAction;
                if (ddtDetails.InvoiceXAdditionalDetail != null && (invoiceModel.AdditionalDetail != null && invoiceModel.AdditionalDetail.NoDataExceptionApprovalId == (int)NoDataExceptionApproval.AcceptWithoutData))
                {
                    ddtDetails.InvoiceXAdditionalDetail.NoDataExceptionApprovalId = invoiceModel.AdditionalDetail.NoDataExceptionApprovalId;
                }

                if (waitingAction == WaitingAction.AvalaraTax)
                    ddtDetails.DDTConversionReason = (int)DDTConversionReason.AvalaraTaxFailed;

                foreach (var item in ddtDetails.InvoiceXBolDetails)
                {
                    item.InvoiceFtlDetail.PricePerGallon = ppg;
                    if (invoiceModel.BolDetails.Any(t => (t.BolNumber != null && t.BolNumber == item.InvoiceFtlDetail.BolNumber)
                                                        || (t.LiftTicketNumber != null && t.LiftTicketNumber == item.InvoiceFtlDetail.LiftTicketNumber)))
                    {
                        item.InvoiceFtlDetail.NetQuantity = invoiceModel.BolDetails.FirstOrDefault(t => (t.BolNumber != null && t.BolNumber == item.InvoiceFtlDetail.BolNumber)
                                                        || (t.LiftTicketNumber != null && t.LiftTicketNumber == item.InvoiceFtlDetail.LiftTicketNumber)).NetQuantity;
                        item.InvoiceFtlDetail.GrossQuantity = invoiceModel.BolDetails.FirstOrDefault(t => (t.BolNumber != null && t.BolNumber == item.InvoiceFtlDetail.BolNumber)
                                                        || (t.LiftTicketNumber != null && t.LiftTicketNumber == item.InvoiceFtlDetail.LiftTicketNumber)).GrossQuantity;

                        //NO NEED TO EDIT BOL HERE AS WE ARE ALREADY UPDATING BOL OR LIFT TICKET NUMBER
                    }

                }
                //Update assetdrop for DDT details if changed 
                UpdateAssetDropsForPrePost(invoiceModel, ddtDetails);
                ddtDetails.SupplierPreferredInvoiceTypeId = invoiceModel.SupplierPreferredInvoiceTypeId; // process without Tax was not generating Invoice as sup pre inv type was DDT
                Context.DataContext.Entry(ddtDetails).State = EntityState.Modified;
                await Context.CommitAsync();

                LogManager.Logger.WriteDebug("ConsolidatedDdtToInvoiceDomain", "UpdatePpgAndWaitingForExistingDdt", $"DDT Id:{invoiceModel.Id}");
            }
        }

        private static void UpdateAssetDropsForPrePost(InvoiceModel invoiceModel, Invoice ddtDetails)
        {
            if (invoiceModel.AssetDrops != null && invoiceModel.AssetDrops.Any())
            {
                foreach (var asset in invoiceModel.AssetDrops)
                {
                    //Existing asset drop details
                    var existingAssetDetails = ddtDetails.AssetDrops.Where(t => t.JobXAssetId == asset.JobXAssetId).FirstOrDefault();
                    if (existingAssetDetails != null)
                    {
                        existingAssetDetails.PreDip = asset.PreDip;
                        existingAssetDetails.PostDip = asset.PostDip;
                    }
                }

            }
        }
        private async Task UpdateExistingDdtToInactive(InvoiceModel invoiceModel)
        {
            var ddtDetails = Context.DataContext.Invoices.SingleOrDefault(t => t.Id == invoiceModel.Id);
            if (ddtDetails != null)
            {
                ddtDetails.IsActive = false;
                ddtDetails.InvoiceVersionStatusId = (int)InvoiceVersionStatus.InActive;
                ddtDetails.WaitingFor = (int)WaitingAction.Nothing;

                if (!string.IsNullOrWhiteSpace(ddtDetails.ReferenceId))
                    ddtDetails.DisplayInvoiceNumber = ddtDetails.ReferenceId;

                Context.DataContext.Entry(ddtDetails).State = EntityState.Modified;
                await Context.CommitAsync();

                LogManager.Logger.WriteDebug("DDTUpdatedWithPrice", "ProcessDDTList_New", $"DDT Id made inactive :{invoiceModel.Id}");
            }
        }

        #endregion

        #region Waiting For Filld Response
        public async Task<bool> ProcessInvoicesWaitingForFilld()
        {
            var response = false;
            try
            {
                var waitingForFilldDdtList = Context.DataContext.Invoices.
                                                        Where(t1 => t1.WaitingFor == (int)WaitingAction.FilldResponse && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                        && t1.IsActive)
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
                                                            OrderId = t2.OrderId ?? 0
                                                        }).ToList().GroupBy(t => t.InvoiceHeaderId).OrderBy(t => t.Select(t1 => t1.InvoiceHeaderId).FirstOrDefault()).ToList();

                //iterate header list and process each ddt
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);

                foreach (var invoiceHeader in waitingForFilldDdtList)
                {
                    List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
                    List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>();

                    var ddtList = invoiceHeader.ToList();
                    if (ddtList.Any())
                    {
                        var ddtIds = ddtList.Select(t => t.DdtId).ToList();
                        var originalDdtList = Context.DataContext.Invoices.Where(t => ddtIds.Contains(t.Id) && t.InvoiceHeaderId == invoiceHeader.Key).ToList();
                        var trackableScheduleIds = originalDdtList.Select(t => t.TrackableScheduleId).Distinct().ToList();
                        

                        var newsfeedParms = ddtList.FirstOrDefault();
                        var originalDdtNumber = newsfeedParms?.DisplayInvoiceNumber;

                        foreach (var ddtItem in ddtList)
                        {
                            var originalDdt = originalDdtList.SingleOrDefault(t => t.Id == ddtItem.DdtId);
                            if (originalDdt != null)
                            {
                                InvoiceModel invoiceModel = GetInvoiceModel(originalDdt, ddtItem.ProductCode, ddtItem.ProductTypeId, ddtItem.TimeZoneName);
                                priceRequestModels.AddRange(SetPriceRequestDetails(invoiceModel.BolDetails, ddtItem.RequestPriceDetailId, invoiceModel));
                                invoiceModelList.Add(invoiceModel);
                            }
                        }

                        if (invoiceModelList.Any())
                        {
                            await GetPriceDetails(priceRequestModels, invoiceModelList);
                            if (invoiceModelList.Any(t => t.WaitingFor == WaitingAction.UpdatedPrice))
                            {
                                foreach (var invoiceModel in invoiceModelList)
                                {
                                    //save details and set ddt to waiting for price
                                    await UpdatePpgAndWaitingForExistingDdt(invoiceModel, WaitingAction.UpdatedPrice);

                                    //statusViewModel.StatusCode = Status.Success;
                                    //statusViewModel.StatusMessage = Resource.warningMessageDDTWaitingForUpdatedPrice;
                                }
                                return response;
                            }

                            //convert ddt to invoice
                            var result = await ProcessDdtList(invoiceModelList, invoiceHeader.Key);
                            if (result.StatusCode == Status.Success)
                            {
                                if (newsfeedParms != null)
                                {
                                    var invoiceModelFirst = invoiceModelList.First();
                                    var usercontext = new UserContext() { Id = newsfeedParms.CreatedBy, CompanyName = newsfeedParms.SupplierCompanyName, CompanyId = newsfeedParms.AcceptedCompanyId, Name = $"{newsfeedParms.FirstName} {newsfeedParms.LastName}" };
                                    ConversionNewsfeedViewModel conversionnewsfeedmodel = GetConversionNewsfeedViewModel(invoiceModelFirst, newsfeedParms.JobId, newsfeedParms.AcceptedCompanyId, newsfeedParms.BuyerCompanyName, newsfeedParms.SupplierCompanyName, newsfeedParms.BuyerCompanyId);
                                    await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed_New(usercontext, conversionnewsfeedmodel, originalDdtNumber);

                                    var notificationDomain = new NotificationDomain(this);
                                    await notificationDomain.AddNotificationEventAsync(EventType.CreateInvoiceFromDDT, invoiceModelFirst.InvoiceHeaderId, newsfeedParms.CreatedBy);
                                }
                            }
                        }
                    }
                }
                response = true;
            }
            catch (Exception ex)
            {
                response = false;
                LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "ProcessInvoicesWaitingForFilld", ex.Message, ex);
            }

            return response;
        }
        #endregion

        #region Waiting for Avalara Tax
        public async Task<int> ProcessInvoicesWaitingForTax()
        {
            int generatedInvoices = 0;

            var btaxServiceEnabled = GetTaxServiceEnabledFlag();
            if (!btaxServiceEnabled)
            {
                return generatedInvoices;
            }

            //new headlist
            var waitingForTaxDdtList = Context.DataContext.Invoices.Where(t1 => t1.WaitingFor == (int)WaitingAction.AvalaraTax
                                                                      && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                      && t1.IsActive
                                                                      && t1.DDTConversionReason != (int)DDTConversionReason.AvalaraTaxFailed)
                                                                     .Select(t2 => new
                                                                     {
                                                                         DdtId = t2.Id,
                                                                         TerminalId = t2.InvoiceXBolDetails.Select(t3 => t3.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                                         t2.Order.FuelRequest.MstProduct.ProductCode,
                                                                         t2.Order.FuelRequest.MstProduct.ProductTypeId,
                                                                         t2.PoNumber,
                                                                         t2.User.Company.Name,
                                                                         t2.CreatedBy,
                                                                         t2.Order.AcceptedCompanyId,
                                                                         t2.Order.FuelRequest.JobId,
                                                                         BuyerCompanyName = t2.Order.FuelRequest.User.Company.Name,
                                                                         SupplierCompanyName = t2.User.Company.Name,
                                                                         t2.Order.BuyerCompanyId,
                                                                         t2.User.FirstName,
                                                                         t2.User.LastName,
                                                                         t2.DisplayInvoiceNumber,
                                                                         t2.Order.FuelRequest.Job.TimeZoneName,
                                                                         t2.InvoiceHeaderId
                                                                     }).ToList().GroupBy(t => t.InvoiceHeaderId).OrderBy(t => t.Select(t1 => t1.InvoiceHeaderId).FirstOrDefault()).ToList();

            //iterate header list and process each ddt
            NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);

            foreach (var gruopItem in waitingForTaxDdtList)
            {
                List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
                var ddtListFromHeader = gruopItem.ToList();
                if (ddtListFromHeader.Any())
                {
                    var newsfeedParms = ddtListFromHeader.FirstOrDefault();
                    var originalDdtNumber = newsfeedParms?.DisplayInvoiceNumber;

                    foreach (var ddtItem in ddtListFromHeader)
                    {
                        var originalDdt = Context.DataContext.Invoices.Where(t => t.Id == ddtItem.DdtId && t.InvoiceHeaderId == gruopItem.Key).SingleOrDefault();
                        if (originalDdt != null)
                        {
                            InvoiceModel invoiceModel = GetInvoiceModel(originalDdt, ddtItem.ProductCode, ddtItem.ProductTypeId, ddtItem.TimeZoneName);
                            invoiceModelList.Add(invoiceModel);
                        }
                    }

                    if (invoiceModelList.Any())
                    {
                        //convert ddt to invoice
                        var result = await ProcessDdtList(invoiceModelList, gruopItem.Key, true);
                        if (result.StatusCode == Status.Success)
                        {
                            generatedInvoices += invoiceModelList.Count;
                            if (newsfeedParms != null)
                            {
                                var invoiceModelFirst = invoiceModelList.First();
                                var usercontext = new UserContext() { Id = newsfeedParms.CreatedBy, CompanyName = newsfeedParms.SupplierCompanyName, CompanyId = newsfeedParms.AcceptedCompanyId, Name = $"{newsfeedParms.FirstName} {newsfeedParms.LastName}" };
                                ConversionNewsfeedViewModel conversionnewsfeedmodel = GetConversionNewsfeedViewModel(invoiceModelFirst, newsfeedParms.JobId, newsfeedParms.AcceptedCompanyId, newsfeedParms.BuyerCompanyName, newsfeedParms.SupplierCompanyName, newsfeedParms.BuyerCompanyId);
                                await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed_New(usercontext, conversionnewsfeedmodel, originalDdtNumber);

                                var notificationDomain = new NotificationDomain(this);
                                await notificationDomain.AddNotificationEventAsync(EventType.CreateInvoiceFromDDT, invoiceModelFirst.InvoiceHeaderId, newsfeedParms.CreatedBy);

                                // await notificationDomain.AddNotificationEventAsync(EventType.InvoiceGeneratedEstablishConnectionWithAvalara, gruopItem.Key, newsfeedParms.CreatedBy);
                            }
                        }
                    }
                }
                var invoicesWithoutTerminals = invoiceModelList.Where(t => t.TypeOfFuel != (int)ProductTypes.NonStandardFuel && t.TerminalId == null || t.TerminalId <= 0).ToList();
                if (invoicesWithoutTerminals != null && invoicesWithoutTerminals.Any())
                {
                    StringBuilder msg = new StringBuilder("Terminal not assigned to DDT/PO : ");
                    foreach (var item in invoicesWithoutTerminals)
                    {
                        msg.Append($" Id: {item.Id} & PoNumber: {item.PoNumber}, ");
                    }

                    LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "ProcessInvoicesWaitingForTax", msg.ToString(), new Exception());
                }
            }

            return generatedInvoices;
        }
        #endregion

        #region manual DDT to Invoice conversion
        public async Task<StatusViewModel> ConvertDdtToInvoiceManually(UserContext userContext, int ddtId, bool isConvertToInv = false, NoDataExceptionPrePostViewModel prePostViewModel = null)
        {
            StatusViewModel statusViewModel = new StatusViewModel(Status.Failed);
            //Get header list 
            try
            {
                var ddtListForConversion = Context.DataContext.Invoices
                                            .Where(t => t.Id == ddtId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                            .SelectMany(t => t.InvoiceHeader.Invoices)
                                            .Select(t => new
                                            {
                                                DdtId = t.Id,
                                                TerminalId = t.InvoiceXBolDetails.Select(t3 => t3.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                t.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                                                t.Order.FuelRequest.MstProduct.ProductCode,
                                                t.Order.FuelRequest.MstProduct.ProductTypeId,
                                                t.Order.FuelRequest.FuelRequestDetail.IsPrePostDipRequired,
                                                t.PoNumber,
                                                t.User.Company.Name,
                                                t.CreatedBy,
                                                t.Order.AcceptedCompanyId,
                                                t.Order.FuelRequest.JobId,
                                                BuyerCompanyName = t.Order.FuelRequest.User.Company.Name,
                                                SupplierCompanyName = t.User.Company.Name,
                                                t.Order.BuyerCompanyId,
                                                t.DisplayInvoiceNumber,
                                                t.InvoiceHeaderId,
                                                t.Order.FuelRequest.Job.TimeZoneName,
                                                OrderPoNumber = t.Order.PoNumber,
                                                OriginalDdt = t
                                            }).ToList();

                //iterate header list and process each ddt
                List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                var newsfeedParms = ddtListForConversion.FirstOrDefault();
                if (newsfeedParms != null)
                {
                    List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>();
                    foreach (var ddtItem in ddtListForConversion)
                    {
                        if (ddtItem.OriginalDdt != null)
                        {
                            InvoiceModel invoiceModel = GetInvoiceModel(ddtItem.OriginalDdt, ddtItem.ProductCode, ddtItem.ProductTypeId, ddtItem.TimeZoneName);
                            priceRequestModels.AddRange(SetPriceRequestDetails(invoiceModel.BolDetails, ddtItem.RequestPriceDetailId, invoiceModel));
                            if (isConvertToInv)
                            {
                                invoiceModel.SupplierPreferredInvoiceTypeId = (int)InvoiceType.Manual; // only in case of creating manual invoice from ddt
                            }

                            if (prePostViewModel != null && prePostViewModel.AssetDetails.Any())
                            {
                                var isAssetDetailsProvided = false;
                                foreach (var asset in prePostViewModel.AssetDetails)
                                {
                                    if (invoiceModel.OrderId == asset.OrderId)
                                    {

                                        foreach (var assetDrop in invoiceModel.AssetDrops)
                                        {
                                            //match asset id and set pre-post data
                                            if (assetDrop.AssetName == asset.AssetName)
                                            {
                                                assetDrop.PreDip = asset.PreDip;
                                                assetDrop.PostDip = asset.PostDip;
                                                assetDrop.TankScaleMeasurement = asset.TankScaleMeasurement;
                                                isAssetDetailsProvided = true;
                                            }
                                        }
                                    }
                                }
                                if (isAssetDetailsProvided)
                                {
                                    if (invoiceModel.IsDropImageReq && invoiceModel.Image == null)
                                    {
                                        invoiceModel.WaitingFor = WaitingAction.Images;
                                    }
                                    else if (invoiceModel.IsSignatureReq && invoiceModel.SignatureId == null)
                                    {
                                        invoiceModel.WaitingFor = WaitingAction.Images;
                                    }
                                    else if (invoiceModel.IsBOLImageReq && invoiceModel.BolDetails.Any(t => t.ImageId == null))
                                    {
                                        invoiceModel.WaitingFor = WaitingAction.Images;
                                    }
                                }
                            }
                            invoiceModelList.Add(invoiceModel);
                            if (ddtItem.IsPrePostDipRequired && invoiceModel.AssetDrops.Any(t => t.PreDip == null || t.PostDip == null || (t.PostDip.HasValue && t.PostDip.Value <= 0)))
                            {
                                invoiceModelList.Clear();
                                statusViewModel.StatusCode = Status.Failed;
                                statusViewModel.StatusMessage = Resource.errPrePostDipDataIsMissing;
                                break;
                            }
                            else if (invoiceModel.WaitingFor == WaitingAction.Images)
                            {
                                var ddtDetails = Context.DataContext.Invoices.SingleOrDefault(t => t.Id == invoiceModel.Id);
                                UpdateAssetDropsForPrePost(invoiceModel, ddtDetails);
                                ddtDetails.WaitingFor = (int)invoiceModel.WaitingFor;
                                ddtDetails.UpdatedBy = userContext.Id;
                                ddtDetails.UpdatedDate = DateTimeOffset.Now;
                                var additionalDetail = ddtDetails.InvoiceXAdditionalDetail;
                                if (additionalDetail != null)
                                {
                                    additionalDetail.NoDataExceptionApprovalId = (int)NoDataExceptionApproval.UploadImages;
                                }

                                Context.DataContext.Entry(ddtDetails).State = EntityState.Modified;
                                await Context.CommitAsync();

                                invoiceModelList.Clear();
                                statusViewModel.StatusCode = Status.Success;
                                statusViewModel.StatusMessage = Resource.SuccessMsgPrePostUpdated;
                                break;
                            }
                        }
                    }
                    if (invoiceModelList.Any())
                    {
                        await GetPriceDetails(priceRequestModels, invoiceModelList);
                        if (invoiceModelList.Any(t => t.WaitingFor == WaitingAction.UpdatedPrice))
                        {
                            foreach (var invoiceModel in invoiceModelList)
                            {
                                //save details and set ddt to waiting for price
                                await UpdatePpgAndWaitingForExistingDdt(invoiceModel, WaitingAction.UpdatedPrice);

                                statusViewModel.StatusCode = Status.Success;
                                statusViewModel.StatusMessage = Resource.warningMessageDDTWaitingForUpdatedPrice;
                            }
                            return statusViewModel;
                        }

                        //convert ddt to invoice
                        statusViewModel = await ProcessDdtList(invoiceModelList, newsfeedParms.InvoiceHeaderId);
                        if (statusViewModel.StatusCode == Status.Success && invoiceModelList.All(t => t.WaitingFor == WaitingAction.Nothing))
                        {
                            statusViewModel.StatusMessage = Resource.errMessageInvoiceCreateSuccess;
                            var invoiceModelFirst = invoiceModelList.First();
                            ConversionNewsfeedViewModel conversionnewsfeedmodel = GetConversionNewsfeedViewModel(invoiceModelFirst, newsfeedParms.JobId, newsfeedParms.AcceptedCompanyId, newsfeedParms.BuyerCompanyName, newsfeedParms.SupplierCompanyName, newsfeedParms.BuyerCompanyId);
                            await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed_New(userContext, conversionnewsfeedmodel, newsfeedParms.DisplayInvoiceNumber);
                            var cumulationQtyList = CreateListOfCumulationQuantityUpdateForDDTConversion(invoiceModelList);
                            if (cumulationQtyList != null && cumulationQtyList.Any())
                            {
                                await new ConsolidatedInvoiceDomain(this).UpdateCumulationQuantitiesPostInvoiceCreate(cumulationQtyList);
                            }
                            var notificationDomain = new NotificationDomain(this);
                            await notificationDomain.AddNotificationEventAsync(EventType.CreateInvoiceFromDDT, conversionnewsfeedmodel.InvoiceHeaderId, newsfeedParms.CreatedBy);

                            if (invoiceModelFirst != null && invoiceModelFirst.IsMarineLocation)
                            {
                                await notificationDomain.AddNotificationEventAsync(EventType.DeliveryClosedSendBDN, conversionnewsfeedmodel.InvoiceHeaderId, newsfeedParms.CreatedBy);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                statusViewModel.StatusMessage = Resource.errMessageFaildToUpdateDDT;
                LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "ConvertDdtToInvoiceManually", $"{ex.Message}, Ddt # - {ddtId}", ex);
            }

            return statusViewModel;
        }

        public async Task<StatusViewModel> ConvertDdtToInvoiceWithBolManually(UserContext userContext, int ddtId, InvoiceViewModelNew invoiceViewModel)
        {
            StatusViewModel statusViewModel = new StatusViewModel(Status.Failed);
            //Get header list 
            try
            {
                var ddtListForConversion = Context.DataContext.Invoices
                                            .Where(t => t.Id == ddtId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                            .SelectMany(t => t.InvoiceHeader.Invoices)
                                            .Select(t => new
                                            {
                                                DdtId = t.Id,
                                                TerminalId = t.InvoiceXBolDetails.Select(t3 => t3.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                t.Order.FuelRequest.FuelTypeId,
                                                t.Order.FuelRequest.MstProduct.ProductCode,
                                                t.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                                                t.Order.FuelRequest.MstProduct.ProductTypeId,
                                                t.PoNumber,
                                                t.User.Company.Name,
                                                t.CreatedBy,
                                                t.Order.AcceptedCompanyId,
                                                TerminalName = t.Order.MstExternalTerminal.Name,
                                                t.Order.CityGroupTerminalId,
                                                OrderTerminalId = t.Order.CityGroupTerminalId,
                                                t.Order.FuelRequest.Job.TimeZoneName,
                                                t.Order.FuelRequest.JobId,
                                                BuyerCompanyName = t.Order.FuelRequest.User.Company.Name,
                                                SupplierCompanyName = t.User.Company.Name,
                                                t.Order.BuyerCompanyId,
                                                t.DisplayInvoiceNumber,
                                                t.InvoiceHeaderId,
                                                OriginalDdt = t,
                                            }).ToList();

                //iterate header list and process each ddt
                List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                var newsfeedParms = ddtListForConversion.FirstOrDefault();
                if (newsfeedParms != null)
                {
                    //var bolDetails = invoiceViewModel.BolDetails;
                    List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>();
                    foreach (var ddtItem in ddtListForConversion)
                    {
                        if (ddtItem.OriginalDdt != null)
                        {
                            InvoiceModel invoiceModel = GetInvoiceModel(ddtItem.OriginalDdt, ddtItem.ProductCode, ddtItem.ProductTypeId, ddtItem.TimeZoneName);

                            // set bol details
                            var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(ddtItem.TimeZoneName);
                            DropAdditionalDetailsModel deliveryDetails = new DropAdditionalDetailsModel() { TerminalId = ddtItem.OrderTerminalId, TerminalName = ddtItem.TerminalName, CityGroupTerminalId = ddtItem.CityGroupTerminalId };

                            if (invoiceModel.CreationMethod != CreationMethod.BulkUploaded && invoiceModel.CreationMethod != CreationMethod.APIUpload)
                            {
                                SetBolDetails(invoiceViewModel, ddtItem.FuelTypeId, invoiceModel.CityGroupTerminalId, invoiceModel);

                                // set lift info                                    
                                SetLiftInformation(invoiceViewModel, ddtItem.FuelTypeId, deliveryDetails, invoiceModel);
                                invoiceModel.BolDetails.ForEach(t => { t.CreatedBy = userContext.Id; t.CreatedDate = currentDate; });
                                await SetInvoiceBolDetails(invoiceModel.BolDetails, ddtItem.InvoiceHeaderId, ddtItem.DdtId);
                                await SaveBulkPlantLocations(invoiceModel.BolDetails);
                            }

                            if (invoiceModel.CreationMethod == CreationMethod.BulkUploaded || invoiceModel.CreationMethod == CreationMethod.APIUpload)
                            {
                                var drop = invoiceViewModel.Drops.Where(t => t.OrderId == ddtItem.OriginalDdt.OrderId).FirstOrDefault();
                                if (drop != null)
                                {
                                    invoiceModel.AssetDrops = SetAssetDropsToInvoice(deliveryDetails.JobId, deliveryDetails.JobCompanyId, userContext.Id,
                                                                        invoiceViewModel.Driver?.Id, invoiceModel.DropEndDate, drop.Assets);
                                    invoiceModel.AssetDrops.ForEach(t => t.OrderId = invoiceModel.OrderId.Value);

                                    //if (invoiceModel.WaitingFor == WaitingAction.PrePostDipData)
                                    //    invoiceModel.WaitingFor = WaitingAction.UpdatedPrice;
                                }

                                if (invoiceModel.WaitingFor == WaitingAction.Images)
                                {
                                    if (invoiceModel.Image == null)
                                        invoiceModel.Image = invoiceViewModel.InvoiceImage;
                                    if (invoiceModel.BolImage == null)
                                    {
                                        invoiceModel.BolDetails.Clear();
                                        SetBolDetails(invoiceViewModel, ddtItem.FuelTypeId, invoiceModel.CityGroupTerminalId, invoiceModel);
                                        invoiceModel.BolDetails.ForEach(t => { t.CreatedBy = userContext.Id; t.CreatedDate = currentDate; });
                                        await SetInvoiceBolDetails(invoiceModel.BolDetails, ddtItem.InvoiceHeaderId, ddtItem.DdtId);
                                        invoiceModel.BolImage = invoiceViewModel.BolDetails.FirstOrDefault()?.Images;
                                    }
                                    if (invoiceModel.AdditionalImage == null)
                                        invoiceModel.AdditionalImage = invoiceViewModel.AdditionalImage;
                                    if (invoiceModel.InspectionRequestVoucherImage == null)
                                        invoiceModel.InspectionRequestVoucherImage = invoiceViewModel.InspectionRequestVoucherImage;
                                    if (invoiceModel.Signature == null)
                                        invoiceModel.Signature = invoiceViewModel.SignatureImage?.ToCustomerSignature();
                                }
                            }

                            // get pricing ddetails
                            priceRequestModels.AddRange(SetPriceRequestDetails(invoiceModel.BolDetails, ddtItem.RequestPriceDetailId, invoiceModel));
                            invoiceModelList.Add(invoiceModel);
                            if (IsPickupFromMultipleTerminals(invoiceModelList.Last(), deliveryDetails.PricingTypeId))
                            {
                                GetInvoicesForSameProduct(invoiceModel, invoiceModelList);
                            }
                        }
                    }

                    if (invoiceModelList.Any())
                    {
                        await GetPriceDetails(priceRequestModels, invoiceModelList);
                        if (invoiceModelList.Any(t => t.WaitingFor == WaitingAction.UpdatedPrice))
                        {
                            foreach (var invoiceModel in invoiceModelList)
                            {
                                //save bol details and set ddt to waiting for price
                                await UpdatePpgAndWaitingForExistingDdt(invoiceModel, WaitingAction.UpdatedPrice);

                                statusViewModel.StatusCode = Status.Success;
                                statusViewModel.StatusMessage = Resource.warningMessageDDTWaitingForUpdatedPrice;
                            }
                            return statusViewModel;
                        }
                        //convert ddt to invoice
                        statusViewModel = await ProcessDdtList(invoiceModelList, newsfeedParms.InvoiceHeaderId);
                        if (statusViewModel.StatusCode == Status.Success && invoiceModelList.All(t => t.WaitingFor == WaitingAction.Nothing))
                        {
                            statusViewModel.StatusMessage = Resource.successMessageInvoiceCreatedFromDDTWaitingForBol;
                            var invoiceModelFirst = invoiceModelList.First();
                            ConversionNewsfeedViewModel conversionnewsfeedmodel = GetConversionNewsfeedViewModel(invoiceModelFirst, newsfeedParms.JobId, newsfeedParms.AcceptedCompanyId, newsfeedParms.BuyerCompanyName, newsfeedParms.SupplierCompanyName, newsfeedParms.BuyerCompanyId);
                            await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed_New(userContext, conversionnewsfeedmodel, newsfeedParms.DisplayInvoiceNumber);

                            var notificationDomain = new NotificationDomain(this);
                            await notificationDomain.AddNotificationEventAsync(EventType.CreateInvoiceFromDDT, newsfeedParms.InvoiceHeaderId, newsfeedParms.CreatedBy);

                            statusViewModel.EntityNumber = invoiceModelFirst.DisplayInvoiceNumber;
                        }
                        await UpdateDeliveryScheduleDeliveryLevelPO(invoiceViewModel, statusViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                statusViewModel.StatusMessage = Resource.errMessageFaildToUpdateDDT;
                LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "ConvertDdtToInvoiceWithBolManually", $"{ex.Message}, Ddt # - {ddtId}", ex);
            }

            return statusViewModel;
        }
        private async Task UpdateDeliveryScheduleDeliveryLevelPO(InvoiceViewModelNew invoiceViewModel, StatusViewModel statusViewModel)
        {
            if (statusViewModel.StatusCode == Status.Success)
            {
                if (invoiceViewModel.Drops.Any())
                {
                    var trackableSchedulesIds = invoiceViewModel.Drops.Select(x => x.TrackableScheduleId).ToList();
                    var deliverySchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(x => trackableSchedulesIds.Contains(x.Id)).ToListAsync();
                    foreach (var deliverySchedule in deliverySchedules)
                    {
                        var deliveryLevelPOInfo = invoiceViewModel.Drops.FirstOrDefault(x => x.TrackableScheduleId == deliverySchedule.Id);
                        if (deliveryLevelPOInfo != null && !string.IsNullOrEmpty(deliveryLevelPOInfo.DeliveryLevelPO))
                        {
                            deliverySchedule.DeliveryLevelPO = deliveryLevelPOInfo.DeliveryLevelPO;
                        }
                    }
                    await Context.CommitAsync();
                }
            }
        }

        private List<CumulationQuantityUpdateViewModel> CreateListOfCumulationQuantityUpdateForDDTConversion(List<InvoiceModel> InvoiceModelList)
        {
            var responseList = new List<CumulationQuantityUpdateViewModel>();
            try
            {
                if (InvoiceModelList != null && InvoiceModelList.Any())
                {
                    foreach (var invoice in InvoiceModelList)
                    {
                        if (invoice.InvoiceTypeId != ((int)InvoiceType.DigitalDropTicketManual)
                            && invoice.InvoiceTypeId != ((int)InvoiceType.DigitalDropTicketMobileApp) && invoice.BolDetails.Any(t => t.TierPricingForBol.Any()))
                        {
                            var item = new CumulationQuantityUpdateViewModel();
                            item.DroppedGallons = invoice.DroppedGallons;
                            item.RequestPriceDetailsId = Context.DataContext.Invoices.FirstOrDefault(t => t.Id == invoice.Id).Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId;
                            responseList.Add(item);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "CreateListOfCumulationQuantityUpdateForDDTConversion", ex.Message, ex);
            }
            return responseList;
        }
        #endregion

        #region Convert DDT without Data
        public async Task<StatusViewModel> ConvertDdtToInvoiceWithoutData(UserContext userContext, int ddtId, NoDataExceptionApproval noDataAction)
        {
            StatusViewModel statusViewModel = new StatusViewModel(Status.Failed);
            //Get header list 
            try
            {
                var ddtListForConversion = Context.DataContext.Invoices
                                            .Where(t => t.Id == ddtId && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                            .SelectMany(t => t.InvoiceHeader.Invoices)
                                            .Select(t => new
                                            {
                                                DdtId = t.Id,
                                                TerminalId = t.InvoiceXBolDetails.Select(t3 => t3.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                t.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                                                t.Order.FuelRequest.MstProduct.ProductCode,
                                                t.Order.FuelRequest.MstProduct.ProductTypeId,
                                                t.Order.FuelRequest.FuelRequestDetail.IsPrePostDipRequired,
                                                t.PoNumber,
                                                t.User.Company.Name,
                                                t.CreatedBy,
                                                t.Order.AcceptedCompanyId,
                                                t.Order.FuelRequest.JobId,
                                                BuyerCompanyName = t.Order.FuelRequest.User.Company.Name,
                                                SupplierCompanyName = t.User.Company.Name,
                                                t.Order.BuyerCompanyId,
                                                t.DisplayInvoiceNumber,
                                                t.InvoiceHeaderId,
                                                t.Order.FuelRequest.Job.TimeZoneName,
                                                OrderPoNumber = t.Order.PoNumber,
                                                OriginalDdt = t
                                            }).ToList();

                //iterate header list and process each ddt
                List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                var newsfeedParms = ddtListForConversion.FirstOrDefault();
                if (newsfeedParms != null)
                {
                    List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>();
                    foreach (var ddtItem in ddtListForConversion)
                    {
                        if (ddtItem.OriginalDdt != null)
                        {
                            InvoiceModel invoiceModel = GetInvoiceModel(ddtItem.OriginalDdt, ddtItem.ProductCode, ddtItem.ProductTypeId, ddtItem.TimeZoneName);
                            invoiceModel.AdditionalDetail.NoDataExceptionApprovalId = (int)noDataAction;
                            priceRequestModels.AddRange(SetPriceRequestDetails(invoiceModel.BolDetails, ddtItem.RequestPriceDetailId, invoiceModel));
                            invoiceModelList.Add(invoiceModel);
                        }
                    }

                    if (invoiceModelList.Any())
                    {
                        await GetPriceDetails(priceRequestModels, invoiceModelList);
                        if (invoiceModelList.Any(t => t.WaitingFor == WaitingAction.UpdatedPrice))
                        {
                            foreach (var invoiceModel in invoiceModelList)
                            {
                                //save details and set ddt to waiting for price
                                await UpdatePpgAndWaitingForExistingDdt(invoiceModel, WaitingAction.UpdatedPrice);

                                statusViewModel.StatusCode = Status.Success;
                                statusViewModel.StatusMessage = Resource.warningMessageDDTWaitingForUpdatedPrice;
                            }
                            return statusViewModel;
                        }

                        //convert ddt to invoice
                        statusViewModel = await ProcessDdtList(invoiceModelList, newsfeedParms.InvoiceHeaderId);
                        if (statusViewModel.StatusCode == Status.Success && invoiceModelList.All(t => t.WaitingFor == WaitingAction.Nothing))
                        {
                            statusViewModel.StatusMessage = Resource.errMessageInvoiceCreateSuccess;
                            var invoiceModelFirst = invoiceModelList.First();
                            ConversionNewsfeedViewModel conversionnewsfeedmodel = GetConversionNewsfeedViewModel(invoiceModelFirst, newsfeedParms.JobId, newsfeedParms.AcceptedCompanyId, newsfeedParms.BuyerCompanyName, newsfeedParms.SupplierCompanyName, newsfeedParms.BuyerCompanyId);
                            await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed_New(userContext, conversionnewsfeedmodel, newsfeedParms.DisplayInvoiceNumber);

                            var notificationDomain = new NotificationDomain(this);
                            await notificationDomain.AddNotificationEventAsync(EventType.CreateInvoiceFromDDT, conversionnewsfeedmodel.InvoiceHeaderId, newsfeedParms.CreatedBy);
                        }

                        // update existing ddt for no-data exception approval status
                        if (statusViewModel.StatusCode == Status.Success)
                        {
                            foreach (var ddtItem in ddtListForConversion)
                            {
                                if (ddtItem.OriginalDdt != null && ddtItem.OriginalDdt.InvoiceXAdditionalDetail != null)
                                {
                                    ddtItem.OriginalDdt.InvoiceXAdditionalDetail.NoDataExceptionApprovalId = (int)noDataAction;
                                    Context.DataContext.Entry(ddtItem.OriginalDdt).State = EntityState.Modified;
                                    await Context.CommitAsync();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                statusViewModel.StatusMessage = Resource.errMessageFaildToUpdateDDT;
                LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "ConvertDdtToInvoiceWithoutData", $"{ex.Message}, Ddt # - {ddtId}", ex);
            }

            return statusViewModel;
        }

        #endregion

        #region Consolidation for Group DR
        public async Task<StatusViewModel> ConsolidateGroupDrInvoices(string groupParentDrId, int companyId, int headerId, bool isWithouTax)
        {
            StatusViewModel statusViewModel = new StatusViewModel(Status.Failed);
            //Get header list 
            try
            {
                IQueryable<Invoice> ddtForConsodlidationQuery = null;

                if (headerId > 0 && !isWithouTax)
                {
                    ddtForConsodlidationQuery = Context.DataContext.Invoices
                                                .Where(t => t.InvoiceHeaderId == headerId && t.Order.AcceptedCompanyId == companyId &&
                                                            t.WaitingFor == (int)WaitingAction.InvoiceConfirmation &&
                                                            t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active);
                }
                else if (headerId > 0 && isWithouTax)
                {
                    ddtForConsodlidationQuery = Context.DataContext.Invoices
                                                .Where(t => t.InvoiceHeaderId == headerId && t.Order.AcceptedCompanyId == companyId &&
                                                            (t.WaitingFor == (int)WaitingAction.PDITaxes || t.WaitingFor == (int)WaitingAction.AvalaraTax) &&
                                                            t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active);
                }
                else if (!string.IsNullOrEmpty(groupParentDrId))
                {
                    ddtForConsodlidationQuery = Context.DataContext.Invoices
                                                .Where(t => t.GroupParentDrId != null && t.GroupParentDrId == groupParentDrId && t.Order.AcceptedCompanyId == companyId &&
                                                        t.WaitingFor == (int)WaitingAction.AllDRCompletion &&
                                                        t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active);
                }

                if (ddtForConsodlidationQuery != null)
                {
                    var ddtForConsolidation = ddtForConsodlidationQuery
                                                .SelectMany(t => t.InvoiceHeader.Invoices)
                                                .Select(t => new
                                                {
                                                    DdtId = t.Id,
                                                    TerminalId = t.InvoiceXBolDetails.Select(t3 => t3.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                    t.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                                                    t.Order.FuelRequest.MstProduct.ProductCode,
                                                    t.Order.FuelRequest.MstProduct.ProductTypeId,
                                                    t.Order.FuelRequest.FuelRequestDetail.IsPrePostDipRequired,
                                                    FRUoM = t.Order.FuelRequest.UoM,
                                                    BDNConfirmationRequired = t.Order.OrderAdditionalDetail == null ? false : t.Order.OrderAdditionalDetail.IsManualBDNConfirmationRequired,
                                                    InvoiceConfirmationRequired = t.Order.OrderAdditionalDetail == null ? false : t.Order.OrderAdditionalDetail.IsManualInvoiceConfirmationRequired,
                                                    t.PoNumber,
                                                    t.User.Company.Name,
                                                    t.CreatedBy,
                                                    t.Order.AcceptedCompanyId,
                                                    t.Order.DefaultInvoiceType,
                                                    t.Order.FuelRequest.JobId,
                                                    t.Order.FuelRequest.Job.IsMarine,
                                                    BuyerCompanyName = t.Order.FuelRequest.User.Company.Name,
                                                    SupplierCompanyName = t.User.Company.Name,
                                                    t.Order.BuyerCompanyId,
                                                    t.DisplayInvoiceNumber,
                                                    t.InvoiceHeaderId,
                                                    t.Order.FuelRequest.Job.TimeZoneName,
                                                    OrderPoNumber = t.Order.PoNumber,
                                                    OriginalDdt = t
                                                }).Distinct().ToList();

                    //iterate header list and process each ddt
                    List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
                    NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                    var notificationDomain = new NotificationDomain(this);
                    var groupByDrAndCompanyId = ddtForConsolidation.GroupBy(t => new { t.AcceptedCompanyId, t.OriginalDdt.GroupParentDrId }).ToList();
                    foreach (var groupItem in groupByDrAndCompanyId)
                    {
                        var ddtListForConversion = groupItem.ToList();
                        var newsfeedParms = ddtListForConversion.FirstOrDefault();
                        if (newsfeedParms != null)
                        {
                            List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>();
                            foreach (var ddtItem in ddtListForConversion)
                            {
                                if (ddtItem.OriginalDdt != null)
                                {
                                    InvoiceModel invoiceModel = GetInvoiceModel(ddtItem.OriginalDdt, ddtItem.ProductCode, ddtItem.ProductTypeId, ddtItem.TimeZoneName);
                                    invoiceModel.UoM = ddtItem.FRUoM; // we need to consider order level UoM for group DR or Invoice confirmation
                                    invoiceModel.IsPdieTaxRequired = isWithouTax ? false : invoiceModel.IsPdieTaxRequired;
                                    invoiceModel.IsProcessWithoutTax = isWithouTax;
                                    if (isWithouTax)
                                        invoiceModel.AdditionalDetail.Notes = invoiceModel.AdditionalDetail.Notes + "Invoice processed without Tax.";

                                    priceRequestModels.AddRange(SetPriceRequestDetails(invoiceModel.BolDetails, ddtItem.RequestPriceDetailId, invoiceModel));
                                    //invoiceModel.InvoiceTypeId = newsfeedParms.DefaultInvoiceType;

                                    invoiceModelList.Add(invoiceModel);
                                    if (ddtItem.IsPrePostDipRequired && invoiceModel.AssetDrops.Any(t => t.PreDip == null || t.PostDip == null || (t.PostDip.HasValue && t.PostDip.Value <= 0)))
                                    {
                                        invoiceModelList.Clear();
                                        statusViewModel.StatusCode = Status.Failed;
                                        statusViewModel.StatusMessage = Resource.errPrePostDipDataIsMissing;
                                        break;
                                    }
                                    else if (invoiceModel.WaitingFor == WaitingAction.Images)
                                    {
                                        var ddtDetails = Context.DataContext.Invoices.SingleOrDefault(t => t.Id == invoiceModel.Id);
                                        UpdateAssetDropsForPrePost(invoiceModel, ddtDetails);
                                        ddtDetails.WaitingFor = (int)invoiceModel.WaitingFor;
                                        ddtDetails.UpdatedBy = invoiceModel.CreatedBy;
                                        ddtDetails.UpdatedDate = DateTimeOffset.Now;
                                        var additionalDetail = ddtDetails.InvoiceXAdditionalDetail;
                                        if (additionalDetail != null)
                                        {
                                            additionalDetail.NoDataExceptionApprovalId = (int)NoDataExceptionApproval.UploadImages;
                                        }

                                        Context.DataContext.Entry(ddtDetails).State = EntityState.Modified;
                                        await Context.CommitAsync();

                                        invoiceModelList.Clear();
                                        statusViewModel.StatusCode = Status.Success;
                                        statusViewModel.StatusMessage = Resource.SuccessMsgPrePostUpdated;
                                        break;
                                    }

                                    if (headerId == 0 && ddtItem.IsMarine)
                                    {
                                        invoiceModelList.ForEach(t => t.IsMarineLocation = ddtItem.IsMarine);
                                        if (invoiceModelList.Any(t => t.WaitingFor == WaitingAction.AllDRCompletion))
                                        {
                                            if (ddtItem.BDNConfirmationRequired)
                                                invoiceModelList.ForEach(t => t.WaitingFor = WaitingAction.BDNConfirmation);
                                            else if (ddtItem.InvoiceConfirmationRequired)
                                                invoiceModelList.ForEach(t => t.WaitingFor = WaitingAction.InvoiceConfirmation);
                                        }
                                    }

                                }
                            }

                            if (invoiceModelList.Any())
                            {
                                if (headerId > 0 && invoiceModelList.Any(t => t.IsMarineLocation))
                                {
                                    if (!isWithouTax)
                                    {
                                        int invoiceModelCount = 1;
                                        foreach (var invoiceModel in invoiceModelList)
                                        {
                                            if (invoiceModelCount == invoiceModelList.Count)
                                            {
                                                await RemoveExistingFeesAndApplyOrderLevelFees(invoiceModelList, invoiceModel);
                                            }
                                            invoiceModelCount++;
                                        }
                                    }
                                }

                                if (headerId == 0 && invoiceModelList.Any(t => t.WaitingFor == WaitingAction.BDNConfirmation || t.WaitingFor == WaitingAction.InvoiceConfirmation))
                                {
                                    //get pricing call added, as after Inv confirmation we are setting waiting for PDIE tax and there we assumed that we have price
                                    await GetPriceDetails(priceRequestModels, invoiceModelList, true);
                                    int invoiceModelCount = 1;
                                    foreach (var invoiceModel in invoiceModelList)
                                    {
                                        if (invoiceModelCount == invoiceModelList.Count)
                                        {
                                            if (invoiceModelList.Any(t => t.IsMarineLocation))
                                                await RemoveExistingFeesAndApplyOrderLevelFees(invoiceModelList, invoiceModel);
                                        }
                                        //save details and set ddt to waiting for price
                                        await UpdatePpgAndWaitingForExistingDdt(invoiceModel, invoiceModel.WaitingFor);

                                        statusViewModel.StatusCode = Status.Success;
                                        statusViewModel.StatusMessage = invoiceModel.WaitingFor == WaitingAction.BDNConfirmation ? Resource.lblWaitingForBDNConfirmation : Resource.lblWaitingForInvoiceConfirmation;
                                        invoiceModelCount++;
                                    }

                                    if (invoiceModelList.Any(t => t.WaitingFor == WaitingAction.InvoiceConfirmation))
                                    {
                                        ConversionNewsfeedViewModel conversionnewsfeedmodel = GetConversionNewsfeedViewModel(invoiceModelList.First(), newsfeedParms.JobId, newsfeedParms.AcceptedCompanyId, newsfeedParms.BuyerCompanyName, newsfeedParms.SupplierCompanyName, newsfeedParms.BuyerCompanyId);
                                        if (ddtListForConversion.Any(t => t.IsMarine))
                                            await notificationDomain.AddNotificationEventAsync(EventType.DeliveryClosedSendBDN, conversionnewsfeedmodel.InvoiceHeaderId, newsfeedParms.CreatedBy);
                                    }

                                    return statusViewModel;
                                }

                                await GetPriceDetails(priceRequestModels, invoiceModelList);
                                if (invoiceModelList.Any(t => t.WaitingFor == WaitingAction.UpdatedPrice))
                                {
                                    foreach (var invoiceModel in invoiceModelList)
                                    {
                                        //save details and set ddt to waiting for price
                                        await UpdatePpgAndWaitingForExistingDdt(invoiceModel, WaitingAction.UpdatedPrice);

                                        statusViewModel.StatusCode = Status.Success;
                                        statusViewModel.StatusMessage = Resource.warningMessageDDTWaitingForUpdatedPrice;
                                    }
                                    return statusViewModel;
                                }
                                //convert ddt to invoice
                                statusViewModel = await ProcessDdtList(invoiceModelList, newsfeedParms.InvoiceHeaderId);
                                if (statusViewModel.StatusCode == Status.Success && invoiceModelList.All(t => t.WaitingFor == WaitingAction.Nothing))
                                {
                                    statusViewModel.StatusMessage = Resource.errMessageInvoiceCreateSuccess;
                                    var invoiceModelFirst = invoiceModelList.First();
                                    AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                                    var userContext = await authenticationDomain.GetUserContextAsync(invoiceModelFirst.CreatedBy, CompanyType.Supplier);

                                    ConversionNewsfeedViewModel conversionnewsfeedmodel = GetConversionNewsfeedViewModel(invoiceModelFirst, newsfeedParms.JobId, newsfeedParms.AcceptedCompanyId, newsfeedParms.BuyerCompanyName, newsfeedParms.SupplierCompanyName, newsfeedParms.BuyerCompanyId);
                                    await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed_New(userContext, conversionnewsfeedmodel, newsfeedParms.DisplayInvoiceNumber);
                                    var cumulationQtyList = CreateListOfCumulationQuantityUpdateForDDTConversion(invoiceModelList);
                                    if (cumulationQtyList != null && cumulationQtyList.Any())
                                    {
                                        await new ConsolidatedInvoiceDomain(this).UpdateCumulationQuantitiesPostInvoiceCreate(cumulationQtyList);
                                    }
                                    await notificationDomain.AddNotificationEventAsync(EventType.CreateInvoiceFromDDT, conversionnewsfeedmodel.InvoiceHeaderId, newsfeedParms.CreatedBy);
                                    if (invoiceModelList.All(t => t.WaitingFor == WaitingAction.Nothing) && ddtListForConversion.Any(t => t.IsMarine))
                                        await notificationDomain.AddNotificationEventAsync(EventType.DeliveryClosedSendBDN, conversionnewsfeedmodel.InvoiceHeaderId, newsfeedParms.CreatedBy);

                                    if (isWithouTax)
                                        LogManager.Logger.WriteDebug("ConsolidatedDdtToInvoiceDomain", "ConsolidateGroupDrInvoices", $"DDT converted to Invoice without Tax. Invoice Header:{headerId}");

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                statusViewModel.StatusMessage = Resource.errMessageFaildToUpdateDDT;
                LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "ConsolidateGroupDrInvoices", $"{ex.Message}, groupParentDrId # - {groupParentDrId}", ex);
            }

            return statusViewModel;
        }

        private async Task RemoveExistingFeesAndApplyOrderLevelFees(List<InvoiceModel> invoiceModelList, InvoiceModel invoiceModel)
        {
            var orderId = invoiceModelList.Select(t => t.OrderId).FirstOrDefault();
            if (orderId != null && orderId.HasValue && orderId.Value > 0)
            {
                var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == invoiceModel.InvoiceHeaderId).ToList();
                if (invoices.Any())
                {
                    //remove flat fees from all invoices and sbstract total fee amount from each invoice
                    foreach (var invoice in invoices)
                    {
                        invoice.FuelRequestFees.Clear();
                        invoice.TotalFeeAmount = 0;
                        invoice.InvoiceHeader.TotalFeeAmount = 0;
                        Context.DataContext.Entry(invoice).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }

                    var fuelFees = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => t.FuelRequest.FuelRequestFees).FirstOrDefault();
                    if (fuelFees.Any())
                    {
                        invoiceModel.FuelFees.Clear();
                        var orderFees = fuelFees.ToFeesViewModel();
                        orderFees.RemoveAll(t => t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
                        orderFees.ForEach(t => t.OrderId = orderId.Value);
                        SetInvoiceFeesToMarineConsolidatedInv(orderFees, invoiceModel);
                        SetCalculatedFeesToMarineConsolidatedInv(invoiceModel);

                        var firstInv = invoices.FirstOrDefault();
                        firstInv.FuelRequestFees = invoiceModel.FuelFees.Select(t => t.ToEntity()).ToList();
                        firstInv.TotalFeeAmount = firstInv.FuelRequestFees.Sum(t => t.TotalFee);
                        firstInv.InvoiceHeader.TotalFeeAmount = firstInv.TotalFeeAmount ?? 0;
                        Context.DataContext.Entry(firstInv).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }
                }
            }
        }

        public async Task SendInvoiceToPDI(int invoiceId)
        {
            var invoice = await Context.DataContext.Invoices.SingleOrDefaultAsync(t => t.Id == invoiceId);
            if (invoice != null)
            {
                CreatePDIAPIWorkflow(invoice, invoice.Order);
            }
        }

        #endregion

        #region Edit consolidated Invoice from grid
        public async Task<StatusViewModel> UpdateConsolidatedInvoices(int invoiceHeaderId, List<InvoiceBolEditGrid> postedInvoices)
        {
            StatusViewModel statusViewModel = new StatusViewModel(Status.Failed);
            //Get header list 
            try
            {
                var ddtForConsolidation = Context.DataContext.Invoices
                                            //.Where(t => (t.WaitingFor == (int)WaitingAction.Nothing || t.WaitingFor == (int)WaitingAction.BDNConfirmation || t.WaitingFor == (int)WaitingAction.InvoiceConfirmation) &&
                                            // removed all waiting for conditions - discussed with sandip and arihant 
                                            .Where(t => t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active &&
                                                        t.InvoiceHeaderId == invoiceHeaderId)
                                            .SelectMany(t => t.InvoiceHeader.Invoices)
                                            .Select(t => new
                                            {
                                                DdtId = t.Id,
                                                TerminalId = t.InvoiceXBolDetails.Select(t3 => t3.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                t.Order.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                                                t.Order.FuelRequest.MstProduct.ProductCode,
                                                t.Order.FuelRequest.MstProduct.ProductTypeId,
                                                t.Order.FuelRequest.FuelRequestDetail.IsPrePostDipRequired,
                                                t.PoNumber,
                                                t.User.Company.Name,
                                                t.CreatedBy,
                                                t.Order.AcceptedCompanyId,
                                                t.Order.DefaultInvoiceType,
                                                t.Order.FuelRequest.JobId,
                                                BuyerCompanyName = t.Order.FuelRequest.User.Company.Name,
                                                SupplierCompanyName = t.User.Company.Name,
                                                t.Order.BuyerCompanyId,
                                                t.DisplayInvoiceNumber,
                                                t.InvoiceHeaderId,
                                                t.Order.FuelRequest.Job.TimeZoneName,
                                                OrderPoNumber = t.Order.PoNumber,
                                                OriginalDdt = t,
                                                IsEbolWorkflowEnabled = (t.Order != null && t.Order.OrderAdditionalDetail != null && t.Order.OrderAdditionalDetail.OnboardingPreference != null) ? t.Order.OrderAdditionalDetail.OnboardingPreference.IsEbolWorkflowEnabled : false
                                            }).Distinct().ToList();

                if (ddtForConsolidation.Count == 0)
                {
                    statusViewModel.StatusMessage = Resource.errMsgNoRecordsFoundForProcessing;
                    return statusViewModel;
                }

                //iterate header list and process each ddt
                List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                var groupByDrAndCompanyId = ddtForConsolidation.GroupBy(t => new { t.AcceptedCompanyId, t.OriginalDdt.GroupParentDrId }).ToList();
                //statusViewModel.EntityHeaderId = invoiceHeaderId;
                foreach (var groupItem in groupByDrAndCompanyId)
                {
                    var ddtListForConversion = groupItem.ToList();
                    var newsfeedParms = ddtListForConversion.FirstOrDefault();
                    if (newsfeedParms != null)
                    {
                        List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>();
                        foreach (var ddtItem in ddtListForConversion)
                        {
                            if (ddtItem.OriginalDdt != null)
                            {
                                InvoiceModel invoiceModel = GetInvoiceModel(ddtItem.OriginalDdt, ddtItem.ProductCode, ddtItem.ProductTypeId, ddtItem.TimeZoneName);
                                if (newsfeedParms.IsEbolWorkflowEnabled && (invoiceModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoiceModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
                                {
                                    await VerifyEBOLWorkflow(invoiceModel, postedInvoices);
                                }
                                //SET modified values from grid
                                var modifiedValues = postedInvoices.Where(t => t.InvoiceId == ddtItem.OriginalDdt.Id).ToList();
                                var postedInvoiceFtlDetail = new InvoiceBolEditGrid();
                                if (modifiedValues.Any())
                                {
                                    foreach (var bolitem in invoiceModel.BolDetails)
                                    {
                                        postedInvoiceFtlDetail = modifiedValues.FirstOrDefault(x => x.InvoiceFtlDetailId == bolitem.Id);
                                        if (postedInvoiceFtlDetail != null && bolitem.Id == postedInvoiceFtlDetail.InvoiceFtlDetailId)
                                        {
                                            bolitem.GrossQuantity = postedInvoiceFtlDetail.GrossQty;
                                            bolitem.NetQuantity = postedInvoiceFtlDetail.NetQty;
                                            bolitem.EbolMatchStatus = postedInvoiceFtlDetail.EbolMatchStatus;
                                            bolitem.DeliveredQuantity = postedInvoiceFtlDetail.DroppedQty; // Dropped Qty column in grid represents delivered qty of bol
                                            //no need to update modified BOL/lift ticket number here.
                                        }
                                    }
                                    if (postedInvoiceFtlDetail != null)
                                    {
                                        //invoiceModel.DroppedGallons = postedInvoiceFtlDetail.DroppedQty;                  
                                        invoiceModel.DroppedGallons = invoiceModel.BolDetails.Sum(t => t.DeliveredQuantity)?? postedInvoiceFtlDetail.DroppedQty;
                                        invoiceModel.Gravity = postedInvoiceFtlDetail.ApiGravity;
                                    }
                                    //assumed from grid its always gallons 
                                    if (invoiceModel.UoM == UoM.MetricTons && invoiceModel.ConversionFactor.HasValue && invoiceModel.ConversionFactor.Value > 0)
                                    {
                                        //convert gallons to converted MT qty
                                        invoiceModel.ConvertedQuantity = invoiceModel.DroppedGallons / invoiceModel.ConversionFactor;
                                    }
                                    else if (postedInvoiceFtlDetail!=null && invoiceModel.UoM == UoM.MetricTons && postedInvoiceFtlDetail.ApiGravity.HasValue && postedInvoiceFtlDetail.ApiGravity.Value > 0)
                                    {
                                        SetConvertedQuantitiesAndGravityForMFN(invoiceModel, invoiceModel.JobCountryId, true);
                                    }
                                    else if (invoiceModel.UoM == UoM.Barrels)
                                    {
                                        SetConvertedQuantitiesAndGravityForMFN(invoiceModel, invoiceModel.JobCountryId, true);
                                    }
                                }

                                priceRequestModels.AddRange(SetPriceRequestDetails(invoiceModel.BolDetails, ddtItem.RequestPriceDetailId, invoiceModel));
                                invoiceModelList.Add(invoiceModel);
                            }
                        }
                        if (invoiceModelList.Any())
                        {
                            await GetPriceDetails(priceRequestModels, invoiceModelList);
                            statusViewModel = await ProcessDdtList(invoiceModelList, newsfeedParms.InvoiceHeaderId, false, true);
                            if (statusViewModel.StatusCode == Status.Success && invoiceModelList.All(t => t.WaitingFor == WaitingAction.Nothing))
                            {
                                statusViewModel.StatusMessage = Resource.errMessageInvoiceUpdatedSuccess;
                                var invoiceModelFirst = invoiceModelList.First();
                                AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                                var userContext = await authenticationDomain.GetUserContextAsync(invoiceModelFirst.CreatedBy, CompanyType.Supplier);

                                ConversionNewsfeedViewModel conversionnewsfeedmodel = GetConversionNewsfeedViewModel(invoiceModelFirst, newsfeedParms.JobId, newsfeedParms.AcceptedCompanyId, newsfeedParms.BuyerCompanyName, newsfeedParms.SupplierCompanyName, newsfeedParms.BuyerCompanyId);
                                await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed_New(userContext, conversionnewsfeedmodel, newsfeedParms.DisplayInvoiceNumber);
                                var cumulationQtyList = CreateListOfCumulationQuantityUpdateForDDTConversion(invoiceModelList);
                                if (cumulationQtyList != null && cumulationQtyList.Any())
                                {
                                    await new ConsolidatedInvoiceDomain(this).UpdateCumulationQuantitiesPostInvoiceCreate(cumulationQtyList);
                                }
                                var notificationDomain = new NotificationDomain(this);
                                await notificationDomain.AddNotificationEventAsync(EventType.CreateInvoiceFromDDT, conversionnewsfeedmodel.InvoiceHeaderId, newsfeedParms.CreatedBy);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                statusViewModel.StatusMessage = Resource.errMessageFaildToUpdateDDT;
                LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "UpdateConsolidatedInvoices", $"{ex.Message}, invoiceHeaderId # - {invoiceHeaderId}", ex);
            }

            return statusViewModel;
        }
        #endregion

        //private async Task<List<PDITaxFTPViewModel>> ProcessPDITaxFromFTP()
        //{
        //    var model = await Context.DataContext.MstAppSettings.Where(w => w.Key == "PDITaxFTPDetails").Select(s=>s.Value).FirstOrDefaultAsync();
        //    FTPConfig config = JsonConvert.DeserializeObject<FTPConfig>(model);
        //    List<PDITaxFTPViewModel> taxList = new List<PDITaxFTPViewModel>();
        //    using (SftpClient sftp = new SftpClient(config.Host, config.UserName, config.Password))
        //    {
        //        try
        //        {
        //            sftp.Connect();
        //            var files = sftp.ListDirectory(config.RemoteDirectory);
        //            if (files != null && files.Any())
        //            {
        //                foreach (var file in files)
        //                {
        //                    if (!string.IsNullOrEmpty(file.Name) && file.Name != "." && file.Name != "..")
        //                        await ReadPDITaxFile(sftp, file, taxList);
        //                }
        //                sftp.Disconnect();

        //                return taxList;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "ProcessPDITaxFromFTP", ex.Message, ex);
        //        }
        //        return taxList;
        //    }
        //}

        //private async Task ReadPDITaxFile(SftpClient client, SftpFile file, List<PDITaxFTPViewModel> PDITaxList)
        //{
        //    var stream = new MemoryStream();
        //    using (Stream remoteTempFile = client.OpenWrite(file.FullName))
        //    {
        //        client.DownloadFile(file.FullName, stream);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        await ConvertCSVToList(file.FullName, stream, PDITaxList);
        //    }
        //}

        //private async Task ConvertCSVToList(string filePath, Stream stream, List<PDITaxFTPViewModel> PDITaxList)
        //{
        //    IExcelDataReader excelReader = null;
        //    DataTable dataTable = null;
        //    excelReader = ExcelReaderFactory.CreateCsvReader(stream);
        //    DataSet result = excelReader.AsDataSet();
        //    if (result != null && result.Tables.Count > 0)
        //    {
        //        dataTable = result.Tables[0];
        //        DataRow fdr = dataTable.Rows[0];
        //        int i = 0;//index
        //        foreach (DataColumn col in fdr.Table.Columns)
        //        {
        //            col.ColumnName = fdr[i].ToString();
        //            Console.WriteLine(col.ColumnName);
        //            Console.ReadLine();
        //            i++;
        //        }
        //       var taxList = (from DataRow dr in dataTable.Rows
        //                                 select new PDITaxFTPViewModel()
        //                                      {
        //                                          OrderNumber = dr["Order Number"].ToString(),
        //                                          CustomerDescription = dr["Customer Description"].ToString(),
        //                                          TaxDescription = dr["Tax Description"].ToString(),
        //                                          TaxType = dr["Tax Type"].ToString(),
        //                                          TaxMethod = dr["Tax Method"].ToString(),
        //                                          TaxRate =dr["Tax Rate"].ToString(),
        //                                          BasedOnUnits = dr["Based On Units"].ToString(),
        //                                          TaxBasis = dr["Tax Basis"].ToString(),
        //                                          TaxAmount = dr["Tax Amount"].ToString(),
        //                                          TaxExceptionDescription = dr["Tax Exception Description"].ToString(),
        //                                          TaxExceptionOverride= dr["Tax Exception Override"].ToString(),
        //                                          TaxCertificateNo = dr["Tax Certificate No"].ToString(),
        //                                      }).ToList();
        //        taxList.RemoveAt(0);//Remove header
        //        PDITaxList.AddRange(taxList);
        //    }
        //}




        public async Task<int> ProcessInvoicesWaitingForPDITax(List<PDITaxFTPViewModel> taxList)
        {
            int generatedInvoices = 0;


            // add logic to fetch Tax details from excel sheet from FTP location
            //   List<PDITaxFTPViewModel> ftpTaxList = await ProcessPDITaxFromFTP();
            var pdiTaxes = new List<PDITaxDetailsViewModel>();
            // if (ftpTaxList != null && ftpTaxList.Any())
            if (taxList != null && taxList.Any())
            {
                taxList.ForEach(t => pdiTaxes.Add(t.ToPDITaxDetailsViewModel()));
            }
            else
            {
                return generatedInvoices;
                //pdiTaxes.Add(new PDITaxDetailsViewModel() { PDIOrderNumber = "ON-509261-21", BasedOnUnits = "Gross",TaxDescription="yash bhai"
                //, TaxAmount = 10.8m, CustomerDescription = "yas", TaxBasis =11.5m,TaxCertificateNumber="123",TaxMethod= "Per Unit", TaxPricingTypeId=3});
            }

            //
            //get list of ddts waiting for PDI taxes
            if (pdiTaxes != null && pdiTaxes.Any())
            {
                List<string> pdiOrderNos = pdiTaxes.Select(s => s.PDIOrderNumber).Distinct().ToList();
                var waitingForPDITaxDdtList = Context.DataContext.Invoices.Where(t1 => t1.WaitingFor == (int)WaitingAction.PDITaxes && t1.InvoiceXAdditionalDetail.PDIDeliveryOrderNo != null
                                                                          && pdiOrderNos.Contains(t1.InvoiceXAdditionalDetail.PDIDeliveryOrderNo)
                                                                          && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                          && t1.IsActive)
                                                                         .Select(t2 => new
                                                                         {
                                                                             DdtId = t2.Id,
                                                                             TerminalId = t2.InvoiceXBolDetails.Select(t3 => t3.InvoiceFtlDetail.TerminalId).FirstOrDefault(),
                                                                             t2.Order.FuelRequest.MstProduct.ProductCode,
                                                                             t2.Order.FuelRequest.MstProduct.ProductTypeId,
                                                                             t2.PoNumber,
                                                                             t2.User.Company.Name,
                                                                             t2.CreatedBy,
                                                                             t2.Order.AcceptedCompanyId,
                                                                             t2.Order.FuelRequest.JobId,
                                                                             BuyerCompanyName = t2.Order.FuelRequest.User.Company.Name,
                                                                             SupplierCompanyName = t2.User.Company.Name,
                                                                             t2.Order.BuyerCompanyId,
                                                                             t2.User.FirstName,
                                                                             t2.User.LastName,
                                                                             t2.DisplayInvoiceNumber,
                                                                             t2.Order.FuelRequest.Job.TimeZoneName,
                                                                             t2.InvoiceHeaderId
                                                                         }).ToList().GroupBy(t => t.InvoiceHeaderId).OrderBy(t => t.Select(t1 => t1.InvoiceHeaderId).FirstOrDefault()).ToList();

                //iterate header list and process each ddt
                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);

                foreach (var gruopItem in waitingForPDITaxDdtList)
                {
                    List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
                    var ddtListFromHeader = gruopItem.ToList();
                    if (ddtListFromHeader.Any())
                    {
                        var newsfeedParms = ddtListFromHeader.FirstOrDefault();
                        var originalDdtNumber = newsfeedParms?.DisplayInvoiceNumber;

                        foreach (var ddtItem in ddtListFromHeader)
                        {
                            var originalDdt = Context.DataContext.Invoices.Where(t => t.Id == ddtItem.DdtId && t.InvoiceHeaderId == gruopItem.Key).SingleOrDefault();
                            if (originalDdt != null)
                            {
                                InvoiceModel invoiceModel = GetInvoiceModel(originalDdt, ddtItem.ProductCode, ddtItem.ProductTypeId, ddtItem.TimeZoneName);
                                invoiceModelList.Add(invoiceModel);
                            }
                        }

                        if (invoiceModelList.Any())
                        {
                            //convert ddt to invoice
                            var result = await ProcessDdtListForPDITaxes(invoiceModelList, gruopItem.Key, pdiTaxes, true);
                            if (result.StatusCode == Status.Success)
                            {
                                generatedInvoices += invoiceModelList.Count;
                                if (newsfeedParms != null)
                                {
                                    var invoiceModelFirst = invoiceModelList.First();
                                    var usercontext = new UserContext() { Id = newsfeedParms.CreatedBy, CompanyName = newsfeedParms.SupplierCompanyName, CompanyId = newsfeedParms.AcceptedCompanyId, Name = $"{newsfeedParms.FirstName} {newsfeedParms.LastName}" };
                                    ConversionNewsfeedViewModel conversionnewsfeedmodel = GetConversionNewsfeedViewModel(invoiceModelFirst, newsfeedParms.JobId, newsfeedParms.AcceptedCompanyId, newsfeedParms.BuyerCompanyName, newsfeedParms.SupplierCompanyName, newsfeedParms.BuyerCompanyId);
                                    await newsfeedDomain.SetDdtToInvoiceCreatedNewsfeed_New(usercontext, conversionnewsfeedmodel, originalDdtNumber);

                                    var notificationDomain = new NotificationDomain(this);
                                    await notificationDomain.AddNotificationEventAsync(EventType.CreateInvoiceFromDDT, invoiceModelFirst.InvoiceHeaderId, newsfeedParms.CreatedBy);

                                    // await notificationDomain.AddNotificationEventAsync(EventType.InvoiceGeneratedEstablishConnectionWithAvalara, gruopItem.Key, newsfeedParms.CreatedBy);
                                }
                            }
                        }
                    }
                    var invoicesWithoutTerminals = invoiceModelList.Where(t => t.TypeOfFuel != (int)ProductTypes.NonStandardFuel && t.TerminalId == null || t.TerminalId <= 0).ToList();
                    if (invoicesWithoutTerminals != null && invoicesWithoutTerminals.Any())
                    {
                        StringBuilder msg = new StringBuilder("Terminal not assigned to DDT/PO : ");
                        foreach (var item in invoicesWithoutTerminals)
                        {
                            msg.Append($" Id: {item.Id} & PoNumber: {item.PoNumber}, ");
                        }

                        LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "ProcessInvoicesWaitingForPDITax", msg.ToString(), new Exception());
                    }
                }

                return generatedInvoices;
            }
            return generatedInvoices;
        }

        public async Task<StatusViewModel> ProcessDdtListForPDITaxes(List<InvoiceModel> invoiceModelList, int invoiceHeaderId, List<PDITaxDetailsViewModel> pdiTaxes, bool isWaitingForTax = false)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var existingHeader = Context.DataContext.InvoiceHeaderDetails.SingleOrDefault(t => t.Id == invoiceHeaderId);
                    if (existingHeader != null)
                    {
                        var invoiceNumber = await GenerateInvoiceNumber_New();
                        int ddtInvoiceList = 1;
                        bool isUpdateOnlyPriceAndWaitingAction = false;
                        bool isAnyPdiTaxFailed = false;
                        string previousDisplayInvoiceNumber = invoiceModelList.First().DisplayInvoiceNumber;
                        foreach (var invoiceModel in invoiceModelList)
                        {

                            if (invoiceModel.SupplierPreferredInvoiceTypeId.HasValue && IsDigitalDropTicket(invoiceModel.SupplierPreferredInvoiceTypeId.Value))
                            {
                                //only update ppg and waiting for status
                                await UpdatePpgAndWaitingForExistingDdt(invoiceModel, WaitingAction.Nothing);
                                isUpdateOnlyPriceAndWaitingAction = true;
                            }
                            else
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

                                        TaxResponseViewModel taxResponse = await CalculateSetPDITaxesToInvoice(invoiceModelList, pdiTaxes);
                                        if (invoiceModelList.Any(t => t.TypeOfFuel != (int)ProductTypes.NonStandardFuel)
                                                                && invoiceModelList.Where(t => t.TypeOfFuel != (int)ProductTypes.NonStandardFuel)
                                                            .Sum(t => t.TotalTaxAmount) <= 0
                                            && taxResponse.StatusCode != Status.Success)
                                        {
                                            invoiceModelList.ForEach(t => { t.WaitingFor = WaitingAction.PDITaxes; t.DDTConversionReason = taxResponse.FailedStatusCode; });
                                            isUpdateOnlyPriceAndWaitingAction = true;
                                            isAnyPdiTaxFailed = true;
                                            invoiceModelList.ForEach(t => t.DisplayInvoiceNumber = previousDisplayInvoiceNumber);
                                            Context.DataContext.InvoiceNumbers.Remove(invoiceNumber);
                                            await Context.CommitAsync();
                                            //  LogManager.Logger.WriteException("", "ProcessDdtListForPDITaxes", "PDI Tax calculation failed  while converting ddt to invoice and invoiceheader is:" + invoiceHeaderId + " displayNumber:" + invoiceModel.DisplayInvoiceNumber, new Exception());
                                            LogManager.Logger.WriteDebug("ConsolidateDDTToInvoiceDomain", "ProcessDdtListForPDITaxes", "PDI Tax calculation failed  while converting ddt to invoice and invoiceheader is:" + invoiceHeaderId + " displayNumber:" + invoiceModel.DisplayInvoiceNumber);

                                        }
                                    }
                                }

                                if (isUpdateOnlyPriceAndWaitingAction)
                                {
                                    await UpdatePpgAndWaitingForExistingDdt(invoiceModel, WaitingAction.PDITaxes);
                                }
                                else
                                {
                                    await UpdateExistingDdtToInactive(invoiceModel);
                                }
                            }
                            ddtInvoiceList++;
                        }

                        if (!isUpdateOnlyPriceAndWaitingAction)
                        {
                            var newHeader = await AddNewInvoiceHeader(invoiceModelList, existingHeader, invoiceNumber.Id);
                            await AddInvoicesForHeader(newHeader, invoiceModelList, false);
                            UpdateExistingInvoiceHeader(existingHeader, newHeader);
                            response.StatusCode = Status.Success;
                            invoiceModelList.ForEach(t => t.InvoiceHeaderId = newHeader.Id);
                        }
                        else
                        {
                            existingHeader.TotalBasicAmount = invoiceModelList.Sum(t => t.BasicAmount);
                            if (invoiceModelList.All(t => t.WaitingFor == WaitingAction.Nothing))
                            {
                                await SendInvoiceToPDI(invoiceModelList.First().Id);
                            }
                        }
                        //if (invoiceModelList.All(t => t.IsMarineLocation) && invoiceModelList.Count > 1)
                        //{
                        //    var invoiceHeaderid = isUpdateOnlyPriceAndWaitingAction ? invoiceHeaderId : invoiceModelList.FirstOrDefault().InvoiceHeaderId;

                        //    await UpdateFlatFeesForMarineConsolidatedInvoices(invoiceHeaderid);

                        //}
                        await Context.CommitAsync();
                        transaction.Commit();

                        if (isAnyPdiTaxFailed)
                        {
                            response.StatusCode = Status.Failed;
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageDdtUpdatedSuccess;
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageFaildToUpdateDDT;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("ConsolidatedDdtToInvoiceDomain", "ProcessDdtListForPDITaxes", $"{ex.Message}, InvoiceHeader - {invoiceHeaderId}", ex);
                }
            }
            return response;
        }

        #region Marine Order level fees on Invoice Consolidation
        public void SetInvoiceFeesForMarineInv(List<FeesViewModel> fees, List<InvoiceModel> invoices)
        {
            fees.RemoveAll(t => !string.IsNullOrWhiteSpace(t.FeeTypeId) && t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
            fees.Where(t => t.FeeTypeId == ((int)FeeType.ProcessingFee).ToString()).ToList().ForEach(t => t.OrderId = 0);
            var invoiceFees = fees.ToList().ToInvoiceFees(invoices.OrderByDescending(t => t.DropEndDate).Select(t => t.DropEndDate).FirstOrDefault());
            var fuelTypes = invoices.GroupBy(t => t.OrderId).ToList();
            foreach (var fuelType in fuelTypes)
            {
                var invoice = fuelType.FirstOrDefault();
                //var drop = consolidateViewModel.Drops.FirstOrDefault(t => t.OrderId == fuelType.Key);
                //invoice.FuelFees = consolidateViewModel.Fees.Where(t => t.OrderId == fuelType.Key).ToList().ToInvoiceFees(invoice.DropEndDate);
                //FuelFeeViewModel surchargeFee = GetSurchargeFee(drop);
                //if (surchargeFee != null)
                //{
                //    invoice.FuelFees.Add(surchargeFee);
                //}

                invoice.FuelFees.ForEach(t => { t.Currency = invoice.Currency; t.UoM = invoice.UoM; });
                invoice.FuelFees.SelectMany(t => t.FeeByQuantities).ToList().ForEach(t =>
                {
                    t.Currency = invoice.Currency;
                    t.UoM = invoice.UoM;
                });
            }
            invoices.FirstOrDefault().FuelFees.AddRange(invoiceFees);
            //FOR FREIGHT ONLY INVOICE - ONLY DELIVERY FEE WILL BE INCLUDED
            if (invoices.Any(t => t.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest))
            {
                invoices.FirstOrDefault().FuelFees.RemoveAll(t => t.FeeTypeId != (int)FeeType.DeliveryFee);
            }
        }

        

        public void SetCalculatedFeesForMarineInv(List<InvoiceModel> invoices)
        {
            decimal? tierPricingInvQty = null;
            if (invoices.Any(t => t.BolDetails.Any(b => b.TierPricingForBol != null && b.TierPricingForBol.Any())))
                tierPricingInvQty = invoices.Sum(t => t.DroppedGallons);

            foreach (var invoice in invoices)
            {
                CalculateAndSetTotalFeeAndQuantityToFuelFees(invoices, invoices.Sum(t => t.DroppedGallons), invoice, invoice.AssetDrops.Count, tierPricingInvQty);

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

        public void SetInvoiceFeesToMarineConsolidatedInv(List<FeesViewModel> fees, InvoiceModel invoice)
        {
            fees.RemoveAll(t => !string.IsNullOrWhiteSpace(t.FeeTypeId) && t.FeeTypeId.Equals(((int)FeeType.DryRunFee).ToString()));
            fees.Where(t => t.FeeTypeId == ((int)FeeType.ProcessingFee).ToString()).ToList().ForEach(t => t.OrderId = 0);
            var invoiceFees = fees.ToList().ToInvoiceFees(invoice.DropEndDate);

            invoice.FuelFees.ForEach(t => { t.Currency = invoice.Currency; t.UoM = invoice.UoM; });
            invoice.FuelFees.SelectMany(t => t.FeeByQuantities).ToList().ForEach(t =>
            {
                t.Currency = invoice.Currency;
                t.UoM = invoice.UoM;
            });
            invoice.FuelFees.AddRange(invoiceFees);
            //FOR FREIGHT ONLY INVOICE - ONLY DELIVERY FEE WILL BE INCLUDED
            if (invoice.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
            {
                invoice.FuelFees.RemoveAll(t => t.FeeTypeId != (int)FeeType.DeliveryFee);
            }
        }

        public void SetCalculatedFeesToMarineConsolidatedInv(InvoiceModel invoice)
        {
            List<InvoiceModel> invoiceModels = new List<InvoiceModel>();
            invoiceModels.Add(invoice);

            CalculateAndSetTotalFeeAndQuantityToFuelFees(invoiceModels, invoice.DroppedGallons, invoice, invoice.AssetDrops.Count, null);

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

        public void CalculateAndSetTotalFeeAndQuantityToFuelFees(List<InvoiceModel> invoices, decimal droppedQuantity, InvoiceModel invoiceModel,
                                                                    int assetCount, decimal? tierPricingInvQty = null)
        {
            var model = GetModelToCalculateTotalDropTimeForMarineInv(invoices);
            var dropTimeDiff = GetDropStartDateAndEndDateDifferenceInSeconds(model);
            var invoiceFees = invoiceModel.FuelFees.Where(t => t.DiscountLineItemId == null && t.FeeTypeId != (int)FeeType.DryRunFee);
            FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
            foreach (var fee in invoiceFees)
            {
                if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon && fee.OrderId > 0)
                {
                    fuelFeesDomain.CalculateTotalFeeAndQuantityForFee(invoiceModel, droppedQuantity, fee, assetCount);
                }
                else
                {
                    if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && fee.OrderId == 0)
                    {
                        fuelFeesDomain.CalculateTotalFeeAndQuantityForFee(invoiceModel, droppedQuantity, fee, assetCount, dropTimeDiff);
                    }
                    else
                    {
                        fuelFeesDomain.CalculateTotalFeeAndQuantityForFee(invoiceModel, droppedQuantity, fee, assetCount, 0, tierPricingInvQty);
                    }
                }

            }
            foreach (var discount in invoiceModel.FuelFees.Where(t => t.DiscountLineItemId != null))
            {
                fuelFeesDomain.CalculateTotalFeeAndQuantityForDiscount(invoiceFees, droppedQuantity, discount);
            }
        }

        private double GetDropStartDateAndEndDateDifferenceInSeconds(InvoiceModel invoiceModel)
        {
            double difference;
            difference = invoiceModel.DropEndDate.AddMilliseconds(-invoiceModel.DropEndDate.Millisecond)
                        .Subtract(invoiceModel.DropStartDate.AddMilliseconds(-invoiceModel.DropStartDate.Millisecond)).TotalSeconds;

            //Below is added to allow EndTime(Next Day) and StartTime(Current Day)
            if (difference < 0)
                difference = difference + 86400;

            return difference;
        }

        private static InvoiceModel GetModelToCalculateTotalDropTimeForMarineInv(List<InvoiceModel> invoices)
        {
            var dropDates = new List<DateTimeOffset>();
            List<AssetDropModel> assets = new List<AssetDropModel>();
            if (invoices.Any(t => t.AssetDrops.Count > 0))
            {
                invoices.SelectMany(t => t.AssetDrops).ToList().Where(t => t.DropGallons != null && t.DropGallons > 0).ToList().
                    ForEach(t => assets.Add(new AssetDropModel()
                    {
                        DropStartDate = t.DropEndDate.Date.Add(t.DropStartDate.DateTime.TimeOfDay),
                        DropEndDate = t.DropEndDate.Date.Add(t.DropEndDate.DateTime.TimeOfDay)
                    }));
            }
            return new InvoiceModel() { CreatedDate = invoices.FirstOrDefault().CreatedDate, DropStartDate = dropDates.OrderBy(t => t).FirstOrDefault(), DropEndDate = dropDates.OrderByDescending(t => t).FirstOrDefault(), AssetDrops = assets };
        }

        #endregion
        private async Task VerifyEBOLWorkflow(InvoiceModel invoiceModel, List<InvoiceBolEditGrid> postedInvoiceBolEdits)
        {

            List<EBolAPIRequestDetails> eBolAPIRequestDetails = new List<EBolAPIRequestDetails>();
            foreach (var item in postedInvoiceBolEdits)
            {
                if (invoiceModel.BolDetails.Any())
                {
                    var invFtl = invoiceModel.BolDetails.FirstOrDefault(t => t.Id == item.InvoiceFtlDetailId);
                    if (invFtl != null && (invoiceModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoiceModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
                    {
                        if (!string.IsNullOrWhiteSpace(invFtl.BolNumber) && invFtl.TerminalId != null)
                        {
                            eBolAPIRequestDetails.Add(new EBolAPIRequestDetails { BOLNumber = item.BolOrLiftNumber, TerminalId = invFtl.TerminalId.Value });
                            item.TerminalId = invFtl.TerminalId.Value;
                        }
                    }
                }
            }
            List<EBolViewModel> ebolDetails = new List<EBolViewModel>();
            if (eBolAPIRequestDetails.Any())
            {
                ebolDetails = await GetEBolDetails(eBolAPIRequestDetails);
            }
            if (ebolDetails.Any())
            {
                foreach (var item in postedInvoiceBolEdits)
                {
                    var eBOLRecordStatus = ebolDetails.FirstOrDefault(x => x.BOLNumber == item.BolOrLiftNumber && x.TerminalId == item.TerminalId);
                    if (eBOLRecordStatus != null)
                    {
                        item.NetQty = eBOLRecordStatus.NetGallons;
                        item.GrossQty = eBOLRecordStatus.GrossGallons;
                        item.EbolMatchStatus = EbolMatchStatus.Match;
                    }
                }
            }

        }

    }
}