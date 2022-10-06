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
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer)]
    public class FuelRequestController : BaseController
    {
        [HttpGet]
        [ActionName("View")]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        public ActionResult Index(int jobId = 0, FuelRequestFilterType filter = FuelRequestFilterType.All, string groupIds = "")
        {
            using (var tracer = new Tracer("FuelRequestController", "Index"))
            {
                var response = ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestFilter(jobId, filter, groupIds);
                RemoveReturnUrl();
                return View("View", response);
            }
        }




        public PartialViewResult FuelRequestsByJob(int jobId = 0, FuelRequestFilterType filter = FuelRequestFilterType.All)
        {
            using (var tracer = new Tracer("FuelRequestController", "FuelRequestsByJob"))
            {
                var response = ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestFilter(jobId, filter);
                return PartialView("View", response);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        public async Task<ActionResult> FuelRequestGrid(FuelRequestDataTableModel requestModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "FuelRequestGrid"))
            {
                var dataTableSearchModel = new DataTableSearchModel(requestModel);

                var dashboardDomain = new DashboardDomain();
                var fuelRequestDomain = new FuelRequestDomain(dashboardDomain);
                var decryptedGroupIds = dashboardDomain.DecryptData(requestModel.GroupIds);

                var filter = new FuelRequestFilterViewModel() { JobId = requestModel.JobId, BrodcastType = requestModel.BrodcastType, EndDate = requestModel.ToDate, StartDate = requestModel.FromDate, GroupIds = decryptedGroupIds, Filter = requestModel.Filter };
                var response = await fuelRequestDomain.GetBuyerFuelRequestGridAsync(CurrentUser.CompanyId, CurrentUser.Id, dataTableSearchModel, filter, requestModel.CountryId, (int)requestModel.Currency, filter.GroupIds);
                //if user login with branded supplier company URL then user will only see only branded supplier fuel requests.
                //we exclude the other fuel requests
                if (CurrentUser.BrandedCompanyId > 0)
                {
                    response = response.Where(top => top.AcceptedCompanyId == CurrentUser.BrandedCompanyId || top.AcceptedCompanyId == 0).ToList();
                    if (response.Count > 0)
                        response[0].TotalCount = response.Count;
                }
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
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        public async Task<ActionResult> GetJobDates(int jobId = 0)
        {
            using (var tracer = new Tracer("FuelRequestController", "GetJobDates"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetSelectedJobDatesAsync(jobId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Create(int jobId = 0, int fuelTypeId = 0, int fuelDisplayGroupId = (int)ProductDisplayGroups.CommonFuelType,
            int truckLoadTypeId = 0, int pricingSourceId = (int)PricingSource.Axxis, int pricingTypeId = 0, string pricingCode = null, int pricingCodeId = 0, string pricingCodeDesc = null)
        {
            using (var tracer = new Tracer("FuelRequestController", "Create"))
            {
                if (jobId == 0)
                {
                    RemoveReturnUrl();
                }
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetLastFuelRequestAsync(jobId, CurrentUser.CompanyId,
                    CurrentUser.Id, fuelTypeId, fuelDisplayGroupId, truckLoadTypeId, pricingSourceId, pricingTypeId, pricingCode, pricingCodeId, pricingCodeDesc);
                response.CompanyId = CurrentUser.CompanyId;
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadCreateFuelRequestFailed);
                }
                else if (response.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity.Count == 0)
                {
                    response.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity.Add(new DeliveryFeeByQuantityViewModel());
                }

                if (response.FuelRequestResale.ResaleCustomer.Count == 0)
                {
                    response.FuelRequestResale.ResaleCustomer.Add(new FuelRequestResaleCustomerViewModel());
                }

                return View(response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> CounterOffer(int fuelRequestId, int supplierId)
        {
            using (var tracer = new Tracer("FuelRequestController", "CounterOffer"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestCounterOfferAsync(fuelRequestId, supplierId, CurrentUser.CompanyId);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadCreateCounterOfferFailed);
                }
                else if (response.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity.Count == 0)
                {
                    response.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity.Add(new DeliveryFeeByQuantityViewModel());
                }
                return View("~/Areas/Buyer/Views/FuelRequest/Create.cshtml", response);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> CounterOffer(FuelRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "CounterOffer(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SaveFuelRequestAsync(viewModel, true, viewModel.CounterOfferSupplierId, UserContext);
                    //this will redirect to Job details page when cancel fuel request is requested from Job Details
                    if (IsReturnUrlExist())
                    {
                        return Redirect(GetReturnUrl());
                    }
                    if (response.StatusCode == Status.Success)
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        return RedirectToAction("Details", "CounterOffer", new { area = "Buyer", fuelRequestId = viewModel.Id, supplierId = viewModel.CounterOfferSupplierId });
                    }
                    else
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageCreateRequestFailed);
                    }
                }
                return View("~/Areas/Buyer/Views/FuelRequest/Create.cshtml", viewModel);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Accept(int fuelRequestId, int supplierId)
        {
            using (var tracer = new Tracer("FuelRequestController", "Accept"))
            {
                var response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().AcceptCounterOfferByBuyerAsync(UserContext, supplierId, fuelRequestId);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                if (response.StatusCode == Status.Success && response.IsFirstTimeBuyer)
                {
                    SendCreditAppNotification(response);
                }
                return RedirectToAction("View", "FuelRequest", new { area = "Buyer" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Save(FuelRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "Save"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.FuelDetails.UpdatedBy = CurrentUser.Id;
                    viewModel.FuelOfferDetails.PrivateSupplierList.AddedById = CurrentUser.Id;
                    viewModel.FuelOfferDetails.PrivateSupplierList.CompanyId = CurrentUser.CompanyId;
                    viewModel.CompanyId = CurrentUser.CompanyId;
                    viewModel.FuelDetails.StatusId = (int)FuelRequestStatus.Open;

                    StatusViewModel response;
                    if (viewModel.Id > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().UpdateFuelRequestAsync(viewModel, UserContext,true);
                    }
                    else
                    {
                        viewModel.FuelDetails.CreatedBy = CurrentUser.Id;
                        response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SaveFuelRequestAsync(viewModel, true, viewModel.CounterOfferSupplierId, UserContext,true);
                    }
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("Details", "FuelRequest", new { area = "Buyer", id = viewModel.Id, hideButtons = true });
                    }
                }

                return RedirectToAction("Create", "FuelRequest", new { area = "Buyer", id = viewModel.Job.JobId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Draft(FuelRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "Draft"))
            {
                //if (ModelState.IsValid)
                {
                    viewModel.FuelDetails.UpdatedBy = CurrentUser.Id;
                    viewModel.FuelOfferDetails.PrivateSupplierList.AddedById = CurrentUser.Id;
                    viewModel.FuelOfferDetails.PrivateSupplierList.CompanyId = CurrentUser.CompanyId;

                    StatusViewModel response;
                    if (viewModel.FuelDetails.FuelTypeId <= 0)
                    {
                        viewModel.FuelDetails.FuelTypeId = 1;
                    }
                    if (viewModel.Id > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().UpdateFuelRequestAsync(viewModel, UserContext);
                    }
                    else
                    {
                        viewModel.FuelDetails.CreatedBy = CurrentUser.Id;
                        response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SaveFuelRequestAsync(viewModel, true, viewModel.CounterOfferSupplierId, UserContext,true);
                    }
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("Edit", "FuelRequest", new { area = "Buyer", id = viewModel.Id });
                    }
                }

                return View("Create", viewModel);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public ActionResult TermsAndConditions()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Edit(int id)
        {
            using (var tracer = new Tracer("FuelRequestController", "Edit"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestAsync(id, 0, CurrentUser.Id, CurrentUser.CompanyId);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadFuelRequestDetailsFailed);
                }
                if (response.DisplayMode == PageDisplayMode.None)
                    return RedirectToAction("Index", "Unauthorized", new { area = "" });
                else
                    return View("Create", response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson, UserRoles.OnsitePerson)]
        public async Task<ActionResult> Details(int id, bool hideButtons = false)
        {
            using (var tracer = new Tracer("FuelRequestController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestAsync(id, 0, CurrentUser.Id);
                response.HideButtons = hideButtons;
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadFuelRequestDetailsFailed);
                }
                if (response.DisplayMode == PageDisplayMode.None)
                    return RedirectToAction("Index", "Unauthorized", new { area = "" });
                else
                    return View("Details", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Details(FuelRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "Details(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.FuelDetails.UpdatedBy = CurrentUser.Id;
                    viewModel.FuelOfferDetails.PrivateSupplierList.AddedById = CurrentUser.Id;
                    viewModel.FuelOfferDetails.PrivateSupplierList.CompanyId = CurrentUser.CompanyId;

                    StatusViewModel response;
                    response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().UpdateOpenFuelRequestAsync(viewModel);

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("Details", "FuelRequest", new { area = "Buyer", id = viewModel.Id });
                    }
                }

                return RedirectToAction("Details", "FuelRequest", new { area = "Buyer", id = viewModel.Id });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Cancel(int id)
        {
            using (var tracer = new Tracer("FuelRequestController", "Cancel"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().CancelFuelRequestAsync(id, UserContext);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);

                //this will redirect to Job details page when cancel fuel request is requested from Job Details
                if (IsReturnUrlExist())
                {
                    return Redirect(GetReturnUrl());
                }

                if (response.StatusCode == Status.Failed)
                {
                    return RedirectToAction("Edit", "FuelRequest", new { area = "Buyer", id = id });
                }
                else
                {
                    return RedirectToAction("View", "FuelRequest", new { area = "Buyer" });
                }
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Delete(int id)
        {
            using (var tracer = new Tracer("FuelRequestController", "Delete"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().DeleteFuelRequestAsync(id);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                if (response.StatusCode == Status.Failed)
                {
                    return RedirectToAction("Edit", "FuelRequest", new { area = "Buyer", id = id });
                }
                else
                {
                    return RedirectToAction("View", "FuelRequest", new { area = "Buyer" });
                }
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> CloneRequest(int id, int? JobId = null, bool IsRetailJob = false, int? StatusId = 0)
        {
            using (var tracer = new Tracer("FuelRequestController", "CloneRequest"))
            {
                if (IsRetailJob && (StatusId == (int)FuelRequestStatus.Open || StatusId == (int)FuelRequestStatus.Accepted || StatusId == (int)FuelRequestStatus.CounterOfferAccepted))
                {
                    Status status = Status.Failed;
                    DisplayCustomMessages((MessageType)status, Resource.warningMessageFuelTypeAlreadyExist);
                    return RedirectToAction("Details", "FuelRequest", new { area = "Buyer", id = id });
                }

                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetCloneRequestAsync(id);


                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageGetCloneFuelRequestFailed);
                }
                return View(response);


            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> CloneRequest(CloneRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "CloneRequest(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SaveCloneFuelRequestAsync(viewModel, CurrentUser.Id);
                    //this will redirect to Job details page when clone fuel request is requested from Job Details
                    if (IsReturnUrlExist())
                    {
                        return Redirect(GetReturnUrl());
                    }

                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("Details", "FuelRequest", new { area = "Buyer", id = viewModel.Id });
                    }
                    else
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageCloneFuelRequestFailed);
                    }
                }

                return View(viewModel);
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
        public ActionResult PartialCounterOfferGrid(int fuelRequestId = 0, FuelRequestFilterViewModel frFilter = null)
        {
            using (var tracer = new Tracer("FuelRequestController", "PartialCounterOfferGrid"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<FuelRequestDomain>().GetBuyerCounterOfferGridAsync(UserContext, fuelRequestId, frFilter)).Result;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult PartialMapView()
        {
            using (var tracer = new Tracer("FuelRequestController", "PartialMapView"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<FuelRequestDomain>().GetMap(CurrentUser.Id)).Result;
                return PartialView("_PartialMapView", response);
            }
        }

        [HttpGet]
        public ActionResult DifferentFuelPrice()
        {
            return PartialView("_PartialDifferentFuelPrice", new DifferentFuelPriceViewModel());
        }

        [HttpGet]
        public ActionResult ResaleDifferentFuelPrice()
        {
            return PartialView("_PartialResaleDifferentFuelPrice", new DifferentFuelPriceViewModel());
        }

        [HttpGet]
        public ActionResult ResaleFee(Currency currency)
        {
            var resaleModel = new FuelRequestResaleFeeViewModel();
            resaleModel.Currency = currency;
            return PartialView("_PartialResaleFee", resaleModel);
        }

        [HttpGet]
        public ActionResult ResaleCustomer()
        {
            return PartialView("_PartialResaleCustomer", new FuelRequestResaleCustomerViewModel());
        }

        [HttpGet]
        public ActionResult DeliveryFeeByQuantity(string prefix, Currency currency, UoM uoM)
        {
            var deliveryFeeByQuantityModel = new DeliveryFeeByQuantityViewModel();
            deliveryFeeByQuantityModel.CollectionHtmlPrefix = prefix;
            deliveryFeeByQuantityModel.Currency = currency;
            deliveryFeeByQuantityModel.UoM = uoM;
            return PartialView("_PartialDeliveryFeeByQuantity", deliveryFeeByQuantityModel);
        }

        [HttpGet]
        public ActionResult AdditionalFee()
        {
            return PartialView("_PartialAdditionalFee", new AdditionalFeeViewModel());
        }

        [HttpGet]
        public ActionResult DeliverySchedule(int scheduleType = (int)DeliveryScheduleType.SpecificDates)
        {
            return PartialView("_PartialDeliveryScheduleFR", new DeliveryScheduleViewModel() { ScheduleType = scheduleType, CreatedBy = CurrentUser.Id });
        }

        [HttpGet]
        public ActionResult SpecialInstruction()
        {
            return PartialView("~/Views/Shared/_PartialSpecialInstruction.cshtml", new SpecialInstructionViewModel());
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
        //if user login with branded supplier company URL then user will only see only branded supplier fuel requests.
        //we exclude the other fuel requests
        [HttpGet]
        public JsonResult GetPrivateSupplierList()
        {
            using (var tracer = new Tracer("FuelRequestController", "GetPrivateSupplierList"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetPrivateSupplierList(CurrentUser.CompanyId, CurrentUser.BrandedCompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddPrivateSupplierList(string name, List<int> suppliers)
        {
            using (var tracer = new Tracer("FuelRequestController", "AddPrivateSupplierList"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SavePrivateSupplierListAsync(CurrentUser.CompanyId, name, suppliers, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CommonOtherFeeTypes(Currency currency, UoM uoM, bool isConstraintFee = false, bool isCommonFee = false, int truckLoadType = (int)TruckLoadTypes.LessTruckLoad)
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

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> CreateNomination(int jobId = 0, int fuelTypeId = 0, int fuelDisplayGroupId = (int)ProductDisplayGroups.MarineFuelType,
            int truckLoadTypeId = 0, int pricingSourceId = (int)PricingSource.Axxis, int pricingTypeId = 0, string pricingCode = null, int pricingCodeId = 0, string pricingCodeDesc = null)
        {
            using (var tracer = new Tracer("FuelRequestController", "CreateNomination"))
            {
                var response = new FuelRequestViewModel();

                response.FuelDeliveryDetails.TruckLoadTypes = TruckLoadTypes.FullTruckLoad;
                if (response.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity.Count == 0)
                {
                    response.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity.Add(new DeliveryFeeByQuantityViewModel());
                }

                if (response.FuelRequestResale.ResaleCustomer.Count == 0)
                {
                    response.FuelRequestResale.ResaleCustomer.Add(new FuelRequestResaleCustomerViewModel());
                }

                if (jobId == 0)
                {
                    RemoveReturnUrl();
                    jobId = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetDefaultNominationJobDetails(CurrentUser.CompanyId);
                }

                if (jobId > 0)
                {
                    response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetLastFuelRequestAsync(jobId, CurrentUser.CompanyId,
                        CurrentUser.Id, fuelTypeId, fuelDisplayGroupId, truckLoadTypeId, pricingSourceId, pricingTypeId, pricingCode, pricingCodeId, pricingCodeDesc, true);
                }

                response.CompanyId = CurrentUser.CompanyId;
                response.FuelDetails.FuelDisplayGroupId = (int)ProductDisplayGroups.MarineFuelType;
                response.FuelDetails.IsMarineLocation = true;
                response.Job.IsMarineLocation = true;

                // use existing FR page, as we are allowing market based pricing for MFN
                return View("Create", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> SaveNomination(FuelRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("FuelRequestController", "SaveNomination"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.FuelDetails.UpdatedBy = CurrentUser.Id;
                    viewModel.FuelOfferDetails.PrivateSupplierList.AddedById = CurrentUser.Id;
                    viewModel.FuelOfferDetails.PrivateSupplierList.CompanyId = CurrentUser.CompanyId;
                    viewModel.CompanyId = CurrentUser.CompanyId;
                    viewModel.FuelDetails.StatusId = (int)FuelRequestStatus.Open;

                    StatusViewModel response;
                    if (viewModel.Id > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().UpdateFuelRequestAsync(viewModel, UserContext);
                    }
                    else
                    {
                        viewModel.FuelDetails.CreatedBy = CurrentUser.Id;
                        response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().SaveFuelRequestAsync(viewModel, true, viewModel.CounterOfferSupplierId, UserContext);
                    }

                    var responseText = response.StatusMessage.Replace("fuel request", "nomination").Replace("Fuel Request", "Nomination").Replace("Fuel request", "Nomination");
                    DisplayCustomMessages((MessageType)response.StatusCode, responseText);
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("Details", "FuelRequest", new { area = "Buyer", id = viewModel.Id, hideButtons = true });
                    }
                }

                return RedirectToAction("CreateNomination", "FuelRequest", new { area = "Buyer", id = viewModel.Job.JobId });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> DeleteNomination(int id)
        {
            using (var tracer = new Tracer("FuelRequestController", "DeleteNomination"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().DeleteFuelRequestAsync(id);
                var responseText = response.StatusMessage.Replace("fuel request", "nomination").Replace("Fuel Request", "Nomination").Replace("Fuel request", "Nomination");
                DisplayCustomMessages((MessageType)response.StatusCode, responseText);
                if (response.StatusCode == Status.Failed)
                {
                    return RedirectToAction("EditNomination", "FuelRequest", new { area = "Buyer", id = id });
                }
                else
                {
                    return RedirectToAction("View", "FuelRequest", new { area = "Buyer" });
                }
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> CancelNomination(int id)
        {
            using (var tracer = new Tracer("FuelRequestController", "CancelNomination"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().CancelFuelRequestAsync(id, UserContext);
                var responseText = response.StatusMessage.Replace("fuel request", "nomination").Replace("Fuel Request", "Nomination").Replace("Fuel request", "Nomination");
                DisplayCustomMessages((MessageType)response.StatusCode, responseText);

                //this will redirect to Job details page when cancel nomination is requested from Job Details
                if (IsReturnUrlExist())
                {
                    return Redirect(GetReturnUrl());
                }

                if (response.StatusCode == Status.Failed)
                {
                    return RedirectToAction("EditNomination", "FuelRequest", new { area = "Buyer", id = id });
                }
                else
                {
                    return RedirectToAction("View", "FuelRequest", new { area = "Buyer" });
                }
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> EditNomination(int id)
        {
            using (var tracer = new Tracer("FuelRequestController", "EditNomination"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestAsync(id, 0, CurrentUser.Id, CurrentUser.CompanyId);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadNominationDetailsFailed);
                }
                if (response.DisplayMode == PageDisplayMode.None)
                    return RedirectToAction("Index", "Unauthorized", new { area = "" });
                else
                    return View("CreateNomination", response);
            }
        }
    }
}