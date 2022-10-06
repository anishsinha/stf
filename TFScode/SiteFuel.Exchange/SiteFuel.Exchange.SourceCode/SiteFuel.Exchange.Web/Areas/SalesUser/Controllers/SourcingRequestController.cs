using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteFuel.Exchange.Web.Controllers;
using System.Threading.Tasks;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Core.StringResources;

namespace SiteFuel.Exchange.Web.Areas.SalesUser.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.BuyerAndSupplier, CompanyType.SupplierAndCarrier, CompanyType.BuyerSupplierAndCarrier)]
    public class SourcingRequestController : BaseController
    {
        // GET: SalesUser/SourcingRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(SourcingRequestViewModel sourcingRequestViewModel)
        {
            if (sourcingRequestViewModel.CustomerDetails.ContactPersons != null && sourcingRequestViewModel.CustomerDetails.ContactPersons.Any())
            {
                var userEmails = sourcingRequestViewModel.CustomerDetails.ContactPersons.Select(t => t.Email).ToList();
                userEmails.Add(sourcingRequestViewModel.CustomerDetails.Email);
                var duplicate = userEmails
                                        .GroupBy(x => x)
                                        .Where(group => group.Count() > 1)
                                        .Select(group => group.Key).ToList();

                if (duplicate != null && duplicate.Any())
                {
                    string duplicateEmails = string.Join(", ", duplicate);
                    StatusViewModel status = new StatusViewModel();
                    status.StatusCode = Status.Failed;
                    status.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageDuplicateEntries,
                        new[] { duplicate.Count == 1 ? $"{duplicateEmails} is" : $"{duplicateEmails} are" });
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
            }

            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().Create(UserContext, sourcingRequestViewModel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(SourcingRequestViewModel sourcingRequestViewModel)
        {
            if (sourcingRequestViewModel.CustomerDetails.ContactPersons != null && sourcingRequestViewModel.CustomerDetails.ContactPersons.Any())
            {
                var userEmails = sourcingRequestViewModel.CustomerDetails.ContactPersons.Select(t => t.Email).ToList();
                userEmails.Add(sourcingRequestViewModel.CustomerDetails.Email);
                var duplicate = userEmails
                                        .GroupBy(x => x)
                                        .Where(group => group.Count() > 1)
                                        .Select(group => group.Key).ToList();

                if (duplicate != null && duplicate.Any())
                {
                    string duplicateEmails = string.Join(", ", duplicate);
                    StatusViewModel status = new StatusViewModel();
                    status.StatusCode = Status.Failed;
                    status.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageDuplicateEntries,
                        new[] { duplicate.Count == 1 ? $"{duplicateEmails} is" : $"{duplicateEmails} are" });
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
            }
            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().Update(UserContext, sourcingRequestViewModel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetRequestDetails(int Id)
        {
            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().GetRequestDetails(Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> GetSourcingDetails(int Id)
        {
            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().GetSourcingDetails(Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetPreferencesSettings()
        {
            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().GetPreferencesSettings(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetSourcingCompanyContactPersons(int companyId)
        {
            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().GetSourcingCompanyContactPersons(companyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetSourcingContactPersonDetails(int userId)
        {
            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().GetSourcingContactPersonDetails(userId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetJobDetails(string jobName, string companyName)
        {
            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().GetSourcingJobDetails(jobName, companyName, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetSourcingRequestGrid(SourcingRequestDisplayStatus RequestStatus, bool isFromDashboard = false) 
        {
            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().GetSourcingRequestGrid(UserContext.CompanyId, RequestStatus, isFromDashboard);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> ChangesSourcingRequestStatus(SourcingRequestStatus sourcingRequestStatus, int Id)
        {
            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().ChangesSourcingRequestStatus(sourcingRequestStatus,Id,UserContext.Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> CreateOrderFromSourcingRequest(SourcingRequestViewModel sourcingRequestViewModel)
        {
            SourcingRequestDomain sourcingRequestDomain = new SourcingRequestDomain();
            var response = new StatusViewModel();
            if (sourcingRequestViewModel.FuelPricingDetails.PricingTypeId == (int)PricingType.Suppliercost)
            {
                var availableGlobalCost = await ContextFactory.Current.GetDomain<CurrentCostDomain>()
                                                                      .GetGlobalCost(UserContext, sourcingRequestViewModel.FuelDetails.FuelTypeId.Value, sourcingRequestViewModel.AddressDetails.StateId, 
                                                                                     sourcingRequestViewModel.AddressDetails.UOM, sourcingRequestViewModel.AddressDetails.Currency);
                if (availableGlobalCost != 0 )
                {
                    response = await sourcingRequestDomain.CreateOrderFromSourcingRequest(UserContext, sourcingRequestViewModel);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.SourcingErrorGlobalCostNotProvided;
                }
            }
            else
            {
                response = await sourcingRequestDomain.CreateOrderFromSourcingRequest(UserContext, sourcingRequestViewModel);
            }
            if (response.StatusCode == Status.Success)
            {
                response = await sourcingRequestDomain.ChangesSourcingRequestStatus(SourcingRequestStatus.OrderCreated, sourcingRequestViewModel.Id, UserContext.Id);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> IsSourcingCompanyExist(bool IsNewCompany, string CompanyName)
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
        public ActionResult GetSourcingCityGroupTerminals(int stateId, bool allStates = false, int selectedCityRackId = 0, PricingSource sourceId = PricingSource.Axxis)
        {
            var pricingDomain = ContextFactory.Current.GetDomain<ExternalPricingDomain>();
            var response = pricingDomain.GetCityGroupTerminalsByStateId(stateId, allStates, selectedCityRackId, sourceId);
            return Json(response, JsonRequestBehavior.AllowGet);
        
        }

        public async Task<ActionResult> IsCitySourcingGroupTerminalPriceAvailable(int jobId, int? fueltypeId, int? selectedCityRackId, float? lattitude, float? longitude, string countryCode, PricingSource sourceId = PricingSource.Axxis)
        {
            var domain = ContextFactory.Current.GetDomain<PricingServiceDomain>();
            var productId = ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelTypeId(fueltypeId ?? 0, (int)sourceId, (int)PricingType.RackHigh);

            if (sourceId == PricingSource.Axxis && productId > 0)
            {
                productId = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetExternalProductId(productId);
            }

            var response = await domain.IsCityRackPriceAvailable(productId, selectedCityRackId ?? 0, sourceId, DateTimeOffset.UtcNow.DateTime);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetClosedTerminal(int fuelTypeId, decimal latitude, decimal longitude, int countryId, int pricingCodeId, string terminal = "", int orderId = 0, int pricingSourceId = (int)PricingSource.Axxis)
        {
            if (orderId > 0)
            {
                var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetClosestTerminals(orderId, terminal);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var fueltype = ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelTypeId(fuelTypeId, pricingSourceId);
                var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetClosestTerminals(fueltype, latitude, longitude, countryId, terminal, pricingCodeId, UserContext.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetOpisTerminals(int cityRackId = 0, decimal latitude = 0, decimal longitude = 0, int countryId = 1, string terminal = "", PricingSource source = PricingSource.Axxis)
        {
            var pricingDomain = ContextFactory.Current.GetDomain<ExternalPricingDomain>();
            var response = pricingDomain.GetOpisTerminals(cityRackId, latitude, longitude, countryId, terminal.Trim(), source, UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetSourcingPricingCodes(int PricingTypeId = 0, int PricingSourceId = (int)PricingSource.Axxis, string Prefix = null, int feedTypeId = 0, int fuelClassTypeId = 0, int tfxProdId = 0)
        {
            PricingCodesRequestViewModel request = new PricingCodesRequestViewModel { PricingTypeId = PricingTypeId, PricingSourceId = PricingSourceId, Search = Prefix, FeedTypeId = feedTypeId, FuelClassTypeId = fuelClassTypeId, TFxProductId = tfxProdId };
            var codes = await ContextFactory.Current.GetDomain<PricingServiceDomain>().GetPricingCodesAsync(request);
            return Json(codes, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserContext()
        {
            return Json(UserContext, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateViewedStatus(int Id, bool IsViewed = true)
        {
            var response = await ContextFactory.Current.GetDomain<SourcingRequestDomain>().UpdateViewedStatus(Id, UserContext.Id, IsViewed);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFeeTypes(Currency currency = Currency.None)
        {
            var masterDomain = new MasterDomain();
            var response = masterDomain.GetFeeTypesAsync(UserContext.CompanyId,currency);
            response.RemoveAll(t => t.Code.Equals(((int)FeeType.DryRunFee).ToString()));
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFeeSubTypes(string currency)
        {
            var masterDomain = new MasterDomain();
            var response = masterDomain.GetAllFeeSubTypes(currency);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetDispatchRegions()
        {
            var dispatchRegions = await ContextFactory.Current.GetDomain<DispatcherDomain>().GetDispatcherRegionsAsync(UserContext);
            var response = dispatchRegions.Select(t => new DropdownDisplayExtended { Id = t.Id, Name = t.Name }).ToList();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}