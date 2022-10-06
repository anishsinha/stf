using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using SiteFuel.Exchange.Web.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    public class ExceptionController : SupplierBaseController
    {
        [HttpGet]
        public ActionResult Manage(int approverStatus = 0)
        {
            ViewBag.approverStatus = approverStatus;
            var exceptionDomain = new ExceptionDomain();
            var response = exceptionDomain.IsLiftFileValidationEnabled(UserContext);
            return View(response);
        }
        [HttpGet]
        public async Task<ActionResult> CompanyExceptions()
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetCompanyExceptions(UserContext);
            return PartialView("_PartialManageExceptions", response);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin)]
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

        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin)]
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

        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        [HttpPost]
        public async Task<JsonResult> GetRetainFuelExceptionData(DataTableAjaxPostModel model)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetRetainFuelExceptionData(UserContext);          

            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    draw = model.draw,
                    data = response,
                    recordsTotal = response.truckDetailViewModels != null ? response.truckDetailViewModels.Count : 0,
                    recordsFiltered = response.truckDetailViewModels != null ? response.truckDetailViewModels.Count : 0,
                },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        [HttpPost]
        public async Task<JsonResult> GetGeneratedExceptionsForApproval(ApprovalExceptionRequestModel requestModel)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetGeneratedExceptionsForApproval(requestModel.ExceptionTypes, UserContext);
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

        [HttpGet]
        public async Task<ActionResult> CustomerExceptions(int customerCompanyId)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetCustomerExceptions(UserContext, customerCompanyId);
            response.Exceptions = response.Exceptions.Where(t => t.ExceptionTypeId != (int)ExceptionType.MissingDeliveries && t.ExceptionTypeId != (int)ExceptionType.UnknownDeliveries).ToList();
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
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<JsonResult> Approve(int exceptionId, ExceptionResolution resolutionTypeId, decimal quantity)
        {
            var invoiceDomain = new InvoiceDomain();
            var response = new StatusViewModel();
            if (resolutionTypeId == ExceptionResolution.DiscardDropTicket)
            {
                response = await invoiceDomain.DiscardEddt(UserContext, exceptionId, resolutionTypeId, ExceptionStatus.Resolved);
            }
            else
            {
                response = await invoiceDomain.ApproveEddtAndCreateInvoice(UserContext, exceptionId, resolutionTypeId, quantity, (int)ExceptionStatus.Resolved);
            }

            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public ActionResult GetBuyerApprovalGrid()
        {
            return PartialView("_PartialBuyerApprovals");
        }

        [HttpPost]
        public async Task<JsonResult> GetBuyerApprovalExceptions(ApprovalExceptionRequestModel requestModel)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetBuyerApprovalExceptions(requestModel.ExceptionTypes, UserContext);
            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    draw = requestModel.draw,
                    data = response,
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count
                },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        public ActionResult GetCarrierSupplierApprovalGrid()
        {
            return PartialView("_PartialCarrierSupplierApprovals");
        }
        [HttpPost]
        public async Task<ActionResult> GetCustomers()
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetCustomersBySupplierOrCarrier(UserContext);
            return Json(response, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public async Task<ActionResult> GetLocationByCustomerId(int customerId, bool isRetailJob = false)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetLocationByCustomerId(customerId, UserContext, isRetailJob);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetOrdersByCustomerAndLocationId(int customerId, int locationId, int tfxProductId = 0)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.GetOrdersByCustomerAndLocationId(customerId, locationId, UserContext, tfxProductId);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<ActionResult> DiscardUnAssignDDT(int exceptionId, int exceptionDdtId)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.DiscardUnAssignDDT(exceptionId, exceptionDdtId);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<ActionResult> LinkedUnAssignDDTToOrder(int exceptionId, int customerId, int locationId, int orderId, int invoiceId)
        {
            var exceptionDomain = new ExceptionDomain();
            var response = await exceptionDomain.LinkedUnAssignDDTToOrder(exceptionId, customerId, locationId, orderId, invoiceId);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> ApproveEddtAndCreateInvoiceMissingDelivery(List<AssignOrderMissingDeliveryModel> assignOrderMissingDelivery, int exceptionId)
        {
            var response = new StatusViewModel();
            var exceptionDomain = new ExceptionDomain();
            if (ModelState.IsValid)
            {
                response = await exceptionDomain.ApproveEddtAndCreateInvoiceMissingDelivery(assignOrderMissingDelivery, exceptionId);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLiftFileValidationDetailsGrid()
        {
            return PartialView("_PartialLiftFileValidationGrid");
        }

        [HttpGet]
        public ActionResult GetLiftFileRecordsGrid(int lfCallId, int recordStatus = 0)
        {
            ViewBag.recordStatus = recordStatus;
            ViewBag.lfCallId = lfCallId;
            return PartialView("_PartialLiftFileRecordsGrid");
        }

        [HttpGet]
        public ActionResult LFValidationGrid()
        {
            var lfvDomain = new LFVDomain();
            var response = lfvDomain.GetLFValidationGrid(UserContext.CompanyId);
            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public ActionResult LFValidationGridWithFilter( DateTimeOffset? startDate = null, DateTimeOffset? endDate = null,bool isCarrierPerFormanceDashboard=false,string carrierIds = "",bool isMatchingWindow=false)
        {
            var lfvDomain = new LFVDomain();
            var response = lfvDomain.GetLFValidationGrid(UserContext.CompanyId, startDate, endDate, isCarrierPerFormanceDashboard, carrierIds, isMatchingWindow);
            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public ActionResult LFRecordsGrid(int recordStatus, int lfCallId)
        {
            var lfrDomain = new LFVDomain();
            var response = lfrDomain.GetLFRecordGrid(UserContext.CompanyId,UserContext.IsSupplierAdmin, recordStatus, lfCallId);

            //return Json(response, JsonRequestBehavior.AllowGet);

            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public ActionResult LFRecordsGridForDashboard(int recordStatus, int lfCallId, DateTimeOffset? startDate, DateTimeOffset? endDate,bool isMatchingWindow=false,string carrierIds="")
        {
            var lfrDomain = new LFVDomain();
            var response = lfrDomain.GetLFRecordGrid(UserContext.CompanyId,UserContext.IsSupplierAdmin, recordStatus, lfCallId, startDate, endDate, isMatchingWindow, carrierIds);
            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public async Task<ActionResult> NoDataExceptionGrid(NoDataExceptionDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("ExceptionController", "NoDataExceptionGrid"))
            {
                var exceptionDomain = new ExceptionDomain();
                var response = await exceptionDomain.GetSupplierNoDataExceptionDdts(CurrentUser.CompanyId, requestModel);
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

        [HttpGet]
        public async Task<ActionResult> GetAddPrePostDipDataModal(int InvoiceheaderId)
        {
            var exceptioDomain = new ExceptionDomain();
            var response = await exceptioDomain.GetNoDataExceptionDDtsForDipData(InvoiceheaderId, UserContext.CompanyId);
            return PartialView("_PartialWaitingForDipDataApproval", response);
        }

        [HttpPost]
        public async Task<ActionResult> SaveDipDataValues(NoDataExceptionPrePostViewModel prePostDipModel)
        {
            var exceptioDomain = new ExceptionDomain();
            var response = await exceptioDomain.CreateInvoiceWithPrePostData(UserContext, prePostDipModel);
            if (response.StatusCode == Status.Success)
                DisplayCustomMessages(MessageType.Success, response.StatusMessage);
            else
                DisplayCustomMessages(MessageType.Error, response.StatusMessage);

            return RedirectToAction("Manage", "Exception", new { area = "Supplier" });
        }

        [HttpGet]
        public ActionResult GetAddInvoiceImagesModal()
        {
            return PartialView("_PartialImageBulkUpload");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.AccountingPerson, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public async Task<ActionResult> ImageBulkUpload(List<HttpPostedFileBase> postedFiles)
        {
            var uploadedFiles = postedFiles;
            if (uploadedFiles.Any())
            {
                StringBuilder imageResult = new StringBuilder();
                foreach (var fileBase in uploadedFiles)
                {
                    if (fileBase != null)
                    {
                        if (fileBase.ContentLength < Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                        {
                            var fileFormat = Path.GetExtension(fileBase.FileName).ToLower();
                            if (fileFormat == ".jpg" || fileFormat == ".png" || fileFormat == ".pdf")
                            {
                                var bulkDomain = ContextFactory.Current.GetDomain<InvoiceBulkUploadDomain>();
                                var fileName = bulkDomain.RemoveSpecialCharacters(fileBase.FileName);
                                var response = bulkDomain.ValidateUploadedImageFiles(UserContext, fileBase.FileName);
                                if (response.StatusCode == Status.Success)
                                {
                                    response = await bulkDomain.UploadImageToBlob(UserContext, fileBase.InputStream, fileName, response.EntityNumber.ToEnum<InvoiceImageType>(), response.EntityId);

                                    if (response.StatusCode == Status.Failed)
                                    {
                                        imageResult.AppendLine("</br>");
                                        imageResult.AppendLine(response.StatusMessage);
                                    }
                                }
                                else
                                {
                                    imageResult.AppendLine("</br>");
                                    imageResult.AppendLine(response.StatusMessage);
                                }
                            }
                            else
                            {
                                imageResult.AppendLine("</br>");
                                imageResult.AppendLine("Only .jpg, .png files are allowed");
                                break;
                            }
                        }
                        else
                        {
                            DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                        }
                    }
                    else
                    {
                        imageResult.AppendLine("</br>");
                        imageResult.AppendLine(Resource.errMessageNoFileChosen);
                    }
                }
                if (imageResult.Length > 0)
                    DisplayCustomMessages(MessageType.Error, imageResult.ToString());
                else
                    DisplayCustomMessages(MessageType.Success, "Invoice images uploaded successfully. Please visit upload status page");
            }
            else
            {
                DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
            }
            return RedirectToAction("Manage", "Exception", new { area = "Supplier" });
        }


        [HttpPost]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> RaiseWaiverRequest(int invoiceHeaderId, WaitingAction waitingFor, NoDataExceptionApproval action)
        {
            var response = new StatusViewModel();
            var exceptionDomain = new ExceptionDomain();
            if (ModelState.IsValid)
            {
                response = await exceptionDomain.RaiseWaiverRequest(invoiceHeaderId, waitingFor, action, UserContext);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin)]
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

        [HttpPost]
        public async Task<ActionResult> AddUnmatchedRecordForReProcessing(List<int> LfRecordIds)
        {
            var domain = new LFVDomain();
            var response = await domain.AddUnmatchedRecordForReProcessing(LfRecordIds, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }       
    }
}