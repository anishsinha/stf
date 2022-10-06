using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    public class DeliveryRequestController : BaseController
    {
        // GET: Buyer/DeliveryRequest
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("View")]
        public ActionResult GetDeliveryRequetsGridForBuyer(DeliveryReqPriority reqPriorityfilter = DeliveryReqPriority.None)
        {
            var response = ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDeliveryRequestFilter(reqPriorityfilter);
            return View("View", response);
        }

        [HttpPost]
        public async Task<JsonResult> GetDeliveryRequestsByPriority(DeliveryReqPriority priority)
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDeliveryRequestsByPriority(priority, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}