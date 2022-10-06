using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.ExternalEntityMappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class ExternalEnityMappingsDomain : BaseDomain
    {
        public ExternalEnityMappingsDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }
        public ExternalEnityMappingsDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<DropdownDisplayItem>> GetExternalCompanies(UserContext userContext)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            try
            {
                response = await Context.DataContext.ExternalCompaniesRoleAccessMapping.Where(t => userContext.Roles.Contains(t.MstRole.Id) && (t.ThirdPartyId == (int)ExternalThirdPartyCompanies.ThirdParty || (userContext.IsSupplier || userContext.IsSupplierAdmin)))
                    .Select(t => new DropdownDisplayItem
                    {
                        Id = t.MstExternalThirdPartyCompanies.Id,
                        Name = t.MstExternalThirdPartyCompanies.CompanyName
                    }).ToListAsync();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "GetExternalCompanies", ex.Message, ex);
            }
            return response;

        }
        #region External Customer Mapping
        public async Task<List<ExternalCustomerMappingViewModel>> GetCustomersForExternalMapping(UserContext userContext)
        {
            List<ExternalCustomerMappingViewModel> externalCustomerMappings = new List<ExternalCustomerMappingViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                externalCustomerMappings = await storedProcedureDomain.GetCustomersForExternalMapping(userContext.CompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "GetCustomersForExternalMapping", ex.Message, ex);
            }
            return externalCustomerMappings;

        }

        public async Task<StatusViewModel> SaveExternalCustomerMappings(ExternalCustomerMappingViewModel viewModel, UserContext usercontext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var customerDetails = await Context.DataContext.ExternalCustomerMappings.Where(t => t.Id == viewModel.Id).FirstOrDefaultAsync();
                if (customerDetails != null && customerDetails.Id > 0)
                {
                    customerDetails.TargetCustomerValue = viewModel.TargetCustomerValue;
                    customerDetails.UpdatedDate = DateTimeOffset.Now;
                    customerDetails.UpdatedBy = usercontext.Id;
                    Context.DataContext.Entry(customerDetails).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                else
                {
                    customerDetails = new ExternalCustomerMappings();
                    customerDetails = viewModel.ToEntity();
                    customerDetails.CreatedBy = usercontext.Id;
                    customerDetails.CreatedByCompanyId = usercontext.CompanyId;
                    Context.DataContext.ExternalCustomerMappings.Add(customerDetails);
                    await Context.CommitAsync();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMsgCustomerExternalMapping;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveExternalCustomerMappings", ex.Message, ex);
            }
            return response;
        }
        #region Customermapping Bulk Upload
        public async Task<StatusViewModel> SaveBulkUploadCustomerMapping(UserContext userContext, string csvText)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "SaveBulkUploadCustomerMapping"))
            {
                try
                {
                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                    var engine = new FileHelperEngine<ExternalCustomerMappingCsvViewModel>();
                    var csvCustomerList = engine.ReadString(csvText).ToList();

                    response = ValidateCustomerMappingFileAsync(userContext, csvCustomerList);
                    if (response.StatusCode == Status.Success)
                    {
                        var listExternalCustomers = JsonConvert.DeserializeObject<List<ExternalCustomerMappingViewModel>>(JsonConvert.SerializeObject(csvCustomerList));
                        GetCustomersId(listExternalCustomers, userContext);

                        var customerIds = listExternalCustomers.Select(t => t.CustomerId).ToList();
                        var listExternalCustomersForUpdate = Context.DataContext.ExternalCustomerMappings.Where(t => customerIds.Contains(t.CustomerId) && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive).ToList();
                        var customerIdsToUpdate = new List<int>();
                        if (listExternalCustomersForUpdate != null && listExternalCustomersForUpdate.Any())
                        {
                            customerIdsToUpdate = listExternalCustomersForUpdate.Select(t => t.CustomerId).ToList();
                            foreach (var customerDetails in listExternalCustomersForUpdate)
                            {
                                customerDetails.TargetCustomerValue = listExternalCustomers.Where(t => t.CustomerId == customerDetails.CustomerId).Select(t => t.TargetCustomerValue).FirstOrDefault();
                                customerDetails.UpdatedDate = DateTimeOffset.Now;
                                customerDetails.UpdatedBy = userContext.Id;
                                Context.DataContext.Entry(customerDetails).State = EntityState.Modified;
                            }
                            await Context.CommitAsync();
                        }

                        var listExternalCustomerMappings = new List<ExternalCustomerMappings>();
                        var listExternalCustomersForInsert = listExternalCustomers.Where(t => !customerIdsToUpdate.Contains(t.CustomerId)).ToList();
                        if (listExternalCustomersForInsert != null && listExternalCustomersForInsert.Any())
                        {
                            foreach (var item in listExternalCustomersForInsert)
                            {

                                ExternalCustomerMappings externalCustomerMappings = new ExternalCustomerMappings();
                                item.ThirdPartyId = (int)ExternalThirdPartyCompanies.PDI;
                                externalCustomerMappings = item.ToEntity();
                                externalCustomerMappings.CreatedBy = userContext.Id;
                                externalCustomerMappings.CreatedByCompanyId = userContext.CompanyId;
                                listExternalCustomerMappings.Add(externalCustomerMappings);
                            }

                            if (listExternalCustomerMappings != null && listExternalCustomerMappings.Any())
                            {
                                Context.DataContext.ExternalCustomerMappings.AddRange(listExternalCustomerMappings);
                                await Context.CommitAsync();
                            }
                        }
                        response.StatusMessage = Resource.successMsgCustomerMappingBulkUpload;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveBulkUploadCustomerMapping", ex.Message, ex);
                }
                return response;
            }
        }
        public void GetCustomersId(List<ExternalCustomerMappingViewModel> externalCustomers, UserContext userContext)
        {
            //Get customer Id of entered customers
           // var externalCustomerNames = externalCustomers.Select(t => t.CustomerName).Distinct().ToList();
            var externalCustomerNames = externalCustomers.Where(t => t.CustomerName != null && t.CustomerName != "").Select(t => t.CustomerName.Trim().ToLower()).Distinct().ToList();
            var customers = Context.DataContext.Companies.Where(t => externalCustomerNames.Contains(t.Name.Trim().ToLower())).Select(t => new { Id = t.Id, Name = t.Name }).ToList();
            foreach (var item in externalCustomers)
            {
                item.CustomerId = customers.Where(t => t.Name.Trim().ToLower() == item.CustomerName.Trim().ToLower()).Select(t => t.Id).FirstOrDefault();
            }
        }

        public StatusViewModel ValidateCustomerMappingFileAsync(UserContext userContext, List<ExternalCustomerMappingCsvViewModel> csvCustomerList)
        {
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "ValidateCustomerMappingFileAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    StringBuilder errorList = new StringBuilder();
                    if (csvCustomerList != null && csvCustomerList.Any())
                    {
                        ValidateCustomerBaseRow(csvCustomerList, errorList);
                        if (errorList.Length <= 0)
                        {
                            ValidateCustomerDuplicateData(csvCustomerList, errorList);
                        }
                        if (errorList.Length <= 0)
                        {
                            ValidateCustomerInvalidRecords(userContext, csvCustomerList, errorList);
                        }
                        if (errorList.Length > 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = errorList.ToString();
                            if (response.StatusMessage.Length > 1000)
                            {
                                response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "No Records found in csv.";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "ValidateCustomerMappingFileAsync", ex.Message, ex);
                }
                return response;
            }
        }
        private static void ValidateCustomerDuplicateData(List<ExternalCustomerMappingCsvViewModel> csvCustomerList, StringBuilder errorList)
        {
            var duplicateCustomers = csvCustomerList.GroupBy(x => x.CustomerName)
                                                 .Where(item => item.Count() > 1)
                                                 .Select(y => y.Key)
                                                 .ToList();
            if (duplicateCustomers != null && duplicateCustomers.Any())
            {
                foreach (var customer in duplicateCustomers)
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgInvalidDuplicateCustomerForMapping, customer));
                }
            }
        }
        private static void ValidateCustomerBaseRow(List<ExternalCustomerMappingCsvViewModel> csvCustomerList, StringBuilder errorList)
        {
            int lineNumberOfCSV = 1;
            if (csvCustomerList != null && csvCustomerList.Any())
            {
                foreach (var item in csvCustomerList)
                {
                    if (string.IsNullOrEmpty(item.CustomerName))
                    {
                        if (errorList.Length > 0)
                        {
                            errorList.Append("</br>");
                        }
                        errorList.AppendLine(string.Format(Resource.errMsgEnterCustomerNameForMapping, lineNumberOfCSV));
                    }
                    lineNumberOfCSV++;
                }
            }
        }
        public void ValidateCustomerInvalidRecords(UserContext userContext, List<ExternalCustomerMappingCsvViewModel> csvCustomerList, StringBuilder errorList)
        {
            //var customers = csvCustomerList.Select(t => t.CustomerName.Trim()).Distinct();
            var validCustomers = Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == userContext.CompanyId).Select(t => t.BuyerCompany.Name.Trim()).Distinct().ToList();
            int lineNumberOfCSV = 1;
            foreach (var item in csvCustomerList)
            {
                if (!validCustomers.Contains(item.CustomerName.Trim()))
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgInvalidCustomerForMapping, item.CustomerName, lineNumberOfCSV));
                }
                lineNumberOfCSV++;
            }
        }
        #endregion
        #endregion

        #region External Customer Location Mapping
        public async Task<List<ExternalCustomerLocationMappingViewModel>> GetCustomerLocationsForExternalMapping(UserContext userContext)
        {
            List<ExternalCustomerLocationMappingViewModel> externalCustomerLocationMappings = new List<ExternalCustomerLocationMappingViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                externalCustomerLocationMappings = await storedProcedureDomain.GetCustomerLocationsForExternalMapping(userContext.CompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "GetCustomerLocationsForExternalMapping", ex.Message, ex);
            }
            return externalCustomerLocationMappings;

        }

        public async Task<StatusViewModel> SaveExternalCustomerLocationMappings(ExternalCustomerLocationMappingViewModel viewModel, UserContext usercontext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var customerLocationDetails = await Context.DataContext.ExternalCustomerLocationMappings.Where(t => t.Id == viewModel.Id).FirstOrDefaultAsync();
                if (customerLocationDetails != null && customerLocationDetails.Id > 0)
                {
                    customerLocationDetails.TargetCustomerLocationValue = viewModel.TargetCustomerLocationValue;
                    customerLocationDetails.UpdatedDate = DateTimeOffset.Now;
                    customerLocationDetails.UpdatedBy = usercontext.Id;
                    Context.DataContext.Entry(customerLocationDetails).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                else
                {
                    customerLocationDetails = new ExternalCustomerLocationMappings();
                    customerLocationDetails = viewModel.ToEntity();
                    customerLocationDetails.CreatedBy = usercontext.Id;
                    customerLocationDetails.CreatedByCompanyId = usercontext.CompanyId;
                    Context.DataContext.ExternalCustomerLocationMappings.Add(customerLocationDetails);
                    await Context.CommitAsync();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMsgCustomerLocationExternalMapping;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveExternalCustomerLocationMappings", ex.Message, ex);
            }
            return response;
        }

        #region CustomerLocationmapping Bulk Upload
        public async Task<StatusViewModel> SaveBulkUploadCustomerLocationMapping(UserContext userContext, string csvText)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "SaveBulkUploadCustomerLocationMapping"))
            {
                try
                {
                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                    var engine = new FileHelperEngine<ExternalCustomerLocationMappingCsvViewModel>();
                    var csvCustomerLocationList = engine.ReadString(csvText).ToList();

                    response = ValidateCustomerLocationMappingFileAsync(userContext, csvCustomerLocationList);
                    if (response.StatusCode == Status.Success)
                    {
                        var listExternalLocations = JsonConvert.DeserializeObject<List<ExternalCustomerLocationMappingViewModel>>(JsonConvert.SerializeObject(csvCustomerLocationList));
                        GetLocationsId(listExternalLocations, userContext);


                        var LocationIds = listExternalLocations.Select(t => t.CustomerLocationId).ToList();
                        var listExternalLocationsForUpdate = Context.DataContext.ExternalCustomerLocationMappings.Where(t => LocationIds.Contains(t.CustomerLocationId) && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive).ToList();
                        var locationIdsToUpdate = new List<int>();
                        if (listExternalLocationsForUpdate != null && listExternalLocationsForUpdate.Any())
                        {
                            locationIdsToUpdate = listExternalLocationsForUpdate.Select(t => t.CustomerLocationId).ToList();
                            foreach (var item in listExternalLocationsForUpdate)
                            {
                                item.TargetCustomerLocationValue = listExternalLocations.Where(t => t.CustomerLocationId == item.CustomerLocationId).Select(t => t.TargetCustomerLocationValue).FirstOrDefault();
                                item.UpdatedDate = DateTimeOffset.Now;
                                item.UpdatedBy = userContext.Id;
                                Context.DataContext.Entry(item).State = EntityState.Modified;
                            }
                            await Context.CommitAsync();
                        }

                        var listExternalCustomerLocationMappings = new List<ExternalCustomerLocationMappings>();
                        var listExternalLocationsForInsert = listExternalLocations.Where(t => !locationIdsToUpdate.Contains(t.CustomerLocationId)).ToList();
                        if (listExternalLocationsForInsert != null && listExternalLocationsForInsert.Any())
                        {
                            foreach (var item in listExternalLocationsForInsert)
                            {

                                ExternalCustomerLocationMappings externalCustomerLocationMappings = new ExternalCustomerLocationMappings();
                                item.ThirdPartyId = (int)ExternalThirdPartyCompanies.PDI;
                                externalCustomerLocationMappings = item.ToEntity();
                                externalCustomerLocationMappings.CreatedBy = userContext.Id;
                                externalCustomerLocationMappings.CreatedByCompanyId = userContext.CompanyId;
                                listExternalCustomerLocationMappings.Add(externalCustomerLocationMappings);
                            }

                            if (listExternalCustomerLocationMappings != null && listExternalCustomerLocationMappings.Any())
                            {
                                Context.DataContext.ExternalCustomerLocationMappings.AddRange(listExternalCustomerLocationMappings);
                                await Context.CommitAsync();
                            }
                        }
                        response.StatusMessage = Resource.successMsgLocationMappingBulkUpload;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveBulkUploadCustomerLocationMapping", ex.Message, ex);
                }
                return response;
            }
        }
        public void GetLocationsId(List<ExternalCustomerLocationMappingViewModel> externalLocations, UserContext userContext)
        {
            //Get location Id of entered locations
            var externalLocationNames = externalLocations.Select(t => t.CustomerLocationName).Distinct().ToList();
            var locations = Context.DataContext.Jobs.Where(t => externalLocationNames.Contains(t.Name)).Select(t => new { Id = t.Id, Name = t.Name }).ToList();
            foreach (var item in externalLocations)
            {
                item.CustomerLocationId = locations.Where(t => t.Name.Trim().ToLower() == item.CustomerLocationName.Trim().ToLower()).Select(t => t.Id).FirstOrDefault();
            }
        }

        public StatusViewModel ValidateCustomerLocationMappingFileAsync(UserContext userContext, List<ExternalCustomerLocationMappingCsvViewModel> csvCustomerLocationList)
        {
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "ValidateCustomerLocationMappingFileAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    StringBuilder errorList = new StringBuilder();
                    if (csvCustomerLocationList != null && csvCustomerLocationList.Any())
                    {
                        ValidateCustomerLocationBaseRow(csvCustomerLocationList, errorList);
                        if (errorList.Length <= 0)
                        {
                            ValidateCustomerLocationDuplicateData(csvCustomerLocationList, errorList);
                        }
                        if (errorList.Length <= 0)
                        {
                            ValidateCustomerLocationInvalidRecords(userContext, csvCustomerLocationList, errorList);
                        }
                        if (errorList.Length > 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = errorList.ToString();
                            if (response.StatusMessage.Length > 1000)
                            {
                                response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "No Records found in csv.";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "ValidateCustomerLocationMappingFileAsync", ex.Message, ex);
                }
                return response;
            }
        }
        private static void ValidateCustomerLocationBaseRow(List<ExternalCustomerLocationMappingCsvViewModel> csvLocationList, StringBuilder errorList)
        {
            int lineNumberOfCSV = 1;
            if (csvLocationList != null && csvLocationList.Any())
            {
                foreach (var item in csvLocationList)
                {
                    if (string.IsNullOrEmpty(item.CustomerLocationName) || string.IsNullOrEmpty(item.CompanyName))
                    {
                        if (errorList.Length > 0)
                        {
                            errorList.Append("</br>");
                        }
                        errorList.AppendLine(string.Format(Resource.errMsgEnterLocationMapping, lineNumberOfCSV));
                    }
                    lineNumberOfCSV++;
                }
            }
        }
        private static void ValidateCustomerLocationDuplicateData(List<ExternalCustomerLocationMappingCsvViewModel> csvLocationList, StringBuilder errorList)
        {
            var duplicateLocations = csvLocationList.GroupBy(x => new { x.CustomerLocationName , x.CompanyName})
                                                 .Where(item => item.Count() > 1)
                                                 .Select(y => y.Key)
                                                 .ToList();
            if (duplicateLocations != null && duplicateLocations.Any())
            {
                foreach (var location in duplicateLocations)
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgInvalidDuplicateLocationForMapping, location));
                }
            }
        }
        public void ValidateCustomerLocationInvalidRecords(UserContext userContext, List<ExternalCustomerLocationMappingCsvViewModel> csvLocationList, StringBuilder errorList)
        {
            
            var validLocations = Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == userContext.CompanyId).Select(t => new { JobName = t.FuelRequest.Job.Name, Customer = t.BuyerCompany.Name }).Distinct().ToList();
            int lineNumberOfCSV = 1;
            foreach (var item in csvLocationList)
            {
                if (!validLocations.Any(t => t.JobName.ToLower().Trim() == item.CustomerLocationName.ToLower().Trim()))
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgInvalidCustomerLocationForMapping, item.CustomerLocationName, lineNumberOfCSV));
                }
                else if (!validLocations.Any(t => t.JobName.ToLower().Trim() == item.CustomerLocationName.ToLower().Trim() && t.Customer.ToLower().Trim() == item.CompanyName.ToLower().Trim()))
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgInvalidCustomerForLocationForMapping, item.CompanyName, item.CustomerLocationName, lineNumberOfCSV));
                }
                lineNumberOfCSV++;
            }
        }
        #endregion
        #endregion

        #region External Product Mapping
        public async Task<List<ExternalProductMappingViewModel>> GetProductsForExternalMapping(UserContext userContext)
        {
            List<ExternalProductMappingViewModel> externalProductMappings = new List<ExternalProductMappingViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                externalProductMappings = await storedProcedureDomain.GetProductsForExternalMapping(userContext.CompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "GetProductsForExternalMapping", ex.Message, ex);
            }
            return externalProductMappings;

        }

        public async Task<StatusViewModel> SaveExternalProductMappings(ExternalProductMappingViewModel viewModel, UserContext usercontext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var productDetails = await Context.DataContext.ExternalProductMappings.Where(t => t.Id == viewModel.Id).FirstOrDefaultAsync();
                if (productDetails != null && productDetails.Id > 0)
                {
                    productDetails.TargetProductValue = viewModel.TargetProductValue;
                    productDetails.UpdatedDate = DateTimeOffset.Now;
                    productDetails.UpdatedBy = usercontext.Id;
                    Context.DataContext.Entry(productDetails).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                else
                {
                    productDetails = new ExternalProductMappings();
                    productDetails = viewModel.ToEntity();
                    productDetails.CreatedBy = usercontext.Id;
                    productDetails.CreatedByCompanyId = usercontext.CompanyId;
                    Context.DataContext.ExternalProductMappings.Add(productDetails);
                    await Context.CommitAsync();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMsgProductExternalMapping;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveExternalProductMappings", ex.Message, ex);
            }
            return response;
        }

        #region Product mapping Bulk Upload
        public async Task<StatusViewModel> SaveBulkUploadProductMapping(UserContext userContext, string csvText)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "SaveBulkUploadProductMapping"))
            {
                try
                {
                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                    var engine = new FileHelperEngine<ExternalProductMappingCsvViewModel>();
                    var csvProductList = engine.ReadString(csvText).ToList();

                    response = ValidateProductMappingFileAsync(userContext, csvProductList);
                    if (response.StatusCode == Status.Success)
                    {
                        var listExternalProducts = JsonConvert.DeserializeObject<List<ExternalProductMappingViewModel>>(JsonConvert.SerializeObject(csvProductList));
                        GetProductsId(listExternalProducts, userContext);

                        var productIds = listExternalProducts.Where(t => t.ProductId != null &&  t.OtherProductId == null).Select(t => t.ProductId.Value).ToList();
                        await AddOrUpdateTfxProductIdsMappings(userContext, listExternalProducts, productIds);

                        productIds = listExternalProducts.Where(t => t.ProductId == null && t.OtherProductId != null).Select(t => t.OtherProductId.Value).ToList();
                        await AddOrUpdateOtherProductIdsMappings(userContext, listExternalProducts, productIds);
                        response.StatusMessage = Resource.successMsgProductMappingBulkUpload;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveBulkUploadProductMapping", ex.Message, ex);
                }
                return response;
            }
        }

        private async Task AddOrUpdateTfxProductIdsMappings(UserContext userContext, List<ExternalProductMappingViewModel> listExternalProducts, List<int> productIds)
        {
            var listExternalProductsForUpdate = Context.DataContext.ExternalProductMappings.Where(t => t.TfxProductId != null && productIds.Contains(t.TfxProductId.Value) && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive).ToList();
            var productIdsToUpdate = new List<int>();
            if (listExternalProductsForUpdate != null && listExternalProductsForUpdate.Any())
            {
                productIdsToUpdate = listExternalProductsForUpdate.Select(t => t.TfxProductId.Value).ToList();
                foreach (var item in listExternalProductsForUpdate)
                {
                    item.TargetProductValue = listExternalProducts.Where(t => t.ProductId == item.TfxProductId).Select(t => t.TargetProductValue).FirstOrDefault();
                    item.UpdatedDate = DateTimeOffset.Now;
                    item.UpdatedBy = userContext.Id;
                    Context.DataContext.Entry(item).State = EntityState.Modified;
                }
                await Context.CommitAsync();
            }
            var listExternalProductMappings = new List<ExternalProductMappings>();
            var listExternalProductsForInsert = listExternalProducts.Where(t => t.ProductId != null && !productIdsToUpdate.Contains(t.ProductId.Value)).ToList();
            if (listExternalProductsForInsert != null && listExternalProductsForInsert.Any())
            {
                foreach (var item in listExternalProductsForInsert)
                {

                    ExternalProductMappings externalProductMappings = new ExternalProductMappings();
                    item.ThirdPartyId = (int)ExternalThirdPartyCompanies.PDI;
                    externalProductMappings = item.ToEntity();
                    externalProductMappings.CreatedBy = userContext.Id;
                    externalProductMappings.CreatedByCompanyId = userContext.CompanyId;
                    listExternalProductMappings.Add(externalProductMappings);
                }

                if (listExternalProductMappings != null && listExternalProductMappings.Any())
                {
                    Context.DataContext.ExternalProductMappings.AddRange(listExternalProductMappings);
                    await Context.CommitAsync();
                }
            }
        }

        private async Task AddOrUpdateOtherProductIdsMappings(UserContext userContext, List<ExternalProductMappingViewModel> listExternalProducts, List<int> otherProductIds)
        {
            var listExternalProductsForUpdate = Context.DataContext.ExternalProductMappings.Where(t => t.OtherProductId != null && otherProductIds.Contains(t.OtherProductId.Value) && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive).ToList();
            var productIdsToUpdate = new List<int>();
            if (listExternalProductsForUpdate != null && listExternalProductsForUpdate.Any())
            {
                productIdsToUpdate = listExternalProductsForUpdate.Select(t => t.OtherProductId.Value).ToList();
                foreach (var item in listExternalProductsForUpdate)
                {
                    item.TargetProductValue = listExternalProducts.Where(t => t.OtherProductId != null && t.OtherProductId == item.OtherProductId).Select(t => t.TargetProductValue).FirstOrDefault();
                    item.UpdatedDate = DateTimeOffset.Now;
                    item.UpdatedBy = userContext.Id;
                    Context.DataContext.Entry(item).State = EntityState.Modified;
                }
                await Context.CommitAsync();
            }
            var listExternalProductMappings = new List<ExternalProductMappings>();
            var listExternalProductsForInsert = listExternalProducts.Where(t => t.OtherProductId != null && !productIdsToUpdate.Contains(t.OtherProductId.Value)).ToList();
            if (listExternalProductsForInsert != null && listExternalProductsForInsert.Any())
            {
                foreach (var item in listExternalProductsForInsert)
                {
                    ExternalProductMappings externalProductMappings = new ExternalProductMappings();
                    item.ThirdPartyId = (int)ExternalThirdPartyCompanies.PDI;
                    externalProductMappings = item.ToEntity();
                    externalProductMappings.CreatedBy = userContext.Id;
                    externalProductMappings.CreatedByCompanyId = userContext.CompanyId;
                    listExternalProductMappings.Add(externalProductMappings);
                }

                if (listExternalProductMappings != null && listExternalProductMappings.Any())
                {
                    Context.DataContext.ExternalProductMappings.AddRange(listExternalProductMappings);
                    await Context.CommitAsync();
                }
            }
        }

        public void GetProductsId(List<ExternalProductMappingViewModel> externalProducts, UserContext userContext)
        {
            //Get product Id of entered products
            var externalProductNames = externalProducts.Select(t => t.ProductName).Distinct().ToList();
            var products = Context.DataContext.MstTfxProducts.Where(t => externalProductNames.Contains(t.Name)).Select(t => new { Id = t.Id, Name = t.Name }).ToList();
            foreach (var item in externalProducts)
            {
                item.ProductId = products.Where(t => t.Name.Trim().ToLower() == item.ProductName.Trim().ToLower()).Select(t => t.Id).FirstOrDefault();
            }

            var otherProducts = Context.DataContext.MstProducts.Where(t => t.ProductTypeId == (int)ProductTypes.NonStandardFuel && t.IsActive 
                                                                &&  externalProductNames.Contains(t.Name)).Select(t => new { Id = t.Id, Name = t.DisplayName }).ToList();
            foreach (var item in externalProducts)
            {
                if (!(item.ProductId.HasValue && item.ProductId.Value > 0))
                {
                    if (!products.Any(t => t.Name.Trim().ToLower() == item.ProductName.Trim().ToLower()))
                    {
                        item.ProductId = null;
                        item.OtherProductId = otherProducts.Where(t => t.Name.Trim().ToLower() == item.ProductName.Trim().ToLower()).Select(t => t.Id).FirstOrDefault();
                    }
                }
            }
        }

        public StatusViewModel ValidateProductMappingFileAsync(UserContext userContext, List<ExternalProductMappingCsvViewModel> csvProductList)
        {
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "ValidateProductMappingFileAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    StringBuilder errorList = new StringBuilder();
                    if (csvProductList != null && csvProductList.Any())
                    {
                        ValidateProductBaseRow(csvProductList, errorList);
                        if (errorList.Length <= 0)
                        {
                            ValidateProductDuplicateData(csvProductList, errorList);
                        }
                        if (errorList.Length <= 0)
                        {
                            ValidateProductInvalidRecords(userContext, csvProductList, errorList);
                        }
                        if (errorList.Length > 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = errorList.ToString();
                            if (response.StatusMessage.Length > 1000)
                            {
                                response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "No Records found in csv.";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "ValidateProductMappingFileAsync", ex.Message, ex);
                }
                return response;
            }
        }
        private static void ValidateProductBaseRow(List<ExternalProductMappingCsvViewModel> csvProductList, StringBuilder errorList)
        {
            int lineNumberOfCSV = 1;
            if (csvProductList != null && csvProductList.Any())
            {
                foreach (var item in csvProductList)
                {
                    if (string.IsNullOrEmpty(item.ProductName))
                    {
                        if (errorList.Length > 0)
                        {
                            errorList.Append("</br>");
                        }
                        errorList.AppendLine(string.Format(Resource.errMsgEnterProductNameForMapping, lineNumberOfCSV));
                    }
                    lineNumberOfCSV++;
                }
            }
        }
        private static void ValidateProductDuplicateData(List<ExternalProductMappingCsvViewModel> csvProductList, StringBuilder errorList)
        {
            var duplicateProducts = csvProductList.GroupBy(x => x.ProductName)
                                                 .Where(item => item.Count() > 1)
                                                 .Select(y => y.Key)
                                                 .ToList();
            if (duplicateProducts != null && duplicateProducts.Any())
            {
                foreach (var product in duplicateProducts)
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgDuplicateProductForMapping, product));
                }
            }
        }
        public void ValidateProductInvalidRecords(UserContext userContext, List<ExternalProductMappingCsvViewModel> csvProductList, StringBuilder errorList)
        {
            
            var validProducts = Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == userContext.CompanyId).Select(t => t.FuelRequest.MstProduct.MstTFXProduct.Name).Distinct().ToList();
            var otherProducts = Context.DataContext.MstProducts.Where(t => t.IsActive && t.ProductTypeId == (int)ProductTypes.NonStandardFuel).Select(t => t.DisplayName).ToList();
            validProducts.AddRange(otherProducts);

            int lineNumberOfCSV = 1;
            foreach (var item in csvProductList)
            {
                if (!validProducts.Contains(item.ProductName))
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgInvalidDuplicateProductForMapping, item.ProductName, lineNumberOfCSV));
                }
                lineNumberOfCSV++;
            }
        }
        #endregion
        #endregion

        #region External Supplier Mapping
        public async Task<List<ExternalSupplierMappingViewModel>> GetSuppliersForExternalMapping(UserContext userContext)
        {
            List<ExternalSupplierMappingViewModel> externalSupplierMappings = new List<ExternalSupplierMappingViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                externalSupplierMappings = await storedProcedureDomain.GetSuppliersForExternalMapping(userContext.CompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "GetSuppliersForExternalMapping", ex.Message, ex);
            }
            return externalSupplierMappings;

        }

        public async Task<StatusViewModel> SaveExternalSupplierMappings(ExternalSupplierMappingViewModel viewModel, UserContext usercontext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var supplierDetails = await Context.DataContext.ExternalSupplierMappings.Where(t => t.Id == viewModel.Id).FirstOrDefaultAsync();
                if (supplierDetails != null && supplierDetails.Id > 0)
                {
                    supplierDetails.TargetSupplierValue = viewModel.TargetSupplierValue;
                    supplierDetails.UpdatedDate = DateTimeOffset.Now;
                    supplierDetails.UpdatedBy = usercontext.Id;
                    Context.DataContext.Entry(supplierDetails).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                else
                {
                    supplierDetails = new ExternalSupplierMappings();
                    supplierDetails = viewModel.ToEntity();
                    supplierDetails.CreatedBy = usercontext.Id;
                    supplierDetails.CreatedByCompanyId = usercontext.CompanyId;
                    Context.DataContext.ExternalSupplierMappings.Add(supplierDetails);
                    await Context.CommitAsync();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMsgSupplierExternalMapping;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveExternalSupplierMappings", ex.Message, ex);
            }
            return response;
        }
        #region Supplier mapping Bulk Upload
        public async Task<StatusViewModel> SaveBulkUploadSupplierMapping(UserContext userContext, string csvText)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "SaveBulkUploadSupplierMapping"))
            {
                try
                {
                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                    var engine = new FileHelperEngine<ExternalSupplierMappingCsvViewModel>();
                    var csvSupplierList = engine.ReadString(csvText).ToList();

                    response = ValidateSupplierMappingFileAsync(userContext, csvSupplierList);
                    if (response.StatusCode == Status.Success)
                    {
                        var listExternalSuppliers = JsonConvert.DeserializeObject<List<ExternalSupplierMappingViewModel>>(JsonConvert.SerializeObject(csvSupplierList));
                        GetSuppliersId(listExternalSuppliers, userContext);

                        var supplierIds = listExternalSuppliers.Select(t => t.SupplierId).ToList();
                        var listExternalSuppliersForUpdate = Context.DataContext.ExternalSupplierMappings.Where(t => supplierIds.Contains(t.SupplierId) && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive).ToList();

                        var supplierIdsToUpdate = new List<int>();
                        if (listExternalSuppliersForUpdate != null && listExternalSuppliersForUpdate.Any())
                        {
                            supplierIdsToUpdate = listExternalSuppliersForUpdate.Select(t => t.SupplierId).ToList();
                            foreach (var item in listExternalSuppliersForUpdate)
                            {
                                item.TargetSupplierValue = listExternalSuppliers.Where(t => t.SupplierId == item.SupplierId).Select(t => t.TargetSupplierValue).FirstOrDefault();
                                item.UpdatedDate = DateTimeOffset.Now;
                                item.UpdatedBy = userContext.Id;
                                Context.DataContext.Entry(item).State = EntityState.Modified;
                            }
                            await Context.CommitAsync();
                        }
                        var listExternalSupplierMappings = new List<ExternalSupplierMappings>();
                        var listExternalSuppliersForInsert = listExternalSuppliers.Where(t => !supplierIdsToUpdate.Contains(t.SupplierId)).ToList();
                        if (listExternalSuppliersForInsert != null && listExternalSuppliersForInsert.Any())
                        {
                            foreach (var item in listExternalSuppliersForInsert)
                            {
                                ExternalSupplierMappings externalSupplierMappings = new ExternalSupplierMappings();
                                item.ThirdPartyId = (int)ExternalThirdPartyCompanies.PDI;
                                externalSupplierMappings = item.ToEntity();
                                externalSupplierMappings.CreatedBy = userContext.Id;
                                externalSupplierMappings.CreatedByCompanyId = userContext.CompanyId;
                                listExternalSupplierMappings.Add(externalSupplierMappings);
                            }

                            if (listExternalSupplierMappings != null && listExternalSupplierMappings.Any())
                            {
                                Context.DataContext.ExternalSupplierMappings.AddRange(listExternalSupplierMappings);
                                await Context.CommitAsync();
                            }
                        }
                        response.StatusMessage = Resource.successMsgSupplierMappingBulkUpload;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveBulkUploadSupplierMapping", ex.Message, ex);
                }
                return response;
            }
        }
        public void GetSuppliersId(List<ExternalSupplierMappingViewModel> externalSuppliers, UserContext userContext)
        {
            //Get supplier Id of entered suppliers
            var externalSupplierNames = externalSuppliers.Select(t => t.SupplierName).Distinct().ToList();
            var suppliers = Context.DataContext.Companies.Where(t => externalSupplierNames.Contains(t.Name)).Select(t => new { Id = t.Id, Name = t.Name }).ToList();
            foreach (var item in externalSuppliers)
            {
                item.SupplierId = suppliers.Where(t => t.Name.Trim().ToLower() == item.SupplierName.Trim().ToLower()).Select(t => t.Id).FirstOrDefault();
            }
        }

        public StatusViewModel ValidateSupplierMappingFileAsync(UserContext userContext, List<ExternalSupplierMappingCsvViewModel> csvSupplierList)
        {
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "ValidateSupplierMappingFileAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    StringBuilder errorList = new StringBuilder();
                    if (csvSupplierList != null && csvSupplierList.Any())
                    {
                        ValidateSupplierBaseRow(csvSupplierList, errorList);
                        if (errorList.Length <= 0)
                        {
                            ValidateSupplierDuplicateData(csvSupplierList, errorList);
                        }
                        if (errorList.Length <= 0)
                        {
                            ValidateSupplierInvalidRecords(userContext, csvSupplierList, errorList);
                        }
                        if (errorList.Length > 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = errorList.ToString();
                            if (response.StatusMessage.Length > 1000)
                            {
                                response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "No Records found in csv.";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "ValidateSupplierMappingFileAsync", ex.Message, ex);
                }
                return response;
            }
        }
        private static void ValidateSupplierBaseRow(List<ExternalSupplierMappingCsvViewModel> csvSupplierList, StringBuilder errorList)
        {
            int lineNumberOfCSV = 1;
            if (csvSupplierList != null && csvSupplierList.Any())
            {
                foreach (var item in csvSupplierList)
                {
                    if (string.IsNullOrEmpty(item.SupplierName))
                    {
                        if (errorList.Length > 0)
                        {
                            errorList.Append("</br>");
                        }
                        errorList.AppendLine(string.Format(Resource.errMsgEnterSupplierNameForMapping, lineNumberOfCSV));
                    }
                    lineNumberOfCSV++;
                }
            }
        }
        private static void ValidateSupplierDuplicateData(List<ExternalSupplierMappingCsvViewModel> csvSupplierList, StringBuilder errorList)
        {
            var duplicateSuppliers = csvSupplierList.GroupBy(x => x.SupplierName)
                                                 .Where(item => item.Count() > 1)
                                                 .Select(y => y.Key)
                                                 .ToList();
            if (duplicateSuppliers != null && duplicateSuppliers.Any())
            {
                foreach (var supplier in duplicateSuppliers)
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgDuplicateSupplierForMapping, supplier));
                }
            }
        }
        public void ValidateSupplierInvalidRecords(UserContext userContext, List<ExternalSupplierMappingCsvViewModel> csvSupplierList, StringBuilder errorList)
        {
            var validSupplier = userContext.CompanyName;
            int lineNumberOfCSV = 1;
            foreach (var item in csvSupplierList)
            {
                if (item.SupplierName.ToLower().Trim() != validSupplier.ToLower().Trim())
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgInvalidSupplierForMapping, item.SupplierName, lineNumberOfCSV));
                }
                lineNumberOfCSV++;
            }
        }
        #endregion
        #endregion

        #region External Terminal Mapping
        public async Task<List<ExternalTerminalMappingViewModel>> GetTerminalsForExternalMapping(UserContext userContext)
        {
            List<ExternalTerminalMappingViewModel> externalTerminalMappings = new List<ExternalTerminalMappingViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                externalTerminalMappings = await storedProcedureDomain.GetTerminalsForExternalMapping(userContext.CompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "GetTerminalsForExternalMapping", ex.Message, ex);
            }
            return externalTerminalMappings;

        }
        public async Task<StatusViewModel> SaveExternalTerminalMappings(ExternalTerminalMappingViewModel viewModel, UserContext usercontext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var terminalDetails = await Context.DataContext.ExternalTerminalMappings.Where(t => t.Id == viewModel.Id && t.CreatedCompanyId == usercontext.CompanyId).FirstOrDefaultAsync();
                if (terminalDetails != null && terminalDetails.Id > 0)
                {
                    terminalDetails.TargetTerminalValue = viewModel.TargetTerminalValue;
                    terminalDetails.CreatedCompanyId = usercontext.CompanyId;
                    terminalDetails.UpdatedDate = DateTimeOffset.Now;
                    terminalDetails.UpdatedBy = usercontext.Id;
                    Context.DataContext.Entry(terminalDetails).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                else
                {
                    terminalDetails = new ExternalTerminalMappings();
                    terminalDetails = viewModel.ToEntity();
                    terminalDetails.CreatedBy = usercontext.Id;
                    terminalDetails.CreatedCompanyId = usercontext.CompanyId;
                    Context.DataContext.ExternalTerminalMappings.Add(terminalDetails);
                    await Context.CommitAsync();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMsgTerminalExternalMapping;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveExternalTerminalMappings", ex.Message, ex);
            }
            return response;
        }
        #region Terminal mapping Bulk Upload
        public async Task<StatusViewModel> SaveBulkUploadTerminalMapping(UserContext userContext, string csvText)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "SaveBulkUploadTerminalMapping"))
            {
                try
                {
                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                    var engine = new FileHelperEngine<ExternalTerminalMappingCsvViewModel>();
                    var csvTerminalList = engine.ReadString(csvText).ToList();

                    response = ValidateTerminalMappingFileAsync(userContext, csvTerminalList);
                    if (response.StatusCode == Status.Success)
                    {
                        var listExternalTerminals = JsonConvert.DeserializeObject<List<ExternalTerminalMappingViewModel>>(JsonConvert.SerializeObject(csvTerminalList));
                        GetTerminalsId(listExternalTerminals, userContext);

                        var terminalIds = listExternalTerminals.Select(t => t.TerminalId).ToList();
                        var listExternalTerminalsForUpdate = Context.DataContext.ExternalTerminalMappings.Where(t => terminalIds.Contains(t.TerminalId) && t.CreatedCompanyId == userContext.CompanyId && t.IsActive).ToList();
                        var terminalIdsToUpdate = new List<int>();
                        if (listExternalTerminalsForUpdate != null && listExternalTerminalsForUpdate.Any())
                        {
                            terminalIdsToUpdate = listExternalTerminalsForUpdate.Select(t => t.TerminalId).ToList();
                            foreach (var item in listExternalTerminalsForUpdate)
                            {
                                item.TargetTerminalValue = listExternalTerminals.Where(t => t.TerminalId == item.TerminalId).Select(t => t.TargetTerminalValue).FirstOrDefault();
                                item.UpdatedDate = DateTimeOffset.Now;
                                item.UpdatedBy = userContext.Id;
                                Context.DataContext.Entry(item).State = EntityState.Modified;
                            }
                            await Context.CommitAsync();
                        }
                        var listExternalTerminalMappings = new List<ExternalTerminalMappings>();
                        var listExternalTerminalsForInsert = listExternalTerminals.Where(t => !terminalIdsToUpdate.Contains(t.TerminalId)).ToList();
                        if (listExternalTerminalsForInsert != null && listExternalTerminalsForInsert.Any())
                        {
                            foreach (var item in listExternalTerminalsForInsert)
                            {

                                ExternalTerminalMappings externalterminalMappings = new ExternalTerminalMappings();
                                item.ThirdPartyId = (int)ExternalThirdPartyCompanies.PDI;
                                externalterminalMappings = item.ToEntity();
                                externalterminalMappings.CreatedBy = userContext.Id;
                                externalterminalMappings.CreatedCompanyId = userContext.CompanyId;
                                listExternalTerminalMappings.Add(externalterminalMappings);
                            }

                            if (listExternalTerminalMappings != null && listExternalTerminalMappings.Any())
                            {
                                Context.DataContext.ExternalTerminalMappings.AddRange(listExternalTerminalMappings);
                                await Context.CommitAsync();
                            }
                        }
                        response.StatusMessage = Resource.successMsgTerminalMappingBulkUpload;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveBulkUploadBulkPlantMapping", ex.Message, ex);
                }
                return response;
            }
        }
        public void GetTerminalsId(List<ExternalTerminalMappingViewModel> externalTerminals, UserContext userContext)
        {
            //Get Terminal Id of entered terminals
            var externalTerminalsNames = externalTerminals.Select(t => t.TerminalName).Distinct().ToList();
            var terminals = Context.DataContext.MstExternalTerminals.Where(t => externalTerminalsNames.Contains(t.Name)).Select(t => new { Id = t.Id, Name = t.Name }).ToList();
            foreach (var item in externalTerminals)
            {
                item.TerminalId = terminals.Where(t => t.Name.Trim().ToLower() == item.TerminalName.Trim().ToLower()).Select(t => t.Id).FirstOrDefault();
            }
        }
        public StatusViewModel ValidateTerminalMappingFileAsync(UserContext userContext, List<ExternalTerminalMappingCsvViewModel> csvTerminaltList)
        {
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "ValidateTerminalMappingFileAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    StringBuilder errorList = new StringBuilder();
                    if (csvTerminaltList != null && csvTerminaltList.Any())
                    {
                        ValidateTerminalBaseRow(csvTerminaltList, errorList);
                        if (errorList.Length <= 0)
                        {
                            ValidateTerminalDuplicateData(csvTerminaltList, errorList);
                        }
                        if (errorList.Length <= 0)
                        {
                            ValidateTerminalInvalidRecords(userContext, csvTerminaltList, errorList);
                        }
                        if (errorList.Length > 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = errorList.ToString();
                            if (response.StatusMessage.Length > 1000)
                            {
                                response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "No Records found in csv.";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "ValidateTerminalMappingFileAsync", ex.Message, ex);
                }
                return response;
            }
        }
        private static void ValidateTerminalBaseRow(List<ExternalTerminalMappingCsvViewModel> csvTerminalList, StringBuilder errorList)
        {
            int lineNumberOfCSV = 1;
            if (csvTerminalList != null && csvTerminalList.Any())
            {
                foreach (var item in csvTerminalList)
                {
                    if (string.IsNullOrEmpty(item.TerminalName))
                    {
                        if (errorList.Length > 0)
                        {
                            errorList.Append("</br>");
                        }
                        errorList.AppendLine(string.Format(Resource.errMsgEnterTerminalNameForMapping, lineNumberOfCSV));
                    }
                    lineNumberOfCSV++;
                }
            }
        }
        private static void ValidateTerminalDuplicateData(List<ExternalTerminalMappingCsvViewModel> csvTerminalList, StringBuilder errorList)
        {
            var duplicateTerminals = csvTerminalList.GroupBy(x => x.TerminalName)
                                                 .Where(item => item.Count() > 1)
                                                 .Select(y => y.Key)
                                                 .ToList();
            if (duplicateTerminals != null && duplicateTerminals.Any())
            {
                foreach (var terminal in duplicateTerminals)
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgDuplicateTerminalForMapping, terminal));
                }
            }
        }
        public void ValidateTerminalInvalidRecords(UserContext userContext, List<ExternalTerminalMappingCsvViewModel> csvTerminalList, StringBuilder errorList)
        {
            
            var validTerminals = Context.DataContext.MstExternalTerminals.Where(t => t.IsActive).Select(t => t.Name).Distinct().ToList();
            int lineNumberOfCSV = 1;
            foreach (var item in csvTerminalList)
            {
                if (!validTerminals.Contains(item.TerminalName))
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgInvalidTerminalForMapping, item.TerminalName, lineNumberOfCSV));
                }
                lineNumberOfCSV++;
            }
        }
        #endregion
        #endregion

        #region External Bulk Plant Mapping
        public async Task<List<ExternalBulkPlantMappingViewModel>> GetBulkPlantsForExternalMapping(UserContext userContext)
        {
            List<ExternalBulkPlantMappingViewModel> externalBulkPlantMappings = new List<ExternalBulkPlantMappingViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                externalBulkPlantMappings = await storedProcedureDomain.GetBulkPlantsForExternalMapping();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "GetBulkPlantsForExternalMapping", ex.Message, ex);
            }
            return externalBulkPlantMappings;

        }
        public async Task<StatusViewModel> SaveExternalBulkPlantMappings(ExternalBulkPlantMappingViewModel viewModel, UserContext usercontext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var bulkPlantDetails = await Context.DataContext.ExternalBulkPlantMappings.Where(t => t.Id == viewModel.Id).FirstOrDefaultAsync();
                if (bulkPlantDetails != null && bulkPlantDetails.Id > 0)
                {
                    bulkPlantDetails.TargetBulkPlantValue = viewModel.TargetBulkPlantValue;
                    bulkPlantDetails.UpdatedDate = DateTimeOffset.Now;
                    bulkPlantDetails.UpdatedBy = usercontext.Id;
                    Context.DataContext.Entry(bulkPlantDetails).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                else
                {
                    bulkPlantDetails = new ExternalBulkPlantMappings();
                    bulkPlantDetails = viewModel.ToEntity();
                    bulkPlantDetails.CreatedBy = usercontext.Id;
                    bulkPlantDetails.CreatedByCompanyId = usercontext.CompanyId;
                    Context.DataContext.ExternalBulkPlantMappings.Add(bulkPlantDetails);
                    await Context.CommitAsync();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMsgBulkPlantExternalMapping;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveExternalTerminalMappings", ex.Message, ex);
            }
            return response;
        }
        #region BulkPlant mapping Bulk Upload
        public async Task<StatusViewModel> SaveBulkUploadBulkPlantMapping(UserContext userContext, string csvText)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "SaveBulkUploadBulkPlantMapping"))
            {
                try
                {
                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                    var engine = new FileHelperEngine<ExternalBulkPlantMappingCsvViewModel>();
                    var csvBulkPlantList = engine.ReadString(csvText).ToList();

                    response = ValidateBulkplantMappingFileAsync(userContext, csvBulkPlantList);
                    if (response.StatusCode == Status.Success)
                    {
                        var listExternalBulkPlants = JsonConvert.DeserializeObject<List<ExternalBulkPlantMappingViewModel>>(JsonConvert.SerializeObject(csvBulkPlantList));
                        GetBulkPlantsId(listExternalBulkPlants, userContext);

                        var bulkplantIds = listExternalBulkPlants.Select(t => t.BulkPlantId).ToList();
                        var listExternalBulkPlantsForUpdate = Context.DataContext.ExternalBulkPlantMappings.Where(t => bulkplantIds.Contains(t.BulkPlantId) && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive).ToList();
                        var bulkPlantIdsToUpdate = new List<int>();
                        if (listExternalBulkPlantsForUpdate != null && listExternalBulkPlantsForUpdate.Any())
                        {
                            bulkPlantIdsToUpdate = listExternalBulkPlantsForUpdate.Select(t => t.BulkPlantId).ToList();
                            foreach (var item in listExternalBulkPlantsForUpdate)
                            {
                                item.TargetBulkPlantValue = listExternalBulkPlants.Where(t => t.BulkPlantId == item.BulkPlantId).Select(t => t.TargetBulkPlantValue).FirstOrDefault();
                                item.UpdatedDate = DateTimeOffset.Now;
                                item.UpdatedBy = userContext.Id;
                                Context.DataContext.Entry(item).State = EntityState.Modified;
                            }
                            await Context.CommitAsync();
                        }
                        var listExternalBulkPlantMappings = new List<ExternalBulkPlantMappings>();
                        var listExternalBulkPlantsForInsert = listExternalBulkPlants.Where(t => !bulkPlantIdsToUpdate.Contains(t.BulkPlantId)).ToList();
                        if (listExternalBulkPlantsForInsert != null && listExternalBulkPlantsForInsert.Any())
                        {
                            foreach (var item in listExternalBulkPlantsForInsert)
                            {

                                ExternalBulkPlantMappings externalBulkplantMappings = new ExternalBulkPlantMappings();
                                item.ThirdPartyId = (int)ExternalThirdPartyCompanies.PDI;
                                externalBulkplantMappings = item.ToEntity();
                                externalBulkplantMappings.CreatedBy = userContext.Id;
                                externalBulkplantMappings.CreatedByCompanyId = userContext.CompanyId;
                                listExternalBulkPlantMappings.Add(externalBulkplantMappings);
                            }

                            if (listExternalBulkPlantMappings != null && listExternalBulkPlantMappings.Any())
                            {
                                Context.DataContext.ExternalBulkPlantMappings.AddRange(listExternalBulkPlantMappings);
                                await Context.CommitAsync();
                            }
                        }
                        response.StatusMessage = Resource.successMsgBulkPlantMappingBulkUpload;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveBulkUploadBulkPlantMapping", ex.Message, ex);
                }
                return response;
            }
        }
        public void GetBulkPlantsId(List<ExternalBulkPlantMappingViewModel> externalBulkPlants, UserContext userContext)
        {
            //Get Bulk Plant Id of entered bulkplants
            var externalBulkPlantsNames = externalBulkPlants.Select(t => t.BulkPlantName).Distinct().ToList();
            var bulkplants = Context.DataContext.BulkPlantLocations.Where(t => externalBulkPlantsNames.Contains(t.Name) && t.IsActive).Select(t => new { Id = t.Id, Name = t.Name }).ToList();
            foreach (var item in externalBulkPlants)
            {
                item.BulkPlantId = bulkplants.Where(t => t.Name.Trim().ToLower() == item.BulkPlantName.Trim().ToLower()).Select(t => t.Id).FirstOrDefault();
            }
        }
        public StatusViewModel ValidateBulkplantMappingFileAsync(UserContext userContext, List<ExternalBulkPlantMappingCsvViewModel> csvBulkPlantList)
        {
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "ValidateBulkplantMappingFileAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    StringBuilder errorList = new StringBuilder();
                    if (csvBulkPlantList != null && csvBulkPlantList.Any())
                    {
                        ValidateBulkPlantBaseRow(csvBulkPlantList, errorList);
                        if (errorList.Length <= 0)
                        {
                            ValidateBulkPlantDuplicateData(csvBulkPlantList, errorList);
                        }
                        if (errorList.Length <= 0)
                        {
                            ValidateBulkPlantInvalidRecords(userContext, csvBulkPlantList, errorList);
                        }
                        if (errorList.Length > 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = errorList.ToString();
                            if (response.StatusMessage.Length > 1000)
                            {
                                response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "No Records found in csv.";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "ValidateBulkplantMappingFileAsync", ex.Message, ex);
                }
                return response;
            }
        }
        private static void ValidateBulkPlantBaseRow(List<ExternalBulkPlantMappingCsvViewModel> csvBulkPlantList, StringBuilder errorList)
        {
            int lineNumberOfCSV = 1;
            if (csvBulkPlantList != null && csvBulkPlantList.Any())
            {
                foreach (var item in csvBulkPlantList)
                {
                    if (string.IsNullOrEmpty(item.BulkPlantName))
                    {
                        if (errorList.Length > 0)
                        {
                            errorList.Append("</br>");
                        }
                        errorList.AppendLine(string.Format(Resource.errMsgEnterBulkPlantNameForMapping, lineNumberOfCSV));
                    }
                    lineNumberOfCSV++;
                }
            }
        }
        private static void ValidateBulkPlantDuplicateData(List<ExternalBulkPlantMappingCsvViewModel> csvBulkPlantList, StringBuilder errorList)
        {
            var duplicateBukPlants = csvBulkPlantList.GroupBy(x => x.BulkPlantName)
                                                 .Where(item => item.Count() > 1)
                                                 .Select(y => y.Key)
                                                 .ToList();
            if (duplicateBukPlants != null && duplicateBukPlants.Any())
            {
                foreach (var bulkplant in duplicateBukPlants)
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgDuplicateBulkPlantForMapping, bulkplant));
                }
            }
        }
        public void ValidateBulkPlantInvalidRecords(UserContext userContext, List<ExternalBulkPlantMappingCsvViewModel> csvBulkPlantList, StringBuilder errorList)
        {
            
            var validBulkPants = Context.DataContext.BulkPlantLocations.Where(t => t.IsActive).Select(t => t.Name).Distinct().ToList();
            int lineNumberOfCSV = 1;
            foreach (var item in csvBulkPlantList)
            {
                if (!validBulkPants.Contains(item.BulkPlantName))
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgInvalidBulkPlantForMapping, item.BulkPlantName, lineNumberOfCSV));
                }
                lineNumberOfCSV++;
            }
        }
        #endregion
        #endregion

        #region External Driver Mapping
        public async Task<List<ExternalDriverMappingViewModel>> GetDriversForExternalMapping(UserContext userContext)
        {
            List<ExternalDriverMappingViewModel> externalDriverMappings = new List<ExternalDriverMappingViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                externalDriverMappings = await storedProcedureDomain.GetDriversForExternalMapping();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "GetDriversForExternalMapping", ex.Message, ex);
            }
            return externalDriverMappings;

        }

        public async Task<StatusViewModel> SaveExternalDriverMappings(ExternalDriverMappingViewModel viewModel, UserContext usercontext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var driverDetails = await Context.DataContext.ExternalDriverMappings.Where(t => t.Id == viewModel.Id).FirstOrDefaultAsync();
                if (driverDetails != null && driverDetails.Id > 0)
                {
                    driverDetails.TargetDriverValue = viewModel.TargetDriverValue;
                    driverDetails.UpdatedDate = DateTimeOffset.Now;
                    driverDetails.UpdatedBy = usercontext.Id;
                    Context.DataContext.Entry(driverDetails).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                else
                {
                    driverDetails = new ExternalDriverMappings();
                    driverDetails = viewModel.ToEntity();
                    driverDetails.CreatedBy = usercontext.Id;
                    driverDetails.CreatedByCompanyId = usercontext.CompanyId;
                    Context.DataContext.ExternalDriverMappings.Add(driverDetails);
                    await Context.CommitAsync();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMsgDriverExternalMapping;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveExternalDriverMappings", ex.Message, ex);
            }
            return response;
        }
        #region Drivermapping Bulk Upload
        public async Task<StatusViewModel> SaveBulkUploadDriverMapping(UserContext userContext, string csvText)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "SaveBulkUploadDriverMapping"))
            {
                try
                {
                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                    var engine = new FileHelperEngine<ExternalDriverMappingCsvViewModel>();
                    var csvDriverList = engine.ReadString(csvText).ToList();

                    response = ValidateDriverMappingFileAsync(userContext, csvDriverList);
                    if (response.StatusCode == Status.Success)
                    {                        
                        var listExternalDrivers = JsonConvert.DeserializeObject<List<ExternalDriverMappingViewModel>>(JsonConvert.SerializeObject(csvDriverList));
                        GetDriversId(listExternalDrivers, userContext);

                        var driverIds = listExternalDrivers.Select(t => t.DriverId).ToList();
                        var listExternalDriversForUpdate = Context.DataContext.ExternalDriverMappings.Where(t => driverIds.Contains(t.DriverId) && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive).ToList();
                        var driverIdsToUpdate = new List<int>();
                        if (listExternalDriversForUpdate != null && listExternalDriversForUpdate.Any())
                        {
                            driverIdsToUpdate = listExternalDriversForUpdate.Select(t => t.DriverId).ToList();
                            foreach (var driverDetails in listExternalDriversForUpdate)
                            {
                                driverDetails.TargetDriverValue = listExternalDrivers.Where(t => t.DriverId == driverDetails.DriverId).Select(t => t.TargetDriverValue).FirstOrDefault();
                                driverDetails.UpdatedDate = DateTimeOffset.Now;
                                driverDetails.UpdatedBy = userContext.Id;
                                Context.DataContext.Entry(driverDetails).State = EntityState.Modified;
                            }
                            await Context.CommitAsync();
                        }

                        var listExternalDriverMappings = new List<ExternalDriverMappings>();
                        var listExternalDriversForInsert = listExternalDrivers.Where(t => !driverIdsToUpdate.Contains(t.DriverId)).ToList();
                        if (listExternalDriversForInsert != null && listExternalDriversForInsert.Any())
                        {
                            foreach (var item in listExternalDriversForInsert)
                            {

                                ExternalDriverMappings externalDriverMappings = new ExternalDriverMappings();
                                item.ThirdPartyId = (int)ExternalThirdPartyCompanies.PDI;
                                externalDriverMappings = item.ToEntity();
                                externalDriverMappings.CreatedBy = userContext.Id;
                                externalDriverMappings.CreatedByCompanyId = userContext.CompanyId;
                                listExternalDriverMappings.Add(externalDriverMappings);
                            }

                            if (listExternalDriverMappings != null && listExternalDriverMappings.Any())
                            {
                                Context.DataContext.ExternalDriverMappings.AddRange(listExternalDriverMappings);
                                await Context.CommitAsync();
                            }
                        }
                        response.StatusMessage = Resource.successMsgDriverMappingBulkUpload;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageExternalDriverBulkUpload;
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveBulkUploadDriverMapping", ex.Message, ex);
                }
                return response;
            }
        }
        public void GetDriversId(List<ExternalDriverMappingViewModel> externalDrivers, UserContext userContext)
        {
            var firstNameList = externalDrivers.Select(s => s.FirstName.Trim().ToLower()).ToList();
            var lastNameList = externalDrivers.Select(s => s.LastName.Trim().ToLower()).ToList();
            var emailList = externalDrivers.Select(s => s.Email.Trim().ToLower()).ToList();

            var drivers = Context.DataContext.Users.Where(t => firstNameList.Contains(t.FirstName.Trim().ToLower())
                                                                && lastNameList.Contains(t.LastName.Trim().ToLower())
                                                                && emailList.Contains(t.Email.Trim().ToLower())).ToList();
             foreach (var item in externalDrivers)
             {
                item.DriverId = drivers.Where(t => item.FirstName.Trim().ToLower() == t.FirstName.Trim().ToLower()
                                                                 && item.LastName.Trim().ToLower() == t.LastName.Trim().ToLower()
                                                                 && item.Email.Trim().ToLower() == t.Email.Trim().ToLower()).Select(t => t.Id).FirstOrDefault();
             }     
        }

        public StatusViewModel ValidateDriverMappingFileAsync(UserContext userContext, List<ExternalDriverMappingCsvViewModel> csvDriverList)
        {
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "ValidateDriverMappingFileAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    StringBuilder errorList = new StringBuilder();
                    if (csvDriverList != null && csvDriverList.Any())
                    {
                        ValidateDriverBaseRow(csvDriverList, errorList);
                        if (errorList.Length <= 0)
                        {
                            ValidateDriverDuplicateData(csvDriverList, errorList);
                        }
                        if (errorList.Length <= 0)
                        {
                            ValidateDriverInvalidRecords(userContext, csvDriverList, errorList);
                        }
                        if (errorList.Length > 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = errorList.ToString();
                            if (response.StatusMessage.Length > 1000)
                                response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                        }
                        else
                        {
                            response.StatusCode = Status.Success;                            
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "No Records found in csv.";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "ValidateDriverMappingFileAsync", ex.Message, ex);
                }
                return response;
            }
        }
        private static void ValidateDriverDuplicateData(List<ExternalDriverMappingCsvViewModel> csvDriverList, StringBuilder errorList)
        {

            var duplicateDrivers = csvDriverList.GroupBy(x => x.FirstName.Trim().ToLower() + x.LastName.Trim().ToLower() + x.Email.Trim().ToLower())
                                                 .Where(item => item.Count() > 1)
                                                 .Select(y => y.Key)
                                                 .ToList();
            if (duplicateDrivers != null && duplicateDrivers.Any())
            {
                foreach (var driver in duplicateDrivers)
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgDuplicateDriverForMapping, driver));
                }
            }
        }
        private static void ValidateDriverBaseRow(List<ExternalDriverMappingCsvViewModel> csvDriverList, StringBuilder errorList)
        {
            int lineNumberOfCSV = 1;
            if (csvDriverList != null && csvDriverList.Any())
            {
                foreach (var item in csvDriverList)
                {
                    if ((string.IsNullOrEmpty(item.FirstName) && string.IsNullOrEmpty(item.LastName)) || string.IsNullOrEmpty(item.Email))
                    {
                        if (errorList.Length > 0)
                        {
                            errorList.Append("</br>");
                        }
                        errorList.AppendLine(string.Format(Resource.errMsgEnterDriverNameForMapping, lineNumberOfCSV));
                    }
                    lineNumberOfCSV++;
                }
            }
        }
        public void ValidateDriverInvalidRecords(UserContext userContext, List<ExternalDriverMappingCsvViewModel> csvDriverList, StringBuilder errorList)
        {
            var validDrivers = Context.DataContext.Users.Where(t => t.Company.IsActive &&
                            t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Driver)
                            && ((t.IsActive && t.IsOnboardingComplete) || (!t.IsActive && !t.IsOnboardingComplete)))
                            .Select(t => new { t.FirstName, t.LastName, t.Email }).Distinct().ToList();

            int lineNumberOfCSV = 1;
            foreach (var item in csvDriverList)
            {
                if (!validDrivers.Any(t => t.FirstName.ToLower().Trim() == item.FirstName.ToLower().Trim() && t.LastName.ToLower().Trim() == item.LastName.ToLower().Trim() && t.Email.ToLower().Trim() == item.Email.ToLower().Trim()))
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgInvalidDriverForMapping, item.FirstName + ' ' + item.LastName, lineNumberOfCSV));
                }
                lineNumberOfCSV++;
            }
        }
        #endregion
        #endregion

        #region External Carrier Mapping
        public async Task<List<ExternalCarrierMappingViewModel>> GetCarriersForExternalMapping(UserContext userContext)
        {
            List<ExternalCarrierMappingViewModel> externalCarrierMappings = new List<ExternalCarrierMappingViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                externalCarrierMappings = await storedProcedureDomain.GetCarriersForExternalMapping();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "GetCarriersForExternalMapping", ex.Message, ex);
            }
            return externalCarrierMappings;

        }

        public async Task<StatusViewModel> SaveExternalCarrierMappings(ExternalCarrierMappingViewModel viewModel, UserContext usercontext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var carrierDetails = await Context.DataContext.ExternalCarrierMappings.Where(t => t.Id == viewModel.Id).FirstOrDefaultAsync();
                if (carrierDetails != null && carrierDetails.Id > 0)
                {
                    carrierDetails.TargetCarrierValue = viewModel.TargetCarrierValue;
                    carrierDetails.UpdatedDate = DateTimeOffset.Now;
                    carrierDetails.UpdatedBy = usercontext.Id;
                    Context.DataContext.Entry(carrierDetails).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                else
                {
                    carrierDetails = new ExternalCarrierMappings();
                    carrierDetails = viewModel.ToEntity();
                    carrierDetails.CreatedBy = usercontext.Id;
                    carrierDetails.CreatedByCompanyId = usercontext.CompanyId;
                    Context.DataContext.ExternalCarrierMappings.Add(carrierDetails);
                    await Context.CommitAsync();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMsgCarrierExternalMapping;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveExternalCarrierMappings", ex.Message, ex);
            }
            return response;
        }

        #region Carrier mapping Bulk Upload

        public async Task<StatusViewModel> SaveBulkUploadCarrierMapping(UserContext userContext, string csvText)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "SaveBulkUploadCarrierMapping"))
            {
                try
                {
                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                    var engine = new FileHelperEngine<ExternalCarrierMappingCsvViewModel>();
                    var csvCarrierList = engine.ReadString(csvText).ToList();

                    response = ValidateCarrierMappingFileAsync(userContext, csvCarrierList);
                    if (response.StatusCode == Status.Success)
                    {
                        var listExternalCarriers = JsonConvert.DeserializeObject<List<ExternalCarrierMappingViewModel>>(JsonConvert.SerializeObject(csvCarrierList));
                        GetCarrierId(listExternalCarriers, userContext);

                        var carrierIds = listExternalCarriers.Select(t => t.CarrierId).ToList();
                        var listExtrenalCarriersForUpdate = Context.DataContext.ExternalCarrierMappings.Where(t => carrierIds.Contains(t.CarrierId) && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive).ToList();

                        var carrierIdsToUpdate = new List<int>();
                        if (listExtrenalCarriersForUpdate != null && listExtrenalCarriersForUpdate.Any())
                        {
                            carrierIdsToUpdate = listExtrenalCarriersForUpdate.Select(t => t.CarrierId).ToList();
                            foreach (var item in listExtrenalCarriersForUpdate)
                            {
                                item.TargetCarrierValue = listExternalCarriers.Where(t => t.CarrierId == item.CarrierId).Select(t => t.TargetCarrierValue).FirstOrDefault();
                                item.UpdatedDate = DateTimeOffset.Now;
                                item.UpdatedBy = userContext.Id;
                                Context.DataContext.Entry(item).State = EntityState.Modified;
                            }
                            await Context.CommitAsync();
                        }
                        var listExternalCarrierMappings = new List<ExternalCarrierMappings>();
                        var listExternalCarrierForInsert = listExternalCarriers.Where(t => !carrierIdsToUpdate.Contains(t.CarrierId)).ToList();
                        if (listExternalCarrierForInsert != null && listExternalCarrierForInsert.Any())
                        {
                            foreach (var item in listExternalCarrierForInsert)
                            {
                                ExternalCarrierMappings externalCarrierMappings = new ExternalCarrierMappings();
                                item.ThirdPartyId = (int)ExternalThirdPartyCompanies.PDI;
                                externalCarrierMappings = item.ToEntity();
                                externalCarrierMappings.CreatedBy = userContext.Id;
                                externalCarrierMappings.CreatedByCompanyId = userContext.CompanyId;
                                listExternalCarrierMappings.Add(externalCarrierMappings);
                            }
                            if (listExternalCarrierMappings != null && listExternalCarrierMappings.Any())
                            {
                                Context.DataContext.ExternalCarrierMappings.AddRange(listExternalCarrierMappings);
                                await Context.CommitAsync();
                            }
                        }
                        response.StatusMessage = Resource.successMsgCarrierMappingBulkUpload;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveBulkUploadCarrierMapping", ex.Message, ex);
                }
                return response;
            }

        }
        public void GetCarrierId(List<ExternalCarrierMappingViewModel> externalCarriers, UserContext userContext)
        {
            var externalCarrierNames = externalCarriers.Select(t => t.CarrierName).Distinct().ToList();
            var carriers = Context.DataContext.Companies.Where(x => externalCarrierNames.Contains(x.Name)
                                                             &&(x.CompanyTypeId ==(int) CompanyType.Carrier
                                                              ||x.CompanyTypeId ==(int) CompanyType.SupplierAndCarrier
                                                              ||x.CompanyTypeId ==(int) CompanyType.BuyerSupplierAndCarrier)
                                                              )
                                                              .Select(x => new { Id = x.Id, Name = x.Name }).ToList();

            foreach (var item in externalCarriers)
            {
                item.CarrierId = carriers.Where(t => t.Name.ToLower().Trim() == item.CarrierName.ToLower().Trim()).Select(t => t.Id).FirstOrDefault();
            }
        }
        public StatusViewModel ValidateCarrierMappingFileAsync(UserContext userContext, List<ExternalCarrierMappingCsvViewModel> csvCarrierList)
        {
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "ValidateCarrierMappingFileAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    StringBuilder errorList = new StringBuilder();
                    if (csvCarrierList != null && csvCarrierList.Any())
                    {
                        ValidateCarrierBaseRow(csvCarrierList, errorList);
                        if (errorList.Length <= 0)
                        {
                            ValidateCarrierDuplicateData(csvCarrierList, errorList);
                        }
                        if (errorList.Length <= 0)
                        {
                            ValidateCarrierInvalidRecord(userContext, csvCarrierList, errorList);
                        }
                        if (errorList.Length > 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = errorList.ToString();
                            if (response.StatusMessage.Length > 1000)
                            {
                                response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "No Records found in csv.";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "ValidateCarrierMappingFileAsync", ex.Message, ex);
                }
                return response;
            }
        }
        public static void ValidateCarrierBaseRow(List<ExternalCarrierMappingCsvViewModel> csvCarrierList, StringBuilder errorList)
        {
            int lineNumberOfCSV = 1;
            if (csvCarrierList != null && csvCarrierList.Any())
            {
                foreach (var item in csvCarrierList)
                {
                    if (string.IsNullOrEmpty(item.CarrierName))
                    {
                        if (errorList.Length > 0)
                        {
                            errorList.Append("</br>");
                            errorList.AppendLine(string.Format(Resource.errMsgEnterCarrierNameForMapping, lineNumberOfCSV));
                        }
                        lineNumberOfCSV++;
                    }
                }
            }
        }
        public static void ValidateCarrierDuplicateData(List<ExternalCarrierMappingCsvViewModel> csvCarrierList, StringBuilder errorList)
        {
            var duplicateCarriers = csvCarrierList.GroupBy(c => c.CarrierName)
                                                  .Where(item => item.Count() > 1)
                                                  .Select(k => k.Key)
                                                  .ToList();

            if (duplicateCarriers != null && duplicateCarriers.Any())
            {
                foreach (var carrier in duplicateCarriers)
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                      errorList.AppendLine(string.Format(Resource.errMsgDuplicateCarrierForMapping, carrier));
                }
            }
        }
        public void ValidateCarrierInvalidRecord(UserContext userContext, List<ExternalCarrierMappingCsvViewModel> csvCarrierList, StringBuilder errorList)
        {
            var carriers = csvCarrierList.Select(t => t.CarrierName).Distinct();
            var validCarriers = Context.DataContext.Companies.Where(t => carriers.Contains(t.Name) 
                                                                 &&t.IsActive
                                                                 &&( t.CompanyTypeId ==(int) CompanyType.Carrier
                                                                   ||t.CompanyTypeId ==(int) CompanyType.SupplierAndCarrier
                                                                   ||t.CompanyTypeId ==(int) CompanyType.BuyerSupplierAndCarrier)
                                                                 ).Select(t => t.Name.ToLower()).ToList();
            int lineNumberOfCSV = 1;

            foreach (var item in csvCarrierList)
            {
                if (!validCarriers.Contains(item.CarrierName.ToLower()))
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                        errorList.AppendLine(string.Format(Resource.errMsgInvalidCarrierForMapping, item.CarrierName, lineNumberOfCSV));
                }
                lineNumberOfCSV++;
            }

        }
        #endregion
        #endregion             

        #region External Vechicle Mapping
        public async Task<List<ExternalVehicleMappingViewModel>> GetVehiclesForExternalMapping(UserContext userContext)
        {
            List<ExternalVehicleMappingViewModel> externalCarrierMappings = new List<ExternalVehicleMappingViewModel>();
            try
            {
                externalCarrierMappings = await new FreightServiceDomain(this).GetVehiclesForExternalMapping(userContext.CompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "GetVechiclesForExternalMapping", ex.Message, ex);
            }
            return externalCarrierMappings;

        }

        public async Task<StatusViewModel> SaveExternalVehicleMappings(ExternalVehicleMappingViewModel viewModel, UserContext usercontext)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                viewModel.UserId = usercontext.Id;
                response = await new FreightServiceDomain(this).SaveExternalVehicleMapping(viewModel);
                if (response.StatusCode == (int)Status.Success)
                {
                    response.StatusMessage = Resource.successMsgVehicleExternalMapping;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveExternalVehicleMappings", ex.Message, ex);
            }
            return response;
        }

        #region Vehicle mapping Bulk Upload
        public async Task<StatusViewModel> SaveBulkUploadVehicleMapping(UserContext userContext, string csvText)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "SaveBulkUploadVehicleMapping"))
            {
                try
                {                  
                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                    var engine = new FileHelperEngine<ExternalVehicleMappingCsvViewModel>();
                    var csvVehicleList = engine.ReadString(csvText).ToList();
                     
                    response = ValidateVehicleMappingFileAsync(userContext, csvVehicleList);                    
                    if (response.StatusCode == Status.Success)
                    {                                   
                        var listExternalVehicles = JsonConvert.DeserializeObject<List<ExternalVehicleMappingViewModel>>(JsonConvert.SerializeObject(csvVehicleList));
                        response = await new FreightServiceDomain(this).SaveBulkUploadVehicleMapping(listExternalVehicles, userContext.Id);                        
                    }                 

                    if (response.StatusCode == (int)Status.Success)
                    {
                        response.StatusMessage = Resource.successMsgVehicleExternalMapping;
                    }               
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "SaveBulkUploadCarrierMapping", ex.Message, ex);
                }
                return response;
            }
        }

        public StatusViewModel ValidateVehicleMappingFileAsync(UserContext userContext, List<ExternalVehicleMappingCsvViewModel> csvVehicleList)
        {
            using (var tracer = new Tracer("ExternalEnityMappingsDomain", "ValidateVehicleMappingFileAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    StringBuilder errorList = new StringBuilder();
                    if (csvVehicleList != null && csvVehicleList.Any())
                    {
                        ValidateVehicleBaseRow(csvVehicleList, errorList);
                        if (errorList.Length <= 0)
                        {
                            ValidateVehicleDuplicateData(csvVehicleList, errorList);
                        }
                        if (errorList.Length > 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = errorList.ToString();
                            if (response.StatusMessage.Length > 1000)
                            {
                                response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "No Records found in csv.";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("ExternalEnityMappingsDomain", "ValidateVehicleMappingFileAsync", ex.Message, ex);
                }
                return response;
            }
        }
        public static void ValidateVehicleBaseRow(List<ExternalVehicleMappingCsvViewModel> csvVehicleList, StringBuilder errorList)
        {
            int lineNumberOfCSV = 1;
            if (csvVehicleList != null && csvVehicleList.Any())
            {
                foreach (var item in csvVehicleList)
                {
                    if (string.IsNullOrEmpty(item.TruckName))
                    {
                        if (errorList.Length > 0)
                        {
                            errorList.Append("</br>");
                            errorList.AppendLine(string.Format(Resource.errMsgEnterVehicleNameForMapping, lineNumberOfCSV));
                        }
                        lineNumberOfCSV++;
                    }
                }
            }
        }
        public static void ValidateVehicleDuplicateData(List<ExternalVehicleMappingCsvViewModel> csvVehicleList, StringBuilder errorList)
        {
            var duplicateVehicles = csvVehicleList.GroupBy(c => c.TruckName)
                                                  .Where(item => item.Count() > 1)
                                                  .Select(k => k.Key)
                                                  .ToList();

            if (duplicateVehicles != null && duplicateVehicles.Any())
            {
                foreach (var vehicle in duplicateVehicles)
                {
                    if (errorList.Length > 0)
                    {
                        errorList.Append("</br>");
                    }
                    errorList.AppendLine(string.Format(Resource.errMsgDuplicateVehicleForMapping, vehicle));
                }
            }
        }

        #endregion

        #endregion
        private string RemoveHeaderAndGuidelinesFromFile(string csvText)
        {
            csvText = Regex.Replace(csvText.Trim(), @"\A.*", string.Empty, RegexOptions.IgnoreCase);
            csvText = Regex.Replace(csvText.Trim(), @",\n", string.Empty, RegexOptions.IgnoreCase);
            csvText = csvText.TrimEnd(',');

            return csvText;
        }
    }
}
