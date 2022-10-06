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
	public class NewIncomingFuelRequestProcessor : BaseSuperAdminEventProcessor, IEmailProcessor
    {
		private NotificationFuelRequestCreatedViewModel viewModel;

        public EventType EventType => EventType.NewIncomingFuelRequest;

        public NewIncomingFuelRequestProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetFuelRequestNotificationDetails(notificationEventViewModel.EntityId, notificationEventViewModel.EventType);
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
                var superAdminEmailList = ContextFactory.Current.GetDomain<ApplicationDomain>().GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingNewIncomingFRMailingList);
                if (viewModel.Id > 0)
                {
                    //Send an email for Order Start
                    var callbackUrl = $"~/SuperAdmin/SuperAdmin/FuelRequestDetails?id={viewModel.Id}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.NewIncomingFuelRequest_SuperAdmin, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.BodyText = string.Format(notification.BodyText, viewModel.CompanyName,viewModel.FuelRequestNumber, 
                        viewModel.DeliveryStartDate, viewModel.DeliveryStartTime);
                    notification.ShowHelpLineInfo = false;
                    SendNotificationForDefaultEvent(superAdminEmailList, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NewIncomingFuelRequestProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
