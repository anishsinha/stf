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
	public class JobUpdationProcessor : BaseJobEventProcessor, IEmailProcessor
    {
		private NotificationJobViewModel viewModel;

        public EventType EventType => EventType.JobUpdated;

        public JobUpdationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetJobNotificationDetails(notificationEventViewModel.EntityId);
		}

		public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            try
            {
                viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
                
                var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.CompanyId, notificationEventViewModel.EventType);
                if (companyAdminList.Count > 0)
                {
                    var notification = GetNotificationContent();
                    var bodyText = notification.BodyText;
                    foreach (var item in companyAdminList)
                    {
                        notification.BodyText = bodyText;
                        notification.BodyText = GetNotificationBodyText(notification.BodyText, item.FirstName, notificationEventViewModel.JsonMessage);
                        SendNotification(item.Email, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobUpdationProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
            }
        }

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
			viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
		}

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                if (viewModel.AssignedTo.Any())
                {
                    var notification = GetNotificationContent();
                    var bodyText = notification.BodyText;
                    foreach (var item in viewModel.AssignedTo)
                    {
                        notification.BodyText = bodyText;
                        notification.BodyText = GetNotificationBodyText(notification.BodyText, item.FirstName, notificationEventViewModel.JsonMessage);
                        SendNotification(item.Email, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobUpdationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }


        private string GetOnsiteInfo()
        {
            return viewModel.OnsitePersons.Any() ? $"{viewModel.OnsitePersons.First().FirstName} {viewModel.OnsitePersons.First().LastName}"
                                                         : string.Empty;
        }

        private string GetApproverInfo()
        {
            return viewModel.IsApprovalWorkflowEnabled ? $"{viewModel.ApproverUser.FirstName} {viewModel.ApproverUser.LastName}"
                                                                : string.Empty;
        }

        private NotificationViewModel GetNotificationContent()
        {
            var callbackUrl = $"~/Buyer/Job/Details/{viewModel.Id}";
            var notification = notificationDomain.GetEmailNotificationContent(EventType.JobUpdated, CompanyType.Buyer, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationBodyText(string bodyText, string firstName, string jsonMessage)
        {
            var proFormaStatus = string.Empty;
            if (!string.IsNullOrEmpty(jsonMessage))
            {
                var jobMessage = JsonConvert.DeserializeObject<JobMessageViewModel>(jsonMessage);
                proFormaStatus = jobMessage.ProFormaPoStatus;
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 3);
                if (jobMessage.ProFormaPoStatus == "Disabled")
                    bodyText = notificationDomain.RemoveBodyContent(bodyText, 4);
                else
                    bodyText = notificationDomain.ReplaceBodyContent(bodyText, 4);
            }
            else
            {
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 3);
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 4);
            }

            bodyText = string.Format(bodyText,
                                    firstName,
                                    $"{viewModel.Creator.FirstName} {viewModel.Creator.LastName}",
                                    viewModel.Name, GetOnsiteInfo(), GetApproverInfo(), proFormaStatus);
            bodyText = viewModel.OnsitePersons.Any() ? notificationDomain.ReplaceBodyContent(bodyText, 1) : notificationDomain.RemoveBodyContent(bodyText, 1);
            bodyText = viewModel.IsApprovalWorkflowEnabled ? notificationDomain.ReplaceBodyContent(bodyText, 2) : notificationDomain.RemoveBodyContent(bodyText, 2);
            return bodyText;
        }
    }
}
