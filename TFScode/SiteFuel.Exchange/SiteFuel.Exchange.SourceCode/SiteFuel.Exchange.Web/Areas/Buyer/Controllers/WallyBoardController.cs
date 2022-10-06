using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Dispatcher;
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
    [AuthorizeCompany(CompanyType.Buyer, CompanyType.BuyerAndSupplier, CompanyType.BuyerSupplierAndCarrier)]
    public class WallyBoardController : BaseController
    {

        [HttpPost]
        public async Task<ActionResult> GetOnGoingLoadsForMapView(BuyerWhereIsMyDriverInputModel input)
        {
            using (var tracer = new Tracer("WallyBoardController", "GetOnGoingLoadsForMapView"))
            {
                if (input.FromDate == DateTimeOffset.MinValue && input.ToDate == DateTimeOffset.MinValue)
                {
                    input.FromDate = DateTimeOffset.Now.Date;
                    input.ToDate = DateTimeOffset.Now.Date;
                }
                if (string.IsNullOrWhiteSpace(input.DriverSearch))
                {
                    input.DriverSearch = string.Empty;
                }
                var response = await ContextFactory.Current.GetDomain<WallyBoardDomain>().GetOnGoingLoadsForMapView(UserContext, input);
                return Json(response, JsonRequestBehavior.DenyGet);
            }
        }

        public async Task<ActionResult> GetBuyerLoadFilterData(bool isShowCarrierManaged = false)
        {
            using (var tracer = new Tracer("WallyBoardController", "GetBuyerLoadFilterData"))
            {
                var response = await ContextFactory.Current.GetDomain<JobDomain>().GetBuyerLoadFilterData(UserContext.CompanyId, UserContext.Id,isShowCarrierManaged);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetUserCountry()
        {
            var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
            string response = await domain.GetUserCountryAsync(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<JsonResult> GetBuyerLoads(BuyerWhereIsMyDriverInputModel input)
        {
            var domain = ContextFactory.Current.GetDomain<WallyBoardDomain>();
            if (input.FromDate == DateTimeOffset.MinValue && input.ToDate == DateTimeOffset.MinValue)
            {
                input.FromDate = DateTimeOffset.Now.Date;
                input.ToDate = DateTimeOffset.Now.Date;
            }
            if (string.IsNullOrWhiteSpace(input.DriverSearch))
            {
                input.DriverSearch = string.Empty;
            }
            var dataTableSearchModel = new DataTableSearchModel(input);
            var response = await domain.GetBuyerLoadsForGrid(UserContext, input, dataTableSearchModel);
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
    }
}