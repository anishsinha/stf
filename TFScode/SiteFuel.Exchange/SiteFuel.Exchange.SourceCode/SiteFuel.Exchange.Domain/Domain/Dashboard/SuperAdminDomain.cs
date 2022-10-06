using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Domain.Mappers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SiteFuel.Exchange.DataAccess.Entities;
using System.Text.RegularExpressions;
using System.IO;
using FileHelpers;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.ViewModels.Queue;
using Newtonsoft.Json;
using SiteFuel.Exchange.EmailManager;
using System.Web;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Job;

namespace SiteFuel.Exchange.Domain
{
    public class SuperAdminDomain : BaseDomain
    {
        public SuperAdminDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public SuperAdminDomain(BaseDomain domain)
            : base(domain)
        {
        }


        public async Task<OnboardingViewModel> ConvertExternalToInternal(int id)
        {
            OnboardingViewModel response = new OnboardingViewModel();
            try
            {
                var externalSupplier = await Context.DataContext.ExternalSuppliers.FirstOrDefaultAsync(t => t.Id == id);
                if (externalSupplier != null)
                {
                    response.User = externalSupplier.ToUserViewModel();
                    response.User.Company = externalSupplier.ToCompanyViewModel();
                    response.ExternalSupplierId = externalSupplier.Id;
                    var address = externalSupplier.ExternalSupplierAddresses.FirstOrDefault(t => t.IsActive);
                    if (address != null)
                    {
                        response.CompanyAddress = address.ToCompanyAddressViewModel();
                        response.SupplierProfile = new SupplierProfileViewModel()
                        {
                            IsStateWideService = address.IsStateWideService,
                            Radius = address.Radius,
                            ServingStates = address.MstStates.Select(t => t.Id).ToList(),
                            SupplierQualifications = address.MstSupplierQualifications.Select(t => t.Id).ToList()
                        };
                    }
                    response.CompanyAddress.SupplierWorkingHours = Context.DataContext.MstWeekDays
                         .OrderBy(t => t.Id).Select(t => new SupplierWorkingHoursViewModel
                         {
                             WeekDayId = t.Id,
                             WeekDayName = t.Name,
                             StartTime = t.Id == (int)WeekDay.Saturday || t.Id == (int)WeekDay.Sunday ? "00:00" : "08:00",
                             EndTime = t.Id == (int)WeekDay.Saturday || t.Id == (int)WeekDay.Sunday ? "00:00" : "17:00"
                         }).ToList();
                    response.CompanyAddress.IsOnboarding = true;
                    response.User.Company.IsOnboarding = true;
                    response.IsExternalSupplier = true;
                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "ConvertExternalToInternal", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> GetProgressReport(ProgressReportFilter filter, bool IsFromSuperAdminDB = false, string emailList = "")
        {
            bool response = false;
            try
            {
                var helperDomain = new HelperDomain(this);
                var storeProcedureDomain = new StoredProcedureDomain(this);
                var notificationDomain = new NotificationDomain(this);
                var emailDomain = new EmailDomain(this);

                var serverUrl = helperDomain.GetServerUrl();
                var emailTemplate = helperDomain.GetApplicationEventNotificationTemplate();

                var progressReport = await storeProcedureDomain.GetProgressReport(filter);
                progressReport.DailyReportLogo = helperDomain.GetAbsoluteServerUrl(serverUrl, Resource.email_DailyReportLogo);

                if (IsFromSuperAdminDB)
                {
                    progressReport.HideActiveAccountsAndUsers = true;
                }

                if (filter.AccountOwnerId != 0)
                {
                    var accountOwner = Context.DataContext.Users.SingleOrDefault(t => t.Id == filter.AccountOwnerId);
                    progressReport.ProgressReportCount.AccountOwnerName = accountOwner != null ? $"{accountOwner.FirstName + " " + accountOwner.LastName}" : string.Empty;
                }

                var emailClient = Email.GetClient();
                var mailBody = emailClient.GetHtml<ProgressReportViewModel>(Resource.ProgressReportBody, progressReport);

                var callbackUrl = $"~/SuperAdmin/Dashboard";
                var notification = notificationDomain.GetNotificationContent(EventSubType.ProgressReport, serverUrl, callbackUrl);

                var emailModel = new ApplicationEventNotificationViewModel
                {
                    To = emailList.Split(';').ToList(),
                    Subject = Resource.emailProgressReport_SubjectText,
                    CompanyLogo = notification.CompanyLogo,
                    BodyText = mailBody,
                    ShowHelpLineInfo = false,
                    BodyButtonText = notification.BodyButtonText,
                    BodyButtonUrl = notification.BodyButtonUrl
                };

                response = await emailDomain.SendEmail(emailTemplate, emailModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetProgressReport", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<ConvertedSupplierGridViewModel>> GetConvertedSuppliers(TimeSpan timeZoneOffset)
        {
            List<ConvertedSupplierGridViewModel> response = new List<ConvertedSupplierGridViewModel>();
            try
            {
                var convertedSuppliers = await Context.DataContext.ConvertedSuppliers.OrderByDescending(t => t.Id).ToListAsync();
                response = convertedSuppliers.Select(t => new ConvertedSupplierGridViewModel()
                {
                    SupplierName = t.SupplierName,
                    Type = t.MstExternalSupplierType.Name,
                    ContactPerson = t.ContactPerson,
                    AddedBy = t.AddedBy,
                    Address = t.Address,
                    DateAdded = t.DateAdded.ToBrowserDateTime(timeZoneOffset).ToString(Resource.constFormatDateTime),
                    ConvertedBy = t.ConvertedBy,
                    DateConverted = t.DateConverted.ToBrowserDateTime(timeZoneOffset).ToString(Resource.constFormatDateTime)
                }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetConvertedSuppliers", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<JobGridViewModel>> GetJobGridAsync(int companyId)
        {
            var response = new List<JobGridViewModel>();
            try
            {
                var jobs = await Context.DataContext.Jobs
                                .Include(t => t.JobBudget).Include(t => t.Users1)
                                .Include(t => t.MstState).Include(t => t.JobXAssets).Include("JobXAssets.Asset")
                                .Where(t => t.IsActive && t.CompanyId == companyId).OrderByDescending(t => t.Id).ToListAsync();
                jobs.ForEach(t => response.Add(t.ToGridViewModel()));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetJobGridAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<ProductViewModel> GetProductDetails(int productId)
        {
            ProductViewModel response = new ProductViewModel();
            try
            {
                var tfxProduct = await Context.DataContext.MstTfxProducts.FirstOrDefaultAsync(t => t.IsActive && t.Id == productId);
                if (tfxProduct != null)
                {
                    response.AxxisProductId = tfxProduct.MstProducts.FirstOrDefault(t => t.PricingSourceId == (int)PricingSource.Axxis)?.Id;
                    //response.ParklandProductId = tfxProduct.MstProducts.FirstOrDefault(t => t.PricingSourceId == (int)PricingSource.OPIS)?.Id;
                    response.PlattsProductId = tfxProduct.MstProducts.FirstOrDefault(t => t.PricingSourceId == (int)PricingSource.PLATTS)?.Id;
                    response.OpisProductId = tfxProduct.MstProducts.FirstOrDefault(t => t.PricingSourceId == (int)PricingSource.OPIS)?.Id;
                    response.DisplayName = tfxProduct.Name;
                    response.ProductDescription = tfxProduct.ProductDescription;
                    response.Id = productId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetProductDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<ProductTypeMappingViewModel> GetMappedProductType(int productTypeId, bool isBlend)
        {
            ProductTypeMappingViewModel response = new ProductTypeMappingViewModel();
            try
            {
                if (!isBlend)
                {
                    var productTypeMapping = await Context.DataContext.ProductTypeCompatibilityMappings.Where(t => t.ProductTypeId == productTypeId).Select(t => t.MappedToProductTypeId).ToListAsync(); ;
                    if (productTypeMapping != null)
                    {
                        response.MappedToProductTypeIds = productTypeMapping;
                    }
                }
                else
                {
                    var blendProductTypeMapping = await Context.DataContext.MstBlendProductTypeMapping.Where(t => t.ProductTypeId == productTypeId).Select(t => t.MappedToProductTypeId).ToListAsync(); ;
                    if (blendProductTypeMapping != null)
                    {
                        response.MappedToProductTypeIds = blendProductTypeMapping;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetMappedProductType", ex.Message, ex);
            }
            return response;
        }

        public async Task<User> CreateUserCompany(OnboardingViewModel viewModel, int userId)
        {
            User user = null;
            RegisterViewModel newCompanyViewModel = new RegisterViewModel
            {
                FirstName = viewModel.User.FirstName,
                LastName = viewModel.User.LastName,
                Email = viewModel.User.Email,
                MobileNumber = viewModel.User.MobileNumber,
                Password = viewModel.User.Password,
                ConfirmPassword = viewModel.User.ConfirmPassword,
                Title = viewModel.User.Title,
                Company = new CompanyViewModel() { Name = viewModel.User.Company.Name, CompanyTypeId = viewModel.User.Company.CompanyTypeId }
            };
            AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
            user = authenticationDomain.RegisterCompany(newCompanyViewModel, userId);
            Context.DataContext.Users.Add(user);
            await Context.CommitAsync();
            user.Company.CreatedBy = userId;
            user.Company.UpdatedBy = userId;
            user.Company.AccountTypeId = viewModel.User.IsAccountSfxOwned ? (int)AccountType.SfxOwned : (int)AccountType.Real;
            await Context.CommitAsync();
            return user;
        }

        public async Task<StatusViewModel> SaveCityZip(int stateId, string previousCityName, string cityName, string zipCodes)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var city = await Context.DataContext.MstCities.SingleOrDefaultAsync(t => t.StateId == stateId && t.Name == previousCityName);
                if (city != null)
                {
                    city.Name = cityName;
                    city.ZipCodes = zipCodes;

                    Context.DataContext.Entry(city).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageCityZipUpdatedSuccessfully;
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("SuperAdminDomain", "SaveCityZip", ex.Message, ex);
            }
            return response;
        }

        public async Task<User> ExternalSupplierToInternal(OnboardingViewModel viewModel, int userId, string userName)
        {
            User user = null;
            var externalSupplier = await Context.DataContext.ExternalSuppliers.FirstOrDefaultAsync(t => t.Id == viewModel.ExternalSupplierId);
            if (externalSupplier != null)
            {
                user = await CreateUserCompany(viewModel, userId);
                if (externalSupplier.ExternalSupplierAddresses.Count(t => t.IsActive) > 1)
                {
                    var addresses = externalSupplier.ExternalSupplierAddresses.Where(t => t.IsActive).Skip(1);
                    var mstQualifications = Context.DataContext.MstSupplierQualifications.ToList();
                    var mstStates = Context.DataContext.MstStates.ToList();
                    var mstProductTypes = Context.DataContext.MstProductTypes.ToList();
                    var workingHours = Context.DataContext.MstWeekDays
                        .OrderBy(t => t.Id).Select(t => new SupplierWorkingHoursViewModel
                        {
                            WeekDayId = t.Id,
                            WeekDayName = t.Name,
                            StartTime = t.Id == (int)WeekDay.Saturday || t.Id == (int)WeekDay.Sunday ? "00:00" : "08:00",
                            EndTime = t.Id == (int)WeekDay.Saturday || t.Id == (int)WeekDay.Sunday ? "00:00" : "17:00"
                        }).ToList();
                    foreach (var address in addresses)
                    {
                        CompanyAddressViewModel companyAddress = address.ToCompanyAddressViewModel();
                        CompanyAddress addressEntity = companyAddress.ToEntity();
                        addressEntity.IsDefault = false;
                        if (user.Company.CompanyTypeId == (int)CompanyType.Supplier || user.Company.CompanyTypeId == (int)CompanyType.BuyerAndSupplier)
                        {
                            var supplierAddressXQualifications = mstQualifications.Where(t => companyAddress.SupplierProfile.SupplierQualifications.Contains(t.Id)).ToList();
                            var supplierAddressXServingStates = mstStates.Where(t => companyAddress.SupplierProfile.ServingStates.Contains(t.Id)).ToList();

                            addressEntity.MstSupplierQualifications = supplierAddressXQualifications;
                            addressEntity.MstStates = supplierAddressXServingStates;

                            addressEntity.MstProductTypes = mstProductTypes.Where(t => companyAddress.SupplierProductTypes.Contains(t.Id)).ToList();
                            addressEntity.SupplierAddressXWorkingHours = workingHours.Select(t => new SupplierAddressXWorkingHour
                            {
                                WeekDayId = t.WeekDayId,
                                StartTime = Convert.ToDateTime(t.StartTime).TimeOfDay,
                                EndTime = Convert.ToDateTime(t.EndTime).TimeOfDay
                            }).ToList();

                            addressEntity.SupplierAddressXSetting = companyAddress.SupplierProfile.ToEntity();
                            user.Company.CompanyAddresses.Add(addressEntity);
                        }
                    }
                    await Context.CommitAsync();
                }
                UpdateConvertedData(externalSupplier, userName);
            }
            return user;
        }

        private void UpdateConvertedData(ExternalSupplier externalSupplier, string userName)
        {
            string externalAddress = string.Empty;
            var defaultAddress = externalSupplier.ExternalSupplierAddresses.FirstOrDefault(t => t.IsActive);
            if (defaultAddress != null)
            {
                externalAddress = string.Format("{0}, {1}, {2} {3}", defaultAddress.Address, defaultAddress.City, defaultAddress.MstState.Code, defaultAddress.ZipCode);
            }
            User addedUser = Context.DataContext.Users.SingleOrDefault(t => t.Id == externalSupplier.CreatedBy);
            ConvertedSupplier convertedSupplier = new ConvertedSupplier()
            {
                SupplierName = externalSupplier.Name,
                Type = externalSupplier.CompanyTypeId,
                ContactPerson = externalSupplier.ContactPersonName,
                Address = externalAddress,
                ConvertedBy = userName,
                DateConverted = DateTimeOffset.Now,
                DateAdded = externalSupplier.CreatedDate,
                AddedBy = addedUser.FirstName + " " + addedUser.LastName
            };
            Context.DataContext.ConvertedSuppliers.Add(convertedSupplier);

            Context.DataContext.ExternalSuppliers.Remove(externalSupplier);
            Context.Commit();
        }

        public async Task<List<SuperAdminCompanyGridViewModel>> GetCompaniesAsync(CompanyDataTableViewModel model)
        {
            var response = new List<SuperAdminCompanyGridViewModel>();
            try
            {
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                if (!string.IsNullOrEmpty(model.StartDate))
                {
                    StartDate = Convert.ToDateTime(model.StartDate).Date;
                }
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(model.EndDate))
                {
                    EndDate = Convert.ToDateTime(model.EndDate).Date.AddDays(1);
                }

                response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetCompanies(StartDate, EndDate, model, (int)model.filter);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetCompaniesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<SiteFuelUserGridViewModel>> GetSiteFuelUsersAsync(SiteFuelUserFilterType filter = SiteFuelUserFilterType.All)
        {
            var response = new List<SiteFuelUserGridViewModel>();
            try
            {
                var sitefuelUsers = Context.DataContext.Users.Include(t => t.MstRoles).Where(t => !t.Email.Contains(".delete"));
                switch (filter)
                {
                    case SiteFuelUserFilterType.All:
                    case SiteFuelUserFilterType.AllSuperAdmin:
                        sitefuelUsers = sitefuelUsers.Where(t => t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.SuperAdmin));
                        break;
                    case SiteFuelUserFilterType.ActiveSuperAdmin:
                        sitefuelUsers = sitefuelUsers.Where(t => t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.SuperAdmin && t.IsActive));
                        break;
                    case SiteFuelUserFilterType.InActiveSuperAdmin:
                        sitefuelUsers = sitefuelUsers.Where(t => t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.SuperAdmin && !t.IsActive));
                        break;
                    case SiteFuelUserFilterType.InternalSalesPerson:
                        sitefuelUsers = sitefuelUsers.Where(t => t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.InternalSalesPerson));
                        break;
                    case SiteFuelUserFilterType.ExternalVendor:
                        sitefuelUsers = sitefuelUsers.Where(t => t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.ExternalVendor));
                        break;
                    case SiteFuelUserFilterType.AccountSpecialist:
                        sitefuelUsers = sitefuelUsers.Where(t => t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.AccountSpecialist));
                        break;
                }

                await sitefuelUsers.ForEachAsync(t => response.Add(new SiteFuelUserGridViewModel
                {
                    Id = t.Id,
                    Name = $"{t.FirstName} {t.LastName}",
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber,
                    RoleNames = string.Join(" <br/>", t.MstRoles.Select(x => x.Name).ToList()),
                    AddedBy = Context.DataContext.Users.Where(t1 => t1.Id == t.CreatedBy).Select(t1 => (t1.FirstName + " " + t1.LastName)).FirstOrDefault(),
                    AddedDate = t.CreatedDate.ToString(Resource.constFormatDate),
                    IsActive = t.IsActive,
                    IsSalesCalculatorAllowed = t.IsSalesCalculatorAllowed,
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetSiteFuelUsersAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ImpersonationActivityLogViewModel>> GetImpersonationActivityLogAsync(ImpersonateLogDataTableViewModel requestModel)
        {
            var response = new List<ImpersonationActivityLogViewModel>();
            try
            {
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                if (!string.IsNullOrEmpty(requestModel.StartDate))
                {
                    StartDate = Convert.ToDateTime(requestModel.StartDate).Date;
                }
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(requestModel.EndDate))
                {
                    EndDate = Convert.ToDateTime(requestModel.EndDate).Date.AddDays(1);
                }

                response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetImpersonedActivityLog(StartDate, EndDate, requestModel, requestModel.ImpersonatedBy ?? 0);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetImpersonationActivityLogAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<CompanyUserGridViewModel>> GetCompanyUsersAsync(DataTableSearchModel dataTableSearchModel, CompanyUsersDataTableViewModel requestModel)
        {
            List<CompanyUserGridViewModel> response = new List<CompanyUserGridViewModel>();
            try
            {
                response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetCompanyUsers(requestModel.CompanyId, dataTableSearchModel, requestModel.StatusFilter, requestModel.RoleFilter);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SettingsDomain", "GetCompanyUsersAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<ResponseViewModel> AssignAccountOwner(int companyId, int? accountOwnerId)
        {
            using (var tracer = new Tracer("SuperAdminDomain", "AssignAccountOwner"))
            {
                var response = new ResponseViewModel(Status.Success);

                try
                {
                    var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId);
                    if (company != null)
                    {
                        company.AccountOwnerId = accountOwnerId;
                        Context.DataContext.Entry(company).State = EntityState.Modified;
                        await Context.CommitAsync();
                        response.StatusMessage = Resource.errMessageAccountOwnerAssignementSuccess;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageFailed;
                    LogManager.Logger.WriteException("SuperAdminDomain", "AssignAccountOwner", ex.Message, ex);
                }

                return response;
            }
        }

        public List<ImpersonationHistoryViewModel> GetImpersonations()
        {
            var response = new List<ImpersonationHistoryViewModel>();
            try
            {
                var activeImporsonations = (from history in Context.DataContext.ImpersonationHistories
                                            join user in Context.DataContext.Users on history.UserId equals user.Id
                                            join superAdmin in Context.DataContext.Users on history.ImpersonatedBy equals superAdmin.Id
                                            where user.IsImpersonated == true && history.ImpersonatedEndTime == null &&
                                            superAdmin.Id > 0 && user.Id > 0
                                            select new
                                            {
                                                id = history.Id,
                                                userName = user.FirstName + " " + user.LastName,
                                                impoersonatedBy = superAdmin.FirstName + " " + superAdmin.LastName,
                                                startTime = history.ImpersonatedStartTime
                                            }).Distinct();

                foreach (var item in activeImporsonations)
                {
                    response.Add(new ImpersonationHistoryViewModel()
                    {
                        Id = item.id,
                        ImpersonatedBy = item.impoersonatedBy,
                        ImpersonatedUser = item.userName,
                        ImpersonatedStartTime = item.startTime.ToString(Resource.constFormatDateTime)
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetImpersonations", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> RemoveImpersonation(int id, int terminatedByUserId)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var history = await Context.DataContext.ImpersonationHistories.SingleOrDefaultAsync(t => t.Id == id);
                    if (history != null)
                    {
                        history.ImpersonatedEndTime = DateTimeOffset.Now;
                        history.TerminatedBy = terminatedByUserId;
                        Context.DataContext.Entry(history).State = EntityState.Modified;

                        var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == history.UserId);
                        if (user != null)
                        {
                            user.IsImpersonated = false;
                            Context.DataContext.Entry(user).State = EntityState.Modified;

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageImpersonationRemovedSuccess;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SuperAdminDomain", "RemoveImpersonation", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<ImpersonationHistoryViewModel>> GetImpersonationHistoryAsync(int impersonatedBy, string fromDate, string toDate)
        {
            var response = new List<ImpersonationHistoryViewModel>();
            try
            {
                var impersonationHistories = await Context.DataContext.ImpersonationHistories
                     .Where(t => impersonatedBy == 0 || t.ImpersonatedBy == impersonatedBy).ToListAsync();

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTimeOffset startDate = Convert.ToDateTime(fromDate).Date;
                    impersonationHistories = impersonationHistories.Where(t => t.ImpersonatedStartTime >= startDate).ToList();
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTimeOffset endDate = Convert.ToDateTime(toDate).Date.AddDays(1);
                    impersonationHistories = impersonationHistories.Where(t => t.ImpersonatedEndTime < endDate).ToList();
                }

                response = impersonationHistories.OrderByDescending(t => t.ImpersonatedStartTime).ThenByDescending(t => t.ImpersonatedEndTime).Select(t => new ImpersonationHistoryViewModel
                {
                    Id = t.Id,
                    ImpersonatedStartTime = t.ImpersonatedStartTime.ToString(Resource.constFormatDateTime),
                    ImpersonatedEndTime = t.ImpersonatedEndTime != null ? ((DateTimeOffset)t.ImpersonatedEndTime).ToString(Resource.constFormatDateTime) : "",
                    ImpersonatedUser = Context.DataContext.Users.Where(t1 => t1.Id == t.UserId).Select(t1 => (t1.FirstName + " " + t1.LastName)).FirstOrDefault(),
                    ImpersonatedBy = Context.DataContext.Users.Where(t1 => t1.Id == t.ImpersonatedBy).Select(t1 => (t1.FirstName + " " + t1.LastName)).FirstOrDefault(),
                    TerminatedBy = t.TerminatedBy.HasValue ? Context.DataContext.Users.Where(t1 => t1.Id == t.TerminatedBy).Select(t1 => (t1.FirstName + " " + t1.LastName)).FirstOrDefault() : ""
                }).ToList();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetImpersonationHistoryAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<SuperAdminOrderGridViewModel>> GetOrdersByPoNumberAsync(string poNumber)
        {
            List<SuperAdminOrderGridViewModel> orders = new List<SuperAdminOrderGridViewModel>();
            try
            {
                orders = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetOrdersByPoNumberAsync(poNumber);
                HelperDomain helperDomain = new HelperDomain(this);
                foreach (var order in orders)
                {
                    order.RackOrPpg = order.PricePerGallon; // helperDomain.GetPricePerGallon(order.PricePerGallon, order.PricingTypeId, order.RackAvgTypeId ?? 0);
                    order.GallonsOrdered = helperDomain.GetQuantityRequested(order.Quantity);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetOrdersByPoNumberAsync", ex.Message, ex);
            }
            return orders;
        }

        public async Task<StatusViewModel> DeleteOrder(int orderId, UserContext userContext)
        {
            var response = new StatusViewModel();

            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == orderId).FirstOrDefaultAsync();
                if (order != null)
                {
                    bool deleteOrder = true;
                    if (!order.IsEndSupplier)
                    {
                        int brokeredOrderStatusId = GetBrokeredOrderStatus(order);
                        if (brokeredOrderStatusId > 0 && brokeredOrderStatusId == (int)OrderStatus.Open)
                        {
                            deleteOrder = false;
                        }
                    }

                    if (deleteOrder)
                    {
                        var statusId = order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId;
                        if (statusId == (int)OrderStatus.Canceled || statusId == (int)OrderStatus.Closed)
                        {
                            var isActiveInvoice = Context.DataContext.Invoices.Any(t => t.OrderId == orderId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active);
                            if (!isActiveInvoice)
                            {
                                order.IsActive = false;
                                order.UpdatedBy = userContext.Id;
                                order.UpdatedDate = DateTimeOffset.Now;

                                Context.DataContext.Entry(order).State = EntityState.Modified;
                                //var newsfeeds = Context.DataContext.Newsfeeds.Where(t => t.TargetEntityId == orderId);
                                //newsfeeds.ToList().ForEach(t => t.IsActive = false);

                                await Context.CommitAsync();
                                response.StatusCode = Status.Success;

                                if (IsOrderSyncWithQB(order.Id))
                                    response.StatusMessage = string.Format(Resource.errMessageDeletedSuccessAndQBSync, Resource.lblOrder);
                                else
                                    response.StatusMessage = string.Format(Resource.errMessageDeletedSuccess, Resource.lblOrder);
                            }
                            else
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageActiveInvoiceOrderCannotDelete;
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageOpenOrderCannotDelete;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageBrokeredOrderCannotDelete;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageFailed;
                LogManager.Logger.WriteException("SuperAdminDomain", "DeleteOrder", ex.Message, ex);
            }

            return response;
        }

        private bool IsOrderSyncWithQB(int orderId)
        {
            var order = $"\"OrderId\":{orderId}";
            return Context.DataContext.QbWorkflows.Any(t => t.ParameterJson.Contains(order)
                            && t.Status == (int)AccountingWorkflowStatus.Completed);
        }

        private int GetBrokeredOrderStatus(Order order)
        {
            var brokeredFuelRequest = order.FuelRequest.FuelRequests1.OrderByDescending(t => t.Id).FirstOrDefault();
            if (brokeredFuelRequest != null && brokeredFuelRequest.GetFuelRequestLastOrder() != null)
            {
                var brokeredOrder = brokeredFuelRequest.GetFuelRequestLastOrder();
                if (brokeredOrder != null)
                {
                    return brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId;
                }
            }
            return 0;
        }

        public async Task<OrderDetailsViewModel> GetOrderDetailsAsync(int id)
        {
            OrderDetailsViewModel response = new OrderDetailsViewModel();
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == id);
                if (order != null)
                {
                    HelperDomain helperDomain = new HelperDomain(this);
                    var fuelRequest = order.FuelRequest;
                    var job = fuelRequest.Job;
                    var status = order.OrderXStatuses.FirstOrDefault(t => t.IsActive);
                    response.Id = order.Id;
                    response.PoNumber = order.PoNumber;
                    response.BuyerCompanyName = order.BuyerCompany.Name;
                    response.SupplierCompanyName = order.Company.Name;
                    response.JobName = job.Name;
                    response.JobLocation = new AddressViewModel { City = job.City, Address = job.Address, StateCode = job.MstState.Code, ZipCode = job.ZipCode, LocationType = (int)job.LocationType };
                    response.FuelType = helperDomain.GetProductName(fuelRequest.MstProduct);
                    response.TypeOfFuel = fuelRequest.MstProduct.ProductDisplayGroupId;
                    response.ProductDescription = fuelRequest.FuelDescription;
                    response.FuelDeliveryDetails = fuelRequest.FuelRequestDetail.ToViewModel();
                    response.EstimatedGallonsPerDelivery = fuelRequest.EstimateGallonsPerDelivery;
                    response.Distance = order.TerminalId == null ? 0 : helperDomain.CalculateDistance(job.Latitude, job.Longitude, order.MstExternalTerminal.Latitude, order.MstExternalTerminal.Longitude);
                    response.TerminalName = order.TerminalId == null ? Resource.lblHyphen : order.MstExternalTerminal.Name;
                    response.StatusId = status.StatusId;
                    response.JobStartDate = job.StartDate.ToString(Resource.constFormatDate);
                    response.JobEndDate = job.EndDate;
                    response.FuelDeliveredPercentage = helperDomain.GetFuelDeliveredPercentage(order);
                    response.GallonsDelivered = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons);
                    response.DisplayGallonsOrdered = helperDomain.GetQuantityRequested(order.BrokeredMaxQuantity, fuelRequest.MaxQuantity);
                    response.PricePerGallon = helperDomain.GetPricePerGallon(fuelRequest);
                    response.FuelDetails.FuelQuantity.QuantityTypeId = fuelRequest.QuantityTypeId;
                    response.IsProFormaPo = order.IsProFormaPo;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetOrderDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> EditOrderStartDateAsync(int id, DateTime startDate)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == id);
                if (order != null)
                {
                    order.FuelRequest.FuelRequestDetail.StartDate = startDate;
                    Context.DataContext.Entry(order).State = EntityState.Modified;
                    Context.Commit();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageStartDateUpdated;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "EditOrderStartDateAsync", ex.Message, ex);
            }
            return response;
        }

        public SupplierGeoViewModel GetSupplierGeoView()
        {
            var response = new SupplierGeoViewModel();
            return response;
        }

        public async Task<SupplierGeoViewModel> SearchSuppliersByLocation(SupplierGeoViewModel supplierModel)
        {
            try
            {
                var geoCodes = GoogleApiDomain.GetGeocode(supplierModel.ZipCode);
                if (geoCodes != null)
                {
                    supplierModel.Latitude = Convert.ToDecimal(geoCodes.Latitude);
                    supplierModel.Longitude = Convert.ToDecimal(geoCodes.Longitude);
                    supplierModel.State.Code = Convert.ToString(geoCodes.StateCode);
                    supplierModel.State.Name = Convert.ToString(geoCodes.StateName);
                }
                else
                {
                    supplierModel.Suppliers = new List<SupplierDetailViewModel>();
                    return supplierModel;
                }

                // get fuel types in comma seperated
                string fuelTypes = string.Join(",", supplierModel.SupplierFuelTypes.Select(t => t));

                int accountTypeId = supplierModel.AccountTypeId ?? 0;
                supplierModel.Suppliers = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().SearchSuppliersByLocationAsync(supplierModel, accountTypeId, fuelTypes);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "SearchSuppliersByLocation", ex.Message, ex);
            }
            return supplierModel;
        }

        public async Task<StatusViewModel> UpdateCompanyAccountTypeStatusAsync(int companyId, bool isSfxOwned)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId);
                    if (company != null)
                    {
                        if (!isSfxOwned)
                        {
                            var onboardingDomain = new OnboardingDomain(this);
                            var users = await Context.DataContext.Users.Where(t => t.CompanyId == companyId).ToListAsync();
                            foreach (var user in users)
                            {
                                //sent Email To All CompanyUsers
                                await onboardingDomain.SentCredentialsEmailToCompanyUser(user.Id, isSfxOwned, companyId);
                            }
                            response.StatusMessage = ResourceMessages.GetMessage(Resource.successMessageForSFXOwnedUserUpdateCredentials, new object[] { company.Name });
                        }
                        await ContextFactory.Current.GetDomain<SettingsDomain>().ToggleBlockUserNotificationsAsync(companyId, isSfxOwned);

                        company.AccountTypeId = isSfxOwned ? (int)AccountType.SfxOwned : (int)AccountType.Real;
                        await Context.CommitAsync();

                        transaction.Commit();

                        response.StatusCode = Status.Success;

                        if (isSfxOwned)
                        {
                            response.StatusMessage = Resource.errMessageCompanyAccountTypeSaveSuccess;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SuperAdminDomain", "UpdateCompanyAccountTypeStatusAsync", ex.Message, ex);
                }
            }
            return response;
        }


        public async Task<StatusViewModel> UpdateBuyerAuditStatusAsync(int companyId, bool isAuditApplicable)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == companyId);
                    if (company != null)
                    {
                        company.IsAuditApplicable = isAuditApplicable;
                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        if (isAuditApplicable)
                        { response.StatusMessage = ResourceMessages.GetMessage(Resource.successMsgAuditApplicable, new object[] { company.Name }); }
                        else
                        { response.StatusMessage = ResourceMessages.GetMessage(Resource.successMsgAuditNotApplicable, new object[] { company.Name }); }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SuperAdminDomain", "UpdateBuyerAuditStatusAsync", ex.Message, ex);
                }
            }
            return response;
        }


        public async Task<StatusViewModel> SaveExternalSupplierAsync(UserContext userContext, ExternalSupplierViewModel viewModel)
        {
            using (var tracer = new Tracer("SuperAdminDomain", "SaveExternalSupplierAsync"))
            {
                var response = new StatusViewModel();

                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        ExternalSupplier externalSupplier = new ExternalSupplier
                        {
                            Name = viewModel.CompanyDetails.Name,
                            CompanyTypeId = viewModel.CompanyDetails.CompanyTypeId,
                            Website = viewModel.CompanyDetails.Website,
                            InPipedrive = viewModel.CompanyDetails.InPipedrive,
                            ContactPersonName = viewModel.ContactPersonDetails.Name,
                            ContactPersonEmail = viewModel.ContactPersonDetails.Email,
                            ContactPersonPhoneNumber = viewModel.ContactPersonDetails.PhoneNumber,
                            IsActive = true,
                            CreatedBy = userContext.Id,
                            CreatedDate = DateTimeOffset.Now,
                            UpdatedBy = userContext.Id,
                            UpdatedDate = DateTimeOffset.Now
                        };

                        Context.DataContext.ExternalSuppliers.Add(externalSupplier);

                        if (viewModel.OtherLocationsAndServices.Count > 0)
                        {
                            var country = Context.DataContext.MstCountries.First(t => t.Id == 1).Code;
                            foreach (var otherLocationAndService in viewModel.OtherLocationsAndServices)
                            {
                                var state = Context.DataContext.MstStates.First(t => t.Id == otherLocationAndService.StateId).Code;
                                var point = GoogleApiDomain.GetGeocode($"{otherLocationAndService.Address} {otherLocationAndService.City} {state} {country} {otherLocationAndService.ZipCode}");
                                var externalSupplierOtherAddress = new ExternalSupplierAddress
                                {
                                    ExternalSupplierId = externalSupplier.Id,
                                    Address = otherLocationAndService.Address,
                                    City = otherLocationAndService.City,
                                    StateId = otherLocationAndService.StateId,
                                    ZipCode = otherLocationAndService.ZipCode,
                                    CountryId = otherLocationAndService.CountryId,
                                    PhoneTypeId = otherLocationAndService.PhoneType,
                                    PhoneNumber = otherLocationAndService.PhoneNumber,
                                    IsStateWideService = otherLocationAndService.SupplierProfile.IsStateWideService,
                                    Radius = otherLocationAndService.SupplierProfile.Radius,
                                    NumberOfTrucks = otherLocationAndService.SupplierProfile.NumberOfTrucks,
                                    Latitude = point != null ? Convert.ToDecimal(point.Latitude) : 0,
                                    Longitude = point != null ? Convert.ToDecimal(point.Longitude) : 0,
                                    IsActive = true,
                                    CreatedBy = userContext.Id,
                                    CreatedDate = DateTimeOffset.Now,
                                    UpdatedBy = userContext.Id,
                                    UpdatedDate = DateTimeOffset.Now
                                };
                                var otherAddressXQualifications = Context.DataContext.MstSupplierQualifications.Where(t => otherLocationAndService.SupplierProfile.SupplierQualifications.Contains(t.Id)).ToList();
                                var otherAddressXServingStates = Context.DataContext.MstStates.Where(t => otherLocationAndService.SupplierProfile.ServingStates.Contains(t.Id)).ToList();
                                var otherAddressXProductTypes = Context.DataContext.MstProductTypes.Where(t => otherLocationAndService.SupplierProductTypes.Contains(t.Id)).ToList();

                                //Delete all existing qualifications and add new (if any)
                                externalSupplierOtherAddress.MstSupplierQualifications.ToList().RemoveAll(t => t.Id > 0);
                                externalSupplierOtherAddress.MstSupplierQualifications = otherAddressXQualifications;

                                //Delete all existing serving states and add new (if any)
                                externalSupplierOtherAddress.MstStates.ToList().RemoveAll(t => t.Id > 0);
                                externalSupplierOtherAddress.MstStates = otherAddressXServingStates;

                                //Delete all existing fueltypes and add new
                                externalSupplierOtherAddress.MstProductTypes.ToList().RemoveAll(t => t.Id > 0);
                                externalSupplierOtherAddress.MstProductTypes = otherAddressXProductTypes;

                                Context.DataContext.ExternalSupplierAddresses.Add(externalSupplierOtherAddress);
                                if (otherLocationAndService.SupplierProfile.BobtailTransportTankWagon != null && otherLocationAndService.SupplierProfile.BobtailTransportTankWagon.Count > 0)
                                {
                                    foreach (var bobtailTransportTankWagon in otherLocationAndService.SupplierProfile.BobtailTransportTankWagon)
                                    {
                                        var externalSupplierAddressTruckType = new ExternalSupplierAddressTruckType
                                        {
                                            ExternalSupplierAddress = externalSupplierOtherAddress,
                                            TruckTypeId = bobtailTransportTankWagon
                                        };
                                        Context.DataContext.ExternalSupplierAddressTruckTypes.Add(externalSupplierAddressTruckType);
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(viewModel.CompanyDetails.Notes))
                        {
                            var externalSupplierNote = new ExternalSupplierNote
                            {
                                ExternalSupplierId = externalSupplier.Id,
                                IsCompleted = false,
                                IsActive = true,
                                Note = viewModel.CompanyDetails.Notes,
                                CreatedBy = userContext.Id,
                                CreatedDate = DateTimeOffset.Now,
                                UpdatedBy = userContext.Id,
                                UpdatedDate = DateTimeOffset.Now
                            };
                            Context.DataContext.ExternalSupplierNotes.Add(externalSupplierNote);
                        }

                        var externalSupplierStatus = new ExternalSupplierStatus
                        {
                            ExternalSupplierId = externalSupplier.Id,
                            StatusId = (int)ExternalSupplierStatuses.New,
                            IsActive = true,
                            CreatedBy = userContext.Id,
                            CreatedDate = DateTimeOffset.Now,
                            UpdatedBy = userContext.Id,
                            UpdatedDate = DateTimeOffset.Now
                        };
                        Context.DataContext.ExternalSupplierStatuses.Add(externalSupplierStatus);

                        await Context.CommitAsync();
                        transaction.Commit();

                        viewModel.CompanyDetails.Id = externalSupplier.Id;
                        response.StatusCode = Status.Success;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("SuperAdminDomain", "SaveExternalSupplierAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StatusViewModel> UpdateExternalSupplierAsync(UserContext userContext, ExternalSupplierViewModel viewModel)
        {
            using (var tracer = new Tracer("SuperAdminDomain", "UpdateExternalSupplierAsync"))
            {
                var response = new StatusViewModel();

                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var externalSupplier = Context.DataContext.ExternalSuppliers.FirstOrDefault(t => t.Id == viewModel.CompanyDetails.Id);
                        externalSupplier.Name = viewModel.CompanyDetails.Name;
                        externalSupplier.CompanyTypeId = viewModel.CompanyDetails.CompanyTypeId;
                        externalSupplier.Website = viewModel.CompanyDetails.Website;
                        externalSupplier.InPipedrive = viewModel.CompanyDetails.InPipedrive;
                        externalSupplier.ContactPersonName = viewModel.ContactPersonDetails.Name;
                        externalSupplier.ContactPersonEmail = viewModel.ContactPersonDetails.Email;
                        externalSupplier.ContactPersonPhoneNumber = viewModel.ContactPersonDetails.PhoneNumber;
                        externalSupplier.IsActive = true;
                        externalSupplier.UpdatedBy = userContext.Id;
                        externalSupplier.UpdatedDate = DateTimeOffset.Now;

                        externalSupplier.ExternalSupplierAddresses.ToList().ForEach(t => t.IsActive = false);

                        if (viewModel.OtherLocationsAndServices.Count > 0)
                        {
                            var country = Context.DataContext.MstCountries.First(t => t.Id == 1).Code;
                            foreach (var otherLocationAndService in viewModel.OtherLocationsAndServices)
                            {
                                var state = Context.DataContext.MstStates.First(t => t.Id == otherLocationAndService.StateId).Code;
                                var point = GoogleApiDomain.GetGeocode($"{otherLocationAndService.Address} {otherLocationAndService.City} {state} {country} {otherLocationAndService.ZipCode}");

                                var externalSupplierOtherAddress = new ExternalSupplierAddress
                                {
                                    ExternalSupplierId = externalSupplier.Id,
                                    Address = otherLocationAndService.Address,
                                    City = otherLocationAndService.City,
                                    StateId = otherLocationAndService.StateId,
                                    ZipCode = otherLocationAndService.ZipCode,
                                    CountryId = otherLocationAndService.CountryId,
                                    PhoneTypeId = otherLocationAndService.PhoneType,
                                    PhoneNumber = otherLocationAndService.PhoneNumber,
                                    IsStateWideService = otherLocationAndService.SupplierProfile.IsStateWideService,
                                    Radius = otherLocationAndService.SupplierProfile.Radius,
                                    NumberOfTrucks = otherLocationAndService.SupplierProfile.NumberOfTrucks,
                                    Latitude = point != null ? Convert.ToDecimal(point.Latitude) : 0,
                                    Longitude = point != null ? Convert.ToDecimal(point.Longitude) : 0,
                                    IsActive = true,
                                    CreatedBy = userContext.Id,
                                    CreatedDate = DateTimeOffset.Now,
                                    UpdatedBy = userContext.Id,
                                    UpdatedDate = DateTimeOffset.Now
                                };
                                var otherAddressXQualifications = Context.DataContext.MstSupplierQualifications.Where(t => otherLocationAndService.SupplierProfile.SupplierQualifications.Contains(t.Id)).ToList();
                                var otherAddressXServingStates = Context.DataContext.MstStates.Where(t => otherLocationAndService.SupplierProfile.ServingStates.Contains(t.Id)).ToList();
                                var otherAddressXProductTypes = Context.DataContext.MstProductTypes.Where(t => otherLocationAndService.SupplierProductTypes.Contains(t.Id)).ToList();

                                //Delete all existing qualifications and add new (if any)
                                externalSupplierOtherAddress.MstSupplierQualifications.ToList().RemoveAll(t => t.Id > 0);
                                externalSupplierOtherAddress.MstSupplierQualifications = otherAddressXQualifications;

                                //Delete all existing serving states and add new (if any)
                                externalSupplierOtherAddress.MstStates.ToList().RemoveAll(t => t.Id > 0);
                                externalSupplierOtherAddress.MstStates = otherAddressXServingStates;

                                //Delete all existing fueltypes and add new
                                externalSupplierOtherAddress.MstProductTypes.ToList().RemoveAll(t => t.Id > 0);
                                externalSupplierOtherAddress.MstProductTypes = otherAddressXProductTypes;

                                externalSupplier.ExternalSupplierAddresses.Add(externalSupplierOtherAddress);
                                if (otherLocationAndService.SupplierProfile.BobtailTransportTankWagon != null && otherLocationAndService.SupplierProfile.BobtailTransportTankWagon.Count > 0)
                                {
                                    foreach (var bobtailTransportTankWagon in otherLocationAndService.SupplierProfile.BobtailTransportTankWagon)
                                    {
                                        var externalSupplierAddressTruckType = new ExternalSupplierAddressTruckType
                                        {
                                            ExternalSupplierAddress = externalSupplierOtherAddress,
                                            TruckTypeId = bobtailTransportTankWagon
                                        };
                                        Context.DataContext.ExternalSupplierAddressTruckTypes.Add(externalSupplierAddressTruckType);
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(viewModel.CompanyDetails.Notes))
                        {
                            var externalSupplierNote = new ExternalSupplierNote
                            {
                                IsCompleted = false,
                                IsActive = true,
                                Note = viewModel.CompanyDetails.Notes,
                                CreatedBy = userContext.Id,
                                CreatedDate = DateTimeOffset.Now,
                                UpdatedBy = userContext.Id,
                                UpdatedDate = DateTimeOffset.Now
                            };
                            externalSupplier.ExternalSupplierNotes.Add(externalSupplierNote);
                        }

                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("SuperAdminDomain", "UpdateExternalSupplierAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<ExternalSupplierDetailsModel> GetExternalSupplierDetailsAsync(int externalSupplierId)
        {
            var externalSupplierViewModel = new ExternalSupplierDetailsModel();
            try
            {
                var externalSupplier = await Context.DataContext.ExternalSuppliers.Where(t => t.Id == externalSupplierId).FirstAsync();
                externalSupplierViewModel.Id = externalSupplier.Id;
                externalSupplierViewModel.CompanyAddress = externalSupplier.ExternalSupplierAddresses.First(t => t.IsActive).ToViewModel(new ExternalSupplierAddressDetail());
                externalSupplierViewModel.Name = externalSupplier.Name;
                externalSupplierViewModel.InPipedrive = externalSupplier.InPipedrive;
                externalSupplierViewModel.Website = externalSupplier.Website;
                externalSupplierViewModel.CompanyType = Enum.GetName(typeof(ExternalSupplierType), externalSupplier.CompanyTypeId);
                externalSupplierViewModel.ContactPersonName = externalSupplier.ContactPersonName;
                externalSupplierViewModel.ContactPersonEmail = externalSupplier.ContactPersonEmail;
                externalSupplierViewModel.ContactPersonPhoneNumber = externalSupplier.ContactPersonPhoneNumber;
                externalSupplierViewModel.OtherLocationsAndServices = externalSupplier.ExternalSupplierAddresses.Where(t => t.IsActive).Skip(1).ToList().ToViewModel(new List<ExternalSupplierAddressDetail>());
                externalSupplierViewModel.Status = Enum.GetName(typeof(ExternalSupplierStatuses), externalSupplier.ExternalSupplierStatuses.First(t => t.IsActive).StatusId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetExternalSupplierDetailsAsync", ex.Message, ex);
            }
            return externalSupplierViewModel;
        }

        public async Task<List<ExternalSupplierNotesGridViewModel>> GetExternalSupplierNotesAsync(int externalSupplierId)
        {
            var externalSupplierViewModel = new List<ExternalSupplierNotesGridViewModel>();
            try
            {
                var externalSupplierNotes = Context.DataContext.ExternalSupplierNotes.Where(t => t.ExternalSupplierId == externalSupplierId);

                await externalSupplierNotes.ForEachAsync(t => externalSupplierViewModel.Add(new ExternalSupplierNotesGridViewModel()
                {
                    Id = t.Id,
                    Notes = t.Note,
                    IsCompleted = t.IsCompleted,
                    DateAdded = t.CreatedDate.ToString(@Resource.constFormatDate),
                    AddedBy = Context.DataContext.Users.Where(t1 => t1.Id == t.CreatedBy).Select(t1 => (t1.FirstName + " " + t1.LastName)).FirstOrDefault(),
                    DateCompleted = t.IsCompleted ? t.UpdatedDate.ToString(@Resource.constFormatDate) : Resource.lblHyphen,
                    CompletedBy = t.IsCompleted ? Context.DataContext.Users.Where(t1 => t1.Id == t.UpdatedBy).Select(t1 => (t1.FirstName + " " + t1.LastName)).FirstOrDefault() : Resource.lblHyphen,
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetExternalSupplierNotesAsync", ex.Message, ex);
            }
            return externalSupplierViewModel;
        }

        public async Task<Status> CompleteExternalSupplierNote(UserContext userContext, int noteId)
        {
            Status response = new Status();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var note = await Context.DataContext.ExternalSupplierNotes.SingleOrDefaultAsync(t => t.Id == noteId);
                    if (note != null)
                    {
                        note.IsCompleted = true;
                        note.UpdatedBy = userContext.Id;
                        note.UpdatedDate = DateTimeOffset.Now;
                        await Context.CommitAsync();

                        transaction.Commit();

                        response = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SuperAdminDomain", "CompleteExternalSupplierNote", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<ExternalSupplierStatusesGridViewModel>> GetExternalSupplierStatusesAsync(int externalSupplierId)
        {
            var externalSupplierViewModel = new List<ExternalSupplierStatusesGridViewModel>();
            try
            {
                var externalSupplierStatuses = Context.DataContext.ExternalSupplierStatuses.Where(t => t.ExternalSupplierId == externalSupplierId);

                await externalSupplierStatuses.ForEachAsync(t => externalSupplierViewModel.Add(new ExternalSupplierStatusesGridViewModel()
                {
                    Id = t.Id,
                    Status = Enum.GetName(typeof(ExternalSupplierStatuses), t.StatusId),
                    DateAdded = t.CreatedDate.ToString(@Resource.constFormatDate),
                    AddedBy = Context.DataContext.Users.Where(t1 => t1.Id == t.CreatedBy).Select(t1 => (t1.FirstName + " " + t1.LastName)).FirstOrDefault(),
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetExternalSupplierStatusesAsync", ex.Message, ex);
            }
            return externalSupplierViewModel;
        }

        public async Task<Status> ChangeExternalSupplierStatusAsync(UserContext userContext, int externalSupplierId, int statusId)
        {
            Status response = Status.Failed;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var externalSupplier = await Context.DataContext.ExternalSuppliers.SingleOrDefaultAsync(t => t.Id == externalSupplierId);
                    if (externalSupplier != null)
                    {
                        externalSupplier.ExternalSupplierStatuses.ToList().ForEach(t => t.IsActive = false);
                        ExternalSupplierStatus externalSupplierStatus = new ExternalSupplierStatus();
                        externalSupplierStatus.StatusId = statusId;
                        externalSupplierStatus.IsActive = true;
                        externalSupplierStatus.CreatedBy = userContext.Id;
                        externalSupplierStatus.CreatedDate = DateTimeOffset.Now;
                        externalSupplier.ExternalSupplierStatuses.Add(externalSupplierStatus);

                        await Context.CommitAsync();

                        transaction.Commit();

                        response = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SuperAdminDomain", "ChangeExternalSupplierStatus", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<Status> AddNewExternalSupplierNoteAsync(UserContext userContext, int externalSupplierId, string note)
        {
            Status response = Status.Failed;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var externalSupplier = await Context.DataContext.ExternalSuppliers.SingleOrDefaultAsync(t => t.Id == externalSupplierId);
                    if (externalSupplier != null)
                    {
                        ExternalSupplierNote externalSupplierNote = new ExternalSupplierNote();
                        externalSupplierNote.Note = note;
                        externalSupplierNote.CreatedBy = userContext.Id;
                        externalSupplierNote.CreatedDate = DateTimeOffset.Now;
                        externalSupplier.ExternalSupplierNotes.Add(externalSupplierNote);

                        await Context.CommitAsync();

                        transaction.Commit();

                        response = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SuperAdminDomain", "AddNewExternalSupplierNoteAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<ExternalSupplierViewModel> ExternalSupplierDetailsForEditAsync(int externalSupplierId)
        {
            var externalSupplierViewModel = new ExternalSupplierViewModel();
            try
            {
                var externalSupplier = await Context.DataContext.ExternalSuppliers.Where(t => t.Id == externalSupplierId).FirstAsync();
                externalSupplierViewModel.CompanyDetails.Id = externalSupplier.Id;
                externalSupplierViewModel.CompanyDetails.Name = externalSupplier.Name;
                externalSupplierViewModel.CompanyDetails.InPipedrive = externalSupplier.InPipedrive;
                externalSupplierViewModel.CompanyDetails.Website = externalSupplier.Website;
                externalSupplierViewModel.CompanyDetails.CompanyTypeId = externalSupplier.CompanyTypeId;
                externalSupplierViewModel.ContactPersonDetails.Name = externalSupplier.ContactPersonName;
                externalSupplierViewModel.ContactPersonDetails.Email = externalSupplier.ContactPersonEmail;
                externalSupplierViewModel.ContactPersonDetails.PhoneNumber = externalSupplier.ContactPersonPhoneNumber;
                externalSupplierViewModel.OtherLocationsAndServices = externalSupplier.ExternalSupplierAddresses.Where(t => t.IsActive).OrderBy(o => o.Id).ToList().ToViewModel(new List<LocationsViewModel>());
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetExternalSupplierDetailsAsync", ex.Message, ex);
            }
            return externalSupplierViewModel;
        }


        public StatusViewModel ValidateExternalSuppliersBulkUploadCsv(string csvText, string csvFilePath)
        {
            using (var tracer = new Tracer("SuperAdminDomain", "ValidateExternalSuppliersBulkUploadCsv"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    var csvHeaderLine = Regex.Matches(csvText.Trim(), @"^.*Company Name.*\n").Cast<Match>().FirstOrDefault();
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

                    csvText = Regex.Replace(csvText.Trim(), @"^.*Company Name.*\n", string.Empty, RegexOptions.IgnoreCase);
                    csvText = Regex.Replace(csvText.Trim(), @",,,,,,,,,,,,,,,,,,,,,,,,,,\n", string.Empty, RegexOptions.IgnoreCase);
                    var indexOfGuideline = csvText.IndexOf("SFX Guidelines");
                    if (indexOfGuideline > 0)
                    {
                        csvText = csvText.Substring(0, indexOfGuideline);
                    }

                    var engine = new FileHelperEngine<ExternalSuppliersCsvRecordViewModel>();
                    var csvExternalSupplierList = engine.ReadString(csvText).ToList();
                    csvExternalSupplierList = csvExternalSupplierList.Where(t => !string.IsNullOrWhiteSpace(t.CompanyName) && t.CompanyName != "Company Name").ToList();
                    var zipcodeRegEx = @"^\d{5}(-\d{4})?$)|(^[A-Z]{1}\d{1}[A-Z]{1} *\d{1}[A-Z]{1}\d{1}$";

                    if (csvExternalSupplierList.Count > 0)
                    {

                        var allStates = Context.DataContext.MstStates.Where(t => t.IsActive).ToList();
                        var allExternalCompanies = Context.DataContext.ExternalSuppliers.Where(t => t.IsActive).ToList();
                        var allFuelTypes = Context.DataContext.MstProductTypes.Where(t => t.IsActive).Select(t => t.Name.ToLower().Trim()).OrderBy(t => t).ToList();
                        var allQualifications = Context.DataContext.MstSupplierQualifications.Where(t => t.IsActive).Select(t => t.Name.ToLower().Trim()).ToList();

                        int lineNumberOfCSV = 1;
                        foreach (var record in csvExternalSupplierList)
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

                            //validate external supplier
                            if (allExternalCompanies.Select(t => t.Name.ToLower().Trim()).Contains(record.CompanyName.ToLower().Trim()))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageExternalSupplierExists, record.CompanyName);
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
                            if (!Regex.IsMatch((record.PhoneNo.Trim().Replace(" ", "").Replace("-", "")), @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")
                                || !Regex.IsMatch((record.ContactPersonPhoneNo.Trim().Replace(" ", "").Replace("-", "")), @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")
                                || (!string.IsNullOrWhiteSpace(record.PhoneNo1) && !Regex.IsMatch((record.PhoneNo1.Trim().Replace(" ", "").Replace("-", "")), @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
                                || (!string.IsNullOrWhiteSpace(record.PhoneNo2) && !Regex.IsMatch((record.PhoneNo2.Trim().Replace(" ", "").Replace("-", "")), @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
                                )
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageInvalidPhoneNumber, record.PhoneNo);
                                return response;
                            }

                            //validate fuel type
                            if (!string.IsNullOrWhiteSpace(record.FuelType) && record.FuelType.ToLower().Trim() != "all")
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
                            if (record.ServingStates.ToLower().Trim() != "all")
                            {
                                var states = record.ServingStates.ToLower().Trim().Split(',').ToList();
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
                        response.StatusMessage = Resource.successMessageForExternalSupplierBulkUpload;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageCanNotProcessZeroRecords;
                    }

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("SettingsDomain", "ValidateExternalSuppliersBulkUploadCsv", ex.Message, ex);
                }

                return response;
            }
        }
        private bool IsRequiredFieldMissing(ExternalSuppliersCsvRecordViewModel record) => string.IsNullOrWhiteSpace(record.CompanyName) || string.IsNullOrWhiteSpace(record.CompanyType) || string.IsNullOrWhiteSpace(record.ContactPersonName) || string.IsNullOrWhiteSpace(record.ContactPersonPhoneNo) || string.IsNullOrWhiteSpace(record.ContactPersonEmail) || string.IsNullOrWhiteSpace(record.Address) || string.IsNullOrWhiteSpace(record.City)
               || string.IsNullOrWhiteSpace(record.State) || string.IsNullOrWhiteSpace(record.ZipCode) || string.IsNullOrWhiteSpace(record.PhoneNo) || string.IsNullOrWhiteSpace(record.ServingStates);

        public async Task<StatusViewModel> SaveBulkExternalSuppliersAsync(string csvText, int userId, int companyId = 0)
        {
            using (var tracer = new Tracer("SuperAdminDomain", "SaveBulkExternalSuppliersAsync"))
            {
                var response = new StatusViewModel();
                try
                {
                    LogManager.Logger.WriteInfo("SuperAdminDomain", "SaveBulkExternalSuppliersAsync", "\n\n[" + csvText + "]\n\n");
                    csvText = Regex.Replace(csvText.Trim(), @"^.*Company Name.*\n", string.Empty, RegexOptions.IgnoreCase);
                    csvText = Regex.Replace(csvText.Trim(), @",,,,,,,,,,,,,,,,,,,,,,,,,,\n", string.Empty, RegexOptions.IgnoreCase);
                    var indexOfGuideline = csvText.IndexOf("SFX Guidelines");
                    if (indexOfGuideline > 0)
                    {
                        csvText = csvText.Substring(0, indexOfGuideline);
                    }

                    var engine = new FileHelperEngine<ExternalSuppliersCsvRecordViewModel>();
                    var csvExternalSuppliersList = engine.ReadString(csvText).ToList();
                    csvExternalSuppliersList = csvExternalSuppliersList.Where(t => !string.IsNullOrWhiteSpace(t.CompanyName) && t.Address != "Company Name").ToList();

                    int row = 0;
                    var invalidAddressList = new List<int>();
                    var invalidDataList = new List<int>();
                    foreach (var item in csvExternalSuppliersList)
                    {
                        row = row + 1;
                        var status = await SaveExternalSupplierAsync(item, userId, companyId);
                        if (status.StatusCode == Status.Failed)
                        {
                            if (status.StatusMessage == Resource.errMessageInvalidData)
                                invalidDataList.Add(row);
                            else if (status.StatusMessage == Resource.errMsgInvalidCombinationOfAddress)
                                invalidAddressList.Add(row);
                        }
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageForExternalSupplierBulkUpload;
                    if (invalidAddressList.Count > 0 && csvExternalSuppliersList.Count == invalidAddressList.Count)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMsgInvalidCombinationOfAddress;
                    }
                    else if (invalidAddressList.Count > 0 && csvExternalSuppliersList.Count != invalidAddressList.Count)
                    {
                        if (invalidDataList.Any())
                        {
                            response.StatusMessage = "For rows " + string.Join(",", invalidDataList) + " " + Resource.errMessageInvalidData.ToLower() + "<br/>" +
                                "For rows " + string.Join(",", invalidAddressList) + " " + Resource.errMsgInvalidCombinationOfAddress.ToLower() + "<br/>" +
                                "Other " + Resource.successMessageForExternalSupplierBulkUpload.ToLower();
                        }
                        else
                        {
                            response.StatusMessage = "For rows " + string.Join(",", invalidAddressList) + " " + Resource.errMsgInvalidCombinationOfAddress.ToLower() + "<br/> Other " + Resource.successMessageForExternalSupplierBulkUpload.ToLower();
                        }
                    }
                    else if (invalidDataList.Count > 0 && csvExternalSuppliersList.Count == invalidDataList.Count)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageInvalidData;
                    }
                    else if (invalidDataList.Count > 0 && csvExternalSuppliersList.Count != invalidDataList.Count)
                    {
                        response.StatusMessage = "For rows " + string.Join(",", invalidDataList) + " " + Resource.errMessageInvalidData.ToLower() + "<br/> Other " + Resource.successMessageForExternalSupplierBulkUpload.ToLower();
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("SuperAdminDomain", "SaveBulkExternalSuppliersAsync", ex.Message, ex);
                }

                return response;
            }
        }

        private async Task<StatusViewModel> SaveExternalSupplierAsync(ExternalSuppliersCsvRecordViewModel externalSuppliersCsvRecordViewModel, int userId, int companyId)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);

            try
            {
                ExternalSupplier externalSupplier = new ExternalSupplier
                {
                    Name = externalSuppliersCsvRecordViewModel.CompanyName,
                    CompanyTypeId = (int)Enum.Parse(typeof(ExternalSupplierType), externalSuppliersCsvRecordViewModel.CompanyType.Replace(" ", string.Empty)),
                    Website = externalSuppliersCsvRecordViewModel.Website,
                    InPipedrive = string.IsNullOrEmpty(externalSuppliersCsvRecordViewModel.InPipedrive) ? false : externalSuppliersCsvRecordViewModel.InPipedrive.ToLower() == "true" ? true : false,
                    ContactPersonName = externalSuppliersCsvRecordViewModel.ContactPersonName,
                    ContactPersonEmail = externalSuppliersCsvRecordViewModel.ContactPersonEmail,
                    ContactPersonPhoneNumber = externalSuppliersCsvRecordViewModel.ContactPersonPhoneNo,
                    IsActive = true,
                    CreatedBy = userId,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedBy = userId,
                    UpdatedDate = DateTimeOffset.Now
                };

                Context.DataContext.ExternalSuppliers.Add(externalSupplier);

                var externalSupplierMainAddress = SetValuesToAddressEntity(userId, externalSupplier.Id, externalSuppliersCsvRecordViewModel.Address, externalSuppliersCsvRecordViewModel.City, externalSuppliersCsvRecordViewModel.State, externalSuppliersCsvRecordViewModel.ZipCode, externalSuppliersCsvRecordViewModel.PhoneNo, externalSuppliersCsvRecordViewModel);
                Context.DataContext.ExternalSupplierAddresses.Add(externalSupplierMainAddress);

                if (!string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.Address1))
                {
                    var externalSupplierAddress1 = SetValuesToAddressEntity(userId, externalSupplier.Id, externalSuppliersCsvRecordViewModel.Address1, externalSuppliersCsvRecordViewModel.City1, externalSuppliersCsvRecordViewModel.State1, externalSuppliersCsvRecordViewModel.ZipCode1, externalSuppliersCsvRecordViewModel.PhoneNo1, externalSuppliersCsvRecordViewModel);
                    Context.DataContext.ExternalSupplierAddresses.Add(externalSupplierAddress1);
                }
                if (!string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.Address2))
                {
                    var externalSupplierAddress2 = SetValuesToAddressEntity(userId, externalSupplier.Id, externalSuppliersCsvRecordViewModel.Address2, externalSuppliersCsvRecordViewModel.City2, externalSuppliersCsvRecordViewModel.State2, externalSuppliersCsvRecordViewModel.ZipCode2, externalSuppliersCsvRecordViewModel.PhoneNo2, externalSuppliersCsvRecordViewModel);
                    Context.DataContext.ExternalSupplierAddresses.Add(externalSupplierAddress2);
                }

                if (!string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.Notes))
                {
                    var externalSupplierNote = new ExternalSupplierNote
                    {
                        ExternalSupplierId = externalSupplier.Id,
                        IsCompleted = false,
                        IsActive = true,
                        Note = externalSuppliersCsvRecordViewModel.Notes,
                        CreatedBy = userId,
                        CreatedDate = DateTimeOffset.Now,
                        UpdatedBy = userId,
                        UpdatedDate = DateTimeOffset.Now
                    };
                    Context.DataContext.ExternalSupplierNotes.Add(externalSupplierNote);
                }

                var externalSupplierStatus = new ExternalSupplierStatus
                {
                    ExternalSupplierId = externalSupplier.Id,
                    StatusId = (int)ExternalSupplierStatuses.New,
                    IsActive = true,
                    CreatedBy = userId,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedBy = userId,
                    UpdatedDate = DateTimeOffset.Now
                };
                Context.DataContext.ExternalSupplierStatuses.Add(externalSupplierStatus);

                await Context.CommitAsync();

                response.StatusCode = Status.Success;
                return response;
            }
            catch (InvalidDataException ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = ex.Message;
                return response;
            }
            catch (Exception)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageInvalidData;
                return response;
            }
        }

        private string GenerateFileName()
        {
            return string.Format("ExternalMeterData-{0}.txt", DateTime.Now.Ticks);
        }

        private QueueMessageViewModel GetEnqueueMessageRequestViewModel(UserContext userContext, string blobStoragePath, ExternalMeterDataViewModel viewModel)
        {
            var jsonViewModel = new ExternalMeterDataUploadQueueMsg()
            {
                FileUploadPath = blobStoragePath,
                SupplierCompanyId = viewModel.SupplierCompanyId
            };

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.ExternalMeterDataUpload,
                JsonMessage = json
            };
        }

        public async Task<StatusViewModel> UploadFileToBlob(UserContext userContext, Stream fileStream, ExternalMeterDataViewModel viewModel)
        {
            using (var tracer = new Tracer("SuperAdminDomain", "UploadFileToBlob"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var azureBlob = new AzureBlobStorage();
                    var filePath = await azureBlob.SaveBlobAsync(fileStream, GenerateFileName(), BlobContainerType.ExternalMeterFeed.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        var queueDomain = new QueueMessageDomain();
                        var queueRequest = GetEnqueueMessageRequestViewModel(userContext, filePath, viewModel);
                        var queueId = queueDomain.EnqeueMessage(queueRequest);

                        var redirectPath = Resource.urlAMPStatusPath;
                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMessageForExternalMeterDataUpload, queueId, redirectPath);
                    }
                    else
                        response.StatusMessage = Resource.errMessageErrorInAzureServer;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    LogManager.Logger.WriteException("SuperAdminDomain", "UploadFileToBlob", ex.Message, ex);
                }
                return response;
            }
        }

        private ExternalSupplierAddress SetValuesToAddressEntity(int userId, int externalSupplierId, string address, string city, string stateCode, string zipCode, string phoneNo, ExternalSuppliersCsvRecordViewModel externalSuppliersCsvRecordViewModel)
        {
            var externalSupplierMainAddress = new ExternalSupplierAddress();
            var truckTypes = Enum.GetValues(typeof(Trucks)).Cast<Trucks>().ToList().Select(t => t);
            try
            {
                externalSupplierMainAddress.ExternalSupplierId = externalSupplierId;
                externalSupplierMainAddress.Address = address;
                externalSupplierMainAddress.City = city;
                externalSupplierMainAddress.ZipCode = zipCode;
                externalSupplierMainAddress.CountryId = 1;
                externalSupplierMainAddress.PhoneTypeId = 1;
                externalSupplierMainAddress.PhoneNumber = phoneNo;
                externalSupplierMainAddress.IsStateWideService = string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.SpecificRadius) ? true : false;
                externalSupplierMainAddress.Radius = string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.SpecificRadius) ? 0 : Convert.ToInt32(externalSuppliersCsvRecordViewModel.SpecificRadius);
                externalSupplierMainAddress.NumberOfTrucks = string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.HowManyTrucks) ? null : (int?)Convert.ToInt32(externalSuppliersCsvRecordViewModel.HowManyTrucks);
                externalSupplierMainAddress.IsActive = true;
                externalSupplierMainAddress.CreatedBy = userId;
                externalSupplierMainAddress.CreatedDate = DateTimeOffset.Now;
                externalSupplierMainAddress.UpdatedBy = userId;
                externalSupplierMainAddress.UpdatedDate = DateTimeOffset.Now;

                var state = Context.DataContext.MstStates.FirstOrDefault(t => t.Code.ToLower() == stateCode.ToLower());
                if (state != null)
                {
                    externalSupplierMainAddress.StateId = state.Id;
                    var point = GoogleApiDomain.GetGeocode($"{externalSupplierMainAddress.Address} {externalSupplierMainAddress.City} {state.Code} {"USA"} {externalSupplierMainAddress.ZipCode}");
                    if (point != null)
                    {
                        externalSupplierMainAddress.Latitude = Convert.ToDecimal(point.Latitude);
                        externalSupplierMainAddress.Longitude = Convert.ToDecimal(point.Longitude);
                    }
                    else
                    {
                        throw new InvalidDataException(Resource.errMsgInvalidCombinationOfAddress);
                    }
                }
                var supplierAddressXQualifications = new List<MstSupplierQualification>();

                if (!(string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.DBE)) && externalSuppliersCsvRecordViewModel.DBE.ToLower() == "all")
                {
                    supplierAddressXQualifications = Context.DataContext.MstSupplierQualifications.ToList();
                }
                else if (!(string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.DBE)))
                {
                    var dbeArray = externalSuppliersCsvRecordViewModel.DBE.ToLower().Split(',');
                    supplierAddressXQualifications = Context.DataContext.MstSupplierQualifications.Where(t => dbeArray.Contains(t.Name.ToLower())).ToList();
                }
                externalSupplierMainAddress.MstSupplierQualifications = supplierAddressXQualifications;

                var supplierAddressXServingStates = new List<MstState>();
                if (!(string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.ServingStates)) && externalSuppliersCsvRecordViewModel.ServingStates.ToLower() == "all")
                {
                    supplierAddressXServingStates = Context.DataContext.MstStates.ToList();
                }
                else if (!(string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.ServingStates)))
                {
                    var statesArray = externalSuppliersCsvRecordViewModel.ServingStates.ToLower().Split(',');
                    supplierAddressXServingStates = Context.DataContext.MstStates.Where(t => statesArray.Contains(t.Name.ToLower())).ToList();
                }
                externalSupplierMainAddress.MstStates = supplierAddressXServingStates;

                var supplierAddressXProductTypes = new List<MstProductType>();
                if (!(string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.FuelType)) && externalSuppliersCsvRecordViewModel.FuelType.ToLower() == "all")
                {
                    supplierAddressXProductTypes = Context.DataContext.MstProductTypes.ToList();
                }
                else if (!(string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.FuelType)))
                {
                    var fuelTypesArray = externalSuppliersCsvRecordViewModel.FuelType.ToLower().Split(',');
                    supplierAddressXProductTypes = Context.DataContext.MstProductTypes.Where(t => fuelTypesArray.Contains(t.Name.ToLower())).ToList();
                }
                externalSupplierMainAddress.MstProductTypes = supplierAddressXProductTypes;

                var truckType = new List<int>();
                if (!(string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.TruckType)) && externalSuppliersCsvRecordViewModel.TruckType.ToLower() == "all")
                {
                    truckType = truckTypes.Select(t => (int)t).ToList();
                    foreach (var bobtailTransportTankWagon in truckType)
                    {
                        var externalSupplierAddressTruckType = new ExternalSupplierAddressTruckType
                        {
                            ExternalSupplierAddress = externalSupplierMainAddress,
                            TruckTypeId = bobtailTransportTankWagon
                        };
                        Context.DataContext.ExternalSupplierAddressTruckTypes.Add(externalSupplierAddressTruckType);
                    }
                }
                else if (!(string.IsNullOrWhiteSpace(externalSuppliersCsvRecordViewModel.TruckType)))
                {
                    var truckTypesArray = externalSuppliersCsvRecordViewModel.TruckType.Split(',');

                    if (truckTypesArray != null && truckTypesArray.Count() > 0)
                    {
                        foreach (var bobtailTransportTankWagon in truckTypesArray)
                        {
                            var externalSupplierAddressTruckType = new ExternalSupplierAddressTruckType
                            {
                                ExternalSupplierAddress = externalSupplierMainAddress,
                                TruckTypeId = (int)Enum.Parse(typeof(Trucks), bobtailTransportTankWagon.Replace(" ", string.Empty))
                            };
                            Context.DataContext.ExternalSupplierAddressTruckTypes.Add(externalSupplierAddressTruckType);
                        }
                    }
                }

                return externalSupplierMainAddress;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "SetValuesToAddressEntity", ex.Message, ex);
                throw;
            }
        }

        public async Task<DashboardFuelRequestViewModel> FuelRequestsStats()
        {
            var response = new DashboardFuelRequestViewModel();
            try
            {
                string fuelRequestTypes = string.Empty, fuelRequestStatuses = string.Empty;

                fuelRequestTypes = string.Join(",", (int)FuelRequestType.FuelRequest, (int)FuelRequestType.BrokeredFuelRequest, (int)FuelRequestType.ThirdPartyRequest);
                fuelRequestStatuses = string.Join(",", (int)FuelRequestFilterType.Open, (int)FuelRequestFilterType.Expired, (int)FuelRequestFilterType.Accepted);

                var fuelRequeststats = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAllFuelRequests(string.Empty, string.Empty, fuelRequestTypes, fuelRequestStatuses);

                if (fuelRequeststats != null)
                {
                    response.ThirdPartyFRCount = fuelRequeststats.ThirdPartyFRCount;
                    response.OpenFuelRequestCount = fuelRequeststats.OpenFuelRequestCount;
                    response.BrokeredFuelRequestRequestCount = fuelRequeststats.BrokeredFuelRequestRequestCount;
                    response.ExpiredFuelRequestCount = fuelRequeststats.ExpiredFuelRequestCount;
                    response.AboutToExpireCount = fuelRequeststats.AboutToExpireCount;
                    response.TotalFuelRequestCount = fuelRequeststats.TotalFuelRequestCount;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestDomain", "FuelRequestsStats", ex.Message, ex);
            }
            return response;
        }

        public async Task<CompanyGroupViewModel> CreateCompanyGroup(CompanyGroupViewModel model)
        {
            var response = new CompanyGroupViewModel();
            using (var tracer = new Tracer("SuperAdminDomain", "CreateCompanyGroup"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.OwnerCompanyId <= 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectParentAccount;
                            return response;
                        }

                        if (string.IsNullOrWhiteSpace(model.SeletedChildCompanies))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectChildAccount;
                            return response;
                        }

                        var IsUpdate = false;
                        var selectedChildCompanyIds = model.SeletedChildCompanies.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        var parentCompany = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == model.OwnerCompanyId);
                        if (parentCompany != null)
                        {
                            // remove existing company group mapping if any
                            var existingGroup = Context.DataContext.Companies.Where(t => t.ParentCompanyId == model.OwnerCompanyId);
                            var ids = existingGroup.Select(x => x.Id).ToList();
                            if (existingGroup.Any())
                            {
                                await existingGroup.ForEachAsync(t1 => t1.ParentCompanyId = null);
                                IsUpdate = true;
                            }

                            // create company group
                            for (int i = 0; i < selectedChildCompanyIds.Count; i++)
                            {
                                int id = selectedChildCompanyIds[i];
                                var childCompany = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == id);
                                childCompany.ParentCompanyId = model.OwnerCompanyId;
                            }

                            var deletedChildCompanyIds = ids.Except(selectedChildCompanyIds);

                            await DeleteCompanyFromSubGroups(deletedChildCompanyIds);

                            await Context.CommitAsync();
                            transaction.Commit();
                        }

                        response.StatusCode = Status.Success;

                        if (!IsUpdate)
                            response.StatusMessage = Resource.successMessageCreateAccountGroup;
                        else
                            response.StatusMessage = Resource.successMessageUpdateAccountGroup;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageFailedToCreateGroup;
                        LogManager.Logger.WriteException("SuperAdminDomain", "CreateCompanyGroup", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        private async Task DeleteCompanyFromSubGroups(IEnumerable<int> deletedChildCompanyIds)
        {
            if (deletedChildCompanyIds.Any())
            {
                var subGroupCompanyToDelete = await Context.DataContext.CompanyGroupXCompanies.Where(t => deletedChildCompanyIds.Contains(t.SubCompanyId)).ToListAsync();
                foreach (var item in subGroupCompanyToDelete)
                {
                    Context.DataContext.CompanyGroupXCompanies.Remove(item);
                    await Context.CommitAsync();

                    var group = await Context.DataContext.CompanyGroups.FirstOrDefaultAsync(t => t.Id == item.CompanyGroupId);
                    if (group != null && !group.CompanyGroupXCompanies.Any())
                    {
                        Context.DataContext.CompanyGroups.Remove(group);
                    }
                }
            }
        }

        public async Task<List<CompanyGroupViewModel>> GetAllCompanyGroups(int parentCompanyId = 0)
        {
            var response = new List<CompanyGroupViewModel>();
            using (var tracer = new Tracer("SuperAdminDomain", "GetAllCompanyGroups"))
            {
                try
                {
                    var storedProcedureDomain = new StoredProcedureDomain(this);
                    response = await storedProcedureDomain.GetAllCompanyGroups(parentCompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("SuperAdminDomain", "GetAllCompanyGroups", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<CompanyGroupViewModel> GetGroupDetails(int parentCompanyId = 0)
        {
            var response = new CompanyGroupViewModel();
            using (var tracer = new Tracer("SuperAdminDomain", "GetGroupDetails"))
            {
                try
                {
                    var storedProcedureDomain = new StoredProcedureDomain(this);
                    response = await storedProcedureDomain.GetCompanyGroupDetails(parentCompanyId);
                    response.IsSubGroupExist = await Context.DataContext.CompanyGroups.AnyAsync(t => t.OwnerCompanyId == parentCompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("SuperAdminDomain", "GetGroupDetails", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<StateViewModel> DeleteCompanyGroup(int parentCompanyId)
        {
            var response = new StateViewModel();
            using (var tracer = new Tracer("SuperAdminDomain", "DeleteCompanyGroup"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var parentCompany = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == parentCompanyId);
                        if (parentCompany != null)
                        {
                            var subGroups = await Context.DataContext.CompanyGroups.Where(t => t.OwnerCompanyId == parentCompanyId).ToListAsync();
                            if (subGroups != null && subGroups.Any())
                            {
                                foreach (var subGroup in subGroups)
                                {
                                    var subGroupCompanies = subGroup.CompanyGroupXCompanies.ToList();
                                    if (subGroupCompanies != null && subGroupCompanies.Any())
                                        subGroupCompanies.ToList().RemoveAll(t => t.CompanyGroupId == subGroup.Id);

                                    Context.DataContext.CompanyGroups.Remove(subGroup);
                                    await Context.CommitAsync();
                                }

                            }

                            await Context.DataContext.Companies.Where(t => t.ParentCompanyId == parentCompanyId).ForEachAsync(t1 => t1.ParentCompanyId = null);
                            await Context.CommitAsync();

                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageAcccountGroupDelete;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageAcccountGroupDelete;
                        LogManager.Logger.WriteException("SuperAdminDomain", "DeleteCompanyGroup", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<List<ProductViewModel>> GetProductMappingGridAsync(ProductDataTableViewModel requestModel, DataTableSearchModel model)
        {
            List<ProductViewModel> response = new List<ProductViewModel>();

            try
            {
                if (requestModel == null)
                {
                    requestModel = new ProductDataTableViewModel();
                }

                response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetProductMappingGridAsync(requestModel, model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetProductMappingGridAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<ProductTypeMappingViewModel>> GetBlendProductTypeMappingGridAsync()
        {
            List<ProductTypeMappingViewModel> response = new List<ProductTypeMappingViewModel>();

            try
            {
                var allMappedToProductTypeIds = await Context.DataContext.MstBlendProductTypeMapping.Select(t => new DropdownDisplayExtendedId
                {
                    Id = t.ProductTypeId,
                    CodeId = t.MappedToProductTypeId
                }).ToListAsync();

                var productTypes = await Context.DataContext.MstProductTypes.Where(t => t.IsActive).Select(t => new { t.Id, t.Name }).OrderBy(t => t.Name).ToListAsync();
                foreach (var item in productTypes)
                {
                    ProductTypeMappingViewModel productType = new ProductTypeMappingViewModel();
                    productType.ProductTypeId = item.Id;
                    productType.ProductType = item.Name;
                    var mappedToProductTypeIds = allMappedToProductTypeIds.Where(t => t.Id == item.Id).Select(t => t.CodeId).Distinct().ToList();
                    if (mappedToProductTypeIds.Any())
                    {
                        var mappedToProductType = productTypes.Where(t => mappedToProductTypeIds.Contains(t.Id)).Select(t => t.Name).Distinct().ToList();
                        productType.MappedToProductType = string.Join(",", mappedToProductType);
                        productType.MappedToProductTypeIds = mappedToProductTypeIds;
                    }
                    else
                    {
                        productType.MappedToProductType = Resource.lblHyphen;
                    }
                    response.Add(productType);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetBlendProductTypeMappingGridAsync", ex.Message, ex);
            }

            return response;
        }


        public async Task<List<ProductTypeMappingViewModel>> GetProductTypeMappingGridAsync()
        {
            List<ProductTypeMappingViewModel> response = new List<ProductTypeMappingViewModel>();

            try
            {
                var allMappedToProductTypeIds = await Context.DataContext.ProductTypeCompatibilityMappings.Select(t => new DropdownDisplayExtendedId
                {
                    Id = t.ProductTypeId,
                    CodeId = t.MappedToProductTypeId
                }).ToListAsync();

                var productTypes = await Context.DataContext.MstProductTypes.Where(t => t.IsActive).Select(t => new { t.Id, t.Name }).OrderBy(t => t.Name).ToListAsync();
                foreach (var item in productTypes)
                {
                    ProductTypeMappingViewModel productType = new ProductTypeMappingViewModel();
                    productType.ProductTypeId = item.Id;
                    productType.ProductType = item.Name;
                    var mappedToProductTypeIds = allMappedToProductTypeIds.Where(t => t.Id == item.Id).Select(t => t.CodeId).Distinct().ToList();
                    if (mappedToProductTypeIds.Any())
                    {
                        var mappedToProductType = productTypes.Where(t => mappedToProductTypeIds.Contains(t.Id)).Select(t => t.Name).Distinct().ToList();
                        productType.MappedToProductType = string.Join(",", mappedToProductType);
                        productType.MappedToProductTypeIds = mappedToProductTypeIds;
                    }
                    else
                    {
                        productType.MappedToProductType = Resource.lblHyphen;
                    }
                    response.Add(productType);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetProductTypeMappingGridAsync", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<InvoiceExportViewModel>> GetInvoiceDetailsToExport(InvoiceDataTableViewModel requestModel)
        {
            List<InvoiceExportViewModel> result = new List<InvoiceExportViewModel>();
            using (var tracer = new Tracer("SuperAdminDomain", "ExportInvoicesToCsv"))
            {
                try
                {
                    var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceGridAsync(requestModel);
                    result = response.Select(t => new InvoiceExportViewModel
                    {
                        InvoiceNumber = t.InvoiceNumber?.Replace(",", ""),
                        QbInvoiceNumber = t.QbInvoiceNumber?.Replace(",", ""),
                        PoNumber = t.PoNumber?.Replace(",", ""),
                        Buyer = t.Buyer?.Replace(",", ""),
                        BuyerAccountOwner = t.BuyerAccountOwner?.Replace(",", ""),
                        Supplier = t.Supplier?.Replace(",", ""),
                        SupplierAccountOwner = t.SupplierAccountOwner?.Replace(",", ""),
                        Location = t.Location?.Replace(",", ""),
                        FuelType = t.FuelType?.Replace(",", ""),
                        DropDate = t.DropDate?.Replace(",", ""),
                        DropTime = t.DropTime?.Replace(",", ""),
                        TotalDroppedGallons = t.TotalDroppedGallons?.Replace(",", ""),
                        RackPPG = t.RackPPG?.Replace(",", ""),
                        TotalInvoiceAmount = t.TotalInvoiceAmount?.Replace(",", ""),
                        InvoiceDate = t.InvoiceDate?.Replace(",", ""),
                        PaymentDueDate = t.PaymentDueDate?.Replace(",", ""),
                        PaymentTerms = t.PaymentTerms?.Replace(",", ""),
                        LastEditDate = t.LastEditDate?.Replace(",", ""),
                        Status = t.Status,
                        IsBrokered = !string.IsNullOrWhiteSpace(t.BrokeredChainId) ? "Yes" : ""
                    }).ToList();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("SuperAdminDomain", "ExportInvoicesToCsv", ex.Message, ex);
                }

                return result;
            }
        }

        public async Task<StateViewModel> CreateProductMapping(ProductViewModel productViewModel, UserContext userContext)
        {
            var response = new StateViewModel();
            using (var tracer = new Tracer("SuperAdminDomain", "CreateProductMapping"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var storedProcedureDomain = new StoredProcedureDomain(this);
                        productViewModel.UpdatedBy = userContext.Id;
                        productViewModel.ProductName = productViewModel.DisplayName;
                        if (productViewModel.Id == 0)
                        {
                            var pricingService = await new PricingServiceDomain(this).SaveTfxProduct(productViewModel);
                            if (pricingService != null && pricingService.Result > 0)
                            { 
                               var tfxProductId =  await storedProcedureDomain.SaveNewTfxProductAsync(pricingService.Result, productViewModel.DisplayName, productViewModel.ProductCode, productViewModel.ProductDisplayGroupId, productViewModel.ProductTypeId, productViewModel.ProductDescription);

                                if (productViewModel.AxxisProductId > 0)
                                    await AddProductMapping(tfxProductId, productViewModel.DisplayName, productViewModel.AxxisProductId.Value);

                                if (productViewModel.OpisProductId > 0)
                                    await AddProductMapping(tfxProductId, productViewModel.DisplayName, productViewModel.OpisProductId.Value);

                                if (productViewModel.PlattsProductId > 0)
                                    await AddProductMapping(tfxProductId, productViewModel.DisplayName, productViewModel.PlattsProductId.Value);

                                if ( productViewModel.isNewAxissProductAdded() )
                                {

                                    productViewModel.DisplayName = !string.IsNullOrWhiteSpace(productViewModel.AxxisProductName) ? productViewModel.AxxisProductName.Trim() : productViewModel.DisplayName;
                                    var pricingServiceResponse = await new PricingServiceDomain(this).SaveNewMstProduct(productViewModel, tfxProductId, (int)PricingSource.Axxis);
                                    if (pricingServiceResponse != null && pricingServiceResponse.Result > 0)
                                    {
                                        // save mst product in exchange using stored procedure                                   
                                        await storedProcedureDomain.SaveNewProductAsync(productViewModel.AxxisProductName, pricingServiceResponse.Result, productViewModel.ProductTypeId, productViewModel.ProductDisplayGroupId, (int)PricingSource.Axxis, null, tfxProductId, productViewModel.ProductName);
                                    }

                                }
                                if (productViewModel.isNewOpisProductAdded())
                                {
                                    productViewModel.DisplayName = !string.IsNullOrWhiteSpace(productViewModel.OpisProductName) ? productViewModel.OpisProductName.Trim() : productViewModel.DisplayName;
                                    var pricingServiceResponse = await new PricingServiceDomain(this).SaveNewMstProduct(productViewModel, tfxProductId, (int)PricingSource.OPIS);
                                    if (pricingServiceResponse != null && pricingServiceResponse.Result > 0)
                                    {

                                        await storedProcedureDomain.SaveNewProductAsync(productViewModel.OpisProductName, pricingServiceResponse.Result, productViewModel.ProductTypeId, productViewModel.ProductDisplayGroupId, (int)PricingSource.OPIS, null, tfxProductId, productViewModel.ProductName);
                                    }
                                }
                                if (productViewModel.isNewPlattsProductAdded())
                                {
                                    productViewModel.DisplayName = !string.IsNullOrWhiteSpace(productViewModel.PlattsProductName) ? productViewModel.PlattsProductName.Trim() : productViewModel.DisplayName;
                                    var pricingServiceResponse = await new PricingServiceDomain(this).SaveNewMstProduct(productViewModel, tfxProductId, (int)PricingSource.PLATTS);
                                    if (pricingServiceResponse != null && pricingServiceResponse.Result > 0)
                                    {
                                        await storedProcedureDomain.SaveNewProductAsync(productViewModel.PlattsProductName, pricingServiceResponse.Result, productViewModel.ProductTypeId, productViewModel.ProductDisplayGroupId, (int)PricingSource.PLATTS, null, tfxProductId, productViewModel.ProductName);
                                    }
                                }

                                if (productViewModel.shouldAddNewAxxisProduct())
                                {
                                    productViewModel.DisplayName = !string.IsNullOrWhiteSpace(productViewModel.AxxisProductName) ? productViewModel.AxxisProductName.Trim() : productViewModel.DisplayName;
                                    var pricingServiceResponse = await new PricingServiceDomain(this).SaveNewMstProduct(productViewModel, tfxProductId, (int)PricingSource.Axxis);
                                    if (pricingServiceResponse != null && pricingServiceResponse.Result > 0)
                                    {
                                        // save mst product in exchange using stored procedure 
                                        await storedProcedureDomain.SaveNewProductAsync(productViewModel.DisplayName, pricingServiceResponse.Result, productViewModel.ProductTypeId, productViewModel.ProductDisplayGroupId, (int)PricingSource.Axxis, null, tfxProductId, productViewModel.DisplayName);
                                    }
                                }
                                //if (productViewModel.OpisProductId > 0)
                                //    await AddProductMapping(tfxProduct.Id, productViewModel.DisplayName, productViewModel.OpisProductId.Value, true);

                                transaction.Commit();

                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.successMessageAddedProductMapping;
                            }
                        }
                        else
                        {
                            var pricingService = await new PricingServiceDomain(this).UpdateTfxProduct(productViewModel);
                            if (pricingService != null && pricingService.Result > 0)
                            {
                                var tfxProduct = await Context.DataContext.MstTfxProducts.FirstOrDefaultAsync(t => t.IsActive && t.Id == productViewModel.Id);
                                if (productViewModel.Id > 0 && tfxProduct != null)
                                {
                                    tfxProduct.Name = productViewModel.DisplayName;
                                    tfxProduct.ProductCode = productViewModel.ProductCode;
                                    tfxProduct.ProductDisplayGroupId = productViewModel.ProductDisplayGroupId;
                                    tfxProduct.ProductTypeId = productViewModel.ProductTypeId;
                                    tfxProduct.ProductDescription = productViewModel.ProductDescription;
                                    tfxProduct.IsActive = true;
                                    tfxProduct.UpdatedBy = productViewModel.UpdatedBy;
                                    tfxProduct.UpdatedDate = DateTimeOffset.Now;

                                    Context.DataContext.Entry(tfxProduct).State = EntityState.Modified;
                                    await Context.CommitAsync();

                                    if (productViewModel.AxxisProductId > 0)
                                        await UpdateProductMapping(tfxProduct, productViewModel.DisplayName, productViewModel.AxxisProductId.Value, (int)PricingSource.Axxis);
                                    else
                                        await RemoveProductMapping(tfxProduct, (int)PricingSource.Axxis);

                                    if (productViewModel.OpisProductId > 0)
                                        await UpdateProductMapping(tfxProduct, productViewModel.DisplayName, productViewModel.OpisProductId.Value, (int)PricingSource.OPIS);
                                    else
                                        await RemoveProductMapping(tfxProduct, (int)PricingSource.OPIS);

                                    if (productViewModel.PlattsProductId > 0)
                                        await UpdateProductMapping(tfxProduct, productViewModel.DisplayName, productViewModel.PlattsProductId.Value, (int)PricingSource.PLATTS);
                                    else
                                        await RemoveProductMapping(tfxProduct, (int)PricingSource.PLATTS);
                                    if (productViewModel.isNewAxissProductAdded())
                                    {
                                        productViewModel.Id = 0; // set to 0 allow adding new product
                                        productViewModel.DisplayName = !string.IsNullOrWhiteSpace(productViewModel.AxxisProductName) ? productViewModel.AxxisProductName.Trim() : productViewModel.DisplayName;
                                        var pricingServiceResponse = await new PricingServiceDomain(this).SaveNewMstProduct(productViewModel, tfxProduct.Id, (int)PricingSource.Axxis);
                                        if (pricingServiceResponse != null && pricingServiceResponse.Result > 0)
                                        {

                                            await storedProcedureDomain.SaveNewProductAsync(productViewModel.AxxisProductName, pricingServiceResponse.Result, productViewModel.ProductTypeId, productViewModel.ProductDisplayGroupId, (int)PricingSource.Axxis, null, tfxProduct.Id, productViewModel.ProductName);
                                        }
                                    }
                                    if (productViewModel.isNewOpisProductAdded())
                                    {
                                        productViewModel.Id = 0; // set to 0 allow adding new product
                                        var pricingServiceResponse = await new PricingServiceDomain(this).SaveNewMstProduct(productViewModel, tfxProduct.Id, (int)PricingSource.OPIS);
                                        if (pricingServiceResponse != null && pricingServiceResponse.Result > 0)
                                        {
                                            await storedProcedureDomain.SaveNewProductAsync(productViewModel.OpisProductName, pricingServiceResponse.Result, productViewModel.ProductTypeId, productViewModel.ProductDisplayGroupId, (int)PricingSource.OPIS, null, tfxProduct.Id, productViewModel.ProductName);
                                        }
                                    }
                                    if (productViewModel.isNewPlattsProductAdded())
                                    {
                                        productViewModel.Id = 0; // set to 0 allow adding new product
                                        var pricingServiceResponse = await new PricingServiceDomain(this).SaveNewMstProduct(productViewModel, tfxProduct.Id, (int)PricingSource.PLATTS);
                                        if (pricingServiceResponse != null && pricingServiceResponse.Result > 0)
                                        {
                                            await storedProcedureDomain.SaveNewProductAsync(productViewModel.PlattsProductName, pricingServiceResponse.Result, productViewModel.ProductTypeId, productViewModel.ProductDisplayGroupId, (int)PricingSource.PLATTS, null, tfxProduct.Id, productViewModel.ProductName);
                                        }
                                    }

                                    if (productViewModel.shouldAddNewAxxisProduct())
                                    {
                                        productViewModel.Id = 0; // set to 0 allow adding new product
                                        productViewModel.DisplayName = !string.IsNullOrWhiteSpace(productViewModel.AxxisProductName) ? productViewModel.AxxisProductName.Trim() : productViewModel.DisplayName;
                                        var pricingServiceResponse = await new PricingServiceDomain(this).SaveNewMstProduct(productViewModel, tfxProduct.Id, (int)PricingSource.Axxis);
                                        if (pricingServiceResponse != null && pricingServiceResponse.Result > 0)
                                        {
                                            await storedProcedureDomain.SaveNewProductAsync(productViewModel.DisplayName, pricingServiceResponse.Result, productViewModel.ProductTypeId, productViewModel.ProductDisplayGroupId, (int)PricingSource.Axxis, null, tfxProduct.Id, productViewModel.DisplayName);
                                        }
                                    }
                                    transaction.Commit();
                                }

                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.successMessageUpdateProductMapping;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageCreateProductMapping;
                        LogManager.Logger.WriteException("SuperAdminDomain", "CreateProductMapping", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        private async Task AddProductMapping(int tfxProductId, string displayName, int productId, bool isActualOpis = false)
        {
            var product = await Context.DataContext.MstProducts.FirstOrDefaultAsync(t => t.IsActive && t.Id == productId);
            if (product != null)
            {
                product.TfxProductId = tfxProductId;
                product.DisplayName = displayName;
                Context.DataContext.Entry(product).State = EntityState.Modified;
                await Context.CommitAsync();
            }

            //if (isActualOpis)
            //{
            //    var opisProduct = await Context.DataContext.MstOPISProducts.FirstOrDefaultAsync(t => t.IsActive && t.Id == productId);
            //    if (opisProduct != null)
            //    {
            //        opisProduct.TfxProductId = tfxProductId;
            //        opisProduct.DisplayName = displayName;
            //        Context.DataContext.Entry(opisProduct).State = EntityState.Modified;

            //    }
            //}
        }

        private async Task UpdateProductMapping(MstTfxProduct tfxProduct, string displayName, int newProductId, int pricingSourceId, bool isActualOpis = false)
        {
            await RemoveProductMapping(tfxProduct, pricingSourceId);
            var product = await Context.DataContext.MstProducts.FirstOrDefaultAsync(t => t.IsActive && t.Id == newProductId);
            if (product != null)
            {
                product.TfxProductId = tfxProduct.Id;
                product.DisplayName = displayName;
                product.ProductDisplayGroupId = tfxProduct.ProductDisplayGroupId;
                product.ProductTypeId = tfxProduct.ProductTypeId;
                product.ProductCode = tfxProduct.ProductCode;
                Context.DataContext.Entry(product).State = EntityState.Modified;
                await Context.CommitAsync();
            }
        }

        private async Task RemoveProductMapping(MstTfxProduct tfxProduct, int pricingSourceId, bool isActualOpis = false)
        {
            var product = tfxProduct.MstProducts.FirstOrDefault(t => t.PricingSourceId == pricingSourceId);
            if (product != null)
            {
                product.TfxProductId = null;
                product.DisplayName = null;
                Context.DataContext.Entry(product).State = EntityState.Modified;
                await Context.CommitAsync();
            }
        }

        public async Task<StateViewModel> SaveProductTypeMapping(ProductTypeMappingViewModel productTypeMappingViewModel)
        {
            var response = new StateViewModel();
            using (var tracer = new Tracer("SuperAdminDomain", "SaveProductTypeMapping"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var existingProductType = Context.DataContext.ProductTypeCompatibilityMappings.Where(t => t.ProductTypeId == productTypeMappingViewModel.ProductTypeId);
                        foreach (var item in existingProductType)
                        {
                            Context.DataContext.ProductTypeCompatibilityMappings.Remove(item);
                        }
                        await Context.CommitAsync();

                        if (productTypeMappingViewModel.MappedToProductTypeIds != null)
                        {
                            foreach (var item in productTypeMappingViewModel.MappedToProductTypeIds)
                            {
                                var productTypeMapped = new ProductTypeCompatibilityMapping();
                                productTypeMapped.ProductTypeId = productTypeMappingViewModel.ProductTypeId;
                                productTypeMapped.MappedToProductTypeId = item;
                                Context.DataContext.ProductTypeCompatibilityMappings.Add(productTypeMapped);
                            }
                        }

                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageUpdatedProductTypeMapping;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageCreateProductMapping;
                        LogManager.Logger.WriteException("SuperAdminDomain", "SaveProductTypeMapping", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StateViewModel> SaveBlendProductTypeMapping(ProductTypeMappingViewModel productTypeMappingViewModel)
        {
            var response = new StateViewModel();
            using (var tracer = new Tracer("SuperAdminDomain", "SaveBlendProductTypeMapping"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var existingProductType = Context.DataContext.MstBlendProductTypeMapping.Where(t => t.ProductTypeId == productTypeMappingViewModel.ProductTypeId);
                        foreach (var item in existingProductType)
                        {
                            Context.DataContext.MstBlendProductTypeMapping.Remove(item);
                        }
                        await Context.CommitAsync();

                        if (productTypeMappingViewModel.MappedToProductTypeIds != null)
                        {
                            foreach (var item in productTypeMappingViewModel.MappedToProductTypeIds)
                            {
                                var productTypeMapped = new MstBlendProductTypeMapping();
                                productTypeMapped.ProductTypeId = productTypeMappingViewModel.ProductTypeId;
                                productTypeMapped.MappedToProductTypeId = item;
                                Context.DataContext.MstBlendProductTypeMapping.Add(productTypeMapped);
                            }
                        }

                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageUpdatedProductTypeMapping;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageCreateProductMapping;
                        LogManager.Logger.WriteException("SuperAdminDomain", "SaveBlendProductTypeMapping", ex.Message, ex);
                    }
                }

                return response;
            }
        }


        public string ListToCSV<T>(List<T> list, int currencyType = 0)
        {
            if (list == null || list.Count == 0)
                return "";

            System.Text.StringBuilder sList = new System.Text.StringBuilder();

            Type type = typeof(T);
            var props = type.GetProperties();
            sList.Append(string.Join(",", props.Select(p => p.Name.DisplayName<T>(currencyType))));
            sList.Append(Environment.NewLine);

            foreach (var element in list)
            {
                sList.AppendLine(string.Join(",", props.Select(p => p.GetValue(element, null))));
            }

            return sList.ToString();
        }

        public async Task<List<JobBuyerDashboardViewModel>> GetMarinePortsForSuperAdmin(int countryId, UserContext userContext)
        {
            var response = new List<JobBuyerDashboardViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                response = await storedProcedureDomain.GetMarinePortsForSuperAdmin(countryId, userContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetMarinePortsForSuperAdmin", ex.Message, ex);
            }
            return response;

        }

        public async Task<StatusViewModel> SaveMarinePort(JobBuyerDashboardViewModel port, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                //validate duplicate port names 
                if (!string.IsNullOrWhiteSpace(port.JobName))
                {
                    var existingJob = Context.DataContext.Jobs.
                                Where(t => t.IsActive && t.IsMarine && t.LocationType == JobLocationTypes.Port && t.Name.Trim().ToLower() == port.JobName.Trim().ToLower())
                                .Select(t => t.Name)
                                 .FirstOrDefault();
                    if (existingJob != null)
                    {
                        if (port.JobID > 0)
                        {
                            if (port.JobName.Trim().ToLower() != existingJob.Trim().ToLower())
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessagePortNameExists, port.JobName);
                                return response;
                            }

                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessagePortNameExists, port.JobName);
                            return response;
                        }

                    }
                    var jobStepViewModel = await GetJobStepViewModel(port, userContext);
                    if (jobStepViewModel != null && jobStepViewModel.Job != null)
                    {
                        var domain = new JobDomain(this);
                        var jobSaveStatus = new StatusViewModel();
                        if (jobStepViewModel.Job.Id > 0)
                        {
                            jobSaveStatus = await UpdateMarinePort(jobStepViewModel);
                        }
                        else
                        {
                            jobSaveStatus = await domain.SaveJobStepsAsync(userContext, jobStepViewModel, userContext.IsSuperAdmin ? false : true);

                        }
                        if (jobSaveStatus != null)
                        {
                            response.StatusCode = jobSaveStatus.StatusCode;
                            response.StatusMessage = jobSaveStatus.StatusMessage;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageFailedToCreatePort;
                LogManager.Logger.WriteException("SuperAdminDomain", "SaveMarinePort", ex.Message, ex);
            }
            return response;
        }

        public async Task<JobStepsViewModel> GetJobStepViewModel(JobBuyerDashboardViewModel port, UserContext userContext)
        {
            var response = new JobStepsViewModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(port.JobName))
                {
                    response.Job.Name = port.JobName;
                    response.Job.Id = port.JobID;
                    response.Job.StartDate = DateTimeOffset.Now;
                    response.Job.EndDate = null;
                    response.Job.Address = port.Address;
                    response.Job.ZipCode = port.ZipCode;
                    response.Job.City = port.City;

                    response.Job.State.Id = port.StateID.HasValue ? port.StateID.Value : 0;
                    var stateName = await Context.DataContext.MstStates.Where(t => t.Id == port.StateID && t.IsActive).Select(t => t.Name).FirstOrDefaultAsync();
                    response.Job.State.Name = stateName;
                    var countryName = await Context.DataContext.MstCountries.Where(t => t.Id == port.CountryId).Select(t => t.Code).FirstOrDefaultAsync();
                    response.Job.Country.Id = port.CountryId;
                    response.Job.Country.Name = countryName;
                    response.Job.CountyName = port.CountyName;
                    // set default currency  as USD and uom as Gallons for CAR country 
                    if ((response.Job.Country.Id == (int)Country.USA) || (response.Job.Country.Id == (int)Country.CAR))
                    {
                        response.Job.Country.Currency = (Currency)Enum.Parse(typeof(Currency), "USD");
                        response.Job.Country.UoM = (UoM)Enum.Parse(typeof(UoM), "Gallons");
                    }
                    else
                    {
                        response.Job.Country.Currency = (Currency)Enum.Parse(typeof(Currency), "CAD");
                        response.Job.Country.UoM = (UoM)Enum.Parse(typeof(UoM), "Litres");
                    }

                    response.Job.IsGeocodeUsed = port.IsGeocodeUsed;
                    response.Job.Latitude = !string.IsNullOrEmpty(port.Latitude) ? Convert.ToDecimal(port.Latitude) : 0;
                    response.Job.Longitude = !string.IsNullOrEmpty(port.Longitude) ? Convert.ToDecimal(port.Longitude) : 0;



                    response.Job.InventoryDataCaptureType = (InventoryDataCaptureType)Enum.Parse(typeof(InventoryDataCaptureType), "NotSpecified");
                    response.Job.IsProFormaPoEnabled = false;
                    response.Job.IsRetailJob = false;
                    response.Job.IsAutoCreateDREnable = false;
                    response.JobBudget.IsTaxExempted = false;
                    response.Job.IsAssetTracked = false;
                    response.Job.StatusId = (int)JobStatus.Open;
                    response.Job.CreatedDate = DateTimeOffset.Now;
                    response.Job.ReopenDate = DateTimeOffset.Now;
                    response.Job.IsBackdatedJob = true;
                    response.Job.IsVarious = false;
                    response.Job.LocationType = JobLocationTypes.Port;
                    response.Job.IsMarine = true;
                    response.Job.LocationManagedType = LocationManagedType.NotSpecified;
                    response.UserId = userContext.Id;
                    response.CompanyId = ApplicationConstants.SuperAdminCompanyId; // for the time being
                    response.Job.TrailerType = Enum.GetValues(typeof(TrailerTypeStatus)).Cast<TrailerTypeStatus>().ToList();
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetJobStepViewModel", ex.Message, ex);
                throw; // I want to catch exception thrown from here in calling method to make the call as failed;
            }
            return response;

        }

        public async Task<StatusViewModel> UpdateMarinePort(JobStepsViewModel viewModel)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (viewModel.Job.Id > 0 && !string.IsNullOrWhiteSpace(viewModel.Job.Name))
                    {

                        var job = await Context.DataContext.Jobs.Where(t => t.Id == viewModel.Job.Id).FirstOrDefaultAsync();
                        if (job != null)
                        {
                            job.Name = viewModel.Job.Name;
                            job.CountryId = viewModel.Job.Country.Id;
                            job.IsGeocodeUsed = viewModel.Job.IsGeocodeUsed;
                            job.StateId = viewModel.Job.State.Id;
                            if (viewModel.Job.Country.Id != (int)Country.CAR)
                            {
                                job.Address = viewModel.Job.Address;
                                job.ZipCode = viewModel.Job.ZipCode;
                                job.City = viewModel.Job.City;
                                job.StateId = viewModel.Job.State.Id;
                                job.CountyName = viewModel.Job.CountyName;
                                job.Longitude = viewModel.Job.Longitude;
                                job.Latitude = viewModel.Job.Latitude;

                                if (viewModel.Job.Latitude == 0 || viewModel.Job.Longitude == 0)
                                {
                                    var stateCode = Context.DataContext.MstStates.First(t => t.Id == viewModel.Job.State.Id).Code;
                                    var countryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.Job.Country.Id).Code;
                                    var point = GoogleApiDomain.GetGeocode($"{viewModel.Job.Address} {viewModel.Job.City} {stateCode} {countryCode} {viewModel.Job.ZipCode}");
                                    if (point != null)
                                    {
                                        job.Latitude = Convert.ToDecimal(point.Latitude);
                                        job.Longitude = Convert.ToDecimal(point.Longitude);
                                        job.CountyName = point.CountyName != null ? point.CountyName : viewModel.Job.CountyName;
                                    }
                                    else
                                    {

                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = "Invalid Address or Latitude/Longitude";
                                        return response;
                                    }
                                }
                            }
                            else if (viewModel.Job.Country.Id == (int)Country.CAR)
                            {
                                job.Address = string.IsNullOrWhiteSpace(viewModel.Job.Address) ? (viewModel.Job.State.Name ?? Resource.lblCaribbean) : viewModel.Job.Address.Trim();
                                job.City = string.IsNullOrWhiteSpace(viewModel.Job.City) ? (viewModel.Job.State.Name ?? Resource.lblCaribbean) : viewModel.Job.City.Trim();
                                job.ZipCode = string.IsNullOrWhiteSpace(viewModel.Job.ZipCode) ? (viewModel.Job.State.Name ?? Resource.lblCaribbean) : viewModel.Job.ZipCode.Trim();
                                job.CountyName = string.IsNullOrWhiteSpace(viewModel.Job.CountyName) ? (viewModel.Job.State.Name ?? Resource.lblCaribbean) : viewModel.Job.CountyName.Trim();
                                if (viewModel.Job.Latitude == 0 || viewModel.Job.Longitude == 0)
                                {
                                    var stateCode = Context.DataContext.MstStates.First(t => t.Id == viewModel.Job.State.Id).Code;
                                    var countryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.Job.Country.Id).Code;
                                    var point = GoogleApiDomain.GetGeocode($"{job.Address} {job.City} {stateCode} {countryCode} {job.ZipCode}");
                                    if (point != null)
                                    {
                                        job.Latitude = Convert.ToDecimal(point.Latitude);
                                        job.Longitude = Convert.ToDecimal(point.Longitude);
                                    }
                                    else
                                    {

                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = "Invalid Address or Latitude/Longitude";
                                        return response;
                                    }
                                }
                                if ((string.IsNullOrWhiteSpace(viewModel.Job.Address) ||
                                   string.IsNullOrWhiteSpace(viewModel.Job.City) ||
                                    viewModel.Job.State.Id == 0 ||
                                    viewModel.Job.Country.Id == 0 ||
                                    string.IsNullOrWhiteSpace(viewModel.Job.ZipCode) ||
                            string.IsNullOrWhiteSpace(viewModel.Job.CountyName)) && (viewModel.Job.Latitude != 0 && viewModel.Job.Longitude != 0))
                                {
                                    var point = GoogleApiDomain.GetAddress(Convert.ToDouble(viewModel.Job.Latitude), Convert.ToDouble(viewModel.Job.Longitude));
                                    if (point != null && !string.IsNullOrWhiteSpace(point.CountryCode))
                                    {
                                        var country = Context.DataContext.MstCountries.Single(t => t.Code.ToLower().Contains(point.CountryCode.ToLower())).ToViewModel();

                                        job.Address = string.IsNullOrWhiteSpace(point.Address) ? (viewModel.Job.State.Name ?? Resource.lblCaribbean) : point.Address;
                                        job.ZipCode = string.IsNullOrWhiteSpace(point.ZipCode) ? (viewModel.Job.State.Name ?? Resource.lblCaribbean) : point.ZipCode;
                                        job.City = point.City;
                                        job.StateId = viewModel.Job.State.Id;
                                        job.CountyName = !string.IsNullOrWhiteSpace(viewModel.Job.CountyName) ? viewModel.Job.CountyName : (point.CountyName ?? viewModel.Job.State.Name);
                                        job.CountryId = country.Id;
                                        job.Latitude = viewModel.Job.Latitude;
                                        job.Longitude = viewModel.Job.Longitude;
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = Resource.errMessageJobCreateFailedInvalidAddress;
                                        return response;
                                    }
                                }
                            }
                            Context.DataContext.Entry(job).State = EntityState.Modified;
                            await Context.CommitAsync();
                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = "Successfully updated port " + job.Name;
                        }
                    }

                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = "Failed to update Port";
                    transaction.Rollback();
                    LogManager.Logger.WriteException("SuperAdminDomain", "UpdateMarinePort", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> DeletePort(int id, int userId)
        {
            var response = new StatusViewModel();
            try
            {
                var domain = new JobDomain(this);
                response = await domain.DeleteJobAsync(id, userId);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "DeletePort", ex.Message, ex);

            }
            return response;
        }

        public async Task<StatusViewModel> SaveMarineVessel(AssetViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                //validate duplicate vessel and imo number 
                if (!string.IsNullOrWhiteSpace(viewModel.Name))
                {
                    //var existingAsset = Context.DataContext.Assets.
                    //            Where(t => t.IsActive && t.IsMarine && t.Type == (int)AssetType.Vessle && t.Name.Trim().ToLower() == viewModel.Name.Trim().ToLower())
                    //            .Select(t => new { t.Name, t.Id } )
                    //             .FirstOrDefault();
                    var existingAsset = Context.DataContext.Assets.
                              Where(t => t.IsActive && t.IsMarine && t.Type == (int)AssetType.Vessle && t.Name.Trim().ToLower() == viewModel.Name.Trim().ToLower())
                              .Select(t => new { t.Name, t.Id })
                               .FirstOrDefault();
                    if (existingAsset != null)
                    {
                        if (viewModel.Id > 0)
                        {
                            if ((viewModel.Name.Trim().ToLower() == existingAsset.Name.Trim().ToLower()) && existingAsset.Id != viewModel.Id)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageVesselNameAlreadyExists, viewModel.Name);
                                return response;
                            }

                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageVesselNameAlreadyExists, viewModel.Name);
                            return response;
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(viewModel.IMONumber))
                    {
                        var existingIMO = Context.DataContext.Assets.
                                       Where(t => t.IsActive && t.IsMarine && t.Type == (int)AssetType.Vessle
                                       && (t.AssetAdditionalDetail != null && t.AssetAdditionalDetail.IMONumber.Trim().ToLower() == viewModel.IMONumber.Trim().ToLower()))
                                       .Select(t => new { t.Id, t.AssetAdditionalDetail.IMONumber }).FirstOrDefault();
                        if (existingIMO != null)
                        {
                            if (viewModel.Id > 0)
                            {
                                if ((viewModel.IMONumber.Trim().ToLower() == existingIMO.IMONumber.Trim().ToLower()) && viewModel.Id != existingIMO.Id)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageIMONumberAlreadyExists, viewModel.IMONumber);
                                    return response;
                                }

                            }
                            else
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageIMONumberAlreadyExists, viewModel.IMONumber);
                                return response;
                            }
                        }
                    }
                    Asset vessel = new Asset();
                    if (viewModel.Id == 0)
                    {
                        viewModel.CreatedDate = DateTimeOffset.Now;
                        viewModel.AssetAdditionalDetail.UpdatedBy = userContext.Id;
                        viewModel.AssetAdditionalDetail.UpdatedDate = DateTimeOffset.Now;
                        viewModel.Type = (int)AssetType.Vessle;
                        viewModel.AssetAdditionalDetail.Flag = string.IsNullOrWhiteSpace(viewModel.Flag) ? null : viewModel.Flag.Trim();
                        viewModel.AssetAdditionalDetail.IMONumber = string.IsNullOrWhiteSpace(viewModel.IMONumber) ? null : viewModel.IMONumber.Trim();
                        viewModel.IsActive = true;
                        viewModel.IsMarine = true;
                        viewModel.UpdatedDate = DateTimeOffset.Now;
                        viewModel.UpdatedBy = userContext.Id;
                        viewModel.UserId = userContext.Id;
                        vessel = viewModel.ToEntity();

                    }
                    else if (viewModel.Id > 0)
                    {
                        vessel = await Context.DataContext.Assets.Where(t => t.Id == viewModel.Id && t.IsActive).FirstOrDefaultAsync();
                        vessel.Name = viewModel.Name.Trim();
                        vessel.AssetAdditionalDetail.Flag = string.IsNullOrWhiteSpace(viewModel.Flag) ? null : viewModel.Flag.Trim();
                        vessel.AssetAdditionalDetail.IMONumber = string.IsNullOrWhiteSpace(viewModel.IMONumber) ? null : viewModel.IMONumber.Trim();
                        vessel.UpdatedBy = userContext.Id;
                        vessel.UpdatedDate = DateTimeOffset.Now;
                        vessel.AssetAdditionalDetail.UpdatedBy = userContext.Id;
                        vessel.AssetAdditionalDetail.UpdatedDate = DateTimeOffset.Now;
                    }
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (viewModel.Id == 0)
                            {
                                if (vessel != null)
                                {
                                    vessel.CompanyId = userContext.IsSuperAdmin ? ApplicationConstants.SuperAdminCompanyId : userContext.CompanyId;
                                    Context.DataContext.Assets.Add(vessel);
                                    await Context.CommitAsync();
                                    transaction.Commit();

                                    response.StatusCode = Status.Success;
                                    response.StatusMessage = Resource.successMessageVesselCreated;
                                }
                            }
                            else if (viewModel.Id > 0)
                            {
                                Context.DataContext.Entry(vessel).State = EntityState.Modified;
                                await Context.CommitAsync();
                                transaction.Commit();
                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.successMsgVessedUpdated;
                            }
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageVesselCreated;

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageVesselCreated;
                LogManager.Logger.WriteException("SuperAdminDomain", "SaveMarineVessel", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<AssetViewModel>> GetMarineVesselsForSuperAdmin(UserContext userContext)
        {
            var response = new List<AssetViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain();
                response = await spDomain.GetMarineVesselsForSuperAdmin(userContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetMarineVesselsForSuperAdmin", ex.Message, ex);
            }
            return response;

        }

        public async Task<StatusViewModel> DeleteVessel(int id)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var asset = await Context.DataContext.Assets.Where(t => t.Id == id && t.IsActive).FirstOrDefaultAsync();
                    if (asset != null)
                    {
                        asset.IsActive = false;
                        if (asset.AssetAdditionalDetail != null)
                        {
                            asset.AssetAdditionalDetail.IsActive = false;
                        }
                        Context.DataContext.Entry(asset).State = EntityState.Modified;
                        await Context.CommitAsync();
                        transaction.Commit();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMsgVesselDeleted;
                    }

                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMsgVesselDeleted;
                    LogManager.Logger.WriteException("SuperAdminDomain", "DeleteVessel", ex.Message, ex);
                }
                return response;
            }

        }

        public async Task<StatusViewModel> SaveTerminalProductMapping(TerminalProductMappingDetailsViewModel model)
        {
            var response = new StatusViewModel();
            try
            {
                if (model !=null && model.MappedProducts !=null && model.MappedProducts.Any() && model.TerminalId > 0)
                {
                    //get currently mapped product list for given terminalid
                    //find ids that are removed
                    //find ids that are newly added 
                    // find external product id for newly added products from existing mapping entries  else save externalproductid as zero 

                    List<MstProductMapping> newEntities = new List<MstProductMapping>();

                    var spDomain = new StoredProcedureDomain(this);
                    TerminalProductMappingInput spResponse = await spDomain.GetTerminalProductMappingDetailsInput(model.TerminalId, model.MappedProducts);

                    if (spResponse !=null)
                    {
                        var productIdMappingStatuses = spResponse.ProductIdXMappingStatus;
                        var externalProductIds = spResponse.ExternalProductIdXProductIds;
                        //add new mappings 
                        if (spResponse.NewProductIds !=null && spResponse.NewProductIds.Any())
                        {
                            foreach (var productId in spResponse.NewProductIds)
                            {
                                var newEntity = new MstProductMapping();
                                bool isMappingExist = productIdMappingStatuses != null && productIdMappingStatuses.Any() ? productIdMappingStatuses.Where(t => t.ProductId == productId).FirstOrDefault().IsMappingExists : false;
                                int externalProductId = 0;
                                if (externalProductIds != null && externalProductIds.Any())
                                {
                                    externalProductId =  externalProductIds.Where(t => t.ProductId == productId).Select(t => t.ExternalProductId).FirstOrDefault();
                                }
                                if (!isMappingExist)
                                {
                                    newEntity.ExternalTerminalId = model.TerminalId;
                                    newEntity.ProductId = productId;
                                    newEntity.ExternalProductId = externalProductId;
                                    newEntity.IsActive = true;
                                    newEntity.UpdatedDate = DateTimeOffset.Now;
                                    newEntity.UpdatedBy = 1;

                                    newEntities.Add(newEntity);
                                }
                            }

                        }

                        if (spResponse.RemovedProductIds !=null && spResponse.RemovedProductIds.Any())
                        {
                            var query = $"DELETE from MstProductMappings WHERE ProductId IN  ({string.Join<int>(",", spResponse.RemovedProductIds)}) AND ExternalTerminalId ={model.TerminalId}";
                            Context.DataContext.Database.ExecuteSqlCommand(query);
                        }
                        if (newEntities !=null && newEntities.Any())
                        {
                             Context.DataContext.MstProductMappings.AddRange(newEntities);
                        }
                       await Context.CommitAsync();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = "Successfully updated terminal-product assignment";
                    }
                }               
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageFailedToCreateMapping;
                LogManager.Logger.WriteException("SuperAdminDomain", "SaveTerminalProductMapping", ex.Message, ex);
            }
            return response;
        }

        
    }
}
