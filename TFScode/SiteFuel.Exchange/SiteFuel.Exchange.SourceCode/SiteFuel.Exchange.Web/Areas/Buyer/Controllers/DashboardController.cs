using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System;
using SiteFuel.Exchange.ViewModels;
using System.Collections.Generic;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using static SiteFuel.Exchange.Web.Helpers.CommonHelperMethods;
using SiteFuel.Exchange.ViewModels.Job;
using SiteFuel.Exchange.ViewModels.Dispatcher;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    [AuthorizeCompany(CompanyType.Buyer)]
    public class DashboardController : BaseController
    {
        public async Task<ActionResult> Index(string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardController", "Index"))
            {
                RemoveReturnUrl();

                var dashboardDomain = new DashboardDomain();
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await dashboardDomain.GetBuyerDashboardAsync(UserContext, 0, decryptedGroupIds);
                response.CompanyGroup.EncryptedGroupIds = groupIds;
                return View(response);
            }
        }

        #region Buyer Dashboard New
        public ActionResult DashboardNew(string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardController", "DashboardNew"))
            {
                RemoveReturnUrl();
                return View();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetJobDetailsForBuyerDashboard(int CountryId = (int)Country.USA)
        {
            var response = new List<JobBuyerDashboardViewModel>();
            try
            {
                response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetJobDetailsForBuyerDashboard(CurrentUser.CompanyId, CountryId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetJobDetailsForBuyerDashboard", ex.Message, ex);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetBuyerLoadsForDashboard(BuyerLoadsForDashboardInputModel input)
        {
            var response = new List<BuyerLoadsForDashboardViewModel>();
            try
            {
                var domain = ContextFactory.Current.GetDomain<DashboardDomain>();
                response = await domain.GetBuyerLoadsForDashboard(UserContext, input);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetBuyerLoadsForDashboard", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> GetLocationInventory(string jobId = "", int priority = 0, int SelectedTab = (int)SelctedSalesTab.Priority, int CountryId = (int)Country.USA)
        {
            //SelectedTab set to 1 by default because on dashboard we are showing only priority tab
            var response = new List<LocationInventoryModel>();
            try
            {
                response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetLocationInventory(CurrentUser.CompanyId, jobId, priority, SelectedTab, CountryId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetLocationInventory", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> GetInvoiceGridForBuyerDashboardAsync(InvoiceGridBuyerDashboardInputModel requestModel)
        {
            using (var tracer = new Tracer("DashboardController", "GetInvoiceGridForBuyerDashboardAsync"))
            {
                List<InvoiceGridBuyerDashboardModel> response = new List<InvoiceGridBuyerDashboardModel>();
                try
                {
                    var dashboardDomain = new DashboardDomain();
                    var invoiceDomain = new InvoiceDomain(dashboardDomain);

                    requestModel.GroupIds = dashboardDomain.DecryptData(requestModel.GroupIds);
                    requestModel.UserId = CurrentUser.Id;
                    requestModel.CompanyId = CurrentUser.CompanyId;
                    requestModel.IsBuyerAdmin = CurrentUser.IsBuyerAdmin;
                    requestModel.BrandedCompanyId = CurrentUser.BrandedCompanyId;

                    response = await invoiceDomain.GetInvoiceGridForBuyerDashboardAsync(requestModel);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashBoardController", "GetInvoiceGridForBuyerDashboardAsync", ex.Message, ex);
                }
              
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetNewBuyerDashboardTileSettings()
        {
            var response = new UserPageSettingViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetNewBuyerDashboardTileSettings(UserContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetNewBuyerDashboardTileSettings", ex.Message, ex);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public async Task<ActionResult> Index(int selectedJobId, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardController", "Index(selectedJobId)"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetBuyerDashboardAsync(UserContext, selectedJobId);
                return View(response);
            }
        }
        //Can buyer do dip test of tanks //Get available tanks count
        [HttpGet]
        public async Task<JsonResult> GetBuyerDipTestAvailability()
        {
            var response = 0;
            try
            {
                using (var tracer = new Tracer("DashboardController", "GetBuyerDipTestAvailability"))
                {
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetAssetCountForBuyerCompany(CurrentUser.CompanyId, 2);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetBuyerDipTestAvailability", ex.Message, ex);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJobsForCountry(int countryId)
        {
            using (var tracer = new Tracer("DashboardController", "GetJobsForCountry"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetJobs(CurrentUser.Id, countryId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public PartialViewResult GetBuyerJobMapForCountry(int selectedJobId = 0, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("BrokerController", "BrokeredOrders"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<DashboardDomain>().GetBuyerJobLocationsForMap(CurrentUser.Id, selectedJobId, currency, countryId)).Result;
                return PartialView("_PartialBuyerJobMapView", response);
            }
        }

        public async Task<JsonResult> UpdateIsTaxExemptDisplayed()
        {
            using (var tracer = new Tracer("DashboardController", "UpdateIsTaxExemptDisplayed"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().UpdateIsTaxExemptDisplayed(CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetJobsCount(int jobId = 0, string groupIds = "", int countryId = (int)Country.USA, Currency currency = Currency.CAD)
        {
            using (var tracer = new Tracer("DashboardController", "GetJobsCount"))
            {
                var dashboardDomain = new DashboardDomain();
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await dashboardDomain.GetBuyerDashboardJobsAsync(CurrentUser.Id, jobId, countryId, currency, decryptedGroupIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetFuelRequestsCount(int jobId = 0, string groupIds = "", int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetFuelRequestsCount"))
            {
                var dashboardDomain = new DashboardDomain();
                var fuelRequestDomain = new FuelRequestDomain(dashboardDomain);
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await fuelRequestDomain.GetBuyerDashboardFuelRequestAsync(CurrentUser.CompanyId, CurrentUser.Id, jobId, countryId, (int)currency, decryptedGroupIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetDeliveryStatistics(int selectedJobId = 0, string groupIds = "", int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetDeliveryStatistics"))
            {
                var dashboardDomain = new DashboardDomain();
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await dashboardDomain.GetBuyerDeliveryStatisticsAsync(CurrentUser.CompanyId, CurrentUser.Id, selectedJobId, countryId, (int)currency, decryptedGroupIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetQuoteRequestsCount(int jobId = 0, string groupIds = "", int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetQuoteRequestsCount"))
            {
                var dashboardDomain = new DashboardDomain();
                var quoteRequestDomain = new QuoteRequestDomain(dashboardDomain);
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await quoteRequestDomain.GetBuyerDashboardQuoteRequestAsync(UserContext, jobId, countryId, currency, decryptedGroupIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetOrdersCount(int jobId = 0, int countryId = (int)Country.All, Currency currency = Currency.None, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardController", "GetOrdersCount"))
            {
                var dashboardDomain = new DashboardDomain();
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await dashboardDomain.GetBuyerDashboardOrdersAsync(CurrentUser.Id, CurrentUser.CompanyId, jobId, countryId, (int)currency, decryptedGroupIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetInvoicesCount(int jobId = 0, string groupIds = "", int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetInvoicesCount"))
            {
                var dashboardDomain = new DashboardDomain();
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await dashboardDomain.GetBuyerDashboardInvoicesAsync(CurrentUser.Id, CurrentUser.CompanyId, CurrentUser.IsBuyerAdmin, jobId, 0, countryId, currency, decryptedGroupIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetDropTicketsCount(int jobId = 0, string groupIds = "", int countryId = (int)Country.All, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("DashboardController", "GetDropTicketsCount"))
            {
                var dashboardDomain = new DashboardDomain();
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await dashboardDomain.GetBuyerDashboardInvoicesAsync(CurrentUser.Id, CurrentUser.CompanyId, CurrentUser.IsBuyerAdmin, jobId, (int)InvoiceType.DigitalDropTicketManual, countryId, currency, decryptedGroupIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetWaitingForApprovalCount(int jobId = 0, int countryId = (int)Country.All, Currency currency = Currency.None, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardController", "GetWaitingForApprovalCount"))
            {
                var dashboardDomain = new DashboardDomain();
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await dashboardDomain.GetBuyerDashboardWaitingForApprovalCountAsync(CurrentUser.Id, CurrentUser.CompanyId, jobId, countryId, currency, decryptedGroupIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetJobAverages(int jobId = 0, int fuelTypeId = 0, int countryId = (int)Country.All, Currency currency = Currency.None, string groupIds = "")
        {
            using (var tracer = new Tracer("DashboardController", "GetJobAverages"))
            {
                var dashboardDomain = new DashboardDomain();
                var decryptedGroupIds = dashboardDomain.DecryptData(groupIds);
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetBuyerDashboardJobAvgsAsync(UserContext, jobId, fuelTypeId, countryId, currency, decryptedGroupIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult YourSuppliers()
        {
            return View();
        }

        public async Task<JsonResult> YourSuppliersGrid()
        {
            using (var tracer = new Tracer("DashboardController", "YourSuppliersGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetYourSuppliersPerformanceData(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> SupplierDetails(int supplierCompanyId, int jobId = 0)
        {
            using (var tracer = new Tracer("DashboardController", "SupplierDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetSupplierDetails(supplierCompanyId, jobId, UserContext);
                return View(response);
            }
        }

        public async Task<JsonResult> GetNewsfeed(int entityId, int currentPage, int latestId = 0)
        {
            var response = await ContextFactory.Current.GetDomain<NewsfeedDomain>().GetNewsfeedForBuyerAndSupplier(UserContext, entityId, currentPage, latestId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
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

        //Get partial view for demand capture
        [HttpGet]
        public PartialViewResult GetBuyerDemandCaptureView()
        {
            try
            {
                using (var tracer = new Tracer("Dashboard", "GetBuyerDemandCaptureView"))
                {
                    return PartialView("_PartialBuyerDemandCapture");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetBuyerDemandCaptureView", ex.Message, ex);
                throw;
            }
        }
        //Get partial view for demand capture
        [HttpGet]
        public async Task<PartialViewResult> GetDipTestView(int jobId)
        {
            try
            {
                using (var tracer = new Tracer("Dashboard", "GetDipTestView"))
                {
                    var tanks = await ContextFactory.Current.GetDomain<JobDomain>().GetTanksByJobId(UserContext.CompanyId, jobId);
                    return PartialView("_PartialBuyerDipTest", tanks);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetDipTestView", ex.Message, ex);
                return null;
            }
        }
        //Get partial view for demand capture
        [HttpGet]
        public async Task<PartialViewResult> GetDeliveryRequestView(int jobId)
        {
            CreateDeliveryRequestModel jobWithProductTypes = new CreateDeliveryRequestModel();
            using (var tracer = new Tracer("Dashboard", "GetDeliveryRequestView"))
            {
                try
                {
                    var job = await ContextFactory.Current.GetDomain<JobDomain>().GetProductTypesForJob(UserContext.Id, UserContext.CompanyId, jobId, CurrentUser.BrandedCompanyId);
                    if (job != null && job.ProductTypes.Any())
                    {
                        jobWithProductTypes = job;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashBoardController", "GetDeliveryRequestView", ex.Message + " JobId :" + jobId, ex);
                }
                return PartialView("_PartialBuyerDeliveryRequest", jobWithProductTypes);
            }
        }

        //Get tanks for a dip test by job id
        [HttpGet]
        public async Task<JsonResult> GetAssetsByJobId(int jobId)
        {
            var response = new List<TankDetailViewModel>();
            try
            {
                response = await ContextFactory.Current.GetDomain<JobDomain>().GetTanksByJobId(UserContext.CompanyId, jobId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "GetAssetsByJobId", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public async Task<JsonResult> CreateDeliveryRequest(CreateDeliveryRequestModel job)
        {
            var response = new StatusViewModel();
            try
            {
                response = await new FreightServiceDomain().CreateDeliveryRequestsFromBuyer(job, UserContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DashBoardController", "CreateDeliveryRequest", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private async Task<DeliveryWindowInfoModel> getDeliveryWindow(JobProductTypeDetails item)
        {
            DeliveryWindowInfoModel deliveryWindowInfoModel = null;
            if (!string.IsNullOrEmpty(item.RetainTime) && !string.IsNullOrEmpty(item.RetainDate))
            {
                deliveryWindowInfoModel = new DeliveryWindowInfoModel();
                deliveryWindowInfoModel.RetainTime = item.RetainTime;
                deliveryWindowInfoModel.RetainDate = Convert.ToDateTime(item.RetainDate);
                deliveryWindowInfoModel.StartTime = item.StartTime;
                deliveryWindowInfoModel.StartDate = Convert.ToDateTime(item.StartDate);
                deliveryWindowInfoModel.EndTime = item.EndTime;
                deliveryWindowInfoModel.EndDate = Convert.ToDateTime(item.EndDate);
            }
            else if (!item.IsRetainButtonClick)
            {
                var forcastinPreferance = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Tank, item.AssetId);
                if (forcastinPreferance.ForcastingServiceSetting.IsEnabled)
                {
                    TankRetainWindowInfo tankRetainWindowInfo = new TankRetainWindowInfo();
                    tankRetainWindowInfo.Quantity = item.RequiredQuantity;
                    tankRetainWindowInfo.Id = item.AssetId;
                    tankRetainWindowInfo.siteId = item.JobDisplayId;
                    tankRetainWindowInfo.storageId = item.StorageId;
                    tankRetainWindowInfo.tankId = item.TankId;
                    IntializeRetainParametersVals(tankRetainWindowInfo, forcastinPreferance);
                    var result = await ContextFactory.Current.GetDomain<SalesDomain>().CalculateTankRetainWindowInfo(tankRetainWindowInfo);
                    if (result != null && !string.IsNullOrEmpty(result.RetainTime))
                    {
                        deliveryWindowInfoModel = new DeliveryWindowInfoModel();
                        deliveryWindowInfoModel.RetainTime = result.RetainTime;
                        deliveryWindowInfoModel.RetainDate = Convert.ToDateTime(result.RetainDate);
                        deliveryWindowInfoModel.StartTime = result.WindowStartTime;
                        deliveryWindowInfoModel.StartDate = Convert.ToDateTime(result.WindowStartDate);
                        deliveryWindowInfoModel.EndTime = result.WindowEndTime;
                        deliveryWindowInfoModel.EndDate = Convert.ToDateTime(result.WindowEndDate);
                    }
                }
            }
            return deliveryWindowInfoModel;
        }

        [HttpGet]
        public async Task<JsonResult> GetRecurringSchedulesByProductTypeId(int jobId, int productTypeId, string prefix, int uom)
        {
            var response = new List<RecurringSchedule>();
            var recurringDrs = await new FreightServiceDomain().GetJobRecurringSchedules(new List<int> { jobId }, new List<int> { productTypeId });
            if (recurringDrs != null && recurringDrs.Any())
            {
                foreach (var record in recurringDrs)
                {
                    RecurringSchedule recurringDR = new RecurringSchedule()
                    {
                        Id = record.Id,
                        ScheduleType = record.ScheduleType,
                        MonthDayId = record.MonthDayId,
                        Date = record.Date,
                        ScheduleQuantityType = record.ScheduleQuantityType,
                        RequiredQuantity = record.RequiredQuantity,
                        ProductTypeId = productTypeId,
                        WeekDayId = record.WeekDayId.ToArray(),
                        Prefix = prefix,
                        Index = productTypeId,
                        UoM = uom
                    };
                    response.Add(recurringDR);
                }
            }
            if (response.Count() == 0)
            {
                var recurringDR = new RecurringSchedule();
                recurringDR.ScheduleType = (int)ScheduleTypes.Weekly;
                recurringDR.ScheduleQuantityType = (int)ScheduleQuantityType.Quantity;
                recurringDR.Prefix = prefix;
                recurringDR.ProductTypeId = productTypeId;
                recurringDR.UoM = uom;
                response.Add(recurringDR);
            }
            if (productTypeId > 0)
            {
                response.ForEach(x => x.Index = productTypeId);
            }
            var data = RenderRazorViewToString("_PartialRecurringDeliveryRequests", response);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddDefaultScheduleDetailsForBuyer(int Index, int Records, string prefix, int uom)
        {
            var response = new RecurringSchedule();
            using (var tracer = new Tracer("ScheduleBuilderController", "AddDefaultScheduleDetailsForBuyer"))
            {
                response.ScheduleType = (int)ScheduleTypes.Weekly;
                response.ScheduleQuantityType = (int)ScheduleQuantityType.Quantity;
                response.Index = Index;
                response.MaxIndex = Records;
                response.Prefix = prefix;
                response.ProductTypeId = Index;
                response.UoM = uom;
                var data = RenderRazorViewToString("_PartialRecurringDR", response);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        //Get partial view for demand capture
        [HttpPost]
        public async Task<PartialViewResult> GetRetainWindowInfo(CreateDeliveryRequestModel jobInfo)
        {
            using (var tracer = new Tracer("Dashboard", "GetRetainWindowInfo"))
            {
                try
                {
                    CreateDeliveryRequestModel jobWithProductTypes = new CreateDeliveryRequestModel();
                    try
                    {
                        var job = await ContextFactory.Current.GetDomain<JobDomain>().GetProductTypesForJob(UserContext.Id, UserContext.CompanyId, jobInfo.JobId, CurrentUser.BrandedCompanyId);
                        if (job != null && job.ProductTypes.Any())
                        {
                            jobWithProductTypes = job;
                        }
                        foreach (var item in jobWithProductTypes.ProductTypes)
                        {
                            var forcastinPreferance = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Tank, item.AssetId);
                            if (forcastinPreferance.ForcastingServiceSetting.IsEnabled)
                            {
                                TankRetainWindowInfo tankRetainWindowInfo = new TankRetainWindowInfo();
                                await IntializeRetainInfo(jobInfo, item, forcastinPreferance, tankRetainWindowInfo);
                            }
                            item.IsRetainButtonClick = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.Logger.WriteException("DashBoardController", "GetDeliveryRequestView", ex.Message + " JobId :" + jobInfo.JobId, ex);
                    }
                    return PartialView("_PartialBuyerDeliveryRequest", jobWithProductTypes);

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DashBoardController", "GetRetainWindowInfo", ex.Message + " JobId :" + jobInfo.JobId, ex);
                }
            }
            return PartialView("_PartialBuyerDeliveryRequest", jobInfo);
        }

        private static async Task IntializeRetainInfo(CreateDeliveryRequestModel jobInfo, JobProductTypeDetails item, ViewModels.Forcasting.ForcastingPreferenceViewModel forcastinPreferance, TankRetainWindowInfo tankRetainWindowInfo)
        {
            var assetDetails = jobInfo.ProductTypes.Where(top => top.AssetId == item.AssetId).FirstOrDefault();
            if (assetDetails != null && assetDetails.RequiredQuantity > 0)
            {
                tankRetainWindowInfo.Quantity = assetDetails.RequiredQuantity;
                tankRetainWindowInfo.Id = item.AssetId;
                tankRetainWindowInfo.siteId = item.JobDisplayId;
                tankRetainWindowInfo.storageId = item.StorageId;
                tankRetainWindowInfo.tankId = item.TankId;
                IntializeRetainParametersVals(tankRetainWindowInfo, forcastinPreferance);
                var result = await ContextFactory.Current.GetDomain<SalesDomain>().CalculateTankRetainWindowInfo(tankRetainWindowInfo);
                if (result != null && !string.IsNullOrEmpty(result.RetainTime))
                {
                    item.RetainDate = result.RetainDate;
                    item.RetainTime = result.RetainTime;
                    item.StartDate = result.WindowStartDate;
                    item.StartTime = result.WindowStartTime;
                    item.EndDate = result.WindowEndDate;
                    item.EndTime = result.WindowEndTime;
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> CalculateTankDetailsRetainWindowInfo(List<TankRetainWindowInfo> tankRetainWindowInfo)
        {
            var response = new List<TankRetainInfo>();
            using (var tracer = new Tracer("DashboardController", "CalculateTankDetailsRetainWindowInfo"))
            {
                foreach (var item in tankRetainWindowInfo)
                {
                    var forcastinPreferance = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Tank, item.Id);
                    if (forcastinPreferance.ForcastingServiceSetting.IsEnabled)
                    {
                        IntializeRetainParametersVals(item, forcastinPreferance);
                        var result = await ContextFactory.Current.GetDomain<SalesDomain>().CalculateTankRetainWindowInfo(item);
                        if (result != null && !string.IsNullOrEmpty(result.RetainTime))
                        {
                            TankRetainInfo tankRetainInfo = new TankRetainInfo();
                            tankRetainInfo.Id = item.Id;
                            tankRetainInfo.TankName = item.TankName;
                            tankRetainInfo.storageId = item.storageId;
                            tankRetainInfo.siteId = item.siteId;
                            tankRetainInfo.tankId = item.tankId;
                            tankRetainInfo.RetainDate = result.RetainDate;
                            tankRetainInfo.RetainTime = result.RetainTime;
                            tankRetainInfo.WindowStartDate = result.WindowStartDate;
                            tankRetainInfo.WindowStartTime = result.WindowStartTime;
                            tankRetainInfo.WindowEndDate = result.WindowEndDate;
                            tankRetainInfo.WindowEndTime = result.WindowEndTime;
                            response.Add(tankRetainInfo);
                        }
                    }
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        private static void IntializeRetainParametersVals(TankRetainWindowInfo item, ViewModels.Forcasting.ForcastingPreferenceViewModel forcastinPreferance)
        {
            item.startBufferUOM = forcastinPreferance.ForcastingServiceSetting.StartBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.StartBufferUOM;
            item.startBuffer = forcastinPreferance.ForcastingServiceSetting.StartBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.StartBuffer;

            item.endBufferUOM = forcastinPreferance.ForcastingServiceSetting.EndBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.EndBufferUOM;
            item.endBuffer = forcastinPreferance.ForcastingServiceSetting.EndBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.EndBuffer;

            int retainBufferUOM = forcastinPreferance.ForcastingServiceSetting.RetainTimeBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.RetainTimeBufferUOM;
            int retainBuffer = forcastinPreferance.ForcastingServiceSetting.RetainTimeBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.RetainTimeBuffer;

            if (item.startBufferUOM == retainBufferUOM)
            {
                item.maxBuffer = item.startBuffer > retainBuffer ? item.startBuffer : retainBuffer;
                item.maxBufferUOM = item.startBufferUOM == (int)RateOfConsumsionUOM.Hours ? (int)RateOfConsumsionUOM.Hours : (int)RateOfConsumsionUOM.Days;
            }
            else
            {
                if (item.startBufferUOM == (int)RateOfConsumsionUOM.Hours && retainBufferUOM == (int)RateOfConsumsionUOM.Days)
                {
                    item.maxBuffer = retainBuffer;
                    item.maxBuffer = (int)RateOfConsumsionUOM.Days;
                }
                else if (item.startBufferUOM == (int)RateOfConsumsionUOM.Days && retainBufferUOM == (int)RateOfConsumsionUOM.Hours)
                {
                    item.maxBuffer = item.startBuffer;
                    item.maxBuffer = (int)RateOfConsumsionUOM.Days;
                }
            }
        }
    }
}