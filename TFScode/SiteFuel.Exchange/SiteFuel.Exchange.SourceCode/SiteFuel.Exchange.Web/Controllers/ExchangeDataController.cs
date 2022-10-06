using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Controllers
{
    [AllowAnonymous]
    public class ExchangeDataController : Controller
    {
        // GET: ExchangeData
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InventoryDetails(string token, int supplierId = 0)
        {
            var model = new UnAthorizedInventoryData();
            model.CompanyToken = token;
            model.SelectedSupplierId = supplierId;
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> InventoryGrid(string token, int supplierId = 0)
        {
            var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetInventoryDetailsForUnauthorizedUser(token, supplierId);
            //return response;
            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    data = response.Data,
                    //draw = requestModel.draw,
                    recordsTotal = response.Data.Count,
                    recordsFiltered = response.Data.Count
                },

                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}