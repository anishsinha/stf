using Newtonsoft.Json;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;


namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class LoadOptimizationUserMapper
    {
        public static LoadOptimizationUserViewModel ToViewModel(this LoadOptimizationUser entity, LoadOptimizationUserViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new LoadOptimizationUserViewModel();

            viewModel.Id = entity.Id;
            viewModel.CompanyId = entity.CompanyId;

            if (!string.IsNullOrEmpty(entity.DistributedUsers))
                viewModel.DistributedUsers = JsonConvert.DeserializeObject<List<int>>(entity.DistributedUsers);

            return viewModel;
        }

        public static LoadOptimizationUser ToEntity(this LoadOptimizationUserViewModel viewModel, int companyId, int userId,  LoadOptimizationUser entity = null)
        {
            if (entity == null)
                entity = new LoadOptimizationUser();

            entity.Id = viewModel.Id;
            entity.CompanyId = companyId;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.CreatedBy = userId;
            if (viewModel.DistributedUsers.AnyAndNotNull())
                entity.DistributedUsers = JsonConvert.SerializeObject(viewModel.DistributedUsers);

            return entity;
        }
    }
}
