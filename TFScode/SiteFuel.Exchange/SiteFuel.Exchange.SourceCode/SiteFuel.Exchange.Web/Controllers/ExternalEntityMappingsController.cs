using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.ExternalEntityMappings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Controllers
{
    public class ExternalEntityMappingsController : BaseController
    {
        [HttpGet]
        public async Task<JsonResult> GetExternalCompanies()
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "GetExternalCompanies"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().GetExternalCompanies(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region External Customer Mapping
        [HttpGet]
        public async Task<JsonResult> GetCustomersForExternalMapping()
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "GetCustomersForExternalMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().GetCustomersForExternalMapping(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveExternalCustomerMappings(ExternalCustomerMappingViewModel viewModel)
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "SaveExternalCustomerMappings"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().SaveExternalCustomerMappings(viewModel,UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region Customer Mapping Bulk Upload
        [HttpPost]
        public async Task<JsonResult> BulkUploadCustomerMapping()
        {
            var response = new StatusViewModel();
            try
            {
                var file = Request.Files;
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var csvFile = file[0];
                    if (csvFile != null && csvFile.ContentLength > 0)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            var externalMappingsDomain = ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>();
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            response = await externalMappingsDomain.SaveBulkUploadCustomerMapping(UserContext, csvText);
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageSelectCsvFile;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEntityMappingsController", "BulkUploadCustomerMapping", ex.Message, ex);
                response.StatusCode = Status.Failed;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region External Customer Location Mappings
        [HttpGet]
        public async Task<JsonResult> GetCustomerLocationsForExternalMapping()
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "GetCustomerLocationsForExternalMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().GetCustomerLocationsForExternalMapping(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveExternalCustomerLocationMappings(ExternalCustomerLocationMappingViewModel viewModel)
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "SaveExternalCustomerLocationMappings"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().SaveExternalCustomerLocationMappings(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region Customer Mapping Bulk Upload
        [HttpPost]
        public async Task<JsonResult> BulkUploadCustomerLocationMapping()
        {
            var response = new StatusViewModel();
            try
            {
                var file = Request.Files;
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var csvFile = file[0];
                    if (csvFile != null && csvFile.ContentLength > 0)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            var externalMappingsDomain = ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>();
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            response = await externalMappingsDomain.SaveBulkUploadCustomerLocationMapping(UserContext, csvText);
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageSelectCsvFile;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEntityMappingsController", "BulkUploadCustomerLocationMapping", ex.Message, ex);
                response.StatusCode = Status.Failed;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region External Product Mappings
        [HttpGet]
        public async Task<JsonResult> GetProductsForExternalMapping()
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "GetProductsForExternalMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().GetProductsForExternalMapping(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveExternalProductMappings(ExternalProductMappingViewModel viewModel)
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "SaveExternalProductMappings"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().SaveExternalProductMappings(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region Product Mapping Bulk Upload
        [HttpPost]
        public async Task<JsonResult> BulkUploadProductMapping()
        {
            var response = new StatusViewModel();
            try
            {
                var file = Request.Files;
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var csvFile = file[0];
                    if (csvFile != null && csvFile.ContentLength > 0)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            var externalMappingsDomain = ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>();
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            response = await externalMappingsDomain.SaveBulkUploadProductMapping(UserContext, csvText);
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageSelectCsvFile;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEntityMappingsController", "BulkUploadProductMapping", ex.Message, ex);
                response.StatusCode = Status.Failed;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region External Supplier Mappings
        [HttpGet]
        public async Task<JsonResult> GetSuppliersForExternalMapping()
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "GetSuppliersForExternalMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().GetSuppliersForExternalMapping(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveExternalSupplierMappings(ExternalSupplierMappingViewModel viewModel)
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "SaveExternalSupplierMappings"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().SaveExternalSupplierMappings(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region Supplier Mapping Bulk Upload
        [HttpPost]
        public async Task<JsonResult> BulkUploadSupplierMapping()
        {
            var response = new StatusViewModel();
            try
            {
                var file = Request.Files;
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var csvFile = file[0];
                    if (csvFile != null && csvFile.ContentLength > 0)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            var externalMappingsDomain = ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>();
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            response = await externalMappingsDomain.SaveBulkUploadSupplierMapping(UserContext, csvText);
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageSelectCsvFile;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEntityMappingsController", "BulkUploadSupplierMapping", ex.Message, ex);
                response.StatusCode = Status.Failed;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region External Terminal Mappings
        [HttpGet]
        public async Task<JsonResult> GetTerminalsForExternalMapping()
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "GetTerminalsForExternalMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().GetTerminalsForExternalMapping(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveExternalTerminalMappings(ExternalTerminalMappingViewModel viewModel)
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "SaveExternalTerminalMappings"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().SaveExternalTerminalMappings(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region Terminal Mapping Bulk Upload
        [HttpPost]
        public async Task<JsonResult> BulkUploadTerminalMapping()
        {
            var response = new StatusViewModel();
            try
            {
                var file = Request.Files;
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var csvFile = file[0];
                    if (csvFile != null && csvFile.ContentLength > 0)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            var externalMappingsDomain = ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>();
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            response = await externalMappingsDomain.SaveBulkUploadTerminalMapping(UserContext, csvText);
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageSelectCsvFile;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEntityMappingsController", "BulkUploadTerminalMapping", ex.Message, ex);
                response.StatusCode = Status.Failed;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region External BulkPlant Mappings
        [HttpGet]
        public async Task<JsonResult> GetBulkPlantsForExternalMapping()
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "GetBulkPlantsForExternalMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().GetBulkPlantsForExternalMapping(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> SaveExternalBulkPlantMappings(ExternalBulkPlantMappingViewModel viewModel)
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "SaveExternalBulkPlantMappings"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().SaveExternalBulkPlantMappings(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region Bulk Plant Mapping Bulk Upload
        [HttpPost]
        public async Task<JsonResult> BulkUploadBulkPlantMapping()
        {
            var response = new StatusViewModel();
            try
            {
                var file = Request.Files;
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var csvFile = file[0];
                    if (csvFile != null && csvFile.ContentLength > 0)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            var externalMappingsDomain = ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>();
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            response = await externalMappingsDomain.SaveBulkUploadBulkPlantMapping(UserContext, csvText);
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageSelectCsvFile;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEntityMappingsController", "BulkUploadBulkPlantMapping", ex.Message, ex);
                response.StatusCode = Status.Failed;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region External Driver Mappings
        [HttpGet]
        public async Task<JsonResult> GetDriversForExternalMapping()
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "GetDriversForExternalMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().GetDriversForExternalMapping(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveExternalDriverMappings(ExternalDriverMappingViewModel viewModel)
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "SaveExternalDriverMappings"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().SaveExternalDriverMappings(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region Driver Mapping Bulk Upload
        [HttpPost]
        public async Task<JsonResult> BulkUploadDriverMapping()
        {
            var response = new StatusViewModel();
            try
            {
                var file = Request.Files;
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var csvFile = file[0];
                    if (csvFile != null && csvFile.ContentLength > 0)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            var externalMappingsDomain = ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>();
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            response = await externalMappingsDomain.SaveBulkUploadDriverMapping(UserContext, csvText);
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageSelectCsvFile;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEntityMappingsController", "BulkUploadDriverMapping", ex.Message, ex);
                response.StatusCode = Status.Failed;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region External Carrier Mappings
        [HttpGet]
        public async Task<JsonResult> GetCarriersForExternalMapping()
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "GetCarriersForExternalMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().GetCarriersForExternalMapping(UserContext);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                // return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveExternalCarrierMappings(ExternalCarrierMappingViewModel viewModel)
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "SaveExternalCarrierMappings"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().SaveExternalCarrierMappings(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region Carrier Mapping Bulk Upload
        [HttpPost]
        public async Task<JsonResult> BulkUploadCarrierMapping()
        {
            var response = new StatusViewModel();
            try
            {
                var file = Request.Files;
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var csvFile = file[0];
                    if (csvFile != null && csvFile.ContentLength > 0)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            var externalMappingsDomain = ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>();
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            response = await externalMappingsDomain.SaveBulkUploadCarrierMapping(UserContext, csvText);
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageSelectCsvFile;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEntityMappingsController", "BulkUploadCarrierMapping", ex.Message, ex);
                response.StatusCode = Status.Failed;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #endregion

        #region External Vehicle Mappings
        [HttpGet]
        public async Task<JsonResult> GetVehiclesForExternalMapping()
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "GetVechiclesForExternalMapping"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().GetVehiclesForExternalMapping(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveExternalVehicleMappings(ExternalVehicleMappingViewModel viewModel)
        {
            using (var tracer = new Tracer("ExternalEntityMappingsController", "SaveExternalVechicleMappings"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>().SaveExternalVehicleMappings(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region Vehicle Mapping Bulk Upload
        [HttpPost]
        public async Task<JsonResult> BulkUploadVehicleMapping()
        {
            var response = new StatusViewModel();
            try
            {
                var file = Request.Files;
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var csvFile = file[0];
                    if (csvFile != null && csvFile.ContentLength > 0)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            
                            var externalMappingsDomain = ContextFactory.Current.GetDomain<ExternalEnityMappingsDomain>();
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            response = await externalMappingsDomain.SaveBulkUploadVehicleMapping(UserContext, csvText);
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageSelectCsvFile;
                        }
                    }
                    
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageSelectCsvFile;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEntityMappingsController", "BulkUploadVehicleMapping", ex.Message, ex);
                response.StatusCode = Status.Failed;
            }           
                return Json(response, JsonRequestBehavior.AllowGet);          
            
        }
        #endregion

        #endregion
    }
}