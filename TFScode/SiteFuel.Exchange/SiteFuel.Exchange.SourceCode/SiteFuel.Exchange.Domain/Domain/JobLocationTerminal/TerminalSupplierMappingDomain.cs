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
using static Velyo.Google.Services.GeocodingResponseData;

namespace SiteFuel.Exchange.Domain
{
    public class TerminalSupplierMappingDomain : BaseDomain
    {
        public TerminalSupplierMappingDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public TerminalSupplierMappingDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<TerminalSupplierAndDescDropDown> GetTerminalSupplierAndDesc(int country)
        {
            var response = new TerminalSupplierAndDescDropDown();
            using (var tracer = new Tracer("TerminalMappingDomain", "GetTerminalSupplierAndDesc"))
            {
                try
                {
                  Country countryId= (country == (int)Country.USA ? Country.USA : Country.CAN);
                    response.TerminalSupplierList = await Context.DataContext.MstTerminalSuppliers.Where(w => w.IsActive == true && w.CountryId == countryId).Select(s => new DropdownDisplayExtendedItem()
                    {
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Code + " - "+ s.Name
                    }).ToListAsync();
                    response.TerminalDescriptionList = await Context.DataContext.MstTerminalItemDescriptions.Where(w => w.IsActive == true && w.CountryId == countryId).Select(s => new DropdownDisplayExtendedItem()
                    {
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Code + " - " + s.Name
                    }).ToListAsync();
                    return response;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TerminalMappingDomain", "GetTerminalSupplierAndDesc", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StateViewModel> SaveTerminalItemDescription(TerminalSupplierViewModel model, UserContext userContext)
        {
            var response = new StateViewModel(Status.Failed);
            using (var tracer = new Tracer("TerminalSupplierMappingDomain", "SaveTerminalItemDescriptions"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(model.Code))
                        {
                            response.StatusMessage = string.Format(Resource.valMessageRequired, "Description Code");
                            return response;
                        }
                        else if (string.IsNullOrWhiteSpace(model.Name))
                        {
                            response.StatusMessage = string.Format(Resource.valMessageRequired, "Description");
                            return response;
                        }
                        else if (model.ProductTypeId <= 0)
                        {
                            response.StatusMessage = string.Format(Resource.valMessageInvalid, "Product Type");
                            return response;
                        }
                        //var existingItemCode = await Context.DataContext.MstTerminalItemDescriptions.Where(t => t.IsActive && t.Code.ToLower().Trim() == model.Code.ToLower().Trim() && t.CountryId==model.Country).FirstOrDefaultAsync();
                        //if (existingItemCode != null)
                        //{
                        //    response.StatusMessage = string.Format(Resource.errorMessageTerminalDescExists, "code", existingItemCode.Name);
                        //    return response;
                        //}
                        var existingItemDesc = await Context.DataContext.MstTerminalItemDescriptions.Where(t => t.IsActive && t.Name.ToLower().Trim() == model.Name.ToLower().Trim() && t.CountryId == model.Country).FirstOrDefaultAsync();
                        if (existingItemDesc != null)
                        {
                            response.StatusMessage = string.Format(Resource.errorMessageTerminalDescExists, "name ", existingItemDesc.Name);
                            return response;
                        }
                        model.IsActive = true;
                        model.AddedBy = userContext.Id;
                        model.AddedDate = DateTimeOffset.Now;
                        model.UpdatedBy = userContext.Id;
                        model.UpdatedDate = DateTimeOffset.Now;
                        var entity = model.ToItemDescriptionEntity();
                        Context.DataContext.MstTerminalItemDescriptions.Add(entity);

                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageTerminalDescriptionSaved;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusMessage = Resource.errorMessageSaveUpdateTerminalDescriptionFailed;
                        LogManager.Logger.WriteException("TerminalSupplierMappingDomain", "SaveTerminalItemDescriptions", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StateViewModel> UpdateTerminalItemDescription(TerminalSupplierViewModel model, UserContext userContext)
        {
            var response = new StateViewModel(Status.Failed);
            using (var tracer = new Tracer("TerminalSupplierMappingDomain", "UpdateTerminalItemDescriptions"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(model.Code))
                        {
                            response.StatusMessage = string.Format(Resource.valMessageRequired, "Description Code");
                            return response;
                        }
                        else if (string.IsNullOrWhiteSpace(model.Name))
                        {
                            response.StatusMessage = string.Format(Resource.valMessageRequired, "Description");
                            return response;
                        }
                        else if (model.ProductTypeId <= 0)
                        {
                            response.StatusMessage = string.Format(Resource.valMessageInvalid, "Product Type");
                            return response;
                        }
                        //var existingItemCode = await Context.DataContext.MstTerminalItemDescriptions.Where(t => t.Id != model.Id &&   t.IsActive && t.Code.ToLower().Trim() == model.Code.ToLower().Trim() && t.CountryId == model.Country).FirstOrDefaultAsync();
                        //if (existingItemCode != null)
                        //{
                        //    response.StatusMessage = string.Format(Resource.errorMessageTerminalDescExists, "code", existingItemCode.Name);
                        //    return response;
                        //}
                        var existingItemDesc = await Context.DataContext.MstTerminalItemDescriptions.Where(t => t.Id != model.Id &&  t.IsActive && t.Name.ToLower().Trim() == model.Name.ToLower().Trim() && t.CountryId == model.Country).FirstOrDefaultAsync();
                        if (existingItemDesc != null)
                        {
                            response.StatusMessage = string.Format(Resource.errorMessageTerminalDescExists, "name ", existingItemDesc.Name);
                            return response;
                        }
                        var existingEntity = await Context.DataContext.MstTerminalItemDescriptions.Where(t => t.Id == model.Id && t.IsActive).FirstOrDefaultAsync();
                        if (existingEntity != null)
                        {
                            model.IsActive = true;
                            model.AddedBy = existingEntity.Id;
                            model.AddedDate = existingEntity.AddedDate;
                            model.UpdatedBy = userContext.Id;
                            model.UpdatedDate = DateTimeOffset.Now;
                            var entity = model.ToItemDescriptionEntity(existingEntity);
                            Context.DataContext.Entry(entity).State = EntityState.Modified;

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageTerminalDescriptionUpdated;
                        }
                        else
                        {
                            response.StatusMessage = Resource.errorMessageTerminalItemDescriptionNotFound;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusMessage = Resource.errorMessageSaveUpdateTerminalDescriptionFailed;
                        LogManager.Logger.WriteException("TerminalSupplierMappingDomain", "UpdateTerminalItemDescriptions", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StateViewModel> DeleteTerminalItemDescription(int id, UserContext userContext)
        {
            var response = new StateViewModel(Status.Failed);
            using (var tracer = new Tracer("TerminalSupplierMappingDomain", "DeleteTerminalItemDescription"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var existingEntity = await Context.DataContext.MstTerminalItemDescriptions.Where(t => t.Id == id && t.IsActive).FirstOrDefaultAsync();
                        if (existingEntity != null)
                        {
                            existingEntity.IsActive = false;
                            existingEntity.UpdatedBy = userContext.Id;
                            existingEntity.UpdatedDate = DateTimeOffset.Now;
                            Context.DataContext.Entry(existingEntity).State = EntityState.Modified;

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.successMessageTerminalDescriptionDelete, existingEntity.Name);
                        }
                        else
                        {
                            response.StatusMessage = Resource.errorMessageTerminalItemDescriptionNotFound;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusMessage = Resource.errorMessageDeleteTerminalItemDescriptionFailed;
                        LogManager.Logger.WriteException("TerminalSupplierMappingDomain", "DeleteTerminalItemDescription", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StatusViewModel> SaveTerminalItemCodeMapping(List<TerminalItemCodeMappingViewModel> mappingList, int userId, int companyId, List<string> errorInfo = null)
        {
            using (var tracer = new Tracer("TerminalSupplierMappingDomain", "SaveTerminalItemCodeMapping"))
            {
                var response = new StatusViewModel(Status.Failed);
                var isMappingCreated = false;
                if (errorInfo == null)
                    errorInfo = new List<string>();

                if (mappingList == null || !mappingList.Any())
                    return response;

                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var model in mappingList)
                        {
                            var isValid = ValidateTerminalItemCodeDetails(model, errorInfo);
                            if (!isValid)
                            {
                                continue;
                            }
                            else
                            {
                                List<TerminalItemCodeMapping> itemCodeMappingEntityList = new List<TerminalItemCodeMapping>();
                                model.CompanyId = companyId;
                                model.IsActive = true;
                                model.AddedBy = userId;
                                model.AddedDate = DateTimeOffset.Now;
                                model.UpdatedBy = userId;
                                model.UpdatedDate = DateTimeOffset.Now;
                                if (model.ExpiryDate == null)
                                    model.ExpiryDate = new DateTime((DateTime.Now.Year + ApplicationConstants.DefaultTerminalItemCodeMappingExpiryDate), 12, 31);

                                var itemCodes = model.ItemCode.Split(',').ToList();
                                foreach (var itemCode in itemCodes)
                                {
                                    var productTypeId = await Context.DataContext.MstTerminalItemDescriptions.Where(t => t.Id == model.ItemDescriptionId && t.IsActive).Select(t => t.ProductTypeId).FirstOrDefaultAsync();
                                    var existingProductMapping = await Context.DataContext.TerminalItemCodeMappings.Where(t => t.IsActive && t.CompanyId == model.CompanyId && t.ItemCode.ToLower().Trim() == itemCode.ToLower().Trim()).FirstOrDefaultAsync();
                                    if (existingProductMapping != null && existingProductMapping.MstTerminalItemDescription.ProductTypeId != productTypeId)
                                    {
                                        errorInfo.Add(string.Format(Resource.errorMessageItemCodeMappingExistsForProductType, itemCode, existingProductMapping.MstTerminalItemDescription.MstProductType.Name));
                                        continue;
                                    }

                                    var existingMapping = await Context.DataContext.TerminalItemCodeMappings.Where(t => t.IsActive && t.CompanyId == model.CompanyId && t.ItemDescriptionId == model.ItemDescriptionId && t.ItemCode.ToLower().Trim() == itemCode.ToLower().Trim()).FirstOrDefaultAsync();
                                    if (existingMapping != null)
                                    {
                                        errorInfo.Add(string.Format(Resource.errorMessageItemCodeMappingExists, itemCode));
                                        continue;
                                    }

                                    model.ItemCode = itemCode;
                                    var itemCodeMapping = model.ToItemCodeViewModel();

                                    var entity = itemCodeMapping.ToItemCodeMappingEntity();
                                    itemCodeMappingEntityList.Add(entity);
                                }

                                if (itemCodeMappingEntityList.Any())
                                {
                                    Context.DataContext.TerminalItemCodeMappings.AddRange(itemCodeMappingEntityList);
                                    await Context.CommitAsync();
                                    isMappingCreated = true;
                                }
                            }
                        }

                        if (isMappingCreated)
                        {
                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            errorInfo.Add(Resource.successMessageItemCodeMappingSaved);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        errorInfo.Add(Resource.errorMessageSaveTerminalItemCodeMappingFailed);
                        LogManager.Logger.WriteException("TerminalSupplierMappingDomain", "SaveTerminalItemCodeMapping", ex.Message, ex);
                    }

                    if (errorInfo.Any())
                    {
                        response.StatusMessage = errorInfo.First();
                        response.ResponseData = errorInfo;
                    }
                }

                return response;
            }
        }

        private bool ValidateTerminalItemCodeDetails(TerminalItemCodeMappingViewModel model, List<string> errorInfo)
        {
            bool isValid = true;
            if (model.TerminalSupplierId <= 0)
            {
                errorInfo.Add(string.Format(Resource.valMessageInvalid, Resource.lblTerminalSupplier));
                isValid = false;
            }

            if (model.ItemDescriptionId <= 0)
            {
                errorInfo.Add(string.Format(Resource.valMessageInvalid, Resource.lblTerminalItemDescription));
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(model.ItemCode))
            {
                errorInfo.Add(string.Format(Resource.valMessageRequired, Resource.lblTerminalItemCode));
                isValid = false;
            }
            else
            {
                List<string> itemCodes = new List<string>();
                try
                {
                    itemCodes = model.ItemCode.Split(',').ToList();
                }
                catch
                {
                    errorInfo.Add(Resource.errorMessageInvalidItemCode);
                    isValid = false;
                }
            }

            if (model.EffectiveDate != null && model.EffectiveDate.Date > DateTimeOffset.Now.Date)
            {
                errorInfo.Add(Resource.errorMessageEffectiveDate);
                isValid = false;
            }

            if (model.Id == 0 && model.ExpiryDate != null && model.ExpiryDate.Value.Date < DateTimeOffset.Now.Date)
            {
                errorInfo.Add(Resource.errorMessageExpiryDate);
                isValid = false;
            }           

            return isValid;
        }

        private bool ValidateBulkUploadTerminalItemCodeMapping(HttpPostedFileBase csvFile, List<string> errorInfo)
        {
            bool isValid = true;
            int rowNumber = 2;
            var csvMappingList = new List<TerminalItemCodeMappingBulkCsvViewModel>();
            using (var stream = new MemoryStream())
            {
                csvFile.InputStream.CopyTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                    {
                        csv.Configuration.RegisterClassMap<TerminalItemCodeMappingBulkCsvViewModelMap>();
                        try
                        {
                            csvMappingList = csv.GetRecords<TerminalItemCodeMappingBulkCsvViewModel>().ToList();
                        }
                        catch (Exception)
                        {
                            csvMappingList = new List<TerminalItemCodeMappingBulkCsvViewModel>();
                        }
                    }
                }
            }

            if (csvMappingList.Any())
            {
                foreach (var model in csvMappingList)
                {
                    if (string.IsNullOrWhiteSpace(model.TerminalSupplier))
                    {
                        errorInfo.Add(string.Format(Resource.valMessageRequiredBulkUpload, Resource.lblTerminalSupplier, rowNumber));
                        isValid = false;
                    }
                    
                    if (string.IsNullOrWhiteSpace(model.TerminalItemDescription))
                    {
                        errorInfo.Add(string.Format(Resource.valMessageRequiredBulkUpload, Resource.lblTerminalItemDescription, rowNumber));
                        isValid = false;
                    }

                    if (string.IsNullOrWhiteSpace(model.TerminalItemCode))
                    {
                        errorInfo.Add(string.Format(Resource.valMessageRequiredBulkUpload, Resource.lblTerminalItemCode, rowNumber));
                        isValid = false;
                    }
                    else
                    {
                        List<string> itemCodes = new List<string>();
                        try
                        {
                            itemCodes = model.TerminalItemCode.Split(',').ToList();
                        }
                        catch
                        {
                            errorInfo.Add(string.Format(Resource.errorMessageInvalidItemCode, rowNumber));
                            isValid = false;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(model.EffectiveDate))
                    {
                        errorInfo.Add(string.Format(Resource.valMessageRequiredBulkUpload, Resource.lblEffectiveDate, rowNumber));
                        isValid = false;
                    }
                    else
                    {
                        DateTimeOffset effectiveDate;
                        var isValidEffectiveDate = DateTimeOffset.TryParse(model.EffectiveDate, out effectiveDate);
                        if(!isValidEffectiveDate)
                        {
                            errorInfo.Add(string.Format(Resource.valMessageInvalidBulkUpload, Resource.lblEffectiveDate, rowNumber));
                            isValid = false;
                        }
                        else if(effectiveDate.Date > DateTimeOffset.Now.Date)
                        {
                            errorInfo.Add(string.Format(Resource.errorMessageEffectiveDateBulkUpload, rowNumber));
                            isValid = false;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(model.ExpiryDate))
                    {
                        DateTimeOffset expiryDate;
                        var isValidExpiryDate = DateTimeOffset.TryParse(model.ExpiryDate, out expiryDate);
                        if (!isValidExpiryDate)
                        {
                            errorInfo.Add(string.Format(Resource.errorMessageExpiryDateBulkUpload, rowNumber));
                            isValid = false;
                        }
                        else if(expiryDate.Date < DateTimeOffset.Now.Date)
                        {
                            errorInfo.Add(string.Format(Resource.errorMessageExpiryDateBulkUpload, rowNumber));
                            isValid = false;
                        }
                    }

                    rowNumber++;
                }
            }

            return isValid;
        }

        public async Task<StateViewModel> SaveTerminalSupplier(TerminalSupplierViewModel model, UserContext userContext)
        {
            var response = new StateViewModel(Status.Failed);
            using (var tracer = new Tracer("TerminalSupplierMappingDomain", "SaveTerminalSupplier"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(model.Code))
                        {
                            response.StatusMessage = string.Format(Resource.valMessageRequired, "Supplier Code");
                            return response;
                        }
                        else if (string.IsNullOrWhiteSpace(model.Name))
                        {
                            response.StatusMessage = string.Format(Resource.valMessageRequired, "Supplier Name");
                            return response;
                        }


                        var existingCode = await Context.DataContext.MstTerminalSuppliers.Where(t => t.IsActive && t.Code.ToLower().Trim() == model.Code.ToLower().Trim() && t.CountryId==model.Country).FirstOrDefaultAsync();
                        if (existingCode != null)
                        {
                            response.StatusMessage = string.Format(Resource.errorMessageTerminalSupplierExists, "code", model.Code);
                            return response;
                        }
                        var existingName = await Context.DataContext.MstTerminalSuppliers.Where(t => t.IsActive && t.Name.ToLower().Trim() == model.Name.ToLower().Trim() && t.CountryId == model.Country).FirstOrDefaultAsync();
                        if (existingName != null)
                        {
                            response.StatusMessage = string.Format(Resource.errorMessageTerminalSupplierExists, "name", model.Name);
                            return response;
                        }
                        
                        model.IsActive = true;
                        model.AddedBy = userContext.Id;
                        model.AddedDate = DateTimeOffset.Now;
                        model.UpdatedBy = userContext.Id;
                        model.UpdatedDate = DateTimeOffset.Now;
                        var entity = model.ToTerminalSupplierEntity();
                        Context.DataContext.MstTerminalSuppliers.Add(entity);

                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageTerminalSupplierDescriptionSaved;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageSaveUpdateTerminalDescriptionFailed;
                        LogManager.Logger.WriteException("TerminalSupplierMappingDomain", "SaveTerminalSupplier", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StateViewModel> UpdateTerminalSupplier(TerminalSupplierViewModel model, UserContext userContext)
        {
            var response = new StateViewModel();
            using (var tracer = new Tracer("TerminalSupplierMappingDomain", "UpdateTerminalSupplier"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(model.Code))
                        {
                            response.StatusMessage = string.Format(Resource.valMessageRequired, "Supplier Code");
                            return response;
                        }
                        else if (string.IsNullOrWhiteSpace(model.Name))
                        {
                            response.StatusMessage = string.Format(Resource.valMessageRequired, "Supplier Name");
                            return response;
                        }


                        var existingCode = await Context.DataContext.MstTerminalSuppliers.Where(t => t.Id != model.Id && t.IsActive && t.Code.ToLower().Trim() == model.Code.ToLower().Trim() && t.CountryId == model.Country).FirstOrDefaultAsync();
                        if (existingCode != null)
                        {

                            response.StatusMessage = string.Format(Resource.errorMessageTerminalSupplierExists, "code", model.Code);
                            return response;
                        }
                        var existingName = await Context.DataContext.MstTerminalSuppliers.Where(t => t.Id != model.Id && t.IsActive &&  t.Name.ToLower().Trim() == model.Name.ToLower().Trim() && t.CountryId==model.Country).FirstOrDefaultAsync();
                        if (existingName != null)
                        {

                            response.StatusMessage = string.Format(Resource.errorMessageTerminalSupplierExists,"name", model.Name);
                            return response;
                        }
                        var existingEntity = await Context.DataContext.MstTerminalSuppliers.Where(t => t.Id == model.Id && t.IsActive).FirstOrDefaultAsync();
                        if (existingEntity != null)
                        {
                            model.IsActive = true;
                            model.AddedBy = existingEntity.Id;
                            model.AddedDate = existingEntity.AddedDate;
                            model.UpdatedBy = userContext.Id;
                            model.UpdatedDate = DateTimeOffset.Now;
                            var entity = model.ToTerminalSupplierEntity(existingEntity);
                            Context.DataContext.Entry(entity).State = EntityState.Modified;

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageTerminalSupplierUpdated;
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errorMessageDeleteTerminalSupplierFailed;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageSaveUpdateTerminalSupplierFailed;
                        LogManager.Logger.WriteException("TerminalSupplierMappingDomain", "UpdateTerminalSupplier", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StateViewModel> DeleteTerminalSupplier(int id, UserContext userContext)
        {
            var response = new StateViewModel();
            using (var tracer = new Tracer("TerminalSupplierMappingDomain", "DeleteTerminalSupplier"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var existingEntity = await Context.DataContext.MstTerminalSuppliers.Where(t => t.Id == id && t.IsActive).FirstOrDefaultAsync();
                        if (existingEntity != null)
                        {
                            existingEntity.IsActive = false;
                            existingEntity.UpdatedBy = userContext.Id;
                            existingEntity.UpdatedDate = DateTimeOffset.Now;
                            Context.DataContext.Entry(existingEntity).State = EntityState.Modified;

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.successMessageTerminalSupplierDelete, existingEntity.Name);
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errorMessageDeleteTerminalSupplierFailed;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageSaveUpdateTerminalSupplierFailed;
                        LogManager.Logger.WriteException("TerminalSupplierMappingDomain", "DeleteTerminalSupplier", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public  List<TerminalSupplierViewModel> GetTerminalSupplierGridAsync(TerminalSupplierViewModel model)
        {
            List<TerminalSupplierViewModel> response = new List<TerminalSupplierViewModel>();
            try
            {
               var res = Context.DataContext.MstTerminalSuppliers.Where(w => w.IsActive == true && w.CountryId == model.Country).OrderByDescending(t => t.Id).ToList();
                foreach (var item in res)
                {
                    
                    response.Add(item.ToTerminalSupplierViewModel());
                 }
            
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetTerminalSupplierGridAsync", ex.Message, ex);
            }

            return response;
        }
        public  List<TerminalSupplierViewModel> GetTerminalItemDescGridAsync(TerminalSupplierViewModel model)
        {
            List<TerminalSupplierViewModel> response = new List<TerminalSupplierViewModel>();
            try
            {
               var res =  Context.DataContext.MstTerminalItemDescriptions.Include(td => td.MstProductType)
                    .Where(w => w.IsActive == true && w.CountryId == model.Country).OrderByDescending(t => t.Id).ToList();
                foreach (var item in res)
                {
                    response.Add(item.ToTerminalItemDescGridViewModel());
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetTerminalSupplierGridAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateTerminalItemCodeMapping(TerminalItemCodeMappingViewModel model, int userId, int companyId)
        {
            using (var tracer = new Tracer("TerminalSupplierMappingDomain", "UpdateTerminalItemCodeMapping"))
            {
                var response = new StatusViewModel(Status.Failed);
                var errorInfo = new List<string>();

                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var isValid = ValidateTerminalItemCodeDetails(model, errorInfo);
                        if (!isValid)
                        {
                            response.StatusMessage = errorInfo.First();
                            return response;
                        }
                        else
                        {
                            var productTypeId = await Context.DataContext.MstTerminalItemDescriptions.Where(t => t.Id == model.ItemDescriptionId && t.IsActive).Select(t => t.ProductTypeId).FirstOrDefaultAsync();
                            var existingProductType = await Context.DataContext.TerminalItemCodeMappings.Where(t => t.IsActive && t.CompanyId == companyId && t.ItemCode.ToLower().Trim() == model.ItemCode.ToLower().Trim()).FirstOrDefaultAsync();
                            if (existingProductType != null && productTypeId != existingProductType.MstTerminalItemDescription.ProductTypeId)
                            {
                                response.StatusMessage = string.Format(Resource.errorMessageItemCodeMappingExistsForProductType, model.ItemCode, existingProductType.MstTerminalItemDescription.MstProductType.Name);
                                return response;
                            }

                            var existingMapping = await Context.DataContext.TerminalItemCodeMappings.Where(t => t.Id != model.Id && t.CompanyId == companyId && t.IsActive && t.TerminalSupplierId == model.TerminalSupplierId && t.ItemDescriptionId == model.ItemDescriptionId && t.ItemCode.ToLower().Trim() == model.ItemCode.ToLower().Trim()).FirstOrDefaultAsync();
                            if (existingMapping != null)
                            {
                                response.StatusMessage = string.Format(Resource.errorMessageItemCodeMappingExists, model.ItemCode);
                                return response;
                            }

                            var mapping = await Context.DataContext.TerminalItemCodeMappings.Where(t => t.Id == model.Id && t.CompanyId == companyId && t.IsActive).FirstOrDefaultAsync();
                            if (mapping != null)
                            {
                                model.CompanyId = mapping.CompanyId;
                                model.IsActive = mapping.IsActive;
                                model.AddedBy = mapping.AddedBy;
                                model.AddedDate = mapping.AddedDate;
                                model.UpdatedBy = userId;
                                model.UpdatedDate = DateTimeOffset.Now;
                                if (mapping.ExpiryDate == null)
                                    model.ExpiryDate = new DateTime((DateTime.Now.Year + ApplicationConstants.DefaultTerminalItemCodeMappingExpiryDate), 12, 31);
                                var itemCodeMapping = model.ToItemCodeViewModel();

                                var entity = itemCodeMapping.ToItemCodeMappingEntity(mapping);
                                Context.DataContext.Entry(entity).State = EntityState.Modified;

                                await Context.CommitAsync();
                                transaction.Commit();

                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.successMessageItemCodeMappingUpdate;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusMessage = Resource.errorMessageSaveTerminalItemCodeMappingFailed;
                        LogManager.Logger.WriteException("TerminalSupplierMappingDomain", "UpdateTerminalItemCodeMapping", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StateViewModel> DeleteTerminalItemCodeMapping(int id, UserContext userContext)
        {
            var response = new StateViewModel(Status.Failed);
            using (var tracer = new Tracer("TerminalSupplierMappingDomain", "DeleteTerminalItemCodeMapping"))
            {
                try
                {
                    var existingEntity = await Context.DataContext.TerminalItemCodeMappings.Where(t => t.Id == id && t.IsActive).FirstOrDefaultAsync();
                    if (existingEntity != null)
                    {
                        existingEntity.IsActive = false;
                        existingEntity.UpdatedBy = userContext.Id;
                        existingEntity.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(existingEntity).State = EntityState.Modified;

                        await Context.CommitAsync();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMessageTerminalItemCodeMappingDelete, existingEntity.ItemCode);
                    }
                    else
                    {
                        response.StatusMessage = Resource.errorMessageDeleteTerminalItemCodeNotFound;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errorMessageDeleteTerminalItemCodeMappingFailed;
                    LogManager.Logger.WriteException("TerminalSupplierMappingDomain", "DeleteTerminalItemCodeMapping", ex.Message, ex);
                }

                return response;
            }
        }

        public List<DropdownDisplayExtendedItem> GetProductTypeDropDown()
        {
            List<DropdownDisplayExtendedItem> response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response =  Context.DataContext.MstProductTypes.Where(w => w.IsActive == true)
                                 .Select(s => new DropdownDisplayExtendedItem
                                 {
                                  Id=s.Id,
                                  Name=s.Name
                                 }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuperAdminDomain", "GetProductTypeDropDown", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> UploadTerminalItemCodeMappingFileToBlob(UserContext userContext, HttpPostedFileBase csvFile)
        {
            using (var tracer = new Tracer("TerminalSupplierMappingDomain", "UploadTerminalItemCodeMappingFileToBlob"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var errorInfo = new List<string>();
                    var azureBlob = new AzureBlobStorage();
                    var isValid = ValidateBulkUploadTerminalItemCodeMapping(csvFile, errorInfo);
                    if (isValid)
                    {
                        var filePath = await azureBlob.SaveBlobAsync(csvFile.InputStream, GenerateFileName(userContext.CompanyId, userContext.Id), BlobContainerType.ProductMappingBulkUpload.ToString().ToLower());
                        if (!string.IsNullOrWhiteSpace(filePath))
                        {
                            var queueDomain = new QueueMessageDomain();
                            var queueRequest = GetQueueEventForTerminalItemCodeMappingFileUpload(userContext, filePath);
                            var queueId = queueDomain.EnqeueMessage(queueRequest);

                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.successMessageProductMappingBulkWithRequestNo, string.Concat(Constants.TFXTerminalItemCodeMappingBulkUploadSuffix, queueId.ToString("000")));
                        }
                        else
                            response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    }
                    else
                    {
                        StringBuilder str = new StringBuilder();
                        errorInfo.ForEach(t => str.Append(t));
                        response.StatusMessage = str.ToString();
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    LogManager.Logger.WriteException("TerminalSupplierMappingDomain", "UploadTerminalItemCodeMappingFileToBlob", ex.Message, ex);
                }
                return response;
            }
        }

        private QueueMessageViewModel GetQueueEventForTerminalItemCodeMappingFileUpload(UserContext userContext, string blobStoragePath)
        {
            var jsonViewModel = new TerminalItemCodeMappingBulkUploadModel();
            jsonViewModel.UserId = userContext.Id;
            jsonViewModel.CompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.TerminalItemCodeMappingBulkUpload,
                JsonMessage = json
            };
        }

        private string GenerateFileName(int companyId, int userId)
        {
            return string.Concat(values: Constants.TerminalItemCodeMappingBulk + Resource.lblSingleHyphen + companyId + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
        }

        public async Task<List<string>> ProcessTerminalItemCodeMappingBulkUploadFile(TerminalItemCodeMappingBulkUploadModel bulkUploadViewModel)
        {
            List<string> errorInfo = new List<string>();
            using (var tracer = new Tracer("TerminalSupplierMappingDomain", "ProcessTerminalItemCodeMappingBulkUploadFile"))
            {
                StringBuilder processMessage = new StringBuilder();
                var csvMappingList = new List<TerminalItemCodeMappingBulkCsvViewModel>();
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
                                    csv.Configuration.RegisterClassMap<TerminalItemCodeMappingBulkCsvViewModelMap>();
                                    try
                                    {
                                        csvMappingList = csv.GetRecords<TerminalItemCodeMappingBulkCsvViewModel>().ToList();
                                    }
                                    catch (Exception)
                                    {
                                        csvMappingList = new List<TerminalItemCodeMappingBulkCsvViewModel>();
                                    }
                                }
                            }
                        }

                        
                        if (csvMappingList.Any())
                        {
                            int i = 1;                           
                            var mappingListToProcess = new List<TerminalItemCodeMappingViewModel>();

                            // add row number
                            csvMappingList.ForEach(t => t.RowNumber = ++i);
                            foreach (var mapping in csvMappingList)
                            {
                                // format fields
                                var isValidRow = true;
                                var terminalSupplier = mapping.TerminalSupplier.ToFormattedStringInLowerCase();
                                var terminalItemDesc = mapping.TerminalItemDescription.ToFormattedStringInLowerCase();
                                //var productType = mapping.TFXProductType.ToFormattedStringInLowerCase();
                                
                                DateTimeOffset effectiveDate = DateTimeOffset.Now;
                                if (string.IsNullOrWhiteSpace(mapping.EffectiveDate))
                                {
                                    isValidRow = false;
                                    errorInfo.Add(string.Format(Resource.valMessageInvalidBulkUpload, Resource.lblEffectiveDate, mapping.RowNumber));
                                }
                                else
                                    DateTimeOffset.TryParse(mapping.EffectiveDate, out effectiveDate);

                                DateTimeOffset expiryDate; 
                                if (!string.IsNullOrWhiteSpace(mapping.ExpiryDate))
                                {
                                    DateTimeOffset.TryParse(mapping.ExpiryDate, out expiryDate);
                                }
                                else
                                {
                                    expiryDate = new DateTime((DateTime.Now.Year + ApplicationConstants.DefaultTerminalItemCodeMappingExpiryDate), 12, 31);
                                }

                                var mstTerminalSupplier = await Context.DataContext.MstTerminalSuppliers.Where(t => t.IsActive && t.Name.ToLower().Trim() == terminalSupplier).FirstOrDefaultAsync();
                                if (mstTerminalSupplier != null)
                                {
                                    mapping.TerminalSupplierId = mstTerminalSupplier.Id;
                                }
                                else
                                {
                                    isValidRow = false;
                                    var errorMsg = string.Format(Resource.errorMessageFileProcessRecordNotFoundInTFX, Resource.lblTerminalSupplier, mapping.TerminalSupplier, mapping.RowNumber);
                                    errorInfo.Add(errorMsg);
                                }

                                var mstTerminalItemDesc = await Context.DataContext.MstTerminalItemDescriptions.Where(t => t.IsActive && t.Name.ToLower().Trim() == terminalItemDesc).FirstOrDefaultAsync();
                                if (mstTerminalItemDesc != null)
                                {
                                    mapping.ItemDescriptionId = mstTerminalItemDesc.Id;
                                }
                                else
                                {
                                    isValidRow = false;
                                    var errorMsg = string.Format(Resource.errorMessageFileProcessRecordNotFoundInTFX, Resource.lblTerminalItemDescription, mapping.TerminalItemDescription, mapping.RowNumber);
                                    errorInfo.Add(errorMsg);
                                }

                                if (isValidRow)
                                {
                                    mapping.CompanyId = bulkUploadViewModel.CompanyId;
                                    var model = mapping.ToItemCodeMappingViewModel();

                                    model.EffectiveDate = effectiveDate;
                                    model.ExpiryDate = expiryDate;                                    
                                    mappingListToProcess.Add(model);                                    
                                }
                            }

                            if(mappingListToProcess.Any())
                            {
                                var response = await SaveTerminalItemCodeMapping(mappingListToProcess, bulkUploadViewModel.UserId, bulkUploadViewModel.CompanyId, errorInfo);
                                if(response.StatusCode == Status.Success)
                                {
                                    errorInfo.RemoveAll(t => t == Resource.successMessageItemCodeMappingSaved);
                                    var msg = "<p class='color-green'>" + string.Format(Resource.successMessageTerminalItemCodeBulkUpload, mappingListToProcess.Count) + "</p><br />";
                                    errorInfo.Add(msg);
                                }
                            }
                            else
                            {
                                errorInfo.Add(Resource.errorMessageNoRecordsFoudItemCodeMapping);
                            }
                        }
                        else
                        {
                            errorInfo.Add(Resource.errorMessageNoRecordsFoudItemCodeMapping);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("TerminalSupplierMappingDomain", "ProcessTerminalItemCodeMappingBulkUploadFile", ex.Message, ex);
                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Resource.errorProcessTerminalItemCodeMappingBulkUploadFile);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }

                return errorInfo;
            }
        }        
    }
}
