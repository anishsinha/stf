using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using System.Threading.Tasks;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Core.StringResources;
using System.IO;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Web.Helpers;
using SiteFuel.Exchange.ViewModels.Forcasting;
using SiteFuel.Exchange.ViewModels.Job;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier)]
    public class JobController : BaseController
    {
        // GET: Supplier/Job
        [ActionName("View")]
        [HttpGet]
        public ActionResult Index(JobFilterViewModel jobFilter = null)
        {
            using (var tracer = new Tracer("JobController", "View"))
            {
                var response = new JobFilterViewModel() { Id = UserContext.CompanyId };
                return View("View", response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> JobGrid(JobFilterViewModel jobFilter = null)
        {
            using (var tracer = new Tracer("JobController", "JobGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobDetailsForSupplier(jobFilter, UserContext);
                return new JsonResult
                {
                    Data = response,

                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                //return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            using (var tracer = new Tracer("JobController", "JobDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobStepsForSuperAdminAsync(id, CurrentUser.CompanyId);
                response.IsCreatedByMe = true;
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadJobDetailsFailed);
                }
                else
                {
                    response.Job.ForcastingPreference = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Job, response.Job.Id);
                    response.Job.OnSiteContactUserInfo = await ContextFactory.Current.GetDomain<JobDomain>().GetJobOnsiteContactDetails(response.Job.Id);
                }

                //set Return URL to get back when submit or cancel is clicked from job details tab
                string returnUrl = null;
                if (id > 0)
                {
                    returnUrl = Url.Action("Details", "Job", new { area = "Supplier", Id = id });
                }
                SetReturnUrl(returnUrl);
                return View("JobDetails", response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ShowDetails(int id)
        {
            using (var tracer = new Tracer("JobController", "JobDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetJobStepsForSuperAdminAsync(id, CurrentUser.CompanyId);
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
                    returnUrl = Url.Action("ShowDetails", "Job", new { area = "Supplier", Id = id });
                }
                SetReturnUrl(returnUrl);
                return View("JobDetails", response);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Dispatcher, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public async Task<ActionResult> SaveProductSequence(ProductSequenceViewModel viewModel)
        {
            using (var tracer = new Tracer("JobController", "SaveProductSequence"))
            {
                var response = await ContextFactory.Current.GetDomain<CompanyDomain>().SaveProductSequence(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Dispatcher)]
        public async Task<ActionResult> SaveJob(JobStepsViewModelForSuperAdmin viewModel)
        {
            using (var tracer = new Tracer("JobController", "SaveJob"))
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


                    if (response.StatusCode == Status.Success)
                    {
                        //Save the forcasting setting details.
                        var forcastingResponse = await Saveforcastingdetails(viewModel, response);
                        if (forcastingResponse.StatusCode == Status.Failed)
                        {
                            DisplayCustomMessages((MessageType)forcastingResponse.StatusCode, forcastingResponse.StatusMessage);
                            return View("~/Areas/Supplier/Views/Job/CreateJob.cshtml", viewModel);
                        }
                        else
                        {
                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        }
                        if (viewModel.IsCreatedByMe)
                            return RedirectToAction("Details", "Job", new { area = "Supplier", id = viewModel.Job.Id });
                        else
                            return RedirectToAction("ShowDetails", "Job", new { area = "Supplier", id = viewModel.Job.Id });
                    }
                    else if (response.StatusCode == Status.Failed)
                    {
                        DisplayCustomMessages(MessageType.Error, response.StatusMessage);
                        return View("~/Areas/Supplier/Views/Job/CreateJob.cshtml", viewModel);
                    }
                }

                if (viewModel.IsCreatedByMe)
                    return RedirectToAction("Details", "Job", new { area = "Supplier", id = viewModel.Job.Id });
                else
                    return RedirectToAction("ShowDetails", "Job", new { area = "Supplier", id = viewModel.Job.Id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Dispatcher)]
        public async Task<ActionResult> CreateAsset(AssetViewModel viewModel, HttpPostedFileBase imageFile = null, int jobId = 0)
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
                    }
                    response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().CreateAssetsAsync(UserContext, viewModel, jobId);


                    if (response.StatusCode == Status.Success)
                    {
                        if (viewModel.Type == (int)AssetType.Tank)
                        {
                            var forcastingResponse = await Savetankforcastingdetails(viewModel, response);
                            if (forcastingResponse.StatusCode == Status.Failed)
                            {
                                DisplayCustomMessages((MessageType)forcastingResponse.StatusCode, forcastingResponse.StatusMessage);

                            }
                            else
                            {
                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                                //This code will assign single asset to selected job when added from job details screen (tab)
                                return RedirectToAction("Details", "Job", new { area = "Supplier", id = jobId });
                            }
                        }
                        else
                        {
                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                            //This code will assign single asset to selected job when added from job details screen (tab)
                            return RedirectToAction("Details", "Job", new { area = "Supplier", id = jobId });
                        }

                    }
                    else
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        viewModel.StatusMessage = response.StatusMessage;
                    }
                }

            }
            return View(viewModel);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Dispatcher)]
        public async Task<ActionResult> CreateAsset(int jobId, int type = 1)
        {
            var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
            var response = await assetDomain.GetAssetAsync(0, jobId);
            if (type == (int)AssetType.Tank)
            {
                response.AssetAdditionalDetail.DipTestMethod = await assetDomain.GetDefaultDiptest(CurrentUser.CompanyId);
            }
            response.JobId = jobId;
            response.Type = type;
            response.AssetAdditionalDetail.Type = type;
            response.AssetAdditionalDetail.TankAcceptDelivery = ContextFactory.Current.GetDomain<AssetDomain>().AddDefaultTankAcceptDays();
            response.MaxAllowedFileSize = SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize;
            response.ForcastingPreference = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Job, jobId);
            response.ForcastingPreference.IsEditable = true;
            return PartialView("CreateAsset", response);
        }

        public async Task<JsonResult> GetNewsfeed(int entityId, int currentPage, int latestId = 0)
        {
            var response = await ContextFactory.Current.GetDomain<NewsfeedDomain>().GetNewsfeed(UserContext, EntityType.Job, entityId, currentPage, latestId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> AssignAssetsTanksToJob(AssetFilterViewModel assetFilter = null)
        {
            using (var tracer = new Tracer("JobController", "AssignAssetsToJob"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetTpoAssetGridAsync(UserContext, assetFilter.JobId, (int)assetFilter.Type);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Dispatcher)]
        public async Task<JsonResult> AssignAssets(List<int> assets, int jobId)
        {
            using (var tracer = new Tracer("AssetController", "AssignAssets"))
            {

                var response = await ContextFactory.Current.GetDomain<AssetDomain>().AssignAssetsToJobAsync(UserContext, jobId, assets, true);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return Json(response.StatusCode, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditLocationToRegion(JobToRegionAssignViewModel jobInfo)
        {
            var response = new JobToRegionAssignViewModel();
            try
            {
                if (jobInfo != null && jobInfo.JobId > 0)
                {
                    response.CompanyId = UserContext.CompanyId;
                    response.JobId = jobInfo.JobId;
                    response.JobName = jobInfo.JobName;
                    response.RegionId = jobInfo.RegionId;
                    response.UpdatedBy = UserContext.Id;
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobController", "EditLocationToRegion", ex.Message, ex);
            }
            return PartialView("_PartialJobRegionAssignment", response);
        }
        [HttpPost]
        public ActionResult EditLocationToRoute(JobToRegionAssignViewModel jobInfo)
        {
            var response = new JobToRegionAssignViewModel();
            try
            {
                if (jobInfo != null && jobInfo.JobId > 0)
                {
                    response.CompanyId = UserContext.CompanyId;
                    response.JobId = jobInfo.JobId;
                    response.JobName = jobInfo.JobName;
                    response.RegionId = jobInfo.RegionId;
                    response.RouteId = jobInfo.RouteId;
                    response.UpdatedBy = UserContext.Id;
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobController", "EditLocationToRoute", ex.Message, ex);
            }
            return PartialView("_PartialJobRouteAssignment", response);
        }
        [HttpPost]
        public async Task<ActionResult> SaveRegionAssignmentForJob(JobToRegionAssignViewModel jobToAssign)
        {
            var response = new StatusViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().AssignJobToRegion(jobToAssign);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobController", "EditLocationToRegion", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateJobType(int id, bool isRetail)
        {
            using (var tracer = new Tracer("JobController", "UpdateJobType"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().UpdateJobType(id, isRetail);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EditLocationToCarrier(JobToCarrierAssignViewModel jobInfo)
        {
            var response = new JobToCarrierAssignViewModel();
            try
            {
                if (jobInfo != null && jobInfo.JobId > 0)
                {
                    response.CompanyId = UserContext.CompanyId;
                    response.JobId = jobInfo.JobId;
                    response.JobName = jobInfo.JobName;
                    response.CarrierId = jobInfo.CarrierId;
                    response.CarrierName = jobInfo.CarrierName;
                    response.UpdatedBy = UserContext.Id;
                    response.IsCreateFreightOnlyOrder = false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobController", "EditLocationToCarrier", ex.Message, ex);
            }
            return PartialView("_PartialJobCarrierAssignment", response);
        }

        [HttpPost]
        public async Task<ActionResult> SaveCarrierAssignmentForJob(JobToCarrierAssignViewModel jobToAssign)
        {
            var response = new StatusViewModel();
            try
            {
                var existingCarrierId = GetCarrierForJob(jobToAssign.JobId, UserContext.CompanyId);
                if (existingCarrierId != jobToAssign.CarrierId || existingCarrierId == 0)
                {
                    var jobCarrierDetails = BindToSupplierCarrierViewModel(jobToAssign, UserContext);
                    var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                    response = await fsDomain.AssignCarrierToJob(jobCarrierDetails);
                    if (response.StatusCode == Status.Success)
                    {
                        if (existingCarrierId == 0)
                        {
                            if (jobToAssign.IsCreateFreightOnlyOrder)
                            {
                                // assign the existing order to the assigned carrier, once the supplier update the carrier for the location.
                                var supplierCarriers = new List<SupplierCarrierViewModel>() { jobCarrierDetails };
                                await ContextFactory.Current.GetDomain<CarrierDomain>().createFreightOrdersForAssignedCarrier(supplierCarriers, UserContext);
                            }
                        }
                        else if (existingCarrierId != jobToAssign.CarrierId)
                        {
                            // update the existing carrier for the job and also close the existing order for the previous carrier assigned.
                            var editFreightOnlyOrders = new EditFreightOnlyOrderViewModel()
                            {
                                CarrierCompanyId = existingCarrierId,
                                removedJobsIds = new List<int>() { jobToAssign.JobId }
                            };
                            await ContextFactory.Current.GetDomain<CarrierDomain>().EditFreightOnlyOrders(editFreightOnlyOrders, UserContext);
                            if (jobToAssign.IsCreateFreightOnlyOrder)
                            {
                                var supplierCarriers = new List<SupplierCarrierViewModel>() { jobCarrierDetails };
                                await ContextFactory.Current.GetDomain<CarrierDomain>().createFreightOrdersForAssignedCarrier(supplierCarriers, UserContext);
                            }

                        }
                        // update the job carrier details.
                        await ContextFactory.Current.GetDomain<CarrierDomain>().UpdateJobCarrierDetails(jobCarrierDetails, UserContext.Id, UserContext.CompanyId, true);
                        response.StatusMessage = Resource.successMsgCarrierAssignment;
                    }
                }
                else
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMsgCarrierAssignment;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMsgCarrierAssignment;
                LogManager.Logger.WriteException("JobController", "SaveCarrierAssignmentForJob", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private static SupplierCarrierViewModel BindToSupplierCarrierViewModel(JobToCarrierAssignViewModel jobToAssign, UserContext userContext)
        {
            var companyInfo = CommonHelperMethods.GetCompanyDetailsByJobId(jobToAssign.JobId);
            return new SupplierCarrierViewModel()
            {
                Carrier = new DropdownDisplayItem()
                {
                    Id = jobToAssign.CarrierId,
                    Name = jobToAssign.CarrierName
                },

                Jobs = new List<CarrierJobViewModel>()
                         {
                             new CarrierJobViewModel()
                             {
                                 Job = new JobWithEmails()
                                 {
                                     Id = jobToAssign.JobId,
                                     Name = jobToAssign.JobName
                                 },
                                 BuyerCompanyId = companyInfo.Item1,
                                 BuyerCompanyName = companyInfo.Item2,
                             }
                        },
                CreatedBy = userContext.Id,
                CreatedOn = DateTimeOffset.Now,
                IsActive = true,
                IsDeleted = false,
                SupplierCompanyId = userContext.CompanyId,
                SupplierCompanyName = userContext.Name
            };
        }
        [HttpPost]
        public async Task<ActionResult> SaveRouteAssignmentForJob(JobToRegionAssignViewModel jobToAssign)
        {
            var response = new StatusViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().AssignJobToRoute(jobToAssign);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobController", "SaveRouteAssignmentForJob", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        private static int GetCarrierForJob(int jobId, int companyId)
        {
            return ContextFactory.Current.GetDomain<CarrierDomain>().GetCarrierByJobId(jobId, companyId);
        }
        public ActionResult SetCompanyOwnedLocation(int Id, int buyerCompanyId, bool companyOwnedLocation)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                using (var tracer = new Tracer("Dashboard", "SetCompanyOwnedLocation"))
                {
                    response = ContextFactory.Current.GetDomain<JobDomain>().SetCompanyOwnedLocation(UserContext.CompanyId, buyerCompanyId, Id, companyOwnedLocation, OrderCreationMethod.FromTPO, UserContext);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobController", "SetCompanyOwnedLocation", ex.Message, ex);

            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetProductSequencing(ProductSequencingCreationMethod sequenceMethod, int jobId = 0)
        {
            using (var tracer = new Tracer("JobController", "GetProductSequencing"))
            {
                var response = await ContextFactory.Current.GetDomain<CompanyDomain>().GetProductSequence(UserContext.CompanyId, sequenceMethod, jobId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetProductSequenceTypes(ProductSequencingCreationMethod sequenceMethod, ProductSequenceType sequenceType = ProductSequenceType.Product, int jobId = 0)
        {
            var response = await ContextFactory.Current.GetDomain<CompanyDomain>().GetProductDisplaySequenceTypes(CurrentUser.CompanyId, sequenceMethod, sequenceType, jobId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        private async Task<StatusViewModel> Saveforcastingdetails(JobStepsViewModelForSuperAdmin viewModel, StatusViewModel response)
        {
            var forcastingResponse = await SaveforcastingSettingJobLevel(viewModel.Job.ForcastingPreference, response);
            return forcastingResponse;
        }
        private async Task<StatusViewModel> SaveforcastingSettingJobLevel(ForcastingPreferenceViewModel viewModel, StatusViewModel response)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            statusViewModel.StatusCode = Status.Success;
            if (viewModel != null && viewModel.ForcastingServiceSetting != null && response.StatusCode == (int)Status.Success)
            {
                statusViewModel = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().SaveForeCastingPreferanceSetting(viewModel, UserContext, (int)ForcastingSettingLevel.Job, response.EntityId, 0, response.CustomerCompanyId);
            }
            return statusViewModel;
        }
        private async Task<StatusViewModel> Savetankforcastingdetails(AssetViewModel viewModel, StatusViewModel response)
        {
            var forcastingResponse = await SaveforcastingSettingTankLevel(viewModel.ForcastingPreference, response);
            return forcastingResponse;
        }
        private async Task<StatusViewModel> SaveforcastingSettingTankLevel(ForcastingPreferenceViewModel viewModel, StatusViewModel response)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            statusViewModel.StatusCode = Status.Success;
            if (viewModel != null && viewModel.ForcastingServiceSetting != null && response.StatusCode == (int)Status.Success)
            {
                statusViewModel = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().SaveForeCastingPreferanceSetting(viewModel, UserContext, (int)ForcastingSettingLevel.Tank, response.EntityId, 0, response.CustomerCompanyId);
            }
            return statusViewModel;
        }

        [HttpPost]
        public async Task<JsonResult> SaveDistanceCoveredForJobLocation(int JobId, string DistanceCovered)
        {
            var response = new StatusViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().UpdateDistanceCovered(JobId, DistanceCovered);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobController", "SaveDistanceCoveredForJobLocation", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> EditLocationToInventory(JobGridSupplierViewModel jobInfo)
        {
            var response = new JobGridSupplierViewModel();
            try
            {
                if (jobInfo != null)
                {
                    response.JobID = jobInfo.JobID;
                    response.JobName = jobInfo.JobName;
                    response.InventoryDataCaptureType = jobInfo.InventoryDataCaptureType;
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobController", "EditLocationToInventory", ex.Message, ex);
            }
            return PartialView("_PartialJobInventoryAssignment", response);
        }
        public async Task<ActionResult> SaveInventoryAssignmentForJob(JobGridSupplierViewModel jobToAssign)
        {
            var response = new StatusViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<JobDomain>().SaveInventoryTypeForJob(UserContext, jobToAssign);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobController", "SaveInventoryAssignmentForJob", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]

        public async Task<ActionResult> MarinePortsAndVessels()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> UpdateJobContactDetails(OnsiteJobUserViewModel person)
        {
            var response = new StatusViewModel();
            if (person != null)
            {
                response = await ContextFactory.Current.GetDomain<JobDomain>().UpdateOnsiteJobContactDetails(person);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}