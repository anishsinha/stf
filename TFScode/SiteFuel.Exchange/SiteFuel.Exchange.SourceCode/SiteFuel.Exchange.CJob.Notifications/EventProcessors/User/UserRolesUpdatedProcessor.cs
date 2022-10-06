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
	public class UserRolesUpdatedProcessor : BaseUserEventProcessor, IEmailProcessor
    {
		private InvitedUserNotificationViewModel viewModel;

        public EventType EventType => EventType.UserRolesUpdated;

        public UserRolesUpdatedProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetOnboardedUser(notificationEventViewModel.EntityId, notificationEventViewModel.TriggeredByUserId);
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
                    var callbackUrl = $"~/Account/Login";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.UserRolesUpdated_OnboardedUser, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.BodyText = string.Format(notification.BodyText,
                                                $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                viewModel.InvitedByName,
                                                string.Join(",", viewModel.Roles));

                    if (SendNotificationForDefaultEvent(viewModel.User.Email, notification))
                    {
                        //Send an email to all admins of the company
                        callbackUrl = $"~/Settings/Profile/CompanyUsers";
                        notification = notificationDomain.GetNotificationContent(EventSubType.UserRolesUpdated_CompanyAdmin, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);

                        var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.InvitedCompanyId, notificationEventViewModel.EventType);
                        var bodyText = notification.BodyText;
                        foreach (var item in companyAdminList)
                        {
                            if (item.Id != viewModel.User.Id)
                            {
                                notification.BodyText = string.Format(bodyText,
                                                                $"{item.FirstName} {item.LastName}",
                                                                viewModel.InvitedByName,
                                                                $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                                string.Join(",", viewModel.Roles));
                                SendNotification(item.Email, notification, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("UserRolesUpdatedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
