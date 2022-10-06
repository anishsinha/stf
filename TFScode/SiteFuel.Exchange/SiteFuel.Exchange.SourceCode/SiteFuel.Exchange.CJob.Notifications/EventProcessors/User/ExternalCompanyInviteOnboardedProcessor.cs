using Newtonsoft.Json;
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
	public class ExternalCompanyInviteOnboardedProcessor : BaseUserEventProcessor, IEmailProcessor
    {
		private InvitedUserNotificationViewModel viewModel;

        public EventType EventType => EventType.ExternalCompanyInviteOnboarded;

        public ExternalCompanyInviteOnboardedProcessor()
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
                    //Send an email to invited external user
                    var callbackUrl = $"~/Account/Login";
                    var supplierURL = string.Empty;
                    if (!string.IsNullOrEmpty(notificationEventViewModel.JsonMessage))
                    {
                        var message = JsonConvert.DeserializeObject<AddUserMessageViewModel>(notificationEventViewModel.JsonMessage);
                        supplierURL = message.SupplierURL;
                        callbackUrl = $"~/Account/SupplierLogin?supplierURL=" + supplierURL;
                    }
                    var notification = notificationDomain.GetNotificationContent(EventSubType.ExternalCompanyInviteOnboarded_Invitee, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.ApplicationTemplateId = notificationEventViewModel.ApplicationTemplateId;
                    notification.Subject = string.Format(notification.Subject, notificationEventViewModel.BrandedCompanyName);
                    notification.BodyText = string.Format(notification.BodyText,
                                                $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                string.Join(",", viewModel.Roles), notificationEventViewModel.BrandedCompanyName);

                    if (SendNotificationForDefaultEvent(viewModel.User.Email, notification))
                    {
                        if (notificationEventViewModel.EntityId != notificationEventViewModel.TriggeredByUserId)
                        {
                            var inviter = notificationDomain.GetOnboardedUser(notificationEventViewModel.TriggeredByUserId, notificationEventViewModel.EntityId);
                            notification = notificationDomain.GetNotificationContent(EventSubType.ExternalCompanyInviteOnboarded_Inviter, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                            notification.BodyText = string.Format(notification.BodyText,
                                                        $"{inviter.User.FirstName} {inviter.User.LastName}",
                                                        inviter.InvitedByName,
                                                        inviter.InvitedCompanyName);
                            SendNotification(inviter.User.Email, notification);

                        }
                        else
                        {
                            //Update the user invite table
                            notificationDomain.UpdateUserInviteStatus(notificationEventViewModel.EntityId, true);
                        }

                        //Send an email to all sales people
                        var salesEmailList = ContextFactory.Current.GetDomain<ApplicationDomain>().GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingCompanyOnboardedMailingList);
                        var salesEmails = salesEmailList.Split(';').ToList();
                        notification = notificationDomain.GetNotificationContent(EventSubType.ExternalCompanyInviteOnboarded_SalesPeople, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                        var bodyText = notification.BodyText;
                        foreach (var item in salesEmails)
                        {
                            notification.BodyText = string.Format(bodyText,
                                                            $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                            viewModel.InvitedCompanyName);
                            SendNotificationForDefaultEvent(item, notification);
                        }
                        notificationDomain.UpdateUserInviteStatus(viewModel.User.Id, true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalCompanyInviteOnboardedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
