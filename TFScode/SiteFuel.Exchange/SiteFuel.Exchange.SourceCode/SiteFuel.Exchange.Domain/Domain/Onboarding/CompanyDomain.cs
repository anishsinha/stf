using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class CompanyDomain : BaseDomain
    {
        public CompanyDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public CompanyDomain(BaseDomain domain) : base(domain)
        {
        }

        public readonly Decimal OneLitterEqualsToOneGallon = 0.264M;
        public readonly Decimal OneGallonEqualsToOneLitter = 3.785M;
        public async Task<bool> IsCompanyExist(string companyName, int companyId = 0, bool checkIsActive = false)
        {
            var response = false;
            try
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    return response;

                if (checkIsActive)
                    response = await Context.DataContext.Companies.AnyAsync(t => t.Id != companyId && t.Name.ToLower() == companyName.ToLower() && t.IsActive && !t.IsDeleted);
                else
                    response = await Context.DataContext.Companies.AnyAsync(t => t.Id != companyId && t.Name.ToLower() == companyName.ToLower() && !t.IsDeleted);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "IsCompanyExist", ex.Message + " companyId: " + companyId + " companyName: " + companyName, ex);
            }
            return response;
        }

        public bool IsDispatcherExists(int companyId)
        {
            return Context.DataContext.Users.Any(t => t.CompanyId == companyId && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Dispatcher));
        }

        public List<int> MultiCountrySupportForBuyer(int companyId)
        {
            var resp = Context.DataContext.Jobs.Where(x => x.CompanyId == companyId).Select(x => x.CountryId).Distinct().ToList();
            if (!resp.Any())
            {
                return Context.DataContext.CompanyAddresses.Where(x => x.CompanyId == companyId).Select(x => x.CountryId).Distinct().ToList();
            }
            else
            {
                return resp;
            }
        }

        public List<int> MultiCountrySupportForSupplier(int companyId)
        {
            var resp = Context.DataContext.CompanyAddresses.Where(x => x.CompanyId == companyId).Select(x => x.CountryId).Distinct().ToList();
            if (!resp.Any())
            {
                resp.Add(1);
            }
            return resp;
        }

        public bool IsExternalCompanyExist(string companyName, int companyId = 0)
        {
            var response = false;
            try
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    return response;

                response = Context.DataContext.Companies.Any(t => t.Id != companyId && t.Name.ToLower() == companyName.ToLower() && t.IsActive);
                if (!response)
                {
                    response = Context.DataContext.ExternalSuppliers.Any(t => t.Id != companyId && t.Name.ToLower() == companyName.ToLower() && t.IsActive);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "IsExternalCompanyExist", ex.Message + " companyId: " + companyId + " companyName: " + companyName, ex);
            }
            return response;
        }

        public int GetCompanyTypeId(int companyId)
        {
            var response = 0;
            try
            {
                response = Context.DataContext.Companies.Where(t => t.Id == companyId).First().CompanyTypeId;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetCompanyTypeId", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> IsOnboardedCompany(string companyName)
        {
            var response = true;
            try
            {
                var tpoCompanies = await GetTpoCompaniesById(0); // get all tpo companies
                var companyIds = tpoCompanies.Select(t => t.Id);
                response = Context.DataContext.Companies.Any(t => t.Name.Equals(companyName) && !companyIds.Contains(t.Id) && !t.IsDeleted);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "IsOnboardedCompany", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<CompanyViewModel>> GetTpoCompaniesById(int companyId)
        {
            List<CompanyViewModel> companies = new List<CompanyViewModel>();
            try
            {
                var orders = await Context.DataContext.Orders
                                                .Where(t => t.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest &&
                                                            (t.AcceptedCompanyId == companyId || companyId == 0)).Select(t => new { t.BuyerCompanyId, BuyerCompanyName = t.BuyerCompany.Name })
                                                .GroupBy(p => p.BuyerCompanyId).Select(grp => grp.FirstOrDefault()).ToListAsync();

                orders.ForEach(t => companies.Add(new CompanyViewModel()
                {
                    Id = t.BuyerCompanyId,
                    Name = t.BuyerCompanyName
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetCompanyTypeId", ex.Message, ex);
            }
            return companies;
        }

        public async Task<List<TPOCustomerViewModel>> GetUsersByCompany(int companyId)
        {
            List<TPOCustomerViewModel> userList = new List<TPOCustomerViewModel>();
            try
            {
                var users = await Context.DataContext.Users.Include(t => t.UserXNotificationSettings)
                                                     .Where(t => t.CompanyId == companyId).ToListAsync();
                string eventTypeDelivery = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingNotifyDeliveries);
                string eventTypeSchedule = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingNotifySchedules);
                var eventTypeDeliveryIds = eventTypeDelivery.TrimStart(',').Split(',');
                var eventTypeScheduleIds = eventTypeSchedule.TrimStart(',').Split(',');

                userList = (from u in users
                            select new TPOCustomerViewModel()
                            {
                                UserId = u.Id,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                FullName = $"{u.FirstName} {u.LastName}",
                                Email = u.Email,
                                PhoneNumber = u.PhoneNumber,
                                IsInvitationEnabled = Context.DataContext.Notifications.Any(t => t.EntityId == u.Id && t.EventTypeId == (int)EventType.TPOUserInvitedForEULAAcceptance),
                                IsNotifyDeliveries = u.UserXNotificationSettings.Any(t => t.UserId == u.Id && eventTypeDeliveryIds.Contains(t.EventTypeId.ToString()) && t.IsEmail),
                                IsNotifySchedules = u.UserXNotificationSettings.Any(t => t.UserId == u.Id && eventTypeScheduleIds.Contains(t.EventTypeId.ToString()) && t.IsEmail),
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetUsersByCompany", ex.Message, ex);
            }
            return userList;
        }

        public async Task<MemoryStream> GetAtlasOilReportCsvStream(int companyId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            MemoryStream result = null;
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var report = spDomain.GetAtlasOilReport(companyId, startDate, endDate);
                var csvReport = report.Select(t => t.ToCsvViewModel());
                result = await GetCsvAsMemoryStream(csvReport);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetAtlasOilReportCsvStream", ex.Message, ex);
            }
            return result;
        }

        public async Task<MemoryStream> GetAtlasOilCarrierReportCsvStream(int companyId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            MemoryStream result = null;
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var report = spDomain.GetAtlasOilCarrierReport(companyId, startDate, endDate);
                var csvReport = report.Select(t => t.ToCsvViewModel());
                result = await GetCsvAsMemoryStream(csvReport);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetAtlasOilCarrierReportCsvStream", ex.Message, ex);
            }
            return result;
        }

        private async Task<MemoryStream> GetCsvAsMemoryStream(IEnumerable<AtlasOilCsvOutputViewModel> transactionsArray)
        {
            var memoryStream = new MemoryStream();
            var flatFileWriter = new StreamWriter(memoryStream, Encoding.ASCII);
            var fileWriterEngine = new FileHelperEngine(typeof(AtlasOilCsvOutputViewModel));

            fileWriterEngine.HeaderText = Resource.AtlasOilCsvHeaderText;
            fileWriterEngine.WriteStream(flatFileWriter, transactionsArray);

            // Flush contents of fileWriterStream to underlying docStream:
            await flatFileWriter.FlushAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        private async Task<MemoryStream> GetCsvAsMemoryStream(IEnumerable<AtlasOilCarrierCsvOutputViewModel> transactionsArray)
        {
            var memoryStream = new MemoryStream();
            var flatFileWriter = new StreamWriter(memoryStream, Encoding.ASCII);
            var fileWriterEngine = new FileHelperEngine(typeof(AtlasOilCarrierCsvOutputViewModel));

            fileWriterEngine.HeaderText = Resource.AtlasOilCarrierCsvHeaderText;
            fileWriterEngine.WriteStream(flatFileWriter, transactionsArray);

            // Flush contents of fileWriterStream to underlying docStream:
            await flatFileWriter.FlushAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public async Task<List<DropdownDisplayItem>> GetCompanyServingStates(int companyId, int countryId)
        {
            List<DropdownDisplayItem> servingStates = new List<DropdownDisplayItem>();
            try
            {
                var states = await Context.DataContext.CompanyAddresses
                        .Where(t => t.CompanyId == companyId && t.IsActive && t.MstCountry.Id == countryId).Select(t1 => t1.MstStates).ToListAsync();

                foreach (var state in states)
                {
                    foreach (var servingstate in state)
                    {
                        if (!servingStates.Any(t => t.Id == servingstate.Id))
                        {
                            servingStates.Add(new DropdownDisplayItem() { Id = servingstate.Id, Name = servingstate.Name });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetCompanyServingStates", ex.Message, ex);
            }
            return servingStates;
        }

        public async Task<StatusViewModel> AddServingStateAsync(UserContext userContext, int countryId, int stateId)
        {
            var response = new StatusViewModel(Status.Success);
            var isStateExistsInServingList = Context.DataContext.CompanyAddresses
                        .Any(t => t.CompanyId == userContext.CompanyId && t.IsActive && t.MstCountry.Id == countryId && t.MstStates.Any(t1 => t1.Id == stateId));

            if (!isStateExistsInServingList)
            {
                try
                {
                    //add this stat as new serving state to default address
                    var defaultAddress = Context.DataContext.CompanyAddresses.FirstOrDefault(t => t.CompanyId == userContext.CompanyId && t.CountryId == countryId && t.IsActive);
                    if (defaultAddress != null)
                    {
                        var state = Context.DataContext.MstStates.SingleOrDefault(t => t.Id == stateId);
                        defaultAddress.MstStates.Add(state);
                        Context.DataContext.Entry(defaultAddress).State = EntityState.Modified;
                        await Context.CommitAsync();

                        response.StatusMessage = Resource.SuccessServingStateAddedToYourAddress;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.ErrorNoAddressFoundInCounty;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("CompanyDomain", "AddServingStateAsync", ex.Message + "companyId - " + userContext.CompanyId, ex);
                }
            }

            return response;
        }

        public bool IsFeatureEnabledForCompany(int companyId, CompanyType companyTypeId, FeatureTypes feature)
        {
            var response = false;
            try
            {
                response = Context.DataContext.CompanyFeatures.Any(t => t.CompanyId == companyId
                                                && t.MstFeature.CompanyTypeId == (int)companyTypeId && t.MstFeature.Name.Replace(" ", "").ToLower() == feature.ToString().ToLower() && t.IsActive);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "IsFeatureEnabledForCompany", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<FeaturesViewModel>> GetFeaturesAsync(int companyId, CompanyType companyType)
        {
            var response = new List<FeaturesViewModel>();
            try
            {
                response = await (from FTR in Context.DataContext.MstFeatures
                                  join CFTR in Context.DataContext.CompanyFeatures on FTR.Id equals CFTR.FeatureId into temp
                                  from f in temp.Where(t => t == null || t.CompanyId == companyId).DefaultIfEmpty()
                                  where (FTR.CompanyTypeId == (int)companyType) && FTR.IsActive
                                  orderby FTR.Id
                                  select new FeaturesViewModel { Id = FTR.Id, Name = FTR.Name, Code = FTR.Name.Replace(" ", ""), IsEnabled = f == null ? false : f.IsActive, CompanyType = (CompanyType)FTR.CompanyTypeId, Description = "" }
                                 ).ToListAsync();

                if (response != null && response.Count > 0)
                {
                    response.ForEach(t => t.Description = GetFeatureSettingDesription(t.Code));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetFeaturesAsync", ex.Message, ex);
            }

            return response;
        }

        private string GetFeatureSettingDesription(string featureCode)
        {
            string description = null;
            featureCode = featureCode.Replace(" ", "").ToLower();
            try
            {
                switch (featureCode.ToLower())
                {
                    case "quickbooks":
                        description = Resource.featureDescriptionQuickbooks;
                        break;
                    case "manageoffer":
                        description = Resource.featureDescriptionManageOffer;
                        break;
                    case "requestforquotes":
                        description = Resource.featureDescriptionRequestForQuotes;
                        break;
                    case "timecard":
                        description = Resource.featureDescriptionTimeCard;
                        break;
                    case "accountgroup":
                        description = Resource.featureDescriptionAccountGroup;
                        break;
                    case "managesmsalerts":
                        description = Resource.featureDescriptionManageSMSAlerts;
                        break;
                    case "productmapping":
                        description = Resource.featureDescriptionProductMapping;
                        break;
                    case "messages":
                        description = Resource.featureDescriptionMessages;
                        break;
                    case "apidashboard":
                        description = Resource.featureDescriptionApiDashboard;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetFeatureSettingDesription", ex.Message, ex);
            }

            return description;
        }

        public StatusViewModel UpdateFeatureSettings(UserContext userContext, int featureId, bool isFeatureEnable)
        {
            var response = new StatusViewModel();
            try
            {
                var companyFeature = Context.DataContext.CompanyFeatures.FirstOrDefault(t => t.FeatureId == featureId && t.CompanyId == userContext.CompanyId);
                if (companyFeature != null)
                {
                    companyFeature.IsActive = isFeatureEnable;
                    companyFeature.UpdatedBy = userContext.Id;
                    companyFeature.UpdatedDate = DateTimeOffset.Now;

                    Context.DataContext.Entry(companyFeature).State = EntityState.Modified;
                }
                else
                {
                    companyFeature = new CompanyFeature()
                    {
                        CompanyId = userContext.CompanyId,
                        FeatureId = featureId,
                        IsActive = isFeatureEnable,
                        UpdatedBy = userContext.Id
                    };
                    Context.DataContext.CompanyFeatures.Add(companyFeature);
                }
                Context.Commit();
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageFeatureUpdateSuccessfully;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "UpdateFeatureSettingAsync", ex.Message, ex);
            }

            return response;
        }

        public List<int> ServingCountry(int companyId, CompanyType companyType, CompanyType companySubType)
        {
            if (companyType == CompanyType.Buyer || companySubType == CompanyType.Buyer)
            {
                return MultiCountrySupportForBuyer(companyId);
            }
            else
            {
                return MultiCountrySupportForSupplier(companyId);
            }
        }

        public async Task<List<DropdownDisplayItem>> GetCarriers(int supplierCompanyId)
        {
            return await Context.DataContext.Companies.Where(t => t.Id != supplierCompanyId && (t.CompanyTypeId == (int)CompanyType.BuyerSupplierAndCarrier ||
                                                                                        t.CompanyTypeId == (int)CompanyType.Carrier ||
                                                                                        t.CompanyTypeId == (int)CompanyType.SupplierAndCarrier) && t.IsActive)
                                                            .OrderByDescending(t => t.Id)
                                                            .Select(t => new DropdownDisplayItem() { Id = t.Id, Name = t.Name })
                                                            .ToListAsync();
        }


        public async Task<List<CarrierJobViewModel>> GetJobsForSupplier(int supplierCompanyId)
        {
            return await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == supplierCompanyId)
                                                            .GroupBy(t => t.FuelRequest.JobId)
                                                            .Select(t => t.FirstOrDefault())
                                                            .OrderByDescending(t => t.FuelRequest.JobId)
                                                            .Select(t => new CarrierJobViewModel { Job = new JobWithEmails() { Id = t.FuelRequest.JobId, Name = t.FuelRequest.Job.Name }, BuyerCompanyId = t.FuelRequest.Job.CompanyId, BuyerCompanyName = t.FuelRequest.Job.Company.Name })
                                                            .OrderBy(t => t.Job.Name)
                                                            .ToListAsync();
        }

        public async Task<OnboardingPreferenceViewModel> GetPreferencesSettingAsync(int id, UserContext userContext, int brandedCompanyId = -1)
        {
            OnboardingPreferenceViewModel response = new OnboardingPreferenceViewModel();
            try
            {
                OnboardingPreference onboardingPreference = null;
                if (brandedCompanyId > 0)
                {
                    onboardingPreference = await Context.DataContext.OnboardingPreferences.Include(t => t.BuyerXOnboardingPreferences)
                                                        .Where(t => (id > 0 && t.Id == id) ||
                                                                   (t.IsActive && t.CompanyId == brandedCompanyId))
                                                       .OrderByDescending(t => t.Id)
                                                       .FirstOrDefaultAsync();
                }
                else
                {
                    onboardingPreference = await Context.DataContext.OnboardingPreferences.Include(t => t.BuyerXOnboardingPreferences)
                                                  .Where(t => (id > 0 && t.Id == id) ||
                                                             (t.IsActive && t.CompanyId == userContext.CompanyId))
                                                 .OrderByDescending(t => t.Id)
                                                 .FirstOrDefaultAsync();
                }
                if (onboardingPreference != null)
                {

                    response = onboardingPreference.ToViewModel();
                    var appDomain = new ApplicationDomain(this);
                    if (response.RetainThreshold <= 0)
                    {
                        response.RetainThreshold = Convert.ToInt32(appDomain.GetKeySettingValue(ApplicationConstants.KeyOtherCountryRetainThreshold, ""));
                        response.UOM = (int)RetainThresholdUoM.Gallons;

                        var companyAddress = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanyAddressAsync(userContext.CompanyId);
                        if (companyAddress != null && companyAddress.Id > 0)
                        {
                            if (companyAddress.Country.Code == Country.CAN.ToString())
                            {
                                response.RetainThreshold = Convert.ToInt32(appDomain.GetKeySettingValue(ApplicationConstants.KeyCanadaRetainThreshold, ""));
                                response.UOM = (int)RetainThresholdUoM.Litres;
                            }
                        }
                    }
                    if (response.UOM == (int)RetainThresholdUoM.Gallons)
                        response.RetainThresholdValueConvertion = string.Format("{0} L", response.RetainThreshold * ApplicationConstants.OneGallonEqualsToOneLitter);
                    else
                        response.RetainThresholdValueConvertion = string.Format("{0} G", response.RetainThreshold * ApplicationConstants.OneLitterEqualsToOneGallon);

                    if (response.IsLiftFileValidationEnabled)
                    {
                        var inputParameters = onboardingPreference.LiftFileValidationParameters.Where(t => t.ParameterType == LFVParameterType.Input && t.IsActive).FirstOrDefault();
                        var outputParameters = onboardingPreference.LiftFileValidationParameters.Where(t => t.ParameterType == LFVParameterType.Output && t.IsActive).FirstOrDefault();
                        if (inputParameters != null)
                        {
                            response.LfvInputParameter = inputParameters.ToViewModel();
                        }
                        if (outputParameters != null)
                            response.LfvOutputParameter = outputParameters.ToViewModel();
                    }
                    //response.IsProductSequencingEnable = true;
                    if (response.IsProductSequencingEnabled)
                    {
                        response.ProductSequencing = await GetProductSequence(userContext.CompanyId, ProductSequencingCreationMethod.Account);
                    }

                    response.LoadQueueAttributes = GetLoadQueueAttributes(response.LoadQueueAttributesValue);
                    response.DRQueueAttributes = GetDRQueueAttributes(response.DRQueueAttributesValue);

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                var brandingPreference = await Context.DataContext.OnboardingPreferences.Where(t => t.IsActive && (t.CompanyId == userContext.CompanyId))
                                                    .OrderByDescending(t => t.Id)
                                                    .FirstOrDefaultAsync();
                if (brandingPreference != null)
                {
                    response.IsBrandMyWebsite = brandingPreference.IsBrandMyWebsite;
                    response.ImageFilePath = brandingPreference.ImageFilePath;
                    response.URLName = brandingPreference.URLName;
                    response.BackgroundImageFilePath = brandingPreference.BackgroundImageFilePath;
                    response.FaviconFilePath = brandingPreference.FaviconImageFilePath;
                    response.CarrierOnboardingImageFilePath = brandingPreference.CarrierOnboardingImageFilePath;
                }
                if (onboardingPreference != null)
                {
                    var carrierDeliveryXUserSettings = await Context.DataContext.CarrierDeliveryXUserSettings.Where(t => t.CompanyId == onboardingPreference.CompanyId).Select(x => x.UserId).ToListAsync();
                    if (carrierDeliveryXUserSettings.Any())
                    {
                        response.CarrierUsers = carrierDeliveryXUserSettings;
                    }
                }
                if (onboardingPreference != null)
                {
                    response.IsSelectedIdentityProvider = false;
                    var selectedIDP = await Context.DataContext.CompanyIdentityServices
                        .Where(t => t.CompanyId == onboardingPreference.CompanyId && t.IsActive && t.IsAvailable).SingleOrDefaultAsync();
                    if (selectedIDP != null)
                    {
                        response.SelectedIdentityProvider = selectedIDP.IdentityProviderId;
                        response.IsSelectedIdentityProvider = true;
                    }
                }
                if (response.IsLoadOptimization)
                {
                    response.LoadOptimizationUsers = await GetLoadOptimizationUsers(userContext.CompanyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetPreferencesSettingAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<ProductSequenceViewModel> GetProductSequence(int companyId, ProductSequencingCreationMethod sequenceMethod, int jobId = 0)
        {
            ProductSequenceViewModel response = new ProductSequenceViewModel() { SequenceType = ProductSequenceType.Product, SequenceMethod = sequenceMethod, JobId = jobId };
            try
            {
                var productSequence = await Context.DataContext.SupplierXProductSequencing
                                            .Where(t => t.SupplierCompanyId == companyId
                                                        && t.IsActive
                                                        && (!t.JobId.HasValue || t.JobId.Value == jobId))
                                            .Select(t => new
                                            {
                                                ProductTypeId = t.ProductId,
                                                Sequence = t.SequenceNumber,
                                                OrderId = t.OrderId,
                                                JobId = t.JobId,
                                                DisplayName = t.OrderId.HasValue ? (t.Order.PoNumber + " - " + t.Order.FuelRequest.MstProduct.MstProductType.Name) : t.MstProductType.Name
                                            })
                                            .OrderBy(t => t.Sequence)
                                            .ToListAsync();

                if (productSequence != null)
                {
                    var JobSequence = productSequence.Where(t => t.JobId.HasValue && jobId > 0 && t.JobId.Value == jobId)
                                            .Select(t => new ProductSequenceModel
                                            {
                                                ProductTypeId = t.ProductTypeId,
                                                OrderId = t.OrderId,
                                                DisplayName = t.DisplayName,
                                                Sequence = t.Sequence
                                            })
                                            .ToList();
                    if (sequenceMethod == ProductSequencingCreationMethod.Job && JobSequence != null && JobSequence.Any())
                    {
                        response.ProductSequence = JobSequence;
                    }
                    else
                    {
                        response.ProductSequence = productSequence.Where(t => !t.JobId.HasValue)
                                            .Select(t => new ProductSequenceModel
                                            {
                                                ProductTypeId = t.ProductTypeId,
                                                OrderId = t.OrderId,
                                                DisplayName = t.DisplayName,
                                                Sequence = t.Sequence
                                            })
                                            .ToList();
                        if (sequenceMethod == ProductSequencingCreationMethod.Job)
                        {
                            var jobProducts = await GetProductSequenceTypes(companyId, ProductSequencingCreationMethod.Job, ProductSequenceType.Product, jobId);
                            var jobProductIds = jobProducts.Select(t => t.Id).ToList();
                            response.ProductSequence = response.ProductSequence.Where(t => t.ProductTypeId.HasValue && jobProductIds.Contains(t.ProductTypeId.Value)).ToList();
                        }
                    }

                    response.ProductIds = response.ProductSequence.Where(t => t.ProductTypeId.HasValue).Select(t => t.ProductTypeId.Value).ToList();
                    if (response.ProductSequence.Any(t => t.OrderId.HasValue))
                    {
                        response.SequenceType = ProductSequenceType.Order;
                        response.ProductIds = response.ProductSequence.Select(t => t.OrderId.Value).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetProductSequence", ex.Message, ex);
            }
            return response;
        }
        public async Task<DisplaySequenceModel> GetProductDisplaySequenceTypes(int supplierCompanyId, ProductSequencingCreationMethod sequenceMethod, ProductSequenceType sequenceType, int jobId)
        {
            var response = new DisplaySequenceModel();
            try
            {
                response.DisplayListSeq = await GetProductSequenceTypes(supplierCompanyId, sequenceMethod, sequenceType, jobId);
                var updatedSeq = await GetProductSequence(supplierCompanyId, sequenceMethod, jobId);
                if (updatedSeq != null && updatedSeq.SequenceType == sequenceType)
                {
                    if (updatedSeq.SequenceType == ProductSequenceType.Order)
                    {
                        updatedSeq.ProductSequence.ForEach(t => { response.SelectedSeq.Add(new DropdownDisplayExtendedId { Id = t.OrderId ?? 0, Name = t.DisplayName }); });
                    }
                    else
                    {
                        updatedSeq.ProductSequence.ForEach(t => { response.SelectedSeq.Add(new DropdownDisplayExtendedId { Id = t.ProductTypeId ?? 0, Name = t.DisplayName }); });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetProductDisplaySequenceTypes", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayExtendedId>> GetProductSequenceTypes(int supplierCompanyId, ProductSequencingCreationMethod sequenceMethod, ProductSequenceType sequenceType, int jobId)
        {
            List<DropdownDisplayExtendedId> response = new List<DropdownDisplayExtendedId>();
            try
            {
                if (sequenceMethod == ProductSequencingCreationMethod.Account)
                {
                    response = await Context.DataContext.MstProductTypes.Where(t => t.IsActive)
                                                         .Select(t => new DropdownDisplayExtendedId() { Id = t.Id, CodeId = t.Id, Name = t.Name })
                                                         .ToListAsync();
                }
                else
                {
                    if (sequenceType == ProductSequenceType.Order)
                    {
                        response = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == supplierCompanyId && t.IsActive
                                                                     && t.FuelRequest.JobId == jobId && t.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open))
                                                         .Select(t => new DropdownDisplayExtendedId() { Id = t.Id, CodeId = t.FuelRequest.MstProduct.MstProductType.Id, Name = t.PoNumber + " - " + t.FuelRequest.MstProduct.MstProductType.Name })
                                                         .Distinct()
                                                         .ToListAsync();

                    }
                    else
                    {
                        response = await Context.DataContext.JobXAssets
                                            .Where(t => t.RemovedBy == null && t.JobId == jobId && t.Asset.Type == (int)AssetType.Tank && t.Asset.IsActive)
                                            .Select(t => new DropdownDisplayExtendedId() { Id = t.Asset.MstProductType.Id, Name = t.Asset.MstProductType.Name })
                                            .Distinct()
                                            .ToListAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetProductSequenceTypes", ex.Message, ex);
            }
            return response;
        }

        public async Task<OnboardingPreferenceViewModel> GetPreferencesSettingBySupplierAsync(int supplierCompanyId)
        {
            OnboardingPreferenceViewModel response = new OnboardingPreferenceViewModel();
            try
            {
                var onboardingPreference = await Context.DataContext.OnboardingPreferences
                    .Where(t => t.IsActive && t.CompanyId == supplierCompanyId)
                                                    .OrderByDescending(t => t.Id)
                                                    .FirstOrDefaultAsync();
                if (onboardingPreference != null)
                {
                    response = onboardingPreference.ToViewModel();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetPreferencesSettingAsync", ex.Message, ex);
            }
            return response;
        }

        private List<int> GetLoadQueueAttributes(string loadQueueAttributes)
        {
            var response = new List<int>();
            if (!string.IsNullOrEmpty(loadQueueAttributes))
            {
                var viewModel = JsonConvert.DeserializeObject<LoadQueueAttributesViewModel>(loadQueueAttributes);
                if (viewModel.CustomerName)
                {
                    response.Add((int)LoadQueueAttributes.CustomerName);
                }
                if (viewModel.Driver)
                {
                    response.Add((int)LoadQueueAttributes.Driver);
                }
                if (viewModel.LocationName)
                {
                    response.Add((int)LoadQueueAttributes.LocationName);
                }
                if (viewModel.TrailerName)
                {
                    response.Add((int)LoadQueueAttributes.TrailerName);
                }
            }
            else
            {
                response.Add((int)LoadQueueAttributes.CustomerName);
                response.Add((int)LoadQueueAttributes.Driver);
                response.Add((int)LoadQueueAttributes.LocationName);
                response.Add((int)LoadQueueAttributes.TrailerName);
            }

            return response;
        }

        private List<int> GetDRQueueAttributes(string drQueueAttributes)
        {
            var response = new List<int>();
            if (!string.IsNullOrEmpty(drQueueAttributes))
            {
                var viewModel = JsonConvert.DeserializeObject<DRQueueAttributesViewModel>(drQueueAttributes);
                if (viewModel.CustomerName)
                {
                    response.Add((int)DRQueueAttributes.CustomerName);
                }
                if (viewModel.DeliveryShift)
                {
                    response.Add((int)DRQueueAttributes.DeliveryShift);
                }
                if (viewModel.TrailerCompatibility)
                {
                    response.Add((int)DRQueueAttributes.TrailerCompatibility);
                }
            }
            else
            {
                response.Add((int)DRQueueAttributes.CustomerName);
                response.Add((int)DRQueueAttributes.DeliveryShift);
                response.Add((int)DRQueueAttributes.TrailerCompatibility);
            }

            return response;
        }
        public async Task<OnboardingPreferenceViewModel> SavePreferencesSetting(OnboardingPreferenceViewModel viewModel, UserContext userContext)
        {
            if (viewModel == null)
            {
                viewModel = new OnboardingPreferenceViewModel();
                viewModel.StatusCode = Status.Failed;
                viewModel.StatusMessage = Status.Failed.ToString();
                return viewModel;
            }

            if (viewModel.IsUnscheduledDeliveryAllowed && viewModel.IsCustomUnScheduleDelivery)
            {
                if (viewModel.DeliveryDays == null || viewModel.DeliveryDays.Count <= 0)
                {
                    viewModel.StatusCode = Status.Failed;
                    viewModel.StatusMessage = Resource.errMessageDayCantEmpty;
                    return viewModel;
                }

                if (viewModel.ShiftEndTime == null || viewModel.ShiftStartTime == null)
                {
                    viewModel.StatusCode = Status.Failed;
                    viewModel.StatusMessage = Resource.errMessageShiftCantEmpty;
                    return viewModel;
                }

                if (viewModel.IsCustomUnScheduleDelivery && (Convert.ToDateTime(viewModel.ShiftStartTime).TimeOfDay > Convert.ToDateTime(viewModel.ShiftEndTime).TimeOfDay))
                {
                    viewModel.StatusCode = Status.Failed;
                    viewModel.StatusMessage = Resource.errMessageShiftEndTimeGreaterThan;
                    return viewModel;
                }

                if (viewModel.DeliveryDays != null)
                {
                    viewModel.DeliveryDaysInString = string.Join<int>(",", viewModel.DeliveryDays);
                }
            }
            else
            {
                viewModel.IsCustomUnScheduleDelivery = false;
                viewModel.DeliveryDaysInString = null;
                viewModel.ShiftStartTime = Resource.errMessageDefaultZeroValue;
                viewModel.ShiftEndTime = Resource.errMessageDefaultZeroValue;

            }

            var loadQueueAttributes = GetLoadQueueAttributes(viewModel.LoadQueueAttributes);
            viewModel.LoadQueueAttributesValue = JsonConvert.SerializeObject(loadQueueAttributes);

            var drQueueAttributes = GetDRQueueAttributes(viewModel.DRQueueAttributes);
            viewModel.DRQueueAttributesValue = JsonConvert.SerializeObject(drQueueAttributes);

            using (var trasaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await Context.DataContext.Users.Where(t => t.Id == userContext.Id && t.CompanyId == userContext.CompanyId).FirstOrDefaultAsync();
                    if (user == null)
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = Resource.errorMessageUserNotFound;
                        return viewModel;
                    }

                    if (viewModel.IsLoadOptimization)
                    {
                        var _result = await SaveLoadOptimizationUsers(viewModel.LoadOptimizationUsers, userContext);
                        
                        if (_result.StatusCode != Status.Success)
                        {
                            viewModel.StatusCode = Status.Failed;
                            return viewModel;
                        }
                    }

                    var existingPreferences = await Context.DataContext.OnboardingPreferences.Where(t => t.IsActive && t.CompanyId == userContext.CompanyId)
                                                                      .OrderByDescending(t => t.Id)
                                                                      .ToListAsync();
                    if (existingPreferences != null)
                    {
                        foreach (var existingPreference in existingPreferences)
                        {
                            existingPreference.IsActive = false;
                            Context.DataContext.Entry(existingPreference).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }
                    }
                    viewModel.CreatedBy = userContext.Id;
                    viewModel.CreatedDate = DateTimeOffset.Now;

                    var response = await ContextFactory.Current.GetDomain<OnboardingDomain>().SavePreferencesSetting(viewModel, userContext, user);
                    if (response.StatusCode == Status.Success)
                    {
                        await Context.CommitAsync();
                        trasaction.Commit();
                        await SaveProductSequence(viewModel.ProductSequencing, userContext);
                        await SaveCarrierDeliveryXUserSetting(viewModel);
                        await SaveIdentityProviderSetting(viewModel);
                        response.EntityId = user.Company.OnboardingPreferences.Count() > 0 ? user.Company.OnboardingPreferences.OrderByDescending(top => top.Id).FirstOrDefault().Id : 0;
                    }
                    else
                    {
                        trasaction.Rollback();
                    }
                    return response;
                }
                catch (Exception ex)
                {
                    trasaction.Rollback();
                    LogManager.Logger.WriteException("CompanyDomain", "SavePreferencesSetting", ex.Message, ex);
                }
            }

            return viewModel;
        }

        public async Task<StatusViewModel> SaveLoadOptimizationUsers(List<int> users, UserContext userContext)
        {
            var response = new StatusViewModel();

            try
            {
                var result = await Context.DataContext.LoadOptimizationUsers.Where(x => x.CompanyId == userContext.CompanyId).FirstOrDefaultAsync();

                if (result != null)
                {
                    result.CreatedBy = userContext.Id;
                    result.CreatedDate = DateTimeOffset.Now;
                    result.DistributedUsers = users.AnyAndNotNull() ? JsonConvert.SerializeObject(users) : string.Empty ;

                    Context.DataContext.Entry(result).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                else 
                {
                    var viewModel = new LoadOptimizationUserViewModel();
                    viewModel.DistributedUsers = users;
                    
                    var entity = viewModel.ToEntity(userContext.CompanyId, userContext.Id);
                    Context.DataContext.LoadOptimizationUsers.Add(entity);
                    await Context.CommitAsync();
                }

                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "SaveLoadOptimizationUsers", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetLoadOptimizationUsers(int CompanyId)
        {
            var response = new List<int>();

            try
            {
                var result = await Context.DataContext.LoadOptimizationUsers.Where(x => x.CompanyId == CompanyId).FirstOrDefaultAsync();

                if (result != null)
                {
                    if (!string.IsNullOrEmpty(result.DistributedUsers))
                        response = JsonConvert.DeserializeObject<List<int>>(result.DistributedUsers);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetLoadOptimizationUsers", ex.Message, ex);
            }
            return response;

        }

        private async Task SaveIdentityProviderSetting(OnboardingPreferenceViewModel viewModel)
        {
            try
            {
                var idps = await Context.DataContext.CompanyIdentityServices.Where(x => x.CompanyId == viewModel.CompanyId
                && x.IsActive && x.IsAvailable).ToListAsync();
                if (idps != null)
                {
                    foreach (var idp in idps)
                    {
                        idp.IsActive = false;
                        if (viewModel.SelectedIdentityProvider.HasValue && idp.IdentityProviderId == viewModel.SelectedIdentityProvider)
                        {
                            idp.IsActive = viewModel.IsSelectedIdentityProvider;
                            Context.DataContext.Entry(idp).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "SaveIdentityProviderSetting", ex.Message, ex);
            }
        }

        private async Task SaveCarrierDeliveryXUserSetting(OnboardingPreferenceViewModel viewModel)
        {
            if (viewModel.CarrierUsers != null && viewModel.CarrierUsers.Any() && viewModel.IsCarrierTileEmailNotification)
            {
                //save CarrierDeliveryXUserSettings
                var carrierDeliveryXUserInfo = await Context.DataContext.CarrierDeliveryXUserSettings.Where(x => x.CompanyId == viewModel.CompanyId).ToListAsync();
                if (carrierDeliveryXUserInfo.Any())
                {
                    Context.DataContext.CarrierDeliveryXUserSettings.RemoveRange(carrierDeliveryXUserInfo);
                    await Context.CommitAsync();
                }
                List<CarrierDeliveryXUserSetting> carrierDeliveryXUserSettings = new List<CarrierDeliveryXUserSetting>();
                viewModel.CarrierUsers.ForEach(x =>
                {
                    CarrierDeliveryXUserSetting carrierDeliveryXUser = new CarrierDeliveryXUserSetting();
                    carrierDeliveryXUser.UserId = x;
                    carrierDeliveryXUser.CompanyId = viewModel.CompanyId;
                    carrierDeliveryXUser.CreatedBy = viewModel.CreatedBy;
                    carrierDeliveryXUser.CreatedDate = DateTime.Now;
                    carrierDeliveryXUserSettings.Add(carrierDeliveryXUser);
                });
                if (carrierDeliveryXUserSettings.Any())
                {
                    Context.DataContext.CarrierDeliveryXUserSettings.AddRange(carrierDeliveryXUserSettings);
                    await Context.CommitAsync();
                    //add user to UserXNotificationSettings
                    var eventInfo = Context.DataContext.MstEventTypes.FirstOrDefault(x => x.Id == (int)EventType.CarrierDeliveries);
                    if (eventInfo != null)
                    {
                        //remove duplicate users entry in UserXNotificationSettings.
                        var userdetails = await Context.DataContext.UserXNotificationSettings.Where(x => x.EventTypeId == (int)EventType.CarrierDeliveries && viewModel.CarrierUsers.Contains(x.UserId)).Select(x => x.UserId).ToListAsync();
                        if (userdetails.Any())
                        {
                            viewModel.CarrierUsers = viewModel.CarrierUsers.Except(userdetails).ToList();
                        }
                        List<UserXNotificationSetting> userXNotificationSettings = new List<UserXNotificationSetting>();
                        viewModel.CarrierUsers.ForEach(x =>
                        {
                            UserXNotificationSetting userXNotificationSetting = new UserXNotificationSetting();
                            userXNotificationSetting.UserId = x;
                            userXNotificationSetting.EventTypeId = eventInfo.Id;
                            userXNotificationSetting.IsEmail = true;
                            userXNotificationSetting.IsSMS = false;
                            userXNotificationSetting.IsInApp = false;
                            userXNotificationSettings.Add(userXNotificationSetting);
                        });
                        if (userXNotificationSettings.Any())
                        {
                            Context.DataContext.UserXNotificationSettings.AddRange(userXNotificationSettings);
                            await Context.CommitAsync();
                        }
                    }

                }
            }
            else
            {
                //remove users from CarrierDeliveryXUserSettings and UserXNotificationSettings.
                List<int> userIds = new List<int>();
                var carrierDeliveryXUserSettings = await Context.DataContext.CarrierDeliveryXUserSettings.Where(x => x.CompanyId == viewModel.CompanyId).ToListAsync();
                if (carrierDeliveryXUserSettings.Any())
                {
                    carrierDeliveryXUserSettings.ForEach(x =>
                    {
                        userIds.Add(x.UserId);
                    });
                    Context.DataContext.CarrierDeliveryXUserSettings.RemoveRange(carrierDeliveryXUserSettings);
                    await Context.CommitAsync();
                }
                if (userIds.Any())
                {
                    var userXNotificationSettings = await Context.DataContext.UserXNotificationSettings.Where(x => userIds.Contains(x.UserId) && x.EventTypeId == (int)EventType.CarrierDeliveries).ToListAsync();
                    if (userXNotificationSettings.Any())
                    {
                        Context.DataContext.UserXNotificationSettings.RemoveRange(userXNotificationSettings);
                        await Context.CommitAsync();
                    }

                }
            }
        }

        private LoadQueueAttributesViewModel GetLoadQueueAttributes(List<int> loadQueueAttributes)
        {
            LoadQueueAttributesViewModel response = new LoadQueueAttributesViewModel();
            if (loadQueueAttributes != null)
            {
                foreach (var loadqueue in loadQueueAttributes)
                {
                    switch (loadqueue)
                    {
                        case (int)LoadQueueAttributes.CustomerName:
                            response.CustomerName = true;
                            break;
                        case (int)LoadQueueAttributes.Driver:
                            response.Driver = true;
                            break;
                        case (int)LoadQueueAttributes.LocationName:
                            response.LocationName = true;
                            break;
                        case (int)LoadQueueAttributes.TrailerName:
                            response.TrailerName = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            return response;
        }

        private DRQueueAttributesViewModel GetDRQueueAttributes(List<int> drQueueAttributes)
        {
            DRQueueAttributesViewModel response = new DRQueueAttributesViewModel();
            if (drQueueAttributes != null)
            {
                foreach (var drqueue in drQueueAttributes)
                {
                    switch (drqueue)
                    {
                        case (int)DRQueueAttributes.CustomerName:
                            response.CustomerName = true;
                            break;
                        case (int)DRQueueAttributes.DeliveryShift:
                            response.DeliveryShift = true;
                            break;
                        case (int)DRQueueAttributes.TrailerCompatibility:
                            response.TrailerCompatibility = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            return response;
        }

        public async Task<OnboardingPreferenceViewModel> IsUserInvitedBySupplier(int userId)
        {
            OnboardingPreferenceViewModel response = new OnboardingPreferenceViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await Context.DataContext.SupplierInvitationDetails.Where(t => t.UserId == userId).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        var onboardingPreferencesDetails = await Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == user.CompanyId && t.UserId == t.UserId && t.IsActive && t.IsBrandMyWebsite).FirstOrDefaultAsync();
                        if (onboardingPreferencesDetails != null)
                        {
                            response.StatusCode = Status.Success;
                            response.StatusMessage = onboardingPreferencesDetails.URLName;
                            response.EntityNumber = onboardingPreferencesDetails.ImageFilePath;
                            response.IsBrandMyWebsite = onboardingPreferencesDetails.IsBrandMyWebsite;
                            response.BackgroundColor = onboardingPreferencesDetails.BackgroundColor;
                            response.ButtonColor = onboardingPreferencesDetails.ButtonColor;
                            response.FontColor = onboardingPreferencesDetails.FontColor;
                            response.ForegroundColor = onboardingPreferencesDetails.ForegroundColor;
                            response.HeaderColor = onboardingPreferencesDetails.HeaderColor;
                            response.IconColor = onboardingPreferencesDetails.IconColor;
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Empty;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = string.Empty;
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("CompanyDomain", "IsUserInvitedBySupplier", ex.Message, ex);
                }
            }

            return response;
        }

        public Tuple<int, string> GetCompanyDetailsByJobId(int jobId)
        {
            var companyId = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == jobId && t.IsActive)?.CompanyId ?? 0;
            string companyName = string.Empty;
            if (companyId > 0)
            {
                companyName = Context.DataContext.Companies.FirstOrDefault(t => t.Id == companyId)?.Name;
            }
            return new Tuple<int, string>(companyId, companyName);
        }
        public async Task<StatusViewModel> SaveProductSequence(ProductSequenceViewModel productSequenceDetails, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                var productSequences = await Context.DataContext.SupplierXProductSequencing.Where(t => t.SupplierCompanyId == userContext.CompanyId
                                                                         && (productSequenceDetails.JobId == 0 || productSequenceDetails.JobId == t.JobId)
                                                                         && t.SequenceCreationMethod == productSequenceDetails.SequenceMethod && t.IsActive)
                                                        .ToListAsync();
                if (productSequences != null && productSequences.Any())
                {
                    productSequences.ForEach(t =>
                    {
                        t.IsActive = false;
                        t.LastModifiedDate = DateTimeOffset.UtcNow;
                        t.UpdatedBy = userContext.Id;
                    });
                    //Context.Commit();
                    await Context.CommitAsync();
                }

                if (productSequenceDetails != null && productSequenceDetails.ProductSequence != null && productSequenceDetails.ProductSequence.Any())
                {
                    var sequences = productSequenceDetails.ToEntities(userContext);
                    Context.DataContext.SupplierXProductSequencing.AddRange(sequences);
                    await Context.CommitAsync();

                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.msgSequenceSaveSuccessfully;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
                LogManager.Logger.WriteException("CompanyDomain", "SaveProductSequence", ex.Message, ex);
            }
            return response;
        }
        public async Task<ProductSequenceViewModel> GetProductSequenceForJobs(int companyId, List<int> jobIds)
        {
            ProductSequenceViewModel response = new ProductSequenceViewModel() { SequenceType = ProductSequenceType.Product, SequenceMethod = ProductSequencingCreationMethod.Job, JobId = 0 };

            try
            {
                var JobSequence = await Context.DataContext.SupplierXProductSequencing
                                            .Where(t => t.SupplierCompanyId == companyId && t.IsActive
                                                        && t.SequenceCreationMethod == ProductSequencingCreationMethod.Job
                                                        && jobIds.Contains(t.JobId.Value))
                                            .Select(t => new ProductSequenceModel
                                            {
                                                ProductTypeId = t.ProductId,
                                                Sequence = t.SequenceNumber,
                                                OrderId = t.OrderId,
                                                JobId = t.JobId,
                                                DisplayName = t.OrderId.HasValue ? (t.Order.PoNumber + " - " + t.Order.FuelRequest.MstProduct.MstProductType.Name) : t.MstProductType.Name
                                            })
                                            .OrderBy(t => t.Sequence).ToListAsync();

                var jobsForProductSeqNotDefined = jobIds.Where(t => !JobSequence.Any(t1 => t1.JobId == t)).ToList();

                if (!JobSequence.Any() || jobsForProductSeqNotDefined.Any())
                {
                    var accountSequence = await Context.DataContext.SupplierXProductSequencing
                                                .Where(t => t.SupplierCompanyId == companyId &&
                                                    t.IsActive && t.SequenceCreationMethod == ProductSequencingCreationMethod.Account)
                                                .Select(t => new ProductSequenceModel
                                                {
                                                    ProductTypeId = t.ProductId,
                                                    Sequence = t.SequenceNumber,
                                                    OrderId = t.OrderId,
                                                    JobId = t.JobId,
                                                    DisplayName = t.OrderId.HasValue ? (t.Order.PoNumber + " - " + t.Order.FuelRequest.MstProduct.MstProductType.Name) : t.MstProductType.Name
                                                })
                                                .OrderBy(t => t.Sequence).ToListAsync();

                    if (accountSequence != null || accountSequence.Any())
                    {
                        foreach (var jobId in jobIds)
                        {
                            var tempList = JobSequence.Where(j => j.JobId == jobId).ToList();
                            if (tempList != null && tempList.Any())
                                response.ProductSequence.AddRange(tempList);
                            else
                            {
                                if (accountSequence != null)
                                {
                                    List<ProductSequenceModel> cloneList = new List<ProductSequenceModel>(accountSequence.Count);
                                    accountSequence.ForEach((item) => { cloneList.Add(item.ToClone()); });
                                    cloneList.ForEach(j => { j.JobId = jobId; });

                                    response.ProductSequence.AddRange(cloneList);
                                }
                            }
                        }
                    }
                }
                else
                {
                    response.ProductSequence = JobSequence;
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetProductSequenceForJobs", ex.Message, ex);
            }
            return response;
        }
        public List<DropdownDisplayItem> GetCompanySupplierAdminUsers(int companyId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            try
            {
                var user = Context.DataContext.Users.Where(t => t.CompanyId == companyId && t.IsActive && t.IsEmailConfirmed && t.IsOnboardingComplete &&
                t.MstRoles.Any(topx => topx.Name == UserRoles.Supplier.ToString() 
                 || topx.Name == UserRoles.Admin.ToString() || topx.Name == UserRoles.Buyer.ToString()
                 || topx.Name == UserRoles.BuyerAdmin.ToString() || topx.Name == UserRoles.Carrier.ToString()
                 || topx.Name == UserRoles.CarrierAdmin.ToString() || topx.Name == UserRoles.Dispatcher.ToString()
                 || topx.Name == UserRoles.SupplierAdmin.ToString())).ToList();
                if (user != null)
                {
                    foreach (var useritem in user)
                    {
                        response.Add(new DropdownDisplayItem { Id = useritem.Id, Name = useritem.FirstName + " " + useritem.LastName });
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetCompanySupplierAdminUsers", ex.Message, ex);
            }

            return response.Distinct().ToList();
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetPortsList()
        {
            var response = new List<DropdownDisplayExtendedItem>();
            response = await Context.DataContext.Jobs.Where(t => t.IsActive && t.IsMarine && t.LocationType == JobLocationTypes.Port)
                                                .Select(t => new DropdownDisplayExtendedItem() { Id = t.Id, Name = t.Name })
                                                .ToListAsync();
            
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetVesselList(int companyId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            response = await Context.DataContext.Assets.Where(t => t.IsActive && t.IsMarine && t.Type == (int)AssetType.Vessle && (t.CompanyId==ApplicationConstants.SuperAdminCompanyId || t.CompanyId==companyId))
                                                .Select(t => new DropdownDisplayExtendedItem() { Id = t.Id, Name = t.Name })
                                                .ToListAsync();

            return response;
        }
    }
}
