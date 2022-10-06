using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.FilldService;
using Stripe;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class SettingsDomain : BaseDomain
    {
        public SettingsDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public SettingsDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<CompanyInformationViewModel> GetCompanyInformationAsync(int userId)
        {
            CompanyInformationViewModel response = new CompanyInformationViewModel();
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                if (user != null)
                {
                    if (user.Company != null)
                    {
                        response.Company = user.Company.ToViewModel();

                        var companyXTheme = user.Company.CompanyXMobileAppThemes.SingleOrDefault();
                        if (companyXTheme != null)
                            response.Company.ThemeId = companyXTheme.ThemeId;
                        else
                            response.Company.ThemeId = Constants.DefaultTheme;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCompanyInformationAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<CompanyInformationViewModel> GetCompanyInformationByIdAsync(int CompanyId)
        {
            CompanyInformationViewModel response = new CompanyInformationViewModel();
            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == CompanyId);
                if (company != null)
                {
                    response.Company = company.ToViewModel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCompanyInformationByIdAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> GetTaxExclusionAsync(int companyId)
        {
            var response = false;
            try
            {
                var noraExclusion = await Context.DataContext.TaxExclusions.Where(t => t.CompanyId == companyId).FirstOrDefaultAsync();
                if (noraExclusion != null)
                {

                    response = noraExclusion.IsActive;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetTaxExclusionAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateTaxExclusionAsync(UserContext userContext, bool isChecked)
        {
            var response = new StatusViewModel();
            try
            {
                var noraExclusion = await Context.DataContext.TaxExclusions.Where(t => t.CompanyId == userContext.CompanyId).FirstOrDefaultAsync();
                if (noraExclusion != null)
                {
                    noraExclusion.UpdatedBy = userContext.Id;
                    noraExclusion.UpdatedDate = DateTimeOffset.Now;
                    noraExclusion.IsActive = isChecked;
                }
                else
                {
                    var exclusion = new TaxExclusion()
                    {
                        ExclusionType = TaxExclusionType.NORA,
                        CompanyId = userContext.CompanyId,
                        CreatedBy = userContext.Id,
                        IsActive = isChecked,
                        UpdatedBy = userContext.Id,
                        CreatedDate = DateTimeOffset.Now
                    };
                    Context.DataContext.TaxExclusions.Add(exclusion);
                }
                await Context.CommitAsync();
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "UpdateTaxExclusionAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveCompanyInformationAsync(CompanyViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == viewModel.Id);
                    if (company != null)
                    {
                        if (company.Image != null)
                        {
                            if (!viewModel.CompanyLogo.IsRemoved)
                            {
                                if (viewModel.CompanyLogo.Id > 0)
                                {
                                    company.Image = viewModel.CompanyLogo.ToEntity();
                                }
                            }
                            else
                            {
                                Context.DataContext.Images.Remove(company.Image);
                            }
                        }
                        else
                        {
                            if (viewModel.CompanyLogo.Id == 0 && !string.IsNullOrWhiteSpace(viewModel.CompanyLogo.FilePath))
                            {
                                company.Image = viewModel.CompanyLogo.ToEntity();
                            }
                        }

                        company = viewModel.ToEntity(company);
                        Context.DataContext.Entry(company).State = EntityState.Modified;

                        if (viewModel.ThemeId > 0)
                        {
                            var companyXTheme = company.CompanyXMobileAppThemes.SingleOrDefault(t => t.AppTypeId == (int)AppType.BuyerApp);
                            if (companyXTheme == null)
                            {
                                companyXTheme = new CompanyXMobileAppTheme();
                                companyXTheme.AppTypeId = (int)AppType.BuyerApp;
                                companyXTheme.CompanyId = company.Id;
                                companyXTheme.ThemeId = viewModel.ThemeId;
                                company.CompanyXMobileAppThemes.Add(companyXTheme);
                            }
                            else
                            {
                                companyXTheme.ThemeId = viewModel.ThemeId;
                                Context.DataContext.Entry(companyXTheme).State = EntityState.Modified;
                            }
                        }

                        await Context.CommitAsync();
                        transaction.Commit();

                        //Send response
                        viewModel.CompanyLogo.Id = company.Image != null ? company.Image.Id : 0;
                        viewModel.CompanyLogo.Data = company.Image?.Data;
                        viewModel.CompanyLogo.FilePath = company.Image?.FilePath;

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageCompanyInformationSaveSuccess;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageCompanyInformationSaveFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "SaveCompanyInformationAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<CompanyAddressViewModel>> GetCompanyAddressListAsync(int companyId)
        {
            List<CompanyAddressViewModel> response = new List<CompanyAddressViewModel>();
            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId);
                if (company != null)
                {
                    response = company.CompanyAddresses.Select(t => new CompanyAddressViewModel(Status.Success)
                    {
                        Id = t.Id,
                        Address = t.Address,
                        City = t.City,
                        State = t.MstState.ToViewModel(),
                        Country = t.MstCountry.ToViewModel(),
                        ZipCode = t.ZipCode,
                        IsDefault = t.IsDefault,
                        CompanyId = t.CompanyId,
                        IsActive = t.IsActive
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCompanyAddressListAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<CompanyAddressViewModel> GetCompanyAddressSettingsAsync(int companyId, int companyAddressId = 0)
        {
            CompanyAddressViewModel response = new CompanyAddressViewModel();
            try
            {
                var company = await Context.DataContext.Companies.Include(t => t.CompanyAddresses).Include(t => t.BillingAddresses)
                                    .SingleOrDefaultAsync(t => t.Id == companyId);
                if (company != null)
                {
                    if (companyAddressId > 0)
                    {
                        var companyAddress = company.CompanyAddresses.SingleOrDefault(t => t.Id == companyAddressId);
                        if (companyAddress != null)
                        {
                            response = companyAddress.ToViewModel();
                            if (company.CompanyTypeId != (int)CompanyType.Buyer)
                            {
                                response.SupplierProductTypes = companyAddress.MstProductTypes.Select(t => t.Id).ToList();
                                response.ServiceOffering = companyAddress.CompanyXServingLocations.ToList().ToViewModel();
                            }
                        }
                    }
                    else
                    {
                        response.SupplierProductTypes = Context.DataContext.MstProductTypes.Select(t => t.Id).ToList();
                        response.CompanyId = company.Id;
                        response.IsDefault = !company.CompanyAddresses.Any(t => t.IsDefault);
                        response.ServiceOffering = Enum.GetValues(typeof(ServiceOfferingType)).Cast<ServiceOfferingType>().Select(c => new CompanyServiceAreaModel { ServiceDeliveryType = c }).ToList();

                    }
                    response.CompanyTypeId = (CompanyType)company.CompanyTypeId;

                    var companyBillingAddress = company.BillingAddresses.OrderByDescending(t => t.UpdatedDate).FirstOrDefault(t => t.CompanyId == companyId && t.IsActive);
                    if (companyBillingAddress != null)
                    {
                        response.BillingAddress = companyBillingAddress.ToViewModel();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCompanyAddressSettingsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<CompanyAddressViewModel> GetCompanyAddressAsync(int companyId)
        {
            CompanyAddressViewModel response = new CompanyAddressViewModel();
            try
            {
                var company = await Context.DataContext.Companies.Include(t => t.CompanyAddresses).Include(t => t.BillingAddresses)
                                    .SingleOrDefaultAsync(t => t.Id == companyId);
                if (company != null)
                {
                    if (company.CompanyAddresses.Any())
                    {
                        var defaultCompanyAddress = company.CompanyAddresses.SingleOrDefault(t => t.IsActive && t.IsDefault);
                        if (defaultCompanyAddress != null)
                        {
                            response = defaultCompanyAddress.ToViewModel();
                        }
                    }

                    response.CompanyTypeId = (CompanyType)company.CompanyTypeId;
                    var companyBillingAddress = company.BillingAddresses.OrderByDescending(t => t.Id).FirstOrDefault(t => t.CompanyId == companyId && t.IsActive && t.IsDefault);
                    if (companyBillingAddress != null)
                    {
                        response.BillingAddress = companyBillingAddress.ToViewModel();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCompanyAddressAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> SaveCompanyAddressSettingsAsync(CompanyAddressViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var company = await Context.DataContext.Companies.Include(t => t.CompanyAddresses)
                                                                     .SingleOrDefaultAsync(t => t.Id == viewModel.CompanyId);
                    if (company != null)
                    {
                        var companyAddress = company.CompanyAddresses.SingleOrDefault(t => t.Id == viewModel.Id);
                        //if (companyAddress != null)
                        //{
                        //    var uncheckedQualifications = companyAddress.MstSupplierQualifications.Select(t => t.Id).Except(viewModel.SupplierProfile.SupplierQualifications);
                        //    var orderQualifications = new List<int>();
                        //    company.Orders.Where(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open).ToList().ForEach(
                        //        t => orderQualifications.AddRange(t.FuelRequest.MstSupplierQualifications.Select(t1 => t1.Id).ToList()));
                        //    if (uncheckedQualifications.Intersect(orderQualifications).Any())
                        //    {
                        //        var orderQualificationNames = Context.DataContext.MstSupplierQualifications.Where(t => uncheckedQualifications.Intersect(orderQualifications).Contains(t.Id)).Select(t => t.Name).Distinct().ToList();
                        //        response.StatusCode = Status.Failed;
                        //        response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageEditQualificationWithOpenOrders, new object[] { string.Join(", ", orderQualificationNames) });

                        //        transaction.Commit();
                        //        return response;
                        //    }
                        //}

                        var existingCompanyAddress = Context.DataContext.CompanyAddresses.Where(t => t.CompanyId == viewModel.CompanyId && t.ZipCode == viewModel.ZipCode &&
                                                                                      t.Address == viewModel.Address && t.City == viewModel.City &&
                                                                                      t.StateId == viewModel.State.Id);

                        if ((existingCompanyAddress.Any() && companyAddress == null) || (existingCompanyAddress.Any(t => t.Id != viewModel.Id)))
                        {
                            response.StatusMessage = Resource.errMessageSimilarCompanyAddressPresent;
                            return response;
                        }

                        if (viewModel.IsDefault)
                        {
                            if (company.CompanyAddresses.Any(t => t.IsDefault))
                            {
                                company.CompanyAddresses.ToList().ForEach(t => t.IsDefault = false);
                            }
                        }

                        var state = Context.DataContext.MstStates.First(t => t.Id == viewModel.State.Id);
                        var country = Context.DataContext.MstCountries.First(t => t.Id == viewModel.Country.Id).Code;
                        if (viewModel.Country.Id == (int)Country.CAR && viewModel.IsMissingAddress())
                        {
                            if (string.IsNullOrWhiteSpace(viewModel.Address))
                                viewModel.Address = state.Name ?? Resource.lblCaribbean;
                            if (string.IsNullOrWhiteSpace(viewModel.City))
                                viewModel.City = state.Name ?? Resource.lblCaribbean;
                            if (string.IsNullOrWhiteSpace(viewModel.ZipCode))
                                viewModel.ZipCode = state.Name ?? Resource.lblCaribbean;
                        }
                        var point = GoogleApiDomain.GetGeocode($"{viewModel.Address} {viewModel.City} {state.Code} {country} {viewModel.ZipCode}");

                        if (companyAddress != null)
                        {
                            viewModel.ToEntity(companyAddress);
                            if (point != null)
                            {
                                companyAddress.Latitude = Convert.ToDecimal(point.Latitude);
                                companyAddress.Longitude = Convert.ToDecimal(point.Longitude);
                            }
                            else
                            {
                                companyAddress.Latitude = 0;
                                companyAddress.Longitude = 0;
                            }
                        }
                        else
                        {
                            companyAddress = viewModel.ToEntity();

                            if (point != null)
                            {
                                companyAddress.Latitude = Convert.ToDecimal(point.Latitude);
                                companyAddress.Longitude = Convert.ToDecimal(point.Longitude);
                            }

                            Context.DataContext.CompanyAddresses.Add(companyAddress);
                            await Context.CommitAsync();
                        }

                        if (company.CompanyTypeId != (int)CompanyType.Buyer)
                        {
                            var supplierAddressXProductTypes = Context.DataContext.MstProductTypes.Where(t => viewModel.SupplierProductTypes.Contains(t.Id)).ToList();
                            var servingAreas = await GetServiceOfferingModel(viewModel.ServiceOffering);

                            foreach (var pAddress in companyAddress.CompanyXServingLocations.ToList())
                            {
                                Context.DataContext.CompanyXServingLocations.Remove(pAddress);
                            }
                            Context.Commit();
                            servingAreas.Where(t => t.IsEnable).ToList().ToEntity().ForEach(t => companyAddress.CompanyXServingLocations.Add(t));

                            //Delete all existing fueltypes and add new
                            companyAddress.MstProductTypes.ToList().RemoveAll(t => t.Id > 0);
                            companyAddress.MstProductTypes = supplierAddressXProductTypes;

                        }

                        await Context.CommitAsync();
                    }
                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageCompanyAddressSaveSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageCompanyAddressSaveFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "SaveCompanyAddressSettingsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<ServiceOffering>> GetServiceOfferingModel(List<CompanyServiceAreaModel> viewModel)
        {
            var response = new List<ServiceOffering>();
            if (viewModel != null)
            {
                var states = viewModel.SelectMany(t => t.StateIds);
                var cities = viewModel.SelectMany(t => t.CityIds).Where(t => t.HasValue).ToList();
                var ZipData = await Context.DataContext.MstCities.Where(t => states.Contains(t.StateId) && cities.Contains(t.Id))
                                                .Select(t => new { CityId = t.Id, t.StateId, t.ZipCodes }).ToListAsync();
                foreach (var service in viewModel)
                {
                    var serviceType = new ServiceOffering { ServiceDeliveryType = service.ServiceDeliveryType, IsEnable = service.StateIds.Any() };
                    if (service.CityIds.Any() && ZipData.Any())
                    {
                        serviceType.ServiceAreas = ZipData.Where(t => service.StateIds.Contains(t.StateId)
                                                                        && service.CityIds.Contains(t.CityId)
                                                                        && service.ZipCodes.Any(t1 => t.ZipCodes.Contains(t1)))
                                                          .Select(t => new ServiceArea
                                                          {
                                                              StateId = t.StateId,
                                                              CityId = t.CityId,
                                                              ZipCode = string.Join(",", service.ZipCodes.Where(t1 => t.ZipCodes.Contains(t1)))
                                                          }).ToList();
                    }
                    else if (service.StateIds.Any())
                    {
                        serviceType.ServiceAreas = service.StateIds.Select(t => new ServiceArea { StateId = t }).ToList();
                    }
                    response.Add(serviceType);
                }
            }
            return response;
        }
        public async Task<StatusViewModel> SaveBillingAddressAsync(CompanyAddressViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            if (viewModel.BillingAddress != null)
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var billingAddresses = Context.DataContext.BillingAddresses.Where(t => t.CompanyId == viewModel.CompanyId && t.IsActive && t.IsDefault);
                        if (billingAddresses.Any())
                        {
                            foreach (var billingadd in billingAddresses)
                            {
                                billingadd.IsActive = false;
                                billingadd.IsDefault = false;
                            }
                        }
                        await Context.CommitAsync();

                        //var billingState = Context.DataContext.MstStates.First(t => t.Id == viewModel.BillingAddress.State.Id);
                        //var billingCountry = Context.DataContext.MstCountries.First(t => t.Id == viewModel.BillingAddress.Country.Id).Code;

                        //if (viewModel.BillingAddress.Country.Id == (int)Country.CAR && viewModel.BillingAddress.IsMissingAddress())
                        //{
                        //    if (string.IsNullOrWhiteSpace(viewModel.BillingAddress.Address))
                        //        viewModel.BillingAddress.Address = billingState.Name ?? Resource.lblCaribbean;
                        //    if(string.IsNullOrWhiteSpace(viewModel.BillingAddress.City))
                        //        viewModel.BillingAddress.City = billingState.Name ?? Resource.lblCaribbean;
                        //    if(string.IsNullOrWhiteSpace(viewModel.BillingAddress.ZipCode))
                        //        viewModel.BillingAddress.ZipCode = billingState.Name ?? Resource.lblCaribbean;

                        //}
                        var billingPoint = GoogleApiDomain.GetGeocode($"{viewModel.BillingAddress.Address} {viewModel.BillingAddress.City} {viewModel.BillingAddress.County} {viewModel.BillingAddress.ZipCode} {viewModel.BillingAddress.State.Name} {viewModel.BillingAddress.Country.Name}");
                        if (billingPoint != null)
                        {
                            viewModel.BillingAddress.Latitude = Convert.ToDecimal(billingPoint.Latitude);
                            viewModel.BillingAddress.Longitude = Convert.ToDecimal(billingPoint.Longitude);
                        }

                        var address = viewModel.BillingAddress.ToEntity();
                        address.IsDefault = true; //set as default (We Inactivated the previous default address).

                        Context.DataContext.BillingAddresses.Add(address);
                        await Context.CommitAsync();

                        transaction.Commit();

                        //Send response
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageBillingAddressSaveSuccess;
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageBillingAddressSaveFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("SettingsDomain", "SaveBillingAddressAsync", ex.Message, ex);
                    }
                }
            }

            return response;
        }

        public async Task<StatusViewModel> ChangeCompanyAddressSettingsStatusAsync(int companyAddressId, int userId, bool isActive)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var companyAddress = await Context.DataContext.CompanyAddresses.SingleOrDefaultAsync(t => t.Id == companyAddressId);
                    if (companyAddress != null)
                    {
                        companyAddress.UpdatedBy = userId;
                        companyAddress.IsActive = isActive;
                    }

                    await Context.CommitAsync();

                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageCompanyAddressStatusSaveSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageCompanyAddressStatusSaveFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "ChangeCompanyAddressSettingsStatusAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<PaymentViewModel>> GetCompanyPaymentCardListAsync(int companyId)
        {
            List<PaymentViewModel> response = new List<PaymentViewModel>();
            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId && t.IsActive);
                if (company != null)
                {
                    var companyStripeCards = company.CompanyXStripeCards.Where(t => t.CompanyId == companyId).ToList();
                    companyStripeCards.ForEach(t => response.Add(t.ToPaymentViewModel()));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCompanyPaymentCardListAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<PaymentViewModel> GetCompanyPaymentInformationAsync(int companyId, int paymentCardId = 0)
        {
            PaymentViewModel response = new PaymentViewModel(Status.Success);
            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId && t.IsActive);
                if (company != null)
                {
                    if (paymentCardId > 0)
                    {
                        var companyUserPaymentCard = company.CompanyXStripeCards.SingleOrDefault(t => t.Id == paymentCardId);

                        if (companyUserPaymentCard != null)
                        {
                            response = companyUserPaymentCard.ToPaymentViewModel();
                        }
                        response.DisplayMode = PageDisplayMode.View;
                    }
                    else
                    {
                        var cardDetails = company.CompanyXStripeCards.FirstOrDefault(t => t.IsPrimary);
                        if (cardDetails != null)
                        {
                            response = cardDetails.ToPaymentViewModel();
                        }
                        response.Card = new PaymentCardViewModel();
                        response.Id = 0;
                        response.CardToken = null;
                        response.DisplayMode = PageDisplayMode.Create;
                        response.CompanyId = company.Id;
                    }
                    response.Card.IsPrimaryVisible = true;
                    var state = await Context.DataContext.MstStates.SingleOrDefaultAsync(t => t.Code == response.State.Code);
                    var country = await Context.DataContext.MstCountries.SingleOrDefaultAsync(t => t.Code == response.Country.Code);
                    if (state != null)
                    {
                        response.State.Id = state.Id;
                    }

                    if (country != null)
                    {
                        response.Country.Id = country.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                response.CompanyId = companyId;
                response.DisplayMode = paymentCardId > 0 ? PageDisplayMode.View : PageDisplayMode.Create;
                response.StatusMessage = Resource.errMessageCompanyGetPaymentCardFailed;
                LogManager.Logger.WriteException("SettingsDomain", "GetCompanyPaymentInformationAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveCompanyPaymentInformationAsync(PaymentViewModel viewModel)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == viewModel.CompanyId);
                    if (company != null)
                    {
                        if (company.CompanyXStripeCards.Any(t => t.IsPrimary))
                        {
                            if (viewModel.Card.IsPrimary)
                            {
                                company.CompanyXStripeCards.ToList().ForEach(t => t.IsPrimary = false);
                            }
                        }
                        else
                        {
                            viewModel.Card.IsPrimary = true;
                        }

                        var state = await Context.DataContext.MstStates.SingleOrDefaultAsync(t => t.Id == viewModel.State.Id);
                        var country = await Context.DataContext.MstCountries.SingleOrDefaultAsync(t => t.Id == viewModel.Country.Id);
                        if (state != null)
                        {
                            viewModel.State.Code = state.Code;
                        }

                        if (country != null)
                        {
                            viewModel.Country.Code = country.Code;
                        }
                        var companyPaymentCard = viewModel.ToCompanyUserXStripeCardEntity();
                        Context.DataContext.CompanyXStripeCards.Add(companyPaymentCard);
                    }

                    await Context.DataContext.SaveChangesAsync();
                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSaveCompanyPaymentCardSuccess;
                }
                catch (StripeException ex)
                {
                    transaction.Rollback();
                    if (ex.StripeError.DeclineCode == "live_mode_test_card" && ex.StripeError.ErrorType == "card_error")
                    {
                        response.StatusMessage = Resource.errMessageTestCardUsedInLive;
                    }
                    if (ex.StripeError.Code == "incorrect_number" && ex.StripeError.ErrorType == "card_error")
                    {
                        response.StatusMessage = Resource.errMessageTestCardUsedInLive;
                    }
                    if (ex.StripeError.Code == "parameter_missing" && ex.StripeError.ErrorType == "invalid_request_error")
                    {
                        response.StatusMessage = Resource.errMessageCreditCardNumberRequired;
                    }
                    LogManager.Logger.WriteException("SettingsDomain", "SaveCompanyPaymentInformationAsync", ex.Message, ex);
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageSaveCompanyPaymentCardFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "SaveCompanyPaymentInformationAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<CompanySettingViewModel> GetCompanySettingDetailsAsync(int companyId)
        {
            CompanySettingViewModel response = new CompanySettingViewModel();
            try
            {
                var companySetting = await Context.DataContext.CompanySettings.SingleOrDefaultAsync(t => t.CompanyId == companyId);
                if (companySetting != null)
                {
                    response.ProcessingFee = companySetting.ProcessingFee;
                    response.ProcessingFeeType = companySetting.ProcessingType;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCompanySettingDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveCompanySettingAsync(CompanySettingViewModel viewModel, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var companySetting = await Context.DataContext.CompanySettings.SingleOrDefaultAsync(t => t.CompanyId == userContext.CompanyId);
                        if (companySetting == null)
                        {
                            companySetting = new CompanySetting();
                            companySetting.CompanyId = userContext.CompanyId;
                            companySetting.CreatedBy = userContext.Id;
                            companySetting.CreatedDate = DateTimeOffset.Now;
                            companySetting.UpdatedBy = userContext.Id;
                            companySetting.UpdatedDate = DateTimeOffset.Now;
                            companySetting.ProcessingFee = viewModel.ProcessingFee;
                            companySetting.ProcessingType = viewModel.ProcessingFeeType;
                            Context.DataContext.CompanySettings.Add(companySetting);
                        }
                        else
                        {
                            companySetting.ProcessingFee = viewModel.ProcessingFee;
                            companySetting.ProcessingType = viewModel.ProcessingFeeType;
                            companySetting.UpdatedBy = userContext.Id;
                            companySetting.UpdatedDate = DateTimeOffset.Now;
                            Context.DataContext.Entry(companySetting).State = EntityState.Modified;
                        }

                        await Context.CommitAsync();
                        transaction.Commit();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageCCProcessingSucess;
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessagFailedToSaveDetails;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("SettingsDomain", "SaveCompanySettingAsync", ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "SaveCompanySettingAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> SaveUserNotificationSettingsAsync(UserNotificationsViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == viewModel.UserId);
                    if (user != null)
                    {
                        foreach (var setting in viewModel.UserNotificationSettings)
                        {
                            var itemToChange = user.UserXNotificationSettings.FirstOrDefault(t => t.UserId == setting.UserId && t.EventTypeId == setting.EventTypeId);
                            if (itemToChange != null)
                            {
                                itemToChange.IsEmail = setting.IsEmail;
                                itemToChange.IsInApp = setting.IsInApp;
                                itemToChange.IsSMS = setting.IsSMS;
                            }
                            else if (setting.IsEmail || setting.IsSMS)
                            {
                                UserXNotificationSetting userXNotification = new UserXNotificationSetting();
                                userXNotification.IsEmail = setting.IsEmail;
                                userXNotification.IsInApp = setting.IsInApp;
                                userXNotification.IsSMS = setting.IsSMS;
                                userXNotification.UserId = setting.UserId;
                                userXNotification.EventTypeId = setting.EventTypeId;
                                Context.DataContext.UserXNotificationSettings.Add(userXNotification);
                            }
                        }
                    }

                    await Context.CommitAsync();

                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSaveUserNotificationSettingsSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageSaveUserNotificationSettingsFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "SaveUserNotificationSettingsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteCompanyPaymentInformationAsync(int paymentCardId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var companyPaymentCard = Context.DataContext.CompanyXStripeCards.SingleOrDefault(t => t.Id == paymentCardId);
                    if (companyPaymentCard != null)
                    {
                        Context.DataContext.CompanyXStripeCards.Remove(companyPaymentCard);
                        await Context.CommitAsync();
                    }

                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageDeleteCompanyPaymentCardSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageDeleteCompanyPaymentCardFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "DeleteCompanyPaymentInformationAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<UserViewModel> GetUserAsync(int userId)
        {
            UserViewModel response = new UserViewModel();
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                if (user != null)
                {
                    response = user.ToViewModel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetUserAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveUserInformationAsync(UserViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == viewModel.Id);
                    if (user != null)
                    {
                        user = viewModel.ToEntity(user);
                        await Context.CommitAsync();

                        //return some extra information
                        viewModel = user.ToViewModel(viewModel);
                    }
                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageUserInformationSaveSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageUserInformationSaveFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "SaveUserInformationAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<UserNotificationSettingViewModel>> GetUserNotificationSettingsAsync(int userId)
        {
            List<UserNotificationSettingViewModel> response = new List<UserNotificationSettingViewModel>();
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                if (user != null)
                {
                    response = user.UserXNotificationSettings.Where(t => t.UserId == userId).Select(t => new UserNotificationSettingViewModel(Status.Success)
                    {
                        UserId = user.Id,
                        EventTypeId = t.EventTypeId,
                        EventTypeName = t.MstEventType.Name,
                        IsEmail = t.IsEmail,
                        IsSMS = t.IsSMS,
                        IsInApp = t.IsInApp

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetUserNotificationSettingsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UserNotificationSettingViewModel>> GetUserNotificationSettingsAsync(int userId, int companyTypeId)
        {
            List<UserNotificationSettingViewModel> response = new List<UserNotificationSettingViewModel>();

            try
            {
                response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetUserNotificationSettingsAsync(userId, companyTypeId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetUserNotificationSettingsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<AdditionalUserViewModel>> GetInvitedUserListAsync(int companyId, int userId, List<int> roleIds = null)
        {
            List<AdditionalUserViewModel> response = new List<AdditionalUserViewModel>();
            try
            {
                var company = Context.DataContext.Companies.Where(t => t.Id == companyId);
                if (company != null)
                {
                    var companyInvitedUsers = company.SelectMany(t => t.CompanyXAdditionalUserInvites);
                    var invitedUsers = await companyInvitedUsers.OrderByDescending(t => t.Id).ToListAsync();

                    response = invitedUsers.Where(t => t.CompanyId == companyId && (roleIds == null || t.MstRoles.Any(r => roleIds.Contains(r.Id)))).Select(t => new AdditionalUserViewModel(Status.Success)
                    {
                        Id = t.Id,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Name = $"{t.FirstName} {t.LastName}",
                        Email = t.Email,
                        CompanyId = t.Company.Id,
                        CompanyName = t.Company.Name,
                        InvitedBy = $"{t.User.FirstName} {t.User.LastName}",
                        RoleIds = t.MstRoles.Select(x => x.Id).ToList(),
                        IsInvitationSent = t.IsInvitationSent,
                        IsOnboarded = t.User.IsOnboardingComplete,
                        PhoneNumber = t.User.PhoneNumber,
                        RoleNames = string.Join(" <br/>", t.MstRoles.Select(x => x.Name).ToList()),
                        DT_RowId = "DT_RowId_" + t.Id
                    }).ToList();
                    var driverEmailIds = response.Select(x => x.Email).ToList();
                    var driver = Context.DataContext.Users.Where(t => !t.IsActive && t.CompanyId == companyId
                                                       && !t.IsOnboardingComplete && driverEmailIds.Contains(t.UserName)).Select(x => new { x.Id, x.UserName }).ToList();
                    response.ForEach(x =>
                    {
                        var driverInfo = driver.FirstOrDefault(x1 => x1.UserName == x.Email);
                        if (driverInfo != null)
                        {
                            x.DriverId = driverInfo.Id;
                        }
                    });
                    List<RegionDriverRemoveModel> driverRemoveModel = new List<RegionDriverRemoveModel>();
                    response.ForEach(x =>
                    {
                        driverRemoveModel.Add(new RegionDriverRemoveModel { DriverId = x.DriverId, UserId = userId });
                    });

                    var driverDSBScheduleInfo = await ContextFactory.Current.GetDomain<FreightServiceDomain>().CheckInvitedDriverScheduleExists(driverRemoveModel);
                    driverDSBScheduleInfo.ForEach(x =>
                    {
                        var driverDSBINfo = response.FirstOrDefault(x1 => x1.DriverId == x.DriverId);
                        if (driverDSBINfo != null)
                        {
                            driverDSBINfo.IsScheduleExists = true;
                            driverDSBINfo.ScheduleBuilderIds.AddRange(x.ScheduleBuilderIds);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetInvitedUserListAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<AdditionalUserViewModel>> GetOnboardedUserListAsync(int companyId, int userId, List<int> roleIds = null)
        {
            List<AdditionalUserViewModel> response = new List<AdditionalUserViewModel>();
            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId && t.IsActive);
                if (company != null)
                {
                    response = company.Users.Where(t => t.Id != userId && t.IsOnboardingComplete && (roleIds == null || t.MstRoles.Any(r => roleIds.Contains(r.Id)))).Select(t => new AdditionalUserViewModel(Status.Success)
                    {
                        Id = t.Id,
                        DriverUserId = t.Id,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Email = t.Email,
                        CompanyId = t.Company.Id,
                        RoleIds = t.MstRoles.Select(x => x.Id).ToList(),
                        RoleNames = string.Join(" <br/>", t.MstRoles.Select(x => x.Name).ToList()),
                        IsOnboarded = t.IsOnboardingComplete,
                        PhoneNumber = t.PhoneNumber,
                        IsActive = t.IsActive,
                        IsApiAccessAllowed = t.IsApiAccessAllowed

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetOnboardedUserListAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ApiUserViewModel>> CompanyOnboardedApiUsers(int companyId, int userId)
        {
            List<ApiUserViewModel> response = new List<ApiUserViewModel>();
            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId && t.IsActive);
                if (company != null)
                {
                    response = company.Users.Where(t => t.Id != userId && t.IsOnboardingComplete && t.IsActive && t.IsApiAccessAllowed && t.MstRoles.Any(r => r.Id == (int)UserRoles.Admin)).Select(t => new ApiUserViewModel()
                    {
                        UserId = t.Id,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Email = t.Email,
                        UserName = t.UserName,
                        CompanyId = t.Company.Id,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "CompanyOnboardedApiUsers", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SetApiUserPassword(ApiUserViewModel apiUser, UserContext currentUser)
        {
            var response = new StatusViewModel();

            try
            {
                var user = Context.DataContext.Users.SingleOrDefault(t => t.Email.ToLower() == apiUser.Email.ToLower() && t.CompanyId == currentUser.CompanyId);
                if (user != null)
                {
                    if (user.IsEmailConfirmed && user.IsActive && user.IsApiAccessAllowed)
                    {
                        var salt = CryptoHelperMethods.GenerateSalt();
                        user.AccessFailedCount = 0;
                        user.LockoutEndDateUtc = null;
                        user.PasswordHash = CryptoHelperMethods.GenerateHash(apiUser.PlainPassword, salt);
                        user.SecurityStamp = salt;
                        user.FingerPrint = CryptoHelperMethods.GenerateHash(user.PasswordHash, CryptoHelperMethods.GenerateSalt());

                        user.UpdatedBy = currentUser.Id;
                        user.UpdatedDate = DateTimeOffset.Now;

                        //Add user and default role as admin
                        Context.DataContext.Entry(user).State = EntityState.Modified;
                        await Context.CommitAsync();

                        response.StatusCode = Status.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingDomain", "SetApiUserPassword", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<AdditionalUserViewModel>> GetUserListForManageAlertsAsync(int companyId, int userId)
        {
            List<AdditionalUserViewModel> response = new List<AdditionalUserViewModel>();
            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId && t.IsActive);
                if (company != null)
                {
                    response = company.Users.Where(t => t.IsOnboardingComplete).Select(t => new AdditionalUserViewModel(Status.Success)
                    {
                        Id = t.Id,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Email = t.Email,
                        CompanyId = t.Company.Id,
                        RoleIds = t.MstRoles.Select(x => x.Id).ToList(),
                        EventTypeId = t.UserXNotificationSettings.Where(x => x.IsSMS).Select(t1 => t1.MstEventType.Id).ToList(),
                        RoleNameList = t.MstRoles.Select(x => x.Name).ToList(),
                        IsActive = t.IsActive
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetUserListForManageAlertsAsync", ex.Message, ex);
            }
            return response;
        }

        public StatusViewModel UpdateUserEvents(int userId, string events)
        {
            var response = new StatusViewModel();
            List<int> eventTypeIds = JsonConvert.DeserializeObject<List<int>>(events);
            var userNotification = Context.DataContext.UserXNotificationSettings.Where(t =>
                                    t.UserId == userId &&
                                    (t.MstEventType.NotificationType == (int)NotificationType.Sms ||
                                     t.MstEventType.NotificationType == (int)NotificationType.EmailAndSms)
                                    ).ToList();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var userEvents = new List<int>();
                    foreach (var item in userNotification)
                    {
                        if (eventTypeIds.Contains(item.EventTypeId))
                            item.IsSMS = true;
                        else
                            item.IsSMS = false;

                        userEvents.Add(item.EventTypeId);
                    }

                    var notPresentUserEvents = eventTypeIds.Except(userEvents);
                    foreach (var notPresentUserEvent in notPresentUserEvents)
                    {
                        var userXNotification = new UserXNotificationSetting();
                        userXNotification.UserId = userId;
                        userXNotification.EventTypeId = notPresentUserEvent;
                        userXNotification.IsEmail = false;
                        userXNotification.IsSMS = true;
                        userXNotification.IsInApp = false;
                        Context.DataContext.UserXNotificationSettings.Add(userXNotification);
                    }

                    Context.Commit();
                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageNotificationUpdatedSuccessfully;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingDomain", "UpdateUserEvents", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<AdditionalUsersViewModel> GetInvitedAdditionalUsersAsync(int additionUserId = 0, bool isInvitedUser = true, int companyId = 0)
        {
            AdditionalUsersViewModel response = new AdditionalUsersViewModel(Status.Success);
            try
            {
                if (additionUserId > 0)
                {
                    var driverUserId = 0;
                    var invitedUser = new AdditionalUserViewModel(Status.Success);
                    if (isInvitedUser)
                    {
                        var additionalUser = await Context.DataContext.CompanyXAdditionalUserInvites
                            .Select(t => new
                            {
                                t.Id,
                                t.FirstName,
                                t.LastName,
                                t.Email,
                                InvitedBy = t.User == null ? "" : t.User.FirstName + " " + t.User.LastName,
                                UserId = t.User == null ? (int?)null : t.User.Id,
                                RoleIds = t.MstRoles.Select(x => x.Id),
                                t.Company.CompanyTypeId,
                                t.CompanyId,
                                t.Title
                            }).SingleOrDefaultAsync(t => t.Id == additionUserId);

                        invitedUser.Id = additionalUser.Id;
                        invitedUser.FirstName = additionalUser.FirstName;
                        invitedUser.LastName = additionalUser.LastName;
                        invitedUser.Email = additionalUser.Email;
                        invitedUser.DisplayMode = PageDisplayMode.Edit;
                        invitedUser.InvitedBy = additionalUser.InvitedBy;
                        invitedUser.RoleIds = additionalUser.RoleIds.ToList();
                        invitedUser.IsInvitedUser = true;
                        invitedUser.CompanyTypeId = additionalUser.CompanyTypeId;
                        invitedUser.CompanyId = additionalUser.CompanyId;
                        invitedUser.Title = additionalUser.Title;

                        response.CompanyTypeId = additionalUser.CompanyTypeId;
                        response.CompanyId = additionalUser.CompanyId;
                        response.AdditionalUsers.Add(invitedUser);
                        if (invitedUser.RoleIds.Contains((int)UserRoles.Driver))
                        {
                            driverUserId = Context.DataContext.Users
                                            .Where(t => t.Email == additionalUser.Email && t.CompanyId == additionalUser.CompanyId)
                                            .Select(t => t.Id).FirstOrDefault();
                        }
                    }
                    else
                    {
                        var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == additionUserId);

                        invitedUser.Id = user.Id;
                        invitedUser.FirstName = user.FirstName;
                        invitedUser.LastName = user.LastName;
                        invitedUser.Email = user.Email;
                        invitedUser.DisplayMode = PageDisplayMode.Edit;
                        invitedUser.RoleIds = user.MstRoles.Select(x => x.Id).ToList();
                        invitedUser.IsInvitedUser = false;
                        invitedUser.CompanyId = user.Company.Id;
                        invitedUser.CompanyTypeId = user.Company.CompanyTypeId;
                        invitedUser.IsApiAccessAllowed = user.IsApiAccessAllowed;
                        invitedUser.Title = user.Title;

                        response.CompanyTypeId = user.Company.CompanyTypeId;
                        response.CompanyId = user.Company.Id;
                        response.AdditionalUsers.Add(invitedUser);
                        driverUserId = user.Id;
                    }

                    if (invitedUser.RoleIds.Contains((int)UserRoles.Driver))
                    {
                        var fsDomain = new FreightServiceDomain(this);
                        var driverObject = await fsDomain.GetDriverObjectById(driverUserId);
                        if (driverObject != null)
                        {
                            invitedUser.DriverInfo = new DriverInformationViewModel()
                            {
                                LicenseNumber = driverObject.LicenseNumber,
                                ProfilePhotoUrl = driverObject.ProfilePhoto,
                                ShiftId = driverObject.ShiftId,
                                TrailerType = driverObject.TrailerType,
                                CardNumbers = driverObject.CardNumbers,
                                ExpiryDate = driverObject.ExpiryDate,
                                CompanyName = driverObject.CompanyName,
                                LicenseTypeId = driverObject.LicenseTypeId,
                                IsFilldAuthorized = driverObject.IsFilldAuthorized,
                            };
                        }
                    }
                }
                else
                {
                    response.CompanyId = companyId;
                    response.AdditionalUsers.Add(new AdditionalUserViewModel(Status.Success) { CompanyId = companyId });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetInvitedAdditionalUsersAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<Response> CreateAdditionalUsersAsync(AdditionalUsersViewModel viewModel)
        {
            Response response = new Response(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    response = await AddCompanyUser(viewModel);
                    if (response.StatusCode == Status.Success)
                    {
                        transaction.Commit();
                        response.StatusMessages.Add(Resource.errMessageCreateAdditionalUserSuccess);
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessages.Add(Resource.errMessageCreateAdditionalUserFailed);
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "CreateAdditionalUsersAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<Response> AddCompanyUser(AdditionalUsersViewModel viewModel)
        {
            Response response = new Response(Status.Failed);
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == viewModel.UserId);
                if (user != null && user.Company != null)
                {
                    List<string> email = viewModel.AdditionalUsers.Select(t => t.Email).ToList();
                    Context.DataContext.Users.Where(t => email.Contains(t.Email)).ToList()
                        .ForEach(t => response.StatusMessages.Add(ResourceMessages.GetMessage(Resource.errMessageUserAlreadyOnboardedAgainstCompany,
                                new[] { t.Email, t.Company.Name })));

                    Context.DataContext.CompanyXAdditionalUserInvites.Where(t => email.Contains(t.Email)).ToList()
                        .ForEach(t => response.StatusMessages.Add(
                           ResourceMessages.GetMessage(Resource.errMessageUserAlreadyInvited,
                                new[] { t.Email, t.User.FirstName, t.User.LastName, t.Company.Name })));

                    var fsDomain = new FreightServiceDomain(this);
                    if (response.StatusMessages.Count == 0)
                    {
                        foreach (var additionalUser in viewModel.AdditionalUsers)
                        {
                            if (additionalUser.RoleIds.Contains((int)UserRoles.Admin) && additionalUser.RoleIds.Count > 0)
                            {
                                additionalUser.RoleIds = new List<int> { (int)UserRoles.Admin };
                            }
                            CompanyXAdditionalUserInvite companyXAdditionalUserInvite = new CompanyXAdditionalUserInvite
                            {
                                Id = additionalUser.Id,
                                Email = additionalUser.Email.Trim().ToLower(),
                                InvitedBy = viewModel.UserId,
                                CompanyId = user.Company.Id,
                                FirstName = additionalUser.FirstName,
                                LastName = additionalUser.LastName,
                                MstRoles = Context.DataContext.MstRoles.Where(t => additionalUser.RoleIds.Contains(t.Id)).ToList(),
                                IsInvitationSent = false,
                                Title = additionalUser.Title
                            };

                            user.Company.CompanyXAdditionalUserInvites.Add(companyXAdditionalUserInvite);
                            await Context.CommitAsync();

                            //Add to user if driver role
                            if (additionalUser.RoleIds.Contains((int)UserRoles.Driver))
                            {
                                var StaticPassword = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingApplicationUserDefaultPassword, "First#1234");
                                var RandomPassword = CryptoHelperMethods.GeneratePassword(Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings[ApplicationConstants.RandomPasswordLength]), StaticPassword);
                                var salt = CryptoHelperMethods.GenerateSalt();
                                User newUser = new User
                                {
                                    FirstName = additionalUser.FirstName,
                                    LastName = additionalUser.LastName,
                                    UserName = additionalUser.Email.Trim().ToLower(),
                                    Email = additionalUser.Email.Trim().ToLower(),
                                    IsEmailConfirmed = false,
                                    PhoneNumber = string.IsNullOrWhiteSpace(additionalUser.PhoneNumber) ? Constants.DummyPhoneNumber : additionalUser.PhoneNumber,
                                    IsPhoneNumberConfirmed = false,
                                    IsTwoFactorEnabled = false,
                                    AccessFailedCount = 0,
                                    IsLockoutEnabled = true,
                                    LockoutEndDateUtc = null,
                                    PasswordHash = CryptoHelperMethods.GenerateHash(RandomPassword, salt),
                                    SecurityStamp = salt,
                                    FingerPrint = CryptoHelperMethods.GenerateHash(additionalUser.Email, CryptoHelperMethods.GenerateSalt()),
                                    IsOnboardingComplete = false,
                                    IsActive = false,
                                    CreatedBy = viewModel.UserId,
                                    CreatedDate = DateTimeOffset.Now,
                                    UpdatedBy = viewModel.UserId,
                                    UpdatedDate = DateTimeOffset.Now,
                                    CompanyId = additionalUser.CompanyId,
                                    MstRoles = Context.DataContext.MstRoles.Where(t => additionalUser.RoleIds.Contains(t.Id)).ToList(),
                                    Title = additionalUser.Title
                                };
                                Context.DataContext.Users.Add(newUser);
                                await Context.CommitAsync();
                                additionalUser.Id = newUser.Id;
                                await AddDriverObject(fsDomain, additionalUser, newUser);
                            }

                            //Add an entry to notifications
                            var notificationDomain = new NotificationDomain(this);
                            if (!string.IsNullOrEmpty(viewModel.SupplierURL))
                            {
                                var message = new AddUserMessageViewModel { SupplierURL = viewModel.SupplierURL };
                                var jsonMessage = new JavaScriptSerializer().Serialize(message);
                                await notificationDomain.AddNotificationEventAsync(EventType.AdditionalUserAdded, companyXAdditionalUserInvite.Id, user.Id, null, jsonMessage, viewModel.ApplicationTemplateId);
                            }
                            else
                            {
                                await notificationDomain.AddNotificationEventAsync(EventType.AdditionalUserAdded, companyXAdditionalUserInvite.Id, user.Id, null, null, viewModel.ApplicationTemplateId);
                            }
                            response.StatusCode = Status.Success;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusMessages.Add(Resource.errMessageCreateAdditionalUserFailed);
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("SettingsDomain", "AddCompanyUser", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> EditAdditionalUsersAsync(AdditionalUsersViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var currentUser = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == viewModel.UserId);
                    if (currentUser != null && currentUser.Company != null)
                    {
                        foreach (var additionalUser in viewModel.AdditionalUsers)
                        {
                            if (additionalUser.Id != 0)
                            {
                                if (additionalUser.RoleIds.Contains((int)UserRoles.Admin) && additionalUser.RoleIds.Count > 0)
                                {
                                    additionalUser.RoleIds = new List<int> { (int)UserRoles.Admin };
                                }
                                else
                                {
                                    additionalUser.IsApiAccessAllowed = false;
                                }
                                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == additionalUser.Id);
                                var userRoles = Context.DataContext.MstRoles.Where(t => additionalUser.RoleIds.Contains(t.Id)).ToList();

                                if (String.IsNullOrEmpty(additionalUser.InvitedBy))
                                {
                                    var existingRoles = user.MstRoles.Select(t => t.Id);
                                    if (!Enumerable.SequenceEqual(existingRoles, userRoles.Select(x => x.Id).ToList()))
                                    {
                                        user.FingerPrint = CryptoHelperMethods.GenerateHash(user.Email, CryptoHelperMethods.GenerateSalt());
                                        user.UpdatedBy = currentUser.Id;
                                        user.UpdatedDate = DateTimeOffset.Now;
                                    }
                                    user.MstRoles.ToList().RemoveAll(t => t.Id > 0);
                                    user.IsApiAccessAllowed = additionalUser.IsApiAccessAllowed;
                                    user.MstRoles = userRoles;
                                    user.PhoneNumber = string.IsNullOrWhiteSpace(additionalUser.PhoneNumber) ? Constants.DummyPhoneNumber : additionalUser.PhoneNumber;

                                    if (!additionalUser.RoleIds.Contains((int)UserRoles.Admin))
                                    {
                                        user.IsApiAccessAllowed = false;
                                    }
                                    await Context.CommitAsync();

                                    await UpdateFreightDriverObject(additionalUser, userRoles, user, existingRoles);

                                    //Add an entry to notifications
                                    var notificationDomain = new NotificationDomain(this);
                                    await notificationDomain.AddNotificationEventAsync(EventType.UserRolesUpdated, user.Id, currentUser.Id);
                                }
                                else
                                {
                                    var existingInvitedUser = currentUser.Company.CompanyXAdditionalUserInvites.FirstOrDefault(t => t.Id != additionalUser.Id && t.Email.ToLower() == additionalUser.Email.ToLower());
                                    if (existingInvitedUser != null)
                                    {
                                        transaction.Rollback();
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageUserAlreadyOnboardedAgainstCompany, new[] { existingInvitedUser.Email, existingInvitedUser.Company.Name });
                                        return response;
                                    }
                                    var companyXAdditionalUserInvites = currentUser.Company.CompanyXAdditionalUserInvites.SingleOrDefault(t => t.Id == additionalUser.Id);
                                    if (companyXAdditionalUserInvites != null)
                                    {
                                        var existingUser = Context.DataContext.Users.Where(t => t.Email == companyXAdditionalUserInvites.Email && !t.IsOnboardingComplete).FirstOrDefault();

                                        companyXAdditionalUserInvites.Email = additionalUser.Email;
                                        companyXAdditionalUserInvites.FirstName = additionalUser.FirstName;
                                        companyXAdditionalUserInvites.LastName = additionalUser.LastName;
                                        companyXAdditionalUserInvites.MstRoles.ToList().RemoveAll(t => t.Id > 0);
                                        companyXAdditionalUserInvites.MstRoles = userRoles;
                                        companyXAdditionalUserInvites.Title = additionalUser.Title;

                                        if (existingUser != null)
                                        {
                                            var existingRoles = existingUser.MstRoles.Select(t => t.Id);
                                            existingUser.Email = additionalUser.Email;
                                            existingUser.FirstName = additionalUser.FirstName;
                                            existingUser.LastName = additionalUser.LastName;
                                            existingUser.PhoneNumber = string.IsNullOrWhiteSpace(additionalUser.PhoneNumber) ? Constants.DummyPhoneNumber : additionalUser.PhoneNumber;
                                            existingUser.MstRoles.ToList().RemoveAll(t => t.Id > 0);
                                            existingUser.MstRoles = userRoles;
                                            await UpdateFreightDriverObject(additionalUser, userRoles, existingUser, existingRoles);
                                        }

                                        await Context.CommitAsync();

                                        //Add an entry to notifications
                                        var notificationDomain = new NotificationDomain(this);
                                        await notificationDomain.AddNotificationEventAsync(EventType.AdditionalUserUpdated, companyXAdditionalUserInvites.Id, currentUser.Id);
                                    }
                                }
                            }
                        }
                    }

                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageUserInformationSaveSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageUserInformationSaveFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "EditAdditionalUsersAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private async Task UpdateFreightDriverObject(AdditionalUserViewModel additionalUser, List<MstRole> userRoles, User existingUser, IEnumerable<int> existingRoles)
        {
            FreightServiceDomain fsDomain = new FreightServiceDomain(this);
            if (existingRoles.Contains((int)UserRoles.Driver) && userRoles.Select(t => t.Id).Contains((int)UserRoles.Driver))
            {
                await UpdateDriverObject(fsDomain, additionalUser, existingUser);
            }
            else if (!existingRoles.Contains((int)UserRoles.Driver) && userRoles.Select(t => t.Id).Contains((int)UserRoles.Driver))
            {
                await AddDriverObject(fsDomain, additionalUser, existingUser);
            }
            else if (existingRoles.Contains((int)UserRoles.Driver) && !userRoles.Select(t => t.Id).Contains((int)UserRoles.Driver))
            {
                await fsDomain.DeleteDriverObject(existingUser.Id, existingUser.CompanyId ?? 0);
            }
        }

        private async Task AddDriverObject(FreightServiceDomain fsDomain, AdditionalUserViewModel additionalUser, User existingUser)
        {
            var driverObject = new DriverObjectModel()
            {
                DriverId = existingUser.Id,
                DriverName = $"{additionalUser.FirstName} {additionalUser.LastName}",
                CompanyId = additionalUser.CompanyId,
                LicenseNumber = additionalUser.DriverInfo?.LicenseNumber,
                ShiftId = additionalUser.DriverInfo?.ShiftId,
                TrailerType = additionalUser.DriverInfo?.TrailerType,
                CardNumbers = additionalUser.DriverInfo?.CardNumbers,
                ProfilePhoto = "",
                IsActive = additionalUser.IsActive,
                IsDeleted = false,
                CreatedBy = existingUser.CreatedBy,
                CreatedOn = existingUser.CreatedDate,
                CompanyName = additionalUser.DriverInfo?.CompanyName,
                ExpiryDate = additionalUser.DriverInfo?.ExpiryDate,
                LicenseTypeId = additionalUser.DriverInfo?.LicenseTypeId,
                Regions = additionalUser.DriverInfo?.Regions,
                IsFilldAuthorized = additionalUser.DriverInfo.IsFilldAuthorized
            };
            await fsDomain.CreateDriverObject(driverObject);
        }

        private async Task UpdateDriverObject(FreightServiceDomain fsDomain, AdditionalUserViewModel additionalUser, User exstingUser)
        {
            var driverObject = new DriverObjectModel()
            {
                DriverId = exstingUser.Id,
                DriverName = $"{additionalUser.FirstName} {additionalUser.LastName}",
                CompanyId = additionalUser.CompanyId,
                LicenseNumber = additionalUser.DriverInfo?.LicenseNumber,
                ShiftId = additionalUser.DriverInfo?.ShiftId,
                TrailerType = additionalUser.DriverInfo?.TrailerType,
                CardNumbers = additionalUser.DriverInfo?.CardNumbers,
                ProfilePhoto = "",
                IsActive = additionalUser.IsActive,
                IsDeleted = false,
                CompanyName = additionalUser.DriverInfo?.CompanyName,
                LicenseTypeId = additionalUser.DriverInfo?.LicenseTypeId,
                ExpiryDate = additionalUser.DriverInfo?.ExpiryDate,
                Regions = additionalUser.DriverInfo?.Regions,
                IsFilldAuthorized = additionalUser.DriverInfo.IsFilldAuthorized,
            };
            await fsDomain.UpdateDriverObject(driverObject);
        }

        public async Task<StatusViewModel> ChangeUserStatusAsync(int changedBy, int driverId, bool isActive)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == driverId);
                    if (user != null)
                    {
                        //removing all order and delivery assiged to this driver
                        if (!isActive)
                        {
                            var helperDomain = new HelperDomain(this);
                            var listOfOrdersAssignedToDriver = Context.DataContext.OrderXDrivers.Where(t => t.DriverId == driverId && t.IsActive).ToList();
                            listOfOrdersAssignedToDriver.ForEach((x) => helperDomain.RemoveDriverFromOrder(x, changedBy));

                            var listOfSchedulesAssignedtoDriver = Context.DataContext.DeliveryScheduleXDrivers.Where(t => t.DriverId == driverId && t.IsActive).ToList();
                            listOfSchedulesAssignedtoDriver.ForEach((x) => helperDomain.RemoveDriverFromSchedule(x, changedBy));
                        }
                        user.IsActive = isActive;
                        await Context.CommitAsync();
                        transaction.Commit();

                        //Send response
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageUserStatusChangeSuccess;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageUpdateFailed;
                    LogManager.Logger.WriteException("SettingsDomain", "ChangeUserStatusAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> ChangeAllowSalesCalculatorStatusAsync(int userId, bool isAllowed)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                    if (user != null)
                    {
                        user.IsSalesCalculatorAllowed = isAllowed;
                        await Context.CommitAsync();

                        transaction.Commit();

                        //Send response
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageUserStatusChangeSuccess;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageUpdateFailed;
                    LogManager.Logger.WriteException("SettingsDomain", "ChangeAllowSalesCalculatorStatusAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> ToggleBlockUserNotificationsAsync(int companyId, bool isBlocked)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (companyId > 0)
                    {
                        var companyUsers = await Context.DataContext.Users.Where(t => t.CompanyId == companyId).ToListAsync();
                        if (companyUsers.Any())
                        {
                            foreach (var user in companyUsers)
                            {
                                var eventsToNotify = await Context.DataContext.UserXNotificationSettings.Where(t1 => t1.UserId == user.Id).ToListAsync();
                                eventsToNotify.ForEach(t => t.IsEmail = !isBlocked);
                            }
                            await Context.CommitAsync();
                            transaction.Commit();

                            //Send response
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageUserStatusChangeSuccess;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "ToggleBlockUserNotificationsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteInvitedUser(UserContext userContext, int userId, List<string> ScheduleBuilderIds, bool IsScheduleExists)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var additionalInvitedUser = Context.DataContext.CompanyXAdditionalUserInvites.SingleOrDefault(t => t.Id == userId);
                    if (additionalInvitedUser != null)
                    {
                        var result = RemoveDriverFromUserTable(userContext, response, additionalInvitedUser);
                        if (result)
                        {
                            foreach (var job in additionalInvitedUser.Jobs)
                            {
                                job.CompanyXAdditionalUserInvites.Remove(additionalInvitedUser);
                            }

                            Context.DataContext.CompanyXAdditionalUserInvites.Remove(additionalInvitedUser);

                            //Delete the pending notification against this users
                            if (!additionalInvitedUser.IsInvitationSent)
                            {
                                var pendingNotifications = Context.DataContext.Notifications.Where
                                (
                                    t =>
                                    t.EntityId == userId &&
                                    (
                                        t.EventTypeId == (int)EventType.AdditionalUserAdded ||
                                        t.EventTypeId == (int)EventType.AdditionalUserUpdated
                                    )
                                );
                                Context.DataContext.Notifications.RemoveRange(pendingNotifications);
                            }

                            var driver = Context.DataContext.Users.FirstOrDefault(t => !t.IsActive && t.CompanyId == userContext.CompanyId
                                                       && !t.IsOnboardingComplete && t.UserName == additionalInvitedUser.Email);
                            if (driver != null && driver.MstRoles.Any(t => t.Id == (int)UserRoles.Driver))
                            {
                                var status = await RemoveInvitedDriver(userContext, driver.Id, ScheduleBuilderIds, IsScheduleExists);
                                if (status.StatusCode == (int)Status.Success)
                                {
                                    await Context.CommitAsync();
                                    transaction.Commit();
                                    response.StatusCode = Status.Success;
                                    response.StatusMessage = Resource.errMessageUserDeletionSuccess;
                                }
                                else
                                {
                                    transaction.Rollback();
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageCantDeleteAssignedDriver, additionalInvitedUser.FirstName, additionalInvitedUser.LastName);
                                }
                            }
                            else
                            {
                                await Context.CommitAsync();
                                transaction.Commit();
                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.errMessageUserDeletionSuccess;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "DeleteInvitedUser", ex.Message, ex);
                }
            }
            return response;
        }

        private static async Task<StatusViewModel> RemoveInvitedDriver(UserContext userContext, int userId, List<string> ScheduleBuilderIds, bool isScheduleExists)
        {
            RegionDriverRemoveModel driverRemoveModel = new RegionDriverRemoveModel();
            driverRemoveModel.DriverId = userId;
            driverRemoveModel.UserId = userContext.Id;
            driverRemoveModel.IsScheduleExists = isScheduleExists;
            driverRemoveModel.ScheduleBuilderIds = ScheduleBuilderIds;
            var driverresponse = await ContextFactory.Current.GetDomain<FreightServiceDomain>().DeleteInvitedDriverFromRegion(driverRemoveModel);
            return driverresponse;
        }

        public async Task<List<PrivateSupplierListViewModel>> GetPrivateSupplierListsAsync(int companyId)
        {
            List<PrivateSupplierListViewModel> response = new List<PrivateSupplierListViewModel>();
            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId && t.IsActive);
                if (company != null)
                {
                    var blacklistedCompanyIds = await GetBlacklistedCompanyIdsAsync(companyId);
                    response = company.PrivateSupplierLists.Where(t => t.IsActive).Select(t => new PrivateSupplierListViewModel(Status.Success)
                    {
                        Id = t.Id,
                        AddedByName = $"{t.User.FirstName} {t.User.LastName}",
                        CompanyId = t.CompanyId,
                        Name = t.Name,
                        Suppliers = t.Companies.Where(t1 => !blacklistedCompanyIds.Contains(t1.Id)).Select(t1 => t1.Id).ToList(),
                        CreatedDate = t.CreatedDate.ToString(Resource.constFormatDate),
                        SuppliersCount = t.Companies.Where(t1 => !blacklistedCompanyIds.Contains(t1.Id)).Select(t1 => t1.Id).Count(),
                        UpdatedDt = t.UpdatedDate.ToString(Resource.constFormatDate),
                        UpdatedByName = $"{t.User1.FirstName} {t.User1.LastName}",
                        IsAllowDelete = !t.FuelRequests.Any(t1 => t1.FuelRequestXStatuses.FirstOrDefault(t2 => t2.IsActive).StatusId == (int)FuelRequestStatus.Open)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetPrivateSupplierListsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<PrivateSupplierListsViewModel> GetPrivateSupplierListDetails(int privateSupplierListId = 0)
        {
            PrivateSupplierListsViewModel response = new PrivateSupplierListsViewModel(Status.Success);
            try
            {
                var privateSupplierList = await Context.DataContext.PrivateSupplierLists.SingleOrDefaultAsync(t => t.Id == privateSupplierListId);
                if (privateSupplierList != null)
                {
                    var blacklistedCompanyIds = await GetBlacklistedCompanyIdsAsync(privateSupplierList.CompanyId);
                    var privateSupplier = new PrivateSupplierListViewModel();

                    privateSupplier.Id = privateSupplierList.Id;
                    privateSupplier.Name = privateSupplierList.Name;
                    privateSupplier.IsNewSupplierList = true;
                    privateSupplier.DisplayMode = PageDisplayMode.Edit;
                    privateSupplier.Suppliers = privateSupplierList.Companies.Where(t => !blacklistedCompanyIds.Contains(t.Id)).Select(t => t.Id).ToList();

                    response.PrivateSupplierLists.Add(privateSupplier);
                    response.IsAllowDelete = !privateSupplierList.FuelRequests.Any(t1 => t1.FuelRequestXStatuses.FirstOrDefault(t2 => t2.IsActive).StatusId == (int)FuelRequestStatus.Open);
                }
                else
                {
                    response.PrivateSupplierLists.Add(new PrivateSupplierListViewModel(Status.Success) { IsNewSupplierList = true, DisplayMode = PageDisplayMode.Create });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetPrivateSupplierListDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<Response> AddPrivateSupplierListsAsync(PrivateSupplierListsViewModel viewModel)
        {
            Response response = new Response(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == viewModel.CompanyId && t.IsActive);
                    if (company != null)
                    {
                        List<string> names = viewModel.PrivateSupplierLists.Select(t => t.Name).ToList();
                        var existingNamesList = company.PrivateSupplierLists.Where(t => names.Contains(t.Name) && t.IsActive).Select(t => t.Name).ToList();
                        if (existingNamesList != null && existingNamesList.Count > 0)
                        {
                            string existingNames = string.Join(", ", existingNamesList);
                            response.StatusCode = Status.Failed;
                            response.StatusMessages.Add(ResourceMessages.GetMessage(Resource.valMessageAlreadyExist,
                                new[] { existingNamesList.Count == 1 ? $"{existingNames} is" : $"{existingNames} are" }));

                            return response;
                        }

                        viewModel.PrivateSupplierLists.ForEach(t => company.PrivateSupplierLists.Add(new PrivateSupplierList
                        {
                            AddedBy = viewModel.UserId,
                            Name = t.Name.Trim(),
                            Companies = Context.DataContext.Companies.Where(t1 => t.Suppliers.Contains(t1.Id)).ToList(),
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            UpdatedBy = viewModel.UserId,
                            IsActive = true
                        }));

                        await Context.CommitAsync();
                        transaction.Commit();

                        //Send response
                        response.StatusCode = Status.Success;
                        response.StatusMessages.Add(Resource.errMessagePrivateSupplierListAddSuccess);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "AddPrivateSupplierListsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> EditPrivateSupplierListAsync(PrivateSupplierListsViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == viewModel.CompanyId && t.IsActive);
                    if (company != null)
                    {
                        foreach (var privateSupplierList in viewModel.PrivateSupplierLists)
                        {
                            var supplierList = company.PrivateSupplierLists.SingleOrDefault(t => t.Id == privateSupplierList.Id);
                            if (supplierList != null)
                            {
                                var supplierCompanyList = Context.DataContext.Companies.Where(t => privateSupplierList.Suppliers.Contains(t.Id)).ToList();
                                supplierList.Companies.ToList().RemoveAll(t => t.Id > 0);
                                supplierList.Companies = supplierCompanyList;

                                supplierList.UpdatedDate = DateTime.Now;
                                supplierList.UpdatedBy = viewModel.UserId;
                            }
                        }
                    }

                    await Context.CommitAsync();

                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageEditPrivateSupplierListSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageEditPrivateSupplierListFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "EditPrivateSupplierListAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> DeletePrivateSupplierListAsync(int privateSupplierListId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var supplierList = await Context.DataContext.PrivateSupplierLists.FirstOrDefaultAsync(t => t.Id == privateSupplierListId);
                    if (supplierList != null)
                    {
                        supplierList.IsActive = false;
                        Context.DataContext.Entry(supplierList).State = EntityState.Modified;

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageDeletePrivateSupplierListSuccess;
                    }

                    await Context.CommitAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageDeletePrivateSupplierListFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "DeletePrivateSupplierListAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<Response> InviteCompaniesAsync(AdditionalUsersViewModel viewModel)
        {
            Response response = new Response(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == viewModel.UserId && t.IsActive);
                    if (user != null)
                    {
                        List<string> email = viewModel.AdditionalUsers.Select(t => t.Email).ToList();
                        Context.DataContext.Users.Where(t => email.Contains(t.Email)).ToList()
                            .ForEach(t => response.StatusMessages.Add(ResourceMessages.GetMessage(Resource.errMessageUserAlreadyOnboardedAgainstCompany,
                                    new[] { t.Email, t.Company.Name })));

                        Context.DataContext.UserXInvites.Where(t => email.Contains(t.Email) && t.IsOnboarded).ToList()
                            .ForEach(t => response.StatusMessages.Add(
                               ResourceMessages.GetMessage(Resource.errMessageUserAlreadyOnboarded,
                                    new[] { t.Email })));

                        if (response.StatusMessages.Count == 0)
                        {
                            foreach (var additionalUser in viewModel.AdditionalUsers)
                            {
                                UserXInvite userXInvite;
                                if (Context.DataContext.UserXInvites.Any(t => t.Email == additionalUser.Email && t.InvitedBy == viewModel.UserId))
                                {
                                    userXInvite = Context.DataContext
                                                    .UserXInvites
                                                    .FirstOrDefault(
                                                        t =>
                                                        t.Email == additionalUser.Email &&
                                                        t.InvitedBy == viewModel.UserId);
                                    userXInvite.IsInvitationSent = false;
                                    Context.DataContext.Entry(userXInvite).State = EntityState.Modified;
                                }
                                else
                                {
                                    userXInvite = new UserXInvite
                                    {
                                        FirstName = additionalUser.FirstName,
                                        LastName = additionalUser.LastName,
                                        Email = additionalUser.Email,
                                        InvitedBy = viewModel.UserId,
                                        IsInvitationSent = false,
                                        IsOnboarded = false
                                    };
                                    userXInvite = Context.DataContext.UserXInvites.Add(userXInvite);
                                }

                                await Context.CommitAsync();

                                //Add an entry to notifications
                                await ContextFactory.Current.GetDomain<NotificationDomain>()
                                                            .AddNotificationEventAsync(
                                                                EventType.ExternalCompanyInvited,
                                                                userXInvite.Id,
                                                                user.Id);

                                response.StatusCode = Status.Success;
                                response.StatusMessages.Add(Resource.errMessageInviteCompanySuccess);
                            }
                            transaction.Commit();

                            //Send response
                            return response;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "InviteCompaniesAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<AdditionalUserViewModel>> GetInvitedCompaniesListAsync(int userId)
        {
            List<AdditionalUserViewModel> response = new List<AdditionalUserViewModel>();
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.IsActive && t.Id == userId);
                if (user != null)
                {
                    response = user.UserXInvites.Select(t => new AdditionalUserViewModel(Status.Success)
                    {
                        Id = t.Id,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Email = t.Email,
                        InvitedBy = $"{t.User.FirstName} {t.User.LastName}",
                        IsOnboarded = t.IsOnboarded,
                        IsInvitationSent = t.IsInvitationSent
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetInvitedCompaniesListAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<AdditionalUserViewModel> GetInvitedCompanyDetailsAsync(int id)
        {
            AdditionalUserViewModel response = new AdditionalUserViewModel();
            try
            {
                var invitedUser = await Context.DataContext.UserXInvites.SingleOrDefaultAsync(t => t.Id == id);
                if (invitedUser != null)
                {
                    response = new AdditionalUserViewModel(Status.Success)
                    {
                        Id = invitedUser.Id,
                        FirstName = invitedUser.FirstName,
                        LastName = invitedUser.LastName,
                        Email = invitedUser.Email,
                        InvitedBy = $"{invitedUser.User.FirstName} {invitedUser.User.LastName}",
                        IsOnboarded = invitedUser.IsOnboarded,
                        IsInvitationSent = invitedUser.IsInvitationSent,
                        DisplayMode = PageDisplayMode.Edit
                    };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetInvitedCompaniesListAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> EditInvitedCompanyAsync(AdditionalUserViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var userXInvite = await Context.DataContext.UserXInvites.SingleOrDefaultAsync(t => t.Id == viewModel.Id);
                    if (userXInvite != null)
                    {
                        if (!userXInvite.IsOnboarded)
                        {
                            userXInvite.FirstName = viewModel.FirstName;
                            userXInvite.LastName = viewModel.LastName;
                            userXInvite.Email = viewModel.Email;

                            Context.DataContext.Entry(userXInvite).State = EntityState.Modified;
                            await Context.CommitAsync();

                            //Add an entry to notifications
                            await ContextFactory.Current.GetDomain<NotificationDomain>()
                                                        .AddNotificationEventAsync(
                                                            EventType.ExternalCompanyInviteUpdated,
                                                            userXInvite.Id,
                                                            userXInvite.InvitedBy);
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageUserAlreadyOnboarded, new object[] { userXInvite.Email });
                        }
                    }

                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageEditCompanySuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageEditCompanyFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "EditInvitedCompanyAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<CreditAppViewModel> GetCreditAppDetailsAsync(int companyId)
        {
            CreditAppViewModel response = new CreditAppViewModel();
            try
            {
                var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId);
                if (company != null)
                {
                    var creditAppDetails = Context.DataContext.CreditAppDetails.Where(t => t.CompanyId == companyId).FirstOrDefault();
                    if (creditAppDetails != null)
                    {
                        response = creditAppDetails.ToViewModel();
                    }
                    else
                    {
                        var user = company.Users.FirstOrDefault();
                        response.CompanyId = companyId;
                        response.IsEnabled = company.IsCreditAppEnabled;
                        response.Body = ResourceMessages.GetMessage(Resource.txtCreditAppEmailBody, new[] { user.Email, $"{user.FirstName} {user.LastName}" });
                        response.Body = response.Body.Replace("</br>", Environment.NewLine);
                        response.Subject = ResourceMessages.GetMessage(Resource.emailCreditApp_Buyer_SubjectText, new[] { company.Name });
                    }
                }

                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCreditAppDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveCreditAppDetailsAsync(CreditAppViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var creditApp = await Context.DataContext.CreditAppDetails.SingleOrDefaultAsync(t => t.Id == viewModel.Id);
                    if (creditApp != null)
                    {
                        creditApp = viewModel.ToEntity(creditApp);

                        Context.DataContext.Entry(creditApp).State = EntityState.Modified;
                        await Context.CommitAsync();

                    }
                    else
                    {
                        creditApp = viewModel.ToEntity();
                        Context.DataContext.CreditAppDetails.Add(creditApp);
                        var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == viewModel.CompanyId);
                        company.IsCreditAppEnabled = viewModel.IsEnabled;
                        Context.DataContext.Entry(company).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }

                    transaction.Commit();

                    viewModel.Id = creditApp.Id;
                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSaveCreditAppDetailsSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageSaveCreditAppDetailsFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "SaveCreditAppDetailsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<DocumentViewModel>> GetCreditAppMaterialAsync(int companyId)
        {
            List<DocumentViewModel> response = new List<DocumentViewModel>();
            try
            {
                var documents = await Context.DataContext.CreditAppDocuments.Include(t => t.User)
                                                                                .Where(t => t.CompanyId == companyId)
                                                                                .OrderByDescending(t => t.Id)
                                                                                .ToListAsync();
                documents.ForEach(t => response.Add(t.ToViewModel()));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCreditAppMaterial", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveCreditAppDocumentsAsync(List<DocumentViewModel> files, int companyId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var documents = await Context.DataContext.CreditAppDocuments
                                                                    .Where(t => t.CompanyId == companyId)
                                                                    .ToListAsync();
                    foreach (var file in files)
                    {
                        var document = documents.SingleOrDefault(t => t.FileName.ToLower() == file.FileName.ToLower());
                        if (document != null)
                        {
                            document.FileName = file.FileName;
                            document.ModifiedFileName = file.ModifiedFileName;
                            document.AddedBy = file.AddedBy;
                            document.FilePath = file.FilePath;
                            Context.DataContext.Entry(document).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }
                        else
                        {
                            Context.DataContext.CreditAppDocuments.Add(file.ToEntity());
                            await Context.CommitAsync();
                        }
                    }

                    if (files.Count > 0)
                    {
                        var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId);
                        if (company != null && !company.IsCreditAppEnabled)
                        {
                            company.IsCreditAppEnabled = true;
                            Context.DataContext.Entry(company).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }
                    }

                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageUploadCreditAppDocumentsSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageUploadCreditAppDocumentsFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "SaveCreditAppDetailsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<DocumentViewModel> DeleteCreditAppDocumentAsync(int id, int companyId)
        {
            DocumentViewModel response = new DocumentViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var creditAppDocument = Context.DataContext.CreditAppDocuments.Include(t => t.Company).SingleOrDefault(t => t.Id == id);
                    if (creditAppDocument != null)
                    {
                        Context.DataContext.CreditAppDocuments.Remove(creditAppDocument);
                        await Context.CommitAsync();

                        var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId);
                        if (company != null && company.CreditAppDocuments.Count == 0)
                        {
                            company.IsCreditAppEnabled = false;
                            Context.DataContext.Entry(company).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }
                    }

                    transaction.Commit();

                    //Send response
                    response = creditAppDocument.ToViewModel();
                    response.StatusMessage = Resource.errMessageDeleteCreditAppDocumentSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageDeleteCreditAppDocumentFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "DeleteCreditAppDocumentAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<DocumentViewModel> GetCreditAppDocumentDetailsAsync(int id)
        {
            DocumentViewModel response = new DocumentViewModel();
            try
            {
                var document = await Context.DataContext.CreditAppDocuments.Include(t => t.User).SingleOrDefaultAsync(t => t.Id == id);

                if (document != null)
                {
                    response = document.ToViewModel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCreditAppDocumentDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<string>> GetFilePathDetailsAsync(List<string> documents, int companyId)
        {
            List<string> response = new List<string>();
            try
            {
                response = await Context.DataContext.CreditAppDocuments
                                                                    .Where(t => t.CompanyId == companyId &&
                                                                                documents.Any(t1 => t1.ToLower() == t.FileName.ToLower()))
                                                                    .Select(t => t.FilePath)
                                                                    .ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetFilePathDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetBlacklistedCompanyIdsAsync(int companyId, List<int> groupIdslist = null)
        {
            var response = new List<int>();
            try
            {
                if (groupIdslist == null)
                    groupIdslist = new List<int>();

                var company = await Context.DataContext.Companies.Include(t => t.CompanyBlacklists).FirstOrDefaultAsync(t => t.Id == companyId || (groupIdslist.Count > 0 && t.SubCompanies.Any(t1 => t1.SubCompanyId == t.Id && groupIdslist.Contains(t1.CompanyGroupId))));
                if (company != null)
                {
                    response = company.CompanyBlacklists.Where(t => t.RemovedBy == null).Select(t => t.CompanyId).ToList();
                }

                // Supplier who blacklisted buyers, so get all these suppliers
                var suppliers = await Context.DataContext.CompanyBlacklists.Where(t => (t.CompanyId == companyId || (groupIdslist.Count > 0 && t.Company.SubCompanies.Any(t1 => t1.SubCompanyId == t.Id && groupIdslist.Contains(t1.CompanyGroupId)))) && t.RemovedBy == null).Select(t => t.AddedByCompanyId).ToListAsync();
                response.AddRange(suppliers);
                response = response.Distinct().ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetBlacklistCompanyIdsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<CompanyBlacklistGridViewModel> GetCompanyBlacklistAsync(int companyId, TimeSpan timeZoneOffset)
        {
            var response = new CompanyBlacklistGridViewModel();
            try
            {
                var company = await Context.DataContext.Companies.Include(t => t.CompanyBlacklists).FirstOrDefaultAsync(t => t.Id == companyId);
                if (company != null)
                {
                    var companyTypes = new List<int>() { (int)CompanyType.Buyer, (int)CompanyType.Supplier, (int)CompanyType.BuyerAndSupplier };
                    if (company.CompanyTypeId == (int)CompanyType.Buyer)
                    {
                        companyTypes = companyTypes.Where(t => t != company.CompanyTypeId).Select(t => t).ToList();
                    }
                    var existingCompanyIds = company.CompanyBlacklists.Where(t => t.RemovedBy == null).Select(t => t.CompanyId).ToList();
                    var blackListCompanies = company.CompanyBlacklists.OrderByDescending(t => t.Id).Select(t => t.ToViewModel()).ToList();

                    foreach (var item in blackListCompanies)
                    {
                        item.AddedDate = item.AddedDate.ToBrowserDateTime(timeZoneOffset);
                        if (item.RemovedDate.HasValue)
                            item.RemovedDate = item.RemovedDate.Value.ToBrowserDateTime(timeZoneOffset);
                    }
                    response.BlacklistCompanies = blackListCompanies;
                    response.SelectCompanyList = Context.DataContext.Companies.Where(t => t.Id != companyId &&
                    companyTypes.Contains(t.CompanyTypeId) && !existingCompanyIds.Contains(t.Id))
                    .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name }).ToList();

                    var blacklistedCompanyIds = await GetBlacklistedCompanyIdsAsync(companyId);
                    response.SelectCompanyList = response.SelectCompanyList.Where(t => !blacklistedCompanyIds.Contains(t.Id)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCompanyBlacklistAsync", ex.Message, ex);
            }
            return response;

        }

        public async Task<StatusViewModel> AddCompanyToBlacklistAsync(int userId, int companyId, int blacklistCompanyId, string reason)
        {
            var response = new StatusViewModel() { StatusMessage = Resource.errMessageAddToBlacklistFailed };
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var company = await Context.DataContext.Companies.FirstOrDefaultAsync(t => t.Id == companyId);
                    if (company != null)
                    {
                        var blacklist = new CompanyBlacklist
                        {
                            CompanyId = blacklistCompanyId,
                            AddedBy = userId,
                            AddedDate = DateTimeOffset.Now,
                            Reason = reason
                        };
                        company.CompanyBlacklists.Add(blacklist);
                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageAddToBlacklistSuccess;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "AddCompanyToBlacklistAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> RemoveCompanyFromBlacklistAsync(int userId, int companyId, int blacklistCompanyId)
        {
            var response = new StatusViewModel() { StatusMessage = Resource.errMessageRemoveFromBlacklistFailed };
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var company = await Context.DataContext.Companies.Include(t => t.CompanyBlacklists).FirstOrDefaultAsync(t => t.Id == companyId);
                    if (company != null)
                    {
                        var blacklist = company.CompanyBlacklists.FirstOrDefault(t => t.CompanyId == blacklistCompanyId && t.RemovedBy == null);
                        if (blacklist != null)
                        {
                            blacklist.RemovedBy = userId;
                            blacklist.RemovedDate = DateTimeOffset.Now;
                            Context.DataContext.Entry(blacklist).State = EntityState.Modified;

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageRemoveFromBlacklistSuccess;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "AddCompanyToBlacklistAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<TimeCardSettingViewModel> GetTimeCardSettings(int companyId)
        {
            var response = new TimeCardSettingViewModel(Status.Success);
            try
            {
                var companyDetails = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId);
                if (companyDetails != null)
                {
                    response.CompanyId = companyId;
                    response.IsTimeCardEnabled = companyDetails.IsTimeCardEnabled;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetTimeCardSettings", ex.Message, ex);
            }
            return response;
        }

        public async Task<TimeCardSettingViewModel> UpdateTimeCardSettings(int userId, int companyId, bool setTimeCard)
        {
            var response = new TimeCardSettingViewModel(Status.Success);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var companyDetails = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId);
                    if (companyDetails != null)
                    {
                        companyDetails.IsTimeCardEnabled = setTimeCard;
                        companyDetails.UpdatedBy = userId;
                        companyDetails.UpdatedDate = DateTimeOffset.Now;

                        Context.DataContext.Entry(companyDetails).State = EntityState.Modified;

                        await Context.CommitAsync();
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "UpdateTimeCardSettings", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<FavoriteFuelViewModel> GetOtherFuelListAsync(int companyId)
        {
            var response = new FavoriteFuelViewModel();
            try
            {
                var favoriteFuels = Context.DataContext.CompanyFavoriteFuels.Include(t => t.MstTfxProduct)
                    .Where(t => t.CompanyId == companyId && t.RemovedBy == null).Select(t => t.MstTfxProduct.Id);

                response.FuelTypeList = await Context.DataContext.MstTfxProducts.Where(t => !favoriteFuels.Contains(t.Id) &&
                                                                                        (t.ProductDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType))
                                                                                        .Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name }).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetOtherFuelListAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<FavoriteFuelGridViewModel>> GetFavoriteFuelListAsync(int companyId)
        {
            var response = new List<FavoriteFuelGridViewModel>();
            try
            {
                var favoriteFuels = await Context.DataContext.CompanyFavoriteFuels.Include(t => t.MstTfxProduct)
                    .Include(t => t.User).Include(t => t.User1).Where(t => t.CompanyId == companyId && t.RemovedBy == null)
                    .OrderByDescending(t => t.Id).ToListAsync();

                response = favoriteFuels.Select(t => t.ToGridViewModel(new FavoriteFuelGridViewModel())).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetFavoriteFuelListAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<FavoriteFuelGridViewModel>> GetFavoriteFuelHistoryAsync(int companyId)
        {
            var response = new List<FavoriteFuelGridViewModel>();
            try
            {
                var favoriteFuels = await Context.DataContext.CompanyFavoriteFuels.Include(t => t.MstProduct)
                    .Include(t => t.User).Include(t => t.User1).Where(t => t.CompanyId == companyId && t.RemovedBy != null)
                    .OrderByDescending(t => t.RemovedDate).ToListAsync();

                response = favoriteFuels.Select(t => t.ToGridViewModel(new FavoriteFuelGridViewModel())).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetFavoriteFuelHistoryAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveFavoriteFuelsAsync(int companyId, int userId, FavoriteFuelViewModel favoriteFuels)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var companyFavorites = Context.DataContext.CompanyFavoriteFuels.Where(t => t.CompanyId == companyId
                                            && t.RemovedBy == null).Select(t => t.FuelTypeId).ToList();
                    foreach (var fuelId in favoriteFuels.SelectedFuelTypes)
                    {
                        var favoriteFuel = new CompanyFavoriteFuel
                        {
                            FuelTypeId = 1,
                            TfxFuelTypeId = fuelId,
                            CompanyId = companyId,
                            AddedBy = userId,
                            AddedDate = DateTimeOffset.Now
                        };
                        if (!companyFavorites.Any(t => t == fuelId))
                        {
                            Context.DataContext.CompanyFavoriteFuels.Add(favoriteFuel);
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageFavoriteFuelAddedSuccess;
                        }
                    }
                    await Context.CommitAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageFavoriteFuelAddedFailed;
                    LogManager.Logger.WriteException("SettingsDomain", "SaveFavoriteFuelsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> RemoveFavoriteFuelAsync(int companyId, int userId, int favoriteId)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var favoriteFuel = await Context.DataContext.CompanyFavoriteFuels.FirstOrDefaultAsync(t => t.Id == favoriteId);
                    if (favoriteFuel != null)
                    {
                        favoriteFuel.RemovedBy = userId;
                        favoriteFuel.RemovedDate = DateTimeOffset.Now;

                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageFavoriteFuelRemovedSuccess;
                    }
                    else
                    {
                        response.StatusMessage = Resource.errMessageFavoriteFuelRemovedFailed;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusMessage = Resource.errMessageFavoriteFuelRemovedFailed;
                    LogManager.Logger.WriteException("SettingsDomain", "RemoveFavoriteFuelAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<CompanyExternalIdGridViewModel>> GetExternalCompanyIds(UserContext userContext)
        {
            var response = new List<CompanyExternalIdGridViewModel>();
            try
            {
                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                response = await storedProcedureDomain.GetExternalCompanyIdentifications(userContext.CompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetExternalCompanyIds", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveExternalCompanyId(UserContext userContext, CompanyExternalIdViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var externalIdentification = await Context.DataContext.CompanyExternalIdentifications.FirstOrDefaultAsync(t => t.Id == viewModel.Id
                                            && t.IsActive && t.IdentifyingCompanyId == userContext.CompanyId && t.IdentifiedCompanyId == viewModel.CompanyId);
                if (externalIdentification == null)
                {
                    var identification = new CompanyExternalIdentification()
                    {
                        IdentifyingCompanyId = userContext.CompanyId,
                        IdentifiedCompanyId = viewModel.CompanyId,
                        ExternalId = viewModel.ExternalId,
                        AddedBy = userContext.Id,
                        AddedDate = DateTimeOffset.Now,
                        UpdatedBy = userContext.Id,
                        IsActive = true
                    };
                    Context.DataContext.CompanyExternalIdentifications.Add(identification);
                    response.StatusMessage = Resource.successMessageAddedExternalId;
                }
                else
                {
                    externalIdentification.ExternalId = viewModel.ExternalId;
                    externalIdentification.UpdatedBy = userContext.Id;
                    externalIdentification.UpdatedDate = DateTimeOffset.Now;
                    response.StatusMessage = Resource.successMessageUpdatedExternalId;
                }
                await Context.CommitAsync();
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageFailedToSaveExternalId;
                LogManager.Logger.WriteException("SettingsDomain", "SaveExternalComapnyId", ex.Message, ex);
            }
            return response;
        }

        public StatusViewModel ValidateLocationBulkUploadCsv(string csvText, string csvFilePath)
        {
            using (var tracer = new Tracer("SettingsDomain", "ValidateLocationBulkUploadCsv"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    var csvHeaderLine = Regex.Matches(csvText.Trim(), @"^.*Address.*\n").Cast<Match>().FirstOrDefault();
                    string[] lines = File.ReadAllLines(csvFilePath);
                    string headerLine = lines.FirstOrDefault();
                    if (csvHeaderLine.Value.Trim() == headerLine)
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageBulkUploadHeaderMismatch;
                    }

                    csvText = Regex.Replace(csvText.Trim(), @"^.*Address.*\n", string.Empty, RegexOptions.IgnoreCase);
                    csvText = Regex.Replace(csvText.Trim(), @",,,,,,,,,,,,,,,,,,,,,,,,,,\n", string.Empty, RegexOptions.IgnoreCase);
                    var indexOfGuideline = csvText.IndexOf("SFX Guidelines");
                    if (indexOfGuideline > 0)
                    {
                        csvText = csvText.Substring(0, indexOfGuideline);
                    }

                    var engine = new FileHelperEngine<LocationsCsvRecordViewModel>();
                    var csvLocationList = engine.ReadString(csvText).ToList();
                    csvLocationList = csvLocationList.Where(t => !string.IsNullOrWhiteSpace(t.Address) && t.Address != "Address").ToList();
                    var zipcodeRegEx = @"^\d{5}(-\d{4})?$)|(^[A-Z]{1}\d{1}[A-Z]{1} *\d{1}[A-Z]{1}\d{1}$";

                    if (csvLocationList.Count - 1 > 0)
                    {

                        var allStates = Context.DataContext.MstStates.Where(t => t.IsActive).ToList();
                        var allFuelTypes = Context.DataContext.MstProductTypes.Where(t => t.IsActive).Select(t => t.Name.ToLower().Trim()).OrderBy(t => t).ToList();
                        var allQualifications = Context.DataContext.MstSupplierQualifications.Where(t => t.IsActive).Select(t => t.Name.ToLower().Trim()).ToList();

                        int lineNumberOfCSV = 1;
                        foreach (var record in csvLocationList)
                        {
                            lineNumberOfCSV++;

                            //Required field validation
                            if (IsRequiredFieldMissing(record))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageBulkUploadRequiredFieldsAreMissing, lineNumberOfCSV);
                                return response;
                            }

                            //validate State
                            if (!allStates.Select(t => t.Code.ToLower().Trim()).Contains(record.State.ToLower().Trim()))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInvalidStateCode, record.State);
                                return response;
                            }

                            //validate Zip
                            if (!Regex.Match(record.ZipCode.ToLower().Trim(), zipcodeRegEx).Success)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInvalidSpecificZipCode, record.ZipCode);
                                return response;
                            }

                            //validate phone number
                            if (!Regex.IsMatch((record.PhoneNo.Trim().Replace(" ", "").Replace("-", "")), @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInvalidPhoneNumber, record.PhoneNo);
                                return response;
                            }

                            //validate fuel type
                            if (record.FuelType.ToLower().Trim() != "all")
                            {
                                var fuelTypes = record.FuelType.ToLower().Trim().Split(',').ToList();
                                foreach (var item in fuelTypes)
                                {
                                    if (!allFuelTypes.Contains(item))
                                    {
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = string.Format(Resource.errMessageInvalidFuelType, item);
                                        return response;
                                    }
                                }
                            }

                            //validate serving states (States where you deliver) 
                            if (record.States.ToLower().Trim() != "all")
                            {
                                var states = record.States.ToLower().Trim().Split(',').ToList();
                                foreach (var item in states)
                                {
                                    if (!allStates.Select(t => t.Name.ToLower().Trim()).Contains(item))
                                    {
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = string.Format(Resource.errMessageInvalidServingState, item);
                                        return response;
                                    }
                                }
                            }

                            //validate Supplier Qualifications (DBE)
                            if (!string.IsNullOrWhiteSpace(record.DBE) && record.DBE.ToLower().Trim() != "all")
                            {
                                var qualifications = record.DBE.ToLower().Trim().Split(',').ToList();
                                foreach (var item in qualifications)
                                {
                                    if (!allQualifications.Contains(item))
                                    {
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = string.Format(Resource.errMessageInvalidDBE, item);
                                        return response;
                                    }
                                }
                            }
                        }
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageForLocationBulkUpload;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageCanNotProcessZeroRecords;
                    }

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("SettingsDomain", "ValidateLocationBulkUploadCsv", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<StatusViewModel> SaveBulkLocationsAsync(string csvText, int userId, int companyId = 0)
        {
            using (var tracer = new Tracer("SettingsDomain", "SaveBulkLocationsAsync"))
            {
                var response = new StatusViewModel();
                try
                {
                    LogManager.Logger.WriteInfo("SettingsDomain", "SaveBulkLocationsAsync", "\n\n[" + csvText + "]\n\n");
                    csvText = Regex.Replace(csvText.Trim(), @"^.*Address.*\n", string.Empty, RegexOptions.IgnoreCase);
                    csvText = Regex.Replace(csvText.Trim(), @",,,,,,,,,,,,,,,,,,,,,,,,,,\n", string.Empty, RegexOptions.IgnoreCase);
                    var indexOfGuideline = csvText.IndexOf("SFX Guidelines");
                    if (indexOfGuideline > 0)
                    {
                        csvText = csvText.Substring(0, indexOfGuideline);
                    }

                    var engine = new FileHelperEngine<LocationsCsvRecordViewModel>();
                    var csvLocationList = engine.ReadString(csvText).ToList();
                    csvLocationList = csvLocationList.Where(t => !string.IsNullOrWhiteSpace(t.Address) && t.Address != "Address").ToList();

                    int row = 0;
                    var similarAddressList = new List<int>();
                    var invalidAddressList = new List<int>();
                    foreach (var item in csvLocationList)
                    {
                        row = row + 1;
                        var status = await SaveLocationAsync(item, userId, companyId);
                        if (status.StatusCode == Status.Failed)
                        {
                            if (status.StatusMessage == Resource.errMessageSimilarCompanyAddressPresent)
                                similarAddressList.Add(row);
                            else if (status.StatusMessage == Resource.errMsgInvalidCombinationOfAddress)
                                invalidAddressList.Add(row);
                        }
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageForLocationBulkUpload;
                    if (similarAddressList.Count > 0 && csvLocationList.Count == similarAddressList.Count)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageSimilarCompanyAddressPresent;
                    }
                    else if (similarAddressList.Count > 0 && csvLocationList.Count != similarAddressList.Count)
                    {
                        if (invalidAddressList.Any())
                        {
                            response.StatusMessage = "For rows " + string.Join(",", similarAddressList) + " " + Resource.errMessageSimilarCompanyAddressPresent.ToLower() + "<br/>" +
                                "For rows " + string.Join(",", invalidAddressList) + " " + Resource.errMsgInvalidCombinationOfAddress.ToLower() + "<br/>" +
                                "Other " + Resource.successMessageForLocationBulkUpload.ToLower();
                        }
                        else
                        {
                            response.StatusMessage = "For rows " + string.Join(",", similarAddressList) + " " + Resource.errMessageSimilarCompanyAddressPresent.ToLower() + "<br/> Other " + Resource.successMessageForLocationBulkUpload.ToLower();
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("SettingsDomain", "SaveBulkLocationsAsync", ex.Message, ex);
                }

                return response;
            }
        }

        private bool IsRequiredFieldMissing(LocationsCsvRecordViewModel record) => string.IsNullOrWhiteSpace(record.Address) || string.IsNullOrWhiteSpace(record.City)
               || string.IsNullOrWhiteSpace(record.State) || string.IsNullOrWhiteSpace(record.ZipCode) || string.IsNullOrWhiteSpace(record.PhoneNo) || string.IsNullOrWhiteSpace(record.FuelType);

        private async Task<StatusViewModel> SaveLocationAsync(LocationsCsvRecordViewModel locationsCsvRecordViewModel, int userId, int companyId)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            CompanyAddress companyAddress = new CompanyAddress();
            companyAddress.Address = locationsCsvRecordViewModel.Address;
            companyAddress.City = locationsCsvRecordViewModel.City;
            companyAddress.ZipCode = locationsCsvRecordViewModel.ZipCode;
            companyAddress.PhoneTypeId = 1;
            companyAddress.PhoneNumber = locationsCsvRecordViewModel.PhoneNo;
            companyAddress.CompanyId = companyId;
            companyAddress.CountryId = 1;
            companyAddress.IsActive = true;
            companyAddress.UpdatedDate = DateTimeOffset.Now;
            companyAddress.UpdatedBy = userId;

            var state = Context.DataContext.MstStates.FirstOrDefault(t => t.Code.ToLower() == locationsCsvRecordViewModel.State.ToLower());
            if (state != null)
            {
                companyAddress.StateId = state.Id;
                var point = GoogleApiDomain.GetGeocode($"{companyAddress.Address} {companyAddress.City} {state.Code} {"USA"} {companyAddress.ZipCode}");
                if (point != null)
                {
                    companyAddress.Latitude = Convert.ToDecimal(point.Latitude);
                    companyAddress.Longitude = Convert.ToDecimal(point.Longitude);
                }
                else
                {
                    response.StatusMessage = Resource.errMsgInvalidCombinationOfAddress;
                    return response;
                }

                var existingCompanyAddress = Context.DataContext.CompanyAddresses.Any(t => t.CompanyId == companyId && t.ZipCode == locationsCsvRecordViewModel.ZipCode &&
                                                                                      t.Address == locationsCsvRecordViewModel.Address && t.City == locationsCsvRecordViewModel.City &&
                                                                                      t.StateId == state.Id && t.IsActive);
                if (existingCompanyAddress)
                {
                    response.StatusMessage = Resource.errMessageSimilarCompanyAddressPresent;
                    return response;
                }
            }

            Context.DataContext.CompanyAddresses.Add(companyAddress);
            await Context.CommitAsync();

            var supplierAddressXSetting = new SupplierAddressXSetting();
            supplierAddressXSetting.AddressId = companyAddress.Id;
            supplierAddressXSetting.IsHedgeOrderAllowed = !(string.IsNullOrWhiteSpace(locationsCsvRecordViewModel.HedgeOrders)) && locationsCsvRecordViewModel.HedgeOrders.ToLower() == "no" ? false : true;
            supplierAddressXSetting.IsOverWaterRefuelingAllowed = !(string.IsNullOrWhiteSpace(locationsCsvRecordViewModel.WaterRefueling)) && locationsCsvRecordViewModel.WaterRefueling == "no" ? false : true;
            if (string.IsNullOrWhiteSpace(locationsCsvRecordViewModel.SpecificRadius))
            {
                supplierAddressXSetting.IsStateWideService = true;
            }
            else
            {
                supplierAddressXSetting.Radius = Convert.ToInt32(locationsCsvRecordViewModel.SpecificRadius);
            }

            var supplierAddressXQualifications = new List<MstSupplierQualification>();

            if (!(string.IsNullOrWhiteSpace(locationsCsvRecordViewModel.DBE)) && locationsCsvRecordViewModel.DBE.ToLower() == "all")
            {
                supplierAddressXQualifications = Context.DataContext.MstSupplierQualifications.ToList();
            }
            else if (!(string.IsNullOrWhiteSpace(locationsCsvRecordViewModel.DBE)))
            {
                var dbeArray = locationsCsvRecordViewModel.DBE.ToLower().Split(',');
                supplierAddressXQualifications = Context.DataContext.MstSupplierQualifications.Where(t => dbeArray.Contains(t.Name.ToLower())).ToList();
            }
            companyAddress.MstSupplierQualifications = supplierAddressXQualifications;

            var supplierAddressXServingStates = new List<MstState>();
            if (!(string.IsNullOrWhiteSpace(locationsCsvRecordViewModel.States)) && locationsCsvRecordViewModel.States.ToLower() == "all")
            {
                supplierAddressXServingStates = Context.DataContext.MstStates.ToList();
            }
            else if (!(string.IsNullOrWhiteSpace(locationsCsvRecordViewModel.States)))
            {
                var statesArray = locationsCsvRecordViewModel.States.ToLower().Split(',');
                supplierAddressXServingStates = Context.DataContext.MstStates.Where(t => statesArray.Contains(t.Name.ToLower())).ToList();
            }
            companyAddress.MstStates = supplierAddressXServingStates;

            var supplierAddressXProductTypes = new List<MstProductType>();
            if (!(string.IsNullOrWhiteSpace(locationsCsvRecordViewModel.FuelType)) && locationsCsvRecordViewModel.FuelType.ToLower() == "all")
            {
                supplierAddressXProductTypes = Context.DataContext.MstProductTypes.ToList();
            }
            else if (!(string.IsNullOrWhiteSpace(locationsCsvRecordViewModel.FuelType)))
            {
                var fuelTypesArray = locationsCsvRecordViewModel.FuelType.ToLower().Split(',');
                supplierAddressXProductTypes = Context.DataContext.MstProductTypes.Where(t => fuelTypesArray.Contains(t.Name.ToLower())).ToList();
            }
            companyAddress.MstProductTypes = supplierAddressXProductTypes;

            companyAddress.SupplierAddressXWorkingHours.Add(GetWorkingHourDaywise(companyAddress.Id, WeekDay.Monday, locationsCsvRecordViewModel.MondayStartTime, locationsCsvRecordViewModel.MondayEndTime));
            companyAddress.SupplierAddressXWorkingHours.Add(GetWorkingHourDaywise(companyAddress.Id, WeekDay.Tuesday, locationsCsvRecordViewModel.TuesdayStartTime, locationsCsvRecordViewModel.TuesdayEndTime));
            companyAddress.SupplierAddressXWorkingHours.Add(GetWorkingHourDaywise(companyAddress.Id, WeekDay.Wednesday, locationsCsvRecordViewModel.WednesdayStartTime, locationsCsvRecordViewModel.WednesdayEndTime));
            companyAddress.SupplierAddressXWorkingHours.Add(GetWorkingHourDaywise(companyAddress.Id, WeekDay.Thursday, locationsCsvRecordViewModel.ThursdayStartTime, locationsCsvRecordViewModel.ThursdayEndTime));
            companyAddress.SupplierAddressXWorkingHours.Add(GetWorkingHourDaywise(companyAddress.Id, WeekDay.Friday, locationsCsvRecordViewModel.FridayStartTime, locationsCsvRecordViewModel.FridayEndTime));
            companyAddress.SupplierAddressXWorkingHours.Add(GetWorkingHourDaywise(companyAddress.Id, WeekDay.Saturday, locationsCsvRecordViewModel.SaturdayStartTime, locationsCsvRecordViewModel.SaturdayEndTime));
            companyAddress.SupplierAddressXWorkingHours.Add(GetWorkingHourDaywise(companyAddress.Id, WeekDay.Sunday, locationsCsvRecordViewModel.SundayStartTime, locationsCsvRecordViewModel.SundayEndTime));

            await Context.CommitAsync();

            response.StatusCode = Status.Success;
            return response;
        }

        private SupplierAddressXWorkingHour GetWorkingHourDaywise(int addressId, WeekDay weekDay, string startTime, string endTime)
        {
            return new SupplierAddressXWorkingHour()
            {
                AddressId = addressId,
                WeekDayId = (int)weekDay,
                StartTime = string.IsNullOrWhiteSpace(startTime) ? new TimeSpan(0, 0, 0) : Convert.ToDateTime(startTime).TimeOfDay,
                EndTime = string.IsNullOrWhiteSpace(endTime) ? new TimeSpan(0, 0, 0) : Convert.ToDateTime(endTime).TimeOfDay
            };
        }

        private bool RemoveDriverFromUserTable(UserContext userContext, StatusViewModel response, CompanyXAdditionalUserInvite additionalInvitedUser)
        {
            bool shouldCommitTransaction = true;
            if (additionalInvitedUser.MstRoles.Any(t => t.Id == (int)UserRoles.Driver))
            {
                var driver = Context.DataContext.Users.FirstOrDefault(t => !t.IsActive && t.CompanyId == userContext.CompanyId
                                                        && !t.IsOnboardingComplete && t.Email == additionalInvitedUser.Email);
                if (driver != null)
                {
                    driver.IsDeleted = true;
                    driver.UpdatedBy = userContext.Id;
                    driver.UpdatedDate = DateTime.Now;
                    var message = $"The driver '{driver.FirstName} {driver.LastName}' was deleted by '{userContext.Name}' of company '{userContext.CompanyName}'";
                    LogManager.Logger.WriteException("SettingsDomain", "RemoveDriverFromUserTable", message, null);
                    if (driver.OrderXDrivers.Any(t => t.IsActive))
                    {
                        driver.OrderXDrivers.Where(t => t.IsActive).ToList().ForEach(x => x.IsActive = false);
                    }
                    if (driver.DeliveryScheduleXDrivers.Any(t => t.IsActive))
                    {
                        driver.DeliveryScheduleXDrivers.Where(t => t.IsActive).ToList().ForEach(x => x.IsActive = false);
                    }
                    //if (!driver.OrderXDrivers.Any(t => t.IsActive) && !driver.DeliveryScheduleXDrivers.Any(t => t.IsActive))
                    //{
                    //    Context.DataContext.OrderXDrivers.RemoveRange(driver.OrderXDrivers.Where(t => !t.IsActive));
                    //    Context.DataContext.DeliveryScheduleXDrivers.RemoveRange(driver.DeliveryScheduleXDrivers.Where(t => !t.IsActive));
                    //    Context.DataContext.Users.Remove(driver);


                    //}
                    //else
                    //{
                    //    shouldCommitTransaction = false;
                    //    response.StatusMessage = string.Format(Resource.errMessageCantDeleteAssignedDriver, driver.FirstName, driver.LastName);
                    //}
                }
                else
                {
                    shouldCommitTransaction = false;
                    response.StatusMessage = string.Format(Resource.errMessageCantDeleteAssignedDriver, additionalInvitedUser.Email, string.Empty);
                }
            }

            return shouldCommitTransaction;
        }

        public async Task<StatusViewModel> SaveCompanySubGroup(CompanySubGroupViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var companyGroups = await Context.DataContext.CompanyGroups.Where(t => t.GroupName.ToLower() == viewModel.SubGroupName.ToLower()).ToListAsync();
                    if (companyGroups != null && companyGroups.Any())
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageCompanyGroupAlreayExists;
                        transaction.Rollback();
                        return response;
                    }

                    if (viewModel.SubGroupCompanyIds == null || viewModel.SubGroupCompanyIds.Count == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageSelectChildAccount;
                        transaction.Rollback();
                        return response;
                    }

                    bool isExistingcompanyGroup = false;
                    var existingGroups = Context.DataContext.CompanyGroups.Where(t => t.CompanyGroupTypeId == viewModel.CompanyType &&
                                                                                      t.OwnerCompanyId == userContext.CompanyId).ToList();
                    foreach (var grp in existingGroups)
                    {
                        var subGrps = grp.CompanyGroupXCompanies.Select(t => t.SubCompanyId).ToList();
                        var lstExcept = viewModel.SubGroupCompanyIds.Except(subGrps).ToList();

                        if (lstExcept.Count == 0 && subGrps.Count == viewModel.SubGroupCompanyIds.Count)
                        {
                            isExistingcompanyGroup = true;
                            break;
                        }
                    }


                    if (isExistingcompanyGroup)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageCompanyGroupAlreayExists;
                        transaction.Rollback();
                        return response;
                    }

                    var companyGroup = new CompanyGroup
                    {
                        GroupName = viewModel.SubGroupName,
                        CompanyGroupTypeId = viewModel.CompanyType,
                        OwnerCompanyId = userContext.CompanyId,
                        IsActive = true,
                        UpdatedBy = userContext.Id,
                        UpdatedDate = DateTimeOffset.Now
                    };

                    Context.DataContext.CompanyGroups.Add(companyGroup);
                    await Context.CommitAsync();

                    var companyGroupXCompanies = await Context.DataContext.CompanyGroupXCompanies.Where(t => t.SubCompanyId == viewModel.OwnerCompanyId && t.CompanyGroupId == companyGroup.Id).ToListAsync();
                    if (companyGroupXCompanies != null && companyGroupXCompanies.Any())
                    {
                        Context.DataContext.CompanyGroupXCompanies.RemoveRange(companyGroupXCompanies);
                        await Context.CommitAsync();
                    }

                    foreach (var companyId in viewModel.SubGroupCompanyIds)
                    {
                        var companyGroupXCompany = new CompanyGroupXCompany()
                        {
                            CompanyGroupId = companyGroup.Id,
                            SubCompanyId = companyId,
                            IsActive = true,
                            UpdatedBy = userContext.Id,
                            UpdatedDate = DateTimeOffset.Now
                        };
                        Context.DataContext.CompanyGroupXCompanies.Add(companyGroupXCompany);
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageCompanyGroupCreate;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageFailedToCreateGroup;
                    LogManager.Logger.WriteException("SettingsDomain", "SaveCompanySubGroup", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> EditCompanySubGroup(CompanySubGroupViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (viewModel.SubGroupCompanyIds == null || viewModel.SubGroupCompanyIds.Count == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageSelectChildAccount;
                        transaction.Rollback();
                        return response;
                    }

                    var companyGroup = Context.DataContext.CompanyGroups.FirstOrDefault(t => t.Id == viewModel.Id);
                    if (companyGroup == null)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageCompanyGroupNotExists;
                        transaction.Rollback();
                        return response;
                    }

                    bool isExistingcompanyGroup = false;
                    var existingGroups = Context.DataContext.CompanyGroups.Where(t => t.CompanyGroupTypeId == viewModel.CompanyType && companyGroup.GroupName.ToLower() == viewModel.SubGroupName.ToLower() &&
                                                                                      t.OwnerCompanyId == userContext.CompanyId).ToList();
                    foreach (var grp in existingGroups)
                    {
                        var subGrps = grp.CompanyGroupXCompanies.Select(t => t.SubCompanyId).ToList();
                        var lstExcept = viewModel.SubGroupCompanyIds.Except(subGrps).ToList();

                        if (lstExcept.Count == 0 && subGrps.Count == viewModel.SubGroupCompanyIds.Count)
                        {
                            isExistingcompanyGroup = true;
                            break;
                        }
                    }

                    if (isExistingcompanyGroup)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageCompanyGroupAlreayExists;
                        transaction.Rollback();
                        return response;
                    }

                    companyGroup.GroupName = viewModel.SubGroupName;
                    companyGroup.CompanyGroupTypeId = viewModel.CompanyType;
                    companyGroup.OwnerCompanyId = viewModel.OwnerCompanyId;
                    companyGroup.UpdatedBy = userContext.Id;
                    companyGroup.UpdatedDate = DateTimeOffset.Now;
                    Context.DataContext.Entry(companyGroup).State = EntityState.Modified;

                    var companyGroupXCompanies = await Context.DataContext.CompanyGroupXCompanies.Where(t => t.CompanyGroupId == companyGroup.Id).ToListAsync();
                    if (companyGroupXCompanies != null && companyGroupXCompanies.Any())
                    {
                        Context.DataContext.CompanyGroupXCompanies.RemoveRange(companyGroupXCompanies);
                        await Context.CommitAsync();
                    }

                    foreach (var companyId in viewModel.SubGroupCompanyIds)
                    {
                        var companyGroupXCompany = new CompanyGroupXCompany()
                        {
                            CompanyGroupId = companyGroup.Id,
                            SubCompanyId = companyId,
                            IsActive = true,
                            UpdatedBy = userContext.Id,
                            UpdatedDate = DateTimeOffset.Now
                        };
                        Context.DataContext.CompanyGroupXCompanies.Add(companyGroupXCompany);
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageCompanyGroupUpdate;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageCompanyGroupUpdateFailed;
                    LogManager.Logger.WriteException("SettingsDomain", "SaveCompanySubGroup", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<ChildCompanyViewModel>> GetCompanySubGroupGridDetails(int CompanyId)
        {
            var response = new List<ChildCompanyViewModel>();
            try
            {
                var subGroups = await Context.DataContext.CompanyGroups.Where(t => t.OwnerCompanyId == CompanyId).OrderByDescending(t1 => t1.Id).ToListAsync();
                if (subGroups != null && subGroups.Any())
                {
                    foreach (var companyGroup in subGroups)
                    {
                        ChildCompanyViewModel obj = new ChildCompanyViewModel();
                        obj.Id = companyGroup.Id;
                        obj.Name = companyGroup.GroupName;
                        obj.CompanyType = companyGroup.CompanyGroupTypeId.ToString();
                        obj.ChildCompanyType = companyGroup.CompanyGroupTypeId;

                        System.Text.StringBuilder cmpNames = new System.Text.StringBuilder();
                        companyGroup.CompanyGroupXCompanies.ToList().ForEach(t => cmpNames.Append(", " + t.SubCompany.Name));
                        obj.SelectedCompanyNames = cmpNames.ToString().TrimStart(',');

                        response.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCompanySubGroupGridDetails", ex.Message, ex);
            }

            return response;
        }

        public async Task<CompanySubGroupViewModel> GetSubGroupById(int subGroupId)
        {
            var response = new CompanySubGroupViewModel();
            try
            {
                var subGroup = await Context.DataContext.CompanyGroups.FirstOrDefaultAsync(t => t.Id == subGroupId);
                if (subGroup != null)
                {
                    response.SubGroupName = subGroup.GroupName;
                    response.CompanyType = subGroup.CompanyGroupTypeId;
                    response.OwnerCompanyId = subGroup.OwnerCompanyId;

                    var subGroupCompanies = subGroup.CompanyGroupXCompanies.ToList();
                    foreach (var company in subGroupCompanies)
                    {
                        ChildCompanyViewModel obj = new ChildCompanyViewModel();
                        obj.Id = company.SubCompanyId;
                        obj.Name = company.SubCompany.Name;
                        response.Companies.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSubGroupById", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> DeleteCompanySubGroup(int subGroupId)
        {
            var response = new StatusViewModel();
            try
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    var subGroup = await Context.DataContext.CompanyGroups.FirstOrDefaultAsync(t => t.Id == subGroupId);
                    if (subGroup != null)
                    {
                        Context.DataContext.CompanyGroupXCompanies.RemoveRange(subGroup.CompanyGroupXCompanies.ToList());
                        Context.DataContext.CompanyGroups.Remove(subGroup);

                        await Context.CommitAsync();
                        transaction.Commit();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageCompanyGroupDelete;
                    }
                    else
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageCompanySubGroupNotExists;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "DeleteCompanySubGroup", ex.Message, ex);
            }

            return response;
        }

        public StatusViewModel EditAccountingCompanyId(AccountingCompanyIdDetailsViewModel data, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                this.SetBuyerSupplierInformation(data.SupplierCompanyId, data.BuyerCompanyId, data.JobId, data.AccountingCompanyId, true, OrderCreationMethod.Unknown, userContext);
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageAccountingCompanyIdUpdated;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "EditAccountingCompanyId", ex.Message, ex);

            }
            return response;
        }
        //Input parameters: suppliercompId, buyercompanyId, accounting comp id,accountingCompanyId ,isUpdate Flag 
        //set isUpdate = true only when updating already existing accountingCompanyId else set it to false 
        public void SetBuyerSupplierInformation(int supplierCompanyId, int buyerCompanyId, int jobId, string accountingCompanyId, bool isUpdate, OrderCreationMethod creationMethod, UserContext userContext)
        {
            // Create new entry for supplierCompanyId, int buyerCompanyId, string accountingCompanyId              
            try
            {
                var buyerAccountingCompany = Context.DataContext.SupplierXBuyerDetails.Where(x => x.SupplierCompanyId == supplierCompanyId && x.BuyerCompanyId == buyerCompanyId && x.JobId == jobId).ToList().FirstOrDefault();
                if (buyerAccountingCompany != null)
                {

                    buyerAccountingCompany.AccountingCompanyId = accountingCompanyId;
                    Context.DataContext.Entry(buyerAccountingCompany).State = EntityState.Modified;
                    Context.Commit();
                }
                else
                {

                    AddBuyerSupplierInformation(supplierCompanyId, buyerCompanyId, jobId, accountingCompanyId, creationMethod, userContext);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "SetBuyerSupplierInformation", ex.Message, ex);
            }
        }

        public StatusViewModel SetSupplierIsBadgeMandatory(int supplierCompanyId, int buyerCompanyId, int jobId, bool isBadgeMandatory, OrderCreationMethod creationMethod, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                var buyerSupplierDetails = Context.DataContext.SupplierXBuyerDetails.FirstOrDefault(x => x.SupplierCompanyId == supplierCompanyId && x.BuyerCompanyId == buyerCompanyId && x.JobId == jobId && x.IsActive);
                if (buyerSupplierDetails != null)
                {
                    buyerSupplierDetails.IsBadgeMandatory = isBadgeMandatory;
                    Context.DataContext.Entry(buyerSupplierDetails).State = EntityState.Modified;
                    Context.Commit();
                }
                else
                {
                    buyerSupplierDetails = new SupplierXBuyerDetails
                    {
                        SupplierCompanyId = supplierCompanyId,
                        BuyerCompanyId = buyerCompanyId,
                        JobId = jobId,
                        IsBadgeMandatory = isBadgeMandatory,
                        OrderCreationMethod = creationMethod,
                        LastModifiedDate = DateTimeOffset.Now,
                        CreatedBy = userContext.Id,
                        UpdatedBy = userContext.Id,
                        IsActive = true,
                    };
                    Context.DataContext.SupplierXBuyerDetails.Add(buyerSupplierDetails);
                    Context.Commit();
                }
                response.StatusCode = Status.Success;
                if (isBadgeMandatory)
                {
                    response.StatusMessage = Resource.successMessageIsBadgeMandatoryChecked;
                }
                else
                {
                    response.StatusMessage = Resource.successMessageIsBadgeMandatoryUnchecked;
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "SetSupplierIsBadgeMandatory", ex.Message, ex);
            }
            return response;
        }
        private void AddBuyerSupplierInformation(int supplierCompanyId, int buyerCompanyId, int jobId, string accountingCompanyId, OrderCreationMethod creationMethod, UserContext userContext)
        {
            try
            {
                var buyerSupplierDetails = new SupplierXBuyerDetails
                {
                    SupplierCompanyId = supplierCompanyId,
                    BuyerCompanyId = buyerCompanyId,
                    JobId = jobId,
                    AccountingCompanyId = accountingCompanyId,
                    OrderCreationMethod = creationMethod,
                    LastModifiedDate = DateTimeOffset.Now,
                    CreatedBy = userContext.Id,
                    UpdatedBy = userContext.Id,
                    IsActive = true,
                };
                Context.DataContext.SupplierXBuyerDetails.Add(buyerSupplierDetails);
                Context.Commit();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "AddBuyerSupplierInformation", ex.Message, ex);
            }
        }
        public AccountingCompanyIdDetailsViewModel GetAccountingCompanyID(int buyerCompanyId, int JobId, UserContext userContext)
        {
            var response = new AccountingCompanyIdDetailsViewModel();
            try
            {
                SupplierXBuyerDetails supplierBuyerDetails = new SupplierXBuyerDetails();
                supplierBuyerDetails = Context.DataContext.SupplierXBuyerDetails.Where(x => x.SupplierCompanyId == userContext.CompanyId && x.BuyerCompanyId == buyerCompanyId && x.JobId == JobId).ToList().FirstOrDefault();
                var buyerCompanyName = Context.DataContext.Companies.Where(x => x.Id == buyerCompanyId)
                                                                    .Select(x1 => x1.Name).FirstOrDefault().ToString();
                var jobName = Context.DataContext.Jobs.Where(x => x.Id == JobId)
                                                                    .Select(x1 => x1.Name).FirstOrDefault().ToString();
                //Implies we need to add its Entry into DB as it doesnt exist
                if (supplierBuyerDetails == null)
                {
                    response.AccountingCompanyId = string.Empty;
                }
                else
                {
                    response.AccountingCompanyId = supplierBuyerDetails.AccountingCompanyId;
                }
                response.BuyerCompanyName = buyerCompanyName;
                response.BuyerCompanyId = buyerCompanyId;
                response.SupplierCompanyId = userContext.CompanyId;
                response.JobName = jobName;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetAccountingCompanyID", ex.Message, ex);
            }
            return response;
        }

        public string GetAccountingCompanyIdforOrder(int buyerCompanyId, int jobId, UserContext userContext)
        {
            string response = string.Empty;

            try
            {
                response = Context.DataContext.SupplierXBuyerDetails.Where(x => x.SupplierCompanyId == userContext.CompanyId && x.BuyerCompanyId == buyerCompanyId && x.JobId == jobId)
                                                              .Select(x1 => x1.AccountingCompanyId).ToList().FirstOrDefault();
                if ((response != null) || (response != string.Empty))
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetAccountingCompanyIdforOrder", ex.Message, ex);
            }
            return response;

        }

        public string GetAccountingCompanyIdforOrder(int buyerCompanyId, int supplierCompanyId)
        {
            string response = string.Empty;

            try
            {
                response = Context.DataContext.SupplierXBuyerDetails.Where(x => x.SupplierCompanyId == supplierCompanyId && x.BuyerCompanyId == buyerCompanyId)
                                                              .Select(x1 => x1.AccountingCompanyId).ToList().FirstOrDefault();
                if ((response != null) || (response != string.Empty))
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetAccountingCompanyIdforOrder", ex.Message, ex);
            }
            return response;

        }
        public async Task<List<FavoriteSideMenuViewModel>> GetFavoriteSideMenu(int userId)
        {
            var response = new List<FavoriteSideMenuViewModel>();
            try
            {
                var entities = await Context.DataContext.FavoriteSideMenu.Where(t => t.UserId == userId).ToListAsync();
                entities.ForEach(t => response.Add(t.ToViewModel()));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingDomain", "GetFavoriteSideMenu", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> SetFavoriteSideMenu(FavoriteSideMenuViewModel favoriteLeftMenuViewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var entity = favoriteLeftMenuViewModel.ToEntity();

                Context.DataContext.FavoriteSideMenu.Add(entity);
                await Context.DataContext.SaveChangesAsync();

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successFavoriteLeftMenuAdded;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingDomain", "SetFavoriteSideMenu", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> RemoveFavoriteSideMenu(FavoriteSideMenuViewModel favoriteLeftMenuViewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var entity = await Context.DataContext.FavoriteSideMenu.Where(
                    q => q.UserId == favoriteLeftMenuViewModel.UserId &&
                    q.LinkId == favoriteLeftMenuViewModel.LinkId).ToListAsync();

                Context.DataContext.FavoriteSideMenu.RemoveRange(entity);
                Context.DataContext.SaveChanges();

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successFavoriteLeftMenuRemoved;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingDomain", "RemoveFavoriteSideMenu", ex.Message, ex);
            }

            return response;
        }

        public AdditionalUsersViewModel InitialiseDriverModel(DriverObjectModel driverModel, UserContext userContext)
        {
            AdditionalUsersViewModel model = new AdditionalUsersViewModel();
            try
            {
                driverModel.CreatedBy = userContext.Id;
                driverModel.CompanyId = userContext.CompanyId;
                driverModel.CreatedOn = DateTimeOffset.Now;

                model = driverModel.ToDriverViewModel();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingDomain", "InitialiseDriverModel", ex.Message, ex);
            }

            return model;
        }

        public async Task<DriverGridViewModel> GetAllDrivers(UserContext userContext)
        {
            DriverGridViewModel model = new DriverGridViewModel();
            try
            {
                var roleIds = new List<int>();
                roleIds.Add((int)UserRoles.Driver);

                var invitedUsers = await GetInvitedUserListAsync(userContext.CompanyId, userContext.Id, roleIds);
                var onboardedUsers = await GetOnboardedUserListAsync(userContext.CompanyId, userContext.Id, roleIds);

                var freightDomain = new FreightServiceDomain(this);
                var drivers = await freightDomain.GetDriversByCompany(userContext.CompanyId);

                // update invited users
                if (invitedUsers != null && invitedUsers.Count > 0)
                {
                    foreach (var invitedUser in invitedUsers)
                    {
                        //var name = $"{invitedUser.FirstName} {invitedUser.LastName}";
                        var user = Context.DataContext.Users
                                            .Where(t => t.Email == invitedUser.Email && t.CompanyId == invitedUser.CompanyId)
                                            .Select(t => new { t.Id, t.PhoneNumber })
                                            .OrderByDescending(t => t.Id).FirstOrDefault();
                        if (user != null)
                        {
                            invitedUser.DriverUserId = user.Id;
                            invitedUser.PhoneNumber = user.PhoneNumber;
                            if (drivers != null && drivers.Count > 0)
                            {
                                var driver = drivers.Where(t => t.DriverId == user.Id && t.CompanyId == invitedUser.CompanyId).FirstOrDefault();
                                if (driver != null)
                                    setDriverInfo(invitedUser, driver);
                            }
                        }
                    }
                    invitedUsers.ForEach(t => { model.InvitedDrivers.Add(t.ToDriverModel()); });
                }

                // update onboarding users
                if (onboardedUsers != null && onboardedUsers.Count > 0)
                {
                    foreach (var onboardedUser in onboardedUsers)
                    {   
                        if (drivers != null && drivers.Count > 0)
                        {
                            var driver = drivers.Where(t => t.DriverId == onboardedUser.Id && t.CompanyId == userContext.CompanyId).FirstOrDefault();
                            if (driver != null)
                                setDriverInfo(onboardedUser, driver);
                        }
                    }
                    onboardedUsers.ForEach(t => model.OnboardedDrivers.Add(t.ToDriverModel()));
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingDomain", "GetAllDrivers", ex.Message, ex);
            }

            return model;
        }

        private static void setDriverInfo(AdditionalUserViewModel user, DriverObjectModel driver)
        {
            if (user.DriverInfo == null)
                user.DriverInfo = new DriverInformationViewModel();
            user.DriverInfo.LicenseNumber = driver.LicenseNumber;
            user.DriverInfo.LicenseTypeId = driver.LicenseTypeId;
            user.DriverInfo.ShiftId = driver.ShiftId;
            user.DriverInfo.TrailerType = driver.TrailerType;
            user.DriverInfo.ExpiryDate = driver.ExpiryDate;
            user.DriverInfo.CompanyName = driver.CompanyName;
            user.DriverInfo.Regions = driver.Regions;
            user.DriverInfo.IsFilldAuthorized = driver.IsFilldAuthorized;
        }
        public async Task<AdditionalUsersViewModel> GetInvitedCompanyUsersAsync(int invitedUserId, UserContext userContext)
        {
            AdditionalUsersViewModel response = new AdditionalUsersViewModel(Status.Success);
            try
            {
                if (invitedUserId > 0)
                {
                    var invitedUser = new AdditionalUserViewModel(Status.Success);
                    var driverUserId = 0;
                    var invitedUsers = Context.DataContext.InvitedUsers.Where(t => t.Id == invitedUserId).Select(t => t.Email).FirstOrDefault();
                    if (invitedUsers != null)
                    {
                        bool existingUser = await ContextFactory.Current.GetDomain<HelperDomain>().IsEmailExistAsync(invitedUsers.Trim(), true);
                        if (existingUser)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageUserAlreadyExistsInTFX;
                            return response;
                        }
                        else
                        {
                            var additionalUser = await Context.DataContext.InvitedUsers
                                .Select(t => new
                                {
                                    t.Id,
                                    t.FirstName,
                                    t.LastName,
                                    t.Email,
                                    InvitedBy = t.Company == null ? "" : t.Company.Name,
                                    UserId = t.User == null ? (int?)null : t.User.Id,
                                    RoleIds = t.MstRoles.Select(x => x.Id),
                                    t.Company.CompanyTypeId,
                                    t.CompanyId,
                                    t.Title
                                }).SingleOrDefaultAsync(t => t.Id == invitedUserId);

                            invitedUser.Id = additionalUser.Id;
                            invitedUser.FirstName = additionalUser.FirstName;
                            invitedUser.LastName = additionalUser.LastName;
                            invitedUser.Email = additionalUser.Email;
                            invitedUser.DisplayMode = PageDisplayMode.Create;
                            invitedUser.InvitedBy = additionalUser.InvitedBy;
                            invitedUser.RoleIds = additionalUser.RoleIds.ToList();
                            invitedUser.IsInvitedUser = true;
                            invitedUser.CompanyId = additionalUser.CompanyId;
                            invitedUser.Title = additionalUser.Title.ToString();

                            response.CompanyId = additionalUser.CompanyId;
                            response.AdditionalUsers.Add(invitedUser);
                            if (invitedUser.RoleIds.Contains((int)UserRoles.Driver))
                            {
                                driverUserId = Context.DataContext.Users
                                                .Where(t => t.Email == additionalUser.Email && t.CompanyId == additionalUser.CompanyId)
                                                .Select(t => t.Id).FirstOrDefault();
                            }

                        }

                        if (invitedUser.RoleIds.Contains((int)UserRoles.Driver))
                        {
                            var fsDomain = new FreightServiceDomain(this);
                            var driverObject = await fsDomain.GetDriverObjectById(driverUserId);
                            if (driverObject != null)
                            {
                                invitedUser.DriverInfo = new DriverInformationViewModel()
                                {
                                    LicenseNumber = driverObject.LicenseNumber,
                                    ProfilePhotoUrl = driverObject.ProfilePhoto,
                                    ShiftId = driverObject.ShiftId,
                                    TrailerType = driverObject.TrailerType,
                                    CardNumbers = driverObject.CardNumbers,
                                    ExpiryDate = driverObject.ExpiryDate,
                                    CompanyName = driverObject.CompanyName,
                                    LicenseTypeId = driverObject.LicenseTypeId,
                                    IsFilldAuthorized = driverObject.IsFilldAuthorized,
                                };
                            }
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageInvitedUserAlreadyRegistered;
                    }
                }
                else
                {
                    response.CompanyId = userContext.Id;
                    response.AdditionalUsers.Add(new AdditionalUserViewModel(Status.Success) { CompanyId = userContext.Id });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetInvitedCompanyUsersAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<Response> CreateExternalAdditionalUsersAsync(AdditionalUsersViewModel viewModel)
        {
            Response response = new Response(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    response = await AddExternalCompanyUser(viewModel);
                    if (response.StatusCode == Status.Success)
                    {
                        transaction.Commit();
                        response.StatusMessages.Add(Resource.errMessageCreateAdditionalUserSuccess);
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessages.Add(Resource.errMessageCreateAdditionalUserFailed);
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SettingsDomain", "CreateExternalAdditionalUsersAsync", ex.Message, ex);
                }
            }
            return response;
        }
        public async Task<Response> AddExternalCompanyUser(AdditionalUsersViewModel viewModel)
        {
            Response response = new Response(Status.Failed);
            try
            {
                var currentUser = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == viewModel.UserId);
                if (currentUser != null && currentUser.Company != null)
                {
                    List<string> email = viewModel.AdditionalUsers.Select(t => t.Email).ToList();
                    Context.DataContext.Users.Where(t => email.Contains(t.Email)).ToList()
                        .ForEach(t => response.StatusMessages.Add(ResourceMessages.GetMessage(Resource.errMessageUserAlreadyOnboardedAgainstCompany,
                                new[] { t.Email, t.Company.Name })));

                    var fsDomain = new FreightServiceDomain(this);
                    foreach (var additionalUser in viewModel.AdditionalUsers)
                    {
                        if (additionalUser.Id != 0)
                        {

                            if (additionalUser.RoleIds.Contains((int)UserRoles.Admin) && additionalUser.RoleIds.Count > 0)
                            {
                                additionalUser.RoleIds = new List<int> { (int)UserRoles.Admin };
                            }

                            var existingInvitedUser = Context.DataContext.InvitedUsers.FirstOrDefault(t => t.Id != additionalUser.Id && t.Email.ToLower() == additionalUser.Email.ToLower());
                            if (existingInvitedUser != null)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessages.Add(ResourceMessages.GetMessage(Resource.errMessageUserAlreadyOnboardedAgainstCompany, new[] { existingInvitedUser.Email }));
                                return response;
                            }
                            var InvitedUsers = Context.DataContext.InvitedUsers.SingleOrDefault(t => t.Id == additionalUser.Id);
                            if (InvitedUsers != null)
                            {

                                InvitedUsers.MstRoles.ToList().RemoveAll(t => t.Id > 0);

                                var mstRole = Context.DataContext.MstRoles.Where(t => additionalUser.RoleIds.Contains(t.Id)).ToList();
                                foreach (var role in mstRole)
                                {
                                    InvitedUsers.MstRoles.Add(role);
                                }
                                InvitedUsers.InvitedBy = viewModel.UserId;
                                Context.DataContext.Entry(InvitedUsers).State = EntityState.Modified;
                                await Context.CommitAsync();

                                //Add to user if driver role
                                if (additionalUser.RoleIds.Contains((int)UserRoles.Driver))
                                {
                                    var StaticPassword = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingApplicationUserDefaultPassword, "First#1234");
                                    var RandomPassword = CryptoHelperMethods.GeneratePassword(Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings[ApplicationConstants.RandomPasswordLength]), StaticPassword);
                                    var salt = CryptoHelperMethods.GenerateSalt();
                                    User newUser = new User
                                    {
                                        FirstName = additionalUser.FirstName,
                                        LastName = additionalUser.LastName,
                                        UserName = additionalUser.Email.Trim().ToLower(),
                                        Email = additionalUser.Email.Trim().ToLower(),
                                        IsEmailConfirmed = false,
                                        PhoneNumber = string.IsNullOrWhiteSpace(additionalUser.PhoneNumber) ? Constants.DummyPhoneNumber : additionalUser.PhoneNumber,
                                        IsPhoneNumberConfirmed = false,
                                        IsTwoFactorEnabled = false,
                                        AccessFailedCount = 0,
                                        IsLockoutEnabled = true,
                                        LockoutEndDateUtc = null,
                                        PasswordHash = CryptoHelperMethods.GenerateHash(RandomPassword, salt),
                                        SecurityStamp = salt,
                                        FingerPrint = CryptoHelperMethods.GenerateHash(additionalUser.Email, CryptoHelperMethods.GenerateSalt()),
                                        IsOnboardingComplete = false,
                                        IsActive = false,
                                        CreatedBy = viewModel.UserId,
                                        CreatedDate = DateTimeOffset.Now,
                                        UpdatedBy = viewModel.UserId,
                                        UpdatedDate = DateTimeOffset.Now,
                                        CompanyId = additionalUser.CompanyId,
                                        MstRoles = Context.DataContext.MstRoles.Where(t => additionalUser.RoleIds.Contains(t.Id)).ToList(),
                                        Title = additionalUser.Title
                                    };
                                    Context.DataContext.Users.Add(newUser);
                                    await Context.CommitAsync();
                                    additionalUser.Id = newUser.Id;

                                    await AddDriverObject(fsDomain, additionalUser, newUser);
                                }

                                //Add an entry to notifications
                                var notificationDomain = new NotificationDomain(this);
                                await notificationDomain.AddNotificationEventAsync(EventType.InvitedUserAdded, InvitedUsers.Id, currentUser.Id, null, null, viewModel.ApplicationTemplateId);

                            }
                            response.StatusCode = Status.Success;
                        }
                    }
                }
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusMessages.Add(Resource.errMessageCreateAdditionalUserFailed);
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("SettingsDomain", "AddExternalCompanyUser", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<AdditionalUserViewModel>> GetExternalInvitedUserListAsync(int companyId, int userId, List<int> roleIds = null)
        {
            List<AdditionalUserViewModel> response = new List<AdditionalUserViewModel>();
            try
            {
                var company = Context.DataContext.Companies.Where(t => t.Id == companyId);
                if (company != null)
                {
                    var companyInvitedUsers = company.SelectMany(t => t.InvitedUsers);
                    var invitedUsers = await companyInvitedUsers.OrderByDescending(t => t.Id).ToListAsync();

                    response = invitedUsers.Where(t => t.CompanyId == companyId && (roleIds == null || t.MstRoles.Any(r => roleIds.Contains(r.Id)))).Select(t => new AdditionalUserViewModel(Status.Success)
                    {
                        Id = t.Id,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Name = $"{t.FirstName} {t.LastName}",
                        Email = t.Email,
                        CompanyId = t.Company.Id,
                        CompanyName = t.Company.Name,
                        InvitedBy = $"{t.User.FirstName} {t.User.LastName}",
                        RoleIds = t.MstRoles.Select(x => x.Id).ToList(),
                        IsOnboarded = t.User.IsOnboardingComplete,
                        PhoneNumber = t.User.PhoneNumber,
                        RoleNames = string.Join(" <br/>", t.MstRoles.Select(x => x.Name).ToList()),
                        DT_RowId = "DT_RowId_" + t.Id
                    }).ToList();
                    var driverEmailIds = response.Select(x => x.Email).ToList();
                    var driver = Context.DataContext.Users.Where(t => !t.IsActive && t.CompanyId == companyId
                                                       && !t.IsOnboardingComplete && driverEmailIds.Contains(t.UserName)).Select(x => new { x.Id, x.UserName }).ToList();
                    response.ForEach(x =>
                    {
                        var driverInfo = driver.FirstOrDefault(x1 => x1.UserName == x.Email);
                        if (driverInfo != null)
                        {
                            x.DriverId = driverInfo.Id;
                        }
                    });
                    List<RegionDriverRemoveModel> driverRemoveModel = new List<RegionDriverRemoveModel>();
                    response.ForEach(x =>
                    {
                        driverRemoveModel.Add(new RegionDriverRemoveModel { DriverId = x.DriverId, UserId = userId });
                    });

                    var driverDSBScheduleInfo = await ContextFactory.Current.GetDomain<FreightServiceDomain>().CheckInvitedDriverScheduleExists(driverRemoveModel);
                    driverDSBScheduleInfo.ForEach(x =>
                    {
                        var driverDSBINfo = response.FirstOrDefault(x1 => x1.DriverId == x.DriverId);
                        if (driverDSBINfo != null)
                        {
                            driverDSBINfo.IsScheduleExists = true;
                            driverDSBINfo.ScheduleBuilderIds.AddRange(x.ScheduleBuilderIds);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetExternalInvitedUserListAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AddFuelAssetInformation(FleetTrailers viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            if (viewModel != null)
            {
                var entity = new FleetInformation();
                entity.CompanyId = userContext.CompanyId;
                entity.FleetType = viewModel.FleetType == 0 ? FleetType.FuelAsset : viewModel.FleetType;
                entity.TrailerServiceType = (int)viewModel.FuelTrailerServiceTypeFTL;
                entity.Capacity = viewModel.Capacity;
                entity.DoesTrailerHasPump = viewModel.TrailerHasPump;
                entity.IsTrailerMetered = viewModel.IsTrailerMetered;
                entity.Count = viewModel.Count;
                Context.DataContext.FleetInformations.Add(entity);
                await Context.CommitAsync();

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageSuccess;
            }
            return response;
        }
        public async Task<StatusViewModel> AddDefAssetInformation(FleetTrailers viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            if (viewModel != null)
            {
                var entity = new FleetInformation();
                entity.CompanyId = userContext.CompanyId;
                entity.FleetType = viewModel.FleetType == 0 ? FleetType.DEF : viewModel.FleetType;
                entity.TrailerServiceType = (int)viewModel.DEFTrailerServiceType;
                entity.Capacity = viewModel.Capacity;
                entity.DoesTrailerHasPump = viewModel.TrailerHasPump;
                entity.IsTrailerMetered = viewModel.IsTrailerMetered;
                entity.Count = viewModel.Count;
                entity.IsPackagedGoods = viewModel.PackagedGoods;
                Context.DataContext.FleetInformations.Add(entity);
                await Context.CommitAsync();

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageSuccess;
            }
            return response;
        }

        public async Task<List<AdditiveProductDetailsViewModel>> GetAdditiveProductsListForCompany(UserContext context)
        {
            var response = new List<AdditiveProductDetailsViewModel>();
            try
            {
                response = await  Context.DataContext.MstProducts
                           .Where(t => t.CompanyId == context.CompanyId && t.IsActive && t.ProductTypeId == (int)ProductTypes.Additives)
                           .Select(t => new AdditiveProductDetailsViewModel { Id = t.Id, AdditiveProductName = t.Name }).OrderByDescending(t=>t.Id).ToListAsync();
                
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("SettingsDomain", "GetAdditiveProductsListForCompany", ex.Message, ex);
            }
            return response;
        }
            
        public async Task<List<ReasonCategoryViewModel>> GetReasonCategories(int companyId)
        {
            var response = new List<ReasonCategoryViewModel>();
            try
            {
                response = await Context.DataContext.ReasonCategories
                           .Where(t => t.CompanyId == companyId && t.IsActive && !t.IsDeleted)
                           .Select(t => new ReasonCategoryViewModel { Id = t.Id, Name = t.Name }).OrderByDescending(t => t.Id).ToListAsync();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetReasonCategories", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetReasonCategoryListDDL(int companyId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = await Context.DataContext.ReasonCategories
                           .Where(t => t.CompanyId == companyId && t.IsActive && !t.IsDeleted)
                           .Select(t => new DropdownDisplayExtendedItem { Id = t.Id, Name = t.Name }).OrderBy(t => t.Name).ToListAsync();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetReasonCategoryListDDL", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveReasonCategory(ReasonCategoryViewModel reasonCategoryViewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                if (reasonCategoryViewModel.Id > 0)
                {
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var existingCategory = Context.DataContext.ReasonCategories.Where(t => t.Id == reasonCategoryViewModel.Id).SingleOrDefault();
                            existingCategory.IsActive = false;
                            existingCategory.IsDeleted = reasonCategoryViewModel.IsDeleted;
                            if (reasonCategoryViewModel.IsDeleted)
                            {
                                existingCategory.UpdatedBy = userContext.Id;
                                existingCategory.UpdatedDate = DateTimeOffset.Now;
                            }

                            foreach (var reasonCode in existingCategory.ReasonCodeDetails)
                            {
                                reasonCode.IsActive = false;
                                if (reasonCategoryViewModel.IsDeleted)
                                {
                                    reasonCode.UpdatedBy = userContext.Id;
                                    reasonCode.UpdatedDate = DateTimeOffset.Now;
                                }
                            }
                            
                            Context.DataContext.Entry(existingCategory).State = EntityState.Modified;
                            await Context.CommitAsync();

                            if (!reasonCategoryViewModel.IsDeleted)
                            {
                                var newCategory = reasonCategoryViewModel.ToEntity(userContext, true);
                                if (!CheckIfCategoryNameExists(reasonCategoryViewModel, userContext, response))
                                {
                                    var existingReasonCodes = Context.DataContext.ReasonCategories.Where(t => t.Id == reasonCategoryViewModel.Id).SelectMany(t => t.ReasonCodeDetails).ToList();
                                    foreach (var item in existingReasonCodes)
                                    {
                                        newCategory.ReasonCodeDetails.Add(item.ToNewEntityFromExisting(userContext));
                                    }
                                    Context.DataContext.ReasonCategories.Add(newCategory);
                                    await Context.CommitAsync();
                                }
                            }

                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.successMsgEntityUpdated, Resource.lblCategory);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogManager.Logger.WriteException("SettingsDomain", "SaveReasonCategory", ex.Message, ex);
                        }
                    }
                }
                else
                {
                    if (!CheckIfCategoryNameExists(reasonCategoryViewModel, userContext, response))
                    {
                        var newCategory = reasonCategoryViewModel.ToEntity(userContext);
                        Context.DataContext.ReasonCategories.Add(newCategory);
                        await Context.CommitAsync();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMsgEntitySaved, Resource.lblCategory);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "SaveReasonCategory", ex.Message, ex);
            }
            return response;
        }

        private bool CheckIfCategoryNameExists(ReasonCategoryViewModel reasonCategoryViewModel, UserContext userContext, StatusViewModel response)
        {
            var isNameExists = Context.DataContext.ReasonCategories.Any(t => t.IsActive && t.CompanyId == userContext.CompanyId && t.Name.ToLower() == reasonCategoryViewModel.Name.ToLower());
            if(isNameExists)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.errMsgEntityAlreadyExists, $"{Resource.lblCategory} - {reasonCategoryViewModel.Name}");
            }
            return isNameExists;
        }

        public async Task<StatusViewModel> SaveReasonCodeDescription(ReasonCodeModel reasonCodeModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                if (reasonCodeModel.Id > 0)
                {
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var existingodedetails = Context.DataContext.ReasonCodes.Where(t => t.Id == reasonCodeModel.Id).SingleOrDefault();
                            existingodedetails.IsActive = false;
                            existingodedetails.IsDeleted = reasonCodeModel.IsDeleted;
                            if (reasonCodeModel.IsDeleted)
                            {
                                existingodedetails.UpdatedBy = userContext.Id;
                                existingodedetails.UpdatedDate = DateTimeOffset.Now;
                            }

                            Context.DataContext.Entry(existingodedetails).State = EntityState.Modified;
                            await Context.CommitAsync();

                            if (!reasonCodeModel.IsDeleted)
                            {
                                await AddNewResonCodeDetails(reasonCodeModel, userContext);
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogManager.Logger.WriteException("SettingsDomain", "SaveReasonCodeDescription", ex.Message, ex);
                        }

                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMsgEntityUpdated, Resource.lblReasonCode);
                    }
                }
                else
                {
                    await AddNewResonCodeDetails(reasonCodeModel, userContext);
                    response.StatusCode = Status.Success;
                    response.StatusMessage = string.Format(Resource.successMsgEntitySaved, Resource.lblReasonCode);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "SaveReasonCodeDescription", ex.Message, ex);
            }
            return response;
        }

        private async Task AddNewResonCodeDetails(ReasonCodeModel reasonCodeModel, UserContext userContext)
        {
            var newCodeDetails = reasonCodeModel.ToEntity(userContext);
            Context.DataContext.ReasonCodes.Add(newCodeDetails);
            await Context.CommitAsync();
        }

        public async Task<StatusViewModel> DeleteReasonCategory(int id, UserContext context)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var existingCategory = await Context.DataContext.ReasonCategories.Where(t => t.Id == id).SingleOrDefaultAsync();
                if(existingCategory != null)
                {
                    var model = new ReasonCategoryViewModel()
                    {
                        Id = existingCategory.Id,
                        CompanyId = existingCategory.CompanyId,
                        IsDeleted = true,
                        IsActive = false
                    };
                    response = await SaveReasonCategory(model, context);
                    if(response.StatusCode == Status.Success)
                    {
                        response.StatusMessage = string.Format(Resource.errMessageDeletedSuccess, existingCategory.Name);
                    }
                }
                else
                {
                    response.StatusMessage = Resource.errRecordNotFound;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "DeleteReasonCategory", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ReasonCodeModel>> GetReasonCodes(int companyId, int? categoryId = null)
        {
            var response = new List<ReasonCodeModel>();
            try
            {
                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    response = await Context.DataContext.ReasonCodes
                               .Where(t => t.CategoryId == categoryId && t.CompanyId == companyId && t.IsActive && !t.IsDeleted)
                               .Select(t => new ReasonCodeModel { Id = t.Id, ReasonCode = t.ReasonCode, Description = t.Description, IsActive = t.IsActive, IsDeleted = t.IsDeleted, CategoryId = t.CategoryId }).OrderByDescending(t => t.Id).ToListAsync();
                }
                else
                {
                    response = await Context.DataContext.ReasonCodes
                              .Where(t => (t.CategoryId == null || t.CategoryId == 0) && t.CompanyId == companyId && t.IsActive && !t.IsDeleted)
                              .Select(t => new ReasonCodeModel { Id = t.Id, ReasonCode = t.ReasonCode, Description = t.Description, IsActive = t.IsActive, IsDeleted = t.IsDeleted, CategoryId = t.CategoryId }).OrderByDescending(t => t.Id).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetReasonCodes", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteReasonCode(int id, UserContext context)
        {
            var response = new StatusViewModel();
            try
            {
                var existingReasonCode = await Context.DataContext.ReasonCodes.Where(t => t.Id == id).SingleOrDefaultAsync();
                if (existingReasonCode != null)
                {
                    var model = new ReasonCodeModel()
                    {
                        Id = existingReasonCode.Id,
                        CompanyId = existingReasonCode.CompanyId,
                        IsDeleted = true,
                        IsActive = false
                    };
                    response = await SaveReasonCodeDescription(model, context);
                    if (response.StatusCode == Status.Success)
                    {
                        response.StatusMessage = string.Format(Resource.errMessageDeletedSuccess, "Reason Description");
                    }
                }
                else
                {
                    response.StatusMessage = Resource.errRecordNotFound;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "DeleteReasonCode", ex.Message, ex);
            }
            return response;
        }
    }
}
