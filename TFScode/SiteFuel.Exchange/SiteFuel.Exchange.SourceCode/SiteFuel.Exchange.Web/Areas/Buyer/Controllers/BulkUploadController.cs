using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer)]
    public class BulkUploadController : BaseController
    {
        public ActionResult RequestStatus()
        {
            return View();
        }

        [HttpGet]
        public JsonResult BulkUploadDetails()
        {
            using (var tracer = new Tracer("BulkUploadController", "BulkUploadDetails"))
            {
                List<QueueProcessType> processTypes = new List<QueueProcessType>() { QueueProcessType.PoNumberBulkUpload, QueueProcessType.DemandCaptureUpload,QueueProcessType.TankBulkUpload, QueueProcessType.JobsBulkUpload, QueueProcessType.AssetBulkUpload };
                var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetUploadDetails(UserContext.Id, processTypes);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                //return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}