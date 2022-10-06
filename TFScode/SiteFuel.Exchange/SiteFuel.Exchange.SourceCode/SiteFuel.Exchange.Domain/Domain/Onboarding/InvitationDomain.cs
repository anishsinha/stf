using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class InvitationDomain : BaseDomain
    {
        public InvitationDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public InvitationDomain(BaseDomain domain)
            : base(domain)
        {
        }
        public async Task<StatusViewModel> SaveThirdPartyInvitation(ThirdPartyCompanyInviteViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                if (viewModel != null)
                {
                    var invitedCompanyId = GetInvitatedCompanyIdByToken(viewModel.Token);
                    if (invitedCompanyId != null)
                    {
                        var supplierURL = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == invitedCompanyId.Value && t.IsActive).Select(t => t.URLName).FirstOrDefault();
                        if (!viewModel.CompanyInfo.IsNewCompany)
                        {
                            var invitedUser = SaveExistingUserInvitation(viewModel);
                            invitedUser.InvitedBy =  (int)SystemUser.System;
                            invitedUser.CompanyId = invitedCompanyId.Value;
                            Context.DataContext.InvitedUsers.Add(invitedUser);
                            await Context.CommitAsync();
                            response.EntityId = invitedUser.Id;
                            response.StatusCode = Status.Success;
                            response.EntityNumber = supplierURL;
                            await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(
                                                                      EventType.InvitedUser,
                                                                      response.EntityId,
                                                                      invitedUser.InvitedBy
                                                                      );
                        }
                        else
                        {
                            var thirdPartyInvite = SaveNewCompanyInvitation(viewModel);
                            thirdPartyInvite.InvitedByCompanyId = invitedCompanyId.Value;
                            Context.DataContext.ThirdPartyCompanyInvites.Add(thirdPartyInvite);
                            await Context.CommitAsync();
                            response.EntityId = thirdPartyInvite.Id;
                            response.StatusCode = Status.Success;
                            response.EntityNumber = supplierURL;
                            await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(
                                                                      EventType.InvitedNewUser,
                                                                      response.EntityId,
                                                                      (int)SystemUser.System
                                                                      );
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "Invalid invitation link.";
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitationDomain", "SaveThirdPartyInvitation", ex.Message, ex);
            }
            return response;
        }

        public async Task<OnboardingPreferenceViewModel> GetCarrierOnboardingForBranding(string token)
        {
            var response = new OnboardingPreferenceViewModel();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var invitedCompanyId = GetInvitatedCompanyIdByToken(token);
                    if (invitedCompanyId != null && invitedCompanyId.HasValue)
                    {
                        var brandingPreference = await Context.DataContext.OnboardingPreferences
                                                .Where(t => t.CompanyId == invitedCompanyId.Value && t.IsActive)
                                                .FirstOrDefaultAsync();

                        if (brandingPreference != null)
                        {
                            response.IsBrandMyWebsite = brandingPreference.IsBrandMyWebsite;
                            response.ButtonColor = brandingPreference.ButtonColor;
                            response.BackgroundColor = brandingPreference.BackgroundColor;
                            response.ForegroundColor = brandingPreference.ForegroundColor;
                            response.IconColor = brandingPreference.IconColor;
                            response.FontColor = brandingPreference.FontColor;
                            response.HeaderColor = brandingPreference.HeaderColor;

                            var imageViewModel = new ImageViewModel();
                            imageViewModel.FilePath = brandingPreference.ImageFilePath;
                            response.ImageFilePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);

                            imageViewModel = new ImageViewModel();
                            imageViewModel.FilePath = brandingPreference.FaviconImageFilePath;
                            response.FaviconFilePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);

                            imageViewModel = new ImageViewModel();
                            imageViewModel.FilePath = brandingPreference.CarrierOnboardingImageFilePath;
                            response.CarrierOnboardingImageFilePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitationDomain", "GetCarrierOnboardingForBranding", ex.Message, ex);
            }
            return response;
        }

        //GET WIZARD DATA
        public ThirdPartyCompanyInviteViewModel GetInvitedCompanyRawDataById(int id)
        {
            var response = new ThirdPartyCompanyInviteViewModel();
            try
            {
                var _response = Context.DataContext.ThirdPartyCompanyInvites.Where(t => t.Id == id).FirstOrDefault();
                if (_response != null)
                {
                    response = _response.ToViewModel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitationDomain", "GetInvitedCompanyRawDataById", ex.Message, ex);
            }
            return response;

        }

        public async Task<StatusViewModel> InvitedCompanyRegistered(int id, int registeredCompanyId)
        {
            var response = new StatusViewModel();
            try
            {
                var _response = await Context.DataContext.ThirdPartyCompanyInvites.Where(t => t.Id == id).FirstOrDefaultAsync();
                if (_response != null)
                {
                    _response.IsActive = false;
                    _response.IsInvitedCompanyRegistered = true;
                    _response.RegisteredCompanyId = registeredCompanyId;
                    Context.DataContext.Entry(_response).State = EntityState.Modified;
                    await Context.CommitAsync();
                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitationDomain", "SaveThirdPartyInvitation", ex.Message, ex);
            }
            return response;
        }

        public async Task<ThirdPartyCompanyInviteViewModel> GetInvitedCompanyDetails(int id)
        {
            var response = new ThirdPartyCompanyInviteViewModel();
            try
            {
                var _response = await Context.DataContext.ThirdPartyCompanyInvites.Where(t => t.Id == id).FirstOrDefaultAsync();
                if (_response != null)
                {
                    response = _response.ToOnboardViewModel();
                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitationDomain", "GetInvitedCompanyDetails", ex.Message, ex);
            }
            return response;
        }
        public List<ServiceArea> GetCitiesAndZipFromStates(string stateIds)
        {
            var response = new List<ServiceArea>();
            try
            {
                var stateId = new List<int>();
                if (!string.IsNullOrEmpty(stateIds))
                {
                    stateId = stateIds.Split(',').Select(int.Parse).ToList();
                    var listItem = Context.DataContext.MstCities.Where(t => t.IsActive && stateId.Contains(t.StateId))
                                                                            .Select(t => new
                                                                            {
                                                                                Id = t.Id,
                                                                                Name = t.Name,
                                                                                Code = t.ZipCodes,
                                                                                stateId = t.StateId
                                                                            }).ToList();
                    if (listItem != null && listItem.Any())
                    {
                        foreach (var item in listItem)
                        {
                            var zipCodes = item.Code != null ? item.Code.Split(',').ToList() : new List<string>();
                            foreach (var zip in zipCodes)
                            {
                                var itemDetails = new ServiceArea();
                                itemDetails.CityId = item.Id;
                                itemDetails.CityName = item.Name;
                                itemDetails.ZipCode = zip;
                                itemDetails.StateId = item.stateId;
                                response.Add(itemDetails);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitationDomain", "GetCitiesAndZipFromStates", ex.Message, ex);
            }
            return response;
        }

        public List<ServiceArea> GetCitiesAndZipFromStatesNew(string stateIds)
        {
            var response = new List<ServiceArea>();
            try
            {
                var stateId = new List<int>();
                if (!string.IsNullOrEmpty(stateIds))
                {
                    stateId = stateIds.Split(',').Select(int.Parse).ToList();
                    var listItem = Context.DataContext.MstCities.Where(t => t.IsActive && stateId.Contains(t.StateId))
                                                                            .Select(t => new
                                                                            {
                                                                                Id = t.Id,
                                                                                Name = t.Name,
                                                                                Code = t.ZipCodes,
                                                                                stateId = t.StateId
                                                                            }).ToList();
                    if (listItem != null && listItem.Any())
                    {
                        foreach (var item in listItem)
                        {
                            var zipCodes = item.Code != null ? item.Code.Split(',').ToList() : new List<string>();
                            foreach (var zip in zipCodes)
                            {
                                var itemDetails = new ServiceArea();
                                itemDetails.CityId = item.Id;
                                itemDetails.CityName = item.Name;
                                itemDetails.ZipCode = zip;
                                itemDetails.StateId = item.stateId;
                                response.Add(itemDetails);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitationDomain", "GetCitiesAndZipFromStates", ex.Message, ex);
            }
            return response;
        }
        public async Task<BooleanResponseModel> GetInvitationTokenByCompany(int id)
        {
            var response = new BooleanResponseModel();
            try
            {
                var IsThirdPartyEnabled = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == id && t.IsActive).Select(t => t.IsThirdPartyInvitationEnabled).FirstOrDefault();
                response.Result = IsThirdPartyEnabled;
                if (IsThirdPartyEnabled)
                {
                    var companyToken = await Context.DataContext.CompanyTokens.Where(t => t.CompanyId == id).FirstOrDefaultAsync();
                    if (companyToken != null)
                    {
                        response.Message = companyToken.Token;
                    }
                }
                response.Status = Status.Success;
            }
            catch (Exception ex)
            {
                response.Status = Status.Failed;
                LogManager.Logger.WriteException("InvitationDomain", "GetInvitationTokenByCompany", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> GenerateInvitationToken(int id)
        {
            var response = new StatusViewModel();
            try
            {
                //TOKEN ALREADY EXIST?
                var existingCompanyToken = await Context.DataContext.CompanyTokens.Where(t => t.CompanyId == id).FirstOrDefaultAsync();

                if (existingCompanyToken == null)
                {
                    var newCompanyToken = new CompanyToken
                    {
                        CompanyId = id,
                        Token = Guid.NewGuid().ToString(),
                        CreatedDate = new DateTimeOffset()
                    };

                    Context.DataContext.CompanyTokens.Add(newCompanyToken);
                    await Context.CommitAsync();

                    response.EntityNumber = newCompanyToken.Token;
                    response.StatusCode = Status.Success;
                }
                else
                {
                    response.EntityNumber = existingCompanyToken.Token;
                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitationDomain", "GenerateInvitationToken", ex.Message, ex);
            }
            return response;
        }

        public int? GetInvitatedCompanyIdByToken(string token)
        {
            int? response = null;
            try
            {
                var companyToken = Context.DataContext.CompanyTokens.Where(t => t.Token == token).FirstOrDefault();
                if (companyToken != null)
                {
                    response = companyToken.CompanyId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitationDomain", "GetInvitatedCompanyIdByToken", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<CarrierDetailsModel>> GetNonRegisteredInvitedCompanies(int invitedByCompanyId, ThirdPartyCompanyFilter filter)
        {
            var response = new List<CarrierDetailsModel>();

            try
            {
                var unRegisteredInvitedCompanies = await Context.DataContext.ThirdPartyCompanyInvites.Where(t => t.InvitedByCompanyId == invitedByCompanyId && !t.IsInvitedCompanyRegistered).OrderByDescending(t=>t.Id).ToListAsync();

                if (unRegisteredInvitedCompanies.Any())
                {
                    var filterServices = !string.IsNullOrEmpty(filter?.ServicesOffered) ? filter.ServicesOffered.Split(',').Select(x => (ServiceOfferingType)int.Parse(x)).ToList() : new List<ServiceOfferingType>();
                    var filterStates = !string.IsNullOrEmpty(filter?.States) ? filter.States.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>();
                    var filterZipCodes = !string.IsNullOrEmpty(filter?.ZipCodes) ? filter.ZipCodes.Split(',').ToList() : new List<string>();
                    var isTrailerHasPump = filter != null && filter.IsPump;
                    var isTrailerMetered = filter != null && filter.IsMetered;
                    var IsPackagedGoods = filter != null && filter.IsPackagedGoods;
                    var isAreaFilterEnable = filter != null && (filter.CountryId > 0 || filterServices.Any());
                    if (isAreaFilterEnable && filter.CountryId > 0 && !filterStates.Any())
                    {
                        filterStates = Context.DataContext.MstCountries.Where(t => t.Id == filter.CountryId).Select(t => t.MstStates.Select(t1 => t1.Id).ToList()).FirstOrDefault();
                    }
                    var thirdpartyCompanies = new List<ThirdPartyCompanyInviteViewModel>();

                    unRegisteredInvitedCompanies.ForEach(t => thirdpartyCompanies.Add(t.ToViewModel()));
                    var filteredcompanies = thirdpartyCompanies.Where(t => (!isAreaFilterEnable || t.ServiceOffering.Any(t2 =>
                                                                                                    (filter.CountryId == 0 ||
                                                                                                            ((!filterStates.Any() || t2.ServiceAreas.Any(t3 => filterStates.Contains(t3.StateId)))
                                                                                                            && (!filterZipCodes.Any() || t2.ServiceAreas.Any(t3 => filterZipCodes.Contains(t3.ZipCode)))))
                                                                                                    && (!filterServices.Any() || (t2.IsEnable && filterServices.Contains(t2.ServiceDeliveryType)))))
                                                                            && (!isTrailerHasPump || t.FleetInfo.DefAssets.Any(t1 => t1.TrailerHasPump) || t.FleetInfo.FuelAssets.Any(t1 => t1.TrailerHasPump))
                                                                            && (!isTrailerMetered || t.FleetInfo.DefAssets.Any(t1 => t1.IsTrailerMetered) || t.FleetInfo.FuelAssets.Any(t1 => t1.IsTrailerMetered))
                                                                            && (!IsPackagedGoods || t.FleetInfo.DefAssets.Any(t1 => t1.PackagedGoods))
                                                                       ).OrderByDescending(t => t.Id).ToList();

                    filteredcompanies.ForEach(t => response.Add(t.ToCarrierViewModel()));
                }
                   
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("invitationdomain", "GetNonRegisteredInvitedCompanies", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<CarrierDetailsModel>> GetRegisteredInvitedCompanies(int invitedByCompanyId, ThirdPartyCompanyFilter filter)
        {
            var response = new List<CarrierDetailsModel>();

            try
            {
                var registeredInvitedCompantIds = await Context.DataContext.ThirdPartyCompanyInvites.Where(t => t.InvitedByCompanyId == invitedByCompanyId && t.IsInvitedCompanyRegistered && t.RegisteredCompanyId > 0).Select(s => s.RegisteredCompanyId).ToListAsync();

                if (registeredInvitedCompantIds.Any())
                {
                    var filterServices = !string.IsNullOrEmpty(filter?.ServicesOffered) ? filter.ServicesOffered.Split(',').Select(x => (ServiceOfferingType)int.Parse(x)).ToList() : new List<ServiceOfferingType>();
                    var filterStates = !string.IsNullOrEmpty(filter?.States) ? filter.States.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>();
                    var filterZipCodes = !string.IsNullOrEmpty(filter?.ZipCodes) ? filter.ZipCodes.Split(',').ToList() : new List<string>();
                    var isTrailerHasPump = filter != null && filter.IsPump;
                    var isTrailerMetered = filter != null && filter.IsMetered;
                    var IsPackagedGoods = filter != null && filter.IsPackagedGoods;
                    var isAreaFilterEnable = filter != null && (filter.CountryId > 0 || filterServices.Any());
                    if (isAreaFilterEnable && filter.CountryId > 0 && !filterStates.Any())
                    {
                        filterStates = Context.DataContext.MstCountries.Where(t => t.Id == filter.CountryId).Select(t => t.MstStates.Select(t1 => t1.Id).ToList()).FirstOrDefault();
                    }
                    var registeredInvitedCompanies = await Context.DataContext.Companies.Where(t => registeredInvitedCompantIds.Contains(t.Id)
                                                                                                    && (!isAreaFilterEnable || t.CompanyAddresses.Any(t1 => t1.CompanyXServingLocations.Any(t2 =>
                                                                                                                                                                        (filter.CountryId == 0 || 
                                                                                                                                                                                ((!filterStates.Any() || filterStates.Contains(t2.StateId))
                                                                                                                                                                                && (!filterZipCodes.Any() || filterZipCodes.Contains(t2.ZipCode))))
                                                                                                                                                                        && (!filterServices.Any() || filterServices.Contains(t2.ServiceOfferingType)))))
                                                                                                    && (!isTrailerHasPump || t.FleetInformations.Any(t1 => t1.DoesTrailerHasPump))
                                                                                                    && (!isTrailerMetered || t.FleetInformations.Any(t1 => t1.IsTrailerMetered))
                                                                                                    && (!IsPackagedGoods || t.FleetInformations.Any(t1 => t1.IsPackagedGoods))
                                                                                                    )
                                                                                            .Select(t => new
                                                                                            {
                                                                                                t.Id,
                                                                                                t.Name,
                                                                                                User = t.Users.Select(t1 => new { t1.FirstName, t1.LastName, t1.Email }).FirstOrDefault(),
                                                                                                Address = t.CompanyAddresses.Where(t1 => t1.IsActive && t1.IsDefault).Select(t1 => new { t1.Address, t1.City, t1.PhoneNumber }).FirstOrDefault(),
                                                                                                ServiceLocationTypes = t.CompanyAddresses.Select(t1 => t1.CompanyXServingLocations.Select(t2 => t2.ServiceOfferingType).Distinct().ToList()).FirstOrDefault(),
                                                                                                Fleetinfo = t.FleetInformations.Select(t1 => new { t1.FleetType, t1.TrailerServiceType, t1.Count }).ToList()
                                                                                            }).OrderByDescending(t => t.Id).ToListAsync();


                    foreach (var company in registeredInvitedCompanies)
                    {
                        var companyDetails = new CarrierDetailsModel
                        {
                            Id = company.Id,
                            CompanyName = company.Name,
                            ContactInformation = $"{company.User.FirstName} {company.User.LastName}",
                            Email = company.User.Email,
                            PhoneNumber = company.Address?.PhoneNumber ?? @Resource.lblHyphen,
                            CompanyAddress = company.Address != null ? $"{company.Address.Address}, {company.Address.City}" : "",
                            ServiceOffered = string.Join(", ", company.ServiceLocationTypes.Select(t => t.GetDisplayName())),
                            DefTrailers = company.Fleetinfo.Where(t => t.FleetType == FleetType.DEF).Count(),
                            FtlTrailers = company.Fleetinfo.Where(t => t.FleetType != FleetType.DEF && t.TrailerServiceType == (int)FuelTrailerAssetType.FTL).Sum(t => t.Count),
                            LtlTrailers = company.Fleetinfo.Where(t => t.FleetType != FleetType.DEF && t.TrailerServiceType == (int)FuelTrailerAssetType.LTL).Sum(t => t.Count)
                        };
                        response.Add(companyDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("invitationdomain", "GetRegisteredInvitedCompanies", ex.Message, ex);
            }
            return response;
        }

        ////
        public async Task<ThirdPartyCompanyInviteViewModel> GetNonRegisteredInvitedCompany(int id)
        {
            var response = new ThirdPartyCompanyInviteViewModel();

            try
            {
                var company = await Context.DataContext.ThirdPartyCompanyInvites.Where(t => t.Id == id).FirstOrDefaultAsync();

                if (company != null)
                {
                    response = company.ToViewModel();
                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("invitationdomain", "GetNonRegisteredInvitedCompanies", ex.Message, ex);
            }
            return response;
        }

        public async Task<ThirdPartyCompanyInviteViewModel> GetRegisteredInvitedCompany(int companyId)
        {
            var response = new ThirdPartyCompanyInviteViewModel();

            try
            {
                var company = await Context.DataContext.Companies.Where(t => t.Id == companyId)
                                                                                    .Select(t => new
                                                                                    {
                                                                                        t.Id,
                                                                                        t.Name,
                                                                                        t.CompanyTypeId,
                                                                                        User = t.Users.Select(t1 => new { t1.FirstName, t1.LastName, t1.Email, t1.PhoneNumber }).FirstOrDefault(),
                                                                                        Address = t.CompanyAddresses.Where(t1 => t1.IsActive && t1.IsDefault).Select(t1 => new { t1.Address, t1.City, t1.StateId, t1.CountryId, t1.PhoneNumber, t1.ZipCode }).FirstOrDefault(),
                                                                                        Fleetinfo = t.FleetInformations.ToList(),
                                                                                        ServiceOffering = t.CompanyAddresses.Where(t1 => t1.IsDefault).Select(t1 => t1.CompanyXServingLocations.ToList()).FirstOrDefault(),
                                                                                    }).FirstOrDefaultAsync();


                if(company != null)
                {
                    response = new ThirdPartyCompanyInviteViewModel
                    {
                        Id = company.Id,
                        CompanyInfo = new CompanyInfo
                        {
                            CompanyAddress = company.Address != null ? company.Address.Address : "",
                            City = company.Address != null ? company.Address.City : "",
                            CompanyName = company.Name,
                            CompanyTypeId = company.CompanyTypeId,
                            StateId = company.Address != null ? company.Address.StateId : 0,
                            CountryId = company.Address != null ? company.Address.CountryId : 0,
                            Zip = company.Address?.ZipCode,
                            PhoneNumber = company.Address?.PhoneNumber
                        },
                        FleetInfo = company.Fleetinfo.ToViewModel(),
                        UserInfo = new UserInfo
                        {
                            FirstName = company.User.FirstName,
                            LastName = company.User.LastName,
                            Email = company.User.Email,
                        },
                        ServiceOffering = company.ServiceOffering.ToServiceViewModel(),
                    };
                }
                response.StatusCode = Status.Success;

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("invitationdomain", "GetRegisteredInvitedCompanies", ex.Message, ex);
            }
            return response;
        }
        ///
        public InvitedUser SaveExistingUserInvitation(ThirdPartyCompanyInviteViewModel viewModel)
        {
            var thirdPartyUserInvite = viewModel.ToExistingEntity();
            return thirdPartyUserInvite;
        }
        public ThirdPartyCompanyInvites SaveNewCompanyInvitation(ThirdPartyCompanyInviteViewModel viewModel)
        {
            var thirdPartyInvite = viewModel.ToEntity();
            return thirdPartyInvite;
        }
    }
}
