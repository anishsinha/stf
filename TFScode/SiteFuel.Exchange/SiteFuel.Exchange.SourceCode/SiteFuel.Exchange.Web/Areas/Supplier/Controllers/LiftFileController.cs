using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier, CompanyType.SupplierAndCarrier)]
    public class LiftFileController : BaseController
    {
        // GET: Supplier/LiftFile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetLFBolEditDetailsForSlider(LFRecordsGridViewModel model)
        {
            var response = new LFBolEditViewModel ();
            try
            {               
                if (model != null && model.LiftFileRecordId > 0)
                {
                    response = await ContextFactory.Current.GetDomain<LFVDomain>().GetLFBolEditViewModel(model,UserContext);
                }                
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LiftFileController", "GetLFBolEditDetailsForSlider", ex.Message, ex);
            }
            if (model != null && model.IsFromScratchReport)
                return Json(response, JsonRequestBehavior.AllowGet);
            else
               return PartialView("_PartialLFBolEdit", response);
        }
        [HttpPost]
        public  async Task<ActionResult> SaveLFBolEditDetails(LFBolEditViewModel model)
        {
             var response = new StatusViewModel();          
             response = await ContextFactory.Current.GetDomain<LFVDomain>().SaveLFBolEditDetails(model,UserContext);
             return Json(response, JsonRequestBehavior.AllowGet);          
        }
        [HttpGet]

        public async Task<ActionResult> GetLiftFileRecordsByBolFileName(string bol, string fileName)
        {
            var model = new LFRecordsGridViewModel();
            model.bol = string.IsNullOrWhiteSpace(bol) ? string.Empty : bol.Trim();
            model.FileName = string.IsNullOrWhiteSpace(fileName) ? string.Empty : fileName.Trim();
            return PartialView("_PartialLiftFileRecordsByBolFileName", model);
        }

        [HttpGet]
        public ActionResult LFRecordsGridByBolFileName(string bol, string fileName)
        {
            var lfrDomain = new LFVDomain();
            var response = lfrDomain.GetLFRecordByBolfilenameGrid(bol,fileName,UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //Using same api for Ignore match from nomatch and unmatch grid
        [HttpPost]
        public async Task<ActionResult> AddRecordsAsIgnoreMatch(List<int> LfRecordIds, int DescriptionId = 0, string DescriptionText = "")
        {
            var domain = new LFVDomain();
            var response = await domain.AddUnmatchedRecordsAsIgnoreMatch(LfRecordIds, UserContext, DescriptionId, DescriptionText);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //Supplier BOL Report
        [HttpGet]
        public async Task<JsonResult> GetLiftFileRecordsWithMissingTFXDeliveryDetails()
        {
            var domain = new LFVDomain();
            var response = await domain.GetLiftFileRecordsWithMissingTFXDeliveryDetails(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        //Carrier BOL Report
        [HttpPost]
        public async Task<JsonResult> GetTFXDeliveryDetailsWithMissingLiftFileRecords(DateTimeOffset? fromDate,DateTimeOffset? toDate)
        {
            var domain = new LFVDomain();
            var response = await domain.GetTFXDeliveryDetailsWithMissingLiftFileRecords(UserContext,fromDate,toDate);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<JsonResult> GetLiftFileRecordsScratchReport()
        {
            var domain = new LFVDomain();
            var response = await domain.GetLiftFileRecordsScratchReport(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult Dashboard()
        {
            var domain = new LFVDomain();
            ViewBag.matchingWindowDays =  domain.GetMatchingWindowDays(UserContext.CompanyId);
            return View();
        }

        [HttpGet]
        public ActionResult LFVScratchReport()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CarrierBolReport()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SupplierBolReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LFVAccrualReport()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetLFVCarrierDropDwn(DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            var domain = new LFVDomain();
            var response = await domain.GetLiftFileCarrierDropDwn(UserContext, fromDate, toDate);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetLFVAccrualReportGrid(AccrualReportGridInputViewModel input)
        {
            
            if (string.IsNullOrWhiteSpace(input.ProductTypeIds))
            {
                input.ProductTypeIds = string.Empty;
            }
            var dataTableSearchModel = new DataTableSearchModel(input);
            var domain = new LFVDomain();
            var response = await domain.GetLFVAccrualReportGrid(UserContext, input, dataTableSearchModel);
            var totalCount = 0;
            var filteredCount = 0;

            if (response.Any())
            {
                totalCount = response[0].TotalCount;
                filteredCount = response[0].FilteredCount;
            }

            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    draw = input.draw,
                    data = response,
                    recordsTotal = totalCount,
                    recordsFiltered = filteredCount
                },

                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public async Task<JsonResult> GetLFVValidationStatsAndProductTypesDDL(AccrualReportGridInputViewModel input)
        {
            var domain = new LFVDomain();
            var response = await domain.GetLFVValidationStatsAndProductTypesDDL(UserContext,input);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateLiftFileRecord(LFRecordsGridViewModel data)
        {
            var domain = new LFVDomain();
            var response = await domain.UpdateLiftFileRecord(data,UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<JsonResult> GetReasonDescriptionList()
        {
            var response = await new LFVDomain().GetReasonDescriptionList(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);

        }
    }
}
