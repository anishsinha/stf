using Microsoft.Owin.Security;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SiteFuel.Exchange.ViewModels.Forcasting;

namespace SiteFuel.Exchange.Web.Controllers
{
    [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin)]
    public class OnboardingController : BaseController
    {
        public async Task<ActionResult> Company(int Id = 0, string supplierURL = "")
        {
            using (var tracer = new Tracer("OnboardingController", "Company"))
            {
                var appDomain = new ApplicationDomain();
                var response = await ContextFactory.Current.GetDomain<OnboardingDomain>().GetOnboardingViewModelAsync(Id == 0 ? CurrentUser.Id : Id);
                response.User.Company.WorkPreference = WorkPreference.Calendar;
                ViewBag.SiteFuelExchangeUrl = appDomain.GetKeySettingValue("SupplierLoginURL", "");
                response.User.SupplierURL = supplierURL;
               
                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Company(OnboardingViewModel viewModel, HttpPostedFileBase imageFile, HttpPostedFileBase brandWebsiteimageFile, HttpPostedFileBase brandBackgroundImageFile)
        {
            using (var tracer = new Tracer("OnboardingController", "Company(viewModel)"))
            {
                if (viewModel.User.Company.CompanyTypeId != (int)CompanyType.Buyer)
                {
                    if (viewModel.CompanyAddress.ServiceOffering == null || !viewModel.CompanyAddress.ServiceOffering.Any(t => (t.AreaWide == ServiceAreaType.StateWide && t.StateIds.Any()) || (t.AreaWide == ServiceAreaType.ZipWide && t.ZipCodes.Any())))
                    {
                        DisplayCustomMessages((MessageType)Status.Failed, "Please add at least one of the service offering.");
                        return View(viewModel);
                    }
                }
                if (imageFile != null)
                {
                    if (!viewModel.User.Company.CompanyLogo.IsRemoved)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            imageFile.InputStream.CopyTo(ms);
                            viewModel.User.Company.CompanyLogo.Data = ms.GetBuffer();
                        }
                        await viewModel.User.Company.CompanyLogo.UploadImageToAzureBlobService(ApplicationConstants.CompanyLogoFileNamePrefix, BlobContainerType.CompanyProfile);
                    }
                }
                else if (viewModel.User.Company.CompanyLogo != null && viewModel.User.Company.CompanyLogo.IsRemoved)
                {
                    viewModel.User.Company.CompanyLogo.Id = 0;
                    viewModel.User.Company.CompanyLogo.Data = new byte[] { 0x20 };
                    viewModel.User.Company.CompanyLogo.FilePath = null;
                }
                if (brandWebsiteimageFile != null)
                {
                    ImageViewModel imageViewModel = new ImageViewModel();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        brandWebsiteimageFile.InputStream.CopyTo(ms);
                        imageViewModel.Data = ms.GetBuffer();
                    }
                    await imageViewModel.UploadImageToAzureBlobService(ApplicationConstants.CompanyLogoFileNamePrefix, BlobContainerType.BrandWebsite);
                    viewModel.PreferencesSetting.ImageFilePath = imageViewModel.FilePath;
                }
                if (brandBackgroundImageFile != null)
                {
                    var imageViewModel = new ImageViewModel();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        brandBackgroundImageFile.InputStream.CopyTo(ms);
                        imageViewModel.Data = ms.GetBuffer();
                    }
                    await imageViewModel.UploadImageToAzureBlobService(ApplicationConstants.CompanyBackgroundFileNamePrefix, BlobContainerType.BrandWebsite);
                    viewModel.PreferencesSetting.BackgroundImageFilePath = imageViewModel.FilePath;
                }
                if (ModelState.IsValid)
                {
                    viewModel.User.Company.UpdatedBy = CurrentUser.Id;
                    viewModel.CompanyAddress.UpdatedBy = CurrentUser.Id;

                    if (CurrentUser.IsAccountingPerson || CurrentUser.IsBuyerAdmin || CurrentUser.IsSupplierAdmin || CurrentUser.IsCarrierAdmin)
                    {
                        viewModel.BillingAddress.UpdatedBy = CurrentUser.Id;
                    }

                    var response = await ContextFactory.Current.GetDomain<OnboardingDomain>().SaveOnboardingViewModelAsync(viewModel, UserContext);
                    if (response.StatusCode == Status.Success)
                    {
                        //Save the forcasting setting details.
                        var forcastingResponse = new StatusViewModel();
                        if (viewModel.PreferencesSetting.ForcastingPreference.ForcastingServiceSetting.IsEnabled)
                        {
                            forcastingResponse = await Saveforcastingdetails(viewModel, response);
                        }

                        if ((viewModel.PreferencesSetting.ForcastingPreference.ForcastingServiceSetting.IsEnabled && forcastingResponse.StatusCode == Status.Success) || !viewModel.PreferencesSetting.ForcastingPreference.ForcastingServiceSetting.IsEnabled)
                        {
                            if (CurrentUser.IsSuperAdmin || CurrentUser.IsAccountSpecialist)
                            {
                                await ContextFactory.Current.GetDomain<OnboardingDomain>().SentCredentialsEmailToCompanyUser(viewModel.User.Id, viewModel.User.IsAccountSfxOwned, viewModel.CompanyId);
                                return RedirectToAction("ViewCompanies", "SuperAdmin", new { area = "SuperAdmin" });
                            }
                            ResetCompanyClaims(viewModel.User.Company.Name, viewModel.User.Company.CompanyTypeId);
                            return RedirectToAction("Index", "Home", new { area = "" });
                        }
                        else
                        {
                            DisplayCustomMessages((MessageType)forcastingResponse.StatusCode, forcastingResponse.StatusMessage);
                        }

                    }
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessages.ToList());
                }
                return View(viewModel);
            }
        }

        [HttpGet]
        public ActionResult Terms()
        {
            return View();
        }

        private void ResetCompanyClaims(string companyName, int companyTypeId)
        {
            using (var tracer = new Tracer("OnboardingController", "ResetCompanyClaims"))
            {
                var Identity = new ClaimsIdentity(User.Identity);

                Identity.RemoveClaim(Identity.FindFirst(ApplicationSecurityConstants.SelectedProfile));
                if (companyTypeId == (int)CompanyType.Buyer || companyTypeId == (int)CompanyType.BuyerAndSupplier)
                {
                    Identity.AddClaim(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Buyer).ToString()));
                }
                else
                {
                    Identity.AddClaim(new Claim(ApplicationSecurityConstants.SelectedProfile, ((int)CompanyType.Supplier).ToString()));
                }
                Identity.RemoveClaim(Identity.FindFirst(ApplicationSecurityConstants.CompanyTypeId));
                Identity.AddClaim(new Claim(ApplicationSecurityConstants.CompanyTypeId, (companyTypeId).ToString()));
                Identity.RemoveClaim(Identity.FindFirst(ApplicationSecurityConstants.CompanyName));
                Identity.AddClaim(new Claim(ApplicationSecurityConstants.CompanyName, companyName));
                AuthenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                                                                        new ClaimsPrincipal(Identity),
                                                                        new AuthenticationProperties
                                                                        {
                                                                            IsPersistent = Convert.ToBoolean(Identity.FindFirst(ApplicationSecurityConstants.RememberMe).Value)
                                                                        });
            }
        }

        public PartialViewResult GetPricingMatrix()
        {
            return PartialView("_PartialHaulerPricingMatrix", new HaulerPricingMatrixViewModel());
        }
        private async Task<StatusViewModel> Saveforcastingdetails(OnboardingViewModel viewModel, Response response)
        {
            var forcastingResponse = await SaveforcastingSettingAccountLevel(viewModel.PreferencesSetting.ForcastingPreference, response, 0);
            return forcastingResponse;
        }
        private async Task<StatusViewModel> SaveforcastingSettingAccountLevel(ForcastingPreferenceViewModel viewModel, Response response, int preventityId)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            statusViewModel.StatusCode = Status.Success;
            if (viewModel != null && viewModel.ForcastingServiceSetting != null && response.StatusCode == (int)Status.Success)
            {
                statusViewModel = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().SaveForeCastingPreferanceSetting(viewModel, UserContext, (int)ForcastingSettingLevel.Account, response.EntityId, preventityId);
            }
            return statusViewModel;
        }
    }
}