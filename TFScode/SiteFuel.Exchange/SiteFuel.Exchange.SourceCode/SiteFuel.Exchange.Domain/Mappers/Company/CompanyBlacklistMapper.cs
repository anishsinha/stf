using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class CompanyBlacklistMapper
    {
        public static CompanyBlacklistViewModel ToViewModel(this CompanyBlacklist entity, CompanyBlacklistViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new CompanyBlacklistViewModel();

            viewModel.CompanyId = entity.CompanyId;
            viewModel.CompanyName = entity.Company1.Name;
            viewModel.AddedBy = entity.AddedBy;
            viewModel.AddedByName = $"{entity.User.FirstName} {entity.User.LastName}";
            viewModel.AddedByCompanyId = entity.AddedByCompanyId;
            viewModel.AddedDate = entity.AddedDate;
            viewModel.Reason = entity.Reason;
            viewModel.RemovedBy = entity.RemovedBy;
            viewModel.RemovedByName = entity.User1 == null ? string.Empty : $"{entity.User1.FirstName} {entity.User1.LastName}";
            viewModel.RemovedDate = entity.RemovedDate;

            return viewModel;
        }

        public static CompanyBlacklist ToEntity(this CompanyBlacklistViewModel viewModel, CompanyBlacklist entity = null)
        {
            if (entity == null)
                entity = new CompanyBlacklist();

            entity.CompanyId = viewModel.CompanyId;
            entity.AddedBy = viewModel.AddedBy;
            entity.AddedByCompanyId = viewModel.AddedByCompanyId;
            entity.AddedDate = viewModel.AddedDate;
            entity.Reason = viewModel.Reason;
            entity.RemovedBy = viewModel.RemovedBy;
            entity.RemovedDate = viewModel.RemovedDate;

            return entity;
        }
    }
}
