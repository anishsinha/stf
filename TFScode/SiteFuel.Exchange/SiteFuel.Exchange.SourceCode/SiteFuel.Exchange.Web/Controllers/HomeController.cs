using Microsoft.Owin.Security;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using SiteFuel.Exchange.Core.Logger;

namespace SiteFuel.Exchange.Web.Controllers
{
    [AuthorizeRole]
    public class HomeController : BaseController
    {
        private List<BreadcrumbMappingViewModel> breadcrumbData;

        public async Task<ActionResult> Index()
        {
            if (!CurrentUser.IsImpersonated)
            {
                int impersonatedUserId = ContextFactory.Current.GetDomain<AuthenticationDomain>().GetImpersonatedUserId(CurrentUser.Id);
                if (impersonatedUserId > 0)
                {
                    return RedirectToAction("ToggleUser", "Account", new { area = "", id = impersonatedUserId, impersonation = CurrentUserImpersonationFlag.AlreadyImpersonated });
                }
            }
            else
            {
                int impersonatedById = ContextFactory.Current.GetDomain<AuthenticationDomain>().GetImpersonatedByUserId(CurrentUser.Id);
                if (impersonatedById > 0)
                {
                    return RedirectToAction("ToggleUser", "Account", new { area = "", id = impersonatedById, impersonation = CurrentUserImpersonationFlag.ImpersonationDone });
                }
            }

            if (!CurrentUser.IsImpersonated)
            {
                var isOnboardingComplete = await ContextFactory.Current.GetDomain<HelperDomain>().IsOnboardingComplete(CurrentUser.Id);
                if (!isOnboardingComplete)
                {
                    return RedirectToAction("Company", "Onboarding", new { area = "" });
                }
            }

            if (!CurrentUser.IsImpersonated && CurrentUser.IsFirstLogin)
            {
                return RedirectToAction("ChangePassword", "Account", new { area = "" });
            }

            if (CurrentUser.IsBuyerCompany)
            {
               // return RedirectToAction("Index", "Dashboard", new { area = "Buyer" });
                return RedirectToAction("DashboardNew", "Dashboard", new { area = "Buyer" });
            }
            else if (CurrentUser.IsSupplierCompany)
            {
                if (CurrentUser.IsDriver)
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Driver" });
                }
                else if (CurrentUser.IsDispatcher)
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Dispatcher" });
                }
                else if (CurrentUser.IsSalesUser)
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "SalesUser" });
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Supplier" });
                }
            }
            else if (CurrentUser.IsCarrierCompany)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Dispatcher" });
            }
            else
            {
                if (CurrentUser.IsSuperAdmin || CurrentUser.IsAccountSpecialist)
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "SuperAdmin" });
                }
                else if (CurrentUser.IsInternalSalesPerson)
                {
                    return RedirectToAction("Calculator", "InternalSales", new { area = "Sales" });
                }
                else
                {
                    return RedirectToAction("Contact", "Home", new { area = "" });
                }
            }
        }

        public ActionResult SetCulture(string culture)
        {
            culture = CultureHelperMethods.GetImplementedCulture(culture);

            var Identity = new ClaimsIdentity(User.Identity);
            Identity.RemoveClaim(Identity.FindFirst(ApplicationSecurityConstants.ApplicationCulture));
            Identity.AddClaim(new Claim(ApplicationSecurityConstants.ApplicationCulture, culture));
            AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                                                                    new ClaimsPrincipal(Identity),
                                                                    new AuthenticationProperties
                                                                    {
                                                                        IsPersistent = true
                                                                    });

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Contact()
        {
            using (var tracer = new Tracer("HomeController", "Contact"))
            {
                var user = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserByIdAsync(CurrentUser.Id);

                var viewModel = new SupportViewModel
                {
                    Id = user.Id,
                    IsContactByEmail = true,
                    Email = user.Email,
                    IsContactByPhoneNumber = true,
                    PhoneNumber = user.PhoneNumber
                };
                var supportContactNumber = ContextFactory.Current.GetDomain<MasterDomain>().GetApplicationSettingValue(Constants.SupportContactNumber);
                viewModel.SupportContactNumber = supportContactNumber;
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(SupportViewModel viewModel)
        {
            using (var tracer = new Tracer("HomeController", "Contact(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var serverUrl = ContextFactory.Current.GetDomain<HelperDomain>().GetServerUrl();
                        var notification = ContextFactory.Current.GetDomain<NotificationDomain>().GetNotificationContent(EventSubType.ContactUs, serverUrl, string.Empty);
                        var emailTemplate = ContextFactory.Current.GetDomain<HelperDomain>().GetApplicationEventNotificationTemplate();
                        var emailList = ContextFactory.Current.GetDomain<ApplicationDomain>().GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingContactUsMailingList);
                        var contactBy = string.Empty;
                        if (viewModel.IsContactByEmail)
                        {
                            contactBy = $"Email : {viewModel.Email}";
                        }
                        if (viewModel.IsContactByPhoneNumber)
                        {
                            if (string.IsNullOrWhiteSpace(contactBy))
                            {
                                contactBy = $"Phone Number : {viewModel.PhoneNumber}";
                            }
                            else
                            {
                                contactBy = $"{contactBy} <br>Phone Number : {viewModel.PhoneNumber}";
                            }
                        }
                        var emailModel = new ApplicationEventNotificationViewModel
                        {
                            To = emailList.Split(';').ToList(),
                            Subject = notification.Subject,
                            CompanyLogo = notification.CompanyLogo,
                            CompanyText = notification.CompanyText,
                            BodyLogo = notification.BodyLogo,
                            BodyText = string.Format(notification.BodyText,
                                                viewModel.Subject,
                                                viewModel.Question,
                                                contactBy),
                            BodyButtonText = notification.BodyButtonText,
                            BodyButtonUrl = notification.BodyButtonUrl
                        };
                        await ContextFactory.Current.GetDomain<EmailDomain>().SendEmail(emailTemplate, emailModel);

                        //Display success message
                        DisplayCustomMessages(MessageType.Success, Resource.errMessageContactUsSuccess);
                    }
                    catch (Exception ex)
                    {
                        DisplayCustomMessages(MessageType.Error, Resource.errMessageContactUsFailed);
                        LogManager.Logger.WriteException("HomeController", "Contact(viewModel)", ex.Message, ex);
                    }
                }
                return RedirectToAction("Index", "Home", new { area = "" }); ;
            }
        }

        public ActionResult GetBreadcrumbMapping()
        {
            if (breadcrumbData == null)
            {
                string breadCrumbMapping = string.Empty;
                var filePath = Server.MapPath("~\\Content\\BreadcrumbMap.json");
                using (StreamReader file = System.IO.File.OpenText(filePath))
                {
                    breadCrumbMapping = file.ReadToEnd();
                    
                }
                breadcrumbData = JsonConvert.DeserializeObject<List<BreadcrumbMappingViewModel>>(breadCrumbMapping);
            }
            return new JsonResult
            {
                Data = breadcrumbData,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult TPDAPIDashboard()
        {
            using (var tracer = new Tracer("HomeController", "TPDAPIDashboard"))
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetApiLogs(int companyId,string fromDate,string toDate,string apiName,int viewType=(int) APIResultType.Total)
        {
            using (var tracer = new Tracer("HomeController", "GetApiLogs"))
            {
                ApiLogViewModel model = new ApiLogViewModel();
                model.CompanyId = companyId;
                if(!String.IsNullOrEmpty(fromDate))
                model.FromDate = Convert.ToDateTime(fromDate);
                if (!String.IsNullOrEmpty(toDate))
                    model.ToDate = Convert.ToDateTime(toDate) ;
                if (!string.IsNullOrEmpty(apiName))
                    model.Url = apiName;
                model.Message = viewType.ToString();
                var response = await ContextFactory.Current.GetDomain<ExceptionLogDomain>().GetApiLogs(model);
                //return Json(response, JsonRequestBehavior.AllowGet); ;
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }


        [HttpGet]
        public async Task<JsonResult> GetApiLogRequestResponse(int id ,string ReqResType)
        {
            using (var tracer = new Tracer("HomeController", "GetApiLogRequestResponse"))
            {
                ApiLogViewModel model = new ApiLogViewModel();
                model.Id = id;
                model.ReqResType = Convert.ToInt32(ReqResType);
                var response = await ContextFactory.Current.GetDomain<ExceptionLogDomain>().GetApiLogRequestResponse(model,UserContext);
                return Json(response, JsonRequestBehavior.AllowGet); ;
            }
        }


        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            //if (this is HomeController)
            CheckIsOktaRequest();
            
            return base.BeginExecuteCore(callback, state);
        }
    }
}