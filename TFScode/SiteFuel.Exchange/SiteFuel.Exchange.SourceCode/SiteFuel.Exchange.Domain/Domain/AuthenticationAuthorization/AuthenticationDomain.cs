using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class AuthenticationDomain : BaseDomain
    {
        public AuthenticationDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public AuthenticationDomain(BaseDomain domain)
            : base(domain)
        {
        }

        private async Task<string> GenerateApplicationTokenAsync(int id, string toBeHashed, string salt = "", int tokenExpiryTimeInMinutes = 0)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GenerateApplicationTokenAsync"))
            {
                var response = string.Empty;

                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var newSalt = string.IsNullOrWhiteSpace(salt) ? CryptoHelperMethods.GenerateSalt() : salt;
                        response = CryptoHelperMethods.GenerateHash(toBeHashed, newSalt);
                        if (tokenExpiryTimeInMinutes <= 0)
                        {
                            tokenExpiryTimeInMinutes = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<int>(ApplicationConstants.KeyAppSettingTokenExpiryTime);
                        }

                        var expiredUserToken = Context.DataContext.UserTokens.Where(t => t.ExpiryTime < DateTimeOffset.Now);
                        if (expiredUserToken.Count() > 0)
                        {
                            Context.DataContext.UserTokens.RemoveRange(expiredUserToken);
                        }

                        Context.DataContext.UserTokens.Add(new UserToken
                        {
                            UserId = id,
                            Token = response,
                            ExpiryTime = DateTimeOffset.Now.AddMinutes(tokenExpiryTimeInMinutes)
                        });
                        await Context.CommitAsync();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("AuthenticationDomain", "GenerateApplicationTokenAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<UserContext> GetUserContextFromTokenAsync(string token)
        {
            try
            {
                var tokenDetails = Context.DataContext.UserTokens.SingleOrDefault(t => t.Token == token);
                if (tokenDetails != null)
                {
                    var response = await GetUserContextAsync(tokenDetails.User.Id, (CompanyType)tokenDetails.User.Company.CompanyTypeId);
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthenticationDomain", "ValidateAuthTokenAsync", ex.Message, ex);
            }
            return null;
        }

        public async Task<Status> CreateUserAsync(RegisterViewModel viewModel, int? createdById)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "CreateUserAsync"))
            {
                var response = Status.Failed;
                try
                {
                    User user = RegisterCompany(viewModel, createdById);
                    if (viewModel.AdditionalUserId > 0)
                    {
                        var userXInvite = Context.DataContext.UserXInvites.SingleOrDefault(t => t.Id == viewModel.AdditionalUserId);
                        if (userXInvite != null)
                        {
                            userXInvite.FirstName = user.FirstName;
                            userXInvite.LastName = user.LastName;
                            userXInvite.Email = user.Email;

                            Context.DataContext.Entry(userXInvite).State = EntityState.Modified;
                        }
                    }
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            Context.DataContext.Users.Add(user);
                            await Context.CommitAsync();

                            user.Company.CreatedBy = createdById ?? user.Id;
                            user.Company.UpdatedBy = createdById ?? user.Id;
                            await Context.CommitAsync();
                            transaction.Commit();
                            response = Status.Success;
                            viewModel.Id = user.Id;
                            viewModel.Company.Id = user.Company.Id;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogManager.Logger.WriteException("AuthenticationDomain", "CreateUserAsync", ex.Message, ex);
                        }

                        if (!string.IsNullOrEmpty(viewModel.SupplierURL))
                        {
                            try
                            {
                                var websiteDetails = Context.DataContext.OnboardingPreferences.Where(top => top.URLName.Trim().ToUpper() == viewModel.SupplierURL.Trim().ToUpper()).FirstOrDefault();
                                if (websiteDetails != null)
                                {
                                    SupplierInvitationDetails supplierInvitationDetails = new SupplierInvitationDetails();
                                    supplierInvitationDetails.UserId = user.Id;
                                    supplierInvitationDetails.CompanyId = websiteDetails.CompanyId;
                                    supplierInvitationDetails.SupplierId = websiteDetails.UserId;
                                    supplierInvitationDetails.CreatedDate = DateTime.Now;
                                    Context.DataContext.SupplierInvitationDetails.Add(supplierInvitationDetails);
                                    await Context.CommitAsync();
                                }

                            }
                            catch (Exception ex)
                            {
                                LogManager.Logger.WriteException("AuthenticationDomain", "CreateUserAsync-SupplierInvitationDetails", ex.Message, ex);
                            }
                        }
                        //IS INVITED REGISTRATION?
                        if (viewModel.InvitationId > 0)
                        {
                            var invitationDomain = new InvitationDomain(this);
                            await invitationDomain.InvitedCompanyRegistered(viewModel.InvitationId, viewModel.Company.Id);

                            var companyDetails = await invitationDomain.GetInvitedCompanyDetails(viewModel.InvitationId);
                            companyDetails.FleetInfo.ToFleetEntity().ForEach(t => user.Company.FleetInformations.Add(t));

                            var companyAddress = companyDetails.CompanyInfo.ToEntity();
                            var serviceOfferingEntity = companyDetails.ServiceOffering.ToEntity();
                            foreach (var item in serviceOfferingEntity)
                            {
                                companyAddress.CompanyXServingLocations.Add(item);
                            }
                            user.Title = companyDetails.UserInfo.Title;
                            user.Company.CompanyAddresses.Add(companyAddress);
                            await Context.CommitAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "CreateUserAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<StatusViewModel> UpdatePasswordAsync(int UserId, string Password)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "UpdatePasswordAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == UserId);
                    if (user != null)
                    {
                        var salt = CryptoHelperMethods.GenerateSalt();
                        user.PasswordHash = CryptoHelperMethods.GenerateHash(Password, salt);
                        user.SecurityStamp = salt;
                        user.FingerPrint = CryptoHelperMethods.GenerateHash(user.PasswordHash, CryptoHelperMethods.GenerateSalt());
                        user.IsEmailConfirmed = true;
                        //user.IsPhoneNumberConfirmed = true;
                        user.IsOnboardingComplete = true;
                        user.IsFirstLogin = true;

                        Context.DataContext.Entry(user).State = EntityState.Modified;
                        await Context.CommitAsync();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageUpdatePasswordFailed;
                    LogManager.Logger.WriteException("AuthenticationDomain", "UpdatePasswordAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public User RegisterCompany(RegisterViewModel viewModel, int? createdById)
        {
            var createdBy = createdById ?? (int)SystemUser.System;
            var salt = CryptoHelperMethods.GenerateSalt();
            User user = new User
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                UserName = viewModel.Email.Trim().ToLower(),
                Email = viewModel.Email.Trim().ToLower(),
                IsEmailConfirmed = false,
                PhoneNumber = viewModel.MobileNumber,
                IsPhoneNumberConfirmed = viewModel.IsPhoneNumberConfirmed,
                IsTwoFactorEnabled = false,
                AccessFailedCount = 0,
                IsLockoutEnabled = true,
                LockoutEndDateUtc = null,
                PasswordHash = CryptoHelperMethods.GenerateHash(viewModel.Password, salt),
                SecurityStamp = salt,
                FingerPrint = CryptoHelperMethods.GenerateHash(viewModel.Email, CryptoHelperMethods.GenerateSalt()),
                IsOnboardingComplete = false,
                OnboardedTypeId = (int)OnboardedType.Direct,
                IsActive = true,
                CreatedBy = createdBy,
                CreatedDate = DateTimeOffset.Now,
                UpdatedBy = createdBy,
                UpdatedDate = DateTimeOffset.Now,
                Title = viewModel.Title
            };

            var company = new Company
            {
                Name = viewModel.Company.Name.Trim(),
                CompanyTypeId = viewModel.Company.CompanyTypeId,
                CompanySizeId = 1,
                BusinessTenureId = 1,
                FuelQuantityId = 1,
                IsActive = true,
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now
            };

            user.Company = company;

            user.MstRoles.Add(Context.DataContext.MstRoles.Single(t => t.Id == (int)UserRoles.Admin));

            var sqlQuery = Context.DataContext.MstCompanyUserRoleXEventTypes
                            .Where(t => t.CompanyTypeId == company.CompanyTypeId
                                    && t.RoleId == (int)UserRoles.Admin
                                    && t.NotificationType != (int)NotificationType.Nothing);

            var userRoleIds = user.MstRoles.Select(t => t.Id).ToList();
            var defaultEnabled = GetDefaultEnabledNotifications(userRoleIds, company.CompanyTypeId);
            var notificationSettings = new List<UserXNotificationSetting>();
            foreach (var item in sqlQuery)
            {
                var setting = GetNotificationSetting(user.Id, item.EventTypeId, defaultEnabled.Contains(item.EventTypeId));
                notificationSettings.Add(setting);
            }
            user.UserXNotificationSettings = notificationSettings.Distinct().ToList();
            return user;
        }

        public async Task<Status> AddNewUserAsync(RegisterViewModel viewModel, int CreatedBy)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "AddNewUserAsync"))
            {
                var response = Status.Failed;

                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var salt = CryptoHelperMethods.GenerateSalt();
                        User user = new User
                        {
                            FirstName = viewModel.FirstName,
                            LastName = viewModel.LastName,
                            UserName = viewModel.Email.Trim().ToLower(),
                            Email = viewModel.Email.Trim().ToLower(),
                            IsEmailConfirmed = true,
                            PhoneNumber = viewModel.MobileNumber,
                            IsPhoneNumberConfirmed = viewModel.IsPhoneNumberConfirmed,
                            IsTwoFactorEnabled = false,
                            AccessFailedCount = 0,
                            IsLockoutEnabled = false,
                            LockoutEndDateUtc = null,
                            PasswordHash = CryptoHelperMethods.GenerateHash(viewModel.Password, salt),
                            SecurityStamp = salt,
                            FingerPrint = CryptoHelperMethods.GenerateHash(viewModel.Email, CryptoHelperMethods.GenerateSalt()),
                            IsOnboardingComplete = true,
                            OnboardedDate = DateTimeOffset.Now,
                            IsFirstLogin = true,
                            IsActive = true,
                            IsTaxExemptDisplayed = true,
                            CreatedBy = CreatedBy,
                            CreatedDate = DateTimeOffset.Now,
                            UpdatedBy = CreatedBy,
                            UpdatedDate = DateTimeOffset.Now
                        };

                        Context.DataContext.Users.Add(user);

                        switch (viewModel.UserRole)
                        {
                            case UserRoles.SuperAdmin:
                                user.IsSalesCalculatorAllowed = true;
                                user.MstRoles.Add(Context.DataContext.MstRoles.Single(t => t.Id == (int)UserRoles.SuperAdmin));
                                break;
                            case UserRoles.InternalSalesPerson:
                                user.IsSalesCalculatorAllowed = true;
                                user.MstRoles.Add(Context.DataContext.MstRoles.Single(t => t.Id == (int)UserRoles.InternalSalesPerson));
                                break;
                            case UserRoles.ExternalVendor:
                                user.MstRoles.Add(Context.DataContext.MstRoles.Single(t => t.Id == (int)UserRoles.ExternalVendor));
                                break;
                            case UserRoles.AccountSpecialist:
                                user.MstRoles.Add(Context.DataContext.MstRoles.Single(t => t.Id == (int)UserRoles.AccountSpecialist));
                                break;
                        }

                        await Context.CommitAsync();
                        transaction.Commit();

                        response = Status.Success;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("SettingsDomain", "AddNewUserAsync", ex.Message, ex);
                    }
                }
                return response;
            }
        }

        public async Task<ConfirmationToken> GenerateEmailConfirmationTokenAsync(string email)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GenerateEmailConfirmationTokenAsync"))
            {
                var response = new ConfirmationToken
                {
                    Id = 0,
                    Token = string.Empty
                };

                try
                {
                    var user = await Context.DataContext.Users.FirstOrDefaultAsync(t => t.Email.ToLower() == email.ToLower());
                    if (user != null)
                    {
                        var tokenExpiryTimeInMinutes = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<int>(ApplicationConstants.KeyAppSettingTokenExpiryTime);

                        response.Id = user.Id;
                        response.Token = await GenerateApplicationTokenAsync(user.Id, user.Email, string.Empty, tokenExpiryTimeInMinutes);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GenerateEmailConfirmationTokenAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<ConfirmationToken> GenerateEmailConfirmationTokenAsync(int id, string email)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GenerateEmailConfirmationTokenAsync"))
            {
                var response = new ConfirmationToken
                {
                    Id = 0,
                    Token = string.Empty
                };

                try
                {
                    var tokenExpiryTimeInMinutes = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<int>(ApplicationConstants.KeyAppSettingTokenExpiryTime);

                    response.Id = id;
                    response.Token = await GenerateApplicationTokenAsync(id, email, string.Empty, tokenExpiryTimeInMinutes);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GenerateEmailConfirmationTokenAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<ConfirmationToken> GenerateAuthTokenAsync(int id, string email, string fingerPrint)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GenerateAuthTokenAsync"))
            {
                var response = new ConfirmationToken
                {
                    Id = 0,
                    Token = string.Empty
                };

                try
                {
                    var tokenExpiryTimeInMinutes = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<int>(ApplicationConstants.KeyAppSettingMobileTokenExpiryTime);

                    response.Id = id;
                    response.Token = await GenerateApplicationTokenAsync(id, email, fingerPrint, tokenExpiryTimeInMinutes);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GenerateAuthTokenAsync", ex.Message, ex);
                }

                return response;
            }
        }


        public async Task<ConfirmationToken> GenerateAuthMobileTokenAsync(int id, string email, string fingerPrint, int tokenExpiryTimeInMinutes)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GenerateAuthTokenAsync"))
            {
                var response = new ConfirmationToken
                {
                    Id = 0,
                    Token = string.Empty
                };

                try
                {
                    response.Id = id;
                    response.Token = await GenerateApplicationTokenAsync(id, email, fingerPrint, tokenExpiryTimeInMinutes);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GenerateAuthTokenAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<Status> ConfirmEmailAsync(int userId, string token)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "ConfirmEmailAsync"))
            {
                Status response = Status.Failed;

                try
                {
                    var user = await Context.DataContext.Users.FirstOrDefaultAsync(t => t.Id == userId);
                    if (user != null)
                    {
                        LogManager.Logger.WriteDebug("AuthenticationDomain", "ConfirmEmailAsync", "userId = " + userId + " Code = " + token);
                        var applicationToken = user.UserTokens.FirstOrDefault(t => t.UserId == userId && t.Token == token);
                        if (applicationToken != null)
                        {
                            LogManager.Logger.WriteDebug("AuthenticationDomain", "ConfirmEmailAsync", "ExpiryDateTime = " + applicationToken.ExpiryTime + " CurrentDateTime = " + DateTimeOffset.Now);
                            if (applicationToken.ExpiryTime > DateTimeOffset.Now)
                            {
                                //Confirm email and activate account
                                user.IsEmailConfirmed = true;

                                user.IsActive = true;
                                user.UpdatedBy = userId;
                                user.UpdatedDate = DateTimeOffset.Now;

                                response = Status.Success;

                                //Remove token
                                Context.DataContext.UserTokens.Remove(applicationToken);
                                await Context.CommitAsync();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "ConfirmEmailAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<bool> IsEulaAccepted(int userId)
        {
            var response = false;
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                if (user != null)
                {
                    response = user.IsEULAAccepted;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationDomain", "IsEulaAccepted", "Exception Details : ", ex);
            }
            return response;
        }

        public async Task<StatusViewModel> EulaAccepted(int userId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId && !t.IsEULAAccepted);
                    if (user != null)
                    {
                        //user.IsActive = true;
                        //user.IsEmailConfirmed = true;
                        user.IsEULAAccepted = true;
                        if (user.OnboardedTypeId == (int)OnboardedType.ThirdPartyOrderOnboarded && user.IsEmailConfirmed && user.IsOnboardingComplete)
                        {
                            user.OnboardedDate = DateTimeOffset.Now;
                        }
                        //user.Company.IsActive = true;
                        Context.DataContext.Entry(user).State = EntityState.Modified;
                        await Context.CommitAsync();

                        transaction.Commit();
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageInvoicePaidSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("AuthenticationDomain", "EulaAccepted", ex.Message, ex);
                }
            }

            return response;
        }
        public async Task<StatusViewModel> CheckSupplierInvited(int userId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await Context.DataContext.SupplierInvitationDetails.Where(t => t.UserId == userId).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        var onboardingPreferencesDetails = await Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == user.CompanyId && t.UserId == userId && t.IsActive && t.IsBrandMyWebsite).FirstOrDefaultAsync();
                        if (onboardingPreferencesDetails != null)
                        {
                            response.StatusCode = Status.Success;
                            response.StatusMessage = onboardingPreferencesDetails.URLName;
                            response.EntityNumber = onboardingPreferencesDetails.ImageFilePath;
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
                    LogManager.Logger.WriteException("AuthenticationDomain", "CheckSupplierInvited", ex.Message, ex);
                }
            }

            return response;
        }
        public async Task<UserViewModel> GetUserByIdAsync(int userId)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetUserByIdAsync"))
            {
                UserViewModel response = new UserViewModel();
                try
                {
                    var user = await Context.DataContext.Users.FirstOrDefaultAsync(t => t.Id == userId);
                    if (user != null)
                    {
                        response = user.ToViewModel();
                        response.StatusCode = AuthStatus.Success;
                    }
                    else
                    {
                        response.StatusCode = AuthStatus.Failed;
                        response.StatusMessage = Resource.errMessageUserNotExist;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = AuthStatus.Failed;
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetUserByIdAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<UserViewModel> GetUserByEmailAsync(string email)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetUserByEmailAsync"))
            {
                UserViewModel response = new UserViewModel();
                try
                {
                    var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Email.ToLower() == email.ToLower());
                    if (user != null)
                    {
                        response = user.ToViewModel();
                    }
                    else
                    {
                        response.StatusMessage = Resource.errMessageUserNotExist;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageConfirmationEmailFailed;
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetUserByEmailAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<Status> ResetPasswordAsync(ResetPasswordViewModel viewModel)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "ResetPasswordAsync"))
            {
                Status response = Status.Failed;

                try
                {
                    var user = Context.DataContext.Users.SingleOrDefault(t => t.Email.ToLower() == viewModel.Email.ToLower());
                    if (user != null)
                    {
                        if (user.IsEmailConfirmed && user.IsActive)
                        {
                            var salt = CryptoHelperMethods.GenerateSalt();
                            user.AccessFailedCount = 0;
                            user.LockoutEndDateUtc = null;
                            user.PasswordHash = CryptoHelperMethods.GenerateHash(viewModel.Password, salt);
                            user.SecurityStamp = salt;
                            user.FingerPrint = CryptoHelperMethods.GenerateHash(user.PasswordHash, CryptoHelperMethods.GenerateSalt());

                            user.UpdatedBy = user.Id;
                            user.UpdatedDate = DateTimeOffset.Now;

                            //Add user and default role as admin
                            Context.DataContext.Entry(user).State = EntityState.Modified;
                            await Context.CommitAsync();

                            response = Status.Success;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "ResetPasswordAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<UserViewModel> PasswordMobileSignInAsync(ApiLoginViewModel viewModel)
        {
            var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Email.ToLower() == viewModel.Email.ToLower());
            var response = await PasswordSignInAsync(user, viewModel.Password, false, (int)LoginSource.Mobile);
            return response;
        }

        public async Task<UserViewModel> PasswordSignInAsync(LoginViewModel viewModel)
        {
            var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Email.ToLower() == viewModel.Email.ToLower());
            var response = await PasswordSignInAsync(user, viewModel.Password);
            return response;
        }

        #region method used for okta authentication 


        public async Task<RegisterAdditionalUserViewModel> GetAdditionalUserInviteByEmailAsync(string email)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetAdditionalUserInviteByEmailAsync"))
            {
                RegisterAdditionalUserViewModel response = new RegisterAdditionalUserViewModel();
                try
                {
                    var additionalUser = await Context.DataContext.CompanyXAdditionalUserInvites.FirstOrDefaultAsync(t => t.Email.ToUpper().Trim() == email.ToUpper().Trim());
                    if (additionalUser != null)
                    {
                        if (additionalUser.Company != null)
                        {
                            response.Company = additionalUser.Company.ToViewModel();
                        }
                        response.Id = additionalUser.Id;
                        response.CustomerCompanyId = additionalUser.CompanyId;                       
                        response.AdditionalUserId = additionalUser.Id;
                        response.FirstName = additionalUser.FirstName;
                        response.LastName = additionalUser.LastName;
                        response.Email = additionalUser.Email;
                        response.InvitedBy = additionalUser.InvitedBy;
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;

                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageInvitationRevoked;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetAdditionalUserInviteByEmailAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<User> GetUserForOktaAsync(string email)
        {

            return await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Email.ToLower() == email.ToLower());
           
        }

        public async Task<List<User>> GetUsersForOktaAsync(int companyId, int count = 1)
        {
            if (count==1)
            {
                return await Context.DataContext.Users.Where(t => t.CompanyId == companyId).OrderByDescending(t => t.Id).ToListAsync();
                
            }
            return await Context.DataContext.Users.Where(t => t.CompanyId == companyId).OrderByDescending(t => t.Id).Take(count).ToListAsync();

        }

        public async Task<CompanyIdentityService> GetCompanyIdentityServices(User user)
        {
            return await Context.DataContext.CompanyIdentityServices.SingleOrDefaultAsync(t1 => t1.CompanyId == user.CompanyId && t1.IsActive);
        }

        public async Task UpdateUserActiveAsnc(string email, bool IsActive)
        {  
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Email.ToLower() == email.ToLower());
                    if (user != null)
                    {
                        user.IsActive = IsActive;
                        Context.DataContext.Entry(user).State = EntityState.Modified;
                        Context.Commit();
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("AuthenticationDomain", "UserActiveOrDeActiveFromOktaAsnc", ex.Message, ex);
                }
            }
        }
        #endregion

        public StatusViewModel UpdateLastAccessedDate(int userId)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == userId && !t.IsImpersonated);
                    if (user != null)
                    {
                        user.LastAccessedDate = DateTime.Now;

                        Context.DataContext.Entry(user).State = EntityState.Modified;
                        Context.Commit();

                        transaction.Commit();
                    }

                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("AuthenticationDomain", "UpdateLastAccessedDate", ex.Message, ex);
                }
            }

            return response;
        }

        public int GetBrandedCompanyId(AppType appType)
        {
            int response = 0;
            try
            {
                if (appType == AppType.NFNBuyer)
                {
                    var companyID = Context.DataContext.MstAppSettings.Where(t => t.Key == Constants.NFNSupplierCompanyId)
                                        .Select(t => t.Value).SingleOrDefault();
                    if (!string.IsNullOrWhiteSpace(companyID))
                    {
                        int.TryParse(companyID, out response);
                    }
                }

                if (appType == AppType.HandabandBuyer)
                {
                    var companyID = Context.DataContext.MstAppSettings.Where(t => t.Key == Constants.HandaBandSupplierCompanyId)
                                        .Select(t => t.Value).SingleOrDefault();
                    if (!string.IsNullOrWhiteSpace(companyID))
                    {
                        int.TryParse(companyID, out response);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthenticationDomain", "GetBrandedCompanyId", ex.Message, ex);
            }
            return response;
        }

        public async Task<UserContext> GetUserContextAsync(int userId, CompanyType companyType = CompanyType.Unknown)
        {
            var currentUser = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
            if (currentUser != null)
            {
                var roleIds = currentUser.MstRoles.Where(t => t.IsActive).Select(t => t.Id).ToList();
                var companyTypeId = companyType == CompanyType.Unknown ?
                    (CompanyType)currentUser.Company.CompanyTypeId : companyType;

                return new UserContext(currentUser.Id, $"{currentUser.FirstName} {currentUser.LastName}", currentUser.UserName, currentUser.Email, currentUser.Company.Id, currentUser.Company.Name,
                   companyTypeId, companyTypeId, roleIds,
                    currentUser.IsFirstLogin, currentUser.IsSalesCalculatorAllowed, currentUser.IsImpersonated, 0);
            }
            else
            {
                return new UserContext();
            }
        }

        public async Task<MobileThemeViewModel> GetMobileTheme(string supplierCode, int appTypeId)
        {
            var response = new MobileThemeViewModel();
            try
            {
                response.CompanyLogo = string.Empty;
                bool setDefaultTheme = false;
                if (!string.IsNullOrEmpty(supplierCode))
                {
                    var company = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.SupplierCode == supplierCode);
                    if (company != null)
                    {
                        response.CompanyName = company.Name;
                        response.SupplierCode = supplierCode;
                        if (company.Image != null)
                        {
                            if (!string.IsNullOrEmpty(company.Image.FilePath))
                            {
                                ImageViewModel imageViewModel = new ImageViewModel();
                                imageViewModel.FilePath = company.Image.FilePath;
                                response.CompanyLogo = imageViewModel.GetAzureFilePath(BlobContainerType.CompanyProfile);
                            }
                        }

                        response.CompanyId = company.Id;
                        var theme = company.CompanyXMobileAppThemes.SingleOrDefault(t => t.AppTypeId == appTypeId);
                        if (theme != null)
                        {
                            var themeDetails = theme.MstMobileAppTheme.MstMobileAppThemeDetails.ToList();
                            foreach (var item in themeDetails)
                            {
                                switch (item.Key)
                                {
                                    case ApplicationConstants.KeyMobileAppThemePrimaryButtonBackgroundColor:
                                        response.PrimaryButtonBackgroundColor = item.Value;
                                        break;
                                    case ApplicationConstants.KeyMobileAppThemePrimaryButtonForeColor:
                                        response.PrimaryButtonForeColor = item.Value;
                                        break;
                                    case ApplicationConstants.KeyMobileAppThemeHeaderBackgroundColor:
                                        response.HeaderBackgroundColor = item.Value;
                                        break;
                                    case ApplicationConstants.KeyMobileAppThemeHeaderForeColor:
                                        response.HeaderForeColor = item.Value;
                                        break;
                                    case ApplicationConstants.KeyMobileAppThemeFooterBackgroundColor:
                                        response.FooterBackgroundColor = item.Value;
                                        break;
                                    case ApplicationConstants.KeyMobileAppThemeFooterForeColor:
                                        response.FooterForeColor = item.Value;
                                        break;
                                    case ApplicationConstants.KeyMobileAppThemeSecondaryButtonBackgroundColor:
                                        response.SecondaryButtonBackgroundColor = item.Value;
                                        break;
                                    case ApplicationConstants.KeyMobileAppThemeSecondaryButtonForeColor:
                                        response.SecondaryButtonForeColor = item.Value;
                                        break;
                                }
                            }
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageSuccess;
                        }
                        else
                        {
                            setDefaultTheme = true;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageSupplierCodeInvalid;
                    }
                }
                else
                {
                    setDefaultTheme = true;
                }

                if (setDefaultTheme)
                {
                    response.PrimaryButtonBackgroundColor = "#36417a";
                    response.PrimaryButtonForeColor = "#FFFFFF";
                    response.HeaderBackgroundColor = "#36417a";
                    response.HeaderForeColor = "#FFFFFF";
                    response.FooterBackgroundColor = "#36417a";
                    response.FooterForeColor = "#FFFFFF";
                    response.SecondaryButtonBackgroundColor = "#ffab63";
                    response.SecondaryButtonForeColor = "#FFFFFF";
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthenticationDomain", "GetMobileTheme", ex.Message, ex);
            }
            return response;
        }

        public async Task<UserViewModel> PasswordSignInAsync(int id, bool shouldBypassPassword = false)
        {
            var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == id);
            var response = await PasswordSignInAsync(user, string.Empty, shouldBypassPassword);
            return response;
        }

        public async Task<UserViewModel> PasswordSignInAsync(User user, string password = "", bool shouldBypassPassword = false, int loginSource = (int)LoginSource.Web)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "PasswordSignInAsync"))
            {
                UserViewModel response = new UserViewModel();
                try
                {
                    if (user == null)
                    {
                        response.StatusCode = AuthStatus.InvalidUser;
                        response.StatusMessage = Resource.errMessageUserNotExist;
                        return response;
                    }

                    if (loginSource==(int)LoginSource.Web)
                    {
                        var supportingOkta = await Context.DataContext.CompanyIdentityServices.SingleOrDefaultAsync(t => t.CompanyId == user.CompanyId && t.IsAvailable);

                        if (supportingOkta!=null)
                        {
                            if (supportingOkta.IdentityProvider.IsSSO)
                            {
                                response.StatusCode = AuthStatus.InvalidUser;
                                response.StatusMessage = Resource.errMessageLoginWithSSO;
                                return response;
                            }
                            
                        }
                    }
                    
                    //EULA Not Accepted
                    if (!shouldBypassPassword && !user.IsEULAAccepted && user.OnboardedTypeId == (int)OnboardedType.ThirdPartyOrderOnboarded)
                    {
                        if (loginSource == (int)LoginSource.Web)
                        {
                            if (!user.IsEULAAccepted)
                            {
                                response = user.ToViewModel(response);
                                response.StatusCode = AuthStatus.AcceptEULA;
                                response.StatusMessage = Resource.errMessageAcceptEULA;
                                return response;
                            }
                            else
                            {
                                response.StatusCode = AuthStatus.InvalidUser;
                                response.StatusMessage = Resource.errMessageUserNotExist;
                                return response;
                            }
                        }
                        else if (loginSource == (int)LoginSource.Mobile)
                        {
                            response.StatusCode = AuthStatus.EmailNotConfirmed;
                            response.StatusMessage = Resource.errMessageTPOEmailNotConfirmed;
                            return response;
                        }
                    }

                    if (!user.IsEmailConfirmed && user.OnboardedTypeId != (int)OnboardedType.ThirdPartyOrderOnboarded)
                    {
                        response.StatusCode = AuthStatus.EmailNotConfirmed;
                        response.StatusMessage = Resource.errMessageEmailNotConfirmed;
                        return response;
                    }

                    if (!shouldBypassPassword && !user.IsActive && user.OnboardedTypeId != (int)OnboardedType.ThirdPartyOrderOnboarded)
                    {
                        response.StatusCode = AuthStatus.InActiveUser;
                        response.StatusMessage = Resource.errMessageDeactivatedUser;
                        return response;
                    }

                    if (!user.IsOnboardingComplete && loginSource == (int)LoginSource.Mobile && user.OnboardedTypeId != (int)OnboardedType.ThirdPartyOrderOnboarded)
                    {
                        response.StatusCode = AuthStatus.NotOnboarded;
                        response.StatusMessage = Resource.errMessageNotOnboarded;
                        return response;
                    }

                    if ((!shouldBypassPassword && user.IsImpersonated) && loginSource != (int)LoginSource.Mobile)
                    {
                        response.StatusCode = AuthStatus.UnderImpersonation;
                        response.StatusMessage = Resource.errMessageGettingImpersonated;
                        return response;
                    }

                    if (!shouldBypassPassword && user.IsLockoutEnabled && user.LockoutEndDateUtc != null && user.LockoutEndDateUtc > DateTimeOffset.Now)
                    {
                        response.StatusCode = AuthStatus.AccountLocked;
                        response.StatusMessage = Resource.errMessageAccountLocked;
                        return response;
                    }

                    var passwordHash = CryptoHelperMethods.GenerateHash(password, user.SecurityStamp);
                    if (passwordHash == user.PasswordHash || shouldBypassPassword)
                    {
                        user.AccessFailedCount = 0;
                        user.LockoutEndDateUtc = null;

                        Context.DataContext.Entry(user).State = EntityState.Modified;
                        await Context.CommitAsync();

                        response = user.ToViewModel(response);

                        if (user.OnboardedTypeId == (int)OnboardedType.ThirdPartyOrderOnboarded)
                        {
                            response.IsBuyerTPOCreated = true;
                            if (!shouldBypassPassword && loginSource == (int)LoginSource.Web && user.IsEULAAccepted && !user.IsOnboardingComplete)
                            {
                                response.StatusCode = AuthStatus.TPOBuyerNotOnboarded;
                                response.StatusMessage = Resource.errMessageOnboardTPOBuyer;
                                return response;
                            }
                        }

                        response.StatusCode = AuthStatus.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                    else
                    {
                        response.StatusCode = AuthStatus.LoginFailed;
                        response.StatusMessage = Resource.errMessageLoginFailed;
                        if (user.IsLockoutEnabled)
                        {
                            int maxAllowedAttempts = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<int>(ApplicationConstants.KeyAppSettingMaxFailedAccessAttemptsBeforeLockout);
                            user.AccessFailedCount += 1;
                            if (maxAllowedAttempts <= user.AccessFailedCount)
                            {
                                int lockoutTimeInMinutes = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<int>(ApplicationConstants.KeyAppSettingAccountLockoutTimeSpan);
                                user.LockoutEndDateUtc = DateTimeOffset.Now.AddMinutes(lockoutTimeInMinutes);
                                user.FingerPrint = CryptoHelperMethods.GenerateHash(user.PasswordHash, CryptoHelperMethods.GenerateSalt());
                                response.StatusMessage = Resource.errMessageAccountLocked;
                            }
                            Context.DataContext.Entry(user).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageLoginFailed;
                    LogManager.Logger.WriteException("AuthenticationDomain", "PasswordSignInAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<bool> ValidateFingerPrintAsync(string token)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "ValidateFingerPrintAsync"))
            {
                bool response = false;
                try
                {
                    response = await Context.DataContext.Users.AnyAsync(t => t.FingerPrint == token);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "ValidateFingerPrintAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<string> ValidateImpersonationAsync(int userId, int impersonatedBy)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "ValidateImpersonationAsync"))
            {
                string response = string.Empty;
                try
                {
                    var impersonation = await Context.DataContext.ImpersonationHistories.OrderByDescending(t => t.Id).FirstOrDefaultAsync(t => t.UserId == userId && t.ImpersonatedBy == impersonatedBy);
                    if (impersonation != null && impersonation.ImpersonatedEndTime.HasValue)
                    {
                        if (impersonation.TerminatedBy.HasValue)
                        {
                            var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == impersonation.UserId);
                            var terminatedBy = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == impersonation.TerminatedBy.Value);
                            response = string.Format(Resource.errMessageImpersonationRemoved, user.FirstName + " " + user.LastName, terminatedBy.UserName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "ValidateImpersonationAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<bool> UserLogout(ApiLogoutViewModel viewModel)
        {
            bool response = true;
            try
            {
                if (!string.IsNullOrEmpty(viewModel.FCMAppId) && viewModel.UserId != 0)
                {
                    var applocation = await Context.DataContext.AppLocations.FirstOrDefaultAsync(t => t.FCMAppId == viewModel.FCMAppId && t.UserId == viewModel.UserId);
                    if (applocation != null)
                    {
                        applocation.IsUserLogout = true;
                        Context.DataContext.Entry(applocation).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthenticationDomain", "UserLogout", ex.Message, ex);
                response = false;
            }
            return response;
        }

        public async Task<bool> ValidateAuthTokenAsync(string token)
        {
            bool response = false;
            try
            {
                var userToken = Context.DataContext.UserTokens.SingleOrDefault(t => t.Token == token);
                if (userToken != null)
                {
                    if (userToken.ExpiryTime < DateTimeOffset.Now)
                    {
                        Context.DataContext.UserTokens.Remove(userToken);
                        await Context.CommitAsync();
                    }
                    else
                    {
                        response = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthenticationDomain", "ValidateAuthTokenAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<bool> RemoveAuthTokenAsync(int userId)
        {
            bool response = false;
            try
            {
                var userToken = await Context.DataContext.UserTokens.Where(t => t.UserId == userId)
                                        .OrderByDescending(t => t.ExpiryTime).FirstOrDefaultAsync();
                if (userToken != null)
                {
                    Context.DataContext.UserTokens.Remove(userToken);
                    await Context.CommitAsync();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthenticationDomain", "RemoveAuthTokenAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> RemoveAuthTokenAsync(string token)
        {
            bool response = false;
            try
            {
                var userToken = await Context.DataContext.UserTokens.Where(t => t.Token == token)
                                        .OrderByDescending(t => t.ExpiryTime).FirstOrDefaultAsync();
                if (userToken != null)
                {
                    Context.DataContext.UserTokens.Remove(userToken);
                    await Context.CommitAsync();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthenticationDomain", "RemoveAuthTokenAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ChangePasswordAsync(ChangePasswordViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == viewModel.UserId);
                if (user != null && user.IsEmailConfirmed && user.IsActive)
                {
                    if (user.PasswordHash == CryptoHelperMethods.GenerateHash(viewModel.OldPassword, user.SecurityStamp))
                    {
                        var salt = CryptoHelperMethods.GenerateSalt();
                        user.AccessFailedCount = 0;
                        user.LockoutEndDateUtc = null;
                        user.PasswordHash = CryptoHelperMethods.GenerateHash(viewModel.NewPassword, salt);
                        user.SecurityStamp = salt;
                        user.FingerPrint = CryptoHelperMethods.GenerateHash(user.PasswordHash, CryptoHelperMethods.GenerateSalt());

                        user.UpdatedBy = user.Id;
                        user.UpdatedDate = DateTimeOffset.Now;
                        user.IsFirstLogin = false;
                        //Add user and default role as admin
                        Context.DataContext.Entry(user).State = EntityState.Modified;
                        await Context.CommitAsync();

                        //Send response
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageUpdatePasswordSuccess;
                    }
                    else
                    {
                        response.StatusMessage = Resource.valWrongOldPassword;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdatePasswordFailed;
                LogManager.Logger.WriteException("AuthenticationDomain", "ChangePasswordAsync", ex.Message, ex);
            }

            return response;
        }

      
        public async Task<RegisterAdditionalUserViewModel> GetAdditionalUserInviteAsync(int id)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetAdditionalUserInviteAsync"))
            {
                RegisterAdditionalUserViewModel response = new RegisterAdditionalUserViewModel();
                try
                {
                    var additionalUser = await Context.DataContext.CompanyXAdditionalUserInvites.FirstOrDefaultAsync(t => t.Id == id);
                    if (additionalUser != null)
                    {
                        if (additionalUser.Company != null)
                        {
                            response.Company = additionalUser.Company.ToViewModel();
                            if (additionalUser.MstRoles != null)
                            {  
                                response.RoleIds = additionalUser.MstRoles.Select(t1 => t1.Id).ToList();
                            }
                            response.AdditionalUserId = id;
                            response.FirstName = additionalUser.FirstName;
                            response.LastName = additionalUser.LastName;
                            response.Email = additionalUser.Email;
                            response.InvitedBy = additionalUser.InvitedBy;
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageSuccess;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageInvitationRevoked;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetAdditionalUserInviteAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<RegisterViewModel> GetInvitedCompanyUserAsync(int id)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetInvitedCompanyUserAsync"))
            {
                RegisterViewModel response = new RegisterViewModel();
                try
                {
                    var additionalUser = await Context.DataContext.UserXInvites.FirstOrDefaultAsync(t => t.Id == id);
                    if (additionalUser != null)
                    {
                        response.AdditionalUserId = id;
                        response.Email = additionalUser.Email;

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageInvitationRevoked;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetInvitedCompanyUserAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<StatusViewModel> OnboardCompanyUserAsync(RegisterAdditionalUserViewModel viewModel)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "OnboardCompanyUserAsync"))
            {
                var response = new StatusViewModel();

                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var additionalUser = await Context.DataContext.CompanyXAdditionalUserInvites.FirstOrDefaultAsync(t => t.Id == viewModel.AdditionalUserId);

                        var userToUpdate = await Context.DataContext.Users.FirstOrDefaultAsync(t => t.Email == viewModel.Email);
                        if (userToUpdate != null && userToUpdate.CompanyId != viewModel.Company.Id)
                        {
                            response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageUserAlreadyExistsWithAnotherCompanyInSFX, new object[] { viewModel.Email, userToUpdate.Company.Name });
                            return response;
                        }
                        if (additionalUser != null)
                        {
                            var salt = CryptoHelperMethods.GenerateSalt();
                            //Update user and delete additionalUSer
                            if (userToUpdate == null)
                            {
                                userToUpdate = new User
                                {
                                    CreatedBy = additionalUser.InvitedBy,
                                    CreatedDate = DateTimeOffset.Now,
                                };
                            }

                            userToUpdate.FirstName = viewModel.FirstName;
                            userToUpdate.LastName = viewModel.LastName;
                            userToUpdate.UserName = viewModel.Email.Trim().ToLower();
                            userToUpdate.Email = viewModel.Email.Trim().ToLower();
                            userToUpdate.IsEmailConfirmed = true;
                            userToUpdate.PhoneNumber = viewModel.MobileNumber;
                            userToUpdate.IsPhoneNumberConfirmed = viewModel.IsPhoneNumberConfirmed;
                            userToUpdate.IsTwoFactorEnabled = false;
                            userToUpdate.AccessFailedCount = 0;
                            userToUpdate.IsLockoutEnabled = true;
                            userToUpdate.LockoutEndDateUtc = null;
                            userToUpdate.PasswordHash = CryptoHelperMethods.GenerateHash(viewModel.Password, salt);
                            userToUpdate.SecurityStamp = salt;
                            userToUpdate.FingerPrint = CryptoHelperMethods.GenerateHash(viewModel.Email, CryptoHelperMethods.GenerateSalt());
                            userToUpdate.IsOnboardingComplete = true;
                            userToUpdate.OnboardedDate = DateTimeOffset.Now;
                            userToUpdate.IsActive = true;
                            userToUpdate.IsTaxExemptDisplayed = true;
                            userToUpdate.UpdatedBy = (int)SystemUser.System;
                            userToUpdate.UpdatedDate = DateTimeOffset.Now;

                            if (userToUpdate.Id > 0)
                            {
                                Context.DataContext.Entry(userToUpdate).State = EntityState.Modified;
                                await Context.CommitAsync();
                            }
                            else
                            {
                                userToUpdate.Company = additionalUser.Company;
                                userToUpdate.MstRoles = additionalUser.MstRoles.ToList();
                                Context.DataContext.Users.Add(userToUpdate);
                                await Context.CommitAsync();
                            }

                            //Update user notification settings
                            var companyTypeId = additionalUser.Company.CompanyTypeId;
                            var userRoleIds = userToUpdate.MstRoles.Select(t => t.Id).ToList();
                            var sqlQuery = Context.DataContext.MstCompanyUserRoleXEventTypes
                                            .Where(t => t.NotificationType != (int)NotificationType.Nothing
                                                    && t.CompanyTypeId == companyTypeId
                                                    && userRoleIds.Contains(t.RoleId));

                            var defaultEnabled = GetDefaultEnabledNotifications(userRoleIds, companyTypeId);
                            var notificationSettings = new List<UserXNotificationSetting>();
                            foreach (var item in sqlQuery)
                            {
                                var setting = GetNotificationSetting(userToUpdate.Id, item.EventTypeId, defaultEnabled.Contains(item.EventTypeId));
                                notificationSettings.Add(setting);
                            }
                            userToUpdate.UserXNotificationSettings = notificationSettings.GroupBy(t => t.EventTypeId).Select(grp => grp.First()).ToList();

                            int jobId = 0;
                            bool isOnsiteRole = additionalUser.MstRoles.Any(t => t.Id == (int)UserRoles.OnsitePerson);
                            if (additionalUser.Jobs.Count > 0)
                            {
                                foreach (var job in additionalUser.Jobs)
                                {
                                    job.Users.Add(userToUpdate);
                                    jobId = job.Id;

                                    if (isOnsiteRole)
                                    {
                                        job.Users1.Add(userToUpdate);
                                    }
                                    Context.DataContext.Entry(job).State = EntityState.Modified;
                                }
                            }
                            Context.DataContext.CompanyXAdditionalUserInvites.Remove(additionalUser);

                            await Context.CommitAsync();
                            transaction.Commit();

                            NotificationDomain notificationDomain = new NotificationDomain(this);
                            if (!string.IsNullOrEmpty(viewModel.SupplierURL))
                            {
                                var applicationTemplateId = ContextFactory.Current.GetDomain<HelperDomain>().GetApplicationTemplateId(viewModel.SupplierURL);
                                var message = new AddUserMessageViewModel { SupplierURL = viewModel.SupplierURL };
                                var jsonMessage = new JavaScriptSerializer().Serialize(message);
                                await notificationDomain.AddNotificationEventAsync(EventType.AdditionalUserOnboarded, userToUpdate.Id, userToUpdate.Id, null, jsonMessage, applicationTemplateId);
                            }
                            else
                            {
                                await notificationDomain.AddNotificationEventAsync(EventType.AdditionalUserOnboarded, userToUpdate.Id, userToUpdate.Id, null, null, (int)ApplicationTemplate.TrueFill);
                            }

                            if (jobId > 0)
                            {
                                await notificationDomain.AddNotificationEventAsync(EventType.JobAssignment, jobId, userToUpdate.Id);
                            }
                        }

                        response.StatusCode = Status.Success;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("AuthenticationDomain", "OnboardCompanyUserAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public UserXNotificationSetting GetNotificationSetting(int userId, int eventTypeId, bool isEmail = false, bool isSms = false, bool isInApp = false)
        {
            var userXNotificationSetting = new UserXNotificationSetting
            {
                UserId = userId,
                EventTypeId = eventTypeId,
                IsEmail = isEmail,
                IsSMS = isSms,
                IsInApp = isInApp
            };

            return userXNotificationSetting;
        }

        public Status AddImpersonationHistory(int ImpersonatedUserId, int ImpersonatedBy)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "AddImpersonationHistory"))
            {
                var response = Status.Failed;

                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        ImpersonationHistory impersonation = new ImpersonationHistory
                        {
                            UserId = ImpersonatedUserId,
                            ImpersonatedBy = ImpersonatedBy,
                            ImpersonatedStartTime = DateTimeOffset.Now
                        };
                        Context.DataContext.ImpersonationHistories.Add(impersonation);

                        var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == ImpersonatedUserId);
                        if (user != null)
                        {
                            user.IsImpersonated = true;
                        }
                        Context.DataContext.Entry(user).State = EntityState.Modified;

                        Context.Commit();
                        transaction.Commit();

                        response = Status.Success;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("SettingsDomain", "AddImpersonationHistory", ex.Message, ex);
                    }
                }
                return response;
            }
        }

        public Status UpdateImpersonationHistory(int ImpersonatedUserId, int ImpersonatedBy)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "UpdateImpersonationHistory"))
            {
                var response = Status.Failed;
                try
                {
                    var impersonationDetails = Context.DataContext.ImpersonationHistories.SingleOrDefault(t => t.UserId == ImpersonatedUserId && t.ImpersonatedBy == ImpersonatedBy && t.ImpersonatedEndTime == null);
                    if (impersonationDetails != null)
                    {
                        impersonationDetails.ImpersonatedEndTime = DateTimeOffset.Now;
                        Context.DataContext.Entry(impersonationDetails).State = EntityState.Modified;

                        var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == ImpersonatedUserId);
                        if (user != null)
                        {
                            user.IsImpersonated = false;
                        }
                        Context.DataContext.Entry(user).State = EntityState.Modified;

                        Context.Commit();

                        response = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "UpdateImpersonationHistory", ex.Message, ex);
                }

                return response;
            }
        }

        public int GetImpersonatedUserId(int userId)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetImpersonatedUserId"))
            {
                int response = 0;
                try
                {
                    var impersonationHistory = Context.DataContext.ImpersonationHistories.OrderByDescending(t1 => t1.Id).FirstOrDefault(t => t.ImpersonatedBy == userId);
                    if (impersonationHistory != null && impersonationHistory.ImpersonatedEndTime == null)
                    {
                        return impersonationHistory.UserId;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetImpersonatedUserId", ex.Message, ex);
                }

                return response;
            }
        }

        public int GetImpersonatedByUserId(int userId)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetImpersonatedByUserId"))
            {
                int response = 0;
                try
                {
                    var impersonationHistory = Context.DataContext.ImpersonationHistories.OrderByDescending(t1 => t1.Id).First(t => t.UserId == userId);
                    if (impersonationHistory != null && impersonationHistory.ImpersonatedEndTime != null)
                    {
                        return impersonationHistory.ImpersonatedBy;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetImpersonatedByUserId", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<UserViewModel> GetUserByTokenAsync(string token)
        {
            var response = new UserViewModel();
            try
            {
                var userInfo = await Context.DataContext.UserTokens
                            .Where(t => t.Token.Equals(token) && t.ExpiryTime > DateTimeOffset.Now
                                    && t.User.IsActive && t.User.IsEmailConfirmed && t.User.IsOnboardingComplete)
                            .Select(t => new
                            {
                                t.User,
                                Roles = t.User.MstRoles.Select(t1 => new { t1.Id, t1.Name }),
                                Company = new
                                {
                                    t.User.Company.Id,
                                    t.User.Company.CompanyTypeId,
                                    t.User.Company.Name,
                                    t.User.Company.CompanyLogoId,
                                    t.User.Company.Image
                                }
                            }).FirstOrDefaultAsync();
                if (userInfo != null)
                {
                    response = userInfo.User.ToUserViewModel();
                    response.Roles = userInfo.Roles.Select(t => new RoleViewModel { Id = t.Id, Name = t.Name }).ToList();
                    if (userInfo.Company != null && userInfo.Company.Id > 0)
                    {
                        response.CompanyId = userInfo.Company.Id;
                        response.CompanyName = userInfo.Company.Name;
                        response.CompanyTypeId = userInfo.Company.CompanyTypeId;
                        response.CompanyLogoId = userInfo.Company.CompanyLogoId;
                        if (userInfo.Company.Image == null || string.IsNullOrEmpty(userInfo.Company.Image.FilePath))
                        {
                            response.CompanyLogo = string.Empty;
                        }
                        else
                        {
                            response.CompanyLogo = userInfo.Company.Image.ToViewModel().GetAzureFilePath(BlobContainerType.CompanyProfile);
                        }
                    }
                    response.StatusCode = AuthStatus.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthenticationDomain", "GetUserByToken", ex.Message, ex);
            }
            return response;
        }

        public List<int> GetDefaultEnabledNotifications(List<int> RoleIds, int companyType)
        {
            var response = new List<int>();
            if (RoleIds.Any(t => t == (int)UserRoles.Admin || t == (int)UserRoles.Buyer || t == (int)UserRoles.Supplier || t == (int)UserRoles.Carrier))
            {
                try
                {
                    switch (companyType)
                    {
                        case (int)CompanyType.Buyer:
                            response.AddRange(GetDefaultEnabledNotificationSetting(ApplicationConstants.KeyAppSettingBuyerDefaultEnabledNotifications));
                            break;
                        case (int)CompanyType.Supplier:
                            response.AddRange(GetDefaultEnabledNotificationSetting(ApplicationConstants.KeyAppSettingSupplierDefaultEnabledNotifications));
                            break;
                        case (int)CompanyType.Carrier:
                            response.AddRange(GetDefaultEnabledNotificationSetting(ApplicationConstants.KeyAppSettingCarrierDefaultEnabledNotifications));
                            break;
                        case (int)CompanyType.BuyerAndSupplier:
                            response.AddRange(GetDefaultEnabledNotificationSetting(ApplicationConstants.KeyAppSettingBuyerDefaultEnabledNotifications));
                            response.AddRange(GetDefaultEnabledNotificationSetting(ApplicationConstants.KeyAppSettingSupplierDefaultEnabledNotifications));
                            break;
                        case (int)CompanyType.SupplierAndCarrier:
                            response.AddRange(GetDefaultEnabledNotificationSetting(ApplicationConstants.KeyAppSettingSupplierDefaultEnabledNotifications));
                            response.AddRange(GetDefaultEnabledNotificationSetting(ApplicationConstants.KeyAppSettingCarrierDefaultEnabledNotifications));
                            break;
                        case (int)CompanyType.BuyerSupplierAndCarrier:
                            response.AddRange(GetDefaultEnabledNotificationSetting(ApplicationConstants.KeyAppSettingBuyerDefaultEnabledNotifications));
                            response.AddRange(GetDefaultEnabledNotificationSetting(ApplicationConstants.KeyAppSettingSupplierDefaultEnabledNotifications));
                            response.AddRange(GetDefaultEnabledNotificationSetting(ApplicationConstants.KeyAppSettingCarrierDefaultEnabledNotifications));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetDefaultEnabledNotifications", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<string> GetUserCountryAsync(UserContext userContext)
        {
            var response = string.Empty;
            try
            {
                response = await Context.DataContext.CompanyAddresses
                            .Where(t => t.CompanyId == userContext.CompanyId && t.IsDefault)
                            .Select(t => t.MstCountry.Code).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthenticationDomain", "GetUserCountryAsync", ex.Message, ex);
            }
            if (string.IsNullOrWhiteSpace(response))
            {
                response = Country.USA.ToString();
            }
            return response;
        }

        private List<int> GetDefaultEnabledNotificationSetting(string appSettingKeyName)
        {
            var response = new List<int>();
            try
            {
                var appDomain = new ApplicationDomain(this);
                var appSettingValue = appDomain.GetKeySettingValue(appSettingKeyName, string.Empty);
                if (!string.IsNullOrWhiteSpace(appSettingValue))
                {
                    response = appSettingValue.Split(',')
                                .Where(t => !string.IsNullOrWhiteSpace(t))
                                .Select(t => Convert.ToInt32(t)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthenticationDomain", "GetDefaultEnabledNotificationSetting: Key=" + appSettingKeyName, ex.Message, ex);
            }
            return response;
        }
        public async Task<List<CompanyUserDetails>> GetCompanyUsers(int companyId)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetCompanyUsers"))
            {
                List<CompanyUserDetails> response = new List<CompanyUserDetails>();
                try
                {
                    var user = await Context.DataContext.Users.Where(t => t.CompanyId == companyId && t.IsActive && t.IsEmailConfirmed && t.IsOnboardingComplete && t.MstRoles.Any(topx => topx.Name == UserRoles.Carrier.ToString() || topx.Name == UserRoles.Dispatcher.ToString() || topx.Name == UserRoles.Supplier.ToString() || topx.Name == UserRoles.Admin.ToString())).ToListAsync();
                    if (user != null)
                    {
                        foreach (var useritem in user)
                        {
                            response.Add(new CompanyUserDetails { EmailAddress = useritem.Email, FirstName = useritem.FirstName, LastName = useritem.LastName, Role = string.Join(",", useritem.MstRoles.Select(t => t.Name).ToList()), UserId = useritem.Id, UserName = useritem.UserName, IsPhoneNumberConfirmed = useritem.IsPhoneNumberConfirmed, PhoneNumber = useritem.PhoneNumber });
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetCompanyUsers", ex.Message, ex);
                }

                return response;
            }
        }
        public async Task<List<SendBirdCompanyUserViewModel>> GetUserDetails(List<int> userIds, string RegionID, string RegionName, string RegionDescription)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetCompanyUsers"))
            {
                List<SendBirdCompanyUserViewModel> response = new List<SendBirdCompanyUserViewModel>();
                try
                {
                    var user = await Context.DataContext.Users.Where(t => userIds.Contains(t.Id) && t.IsActive && t.IsEmailConfirmed && t.IsOnboardingComplete).ToListAsync();
                    if (user != null)
                    {
                        foreach (var useritem in user)
                        {
                            response.Add(new SendBirdCompanyUserViewModel { EmailAddress = useritem.Email, FirstName = useritem.FirstName, LastName = useritem.LastName, Role = string.Join(",", useritem.MstRoles.Select(t => t.Name).ToList()), UserId = useritem.Id, UserName = useritem.UserName, IsPhoneNumberConfirmed = useritem.IsPhoneNumberConfirmed, PhoneNumber = useritem.PhoneNumber, RegionDescription = string.IsNullOrEmpty(RegionDescription) ? string.Empty : RegionDescription, RegionID = RegionID, RegionName = RegionName, SendBirdRegionID = RegionID.Substring(0, 10).ToUpper() });
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetCompanyUsers", ex.Message, ex);
                }

                return response;
            }
        }
        public async Task<List<CompanyUserDetails>> GetUserDetails(List<int> userIds)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetCompanyUsers"))
            {
                List<CompanyUserDetails> response = new List<CompanyUserDetails>();
                try
                {
                    var user = await Context.DataContext.Users.Where(t => userIds.Contains(t.Id) && t.IsActive && t.IsEmailConfirmed && t.IsOnboardingComplete).ToListAsync();
                    if (user != null)
                    {
                        foreach (var useritem in user)
                        {
                            response.Add(new CompanyUserDetails { EmailAddress = useritem.Email, FirstName = useritem.FirstName, LastName = useritem.LastName, Role = string.Join(",", useritem.MstRoles.Select(t => t.Name).ToList()), UserId = useritem.Id, UserName = useritem.UserName, IsPhoneNumberConfirmed = useritem.IsPhoneNumberConfirmed, PhoneNumber = useritem.PhoneNumber });
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetCompanyUsers", ex.Message, ex);
                }

                return response;
            }
        }
        public Tuple<string, string, int, string, string> WebSiteConfigurationDetails(string URLName, int CompanyId, int UserId)
        {
            try
            {
                if (!string.IsNullOrEmpty(URLName))
                {
                    var websiteDetails = Context.DataContext.OnboardingPreferences.Where(top => top.IsActive == true && top.URLName.Trim().ToUpper() == URLName.Trim().ToUpper() && top.IsBrandMyWebsite).OrderByDescending(top => top.Id)
                                                                                  .Select(t => new { t.ImageFilePath, t.BackgroundImageFilePath, t.CompanyId, t.ButtonColor, t.FaviconImageFilePath })
                                                                                  .FirstOrDefault();
                    if (websiteDetails != null)
                    {
                        return Tuple.Create(websiteDetails.ImageFilePath, websiteDetails.BackgroundImageFilePath, websiteDetails.CompanyId, websiteDetails.ButtonColor, websiteDetails.FaviconImageFilePath);
                    }
                }
                else
                {
                    if (CompanyId > 0 && UserId > 0)
                    {
                        var supplierInvitationDetails = Context.DataContext.SupplierInvitationDetails.Where(top => top.UserId == UserId).Select(t => new { t.CompanyId }).FirstOrDefault();
                        if (supplierInvitationDetails != null)
                        {
                            var supplierWebsiteDetails = Context.DataContext.OnboardingPreferences.Where(top => top.IsActive == true && top.CompanyId == supplierInvitationDetails.CompanyId && top.IsBrandMyWebsite).OrderByDescending(top => top.Id)
                                                                                  .Select(t => new { t.ImageFilePath, t.BackgroundImageFilePath, t.CompanyId, t.ButtonColor, t.FaviconImageFilePath })
                                                                                   .FirstOrDefault();
                            if (supplierWebsiteDetails != null)
                            {
                                return Tuple.Create(supplierWebsiteDetails.ImageFilePath, supplierWebsiteDetails.BackgroundImageFilePath, supplierWebsiteDetails.CompanyId, supplierWebsiteDetails.ButtonColor, supplierWebsiteDetails.FaviconImageFilePath);
                            }
                        }
                        else
                        {
                            var supplierWebsiteDetails = Context.DataContext.OnboardingPreferences.Where(top => top.IsActive == true && top.CompanyId == CompanyId && top.IsBrandMyWebsite).OrderByDescending(top => top.Id)
                                                                                                  .Select(t => new { t.ImageFilePath, t.BackgroundImageFilePath, t.CompanyId, t.ButtonColor, t.FaviconImageFilePath })
                                                                                                  .FirstOrDefault();
                            if (supplierWebsiteDetails != null)
                            {
                                return Tuple.Create(supplierWebsiteDetails.ImageFilePath, supplierWebsiteDetails.BackgroundImageFilePath, supplierWebsiteDetails.CompanyId, supplierWebsiteDetails.ButtonColor, supplierWebsiteDetails.FaviconImageFilePath);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuthenticationDomain", "WebSiteConfigurationDetails", ex.Message, ex);

            }

            return Tuple.Create(string.Empty, string.Empty, 0, string.Empty, string.Empty);
        }

        public async Task<List<UserViewModel>> GetUserDetailsByIds(List<int> userId)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetUserDetailsByIds"))
            {
                List<UserViewModel> response = new List<UserViewModel>();
                try
                {
                    if (userId != null)
                    {
                        userId = userId.Distinct().ToList();
                        response = await Context.DataContext.Users.Where(t => userId.Contains(t.Id))
                            .Select(t => new UserViewModel()
                            {
                                Id = t.Id,
                                FirstName = t.FirstName,
                                LastName = t.LastName,
                                Email = t.Email,
                                PhoneNumber = t.PhoneNumber,
                                IsPhoneNumberConfirmed = t.IsPhoneNumberConfirmed
                            })
                            .ToListAsync();
                        response.ForEach(t => { t.FullName = t.FirstName + " " + t.LastName; t.StatusCode = AuthStatus.Success; });
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetUserDetailsByIds", ex.Message, ex);
                }

                return response;
            }
        }
        public string GetSupplierURLDetails(int companyID)
        {
            var companyDetails = Context.DataContext.OnboardingPreferences.Where(top => top.CompanyId == companyID && top.IsBrandMyWebsite && top.IsActive).OrderByDescending(top => top.Id).FirstOrDefault();
            if (companyDetails != null)
                return companyDetails.URLName;
            else
                return string.Empty;
        }
        public Tuple<string, string, string, string> GetUserBrandingDetails(int userID)
        {
            var userDetails = Context.DataContext.Users.Where(top => top.Id == userID && top.IsActive && top.IsEmailConfirmed && top.IsOnboardingComplete).FirstOrDefault();
            if (userDetails != null)
            {
                if (userDetails.Company != null)
                {
                    var companyDetails = Context.DataContext.OnboardingPreferences.Where(top => top.CompanyId == userDetails.CompanyId.Value && top.IsBrandMyWebsite && top.IsActive).OrderByDescending(top => top.Id).FirstOrDefault();
                    if (companyDetails != null)
                    {
                        return Tuple.Create(companyDetails.URLName, companyDetails.ImageFilePath, companyDetails.BackgroundImageFilePath, companyDetails.ButtonColor);
                    }
                }
            }
            return Tuple.Create(string.Empty, string.Empty, string.Empty, string.Empty);
        }


        public async Task<RegisterAdditionalUserViewModel> GetExternalUserInviteAsync(int id)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "GetExternalUserInviteAsync"))
            {
                RegisterAdditionalUserViewModel response = new RegisterAdditionalUserViewModel();
                try
                {
                    var additionalUser = await Context.DataContext.InvitedUsers.FirstOrDefaultAsync(t => t.Id == id);
                    if (additionalUser != null)
                    {
                        if (additionalUser.Company != null)
                        {
                            response.Company = additionalUser.Company.ToViewModel();
                            response.AdditionalUserId = id;
                            response.FirstName = additionalUser.FirstName;
                            response.LastName = additionalUser.LastName;
                            response.Email = additionalUser.Email;
                            response.InvitedBy = additionalUser.InvitedBy;
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageSuccess;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageInvitationRevoked;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AuthenticationDomain", "GetExternalUserInviteAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<StatusViewModel> OnboardExternalCompanyUserAsync(RegisterAdditionalUserViewModel viewModel)
        {
            using (var tracer = new Tracer("AuthenticationDomain", "OnboardExternalCompanyUserAsync"))
            {
                var response = new StatusViewModel();

                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var additionalUser = await Context.DataContext.InvitedUsers.FirstOrDefaultAsync(t => t.Id == viewModel.AdditionalUserId);

                        var userToUpdate = await Context.DataContext.Users.FirstOrDefaultAsync(t => t.Email == viewModel.Email);
                        if (userToUpdate != null && userToUpdate.CompanyId != viewModel.Company.Id)
                        {
                            response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageUserAlreadyExistsWithAnotherCompanyInSFX, new object[] { viewModel.Email, userToUpdate.Company.Name });
                            return response;
                        }
                        if (additionalUser != null)
                        {
                            var salt = CryptoHelperMethods.GenerateSalt();
                            //Update user and delete additionalUSer
                            if (userToUpdate == null)
                            {
                                userToUpdate = new User
                                {
                                    CreatedBy = additionalUser.InvitedBy,
                                    CreatedDate = DateTimeOffset.Now,
                                };
                            }

                            userToUpdate.FirstName = viewModel.FirstName;
                            userToUpdate.LastName = viewModel.LastName;
                            userToUpdate.UserName = viewModel.Email.Trim().ToLower();
                            userToUpdate.Email = viewModel.Email.Trim().ToLower();
                            userToUpdate.IsEmailConfirmed = true;
                            userToUpdate.PhoneNumber = viewModel.MobileNumber;
                            userToUpdate.IsPhoneNumberConfirmed = viewModel.IsPhoneNumberConfirmed;
                            userToUpdate.IsTwoFactorEnabled = false;
                            userToUpdate.AccessFailedCount = 0;
                            userToUpdate.IsLockoutEnabled = true;
                            userToUpdate.LockoutEndDateUtc = null;
                            userToUpdate.PasswordHash = CryptoHelperMethods.GenerateHash(viewModel.Password, salt);
                            userToUpdate.SecurityStamp = salt;
                            userToUpdate.FingerPrint = CryptoHelperMethods.GenerateHash(viewModel.Email, CryptoHelperMethods.GenerateSalt());
                            userToUpdate.IsOnboardingComplete = true;
                            userToUpdate.OnboardedDate = DateTimeOffset.Now;
                            userToUpdate.IsActive = true;
                            userToUpdate.IsTaxExemptDisplayed = true;
                            userToUpdate.UpdatedBy = (int)SystemUser.System;
                            userToUpdate.UpdatedDate = DateTimeOffset.Now;
                            userToUpdate.Title = additionalUser.Title;

                            if (userToUpdate.Id > 0)
                            {
                                Context.DataContext.Entry(userToUpdate).State = EntityState.Modified;
                                await Context.CommitAsync();
                            }
                            else
                            {
                                userToUpdate.Company = additionalUser.Company;
                                userToUpdate.MstRoles = additionalUser.MstRoles.ToList();
                                Context.DataContext.Users.Add(userToUpdate);
                                await Context.CommitAsync();
                            }

                            //Update user notification settings
                            var companyTypeId = additionalUser.Company.CompanyTypeId;
                            var userRoleIds = userToUpdate.MstRoles.Select(t => t.Id).ToList();
                            var sqlQuery = Context.DataContext.MstCompanyUserRoleXEventTypes
                                            .Where(t => t.NotificationType != (int)NotificationType.Nothing
                                                    && t.CompanyTypeId == companyTypeId
                                                    && userRoleIds.Contains(t.RoleId));

                            var defaultEnabled = GetDefaultEnabledNotifications(userRoleIds, companyTypeId);
                            var notificationSettings = new List<UserXNotificationSetting>();
                            foreach (var item in sqlQuery)
                            {
                                var setting = GetNotificationSetting(userToUpdate.Id, item.EventTypeId, defaultEnabled.Contains(item.EventTypeId));
                                notificationSettings.Add(setting);
                            }
                            userToUpdate.UserXNotificationSettings = notificationSettings.GroupBy(t => t.EventTypeId).Select(grp => grp.First()).ToList();

                            int jobId = 0;
                            //if (additionalUser.Jobs.Count > 0)
                            //{
                            //    foreach (var job in additionalUser.Jobs)
                            //    {
                            //        job.Users.Add(userToUpdate);
                            //        jobId = job.Id;

                            //        if (isOnsiteRole)
                            //        {
                            //            job.Users1.Add(userToUpdate);
                            //        }
                            //        Context.DataContext.Entry(job).State = EntityState.Modified;
                            //    }
                            //}
                            Context.DataContext.InvitedUsers.Remove(additionalUser);

                            await Context.CommitAsync();
                            transaction.Commit();

                            NotificationDomain notificationDomain = new NotificationDomain(this);
                            if (!string.IsNullOrEmpty(viewModel.SupplierURL))
                            {
                                var applicationTemplateId = ContextFactory.Current.GetDomain<HelperDomain>().GetApplicationTemplateId(viewModel.SupplierURL);
                                var message = new AddUserMessageViewModel { SupplierURL = viewModel.SupplierURL };
                                var jsonMessage = new JavaScriptSerializer().Serialize(message);
                                await notificationDomain.AddNotificationEventAsync(EventType.AdditionalUserOnboarded, userToUpdate.Id, userToUpdate.Id, null, jsonMessage, applicationTemplateId);
                            }
                            else
                            {
                                await notificationDomain.AddNotificationEventAsync(EventType.AdditionalUserOnboarded, userToUpdate.Id, userToUpdate.Id, null, null, (int)ApplicationTemplate.TrueFill);
                            }

                            if (jobId > 0)
                            {
                                await notificationDomain.AddNotificationEventAsync(EventType.JobAssignment, jobId, userToUpdate.Id);
                            }
                        }

                        response.StatusCode = Status.Success;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("AuthenticationDomain", "OnboardExternalCompanyUserAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }


    }
}
