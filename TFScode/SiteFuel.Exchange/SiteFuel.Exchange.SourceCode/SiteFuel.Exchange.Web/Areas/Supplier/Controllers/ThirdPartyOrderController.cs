using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier)]
    public class ThirdPartyOrderController : BaseController
    {
        // GET: Supplier/ThirdPartyOrder
        public ActionResult RequestStatus()
        {
            return View();
        }

        [HttpGet]
        public JsonResult BulkUploadDetails()
        {
            using (var tracer = new Tracer("ThirdPartyOrderController", "BulkUploadDetails"))
            {
                List<QueueProcessType> processTypes = new List<QueueProcessType>() {
                    QueueProcessType.ThirdPartyOrderBulkUpload,
                    QueueProcessType.InvoiceBulkUpload,
                    QueueProcessType.InvoiceImageUpload,
                    QueueProcessType.InvoiceUploadErrors,
                    QueueProcessType.PoNumberBulkUpload,
                    QueueProcessType.CreateFreightOnlyOrder,
                    QueueProcessType.DemandCaptureUpload,
                    QueueProcessType.ProductMappingBulkUpload,
                    QueueProcessType.TerminalItemCodeMappingBulkUpload,
                    QueueProcessType.TankBulkUpload,
                    QueueProcessType.AssetBulkUpload
                };
                var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetUploadDetails(UserContext.Id, processTypes);
                //return Json(response, JsonRequestBehavior.AllowGet);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };


            }
        }
    }
}