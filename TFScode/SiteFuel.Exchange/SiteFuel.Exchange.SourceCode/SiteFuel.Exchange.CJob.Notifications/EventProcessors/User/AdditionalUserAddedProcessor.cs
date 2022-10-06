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
	public class AdditionalUserAddedProcessor : BaseUserEventProcessor, IEmailProcessor
    {
		private InvitedUserNotificationViewModel viewModel;

        public EventType EventType => EventType.AdditionalUserAdded;

        public AdditionalUserAddedProcessor()
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
                if (viewModel.User.Id > 0)
                {
                    var callbackUrl = $"~/Account/RegisterCompanyUser?id={viewModel.User.Id}";
                    var supplierURL = string.Empty;
                    if (!string.IsNullOrEmpty(notificationEventViewModel.JsonMessage))
                    {
                        var message = JsonConvert.DeserializeObject<AddUserMessageViewModel>(notificationEventViewModel.JsonMessage);
                        supplierURL = message.SupplierURL;
                        callbackUrl = $"~/Account/RegisterCompanyUser?id={viewModel.User.Id}&supplierURL=" + supplierURL;
                    }
                    var notification = notificationDomain.GetNotificationContent(EventSubType.AdditionalUserAdded_InvitedUser, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType, supplierURL);
                    notification.ApplicationTemplateId = notificationEventViewModel.ApplicationTemplateId;
                    notification.Subject = string.Format(notification.Subject, notificationEventViewModel.BrandedCompanyName);
                    notification.BodyText = string.Format(notification.BodyText,
                                                $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                viewModel.InvitedByName,
                                                string.Join(",", viewModel.Roles), notificationEventViewModel.BrandedCompanyName);

                    if (SendNotificationForDefaultEvent(viewModel.User.Email, notification))
                    {
                        //Send an email to all admins of the company
                        callbackUrl = $"~/Settings/Profile/CompanyUsers";
                        notification = notificationDomain.GetNotificationContent(EventSubType.AdditionalUserAdded_CompanyAdmin, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType, supplierURL);

                        var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.InvitedCompanyId, notificationEventViewModel.EventType);
                        var bodyText = notification.BodyText;
                        var subject = notification.Subject;
                        foreach (var item in companyAdminList)
                        {
                            notification.ApplicationTemplateId = notificationEventViewModel.ApplicationTemplateId;
                            notification.Subject = string.Format(subject, notificationEventViewModel.BrandedCompanyName);
                            notification.BodyText = string.Format(bodyText,
                                                            $"{item.FirstName} {item.LastName}",
                                                            $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                            viewModel.InvitedByName,
                                                            $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                            string.Join(",", viewModel.Roles), notificationEventViewModel.BrandedCompanyName);
                            SendNotification(item.Email, notification, true);
                        }

                        //Update the invite table
                        notificationDomain.UpdateAdditionalUserStatus(viewModel.User.Id, true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AdditionalUserAddedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
