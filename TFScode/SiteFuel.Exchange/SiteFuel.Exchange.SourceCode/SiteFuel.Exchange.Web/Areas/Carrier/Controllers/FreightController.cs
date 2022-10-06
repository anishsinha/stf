using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Domain.Services;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Web.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Carrier.Controllers
{
    public class FreightController : BaseController
    {
        [AuthorizeCompany(CompanyType.Carrier)]
        [ActionName("View")]
        public ActionResult Index()
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetCompanyDefaultCurrency(UserContext.CompanyId)).Result;
            return View("View", response);
        }

        [HttpPost]
        public ActionResult Create(TruckDetailViewModel inputData)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<AssetDomain>().SaveTruckDetailsAsync(UserContext, inputData)).Result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteTruck(TruckDetailViewModel inputData)
        {
            inputData.TfxCompanyId = CurrentUser.CompanyId;
            inputData.TfxCreatedBy = CurrentUser.Id;
            inputData.CreatedDate = DateTimeOffset.Now;
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().DeleteTruckAsync(inputData)).Result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllTruckDetails()
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetAllTruckDetails(UserContext.CompanyId)).Result;
            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetCompantDrivers()
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<HelperDomain>().GetAllDrivers(UserContext.CompanyId)).Result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetFuelTypes()
        {
            var response = Task.Run(() => CommonHelperMethods.GetAssetFuelTypes().OrderBy(top => top.Name).ToList()).Result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CreateDeliveryRequest()
        {
            return PartialView("_PartialDeliveryRequest");
        }

        [HttpGet]
        public ActionResult GetJobsWithTanks(string Prefix = "")
        {
            string regionId = "";
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetSiteList(regionId, UserContext, Prefix));
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetJobListForCarrier(string regionId, bool isShowCarrierManaged = false, string carriers = "")
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetJobListForCarrier(regionId, UserContext, isShowCarrierManaged, carriers);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetShiftByDrivers(string driverList, int scheduleType)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetShiftByDrivers(driverList, scheduleType));
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getRegionShiftSchedule(string regionId, int scheduleType)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetRegionShiftScheduleByRegionId(regionId, scheduleType));
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckLocationAssignedToCarrier(int jobId)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().CheckLocationAssignedToCarrier(jobId, UserContext));
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetCreateLoadJobListForCarrier(string regionId)
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetCreateLoadJobListForCarrier(regionId, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetDRReportData(DRReportFilterInputViewModel inputData)
        {
            var domain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await domain.GetAllDeliveryRequests(inputData, UserContext);
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> ResetRetainFuelDetails(TruckDetailViewModel truckDetailViewModel)
        {
            using (var tracer = new Tracer("FreightController", "ResetFuelRetainDetails"))
            {
                var domain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                var response = await domain.ResetFuelRetainDetails(truckDetailViewModel, UserContext);
                return Json(response);
            }
        }
        [HttpPost]
        public async Task<JsonResult> UpdateRetainFuelDetails(TruckDetailViewModel truckDetailViewModel)
        {
            using (var tracer = new Tracer("FreightController", "ResetFuelRetainDetails"))
            {
                var domain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                var response = await domain.UpdateRetainFuelDetails(truckDetailViewModel, UserContext);
                return Json(response);
            }
        }
        [HttpPost]
        public async Task<JsonResult> ConfirmRetainFuelException(TruckDetailViewModel truckDetailViewModel)
        {
            using (var tracer = new Tracer("FreightController", "ConfirmRetainFuelException"))
            {
                var domain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                var response = await domain.ConfirmRetainFuelException(truckDetailViewModel, UserContext);
                return Json(response);
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetCarrierRegions()
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetAllCarrierRegions(CommonHelperMethods.GetCarriers(CurrentUser.CompanyId));
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> CreditCheckApprovalFileUpload()//string heldDrId, HttpPostedFileBase file = null
        {
            var response = new StatusViewModel();
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string heldDrId = Convert.ToString(Request.Form["heldDrId"]);
                if (file != null)
                {
                    response = await AzureStorageService.UploadImageToBlob(UserContext, file.InputStream, file.FileName, BlobContainerType.CreditCheckApprovalFiles);
                    if (response.StatusCode == Status.Success)
                    {
                        response = await ContextFactory.Current.GetDomain<HeldDrQueueDomain>().OverrideCreditCheckApproval(heldDrId, file.FileName, response.StatusMessage, UserContext.Id);
                    }
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}