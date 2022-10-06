using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class InActiveDriverAssignedToOrderProcessor : BaseOrderEventProcessor, IEmailProcessor
    {
        private InActiveDriverViewModel viewModel;

        public EventType EventType => EventType.InActiveDriverAssignedToOrder;

        public InActiveDriverAssignedToOrderProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            viewModel = ContextFactory.Current.GetDomain<PushNotificationDomain>().EmailNotificationForInActiveDriver(notificationEventViewModel);
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
                    var callbackUrl = "";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.DriverNotOnboarded_Supplier, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);

                    var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.CompanyId, EventType);
                    if (companyAdminList.Count > 0)
                    {
                        foreach (var item in companyAdminList)
                        {
                            notification.BodyText = string.Format(notification.BodyText,
                                $"{ item.FirstName }{ item.LastName }",
                                viewModel.DriverInvitedDate.ToString(Resource.constFormatDate),
                                $"{viewModel.DriverFirstName }{viewModel.DriverLastName}",
                                viewModel.PONumber,
                                viewModel.StartDate.ToString(Resource.constFormatDate));

                            SendNotification(item.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InActiveDriverAssignedToOrderProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
