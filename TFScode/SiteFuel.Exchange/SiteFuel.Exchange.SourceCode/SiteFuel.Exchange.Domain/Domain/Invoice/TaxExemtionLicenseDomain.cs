using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SiteFuel.Exchange.DataAccess.Entities;
using System.Text;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain
{
    public class TaxExemtionLicenseDomain : BaseDomain
    {
        public TaxExemtionLicenseDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public TaxExemtionLicenseDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<TaxExemptionViewModel> GetTaxExemptionInfo(int companyId, int licenseId)
        {
            TaxExemptionViewModel response = new TaxExemptionViewModel();
            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId);
                if (licenseId > 0)
                {
                    var license = await Context.DataContext.TaxExemptLicenses.SingleOrDefaultAsync(t => t.Id == licenseId);
                    response = license.ToViewModel();
                }
                else
                {
                    var companyLicense = company.TaxExemptLicenses.FirstOrDefault(t => t.IsActive);
                    if (companyLicense != null)
                    {
                        response.IDCode = companyLicense.IDCode;
                    }
                    response.EffectiveDate = DateTime.Today;
                }
                if (company.CompanyAddresses.Any(t => t.IsDefault))
                {
                    response.CompanyAddress = company.CompanyAddresses.SingleOrDefault(t => t.IsDefault).ToViewModel();
                }
                response.CompanyEffectiveDate = company.CreatedDate;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TaxExemtionLicenseDomain", "GetTaxExemptionInfo", ex.Message, ex);
            }
            return response;
        }

        public async Task<TaxExemptionViewModel> SaveTaxExemptionLicense(TaxExemptionViewModel viewModel)
        {
            var license = new TaxExemptLicens();

            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == viewModel.CompanyId);
                if (viewModel.IsSameCompanyAddress)
                {
                    var companyDefaultAddress = company.CompanyAddresses.SingleOrDefault(t => t.IsDefault);
                    if (companyDefaultAddress != null)
                    {
                        viewModel.Address = license.Address = companyDefaultAddress.Address;
                        viewModel.City = license.City = companyDefaultAddress.City;
                        viewModel.ZipCode = license.PostalCode = companyDefaultAddress.ZipCode;
                        viewModel.Country.Id = license.CountryId = companyDefaultAddress.CountryId;
                        viewModel.State.Id = license.StateId = companyDefaultAddress.StateId;
                        viewModel.State.Code = companyDefaultAddress.MstState.Code;

                        viewModel.CompanyAddress = companyDefaultAddress.ToViewModel();
                    }
                }
                else
                {
                    viewModel.State.Code = Context.DataContext.MstStates.First(t => t.Id == viewModel.State.Id).Code;
                    var countryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.Country.Id).Code;
                    var point = GoogleApiDomain.GetGeocode($"{viewModel.Address} {viewModel.City} {viewModel.State.Code} {countryCode} {viewModel.ZipCode}");
                    if (point == null)
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = Resource.errMessageJobCreateFailedInvalidAddress;
                        return viewModel;
                    }
                    license.Address = viewModel.Address;
                    license.City = viewModel.City;
                    license.PostalCode = viewModel.ZipCode;
                    license.CountryId = viewModel.Country.Id;
                }
                viewModel.IDType = Resource.valTaxLicenseDefaultIDCode;
                license = viewModel.ToEntity();

                var entityCustomId = string.Empty;
                license.EntityCustomId = viewModel.EntityCustomId = entityCustomId;

                if (company.TaxExemptLicenses.Any())
                {
                    entityCustomId = company.TaxExemptLicenses.FirstOrDefault().EntityCustomId;
                }
                else
                {
                    entityCustomId = viewModel.CompanyId.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString()
                                               + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() +
                                               DateTime.Now.Millisecond.ToString();
                }

                var accountCustomId = viewModel.CompanyId.ToString() + viewModel.UserId.ToString() +
                                             DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                                             DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() +
                                             DateTime.Now.Millisecond.ToString();
                license.EntityCustomId = viewModel.EntityCustomId = entityCustomId;
                license.AccountCustomId = viewModel.AccountCustomId = accountCustomId;

                if (viewModel.IsDefault)
                {
                    var openJobs = company.Jobs.Where(t => t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open).ToList();
                    openJobs.ForEach(t => t.TaxExemptLicenses.Add(license));
                }
            }
            catch (Exception ex)
            {
                viewModel.StatusMessage = Resource.errMessageLicenseSaveFailed;
                LogManager.Logger.WriteException("TaxExemptionDomain", "SaveTaxExemptionLicense", ex.Message, ex);
                return viewModel;
            }

            try
            {
                var businessSubType = await Context.DataContext.MstBusinessSubTypes.SingleOrDefaultAsync(t => t.Id == viewModel.BusinessSubType);
                if (businessSubType != null)
                {
                    viewModel.Jurisdiction = businessSubType.Jurisdiction;
                    viewModel.BusinessSubTypeVal = businessSubType.Code;
                    license.Jurisdiction = businessSubType.Jurisdiction;
                }
                List<TaxExemptionViewModel> lstTaxLicense = new List<TaxExemptionViewModel>();
                lstTaxLicense.Add(viewModel);
                var avaDomain = AvalaraDomain.ImportBusinessLicense(lstTaxLicense);
                if (avaDomain.NumberAccountsInserted <= 0)
                {
                    StringBuilder sbError = new StringBuilder();
                    if (avaDomain.BusinessEntityResults != null && avaDomain.BusinessEntityResults.Any())
                    {
                        foreach (var item in avaDomain.BusinessEntityResults)
                        {
                            if (item.BusinessEntityErrors != null && item.BusinessEntityErrors.Any())
                            {
                                foreach (var err in item.BusinessEntityErrors)
                                {
                                    sbError.Append("Error : " + err.ErrorCode + " - " + err.ErrorMessage + Environment.NewLine);
                                }
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(viewModel.LicenseNumber))
                                    sbError.Append(item.Message);
                                else
                                    sbError.Append(Resource.errMessageLicensesAlreadyExists + " - " + item.Message);
                            }
                        }
                        viewModel.StatusMessage = sbError.ToString();
                    }
                    LogManager.Logger.WriteInfo("TaxExemptionDomain", "SaveTaxExemptionLicense", "Could not save to Avalara Business Entities : " + viewModel.StatusMessage);
                    return viewModel;
                }
            }
            catch (Exception ex)
            {
                viewModel.StatusMessage = Resource.errMessageLicenseSaveFailed;
                LogManager.Logger.WriteException("TaxExemptionDomain", "SaveTaxExemptionLicense", ex.Message, ex);
                return viewModel;
            }

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    Context.DataContext.TaxExemptLicenses.Add(license);
                    Context.DataContext.TaxExemptLicenses.Where(t => t.CompanyId == viewModel.CompanyId).ToList()
                        .ForEach(t => { t.IDCode = viewModel.IDCode; t.LegalName = viewModel.LegalName; t.TradeName = viewModel.TradeName; });
                    await Context.CommitAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    viewModel.StatusMessage = Resource.errMessageLicenseSaveFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("TaxExemptionDomain", "SaveTaxExemptionLicense", ex.Message, ex);
                    return viewModel;
                }
            }

            viewModel.StatusCode = Status.Success;
            viewModel.StatusMessage = Resource.errMessageLicenseSaveSuccess;
            return viewModel;
        }

        public async Task<TaxExemptionViewModel> UpdateTaxExemptionLicense(TaxExemptionViewModel viewModel)
        {
            viewModel.IsUpdateRequest = true;
            var license = await Context.DataContext.TaxExemptLicenses.SingleOrDefaultAsync(t => t.Id == viewModel.Id);

            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == viewModel.CompanyId);
                if (viewModel.IsSameCompanyAddress)
                {
                    var companyDefaultAddress = company.CompanyAddresses.SingleOrDefault(t => t.IsDefault);
                    if (companyDefaultAddress != null)
                    {
                        license.Address = companyDefaultAddress.Address;
                        license.City = companyDefaultAddress.City;
                        license.PostalCode = companyDefaultAddress.ZipCode;
                        license.CountryId = viewModel.Country.Id;
                        viewModel.State.Code = companyDefaultAddress.MstState.Code;
                        viewModel.Address = companyDefaultAddress.Address;
                        viewModel.ZipCode = companyDefaultAddress.ZipCode;
                        viewModel.City = companyDefaultAddress.City;
                        viewModel.State.Id = companyDefaultAddress.MstState.Id;

                        viewModel.CompanyAddress = companyDefaultAddress.ToViewModel();
                    }
                }
                else
                {
                    viewModel.State.Code = Context.DataContext.MstStates.First(t => t.Id == viewModel.State.Id).Code;
                    var countryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.Country.Id).Code;
                    var point = GoogleApiDomain.GetGeocode($"{viewModel.Address} {viewModel.City} {viewModel.State.Code} {countryCode} {viewModel.ZipCode}");
                    if (point == null)
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = Resource.errMessageJobCreateFailedInvalidAddress;
                        return viewModel;
                    }
                    license.Address = viewModel.Address;
                    license.City = viewModel.City;
                    license.PostalCode = viewModel.ZipCode;
                    license.CountryId = viewModel.Country.Id;
                }
                viewModel.AccountCustomId = license.AccountCustomId;
                viewModel.EntityCustomId = license.EntityCustomId;
                viewModel.IDType = Resource.valTaxLicenseDefaultIDCode;
                license = viewModel.ToEntity(license);
                if (viewModel.IsDefault)
                {
                    var openJobs = company.Jobs
                                   .Where
                                   (
                                        t => t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open
                                        && !t.TaxExemptLicenses.Contains(license)
                                   ).ToList();
                    openJobs.ForEach(t => t.TaxExemptLicenses.Add(license));
                }
            }
            catch (Exception ex)
            {
                viewModel.StatusMessage = Resource.errMessageLicenseSaveFailed;
                LogManager.Logger.WriteException("TaxExemptionDomain", "UpdateTaxExemptionLicense", ex.Message, ex);
                return viewModel;
            }

            try
            {
                var businessSubType = await Context.DataContext.MstBusinessSubTypes.SingleOrDefaultAsync(t => t.Id == viewModel.BusinessSubType);
                if (businessSubType != null)
                {
                    viewModel.Jurisdiction = businessSubType.Jurisdiction;
                    viewModel.BusinessSubTypeVal = businessSubType.Code;
                    license.Jurisdiction = businessSubType.Jurisdiction;
                }
                List<TaxExemptionViewModel> lstTaxLicense = new List<TaxExemptionViewModel>();
                lstTaxLicense.Add(viewModel);
                var avaDomain = AvalaraDomain.ImportBusinessLicense(lstTaxLicense);
                if (avaDomain.NumberAccountsFailed > 0 || avaDomain.NumberEntitiesFailed > 0)
                {
                    StringBuilder sbError = new StringBuilder();
                    if (avaDomain.BusinessEntityResults != null && avaDomain.BusinessEntityResults.Any())
                    {
                        foreach (var item in avaDomain.BusinessEntityResults)
                        {
                            if (item.BusinessEntityErrors != null && item.BusinessEntityErrors.Any())
                            {
                                foreach (var err in item.BusinessEntityErrors)
                                {
                                    sbError.Append("Error : " + err.ErrorCode + " - " + err.ErrorMessage + Environment.NewLine);
                                }
                            }
                            else
                            {
                                sbError.Append(Resource.errMessageLicensesAlreadyExists + " - " + item.Message);
                            }
                        }
                        viewModel.StatusMessage = sbError.ToString();
                    }
                    LogManager.Logger.WriteInfo("TaxExemptionDomain", "UpdateTaxExemptionLicense", "Could not save to Avalara Business Entities : " + viewModel.StatusMessage);
                    return viewModel;
                }
            }
            catch (Exception ex)
            {
                viewModel.StatusMessage = Resource.errMessageLicenseSaveFailed;
                LogManager.Logger.WriteException("TaxExemptionDomain", "UpdateTaxExemptionLicense", ex.Message, ex);
                return viewModel;
            }

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    Context.DataContext.Entry(license).State = EntityState.Modified;
                    Context.DataContext.TaxExemptLicenses.Where(t => t.CompanyId == viewModel.CompanyId).ToList()
                        .ForEach(t => { t.IDCode = viewModel.IDCode; t.LegalName = viewModel.LegalName; t.TradeName = viewModel.TradeName; });
                    await Context.CommitAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    viewModel.StatusMessage = Resource.errMessageLicenseSaveFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("TaxExemptionDomain", "UpdateTaxExemptionLicense", ex.Message, ex);
                    return viewModel;
                }
            }

            viewModel.StatusCode = Status.Success;
            viewModel.StatusMessage = Resource.errMessageLicenseSaveSuccess;
            return viewModel;
        }

        public async Task<StatusViewModel> DeleteLicensesAsync(int userId, List<int> licenses)
        {
            StatusViewModel response = new StatusViewModel();
            List<TaxExemptionViewModel> viewModelList = new List<TaxExemptionViewModel>();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                    if (user != null && licenses != null)
                    {
                        foreach (int licenseId in licenses)
                        {
                            var license = await Context.DataContext.TaxExemptLicenses.SingleOrDefaultAsync(t => t.Id == licenseId);
                            if (license != null)
                            {
                                var existingJobs = license.Jobs.ToList();
                                existingJobs.ForEach(t => license.Jobs.Remove(t));

                                var existingOrders = license.Orders.ToList();
                                existingOrders.ForEach(t => license.Orders.Remove(t));

                                license.IsActive = false;
                                license.UpdatedBy = userId;
                                license.UpdatedDate = DateTimeOffset.Now;
                                Context.DataContext.Entry(license).State = EntityState.Modified;
                                await Context.CommitAsync();
                                viewModelList.Add(license.ToViewModel());
                            }
                        }

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageLicenseDeleteSuccess;

                        transaction.Commit();

                        //List<TaxExemptionViewModel> lstTaxLicense = new List<TaxExemptionViewModel>();

                        //foreach (TaxExemptionViewModel viewModel in viewModelList)
                        //{
                        //    viewModel.IsUpdateRequest = true;
                        //    viewModel.ObsoleteDate = DateTimeOffset.Now.AddDays(ApplicationConstants.PreviousDate);
                        //    lstTaxLicense.Add(viewModel);
                        //}

                        //var avaDomain = AvalaraDomain.ImportBusinessLicense(lstTaxLicense);
                        //if (avaDomain.NumberAccountsFailed > 0 || avaDomain.NumberEntitiesFailed > 0)
                        //{
                        //    StringBuilder sbError = new StringBuilder();
                        //    if (avaDomain.BusinessEntityResults != null && avaDomain.BusinessEntityResults.Any())
                        //    {
                        //        foreach (var item in avaDomain.BusinessEntityResults)
                        //        {
                        //            if (item.BusinessEntityErrors != null && item.BusinessEntityErrors.Any())
                        //            {
                        //                foreach (var err in item.BusinessEntityErrors)
                        //                {
                        //                    sbError.Append("Error : " + err.ErrorCode + " - " + err.ErrorMessage + Environment.NewLine);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                sbError.Append(item.Message);
                        //            }
                        //        }
                        //        response.StatusMessage = sbError.ToString();
                        //    }
                        //    LogManager.Logger.WriteInfo("TaxExemptionDomain", "DeleteLicensesAsync", "Could not save to Avalara Business Entities : " + response.StatusMessage);
                        //    return response;
                        //}
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("TaxExemptionDomain", "DeleteLicensesAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<CompanyTaxesViewModel> InitializeCompanyTax(int companyId, CompanyType companyTypeId, CompanyType companySubTypeId)
        {
            var model = new CompanyTaxesViewModel();
            try
            {
                await GetDirectTaxDetails(companyId, model, companyTypeId, companySubTypeId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TaxExemtionLicenseDomain", "InitializeCompanyTax", ex.Message, ex);
            }
            return model;
        }

        public async Task<StatusViewModel> SaveDirectTaxAsync(CompanyTaxesViewModel directTaxModel, int userId, int companyId)
        {
            StatusViewModel response = new StatusViewModel();
            List<DirectTax> directTaxList = new List<DirectTax>();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var existingTaxes = await Context.DataContext.DirectTaxes.Where(t => t.CompanyId == companyId && t.IsActive).ToListAsync();
                    if (existingTaxes != null && existingTaxes.Any())
                    {
                        foreach (var obj in existingTaxes)
                        {
                            obj.IsActive = false;
                            obj.UpdatedBy = userId;
                            obj.UpdatedDate = DateTimeOffset.Now;
                            Context.DataContext.Entry(obj).State = EntityState.Modified;
                        }
                    }

                    var isStateSelected = false;
                    if (directTaxModel.IsDirectTax && directTaxModel.DirectTaxes != null && directTaxModel.DirectTaxes.Any())
                    {
                        foreach (var directTax in directTaxModel.DirectTaxes)
                        {
                            foreach (var stateId in directTax.StateIds)
                            {
                                DirectTax obj = new DirectTax();

                                obj.CompanyId = companyId;
                                obj.IsDirectTax = directTaxModel.IsDirectTax;
                                obj.StateId = stateId;
                                obj.CountryId = directTax.CountryId;

                                obj.IsActive = true;
                                obj.AddedBy = userId;
                                obj.AddedDate = DateTimeOffset.Now;
                                obj.UpdatedBy = userId;
                                obj.UpdatedDate = DateTimeOffset.Now;

                                directTaxList.Add(obj);

                                isStateSelected = true;
                            }
                        }
                    }

                    if (!isStateSelected && !directTaxModel.IsEdit)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageSelectStates;
                        return response;
                    }

                    Context.DataContext.DirectTaxes.AddRange(directTaxList);
                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageDirectTaxSave;

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.failedMessageDirectTaxSave;
                    LogManager.Logger.WriteException("TaxExemtionLicenseDomain", "SaveDirectTaxAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<CompanyTaxesViewModel> GetDirectTaxDetails(int companyId, CompanyTaxesViewModel model, CompanyType companyTypeId, CompanyType companySubTypeId)
        {
            if (model.DirectTaxes == null)
                model.DirectTaxes = new List<DirectTaxesViewModel>();
            try
            {
                var masterDomain = new MasterDomain(this);
                var stateList = masterDomain.GetStatesOfAllCountries();

                var companyDomain = new CompanyDomain(this);
                var servingCountries = companyDomain.ServingCountry(companyId, companyTypeId, companySubTypeId);
                foreach (var countryId in servingCountries)
                {
                    DirectTaxesViewModel obj = new DirectTaxesViewModel
                    {
                        CountryId = countryId,
                        StateList = stateList.Where(t => t.CountryId == countryId).ToList()
                    };
                    model.DirectTaxes.Add(obj);
                }

                var directTaxList = await Context.DataContext.DirectTaxes.Where(t => t.CompanyId == companyId && t.IsActive).ToListAsync();
                if (directTaxList != null && directTaxList.Any())
                {
                    model.IsDirectTax = directTaxList[0].IsDirectTax;
                    model.IsEdit = true;
                    var countryIds = directTaxList.Select(t => t.CountryId).Distinct().ToList();
                    foreach (var id in countryIds)
                    {
                        var statesByCountry = directTaxList.Where(t => t.CountryId == id).AsEnumerable().Select(t1 => t1.StateId).ToList();
                        model.DirectTaxes.Where(t => t.CountryId == id).ToList().ForEach(t1 => t1.StateIds.AddRange(statesByCountry));
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TaxExemtionLicenseDomain", "GetDirectTaxDetails", ex.Message, ex);
            }
            return model;
        }

        public async Task<bool> IsDirectTaxEnabledForState(int buyerCompanyId, int jobLocationStateId)
        {
            bool hasDirectTax = false;
            try
            {
                hasDirectTax = await Context.DataContext.DirectTaxes.AnyAsync(t => t.StateId == jobLocationStateId &&
                                                                              t.CompanyId == buyerCompanyId &&
                                                                              t.IsDirectTax && t.IsActive);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TaxExemtionLicenseDomain", "IsDirectTaxEnabledForState", ex.Message, ex);
            }

            return hasDirectTax;
        }
    }
}
