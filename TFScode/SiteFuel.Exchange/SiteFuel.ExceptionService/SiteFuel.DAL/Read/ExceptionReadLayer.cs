using SiteFuel.DAL.Usp;
using SiteFuel.DataAccess;
using SiteFuel.Models.ApiModels;
using SiteFuel.Models.Common;
using SiteFuel.Models.Common.Enums;
using SiteFuel.Models.CompanyException;
using SiteFuel.Models.InvoiceException;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DAL.Read
{
    public class ExceptionReadLayer : BaseLayer
    {
        public ExceptionReadLayer()
        {
        }
        public ExceptionReadLayer(BaseLayer baseLayer) : base(baseLayer)
        {
        }

        public async Task<bool> IsCompanyExceptionsEnabled(int ownerCompanyId)
        {
            var response = false;
            try
            {
                response = await Context.DataContext.CompanyExceptions.AnyAsync(t =>
                                 t.OwnerCompanyId == ownerCompanyId && t.IsActive && !t.IsDeleted);
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> IsCompanyExceptionsEnabledByType(int ownerCompanyId, int exceptionType)
        {
            var response = false;
            try
            {
                response = await Context.DataContext.CompanyExceptions.AnyAsync(t =>
                                 t.OwnerCompanyId == ownerCompanyId && t.ExceptionTypeId == exceptionType && t.IsActive && !t.IsDeleted);
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> IsCustomerExceptionsEnabled(int ownerCompanyId, int enabledForCompanyId)
        {
            var response = false;
            try
            {
                var result = await GetExceptions(ownerCompanyId, enabledForCompanyId);
                if (result != null)
                {
                    response = result.Any();
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<List<EnabledExceptionModel>> GetEnabledExceptions(int ownerCompanyId, int enabledForCompanyId, List<BrokeredOrdersModel> brokeredOrders)
        {
            var response = new List<EnabledExceptionModel>();
            try
            {
                EnabledExceptionModel dqvException = null;
                var result = await GetExceptions(ownerCompanyId, enabledForCompanyId, brokeredOrders);
                if (result != null)
                {
                    response = result;
                    if (brokeredOrders != null && brokeredOrders.Any())
                    {
                        response = result.Where(t => t.ExceptionTypeId != (int)ExceptionType.DeliveredQuantityVariance).ToList();
                        foreach (var broker in brokeredOrders.OrderByDescending(t => t.SequenceFromEndSupplier))
                        {
                            var filteredDqv = result.FirstOrDefault(t => t.ExceptionTypeId == (int)ExceptionType.DeliveredQuantityVariance && t.OwnerCompanyId == broker.BuyerCompanyId);
                            if (filteredDqv != null)
                            {
                                if (dqvException == null)
                                {
                                    dqvException = filteredDqv;
                                }
                                else
                                {
                                    dqvException.OwnerCompanyId = filteredDqv.OwnerCompanyId;
                                    dqvException.ApproverCompanyId = filteredDqv.ApproverCompanyId;
                                    dqvException.AutoApprovalDays = filteredDqv.AutoApprovalDays;
                                }
                            }
                        }
                    }
                    if (dqvException != null)
                    {
                        response.Add(dqvException);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<List<int>> GetPendingExceptionsById(List<int> pendingExceptionIds, int exceptionTypeId)
        {
            var response = new List<int>();
            try
            {
                response = await Context.DataContext.GeneratedExceptions.Where(t =>
                               t.ExceptionTypeId == exceptionTypeId && pendingExceptionIds.Contains(t.Id) && t.StatusId == (int)ExceptionStatus.Raised)
                               .Select(t => t.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        private async Task<List<EnabledExceptionModel>> GetExceptions(int ownerCompanyId, int enabledForCompanyId, List<BrokeredOrdersModel> brokeredOrders = null)
        {
            UspLayer uspLayer = new UspLayer();
            var response = await uspLayer.GetEnabledExceptions(ownerCompanyId, enabledForCompanyId, brokeredOrders);
            return response;
        }

        public async Task<List<EnabledExceptionModel>> GetEnabledException(int ownerCompanyId, int exceptionTypeId)
        {
            var response = new List<EnabledExceptionModel>();
            try
            {
                var result = GetEnabledExceptionByType(ownerCompanyId, exceptionTypeId);
                response = await result.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        private IQueryable<EnabledExceptionModel> GetEnabledExceptionByType(int ownerCompanyId, int exceptionTypeId)
        {
            var response = from cp in Context.DataContext.CompanyExceptions
                           where cp.OwnerCompanyId == ownerCompanyId
                           && cp.ExceptionTypeId == exceptionTypeId
                           && cp.IsActive == true && cp.IsDeleted == false
                           select new EnabledExceptionModel()
                           {
                               ExceptionTypeId = cp.ExceptionTypeId,
                               OwnerCompanyId = cp.OwnerCompanyId,
                               ApproverCompanyId = cp.OwnerCompanyId,
                               AutoApprovalDays = cp.AutoApprovalDays
                           };
            return response;
        }

        public async Task<List<EnabledExceptionModel>> GetCompaniesForEnabledException(int exceptionTypeId)
        {
            var response = new List<EnabledExceptionModel>();
            var query = from cp in Context.DataContext.CompanyExceptions
                        where cp.ExceptionTypeId == exceptionTypeId
                        && cp.IsActive == true && cp.IsDeleted == false
                        select new EnabledExceptionModel()
                        {
                            ExceptionTypeId = cp.ExceptionTypeId,
                            OwnerCompanyId = cp.OwnerCompanyId,
                            ApproverCompanyId = cp.OwnerCompanyId,
                            AutoApprovalDays = cp.AutoApprovalDays,
                            Threshold = cp.Threshold ?? 0
                        };
            response = await query.ToListAsync();
            return response;
        }

        public async Task<List<SiteFuel.Exchange.Utilities.DropdownDisplayExtendedId>> GetDelayInvoiceCreationTimeByCompany(List<int> companyIds)
        {
            var query = from cp in Context.DataContext.CompanyExceptions
                        where companyIds.Contains(cp.OwnerCompanyId)
                        && cp.IsActive && !cp.IsDeleted
                        select new SiteFuel.Exchange.Utilities.DropdownDisplayExtendedId()
                        {
                            Id = cp.OwnerCompanyId,
                            CodeId = cp.DelayInvoiceCreationTime.HasValue ? cp.DelayInvoiceCreationTime.Value : 0,
                        };
            var response = await query.ToListAsync();
            return response;
        }
    }
}