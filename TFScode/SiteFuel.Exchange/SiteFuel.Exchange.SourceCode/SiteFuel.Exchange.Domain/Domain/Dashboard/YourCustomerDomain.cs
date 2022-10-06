using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class YourCustomerDomain : BaseDomain
    {
        public YourCustomerDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public YourCustomerDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<BillingAddressViewModel>> GetBillingAddressSummary(int companyId)
        {
            var response = new List<BillingAddressViewModel>();
            try
            {
                var billingAddresses = await Context.DataContext.BillingAddresses.Where(t => t.CompanyId == companyId && t.IsActive).OrderByDescending(t => t.IsDefault).ToListAsync();
                foreach (var item in billingAddresses)
                {
                    var data = new BillingAddressViewModel();

                    data.Id = item.Id;
                    data.Name = item.Name;
                    data.Address = item.Address;
                    data.Address2 = item.AddressLine2;
                    data.Address3 = item.AddressLine3;
                    data.City = item.City;
                    data.State.Name = item.StateName;
                    data.County = item.County;
                    data.ZipCode = item.ZipCode;
                    data.Country.Name = item.CountryName;
                    data.IsDefault = item.IsDefault;
                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("YourCustomerDomain", "GetBillingAddressSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<BillingAddressViewModel> GetBillingAddressDetails(int billingAddressId)
        {
            var viewModel = new BillingAddressViewModel();
            try
            {
                var billingAddress = await Context.DataContext.BillingAddresses.FirstOrDefaultAsync(t => t.Id == billingAddressId);
                billingAddress.ToViewModel(viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("YourCustomerDomain", "GetBillingAddressDetails", ex.Message, ex);
            }

            return viewModel;
        }

        public async Task<StatusViewModel> CreateBillingAddress(BillingAddressViewModel model, int userId)
        {
            var response = new StatusViewModel(Status.Success);

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var billingAddress = model.ToEntity();

                    var isBillingAddressExits = await Context.DataContext.BillingAddresses.Where(t => t.CompanyId == model.CompanyId && t.IsActive).ToListAsync();
                    if (isBillingAddressExits == null || isBillingAddressExits.Count == 0)
                    {
                        billingAddress.IsDefault = true; //When user adds first billing address set as default.
                    }

                    billingAddress.UpdatedBy = userId;
                    billingAddress.UpdatedDate = DateTimeOffset.Now;
                    Context.DataContext.BillingAddresses.Add(billingAddress);
                    await Context.CommitAsync();

                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = "Created successfully.";
                }

                catch (Exception ex)
                {
                    response = new StatusViewModel(Status.Failed);
                    transaction.Rollback();
                    LogManager.Logger.WriteException("YourCustomerDomain", "CreateBillingAddress", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateBillingAddress(BillingAddressViewModel model, int userId)
        {
            var response = new StatusViewModel(Status.Success);

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var billingAddress = await Context.DataContext.BillingAddresses.FirstOrDefaultAsync(t => t.Id == model.Id);
                    if (billingAddress != null && model != null)
                    {
                        var item =  model.ToEntity(billingAddress);
                        item.UpdatedBy = userId;
                        item.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(item).State = EntityState.Modified;
                        await Context.CommitAsync();

                        transaction.Commit();
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = "Updated successfully.";
                }

                catch (Exception ex)
                {
                    response = new StatusViewModel(Status.Failed);
                    transaction.Rollback();
                    LogManager.Logger.WriteException("YourCustomerDomain", "UpdateBillingAddress", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteBillingAddress(int billingAddressId, int userId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.BillingAddressId == billingAddressId);
                if (job == null)
                {
                    var billingAddress = await Context.DataContext.BillingAddresses.FirstOrDefaultAsync(t => t.Id == billingAddressId);
                    if (billingAddress != null)
                    {
                        billingAddress.IsActive = false;
                        billingAddress.UpdatedDate = DateTimeOffset.Now;
                        billingAddress.UpdatedBy = userId;
                        Context.DataContext.Entry(billingAddress).State = EntityState.Modified;
                        await Context.CommitAsync();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = "Deleted successfully";
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = "Address is in use,can't be deleted";
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("YourCustomerDomain", "DeleteBillingAddress", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SetDefaultBillingAddress(int billingAddressId, int companyId, int userId)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var billingAddresses = await Context.DataContext.BillingAddresses.Where(t => t.CompanyId == companyId && t.IsActive && t.IsDefault).ToListAsync();
                    foreach (var item in billingAddresses)
                    {
                        item.IsDefault = false;
                        Context.DataContext.Entry(item).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }

                    var billingAddress = await Context.DataContext.BillingAddresses.FirstOrDefaultAsync(t => t.Id == billingAddressId);
                    if (billingAddress != null)
                    {
                        billingAddress.IsDefault = true;
                        billingAddress.UpdatedDate = DateTimeOffset.Now;
                        billingAddress.UpdatedBy = userId;
                        Context.DataContext.Entry(billingAddress).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }

                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = "Address set to default successfully";
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageUpdateFailed;
                    LogManager.Logger.WriteException("YourCustomerDomain", "SetDefaultBillingAddress", ex.Message, ex);
                }
            }
            return response;
        }
    }
}

