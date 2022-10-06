using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
			return View("Index");
		}

		

		public ViewResult GenericError()
        {
            return View("Index");
        }

		

		public ViewResult NotFound(HandleErrorInfo exception)
        {
            ViewBag.Title = "Page Not Found";
            return View("Index");
        }
    }
}