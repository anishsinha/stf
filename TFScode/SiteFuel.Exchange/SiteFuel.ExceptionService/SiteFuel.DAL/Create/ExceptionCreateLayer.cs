using SiteFuel.DAL.Mappers;
using SiteFuel.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models.ApiModels;
using SiteFuel.Models.Common;
using SiteFuel.Models.InvoiceException;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExceptionType = SiteFuel.Exchange.Utilities.ExceptionType;

namespace SiteFuel.DAL.Create
{
    public class ExceptionCreateLayer : BaseLayer
    {
        public ExceptionCreateLayer()
        {
        }
        public ExceptionCreateLayer(BaseLayer baseLayer) : base(baseLayer)
        {
        }

        public async Task<bool> SaveNewCompanyExceptions(ManageExceptionModel model, List<int> updatedExceptions)
        {
            var response = false;
            var newExceptions = model.Exceptions.Where(t => t.ApproverId.HasValue && !updatedExceptions.Contains(t.TypeId));
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var newException in newExceptions)
                    {
                        var companyException = new CompanyException();
                        companyException.OwnerCompanyId = model.OwnerCompanyId;
                        companyException.ExceptionTypeId = newException.TypeId;
                        companyException.ExceptionApproverId = newException.ApproverId.Value;
                        companyException.IsAutoApprovalEnabled = newException.AutoApprovalDays.HasValue;
                        companyException.AutoApprovalDays = newException.AutoApprovalDays;
                        companyException.DelayInvoiceCreationTime = newException.DelayInvoiceCreationTime;
                        companyException.Threshold = newException.Threshold;
                        companyException.CreatedBy = model.UserId;
                        companyException.CreatedOn = DateTimeOffset.Now;
                        companyException.UpdateBy = model.UserId;
                        companyException.UpdatedOn = DateTimeOffset.Now;
                        companyException.IsActive = newException.IsActive;
                        Context.DataContext.CompanyExceptions.Add(companyException);
                    }
                    await Context.DataContext.SaveChangesAsync();
                    transaction.Commit();
                    response = true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return response;
        }

        public async Task<bool> SaveNewCustomerExceptions(ManageCustomerExceptionModel model, List<int> updatedExceptions)
        {
            var response = false;
            var newExceptions = model.Exceptions.Where(t => !updatedExceptions.Contains(t.ExceptionTypeId));
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (newExceptions.Any())
                    {
                        var exceptionTypes = newExceptions.Select(t => t.ExceptionTypeId);
                        var exceptionApprovers = await Context.DataContext.CompanyExceptions
                                                .Where(t => exceptionTypes.Contains(t.ExceptionTypeId) && t.OwnerCompanyId == model.OwnerCompanyId)
                                                .Select(t => new { t.ExceptionTypeId, t.ExceptionApproverId }).ToListAsync();

                        foreach (var newException in newExceptions)
                        {
                            var approver = exceptionApprovers.First(t => t.ExceptionTypeId == newException.ExceptionTypeId);
                            var approverCompanyId = approver.ExceptionApproverId == (int)Models.Common.Enums.ExceptionApprover.Self
                                                  ? model.OwnerCompanyId : model.EnabledForCompanyId;

                            var customerException = new CustomerException();
                            customerException.ExceptionTypeId = newException.ExceptionTypeId;
                            customerException.OwnerCompanyId = model.OwnerCompanyId;
                            customerException.EnabledForCompanyId = model.EnabledForCompanyId;
                            customerException.ApproverCompanyId = approverCompanyId;
                            customerException.CreatedBy = model.UserId;
                            customerException.CreatedOn = DateTimeOffset.Now;
                            customerException.UpdateBy = model.UserId;
                            customerException.UpdatedOn = DateTimeOffset.Now;
                            customerException.IsActive = newException.IsActive;
                            Context.DataContext.CustomerExceptions.Add(customerException);
                        }
                        await Context.DataContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    response = true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return response;
        }

        public async Task<List<ExceptionRaised>> SaveGeneratedExceptions(List<GeneratedExceptionModel> exceptionModels)
        {
            var response = new List<ExceptionRaised>();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var entities = new List<GeneratedException>();
                    foreach (var item in exceptionModels)
                    {
                        var generatedException = item.ToEntity();
                        Context.DataContext.GeneratedExceptions.Add(generatedException);
                        await Context.DataContext.SaveChangesAsync();
                        response.Add(new ExceptionRaised()
                        {
                            InvoiceId = generatedException.InvoiceId,
                            ExceptionId = generatedException.Id,
                            ExceptionTypeId = generatedException.ExceptionTypeId,
                            RaisedOn = generatedException.GeneratedOn,
                            ApproverCompanyId = generatedException.ApproverCompanyId,
                            StatusId = generatedException.StatusId,
                            OrderId = item.GeneratedExceptionDetail.OrderId
                        });
                    }
                    transaction.Commit();                   
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    response.Clear();
                    throw;
                }
            }
            return response;
        }

        public async Task<bool> UpdateGeneratedExceptionForAutoReject(List<int> generatedExceptionIdList)
        {
            var response = false;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var generatedExceptions = Context.DataContext.GeneratedExceptions.Where(w => generatedExceptionIdList.Contains(w.Id)).ToList();
                    var entities = new List<GeneratedException>();
                    foreach (var item in generatedExceptions)
                    {

                        if (item.ExceptionTypeId == (int)ExceptionType.InvoiceApiException)
                        {
                            item.ResolutionTypeId = (int)ExceptionResolution.DiscardDropTicket;
                        }
                        else
                        {
                            item.ResolutionTypeId = (int)ExceptionResolution.DiscardException;
                        }
                        item.StatusId = (int)ExceptionStatus.AutoDiscard;
                        item.ResolvedOn = DateTimeOffset.Now;
                        Context.DataContext.GeneratedExceptions.AddRange(entities);
                    }
                    
                    await Context.DataContext.SaveChangesAsync();
                    transaction.Commit();
                    response = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return response;
        }
    }
}