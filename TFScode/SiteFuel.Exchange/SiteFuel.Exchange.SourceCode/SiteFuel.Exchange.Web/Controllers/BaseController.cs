using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Domain.Services;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.DispatchScheduler;
using SiteFuel.Exchange.ViewModels.Forcasting;
using SiteFuel.Exchange.ViewModels.RouteInfo;
using SiteFuel.Exchange.Web.Areas.Dispatcher.Models;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Common;
using SiteFuel.Exchange.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Web.UI;
using static SiteFuel.Exchange.Web.Helpers.CommonHelperMethods;

namespace SiteFuel.Exchange.Web.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public abstract class BaseController : Controller
    {
        public static bool IsOktaSignOut = false;
        protected override ITempDataProvider CreateTempDataProvider()
        {
            return new CookieTempDataProvider();
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;
            string defaultCultureName = "en-US";
            //Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies[ApplicationConstants.CurrentCulture];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
            }
            else
            {
                //Obtain it from HTTP header AcceptLanguages
                cultureName = (Request.UserLanguages != null && Request.UserLanguages.Length > 0)
                                    ? Request.UserLanguages[0]
                                    : null;
            }
            if (cultureName != defaultCultureName)
            {
                cultureName = defaultCultureName;
            }
            //Validate culture name
            cultureName = CultureHelperMethods.GetImplementedCulture(cultureName);

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

        protected void CheckIsOktaRequest()
        {
            #region Okta implementation 
            //below code block is to make TFX log-ed out in case any user already log-ed in with okta and not impact normal log-ed in i.e form authentication 
            if ((HttpContext.Request.UrlReferrer == null || !HttpContext.Request.UrlReferrer.AbsoluteUri.Contains(ApplicationConstants.IdpOkta)) && IsOktaSignOut)
            {
                IsOktaSignOut = false;
            }
            if (!IsOktaSignOut && HttpContext.Request.UrlReferrer != null && HttpContext.Request.UrlReferrer.AbsoluteUri.Contains(ApplicationConstants.IdpOkta))
            {
                var response = Task.Run(() => LogoutOkta()).Result;
                IsOktaSignOut = true;

            }
            #endregion
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string lastAccessDate = string.Empty;
            try
            {

                if (CurrentUser.IsImpersonated && filterContext.RequestContext.HttpContext.Request.HttpMethod == "POST")
                {
                    var parameters = filterContext.ActionParameters.Where(x => !(x.Value is HttpPostedFileBase)).ToList();

                    var formData = filterContext.ActionParameters.Count > 0 ? JsonConvert.SerializeObject(parameters) : "";
                    string cotroller = filterContext.RouteData.Values["controller"].ToString();
                    string action = filterContext.RouteData.Values["action"].ToString();
                    Core.ContextFactory.Current.GetDomain<HelperDomain>().LogImpersonationActivity(CurrentUser.Id, CurrentUser.ImpersonatedBy, cotroller, action, formData, "Start");
                }

                if (filterContext.HttpContext.Request.IsAjaxRequest() && Request.Headers["x-culture-header"] != null)
                {
                    var cultureName = Request.Headers["x-culture-header"].ToString();
                    if (!cultureName.Equals(string.Empty))
                    {
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
                    }
                }



                if (HttpContext.Request.Cookies.AllKeys.Contains(ApplicationConstants.LastAccessedDate) && (HttpContext.Request.UrlReferrer == null || !HttpContext.Request.UrlReferrer.AbsoluteUri.Contains(ApplicationConstants.IdpOkta)))  // Company name - dev-27245004.okta.com, for localhost
                {
                    lastAccessDate = HttpContext.Request.Cookies[ApplicationConstants.LastAccessedDate].Value;
                    if (!string.IsNullOrWhiteSpace(lastAccessDate) && (DateTime.Now - Convert.ToDateTime(lastAccessDate)).TotalMinutes >= 60 && CurrentUser.Id != 0)
                    {
                        ContextFactory.Current.GetDomain<AuthenticationDomain>().UpdateLastAccessedDate(CurrentUser.Id);
                        SetCookie();
                    }
                }
                else
                {
                    SetCookie();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseController", "OnActionExecuting", ex.Message + lastAccessDate, ex);
            }
            base.OnActionExecuting(filterContext);
        }

        protected void SetCookie()
        {
            HttpCookie lastAccessDateCookies = new HttpCookie(ApplicationConstants.LastAccessedDate);
            lastAccessDateCookies.Value = Convert.ToString(DateTime.Now);
            Response.Cookies.Add(lastAccessDateCookies);
        }


        public JsonResult ValidateFuelType(int? JobId, int FuelTypeId, bool RequestCheck, int pricingSourceId = 0)
        {
            using (var tracer = new Tracer("BaseController", "ValidateFuelType"))
            {

                var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().ValidateFuelType(JobId, FuelTypeId, RequestCheck, pricingSourceId);
                if (response.StatusCode == Status.Warning)
                {
                    response.StatusCode = Status.Failed;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                return Json(JsonRequestBehavior.AllowGet);
            }

        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                if (CurrentUser.IsImpersonated && filterContext.RequestContext.HttpContext.Request.HttpMethod == "POST")
                {
                    var formData = JsonConvert.SerializeObject(filterContext.Result);
                    string cotroller = filterContext.RouteData.Values["controller"].ToString();
                    string action = filterContext.RouteData.Values["action"].ToString();
                    Core.ContextFactory.Current.GetDomain<HelperDomain>().LogImpersonationActivity(CurrentUser.Id, CurrentUser.ImpersonatedBy, cotroller, action, formData, "End");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseController", "OnActionExecuting", ex.Message, ex);

            }
            base.OnActionExecuted(filterContext);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();
            }
            base.OnResultExecuting(filterContext);
        }


        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var controllerName = filterContext.RouteData.Values[ApplicationConstants.Controller].ToString();
            var actionName = filterContext.RouteData.Values[ApplicationConstants.Action].ToString();
            //Log the error!!
            LogManager.Logger.WriteException(controllerName, actionName, filterContext.Exception.Message, filterContext.Exception);

            if (filterContext.Exception is UnauthorizedAccessException)
                filterContext.Result = RedirectToAction("Index", "Unauthorized", new { Area = "" });
            else
                //Redirect or return a view, but not both.
                filterContext.Result = RedirectToAction("GenericError", "Error", new { Area = "" });
        }

        protected string RenderRazorViewToString<T>(string viewName, T model) where T : class
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected void DisplayCustomMessages(MessageType type, string message, string linkText, string linkUrl)
        {
            TempData[ApplicationConstants.CustomMessageType] = type;
            TempData[ApplicationConstants.CustomMessage] = new string[] { message };
            TempData[ApplicationConstants.CustomMessageLinkText] = linkText;
            TempData[ApplicationConstants.CustomMessageLink] = linkUrl;
        }

        protected void DisplayCustomMessages(MessageType type, string message)
        {
            TempData[ApplicationConstants.CustomMessageType] = type;
            TempData[ApplicationConstants.CustomMessage] = new string[] { message };
        }

        protected TimeSpan GetBrowserTimeZoneOffset()
        {
            int offsetInt = 0;
            if (HttpContext.Request.Cookies.AllKeys.Contains(ApplicationConstants.TimeZoneOffset))
            {
                var offset = HttpContext.Request.Cookies[ApplicationConstants.TimeZoneOffset].Value;
                Int32.TryParse(offset, out offsetInt);
            }
            return new TimeSpan(0, offsetInt, 0);
        }

        protected void DisplayCustomMessages(MessageType type, List<string> messages)
        {
            if (messages.Count == 0)
            {
                return;
            }
            TempData[ApplicationConstants.CustomMessageType] = type;
            TempData[ApplicationConstants.CustomMessage] = messages.ToArray();
        }

        protected ApplicationUser CurrentUser
        {
            get
            {
                return new ApplicationUser(this.User as ClaimsPrincipal);
            }
        }

        protected UserContext UserContext
        {
            get
            {
                var userContext = new UserContext(CurrentUser.Id, CurrentUser.Name, CurrentUser.UserName, CurrentUser.Email,
                                    CurrentUser.CompanyId, CurrentUser.CompanyName, CurrentUser.CompanyTypeId, CurrentUser.CompanySubTypeId,
                                    CurrentUser.Roles, CurrentUser.IsFirstLogin, CurrentUser.IsSalesCalculatorAllowed, CurrentUser.IsImpersonated,
                                    CurrentUser.ImpersonatedBy, CurrentUser.CxmlFormPost, CurrentUser.CxmlBuyerCookie, CurrentUser.ApplicationTemplateId,
                                    CurrentUser.BrandedCompanyId);
                var currentArea = ControllerContext.RouteData.DataTokens["area"];
                if (currentArea != null)
                    userContext.ActingCompanyType = currentArea.ToString();
                return userContext;
            }
        }

        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home", new { area = "" });
            }

            return returnUrl;
        }

        protected bool IsReturnUrlExist()
        {
            return TempData[ApplicationConstants.ReturnUrl] != null;
        }

        protected void SetReturnUrl(string returnUrl)
        {
            TempData[ApplicationConstants.ReturnUrl] = returnUrl;
        }

        protected string GetReturnUrl()
        {
            string returnUrl = string.Empty;
            if (IsReturnUrlExist())
            {
                returnUrl = TempData[ApplicationConstants.ReturnUrl] as string;
            }
            return returnUrl;
        }

        protected void RemoveReturnUrl()
        {
            if (IsReturnUrlExist())
            {
                TempData.Remove(ApplicationConstants.ReturnUrl);
            }
        }

        public async Task<JsonResult> GetImage(int imageId)
        {
            var response = await Core.ContextFactory.Current.GetDomain<Domain.HelperDomain>().GetImage(imageId);
            var image = Convert.ToBase64String(response.Data);
            return Json(image, JsonRequestBehavior.AllowGet);
        }

        protected async Task<ImageViewModel> SetImageDataToBolb(ImageViewModel imageViewModel, HttpPostedFileBase[] imageFiles, BlobContainerType container, BlobContainerType subContainerName = BlobContainerType.None)
        {
            if (imageFiles != null && imageFiles[0] != null && imageFiles.Length > 0)
            {
                if (imageViewModel == null)
                    imageViewModel = new ImageViewModel();
                if (imageFiles.Length > 1)
                    PdfHelper.Instance.SetStreamInImageModel(imageFiles, UserContext.Id, imageViewModel);
                else
                {
                    var image = imageFiles[0];
                    var fileName = Path.GetFileName(image.FileName);
                    imageViewModel.InputStream = image.InputStream;
                    imageViewModel.Name = fileName;
                }
                var subcontainer = "";
                if (subContainerName != BlobContainerType.None)
                {
                    subcontainer = subContainerName.GetDisplayName().ToLower();
                    imageViewModel.Name = subcontainer + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "/" + imageViewModel.Name;
                }
                await UploadFileToAzure(imageViewModel, container);
            }
            return imageViewModel;
        }
        public void SendCreditAppNotification(FuelRequestStatusViewModel response)
        {
            var notification = Core.ContextFactory.Current.GetDomain<NotificationDomain>().GetCreditAppNotificationDetails(CurrentUser.CompanyId, response.ToUser);
            var emailTemplate = Core.ContextFactory.Current.GetDomain<HelperDomain>().GetApplicationEventNotificationTemplate();
            var emailModel = new ApplicationEventNotificationViewModel
            {
                From = notification.From,
                To = new List<string> { response.ToUserEmail },
                Subject = notification.Subject,
                CompanyLogo = notification.CompanyLogo,
                BodyText = notification.Body,
                Attachments = notification.Attachments,
                ShowFooterContent = false,
                ShowHelpLineInfo = false
            };
            Email.GetClient().SendAsync(emailTemplate, emailModel);
        }

        protected async Task<StatusViewModel> UploadFileToAzure(ImageViewModel viewModel, BlobContainerType containerType)
        {
            StatusViewModel response = new StatusViewModel(Status.Success);
            //var bulkDomain = ContextFactory.Current.GetDomain<InvoiceDomain>();
            if (viewModel != null)
            {
                response = await AzureStorageService.UploadImageToBlob(UserContext, viewModel.InputStream, viewModel.Name, containerType);
                //response = await bulkDomain.UploadImageToBlob(UserContext, viewModel.InputStream, viewModel.Name, containerType);
                if (response.StatusCode == Status.Success)
                    viewModel.FilePath = response.StatusMessage;
            }
            return response;
        }

        [HttpGet]
        public string GetEncryptedData(string inputUrl)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(inputUrl));
        }

        [HttpGet]
        public async Task<JsonResult> GetProductListByZip(string zipCode, decimal radius = 100, PricingSource source = PricingSource.Axxis)
        {
            using (var tracer = new Tracer("BaseController", "GetProductList"))
            {
                var response = await ContextFactory.Current.GetDomain<HelperDomain>().GetProductsByZip(zipCode, radius, source, CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetPricingCodes(int PricingTypeId = 0, int PricingSourceId = (int)PricingSource.Axxis, string Prefix = null, int feedTypeId = 0, int fuelClassTypeId = 0, int tfxProdId = 0)
        {
            PricingCodesRequestViewModel request = new PricingCodesRequestViewModel { PricingTypeId = PricingTypeId, PricingSourceId = PricingSourceId, Search = Prefix, FeedTypeId = feedTypeId, FuelClassTypeId = fuelClassTypeId, TFxProductId = tfxProdId };
            var codes = await ContextFactory.Current.GetDomain<PricingServiceDomain>().GetPricingCodesAsync(request);
            return Json(codes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCityGroupTerminals(int stateId, bool allStates = false, int selectedCityRackId = 0, PricingSource sourceId = PricingSource.Axxis)
        {
            using (var tracer = new Tracer("BaseController", "GetCityGroupTerminals"))
            {
                var pricingDomain = ContextFactory.Current.GetDomain<ExternalPricingDomain>();
                var response = pricingDomain.GetCityGroupTerminalsByStateId(stateId, allStates, selectedCityRackId, sourceId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> IsCityGroupTerminalPriceAvailable(int jobId, int? fueltypeId, int? selectedCityRackId, float? lattitude, float? longitude, string countryCode, PricingSource sourceId = PricingSource.Axxis)
        {
            using (var tracer = new Tracer("BaseController", "GetProductList"))
            {
                var domain = ContextFactory.Current.GetDomain<PricingServiceDomain>();
                var productId = ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelTypeId(fueltypeId ?? 0, (int)sourceId, (int)PricingType.RackHigh);

                if (sourceId == PricingSource.Axxis && productId > 0)
                {
                    productId = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetExternalProductId(productId);
                }

                var response = await domain.IsCityRackPriceAvailable(productId, selectedCityRackId ?? 0, sourceId, DateTimeOffset.UtcNow.DateTime);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetDemandCaptureChartData(string SiteId, int noOfDays, int tfxJobId)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDemandCaptureChartData(SiteId, noOfDays, tfxJobId, 0)); // 0 to display all data chart to buyer
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetDipTestDetails(string siteId, string tankId, int noOfDays)
        {
            var result = new ResponseMessage();
            var response = await ContextFactory.Current.GetDomain<DispatcherDomain>().GetDipTestDetails(siteId, tankId, noOfDays);
            if (response.Count() > 0)
                result.StatusCode = HttpStatusCode.Found;
            else
                result.StatusCode = HttpStatusCode.NotFound;
            result.Data = response.ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RetryFailedQbRequest(long Id)
        {
            using (var tracer = new Tracer("BaseController", "RetryFailedQbRequest"))
            {
                var response = ContextFactory.Current.GetDomain<QbDomain>().RetryQbRequests(Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetTfxProduct(int tfxProductId)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetTfxProduct(tfxProductId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetQbRequestXml(int Id)
        {
            using (var tracer = new Tracer("QuickBooksController", "GetQbRequestXml"))
            {
                var qbDomain = ContextFactory.Current.GetDomain<QbDomain>();
                var response = qbDomain.GetQbRequestXml(Id);
                return new JsonResult()
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public JsonResult GetQbResponseXml(int Id)
        {
            using (var tracer = new Tracer("QuickBooksController", "GetQbResponseXml"))
            {
                var qbDomain = ContextFactory.Current.GetDomain<QbDomain>();
                var response = qbDomain.GetQbResponseXml(Id);
                return new JsonResult()
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetNextDeliverySchedules(int orderId)
        {
            using (var tracer = new Tracer("OrderController", "GetNextDeliverySchedules"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetNextDeliveryScheduleDetails(orderId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetCurrencySpecificHeaders()
        {
            using (var tracer = new Tracer("BaseController", "GetCurrencySpecificHeaders"))
            {
                var currencyBasedHeaders = new Dictionary<string, string>();
                var cultures = new string[] { "en-US", "en-CA" };

                for (int cntCult = 0; cntCult < cultures.Length; ++cntCult)
                {
                    var currency = Enum.GetName(typeof(Currency), cntCult + 1).ToLower();
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultures[cntCult]);
                    currencyBasedHeaders.Add("qty_req_" + currency, Resource.gridColumnGallonsRequested);
                    currencyBasedHeaders.Add("rack_ppg_" + currency, string.Format("{0} ({1})", Resource.lblRackPPG, Resource.lblCurrencyCode));
                    currencyBasedHeaders.Add("total_amount_" + currency, string.Format("{0} ({1})", Resource.lblTotalAmount, Resource.lblCurrencyCode));
                    currencyBasedHeaders.Add("total_invoice_amount_" + currency, string.Format("{0} ({1})", Resource.gridColumnInvoiceAmount, Resource.lblCurrencyCode));
                    currencyBasedHeaders.Add("qty_delivered_" + currency, Resource.gridColumnGallonsDelivered);
                    currencyBasedHeaders.Add("qty_scheduled_" + currency, Resource.gridColumnGallonsScheduled);
                    currencyBasedHeaders.Add("rack_paid_" + currency, string.Format("{0} ({1})", Resource.gridColumnRackPPGPaid, Resource.lblCurrencyCode));
                    currencyBasedHeaders.Add("rack_sold_" + currency, string.Format("{0} ({1})", Resource.gridColumnRackPPGSold, Resource.lblCurrencyCode));
                    currencyBasedHeaders.Add("qty_brokered_" + currency, Resource.gridColumnGallonsBrokered);
                    currencyBasedHeaders.Add("rack_brokered_" + currency, string.Format("{0} ({1})", Resource.gridColumnBrokerRackPPG, Resource.lblCurrencyCode));
                    currencyBasedHeaders.Add("budget_" + currency, string.Format("{0} ({1})", Resource.gridColumnBudget, Resource.lblCurrencyCode));
                    currencyBasedHeaders.Add("total_spend_" + currency, string.Format("{0} ({1})", Resource.gridColumnTotalSpend, Resource.lblCurrencyCode));
                }
                return Json(currencyBasedHeaders, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 3600, VaryByParam = "id", Location = OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> DownloadImage(int id = 0)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetImage(id);
            if (response != null)
            {
                var fileName = string.Format("{0}.png", response.Item1);
                return File(response.Item2, "image/png", fileName);
            }
            return new EmptyResult();
        }

        [HttpGet]
        public async Task<ActionResult> BaseballCardDetails(int supplierCompanyId)
        {
            using (var tracer = new Tracer("BaseController", "BaseballCardDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetBaseballCardDetailsAsync(UserContext.CompanyId, supplierCompanyId);
                return PartialView("_PartialBaseballCardDetails", response);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetBlendProductTypeMapping()
        {
            var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetBlendProductTypeMappingGridAsync();
            return new JsonResult
            {
                Data = response,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult OfferFeeDetails(int OfferPricingId)
        {
            var fuelDeliveryDetails = ContextFactory.Current.GetDomain<OfferDomain>().GetOfferFeesAsync(UserContext, OfferPricingId);
            return PartialView("~/Views/Shared/_PartialOfferFeeDetails.cshtml", fuelDeliveryDetails);
        }

        [HttpGet]
        public async Task<JsonResult> GetZipCodeOfCity(string stateCode, string city)
        {
            var response = await ContextFactory.Current.GetDomain<ZipCodeServiceDomain>().GetZipCodeList(stateCode, city);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCitesOfState(int stateId)
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetCities(stateId, false);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMultipleCites(string stateId, bool withAllOption = false)
        {
            var stateIds = stateId.Split(',').Select(Int32.Parse).ToList();
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetCities(stateIds, withAllOption);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetMultipleZipCodes(string cityId)
        {
            var cityIds = cityId.Split(',').Select(Int32.Parse).ToList();
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetZipCodes(cityIds);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetZipCodes(string stateCode, string city)
        {
            var response = await ContextFactory.Current.GetDomain<ZipCodeServiceDomain>().GetZipCodes(stateCode, city);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OfferPricingDetails(int OfferPricingId)
        {
            var fuelDeliveryDetails = ContextFactory.Current.GetDomain<OfferDomain>().GetOfferPricingAsync(UserContext, OfferPricingId);
            return PartialView("~/Views/Shared/_PartialOfferPricingDetails.cshtml", fuelDeliveryDetails);
        }

        [HttpPost]
        public async Task<ActionResult> InvoiceReportGrid(InvoiceReportFilter requestModel)
        {
            using (var tracer = new Tracer("ReportController", "InvoiceReportGrid"))
            {
                var response = new List<InvoiceReportViewModel>();
                var dataTableSearchModel = new DataTableSearchModel(requestModel);
                var totalCount = 0;


                if (!string.IsNullOrEmpty(requestModel.StartDate) && !string.IsNullOrEmpty(requestModel.EndDate))
                {
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetInvoiceReportAsync(requestModel, dataTableSearchModel);

                    if (response.Count > 0)
                        totalCount = response[0].TotalCount;

                }
                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = requestModel.draw,
                        data = response,
                        recordsTotal = totalCount,
                        recordsFiltered = totalCount

                    },

                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public async Task<ActionResult> ExportInvoiceReportToCsv(string startDate, string endDate,
                string jobIds, string customerCompanyIds, string supplierCompanyIds, int companyProfile, int currencyType = 0)
        {
            using (var tracer = new Tracer("ReportController", "ExportInvoiceReportToCsv"))
            {
                var lstjobIds = new List<int>();
                var lstcustomerCompanyIds = new List<int>();
                var lstsupplierCompanyIds = new List<int>();

                if (!string.IsNullOrEmpty(jobIds) && jobIds.ToLower() != "undefined")
                    lstjobIds = jobIds.Split(',').Select(x => Convert.ToInt32(x)).Distinct().ToList();

                if (!string.IsNullOrEmpty(customerCompanyIds) && customerCompanyIds.ToLower() != "undefined")
                    lstcustomerCompanyIds = customerCompanyIds.Split(',').Select(x => Convert.ToInt32(x)).Distinct().ToList();

                if (!string.IsNullOrEmpty(supplierCompanyIds) && supplierCompanyIds.ToLower() != "undefined")
                    lstsupplierCompanyIds = supplierCompanyIds.Split(',').Select(x => Convert.ToInt32(x)).Distinct().ToList();

                InvoiceReportFilter requestModel = new InvoiceReportFilter();
                requestModel.StartDate = startDate;
                requestModel.EndDate = endDate;
                requestModel.CompanyId = CurrentUser.CompanyId;
                requestModel.CompanyProfile = companyProfile;
                requestModel.CustomerCompanyIds = lstcustomerCompanyIds;
                requestModel.SupplierCompanyIds = lstsupplierCompanyIds;
                requestModel.JobIds = lstjobIds;

                var result = await ContextFactory.Current.GetDomain<ReportDomain>().GetInvoiceReportToExport(requestModel);
                var csv = new SuperAdminDomain().ListToCSV<InvoiceReportExportViewModel>(result, currencyType);
                var fileName = "invoice-report-" + DateTimeOffset.UtcNow.ToString("MM/dd/yyyy HH:mm tt") + ".csv";
                return File(new System.Text.UTF8Encoding().GetBytes(csv), Core.Utilities.MediaType.Text, fileName);
            }
        }

        [HttpPost]
        public async Task SaveUserGridConfiguration(UserGridConfigurationViewModel viewModel)
        {
            using (var tracer = new Tracer("BaseController", "SaveUserGridConfiguration"))
            {
                viewModel.UserId = CurrentUser.Id;
                if (viewModel.GridId > 0)
                    await ContextFactory.Current.GetDomain<GridConfigurationDomain>().SaveUserGridConfiguration(viewModel);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetUserGridConfigurationAsync(GridName gridId)
        {
            using (var tracer = new Tracer("BaseController", "GetUserGridConfigurationAsync"))
            {
                var response = await ContextFactory.Current.GetDomain<GridConfigurationDomain>().GetUserGridConfigurationAsync(CurrentUser.Id, gridId);
                return Json(response.Setting, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SendEmailInvoiceReport(InvoiceReportFilter filter)
        {
            using (var tracer = new Tracer("BaseController", "SendEmailInvoiceReport"))
            {
                var jsonMessage = new JavaScriptSerializer().Serialize(filter);
                await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.InvoiceReportGenerated, filter.CompanyId, filter.UserId, null, jsonMessage);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult QuickBookReport()
        {
            return PartialView("_PartialQuickbooksReport");
        }
        [HttpGet]
        public ActionResult QuickBookSummary(int workflowId)
        {
            return PartialView("_PartialQuickbooksSummary", workflowId);
        }

        //[HttpGet]
        //public async Task<JsonResult> GetWebNotifications(int currentId)
        //{
        //    var response = await ContextFactory.Current.GetDomain<WebNotificationDomain>().GetWebNotifications(CurrentUser.Id, currentId);
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public async Task<JsonResult> SetWebNotificationsAsRead(int notificationType)
        //{
        //    var response = await ContextFactory.Current.GetDomain<WebNotificationDomain>().SetWebNotificationsAsRead(CurrentUser.Id, notificationType);
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetFTLFees()
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetFTLFees();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWaiverApplicableFees()
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetWaiverApplicableFees();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DeliveryCompletedGridAsync(InvoiceFilterViewModel invoiceFilter = null, ViewInvoices view = ViewInvoices.All)
        {
            var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetCompletedDeliveriesForOrder(invoiceFilter.OrderId, invoiceFilter.StartDate, invoiceFilter.EndDate);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EmailDocumentPopup(int id, string number, string desc, CompanyType companyType, DocumentName docName)
        {
            using (var tracer = new Tracer("BaseController", "EmailDocumentPopup"))
            {
                var model = new EmailDocumentViewModel();
                if (docName == DocumentName.PO)
                {
                    model.OrderId = id;
                    model.PoNumber = number;
                }
                else if (docName == DocumentName.BDR)
                {
                    model.InvoiceHeaderId = id;
                    model.InvoiceNumber = number;
                }
                else if (docName == DocumentName.InvoiceSummary)
                {
                    model.InvoiceId = id;
                    model.InvoiceNumber = number;
                }
                else if (docName == DocumentName.MarineTaxAffidavit)
                {
                    model.InvoiceHeaderId = id;
                    model.InvoiceNumber = number;
                }
                else if (docName == DocumentName.CGInspection)
                {
                    model.InvoiceHeaderId = id;
                    model.InvoiceNumber = number;
                }
                else if (docName == DocumentName.InspRequestVoucher)
                {
                    model.InvoiceHeaderId = id;
                    model.InvoiceNumber = number;
                }
                else
                {
                    model.InvoiceId = id;
                    model.InvoiceNumber = number;
                    model.PoNumber = desc;
                }

                model.CompanyType = companyType;
                model.DocumentName = docName;

                return PartialView("_PartialEmailPopup", model);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveEmailDocumentNotification(EmailDocumentViewModel emailDocumentViewModel)
        {
            using (var tracer = new Tracer("BaseController", "SaveEmailDocumentNotification"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().SaveEmailDocNotificationDetails(emailDocumentViewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddTankSchedule(int frId)
        {
            var tank = new TankDetailsViewModel() { StartDate = DateTimeOffset.Now };
            var tankFrequency = new TankRentalFrequencyViewModel() { FuelRequestId = frId, FrequencyTypes = FrequencyTypes.Weekly };
            tankFrequency.Tanks.Add(tank);

            return PartialView("~/Views/Shared/TankRentalInvoice/_PartialTankFrequencies.cshtml", tankFrequency);
        }

        [HttpGet]
        public ActionResult AddTank(int tankScheduleId)
        {
            return PartialView("~/Views/Shared/TankRentalInvoice/_PartialTankDetails.cshtml", new TankDetailsViewModel() { BillingFrequencyId = tankScheduleId, StartDate = DateTimeOffset.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadSpecialInstructionFiles(SpecialInstructionAttachmentViewModel model, HttpPostedFileBase[] specialInstructionFiles)
        {
            StatusViewModel response = new StatusViewModel();

            if (specialInstructionFiles == null || specialInstructionFiles.Length <= 0)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.btnLabelSelectFile;
            }
            else if (model == null || model.Id == 0)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageOrderNotFound;
            }
            else
            {
                var files = new List<AttachmentViewModel>();
                foreach (var file in specialInstructionFiles)
                {
                    AttachmentViewModel obj = new AttachmentViewModel();
                    obj.Name = file.FileName;
                    obj.FileStream = file.InputStream;
                    files.Add(obj);
                }

                response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveSpecialInstructionFilesAsync(model.Id, UserContext, files);
            }

            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);

            return RedirectToAction("Details", "Order", new { id = model?.Id });
        }

        public FileContentResult DownloadSpecialInstructionDocument(int orderId, string fileName)
        {
            using (var tracer = new Tracer("OrderController", "DownloadSpecialInstructionDocument"))
            {
                var azureBlob = new Core.Infrastructure.AzureBlobStorage();
                var fileStream = azureBlob.DownloadBlob(fileName, BlobContainerType.SpecialInstructions.ToString().ToLower());

                var memoryStream = fileStream as MemoryStream;
                var fileExtension = fileName.Split('.').LastOrDefault();
                string mimeType = MediaType.Pdf;

                switch (fileExtension)
                {
                    case "pdf": mimeType = MediaType.Pdf; break;
                    case "doc": mimeType = MediaType.Doc; break;
                    case "docx": mimeType = MediaType.Docx; break;
                    case "bmp": mimeType = MediaType.Bmp; break;
                    case "png": mimeType = MediaType.Png; break;
                    case "jpeg": mimeType = MediaType.Jpeg; break;
                    case "jpg": mimeType = MediaType.Jpg; break;
                }

                Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
                return File(memoryStream.ToArray(), mimeType);
            }
        }

        public async Task<JsonResult> DeleteSpecialInstructionDocument(int orderId, string fileName)
        {
            using (var tracer = new Tracer("OrderController", "DeleteSpecialInstructionDocument"))
            {
                StatusViewModel response;
                response = await ContextFactory.Current.GetDomain<OrderDomain>().DeleteSpecialInstructionDocument(UserContext, orderId, fileName);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public FileContentResult DownloadInvoiceFile(int invoiceId, string fileName)
        {

            using (var tracer = new Tracer("BaseController", "DownloadInvoiceFile"))
            {
                var azureBlob = new Core.Infrastructure.AzureBlobStorage();
                var fileStream = azureBlob.DownloadBlob(fileName, BlobContainerType.InvoicePdfFiles.ToString().ToLower());
                if (fileName.Contains(ApplicationConstants.AssetDropImageFileNamePrefix))
                {
                    fileStream = azureBlob.DownloadBlob(fileName, BlobContainerType.JobFilesUpload.ToString().ToLower());
                }


                var memoryStream = fileStream as MemoryStream;
                var fileExtension = fileName.Split('.').LastOrDefault();
                string mimeType = MediaType.Pdf;

                switch (fileExtension.ToLower())
                {
                    case "pdf": mimeType = MediaType.Pdf; break;
                    case "doc": mimeType = MediaType.Doc; break;
                    case "docx": mimeType = MediaType.Docx; break;
                    case "bmp": mimeType = MediaType.Bmp; break;
                    case "png": mimeType = MediaType.Png; break;
                    case "jpeg": mimeType = MediaType.Jpeg; break;
                    case "jpg": mimeType = MediaType.Jpg; break;
                }

                Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
                return File(memoryStream.ToArray(), mimeType);
            }
        }

        public FileContentResult DownloadJobFile(string filePath)
        {
            using (var tracer = new Tracer("BaseController", "DownloadJobFile"))
            {
                var azureBlob = new Core.Infrastructure.AzureBlobStorage();
                var fileStream = azureBlob.DownloadBlob(filePath, BlobContainerType.JobFilesUpload.ToString().ToLower());

                var memoryStream = fileStream as MemoryStream;
                var fileExtension = filePath.Split('.').LastOrDefault();
                string mimeType = MediaType.Pdf;

                switch (fileExtension.ToLower())
                {
                    case "pdf": mimeType = MediaType.Pdf; break;
                    case "doc": mimeType = MediaType.Doc; break;
                    case "docx": mimeType = MediaType.Docx; break;
                    case "bmp": mimeType = MediaType.Bmp; break;
                    case "png": mimeType = MediaType.Png; break;
                    case "jpeg": mimeType = MediaType.Jpeg; break;
                    case "jpg": mimeType = MediaType.Jpg; break;
                }

                Response.AppendHeader("Content-Disposition", "inline; filename=" + filePath);
                return File(memoryStream.ToArray(), mimeType);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetTerminalsForOrders(AutoCompleteInputViewModel model)
        {
            using (var tracer = new Tracer("BaseController", "GetTerminalsForOrders"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetClosestTerminalsForOrders(UserContext.CompanyId, model.OrderList, model.Terminal);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetStatesEx(int countryId = (int)Country.All)
        {
            var response = CommonHelperMethods.GetStatesEx(countryId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetOrderBadgesByTerminal(GetOrderBadgeDetailsInput input)
        {
            var response = await new ScheduleBuilderDomain().GetOrderBadgesByTerminal(input.OrderIds, input.PickupLocationType, input.PickupLocationId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCountriesEx()
        {
            var response = CommonHelperMethods.GetCountriesEx();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCountriesGroupEx(int countryId = 0)
        {
            var response = CommonHelperMethods.GetCountriesGroupEx(countryId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveDemandCaptureInfo(string fileName, string errors)
        {
            using (var tracer = new Tracer("DashboardController", "SaveDemandCaptureInfo"))
            {
                var bulkDomain = ContextFactory.Current.GetDomain<DashboardDomain>();
                bulkDomain.AddErrorListToQueue(errors, fileName, UserContext);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetFreightServiceParameters()
        {
            var result = await ContextFactory.Current.GetDomain<MasterDomain>().GetFreightServiceParameters(UserContext.CompanyId, UserContext.Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDemandCaptureForm(int sourceType = 1)
        {
            return PartialView("~/Views/Shared/DemandCapture/_PartialDemandCaptureForm.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> GetEditHeldDrForm(string id)
        {
            var model = await ContextFactory.Current.GetDomain<HeldDrQueueDomain>().GetHeldDeliveryRequestById(id);
            return PartialView("~/Views/Shared/_PartialEditHeldDR.cshtml", model);
        }

        public ActionResult GetPartialViewAsPdf(string partialViewName, string fileNameWithoutExtension, object pdfModel, bool AddPortraitOrientation = true)
        {
            return PDFView.GetPartialViewAsPdf(partialViewName, fileNameWithoutExtension, pdfModel, "_LayoutPdf", AddPortraitOrientation);
        }

        public byte[] GetPartialViewPdfContent(string partialViewName, object pdfModel)
        {
            return PDFView.GetPartialViewPdfContent(partialViewName, this.ControllerContext, pdfModel, "_LayoutPdf");
        }
        public byte[] GetPartialViewPdfContentWithDefaultMasterName(string partialViewName, object pdfModel)
        {
            return PDFView.GetPartialViewPdfContent(partialViewName, this.ControllerContext, pdfModel, "");
        }

        [HttpPost]
        public async Task<ActionResult> InvoiceReconcilationGrid(InvoiceReportFilter requestModel)
        {
            using (var tracer = new Tracer("ReportController", "InvoiceReconcilationGrid"))
            {
                var response = new List<InvoiceReconcilationViewModel>();
                var dataTableSearchModel = new DataTableSearchModel(requestModel);
                var totalCount = 0;
                if (!string.IsNullOrEmpty(requestModel.StartDate) && !string.IsNullOrEmpty(requestModel.EndDate))
                {
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetInvoiceReconcilationAsync(requestModel, dataTableSearchModel);
                    if (response.Count > 0)
                        totalCount = response[0].TotalCount;
                }
                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = requestModel.draw,
                        data = response,
                        recordsTotal = totalCount,
                        recordsFiltered = totalCount
                    },

                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        #region DipTest Methods
        [AuthorizeCompany(CompanyType.Carrier, CompanyType.Buyer, CompanyType.Supplier)]
        public async Task<JsonResult> GetDipTestData(int? jobId, string regionId = "", int buyerCompanyId = 0, bool requestFromBuyerWallyBoard = false, bool isCreateDR = true)
        {

            var dipTests = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDipTest(jobId, regionId, UserContext, buyerCompanyId, requestFromBuyerWallyBoard, isCreateDR);
            return Json(dipTests, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeCompany(CompanyType.Carrier, CompanyType.Buyer, CompanyType.Supplier)]
        public async Task<JsonResult> IsTankNotAvailableForOrderProducts(int jobId, int customerId, string regionId, string productTypeIds, int endSupplier = 0)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "IsTankNotAvailableForOrderProducts"))
            {
                var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().IsTankNotAvailableForOrderProducts(UserContext, customerId, jobId, regionId, productTypeIds, endSupplier);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> DiptestSummaryGrid(int lastUpdated = 24)
        {
            var dipTests = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDiptestSummaryAsync(lastUpdated, UserContext);
            return new JsonResult
            {
                Data = dipTests,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public JsonResult IsValidTpoCompany(int companyId)
        {
            var result = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().IsValidTpoCompany(companyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetDemandCaptureChartDataByTankAndSite(int jobId, int companyId, int noOfDays)
        {
            //get tanks for job
            var assetIds = await ContextFactory.Current.GetDomain<JobDomain>().GetAssetIdsByJobForDipTest(companyId, jobId);
            //get display job id for job
            string timeZoneName = string.Empty;
            var siteId = ContextFactory.Current.GetDomain<JobDomain>().GetDisplayJobIdByJobIdForJob(jobId, ref timeZoneName);
            //get dip test details
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDemandCaptureChartDataByTankAndSite(assetIds, siteId, noOfDays);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetJobTankDetails(int jobId)
        {
            var jobTanks = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetJobTankList(jobId);
            return Json(jobTanks, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeCompany(CompanyType.Carrier)]
        public async Task<JsonResult> GetSiteList(string regionId)
        {
            var dipTests = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetSiteList(regionId, UserContext, string.Empty);
            return Json(dipTests, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> EditHeldDr(HeldDeliveryRequestModel model)
        {
            var response = new StatusViewModel();
            if (ModelState.IsValid)
            {
                response = await ContextFactory.Current.GetDomain<HeldDrQueueDomain>().EditHeldDr(model, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> RaiseRequests(RaiseDeliveryRequest inputData)
        {
            var startTime = DateTime.Now;
            var preferenceSeting = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetCreateDrSetting(UserContext.CompanyId);

            //CREDIT CHECK DRS
            if (preferenceSeting.CreditCheckType == CreditCheckTypes.SAP)
            {
                if (inputData.DeliveryRequests.Any() && inputData.DeliveryRequests.Any(dr => dr.IndicativePrice > 0 && dr.ScheduleQuantityType == 1 && dr.RequiredQuantity > 0))
                {
                    var creditCheckRequests = inputData.DeliveryRequests.FindAll(dr => dr.IndicativePrice > 0 && dr.ScheduleQuantityType == 1 && dr.RequiredQuantity > 0);
                    var pushtoHeldQueueResponse = await ContextFactory.Current.GetDomain<HeldDrQueueDomain>().PushtoHeldQueue(UserContext, creditCheckRequests);

                    if (pushtoHeldQueueResponse.StatusCode == Status.Success)
                    {
                        var sapResponse = ContextFactory.Current.GetDomain<HeldDrQueueDomain>().CallSAPAPI(pushtoHeldQueueResponse, UserContext);
                        return Json(sapResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(pushtoHeldQueueResponse, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Error Text", JsonRequestBehavior.AllowGet);
                }
            }
            //NON CREDIT CHECK DRS
            else
            {
                var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().RaiseDeliveryRequests(inputData.DeliveryRequests, UserContext);
                LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "BaseController", "RaiseRequests", JsonConvert.SerializeObject(inputData), JsonConvert.SerializeObject(response), 0, "Azure-AppService", startTime, DateTime.Now);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> ResendHeldDrToSAP(string heldDrId)
        {
            var response = new StatusViewModel();
            var heldDr = await ContextFactory.Current.GetDomain<HeldDrQueueDomain>().GetHeldDeliveryRequestById(heldDrId);

            if (!string.IsNullOrWhiteSpace(heldDr.HeldDrId))
            {
                var model = new HeldDeliveryRequestsModel();
                model.Requests.Add(heldDr);
                response = ContextFactory.Current.GetDomain<HeldDrQueueDomain>().CallSAPAPI(model, UserContext);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ShowHeldDeliveryRequests()
        {
            return PartialView("~/Views/Shared/_PartialHeldDeliveryRequests.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> GetHeldDeliveryRequests()
        {
            var response = await ContextFactory.Current.GetDomain<HeldDrQueueDomain>().GetHeldDeliveryRequests(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteHeldDr(string heldDrId)
        {
            var response = await ContextFactory.Current.GetDomain<HeldDrQueueDomain>().DeleteHeldDr(heldDrId, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> RaiseRequest(RaiseDeliveryRequestInput inputData)
        {
            var startTime = DateTime.Now;
            var preferenceSeting = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetCreateDrSetting(UserContext.CompanyId);

            //CREDIT CHECK DRS
            if (preferenceSeting.CreditCheckType == CreditCheckTypes.SAP)
            {
                if (inputData != null && inputData.IndicativePrice > 0 && inputData.ScheduleQuantityType == 1 && inputData.RequiredQuantity > 0)
                {
                    var pushtoHeldQueueResponse = await ContextFactory.Current.GetDomain<HeldDrQueueDomain>().PushtoHeldQueue(UserContext, new List<RaiseDeliveryRequestInput>() { inputData });

                    if (pushtoHeldQueueResponse.StatusCode == Status.Success)
                    {
                        var sapResponse = ContextFactory.Current.GetDomain<HeldDrQueueDomain>().CallSAPAPI(pushtoHeldQueueResponse, UserContext);
                        return Json(sapResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(pushtoHeldQueueResponse, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Error Text", JsonRequestBehavior.AllowGet);
                }
            }
            //NON CREDIT CHECK DRS
            else
            {
                var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().RaiseDeliveryRequests(new List<RaiseDeliveryRequestInput>() { inputData }, UserContext);
                LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "BaseController", "RaiseRequest", JsonConvert.SerializeObject(inputData), JsonConvert.SerializeObject(response), 0, "Azure-AppService", startTime, DateTime.Now);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetHeldDeliveryRequestCount()
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetHeldDeliveryRequestCount(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ReCreateDeliveryRequests(ReCreateDeliveryRequestsViewModel viewModel)
        {
            var requestJson = JsonConvert.SerializeObject(viewModel);
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().ReCreateDeliveryRequests(viewModel, UserContext);
            var responseJson = JsonConvert.SerializeObject(response);
            LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "BaseController", "ReCreateDeliveryRequests", requestJson, responseJson, 0, "Azure-AppService", DateTime.Now, DateTime.Now);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDeliveryRequests(string regionId = "", string selectedDate = null)
        {
            var startTime = DateTime.Now;
            var requestModelJson = JsonConvert.SerializeObject(new { regionId = regionId, selectedDate = selectedDate });
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDeliveryRequests(UserContext.CompanyId, regionId, selectedDate)).Result;
            LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "BaseController", "GetDeliveryRequests", requestModelJson, JsonConvert.SerializeObject(response), 0, "Azure-AppService", startTime, DateTime.Now);

            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public async Task<ActionResult> GetCalendarDeliveryRequests(CalendarFilterModel inputModel)
        {
            using (var tracer = new Tracer("BaseController", "GetCalendarDeliveryRequests"))
            {
                var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetCalendarDeliveryRequests(UserContext.CompanyId, inputModel);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public JsonResult GetScheduleQtyType()
        {
            var response = CommonHelperMethods.GetScheduleQtyType();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBrokeredDrRequestedToMe(string regionId, string selectedDate = null)
        {
            using (var tracer = new Tracer("BaseController", "GetBrokeredDrRequestedToMe"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetBrokeredDrRequestedToMe(UserContext.CompanyId, regionId, selectedDate)).Result;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult GetBrokeredDrRequestedByMe(string regionId, string selectedDate = null)
        {
            using (var tracer = new Tracer("BaseController", "GetBrokeredDrRequestedByMe"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetBrokeredDrRequestedByMe(UserContext.CompanyId, regionId, selectedDate)).Result;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetDeliveryRequestById(string deliveryRequestId)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDeliveryRequestByIdAsync(deliveryRequestId)).Result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> createBuyerDeliveryRequest(List<RaiseDeliveryRequestInput> inputData)
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().RaiseDeliveryRequests(inputData, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateDeliveryRequest(List<DeliveryRequestViewModel> inputData)
        {
            var requestJson = JsonConvert.SerializeObject(inputData);
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().UpdateDeliveryRequest(inputData, UserContext);
            if (response.StatusCode == (int)Status.Success)
            {
                if (inputData.Any(t => t.IsSpiltDRAdded && t.SpiltDRs.Any()))
                {
                    foreach (var data in inputData.Where(t => t.IsSpiltDRAdded && t.SpiltDRs.Any()))
                    {
                        var status = await AddSubDrs(data);
                        var splitResponse = status.Data as StatusViewModel;
                        if (splitResponse.StatusCode != Status.Failed)
                        {
                            response.StatusMessage = splitResponse.StatusMessage;
                        }
                    }
                }
            }
            var responseJson = JsonConvert.SerializeObject(response);
            LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "BaseController", "UpdateDeliveryRequest", requestJson, responseJson, 0, "Azure-AppService", DateTime.Now, DateTime.Now);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AddSubDrs(DeliveryRequestViewModel inputData)
        {
            StatusViewModel response = new StatusViewModel();
            SpiltDeliveryRequestViewModel spiltDeliveryRequestModel = new SpiltDeliveryRequestViewModel();
            spiltDeliveryRequestModel.GroupParentDrId = inputData.GroupParentDRId;
            spiltDeliveryRequestModel.UserId = UserContext.Id;
            inputData.SpiltDRs.ForEach(x =>
            {
                spiltDeliveryRequestModel.SpiltDRsViewModel.Add(new SpiltDRsViewModel { RequiredQuantity = x.RequiredQuantity, ScheduleQuantityType = x.ScheduleQuantityType });
            });

            response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().UpdateSpiltDRsInfo(spiltDeliveryRequestModel, inputData, UserContext);
            if (response.StatusCode == Status.Success)
            {
                var spiltDeliveryRequestJson = JsonConvert.SerializeObject(spiltDeliveryRequestModel);
                LogManager.Logger.WriteError("BaseController", "UpdateSpiltDRsInfo", spiltDeliveryRequestJson);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeBrokeredDrStatus(string drId, string blendedGroupId, BrokeredDrCarrierStatus status)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            if (status == BrokeredDrCarrierStatus.Rejected)
            {
                try
                {
                    blendedGroupId = blendedGroupId.Replace("null", string.Empty);
                    if (string.IsNullOrEmpty(blendedGroupId))
                    {
                        statusViewModel = await ChangeBrokeredDRStatusInfo(drId, status, statusViewModel);
                    }
                    else
                    {
                        var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetBlendedChildDeliveryRequestInfo(blendedGroupId);
                        if (response != null)
                        {
                            foreach (var item in response)
                            {
                                statusViewModel = await ChangeBrokeredDRStatusInfo(item.DrId, status, statusViewModel);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("BaseController", "ChangeBrokeredDrStatus", ex.Message, ex);
                }
            }
            else
            {
                blendedGroupId = blendedGroupId.Replace("null", string.Empty);
                if (string.IsNullOrEmpty(blendedGroupId))
                {
                    statusViewModel = await ContextFactory.Current.GetDomain<FreightServiceDomain>().ChangeBrokeredDrStatus(drId, status);
                }
                else
                {
                    var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetBlendedChildDeliveryRequestInfo(blendedGroupId);
                    if (response != null)
                    {
                        foreach (var item in response)
                        {
                            statusViewModel = await ContextFactory.Current.GetDomain<FreightServiceDomain>().ChangeBrokeredDrStatus(item.DrId, status);
                        }
                    }
                }
            }
            return Json(statusViewModel, JsonRequestBehavior.AllowGet);
        }

        private async Task<StatusViewModel> ChangeBrokeredDRStatusInfo(string drId, BrokeredDrCarrierStatus status, StatusViewModel statusViewModel)
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetAvailableDRCarrierDetails(drId);
            if (response.StatusCode == (int)Status.Success)
            {
                //create the Brokered order between supplier and new carrier.
                statusViewModel = await BrokeredCarrierRejectWorkflow(statusViewModel, response, UserContext);
            }
            else if (response.StatusCode == (int)Status.Warning)
            {
                //update the reject list as no next available available carrier found. 
                // change the delivery request status rejected.
                DRCarrierRejectInfoModel dRCarrierRejectInfoModel = new DRCarrierRejectInfoModel();
                dRCarrierRejectInfoModel.DeliveryRequestId = response.DeliveryRequest.Id;
                dRCarrierRejectInfoModel.CarrierRejectInfo = new TfxCarrierRejectInfoModel { Id = UserContext.CompanyId, Name = UserContext.CompanyName, RejectDate = DateTime.Now.Date, RejectTime = DateTime.Now.TimeOfDay, RejectedBy = UserContext.Id };
                //update reject list.
                var carrierRejectStatus = await ContextFactory.Current.GetDomain<FreightServiceDomain>().UpdateDRCarrierRejectList(dRCarrierRejectInfoModel);
                if (carrierRejectStatus.StatusCode == (int)Status.Success)
                {
                    statusViewModel = await ContextFactory.Current.GetDomain<FreightServiceDomain>().ChangeBrokeredDrStatus(drId, status);
                }
            }

            return statusViewModel;
        }

        private async Task<StatusViewModel> BrokeredCarrierRejectWorkflow(StatusViewModel statusViewModel, DRAvailableCarrierSequenceModel response, UserContext userContext)
        {
            DeliveryRequestBrokerInfoViewModel deliveryRequestBrokerInfoViewModel = new DeliveryRequestBrokerInfoViewModel();
            deliveryRequestBrokerInfoViewModel.OrderId = response.TfxSupplierOrderId;
            deliveryRequestBrokerInfoViewModel.CarrierCompanyId = response.CarrierInfo.Id;
            deliveryRequestBrokerInfoViewModel.CarrierCompanyName = response.CarrierInfo.Name;
            deliveryRequestBrokerInfoViewModel.CarrierRegionId = response.CarrierInfo.RegionId;
            deliveryRequestBrokerInfoViewModel.DeliveryRequest = response.DeliveryRequest;
            deliveryRequestBrokerInfoViewModel.IsDispatchRetainedByCustomer = false;
            var brokeredCarrierOrder = await ContextFactory.Current.GetDomain<CarrierDomain>().BrokerDeliveryRequestToNextCarrier(deliveryRequestBrokerInfoViewModel, response.DeliveryRequest.Id);
            if (brokeredCarrierOrder.StatusCode == (int)Status.Success)
            {
                DRCarrierRejectInfoModel dRCarrierRejectInfoModel = new DRCarrierRejectInfoModel();
                dRCarrierRejectInfoModel.DeliveryRequestId = response.DeliveryRequest.Id;
                dRCarrierRejectInfoModel.CarrierRejectInfo = new TfxCarrierRejectInfoModel { Id = userContext.CompanyId, Name = userContext.CompanyName, RejectDate = DateTime.Now.Date, RejectTime = DateTime.Now.TimeOfDay, RejectedBy = UserContext.Id };
                //update reject list.
                statusViewModel = await ContextFactory.Current.GetDomain<FreightServiceDomain>().UpdateDRCarrierRejectList(dRCarrierRejectInfoModel);
                if (statusViewModel.StatusCode == (int)Status.Success)
                {

                    // cancelled the fuel request and order between supplier and previous carrier.
                    int fuelRequestId = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetBrokeredFuelDetailsAsync(response.DeliveryRequest.OrderId.Value);
                    if (fuelRequestId > 0)
                    {
                        var fuelRequestStatus = await ContextFactory.Current.GetDomain<FuelRequestDomain>().CancelBrokeredFuelRequestAsync(fuelRequestId, UserContext.Id);
                        if (fuelRequestStatus.StatusCode == (int)Status.Success)
                        {
                            CancelOrderViewModel cancelOrderViewModel = new CancelOrderViewModel();
                            cancelOrderViewModel.OrderId = response.DeliveryRequest.OrderId.Value;
                            cancelOrderViewModel.Reason = Resource.msgbrokeredtoanothercarrier;
                            cancelOrderViewModel.CanceledBy = userContext.Id;
                            var ordercancelStatus = await ContextFactory.Current.GetDomain<OrderDomain>().CancelBrokeredOrderAsync(cancelOrderViewModel);
                            if (ordercancelStatus.StatusCode != (int)Status.Success)
                            {
                                statusViewModel.StatusCode = Status.Failed;
                                statusViewModel.StatusMessage = Resource.failedbrokerederrorMessage;
                            }
                            else
                            {
                                var drCloseStatus = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().DeleteDeliveryRequestOnOrderClose(new List<int> { cancelOrderViewModel.OrderId }, UserContext);
                                if (drCloseStatus.StatusCode != (int)Status.Success)
                                {
                                    DisplayCustomMessages((MessageType)drCloseStatus.StatusCode, drCloseStatus.StatusMessage);
                                }
                                statusViewModel.StatusMessage = Resource.msgDrRejectedSuccessfully;
                            }
                        }
                        else
                        {
                            statusViewModel.StatusCode = Status.Failed;
                            statusViewModel.StatusMessage = Resource.failedbrokerederrorMessage;
                        }
                    }
                }
            }
            else
            {
                statusViewModel = brokeredCarrierOrder;
            }

            return statusViewModel;
        }

        [HttpPost]
        public async Task<TankVolumeAndUllageViewModel> GetTankVolumeAndUllage(TankVolumeAndUllageInputModel requestModel)
        {
            TankVolumeAndUllageViewModel response = new TankVolumeAndUllageViewModel();
            try
            {
                response = await new FreightServiceDomain().GetTankVolumeAndUllage(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseController", "GetTankVolumeAndUllage", ex.Message, ex);
            }
            return response;
        }
        //[HttpGet]
        //public ActionResult GetAllDeliveryRequestsForBuyer()
        //{
        //    string regionId = "";
        //    var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDeliveryRequests(UserContext.CompanyId, regionId));
        //    return View("View", response);
        //}
        #endregion

        #region JobSpecific Bill To
        [HttpGet]
        public PartialViewResult GetJobSpecificBillToView()
        {
            return PartialView("~/Views/Shared/_PartialJobSpecificBillingAddress.cshtml", new JobSpecificBillToViewModel());
        }
        #endregion

        #region FavoriteSideMenu

        //Get favorite side menu for user
        [HttpGet]
        public async Task<JsonResult> GetFavoriteSideMenu()
        {
            var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetFavoriteSideMenu(UserContext.Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //Make side menu favorite/unfavorite
        [HttpPost]
        public async Task<JsonResult> UpdateFavoriteSideMenu(FavoriteSideMenuViewModel favoriteSideMenu)
        {
            var response = new StatusViewModel();
            try
            {
                favoriteSideMenu.UserId = CurrentUser.Id;
                favoriteSideMenu.CompanyId = CurrentUser.CompanyId;

                if (favoriteSideMenu.IsFavorite)
                {
                    response = await ContextFactory.Current.GetDomain<SettingsDomain>().RemoveFavoriteSideMenu(favoriteSideMenu);
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<SettingsDomain>().SetFavoriteSideMenu(favoriteSideMenu);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseController", "UpdateFavoriteSideMenu", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion FavoriteSideMenu

        [HttpPost]
        public ActionResult UpdateTankSequence(TankDetailViewModel inputData, bool pageReload)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().UpdateTankSequence(inputData)).Result;
            if (response.StatusCode == 0)
            {
                response.StatusMessage = Resource.successMessageForUpdateTankSequence;
            }
            if (pageReload)
            {
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckDuplicateTankSequence(TankDetailViewModel inputData)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().CheckDuplicateTankSequence(inputData)).Result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteRecurringSchedule(string Id)
        {
            using (var tracer = new Tracer("BaseController", "DeleteRecurringSchedule"))
            {
                var response = await Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().DeleteRecurringSchedule(Id, UserContext.Id));
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        #region RouteInfo

        [HttpPost]
        public async Task<JsonResult> CreateRouteInfo(RouteInformationModel model)
        {
            using (var tracer = new Tracer("BaseController", "CreateRouteInfo"))
            {
                model.CreatedBy = UserContext.Id;
                var response = await Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().CreateRouteInfo(model));
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> UpdateRouteInfo(RouteInformationModel model)
        {
            using (var tracer = new Tracer("BaseController", "UpdateRouteInfo"))
            {
                var response = await Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().UpdateRouteInfo(model));
                return Json(response, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public async Task<JsonResult> DeleteRouteInfo(string RouteId, string RegionId)
        {
            using (var tracer = new Tracer("BaseController", "DeleteRouteInfo"))
            {
                var response = await Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().DeleteRouteInfo(RouteId, RegionId, UserContext.Id));
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public async Task<JsonResult> GetRegionLocationDetails(string regionId)
        {
            using (var tracer = new Tracer("BaseController", "GetRegionLocationDetails"))
            {
                var response = await Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetRegionLocationDetails(UserContext.CompanyId, regionId));
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public async Task<JsonResult> GetRouteLocationDetails(string Id, string regionId)
        {
            using (var tracer = new Tracer("BaseController", "GetRegionLocationDetails"))
            {
                var response = await Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetRouteLocationDetails(Id, regionId));
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public async Task<JsonResult> GetRouteInfoDetails(string regionId)
        {
            using (var tracer = new Tracer("BaseController", "GetRouteInfoDetails"))
            {
                var response = await Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetRouteInfoDetails(UserContext.CompanyId, regionId));
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetRoutesForTPOOrder(string regionId)
        {
            using (var tracer = new Tracer("BaseController", "GetRoutesForTPOOrder"))
            {
                var sbDomain = ContextFactory.Current.GetDomain<ScheduleBuilderDomain>();
                var response = Task.Run(() => sbDomain.GetRouteDetails(regionId)).Result;
                if (response == null)
                    response = new List<DropdownDisplayExtended>();
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public async Task<JsonResult> UpdateShiftInfo(RouteInformationModel model)
        {
            using (var tracer = new Tracer("BaseController", "UpdateShiftInfo"))
            {
                var response = await Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().UpdateShiftInfo(model));
                return Json(response, JsonRequestBehavior.AllowGet);

            }

        }
        #endregion

        [HttpPost]
        public async Task<JsonResult> GetBuyerRecurringScheduleDetails(BuyerRecurringSchedule buyerRecurringSchedule)
        {

            using (var tracer = new Tracer("BaseController", "GetBuyerRecurringScheduleDetails"))
            {

                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetRecurringScheduleDetails(buyerRecurringSchedule.JobId, buyerRecurringSchedule.PoNumber, buyerRecurringSchedule.JobSiteId);
                if (response.Count() == 0)
                {
                    var recurringDR = new RecurringDRSchedule();
                    recurringDR.ScheduleType = GetMstScheduleTypes().Where(top => top.Id != 4).ToList().FirstOrDefault().Id;
                    recurringDR.ScheduleQuantityType = Enumeration.GetAll<ScheduleQuantityType>().FirstOrDefault().Id;
                    response.Add(recurringDR);
                }
                if (buyerRecurringSchedule.AssetId > 0)
                {
                    response.ForEach(x => x.Index = buyerRecurringSchedule.AssetId);
                }
                var data = RenderRazorViewToString("_PartialBuyerRecurringSchedule", response);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddDefaultScheduleDetails(int Index, int Records)
        {
            List<RecurringDRSchedule> recurringDRSchedules = new List<RecurringDRSchedule>();
            var response = new RecurringDRSchedule();
            using (var tracer = new Tracer("ScheduleBuilderController", "GetBuyerDefaultScheduleDetails"))
            {

                response.ScheduleType = GetMstScheduleTypes().Where(top => top.Id != 4).ToList().FirstOrDefault().Id;
                response.ScheduleQuantityType = Enumeration.GetAll<ScheduleQuantityType>().FirstOrDefault().Id;
                response.Index = Index;
                response.MaxIndex = Records;
                recurringDRSchedules.Add(response);
                var data = RenderRazorViewToString("_PartialBuyerRecurringSchedule", recurringDRSchedules);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetTrailerCompartmentDetails(List<string> Id)
        {
            using (var tracer = new Tracer("BaseController", "GetTrailerCompartmentDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetTrailerCompartmentDetails(Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetTrailerFuelRetainDetails(List<string> Id)
        {
            using (var tracer = new Tracer("BaseController", "GetTrailerFuelRetainDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetTrailerFuelRetainDetails(Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddTierFuelQuantity(Currency currency, UoM uoM, int rowIndex)
        {
            using (var tracer = new Tracer("BaseController", "AddTierFuelQuantity"))
            {
                var model = new PricingViewModel();
                model.Currency = currency;
                model.UoM = uoM;
                model.RowIndex = ++rowIndex;
                return PartialView("_PartialTierFuelQuantities", model);
            }
        }

        public ActionResult AddOrderBadgeDetails()
        {
            return PartialView("~/Views/Shared/_PartialAddTerminalBulkBadge.cshtml", new TerminalBulkBadgeViewModel());
        }

        [HttpGet]
        public JsonResult GetFormattedPricePerGallon(decimal pricePerGallon, int pricingTypeId, int rackAvgTypeId)
        {
            try
            {
                using (var tracer = new Tracer("BaseController", "GetFormattedPricePerGallon"))
                {
                    var response = ContextFactory.Current.GetDomain<HelperDomain>().GetPricePerGallon(pricePerGallon, pricingTypeId, rackAvgTypeId);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseController", "GetFormattedPricePerGallon", ex.Message, ex);
                return null;
            }
        }

        [HttpPost]
        public ActionResult GetTierPricingForSlider(PricingViewModel model)
        {
            try
            {
                using (var tracer = new Tracer("BaseController", "GetTierPricingForSlider"))
                {
                    return PartialView("~/Views/Shared/_PartialTierPricingSlider.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseController", "GetTierPricingForSlider", ex.Message, ex);
                return null;
            }
        }

        [HttpPost]
        public ActionResult GetTierPricingForSliderTpo(PricingViewModel model)
        {
            try
            {
                using (var tracer = new Tracer("BaseController", "GetTierPricingForSliderTpo"))
                {
                    return PartialView("~/Views/Shared/_PartialTierPricingSliderTpo.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseController", "GetTierPricingForSliderTpo", ex.Message, ex);
                return null;
            }
        }

        [HttpPost]
        public ActionResult GetTierPricingForSliderBroker(PricingViewModel model)
        {
            try
            {
                using (var tracer = new Tracer("BaseController", "GetTierPricingForSliderBroker"))
                {
                    return PartialView("~/Views/Shared/_PartialTierPricingSliderBroker.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseController", "GetTierPricingForSliderBroker", ex.Message, ex);
                return null;
            }
        }
        //Get partial view for demand capture
        [HttpGet]
        public async Task<ActionResult> GetPartialIMSForcastingViewForTpoJob(int jobId)
        {
            try
            {

                var data = new ForcastingPreferenceViewModel() { IsEditable = true };
                var preferenceSeting = new OnboardingPreferenceViewModel();
                if (jobId == 0)
                {
                    preferenceSeting = await ContextFactory.Current.GetDomain<CompanyDomain>().GetPreferencesSettingAsync(0, UserContext);
                }
                data = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, jobId == 0 ? (int)ForcastingSettingLevel.Account : (int)ForcastingSettingLevel.Job, jobId == 0 ? preferenceSeting.Id : jobId);

                return PartialView("~/Views/Shared/Forcasting/_PartialIMSForecasting.cshtml", data);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetPartialIMSForcastingViewForTpoJob", ex.Message, ex);
                return null;
            }
        }

        [HttpGet]
        public ActionResult AddTierFuelQuantityTpo(Currency currency, UoM uoM, int rowIndex)
        {

            var model = new PricingViewModel();
            model.Currency = currency;
            model.UoM = uoM;
            model.RowIndex = ++rowIndex;
            return PartialView("_PartialTierFuelQuantitiesTpo", model);

        }

        [HttpGet]
        public ActionResult AddTierFuelQuantityBroker(Currency currency, UoM uoM, int rowIndex)
        {

            var model = new PricingViewModel();
            model.Currency = currency;
            model.UoM = uoM;
            model.RowIndex = ++rowIndex;
            return PartialView("_PartialTierFuelQuantitiesBroker", model);

        }

        [HttpGet]
        public ActionResult GetOttoSetting()
        {

            var response = ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetOttoSetting(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<JsonResult> GetOttoScheduleDetails(string regionId, string shiftStartTime, string shiftEndTime, string date)
        {

            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetOttoScheduleDetails(UserContext.CompanyId, regionId, shiftStartTime, shiftEndTime, date);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<JsonResult> GetOtherProductsOfSupplier()
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetOtherProductsOfSupplier(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSplitDeliveryRequests(SplitDeliveryRequestModel inputData)
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().CreateSplitDeliveryRequests(inputData, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSplitBlendDeliveryRequests(List<SplitDrArray> splitDrArray)
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().CreateSplitBlendDeliveryRequests(splitDrArray, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetDRReportDropDownFilters()
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDRReportDropDownFilters(CurrentUser.Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateLoadQueue(List<DSBLoadQueueModel> inputData)
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().CreateLoadQueue(inputData, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteLoadQueue(List<string> inputData)
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().DeleteLoadQueue(inputData);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SaveDrFilterPreferences(DrFilterPreferencesModel model)
        {
            model.CompanyId = UserContext.CompanyId;
            model.UserId = UserContext.Id;
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().SaveDrFilterPreferences(model);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetDrFilterPreferences()
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDrFilterPreferences(UserContext.Id);
            LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "BaseController", "GetDrFilterPreferences", JsonConvert.SerializeObject(new { UserId = UserContext.Id }), JsonConvert.SerializeObject(response), 0, "Azure-AppService", DateTime.Now, DateTime.Now);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveFilters(TfxModule moduleId, string filterInput)
        {
            var domain = ContextFactory.Current.GetDomain<DispatcherDomain>();
            var response = await domain.SaveFilters(UserContext.Id, moduleId, filterInput);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetFilters(TfxModule moduleId)
        {
            var domain = ContextFactory.Current.GetDomain<DispatcherDomain>();
            var response = await domain.GetFilters(UserContext.Id, moduleId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> GetLoadQueueNotifications(List<DSBLoadQueueNotificationModel> inputData)
        {
            var response = await ContextFactory.Current.GetDomain<DsbLoadQueueDomain>().GetLoadQueueNotifications(inputData, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> LoadSourceRegionView(int fuelRequestId)
        {
            var model = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFRDetailsById(fuelRequestId, UserContext.CompanyId);
            return PartialView("_PartialSourceRegion", model);
        }

        #region Sourcing Request

        [HttpGet]
        public ActionResult GetAllTPOCompanies(bool isMarineNomination = false)
        {
            var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetAllTPOCompanies("", isMarineNomination);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllBuyerCompanies()
        {
            var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetAllBuyerCompanies();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult FreightOnBoardType()
        {
            var response = new List<DropdownDisplayItem>();
            var enumValueArray = Enum.GetValues(typeof(FreightOnBoardTypes));
            foreach (int name in enumValueArray)
                response.Add(new DropdownDisplayItem() { Id = name, Name = EnumHelperMethods.GetDisplayName((FreightOnBoardTypes)name) });
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult TruckLoadType()
        {
            var response = new List<DropdownDisplayItem>();
            var enumValueArray = Enum.GetValues(typeof(TruckLoadTypes));
            foreach (int name in enumValueArray)
                response.Add(new DropdownDisplayItem() { Id = name, Name = EnumHelperMethods.GetDisplayName((TruckLoadTypes)name) });
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetJobLists(string companyName, bool isFtl, bool foAsTerminal)
        {
            var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetJobList(companyName, isFtl, foAsTerminal, UserContext.Id, UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCurrenyList()
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetCurrenyList();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetUoMList()
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetUoMList();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetStatesOfAllCountries(int countryId = (int)Country.All)
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetStatesOfAllCountries(countryId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetFuelProducts(ProductDisplayGroups productDisplayGroupId, int companyId = 0, int jobId = 0)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetAxxisFuelProducts(productDisplayGroupId, companyId, jobId)).Result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult QuantityIndicatorTypes()
        {
            var response = new List<DropdownDisplayItem>();
            var enumValueArray = Enum.GetValues(typeof(QuantityIndicatorTypes));
            foreach (int name in enumValueArray)
                response.Add(new DropdownDisplayItem() { Id = name, Name = EnumHelperMethods.GetDisplayName((QuantityIndicatorTypes)name) });
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetRackAvgPricingTypes()
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetRackAvgPricingTypes();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllFeeTypes(int companyId, Currency currency = Currency.None, int truckLoadType = (int)TruckLoadTypes.LessTruckLoad)
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetAllFeeTypes(companyId, currency, truckLoadType);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllFeeSubTypes(string feeTypeId, Currency currency = Currency.None)
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetAllFeeSubTypes(feeTypeId, currency);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllFeeConstraintTypes()
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetAllFeeConstraintTypes();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PaymentMethods()
        {
            var response = new List<DropdownDisplayItem>();
            var enumValueArray = Enum.GetValues(typeof(PaymentMethods));
            foreach (int name in enumValueArray)
                response.Add(new DropdownDisplayItem() { Id = name, Name = EnumHelperMethods.GetDisplayName((PaymentMethods)name) });
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCountryList()
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetCountriesEx();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetAddressByLongLat(decimal latitude, decimal longitude)
        {
            var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
            return Json(geoAddress, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetSourcingPriceTypes()
        {
            var response = new List<DropdownDisplayItem>();
            var enumValueArray = Enum.GetValues(typeof(PricingType));
            foreach (int name in enumValueArray)
                response.Add(new DropdownDisplayItem() { Id = name, Name = EnumHelperMethods.GetDisplayName((PricingType)name) });
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Freightregion
        [HttpPost]
        public async Task<JsonResult> GetFreightRateTables(FreightRateInputViewModel filter)
        {
            using (var tracer = new Tracer("OrderController", "GetFreightRateTables"))
            {
                if (filter != null)
                    filter.SupplierId = UserContext.CompanyId;
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFreightRateTables(filter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetFuelSurchargeTables(FreightRateInputViewModel filter)
        {
            using (var tracer = new Tracer("OrderController", "GetFuelSurchargeTables"))
            {
                if (filter != null)
                    filter.SupplierId = UserContext.CompanyId;
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFuelSurchargeTables(filter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetAccessorialFeeTables(FreightRateInputViewModel filter)
        {
            using (var tracer = new Tracer("OrderController", "GetAccessorialFeeTables"))
            {
                if (filter != null)
                    filter.SupplierId = UserContext.CompanyId;
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAccessorialFeeTables(filter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetFuelSurchargeTablesForInvoice(FreightRateInputViewModel filter)
        {
            using (var tracer = new Tracer("OrderController", "GetFuelSurchargeTablesForInvoice"))
            {
                if (filter != null)
                    filter.SupplierId = UserContext.CompanyId;
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFuelSurchargeTablesForInvoice(filter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetFreightRateTablesForInvoice(FreightRateInputViewModel filter)
        {
            using (var tracer = new Tracer("OrderController", "GetFreightRateTablesForInvoice"))
            {
                if (filter != null)
                    filter.SupplierId = UserContext.CompanyId;
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFreightRateTablesForInvoice(filter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetAccessorialFeeTablesForInvoice(FreightRateInputViewModel filter)
        {
            using (var tracer = new Tracer("OrderController", "GetAccessorialFeeTablesForInvoice"))
            {
                if (filter != null)
                    filter.SupplierId = UserContext.CompanyId;
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAccessorialFeeTablesForInvoice(filter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetAccessorialFeeTablesForConsolidated(AccessorialFeeInvoiceInputViewModel filter)
        {
            using (var tracer = new Tracer("OrderController", "GetAccessorialFeeTablesForConsolidated"))
            {
                if (filter == null)
                {
                    filter = new AccessorialFeeInvoiceInputViewModel();
                    filter.SupplierId = UserContext.CompanyId;
                }
                else
                {
                    filter.SupplierId = UserContext.CompanyId;
                }
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAccessorialFeeTablesForConsolidated(filter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetAccessorialFeeTablesForSelectedOrder(string orderIds)
        {
            var response = new List<DropdownDisplayItem>();
            using (var tracer = new Tracer("OrderController", "GetAccessorialFeeTablesForSelectedOrder"))
            {
                if (!string.IsNullOrEmpty(orderIds))
                {
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAccessorialFeeTablesForSelectedOrder(orderIds);
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet); ;
        }

        [HttpPost]
        public async Task<JsonResult> GetFreightCostForInvoice(FreightCostInputViewModel filter)
        {
            using (var tracer = new Tracer("OrderController", "GetFreightCostForInvoice"))
            {
                if (filter != null)
                    filter.SupplierId = UserContext.CompanyId;
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFreightCostForInvoice(filter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region OptionalPickup

        [HttpPost]
        public async Task<JsonResult> GetTerminalsForOptionalPickup(AutoCompleteInputViewModel model)
        {

            var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetClosestTerminalsForOptionalPickup(UserContext.CompanyId, model.OrderList, model.Terminal, model.FuelTypeId);
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public async Task<JsonResult> GetOrderFuelType(List<int> OrderList)
        {
            var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetOrderFuelType(OrderList);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> AddOptionalPickup(List<DSBColumnOptionalPickupInfoModel> OptionalPickupDetails)
        {
            var response = new StatusViewModel();
            var DriverColIndex = 0;
            var ShiftOrderNumber = 0;
            if (OptionalPickupDetails != null && OptionalPickupDetails.Any())
            {
                DriverColIndex = OptionalPickupDetails.FirstOrDefault().DriverColIndex + 1;
                ShiftOrderNumber = OptionalPickupDetails.FirstOrDefault().ShiftOrderNumber;
                OptionalPickupDetails = OptionalPickupDetails.Where(x => x.isAdded == 1).ToList();
                OptionalPickupDetails.ForEach(x => x.CompanyId = CurrentUser.CompanyId);
            }
            if (OptionalPickupDetails.Any())
            {
                response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().AddOptionalPickup(OptionalPickupDetails, UserContext);
                if (response.StatusCode == (int)Status.Success)
                {
                    int driverId = OptionalPickupDetails.FirstOrDefault() == null ? 0 : OptionalPickupDetails.FirstOrDefault().DriverId;
                    if (driverId > 0)
                    {
                        var notificationStatus = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().SendOptionalPickupRefreshPushNotificationDriver(Resource.optionalPickupRefresh, driverId);
                        LogManager.Logger.WriteDebug("AddOptionalPickup", "PushNotification", "StatusCode:" + notificationStatus.StatusCode + " -Message:-" + notificationStatus.StatusMessage);
                    }
                }
            }
            else
            {
                response.StatusCode = Status.Success;
                response.StatusMessage = string.Format(Resource.successOptionalPickup, DriverColIndex, ShiftOrderNumber);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GetOptionalPickupDetails(DSBColumnOptionalPickupInfoModel OptionalPickupDetails)
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetOptionalPickupDetails(OptionalPickupDetails);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteOptionalPickupDetails(string Id, int driverId)
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().DeleteOptionalPickupDetails(Id);

            if (response.StatusCode == (int)Status.Success && driverId > 0)
            {
                var notificationStatus = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().SendOptionalPickupRefreshPushNotificationDriver(Resource.optionalPickupRefresh, driverId);
                LogManager.Logger.WriteDebug("DeleteOptionalPickupDetails", "PushNotification", "StatusCode:" + notificationStatus.StatusCode + " -Message:-" + notificationStatus.StatusMessage);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetDSBShiftFilters(TfxModule moduleId, string regionId)
        {
            var domain = ContextFactory.Current.GetDomain<DispatcherDomain>();
            var response = await domain.GetDSBShiftFilters(UserContext.Id, moduleId, regionId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult> BDRPdf(int invoiceHeaderId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetBDRPdfAsync(invoiceHeaderId, CompanyType.Supplier, UserContext);
            response.IsFromDownloadPdf = false;
            return PartialView("_PartialBDRPdf", response);
        }

        [HttpGet]
        public async Task<ActionResult> MarineTaxAffidavit(int invoiceHeaderId)
        {
            var response = new MarineTaxAffidavitPdfViewModel();
            response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineTaxAffidavitInfo(invoiceHeaderId, UserContext);
            response.IsFromDownloadPdf = false;
            return PartialView("_PartialMarineTaxAffidavitDetails", response);
        }

        [HttpGet]
        public async Task<ActionResult> MarineBDNImage(int invoiceHeaderId)
        {
            var response = new MarineBDNPdfViewModel();
            response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineBDNInfo(invoiceHeaderId, UserContext);
            response.IsFromDownloadPdf = false;
            return PartialView("_PartialMarineBDNDetails", response);
        }

        [HttpGet]
        public async Task<ActionResult> MarineCGInspectionDocument(int invoiceHeaderId)
        {
            var response = new MarineCGInspectionDocumentPdfViewModel();
            response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineCGInspectionDocumentInfo(invoiceHeaderId, UserContext);
            response.IsFromDownloadPdf = false;
            return PartialView("_PartialCGInspectionDocument", response);
        }

        [HttpGet]
        public async Task<ActionResult> MarineInspectionVoucherDocument(int invoiceHeaderId)
        {
            var response = new List<MarineInspectionRequestVoucherViewModel>();
            response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineInspectionVoucherDocumentInfo(invoiceHeaderId, UserContext);
            if (response != null && response.Any())
            {
                response.ForEach(t => t.IsFromDownloadPdf = false);
            }
            return PartialView("_PartialMarineInspectionVoucherTab", response);
        }

        public async Task<ActionResult> DownloadProformaBDNPdf(int id, CompanyType companyType = CompanyType.Supplier)
        {
            using (var tracer = new Tracer("BaseController", "DownloadProformaBDNPdf"))
            {

                var proformaPdfModel = await ContextFactory.Current.GetDomain<OrderDomain>().GetProformaBDNPdfAsync(id);
                proformaPdfModel.PhoneNumber = proformaPdfModel.PhoneNumber.ToFormattedPhoneNumber();
                proformaPdfModel.PoContact.PhoneNumber = proformaPdfModel.PoContact.PhoneNumber.ToFormattedPhoneNumber();
                string partialViewName = "_PartialProformaBDNPdf";
                return GetPartialViewAsPdf(partialViewName, "BDNProforma", proformaPdfModel);
            }
        }
        public async Task<ActionResult> DownloadBDRPdf(int id, CompanyType companyType = CompanyType.Supplier)
        {
            using (var tracer = new Tracer("BaseController", "DownloadBDRPdf"))
            {
                var invoicePdfModel = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetBDRPdfAsync(id, companyType);
                invoicePdfModel.InvoicePdfHeaderDetail.SupplierPhoneNumber = invoicePdfModel.InvoicePdfHeaderDetail.SupplierPhoneNumber.ToFormattedPhoneNumber();
                invoicePdfModel.InvoicePdfHeaderDetail.PoContact.PhoneNumber = invoicePdfModel.InvoicePdfHeaderDetail.PoContact.PhoneNumber.ToFormattedPhoneNumber();

                string partialViewName = "_PartialBDRPdf";
                return GetPartialViewAsPdf(partialViewName, invoicePdfModel.BDRDetailsModel.BDRNumber, invoicePdfModel);
            }
        }

        public async Task<ActionResult> DownloadTaxAffidavitAsPdf(int invoiceHeaderId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineTaxAffidavitInfo(invoiceHeaderId, UserContext);
            response.IsFromDownloadPdf = true;
            string partialViewName = "_PartialMarineTaxAffidavitDetails";
            return GetPartialViewAsPdf(partialViewName, response.BDRNumber, response);

        }

        public async Task<ActionResult> DownloadBDNAsPdf(int invoiceHeaderId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineBDNInfo(invoiceHeaderId, UserContext);
            response.IsFromDownloadPdf = true;
            string partialViewName = "_PartialMarineBDNDetails";
            return GetPartialViewAsPdf(partialViewName, response.BDRNumber, response);

        }
        public async Task<ActionResult> DownloadMarineCGInspectionDocument(int invoiceHeaderId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineCGInspectionDocumentInfo(invoiceHeaderId, UserContext);
            response.IsFromDownloadPdf = true;
            string partialViewName = "_PartialCGInspectionDocument";
            var displayInvNumber = ContextFactory.Current.GetDomain<NotificationDomain>().GetDisplayInvoiceNumberById(0, invoiceHeaderId);
            var fileName = "CGInspection -" + displayInvNumber;
            return GetPartialViewAsPdf(partialViewName, fileName, response);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadMarineInspectionVouchers(int invoiceHeaderId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineInspectionVoucherDocumentInfo(invoiceHeaderId, UserContext);
            if (response != null && response.Any())
            {
                response.ForEach(t => t.IsFromDownloadPdf = true);
                string partialViewName = "_PartialInspRequestVoucherDownload";
                var displayInvNumber = ContextFactory.Current.GetDomain<NotificationDomain>().GetDisplayInvoiceNumberById(0, invoiceHeaderId);
                var fileName = "Insp.Request Voucher -" + displayInvNumber;
                return GetPartialViewAsPdf(partialViewName, fileName, response);
            }
            return null;
        }

        [HttpGet]
        public ActionResult GetMarineDDTsEditView(int invoiceHeaderId, int invoiceTypeId, string displayInvoiceNumber, int orderId, UoM uom, bool canEdit)
        {
            var model = new InvoiceBDNEditFilterViewModel() { InvoiceHeaderId = invoiceHeaderId, DisplayInvoiceNumber = displayInvoiceNumber, InvoiceType = (InvoiceType)invoiceTypeId, OrderId = orderId, UoM = uom, CanEdit = canEdit };
            return PartialView("_PartialMarineDDTEditGrid", model);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadBDRSummary(int id)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetSupplierInvoiceDetailAsync(id, CurrentUser.CompanyId, UserContext);
            return GetPartialViewAsPdf("~/Views/Shared/_PartialInvoiceDetailSummaryPDF.cshtml", response.Invoice.DisplayInvoiceNumber, response);
        }

        [HttpGet]
        public ActionResult GetCountryGroupList(int countryId)
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetMstCountryAsGroup(countryId);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetVesselDetails(int vesselId)
        {
            var result = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetVesselDetails(vesselId, UserContext);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region okta implementation 

        [AuthorizeRole] // All roles are allowed
        public async Task<ActionResult> LogoutOkta()
        {
            var currentSupplierURL = CurrentUser.SupplierURL;
            await SignOutOkta();
            if (!string.IsNullOrEmpty(currentSupplierURL))
            {
                return RedirectToAction("Index", "Home", new { area = "", supplierURL = ApplicationConstants.SupplierURL + currentSupplierURL });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        [AuthorizeRole]
        public async Task SignOutOkta(bool isChangePassword = false)
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
            //https://stackoverflow.com/questions/20569396/why-call-signoutdefaultauthenticationtypes-externalcookie-before-use-of-applic/20575643#20575643
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignOut(ApplicationSecurityConstants.ApplicationCookie);
        }

        #endregion

        [HttpPost]
        public async Task<JsonResult> GetJobCoordinates(List<int> jobIds)
        {
            var response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobCoordinates(jobIds);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GetMarineCustomers(bool IsMarine)
        {
            var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetCustomersForSupplier(UserContext.CompanyId, IsMarine);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GetCustomerJobs(string customerId, bool IsMarine)
        {
            var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetCustomerJobs(customerId, IsMarine, UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GetAssetAndTankForOrders(string jobId)
        {
            var response = new List<DropdownDisplayExtendedProperty>();
            if (!string.IsNullOrWhiteSpace(jobId))
            {
                response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAssetAndTankForOrders(jobId);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GetBuyerCustomerJobs(bool IsMarine)
        {
            var response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobsForBuyer(UserContext.CompanyId, IsMarine);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetJobsForSupplier(bool IsMarine = false)
        {
            var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetJobsForSupplier(UserContext.CompanyId, IsMarine);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> ValidateTankMaxFill(List<DeliveryRequestViewModel> model)
        {
            LogManager.Logger.WriteDebug("ScheduleBuilderController", "ValidateTankMaxFill", JsonConvert.SerializeObject(model));
            var response = new ResponseViewModel();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}