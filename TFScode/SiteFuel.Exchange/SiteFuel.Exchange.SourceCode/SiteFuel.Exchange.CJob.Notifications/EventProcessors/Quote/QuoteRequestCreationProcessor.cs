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
    public class QuoteRequestCreationProcessor : BaseQuoteEventProcessor, IEmailProcessor
    {
        private NotificationQuoteViewModel viewModel;

        public EventType EventType => EventType.QuoteRequestCreated;

        public QuoteRequestCreationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetQuoteNotificationDetails(notificationEventViewModel.EntityId, notificationEventViewModel.EventType);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

            var callbackUrl = $"~/Supplier/Quote/BuyerQuoteDetails/{viewModel.QuoteId}";
            //send email to eligible suppliers
            SendFuelRequestEmailToSuppliers(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
        }

        protected virtual void SendFuelRequestEmailToSuppliers(NotificationQuoteViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            viewModel.Suppliers.AddRange(viewModel.DefaultSupplierEmailRecievers);
            viewModel.Suppliers = viewModel.Suppliers.GroupBy(x => x.Email).Select(x => x.First()).ToList();

            if (viewModel.Suppliers.Count > 0)
            {
                var notification = notificationDomain.GetEmailNotificationContent(EventType, CompanyType.Supplier, _serverUrl, callbackUrl);
                var bodyText = notification.BodyText;
                foreach (var supplier in viewModel.Suppliers)
                {
                    //to avoid duplicate emails in case Admin is creator
                    notification.BodyText = string.Format(bodyText,
                                                            $"{supplier.FirstName} {supplier.LastName}",
                                                            viewModel.BuyerQuoteNumber);

                    SendNotification(supplier.Email, notification);
                }
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                //Need to Added For Buyer
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QuoteRequestCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
