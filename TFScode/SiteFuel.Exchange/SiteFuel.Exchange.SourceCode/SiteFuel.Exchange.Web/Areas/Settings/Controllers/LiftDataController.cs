using Microsoft.Owin.Security;
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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Settings.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer, CompanyType.Supplier, CompanyType.Carrier)]
    public class LiftDataController : BaseController
    {
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> UploadPoFile(HttpPostedFileBase poCsvFile)
        {
            using (var tracer = new Tracer("LiftDataController", "UploadPoFile"))
            {
                if (poCsvFile != null && poCsvFile.ContentLength > 0)
                {
                    if (poCsvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                    {
                        if (Path.GetExtension(poCsvFile.FileName).ToLower() == ".csv")
                        {
                            var domain = ContextFactory.Current.GetDomain<PoNumberBulkUploadDomain>();
                            var response = await domain.ProcessUploadedPoFile(poCsvFile, UserContext);

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

            return RedirectToAction("PreferencesSetting", "Profile", new { area = "Settings" });
        }

        public ActionResult GetPoNumberGridView()
        {
            return PartialView("_PartialPONumbersGrid");
        }

        [HttpPost]
        public async Task<ActionResult> GetPoNumberGridData(DataTableAjaxPostModel model)
        {
            var domain = new LFVDomain();
            var response = await domain.GetPoNumbersGrid(UserContext.CompanyId);
            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    draw = model.draw,
                    data = response,
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count
                },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public async Task<ActionResult> DeleteRecord(int recordId)
        {
            var domain = new LFVDomain();
            var response = await domain.DeleteSelfHaulPONumber(recordId);
            return Json(response,JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetBadgeNumberGridView()
        {
            return PartialView("_PartialBadgeNumbersGrid");
        }

        [HttpPost]
        public async Task<ActionResult> GetBadgeNumberGridData(DataTableAjaxPostModel model)
        {
            var domain = new LFVDomain();
            var response = await domain.GetBadgeNumbersGrid(UserContext.CompanyId);
            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    draw = model.draw,
                    data = response,
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count
                },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public async Task<ActionResult> DeleteBadgeNumber(int recordId)
        {
            var domain = new LFVDomain();
            var response = await domain.DeleteBadgeNumber(recordId);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetCarrierListGridView()
        {
            return PartialView("_PartialLiftFileCarrierNamesGrid");
        }

        [HttpPost]
        public async Task<ActionResult> GetCarrierNamesListGridData(DataTableAjaxPostModel model)
        {
            var domain = new LFVDomain();
            var response = await domain.GetLiftFileCarrierNamesGrid(UserContext.CompanyId);
            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    draw = model.draw,
                    data = response,
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count
                },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public async Task<ActionResult> DeleteCarrierName(int recordId)
        {
            var domain = new LFVDomain();
            var response = await domain.DeleteLiftFileCarrierName(recordId);
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetQuebecBadgeListGridView()
        {
            return PartialView("_PartialQuebecBillingBadgesGrid");
        }

        [HttpPost]
        public async Task<ActionResult> GetQuebecBadgeListGridData(DataTableAjaxPostModel model)
        {
            var domain = new LFVDomain();
            var response = await domain.GetQuebecBadgeListGridData(UserContext.CompanyId);
            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    draw = model.draw,
                    data = response,
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count
                },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public async Task<ActionResult> DeleteBilingBadge(int recordId)
        {
            var domain = new LFVDomain();
            var response = await domain.DeleteBilingBadge(recordId);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

    }
}