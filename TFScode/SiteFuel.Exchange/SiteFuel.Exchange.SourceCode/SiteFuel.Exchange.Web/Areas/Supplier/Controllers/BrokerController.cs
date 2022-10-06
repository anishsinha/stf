using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier)]
    public class BrokerController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> BrokerSupplier()
        {
            using (var tracer = new Tracer("BrokerController", "BrokerSupplier"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().IsBrokerOrderExist(CurrentUser.CompanyId);
                ViewBag.IsOrderExist = response;
                TempData["IsOrderSummary"] = false;
                return View(new BrokerFilterViewModel());
            }
        }

        [HttpGet]
        public PartialViewResult BrokeredOrders(Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("BrokerController", "BrokeredOrders"))
            {
                var response = new BrokerFilterViewModel() { Currency = currency, CountryId = countryId };
                return PartialView("_PartialBrokerOrders", response);
            }
        }

        [HttpGet]
        public PartialViewResult BrokeredFuelRequests(Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            var response = new OrderFilterViewModel() { OrderId = 0, Filter = OrderFilterType.All, Currency = currency, CountryId = countryId };
            return PartialView("_PartialBrokeredFuelRequests", response);
        }

        [HttpGet]
        public PartialViewResult BrokeredFuelRequest()
        {
            var response = new OrderFilterViewModel() { OrderId = 0, Filter = OrderFilterType.All };
            return PartialView("_PartialBrokeredFuelRequestGrid", response);
        }

        [HttpGet]
        public PartialViewResult BrokeredInvoices(Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            var response = new InvoiceFilterViewModel() { Filter = InvoiceFilterType.BrokerInvoices, Currency = currency, CountryId = countryId };
            return PartialView("_PartialInvoiceGrid", response);
        }

        [HttpGet]
        public PartialViewResult BrokeredDropTickets(Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("BrokerController", "BrokeredDropTickets"))
            {
                var response = new InvoiceFilterViewModel() { Filter = InvoiceFilterType.BrokerInvoices, AllowedInvoiceType = (int)InvoiceType.DigitalDropTicketManual, Currency = currency, CountryId = countryId };
                return PartialView("_PartialDigitalDropTicketGrid", response);
            }
        }

        [HttpGet]
        public PartialViewResult BrokeredActivity(Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("BrokerController", "BrokeredDropTickets"))
            {
                var response = new BrokerFilterViewModel() { Currency = currency, CountryId = countryId };
                return PartialView("_PartialBrokerActivityGrid", response);
            }
        }

        [HttpGet]
        public ActionResult GetBrokerMapData(OrderFilterViewModel orderFilter = null)
        {
            using (var tracer = new Tracer("BrokerController", "GetBrokerMapData"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetBrokerMap(CurrentUser.CompanyId, orderFilter)).Result;
                var jsonResult = new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                return PartialView("_PartialBrokerOrderMapView", jsonResult);
            }
        }

        [HttpPost]
        public async Task<ActionResult> BrokeredFuelRequestView(BrokerFuelRequestDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("BrokerController", "BrokeredFuelRequestView"))
            {
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetBrokerFuelRequestGrid(CurrentUser.CompanyId, requestModel);
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
        public ActionResult BrokeredCounterOffersView(string StartDate, string EndDate, int fuelRequestId = 0, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("BrokerController", "BrokeredCounterOffersView"))
            {
                var response = ContextFactory.Current.GetDomain<FuelRequestDomain>().GetBrokeredCounterOfferGridAsync(UserContext, StartDate, EndDate, fuelRequestId, currency, countryId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> OrdersGrid(OrderDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("BrokerController", "OrdersGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetBrokerOrders(CurrentUser.CompanyId, requestModel);
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
        public async Task<ActionResult> Activity(string StartDate, string EndDate, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("BrokerController", "Activity"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetBrokerActivityAsync(CurrentUser.CompanyId, StartDate, EndDate, currency, countryId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            using (var tracer = new Tracer("BrokerController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetBrokerFuelRequestAsync(id, CurrentUser.CompanyId, false);
                if (response.StatusCode != Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                response.DisplayMode = PageDisplayMode.View;
                response.FuelRequestId = id;
                return View("Details", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Details(BrokerFuelRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("BrokerController", "Details(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.UpdatedBy = CurrentUser.Id;
                    viewModel.Details.PrivateSupplierList.AddedById = CurrentUser.Id;
                    viewModel.Details.PrivateSupplierList.CompanyId = CurrentUser.CompanyId;

                    var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().UpdateOpenBrokerFuelRequestAsync(viewModel);

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("Details", "Order", new { area = "Supplier", id = viewModel.Details.OrderId });
                    }
                }

                return RedirectToAction("Details", "Broker", new { area = "Supplier", id = viewModel.Details.FuelDeliveryDetails.FuelRequestId });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Cancel(int id)
        {
            using (var tracer = new Tracer("BrokerController", "Cancel"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().CancelFuelRequestAsync(id, UserContext);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Broker", new { area = "Supplier", id = id });
            }
        }
        // GET: Supplier/Broker
        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Create(int id)
        {
            using (var tracer = new Tracer("BrokerController", "Create"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetBrokerFuelRequestAsync(id, CurrentUser.CompanyId, true);
                response.Details.PrivateSupplierList.IsPublicRequest = true;
                response.Details.FuelDeliveryDetails.PoContactId = CurrentUser.Id;
                if (response.StatusCode != Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }

                response.DisplayMode = PageDisplayMode.Create;
                return View(response);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Create(BrokerFuelRequestViewModel brokerViewModel)
        {
            using (var tracer = new Tracer("BrokerController", "Create(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SaveBrokerFuelRequestAsync(UserContext, brokerViewModel);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("Details", "Broker", new { area = "Supplier", id = brokerViewModel.FuelRequestId });
                    }
                }
                return View(brokerViewModel);
            }
        }
        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> CloneRequest(int id)
        {
            using (var tracer = new Tracer("BrokerController", "CloneRequest"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetCloneRequestAsync(id);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageGetCloneFuelRequestFailed);
                }
                return View("~/Areas/Buyer/Views/FuelRequest/CloneRequest.cshtml", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> CloneRequest(CloneRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("BrokerController", "CloneRequest"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SaveCloneFuelRequestAsync(viewModel, CurrentUser.Id, true);
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("Create", "Broker", new { area = "Supplier", id = viewModel.Id });
                    }
                    else
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageCloneFuelRequestFailed);
                    }
                }

                return View("~/Areas/Buyer/Views/FuelRequest/CloneRequest.cshtml", viewModel);
            }
        }

        [HttpGet]
        public JsonResult GetPrivateSupplierList()
        {
            using (var tracer = new Tracer("BrokerController", "GetPrivateSupplierList"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetPrivateSupplierList(CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> AddPrivateSupplierList(string name, List<int> suppliers)
        {
            using (var tracer = new Tracer("BrokerController", "AddPrivateSupplierList"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SavePrivateSupplierListAsync(CurrentUser.CompanyId, name, suppliers, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult DifferentFuelPrice()
        {
            return PartialView("_PartialDifferentFuelPrice", new DifferentFuelPriceViewModel());
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult DeliveryFeeByQuantity(string prefix, Currency currency, UoM uoM)
        {
            var deliveryFeeByQuantityModel = new DeliveryFeeByQuantityViewModel();
            deliveryFeeByQuantityModel.CollectionHtmlPrefix = prefix;
            deliveryFeeByQuantityModel.Currency = currency;
            deliveryFeeByQuantityModel.UoM = uoM;
            return PartialView("_PartialDeliveryFeeByQuantity", deliveryFeeByQuantityModel);
        }

        [HttpGet]
        public ActionResult CommonFeeTypes(Currency currency, UoM uoM, bool isConstraintFee = false, bool isCommonFee = false, int truckLoadType = (int)TruckLoadTypes.LessTruckLoad)
        {
            var model = isConstraintFee ? new FeesViewModel
            {
                FeeConstraintTypeId = (int)FeeConstraintType.Weekend,
                CommonFee = isCommonFee,
                TruckLoadType = truckLoadType
            } : new FeesViewModel
            {
                CommonFee = isCommonFee,
                TruckLoadType = truckLoadType
            };

            model.CommonFee = isCommonFee;
            model.Currency = currency;
            model.UoM = uoM;
            return PartialView("_PartialBrokerFeeType", model);
        }

        [HttpGet]
        public ActionResult OtherFeeType()
        {
            return PartialView("_PartialBrokerOtherFeeType", new FeesViewModel());
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult AdditionalFee()
        {
            return PartialView("_PartialBrokerAdditionalFee", new AdditionalFeeViewModel());
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult SpecialInstruction()
        {
            return PartialView("_PartialSpecialInstruction", new SpecialInstructionViewModel());
        }
    }
}