using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SiteFuel.Exchange.Logger;
using System.Collections.Generic;
using System.Linq;
using SiteFuel.Exchange.Web.Areas.Dispatcher.Models;
using System.Net;
using SiteFuel.Exchange.ViewModels.Job;
using SiteFuel.Exchange.ViewModels.Forcasting;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer)]
    public class JobController : BaseController
    {
        [HttpGet]
        [ActionName("View")]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        public ActionResult Index(int jobId = 0, JobFilterType filter = JobFilterType.All)
        {
            using (var tracer = new Tracer("JobController", "Index"))
            {
                var response = ContextFactory.Current.GetDomain<JobDomain>().GetJobFilterAsync(jobId, filter);
                return View("View", response);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        public async Task<ActionResult> JobGrid(JobFilterViewModel jobFilter = null)
        {
            using (var tracer = new Tracer("JobController", "JobGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobGridAsync(CurrentUser.Id, jobFilter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson)]
        public async Task<ActionResult> Close(int id)
        {
            using (var tracer = new Tracer("JobController", "Close"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().CloseJobAsync(UserContext, id, CurrentUser.Id);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Job", new { area = "Buyer", id = id });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin)]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public async Task<ActionResult> Create(bool isThisFromFuelRequest = false, bool isThisFromQuoteRequest = false)
        {
            using (var tracer = new Tracer("JobController", "Create"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobStepsAsync(0, CurrentUser.CompanyId);
                response.IsJobCreationFromFuelRequest = isThisFromFuelRequest;
                response.IsJobCreationFromQuoteRequest = isThisFromQuoteRequest;
                response.Job.ForcastingPreference = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Job, response.Job.Id);

                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Save(JobStepsViewModel viewModel)
        {
            using (var tracer = new Tracer("JobController", "Save"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (viewModel.Job.IsMarine)
                            viewModel.Job.Country.UoM = viewModel.Job.MarineUom;
                        viewModel.UserId = CurrentUser.Id;
                        viewModel.Job.StatusId = (int)JobStatus.Open;
                        if (viewModel.Job.IsVarious)
                        {

                            viewModel.Job.LocationType = JobLocationTypes.Various;
                        }
                        if (string.IsNullOrWhiteSpace(viewModel.Job.CountyName))
                        {
                            viewModel.Job.CountyName = viewModel.Job.City;
                        }
                        if (viewModel.Job.DeliveryDaysList.Count > 0)
                        {
                            //foreach(var delivery in viewModel.Job.DeliveryDaysList)
                            //{
                            //    if(delivery.DeliveryDays.Count==0)
                            //    {
                            //        viewModel.Job.DeliveryDaysList.Remove(delivery);
                            //    }
                            //}
                        }

                        SaveJobStatusViewModel response;
                        viewModel.Job.SiteImage = await SetImageDataToBolb(viewModel.Job.SiteImage, viewModel.Job.SiteImageFiles, BlobContainerType.JobFilesUpload);
                        viewModel.Job.AdditionalImage.SiteImage = await SetImageDataToBolb(viewModel.Job.AdditionalImage.SiteImage, viewModel.Job.AdditionalImage.SiteImageFiles, BlobContainerType.JobFilesUpload);

                        if (viewModel.Job.Id > 0)
                        {
                            response = await ContextFactory.Current.GetDomain<JobDomain>().UpdateJobStepsAsync(UserContext, viewModel);
                            response.JobId = viewModel.Job.Id;
                        }
                        else
                        {
                            viewModel.Job.CreatedBy = CurrentUser.Id;
                            response = await ContextFactory.Current.GetDomain<JobDomain>().SaveJobStepsAsync(UserContext, viewModel);
                        }
                        //Save the forcasting setting details.
                        var forcastingSettingResponse = await SaveforcastingSettingJobLevel(viewModel.Job.ForcastingPreference, response);
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        if (response.StatusCode == Status.Success && forcastingSettingResponse.StatusCode == Status.Success)
                        {
                            if (response.IsAssetTrackingEnabled)
                            {
                                //return RedirectToAction("AssignAssets", "Job", new { area = "Buyer", id = viewModel.Job.Id, isThisFromFuelRequest = viewModel.IsJobCreationFromFuelRequest });
                                return RedirectToAction("Details", "Job", new { area = "Buyer", id = viewModel.Job.Id });
                            }
                            else if (viewModel.IsJobCreationFromFuelRequest)
                            {
                                return RedirectToAction("Create", "FuelRequest", new { area = "Buyer", jobId = viewModel.Job.Id });
                            }
                            else if (viewModel.IsJobCreationFromQuoteRequest)
                            {
                                return RedirectToAction("Create", "Quote", new { area = "Buyer", jobId = viewModel.Job.Id });
                            }

                            return RedirectToAction("Details", "Job", new { area = "Buyer", id = viewModel.Job.Id });
                        }
                        else if (forcastingSettingResponse.StatusCode == Status.Failed)
                        {
                            DisplayCustomMessages(MessageType.Error, forcastingSettingResponse.StatusMessage);
                            return RedirectToAction("Details", "Job", new { area = "Buyer", id = viewModel.Job.Id });
                        }
                        else if (response.StatusCode == Status.Warning)
                        {
                            DisplayCustomMessages(MessageType.Error, response.StatusMessage);
                            return RedirectToAction("Details", "Job", new { area = "Buyer", id = viewModel.Job.Id });
                        }

                    }
                    catch (Exception ex)
                    {
                        LogManager.Logger.WriteException("JobController", "Save", ex.Message, ex);
                    }
                }
                return View("Create", viewModel);
            }
        }

        private async Task<StatusViewModel> SaveforcastingSettingJobLevel(ForcastingPreferenceViewModel viewModel, SaveJobStatusViewModel response)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            statusViewModel.StatusCode = Status.Failed;
            if (viewModel != null && viewModel.ForcastingServiceSetting != null && response.StatusCode == (int)Status.Success)
            {
                statusViewModel = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().SaveForeCastingPreferanceSetting(viewModel, UserContext, (int)ForcastingSettingLevel.Job, response.JobId);
            }
            else
            {
                statusViewModel.StatusMessage = response.StatusMessage;
            }
            return statusViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Draft(JobStepsViewModel viewModel)
        {
            using (var tracer = new Tracer("JobController", "Draft"))
            {
                if (ModelState.IsValid)
                {
                    if (viewModel.Job.IsMarine)
                        viewModel.Job.Country.UoM = viewModel.Job.MarineUom;
                    viewModel.UserId = CurrentUser.Id;

                    StatusViewModel response;
                    await SaveJobImagesDraftMode(viewModel);
                    if (viewModel.Job.Id > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<JobDomain>().UpdateJobStepsAsync(UserContext, viewModel);
                    }
                    else
                    {
                        viewModel.Job.CreatedBy = CurrentUser.Id;
                        response = await ContextFactory.Current.GetDomain<JobDomain>().SaveJobStepsAsync(UserContext, viewModel);
                    }
                    if (response.StatusCode == Status.Success)
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        return RedirectToAction("Details", "Job", new { area = "Buyer", id = viewModel.Job.Id });
                    }
                    else
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    }
                }
                return View("Create", viewModel);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        public async Task<ActionResult> Details(int id)
        {
            using (var tracer = new Tracer("JobController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobStepsAsync(id);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadJobDetailsFailed);
                }
                else
                {
                    response.Job.ForcastingPreference = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Job, response.Job.Id);
                }
                //set Return URL to get back when submit or cancel is clicked from job details tab
                string returnUrl = null;
                if (id > 0)
                {
                    returnUrl = Url.Action("Details", "Job", new { area = "Buyer", Id = id });
                }
                SetReturnUrl(returnUrl);
                return View("Create", response);
            }
        }
        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin)]
        public async Task<ActionResult> Delete(int id)
        {
            using (var tracer = new Tracer("JobController", "Delete"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().DeleteJobAsync(id, CurrentUser.Id);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                if (response.StatusCode == Status.Failed)
                {
                    return RedirectToAction("Details", "Job", new { area = "Buyer", id = id });
                }
                else
                {
                    return RedirectToAction("View", "Job", new { area = "Buyer" });
                }
            }
        }

        [HttpGet]
        public ActionResult PartialMapView(JobFilterViewModel jobFilter = null)
        {
            using (var tracer = new Tracer("JobController", "PartialMapView"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<JobDomain>().GetMapDataAsync(CurrentUser.Id, jobFilter)).Result;
                return PartialView("_PartialMapView", response);
            }
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

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult GetContactPerson()
        {
            return PartialView("_PartialContactPerson", new JobContactViewModel() { CompanyId = CurrentUser.CompanyId });
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult GetSubcontractor()
        {
            return PartialView("_PartialSubcontractor", new SubcontractorViewModel());
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> ReOpen(int jobId)
        {
            using (var tracer = new Tracer("JobController", "ReOpen"))
            {
                var reOpenJob = await ContextFactory.Current.GetDomain<JobDomain>().GetReOpenAsync(jobId);
                return View(reOpenJob);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> ReOpen(ReOpenJobViewModel viewModel)
        {
            using (var tracer = new Tracer("JobController", "ReOpen(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<JobDomain>().ReOpenJobAsync(UserContext, viewModel, CurrentUser.Id);

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Failed)
                    {
                        return RedirectToAction("ReOpen", "Job", new { area = "Buyer", jobId = viewModel.JobId });
                    }

                    return RedirectToAction("Details", "Job", new { area = "Buyer", id = viewModel.JobId });
                }

                return View(viewModel);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public ActionResult AssignAssets(bool isThisFromFuelRequest = false)
        {
            return View();
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<JsonResult> AssignAssetsToJob(AssetFilterViewModel assetFilter = null)
        {
            var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetGridAsync(UserContext, assetFilter, true);
            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<JsonResult> AssignAssets(List<int> assets, int jobId)
        {
            using (var tracer = new Tracer("JobController", "AssignAssets"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().AssignAssetsToJobAsync(UserContext, jobId, assets);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return Json(response.StatusCode, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin)]
        public async Task<ActionResult> CreateAsset(int jobId, int type = 1)
        {
            var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetAsync(0, jobId);
            response.JobId = jobId;
            response.CompanyId = CurrentUser.CompanyId;
            response.Type = type;
            response.AssetAdditionalDetail.Type = type;
            response.AssetAdditionalDetail.TankAcceptDelivery = ContextFactory.Current.GetDomain<AssetDomain>().AddDefaultTankAcceptDays();
            response.MaxAllowedFileSize = SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize;
            if (type == 2)
                response.ForcastingPreference = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Job, jobId);
            return PartialView("CreateAsset", response);
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadJobAssets(int jobId, HttpPostedFileBase csvFile)
        {
            if (csvFile != null && csvFile.ContentLength > 0)
            {
                if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                {
                    string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                    var csvSunbeltAssetsFilePath = Server.MapPath("~\\Content\\Job_Assets_Sample.csv");
                    var csvSfxAssetsFilePath = Server.MapPath("~\\Content\\Asset_Bulkupload_Template.csv");

                    var jobDomain = ContextFactory.Current.GetDomain<JobDomain>();
                    bool isSFXAssetsCsv = false;
                    var response = jobDomain.ValidateJobAssetCsvHeader(csvText, csvSunbeltAssetsFilePath, csvSfxAssetsFilePath, ref isSFXAssetsCsv);

                    if (response.StatusCode == Status.Success)
                    {
                        await jobDomain.UploadJobAssetFileToBlob(UserContext, jobId, csvFile.InputStream, csvFile.FileName);
                        if (isSFXAssetsCsv)
                        {
                            var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
                            response = await assetDomain.SaveBulkAssetsAsync(csvText.Trim(), CurrentUser.Id, CurrentUser.CompanyId, jobId);
                        }
                        else
                        {
                            response = await jobDomain.SaveJobBulkAssetsAsync(csvText.Trim(), UserContext, jobId);
                        }
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        if (response.StatusCode != Status.Failed)
                        {
                            if (jobId > 0)
                            {
                                return RedirectToAction("Details", "Job", new { area = "Buyer", id = jobId });
                            }
                            return RedirectToAction("View", "Asset", new { area = "Buyer" });
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
                DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
            }
            if (jobId > 0)
            {
                return RedirectToAction("Details", "Job", new { area = "Buyer", id = jobId });
            }
            return RedirectToAction("View", "Asset", new { area = "Buyer" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin)]
        public async Task<ActionResult> CreateAsset(AssetViewModel viewModel, HttpPostedFileBase imageFile = null, int jobId = 0, bool isThisFromFuelRequest = false)
        {
            using (var tracer = new Tracer("JobController", "CreateAsset(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    StatusViewModel response;
                    viewModel.UserId = CurrentUser.Id;
                    viewModel.CreatedDate = DateTimeOffset.Now;

                    if (imageFile != null)
                    {
                        var reader = new BinaryReader(imageFile.InputStream);
                        byte[] imageData = reader.ReadBytes((int)imageFile.InputStream.Length);
                        viewModel.Image = new ImageViewModel() { Data = imageData };
                        await viewModel.Image.UploadImageToAzureBlobService(ApplicationConstants.AssetDropImageFileNamePrefix, BlobContainerType.JobFilesUpload);
                    }

                    response = await ContextFactory.Current.GetDomain<AssetDomain>().SavejobXAssetAsync(UserContext, viewModel, jobId);

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Success)
                    {
                        //This code will assign single asset to selected job when added from job details screen (tab)
                        return RedirectToAction("Details", "Job", new { area = "Buyer", id = jobId });

                        //if (viewModel.Type == (int)AssetType.Tank)
                        //{
                        //    var forcastingResponse = await Savetankforcastingdetails(viewModel, response);
                        //    if (forcastingResponse.StatusCode == Status.Failed)
                        //    {
                        //        DisplayCustomMessages((MessageType)forcastingResponse.StatusCode, forcastingResponse.StatusMessage);
                        //    }
                        //    else
                        //    {
                        //        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        //        //This code will assign single asset to selected job when added from job details screen (tab)
                        //        return RedirectToAction("Details", "Job", new { area = "Buyer", id = jobId });
                        //    }
                        //}
                        //else
                        //{
                        //    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        //    //This code will assign single asset to selected job when added from job details screen (tab)
                        //    return RedirectToAction("Details", "Job", new { area = "Buyer", id = jobId });
                        //}
                    }
                }

                return View(viewModel);
            }
        }

        public ActionResult CancelCreateAsset(int Id, bool isThisCalledFromJobDetails = false)
        {
            if (isThisCalledFromJobDetails)
            {
                return RedirectToAction("Details", "Job", new { area = "Buyer", id = Id });
            }
            else
            {
                return RedirectToAction("AssignAssets", "Job", new { area = "Buyer", id = Id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin)]
        public async Task<ActionResult> AssetBulkUpload(AssetViewModel viewModel, HttpPostedFileBase csvFile, bool isThisFromFuelRequest = false)
        {
            using (var tracer = new Tracer("JobController", "AssetBulkUpload"))
            {
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                    {
                        string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                        var csvFilePath = Server.MapPath("~/Content/Asset_Bulkupload_Template.csv");

                        var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
                        var response = assetDomain.ValidateCsvHeader(csvText, csvFilePath);
                        if (response.StatusCode == Status.Success)
                        {
                            response = await assetDomain.SaveBulkAssetsAsync(csvText.Trim(), CurrentUser.Id, CurrentUser.CompanyId);

                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                            if (response.StatusCode != Status.Failed)
                            {
                                return RedirectToAction("AssignAssets", "Job", new { area = "Buyer", id = Convert.ToInt32(RouteData.Values["id"]), isThisFromFuelRequest = isThisFromFuelRequest });
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
                    DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
                }

                return RedirectToAction("CreateAsset", "Job", new { area = "Buyer", jobId = Convert.ToInt32(RouteData.Values["id"]) });
            }
        }

        [HttpGet]
        public ActionResult ActivityReport(int jobId)
        {
            return PartialView("_PartialJobActivity", jobId);
        }

        [HttpGet]
        public async Task<ActionResult> ActivityReportGrid(int jobId, string fromDate, string toDate)
        {
            using (var tracer = new Tracer("JobController", "ActivityReportGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetActivityReportAsync(CurrentUser.CompanyId, jobId, fromDate, toDate);
                return new JsonResult { Data = response, MaxJsonLength = int.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpGet]
        public ActionResult AuditReport(int jobId)
        {
            return PartialView("_PartialJobAudit", jobId);
        }

        [HttpPost]
        public async Task<ActionResult> AuditReportGrid(AuditDataTableViewModel auditDataViewModel)
        {
            using (var tracer = new Tracer("JobController", "AuditReportGrid"))
            {
                var dataTableSearchModel = new DataTableSearchModel(auditDataViewModel);
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetAuditReportAsync(auditDataViewModel, dataTableSearchModel);
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].DropDetail.TotalCount;
                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = auditDataViewModel.draw,
                        data = response,
                        recordsTotal = totalCount,
                        recordsFiltered = totalCount
                    },
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public ActionResult Resale(int id = 0)
        {
            var model = new ResaleFilterViewModel();
            model.Id = id;
            return View(model);
        }

        public ActionResult JobResale(int id = 0)
        {
            var model = new ResaleFilterViewModel();
            model.Id = id;
            return PartialView("Resale", model);
        }

        public ActionResult ResaleGrid(int id = 0, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("JobController", "ResaleGrid"))
            {
                var response = ContextFactory.Current.GetDomain<JobDomain>().GetResaleGridData(CurrentUser.Id, CurrentUser.CompanyId, id, currency, countryId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetNewsfeed(int entityId, int currentPage, int latestId = 0)
        {
            var response = await ContextFactory.Current.GetDomain<NewsfeedDomain>().GetNewsfeed(UserContext, EntityType.Job, entityId, currentPage, latestId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetJobById(int jobId = 0)
        {
            using (var tracer = new Tracer("JobController", "GetJobById"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobByIdAsync(jobId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Buyer, UserRoles.BuyerAdmin)]
        public JsonResult GetJobsForOffer(int offerPricingId)
        {
            using (var tracer = new Tracer("JobController", "GetJobsForOffer"))
            {
                var response = ContextFactory.Current.GetDomain<JobDomain>().GetJobsForOfferPricing(CurrentUser.CompanyId, offerPricingId, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Buyer, UserRoles.BuyerAdmin)]

        public ActionResult BuyerWallyBoard()
        {
            return View("BuyerWallyBoard");
        }
        [HttpPost]
        public async Task<JsonResult> GetJobLocationDetails(string jobList = "", string inventoryCaptureTypeIds = "")
        {
            var result = new ResponseMessage();
            var response = await ContextFactory.Current.GetDomain<DispatcherDomain>().GetJobLocationDetails(CurrentUser.CompanyId, true, jobList, inventoryCaptureTypeIds);
            if (response.jobLocationDetails.Any())
                result.StatusCode = HttpStatusCode.Found;
            else
                result.StatusCode = HttpStatusCode.NotFound;
            result.Data = response;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSupplierCarrierForjobs(List<int> jobIds)
        {
            var result = new SupplierCarrierInfoDDL();
            var jobDomain = ContextFactory.Current.GetDomain<JobDomain>();
            result = jobDomain.GetSuppliersCarriersForJob(jobIds, UserContext.CompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //private async Task<StatusViewModel> Savetankforcastingdetails(AssetViewModel viewModel, StatusViewModel response)
        //{
        //    var forcastingResponse = await SaveforcastingSettingTankLevel(viewModel.ForcastingPreference, response);
        //    return forcastingResponse;
        //}
        //private async Task<StatusViewModel> SaveforcastingSettingTankLevel(ForcastingPreferenceViewModel viewModel, StatusViewModel response)
        //{
        //    StatusViewModel statusViewModel = new StatusViewModel();
        //    statusViewModel.StatusCode = Status.Success;
        //    if (viewModel != null && viewModel.ForcastingServiceSetting != null && response.StatusCode == (int)Status.Success)
        //    {
        //        statusViewModel = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().SaveForeCastingPreferanceSetting(viewModel, UserContext, (int)ForcastingSettingLevel.Tank, response.EntityId, 0, response.CustomerCompanyId);
        //    }
        //    return statusViewModel;
        //}
        [HttpPost]
        public async Task<ActionResult> BulkUploadJobs()
        {
            var file = Request.Files;
            var response = new StatusViewModel();
            if (Request.Files != null && Request.Files.Count > 0)
            {
                var csvFile = file[0];
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (csvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            var csvJobsFilePath = Server.MapPath("~\\Content\\Job_Bulkupload_Template.csv");
                            
                            response = ContextFactory.Current.GetDomain<JobDomain>().ValidateJobsCsvHeader(csvText, csvJobsFilePath, UserContext, CompanyType.Buyer);
                            if (response.StatusCode == Status.Success)
                            {
                                response = await ContextFactory.Current.GetDomain<JobDomain>().UploadJobFileToBlob(UserContext, csvFile.InputStream, csvFile.FileName, CompanyType.Buyer);
                            }
                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
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
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #region PrivateMethod
        private async Task SaveJobImagesDraftMode(JobStepsViewModel viewModel)
        {
            viewModel.Job.SiteImage = await SetImageDataToBolb(viewModel.Job.SiteImage, viewModel.Job.SiteImageFiles, BlobContainerType.JobFilesUpload);
            viewModel.Job.AdditionalImage.SiteImage = await SetImageDataToBolb(viewModel.Job.AdditionalImage.SiteImage, viewModel.Job.AdditionalImage.SiteImageFiles, BlobContainerType.JobFilesUpload);
        }
        #endregion PrivateMethod
    }
}