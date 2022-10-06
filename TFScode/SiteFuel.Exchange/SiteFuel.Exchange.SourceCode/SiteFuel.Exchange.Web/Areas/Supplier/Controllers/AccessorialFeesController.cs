using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier)]
    public class AccessorialFeesController : BaseController
    {
        #region Accessorial Fee related implementation 

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetViewAccessorialFeeSummary(ViewAccessorialFeeInputViewModel input)
        {
            using (var tracer = new Tracer("AccessorialFeeController", "GetViewAccessorialFeeSummary"))
            {
                var response = await ContextFactory.Current.GetDomain<AccessorialFeeDomain>().GetAccessorialFeeSummary(input, CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAccessorialFee(int accessorialFeeId)
        {
            using (var tracer = new Tracer("AccessorialFeeController", "GetAccessorialFee"))
            {
                var response = await ContextFactory.Current.GetDomain<AccessorialFeeDomain>().GetAccessorialFee(accessorialFeeId, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ArchiveAccessorialFee(int accessorialFeeId)
        {
            using (var tracer = new Tracer("AccessorialFeeController", "ArchiveAccessorialFee"))
            {
                var response = await ContextFactory.Current.GetDomain<AccessorialFeeDomain>().ArchiveAccessorialFee(accessorialFeeId, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateAccessorialFee(AccessorialFeeViewModel model)
        {
            var response = new StatusViewModel(Status.Failed);
            if (model == null || (model != null && model.Status == FreightTableStatus.Unknown))
            {
                response.StatusMessage = Resource.errorMessageInvalidFeeStatus;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                var feeResponse = await ContextFactory.Current.GetDomain<AccessorialFeeDomain>().CreateAccessorialFee(model, CurrentUser.CompanyId, CurrentUser.Id);
                return Json(feeResponse, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAccessorialFee(AccessorialFeeViewModel model)
        {
            var response = new StatusViewModel(Status.Failed);
            if (model == null || (model != null && model.Status == FreightTableStatus.Unknown))
            {
                response.StatusMessage = Resource.errorMessageInvalidFeeStatus;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else if(model.Id == 0)
            {
                response.StatusMessage = Resource.errorMessageInvalidAccessorialFee;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var feeResponse = await ContextFactory.Current.GetDomain<AccessorialFeeDomain>().CreateAccessorialFee(model, CurrentUser.CompanyId, CurrentUser.Id);
                return Json(feeResponse, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}