using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SiteFuel.Exchange.Domain.Mappers;

namespace SiteFuel.Exchange.Domain
{
    public class AccessorialFeeDomain : BaseDomain
    {
        public AccessorialFeeDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public AccessorialFeeDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<AccessorialFeeGridViewModel>> GetAccessorialFeeSummary(ViewAccessorialFeeInputViewModel input, int supplierCompanyId)
        {
            var response = new List<AccessorialFeeGridViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var accessorialFeeSummary = await storedProcedureDomain.GetAccessorialFeeSummary(input, supplierCompanyId);
                var accessorialFeeMapping = accessorialFeeSummary.GroupBy(t => t.Id);
                foreach (var item in accessorialFeeMapping)
                {
                    var accessorialFee = item.FirstOrDefault();
                    var data = new AccessorialFeeGridViewModel();

                    data.Id = accessorialFee.Id;
                    data.TableName = accessorialFee.TableName;
                    data.TableType = accessorialFee.TableType.GetDisplayName();
                    data.StatusName = accessorialFee.StatusId.GetDisplayName();
                    if (accessorialFee.StatusId == FreightTableStatus.Archived)
                        data.IsArchived = true;
                    data.StartDate = accessorialFee.StartDate.ToString(Resource.constFormatDate);
                    data.EndDate = accessorialFee.EndDate != null ? accessorialFee.EndDate.Value.ToString(Resource.constFormatDate) : null;
                    data.DateRange = accessorialFee.StartDate.ToString(Resource.constFormatDate) + (accessorialFee.EndDate.HasValue ? (" - " + accessorialFee.EndDate.Value.ToString(Resource.constFormatDate)) : "");

                    data.SourceRegion = Resource.lblHyphen;
                    var sourceRegions = Context.DataContext.FreightTableSourceRegions.Where(t => t.AccessorialFeeId == accessorialFee.Id).Select(t => t.SourceRegion.Name).Distinct().ToList();
                    if (sourceRegions != null && sourceRegions.Count > 0)
                    {
                        data.SourceRegion = string.Join(",", sourceRegions);
                    }

                    data.Customer = Resource.lblHyphen;
                    var customers = Context.DataContext.FreightTableCompanies.Where(t => t.AccessorialFeeId == accessorialFee.Id && t.AssignedCompanyType == AssignedCompanyType.Customer).Select(t => t.AssignedCompany.Name).Distinct().ToList();
                    if (customers != null && customers.Count > 0)
                    {
                        data.Customer = string.Join(",", customers);
                    }

                    data.Carrier = Resource.lblHyphen;
                    var carriers = Context.DataContext.FreightTableCompanies.Where(t => t.AccessorialFeeId == accessorialFee.Id && t.AssignedCompanyType == AssignedCompanyType.Carrier).Select(t => t.AssignedCompany.Name).Distinct().ToList();
                    if (carriers != null && carriers.Count > 0)
                    {
                        data.Carrier = string.Join(",", carriers);
                    }
                    
                    if (accessorialFee.TableType == TableTypes.Master)
                    {
                        data.Carrier = Resource.lblHyphen;
                        data.Customer = Resource.lblHyphen;
                    }

                    data.Terminal = Resource.lblHyphen;
                    var terminals = Context.DataContext.FreightTablePickupLocations.Where(t => t.AccessorialFeeId == accessorialFee.Id && t.TerminalId != null).Select(t => t.MstExternalTerminal.Name).Distinct().ToList();
                    if (terminals != null && terminals.Count > 0)
                    {
                        data.Terminal = string.Join(", ", terminals);
                    }

                    data.BulkPlant = Resource.lblHyphen;
                    var bulkPlants = Context.DataContext.FreightTablePickupLocations.Where(t => t.AccessorialFeeId == accessorialFee.Id && t.BulkPlantId != null).Select(t => t.BulkPlantLocation.Name).Distinct().ToList();
                    if (bulkPlants != null && bulkPlants.Count > 0)
                    {
                        data.BulkPlant = string.Join(", ", bulkPlants);
                    }

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AccessorialFeeDomain", "GetAccessorialFeeSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ArchiveAccessorialFee(int accessorialFeeId, int userId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var accessorialFee = await Context.DataContext.AccessorialFees.FirstOrDefaultAsync(t => t.Id == accessorialFeeId);
                if (accessorialFee != null)
                {
                    accessorialFee.StatusId = FreightTableStatus.Archived;
                    accessorialFee.UpdatedDate = DateTimeOffset.Now;
                    accessorialFee.UpdatedBy = userId;
                    Context.DataContext.Entry(accessorialFee).State = EntityState.Modified;
                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("AccessorialFeeDomain", "ArchiveAccessorialFee", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateAccessorialFee(AccessorialFeeViewModel model, int companyId, int userId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                response = ValidateFeeViewModel(model, companyId, response);

                if (response.StatusCode == Status.Success)
                {
                    //Save or update fees tables.
                    var accessorialFee = model.ToEntity(companyId, userId);
                    if (accessorialFee != null)
                    {
                        using (var transaction = Context.DataContext.Database.BeginTransaction())
                        {
                            try
                            {
                                if(model.Id > 0)
                                {
                                    var exisingFee = await Context.DataContext.AccessorialFees.Where(t => t.Id == model.Id).FirstOrDefaultAsync();
                                    if(exisingFee != null)
                                    {
                                        exisingFee.IsActive = false;
                                        exisingFee.UpdatedBy = userId;
                                        exisingFee.UpdatedDate = DateTimeOffset.Now;

                                        var maxVersion = await Context.DataContext.AccessorialFees
                                                                      .Where(t => t.Name.ToLower() == exisingFee.Name.ToLower() &&
                                                                                  t.SupplierCompanyId == companyId
                                                                            )
                                                                      .OrderByDescending(t => t.Version)
                                                                      .Select(t => t.Version)
                                                                      .FirstOrDefaultAsync();
                                        accessorialFee.Version = maxVersion + 1;

                                        Context.DataContext.Entry(exisingFee).State = EntityState.Modified;
                                    }
                                }

                                Context.DataContext.AccessorialFees.Add(accessorialFee);
                                await Context.CommitAsync();
                                transaction.Commit();

                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.successMessageAccessorialFeeSavedSuccessfully;
                            }
                            catch (Exception ex)
                            {
                                if (transaction != null && transaction.UnderlyingTransaction.Connection != null)
                                {
                                    transaction.Rollback();
                                }
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errorMessageFailedToSaveAccessorialFee;

                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errorMessageFailedToSaveAccessorialFee;
                LogManager.Logger.WriteException("AccessorialFeeDomain", "CreateAccessorialFee", ex.Message, ex);
            }
            return response;
        }

        private StatusViewModel ValidateFeeViewModel(AccessorialFeeViewModel model, int companyId, StatusViewModel response)
        {
            if (response == null)
                response = new StatusViewModel();

            response.StatusMessage = null;
            response.StatusCode = Status.Failed;

            if (string.IsNullOrEmpty(model.Name))
                response.StatusMessage = Resource.errFeeTableName; 
            else
            {
                var isNameExists = IsAccessorialFeeNameExists(model, companyId);
                if (isNameExists)
                    response.StatusMessage = Resource.errFeeTableNameAlreadyExists;
            }

            if(model.Status == FreightTableStatus.Published)
            {
                if (model.TableType == TableTypes.Master)
                {
                    model.CustomerIds.Clear();
                    model.CarrierIds.Clear();
                }
                else if (model.TableType == TableTypes.CarrierSpecific)
                {
                    if (!model.CarrierIds.Any())
                        response.StatusMessage = Resource.errSelectCarrier;
                }
                else if (model.TableType == TableTypes.CustomerSpecific)
                {
                    if (!model.CustomerIds.Any())
                        response.StatusMessage = Resource.errSelectCustomer;
                }

                if (!model.SourceRegionIds.Any())
                    response.StatusMessage = Resource.errSelectSourceRegion;

                if (!model.TerminalsAndBulkPlants.Any())
                    response.StatusMessage = Resource.errorMessageSelectTerminalOrBulkplant;
            }

            if (string.IsNullOrWhiteSpace(response.StatusMessage))
                response.StatusCode = Status.Success;

            return response;
        }

        private bool IsAccessorialFeeNameExists(AccessorialFeeViewModel model, int companyId)
        {
            return Context.DataContext.AccessorialFees.Any(t => (model.Id == 0 || t.Id != model.Id) &&
                                                                t.IsActive &&
                                                                t.SupplierCompanyId == companyId &&
                                                                t.Name.ToLower() == model.Name.ToLower());
        }

        public async Task<AccessorialFeeViewModel> GetAccessorialFee(int id, UserContext userContext)
        {
            AccessorialFeeViewModel viewModel = new AccessorialFeeViewModel();
            var entity = await Context.DataContext.AccessorialFees.Where(t => t.Id == id && t.SupplierCompanyId == userContext.CompanyId && t.IsActive)
                            .SingleOrDefaultAsync();
            if (entity != null)
            {
                viewModel.Fees = entity.FuelFees.ToFeesViewModel();

                entity.ToViewModel(viewModel);
            }
            else
                viewModel.StatusMessage = "Fee table details not found.";

            return viewModel;
        }

        public async Task<List<FeesViewModel>> GetAccessorialFeeByAccessorialFeeId(string accessorialFeeId, UserContext userContext)
        {
            List<FeesViewModel> viewModel = new List<FeesViewModel>();
            if (!string.IsNullOrEmpty(accessorialFeeId))
            {
                var Ids = accessorialFeeId.Split(',').Select(x => Convert.ToInt32(x)).Distinct().ToList();
                var entity = await Context.DataContext.AccessorialFees.Where(t => Ids.Contains(t.Id) && t.SupplierCompanyId == userContext.CompanyId && t.IsActive)
                                .ToListAsync();
                foreach (var item in entity)
                {
                    if (entity != null)
                    {
                        viewModel.AddRange(item.FuelFees.ToFeesViewModel());
                    }
                }
            }
            return viewModel;
        }
    }
}
