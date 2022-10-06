using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Controllers
{

    public partial class AccountController
    {
        #region parklandOktaIntegration 

        [AllowAnonymous]
        public ActionResult ParklandOktaLogin(string returnUrl)
        {
            if (returnUrl != null && (returnUrl.ToLower().Contains(ApplicationConstants.IdpReturnUrlAccessDenied) || returnUrl.ToLower().Contains(ApplicationConstants.IdpReturnUrlGenericerror)))
            {
                return View(new LoginViewModel());
            }
            return new Saml2ChallengeResult(Url.Action("ParklandOktaLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ParklandOktaLoginCallback(string returnUrl)
        {

            LoginViewModel viewModel = new LoginViewModel();
            try
            {
                var loginInfo = await HttpContext.GetOwinContext().Authentication.GetExternalLoginInfoAsync();
                if (loginInfo == null)
                {
                    LogManager.Logger.WriteDebug("AccountController.Okta", "GetExternalLoginInfoAsync", "No user found in Okta");
                    return View(viewModel);
                }

                #region TODO - will get claim and role for integration 
                //var identity = new ClaimsIdentity(loginInfo.ExternalIdentity.Claims,
                //    DefaultAuthenticationTypes.ApplicationCookie);
                //var authProps = new AuthenticationProperties
                //{
                //    IsPersistent = true,
                //    ExpiresUtc = DateTime.UtcNow.AddMinutes(1)
                //};
                //HttpContext.GetOwinContext().Authentication.SignIn(authProps, identity);
                #endregion

                UserViewModel userViewModel = new UserViewModel();
                viewModel.Email = loginInfo.Login.ProviderKey;

                var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                var authenticationDomain = ContextFactory.Current.GetDomain<AuthenticationDomain>();

                var user = await authenticationDomain.GetUserForOktaAsync(viewModel.Email);

                if (user == null)
                {
                    // check invited user, if yes prompt user registration .
                    var invitedUser = await authenticationDomain.GetAdditionalUserInviteByEmailAsync(viewModel.Email);
                    if (invitedUser.StatusCode == Status.Success)
                    {
                        string supplierURL = authenticationDomain.GetSupplierURLDetails(invitedUser.CustomerCompanyId);
                        return RedirectToAction("RegisterCompanyUser", "Account", new { area = "", id = invitedUser.Id, supplierURL = supplierURL });
                    }
                    else
                    {
                        userViewModel.StatusCode = AuthStatus.InvalidUser;
                        userViewModel.StatusMessage = Resource.errMessageUserNotExist;
                        DisplayCustomMessages((MessageType)Status.Failed, userViewModel.StatusMessage);
                        return View(viewModel);
                    }
                }

                if (!user.IsActive)
                {
                    userViewModel.StatusCode = AuthStatus.InvalidUser;
                    userViewModel.StatusMessage = Resource.errMessageUserInactive;
                    DisplayCustomMessages((MessageType)Status.Failed, userViewModel.StatusMessage);
                    return View(viewModel);
                }

                var cIs = await authenticationDomain.GetCompanyIdentityServices(user);
                if (cIs == null)
                {
                    userViewModel.StatusCode = AuthStatus.LoginFailed;
                    userViewModel.StatusMessage = Resource.errMessageIDPNotEnabled;
                    DisplayCustomMessages((MessageType)Status.Failed, userViewModel.StatusMessage);
                    return View(viewModel);
                }


                userViewModel = await authenticationDomain.PasswordSignInAsync(user, "", true, (int)LoginSource.Idp);

                viewModel.StatusMessage = userViewModel.StatusMessage;
                if (userViewModel.StatusCode == AuthStatus.AcceptEULA)
                {
                    return RedirectToAction("BuyerEULA", new { id = userViewModel.Id });
                }
                else if (userViewModel.StatusCode == AuthStatus.Success || userViewModel.StatusCode == AuthStatus.TPOBuyerNotOnboarded)
                {
                    userViewModel.ApplicationTemplateId = (int)ApplicationTemplate.TrueFill;
                    //Configure Supplier Website
                    ConfigureSupplierWebsite(viewModel, domain, userViewModel);
                    viewModel.StatusCode = Status.Success;
                    SetAuthenticationClaims(userViewModel, viewModel.RememberMe);
                    if (userViewModel.StatusCode == AuthStatus.TPOBuyerNotOnboarded)
                    {
                        DisplayCustomMessages((MessageType)Status.Warning,
                                                    $"{viewModel.StatusMessage} ",
                                                    Resource.btnLabelClickHere,
                                                    Url.Action("Company", "Onboarding", new { area = "", id = userViewModel.Id }));
                        if (string.IsNullOrEmpty(viewModel.SupplierURL))
                        {
                            return View(viewModel);
                        }
                        else
                        {
                            return RedirectToAction("SupplierLogin", new { supplierURL = viewModel.SupplierURL });
                        }
                    }

                    if (!userViewModel.IsOnboardingComplete)
                    {
                        return RedirectToAction("Company", "Onboarding", new { area = "" });
                    }
                    domain.UpdateLastAccessedDate(userViewModel.Id);
                    return Redirect(GetRedirectUrl(returnUrl));
                }
                else if (userViewModel.StatusCode == AuthStatus.EmailNotConfirmed)
                {
                    viewModel.StatusCode = Status.Warning;
                    DisplayCustomMessages((MessageType)viewModel.StatusCode,
                                                $"{viewModel.StatusMessage}. ",
                                                Resource.btnLabelResend,
                                                GetResendActivationLink(viewModel.SupplierURL));
                }
                else
                {
                    viewModel.StatusCode = Status.Failed;
                    DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AccountController", "ParklandOktaLoginCallback", ex.Message + "\nEmail:" + viewModel.Email, ex);
                DisplayCustomMessages((MessageType)Status.Failed, "Something went wrong.");
            }

            return View(viewModel);
        }

        #endregion
    }
}