using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Controllers
{
    public class OrderBaseController : BaseController
    {
        public PartialViewResult OrderBrokeredFuelRequestsGrid(int orderId)
        {
            return PartialView("~/Areas/Supplier/Views/Shared/_PartialBrokeredFuelRequestGrid.cshtml", new OrderFilterViewModel { OrderId = orderId });
        }

        public PartialViewResult OrderHistoryGrid(int id)
        {
            return PartialView("~/Areas/Supplier/Views/Shared/_PartialOrderHistoryGrid.cshtml", id);
        }

        public PartialViewResult OrderInvoicesGrid(int orderId, string poNumber = "")
        {
            var response = new InvoiceFilterViewModel();

            int invoiceAttachmentsPerEmail = 5;
            int maxInvoiceCountPerSession = 30;
            var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
            response = ContextFactory.Current.GetDomain<OrderDomain>().GetUoMandCurrencyForOrder(orderId);
            int.TryParse(masterDomain.GetApplicationSettingValue(Constants.MaxInvoiceAttachmentsPerEmailKey), out invoiceAttachmentsPerEmail);
            int.TryParse(masterDomain.GetApplicationSettingValue(Constants.MaxInvoiceCountPerSessionKey), out maxInvoiceCountPerSession);
            response.MaxInvoiceAttachmentsPerEmail = invoiceAttachmentsPerEmail;
            response.MaxInvoiceCountPerSession = maxInvoiceCountPerSession;
            //MaxInvoiceAttachmentsPerEmail = invoiceAttachmentsPerEmail, MaxInvoiceCountPerSession = maxInvoiceCountPerSession
            // return PartialView("~/Areas/Supplier/Views/Shared/_PartialInvoiceFilterGrid.cshtml", new InvoiceFilterViewModel { OrderId = orderId, PoNumber = poNumber, MaxInvoiceAttachmentsPerEmail = invoiceAttachmentsPerEmail, MaxInvoiceCountPerSession = maxInvoiceCountPerSession });
            return PartialView("~/Areas/Supplier/Views/Shared/_PartialInvoiceFilterGrid.cshtml", response);
        }
        public PartialViewResult OrderDropTicketsGrid(int orderId)
        {
            var response = new InvoiceFilterViewModel();
            try
            {
                response = ContextFactory.Current.GetDomain<OrderDomain>().GetUoMandCurrencyForOrder(orderId);               
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderBaseController", "OrderDropTicketsGrid",ex.Message,ex);
                
            }
            return PartialView("~/Areas/Supplier/Views/Shared/_PartialDigitalDropTicketFilterGrid.cshtml", response);
        }

        public PartialViewResult OrderCompletedDeliveryGrid()
        {
            return PartialView("~/Views/Shared/_PartialCompletedDeliveriesGrid.cshtml", new InvoiceFilterViewModel());
        }

        public PartialViewResult OrderVersionView(int id)
        {
            return PartialView("_PartialOrderVersionHistoryGrid", id);
        }

        public ActionResult DownloadPdf(int id, int orderDetailVersionId = 0)
        {
            using (var tracer = new Tracer("OrderController", "DownloadPdf"))
            {
                var orderPdfViewModel = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetOrderPoAsync(id, orderDetailVersionId)).Result;
                orderPdfViewModel.PhoneNumber = orderPdfViewModel.PhoneNumber.ToFormattedPhoneNumber();
                orderPdfViewModel.PoContact.PhoneNumber = orderPdfViewModel.PoContact.PhoneNumber.ToFormattedPhoneNumber();
                return GetPartialViewAsPdf("_PartialOrderPo", orderPdfViewModel.PoNumber, orderPdfViewModel, false);
            }
        }

        [HttpGet]
        public ActionResult OrderVersionHistory(int orderId)
        {
            using (var tracer = new Tracer("OrderController", "OrderVersionHistory"))
            {
                var response = ContextFactory.Current.GetDomain<OrderDomain>().GetOrderVersionsHistoryAsync(orderId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public JsonResult GetOrderPdfViewModelAsByetArray(int orderId)
        {
            using (var tracer = new Tracer("OrderBaseController", "GetOrderPdfViewModelAsByetArray"))
            {
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                OrderDomain orderDomain = ContextFactory.Current.GetDomain<OrderDomain>();

                var orderPdfModel = Task.Run(() => orderDomain.GetOrderPoAsync(orderId)).Result;

                return Json(GetPartialViewPdfContentWithDefaultMasterName("_PartialOrderPo", orderPdfModel), JsonRequestBehavior.AllowGet);
            }
        }

        #region OrderGroup Methods


        public JsonResult GetUserContext()
        {
            return Json(UserContext, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetFuelTypes(int customerId, int jobId)
        {
            var supplierCompany = CurrentUser.IsSupplierCompany ? UserContext.CompanyId : customerId;
            var buyerCompany = CurrentUser.IsSupplierCompany ? customerId : UserContext.CompanyId;
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().GetFilteredFuelProductsAsync(supplierCompany, buyerCompany, jobId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetJobsForCustomers(int companyId)
        {
            var supplierCompany = CurrentUser.IsSupplierCompany ? UserContext.CompanyId : companyId;
            var buyerCompany = CurrentUser.IsSupplierCompany ? companyId : UserContext.CompanyId;
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().GetJobsForCustomer(supplierCompany, buyerCompany);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetBlendedGroupDetails(int groupId)
        {
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().GetBlendedGroupDetailsAsync(groupId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetFilteredOrders(int customerId, int jobId, int[] tfxFuelTypeIds, int groupId)
        {
            var supplierCompany = CurrentUser.IsSupplierCompany ? UserContext.CompanyId : customerId;
            var buyerCompany = CurrentUser.IsSupplierCompany ? customerId : UserContext.CompanyId;
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().GetFilteredOrdersAsync(supplierCompany, buyerCompany, jobId, tfxFuelTypeIds, groupId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetOrdersForTierGroup(int buyerCompanyId, int supplierCompanyId, int fuelGroupType, int jobId)
        {
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().GetOrdersForTierGroup(buyerCompanyId, fuelGroupType, jobId, supplierCompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetJobsByFuelGroup(int buyerCompanyId, int supplierCompanyId, int fuelGroupType)
        {
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().GetJobsForCustomer(buyerCompanyId, fuelGroupType, supplierCompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetOrderGroupDetails(int groupId)
        {
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().GetOrderGroupDetails(groupId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrderGroup(OrderGroupViewModel groupViewModel)
        {
            using (var tracer = new Tracer("BaseController", "CreateOrderGroup"))
            {
                var ordGroupDomain = ContextFactory.Current.GetDomain<OrderGroupDomain>();
                var result = await ordGroupDomain.CreateGroup(groupViewModel, UserContext);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> EditOrderGroup(OrderGroupViewModel groupViewModel)
        {
            using (var tracer = new Tracer("BaseController", "EditOrderGroup"))
            {
                var ordGroupDomain = ContextFactory.Current.GetDomain<OrderGroupDomain>();
                StatusViewModel result = await ordGroupDomain.EditGroup(groupViewModel, UserContext);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteOrderGroup(int groupId)
        {
            using (var tracer = new Tracer("BaseController", "DeleteOrderGroup"))
            {
                var ordGroupDomain = ContextFactory.Current.GetDomain<OrderGroupDomain>();
                StatusViewModel result = await ordGroupDomain.DeleteOrderGroup(groupId, UserContext);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> InactivateOrderGroup(int groupId)
        {
            using (var tracer = new Tracer("BaseController", "InactivateOrderGroup"))
            {
                var ordGroupDomain = ContextFactory.Current.GetDomain<OrderGroupDomain>();
                StatusViewModel result = await ordGroupDomain.InactivateGroup(groupId, UserContext);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> FillOrderGroupDdl(int groupTypeId)
        {
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().FillOrderGroupDdlAsync(UserContext, groupTypeId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ViewOrderGroupDetails(OrderGroupSearchModel viewOrderGroupFilterModel)
        {
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().ViewOrderGroupsAsync(UserContext, viewOrderGroupFilterModel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}