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
using SiteFuel.Exchange.DataAccess.Entities;

namespace SiteFuel.Exchange.Domain
{
    public class FreightRateDomain : BaseDomain
    {
        public FreightRateDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public FreightRateDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<FreightRateGridViewModel>> GetFreightRateSummary(ViewFreightRateInputViewModel input, int supplierCompanyId)
        {
            var response = new List<FreightRateGridViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var freightRateSummary = await storedProcedureDomain.GetFreightRateSummary(input, supplierCompanyId);
                var freightRateMapping = freightRateSummary.GroupBy(t => t.Id);
                foreach (var item in freightRateMapping)
                {
                    var freightRate = item.FirstOrDefault();
                    var data = new FreightRateGridViewModel();

                    data.Id = freightRate.Id;
                    data.TableName = freightRate.TableName;
                    data.FreightRateRuleType = freightRate.FreightRateRuleType.GetDisplayName();
                    data.FreightRateRuleTypeValue = (int)freightRate.FreightRateRuleType;
                    data.TableType = freightRate.TableType.GetDisplayName();
                    data.StatusName = freightRate.StatusId.GetDisplayName();
                    if (freightRate.StatusId == FreightTableStatus.Archived)
                        data.IsArchived = true;
                    data.StartDate = freightRate.StartDate.ToString(Resource.constFormatDate);
                    data.EndDate = freightRate.EndDate != null ? freightRate.EndDate.Value.ToString(Resource.constFormatDate) : null;
                    data.DateRange = freightRate.StartDate.ToString(Resource.constFormatDate) + (freightRate.EndDate.HasValue ? (" - " + freightRate.EndDate.Value.ToString(Resource.constFormatDate)) : "");

                    data.SourceRegion = Resource.lblHyphen;
                    var sourceRegions = Context.DataContext.FreightTableSourceRegions.Where(t => t.FreightRateRuleId == freightRate.Id).Select(t => t.SourceRegion.Name).Distinct().ToList();
                    if (sourceRegions != null && sourceRegions.Count > 0)
                    {
                        data.SourceRegion = string.Join(", ", sourceRegions);
                    }

                    data.Customer = Resource.lblHyphen;
                    var customers = Context.DataContext.FreightTableCompanies.Where(t => t.FreightRateRuleId == freightRate.Id && t.AssignedCompanyType == AssignedCompanyType.Customer).Select(t => t.AssignedCompany.Name).Distinct().ToList();
                    if (customers != null && customers.Count > 0)
                    {
                        data.Customer = string.Join(", ", customers);
                    }

                    data.Carrier = Resource.lblHyphen;
                    var carriers = Context.DataContext.FreightTableCompanies.Where(t => t.FreightRateRuleId == freightRate.Id && t.AssignedCompanyType == AssignedCompanyType.Carrier).Select(t => t.AssignedCompany.Name).Distinct().ToList();
                    if (carriers != null && carriers.Count > 0)
                    {
                        data.Carrier = string.Join(", ", carriers);
                    }

                    if (freightRate.TableType == TableTypes.Master)
                    {
                        data.Carrier = Resource.lblHyphen;
                        data.Customer = Resource.lblHyphen;
                    }

                    data.Terminal = Resource.lblHyphen;
                    var terminals = Context.DataContext.FreightTablePickupLocations.Where(t => t.FreightRateRuleId == freightRate.Id && t.TerminalId != null).Select(t => t.MstExternalTerminal.Name).Distinct().ToList();
                    if (terminals != null && terminals.Count > 0)
                    {
                        data.Terminal = string.Join(", ", terminals);
                    }

                    data.BulkPlant = Resource.lblHyphen;
                    var bulkPlants = Context.DataContext.FreightTablePickupLocations.Where(t => t.FreightRateRuleId == freightRate.Id && t.BulkPlantId != null).Select(t => t.BulkPlantLocation.Name).Distinct().ToList();
                    if (bulkPlants != null && bulkPlants.Count > 0)
                    {
                        data.BulkPlant = string.Join(", ", bulkPlants);
                    }

                    data.FuelGroup = Resource.lblHyphen;
                    var fuelGroups = Context.DataContext.FreightRateFuelGroups.Where(t => t.FreightRateRuleId == freightRate.Id).Select(t => t.FuelGroup.GroupName).Distinct().ToList();
                    if (fuelGroups != null && fuelGroups.Count > 0)
                    {
                        data.FuelGroup = string.Join(", ", fuelGroups);
                    }

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightRateDomain", "GetFreightRateSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<FreightRateViewModel> GetFreightRateDetails(int freightRateId)
        {
            var viewModel = new FreightRateViewModel();
            try
            {
                var freightRate = await Context.DataContext.FreightRateRules.FirstOrDefaultAsync(t => t.Id == freightRateId);
                freightRate.ToViewModel(viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightRateDomain", "GetFreightRateDetails", ex.Message, ex);
            }

            return viewModel;
        }

        public async Task<FreightRateTableViewModel> GetFreightRateTableForView(int freightRateId)
        {
            var viewModel = new FreightRateTableViewModel();
            try
            {
                var freightRate = await Context.DataContext.FreightRateRules.FirstOrDefaultAsync(t => t.Id == freightRateId);
                freightRate.ToFreightRateTableViewModel(viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FreightRateDomain", "GetFreightRateTableForView", ex.Message, ex);
            }

            return viewModel;
        }

        public async Task<StatusViewModel> ArchiveFreightRate(int freightRateId, int userId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var freightRate = await Context.DataContext.FreightRateRules.FirstOrDefaultAsync(t => t.Id == freightRateId);
                if (freightRate != null)
                {
                    freightRate.Status = FreightTableStatus.Archived;
                    freightRate.UpdatedDate = DateTimeOffset.Now;
                    freightRate.UpdatedBy = userId;
                    Context.DataContext.Entry(freightRate).State = EntityState.Modified;
                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("FreightRateDomain", "ArchiveFreightRate", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> GetExistingFreightRate(FreightRateViewModel model, int companyId)
        {
            var response = new StatusViewModel(Status.Success);

            var fs = await Context.DataContext.FreightRateRules.FirstOrDefaultAsync(f => f.Name.Trim().ToLower().Equals(model.Name.Trim().ToLower())
                   && f.CreatedByCompanyId == companyId && f.IsActive);
            if (fs != null)
            {
                response.StatusMessage = fs.Name + " is already in published mode.";
                response.StatusCode = Status.Failed;
                return response;
            }
            return null;
        }

        public async Task<StatusViewModel> CreateFreightRate(FreightRateViewModel model, int companyId, int userId)
        {
            var response = new StatusViewModel(Status.Success);

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var freightRate = model.ToEntity(companyId, userId);
                    Context.DataContext.FreightRateRules.Add(freightRate);
                    await Context.CommitAsync();

                    foreach (var item in model.TerminalsAndBulkPlants)
                    {
                        FreightTablePickupLocation freightTablePickupLocation = new FreightTablePickupLocation
                        {
                            FreightRateRuleId = freightRate.Id,
                            BulkPlantId = item.Code == "Bulk Plants" ? Int32.Parse(item.Id.Split("_".ToCharArray())[1]) : new Nullable<int>(),
                            TerminalId = item.Code == "Terminals" ? Int32.Parse(item.Id.Split("_".ToCharArray())[1]) : new Nullable<int>(),
                        };
                        freightRate.FreightTablePickupLocations.Add(freightTablePickupLocation);
                    }
                    await Context.CommitAsync();

                    foreach (var item in model.SourceRegionIds)
                    {
                        FreightTableSourceRegion freightTableSourceRegion = new FreightTableSourceRegion
                        {
                            SourceRegionId = item
                        };
                        freightRate.FreightTableSourceRegions.Add(freightTableSourceRegion);
                    }
                    await Context.CommitAsync();

                    foreach (var item in model.CustomerIds)
                    {
                        FreightTableCompany freightTableCompany = new FreightTableCompany
                        {
                            AssignedCompanyId = item,
                            AssignedCompanyType = AssignedCompanyType.Customer,
                            IsActive = true
                        };
                        freightRate.FreightTableCompanies.Add(freightTableCompany);
                    }
                    await Context.CommitAsync();

                    foreach (var item in model.CarrierIds)
                    {
                        FreightTableCompany freightTableCompany = new FreightTableCompany
                        {
                            AssignedCompanyId = item,
                            AssignedCompanyType = AssignedCompanyType.Carrier,
                            IsActive = true
                        };
                        freightRate.FreightTableCompanies.Add(freightTableCompany);
                    }
                    await Context.CommitAsync();

                    foreach (var item in model.FreightRateFuelGroups)
                    {
                        FreightRateFuelGroup freightRateFuelGroup = new FreightRateFuelGroup
                        {
                            MinQuantity = item.MinQuantity,
                            FuelGroupId = item.FuelGroupId
                        };
                        freightRate.FreightRateFuelGroups.Add(freightRateFuelGroup);
                    }
                    await Context.CommitAsync();

                    if (freightRate.FreightRateRuleType == FreightRateRuleType.Route)
                    {
                        foreach (var item in model.FreightRateRouteTables)
                        {
                            FreightRateRouteTable freightRateRouteTable = new FreightRateRouteTable
                            {
                                StartQuantity = item.StartQuantity,
                                EndQuantity = item.EndQuantity,
                                RateValue = item.RateValue,
                                FuelGroupId = item.FuelGroupId,
                                IsActive = true
                            };
                            freightRate.FreightRateRouteTables.Add(freightRateRouteTable);
                        }
                        await Context.CommitAsync();
                    }
                    else if (freightRate.FreightRateRuleType == FreightRateRuleType.Range)
                    {
                        foreach (var item in model.FreightRateRangeTables)
                        {
                            FreightRateRangeTable freightRateRangeTable = new FreightRateRangeTable
                            {
                                UptoQuantity = item.UptoQuantity,
                                RateValue = item.RateValue,
                                FuelGroupId = item.FuelGroupId,
                                IsActive = true
                            };
                            freightRate.FreightRateRangeTables.Add(freightRateRangeTable);
                        }
                        await Context.CommitAsync();
                    }
                    else if (freightRate.FreightRateRuleType == FreightRateRuleType.P2P)
                    {
                        await AddAndUpdateFreightRatePtoP(freightRate, model);
                    }

                    transaction.Commit();
                    response.StatusMessage = "Created successfully.";
                }

                catch (Exception ex)
                {
                    response = new StatusViewModel(Status.Failed);
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FreightRateDomain", "CreateFreightRate", ex.Message, ex);
                }
            }
            return response;
        }

        private async Task AddAndUpdateFreightRatePtoP(FreightRateRule freightRate, FreightRateViewModel model)
        {
            var terminalIds = model.FreightRatePtoPTables.Where(t => t.TerminalId != null).Select(t => t.TerminalId).Distinct().ToList();
            foreach (var terminalId in terminalIds)
            {
                var freightRatePtoPTable = new FreightRatePtoPTable
                {
                    TerminalId = terminalId,
                    IsActive = true
                };
                freightRate.FreightRatePtoPTables.Add(freightRatePtoPTable);
                await Context.CommitAsync();

                var jobIds = model.FreightRatePtoPTables.Where(t => t.TerminalId == terminalId).Select(t => t.JobId).Distinct().ToList();
                foreach (var jobId in jobIds)
                {
                    var locationModel = model.FreightRatePtoPTables.FirstOrDefault(t => t.TerminalId == terminalId && t.JobId == jobId);
                    var freightRatePtoPLocation = new FreightRatePtoPLocation
                    {
                        FreightRatePtoPTableId = freightRatePtoPTable.Id,
                        JobId = jobId,
                        AssumedMiles = locationModel.AssumedMiles,
                        IsLaneRequired = locationModel.IsLaneRequired,
                        LaneID = locationModel.LaneID
                    };
                    Context.DataContext.FreightRatePtoPLocations.Add(freightRatePtoPLocation);
                    await Context.CommitAsync();

                    var freightRatePtoPFuelGroups = model.FreightRatePtoPTables.Where(t => t.TerminalId == terminalId && t.JobId == jobId).Distinct().ToList();
                    foreach (var item in freightRatePtoPFuelGroups)
                    {
                        var freightRatePtoPFuelGroup = new FreightRatePtoPFuelGroup
                        {
                            FreightRatePtoPLocationId = freightRatePtoPLocation.Id,
                            FuelGroupId = item.FuelGroupId,
                            RateValue = item.RateValue
                        };
                        Context.DataContext.FreightRatePtoPFuelGroups.Add(freightRatePtoPFuelGroup);
                        await Context.CommitAsync();
                    }
                }
            }

            var bulkPlantIds = model.FreightRatePtoPTables.Where(t => t.BulkPlantId != null).Select(t => t.BulkPlantId).Distinct().ToList();
            foreach (var bulkPlant in bulkPlantIds)
            {
                var freightRatePtoPTable = new FreightRatePtoPTable
                {
                    BulkPlantId = bulkPlant,
                    IsActive = true
                };
                freightRate.FreightRatePtoPTables.Add(freightRatePtoPTable);
                await Context.CommitAsync();

                var jobIds = model.FreightRatePtoPTables.Where(t => t.BulkPlantId == bulkPlant).Select(t => t.JobId).Distinct().ToList();
                foreach (var jobId in jobIds)
                {
                    var locationModel = model.FreightRatePtoPTables.FirstOrDefault(t => t.BulkPlantId == bulkPlant && t.JobId == jobId);
                    var freightRatePtoPLocation = new FreightRatePtoPLocation
                    {
                        FreightRatePtoPTableId = freightRatePtoPTable.Id,
                        JobId = jobId,
                        AssumedMiles = locationModel.AssumedMiles,
                        IsLaneRequired = locationModel.IsLaneRequired,
                        LaneID = locationModel.LaneID
                    };
                    Context.DataContext.FreightRatePtoPLocations.Add(freightRatePtoPLocation);
                    await Context.CommitAsync();

                    var freightRatePtoPFuelGroups = model.FreightRatePtoPTables.Where(t => t.BulkPlantId == bulkPlant && t.JobId == jobId).Distinct().ToList();
                    foreach (var item in freightRatePtoPFuelGroups)
                    {
                        var freightRatePtoPFuelGroup = new FreightRatePtoPFuelGroup
                        {
                            FreightRatePtoPLocationId = freightRatePtoPLocation.Id,
                            FuelGroupId = item.FuelGroupId,
                            RateValue = item.RateValue
                        };
                        Context.DataContext.FreightRatePtoPFuelGroups.Add(freightRatePtoPFuelGroup);
                        await Context.CommitAsync();
                    }
                }
            }
        }

        public async Task<StatusViewModel> UpdateFreightRate(FreightRateViewModel model, int companyId, int userId)
        {
            var response = new StatusViewModel(Status.Success);

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var freightRate = await Context.DataContext.FreightRateRules.FirstOrDefaultAsync(t => t.Id == model.Id);
                    if (freightRate != null && model != null)
                    {
                        freightRate = model.ToEntity(companyId, userId, freightRate);
                        freightRate.UpdatedBy = userId;
                        freightRate.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(freightRate).State = EntityState.Modified;
                        await Context.CommitAsync();

                        Context.DataContext.FreightTablePickupLocations.RemoveRange(freightRate.FreightTablePickupLocations);
                        foreach (var item in model.TerminalsAndBulkPlants)
                        {
                            FreightTablePickupLocation freightTablePickupLocation = new FreightTablePickupLocation
                            {
                                FreightRateRuleId = freightRate.Id,
                                BulkPlantId = item.Code == "Bulk Plants" ? Int32.Parse(item.Id.Split("_".ToCharArray())[1]) : new Nullable<int>(),
                                TerminalId = item.Code == "Terminals" ? Int32.Parse(item.Id.Split("_".ToCharArray())[1]) : new Nullable<int>(),
                            };
                            freightRate.FreightTablePickupLocations.Add(freightTablePickupLocation);
                        }

                        Context.DataContext.FreightTableSourceRegions.RemoveRange(freightRate.FreightTableSourceRegions);
                        foreach (var item in model.SourceRegionIds)
                        {
                            FreightTableSourceRegion freightTableSourceRegion = new FreightTableSourceRegion
                            {
                                SourceRegionId = item
                            };
                            freightRate.FreightTableSourceRegions.Add(freightTableSourceRegion);
                            await Context.CommitAsync();
                        }

                        Context.DataContext.FreightTableCompanies.RemoveRange(freightRate.FreightTableCompanies);
                        foreach (var item in model.CustomerIds)
                        {
                            FreightTableCompany freightTableCompany = new FreightTableCompany
                            {
                                AssignedCompanyId = item,
                                IsActive = true,
                                AssignedCompanyType = AssignedCompanyType.Customer,
                            };
                            freightRate.FreightTableCompanies.Add(freightTableCompany);
                            await Context.CommitAsync();
                        }

                        foreach (var item in model.CarrierIds)
                        {
                            FreightTableCompany freightTableCompany = new FreightTableCompany
                            {
                                AssignedCompanyId = item,
                                IsActive = true,
                                AssignedCompanyType = AssignedCompanyType.Carrier,
                            };
                            freightRate.FreightTableCompanies.Add(freightTableCompany);
                            await Context.CommitAsync();
                        }

                        Context.DataContext.FreightRateFuelGroups.RemoveRange(freightRate.FreightRateFuelGroups);
                        foreach (var item in model.FreightRateFuelGroups)
                        {
                            FreightRateFuelGroup freightRateFuelGroup = new FreightRateFuelGroup
                            {
                                MinQuantity = item.MinQuantity,
                                FuelGroupId = item.FuelGroupId
                            };
                            freightRate.FreightRateFuelGroups.Add(freightRateFuelGroup);
                            await Context.CommitAsync();
                        }

                        if (freightRate.FreightRateRuleType == FreightRateRuleType.Route)
                        {
                            Context.DataContext.FreightRateRouteTables.RemoveRange(freightRate.FreightRateRouteTables);
                            foreach (var item in model.FreightRateRouteTables)
                            {
                                FreightRateRouteTable freightRateRouteTable = new FreightRateRouteTable
                                {
                                    StartQuantity = item.StartQuantity,
                                    EndQuantity = item.EndQuantity,
                                    RateValue = item.RateValue,
                                    FuelGroupId = item.FuelGroupId,
                                    IsActive = true
                                };
                                freightRate.FreightRateRouteTables.Add(freightRateRouteTable);
                            }
                            await Context.CommitAsync();
                        }
                        else if (freightRate.FreightRateRuleType == FreightRateRuleType.Range)
                        {
                            Context.DataContext.FreightRateRangeTables.RemoveRange(freightRate.FreightRateRangeTables);
                            foreach (var item in model.FreightRateRangeTables)
                            {
                                FreightRateRangeTable freightRateRangeTable = new FreightRateRangeTable
                                {
                                    UptoQuantity = item.UptoQuantity,
                                    RateValue = item.RateValue,
                                    FuelGroupId = item.FuelGroupId,
                                    IsActive = true
                                };
                                freightRate.FreightRateRangeTables.Add(freightRateRangeTable);
                            }
                            await Context.CommitAsync();
                        }
                        else if (freightRate.FreightRateRuleType == FreightRateRuleType.P2P)
                        {
                            var freightRatePtoPTableIds = freightRate.FreightRatePtoPTables.Where(t => t.FreightRateRuleId == model.Id).Select(t => t.Id).ToList();
                            var freightRatePtoPLocations = Context.DataContext.FreightRatePtoPLocations.Where(t => freightRatePtoPTableIds.Contains(t.FreightRatePtoPTableId)).ToList();
                            if (freightRatePtoPLocations != null)
                            {
                                var freightRatePtoPLocationIds = freightRatePtoPLocations.Select(t => t.Id).ToList();
                                var freightRatePtoPFuelGroups = Context.DataContext.FreightRatePtoPFuelGroups.Where(t => freightRatePtoPLocationIds.Contains(t.FreightRatePtoPLocationId)).ToList();
                                if (freightRatePtoPFuelGroups != null)
                                    Context.DataContext.FreightRatePtoPFuelGroups.RemoveRange(freightRatePtoPFuelGroups);
                                if (freightRate.FreightRatePtoPTables != null)
                                    Context.DataContext.FreightRatePtoPTables.RemoveRange(freightRate.FreightRatePtoPTables);
                                await AddAndUpdateFreightRatePtoP(freightRate, model);
                            }
                        }

                        transaction.Commit();
                    }
                    response.StatusMessage = "Updated successfully.";
                }

                catch (Exception ex)
                {
                    response = new StatusViewModel(Status.Failed);
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FreightRateDomain", "UpdateFreightRate", ex.Message, ex);
                }
            }
            return response;
        }
    }
}
