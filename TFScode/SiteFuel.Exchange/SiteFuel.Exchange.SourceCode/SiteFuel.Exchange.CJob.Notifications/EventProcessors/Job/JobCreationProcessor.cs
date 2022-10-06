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
    public class JobCreationProcessor : BaseJobEventProcessor, IEmailProcessor
    {
        private NotificationJobViewModel viewModel;

        public EventType EventType => EventType.JobCreated;

        public JobCreationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetJobNotificationDetails(notificationEventViewModel.EntityId);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

                //Send an email to all admins of the company
                var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.CompanyId, notificationEventViewModel.EventType);
                if (companyAdminList.Count > 0)
                {
                    var notification = GetNotificationContent();
                    var bodyText = notification.BodyText;
                    foreach (var item in companyAdminList)
                    {
                        notification.BodyText = bodyText;
                        notification.BodyText = GetNotificationBodyText(notification.BodyText,item.FirstName);
                        SendNotification(item.Email, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobCreationProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
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
                    if (viewModel.AssignedTo.Any())
                    {
                        var notification = GetNotificationContent();
                        var bodyText = notification.BodyText;
                        foreach (var item in viewModel.AssignedTo)
                        {
                            notification.BodyText = bodyText;
                            notification.BodyText = GetNotificationBodyText(notification.BodyText, item.FirstName);
                            SendNotification(item.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private string GetOnsiteInfo()
        {
            return viewModel.OnsitePersons.Any() ? $"{viewModel.OnsitePersons.First().FirstName} {viewModel.OnsitePersons.First().LastName}"
                                                          : string.Empty;
        }

        private string GetApproverInfo()
        {
            return viewModel.IsApprovalWorkflowEnabled ? $"{viewModel.ApproverUser.FirstName} {viewModel.ApproverUser.LastName}"
                                                                : string.Empty;
        }

        private NotificationViewModel GetNotificationContent()
        {
            var callbackUrl = $"~/Buyer/Job/Details/{viewModel.Id}";
            var notification = notificationDomain.GetEmailNotificationContent(EventType.JobCreated, CompanyType.Buyer, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationBodyText(string bodyText, string firstName)
        {
            bodyText = string.Format(bodyText,
                                    firstName,
                                    $"{viewModel.Creator.FirstName} {viewModel.Creator.LastName}",
                                    viewModel.Name, GetOnsiteInfo(), GetApproverInfo());
            bodyText = viewModel.OnsitePersons.Any() ? notificationDomain.ReplaceBodyContent(bodyText, 1) : notificationDomain.RemoveBodyContent(bodyText, 1);
            bodyText = viewModel.IsApprovalWorkflowEnabled ? notificationDomain.ReplaceBodyContent(bodyText, 2) : notificationDomain.RemoveBodyContent(bodyText, 2);
            bodyText = viewModel.IsProFormaPoEnabled ? notificationDomain.ReplaceBodyContent(bodyText, 3) : notificationDomain.RemoveBodyContent(bodyText, 3);
            return bodyText;
        }
    }
}
