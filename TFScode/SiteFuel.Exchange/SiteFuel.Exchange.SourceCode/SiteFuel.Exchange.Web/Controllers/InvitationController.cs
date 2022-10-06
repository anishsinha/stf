using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Controllers
{
    [AllowAnonymous]
    public class InvitationController : BaseController
    {
        // GET: Invitation
        public async Task<ActionResult> Index(string token="")
        {
            if (!string.IsNullOrEmpty(token))
            {
                var response = await ContextFactory.Current.GetDomain<InvitationDomain>().GetCarrierOnboardingForBranding(token);
                ViewBag.FaviconPath = response.FaviconFilePath;
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Save(ThirdPartyCompanyInviteViewModel thirdPartyCompanyInviteViewModel)
        {
            var response = await ContextFactory.Current.GetDomain<InvitationDomain>().SaveThirdPartyInvitation(thirdPartyCompanyInviteViewModel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetThirdPartyCompanyTypes()
        {
            var response = new List<DropdownDisplayItem>();
            var enumValueArray = Enum.GetValues(typeof(InvitedCompanyType));
            foreach (int item in enumValueArray)
                response.Add(new DropdownDisplayItem() {Id = item, Name = EnumHelperMethods.GetDisplayName((InvitedCompanyType)item) });
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCitiesFromStates(string stateIds)
        {
            var response = ContextFactory.Current.GetDomain<InvitationDomain>().GetCitiesAndZipFromStates(stateIds);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> GetCityAndZipsByState(string stateIds)
        {
            var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetCityAndZipsByState(stateIds);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetPhoneTypes()
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetPhoneTypes();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> IsEmailExist(string email)
        {
            bool response = await ContextFactory.Current.GetDomain<HelperDomain>().IsEmailExistAsync(email.Trim(), true);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStateList()
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetStateList();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCompanies()
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetCompanies()).Result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllTrailerAssetTypes()
        {
            var response = new { FuelTrailerAssetType = new List<DropdownDisplayItem>(), DefTrailerAssetType = new List<DropdownDisplayItem>() };

            foreach (int item in Enum.GetValues(typeof(FuelTrailerAssetType)))
                response.FuelTrailerAssetType.Add(new DropdownDisplayItem() { Id = item, Name = EnumHelperMethods.GetDisplayName((FuelTrailerAssetType)item) });

            foreach (int item in Enum.GetValues(typeof(DefTrailerAssetType)))
                response.DefTrailerAssetType.Add(new DropdownDisplayItem() { Id = item, Name = EnumHelperMethods.GetDisplayName((DefTrailerAssetType)item) });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetCarrierOnboardingForBranding(string token)
        {
            var response = await ContextFactory.Current.GetDomain<InvitationDomain>().GetCarrierOnboardingForBranding(token);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}