using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier, CompanyType.SupplierAndCarrier)]
    public class FuelRequestController : BaseController
    {
        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> CounterOffer(int fuelRequestId, int supplierId = 0)
        {
            using (var tracer = new Tracer("FuelRequestController", "CounterOffer"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestCounterOfferAsync(fuelRequestId, CurrentUser.Id, CurrentUser.CompanyId);
                response.CounterOfferSupplierId = supplierId;
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadCreateCounterOfferFailed);
                    return RedirectToAction("Details", "FuelRequest", new { id = fuelRequestId });
                }
                else if (response.CounterOfferDetails.BuyerStatus == (int)CounterOfferStatus.Pending)
                {
                    return RedirectToActionPermanent("Details", "CounterOffer", new { fuelRequestId = fuelRequestId });
                }

                if (response.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity.Count == 0)
                {
                    response.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity.Add(new DeliveryFeeByQuantityViewModel());
                }

                response.IsCounterOffer = true; // set iscounter offer flag to true
                if (response.IsBrokeredCounterOffer)
                {
                    return View("~/Areas/Supplier/Views/CounterOffer/Create.cshtml", response);
                }
                else
                {
                    return View("~/Areas/Buyer/Views/FuelRequest/Create.cshtml", response);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> CounterOffer(FuelRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "CounterOffer(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    StatusViewModel response;
                    if (viewModel.CounterOfferSupplierId > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SaveFuelRequestAsync(viewModel, true, viewModel.CounterOfferSupplierId, UserContext);
                    }
                    else
                    {
                        response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SaveFuelRequestAsync(viewModel, false, CurrentUser.Id, UserContext);
                    }
                    if (response.StatusCode == Status.Success)
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        return RedirectToAction("Details", "CounterOffer", new { area = "Supplier", fuelRequestId = viewModel.Id, supplierId = viewModel.CounterOfferSupplierId });
                    }
                    else
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageCreateRequestFailed);
                    }
                }
                if (viewModel.IsBrokeredCounterOffer)
                {
                    return View("~/Areas/Supplier/Views/CounterOffer/Create.cshtml", viewModel);
                }
                else
                {
                    return View("~/Areas/Buyer/Views/FuelRequest/Create.cshtml", viewModel);
                }
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Edit(int id)
        {
            using (var tracer = new Tracer("FuelRequestController", "Edit"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestAsync(id, CurrentUser.Id);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadFuelRequestDetailsFailed);
                }
                return View("~/Areas/Buyer/Views/FuelRequest/Create.cshtml", response);
            }
        }

        [HttpGet]
        [ActionName("View")]
        public ActionResult FuelRequests(FuelRequestFilterType filter = FuelRequestFilterType.All)
        {
            using (var tracer = new Tracer("FuelRequestController", "FuelRequests"))
            {
                var response = ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestFilter(0, filter);
                return View("FuelRequestGrid", response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> FuelRequestGrid(FuelRequestDataTableModel requestModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "FuelRequestGrid"))
            {
                var dataTableSearchModel = new DataTableSearchModel(requestModel);
                var fuelRequestStat = new USP_SupplierRequestsViewModel()
                {
                    CompanyId = CurrentUser.CompanyId,
                    UserId = CurrentUser.Id,
                    dataTableSearchValues = dataTableSearchModel,
                    AddressId = requestModel.AddressId,
                    Broadcast = (int)requestModel.BrodcastType,
                    StatusFilter = (int)requestModel.Filter,
                    CurrencyType = (int)requestModel.Currency,
                    CountryId = requestModel.CountryId
                };
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetSupplierFuelReqestGridAsync(fuelRequestStat, requestModel.FromDate, requestModel.ToDate);

                var totalCount = 0;
                if (response.Count > 0)
                    totalCount = response[0].TotalCount;
                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = requestModel.draw,
                        data = response,
                        recordsTotal = totalCount,
                        recordsFiltered = totalCount

                    },

                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public async Task<string> CalculateTerminalPrice(int jobId, int productId, int rackType, decimal rackPrice, bool includeTaxes, int pricingCodeId, int cityRackTerminalId = 0, int sourceId = 1)
        {
            using (var tracer = new Tracer("FuelRequestController", "CalculateTerminalPrice"))
            {
                decimal response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().CalculateTerminalPrice(jobId, productId, rackType, rackPrice, includeTaxes, pricingCodeId, cityRackTerminalId, sourceId);
                return response.GetPreciseValue(6).ToString();
            }
        }

        [HttpGet]
        public ActionResult PartialCounterOfferGrid(int fuelRequestId = 0, FuelRequestFilterViewModel frFilter = null, string fromDate = "", string toDate = "")
        {
            using (var tracer = new Tracer("FuelRequestController", "PartialCounterOfferGrid"))
            {
                var response = ContextFactory.Current.GetDomain<FuelRequestDomain>().GetSupplierCounterOfferGridAsync(CurrentUser.CompanyId, CurrentUser.Id, fuelRequestId, frFilter, fromDate, toDate);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            using (var tracer = new Tracer("FuelRequestController", "Details"))
            {
                var isCounterOfferAvailable = ContextFactory.Current.GetDomain<HelperDomain>().IsCounterOfferAvailable(id, CurrentUser.Id);
                if (isCounterOfferAvailable)
                {
                    return RedirectToAction("Details", "CounterOffer", new { area = "Supplier", fuelRequestId = id });
                }
                else
                {
                    var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestDetailsAsync(id, CurrentUser.CompanyId);
                    if (response.StatusCode == Status.Failed)
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadFuelRequestDetailsFailed);
                    }
                    return View(response);
                }
            }
        }

        public async Task<ActionResult> FuelRequestDetails(int id)
        {
            using (var tracer = new Tracer("FuelRequestController", "FuelRequestDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestDetailsAsync(id, CurrentUser.CompanyId);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadFuelRequestDetailsFailed);
                }
                return View("Details", response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> GetJobDates(int jobId = 0)
        {
            using (var tracer = new Tracer("FuelRequestController", "GetJobDates"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetSelectedJobDatesAsync(jobId);
                response.IsResaleEnabled = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Decline(int id)
        {
            using (var tracer = new Tracer("FuelRequestController", "Decline"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().DeclineFuelRequest(id, CurrentUser.Id);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                return RedirectToAction("View", "FuelRequest", new { area = "Supplier" });
            }
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Accept(int fuelRequestId, SourceRegionsViewModel sourceRegionModel = null)
        {
            //Change this function to JsonResult when Source Region and termainals are uncomment
            using (var tracer = new Tracer("FuelRequestController", "Accept"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().AcceptFuelRequest(UserContext, fuelRequestId, sourceRegionModel);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("View", "FuelRequest", new { area = "Supplier" });
                }
                else if (response.IsFirstTimeBuyer)
                {
                    SendCreditAppNotification(response);
                }
                //return Json(response, JsonRequestBehavior.AllowGet);
                return RedirectToAction("View", "Order", new { area = "Supplier", filter = OrderFilterType.All, orderId = response.OrderId });
            }
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public ActionResult TermsAndConditions()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult DifferentFuelPrice()
        {
            return PartialView("~/Views/Shared/_PartialDifferentFuelPrice.cshtml", new DifferentFuelPriceViewModel());
        }

        [HttpGet]
        //[OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult DeliveryFeeByQuantity(string prefix, Currency currency, UoM uoM)
        {
            var deliveryFeeByQuantityModel = new DeliveryFeeByQuantityViewModel();
            deliveryFeeByQuantityModel.CollectionHtmlPrefix = prefix;
            deliveryFeeByQuantityModel.UoM = uoM;
            deliveryFeeByQuantityModel.Currency = currency;
            return PartialView("~/Views/Shared/_PartialDeliveryFeeByQuantity.cshtml", deliveryFeeByQuantityModel);
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult AdditionalFee()
        {
            return PartialView("_PartialAdditionalFee", new AdditionalFeeViewModel());
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult SpecialInstruction()
        {
            return PartialView("~/Views/Shared/_PartialSpecialInstruction.cshtml", new SpecialInstructionViewModel());
        }

        public JsonResult GetMstProducts(int productId = 0)
        {
            var products = Helpers.CommonHelperMethods.GetMstProducts();
            var response = products.FirstOrDefault(t => t.Id == productId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeliverySchedule(int scheduleType = (int)DeliveryScheduleType.SpecificDates, bool IsBrokeredFuelRequest = false)
        {
            if (IsBrokeredFuelRequest)
            {
                return PartialView("_PartialBrokerDeliverySchedule", new DeliveryScheduleViewModel() { ScheduleType = scheduleType, CreatedBy = CurrentUser.Id });
            }

            return PartialView("~/Areas/Buyer/Views/Shared/_PartialDeliveryScheduleFR.cshtml", new DeliveryScheduleViewModel() { ScheduleType = scheduleType, CreatedBy = CurrentUser.Id });
        }

        [HttpGet]
        public async Task<JsonResult> GetProductList(ProductDisplayGroups displayGroupId, int jobId = 0, decimal radius = 100, string zipCode = "", PricingSource source = PricingSource.Axxis)
        {
            using (var tracer = new Tracer("FuelRequestController", "GetProductList"))
            {
                var response = new List<DropdownDisplayItem>();
                var pricingDomain = ContextFactory.Current.GetDomain<ExternalPricingDomain>();
                //if (source == PricingSource.Axxis)
                //{
                //    response = await pricingDomain.GetAxxisFuelProducts(displayGroupId, CurrentUser.CompanyId, jobId, radius, zipCode);
                //}
                //else
                //{
                //    response = pricingDomain.GetSourceBasedFuelProducts(source);
                //}
                response = await pricingDomain.GetAxxisFuelProducts(displayGroupId, CurrentUser.CompanyId, jobId, radius, zipCode);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetNewFuelRequests()
        {
            var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetLatestReceivedFuelRequestsAsync(CurrentUser.CompanyId, CurrentUser.Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CommonOtherFeeTypes(Currency currency, UoM uoM, bool isConstraintFee = false, bool isCommonFee = false, int truckLoadType = (int)TruckLoadTypes.LessTruckLoad)
        {
            var model = isConstraintFee ? new FeesViewModel { FeeConstraintTypeId = (int)FeeConstraintType.Weekend } : new FeesViewModel();
            model.CommonFee = isCommonFee;
            model.TruckLoadType = truckLoadType;
            model.CommonFee = isCommonFee;
            model.Currency = currency;
            model.UoM = uoM;
            return PartialView("_PartialFeeType", model);
        }

        [HttpPost]
        public JsonResult GetFeeSubTypes(string feeTypeId, Currency currency = Currency.None)
        {
            using (var tracer = new Tracer("FuelRequestController", "GetFeeSubTypes"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetAllFeeSubTypes(feeTypeId, currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> AcknowledgeNomination(int nominationId)
        {
            using (var tracer = new Tracer("FuelRequestController", "AcknowledgeNomination"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().AcknowledgeNomination(nominationId, UserContext);
                if (response != null)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                return RedirectToAction("View", "FuelRequest", new { area = "Supplier" });
            }
        }
    }
}