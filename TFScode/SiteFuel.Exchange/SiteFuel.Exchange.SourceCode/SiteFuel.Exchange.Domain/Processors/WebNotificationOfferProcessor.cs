using System.Collections.Generic;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.WebNotification;

namespace SiteFuel.Exchange.Domain.Processors
{
    //public class WebNotificationOfferProcessor : IQueueMessageProcessor
    //{
    //    public QueueProcessType ProcessorName => QueueProcessType.OfferCreated;

    //    public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
    //    {
    //        errorInfo = new List<string>();
    //        queueMessageJson = queueMessage.JsonMessage;
    //        var offerJsonMsg = JsonConvert.DeserializeObject<NotificationOfferQueMsg>(queueMessageJson);
    //        if(offerJsonMsg == null)
    //        {
    //            errorInfo.Add("Couldn't parse WebNotification Offer jsonMessage. Contact Support.");
    //            throw new QueueMessageFatalException($"Couldn't parse WebNotification Offer jsonMessage to NotificationOfferQueMsg", errorInfo);
    //        }
    //        if (queueMessage.RetryCount < 2)
    //        {
    //            var domain = ContextFactory.Current.GetDomain<WebNotificationOfferDomain>();
    //            domain.ProcessOfferJsonMessage(offerJsonMsg, errorInfo);
    //            return true;
    //        }
    //        else
    //        {
    //            errorInfo.Add("WebNotification Offer processing reached maximum retry count");
    //            throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
    //        }
    //    }
    //}
}
