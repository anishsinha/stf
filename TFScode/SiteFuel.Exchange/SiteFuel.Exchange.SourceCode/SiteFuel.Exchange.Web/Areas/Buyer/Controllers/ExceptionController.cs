using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    public class ExceptionController : BaseController
    {
        [HttpGet]
        public ActionResult Manage(int approverStatus = 0)
        {
            ViewBag.approverStatus = approverStatus;
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> CompanyExceptions()
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetCompanyExceptions(UserContext);
            return PartialView("_PartialManageExceptions", response);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Buyer, UserRoles.BuyerAdmin)]
        public async Task<JsonResult> CompanyExceptions(ManageExceptionModel model)
        {
            var response = new StatusViewModel();
            if (ModelState.IsValid)
            {
                var exceptionDomain = new ExceptionDomain();
                response = await exceptionDomain.SaveCompanyExceptions(UserContext, model);
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }
        public ActionResult MyApprovalExceptions()
        {
            return PartialView("_PartialMyApprovalExceptions");
        }

        [AuthorizeRole(UserRoles.Buyer, UserRoles.BuyerAdmin)]
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

        [HttpPost]
        [AuthorizeRole(UserRoles.Buyer, UserRoles.BuyerAdmin)]
        public async Task<JsonResult> Approve(int exceptionId, ExceptionResolution resolutionTypeId, decimal quantity)
        {
            var invoiceDomain = new InvoiceDomain();
            var response = await invoiceDomain.ApproveEddtAndCreateInvoice(UserContext, exceptionId, resolutionTypeId, quantity, (int)ExceptionStatus.Resolved);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public ActionResult GetSupplierApprovalGrid()
        {
            return PartialView("_PartialSupplierApprovals");
        }

        [HttpPost]
        public async Task<JsonResult> GetSupplierApprovalExceptions(ApprovalExceptionRequestModel requestModel)
        {
            ExceptionDomain exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetSupplierApprovalExceptions(requestModel.ExceptionTypes, UserContext);
            if (response != null)
            {
                int exceptionTypeId = 0;
                int.TryParse(requestModel.ExceptionTypes, out exceptionTypeId);
                if (exceptionTypeId == (int)ExceptionType.UnknownDeliveries || exceptionTypeId == (int)ExceptionType.MissingDeliveries)
                {
                    return new JsonResult
                    {
                        Data = new DatatableResponse()
                        {
                            draw = requestModel.draw,
                            data = response.DeliveryMismatchExceptions,
                            recordsTotal = response.DeliveryMismatchExceptions != null ? response.DeliveryMismatchExceptions.Count : 0,
                            recordsFiltered = response.DeliveryMismatchExceptions != null ? response.DeliveryMismatchExceptions.Count : 0,
                        },
                        MaxJsonLength = int.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new DatatableResponse()
                        {
                            draw = requestModel.draw,
                            data = response.GeneratedExceptions,
                            recordsTotal = response.GeneratedExceptions != null ? response.GeneratedExceptions.Count : 0,
                            recordsFiltered = response.GeneratedExceptions != null ? response.GeneratedExceptions.Count : 0,
                        },
                        MaxJsonLength = int.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            else
            {
                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = requestModel.draw,
                        data = new List<ExceptionApprovalResponseModel>(),
                        recordsTotal = 0,
                        recordsFiltered = 0
                    },
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.Buyer, UserRoles.BuyerAdmin)]
        [HttpPost]
        public async Task<JsonResult> GetGeneratedExceptionsForApproval(ApprovalExceptionRequestModel requestModel)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetGeneratedExceptionsForApproval(requestModel.ExceptionTypes, UserContext);
            if (response != null)
            {
                int exceptionTypeId = 0;
                int.TryParse(requestModel.ExceptionTypes, out exceptionTypeId);
                if (exceptionTypeId == (int)ExceptionType.UnknownDeliveries)
                {
                    return new JsonResult
                    {
                        Data = new DatatableResponse()
                        {
                            draw = requestModel.draw,
                            data = response.DeliveryMismatchExceptions,
                            recordsTotal = response.DeliveryMismatchExceptions != null ? response.DeliveryMismatchExceptions.Count : 0,
                            recordsFiltered = response.DeliveryMismatchExceptions != null ? response.DeliveryMismatchExceptions.Count : 0,
                        },
                        MaxJsonLength = int.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new DatatableResponse()
                        {
                            draw = requestModel.draw,
                            data = response.GeneratedExceptions,
                            recordsTotal = response.GeneratedExceptions != null ? response.GeneratedExceptions.Count : 0,
                            recordsFiltered = response.GeneratedExceptions != null ? response.GeneratedExceptions.Count : 0,
                        },
                        MaxJsonLength = int.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            else
            {
                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = requestModel.draw,
                        data = new List<ExceptionApprovalResponseModel>(),
                        recordsTotal = 0,
                        recordsFiltered = 0
                    },
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        [HttpGet]
        public async Task<ActionResult> CustomerExceptions(int customerCompanyId)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetCustomerExceptions(UserContext, customerCompanyId);
            return PartialView("CustomerExceptions", response);
        }
        [HttpPost]
        public async Task<ActionResult> CustomerExceptions(ManageCustomerExceptionModel model)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.SaveCustomerExceptions(UserContext, model);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<ActionResult> NoDataExceptionGrid(NoDataExceptionDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("ExceptionController", "NoDataExceptionGrid"))
            {
                var exceptionDomain = new ExceptionDomain();
                 var response = await exceptionDomain.GetBuyerNoDataExceptionDdts(CurrentUser.CompanyId, requestModel);

                //var response = new List<NoDataExceptionDdtGridViewModel>();
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;

                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = requestModel.draw,
                        data = response,
                        recordsTotal = totalCount,
                        recordsFiltered = totalCount
                    },

                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Buyer, UserRoles.BuyerAdmin)]
        public async Task<ActionResult> AcceptDdtWithoutData(int invoiceHeaderId, WaitingAction waitingFor, NoDataExceptionApproval action)
        {
            var response = new StatusViewModel();
            var exceptionDomain = new ExceptionDomain();
            if (ModelState.IsValid)
            {
                response = await exceptionDomain.AcceptDdtWithNoData(invoiceHeaderId, waitingFor, action, UserContext);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}