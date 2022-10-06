using Microsoft.Owin.Security;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Forcasting;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Settings.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer, CompanyType.Supplier, CompanyType.Carrier)]
    public class ProfileController : BaseController
    {
        [HttpGet]
        public ActionResult CompanyInformation()
        {
            using (var tracer = new Tracer("ProfileController", "CompanyInformation"))
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCompanyProfileTab(int userId)
        {
            using (var tracer = new Tracer("ProfileController", "GetCompanyProfileTab"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanyInformationAsync(userId);
                return PartialView("~/Areas/Settings/Views/Shared/_PartialCompanyProfile.cshtml", response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CompanyDetails(int Id)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanyInformationByIdAsync(Id);
                return View("CompanyInformation", response);
            }
        }

        public PartialViewResult CompanyProfilePartial(int Id)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyProfilePartial"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanyInformationByIdAsync(Id)).Result;
                return PartialView("_CompanyProfile", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CompanyInformation(CompanyInformationViewModel viewModel, HttpPostedFileBase imageFile)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyInformation(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.Company.UpdatedBy = CurrentUser.Id;

                    if (imageFile != null)
                    {
                        if (!viewModel.Company.CompanyLogo.IsRemoved)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                imageFile.InputStream.CopyTo(ms);
                                viewModel.Company.CompanyLogo.Data = ms.GetBuffer();
                            }
                            await viewModel.Company.CompanyLogo.UploadImageToAzureBlobService(ApplicationConstants.CompanyLogoFileNamePrefix, BlobContainerType.CompanyProfile);
                        }
                    }

                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveCompanyInformationAsync(viewModel.Company);
                    viewModel.StatusCode = response.StatusCode;
                    viewModel.StatusMessage = response.StatusMessage;

                    DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                }
                return View(viewModel);
            }
        }

        [HttpGet]
        public ActionResult CompanyAddresses(int id = 0)
        {
            return View("CompanyAddressGrid", id == 0 ? CurrentUser.CompanyId : id);
        }
        
        public ActionResult CompanyAddressesPartial(int id)
        {
            return PartialView("~/Views/Shared/_PartialAngularView.cshtml", id);
        }
        
                [HttpGet]
        public async Task<ActionResult> CompanyAddressGrid(int companyId = 0)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyAddressGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanyAddressListAsync(companyId == 0 ? CurrentUser.CompanyId : companyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CompanyAddress(int id = 0, int companyId = 0)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyAddress"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanyAddressSettingsAsync(companyId == 0 ? CurrentUser.CompanyId : companyId, id);
                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CompanyAddress(CompanyAddressViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyAddress(viewModel)"))
            {
                if (UserContext.CompanyTypeId != CompanyType.Buyer && (viewModel.ServiceOffering == null || !viewModel.ServiceOffering.Any(t => (t.AreaWide == ServiceAreaType.StateWide && t.StateIds.Any()) || (t.AreaWide == ServiceAreaType.ZipWide && t.ZipCodes.Any()))))
                {
                    DisplayCustomMessages((MessageType)Status.Failed, "Please add at least one of the service offering.");
                    return View(viewModel);
                }
                if (ModelState.IsValid)
                {
                    viewModel.UpdatedBy = CurrentUser.Id;

                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveCompanyAddressSettingsAsync(viewModel);

                    viewModel.StatusCode = response.StatusCode;
                    viewModel.StatusMessage = response.StatusMessage;

                    DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                    if (viewModel.StatusCode != Status.Success)
                    {
                        return View(viewModel);
                    }

                    if (CurrentUser.IsSuperAdmin)
                    {
                        return Redirect("/SuperAdmin/SuperAdmin/CompanyDetails?id="+ viewModel.CompanyId);
                    }
                    else
                    {
                        return RedirectToAction("CompanyAddresses", "Profile", new { area = "Settings" });
                    }
                }
                return View(viewModel);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> ChangeCompanyAddressStatus(int id, bool isActive)
        {
            using (var tracer = new Tracer("ProfileController", "ChangeCompanyAddressStatus"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().ChangeCompanyAddressSettingsStatusAsync(id, CurrentUser.Id, isActive);
                if (response.StatusCode != Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return PartialView("_DisplayCustomMessage");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CompanyPaymentDetails()
        {
            using (var tracer = new Tracer("ProfileController", "CompanyPaymentDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanySettingDetailsAsync(CurrentUser.CompanyId);
                return PartialView("CompanyPaymentSetting", response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CompanyPaymentsGrid()
        {
            using (var tracer = new Tracer("ProfileController", "CompanyPaymentsGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanyPaymentCardListAsync(CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CompanyPaymentInformation(int id = 0)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyPaymentInformation"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanyPaymentInformationAsync(CurrentUser.CompanyId, id);
                response.UserId = CurrentUser.Id;

                if (response.StatusCode != Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CompanyPaymentInformation(PaymentViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyPaymentInformation(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveCompanyPaymentInformationAsync(viewModel);
                    viewModel.StatusCode = response.StatusCode;
                    viewModel.StatusMessage = response.StatusMessage;

                    DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);

                    if (response.StatusCode == Status.Success)
                    {
                        //Settings / TaxExemption / CompanyTaxes
                        return RedirectToAction("CompanyTaxes", "TaxExemption", new { area = "Settings" });
                    }
                }

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin)]
        public async Task<ActionResult> SaveCompanySetting(CompanySettingViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "SaveCompanySetting"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveCompanySettingAsync(viewModel, UserContext);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
            }
            ///Settings/TaxExemption/CompanyTaxes
            return RedirectToAction("CompanyTaxes", "TaxExemption", new { area = "Settings" });
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> DeleteCompanyPaymentInformation(int id)
        {
            using (var tracer = new Tracer("ProfileController", "DeleteCompanyPaymentInformation"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().DeleteCompanyPaymentInformationAsync(id);

                if (response.StatusCode != Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return PartialView("_DisplayCustomMessage");
                }
                return RedirectToAction("CompanyTaxes", "TaxExemption", new { area = "Settings" });
            }
        }

        [HttpGet]
        public async Task<ActionResult> UserInformation()
        {
            using (var tracer = new Tracer("ProfileController", "UserInformation"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetUserAsync(CurrentUser.Id);
                return View(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> UserDetails(int id)
        {
            using (var tracer = new Tracer("ProfileController", "UserDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetUserAsync(id);
                return View("UserInformation", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserInformation(UserViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "UserInformation"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.UpdatedBy = CurrentUser.Id;

                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveUserInformationAsync(viewModel);

                    viewModel.StatusCode = (AuthStatus)response.StatusCode;
                    viewModel.StatusMessage = response.StatusMessage;

                    DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);

                    if (response.StatusCode == Status.Success && viewModel.Email == CurrentUser.Email)
                    {
                        var Identity = new ClaimsIdentity(User.Identity);
                        Identity.RemoveClaim(Identity.FindFirst(ApplicationSecurityConstants.UserName));
                        Identity.AddClaim(new Claim(ApplicationSecurityConstants.UserName, $"{viewModel.FirstName} {viewModel.LastName}"));
                        var rememberMe = Identity.FindFirst(ApplicationSecurityConstants.RememberMe);
                        AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                                                                                new ClaimsPrincipal(Identity),
                                                                                new AuthenticationProperties
                                                                                {
                                                                                    IsPersistent = rememberMe != null ? Convert.ToBoolean(rememberMe.Value) : true
                                                                                });
                    }
                    if (viewModel.Id == CurrentUser.Id)
                    {
                        return RedirectToAction("UserDetails", "Profile", new { area = "Settings", id = viewModel.Id });
                    }
                    else if (CurrentUser.IsSuperAdmin)
                    {
                        if (viewModel.Roles.Any(t => t.Id == (int)UserRoles.SuperAdmin))
                        {
                            return RedirectToAction("ViewSiteFuelUsers", "SuperAdmin", new { area = "SuperAdmin", filter = SiteFuelUserFilterType.AllSuperAdmin });
                        }
                        else if (viewModel.Roles.Any(t => t.Id == (int)UserRoles.InternalSalesPerson))
                        {
                            return RedirectToAction("ViewSiteFuelUsers", "SuperAdmin", new { area = "SuperAdmin", filter = SiteFuelUserFilterType.InternalSalesPerson });
                        }
                        else if (viewModel.Roles.Any(t => t.Id == (int)UserRoles.AccountSpecialist))
                        {
                            return RedirectToAction("ViewSiteFuelUsers", "SuperAdmin", new { area = "SuperAdmin", filter = SiteFuelUserFilterType.AccountSpecialist });
                        }
                        else if (viewModel.Roles.Any(t => t.Id == (int)UserRoles.ExternalVendor))
                        {
                            return RedirectToAction("ViewSiteFuelUsers", "SuperAdmin", new { area = "SuperAdmin", filter = SiteFuelUserFilterType.ExternalVendor });
                        }
                        else if (viewModel.Roles.Any() && viewModel.CompanyId > 0)
                        {
                            return RedirectToAction("CompanyUsers", "SuperAdmin", new { area = "SuperAdmin", id = viewModel.CompanyId });
                        }
                    }
                }
                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ResendActivationLink(int userId, bool isInvitedUser = false)
        {
            using (var tracer = new Tracer("ProfileController", "ResendActivationLink"))
            {
                var domain = new AuthenticationDomain();
                if (!isInvitedUser)
                {
                    await new NotificationDomain(domain).AddNotificationEventAsync(EventType.SendActivationLink, userId, CurrentUser.Id, null, null, CurrentUser.ApplicationTemplateId);
                }
                else
                {
                    await new NotificationDomain(domain).AddNotificationEventAsync(EventType.AdditionalUserAdded, userId, CurrentUser.Id, null, null, CurrentUser.ApplicationTemplateId);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserNotifications()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> UserNotificationsGrid()
        {
            using (var tracer = new Tracer("ProfileController", "UserNotificationsGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetUserNotificationSettingsAsync(CurrentUser.Id, (int)CurrentUser.CompanyTypeId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UserNotifications(UserNotificationsViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "UserNotifications"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.UserId = CurrentUser.Id;
                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveUserNotificationSettingsAsync(viewModel);
                    viewModel.StatusCode = response.StatusCode;
                    viewModel.StatusMessage = response.StatusMessage;
                    DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

                return View(viewModel);
            }
        }

        [HttpGet]
        public ActionResult CompanyUsers()
        {
            using (var tracer = new Tracer("ProfileController", "CompanyUsers"))
            {
                return View("CompanyUsersGrid", CurrentUser.CompanyId);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CompanyInvitedUsersGrid(int companyId = 0)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyInvitedUsersGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetInvitedUserListAsync(companyId, UserContext.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<ActionResult> CompanyOnboardedUsersGrid()
        {
            using (var tracer = new Tracer("ProfileController", "CompanyOnboardedUsersGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetOnboardedUserListAsync(CurrentUser.CompanyId, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> CompanyOnboardedApiUsers()
        {
            using (var tracer = new Tracer("ProfileController", "CompanyOnboardedApiUsers"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().CompanyOnboardedApiUsers(CurrentUser.CompanyId, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetApiUserForm(ApiUserViewModel apiUser)
        {
            return PartialView("_PartialApiUserForm", apiUser);
        }

        [HttpPost]
        public async Task<JsonResult> SetApiUserPassword(ApiUserViewModel apiUser)
        {
            using (var tracer = new Tracer("ProfileController", "SetApiUserPassword"))
            {
                var response = new StatusViewModel();

                response = await ContextFactory.Current.GetDomain<SettingsDomain>().SetApiUserPassword(apiUser, UserContext);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CompanyNotificationSettings()
        {
            using (var tracer = new Tracer("ProfileController", "CompanyNotificationSettings"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetUserListForManageAlertsAsync(CurrentUser.CompanyId, CurrentUser.Id);
                return View(response);
            }
        }

        [HttpPost]
        public ActionResult UpdateUserEvents(int userId, string events)
        {
            using (var tracer = new Tracer("ProfileController", "UpdateUserEvents"))
            {
                var response = ContextFactory.Current.GetDomain<SettingsDomain>().UpdateUserEvents(userId, events);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CompanyAdditionalUsers(int id = 0, bool isInvitedUser = true, int companyId = 0)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyAdditionalUsers"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetInvitedAdditionalUsersAsync(id, isInvitedUser, companyId);
                response.UserId = CurrentUser.Id;

                response.DisplayMode = (id == 0) ? PageDisplayMode.Create : PageDisplayMode.Edit;

                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> AddCompanyUsers(AdditionalUsersViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "AddCompanyUsers"))
            {
                ValidateDriverTrailerType(viewModel);
                if (ModelState.IsValid)
                {
                    var duplicate = viewModel.AdditionalUsers
                                                .GroupBy(x => x.Email)
                                                .Where(group => group.Count() > 1)
                                                .Select(group => group.Key).ToList();

                    if (duplicate != null && duplicate.Count > 0)
                    {
                        string duplicateEmails = string.Join(", ", duplicate);
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageDuplicateEntries,
                            new[] { duplicate.Count == 1 ? $"{duplicateEmails} is" : $"{duplicateEmails} are" });

                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                    }
                    else
                    {
                        viewModel.AdditionalUsers.ForEach(t => t.CompanyId = CurrentUser.CompanyId);
                        viewModel.SupplierURL = CurrentUser.SupplierURL;
                        viewModel.ApplicationTemplateId = CurrentUser.ApplicationTemplateId;
                        var response = await ContextFactory.Current.GetDomain<SettingsDomain>().CreateAdditionalUsersAsync(viewModel);

                        viewModel.StatusCode = response.StatusCode;
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessages.ToList());
                    }

                    if (viewModel.StatusCode == Status.Failed)
                    {
                        viewModel.DisplayMode = PageDisplayMode.Create;
                        return View("CompanyAdditionalUsers", viewModel);
                    }
                }
                return RedirectToAction("CompanyUsers", "Profile", new { area = "Settings" });
            }
        }

        private void ValidateDriverTrailerType(AdditionalUsersViewModel viewModel)
        {
            if (viewModel.AdditionalUsers != null)
            {
                foreach (var item in viewModel.AdditionalUsers)
                {
                    var roleName = ContextFactory.Current.GetDomain<MasterDomain>().GetRoleName(item.RoleIds);
                    if (!roleName.Contains(UserRoles.Driver.ToString()))
                    {
                        var LicenseTypeId = ModelState.Where(x => x.Key.Contains("LicenseTypeId") && x.Value.Errors.Count() > 0).ToList();
                        if (LicenseTypeId != null)
                        {
                            foreach (var licenseTypeIdItem in LicenseTypeId)
                            {
                                ModelState.Remove(licenseTypeIdItem.Key);

                            }

                        }
                        var expiryDate = ModelState.Where(x => x.Key.Contains("ExpiryDate") && x.Value.Errors.Count() > 0).ToList();
                        if (expiryDate != null)
                        {
                            foreach (var expiryDateItem in expiryDate)
                            {
                                ModelState.Remove(expiryDateItem.Key);

                            }
                        }

                    }
                }

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> EditCompanyUser(AdditionalUsersViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "EditCompanyUser"))
            {
                ValidateDriverTrailerType(viewModel);
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().EditAdditionalUsersAsync(viewModel);

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Failed)
                    {
                        viewModel.DisplayMode = PageDisplayMode.Edit;
                        return View("CompanyAdditionalUsers", viewModel);
                    }
                }
                return RedirectToAction("CompanyUsers", "Profile", new { area = "Settings" });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> ChangeUserStatus(int id, bool isActive)
        {
            using (var tracer = new Tracer("ProfileController", "ChangeUserStatus"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().ChangeUserStatusAsync(CurrentUser.Id, id, isActive);
                if (response.StatusCode != Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageUserStatusChangeFailed);
                    return PartialView("_DisplayCustomMessage");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> ChangeAllowSalesCalculatorStatus(int id, bool isAllowed)
        {
            using (var tracer = new Tracer("ProfileController", "ChangeAllowSalesCalculatorStatus"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().ChangeAllowSalesCalculatorStatusAsync(id, isAllowed);
                if (response.StatusCode != Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageUserStatusChangeFailed);
                    return PartialView("_DisplayCustomMessage");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> DeleteInvitedUser(int id, bool IsScheduleExists, string ScheduleBuilderIdInfo)
        {
            using (var tracer = new Tracer("ProfileController", "DeleteInvitedUser"))
            {
                List<string> ScheduleBuilderIds = new List<string>();
                if (IsScheduleExists)
                {
                    ScheduleBuilderIds = ScheduleBuilderIdInfo.Split(',').ToList();
                }
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().DeleteInvitedUser(UserContext, id, ScheduleBuilderIds, IsScheduleExists);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Buyer, CompanyType.Supplier)]
        public ActionResult PrivateSupplierListGrid()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Buyer, CompanyType.Supplier)]
        public async Task<ActionResult> PrivateSupplierLists()
        {
            using (var tracer = new Tracer("ProfileController", "PrivateSupplierLists"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetPrivateSupplierListsAsync(CurrentUser.CompanyId);
                return Json(new { data = response }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Buyer, CompanyType.Supplier)]
        public async Task<ActionResult> PrivateSupplierList(int id = 0)
        {
            using (var tracer = new Tracer("ProfileController", "PrivateSupplierList"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetPrivateSupplierListDetails(id);
                response.DisplayMode = id == 0 ? PageDisplayMode.Create : PageDisplayMode.Edit;
                return View(response);
            }
        }

        public ActionResult AddPrivateSupplierList()
        {
            return PartialView("_PartialPrivateSupplierList", new PrivateSupplierListViewModel() { IsNewSupplierList = true, DisplayMode = PageDisplayMode.Create });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> AddPrivateSupplierLists(PrivateSupplierListsViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "AddPrivateSupplierLists"))
            {
                if (ModelState.IsValid)
                {
                    var duplicate = viewModel.PrivateSupplierLists
                                                .GroupBy(x => x.Name)
                                                .Where(group => group.Count() > 1)
                                                .Select(group => group.Key).ToList();

                    if (duplicate != null && duplicate.Count > 0)
                    {
                        string duplicateNames = string.Join(", ", duplicate);
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageDuplicateEntries,
                            new[] { duplicate.Count == 1 ? $"{duplicateNames} is" : $"{duplicateNames} are" });

                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                    }
                    else
                    {
                        viewModel.UserId = CurrentUser.Id;
                        viewModel.CompanyId = CurrentUser.CompanyId;
                        var response = await ContextFactory.Current.GetDomain<SettingsDomain>().AddPrivateSupplierListsAsync(viewModel);

                        viewModel.StatusCode = response.StatusCode;
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessages.ToList());
                    }

                    if (viewModel.StatusCode == Status.Failed)
                    {
                        viewModel.DisplayMode = PageDisplayMode.Create;
                        return View("PrivateSupplierList", viewModel);
                    }
                }
                return RedirectToAction("PrivateSupplierListGrid", "Profile", new { area = "Settings" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> EditPrivateSupplierList(PrivateSupplierListsViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "EditPrivateSupplierList"))
            {
                //if (ModelState.IsValid)
                {
                    viewModel.CompanyId = CurrentUser.CompanyId;
                    viewModel.UserId = CurrentUser.Id;
                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().EditPrivateSupplierListAsync(viewModel);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Failed)
                    {
                        viewModel.DisplayMode = PageDisplayMode.Edit;
                        return View("PrivateSupplierList", viewModel);
                    }
                }
                return RedirectToAction("PrivateSupplierListGrid", "Profile", new { area = "Settings" });
            }
        }
        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Delete(int id)
        {
            using (var tracer = new Tracer("ProfileController", "Delete"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().DeletePrivateSupplierListAsync(id);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                if (response.StatusCode == Status.Failed)
                {
                    return RedirectToAction("EditPrivateSupplierList", "Profile", new { area = "Settings", id = id });
                }
                else
                {
                    return RedirectToAction("PrivateSupplierListGrid", "Profile", new { area = "Settings" });
                }
            }
        }

        public ActionResult InviteAdditionalUsers()
        {
            using (var tracer = new Tracer("ProfileController", "InvitedCompaniesGrid"))
            {
                return PartialView("_PartialAdditionalUsers", new AdditionalUserViewModel());
            }
        }

        public ActionResult InvitedCompanies()
        {
            return View();
        }
        public async Task<ActionResult> InvitedCompaniesGrid()
        {
            using (var tracer = new Tracer("ProfileController", "InvitedCompaniesGrid"))
            {
                
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetInvitedCompaniesListAsync(CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InviteCompanies()
        {
            return View(new AdditionalUsersViewModel() { DisplayMode = PageDisplayMode.Create });
        }

        public ActionResult InviteMoreCompanies()
        {
            return PartialView("_PartialInviteCompanies", new AdditionalUserViewModel() { DisplayMode = PageDisplayMode.Create });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InviteCompanies(AdditionalUsersViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "InviteCompanies(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    var duplicate = viewModel.AdditionalUsers
                                                .GroupBy(x => x.Email)
                                                .Where(group => group.Count() > 1)
                                                .Select(group => group.Key).ToList();

                    if (duplicate != null && duplicate.Count > 0)
                    {
                        string duplicateEmails = string.Join(", ", duplicate);
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageDuplicateEntries,
                            new[] { duplicate.Count == 1 ? $"{duplicateEmails} is" : $"{duplicateEmails} are" });

                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                    }
                    else
                    {
                        viewModel.UserId = CurrentUser.Id;
                        var response = await ContextFactory.Current.GetDomain<SettingsDomain>().InviteCompaniesAsync(viewModel);

                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessages.ToList());

                        if (response.StatusCode == Status.Success)
                        {
                            return RedirectToAction("InvitedCompanies", "Profile", new { area = "Settings" });
                        }
                    }
                }
                return View("InviteCompanies", viewModel);
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditInvitedCompany(int id)
        {
            using (var tracer = new Tracer("ProfileController", "EditInvitedCompany"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetInvitedCompanyDetailsAsync(id);
                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditInvitedCompany(AdditionalUserViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "EditInvitedCompany(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().EditInvitedCompanyAsync(viewModel);

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Failed)
                    {
                        return View(viewModel);
                    }
                }
                return RedirectToAction("InvitedCompanies", "Profile", new { area = "Settings" });
            }
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<ActionResult> CreditApp()
        {
            using (var tracer = new Tracer("ProfileController", "CreditApp"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCreditAppDetailsAsync(CurrentUser.CompanyId);
                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCompany(CompanyType.Supplier)]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> CreditApp(CreditAppViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "CreditApp(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.UpdatedBy = CurrentUser.Id;
                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveCreditAppDetailsAsync(viewModel);

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Failed)
                    {
                        return View(viewModel);
                    }
                }
                return View(viewModel);
            }
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<ActionResult> GetCreditAppMaterial()
        {
            using (var tracer = new Tracer("ProfileController", "GetCreditAppMaterial"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCreditAppMaterialAsync(CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCompany(CompanyType.Supplier)]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> UploadDocuments(HttpPostedFileBase[] files)
        {
            using (var tracer = new Tracer("ProfileController", "UploadDocuments"))
            {
                List<DocumentViewModel> documents = new List<DocumentViewModel>();
                await CheckForExistingFiles(files);
                if (files != null && files.Length > 0 && !Directory.Exists(Server.MapPath("~/Documents")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Documents"));
                }
                if (files != null && files.Length > 0)
                {
                    foreach (HttpPostedFileBase file in files)
                    {
                        if (file != null)
                        {
                            var inputFileName = Path.GetFileName(file.FileName);
                            var modifiedFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                            var serverSavePath = Path.Combine(Server.MapPath("~/Documents/") + modifiedFileName);
                            file.SaveAs(serverSavePath);
                            documents.Add(new DocumentViewModel()
                            {
                                FileName = inputFileName,
                                ModifiedFileName = modifiedFileName,
                                FilePath = serverSavePath,
                                AddedBy = CurrentUser.Id,
                                CompanyId = CurrentUser.CompanyId
                            });
                        }
                    }
                }
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveCreditAppDocumentsAsync(documents, CurrentUser.CompanyId);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);

                return RedirectToAction("CreditApp", "Profile", new { area = "Settings" });
            }
        }
        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier)]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> DeleteCreditAppDocument(int id)
        {
            using (var tracer = new Tracer("ProfileController", "DeleteCreditAppDocument"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().DeleteCreditAppDocumentAsync(id, CurrentUser.CompanyId);

                if (response.StatusCode == Status.Success)
                {
                    if (System.IO.File.Exists(response.FilePath))
                    {
                        System.IO.File.Delete(response.FilePath);
                    }
                }
                else
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return PartialView("_DisplayCustomMessage");
                }
                StatusViewModel viewModel = new StatusViewModel() { StatusCode = response.StatusCode, StatusMessage = response.StatusMessage };
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeCompany(CompanyType.Supplier)]
        public async Task<FilePathResult> DownloadCreditAppDocument(int id)
        {
            using (var tracer = new Tracer("ProfileController", "DownloadCreditAppDocument"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCreditAppDocumentDetailsAsync(id);
                return File(response.FilePath, "multipart/form-data", response.FileName);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CompanyBlacklist()
        {
            using (var tracer = new Tracer("ProfileController", "CompanyBlacklist"))
            {
                TimeSpan TimeZoneOffset = GetBrowserTimeZoneOffset();
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanyBlacklistAsync(CurrentUser.CompanyId, TimeZoneOffset);
                return View("CompanyBlacklist", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CompanyBlacklist(CompanyBlacklistGridViewModel blackList)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyBlacklist"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().AddCompanyToBlacklistAsync(CurrentUser.Id, CurrentUser.CompanyId, blackList.SelectedCompanyId, blackList.Reason);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                return RedirectToAction("CompanyBlacklist", "Profile", new { area = "Settings" });
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<JsonResult> RemoveFromBlacklist(int selectedCompanyId)
        {
            using (var tracer = new Tracer("ProfileController", "RemoveFromBlacklist"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().RemoveCompanyFromBlacklistAsync(CurrentUser.Id, CurrentUser.CompanyId, selectedCompanyId);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return Json(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> TimeCardSetting()
        {
            using (var tracer = new Tracer("ProfileController", "TimeCardSetting"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetTimeCardSettings(CurrentUser.CompanyId);
                return View("TimeCardSetting", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin)]
        public async Task<ActionResult> TimeCardSetting(bool isTimeCardEnabled)
        {
            using (var tracer = new Tracer("ProfileController", "TimeCardSetting(bool)"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().UpdateTimeCardSettings(CurrentUser.Id, CurrentUser.CompanyId, isTimeCardEnabled);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                return RedirectToAction("TimeCardSetting", "Profile", new { area = "Settings" });
            }
        }

        [HttpGet]
        public async Task<ActionResult> FavoriteFuels()
        {
            var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetOtherFuelListAsync(CurrentUser.CompanyId);
            return View("FavoriteFuels", response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCompany(CompanyType.Buyer, CompanyType.BuyerAndSupplier)]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> FavoriteFuels(FavoriteFuelViewModel favoriteFuels)
        {
            var domain = ContextFactory.Current.GetDomain<SettingsDomain>();
            var response = await domain.SaveFavoriteFuelsAsync(CurrentUser.CompanyId, CurrentUser.Id, favoriteFuels);
            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            var favoriteFuelTyes = await domain.GetOtherFuelListAsync(CurrentUser.CompanyId);
            if (response.StatusCode == Status.Success)
            {
                favoriteFuels = favoriteFuelTyes;
            }
            else
            {
                favoriteFuels.FuelTypeList = favoriteFuelTyes.FuelTypeList;
            }
            return View("FavoriteFuels", favoriteFuels);
        }

        [HttpPost]
        [AuthorizeCompany(CompanyType.Buyer, CompanyType.BuyerAndSupplier)]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<JsonResult> RemoveFavoriteFuel(int favoriteId)
        {
            var response = await ContextFactory.Current.GetDomain<SettingsDomain>().RemoveFavoriteFuelAsync(CurrentUser.CompanyId, CurrentUser.Id, favoriteId);
            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Buyer, CompanyType.BuyerAndSupplier)]
        public async Task<JsonResult> GetFavoriteFuelGrid()
        {
            var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetFavoriteFuelListAsync(CurrentUser.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Buyer, CompanyType.BuyerAndSupplier)]
        public async Task<JsonResult> GetFavoriteFuelHistory()
        {
            var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetFavoriteFuelHistoryAsync(CurrentUser.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private async Task CheckForExistingFiles(HttpPostedFileBase[] files)
        {
            using (var tracer = new Tracer("ProfileController", "CheckForExistingFiles"))
            {
                List<string> documents = new List<string>();
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        var inputFileName = Path.GetFileName(file.FileName);
                        documents.Add(inputFileName);
                    }
                }
                if (documents.Count > 0)
                {
                    List<string> matchingFiles = await ContextFactory.Current.GetDomain<SettingsDomain>().GetFilePathDetailsAsync(documents, CurrentUser.CompanyId);
                    if (matchingFiles.Count > 0)
                    {
                        foreach (string path in matchingFiles)
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                    }
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> LocationsBulkUpload(int companyId, HttpPostedFileBase csvFile)
        {
            using (var tracer = new Tracer("ProfileController", "LocationsBulkUpload"))
            {
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (csvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            var csvFilePath = Server.MapPath(Resource.locationsBulkUploadFilePath);

                            var settingsDomain = ContextFactory.Current.GetDomain<SettingsDomain>();
                            var response = settingsDomain.ValidateLocationBulkUploadCsv(csvText, csvFilePath);
                            if (response.StatusCode == Status.Success)
                            {
                                response = await settingsDomain.SaveBulkLocationsAsync(csvText.Trim(), CurrentUser.Id, companyId);
                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                                if (response.StatusCode != Status.Failed)
                                {
                                    return RedirectToAction("CompanyDetails", "SuperAdmin", new { area = "SuperAdmin", Id = companyId });
                                }
                            }
                            else
                            {
                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                            }
                        }
                        else
                        {
                            DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                        }
                    }
                    else
                    {
                        DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                    }
                }
                else
                {
                    DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
                }

                return RedirectToAction("CompanyDetails", "SuperAdmin", new { area = "SuperAdmin", Id = companyId });
            }
        }

        [HttpGet]
        public ActionResult BillingAddress()
        {
            using (var tracer = new Tracer("ProfileController", "BillingAddress"))
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetBillingAddressTab(int companyId)
        {
            using (var tracer = new Tracer("ProfileController", "GetBillingAddressTab"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanyAddressAsync(companyId);
                return PartialView("~/Areas/Settings/Views/Profile/BillingAddress.cshtml", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin, UserRoles.AccountingPerson)]
        public async Task<ActionResult> BillingAddress(CompanyAddressViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "BillingAddress"))
            {               
                if (ModelState.IsValid)
                {
                    viewModel.BillingAddress.UpdatedBy = CurrentUser.Id;
                    viewModel.BillingAddress.CompanyId = CurrentUser.CompanyId;
                    var response = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveBillingAddressAsync(viewModel);

                    viewModel.BillingAddress.StatusCode = response.StatusCode;
                    viewModel.BillingAddress.StatusMessage = response.StatusMessage;

                    DisplayCustomMessages((MessageType)viewModel.BillingAddress.StatusCode, viewModel.BillingAddress.StatusMessage);
                }
                return View("CompanyInformation");
            }
        }


        public ActionResult QuickBooksAccount()
        {
            var domain = new QbDomain();
            var profile = domain.GetQbCompanyProfile(CurrentUser.CompanyId);
            return View(profile);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin, UserRoles.AccountingPerson)]
        public JsonResult RequestPaymentTermsFromQuickbooks()
        {
            var qbDomain = new QbDomain();
            qbDomain.CreateNewWorkflow(
                new AccountingWorkflowViewModel()
                {
                    QbCompanyProfileId = CurrentUser.CompanyId,
                    Type = AccountingWorkflowType.PaymentTerms,
                    ParameterJson = JsonConvert.SerializeObject(new RequestParameters { CompanyId = CurrentUser.CompanyId }),
                    Status = AccountingWorkflowStatus.Created,
                    SoftwareVersion = "13.0",
                    CreatedOn = DateTimeOffset.Now,
                    UpdatedOn = DateTimeOffset.Now
                });
            return Json(new { message = "Request has been queued to retrieve latest Payment terms from Quickbooks. Make sure your Quickbooks is running. Reload this page after few minutes" }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        //[AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.AccountingPerson)]
        public ActionResult ExternalIdentity()
        {
            return View();
        }

        [HttpGet]
        //[AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.AccountingPerson)]
        public async Task<JsonResult> GetExternalCompanyIds()
        {
            var settingsDomain = new SettingsDomain();
            var response = await settingsDomain.GetExternalCompanyIds(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.AccountingPerson)]
        public async Task<JsonResult> SaveExternalId(CompanyExternalIdViewModel viewModel)
        {
            var settingsDomain = new SettingsDomain();
            var response = await settingsDomain.SaveExternalCompanyId(UserContext, viewModel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CompanySubGroup()
        {
            CompanySubGroupViewModel model;
            using (var tracer = new Tracer("ProfileController", "CompanySubGroup"))
            {
                model = new CompanySubGroupViewModel()
                {
                    OwnerCompanyId = CurrentUser.CompanyId,
                    Companies = Helpers.CommonHelperMethods.GetChildCompaniesByCompany(CurrentUser.CompanyId).Companies,
                    CompanyGroup = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetGroupDetails(CurrentUser.CompanyId)
                };

                return View(model);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> GetCompanySubGroupGridDetails()
        {
            using (var tracer = new Tracer("ProfileController", "GetCompanySubGroupGridDetails"))
            {
                var model = await ContextFactory.Current.GetDomain<SettingsDomain>().GetCompanySubGroupGridDetails(CurrentUser.CompanyId);
                return new JsonResult
                {
                    Data = model,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CompanySubGroup(CompanySubGroupViewModel viewModel)
        {
            StatusViewModel model;
            using (var tracer = new Tracer("ProfileController", "CompanySubGroup"))
            {
                if (ModelState.IsValid)
                {
                    if (viewModel.Id > 0)
                        model = await ContextFactory.Current.GetDomain<SettingsDomain>().EditCompanySubGroup(viewModel, UserContext);
                    else
                        model = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveCompanySubGroup(viewModel, UserContext);

                    DisplayCustomMessages((MessageType)model.StatusCode, model.StatusMessage);
                }

                return RedirectToAction("CompanySubGroup");
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<JsonResult> GetSubGroupById(int id)
        {
            using (var tracer = new Tracer("ProfileController", "GetSubGroupById"))
            {
                var model = await ContextFactory.Current.GetDomain<SettingsDomain>().GetSubGroupById(id);
                return new JsonResult
                {
                    Data = model,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<JsonResult> DeleteCompanySubGroup(int subGroupId)
        {
            using (var tracer = new Tracer("ProfileController", "DeleteCompanySubGroup"))
            {
                var model = await ContextFactory.Current.GetDomain<SettingsDomain>().DeleteCompanySubGroup(subGroupId);
                return new JsonResult
                {
                    Data = model,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin)]
        public ActionResult CompanyFeatures()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> GetFeatures()
        {
            using (var tracer = new Tracer("ProfileController", "GetFeatures"))
            {
                var response = await ContextFactory.Current.GetDomain<CompanyDomain>().GetFeaturesAsync(CurrentUser.CompanyId, CurrentUser.CompanyTypeId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult UpdateFeatureSetting(int id, bool isFeatureEnable)
        {
            StatusViewModel response;
            using (var tracer = new Tracer("ProfileController", "UpdateBuyerAuditStatus"))
            {
                if (CurrentUser.IsSupplierAdmin || CurrentUser.IsBuyerAdmin || CurrentUser.IsCarrierAdmin)
                {
                    response = ContextFactory.Current.GetDomain<CompanyDomain>().UpdateFeatureSettings(UserContext, id, isFeatureEnable);
                }
                else
                {
                    response = new StatusViewModel()
                    {
                        StatusCode = Status.Failed,
                        MessageCode = 401,
                        StatusMessage = "You don't have permission to modify these settings."
                    };
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetTerminalCardNumber(string prefix = "")
        {
            var model = new TerminalCardNumberViewModel() { CollectionHtmlPrefix = prefix };
            return PartialView("_PartialTerminalCardNumber", model);
        }

        public ActionResult CarrierCompanies()
        {
            return View();
        }

        public async Task<JsonResult> GetCarriers()
        {
            var response = await ContextFactory.Current.GetDomain<CompanyDomain>().GetCarriers(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetJobsForSupplierToCarrier()
        {
            var response = await ContextFactory.Current.GetDomain<CompanyDomain>().GetJobsForSupplier(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetAssignedCarriersForSupplier(int carrierCompanyId = 0)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.GetAssignedCarriersForSupplier(UserContext.CompanyId, carrierCompanyId);
            response = ContextFactory.Current.GetDomain<CarrierDomain>().GetCarrierUserEmails(response);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetAssignedCarriers()
        {
            var carrierDomain = ContextFactory.Current.GetDomain<CarrierDomain>();
            var response = await carrierDomain.GetAssignedCarrierUsers(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AssignCarriers(List<SupplierCarrierViewModel> carriers)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            carriers.ForEach(t => { t.CreatedBy = UserContext.Id; t.SupplierCompanyId = UserContext.CompanyId; t.SupplierCompanyName = UserContext.CompanyName; });
            var response = await fsDomain.AssignCarriers(carriers);
            if (response.StatusCode == Status.Success)
            {
                var JCDresponse = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().SaveJobCarrierDetailJob(carriers, UserContext.Id, UserContext.CompanyId);

            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> UpdateAssignedCarrier(SupplierCarrierViewModel viewModel)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            viewModel.CreatedBy = UserContext.Id;
            viewModel.SupplierCompanyId = UserContext.CompanyId;
            viewModel.SupplierCompanyName = UserContext.CompanyName;
            var response = await fsDomain.UpdateAssignedCarrier(viewModel);
            if (response.StatusMessage.Equals("Success"))
            {
                response = await ContextFactory.Current.GetDomain<CarrierDomain>().UpdateJobCarrierDetails(viewModel, UserContext.Id, UserContext.CompanyId);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DeleteAssignedCarrier(SupplierCarrierViewModel viewModel)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            viewModel.CreatedBy = UserContext.Id;
            viewModel.SupplierCompanyId = UserContext.CompanyId;
            viewModel.SupplierCompanyName = UserContext.CompanyName;
            var response = await fsDomain.DeleteAssignedCarrier(viewModel);
            if (response.StatusMessage.Equals("Success"))
            {
                response = await ContextFactory.Current.GetDomain<CarrierDomain>().DeleteJobCarrierDetails(viewModel);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PreferencesSetting()
        {
            using (var tracer = new Tracer("ProfileController", "PreferencesSetting"))
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetPreferencesSetting(int id = 0)
        {
            using (var tracer = new Tracer("ProfileController", "GetPreferencesSetting"))
            {
                var appDomain = new ApplicationDomain();
                ViewBag.SiteFuelExchangeUrl = appDomain.GetKeySettingValue("SupplierLoginURL", "");
                ImageViewModel imageViewModel = new ImageViewModel();
                var response = await ContextFactory.Current.GetDomain<CompanyDomain>().GetPreferencesSettingAsync(id, UserContext);
                if (!string.IsNullOrEmpty(response.ImageFilePath))
                {
                    imageViewModel.FilePath = response.ImageFilePath;
                    response.ImageFilePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                }
                if (!string.IsNullOrEmpty(response.BackgroundImageFilePath))
                {
                    imageViewModel.FilePath = response.BackgroundImageFilePath;
                    response.BackgroundImageFilePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                }
                if (!string.IsNullOrEmpty(response.FaviconFilePath))
                {
                    imageViewModel.FilePath = response.FaviconFilePath;
                    response.FaviconFilePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                }
                if (!string.IsNullOrEmpty(response.CarrierOnboardingImageFilePath))
                {
                    imageViewModel.FilePath = response.CarrierOnboardingImageFilePath;
                    response.CarrierOnboardingImageFilePath = imageViewModel.GetAzureFilePath(BlobContainerType.BrandWebsite);
                }
                if (response != null)
                    response.ForcastingPreference = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Account, response.Id);
                if (response.ForcastingPreference.ForcastingServiceSetting != null)
                {
                    response.ForcastingPreference.ForcastingServiceSetting.IsOttoAutoDRCreationDisplay = true;
                    await IntializeForcastingCarrierList(response);
                }
                return PartialView("~/Views/Shared/_PartialPreferences.cshtml", response);
            }
        }

        private async Task IntializeForcastingCarrierList(OnboardingPreferenceViewModel response)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var carrierresponse = await fsDomain.GetAssignedCarriersForSupplier(UserContext.CompanyId, 0);
            carrierresponse = ContextFactory.Current.GetDomain<CarrierDomain>().GetCarrierUserEmails(carrierresponse);
            response.ForcastingPreference.ForcastingServiceSetting.CarrierList = carrierresponse.Select(top => new DropdownDisplayItem { Id = top.Carrier.Id, Name = top.Carrier.Name }).ToList();
        }

        [HttpGet]
        public async Task<JsonResult> GetPreferencesSettingForBranding(int id = 0)
        {
            using (var tracer = new Tracer("ProfileController", "GetPreferencesSetting"))
            {
                var response = new OnboardingPreferenceViewModel();
                response = await ContextFactory.Current.GetDomain<CompanyDomain>().GetPreferencesSettingAsync(id, UserContext, CurrentUser.BrandedCompanyId);
                if (response == null)
                {
                    var isInvited = await ContextFactory.Current.GetDomain<CompanyDomain>().IsUserInvitedBySupplier(UserContext.Id);
                    if (isInvited.StatusCode == Status.Success)
                    {
                        response = isInvited;
                    }
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetFeatureSetting()
        {
            using (var tracer = new Tracer("ProfileController", "GetFeatureSetting"))
            {
                var response = await ContextFactory.Current.GetDomain<CompanyDomain>().GetFeaturesAsync(CurrentUser.CompanyId, CurrentUser.CompanyTypeId);
                return PartialView("~/Views/Shared/_PartialFeatureSetting.cshtml", response);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin)]
        public async Task<ActionResult> SavePreferencesSetting(OnboardingPreferenceViewModel model, HttpPostedFileBase brandWebsiteimageFile, HttpPostedFileBase brandBackgroundImageFile, HttpPostedFileBase selfpoCsvFile, HttpPostedFileBase wholesalebadgeCsvFile, HttpPostedFileBase carrierListCsvFile, HttpPostedFileBase quebecBadgeListCsvFile, HttpPostedFileBase BrandWebsitefaviconFile, HttpPostedFileBase brandWebsiteCarrierOnboardingImageFile)
        {
            using (var tracer = new Tracer("ProfileController", "SavePreferencesSetting"))
            {
                if (!string.IsNullOrEmpty(model.ProductSequencingJson))
                {
                    model.ProductSequencing.SequenceType = ProductSequenceType.Product;
                    model.ProductSequencing.ProductSequence = JsonConvert.DeserializeObject<List<ProductSequenceModel>>(model.ProductSequencingJson);
                }
                var poFileUploadStatus = new StatusViewModel(Status.Failed);
                var wholesalebadgeFileUploadStatus = new StatusViewModel(Status.Failed);
                var carrierListFileUploadStatus = new StatusViewModel(Status.Failed);
                var quebecBadgeFileUploadStatus = new StatusViewModel(Status.Failed);
                if (brandWebsiteimageFile != null)
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    if (!model.IsRemoved)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            brandWebsiteimageFile.InputStream.CopyTo(ms);
                            imageViewModel.Data = ms.GetBuffer();
                        }
                        await imageViewModel.UploadImageToAzureBlobService(ApplicationConstants.CompanyLogoFileNamePrefix, BlobContainerType.BrandWebsite);
                        model.ImageFilePath = imageViewModel.FilePath;
                    }
                }
                if (brandBackgroundImageFile != null)
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    if (!model.IsRemoved)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            brandBackgroundImageFile.InputStream.CopyTo(ms);
                            imageViewModel.Data = ms.GetBuffer();
                        }

                        await imageViewModel.UploadImageToAzureBlobService(ApplicationConstants.CompanyBackgroundFileNamePrefix, BlobContainerType.BrandWebsite);
                        model.BackgroundImageFilePath = imageViewModel.FilePath;
                    }
                }
                if (BrandWebsitefaviconFile != null)
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    if (!model.IsRemoved)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            BrandWebsitefaviconFile.InputStream.CopyTo(ms);
                            imageViewModel.Data = ms.GetBuffer();
                        }
                        await imageViewModel.UploadImageToAzureBlobService(ApplicationConstants.CompanyFaviconNamePrefix, BlobContainerType.BrandWebsite);
                        model.FaviconFilePath = imageViewModel.FilePath;
                    }
                }
                if (brandWebsiteCarrierOnboardingImageFile != null)
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    if (!model.IsRemoved)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            brandWebsiteCarrierOnboardingImageFile.InputStream.CopyTo(ms);
                            imageViewModel.Data = ms.GetBuffer();
                        }
                        await imageViewModel.UploadImageToAzureBlobService(ApplicationConstants.CarrierOnboardingNamePrefix, BlobContainerType.BrandWebsite);
                        model.CarrierOnboardingImageFilePath = imageViewModel.FilePath;
                    }
                }
                model.ImageFilePath = brandWebsiteimageFile != null ? model.ImageFilePath : model.hdnImageFilePath;
                model.BackgroundImageFilePath = brandBackgroundImageFile != null ? model.BackgroundImageFilePath : model.hdnBackgroundImageFilePath;
                model.FaviconFilePath = BrandWebsitefaviconFile != null ? model.FaviconFilePath : model.hdnfaviconFilePath;
                model.CarrierOnboardingImageFilePath = brandWebsiteCarrierOnboardingImageFile != null ? model.CarrierOnboardingImageFilePath : model.hdnCarrierOnboardingImageFilePath;
                // clear image cache for enable/disable bw or update image
                ClearBrandImageCache(CurrentUser.CompanyId, model.URLName);

                if (model.IsLiftFileValidationEnabled && ((selfpoCsvFile != null && selfpoCsvFile.ContentLength > 0)
                    || (wholesalebadgeCsvFile != null && wholesalebadgeCsvFile.ContentLength > 0) || (carrierListCsvFile != null && carrierListCsvFile.ContentLength > 0)
                    || (quebecBadgeListCsvFile != null && quebecBadgeListCsvFile.ContentLength > 0)))
                {
                    var isPOListFound = false;
                    var isBadgeListFound = false;
                    var isCarrierListFound = false;
                    var isquebecBadgeListFound = false;
                    //self haul po upload
                    if (model.LfvInputParameter.IsIgnoreSelfHauling && selfpoCsvFile != null && selfpoCsvFile.ContentLength > 0)
                    {
                        isPOListFound = true;
                        if (selfpoCsvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                        {

                            if (Path.GetExtension(selfpoCsvFile.FileName).ToLower() == ".csv")
                            {

                                var domain = ContextFactory.Current.GetDomain<PoNumberBulkUploadDomain>();
                                poFileUploadStatus = await domain.ProcessUploadedPoFile(selfpoCsvFile, UserContext);
                                // DisplayCustomMessages((MessageType)poFileUploadStatus.StatusCode, poFileUploadStatus.StatusMessage);
                            }
                            else
                            {
                                DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                            }
                        }
                        else
                        {
                            DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                        }
                    }
                    //wholesale badge upload
                    if (model.LfvInputParameter.IsIgnoreWholesaleBadge && wholesalebadgeCsvFile != null && wholesalebadgeCsvFile.ContentLength > 0)
                    {
                        isBadgeListFound = true;
                        if (wholesalebadgeCsvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                        {
                            if (Path.GetExtension(wholesalebadgeCsvFile.FileName).ToLower() == ".csv")
                            {
                                var domain = ContextFactory.Current.GetDomain<LFVDomain>();
                                wholesalebadgeFileUploadStatus = await domain.ProcessUploadedWholeSaleBadgeFile(wholesalebadgeCsvFile, UserContext);

                                //  DisplayCustomMessages((MessageType)wholesalebadgeFileUploadStatus.StatusCode, poFileUploadStatus.StatusMessage);
                            }
                            else
                            {
                                DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                            }
                        }
                        else
                        {
                            DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                        }
                    }
                    //carrier list upload
                    if (model.LfvInputParameter.IsIgnoreNonRegisteredCarriers && carrierListCsvFile != null && carrierListCsvFile.ContentLength > 0)
                    {
                        isCarrierListFound = true;
                        if (carrierListCsvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                        {
                            if (Path.GetExtension(carrierListCsvFile.FileName).ToLower() == ".csv")
                            {
                                var domain = ContextFactory.Current.GetDomain<LFVDomain>();
                                carrierListFileUploadStatus = await domain.ProcessUploadedCarrierListFile(carrierListCsvFile, UserContext);
                            }
                            else
                            {
                                DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                            }
                        }
                        else
                        {
                            DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                        }
                    }
                    // Quebec badge list upload
                    if (model.LfvInputParameter.IsIgnoreQuebecBillingBadges && quebecBadgeListCsvFile != null && quebecBadgeListCsvFile.ContentLength > 0)
                    {
                        isquebecBadgeListFound = true;
                        if (quebecBadgeListCsvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                        {
                            if (Path.GetExtension(quebecBadgeListCsvFile.FileName).ToLower() == ".csv")
                            {

                                var domain = ContextFactory.Current.GetDomain<LFVDomain>();
                                quebecBadgeFileUploadStatus = await domain.ProcessUploadedQuebecBadgeListFile(quebecBadgeListCsvFile, UserContext);
                            }
                            else
                            {
                                DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                            }
                        }
                        else
                        {
                            DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                        }
                    }
                    if (isPOListFound || isBadgeListFound || isCarrierListFound || isquebecBadgeListFound)
                    {
                        var isPOUploadSuccess = false;
                        var isBadgeFileUploadSuccess = false;
                        var isCarrierFileUploadSuccess = false;
                        var isQuebecBadgeFileUploadSuccess = false;
                        if (isPOListFound)
                        {
                            isPOUploadSuccess = poFileUploadStatus.StatusCode == Status.Success ? true : false;
                        }
                        if (isBadgeListFound)
                        {
                            isBadgeFileUploadSuccess = wholesalebadgeFileUploadStatus.StatusCode == Status.Success ? true : false;
                        }
                        if (isCarrierListFound)
                        {
                            isCarrierFileUploadSuccess = carrierListFileUploadStatus.StatusCode == Status.Success ? true : false;
                        }
                        if (isquebecBadgeListFound)
                        {
                            isQuebecBadgeFileUploadSuccess = quebecBadgeFileUploadStatus.StatusCode == Status.Success ? true : false;
                        }
                        var csvProcessingResponseParameters = new
                        {
                            IsPOListFound = isPOListFound,
                            IsBadgeListFound = isBadgeListFound,
                            IsBadgeFileUploadSucess = isBadgeFileUploadSuccess,
                            IsPOFileUploadSucess = isPOUploadSuccess,
                            IsCarrierListFound = isCarrierListFound,
                            IsCarrierFileUploadSucess = isCarrierFileUploadSuccess,
                            IsQuebecBadgeListFound = isquebecBadgeListFound,
                            IsQuebecBadgeFileUploadSucess = isQuebecBadgeFileUploadSuccess
                        };
                        if (IsSavePreferenceSettings(csvProcessingResponseParameters))
                        {
                            var response = await ContextFactory.Current.GetDomain<CompanyDomain>().SavePreferencesSetting(model, UserContext);
                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                            //Save the forcasting setting details.
                            await Saveforcastingdetails(model, response);
                        }
                        else
                        {
                            if (!isBadgeFileUploadSuccess && isBadgeListFound)
                            {
                                DisplayCustomMessages((MessageType)wholesalebadgeFileUploadStatus.StatusCode, wholesalebadgeFileUploadStatus.StatusMessage);
                            }
                            if (!isPOUploadSuccess && isPOListFound)
                            {
                                DisplayCustomMessages((MessageType)poFileUploadStatus.StatusCode, poFileUploadStatus.StatusMessage);
                            }
                            if (!isCarrierFileUploadSuccess && isCarrierListFound)
                            {
                                DisplayCustomMessages((MessageType)carrierListFileUploadStatus.StatusCode, carrierListFileUploadStatus.StatusMessage);
                            }
                        }
                    }
                }
                else
                {
                    var response = await ContextFactory.Current.GetDomain<CompanyDomain>().SavePreferencesSetting(model, UserContext);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    //Save the forcasting setting details.
                    await Saveforcastingdetails(model, response);
                }
                return RedirectToAction("PreferencesSetting", "Profile", new { area = "Settings" });
            }
        }
        public void ClearBrandImageCache(int companyId, string urlName)
        {
            var cacheFKey = $"{ApplicationConstants.CompanyFaviconCacheName}_{companyId}";
            CacheManager.Remove(cacheFKey);
            var cacheLKey = $"{ApplicationConstants.CompanyLogoCacheName}_{companyId}";
            CacheManager.Remove(cacheLKey);
            var cacheFUKey = $"{ApplicationConstants.CompanyFaviconCacheName}_{urlName}";
            CacheManager.Remove(cacheFUKey);
            var cacheLUKey = $"{ApplicationConstants.CompanyLogoCacheName}_{urlName}";
            CacheManager.Remove(cacheLUKey);
            var cacheCKey = $"{ApplicationConstants.CarrierOnboardingCacheName}_{companyId}";
            CacheManager.Remove(cacheFKey);
            var cacheCUKey = $"{ApplicationConstants.CarrierOnboardingCacheName}_{urlName}";
            CacheManager.Remove(cacheFUKey);
        }
        public bool IsSavePreferenceSettings(dynamic csvProcessingResponseParameters)
        {
            bool shouldSave = true;
            if (csvProcessingResponseParameters != null)
            {
                if (csvProcessingResponseParameters.IsPOListFound)
                {
                    if (!csvProcessingResponseParameters.IsPOFileUploadSucess)
                    {
                        shouldSave = false;
                    }
                }
                if (csvProcessingResponseParameters.IsBadgeListFound)
                {
                    if (!csvProcessingResponseParameters.IsBadgeFileUploadSucess)
                    {
                        shouldSave = false;
                    }
                }
                if (csvProcessingResponseParameters.IsCarrierListFound)
                {
                    if (!csvProcessingResponseParameters.IsCarrierFileUploadSucess)
                    {
                        shouldSave = false;
                    }
                }
                if (csvProcessingResponseParameters.IsQuebecBadgeListFound)
                {
                    if (!csvProcessingResponseParameters.IsQuebecBadgeFileUploadSucess)
                    {
                        shouldSave = false;
                    }
                }
            }
            return shouldSave;
        }

        private void UpdateClaimValue(string key, string value)
        {
            var Identity = new ClaimsIdentity(User.Identity);
            Identity.RemoveClaim(Identity.FindFirst(key));
            Identity.AddClaim(new Claim(key, value));
            var rememberMe = Identity.FindFirst(ApplicationSecurityConstants.RememberMe);
            AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                                                                    new ClaimsPrincipal(Identity),
                                                                    new AuthenticationProperties
                                                                    {
                                                                        IsPersistent = rememberMe != null ? Convert.ToBoolean(rememberMe.Value) : true
                                                                    });
        }
        public ActionResult DriverManagement()
        {
            using (var tracer = new Tracer("ProfileController", "DriverManagement"))
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetShifts()
        {
            using (var tracer = new Tracer("ProfileController", "GetShifts"))
            {
                var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                var response = await fsDomain.GetCompanyShiftDdl(UserContext.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAllDrivers()
        {
            using (var tracer = new Tracer("ProfileController", "GetAllDrivers"))
            {
                var domain = ContextFactory.Current.GetDomain<SettingsDomain>();
                var response = await domain.GetAllDrivers(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddDriver(DriverObjectModel driverModel)
        {
            AdditionalUsersViewModel viewModel = new AdditionalUsersViewModel();
            using (var tracer = new Tracer("ProfileController", "AddDriver"))
            {
                if (ModelState.IsValid)
                {
                    viewModel = ContextFactory.Current.GetDomain<SettingsDomain>().InitialiseDriverModel(driverModel, UserContext);
                    // update driver
                    if (driverModel.DriverId > 0)
                    {
                        var response = await ContextFactory.Current.GetDomain<SettingsDomain>().EditAdditionalUsersAsync(viewModel);
                        viewModel.StatusCode = response.StatusCode;
                        viewModel.StatusMessage = response.StatusMessage;
                    }
                    else
                    {
                        var duplicate = viewModel.AdditionalUsers
                                                    .GroupBy(x => x.Email)
                                                    .Where(group => group.Count() > 1)
                                                    .Select(group => group.Key).ToList();

                        if (duplicate != null && duplicate.Count > 0)
                        {
                            string duplicateEmails = string.Join(", ", duplicate);
                            viewModel.StatusCode = Status.Failed;
                            viewModel.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageDuplicateEntries,
                                new[] { duplicate.Count == 1 ? $"{duplicateEmails} is" : $"{duplicateEmails} are" });
                        }
                        else
                        {
                            viewModel.AdditionalUsers.ForEach(t => t.CompanyId = CurrentUser.CompanyId);
                            var response = await ContextFactory.Current.GetDomain<SettingsDomain>().CreateAdditionalUsersAsync(viewModel);

                            viewModel.StatusCode = response.StatusCode;
                            viewModel.StatusMessage = response.StatusMessages.FirstOrDefault();
                        }
                    }
                }

                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }
        }
        private async Task Saveforcastingdetails(OnboardingPreferenceViewModel viewModel, StatusViewModel response)
        {
            var forcastingResponse = await SaveforcastingSettingAccountLevel(viewModel.ForcastingPreference, response, viewModel.Id);
            if (forcastingResponse.StatusCode == Status.Failed)
            {
                DisplayCustomMessages((MessageType)forcastingResponse.StatusCode, forcastingResponse.StatusMessage);
            }
        }
        private async Task<StatusViewModel> SaveforcastingSettingAccountLevel(ForcastingPreferenceViewModel viewModel, StatusViewModel response, int preventityId)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            statusViewModel.StatusCode = Status.Success;
            if (viewModel != null && viewModel.ForcastingServiceSetting != null && response.StatusCode == (int)Status.Success)
            {
                statusViewModel = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().SaveForeCastingPreferanceSetting(viewModel, UserContext, (int)ForcastingSettingLevel.Account, response.EntityId, preventityId);
            }
            else
            {
                statusViewModel.StatusMessage = response.StatusMessage;
            }
            return statusViewModel;
        }

        [HttpPost]
        public async Task<JsonResult> BulkUploadCarrier()
        {
            var response = new StatusViewModel();
            try
            {
                var file = Request.Files;
                var IsCreateFreightOrder = Request.Form["IsCreateFreightOrder"];
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var csvFile = file[0];
                    if (csvFile != null && csvFile.ContentLength > 0)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            List<SupplierCarrierViewModel> supplierCarrierViewModels = new List<SupplierCarrierViewModel>();
                            var carrierDomain = ContextFactory.Current.GetDomain<CarrierDomain>();
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            var saveCarrierResponse = await carrierDomain.SaveCarrierAssignmentFileAsync(UserContext, csvText, supplierCarrierViewModels);
                            supplierCarrierViewModels = saveCarrierResponse.Item1;
                            response = saveCarrierResponse.Item2;
                            if (supplierCarrierViewModels != null && supplierCarrierViewModels.Any() && IsCreateFreightOrder == "1")
                            {
                                response = await ContextFactory.Current.GetDomain<CarrierDomain>().createFreightOrdersForAssignedCarrier(supplierCarrierViewModels, UserContext);
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "BulkUploadCarrier", ex.Message, ex);
                response.StatusCode = Status.Failed;
            }
            if (response.StatusCode == Status.Success)
            {
                response.StatusMessage = Resource.successOrdersAssignedToCarrier;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> ExternalCompanyInvitedUsers(int id)
        {
            using (var tracer = new Tracer("ProfileController", "CompanyAdditionalUsers"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetInvitedCompanyUsersAsync(id, UserContext);
                response.UserId = CurrentUser.Id;

                response.DisplayMode = PageDisplayMode.Create;
                if (response.StatusMessage == Resource.errMessageUserAlreadyExistsInTFX)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("CompanyUsers", "Profile", new { area = "Settings" });
                }
                return View(response);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddExternalCompanyUsers(AdditionalUsersViewModel viewModel)
        {
            using (var tracer = new Tracer("ProfileController", "AddCompanyUsers"))
            {
                        viewModel.CompanyId = CurrentUser.CompanyId;
                        viewModel.SupplierURL = CurrentUser.SupplierURL;
                        viewModel.ApplicationTemplateId = CurrentUser.ApplicationTemplateId;
                        var response = await ContextFactory.Current.GetDomain<SettingsDomain>().CreateExternalAdditionalUsersAsync(viewModel);

                        viewModel.StatusCode = response.StatusCode;
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessages.ToList());


                if (viewModel.StatusCode == Status.Failed)
                {
                    viewModel.DisplayMode = PageDisplayMode.Create;
                    return View("ExternalCompanyInvitedUsers", viewModel);
                }

                return RedirectToAction("CompanyUsers", "Profile", new { area = "Settings" });
            }
        }
        [HttpGet]
        public async Task<ActionResult> ExternalCompanyInvitedUsersGrid()
        {
            using (var tracer = new Tracer("ProfileController", "CompanyInvitedUsersGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().GetExternalInvitedUserListAsync(CurrentUser.CompanyId, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddFuelAssetInformation(FleetTrailers model)
        {
            var response = await ContextFactory.Current.GetDomain<SettingsDomain>().AddFuelAssetInformation(model, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> AddDefAssetInformation(FleetTrailers defAssets)
        {
            var response = await ContextFactory.Current.GetDomain<SettingsDomain>().AddDefAssetInformation(defAssets, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetAdditiveProductListGridView()
        {
            return PartialView("_PartialAdditiveProductsGrid");
        }

        /// <summary>
        /// Edit or Detele Additive products
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SaveAdditiveProduct(AdditiveProductDetailsViewModel product)
        {
            var response = await ContextFactory.Current.GetDomain<ProductDomain>().SaveAdditiveBlendingProduct(product, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

       [HttpGet]

       public async Task<ActionResult> GetAdditiveProductsGrid()
        {
            List<AdditiveProductDetailsViewModel> produtcs = new List<AdditiveProductDetailsViewModel>();

            produtcs = await ContextFactory.Current.GetDomain<SettingsDomain>().GetAdditiveProductsListForCompany(UserContext);
            return Json(produtcs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteAdditiveProduct(int id)
        {
            var request = new AdditiveProductDetailsViewModel() { IsDeleted = true, Id = id };
            var response = await ContextFactory.Current.GetDomain<ProductDomain>().SaveAdditiveBlendingProduct(request, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetReasonCategoryAndCodesView()
        {
            var model = new ReasonViewModel();
            model.ReasonCategory = new ReasonCategoryViewModel();
            model.ReasonCode = new ReasonCodeModel();
            return PartialView("_PartialReasonCategoryAndCodesPopup", model);
        }

        [HttpGet]
        public async Task<ActionResult> GetReasonCategories()
        {
            List<ReasonCategoryViewModel> categories = new List<ReasonCategoryViewModel>();

            categories = await ContextFactory.Current.GetDomain<SettingsDomain>().GetReasonCategories(UserContext.CompanyId);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetReasonCategoryListDDL()
        {
            List<DropdownDisplayExtendedItem> categories = new List<DropdownDisplayExtendedItem>();

            categories = await ContextFactory.Current.GetDomain<SettingsDomain>().GetReasonCategoryListDDL(UserContext.CompanyId);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveReasonCategory(ReasonCategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveReasonCategory(category, UserContext);
                category.StatusCode = response.StatusCode;
                category.StatusMessage = response.StatusMessage;
            }
            return Json(category, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteReasonCategory(int id)
        {
            var response = new StatusViewModel(Status.Failed);
            if (id <= 0)
            {
                response.StatusMessage = "Invalid request";
            }
            else
            {
                response = await ContextFactory.Current.GetDomain<SettingsDomain>().DeleteReasonCategory(id, UserContext);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetReasonCodes(int? categoryId = null)
        {
            List<ReasonCodeModel> reasonCodes = new List<ReasonCodeModel>();

            reasonCodes = await ContextFactory.Current.GetDomain<SettingsDomain>().GetReasonCodes(UserContext.CompanyId, categoryId);
            return Json(reasonCodes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveReasonCode(ReasonCodeModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await ContextFactory.Current.GetDomain<SettingsDomain>().SaveReasonCodeDescription(model, UserContext);
                model.StatusCode = response.StatusCode;
                model.StatusMessage = response.StatusMessage;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteReasonCode(int id)
        {
            var response = new StatusViewModel(Status.Failed);
            if (id <= 0)
            {
                response.StatusMessage = "Invalid request";
            }
            else
            {
                response = await ContextFactory.Current.GetDomain<SettingsDomain>().DeleteReasonCode(id, UserContext);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetPreferencesSettingAsync()
        {
            var response = await ContextFactory.Current.GetDomain<CompanyDomain>().GetPreferencesSettingAsync(0, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
} 