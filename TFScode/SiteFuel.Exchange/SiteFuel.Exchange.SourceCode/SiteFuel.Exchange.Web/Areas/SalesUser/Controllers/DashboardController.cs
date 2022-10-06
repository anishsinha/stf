using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteFuel.Exchange.Web.Controllers;
using System.Threading.Tasks;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.SalesUser;

namespace SiteFuel.Exchange.Web.Areas.SalesUser.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.BuyerAndSupplier, CompanyType.SupplierAndCarrier, CompanyType.BuyerSupplierAndCarrier)]
    public class DashboardController : BaseController
    {
        // GET: SalesUser/Dashboard
        [AuthorizeRole(UserRoles.Admin, UserRoles.Supplier, UserRoles.Sales)]
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetSalesUserOrders()
        {
                var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().GetSalesUserOrders(UserContext.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetSalesInvoiceDashboard(int InvoiceTypeId)
        {
            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().GetSalesInvoiceDashboard(UserContext.CompanyId, InvoiceTypeId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSalesDR(List<RaiseDeliveryRequestInput> deliveryRequest)
        {
            var response = await ContextFactory.Current.GetDomain<Domain.Domain.SalesUser.DREntryDomain>().CreateDRFromSalesUser(UserContext, deliveryRequest);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ValidateSalesDR(SalesUserDRViewModel salesDRViewModel)
        {
            var response = await ContextFactory.Current.GetDomain<Domain.Domain.SalesUser.DREntryDomain>().ValidateDREntry(UserContext, salesDRViewModel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetCustomersJobsForSalesDR()
        {
            var response = await ContextFactory.Current.GetDomain<Domain.Domain.SalesUser.DREntryDomain>().GetCustomersAndJobList(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetProductsForSalesDR(int CompanyId, int JobId)
        {
            var response = await ContextFactory.Current.GetDomain<Domain.Domain.SalesUser.DREntryDomain>().GetProductsForSalesDR(CompanyId, JobId, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}