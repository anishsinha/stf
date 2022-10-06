using SiteFuel.Exchange.CJob.Notifications.EventProcessors;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SiteFuel.Exchange.CJob.Notifications
{
    public class ContinuousEventProcessor
    {
        readonly List<IEmailProcessor> processors = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                                     where t.GetInterfaces().Contains(typeof(IEmailProcessor))
                                                              && t.GetConstructor(Type.EmptyTypes) != null
                                                     select Activator.CreateInstance(t) as IEmailProcessor).ToList();

        public void ProcessEvent(NotificationEventViewModel notificationEvent)
        {
            var processor = processors.FirstOrDefault(x => x.EventType == notificationEvent.EventType);
            var emailProcessor = processor as BaseEventProcessor;

            if (emailProcessor != null)
            {
                emailProcessor = Activator.CreateInstance(emailProcessor.GetType()) as BaseEventProcessor;
                var notificationDomain = new NotificationDomain();
                (emailProcessor as IEmailProcessor).Initialize(notificationEvent);
                emailProcessor.NotificationEventViewModel = notificationEvent;
                emailProcessor.SendEmail(notificationDomain, notificationEvent);
                emailProcessor.UpdateNotificationStatus(notificationDomain, notificationEvent);
            }
        }
    }
}
