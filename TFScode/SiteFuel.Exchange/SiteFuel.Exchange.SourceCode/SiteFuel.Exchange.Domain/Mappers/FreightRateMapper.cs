using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class FreightRateMapper
    {
        public static FreightRateRule ToEntity(this FreightRateViewModel viewModel, int companyId, int userId, FreightRateRule entity = null)
        {
            if (entity == null)
                entity = new FreightRateRule();

            entity.Id = viewModel.Id;
            entity.Name = viewModel.Name;
            entity.TableType = viewModel.TableType;
            entity.FuelGroupType = viewModel.FuelGroupType;
            entity.FreightRateCalcPreferenceType = viewModel.FreightRateCalcPreferenceType;
            entity.FreightRateCalcPrefFuelGroupId = viewModel.FreightRateCalcPrefFuelGroupId;
            entity.FreightRateRuleType = viewModel.FreightRateRuleType;
            entity.MixLoadMinValue = viewModel.MixLoadMinValue;
            entity.CreatedByCompanyId = companyId;
            entity.Status = viewModel.Status;
            entity.StartDate = viewModel.StartDate;
            entity.EndDate = viewModel.EndDate;
            entity.CreatedBy = userId;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.IsActive = true;
            entity.UpdatedBy = userId;
            entity.UpdatedDate = entity.CreatedDate;

            return entity;
        }

        public static FreightRateTableViewModel ToFreightRateTableViewModel(this FreightRateRule entity, FreightRateTableViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FreightRateTableViewModel();

            viewModel.FreightRateRuleType = entity.FreightRateRuleType;
            foreach (var item in entity.FreightRateFuelGroups.OrderBy(t => t.FuelGroupId))
            {
                FreightRateFuelGroupViewModel freightRateFuelGroup = new FreightRateFuelGroupViewModel
                {
                    MinQuantity = item.MinQuantity,
                    FuelGroupId = item.FuelGroupId,
                    FuelGroupName = item.FuelGroup.GroupName
                };
                viewModel.FreightRateFuelGroups.Add(freightRateFuelGroup);
            }

            if (entity.FreightRateRuleType == FreightRateRuleType.Route)
            {
                foreach (var item in entity.FreightRateRouteTables.OrderBy(t => t.FuelGroupId))
                {
                    FreightRateRouteTableViewModel freightRateRouteTable = new FreightRateRouteTableViewModel
                    {
                        StartQuantity = item.StartQuantity,
                        EndQuantity = item.EndQuantity,
                        RateValue = item.RateValue,
                        FuelGroupId = item.FuelGroupId,
                        FuelGroupName = item.FuelGroup.GroupName
                    };
                    viewModel.FreightRateRouteTables.Add(freightRateRouteTable);
                }
            }
            else if (entity.FreightRateRuleType == FreightRateRuleType.Range)
            {
                foreach (var item in entity.FreightRateRangeTables.OrderBy(t => t.FuelGroupId))
                {
                    FreightRateRangeTableViewModel freightRateRangeTable = new FreightRateRangeTableViewModel
                    {
                        UptoQuantity = item.UptoQuantity,
                        RateValue = item.RateValue,
                        FuelGroupId = item.FuelGroupId,
                        FuelGroupName = item.FuelGroup.GroupName
                    };
                    viewModel.FreightRateRangeTables.Add(freightRateRangeTable);
                }
            }

            return viewModel;
        }

        public static FreightRateViewModel ToViewModel(this FreightRateRule entity, FreightRateViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FreightRateViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.TableType = entity.TableType;
            viewModel.FuelGroupType = entity.FuelGroupType;
            viewModel.FreightRateCalcPreferenceType = entity.FreightRateCalcPreferenceType;
            viewModel.FreightRateCalcPrefFuelGroupId = entity.FreightRateCalcPrefFuelGroupId;
            viewModel.FreightRateRuleType = entity.FreightRateRuleType;
            viewModel.MixLoadMinValue = entity.MixLoadMinValue;
            viewModel.Status = entity.Status;
            viewModel.StartDate = entity.StartDate;
            viewModel.EndDate = entity.EndDate;
            viewModel.CustomerIds = entity.FreightTableCompanies.Where(t => t.AssignedCompanyType == AssignedCompanyType.Customer).Select(t => t.AssignedCompanyId).ToList();
            viewModel.CarrierIds = entity.FreightTableCompanies.Where(t => t.AssignedCompanyType == AssignedCompanyType.Carrier).Select(t => t.AssignedCompanyId).ToList();

            viewModel.SourceRegionIds = entity.FreightTableSourceRegions.Select(t => t.SourceRegionId).ToList();

            var pickupLocations = entity.FreightTablePickupLocations;
            viewModel.TerminalsAndBulkPlants = new List<DropdownDisplayExtended>();
            foreach (var item in pickupLocations)
            {
                if (item.TerminalId.HasValue && item.MstExternalTerminal.IsActive)
                    viewModel.TerminalsAndBulkPlants.Add(new DropdownDisplayExtended { Id = "Terminals_" + item.TerminalId.Value.ToString(), Code = "Terminals", Name = item.MstExternalTerminal.Name });

                if (item.BulkPlantId.HasValue && item.BulkPlantLocation.IsActive)
                    viewModel.TerminalsAndBulkPlants.Add(new DropdownDisplayExtended { Id = "BulkPlants_" + item.BulkPlantId.Value.ToString(), Code = "Bulk Plants", Name = item.BulkPlantLocation.Name });
            }

            viewModel.FuelGroupIds = entity.FreightRateFuelGroups.Select(t => t.FuelGroupId).Distinct().ToList();

            foreach (var item in entity.FreightRateFuelGroups)
            {
                FreightRateFuelGroupViewModel freightRateFuelGroup = new FreightRateFuelGroupViewModel
                {
                    MinQuantity = item.MinQuantity,
                    FuelGroupId = item.FuelGroupId,
                    FuelGroupName = item.FuelGroup.GroupName
                };
                viewModel.FreightRateFuelGroups.Add(freightRateFuelGroup);
            }

            if (entity.FreightRateRuleType == FreightRateRuleType.Route)
            {
                foreach (var item in entity.FreightRateRouteTables)
                {
                    FreightRateRouteTableViewModel freightRateRouteTable = new FreightRateRouteTableViewModel
                    {
                        StartQuantity = item.StartQuantity,
                        EndQuantity = item.EndQuantity,
                        RateValue = item.RateValue,
                        FuelGroupId = item.FuelGroupId,
                        FuelGroupName = item.FuelGroup.GroupName
                    };
                    viewModel.FreightRateRouteTables.Add(freightRateRouteTable);
                }
            }
            else if (entity.FreightRateRuleType == FreightRateRuleType.Range)
            {
                foreach (var item in entity.FreightRateRangeTables)
                {
                    FreightRateRangeTableViewModel freightRateRangeTable = new FreightRateRangeTableViewModel
                    {
                        UptoQuantity = item.UptoQuantity,
                        RateValue = item.RateValue,
                        FuelGroupId = item.FuelGroupId,
                        FuelGroupName = item.FuelGroup.GroupName
                    };
                    viewModel.FreightRateRangeTables.Add(freightRateRangeTable);
                }
            }
            else if (entity.FreightRateRuleType == FreightRateRuleType.P2P)
            {
                var freightRatePtoPTables = new List<FreightRatePtoPTableViewModel>();
                foreach (var item in entity.FreightRatePtoPTables)
                {
                    var ptoPLocations = item.FreightRatePtoPLocations;
                    foreach (var location in ptoPLocations)
                    {
                        var fuelGroups = location.FreightRatePtoPFuelGroups;
                        foreach (var fuelGroup in fuelGroups)
                        {
                            var freightRatePtoPTable = new FreightRatePtoPTableViewModel();
                            freightRatePtoPTable.TerminalId = item.TerminalId;
                            freightRatePtoPTable.BulkPlantId = item.BulkPlantId;
                            freightRatePtoPTable.TerminalAndBulkPlantName = item.TerminalId.HasValue ? item.MstExternalTerminal.Name : item.BulkPlantLocation.Name;

                            freightRatePtoPTable.LocationName = location.Job.Name;
                            freightRatePtoPTable.LocationAddress = location.Job.Address;
                            freightRatePtoPTable.LaneID = location.LaneID;
                            freightRatePtoPTable.JobId = location.JobId;
                            freightRatePtoPTable.IsLaneRequired = location.IsLaneRequired;
                            freightRatePtoPTable.AssumedMiles = location.AssumedMiles;
                            
                            freightRatePtoPTable.FuelGroupId = fuelGroup.FuelGroupId;
                            freightRatePtoPTable.FuelGroupName = fuelGroup.FuelGroup.GroupName;
                            freightRatePtoPTable.RateValue = fuelGroup.RateValue;
                            freightRatePtoPTables.Add(freightRatePtoPTable);
                        }
                    }
                }
                viewModel.FreightRatePtoPTables = freightRatePtoPTables;

                viewModel.JobIds = freightRatePtoPTables.Select(t => t.JobId).Distinct().ToList();
            }

            return viewModel;
        }
    }
}
