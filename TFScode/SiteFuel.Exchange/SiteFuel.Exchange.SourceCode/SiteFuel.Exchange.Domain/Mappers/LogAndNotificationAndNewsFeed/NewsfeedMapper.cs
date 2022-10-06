using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public static class NewsfeedMapper
    {
        public static Newsfeed ToEntity(this NewsfeedViewModel viewModel, Newsfeed entity = null)
        {
            if (entity == null)
                entity = new Newsfeed();

            entity.Id = viewModel.Id;
            entity.EventId = (int)viewModel.EventId;
            entity.EntityId = viewModel.EntityId;
            entity.EntityTypeId = (int)viewModel.EntityTypeId;
            entity.TargetEntityId = viewModel.TargetEntityId;
            entity.RecipientCompanyId = viewModel.RecipientCompanyId;
            entity.FeedMessage = viewModel.FeedMessage;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.IsRead = viewModel.IsRead;
            entity.IsActive = viewModel.IsActive;
            return entity;
        }

        public static NewsfeedViewModel ToViewModel(this Newsfeed entity, NewsfeedViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new NewsfeedViewModel();

            viewModel.Id = entity.Id;
            viewModel.EventId = (NewsfeedEvent)entity.EventId;
            viewModel.EntityId = entity.EntityId;
            viewModel.EntityTypeId = (EntityType)entity.EntityTypeId;
            viewModel.TargetEntityId = entity.TargetEntityId;
            viewModel.RecipientCompanyId = entity.RecipientCompanyId;
            viewModel.FeedMessage = entity.FeedMessage;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.IsRead = entity.IsRead;

            return viewModel;
        }

        public static NewsfeedListViewModel ToListViewModel(this Newsfeed entity, NewsfeedListViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new NewsfeedListViewModel();

            viewModel.Id = entity.Id;
            viewModel.EventId = (NewsfeedEvent)entity.EventId;
            viewModel.EntityId = entity.EntityId;
            viewModel.EntityTypeId = (EntityType)entity.EntityTypeId;
            viewModel.TargetEntityId = entity.TargetEntityId;
            viewModel.RecipientCompanyId = entity.RecipientCompanyId;
            viewModel.FeedMessage = entity.FeedMessage;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = entity.CreatedDate.ToString(Resource.constFormatDateTime);
            viewModel.IsRead = entity.IsRead;

            return viewModel;
        }
    }
}
