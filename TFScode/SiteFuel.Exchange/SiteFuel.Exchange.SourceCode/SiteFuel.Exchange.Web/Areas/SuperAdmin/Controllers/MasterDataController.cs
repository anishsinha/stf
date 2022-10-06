using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SiteFuel.Exchange.Web.Attributes;

namespace SiteFuel.Exchange.Web.Areas.SuperAdmin.Controllers
{
    public class MasterDataController : BaseController
    {
        public ActionResult ViewAppSettings()
        {
             return View();
        }

        [HttpGet]
        public ActionResult EditAppSettings(int id)
        {
            using (var tracer = new Tracer("MasterDataController", "EditAppSettings"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetAppSettings(id);
                return View(response);
            }
        }

        [HttpPost]
        public ActionResult EditAppSettings(AppSettingViewModel viewModel)
        {
            using (var tracer = new Tracer("MasterDataController", "EditAppSettings(viewModel)"))
            {
                viewModel.UpdatedBy = CurrentUser.Id;
                viewModel.UpdatedDate = DateTimeOffset.Now;

                var response = ContextFactory.Current.GetDomain<MasterDomain>().SaveAppSettings(viewModel);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                if(response.StatusCode == Status.Success)
                {
                    return RedirectToAction("ViewAppSettings", "MasterData", new { area = "SuperAdmin" });
                }

                return View(viewModel);
            }
        }

        public async Task<ActionResult> GetAppSettings()
        {
            using (var tracer = new Tracer("MasterDataController", "GetAppSettings"))
            {
                var response = await ContextFactory.Current.GetDomain<MasterDomain>().GetAppSettingsAsync();
                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public ActionResult NotificationSettings()
        {
            using (var tracer = new Tracer("MasterDataController", "NotificationSettings"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetDefaultNotificationSettings();
                return View(response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public ActionResult GetEventGroupDetails(string eventGroupId)
        {
            using (var tracer = new Tracer("MasterDataController", "NotificationSettings"))
            {
                var response = new NotificationSettingsViewModel();
                
                response = ContextFactory.Current.GetDomain<MasterDomain>().GetNotificationGroupDetails(eventGroupId);
                
                return PartialView("_PartialUserNotificationDetails", response);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public ActionResult UpdateUserNotifications(int companyTypeId, int eventTypeId, string roleIds, bool isEmail, bool isSms)
        {
            using (var tracer = new Tracer("MasterDataController", "UpdateUserNotifications"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().UpdateRolesForNotification(UserContext.Id, companyTypeId,
                    eventTypeId, roleIds, isEmail, isSms);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public ActionResult updateUserRoleForSelectedEvent(int eventTypeId, string buyerRolesIds, string supplierRoleIds,
                bool isEmail, bool isSms, bool isForBuyerUsers, bool isForSupplierUsers)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("MasterDataController", "updateUserRoleForSelectedEvent"))
            {
                if (isForBuyerUsers)
                {
                    response = ContextFactory.Current.GetDomain<MasterDomain>().UpdateUserRolesForSelectedEvent(UserContext.Id, (int)CompanyType.Buyer,
                        eventTypeId, buyerRolesIds, isEmail, isSms);
                }

                if (isForSupplierUsers)
                {
                    response = ContextFactory.Current.GetDomain<MasterDomain>().UpdateUserRolesForSelectedEvent(UserContext.Id, (int)CompanyType.Supplier,
                        eventTypeId, supplierRoleIds, isEmail, isSms);
                }

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public ActionResult GetNotificationTemplates(int eventId, int notificationType)
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetNotificationEventTemplate(eventId, notificationType);

            return PartialView("_PartialNotificationTemplate", response);
        }

        public ActionResult ViewPricingSettings()
        {
            return View();
        }

        public async Task<ActionResult> GetPricingConfigSettings()
        {
            using (var tracer = new Tracer("MasterDataController", "GetPricingConfigSettings"))
            {
                var response = await ContextFactory.Current.GetDomain<PricingServiceDomain>().GetPricingConfigSettingsAsync();
                return new JsonResult
                {
                    Data = response.ConfigList,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditPricingConfig(int id)
        {
            var model = new PricingConfigViewModel();
            using (var tracer = new Tracer("MasterDataController", "EditPricingConfig"))
            {
                var response = await ContextFactory.Current.GetDomain<PricingServiceDomain>().GetPricingConfigSettingsAsync(id);
                if (response != null && response.Status == Status.Success && response.Config != null)
                {                   
                    model = response.Config;
                }
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditPricingConfigSettings(PricingConfigViewModel viewModel)
        {
            using (var tracer = new Tracer("MasterDataController", "EditPricingConfigSettings(viewModel)"))
            {
                viewModel.UpdatedBy = "1";
                var response = await ContextFactory.Current.GetDomain<PricingServiceDomain>().EditPricingConfigSettingsAsync(viewModel);
                if (response.Status == Status.Success)
                {
                    // update App Settings of exchange if same key exists in exchange
                    if (response.Config != null && !string.IsNullOrWhiteSpace(response.Config.Key))
                    {
                        var appSettingModel = new AppSettingViewModel() { Key = response.Config.Key, Value = response.Config.Value, UpdatedBy = 1, UpdatedDate = DateTimeOffset.Now };
                        var exchangeResponse = ContextFactory.Current.GetDomain<MasterDomain>().SaveAppSettingByKey(appSettingModel);
                    }

                    return RedirectToAction("ViewPricingSettings", "MasterData", new { area = "SuperAdmin" });
                }

                return View(viewModel);
            }
        }
    }
}