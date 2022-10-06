using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Api.Mobile.Common;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class AccountController : ApiBaseController
    {
        private const int tokenExpriryInMinutes = 100 * 365 * 24 * 60; // 100 Years token expiry for mobile logins
        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<AuthResponseViewModel> Login(ApiLoginViewModel viewModel)
        {
            AuthResponseViewModel response = new AuthResponseViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var authDomain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                    var userViewModel = await authDomain.PasswordMobileSignInAsync(viewModel);
                    response.StatusCode = userViewModel.StatusCode;
                    response.StatusMessage = userViewModel.StatusMessage;

                    if (userViewModel.StatusCode == AuthStatus.Success)
                    {
                        if (viewModel.AppType == AppType.DriverApp &&
                            (userViewModel.CompanyTypeId == (int)CompanyType.Supplier ||
                            userViewModel.CompanyTypeId == (int)CompanyType.BuyerAndSupplier ||
                            userViewModel.CompanyTypeId == (int)CompanyType.BuyerSupplierAndCarrier ||
                            userViewModel.CompanyTypeId == (int)CompanyType.SupplierAndCarrier ||
                            userViewModel.CompanyTypeId == (int)CompanyType.Carrier) &&
                            userViewModel.Roles.Any(t => (
                                            t.Id == (int)UserRoles.Admin ||
                                            t.Id == (int)UserRoles.Driver ||
                                            t.Id == (int)UserRoles.Supplier)))
                        {
                            //build the response
                            response.CompanyId = userViewModel.CompanyId;
                            response.CompanyDefaultCountry = userViewModel.CompanyDefaultCountry;
                            authDomain.UpdateLastAccessedDate(userViewModel.Id);
                        }
                        else if ((viewModel.AppType == AppType.BuyerApp || viewModel.AppType == AppType.NFNBuyer || viewModel.AppType == AppType.HandabandBuyer) &&
                                 (userViewModel.CompanyTypeId == (int)CompanyType.Buyer ||
                                 userViewModel.CompanyTypeId == (int)CompanyType.BuyerAndSupplier ||
                                 userViewModel.CompanyTypeId == (int)CompanyType.BuyerSupplierAndCarrier) &&
                                 userViewModel.Roles.Any(t => (
                                                t.Id == (int)UserRoles.Admin ||
                                                t.Id == (int)UserRoles.Buyer)))
                        {
                            if (viewModel.AppType == AppType.NFNBuyer || viewModel.AppType == AppType.HandabandBuyer)
                                response.BrandedCompanyId = authDomain.GetBrandedCompanyId(viewModel.AppType);

                            //build the response
                            response.CompanyId = userViewModel.CompanyId;
                            response.IsBuyerTPOCreated = userViewModel.IsBuyerTPOCreated;
                            authDomain.UpdateLastAccessedDate(userViewModel.Id);
                        }
                        else if (viewModel.AppType == AppType.ExternalApiCaller &&
                                 userViewModel.Roles.Count == 1 &&
                                 userViewModel.Roles.Any(t => t.Id == (int)UserRoles.ExternalVendor))
                        {

                            response.CompanyId = 0;
                            authDomain.UpdateLastAccessedDate(userViewModel.Id);
                        }
                        else if ((userViewModel.CompanyTypeId == (int)CompanyType.Carrier || userViewModel.CompanyTypeId == (int)CompanyType.Supplier
                            || userViewModel.CompanyTypeId == (int)CompanyType.SupplierAndCarrier || userViewModel.CompanyTypeId == (int)CompanyType.BuyerSupplierAndCarrier) &&
                                 viewModel.AppType == AppType.ExternalApiCaller &&
                                 userViewModel.Roles.Any(t => t.Id == (int)UserRoles.Admin))
                        {
                            authDomain.UpdateLastAccessedDate(userViewModel.Id);
                        }
                        else
                        {
                            response.StatusCode = AuthStatus.UnAuthorized;
                            response.Token = null;
                            response.StatusMessage = Resource.errMessageUnauthorizedUser;
                        }

                        if (response.StatusCode == AuthStatus.Success)
                        {
                            var token = await authDomain.GenerateAuthMobileTokenAsync(
                                                       userViewModel.Id,
                                                       userViewModel.Email,
                                                       string.Empty, tokenExpriryInMinutes);
                            response.UserId = userViewModel.Id;
                            response.UserName = userViewModel.FullName;
                            response.Token = token.Token;
                        }
                    }
                }
                else
                {
                    var geterror = new CommonMethods().GetErrorMessage(ModelState);
                    response.StatusCode = AuthStatus.Failed;
                    response.StatusMessage = geterror.StatusMessage;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AccountController", "Login", ex.Message + "\nEmail:" + viewModel.Email, ex);
            }
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        [ValidateToken]
        public async Task<AuthResponseViewModel> Logout(ApiLogoutViewModel viewModel)
        {
            AuthResponseViewModel response = new AuthResponseViewModel();
            try
            {
                var token = Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    AuthenticationDomain authenticationDomain = new AuthenticationDomain();
                    var result = await authenticationDomain.RemoveAuthTokenAsync(token);
                    if (result)
                    {
                        var logoutResponse = await authenticationDomain.UserLogout(viewModel);
                        if (logoutResponse)
                        {
                            response.StatusCode = AuthStatus.Success;
                            response.StatusMessage = Resource.errMessageSuccess;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AccountController", "Logout", ex.Message, ex);
            }
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<AuthResponseViewModel> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            AuthResponseViewModel response = new AuthResponseViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();

                    UserViewModel userViewModel = await domain.GetUserByEmailAsync(viewModel.Email);
                    response.StatusCode = userViewModel.StatusCode;
                    response.StatusMessage = userViewModel.StatusMessage;

                    if (userViewModel.StatusCode == AuthStatus.Success)
                    {
                        if (userViewModel.IsEmailConfirmed)
                        {
                            if (userViewModel.IsActive)
                            {
                                // Send an email with this link
                                var confirmationToken = await domain.GenerateEmailConfirmationTokenAsync(userViewModel.Id, userViewModel.Email);
                                if (confirmationToken.Id > 0)
                                {
                                    var serverUrl = ContextFactory.Current.GetDomain<HelperDomain>().GetServerUrl();
                                    var callbackUrl = $"Account/ResetPassword?userId={userViewModel.Id}&email={userViewModel.Email}&code={confirmationToken.Token}";
                                    var notification = ContextFactory.Current.GetDomain<NotificationDomain>().GetNotificationContent(EventSubType.ForgotPassword, serverUrl, callbackUrl);
                                    var emailTemplate = ContextFactory.Current.GetDomain<HelperDomain>().GetApplicationEventNotificationTemplate();
                                    var emailModel = new ApplicationEventNotificationViewModel
                                    {
                                        To = new List<string> { viewModel.Email },
                                        Subject = notification.Subject,
                                        CompanyLogo = notification.CompanyLogo,
                                        CompanyText = notification.CompanyText,
                                        BodyLogo = notification.BodyLogo,
                                        BodyText = string.Format(notification.BodyText, userViewModel.FullName),
                                        BodyButtonText = notification.BodyButtonText,
                                        BodyButtonUrl = notification.BodyButtonUrl
                                    };
                                    Email.GetClient().Send(emailTemplate, emailModel);

                                    response.StatusCode = AuthStatus.Success;
                                    response.StatusMessage = Resource.errMessageCheckYourEmail;
                                }
                                else
                                {
                                    response.StatusCode = AuthStatus.TokenFailure;
                                    response.StatusMessage = Resource.errMessageLoginFailed;
                                }
                            }
                            else
                            {
                                response.StatusCode = AuthStatus.InActiveUser;
                                response.StatusMessage = Resource.errMessageDeactivatedUser;
                            }
                        }
                        else
                        {
                            response.StatusCode = AuthStatus.EmailNotConfirmed;
                            response.StatusMessage = Resource.errMessageEmailNotConfirmed;
                        }
                    }
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
                response.StatusMessage = Resource.errMessageFailed;
                LogManager.Logger.WriteException("AccountController", "ForgotPassword", ex.Message, ex);
            }
            return response;
        }

        [HttpGet]
        public async Task<MobileThemeViewModel> GetMobileTheme(string supplierCode, int appTypeId = (int)AppType.BuyerApp)
        {
            MobileThemeViewModel response = new MobileThemeViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetMobileTheme(supplierCode, appTypeId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AccountController", "GetMobileTheme", ex.Message, ex);
            }
            return response;
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<CompanyUserViewModel>> GetCompanyUsers(int companyID, int driverId)
        {
            List<CompanyUserViewModel> response = new List<CompanyUserViewModel>();
            try
            {
                var regionDispactherDetails = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetRegionDispactherDetails(driverId, companyID, string.Empty);
                if (regionDispactherDetails.Count() > 0)
                {
                    foreach (var item in regionDispactherDetails)
                    {
                        CompanyUserViewModel companyUserViewModel = new CompanyUserViewModel();
                        if (response.Where(top => top.RegionID != item.RegionID).Count() == 0)
                        {
                            companyUserViewModel.RegionID = item.RegionID;
                            companyUserViewModel.RegionName = item.RegionName;
                            companyUserViewModel.RegionDescription = string.IsNullOrEmpty(item.RegionDescription) ? string.Empty : item.RegionDescription;
                            companyUserViewModel.SendBirdRegionID = item.RegionID.Substring(0, 10).ToUpper();
                            List<CompanyUserDetails> comresponse = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserDetails(item.dispactherIds.Distinct().ToList());
                            if (comresponse.Count() > 0)
                            {
                                companyUserViewModel.companyUserDetails = comresponse;
                            }
                            response.Add(companyUserViewModel);
                        }
                    }

                }
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AccountController", "GetCompanyUsers", ex.Message, ex);
            }
            return response;
        }
        [HttpGet]
        [ApiLog(Enabled = true)]
        public AppSettingViewModel GetSendBirdAPPKey()
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetAppSettings(ApplicationConstants.SendbirdAppId.ToString());
            return response;
        }
    }
}
