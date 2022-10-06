using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class TerminalMapper
    {
        public static AddressViewModel ToAddressViewModel(this MstExternalTerminal entity, AddressViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AddressViewModel(Status.Success);
            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.StateCode = entity.StateCode;
            viewModel.CountryCode = entity.CountryCode;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.CountyName = entity.CountyName;
            return viewModel;
        }

        public static MstTerminalItemDescription ToItemDescriptionEntity(this TerminalSupplierViewModel viewModel, MstTerminalItemDescription entity = null)
        {
            if(entity == null)
                entity = new MstTerminalItemDescription();

            entity.Code = viewModel.Code;
            entity.Name = viewModel.Name?.Trim();
            entity.ProductTypeId = viewModel.ProductTypeId;
            entity.CountryId = viewModel.Country;
            entity.AddedBy = viewModel.AddedBy;
            entity.AddedDate = viewModel.AddedDate;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.IsActive = viewModel.IsActive;

            return entity;
        }

        public static TerminalItemCodeMappingViewModel ToItemCodeViewModel(this TerminalItemCodeMappingViewModel viewModel)
        {
            var entity = new TerminalItemCodeMappingViewModel();

            entity.TerminalSupplierId = viewModel.TerminalSupplierId;
            entity.ItemDescriptionId = viewModel.ItemDescriptionId;
            entity.ItemCode = viewModel.ItemCode;
            entity.CompanyId = viewModel.CompanyId;
            entity.EffectiveDate = viewModel.EffectiveDate;
            entity.ExpiryDate = viewModel.ExpiryDate;
            entity.AddedBy = viewModel.AddedBy;
            entity.AddedDate = viewModel.AddedDate;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.IsActive = viewModel.IsActive;

            return entity;
        }

        public static TerminalItemCodeMapping ToItemCodeMappingEntity(this TerminalItemCodeMappingViewModel viewModel, TerminalItemCodeMapping entity = null)
        {
            if(entity == null)
                entity = new TerminalItemCodeMapping();

            entity.TerminalSupplierId = viewModel.TerminalSupplierId;
            entity.ItemDescriptionId = viewModel.ItemDescriptionId;
            entity.ItemCode = viewModel.ItemCode;
            entity.EffectiveDate = viewModel.EffectiveDate;
            entity.ExpiryDate = viewModel.ExpiryDate;
            entity.AddedBy = viewModel.AddedBy;
            entity.AddedDate = viewModel.AddedDate;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.IsActive = viewModel.IsActive;
            entity.CompanyId = viewModel.CompanyId;
            return entity;
        }

        public static MstTerminalSupplier ToTerminalSupplierEntity(this TerminalSupplierViewModel viewModel, MstTerminalSupplier entity = null)
        {
            if (entity == null)
                entity = new MstTerminalSupplier();

            entity.Code = viewModel.Code;
            entity.Name = viewModel.Name?.Trim();
            entity.CountryId = viewModel.Country;
            entity.AddedBy = viewModel.AddedBy;
            entity.AddedDate = viewModel.AddedDate;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.IsActive = viewModel.IsActive;

            return entity;
        }

        public static TerminalItemCodeMappingViewModel ToItemCodeMappingViewModel(this TerminalItemCodeMappingBulkCsvViewModel viewModel)
        {
            var entity = new TerminalItemCodeMappingViewModel();

            entity.TerminalSupplierId = viewModel.TerminalSupplierId;
            entity.ItemDescriptionId = viewModel.ItemDescriptionId;
            entity.ItemCode = viewModel.TerminalItemCode;
            entity.CompanyId = viewModel.CompanyId;
         
            return entity;
        }

        public static TerminalSupplierViewModel ToTerminalSupplierViewModel(this MstTerminalSupplier entity, TerminalSupplierViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TerminalSupplierViewModel();
            viewModel.Id = entity.Id;
            viewModel.Code = entity.Code;
            viewModel.Name = entity.Name?.Trim();
            viewModel.Country = entity.CountryId;
            viewModel.UpdatedDate = entity.UpdatedDate;
            return viewModel;
        }

        public static TerminalSupplierViewModel ToTerminalItemDescGridViewModel(this MstTerminalItemDescription entity, TerminalSupplierViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TerminalSupplierViewModel();
            viewModel.Id = entity.Id;
            viewModel.Code = entity.Code;
            viewModel.Name = entity.Name?.Trim();
            viewModel.Country = entity.CountryId;
            viewModel.ProductTypeId = entity.ProductTypeId;
            viewModel.ProductTypeName = entity.MstProductType.Name;
            return viewModel;
        }

        public static MstExternalTerminal ToTerminalViewModel(this MstExternalTerminal entity, MstExternalTerminal viewModel = null)
        {

            if (viewModel == null)
                viewModel = new MstExternalTerminal();
            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.Latitude = entity.Latitude;
            viewModel.Longitude = entity.Longitude;
            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.StateCode = entity.StateCode;
            viewModel.StateId = entity.StateId;
            viewModel.CountryCode = entity.CountryCode;
            viewModel.CountyName = entity.CountyName;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.Abbreviation = entity.Abbreviation;
            viewModel.Code = entity.Code;
            viewModel.ControlNumber = entity.ControlNumber;
            viewModel.Currency = entity.Currency;
            viewModel.PricingSourceId = entity.PricingSourceId;
            viewModel.TerminalOwner = viewModel.TerminalOwner;

            return viewModel;
        }
    }
}
