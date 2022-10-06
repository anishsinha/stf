using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
	public class DeliveryRequestCreationProcessor : BaseDeliveryScheduleEventProcessor, IEmailProcessor
    {
		private NotificationDeliveryRequestViewModel viewModel;

        public EventType EventType => EventType.TankDeliveryRequestCreated;
        private string userName;
        public DeliveryRequestCreationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var message = JsonConvert.DeserializeObject<TankDeliveryRequestMessageViewModel>(notificationEventViewModel.JsonMessage);

            viewModel = Task.Run(() => notificationDomain.GetTankDeliveryRequestNotificationDetails(message.EntityId)).Result;

            userName = ContextFactory.Current.GetDomain<HelperDomain>().GetUserNameById(notificationEventViewModel.TriggeredByUserId);
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
                if (viewModel.JobId > 0)
                {
                    var carrierEmails = notificationDomain.GetCarrrierUserEmails(viewModel.JobId);
                    foreach (var item in carrierEmails)
                    {
                        var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Carrier);
                        notification.Subject = GetNotificationSubjectText(notification.Subject);
                        notification.BodyText = GetNotificationBodyText(notification.BodyText, item.Name);
                        SendNotificationForDefaultEvent(item.Code, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryScheduleCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = string.Empty;
            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationSubjectText(string subjectText)
        {
            subjectText = string.Format(subjectText, viewModel.JobName);
            return subjectText;
        }

        private string GetNotificationBodyText(string bodyText,string firstName)
        {
            string quantity = string.Empty;
            if (viewModel.QuantityId == 0 || viewModel.QuantityId == (int)ScheduleQuantityType.Quantity)
                quantity = viewModel.Quantity + " " + viewModel.UoM.ToString();
            else
                quantity = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)viewModel.QuantityId);

            bodyText = string.Format(bodyText,
                                    firstName,
                                    userName,
                                    viewModel.BuyerCompanyName,
                                    viewModel.JobName,
                                    viewModel.TankName,
                                    viewModel.ProductType,
                                    viewModel.FuelType,
                                    quantity
                                    );
            return bodyText;
        }
    }
}
