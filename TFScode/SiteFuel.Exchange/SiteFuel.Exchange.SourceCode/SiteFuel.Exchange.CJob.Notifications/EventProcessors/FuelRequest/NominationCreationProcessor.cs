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
	public class NominationCreationProcessor : BaseFuelRequestEventProcessor, IEmailProcessor
    {
		private NotificationFuelRequestCreatedViewModel viewModel;

        public EventType EventType => EventType.NominationAcknowledgement;

        public NominationCreationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetFuelRequestNotificationDetails(notificationEventViewModel.EntityId, notificationEventViewModel.EventType);
		}

		public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
			
			//Send an email to company admins
			SendNominationEmailToBuyer(viewModel, notificationEventViewModel.EventType);
		}

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
			
		}

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
			
		}

        private void SendNominationEmailToBuyer(NotificationFuelRequestCreatedViewModel viewModel, EventType eventType)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            var notification = GetNotificationContent(eventType, CompanyType.Buyer);
            var bodyText = notification.BodyText;
            var suppliers = notificationDomain.GetSuppliersForNominationAcknowledgement(viewModel.Id);
            if (suppliers != null && suppliers.Any())
            {
                foreach (var supplier in suppliers)
                {
                    notification.BodyText = string.Format(bodyText, viewModel.CompanyName,
                                            string.IsNullOrWhiteSpace(viewModel.ExternalPoNumber) ? viewModel.FuelRequestNumber : viewModel.ExternalPoNumber,
                                            supplier);

                    SendNotificationForDefaultEvent(viewModel.Creator.Email, notification);
                    notificationDomain.UpdateAcknowledgementSentStatus(viewModel.Id, true);
                }
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = $"~/Buyer/FuelRequest/EditNomination?id={viewModel.Id}";
            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);

            return notification;
        }
    }
}
