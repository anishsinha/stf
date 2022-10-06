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
	public class JobAssignmentProcessor : BaseJobEventProcessor, IEmailProcessor
    {
		private NotificationJobViewModel viewModel;

        public EventType EventType => EventType.JobAssignment;

        public JobAssignmentProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetJobAssignmentNotificationDetails(notificationEventViewModel);
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
                if (viewModel.Id > 0)
                {
                    //Send an email to FR creator
                    var callbackUrl = $"~/Buyer/Job/Details/{viewModel.Id}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.JobAssignment, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.Creator.FirstName} {viewModel.Creator.LastName}",
                                                    viewModel.CompanyName,
                                                    viewModel.Name);

                    SendNotification(viewModel.Creator.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobAssignmentProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
