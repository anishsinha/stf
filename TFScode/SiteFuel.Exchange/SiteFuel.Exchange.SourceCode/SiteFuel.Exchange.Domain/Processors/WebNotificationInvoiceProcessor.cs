using System.Collections.Generic;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.WebNotification;

namespace SiteFuel.Exchange.Domain.Processors
{ 
    //public class WebNotificationInvoiceProcessor : IQueueMessageProcessor
    //{
    //    public QueueProcessType ProcessorName => QueueProcessType.InvoiceCreated;

    //    public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
    //    {
    //        errorInfo = new List<string>();
    //        queueMessageJson = queueMessage.JsonMessage;
    //        var bulkUploadMsg = JsonConvert.DeserializeObject<NotificationInvoiceQueMsg>(queueMessageJson);
    //        if (bulkUploadMsg == null)
    //        {
    //            errorInfo.Add("Couldn't parse WebNotification Invoice jsonMessage. Contact Support.");
    //            throw new QueueMessageFatalException($"Couldn't parse WebNotification Invoice jsonMessage to ThirdPartyBulkUploadQueueMsg", errorInfo);
    //        }
    //        if (queueMessage.RetryCount < 2)
    //        {
    //            var domain = ContextFactory.Current.GetDomain<WebNotificationInvoiceDomain>();
    //            domain.ProcessInvoiceJsonMessage(bulkUploadMsg, errorInfo);
    //            return true;
    //        }
    //        else
    //        {
    //            errorInfo.Add("Bulk upload processing reached maximum retry count");
    //            throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
    //        }
    //    }
    //}
}
