using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class ContactPersonMapper
    {
        public static JobContactViewModel ToViewModel(this CompanyXAdditionalUserInvite entity, JobContactViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new JobContactViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.FirstName = entity.FirstName;
            viewModel.LastName = entity.LastName;
            viewModel.Email = entity.Email;
            viewModel.RoleIds = entity.MstRoles.Select(t => t.Id).ToList();
            viewModel.RoleNames = string.Join(" ,", entity.MstRoles.Select(t => t.Name).ToList());

            return viewModel;
        }

        public static CompanyXAdditionalUserInvite ToEntity(this JobContactViewModel viewModel, CompanyXAdditionalUserInvite entity = null)
        {
            if (entity == null)
                entity = new CompanyXAdditionalUserInvite();

            entity.Id = viewModel.Id;
            entity.FirstName = viewModel.FirstName;
            entity.LastName = viewModel.LastName;
            entity.Email = viewModel.Email.Trim();
            entity.InvitedBy = viewModel.InvitedById;
            entity.CompanyId = viewModel.CompanyId;
            entity.IsInvitationSent = viewModel.IsInvitationSent;

            return entity;
        }
    }
}
