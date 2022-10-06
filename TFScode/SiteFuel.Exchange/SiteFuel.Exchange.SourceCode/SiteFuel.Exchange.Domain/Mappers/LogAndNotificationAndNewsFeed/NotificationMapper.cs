using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationEventViewModel ToViewModel(this Notification entity, NotificationType type = NotificationType.Email, NotificationEventViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new NotificationEventViewModel();

            viewModel.Id = entity.Id;
            viewModel.EventType = (EventType)entity.EventTypeId;
            viewModel.EntityId = entity.EntityId;
            viewModel.TriggeredByUserId = entity.TriggeredBy;
            viewModel.JsonMessage = entity.JsonMessage;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.NotificationType = entity.NotificationType;
            viewModel.IsManualTrigger = entity.IsManualTrigger;
            viewModel.BrandedCompanyName = entity.MstApplicationTemplate != null ? entity.MstApplicationTemplate.BrandedCompanyName : Resource.lblTrueFill;
            viewModel.ApplicationTemplateId = entity.MstApplicationTemplate != null ? entity.MstApplicationTemplate.Id : (int)ApplicationTemplate.TrueFill;
            switch (type)
            {
                case NotificationType.Email:
                    viewModel.IsNotificationSent = entity.IsEmailNotificationSent;
                    break;
                case NotificationType.Sms:
                    viewModel.IsNotificationSent = entity.IsSmsNotificationSent;
                    break;
                case NotificationType.InApp:
                    viewModel.IsNotificationSent = entity.IsInAppNotificationSent;
                    break;
            }

            return viewModel;
        }
    }
}
