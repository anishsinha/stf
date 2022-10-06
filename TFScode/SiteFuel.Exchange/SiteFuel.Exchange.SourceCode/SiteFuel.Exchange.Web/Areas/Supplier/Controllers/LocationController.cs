using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    public class LocationController : BaseController
    {
        // GET: Supplier/Location
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetTerminals(TerminalsDataTableViewModel requestModel)
        {
            var response = new List<PickupLocationDetailViewModel>();
            try
            {
                var dataTableSearchModel = new DataTableSearchModel(requestModel);
                response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetTerminalForGridAsync(requestModel.CountryId, dataTableSearchModel);
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
                        draw = requestModel.draw,
                        data = response,
                        recordsTotal = totalCount,
                        recordsFiltered = filteredCount
                    },

                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LocationController", "GetTerminals", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetBulkPlants(int countryId)
        {
            var response = new List<PickupLocationDetailViewModel>();
            try
            {
                response = await ContextFactory.Current.GetDomain<LocationDomain>().GetBulkPlantsAsync(countryId, CurrentUser.CompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LocationController", "CreateMobileInvoiceAsync", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveBulkPlantLocation(PickupLocationDetailViewModel inputModel)
        {
            var response = new StatusViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<LocationDomain>().SaveBulkPlantLocationAsync(inputModel, UserContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LocationController", "SaveBulkPlantLocation", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }
    }
}
