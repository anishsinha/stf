using Microsoft.Owin.Security;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
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
    public class TaxExemptionController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Create(int id = 0)
        {
            using (var tracer = new Tracer("TaxExemptionController", "Create"))
            {
                var response = await ContextFactory.Current.GetDomain<TaxExemtionLicenseDomain>().GetTaxExemptionInfo(CurrentUser.CompanyId, id);
                if (id == 0)
                {
                    response.LegalName = CurrentUser.CompanyName;
                }
                else
                {
                    response.IsAgreed = true;
                }
                return View("~/Areas/Settings/Views/Profile/TaxExemption.cshtml", response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(TaxExemptionViewModel viewModel)
        {
            using (var tracer = new Tracer("TaxExemptionController", "Create(viewModel)"))
            {
                viewModel.UserId = CurrentUser.Id;
                viewModel.CompanyId = CurrentUser.CompanyId;
                viewModel.BusinessType = CurrentUser.IsBuyerCompany ? (int)CompanyType.Buyer : (int)CompanyType.Supplier;
                TaxExemptionViewModel response;
                if (viewModel.Id > 0)
                {
                    response = await ContextFactory.Current.GetDomain<TaxExemtionLicenseDomain>().UpdateTaxExemptionLicense(viewModel);
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<TaxExemtionLicenseDomain>().SaveTaxExemptionLicense(viewModel);
                }
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                if (response.StatusCode == Status.Success)
                {
                    return RedirectToAction("CompanyTaxes", "TaxExemption", new { area = "Settings" });
                }
                response.IDCode = viewModel.IDCode;
                response.IsSameCompanyAddress = viewModel.IsSameCompanyAddress;
                return View("~/Areas/Settings/Views/Profile/TaxExemption.cshtml", response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCompanyTaxExclusion()
        {
            var settingDomain = ContextFactory.Current.GetDomain<SettingsDomain>();
            var response = await settingDomain.GetTaxExclusionAsync(UserContext.CompanyId);
            return PartialView("~/Areas/Settings/Views/Shared/_PartialTaxExclusion.cshtml", response);
        }


        [HttpGet]
        public async Task<ActionResult> CompanyTaxes()
        {
            var taxDomain = ContextFactory.Current.GetDomain<TaxExemtionLicenseDomain>();
            var response = await taxDomain.InitializeCompanyTax(CurrentUser.CompanyId, CurrentUser.CompanyTypeId, CurrentUser.CompanySubTypeId);
            return View("~/Areas/Settings/Views/Profile/CompanyTaxes.cshtml", response);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin)]
        public async Task<JsonResult> UpdateExclusion(bool IsNoraExclusion)
        {
            var settingDomain = ContextFactory.Current.GetDomain<SettingsDomain>();
            var response = await settingDomain.UpdateTaxExclusionAsync(UserContext, IsNoraExclusion);
            return Json(response.StatusCode, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> TaxExemptLicensesGrid()
        {
            var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetTaxExemptionLicenses(CurrentUser.CompanyId, true);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<JsonResult> DeleteLicenses(List<int> licenses)
        {
            var response = await ContextFactory.Current.GetDomain<TaxExemtionLicenseDomain>().DeleteLicensesAsync(CurrentUser.Id, licenses);
            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            return Json(response.StatusCode, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> SaveDirectTax(CompanyTaxesViewModel directTaxModel)
        {
            var taxDomain = ContextFactory.Current.GetDomain<TaxExemtionLicenseDomain>();
            var response = await taxDomain.SaveDirectTaxAsync(directTaxModel, CurrentUser.Id, CurrentUser.CompanyId);
            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            return RedirectToAction("CompanyTaxes", "TaxExemption", new { area = "Settings" });
        }
    }
}