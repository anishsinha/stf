using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Core;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class PDIAPIProcessor: IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.PDIAPIDeliveryDetails;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var input = JsonConvert.DeserializeObject<PDIAPIRequestViewModel>(queueMessageJson);
            if (input == null)
            {
                errorInfo.Add("Couldn't parse PDI Api generation jsonMessage. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse PDI Api generation jsonMessage jsonMessage", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var domain = ContextFactory.Current.GetDomain<PDIEnterpriseDomain>();
                errorInfo.AddRange(Task.Run(() => domain.ProcessDeliveryDetailsToPDI(input)).Result);
                return true;
            }
            else
            {
                errorInfo.Add("PDI Api generation processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }
    }
}
