using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Offer;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier)]
    public class OfferController : BaseController
    {
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        [HttpGet]
        public ActionResult Create(int OfferPricingId = 0, string custlist = "", Currency currency = Currency.USD, Country country = Country.USA)
        {
            OfferViewModel offerViewModel;
            if (OfferPricingId == 0)
            {
                offerViewModel = ContextFactory.Current.GetDomain<OfferDomain>().GetOfferViewModel(UserContext, currency, country);
            }
            else
            {
                offerViewModel = ContextFactory.Current.GetDomain<SupplierOfferDomain>().GetSupplierOfferDetail(UserContext.CompanyId, OfferPricingId);
            }
            if (!string.IsNullOrWhiteSpace(custlist))
            {
                offerViewModel.Customers = custlist.Split(',').Select(t => Convert.ToInt32(t)).ToList();
            }
            return View(offerViewModel);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        [HttpGet]
        public new ActionResult View(string custlist = "", Country country = Country.USA)
        {
            var vm = new OfferSummaryViewModel();
            if (!string.IsNullOrWhiteSpace(custlist))
            {
                vm.Customers = custlist.Split(',').Select(t => t).ToList();
            }
            vm.Country.Id = (int)country;
            return View("View", vm);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        [HttpGet]
        public ActionResult GetSupplierOffers(string custlist = "")
        {
            var vm = new OfferSummaryViewModel();
            if (!string.IsNullOrWhiteSpace(custlist))
            {
                vm.Customers = custlist.Split(',').Select(t => t).ToList();
            }
            return PartialView("_PartialSupplierOffers", vm);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public ActionResult SupplierOffersGrid(OfferItemDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("OfferController", "SupplierOffersGrid"))
            {
                var filter = requestModel.ParseSearchablesByName<OfferSummaryFilter>();

                filter.OfferEnumType = requestModel.OfferType;
                filter.CustomerIds = requestModel.Customers;

                var response = ContextFactory.Current.GetDomain<SupplierOfferDomain>().GetSupplierOfferGridAsync(UserContext.CompanyId, filter, requestModel.CountryId, (int)requestModel.Currency);
                return GetResult(requestModel.draw, response);
            }
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        [HttpGet]
        public async Task<ActionResult> PricingTable(int countryId)
        {
            var response = await ContextFactory.Current.GetDomain<OfferDomain>().InitialisePricingTable(UserContext, countryId);
            return PartialView("PricingTable", response);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        [HttpPost]
        public async Task<ActionResult> Create(OfferViewModel viewModel)
        {
            StatusViewModel response;
            viewModel.UpdatedBy = UserContext.Id;
            if (viewModel.Id > 0)
            {
                //update offer
                response = await ContextFactory.Current.GetDomain<OfferDomain>().UpdateOffer(viewModel);
            }
            else
            {
                //add new offer
                response = await ContextFactory.Current.GetDomain<OfferDomain>().SaveOffer(viewModel, UserContext);
            }
            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            if (response.StatusCode == Status.Failed)
                return View("Create", viewModel);
            else
                return RedirectToAction("View");
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        [HttpGet]
        public ActionResult Details(int offerPricingId)
        {
            OfferViewModel offerViewModel;
            offerViewModel = ContextFactory.Current.GetDomain<SupplierOfferDomain>().GetSupplierOfferDetail(UserContext.CompanyId, offerPricingId);
            return PartialView("Details", offerViewModel);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> SupplierPricingTable(OfferItemDataTableViewModel requestModel)
        {
            var dataTableSearchModel = new DataTableSearchModel(requestModel);
            var filter = new OfferFilterViewModel() { OfferType = requestModel.OfferType };
            var offerDomain = ContextFactory.Current.GetDomain<OfferDomain>();
            var response = await offerDomain.GetSupplierPricingTableAsync(CurrentUser.CompanyId, dataTableSearchModel, filter, requestModel.CountryId, (int)requestModel.Currency);
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

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult OfferLocationTypes()
        {
            return PartialView("~/Areas/Supplier/Views/Shared/_PartialOfferLocationType.cshtml", new OfferLocationViewModel());
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public ActionResult QuickUpdate(OfferQuickUpdateViewModel viewModel)
        {
            StatusViewModel response;
            if (viewModel.States.Count == 0)
            {
                viewModel.States.Add(0);
            }
            response = ContextFactory.Current.GetDomain<OfferQuickUpdateDomain>().QuickUpdate(viewModel, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> SavePreferenceSetting(OfferQuickUpdateViewModel model)
        {
            using (var tracer = new Tracer("OfferController", "SavePreferenceSetting"))
            {
                StatusViewModel response;
                response = await ContextFactory.Current.GetDomain<OfferDomain>().SavePreferenceSetting(model, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> LaunchToMarket(int offerPricingId)
        {
            using (var tracer = new Tracer("OfferController", "LaunchToMarket"))
            {
                StatusViewModel response;
                response = await ContextFactory.Current.GetDomain<OfferQuickUpdateDomain>().LaunchToMarketAsync(offerPricingId, UserContext.Id, UserContext.CompanyId);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetCustomersList(string tiers)
        {
            List<int> intTiers = new List<int>();
            var splitedValues = tiers.Split(',');
            foreach (var item in splitedValues)
            {
                int.TryParse(item, out int tier);
                intTiers.Add(tier);
            }
            var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
            var response = await masterDomain.GetSupplierCustomers(UserContext.CompanyId, intTiers);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetProductsOfExistingOffers(QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            response = await ContextFactory.Current.GetDomain<SupplierOfferDomain>().GetProductsOfExistingOffersAsync(UserContext.CompanyId, filter);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetStatesForExistingOffers(QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            response = await ContextFactory.Current.GetDomain<SupplierOfferDomain>().GetStatesForExistingOffersAsync(UserContext.CompanyId, filter);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetCitiesForExistingOffers(QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            response = await ContextFactory.Current.GetDomain<SupplierOfferDomain>().GetCitiesForExistingOffersAsync(UserContext.CompanyId, filter);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetZipsForExistingOffers(QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayExtended>();
            response = await ContextFactory.Current.GetDomain<SupplierOfferDomain>().GetZipsForExistingOffersAsync(UserContext.CompanyId, filter);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetPricingTypesForExistingOffers(QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            response = await ContextFactory.Current.GetDomain<SupplierOfferDomain>().GetPricingTypesForExistingOffersAsync(UserContext.CompanyId, filter);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetFeeTypesForExistingOffers(QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            response = await ContextFactory.Current.GetDomain<SupplierOfferDomain>().GetFeeTypeForExistingOffersAsync(UserContext.CompanyId, filter);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetFeeSubTypesForExistingOffers(QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            response = await ContextFactory.Current.GetDomain<SupplierOfferDomain>().GetFeeSubTypeForExistingOffersAsync(UserContext.CompanyId, filter);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetQuickUpdateHistory()
        {
            return PartialView("_PartialQuickUpdateHistory");
        }

        [HttpPost]
        public async Task<ActionResult> GetQuickUpdateHistoryGrid(OfferFilterViewModel postModel)
        {
            var offerQuickUpdateDomain = new OfferQuickUpdateDomain();
            var response = await offerQuickUpdateDomain.GetQuickUpdateHistory(UserContext, postModel.CountryId);
            response.draw = postModel.draw;
            return new JsonResult
            {
                Data = response,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public async Task<JsonResult> UndoQuickUpdate(int commandId)
        {
            var offerQuickUpdateDomain = new OfferQuickUpdateDomain();
            var response = await offerQuickUpdateDomain.QuickUpdateUndoAsync(UserContext, commandId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> GetQuickUpdatedItems(QuickUpdatedItemDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("OfferController", "GetQuickUpdatedItems"))
            {
                var domain = ContextFactory.Current.GetDomain<OfferQuickUpdateDomain>();
                var response = await domain.GetQuickUpdatedItemsAsync(requestModel.CommandId);
                return GetResult(requestModel.draw, response);
            }
        }

        private static ActionResult GetResult(int requestModeldraw, List<OfferViewModel> response)
        {
            var datagridResult = response.Select(x => new SupplierOfferGridViewModelItem
            {
                OfferPricingId = x.Id,
                Locations = string.Join("<br/>", x.LocationViewModel.Select(y => y.ToString())),
                CreatedDate = x.CreatedDate.ToString("MM/dd/yyyy hh:mm tt"),
                UpdatedDate = x.UpdatedDate.ToString("MM/dd/yyyy hh:mm tt"),
                Customers = x.CustomerNames.Count > 0 ? string.Join(", ", x.CustomerNames) : Resource.lblHyphen,
                Fees = x.FuelDeliveryDetails.FuelFees.FuelRequestFees.Count > 0 ? string.Join("<br/>", x.FuelDeliveryDetails.FuelFees.FuelRequestFees.Select(y => y.ToString())) : Resource.lblHyphen,
                FuelTypes = x.FuelTypeName,
                Name = x.Name,
                OfferType = (OfferType)x.OfferTypeId,
                Pricing = x.FuelPricing.FormattedPricing,
                Tiers = x.TierNames.Count > 0 ? string.Join(", ", x.TierNames) : Resource.lblHyphen,
                TruckLoadType = x.FuelPricing.FuelPricingDetails.TruckLoadTypes == TruckLoadTypes.FullTruckLoad ? Resource.lblFullTruckLoad : Resource.lblLessTruckLoad
            });

            var totalCount = response.Count;
            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    draw = requestModeldraw,
                    data = datagridResult,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount
                },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public JsonResult GetSupplierProducts(int companyId, int countryId, int sourceId)
        {
            var response = new List<DropdownDisplayItem>();
            var pricingDomain = ContextFactory.Current.GetDomain<MasterDomain>();
            response = pricingDomain.GetSupplierProducts(companyId, countryId, sourceId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllSupplierProducts(int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            var pricingDomain = ContextFactory.Current.GetDomain<MasterDomain>();
            response = pricingDomain.GetAllSupplierProducts(companyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}