using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Logger;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using SiteFuel.Exchange.Web.Models;
using SiteFuel.Exchange.ViewModels.Job;

namespace SiteFuel.Exchange.Web.Areas.SuperAdmin.Controllers
{
    public class SuperAdminController : BaseController
    {
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult ViewCompanies(CompanyFilterType filter = CompanyFilterType.All)
        {
            return View(filter);
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult ViewSiteFuelUsers(SiteFuelUserFilterType filter = SiteFuelUserFilterType.All)
        {
            return View(filter);
        }

        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> GetAccountActivity(string startDate, string endDate)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetAccountActivity"))
            {
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAccountActivityReport(startDate, endDate);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> GetCompanies(CompanyDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetCompanies"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetCompaniesAsync(requestModel);
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;

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
        public ActionResult ManageCityZip()
        {
            var response = new SuperAdminLocationViewModel();
            return View(response);
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> GetCompaniesCreatedByTPO(string startDate, string endDate, CompanyFilterType filter = CompanyFilterType.All)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetCompaniesCreatedByTPO"))
            {
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetCompaniesCreatedByTPO(startDate, endDate);
                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> GetSiteFuelUsers(SiteFuelUserFilterType filter = SiteFuelUserFilterType.All)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetSiteFuelUsers"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetSiteFuelUsersAsync(filter);
                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult CompanyUsers(int id = 0, CompanyUsersFilterType filter = CompanyUsersFilterType.All)
        {
            using (var tracer = new Tracer("SuperAdminController", "CompanyUsers"))
            {
                var response = new UsersFilterViewModel()
                {
                    Id = id,
                    Filter = filter
                };
                return View(response);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public PartialViewResult CompanyUsersPartial(int id = 0, CompanyUsersFilterType filter = CompanyUsersFilterType.All)
        {
            using (var tracer = new Tracer("SuperAdminController", "CompanyUsersPartial"))
            {
                var response = new UsersFilterViewModel()
                {
                    Id = id,
                    Filter = filter
                };
                return PartialView("_PartialUsersGrid", response);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public PartialViewResult CompanyJobsPartial(int id = 0)
        {
            using (var tracer = new Tracer("SuperAdminController", "CompanyJobsPartial"))
            {
                return PartialView("_PartialJobGrid", id);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> JobGrid(int companyId = 0)
        {
            using (var tracer = new Tracer("SuperAdminController", "JobGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetJobGridAsync(companyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> JobDetails(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "JobDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobStepsForSuperAdminAsync(id);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadJobDetailsFailed);
                }

                //set Return URL to get back when submit or cancel is clicked from job details tab
                string returnUrl = null;
                if (id > 0)
                {
                    returnUrl = Url.Action("JobDetails", "SuperAdmin", new { area = "SuperAdmin", Id = id });
                }
                SetReturnUrl(returnUrl);
                return View("CreateJob", response);
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult GetContactPerson(int companyId, int companyTypeId)
        {
            return PartialView("_PartialContactPerson", new JobContactViewModel() { CompanyId = companyId, CompanyTypeId = companyTypeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> SaveJob(JobStepsViewModelForSuperAdmin viewModel)
        {
            using (var tracer = new Tracer("SuperAdminController", "Save"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.UserId = CurrentUser.Id;
                    viewModel.Job.StatusId = (int)JobStatus.Open;

                    SaveJobStatusViewModel response = new SaveJobStatusViewModel();

                    viewModel.Job.ImageDetails.SiteImage = await SetImageDataToBolb(viewModel.Job.ImageDetails.SiteImage, viewModel.Job.ImageDetails.SiteImageFiles, BlobContainerType.JobFilesUpload);
                    viewModel.Job.ImageDetails.AdditionalImage.SiteImage = await SetImageDataToBolb(viewModel.Job.ImageDetails.AdditionalImage.SiteImage, viewModel.Job.ImageDetails.AdditionalImage.SiteImageFiles, BlobContainerType.JobFilesUpload);

                    if (viewModel.Job.Id > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<JobDomain>().UpdateJobStepsForSuperAdminAsync(UserContext, viewModel);
                    }

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("JobDetails", "SuperAdmin", new { area = "SuperAdmin", id = viewModel.Job.Id });
                    }
                    else if (response.StatusCode == Status.Failed)
                    {
                        DisplayCustomMessages(MessageType.Error, response.StatusMessage);
                        return RedirectToAction("JobDetails", "SuperAdmin", new { area = "SuperAdmin", id = viewModel.Job.Id });
                    }
                }
                return View("Create", viewModel);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        [HttpPost]
        public async Task<ActionResult> GetCompanyUsers(CompanyUsersDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetCompanyUsers"))
            {
                var dataTableSearchModel = new DataTableSearchModel(requestModel);
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetCompanyUsersAsync(dataTableSearchModel, requestModel);
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;

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

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> AssignAccountOwner(int companyId, int? accountOwnerId)
        {
            var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().AssignAccountOwner(companyId, accountOwnerId);
            return new JsonResult
            {
                Data = response,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "DeleteOrder"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().DeleteOrder(id, UserContext);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<JsonResult> SaveCityZip(int stateId, string previousCity, string city, string zipCodes)
        {
            StatusViewModel response;
            using (var tracer = new Tracer("SuperAdminController", "SaveCityZip"))
            {
                response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().SaveCityZip(stateId, previousCity, city, zipCodes);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public ActionResult Create(UserRoles userRole = UserRoles.SuperAdmin)
        {
            using (var tracer = new Tracer("SuperAdminController", "Create"))
            {
                var StaticPassword = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingApplicationUserDefaultPassword, "First#1234");
                var RandomPassword = CryptoHelperMethods.GeneratePassword(Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings[ApplicationConstants.RandomPasswordLength]), StaticPassword);
                RegisterViewModel superAdminViewModel = new RegisterViewModel
                {
                    UserRole = userRole,
                    Password = RandomPassword,
                    ConfirmPassword = RandomPassword
                };
                return View(superAdminViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> Create(RegisterViewModel viewModel)
        {
            using (var tracer = new Tracer("SuperAdminController", "Create(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().AddNewUserAsync(viewModel, CurrentUser.Id);
                    if (response == Status.Success)
                    {
                        viewModel.StatusCode = Status.Success;
                        viewModel.StatusMessage = Resource.errMessageCreateAdditionalUserSuccess;

                        var serverUrl = ContextFactory.Current.GetDomain<HelperDomain>().GetServerUrl();
                        var callbackUrl = $"~/Account/Login";
                        var notification = ContextFactory.Current.GetDomain<NotificationDomain>().GetNotificationContent(EventSubType.SuperAdminCreatedNewUser, serverUrl, callbackUrl);
                        var emailTemplate = ContextFactory.Current.GetDomain<HelperDomain>().GetApplicationEventNotificationTemplate();
                        var emailModel = new ApplicationEventNotificationViewModel
                        {
                            To = new List<string> { viewModel.Email },
                            Subject = notification.Subject,
                            CompanyLogo = notification.CompanyLogo,
                            CompanyText = notification.CompanyText,
                            BodyLogo = notification.BodyLogo,
                            BodyText = string.Format(notification.BodyText, $"{viewModel.FirstName} {viewModel.LastName}", viewModel.Email, viewModel.Password),
                            BodyButtonText = notification.BodyButtonText,
                            BodyButtonUrl = notification.BodyButtonUrl
                        };
                        await ContextFactory.Current.GetDomain<EmailDomain>().SendEmail(emailTemplate, emailModel);

                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);

                        if (viewModel.UserRole == UserRoles.SuperAdmin)
                        {
                            return RedirectToAction("ViewSiteFuelUsers", new { filter = SiteFuelUserFilterType.AllSuperAdmin });
                        }
                        else if (viewModel.UserRole == UserRoles.InternalSalesPerson)
                        {
                            return RedirectToAction("ViewSiteFuelUsers", new { filter = SiteFuelUserFilterType.InternalSalesPerson });
                        }
                        else if (viewModel.UserRole == UserRoles.AccountSpecialist)
                        {
                            return RedirectToAction("ViewSiteFuelUsers", new { filter = SiteFuelUserFilterType.AccountSpecialist });
                        }
                        else if (viewModel.UserRole == UserRoles.ExternalVendor)
                        {
                            return RedirectToAction("ViewSiteFuelUsers", new { filter = SiteFuelUserFilterType.ExternalVendor });
                        }
                    }
                    DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                }

                return View(viewModel);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult CreateCompany()
        {
            using (var tracer = new Tracer("SuperAdminController", "CreateCompany"))
            {
                var StaticPassword = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingApplicationUserDefaultPassword, "First#1234");
                OnboardingViewModel onboardingViewModel = new OnboardingViewModel();
                onboardingViewModel.User = new RegisterViewModel() { Password = StaticPassword, ConfirmPassword = StaticPassword };
                onboardingViewModel.User.Company.CompanyTypeId = (int)CompanyType.Buyer;
                onboardingViewModel.Card.BypassPaymentDetails = true;
                onboardingViewModel.User.Company.AccountOwnerId = CurrentUser.Id;
                onboardingViewModel.CompanyAddress.ServiceOffering = Enum.GetValues(typeof(ServiceOfferingType)).Cast<ServiceOfferingType>().Select(c => new CompanyServiceAreaModel { ServiceDeliveryType = c }).ToList();
                return View("~/Views/Onboarding/Company.cshtml", onboardingViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> CreateCompany(RegisterViewModel viewModel)
        {
            using (var tracer = new Tracer("SuperAdminController", "CreateCompany(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
                    var response = await domain.CreateUserAsync(viewModel, CurrentUser.Id);
                    if (response == Status.Success)
                    {
                        return RedirectToAction("Company", "Onboarding", new { area = "", viewModel.Id });
                    }
                }
                return View(viewModel);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> ConvertToInternal(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "ConvertToInternal"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().ConvertExternalToInternal(id);
                if (response.StatusCode == Status.Success)
                {
                    response.Card.BypassPaymentDetails = CurrentUser.IsSuperAdmin;
                }
                return View("~/Views/Onboarding/Company.cshtml", response);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult CompanyDetails(int id, bool isTPOCompany = false)
        {
            using (var tracer = new Tracer("SuperAdminController", "CompanyDetails"))
            {
                CompanyViewModel company = new CompanyViewModel();
                company.Id = id;
                company.IsTPOCompany = isTPOCompany;
                return View("CompanyDetails", company);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin)]
        public ActionResult AuditReportAxxis(AuditReportAxxisFilter reportFilter)
        {
            if (reportFilter == null)
                reportFilter = new AuditReportAxxisFilter();

            return View(reportFilter);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> SendProgressReportNotification(ProgressReportFilter filter)
        {
            var response = false;
            using (var tracer = new Tracer("SuperAdminController", "SendProgressReportNotification"))
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

                DateTime startDate = Convert.ToDateTime(filter.StartDate.Date).Add(filter.StartTime.TimeOfDay);
                DateTime endDate = Convert.ToDateTime(filter.EndDate.Date).Add(filter.EndTime.TimeOfDay);

                DateTime monthStartDate = new DateTime(endDate.Year, endDate.Month, 1);
                DateTime monthEndDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month), 23, 59, 59);

                filter.StartDate = new DateTimeOffset(startDate, timeZoneInfo.GetUtcOffset(startDate));
                filter.EndDate = new DateTimeOffset(endDate, timeZoneInfo.GetUtcOffset(endDate));

                filter.MonthStartDate = new DateTimeOffset(monthStartDate, timeZoneInfo.GetUtcOffset(monthStartDate));
                filter.MonthEndDate = new DateTimeOffset(monthEndDate, timeZoneInfo.GetUtcOffset(monthEndDate));

                response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetProgressReport(filter, true, CurrentUser.Email);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin)]
        public JsonResult GetAuditReportAxxisGrid(string startDate, string endDate)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetAuditReportAxxisGrid"))
            {
                var response = ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAxxisAuditReport(startDate, endDate);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult FuelRequests()
        {
            return View("FuelRequestsSummary");
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult Invoices()
        {
            return View("InvoiceSummary");
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> InvoiceGrid(InvoiceDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoiceGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceGridAsync(requestModel);
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;

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

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult ViewAccountActivity()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<JsonResult> FuelRequestsGrid(FuelRequestDataTableModel requestModel)
        {
            using (var tracer = new Tracer("SuperAdminController", "FuelRequestsGrid"))
            {
                var dataTableSearchModel = new DataTableSearchModel(requestModel);
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFuelRequestsSummary(dataTableSearchModel, requestModel.FromDate, requestModel.ToDate);

                var totalCount = 0;
                if (response != null && response.Any())
                    totalCount = response[0].TotalCount;
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

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> FuelRequestDetails(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "FuelRequestDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestAsync(id);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadFuelRequestDetailsFailed);
                }
                return View("~/Areas/Buyer/Views/FuelRequest/Details.cshtml", response);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult DeleteRequests()
        {
            return View();
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> DeleteRequestsInvoiceGrid(string invoiceNumber)
        {
            using (var tracer = new Tracer("SuperAdminController", "DeleteRequestsInvoiceGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetDeleteRequestsInvoiceGridAsync(invoiceNumber);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> DeleteInvoice(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "DeleteInvoice"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().DeleteInvoice(id, UserContext);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> GetOrders(string poNumber)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetOrders"))
            {
                var orderDetails = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetOrdersByPoNumberAsync(poNumber);
                return Json(orderDetails, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> OrderDetails(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "OrderDetails"))
            {
                var orderDetails = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetOrderDetailsAsync(id);
                return View(orderDetails);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> EditOrderStartDate(int id, DateTime startDate)
        {
            using (var tracer = new Tracer("SuperAdminController", "EditOrderStartDate"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().EditOrderStartDateAsync(id, startDate);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return PartialView("_DisplayCustomMessage");
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult SupplierGeoView()
        {
            return View("SupplierGeoView", new SupplierGeoViewModel() { IncludeAllLocations = true });
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> SupplierGeoView(SupplierGeoViewModel supplierModel)
        {
            using (var tracer = new Tracer("SuperAdminController", "SupplierGeoView(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    await ContextFactory.Current.GetDomain<SuperAdminDomain>().SearchSuppliersByLocation(supplierModel);
                }
                return View("SupplierGeoView", supplierModel);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> ExternalSuppliers()
        {
            using (var tracer = new Tracer("SuperAdminController", "ExternalSuppliers"))
            {
                TimeSpan TimeZoneOffset = GetBrowserTimeZoneOffset();
                string offset = string.Format(Resource.constOffsetFormat, TimeZoneOffset.Hours, TimeZoneOffset.Minutes);
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetExternalSuppliersAsync(offset);
                return View(response);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> GetConvertedSuppliers()
        {
            using (var tracer = new Tracer("SuperAdminController", "GetConvertedSuppliers"))
            {
                TimeSpan timeZoneOffset = GetBrowserTimeZoneOffset();
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetConvertedSuppliers(timeZoneOffset);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult CreateExternalSupplier()
        {
            using (var tracer = new Tracer("SuperAdminController", "CreateExternalSupplier"))
            {
                var response = new ExternalSupplierViewModel();
                response.CompanyAddress = new LocationsViewModel();
                return View(response);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> EditExternalSupplier(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "EditExternalSupplier"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().ExternalSupplierDetailsForEditAsync(id);
                response.CompanyAddress = response.OtherLocationsAndServices.First();
                response.OtherLocationsAndServices.Skip(1).ToList().ForEach(t => t.IsDefault = false);
                return View("CreateExternalSupplier", response);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> CreateExternalSupplier(ExternalSupplierViewModel externalSupplierViewModel)
        {
            using (var tracer = new Tracer("SuperAdminController", "CreateExternalSupplier"))
            {
                if (ModelState.IsValid)
                {
                    StatusViewModel response = new StatusViewModel();
                    if (externalSupplierViewModel.OtherLocationsAndServices == null || externalSupplierViewModel.OtherLocationsAndServices.Count <= 0)
                    {
                        response.StatusCode = Status.Failed;
                        DisplayCustomMessages((MessageType)response.StatusCode, Resource.valMessageExternalSupplierAddressAndService);
                        return View(externalSupplierViewModel);
                    }
                    var duplicateAddress = (from addresses in externalSupplierViewModel.OtherLocationsAndServices
                                            group addresses by new { addresses.Address, addresses.ZipCode, addresses.City, addresses.StateId } into address
                                            select new { count = address.Count() }).FirstOrDefault();

                    if (duplicateAddress.count > 1)
                    {
                        response.StatusCode = Status.Failed;
                        DisplayCustomMessages((MessageType)response.StatusCode, Resource.valMessageExternalSupplierDuplicateAddress);
                        return View(externalSupplierViewModel);
                    }

                    if (externalSupplierViewModel.CompanyDetails.Id > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().UpdateExternalSupplierAsync(UserContext, externalSupplierViewModel);
                    }
                    else
                    {
                        response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().SaveExternalSupplierAsync(UserContext, externalSupplierViewModel);
                    }

                    if (response.StatusCode == Status.Failed)
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageSaveExternalSupplierFailed);
                    }
                    else
                    {
                        return RedirectToAction("ExternalSupplierDetails", new { id = externalSupplierViewModel.CompanyDetails.Id });
                    }
                }
                return View(externalSupplierViewModel);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public PartialViewResult AddLocationAndServices()
        {
            return PartialView("_PartialOtherLocationsAndServices", new LocationsViewModel() { IsDefault = false });
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> UpdateCompanyAccountTypeStatus(int id, bool isSfxOwned)
        {
            using (var tracer = new Tracer("SuperAdminController", "UpdateCompanyAccountTypeStatus"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().UpdateCompanyAccountTypeStatusAsync(id, isSfxOwned);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> UpdateBuyerAuditStatus(int id, bool isAuditApplicable)
        {
            using (var tracer = new Tracer("SuperAdminController", "UpdateBuyerAuditStatus"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().UpdateBuyerAuditStatusAsync(id, isAuditApplicable);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> ExternalSupplierDetails(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "ExternalSupplierDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetExternalSupplierDetailsAsync(id);
                return View("ExternalSupplierDetails", response);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> GetExternalSupplierNotes(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetExternalSupplierNotes"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetExternalSupplierNotesAsync(id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> CompleteExternalSupplierNote(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "CompleteExternalSupplierNote"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().CompleteExternalSupplierNote(UserContext, id);
                if (response == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response, Resource.errMessageCompleteNoteFailed);
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> GetExternalSupplierStatuses(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetExternalSupplierStatuses"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetExternalSupplierStatusesAsync(id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> ChangeExternalSupplierStatus(int externalSupplierId, int statusId)
        {
            using (var tracer = new Tracer("SuperAdminController", "ChangeExternalSupplierStatus"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().ChangeExternalSupplierStatusAsync(UserContext, externalSupplierId, statusId);
                if (response == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response, Resource.errMessageChangeExternalSupplierStatusFailed);
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> AddNewExternalSupplierNote(int externalSupplierId, string note)
        {
            using (var tracer = new Tracer("SuperAdminController", "AddNewExternalSupplierNote"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().AddNewExternalSupplierNoteAsync(UserContext, externalSupplierId, note);
                if (response == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response, Resource.errMessageAddNewExternalSupplierNoteFailed);
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> ExternalSuppliersBulkUpload(HttpPostedFileBase csvFile)
        {
            using (var tracer = new Tracer("SuperAdminController", "ExternalSuppliersBulkUpload"))
            {
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (csvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            var csvFilePath = Server.MapPath(Resource.externalSuppliersBulkUploadFilePath);

                            var settingsDomain = ContextFactory.Current.GetDomain<SuperAdminDomain>();
                            var response = settingsDomain.ValidateExternalSuppliersBulkUploadCsv(csvText, csvFilePath);
                            if (response.StatusCode == Status.Success)
                            {
                                response = await settingsDomain.SaveBulkExternalSuppliersAsync(csvText.Trim(), CurrentUser.Id);
                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                                if (response.StatusCode != Status.Failed)
                                {
                                    return RedirectToAction("ExternalSuppliers");
                                }
                            }
                            else
                            {
                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                            }
                        }
                        else
                        {
                            DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                        }
                    }
                    else
                    {
                        DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                    }
                }
                else
                {
                    DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
                }
                return RedirectToAction("ExternalSuppliers");
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult ExternalMeterData()
        {
            return View(new ExternalMeterDataViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> ExternalMeterData(ExternalMeterDataViewModel viewModel)
        {
            using (var tracer = new Tracer("SuperAdminController", "ExternalMeterData(viewModel)"))
            {
                try
                {
                    var response = new StatusViewModel();
                    response.StatusMessage = Resource.errMessageInvalidData;
                    if (!string.IsNullOrEmpty(viewModel.MeterData) && viewModel.SupplierCompanyId > 0)
                    {
                        // convert string to stream
                        byte[] byteArray = Encoding.UTF8.GetBytes(viewModel.MeterData);
                        MemoryStream stream = new MemoryStream(byteArray);
                        response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().UploadFileToBlob(UserContext, stream, viewModel);
                        if (response.StatusCode == Status.Success)
                        {
                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                            return RedirectToAction("ExternalMeterData");
                        }
                    }
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("SuperAdminController", "ExternalMeterData(viewModel)", ex.Message, ex);
                }
            }
            return View(viewModel);
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult ExternalMeterDataUploadStatus()
        {
            return View("RequestStatus");
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public JsonResult ExternalMeterDataUploadStatusDetails()
        {
            using (var tracer = new Tracer("SuperAdminController", "ExternalMeterDataUploadStatusDetails"))
            {
                List<QueueProcessType> processTypes = new List<QueueProcessType>() { QueueProcessType.ExternalMeterDataUpload };

                var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetUploadDetails(UserContext.Id, processTypes);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult FuelRequestsDashboardSummary(FuelRequestFilterType filter = FuelRequestFilterType.All, FuelRequestType frTypeFilter = FuelRequestType.All)
        {
            DashboardFuelRequestViewModel dashboardFuelRequestViewModel = new DashboardFuelRequestViewModel();
            dashboardFuelRequestViewModel.FuelRequestStatus = filter;
            dashboardFuelRequestViewModel.FuelRequestType = frTypeFilter;
            return View("FuelRequestsDashboardSummary", dashboardFuelRequestViewModel);
        }

        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<JsonResult> FuelRequestsStats()
        {
            var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().FuelRequestsStats();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<JsonResult> GetAllFuelRequests(FuelRequestDataTableModel requestModel)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetAllFuelRequests"))
            {
                string fuelRequestTypes = string.Empty, fuelRequestStatuses = string.Empty;
                int fuelRequestTypeId = (int)requestModel.FuelRequestTypeFilter;
                int fuelRequestStatusId = (int)requestModel.Filter;

                if (fuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest && fuelRequestStatusId == (int)FuelRequestFilterType.Accepted)
                {
                    fuelRequestTypes = ((int)FuelRequestType.ThirdPartyRequest).ToString();
                    fuelRequestStatuses = "";
                }
                else if (fuelRequestTypeId == (int)FuelRequestType.FuelRequest && fuelRequestStatusId == (int)FuelRequestFilterType.Open)
                {
                    fuelRequestTypes = ((int)FuelRequestType.FuelRequest).ToString();
                    fuelRequestStatuses = ((int)FuelRequestStatus.Open).ToString();
                }
                else if (fuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && fuelRequestStatusId == (int)FuelRequestFilterType.Open)
                {
                    fuelRequestTypes = ((int)FuelRequestType.BrokeredFuelRequest).ToString();
                    fuelRequestStatuses = ((int)FuelRequestStatus.Open).ToString();
                }
                else if (fuelRequestTypeId == (int)FuelRequestType.All && fuelRequestStatusId == (int)FuelRequestFilterType.Expired)
                {
                    fuelRequestTypes = string.Join(",", (int)FuelRequestType.BrokeredFuelRequest, (int)FuelRequestType.FuelRequest);
                    fuelRequestStatuses = ((int)FuelRequestStatus.Expired).ToString();
                }
                else if (fuelRequestTypeId == (int)FuelRequestType.All && fuelRequestStatusId == (int)FuelRequestFilterType.AboutToExpire)
                {
                    fuelRequestTypes = string.Join(",", (int)FuelRequestType.BrokeredFuelRequest, (int)FuelRequestType.FuelRequest, (int)FuelRequestType.ThirdPartyRequest);
                    fuelRequestStatuses = ((int)FuelRequestStatus.Open).ToString();
                }
                else
                {
                    fuelRequestTypes = string.Empty;
                    fuelRequestStatuses = string.Empty;
                }

                var dataTableSearchModel = new DataTableSearchModel(requestModel);
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetFuelRequestsSummary(dataTableSearchModel, requestModel.FromDate, requestModel.ToDate, fuelRequestStatuses, fuelRequestTypes);

                var totalCount = 0;
                if (fuelRequestStatusId == (int)FuelRequestFilterType.AboutToExpire)
                {
                    response = response.Where(t => t.AboutToExpire).ToList();
                    totalCount = response.Count;
                }

                if (response != null && response.Any() && fuelRequestStatusId != (int)FuelRequestFilterType.AboutToExpire)
                {
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

        [AuthorizeRole(UserRoles.SuperAdmin)]
        public ActionResult GroupCompanies()
        {
            return View();
        }

        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> CreateCompanyGroup(int id = 0)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetImpersonations"))
            {
                var model = new CompanyGroupViewModel();
                if (id > 0)
                {
                    model = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetGroupDetails(id);
                    model.IsEditAccount = true;
                    return View(model);
                }
                else
                {
                    return View(model);
                }
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> CreateCompanyGroup(CompanyGroupViewModel model)
        {
            using (var tracer = new Tracer("SuperAdminController", "CreateCompanyGroup"))
            {
                model = await ContextFactory.Current.GetDomain<SuperAdminDomain>().CreateCompanyGroup(model);

                DisplayCustomMessages((MessageType)model.StatusCode, model.StatusMessage);

                if (model.StatusCode == Status.Failed)
                    return View(model);
                else
                    return RedirectToAction("GroupCompanies");
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<JsonResult> GetAllCompanyGroups(int parentCompanyId = 0)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetAllCompanyGroups"))
            {
                var model = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetAllCompanyGroups(parentCompanyId);
                return new JsonResult
                {
                    Data = model,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<JsonResult> GetParentCompanyDetails(int parentCompanyId = 0)
        {
            List<ChildCompanyViewModel> model;
            using (var tracer = new Tracer("SuperAdminController", "GetParentCompanyDetails"))
            {
                model = await Task.Run(() => Helpers.CommonHelperMethods.GetParentCompanies(parentCompanyId));
                if (parentCompanyId > 0)
                {
                    model = model.Where(t => t.Id == parentCompanyId).ToList();
                }

                return new JsonResult
                {
                    Data = model,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<JsonResult> GetChildCompanies(CompanyGroupType companyType = CompanyGroupType.Unknown, int parentCompanyId = 0)
        {
            List<ChildCompanyViewModel> model;
            using (var tracer = new Tracer("SuperAdminController", "GetChildCompanies"))
            {
                System.Threading.Thread.Sleep(1000);
                model = await Task.Run(() => Helpers.CommonHelperMethods.GetParentCompanies(parentCompanyId));
                if (companyType != CompanyGroupType.Unknown && companyType != CompanyGroupType.BuyerAndSupplier)
                {
                    model = model.Where(t => t.ChildCompanyType == companyType).ToList();
                }

                return new JsonResult
                {
                    Data = model,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<JsonResult> DeleteCompanyGroup(int parentCompanyId)
        {
            StatusViewModel model;
            using (var tracer = new Tracer("SuperAdminController", "DeleteCompanyGroups"))
            {
                model = await ContextFactory.Current.GetDomain<SuperAdminDomain>().DeleteCompanyGroup(parentCompanyId);
                return new JsonResult
                {
                    Data = model,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> InvoiceDetail(int id = 0)
        {
            using (var tracer = new Tracer("SuperAdminController", "InvoiceDetail"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceDetailAsync(id);
                return View(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> InvoiceHistoryGrid(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoiceHistoryGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceHistoryGridAsync(id, 0, true);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> ExportInvoicesToCsv(string startDate, string endDate)
        {
            using (var tracer = new Tracer("SuperAdminController", "ExportInvoicesToCsv"))
            {
                InvoiceDataTableViewModel requestModel = new InvoiceDataTableViewModel();
                requestModel.StartDate = startDate;
                requestModel.EndDate = endDate;

                var result = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetInvoiceDetailsToExport(requestModel);
                var csv = new SuperAdminDomain().ListToCSV<InvoiceExportViewModel>(result);

                var fileName = "invoice-report-" + DateTimeOffset.UtcNow.ToString("MM/dd/yyyy HH:mm tt") + ".csv";
                return File(new System.Text.UTF8Encoding().GetBytes(csv), Core.Utilities.MediaType.Text, fileName);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult InvoiceHistoryTab(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "InvoiceHistoryTab"))
            {
                return PartialView("_PartialInvoiceHistoryGrid", id);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> InvoicePdfTab(int id)
        {
            using (var tracer = new Tracer("SuperAdminController", "InvoicePdfTab"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoicePdfNewAsync(id, CompanyType.Supplier);
                return PartialView("_PartialInvoicePdf", response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> ConsolidatedInvoicePdfTab(int invoiceHeaderId)
        {
            using (var tracer = new Tracer("SuperAdminController", "ConsolidatedInvoicePdfTab"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetConsolidatedInvoicePdfAsync(invoiceHeaderId, CompanyType.Supplier, UserContext);
                if (response.Invoices.Any() && response.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.TankRental || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.TankRental)))
                {
                    return PartialView("_PartialTankRentalInvoicePdf", response);
                }
                else if (response.Invoices.Any() && response.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.Balance || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.Balance)))
                {
                    return PartialView("_PartialBalanceInvoicePdf", response);
                }
                else if (response.Invoices.Any() && response.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.DryRun))
                {
                    return PartialView("_PartialDryRunInvoicePdf", response);
                }
                else
                {
                    return PartialView("_PartialConsolidatedInvoicePdf", response);
                }
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult ProductMapping()
        {
            return View("ProductMapping", new ProductViewModel());
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult ProductTypeMapping()
        {
            return View("ProductTypeMapping", new ProductTypeMappingViewModel());
        }

        [HttpGet]
        public async Task<JsonResult> GetProducts(string search = "")
        {
            var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
            var response = await masterDomain.GetProducts(search);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> GetProductDetails(int productId)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetProductDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetProductDetails(productId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> ProductMappingGrid(ProductDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("SuperAdminController", "ProductMappingGrid"))
            {
                var dataTableSearchModel = new DataTableSearchModel(requestModel);
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetProductMappingGridAsync(requestModel, dataTableSearchModel);
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;

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

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<JsonResult> CreateProductMapping(ProductViewModel productViewModel)
        {
            StatusViewModel model;
            using (var tracer = new Tracer("SuperAdminController", "CreateProductMapping"))
            {
                model = await ContextFactory.Current.GetDomain<SuperAdminDomain>().CreateProductMapping(productViewModel, UserContext);
                return new JsonResult
                {
                    Data = model,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> CreateProductTypeMapping(ProductTypeMappingViewModel productTypeMappingViewModel)
        {
            StatusViewModel model;
            using (var tracer = new Tracer("SuperAdminController", "CreateProductTypeMapping"))
            {
                if (productTypeMappingViewModel.IsBlend)
                {
                    model = await ContextFactory.Current.GetDomain<SuperAdminDomain>().SaveBlendProductTypeMapping(productTypeMappingViewModel);
                }
                else
                {
                    model = await ContextFactory.Current.GetDomain<SuperAdminDomain>().SaveProductTypeMapping(productTypeMappingViewModel);
                }
                return new JsonResult
                {
                    Data = model,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> BlendProductTypeMappingGrid()
        {
            using (var tracer = new Tracer("SuperAdminController", "BlendProductTypeMappingGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetBlendProductTypeMappingGridAsync();
                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> ProductTypeMappingGrid()
        {
            using (var tracer = new Tracer("SuperAdminController", "ProductTypeMappingGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetProductTypeMappingGridAsync();
                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> GetMappedProductType(int productTypeId, bool isBlend)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetMappedProductType"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetMappedProductType(productTypeId, isBlend);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetProductTypeList(int productTypeId)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetProductTypeList"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetProductTypes(productTypeId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public ActionResult GetTerminalSupplier()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> SaveTerminalItemDescription(TerminalSupplierViewModel model)
        {
            StatusViewModel response;
            using (var tracer = new Tracer("SuperAdminController", "SaveTerminalItemDescription"))
            {
                if (model.Id > 0)
                {
                    response = await ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().UpdateTerminalItemDescription(model, UserContext);
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().SaveTerminalItemDescription(model, UserContext);
                }
                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> DeleteTerminalItemDescription(int id)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("SuperAdminController", "DeleteTerminalItemDescription"))
            {
                if (id > 0)
                {
                    response = await ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().DeleteTerminalItemDescription(id, UserContext);
                }

                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public JsonResult GetTerminalSupplierGrid(TerminalSupplierViewModel model)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetTerminalSupplierGrid"))
            {
                model.Country = ((int)Country.CAN == model.CountryId ? Country.CAN : Country.USA);
                var response = ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().GetTerminalSupplierGridAsync(model);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetTerminalItemDescGrid(TerminalSupplierViewModel model)
        {
            using (var tracer = new Tracer("SuperAdminController", "GetTerminalSupplierDescGrid"))
            {
                model.Country = ((int)Country.CAN == model.CountryId ? Country.CAN : Country.USA);
                var response = ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().GetTerminalItemDescGridAsync(model);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> SaveTerminalSupplier(TerminalSupplierViewModel model)
        {
            StatusViewModel response;
            using (var tracer = new Tracer("SuperAdminController", "SaveTerminalSupplier"))
            {
                if (model.Id > 0)
                {
                    response = await ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().UpdateTerminalSupplier(model, UserContext);
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().SaveTerminalSupplier(model, UserContext);
                }
                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> DeleteTerminalSupplier(int id)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("SuperAdminController", "DeleteTerminalSupplier"))
            {
                if (id > 0)
                {
                    response = await ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().DeleteTerminalSupplier(id, UserContext);
                }

                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public JsonResult GetProductTypeDropDown()
        {
            using (var tracer = new Tracer("SuperAdminController", "GetProductTypeDropDown"))
            {
                var response = ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().GetProductTypeDropDown();
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> MarinePortsAndVessels()
        {
            return View();
        }

        [HttpGet]

        public async Task<ActionResult> GetMarinePortsForSuperAdmin(int countryId = (int)Country.USA)
        {
            var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetMarinePortsForSuperAdmin(countryId, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SaveMarinePort(JobBuyerDashboardViewModel port)
        {
            var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().SaveMarinePort(port, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> DeletePort(int id)
        {
            var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().DeletePort(id, CurrentUser.Id);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> GetMarineVesselsForSuperAdmin(int countryId = (int)Country.USA)
        {
            var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetMarineVesselsForSuperAdmin(UserContext);
            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public async Task<ActionResult> DeleteVessel(int id)
        {
            var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().DeleteVessel(id);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public async Task<ActionResult> SaveMarineVessel(AssetViewModel vessel)
        {
            var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().SaveMarineVessel(vessel, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> CreateTerminals()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> GetTerminals(int countryId = (int)Country.USA)
        {
            var response = new List<PickupLocationDetailViewModel>();
            response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetTerminalsForSuperadminGridAsync(countryId);           
            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> SaveTerminal(PickupLocationDetailViewModel terminal)
        {
            var response =new StatusViewModel();
            response = await ContextFactory.Current.GetDomain<MasterDomain>().SaveTerminalDetails(terminal,UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> GetTerminalMappingDetails(int countryId,int pricingSourceId = (int)PricingSource.Axxis)
        {
            var response = await ContextFactory.Current.GetDomain<MasterDomain>().GetTerminalProductMappingDetails(countryId, pricingSourceId);
            return  new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> GetMstProductsForTerminalMapping(int pricingSourceId = (int)PricingSource.Axxis)
        {
            var response = await ContextFactory.Current.GetDomain<MasterDomain>().GetMstProductsForTerminalMapping(pricingSourceId);
            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> SaveTerminalProductMapping(TerminalProductMappingDetailsViewModel model)
        {
            var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().SaveTerminalProductMapping(model);
            return Json(response, JsonRequestBehavior.DenyGet);
        }
    }

}
