using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
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
using Newtonsoft.Json;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.ViewModels.Asset;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.ViewModels.Forcasting;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    public class AssetController : BaseController
    {
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int id = 0, AssetFilterType filter = AssetFilterType.All)
        {
            using (var tracer = new Tracer("AssetController", "Index"))
            {
                RemoveReturnUrl();
                ViewBag.JobId = id;
                var response = ContextFactory.Current.GetDomain<AssetDomain>().GetAssetFilterAsync(id, CurrentUser.CompanyId, filter);
                return View("View", response);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        public PartialViewResult AssetDetails(int Id, AssetFilterType filter = AssetFilterType.All, bool isJobDetails = false)
        {
            using (var tracer = new Tracer("AssetController", "AssetDetails"))
            {
                ViewBag.JobId = Id;
                var response = ContextFactory.Current.GetDomain<AssetDomain>().GetAssetFilterAsync(Id, CurrentUser.CompanyId, filter);
                response.IsJobDetails = isJobDetails;
                return PartialView("_PartialAssetTankDetails", response);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        [HttpGet]
        public async Task<ActionResult> AssetGrid(AssetFilterViewModel assetFilter = null)
        {
            using (var tracer = new Tracer("AssetController", "AssetGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetGridAsync(UserContext, assetFilter,false,CurrentUser.BrandedCompanyId);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpGet]
        public ActionResult BulkUpload()
        {
            return RedirectToAction("Create", "Asset", new { area = "Buyer" });
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BulkUpload(AssetViewModel viewModel, HttpPostedFileBase csvFile)
        {
            using (var tracer = new Tracer("AssetController", "BulkUpload"))
            {
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                    {
                        string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                        var csvFilePath = Server.MapPath("~\\Content\\Asset_Bulkupload_Template.csv");

                        var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
                        var response = assetDomain.ValidateCsvHeader(csvText, csvFilePath);
                        if (response.StatusCode == Status.Success)
                        {
                            response = await assetDomain.SaveBulkAssetsAsync(csvText.Trim(), CurrentUser.Id, CurrentUser.CompanyId);
                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                            if (response.StatusCode != Status.Failed)
                            {
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

                return RedirectToAction("Create", "Asset", new { area = "Buyer" });
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TankBulkUpload(AssetViewModel viewModel, HttpPostedFileBase csvFile, int jobId)
        {
            using (var tracer = new Tracer("AssetController", "TankBulkUpload"))
            {
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (csvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            var csvFilePath = Server.MapPath("~\\Content\\Tank_BulkUpload_Template.csv");

                            var tankBulkUploadDomain = ContextFactory.Current.GetDomain<TankBulkUploadDomain>();
                            var response = await tankBulkUploadDomain.ValidateTankBulkFile(csvText, csvFilePath, UserContext, CompanyType.Buyer, jobId);
                            if (response.StatusCode == Status.Success)
                            {
                                response = await tankBulkUploadDomain.UploadTankFileToBlob(UserContext, csvFile.InputStream, csvFile.FileName, CompanyType.Buyer);
                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                                if (response.StatusCode != Status.Failed)
                                {
                                    if (jobId > 0)
                                        return RedirectToAction("Details", "Job", new { area = "Buyer", id = jobId });
                                    else
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
                        DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                    }
                }
                else
                {
                    DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
                }

                if (jobId > 0)
                    return RedirectToAction("Details", "Job", new { area = "Buyer", id = jobId });
                else
                    return RedirectToAction("View", "Asset", new { area = "Buyer" });
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpGet]
        public async Task<ActionResult> Create(int id = 0, int type = 1, bool isJobDetails = false)
        {
            using (var tracer = new Tracer("AssetController", "Create"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetAsync(id);
                if (id == 0)
                {
                    response.CompanyId = CurrentUser.CompanyId;
                    response.AssetAdditionalDetail.TankAcceptDelivery = ContextFactory.Current.GetDomain<AssetDomain>().AddDefaultTankAcceptDays();
                }
                response.Type = type;
                response.AssetAdditionalDetail.Type = type;
                response.IsJobDetails = isJobDetails;
                response.MaxAllowedFileSize = SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize;
                if (response != null)
                {
                    response.ForcastingPreference = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Tank, response.Id);
                }
                return PartialView("Create", response);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssetViewModel viewModel, HttpPostedFileBase imageFile = null)
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

                if (viewModel.Id > 0)
                {
                    response = await ContextFactory.Current.GetDomain<AssetDomain>().UpdateAssetAsync(UserContext, viewModel);
                    response.EntityId = viewModel.Id;
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<AssetDomain>().SaveAssetAsync(viewModel);
                }

                //Save the forcasting setting details.
                if (response.StatusCode == Status.Success)
                {
                    await Saveforcastingdetails(viewModel, response);
                }

                if (viewModel.IsJobDetails && viewModel.JobId > 0)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("Details", "Job", new { area = "Buyer", id = viewModel.JobId });

                }
                else
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("View", "Asset", new { area = "Buyer" });

                }
            }

            return View(viewModel);
        }

        private async Task Saveforcastingdetails(AssetViewModel viewModel, StatusViewModel response)
        {
            if (viewModel.Type == (int)AssetType.Tank)
            {
                var forcastingResponse = await SaveforcastingSettingTankLevel(viewModel.ForcastingPreference, response);
                if (forcastingResponse.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)forcastingResponse.StatusCode, forcastingResponse.StatusMessage);
                }
                else
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
            }
            else
            {
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            using (var tracer = new Tracer("AssetController", "Delete"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().DeleteAssetAsync(CurrentUser.Id, id);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("View", "Asset", new { area = "Buyer" });
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        [HttpGet]
        public async Task<ActionResult> Details(int id, int type = 1)
        {
            using (var tracer = new Tracer("AssetController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetDetailAsync(id);
                response.Asset.Type = type;
                return PartialView("Details", response);
                //return View(response);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            using (var tracer = new Tracer("AssetController", "Edit"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetAsync(id);
                return View(response);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AssetViewModel viewModel)
        {
            using (var tracer = new Tracer("AssetController", "Edit"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.UserId = CurrentUser.Id;
                    viewModel.UpdatedDate = DateTimeOffset.Now;

                    var response = await ContextFactory.Current.GetDomain<AssetDomain>().UpdateAssetAsync(UserContext, viewModel);
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("View", "Asset", new { area = "Buyer" });
                    }
                    else
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    }
                }
                return View(viewModel);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpGet]
        public async Task<ActionResult> Remove(int id, int jobId)
        {
            using (var tracer = new Tracer("AssetController", "Remove"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().RemoveFromJobAsync(UserContext, id);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                //this redirects to Job Details page when assign asset is called from Job Details page
                if (IsReturnUrlExist())
                {
                    return Redirect(GetReturnUrl());
                }
                return RedirectToAction("View", "Asset", new { area = "Buyer" });
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpGet]
        public async Task<ActionResult> Assign(int id, int assetId)
        {
            using (var tracer = new Tracer("AssetController", "Assign"))
            {
                var asset = new AssetJobAssignmentViewModel { AssetId = assetId, Id = id };
                asset.JobId = await ContextFactory.Current.GetDomain<AssetDomain>().GetCurrentJobId(id);
                return View(asset);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<JsonResult> DeleteAssets(List<int> assets)
        {
            using (var tracer = new Tracer("AssetController", "DeleteAssets"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().DeleteAssetAsync(CurrentUser.Id, assets);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return Json(response.StatusCode, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Assign(AssetJobAssignmentViewModel viewModel)
        {
            using (var tracer = new Tracer("AssetController", "Assign"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.AssignedBy = CurrentUser.Id;
                    viewModel.AssignedDate = DateTimeOffset.Now;

                    var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
                    if (viewModel.Id > 0)
                    {
                        await assetDomain.RemoveFromJobAsync(UserContext, viewModel.Id, viewModel.JobId);
                    }
                    var response = await assetDomain.AssignToJobAsync(UserContext, viewModel);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }

                //this redirects to Job Details page when assign asset is called from Job Details page
                if (IsReturnUrlExist())
                {
                    return Redirect(GetReturnUrl());
                }
                return RedirectToAction("View", "Asset", new { area = "Buyer" });
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpPost]
        public async Task<ActionResult> UploadImage(AssetDetailViewModel viewModel, HttpPostedFileBase imageFile)
        {
            var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
            if (viewModel.Asset.Image.IsRemoved)
            {
                var response = await assetDomain.RemoveImageAsync(viewModel.Asset.Id, viewModel.Asset.Image.Id);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }
            else if (imageFile != null)
            {
                var reader = new BinaryReader(imageFile.InputStream);
                byte[] imageData = reader.ReadBytes((int)imageFile.InputStream.Length);

                var response = await assetDomain.SaveImageAsync(viewModel.Asset.Id, imageData);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }

            return RedirectToAction("Details", "Asset", new { area = "Buyer", id = viewModel.Asset.Id });
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        [HttpGet]
        public ActionResult PartialAssetHistoryView(int id)
        {
            using (var tracer = new Tracer("AssetController", "PartialAssetHistoryView"))
            {
                var response = ContextFactory.Current.GetDomain<AssetDomain>().GetAssetHistoryGrid(id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public PartialViewResult PartialDuplicateAssetView(int jobId = 0)
        {
            using (var tracer = new Tracer("AssetController", "PartialDuplicateAssetView"))
            {
                var response = new AssetDuplicateGridViewModel();
                var duplicates = TempData["duplicates"] as string;
                if (duplicates == null)
                {
                    response = Task.Run(() => ContextFactory.Current.GetDomain<AssetDomain>().GetDuplicateAssets(CurrentUser.CompanyId)).Result;
                }
                else
                {
                    response = JsonConvert.DeserializeObject<AssetDuplicateGridViewModel>(duplicates);
                }
                response.JobId = jobId;
                return PartialView("_PartialAssetDuplicateGrid", response);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> UploadDuplicates(AssetDuplicateGridViewModel duplicates)
        {
            var response = await ContextFactory.Current.GetDomain<AssetDomain>().SaveUpdatedAssetsAsync(CurrentUser.Id, duplicates);
            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            if (response.StatusCode == Status.Failed)
            {
                TempData["duplicates"] = JsonConvert.SerializeObject(duplicates);
            }
            return RedirectToAction("View", "Asset", new { area = "Buyer" });
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> DeleteDuplicates(AssetDuplicateGridViewModel duplicates)
        {
            var response = await ContextFactory.Current.GetDomain<AssetDomain>().DeleteDuplicateAssetsAsync(CurrentUser.CompanyId, duplicates);
            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            return RedirectToAction("View", "Asset", new { area = "Buyer" });
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpPost]
        public async Task<ActionResult> AssignSubcontractor(int assetId, int subcontractorId, int jobId)
        {
            var response = await ContextFactory.Current.GetDomain<AssetDomain>().AssignAssetSubcontractor(UserContext, assetId, subcontractorId, jobId);
            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            return PartialView("_DisplayCustomMessage");
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpGet]
        public async Task<ActionResult> GetTankTypesGrid()
        {
            using (var tracer = new Tracer("AssetController", "GetTankTypesGrid"))
            {
                return PartialView("_PartialTankTypesGrid");
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpGet]
        public async Task<JsonResult> GetTankTypesByCompanyAsync()
        {
            using (var tracer = new Tracer("AssetController", "GetTankTypesByCompanyAsync"))
            {
                var response = new List<TankModalTypeViewModel>();
                try
                {

                    response = await ContextFactory.Current.GetDomain<AssetDomain>().GetTankTypesByCompanyAsync(CurrentUser.CompanyId);

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetController", "GetTankTypesByCompanyAsync", ex.Message, ex);
                }

                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        data = response,
                    },

                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpPost]
        public async Task<JsonResult> TankTypeDipChartBulkUpload(string tankTypeName, string tankTypeModal, int scaleMeasurement)
        {
            using (var tracer = new Tracer("AssetController", "TankTypeDipChartBulkUpload"))
            {
                var response = new StatusViewModel();
                try
                {
                    var tankTypeViewModel = new TankModalTypeViewModel { Name = tankTypeName, Modal = tankTypeModal, ScaleMeasurement = scaleMeasurement };

                    if (Request.Files.Count > 0)
                    {
                        var csvFile = Request.Files[0];

                        if (csvFile != null && csvFile.ContentLength > 0)
                        {
                            if (csvFile.ContentLength < Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                            {
                                if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                                {
                                    string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                                    var csvFilePath = Server.MapPath("~\\Content\\TankMakeModel_Bulkupload_Template.csv");
                                    var dipChartDetails = await ContextFactory.Current.GetDomain<TankBulkUploadDomain>().ValidateTankTypesBulkFile(csvText, csvFilePath);

                                    if (dipChartDetails.Count > 0)
                                    {
                                        //initilize
                                        tankTypeViewModel.DipChartDetails = dipChartDetails;
                                        tankTypeViewModel.CreatedBy = CurrentUser.Id;
                                        tankTypeViewModel.CreatedByCompanyId = CurrentUser.CompanyId;
                                        tankTypeViewModel.BuyerCompanyId = CurrentUser.CompanyId;
                                        tankTypeViewModel.IsActive = true;
                                        tankTypeViewModel.CreatedOn = DateTime.Now;

                                        if (scaleMeasurement == (int)TankScaleMeasurement.Cm)
                                            tankTypeViewModel.ScaleMeasurementText = Resource.lblCm;
                                        else if (scaleMeasurement == (int)TankScaleMeasurement.Inches)
                                            tankTypeViewModel.ScaleMeasurementText = Resource.lblInches;

                                        //generate pdf and set path
                                        Random rand = new Random();
                                        var fileName = tankTypeViewModel.Name.ToLower() + "-" + tankTypeViewModel.Modal.ToLower() + "-" + CurrentUser.CompanyId + "-" + rand.Next(100).ToString() + ".pdf";
                                        var partialPdfView = GetPartialViewPdfContent("_PartialTankTypeDipChartPdf", tankTypeViewModel);
                                        Stream stream = new MemoryStream(partialPdfView);
                                        var azureBlob = new AzureBlobStorage();
                                        tankTypeViewModel.PdfFilePath = await azureBlob.SaveBlobAsync(stream, fileName, BlobContainerType.TankTypeDipChart.ToString().ToLower());
                                        response = await ContextFactory.Current.GetDomain<AssetDomain>().SaveTankTypes(tankTypeViewModel);
                                    }
                                    else { response.StatusMessage = Resource.errorInvalidDataInFile; }
                                }
                                else { response.StatusMessage = Resource.errMessageSelectCsvFile; }
                            }
                            else { response.StatusMessage = Resource.errFileSizeMessage; }
                        }
                        else { response.StatusMessage = Resource.errMessageNoFileChosen; }
                    }
                    else { response.StatusMessage = Resource.errMessageNoFileChosen; }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetController", "TankTypeDipChartBulkUpload", ex.Message, ex);
                }

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpPost]
        public async Task<JsonResult> DeleteTankDipChartById(string id)
        {
            using (var tracer = new Tracer("AssetController", "DeleteTankDipChartById"))
            {
                var response = new StatusViewModel();
                try
                {

                    response = await ContextFactory.Current.GetDomain<AssetDomain>().DeleteTankDipChartById(id);

                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetController", "DeleteTankDipChartById", ex.Message, ex);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpGet]
        public async Task<JsonResult> GetAllTankTypeNameForDipChart(string searchValue)
        {
            var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAllTankTypeNameForDipChart(CurrentUser.CompanyId, searchValue);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        private async Task<StatusViewModel> SaveforcastingSettingTankLevel(ForcastingPreferenceViewModel viewModel, StatusViewModel response)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            statusViewModel.StatusCode = Status.Failed;
            if (viewModel != null && viewModel.ForcastingServiceSetting != null && response.StatusCode == (int)Status.Success)
            {
                statusViewModel = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().SaveForeCastingPreferanceSetting(viewModel, UserContext, (int)ForcastingSettingLevel.Tank, response.EntityId);
            }
            else
            {
                statusViewModel.StatusMessage = response.StatusMessage;
            }
            return statusViewModel;
        }
        
        [HttpGet]
        public  JsonResult GetTFXFuelTypeByProductTypeId(int productTypeId)
        {
            using (var tracer = new Tracer("AssetController", "GetTFXFuelTypeByProductTypeId"))
            {
                var response = new List<DropdownDisplayItem>();
                try
                {
                    response =  ContextFactory.Current.GetDomain<MasterDomain>().GetTFXFuelTypeByProductTypeId(productTypeId);

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetController", "GetTFXFuelTypeByProductTypeId", ex.Message, ex);
                }

                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        data = response,
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}