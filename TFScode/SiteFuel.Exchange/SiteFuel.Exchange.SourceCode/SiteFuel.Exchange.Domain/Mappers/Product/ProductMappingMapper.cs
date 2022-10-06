using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class ProductMappingMapper
    {
        public static SupplierMappedProductDetails ToEntity(this ProductMappingViewModel viewModel, int? terminalId, int fuelTypeId, SupplierMappedProductDetails entity = null)
        {
            if (entity == null)
                entity = new SupplierMappedProductDetails();

            entity.Id = viewModel.Id;
            entity.MyProductId = string.IsNullOrWhiteSpace(viewModel.MyProductId) ? null : viewModel.MyProductId;
            entity.BackOfficeProductId = string.IsNullOrWhiteSpace(viewModel.BackOfficeProductId) ? null : viewModel.BackOfficeProductId;
            entity.DriverProductId = string.IsNullOrWhiteSpace(viewModel.DriverProductId) ? null : viewModel.DriverProductId;
            entity.CompanyId = viewModel.CompanyId;
            entity.TerminalId = terminalId;
            entity.FuelTypeId = fuelTypeId;
            entity.IsActive = viewModel.IsActive;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
           // entity.TerminalItemCode = viewModel.TerminalItemCode;
            return entity;
        }

        public static ProductMappingViewModel ToViewModel(this ProductMappingBulkCsvViewModel viewModel, ProductMappingViewModel entity = null)
        {
            if (entity == null)
                entity = new ProductMappingViewModel();

            entity.MyProductId = viewModel.MyProductId;
            entity.BackOfficeProductId = viewModel.BackOfficeProductId;
            entity.DriverProductId = viewModel.DriverProductId;
          //  entity.TerminalItemCode = viewModel.TerminalItemCode;
            entity.CompanyId = viewModel.CompanyId;
            if (entity.States == null)
                entity.States = new List<DropdownDisplayItem>();
            entity.States.Add(new DropdownDisplayItem() { Id = viewModel.StateId });
            if (entity.Cities == null)
                entity.Cities = new List<DropdownDisplayExtendedItem>();
            entity.Cities.Add(new DropdownDisplayExtendedItem() { Name = viewModel.City });
            if (entity.Terminals == null)
                entity.Terminals = new List<DropdownDisplayItem>();
            if (viewModel.TerminalId.HasValue && viewModel.TerminalId !=null && viewModel.TerminalId.Value > 0)
            {
                entity.Terminals.Add(new DropdownDisplayItem() { Id = viewModel.TerminalId.Value });
            }
           
            if (entity.FuelTypes == null)
                entity.FuelTypes = new List<DropdownDisplayItem>();
            entity.FuelTypes.Add(new DropdownDisplayItem() { Id = viewModel.FuelTypeId });
            entity.IsBulkUploadRequest = true;
            entity.RowNumber = viewModel.RowNumber;
            return entity;
        }
    }
}
