using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public interface IAuthenticationDomain
    {
        SiteFuelUow Context{get;}
      
        Task<UserContext> GetUserContextFromTokenAsync(string token);
        Task<Status> CreateUserAsync(RegisterViewModel viewModel, int? createdById);
        Task<StatusViewModel> UpdatePasswordAsync(int UserId, string Password);
        User RegisterCompany(RegisterViewModel viewModel, int? createdById);
        Task<Status> AddNewUserAsync(RegisterViewModel viewModel, int CreatedBy);
        Task<ConfirmationToken> GenerateEmailConfirmationTokenAsync(string email);
        Task<ConfirmationToken> GenerateEmailConfirmationTokenAsync(int id, string email);

        Task<ConfirmationToken> GenerateAuthTokenAsync(int id, string email, string fingerPrint);

        Task<ConfirmationToken> GenerateAuthMobileTokenAsync(int id, string email, string fingerPrint, int tokenExpiryTimeInMinutes);
        Task<Status> ConfirmEmailAsync(int userId, string token);

        Task<bool> IsEulaAccepted(int userId);

        Task<StatusViewModel> EulaAccepted(int userId);

        Task<StatusViewModel> CheckSupplierInvited(int userId);

        Task<UserViewModel> GetUserByIdAsync(int userId);

        Task<UserViewModel> GetUserByEmailAsync(string email);

        Task<Status> ResetPasswordAsync(ResetPasswordViewModel viewModel);

        Task<UserViewModel> PasswordMobileSignInAsync(ApiLoginViewModel viewModel);

       
        Task<RegisterAdditionalUserViewModel> GetAdditionalUserInviteByEmailAsync(string email);

        Task<User> GetUserForOktaAsync(string email);

        Task<List<User>> GetUsersForOktaAsync(int companyId, int count = 1);

        Task<CompanyIdentityService> GetCompanyIdentityServices(User user);
        Task UpdateUserActiveAsnc(string email, bool IsActive);

        StatusViewModel UpdateLastAccessedDate(int userId);
        int GetBrandedCompanyId(AppType appType);
        Task<UserContext> GetUserContextAsync(int userId, CompanyType companyType = CompanyType.Unknown);
        Task<MobileThemeViewModel> GetMobileTheme(string supplierCode, int appTypeId);
        Task<UserViewModel> PasswordSignInAsync(int id, bool shouldBypassPassword = false);
        Task<UserViewModel> PasswordSignInAsync(User user, string password = "", bool shouldBypassPassword = false, int loginSource = (int)LoginSource.Web);
        Task<UserViewModel> PasswordSignInAsync(LoginViewModel viewModel);

        Task<bool> ValidateFingerPrintAsync(string token);
        Task<string> ValidateImpersonationAsync(int userId, int impersonatedBy);
        Task<bool> UserLogout(ApiLogoutViewModel viewModel);
        Task<bool> ValidateAuthTokenAsync(string token);
        Task<bool> RemoveAuthTokenAsync(int userId);
        Task<bool> RemoveAuthTokenAsync(string token);
        Task<StatusViewModel> ChangePasswordAsync(ChangePasswordViewModel viewModel);
        Task<RegisterAdditionalUserViewModel> GetAdditionalUserInviteAsync(int id);
        Task<RegisterViewModel> GetInvitedCompanyUserAsync(int id);
        Task<StatusViewModel> OnboardCompanyUserAsync(RegisterAdditionalUserViewModel viewModel);
        UserXNotificationSetting GetNotificationSetting(int userId, int eventTypeId, bool isEmail = false, bool isSms = false, bool isInApp = false);

        Status AddImpersonationHistory(int ImpersonatedUserId, int ImpersonatedBy);
        Status UpdateImpersonationHistory(int ImpersonatedUserId, int ImpersonatedBy);
        int GetImpersonatedUserId(int userId);
        int GetImpersonatedByUserId(int userId);
        Task<UserViewModel> GetUserByTokenAsync(string token);
        List<int> GetDefaultEnabledNotifications(List<int> RoleIds, int companyType);
        Task<string> GetUserCountryAsync(UserContext userContext);       
        Task<List<CompanyUserDetails>> GetCompanyUsers(int companyId);
        Task<List<SendBirdCompanyUserViewModel>> GetUserDetails(List<int> userIds, string RegionID, string RegionName, string RegionDescription);
        Task<List<CompanyUserDetails>> GetUserDetails(List<int> userIds);
        Tuple<string, string, int, string, string> WebSiteConfigurationDetails(string URLName, int CompanyId, int UserId);
        Task<List<UserViewModel>> GetUserDetailsByIds(List<int> userId);
        string GetSupplierURLDetails(int companyID);
        Tuple<string, string, string, string> GetUserBrandingDetails(int userID);
        Task<RegisterAdditionalUserViewModel> GetExternalUserInviteAsync(int id);
        Task<StatusViewModel> OnboardExternalCompanyUserAsync(RegisterAdditionalUserViewModel viewModel);


    }
}
