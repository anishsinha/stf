using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
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
    public class CarrierEmailOrderCreationProcessor : BaseDeliveryScheduleEventProcessor, IEmailProcessor
    {
        private NotificationOrderViewModel viewModel;

        public EventType EventType => EventType.CarrierEmailOrderCreated;

        public CarrierEmailOrderCreationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = Task.Run(() => notificationDomain.GetOrderNotificationDetailsForCarrierEmail(notificationEventViewModel.EntityId)).Result;
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
                        notification.BodyText = GetNotificationBodyText(notification.BodyText, item.Name);
                        //SendNotificationForDefaultEvent(item.Code, notification);
                        SendNotification(item.Code, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierEmailOrderCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = string.Empty;
            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationBodyText(string bodyText, string firstName)
        {
            var specailInstructions = @Resource.lblSingleHyphen;
            if (viewModel.SpecialInstructions != null && viewModel.SpecialInstructions.Count > 0)
            {
                specailInstructions = string.Empty;
                int i = 0;
                foreach (var item in viewModel.SpecialInstructions)
                {
                    i = i + 1;
                    specailInstructions = specailInstructions + "<br/>" + i + ") " + item;
                }
            }
            bodyText = string.Format(bodyText,
                                    firstName,
                                    viewModel.SupplierUser.FirstName + " " + viewModel.SupplierUser.LastName,
                                    viewModel.OrderName,
                                    viewModel.PoNumber,
                                    viewModel.TerminalName,
                                    viewModel.DeliveryStartDate,
                                    viewModel.BuyerCompanyName,
                                    viewModel.JobName,
                                    viewModel.OrderType,
                                    viewModel.FuelType,
                                    viewModel.Quantity,
                                    specailInstructions
                                    );
            return bodyText;
        }
    }
}
