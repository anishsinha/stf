using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.BillingStatement;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Controllers
{
    [AllowAnonymous]
    public class ValidationController : BaseController
    {
        public async Task<JsonResult> IsCompanyExist(RegisterViewModel user)
        {
            bool result = await ContextFactory.Current.GetDomain<CompanyDomain>().IsCompanyExist(user.Company.Name, user.Company.Id);
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsOnboardedCompany(string companyName)
        {
            bool result = await ContextFactory.Current.GetDomain<CompanyDomain>().IsOnboardedCompany(companyName);
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsPhoneNumberValid(string phoneNumber)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(phoneNumber))
                result = ContextFactory.Current.GetDomain<NotificationDomain>().IsPhoneNumberValid(phoneNumber.Trim());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsEmailExist(string email)
        {
            bool result = await ContextFactory.Current.GetDomain<HelperDomain>().IsEmailExistAsync(email.Trim());
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsSourceRegionExist(string name, int id)
        {
            bool result = await ContextFactory.Current.GetDomain<HelperDomain>().IsSourceRegionExist(name.Trim(), id, CurrentUser.CompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsAdditionalUserEmailExist(string email)
        {
            bool result = await ContextFactory.Current.GetDomain<HelperDomain>().IsAdditionalUserEmailExistAsync(email.Trim());
            return Json(!result, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> IsNewDriverUserEmailExist(string driverEmail)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(driverEmail))
            {
                result = await ContextFactory.Current.GetDomain<HelperDomain>().IsAdditionalUserEmailExistAsync(driverEmail.Trim());
            }
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsLicenseNumberExist(string licenseNumber, int Id)
        {
            bool result = await ContextFactory.Current.GetDomain<HelperDomain>().IsLicenseNumberExist(licenseNumber.Trim().ToLower(), Id);
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsTPOEmailExists(ThirdPartyOrderViewModel viewModel)
        {
            bool result = await ContextFactory.Current.GetDomain<HelperDomain>().IsEmailExistAsync(viewModel.CustomerDetails.Email.Trim(), true);
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> IsTPOCompanyExist(ThirdPartyOrderViewModel viewModel)
        {
            bool result;
            if (viewModel.CustomerDetails.IsNewCompany)
            {

                result = await ContextFactory.Current.GetDomain<CompanyDomain>().IsCompanyExist(viewModel.CustomerDetails.CompanyName);
            }
            else
            {
                result = await ContextFactory.Current.GetDomain<CompanyDomain>().IsOnboardedCompany(viewModel.CustomerDetails.CompanyName);
            }

            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsCompanyNameExist(bool IsNewCompany, string CompanyName)
        {
            bool result;
            if (IsNewCompany)
            {
                result = await ContextFactory.Current.GetDomain<CompanyDomain>().IsCompanyExist(CompanyName);
            }
            else
            {
                result = await ContextFactory.Current.GetDomain<CompanyDomain>().IsOnboardedCompany(CompanyName);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsExistingCompany(bool IsNewCompany, string CompanyName)
        {
            bool result = false;
            if (IsNewCompany)
            {
                result = await ContextFactory.Current.GetDomain<CompanyDomain>().IsCompanyExist(CompanyName);
            }
            //else
            //{
            //    result = await ContextFactory.Current.GetDomain<CompanyDomain>().IsOnboardedCompany(CompanyName);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsTPOCompanyExistInUpdateMode(OrderDetailsViewModel viewModel)
        {
            bool result = await ContextFactory.Current.GetDomain<CompanyDomain>().IsCompanyExist(viewModel.BuyerCompanyName, viewModel.BuyerCompanyId);
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsEmailExistTpoInUpdateMode(OrderDetailsViewModel viewModel)
        {
            bool result = await ContextFactory.Current.GetDomain<HelperDomain>().IsEmailExistAsync(viewModel.BuyerUserEmail.Trim(), viewModel.BuyerUserId);
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidAddress([Bind(Include = "Address")]string address)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidAddress(address);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidCompanyAddress(CompanyAddressViewModel companyAddress)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidAddress(companyAddress.Address);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidExternalCompanyAddress(string Address)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidAddress(Address);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidBillingAddress(CompanyAddressViewModel address)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidAddress(address.BillingAddress.Address);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidJobAddress(JobViewModel job)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidAddress(job.Address);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidTPOJobAddress(ThirdPartyOrderViewModel viewModel)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidAddress(viewModel.AddressDetails.Address);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidJobName(JobViewModel job)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidJobName(job.Id, job.Name, CurrentUser.CompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidSiteId(JobViewModel job)
        {
            bool result = ContextFactory.Current.GetDomain<JobDomain>().IsValidSiteId(job.Id, job.JobID, CurrentUser.CompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidDealName(DiscountViewModel discount)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidDealName(discount.InvoiceId, discount.DealName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidNewJobName(string NewJobName)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidJobName(0, NewJobName, CurrentUser.CompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidTankId(int jobId, int assetId, string tankId, string storageId)//AssetViewModel asset)
        {
            bool result = ContextFactory.Current.GetDomain<TankBulkUploadDomain>().IsValidTankId(jobId, assetId, tankId, storageId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidFullAddress(AddressViewModel address)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidFullAddress(address);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidPONumber(FuelRequestViewModel fuelRequest)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidPONumber(fuelRequest.Id, fuelRequest.ExternalPoNumber, fuelRequest.CompanyId, fuelRequest.IsCounterOffer);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidPONumberInOrder(int orderId, int companyId, string poNumber)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidPONumberInOrder(orderId, companyId, poNumber);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidPONumberInBroker(BrokerFuelRequestViewModel fuelRequest)
        {
            bool result = true;
            if (!fuelRequest.Terms.IsProFormaPoEnabled)
                result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidPONumber(0, fuelRequest.Terms.ExternalPoNumber, fuelRequest.Terms.CompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidPONumberInCloneRequest(CloneRequestViewModel fuelRequest)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidPONumber(0, fuelRequest.ExternalPoNumber, fuelRequest.CompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidProductDisplayName(ProductViewModel product)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidProductDisplayName(product.Id, product.DisplayName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsTPOValidPONumber(ThirdPartyOrderViewModel viewModel)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidPONumberInTPO(viewModel.PONumber, viewModel.CustomerDetails.CompanyName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidAssetName(AssetViewModel asset)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidAssetName(asset.Id, asset.Name, asset.CompanyId, asset.Type, asset.JobId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult IsAssetExists(string asset, int orderId)
        //{
        //    bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsAssetExists(asset, orderId);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult IsValidSupplierListName(string Name)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidSupplierListName(Name, CurrentUser.CompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsFuelRequestExistForJobStartDate(JobViewModel job)
        {
            bool result = await ContextFactory.Current.GetDomain<HelperDomain>().IsFuelRequestExistForJobStartDate(job);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> IsFuelRequestExistForJobEndDate(JobViewModel job)
        {
            bool result = await ContextFactory.Current.GetDomain<HelperDomain>().IsFuelRequestExistForJobEndDate(job);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAddressByZip(string zipCode, string address = null)
        {
            var zipAddress = zipCode;
            if (!string.IsNullOrWhiteSpace(address))
            {
                zipAddress = GoogleApiDomain.AddCountryCodeToZipcode(zipCode);
                zipAddress = address + ", " + zipAddress;
            }
            var geoCodes = GoogleApiDomain.GetGeocode(zipAddress);
            if (geoCodes == null)
            {
                geoCodes = new Geocode();
            }
            geoCodes.ZipCode = zipCode;
            return Json(geoCodes, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAddress(string address)
        {
            var _address = GoogleApiDomain.GetAddress(address);
            return Json(_address, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsExternalCompanyExist(ExternalCompanyViewModel companyDetails)
        {
            bool result = ContextFactory.Current.GetDomain<CompanyDomain>().IsExternalCompanyExist(companyDetails.Name, companyDetails.Id);
            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidStatementId(BillingScheduleViewModel viewModel)
        {
            bool result = ContextFactory.Current.GetDomain<HelperDomain>().IsValidBillingStatementId(viewModel.BillingStatementId, CurrentUser.CompanyId, viewModel.Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCookieExpirationTime()
        {
            int cookieExpirationMinutes = ContextFactory.Current.GetDomain<ApplicationDomain>().GetApplicationSettingValue(ApplicationConstants.KeyAppSettingCookieExpirationTime,300);
            return Json(cookieExpirationMinutes, JsonRequestBehavior.AllowGet);
        }
    }
}