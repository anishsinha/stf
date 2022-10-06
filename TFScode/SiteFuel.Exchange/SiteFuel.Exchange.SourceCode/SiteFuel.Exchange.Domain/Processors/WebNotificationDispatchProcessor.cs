using System.Collections.Generic;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.WebNotification;

namespace SiteFuel.Exchange.Domain.Processors
{
    //public class WebNotificationDispatchProcessor : IQueueMessageProcessor
    //{
    //    public QueueProcessType ProcessorName => QueueProcessType.DispatchLocation;

    //    public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
    //    {
    //        errorInfo = new List<string>();
    //        queueMessageJson = queueMessage.JsonMessage;
    //        var dispatchQueMsg = JsonConvert.DeserializeObject<NotificationDispatchLocationViewModel>(queueMessageJson);
    //        if (dispatchQueMsg == null)
    //        {
    //            errorInfo.Add("Couldn't parse WebNotification Dispatch jsonMessage. Contact Support.");
    //            throw new QueueMessageFatalException($"Couldn't parse WebNotification Dispatch jsonMessage to NotificationDispatchLocationViewModel", errorInfo);
    //        }
    //        if (queueMessage.RetryCount < 2)
    //        {
    //            var domain = ContextFactory.Current.GetDomain<WebNotificationDispatchDomain>();
    //            domain.ProcessDispatchJsonMessage(dispatchQueMsg, errorInfo);
    //            return true;
    //        }
    //        else
    //        {
    //            errorInfo.Add("WebNotification Dispatch processing reached maximum retry count");
    //            throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
    //        }
    //    }
    //}
}
