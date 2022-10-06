using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Core.Utilities;

namespace SiteFuel.Exchange.Domain
{
    public class EmailDomain : BaseDomain
    {
        public EmailDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public EmailDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<bool> SendEmail(string emailTemplate, ApplicationEventNotificationViewModel model)
        {
            NotificationLogDomain notificationLogDomain = ContextFactory.Current.GetDomain<NotificationLogDomain>();
            var appDomain = new ApplicationDomain();
            var emailSendingEnabled = appDomain.GetApplicationSettingValue<bool>(Constants.EmailSendingEnabled);
            var response = false;
            if (emailSendingEnabled)
            {
                response = await Email.GetClient().SendAsync(emailTemplate, model);

                var viewModel = new NotificationViewModel();
                viewModel.Subject = model.Subject;
                viewModel.BodyText = model.BodyText;
                viewModel.To = model.To;
                viewModel.CC = model.Cc;
                viewModel.BCC = model.Bcc;
                notificationLogDomain.SaveEmailNotificationLog(viewModel, response);
            }
            return response;
        }
    }
}
