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
	public class AdditionalUserUpdatedProcessor : BaseUserEventProcessor, IEmailProcessor
    {
		private InvitedUserNotificationViewModel viewModel;

        public EventType EventType => EventType.AdditionalUserUpdated;

        public AdditionalUserUpdatedProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetAdditionalUser(notificationEventViewModel.EntityId);
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
                //Send an email to invited user of the company
                if (viewModel.User != null)
                {
                    var callbackUrl = $"~/Account/RegisterCompanyUser?id={viewModel.User.Id}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.AdditionalUserUpdated_InvitedUser, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.BodyText = string.Format(notification.BodyText,
                                                $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                viewModel.InvitedByName,
                                                viewModel.InvitedCompanyName,
                                                viewModel.InvitedCompanyName,
                                                string.Join(",", viewModel.Roles));

                    if (SendNotificationForDefaultEvent(viewModel.User.Email, notification))
                    {
                        //Send an email to all admins of the company
                        callbackUrl = $"~/Settings/Profile/CompanyUsers";
                        notification = notificationDomain.GetNotificationContent(EventSubType.AdditionalUserUpdated_CompanyAdmin, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                        var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.InvitedCompanyId, notificationEventViewModel.EventType);
                        var bodyText = notification.BodyText;
                        foreach (var item in companyAdminList)
                        {
                            notification.BodyText = string.Format(bodyText,
                                                            $"{item.FirstName} {item.LastName}",
                                                            viewModel.InvitedByName,
                                                            $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                            $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                            string.Join(",", viewModel.Roles));
                            SendNotification(item.Email, notification, true);
                        }

                        //Update the invite table
                        notificationDomain.UpdateAdditionalUserStatus(viewModel.User.Id, true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AdditionalUserUpdatedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
