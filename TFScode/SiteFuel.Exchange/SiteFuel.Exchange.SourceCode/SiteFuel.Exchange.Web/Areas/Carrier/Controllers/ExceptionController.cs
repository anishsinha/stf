using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Carrier.Controllers
{
    public class ExceptionController : BaseController
    {
        [HttpGet]
        public ActionResult Manage(int approverStatus = 0)
        {
            ViewBag.approverStatus = approverStatus;
            return View();
        }
        public ActionResult MyApprovalExceptions()
        {
            return PartialView("_PartialMyApprovalExceptions");
        }
        [AuthorizeRole(UserRoles.Carrier, UserRoles.CarrierAdmin)]
        [HttpPost]
        public async Task<JsonResult> MyApprovalExceptionGrid(DataTableAjaxPostModel model)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetMyApprovalExceptions(UserContext);
            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    draw = model.draw,
                    data = response.GeneratedExceptions,
                    recordsTotal = response.GeneratedExceptions.Count,
                    recordsFiltered = response.GeneratedExceptions.Count
                },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult GetSupplierApprovalGrid()
        {
            return PartialView("_PartialSupplierApprovals");
        }
        [HttpPost]
        [AuthorizeRole(UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<JsonResult> Approve(int exceptionId, ExceptionResolution resolutionTypeId, decimal quantity)
        {
            var invoiceDomain = new InvoiceDomain();
            var response = await invoiceDomain.ApproveEddtAndCreateInvoice(UserContext, exceptionId, resolutionTypeId, quantity, (int)ExceptionStatus.Resolved);
            return Json(response, JsonRequestBehavior.DenyGet);
        }
    }
}