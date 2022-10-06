using System.Web.Mvc;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;

namespace SiteFuel.Exchange.Web.Areas.SuperAdmin.Controllers
{
    [AuthorizeRole(UserRoles.SuperAdmin)]
    public class ExceptionController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("Exception");
        }

        [HttpPost]
        public ActionResult GetExceptionGrid(ExceptionDataTableModel exceptionModel)
        {
            using (var tracer = new Tracer("ExceptionController", "GetExceptionGrid"))
            {
                var dataTableSearchModel = new DataTableSearchModel(exceptionModel);
                var exceptionDomain = ContextFactory.Current.GetDomain<ExceptionLogDomain>();
                var response = exceptionDomain.GetParticularExceptionsAsync(exceptionModel, dataTableSearchModel);
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;
                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = exceptionModel.draw,
                        data = response,
                        recordsTotal = totalCount,
                        recordsFiltered = totalCount
                    },
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public JsonResult GetException(int Id)
        {
            using (var tracer = new Tracer("ExceptionController", "GetException"))
            {
                var exceptionDomain = ContextFactory.Current.GetDomain<ExceptionLogDomain>();
                var response = exceptionDomain.GetException(Id);
                return new JsonResult()
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}
