using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels.ExternalEntityMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class ExternalEntityMappingsMapper
    {
        public static ExternalCustomerMappings ToEntity(this ExternalCustomerMappingViewModel viewModel, ExternalCustomerMappings entity = null)
        {
            if (entity == null)
            {
                entity = new ExternalCustomerMappings();
            }

            entity.CustomerId = viewModel.CustomerId;
            entity.TargetCustomerValue = viewModel.TargetCustomerValue;
            entity.ThirdPartyId = viewModel.ThirdPartyId;
            entity.IsActive = true;
            entity.CreatedDate = DateTimeOffset.Now;
            return entity;
        }

        public static ExternalCustomerLocationMappings ToEntity(this ExternalCustomerLocationMappingViewModel viewModel, ExternalCustomerLocationMappings entity = null)
        {
            if (entity == null)
            {
                entity = new ExternalCustomerLocationMappings();
            }

            entity.CustomerLocationId = viewModel.CustomerLocationId;
            entity.TargetCustomerLocationValue = viewModel.TargetCustomerLocationValue;
            entity.ThirdPartyId = viewModel.ThirdPartyId;
            entity.IsActive = true;
            entity.CreatedDate = DateTimeOffset.Now;
            return entity;
        }

        public static ExternalProductMappings ToEntity(this ExternalProductMappingViewModel viewModel, ExternalProductMappings entity = null)
        {
            if (entity == null)
                entity = new ExternalProductMappings();

            if (viewModel.ProductId.HasValue && viewModel.ProductId.Value > 0)
                entity.TfxProductId = viewModel.ProductId.Value;
            else if (viewModel.OtherProductId.HasValue && viewModel.OtherProductId.Value > 0)
                entity.OtherProductId = viewModel.OtherProductId.Value;

            entity.TargetProductValue = viewModel.TargetProductValue;
            entity.ThirdPartyId = viewModel.ThirdPartyId;
            entity.IsActive = true;
            entity.CreatedDate = DateTimeOffset.Now;

            return entity;
        }
        public static ExternalTerminalMappings ToEntity(this ExternalTerminalMappingViewModel viewModel, ExternalTerminalMappings entity = null)
        {
            if (entity == null)
            {
                entity = new ExternalTerminalMappings();
            }

            entity.TerminalId = viewModel.TerminalId;
            entity.TargetTerminalValue = viewModel.TargetTerminalValue;
            entity.ThirdPartyId = viewModel.ThirdPartyId;
            entity.IsActive = true;
            entity.CreatedDate = DateTimeOffset.Now;
            return entity;
        }
        public static ExternalSupplierMappings ToEntity(this ExternalSupplierMappingViewModel viewModel, ExternalSupplierMappings entity = null)
        {
            if (entity == null)
            {
                entity = new ExternalSupplierMappings();
            }

            entity.SupplierId = viewModel.SupplierId;
            entity.TargetSupplierValue = viewModel.TargetSupplierValue;
            entity.ThirdPartyId = viewModel.ThirdPartyId;
            entity.IsActive = true;
            entity.CreatedDate = DateTimeOffset.Now;
            return entity;
        }
        public static ExternalBulkPlantMappings ToEntity(this ExternalBulkPlantMappingViewModel viewModel, ExternalBulkPlantMappings entity = null)
        {
            if (entity == null)
            {
                entity = new ExternalBulkPlantMappings();
            }

            entity.BulkPlantId = viewModel.BulkPlantId;
            entity.TargetBulkPlantValue = viewModel.TargetBulkPlantValue;
            entity.ThirdPartyId = viewModel.ThirdPartyId;
            entity.IsActive = true;
            entity.CreatedDate = DateTimeOffset.Now;
            return entity;
        }
        public static ExternalCarrierMappings ToEntity(this ExternalCarrierMappingViewModel viewModel, ExternalCarrierMappings entity = null)
        {
            if (entity == null)
            {
                entity = new ExternalCarrierMappings();
            }

            entity.CarrierId = viewModel.CarrierId;
            entity.TargetCarrierValue = viewModel.TargetCarrierValue;
            entity.ThirdPartyId = viewModel.ThirdPartyId;
            entity.IsActive = true;
            entity.CreatedDate = DateTimeOffset.Now;
            return entity;
        }
        public static ExternalDriverMappings ToEntity(this ExternalDriverMappingViewModel viewModel, ExternalDriverMappings entity = null)
        {
            if (entity == null)
            {
                entity = new ExternalDriverMappings();
            }

            entity.DriverId = viewModel.DriverId;
            entity.TargetDriverValue = viewModel.TargetDriverValue;
            entity.ThirdPartyId = viewModel.ThirdPartyId;
            entity.IsActive = true;
            entity.CreatedDate = DateTimeOffset.Now;
            return entity;
        }
    }
}
