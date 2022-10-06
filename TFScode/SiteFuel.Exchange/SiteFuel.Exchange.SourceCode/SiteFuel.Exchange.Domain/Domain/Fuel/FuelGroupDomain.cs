using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class FuelGroupDomain : BaseDomain
    {
        public FuelGroupDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public FuelGroupDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<FuelGroupGridViewModel>> GetFuelGroupSummary(int supplierCompanyId)
        {
            var response = new List<FuelGroupGridViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var fuelGroupSummary = await storedProcedureDomain.GetFuelGroupSummary(supplierCompanyId);
                var fuelGroupMapping = fuelGroupSummary.GroupBy(t => t.Id);
                foreach (var item in fuelGroupMapping)
                {
                    var fuelGroup = item.FirstOrDefault();
                    var data = new FuelGroupGridViewModel();

                    data.Id = fuelGroup.Id;
                    data.GroupName = fuelGroup.GroupName;
                    data.FuelGroupType = fuelGroup.FuelGroupType.GetDisplayName();
                    data.TableType = fuelGroup.TableType.GetDisplayName().Replace("Specific", "");
                    data.StatusName = fuelGroup.StatusId.GetDisplayName();

                    data.ProductType = Resource.lblHyphen;
                    var productTypes = fuelGroupSummary.Where(t => t.Id == fuelGroup.Id).Select(t => t.ProductType).Distinct().ToList();
                    if (productTypes != null && productTypes.Count > 0)
                    {
                        data.ProductType = string.Join(", ", productTypes);
                    }

                    if (fuelGroup.TableType == TableTypes.CustomerSpecific)
                    {
                        data.Customer = fuelGroup.Company;
                        data.Carrier = Resource.lblHyphen;
                    }
                    else if (fuelGroup.TableType == TableTypes.CarrierSpecific)
                    {
                        data.Carrier = fuelGroup.Company;
                        data.Customer = Resource.lblHyphen;
                    }
                    else if (fuelGroup.TableType == TableTypes.Master)
                    {
                        data.Carrier = Resource.lblHyphen;
                        data.Customer = Resource.lblHyphen;
                    }

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelGroupDomain", "GetFuelGroupSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<FuelGroupViewModel> GetFuelGroupDetails(int fuelGroupId)
        {
            var viewModel = new FuelGroupViewModel();
            try
            {
                var fuelGroup = await Context.DataContext.FuelGroups.FirstOrDefaultAsync(t => t.Id == fuelGroupId);
                fuelGroup.ToViewModel(viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelGroupDomain", "GetFuelGroupDetails", ex.Message, ex);
            }

            return viewModel;
        }

        public async Task<StatusViewModel> ArchiveFuelGroup(int fuelGroupId, int userId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var fuelGroup = await Context.DataContext.FuelGroups.FirstOrDefaultAsync(t => t.Id == fuelGroupId);
                if (fuelGroup != null)
                {
                    fuelGroup.StatusId = FreightTableStatus.Archived;
                    fuelGroup.UpdatedDate = DateTimeOffset.Now;
                    fuelGroup.UpdatedBy = userId;
                    Context.DataContext.Entry(fuelGroup).State = EntityState.Modified;
                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("FuelGroupDomain", "ArchiveFuelGroup", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> GetExistingFuelGroup(FuelGroupViewModel model, int companyId)
        {
            var response = new StatusViewModel(Status.Success);

            var fs = await Context.DataContext.FuelGroups.FirstOrDefaultAsync(f => f.GroupName.Trim().ToLower().Equals(model.GroupName.Trim().ToLower())
                   && f.CreatedByCompanyId == companyId && f.IsActive);
            if (fs != null)
            {
                response.StatusMessage = fs.GroupName + " fuel group already exists.";
                response.StatusCode = Status.Failed;
                return response;
            }
            return null;
        }

        public async Task<StatusViewModel> CreateFuelGroup(FuelGroupViewModel model, int companyId, int userId)
        {
            var response = new StatusViewModel(Status.Success);

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var fuelGroup = model.ToEntity(companyId, userId);
                    Context.DataContext.FuelGroups.Add(fuelGroup);
                    await Context.CommitAsync();

                    foreach (var item in model.ProductTypeIds)
                    {
                        FuelGroupProductType fuelGroupProduct = new FuelGroupProductType();
                        fuelGroupProduct.ProductTypeId = item;
                        fuelGroup.FuelGroupProductTypes.Add(fuelGroupProduct);
                    }

                    foreach (var item in model.FuelTypeIds)
                    {
                        FuelGroupFuelType fuelGroupFuelType = new FuelGroupFuelType();
                        fuelGroupFuelType.TfxProductId = item;
                        fuelGroup.FuelGroupFuelTypes.Add(fuelGroupFuelType);
                    }
                    await Context.CommitAsync();
                    transaction.Commit();
                    response.StatusMessage = "Created successfully.";
                }

                catch (Exception ex)
                {
                    response = new StatusViewModel(Status.Failed);
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelGroupDomain", "CreateFuelGroup", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateFuelGroup(FuelGroupViewModel model, int companyId, int userId)
        {
            var response = new StatusViewModel(Status.Success);

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var fuelGroup = await Context.DataContext.FuelGroups.FirstOrDefaultAsync(t => t.Id == model.Id);
                    if (fuelGroup != null && model != null)
                    {
                        fuelGroup = model.ToEntity(companyId, userId, fuelGroup);
                        fuelGroup.UpdatedBy = userId;
                        fuelGroup.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(fuelGroup).State = EntityState.Modified;
                        await Context.CommitAsync();

                        Context.DataContext.FuelGroupProductTypes.RemoveRange(fuelGroup.FuelGroupProductTypes);
                        foreach (var item in model.ProductTypeIds)
                        {
                            var fuelGroupProductType = new FuelGroupProductType();
                            fuelGroupProductType.FuelGroupId = model.Id;
                            fuelGroupProductType.ProductTypeId = item;
                            Context.DataContext.FuelGroupProductTypes.Add(fuelGroupProductType);
                        }
                        await Context.CommitAsync();

                        Context.DataContext.FuelGroupFuelTypes.RemoveRange(fuelGroup.FuelGroupFuelTypes);
                        foreach (var item in model.FuelTypeIds)
                        {
                            var fuelGroupFuelType = new FuelGroupFuelType();
                            fuelGroupFuelType.FuelGroupId = model.Id;
                            fuelGroupFuelType.TfxProductId = item;
                            Context.DataContext.FuelGroupFuelTypes.Add(fuelGroupFuelType);
                        }
                        await Context.CommitAsync();

                        transaction.Commit();
                    }
                    response.StatusMessage = "Updated successfully.";
                }

                catch (Exception ex)
                {
                    response = new StatusViewModel(Status.Failed);
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelGroupDomain", "UpdateFuelGroup", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<DropdownCustomItem>> GetFuelTypes(string productTypeIds, string fuelGroupType, int editingGroupId, int editingcompanyId,int CurrentUserCompanyId)
        {

            List<DropdownCustomItem> response = new List<DropdownCustomItem>();
            try
            {
                if (!string.IsNullOrEmpty(productTypeIds))
                {
                    var pIds = new List<int>();

                    pIds = productTypeIds.Split(',').Select(int.Parse).ToList();

                    var r = await Context.DataContext
                                     .MstTfxProducts
                                     .Where(t => t.IsActive
                                     && pIds.Contains(t.ProductTypeId)
                                     && t.ProductDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType)
                                     .Select(t => new DropdownCustomItem
                                     {
                                         Id = t.Id,
                                         Name = t.Name,
                                         isDisabled = false
                                     }).ToListAsync();
                    response.AddRange(r);

                    FuelGroupType fType = (FuelGroupType)int.Parse(fuelGroupType);

                    List<int> fgIds = null;
                    if (fType == FuelGroupType.Standard)
                    {
                        fgIds = await Context.DataContext.FuelGroups.Where(f => f.FuelGroupType == fType && f.Id != editingGroupId &&
                                          f.IsActive && f.StatusId == FreightTableStatus.Published && f.CreatedByCompanyId == CurrentUserCompanyId)
                                        .Select(fgId => fgId.Id).ToListAsync();
                    }
                    else if (fType == FuelGroupType.Custom)
                    {
                        fgIds = await Context.DataContext.FuelGroups.Where(f => f.FuelGroupType == fType && f.Id != editingGroupId &&
                                          f.AssignedCompanyId == editingcompanyId && f.IsActive && f.StatusId == FreightTableStatus.Published && f.CreatedByCompanyId == CurrentUserCompanyId)
                                        .Select(fgId => fgId.Id).ToListAsync();
                    }

                    if (fgIds != null && fgIds.Count > 0)
                    {
                        var tPids = await Context.DataContext.FuelGroupFuelTypes.Where(f => fgIds.Contains(f.FuelGroupId)).Select(pId => pId.TfxProductId).ToListAsync();
                        if (tPids != null && tPids.Count > 0)
                        {
                            response.Where(t => tPids.Contains(t.Id)).ToList().ForEach(t =>
                            {
                                t.isDisabled = true;
                            });
                        }
                    }

                }
                else
                {
                    var r = await Context.DataContext
                                    .MstTfxProducts
                                    .Where(t => t.IsActive
                                    && t.ProductDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType)
                                    .Select(t => new DropdownCustomItem
                                    {
                                        Id = t.Id,
                                        Name = t.Name,
                                        isDisabled = false
                                    }).ToListAsync();
                    response.AddRange(r);
                }


            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelGroupDomain", "GetAllFuelProducts", ex.Message, ex);
            }
            return response;

        }

        public async Task<List<DropdownDisplayItem>> GetFuelGroups(FuelGroupType fuelGroupType, string companyIds, int createdByCompanyId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            List<DropdownDisplayItem> fuelGroups=null;
            try
            {
                if (fuelGroupType== FuelGroupType.Standard)
                {
                    fuelGroups = await Context.DataContext.FuelGroups.Where(t => t.IsActive &&
                    t.FuelGroupType== fuelGroupType && t.CreatedByCompanyId == createdByCompanyId 
                    && t.StatusId == FreightTableStatus.Published)
                         .Select(t => new DropdownDisplayItem
                         {
                             Id = t.Id,
                             Name = t.GroupName
                         }).ToListAsync();

                }
                else if (!string.IsNullOrEmpty(companyIds))
                {
                    List<int> cIds = companyIds.Split(',').Select(int.Parse).ToList();
                    fuelGroups = await Context.DataContext.FuelGroups.Where(t => t.IsActive && t.CreatedByCompanyId == createdByCompanyId 
                    && t.FuelGroupType==fuelGroupType && t.AssignedCompanyId.HasValue 
                    && cIds.Contains(t.AssignedCompanyId.Value) && t.StatusId == FreightTableStatus.Published)
                         .Select(t => new DropdownDisplayItem
                         {
                             Id = t.Id,
                             Name = t.GroupName
                         }).ToListAsync();
                }

                if (fuelGroups != null && fuelGroups.Count > 0)
                response.AddRange(fuelGroups);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelGroupDomain", "GetFuelGroups", ex.Message, ex);
            }
            return response;

        }
    }
}
