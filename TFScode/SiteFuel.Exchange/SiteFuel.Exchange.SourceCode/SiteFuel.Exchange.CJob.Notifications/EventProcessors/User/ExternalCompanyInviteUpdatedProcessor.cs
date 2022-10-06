using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
	public class ExternalCompanyInviteUpdatedProcessor : BaseUserEventProcessor, IEmailProcessor
    {
		private InvitedUserNotificationViewModel viewModel;

        public EventType EventType => EventType.ExternalCompanyInviteUpdated;

        public ExternalCompanyInviteUpdatedProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetUserInvite(notificationEventViewModel.EntityId);
		}

		public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
        }

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
			viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
		}

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                if (viewModel.User.Id > 0)
                {
                    //Send an email to invited external user
                    var callbackUrl = $"~/Account/RegisterInvitedCompanyUser?id={viewModel.User.Id}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.ExternalCompanyInviteUpdated, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.BodyText = string.Format(notification.BodyText,
                                                $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                viewModel.InvitedByName,
                                                viewModel.InvitedCompanyName);

                    if (SendNotificationForDefaultEvent(viewModel.User.Email, notification))
                    {
                        //Update the invite table
                        notificationDomain.UpdateUserInviteStatus(viewModel.User.Id, true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalCompanyInviteUpdatedProcessor", "ProcessExternalCompanyInviteUpdated", "Exception Details : ", ex);
            }
        }
    }
}
