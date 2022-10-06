using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.ThirdPartyOrder;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class BulkUploadProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.ThirdPartyOrderBulkUpload;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var bulkUploadMsg = JsonConvert.DeserializeObject<ThirdPartyBulkUploadQueueMsg>(queueMessageJson);
            if(bulkUploadMsg == null)
            {
                errorInfo.Add("Couldn't parse bulkUploadMsg. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse bulkUploadMsg to ThirdPartyBulkUploadQueueMsg", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                return ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().ProcessOrderBulkUploadJsonMessage(queueMessage.MessageId, bulkUploadMsg, errorInfo); //This whole process can be asynchronous
            }
            else
            {
                errorInfo.Add("Bulk upload processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }
    }
}
