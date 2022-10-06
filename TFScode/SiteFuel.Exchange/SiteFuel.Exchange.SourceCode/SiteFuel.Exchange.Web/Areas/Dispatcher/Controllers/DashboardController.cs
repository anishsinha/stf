using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Dispatcher;
using SiteFuel.Exchange.ViewModels.Job;
using SiteFuel.Exchange.Web.Areas.Dispatcher.Models;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Dispatcher.Controllers
{
    [AuthorizeCompany(CompanyType.Carrier, CompanyType.Supplier, CompanyType.SupplierAndCarrier, CompanyType.BuyerSupplierAndCarrier, CompanyType.BuyerAndSupplier)]
    public class DashboardController : BaseController
    {
        // GET: Dispatcher/Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetUserCountry()
        {
            var domain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
            string response = await domain.GetUserCountryAsync(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetJobLocationDetails(string jobList = "", string inventoryCaptureTypeIds = "")
        {
            var result = new ResponseMessage();
            var response = await ContextFactory.Current.GetDomain<DispatcherDomain>().GetJobLocationDetails(CurrentUser.CompanyId,false, jobList, inventoryCaptureTypeIds);
            if (response.jobLocationDetails.Any())
                result.StatusCode = HttpStatusCode.Found;
            else
                result.StatusCode = HttpStatusCode.NotFound;
            result.Data = response;


            return new JsonResult
            {
                Data = result,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public JsonResult GetCarriersForSupplierDashboard()
        {
            var response =  ContextFactory.Current.GetDomain<DispatcherDomain>().GetCarriersForSupplierDashboard(CurrentUser.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GetDemandCaptureChartData(DemandCaptureChartData demandCaptureChartData)
        {
            var result = new ResponseMessage();
            var response = await ContextFactory.Current.GetDomain<DispatcherDomain>().GetJobDemandCaptureChartData(demandCaptureChartData);
            if (response.Count() > 0)
                result.StatusCode = HttpStatusCode.Found;
            else
                result.StatusCode = HttpStatusCode.NotFound;
            result.Data = response.ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetRegions()
        {
            var domain = ContextFactory.Current.GetDomain<DispatcherDomain>();
            var response = await domain.GetDispatcherRegionsAsync(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetOnGoingLoads(WhereIsMyDriverInputModel input)
        {
            var domain = ContextFactory.Current.GetDomain<DispatcherDomain>();
            if (input.FromDate == DateTimeOffset.MinValue && input.ToDate == DateTimeOffset.MinValue)
            {
                input.FromDate = DateTimeOffset.Now.Date;
                input.ToDate = DateTimeOffset.Now.Date;
            }
            if (string.IsNullOrWhiteSpace(input.DriverSearch))
            {
                input.DriverSearch = string.Empty;
            }
            var response = await domain.GetOnGoingLoadsAsync(UserContext, input);
            return Json(response, JsonRequestBehavior.DenyGet);
        }       

        [HttpPost]
        public async Task<JsonResult> GetDispatcherLoads(WhereIsMyDriverInputModel input)
        {
            var domain = ContextFactory.Current.GetDomain<DispatcherDomain>();
            if (input.FromDate == DateTimeOffset.MinValue && input.ToDate == DateTimeOffset.MinValue)
            {
                input.FromDate = DateTimeOffset.Now.Date;
                input.ToDate = DateTimeOffset.Now.Date;
            }
            if (string.IsNullOrWhiteSpace(input.DriverSearch))
            {
                input.DriverSearch = string.Empty;
            }
            var dataTableSearchModel = new DataTableSearchModel(input);
            var response = await domain.GetDispatcherLoadsAsync(UserContext, input, dataTableSearchModel);
            var totalCount = 0;
            var filteredCount = 0;

            if (response.Any())
            {
                totalCount = response[0].TotalCount;
                filteredCount = response[0].FilteredCount;
            }

            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    draw = input.draw,
                    data = response,
                    recordsTotal = totalCount,
                    recordsFiltered = filteredCount
                },

                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public async Task<JsonResult> GetDriverAdditionalDetails(int driverId)
        {
            var response = await ContextFactory.Current.GetDomain<DispatcherDomain>().GetDriverAdditionalDetailsAsync(driverId,UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetCountryDetails()
        {
            
            var response = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetCountriesEx());
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetStateDetails(string countryId)
        {
            var countryIds = countryId.Split(',').Select(Int32.Parse).ToList();
            var response = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetStates(countryIds));
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetCities(string stateId)
        {
            var stateIds = stateId.Split(',').Select(Int32.Parse).ToList();
            var response = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetCities(stateIds));
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetProductTypes()
        {
            var result = new ResponseMessage();
            var response = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetProductTypes());
            if (response.Count() > 0)
                result.StatusCode = HttpStatusCode.Found;
            else
                result.StatusCode = HttpStatusCode.NotFound;
            result.Data = response;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> DDTInvoiceDetails(int invoiceHeaderId)
        {
            string viewContent = string.Empty;
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetConsolidatedInvoicePdfAsync(invoiceHeaderId, CompanyType.Supplier);
            if (response.Invoices.Any() && response.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.TankRental || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.TankRental)))
            {
                 viewContent = ConvertViewToString("_PartialTankRentalInvoicePdf", response);

            }
            else if (response.Invoices.Any() && response.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.Balance || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.Balance)))
            {
                 viewContent = ConvertViewToString("_PartialBalanceInvoicePdf", response);
            }
            else if (response.Invoices.Any() && response.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.DryRun))
            {
                 viewContent = ConvertViewToString("_PartialDryRunInvoicePdf", response);
            }
            else
            {
                 viewContent = ConvertViewToString("_PartialConsolidatedInvoicePdf", response);
            }
            return Json(viewContent, JsonRequestBehavior.AllowGet);
        }
        public string ConvertViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }
    }

}