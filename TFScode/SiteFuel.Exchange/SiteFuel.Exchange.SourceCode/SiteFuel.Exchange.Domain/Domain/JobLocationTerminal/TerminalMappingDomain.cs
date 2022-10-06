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
using static Velyo.Google.Services.GeocodingResponseData;

namespace SiteFuel.Exchange.Domain
{
    public class TerminalMappingDomain : BaseDomain
    {
        public TerminalMappingDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public TerminalMappingDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<List<TerminalMappingGridViewModel>> GetTerminalMappingGrid(int companyId, int SelectedCountryId, int timeout = 30)
        {
            var response = new List<TerminalMappingGridViewModel>();
            using (var tracer = new Tracer("TerminalMappingDomain", "GetTerminalMappingGrid"))
            {
                try
                {
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetTerminalMappingGridAsync(companyId, SelectedCountryId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TerminalMappingDomain", "GetTerminalMappingGrid", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> SaveTerminalMapping(TerminalMappingGridViewModel terminalMapping, UserContext userContext)
        {
            var response = new StatusViewModel();
            if (ValidateTerminalMappingModel(terminalMapping, response))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        TerminalCompanyAlias existingMapping = null;
                        
                        if (!terminalMapping.TerminalSupplierId.HasValue || terminalMapping.TerminalSupplierId ==null 
                            || terminalMapping.TerminalSupplierId.Value==0 )
                        {
                            if (terminalMapping.IsBulkPlant)
                                existingMapping = await Context.DataContext.TerminalCompanyAliases.FirstOrDefaultAsync(t => t.BulkPlantId == terminalMapping.TerminalId && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive && t.IsBulkPlant == terminalMapping.IsBulkPlant && t.TerminalSupplierId ==null);
                            else
                                existingMapping = await Context.DataContext.TerminalCompanyAliases.FirstOrDefaultAsync(t => t.TerminalId == terminalMapping.TerminalId && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive && t.IsBulkPlant == terminalMapping.IsBulkPlant && t.TerminalSupplierId == null);

                            if (existingMapping == null)
                            {
                                await SaveMapping(terminalMapping, userContext);

                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.successMessageAddedTerminalMapping;
                            }
                            else
                            {
                                // edit mapping
                                await UpdateMapping(terminalMapping, userContext);
                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.successMessageUpdateTerminalMapping;
                            }
                            transaction.Commit();

                        }
                        else if (terminalMapping.TerminalSupplierId.HasValue && terminalMapping.TerminalSupplierId.Value >0)
                        {
                            await SaveMapping(terminalMapping, userContext);
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageAddedTerminalMapping;
                            transaction.Commit();
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageUpdateProductMapping;
                        LogManager.Logger.WriteException("TerminalMappingDomain", "SaveTerminalMapping", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        private async Task UpdateMapping(TerminalMappingGridViewModel terminalMapping, UserContext userContext)
        {
            // inactive existing mapping 
            TerminalCompanyAlias existingMapping = null;

            if (!terminalMapping.TerminalSupplierId.HasValue || terminalMapping.TerminalSupplierId == null
                            || terminalMapping.TerminalSupplierId.Value == 0)
            {
                if (terminalMapping.IsBulkPlant)
                    existingMapping = await Context.DataContext.TerminalCompanyAliases.FirstOrDefaultAsync(t => t.BulkPlantId == terminalMapping.TerminalId && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive && t.IsBulkPlant == terminalMapping.IsBulkPlant);
                else
                    existingMapping = await Context.DataContext.TerminalCompanyAliases.FirstOrDefaultAsync(t => t.TerminalId == terminalMapping.TerminalId && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive && t.IsBulkPlant == terminalMapping.IsBulkPlant);

                if (existingMapping != null)
                {
                    existingMapping.UpdatedBy = userContext.Id;
                    existingMapping.UpdatedDate = DateTimeOffset.Now;
                    existingMapping.CreatedByCompanyId = userContext.CompanyId;
                    existingMapping.AssignedTerminalId = terminalMapping.AssignedTerminalId.Trim();
                }
            }
            
          
            //}
            Context.DataContext.Entry(existingMapping).State = EntityState.Modified;
            await Context.CommitAsync();
        }

        private async Task SaveMapping(TerminalMappingGridViewModel terminalMapping, UserContext userContext)
        {
            try
            {
                TerminalCompanyAlias terminalCompanyAlias = new TerminalCompanyAlias();

                if (terminalCompanyAlias != null)
                {
                    terminalCompanyAlias.IsActive = true;
                    terminalCompanyAlias.CreatedBy = userContext.Id;
                    terminalCompanyAlias.CreatedDate = DateTimeOffset.Now;
                    terminalCompanyAlias.UpdatedBy = userContext.Id;
                    terminalCompanyAlias.UpdatedDate = DateTimeOffset.Now;
                    terminalCompanyAlias.AssignedTerminalId = terminalMapping.AssignedTerminalId.Trim();
                   // terminalCompanyAlias.AssignedTerminalSupplierId = string.IsNullOrWhiteSpace(terminalMapping.AssignedTermSupplierId) ? terminalMapping.AssignedTermSupplierId : terminalMapping.AssignedTermSupplierId.Trim();
                    if (terminalMapping.TerminalSupplierId.HasValue && terminalMapping.TerminalSupplierId.Value > 0)
                    {
                        terminalCompanyAlias.TerminalSupplierId = terminalMapping.TerminalSupplierId;
                    }
                    
                    if(terminalMapping.IsBulkPlant)
                        terminalCompanyAlias.BulkPlantId = terminalMapping.TerminalId;
                    else
                    terminalCompanyAlias.TerminalId = terminalMapping.TerminalId;
                    
                    terminalCompanyAlias.CreatedByCompanyId = userContext.CompanyId;
                    terminalCompanyAlias.IsBulkPlant = terminalMapping.IsBulkPlant;
                    Context.DataContext.TerminalCompanyAliases.Add(terminalCompanyAlias);
                    await Context.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TerminalMappingDomain", "SaveMapping", ex.Message, ex);
            }

        }

        public async Task<StatusViewModel> CheckDuplicateTerminalId(TerminalMappingGridViewModel model, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("TerminalMappingDomain", "CheckDuplicateTerminalId"))
            {
                try
                {

                    if (!model.TerminalSupplierId.HasValue || model.TerminalSupplierId==null || model.TerminalSupplierId.Value == 0)
                    {
                        response = await ValidateTerminalMapping(model, userContext,false);
                    }
                    else if (model.TerminalSupplierId.HasValue && model.TerminalSupplierId.Value > 0)
                    {
                        response = await ValidateTerminalMapping(model, userContext, true);
                    }


                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TerminalMappingDomain", "CheckDuplicateTerminalId", ex.Message, ex);
                }
                return response;
            }
        }
        private bool ValidateTerminalMappingModel(TerminalMappingGridViewModel model, StatusViewModel status)
        {
            var response = false;
            if (model == null)
            {
                status.StatusMessage = Resource.errMessageInvalidData;
                return response;
            }

            if (string.IsNullOrWhiteSpace(model.AssignedTerminalId) && model.TerminalId == 0)
            {
                status.StatusMessage = Resource.errorMessageTerminalId;
                return response;
            }

            return response = true;
        }


        public async Task<StatusViewModel> DeleteTerminalMappingById(int mappingId, int companyId, UserContext userContext)
        {
            using (var tracer = new Tracer("TerminalMappingDomain", "DeleteTerminalMappingById"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var existingMapping = await Context.DataContext.TerminalCompanyAliases.FirstOrDefaultAsync(t => t.Id == mappingId && t.CreatedByCompanyId == companyId && t.IsActive);
                    if (existingMapping != null)
                    {
                        existingMapping.IsActive = false;
                        existingMapping.UpdatedBy = userContext.Id;
                        existingMapping.UpdatedDate = DateTimeOffset.Now;

                        Context.DataContext.Entry(existingMapping).State = EntityState.Modified;
                        await Context.CommitAsync();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageTerminalMappingDeleted;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageTerminalMappingNotFound;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errorMessageFailedToDeleteTerminalMapping;
                    LogManager.Logger.WriteException("TerminalMappingDomain", "DeleteTerminalMappingById", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<StatusViewModel> UpdateTerminalId(TerminalMappingGridViewModel mapping, UserContext userContext)
        {
            using (var tracer = new Tracer("TerminalMappingDomain", "UpdateTerminalId"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var existingMapping = await Context.DataContext.TerminalCompanyAliases.FirstOrDefaultAsync(t => t.Id == mapping.Id && t.CreatedByCompanyId == mapping.CreatedByCompanyId && t.IsActive && t.IsBulkPlant==mapping.IsBulkPlant);
                    if (existingMapping != null)
                    {
                        if (existingMapping.AssignedTerminalId.ToLower().Trim() == mapping.AssignedTerminalId.ToLower().Trim())
                        {
                            existingMapping.AssignedTerminalId = mapping.AssignedTerminalId.Trim();

                            if ((string.IsNullOrWhiteSpace(existingMapping.AssignedTerminalId) || existingMapping.AssignedTerminalId == Resource.lblHyphen))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errorMessageTerminalNamesValidation;
                                return response;
                            }

                            existingMapping.UpdatedBy = userContext.Id;
                            existingMapping.UpdatedDate = DateTimeOffset.Now;

                            Context.DataContext.Entry(existingMapping).State = EntityState.Modified;
                            await Context.CommitAsync();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageUpdateTerminalMapping;
                        }
                        else
                        {
                            existingMapping.IsActive = false;
                            await SaveMapping(mapping, userContext);

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageUpdateTerminalMapping;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessageTerminalMappingNotFound;
                    }

                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errorMessageFailedToDeleteTerminalMapping;
                    LogManager.Logger.WriteException("TerminalMappingDomain", "UpdateTerminalId", ex.Message, ex);
                }
                return response;
            }
        }

        
        public async Task<StatusViewModel> ValidateTerminalMapping(TerminalMappingGridViewModel viewModel, UserContext userContext,  bool IsValidateTerminalSupplier = false)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                TerminalCompanyAlias existingMapping = null;
                if (!IsValidateTerminalSupplier)
                {
                    if (viewModel.IsBulkPlant)
                        existingMapping = await Context.DataContext.TerminalCompanyAliases.FirstOrDefaultAsync(t => t.BulkPlantId == viewModel.TerminalId && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive && t.IsBulkPlant == viewModel.IsBulkPlant && t.TerminalSupplierId ==null);
                    else
                        existingMapping = await Context.DataContext.TerminalCompanyAliases.FirstOrDefaultAsync(t => t.TerminalId == viewModel.TerminalId && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive && t.IsBulkPlant == viewModel.IsBulkPlant && t.TerminalSupplierId == null);


                    if (existingMapping != null && viewModel.Id == 0)
                    {
                        response.StatusCode = Status.Warning;
                        response.StatusMessage = Resource.warningTerminalExist;
                        return response;
                    }

                    if (!string.IsNullOrEmpty(viewModel.AssignedTerminalId))
                    {
                        existingMapping = Context.DataContext.TerminalCompanyAliases.FirstOrDefault(t => t.AssignedTerminalId.ToLower().Trim() == viewModel.AssignedTerminalId.ToLower().Trim() && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive == true && t.IsBulkPlant == viewModel.IsBulkPlant && t.TerminalSupplierId == null);

                        if (existingMapping != null && existingMapping.Id != viewModel.Id)
                        {
                            response.StatusCode = Status.Warning;
                            response.StatusMessage = Resource.warningMyTerminalIdExist;                          
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                        }
                    }
                }
                
                else if (IsValidateTerminalSupplier)
                {
                    if (viewModel.IsBulkPlant)
                    {
                        existingMapping = await Context.DataContext.TerminalCompanyAliases
                                                .FirstOrDefaultAsync(t => t.BulkPlantId == viewModel.TerminalId
                                                && t.CreatedByCompanyId == userContext.CompanyId
                                                && t.IsActive && t.IsBulkPlant == viewModel.IsBulkPlant
                                                && t.TerminalSupplierId == viewModel.TerminalSupplierId.Value);
                    }
                    else
                    {
                        existingMapping = await Context.DataContext.TerminalCompanyAliases.
                                           FirstOrDefaultAsync(t => t.TerminalId == viewModel.TerminalId
                                           && t.CreatedByCompanyId == userContext.CompanyId
                                           && t.IsActive && t.IsBulkPlant == viewModel.IsBulkPlant
                                           && t.TerminalSupplierId == viewModel.TerminalSupplierId.Value);
                    }
                    if (existingMapping != null && viewModel.Id == 0)
                    {
                        response.StatusCode = Status.Warning;
                        response.StatusMessage = "Mapping exists for Terminal ,Terminal Supplier";
                        return response;
                    }
                    // Maintain unique combination of TerminalId and TerminalSupplier always and assigned terminal id
                    if (viewModel.IsBulkPlant)
                    {
                        existingMapping = await Context.DataContext.TerminalCompanyAliases
                                                .FirstOrDefaultAsync(t => t.BulkPlantId == viewModel.TerminalId
                                                && t.CreatedByCompanyId == userContext.CompanyId
                                                && t.IsActive && t.IsBulkPlant == viewModel.IsBulkPlant
                                                && t.TerminalSupplierId == viewModel.TerminalSupplierId.Value
                                                && t.AssignedTerminalId.ToLower() == viewModel.AssignedTerminalId.ToLower());
                    }
                    else
                        existingMapping = await Context.DataContext.TerminalCompanyAliases.
                                            FirstOrDefaultAsync(t => t.TerminalId == viewModel.TerminalId
                                            && t.CreatedByCompanyId == userContext.CompanyId
                                            && t.IsActive && t.IsBulkPlant == viewModel.IsBulkPlant
                                            && t.TerminalSupplierId == viewModel.TerminalSupplierId.Value
                                            && t.AssignedTerminalId.ToLower() == viewModel.AssignedTerminalId.ToLower());
                    if (existingMapping != null && viewModel.Id == 0)
                    {
                        response.StatusCode = Status.Warning;
                        response.StatusMessage = "Mapping exists for Terminal ,Terminal Supplier and TerminalId";
                        return response;
                    }
                    else
                    {
                        response.StatusCode = Status.Success;
                    }

                }                             
            }
            catch (Exception ex)
            {                
                LogManager.Logger.WriteException("TerminalMappingDomain", "ValidateTerminalMapping", ex.Message, ex);
            }
            return response;
        }


    }
}
