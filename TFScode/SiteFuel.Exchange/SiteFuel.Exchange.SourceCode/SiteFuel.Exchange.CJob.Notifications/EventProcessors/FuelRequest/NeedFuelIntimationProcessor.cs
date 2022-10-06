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
	public class NeedFuelIntimationProcessor : BaseFuelRequestEventProcessor, IEmailProcessor
    {
		private NotificationRequestFuelViewModel viewModel;

        public EventType EventType => EventType.NeedFuelIntimationCreatedUsingAdvertisementWidget;

        public NeedFuelIntimationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetNeedFuelIntimationDetails(notificationEventViewModel.EntityId);
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
                if (viewModel != null)
                {
                    var quantityRequested = ($"{viewModel.Quantity} { viewModel.UoM}").ToString();
                    var notification = notificationDomain.GetNotificationContent(EventSubType.NeedFuelIntimationCreated, _serverUrl, string.Empty);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    viewModel.CustomerName,
                                                    viewModel.CompanyName,
                                                    viewModel.PhoneNumber,
                                                    viewModel.Email,
                                                    viewModel.FuelType,
                                                    quantityRequested,
                                                    viewModel.UoM,
                                                    viewModel.PricePerGallon,
                                                    viewModel.Zipcode);

                    foreach (var email in viewModel.EmailRecipients)
                    {
                        SendNotification(email, notification);
                    }

                    //Update the invite table
                    notificationDomain.UpdateNeedFuelIntimationDetails(viewModel.Id, true);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NeedFuelIntimationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
