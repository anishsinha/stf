using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class ProductTypeMapper
    {
        public static ProductTypeViewModel ToViewModel(this MstProductType entity, ProductTypeViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new ProductTypeViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;

            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;

            return viewModel;
        }

        public static MstProductType ToEntity(this ProductTypeViewModel viewModel, MstProductType entity = null)
        {
            if (entity == null)
                entity = new MstProductType();

            entity.Id = viewModel.Id.HasValue ? viewModel.Id.Value : 0;
            entity.Name = viewModel.Name;

            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;

            return entity;
        }

        public static List<SupplierXProductSequencing> ToEntities(this ProductSequenceViewModel viewModel, UserContext userContext)
        {
            List<SupplierXProductSequencing> entities  = new List<SupplierXProductSequencing>();
            foreach (var model in viewModel.ProductSequence)
            {
                var entity = new SupplierXProductSequencing
                {
                    JobId = viewModel.JobId == 0 ? (int?)null : viewModel.JobId,
                    IsActive = true,
                    LastModifiedDate = DateTimeOffset.Now,
                    SequenceCreationMethod = viewModel.SequenceMethod,
                    SequenceNumber = model.Sequence,
                    ProductId = model.ProductTypeId,
                    OrderId = model.OrderId,
                    CreatedBy = userContext.Id,
                    UpdatedBy = userContext.Id,
                    SupplierCompanyId = userContext.CompanyId
                };
                entities.Add(entity);
            } 
            return entities;
        }
    }
}
