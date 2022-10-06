using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class UserNotificationSettingMapper
    {
        public static UserNotificationSettingViewModel ToViewModel(this UserXNotificationSetting entity, UserNotificationSettingViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new UserNotificationSettingViewModel(Status.Success);

            viewModel.UserId = entity.UserId;
            viewModel.EventTypeId = entity.EventTypeId;
            viewModel.EventTypeName = entity.MstEventType.Name;
            viewModel.IsEmail = entity.IsEmail;
            viewModel.IsSMS = entity.IsSMS;
            viewModel.IsInApp = entity.IsInApp;
            return viewModel;
        }

        public static UserXNotificationSetting ToEntity(this UserNotificationSettingViewModel viewModel, UserXNotificationSetting entity = null)
        {
            if (entity == null)
                entity = new UserXNotificationSetting();

            entity.UserId = viewModel.UserId;
            entity.EventTypeId = viewModel.EventTypeId;
            entity.IsEmail = viewModel.IsEmail;
            entity.IsSMS = viewModel.IsSMS;
            entity.IsInApp = viewModel.IsInApp;

            return entity;
        }

        public static TemplateViewModel ToViewModel(this MstNotificationTemplate entity, TemplateViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TemplateViewModel();

            viewModel.id = entity.Id;
            viewModel.Subject = entity.Subject;
            if (viewModel.NotificationType == (int)NotificationType.Email)
                viewModel.Body = entity.Body;
            else if (viewModel.NotificationType == (int)NotificationType.Sms)
                viewModel.Body = entity.SmsText;
            viewModel.ButtonText = entity.ButtonText;
            return viewModel;
        }
    }
}
