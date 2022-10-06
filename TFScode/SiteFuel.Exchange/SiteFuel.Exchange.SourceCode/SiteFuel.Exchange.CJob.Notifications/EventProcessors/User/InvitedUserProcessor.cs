using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors.User
{
    public class InvitedUserProcessor : BaseUserEventProcessor, IEmailProcessor
    {
        private InvitedUserNotifyViewModel viewModel;

        public EventType EventType => EventType.InvitedUser;

        public InvitedUserProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetInvitedUser(notificationEventViewModel.EntityId);
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
                if (viewModel.User.Id > 0)
                {
                    var supplierURL = string.Empty;
                    var callbackUrl = $"~/Settings/Profile/ExternalCompanyInvitedUsers?id={viewModel.User.Id}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.InvitedUserAdded_CompanyAdmin, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType, supplierURL);
                    
                        var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.InvitedCompanyId, notificationEventViewModel.EventType);
                        var bodyText = notification.BodyText;
                        var subject = notification.Subject;
                        foreach (var item in companyAdminList)
                        {
                            notification.ApplicationTemplateId = notificationEventViewModel.ApplicationTemplateId;
                            notification.Subject = string.Format(subject, notificationEventViewModel.BrandedCompanyName);
                            notification.BodyText = string.Format(bodyText,
                                                            $"{item.FirstName} {item.LastName}",
                                                            viewModel.User.FirstName,
                                                            viewModel.User.LastName,
                                                            viewModel.User.Email
                                                            );
                            SendNotification(item.Email, notification, true);
                        }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitedUserProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
