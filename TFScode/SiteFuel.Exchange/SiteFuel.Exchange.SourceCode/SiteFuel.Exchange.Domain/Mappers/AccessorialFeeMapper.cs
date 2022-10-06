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
    public static class AccessorialFeeMapper
    {
        public static AccessorialFee ToEntity(this AccessorialFeeViewModel viewModel, int companyId, int userId, AccessorialFee entity = null)
        {
            if (entity == null)
                entity = new AccessorialFee();

            entity.Id = viewModel.Id;
            entity.Name = viewModel.Name;
            entity.TableType = viewModel.TableType;
            entity.SupplierCompanyId = companyId;
            entity.StatusId = viewModel.Status;
            entity.StartDate = viewModel.StartDate;
            entity.EndDate = viewModel.EndDate;
            entity.Version = viewModel.Version == 0 ? 1 : viewModel.Version;
            entity.CreatedBy = userId;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.IsActive = true;
            entity.UpdatedBy = userId;
            entity.UpdatedDate = entity.CreatedDate;

            foreach (var item in viewModel.TerminalsAndBulkPlants)
            {
                FreightTablePickupLocation freightTablePickupLocation = new FreightTablePickupLocation
                {
                    BulkPlantId = item.Code == "Bulk Plants" ? Int32.Parse(item.Id.Split("_".ToCharArray())[1]) : new Nullable<int>(),
                    TerminalId = item.Code == "Terminals" ? Int32.Parse(item.Id.Split("_".ToCharArray())[1]) : new Nullable<int>(),
                    IsActive = true
                };
                entity.FreightTablePickupLocations.Add(freightTablePickupLocation);
            }

            foreach (var item in viewModel.SourceRegionIds)
            {
                FreightTableSourceRegion freightTableSourceRegion = new FreightTableSourceRegion
                {
                    SourceRegionId = item                    
                };
                entity.FreightTableSourceRegions.Add(freightTableSourceRegion);
            }

            foreach (var item in viewModel.CustomerIds)
            {
                FreightTableCompany freightTableCompany = new FreightTableCompany
                {
                    AssignedCompanyId = item,
                    IsActive = true,
                    AssignedCompanyType = AssignedCompanyType.Customer
                };
                entity.FreightTableCompanies.Add(freightTableCompany);
            }

            foreach (var item in viewModel.CarrierIds)
            {
                FreightTableCompany freightTableCompany = new FreightTableCompany
                {
                    AssignedCompanyId = item,
                    IsActive = true,
                    AssignedCompanyType = AssignedCompanyType.Carrier
                };
                entity.FreightTableCompanies.Add(freightTableCompany);
            }

            //fees
            var fuelFees = viewModel.Fees.ToList().ToInvoiceFees(DateTimeOffset.Now);
            
            //fuelFees.ForEach(t => { t.Currency = ; t.UoM = ; });
            //fuelFees.SelectMany(t => t.FeeByQuantities).ToList().ForEach(t =>
            //{
            //    t.Currency = ;
            //    t.UoM = ;
            //});

            entity.FuelFees = fuelFees.Select(t => t.ToEntity()).ToList();


            return entity;
        }

        public static AccessorialFeeViewModel ToViewModel(this AccessorialFee entity, AccessorialFeeViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AccessorialFeeViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.TableType = entity.TableType;
            viewModel.Status = entity.StatusId;
            viewModel.StartDate = entity.StartDate;
            viewModel.EndDate = entity.EndDate;
            viewModel.Version = entity.Version;
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

            viewModel.Fees = entity.FuelFees.ToFeesViewModel();
            return viewModel;
        }
    }
}
