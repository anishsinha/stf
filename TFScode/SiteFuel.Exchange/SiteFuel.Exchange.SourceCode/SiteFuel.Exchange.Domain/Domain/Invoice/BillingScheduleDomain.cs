using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Domain.Mappers.BillingStatement;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.BillingStatement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class BillingScheduleDomain : BillingStatementDomain
    {
        public BillingScheduleDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public BillingScheduleDomain(BaseDomain domain) : base(domain)
        {
        }

        public BillingScheduleViewModel GetBillingScheduleViewModel(UserContext userContext, int scheduleId)
        {
            BillingScheduleViewModel viewModel;
            if (scheduleId > 0)
            {
                var entity = Context.DataContext.BillingSchedules.Where(t => t.Id == scheduleId)
                    .Select(t => new
                    {
                        t.Id,
                        t.BillingStatementId,
                        t.TimeZoneName,
                        t.CreatedBy,
                        t.CreatedByCompanyId,
                        t.CreatedDate,
                        t.CustomerId,
                        t.FrequencyTypeId,
                        t.IsActive,
                        t.BillingScheduleXCustomerOrders,
                        t.PaymentTermId,
                        t.PaymentNetDays,
                        t.ScheduleChainId,
                        t.StartDate,
                        t.VersionNumber,
                        t.WeekDayId,
                        t.UpdateFrequencyType,
                        t.UpdateFrequencyValue,
                        t.CountryId,
                        t.IsIncludePreviousStatement,
                        IsStatementExists = t.BillingStatements.Any(t1 => t1.IsGenerated)
                    }).SingleOrDefault();
                //viewModel = entity.ToViewModel();

                viewModel = new BillingScheduleViewModel();
                if (entity != null)
                {
                    viewModel.Id = entity.Id;
                    viewModel.BillingStatementId = entity.BillingStatementId;
                    viewModel.CompanyTimeZone = entity.TimeZoneName;
                    viewModel.CreatedBy = entity.CreatedBy;
                    viewModel.CreatedByCompanyId = entity.CreatedByCompanyId;
                    viewModel.CreatedDate = entity.CreatedDate;
                    viewModel.CustomerId = entity.CustomerId;
                    viewModel.FrequencyTypeId = entity.FrequencyTypeId;
                    viewModel.IsActive = entity.IsActive;
                    viewModel.Orders = entity.BillingScheduleXCustomerOrders.Select(t => t.OrderId).ToList();
                    viewModel.PaymentNetDays = entity.PaymentNetDays;
                    viewModel.PaymentTermId = entity.PaymentTermId;
                    viewModel.ScheduleChainId = entity.ScheduleChainId;
                    viewModel.StartDate = entity.StartDate;
                    viewModel.TimeZone = entity.TimeZoneName;
                    viewModel.VersionNumber = entity.VersionNumber;
                    viewModel.WeekDayId = entity.WeekDayId;
                    viewModel.UpdateFrequencyTypeId = entity.UpdateFrequencyType;
                    viewModel.UpdateFrequencyValue = entity.UpdateFrequencyValue;
                    viewModel.CountryId = entity.CountryId;
                    viewModel.IsIncludePreviousStatement = entity.IsIncludePreviousStatement;
                    viewModel.IsStatmentExists = entity.IsStatementExists;
                }
            }
            else
            {
                viewModel = new BillingScheduleViewModel();
                viewModel.CompanyTimeZone = GetCompanysDefaultTimeZone(userContext.CompanyId);
                viewModel.CreatedByCompanyId = userContext.CompanyId;
                viewModel.CreatedBy = userContext.Id;
            }

            viewModel.IsCountryDropdownRequired = Context.DataContext.CompanyAddresses
                                                    .Where(t => t.CompanyId == userContext.CompanyId)
                                                    .Select(t => t.CountryId).Distinct().Count() > 1;
            //if (countries.Count > 1)
            //    viewModel.IsCountryDropdownRequired = true;
            //else
            //{
            //    viewModel.IsCountryDropdownRequired = false;
            //    viewModel.CountryId = countries.First();
            //}

            return viewModel;
        }

        private string GetCompanysDefaultTimeZone(int companyId)
        {
            var compAddress = Context.DataContext.CompanyAddresses
                        .Where(t => t.CompanyId == companyId && t.IsActive && t.IsDefault)
                        .Select(t => new { t.Latitude, t.Longitude }).FirstOrDefault();

            if (compAddress != null)
            {
                string timeZoneName = GoogleApiDomain.GetTimeZone(compAddress.Latitude, compAddress.Longitude);
                if (!string.IsNullOrEmpty(timeZoneName))
                {
                    return timeZoneName;
                }
            }
            return string.Empty;
        }

        public StatusViewModel CreateBillingSchedule(BillingScheduleViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                //add shedule
                viewModel.CreatedBy = userContext.Id;
                viewModel.CreatedByCompanyId = userContext.CompanyId;
                viewModel.VersionNumber = 1;
                var entity = viewModel.ToEntity();
                if (ValidateBillingSchedule(viewModel, response))
                {
                    if (entity != null)
                    {
                        Context.DataContext.BillingSchedules.Add(entity);
                        Context.DataContext.SaveChanges();

                        if (entity.BillingStatementId.Equals(ApplicationConstants.BillingStatementPrefix))
                        {
                            entity.BillingStatementId = ApplicationConstants.BillingStatementPrefix + entity.Id.ToString().PadLeft(7, '0');
                        }
                        Context.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageBillingScheduleCreated;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageBillingScheduleCreateFailed;
                LogManager.Logger.WriteException("BillingScheduleDomain", "CreateBillingSchedule", ex.Message, ex);
            }
            return response;
        }

        private bool ValidateBillingSchedule(BillingScheduleViewModel viewModel, StatusViewModel response)
        {
            var isValid = true;
            if (!string.IsNullOrWhiteSpace(viewModel.BillingStatementId))
            {
                var exists = Context.DataContext.BillingSchedules
                                    .Any(t => t.CreatedByCompanyId == viewModel.CreatedByCompanyId && t.IsActive
                                    && t.Id != viewModel.Id && t.BillingStatementId == viewModel.BillingStatementId);
                if (exists)
                {
                    isValid = false;
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageBillingStatementIdAlreadyExists;
                }
            }
            return isValid;
        }

        public async Task<List<BillingScheduleGridViewModel>> GetBillingScheduleGridAsync(UserContext userContext, BillingDataTableViewModel filter = null)
        {
            List<BillingScheduleGridViewModel> response = new List<BillingScheduleGridViewModel>();
            try
            {
                if (filter == null)
                {
                    filter = new BillingDataTableViewModel();
                }

                response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetBillingScheduleGridAsync(userContext.CompanyId, filter);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BillingScheduleDomain", "GetBillingScheduleGridAsync", ex.Message, ex);
            }

            return response;
        }

        public List<int> GetBillingSchedules()
        {
            return Context.DataContext.BillingSchedules.Where(t => t.IsActive).Select(t => t.Id).ToList();
        }

        public async Task ProcessBillingSchedulesAsync(int billingScheduleId)
        {
            try
            {
                var billingSchedule = await Context.DataContext.BillingSchedules.Where(t => t.Id == billingScheduleId)
                                                                                    .Select(t => new
                                                                                    {
                                                                                        t.Id,
                                                                                        t.StartDate,
                                                                                        t.TimeZoneName,
                                                                                        t.FrequencyTypeId,
                                                                                        LastStatement = t.BillingStatements.Where(t1 => t1.IsGenerated).OrderByDescending(x => x.EndDate).Select(t1 => new { t1.StartDate, t1.EndDate }).FirstOrDefault(),
                                                                                        BillingCycles = t.BillingStatements.Where(t1 => t1.IsGenerated).Select(t1 => new { t1.StartDate, t1.EndDate }).ToList(),
                                                                                        IncludePreviousStatements = t.IsIncludePreviousStatement,
                                                                                        ChainId = t.ScheduleChainId
                                                                                    }
                                                                                    ).FirstOrDefaultAsync();

                var currentDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(billingSchedule.TimeZoneName);
                var offset = DateTimeOffset.Now.GetOffset(billingSchedule.TimeZoneName);
                var bsStartDate = billingSchedule.StartDate.AttachOffset(offset);
                if (billingSchedule.LastStatement != null)
                {
                    if (!billingSchedule.IncludePreviousStatements)
                    {
                        DateTimeOffset startDate = billingSchedule.LastStatement.EndDate.Date;
                        startDate = startDate.AttachOffset(offset);
                        bsStartDate = startDate.AddDays(1);
                    }
                }
                else
                {
                    if (!billingSchedule.IncludePreviousStatements)
                    {
                        var statementEndDate = GetExistingBillingStatementEndDate(billingSchedule.ChainId, billingSchedule.Id);
                        if (statementEndDate.HasValue)
                        {
                            DateTimeOffset startDate = statementEndDate.Value.Date;
                            startDate = startDate.AttachOffset(offset);
                            bsStartDate = startDate.AddDays(1);
                        }
                    }
                }

                Dictionary<DateTimeOffset, DateTimeOffset> statementCycle = GetStatementCycle(bsStartDate, currentDateTime, billingSchedule.FrequencyTypeId);
                var newStatements = statementCycle.Where(t => !billingSchedule.BillingCycles.Any(t1 => t1.StartDate == t.Key && t1.EndDate == t.Value)).ToList();

                foreach (var statement in newStatements)
                {
                    BillingStatementRequestViewModel billingStatement = await GetStatementRequestViewModel(billingScheduleId, statement.Key, statement.Value, currentDateTime);
                    if (billingStatement.BillingScheduleId != null && billingStatement.BillingScheduleId != 0)
                    {
                        var statementId = await CreateBillingStatement(billingStatement);
                        if (statementId != 0)
                        {
                            NotificationDomain notificationDomain = new NotificationDomain(this);
                            await notificationDomain.AddNotificationEventAsync(EventType.BillingStatementGenerated, statementId, billingStatement.CreatedBy);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BillingScheduleDomain", "ProcessBillingSchedulesAsync", ex.Message, ex);
            }
        }

        private DateTimeOffset? GetExistingBillingStatementEndDate(string chainId, int billingScheduleId)
        {
            var previousSchedule = Context.DataContext.BillingSchedules.Where(t => t.Id != billingScheduleId && t.ScheduleChainId == chainId)
                                                                                .SelectMany(t => t.BillingStatements.Where(t1 => t1.IsGenerated))
                                                                                .OrderByDescending(t => t.Id)
                                                                                .Select(t1 => new { t1.StartDate, t1.EndDate })
                                                                                .FirstOrDefault();
            if (previousSchedule != null)
                return previousSchedule.EndDate;

            return null;
        }

        private async Task<BillingStatement> GetActiveBillingStatement(BillingStatement statement)
        {
            BillingStatement response = statement;
            if (!statement.IsActive)
            {
                response = await Context.DataContext.BillingStatements.SingleOrDefaultAsync(t => t.StatementChainId.Equals(statement.StatementChainId) && t.IsGenerated && !t.ParentId.HasValue && t.IsActive);
                if (response == null)
                    response = statement;
            }
            return response;
        }

        public async Task ProcessBillingSchedulesForEditedInvoiceAsync(int billingScheduleId)
        {
            NotificationDomain notificationDomain = new NotificationDomain(this);
            try
            {               
                var billingSchedules = Context.DataContext.BillingStatements.Where(x => !x.IsGenerated && x.ParentId.HasValue)
                                        .AsEnumerable()
                                        .Select(x =>
                                      new
                                      {
                                          allActiveInvoiceNumbers = Task.Run(() => GetActiveBillingStatement(x.ParentBillingStatement)).Result
                                                                .BillingStatementXInvoices.Where(t => t.IsActive)
                                                                .Select(i => i.Invoice.InvoiceHeader.InvoiceNumberId),
                                          BillingStatement = x,
                                          x.BillingSchedule.UpdateFrequencyType,
                                          x.BillingSchedule.UpdateFrequencyValue,
                                          x.CreatedDate,
                                          x.CreatedBy,
                                          x.Id,
                                          x.BillingScheduleId,
                                          x.BillingSchedule.TimeZoneName,
                                          x.ParentBillingStatement,
                                          x.BillingStatementXInvoices
                                      })
                                      .ToList();

                foreach (var item in billingSchedules)
                {
                    var startTime = item.CreatedDate;
                    var currentDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(item.TimeZoneName);
                    var duration = item.UpdateFrequencyValue;
                    if (item.UpdateFrequencyType == (int)UpdateFrequencyTypes.Day)
                    {
                        duration = duration * 24; //in hours
                    }
                    duration = duration * 60; // in minutes
                    if ((currentDateTime - startTime).TotalMinutes >= duration)
                    {
                        item.BillingStatement.TotalStatementValue = 0;
                        item.BillingStatement.TotalQuantityDropped = 0;
                        var invoices = Context.DataContext.Invoices
                                        .Where(t => item.allActiveInvoiceNumbers.Contains(t.InvoiceHeader.InvoiceNumberId) && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                        .Select(t => new
                                        {
                                            TotalAmount = t.BasicAmount + (t.TotalFeeAmount ?? 0) + t.TotalTaxAmount - t.TotalDiscountAmount,
                                            t.DroppedGallons,
                                            t.Id
                                        }).ToList();

                        if (invoices.Any())
                        {
                            item.BillingStatement.BillingStatementXInvoices.ToList().ForEach(inv => inv.IsActive = false);
                            invoices.ForEach(inv => item.BillingStatement.BillingStatementXInvoices.Add(new BillingStatementXInvoice { InvoiceId = inv.Id, IsActive = true }));

                            item.BillingStatement.TotalStatementValue = invoices.Sum(t => t.TotalAmount);
                            item.BillingStatement.TotalQuantityDropped = invoices.Sum(t => t.DroppedGallons);

                            await Context.CommitAsync();
                            await notificationDomain.AddNotificationEventAsync(EventType.BillingStatementUpdated, item.Id, item.CreatedBy);
                        }
                        item.BillingStatement.IsGenerated = true;
                        Task.Run(() => GetActiveBillingStatement(item.BillingStatement.ParentBillingStatement)).Result.IsActive = false;
                    }
                }

                await Context.CommitAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BillingScheduleDomain", "ProcessBillingSchedulesForEditedInvoiceAsync", ex.Message, ex);
            }
        }

        public StatusViewModel UpdateBillingSchedule(BillingScheduleViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                var newEntity = viewModel.ToEntity();
                if (ValidateBillingSchedule(viewModel, response))
                {
                    if (newEntity != null)
                    {
                        using (var transaction = Context.DataContext.Database.BeginTransaction())
                        {
                            try
                            {
                                var oldEntity = Context.DataContext.BillingSchedules.SingleOrDefault(t => t.Id == viewModel.Id);
                                oldEntity.IsActive = false;
                                Context.DataContext.Entry(oldEntity).State = EntityState.Modified;
                                newEntity.VersionNumber = oldEntity.VersionNumber + 1;
                                newEntity.ScheduleChainId = oldEntity.ScheduleChainId;

                                Context.DataContext.BillingSchedules.Add(newEntity);
                                Context.DataContext.SaveChanges();

                                if (newEntity.BillingStatementId.Equals(ApplicationConstants.BillingStatementPrefix))
                                {
                                    newEntity.BillingStatementId = ApplicationConstants.BillingStatementPrefix + newEntity.Id.ToString().PadLeft(7, '0');
                                }
                                if (newEntity.IsIncludePreviousStatement)
                                {
                                    //make all old statements void
                                    var oldScheduleStatements = Context.DataContext.BillingSchedules.Where(t => t.ScheduleChainId == oldEntity.ScheduleChainId).SelectMany(t => t.BillingStatements).ToList();
                                    oldScheduleStatements.ForEach(t => t.IsActive = false);
                                }
                                Context.Commit();
                                transaction.Commit();

                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.successMessageBillingScheduleUpdated;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageBillingScheduleUpdateFailed;
                                LogManager.Logger.WriteException("BillingScheduleDomain", "UpdateBillingSchedule", ex.Message, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageBillingScheduleCreateFailed;
                LogManager.Logger.WriteException("BillingScheduleDomain", "UpdateBillingSchedule", ex.Message, ex);
            }
            return response;
        }
    }
}
