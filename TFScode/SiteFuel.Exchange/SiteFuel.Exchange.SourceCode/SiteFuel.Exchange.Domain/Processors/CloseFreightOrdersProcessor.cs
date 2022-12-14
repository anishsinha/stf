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
using SiteFuel.Exchange.ViewModels.FreightOnlyOrder;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class CloseFreightOrdersProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.CloseFreightOnlyOrder;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var bulkUploadMsg = JsonConvert.DeserializeObject<CloseFreightOnlyOrderQueueMsg>(queueMessageJson);
            if (bulkUploadMsg == null)
            {
                errorInfo.Add("Couldn't parse bulkUploadMsg. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse bulkUploadMsg to CloseFreightOnlyOrderQueueMsg", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var carrierDomain = ContextFactory.Current.GetDomain<CarrierDomain>();
                carrierDomain.ProcessCloseFreightOnlyOrderQueueMsg(bulkUploadMsg, errorInfo);
                return true;
            }
            else
            {
                errorInfo.Add("CloseFreightOnlyOrderQueueMsg processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }

    }
}
