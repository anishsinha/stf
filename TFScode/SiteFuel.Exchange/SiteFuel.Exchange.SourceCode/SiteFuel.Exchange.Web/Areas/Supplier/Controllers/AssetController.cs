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
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.ViewModels.Forcasting;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier)]
    public class AssetController : BaseController
    {

        [HttpGet]
        public async Task<ActionResult> Details(int id, int type = 1)
        {
            using (var tracer = new Tracer("AssetController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetDetailAsync(id);
                response.Asset.Type = type;
                return PartialView("Details", response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> AssetGrid(AssetFilterViewModel assetFilter = null)
        {
            using (var tracer = new Tracer("AssetController", "AssetGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetInfoGridAsync(CurrentUser.Id, assetFilter.JobId, (int)assetFilter.Type);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public async Task<ActionResult> Create(int id = 0, int type = 1, int jobId = 0)
        {
            using (var tracer = new Tracer("AssetController", "Create"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetAsync(id);
                if (id == 0)
                {
                    var BuyerCompanyId = ContextFactory.Current.GetDomain<AssetDomain>().GetBuyerCompanyId(jobId);
                    response.CompanyId = BuyerCompanyId != 0 ? BuyerCompanyId : CurrentUser.CompanyId;
                    response.AssetAdditionalDetail.TankAcceptDelivery = ContextFactory.Current.GetDomain<AssetDomain>().AddDefaultTankAcceptDays();
                }
                response.Type = type;
                response.AssetAdditionalDetail.Type = type;
                response.MaxAllowedFileSize = SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize;
                if (response != null)
                {
                    response.ForcastingPreference = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Tank, response.Id);
                }
                return PartialView("Create", response);
            }
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssetViewModel viewModel, HttpPostedFileBase imageFile = null, int jobId = 0)
        {
            ValidateForcastingModelState(viewModel);
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
                    response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().UpdateAssetAsync(UserContext, viewModel);
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().CreateAssetsAsync(UserContext, viewModel, jobId);
                }
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
                            return RedirectToAction("Details", "Job", new { area = "Supplier", id = viewModel.JobId });
                        }
                    }
                    else
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        return RedirectToAction("Details", "Job", new { area = "Supplier", id = viewModel.JobId });
                    }
                }
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Job", new { area = "Supplier", id = viewModel.JobId });

            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> GetTankTypesGrid(int jobId)
        {
            using (var tracer = new Tracer("AssetController", "GetTankTypesGrid"))
            {
                return PartialView("_PartialTankTypesGridForSupplier", jobId);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetTankDipChartCompanyAsync(int jobId = 0)
        {
            using (var tracer = new Tracer("AssetController", "GetTankDipChartCompanyAsync"))
            {
                var response = new List<TankModalTypeViewModel>();
                try
                {
                    response = await ContextFactory.Current.GetDomain<AssetDomain>().GetTankTypesByCompanyAsync(CurrentUser.CompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetController", "GetTankDipChartCompanyAsync", ex.Message, ex);
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

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpPost]
        public async Task<JsonResult> TankTypeDipChartBulkUpload(string tankTypeName, string tankTypeModal, int scaleMeasurement, int jobId = 0)
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
                                    var BuyerCompanyId = ContextFactory.Current.GetDomain<AssetDomain>().GetBuyerCompanyId(jobId);
                                    if (dipChartDetails.Count > 0)
                                    {
                                        //initilize
                                        tankTypeViewModel.DipChartDetails = dipChartDetails;
                                        tankTypeViewModel.CreatedBy = CurrentUser.Id;
                                        tankTypeViewModel.CreatedByCompanyId = CurrentUser.CompanyId;
                                        tankTypeViewModel.BuyerCompanyId = BuyerCompanyId != 0 ? BuyerCompanyId : CurrentUser.CompanyId;
                                        tankTypeViewModel.IsActive = true;
                                        tankTypeViewModel.CreatedOn = DateTime.Now;

                                        if (scaleMeasurement == (int)TankScaleMeasurement.Cm)
                                            tankTypeViewModel.ScaleMeasurementText = Resource.lblCm;
                                        else if (scaleMeasurement == (int)TankScaleMeasurement.Inches)
                                            tankTypeViewModel.ScaleMeasurementText = Resource.lblInches;

                                        //generate pdf and set path
                                        var fileName = tankTypeViewModel.Name.ToLower() + "-" + tankTypeViewModel.Modal.ToLower() + "-" + CurrentUser.CompanyId + "-" + Guid.NewGuid().ToString() + ".pdf";
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

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
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

        [HttpGet]
        public async Task<JsonResult> GetAllTankTypeNameForDipChart(string searchValue)
        {
            var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAllTankTypeNameForDipChart(CurrentUser.CompanyId, searchValue);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Dispatcher)]
        public async Task<JsonResult> DeleteAssets(List<int> assets, int jobId)
        {
            using (var tracer = new Tracer("AssetController", "DeleteAssets"))
            {

                var response = await ContextFactory.Current.GetDomain<AssetDomain>().DeleteAssetAsync(CurrentUser.Id, assets);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return Json(response.StatusCode, JsonRequestBehavior.AllowGet);
            }
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
        private void ValidateForcastingModelState(AssetViewModel viewModel)
        {
            if (viewModel.ForcastingPreference != null && viewModel.ForcastingPreference.ForcastingServiceSetting != null)
            {
                if (!viewModel.ForcastingPreference.ForcastingServiceSetting.IsEnabled)
                {
                    var ForcastingServiceSettingInfo = ModelState.Where(x => x.Key.Contains("ForcastingServiceSetting") && x.Value.Errors.Count() > 0).ToList();
                    if (ForcastingServiceSettingInfo != null)
                    {
                        foreach (var forcastingServiceSettingItem in ForcastingServiceSettingInfo)
                        {
                            ModelState.Remove(forcastingServiceSettingItem.Key);

                        }

                    }
                }
            }
        }
    }
}