using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Carrier.Controllers
{
    public class SelfServiceAliasController : BaseController
    {
        [AuthorizeCompany(CompanyType.Carrier, CompanyType.Supplier, CompanyType.Buyer)]
        [ActionName("View")]
        // GET: SelfServiceAlias/View
        public ActionResult Index()
        {
           var isLiftFileValidationEnabled = ContextFactory.Current.GetDomain<HelperDomain>().IsLiftFileValidationEnabled(UserContext.CompanyId).Result;
            ViewBag.IsLiftFileValidationEnabled = isLiftFileValidationEnabled;
            return View("View");
        }

        [HttpGet]
        public async Task<JsonResult> GetCountries(int companyId)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "GetCountries"))
            {
                var countryList = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetCountriesEx());
                var defaultCountry = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetDefaultServingCountry(companyId));
                var servingCountries = Helpers.CommonHelperMethods.ServingCountry(CurrentUser.CompanyId, CurrentUser.CompanyTypeId, CurrentUser.CompanySubTypeId);
                var response = new { CountryList = countryList, ServingCountries = servingCountries, DefaultCountryId = defaultCountry };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetDefaultServingCountry(int companyId)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "GetDefaultServingCountry"))
            {
                var defaultCountry = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetDefaultServingCountry(companyId));
                var response = new { DefaultCountryId = defaultCountry };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetStates(int countryId)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "GetStates"))
            {
                var response = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetTerminalStates(countryId));
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetCities(List<int> stateIds)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "GetCities"))
            {
                var response = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetTerminalCities(stateIds));
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetTerminals(ProductMappingViewModel filter)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "GetTerminals"))
            {
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSupplierTerminals(filter.CompanyId, filter.StateIds, filter.CityIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetBulkPlants(ProductMappingViewModel filter)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "GetBulkPlants"))
            {
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetBulkPlants(filter.CompanyId, filter.StateIds, filter.CityIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetTerminalsForMapping(TerminalMappingViewModel filter)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "GetTerminalsForMapping"))
            {
                filter.CompanyId =  CurrentUser.CompanyId;
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetTerminalsForMapping(filter.CompanyId,filter.CountryId, filter.StateIds, filter.CityIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetServingFuelTypesByCompany(int companyId)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "GetServingFuelTypesByCompany"))
            {
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSupplierFuelTypes(companyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveProductMapping(ProductMappingViewModel productMapping)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "SaveProductMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<ProductMappingDomain>().SaveProductMapping(productMapping, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveTerminalMapping(TerminalMappingGridViewModel terminalMapping)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "SaveProductMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<TerminalMappingDomain>().SaveTerminalMapping(terminalMapping, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetProductMappingDetails(ProductMappingViewModel model)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "GetProductMappingDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<ProductMappingDomain>().GetProductMappingDetailsAsync(model);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetTerminalMappingGrid(int SelectedCountryId)
        {
            var response = new List<TerminalMappingGridViewModel>();
            try
            {
                response = await ContextFactory.Current.GetDomain<TerminalMappingDomain>().GetTerminalMappingGrid(UserContext.CompanyId, SelectedCountryId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("LocationController", "GetTerminals", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteProductMappingById(ProductMappingGridViewModel model)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "DeleteProductMappingById"))
            {
                var response = await ContextFactory.Current.GetDomain<ProductMappingDomain>().DeleteProductMappingById(model.Id, model.CompanyId, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<JsonResult> UpdateProductNames(List<ProductMappingGridViewModel> model)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "UpdateProductNames"))
            {
                var response = await ContextFactory.Current.GetDomain<ProductMappingDomain>().UpdateProductNames(model, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<JsonResult> UpdateTerminalId(TerminalMappingGridViewModel model)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "UpdateTerminalId"))
            {
                var response = await ContextFactory.Current.GetDomain<TerminalMappingDomain>().UpdateTerminalId(model, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<JsonResult> DeleteTerminalMappingById(TerminalMappingGridViewModel model)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "DeleteTerminalMappingById"))
            {
                var response = await ContextFactory.Current.GetDomain<TerminalMappingDomain>().DeleteTerminalMappingById(model.Id, UserContext.CompanyId, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public FileResult DownloadProductMappingTemplate(long id)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "DownloadProductMappingTemplate"))
            {
                var path = Server.MapPath("~/" + Resource.ProductMappingBulkUploadFilePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = "ProductMapping_Template.csv";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CheckDuplicateTerminalId(TerminalMappingGridViewModel model)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "CheckDuplicateCustomerId"))
            {
                var response = await ContextFactory.Current.GetDomain<TerminalMappingDomain>().CheckDuplicateTerminalId(model, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UploadProductMappingTemplate(HttpPostedFileBase[] files)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("SelfServiceAliasController", "UploadProductMappingTemplate"))
            {
                var csvFile = Request.Files[0];
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (csvFile.ContentLength < Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                    {
                        
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            if (csvFile == null || csvFile.ContentLength <= 0)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageEmptyProductMappingFile;
                            }
                            else
                            {
                                var productMappingdomain = ContextFactory.Current.GetDomain<ProductMappingDomain>();
                                response = await productMappingdomain.UploadProductMappingFileToBlob(UserContext, csvFile.InputStream, csvFile.FileName);
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errFileSizeMessage;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageNoFileChosen;
                }

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetTerminalSupplierAndDesc(int countryId)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "GetTerminalSupplierAndDesc"))
            {
                var response = await ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().GetTerminalSupplierAndDesc(countryId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveTerminalItemCodeMapping(TerminalItemCodeMappingViewModel model)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "SaveTerminalItemCodeMapping"))
            {
                var mappingList = new List<TerminalItemCodeMappingViewModel>();
                mappingList.Add(model);

                var response = await ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().SaveTerminalItemCodeMapping(mappingList, UserContext.Id, UserContext.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateTerminalItemCodeMapping(TerminalItemCodeMappingViewModel model)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "UpdateTerminalItemCodeMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().UpdateTerminalItemCodeMapping(model, UserContext.Id, UserContext.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteTerminalItemCodeMapping(int id)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "DeleteTerminalItemCodeMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>().DeleteTerminalItemCodeMapping(id, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetTerminalItemCodeMappings(ItemCodeMappingGridRequestViewModel filter)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "GetTerminalItemCodeMappings"))
            {
                filter.Country = (filter.CountryId == (int)Country.USA ? Country.USA : Country.CAN);
                filter.CompanyId = UserContext.CompanyId;
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetTerminalItemCodeMappings(filter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public FileResult DownloadTerminalItemCodeMappingTemplate(long id)
        {
            using (var tracer = new Tracer("SelfServiceAliasController", "DownloadTerminalItemCodeMappingTemplate"))
            {
                var path = Server.MapPath("~/" + Resource.TerminalItemCodeMappingBulkUploadFilePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = "TerminalItemCodeMapping_Template.csv";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
        }

        [HttpPost]
        public async Task<JsonResult> BulkUploadTerminalItemCodeMappingFile(HttpPostedFileBase[] files)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("SelfServiceAliasController", "BulkUploadTerminalItemCodeMappingFile"))
            {
                var csvFile = Request.Files[0];
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (csvFile.ContentLength < Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                    {

                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            if (csvFile == null || csvFile.ContentLength <= 0)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageEmptyProductMappingFile;
                            }
                            else
                            {
                                var domain = ContextFactory.Current.GetDomain<TerminalSupplierMappingDomain>();
                                response = await domain.UploadTerminalItemCodeMappingFileToBlob(UserContext, csvFile);
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errFileSizeMessage;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageNoFileChosen;
                }

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetTerminalSuppliers(int countryId)
        {
           
               var response=  await ContextFactory.Current.GetDomain<MasterDomain>().GetTerminalSupplierList(countryId);
                return Json(response,JsonRequestBehavior.AllowGet);
                
        }

        [HttpGet]
        public async Task<JsonResult> GetAssignedTerminalIdsForMapping()
        {
            var response = await ContextFactory.Current.GetDomain<MasterDomain>().GetAssignedTerminalIdsForMapping(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveCarrierMapping(UspCarrierMapping carrierMapping)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveCarrierMapping(carrierMapping, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetCarrierIDMappings(int countryId = (int)Country.USA)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetCarrierIDMappings(UserContext, countryId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> DeleteCarrierIDMapping(int mappingId)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().DeleteCarrierIDMapping(mappingId, UserContext);
            return Json(response,JsonRequestBehavior.AllowGet);
        }
    }
}