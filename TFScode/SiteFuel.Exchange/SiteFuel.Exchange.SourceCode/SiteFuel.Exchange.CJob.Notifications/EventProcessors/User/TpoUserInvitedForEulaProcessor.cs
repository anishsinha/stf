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
	public class TpoUserInvitedForEulaProcessor : BaseUserEventProcessor, IEmailProcessor
    {
		private InvitedUserNotificationViewModel viewModel;

        public EventType EventType => EventType.TPOUserInvitedForEULAAcceptance;

        public TpoUserInvitedForEulaProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetNotificationDetailsById(notificationEventViewModel.Id);
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
                    //Send an email to invited user
                    var callbackUrl = $"~/Account/BuyerEULA?id={viewModel.User.Id}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.TPOUserInvitedForEULAAcceptance, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.BodyText = string.Format(notification.BodyText,
                                                $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                viewModel.InvitedByName,
                                                viewModel.InvitedCompanyName,
                                                viewModel.User.Email,
                                                viewModel.User.Password,
                                                viewModel.SupplierCode);

                    if (SendNotificationForDefaultEvent(viewModel.User.Email, notification))
                    {
                        notificationDomain.UpdateUserInviteStatus(viewModel.User.Id, true);

                        notificationDomain.UpdateInvitationLinkSentStatus(viewModel.User.CompanyId, viewModel.InvitedCompanyId);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TpoUserInvitedForEulaProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
