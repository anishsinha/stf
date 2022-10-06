using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors.Job
{
    public class JobAttributeChangeProcessor: BaseJobEventProcessor, IEmailProcessor
    {
        private NotificationJobViewModel viewModel;
        public EventType EventType => EventType.LocationAttributeChange;
        public JobAttributeChangeProcessor()
        {

        }
        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetJobAttributeChangeDetails(notificationEventViewModel.EntityId ,notificationEventViewModel.TriggeredByUserId);
        }
        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
           var notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
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
                        notification.BodyText = GetNotificationBodyDetails(notification.BodyText, item.FirstName,item.LastName, notificationEventViewModel.JsonMessage);
                        SendNotification(item.Email, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobAttributeChangeProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
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
                if (viewModel.Id > 0)
                {
                    if (viewModel.AssignedTo.Any() || viewModel.OnsitePersons.Any())
                    {
                        var notification = GetNotificationContent();
                        var bodyText = notification.BodyText;
                        foreach (var item in viewModel.AssignedTo)
                        {
                            notification.BodyText = bodyText;
                            notification.BodyText = GetNotificationBodyDetails(notification.BodyText, item.FirstName, item.LastName, notificationEventViewModel.JsonMessage);
                            SendNotification(item.Email, notification);
                        }
                    }
                }
                   
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobAttributeChangeProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
        private NotificationViewModel GetNotificationContent()
        {
            var callbackUrl = $"~/Buyer/Job/Details/{viewModel.Id}";
            var notification = notificationDomain.GetEmailNotificationContent(EventType.LocationAttributeChange, CompanyType.Buyer, _serverUrl, callbackUrl);
            return notification;
        }
        private string GetNotificationBodyDetails(string bodyText, string firstName, string lastname, string jsonMessage)
        {
            bodyText = string.Format(bodyText,
                                    firstName,
                                    lastname,
                                    viewModel.CompanyName,
                                    viewModel.Name);
            return bodyText;
        }
    }
}
