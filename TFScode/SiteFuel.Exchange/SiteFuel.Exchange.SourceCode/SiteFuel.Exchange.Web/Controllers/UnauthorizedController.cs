using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Controllers
{
    public class UnauthorizedController : BaseController
    {
        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult Index()
        {
            return View();
        }
    }
}