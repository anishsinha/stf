using SiteFuel.Models.ApiModels;
using SiteFuel.Models.Common.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DAL.Update
{
    public class ExceptionUpdateLayer : BaseLayer
    {
        public ExceptionUpdateLayer()
        {
        }

        public ExceptionUpdateLayer(BaseLayer baseLayer) : base(baseLayer)
        {
        }

        public async Task<List<int>> UpdateExistingCompanyExceptions(ManageExceptionModel model)
        {
            var response = new List<int>();

            var existingExceptions = await Context.DataContext.CompanyExceptions.Where(t => !t.IsDeleted
                                                    && t.OwnerCompanyId == model.OwnerCompanyId).ToListAsync();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var exception in existingExceptions)
                    {
                        var modifiedException = model.Exceptions.FirstOrDefault(t => t.TypeId == exception.ExceptionTypeId && t.ApproverId.HasValue);
                        if (modifiedException != null)
                        {
                            exception.OwnerCompanyId = model.OwnerCompanyId;
                            exception.ExceptionTypeId = modifiedException.TypeId;
                            exception.ExceptionApproverId = modifiedException.ApproverId.Value;
                            exception.IsAutoApprovalEnabled = modifiedException.AutoApprovalDays.HasValue;
                            exception.AutoApprovalDays = modifiedException.AutoApprovalDays;
                            exception.DelayInvoiceCreationTime = modifiedException.DelayInvoiceCreationTime;
                            exception.Threshold = modifiedException.Threshold;
                            exception.UpdateBy = model.UserId;
                            exception.UpdatedOn = DateTimeOffset.Now;
                            exception.IsActive = modifiedException.IsActive;                            
                            response.Add(exception.ExceptionTypeId);
                        }
                    }
                    await UpdateCustomerExceptionApprovers(model);

                    await Context.DataContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return response;
        }

        public async Task<List<int>> UpdateExistingCustomerExceptions(ManageCustomerExceptionModel model)
        {
            var response = new List<int>();

            var existingExceptions = await (from cp in Context.DataContext.CompanyExceptions
                                            join cu in Context.DataContext.CustomerExceptions
                                            on cp.ExceptionTypeId equals cu.ExceptionTypeId into grp
                                            from g in grp.DefaultIfEmpty()
                                            where cp.OwnerCompanyId == model.OwnerCompanyId
                                            && g.EnabledForCompanyId == model.EnabledForCompanyId
                                            && cp.IsDeleted == false && g.IsDeleted == false
                                            select new
                                            {
                                                CustomerException = g,
                                                ApproverId = cp.ExceptionApproverId
                                            }).ToListAsync();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var existingException in existingExceptions)
                    {
                        var exception = existingException.CustomerException;
                        var approverCompanyId = existingException.ApproverId == (int)ExceptionApprover.Self
                                                ? model.OwnerCompanyId : model.EnabledForCompanyId;

                        var modifiedException = model.Exceptions.FirstOrDefault(t => t.ExceptionTypeId == exception.ExceptionTypeId);
                        if (modifiedException != null)
                        {
                            exception.ExceptionTypeId = modifiedException.ExceptionTypeId;
                            exception.OwnerCompanyId = model.OwnerCompanyId;
                            exception.EnabledForCompanyId = model.EnabledForCompanyId;
                            exception.ApproverCompanyId = approverCompanyId;
                            exception.UpdateBy = model.UserId;
                            exception.UpdatedOn = DateTimeOffset.Now;
                            exception.IsActive = modifiedException.IsActive;
                            response.Add(exception.ExceptionTypeId);
                        }
                    }
                    await Context.DataContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return response;
        }

        public async Task<bool> ApproveException(ExceptionApprovalRequestModel model)
        {
            var response = false;
            var generatedExceptions = await Context.DataContext.GeneratedExceptions.Where(t => model.ExceptionIds.Contains(t.Id)).ToListAsync();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (generatedExceptions != null && generatedExceptions.Any())
                    {
                        foreach (var generatedException in generatedExceptions)
                        {
                            generatedException.ResolutionTypeId = model.ResolutionTypeId;
                            generatedException.StatusId = model.StatusId;
                            if (model.StatusId == (int)ExceptionStatus.AutoApproved)
                            {
                                generatedException.AutoApprovedOn = DateTimeOffset.Now;
                                generatedException.ResolvedOn = DateTime.Now;
                            }
                            else
                            {
                                generatedException.ResolvedOn = DateTimeOffset.Now;
                            }
                            await Context.DataContext.SaveChangesAsync();
                        }
                        transaction.Commit();
                        response = true;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return response;
        }

        private async Task<bool> UpdateCustomerExceptionApprovers(ManageExceptionModel model)
        {
            var response = false;

            var customerExceptions = await Context.DataContext.CustomerExceptions
                                            .Where(t => t.OwnerCompanyId == model.OwnerCompanyId
                                            && (t.ExceptionTypeId == (int)ExceptionType.DeliveredQuantityVariance
                                            || t.ExceptionTypeId == (int)ExceptionType.DuplicateInvoice
                                            || t.ExceptionTypeId == (int)ExceptionType.InvoiceApiException
                                            || t.ExceptionTypeId == (int)ExceptionType.UnmatchedDeliveryLocation))
                                            .Select(t => new
                                            {
                                                t.ExceptionTypeId,
                                                t.EnabledForCompanyId,
                                                t.ApproverCompanyId,
                                                t.IsActive
                                            }).ToListAsync();
            try
            {
                var sqlCommand = "UPDATE CustomerExceptions SET ApproverCompanyId={0} WHERE ExceptionTypeId={1} AND OwnerCompanyId={2} AND IsActive={3}";
                foreach (var item in customerExceptions)
                {
                    var modifiedException = model.Exceptions.FirstOrDefault(t => t.TypeId == item.ExceptionTypeId);
                    if (modifiedException != null)
                    {
                        var approverCompanyId = modifiedException.ApproverId == (int)ExceptionApprover.Self
                                            ? model.OwnerCompanyId : item.EnabledForCompanyId;

                        if (item.ApproverCompanyId != approverCompanyId || item.IsActive != modifiedException.IsActive)
                        {
                            var sqlUpdateCommand = string.Format(sqlCommand, approverCompanyId, item.ExceptionTypeId, model.OwnerCompanyId, modifiedException.IsActive ? 1 : 0);
                            await Context.DataContext.Database.ExecuteSqlCommandAsync(sqlUpdateCommand);
                        }
                    }
                }
                response = true;
            }
            catch
            {
                throw;
            }
            return response;
        }
    }
}
