using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Carrier.Controllers
{
    public class CalendarController : BaseController
    {
        // GET: Carrier/Calendar
        public ActionResult Index()
        {
            return View();
        }
    }
}