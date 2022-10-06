using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class OrderTankMappingProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.CreateTankOrderMappingInFreightService;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var input = JsonConvert.DeserializeObject<OrderTankMappingProcessorReqViewModel>(queueMessageJson);
            if (input == null)
            {
                errorInfo.Add("Couldn't parse order tank mapping jsonMessage. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse order tank mapping jsonMessage", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var domain = ContextFactory.Current.GetDomain<OrderDomain>();
                errorInfo.AddRange(Task.Run(() => domain.CreateOrderTankMappingInFreightService(input)).Result);
                if (errorInfo.Count > 0)
                    return false;
                else
                    return true;
            }
            else
            {
                errorInfo.Add("Order tank mapping process reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}, QueueMessageId:" + queueMessage.MessageId, errorInfo);
            }
        }
    }
}
