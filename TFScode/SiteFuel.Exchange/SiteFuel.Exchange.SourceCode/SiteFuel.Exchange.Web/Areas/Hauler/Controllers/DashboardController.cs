using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Hauler.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Hauler/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}