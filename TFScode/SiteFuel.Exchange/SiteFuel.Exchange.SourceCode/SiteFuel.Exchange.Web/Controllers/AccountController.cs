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
    [AllowAnonymous]
    public partial class AccountController : BaseController
    {
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult Login(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (returnUrl.Contains(ApplicationConstants.SupplierURL))
                {
                    var supplierURL = returnUrl.Split('_');
                    if (supplierURL.Length > 0)
                    {
                        return RedirectToAction("SupplierLogin", new { supplierURL = supplierURL[1] });
                    }

                }
            }
           
            if (Request.UrlReferrer != null && !Request.IsAuthenticated && !string.IsNullOrEmpty(returnUrl) 
                && !returnUrl.Equals("/") && !Request.UrlReferrer.AbsoluteUri.Contains(ApplicationConstants.IdpOkta))
            {
                DisplayCustomMessages(MessageType.Warning, Resource.errMessageCookiesTimedOut);
            }
            
            ViewBag.ReturnUrl = returnUrl;
            if (Request.UrlReferrer!=null && Request.UrlReferrer.AbsoluteUri.Contains(ApplicationConstants.IdpOkta))
            {
                // when user is logged in with diff roles and sign in by clicking on app link okta dashboard 
                // then the user is logged in using users exchange url and leads to unauthorize access if the 
                //  page is not acceissable to new user. hence the change to "/" for okta user to redirect role type based default page.
                //  current limitation with login with okta
                return ParklandOktaLogin("/"); 
            }
            return View(new LoginViewModel());
        }
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult SupplierLogin(string supplierURL)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            if (!string.IsNullOrEmpty(supplierURL))
            {
                supplierURL = supplierURL.Trim().ToUpper();
                var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                var websiteLogoPath = domain.WebSiteConfigurationDetails(supplierURL, 0, 0);
                if (!string.IsNullOrEmpty(websiteLogoPath.Item1) || !string.IsNullOrEmpty(websiteLogoPath.Item2))
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    imageViewModel.FilePath = websiteLogoPath.Item1;
                    loginViewModel.SupplierLogoPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    imageViewModel.FilePath = websiteLogoPath.Item2;
                    loginViewModel.BackgroundImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    loginViewModel.SupplierURL = supplierURL;
                    loginViewModel.ButtonColor = websiteLogoPath.Item4;
                    imageViewModel.FilePath = websiteLogoPath.Item5;
                    loginViewModel.FaviconImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    ViewBag.supplierURL = supplierURL;
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }

            return View(loginViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SupplierLogin(LoginViewModel viewModel, string returnUrl, string supplierURL)
        {
            using (var tracer = new Tracer("AccountController", "SupplierLogin"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                        var userViewModel = await domain.PasswordSignInAsync(viewModel);
                        viewModel.StatusMessage = userViewModel.StatusMessage;
                        if (userViewModel.StatusCode == AuthStatus.AcceptEULA)
                        {
                            return RedirectToAction("BuyerEULA", new { id = userViewModel.Id });
                        }
                        else if (userViewModel.StatusCode == AuthStatus.Success || userViewModel.StatusCode == AuthStatus.TPOBuyerNotOnboarded)
                        {
                            userViewModel.ApplicationTemplateId = ContextFactory.Current.GetDomain<HelperDomain>().GetApplicationTemplateId(viewModel.SupplierURL);
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
                                    return RedirectToAction("Login");
                                }
                                else
                                {
                                    return RedirectToAction("SupplierLogin", new { supplierURL = viewModel.SupplierURL });
                                }
                            }

                            if (!userViewModel.IsOnboardingComplete)
                            {
                                return RedirectToAction("Company", "Onboarding", new { supplierURL = viewModel.SupplierURL });
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
                        LogManager.Logger.WriteException("AccountController", "SupplierLogin", ex.Message + "\nEmail:" + viewModel.Email, ex);
                        DisplayCustomMessages((MessageType)Status.Failed, "Something went wrong.");
                    }
                }
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel viewModel, string returnUrl)
        {
            using (var tracer = new Tracer("AccountController", "Login"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                        var userViewModel = await domain.PasswordSignInAsync(viewModel);
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
                                    return RedirectToAction("Login");
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
                        LogManager.Logger.WriteException("AccountController", "Login", ex.Message + "\nEmail:" + viewModel.Email, ex);
                        DisplayCustomMessages((MessageType)Status.Failed, "Something went wrong.");
                    }
                }
                return View(viewModel);
            }
        }

        private static void ConfigureSupplierWebsite(LoginViewModel viewModel, AuthenticationDomain domain, UserViewModel userViewModel)
        {
            userViewModel.SupplierURL = string.IsNullOrEmpty(viewModel.SupplierURL) ? string.Empty : viewModel.SupplierURL;
            userViewModel.ButtonColor = string.IsNullOrEmpty(viewModel.ButtonColor) ? string.Empty : viewModel.ButtonColor;
            var websiteLogoPath = domain.WebSiteConfigurationDetails(viewModel.SupplierURL, userViewModel.CompanyId, userViewModel.Id);
            if (!string.IsNullOrEmpty(websiteLogoPath.Item1) || !string.IsNullOrEmpty(websiteLogoPath.Item2))
            {
                ImageViewModel imageViewModel = new ImageViewModel();
                imageViewModel.FilePath = websiteLogoPath.Item1;
                userViewModel.SupplierLogoPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                userViewModel.BrandedCompanyId = websiteLogoPath.Item3;
            }
            else
            {
                userViewModel.SupplierLogoPath = string.Empty;
            }
            //if (!string.IsNullOrEmpty(websiteLogoPath.Item5))
            //{
            //    ImageViewModel imageViewModel = new ImageViewModel();
            //    imageViewModel.FilePath = websiteLogoPath.Item5;
            //    userViewModel.SupplierFaviconPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
            //}
            //else
            //{
            //    userViewModel.SupplierFaviconPath = string.Empty;
            //}
        }

        [AuthorizeRole] // All roles are allowed
        public async Task<ActionResult> Logout()
        {
            var currentSupplierURL = CurrentUser.SupplierURL;
            await SignOut();
            if (!string.IsNullOrEmpty(currentSupplierURL))
            {
                return RedirectToAction("Index", "Home", new { area = "", supplierURL = ApplicationConstants.SupplierURL + currentSupplierURL });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult Register(string supplierURL, int invitationId = 0)
        {
            ViewBag.supplierURL = supplierURL;
            var viewModel = new RegisterViewModel();
            if (invitationId > 0)
            {
                var invitedCompanyDetails = new InvitationDomain().GetInvitedCompanyRawDataById(invitationId);
                viewModel.Company.Name = invitedCompanyDetails.CompanyInfo.CompanyName;
                viewModel.Company.CompanyTypeId = invitedCompanyDetails.CompanyInfo.CompanyTypeId;
                viewModel.Title = invitedCompanyDetails.UserInfo.Title;
                viewModel.FirstName = invitedCompanyDetails.UserInfo.FirstName;
                viewModel.LastName = invitedCompanyDetails.UserInfo.LastName;
                viewModel.Email = invitedCompanyDetails.UserInfo.Email;
                viewModel.IsInvitedCompanyRegistered = invitedCompanyDetails.IsInvitedCompanyRegistered;
                viewModel.RegisteredCompanyId = invitedCompanyDetails.RegisteredCompanyId;
                viewModel.InvitationId = invitedCompanyDetails.Id;
                viewModel.MobileNumber = invitedCompanyDetails.CompanyInfo.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(supplierURL))
            {
                var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                var websiteLogoPath = domain.WebSiteConfigurationDetails(supplierURL, 0, 0);
                if (!string.IsNullOrEmpty(websiteLogoPath.Item1) || !string.IsNullOrEmpty(websiteLogoPath.Item2))
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    imageViewModel.FilePath = websiteLogoPath.Item1;
                    viewModel.SupplierLogoPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    viewModel.SupplierURL = supplierURL;
                    imageViewModel.FilePath = websiteLogoPath.Item2;
                    viewModel.ButtonColor = websiteLogoPath.Item4;
                    viewModel.BackgroundImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                }
                else
                {
                    viewModel.SupplierURL = string.Empty;
                }
                if (!string.IsNullOrEmpty(websiteLogoPath.Item5))
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    imageViewModel.FilePath = websiteLogoPath.Item5;
                    viewModel.FaviconImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel viewModel, string supplierURL)
        {
            using (var tracer = new Tracer("AccountController", "Register"))
            {
                if (ModelState.IsValid)
                {
                    Status response = new Status();

                    var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                    response = await domain.CreateUserAsync(viewModel, null);

                    if (response == Status.Success)
                    {
                        viewModel.StatusCode = Status.Success;
                        viewModel.StatusMessage = Resource.errMessageUserCreateSuccess;



                        // Send an email with this link
                        var confirmationToken = await domain.GenerateEmailConfirmationTokenAsync(viewModel.Email);
                        if (confirmationToken.Id > 0)
                        {
                            var helperDomain = new HelperDomain(domain);
                            var notificationDomain = new NotificationDomain(domain);
                            var serverUrl = helperDomain.GetServerUrl();
                            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { area = "", userId = confirmationToken.Id, code = confirmationToken.Token });
                            if (!string.IsNullOrEmpty(viewModel.SupplierURL))
                            {
                                callbackUrl = Url.Action("ConfirmEmail", "Account", new { area = "", userId = confirmationToken.Id, code = confirmationToken.Token, supplierURL = viewModel.SupplierURL });
                            }
                            LogManager.Logger.WriteDebug("ConfirmEmail", "Account", "userId = " + confirmationToken.Id + " Code = " + confirmationToken.Token);
                            var notification = notificationDomain.GetNotificationContent(EventSubType.EmailVerification, serverUrl, callbackUrl, null, viewModel.SupplierURL);
                            var applicationTemplate = notificationDomain.GetApplicationTemplate(viewModel.SupplierURL);
                            var emailModel = new ApplicationEventNotificationViewModel
                            {
                                To = new List<string> { viewModel.Email },
                                Subject = string.Format(notification.Subject, applicationTemplate.BrandedCompanyName),
                                CompanyLogo = applicationTemplate.CompanyLogo,
                                CompanyText = notification.CompanyText,
                                BodyLogo = notification.BodyLogo,
                                BodyText = string.Format(notification.BodyText, $"{viewModel.FirstName} {viewModel.LastName}", applicationTemplate.BrandedCompanyName),
                                BodyButtonText = notification.BodyButtonText,
                                BodyButtonUrl = notification.BodyButtonUrl,
                                ShowUserSettingsLink = false,
                                From = applicationTemplate.FromEmail,
                                SenderName = applicationTemplate.SenderName,
                                ApplicationTemplateId = applicationTemplate.ApplicationTemplateId
                            };
                            await new EmailDomain(domain).SendEmail(applicationTemplate.Template, emailModel);

                            ModelState.Clear();
                            viewModel.StatusCode = Status.Success;
                            viewModel.StatusMessage = Resource.errMessageCreateAccountSuccess;
                        }
                        else
                        {
                            viewModel.StatusCode = Status.Warning;
                            viewModel.StatusMessage = Resource.errMessageConfirmationEmailFailed;

                            DisplayCustomMessages((MessageType)viewModel.StatusCode,
                                                    $"{viewModel.StatusMessage}. ",
                                                    Resource.btnLabelResend,
                                                    GetResendActivationLink(viewModel.SupplierURL));

                            return View(viewModel);
                        }
                    }
                    DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                }
                return RedirectToAction("RegisterConfirmation", "Account", new { area = "", supplierURL = supplierURL, invitationId = viewModel.InvitationId });
            }
        }

        [HttpGet]
        public async Task<ActionResult> RegisterCompanyUser(int id, string supplierURL)
        {
            using (var tracer = new Tracer("AccountController", "RegisterCompanyUser"))
            {
                var viewModel = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetAdditionalUserInviteAsync(id);
                var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                if (viewModel.StatusCode != Status.Success)
                {
                    DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                    if (viewModel.Company != null)
                    {
                        //var websiteBrandDetails = domain.GetSupplierURLDetails(viewModel.Company.Id);
                        if (!string.IsNullOrEmpty(supplierURL))
                        {
                            return RedirectToAction("SupplierLogin", "Account", new { area = "", supplierURL = supplierURL });
                        }
                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    }
                    return RedirectToAction("Login", "Account");
                }

                if (viewModel.Company != null && viewModel.InvitedBy > 0)
                {
                    var websiteLogoPath = domain.WebSiteConfigurationDetails(supplierURL, string.IsNullOrEmpty(supplierURL) ? viewModel.Company.Id : 0, string.IsNullOrEmpty(supplierURL) ? viewModel.InvitedBy : 0);
                    if (!string.IsNullOrEmpty(websiteLogoPath.Item1) || !string.IsNullOrEmpty(websiteLogoPath.Item2) || !string.IsNullOrEmpty(websiteLogoPath.Item4))
                    {
                        ImageViewModel imageViewModel = new ImageViewModel();
                        imageViewModel.FilePath = websiteLogoPath.Item1;
                        viewModel.SupplierLogoPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                        imageViewModel.FilePath = websiteLogoPath.Item2;
                        viewModel.BackgroundImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                        viewModel.ButtonColor = websiteLogoPath.Item4;
                        if (string.IsNullOrEmpty(supplierURL))
                        {
                            var supplierURLDetails = domain.GetSupplierURLDetails(viewModel.Company.Id);
                            viewModel.SupplierURL = supplierURLDetails;
                            ViewBag.supplierURL = supplierURLDetails;
                        }
                        else
                        {
                            viewModel.SupplierURL = supplierURL;
                            ViewBag.supplierURL = supplierURL;
                        }
                    }
                    if (!string.IsNullOrEmpty(websiteLogoPath.Item5))
                    {
                        ImageViewModel imageViewModel = new ImageViewModel();
                        imageViewModel.FilePath = websiteLogoPath.Item5;
                        viewModel.FaviconImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    }
                    var res = ContextFactory.Current.GetDomain<HelperDomain>().GetExternalIdentityProviders(viewModel.Company.Id);

                    if (res != null && res.Count > 0 && viewModel.RoleIds != null)
                    {
                        if (viewModel.RoleIds.Contains((int)UserRoles.Driver) || viewModel.RoleIds.Contains((int)UserRoles.Buyer))
                        {
                            viewModel.IsRegisterThroughOkta = false; // force to enter password while company user registration 
                        }
                        else
                        {
                            viewModel.IsRegisterThroughOkta = true; // random default password will be saved while user registration 
                        }
                    }
                }
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterCompanyUser(RegisterAdditionalUserViewModel viewModel)
        {
            var response = new StatusViewModel();
            response.StatusMessage = Resource.errMessageRegisterCompanyUserFailed;
            using (var tracer = new Tracer("AccountController", "RegisterCompanyUser(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    if (viewModel.IsRegisterThroughOkta)
                    {
                        var StaticPassword = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingApplicationUserDefaultPassword, "First#1234");
                        var RandomPassword = CryptoHelperMethods.GeneratePassword(Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings[ApplicationConstants.RandomPasswordLength]), StaticPassword);
                        viewModel.Password = RandomPassword;
                        viewModel.ConfirmPassword = RandomPassword;
                    }
                    response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().OnboardCompanyUserAsync(viewModel);
                    if (response.StatusCode == Status.Success)
                    {
                        if (viewModel.Company != null)
                        {  
                            //var websiteBrandDetails = domain.GetSupplierURLDetails(viewModel.Company.Id);
                            if (!string.IsNullOrEmpty(viewModel.SupplierURL))
                            {
                                return RedirectToAction("SupplierLogin", "Account", new { area = "", supplierURL = viewModel.SupplierURL });
                            }
                            else
                            {
                                return RedirectToAction("Login", "Account");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }

                    }
                    DisplayCustomMessages((MessageType)Status.Failed, response.StatusMessage);
                }
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<ActionResult> RegisterInvitedCompanyUser(int id)
        {
            using (var tracer = new Tracer("AccountController", "RegisterInvitedCompanyUser"))
            {
                var viewModel = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetInvitedCompanyUserAsync(id);
                //Need to decide whether to show error message - As of now No Error Display
                //DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                return View("Register", viewModel);
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        [AllowAnonymous]
        public ActionResult UserAgreement()
        {
            return View();
        }

        

        public async Task<ActionResult> ConfirmEmail(int userId, string code, int encoded = 0, string supplierURL = "")
        {
            using (var tracer = new Tracer("AccountController", "ConfirmEmail"))
            {
                if (userId > 0 && code != null)
                {
                    //if (encoded == 1)
                    //    code = HttpUtility.UrlDecode(code);

                    var response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().ConfirmEmailAsync(userId, code);
                    if (response == Status.Success)
                    {
                        DisplayCustomMessages((MessageType)Status.Success, Resource.errMessageEmailConfirmed);
                    }
                    else
                    {
                        var user = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserByIdAsync(userId);
                        var linkURL = Url.Action("ResendActivationLink", "Account", new { area = "", email = user.Email });
                        if (!string.IsNullOrEmpty(supplierURL))//websiteBrandDetails.StatusCode == Status.Success)
                        {
                            linkURL = Url.Action("ResendActivationLink", "Account", new { area = "", email = user.Email, supplierURL = supplierURL });
                        }
                        DisplayCustomMessages((MessageType)Status.Warning,
                                                $"{Resource.errMessageLinkExpired}. ",
                                                Resource.btnLabelResend,
                                                linkURL);
                    }
                }
                else
                {
                    DisplayCustomMessages((MessageType)Status.Failed, Resource.errMessageFailedConfirmEmailUserNotFound);
                }
                //var websiteBrandDetails = await ContextFactory.Current.GetDomain<AuthenticationDomain>().CheckSupplierInvited(userId);
                if (!string.IsNullOrEmpty(supplierURL))//websiteBrandDetails.StatusCode == Status.Success)
                {
                    return RedirectToAction("SupplierLogin", "Account", new { area = "", supplierURL = supplierURL });
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

            }
        }

        [HttpGet]
        public ActionResult ResendActivationLink(string email, string supplierURL)
        {
            using (var tracer = new Tracer("AccountController", "ResendActivationLink"))
            {
                var viewModel = new ForgotPasswordViewModel();
                if (!string.IsNullOrEmpty(email))
                {
                    viewModel.Email = email;
                    var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                    var websiteLogoPath = domain.WebSiteConfigurationDetails(supplierURL, 0, 0);
                    if (!string.IsNullOrEmpty(websiteLogoPath.Item1) || !string.IsNullOrEmpty(websiteLogoPath.Item2) || !string.IsNullOrEmpty(websiteLogoPath.Item4))
                    {
                        ImageViewModel imageViewModel = new ImageViewModel();
                        imageViewModel.FilePath = websiteLogoPath.Item1;
                        viewModel.SupplierLogoPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                        imageViewModel.FilePath = websiteLogoPath.Item2;
                        viewModel.ButtonColor = websiteLogoPath.Item4;
                        viewModel.BackgroundImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                        viewModel.SupplierURL = supplierURL;
                        ViewBag.supplierURL = supplierURL;
                    }
                    if (!string.IsNullOrEmpty(websiteLogoPath.Item5))
                    {
                        ImageViewModel imageViewModel = new ImageViewModel();
                        imageViewModel.FilePath = websiteLogoPath.Item5;
                        viewModel.FaviconImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    }
                }
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<ActionResult> BuyerEULA(int id)
        {
            var response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().IsEulaAccepted(id);
            if (!response)
            {
                var websiteBrandDetails = await ContextFactory.Current.GetDomain<AuthenticationDomain>().CheckSupplierInvited(id);
                if (websiteBrandDetails.StatusCode == Status.Success)
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    imageViewModel.FilePath = websiteBrandDetails.EntityNumber;
                    ViewBag.SupplierLogoURL = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                }
                else
                {
                    ViewBag.SupplierLogoURL = string.Empty;
                }
                return View("BuyerEULA", id);
            }
            else
            {
                var websiteBrandDetails = await ContextFactory.Current.GetDomain<AuthenticationDomain>().CheckSupplierInvited(id);
                if (websiteBrandDetails.StatusCode == Status.Success)
                {
                    return RedirectToAction("SupplierLogin", "Account", new { area = "", supplierURL = websiteBrandDetails.StatusMessage });
                }
                else
                {
                    return RedirectToAction("Login", "Account", new { area = "" });
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> EulaAccepted(int id)
        {
            var response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().EulaAccepted(id);
            if (response.StatusCode == Status.Success)
            {
                var websiteBrandDetails = await ContextFactory.Current.GetDomain<AuthenticationDomain>().CheckSupplierInvited(id);
                if (websiteBrandDetails.StatusCode == Status.Success)
                {
                    return RedirectToAction("SupplierLogin", "Account", new { area = "", supplierURL = websiteBrandDetails.StatusMessage });
                }
                else
                {
                    return RedirectToAction("Login", "Account", new { area = "" });
                }
                //return View("AppDownloadLink");
            }
            else
            {
                return View("BuyerEULA", id);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResendActivationLink(ForgotPasswordViewModel viewModel)
        {
            using (var tracer = new Tracer("AccountController", "ResendActivationLink"))
            {
                if (ModelState.IsValid)
                {
                    var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                    var notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                    var userViewModel = await domain.GetUserByEmailAsync(viewModel.Email);

                    if (userViewModel.StatusCode == AuthStatus.Success)
                    {
                        // Send an email with this link
                        var confirmationToken = await domain.GenerateEmailConfirmationTokenAsync(viewModel.Email);
                        if (confirmationToken.Id > 0)
                        {
                            var serverUrl = new HelperDomain(domain).GetServerUrl();
                            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { area = "", userId = confirmationToken.Id, code = confirmationToken.Token });
                            if (!string.IsNullOrEmpty(viewModel.SupplierURL))
                            {
                                callbackUrl = Url.Action("ConfirmEmail", "Account", new { area = "", userId = confirmationToken.Id, code = confirmationToken.Token, supplierURL = viewModel.SupplierURL });
                            }
                            var notification = notificationDomain.GetNotificationContent(EventSubType.EmailVerification, serverUrl, callbackUrl);
                            var applicationTemplate = notificationDomain.GetApplicationTemplate(viewModel.SupplierURL);
                            var emailModel = new ApplicationEventNotificationViewModel
                            {
                                To = new List<string> { viewModel.Email },
                                Subject = string.Format(notification.Subject, applicationTemplate.BrandedCompanyName),
                                CompanyLogo = applicationTemplate.CompanyLogo,
                                CompanyText = notification.CompanyText,
                                BodyLogo = notification.BodyLogo,
                                BodyText = string.Format(notification.BodyText, $"{userViewModel.FirstName} {userViewModel.LastName}", applicationTemplate.BrandedCompanyName),
                                BodyButtonText = notification.BodyButtonText,
                                BodyButtonUrl = notification.BodyButtonUrl,
                                ShowUserSettingsLink = false,
                                From = applicationTemplate.FromEmail,
                                SenderName = applicationTemplate.SenderName,
                                ApplicationTemplateId = applicationTemplate.ApplicationTemplateId
                            };
                            await new EmailDomain(domain).SendEmail(applicationTemplate.Template, emailModel);

                            ModelState.Clear();
                            viewModel.StatusCode = AuthStatus.Success;
                            viewModel.StatusMessage = Resource.errMessageCheckYourEmail;
                        }
                        else
                        {
                            viewModel.StatusCode = AuthStatus.TokenFailure;
                            viewModel.StatusMessage = Resource.errMessageLoginFailed;
                            DisplayCustomMessages(MessageType.Warning,
                                                    $"{viewModel.StatusMessage}. ",
                                                    Resource.btnLabelResend,
                                                    GetResendActivationLink(viewModel.SupplierURL));

                            return View(viewModel);
                        }
                    }
                    DisplayCustomMessages(viewModel.StatusCode == AuthStatus.Success ? MessageType.Success : MessageType.Error, viewModel.StatusMessage);
                }
                return View(viewModel);
            }
        }

        private string GetResendActivationLink(string supplierURL)
        {
            string linkURL = Url.Action("ResendActivationLink", "Account", new { area = "" });
            if (!string.IsNullOrEmpty(supplierURL))
            {
                linkURL = Url.Action("ResendActivationLink", "Account", new { supplierURL = supplierURL });
            }
            return linkURL;
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult ForgotPassword(string supplierURL)
        {
            ForgotPasswordViewModel forgotPasswordViewModel = new ForgotPasswordViewModel();
            if (!string.IsNullOrEmpty(supplierURL))
            {
                supplierURL = supplierURL.Trim().ToUpper();
                var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                var websiteLogoPath = domain.WebSiteConfigurationDetails(supplierURL, 0, 0);
                if (!string.IsNullOrEmpty(websiteLogoPath.Item1) || !string.IsNullOrEmpty(websiteLogoPath.Item2) || !string.IsNullOrEmpty(websiteLogoPath.Item4) || !string.IsNullOrEmpty(websiteLogoPath.Item5))
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    imageViewModel.FilePath = websiteLogoPath.Item1;
                    forgotPasswordViewModel.SupplierLogoPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    imageViewModel.FilePath = websiteLogoPath.Item2;
                    forgotPasswordViewModel.ButtonColor = websiteLogoPath.Item4;
                    forgotPasswordViewModel.BackgroundImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    imageViewModel.FilePath = websiteLogoPath.Item5;
                    forgotPasswordViewModel.FaviconImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    forgotPasswordViewModel.SupplierURL = supplierURL;
                    ViewBag.supplierURL = supplierURL;
                }
            }
            return View(forgotPasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            using (var tracer = new Tracer("AccountController", "ForgotPassword"))
            {
                if (ModelState.IsValid)
                {
                    var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                    var notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                    UserViewModel userViewModel = await domain.GetUserByEmailAsync(viewModel.Email);
                    bool flag = false;
                    if (userViewModel.StatusCode == AuthStatus.Success && userViewModel.IsEmailConfirmed && userViewModel.IsActive)
                    {
                        //stop reset user from tfx if okta is enable for that company
                        var res = ContextFactory.Current.GetDomain<HelperDomain>().GetExternalIdentityProviders(userViewModel.CompanyId);                        
                        if (res != null && res.Count > 0)
                        {
                            viewModel.StatusCode = AuthStatus.Failed;
                            flag = true;
                        }
                    }

                    string lblLinkText = string.Empty;
                    var messageType = MessageType.Error;
                    if (userViewModel.StatusCode == AuthStatus.Success)
                    {
                        if (userViewModel.IsEmailConfirmed)
                        {
                            if (!userViewModel.IsActive)
                            {
                                viewModel.StatusCode = AuthStatus.InActiveUser;
                                viewModel.StatusMessage = Resource.errMessageDeactivatedUser;
                            }
                            else
                            {
                                // Send an email with this link
                                var confirmationToken = await domain.GenerateEmailConfirmationTokenAsync(userViewModel.Id, userViewModel.Email);
                                if (confirmationToken.Id > 0)
                                {
                                    var serverUrl = ContextFactory.Current.GetDomain<HelperDomain>().GetServerUrl();
                                    var callbackUrl = Url.Action("ResetPassword", "Account", new { area = "", userId = confirmationToken.Id, email = userViewModel.Email, code = confirmationToken.Token });
                                    if (!string.IsNullOrEmpty(viewModel.SupplierURL))
                                    {
                                        callbackUrl = Url.Action("ResetPassword", "Account", new { area = "", userId = confirmationToken.Id, email = userViewModel.Email, code = confirmationToken.Token, supplierURL = viewModel.SupplierURL });
                                    }
                                    var notification = notificationDomain.GetNotificationContent(EventSubType.ForgotPassword, serverUrl, callbackUrl);
                                    var applicationTemplate = notificationDomain.GetApplicationTemplate(viewModel.SupplierURL);
                                    var emailModel = new ApplicationEventNotificationViewModel
                                    {
                                        To = new List<string> { viewModel.Email },
                                        Subject = string.Format(notification.Subject, applicationTemplate.BrandedCompanyName),
                                        CompanyLogo = applicationTemplate.CompanyLogo,
                                        CompanyText = notification.CompanyText,
                                        BodyLogo = notification.BodyLogo,
                                        BodyText = string.Format(notification.BodyText, $"{userViewModel.FirstName} {userViewModel.LastName}"),
                                        BodyButtonText = notification.BodyButtonText,
                                        BodyButtonUrl = notification.BodyButtonUrl,
                                        ShowUserSettingsLink = false,
                                        From = applicationTemplate.FromEmail,
                                        SenderName = applicationTemplate.SenderName,
                                        ApplicationTemplateId = applicationTemplate.ApplicationTemplateId
                                    };
                                    await ContextFactory.Current.GetDomain<EmailDomain>().SendEmail(applicationTemplate.Template, emailModel);

                                    ModelState.Clear();
                                    messageType = MessageType.Success;
                                    viewModel.StatusCode = AuthStatus.Success;
                                    viewModel.StatusMessage = Resource.errMessageCheckYourEmail;
                                }
                                else
                                {
                                    viewModel.StatusCode = AuthStatus.TokenFailure;
                                    viewModel.StatusMessage = Resource.errMessageConfirmationEmailFailed;
                                    lblLinkText = Resource.btnLabelResend;
                                    messageType = MessageType.Warning;
                                }
                            }
                        }
                        else
                        {
                            viewModel.StatusCode = AuthStatus.EmailNotConfirmed;
                            viewModel.StatusMessage = Resource.errMessageEmailNotConfirmed;
                            DisplayCustomMessages(MessageType.Error,
                                                    $"{viewModel.StatusMessage}. ",
                                                    Resource.btnLabelResend,
                                                    GetResendActivationLink(viewModel.SupplierURL));
                            return View(viewModel);
                        }
                    }
                    else
                    {
                        viewModel.StatusCode = AuthStatus.InvalidUser;
                        if (!flag)
                        {
                            viewModel.StatusMessage = Resource.errMessageUserNotExist;
                        }
                        else
                        {
                            viewModel.StatusMessage = Resource.errMessageOktaForgetPassword;
                        }
                    }

                    if (string.IsNullOrEmpty(viewModel.SupplierURL))
                    {
                        DisplayCustomMessages(messageType, viewModel.StatusMessage, lblLinkText, Url.Action("ForgotPassword", new { area = "" }));
                    }
                    else
                    {
                        DisplayCustomMessages(messageType, viewModel.StatusMessage, lblLinkText, Url.Action("ForgotPassword", new { supplierURL = viewModel.SupplierURL }));
                    }
                }
                return View(viewModel);
            }
        }

        public async Task<ActionResult> ResetPassword(int userId, string email, string code, string supplierURL)
        {
            using (var tracer = new Tracer("AccountController", "ResetPassword"))
            {
                var viewModel = new ResetPasswordViewModel();
                if (userId > 0 && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(code))
                {
                    var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                    var websiteLogoPath = domain.WebSiteConfigurationDetails(supplierURL, 0, 0);
                    var response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().ConfirmEmailAsync(userId, code);
                    if (response == Status.Success)
                    {
                        if (!string.IsNullOrEmpty(websiteLogoPath.Item1) || !string.IsNullOrEmpty(websiteLogoPath.Item2))
                        {
                            ImageViewModel imageViewModel = new ImageViewModel();
                            imageViewModel.FilePath = websiteLogoPath.Item1;
                            viewModel.SupplierLogoPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                            imageViewModel.FilePath = websiteLogoPath.Item2;
                            viewModel.BackgroundImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                            viewModel.SupplierURL = supplierURL;
                            viewModel.ButtonColor = websiteLogoPath.Item4;
                        }
                        if (!string.IsNullOrEmpty(websiteLogoPath.Item5))
                        {
                            ImageViewModel imageViewModel = new ImageViewModel();
                            imageViewModel.FilePath = websiteLogoPath.Item5;
                            viewModel.FaviconImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                        }
                        viewModel.Email = email;
                    }
                    else
                    {
                        ForgotPasswordViewModel forgotPasswordViewModel = new ForgotPasswordViewModel();
                        if (!string.IsNullOrEmpty(websiteLogoPath.Item1) || !string.IsNullOrEmpty(websiteLogoPath.Item2))
                        {
                            ImageViewModel imageViewModel = new ImageViewModel();
                            imageViewModel.FilePath = websiteLogoPath.Item1;
                            forgotPasswordViewModel.SupplierLogoPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                            imageViewModel.FilePath = websiteLogoPath.Item2;
                            forgotPasswordViewModel.BackgroundImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                            forgotPasswordViewModel.SupplierURL = supplierURL;
                            forgotPasswordViewModel.ButtonColor = websiteLogoPath.Item4;
                            forgotPasswordViewModel.Email = email;
                        }
                        if (!string.IsNullOrEmpty(websiteLogoPath.Item5))
                        {
                            ImageViewModel imageViewModel = new ImageViewModel();
                            imageViewModel.FilePath = websiteLogoPath.Item5;
                            forgotPasswordViewModel.FaviconImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                        }
                        DisplayCustomMessages(MessageType.Warning, Resource.errMessageLinkExpired);
                        return View("ForgotPassword", forgotPasswordViewModel);
                    }
                }
                else
                {
                    DisplayCustomMessages((MessageType)Status.Failed, Resource.errMessageFailedResetPasswordUserNotFound);
                }
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            using (var tracer = new Tracer("AccountController", "ResetPassword"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().ResetPasswordAsync(viewModel);
                    if (response == Status.Success)
                    {
                        DisplayCustomMessages((MessageType)Status.Success, Resource.errMessagePasswordResetSuccess);
                        if (string.IsNullOrEmpty(viewModel.SupplierURL))
                        {
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            return RedirectToAction("SupplierLogin", new { supplierURL = viewModel.SupplierURL });
                        }
                    }
                    DisplayCustomMessages((MessageType)Status.Failed, Resource.errMessageFailedResetPasswordUserNotFound);
                }
                return View(viewModel);
            }
        }

        [AuthorizeRole]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel() { UserId = CurrentUser.Id });
        }

        [HttpPost]
        [AuthorizeRole]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            using (var tracer = new Tracer("AccountController", "ChangePassword"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.UserId = CurrentUser.Id;
                    var response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().ChangePasswordAsync(viewModel);
                    if (response.StatusCode == Status.Success)
                    {
                        await SignOut(true);
                        if (string.IsNullOrEmpty(CurrentUser.SupplierURL))
                            return RedirectToAction("Login", "Account", new { area = "" });
                        else
                            return RedirectToAction("SupplierLogin", "Account", new { supplierURL = CurrentUser.SupplierURL });
                    }
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                return View(viewModel);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin)]
        public ActionResult ChangeCompanyType(CompanyType companyType)
        {
            using (var tracer = new Tracer("AccountController", "ChangeCompanyType"))
            {
                var Identity = new ClaimsIdentity(User.Identity);
                Identity.RemoveClaim(Identity.FindFirst(ApplicationSecurityConstants.SelectedProfile));
                Identity.AddClaim(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)companyType).ToString()));
                var rememberMe = Identity.FindFirst(ApplicationSecurityConstants.RememberMe);
                AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                                                                        new ClaimsPrincipal(Identity),
                                                                        new AuthenticationProperties
                                                                        {
                                                                            IsPersistent = rememberMe != null ? Convert.ToBoolean(rememberMe.Value) : true
                                                                        });
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        public async Task<ActionResult> ImpersonateUser(int id, CurrentUserImpersonationFlag impersonation = CurrentUserImpersonationFlag.Impersonating)
        {
            using (var tracer = new Tracer("AccountController", "ImpersonateUser"))
            {
                var authenticationDomain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                var userViewModel = await authenticationDomain.PasswordSignInAsync(id, true);

                if (userViewModel.StatusCode == AuthStatus.Success)
                {
                    //clear logged in SUPER ADMIN's claim
                    await SignOut();
                    //Configure Supplier Website
                    LoginViewModel viewModel = new LoginViewModel();
                    ConfigureSupplierWebsite(viewModel, authenticationDomain, userViewModel);
                    var claims = new List<Claim>
                    {
                        new Claim(ApplicationSecurityConstants.UserId, userViewModel.Id.ToString()),
                        new Claim(ApplicationSecurityConstants.UserName, userViewModel.FullName),
                        new Claim(ApplicationSecurityConstants.Email, userViewModel.Email),
                        new Claim(ApplicationSecurityConstants.CookieTimeStamp, DateTimeOffset.Now.ToString(ApplicationSecurityConstants.CookieTimeStampFormat, CultureInfo.InvariantCulture)),
                        new Claim(ApplicationSecurityConstants.UserFingerPrint, userViewModel.FingerPrint),
                        new Claim(ApplicationSecurityConstants.CompanyId, userViewModel.CompanyId.ToString()),
                        new Claim(ApplicationSecurityConstants.CompanyTypeId, userViewModel.CompanyTypeId.ToString()),
                        new Claim(ApplicationSecurityConstants.CompanyName, userViewModel.CompanyName ?? ""),
                        new Claim(ApplicationSecurityConstants.CompanyLogoId, (userViewModel.CompanyLogoId ?? 0).ToString()),
                        new Claim(ApplicationSecurityConstants.SelectedProfile, "0"),
                        new Claim(ApplicationSecurityConstants.ApplicationCulture, "en-US"),
                        new Claim(ApplicationSecurityConstants.IsFirstLogin, userViewModel.IsFirstLogin.ToString()),
                        new Claim(ApplicationSecurityConstants.IsSalesCalculatorAllowed, userViewModel.IsSalesCalculatorAllowed.ToString()),
                        new Claim(ApplicationSecurityConstants.IsImpersonated, impersonation == CurrentUserImpersonationFlag.ImpersonationDone ? "false" : "true"),
                        new Claim(ApplicationSecurityConstants.ImpersonatedBy, impersonation == CurrentUserImpersonationFlag.ImpersonationDone ? "" : CurrentUser.Id.ToString()),
                        new Claim(ApplicationSecurityConstants.RememberMe, "false"),
                        new Claim(ApplicationSecurityConstants.SupplierURL, (userViewModel.SupplierURL ?? "").ToString()),
                        new Claim(ApplicationSecurityConstants.ButtonColor, (userViewModel.ButtonColor ?? "").ToString()),
                        new Claim(ApplicationSecurityConstants.BrandedCompanyId, userViewModel.BrandedCompanyId.ToString()),
                        new Claim(ApplicationSecurityConstants.ApplicationTemplateId, userViewModel.ApplicationTemplateId.ToString()),
                    };
                    SetCompanyType(userViewModel, claims);
                    foreach (var role in userViewModel.Roles)
                    {
                        claims.Add(new Claim(ApplicationSecurityConstants.UserRole, role.Id.ToString()));
                    }

                    var identity = new ClaimsIdentity(claims, ApplicationSecurityConstants.ApplicationCookie);

                    if (impersonation != CurrentUserImpersonationFlag.AlreadyImpersonated)
                    {
                        if (CurrentUser.IsImpersonated)
                        {
                            if (CurrentUser.Id <= 0)
                                LogManager.Logger.WriteDebug("ImpersonateUser", "AccountController", "End Impersonation [" + CurrentUser.Email + "," + userViewModel.Id + "," + CurrentUser.Id + "]");
                            authenticationDomain.UpdateImpersonationHistory(CurrentUser.Id, id);
                        }
                        else
                        {
                            if (CurrentUser.Id <= 0)
                                LogManager.Logger.WriteDebug("ImpersonateUser", "AccountController", "start Impersonation [" + CurrentUser.Email + "," + userViewModel.Id + "," + CurrentUser.Id + "]");
                            if (CurrentUser.Id > 0)
                                authenticationDomain.AddImpersonationHistory(id, CurrentUser.Id);
                        }
                    }

                    //add Impersonated USERS claims
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                }

                return Redirect(Url.Action("Index", "Home", new { area = "" }));
            }
        }

        private static void SetCompanyType(UserViewModel userViewModel, List<Claim> claims)
        {
            if (userViewModel.CompanyTypeId == (int)CompanyType.BuyerAndSupplier)
            {
                var roleIds = userViewModel.Roles.Select(t => t.Id).ToList();
                if (roleIds.Any(t => t == (int)UserRoles.Supplier) || roleIds.Any(t => t == (int)UserRoles.Driver) || roleIds.Any(t => t == (int)UserRoles.Dispatcher))
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Supplier).ToString()));
                }
                else
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Buyer).ToString()));
                }
            }
            else if (userViewModel.CompanyTypeId == (int)CompanyType.SupplierAndCarrier)
            {
                //Allow role like Driver to loading respective dashboard

                var roleIds = userViewModel.Roles.Select(t => t.Id).ToList();
                if (roleIds.Any(t => t == (int)UserRoles.Carrier))
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Carrier).ToString()));
                }
                else
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Supplier).ToString()));
                }
            }
            else if (userViewModel.CompanyTypeId == (int)CompanyType.BuyerSupplierAndCarrier)
            {
                var roleIds = userViewModel.Roles.Select(t => t.Id).ToList();
                if (roleIds.Any(t => t == (int)UserRoles.Supplier) || roleIds.Any(t => t == (int)UserRoles.Driver) || roleIds.Any(t => t == (int)UserRoles.Dispatcher))
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Supplier).ToString()));
                }
                else if (roleIds.Any(t => t == (int)UserRoles.Carrier))
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Carrier).ToString()));
                }
                else
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Buyer).ToString()));
                }
            }
        }

        public async Task<ActionResult> GetImpersonationTerminatedMessage()
        {
            using (var tracer = new Tracer("AccountController", "GetImpersonationTerminatedMessage"))
            {
                var response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().ValidateImpersonationAsync(CurrentUser.Id, CurrentUser.ImpersonatedBy);
                if (response.Length > 0)
                {
                    if (CurrentUser.ImpersonatedBy <= 0)
                        LogManager.Logger.CustomException("GetImpersonationTerminatedMessage", "AccountController", "start Impersonation To : " + CurrentUser.Email + "-Id :" + CurrentUser.Id, Json(CurrentUser));
                    await ImpersonateUser(CurrentUser.ImpersonatedBy, CurrentUserImpersonationFlag.ImpersonationDone);
                    DisplayCustomMessages(MessageType.Error, response);
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> ToggleUser(int id = 0, CurrentUserImpersonationFlag impersonation = CurrentUserImpersonationFlag.ImpersonationDone)
        {
            using (var tracer = new Tracer("AccountController", "ToggleUser"))
            {
                await ImpersonateUser(id == 0 ? CurrentUser.ImpersonatedBy : id, impersonation);
                return Redirect(Url.Action("Index", "Home", new { area = "" }));
            }
        }

        public ActionResult ForcefulLogout(bool isItForceful = false)
        {
            DisplayCustomMessages(MessageType.Warning, Resource.errMessageForcefulLogout);
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        public async Task<ActionResult> PunchoutStart(string token)
        {
            LogManager.Logger.WriteInfo("AccountController", "PunchoutStart", "Token=>" + token);

            var decodedToken = GetDecodedToken(token);
            var parameters = decodedToken.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
            var userViewModel = await domain.GetUserByTokenAsync(parameters[0]);
            if (userViewModel.StatusCode == AuthStatus.Success && parameters.Length == 3)
            {
                SetAuthenticationClaims(userViewModel, false, parameters[1], parameters[2]);
                domain.UpdateLastAccessedDate(userViewModel.Id);
                var userArea = userViewModel.CompanyTypeId == (int)CompanyType.Buyer ? "Buyer" : "Supplier";
                var targetUrl = Url.Action("View", "Invoice", new { area = userArea });
                return Redirect(targetUrl);
            }
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task StartPriceSync(string token)
        {
            using (var tracer = new Tracer("AccountController", "StartPriceSync"))
            {
                if (token == "6e8c99f7-0357-4d9b-b893-cab826f52f16")
                {
                    var domain = ContextFactory.Current.GetDomain<StoredProcedureDomain>();
                    await domain.OPISPlattsPricingSyncStatus(true, false);

                }
            }
        }

        [AuthorizeRole] // All roles are allowed
        public async Task SignOut(bool isChangePassword = false)
        {
            var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
            if (!string.IsNullOrWhiteSpace(CurrentUser.CxmlBuyerCookie) || isChangePassword)
            {
                await domain.RemoveAuthTokenAsync(CurrentUser.Id);
            }
            if (CurrentUser.IsImpersonated)
            {
                domain.UpdateImpersonationHistory(CurrentUser.Id, CurrentUser.ImpersonatedBy);
            }
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);            
            AuthenticationManager.SignOut(ApplicationSecurityConstants.ApplicationCookie);
        }

        private void SetAuthenticationClaims(UserViewModel userViewModel, bool rememberMe, string formPost = "", string externalCookie = "", bool isImpersonated = false, int impersonatedBy = 0)
        {
            var claims = new List<Claim>
            {
                new Claim(ApplicationSecurityConstants.UserId, userViewModel.Id.ToString()),
                new Claim(ApplicationSecurityConstants.UserName, userViewModel.FullName),
                new Claim(ApplicationSecurityConstants.Email, userViewModel.Email),
                new Claim(ApplicationSecurityConstants.CookieTimeStamp, DateTimeOffset.Now.ToString(ApplicationSecurityConstants.CookieTimeStampFormat, CultureInfo.InvariantCulture)),
                new Claim(ApplicationSecurityConstants.UserFingerPrint, userViewModel.FingerPrint),
                new Claim(ApplicationSecurityConstants.CompanyId, userViewModel.CompanyId.ToString()),
                new Claim(ApplicationSecurityConstants.CompanyTypeId, userViewModel.CompanyTypeId.ToString()),
                new Claim(ApplicationSecurityConstants.CompanyName, userViewModel.CompanyName ?? ""),
                new Claim(ApplicationSecurityConstants.CompanyLogoId, (userViewModel.CompanyLogoId ?? 0).ToString()),
                new Claim(ApplicationSecurityConstants.CompanyLogo, userViewModel.CompanyLogo),
                new Claim(ApplicationSecurityConstants.SelectedProfile, "0"),
                new Claim(ApplicationSecurityConstants.ApplicationCulture, "en-US"),
                new Claim(ApplicationSecurityConstants.IsFirstLogin, userViewModel.IsFirstLogin.ToString()),
                new Claim(ApplicationSecurityConstants.IsSalesCalculatorAllowed, userViewModel.IsSalesCalculatorAllowed.ToString()),
                new Claim(ApplicationSecurityConstants.IsImpersonated, isImpersonated.ToString()),
                new Claim(ApplicationSecurityConstants.ImpersonatedBy, impersonatedBy.ToString()),
                new Claim(ApplicationSecurityConstants.RememberMe, rememberMe.ToString()),
                new Claim(ApplicationSecurityConstants.SupplierURL, (userViewModel.SupplierURL ?? "").ToString()),
                new Claim(ApplicationSecurityConstants.ButtonColor, (userViewModel.ButtonColor ?? "").ToString()),
                new Claim(ApplicationSecurityConstants.BrandedCompanyId, userViewModel.BrandedCompanyId.ToString()),
                new Claim(ApplicationSecurityConstants.ApplicationTemplateId, userViewModel.ApplicationTemplateId.ToString()),
            };
            if (!string.IsNullOrWhiteSpace(formPost))
            {
                claims.Add(new Claim(ApplicationSecurityConstants.CxmlFormPost, formPost));
            }

            if (!string.IsNullOrWhiteSpace(externalCookie))
            {
                claims.Add(new Claim(ApplicationSecurityConstants.CxmlBuyerCookie, externalCookie));
            }

            if (userViewModel.CompanyTypeId == (int)CompanyType.BuyerAndSupplier)
            {
                var roleIds = userViewModel.Roles.Select(t => t.Id).ToList();
                if (roleIds.Any(t => t == (int)UserRoles.Supplier) || roleIds.Any(t => t == (int)UserRoles.Driver) || roleIds.Any(t => t == (int)UserRoles.Dispatcher))
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Supplier).ToString()));
                }
                else
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Buyer).ToString()));
                }
            }
            else if (userViewModel.CompanyTypeId == (int)CompanyType.SupplierAndCarrier)
            {
                //Allow role like Driver to loading respective dashboard

                var roleIds = userViewModel.Roles.Select(t => t.Id).ToList();
                if (roleIds.Any(t => t == (int)UserRoles.Carrier))
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Carrier).ToString()));
                }
                else
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Supplier).ToString()));
                }
            }
            else if (userViewModel.CompanyTypeId == (int)CompanyType.BuyerSupplierAndCarrier)
            {
                var roleIds = userViewModel.Roles.Select(t => t.Id).ToList();
                if (roleIds.Any(t => t == (int)UserRoles.Carrier))
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Carrier).ToString()));
                }
                else if (roleIds.Any(t => t == (int)UserRoles.Supplier) || roleIds.Any(t => t == (int)UserRoles.Driver) || roleIds.Any(t => t == (int)UserRoles.Dispatcher))
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Supplier).ToString()));
                }
                else
                {
                    claims.Remove(claims.First(t => t.Type == ApplicationSecurityConstants.SelectedProfile));
                    claims.Add(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Buyer).ToString()));
                }
            }


            foreach (var role in userViewModel.Roles)
            {
                claims.Add(new Claim(ApplicationSecurityConstants.UserRole, role.Id.ToString()));
            }

            var identity = new ClaimsIdentity(claims, ApplicationSecurityConstants.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = rememberMe }, identity);
        }

        private static string GetDecodedToken(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var bytes = Convert.FromBase64String(token);
                token = System.Text.Encoding.UTF8.GetString(bytes);
            }
            return token;
        }

        [HttpGet]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterConfirmation(string supplierURL)
        {
            using (var tracer = new Tracer("AccountController", "Register"))
            {
                RegisterViewModel viewModel = new RegisterViewModel();
                if (!string.IsNullOrEmpty(supplierURL))
                {
                    supplierURL = supplierURL.Trim().ToUpper();
                    var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                    var websiteLogoPath = domain.WebSiteConfigurationDetails(supplierURL, 0, 0);
                    if (!string.IsNullOrEmpty(websiteLogoPath.Item1) || !string.IsNullOrEmpty(websiteLogoPath.Item2))
                    {
                        ImageViewModel imageViewModel = new ImageViewModel();
                        imageViewModel.FilePath = websiteLogoPath.Item1;
                        viewModel.SupplierLogoPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                        imageViewModel.FilePath = websiteLogoPath.Item2;
                        viewModel.BackgroundImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                        viewModel.SupplierURL = supplierURL;
                        viewModel.ButtonColor = websiteLogoPath.Item4;
                        ViewBag.supplierURL = supplierURL;
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                    if (!string.IsNullOrEmpty(websiteLogoPath.Item5))
                    {
                        ImageViewModel imageViewModel = new ImageViewModel();
                        imageViewModel.FilePath = websiteLogoPath.Item5;
                        viewModel.FaviconImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    }
                }

                return View(viewModel);
            }
        }
        [HttpGet]
        public async Task<ActionResult> RegisterExternalCompanyUser(int id, string supplierURL)
        {
            using (var tracer = new Tracer("AccountController", "RegisterExternalCompanyUser"))
            {
                var viewModel = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetExternalUserInviteAsync(id);
                var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                if (viewModel.StatusCode != Status.Success)
                {
                    DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                    if (viewModel.Company != null)
                    {
                        //var websiteBrandDetails = domain.GetSupplierURLDetails(viewModel.Company.Id);
                        if (!string.IsNullOrEmpty(supplierURL))
                        {
                            return RedirectToAction("SupplierLogin", "Account", new { area = "", supplierURL = supplierURL });
                        }
                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    }
                    return RedirectToAction("Login", "Account");
                }

                if (viewModel.Company != null && viewModel.InvitedBy > 0)
                {
                    var websiteLogoPath = domain.WebSiteConfigurationDetails(supplierURL, string.IsNullOrEmpty(supplierURL) ? viewModel.Company.Id : 0, string.IsNullOrEmpty(supplierURL) ? viewModel.InvitedBy : 0);
                    if (!string.IsNullOrEmpty(websiteLogoPath.Item1) || !string.IsNullOrEmpty(websiteLogoPath.Item2) || !string.IsNullOrEmpty(websiteLogoPath.Item4))
                    {
                        ImageViewModel imageViewModel = new ImageViewModel();
                        imageViewModel.FilePath = websiteLogoPath.Item1;
                        viewModel.SupplierLogoPath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                        imageViewModel.FilePath = websiteLogoPath.Item2;
                        viewModel.BackgroundImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                        viewModel.ButtonColor = websiteLogoPath.Item4;
                        if (string.IsNullOrEmpty(supplierURL))
                        {
                            var supplierURLDetails = domain.GetSupplierURLDetails(viewModel.Company.Id);
                            viewModel.SupplierURL = supplierURLDetails;
                            ViewBag.supplierURL = supplierURLDetails;
                        }
                        else
                        {
                            viewModel.SupplierURL = supplierURL;
                            ViewBag.supplierURL = supplierURL;
                        }
                    }
                    if (!string.IsNullOrEmpty(websiteLogoPath.Item5))
                    {
                        ImageViewModel imageViewModel = new ImageViewModel();
                        imageViewModel.FilePath = websiteLogoPath.Item5;
                        viewModel.FaviconImagePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                    }
                }
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterExternalCompanyUser(RegisterAdditionalUserViewModel viewModel)
        {
            var response = new StatusViewModel();
            response.StatusMessage = Resource.errMessageRegisterCompanyUserFailed;
            using (var tracer = new Tracer("AccountController", "RegisterExternalCompanyUser(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().OnboardExternalCompanyUserAsync(viewModel);
                    if (response.StatusCode == Status.Success)
                    {
                        if (viewModel.Company != null)
                        {   
                            //var websiteBrandDetails = domain.GetSupplierURLDetails(viewModel.Company.Id);
                            if (!string.IsNullOrEmpty(viewModel.SupplierURL))
                            {
                                return RedirectToAction("SupplierLogin", "Account", new { area = "", supplierURL = viewModel.SupplierURL });
                            }
                            else
                            {
                                return RedirectToAction("Login", "Account");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }

                    }
                    DisplayCustomMessages((MessageType)Status.Failed, response.StatusMessage);
                }
                return View(viewModel);
            }
        }

    }
    
}