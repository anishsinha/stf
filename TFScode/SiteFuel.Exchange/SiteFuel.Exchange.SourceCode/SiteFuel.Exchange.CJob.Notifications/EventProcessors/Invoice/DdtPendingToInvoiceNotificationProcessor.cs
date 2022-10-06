using Newtonsoft.Json;
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
    public class DdtPendingToInvoiceNotificationProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.DdtPendingToInvoiceNotification;
        private string userName;

        public DdtPendingToInvoiceNotificationProcessor()
        {
        }

        public override void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            viewModel = notificationDomain.GetInvoiceNotificationDetails(notificationEventViewModel.EntityId);
            userName = ContextFactory.Current.GetDomain<HelperDomain>().GetUserNameById(notificationEventViewModel.TriggeredByUserId);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

                //Send an email to all admins of the company
                var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, notificationEventViewModel.EventType);
                if (companyAdminList.Count > 0)
                {
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier);
                    var bodyText = notification.BodyText;
                    foreach (var item in companyAdminList)
                    {
                        if (item.Id != viewModel.SupplierUser.Id)
                        {
                            notification.Subject = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                            notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName, notificationEventViewModel.JsonMessage);
                            SendNotificationForDefaultEvent(item.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DdtPendingToInvoiceNotificationProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier);
                    notification.Subject = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                    notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.SupplierUser.FirstName, notificationEventViewModel.JsonMessage);
                    SendNotification(viewModel.SupplierUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DdtPendingToInvoiceNotificationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = string.Empty;            
            if (companyType == CompanyType.Buyer)
                callbackUrl = $"~/Buyer/Invoice/Details/{viewModel.Id}";
            else if (companyType == CompanyType.Supplier)
                callbackUrl = $"~/Supplier/Invoice/Details/{viewModel.Id}";

            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationBodyText(string bodyText, string firstName, string jsonMessage)
        {
            var ddt = JsonConvert.DeserializeObject<dynamic>(jsonMessage);

            bodyText = string.Format(bodyText,
                                    firstName,
                                    ddt.DisplayInvoiceNumber,
                                    ddt.BuyerCompanyName,
                                    ddt.DefaultNotificationPeriod);
            bodyText = notificationDomain.ReplaceBodyContent(bodyText, 1);
            return bodyText;
        }

        private string GetNotificationSubjectText(string subjectText, string jsonMessage)
        {
            var ddt = JsonConvert.DeserializeObject<dynamic>(jsonMessage);

            subjectText = string.Format(subjectText, ddt.DisplayInvoiceNumber);
            return subjectText;
        }
    }
}
