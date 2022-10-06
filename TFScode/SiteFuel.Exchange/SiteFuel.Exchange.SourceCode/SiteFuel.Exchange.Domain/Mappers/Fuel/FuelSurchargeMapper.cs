using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.FuelSurcharge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
  public static class FuelSurchargeMapper
    {
        public static FuelSurchargeIndex ToEntity(this FuelSurchargeIndexViewModel viewModel, FuelSurchargeIndex entity = null)
        {
            if (entity == null)
                entity = new FuelSurchargeIndex();

            entity.Name = viewModel.TableName.Trim();
            entity.TableType = (TableTypes)viewModel.TableTypeId;
            entity.ProductId = viewModel.ProductId;
            entity.ProductType = viewModel.ProductId.HasValue ? (SurchargeProductTypes)viewModel.ProductId: SurchargeProductTypes.Unknown; // is it to save Unknown ok?
            entity.StartDate = viewModel.FuelSurchargeTable.StartDate;
            entity.EndDate = viewModel.FuelSurchargeTable.EndDate.HasValue ? viewModel.FuelSurchargeTable.EndDate.Value : new Nullable<DateTimeOffset>();
            entity.CreatedDate = DateTimeOffset.Now;
            entity.IsActive = true;
            entity.UpdatedDate = DateTimeOffset.Now;
            entity.PeriodId = viewModel.PeriodId;
            entity.AreaId = viewModel.AreaId;
            entity.IndexType = viewModel.IsManualUpdate ? IndexType.Manual : IndexType.API;
            entity.ApiAdjustIndexPriceDate = viewModel.ApiAdjustIndexPriceDate;
            entity.ApiEffectiveDate = viewModel.ApiEffectiveDate;// json save
            entity.IndexPriceDate = viewModel.IndexPriceDate;
            entity.StatusId = (FreightTableStatus)viewModel.StatusId;
            entity.ManualEffectiveDate = viewModel.ManualEffectiveDate.HasValue ? viewModel.ManualEffectiveDate.Value : new Nullable<DateTimeOffset>();
            entity.IndexPrice = viewModel.IsManualUpdate ? viewModel.ManualLatestIndexPrice : viewModel.APILatestIndexPrice;
            entity.Notes = viewModel.Notes;
            entity.SupplierCompanyId = entity.SupplierCompanyId;
            entity.CreatedBy = entity.CreatedBy;
            entity.UpdatedBy =entity.UpdatedBy;
            entity.PriceStartValue = viewModel.FuelSurchargeTable.PriceRangeStartValue.HasValue ? viewModel.FuelSurchargeTable.PriceRangeStartValue.Value : new Nullable<decimal>();
            entity.PriceEndValue = viewModel.FuelSurchargeTable.PriceRangeEndValue.HasValue ? viewModel.FuelSurchargeTable.PriceRangeEndValue.Value : new Nullable<decimal>();
            entity.PriceInterval = viewModel.FuelSurchargeTable.PriceRangeInterval.HasValue ? viewModel.FuelSurchargeTable.PriceRangeInterval.Value : new Nullable<decimal>();
            entity.SurchargeStartPercent = viewModel.FuelSurchargeTable.FuelSurchargeStartPercentage.HasValue ? viewModel.FuelSurchargeTable.FuelSurchargeStartPercentage.Value : new Nullable<decimal>();
            entity.SurchargeInterval = viewModel.FuelSurchargeTable.SurchargeInterval.HasValue ? viewModel.FuelSurchargeTable.SurchargeInterval.Value : new Nullable<decimal>();
            return entity;
        }

        public static FuelSurchargeIndexViewModel ToViewModel(this FuelSurchargeIndex entity, FuelSurchargeIndexViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelSurchargeIndexViewModel();
            viewModel.FuelSurchargeIndexId = entity.Id;
            viewModel.TableName = entity.Name;
            viewModel.TableTypeId = (int)entity.TableType;
            if (entity.ProductId.HasValue)
                viewModel.ProductId = entity.ProductId.Value;

            if (entity.PeriodId.HasValue)
                viewModel.PeriodId = entity.PeriodId.Value;

            if (entity.AreaId.HasValue)
                viewModel.AreaId = entity.AreaId.Value;
            
            viewModel.ApiAdjustIndexPriceDate = entity.ApiAdjustIndexPriceDate;
            //return saved json string
            viewModel.ApiEffectiveDate = entity.ApiEffectiveDate;

            viewModel.IndexPriceDate = entity.IndexPriceDate;
            if (entity.IndexType == IndexType.Manual)
            {
                viewModel.ManualLatestIndexPrice = entity.IndexPrice;
                viewModel.IsManualUpdate = true;
            }
            else
            {
                viewModel.APILatestIndexPrice = entity.IndexPrice;
                viewModel.IsManualUpdate = false;
            }
            
            viewModel.StatusId = (int)(entity.StatusId);
            viewModel.ManualEffectiveDate = entity.ManualEffectiveDate;

            viewModel.Notes = entity.Notes;

            var fuelSurchargeTable = new FuelSurchargeTableModel();
            fuelSurchargeTable.StartDate = entity.StartDate;
            fuelSurchargeTable.EndDate = entity.EndDate;
            fuelSurchargeTable.PriceRangeStartValue = entity.PriceStartValue;
            fuelSurchargeTable.PriceRangeEndValue = entity.PriceEndValue;
            fuelSurchargeTable.PriceRangeInterval = entity.PriceInterval;
            fuelSurchargeTable.FuelSurchargeStartPercentage = entity.SurchargeStartPercent;
            fuelSurchargeTable.SurchargeInterval = entity.SurchargeInterval;
            viewModel.FuelSurchargeTable = fuelSurchargeTable;

            var customer = entity.FreightTableCompanies.Where(t => t.AssignedCompanyType == AssignedCompanyType.Customer).ToList();
            viewModel.Customers = new List<DropdownDisplayItem>();
            foreach (var item in customer)
            {
                viewModel.Customers.Add(new DropdownDisplayItem { Id = item.AssignedCompanyId, Name = item.AssignedCompany.Name });
            }

            var carrier = entity.FreightTableCompanies.Where(t => t.AssignedCompanyType == AssignedCompanyType.Carrier).ToList();
            viewModel.Carriers = new List<DropdownDisplayItem>();
            foreach (var item in carrier)
            {
                viewModel.Carriers.Add(new DropdownDisplayItem { Id = item.AssignedCompanyId, Name = item.AssignedCompany.Name });
            }
            
            var sourceRegions = entity.FreightTableSourceRegions;
            viewModel.SourceRegions = new List<DropdownDisplayItem>();
            foreach (var item in sourceRegions)
            {
                viewModel.SourceRegions.Add(new DropdownDisplayItem { Id = item.SourceRegionId, Name = item.SourceRegion.Name });
            }

            var pickupLocations = entity.FreightTablePickupLocations; 
            viewModel.TerminalsAndBulkPlants = new List<DropdownDisplayExtended>();
            foreach (var item in pickupLocations)
            {
                //very trick situation , 
                //since terminal id and bulkplant id may be duplicate and angular2-multiselect control does work with duplicate Id
                //refer 1. "create-fuel-surcharge.component" and "FuelSurchargeDomain-CreateFuelSurchargeTableAsync,GetTerminalsAndBulkPlantsAsync" 
                //refer 2. "FuelSurchargeMapper-ToViewModel"
                //so have to managed to make unique in  Get and save functionality i.e BulkPlants_Id,Terminals_Id.  
                if (item.TerminalId.HasValue && item.MstExternalTerminal.IsActive)
                viewModel.TerminalsAndBulkPlants.Add(new DropdownDisplayExtended { Id = "Terminals_" + item.TerminalId.Value.ToString(), Code = "Terminals", Name = item.MstExternalTerminal.Name });
                
                if(item.BulkPlantId.HasValue && item.BulkPlantLocation.IsActive)
                viewModel.TerminalsAndBulkPlants.Add(new DropdownDisplayExtended { Id = "BulkPlants_" + item.BulkPlantId.Value.ToString(), Code = "Bulk Plants", Name = item.BulkPlantLocation.Name });
                
            }

            var generatedTables = new List<FuelSurchargeTableModel>();
            foreach (var item in entity.FuelSurchargeGeneratedTables)
            {
                var suchargeTable = new FuelSurchargeTableModel();
                suchargeTable.PriceRangeStartValue = item.PriceRangeStartValue;
                suchargeTable.PriceRangeEndValue = item.PriceRangeEndValue;
                suchargeTable.FuelSurchargeStartPercentage = item.FuelSurchargePercentage;
                generatedTables.Add(suchargeTable);
            }
            viewModel.GeneratedSurchargeTable = generatedTables;
            return viewModel;
        }


    }
}
