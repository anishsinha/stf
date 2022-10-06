using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class BillingStatementDomain : BaseDomain
    {
        public BillingStatementDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public BillingStatementDomain(string connectionString) : base(connectionString)
        {
        }

        public BillingStatementDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<StatementSummaryViewModel>> GetStatementSummary(int userId, int companyId, StatementSummaryDataViewModel stmtFilter)
        {
            List<StatementSummaryViewModel> response = new List<StatementSummaryViewModel>();
            try
            {
                var stmtSummary = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetStatementSummary(companyId, userId, stmtFilter);
                foreach (var stmt in stmtSummary)
                {
                    StatementSummaryViewModel data = new StatementSummaryViewModel();
                    data.BillingPeriod = stmt.StartDate.ToString(Resource.constFormatDate) + " - " + stmt.EndDate.ToString(Resource.constFormatDate);
                    data.BillingStatementId = stmt.BillingStatementId;
                    data.CustomerCompany = stmt.CustomerCompany;
                    data.DueDate = stmt.DueDate.ToString(Resource.constFormatDate);
                    data.Id = stmt.Id;
                    data.InvoiceCount = stmt.InvoiceCount;
                    data.StatementDate = stmt.StatementDate.ToString(Resource.constFormatDate);
                    data.StatementNumber = stmt.StatementNumber;
                    data.TotalQuantityDropped = stmt.TotalQuantityDropped.GetCommaSeperatedValue();
                    data.TotalStatementValue = stmt.TotalStatementValue;
                    data.TotalCount = stmt.TotalCount;
                    data.ScheduleId = stmt.ScheduleId;

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BillingStatementDomain", "GetStatementSummary", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> EditStatementAsync(int userId, StatementEditViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            try
            {
                var oldStatement = await Context.DataContext.BillingStatements.Where(t => t.Id == viewModel.StatementId).FirstOrDefaultAsync();

                var invoiceDetails = await Context.DataContext.Invoices.Where(t => viewModel.SelectedInvoices.Contains(t.Id)).Select(t => new { t.DroppedGallons, t.BasicAmount, TotalFeeAmount = t.TotalFeeAmount ?? 0, t.TotalTaxAmount, t.TotalDiscountAmount }).ToListAsync();

                var currentDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(oldStatement.TimeZoneName);

                BillingStatementRequestViewModel billingStatement = new BillingStatementRequestViewModel()
                {
                    BillingScheduleId = oldStatement.BillingScheduleId,
                    StmtStartDate = oldStatement.StartDate,
                    StmtEndDate = oldStatement.EndDate,
                    StmtDueDate = oldStatement.PaymentTermId == (int)PaymentTerms.DueOnReceipt ? currentDateTime : currentDateTime.AddDays(oldStatement.PaymentNetDays),
                    CreatedBy = userId,
                    CreatedDate = currentDateTime,
                    TotalQuantityDropped = invoiceDetails.Sum(t => t.DroppedGallons),
                    TotalStmtValue = invoiceDetails.Sum(t => t.BasicAmount) + invoiceDetails.Sum(t => t.TotalFeeAmount) + invoiceDetails.Sum(t => t.TotalTaxAmount) - invoiceDetails.Sum(t => t.TotalDiscountAmount),
                    Currency = oldStatement.Currency,
                    Uom = oldStatement.UoM,
                    ExchangeRate = oldStatement.ExchangeRate,
                    VersionNumber = oldStatement.VersionNumber + 1,
                    StmtChainId = oldStatement.StatementChainId,
                    InvoiceIds = viewModel.SelectedInvoices,
                    IsGenerated = true,
                    PaymentTermId = oldStatement.PaymentTermId,
                    PaymentNetDays = oldStatement.PaymentNetDays,
                    TimeZoneName = oldStatement.TimeZoneName,
                    FrequencyType = oldStatement.FrequencyTypeId,
                    CreatedCompany = oldStatement.CreatedCompany
                };
                int newStatementId = await CreateBillingStatement(billingStatement, oldStatement);
                if (newStatementId != 0)
                {
                    NotificationDomain notificationDomain = new NotificationDomain(this);
                    await notificationDomain.AddNotificationEventAsync(EventType.BillingStatementUpdated, newStatementId, userId);

                    viewModel.StatementId = newStatementId;
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errEditStatementSuccess;
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errEditStatementFailed;
                LogManager.Logger.WriteException("BillingStatementDomain", "EditStatementAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<BillingStatementDetailsViewModel> GetStatementDetailsAsync(int statementId, UserContext userContext)
        {
            CheckEntityAccess(userContext, statementId, EntityType.BillingStatement);
            BillingStatementDetailsViewModel response = new BillingStatementDetailsViewModel(Status.Failed);
            try
            {
                var statement = await Context.DataContext.BillingStatements.Where(t => t.Id == statementId).Select(t => new
                {
                    t.Id,
                    BillingStatementId = t.BillingSchedule != null ? t.BillingSchedule.BillingStatementId : "-",
                    t.BillingStatementXInvoices.FirstOrDefault().Invoice.Order.BuyerCompany.Name,
                    t.StatementNumber,
                    t.StartDate,
                    t.EndDate,
                    t.PaymentDueDate,
                    t.TotalQuantityDropped,
                    t.TotalStatementValue,
                    t.CreatedDate
                }).FirstOrDefaultAsync();

                if (statement != null)
                {
                    response.BillingPeriod = statement.StartDate.ToString(Resource.constFormatDate) + " - " + statement.EndDate.ToString(Resource.constFormatDate);
                    response.BillingScheduleId = statement.BillingStatementId;
                    response.Id = statement.Id;
                    response.StmtDueDate = statement.PaymentDueDate.ToString(Resource.constFormatDate);
                    response.TotalQuantityDropped = statement.TotalQuantityDropped.GetPreciseValue(6);
                    response.TotalStmtValue = statement.TotalStatementValue.GetPreciseValue(6);
                    response.Customer = statement.Name;
                    response.StatementNumber = statement.StatementNumber.Number;
                    response.StatementDate = statement.CreatedDate.ToString(Resource.constFormatDate);
                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errLoadStatementDetailsFailed;
                LogManager.Logger.WriteException("BillingStatementDomain", "GetStatementSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<StatementInvoiceDetails>> GetStatementInvoicesAsync(int statementId)
        {
            List<StatementInvoiceDetails> response = new List<StatementInvoiceDetails>();
            try
            {
                var invoices = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetStatementInvoiceSummary(statementId);

                foreach (var invoice in invoices)
                {
                    StatementInvoiceDetails invoiceDetails = new StatementInvoiceDetails();
                    invoiceDetails.DropDate = invoice.DropDate.ToString(Resource.constFormatDate);
                    invoiceDetails.FuelType = invoice.FuelType;
                    invoiceDetails.InvoiceId = invoice.Id;
                    invoiceDetails.InvoiceNumber = invoice.Number;
                    invoiceDetails.InvoiceStatus = invoice.Status;
                    invoiceDetails.PoNumber = invoice.PoNumber;
                    invoiceDetails.TotalAmount = invoice.TotalAmount.GetPreciseValue(6);
                    invoiceDetails.TotalQuantityDropped = invoice.DroppedGallons.GetPreciseValue(6).GetCommaSeperatedValue();
                    response.Add(invoiceDetails);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BillingStatementDomain", "GetStatementInvoicesAsync", ex.Message, ex);
            }
            return response;
        }

        protected Dictionary<DateTimeOffset, DateTimeOffset> GetStatementCycle(DateTimeOffset stmtStartDate, DateTimeOffset currentDate, int frequencyType)
        {
            Dictionary<DateTimeOffset, DateTimeOffset> response = new Dictionary<DateTimeOffset, DateTimeOffset>();
            int days = GetDaysBasedOnCycle(frequencyType);
            while (stmtStartDate < currentDate)
            {
                switch (frequencyType)
                {
                    case (int)FrequencyTypes.Daily:
                    case (int)FrequencyTypes.Weekly:
                    case (int)FrequencyTypes.Biweekly:
                        var stmtEndDate = stmtStartDate.AddDays(days).AddTicks(-1);
                        if (stmtEndDate < currentDate.AddMinutes(ApplicationConstants.StatementGenerationWaitingTime))
                        {
                            response.Add(stmtStartDate, stmtEndDate);
                        }
                        stmtStartDate = stmtStartDate.AddDays(days);
                        break;
                    case (int)FrequencyTypes.Monthly:
                        var statementEndDate = stmtStartDate.AddMonths(1).AddTicks(-1);
                        if (statementEndDate < currentDate.AddMinutes(ApplicationConstants.StatementGenerationWaitingTime))
                        {
                            response.Add(stmtStartDate, statementEndDate);
                        }
                        stmtStartDate = stmtStartDate.AddMonths(1);
                        break;
                }
            }
            return response;
        }

        protected async Task<BillingStatementRequestViewModel> GetStatementRequestViewModel(int billingScheduleId, DateTimeOffset stmtStartDate, DateTimeOffset stmtEndDate, DateTimeOffset currentDate)
        {
            BillingStatementRequestViewModel response = new BillingStatementRequestViewModel();
            var scheduleDetails = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetGenerateStatementDetails(billingScheduleId, stmtStartDate, stmtEndDate);

            if (scheduleDetails.Any())
            {
                var billingSchedule = scheduleDetails.First();
                response = new BillingStatementRequestViewModel()
                {
                    BillingScheduleId = billingSchedule.Id,
                    StmtStartDate = stmtStartDate,
                    StmtEndDate = stmtEndDate,
                    StmtDueDate = billingSchedule.PaymentTermId == (int)PaymentTerms.DueOnReceipt ? currentDate : currentDate.AddDays(billingSchedule.PaymentNetDays),
                    CreatedBy = billingSchedule.CreatedBy,
                    CreatedDate = currentDate,
                    TotalQuantityDropped = scheduleDetails.Sum(t => t.DroppedGallons),
                    TotalStmtValue = scheduleDetails.Sum(t => t.BasicAmount) + scheduleDetails.Sum(t => t.TotalFeeAmount) + scheduleDetails.Sum(t => t.TotalTaxAmount) - scheduleDetails.Sum(t => t.TotalDiscountAmount),
                    Currency = billingSchedule.Currency,
                    Uom = billingSchedule.UoM,
                    ExchangeRate = billingSchedule.ExchangeRate,
                    VersionNumber = 1,
                    StmtChainId = GetStatementChainId(string.Empty, billingSchedule.CreatedBy),
                    InvoiceIds = scheduleDetails.Select(t => t.InvoiceId).ToList(),
                    IsGenerated = true,
                    PaymentTermId = billingSchedule.PaymentTermId,
                    PaymentNetDays = billingSchedule.PaymentNetDays,
                    TimeZoneName = billingSchedule.TimeZoneName,
                    FrequencyType = billingSchedule.FrequencyTypeId,
                    CreatedCompany = billingSchedule.CreatedByCompanyId
                };
            }
            return response;
        }

        public async Task CreateBillingStatementForEditedInvoice(int invoiceNumberId, int userId)
        {
            try
            {
                // get all invoices with this invoicenumberid
                var invoices = Context.DataContext.Invoices.Where(t => t.InvoiceHeader.InvoiceNumberId == invoiceNumberId).
                    Select(t => new
                    {
                        t.Id,
                        t.DroppedGallons,
                        Amount = t.BasicAmount + (t.TotalFeeAmount ?? 0) + t.TotalTaxAmount - t.TotalDiscountAmount,
                        t.IsActive
                    }).OrderByDescending(t => t.Id).ToList();

                List<int> invoiceIds = invoices.Select(t => t.Id).ToList();

                // get last active statement which has this invoice in it -- s1
                var lastInvoiceStatement = Context.DataContext.BillingStatementXInvoices
                                    .OrderByDescending(bsi => bsi.BillingStatement.Id)
                                    .Where(bsi => invoiceIds.Contains(bsi.InvoiceId) && bsi.IsActive && bsi.BillingStatement.IsGenerated && !bsi.BillingStatement.ParentId.HasValue)
                                    .Select(t => new
                                    {
                                        t.BillingStatement,
                                        t.BillingStatement.BillingSchedule
                                    })
                                    .FirstOrDefault();

                // if invoiceBillingStatement is not sent till now - make it inactive & add new record as active and this new version
                if (lastInvoiceStatement != null) // means there is some statement configured for this - s1 exists
                {
                    var existingBillingStatement = Context.DataContext.BillingStatements.Any(t => t.ParentId.HasValue &&
                                        t.ParentBillingStatement.StatementChainId.Equals(lastInvoiceStatement.BillingStatement.StatementChainId) &&
                                        !t.IsGenerated);
                    if (!existingBillingStatement) // statement does not exist - create new stmt
                    {
                        var currentDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(lastInvoiceStatement.BillingStatement.TimeZoneName);
                        BillingStatementRequestViewModel viewModel = new BillingStatementRequestViewModel
                        {
                            BillingScheduleId = lastInvoiceStatement.BillingStatement.BillingScheduleId,
                            StmtStartDate = lastInvoiceStatement.BillingStatement.StartDate,
                            StmtEndDate = lastInvoiceStatement.BillingStatement.EndDate,
                            StmtDueDate = lastInvoiceStatement.BillingStatement.PaymentTermId == (int)PaymentTerms.DueOnReceipt ? currentDateTime : currentDateTime.AddDays(lastInvoiceStatement.BillingStatement.PaymentNetDays),
                            CreatedBy = userId,
                            CreatedDate = currentDateTime,
                            TotalQuantityDropped = 0,
                            TotalStmtValue = 0,
                            Currency = lastInvoiceStatement.BillingStatement.Currency,
                            Uom = lastInvoiceStatement.BillingStatement.UoM,
                            ExchangeRate = lastInvoiceStatement.BillingStatement.ExchangeRate,
                            VersionNumber = lastInvoiceStatement.BillingStatement.VersionNumber + 1,
                            StmtChainId = lastInvoiceStatement.BillingStatement.StatementChainId,
                            InvoiceIds = new List<int>(),
                            ParentId = lastInvoiceStatement.BillingStatement.Id,
                            IsGenerated = false,
                            PaymentNetDays = lastInvoiceStatement.BillingStatement.PaymentNetDays,
                            PaymentTermId = lastInvoiceStatement.BillingStatement.PaymentTermId,
                            TimeZoneName = lastInvoiceStatement.BillingStatement.TimeZoneName,
                            FrequencyType = lastInvoiceStatement.BillingStatement.FrequencyTypeId,
                            CreatedCompany = lastInvoiceStatement.BillingStatement.CreatedCompany
                        };
                        await CreateBillingStatement(viewModel, lastInvoiceStatement.BillingStatement);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BillingStatementDomain", "CreateBillingStatementForEditedInvoice", ex.Message, ex);
            }
        }

        protected async Task<int> CreateBillingStatement(BillingStatementRequestViewModel viewModel, BillingStatement oldStatement = null)
        {
            int statementId = 0;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    //Statement Number
                    var statementNumber = new StatementNumber();
                    statementNumber = GetStatementNumber(statementNumber, oldStatement);

                    var billingStatement = GetBillingStatement(viewModel); //Statement
                    statementNumber.BillingStatements.Add(billingStatement);

                    foreach (var invoiceId in viewModel.InvoiceIds)
                    {
                        BillingStatementXInvoice stmtXInvoice = new BillingStatementXInvoice() { InvoiceId = invoiceId, IsActive = true };
                        billingStatement.BillingStatementXInvoices.Add(stmtXInvoice);
                    }

                    if (oldStatement != null && viewModel.IsGenerated)
                    {
                        oldStatement.IsActive = false;
                    }

                    await Context.CommitAsync();
                    transaction.Commit();
                    statementId = billingStatement.Id;
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("BillingStatementDomain", "CreateBillingStatement", ex.Message, ex);
                }
            }
            return statementId;
        }

        public async Task<StatementPdfViewModel> GetStatementPdfDetailsAsync(int statementId)
        {
            StatementPdfViewModel response = new StatementPdfViewModel(Status.Failed);
            try
            {
                var stmtDetails = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetStatementPdfDetails(statementId);
                var invoices = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetStatementPdfInvoiceDetails(statementId);
                if (stmtDetails != null)
                {
                    List<StatementInvoiceDetails> statementSummary = new List<StatementInvoiceDetails>();
                    response = new StatementPdfViewModel(Status.Success)
                    {
                        BillingPeriod = stmtDetails.StartDate.ToString(Resource.constFormatDate) + " - " + stmtDetails.EndDate.ToString(Resource.constFormatDate),
                        ContactPerson = new ContactPersonViewModel() { Name = stmtDetails.ContactPerson, Email = stmtDetails.Email, PhoneNumber = stmtDetails.PhoneNumber },
                        CustomerAddress = new AddressViewModel() { Address = stmtDetails.CompanyAddress, City = stmtDetails.City, StateCode = stmtDetails.StateCode, ZipCode = stmtDetails.ZipCode },
                        CustomerCompany = stmtDetails.CustomerCompany,
                        CustomerId = ApplicationConstants.CustomerNumberPrefix + stmtDetails.CustomerId.ToString(ApplicationConstants.SevenDigit),
                        Id = stmtDetails.Id,
                        Version = stmtDetails.VersionNumber,
                        IsActive = stmtDetails.IsActive,
                        StatementNumber = stmtDetails.StatementNumber,
                        StatementId = stmtDetails.BillingStatementId,
                        DueDate = stmtDetails.DueDate.ToString(Resource.constFormatDate),
                        StatementDate = stmtDetails.StatementDate.ToString(Resource.constFormatDate),
                        StatementReceipt = stmtDetails.PaymentTermId == (int)PaymentTerms.DueOnReceipt ? Resource.lblDueOnReceipt : Resource.lblNet + " " + stmtDetails.PaymentNetDays + " " + Resource.lblDays,
                        StatementValue = stmtDetails.TotalStatementValue,
                        SupplierCompany = stmtDetails.SupplierCompany,
                        SupplierAddress = new AddressViewModel() { Address = stmtDetails.SupplierAddress, City = stmtDetails.SupplierCity, StateCode = stmtDetails.SupplierStateCode, ZipCode = stmtDetails.SupplierZipCode },
                        PhoneNumber = stmtDetails.SupplierPhoneNumber,
                        Image = stmtDetails.CompanyLogo != null ? new ImageViewModel() { Data = stmtDetails.CompanyLogo } : null,
                        Culture = SetThreadCulture(stmtDetails.currency),
                        Invoices = GetInvoiceDetails(invoices, statementSummary)
                    };

                    GetInvoicePaymentSummary(response);
                    response.StatementSummary = statementSummary;
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errLoadStatementDetailsFailed;
                LogManager.Logger.WriteException("BillingStatementDomain", "GetStatementTabDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        private void GetInvoicePaymentSummary(StatementPdfViewModel statementPdfViewModel)
        {
            if (statementPdfViewModel.InvoicePaymentSummary == null)
                statementPdfViewModel.InvoicePaymentSummary = new List<InvoicePaymentViewModel>();
            foreach (var item in statementPdfViewModel.Invoices.Where(t => t.InvoicePayments != null))
            {
                statementPdfViewModel.InvoicePaymentSummary.AddRange(item.InvoicePayments);
            }
        }

        private BillingStatement GetBillingStatement(BillingStatementRequestViewModel viewModel)
        {
            BillingStatement billingStatement = new BillingStatement()
            {
                BillingScheduleId = viewModel.BillingScheduleId,
                StartDate = viewModel.StmtStartDate,
                EndDate = viewModel.StmtEndDate,
                PaymentDueDate = viewModel.StmtDueDate,
                CreatedBy = viewModel.CreatedBy,
                CreatedDate = viewModel.CreatedDate,
                TotalQuantityDropped = viewModel.TotalQuantityDropped,
                TotalStatementValue = viewModel.TotalStmtValue,
                Currency = viewModel.Currency,
                UoM = viewModel.Uom,
                ExchangeRate = viewModel.ExchangeRate,
                StatementChainId = viewModel.StmtChainId,
                VersionNumber = viewModel.VersionNumber,
                IsPaid = false,
                IsActive = true,
                ParentId = viewModel.ParentId,
                IsGenerated = viewModel.IsGenerated,
                PaymentTermId = viewModel.PaymentTermId,
                PaymentNetDays = viewModel.PaymentNetDays,
                TimeZoneName = viewModel.TimeZoneName,
                FrequencyTypeId = viewModel.FrequencyType,
                CreatedCompany = viewModel.CreatedCompany
            };
            return billingStatement;
        }

        private StatementNumber GetStatementNumber(StatementNumber statementNumber, BillingStatement oldStatement = null)
        {
            if (oldStatement == null)
            {
                Context.DataContext.StatementNumbers.Add(statementNumber);
            }
            else
            {
                statementNumber = oldStatement.StatementNumber;
            }
            return statementNumber;
        }

        private List<StatementInvoiceViewModel> GetInvoiceDetails(List<UspStatementPdfInvoiceDetails> statementInvoices, List<StatementInvoiceDetails> statementSummary)
        {
            List<StatementInvoiceViewModel> response = new List<StatementInvoiceViewModel>();
            InvoiceDomain invoiceDomain = new InvoiceDomain(this);
            if (statementInvoices.Any())
            {
                var invoiceMap = statementInvoices.GroupBy(t => t.InvoiceNumber);
                foreach (var invoiceId in invoiceMap)
                {
                    StatementInvoiceViewModel viewModel = new StatementInvoiceViewModel();
                    var invoices = invoiceId.ToList();
                    var invoice = invoices.FirstOrDefault();
                    viewModel.InvoiceNumber = invoice.DisplayInvoiceNumber;
                    viewModel.DisplayJobID = invoice.DisplayJobID;
                    viewModel.WBSNumber = string.IsNullOrWhiteSpace(invoice.CustomAttribute) ? string.Empty : JsonConvert.DeserializeObject<InvoiceCustomAttributeViewModel>(invoice.CustomAttribute).WBSNumber;
                    viewModel.Version = invoice.Version;
                    viewModel.IsHidePricingEnabled = false;
                    viewModel.WaitingForAction = invoice.WaitingFor;
                    viewModel.IsApprovalWorkflowEnabledForJob = invoice.IsApprovalWorkflowEnabled;
                    viewModel.Currency = invoice.Currency;
                    viewModel.InvoiceTypeId = invoice.InvoiceTypeId;
                    viewModel.TerminalName = GetTerminalName(invoice.TerminalName, invoice.CityGroupTerminalName);
                    viewModel.PoNumber = invoice.PoNumber;
                    viewModel.DropDate = GetDropDate(invoice.DropStartDate, invoice.DropEndDate);
                    viewModel.BasicAmount = invoice.BasicAmount + (invoice.TotalAllowance ?? 0);
                    viewModel.IsBuyAndSellOrder = invoice.ExternalBrokerOrder != null;
                    viewModel.BuySellDetails = invoice.ExternalBrokerOrder != null ? GetBuySellPricingDetail(invoice) : null;
                    viewModel.FuelDetails = GetFuelDetails(invoice);
                    viewModel.PricePerGallonDisplay = invoice.PricePerGallonDisplay;
                    viewModel.DroppedGallons = invoice.DroppedGallons;
                    viewModel.PricePerGallon = invoice.PricePerGallon;
                    viewModel.TotalFees = invoice.TotalFeeAmount ?? 0;
                    viewModel.TotalTaxAmount = invoice.TotalTaxAmount;
                    viewModel.TotalAllowance = invoice.TotalAllowance;
                    viewModel.SurchargePercentage = invoice.SurchargePercentage ?? 0;
                    viewModel.SurchargePricingType = (FuelSurchagePricingType)invoice.SurchargePricingType;
                    viewModel.TotalDiscountAmount = invoice.TotalDiscountAmount;
                    viewModel.IsFTL = invoice.IsFTL;
                    viewModel.QbInvoiceNumber = invoice.QbInvoiceNumber;
                    viewModel.FtlDetails = new StatementFtlDetailsViewModel { BOL = invoice.BOL, GrossGallons = invoice.GrossGallons ?? 0, NetGallons = invoice.NetGallons ?? 0, LiftQuantity = invoice.LiftQuantity ?? 0, LiftTicketNumber = invoice.LiftTicketNumber };
                    if ((invoice.InvoiceStatus == InvoiceStatus.PartiallyPaid || invoice.InvoiceStatus == InvoiceStatus.Paid) && !string.IsNullOrWhiteSpace(invoice.QbInvoiceNumber))
                    {
                        viewModel.InvoicePayments = Task.Run(() => invoiceDomain.GetInvoicePayments(invoice.InvoiceNumberId, invoice.QbInvoiceNumber)).Result;
                    }
                    viewModel.Job = new StatementJobViewModel()
                    {
                        JobName = invoice.JobName,
                        Address = invoice.Address,
                        City = invoice.City,
                        StateCode = invoice.StateCode,
                        ZipCode = invoice.ZipCode
                    };
                    GetPickupLocation(invoices, viewModel);
                    GetShippingLocation(invoices, viewModel);
                    GetInvoiceFuelFees(invoices, viewModel);
                    GetAssets(invoices, viewModel);
                    GetSpecialInstructions(invoices, viewModel);
                    GetTaxDetails(invoices, viewModel);
                    statementSummary.Add(new StatementInvoiceDetails
                    {
                        InvoiceNumber = invoice.DisplayInvoiceNumber,
                        FuelType = invoice.FuelType,
                        DropDate = GetDropDate(invoice.DropStartDate, invoice.DropEndDate),
                        TotalQuantityDropped = invoice.DroppedGallons.GetPreciseValue(6).GetCommaSeperatedValue(),
                        Ppg = invoice.PricePerGallon.GetPreciseValue(6),
                        TotalAmount = GetInvoiceAmount(invoice.BasicAmount, invoice.TotalDiscountAmount, invoice.TotalFeeAmount ?? 0, invoice.TotalTaxAmount),
                        StatusId = invoice.InvoiceStatus
                    });
                    response.Add(viewModel);
                }
            }
            return response;
        }

        private void GetPickupLocation(List<UspStatementPdfInvoiceDetails> invoices, StatementInvoiceViewModel viewModel)
        {
            if (invoices.Any(t => t.IsFTL))
            {
                var pickupLocation = invoices.Where(t => t.DispatchLocationId != null && t.LocationType == (int)LocationType.PickUp).GroupBy(t => t.DispatchLocationId).SelectMany(t => t).FirstOrDefault();
                if (pickupLocation != null)
                {
                    viewModel.PickUpLocation = new AddressViewModel() { Address = pickupLocation.DispatchAddress, City = pickupLocation.DispatchLocation };
                }
                else
                {
                    var terminalAddress = invoices.Where(t => t.TerminalAddress != null).GroupBy(t => t.TerminalAddress).SelectMany(t => t).FirstOrDefault();
                    if (terminalAddress != null)
                    {
                        viewModel.PickUpLocation = new AddressViewModel() { Address = terminalAddress.TerminalAddress, City = terminalAddress.TerminalLocation };
                    }
                }
            }
        }

        private void GetShippingLocation(List<UspStatementPdfInvoiceDetails> invoices, StatementInvoiceViewModel viewModel)
        {
            var shippingLocation = invoices.Where(t => t.DispatchLocationId != null && t.LocationType == (int)LocationType.Drop).GroupBy(t => t.DispatchLocationId).SelectMany(t => t).FirstOrDefault();
            if (shippingLocation != null)
            {
                viewModel.ShippingLocation = new AddressViewModel() { Address = shippingLocation.DispatchAddress, City = shippingLocation.DispatchLocation };
            }
        }

        private void GetTaxDetails(List<UspStatementPdfInvoiceDetails> invoices, StatementInvoiceViewModel viewModel)
        {
            var taxes = invoices.GroupBy(t => t.TaxId).Select(t => t.FirstOrDefault());

            viewModel.TaxDetails.AddRange(taxes.Where(t => t.TaxId != null && t.TaxExemptionInd != ApplicationConstants.AvaTaxExemptedInd).Select(t => new StatementTaxDetailsViewModel()
            {
                IsModified = t.IsModified,
                TradingTaxAmount = t.TradingTaxAmount.Value.GetPreciseValue(2),
                RateDescription = t.RateDescription
            }));
        }

        private void GetInvoiceFuelFees(List<UspStatementPdfInvoiceDetails> invoices, StatementInvoiceViewModel viewModel)
        {
            var fees = invoices.GroupBy(t => t.FeeId).Select(t => t.FirstOrDefault());
            InvoiceDomain invoiceDomain = new InvoiceDomain(this);

            viewModel.FuelRequestFees.AddRange(fees.Where(t => t.FeeId != null && t.FeeSubTypeId != (int)FeeSubType.NoFee &&
                                                                        t.FeeTypeId != (int)FeeType.DryRunFee)
            .OrderBy(t => t.FeeTypeId)
            .Select(t => new StatementFeesViewModel()
            {
                FeeSubTypeId = t.FeeSubTypeId.Value,
                FeeTypeId = t.FeeTypeId.ToString(),
                FeeTypeName = t.Currency == Currency.CAD ? t.FeeTypeName.Replace(Constants.Gallon, Constants.Litre) : t.FeeTypeName,
                IncludeInPPG = t.IncludeInPPG,
                TotalFee = t.TotalFee ?? 0,
                FeeSubTypeName = t.Currency == Currency.CAD ? t.FeeSubTypeName.Replace(Constants.Gallon, Constants.Litre) : t.FeeSubTypeName,
                Fee = t.Fee.Value.GetPreciseValue(6),
                MinimumGallons = (t.MinimumGallons ?? 0).GetPreciseValue(6),
                FeeSubQuantity = t.FeeSubQuantity ?? 0,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                OtherFeeDescription = t.OtherFeeDescription
            }));
            viewModel.DiscountLineItems.AddRange(fees.Where(t => t.FeeId != null && t.DiscountLineItemId != null).OrderBy(t => t.FeeTypeId).
                Select(t => new StatementDiscountLineItemViewModel()
                {
                    FeeTypeName = t.Currency == Currency.CAD ? t.FeeTypeName.Replace(Constants.Gallon, Constants.Litre) : t.FeeTypeName,
                    FeeSubTypeId = t.FeeSubTypeId.Value,
                    Amount = t.Fee.Value.GetPreciseValue(6),
                    TotalFee = t.TotalFee ?? 0,
                    FeeSubTypeName = t.Currency == Currency.CAD ? t.FeeSubTypeName.Replace(Constants.Gallon, Constants.Litre).Replace(Resource.lblFlatFee, Resource.lblFlat) : t.FeeSubTypeName.Replace(Resource.lblFlatFee, Resource.lblFlat),
                }));
            var frieghtFee = fees.Where(t => t.FeeTypeId == (int)FeeType.FreightFee).OrderByDescending(t => t.FeeId).FirstOrDefault();
            if (frieghtFee != null)
            {
                viewModel.FreightFeeSubTypeId = frieghtFee.FeeSubTypeId.Value;
                viewModel.FreightFee = frieghtFee.Fee.Value.GetPreciseValue(6);
            }
            if (invoices.First().IsWetHosingDelivery)
            {
                var wetHosingFee = fees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.WetHoseFee && t.DiscountLineItemId == null);
                if (wetHosingFee != null && wetHosingFee.FeeSubQuantity.HasValue)
                {
                    viewModel.WetHoseFee = wetHosingFee.TotalFee ?? 0;

                    if (wetHosingFee.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
                    {
                        viewModel.WetHoseAssetQuantity = Convert.ToInt64(wetHosingFee.FeeSubQuantity.Value);
                    }
                    else if (wetHosingFee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                    {
                        viewModel.WetHoseHours = invoiceDomain.GetHosingTimeInHours(wetHosingFee.FeeSubQuantity.Value.ToString());
                    }
                }
            }
            if (invoices.First().IsOverWaterDelivery)
            {
                var overWaterFee = fees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.OverWaterFee && t.DiscountLineItemId == null);
                if (overWaterFee != null && overWaterFee.FeeSubQuantity.HasValue)
                {
                    viewModel.OverWaterFee = overWaterFee.TotalFee ?? 0;

                    if (overWaterFee.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
                    {
                        viewModel.OverWaterAssetQuantity = Convert.ToInt64(overWaterFee.FeeSubQuantity.Value);
                    }
                    else if (overWaterFee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                    {
                        viewModel.OverWaterHours = invoiceDomain.GetHosingTimeInHours(overWaterFee.FeeSubQuantity.Value.ToString());
                    }
                }
            }

            foreach (var fee in viewModel.FuelRequestFees)
            {
                if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate && (fee.FeeSubQuantity != null && fee.FeeSubQuantity.Value != 0))
                {
                    fee.TotalHours = invoiceDomain.GetHosingTimeInHours(fee.FeeSubQuantity.Value.ToString());
                }
            }
        }

        private void GetSpecialInstructions(List<UspStatementPdfInvoiceDetails> invoices, StatementInvoiceViewModel viewModel)
        {
            var specialInstructions = invoices.GroupBy(t => t.SpecialInstructionId).Select(t => t.FirstOrDefault());
            foreach (var instruction in specialInstructions)
            {
                if (instruction != null && instruction.SpecialInstructionId != null)
                {
                    viewModel.SpecialInstructions.Add(new StatementSpecialInstructionViewModel()
                    {
                        IsInstructionFollowed = instruction.IsInstructionFollowed,
                        Instruction = instruction.Instruction
                    });
                }
            }
        }

        private void GetAssets(List<UspStatementPdfInvoiceDetails> invoices, StatementInvoiceViewModel viewModel)
        {
            var assets = invoices.GroupBy(t => t.AssetDropId).Select(t => t.FirstOrDefault());
            List<StatementAssetViewModel> assetViewModel = new List<StatementAssetViewModel>();
            foreach (var asset in assets)
            {
                if (asset != null && asset.AssetDropId != null)
                {
                    assetViewModel.Add(new StatementAssetViewModel()
                    {
                        DropStatus = asset.DropStatus.Value,
                        AssetName = asset.AssetName,
                        VehicleId = asset.VehicleId,
                        DropStartDate = asset.AssetDropStartDate.Value,
                        DropEndDate = asset.AssetDropEndDate.Value,
                        DropGallons = asset.AssetDroppedGallons.Value,
                        SubcontractorName = asset.SubcontractorName,
                        IsNewAsset = asset.IsNewAsset,
                        AssetDropId = asset.AssetDropId.Value
                    });
                }
            }
            if (viewModel.FreightFeeSubTypeId == (int)FeeSubType.ByAssetCount)
            {
                viewModel.FreightFeeAmount = (viewModel.FreightFee * assetViewModel.Count(t => t.DropStatus == (int)DropStatus.None));
            }
            else if (viewModel.FreightFeeSubTypeId == (int)FeeSubType.PerGallon)
            {
                viewModel.FreightFeeAmount = (viewModel.FreightFee * viewModel.DroppedGallons);
            }
            else
            {
                viewModel.FreightFeeAmount = viewModel.FreightFee;
            }
            viewModel.NoFuelNeededAssetCount = assetViewModel.Count(t => t.DropStatus == (int)DropStatus.NoFuelNeeded);
            viewModel.AssetNotAvailableCount = assetViewModel.Count(t => t.DropStatus == (int)DropStatus.AssetNotAvailable);
            viewModel.Assets = assetViewModel.Where(t => t.DropStatus == (int)DropStatus.None || t.DropStatus == (int)DropStatus.UnplannedTankDrop).OrderBy(t => t.DropStartDate).Select(t => new StatementAssetDropViewModel()
            {
                AssetName = string.Format(@"{0}{1}{2}",
                                        t.AssetName.Replace("-" + t.VehicleId, ""),
                                        !string.IsNullOrWhiteSpace(t.AssetName) && !string.IsNullOrWhiteSpace(t.VehicleId) ? Resource.lblSingleHyphen : string.Empty,
                                        t.VehicleId),
                DropDate = t.DropStartDate,
                EndTime = t.DropEndDate.DateTime.ToString(Resource.constFormat12HourTime2),
                DropGallons = t.DropGallons.GetPreciseValue(6),
                SubcontractorName = t.SubcontractorName ?? string.Empty,
                IsNewAsset = t.IsNewAsset,
            }).GroupBy(t => t.AssetName).Select(t => t.ToList()).ToList();
        }

        private StatementBuyAndSellPricingDetailViewModel GetBuySellPricingDetail(UspStatementPdfInvoiceDetails invoice)
        {
            StatementBuyAndSellPricingDetailViewModel response = new StatementBuyAndSellPricingDetailViewModel();
            response.IsBuyPriceInvoice = invoice.IsBuyPriceInvoice;
            decimal brokerMarkUp = invoice.BrokerMarkUp.Value;
            decimal supplierMarkUp = invoice.SupplierMarkUp.Value;

            if (invoice.IsBuyPriceInvoice)
            {
                response.BasePrice = invoice.PricePerGallon - brokerMarkUp;
            }
            else
            {
                response.BasePrice = invoice.PricePerGallon - brokerMarkUp - supplierMarkUp;
            }
            response.BrokerMarkUp = brokerMarkUp;
            response.SupplierMarkUp = supplierMarkUp;
            response.BuyPrice = response.BasePrice + brokerMarkUp;
            response.SellPrice = response.BuyPrice + supplierMarkUp;
            response.BuyPriceDetail = $"{Resource.lblBasePrice} + {Resource.constSymbolCurrency}{brokerMarkUp.ToString(ApplicationConstants.DecimalFormat2)}";
            response.SellPriceDetail = $"{Resource.lblBuyPrice} + {Resource.constSymbolCurrency}{supplierMarkUp.ToString(ApplicationConstants.DecimalFormat2)}";
            return response;
        }

        private StatementFuelDetailsViewModel GetFuelDetails(UspStatementPdfInvoiceDetails invoice)
        {
            StatementFuelDetailsViewModel response = new StatementFuelDetailsViewModel();
            if (invoice.FuelDisplayGroup == (int)ProductDisplayGroups.OtherFuelType || invoice.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
            {
                response.FuelDescription = invoice.FuelDescription;
            }
            response.FuelType = invoice.FuelType;
            response.UoM = invoice.UoM;
            response.PricingTypeId = invoice.PricingTypeId;
            response.FuelDisplayGroupId = invoice.FuelDisplayGroup == (int)ProductDisplayGroups.FuelTypesInYourArea ? invoice.ProductDisplayGroupId : invoice.FuelDisplayGroup;
            return response;
        }

        private string GetTerminalName(string terminal, string cityGroupTerminal)
        {
            string response = string.Empty;
            if (!string.IsNullOrEmpty(terminal))
                response = terminal;
            if (!string.IsNullOrEmpty(cityGroupTerminal))
                response = cityGroupTerminal;
            return response;
        }

        private decimal GetInvoiceAmount(decimal basicAmount, decimal discountAmount, decimal fee, decimal tax)
        {
            return (basicAmount + fee + tax - discountAmount).GetPreciseValue(6);
        }

        private string GetDropDate(DateTimeOffset dropStartDate, DateTimeOffset dropEndDate)
        {
            return dropEndDate.ToString(Resource.constFormatDate) + " " + dropStartDate.DateTime.ToShortTimeString() + " - " + dropEndDate.DateTime.ToShortTimeString();
        }

        private string SetThreadCulture(Currency currency)
        {
            string culture = currency == Currency.USD ? ApplicationConstants.Culture_USA : ApplicationConstants.Culture_CANADA;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            return culture;
        }

        private int GetDaysBasedOnCycle(int frequencyType)
        {
            int response = 0;
            switch (frequencyType)
            {
                case (int)FrequencyTypes.Daily:
                    response = 1;
                    break;
                case (int)FrequencyTypes.Weekly:
                    response = 7;
                    break;
                case (int)FrequencyTypes.Biweekly:
                    response = 14;
                    break;
            }
            return response;
        }

        private static string GetStatementChainId(string stmtChainId, int createdBy)
        {
            if (string.IsNullOrWhiteSpace(stmtChainId))
            {
                stmtChainId = DateTimeOffset.Now.ToString("yyyyMMddHHmmssFFFFFF") + createdBy;
            }
            return stmtChainId;
        }

        public async Task<List<StatementHistoryViewModel>> GetStatementHistoryAsync(int statementId)
        {
            List<StatementHistoryViewModel> response = new List<StatementHistoryViewModel>();
            try
            {
                var stmtDetails = await Context.DataContext.BillingStatements.Where(t => t.Id == statementId)
                                    .Select(t => new
                                    {
                                        t.VersionNumber,
                                        t.StatementChainId,
                                        t.BillingStatementXInvoices.FirstOrDefault().Invoice.Order.BuyerCompany.Name,
                                        BillingStatementId = t.BillingScheduleId != null ? t.BillingSchedule.BillingStatementId : "-"
                                    }).SingleOrDefaultAsync();

                if (stmtDetails != null)
                {
                    var listOfHistoryItems = await Context.DataContext.BillingStatements
                        .Where(t => t.StatementChainId == stmtDetails.StatementChainId && !t.IsActive && t.VersionNumber < stmtDetails.VersionNumber)
                        .Select(t => new
                        {
                            t.Id,
                            stmtDetails.BillingStatementId,
                            stmtDetails.VersionNumber,
                            stmtDetails.Name,
                            t.StatementNumber,
                            t.StartDate,
                            t.EndDate,
                            t.PaymentDueDate,
                            t.TotalQuantityDropped,
                            t.TotalStatementValue,
                            t.BillingStatementXInvoices.Count
                        }).ToListAsync();

                    if (listOfHistoryItems.Any())
                    {
                        foreach (var item in listOfHistoryItems)
                        {
                            response.Add(new StatementHistoryViewModel()
                            {
                                BillingPeriod = item.StartDate.ToString(Resource.constFormatDate) + " - " + item.EndDate.ToString(Resource.constFormatDate),
                                BillingScheduleId = item.BillingStatementId,
                                Id = item.Id,
                                StatementNumber = item.StatementNumber.Number,
                                StmtDueDate = item.PaymentDueDate.ToString(Resource.constFormatDate),
                                TotalQuantityDropped = item.TotalQuantityDropped.GetPreciseValue(6),
                                TotalStmtValue = item.TotalStatementValue.GetPreciseValue(6),
                                Customer = item.Name,
                                Version = Convert.ToString("V" + item.VersionNumber),
                                InvoiceCount = item.Count
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BillingStatementDomain", "GetStatementHistoryAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task GeneateBillingStatementForSplitLoadInvoice(List<Invoice> invoices, string timeZoneName, int acceptedCompanyId, BillingStatement oldStatement = null)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
            BillingStatementRequestViewModel billingStatement = GetStatementModelForSplitLoadInvoice(invoices, currentDate, timeZoneName, acceptedCompanyId, oldStatement);

            var statementNumber = new StatementNumber();
            statementNumber = GetStatementNumber(statementNumber);

            var billingStatementEntity = GetBillingStatement(billingStatement);
            statementNumber.BillingStatements.Add(billingStatementEntity);

            foreach (var invoiceId in billingStatement.InvoiceIds)
            {
                BillingStatementXInvoice stmtXInvoice = new BillingStatementXInvoice() { InvoiceId = invoiceId, IsActive = true };
                billingStatementEntity.BillingStatementXInvoices.Add(stmtXInvoice);
            }
            if (oldStatement != null)
            {
                oldStatement.IsActive = false;
            }
            await Context.CommitAsync();

            if (billingStatementEntity.Id != 0)
            {
                var eventType = oldStatement != null ? EventType.BillingStatementUpdated : EventType.BillingStatementGenerated;
                NotificationDomain notificationDomain = new NotificationDomain(this);
                await notificationDomain.AddNotificationEventAsync(eventType, billingStatementEntity.Id, billingStatement.CreatedBy);
            }
        }

        private static BillingStatementRequestViewModel GetStatementModelForSplitLoadInvoice(List<Invoice> invoices, DateTimeOffset currentDate, string timeZoneName, int supplierCompanyId, BillingStatement oldStatement)
        {
            Invoice invoice = invoices.First();
            return new BillingStatementRequestViewModel()
            {
                BillingScheduleId = null,
                StmtStartDate = currentDate,
                StmtEndDate = currentDate,
                StmtDueDate = invoice.PaymentTermId == (int)PaymentTerms.DueOnReceipt ? currentDate : currentDate.AddDays(invoice.NetDays),
                CreatedBy = invoice.UpdatedBy,
                CreatedDate = currentDate,
                TotalQuantityDropped = invoices.Sum(t => t.DroppedGallons),
                TotalStmtValue = invoices.Sum(t => t.BasicAmount) + invoices.Sum(t => t.TotalFeeAmount ?? 0) + invoices.Sum(t => t.TotalTaxAmount) - invoices.Sum(t => t.TotalDiscountAmount),
                Currency = invoice.Currency,
                Uom = invoice.UoM,
                ExchangeRate = invoice.ExchangeRate,
                VersionNumber = oldStatement != null ? oldStatement.VersionNumber + 1 : 1,
                StmtChainId = oldStatement != null ? oldStatement.StatementChainId : DateTimeOffset.Now.ToString("yyyyMMddHHmmssFFFFFF") + invoice.UpdatedBy,
                InvoiceIds = invoices.Select(t => t.Id).ToList(),
                IsGenerated = true,
                PaymentNetDays = invoice.NetDays,
                PaymentTermId = invoice.PaymentTermId,
                TimeZoneName = timeZoneName,
                FrequencyType = (int)FrequencyTypes.Daily,
                CreatedCompany = supplierCompanyId
            };
        }
    }
}
