using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class SendActivationLinkProcessor : BaseUserEventProcessor, IEmailProcessor
    {
        private InvitedUserNotificationViewModel viewModel;

        public EventType EventType => EventType.SendActivationLink;

        public SendActivationLinkProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            viewModel = notificationDomain.GetNotificationDetailsById(notificationEventViewModel.Id);
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
                    //Send an email to invited user
                    var domain = new AuthenticationDomain(notificationDomain);
                    var userViewModel = Task.Run(() => domain.GetUserByEmailAsync(viewModel.User.Email)).Result;
                    if (userViewModel.StatusCode == AuthStatus.Success)
                    {
                        var confirmationToken = Task.Run(() => domain.GenerateEmailConfirmationTokenAsync(viewModel.User.Email)).Result;
                        if (confirmationToken.Id > 0)
                        {
                            var callbackUrl = $"~/Account/ConfirmEmail?userId={confirmationToken.Id}&code={HttpUtility.UrlEncode(confirmationToken.Token)}&encoded=1";
                            var notification = notificationDomain.GetNotificationContent(EventSubType.EmailVerification, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);

                            //Send an email to invited user
                            notification.BodyText = string.Format(notification.BodyText, $"{userViewModel.FirstName} {userViewModel.LastName}");
                            SendNotificationForDefaultEvent(viewModel.User.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SendActivationLinkProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
