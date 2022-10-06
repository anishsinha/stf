using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
	public abstract class BaseFuelRequestEventProcessor : BaseEventProcessor
	{
		protected NotificationDomain notificationDomain;

		protected virtual void SendFuelRequestEmailToSuppliers(NotificationFuelRequestCreatedViewModel viewModel, string callbackUrl, int eventTypeId, EventSubType eventSubType)
		{
			viewModel.Suppliers.AddRange(viewModel.DefaultSupplierEmailRecievers);
			viewModel.Suppliers = viewModel.Suppliers.GroupBy(x => x.Email).Select(x => x.First()).ToList();

			if (viewModel.Suppliers.Count > 0)
			{
				var notification = notificationDomain.GetNotificationContent(eventSubType, _serverUrl, callbackUrl, eventTypeId);
				var bodyText = notification.BodyText;
				foreach (var supplier in viewModel.Suppliers)
				{
                    //to avoid duplicate emails in case Admin is creator
                    if(viewModel.IsMarineLocation)
                        bodyText = bodyText.Replace("fuel request", "nomination").Replace("Fuel Request", "Nomination").Replace("Fuel request", "Nomination");

                    notification.BodyText = string.Format(bodyText,
															$"{supplier.FirstName} {supplier.LastName}",
															viewModel.FuelRequestNumber);

					SendNotification(supplier.Email, notification);
				}
			}
		}

		public override List<NotificationUserViewModel> LoadBuyerDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
		{
			return notificationDomain.GetBuyerDefaultRecievers(notificationEventViewModel.TriggeredByCompanyId, notificationEventViewModel.EventType);
		}

		public override List<NotificationUserViewModel> LoadSupplierDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
		{
			//throw new NotImplementedException();
			return new List<NotificationUserViewModel>();
		}
	}
}
