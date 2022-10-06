using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.BillingStatement;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier)]
    public class BillingScheduleController : BaseController
    {
        // GET: Supplier/BIllingSchedule
        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public ActionResult Create(int id = 0)
        {
            var createBillingScheduleViewModel = ContextFactory.Current.GetDomain<BillingScheduleDomain>().GetBillingScheduleViewModel(UserContext, id);
            return View(createBillingScheduleViewModel);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public ActionResult Create(BillingScheduleViewModel viewModel)
        {
            if (viewModel.Id > 0)
            {
                var response = ContextFactory.Current.GetDomain<BillingScheduleDomain>().UpdateBillingSchedule(viewModel, UserContext);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }
            else
            {
                var response = ContextFactory.Current.GetDomain<BillingScheduleDomain>().CreateBillingSchedule(viewModel, UserContext);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }
            return RedirectToAction("View");    
        }

        [HttpGet]
        public async Task<JsonResult> GetOrdersOfBuyer(int companyId, int countryId = 1)
        {
            var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
            var response = await masterDomain.GetSupplierOrdersForBilling(companyId, UserContext.CompanyId, countryId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetTimeZoneList(int countryId)
        {
            var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
            var response = await masterDomain.GetStatementTimeZone(UserContext.CompanyId, countryId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetBuyerCompanies(int countryId = 1)
        {
            var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
            var response = await masterDomain.GetSupplierCustomersForBilling(UserContext.CompanyId, countryId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("View")]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public ActionResult Index()
        {
            return View("View", new BillingScheduleFilterViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> BillingScheduleGrid(BillingDataTableViewModel requestModel)
        {
            var response = await ContextFactory.Current.GetDomain<BillingScheduleDomain>().GetBillingScheduleGridAsync(UserContext, requestModel);

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

        [HttpGet]
        public ActionResult GetScheduleSummary(int countryId = (int)Country.USA)
        {
            var viewModel = new BillingScheduleFilterViewModel();            
            return PartialView("_PartialBillingScheduleGrid", viewModel);
        }
    }
}