using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SiteFuel.Exchange.Domain
{
    public class ProductMappingDomain : BaseDomain
    {
        public ProductMappingDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ProductMappingDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<List<MappedSupplierProductViewModel>> GetSupplierProductMappingGridAsync(int companyId, int timeout = 30)
        {
            var response = new List<MappedSupplierProductViewModel>();
            using (var tracer = new Tracer("ProductMappingDomain", "GetSupplierProductMappingGridAsync"))
            {
                try
                {
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSupplierProductMappingGridAsync(companyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ProductMappingDomain", "GetSupplierProductMappingGridAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<SupplierProductsViewModel> GetSupplierProductMappingAsync(int id = 0)
        {
            using (var tracer = new Tracer("ProductMappingDomain", "GetSupplierProductMappingAsync"))
            {
                var response = new SupplierProductsViewModel();

                try
                {
                    var supplierProductsViewModel = new List<SupplierProductViewModel>();
                    var fuelTypeDetailsViewModel = new List<ProductMappingFuelTypeDetailsViewModel>();
                    var supplierMappedProduct = await Context.DataContext.SupplierMappedProductDetails.FirstOrDefaultAsync(t => t.Id == id);
                    if (supplierMappedProduct != null)
                    {
                        response.Id = id;

                        var supplierProductViewModel = new SupplierProductViewModel();
                        supplierProductViewModel.Id = id;
                        supplierProductViewModel.CompanyId = supplierMappedProduct.CompanyId;
                        supplierProductViewModel.AssignedName = supplierMappedProduct.MyProductId;
                        supplierProductViewModel.MyProductId = supplierMappedProduct.MyProductId;
                        supplierProductViewModel.BackOfficeProductId = supplierMappedProduct.BackOfficeProductId;
                        supplierProductViewModel.DriverProductId = supplierMappedProduct.DriverProductId;
                        supplierProductViewModel.AssignCompanyId = supplierMappedProduct.CompanyId;
                        supplierProductViewModel.TerminalId = supplierMappedProduct.TerminalId.HasValue ? supplierMappedProduct.TerminalId.Value :0 ;
                        supplierProductViewModel.FuelTypeId = supplierMappedProduct.FuelTypeId;
                        supplierProductViewModel.DisplayMode = PageDisplayMode.Edit;

                        //var mappedFuelTypeDetails = Context.DataContext.SupplierMappedProductXFuelTypes.Where(t => t.SupplierMappedProductDetailId == id).ToList();
                        //foreach (var fuelType in mappedFuelTypeDetails)
                        //{
                        //    var fuelTypeDetails = new ProductMappingFuelTypeDetailsViewModel();
                        //    fuelTypeDetails.FuelTypeId = fuelType.TfxProductId;
                        //    fuelTypeDetails.BackOfficeProductCode = fuelType.BackOfficeProductCode;
                        //    fuelTypeDetails.SeaboardProductCode = fuelType.SeaBoardProductCode;
                        //    fuelTypeDetails.CompanyId = supplierMappedProduct.CompanyId;
                        //    fuelTypeDetails.Id = fuelType.Id;

                        //    fuelTypeDetailsViewModel.Add(fuelTypeDetails);
                        //}

                        supplierProductViewModel.ProductMappingFuelTypeDetailsViewModels = fuelTypeDetailsViewModel;
                        supplierProductsViewModel.Add(supplierProductViewModel);


                        response.SupplierProducts = supplierProductsViewModel;
                        response.DisplayMode = PageDisplayMode.Edit;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ProductMappingDomain", "GetSupplierProductMappingAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<StatusViewModel> SaveSupplierProductMapping(SupplierProductsViewModel productViewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("ProductMappingDomain", "SaveSupplierProductMapping"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (productViewModel.Id == 0)
                        {
                            foreach (var productDetails in productViewModel.SupplierProducts)
                            {
                                var supplierProductDetails = new SupplierMappedProductDetails()
                                {
                                    MyProductId = productDetails.AssignedName,
                                    BackOfficeProductId = productDetails.BackOfficeProductId,
                                    DriverProductId = productDetails.DriverProductId,
                                    CompanyId = userContext.CompanyId,
                                    TerminalId = productDetails.TerminalId,
                                    IsActive = true,
                                    CreatedBy = userContext.Id,
                                    CreatedDate = DateTimeOffset.Now,
                                };

                                Context.DataContext.SupplierMappedProductDetails.Add(supplierProductDetails);
                                await Context.CommitAsync();
                                productDetails.Id = supplierProductDetails.Id;

                                //if (productDetails.ProductMappingFuelTypeDetailsViewModels?.Count > 0)
                                //{
                                //    foreach (var fuelTypeDetailsViewModel in productDetails.ProductMappingFuelTypeDetailsViewModels)
                                //    {
                                //        var mappedFuelType = new SupplierMappedProductXFuelType()
                                //        {
                                //            TfxProductId = fuelTypeDetailsViewModel.FuelTypeId,
                                //            SupplierMappedProductDetailId = productDetails.Id,
                                //            BackOfficeProductCode = fuelTypeDetailsViewModel.BackOfficeProductCode,
                                //            SeaBoardProductCode = fuelTypeDetailsViewModel.SeaboardProductCode,
                                //        };

                                //        Context.DataContext.SupplierMappedProductXFuelTypes.Add(mappedFuelType);
                                //        await Context.CommitAsync();
                                //    }
                                //}
                            }

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageAddedProductMapping;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageCreateProductMapping;
                        LogManager.Logger.WriteException("ProductMappingDomain", "SaveSupplierProductMapping", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StatusViewModel> UpdateSupplierProductMapping(SupplierProductsViewModel productViewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("ProductMappingDomain", "UpdateSupplierProductMapping"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (productViewModel?.Id > 0)
                        {
                            var supplierProductDetails = await Context.DataContext.SupplierMappedProductDetails.FirstOrDefaultAsync(t => t.IsActive && t.Id == productViewModel.Id);
                            foreach (var productDetails in productViewModel.SupplierProducts)
                            {
                                if (supplierProductDetails != null)
                                {
                                    // update product details.
                                    await UpdateMappedProductDetails(userContext, supplierProductDetails, productDetails);

                                    // Update fuel type details.
                                    //await UpdateMappedProductFuelType(productViewModel, supplierProductDetails, productDetails);
                                }
                            }

                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageUpdateProductMapping;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageUpdateProductMapping;
                        LogManager.Logger.WriteException("ProductMappingDomain", "UpdateSupplierProductMapping", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        private async Task UpdateMappedProductDetails(UserContext userContext, SupplierMappedProductDetails supplierProductDetails, SupplierProductViewModel productDetails)
        {
            supplierProductDetails.MyProductId = productDetails.AssignedName;
            supplierProductDetails.BackOfficeProductId = productDetails.BackOfficeProductId;
            supplierProductDetails.DriverProductId = productDetails.DriverProductId;
            supplierProductDetails.IsActive = true;
            supplierProductDetails.UpdatedBy = userContext.Id;
            supplierProductDetails.UpdatedDate = DateTimeOffset.Now;

            Context.DataContext.Entry(supplierProductDetails).State = EntityState.Modified;
            await Context.CommitAsync();
        }

        private async Task UpdateMappedProductFuelType(SupplierProductsViewModel productViewModel, SupplierMappedProductDetails supplierProductDetails, SupplierProductViewModel productDetails)
        {
            //var supplierMappedFuelTypes = await Context.DataContext.SupplierMappedProductXFuelTypes.Where(x => x.SupplierMappedProductDetailId == productViewModel.Id).ToListAsync();
            //if (supplierMappedFuelTypes != null)
            //{
            //    Context.DataContext.SupplierMappedProductXFuelTypes.RemoveRange(supplierMappedFuelTypes);
            //    await Context.CommitAsync();

            //    if (productDetails.ProductMappingFuelTypeDetailsViewModels?.Count > 0)
            //    {
            //        foreach (var fuelTypeViewModel in productDetails.ProductMappingFuelTypeDetailsViewModels)
            //        {
            //            var mappedFuelType = new SupplierMappedProductXFuelType()
            //            {
            //                TfxProductId = fuelTypeViewModel.FuelTypeId,
            //                SupplierMappedProductDetailId = supplierProductDetails.Id,
            //                BackOfficeProductCode = fuelTypeViewModel.BackOfficeProductCode,
            //                SeaBoardProductCode = fuelTypeViewModel.SeaboardProductCode,
            //            };

            //            Context.DataContext.SupplierMappedProductXFuelTypes.Add(mappedFuelType);
            //            await Context.CommitAsync();
            //        }
            //    }
            //}
        }

        public async Task<StatusViewModel> SaveProductMapping(ProductMappingViewModel productMapping, UserContext userContext)
        {
            var response = new StatusViewModel();
            if (ValidateProductMappingModel(productMapping, response))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        // save mapping
                        if (productMapping.Id == 0)
                        {
                            await SaveMapping(productMapping, userContext);

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageAddedProductMapping;
                        }
                        else if (productMapping.Id > 0)
                        {
                            // edit mapping
                            await UpdateMapping(productMapping, userContext);
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageUpdateProductMapping;
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageUpdateProductMapping;
                        LogManager.Logger.WriteException("ProductMappingDomain", "SaveProductMapping", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        private async Task UpdateMapping(ProductMappingViewModel productMapping, UserContext userContext, int? terminalId = 0)
        {
            // inactive existing mapping 
            var existingMapping = await Context.DataContext.SupplierMappedProductDetails.FirstOrDefaultAsync(t => t.Id == productMapping.Id && t.IsActive);

            if(productMapping.IsBulkUploadRequest)
            {
                if (string.IsNullOrWhiteSpace(existingMapping.MyProductId))
                    existingMapping.MyProductId = FormatEmptyValue(productMapping.MyProductId);
                if (string.IsNullOrWhiteSpace(existingMapping.BackOfficeProductId))
                    existingMapping.BackOfficeProductId = FormatEmptyValue(productMapping.BackOfficeProductId);
                if (string.IsNullOrWhiteSpace(existingMapping.DriverProductId))
                    existingMapping.DriverProductId = FormatEmptyValue(productMapping.DriverProductId);
                //if (string.IsNullOrWhiteSpace(existingMapping.TerminalItemCode))
                //    existingMapping.DriverProductId = FormatEmptyValue(productMapping.TerminalItemCode);
            }
            else
            {
                existingMapping.MyProductId = FormatEmptyValue(productMapping.MyProductId);
                existingMapping.BackOfficeProductId = FormatEmptyValue(productMapping.BackOfficeProductId);
                existingMapping.DriverProductId = FormatEmptyValue(productMapping.DriverProductId);
              //  existingMapping.TerminalItemCode = FormatEmptyValue(productMapping.TerminalItemCode);
            }
            
            existingMapping.FuelTypeId = productMapping.FuelTypes.First().Id;

            if (terminalId > 0)
                existingMapping.TerminalId = terminalId;

            existingMapping.CompanyId = productMapping.CompanyId;
            existingMapping.UpdatedBy = userContext.Id;
            existingMapping.UpdatedDate = DateTimeOffset.Now;

            Context.DataContext.Entry(existingMapping).State = EntityState.Modified;
            await Context.CommitAsync();
        }

        private string FormatEmptyValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private async Task SaveMapping(ProductMappingViewModel productMapping, UserContext userContext)
        {
            productMapping.IsActive = true;
            productMapping.CreatedBy = userContext.Id;
            productMapping.CreatedDate = DateTimeOffset.Now;
            productMapping.UpdatedBy = userContext.Id;
            productMapping.UpdatedDate = DateTimeOffset.Now;
            var fuelTypeId = productMapping.FuelTypes.First().Id;
            int mappingId = 0;

            if (productMapping.Terminals !=null && productMapping.Terminals.Any())
            {
                foreach (var terminal in productMapping.Terminals)
                {
                    var terminalId = terminal.Id;

                    // check mappping already exists
                    var isExists = IsMappingAlreadyExists(productMapping.CompanyId, terminalId, fuelTypeId, out mappingId);
                    if (isExists)
                    {
                        productMapping.Id = mappingId;
                        await UpdateMapping(productMapping, userContext, terminalId);
                    }
                    else
                    {
                        var supplierProductDetails = productMapping.ToEntity(terminalId, fuelTypeId);
                        Context.DataContext.SupplierMappedProductDetails.Add(supplierProductDetails);
                        await Context.CommitAsync();
                    }
                }
            }
            else
            {
                var isExists = IsMappingAlreadyExists(productMapping.CompanyId, null, fuelTypeId, out mappingId);
                if (isExists)
                {
                    productMapping.Id = mappingId;
                    await UpdateMapping(productMapping, userContext, null);
                }
                else
                {
                    var supplierProductDetails = productMapping.ToEntity(null, fuelTypeId);
                    Context.DataContext.SupplierMappedProductDetails.Add(supplierProductDetails);
                    await Context.CommitAsync();
                }
            }
            
        }

        private bool IsMappingAlreadyExists(int companyId, int? terminalId, int fuelTypeId, out int mappingId)
        {
            var response = false;
            mappingId = 0;
            try
            {
                // check mapping already exists
                var existingMapping = Context.DataContext.SupplierMappedProductDetails
                    .Where(t => t.IsActive && t.CompanyId == companyId && t.TerminalId == terminalId && t.FuelTypeId == fuelTypeId)
                    .FirstOrDefault();
                if (existingMapping != null)
                {
                    mappingId = existingMapping.Id;
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ProductMappingDomain", "IsMappingAlreadyExists", ex.Message, ex);
            }

            return response;
        }

        private bool ValidateProductMappingModel(ProductMappingViewModel model, StatusViewModel status)
        {
            var response = false;
            if (model == null)
            {
                status.StatusMessage = Resource.errMessageInvalidData;
                return response;
            }
            if (model.CompanyId <= 0)
            {
                status.StatusMessage = Resource.errorMessageInvalidCompany;
                return response;
            }
            if (string.IsNullOrWhiteSpace(model.MyProductId) && string.IsNullOrWhiteSpace(model.BackOfficeProductId) && string.IsNullOrWhiteSpace(model.DriverProductId) /*&& string.IsNullOrWhiteSpace(model.TerminalItemCode)*/)
            {
                status.StatusMessage = Resource.errorMessageProductId;
                return response;
            }

            //if (model.Terminals == null || model.Terminals?.Count == 0)
            //{
            //    status.StatusMessage = Resource.errMessageSelectTerminals;
            //    return response;
            //}

            if (model.FuelTypes == null || model.FuelTypes?.Count == 0)
            {
                status.StatusMessage = Resource.errMessageSelectFuelType;
                return response;
            }

            return response = true;
        }

        public async Task<List<ProductMappingGridViewModel>> GetProductMappingDetailsAsync(ProductMappingViewModel model)
        {
            var response = new List<ProductMappingGridViewModel>();
            using (var tracer = new Tracer("ProductMappingDomain", "GetProductMappingDetailsAsync"))
            {
                try
                {
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetProductMappingDetailsAsync(model.CompanyId, model.StateIds, model.CityIds, model.TerminalIds, model.FuelTypeIds, model.CountryId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ProductMappingDomain", "GetProductMappingDetailsAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> DeleteProductMappingById(int mappingId, int companyId, UserContext userContext)
        {
            using (var tracer = new Tracer("ProductMappingDomain", "DeleteProductMappingById"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var existingMapping = await Context.DataContext.SupplierMappedProductDetails.FirstOrDefaultAsync(t => t.Id == mappingId && t.CompanyId == companyId && t.IsActive);
                    if (existingMapping != null)
                    {
                        existingMapping.IsActive = false;
                        existingMapping.UpdatedBy = userContext.Id;
                        existingMapping.UpdatedDate = DateTimeOffset.Now;

                        Context.DataContext.Entry(existingMapping).State = EntityState.Modified;
                        await Context.CommitAsync();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageProductMappingDeleted;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageProductMappingNotFound;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errorMessageFailedToDeleteProductMapping;
                    LogManager.Logger.WriteException("ProductMappingDomain", "DeleteProductMappingById", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<StatusViewModel> UpdateProductNames(List<ProductMappingGridViewModel> model, UserContext userContext)
        {
            using (var tracer = new Tracer("ProductMappingDomain", "UpdateProductNames"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    foreach (var mapping in model)
                    {
                        var existingMapping = await Context.DataContext.SupplierMappedProductDetails.FirstOrDefaultAsync(t => t.Id == mapping.Id && t.CompanyId == mapping.CompanyId && t.IsActive);
                        if (existingMapping != null)
                        {
                            existingMapping.MyProductId = mapping.MyProductId;
                            existingMapping.BackOfficeProductId = mapping.BackOfficeProductId;
                            existingMapping.DriverProductId = mapping.DriverProductId;
                          //  existingMapping.TerminalItemCode = mapping.TerminalItemCode;

                            if((string.IsNullOrWhiteSpace(existingMapping.MyProductId) || existingMapping.MyProductId == Resource.lblHyphen) &&
                               (string.IsNullOrWhiteSpace(existingMapping.BackOfficeProductId) || existingMapping.BackOfficeProductId == Resource.lblHyphen) &&
                               (string.IsNullOrWhiteSpace(existingMapping.DriverProductId) || existingMapping.DriverProductId == Resource.lblHyphen))                               
                              // &&
                              //(string.IsNullOrWhiteSpace(existingMapping.TerminalItemCode) || existingMapping.TerminalItemCode == Resource.lblHyphen))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errorMessageProductNamesValidation;
                                return response;
                            }

                            existingMapping.UpdatedBy = userContext.Id;
                            existingMapping.UpdatedDate = DateTimeOffset.Now;

                            Context.DataContext.Entry(existingMapping).State = EntityState.Modified;
                            await Context.CommitAsync();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageUpdatedProductTypeMapping;
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errorMessageProductMappingNotFound;
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errorMessageFailedToDeleteProductMapping;
                    LogManager.Logger.WriteException("ProductMappingDomain", "UpdateProductNames", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<StatusViewModel> UploadProductMappingFileToBlob(UserContext userContext, Stream fileStream, string fileName)
        {
            using (var tracer = new Tracer("ProductMappingDomain", "UploadProductMappingFileToBlob"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var azureBlob = new AzureBlobStorage();

                    var filePath = await azureBlob.SaveBlobAsync(fileStream, GenerateFileName(userContext.CompanyId, userContext.Id), BlobContainerType.ProductMappingBulkUpload.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        var queueDomain = new QueueMessageDomain();
                        var queueRequest = GetQueueEventForProductMappingFileUpload(userContext, filePath);
                        var queueId = queueDomain.EnqeueMessage(queueRequest);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMessageProductMappingBulkWithRequestNo, string.Concat(Constants.SFXProductMappingBulkUploadSuffix, queueId.ToString("000")));
                    }
                    else
                        response.StatusMessage = Resource.errMessageErrorInAzureServer;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    LogManager.Logger.WriteException("ProductMappingDomain", "UploadProductMappingFileToBlob", ex.Message, ex);
                }
                return response;
            }
        }

        private QueueMessageViewModel GetQueueEventForProductMappingFileUpload(UserContext userContext, string blobStoragePath)
        {
            var jsonViewModel = new ProductMappingBulkUploadModel();
            jsonViewModel.UserId = userContext.Id;
            jsonViewModel.CompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.ProductMappingBulkUpload,
                JsonMessage = json
            };
        }

        private string GenerateFileName(int companyId, int userId)
        {
            return string.Concat(values: Constants.ProductMappingBulk + Resource.lblSingleHyphen + companyId + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
        }

        public async Task<List<string>> ProcessProductMappingBulkUploadFile(ProductMappingBulkUploadModel bulkUploadViewModel)
        {
            List<string> errorInfo = new List<string>();
            using (var tracer = new Tracer("ProductMappingDomain", "ProcessProductMappingBulkUploadFile"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    if (!string.IsNullOrWhiteSpace(bulkUploadViewModel.FileUploadPath))
                    {
                        //processing actual bulk file
                        var azureBlob = new AzureBlobStorage();
                        var fileStream = azureBlob.DownloadBlob(bulkUploadViewModel.FileUploadPath, BlobContainerType.ProductMappingBulkUpload.ToString().ToLower());
                        if (fileStream != null)
                        {
                            using (var reader = new StreamReader(fileStream))
                            {
                                using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                                {
                                    var csvMappingList = new List<ProductMappingBulkCsvViewModel>();
                                    csv.Configuration.RegisterClassMap<ProductMappingBulkCsvViewModelMap>();
                                    try
                                    {
                                        csvMappingList = csv.GetRecords<ProductMappingBulkCsvViewModel>().ToList();
                                    }
                                    catch (Exception)
                                    {
                                        csvMappingList = new List<ProductMappingBulkCsvViewModel>();
                                    }

                                    if (csvMappingList != null && csvMappingList.Any())
                                    {
                                        var mappingListToProcess = csvMappingList.Where(t => (t.MyProductId != null && t.MyProductId?.Trim() != "") ||
                                                                           (t.BackOfficeProductId != null && t.BackOfficeProductId?.Trim() != "") ||
                                                                           (t.DriverProductId != null && t.DriverProductId?.Trim() != "") 
                                                                           //||(t.TerminalItemCode != null && t.TerminalItemCode?.Trim() != "")
                                                                     ).ToList();
                                        if (mappingListToProcess.Any())
                                        {
                                            List<ProductMappingViewModel> productMappingListToSave = new List<ProductMappingViewModel>();
                                            var rowIndex = 0;
                                            foreach (var item in mappingListToProcess)
                                            {
                                                var isDetailsFound = true;
                                                var errorMessage = "{0} details not found. StateCode => " + item.StateCode + ", City => " + item.City + ", TerminalName => " + item.TerminalName + ", ProductName => " + item.ProductName + ". ";
                                                item.CountryCode = item.CountryCode?.Trim();
                                                item.StateCode = item.StateCode?.Trim();
                                                item.City = item.City?.Trim();
                                                item.TerminalName = item.TerminalName?.Trim();
                                                item.ProductName = item.ProductName?.Trim();
                                                item.MyProductId = item.MyProductId?.Trim();
                                                item.BackOfficeProductId = item.BackOfficeProductId?.Trim();
                                                item.DriverProductId = item.DriverProductId?.Trim();
                                                //  item.TerminalItemCode = item.TerminalItemCode?.Trim();

                                                var stateId = await Context.DataContext.MstStates.Where(t => item.StateCode != null && t.Code.ToLower() == item.StateCode.ToLower() && t.IsActive)
                                                                                          .Select(t => t.Id)
                                                                                          .FirstOrDefaultAsync();

                                                var terminalId = await Context.DataContext.MstExternalTerminals.Where(t => item.TerminalName != null && t.Name.ToLower() == item.TerminalName.ToLower() && t.StateId == stateId && t.City == item.City && t.CountryCode.ToLower() == item.CountryCode.ToLower() && t.IsActive)
                                                                                           .OrderByDescending(t => t.Id)
                                                                                           .Select(t => t.Id)
                                                                                           .FirstOrDefaultAsync();
                                              //  

                                              
                                                if (stateId == 0 && terminalId > 0)
                                                {
                                                    errorInfo.Add(string.Format(errorMessage, "State"));
                                                    isDetailsFound = false;
                                                }

                                                var city = await Context.DataContext.MstExternalTerminals.Where(t => item.City != null && t.City.ToLower() == item.City.ToLower() && t.StateId == stateId && t.IsActive)
                                                                                            .Select(t => t.City)
                                                                                            .FirstOrDefaultAsync();
                                                if (city == null && terminalId > 0)
                                                {
                                                    errorInfo.Add(string.Format(errorMessage, "City"));
                                                    isDetailsFound = false;
                                                }

                                                int? termId = terminalId;
                                                if (terminalId == 0)
                                                {
                                                    termId = null;
                                                    //errorInfo.Add(string.Format(errorMessage, "Terminal"));
                                                    //isDetailsFound = false;
                                                }

                                                var fuelTypeId = await Context.DataContext.MstTfxProducts.Where(t => item.ProductName != null && t.Name.ToLower() == item.ProductName.ToLower() && t.IsActive)
                                                                                           .OrderByDescending(t => t.Id)
                                                                                           .Select(t => t.Id)
                                                                                           .FirstOrDefaultAsync();
                                                if (fuelTypeId == 0)
                                                {
                                                    errorInfo.Add(string.Format(errorMessage, "FuelType"));
                                                    isDetailsFound = false;
                                                }

                                                if (isDetailsFound)
                                                {
                                                    item.StateId = stateId;
                                                    item.City = city;                                                   
                                                    item.TerminalId = termId;
                                                    item.FuelTypeId = fuelTypeId;
                                                    item.CompanyId = bulkUploadViewModel.CompanyId;
                                                    item.RowNumber = ++rowIndex;
                                                    var obj = item.ToViewModel();
                                                    productMappingListToSave.Add(obj);
                                                }
                                            }

                                            AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                                            var companyTypeId = await Context.DataContext.Companies.Where(t => t.Id == bulkUploadViewModel.CompanyId && t.IsActive)
                                                                                             .Select(t => t.CompanyTypeId)
                                                                                             .FirstOrDefaultAsync();
                                            var context = authenticationDomain.GetUserContextAsync(bulkUploadViewModel.UserId, (CompanyType)companyTypeId).Result;
                                            foreach (var productMapping in productMappingListToSave)
                                            {
                                                var status = await SaveProductMapping(productMapping, context);
                                                if(status.StatusCode == Status.Failed)
                                                {
                                                    var row = mappingListToProcess.FirstOrDefault(t => t.RowNumber == productMapping.RowNumber);
                                                    errorInfo.Add(status.StatusMessage + ". StateCode => " + row.StateCode + ", City => " + row.City + ", TerminalName => " + row.TerminalName + ", ProductName => " + row.ProductName + ". ");
                                                    productMapping.RowNumber = 0;
                                                }
                                            }

                                            var successMessage = string.Format(Resource.successMessageProductMappingBulkUploadProcessed, productMappingListToSave.Count, mappingListToProcess.Count);
                                            processMessage.Append(successMessage);
                                            errorInfo.Add(successMessage);
                                        }
                                        else
                                        {
                                            errorInfo.Add(Resource.errorMessageProductMappingBulkUploadEmptyRows);
                                        }
                                    }
                                    else
                                    {
                                        errorInfo.Add(Resource.errorMessageProductMappingBulkUploadEmptyRows);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("ProductMappingDomain", "ProcessProductMappingBulkUploadFile", ex.Message, ex);
                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Resource.errorMessageProcessProductMappingBulkUploadFile);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return errorInfo;
            }
        }
    }
}