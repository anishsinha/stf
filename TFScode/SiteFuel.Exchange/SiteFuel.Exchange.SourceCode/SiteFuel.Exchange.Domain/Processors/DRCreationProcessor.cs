using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class DRCreationProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.DRCreation;
        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var input = JsonConvert.DeserializeObject<RaiseDeliveryRequestInput>(queueMessageJson);
            if (input == null)
            {
                errorInfo.Add("Couldn't parse DR creation jsonMessage. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse DR creation jsonMessage", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var domain = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>();
                errorInfo.AddRange(Task.Run(() => domain.CreateDRFromQueueService(input)).Result);
                if (errorInfo.Count > 0)
                    return false;
                else
                    return true;
            }
            else
            {
                errorInfo.Add("Mobile DR creation processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}, QueueMessageId:" + queueMessage.MessageId, errorInfo);
            }
        }
    }
}
