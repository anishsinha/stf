using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.SuperAdmin.Controllers
{
    [AuthorizeRole(UserRoles.SuperAdmin)]
    public class AdvertisementController : BaseController
    {
        public ActionResult ViewRequestPrices()
        {
              return View();
        }

        public ActionResult ViewRequestFuels(RequestFuelFilterType filter = RequestFuelFilterType.All)
        {
              return View(filter);
        }

        public async Task<ActionResult> GetRequestedPrices(string fromDate, string toDate)
        {
            using (var tracer = new Tracer("AdvertisementController", "GetRequestedPrices"))
            {
                var response = await ContextFactory.Current.GetDomain<AdvertisementDomain>().GetRequestedPricesAsync(fromDate, toDate);
                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public async Task<ActionResult> GetRequestedFuels(RequestFuelFilterType filter, string fromDate, string toDate)
        {
            using (var tracer = new Tracer("AdvertisementController", "GetRequestedFuels"))
            {
                var response = await ContextFactory.Current.GetDomain<AdvertisementDomain>().GetRequestedFuelsAsync(filter, fromDate, toDate);
                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public async Task<ActionResult> ChangeRequestFuelStatus(int id, RequestFuelStatusType type, bool isDone)
        {
            using (var tracer = new Tracer("AdvertisementController", "ChangeRequestFuelStatus"))
            {
                var response = await ContextFactory.Current.GetDomain<AdvertisementDomain>().ChangeRequestFuelStatusAsync(id, type, isDone);
                if (response.StatusCode != Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageUserStatusChangeFailed);
                    return PartialView("_DisplayCustomMessage");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}