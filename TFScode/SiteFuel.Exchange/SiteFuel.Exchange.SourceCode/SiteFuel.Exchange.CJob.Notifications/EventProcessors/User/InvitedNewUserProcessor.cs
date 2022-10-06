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

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class InvitedNewUserProcessor: BaseUserEventProcessor, IEmailProcessor
    {
        private InvitedUserNotificationViewModel viewModel;

        public EventType EventType => EventType.InvitedNewUser;

        public InvitedNewUserProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetNewInvitedUserDetails(notificationEventViewModel.EntityId);
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
                    var callbackUrl = $"~/Account/Register?supplierURL=&invitationId={viewModel.User.Id}";
                    var supplierURL = string.Empty;
                    if (!string.IsNullOrEmpty(notificationEventViewModel.JsonMessage))
                    {
                        var message = JsonConvert.DeserializeObject<AddUserMessageViewModel>(notificationEventViewModel.JsonMessage);
                        supplierURL = message.SupplierURL;
                        callbackUrl = $"~/Account/Register?supplierURL={supplierURL}&invitationId={viewModel.User.Id}";
                    }
                    var notification = notificationDomain.GetNotificationContent(EventSubType.InvitedUserAdded_NewInvitedUser, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType, supplierURL);
                    notification.ApplicationTemplateId = notificationEventViewModel.ApplicationTemplateId;
                    notification.Subject = string.Format(notification.Subject, notificationEventViewModel.BrandedCompanyName);
                    notification.BodyText = string.Format(notification.BodyText,
                                                $"{viewModel.User.FirstName} {viewModel.User.LastName}");

                    SendNotificationForDefaultEvent(viewModel.User.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitedNewUserProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
