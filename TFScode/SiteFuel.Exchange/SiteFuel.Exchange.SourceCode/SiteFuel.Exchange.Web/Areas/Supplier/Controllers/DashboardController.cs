using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class DashboardController : BaseController
    {
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<ActionResult> Index(string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardController", "Index"))
            {
                var dashboardDomain = new DashboardDomain();
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await dashboardDomain.GetSupplierDashboardAsync(UserContext, decryptedGroupIds);
                response.CompanyGroup.EncryptedGroupIds = groupIds;
                return View(response);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> GetOrdersCount(string groupIds = "", int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetOrdersCount"))
            {
                var dashboardDomain = new DashboardDomain();
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await dashboardDomain.GetSupplierDashboardOrdersAsync(CurrentUser.CompanyId, countryId, (int)currency, decryptedGroupIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> GetInvoicesAndDropTicketsCount(string groupIds = "", int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetInvoicesAndDropTicketsCount"))
            {
                var dashboardDomain = new DashboardDomain();
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await dashboardDomain.GetSupplierDashboardInvoicesAsync(CurrentUser.CompanyId, CurrentUser.Id, countryId, currency, decryptedGroupIds, (int)InvoiceType.DigitalDropTicketManual);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> UpdateIsTaxExemptDisplayed()
        {
            using (var tracer = new Tracer("DashboardController", "UpdateIsTaxExemptDisplayed"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().UpdateIsTaxExemptDisplayed(CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> GetFuelRequestsGrid(int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetFuelRequestsGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetSupplierDashboardRecentFRAsync(CurrentUser.CompanyId, CurrentUser.Id, countryId, currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> GetQuoteRequestsGrid(int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetQuoteRequestsGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetSupplierDashboardQuotesAsync(CurrentUser.CompanyId, CurrentUser.Id, countryId, (int)currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> GetAllCustomers(int driverId = -1, int countryId = (int)Country.USA, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("DashboardController", "GetAllCustomers"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetCustomers(CurrentUser.CompanyId, true, driverId, countryId, currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public JsonResult GetAllDrivers()
        {
            using (var tracer = new Tracer("DashboardController", "GetAllDrivers"))
            {
                var response = ContextFactory.Current.GetDomain<HelperDomain>().GetAllDrivers(CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> GetCustomerOrders(int driverId, int customerCompanyId = 0, int countryId = (int)Country.USA, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("DashboardController", "GetCustomerOrders"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetCustomerOrders(CurrentUser.CompanyId, driverId, customerCompanyId, countryId, currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> GetGallonsStat(int fuelTypeId = 0, string groupIds = "", int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetGallonsStat"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetSupplierDashboardGallonsStatAsync(CurrentUser.CompanyId, CurrentUser.Id, fuelTypeId, countryId, (int)currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> GetWaitingForApprovalCount(int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetWaitingForApprovalCount"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetSupplierDashboardApprovalCountAsync(CurrentUser.CompanyId, CurrentUser.Id, countryId, currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> GetCalenderData(int month = 0, int year = 0, int driverId = -1, int customerCompanyId = 0, List<int> orders = null, int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetCalenderData"))
            {
                var firstDayOfMonth = new DateTime(year == 0 ? DateTime.Now.Year : year, month == 0 ? DateTime.Now.Month : month, 1);
                var lastDayVisible = firstDayOfMonth.AddMonths(1).AddDays(4);
                var selectedOrders = orders == null ? string.Empty : string.Join<int>(",", orders);
                var calEventData = new Usp_CalenderEventViewModel()
                {
                    BuyerCompanyId = customerCompanyId,
                    CountryId = countryId,
                    CurrencyType = (int)currency,
                    DriverId = driverId,
                    SupplierCompanyId = CurrentUser.CompanyId,
                    FirstDayOfMonth = firstDayOfMonth,
                    LastDayOfMonth = lastDayVisible,
                    SelectedOrders = selectedOrders
                };
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>()
                               .GetSupplierCalenderAsync(calEventData, CurrentUser.Id);
                foreach (var data in response)
                {
                    if (data.calendarEventType == (int)CalendarEventType.Order)
                    {
                        data.url = Url.Action("Details", "Order", new { area = "Supplier", data.id });
                    }
                    else if (data.calendarEventType == (int)CalendarEventType.DeliverySchedule)
                    {
                        if (data.isInvoiceGenerated)
                        {
                            data.url = Url.Action("Details", "Invoice", new { area = "Supplier", data.id });
                        }
                        else
                        {
                            data.url = Url.Action("Details", "Order", new { area = "Supplier", data.id });
                        }
                    }
                    else
                    {
                        data.url = Url.Action("Details", "Invoice", new { area = "Supplier", data.id });
                    }
                }

                return new JsonResult
                {
                    Data = response.ToArray(),
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> AcceptFuelRequest(int id) //fuelRequestId, SourceRegionTpoViewModel sourceRegionModel)
        {
            using (var tracer = new Tracer("DashboardController", "AcceptFuelRequest"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().AcceptFuelRequest(UserContext, id); //fuelRequestId);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                else if (response.IsFirstTimeBuyer)
                {
                    SendCreditAppNotification(response);
                }
                return RedirectToAction("Index", "Dashboard", new { area = "Supplier" });
            }
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier)]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> DeclineFuelRequest(int id)
        {
            using (var tracer = new Tracer("DashboardController", "DeclineFuelRequest"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().DeclineFuelRequest(id, CurrentUser.Id);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                return RedirectToAction("Index", "Dashboard", new { area = "Supplier" });
            }
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier)]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> DeclineQuoteRequest(int id)
        {
            using (var tracer = new Tracer("DashboardController", "DeclineQuoteRequest"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().DeclineQuoteRequest(id, CurrentUser.Id);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                return RedirectToAction("Index", "Dashboard", new { area = "Supplier" });
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> GetDropAverages(int fuelTypeId = 0, int customerId = 0, int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetDropAverages"))
            {
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSupplierDashboardDropAveragesAsync(CurrentUser.CompanyId, CurrentUser.Id, customerId, fuelTypeId, countryId, (int)currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public ActionResult YourCustomers()
        {
            return View();
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> YourCustomersGrid()
        {
            using (var tracer = new Tracer("DashboardController", "YourCustomersGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetYourBuyersPerformanceData(UserContext);
                var result = ContextFactory.Current.GetDomain<DashboardDomain>().FilterSupplierCustomers(response, UserContext, false);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> TpoBuyersGrid()
        {
            using (var tracer = new Tracer("DashboardController", "TpoBuyersGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetYourBuyersPerformanceData(UserContext);
                var result = ContextFactory.Current.GetDomain<DashboardDomain>().FilterSupplierCustomers(response, UserContext, true);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> YourSuppliersGrid()
        {
            using (var tracer = new Tracer("DashboardController", "YourSuppliersGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetYourSuppliersPerformanceData(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<ActionResult> AssignTier(int buyerCompanyId, int tierId)
        {
            var response = await ContextFactory.Current.GetDomain<DashboardDomain>().AssignTier(UserContext, buyerCompanyId, tierId);
            return new JsonResult
            {
                Data = response,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<ActionResult> SupplierDetails(int supplierCompanyId, int jobId = 0)
        {
            using (var tracer = new Tracer("DashboardController", "SupplierDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetSupplierDetails(supplierCompanyId, jobId, UserContext);
                return View(response);
            }
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<ActionResult> BuyerDetails(int buyerCompanyId, int jobId = 0, bool isSurchargeTabSelected = false)
        {
            using (var tracer = new Tracer("DashboardController", "BuyerDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetBuyerDetails(buyerCompanyId, jobId, UserContext);
                return View(response);
            }
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<ActionResult> BuyerJobs(int buyerCompanyId)
        {
            var response = await Task.Run(() => new JobFilterViewModel() { Id = buyerCompanyId });
            return PartialView("~/Areas/Supplier/Views/Shared/_PartialJobGrid.cshtml", response);
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<ActionResult> JobGrid(JobFilterViewModel jobFilter = null)
        {
            using (var tracer = new Tracer("DashboardController", "JobGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetBuyerJobsBySupplierAsync(jobFilter, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier)]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> JobDetails(int id)
        {
            using (var tracer = new Tracer("DashboardController", "JobDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobStepsForSuperAdminAsync(id, CurrentUser.CompanyId);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadJobDetailsFailed);
                }

                //set Return URL to get back when submit or cancel is clicked from job details tab
                string returnUrl = null;
                if (id > 0)
                {
                    returnUrl = Url.Action("JobDetails", "Dashboard", new { area = "Supplier", Id = id });
                }
                SetReturnUrl(returnUrl);
                return View("~/Areas/Supplier/Views/Job/CreateJob.cshtml", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCompany(CompanyType.Supplier)]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> SaveJob(JobStepsViewModelForSuperAdmin viewModel)
        {
            using (var tracer = new Tracer("DashboardController", "SaveJob"))
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
                        return RedirectToAction("BuyerDetails", "Dashboard", new { area = "Supplier", buyerCompanyId = viewModel.CompanyId });
                    }
                    else if (response.StatusCode == Status.Failed)
                    {
                        DisplayCustomMessages(MessageType.Error, response.StatusMessage);
                        return View("~/Areas/Supplier/Views/Job/CreateJob.cshtml", viewModel);
                    }
                }

                return View("BuyerDetails", "Dashboard", new { area = "Supplier", buyerCompanyId = viewModel.CompanyId });
            }
        }

        [HttpPost]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<ActionResult> SaveDirectTax(CustomerDetailsViewModel directTaxModel)
        {
            var taxDomain = ContextFactory.Current.GetDomain<TaxExemtionLicenseDomain>();
            var companyTaxModel = new CompanyTaxesViewModel() { DirectTaxes = directTaxModel.DirectTaxes, IsDirectTax = directTaxModel.IsDirectTax, IsEdit = directTaxModel.IsEditDirectTax };
            var response = await taxDomain.SaveDirectTaxAsync(companyTaxModel, CurrentUser.Id, directTaxModel.SupplierCompanyId);
            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            return RedirectToAction("BuyerDetails", new { buyerCompanyId = directTaxModel.SupplierCompanyId });
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> GetNewsfeed(int entityId, int currentPage, int latestId = 0)
        {
            var response = await ContextFactory.Current.GetDomain<NewsfeedDomain>().GetNewsfeedForBuyerAndSupplier(UserContext, entityId, currentPage, latestId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> AddGlobalFuelCost(int fuelTypeId, decimal cost, UoM uom ,bool isNewEntry = true, string stateIds = "[]",int countryId = (int)Country.USA, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("DashboardController", "AddGlobalFuelCost"))
            {
                var stateId = new List<int?>();
                stateId = JsonConvert.DeserializeObject<List<int?>>(stateIds);
                await ContextFactory.Current.GetDomain<CurrentCostDomain>().AddCurrentCostGridAsync(UserContext, fuelTypeId, cost, isNewEntry, stateId, countryId, currency,uom);
                if (isNewEntry)
                {
                    var response = await ContextFactory.Current.GetDomain<CurrentCostDomain>().GetCurrentCostGridAsync(UserContext, countryId, currency);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> UpdateGlobalFuelCost(int currentCostId, decimal cost, UoM uom, string stateIds = "[]", int countryId = (int)Country.USA, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("DashboardController", "UpdateGlobalFuelCost"))
            {
                var stateId = new List<int?>();
                stateId = JsonConvert.DeserializeObject<List<int?>>(stateIds);
                var response = await ContextFactory.Current.GetDomain<CurrentCostDomain>().UpdateCurrentCostGridAsync(UserContext, currentCostId, cost, stateId, countryId, currency,uom,true);
                if (response.StatusCode == Status.Success)
                {
                    var gridData = await ContextFactory.Current.GetDomain<CurrentCostDomain>().GetCurrentCostGridAsync(UserContext, countryId, currency);
                    return Json(gridData, JsonRequestBehavior.AllowGet);
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier, CompanyType.SupplierAndCarrier, CompanyType.BuyerSupplierAndCarrier)]
        public async Task<JsonResult> GetGFCNotDefinedStates(int fuelTypeId = 0, int currentCostId = 0, int countryId = (int)Country.USA, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("DashboardController", "GetGFCNotDefinedStates"))
            {
                var response = await ContextFactory.Current.GetDomain<CurrentCostDomain>().GetGFCNotDefinedStates(UserContext.CompanyId, fuelTypeId, currentCostId, countryId, currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier, CompanyType.SupplierAndCarrier, CompanyType.BuyerSupplierAndCarrier)]
        public async Task<JsonResult> GetAllProductsForCountry(int countryId)
        {
            using (var tracer = new Tracer("DashboardController", "GetAllProductsForCountry"))
            {
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAllProductsForCountry(countryId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier, CompanyType.SupplierAndCarrier, CompanyType.BuyerSupplierAndCarrier)]
        public async Task<JsonResult> GetGlobalFuelCosts(int countryId = (int)Country.USA, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("DashboardController", "GetGlobalFuelCosts"))
            {
                var response = await ContextFactory.Current.GetDomain<CurrentCostDomain>().GetCurrentCostGridAsync(UserContext, countryId, currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> DeleteGlobalFuelCost(int fuelTypeId, int currentCostId, UoM uom, int countryId = (int)Country.USA, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("DashboardController", "DeleteGlobalFuelCost"))
            {
                StatusViewModel deleteStatus = await ContextFactory.Current.GetDomain<CurrentCostDomain>().IsOpenOrderOrCounterOfferExistForFuelType(UserContext, fuelTypeId, currentCostId, currency,uom);
                if (deleteStatus.StatusCode == Status.Success)
                {
                    await ContextFactory.Current.GetDomain<CurrentCostDomain>().InactiveCurrentFuelCost(UserContext, fuelTypeId, currentCostId, countryId, currency, uom);
                    var response = await ContextFactory.Current.GetDomain<CurrentCostDomain>().GetCurrentCostGridAsync(UserContext, countryId, currency);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(deleteStatus, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public JsonResult IsSupplierCostExistForFuelType(int fuelTypeId, UoM uom ,string stateIds = "[]", int countryId = (int)Country.USA, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("DashboardController", "IsSupplierCostExistForFuelType"))
            {
                var response = ContextFactory.Current.GetDomain<CurrentCostDomain>().IsSupplierCostExistForFuelType(UserContext, fuelTypeId, stateIds, countryId, currency, uom);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public PartialViewResult GetGlobalFuelCostTile(int countryId = (int)Country.All, bool isCollapsed = false)
        {
            using (var tracer = new Tracer("DashboardController", "GetFuelRequestsGrid"))
            {
                var response = new CountryViewModel() { Id = countryId, IsCollapsed = isCollapsed };
                return PartialView("_PartialDashboardGlobalFuelCost", response);
            }
        }

        [HttpPost]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<JsonResult> SaveDBTileSettings(string pageId, bool isMultipleTilesUpdated, List<DashboardTileViewModel> settingsModel)
        {
            using (var tracer = new Tracer("DashboardController", "SaveDBTileSetting"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().SaveDBTileSettings(pageId, settingsModel, UserContext);

                if (isMultipleTilesUpdated)
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        //Can supplier do dip test of tanks //Get available tanks count
        [HttpGet]
        public async Task<JsonResult> GetSupplierDiptestAvailability()
        {
            bool response = false;
            try
            {
                using (var tracer = new Tracer("DashboardController", "GetSupplierDiptestAvailability"))
                {
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSupplierDiptestAvailability(CurrentUser.CompanyId);
                    if (!response && (CurrentUser.CompanyTypeId == CompanyType.SupplierAndCarrier || CurrentUser.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier))
                    {
                        response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetCarrierDiptestAvailability(CurrentUser.CompanyId);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetSupplierDiptestAvailability", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetCarrierDiptestAvailability()
        {
            var response = false;
            try
            {
                using (var tracer = new Tracer("DashboardController", "GetCarrierDiptestAvailability"))
                {
                    response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetCarrierDiptestAvailability(CurrentUser.CompanyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetCarrierDiptestAvailability", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //Get partial view for create dip test data
        [HttpGet]
        public PartialViewResult GetSupplierDipTestView()
        {
            try
            {
                using (var tracer = new Tracer("Dashboard", "GetSupplierDipTestView"))
                {
                    return PartialView("_PartialSupplierDipTest");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetSupplierDipTestView", ex.Message, ex);
                throw;
            }
        }
        //
        //Get partial view for demand capture
        [HttpGet]
        public async Task<PartialViewResult> GetSupplierDipTestForm(int companyId, int jobId)
        {
            try
            {
                using (var tracer = new Tracer("Dashboard", "GetSupplierDipTestForm"))
                {
                    var tanks = await ContextFactory.Current.GetDomain<JobDomain>().GetTanksByJobId(companyId, jobId);
                    return PartialView("_PartialSupplierDipTestForm", tanks);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetSupplierDipTestForm", ex.Message, ex);
                return null;
            }
        }
        //Save dip test for tanks
        [HttpPost]
        public async Task<JsonResult> CreateDipTest(DemandModel[] demands)
        {
            var response = new StatusViewModel();
            try
            {
                if (demands.Any())
                {
                    var supplierId = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAcceptedCompanyIdByJobId((int)demands[0].JobId);
                    if (supplierId > 0)
                    {
                        foreach (var demand in demands)
                        {
                            demand.DataSourceTypeId = (int)DipTestMethod.Manual;
                        }
                        response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().CreateTankDipTest(demands.ToList(), supplierId);
                    }
                    else
                    {
                        response = new StatusViewModel { StatusCode = Status.Warning, StatusMessage = Resource.errorDipTestNotCreated };
                    }
                }
                else
                {
                    response = new StatusViewModel { StatusCode = Status.Warning, StatusMessage = Resource.warningNoTanksForSelectedJob };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "CreateDipTest", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //Get jobs for a dip test
        [HttpGet]
        public async Task<JsonResult> GetOpenJobsForCustomer(int userId, int companyId, CompanyType companyType, int customerCompanyId = 0)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = await ContextFactory.Current.GetDomain<JobDomain>().GetOpenJobsByCompanyType(companyType, userId, companyId, customerCompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetOpenJobsForCustomer", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetJobsByCompanyType(int userId, int companyId, CompanyType companyType, int customerCompanyId = 0)
        {
            var response = new List<JobViewModel>();
            try
            {
                response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobsByCompanyType(companyType, userId, companyId, customerCompanyId, CurrentUser.BrandedCompanyId, UserContext.IsBuyerCompany);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetJobsByCompanyType", ex.Message, ex);
            }
            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetAccountingCompanyId(int buyerCompanyId, int JobId)
        {
            var response = new AccountingCompanyIdDetailsViewModel();
            try
            {
                using (var tracer = new Tracer("Dashboard", "GetAccountingCompanyId"))
                {
                    response = ContextFactory.Current.GetDomain<SettingsDomain>().GetAccountingCompanyID(buyerCompanyId, JobId, UserContext);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetAccountingCompanyId", ex.Message, ex);
            }
            return PartialView("_PartialAccountingCompanyIdDetails", response);

        }

        public ActionResult EditAccountingCompanyId(AccountingCompanyIdDetailsViewModel data)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                using (var tracer = new Tracer("Dashboard", "EditAccountingCompanyId"))
                {
                    response = ContextFactory.Current.GetDomain<SettingsDomain>().EditAccountingCompanyId(data, UserContext);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "EditAccountingCompanyId", ex.Message, ex);

            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetSupplierIsBadgeMandatory(int Id, int buyerCompanyId, bool isBadgeMandatory)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                using (var tracer = new Tracer("Dashboard", "SetSupplierIsBadgeMandatory"))
                {
                    response = ContextFactory.Current.GetDomain<SettingsDomain>().SetSupplierIsBadgeMandatory(UserContext.CompanyId, buyerCompanyId, Id, isBadgeMandatory, OrderCreationMethod.FromTPO, UserContext);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "SetSupplierIsBadgeMandatory", ex.Message, ex);

            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Add new site additional details
        /// </summary>
        /// <param name="siteAval"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PartialSiteAvailavility(string count)
        {
            DeliveryDaysViewModel _objDeliverydays = new DeliveryDaysViewModel();
            using (var tracer = new Tracer("JobController", "PartialSiteAvailavility"))
            {
                _objDeliverydays = await ContextFactory.Current.GetDomain<JobDomain>().GetObject(_objDeliverydays, count);
                return PartialView("_PartialSiteAvailavility", _objDeliverydays);
            }
        }

        [HttpGet]
        public async Task<ActionResult> PartialSiteAvailavilityView(string count)
        {
            DeliveryDaysViewModel _objDeliverydays = new DeliveryDaysViewModel();
            using (var tracer = new Tracer("JobController", "PartialSiteAvailavilityView"))
            {
                _objDeliverydays = await ContextFactory.Current.GetDomain<JobDomain>().GetObject(_objDeliverydays, count);
                return PartialView("_PartialSiteAvailavilityView", _objDeliverydays);
            }
        }
        
        [HttpPost]
        public async Task<JsonResult> SaveUpdateCustomerId(int buyerCompanyId, string customerId)
        {
            var _response = new StatusViewModel();
            try
            {
                using (var tracer = new Tracer("DashboardController", "UpdateCustomerId"))
                {
                    _response = await ContextFactory.Current.GetDomain<DashboardDomain>().SaveUpdateCustomerId(buyerCompanyId, customerId, UserContext);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardController", "SaveUpdateCustomerId", ex.Message, ex);
            }
            return Json(_response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteTpoBuyerCompany(int buyerCompanyId)
        {
            var response = new StatusViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<DashboardDomain>().DeleteTpoBuyerCompany(buyerCompanyId, UserContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardController", "DeleteTpoBuyerCompany", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetInventoryDropdownData(int countryId = (int)Country.All)
        {
            var response = new StatusViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetInventoryDropdownData(UserContext, countryId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardController", "GetInventoryDropdownData", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetInventoryDataForDashboard(InventoryDataViewModel filter)
        {
            var response = new StatusViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetInventoryDataForDashboard(filter, UserContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashboardController", "GetInventoryDataForDashboard", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
